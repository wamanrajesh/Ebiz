'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On


Imports Aptify.Framework.Application
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.BusinessLogic.Security
Imports Aptify.Framework.BusinessLogic.ProcessPipeline
Imports System.Data
Imports System.Collections
Imports Telerik.Web.UI

Namespace Aptify.Framework.Web.eBusiness.CustomerService
    Partial Class OrderConfirmationControl
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_COMPANY_LOGO_IMAGE_URL As String = "CompanyLogoImage"
        Protected Const ATTRIBUTE_COMPANY_ADDRESS As String = "CompanyAddress"
        Protected Const ATTRIBUTE_PRODUCT_PAGE As String = "ProductURL"
        Protected Const ATTRIBUTE_CLASS_VIEW_PAGE As String = "ClassViewURL"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "OrderConfirmation"
        Protected Const ATTRIBUTE_MESSAGESYSTEM As String = "MessageSystem"
        'Issue 15215 Sachin 02/27/2013
        Protected Const ATTRIBUTE_SECURITYERROR_PAGE As String = "securityErrorPage"
        Private m_lCustomerID As Long 'used to query Customer's Email
        Dim sCurrencyFormat As String
        Dim CurrencyCache As CurrencyTypeCache
        Dim arPrice As ArrayList
        Dim arExtended As ArrayList

#Region "OrderConfirmation Specific Properties"
        Public Overridable ReadOnly Property MessageSystem() As String
            Get
                If Not ViewState.Item(ATTRIBUTE_MESSAGESYSTEM) Is Nothing Then
                    Return CStr(ViewState.Item(ATTRIBUTE_MESSAGESYSTEM))
                Else
                    Dim value As String = Me.GetGlobalAttributeValue(ATTRIBUTE_MESSAGESYSTEM)
                    If Not String.IsNullOrEmpty(value) Then
                        ViewState.Item(ATTRIBUTE_MESSAGESYSTEM) = value
                        Return value
                    Else
                        Return ""
                    End If
                End If
            End Get
        End Property
        ''' <summary>
        ''' CompanyLogoImage url
        ''' </summary>
        Public Overridable Property CompanyLogoImage() As String
            Get
                If Not ViewState(ATTRIBUTE_COMPANY_LOGO_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_COMPANY_LOGO_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_COMPANY_LOGO_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' RashmiP, Issue 9974, 09/14/10
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Property CompanyAddress() As String
            Get
                If Not ViewState(ATTRIBUTE_COMPANY_ADDRESS) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_COMPANY_ADDRESS))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_COMPANY_ADDRESS) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' Product page url
        ''' </summary>
        Public Overridable Property ProductURL() As String
            Get
                If Not ViewState(ATTRIBUTE_PRODUCT_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_PRODUCT_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_PRODUCT_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' ClassView page url
        ''' </summary>
        Public Overridable Property ClassViewURL() As String
            Get
                If Not ViewState(ATTRIBUTE_CLASS_VIEW_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_CLASS_VIEW_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_CLASS_VIEW_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ' Issue 15215 02/27/2013 by Sachin
        ''' <summary>
        ''' Security Error Page page url
        ''' </summary>
        Public Overridable ReadOnly Property SecurityErrorPage() As String
            Get
                If Not ViewState(ATTRIBUTE_SECURITYERROR_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_SECURITYERROR_PAGE))
                Else
                    Dim value As String = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings("SecurityErrorPageURL"))
                    If Not String.IsNullOrEmpty(value) Then
                        ViewState(ATTRIBUTE_SECURITYERROR_PAGE) = value
                        Return value
                    Else
                        Return String.Empty
                    End If
                End If
            End Get
        End Property
