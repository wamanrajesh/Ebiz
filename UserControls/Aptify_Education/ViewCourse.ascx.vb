' Aptify e-Business 5.5.1/LMS 5.5.1, June 2014
Option Explicit On
Option Strict On

Imports Telerik.Web.UI
Imports System.Data
Namespace Aptify.Framework.Web.eBusiness.Education
    Partial Class ViewCourseControl
        Inherits eBusiness.BaseUserControlAdvanced

        '(LEFT NAV) attributes
        Protected Const ATTRIBUTE_LEFT_NAV_CLASS_SCHEDULE_IMAGE_URL As String = "LeftNavClassScheduleImage"
        Protected Const ATTRIBUTE_LEFT_NAV_GENERAL_IMAGE_URL As String = "LeftNavGeneralImage"
        Protected Const ATTRIBUTE_LEFT_NAV_INSTRUCTORS_IMAGE_URL As String = "LeftNavInstructorsImage"
        Protected Const ATTRIBUTE_LEFT_NAV_SYLLABUS_IMAGE_URL As String = "LeftNavSyllabusImage"
        Protected Const ATTRIBUTE_LEFT_NAV_PREREQUISITES_IMAGE_URL As String = "LeftNavPrerequisitesImage"
        Protected Const ATTRIBUTE_LEFT_NAV_LOCATIONS_IMAGE_URL As String = "LeftNavLocationsImage"
        '(RIGHT DISPLAY) attributes
        Protected Const ATTRIBUTE_DISPLAY_GENERAL_IMAGE_URL As String = "DisplayGeneralImage"
        Protected Const ATTRIBUTE_DISPLAY_SYLLABUS_IMAGE_URL As String = "DisplaySyllabusImage"
        Protected Const ATTRIBUTE_DISPLAY_PREREQUISITES_IMAGE_URL As String = "DisplayPrerequisitesImage"
        Protected Const ATTRIBUTE_DISPLAY_INSTRUCTOR_IMAGE_URL As String = "DisplayInstructorImage"
        Protected Const ATTRIBUTE_DISPLAY_LOCATIONS_IMAGE_URL As String = "DisplayLocationsImage"
        Protected Const ATTRIBUTE_DISPLAY_CLASS_SCHEDULE_IMAGE_URL As String = "DisplayClassScheduleImage"
        '(GRID DISPLAY) attributes
        Protected Const ATTRIBUTE_DISPLAY_GRID_SYLLABUS_IMAGE_URL As String = "DisplayGridSyllabusImage"
        Protected Const ATTRIBUTE_DISPLAY_GRID_PREREQUISITES_IMAGE_URL As String = "DisplayGridPrerequisitesImage"
        Protected Const ATTRIBUTE_DISPLAY_GRID_PREREQUISITES_PAGE As String = "DisplayGridPrerequisitesPage"
        Protected Const ATTRIBUTE_CATEGORY_PAGE As String = "CategoryPage"
        'control name
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "ViewCourse"

        Private m_sView As String

