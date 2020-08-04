'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports System.Data
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.Application

Namespace Aptify.Framework.Web.eBusiness.ProductCatalog
    Partial Class OrderSummaryControl
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "OrderSummary"

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
        End Sub
        Protected Overrides Sub SetProperties()
            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()
        End Sub

        Private Property ShoppingCart() As Aptify.Framework.Web.eBusiness.AptifyShoppingCart
            Get
                If Context.Items("ShoppingCartCtrl") IsNot Nothing Then
                    Return TryCast(Context.Items("ShoppingCartCtrl"), Aptify.Framework.Web.eBusiness.AptifyShoppingCart)
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal value As Aptify.Framework.Web.eBusiness.AptifyShoppingCart)
                Context.Items("ShoppingCartCtrl") = value
            End Set
        End Property

        Public Overridable Sub Refresh()
            Try
                Dim sc As Aptify.Framework.Web.eBusiness.AptifyShoppingCart
                If Me.ShoppingCart IsNot Nothing Then
                    sc = Me.ShoppingCart
                Else
                    sc = ShoppingCart1
                End If

                With sc
                    lblSubTotal.Text = Format$(.SubTotal, .GetCurrencyFormat(.CurrencyTypeID))
                    lblShipping.Text = Format$(.ShippingAndHandlingCharges, .GetCurrencyFormat(.CurrencyTypeID))
                    lblTax.Text = Format$(.Tax, .GetCurrencyFormat(.CurrencyTypeID))
                    lblTotal.Text = Format$(.GrandTotal, .GetCurrencyFormat(.CurrencyTypeID))
                End With
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub


        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
            'Anil B for Credit Card recognization Performance on 21/jun/2013
            If Not IsPostBack Then
                Refresh()
            End If
        End Sub
    End Class
End Namespace
