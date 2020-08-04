'Aptify e-Business 5.5.1, July 2013
Option Explicit On

Imports Aptify.Framework.Application
Imports Aptify.Framework.ExceptionManagement.ExceptionManager
Imports Aptify.Framework.BusinessLogic.GenericEntity

Namespace Aptify.Framework.Web.eBusiness
    Partial Class ItemsInCart
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced
        Protected Const ATTRIBUTE_USE_ORDERLINES As String = "UseOrderLines"
        Protected Const ATTRIBUTE_VIEWCART_URL As String = "RedirectViewCart"
        Protected Const ATTRIBUTE_CART_IMG_URL As String = "CartImage"

#Region "Properties"
        ''' <summary>
        ''' Rashmi P
        ''' Property set true to show Quantity of Items or No. of Order line in Item Cart
        ''' </summary>
        Public Overridable ReadOnly Property UseOrderLines() As Boolean
            Get
                If Not ViewState.Item(ATTRIBUTE_USE_ORDERLINES) Is Nothing Then
                    Return CStr(ViewState.Item(ATTRIBUTE_USE_ORDERLINES))
                Else
                    Dim value As String = Me.GetGlobalAttributeValue(ATTRIBUTE_USE_ORDERLINES)
                    If Not String.IsNullOrEmpty(value) Then
                        ViewState.Item(ATTRIBUTE_USE_ORDERLINES) = value
                        Return value
                    Else
                        Return False
                    End If
                End If
            End Get

        End Property
        ''Link navigate to View Cart
        Public Overridable Property RedirectViewCart() As String
            Get
                If Not ViewState(ATTRIBUTE_VIEWCART_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_VIEWCART_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_VIEWCART_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        ''Cart Image URL
        Public Overridable Property CartImage() As String
            Get
                If Not Session(ATTRIBUTE_CART_IMG_URL) Is Nothing Then
                    Return CStr(Session(ATTRIBUTE_CART_IMG_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                Session(ATTRIBUTE_CART_IMG_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
#End Region

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            SetProperties()
            If Not Page.IsPostBack Then
                lblItemsInCart.Visible = True
                imgCart.Visible = True
                lblViewcart.Visible = True
                lblItemsInCart.Text = "<a href=" + RedirectViewCart + " style ='color:#e0800c'>" + " 0 Item</a>"
                imgCart.Src = CartImage
                'Anil B for Credit Card recognization Performance on 21/jun/2013
                ItemsUpdate()
            End If
        End Sub

#Region "Procedures"
        Protected Overrides Sub SetProperties()
            If String.IsNullOrEmpty(RedirectViewCart) Then
                RedirectViewCart = Me.GetLinkValueFromXML(ATTRIBUTE_VIEWCART_URL)
            End If
            If String.IsNullOrEmpty(CartImage) Then
                CartImage = Me.GetLinkValueFromXML(ATTRIBUTE_CART_IMG_URL)
            End If

        End Sub

        Public Sub ItemsUpdate()

            Try

                If ShoppingCart1 IsNot Nothing AndAlso ShoppingCart1.GetOrderObject(Page.Session, Page.User, Page.Application) IsNot Nothing Then
                    Dim iLine As Integer
                    Dim lQuantity As Long
                    Dim oOrder As Aptify.Applications.OrderEntry.OrdersEntity

                    If ShoppingCart1.GetOrderObject(Page.Session, Page.User, Page.Application).SubTypes("OrderLines").Count > 0 Then

                        oOrder = ShoppingCart1.GetOrderObject(Page.Session, Page.User, Page.Application)
                        If oOrder IsNot Nothing AndAlso oOrder.SubTypes("OrderLines").Count >= 0 Then
                            lblItemsInCart.Visible = True
                            imgCart.Visible = True
                            lblViewcart.Visible = True
                            iLine = oOrder.SubTypes("OrderLines").Count
                            Dim i, icount As Integer

                            If UseOrderLines Then
                                lblItemsInCart.Text = "<a href=" + RedirectViewCart + " style ='color:#e0800c'>" + iLine.ToString
                                icount = iLine
                            Else
                                For i = 0 To iLine - 1
                                    lQuantity = lQuantity + oOrder.SubTypes("OrderLines").Item(i).GetValue("Quantity")
                                Next
                                lblItemsInCart.Text = "<a href=" + RedirectViewCart + " style ='color:#e0800c'>" + lQuantity.ToString
                                icount = lQuantity
                            End If
                            If icount > 1 Then
                                lblItemsInCart.Text = lblItemsInCart.Text + " Items"
                            Else
                                lblItemsInCart.Text = lblItemsInCart.Text + " Item"
                            End If
                            lblItemsInCart.Text = lblItemsInCart.Text + "</a> <span class=""login_cart_price"">(" + Format$(ShoppingCart1.GrandTotal, ShoppingCart1.GetCurrencyFormat(ShoppingCart1.CurrencyTypeID)) + ")</span>"

                        End If
                        'Nalini Issue 11858
                    Else
                        lblItemsInCart.Text = "<a href=" + RedirectViewCart + " style ='color:#e0800c'>" + " 0 Item</a>"
                        imgCart.Src = CartImage

                    End If
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Publish(New Exception("In ItemsUpdate..."))
            End Try

        End Sub
#End Region
    End Class
End Namespace