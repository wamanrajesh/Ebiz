'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports Aptify.Applications.Education

Namespace Aptify.Framework.Web.eBusiness.Education
    Partial Class CurriculumControl
        Inherits eBusiness.BaseUserControlAdvanced

        Protected Const ATTRIBUTE_LOGIN_REDIRECT_PAGE As String = "LoginRedirect"
        Protected Const ATTRIBUTE_COURSE_CATALOG_PAGE As String = "CourseCatalogPage"
        Protected Const ATTRIBUTE_VIEW_COURSE_PAGE As String = "ViewCoursePage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "Curriculum"

#Region "Curriculum Specific Properties"
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
        ''' <summary>
        ''' LoginRedirect page url
        ''' </summary>
        Public Overridable Property LoginRedirect() As String
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
            If Not Me.WebUserLogin1.User.UserID > 0 Then
                lblError.Text = "Login Required. Please <a href=" & LoginRedirect & ">Login Here</a>"
                lblError.Visible = True
                tblMain.Visible = False
                'require login
                'Session.Add("ReturnToPage", Page.TemplateSourceDirectory & "/Curriculum.aspx")
                'Response.Redirect(Page.TemplateSourceDirectory & "/../Login.aspx")
            ElseIf Not IsPostBack Then
                Me.LoadCategoryCombo()
                Me.LoadCurriculumCombo()
            End If
        End Sub

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(CourseCatalogPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                CourseCatalogPage = Me.GetLinkValueFromXML(ATTRIBUTE_COURSE_CATALOG_PAGE)
            End If
            If String.IsNullOrEmpty(ViewCoursePage) Then
                'since value is the 'default' check the XML file for possible custom setting
                ViewCoursePage = Me.GetLinkValueFromXML(ATTRIBUTE_VIEW_COURSE_PAGE)
            End If
            If String.IsNullOrEmpty(LoginRedirect) Then
                'since value is the 'default' check the XML file for possible custom setting
                LoginRedirect = Me.GetLinkValueFromXML(ATTRIBUTE_VIEW_COURSE_PAGE)
                If String.IsNullOrEmpty(LoginRedirect) Then
                    'when login is not provided redirect to application root
                    LoginRedirect = Request.ApplicationPath
                End If
            End If

        End Sub

        Protected Overridable Sub LoadCategoryCombo()
            Try
                'Add 'All' for first item in Category List
                cmbCategory.Items.Add(New ListItem("<All Categories>", "-1"))

                'Add all other Categories to list
                Dim sSQL As String, dt As Data.DataTable
                sSQL = "SELECT DISTINCT cdc.ID, cdc.Name " & _
                       "FROM " & AptifyApplication.GetEntityBaseDatabase("Curriculum Definitions") & _
                            ".." & AptifyApplication.GetEntityBaseView("Curriculum Definitions") & " cd " & _
                       "INNER JOIN " & AptifyApplication.GetEntityBaseDatabase("Curriculum Definition Categories") & _
                            ".." & AptifyApplication.GetEntityBaseView("Curriculum Definition Categories") & " cdc " & _
                            "ON cd.CategoryID = cdc.ID"
                dt = Me.DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                If Not dt Is Nothing Then
                    For Each dr As Data.DataRow In dt.Rows
                        cmbCategory.Items.Add(New ListItem(dr("Name").ToString, dr("ID").ToString))
                    Next
                End If

                'Check to see if a specific Category is supposed to be preselected
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
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Overridable Sub LoadCurriculumTable()
            Try
                'Issue 4895 - 2007-04-25
                Dim defaultBGColor As Drawing.Color = Me.tblCurriculum.BackColor
                Dim filterBGColor As Drawing.Color = Drawing.Color.Pink
                Dim bFilteredCourse As Boolean = False

                Dim lCurriculumID As Long
                lCurriculumID = CLng(IIf(Me.cmbCurriculum.SelectedIndex < 1, Me.cmbCurriculum.Items(0).Value, Me.cmbCurriculum.SelectedValue))
                Dim lPersonID As Long = Me.WebUserLogin1.User.PersonID

                Dim oCA As CurriculumApplication = New CurriculumApplication()
                oCA.Config(Me.AptifyApplication, lCurriculumID, lPersonID)

                Dim r As TableRow = New TableRow()
                Dim c As TableCell = New TableCell()
                Dim unitsRequired As Long = 0
                Dim unitsTaken As Long = 0
                Dim unitsInProgress As Long = 0
                Dim totalUnitsRequired As Long = 0
                Dim totalUnitsTaken As Long = 0
                Dim totalUnitsInProgress As Long = 0
                Dim totalCurriculumRequired As Long = 0
                Dim totalCurriculumTaken As Long = 0
                Dim totalCurriculumInProgress As Long = 0

                'Clear the table
                Me.tblCurriculum.Rows.Clear()

                'Add Curriculum Name as title to table
                c.Controls.Add(New LiteralControl(oCA.CurriculumName))
                c.ColumnSpan = 6
                c.HorizontalAlign = HorizontalAlign.Center
                c.Font.Bold = True
                c.Font.Size = New FontUnit(FontSize.Large)
                r.Cells.Add(c)
                c = New TableCell
                tblCurriculum.Rows.Add(r)
                r = New TableRow

                'Add Main Header to table
                c.Controls.Add(New LiteralControl("")) 'blank cell
                c.Width = New Unit(10, UnitType.Pixel)
                c.ColumnSpan = 2
                r.Cells.Add(c)
                c = New TableCell
                c.Controls.Add(New LiteralControl("Units Required"))
                c.HorizontalAlign = HorizontalAlign.Center
                r.Cells.Add(c)
                c = New TableCell
                c.Controls.Add(New LiteralControl("Completed"))
                c.HorizontalAlign = HorizontalAlign.Center
                r.Cells.Add(c)
                c = New TableCell
                c.Controls.Add(New LiteralControl("In-Progress"))
                c.HorizontalAlign = HorizontalAlign.Center
                r.Cells.Add(c)
                c = New TableCell
                c.Controls.Add(New LiteralControl("Remaining"))
                c.HorizontalAlign = HorizontalAlign.Center
                r.Cells.Add(c)
                c = New TableCell
                tblCurriculum.Rows.Add(r)
                r = New TableRow

                'Add Courses Header
                c.Controls.Add(New LiteralControl("Courses"))
                c.Font.Bold = True
                c.Font.Italic = True
                c.ColumnSpan = 6
                r.Cells.Add(c)
                c = New TableCell
                tblCurriculum.Rows.Add(r)
                r = New TableRow

                'Add Courses
                For Each oCourse As CurriculumCourseRequirement In oCA.CourseRequirements
                    'Reset unit variables
                    unitsRequired = 0
                    unitsTaken = 0
                    unitsInProgress = 0

                    Dim oFilter As New Aptify.Applications.Education.EducationManagementScopeFilter(Me.AptifyApplication)
                    If Not oFilter.EvaluateCourseFilterByCourseID(oCourse.CourseID, Me.WebUserLogin1.User.PersonID) Then
                        'This user does NOT meet the Course's Filter Rule's criteria
                        r.BackColor = filterBGColor
                        bFilteredCourse = True
                    Else
                        r.BackColor = defaultBGColor
                    End If

                    'Cell1: Blank Cell to indent Courses
                    c.Controls.Add(New LiteralControl("")) 'blank cell
                    c.Width = New Unit(10, UnitType.Pixel)
                    r.Cells.Add(c)
                    c = New TableCell

                    'Cell2: CourseName
                    Dim newCourse As LinkButton = New LinkButton()
                    newCourse.Text = oCourse.CourseName
                    If Not String.IsNullOrEmpty(ViewCoursePage) Then
                        newCourse.PostBackUrl = ViewCoursePage & "?CourseID=" & oCourse.CourseID.ToString
                    Else
                        newCourse.Enabled = False
                        newCourse.ToolTip = "ViewCoursePage property has not been set."
                    End If

                    c.Controls.Add(newCourse)
                    r.Cells.Add(c)
                    c = New TableCell

                    'Cell3: Units Required for Course
                    unitsRequired = oCourse.Units
                    totalUnitsRequired += unitsRequired
                    c.Controls.Add(New LiteralControl(unitsRequired.ToString))
                    c.HorizontalAlign = HorizontalAlign.Center
                    r.Cells.Add(c)
                    c = New TableCell

                    'Only attempt to display classes taken if User is logged in
                    If lPersonID > 0 Then
                        For Each oClass As StudentCertification In oCourse.StudentCertifications
                            'Determine the user's status for this class
                            Select Case oClass.StudentStatus
                                Case StudentCertification.RequirementStudentStatus.InProgress
                                    unitsInProgress = oCourse.Units
                                Case StudentCertification.RequirementStudentStatus.Passed
                                    unitsTaken = oCourse.Units
                            End Select
                        Next

                        'Cell4: Units Taken by User for this Course
                        c.Controls.Add(New LiteralControl(unitsTaken.ToString))
                        c.HorizontalAlign = HorizontalAlign.Center
                        r.Cells.Add(c)
                        c = New TableCell

                        'Cell5: Units In-Progress for this Course
                        c.Controls.Add(New LiteralControl(unitsInProgress.ToString))
                        c.HorizontalAlign = HorizontalAlign.Center
                        r.Cells.Add(c)
                        c = New TableCell

                        'Cell6: Units Remaining for this Course
                        c.Controls.Add(New LiteralControl((unitsRequired - (unitsTaken + unitsInProgress)).ToString))
                        c.ForeColor = Drawing.Color.Red
                        c.HorizontalAlign = HorizontalAlign.Center
                        r.Cells.Add(c)
                        c = New TableCell

                        tblCurriculum.Rows.Add(r)
                        r = New TableRow
                    End If

                    totalUnitsTaken += unitsTaken
                    totalUnitsInProgress += unitsInProgress
                Next

                'Add Courses Sub-Total Row
                c.Controls.Add(New LiteralControl("")) 'blank cell
                c.Width = New Unit(10, UnitType.Pixel)
                r.Cells.Add(c)
                c = New TableCell
                c.Controls.Add(New LiteralControl("Total Courses"))
                c.Font.Italic = True
                r.Cells.Add(c)
                c = New TableCell
                c.Controls.Add(New LiteralControl(totalUnitsRequired.ToString))
                c.HorizontalAlign = HorizontalAlign.Center
                r.Cells.Add(c)
                c = New TableCell
                c.Controls.Add(New LiteralControl(totalUnitsTaken.ToString))
                c.HorizontalAlign = HorizontalAlign.Center
                r.Cells.Add(c)
                c = New TableCell
                c.Controls.Add(New LiteralControl(totalUnitsInProgress.ToString))
                c.HorizontalAlign = HorizontalAlign.Center
                r.Cells.Add(c)
                c = New TableCell
                c.Controls.Add(New LiteralControl((totalUnitsRequired - (totalUnitsTaken + totalUnitsInProgress)).ToString))
                c.ForeColor = Drawing.Color.Red
                c.HorizontalAlign = HorizontalAlign.Center
                r.Cells.Add(c)
                c = New TableCell
                r.Font.Italic = True
                tblCurriculum.Rows.Add(r)
                r = New TableRow

                totalCurriculumRequired = totalUnitsRequired
                totalCurriculumTaken = totalUnitsTaken
                totalCurriculumInProgress = totalUnitsInProgress

                'Reset unit totals
                totalUnitsRequired = 0
                totalUnitsTaken = 0
                totalUnitsInProgress = 0

                ' Changes made to correct Issue 5290 on 11-26-2007
                ' Changes made by Vijay Sitlani

                'Add Categories Header
                c.Controls.Add(New LiteralControl("Categories"))
                c.Font.Bold = True
                c.Font.Italic = True
                c.ColumnSpan = 6
                r.Cells.Add(c)
                c = New TableCell
                tblCurriculum.Rows.Add(r)
                r = New TableRow

                For Each oCat As CurriculumCategoryRequirement In oCA.CategoryRequirements
                    'Reset unit variables
                    unitsRequired = 0
                    unitsTaken = 0
                    unitsInProgress = 0

                    'Cell1: Blank Cell to indent Categories
                    c.Controls.Add(New LiteralControl("")) 'blank cell
                    c.Width = New Unit(10, UnitType.Pixel)
                    r.Cells.Add(c)
                    c = New TableCell

                    'Cell2: CategoryName
                    Dim newCategory As LinkButton = New LinkButton()
                    newCategory.Text = oCat.CourseCategoryName.ToString
                    If Not String.IsNullOrEmpty(CourseCatalogPage) Then
                        newCategory.PostBackUrl = CourseCatalogPage & "?CategoryID=" & oCat.CourseCategoryID.ToString
                    Else
                        newCategory.Enabled = False
                        newCategory.ToolTip = "CourseCatalogPage property has not been set."
                    End If

                    c.Controls.Add(newCategory)
                    r.Cells.Add(c)
                    c = New TableCell

                    'Cell3: Units Required for Category
                    unitsRequired = oCat.Units
                    totalUnitsRequired += unitsRequired
                    c.Controls.Add(New LiteralControl(unitsRequired.ToString))
                    c.HorizontalAlign = HorizontalAlign.Center
                    r.Cells.Add(c)
                    c = New TableCell

                    'Only attempt to display classes taken if User is logged in
                    If lPersonID > 0 Then
                        For Each oClass As StudentCertification In oCat.StudentCertifications
                            'Determine the user's status for this class
                            Select Case oClass.StudentStatus
                                Case StudentCertification.RequirementStudentStatus.InProgress
                                    unitsInProgress += oClass.Units
                                Case StudentCertification.RequirementStudentStatus.Passed
                                    unitsTaken += oClass.Units
                            End Select
                        Next

                        'Cell4: Units Taken by User for this Course
                        c.Controls.Add(New LiteralControl(unitsTaken.ToString))
                        c.HorizontalAlign = HorizontalAlign.Center
                        r.Cells.Add(c)
                        c = New TableCell

                        'Cell5: Units In-Progress for this Course
                        c.Controls.Add(New LiteralControl(unitsInProgress.ToString))
                        c.HorizontalAlign = HorizontalAlign.Center
                        r.Cells.Add(c)
                        c = New TableCell

                        'Cell6: Units Remaining for this Course
                        c.Controls.Add(New LiteralControl((unitsRequired - (unitsTaken + unitsInProgress)).ToString))
                        c.HorizontalAlign = HorizontalAlign.Center
                        c.ForeColor = Drawing.Color.Red
                        r.Cells.Add(c)
                        c = New TableCell

                        tblCurriculum.Rows.Add(r)
                        r = New TableRow
                    End If

                    totalUnitsTaken += unitsTaken
                    totalUnitsInProgress += unitsInProgress
                Next

                totalCurriculumRequired += totalUnitsRequired
                totalCurriculumTaken += totalUnitsTaken
                totalCurriculumInProgress += totalUnitsInProgress

                'Add Categories Sub-Total Row
                c.Controls.Add(New LiteralControl("")) 'blank cell
                c.Width = New Unit(10, UnitType.Pixel)
                r.Cells.Add(c)
                c = New TableCell
                c.Controls.Add(New LiteralControl("Total Categories"))
                c.Font.Italic = True
                r.Cells.Add(c)
                c = New TableCell
                c.Controls.Add(New LiteralControl(totalUnitsRequired.ToString))
                c.HorizontalAlign = HorizontalAlign.Center
                r.Cells.Add(c)
                c = New TableCell
                c.Controls.Add(New LiteralControl(totalUnitsTaken.ToString))
                c.HorizontalAlign = HorizontalAlign.Center
                r.Cells.Add(c)
                c = New TableCell
                c.Controls.Add(New LiteralControl(totalUnitsInProgress.ToString))
                c.HorizontalAlign = HorizontalAlign.Center
                r.Cells.Add(c)
                c = New TableCell
                c.Controls.Add(New LiteralControl((totalUnitsRequired - (totalUnitsTaken + totalUnitsInProgress)).ToString))
                c.ForeColor = Drawing.Color.Red
                c.HorizontalAlign = HorizontalAlign.Center
                r.Cells.Add(c)
                c = New TableCell
                r.Font.Italic = True
                tblCurriculum.Rows.Add(r)
                r = New TableRow

                'Add Curriculum Total Row
                c.Controls.Add(New LiteralControl("Curriculum Total"))
                c.ColumnSpan = 2
                c.Font.Italic = True
                r.Cells.Add(c)
                c = New TableCell
                c.Controls.Add(New LiteralControl(totalCurriculumRequired.ToString))
                c.HorizontalAlign = HorizontalAlign.Center
                r.Cells.Add(c)
                c = New TableCell
                c.Controls.Add(New LiteralControl(totalCurriculumTaken.ToString))
                c.HorizontalAlign = HorizontalAlign.Center
                r.Cells.Add(c)
                c = New TableCell
                c.Controls.Add(New LiteralControl(totalCurriculumInProgress.ToString))
                c.HorizontalAlign = HorizontalAlign.Center
                r.Cells.Add(c)
                c = New TableCell
                c.Controls.Add(New LiteralControl((totalCurriculumRequired - (totalCurriculumTaken + totalCurriculumInProgress)).ToString))
                c.ForeColor = Drawing.Color.Red
                c.HorizontalAlign = HorizontalAlign.Center
                r.Cells.Add(c)
                r.Font.Bold = True
                c = New TableCell
                r.Font.Italic = True
                tblCurriculum.Rows.Add(r)
                r = New TableRow

                If bFilteredCourse Then
                    'blank row
                    c.Controls.Add(New LiteralControl(""))
                    c.ColumnSpan = 6
                    r.Cells.Add(c)
                    c = New TableCell
                    tblCurriculum.Rows.Add(r)
                    r = New TableRow

                    'Add explanation of why some Course rows are highlighted
                    c.Controls.Add(New LiteralControl("Highlighted Courses are Private Courses that are unavailble for Registration. Contact Registration for more information."))
                    c.ColumnSpan = 6
                    c.RowSpan = 2
                    c.Font.Italic = True
                    r.Cells.Add(c)
                    c = New TableCell
                    r.BackColor = filterBGColor
                    tblCurriculum.Rows.Add(r)
                    r = New TableRow
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

         Protected Overridable Sub LoadCurriculumCombo()
            Try
                'Issue 4895 - 2007-04-25
                'Add all other Categories to list
                Dim sSQL As String, dt As Data.DataTable
                cmbCurriculum.Items.Clear()

                sSQL = "SELECT cd.ID, cd.Name, cd.ApplyFilterRule, ISNULL(cd.ScopeFilterRuleID,-1) 'ScopeFilterRuleID' " & _
                         "FROM " & AptifyApplication.GetEntityBaseDatabase("Curriculum Definitions") & _
                              ".." & AptifyApplication.GetEntityBaseView("Curriculum Definitions") & " cd " & _
                         "WHERE cd.Status = 'Active'"

                If IsNumeric(ViewState("CategoryID")) AndAlso CLng(ViewState("CategoryID")) > 0 Then
                    sSQL &= "AND cd.CategoryID=" & ViewState("CategoryID").ToString
                ElseIf CLng(cmbCategory.SelectedValue) > 0 Then
                    sSQL &= "AND cd.CategoryID=" & cmbCategory.SelectedValue
                End If

                sSQL &= " ORDER BY cd.Category, cd.Name"
                dt = Me.DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                If Not dt Is Nothing Then
                    For Each dr As Data.DataRow In dt.Rows
                        If CBool(dr("ApplyFilterRule")) Then
                            If CLng(dr("ScopeFilterRuleID")) > 0 Then
                                'declare a ScopeFilter object to use
                                Dim oFilter As New Aptify.Applications.Education.EducationManagementScopeFilter(Me.AptifyApplication)
                                If oFilter.EvaluateCourseFilter(CLng(dr("ScopeFilterRuleID")), Me.WebUserLogin1.User.PersonID) Then
                                    'User meets criteria for this Curriculum Definition, so add it to list
                                    cmbCurriculum.Items.Add(New ListItem(dr("Name").ToString, dr("ID").ToString))
                                Else
                                    'User does NOT meed the criteria. This item will NOT be added to the list.
                                End If
                            Else
                                'No ScopeFilterRuleID so add this item
                                cmbCurriculum.Items.Add(New ListItem(dr("Name").ToString, dr("ID").ToString))
                            End If
                        Else
                            'No FilterRule applied so add this item
                            cmbCurriculum.Items.Add(New ListItem(dr("Name").ToString, dr("ID").ToString))
                        End If
                    Next
                End If

                'There is a chance that this combobox is empty
                If Me.cmbCurriculum.Items.Count < 1 Then
                    'No Curriculum Definitions Available for this category
                    Me.lblError.Text = "No Curriculum Definitions available for the selected Category. Select another Category."
                    Me.lblError.Visible = True
                    Me.btnLoadCurriculum.Enabled = False
                    Me.cmbCurriculum.Width = New System.Web.UI.WebControls.Unit(125, UnitType.Pixel) 'set a default width of the combobox
                Else
                    Me.lblError.Text = ""
                    Me.lblError.Visible = False
                    Me.btnLoadCurriculum.Enabled = True
                    Me.cmbCurriculum.Width = New System.Web.UI.WebControls.Unit()

                    If IsNumeric(ViewState("CurriculumID")) Then
                        Dim oItem As ListItem
                        oItem = cmbCurriculum.Items.FindByValue(CStr(ViewState("CurriculumID")))
                        If oItem IsNot Nothing Then
                            oItem.Selected = True
                        End If
                    Else
                        cmbCurriculum.SelectedIndex = 0
                    End If
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub cmbCategory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCategory.SelectedIndexChanged
            LoadCurriculumCombo()
        End Sub

        Protected Sub btnLoadCurriculum_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLoadCurriculum.Click
            LoadCurriculumTable()
        End Sub
    End Class
End Namespace