#End Region
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
            'Put user code to initialize the page here
            If Not IsPostBack Then
                LoadOrder()
            End If
        End Sub

        Protected Overrides Sub SetProperties()
            Try
                If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
                'call base method to set parent properties
                MyBase.SetProperties()

                If String.IsNullOrEmpty(ProductURL) Or String.IsNullOrEmpty(ClassViewURL) Then
                    ProductURL = Me.GetLinkValueFromXML(ATTRIBUTE_PRODUCT_PAGE)
                    ClassViewURL = Me.GetLinkValueFromXML(ATTRIBUTE_CLASS_VIEW_PAGE)
                    If String.IsNullOrEmpty(ProductURL) Or String.IsNullOrEmpty(ClassViewURL) Then
                        Me.grdMain.Enabled = False
                        Me.grdMain.ToolTip = "ProductURL and/or ClassViewURL property has not been set."
                    End If
                End If

                If String.IsNullOrEmpty(CompanyLogoImage) Then
                    CompanyLogoImage = Me.GetLinkValueFromXML(ATTRIBUTE_COMPANY_LOGO_IMAGE_URL)
                    Me.companyLogo.Src = CompanyLogoImage
                End If
                'RashmiP Issue 9974, 09/14/10 
                'Company address abstracted, moved in Navigation File.
                If String.IsNullOrEmpty(CompanyAddress) Then
                    Dim strVirtualPath As String = Request.ApplicationPath.ToString & "/"
                    CompanyAddress = Me.GetLinkValueFromXML(ATTRIBUTE_COMPANY_ADDRESS)
                    Me.lblcompanyAddress.Text = CompanyAddress.Substring(strVirtualPath.Length, CompanyAddress.Length - strVirtualPath.Length)
                End If
            Catch ex As Exception
            End Try
        End Sub
        Private Sub LoadOrder()
            If Not IsNumeric(Request.QueryString("ID")) Then
                If Request.QueryString("ID") IsNot Nothing Then
                    Throw New ArgumentException("Parameter must be numeric.", "ID")
                End If
            Else
                'Issue 15215 02/27/2013 by the Sachin
                If DoesUserHaveAccessOnOrder(User1.PersonID, Convert.ToInt32(Request.QueryString("ID"))) Then
                    LoadOrderHeader()
                    LoadOrderDetails()
                    LoadEmailOrder()
                Else
                    Response.Redirect(System.Configuration.ConfigurationManager.AppSettings("virtualdir") & SecurityErrorPage & "?Message=Access to this Order is unauthorized.")
                End If
            End If
        End Sub
        Private Sub LoadOrderHeader()
            Dim sSQL As String, dt As Data.DataTable

            Try
                If Not IsNumeric(Request.QueryString("ID")) Then
                    Throw New ArgumentException("Parameter must be numeric.", "ID")
                End If
                sSQL = "SELECT o.*,ost.Name, OrderStatus, o.OrderParty, ppd.PaymentParty FROM " & _
                       Database & _
                       "..vwOrders o INNER JOIN " & _
                       Database & _
                       "..vwOrderStatusTypes ost ON " & _
                       "o.OrderStatusID=ost.ID " & _
                       " LEFT OUTER JOIN " & Database & "..vwPaymentPartyDetail ppd " & _
                       " ON ppd.OrderID = o.ID " & _
                       " WHERE o.ID = " & Request.QueryString("ID")
                dt = DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)

                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then

                    With dt.Rows(0)

                        If .Item("ID") IsNot Nothing AndAlso Not IsDBNull(.Item("ID")) Then
                            lblOrderID.Text = CStr(.Item("ID"))
                        Else
                            lblOrderID.Text = "-1"
                        End If
                        If .Item("OrderType") IsNot Nothing AndAlso Not IsDBNull(.Item("OrderType")) Then
                            lblOrderType.Text = CStr(.Item("OrderType"))
                        Else
                            lblOrderType.Text = ""
                        End If
                        If .Item("OrderStatus") IsNot Nothing AndAlso Not IsDBNull(.Item("OrderStatus")) Then
                            lblOrderStatus.Text = CStr(.Item("OrderStatus"))
                        Else
                            lblOrderStatus.Text = ""
                        End If
                        If .Item("PayType") IsNot Nothing AndAlso Not IsDBNull(.Item("PayType")) Then
                            lblPayType.Text = CStr(.Item("PayType"))
                        Else
                            lblPayType.Text = ""
                        End If
                        If .Item("BillToID") IsNot Nothing AndAlso Not IsDBNull(.Item("BillToID")) Then
                            lblBillToID.Text = CStr(.Item("BillToID"))
                        Else
                            lblBillToID.Text = "-1"
                        End If
                        If .Item("ShipType") IsNot Nothing AndAlso Not IsDBNull(.Item("ShipType")) Then
                            lblShipType.Text = CStr(.Item("ShipType"))
                        Else
                            lblShipType.Text = ""
                        End If
                        If .Item("ShipTrackingNum") IsNot Nothing AndAlso Not IsDBNull(.Item("ShipTrackingNum")) Then
                            lblShipTrackingNum.Text = CStr(.Item("ShipTrackingNum"))
                        Else
                            lblShipTrackingNum.Text = ""
                        End If
                        If .Item("ShipDate") Is Nothing _
                                OrElse IsDBNull(.Item("ShipDate")) _
                                OrElse Not IsDate(.Item("ShipDate")) _
                                OrElse CDate(.Item("ShipDate")) = DateSerial(1900, 1, 1) Then
                            'The ShipDate is not valid
                            lblShipDate.Text = "Not Shipped"
                        Else
                            lblShipDate.Text = CStr(.Item("ShipDate"))
                        End If

                        ''Display Order Party and Payment Party....................
                        divOrderParty.Visible = False
                        divPaymentParty.Visible = False
                        lblOrderParty.Text = ""
                        lblPaymentParty.Text = ""
                        If .Item("OrderParty") IsNot Nothing AndAlso Not IsDBNull(.Item("OrderParty")) AndAlso _
                            .Item("PaymentParty") IsNot Nothing AndAlso Not IsDBNull(.Item("PaymentParty")) Then
                            If CStr(.Item("OrderParty")) = "Company" AndAlso CStr(.Item("PaymentParty")) = "Individual" OrElse _
                              CStr(.Item("OrderParty")) = "Individual" AndAlso CStr(.Item("PaymentParty")) = "Company" Then
                                divOrderParty.Visible = True
                                divPaymentParty.Visible = True
                                lblOrderParty.Text = CStr(.Item("OrderParty"))
                                lblPaymentParty.Text = CStr(.Item("PaymentParty"))
                            End If
                        End If

                        CurrencyCache = CurrencyTypeCache.Instance(Me.AptifyApplication)

                        If IsNumeric(.Item("CurrencyTypeID")) Then
                            sCurrencyFormat = CurrencyCache.CurrencyType(CInt(.Item("CurrencyTypeID"))).FormatString
                        Else
                            'If not provided in order, user logged in User's Preferred CurrencyTypeID
                            sCurrencyFormat = CurrencyCache.CurrencyType(CInt(Me.User1.PreferredCurrencyTypeID)).FormatString
                        End If

                        If sCurrencyFormat.Length = 0 Then
                            sCurrencyFormat = "Currency"
                        End If

                        lblSubTotal.Text = Format$(.Item("CALC_SubTotal"), sCurrencyFormat)

                        Dim dShippingAndHandlingCharges As Decimal = 0
                        If IsNumeric(.Item("ShipCharge")) Then
                            dShippingAndHandlingCharges = CDec(.Item("ShipCharge"))
                        End If

                        'If IsNumeric(.Item("ShipCharge")) Then
                        If IsNumeric(.Item("CALC_HandlingCharge")) Then
                            dShippingAndHandlingCharges += CDec(.Item("CALC_HandlingCharge"))
                        End If

                        lblShipCharge.Text = Format$(dShippingAndHandlingCharges, sCurrencyFormat)

                        lblSalesTax.Text = Format$(.Item("CALC_SalesTax"), sCurrencyFormat)
                        lblGrandTotal.Text = Format$(.Item("CALC_GrandTotal"), sCurrencyFormat)
                        lblBalance.Text = Format$(.Item("Balance"), sCurrencyFormat)
                        lblPayments.Text = Format$(.Item("CALC_PaymentTotal"), sCurrencyFormat)

                    End With
                    SetAddress(blkShipTo, "ShipTo", dt.Rows(0))
                    SetAddress(blkBillTo, "BillTo", dt.Rows(0))

                    'Set m_CustomerID to be used later
                    If IsNumeric(dt.Rows(0).Item("ShipToID")) Then
                        m_lCustomerID = CLng(dt.Rows(0).Item("ShipToID"))
                    Else
                        m_lCustomerID = -1
                    End If

                Else
                    'Order wasn't found
                End If

            Catch ae As ArgumentException
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ae)
            End Try
        End Sub
        Private Sub SetAddress(ByVal oBlock As NameAddressBlock, _
                               ByVal sPrefix As String, _
                               ByRef dr As System.Data.DataRow)

            With oBlock
                .Name = CStr(dr.Item(sPrefix & "Name"))
                If Not dr.IsNull(sPrefix & "Company") Then
                    .Name = .Name & "/" & CStr(dr.Item(sPrefix & "Company"))
                End If

                'Issue 5053 - Only display customer's address if it is valid/complete
                Dim sLine1 As String = ""
                Dim sLine2 As String = ""
                Dim sLine3 As String = ""
                Dim sCity As String = ""
                Dim sState As String = ""
                Dim sZip As String = ""
                Dim sCountry As String = ""

                'Check if AddressLine1 exists
                If Not dr.IsNull(sPrefix & "AddrLine1") Then
                    sLine1 = CStr(dr.Item(sPrefix & "AddrLine1"))
                End If
                'Check if AddressLine2 exists
                If Not dr.IsNull(sPrefix & "AddrLine2") Then
                    sLine2 = CStr(dr.Item(sPrefix & "AddrLine2"))
                End If
                'Check if AddressLine3 exists
                If Not dr.IsNull(sPrefix & "AddrLine3") Then
                    sLine3 = CStr(dr.Item(sPrefix & "AddrLine3"))
                End If
                'Check if City exists
                If Not dr.IsNull(sPrefix & "City") Then
                    sCity = CStr(dr.Item(sPrefix & "City"))
                End If
                'Check if State exists
                If Not dr.IsNull(sPrefix & "State") Then
                    sState = CStr(dr.Item(sPrefix & "State"))
                End If
                'Check if ZipCode exists
                If Not dr.IsNull(sPrefix & "ZipCode") Then
                    sZip = CStr(dr.Item(sPrefix & "ZipCode"))
                End If
                'Check if Country exists
                If Not dr.IsNull(sPrefix & "Country") Then
                    sCountry = CStr(dr.Item(sPrefix & "Country"))
                End If

                'Only populate AddressBlock of the address is valid/complete
                '2/5/08 RJK - State is no longer required for the valid address test.
                'Several international addresses do not have States.  The new rule
                'is that at least one of the City, State or PostalCode (ZipCode) is provided.
                If sLine1.Length > 0 _
                        AndAlso (sCity.Length > 0 _
                        OrElse sState.Length > 0 _
                        OrElse sZip.Length > 0) Then
                    .AddressLine1 = sLine1
                    .AddressLine2 = sLine2
                    .AddressLine3 = sLine3
                    .City = sCity
                    .State = sState
                    .ZipCode = sZip
                    .Country = sCountry
                Else
                    .AddressLine1 = ""
                    .AddressLine2 = "Address Not Provided"
                    .AddressLine3 = ""
                    .City = ""
                    .State = ""
                    .ZipCode = ""
                    .Country = ""
                End If
            End With

        End Sub
        Private Sub LoadOrderDetails()
            Dim sSQL As String, dt As Data.DataTable
            'Anil B for issue 15341 on 20-03-2013
            'Set Product name as Web Name
            Dim sOrderDetailView As String = AptifyApplication.GetEntityBaseView("OrderLines")
            Dim sProductView As String = AptifyApplication.GetEntityBaseView("Products")
            Try
                sSQL = "SELECT *, case when (P.WebName is null or P.WebName = '') 	then P.Name 	Else P.WebName End as WebName FROM " & _
                       AptifyApplication.GetEntityBaseDatabase("OrderLines") & ".." & sOrderDetailView & " OD inner join " & AptifyApplication.GetEntityBaseDatabase("Products") & ".." & sProductView & " P on OD.ProductID=P.id " & _
                       "WHERE OrderID=" & _
                        Request.QueryString("ID") & " ORDER BY Sequence"

                dt = DataAction.GetDataTable(sSQL)
                '11/23/07 Tamasa, Added the two column values with the correct currency format to the array list from datatable. Issue 5371.
                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                    arPrice = New ArrayList
                    arExtended = New ArrayList
                    For Each row As DataRow In dt.Rows
                        arPrice.Add(Format$(row.Item("Price"), sCurrencyFormat))
                        arExtended.Add(Format$(row.Item("Extended"), sCurrencyFormat))
                    Next
                End If
                'End
                grdMain.DataSource = dt
                grdMain.DataBind()
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Private Sub LoadEmailOrder()
            Try
                'Get ShipTo Person's Email address to pre-populate the Email textbox
                Dim sSQL As String = "SELECT Email FROM " & Database & "..vwPersons WHERE ID=" & m_lCustomerID.ToString
                Dim sEmail As String
                sEmail = DataAction.ExecuteScalar(sSQL, IAptifyDataAction.DSLCacheSetting.UseCache).ToString
                Me.EmailOrderTextBox.Text = sEmail

                'Reset Email message to nothing
                SendEmailLabel.Text = ""
            Catch ex As ArgumentException
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub EmailOrderButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles EmailOrderButton.Click
            Try
                'Suraj Issue 15210 , 3/15/13, check multiple emails are valid or not if valid then send a mail
                If CheckMulipleEmailISValid() Then
                    'Get the Process Flow ID to be used for sending the Order Confirmation Email
                    Dim sSQL As String = "SELECT ID FROM " & Database & "..vwProcessFlows WHERE Name='Send eBusiness Order Confirmation Email'"
                    Dim lProcessFlowID As Long = CLng(DataAction.ExecuteScalar(sSQL, IAptifyDataAction.DSLCacheSetting.UseCache))

                    Dim context As New AptifyContext
                    context.Properties.AddProperty("OrderID", CLng(Request.QueryString("ID")))
                    context.Properties.AddProperty("EmailAddresses", Me.EmailOrderTextBox.Text)
                    'Added by Dipali Issue No:13305
                    context.Properties.AddProperty("MessageSystem", MessageSystem)
                    Dim result As ProcessFlowResult
                    result = ProcessFlowEngine.ExecuteProcessFlow(Me.AptifyApplication, lProcessFlowID, context)
                    If result.IsSuccess Then
                        SendEmailLabel.ForeColor = Drawing.Color.Blue
                        SendEmailLabel.Text = "Order Confirmation has been sent."
                    Else
                        SendEmailLabel.ForeColor = Drawing.Color.Red
                        SendEmailLabel.Text = "Email failed. Contact Customer Service for help."
                    End If
                End If

            Catch ex As ArgumentException
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        'Navin Prasad Issue 11032
        '11/23/07 Tamasa, Added to bound the Item with correct currency format.Issue 5371
        Dim Count As Integer = 0

        'HP Issue#8621:  for products which are of type 'Class', extract ClassID from the class registration entity and set link url accordingly 
        Private Function SetURLPerProductType(ByVal productId As Integer, ByVal extId As Integer) As String
            Dim url As String = String.Empty
            Dim classId As Integer
            Dim sql As String
            Dim p As Aptify.Applications.ProductSetup.ProductObject
            p = CType(Me.AptifyApplication.GetEntityObject("Products", productId), Aptify.Applications.ProductSetup.ProductObject)

            If Not IsNothing(p.GetValue("ProductType")) AndAlso p.GetValue("ProductType").ToString.ToUpper = "CLASS" Then
                sql = "SELECT TOP 1 ClassID FROM " & AptifyApplication.GetEntityBaseDatabase("ClassRegistrations") & _
                          "..vwClassRegistrations WHERE ID=" & extId
                classId = CInt(DataAction.ExecuteScalar(sql))
                If classId > 0 Then
                    url = ClassViewURL & "?ClassID=" & classId
                End If
            Else
                url = ProductURL & "?ID=" & productId
            End If

            Return url
        End Function

        Protected Sub grdMain_PageIndexChanging(ByVal sender As Object, ByVal e As GridPageChangedEventArgs) Handles grdMain.PageIndexChanged
            LoadOrder()
        End Sub
        'Navin Prasad Issue 11032
        Protected Sub grdMain_RowDataBound(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles grdMain.ItemDataBound
            If (TypeOf (e.Item) Is GridDataItem) Then
                Dim labelPrice As Label
                Dim labelExtended As Label

                labelPrice = New Label
                labelExtended = New Label

                labelPrice = DirectCast(e.Item.FindControl("lblPrice"), Label)
                labelExtended = DirectCast(e.Item.FindControl("lblExtended"), Label)

                labelPrice.Text = CStr(arPrice.Item(Count))
                labelExtended.Text = CStr(arExtended.Item(Count))

                ' examine each orderline in order to properly set product links
                DirectCast(e.Item.FindControl("link"), HyperLink).NavigateUrl = _
                SetURLPerProductType(CInt(DataBinder.Eval(e.Item.DataItem, "ProductID")), CInt(DataBinder.Eval(e.Item.DataItem, "ExtendedAttributeID")))
                Count += 1
            End If
        End Sub
        ''' <summary>
        ''' Check web user IsValidUser if yes then allow his to see orderconfirmation details else redirect his login page 
        ''' </summary>
        ''' <param name="UserId">Is the Id of current login user </param>
        ''' <param name="OrderID">Is the id of order which user want to see</param>
        ''' <remarks></remarks>
        Private Function DoesUserHaveAccessOnOrder(ByVal UserId As Long, ByVal OrderID As Long) As Boolean
            Try
                Dim sSQL As String
                'This code for issue 13280 sachin 
                'Get value form persons view in 0 position datable
                sSQL = "SELECT COUNT(O.ID)  from " & _
                       Database & ".." & AptifyApplication.GetEntityBaseView("Persons") & " P " & _
                            ", " & Database & ".." & AptifyApplication.GetEntityBaseView("Orders") & " O " & _
                          " Where ((P.IsGroupAdmin = 1 AND O.BillToCompanyID = P.CompanyID AND P.ID = " & UserId & ") " & _
                              " OR ( O.BillToID = " & UserId & "))" & _
                                 " AND O.ID =" & OrderID
                Return CBool(DataAction.ExecuteScalar(sSQL))
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return False
            End Try
        End Function
        'Suraj Issue 15210 , 3/15/13, check multiple emails are valid or not 
        Private Function CheckMulipleEmailISValid() As Boolean
            Try
                Dim emailAddress As String() = EmailOrderTextBox.Text.Split(New Char() {","c})
                Dim bIsValidEmail As Boolean = False
                Dim email As String
                For Each email In emailAddress
                    'Suraj S Issue 15210 ,3/15/13 use Comman Function for email validation
                    bIsValidEmail = CommonMethods.EmailAddressCheck(email)
                    If (bIsValidEmail) = False Then
                        SendEmailLabel.Text = "Invalid Email Format"
                        SendEmailLabel.ForeColor = Drawing.Color.Red
                        Return False
                        Exit For
                    End If
                Next
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
            Return True
        End Function
    End Class
End Namespace