#Region "ViewCourse: (RIGHT DISPLAY) & Display Grid Properties"
        ''' <summary>
        ''' Category page url
        ''' </summary>
        Public Overridable Property CategoryPage() As String
            Get
                If Not ViewState(ATTRIBUTE_CATEGORY_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_CATEGORY_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_CATEGORY_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' DisplayGridSyllabusImage url
        ''' </summary>
        Public Overridable Property DisplayGridSyllabusImage() As String
            Get
                If Not ViewState(ATTRIBUTE_DISPLAY_GRID_SYLLABUS_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_DISPLAY_GRID_SYLLABUS_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_DISPLAY_GRID_SYLLABUS_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' DisplayGridPrerequisitesImage url
        ''' </summary>
        Public Overridable Property DisplayGridPrerequisitesImage() As String
            Get
                If Not ViewState(ATTRIBUTE_DISPLAY_GRID_PREREQUISITES_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_DISPLAY_GRID_PREREQUISITES_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_DISPLAY_GRID_PREREQUISITES_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' DisplayGridPrerequisites page url
        ''' </summary>
        Public Overridable Property DisplayGridPrerequisitesPage() As String
            Get
                If Not ViewState(ATTRIBUTE_DISPLAY_GRID_PREREQUISITES_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_DISPLAY_GRID_PREREQUISITES_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_DISPLAY_GRID_PREREQUISITES_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' DisplayGeneralImage url
        ''' </summary>
        Public Overridable Property DisplayGeneralImage() As String
            Get
                If Not ViewState(ATTRIBUTE_DISPLAY_GENERAL_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_DISPLAY_GENERAL_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_DISPLAY_GENERAL_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' DisplaySyllabusImage url
        ''' </summary>
        Public Overridable Property DisplaySyllabusImage() As String
            Get
                If Not ViewState(ATTRIBUTE_DISPLAY_SYLLABUS_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_DISPLAY_SYLLABUS_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_DISPLAY_SYLLABUS_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' DisplayPrerequisitesImage url
        ''' </summary>
        Public Overridable Property DisplayPrerequisitesImage() As String
            Get
                If Not ViewState(ATTRIBUTE_DISPLAY_PREREQUISITES_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_DISPLAY_PREREQUISITES_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_DISPLAY_PREREQUISITES_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' DisplayInstructorImage url
        ''' </summary>
        Public Overridable Property DisplayInstructorImage() As String
            Get
                If Not ViewState(ATTRIBUTE_DISPLAY_INSTRUCTOR_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_DISPLAY_INSTRUCTOR_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_DISPLAY_INSTRUCTOR_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' DisplayLocationsImage url
        ''' </summary>
        Public Overridable Property DisplayLocationsImage() As String
            Get
                If Not ViewState(ATTRIBUTE_DISPLAY_LOCATIONS_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_DISPLAY_LOCATIONS_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_DISPLAY_LOCATIONS_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' DisplayClassScheduleImage url
        ''' </summary>
        Public Overridable Property DisplayClassScheduleImage() As String
            Get
                If Not ViewState(ATTRIBUTE_DISPLAY_CLASS_SCHEDULE_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_DISPLAY_CLASS_SCHEDULE_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_DISPLAY_CLASS_SCHEDULE_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
#End Region

#Region "ViewCourse: (LEFT NAV) Properties"
        ''' <summary>
        ''' LeftNavClassScheduleImage url
        ''' </summary>
        Public Overridable Property LeftNavClassScheduleImage() As String
            Get
                If Not ViewState(ATTRIBUTE_LEFT_NAV_CLASS_SCHEDULE_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_LEFT_NAV_CLASS_SCHEDULE_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_LEFT_NAV_CLASS_SCHEDULE_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' LeftNavGeneralImage page url
        ''' </summary>
        Public Overridable Property LeftNavGeneralImage() As String
            Get
                If Not ViewState(ATTRIBUTE_LEFT_NAV_GENERAL_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_LEFT_NAV_GENERAL_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_LEFT_NAV_GENERAL_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' LeftNavInstructorsImage url
        ''' </summary>
        Public Overridable Property LeftNavInstructorsImage() As String
            Get
                If Not ViewState(ATTRIBUTE_LEFT_NAV_INSTRUCTORS_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_LEFT_NAV_INSTRUCTORS_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_LEFT_NAV_INSTRUCTORS_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' LeftNavSyllabusImage url
        ''' </summary>
        Public Overridable Property LeftNavSyllabusImage() As String
            Get
                If Not ViewState(ATTRIBUTE_LEFT_NAV_SYLLABUS_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_LEFT_NAV_SYLLABUS_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_LEFT_NAV_SYLLABUS_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' LeftNavPrerequisitesImage url
        ''' </summary>
        Public Overridable Property LeftNavPrerequisitesImage() As String
            Get
                If Not ViewState(ATTRIBUTE_LEFT_NAV_PREREQUISITES_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_LEFT_NAV_PREREQUISITES_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_LEFT_NAV_PREREQUISITES_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' LeftNavLocationsImage url
        ''' </summary>
        Public Overridable Property LeftNavLocationsImage() As String
            Get
                If Not ViewState(ATTRIBUTE_LEFT_NAV_LOCATIONS_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_LEFT_NAV_LOCATIONS_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_LEFT_NAV_LOCATIONS_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
#End Region

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
            If Not IsPostBack Then
                ''Added by Suvarna for IssueID - 13270 on Mar 11,2013
                'Function added to apply ascending sort on field
                AddExpression()
                If IsNumeric(Request.QueryString("CourseID")) Then
                    LoadCourse()
                Else
                    'Me.divContent.Visible = False
                    Me.lblError.Visible = True
                    lblError.Text = "Unauthorized Access to Course"
                End If
            End If
        End Sub

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            '(GRID DISPLAY)
            If String.IsNullOrEmpty(CategoryPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                CategoryPage = Me.GetLinkValueFromXML(ATTRIBUTE_CATEGORY_PAGE)
                If String.IsNullOrEmpty(CategoryPage) Then
                    lnkCategory.Enabled = False
                    lnkCategory.ToolTip = "CategoryPage property has not been set."
                Else
                    lnkCategory.NavigateUrl = CategoryPage
                End If
            Else
                lnkCategory.NavigateUrl = CategoryPage
            End If
            If String.IsNullOrEmpty(DisplayGridSyllabusImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                DisplayGridSyllabusImage = Me.GetLinkValueFromXML(ATTRIBUTE_DISPLAY_GRID_SYLLABUS_IMAGE_URL)
            End If
            If String.IsNullOrEmpty(DisplayGridPrerequisitesImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                DisplayGridPrerequisitesImage = Me.GetLinkValueFromXML(ATTRIBUTE_DISPLAY_GRID_PREREQUISITES_IMAGE_URL)
            End If
            If String.IsNullOrEmpty(DisplayGridPrerequisitesPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                DisplayGridPrerequisitesPage = Me.GetLinkValueFromXML(ATTRIBUTE_DISPLAY_GRID_PREREQUISITES_PAGE)
                If String.IsNullOrEmpty(DisplayGridPrerequisitesPage) Then
                    grdPrerequisites.Enabled = False
                    grdPrerequisites.ToolTip = "DisplayGridPrerequisitesPage property has not been set."
                End If
            End If
            '(RIGHT DISPLAY)
            If String.IsNullOrEmpty(DisplayGeneralImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                DisplayGeneralImage = Me.GetLinkValueFromXML(ATTRIBUTE_DISPLAY_GENERAL_IMAGE_URL)
            End If
            If String.IsNullOrEmpty(DisplaySyllabusImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                DisplaySyllabusImage = Me.GetLinkValueFromXML(ATTRIBUTE_DISPLAY_SYLLABUS_IMAGE_URL)
            End If
            If String.IsNullOrEmpty(DisplayPrerequisitesImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                DisplayPrerequisitesImage = Me.GetLinkValueFromXML(ATTRIBUTE_DISPLAY_PREREQUISITES_IMAGE_URL)
            End If
            If String.IsNullOrEmpty(DisplayInstructorImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                DisplayInstructorImage = Me.GetLinkValueFromXML(ATTRIBUTE_DISPLAY_INSTRUCTOR_IMAGE_URL)
            End If
            If String.IsNullOrEmpty(DisplayLocationsImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                DisplayLocationsImage = Me.GetLinkValueFromXML(ATTRIBUTE_DISPLAY_LOCATIONS_IMAGE_URL)
            End If
            If String.IsNullOrEmpty(DisplayClassScheduleImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                DisplayClassScheduleImage = Me.GetLinkValueFromXML(ATTRIBUTE_DISPLAY_CLASS_SCHEDULE_IMAGE_URL)
            End If
            '(LEFT NAV)
            If String.IsNullOrEmpty(LeftNavClassScheduleImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                LeftNavClassScheduleImage = Me.GetLinkValueFromXML(ATTRIBUTE_LEFT_NAV_CLASS_SCHEDULE_IMAGE_URL)
                imgScheduleSmall.Src = LeftNavClassScheduleImage
            End If
            If String.IsNullOrEmpty(LeftNavGeneralImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                LeftNavGeneralImage = Me.GetLinkValueFromXML(ATTRIBUTE_LEFT_NAV_GENERAL_IMAGE_URL)
                imgGenInfoSmall.Src = LeftNavGeneralImage
            End If
            If String.IsNullOrEmpty(LeftNavInstructorsImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                LeftNavInstructorsImage = Me.GetLinkValueFromXML(ATTRIBUTE_LEFT_NAV_INSTRUCTORS_IMAGE_URL)
                imgInstructorSmall.Src = LeftNavInstructorsImage
            End If
            If String.IsNullOrEmpty(LeftNavSyllabusImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                LeftNavSyllabusImage = Me.GetLinkValueFromXML(ATTRIBUTE_LEFT_NAV_SYLLABUS_IMAGE_URL)
                imgSyllabusSmall.Src = LeftNavSyllabusImage
            End If
            If String.IsNullOrEmpty(LeftNavPrerequisitesImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                LeftNavPrerequisitesImage = Me.GetLinkValueFromXML(ATTRIBUTE_LEFT_NAV_PREREQUISITES_IMAGE_URL)
                imgPrereqSmall.Src = LeftNavPrerequisitesImage
            End If
            If String.IsNullOrEmpty(LeftNavLocationsImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                LeftNavLocationsImage = Me.GetLinkValueFromXML(ATTRIBUTE_LEFT_NAV_LOCATIONS_IMAGE_URL)
                imgLocationsSmall.Src = LeftNavLocationsImage
            End If
        End Sub

        Protected Overridable Sub LoadCourse()
            'Issue 4895 - 2007-04-25
            'Because Courses may have ScopeFilters applied to them (if using Aptify sp3 or greater)
            'the code below handles both scenarios (if on sp3+ or not) by checking to see if a field
            'exists in the Courses entity that is specific to this sp3 update (the ApplyFilterRule field).
            'If this field is found then the logged in user will be checked against the course's Filter
            'Rules. If the rules are satisfied, the course will be displayed, if not an error message will
            'be displayed instead.
            Try
                Dim dt As Data.DataTable, sSQL As String

                sSQL = Me.AptifyApplication.GetEntityBaseDatabase("Courses") & _
                       "..spGetCourses @ID=" & _
                       Request.QueryString("CourseID")
                dt = Me.DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                If dt IsNot Nothing AndAlso _
                   dt.Rows.Count > 0 Then
                    With dt.Rows(0)
                        'Check to see if we should validate this user against the Course's filter rules
                        Dim bShowCourse As Boolean = True
                        Dim ApplyFilterField As Aptify.Framework.BusinessLogic.GenericEntity.AptifyDataFieldBase
                        ApplyFilterField = Me.AptifyApplication.GetEntityObject("Courses", -1).Fields.Item("ApplyFilterRule")
                        If ApplyFilterField IsNot Nothing Then
                            Dim oFilter As New Aptify.Applications.Education.EducationManagementScopeFilter(Me.AptifyApplication)
                            If Not oFilter.EvaluateCourseFilterByCourseID(CLng(Request.QueryString("CourseID")), Me.User1.PersonID) Then
                                bShowCourse = False
                            End If
                        End If
                        If bShowCourse Then
                            'This user meets the Course's Filter Rule's criteria
                            If CBool(.Item("WebEnabled")) Then
                                m_sView = "General"
                                lblName.Text = .Item("WebName").ToString.Trim
                                lblDescription.Text = .Item("WebDescription").ToString.Trim
                                lnkCategory.Text = .Item("Category").ToString.Trim & ":"
                                lnkCategory.NavigateUrl = CategoryPage & "?CategoryID=" & .Item("CategoryID").ToString
                                If CBool(.Item("ShowInstructorInfo")) Then
                                    trInstructors.Visible = True
                                Else
                                    trInstructors.Visible = False
                                End If

                                If Request.QueryString("View") IsNot Nothing Then
                                    Select Case Request.QueryString("View").Trim.ToUpper
                                        Case "INSTRUCTORS"
                                            If CBool(.Item("ShowInstructorInfo")) Then
                                                m_sView = "Instructors"
                                            Else
                                                m_sView = "General"
                                            End If
                                        Case "SYLLABUS"
                                            m_sView = "Syllabus"
                                        Case "PREREQUISITES"
                                            m_sView = "Prerequisites"
                                        Case "LOCATIONS"
                                            m_sView = "Locations"
                                        Case "SCHEDULE"
                                            m_sView = "Schedule"
                                        Case Else
                                            m_sView = "General"
                                    End Select
                                End If

                                lnkInstructors.Font.Bold = False
                                lnkSyllabus.Font.Bold = False
                                lnkPrerequisites.Font.Bold = False
                                lnkGeneral.Font.Bold = False
                                lnkLocations.Font.Bold = False
                                lnkSchedule.Font.Bold = False

                                lblDetails.Visible = False
                                grdInstructors.Visible = False
                                grdLocations.Visible = False
                                grdPrerequisites.Visible = False
                                grdSyllabus.Visible = False
                                pnlSchedule.Visible = False

                                Select Case m_sView
                                    Case "General"
                                        lnkGeneral.Font.Bold = True
                                        lblDetails.Text = .Item("WebDetails").ToString.Trim
                                        lblDetails.Visible = True
                                        lblTitle.Text = "General Information"
                                        imgTitle.Src = DisplayGeneralImage
                                    Case "Syllabus"
                                        LoadSyllabus()
                                        lnkSyllabus.Font.Bold = True
                                        grdSyllabus.Visible = True
                                        lblTitle.Text = "Standard Course Syllabus"
                                        imgTitle.Src = DisplaySyllabusImage
                                    Case "Instructors"
                                        lnkInstructors.Font.Bold = True
                                        grdInstructors.Visible = True
                                        lblTitle.Text = "Instructor Information"
                                        imgTitle.Src = DisplayInstructorImage
                                        LoadInstructors()
                                    Case "Prerequisites"
                                        lnkPrerequisites.Font.Bold = True
                                        grdPrerequisites.Visible = True
                                        lblTitle.Text = "Prerequisite Courses"
                                        imgTitle.Src = DisplayPrerequisitesImage
                                        LoadPrerequisites()
                                    Case "Locations"
                                        lnkLocations.Font.Bold = True
                                        grdLocations.Visible = True
                                        imgTitle.Src = DisplayLocationsImage
                                        LoadLocations()
                                        lblTitle.Text = "Locations for " & .Item("WebName").ToString
                                    Case "Schedule"
                                        lnkSchedule.Font.Bold = True
                                        pnlSchedule.Visible = True
                                        imgTitle.Src = DisplayClassScheduleImage
                                        LoadSchedule()
                                        lblTitle.Text = "Class Schedule for " & .Item("WebName").ToString
                                End Select

                                Dim sBase As String = Me.Request.Url.AbsolutePath & "?CourseID=" & Request.QueryString("CourseID") & "&View="
                                lnkGeneral.NavigateUrl = sBase & "General"
                                lnkInstructors.NavigateUrl = sBase & "Instructors"
                                lnkLocations.NavigateUrl = sBase & "Locations"
                                lnkPrerequisites.NavigateUrl = sBase & "Prerequisites"
                                lnkSchedule.NavigateUrl = sBase & "Schedule"
                                lnkSyllabus.NavigateUrl = sBase & "Syllabus"
                            Else
                                'This user does not meet the Course's Filter Rule's criteria, so the page won't be displayed
                                Me.lblError.Text = "This is a Private Course and not available for viewing."
                                'Me.divContent.Visible = False
                                Me.lblError.Visible = True
                            End If
                        Else
                            lblName.Text = "This course is not available on the web."
                            lblName.ForeColor = Drawing.Color.Red
                            lblName.Font.Bold = True
                        End If
                    End With
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Private Sub LoadSchedule()
            ClassScheduleControl.LoadCourseSchedule(CLng(Me.Request("CourseID")), -1)
        End Sub
        Private Sub LoadPrerequisites()
            Dim sSQL As String, dt As Data.DataTable, sDB As String = AptifyApplication.GetEntityBaseDatabase("Courses")
            Try
                'sSQL = " SELECT c.ID, c.WebName, c.WebDescription FROM " & _
                '     sDB & "..vwCourses c INNER JOIN " & sDB & _
                '     "..vwCoursePreReqs cs ON cs.PreReqCourseID=c.ID " & _
                '     " WHERE cs.Status='Current' AND cs.CourseID=" & Me.Request("CourseID") & _
                '      " ORDER BY cs.Sequence"
                'Sheetal 16/07/15: For Issue 21368 :Prevent Sql Injection Attack: Used parameterrized sql is best practice
                Dim params(0) As IDataParameter
                params(0) = Me.DataAction.GetDataParameter("@CourseID", SqlDbType.Int, Request.QueryString("CourseID"))
                sSQL = Me.AptifyApplication.GetEntityBaseDatabase("Courses") & "..spLoadPrerequisites"

                'sSQL = Me.AptifyApplication.GetEntityBaseDatabase("Courses") & "..spLoadPrerequisites @CourseID=" & _
                'Request.QueryString("CourseID")

                '  dt = Me.DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                dt = Me.DataAction.GetDataTableParametrized(sSQL, CommandType.StoredProcedure, params)



                'Navin Prasad Issue 11032

                '  DirectCast(grdPrerequisites.Columns(0), HyperLinkColumn).DataNavigateUrlFormatString = DisplayGridPrerequisitesPage & "?View=Prerequisites&CourseID={0:F0}"

                Dim dcolUrl As DataColumn = New DataColumn()
                dcolUrl.Caption = "IDUrl"
                dcolUrl.ColumnName = "IDUrl"

                dt.Columns.Add(dcolUrl)
                If dt.Rows.Count > 0 Then

                    For Each rw As DataRow In dt.Rows
                        rw("IDUrl") = String.Format(DisplayGridPrerequisitesPage & "?View=Prerequisites&CourseID={0:F0}", rw("ID").ToString)
                    Next
                End If

                grdPrerequisites.DataSource = dt
                '' grdPrerequisites.DataBind()
                'Dim rowcounter As Integer = 0

                'For Each row As GridViewRow In grdPrerequisites.Rows
                '    Dim hlink As HyperLink = CType(row.FindControl("lnkWebName"), HyperLink)

                '    hlink.NavigateUrl = String.Format(DisplayGridPrerequisitesPage & "?View=Prerequisites&CourseID={0:F0}", dt.Rows((grdPrerequisites.PageIndex * grdPrerequisites.PageSize) + rowcounter)("ID").ToString)
                '    rowcounter = rowcounter + 1
                'Next


            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Private Sub LoadLocations()
            Dim sSQL As String, dt As Data.DataTable, sDB As String = AptifyApplication.GetEntityBaseDatabase("Companies")
            Try
                ' sSQL = " SELECT c.Name, case len(isnull(c.state,'')) " & _
                '  "WHEN 0 THEN c.City + ', ' + c.Country ELSE " & _
                '    "c.City + ', ' + c.State + ' ' + c.Country END " & _
                '   "Location  FROM " & _
                '   sDB & "..vwCompanies c INNER JOIN " & sDB & _
                '     "..vwCourseSchools cs ON cs.SchoolID=c.ID " & _
                '    " WHERE cs.Status='Active' AND cs.CourseID=" & Me.Request("CourseID") & _
                '    " ORDER BY cs.Rank,c.Name"

                'Sheetal 16/07/15: For Issue 21368 :Prevent Sql Injection Attack: Used parameterrized sql is best practice
                Dim params(0) As IDataParameter
                params(0) = Me.DataAction.GetDataParameter("@CourseID", SqlDbType.Int, Request.QueryString("CourseID"))
                sSQL = Me.AptifyApplication.GetEntityBaseDatabase("Courses") & "..spLoadLocations"
                dt = Me.DataAction.GetDataTableParametrized(sSQL, CommandType.StoredProcedure, params)

                ' dt = Me.DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                grdLocations.DataSource = dt
                ''grdLocations.DataBind()
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Private Sub LoadInstructors()
            Dim sSQL As String, dt As Data.DataTable, sDB As String = AptifyApplication.GetEntityBaseDatabase("Persons")
            Try
                ' sSQL = " SELECT RTRIM(p.LastName) + ', ' + " & _
                '       "p.FirstName Name, case len(isnull(p.state,'')) " & _
                '      "WHEN 0 THEN p.City + ', ' + p.Country ELSE " & _
                '     "p.City + ', ' + p.State + ' ' + p.Country END " & _
                '    "Location, p.Email1 FROM " & _
                '   sDB & "..vwPersons p INNER JOIN " & sDB & _
                '  "..vwCourseInstructors ci ON ci.InstructorID=p.ID " & _
                '  " WHERE ci.Status='Active' AND ci.CourseID=" & Me.Request("CourseID") & _
                '  " ORDER BY ci.Rank,p.LastName,p.FirstName"
                ' dt = Me.DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)

                'Sheetal 16/07/15 :For Issue 21368 :Prevent Sql Injection Attack: Used parameterrized sql is best practice
                Dim params(0) As IDataParameter
                params(0) = Me.DataAction.GetDataParameter("@CourseID", SqlDbType.Int, Request.QueryString("CourseID"))
                sSQL = Me.AptifyApplication.GetEntityBaseDatabase("Courses") & "..spLoadInstructors"
                dt = Me.DataAction.GetDataTableParametrized(sSQL, CommandType.StoredProcedure, params)

                Dim dcolUrl As DataColumn = New DataColumn()
                dcolUrl.Caption = "Email1Url"
                dcolUrl.ColumnName = "Email1Url"

                dt.Columns.Add(dcolUrl)
                If dt.Rows.Count > 0 Then

                    For Each rw As DataRow In dt.Rows
                        rw("Email1Url") = String.Format("mailto:{0:s}", rw("Email1").ToString)
                    Next
                End If

                grdInstructors.DataSource = dt
                ''grdInstructors.DataBind()
                'Navin Prasad Issue 11032
                'Nalini



                'Dim rowcounter As Integer = 0

                'If dt.Rows.Count > 0 Then
                '    For Each row As GridViewRow In grdInstructors.Rows
                '        Dim lnk As HyperLink = CType(row.FindControl("lnkEmail"), HyperLink)
                '        lnk.NavigateUrl = String.Format("mailto:{0:s}", dt.Rows((grdInstructors.PageIndex * grdInstructors.PageSize) + rowcounter)("Email1").ToString)
                '        rowcounter = rowcounter + 1
                '    Next
                'End If



            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Private Sub LoadSyllabus()
            Dim sSQL As String, dt As Data.DataTable, sDB As String = AptifyApplication.GetEntityBaseDatabase("Course Parts")
            Try

                '  sSQL = " SELECT cp.WebName,cp.WebDescription,cp.Type,cp.Duration FROM " & _
                '     sDB & "..vwCoursePartUse cpu INNER JOIN " & sDB & _
                '    "..vwCourseParts cp ON cpu.CoursePartID=cp.ID " & _
                '     " WHERE cpu.CourseID=" & Me.Request("CourseID") & _
                '     " and cp.webenabled=1 ORDER BY cpu.Sequence"

                'Sheetal 16/07/15 :For Issue 21368 :Prevent Sql Injection Attack: Used parameterrized sql is best practice
                Dim params(0) As IDataParameter
                params(0) = Me.DataAction.GetDataParameter("@CourseID", SqlDbType.Int, Request.QueryString("CourseID"))

                sSQL = Me.AptifyApplication.GetEntityBaseDatabase("Courses") & "..spLoadSyllabus"
                dt = Me.DataAction.GetDataTableParametrized(sSQL, CommandType.StoredProcedure, params)


                'dt = Me.DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                grdSyllabus.DataSource = dt
                ''grdSyllabus.DataBind()
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        ''' <summary>
        ''' Nalini 12436 date:1/12/2011
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub grdSyllabus_PageIndexChanging(ByVal sender As Object, ByVal e As GridPageChangedEventArgs) Handles grdSyllabus.PageIndexChanged
            ''grdSyllabus.PageIndex = e.NewPageIndex
            LoadSyllabus()
        End Sub
        Protected Sub grdSyllabus_PageIndexChanging(ByVal sender As Object, ByVal e As GridPageSizeChangedEventArgs) Handles grdSyllabus.PageSizeChanged
            ''grdSyllabus.PageIndex = e.NewPageIndex
            LoadSyllabus()
        End Sub
        ''' <summary>
        ''' Nalini 12436 date:1/12/2011
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub grdPrerequisites_PageIndexChanging(ByVal sender As Object, ByVal e As GridPageChangedEventArgs) Handles grdPrerequisites.PageIndexChanged
            ''grdPrerequisites.PageIndex = e.NewPageIndex
            LoadPrerequisites()
        End Sub
        Protected Sub grdPrerequisites_PageIndexChanging(ByVal sender As Object, ByVal e As GridPageSizeChangedEventArgs) Handles grdPrerequisites.PageSizeChanged
            ''grdPrerequisites.PageIndex = e.NewPageIndex
            LoadPrerequisites()
        End Sub
        ''' <summary>
        ''' Nalini 12436 date:1/12/2011
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub grdInstructors_PageIndexChanging(ByVal sender As Object, ByVal e As GridPageChangedEventArgs) Handles grdInstructors.PageIndexChanged
            ''grdInstructors.PageIndex = e.NewPageIndex
            LoadInstructors()
        End Sub
        Protected Sub grdInstructors_PageIndexChanging(ByVal sender As Object, ByVal e As GridPageSizeChangedEventArgs) Handles grdInstructors.PageSizeChanged
            ''grdInstructors.PageIndex = e.NewPageIndex
            LoadInstructors()
        End Sub
        ''' <summary>
        ''' Nalini 12436 date:1/12/2011
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub grdLocations_PageIndexChanging(ByVal sender As Object, ByVal e As GridPageChangedEventArgs) Handles grdLocations.PageIndexChanged
            ''grdLocations.PageIndex = e.NewPageIndex
            LoadLocations()
        End Sub
        Protected Sub grdLocations_PageIndexChanging(ByVal sender As Object, ByVal e As GridPageSizeChangedEventArgs) Handles grdLocations.PageSizeChanged
            ''grdLocations.PageIndex = e.NewPageIndex
            LoadLocations()
        End Sub
        Protected Sub grdInstructors_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdInstructors.NeedDataSource
            LoadInstructors()
            ''grdStudents.Rebind()
        End Sub
        Protected Sub grdLocations_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdLocations.NeedDataSource
            LoadLocations()
            ''grdStudents.Rebind()
        End Sub
        Protected Sub grdPrerequisites_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdPrerequisites.NeedDataSource
            LoadPrerequisites()
            ''grdStudents.Rebind()
        End Sub
        Protected Sub grdSyllabus_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdSyllabus.NeedDataSource
            LoadSyllabus()
            ''grdStudents.Rebind()
        End Sub
        ''' <summary>
        ''' Added by Suvarna for IssueID - 13270 on Mar 11,2013
        ''' Function added to apply ascending sort on field
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub AddExpression()
            Dim ExpPrerequisitesSort As New GridSortExpression
            Dim ExpInstructorsSort As New GridSortExpression
            Dim ExpLocationsSort As New GridSortExpression
            Dim ExpSyllabusSort As New GridSortExpression
            ExpPrerequisitesSort.FieldName = "WebName"
            ExpPrerequisitesSort.SetSortOrder("Ascending")

            ExpInstructorsSort.FieldName = "Name"
            ExpInstructorsSort.SetSortOrder("Ascending")

            ExpLocationsSort.FieldName = "Name"
            ExpLocationsSort.SetSortOrder("Ascending")

            ExpSyllabusSort.FieldName = "WebName"
            ExpSyllabusSort.SetSortOrder("Ascending")

            grdPrerequisites.MasterTableView.SortExpressions.AddSortExpression(ExpPrerequisitesSort)
            grdInstructors.MasterTableView.SortExpressions.AddSortExpression(ExpInstructorsSort)
            grdLocations.MasterTableView.SortExpressions.AddSortExpression(ExpLocationsSort)
            grdSyllabus.MasterTableView.SortExpressions.AddSortExpression(ExpSyllabusSort)
        End Sub
    End Class
End Namespace