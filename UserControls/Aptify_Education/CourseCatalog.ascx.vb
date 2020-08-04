'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On
Imports Telerik.Web.UI
Imports System.Data
Namespace Aptify.Framework.Web.eBusiness.Education
    Partial Class CourseCatalogControl
        Inherits eBusiness.BaseUserControlAdvanced

        Protected Const ATTRIBUTE_COURSE_CATALOG_PAGE As String = "CourseCatalogPage"
        Protected Const ATTRIBUTE_VIEW_COURSE_PAGE As String = "ViewCoursePage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "CourseCatalog"

#Region "CourseCatalog Specific Properties"
        ''' <summary>
        ''' CourseCatalog page url
        ''' </summary>
        Public Overridable Property CourseCatalogPage() As String
            Get
                If Not ViewState(ATTRIBUTE_COURSE_CATALOG_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_COURSE_CATALOG_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_COURSE_CATALOG_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
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
                Me.LoadCategories()
                'Anil B for issue 15302 on 23/04/2013
                Me.LoadGrid()
            End If
            'Anil B for issue 15302 on 23/04/2013
            'Load grid according to categories
            If IsPostBack Then
                Dim iCategoryIndex As Integer
                If IsNumeric(cmbCategory.SelectedValue) Then
                    LoadGrid()
                End If
            End If
        End Sub

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(CourseCatalogPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                CourseCatalogPage = Me.GetLinkValueFromXML(ATTRIBUTE_COURSE_CATALOG_PAGE)
                If String.IsNullOrEmpty(CourseCatalogPage) Then
                    grdCourses.Columns.RemoveAt(0)
                    grdFilteredCourses.Columns.RemoveAt(0)
                    'Navin Prasad Issue 11032
                    grdCourses.Columns.Insert(0, New BoundField())
                    grdFilteredCourses.Columns.Insert(0, New GridBoundColumn())
                    With DirectCast(grdCourses.Columns(0), GridBoundColumn)
                        .DataField = "Category"
                        .HeaderText = "Category"
                        .ItemStyle.ForeColor = Drawing.Color.Blue
                        .ItemStyle.Font.Underline = True
                    End With
                    With DirectCast(grdFilteredCourses.Columns(0), GridBoundColumn)
                        .DataField = "Category"
                        .HeaderText = "Category"
                        .ItemStyle.ForeColor = Drawing.Color.Blue
                        .ItemStyle.Font.Underline = True
                    End With
                    grdCourses.ToolTip = "CourseCatalogPage and/or ViewCoursePage property has not been set."
                    grdFilteredCourses.ToolTip = "CourseCatalogPage and/or ViewCoursePage property has not been set."
                Else


                    '  DirectCast(grdCourses.Columns(0), HyperLinkColumn).DataNavigateUrlFormatString = Me.CourseCatalogPage & "?CategoryID={0:F0}"
                    ' DirectCast(grdFilteredCourses.Columns(0), HyperLinkColumn).DataNavigateUrlFormatString = CourseCatalogPage & "?CategoryID={0:F0}"
                End If
            End If

            If String.IsNullOrEmpty(ViewCoursePage) Then
                'since value is the 'default' check the XML file for possible custom setting
                ViewCoursePage = Me.GetLinkValueFromXML(ATTRIBUTE_VIEW_COURSE_PAGE)
                If String.IsNullOrEmpty(ViewCoursePage) Then
                    grdCourses.Columns.RemoveAt(1)
                    grdFilteredCourses.Columns.RemoveAt(1)
                    'Navin Prasad Issue 11032
                    grdCourses.Columns.Insert(1, New BoundField())
                    grdFilteredCourses.Columns.Insert(1, New GridBoundColumn())
                    With DirectCast(grdCourses.Columns(1), GridBoundColumn)
                        .DataField = "WebName"
                        .HeaderText = "Course"
                        .ItemStyle.ForeColor = Drawing.Color.Blue
                        .ItemStyle.Font.Underline = True
                    End With
                    'Navin Prasad Issue 11032
                    With DirectCast(grdFilteredCourses.Columns(1), GridBoundColumn)
                        .DataField = "WebName"
                        .HeaderText = "Course"
                        .ItemStyle.ForeColor = Drawing.Color.Blue
                        .ItemStyle.Font.Underline = True
                    End With
                    grdCourses.ToolTip = "CourseCatalogPage and/or ViewCoursePage property has not been set."
                    grdFilteredCourses.ToolTip = "CourseCatalogPage and/or ViewCoursePage property has not been set."
                Else
                    ' DirectCast(grdCourses.Columns(1), HyperLinkColumn).DataNavigateUrlFormatString = ViewCoursePage & "?CourseID={0}"
                    'DirectCast(grdFilteredCourses.Columns(1), HyperLinkColumn).DataNavigateUrlFormatString = ViewCoursePage & "?CourseID={0}"
                End If
            End If

        End Sub

        Protected Overridable Sub LoadCategories()
            Try
                'Anil B for issue 15302 on 23/04/2013
                'Cleare duplicate item from categories dropdown
                cmbCategory.Items.Clear()
                cmbCategory.Items.Add(New ListItem("<All Categories>", "-1", True))
                RecursiveLoadCategory(-1, "")
                If IsNumeric(Request.QueryString("CategoryID")) Then
                    Dim oItem As ListItem
                    oItem = cmbCategory.Items.FindByValue(Request.QueryString("CategoryID"))
                    If oItem IsNot Nothing Then
                        oItem.Selected = True
                    End If
                Else
                    cmbCategory.SelectedIndex = 0
                End If
            Catch ex As Exception
                Me.lblError.Text = "An error occurred while retrieving the Categories."
                Me.lblError.Visible = True
                Me.grdCourses.Visible = False
                Me.grdFilteredCourses.Visible = False
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Protected Overridable Sub RecursiveLoadCategory(ByVal ParentCategoryID As Long, ByVal Indent As String)
            Try
                Dim sSQL As String, dt As Data.DataTable
                sSQL = "SELECT ID,Name FROM " & _
                       AptifyApplication.GetEntityBaseDatabase("Course Categories") & _
                       "..vwCourseCategories WHERE ISNULL(ParentID,-1)=" & ParentCategoryID
                dt = Me.DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                If Not dt Is Nothing Then
                    For Each dr As Data.DataRow In dt.Rows
                        cmbCategory.Items.Add(New ListItem(Indent & dr("Name").ToString, dr("ID").ToString))
                        RecursiveLoadCategory(CLng(dr("ID")), Indent & "---")
                    Next
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Protected Overridable Sub LoadGrid()
            'Issue 4895 - 2007-04-25
            'The Grid displays all Courses that meet the criteria included in the database query created in this method.
            'Because Courses may have ScopeFilters applied to them (if using Aptify sp3 or greater)
            'the code below handles both scenarios (if on sp3+ or not) by using two different
            'queries and two different grids. This code can be simplified by removing all pre-sp2
            'specific code and the grdCourses datagrid after upgrading to sp3.
            Try
                Dim sSQL As String, dt As Data.DataTable

                'The query to use will depend on if Courses has the Filter fields (sp3 or later)
                Dim bFilterFields As Boolean = False
                Dim ApplyFilterField As Aptify.Framework.BusinessLogic.GenericEntity.AptifyDataFieldBase
                ApplyFilterField = Me.AptifyApplication.GetEntityObject("Courses", -1).Fields.Item("ApplyFilterRule")
                If ApplyFilterField IsNot Nothing Then
                    bFilterFields = True
                End If

                If bFilterFields Then
                    'sp3+ query:
                    'The Courses Entity has the FilterRule fields, so we will add them to the query
                    sSQL = "SELECT ID, WebName, Category, CategoryID, ApplyFilterRule, ISNULL(ScopeFilterRuleID,-1) 'ScopeFilterRuleID' , " & _
                           "CASE ApplyFilterRule WHEN 1 THEN 'Private' ELSE 'Public' END 'Scope', " & _
                           "WebDescription, Units, TotalPartDuration," & _
                           "'ViewCourse.aspx?CourseID='+convert(nchar(10),ID) CourseLink FROM " & _
                           Me.AptifyApplication.GetEntityBaseDatabase("Courses") & _
                           ".." & Me.AptifyApplication.GetEntityBaseView("Courses") & _
                           " WHERE WebEnabled=1 AND Status='Available' "

                Else
                    'pre-sp3 query:
                    sSQL = "SELECT ID, WebName, Category, CategoryID, " & _
                           "WebDescription, Units, TotalPartDuration," & _
                           "'ViewCourse.aspx?CourseID='+convert(nchar(10),ID) CourseLink FROM " & _
                           Me.AptifyApplication.GetEntityBaseDatabase("Courses") & _
                           ".." & Me.AptifyApplication.GetEntityBaseView("Courses") & _
                           " WHERE WebEnabled=1 AND Status='Available' "

                End If
                'Anil B for issue 15302 on 23/04/2013
                'Check set -1 as default value to category dropdown
                If Not IsNumeric(cmbCategory.SelectedValue) Then
                    cmbCategory.Items.Add(New ListItem("<All Categories>", "-1", True))
                End If
                If CLng(cmbCategory.SelectedValue) > 0 Then
                    If chkSubCat.Checked Then
                        sSQL &= " AND (CategoryID=" & cmbCategory.SelectedValue & " OR " & _
                                " dbo.fnCourseCategoryLevelsBelow(CategoryID," & cmbCategory.SelectedValue & ")>0 )"
                        'Me.grdCourses.Columns(0).Visible = True
                        If bFilterFields Then
                            Me.grdFilteredCourses.Columns(0).Visible = True
                        Else
                            Me.grdCourses.Columns(0).Visible = True
                        End If
                    Else
                        sSQL &= " AND CategoryID=" & cmbCategory.SelectedValue
                        'Me.grdCourses.Columns(0).Visible = False
                        If bFilterFields Then
                            Me.grdFilteredCourses.Columns(0).Visible = False
                        Else
                            Me.grdCourses.Columns(0).Visible = False
                        End If
                    End If
                    chkSubCat.Enabled = True
                Else
                    chkSubCat.Enabled = False
                    If bFilterFields Then
                        Me.grdFilteredCourses.Columns(0).Visible = True
                    Else
                        Me.grdCourses.Columns(0).Visible = True
                    End If
                End If

                If Me.grdCourses.Columns(0).Visible OrElse Me.grdFilteredCourses.Columns(0).Visible Then
                    sSQL &= " ORDER BY Category,WebName"
                Else
                    sSQL &= " ORDER BY WebName"
                End If
                dt = DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)

                Dim dcolUrl As DataColumn = New DataColumn()
                dcolUrl.Caption = "CategoryUrl"
                dcolUrl.ColumnName = "CategoryUrl"

                dt.Columns.Add(dcolUrl)
                Dim dcolUrll As DataColumn = New DataColumn()
                dcolUrll.Caption = "CourseUrl"
                dcolUrll.ColumnName = "CourseUrl"

                dt.Columns.Add(dcolUrll)
                If dt.Rows.Count > 0 Then

                    For Each rw As DataRow In dt.Rows
                        rw("CategoryUrl") = Me.CourseCatalogPage + "?CategoryID=" + rw("CategoryID").ToString()
                        rw("CourseUrl") = Me.ViewCoursePage + "?CourseID=" + rw("ID").ToString
                    Next
                End If

                If bFilterFields Then
                    'sp3+:
                    If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                        For Each row As Data.DataRow In dt.Rows
                            If CBool(row("ApplyFilterRule")) Then
                                'declare a ScopeFilter object to use
                                Dim oFilter As New Aptify.Applications.Education.EducationManagementScopeFilter(Me.AptifyApplication)
                                If Not oFilter.EvaluateCourseFilter(CLng(row("ScopeFilterRuleID")), Me.WebUserLogin1.User.PersonID) Then
                                    'Will deleting the row actually delete the row from the datatable?
                                    row.Delete()
                                End If
                            End If
                        Next
                    End If



                    Me.grdFilteredCourses.DataSource = dt
                    ''Me.grdFilteredCourses.DataBind()
                    Dim rowcounter As Integer = 0
                    'For Each gvRow As GridDataItem In grdFilteredCourses.Items
                    '    'Navin Prasad Issue 11032
                    '    Dim hlink As HyperLink = CType(gvRow.FindControl("lnkCategory"), HyperLink)
                    '    hlink.NavigateUrl = String.Format(Me.CourseCatalogPage & "?CategoryID={0:F0}", dt.Rows((grdFilteredCourses.CurrentPageIndex * grdFilteredCourses.PageSize) + rowcounter)(3).ToString)
                    '    hlink = CType(gvRow.FindControl("lnkWebName"), HyperLink)
                    '    hlink.NavigateUrl = String.Format(ViewCoursePage & "?CourseID={0}", dt.Rows((grdFilteredCourses.CurrentPageIndex * grdFilteredCourses.PageSize) + rowcounter)(0).ToString)
                    '    ' Dim lbl As Label = CType(gvRow.FindControl("lblTotalPartDuration"), Label)
                    '    ' lbl.Text = String.Format("{0:F0} min", dt.Rows(gvRow.RowIndex)(9).ToString)
                    '    rowcounter = rowcounter + 1
                    'Next

                    Me.grdFilteredCourses.Visible = True
                    Me.grdCourses.Visible = False
                Else
                    'pre-sp3:
                    Me.grdCourses.DataSource = dt
                    ''Me.grdCourses.DataBind()
                    Me.grdCourses.Visible = True
                    Dim rowcounter As Integer = 0
                    'For Each gvRow As GridViewRow In grdCourses.Rows
                    '    'Navin Prasad Issue 11032
                    '    Dim hlink As HyperLink = CType(gvRow.FindControl("lnkCategory"), HyperLink)
                    '    hlink.NavigateUrl = String.Format(Me.CourseCatalogPage & "?CategoryID={0:F0}", dt.Rows((grdCourses.PageIndex * grdCourses.PageSize) + rowcounter)(3).ToString)
                    '    hlink = CType(gvRow.FindControl("lnkWebName"), HyperLink)
                    '    hlink.NavigateUrl = String.Format(ViewCoursePage & "?CourseID={0}", dt.Rows((grdCourses.PageIndex * grdCourses.PageSize) + rowcounter)(0).ToString)
                    '    '  Dim lbl As Label = CType(gvRow.FindControl("lblTotalPartDuration"), Label)
                    '    ' lbl.Text = String.Format("{0:F0} min", dt.Rows(gvRow.RowIndex)(9).ToString)
                    'Next

                  
                    Me.grdFilteredCourses.Visible = False
                End If

            Catch ex As Exception
                'Error occurred while populating the Grid. Display error message and not the grid
                Me.lblError.Text = "An error occurred while retrieving the Courses. Please try again later."
                Me.lblError.Visible = True
                Me.grdCourses.Visible = False
                Me.grdFilteredCourses.Visible = False

                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub cmbCategory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCategory.SelectedIndexChanged
            ResetControls()
            ''LoadGrid()
            grdFilteredCourses.Rebind()
            grdCourses.Rebind()
            AddExpression()
        End Sub
        Protected Sub grdFilteredCourses_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdFilteredCourses.NeedDataSource
            LoadGrid()
        End Sub

        Protected Sub chkSubCat_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSubCat.CheckedChanged
            ResetControls()
            ''LoadGrid()
            grdFilteredCourses.Rebind()
            grdCourses.Rebind()
        End Sub

        Protected Sub ResetControls()
            Me.lblError.Text = ""
            Me.lblError.Visible = False
            Me.grdCourses.Visible = False
            Me.grdFilteredCourses.Visible = True
        End Sub

        Protected Sub grdCourses_PageIndexChanging(ByVal sender As Object, ByVal e As GridPageChangedEventArgs) Handles grdCourses.PageIndexChanged
            ''grdCourses.PageIndex = e.NewPageIndex
            LoadGrid()
        End Sub

        Protected Sub grdFilteredCourses_PageIndexChanging(ByVal sender As Object, ByVal e As GridPageChangedEventArgs) Handles grdFilteredCourses.PageIndexChanged
            '' grdFilteredCourses.PageIndex = e.NewPageIndex
            LoadGrid()
        End Sub
        Protected Sub grdFilteredCourses_PageIndexChanging(ByVal sender As Object, ByVal e As GridPageSizeChangedEventArgs) Handles grdFilteredCourses.PageSizeChanged
            '' grdFilteredCourses.PageIndex = e.NewPageIndex
            LoadGrid()
        End Sub
        Protected Sub grdCourses_PageIndexChanging(ByVal sender As Object, ByVal e As GridPageSizeChangedEventArgs) Handles grdCourses.PageSizeChanged
            '' grdFilteredCourses.PageIndex = e.NewPageIndex
            LoadGrid()
        End Sub

        Private Sub AddExpression()
            Dim ExpOrderSort As New GridSortExpression
            ExpOrderSort.FieldName = "WebName"
            ExpOrderSort.SetSortOrder("Ascending")
            grdCourses.MasterTableView.SortExpressions.AddSortExpression(ExpOrderSort)
            grdFilteredCourses.MasterTableView.SortExpressions.AddSortExpression(ExpOrderSort)
        End Sub
    End Class
End Namespace
