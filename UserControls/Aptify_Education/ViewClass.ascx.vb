'Aptify e-Business 5.5.1/LMS 5.5.1, June 2014
Imports System.Data
Imports Telerik.Web.UI
Imports Aptify.Framework.BusinessLogic.GenericEntity

Namespace Aptify.Framework.Web.eBusiness.Education
    Partial Class ViewClassControl
        Inherits eBusiness.BaseUserControlAdvanced

        '(LEFT NAV) attributes
        Protected Const ATTRIBUTE_CLASS_SCHEDULE_IMAGE_URL As String = "ClassScheduleImage"
        Protected Const ATTRIBUTE_LEFT_NAV_GENERAL_IMAGE_URL As String = "LeftNavGeneralImage"
        Protected Const ATTRIBUTE_LEFT_NAV_INSTRUCTOR_INFO_IMAGE_URL As String = "LeftNavInstrutorInfoImage"
        Protected Const ATTRIBUTE_LEFT_NAV_SYLLABUS_IMAGE_URL As String = "LeftNavSyllabusImage"
        Protected Const ATTRIBUTE_LEFT_NAV_NOTES_IMAGE_URL As String = "LeftNavNotesImage"
        Protected Const ATTRIBUTE_LEFT_NAV_FORUMS_IMAGE_URL As String = "LeftNavForumsImage"
        Protected Const ATTRIBUTE_LEFT_NAV_DOCUMENTS_IMAGE_URL As String = "LeftNavDocumentsImage"
        Protected Const ATTRIBUTE_LEFT_NAV_STUDENTS_IMAGE_URL As String = "LeftNavStudentsImage"
        Protected Const ATTRIBUTE_LEFT_NAV_REGISTER_IMAGE_URL As String = "LeftNavRegisterImage"
        Protected Const ATTRIBUTE_LEFT_NAV_REGISTER_PAGE As String = "LeftNavRegisterPage"
        '(RIGHT DISPLAY) attributes
        Protected Const ATTRIBUTE_DISPLAY_GENERAL_IMAGE_URL As String = "DisplayGeneralImage"
        Protected Const ATTRIBUTE_DISPLAY_SYLLABUS_IMAGE_URL As String = "DisplaySyllabusImage"
        Protected Const ATTRIBUTE_DISPLAY_STUDENTS_IMAGE_URL As String = "DisplayStudentsImage"
        Protected Const ATTRIBUTE_DISPLAY_INSTRUCTOR_IMAGE_URL As String = "DisplayInstructorImage"
        Protected Const ATTRIBUTE_DISPLAY_FORUMS_IMAGE_URL As String = "DisplayForumsImage"
        Protected Const ATTRIBUTE_DISPLAY_DOCUMENTS_IMAGE_URL As String = "DisplayDocumentsImage"
        Protected Const ATTRIBUTE_DISPLAY_NOTES_IMAGE_URL As String = "DisplayNotesImage"
        '(GRID DISPLAY) attributes
        Protected Const ATTRIBUTE_DISPLAY_GRID_SYLLABUS_IMAGE_URL As String = "DisplayGridSyllabusImage"
        Protected Const ATTRIBUTE_DISPLAY_GRID_STUDENT_IMAGE_URL As String = "DisplayGridStudentImage"
        Protected Const ATTRIBUTE_DISPLAY_GRID_INSTRUCTOR_NOTES_IMAGE_URL As String = "DisplayGridInstructorNotesImage"
        'control name
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "ViewClass"
        ''DataTable attribute
        Protected Const ATTRIBUTE_DATATABLE_SYLLABUS As String = "dtSyllabus"
        Protected Const ATTRIBUTE_DATATABLE_STUDENT As String = "dtStudent"


        Private m_sView As String

