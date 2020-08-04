'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Namespace Aptify.Framework.Web.eBusiness.Education
    Partial Class EducationCenter
        Inherits eBusiness.BaseUserControlAdvanced

        Protected Const ATTRIBUTE_REGISTERED_COURSES_IMAGE_URL As String = "RegisteredCoursesImage"
        Protected Const ATTRIBUTE_REGISTERED_COURSES_PAGE As String = "RegisteredCoursesPage"
        Protected Const ATTRIBUTE_REGISTERED_CERTIFICATIONS_IMAGE_URL As String = "RegisteredCertificationsImage"
        Protected Const ATTRIBUTE_REGISTERED_CERTIFICATIONS_PAGE As String = "RegisteredCertificationsPage"
        Protected Const ATTRIBUTE_COURSE_CATALOG_IMAGE_URL As String = "CourseCatalogImage"
        Protected Const ATTRIBUTE_COURSE_CATALOG_PAGE As String = "CourseCatalogPage"
        Protected Const ATTRIBUTE_CLASS_SCHEDULE_IMAGE_URL As String = "ClassScheduleImage"
        Protected Const ATTRIBUTE_CLASS_SCHEDULE_PAGE As String = "ClassSchedulePage"
        Protected Const ATTRIBUTE_CURRICULA_IMAGE_URL As String = "CurriculaImage"
        Protected Const ATTRIBUTE_CURRICULA_PAGE As String = "CurriculaPage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "EducationCenter"

