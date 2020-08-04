'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On
Imports Telerik.Web.UI
Namespace Aptify.Framework.Web.eBusiness.Education
    Partial Class InstructorAuthorizedCoursesControl
        Inherits eBusiness.BaseUserControlAdvanced

        Protected Const ATTRIBUTE_VIEW_COURSE_PAGE As String = "ViewCoursePage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "InstructorAuthorizedCourses"
        Protected Const ATTRIBUTE_DATATABLE_COURSE As String = "dtCourse"
#Region "InstructorAuthorizedCourses Specific Properties"
        ''' <summary>
        ''' ViewCourse page url
        ''' </summary>
        Public Overridable Property ViewCoursePage() As String
            Get
                If Not ViewState(ATTRIBUTE_VIEW_COURSE_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_VIEW_COURSE_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_VIEW_COURSE_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
#End Region

        Protected Overridable Sub OnPageLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
            If Not IsPostBack Then
                AddExpression()
                If InstructorValidator1.IsCurrentUserInstructor() Then
                    Me.LoadGrid()
                Else
                    lblError.Text = "Access Denied - This page only available for instructors"
                    lblError.Visible = True
                    grdCourses.Visible = False
                End If
            End If
        End Sub

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(ViewCoursePage) Then
                'since value is the 'default' check the XML file for possible custom setting
                ViewCoursePage = Me.GetLinkValueFromXML(ATTRIBUTE_VIEW_COURSE_PAGE)
                If String.IsNullOrEmpty(ViewCoursePage) Then
                    Me.grdCourses.Enabled = False
                    Me.grdCourses.ToolTip = "ViewCoursePage property has not been set."
                Else
                    DirectCast(grdCourses.Columns(0), GridHyperLinkColumn).DataNavigateUrlFormatString = ViewCoursePage & "?CourseID={0}"
                End If
            End If

        End Sub

        Protected Overridable Sub LoadGrid()
            'Amruta IssueID 13281 Column change from description to webdescription
            Try
                If ViewState(ATTRIBUTE_DATATABLE_COURSE) IsNot Nothing Then
                    grdCourses.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_COURSE), Data.DataTable)
                    grdCourses.DataBind()
                    Exit Sub
                End If
                Dim sSQL As String, dt As Data.DataTable
                sSQL = "SELECT c.ID,WebName,Category,CategoryID,WebDescription," & _
                       "Units,TotalPartDuration,ci.Status," & _
                       "'ViewCourse.aspx?CourseID='+convert(nchar(10),c.ID) CourseLink FROM " & _
                       Me.AptifyApplication.GetEntityBaseDatabase("Courses") & _
                       "..vwCourses c INNER JOIN " & _
                       Me.AptifyApplication.GetEntityBaseDatabase("Courses") & _
                       "..vwCourseInstructors ci ON c.ID=ci.CourseID " & _
                       "WHERE ci.InstructorID=" & Me.InstructorValidator1.User.PersonID & _
                       " AND c.WebEnabled=1 AND c.Status='Available' "
                sSQL &= " ORDER BY Category,WebName"
                dt = DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                'Navin Prasad Issue 11032
                grdCourses.DataSource = dt
                grdCourses.DataBind()
                ViewState(ATTRIBUTE_DATATABLE_COURSE) = dt
                DirectCast(grdCourses.Columns(0), GridHyperLinkColumn).DataNavigateUrlFormatString = ViewCoursePage & "?CourseID={0}"
                If dt.Rows.Count = 0 Then
                    dt.Rows.Add(dt.NewRow)
                    grdCourses.DataSource = dt
                    grdCourses.DataBind()
                    grdCourses.Items(0).Visible = False
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub grdCourses_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdCourses.NeedDataSource
            If ViewState(ATTRIBUTE_DATATABLE_COURSE) IsNot Nothing Then
                grdCourses.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_COURSE), Data.DataTable)
            End If
        End Sub

        Protected Sub grdCourses_PageIndexChanging(ByVal sender As Object, ByVal e As GridPageChangedEventArgs) Handles grdCourses.PageIndexChanged
            grdCourses.CurrentPageIndex = e.NewPageIndex
            If ViewState(ATTRIBUTE_DATATABLE_COURSE) IsNot Nothing Then
                grdCourses.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_COURSE), Data.DataTable)
            End If
        End Sub

        Private Sub AddExpression()
            Dim ExpOrderSort As New GridSortExpression
            ExpOrderSort.FieldName = "WebName"
            ExpOrderSort.SetSortOrder("Ascending")
            grdCourses.MasterTableView.SortExpressions.AddSortExpression(ExpOrderSort)
        End Sub

        Protected Sub grdCourses_PageSizeChanged(sender As Object, e As Telerik.Web.UI.GridPageSizeChangedEventArgs) Handles grdCourses.PageSizeChanged
            If ViewState(ATTRIBUTE_DATATABLE_COURSE) IsNot Nothing Then
                grdCourses.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_COURSE), Data.DataTable)
            End If
        End Sub
    End Class
End Namespace
