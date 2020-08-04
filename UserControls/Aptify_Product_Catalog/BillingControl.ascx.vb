'Aptify e-Business 5.5.1 SR1, June 2014
Option Explicit On
Option Strict On

Imports Aptify.Framework.Web.eBusiness
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports System.Data
Imports Aptify.Applications.OrderEntry.Payments
Imports Aptify.Applications.Accounting
Imports Aptify.Framework.Application
Imports Aptify.Framework.BusinessLogic.ProcessPipeline
Imports Aptify.Framework.DataServices
Imports Telerik.Web.UI
Imports Aptify.Applications.OrderEntry


Namespace Aptify.Framework.Web.eBusiness.ProductCatalog
    Partial Class BillingControl
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_BILLING_CHANGE_IMAGE_URL As String = "BillingChangeImage"
        Protected Const ATTRIBUTE_BILLING_CHANGE_PAGE As String = "BillingChangePage"
        Protected Const ATTRIBUTE_CONFIRMATION_PAGE As String = "OrderConfirmationPage"
        Protected Const ATTRIBUTE_BACK_BUTTON_PAGE As String = "BackButtonPage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "BillingControl"
        Protected Const ATTRIBUTE_BILL_ME_LATER As String = "BillMeLaterDisplayText"



#Region "BillingControl Specific Properties"
        ''' <summary>
        ''' BillingChangeImage url
        ''' </summary>
        Public Overridable Property BillingChangeImage() As String
            Get
                If Not ViewState(ATTRIBUTE_BILLING_CHANGE_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_BILLING_CHANGE_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_BILLING_CHANGE_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' BillingChange page url
        ''' </summary>
        Public Overridable Property BillingChangePage() As String
            Get
                If Not ViewState(ATTRIBUTE_BILLING_CHANGE_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_BILLING_CHANGE_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_BILLING_CHANGE_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' BackButton page url
        ''' </summary>
        Public Overridable Property BackButtonPage() As String
            Get
                If Not ViewState(ATTRIBUTE_BACK_BUTTON_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_BACK_BUTTON_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_BACK_BUTTON_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' OrderConfirmation page url
        ''' </summary>
        Public Overridable Property OrderConfirmationPage() As String
            Get
                If Not ViewState(ATTRIBUTE_CONFIRMATION_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_CONFIRMATION_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_CONFIRMATION_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''RashmiP, issue 6781
        Public Overridable ReadOnly Property BillMeLaterDisplayText() As String
            Get
                If Not ViewState.Item(ATTRIBUTE_BILL_ME_LATER) Is Nothing Then
                    Return CStr(ViewState.Item(ATTRIBUTE_BILL_ME_LATER))
                Else
                    Dim value As String = Me.GetGlobalAttributeValue(ATTRIBUTE_BILL_ME_LATER)
                    If Not String.IsNullOrEmpty(value) Then
                        ViewState.Item(ATTRIBUTE_BILL_ME_LATER) = value
                    End If
                    Return value
                End If
            End Get
        End Property



#End Region
        '<%--Nalini Issue#12578--%>
        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(BillingChangeImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                BillingChangeImage = Me.GetLinkValueFromXML(ATTRIBUTE_BILLING_CHANGE_IMAGE_URL)
            End If
            If String.IsNullOrEmpty(BillingChangePage) Then
                'since value is the 'default' check the XML file for possible custom setting
                BillingChangePage = Me.GetLinkValueFromXML(ATTRIBUTE_BILLING_CHANGE_PAGE)
                'Me.lnkChangeAddress.ImageUrl = BillingChangeImage
                If String.IsNullOrEmpty(BillingChangePage) Then
                    Me.lnkChangeAddress.Enabled = False
                    Me.lnkChangeAddress.ToolTip = "BillingChangePage property has not been set."
                Else
                    Me.lnkChangeAddress.PostBackUrl = BillingChangePage & "?Type=Billing&ReturnToPage=" & Request.Path
                End If
            Else
                Me.lnkChangeAddress.PostBackUrl = BillingChangePage & "?Type=Billing&ReturnToPage=" & Request.Path
            End If
            If String.IsNullOrEmpty(BackButtonPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                BackButtonPage = Me.GetLinkValueFromXML(ATTRIBUTE_BACK_BUTTON_PAGE)
                If String.IsNullOrEmpty(BackButtonPage) Then
                    Me.cmdBack.Enabled = False
                    Me.cmdBack.ToolTip = "BackButtonPage property has not been set."
                End If
            End If
            If String.IsNullOrEmpty(OrderConfirmationPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                OrderConfirmationPage = Me.GetLinkValueFromXML(ATTRIBUTE_CONFIRMATION_PAGE)
                If String.IsNullOrEmpty(OrderConfirmationPage) Then
                    Me.cmdPlaceOrder.Enabled = False
                    Me.cmdPlaceOrder.ToolTip = "OrderConfirmationPage property has not been set."
                End If
            End If


        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            Dim grdTempCart As RadGrid
            SetProperties()
            If Not IsPostBack Then
                RefreshGrid()
                LoadControls()
                'Added by sandeep for issue 15423 on 12/4/2013
                grdTempCart = TryCast(CartGrid2.FindControl("grdMain"), RadGrid)
                grdTempCart.MasterTableView.GetColumn("AutoRenew").Visible = False

            End If
        End Sub

        Private Sub CheckOrderDetails(ByVal oOrder As AptifyGenericEntityBase)
            Try
                If oOrder.SubTypes("OrderLines").Count > 0 Then
                    If Not String.IsNullOrEmpty(OrderConfirmationPage) Then
                        cmdPlaceOrder.Enabled = True
                        'HP Issue#8972: based on the grandTotal of all orderlines toggle validators requiring credit card information
                        SetCreditCardValidators(ShoppingCart1.GrandTotal)
                    End If
                    tblRowMain.Visible = True
                    lblNoItems.Visible = False
                    lblGotItems.Visible = True
                Else
                    cmdPlaceOrder.Enabled = False
                    tblRowMain.Visible = False
                    lblNoItems.Visible = True
                    lblGotItems.Visible = False
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub LoadControls()
            Dim oOrder As AptifyGenericEntityBase

            Try
                oOrder = ShoppingCart1.GetOrderObject(Session, Page.User, Application)
                CheckOrderDetails(oOrder)
                LoadCreditCardInfo(oOrder)
                LoadBillAddress(oOrder)
                LoadShipmentType(oOrder)
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub LoadDefaultAddress(ByVal sPrefix As String, _
                               ByRef oOrder As AptifyGenericEntityBase)
            Try
                With User1
                    oOrder.SetValue(sPrefix & "AddrLine1", .GetValue("AddressLine1"))
                    oOrder.SetValue(sPrefix & "AddrLine2", .GetValue("AddressLine2"))
                    oOrder.SetValue(sPrefix & "AddrLine3", .GetValue("AddressLine3"))
                    oOrder.SetValue(sPrefix & "City", .GetValue("City"))
                    oOrder.SetValue(sPrefix & "State", .GetValue("State"))
                    oOrder.SetValue(sPrefix & "ZipCode", .GetValue("ZipCode"))
                    oOrder.SetValue(sPrefix & "Country", .GetValue("Country"))
                    oOrder.SetValue(sPrefix & "AreaCode", .GetValue("PhoneAreaCode"))
                    oOrder.SetValue(sPrefix & "Phone", .GetValue("Phone"))
                End With
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub LoadDefaultAddress(ByVal sPrefix As String, _
                                     ByRef oOrder As AptifyGenericEntityBase, _
                                     ByVal PrefAddress As String)
            Try
                With User1
                    Select Case PrefAddress
                        Case "Home"
                            oOrder.SetValue(sPrefix & "AddrLine1", .GetValue("HomeAddressLine1"))
                            oOrder.SetValue(sPrefix & "AddrLine2", .GetValue("HomeAddressLine2"))
                            oOrder.SetValue(sPrefix & "AddrLine3", .GetValue("HomeAddressLine3"))
                            oOrder.SetValue(sPrefix & "City", .GetValue("HomeCity"))
                            oOrder.SetValue(sPrefix & "State", .GetValue("HomeState"))
                            oOrder.SetValue(sPrefix & "ZipCode", .GetValue("HomeZipCode"))
                            oOrder.SetValue(sPrefix & "Country", .GetValue("HomeCountry"))
                        Case "Business"
                            oOrder.SetValue(sPrefix & "AddrLine1", .GetValue("AddressLine1"))
                            oOrder.SetValue(sPrefix & "AddrLine2", .GetValue("AddressLine2"))
                            oOrder.SetValue(sPrefix & "AddrLine3", .GetValue("AddressLine3"))
                            oOrder.SetValue(sPrefix & "City", .GetValue("City"))
                            oOrder.SetValue(sPrefix & "State", .GetValue("State"))
                            oOrder.SetValue(sPrefix & "ZipCode", .GetValue("ZipCode"))
                            oOrder.SetValue(sPrefix & "Country", .GetValue("Country"))
                        Case "Billing"
                            oOrder.SetValue(sPrefix & "AddrLine1", .GetValue("BillingAddressLine1"))
                            oOrder.SetValue(sPrefix & "AddrLine2", .GetValue("BillingAddressLine2"))
                            oOrder.SetValue(sPrefix & "AddrLine3", .GetValue("BillingAddressLine3"))
                            oOrder.SetValue(sPrefix & "City", .GetValue("BillingCity"))
                            oOrder.SetValue(sPrefix & "State", .GetValue("BillingState"))
                            oOrder.SetValue(sPrefix & "ZipCode", .GetValue("BillingZipCode"))
                            oOrder.SetValue(sPrefix & "Country", .GetValue("BillingCountry"))
                        Case "POBox"
                            oOrder.SetValue(sPrefix & "AddrLine1", .GetValue("POBox"))
                            oOrder.SetValue(sPrefix & "AddrLine2", .GetValue("POBoxAddressLine2"))
                            oOrder.SetValue(sPrefix & "AddrLine3", .GetValue("POBoxAddressLine3"))
                            oOrder.SetValue(sPrefix & "City", .GetValue("POBoxCity"))
                            oOrder.SetValue(sPrefix & "State", .GetValue("POBoxState"))
                            oOrder.SetValue(sPrefix & "ZipCode", .GetValue("POBoxZipCode"))
                            oOrder.SetValue(sPrefix & "Country", .GetValue("POBoxCountry"))
                        Case Else
                            oOrder.SetValue(sPrefix & "AddrLine1", .GetValue("AddressLine1"))
                            oOrder.SetValue(sPrefix & "AddrLine2", .GetValue("AddressLine2"))
                            oOrder.SetValue(sPrefix & "AddrLine3", .GetValue("AddressLine3"))
                            oOrder.SetValue(sPrefix & "City", .GetValue("City"))
                            oOrder.SetValue(sPrefix & "State", .GetValue("State"))
                            oOrder.SetValue(sPrefix & "ZipCode", .GetValue("ZipCode"))
                            oOrder.SetValue(sPrefix & "Country", .GetValue("Country"))
                    End Select
                    oOrder.SetValue(sPrefix & "AreaCode", .GetValue("PhoneAreaCode"))
                    oOrder.SetValue(sPrefix & "Phone", .GetValue("Phone"))
                End With
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub LoadBillAddress(ByVal oOrder As AptifyGenericEntityBase)
            Try
                ''Dim oOrder As AptifyGenericEntityBase
                ''oOrder = CartGrid2.Cart.GetOrderObject(Session, Page.User, Application)
                '02/05/08 RJK - Added back the check to see if the Ship To and Bill To Addresses have
                'been set.  Otherwise, it is not possible to change the address to a non-default Address.
                'For the 5583 Issue related to User Profile changes, the Profile page has been updated to refresh
                'the Order's Ship To And Bill To Addresses if the Address is changed on the Profile.

                '//Vijay Sitlani - Changes made to partially resolve the bug reported by Alina for Issue 5583
                ' Changes made on 01-25-2008
                If Len(oOrder.GetValue("ShipToAddrLine1")) = 0 Then
                    'LoadDefaultAddress("ShipTo", oOrder)
                    LoadDefaultAddress("ShipTo", oOrder, User1.GetValue("PreferredShippingAddress"))

                End If
                ' Vijay Sitlani - Changes made to partially resolve the bug reported by Alina for Issue 5583
                ' Changes made on 01-25-2008
                If Len(oOrder.GetValue("BillToAddrLine1")) = 0 Then
                    'LoadDefaultAddress("BillTo", oOrder)
                    LoadDefaultAddress("BillTo", oOrder, User1.GetValue("PreferredBillingAddress"))
                End If
                With User1
                    NameAddressBlock.Name = .FirstName & " " & .LastName
                    If Len(.Company) > 0 Then
                        NameAddressBlock.Name = NameAddressBlock.Name & "/" & .Company
                    End If
                End With
                With oOrder
                    NameAddressBlock.AddressLine1 = CStr(.GetValue("BillToAddrLine1"))
                    NameAddressBlock.AddressLine2 = CStr(.GetValue("BillToAddrLine2"))
                    NameAddressBlock.AddressLine3 = CStr(.GetValue("BillToAddrLine3"))
                    NameAddressBlock.City = CStr(.GetValue("BillToCity"))
                    NameAddressBlock.State = CStr(.GetValue("BillToState"))
                    NameAddressBlock.ZipCode = CStr(.GetValue("BillToZipCode"))
                    NameAddressBlock.Country = CStr(.GetValue("BillToCountry"))
                End With

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Private Sub LoadCreditCardInfo(ByVal oOrder As AptifyGenericEntityBase)

            Try
                'RashmiP, Issue 6781, 09/20/10, to show bill me later option button in credit card uc
                ShowBillMeLater(oOrder)
                CreditCard.LoadCreditCardInfo()
                CreditCard.PaymentTypeID = CLng(oOrder.GetValue("PayTypeID"))
                CreditCard.CCNumber = CStr(oOrder.GetValue("CCAccountNumber"))
                CreditCard.CCExpireDate = CStr(oOrder.GetValue("CCExpireDate"))
                If CBool(oOrder.GetValue("x_CompanyOrder")) = True Then
                    CreditCard.IndividualPayment = False
                    CreditCard.CompanyPayment = True
                Else
                    CreditCard.CompanyPayment = False
                    CreditCard.IndividualPayment = True
                End If
                ' Changes made to add Credit Card security number feature on e-business site.
                ' Change made by Vijay Sitlani for Issue 5369
                ' CreditCard1.CCSecurityNumber = CStr(oOrder.GetValue("CCSecurityNumber"))
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            Finally

            End Try
        End Sub

        Private Sub RefreshGrid()
            Try
                CartGrid2.RefreshGrid()
                'Navin Prasad Issue 11032
                If CartGrid2.Grid.Items.Count > 0 Then
                    'cmdUpdateCart.Visible = True
                    If Not String.IsNullOrEmpty(OrderConfirmationPage) Then
                        cmdPlaceOrder.Enabled = True
                    End If
                    tblRowMain.Visible = True
                    lblNoItems.Visible = False
                Else
                    'cmdUpdateCart.Visible = False
                    cmdPlaceOrder.Enabled = False
                    tblRowMain.Visible = False
                    lblNoItems.Visible = True
                End If
                With CartGrid2.Cart
                    Me.OrderSummary.Refresh()
                End With
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Private Sub cmdPlaceOrder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPlaceOrder.Click
            Dim lOrderID As Long
            Dim sError As String = ""
            'Dim Status As String = ""
            Dim oOrders As OrdersEntity
            Dim RecipientID As Integer
            Dim arrlstRecipientID As New ArrayList
            Dim sendRenewalMail As Boolean = False
            Dim strSubscriberID As String = ""
            Dim strRenewalStatus As String = ""
            Dim dt As DataTable = Nothing
            Dim sSql As String
            Dim iShipID As Integer = 0
            Dim oShipmentTypes As New Aptify.Framework.Web.eBusiness.CommonMethods(DataAction, AptifyApplication, User1, Database)
            Try
                oOrders = ShoppingCart1.GetOrderObject(Session, Page.User, Application)
                'Added by Sandeep for issue 5133 on 07/03/2013
                'if Shippment type is not available for current country for Ship to Address then set Default shipping type
                dt = oShipmentTypes.LoadShipmentType(CInt(oOrders.GetValue("ShipToCountryCodeID")))
                If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                    sSql = "SELECT Top 1 ID FROM " & AptifyApplication.GetEntityBaseDatabase("Shipment Types") & ".." & AptifyApplication.GetEntityBaseView("Shipment Types")
                    iShipID = Convert.ToInt32(DataAction.ExecuteScalar(sSql))
                    If iShipID > 0 Then
                        oOrders.ShipTypeID = iShipID
                    End If
                End If
                With oOrders
                    'Send Notification mail for Membership/Subscription Renewal
                    'Add RecipientID to arrlstRecipientID and pass to SendMailToRecipient() 
                    ''RashmiP, Issue 14956, Reset Order object if Order is for Renew Membership or Subscription
                    'Suraj Issue 16516  we remove the Session("SubscriptionOrder") from renew subscription and renew membership control because while we are changing
                    'the session mode to state server so it will throuth the serialization problem  also we remove the _xRenewalStatus temp fiels from order obj so insteade of this we are 
                    'using the Comments field
                    If ShoppingCart1.GetOrderObject(Me.Session, Me.Page.User, Me.Application) IsNot Nothing Then
                        oOrders = ResetOrderObject(oOrders)
                    End If

                    If oOrders.SubTypes("OrderLines") IsNot Nothing AndAlso oOrders.SubTypes("OrderLines").Count > 0 Then

                        For i As Integer = 0 To oOrders.SubTypes("OrderLines").Count - 1
                            If oOrders.SubTypes("OrderLines").Item(i).GetValue("SubscriberID") IsNot Nothing AndAlso oOrders.SubTypes("OrderLines").Item(i).GetValue("Comments") IsNot Nothing Then
                                strSubscriberID = oOrders.SubTypes("OrderLines").Item(i).GetValue("SubscriberID").ToString().Trim
                                strRenewalStatus = oOrders.SubTypes("OrderLines").Item(i).GetValue("Comments").ToString().Trim

                                'ToDo Check for Renewal Condition 
                                If IsNumeric(strSubscriberID) = True AndAlso CInt(strSubscriberID) > 0 AndAlso (strRenewalStatus.ToUpper() = "RENEWMEMBERSHIP" OrElse strRenewalStatus.ToUpper() = "RENEWSUBSCRIPTION") Then
                                    arrlstRecipientID.Add(CInt(strSubscriberID))
                                    sendRenewalMail = True
                                End If
                            End If
                        Next
                    End If

                    'RashmiP issue 6781, 09/20/10
                    If CreditCard.BillMeLaterChecked Then
                        If String.IsNullOrEmpty(CreditCard.PONumber) Then
                            .SetValue("PONumber", BillMeLaterDisplayText)
                        Else
                            .SetValue("PONumber", CreditCard.PONumber)
                        End If
                        .SetValue("PayTypeID", CreditCard.PaymentTypeID)
                        ShoppingCart1.SaveCart(Session)
                        lOrderID = ShoppingCart1.PlaceOrder(Session, Application, Page.User, sError)
                    Else
                        Page.Validate()
                        .SetValue("PayTypeID", CreditCard.PaymentTypeID)
                        .SetValue("CCAccountNumber", CreditCard.CCNumber)
                        .SetValue("CCExpireDate", CreditCard.CCExpireDate)
                        'Anil B change for 10737 on 13/03/2013
                        'Add condition if payment type is transaction
                        If CreditCard.CCNumber = "-Ref Transaction-" Then
                            .SetValue("ReferenceTransactionNumber", CreditCard.ReferenceTransactionNumber)
                            .SetValue("ReferenceExpiration", CreditCard.ReferenceExpiration)
                        End If
                        If Len(CreditCard.CCSecurityNumber) > 0 Then
                            .SetAddValue("_xCCSecurityNumber", CreditCard.CCSecurityNumber) 'Neha changes for Issue 16675, 06/05/2013,Added CCSecurityNumber as a temperory field for not storing in record history.
                        End If
                        If oOrders.Fields("PaymentInformationID").EmbeddedObjectExists Then
                            Dim oOrderPayInfo As PaymentInformation = DirectCast(oOrders.Fields("PaymentInformationID").EmbeddedObject, PaymentInformation)
                            oOrderPayInfo.CreditCardSecurityNumber = CreditCard.CCSecurityNumber
                            ''RashmiP, Issue 10254, SPM
                            oOrderPayInfo.SetValue("SaveForFutureUse", CreditCard.SaveCardforFutureUse)
                            'Ansar Shaikh - Issue 11986 - 12/27/2011
                            'Ani B for issue 10254 on 22/04/2013
                            'Set CC Partial for credit cart type is reference transaction 
                            If CreditCard.CCNumber = "-Ref Transaction-" Then
                                oOrderPayInfo.SetValue("CCPartial", CreditCard.CCPartial)
                            End If
                        End If

                        If (CreditCard.EnablePaymentTypeSelection = True AndAlso CType(CreditCard.FindControl("rbCompanyPayment"), RadioButton).Checked = True) OrElse (CreditCard.EnablePaymentTypeSelection = False AndAlso (Convert.ToBoolean(oOrders.GetValue("x_CompanyOrder")) = True) OrElse Convert.ToString(Session("ShippingOrderType")) = "COMPANY") Then
                            oOrders.SetAddValue("x_CompanyPayment", True)
                        Else
                            oOrders.SetAddValue("x_CompanyPayment", False)
                        End If

                        ShoppingCart1.SaveCart(Session)
                        lOrderID = ShoppingCart1.PlaceOrder(Session, Application, Page.User, sError)
                    End If
                End With

                If lOrderID > 0 Then

                    If sendRenewalMail = True Then
                        For i = 0 To CInt(arrlstRecipientID.Count) - 1
                            RecipientID = CInt(arrlstRecipientID.Item(i))
                            SendMailToRecipient(RecipientID, strRenewalStatus)
                        Next
                        'Suraj Issue 16516  we remove the Session("SubscriptionOrder") 
                    End If

                    ''RashmiP, Issue 14326, 11/5/12
                    'Amruta,Issue 14381, 5/8/2013 ,Commented function as we are not going to send meeting registration mail automatically to attendees
                    'SendMeetingRegistrationMail(lOrderID)
                    Response.Redirect(OrderConfirmationPage & "?ID=" & lOrderID, False)
                    lblError.Visible = False
                Else
                    OrderFailed(sError)
                End If
                ''reset the Seesion values on completion of the order placement
                Session("ShippingOrderType") = Nothing
                Session("BillingOrderType") = Nothing

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        '''RashmiP, Issue 14956, Reset Order object if Order is for Renew Membership or Subscription
        Private Function ResetOrderObject(ByVal oOrders As OrdersEntity) As OrdersEntity
            'Suraj Issue 16516  we remove the Session("SubscriptionOrder") from renew subscription and renew membership control because while we are changing
            'the session mode to state server so it will throuth the serialization problem also we remove the _xRenewalStatus temp fiels from order obj so insteade of this we are 
            'using the Comments field
            Dim oOrderSubscription As AptifyGenericEntityBase = ShoppingCart1.GetOrderObject(Me.Session, Me.Page.User, Me.Application)
            Dim strSubscriberID1 As String = ""
            Dim strProductID1 As String = ""

            Dim strSubscriberID2 As String = ""
            Dim strProductID2 As String = ""
            Try
                If oOrders.SubTypes("OrderLines") IsNot Nothing AndAlso oOrderSubscription IsNot Nothing Then
                    For i As Integer = 0 To oOrders.SubTypes("OrderLines").Count - 1
                        If oOrders.SubTypes("OrderLines").Item(i).GetValue("Comments") IsNot Nothing AndAlso oOrders.SubTypes("OrderLines").Item(i).GetValue("SubscriberID") IsNot Nothing Then

                            strSubscriberID1 = oOrders.SubTypes("OrderLines").Item(i).GetValue("SubscriberID").ToString().Trim
                            strProductID1 = oOrders.SubTypes("OrderLines").Item(i).GetValue("ProductID").ToString().Trim

                            For j As Integer = 0 To oOrderSubscription.SubTypes("OrderLines").Count - 1

                                If oOrderSubscription.SubTypes("OrderLines").Item(i).GetValue("SubscriberID") IsNot Nothing Then

                                    strSubscriberID2 = oOrderSubscription.SubTypes("OrderLines").Item(j).GetValue("SubscriberID").ToString().Trim
                                    strProductID2 = oOrderSubscription.SubTypes("OrderLines").Item(j).GetValue("ProductID").ToString().Trim

                                    If strSubscriberID1 = strSubscriberID2 And strProductID1 = strProductID2 Then
                                        ''RashmiP, If Quotation order exist for any subscription/membership then cancle that order.
                                        Dim ExistingOrderID As Long = CLng(oOrderSubscription.SubTypes("OrderLines").Item(j).GetValue("_xExistingOrderID"))
                                        If ExistingOrderID > 0 Then
                                            CancelSubscriptionOrder(ExistingOrderID)
                                        End If
                                        Exit For
                                    End If
                                End If
                            Next
                        End If
                    Next
                End If
                Return oOrders
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
            Return Nothing
        End Function
        Private Function GetMessageTemplateID(ByVal sTemplateName As String) As Integer
            Try
                Dim sSql As String
                Dim dt As DataTable
                sSql = "Select ID from vwMessageTemplates where name  = '" & sTemplateName & "'"
                dt = DataAction.GetDataTable(sSql)
                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                    Return CInt(dt.Rows(0).Item("ID"))
                End If
                Return -1
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return -1
            End Try
        End Function

        Public Sub SendMailToRecipient(ByVal RecipientID As Integer, ByVal RenewalType As String)
            Try
                'Get the Process Flow ID to be used for sending the Order Confirmation Email
                Dim sSQL As String = "SELECT ID FROM " & Database & "..vwProcessFlows WHERE Name='Send Renew Subscriptions Notification Email'"
                Dim lProcessFlowID As Long = CLng(DataAction.ExecuteScalar(sSQL, IAptifyDataAction.DSLCacheSetting.UseCache))
                Dim lMessageTemplateID As Long
                Dim context As New AptifyContext
                ''Rashmi P, Issue 14956, select Message template according to product Type.
                If RenewalType = "RENEWMEMBERSHIP" Then
                    lMessageTemplateID = GetMessageTemplateID("Membership Renewed by Admin")
                Else
                    lMessageTemplateID = GetMessageTemplateID("Renew Subscriptions Notification")
                End If


                If lMessageTemplateID <= 0 Then
                    lblError.Text = "Message Template does not exist."
                Else
                    context.Properties.AddProperty("MessageTemplateID", lMessageTemplateID)
                    context.Properties.AddProperty("PersonID", RecipientID)

                    Dim result As ProcessFlowResult
                    result = ProcessFlowEngine.ExecuteProcessFlow(Me.AptifyApplication, lProcessFlowID, context)
                    If result.IsSuccess Then
                        lblError.ForeColor = Drawing.Color.Blue
                        lblError.Text = "Notification sent to the members about their membership renewal."
                    Else
                        lblError.ForeColor = Drawing.Color.Red
                        lblError.Text = "Email Notification failed. Contact Customer Service for help."
                    End If
                End If


            Catch ex As ArgumentException
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try



        End Sub
        'HP Issue#8972: based on the grandTotal of all orderlines toggle validators requiring credit card information
        Private Sub SetCreditCardValidators(ByVal totalDue As Decimal)
            With CreditCard
                If totalDue > 0 Then
                    .CCNumberValidatorSetting = True
                    .CCSecurityNumberValidatorSetting = True
                Else
                    .CCNumberValidatorSetting = False
                    .CCSecurityNumberValidatorSetting = False
                End If
            End With
        End Sub

        Private Sub OrderFailed(ByVal sError As String)
            lblError.Text = "<b>Order Failed To Save</b><BR /><hr />" & sError
            lblError.Visible = True
            Me.tblRowMain.Visible = False
            Me.cmdPlaceOrder.Visible = False
            lblGotItems.Visible = False
            lblNoItems.Visible = False
            Me.OrderSummary.Visible = False
        End Sub
        'Navin Prasad Issue 9388
        Private Sub ProductDownloadFailed(ByVal sError As String)
            lblError.Text = "<b>Failed To Create Downloable Product Info</b><BR /><hr />" & sError
            lblError.Visible = True
        End Sub
        Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBack.Click
            Response.Redirect(BackButtonPage)
        End Sub

        ''' <summary>
        ''' RashmiP issue 6781, 09/20/10
        ''' procedure set properties of credit card, if Company and User's credit Status is approved and credit limit is availabe 
        ''' contion check if payment type is Bill Me Later. 
        ''' </summary>
        Private Sub ShowBillMeLater(ByVal a_oOrder As AptifyGenericEntityBase)
            Dim iPrevPaymentTypeID As Integer
            Dim iPOPaymentType As Integer = 0
            Dim sError As String
            Dim oOrder As Aptify.Applications.OrderEntry.OrdersEntity

            Try
                If Not String.IsNullOrEmpty(AptifyApplication.GetEntityAttribute("Web Shopping Carts", "POPaymentTypeID")) Then
                    iPOPaymentType = CInt(AptifyApplication.GetEntityAttribute("Web Shopping Carts", "POPaymentTypeID"))
                End If
                Dim dr As Data.DataRow = User1.CompanyDataRow
                CreditCard.UserCreditStatus = CInt(User1.GetValue("CreditStatusID"))
                CreditCard.UserCreditLimit = CLng(User1.GetValue("CreditLimit"))
                oOrder = CType(a_oOrder, OrdersEntity)
                If iPOPaymentType > 0 Then
                    If dr IsNot Nothing Then
                        CreditCard.CompanyCreditStatus = CInt(dr.Item("CreditStatusID"))
                        CreditCard.CompanyCreditLimit = CLng(dr.Item("CreditLimit"))
                    End If
                    If oOrder IsNot Nothing Then
                        iPrevPaymentTypeID = CInt(oOrder.GetValue("PayTypeID"))
                        oOrder.SetValue("PayTypeID", iPOPaymentType)
                        CreditCard.CreditCheckLimit = ShoppingCart1.CreditCheckObject.CheckCredit(CType(oOrder, Aptify.Applications.OrderEntry.OrdersEntity), sError)
                    End If
                End If
                oOrder.SubTypes("OrderLines").Item(0).GetValue("ParentSequence")
            Catch ex As Exception

            Finally
                oOrder.SetValue("PayTypeID", iPrevPaymentTypeID)
            End Try

        End Sub

        '''RashmiP, Issue 14326, Send Meeting Registration Mails to all the attendees
        ''' 11/5/12

        'Amruta , Issue 14381,5/8/2013 ,Commented function as we’re not going to send automatically any messages for meeting registrations.

        'Use this function to send an email automatically to every person who is registered for a meeting on this order. 
        'By default, this function is not called within this control. It is provided here as an example of how you can trigger a process flow to send an email to registrants as part of the meeting registration process. 
        'If you decide to send an email using this function, you should create a new process flow that sends an email based on a particular message template that your organization wants to use and identify that process flow in the function below.

        'Private Function SendMeetingRegistrationMail(ByVal lOrderID As Long) As Boolean
        '    Try
        '        Dim sSql, sEmail, sProductType As String, bSucess As Boolean
        '        Dim dt As DataTable
        '        'Amruta Issue 14381 ,18/4/2013,Query changes to send single mail to attendees
        '        sSql = "Select Distinct AttendeeID_Email, AttendeeID_FirstLast ,ProductType from " & Database & "..vwOrderMeetDetail vw inner join " & Database & "..vwProducts p on vw.ProductID= p.ID where OrderID = " & lOrderID

        '        dt = DataAction.GetDataTable(sSql, IAptifyDataAction.DSLCacheSetting.BypassCache)

        '        If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
        '            For Each rw As DataRow In dt.Rows
        '                sProductType = CStr(rw.Item("ProductType"))
        '                If sProductType = "Meeting" Then
        '                    sEmail = CStr(rw.Item("AttendeeID_Email"))

        '                    sSql = "SELECT ID FROM " & Database & "..vwProcessFlows WHERE Name='Send eBusiness Order Confirmation Email'"
        '                    Dim lProcessFlowID As Long = CLng(DataAction.ExecuteScalar(sSql, IAptifyDataAction.DSLCacheSetting.UseCache))

        '                    Dim context As New AptifyContext
        '                    context.Properties.AddProperty("OrderID", lOrderID)
        '                    context.Properties.AddProperty("EmailAddresses", sEmail)
        '                    context.Properties.AddProperty("MessageSystem", "DOT NET MAIL")
        '                    Dim result As ProcessFlowResult
        '                    result = ProcessFlowEngine.ExecuteProcessFlow(Me.AptifyApplication, lProcessFlowID, context)
        '                    If result.IsSuccess Then
        '                        bSucess = True
        '                    Else
        '                        bSucess = False
        '                    End If

        '                End If
        '            Next
        '        End If
        '    Catch ex As Exception

        '    End Try
        'End Function

        ''' <summary>
        ''' Rashmi P, Issue 5133, 12/6/12 Add ShipmentType Selection.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub LoadShipmentType(ByVal a_oOrder As AptifyGenericEntityBase)
            Dim oOrder As Aptify.Applications.OrderEntry.OrdersEntity
            oOrder = CType(a_oOrder, OrdersEntity)
            ''oOrder = CartGrid2.Cart.GetOrderObject(Page.Session, Page.User, Page.Application)
            Dim oShipmentTypes As New Aptify.Framework.Web.eBusiness.CommonMethods(DataAction, AptifyApplication, User1, Database)
            Dim dt As DataTable = Nothing
            Dim bIncludeInShipping As Boolean
            Try
                If oOrder IsNot Nothing Then
                    For Each oOrderLine As OrderLinesEntity In oOrder.SubTypes("OrderLines")
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
                If ddlShipmentType.Items.Count > 0 Then
                    If Not oOrder Is Nothing Then
                        ddlShipmentType.SelectedValue = CStr(oOrder.ShipTypeID)
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
                oOrder = CartGrid2.Cart.GetOrderObject(Session, Page.User, Page.Application)
                oOrder.SetValue("ShipTypeID", ddlShipmentType.SelectedValue)

                oOrder.CalculateOrderTotals(True, True)
                CartGrid2.Cart.SaveCart(Session)
                RefreshGrid()
            End If
        End Sub
        ''' <summary>
        ''' Rashmi P, Issue 14318, Renew MemberShip/Subscription
        ''' Function handle the existing quotation order if one exists tied to that recipient/product , set the order status to Cancelled
        ''' Otherwise, the quotation order will show up in the Pay Off Orders grid and if the admin pays off the quotation order, 
        ''' then the admin will inadvertently renew the subscription/membership twice.
        ''' </summary>
        ''' <param name="OrderID"></param>
        ''' <remarks></remarks>
        Private Sub CancelSubscriptionOrder(ByVal OrderID As Long)
            Try
                Dim oOrderGe As AptifyGenericEntityBase
                oOrderGe = Me.AptifyApplication.GetEntityObject("Orders", OrderID)
                oOrderGe.SetValue("OrderStatusID", OrderStatus.Cancelled)
                oOrderGe.Save(False)
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
    End Class
End Namespace