#Region "EducationCenter Specific Properties"
        ''' <summary>
        ''' RegisteredCoursesImage url
        ''' </summary>
        Public Overridable Property RegisteredCoursesImage() As String
            Get
                If Not ViewState(ATTRIBUTE_REGISTERED_COURSES_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_REGISTERED_COURSES_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_REGISTERED_COURSES_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' RegisteredCourses page url
        ''' </summary>
        Public Overridable Property RegisteredCoursesPage() As String
            Get
                If Not ViewState(ATTRIBUTE_REGISTERED_COURSES_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_REGISTERED_COURSES_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_REGISTERED_COURSES_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' RegisteredCertificationsImage url
        ''' </summary>
        Public Overridable Property RegisteredCertificationsImage() As String
            Get
                If Not ViewState(ATTRIBUTE_REGISTERED_CERTIFICATIONS_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_REGISTERED_CERTIFICATIONS_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_REGISTERED_CERTIFICATIONS_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' RegisteredCertifications page url
        ''' </summary>
        Public Overridable Property RegisteredCertificationsPage() As String
            Get
                If Not ViewState(ATTRIBUTE_REGISTERED_CERTIFICATIONS_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_REGISTERED_CERTIFICATIONS_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_REGISTERED_CERTIFICATIONS_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' CourseCatalogImage url
        ''' </summary>
        Public Overridable Property CourseCatalogImage() As String
            Get
                If Not ViewState(ATTRIBUTE_COURSE_CATALOG_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_COURSE_CATALOG_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_COURSE_CATALOG_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
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
        ''' ClassScheduleImage url
        ''' </summary>
        Public Overridable Property ClassScheduleImage() As String
            Get
                If Not ViewState(ATTRIBUTE_CLASS_SCHEDULE_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_CLASS_SCHEDULE_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_CLASS_SCHEDULE_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' ClassSchedule page url
        ''' </summary>
        Public Overridable Property ClassSchedulePage() As String
            Get
                If Not ViewState(ATTRIBUTE_CLASS_SCHEDULE_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_CLASS_SCHEDULE_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_CLASS_SCHEDULE_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' CurriculaImage url
        ''' </summary>
        Public Overridable Property CurriculaImage() As String
            Get
                If Not ViewState(ATTRIBUTE_CURRICULA_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_CURRICULA_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_CURRICULA_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' Curricula page url
        ''' </summary>
        Public Overridable Property CurriculaPage() As String
            Get
                If Not ViewState(ATTRIBUTE_CURRICULA_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_CURRICULA_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_CURRICULA_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
#End Region

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
            If Not IsPostBack Then
                SetupPage()
            End If
        End Sub

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(RegisteredCoursesImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                RegisteredCoursesImage = Me.GetLinkValueFromXML(ATTRIBUTE_REGISTERED_COURSES_IMAGE_URL)
                Me.imgMyCouses.Src = RegisteredCoursesImage
            End If
            If String.IsNullOrEmpty(RegisteredCoursesPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                RegisteredCoursesPage = Me.GetLinkValueFromXML(ATTRIBUTE_REGISTERED_COURSES_PAGE)
                If String.IsNullOrEmpty(RegisteredCoursesPage) Then
                    Me.lnkMyCourses.Enabled = False
                    Me.lnkMyCourses.ToolTip = "RegisteredCoursesPage property has not been set."
                Else
                    Me.lnkMyCourses.NavigateUrl = RegisteredCoursesPage
                End If
            Else
                Me.lnkMyCourses.NavigateUrl = RegisteredCoursesPage
            End If

            If String.IsNullOrEmpty(RegisteredCertificationsImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                RegisteredCertificationsImage = Me.GetLinkValueFromXML(ATTRIBUTE_REGISTERED_CERTIFICATIONS_IMAGE_URL)
                Me.imgMyCerts.Src = RegisteredCertificationsImage
            End If
            If String.IsNullOrEmpty(RegisteredCertificationsPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                RegisteredCertificationsPage = Me.GetLinkValueFromXML(ATTRIBUTE_REGISTERED_CERTIFICATIONS_PAGE)
                If String.IsNullOrEmpty(RegisteredCertificationsPage) Then
                    Me.lnkMyCerts.Enabled = False
                    Me.lnkMyCerts.ToolTip = "RegisteredCertificationsPage property has not been set."
                Else
                    Me.lnkMyCerts.NavigateUrl = RegisteredCertificationsPage
                End If
            Else
                Me.lnkMyCerts.NavigateUrl = RegisteredCertificationsPage
            End If

            If String.IsNullOrEmpty(CourseCatalogImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                CourseCatalogImage = Me.GetLinkValueFromXML(ATTRIBUTE_COURSE_CATALOG_IMAGE_URL)
                Me.imgCatalog.Src = CourseCatalogImage
            End If
            If String.IsNullOrEmpty(CourseCatalogPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                CourseCatalogPage = Me.GetLinkValueFromXML(ATTRIBUTE_COURSE_CATALOG_PAGE)
                If String.IsNullOrEmpty(CourseCatalogPage) Then
                    Me.lnkCatalog.Enabled = False
                    Me.lnkCatalog.ToolTip = "CourseCatalogPage property has not been set."
                Else
                    Me.lnkCatalog.NavigateUrl = CourseCatalogPage
                End If
            Else
                Me.lnkCatalog.NavigateUrl = CourseCatalogPage
            End If

            If String.IsNullOrEmpty(ClassScheduleImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                ClassScheduleImage = Me.GetLinkValueFromXML(ATTRIBUTE_CLASS_SCHEDULE_IMAGE_URL)
                Me.imgClassSchedule.Src = ClassScheduleImage
            End If
            If String.IsNullOrEmpty(ClassSchedulePage) Then
                'since value is the 'default' check the XML file for possible custom setting
                ClassSchedulePage = Me.GetLinkValueFromXML(ATTRIBUTE_CLASS_SCHEDULE_PAGE)
                If String.IsNullOrEmpty(ClassSchedulePage) Then
                    Me.lnkClassSchedule.Enabled = False
                    Me.lnkClassSchedule.ToolTip = "ClassSchedulePage property has not been set."
                Else
                    Me.lnkClassSchedule.NavigateUrl = ClassSchedulePage
                End If
            Else
                Me.lnkClassSchedule.NavigateUrl = ClassSchedulePage
            End If

            If String.IsNullOrEmpty(CurriculaImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                CurriculaImage = Me.GetLinkValueFromXML(ATTRIBUTE_CURRICULA_IMAGE_URL)
                Me.imgCurricula.Src = CurriculaImage
            End If
            If String.IsNullOrEmpty(CurriculaPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                CurriculaPage = Me.GetLinkValueFromXML(ATTRIBUTE_CURRICULA_PAGE)
                If String.IsNullOrEmpty(CurriculaPage) Then
                    Me.lnkCurricula.Enabled = False
                    Me.lnkCurricula.ToolTip = "CurriculaPage property has not been set."
                Else
                    Me.lnkCurricula.NavigateUrl = CurriculaPage
                End If
            Else
                Me.lnkCurricula.NavigateUrl = CurriculaPage
            End If
        End Sub

        Private Sub SetupPage()
            ' determine if the individual logged in is an active instructor
            ' on any course. If so, show the InstructorCenter link.
            If InstructorValidator1.IsCurrentUserInstructor() Then
                trInstructorCenter.Visible = True
            Else
                trInstructorCenter.Visible = False
            End If
        End Sub
    End Class
End Namespace
