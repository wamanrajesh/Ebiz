'Aptify e-Business 5.5.1, July 2013
Option Strict On
Option Explicit On

Imports Aptify.Framework.Web.eBusiness.SocialNetworkIntegration
Imports System.Web.ClientServices
Imports Aptify.Framework.ExceptionManagement
Imports Aptify.Framework.Integration
Imports Aptify.Framework.BusinessLogic

Namespace Aptify.Framework.Web.eBusiness
    Partial Class SocialNetworkingIntegrationControl
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced
        Protected Const ATTRIBUTE_SOCIALNETWORK_IMAGE_URL As String = "SocialNetworkImage"
        Protected Const ATTRIBUTE_SYSTEM_NAME As String = "SocialNetworkSystemName"
        Protected Const ATTRIBUTE_NEWUSER_PAGE_URL As String = "SocialNetworkNewUserPage"
        Protected Const ATTRIBUTE_HOME_PAGE_URL As String = "SocialNetworkExistingUserHomePage"
        Protected Const ATTRIBUTE_USER_DISABLED_ERROR As String = "UserDisabledError" 'SKB Issue#10654: display message indicating disabled account
        Protected Const ATTRIBUTE_SOCIALNETWORK_CONNOPP_PAGE As String = "SocialNetworkConnectionOptionsPage"
        Protected Const ATTRIBUTE_PROPERTY_NOT_CONFIGUREDMSG As String = " is not configured. Please contact your system administrator for more details."
        Protected m_oSocialNetwork As SocialNetworkIntegrationBase
        Protected m_sErrorString As String = ""
