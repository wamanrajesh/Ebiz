'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On
Imports System.Data
Imports Telerik.Web.UI
Namespace Aptify.Framework.Web.eBusiness.Education
    Partial Class InstructorClassesControl
        Inherits eBusiness.BaseUserControlAdvanced

        Protected Const ATTRIBUTE_VIEW_CLASS_PAGE As String = "ViewClassPage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "InstructorClasses"
        Protected Const ATTRIBUTE_DATATABLE_COURSE As String = "dtCourse"

#Region "InstructorClasses Specific Properties"
        Dim GridDataItem As GridItem

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
                ' ...Query String Name was "Type"
                ' Changes made to get the query string name from a property set by CMS
                ' Changes made by CP 7/14/2008
                Dim sID As String = Request.QueryString(Me.QueryStringRecordIDParameter)
                If Me.IsQueryStringEncrypted Then
                    sID = Aptify.Framework.Web.Common.WebCryptography.Decrypt(sID)
                End If
                If sID IsNot Nothing Then
                    Select Case sID.ToUpper.Trim
                        Case "PAST"
                            Me.cmbType.SelectedIndex = 2
                        Case "ALL"
                            Me.cmbType.SelectedIndex = 3
                        Case "FUTURE"
                            Me.cmbType.SelectedIndex = 1
                        Case Else
                            Me.cmbType.SelectedIndex = 0
                    End Select
                End If
                Me.LoadGrid()
            End If
        End Sub

        Protected Overrides Sub SetProperties()
            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(ViewClassPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                ViewClassPage = Me.GetLinkValueFromXML(ATTRIBUTE_VIEW_CLASS_PAGE)
                If String.IsNullOrEmpty(ViewClassPage) Then
                    Me.grdMyClasses.Enabled = False
                    Me.grdMyClasses.ToolTip = "ViewClassPage property has not been set."
                End If
            End If

        End Sub
        ''' <summary>
        ''' This method loads the grid on the page, override the method functionality to alter the grid loading functionality
        ''' </summary>
        ''' <remarks></remarks>
        '''  'Suraj Issue 14452 , 5/10/13, we are passing the parameter to this methode because when we performing the sorting and filtering option on grid wwe cant use the  grdMyClasses.DataBind() for the grid
        Protected Overridable Sub LoadGrid()
            Dim sSQL As String
            Dim dt As Data.DataTable
            Try
                If ViewState(ATTRIBUTE_DATATABLE_COURSE) IsNot Nothing Then
                    grdMyClasses.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_COURSE), Data.DataTable)
                    grdMyClasses.DataBind()
                    Exit Sub
                End If
                'this code for issue 13280 sachin 
                sSQL = "SELECT ID, WebName,StartDate,EndDate,Type FROM vwClasses c " & _
                        "WHERE InstructorID=" & InstructorValidator1.User.PersonID
                Select Case cmbType.SelectedValue.ToUpper.Trim
                    Case "CURRENT CLASSES"

                        sSQL &= " AND ((convert(varchar(20),c.EndDate,101) = '01/01/1900') or (convert(date,c.StartDate) <= convert(date,GETDATE()) AND convert(date,c.EndDate) >=convert(date,GETDATE()))) "
                    Case "FUTURE CLASSES"
                        sSQL &= " AND convert(date,c.StartDate) > convert(date,GETDATE())"
                    Case "PAST CLASSES"
                        sSQL &= " AND (convert(varchar(20),c.EndDate,101) <> '01/01/1900') and (convert(date,c.EndDate) < convert(date,GETDATE())) "
                End Select
                '11/23/2012 Added by Dipali for issue 6413 
                sSQL &= " AND c.Status IN ('Approved','Completed') and c.WebEnabled = 1 "
                '11/28/07 Added by Tamasa for issue 5132 
                sSQL &= " and exists (Select Id from " + _
                AptifyApplication.GetEntityBaseDatabase("Courses").ToString + ".." + AptifyApplication.GetEntityBaseView("Courses").ToString
                sSQL &= " Where Status='Available' and WebEnabled = 1 and Id = c.CourseID) "
                ' End
                sSQL &= " ORDER BY c.Course,c.StartDate DESC"
                dt = DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                'Modified by Dipali Story No:13280
                Dim dcolUrl As DataColumn = New DataColumn()
                dcolUrl.Caption = "ClassUrl"
                dcolUrl.ColumnName = "ClassUrl"
                dt.Columns.Add(dcolUrl)
                If dt.Rows.Count > 0 Then
                    If dt.Rows.Count > 0 Then
                        For Each rw As DataRow In dt.Rows
                            rw("ClassUrl") = ViewClassPage + "?ClassID=" + rw("ID").ToString()
                        Next
                    End If
                    'Navin Prasad Issue 11032
                    ' DirectCast(grdMyClasses.Columns(0), HyperLinkColumn).DataNavigateUrlFormatString = ViewClassPage & "?ClassID={0:F0}"
                    'Suraj Issue 14452 , 5/10/13, Add   grdMyClasses.DataBind() method for binding proper record  , also we are checking the is this sorting or filtering operation of grid if this is then the vale of isCheckSorting is true 
                    Me.grdMyClasses.DataSource = dt
                    ViewState(ATTRIBUTE_DATATABLE_COURSE) = dt
                    grdMyClasses.DataBind()
                Else
                    Me.grdMyClasses.DataSource = dt
                    grdMyClasses.DataBind()
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Protected Sub grdMyClasses_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdMyClasses.NeedDataSource
            'Suraj Issue 14452 , 5/10/13, this event fire for soting and filtering so, we are passing the parameter as true 
            If ViewState(ATTRIBUTE_DATATABLE_COURSE) IsNot Nothing Then
                grdMyClasses.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_COURSE), Data.DataTable)
            End If
        End Sub
     
        Private Sub CheckInstructor()
            ' determine if the individual logged in is an active instructor
            ' on any course. If so, show the InstructorCenter link.
            If Not InstructorValidator1.IsCurrentUserInstructor() Then
                Me.tblMain.Visible = False
            End If
        End Sub
        Protected Sub grdMyClasses_PageIndexChanging(ByVal sender As Object, ByVal e As GridPageChangedEventArgs) Handles grdMyClasses.PageIndexChanged
            grdMyClasses.CurrentPageIndex = e.NewPageIndex
            If ViewState(ATTRIBUTE_DATATABLE_COURSE) IsNot Nothing Then
                grdMyClasses.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_COURSE), Data.DataTable)
            End If
        End Sub
        Protected Sub grdMyClasses_PageSizeChanging(ByVal sender As Object, ByVal e As GridPageSizeChangedEventArgs) Handles grdMyClasses.PageSizeChanged
            If ViewState(ATTRIBUTE_DATATABLE_COURSE) IsNot Nothing Then
                grdMyClasses.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_COURSE), Data.DataTable)
            End If
        End Sub
        'Neha Changes for Issue 14452 for Date format
        Protected Sub grdMyClasses_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdMyClasses.ItemDataBound
            Try
                Dim dateColumns As New List(Of String)
                'Add datecolumn uniqueName in list for Date format
                dateColumns.Add("GridDateTimeColumnEndDate")
                dateColumns.Add("GridDateTimeColumnStartDate")
                CommonMethods.FormatedDateOnGrid(dateColumns, e.Item)
                'Suraj Issue  14452 ,5/3/13  ,we provide a tool tip for DatePopupButton as well as the GridDateTimeColumnEndDate textbox   
                If TypeOf e.Item Is GridFilteringItem Then
                    Dim filterItem As GridFilteringItem = DirectCast(e.Item, GridFilteringItem)
                    Dim GridDateTimeColumnEndDate As RadDatePicker = DirectCast(filterItem("GridDateTimeColumnEndDate").Controls(0), RadDatePicker)
                    GridDateTimeColumnEndDate.ToolTip = "Enter a filter date"
                    GridDateTimeColumnEndDate.DatePopupButton.ToolTip = "Select a filter date"
                    Dim gridDateTimeColumnStartDate As RadDatePicker = DirectCast(filterItem("GridDateTimeColumnStartDate").Controls(0), RadDatePicker)
                    gridDateTimeColumnStartDate.ToolTip = "Enter a filter date"
                    gridDateTimeColumnStartDate.DatePopupButton.ToolTip = "Select a filter date"
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub AddExpression()
            Dim ExpOrderSort As New GridSortExpression
            ExpOrderSort.FieldName = "WebName"
            ExpOrderSort.SetSortOrder("Ascending")
            grdMyClasses.MasterTableView.SortExpressions.AddSortExpression(ExpOrderSort)
        End Sub
        'Suraj Issue 14452 , 5/10/13, delete old one  cmbType_SelectedIndexChanged   and create new one event and replace the loadgrid methode instead of rebind . 
        Protected Sub cmbType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbType.SelectedIndexChanged
            ViewState(ATTRIBUTE_DATATABLE_COURSE) = Nothing
            LoadGrid()
        End Sub
    End Class
End Namespace
