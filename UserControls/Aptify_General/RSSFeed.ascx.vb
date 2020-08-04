'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Namespace Aptify.Framework.Web.eBusiness

    ''' <summary>
    ''' This is a simple user control that will notify the browser of an existing RSS broadcast
    ''' for the associated Aptify RSSChannel RecordID. This control can be dropped on any page or
    ''' other user control. Just set the ChannelID and RSSTitle properties. The RSSChannel record
    ''' metadata defines all of the RSS Feed properties.
    ''' </summary>
    ''' <remarks></remarks>
    Partial Class RSSFeed
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced

        Private m_HanderURL As String = String.Empty
        Private m_sRSSTitle As String = String.Empty
        Private m_lChannelID As Long = 0

        Protected Const ATTRIBUTE_RSS_FEED_IMAGE_URL As String = "RSSFeedImage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "RSSFeed"
        'Neha issue 14408, 03/28/13, declare property for changepassword 
        Protected Const ATTRIBUTE_HOME_CHANGEPWD As String = "ChangePassword"



#Region "RSSFeed Specific Properties"
        ''' <summary>
        ''' RSSFeedImage url
        ''' </summary>
        Public Overridable Property RSSFeedImage() As String
            Get
                If Not ViewState(ATTRIBUTE_RSS_FEED_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_RSS_FEED_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_RSS_FEED_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        'Neha Issue 14408,03/28/13,added property for ChangePassword,added ChangePassword page url
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
        ''' Must set ChannelID to the Aptify RSSChannel RecordID this RSSFeed belongs to.
        ''' If this property is not set the RSSFeed user control will be disabled.
        ''' The RSSChannel record metadata defines all of the RSS Feed properties.
        ''' </summary>
        Public Property ChannelID() As Long
            Get
                Return m_lChannelID
            End Get
            Set(ByVal value As Long)
                m_lChannelID = value
            End Set
        End Property
        ''' <summary>
        ''' The RSS Broadcast title is required for the link tag that will notify
        ''' the user's browser of the existing RSS Feed.
        ''' </summary>
        Public Property RSSTitle() As String
            Get
                Return m_sRSSTitle
            End Get
            Set(ByVal value As String)
                m_sRSSTitle = value
            End Set
        End Property
#End Region

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(RSSFeedImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                RSSFeedImage = Me.GetLinkValueFromXML(ATTRIBUTE_RSS_FEED_IMAGE_URL)
            End If
            'Neha issue 14408, 03/28/13, set property for chnagepassword page
            If String.IsNullOrEmpty(ChangePassword) Then
                'since value is the 'default' check the XML file for possible custom setting
                ChangePassword = Me.GetLinkValueFromXML(ATTRIBUTE_HOME_CHANGEPWD)
            End If
        End Sub

        ''' <summary>
        ''' On the Load event the RSSFeed's link (PostBackURL property) will be set using the 
        ''' ChannelID property. A Link tag is also inserted in to the MasterPage's Head tag, 
        ''' which is required for browser auto-detection of the RSS broadcast.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim CheckSessionValue As Boolean
            'set control properties from XML file if needed
            SetProperties()
            If Not IsPostBack Then
                If Me.ChannelID > 0 Then
                    'Set the Link to the RSSChannel RecordID as specified by the ChannelID property
                    Me.lnkRSSImage.NavigateUrl = "~/Handlers/RSSFeed.ashx?ChannelID=" & Me.ChannelID.ToString
                    Me.lnkRSSImage.ImageUrl = RSSFeedImage

                    'Me.RSSFeedImageButton.PostBackUrl = "~/Handlers/RSSFeed.ashx?ChannelID=" & Me.ChannelID.ToString
                    'Response.Write("URL: " & Me.RSSFeedImageButton.PostBackUrl)
                    'Dynamically create the <link> tag that will be used to notify the user's browser of the RSS broadcast
                    'attempt to put this in the Master's Head tag
                    Dim linkTag As New System.Web.UI.HtmlControls.HtmlGenericControl("link")
                    linkTag.Attributes.Add("rel", "alternate")
                    linkTag.Attributes.Add("type", "application/rss+xml")
                    linkTag.Attributes.Add("title", Me.RSSTitle)
                    linkTag.Attributes.Add("href", Me.lnkRSSImage.NavigateUrl)
                    Me.Page.Header.Controls.Add(linkTag)
                    'Me.Page.Master.FindControl("MasterHead").Controls.Add(linkTag)

                    'Neha, Issue 14408,03/20/13,checkNewuser value and redirect to changepassword page
                    If Session("CheckNewUser") IsNot Nothing Then
                        CheckSessionValue = Convert.ToBoolean(Session("CheckNewUser"))
                        If CheckSessionValue Then
                            Session.Remove("CheckNewUser")
                            Response.Redirect(ChangePassword)
                        End If
                    End If
                End If
            End If
        End Sub
    End Class
End Namespace


