'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On
Imports Aptify.Framework.Web.eBusiness.SocialNetworkIntegration

'“This version of SyncProfile is for use with Sitefinity”

Namespace Aptify.Framework.Web.eBusiness.CustomerService
    Partial Class SynchProfile
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced
        Protected Const ATTRIBUTE_SOCIALNETWORK_IMAGE_URL As String = "SocialNetworkImage"
        'Protected m_oSocialNetwork As SocialNetworkIntegrationBase
        Protected ATTRIBUTE_SOCIAL_NETWORK_TNCTEXT As String = "SocialNetworkSynchTnCText"
        Protected ATTRIBUTE_SOCIAL_NETWORK_TNCPAGE As String = "SocialNetworkSynchTnCPage"
        Protected Const ATTRIBUTE_SOCIAL_NETWORK_USEPHOTOTEXT As String = "SocialNetworkUsePhotoText"
        Protected m_sSynchedYourProfileText As String
        Protected m_sSyncedText As String 
#Region "Properties"
        Protected Overridable ReadOnly Property SocialNetworkSynchTnCText() As String
            Get
                If Not Session.Item(ATTRIBUTE_SOCIAL_NETWORK_TNCTEXT) Is Nothing Then
                    Return CStr(Session.Item(ATTRIBUTE_SOCIAL_NETWORK_TNCTEXT))
                Else
                    Dim value As String = Me.GetGlobalAttributeValue(ATTRIBUTE_SOCIAL_NETWORK_TNCTEXT)
                    If Not String.IsNullOrEmpty(value) Then
                        Session.Item(ATTRIBUTE_SOCIAL_NETWORK_TNCTEXT) = value
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

        Protected Overridable ReadOnly Property SocialNetworkSynchTnCPage() As String
            Get
                If Not Session.Item(ATTRIBUTE_SOCIAL_NETWORK_TNCPAGE) Is Nothing Then
                    Return CStr(Session.Item(ATTRIBUTE_SOCIAL_NETWORK_TNCPAGE))
                Else
                    Dim value As String = Me.GetGlobalAttributeValue(ATTRIBUTE_SOCIAL_NETWORK_TNCPAGE)
                    If Not String.IsNullOrEmpty(value) Then
                        Session.Item(ATTRIBUTE_SOCIAL_NETWORK_TNCPAGE) = value
                        Return value
                    Else
                        Return String.Empty
                    End If
                End If
            End Get

        End Property
        Public ReadOnly Property SocialNetworkObject() As SocialNetworkIntegrationBase
            Get
                If Session("SocialNetwork") IsNot Nothing Then
                    Return DirectCast(Session("SocialNetwork"), SocialNetworkIntegrationBase)
                Else
                    If SocialNetworkingIntegrationControlSF4.SocialNetworkSystemName <> "" AndAlso WebUserLogin1.User.UserID > 0 Then
                        Session("SocialNetwork") = SocialNetwork.SocialNetworkInstance(SocialNetworkingIntegrationControlSF4.SocialNetworkSystemName, AptifyApplication, WebUserLogin1.User.UserID, Nothing, False)
                    End If
                End If
                Return DirectCast(Session("SocialNetwork"), SocialNetworkIntegrationBase)
            End Get
        End Property

        Public ReadOnly Property SyncStatus() As Boolean
            Get
                Return CBool(IIf(lnkDeactivate.Text = "Activate", True, False))
            End Get
        End Property
#End Region

        Protected Overridable Sub SetSocialNetworkControls()
            Dim bShowSocialNetworkControl As Boolean = False
            hypSocialNetworkSynchText.Visible = True
            If SocialNetworkSynchTnCText <> "" Then
                hypSocialNetworkSynchText.Text = SocialNetworkSynchTnCText
            End If
            If SocialNetworkSynchTnCPage <> "" Then
                hypSocialNetworkSynchText.NavigateUrl = Me.FixLinkForVirtualPath(SocialNetworkSynchTnCPage)
            End If

            If SocialNetworkObject IsNot Nothing Then
                If SocialNetworkUsePhotoText <> "" Then
                    chkUseSocialMediaPhoto.Text = SocialNetworkUsePhotoText
                End If
                'Navin Prasad Issue 12835
                If SocialNetworkObject.IsConnected Then
                    'Profile is synch with linkedin
                    ShowSocialNetworkIntegrationControl(m_sSyncedText, False)
                    If SocialNetworkObject.UserProfile.SynchronizeProfile Then
                        lblActivateStatus.Text = "Active"
                        lblActivateStatus.ForeColor = Drawing.Color.Green
                        lnkDeactivate.Text = "Deactivate"
                        chkUseSocialMediaPhoto.Visible = True
                        chkUseSocialMediaPhoto.Checked = SocialNetworkObject.UserProfile.UseSocialMediaPhoto
                    Else
                        lblActivateStatus.Text = "Inactive"
                        lblActivateStatus.ForeColor = Drawing.Color.Red
                        lnkDeactivate.Text = "Activate"
                        chkUseSocialMediaPhoto.Visible = False
                        chkUseSocialMediaPhoto.Checked = False
                        SocialNetworkObject.UserProfile.UseSocialMediaPhoto = False
                    End If

                Else
                    ShowSocialNetworkIntegrationControl(m_sSynchedYourProfileText, True)
                End If
            Else
                ShowSocialNetworkIntegrationControl(m_sSynchedYourProfileText, True)
            End If

        End Sub
        Protected Overridable Sub ShowSocialNetworkIntegrationControl(SyncText As String, Flag As Boolean)
            lblSyncMessage.Text = m_sSynchedYourProfileText
            SocialNetworkingIntegrationControlSF4.Visible = Flag
            chkUseSocialMediaPhoto.Visible = Not Flag
            tblsync.Visible = Not Flag
        End Sub



        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            m_sSynchedYourProfileText = "Sync your profile with " & SocialNetworkingIntegrationControlSF4.SocialNetworkSystemName
            m_sSyncedText = "Your account is synced with " & SocialNetworkingIntegrationControlSF4.SocialNetworkSystemName
            If Not IsPostBack Then
                SetProperties()
                SetSocialNetworkControls()
            End If
        End Sub

        Protected Sub lnkDeactivate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkDeactivate.Click
            Dim sError As String = ""
            If SocialNetworkObject IsNot Nothing AndAlso SocialNetworkObject.IsConnected Then
                SocialNetworkObject.UserProfile.EBusinessUser = User1
                SocialNetworkObject.UserProfile.SynchronizeProfile = CBool(IIf(lnkDeactivate.Text = "Activate", True, False))
                If User1.UserID > 0 AndAlso SocialNetworkObject.UserProfile.SynchronizePersonExternalAccount(sError) Then
                    SetSocialNetworkControls()
                ElseIf User1.UserID <= 0 Then
                    SetSocialNetworkControls()
                End If
            End If
        End Sub

        Protected Sub chkUseSocialMediaPhoto_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkUseSocialMediaPhoto.CheckedChanged
            SocialNetworkObject.UserProfile.UseSocialMediaPhoto = chkUseSocialMediaPhoto.Checked
        End Sub

        Public Property UseSocialMediaPhoto() As Boolean
            Get
                Return chkUseSocialMediaPhoto.Checked
            End Get
            Set(ByVal value As Boolean)
                chkUseSocialMediaPhoto.Checked = value
            End Set
        End Property

    End Class

End Namespace