#Region "ViewClass: (RIGHT DISPLAY) & Display Grid Properties"
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
        ''' DisplayGridStudentImage url
        ''' </summary>
        Public Overridable Property DisplayGridStudentImage() As String
            Get
                If Not ViewState(ATTRIBUTE_DISPLAY_GRID_STUDENT_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_DISPLAY_GRID_STUDENT_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_DISPLAY_GRID_STUDENT_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' DisplayGridInstructorNotesImage url
        ''' </summary>
        Public Overridable Property DisplayGridInstructorNotesImage() As String
            Get
                If Not ViewState(ATTRIBUTE_DISPLAY_GRID_INSTRUCTOR_NOTES_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_DISPLAY_GRID_INSTRUCTOR_NOTES_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_DISPLAY_GRID_INSTRUCTOR_NOTES_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
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
        ''' DisplayStudentsImage url
        ''' </summary>
        Public Overridable Property DisplayStudentsImage() As String
            Get
                If Not ViewState(ATTRIBUTE_DISPLAY_STUDENTS_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_DISPLAY_STUDENTS_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_DISPLAY_STUDENTS_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
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
        ''' DisplayForumsImage url
        ''' </summary>
        Public Overridable Property DisplayForumsImage() As String
            Get
                If Not ViewState(ATTRIBUTE_DISPLAY_FORUMS_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_DISPLAY_FORUMS_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_DISPLAY_FORUMS_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' DisplayDocumentsImage url
        ''' </summary>
        Public Overridable Property DisplayDocumentsImage() As String
            Get
                If Not ViewState(ATTRIBUTE_DISPLAY_DOCUMENTS_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_DISPLAY_DOCUMENTS_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_DISPLAY_DOCUMENTS_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' DisplayNotesImage url
        ''' </summary>
        Public Overridable Property DisplayNotesImage() As String
            Get
                If Not ViewState(ATTRIBUTE_DISPLAY_NOTES_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_DISPLAY_NOTES_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_DISPLAY_NOTES_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
#End Region

#Region "ViewClass: (LEFT NAV) Properties"
        ''' <summary>
        ''' LeftNavDocumentsImage url
        ''' </summary>
        Public Overridable Property LeftNavRegisterImage() As String
            Get
                If Not ViewState(ATTRIBUTE_LEFT_NAV_REGISTER_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_LEFT_NAV_REGISTER_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_LEFT_NAV_REGISTER_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' LeftNavRegister page url
        ''' </summary>
        Public Overridable Property LeftNavRegisterPage() As String
            Get
                If Not ViewState(ATTRIBUTE_LEFT_NAV_REGISTER_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_LEFT_NAV_REGISTER_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_LEFT_NAV_REGISTER_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' LeftNavDocumentsImage url
        ''' </summary>
        Public Overridable Property LeftNavDocumentsImage() As String
            Get
                If Not ViewState(ATTRIBUTE_LEFT_NAV_DOCUMENTS_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_LEFT_NAV_DOCUMENTS_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_LEFT_NAV_DOCUMENTS_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' LeftNavStudentsImage url
        ''' </summary>
        Public Overridable Property LeftNavStudentsImage() As String
            Get
                If Not ViewState(ATTRIBUTE_LEFT_NAV_STUDENTS_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_LEFT_NAV_STUDENTS_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_LEFT_NAV_STUDENTS_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' LeftNavForumsImage url
        ''' </summary>
        Public Overridable Property LeftNavForumsImage() As String
            Get
                If Not ViewState(ATTRIBUTE_LEFT_NAV_FORUMS_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_LEFT_NAV_FORUMS_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_LEFT_NAV_FORUMS_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' LeftNavNotesImage url
        ''' </summary>
        Public Overridable Property LeftNavNotesImage() As String
            Get
                If Not ViewState(ATTRIBUTE_LEFT_NAV_NOTES_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_LEFT_NAV_NOTES_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_LEFT_NAV_NOTES_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' LeftNavInstrutorInfoImage url
        ''' </summary>
        Public Overridable Property LeftNavInstrutorInfoImage() As String
            Get
                If Not ViewState(ATTRIBUTE_LEFT_NAV_INSTRUCTOR_INFO_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_LEFT_NAV_INSTRUCTOR_INFO_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_LEFT_NAV_INSTRUCTOR_INFO_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
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
        ''' LeftNavGeneralImage url
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
#End Region

#Region "ReadOnly Properties"
        Public Overridable ReadOnly Property IsRegisteredForClass() As Boolean
            Get
                Return CBool(ViewState("IsRegistered"))
            End Get
        End Property
        Public Overridable ReadOnly Property ClassRegistrationID() As Long
            Get
                If ViewState("ClassRegistrationID") Is Nothing Then
                    Return -1
                Else
                    Return CLng(ViewState("ClassRegistrationID"))
                End If
            End Get
        End Property
        Public Overridable ReadOnly Property ClassRegistrationStatus() As String
            Get
                Return CStr(ViewState("ClassRegistrationStatus"))
            End Get
        End Property
        Public Overridable ReadOnly Property ClassRegistrationDateAvailable() As DateTime
            Get
                Return CDate(ViewState("ClassRegistrationDateAvailable"))
            End Get
        End Property
        Public Overridable ReadOnly Property ClassRegistrationDateExpires() As DateTime
            Get
                If Not ViewState("ClassRegistrationDateExpires") Is Nothing Then
                    Return CDate(ViewState("ClassRegistrationDateExpires"))
                Else
                    Return Nothing
                End If
            End Get
        End Property
        Public Overridable ReadOnly Property IsInstructor() As Boolean
            Get
                Return CBool(ViewState("IsInstructor"))
            End Get
        End Property
#End Region

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'Response.Cache.SetCacheability(HttpCacheability.NoCache)
            SetProperties()
            If Not IsPostBack Then
                AddExpression()
                'set control properties from XML file if needed
                If ValidateClassRegistration() Then
                    ViewState("IsRegistered") = "1"
                Else
                    If InstructorValidator1.ValidateClassInstructor(CLng(Request.QueryString("ClassID"))) Then
                        ViewState("IsInstructor") = "1"
                    Else
                        ViewState("IsInstructor") = "0"
                    End If
                    ViewState("IsRegistered") = "0"
                End If
                LoadClass()
            End If

        End Sub

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            '(GRID DISPLAY)
            If String.IsNullOrEmpty(DisplayGridSyllabusImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                DisplayGridSyllabusImage = Me.GetLinkValueFromXML(ATTRIBUTE_DISPLAY_GRID_SYLLABUS_IMAGE_URL)
            End If
            If String.IsNullOrEmpty(DisplayGridStudentImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                DisplayGridStudentImage = Me.GetLinkValueFromXML(ATTRIBUTE_DISPLAY_GRID_STUDENT_IMAGE_URL)
            End If
            If String.IsNullOrEmpty(DisplayGridInstructorNotesImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                DisplayGridInstructorNotesImage = Me.GetLinkValueFromXML(ATTRIBUTE_DISPLAY_GRID_INSTRUCTOR_NOTES_IMAGE_URL)
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
            If String.IsNullOrEmpty(DisplayStudentsImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                DisplayStudentsImage = Me.GetLinkValueFromXML(ATTRIBUTE_DISPLAY_STUDENTS_IMAGE_URL)
            End If
            If String.IsNullOrEmpty(DisplayInstructorImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                DisplayInstructorImage = Me.GetLinkValueFromXML(ATTRIBUTE_DISPLAY_INSTRUCTOR_IMAGE_URL)
            End If
            If String.IsNullOrEmpty(DisplayForumsImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                DisplayForumsImage = Me.GetLinkValueFromXML(ATTRIBUTE_DISPLAY_FORUMS_IMAGE_URL)
            End If
            If String.IsNullOrEmpty(DisplayDocumentsImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                DisplayDocumentsImage = Me.GetLinkValueFromXML(ATTRIBUTE_DISPLAY_DOCUMENTS_IMAGE_URL)
            End If
            If String.IsNullOrEmpty(DisplayNotesImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                DisplayNotesImage = Me.GetLinkValueFromXML(ATTRIBUTE_DISPLAY_NOTES_IMAGE_URL)
                imgInstrutorNotes.Src = DisplayNotesImage
            End If
            '(LEFT NAV)
            If String.IsNullOrEmpty(ClassScheduleImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                ClassScheduleImage = Me.GetLinkValueFromXML(ATTRIBUTE_CLASS_SCHEDULE_IMAGE_URL)
                imgSchedule.Src = ClassScheduleImage
            End If
            If String.IsNullOrEmpty(LeftNavGeneralImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                LeftNavGeneralImage = Me.GetLinkValueFromXML(ATTRIBUTE_LEFT_NAV_GENERAL_IMAGE_URL)
                imgGenInfoSmall.Src = LeftNavGeneralImage
            End If
            If String.IsNullOrEmpty(LeftNavInstrutorInfoImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                LeftNavInstrutorInfoImage = Me.GetLinkValueFromXML(ATTRIBUTE_LEFT_NAV_INSTRUCTOR_INFO_IMAGE_URL)
                imgInstructorSmall.Src = LeftNavInstrutorInfoImage
            End If
            If String.IsNullOrEmpty(LeftNavSyllabusImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                LeftNavSyllabusImage = Me.GetLinkValueFromXML(ATTRIBUTE_LEFT_NAV_SYLLABUS_IMAGE_URL)
                imgSyllabusSmall.Src = LeftNavSyllabusImage
            End If
            If String.IsNullOrEmpty(LeftNavNotesImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                LeftNavNotesImage = Me.GetLinkValueFromXML(ATTRIBUTE_LEFT_NAV_NOTES_IMAGE_URL)
                imgNotesSmall.Src = LeftNavNotesImage
            End If
            If String.IsNullOrEmpty(LeftNavForumsImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                LeftNavForumsImage = Me.GetLinkValueFromXML(ATTRIBUTE_LEFT_NAV_FORUMS_IMAGE_URL)
                imgForumSmall.Src = LeftNavForumsImage
            End If
            If String.IsNullOrEmpty(LeftNavDocumentsImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                LeftNavDocumentsImage = Me.GetLinkValueFromXML(ATTRIBUTE_LEFT_NAV_DOCUMENTS_IMAGE_URL)
                imgDocumentSmall.Src = LeftNavDocumentsImage
            End If
            If String.IsNullOrEmpty(LeftNavStudentsImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                LeftNavStudentsImage = Me.GetLinkValueFromXML(ATTRIBUTE_LEFT_NAV_STUDENTS_IMAGE_URL)
                imgStudentSmall.Src = LeftNavStudentsImage
            End If
            If String.IsNullOrEmpty(LeftNavRegisterImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                LeftNavRegisterImage = Me.GetLinkValueFromXML(ATTRIBUTE_LEFT_NAV_REGISTER_IMAGE_URL)
                imgRegisterSmall.Src = LeftNavRegisterImage
                imgRegisterSmall2.Src = LeftNavRegisterImage
            End If
            If String.IsNullOrEmpty(LeftNavRegisterPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                LeftNavRegisterPage = Me.GetLinkValueFromXML(ATTRIBUTE_LEFT_NAV_REGISTER_PAGE)
                If String.IsNullOrEmpty(LeftNavRegisterPage) Then
                    lnkRegister.Enabled = False
                    lnkRegister.ToolTip = "LeftNavRegisterPage property has not been set."
                Else
                    lnkRegister.NavigateUrl = LeftNavRegisterPage
                End If
            Else
                lnkRegister.NavigateUrl = LeftNavRegisterPage
            End If

        End Sub

        Protected Overridable Function ValidateClassRegistration() As Boolean
            Dim sSQL As String, dt As DataTable
            Try
                ' sSQL = "SELECT TOP 1 ID, Status, DateAvailable, ISNULL(DateExpires,'') DateExpires FROM " & _
                '        AptifyApplication.GetEntityBaseDatabase("Class Registrations") & _
                '       "..vwClassRegistrations WHERE ClassID=" & _
                '      Request.QueryString("ClassID") & _
                '     " AND StudentID=" & Me.InstructorValidator1.User.PersonID & _
                '    " ORDER BY DateRegistered DESC "
                'Sheetal 16/07/15: For Issue 21368 :Prevent Sql Injection Attack: Used parameterrized sql is best practice

                Dim params(1) As IDataParameter
                params(0) = Me.DataAction.GetDataParameter("@ClassID", SqlDbType.Int, Request.QueryString("ClassID"))
                params(1) = Me.DataAction.GetDataParameter("@StudentID", SqlDbType.Int, Me.InstructorValidator1.User.PersonID)

                sSQL = Me.AptifyApplication.GetEntityBaseDatabase("Courses") & "..spValidateClassRegistration"
                dt = Me.DataAction.GetDataTableParametrized(sSQL, CommandType.StoredProcedure, params)

                ' dt = Me.DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then ' Modified by Vijay Soni for Issue#5586 on Nov 14, 2007.
                    With dt.Rows(0)
                        trStudentStatus.Visible = True
                        ViewState("ClassRegistrationID") = .Item("ID")
                        ViewState("ClassRegistrationStatus") = .Item("Status")
                        ViewState("ClassRegistrationDateAvailable") = .Item("DateAvailable")
                        ViewState("ClassRegistrationDateExpires") = .Item("DateExpires")
                        If CLng(ViewState("ClassRegistrationID")) > 0 Then
                            Select Case CStr(ViewState("ClassRegistrationStatus")).Trim.ToUpper
                                Case "REGISTERED", "IN-PROGRESS"
                                    If CDate(.Item("DateAvailable")) <= Now Then
                                        If Not IsDBNull(.Item("DateExpires")) OrElse _
                                           Not CDate(.Item("DateExpires")).Year = 1900 OrElse _
                                           CDate(.Item("DateExpires")) >= Now Then
                                            lblStudentStatus.Text = "Student Status: Currently Registered"
                                            lblStudentStatus.ForeColor = Drawing.Color.DarkGreen
                                            '2007/12/14 MAS: Display Class Registration Start/End
                                            'EndDate may be NULL, or the date equivilent of 1900/01/01 - only display RegisterDates if valid expiry exists.
                                            If Not .Item("DateExpires") Is Nothing _
                                                    AndAlso Not IsDBNull(.Item("DateExpires")) _
                                                    AndAlso CDate(.Item("DateExpires")).Year <> 1900 Then
                                                If CDate(.Item("DateAvailable")) <> CDate("1/1/1900") AndAlso _
                                                                                                   .Item("DateAvailable").ToString <> "" Then
                                                    lblRegisterDates.Text = "  Registered: " & CDate(.Item("DateAvailable")).ToShortDateString & " - " & CDate(.Item("DateExpires")).ToShortDateString
                                                Else
                                                    lblRegisterDates.Text = "  Registration ends: " & CDate(.Item("DateExpires")).ToShortDateString
                                                End If
                                                lblRegisterDates.Visible = True
                                            End If
                                            Return True
                                        Else
                                            lblStudentStatus.Text = "Student Status: Registration Expired"
                                            lblStudentStatus.ForeColor = Drawing.Color.DarkRed
                                            ViewState("ClassRegistrationStatus") = .Item("Status")
                                        End If
                                    Else
                                        lblStudentStatus.Text = "Student Status: Future Registration - " & _
                                                                "Class Available on " & _
                                                                CDate(.Item("DateAvailable")).ToShortDateString
                                        lblStudentStatus.ForeColor = Drawing.Color.DarkBlue
                                    End If
                                Case Else
                                    lblStudentStatus.Text = "Student Status: " & .Item("Status").ToString & " - " & _
                                                            "Class Available on " & _
                                                            CDate(.Item("DateAvailable")).ToShortDateString
                                    lblStudentStatus.ForeColor = Drawing.Color.DarkRed
                            End Select
                        End If
                    End With
                End If
                Return False
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return False
            End Try
        End Function

        Protected Overridable Sub LoadClass()
            Try
                Dim sWebErrorMsg As String = ""
                'Issue 4895 - 2007-04-25
                'The Grid displays all Courses that meet the criteria included in the database query created in this method.
                'Because Courses may have ScopeFilters applied to them (if using Aptify sp3 or greater)
                'the code below handles both scenarios (if on sp3+ or not) by using two different
                'queries and two different grids. This code can be simplified by removing all pre-sp2
                'specific code and the grdCourses datagrid after upgrading to sp3.
                Dim sSQL As String, dt As Data.DataTable
                Dim iInstructorID As Long
                'The query to use will depend on if Courses has the Filter fields (sp3 or later)
                Dim bFilterFields As Boolean = False
                Dim ApplyFilterField As Aptify.Framework.BusinessLogic.GenericEntity.AptifyDataFieldBase
                ApplyFilterField = Me.AptifyApplication.GetEntityObject("Courses", -1).Fields.Item("ApplyFilterRule")
                If ApplyFilterField IsNot Nothing Then
                    bFilterFields = True
                End If


                '  If bFilterFields Then
                'sp3+ query:
                'The Courses Entity has the FilterRule fields, so we will add them to the query
                'sSQL = "SELECT ID, WebName, Category, CategoryID, ApplyFilterRule, ISNULL(ScopeFilterRuleID,-1) 'ScopeFilterRuleID' , " & _

                'Sheetal 16/07/15: For Issue 21368 :Prevent Sql Injection Attack: Used parameterrized sql is best practice
                'sSQL = "SELECT c.*,cr.CategoryID,cr.Category, cr.ApplyFilterRule, ISNULL(cr.ScopeFilterRuleID,-1) 'ScopeFilterRuleID' FROM " & _
                '       Me.AptifyApplication.GetEntityBaseDatabase("Classes") & _
                '       "..vwClasses c INNER JOIN " & _
                '       Me.AptifyApplication.GetEntityBaseDatabase("Classes") & _
                '       "..vwCourses cr ON c.CourseID=cr.ID WHERE c.ID=" & _
                '       Request.QueryString("ClassID")

                ' Else
                'pre-sp3 query:
                'sSQL = "SELECT c.*,cr.CategoryID,cr.Category FROM " & _
                '       Me.AptifyApplication.GetEntityBaseDatabase("Classes") & _
                '       "..vwClasses c INNER JOIN " & _
                '       Me.AptifyApplication.GetEntityBaseDatabase("Classes") & _
                '       "..vwCourses cr ON c.CourseID=cr.ID WHERE c.ID=" & _
                '       Request.QueryString("ClassID")

                ' End If
                'Sheetal 16/07/15: For Issue 21368 :Prevent Sql Injection Attack: Used parameterrized sql is best practice
                Dim params(1) As IDataParameter
                params(0) = Me.DataAction.GetDataParameter("@ClassID", SqlDbType.Int, Request.QueryString("ClassID"))
                params(1) = Me.DataAction.GetDataParameter("@bFilter", SqlDbType.Bit, bFilterFields)

                sSQL = Me.AptifyApplication.GetEntityBaseDatabase("Classes") & "..spLoadClass"
                dt = Me.DataAction.GetDataTableParametrized(sSQL, CommandType.StoredProcedure, params)
                'dt = Me.DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                If dt IsNot Nothing AndAlso _
                   dt.Rows.Count > 0 Then
                    With dt.Rows(0)
                        If CBool(.Item("WebEnabled")) Then

                            Dim bClassAllowed As Boolean = True
                            If bFilterFields Then
                                'Verify the logged in user meets the Course's Filter Rules for this Class
                                If CBool(.Item("ApplyFilterRule")) Then
                                    'declare a ScopeFilter object to use
                                    Dim oFilter As New Aptify.Applications.Education.EducationManagementScopeFilter(Me.AptifyApplication)
                                    If Not oFilter.EvaluateCourseFilter(CLng(.Item("ScopeFilterRuleID")), Me.User1.PersonID) Then
                                        bClassAllowed = False
                                    End If
                                End If
                            End If

                            If bClassAllowed Then
                                m_sView = "General"
                                lblName.Text = .Item("WebName").ToString.Trim
                                lblDescription.Text = .Item("WebDescription").ToString.Trim
                                'StartDate may be NULL, or the date equivilent of 1900/01/01
                                If .Item("StartDate") Is Nothing _
                                        OrElse IsDBNull(.Item("StartDate")) _
                                        OrElse CDate(.Item("StartDate")).Year = 1900 Then
                                    lblStartDate.Text = "Open"
                                Else
                                    lblStartDate.Text = CDate(.Item("StartDate")).ToShortDateString
                                End If
                                'EndDate may be NULL, or the date equivilent of 1900/01/01
                                If .Item("EndDate") Is Nothing _
                                        OrElse IsDBNull(.Item("EndDate")) _
                                        OrElse CDate(.Item("EndDate")).Year = 1900 Then
                                    lblEndDate.Text = "Open"
                                Else
                                    lblEndDate.Text = CDate(.Item("EndDate")).ToShortDateString
                                End If
                                If CBool(.Item("ShowInstructorInfo")) Then
                                    trInstructors.Visible = True
                                Else
                                    trInstructors.Visible = False
                                End If

                                SetupViewType(CBool(.Item("ShowInstructorInfo")))
                                Dim sComments As String = ""
                                If Not IsDBNull(.Item("InstructorComments")) Then
                                    sComments = CStr(.Item("InstructorComments"))
                                End If
                                ''RashmiP, Issue 11290 [Meeting/Class Integration] InstructorID = 0
                                If IsDBNull(.Item("InstructorID")) Then
                                    iInstructorID = 0
                                Else
                                    iInstructorID = CLng(.Item("InstructorID"))
                                End If
                                SetupView(CLng(iInstructorID), _
                                          CStr(.Item("WebName")), _
                                          sComments, _
                                          CStr(.Item("WebDetails")), _
                                          CLng(Request.QueryString("ClassID")), _
                                          CLng(.Item("ProductID")), _
                                          CStr(.Item("ProductType")))

                            Else
                                trContent.Visible = False
                                lblName.Text = "This is a Private Course that is unavailable for viewing."
                                lblName.ForeColor = Drawing.Color.Red
                                lblName.Font.Bold = True
                            End If
                        Else
                            trContent.Visible = False
                            lblName.Text = "This course is not available on the web."
                            lblName.ForeColor = Drawing.Color.Red
                            lblName.Font.Bold = True
                        End If
                    End With
                Else
                    trContent.Visible = False
                    trDescription.Visible = False
                    lblName.Text = "This course is not available on the web."
                    lblName.ForeColor = Drawing.Color.Red
                    lblName.Font.Bold = True
                End If
                'Neha changes for Issue 17773
                ViewState("ProductID") = dt.Rows(0).Item("ProductID")
                If Not ViewState("ProductID") Is Nothing AndAlso Me.ClassRegistrationID <= 0 Then
                    If GetWebPrerequisiteMsg(sWebErrorMsg) Then
                        lnkRegister.Visible = False
                        imgRegisterSmall.Visible = False
                        trRegisterMeeting.Visible = False
                        lblPrerequisiteCheck.ForeColor = Drawing.Color.Red
                        lblPrerequisiteCheck.Visible = True
                        lblPrerequisiteCheck.Text = sWebErrorMsg
                    End If
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                trContent.Visible = False
                trDescription.Visible = False
                lblName.Text = "Error Loading Course"
                lblName.ForeColor = Drawing.Color.Red
                lblName.Font.Bold = True
            End Try
        End Sub
        'Neha changes for Issue 17773,to show web pre-requisite message on viewclass page
        Private Function GetWebPrerequisiteMsg(ByRef sWebErrorMsg As String) As Boolean
            Dim oOrder As New Aptify.Applications.OrderEntry.OrdersEntity
            oOrder = ShoppingCart1.GetOrderObject(Page.Session, Page.User, Page.Application)
            Dim lProductID As Long = CLng(ViewState("ProductID"))
            Dim PrerequisitesOverridePromptResult As Microsoft.VisualBasic.MsgBoxResult
            Try
                If Not oOrder.ValidateProductPrerequisites(lProductID, 1, PrerequisitesOverridePromptResult) Then
                    sWebErrorMsg = CStr(oOrder.WebProdPreRequisiteErrMsg)
                    Return True
                Else
                    sWebErrorMsg = Nothing
                    Return False
                End If
            Catch ex As Exception
                Return False
            End Try
        End Function

        Private Sub SetupView(ByVal InstructorID As Long, ByVal WebName As String, ByVal InstructorComments As String, ByVal WebDetails As String, ByVal ClassID As Long, ByVal ProductID As Long, Optional ByVal ProductType As String = "")

            'HP Issue#9013:  get a handle to the product associated with this class in order to determine availability
            Dim p As Aptify.Applications.ProductSetup.ProductObject = _
            CType(Me.AptifyApplication.GetEntityObject("Products", ProductID), Aptify.Applications.ProductSetup.ProductObject)

            If IsRegisteredForClass Then
                trNotes.Visible = True
            Else
                trNotes.Visible = False
            End If
            If IsRegisteredForClass OrElse IsInstructor Then
                trForum.Visible = True
                trRegister.Visible = False
                trRegisterMeeting.Visible = False
                trDocuments.Visible = True
            Else
                'HP Issue#9013:  determine availability, if not available then adjust display accordingly
                If CStr(p.GetValue("AvailableUntil")) <> "" AndAlso CDate(p.GetValue("AvailableUntil")) < Date.Today Then
                    trRegister.Visible = False
                    trDocuments.Visible = False
                    trForum.Visible = False
                Else
                    'HP Issue#9013: product is available, determine price and adjust display accordingly
                    trForum.Visible = False
                    trDocuments.Visible = False
                    If Me.ClassRegistrationID <= 0 Then
                        trRegister.Visible = True
                        ''RashmiP, Issue 11290 [Meeting/Class Integration]
                        ''If ProductType is Meeting then Redirect to Meeting Registration
                        If ProductType = "Meeting" Then
                            trRegisterMeeting.Visible = True
                            trRegister.Visible = False
                            ViewState("ProductID") = CLng(ProductID)
                        Else
                            trRegister.Visible = True
                            trRegisterMeeting.Visible = False
                            lnkRegister.NavigateUrl &= "?ClassID=" & ClassID

                            'HP Issue#8598:  Pricing rule is working correctly however the ProductObject.GetPrice method does not have an Order object 
                            '                which in turn would have the objects, i.e. BillToPerson, that are required in the Filter Rule therefore no price is returned
                            '                and no price is displayed. In this situation we will extract the price thru the ShoppingCart's GetUserProductPrice method
                            'Dim p As Aptify.Applications.ProductSetup.ProductObject
                            'Dim pr As New Aptify.Applications.OrderEntry.IProductPrice.PriceInfo
                            'p = CType(Me.AptifyApplication.GetEntityObject("Products", ProductID), Aptify.Applications.ProductSetup.ProductObject)
                            'If p.GetPrice(pr, ProductID, 1, InstructorValidator1.User.PersonID, CurrencyTypeID:=InstructorValidator1.User.PreferredCurrencyTypeID) = Applications.OrderEntry.IProductPrice.PriceOutcome.Exist Then
                            '    lnkRegister.Text &= " - " & Format(pr.Price, InstructorValidator1.User.PreferredCurrencyFormat)
                            'End If

                            'End Issue#8598

                        End If
                        Dim price As Decimal = DirectCast(Me.ShoppingCart1.GetUserProductPrice(ProductID), Aptify.Applications.OrderEntry.IProductPrice.PriceInfo).Price
                        If price > 0 Then
                            lnkRegister.Text &= " - " & Format(price, InstructorValidator1.User.PreferredCurrencyFormat)
                            lnkRegisterMeeting.Text &= " - " & Format(price, InstructorValidator1.User.PreferredCurrencyFormat)
                        End If

                    Else
                        trRegister.Visible = False
                        trRegisterMeeting.Visible = False
                    End If
                End If
            End If
            If IsInstructor Then
                trStudents.Visible = True
            Else
                trStudents.Visible = False
            End If

            lnkGeneral.Font.Bold = False
            lnkInstructorInfo.Font.Bold = False
            lnkForum.Font.Bold = False
            lnkSyllabus.Font.Bold = False
            lblDetails.Visible = False
            lnkStudents.Font.Bold = False
            lnkDocuments.Font.Bold = False
            grdStudents.Visible = False
            grdSyllabus.Visible = False
            tblInstructor.Visible = False
            pnlForum.Visible = False
            pnlDocuments.Visible = False

            Select Case m_sView
                Case "General"
                    lnkGeneral.Font.Bold = True
                    lblDetails.Text = WebDetails
                    lblDetails.Visible = True
                    lblTitle.Text = "General Information"
                    imgTitle.ImageUrl = DisplayGeneralImage
                Case "Syllabus"
                    lnkSyllabus.Font.Bold = True
                    grdSyllabus.Visible = True
                    LoadSyllabus()
                    lblTitle.Text = "Class Syllabus"
                    imgTitle.ImageUrl = DisplaySyllabusImage
                Case "Students"
                    lnkStudents.Font.Bold = True
                    grdStudents.Visible = True
                    LoadStudents()
                    lblTitle.Text = "Student List"
                    imgTitle.ImageUrl = DisplayStudentsImage
                Case "Instructor"
                    lnkInstructorInfo.Font.Bold = True
                    tblInstructor.Visible = True
                    lblTitle.Text = "Instructor Information"
                    LoadInstructorInfo(InstructorID)
                    imgTitle.ImageUrl = DisplayInstructorImage
                    lblInstructorNotes.Text = InstructorComments
                    If IsInstructor Then
                        divEditInstructorNotes.Visible = True
                    Else
                        divEditInstructorNotes.Visible = False
                    End If
                Case "Forum"
                    lnkForum.Font.Bold = True
                    pnlForum.Visible = True
                    LoadForum()
                    lblTitle.Text = "Discussion Forum for " & WebName
                    imgTitle.ImageUrl = DisplayForumsImage
                Case "Documents"
                    lnkDocuments.Font.Bold = True
                    Me.pnlDocuments.Visible = True
                    If Me.IsInstructor Then
                        RecordAttachments.AllowAdd = True
                        RecordAttachments.AllowDelete = True
                    Else
                        RecordAttachments.AllowAdd = False
                        RecordAttachments.AllowDelete = False
                    End If
                    Me.RecordAttachments.LoadAttachments("Classes", CLng(Request.QueryString("ClassID")))
                    lblTitle.Text = "Documents for " & WebName
                    imgTitle.ImageUrl = DisplayDocumentsImage
                Case "Notes"
                    lnkNotes.Font.Bold = True
                    pnlNotes.Visible = True
                    LoadNotes()
                    lblTitle.Text = "Student Notes for " & WebName
                    lblDetails.Visible = True
                    lblDetails.Text = "The notes shown below are maintained to provide an area to record comments or thoughts related to this class for each student. Each student's notes are maintained privately and are not shared with the instructor or other students"
                    imgTitle.ImageUrl = DisplayNotesImage
            End Select

            Dim sBase As String = Me.Request.Url.AbsolutePath & "?ClassID=" & Request.QueryString("ClassID") & "&View="
            lnkGeneral.NavigateUrl = sBase & "General"
            lnkInstructorInfo.NavigateUrl = sBase & "Instructor"
            lnkSyllabus.NavigateUrl = sBase & "Syllabus"
            lnkForum.NavigateUrl = sBase & "Forum"
            lnkDocuments.NavigateUrl = sBase & "Documents"
            lnkNotes.NavigateUrl = sBase & "Notes"
            lnkStudents.NavigateUrl = sBase & "Students"
        End Sub
        Private Sub SetupViewType(ByVal ShowInstructor As Boolean)
            If Request.QueryString("View") IsNot Nothing Then
                Select Case Request.QueryString("View").Trim.ToUpper
                    Case "INSTRUCTOR"
                        If ShowInstructor Then
                            m_sView = "Instructor"
                        Else
                            m_sView = "General"
                        End If
                    Case "SYLLABUS"
                        m_sView = "Syllabus"
                    Case "STUDENTS"
                        If IsInstructor Then
                            m_sView = "Students"
                        Else
                            m_sView = "General"
                        End If
                    Case "NOTES"
                        If IsRegisteredForClass Then
                            m_sView = "Notes"
                        Else
                            m_sView = "General"
                        End If
                    Case "FORUM"
                        If IsRegisteredForClass Or IsInstructor Then
                            m_sView = "Forum"
                        Else
                            m_sView = "General" ' dont allow access
                        End If
                    Case "DOCUMENTS"
                        If IsRegisteredForClass Or IsInstructor Then
                            m_sView = "Documents"
                        Else
                            m_sView = "Documents" ' dont allow access
                        End If
                    Case Else
                        m_sView = "General"
                End Select
            End If
        End Sub
        Private Sub LoadInstructorInfo(ByVal InstructorID As Long)
            Dim sSQL As String, dt As Data.DataTable, sDB As String = AptifyApplication.GetEntityBaseDatabase("Persons")
            Try
                sSQL = " SELECT FirstLast, City, State, Country, Email1 FROM " & _
                       sDB & "..vwPersons WHERE ID=" & InstructorID
                dt = Me.DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                    With dt.Rows(0)
                        lblInstructor.Text = .Item("FirstLast").ToString.Trim
                        If Not IsDBNull(.Item("State")) AndAlso _
                           Len(.Item("State")) > 0 Then
                            lblInstructorLocation.Text = .Item("City").ToString.Trim & ", " & _
                                                         .Item("State").ToString.Trim & " " & _
                                                         .Item("Country").ToString.Trim()
                        Else
                            lblInstructorLocation.Text = .Item("City").ToString.Trim & ", " & _
                                                         .Item("Country").ToString.Trim
                        End If
                        lblInstructorEmail.Text = .Item("Email1").ToString.Trim
                        lnkInstructorEmail.HRef = "mailto:" & .Item("Email1").ToString.Trim
                    End With
                End If

                'grdSyllabus.DataSource = dt
                'grdSyllabus.DataBind()
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub LoadNotes()
            Dim sSQL As String, sValue As Object, sDB As String = AptifyApplication.GetEntityBaseDatabase("Persons")
            Try
                sSQL = "SELECT StudentNotes FROM " & _
                       sDB & "..vwClassRegistrations WHERE ID=" & _
                       Me.ClassRegistrationID
                sValue = Me.DataAction.ExecuteScalar(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                If Not sValue Is Nothing AndAlso Len(sValue) > 0 Then
                    lblStudentNotes.Text = CStr(sValue)
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        '2007/12/14 MAS: LEARNING MANAGEMENT SYSTEM SUPPORT
        Private m_dLMS As New Generic.Dictionary(Of Integer, Aptify.Framework.Web.eBusiness.LearningManagementSystems.LMSBase)

        ''' <summary>
        ''' Provided a Learning Management System record ID, the instantiated and configured Plug-In Object will be returned.
        ''' If an invalid ID is provided a failure will occur.
        ''' If insufficient information is provided in the LMS Record, then the LMSBase object will be returned.
        ''' </summary>
        ''' <param name="LmsID">Record ID for the Learning Management System used by the Course Part.</param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private ReadOnly Property LmsObject(ByVal LmsID As Integer) As Aptify.Framework.Web.eBusiness.LearningManagementSystems.LMSBase
            Get
                If m_dLMS.ContainsKey(LmsID) Then
                    Return m_dLMS(LmsID)
                Else
                    'Create a new instance for this LMSType
                    Dim oLMS As LearningManagementSystems.LMSBase
                    Dim sDB As String = AptifyApplication.GetEntityBaseDatabase("Learning Management Systems")
                    Dim sView As String = AptifyApplication.GetEntityBaseView("Learning Management Systems")
                    Dim sSQL As String = "SELECT ID, ObjectName, AssemblyName, ClassName FROM " & sDB & ".." & sView & " WHERE ID=" & LmsID.ToString
                    Dim dt As DataTable = Me.DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                    With dt.Rows(0)
                        If (.Item("ObjectName").ToString <> "" OrElse .Item("AssemblyName").ToString <> "") AndAlso .Item("ClassName").ToString <> "" Then
                            'oLMs1.Config(

                            oLMS = TryCast(AptifyApplication.CreateInstanceFromRepository(dt.Rows(0).Item("ObjectName").ToString, _
                                                                                            dt.Rows(0).Item("AssemblyName").ToString, _
                                                                                            dt.Rows(0).Item("ClassName").ToString),  _
                                            Aptify.Framework.Web.eBusiness.LearningManagementSystems.LMSBase)
                        Else
                            'Not enough information is available in the LMS record, so just use the Base class object
                            oLMS = New Aptify.Framework.Web.eBusiness.LearningManagementSystems.LMSBase
                        End If
                        oLMS.Config(LmsID, AptifyApplication)
                        m_dLMS.Add(LmsID, oLMS)
                        Return oLMS
                    End With
                End If
            End Get
        End Property

        Private Sub LoadSyllabus()
            Dim sSQL As String, dt As Data.DataTable, sDB As String
            Try
                If ViewState(ATTRIBUTE_DATATABLE_SYLLABUS) IsNot Nothing Then
                    grdSyllabus.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_SYLLABUS), Data.DataTable)
                    grdSyllabus.DataBind()
                    Exit Sub
                End If
                '2007/12/13 MAS: the below query has been altered to now only return Course Parts that are WebEnabled.
                sDB = AptifyApplication.GetEntityBaseDatabase("Course Parts")
                'Dim sCpView As String = AptifyApplication.GetEntityBaseView("Course Parts")
                'Dim sCpuView As String = AptifyApplication.GetEntityBaseView("ClassPartUse")
                'Dim sCrpsView As String = AptifyApplication.GetEntityBaseView("ClassRegistrationPartStatus")
                'Dim sCrView As String = AptifyApplication.GetEntityBaseView("Class Registrations")
                'Dim sCView As String = AptifyApplication.GetEntityBaseView("Classes")
                'Dim sCoView As String = AptifyApplication.GetEntityBaseView("Courses")

                'sSQL Added by sheetal patange dated 2/11/2014 for showing class syllabus even if user is not registered sub issue for 6642

                ' If (lnkRegister.Visible = True And Me.IsRegisteredForClass = False) Then
                'sSQL = "SELECT DISTINCT cp.WebName," & _
                '       "cp.WebURL," & _
                '       "cpu.Sequence," & _
                '       "cp.WebDescription," & _
                '       "cp.Type," & _
                '       "cp.Duration," & _
                '       "cp.ID," & _
                '       "cp.LearningManagementSystemID," & _
                '       "'CourseStatus' = 'Not Started'," & _
                '       "'ClassPartStart' = isNull(cpu.StartDate ,N'')," & _
                '       "'ClassPartEnd' = isNull(cpu.EndDate ,N'')," & _
                '       " cpu.RequirePriorPartsComplete," & _
                '       "'RegistrationStart' = ''," & _
                '       "'RegistrationEnd' = ''," & _
                '       "'ClassStart' = isNull(c.StartDate ,N'')," & _
                '       "'ClassEnd' = isNull(c.EndDate ,N'')," & _
                '       " IsNull(cp.status,'') 'CoursePartStatus'," & _
                '       "'CoursePartExpires'= isNull(cp.DateExpires,N'')" & _
                '       "FROM " & sDB & ".." & sCView & " c " & _
                '       "INNER JOIN " & sDB & ".." & sCoView & " co ON co.ID=c.CourseID " & _
                '       "INNER JOIN " & sDB & ".." & sCpuView & " cpu ON cpu.ClassID=c.ID " & _
                '       "INNER JOIN " & sDB & ".." & sCpView & " cp ON cp.ID=cpu.CoursePartID " & _
                '       " WHERE c.ID=" & Me.Request("ClassID") & _
                '       " AND cp.WebEnabled=1 " & _
                '       " ORDER BY cpu.Sequence"




                'Else
                'sSQL = "SELECT DISTINCT cp.WebName," & _
                '       "cp.WebURL," & _
                '       "cpu.Sequence," & _
                '       "cp.WebDescription," & _
                '       "cp.Type," & _
                '       "cp.Duration," & _
                '       "cp.ID," & _
                '       "cp.LearningManagementSystemID," & _
                '       "'CourseStatus' = isNull(crps.Status,N'Not Started')," & _
                '       "'ClassPartStart' = isNull(cpu.StartDate ,N'')," & _
                '       "'ClassPartEnd' = isNull(cpu.EndDate ,N'')," & _
                '       " cpu.RequirePriorPartsComplete," & _
                '       "'RegistrationStart' = isNull(cr.DateAvailable ,N'')," & _
                '       "'RegistrationEnd' = isNull(cr.DateExpires ,N'')," & _
                '       "'ClassStart' = isNull(c.StartDate ,N'')," & _
                '       "'ClassEnd' = isNull(c.EndDate ,N'')," & _
                '       " IsNull(cp.status,'') 'CoursePartStatus'," & _
                '       "'CoursePartExpires'= isNull(cp.DateExpires,N'')" & _
                '       "FROM " & sDB & ".." & sCView & " c " & _
                '       "INNER JOIN " & sDB & ".." & sCoView & " co ON co.ID=c.CourseID " & _
                '       "INNER JOIN " & sDB & ".." & sCpuView & " cpu ON cpu.ClassID=c.ID " & _
                '       "INNER JOIN " & sDB & ".." & sCpView & " cp ON cp.ID=cpu.CoursePartID " & _
                '       "INNER JOIN " & sDB & ".." & sCrView & " cr ON cr.ClassID=c.ID " & _
                '       "LEFT OUTER JOIN " & sDB & ".." & sCrpsView & " crps ON crps.ClassRegistrationID=cr.ID AND crps.CoursePartID=cp.ID " & _
                '       " WHERE c.ID=" & Me.Request("ClassID") & _
                '       " AND cr.ID=" & Me.ClassRegistrationID.ToString & _
                '       " AND cp.WebEnabled=1 " & _
                '       " ORDER BY cpu.Sequence"

                ' End If

                'Sheetal 16/07/15: For Issue 21368 :Prevent Sql Injection Attack: Used parameterrized sql is best practice
                Dim params(3) As IDataParameter
                params(0) = Me.DataAction.GetDataParameter("@ClassID", SqlDbType.Int, Me.Request("ClassID"))
                params(1) = Me.DataAction.GetDataParameter("@LnkReg", SqlDbType.Bit, lnkRegister.Visible)
                params(2) = Me.DataAction.GetDataParameter("@RegForClass", SqlDbType.Bit, Me.IsRegisteredForClass)
                params(3) = Me.DataAction.GetDataParameter("@ClassRegID", SqlDbType.VarChar, Me.ClassRegistrationID.ToString)
                sSQL = sDB & "..spLoadClassSyllabus"
                dt = Me.DataAction.GetDataTableParametrized(sSQL, CommandType.StoredProcedure, params)

                ' dt = Me.DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                'Anil B For Issue 13326 on 04-04-2013
                'Check datatable nothing codition
                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then

                    ''LEARNING MANAGEMENT SYSTEM:

                    If Me.IsRegisteredForClass OrElse Me.IsInstructor Then
                        'Add WebURL for the CoursePart
                        'Navin Prasad Issue 15076
                        Dim sFirstName As String = String.Empty
                        Dim sLastName As String = String.Empty
                        sFirstName = Me.User1.FirstName
                        lblNote.Text = "Note: You may need to refresh this page after completing an online course or quiz to see the results reflected in this grid’s Status column."
                        If Not String.IsNullOrEmpty(sFirstName) AndAlso sFirstName.Contains("'") Then
                            sFirstName = sFirstName.Replace("'", "")
                        End If
                        sLastName = Me.User1.LastName
                        If Not String.IsNullOrEmpty(sLastName) AndAlso sLastName.Contains("'") Then
                            sLastName = sLastName.Replace("'", "")
                        End If
                        For Each row As DataRow In dt.Rows
                            row.Item("WebURL") = LmsObject(CInt(row.Item("LearningManagementSystemID"))).GetLmsUrl(CLng(row.Item("ID")), Me.ClassRegistrationID, Me.User1.PersonID, sFirstName, sLastName, row.Item("WebURL").ToString)
                            row.Item("WebUrl") = "javascript: _do_open_content('" & row.Item("WebUrl").ToString & "');"
                        Next
                    Else
                        'Clear WebURL, since Logged In User should not have access
                        For Each row As DataRow In dt.Rows
                            row.Item("WebURL") = ""
                        Next
                    End If

                    '2007/12/13 MAS Enforce related Date Fields and RequirePriorPartsComplete associated with each Course Part
                    For Each row As DataRow In dt.Rows
                        'Today < Class.StartDate?
                        If Not IsDBNull(row.Item("ClassStart")) _
                                AndAlso row.Item("ClassStart").ToString <> "" _
                                AndAlso Today < CDate(row.Item("ClassStart")).Date _
                                AndAlso CDate(row.Item("ClassStart")).Date <> CDate("1/1/1900") Then
                            row.Item("WebURL") = ""
                            Continue For
                        End If
                        'Today > Class.EndDate?
                        If Not IsDBNull(row.Item("ClassEnd")) _
                                AndAlso row.Item("ClassEnd").ToString <> "" _
                                AndAlso Today > CDate(row.Item("ClassEnd")).Date _
                                AndAlso CDate(row.Item("ClassEnd")).Date <> CDate("1/1/1900") Then
                            row.Item("WebURL") = ""
                            Continue For
                        End If
                        'Today < ClassRegistration.DateAvailable?
                        If Not IsDBNull(row.Item("RegistrationStart")) _
                                AndAlso row.Item("RegistrationStart").ToString <> "" _
                                AndAlso Today < CDate(row.Item("RegistrationStart")).Date _
                                AndAlso CDate(row.Item("RegistrationStart")).Date <> CDate("1/1/1900") Then
                            row.Item("WebURL") = ""
                            Continue For
                        End If
                        'Today > ClassRegistration.DateExpires?
                        If Not IsDBNull(row.Item("RegistrationEnd")) _
                                AndAlso row.Item("RegistrationEnd").ToString <> "" _
                                AndAlso Today > CDate(row.Item("RegistrationEnd")).Date _
                                AndAlso CDate(row.Item("RegistrationEnd")).Date <> CDate("1/1/1900") Then
                            row.Item("WebURL") = ""
                            Continue For
                        End If
                        'Today < ClassPartUse.StartDate?
                        If Not IsDBNull(row.Item("ClassPartStart")) _
                                AndAlso row.Item("ClassPartStart").ToString <> "" _
                                AndAlso Today < CDate(row.Item("ClassPartStart")).Date _
                                AndAlso CDate(row.Item("ClassPartStart")).Date <> CDate("1/1/1900") Then
                            row.Item("WebURL") = ""
                            Continue For
                        End If
                        'Today > ClassPartUse.EndDate?
                        If Not IsDBNull(row.Item("ClassPartEnd")) _
                                AndAlso row.Item("ClassPartEnd").ToString <> "" _
                                AndAlso Today > CDate(row.Item("ClassPartEnd")).Date _
                                AndAlso CDate(row.Item("ClassPartEnd")).Date <> CDate("1/1/1900") Then
                            row.Item("WebURL") = ""
                            Continue For
                        End If
                        'Following code is added by sheetal Patange for issue :6639 dated 21/11/2013. 
                        ' Today > CoursePartExpires?
                        If Not IsDBNull(row.Item("CoursePartExpires")) _
                                AndAlso row.Item("CoursePartExpires").ToString <> "" _
                                AndAlso Today > CDate(row.Item("CoursePartExpires")).Date _
                                AndAlso CDate(row.Item("CoursePartExpires")).Date <> CDate("1/1/1900") Then
                            row.Item("WebURL") = ""
                            Continue For
                        End If
                        'CoursePartStatus <> Active?
                        If Not IsDBNull(row.Item("CoursePartStatus")) _
                               AndAlso row.Item("CoursePartStatus").ToString.Trim() <> "Active" Then
                            row.Item("WebURL") = ""
                            Continue For
                        End If
                    Next

                    'Enforce RequirePriorPartsComplete: 
                    '  If the last part was not completed, then a link will not be provided for this part
                    Dim bPriorPartsComplete As Boolean = True
                    For i As Integer = 0 To dt.Rows.Count - 1
                        'Starting with index 1 because even if the first Course Part has this flag
                        'set, there is no 'Prior Part' to check, so its moot.
                        If i > 0 Then
                            'skip for first Part, since on Prior Part to complete
                            If CBool(dt.Rows(i).Item("RequirePriorPartsComplete")) Then
                                If Not bPriorPartsComplete Then
                                    'Note in Description that Part Requires prior Parts Completion first (only of link is being disabled)
                                    dt.Rows(i).Item("WebName") = dt.Rows(i).Item("WebName").ToString & "<br><small> (All previous parts must first be completed.)</small>"
                                    dt.Rows(i).Item("WebURL") = ""
                                End If
                            End If
                        End If
                        ' 26/11/2013 Modified by Sheetal Patange for Issue 17807 LMS: Add the Completed Criteria to "Passed" Option.
                        If dt.Rows(i).Item("CourseStatus").ToString.ToLower.Trim <> "completed" _
                                AndAlso dt.Rows(i).Item("CourseStatus").ToString.ToLower.Trim <> "success" _
                                AndAlso dt.Rows(i).Item("CourseStatus").ToString.ToLower.Trim <> "failed" _
                                AndAlso dt.Rows(i).Item("CourseStatus").ToString.ToLower.Trim <> "passed" Then
                            'bPriorPartsComplete can only be set to false. This will ensure that if any part 
                            'was not completed, all subsequent parts that have this flag set will not be allowed 
                            bPriorPartsComplete = False
                        End If
                    Next


                    Dim dcolUrl As DataColumn = New DataColumn()
                    dcolUrl.Caption = "WebURLUrl"
                    dcolUrl.ColumnName = "WebURLUrl"

                    dt.Columns.Add(dcolUrl)
                    If dt.Rows.Count > 0 Then
                        'Anil B For Issue 13326 on 04-04-2013
                        'Set WEB URL
                        For Each rw As DataRow In dt.Rows
                            rw("WebURLUrl") = String.Format("{0}", rw("WebURL").ToString)
                        Next
                    End If
                    grdSyllabus.DataSource = dt
                    grdSyllabus.DataBind()
                    ViewState(ATTRIBUTE_DATATABLE_SYLLABUS) = dt
                    Dim rowcounter As Integer = 0

                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Private Sub LoadStudents()
            Dim sSQL As String, dt As Data.DataTable, sDB As String = AptifyApplication.GetEntityBaseDatabase("Course Parts")
            Try
                If ViewState(ATTRIBUTE_DATATABLE_STUDENT) IsNot Nothing Then
                    grdStudents.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_STUDENT), Data.DataTable)
                    grdStudents.DataBind()
                    Exit Sub
                End If
                ' sSQL = " SELECT p.LastName,p.FirstName,p.FirstLast,p.Email1, " & _
                '       " cr.* FROM  " & _
                '       sDB & "..vwClassRegistrations cr INNER JOIN " & sDB & _
                '       "..vwPersons p ON cr.StudentID=p.ID " & _
                '      " WHERE cr.ClassID=" & Me.Request("ClassID") & _
                '      " ORDER BY p.LastName,p.FirstName "

                'Sheetal 16/07/15: For Issue 21368 :Prevent Sql Injection Attack: Used parameterrized sql is best practice
                Dim params(0) As IDataParameter
                params(0) = Me.DataAction.GetDataParameter("@ClassID", SqlDbType.Int, Request.QueryString("ClassID"))
                sSQL = Me.AptifyApplication.GetEntityBaseDatabase("Courses") & "..spLoadStudents"

                'dt = Me.DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                dt = Me.DataAction.GetDataTableParametrized(sSQL, CommandType.StoredProcedure, params)

                If dt.Rows.Count > 0 Then
                    Dim dcolUrlLastName As DataColumn = New DataColumn()
                    dcolUrlLastName.Caption = "LastNameUrl"
                    dcolUrlLastName.ColumnName = "LastNameUrl"

                    dt.Columns.Add(dcolUrlLastName)

                    Dim dcolUrlFirstName As DataColumn = New DataColumn()
                    dcolUrlFirstName.Caption = "FirstNameUrl"
                    dcolUrlFirstName.ColumnName = "FirstNameUrl"

                    dt.Columns.Add(dcolUrlFirstName)

                    Dim dcolUrlScoreUrl As DataColumn = New DataColumn()
                    dcolUrlScoreUrl.Caption = "ScoreUrl"
                    dcolUrlScoreUrl.ColumnName = "ScoreUrl"

                    dt.Columns.Add(dcolUrlScoreUrl)
                    For Each rw As DataRow In dt.Rows
                        rw("LastNameUrl") = String.Format("mailto:{0:s}", rw("Email1").ToString)
                        rw("FirstNameUrl") = String.Format("mailto:{0:s}", rw("Email1").ToString)
                        rw("ScoreUrl") = String.Format("{0:d}", rw("Score").ToString)
                    Next
                    grdStudents.DataSource = dt
                    RemoveInvalidDatefromGrid()
                End If
                grdStudents.DataSource = dt
                grdStudents.DataBind()
                ViewState(ATTRIBUTE_DATATABLE_STUDENT) = dt


            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        ''' <summary>
        ''' RashmiP, issue 11275, Date: 5/25/2011
        ''' Show blank value in place of invalid date in Students Grid
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub RemoveInvalidDatefromGrid()
            Dim dtExpiredate As String
            Dim dtcompleteDate As String
            Try
                'Navin Prasad Issue 11032
                If grdStudents.Items.Count > 0 Then
                    For i As Integer = 0 To grdStudents.Items.Count - 1
                        'Navin Prasad Issue 11032
                        dtExpiredate = grdStudents.Items(i).Cells(5).Text
                        If dtExpiredate.Contains("1/1/1900") Then
                            grdStudents.Items(i).Cells(5).Text = String.Empty
                        End If

                        dtcompleteDate = grdStudents.Items(i).Cells(3).Text
                        If dtcompleteDate.Contains("1/1/1900") Then
                            grdStudents.Items(i).Cells(3).Text = String.Empty
                        End If

                    Next

                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Public Sub LoadForum()
            If IsRegisteredForClass Or IsInstructor Then
                Dim lForumID As Long = SingleForum.GetLinkedForumID("Classes", CLng(Request.QueryString("ClassID")))
                Me.btnCreateForum.Visible = False
                If lForumID > 0 Then
                    SingleForum.LoadForum(lForumID)
                Else
                    Me.SingleForum.Visible = False
                    If Not IsInstructor Then
                        Me.lblDetails.Text = "No forum has been set up for this class. Please contact your instructor to establish a discussion forum for this class."
                    Else
                        Me.lblDetails.Text = "No forum has been set up for this class yet. Discussion forums are available for students to interact with one another and with the instructor. As the instructor of this class, you are allowed to establish a discussion forum for this purpose. If you would like to create a discussion forum for use by this class, please click the 'Create Forum' button below."
                        Me.btnCreateForum.Visible = True
                    End If
                    Me.lblDetails.Visible = True
                End If
            End If
        End Sub

        Protected Sub btnCreateForum_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCreateForum.Click
            Try
                Dim sError As String = ""
                Dim dtStartDate As Date = DateSerial(1900, 1, 1)
                Dim dtEndDate As Date = dtStartDate

                If IsDate(lblStartDate.Text) Then
                    dtStartDate = CDate(lblStartDate.Text)
                End If

                If IsDate(lblEndDate.Text) Then
                    dtEndDate = DateAdd(DateInterval.Day, 15, CDate(lblEndDate.Text))
                End If

                If Me.SingleForum.CreateNewLinkedForum("Class Forum: " & lblName.Text & " (" & Request.QueryString("ClassID") & ")", _
                                                        "Class forum created by instructor (" & InstructorValidator1.User.WebUserStringID & ") on " & Now.ToLongDateString, _
                                                        dtStartDate, _
                                                        dtEndDate, _
                                                        "Classes", _
                                                        CLng(Me.Request.QueryString("ClassID")), , sError) Then
                    Response.Redirect(Request.RawUrl) ' force refresh
                Else
                    Me.lblDetails.Text = sError
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub btnCancelStudentNotes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelStudentNotes.Click
            Try
                Response.Redirect(Me.lnkNotes.NavigateUrl)
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub btnSaveStudentNotes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveStudentNotes.Click
            Try
                Dim oReg As Aptify.Framework.BusinessLogic.GenericEntity.AptifyGenericEntityBase
                oReg = Me.AptifyApplication.GetEntityObject("Class Registrations", Me.ClassRegistrationID)
                oReg.SetValue("StudentNotes", txtStudentNotes.Text)
                lblStudentNotesMessage.Visible = True
                If oReg.Save(False) Then
                    Response.Redirect(Me.lnkNotes.NavigateUrl)
                Else
                    lblStudentNotesMessage.Text = "There was a problem saving your notes: " & oReg.LastError
                    lblStudentNotesMessage.ForeColor = Drawing.Color.Red
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub btnEditStudentNotes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEditStudentNotes.Click
            txtStudentNotes.Visible = True
            txtStudentNotes.Text = lblStudentNotes.Text
            lblStudentNotes.Visible = False
            btnSaveStudentNotes.Visible = True
            btnCancelStudentNotes.Visible = True
            btnEditStudentNotes.Visible = False
        End Sub

        Protected Sub btnEditInstructorNotes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEditInstructorNotes.Click
            txtInstructorNotes.Visible = True
            txtInstructorNotes.Text = lblInstructorNotes.Text
            lblInstructorNotes.Visible = False
            btnSaveInstructorNotes.Visible = True
            btnCancelInstructorNotes.Visible = True
            btnEditInstructorNotes.Visible = False
        End Sub

        Protected Sub btnSaveInstructorNotes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveInstructorNotes.Click
            Try
                Dim oClass As Aptify.Framework.BusinessLogic.GenericEntity.AptifyGenericEntityBase
                oClass = Me.AptifyApplication.GetEntityObject("Classes", CLng(Me.Request("ClassID")))
                oClass.SetValue("InstructorComments", txtInstructorNotes.Text)
                lblInstructorNotesMessage.Visible = True
                If oClass.Save(False) Then
                    Response.Redirect(Me.lnkInstructorInfo.NavigateUrl)
                Else
                    lblInstructorNotesMessage.Text = "There was a problem saving your notes: " & oClass.LastError
                    lblInstructorNotesMessage.ForeColor = Drawing.Color.Red
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub btnCancelInstructorNotes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelInstructorNotes.Click
            Try
                Response.Redirect(Me.lnkInstructorInfo.NavigateUrl)
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub


        ''RashmiP, Issue 11290 [Meeting/Class Integration], Added new link button, on its click event redirect to Meeting Registration
        Protected Sub lnkRegisterMeeting_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkRegisterMeeting.Click
            Try
                If ViewState("ProductID") IsNot Nothing AndAlso IsNumeric(ViewState("ProductID")) Then
                    Dim sProductPage As String, sOrderPage As String
                    If ShoppingCart1.GetProductTypeWebPages(CLng(ViewState("ProductID")), sProductPage, sOrderPage) Then
                        If Len(sOrderPage) > 0 Then
                            ' special order page. redirect there now   
                            'Amruta ,Issue 15428,Added ProductID of meeting to queystring for Meeting Class Registration
                            Response.Redirect(sOrderPage & "?ID=" & ViewState("ProductID"), False)
                        End If
                    End If
                Else
                    lnkRegister.Visible = False
                    imgRegisterSmall.Visible = False
                    imgRegisterSmall2.Visible = False

                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        ''' <summary>
        ''' Nalini 12436 date:1/12/2011
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub grdSyllabus_PageIndexChanging(ByVal sender As Object, ByVal e As GridPageChangedEventArgs) Handles grdSyllabus.PageIndexChanged
            grdSyllabus.CurrentPageIndex = e.NewPageIndex
            If ViewState(ATTRIBUTE_DATATABLE_SYLLABUS) IsNot Nothing Then
                grdSyllabus.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_SYLLABUS), Data.DataTable)
            End If
        End Sub
        Protected Sub grdSyllabus_PageSizeChanged(ByVal sender As Object, ByVal e As GridPageSizeChangedEventArgs) Handles grdSyllabus.PageSizeChanged
            If ViewState(ATTRIBUTE_DATATABLE_SYLLABUS) IsNot Nothing Then
                grdSyllabus.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_SYLLABUS), Data.DataTable)
            End If
        End Sub
        'Neha Change for Issue 14452 For Date format 
        Protected Sub grdStudents_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdStudents.ItemDataBound
            Dim dateColumns As New List(Of String)
            'Add datecolumn uniqueName in list for Date format
            dateColumns.Add("GridDateTimeColumnDateRegistered")
            dateColumns.Add("GridDateTimeColumnDateCompleted")
            dateColumns.Add("GridDateTimeColumnDateAvailable")
            dateColumns.Add("GridDateTimeColumnDateExpires")
            CommonMethods.FormatedDateOnGrid(dateColumns, e.Item)
            'Suraj Issue  14452 ,5/3/13  ,we provide a tool tip for DatePopupButton 
            If TypeOf e.Item Is GridFilteringItem Then
                Dim filterItem As GridFilteringItem = DirectCast(e.Item, GridFilteringItem)
                Dim GridDateTimeColumnDateRegistered As RadDatePicker = DirectCast(filterItem("GridDateTimeColumnDateRegistered").Controls(0), RadDatePicker)
                GridDateTimeColumnDateRegistered.ToolTip = "Enter a filter date"
                GridDateTimeColumnDateRegistered.DatePopupButton.ToolTip = "Select a filter date"
                Dim GridDateTimeColumnDateCompleted As RadDatePicker = DirectCast(filterItem("GridDateTimeColumnDateCompleted").Controls(0), RadDatePicker)
                GridDateTimeColumnDateCompleted.ToolTip = "Enter a filter date"
                GridDateTimeColumnDateCompleted.DatePopupButton.ToolTip = "Select a filter date"
                Dim GridDateTimeColumnDateAvailable As RadDatePicker = DirectCast(filterItem("GridDateTimeColumnDateAvailable").Controls(0), RadDatePicker)
                GridDateTimeColumnDateAvailable.ToolTip = "Enter a filter date"
                GridDateTimeColumnDateAvailable.DatePopupButton.ToolTip = "Select a filter date"
                Dim GridDateTimeColumnDateExpires As RadDatePicker = DirectCast(filterItem("GridDateTimeColumnDateExpires").Controls(0), RadDatePicker)
                GridDateTimeColumnDateExpires.ToolTip = "Enter a filter date"
                GridDateTimeColumnDateExpires.DatePopupButton.ToolTip = "Select a filter date"
            End If

        End Sub

        Protected Sub grdStudents_PageIndexChanging(ByVal sender As Object, ByVal e As GridPageChangedEventArgs) Handles grdStudents.PageIndexChanged
            grdStudents.CurrentPageIndex = e.NewPageIndex
            If ViewState(ATTRIBUTE_DATATABLE_STUDENT) IsNot Nothing Then
                grdStudents.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_STUDENT), Data.DataTable)
            End If
        End Sub
        Protected Sub grdStudents_PageSizeChanged(ByVal sender As Object, ByVal e As GridPageSizeChangedEventArgs) Handles grdStudents.PageSizeChanged
            If ViewState(ATTRIBUTE_DATATABLE_STUDENT) IsNot Nothing Then
                grdStudents.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_STUDENT), Data.DataTable)
            End If
        End Sub
        Protected Sub grdSyllabus_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdSyllabus.NeedDataSource
            If ViewState(ATTRIBUTE_DATATABLE_SYLLABUS) IsNot Nothing Then
                grdSyllabus.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_SYLLABUS), Data.DataTable)
            End If
        End Sub
        Protected Sub grdStudents_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdStudents.NeedDataSource
            If ViewState(ATTRIBUTE_DATATABLE_STUDENT) IsNot Nothing Then
                grdStudents.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_STUDENT), Data.DataTable)
            End If
        End Sub
        Private Sub AddExpression()
            Dim ExpSyllabusSort As New GridSortExpression
            'ExpSyllabusSort.FieldName = "WebName"
            'ExpSyllabusSort.SetSortOrder("Ascending")
            Dim ExpStudentSort As New GridSortExpression
            ExpStudentSort.FieldName = " LastName"
            ExpsyllabusSort.SetSortOrder("Ascending")
            'grdSyllabus.MasterTableView.SortExpressions.AddSortExpression(ExpSyllabusSort)
            grdStudents.MasterTableView.SortExpressions.AddSortExpression(ExpStudentSort)
        End Sub
    End Class
End Namespace
