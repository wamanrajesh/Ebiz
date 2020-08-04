'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On
Imports Aptify.Framework.Web.eBusiness

Namespace Aptify.Framework.Web.eBusiness.Education
    Partial Class ClassScheduleControl
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_LOCATION_VISIBLE As String = "LocationVisible"
        Protected Const ATTRIBUTE_INSTRUCTOR_VISIBLE As String = "InstructorVisible"
        Protected Const ATTRIBUTE_COURSE_CATALOG_PAGE As String = "CourseCatalogPage"
        Protected Const ATTRIBUTE_VIEW_COURSE_PAGE As String = "ViewCoursePage"
        Protected Const ATTRIBUTE_VIEW_CLASS_PAGE As String = "ViewClassPage"
        Protected Const ATTRIBUTE_CLASS_SCHEDULE_IMAGE As String = "ClassScheduleImage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "ClassScheduleControl"

#Region "ClassScheduleControl Specific Properties"
        ''' <summary>
        ''' Controls visibility of the location column
        ''' </summary>
        Public Property LocationVisible() As Boolean
            Get
                If ViewState(ATTRIBUTE_LOCATION_VISIBLE) IsNot Nothing Then
                    Return CBool(ViewState(ATTRIBUTE_LOCATION_VISIBLE))
                Else
                    Return False
                End If
            End Get
            Set(ByVal value As Boolean)
                ViewState(ATTRIBUTE_LOCATION_VISIBLE) = value
            End Set
        End Property
        ''' <summary>
        ''' Controls visibility of the instructor column
        ''' </summary>
        Public Property InstructorVisible() As Boolean
            Get
                If ViewState(ATTRIBUTE_INSTRUCTOR_VISIBLE) IsNot Nothing Then
                    Return CBool(ViewState(ATTRIBUTE_INSTRUCTOR_VISIBLE))
                Else
                    Return False
                End If
            End Get
            Set(ByVal value As Boolean)
                ViewState(ATTRIBUTE_INSTRUCTOR_VISIBLE) = value
            End Set
        End Property
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
        ''' <summary>
        ''' ClassScheduleImage url
        ''' </summary>
        Public Overridable Property ClassScheduleImage() As String
            Get
                If Not ViewState(ATTRIBUTE_CLASS_SCHEDULE_IMAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_CLASS_SCHEDULE_IMAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_CLASS_SCHEDULE_IMAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' Sets/Gets the visible status of the category column
        ''' </summary>
        Private Property CategoryVisible() As Boolean
            Get
                Return tblSchedule.Rows(0).Cells(0).Visible
            End Get
            Set(ByVal value As Boolean)
                For Each tr As HtmlTableRow In tblSchedule.Rows
                    If tr.Cells.Count >= 2 Then
                        tr.Cells(0).Visible = value
                    End If
                Next
            End Set
        End Property
        ''' <summary>
        ''' Sets/Gets the visible status of the course column
        ''' </summary>
        Private Property CourseVisible() As Boolean
            Get
                Return True
                'Return tblSchedule.Rows(0).Cells(1).Visible
            End Get
            Set(ByVal value As Boolean)
                For Each tr As HtmlTableRow In tblSchedule.Rows
                    If tr.Cells.Count >= 2 Then
                        tr.Cells(1).Visible = value
                    End If
                Next
            End Set
        End Property
#End Region

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()

            If Not IsPostBack Then
                tblSchedule.Rows.Clear()
                Dim lCategoryID As Long = -1, lCourseID As Long = -1
                If IsNumeric(Request("CategoryID")) Then
                    lCategoryID = CLng(Request("CategoryID"))
                End If
                If IsNumeric(Request("CourseID")) Then
                    lCourseID = CLng(Request("CourseID"))
                    'If lCourseID > 0 Then
                    '    'Issue 4895 - 2007-04-25
                    '    'Validate this Course is viewable by the logged in user

                    '    'The query to use will depend on if Courses has the Filter fields (sp3 or later)
                    '    Dim bFilterFields As Boolean = False
                    '    Dim ApplyFilterField As Aptify.Framework.BusinessLogic.GenericEntity.AptifyDataFieldBase
                    '    ApplyFilterField = Me.AptifyApplication.GetEntityObject("Courses", -1).Fields.Item("ApplyFilterRule")
                    '    If ApplyFilterField IsNot Nothing Then
                    '        bFilterFields = True
                    '        Dim oFilter As New Aptify.Applications.Education.EducationManagementScopeFilter(Me.AptifyApplication)
                    '        If Not oFilter.EvaluateCourseFilterByCourseID(lCourseID, Me.User1.PersonID) Then
                    '            'This user does not meet this Course's Filter criteria
                    '            Return ""
                    '        End If
                    '    End If
                    'End If

                End If
                Me.LoadCourseSchedule(lCourseID, lCategoryID)
            End If
        End Sub

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()
            Dim sAttribute As String
            Try
                If LocationVisible = False Then
                    'since value is the 'default' check the XML file for possible custom setting
                    sAttribute = ATTRIBUTE_LOCATION_VISIBLE
                    LocationVisible = CBool(Me.GetPropertyValueFromXML(sAttribute))
                End If
                If InstructorVisible = False Then
                    'since value is the 'default' check the XML file for possible custom setting
                    sAttribute = ATTRIBUTE_INSTRUCTOR_VISIBLE
                    InstructorVisible = CBool(Me.GetPropertyValueFromXML(sAttribute))
                End If
            Catch ex As Exception
                If TypeOf ex Is InvalidCastException Then
                    Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(InvalidCastExceptionForBooleanProperties(sAttribute, ex.Message))
                Else
                    Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                End If
            End Try

            If String.IsNullOrEmpty(CourseCatalogPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                CourseCatalogPage = Me.GetLinkValueFromXML(ATTRIBUTE_COURSE_CATALOG_PAGE)
            End If
            If String.IsNullOrEmpty(ViewCoursePage) Then
                'since value is the 'default' check the XML file for possible custom setting
                ViewCoursePage = Me.GetLinkValueFromXML(ATTRIBUTE_VIEW_COURSE_PAGE)
            End If
            If String.IsNullOrEmpty(ViewClassPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                ViewClassPage = Me.GetLinkValueFromXML(ATTRIBUTE_VIEW_CLASS_PAGE)
            End If
            If String.IsNullOrEmpty(ClassScheduleImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                ClassScheduleImage = Me.GetLinkValueFromXML(ATTRIBUTE_CLASS_SCHEDULE_IMAGE)
            End If

        End Sub

        Public Overridable Sub LoadSchedule()
            LoadCourseSchedule(-1, -1)
        End Sub
        Public Overridable Sub LoadCourseSchedule(ByVal CourseID As Long, ByVal CategoryID As Long)
            Try
                Me.ViewState("CategoryID") = CategoryID
                Me.ViewState("CourseID") = CourseID

                LoadCombos()
                LoadGrid(CourseID, CategoryID)
                If Not InstructorVisible Then
                    tdInstructorHeader.Style.Add("display", "none")
                    tdInstructorCombo.Style.Add("display", "none")
                End If
                If Not LocationVisible Then
                    tdLocationHeader.Visible = False
                    tdLocationCombo.Visible = False 'Style.Add("display", "none")
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Protected Overridable Sub LoadGrid(ByVal CourseID As Long, ByVal CategoryID As Long)
            Try
                'Issue 4895 - 2007-04-25
                Dim sSQL As String = Me.GetScheduleSQL(CourseID)
                Dim dt As Data.DataTable, iMonth As Integer = -1, iYear As Integer = -1
                Dim tr As HtmlTableRow
                dt = Me.DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)

                Dim bRowAdded As Boolean = False 'just in case no classes are added to the table we will want display something
                If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                    For Each dr As Data.DataRow In dt.Rows
                        With CDate(dr("StartDate"))
                            'Create Header Row
                            If iMonth <> .Month OrElse iYear <> .Year Then
                                tr = New HtmlTableRow()
                                tr.Cells.Add(New HtmlTableCell)
                                If CourseID > 0 Then
                                    tr.Cells(0).ColSpan = 4
                                ElseIf CategoryID > 0 Then
                                    tr.Cells(0).ColSpan = 5
                                Else
                                    tr.Cells(0).ColSpan = 6
                                End If
                                tr.Cells(0).BgColor = "PaleGoldenrod"
                                tr.Cells(0).Style.Add("font-weight", "bold")
                                tr.Cells(0).Style.Add("font-style", "italic")
                                If .Year = 1900 Then
                                    'If a StartDate was not specified for this Class, 
                                    'then StartDate='19000101' and must be handled accordingly
                                    'Classes with no StartDate will be blocked as OPEN in the Table
                                    tr.Cells(0).InnerHtml = "<img src='" & ClassScheduleImage & "' alt='Calendar' border='0' align='absmiddle' /> Open"
                                Else
                                    tr.Cells(0).InnerHtml = "<img src='" & ClassScheduleImage & "' alt='Calendar' border='0' align='absmiddle' /> " & _
                                                            Format(.AddDays(0), "MMMM") & " " & .Year
                                End If
                                tblSchedule.Rows.Add(tr)
                                iMonth = .Month
                                iYear = .Year
                            End If
                        End With

                        'Add Courses for this Date
                        Dim bClassAllowed As Boolean = True
                        'Verify the logged in user meets the Course's Filter Rules for this Class
                        If CBool(dr("ApplyFilterRule")) Then
                            'declare a ScopeFilter object to use
                            Dim oFilter As New Aptify.Applications.Education.EducationManagementScopeFilter(Me.AptifyApplication)
                            If Not oFilter.EvaluateCourseFilter(CLng(dr("ScopeFilterRuleID")), Me.User1.PersonID) Then
                                bClassAllowed = False
                            End If
                        End If

                        If bClassAllowed Then
                            bRowAdded = True
                            tr = New HtmlTableRow()
                            tr.Cells.Add(New HtmlTableCell)
                            tr.Cells.Add(New HtmlTableCell)
                            tr.Cells.Add(New HtmlTableCell)
                            tr.Cells.Add(New HtmlTableCell)
                            tr.Cells.Add(New HtmlTableCell)
                            tr.Cells.Add(New HtmlTableCell)

                            'Category Column
                            Dim catalogLink As HyperLink = New HyperLink
                            With catalogLink
                                .Text = CStr(dr("Category"))
                                If Not String.IsNullOrEmpty(CourseCatalogPage) Then
                                    .NavigateUrl = CourseCatalogPage & "?CategoryID=" & dr("CategoryID").ToString
                                Else
                                    catalogLink.Enabled = False
                                    catalogLink.ToolTip = "CourseCatalogPage property has not been set."
                                End If
                            End With
                            tr.Cells(0).Controls.Add(catalogLink)
                            'tr.Cells(0).InnerHtml = "<a href=""../Education/CourseCatalog.aspx?CategoryID=" & _
                            '                        dr("CategoryID").ToString & """>" & _
                            '                        "<img src='../Images/CourseCatalogSmall.gif' alt='Image' border='0' align='absmiddle' /></a> " & _
                            '                        "<a href=""../Education/CourseCatalog.aspx?CategoryID=" & _
                            '                        dr("CategoryID").ToString & """>" & _
                            '                        CStr(dr("Category")) & "</a>"
                            tr.Cells(0).Style.Add("border-right", "solid 1px PaleGoldenrod")
                            tr.Cells(0).Style.Add("border-left", "solid 1px PaleGoldenrod")

                            'Course Column
                            Dim viewCourseLink As HyperLink = New HyperLink
                            With viewCourseLink
                                .Text = CStr(dr("Course"))
                                If Not String.IsNullOrEmpty(ViewCoursePage) Then
                                    .NavigateUrl = ViewCoursePage & "?CourseID=" & dr("CourseID").ToString
                                Else
                                    viewCourseLink.Enabled = False
                                    viewCourseLink.ToolTip = "ViewCoursePage property has not been set."
                                End If
                            End With
                            tr.Cells(1).Controls.Add(viewCourseLink)
                            'tr.Cells(1).InnerHtml = "<a href=""ViewCourse.aspx?CourseID=" & _
                            '                        dr("CourseID").ToString & """>" & _
                            '                        CStr(dr("Course")) & "</a>"
                            tr.Cells(1).Style.Add("border-right", "solid 1px PaleGoldenrod")

                            'StartDate/Class Column
                            Dim viewClassLink As HyperLink = New HyperLink
                            With viewClassLink
                                If CDate(dr("StartDate")).Year = 1900 Then
                                    'Display StartDate as OPEN since no StartDate specified
                                    .Text = "Open"
                                Else
                                    .Text = CDate(dr("StartDate")).ToShortDateString
                                End If
                                If Not String.IsNullOrEmpty(ViewClassPage) Then
                                    .NavigateUrl = ViewClassPage & "?ClassID=" & dr("ID").ToString
                                Else
                                    viewClassLink.Enabled = False
                                    viewClassLink.ToolTip = "ViewClassPage property has not been set."
                                End If
                            End With
                            tr.Cells(2).Controls.Add(viewClassLink)
                            'If CDate(dr("StartDate")).Year = 1900 Then
                            '    'Display StartDate as OPEN since no StartDate specified
                            '    tr.Cells(2).InnerHtml = "<a href=""ViewClass.aspx?ClassID=" & _
                            '                            dr("ID").ToString & """>" & _
                            '                            "Open" & _
                            '                            "</a>"
                            'Else
                            '    tr.Cells(2).InnerHtml = "<a href=""ViewClass.aspx?ClassID=" & _
                            '                            dr("ID").ToString & """>" & _
                            '                            CDate(dr("StartDate")).ToShortDateString & _
                            '                            "</a>"
                            'End If
                            tr.Cells(2).Style.Add("border-right", "solid 1px PaleGoldenrod")

                            'Type Column
                            tr.Cells(3).InnerHtml = CStr(dr("Type"))
                            tr.Cells(3).Style.Add("border-right", "solid 1px PaleGoldenrod")

                            'Location Column
                            'RashmiP, Issue 11290 Meeting/Class Integration
                            If Not IsDBNull(dr("Location")) Then
                                tr.Cells(4).InnerHtml = CStr(dr("Location"))
                                tr.Cells(4).Style.Add("border-right", "solid 1px PaleGoldenrod")
                            End If

                            If Not Me.LocationVisible Then
                                tr.Cells(4).Style.Add("display", "none")
                            End If

                            'Instructor Column
                            'RashmiP, Issue 11290 Meeting/Class Integration
                            If Not IsDBNull(dr("Instructor")) Then
                                tr.Cells(5).InnerHtml = CStr(dr("Instructor"))
                                tr.Cells(5).Style.Add("border-right", "solid 1px PaleGoldenrod")
                            End If

                            If Not Me.InstructorVisible Then
                                tr.Cells(5).Style.Add("display", "none")
                            End If
                            tblSchedule.Rows.Add(tr)
                        End If
                    Next
                Else
                    tr = New HtmlTableRow()
                    tr.Cells.Add(New HtmlTableCell)
                    If CourseID > 0 Then
                        tr.Cells(0).ColSpan = 4
                    ElseIf CategoryID > 0 Then
                        tr.Cells(0).ColSpan = 5
                    Else
                        tr.Cells(0).ColSpan = 6
                    End If
                    tr.Cells(0).BgColor = "PaleGoldenrod"
                    tr.Cells(0).Style.Add("font-weight", "bold")
                    tr.Cells(0).Style.Add("font-style", "italic")
                    tr.Cells(0).InnerText = "No Classes Found"
                    tblSchedule.Rows.Add(tr)
                    bRowAdded = True
                End If

                If Not bRowAdded Then
                    'Display a message stating no Classes found since all were blocked because of Filter
                    tr = New HtmlTableRow()
                    tr.Cells.Add(New HtmlTableCell)
                    If CourseID > 0 Then
                        tr.Cells(0).ColSpan = 4
                    ElseIf CategoryID > 0 Then
                        tr.Cells(0).ColSpan = 5
                    Else
                        tr.Cells(0).ColSpan = 6
                    End If
                    tr.Cells(0).BgColor = "PaleGoldenrod"
                    tr.Cells(0).Style.Add("font-weight", "bold")
                    tr.Cells(0).Style.Add("font-style", "italic")
                    tr.Cells(0).InnerText = "No Classes Available for the Selected Course."
                    tblSchedule.Rows.Add(tr)
                End If

                If CourseID > 0 Then
                    Me.CourseVisible = False
                    Me.CategoryVisible = False
                ElseIf CategoryID > 0 Then
                    Me.CategoryVisible = False
                    Me.CourseVisible = True
                Else
                    Me.CourseVisible = True
                    Me.CategoryVisible = True
                End If
            Catch ex As Exception
                Dim tr As HtmlTableRow = New HtmlTableRow()
                tr.Cells.Add(New HtmlTableCell)
                If CourseID > 0 Then
                    tr.Cells(0).ColSpan = 4
                ElseIf CategoryID > 0 Then
                    tr.Cells(0).ColSpan = 5
                Else
                    tr.Cells(0).ColSpan = 6
                End If
                tr.Cells(0).Style.Add("font-weight", "bold")
                tr.Cells(0).Style.Add("font-style", "italic")
                tr.Cells(0).Style.Add("color", "red")
                tr.Cells(0).InnerText = "An Error occurred loading the Classes."
                tblSchedule.Rows.Clear()
                tblSchedule.Rows.Add(tr)
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Overridable Function GetScheduleSQL(ByVal CourseID As Long) As String
            Try
                Dim sSQL As String

                'CASE
                'WHEN condition THEN trueresult
                '  [...n]
                '[ELSE elseresult]
                'End
                '
                'Class.EndDate Is Null (if Nulls are allowed) or Class.EndDate = 19000101 or class.EndDate >=GetDate() 
                '
                ''Nalini issue:13277
                sSQL = "SELECT c.ID,cr.Name Course,c.CourseID,cr.Category,cr.CategoryID, " & _
                           "cr.ApplyFilterRule, ISNULL(cr.ScopeFilterRuleID,-1) 'ScopeFilterRuleID', " & _
                           "c.Type,c.School Location,p.LastName +', ' + p.FirstName Instructor, " & _
                           "CASE WHEN c.StartDate IS NULL THEN '19000101' " & _
                                "WHEN c.StartDate='19000101' THEN '19000101' " & _
                                "ELSE c.StartDate " & _
                                "END 'StartDate' , CASE WHEN c.EndDate IS NULL THEN '19000101' " & _
                                "WHEN c.EndDate='19000101' THEN '19000101' " & _
                                "ELSE c.EndDate " & _
                                "END 'EndDate'  FROM " & _
                           AptifyApplication.GetEntityBaseDatabase("Classes") & _
                            "..vwClasses c LEFT OUTER JOIN " & _
                           AptifyApplication.GetEntityBaseDatabase("Persons") & _
                           "..vwPersons p ON c.InstructorID=p.ID INNER JOIN " & _
                           AptifyApplication.GetEntityBaseDatabase("Courses") & _
                           "..vwCourses cr ON c.CourseID=cr.ID "

                'sSQL = "SELECT c.ID,cr.Name Course,c.CourseID,cr.Category,cr.CategoryID," & _
                '       "c.Type,c.StartDate,c.School Location,p.LastName +', ' + p.FirstName Instructor FROM " & _
                '       AptifyApplication.GetEntityBaseDatabase("Classes") & _
                '       "..vwClasses c INNER JOIN " & _
                '       AptifyApplication.GetEntityBaseDatabase("Persons") & _
                '       "..vwPersons p ON c.InstructorID=p.ID INNER JOIN " & _
                '       AptifyApplication.GetEntityBaseDatabase("Courses") & _
                '       "..vwCourses cr ON c.CourseID=cr.ID WHERE c.StartDate>=DateAdd(mm,-1,GETDATE()) "

                ' Vijay Sitlani made changes o correct Issue 5525 on dated 11-21-2007
                ' Start date condition was in-correct earlier.
                ''Nalini issue:13277
                sSQL &= "WHERE" & _
                        "(c.EndDate>=GETDATE() OR c.EndDate='19000101' OR c.EndDate IS NULL)"

                If CourseID > 0 Then
                    sSQL &= " AND c.CourseID=" & CourseID
                Else
                    If CLng(Me.cmbCourse.SelectedValue) > 0 Then
                        sSQL &= " AND c.CourseID=" & cmbCourse.SelectedValue
                    End If
                End If
                If CLng(Me.cmbCategory.SelectedValue) > 0 Then
                    sSQL &= " AND cr.CategoryID=" & cmbCategory.SelectedValue
                End If
                If Len(Me.cmbType.SelectedValue) > 0 Then
                    sSQL &= " AND c.Type='" & cmbType.SelectedValue & "'"
                End If
                If CLng(Me.cmbStartDate.SelectedValue) > 0 Then
                    sSQL &= " AND c.StartDate<=DateAdd(mm," & cmbStartDate.SelectedValue & ",GETDATE())"
                End If
                If InstructorVisible AndAlso CLng(Me.cmbInstructor.SelectedValue) > 0 Then
                    sSQL &= " AND c.InstructorID=" & cmbInstructor.SelectedValue
                End If
                If LocationVisible AndAlso CLng(Me.cmbLocation.SelectedValue) > 0 Then
                    sSQL &= " AND c.SchoolID=" & cmbLocation.SelectedValue
                End If
                'Modified the SQL Statement below to add the additional logic to ensure that the Class Status is 
                'Approved. Approved is a new field added to the classes entity for Aptify 4.0 Service Pack 3 and 
                'will be available for use in E Business 4.0 SP2.  Ravi Nagarajan, July 31, 2007
                sSQL &= " AND c.WebEnabled=1 AND cr.WebEnabled=1 AND cr.Status='Available' AND c.Status='Approved' " & _
                        "ORDER BY c.StartDate "
                Return sSQL
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return Nothing
            End Try
        End Function

        Protected Overridable Sub LoadCombos()
            Try
                LoadCategories()
                LoadCourses()
                LoadTypes()
                LoadInstructors()
                LoadSchools()
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Overridable Sub LoadTypes()
            cmbType.Items.Clear()
            cmbType.Items.Add(New ListItem("<All Types>", ""))
            cmbType.Items.Add(New ListItem("Classroom", "Classroom"))
            cmbType.Items.Add(New ListItem("Independent Study", "Independent Study"))
            cmbType.Items.Add(New ListItem("Internet", "Internet"))
            cmbType.Items.Add(New ListItem("At Work", "At Work"))
        End Sub
        Protected Overridable Sub LoadInstructors()
            Dim sSQL As String, dt As Data.DataTable
            Try
                sSQL = "SELECT ID, LastName + ', ' + FirstName FROM " & _
                       Me.AptifyApplication.GetEntityBaseDatabase("Persons") & _
                       "..vwPersons WHERE ID IN (SELECT InstructorID FROM " & _
                       Me.AptifyApplication.GetEntityBaseDatabase("Courses") & _
                       "..vwCourseInstructors WHERE Status='Active' "
                If IsNumeric(ViewState("CourseID")) AndAlso CLng(ViewState("CourseID")) > 0 Then
                    sSQL &= " AND CourseID=" & ViewState("CourseID").ToString & ")"
                Else
                    sSQL &= ")"
                End If
                sSQL &= "ORDER BY LastName, FirstName"
                dt = Me.DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                cmbInstructor.Items.Clear()
                cmbInstructor.Items.Add(New ListItem("<All Instructors>", "-1"))
                For Each dr As Data.DataRow In dt.Rows
                    cmbInstructor.Items.Add(New ListItem(dr(1).ToString, dr(0).ToString))
                Next
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Protected Overridable Sub LoadSchools()
            Dim sSQL As String, dt As Data.DataTable
            Try
                sSQL = "SELECT ID, Name FROM " & _
                       Me.AptifyApplication.GetEntityBaseDatabase("Companies") & _
                       "..vwCompanies WHERE ID IN (SELECT SchoolID FROM " & _
                       Me.AptifyApplication.GetEntityBaseDatabase("Courses") & _
                       "..vwCourseSchools WHERE Status='Active' "
                If IsNumeric(ViewState("CourseID")) AndAlso CLng(ViewState("CourseID")) > 0 Then
                    sSQL &= " AND CourseID=" & ViewState("CourseID").ToString & ")"
                Else
                    sSQL &= ")"
                End If
                sSQL &= "ORDER BY Name"
                dt = Me.DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                cmbLocation.Items.Clear()
                cmbLocation.Items.Add(New ListItem("<All Locations>", "-1"))
                For Each dr As Data.DataRow In dt.Rows
                    cmbLocation.Items.Add(New ListItem(dr(1).ToString, dr(0).ToString))
                Next
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Protected Overridable Sub LoadCourses()
            'Issue 4895 - 2007-04-25
            'The Courses ComboBox displays all Courses that meet the criteria included in the database query created in this method.
            'Because Courses may have ScopeFilters applied to them (if using Aptify sp3 or greater)
            'the code below handles both scenarios (if on sp3+ or not) by using two different
            'queries. This code can be simplified by removing all pre-sp2
            'specific code after upgrading to sp3.
            Try
                Dim sSQL As String, dt As Data.DataTable
                cmbCourse.Items.Clear()
                cmbCourse.Items.Add(New ListItem("<All Courses>", "-1", True))

                sSQL = "SELECT ID,WebName, ApplyFilterRule, ISNULL(ScopeFilterRuleID,-1) 'ScopeFilterRuleID'  FROM " & _
                       AptifyApplication.GetEntityBaseDatabase("Courses") & _
                       "..vwCourses "
                If IsNumeric(ViewState("CategoryID")) AndAlso CLng(ViewState("CategoryID")) > 0 Then
                    sSQL &= "WHERE CategoryID=" & ViewState("CategoryID").ToString & " AND "
                ElseIf CLng(cmbCategory.SelectedValue) > 0 Then
                    sSQL &= "WHERE CategoryID=" & cmbCategory.SelectedValue & " AND "
                Else
                    sSQL &= "WHERE "
                End If
                sSQL &= "Status='Available' AND WebEnabled=1 ORDER BY Category, WebName"
                dt = Me.DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                If Not dt Is Nothing Then
                    For Each dr As Data.DataRow In dt.Rows
                        'Omit the course if this user does not meet the Course's Filter Rules
                        If CBool(dr("ApplyFilterRule")) Then
                            'Must check Scope Filter Rule
                            Dim oFilter As New Aptify.Applications.Education.EducationManagementScopeFilter(Me.AptifyApplication)
                            If oFilter.EvaluateCourseFilter(CLng(dr("ScopeFilterRuleID")), Me.User1.PersonID) Then
                                cmbCourse.Items.Add(New ListItem(dr("WebName").ToString, dr("ID").ToString))
                            End If
                        Else
                            'Filter Rule not applied, so add Course
                            cmbCourse.Items.Add(New ListItem(dr("WebName").ToString, dr("ID").ToString))
                        End If
                    Next
                End If
                If IsNumeric(ViewState("CourseID")) Then
                    Dim oItem As ListItem
                    oItem = cmbCourse.Items.FindByValue(CStr(ViewState("CategoryID")))
                    If oItem IsNot Nothing Then
                        oItem.Selected = True
                    End If
                Else
                    cmbCourse.SelectedIndex = 0
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Protected Overridable Sub LoadCategories()
            Try
                cmbCategory.Items.Clear()
                cmbCategory.Items.Add(New ListItem("<All Categories>", "-1", True))
                RecursiveLoadCategory(-1, "")
                If IsNumeric(ViewState("CategoryID")) Then
                    Dim oItem As ListItem
                    oItem = cmbCategory.Items.FindByValue(CStr(ViewState("CategoryID")))
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

        Protected Sub cmbCategory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCategory.SelectedIndexChanged
            Me.LoadCourses()
            LoadGrid(CLng(ViewState("CourseID")), CLng(ViewState("CategoryID")))
        End Sub

        Protected Sub cmbCourse_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCourse.SelectedIndexChanged
            LoadGrid(CLng(ViewState("CourseID")), CLng(ViewState("CategoryID")))
        End Sub

        Protected Sub cmbInstructor_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbInstructor.SelectedIndexChanged
            LoadGrid(CLng(ViewState("CourseID")), CLng(ViewState("CategoryID")))
        End Sub

        Protected Sub cmbLocation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbLocation.SelectedIndexChanged
            LoadGrid(CLng(ViewState("CourseID")), CLng(ViewState("CategoryID")))
        End Sub

        Protected Sub cmbStartDate_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStartDate.SelectedIndexChanged
            LoadGrid(CLng(ViewState("CourseID")), CLng(ViewState("CategoryID")))
        End Sub

        Protected Sub cmbType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbType.SelectedIndexChanged
            LoadGrid(CLng(ViewState("CourseID")), CLng(ViewState("CategoryID")))
        End Sub
    End Class
End Namespace
