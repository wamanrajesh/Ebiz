'Aptify e-Business 5.5.1/LMS 5.5.1, June 2014
Option Explicit On
Imports Aptify.Framework.Web.eBusiness.SocialNetworkIntegration
Imports System.Xml
Imports System.Data
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.Application
Imports Aptify.Framework.Integration
Imports Aptify.Framework.BusinessLogic
Imports System.ComponentModel

Namespace Aptify.Framework.Web.eBusiness
    ''' <summary>
    ''' Aptify e-Business Login ASP.NET User Control
    ''' </summary>
    ''' <remarks></remarks>
    Partial Class WebLogin
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced
        Implements ILogin

        Protected Const ATTRIBUTE_NUM_COLUMNS As String = "TimeForExpiry"
        Protected Const ATTRIBUTE_NEWUSER_PAGE As String = "NewUserPage"
        Protected Const ATTRIBUTE_FORGOTUID_PAGE As String = "ForgotUIDPage"
        Protected Const ATTRIBUTE_MAXLOGINTRIES As String = "maxLoginTries"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "Login"
        Protected Const ATTRIBUTE_USER_DISABLED_ERROR As String = "UserDisabledError" 'SKB Issue#10654: display message indicating disabled account
        Protected Const ATTRIBUTE_HOME_PAGE As String = "HomePage"
        'Neha Issue 14408,01/24/13,declare property for ChangePassword 
        Protected Const ATTRIBUTE_HOME_CHANGEPWD As String = "ChangePassword"
        Private m_iTimeForExpiry As Integer
        'Neha, Issue 14408,03/20/13,declare property for new WebUser 
        Private CheckNewWebUser As Boolean

        ''Added by Suvarna 17790
        Private m_oUser As IUser2
        Private m_disabled As Boolean 'HP Issue#9078 
        Private m_resetRequired As Boolean = False  'JSH 05.08.14
        Private m_temporaryPWD As String = String.Empty  'JSH 05.08.14
        Private m_temporaryUsed As Boolean = False   'JSH 05.08.14
        ''End of addition

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            'Suraj S Issue 15370, 7/31/13 here we are getting the ReturnToPageURL in "URL" QueryString and we storing this URL in to the Session("ReturnToPage")  
            If Request.QueryString("ReturnURL") IsNot Nothing Then
                Session("ReturnToPage") = Aptify.Framework.Web.Common.WebCryptography.Decrypt(Request.QueryString("ReturnURL"))
            End If
            SetProperties()
            If Not IsPostBack Then

                'Anil B for  Issue 13882 on 18-03-2013
                'Set Remember option for login
                If Request.Browser.Cookies Then
                    'Check if the cookies with name LOGIN exist on user's machine
                    If Request.Cookies("LOGIN") IsNot Nothing AndAlso Request.Cookies("LOGIN").Item("RememberMe") IsNot Nothing Then
                        'Pass the user name and password to the VerifyLogin method
                        chkAutoLogin.Checked = CBool(Request.Cookies("LOGIN").Item("RememberMe"))
                    End If
                End If
                CleareCatche()
            End If

            'Anil B for  Issue 13882 on 18-03-2013
            'Set Remember option for login
            If IsPostBack Then
                If Request.Browser.Cookies Then
                    'Check if the cookie with name LOGIN exist on user's machine
                    'Create a cookie with expiry of 30 days
                    Response.Cookies("LOGIN").Expires = DateTime.Now.AddDays(30)
                    'Write username to the cookie
                    Response.Cookies("LOGIN").Item("RememberMe") = chkAutoLogin.Checked
                End If
            End If
        End Sub
        Private Sub CleareCatche()
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1))
            Response.Cache.SetCacheability(HttpCacheability.NoCache)
            Response.Cache.SetNoStore()
            Response.Buffer = True
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
            Response.Expires = -1500
        End Sub
        Public Overridable Property TimeForExpiry() As Integer
            Get
                Return m_iTimeForExpiry
            End Get
            Set(ByVal value As Integer)
                m_iTimeForExpiry = value
            End Set
        End Property
        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(NewUserPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                NewUserPage = Me.GetLinkValueFromXML(ATTRIBUTE_NEWUSER_PAGE)
                If String.IsNullOrEmpty(NewUserPage) Then
                    Me.cmdNewUser.Enabled = False
                    Me.cmdNewUser.ToolTip = "NewUserPage property has not been set."
                End If

            End If
            If String.IsNullOrEmpty(ForgotUIDPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                ForgotUIDPage = Me.GetLinkValueFromXML(ATTRIBUTE_FORGOTUID_PAGE)
                If String.IsNullOrEmpty(ForgotUIDPage) Then
                    Me.hlinkForgotUID.Enabled = False
                    Me.hlinkForgotUID.ToolTip = "ForgotUIDPage property has not been set."
                Else
                    Me.hlinkForgotUID.NavigateUrl = ForgotUIDPage
                End If
            Else
                Me.hlinkForgotUID.NavigateUrl = ForgotUIDPage
            End If
            'Neha Issue 14408,01/24/13,set property for ChangePassword 
            If String.IsNullOrEmpty(ChangePassword) Then
                'since value is the 'default' check the XML file for possible custom setting 
                ChangePassword = Me.GetLinkValueFromXML(ATTRIBUTE_HOME_CHANGEPWD)
                If String.IsNullOrEmpty(ChangePassword) Then
                    Me.cmdLogin.Enabled = False
                    Me.cmdLogin.ToolTip = "ChangePassword property has not been set."
                End If
            End If

            If String.IsNullOrEmpty(HomePage) Then
                'since value is the 'default' check the XML file for possible custom setting
                HomePage = Me.GetLinkValueFromXML(ATTRIBUTE_HOME_PAGE)
            End If

            If TimeForExpiry = 0 Then
                'since value is the 'default' check the XML file for possible custom setting
                If Not String.IsNullOrEmpty(Me.GetPropertyValueFromXML(ATTRIBUTE_NUM_COLUMNS)) Then
                    TimeForExpiry = CInt(Me.GetPropertyValueFromXML(ATTRIBUTE_NUM_COLUMNS))
                End If
            End If
        End Sub

        ''' <summary>
        ''' This event is raised whenever a user logs in
        ''' </summary>
        ''' <remarks></remarks>
        Public Event UserLoggedIn()

        ''' <summary>
        ''' This event is raised whenever a user logs out
        ''' </summary>
        ''' <remarks></remarks>
        Public Event UserLoggedOut()

        ''' <summary>
        ''' ForgotUID page url
        ''' </summary>
        Public Overridable Property ForgotUIDPage() As String
            Get
                If Not ViewState(ATTRIBUTE_FORGOTUID_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_FORGOTUID_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_FORGOTUID_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' NewUser page url
        ''' </summary>
        Public Overridable Property NewUserPage() As String
            Get
                If Not ViewState(ATTRIBUTE_NEWUSER_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_NEWUSER_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_NEWUSER_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' Home page url
        ''' </summary>
        Public Overridable Property HomePage() As String
            Get
                If Not ViewState(ATTRIBUTE_HOME_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_HOME_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_HOME_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        'Neha Issue 14408,01/24/13,added property for ChangePassword,added ChangePassword page url
        Public Overridable Property ChangePassword() As String
            Get
                If Not ViewState(ATTRIBUTE_HOME_CHANGEPWD) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_HOME_CHANGEPWD))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_HOME_CHANGEPWD) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        'Neha, Issue 14408,03/20/13,set property for new webuser  
        Public Property CheckNewUser As Boolean
            Get
                Return CheckNewWebUser
            End Get
            Set(ByVal value As Boolean)
                CheckNewWebUser = value
            End Set
        End Property
        ''' <summary>
        ''' MaxLoginTries
        ''' </summary>
        Public Overridable ReadOnly Property MaxLoginTries() As Integer
            Get
                Try
                    If Not String.IsNullOrEmpty(Me.GetGlobalAttributeValue(ATTRIBUTE_MAXLOGINTRIES)) Then
                        If Not IsNumeric(Me.GetGlobalAttributeValue(ATTRIBUTE_MAXLOGINTRIES)) Then
                            Throw New Exception("Incorrect entry for <Global>...<" & ATTRIBUTE_MAXLOGINTRIES & ">, a numeric value is required. " & _
                                                "Please confirm the entry is correctly input in the 'Aptify_UC_Navigation.config' file.")
                        Else
                            Return CInt(Me.GetGlobalAttributeValue(ATTRIBUTE_MAXLOGINTRIES))
                        End If
                    Else
                        Return 0
                    End If
                Catch ex As Exception
                    Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                    Return 0
                End Try
            End Get
        End Property
        ''' <summary>
        ''' String to show in the title
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.DefaultValue("Login")> _
        Public Overridable Property TitleString() As String
            Get
                If Not ViewState("TitleString") Is Nothing Then
                    Return CStr(ViewState("TitleString"))
                Else
                    Return "Login"
                End If
            End Get
            Set(ByVal value As String)
                ViewState("TitleString") = value
            End Set
        End Property
        ''' <summary>
        ''' Defaults to true, shows the title of the control or not
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Property ShowTitle() As Boolean
            Get
                If Not ViewState("ShowTitle") Is Nothing Then
                    Return CBool(ViewState("ShowTitle"))
                Else
                    Return True
                End If
            End Get
            Set(ByVal value As Boolean)
                ViewState("ShowTitle") = value
            End Set
        End Property
        ''added by Suvarna - 17790
        Public ReadOnly Property User() As IUser Implements ILogin.User
            Get
                Return m_oUser
            End Get
        End Property
        'HP Issue#9078 
        Public ReadOnly Property Disabled() As Boolean
            Get
                Return m_disabled
            End Get
        End Property
        Public Property PasswordResetRequired As Boolean
            Get
                Return m_resetRequired
            End Get
            Set(ByVal value As Boolean)
                m_resetRequired = value
            End Set
        End Property
        Public Property temporaryPWD As String
            Get
                Return m_temporaryPWD
            End Get
            Set(ByVal value As String)
                m_temporaryPWD = value
            End Set
        End Property
        Public Property temporaryUsed As Boolean
            Get
                Return m_temporaryUsed
            End Get
            Set(ByVal value As Boolean)
                m_temporaryUsed = value
            End Set
        End Property

        ''End of Addition by Suvarna


        'RashmiP, Function clears cookies
        Private Sub ClearCookies()
            Dim i As Integer
            Dim aCookie As HttpCookie

            Dim limit As Integer = Request.Cookies.Count - 1
            For i = 0 To limit
                aCookie = Request.Cookies(i)
                aCookie.Expires = DateTime.Now.AddDays(-1)
                Response.Cookies.Add(aCookie)
            Next

            'Check iIf the cookies with name LOGIN exist on user's machine
            If (Request.Cookies("LOGIN") IsNot Nothing) Then
                'Expire the cookie
                Response.Cookies("LOGIN").Expires = DateTime.Now.AddDays(-TimeForExpiry)
            End If

        End Sub

        Private Sub cmdLogin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdLogin.Click
            ' Login to the Aptify Enterprise Web Architecture
            ' Use the Aptify EWA .NET Login Component to do this
            Dim bLoggedIn As Boolean = False
            Try
                If Not String.IsNullOrEmpty(txtUserID.Text.Trim()) And Not String.IsNullOrEmpty(txtPassword.Text.Trim()) Then
                    With WebUserLogin1
                        'HP Issue#9078: add MaxLoginTries from config file
                        If .Login(txtUserID.Text, txtPassword.Text, Page.User, MaxLoginTries) Then

                            tblLogin.Visible = False
                            litLoginLabel.Visible = False
                            tblWelcome.Visible = True
                            lblWelcome.Text = "Welcome, " & _
                                              .User.FirstName & " " & _
                                              .User.LastName
                            lblError.Text = ""

                            If chkAutoLogin.Checked Then
                                .AddAutoLoginCookie(Page.Response, txtUserID.Text, txtPassword.Text)
                            Else
                                .ClearAutoLoginCookie(Page.Response)
                            End If

                            ''Rashmi P: Issue 17849: Need to Secure Tin Can Content Access.
                            AddCookieForLMSUser(Page.Request, Page.Response, CStr(.User.PersonID))
                            ' make sure to persist changes to user, since many
                            ' applications will do a Response.Redirect after
                            ' this event is fired
                            .User.SaveValuesToSessionObject(Page.Session)
                            bLoggedIn = True
                            'Added 05.08.14 - Password reset functionality - force a change
                            If .PasswordResetRequired And .temporaryUsed Then
                                Session("ReturnToPage") = ""
                                OnUserLoggedIn()  'Execute the login extra events for menus and whatnot
                                Dim sRedirect As String = ChangePassword & _
                                "?o1=" & System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(txtPassword.Text))
                                Response.Redirect(sRedirect)
                            End If
                        Else
                            tblLogin.Visible = True
                            litLoginLabel.Visible = True
                            tblWelcome.Visible = False
                            'HP Issue#9078: display message indicating disabled account
                            If .Disabled Then
                                'lblError.Text = "Account has been disabled, please contact the administrator"
                                'SKB Issue#10654: display message indicating disabled account
                                lblError.Text = UserDisabledError
                                hlinkForgotUID.Visible = False
                            Else
                                lblError.Text = "Error logging in"
                            End If
                        End If
                    End With
                Else
                    lblError.Text = "Invalid User ID or Password."
                End If
               
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
            'Sheetal added condition for issue 20038:eBusiness: Redirect Page to Password Change Page In a Scenario
            If (WebUserLogin1.PasswordResetRequired = False OrElse WebUserLogin1.temporaryUsed = False) Then
                If bLoggedIn Then
                    OnUserLoggedIn()
                    'Sheetal Issue 20175, 10/09/2014 :Redirect user to change password when first time login
                    If CheckFirstLogin() Then
                        Session("ReturnToPage") = ""
                    End If
                    'Neha Issue 14408,03/20/13,Added changepassword redirection on firstlogin
                    If Session("UserLoggedIn") = True AndAlso CheckFirstLogin() Then
                        'Response.Redirect(ChangePassword, False)
                        CheckNewUser = True
                        Session("CheckNewUser") = True
                    Else
                        CheckNewUser = False
                    End If
                    Response.Redirect(Request.RawUrl)
                End If
            End If
        End Sub
        'Neha Issue 14408,03/20/13,check firstlogin for user by tracking session count value
        Private Function CheckFirstLogin() As Boolean
            Try
                Dim sSql As String = String.Empty
                Dim dtChkFLogin As New System.Data.DataTable
                Dim bChkFLogin As Boolean = False
                sSql = "SELECT SessionCount FROM " & Database & _
                   ".." & AptifyApplication.GetEntityBaseView("Web Users") & " Where ID= " & WebUserLogin1.User.UserID
                dtChkFLogin = DataAction.GetDataTable(sSql)
                If Not dtChkFLogin Is Nothing AndAlso dtChkFLogin.Rows.Count > 0 AndAlso dtChkFLogin.Rows(0)("SessionCount") = "1" Then
                    bChkFLogin = True
                End If
                Return bChkFLogin
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return False
            End Try
        End Function

        Protected Sub cmdLogOut_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdLogOut.Click
            ' Log Out to the Aptify Enterprise Web Architecture
            ' Use the AptifyWebLogin Component to do this
            Dim bLoggedOut As Boolean = False
            Try
                If WebUserLogin1.Logout() Then
                    lblError.Text = ""
                    tblLogin.Visible = True
                    litLoginLabel.Visible = True
                    tblWelcome.Visible = False
                    ShoppingCartLogin.Clear()
                    'Suraj Issue 15370,7/31/13 revert back changes application variale to session variable
                    Session.Remove("ReturnToPage")
                    WebUserLogin1.ClearAutoLoginCookie(Page.Response)
                    'HP Issue#9078: clear and delete session
                    Session.Clear()
                    Session.Abandon()
                    bLoggedOut = True
                    'RashmiP, Call Clear Cookies function
                    ClearCookies()
                    Session("SocialNetwork") = Nothing
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try

            If bLoggedOut Then
                OnUserLoggedOut()
            End If
        End Sub

        Protected Overrides Sub OnPreRender(ByVal e As System.EventArgs)
            Try
                MyBase.OnPreRender(e)
                With WebUserLogin1
                    If .User.UserID <= 0 Then
                        tblLogin.Visible = True
                        litLoginLabel.Visible = True
                        tblWelcome.Visible = False
                        chkAutoLogin.Checked = False
                    Else
                        tblLogin.Visible = False
                        litLoginLabel.Visible = False
                        tblWelcome.Visible = True
                        lblWelcome.Text = "Welcome, " & _
                                          .User.FirstName & " " & _
                                          .User.LastName
                    End If
                End With
                'Anil B for  Issue 13882 on 18-03-2013
                'Set Remember option for login             
                If Request.Browser.Cookies Then
                    'Check if the cookies with name LOGIN exist on user's machine
                    If Request.Cookies("LOGIN") IsNot Nothing AndAlso Request.Cookies("LOGIN").Item("RememberMe") IsNot Nothing Then
                        'Pass the user name and password to the VerifyLogin method
                        chkAutoLogin.Checked = CBool(Request.Cookies("LOGIN").Item("RememberMe"))
                    End If
                End If
                'lblLogin.Visible = Me.ShowTitle
                'If lblLogin.Visible Then
                '    lblLogin.InnerText = Me.TitleString
                'End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub cmdNewUser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNewUser.Click

            Try
                Session("SocialNetwork") = Nothing
                Response.Redirect(NewUserPage, False)
            Catch ex As Exception

            End Try

        End Sub

        Protected Overridable Sub OnUserLoggedIn()

            Dim sRedirectLocation As String = ""

            Try
                RaiseEvent UserLoggedIn()
                'Sapna DJ Issue 12545
                Session("UserLoggedIn") = True
                'Suraj Issue 15370,7/31/13 revert back changes application variale to session variable
                If Len(Session("ReturnToPage")) <> 0 Then
                    Dim sTemp As String
                    sTemp = CStr((Session("ReturnToPage")))
                    Session("ReturnToPage") = "" ' used only once
                    sRedirectLocation = sTemp
                    Response.Redirect(sTemp)
                Else
                    ' refresh current page
                    sRedirectLocation = Request.RawUrl
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try

            'Response.Redirect(sRedirectLocation)
        End Sub

        Protected Overridable Sub OnUserLoggedOut()
            RaiseEvent UserLoggedOut()
            'Sapna DJ Issue 12545
            If Not Session Is Nothing Then
                Session("UserLoggedIn") = False
            End If
            Response.Redirect(HomePage)
        End Sub
        'SKB Issue#10654: display message indicating disabled account
        ''' <summary>
        '''Error displayed if user is disabled
        ''' </summary>
        Public Overridable ReadOnly Property UserDisabledError() As String
            Get
                If Not Session.Item(ATTRIBUTE_USER_DISABLED_ERROR) Is Nothing Then
                    Return CStr(Session.Item(ATTRIBUTE_USER_DISABLED_ERROR))
                Else
                    Dim value As String = Me.GetGlobalAttributeValue(ATTRIBUTE_USER_DISABLED_ERROR)
                    If Not String.IsNullOrEmpty(value) Then
                        Session.Item(ATTRIBUTE_USER_DISABLED_ERROR) = value
                        Return value
                    Else
                        Return String.Empty
                    End If
                End If
            End Get
        End Property
        Protected Sub chkAutoLogin_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAutoLogin.CheckedChanged
            'Anil B for  Issue 13882 on 18-03-2013
            'Set Remember option for login
            If Request.Browser.Cookies Then
                'Check if the cookie with name LOGIN exist on user's machine
                'Create a cookie with expiry of 30 days
                Response.Cookies("LOGIN").Expires = DateTime.Now.AddDays(30)
                'Write username to the cookie
                Response.Cookies("LOGIN").Item("RememberMe") = chkAutoLogin.Checked
            End If
        End Sub

        ''RashmiP, Issue adding cookie to validate lms user
        '' Issue 17849: Need to Secure Tin Can Content Access
        Public Overridable Sub AddCookieForLMSUser(ByVal Request As System.Web.HttpRequest, _
                                                        ByVal Response As System.Web.HttpResponse, _
                                                        ByVal UserID As String)

            Dim oCookie As System.Web.HttpCookie
            Try


                oCookie = New System.Web.HttpCookie("APTIFY_LMS_UID", UserID)
                If Me.Page IsNot Nothing AndAlso Page.Request.IsSecureConnection Then
                    oCookie.Secure = True
                End If
                oCookie.Expires = DateAdd("yyyy", 1, Date.Today)
                Response.Cookies.Add(oCookie)

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        '' Added by suvarna 17790

        ''' <summary>
        ''' This method is responsible for decrypting a stored password.
        ''' </summary>
        ''' <param name="Application"></param>
        ''' <param name="UserInfo"></param>
        ''' <param name="UserID"></param>
        ''' <returns></returns>
        ''' <remarks>Modified: 05.08.14 - JSH - Password reset functionality added</remarks>
        Protected Function DecryptStoredPassword(ByVal Application As System.Web.HttpApplicationState, _
                                                 ByVal UserInfo As System.Security.Principal.IPrincipal, _
                                                 ByVal UserID As String) As String
            Dim sSQL As String
            Dim oSecurity As Aptify.Framework.BusinessLogic.Security.AptifySecurityKey
            Dim da As DataAction
            Dim sResult As Object = Nothing
            Dim uc As UserCredentials
            Dim g As EBusinessGlobal
            Dim oApp As AptifyApplication
            Dim colParams(0) As Data.IDataParameter
            Dim pUserID As Data.IDataParameter = Nothing
            Dim dtCheckTemp As DataTable
            Dim bTemporaryLogin As Boolean = False
            Try
                g = New EBusinessGlobal
                uc = g.GetLoginCredentials(Application, UserInfo)
                oSecurity = New Aptify.Framework.BusinessLogic.Security.AptifySecurityKey(uc)
                'HP Issue#7812: replace all AptifyApplication references to that provided by EBusinessGlobal
                'oApp = New AptifyApplication(uc)
                oApp = g.GetAptifyApplication(Application, UserInfo)
                Dim sBaseDatabase As String = oApp.GetEntityBaseDatabase("Web Users")

                da = New DataAction(uc)
                pUserID = da.GetDataParameter("@UserID", SqlDbType.NVarChar, UserID)

                '05.08.14 - JSH
                sSQL = "SELECT ISNULL(temporaryPWD,'') temporaryPWD, ISNULL(PasswordResetRequestDate,'') PasswordResetRequestDate FROM " & sBaseDatabase & _
                        "..vwWebUsers WHERE UserID=@UserID"
                colParams(0) = pUserID
                dtCheckTemp = da.GetDataTableParametrized(sSQL, CommandType.Text, colParams)
                If String.IsNullOrEmpty(dtCheckTemp.Rows(0)("temporaryPWD").ToString) = False Then
                    Dim ts As TimeSpan = Now - CDate(dtCheckTemp.Rows(0)("PasswordResetRequestDate"))
                    If CInt(ts.TotalMinutes) < 1440 Then
                        'sResult = dtCheckTemp.Rows(0)("temporaryPWD").ToString.Trim()
                        If PasswordIsHashed(Application, UserInfo) Then
                            temporaryPWD = oSecurity.AptifySecurityKeyObject(WebUserLogin1.GetEbusinessLoginSecurityKey()).DecryptData(WebUserLogin1.GetEbusinessLoginSecurityKey(), CStr(dtCheckTemp.Rows(0)("temporaryPWD")), True)
                        Else
                            temporaryPWD = oSecurity.DecryptData(WebUserLogin1.GetEbusinessLoginSecurityKey(), CStr(dtCheckTemp.Rows(0)("temporaryPWD")))
                        End If
                        PasswordResetRequired = True
                        bTemporaryLogin = True
                    Else
                        bTemporaryLogin = False
                    End If
                Else
                    bTemporaryLogin = False
                End If
                'End 05.08.14 Changes

                'If Not bTemporaryLogin Then
                'NK 12/19/06 Parameterize query for added security
                sSQL = "SELECT ISNULL(PWD,'') FROM " & sBaseDatabase & "..vwWebUsers WHERE UserID=@UserID"
                colParams(0) = pUserID
                sResult = da.ExecuteScalarParametrized(sSQL, CommandType.Text, colParams)
                'End If
                If Not sResult Is Nothing Then
                    If Len(sResult) > 0 Then
                        If PasswordIsHashed(Application, UserInfo) Then
                            'Added 04.23.14 - JSH
                            Return oSecurity.AptifySecurityKeyObject(WebUserLogin1.GetEbusinessLoginSecurityKey()).DecryptData(WebUserLogin1.GetEbusinessLoginSecurityKey(), CStr(sResult), True)
                        Else
                            'Legacy method
                            Return oSecurity.DecryptData(WebUserLogin1.GetEbusinessLoginSecurityKey(), CStr(sResult))
                        End If
                    Else
                        Return ""
                    End If
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return Nothing
            End Try
        End Function

        'HP Issue#9078:     
        ''' <summary>
        ''' This method is responsible for returning the 'disable' status of an e-buisiness user.
        ''' </summary>
        Private Function IsUserDisabled(ByVal Application As System.Web.HttpApplicationState, _
                                        ByVal UserInfo As System.Security.Principal.IPrincipal, _
                                        ByVal UserID As String) As Boolean
            Dim sSQL As String
            Dim da As DataAction
            Dim sResult As Object
            Dim uc As UserCredentials
            Dim g As EBusinessGlobal
            Dim oApp As AptifyApplication
            Dim colParams(0) As Data.IDataParameter
            Dim pUserID As Data.IDataParameter = Nothing

            Try
                g = New EBusinessGlobal
                uc = g.GetLoginCredentials(Application, UserInfo)
                'HP Issue#7812: replace all AptifyApplication references to that provided by EBusinessGlobal
                'oApp = New AptifyApplication(uc)
                oApp = g.GetAptifyApplication(Application, UserInfo)

                da = New DataAction(uc)

                sSQL = "SELECT Disabled FROM " & oApp.GetEntityBaseDatabase("Web Users") & _
                       "..vwWebUsers WHERE UserID=@UserID"

                pUserID = da.GetDataParameter("@UserID", SqlDbType.NVarChar, UserID)
                colParams(0) = pUserID
                sResult = da.ExecuteScalarParametrized(sSQL, CommandType.Text, colParams)

                If sResult Is Nothing Or CStr(sResult) = "" Then
                    Return False
                Else
                    m_disabled = CBool(sResult)
                End If

                Return Disabled()

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Function
        'HP Issue#9078:    
        ''' <summary>
        ''' This method handles checking the number login attempts and eventual disabling of the e-Business user account after 
        ''' the number of tries specified has been reached.
        ''' </summary>
        Private Sub UpdateNumTries(ByVal Application As System.Web.HttpApplicationState, _
                                   ByVal UserInfo As System.Security.Principal.IPrincipal, _
                                   ByVal UserID As String, ByVal MaxLogins As Integer)
            Dim sSQL As String
            Dim da As DataAction
            Dim sResult As DataTable
            Dim uc As UserCredentials
            Dim g As EBusinessGlobal
            Dim oApp As AptifyApplication
            Dim colParams(0) As Data.IDataParameter
            Dim pUserID As Data.IDataParameter = Nothing
            Dim pLock As Integer

            Try
                g = New EBusinessGlobal
                uc = g.GetLoginCredentials(Application, UserInfo)
                'HP Issue#7812: replace all AptifyApplication references to that provided by EBusinessGlobal
                'oApp = New AptifyApplication(uc)
                oApp = g.GetAptifyApplication(Application, UserInfo)

                da = New DataAction(uc)

                pUserID = da.GetDataParameter("@User", SqlDbType.NVarChar, UserID)
                colParams(0) = pUserID
                sSQL = "SELECT ID, NumFailedLoginAttempts FROM " & oApp.GetEntityBaseDatabase("Web Users") & _
                       "..vwWebUsers WHERE UserID=@User"
                sResult = da.GetDataTableParametrized(sSQL, CommandType.Text, colParams)

                If MaxLogins <= CInt(sResult.Rows(0)(1)) + 1 Then
                    pLock = 1
                    m_disabled = True
                Else
                    pLock = 0
                End If

                sSQL = oApp.GetEntityBaseDatabase("Web Users") & _
                       "..spUpdateWebUsers @ID= " & CInt(sResult.Rows(0)(0)) & ", " & _
                                          "@UserID = NULL, @PWD= NULL, @FirstName = NULL, @LastName = NULL, @Title = NULL, " & _
                                          "@Email = NULL, @Company = NULL, @Disabled = " & pLock & ", @LinkID = NULL, @SessionCount = NULL, " & _
                                          "@NumFailedLoginAttempts = " & CInt(sResult.Rows(0)(1)) + 1 & ", @PasswordHint = NULL, @PasswordHintAnswer = NUll "

                da.ExecuteScalar(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        ''' <summary>
        ''' This method handles the actual login process and is overridable by sub-classes to alter the base processing
        ''' logic for logging in an e-Business user.
        ''' </summary>
        ''' <param name="UserID"></param>
        ''' <param name="Password"></param>
        ''' <param name="MaxLoginTries"></param>
        ''' <param name="Session"></param>
        ''' <param name="Application"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Overridable Function InternalLogin(ByVal UserID As String, _
                                                     ByVal Password As String, _
                                                     ByVal MaxLoginTries As Integer, _
                                                     ByVal Session As System.Web.SessionState.HttpSessionState, _
                                                     ByVal Application As System.Web.HttpApplicationState, _
                                                     ByVal UserInfo As System.Security.Principal.IPrincipal) As Boolean
            ' this function attempts to login the user to the database
            ' if success, return TRUE, o.w. return FALSE
            Dim sSQL As String
            Dim iResult As Object
            Dim sDecryptServerPWD As String
            Dim da As DataAction
            Dim uc As UserCredentials
            Dim oApp As AptifyApplication
            Dim g As EBusinessGlobal

            Try

                '6/29/06 RJK - If the Application object (HTTPSessionState) is nothing, grab it from the
                'page.

                If Application Is Nothing Then
                    Application = Me.Page.Application
                End If
                If Session Is Nothing Then
                    Session = Me.Page.Session
                End If

                'HP Issue#9078:  first make sure account is not disabled
                If IsUserDisabled(Application, UserInfo, UserID) Then
                    Return False
                End If

                'We now validate the password authentication in this component rather than in the
                'spLoginUser stored procedure, using appropriate encryption algorithms.  RN 3/25/03.
                sDecryptServerPWD = DecryptStoredPassword(Application, UserInfo, UserID)

                If PasswordIsHashed(Application, UserInfo) = False Then
                    If sDecryptServerPWD Is Nothing OrElse sDecryptServerPWD.Length = 0 Then
                        'No user match or there is no access to the encryption key.
                        Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(New Exception("No user match or there is no access to the encryption key."))
                        Return False
                    ElseIf String.Compare(Password, sDecryptServerPWD) <> 0 Then
                        If String.Compare(Password, temporaryPWD) <> 0 Then
                            Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(New Exception("Compare Failed."))
                            'HP Issue#9078: update number of tries and disable account if necessary
                            '0 indicates unlimited tires
                            If MaxLoginTries > 0 Then
                                UpdateNumTries(Application, UserInfo, UserID, MaxLoginTries)
                            End If
                            Return False
                        Else
                            Password = temporaryPWD 'We're logging in with our temporary
                            temporaryUsed = True
                        End If
                    End If
                Else
                    'Added 04.23.14 - JSH
                    If CheckHash(Application, UserInfo, sDecryptServerPWD, Password) = False Then
                        'If String.Compare(GetHash(Application, UserInfo, Password).Trim(), sDecryptServerPWD.Trim(), True) <> 0 Then
                        If CheckHash(Application, UserInfo, temporaryPWD, Password) = False Then
                            ExceptionManagement.ExceptionManager.Publish(New Exception("Compare Failed."))
                            If MaxLoginTries > 0 Then
                                UpdateNumTries(Application, UserInfo, UserID, MaxLoginTries)
                            End If
                            Return False
                        Else
                            Password = temporaryPWD   'We're using our temporary password
                            temporaryUsed = True
                        End If
                    End If
                    'End new branch for one way hash logic
                End If

                g = New EBusinessGlobal
                uc = g.GetLoginCredentials(Application, UserInfo)
                'HP Issue#7812: replace all AptifyApplication references to that provided by EBusinessGlobal
                'oApp = New AptifyApplication(uc)
                oApp = g.GetAptifyApplication(Application, UserInfo)

                da = New DataAction(uc)

                'HP, Issue 8978: escape apostrophe in username and password
                sSQL = oApp.GetEntityBaseDatabase("Web Users") & _
                       "..spLoginUser @UserID='" & UserID.Replace("'", "''") & "'," & _
                       "@PWD='" & Password.Replace("'", "''") & _
                       "',@MaxLoginTries=" & MaxLoginTries

                iResult = da.ExecuteScalar(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)
                If Not iResult Is Nothing AndAlso _
                   IsNumeric(iResult) AndAlso _
                   CInt(iResult) > 0 Then
                    LoadUserInfo(da, UserID, Application, Session, UserInfo)

                    ' Vijay Sitlani - 10-22-07
                    ' Check and Create Fun.
                    CheckAndCreateCMSLogin(UserID)

                    '2008/02/05 MAS: TEST CODE!!
                    Me.LoginCMSUser(UserID)

                    ' Issue # 12582 fix - Sapna DJ - Remove any link of session object to application object
                    'Aptify.Framework.Web.eBusiness.User.LinkSessionToApplication(Application, Session)
                    Return True
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Function
        Protected Overrides Function SaveViewState() As Object
            Try
                If Page.Items("IsInIndexMode") Is Nothing Then
                    m_oUser.SaveValuesToSessionObject(Page.Session)
                End If
                Return MyBase.SaveViewState
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return Nothing
            End Try
        End Function

        Protected Overrides Sub TrackViewState()
            Try
                If m_oUser IsNot Nothing Then
                    Me.Controls.Add(CType(m_oUser, System.Web.UI.Control))
                    If Page.Items("IsInIndexMode") Is Nothing Then
                        m_oUser.LoadValuesFromSessionObject(Page.Session)
                    End If
                End If
                MyBase.TrackViewState()
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Public Sub New()
            m_oUser = New Aptify.Framework.Web.eBusiness.User()
        End Sub

        ''' <summary>
        ''' This method will pull the UserID and Password from an existing cookie,
        ''' Decrypt them and login the user.
        ''' </summary>
        ''' <param name="Request"></param>
        ''' <param name="Response"></param>
        ''' <param name="Session"></param>
        ''' <param name="Application"></param>
        ''' <param name="UserInfo"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function AutoLogin(ByVal Request As System.Web.HttpRequest, _
                                                   ByVal Response As System.Web.HttpResponse, _
                                                   ByVal Session As System.Web.SessionState.HttpSessionState, _
                                                   ByVal Application As System.Web.HttpApplicationState, _
                                                   ByVal UserInfo As System.Security.Principal.IPrincipal) As Boolean Implements ILogin.AutoLogin
            Dim oCookie As System.Web.HttpCookie
            Dim sUID As String
            Dim sPWD As String

            Try
                oCookie = Request.Cookies.Item("APTIFY_ECOMMERCE_UID")
                If Not oCookie Is Nothing Then
                    If oCookie.Value <> "" Then
                        sUID = oCookie.Value
                        oCookie = Request.Cookies.Item("APTIFY_ECOMMERCE_PWD")
                        sPWD = oCookie.Value

                        ''''''''''''''''''''''''''''''''''''''''
                        '' Issue 5058
                        '' Cookie's UserID and Password should be encrypted. Will first decrypt.
                        Dim oSecurity As Aptify.Framework.BusinessLogic.Security.AptifySecurityKey
                        Dim uc As UserCredentials
                        Dim g As EBusinessGlobal
                        Dim decryptedUID, decryptedPW As String
                        g = New EBusinessGlobal
                        uc = g.GetLoginCredentials(Application, UserInfo)
                        oSecurity = New Aptify.Framework.BusinessLogic.Security.AptifySecurityKey(uc)
                        'Decrypt UserID and Password
                        decryptedUID = oSecurity.DecryptData(WebUserLogin1.GetEbusinessLoginSecurityKey(), sUID)
                        decryptedPW = oSecurity.DecryptData(WebUserLogin1.GetEbusinessLoginSecurityKey(), sPWD)
                        'Navin Prasad Issue 14330 
                        If InternalLogin(decryptedUID, decryptedPW, 0, Session, Application, UserInfo) Then
                            'Must pass in the unencrypted versions of UID and PW, 
                            'because AddAutoLoginCookie encrypts them itself
                            AddAutoLoginCookie(Response, decryptedUID, decryptedPW, uc)
                            AutoLogin = True
                            Session("UserLoggedIn") = True
                        Else
                            'For legacy support, let's try again assuming that the cookie's UID and PWD are not encrypted.
                            If InternalLogin(sUID, sPWD, 0, Session, Application, UserInfo) Then
                                'UserID and Password were not encrypted.
                                'Remove and re-add the Login Cookie, so AddAutoLoginCookie can encrypt them
                                ClearAutoLoginCookie(Response)
                                AddAutoLoginCookie(Response, sUID, sPWD, uc)
                                AutoLogin = True
                                Session("UserLoggedIn") = True
                            Else
                                ClearAutoLoginCookie(Response)
                            End If
                        End If
                    End If
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Function
        'Hooman 11-17-09, Issue 8961:  Overloaded method created to pass user-credentials when the method is called during Session_Start by EBizGlobal
        ''' <summary>
        ''' Adds the auto-login cookies to the response for future logins.
        ''' UserID and Password will be stored in an encrypted format.
        ''' </summary>
        ''' <param name="Response"></param>
        ''' <param name="UserID"></param>
        ''' <param name="Password"></param>
        ''' <remarks></remarks>
        '' <Description("Adds the auto-login cookies to the response for future logins")> _
        Public Overridable Sub AddAutoLoginCookie(ByVal Response As System.Web.HttpResponse, _
                                                  ByVal UserID As String, _
                                                  ByVal Password As String, ByVal UC As UserCredentials)

            Dim oCookie As System.Web.HttpCookie

            ''''''''''''''''''''''''''''''''''''''''
            '' Issue 5058
            '' added the below variables for use to encrypt UserID and Password
            Dim oSecurity As Aptify.Framework.BusinessLogic.Security.AptifySecurityKey

            'Hooman 11-17-09, Issue 8961: No longer needed, the object is passed thur arguments
            'Dim uc As UserCredentials
            'Dim g As EBusinessGlobal

            Try
                'Encrypt UserID and Password

                'Hooman 11-17-09, Issue 8961: No longer needed, the object is passed thur arguments
                'g = New EBusinessGlobal
                'uc = g.GetLoginCredentials(Me.Page.Application, Me.Page.User)

                oSecurity = New Aptify.Framework.BusinessLogic.Security.AptifySecurityKey(UC)
                UserID = oSecurity.EncryptData(WebUserLogin1.GetEbusinessLoginSecurityKey(), UserID)
                Password = oSecurity.EncryptData(WebUserLogin1.GetEbusinessLoginSecurityKey(), Password)
                '' End Issue 5058 changes
                '''''''''''''''''''''''''''''''''''''''''

                oCookie = New System.Web.HttpCookie("APTIFY_ECOMMERCE_UID", UserID)
                'HP Issue#9078: when the page is being transmitted via SSL then also secure the cookie to only transmit via SSL

                'Anil B 16071 09-05-2013
                'Checked if page is nothing
                If Me.Page IsNot Nothing AndAlso Page.Request.IsSecureConnection Then
                    oCookie.Secure = True
                End If
                oCookie.Expires = DateAdd("yyyy", 1, Date.Today)
                Response.Cookies.Add(oCookie)

                oCookie = New System.Web.HttpCookie("APTIFY_ECOMMERCE_PWD", Password)
                oCookie.Expires = DateAdd("yyyy", 1, Date.Today)
                Response.Cookies.Add(oCookie)
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        'Hooman 11-17-09, Issue 8961:  Maintaining method for backwards compatability, now checking for Page refrence before calling overloaded method.
        ''' <summary>
        ''' Adds the auto-login cookies to the response for future logins.
        ''' UserID and Password will be stored in an encrypted format.
        ''' </summary>
        ''' <param name="Response"></param>
        ''' <param name="UserID"></param>
        ''' <param name="Password"></param>
        ''' <remarks></remarks>
        <Description("Adds the auto-login cookies to the response for future logins")> _
        Public Overridable Sub AddAutoLoginCookie(ByVal Response As System.Web.HttpResponse, _
                                                  ByVal UserID As String, _
                                                  ByVal Password As String) Implements ILogin.AddAutoLoginCookie


            Try
                'Make sure method is being called from a Page
                If Me.Page Is Nothing Then
                    Throw New Exception("Method requires a valid 'Page' object reference, please use the overloaded method requiring a UserCredential object. " & _
                                        "Method should be called from a web-page or user-control.")

                Else
                    'call is valid, create required user-credentials for new method
                    Dim g As EBusinessGlobal = New EBusinessGlobal
                    Dim uc As UserCredentials
                    uc = g.GetLoginCredentials(Me.Page.Application, Me.Page.User)
                    AddAutoLoginCookie(Response, UserID, Password, uc)
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try

        End Sub

        Public Overridable Sub ClearAutoLoginCookie(ByVal Response As System.Web.HttpResponse) Implements ILogin.ClearAutoLoginCookie
            Dim oCookie As System.Web.HttpCookie

            Try
                oCookie = Response.Cookies.Item("APTIFY_ECOMMERCE_UID")
                If Not oCookie Is Nothing Then
                    oCookie.Value = ""
                End If
                oCookie = Response.Cookies.Item("APTIFY_ECOMMERCE_PWD")
                If Not oCookie Is Nothing Then
                    oCookie.Value = ""
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        'Vijay Sitlani
        Private m_oCmsIntegrationGeneric As CMSIntegrationGeneric
        Private m_oCmsIntegration As CMSIntegrationInterface
        Private m_oApplication As Aptify.Framework.Application.AptifyApplication

        Private Sub CheckAndCreateCMSLogin(ByVal AptifyWebUserName As String)
            Dim lstAllCMSs As List(Of GenericEntity.AptifyGenericEntity)
            Dim oRep As New ObjectRepository.AptifyObjectRepository(Me.m_oApplication.UserCredentials)
            m_oCmsIntegrationGeneric = New CMSIntegrationGeneric()
            lstAllCMSs = m_oCmsIntegrationGeneric.GetCMSList(Me.m_oApplication, True)
            For Each cmsSystem As GenericEntity.AptifyGenericEntity In lstAllCMSs
                m_oCmsIntegration = TryCast(Aptify.Framework.Application.InstanceCreator.CreateInstanceFromName(cmsSystem.GetValue("CMSAssembly").ToString, cmsSystem.GetValue("CMSClass").ToString), CMSIntegrationInterface)
                m_oCmsIntegration.Config(Me.m_oApplication, cmsSystem.RecordID)
                m_oCmsIntegration.CheckAndCreateUser(AptifyWebUserName)
            Next
        End Sub

        Private Sub LoginCMSUser(ByVal AptifyWebUserName As String)
            'This implementation is specific to Ektron CMS400
            Dim lstAllCMSs As List(Of GenericEntity.AptifyGenericEntity)
            Dim oRep As New ObjectRepository.AptifyObjectRepository(Me.m_oApplication.UserCredentials)
            m_oCmsIntegrationGeneric = New CMSIntegrationGeneric()
            lstAllCMSs = m_oCmsIntegrationGeneric.GetCMSList(Me.m_oApplication, True)
            Dim bResult As Boolean = False
            For Each cmsSystem As GenericEntity.AptifyGenericEntity In lstAllCMSs
                '2008/02/13 MAS: Should only do this if CMS Record is set to Active
                If CBool(cmsSystem.GetValue("Active")) Then
                    m_oCmsIntegration = TryCast(Aptify.Framework.Application.InstanceCreator.CreateInstanceFromName(cmsSystem.GetValue("CMSAssembly").ToString, cmsSystem.GetValue("CMSClass").ToString), CMSIntegrationInterface)
                    m_oCmsIntegration.Config(Me.m_oApplication, cmsSystem.RecordID)
                    bResult = m_oCmsIntegration.LoginCMSUser(AptifyWebUserName, Me.m_oUser.Password, Page)
                End If
            Next
        End Sub
        ''' <summary>
        ''' Returns a boolean value identifying if the E Business Login Key is using a one way hash algorithm.
        ''' </summary>
        ''' <param name="Application">Current Application object</param>
        ''' <param name="UserInfo">Current UserInfo object</param>
        ''' <returns>Boolean</returns>
        ''' <remarks>Added 04.23.14 - JSH</remarks>
        Private Function PasswordIsHashed(ByVal Application As System.Web.HttpApplicationState, _
                                                 ByVal UserInfo As System.Security.Principal.IPrincipal) As Boolean
            Dim bReturn As Boolean = False  'Default to legacy compatibility
            Try
                Dim oSecurity As Aptify.Framework.BusinessLogic.Security.AptifySecurityKey
                Dim oUserCred As UserCredentials
                Dim oGlobal As EBusinessGlobal
                oGlobal = New EBusinessGlobal
                oUserCred = oGlobal.GetLoginCredentials(Application, UserInfo)
                oSecurity = New Aptify.Framework.BusinessLogic.Security.AptifySecurityKey(oUserCred)
                bReturn = oSecurity.IsHashKey(WebUserLogin1.GetEbusinessLoginSecurityKey())
            Catch ex As Exception
                ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
            Return bReturn
        End Function
        ''' <summary>
        ''' Returns a cipher text hash based upon the algorithmic settings of the E Business Login Key
        ''' </summary>
        ''' <param name="Application">Current Application object</param>
        ''' <param name="UserInfo">Current UserInfo object</param>
        ''' <param name="ValueToHash">Unencrypted text value to hash</param>
        ''' <returns>Hashed cipher text string</returns>
        ''' <remarks>Added 04.23.14 - JSH</remarks>
        Private Function GetHash(ByVal Application As System.Web.HttpApplicationState, _
                                                 ByVal UserInfo As System.Security.Principal.IPrincipal,
                                                 ByVal ValueToHash As String) As String
            Dim sReturn As String = String.Empty
            Try
                Dim oSecurity As Aptify.Framework.BusinessLogic.Security.AptifySecurityKey
                Dim oUserCred As UserCredentials
                Dim oGlobal As EBusinessGlobal
                oGlobal = New EBusinessGlobal
                oUserCred = oGlobal.GetLoginCredentials(Application, UserInfo)
                oSecurity = New Aptify.Framework.BusinessLogic.Security.AptifySecurityKey(oUserCred)
                sReturn = oSecurity.EncryptData(WebUserLogin1.GetEbusinessLoginSecurityKey(), ValueToHash)
            Catch ex As Exception
                ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
            Return sReturn
        End Function
        ''' <summary>
        ''' Returns a cipher text hash based upon the algorithmic settings of the E Business Login Key
        ''' </summary>
        ''' <param name="Application">Current Application object</param>
        ''' <param name="UserInfo">Current UserInfo object</param>
        ''' <param name="HashToCompare">Hash value to compare</param>
        ''' <param name="StringToMatch">ClearText string to validate</param>
        ''' <returns>Boolean</returns>
        ''' <remarks>Added 04.23.14 - JSH</remarks>
        Private Function CheckHash(ByVal Application As System.Web.HttpApplicationState, _
                                   ByVal UserInfo As System.Security.Principal.IPrincipal, _
                                   ByVal HashToCompare As String, ByVal StringToMatch As String) As Boolean
            Dim sReturn As Boolean = False
            Try
                Dim oSecurity As Aptify.Framework.BusinessLogic.Security.AptifySecurityKey
                Dim oUserCred As UserCredentials
                Dim oGlobal As EBusinessGlobal
                oGlobal = New EBusinessGlobal
                oUserCred = oGlobal.GetLoginCredentials(Application, UserInfo)
                oSecurity = New Aptify.Framework.BusinessLogic.Security.AptifySecurityKey(oUserCred)
                If oSecurity.AptifySecurityKeyObject(WebUserLogin1.GetEbusinessLoginSecurityKey()).Match(WebUserLogin1.GetEbusinessLoginSecurityKey(), HashToCompare, StringToMatch) Then
                    sReturn = True
                End If
            Catch ex As Exception
                ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
            Return sReturn
        End Function
        Public Function IsHashKey(ByVal Application As System.Web.HttpApplicationState, _
                                   ByVal UserInfo As System.Security.Principal.IPrincipal, _
                                   ByVal sKeyName As String) As Boolean
            Dim bReturn As Boolean = False
            Try
                Dim oUserCred As UserCredentials
                Dim oGlobal As New EBusinessGlobal
                oUserCred = oGlobal.GetLoginCredentials(Application, UserInfo)
                Dim oSecurity As New Aptify.Framework.BusinessLogic.Security.AptifySecurityKey(oUserCred)
                bReturn = oSecurity.AptifySecurityKeyObject(sKeyName).IsHashKey(sKeyName)
            Catch ex As Exception
                ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
            Return bReturn
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="UserId"></param>
        ''' <param name="OldPassword"></param>
        ''' <param name="newpassword"></param>
        ''' <param name="Session"></param>
        ''' <param name="Application"></param>
        ''' <param name="UserInfo"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function UpdateUserPassword(ByVal UserId As String, ByVal OldPassword As String, ByVal NewPassword As String, ByVal Session As System.Web.SessionState.HttpSessionState, ByVal Application As System.Web.HttpApplicationState, ByVal UserInfo As System.Security.Principal.IPrincipal) As Integer
            Try
                Dim iResult As Integer = 0
                Dim sDecryptServerPWD As String = String.Empty
                If Application Is Nothing Then
                    Application = Me.Page.Application
                End If
                If Session Is Nothing Then
                    Session = Me.Page.Session
                End If
                sDecryptServerPWD = DecryptStoredPassword(Application, UserInfo, UserId)
                If PasswordIsHashed(Application, UserInfo) = False Then
                    If sDecryptServerPWD Is Nothing OrElse sDecryptServerPWD.Length = 0 Then
                        'No user match or there is no access to the encryption key.
                        Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(New Exception("No user match or there is no access to the encryption key."))
                        Return 1
                    ElseIf String.Compare(OldPassword, sDecryptServerPWD) <> 0 Then
                        If String.Compare(OldPassword, temporaryPWD) <> 0 Then
                            Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(New Exception("Compare Failed."))
                            Return 2
                        End If
                    End If
                Else
                    'Added 04.23.14 - JSH - One way hash functionality
                    If String.Compare(GetHash(Application, UserInfo, OldPassword).Trim(), sDecryptServerPWD.Trim(), True) <> 0 Then
                        If String.Compare(GetHash(Application, UserInfo, OldPassword).Trim(), temporaryPWD.Trim(), True) <> 0 Then
                            Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(New Exception("Compare Failed."))
                            Return 2
                        End If
                    End If
                End If
                If (OldPassword.ToString() = NewPassword.ToString()) Then
                    Return 4
                End If

                If m_oUser IsNot Nothing Then
                    m_oUser.Password = NewPassword
                    If m_oUser.Save() Then
                        Return 0
                    Else
                        Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(New Exception("Password updation failed."))
                        Return 3
                    End If

                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return 4
            End Try
        End Function
        Protected Sub LoadUserInfo(ByVal DataAction As DataAction, _
                              ByVal UserID As String, _
                              ByVal Application As System.Web.HttpApplicationState, _
                              ByVal Session As System.Web.SessionState.HttpSessionState, _
                              ByVal UserInfo As System.Security.Principal.IPrincipal)
            Try
                Me.LoadUserObjectFromWebUserID(Me.m_oUser, DataAction, UserID, Application, UserInfo)

                If Session Is Nothing Then
                    m_oUser.SaveValuesToSessionObject(Page.Session)
                Else
                    'Neha Changes for hotfix issue 10935, skipped the code while indexing process when page is in index mode.
                    If Page.Items("IsInIndexMode") Is Nothing Then
                        m_oUser.SaveValuesToSessionObject(Session)
                    End If
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        ''' <summary>
        ''' This method will load up a user object based on the provided WebUserID
        ''' </summary>
        ''' <param name="User"></param>
        ''' <param name="DataAction"></param>
        ''' <param name="WebUserID"></param>
        ''' <remarks></remarks>
        Protected Overridable Sub LoadUserObjectFromWebUserID(ByVal User As IUser, _
                                               ByVal DataAction As DataAction, _
                                               ByVal WebUserID As String, _
                                               ByVal Application As System.Web.HttpApplicationState, _
                                               ByVal UserInfo As System.Security.Principal.IPrincipal)
            Dim g As New EBusinessGlobal
            Dim oApp As Aptify.Framework.Application.AptifyApplication
            Dim sSQL As String
            Dim dt As DataTable
            Dim i As Integer
            Dim colParams(0) As Data.IDataParameter
            Dim pUserID As Data.IDataParameter = Nothing
            Dim ds As DataSet
            Dim lPersonOrCompanyCurrencyTypeID As Long = -1
            Dim bCompanyPreferredCurrencyTypeIdExist As Boolean = False


            'HP Issue#7812: replace all AptifyApplication references to that provided by EBusinessGlobal
            'oApp = New AptifyApplication(g.GetLoginCredentials(Application, UserInfo))
            oApp = g.GetAptifyApplication(Application, UserInfo)

            'Vijay Sitlani
            m_oApplication = oApp

            'NK 12/19/06 Parameterize query for added security
            'sSQL = "SELECT * FROM " & oApp.GetEntityBaseDatabase("Web Users") & _
            '      "..vwWebUsers WHERE UserID=@UserID"

            ' Sapna DJ 12/27/2011- Issue #12545 - Investigate Ways to Reduce Size of Session Objects
            sSQL = oApp.GetEntityBaseDatabase("Web Users") & ".." + oApp.GetEntityAttribute("Web Users", "GetEBusinessWebUserDetailsStoredProcedure")

            pUserID = DataAction.GetDataParameter("@WebUserUserID", SqlDbType.NVarChar, WebUserID)
            colParams(0) = pUserID
            'dt = DataAction.GetDataTableParametrized(sSQL, CommandType.Text, colParams)
            ds = DataAction.GetDataSetParametrized(sSQL, CommandType.StoredProcedure, colParams) 'returns multiple results web user, person, company

            If Not ds Is Nothing AndAlso ds.Tables.Count > 0 Then
                With m_oUser
                    'Read and stored WebUser resultset values
                    Dim dtWebUser As DataTable = ds.Tables(0)
                    If Not dtWebUser Is Nothing AndAlso dtWebUser.Rows.Count > 0 Then
                        Dim drWebUser As DataRow = dtWebUser.Rows(0)
                        .WebUserDataRow = drWebUser
                        .WebUserStringID = WebUserID
                        .UserID = CLng(drWebUser.Item("UserID"))
                        .FirstName = drWebUser.Item("FirstName").ToString
                        .LastName = drWebUser.Item("LastName").ToString
                        .Title = drWebUser.Item("Title").ToString
                        .Company = drWebUser.Item("Company").ToString
                        .Email = drWebUser.Item("Email").ToString
                        .PersonID = CLng(drWebUser.Item("LinkID"))
                        'other elements
                        For i = 0 To drWebUser.ItemArray.Length - 1
                            If Not IsDBNull(drWebUser(i)) Then
                                .SetAddValue(dtWebUser.Columns(i).ColumnName, Trim(CStr(drWebUser(i))))
                            End If
                        Next
                    End If

                    'Read and store Person resultset values
                    Dim dtPerson As DataTable = ds.Tables(1)
                    If Not dtPerson Is Nothing AndAlso dtPerson.Rows.Count > 0 Then
                        Dim drPerson As DataRow = dtPerson.Rows(0)

                        .PersonDataRow = drPerson
                        If IsNumeric(drPerson.Item("PreferredCurrencyTypeID")) Then
                            lPersonOrCompanyCurrencyTypeID = CLng(drPerson.Item("PreferredCurrencyTypeID"))
                        End If

                        'other elements
                        For i = 0 To drPerson.ItemArray.Length - 1
                            If Not IsDBNull(drPerson(i)) Then
                                If dtPerson.Columns(i).DataType.Name = "Byte[]" Then
                                    .PersonPhoto = DirectCast(drPerson("Photo"), Byte())
                                    .SetValue(dtPerson.Columns(i).ColumnName, DirectCast(drPerson(i), Byte()))
                                Else
                                    .SetAddValue(dtPerson.Columns(i).ColumnName, Trim(CStr(drPerson(i))))
                                End If
                            End If
                        Next
                    End If



                    'Read and store Company result set values
                    Dim dtCompany As DataTable = ds.Tables(2)
                    If Not dtCompany Is Nothing AndAlso dtCompany.Rows.Count > 0 Then
                        Dim drCompany As DataRow = dtCompany.Rows(0)
                        .CompanyID = CLng(drCompany.Item("CompanyID"))
                        If IsNumeric(drCompany.Item("PreferredCurrencyTypeID")) Then
                            lPersonOrCompanyCurrencyTypeID = CLng(drCompany.Item("PreferredCurrencyTypeID"))
                            bCompanyPreferredCurrencyTypeIdExist = True
                        End If
                        .PreferredCurrencyTypeID = Me.DeterminePreferredCurrency(oApp, DataAction, .PersonID, .CompanyID, lPersonOrCompanyCurrencyTypeID, bCompanyPreferredCurrencyTypeIdExist)
                        .CompanyDataRow = drCompany
                    Else
                        .CompanyID = -1
                        .CompanyDataRow = Nothing
                    End If



                End With
            End If

            ' Sapna DJ 12/27/2011- commented old code - Issue #12545 - Investigate Ways to Reduce Size of Session Objects
            'If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
            '    Dim dr As DataRow = dt.Rows(0)

            '    With m_oUser
            '        .WebUserDataRow = dr
            '        .WebUserStringID = WebUserID
            '        .UserID = CLng(dr.Item("ID"))
            '        .FirstName = dr.Item("FirstName").ToString
            '        .LastName = dr.Item("LastName").ToString
            '        .Title = dr.Item("Title").ToString
            '        .Company = dr.Item("Company").ToString
            '        .Email = dr.Item("Email").ToString
            '        .PersonID = CLng(dr.Item("LinkID"))

            '        'Dim sEncryptedPWD As String = dr.Item("PWD").ToString
            '        'Dim sDecryptedPWD As String = ""
            '        'Dim oSecurity As Aptify.Framework.BusinessLogic.Security.AptifySecurityKey
            '        'oSecurity = New Aptify.Framework.BusinessLogic.Security.AptifySecurityKey(oApp.UserCredentials)
            '        'Decrypt Password
            '        'sDecryptedPWD = oSecurity.DecryptData("E Business Login Key", sEncryptedPWD)

            '        '.Password = sDecryptedPWD

            '        'NK 12/19/06 Parameterize query for added security
            '        'sSQL = "SELECT * FROM " & oApp.GetEntityBaseDatabase("Persons") & _
            '        '       "..vwPersons WHERE ID=@PersonID"
            '        'pUserID = DataAction.GetDataParameter("@PersonID", SqlDbType.NVarChar, dr.Item("LinkID").ToString)
            '        'colParams(0) = pUserID
            '        'dt = DataAction.GetDataTableParametrized(sSQL, CommandType.Text, colParams)

            '        If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then

            '            dr = dt.Rows(2) ' person resultset

            '            .PersonDataRow = dr
            '            If IsNumeric(dr.Item("CompanyID")) Then
            '                .CompanyID = CLng(dr.Item("CompanyID"))
            '                .CompanyDataRow = GetCompanyDataRow(.CompanyID, DataAction, oApp)
            '            Else
            '                .CompanyID = -1
            '                .CompanyDataRow = Nothing
            '            End If
            '            ''RashmiP, Issue 11258
            '            '.PersonPhoto = CType(dr.Item("Photo"), Byte())

            '            For i = 0 To dr.ItemArray.Length - 1
            '                If Not IsDBNull(dr(i)) Then
            '                    If dt.Columns(i).DataType.Name = "Byte[]" Then
            '                        .SetValue(dt.Columns(i).ColumnName, DirectCast(dr(i), Byte()))
            '                    Else
            '                        .SetAddValue(dt.Columns(i).ColumnName, Trim(CStr(dr(i))))
            '                    End If
            '                End If
            '            Next

            '            Dim lPersonCurrencyTypeID As Long = -1

            '            If IsNumeric(dr.Item("PreferredCurrencyTypeID")) Then
            '                lPersonCurrencyTypeID = CLng(dr.Item("PreferredCurrencyTypeID"))
            '            End If

            '            .PreferredCurrencyTypeID = Me.DeterminePreferredCurrency(oApp, DataAction, .PersonID, .CompanyID, lPersonCurrencyTypeID)
            '        End If
            '    End With
            'End If

        End Sub
        ''' <summary>
        ''' The User's Preferred Currency Type is based on either the Preferred Currency Type of the Company linked to the Person.
        ''' If the Company does not have a Preferred Currency or the Person does not have a linked Company, the Person's Preferred
        ''' Currency Type will be used.  If the Person does not have a Preferred Currency Type, the system's Functional Currency will
        ''' be used.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Overridable Function DeterminePreferredCurrency(ByVal AptifyApp As Aptify.Framework.Application.AptifyApplication, _
                                                                    ByVal DataAction As DataAction, _
                                                                    ByVal PersonID As Long, _
                                                                    ByVal CompanyID As Long, _
                                                                    ByVal PersonPreferredCurrencyTypeID As Long) As Long


            Dim lPreferredCurrencyTypeID As Long = -1

            If CompanyID > 0 Then
                'Company has been specified, get the preferred currency from the Company
                Dim params(1) As IDataParameter

                With DataAction
                    params(0) = .GetDataParameter("@CompanyID", SqlDbType.BigInt, CompanyID)
                    params(1) = .GetDataParameter("@PreferredCurrencyTypeID", SqlDbType.Int)
                    params(1).Direction = ParameterDirection.Output


                    .ExecuteNonQueryParametrized(AptifyApp.GetEntityBaseDatabase("Companies") & "..spGetCompanyPreferredCurrencyOUT", CommandType.StoredProcedure, params)

                    If IsNumeric(params(1).Value) Then
                        lPreferredCurrencyTypeID = CInt(params(1).Value)
                    End If
                End With
            End If

            If lPreferredCurrencyTypeID <= 0 AndAlso PersonPreferredCurrencyTypeID > 0 Then
                'Either the company wasn't specified or the Company does not have a preferred Currency defined.
                'Use the Person's preferred currency
                lPreferredCurrencyTypeID = PersonPreferredCurrencyTypeID
            End If

            If lPreferredCurrencyTypeID <= 0 Then
                'Still no value found for the preferred Currency Type, use the System's Functional Currency
                lPreferredCurrencyTypeID = CurrencyTypeCache.Instance(AptifyApp).FunctionalCurrencyType.ID
            End If

            Return lPreferredCurrencyTypeID
        End Function

        ''' <summary>
        ''' The User's Preferred Currency Type is based on either the Preferred Currency Type of the Company linked to the Person.
        ''' If the Company has a preferred currency or if the person is already linked to the company, the company's preferred currenty type id will be used
        ''' If the Company does not have a Preferred Currency or the Person does not have a linked Company, the Person's Preferred
        ''' Currency Type will be used.  If the Person does not have a Preferred Currency Type, the system's Functional Currency will
        ''' be used.
        ''' </summary>    ''' 
        ''' <param name="PersonOrCompPreferredCurrencyTypeID">contains the company's preferred currency type id if the person is linked to company and company has a preferred currency type 
        ''' or else person's preferred currency type id is included</param>
        ''' <remarks></remarks>
        Protected Overridable Function DeterminePreferredCurrency(ByVal AptifyApp As Aptify.Framework.Application.AptifyApplication, _
                                                                    ByVal DataAction As DataAction, _
                                                                    ByVal PersonID As Long, _
                                                                    ByVal CompanyID As Long, _
                                                                    ByVal PersonOrCompPreferredCurrencyTypeID As Long, ByVal CompPreferredCurrencyTypeIdExist As Boolean) As Long


            Dim lPreferredCurrencyTypeID As Long = -1

            If (CompPreferredCurrencyTypeIdExist) Then
                lPreferredCurrencyTypeID = PersonOrCompPreferredCurrencyTypeID 'company preferred currenty type id already exists  
            Else
                lPreferredCurrencyTypeID = Me.DeterminePreferredCurrency(AptifyApp, DataAction, PersonID, CompanyID, PersonOrCompPreferredCurrencyTypeID) 'company preferred currenty type id does not exist
            End If

            Return lPreferredCurrencyTypeID

        End Function
        Public Overridable Function Login(ByVal UserID As String, _
                                      ByVal Password As String, _
                                      ByVal UserInfo As System.Security.Principal.IPrincipal, _
                                      Optional ByVal MaxLoginTries As Integer = 0) _
                                      As Boolean Implements ILogin.Login
            Return InternalLogin(UserID, Password, MaxLoginTries, Nothing, Nothing, UserInfo)
        End Function

        'Neha Changes for hotfix issue 10935, skipped the code while indexing process when page is in index mode.
        Public Overridable Function Logout() As Boolean Implements ILogin.Logout
            ' Logout
            Try
                If Not m_oUser Is Nothing Then
                    m_oUser.Clear()
                    If Page.Items("IsInIndexMode") Is Nothing Then
                        m_oUser.SaveValuesToSessionObject(Page.Session)
                    End If
                End If
                Return True
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Function
        ''End of Addition by Suvarna 17790
    End Class
End Namespace
