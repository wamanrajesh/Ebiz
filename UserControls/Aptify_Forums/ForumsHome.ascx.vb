'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Namespace Aptify.Framework.Web.eBusiness.Forums
    Partial Class ForumsHome
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_SEARCH_PAGE As String = "SearchPage"
        Protected Const ATTRIBUTE_SUBSCRIPTIONS_PAGE As String = "SubscriptionsPage"
        Protected Const ATTRIBUTE_SEARCH_IMAGE_URL As String = "SearchImage"
        Protected Const ATTRIBUTE_SUBSCRIPTIONS_IMAGE_URL As String = "SubscriptionsImage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "ForumsHome"

#Region "ForumsHome Specific Properties"
        ''' <summary>
        ''' Subscriptions page url
        ''' </summary>
        Public Overridable Property SubscriptionsPage() As String
            Get
                If Not ViewState(ATTRIBUTE_SUBSCRIPTIONS_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_SUBSCRIPTIONS_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_SUBSCRIPTIONS_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' Search page url
        ''' </summary>
        Public Overridable Property SearchPage() As String
            Get
                If Not ViewState(ATTRIBUTE_SEARCH_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_SEARCH_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_SEARCH_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' SearchImage url
        ''' </summary>
        Public Overridable Property SearchImage() As String
            Get
                If Not ViewState(ATTRIBUTE_SEARCH_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_SEARCH_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_SEARCH_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' SubscriptionsImage url
        ''' </summary>
        Public Overridable Property SubscriptionsImage() As String
            Get
                If Not ViewState(ATTRIBUTE_SUBSCRIPTIONS_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_SUBSCRIPTIONS_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_SUBSCRIPTIONS_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
#End Region

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
            Try
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(SearchPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                SearchPage = Me.GetLinkValueFromXML(ATTRIBUTE_SEARCH_PAGE)
                If String.IsNullOrEmpty(SearchPage) Then
                    Me.lnkSearch.Enabled = False
                    Me.lnkSearch.ToolTip = "SearchPage property has not been set."
                Else
                    Me.lnkSearch.NavigateUrl = SearchPage
                End If
            Else
                Me.lnkSearch.NavigateUrl = SearchPage
            End If
            If String.IsNullOrEmpty(SearchImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                SearchImage = Me.GetLinkValueFromXML(ATTRIBUTE_SEARCH_IMAGE_URL)
                imgSearch.Src = SearchImage
            End If
            If String.IsNullOrEmpty(SubscriptionsPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                SubscriptionsPage = Me.GetLinkValueFromXML(ATTRIBUTE_SUBSCRIPTIONS_PAGE)
                If String.IsNullOrEmpty(SubscriptionsPage) Then
                    Me.lnkSubscribe.Enabled = False
                    Me.lnkSubscribe.ToolTip = "SubscriptionsPage property has not been set."
                Else
                    Me.lnkSubscribe.NavigateUrl = SubscriptionsPage
                End If
            Else
                Me.lnkSubscribe.NavigateUrl = SubscriptionsPage
            End If
            If String.IsNullOrEmpty(SubscriptionsImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                SubscriptionsImage = Me.GetLinkValueFromXML(ATTRIBUTE_SUBSCRIPTIONS_IMAGE_URL)
                imgSubscribe.Src = SubscriptionsImage
            End If

        End Sub

    End Class
End Namespace
