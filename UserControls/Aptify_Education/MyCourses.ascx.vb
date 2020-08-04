'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On
Imports Telerik.Web.UI
Imports System.Data
Namespace Aptify.Framework.Web.eBusiness.Education
    Partial Class MyCoursesControl
        Inherits eBusiness.BaseUserControlAdvanced

        Protected Const ATTRIBUTE_VIEW_CLASS_PAGE As String = "ViewClassPage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "MyCourses"
        Protected Const ATTRIBUTE_DATATABLE_MYCOURSES As String = "dtMyCourses"


#Region "MyCourses Specific Properties"
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

        Protected Overridable Sub OnPageLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'set control properties from XML file if needed
            SetProperties()
            If Not IsPostBack Then
                ' Changes made to get the query string name from a property set by CMS
                ' Changes made by CP 7/14/2008
                AddExpression()
                Dim sID As String = Request.QueryString(Me.QueryStringRecordIDParameter)
                If Me.IsQueryStringEncrypted Then
                    sID = Aptify.Framework.Web.Common.WebCryptography.Decrypt(sID)
                End If
                If sID IsNot Nothing Then
                    Select Case sID.ToUpper.Trim
                        Case "PAST"
                            Me.cmbType.SelectedIndex = 1
                        Case "ALL"
                            Me.cmbType.SelectedIndex = 2
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
                    Me.grdMyCourses.Enabled = False
                    Me.grdMyCourses.ToolTip = "ViewCertificationPage property has not been set."
                Else
                    '  'Navin Prasad Issue 11032
                    ' DirectCast(grdMyCourses.Columns(0), HyperLinkColumn).DataNavigateUrlFormatString = ViewClassPage & "?ClassID={0:F0}"
                End If
            End If

        End Sub

        ''' <summary>
        ''' This method loads the grid on the page, override the method functionality to alter the grid loading functionality
        ''' </summary>
        ''' <remarks></remarks>
        Protected Overridable Sub LoadGrid()
            Dim sSQL As String
            Dim dt As Data.DataTable
            Try
                If ViewState(ATTRIBUTE_DATATABLE_MYCOURSES) IsNot Nothing Then
                    grdMyCourses.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_MYCOURSES), Data.DataTable)
                    grdMyCourses.DataBind()
                    Exit Sub
                End If
                sSQL = "SELECT cr.ClassID, cr.DateRegistered, c.WebName,c.WebDescription, " & _
                       "'~/Education/ViewClass.aspx?ClassID='+convert(nvarchar(10),cr.ClassID) CourseLink, " & _
                       "cr.Status, cr.PercentScore FROM " & _
                       AptifyApplication.GetEntityBaseDatabase("Class Registrations") & _
                       "..vwClassRegistrations cr INNER JOIN " & _
                       AptifyApplication.GetEntityBaseDatabase("Classes") & _
                       "..vwClasses c ON cr.ClassID=c.ID " & _
                       "WHERE c.WebEnabled=1 AND StudentID=" & User1.PersonID
                Select Case cmbType.SelectedValue.ToUpper.Trim
                    Case "CURRENT/FUTURE COURSES"
                        sSQL &= " AND cr.Status IN('In-Progress','Registered') AND (ISNULL(cr.DateExpires,'')='' OR ISNULL(cr.DateExpires,'')>GETDATE()) "
                    Case "PAST COURSES"
                        sSQL &= " AND ( cr.Status NOT IN('Pending','In-Progress', 'Registered') OR (ISNULL(cr.DateExpires,'')<>'' AND ISNULL(cr.DateExpires,'')<=GETDATE())) "
                End Select
                sSQL &= " ORDER BY cr.Course"
                dt = DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                Dim dcolUrl As DataColumn = New DataColumn()
                dcolUrl.Caption = "ClassUrl"
                dcolUrl.ColumnName = "ClassUrl"

                dt.Columns.Add(dcolUrl)
                If dt.Rows.Count > 0 Then

                    For Each rw As DataRow In dt.Rows
                        rw("ClassUrl") = ViewClassPage + "?ClassID=" + rw("ClassID").ToString
                    Next
                End If


                Me.grdMyCourses.DataSource = dt
                grdMyCourses.DataBind()
                ViewState(ATTRIBUTE_DATATABLE_MYCOURSES) = dt
                
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub cmbType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbType.SelectedIndexChanged
            ViewState(ATTRIBUTE_DATATABLE_MYCOURSES) = Nothing
            LoadGrid()
            AddExpression()
        End Sub
        Protected Sub grdMyCourses_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdMyCourses.NeedDataSource
            If ViewState(ATTRIBUTE_DATATABLE_MYCOURSES) IsNot Nothing Then
                grdMyCourses.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_MYCOURSES), Data.DataTable)
            End If
        End Sub

        ''' <summary>
        ''' Nalini 12436 date:1/12/2011
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub grdMyCourses_PageIndexChanging(ByVal sender As Object, ByVal e As GridPageChangedEventArgs) Handles grdMyCourses.PageIndexChanged
            grdMyCourses.CurrentPageIndex = e.NewPageIndex
            If ViewState(ATTRIBUTE_DATATABLE_MYCOURSES) IsNot Nothing Then
                grdMyCourses.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_MYCOURSES), Data.DataTable)
            End If
        End Sub

        'Suraj Issue 14829 4/29/13,  Remove "ItemDataBound" Event because we remove time from grid 
        Private Sub AddExpression()
            Dim ExpOrderSort As New GridSortExpression
            ExpOrderSort.FieldName = "WebName"
            ExpOrderSort.SetSortOrder("Ascending")
            grdMyCourses.MasterTableView.SortExpressions.AddSortExpression(ExpOrderSort)
        End Sub

        Protected Sub grdMyCourses_PageSizeChanged(sender As Object, e As Telerik.Web.UI.GridPageSizeChangedEventArgs) Handles grdMyCourses.PageSizeChanged
            If ViewState(ATTRIBUTE_DATATABLE_MYCOURSES) IsNot Nothing Then
                grdMyCourses.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_MYCOURSES), Data.DataTable)
            End If
        End Sub
    End Class
End Namespace