#Region "Properties"
        Public Overridable Property SocialNetworkImage() As String
            Get
                If Not Session(ATTRIBUTE_SOCIALNETWORK_IMAGE_URL) Is Nothing Then
                    Return CStr(Session(ATTRIBUTE_SOCIALNETWORK_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                Session(ATTRIBUTE_SOCIALNETWORK_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        Public Overridable Property SocialNetworkNewUserPage() As String
            Get
                If Not Session(ATTRIBUTE_NEWUSER_PAGE_URL) Is Nothing Then
                    Return CStr(Session(ATTRIBUTE_NEWUSER_PAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                Session(ATTRIBUTE_NEWUSER_PAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        Public Overridable Property SocialNetworkExistingUserHomePage() As String
            Get
                If Not Session(ATTRIBUTE_HOME_PAGE_URL) Is Nothing Then
                    Return CStr(Session(ATTRIBUTE_HOME_PAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                Session(ATTRIBUTE_HOME_PAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        Public Overridable Property SocialNetworkConnectionOptionsPage() As String
            Get
                If Not Session(ATTRIBUTE_SOCIALNETWORK_CONNOPP_PAGE) Is Nothing Then
                    Return CStr(Session(ATTRIBUTE_SOCIALNETWORK_CONNOPP_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                Session(ATTRIBUTE_SOCIALNETWORK_CONNOPP_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' Rashmi P
        ''' System Name For Example LinkedIn
        ''' </summary>

        Public Overridable ReadOnly Property SocialNetworkSystemName() As String
            Get
                If Not Session.Item(ATTRIBUTE_SYSTEM_NAME) Is Nothing Then
                    Return CStr(Session.Item(ATTRIBUTE_SYSTEM_NAME))
                Else
                    Dim value As String = Me.GetGlobalAttributeValue(ATTRIBUTE_SYSTEM_NAME)
                    If Not String.IsNullOrEmpty(value) Then
                        Session.Item(ATTRIBUTE_SYSTEM_NAME) = value
                        Return value
                    Else
                        Return String.Empty
                    End If
                End If
            End Get

        End Property
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
#End Region

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            SetProperties()
            ' Call SignOn method if control initialized and has SignOn state true set via Config Method
            If Session("RedirectSignOn") IsNot Nothing AndAlso CBool(Session("RedirectSignOn")) Then
                Session("RedirectSignOn") = False
                Dim sErrorString As String = ""
                If Not SignOn(sErrorString) Then
                    lblError.Text = sErrorString
                    lblError.Visible = True
                    lnkLinkedIn.Visible = False
                    'Clear the session object
                    'Session("SocialNetwork") = Nothing
                Else
                    lblError.Visible = False
                    lnkLinkedIn.Visible = True
                End If
            End If
        End Sub
        Protected Overrides Sub SetProperties()
            Try
                If Trim(SocialNetworkSystemName) <> "" Then
                    If String.IsNullOrEmpty(SocialNetworkImage) Then
                        'since value is the 'default' check the XML file for possible custom setting
                        SocialNetworkImage = Me.GetLinkValueFromXML(ATTRIBUTE_SOCIALNETWORK_IMAGE_URL)
                    End If
                    imgSocialNetwork.Visible = True
                    imgSocialNetwork.Src = SocialNetworkImage
                Else
                    imgSocialNetwork.Visible = False
                End If

                If String.IsNullOrEmpty(SocialNetworkNewUserPage) Then
                    SocialNetworkNewUserPage = Me.GetLinkValueFromXML(ATTRIBUTE_NEWUSER_PAGE_URL)
                End If
                If String.IsNullOrEmpty(SocialNetworkExistingUserHomePage) Then
                    SocialNetworkExistingUserHomePage = Me.GetLinkValueFromXML(ATTRIBUTE_HOME_PAGE_URL)
                End If
                If String.IsNullOrEmpty(SocialNetworkConnectionOptionsPage) Then
                    SocialNetworkConnectionOptionsPage = Me.GetLinkValueFromXML(ATTRIBUTE_SOCIALNETWORK_CONNOPP_PAGE)
                End If
            Catch ex As Exception

            End Try
        End Sub

        Protected Sub lnkLinkedIn_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkLinkedIn.ServerClick
            Config()
        End Sub
        Public ReadOnly Property SocialNetworkObject() As SocialNetworkIntegrationBase
            Get
                If m_oSocialNetwork Is Nothing Then
                    If Session("SocialNetwork") IsNot Nothing Then
                        m_oSocialNetwork = DirectCast(Session("SocialNetwork"), SocialNetworkIntegrationBase)
                    End If
                    ' IF not exists then create object and send to autherization again
                    If m_oSocialNetwork Is Nothing Then
                        If SocialNetworkSystemName <> "" Then
                            m_oSocialNetwork = SocialNetwork.SocialNetworkInstance(SocialNetworkSystemName, AptifyApplication, WebUserLogin1.User.UserID, Nothing, False)
                        Else
                            m_sErrorString = "Social Network System is not configured, Please contact system administrator for more details."
                            ExceptionManagement.ExceptionManager.Publish(New Exception(m_sErrorString))
                        End If
                    End If
                End If
                Return m_oSocialNetwork
            End Get
        End Property
        Protected Overridable Function Config() As Boolean

            If SocialNetworkObject IsNot Nothing Then
                'Add to session object
                Session("SocialNetwork") = SocialNetworkObject
                If Not SocialNetworkObject.IsConnected Then
                    'Clear the Token info so that we get the fresh token
                    SocialNetworkObject.AuthParams.AccessToken = Nothing
                    SocialNetworkObject.AuthParams.AccessSecret = Nothing
                    SocialNetworkObject.GetAuthorizationToken()
                    Response.Redirect(SocialNetworkObject.AuthParams.AuthTokenURL, False)
                    Session("RedirectSignOn") = True
                    Return True
                ElseIf SocialNetworkObject.UserProfile IsNot Nothing Then
                    'Response.Write("<script type='text/javascript'>window.open('" & SocialNetworkObject.UserProfile.ProfileUrl & "');</script>")
                    Return True
                End If
            End If
        End Function

        'Navin Prasad Issue#12545 : e-Business: Reduce Size of Session Objects
        Public Overridable Function SignOn(ByRef ErrorString As String) As Boolean
            If SocialNetworkObject IsNot Nothing Then
                'Add to session object
                Session("SocialNetwork") = SocialNetworkObject
                With SocialNetworkObject
                    .SetAuthorizationData(Session, Page.User, Application, Request)
                    If .IsConnected Then
                        If .UserProfile.EBusinessUser.UserID > 0 Then
                            'Authorization successful then do SOSeect
                            'SKB 02/13/2012 Issue 12866
                            Dim sPWD As String = ""
                            .UserProfile.LoadUserObjectFromWebUserID(AptifyApplication, .UserProfile.WebUserID, sPWD)
                            If WebUserLogin1.Login(.UserProfile.EBusinessUser.WebUserStringID, sPWD, Page.User) Then
                                Session("UserLoggedIn") = True
                                If Session("RememberMe") IsNot Nothing Then
                                    If CBool(Session("RememberMe")) = True Then
                                        WebUserLogin1.AddAutoLoginCookie(Page.Response, .UserProfile.EBusinessUser.WebUserStringID, sPWD)
                                    End If
                                End If
                                'Login Successful then syncronize external account to update latest info
                                If .UserProfile.SynchronizePersonExternalAccount(ErrorString) Then
                                    If SocialNetworkExistingUserHomePage = "" Then
                                        ErrorString = "SocialNetworkExistingUserHomePage" & ATTRIBUTE_PROPERTY_NOT_CONFIGUREDMSG
                                        Return False
                                    Else
                                        NavigateExistingUser()
                                        Return True
                                    End If
                                Else
                                    If ErrorString <> "" Then
                                        ExceptionManager.Publish(New Exception(ErrorString))
                                    End If
                                End If
                            Else
                                'SKB Issue#10654: display message indicating disabled account
                                If WebUserLogin1.Disabled Then
                                    'ErrorString = "Your account has been disabled. Please contact the site administrator for assistance."
                                    ErrorString = UserDisabledError
                                    Return False
                                ElseIf SocialNetworkNewUserPage = "" Then
                                    ErrorString = "SocialNetworkNewUserPage" & ATTRIBUTE_PROPERTY_NOT_CONFIGUREDMSG
                                    Return False
                                Else
                                    Response.Redirect(SocialNetworkNewUserPage)
                                    Return True
                                End If
                            End If
                        Else
                            If WebUserLogin1.User.UserID > 0 Then
                                'SKB Issue 13013 02/27/2012
                                .UserProfile.EBusinessUser = CType(WebUserLogin1.User, IUser2)
                                .UserProfile.SynchronizeProfile = True
                                Session("UserLoggedIn") = True
                                Session("SocialNetwork") = SocialNetworkObject
                                'Login Successful then syncronize external account to update latest info
                                If .UserProfile.SynchronizePersonExternalAccount(ErrorString) Then
                                    If SocialNetworkExistingUserHomePage = "" Then
                                        ErrorString = "SocialNetworkExistingUserHomePage" & ATTRIBUTE_PROPERTY_NOT_CONFIGUREDMSG
                                        Return False
                                    Else
                                        NavigateExistingUser()
                                        Return True
                                    End If
                                Else
                                    If ErrorString <> "" Then
                                        ExceptionManager.Publish(New Exception(ErrorString))
                                    End If
                                End If

                            Else
                                If SocialNetworkConnectionOptionsPage = "" Then
                                    ErrorString = "UserConfirmationPage" & ATTRIBUTE_PROPERTY_NOT_CONFIGUREDMSG
                                    Return False
                                Else
                                    Response.Redirect(SocialNetworkConnectionOptionsPage)
                                    Return True
                                End If
                            End If
                        End If
                    Else
                        If SocialNetworkExistingUserHomePage = "" Then
                            ErrorString = "SocialNetworkExistingUserHomePage" & ATTRIBUTE_PROPERTY_NOT_CONFIGUREDMSG
                        Else
                            Response.Redirect(SocialNetworkExistingUserHomePage)
                        End If
                        Return False
                    End If
                End With
            End If
            ErrorString = m_sErrorString
        End Function
        ''' <summary>
        ''' Navigates Existing User to either previous requested page or to page configured on the user control navigation attribute
        ''' </summary>
        ''' <remarks></remarks>
        Protected Overridable Sub NavigateExistingUser()
			'SKB Issue 12372
            Dim sRedirectLocation As String = ""

            Try
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

    End Class

End Namespace

