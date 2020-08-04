'Aptify e-Business 5.5.1, July 2013


Namespace Aptify.Framework.Web.eBusiness
    Partial Class FollowUs
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "FollowUs"

        Protected Const ATTRIBUTE_FACEBOOK_IMG_URL As String = "FacebookImage"
        Protected Const ATTRIBUTE_FACEBOOK_URL As String = "FacebookUrl"
        Protected Const ATTRIBUTE_TWITTER_IMG_URL As String = "TwitterImage"
        Protected Const ATTRIBUTE_TWITTER_URL As String = "TwitterUrl"
        Protected Const ATTRIBUTE_LINKEDIN_IMG_URL As String = "LinkedInImage"
        Protected Const ATTRIBUTE_LINKEDIN_URL As String = "LinkedInUrl"


#Region "Properties"


        Public Overridable Property FacebookImage() As String
            Get
                If Not ViewState(ATTRIBUTE_FACEBOOK_IMG_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_FACEBOOK_IMG_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_FACEBOOK_IMG_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        Public Overridable Property FacebookUrl() As String
            Get
                If Not ViewState(ATTRIBUTE_FACEBOOK_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_FACEBOOK_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_FACEBOOK_URL) = value
            End Set
        End Property

        Public Overridable Property TwitterImage() As String
            Get
                If Not ViewState(ATTRIBUTE_TWITTER_IMG_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_TWITTER_IMG_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_TWITTER_IMG_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        Public Overridable Property TwitterUrl() As String
            Get
                If Not ViewState(ATTRIBUTE_TWITTER_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_TWITTER_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_TWITTER_URL) = value
            End Set
        End Property

        Public Overridable Property LinkedInImage() As String
            Get
                If Not ViewState(ATTRIBUTE_LINKEDIN_IMG_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_LINKEDIN_IMG_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_LINKEDIN_IMG_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        Public Overridable Property LinkedInUrl() As String
            Get
                If Not ViewState(ATTRIBUTE_LINKEDIN_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_LINKEDIN_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_LINKEDIN_URL) = value
            End Set
        End Property
#End Region

#Region "Procedures"
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


            Try
                SetProperties()
                ImgFaceBook.ImageUrl = FacebookImage
                lnkFaceBook.NavigateUrl = FacebookUrl
                ImgTwitter.ImageUrl = TwitterImage
                lnkTwitter.NavigateUrl = TwitterUrl
                imgLinkedIn.ImageUrl = LinkedInImage
                lnkLinkedIn.NavigateUrl = LinkedInUrl
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try

        End Sub

        Protected Overrides Sub SetProperties()
            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            MyBase.SetProperties()
            If String.IsNullOrEmpty(FacebookImage) Then
                FacebookImage = Me.GetLinkValueFromXML(ATTRIBUTE_FACEBOOK_IMG_URL)
            End If

            If String.IsNullOrEmpty(FacebookUrl) Then
                FacebookUrl = Me.GetLinkValueFromXML(ATTRIBUTE_FACEBOOK_URL)
            End If

            If String.IsNullOrEmpty(TwitterImage) Then
                TwitterImage = Me.GetLinkValueFromXML(ATTRIBUTE_TWITTER_IMG_URL)
            End If

            If String.IsNullOrEmpty(TwitterUrl) Then
                TwitterUrl = Me.GetLinkValueFromXML(ATTRIBUTE_TWITTER_URL)
            End If

            If String.IsNullOrEmpty(LinkedInImage) Then
                LinkedInImage = Me.GetLinkValueFromXML(ATTRIBUTE_LINKEDIN_IMG_URL)
            End If

            If String.IsNullOrEmpty(LinkedInUrl) Then
                LinkedInUrl = Me.GetLinkValueFromXML(ATTRIBUTE_LINKEDIN_URL)
            End If

        End Sub
#End Region

    End Class

End Namespace
