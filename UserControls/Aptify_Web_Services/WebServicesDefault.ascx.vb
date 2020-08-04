'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Namespace Aptify.Framework.Web.eBusiness.WebServices
    Partial Class DefaultControl
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_GET_ORDER_INFO_URL As String = "GetOrderInfoURL"
        Protected Const ATTRIBUTE_UPDATE_PERSON_URL As String = "UpdatePersonURL"
        Protected Const ATTRIBUTE_GET_PRODUCT_LISTING_URL As String = "GetProductListingURL"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "WebServicesDefault"

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
        End Sub

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(GetOrderInfoURL) Then
                GetOrderInfoURL = Me.GetLinkValueFromXML(ATTRIBUTE_GET_ORDER_INFO_URL)
                If String.IsNullOrEmpty(GetOrderInfoURL) Then
                    Me.lnkOrderInfo.Enabled = False
                    Me.lnkOrderInfo.ToolTip = "GetOrderInfoURL property has not been set."
                Else
                    Me.lnkOrderInfo.NavigateUrl = GetOrderInfoURL
                End If
            Else
                Me.lnkOrderInfo.NavigateUrl = GetOrderInfoURL
            End If

            If String.IsNullOrEmpty(UpdatePersonURL) Then
                UpdatePersonURL = Me.GetLinkValueFromXML(ATTRIBUTE_UPDATE_PERSON_URL)
                If String.IsNullOrEmpty(UpdatePersonURL) Then
                    Me.lnkUpdatePerson.Enabled = False
                    Me.lnkUpdatePerson.ToolTip = "UpdatePersonURL property has not been set."
                Else
                    Me.lnkUpdatePerson.NavigateUrl = UpdatePersonURL
                End If
            Else
                Me.lnkUpdatePerson.NavigateUrl = UpdatePersonURL
            End If

            If String.IsNullOrEmpty(GetProductListingURL) Then
                GetProductListingURL = Me.GetLinkValueFromXML(ATTRIBUTE_GET_PRODUCT_LISTING_URL)
                If String.IsNullOrEmpty(GetProductListingURL) Then
                    Me.lnkProductListing.Enabled = False
                    Me.lnkProductListing.ToolTip = "GetProductListingURL property has not been set."
                Else
                    Me.lnkProductListing.NavigateUrl = GetProductListingURL
                End If
            Else
                Me.lnkProductListing.NavigateUrl = GetProductListingURL
            End If

        End Sub

#Region "WebServices Specific Properties"
        ''' <summary>
        ''' GetOrderInfo page url
        ''' </summary>
        Public Overridable Property GetOrderInfoURL() As String
            Get
                If Not ViewState(ATTRIBUTE_GET_ORDER_INFO_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_GET_ORDER_INFO_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_GET_ORDER_INFO_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' UpdatePerson page url
        ''' </summary>
        Public Overridable Property UpdatePersonURL() As String
            Get
                If Not ViewState(ATTRIBUTE_UPDATE_PERSON_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_UPDATE_PERSON_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_UPDATE_PERSON_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' GetProductListing page url
        ''' </summary>
        Public Overridable Property GetProductListingURL() As String
            Get
                If Not ViewState(ATTRIBUTE_GET_PRODUCT_LISTING_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_GET_PRODUCT_LISTING_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_GET_PRODUCT_LISTING_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
#End Region

    End Class
End Namespace
