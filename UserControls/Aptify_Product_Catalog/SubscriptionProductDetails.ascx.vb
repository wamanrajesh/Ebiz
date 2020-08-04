'Aptify e-Business 5.5.1, July 2013
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Framework.Application
Imports Aptify.Framework.DataServices
Imports System.Data
Imports Telerik.Web.UI
Imports Aptify.Applications.OrderEntry
Namespace Aptify.Framework.Web.eBusiness.ProductCatalog
    Partial Class SubscriptionProductDetails
        Inherits BaseUserControlAdvanced

        'Protected Const ATTRIBUTE_VIEW_PRODUCT_SubscriptionProductDetailsLink_PAGE As String = "SubscriptionProductDetailsLinkCart"
        Protected Const ATTRIBUTE_VIEW_CART_PAGE As String = "ViewcartPage"
        Protected Const ATTRIBUTE_CHECKOUT_PAGE As String = "CheckoutPage"
        'Public Overridable Property SubscriptionProductDetailsLinkCart() As String
        '    Get
        '        If Not ViewState(ATTRIBUTE_VIEW_PRODUCT_SubscriptionProductDetailsLink_PAGE) Is Nothing Then
        '            Return CStr(ViewState(ATTRIBUTE_VIEW_PRODUCT_SubscriptionProductDetailsLink_PAGE))
        '        Else
        '            Return String.Empty
        '        End If
        '    End Get
        '    Set(ByVal value As String)
        '        ViewState(ATTRIBUTE_VIEW_PRODUCT_SubscriptionProductDetailsLink_PAGE) = Me.FixLinkForVirtualPath(value)
        '    End Set
        'End Property
        Public Overridable Property ViewcartPage() As String
            Get
                If Not ViewState(ATTRIBUTE_VIEW_CART_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_VIEW_CART_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_VIEW_CART_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        Public Overridable Property CheckoutPage() As String
            Get
                If Not ViewState(ATTRIBUTE_CHECKOUT_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_CHECKOUT_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_CHECKOUT_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        Protected Sub SetProprties()
            'If String.IsNullOrEmpty(SubscriptionProductDetailsLinkCart) Then
            '    'since value is the 'default' check the XML file for possible custom setting
            '    SubscriptionProductDetailsLinkCart = Me.GetLinkValueFromXML(ATTRIBUTE_VIEW_PRODUCT_SubscriptionProductDetailsLink_PAGE)
            '    If String.IsNullOrEmpty(SubscriptionProductDetailsLinkCart) Then

            '        'grdResults.ToolTip = "ViewProductCatagoryPage property has not been set."
            '    Else
            '        ' DirectCast(grdResults.Columns(2), HyperLinkColumn).DataNavigateUrlFormatString = ViewProductCatagoryPage & "?ID={0}"
            '    End If
            'Else
            '    ' DirectCast(grdResults.Columns(2), HyperLinkColumn).DataNavigateUrlFormatString = ViewProductCatagoryPage & "?ID={0}"
            'End If
            If String.IsNullOrEmpty(ViewcartPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                ViewcartPage = Me.GetLinkValueFromXML(ATTRIBUTE_VIEW_CART_PAGE)
            End If

            If String.IsNullOrEmpty(CheckoutPage) Then
                CheckoutPage = Me.GetLinkValueFromXML(ATTRIBUTE_CHECKOUT_PAGE)
            End If
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not IsPostBack Then
                SetProprties()
                LoadDetails()
            End If
        End Sub
        Public Sub LoadDetails()
            Dim sSQL As String
            Dim dt As New DataTable



            If Request.QueryString("ID") IsNot Nothing AndAlso Request.QueryString("ID").ToString() <> "" Then
                sSQL = "SELECT Top 1  ID,Name FROM " & _
                                AptifyApplication.GetEntityBaseDatabase("Products") & _
                                "..vwProducts WHERE ID=" & Request.QueryString("ID").ToString()
                dt = DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)
            End If


            If (dt.Rows.Count > 0) Then
                lblproduct.Text = dt.Rows(0)("Name").ToString()
                'lblSubscriptionval.Text = dt.Rows(0)("StartDate").ToString()
                'lblSubscriptionvalEnd.Text = dt.Rows(0)("EndDate").ToString()
                'lblstatus.Text = dt.Rows(0)("Status").ToString()
            End If
            chkAutoRenew.Checked = Request.QueryString("Autorenew").ToString()
        End Sub

        Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
            Dim oOrder As OrdersEntity
            Dim oOrderLine As OrderLinesEntity
            oOrder = ShoppingCart1.GetOrderObject(Page.Session, Page.User, Page.Application)
            oOrderLine = CType(oOrder.SubTypes("OrderLines").Item(Request.QueryString("Index")), OrderLinesEntity)
            oOrderLine.SetValue("AutoRenew", chkAutoRenew.Checked)
            ShoppingCart1.SaveCart(Page.Session)

            Dim strPrevPage As String = Request.QueryString("PrevPage")

            If strPrevPage IsNot Nothing Then
                If strPrevPage.ToUpper.Contains("VIEWCART") Then
                    Response.Redirect(ViewcartPage)
                ElseIf strPrevPage.ToUpper.Contains("CHECKOUT") Then
                    Response.Redirect(CheckoutPage)
                Else
                    Response.Redirect(ViewcartPage)
                End If
            Else
                Response.Redirect(ViewcartPage)
            End If
        End Sub
    End Class
End Namespace
