'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Namespace Aptify.Framework.Web.eBusiness.Education
    Partial Class InstructorCenterControl
        Inherits eBusiness.BaseUserControlAdvanced

        Protected Const ATTRIBUTE_INSTRUCTOR_CLASS_LIST_PAGE As String = "InstructorClassListPage"
        Protected Const ATTRIBUTE_INSTRUCTOR_STUDENT_LIST_PAGE As String = "InstructorStudentListPage"
        Protected Const ATTRIBUTE_INSTRUCTOR_AUTHORIZED_COURSES_PAGE As String = "InstructorAuthorizedCoursesPage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "InstructorCenter"

#Region "InstructorCenter Specific Properties"
        ''' <summary>
        ''' InstructorClassList page url
        ''' </summary>
        Public Overridable Property InstructorClassListPage() As String
            Get
                If Not ViewState(ATTRIBUTE_INSTRUCTOR_CLASS_LIST_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_INSTRUCTOR_CLASS_LIST_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_INSTRUCTOR_CLASS_LIST_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' InstructorStudentList page url
        ''' </summary>
        Public Overridable Property InstructorStudentListPage() As String
            Get
                If Not ViewState(ATTRIBUTE_INSTRUCTOR_STUDENT_LIST_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_INSTRUCTOR_STUDENT_LIST_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_INSTRUCTOR_STUDENT_LIST_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' InstructorAuthorizedCourses page url
        ''' </summary>
        Public Overridable Property InstructorAuthorizedCoursesPage() As String
            Get
                If Not ViewState(ATTRIBUTE_INSTRUCTOR_AUTHORIZED_COURSES_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_INSTRUCTOR_AUTHORIZED_COURSES_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_INSTRUCTOR_AUTHORIZED_COURSES_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
#End Region

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'set control properties from XML file if needed
            SetProperties()
            If Not IsPostBack Then
                CheckInstructor()
            End If
        End Sub

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(InstructorClassListPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                InstructorClassListPage = Me.GetLinkValueFromXML(ATTRIBUTE_INSTRUCTOR_CLASS_LIST_PAGE)
                If String.IsNullOrEmpty(InstructorClassListPage) Then
                    Me.lnkInstructorClasses.Enabled = False
                    Me.lnkInstructorClasses.ToolTip = "InstructorClassListPage property has not been set."
                Else
                    Me.lnkInstructorClasses.NavigateUrl = InstructorClassListPage
                End If
            Else
                Me.lnkInstructorClasses.NavigateUrl = InstructorClassListPage
            End If

            If String.IsNullOrEmpty(InstructorStudentListPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                InstructorStudentListPage = Me.GetLinkValueFromXML(ATTRIBUTE_INSTRUCTOR_STUDENT_LIST_PAGE)
                If String.IsNullOrEmpty(InstructorStudentListPage) Then
                    Me.lnkInstructorStudents.Enabled = False
                    Me.lnkInstructorStudents.ToolTip = "InstructorStudentListPage property has not been set."
                Else
                    Me.lnkInstructorStudents.NavigateUrl = InstructorStudentListPage
                End If
            Else
                Me.lnkInstructorStudents.NavigateUrl = InstructorStudentListPage
            End If

            If String.IsNullOrEmpty(InstructorAuthorizedCoursesPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                InstructorAuthorizedCoursesPage = Me.GetLinkValueFromXML(ATTRIBUTE_INSTRUCTOR_AUTHORIZED_COURSES_PAGE)
                If String.IsNullOrEmpty(InstructorAuthorizedCoursesPage) Then
                    Me.lnkInstructorAuthCourses.Enabled = False
                    Me.lnkInstructorAuthCourses.ToolTip = "InstructorAuthorizedCoursesPage property has not been set."
                Else
                    Me.lnkInstructorAuthCourses.NavigateUrl = InstructorAuthorizedCoursesPage
                End If
            Else
                Me.lnkInstructorAuthCourses.NavigateUrl = InstructorAuthorizedCoursesPage
            End If

        End Sub

        Private Sub CheckInstructor()
            ' determine if the individual logged in is an active instructor
            ' on any course. If so, show the InstructorCenter link.
            If Not InstructorValidator1.IsCurrentUserInstructor() Then
                Me.tblMain.Visible = False
                'Response.Redirect("/SecurityError.aspx?Message=The+requested+page+is+only+available+for+course+instructors")
            End If
        End Sub
    End Class
End Namespace
