'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On
Imports Telerik.Web.UI
Imports System.Data
Namespace Aptify.Framework.Web.eBusiness.Education
    Partial Class InstructorStudentsControl
        Inherits eBusiness.BaseUserControlAdvanced

        Protected Const ATTRIBUTE_VIEW_CLASS_PAGE As String = "ViewClassPage"
        Protected Const ATTRIBUTE_DATATABLE_STUDENT As String = "dtStudent"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "InstructorStudents"

#Region "InstructorStudents Specific Properties"
        ''' <summary>
        ''' ViewClass page url
        ''' </summary>
        Public Overridable Property ViewClassPage() As String
            Get
                If Not ViewState(ATTRIBUTE_VIEW_CLASS_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_VIEW_CLASS_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_VIEW_CLASS_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
#End Region

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'set control properties from XML file if needed
            SetProperties()
            If Not IsPostBack Then
                AddExpression()
                CheckInstructor()
                ' Suraj S Issue  14452 ,4/30/13 , remove grdStudents.DataSource = String.Empty because when the grid load first time grid display no records
                'Suraj Issue 14452,5/3/13 remove the Course dropdown 
                LoadStudents()
            End If

        End Sub

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(ViewClassPage) Then
                ViewClassPage = Me.GetLinkValueFromXML(ATTRIBUTE_VIEW_CLASS_PAGE)
                If String.IsNullOrEmpty(ViewClassPage) Then
                    Me.grdStudents.Columns.RemoveAt(0)
                    grdStudents.Columns.Insert(0, New GridBoundColumn())
                    With DirectCast(grdStudents.Columns(0), GridBoundColumn)
                        .DataField = "Course"
                        .HeaderText = "Course"
                        .ItemStyle.ForeColor = Drawing.Color.Blue
                        .ItemStyle.Font.Underline = True
                    End With
                    Me.grdStudents.ToolTip = "ViewClassPage property has not been set."
                Else
                    'Navin Prasad  Issue 11032
                    ' DirectCast(grdStudents.Columns(0), HyperLinkColumn).DataNavigateUrlFormatString = ViewClassPage & "?ClassID={0:F0}"
                End If
            End If
        End Sub

        Private Sub CheckInstructor()
            ' determine if the individual logged in is an active instructor
            ' on any course. If so, show the InstructorCenter link.
            If Not InstructorValidator1.IsCurrentUserInstructor() Then
                Me.tblMain.Visible = False
            End If
        End Sub

        Private Sub cmbDate_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbDate.SelectedIndexChanged
            ViewState(ATTRIBUTE_DATATABLE_STUDENT) = Nothing
            LoadStudents()
        End Sub

        Protected Sub grdStudents_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdStudents.NeedDataSource
            If ViewState(ATTRIBUTE_DATATABLE_STUDENT) IsNot Nothing Then
                grdStudents.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_STUDENT), Data.DataTable)
            End If
        End Sub
        'Suraj Issue 14452,5/3/13 remove the LoadCourses methode which is used for binding the Course dropdown
        Private Sub LoadStudents()
            Dim sSQL As String, dt As Data.DataTable, sDB As String = AptifyApplication.GetEntityBaseDatabase("Course Parts")
            Try
                If ViewState(ATTRIBUTE_DATATABLE_STUDENT) IsNot Nothing Then
                    grdStudents.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_STUDENT), Data.DataTable)
                    grdStudents.DataBind()
                    Exit Sub
                End If
                sSQL = " SELECT c.ID ClassID,c.Course,c.StartDate,p.LastName,p.FirstName,p.FirstLast,p.Email1,cr.Status,cr.Score " & _
                       " FROM  " & _
                       sDB & "..vwClassRegistrations cr INNER JOIN " & sDB & _
                       "..vwPersons p ON cr.StudentID=p.ID INNER JOIN " & sDB & _
                       "..vwClasses c ON cr.ClassID=c.ID WHERE c.InstructorID=" & Me.InstructorValidator1.User.PersonID
                'Suraj Issue 14452,5/3/13 remove the Course dropdown so we remove the Course " c.CourseID" condition from query
                If Len(cmbDate.SelectedValue) > 0 Then
                    If IsNumeric(cmbDate.SelectedValue) Then
                        ' specific range
                        Dim lValue As Long = CLng(cmbDate.SelectedValue)
                        If lValue > 0 Then
                            sSQL &= " AND c.StartDate >= GETDATE() AND c.StartDate <=DateAdd(mm," & lValue & ",GETDATE()) "
                        Else
                            sSQL &= " AND c.EndDate <= GETDATE() AND c.EndDate >=DateAdd(mm," & lValue & ",GETDATE()) "
                        End If
                    ElseIf cmbDate.SelectedValue = "*" Then
                        ' all past
                        sSQL &= " AND c.StartDate<=GETDATE() AND c.EndDate >= GETDATE()"
                    ElseIf cmbDate.SelectedValue = "-" Then
                        ' all past
                        sSQL &= " AND c.EndDate < GETDATE()"
                    Else
                        ' all future
                        sSQL &= " AND c.StartDate > GETDATE()"
                    End If
                End If
                '11/23/2012 Added by Dipali for issue 6413 
                sSQL &= " AND c.Status IN ('Approved','Completed') and c.WebEnabled = 1 "
                '01/22/08 Tamasa. Bug Fixes 5132
                sSQL &= " and exists (Select Id from " & _
                sDB & "..vwCourses Where Status='Available' and WebEnabled = 1 and Id = c.CourseID)  "
                sSQL &= " ORDER BY c.Course,c.StartDate DESC,p.LastName,p.FirstName "
                'End
                dt = Me.DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)


                Dim dcolUrl As DataColumn = New DataColumn()
                dcolUrl.Caption = "ClassUrl"
                dcolUrl.ColumnName = "ClassUrl"

                dt.Columns.Add(dcolUrl)
                Dim dcolUrll As DataColumn = New DataColumn()
                dcolUrll.Caption = "MailUrl"
                dcolUrll.ColumnName = "MailUrl"

                dt.Columns.Add(dcolUrll)

                Dim dcolUrllabel As DataColumn = New DataColumn()
                dcolUrllabel.Caption = "StartDateName"
                dcolUrllabel.ColumnName = "StartDateName"

                dt.Columns.Add(dcolUrllabel)
                If dt.Rows.Count > 0 Then


                    For Each rw As DataRow In dt.Rows
                        rw("ClassUrl") = Me.ViewClassPage + "?ClassID=" + rw("ClassID").ToString()
                        rw("MailUrl") = String.Format("mailto:{0}", rw("Email1").ToString)
                        rw("StartDateName") = String.Format("{0:d}", rw("StartDate").ToString)
                    Next
                End If

                grdStudents.DataSource = dt
                grdStudents.DataBind()
                ViewState(ATTRIBUTE_DATATABLE_STUDENT) = dt

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub grdStudents_PageIndexChanging(ByVal sender As Object, ByVal e As GridPageChangedEventArgs) Handles grdStudents.PageIndexChanged
            grdStudents.CurrentPageIndex = e.NewPageIndex
            If ViewState(ATTRIBUTE_DATATABLE_STUDENT) IsNot Nothing Then
                grdStudents.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_STUDENT), Data.DataTable)
            End If
        End Sub
        Protected Sub grdStudents_PageSizeChanging(ByVal sender As Object, ByVal e As GridPageSizeChangedEventArgs) Handles grdStudents.PageSizeChanged
            If ViewState(ATTRIBUTE_DATATABLE_STUDENT) IsNot Nothing Then
                grdStudents.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_STUDENT), Data.DataTable)
            End If
        End Sub
        'Neha Chnages for issue 14452 for Date format
        Protected Sub grdStudents_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdStudents.ItemDataBound
            Dim dateColumns As New List(Of String)
            'Add datecolumn uniqueName in list for Date format
            dateColumns.Add("GridDateTimeColumnEndDate")
            CommonMethods.FormatedDateOnGrid(dateColumns, e.Item)
            'Suraj Issue  14452 ,5/3/13  ,we provide a tool tip for DatePopupButton as well as the GridDateTimeColumnEndDate textbox   
            If TypeOf e.Item Is GridFilteringItem Then
                Dim filterItem As GridFilteringItem = DirectCast(e.Item, GridFilteringItem)
                Dim gridDateTimeColumnStartDate As RadDatePicker = DirectCast(filterItem("GridDateTimeColumnEndDate").Controls(0), RadDatePicker)
                gridDateTimeColumnStartDate.ToolTip = "Enter a filter date"
                gridDateTimeColumnStartDate.DatePopupButton.ToolTip = "Select a filter date"
            End If
        End Sub

        Private Sub AddExpression()
            Dim ExpOrderSort As New GridSortExpression
            ExpOrderSort.FieldName = "Course"
            ExpOrderSort.SetSortOrder("Ascending")
            grdStudents.MasterTableView.SortExpressions.AddSortExpression(ExpOrderSort)
        End Sub
    End Class
End Namespace
