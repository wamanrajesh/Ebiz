'Aptify e-Business 5.5.1/LMS 5.5.1, June 2014
Option Explicit On
Imports Aptify.Framework.Web.eBusiness.SocialNetworkIntegration
Imports System.Xml
Imports Aptify.Framework.Integration
Imports Aptify.Framework.BusinessLogic

Namespace Aptify.Framework.Web.eBusiness
    ''' <summary>
    ''' Aptify e-Business Login ASP.NET User Control
    ''' </summary>
    ''' <remarks></remarks>
    Partial Class WebLogin
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced

        Protected Const ATTRIBUTE_NEWUSER_PAGE As String = "NewUserPage"
        Protected Const ATTRIBUTE_FORGOTUID_PAGE As String = "ForgotUIDPage"
        Protected Const ATTRIBUTE_MAXLOGINTRIES As String = "maxLoginTries"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "Login"
        Protected Const ATTRIBUTE_HOME_CHANGEPWD As String = "ChangePassword"
        'Suraj Issue 14861, 5/8/13, declare property for get a number of days from the nav file for cookies expire
        Protected Const ATTRIBUTE_NUM_COLUMNS As String = "TimeForExpiry"
        Private m_iTimeForExpiry As Integer
        'Neha, Issue 14408,03/20/13,declare property for new WebUser 
        Private CheckNewWebUser As Boolean

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            'Suraj S Issue 15370, 7/31/13 if the user is not login we are redirecting to the loging page if bymistake any one redirect to user for genric login the here we are  getting the ReturnToPageURL in "URL" QueryString and we storing this URL in to the Session("ReturnToPage")  
            If Request.QueryString("ReturnURL") IsNot Nothing Then
                Session("ReturnToPage") = Aptify.Framework.Web.Common.WebCryptography.Decrypt(Request.QueryString("ReturnURL"))
            End If
            SetProperties()
            Dim CheckSessionValue As Boolean
            If Not IsPostBack Then
                'Anil B for  Issue 13882 on 18-03-2013
                'Set Remember option for login
                If Request.Browser.Cookies Then
                    'Check if the cookies with name LOGIN exist on user's machine
                    If Request.Cookies("LOGIN") IsNot Nothing AndAlso Request.Cookies("LOGIN").Item("RememberMe") IsNot Nothing Then
                        chkAutoLogin.Checked = CBool(Request.Cookies("LOGIN").Item("RememberMe"))
                    End If
                End If
                CleareCatche()
            End If
            If Session("CheckNewUser") IsNot Nothing Then
                CheckSessionValue = Convert.ToBoolean(Session("CheckNewUser"))
                If CheckSessionValue Then
                    Session.Remove("CheckNewUser")
                    Response.Redirect(ChangePassword)
                End If
            End If

            If IsPostBack Then
                If Request.Browser.Cookies Then
                    'Check if the cookie with name LOGIN exist on user's machine
                    'If (Request.Cookies("LOGIN") Is Nothing) Then
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


            If String.IsNullOrEmpty(ChangePassword) Then
                'since value is the 'default' check the XML file for possible custom setting
                ChangePassword = Me.GetLinkValueFromXML(ATTRIBUTE_HOME_CHANGEPWD)

            End If
            'Suraj Issue 14861, if TimeForExpiry value is 0 then take a value from nav file.
            If TimeForExpiry = 0 Then
                'since value is the 'default' check the XML file for possible custom setting
                If Not String.IsNullOrEmpty(Me.GetPropertyValueFromXML(ATTRIBUTE_NUM_COLUMNS)) Then
                    TimeForExpiry = CInt(Me.GetPropertyValueFromXML(ATTRIBUTE_NUM_COLUMNS))
                End If
            End If


        End Sub
        'Suraj Issue 14861, 5/8/13, set and get the value of property 
        Public Overridable Property TimeForExpiry() As Integer
            Get
                Return m_iTimeForExpiry
            End Get
            Set(ByVal value As Integer)
                m_iTimeForExpiry = value
            End Set
        End Property
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
        'Neha, Issue 14408,03/20/13,set property for new webuser firstlogin 
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
            'Check If the cookies with name LOGIN exist on user's machine
            If (Request.Cookies("LOGIN") IsNot Nothing) Then
                'Suraj Issue 14861, 5/8/13,  remove hard coded days from AddDays and add the dynamics days which is set from nav file
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

                            ''Rashmi P Issue 17849: Need to Secure Tin Can Content Access.
                            AddCookieForLMSUser(Page.Request, Page.Response, CStr(.User.PersonID))
                            ' make sure to persist changes to user, since many
                            ' applications will do a Response.Redirect after
                            ' this event is fired
                            .User.SaveValuesToSessionObject(Page.Session)
                            bLoggedIn = True
                            'SKB Issue 12066 call SSO for Sitefinity
                            Sitefinity4xSSO1.Sitefinity40SSO(txtUserID.Text, txtPassword.Text)
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
                                lblError.Text = "Account has been disabled, please contact the administrator"
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
            'Sheetal added condition for issue 19707:eBusiness: Redirect Page to Password Change Page In a Scenario
            If (WebUserLogin1.PasswordResetRequired = False OrElse WebUserLogin1.temporaryUsed = False) Then
                'Neha, Issue 14408,03/20/13, Check user loggedin first time and redirect to changepassword page if firstlogin true
                If bLoggedIn Then
                    OnUserLoggedIn()
                    If Session("UserLoggedIn") = True AndAlso CheckFirstLogin() Then
                        CheckNewUser = True
                        Session("CheckNewUser") = True
                    Else
                        CheckNewUser = False
                    End If
                    Response.Redirect(Request.RawUrl)
                End If
            End If
        End Sub
        'Neha, Issue 14408,03/20/13, check firstlogin for user by tracking session count value
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
        'Nalini Issue 12734
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
                    WebUserLogin1.ClearAutoLoginCookie(Page.Response)
                    'HP Issue#9078: clear and delete session
                    Session.Clear()
                    Session.Abandon()
                    'Suraj Issue 15370,7/31/13 revert back changes application variale to session variable
                    Session.Remove("ReturnToPage")
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
                    Else
                        tblLogin.Visible = False
                        litLoginLabel.Visible = False
                        tblWelcome.Visible = True
                        lblWelcome.Text = "Welcome, " & _
                                          .User.FirstName & " " & _
                                          .User.LastName
                        .User.SaveValuesToSessionObject(Page.Session)
                    End If
                End With
                'lblLogin.Visible = Me.ShowTitle
                'If lblLogin.Visible Then
                'lblLogin.InnerText = Me.TitleString
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
                'Sapna - Issue #12582
                Session("UserLoggedIn") = True
                'Sheetal Issue 20175, 10/09/2014 :Redirect user to change password when first time login
                If CheckFirstLogin() Then
                    Session("ReturnToPage") = ""
                End If
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
            'Sapna - Issue #12582
            If Not Session Is Nothing Then
                Session("UserLoggedIn") = False
            End If
            Response.Redirect(Request.RawUrl)
        End Sub

        Protected Sub chkAutoLogin_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAutoLogin.CheckedChanged
            'Anil B for  Issue 13882 on 18-03-2013
            'Set Remember option for login
            'Modified by Dipali Story No:13882:-e-Business: Support Remember Me Option When Logging in Using LinkedIn
            ' lbl.Text = CStr(chkAutoLogin.Checked)
            'Check if the browser support cookies
            If Request.Browser.Cookies Then
                'Check if the cookie with name LOGIN exist on user's machine
                'If (Request.Cookies("LOGIN") Is Nothing) Then
                'Create a cookie with expiry of 30 days
                Response.Cookies("LOGIN").Expires = DateTime.Now.AddDays(30)
                'Write username to the cookie
                Response.Cookies("LOGIN").Item("RememberMe") = chkAutoLogin.Checked
            End If
            'End If
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

    End Class
End Namespace