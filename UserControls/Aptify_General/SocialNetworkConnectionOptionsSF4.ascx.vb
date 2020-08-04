'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Imports Aptify.Framework.Web.eBusiness.SocialNetworkIntegration
Imports System.Xml
Imports Aptify.Framework.Integration
Imports Aptify.Framework.BusinessLogic
Imports Aptify.Framework.Application
Imports Aptify.Framework.BusinessLogic.ProcessPipeline
Imports Aptify.Framework.DataServices

Namespace Aptify.Framework.Web.eBusiness
    ''' <summary>
    ''' Aptify e-Business SocialNetworkLandingControl ASP.NET User Control
    ''' </summary>
    ''' <remarks></remarks>
    Partial Class SocialNetworkConnectionOptions
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced
        Protected Const ATTRIBUTE_NEWUSER_PAGE_URL As String = "NewUserPage"
        Protected Const ATTRIBUTE_HOME_PAGE_URL As String = "ExistingUserHomePage"
        Protected Const ATTRIBUTE_CONTACTUS_URL As String = "ContactUs"
        Protected Const ATTRIBUTE_CONFIRMATION_HEADER As String = "ConfirmationHeader"
        Protected Const ATTRIBUTE_EXISTING_USER_OPTION As String = "ExistingUserOption"
        Protected Const ATTRIBUTE_NEW_USER_OPTION As String = "NewUserOption"
        Protected Const ATTRIBUTE_USER_DISABLED_ERROR As String = "UserDisabledError" 'SKB Issue#10654: display message indicating disabled account
        Protected Const ATTRIBUTE_SOCIAL_NETWORK_TNCTEXT As String = "SocialNetworkSynchTnCText"
        Protected Const ATTRIBUTE_SOCIAL_NETWORK_TNCPAGE As String = "SocialNetworkSynchTnCPage"
        Protected Const ATTRIBUTE_SOCIAL_NETWORK_USEPHOTOTEXT As String = "SocialNetworkUsePhotoText"
        Protected Const ATTRIBUTE_SOCIAL_NETWORK_IAGREESYNCHTEXT As String = "SocialNetworkIAgreeSynchText"
        Protected Const ATTRIBUTE_SESSIONCOOKIETIMEOUT As String = "SessionCookieTimeout"


        Private m_iSessionCookieTimeout As Integer
        Protected m_oSocialNetwork As SocialNetworkIntegrationBase
        Public Event UserLoggedIn()
        Public Overridable ReadOnly Property SessionCookieTimeout() As String
            Get
                If Not ViewState.Item(ATTRIBUTE_SESSIONCOOKIETIMEOUT) Is Nothing Then
                    Return CStr(ViewState.Item(ATTRIBUTE_SESSIONCOOKIETIMEOUT))
                Else
                    Dim value As String = Me.GetGlobalAttributeValue(ATTRIBUTE_SESSIONCOOKIETIMEOUT)
                    If Not String.IsNullOrEmpty(value) Then
                        ViewState.Item(ATTRIBUTE_SESSIONCOOKIETIMEOUT) = value
                        Return value
                    Else
                        Return ""
                    End If
                End If
            End Get

        End Property


        Protected Sub btnContinue_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnContinue.Click
            'Anil B for  Issue 13882 on 18-03-2013
            'Set Remember option for login
            If Request.Browser.Cookies Then
                'Check if the cookie with name LOGIN exist on user's machine
                'Create a cookie with expiry of 30 days
                Response.Cookies("LOGIN").Expires = DateTime.Now.AddDays(30)
                'Write username to the cookie
                Response.Cookies("LOGIN").Item("RememberMe") = chkRememberMe.Checked
            End If

          

            
        ' Login to the Aptify Enterprise Web Architecture
        ' Use the Aptify EWA .NET Login Component to do this

        'Anil B for issue Issue 13316 on 16-03-2013 
        'Set Authorizatio token and authorization verifier from querystring if available
        Dim sauth_token As String = Request.QueryString("oauth_token")
        Dim sauth_verifier As String = Request.QueryString("oauth_verifier")
            Session.Add("sauth_verifier", sauth_verifier)
            Session.Add("oauth_token", sauth_token)
            If rdoExistingUser.Checked Then
        Dim bLoggedIn As Boolean = False
                Try
                    With WebUserLogin1
                        If .Login(txtUserID.Text, txtPassword.Text, Page.User) Then
                            .User.SaveValuesToSessionObject(Page.Session)
                            bLoggedIn = True
        'SKB SSO for Sitefinity
                            Sitefinity4xSSO1.Sitefinity40SSO(txtUserID.Text, txtPassword.Text)
                        Else
                            tblLogin.Visible = True
                            If .Disabled Then
                                lblError.Text = "Account has been disabled, please contact the administrator"
                            Else
                                lblError.Text = "Error logging in"
                            End If
                            chkRememberMe.Checked = False
        'Check iIf the cookies with name LOGIN exist on user's machine
                            If (Request.Cookies("LOGIN") IsNot Nothing) Then
        'Expire the cookie
                                Response.Cookies("LOGIN").Expires = DateTime.Now.AddDays(-SessionCookieTimeout)
                            End If
                        End If
                    End With
                Catch ex As Exception
                    Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                End Try

                If bLoggedIn Then
        'Navin Prasad Issue # 12545
                    Session("UserLoggedIn") = True
        'Anil Issue 10258
        'Session("LoadSocialNetworkPhoto") = True
                    OnUserLoggedIn()
                End If
            Else
                OnNewUserLoggedIn()
            End If
        End Sub
        Public ReadOnly Property SocialNetworkObject() As SocialNetworkIntegrationBase
            Get
                If m_oSocialNetwork Is Nothing Then
                    If Session("SocialNetwork") IsNot Nothing Then
                        m_oSocialNetwork = DirectCast(Session("SocialNetwork"), SocialNetworkIntegrationBase)
                    End If
                End If
                Return m_oSocialNetwork
            End Get
        End Property
        'Navin Prasad Issue 12341
        Private Function SyncSocialNetworkData() As Boolean
            If SocialNetworkObject IsNot Nothing AndAlso SocialNetworkObject.UserProfile.SynchronizeProfile Then
                Dim sSQL As String = "SELECT ID FROM " & Database & "..vwProcessFlows WHERE Name='Social Network User Profile Synchronization'"
                Dim oProcessFlowID As Object = DataAction.ExecuteScalar(sSQL, IAptifyDataAction.DSLCacheSetting.UseCache)
                If oProcessFlowID IsNot Nothing AndAlso IsNumeric(oProcessFlowID) Then
                    Dim lProcessFlowID As Long = CLng(oProcessFlowID)
                    If lProcessFlowID > 0 Then
                        Dim oContext As New AptifyContext
                        Dim result As ProcessFlowResult
                        oContext.Properties.AddProperty("PersonExternalAccountID", SocialNetworkObject.UserProfile.PersonExternalAccountID)
                        result = ProcessFlowEngine.ExecuteProcessFlow(Me.AptifyApplication, lProcessFlowID, oContext)
                        If Not result.IsSuccess Then
                            ExceptionManagement.ExceptionManager.Publish(New Exception("Unable to synchronize social network profile for user '" & WebUserLogin1.User.WebUserStringID & "'"))
                            Return False
                        Else
                            Return True
                        End If
                    End If
                End If
            End If
            Return False
        End Function

        Protected Overridable Sub OnUserLoggedIn()

            Dim sRedirectLocation As String = ""
            Dim sErrorString As String = ""
            Try
                If SocialNetworkObject IsNot Nothing Then
                    SocialNetworkObject.UserProfile.EBusinessUser = WebUserLogin1.User
                    SocialNetworkObject.UserProfile.SynchronizeProfile = chkSynchronizeProfile.Checked
                    SocialNetworkObject.UserProfile.UseSocialMediaPhoto = chkUseSocialMediaPhoto.Checked
                    If Not SocialNetworkObject.UserProfile.SynchronizePersonExternalAccount(sErrorString) Then
                        If sErrorString.Trim.Length > 0 Then
                            lblError.Text = sErrorString
                            Exit Sub
                        End If
                    Else
                        'Navin Prasad  Issue 12341
                        SyncSocialNetworkData()
                    End If
                End If
                NavigateExistingUser()

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try

        End Sub
        Protected Overridable Sub OnNewUserLoggedIn()

            Dim sRedirectLocation As String = ""
            Try
                'Navin Prasad Issue 10258
                If SocialNetworkObject IsNot Nothing Then
                    SocialNetworkObject.UserProfile.UseSocialMediaPhoto = chkUseSocialMediaPhoto.Checked
                    'Navin Prasad Issue 12835
                    ' SocialNetworkObject.UserProfile.SynchronizeProfile = True
                    'Navin Prasad Issue 12341
                    SocialNetworkObject.UserProfile.SynchronizeProfile = chkSynchronizeProfile.Checked
                End If
                If Len(SocialNetworkNewUserPage) <> 0 Then
                    Dim sTemp As String
                    sTemp = CStr(SocialNetworkNewUserPage)
                    sRedirectLocation = sTemp
                    'Response.Redirect(sTemp)
                Else
                    ' refresh current page
                    sRedirectLocation = Request.RawUrl

                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try

            Response.Redirect(sRedirectLocation)

        End Sub
        Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
          'Anil B for  Issue 13882 on 18-03-2013
            'Set Remember option for login
            If Request.Browser.Cookies Then
                'Check if the cookie with name LOGIN exist on user's machine
                'Create a cookie with expiry of 30 days
                Response.Cookies("LOGIN").Expires = DateTime.Now.AddDays(30)
                'Write username to the cookie
                Response.Cookies("LOGIN").Item("RememberMe") = chkRememberMe.Checked
            End If
            Session("SocialNetwork") = Nothing
            Response.Redirect(Request.ApplicationPath)
        End Sub
        Protected Overrides Sub SetProperties()
            Try

                If String.IsNullOrEmpty(SocialNetworkNewUserPage) Then
                    SocialNetworkNewUserPage = Me.GetLinkValueFromXML(ATTRIBUTE_NEWUSER_PAGE_URL)
                End If
                If String.IsNullOrEmpty(SocialNetworkExistingUserHomePage) Then
                    SocialNetworkExistingUserHomePage = Me.GetLinkValueFromXML(ATTRIBUTE_HOME_PAGE_URL)
                End If
                If String.IsNullOrEmpty(ContactUs) Then
                    ContactUs = Me.GetLinkValueFromXML(ATTRIBUTE_CONTACTUS_URL)
                End If
                If String.IsNullOrEmpty(ConfirmationHeader) Then
                    ConfirmationHeader = Me.GetPropertyValueFromXML(ATTRIBUTE_CONFIRMATION_HEADER)
                End If
                If String.IsNullOrEmpty(ExistingUserOption) Then
                    ExistingUserOption = Me.GetPropertyValueFromXML(ATTRIBUTE_EXISTING_USER_OPTION)
                End If
                If String.IsNullOrEmpty(NewUserOption) Then
                    NewUserOption = Me.GetPropertyValueFromXML(ATTRIBUTE_NEW_USER_OPTION)
                End If

                If Not String.IsNullOrEmpty(NewUserOption) Then
                    lblrdoNewUser.Text = NewUserOption
                End If

                If Not String.IsNullOrEmpty(ExistingUserOption) Then
                    LblrdoExistingUser.Text = ExistingUserOption
                End If
                If Not String.IsNullOrEmpty(ContactUs) Then
                    hypContactUS.NavigateUrl = ContactUs
                End If
                If Not String.IsNullOrEmpty(ConfirmationHeader) Then
                    lblLogin.InnerText = ConfirmationHeader
                End If

                If Not String.IsNullOrEmpty(SocialNetworkSynchTnCText) Then
                    hypSocialNetworkSynchText.Text = SocialNetworkSynchTnCText
                End If
                If Not String.IsNullOrEmpty(SocialNetworkSynchTnCPage) Then
                    hypSocialNetworkSynchText.NavigateUrl = Me.FixLinkForVirtualPath(SocialNetworkSynchTnCPage)
                End If

                If Not String.IsNullOrEmpty(SocialNetworkIAgreeSynchText) Then
                    lblchkSynchronizeProfile.Text = SocialNetworkIAgreeSynchText
                End If
                If Not String.IsNullOrEmpty(SocialNetworkUsePhotoText) Then
                    lblchkUseSocialMediaPhoto.Text = SocialNetworkUsePhotoText
                End If
            Catch ex As Exception

            End Try
        End Sub
        Public Overridable Property SocialNetworkNewUserPage() As String
            Get
                If Not ViewState.Item(ATTRIBUTE_NEWUSER_PAGE_URL) Is Nothing Then
                    Return CStr(ViewState.Item(ATTRIBUTE_NEWUSER_PAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState.Item(ATTRIBUTE_NEWUSER_PAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        Public Overridable Property SocialNetworkExistingUserHomePage() As String
            Get
                If Not ViewState.Item(ATTRIBUTE_HOME_PAGE_URL) Is Nothing Then
                    Return CStr(ViewState.Item(ATTRIBUTE_HOME_PAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState.Item(ATTRIBUTE_HOME_PAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        Public Overridable Property ContactUs() As String

            Get
                If ViewState.Item(ATTRIBUTE_CONTACTUS_URL) IsNot Nothing Then
                    Return ViewState.Item(ATTRIBUTE_CONTACTUS_URL).ToString()
                Else
                    Return ""
                End If

            End Get
            Set(ByVal value As String)
                ViewState.Item(ATTRIBUTE_CONTACTUS_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        Public Property ConfirmationHeader() As String
            Get
                If ViewState.Item(ATTRIBUTE_CONFIRMATION_HEADER) IsNot Nothing Then
                    Return ViewState.Item(ATTRIBUTE_CONFIRMATION_HEADER).ToString()
                Else
                    Return ""
                End If

            End Get
            Set(ByVal value As String)
                ViewState.Item(ATTRIBUTE_CONFIRMATION_HEADER) = value
            End Set

        End Property

        Public Property ExistingUserOption() As String
            Get
                If ViewState.Item(ATTRIBUTE_EXISTING_USER_OPTION) IsNot Nothing Then
                    Return ViewState.Item(ATTRIBUTE_EXISTING_USER_OPTION).ToString()
                Else
                    Return ""
                End If

            End Get
            Set(ByVal value As String)
                ViewState.Item(ATTRIBUTE_EXISTING_USER_OPTION) = value
            End Set

        End Property
        Public Property NewUserOption() As String
            Get
                If ViewState.Item(ATTRIBUTE_NEW_USER_OPTION) IsNot Nothing Then
                    Return ViewState.Item(ATTRIBUTE_NEW_USER_OPTION).ToString()
                Else
                    Return ""
                End If

            End Get
            Set(ByVal value As String)
                ViewState.Item(ATTRIBUTE_NEW_USER_OPTION) = value
            End Set
        End Property
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            SetProperties()
            rdoExistingUser.Attributes.Add("OnClick", "EnableDisableOptionControls(this);")
            rdoNewUser.Attributes.Add("OnClick", "EnableDisableOptionControls(this);")
            chkSynchronizeProfile.Attributes.Add("OnClick", "EnableDisablePhotoOption(this);")

            'Anil B for  Issue 13882 on 18-03-2013
            'Set Remember option from home page
            If Not IsPostBack Then
                If Request.Browser.Cookies Then
                    'Check if the cookies with name LOGIN exist on user's machine
                    If Request.Cookies("LOGIN") IsNot Nothing AndAlso Request.Cookies("LOGIN").Item("RememberMe") IsNot Nothing Then
                        'Pass the user name and password to the VerifyLogin method
                        chkRememberMe.Checked = CBool(Request.Cookies("LOGIN").Item("RememberMe"))
                    End If
                End If
            End If

            If IsPostBack Then
                If Request.Browser.Cookies Then
                    'Check if the cookie with name LOGIN exist on user's machine
                    'Create a cookie with expiry of 30 days
                    Response.Cookies("LOGIN").Expires = DateTime.Now.AddDays(30)
                    'Write username to the cookie
                    Response.Cookies("LOGIN").Item("RememberMe") = chkRememberMe.Checked
                End If
            End If
        End Sub
        ''' <summary>
        ''' Navigates Existing User to either previous requested page or to page configured on the user control navigation attribute
        ''' </summary>
        ''' <remarks></remarks>
        Protected Overridable Sub NavigateExistingUser()

            Dim sRedirectLocation As String = ""

            Try
                RaiseEvent UserLoggedIn()
                'Suraj Issue 15370,7/31/13 revert back changes application variale to session variable
                If Len(Session("ReturnToPage")) <> 0 Then
                    Dim sTemp As String
                    sTemp = CStr(Session("ReturnToPage"))
                    Session("ReturnToPage") = "" ' used only once
                    sRedirectLocation = sTemp
                    'Response.Redirect(sTemp)
                ElseIf SocialNetworkExistingUserHomePage <> String.Empty Then
                    sRedirectLocation = SocialNetworkExistingUserHomePage
                Else
                    ' refresh current page
                    sRedirectLocation = Request.RawUrl
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try

            Response.Redirect(sRedirectLocation)
        End Sub
        Protected Overridable ReadOnly Property SocialNetworkSynchTnCText() As String
            Get
                If Not ViewState.Item(ATTRIBUTE_SOCIAL_NETWORK_TNCTEXT) Is Nothing Then
                    Return CStr(ViewState.Item(ATTRIBUTE_SOCIAL_NETWORK_TNCTEXT))
                Else
                    Dim value As String = Me.GetGlobalAttributeValue(ATTRIBUTE_SOCIAL_NETWORK_TNCTEXT)
                    If Not String.IsNullOrEmpty(value) Then
                        ViewState.Item(ATTRIBUTE_SOCIAL_NETWORK_TNCTEXT) = value
                        Return value
                    Else
                        Return String.Empty
                    End If
                End If
            End Get

        End Property

        Protected Overridable ReadOnly Property SocialNetworkSynchTnCPage() As String
            Get
                If Not ViewState.Item(ATTRIBUTE_SOCIAL_NETWORK_TNCPAGE) Is Nothing Then
                    Return CStr(ViewState.Item(ATTRIBUTE_SOCIAL_NETWORK_TNCPAGE))
                Else
                    Dim value As String = Me.GetGlobalAttributeValue(ATTRIBUTE_SOCIAL_NETWORK_TNCPAGE)
                    If Not String.IsNullOrEmpty(value) Then
                        ViewState.Item(ATTRIBUTE_SOCIAL_NETWORK_TNCPAGE) = value
                        Return value
                    Else
                        Return String.Empty
                    End If
                End If
            End Get

        End Property
        Protected Overridable ReadOnly Property SocialNetworkUsePhotoText() As String
            Get
                If Not ViewState.Item(ATTRIBUTE_SOCIAL_NETWORK_USEPHOTOTEXT) Is Nothing Then
                    Return CStr(ViewState.Item(ATTRIBUTE_SOCIAL_NETWORK_USEPHOTOTEXT))
                Else
                    Dim value As String = Me.GetGlobalAttributeValue(ATTRIBUTE_SOCIAL_NETWORK_USEPHOTOTEXT)
                    If Not String.IsNullOrEmpty(value) Then
                        ViewState.Item(ATTRIBUTE_SOCIAL_NETWORK_USEPHOTOTEXT) = value
                        Return value
                    Else
                        Return String.Empty
                    End If
                End If
            End Get

        End Property
        Protected Overridable ReadOnly Property SocialNetworkIAgreeSynchText() As String
            Get
                If Not ViewState.Item(ATTRIBUTE_SOCIAL_NETWORK_IAGREESYNCHTEXT) Is Nothing Then
                    Return CStr(ViewState.Item(ATTRIBUTE_SOCIAL_NETWORK_IAGREESYNCHTEXT))
                Else
                    Dim value As String = Me.GetGlobalAttributeValue(ATTRIBUTE_SOCIAL_NETWORK_IAGREESYNCHTEXT)
                    If Not String.IsNullOrEmpty(value) Then
                        ViewState.Item(ATTRIBUTE_SOCIAL_NETWORK_IAGREESYNCHTEXT) = value
                        Return value
                    Else
                        Return String.Empty
                    End If
                End If
            End Get

        End Property

        Protected Sub chkRememberMe_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkRememberMe.CheckedChanged
             'Anil B for  Issue 13882 on 18-03-2013
            'Set Remember option for login
            If Request.Browser.Cookies Then
                'Check if the cookie with name LOGIN exist on user's machine
                'If (Request.Cookies("LOGIN") Is Nothing) Then
                'Create a cookie with expiry of 30 days
                Response.Cookies("LOGIN").Expires = DateTime.Now.AddDays(30)
                'Write username to the cookie
                Response.Cookies("LOGIN").Item("RememberMe") = chkRememberMe.Checked
            End If
        End Sub
    End Class
End Namespace

                                                                                       