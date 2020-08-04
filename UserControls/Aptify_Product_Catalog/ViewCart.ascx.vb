'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On
Imports Aptify.Framework.DataServices
Imports System.Data
Imports Aptify.Framework.BusinessLogic.GenericEntity

Namespace Aptify.Framework.Web.eBusiness.ProductCatalog
    Partial Class ViewCartControl
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CONTINUE_SHOPPING_BUTTON_PAGE As String = "ContinueShoppingButtonPage"
        Protected Const ATTRIBUTE_CHECKOUT_BUTTON_PAGE As String = "CheckOutButtonPage"
        Protected Const ATTRIBUTE_SAVE_CART_BUTTON_PAGE As String = "SaveCartButtonPage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "ViewCartPage"
        Protected Const ATTRIBUTE_SAVING_MESSAGE As String = "SavingMessage"

#Region "ViewCart Specific Properties"
        ''' <summary>
        ''' ContinueShoppingButton page url
        ''' </summary>
        Public Overridable Property ContinueShoppingButtonPage() As String
            Get
                If Not ViewState(ATTRIBUTE_CONTINUE_SHOPPING_BUTTON_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_CONTINUE_SHOPPING_BUTTON_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_CONTINUE_SHOPPING_BUTTON_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' CheckOutButton page url
        ''' </summary>
        Public Overridable Property CheckOutButtonPage() As String
            Get
                If Not ViewState(ATTRIBUTE_CHECKOUT_BUTTON_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_CHECKOUT_BUTTON_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_CHECKOUT_BUTTON_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' SaveCartButton page url
        ''' </summary>
        Public Overridable Property SaveCartButtonPage() As String
            Get
                If Not ViewState(ATTRIBUTE_SAVE_CART_BUTTON_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_SAVE_CART_BUTTON_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_SAVE_CART_BUTTON_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''RashmiP, issue 10021
        Dim m_sSavingMsg As String
        Public Overridable Property SavingMessage() As String
            Get
                Return m_sSavingMsg
            End Get
            Set(ByVal value As String)
                m_sSavingMsg = value
            End Set
        End Property

        Public Overridable Property ViewCartPage() As String
            Get
                If Not ViewState(ATTRIBUTE_CONTORL_DEFAULT_NAME) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_CONTORL_DEFAULT_NAME))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_CONTORL_DEFAULT_NAME) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

#End Region

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(ContinueShoppingButtonPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                ContinueShoppingButtonPage = Me.GetLinkValueFromXML(ATTRIBUTE_CONTINUE_SHOPPING_BUTTON_PAGE)
                If String.IsNullOrEmpty(ContinueShoppingButtonPage) Then
                    Me.cmdShop.Enabled = False
                    Me.cmdShop.ToolTip = "ContinueShoppingButtonPage property has not been set."
                End If
            End If
            If String.IsNullOrEmpty(CheckOutButtonPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                CheckOutButtonPage = Me.GetLinkValueFromXML(ATTRIBUTE_CHECKOUT_BUTTON_PAGE)
                If String.IsNullOrEmpty(CheckOutButtonPage) Then
                    Me.cmdCheckOut.Enabled = False
                    Me.cmdCheckOut.ToolTip = "CheckOutButtonPage property has not been set."
                End If
            End If
            If String.IsNullOrEmpty(SaveCartButtonPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                SaveCartButtonPage = Me.GetLinkValueFromXML(ATTRIBUTE_SAVE_CART_BUTTON_PAGE)
                If String.IsNullOrEmpty(SaveCartButtonPage) Then
                    Me.cmdSaveCart.Enabled = False
                    Me.cmdSaveCart.ToolTip = "SaveCartButtonPage property has not been set."
                End If
            End If
            ''RashmiP
            If Not String.IsNullOrEmpty(Me.GetPropertyValueFromXML(ATTRIBUTE_SAVING_MESSAGE)) Then
                SavingMessage = Me.GetPropertyValueFromXML(ATTRIBUTE_SAVING_MESSAGE)
            End If
            If String.IsNullOrEmpty(ViewCartPage) Then
                ViewCartPage = Me.GetLinkValueFromXML(ATTRIBUTE_CONTORL_DEFAULT_NAME)
            End If
        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
            Try
                If Not IsPostBack Then

                    If CLng(Request.QueryString("ShoppingCartID")) > 0 Then
                        'HP Issue#9078: only load if the Cart belongs to the logged-in user
                        If CartBelongsToUser(CLng(Request.QueryString("ShoppingCartID"))) Then
                            '    'Load existing shopping cart.  
                            CartGrid.LoadCart(CLng(Request.QueryString("ShoppingCartID")))
                            RefreshGrid()
                        Else
                            RefreshGrid()
                        End If
                    Else
                        RefreshGrid()
                    End If

                    UpdateCampaignDisplay()
                    LoadShipmentType()
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        'HP Issue#9078: return true if shoppingCart ID provided belongs to the logged-in user
        Private Function CartBelongsToUser(ByVal id As Long) As Boolean
            Dim sSQL As String
            Dim value As String

            sSQL = "SELECT Name FROM " & Database & "..vwWebShoppingCarts " & _
                   "WHERE WebUserID = " & User1.UserID & " AND ID = " & CLng(Request.QueryString("ShoppingCartID"))
            value = CStr(DataAction.ExecuteScalar(sSQL))

            Return Not String.IsNullOrEmpty(value)
        End Function

        Private Sub RefreshGrid()
            Try
                CartGrid.RefreshGrid()
                'Navin Prasad Issue 11032

                If CartGrid.Grid.Items.Count > 0 Then
                    tblRowNoItems.Visible = False
                    cmdUpdateCart.Visible = True
                    cmdCheckOut.Visible = True
                    cmdSaveCart.Visible = True
                Else
                    tblRowNoItems.Visible = True
                    cmdUpdateCart.Visible = False
                    cmdCheckOut.Visible = False
                    cmdSaveCart.Visible = False
                    divTotals.Visible = False
                    divCampaign.Visible = False
                    'divhr.Visible = False
                    tblbuttons.Visible = False
                End If
                Dim sCurrencyFormat As String
                With CartGrid.Cart
                    sCurrencyFormat = .GetCurrencyFormat(.CurrencyTypeID)
                    lblSubTotal.Text = Format$(.SubTotal, sCurrencyFormat)
                    lblShipping.Text = Format$(.ShippingAndHandlingCharges, sCurrencyFormat)
                    lblTax.Text = Format$(.Tax, sCurrencyFormat)
                    lblGrandTotal.Text = Format$(.GrandTotal, sCurrencyFormat)
                    'Only check if savings exist if user is logged in
                    If User1.PersonID > 0 Then
                        '20090126 MAS: using different logic for calculating Member Savings since the original way was not properly
                        '              calculating complex pricing rules.
                        '              NOTE: member savings can only be calculated for a User that is logged into the website, 
                        '                    since pricing may be based on the User's address.
                        Dim dSavings As Decimal
                        dSavings = .GetCartMemberSavings(Page.Session, Page.User, Page.Application)
                        If dSavings > 0 Then
                            spnSavings.Visible = True
                            If Not String.IsNullOrEmpty(SavingMessage) Then
                                SavingMessage = SavingMessage.Replace("{0}", Format$(dSavings, sCurrencyFormat))
                                lblTotalSavings.Text = SavingMessage
                            Else
                                lblTotalSavings.Text = "You have saved " + Format$(dSavings, sCurrencyFormat) + "   in your shopping cart since you are a valued member!"
                            End If
                        Else
                            spnSavings.Visible = False
                        End If
                    Else
                        spnSavings.Visible = False
                    End If
                End With
             
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub cmdUpdateCart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdateCart.Click
            ' update the shopping cart object
            ' Iterate through all rows within shopping cart list
            Try
                CartGrid.UpdateCart()
                RefreshGrid()
                MyBase.Response.Redirect(ViewCartPage, False)
               
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub cmdShop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdShop.Click
            Response.Redirect(ContinueShoppingButtonPage)
        End Sub

        Private Sub cmdCheckOut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCheckOut.Click
            Try
                'CartGrid.UpdateCart()
                CartGrid.SaveCart()
                Response.Redirect(CheckOutButtonPage, False)
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub cmdSaveCart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSaveCart.Click
            Try
                If CLng(Request.QueryString("ShoppingCartID")) > 0 And CartBelongsToUser(CLng(Request.QueryString("ShoppingCartID"))) Then
                    'Response.Redirect("SaveCart.aspx?ShoppingCartID=" & Request.QueryString("ShoppingCartID"))
                    Response.Redirect(SaveCartButtonPage & "?ShoppingCartID=" & CStr(Request.QueryString("ShoppingCartID")))
                ElseIf (CartGrid.Cart.CartID > 0) Then
                    Response.Redirect(SaveCartButtonPage & "?ShoppingCartID=" & CStr(CartGrid.Cart.CartID))
                Else
                    Response.Redirect(SaveCartButtonPage)
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub cmdApplyCampaign_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdApplyCampaign.Click
            Try
                If Not CartGrid.Cart.SetOrderCampaign(txtCampaign.Text, Page.Session) Then
                    lblCampaignMsg.Text = "Could not associate campaign code with order."
                Else
                    lblCampaignMsg.Text = ""
                End If
                CartGrid.UpdateCart()
                RefreshGrid()
                UpdateCampaignDisplay(True)
                ' Refresh the control
                MyBase.Response.Redirect(ViewCartPage, False)
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub cmdRemoveCampaign_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRemoveCampaign.Click
            Try
                CartGrid.Cart.RemoveOrderCampaign(Page.Session)
                lblCampaignMsg.Text = ""
                RefreshGrid()
                UpdateCampaignDisplay()
                ' Refresh the control
                MyBase.Response.Redirect(ViewCartPage, False)
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub UpdateCampaignDisplay(Optional ByVal bShowError As Boolean = False)
            If CartGrid.Cart.CampaignCodeID <> -1 Then
                cmdApplyCampaign.Visible = False
                lblCampaignInstructions.Visible = False
                cmdRemoveCampaign.Visible = True
                lblCampaignMsg.Visible = True
                txtCampaign.Visible = False
                lblCampaignMsg.Text = "Campaign code '" & CartGrid.Cart.CampaignCodeName & "' applied."
            Else
                cmdApplyCampaign.Visible = True
                lblCampaignInstructions.Visible = True
                cmdRemoveCampaign.Visible = False
                If bShowError Then
                    lblCampaignMsg.Visible = True
                End If
                txtCampaign.Visible = True
            End If
        End Sub
        ''' <summary>
        ''' Rashmi P, Issue 5133, 12/6/12 Add ShipmentType Selection.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub LoadShipmentType()

            Dim oShipmentTypes As New Aptify.Framework.Web.eBusiness.CommonMethods(DataAction, AptifyApplication, User1, Database)
            Dim oOrder As Aptify.Applications.OrderEntry.OrdersEntity
            oOrder = CartGrid.Cart.GetOrderObject(Page.Session, Page.User, Page.Application)
            Dim bIncludeInShipping As Boolean
            Dim dt As DataTable = Nothing
            Try
                If oOrder IsNot Nothing Then
                    For Each oOrderLine As Aptify.Applications.OrderEntry.OrderLinesEntity In oOrder.SubTypes("OrderLines")
                        bIncludeInShipping = oShipmentTypes.IncludeInShipping(CLng(oOrderLine.GetValue("ProductID")))
                        If bIncludeInShipping = True Then
                            Exit For
                        End If
                    Next
                    If bIncludeInShipping Then
                        dt = oShipmentTypes.LoadShipmentType(CInt(oOrder.GetValue("ShipToCountryCodeID")))
                    End If
                End If
             
                If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                    tdShipment.Visible = False
                    Exit Sub
                End If

                dt.Columns.Add("DisplayField")
                For Each dr As DataRow In dt.Rows

                    dr("DisplayField") = Convert.ToString(dr("Name")).Replace("&reg;", "®")
                Next

                ddlShipmentType.DataTextField = "DisplayField"
                ddlShipmentType.DataValueField = "ID"


                ddlShipmentType.DataSource = dt
                ddlShipmentType.DataBind()
               
                If ddlShipmentType.Items.Count > 0 Then  'Added by sandeep for issue on 07/03/2013
                    If Me.CartGrid.Grid.Items.Count > 0 Then
                        If Not oOrder Is Nothing Then
                            ddlShipmentType.SelectedValue = CStr(oOrder.ShipTypeID)
                        Else
                            ddlShipmentType_SelectedIndexChanged(Nothing, Nothing)
                        End If
                    End If
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        ''' <summary>
        ''' Rashmi P, Issue 5133, 12/6/12 Add ShipmentType Selection.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub ddlShipmentType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlShipmentType.SelectedIndexChanged
            'This is how you set the Shipping Charge to show on Order
            If Not ddlShipmentType.SelectedItem Is Nothing AndAlso ddlShipmentType.SelectedItem.Text <> String.Empty Then
                Dim oOrder As Aptify.Applications.OrderEntry.OrdersEntity
                oOrder = CartGrid.Cart.GetOrderObject(Session, Page.User, Page.Application)
                oOrder.SetValue("ShipTypeID", ddlShipmentType.SelectedValue)

                oOrder.CalculateOrderTotals(True, True)
                CartGrid.Cart.SaveCart(Session)
                RefreshGrid()
            End If
        End Sub
      
    End Class
End Namespace
