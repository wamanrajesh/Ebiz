'Aptify e-Business 5.5.1, July 2013
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Applications.OrderEntry.Payments
Imports Aptify.Applications.Accounting
Imports Aptify.Framework.BusinessLogic.ProcessPipeline
Imports Telerik.Web.UI
Imports Aptify.Framework.Application
Imports Aptify.Framework.BusinessLogic
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.AttributeManagement
Imports System.Data
Imports Aptify.Framework.Web.eBusiness
Imports System.ComponentModel
Imports System.Collections.Generic
Imports Aptify.Applications.OrderEntry


Namespace Aptify.Framework.Web.eBusiness.ProductCatalog

    Partial Class AdminOrderDetail
        Inherits BaseUserControlAdvanced
        Protected Const ATTRIBUTE_ORDER_CONFIRMATION_PAGE As String = "OrderConfirmationURL"
        Protected Const ATTRIBUTE_ADMIN_PAYMENT_SUMMARY As String = "AdminPaymentSummary"
        Protected Const ATTRIBUTE_NOTIFICATION_IMAGE As String = "PaymentNotificationImage"
        Protected Const ATTRIBUTE_NOTIFICATION_TASKTYPE As String = "TaskType"
        Protected Const ATTRIBUTE_NOTIFICATION_TASKASSIGNEDTOID As String = "AssignedTOID"
        Protected Const ATTRIBUTE_NOTIFICATION_TASKDESCRIPTION As String = "TaskDescription"
        Protected Const ATTRIBUTE_NOTIFICATION_TASKSTATUS As String = "TaskStatus"
        Protected Const ATTRIBUTE_NOTIFICATION_TASKPRIORITY As String = "TaskPriority"
        Protected Const ATTRIBUTE_LOGIN_PAGE As String = "LoginPage" 'Added by Sandeep for Issue 15051 on 12/03/2013
        Dim sCuurSymbol As String
#Region "MakePayment Specific Properties"
        ''' <summary>
        ''' OrderConfirmation page url
        ''' </summary>
        Public Overridable Property OrderConfirmationURL() As String
            Get
                If Not ViewState(ATTRIBUTE_ORDER_CONFIRMATION_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_ORDER_CONFIRMATION_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_ORDER_CONFIRMATION_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        Public Overridable Property AdminPaymentSummary() As String
            Get
                If Not ViewState(ATTRIBUTE_ADMIN_PAYMENT_SUMMARY) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_ADMIN_PAYMENT_SUMMARY))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_ADMIN_PAYMENT_SUMMARY) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' PaymentNotificationImage property
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Property PaymentNotificationImage() As String
            Get
                If Not ViewState(ATTRIBUTE_NOTIFICATION_IMAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_NOTIFICATION_IMAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_NOTIFICATION_IMAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        Public Overridable Property TaskType() As String
            Get
                If Not ViewState(ATTRIBUTE_NOTIFICATION_TASKTYPE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_NOTIFICATION_TASKTYPE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_NOTIFICATION_TASKTYPE) = value
            End Set
        End Property

        Public Overridable Property AssignedTOID() As String
            Get
                If Not ViewState(ATTRIBUTE_NOTIFICATION_TASKASSIGNEDTOID) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_NOTIFICATION_TASKASSIGNEDTOID))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_NOTIFICATION_TASKASSIGNEDTOID) = value
            End Set
        End Property

        Public Overridable Property TaskDescription() As String
            Get
                If Not ViewState(ATTRIBUTE_NOTIFICATION_TASKDESCRIPTION) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_NOTIFICATION_TASKDESCRIPTION))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_NOTIFICATION_TASKDESCRIPTION) = value
            End Set
        End Property

        Public Overridable Property TaskStatus() As String
            Get
                If Not ViewState(ATTRIBUTE_NOTIFICATION_TASKSTATUS) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_NOTIFICATION_TASKSTATUS))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_NOTIFICATION_TASKSTATUS) = value
            End Set
        End Property

        Public Overridable Property TaskPriority() As String
            Get
                If Not ViewState(ATTRIBUTE_NOTIFICATION_TASKPRIORITY) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_NOTIFICATION_TASKPRIORITY))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_NOTIFICATION_TASKPRIORITY) = value
            End Set
        End Property
        'Added by Sandeep for Issue 15051 on 12/03/2013
        Public Overridable Property LoginPage() As String
            Get
                If Not ViewState(ATTRIBUTE_LOGIN_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_LOGIN_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_LOGIN_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

#End Region

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(OrderConfirmationURL) Then
                'since value is the 'default' check the XML file for possible custom setting
                OrderConfirmationURL = Me.GetLinkValueFromXML(ATTRIBUTE_ORDER_CONFIRMATION_PAGE)
                If String.IsNullOrEmpty(OrderConfirmationURL) Then
                    Me.grdOrderDetails.Enabled = False
                    Me.grdOrderDetails.ToolTip = "OrderConfirmationURL property has not been set."
                End If
            End If

            If String.IsNullOrEmpty(AdminPaymentSummary) Then
                'since value is the 'default' check the XML file for possible custom setting
                AdminPaymentSummary = Me.GetLinkValueFromXML(ATTRIBUTE_ADMIN_PAYMENT_SUMMARY)
                If String.IsNullOrEmpty(AdminPaymentSummary) Then
                    Me.grdOrderDetails.Enabled = False
                    Me.grdOrderDetails.ToolTip = "AdminPaymentSummary property has not been set."
                End If
            End If

            If String.IsNullOrEmpty(PaymentNotificationImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                PaymentNotificationImage = Me.GetLinkValueFromXML(ATTRIBUTE_NOTIFICATION_IMAGE)
                If String.IsNullOrEmpty(PaymentNotificationImage) Then
                    Me.grdOrderDetails.Enabled = False
                    Me.grdOrderDetails.ToolTip = "PaymentNotificationImage property has not been set."
                End If
            End If

            If String.IsNullOrEmpty(TaskType) Then
                'since value is the 'default' check the XML file for possible custom setting
                TaskType = Me.GetPropertyValueFromXML(ATTRIBUTE_NOTIFICATION_TASKTYPE)
                If String.IsNullOrEmpty(TaskType) Then
                    Me.grdOrderDetails.Enabled = False
                    Me.grdOrderDetails.ToolTip = "TaskType property has not been set."
                End If
            End If

            If String.IsNullOrEmpty(AssignedTOID) Then
                'since value is the 'default' check the XML file for possible custom setting
                AssignedTOID = Me.GetPropertyValueFromXML(ATTRIBUTE_NOTIFICATION_TASKASSIGNEDTOID)
                If String.IsNullOrEmpty(AssignedTOID) Then
                    Me.grdOrderDetails.Enabled = False
                    Me.grdOrderDetails.ToolTip = "AssignedTOID property has not been set."
                End If
            End If

            If String.IsNullOrEmpty(TaskDescription) Then
                'since value is the 'default' check the XML file for possible custom setting
                TaskDescription = Me.GetPropertyValueFromXML(ATTRIBUTE_NOTIFICATION_TASKDESCRIPTION)
                If String.IsNullOrEmpty(TaskDescription) Then
                    Me.grdOrderDetails.Enabled = False
                    Me.grdOrderDetails.ToolTip = "TaskDescription property has not been set."
                End If
            End If

            If String.IsNullOrEmpty(TaskStatus) Then
                'since value is the 'default' check the XML file for possible custom setting
                TaskStatus = Me.GetPropertyValueFromXML(ATTRIBUTE_NOTIFICATION_TASKSTATUS)
                If String.IsNullOrEmpty(TaskStatus) Then
                    Me.grdOrderDetails.Enabled = False
                    Me.grdOrderDetails.ToolTip = "TaskStatus property has not been set."
                End If
            End If

            If String.IsNullOrEmpty(TaskPriority) Then
                'since value is the 'default' check the XML file for possible custom setting
                TaskPriority = Me.GetPropertyValueFromXML(ATTRIBUTE_NOTIFICATION_TASKPRIORITY)
                If String.IsNullOrEmpty(TaskPriority) Then
                    Me.grdOrderDetails.Enabled = False
                    Me.grdOrderDetails.ToolTip = "TaskPriority property has not been set."
                End If
            End If
            'Added by Sandeep for Issue 15051 on 12/03/2013
            If String.IsNullOrEmpty(LoginPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                LoginPage = Me.GetLinkValueFromXML(ATTRIBUTE_LOGIN_PAGE)
            End If

        End Sub

        Private Class PayInfo
            Public OrderID As Long
            Public PayAmount As Decimal
            Public Balance As Decimal
        End Class
        Private Sub LoadOrders()
            Dim dtOrder As Data.DataTable
            Dim dtProducts As DataTable
            Dim sSQL As String
            Dim sProducts As String = String.Empty
            Dim i As Integer, j As Integer
            Dim icount As Integer = 0
            Dim iProdCnt As Integer = 0
            Try
                If ViewState("orderdt") Is Nothing Then
                    Dim ParentID As Long = -1
                    ParentID = Me.DataAction.ExecuteScalar("Select ISNULL(ParentID, -1) as ParentID from " & Database & "..vwCompanies where ID=" & User1.CompanyID.ToString)
                    'add contion when redirecting from group admin dashboard
                    Dim sOrderStatus As String = Request.QueryString("OrderStatus")
                    Dim sWhere As String = ""

                    If String.IsNullOrEmpty(sOrderStatus) = False Then
                        sOrderStatus = sOrderStatus.ToUpper.Trim

                        If sOrderStatus = "PARTLYPAID" Then
                            sWhere = " AND o.Balance < o.GrandTotal "
                        ElseIf sOrderStatus = "UNPAID" Then
                            sWhere = " AND o.Balance = o.GrandTotal "
                        End If
                        Dim sDate As String = Request.QueryString("Date")
                        If String.IsNullOrEmpty(sDate) = False AndAlso IsDate(sDate) Then
                            sWhere = sWhere & " AND Month(o.OrderDate)=" & CType(sDate, Date).Month() & " AND Year(o.OrderDate)=" & CType(sDate, Date).Year() & " "
                        End If
                    End If
                    'fectching order for link to company
                    sSQL = "SELECT o.ShipToName Name, o.ShipToCity City, o.ID, o.OrderDate, o.Line1_ProductName Product,CONVERT(VARCHAR,o.GrandTotal,1) As GrandTotal,CONVERT(VARCHAR,o.Balance,1) As Balance,convert(numeric(10,2), o.Balance) as PayAmount, " & _
                         "ct.CurrencySymbol,o.CurrencyType ,o.CurrencyTypeID, NumDigitsAfterDecimal,  o.CompanyAdministratorComments FROM " & _
                          Database & "..vwOrders o " & _
                          "INNER JOIN " & Database & "..vwCurrencyTypes ct ON o.CurrencyTypeID=ct.ID " & _
                          " WHERE o.BillToCompanyID = " & User1.CompanyID & _
                         " AND o.Balance > 0 AND o.OrderStatus IN ('Taken', 'Shipped','Back-Ordered') And o.OrderType IN ('Regular', 'Quotation','Back-Order') " & sWhere & _
                          "and o.CurrencyTypeID='" & radcurrency.SelectedValue().Trim() & "' Order By o.ShipToName, o.ShipToCity"
                    dtOrder = DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)
                    Dim dcolUrl As DataColumn = New DataColumn()
                    dcolUrl.Caption = "OrderConfirmationURL"
                    dcolUrl.ColumnName = "OrderConfirmationURL"
                    dtOrder.Columns.Add(dcolUrl)
                    If dtOrder.Rows.Count > 0 Then
                        For Each rw As DataRow In dtOrder.Rows
                            rw("OrderConfirmationURL") = OrderConfirmationURL + "?ID=" + rw("ID").ToString
                        Next
                    End If


                    'show product for related order
                    'aparna issue 15060 
                    For i = 0 To dtOrder.Rows.Count - 1
                        sSQL = "SELECT Count(P.Id) 'ProdCount' FROM " & AptifyApplication.GetEntityBaseDatabase("OrderLines") & ".." & AptifyApplication.GetEntityBaseView("OrderLines") & " OD INNER JOIN " & AptifyApplication.GetEntityBaseDatabase("Products") & ".." & AptifyApplication.GetEntityBaseView("Products") & " P ON P.ID = OD.ProductID WHERE OD.OrderID = " & dtOrder.Rows(i)("ID")
                        iProdCnt = Convert.ToInt32(DataAction.ExecuteScalar(sSQL))
                        'Anil B for issue 15378 on 04-04-2013
                        'Show the product web name if available else product name
                        sSQL = "SELECT case when (P.WebName is null or P.WebName = '') 	then P.Name 	Else P.WebName End as Name FROM " & AptifyApplication.GetEntityBaseDatabase("OrderLines") & ".." & AptifyApplication.GetEntityBaseView("OrderLines") & " OD INNER JOIN " & AptifyApplication.GetEntityBaseDatabase("Products") & ".." & AptifyApplication.GetEntityBaseView("Products") & " P ON P.ID = OD.ProductID WHERE OD.OrderID = " & dtOrder.Rows(i)("ID")
                        dtProducts = DataAction.GetDataTable(sSQL)

                        If dtProducts IsNot Nothing Then
                            If iProdCnt <> 0 AndAlso iProdCnt > 2 Then
                                icount = 2
                            Else
                                icount = iProdCnt
                            End If
                        End If

                        For j = 0 To icount - 1
                            sProducts = sProducts + dtProducts.Rows(j)("Name").ToString() + ", "
                        Next
                        If sProducts.Length > 0 Then
                            If iProdCnt > 2 Then
                                sProducts = sProducts.Substring(0, sProducts.Length - 0) & " and others"
                            Else
                                sProducts = sProducts.Substring(0, sProducts.Length - 2)
                            End If
                        End If

                        dtOrder.Rows(i)("Product") = sProducts
                        sProducts = String.Empty
                    Next
                    'End show product for related order
                    'add currency for selected for payment
                    If dtOrder.Rows.Count > 0 Then
                        sSQL = "SELECT CurrencySymbol FROM " & _
                           Database & "..vwCurrencyTypes where ID=" & radcurrency.SelectedValue().Trim()
                        Dim dt = DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)
                        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                            sCuurSymbol = dt.Rows(0)("CurrencySymbol").ToString().Trim
                            ViewState("sCuurSymbol") = sCuurSymbol
                        End If
                        'Add selected order in dictionary
                        Dim orderdetails As New Dictionary(Of Integer, Decimal)
                        If ViewState("CHECKED_ITEMS") IsNot Nothing Then
                            orderdetails = DirectCast(ViewState("CHECKED_ITEMS"), Dictionary(Of Integer, Decimal))
                        End If
                        If orderdetails Is Nothing Or orderdetails.Count <= 0 Then
                            txtTotal.Text = sCuurSymbol & "0.00"
                        End If
                        grdOrderDetails.DataSource = dtOrder
                        ViewState("orderdt") = dtOrder

                    Else
                        lblTotal.Visible = False
                        txtTotal.Visible = False
                        'Anil B change for 10737 on 13/03/2013
                        'Set Credit Card ID to load property form Navigation Config
                        CreditCard.Visible = False
                        cmdMakePayment.Visible = False
                        radcurrency.Visible = False
                        lblfilter.Visible = False
                        payoffdiv.Visible = False
                        lblrecmsg.Visible = True
                    End If
                Else
                    grdOrderDetails.DataSource = CType(ViewState("orderdt"), DataTable)
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub


        Protected Sub grdOrderDetails_PageIndexChanged(ByVal source As Object, ByVal e As Telerik.Web.UI.GridPageChangedEventArgs) Handles grdOrderDetails.PageIndexChanged
            SaveCheckedValues()
        End Sub
        Protected Function GetFormattedCurrency(ByVal Container As Object, ByVal sField As String) As String
            Dim sCurrencySymbol As String
            Dim iNumDecimals As Integer
            Dim sCurrencyFormat As String
            Try
                ' get the appropriate currency data from the data row
                sCurrencySymbol = Container.DataItem("CurrencySymbol")
                iNumDecimals = Container.DataItem("NumDigitsAfterDecimal")

                ' build the string we'll use for formatting the currency
                ' it consists of the symbol followed by 0. and the appropriate
                ' number of decimals needed in the final string
                sCurrencyFormat = sCurrencySymbol.Trim & _
                                  "{0:" & "0." & _
                                  New String("0"c, iNumDecimals) & "}"

                ' format the string using the currency format created
                Return String.Format(sCurrencyFormat, Container.DataItem(sField))
            Catch ex As Exception
                Try
                    ' on failure, at least try and return the
                    ' data contents
                    Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                    Return Container.DataItem(sField)
                Catch ex2 As Exception
                    Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex2)
                    Return "{ERROR}"
                End Try
            End Try
        End Function

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Hidden.Value = "true"
            SetProperties()
            Try
                If Not IsPostBack Then
                    'Anil B change for 10737 on 13/03/2013
                    'Set Credit Card ID to load property form Navigation Config
                    CreditCard.LoadCreditCardInfo()
                    LoadCurrency()
                    ViewState.Remove("orderdt")
                    AddExpression()
                End If
                lblError.Text = ""
                If User1.UserID < 0 Then
                    Response.Redirect(LoginPage) 'Added by Sandeep for Issue 15051 on 12/03/2013
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub grdOrderDetails_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdOrderDetails.NeedDataSource
            If User1.UserID > 0 Then
                LoadOrders()
            End If
        End Sub

        Protected Sub cmdMakePayment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdMakePayment.Click
            ' Me.cmdMakePayment.Attributes.Add("onClick", "validatePage();")

            ' Save a payment to the database with the information the user provided
            Dim i As Integer
            Dim orderdetails As New Dictionary(Of Integer, Decimal)
            Dim arPay() As PayInfo
            Dim sSymCurr As String = ViewState("sCuurSymbol").ToString().Trim()
            Dim lblPay As New Label
            Dim OrderID As HyperLink
            Dim txtPay As New TextBox
            Dim dtorder As DataTable = ViewState("orderdt")
            If ViewState("CHECKED_ITEMS") IsNot Nothing Then
                orderdetails = DirectCast(ViewState("CHECKED_ITEMS"), Dictionary(Of Integer, Decimal))
            End If
            Try
                ReDim arPay(0)
                If Page.IsValid Then
                    For i = 0 To grdOrderDetails.Items.Count - 1
                        lblPay = grdOrderDetails.Items(i).FindControl("lblBalanceAmount")
                        txtPay = grdOrderDetails.Items(i).FindControl("txtPayAmt")
                        Dim chkMakePayment As CheckBox = CType(grdOrderDetails.Items(i).FindControl("chkMakePayment"), CheckBox)
                        If chkMakePayment.Checked = True Then
                            If txtPay.Text.Trim.Length > 0 Then
                                If IsNumeric(txtPay.Text) Then

                                    If CDec(txtPay.Text) > 0 Then
                                        ReDim Preserve arPay(UBound(arPay) + 1)
                                        arPay(UBound(arPay) - 1) = New PayInfo
                                        With arPay(UBound(arPay) - 1)

                                            Dim lblNumDigits As Label = grdOrderDetails.Items(i).FindControl("lblNumDigitsAfterDecimal")
                                            Dim lblBal As Label = grdOrderDetails.Items(i).FindControl("lblBalanceAmount")
                                            OrderID = grdOrderDetails.Items(i).FindControl("lblOrderNo")
                                            .OrderID = OrderID.Text
                                            .PayAmount = Math.Round(CDec(txtPay.Text), _
                                                                      CInt(lblNumDigits.Text))

                                            .Balance = Convert.ToDecimal(lblBal.Text.ToString().Replace(sSymCurr, ""))
                                        End With
                                    End If
                                Else
                                    lblError.Text = "Values entered must be valid currency quantities."
                                    lblError.Visible = True
                                    Exit Sub
                                End If
                            End If
                        End If
                    Next

                    For k = 0 To UBound(arPay) - 1
                        With arPay(k)
                            If orderdetails.ContainsKey(.OrderID) Then
                                orderdetails.Remove(.OrderID)
                            End If
                        End With
                    Next
                    'ADD grid value other than current page
                    For i = 0 To dtorder.Rows.Count - 1
                        For j = 0 To orderdetails.Count - 1
                            If orderdetails.Keys(j) = dtorder.Rows(i)("ID") Then

                                Dim strPay As String = orderdetails.Values(j)
                                If strPay.Trim.Length > 0 Then
                                    If IsNumeric(strPay) Then
                                        If CDec(strPay) > 0 Then
                                            ReDim Preserve arPay(UBound(arPay) + 1)
                                            arPay(UBound(arPay) - 1) = New PayInfo
                                            With arPay(UBound(arPay) - 1)

                                                Dim lblNumDigits As String = dtorder.Rows(i)("NumDigitsAfterDecimal")
                                                Dim lblBal As String = dtorder.Rows(i)("Balance")
                                                .OrderID = dtorder.Rows(i)("ID")
                                                .PayAmount = Math.Round(CDec(strPay), _
                                                                          CInt(lblNumDigits))

                                                .Balance = Convert.ToDecimal(lblBal.ToString().Replace(sSymCurr, ""))
                                            End With
                                        End If
                                    Else
                                        lblError.Text = "Values entered must be valid currency quantities."
                                        lblError.Visible = True
                                        Exit Sub
                                    End If
                                End If
                            End If
                        Next
                    Next

                    ' Now, go through and make sure that each item is validated correctly
                    lblError.Visible = False
                    If UBound(arPay) > 0 Then
                        For i = 0 To UBound(arPay) - 1
                            With arPay(i)
                                If .PayAmount <= 0 Then
                                    lblError.Text = "You must select one or more Orders to make a Payment"
                                    lblError.Visible = True
                                    Exit Sub
                                End If
                                If .PayAmount > .Balance Then
                                    lblError.Text = "Payments must be less than the balance due"
                                    lblError.Visible = True
                                    Exit Sub
                                End If
                            End With
                        Next
                        'Anil B change for 10737 on 13/03/2013
                        'Set Credit Card ID to load property form Navigation Config
                        If Len(CreditCard.CCNumber) = 0 Or _
                           Len(CreditCard.CCExpireDate) = 0 Then
                            lblError.Text = "Credit Card Information Required"
                            lblError.Visible = True
                        End If

                        If PostPayment(arPay) Then
                            'Anil B change for 10737 on 13/03/2013
                            'Set Credit Card ID to load property form Navigation Config
                            CreditCard.CCNumber = ""
                            CreditCard.CCExpireDate = ""
                            CreditCard.CCSecurityNumber = ""
                            LoadOrders()
                            radpaymentmsg.VisibleOnPageLoad = True
                            cmdMakePayment.Enabled = False
                            ViewState.Remove("orderdt")
                            MyBase.Response.Redirect(AdminPaymentSummary, False)

                        Else
                            lblError.Text = "An error took place while processing your payment"
                            lblError.Visible = True
                        End If
                    Else
                        lblError.Visible = True
                        lblError.Text = "You must select one or more Orders to make a Payment"
                    End If
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Function PostPayment(ByVal arPay() As PayInfo) As Boolean
            ' post the payment to the database using the CGI GE
            Dim oPayment As AptifyGenericEntityBase
            Dim i As Integer
            Dim iCurrencyID As Integer = Convert.ToInt32(radcurrency.SelectedValue().Trim())
            Try
                oPayment = AptifyApplication.GetEntityObject("Payments", -1)
                oPayment.SetValue("EmployeeID", EBusinessGlobal.WebEmployeeID(Page.Application))
                oPayment.SetValue("PersonID", User1.PersonID)

                ''Set company value based on the selected payment type
                If (CreditCard.EnablePaymentTypeSelection = True AndAlso CType(CreditCard.FindControl("rbCompanyPayment"), RadioButton).Checked = True) OrElse CreditCard.EnablePaymentTypeSelection = False Then
                    oPayment.SetValue("CompanyID", User1.CompanyID)
                Else
                    oPayment.SetValue("CompanyID", -1)
                End If

                oPayment.SetValue("PaymentDate", Date.Today)
                oPayment.SetValue("DepositDate", Date.Today)
                oPayment.SetValue("EffectiveDate", Date.Today)
                'Anil B change for 10737 on 13/03/2013
                'Set Credit Card ID to load property form Navigation Config
                oPayment.SetValue("PaymentTypeID", CreditCard.PaymentTypeID)
                oPayment.SetValue("CCAccountNumber", CreditCard.CCNumber)
                oPayment.SetValue("CCExpireDate", CreditCard.CCExpireDate)
                'Anil B change for 10254 on 29/03/2013
                'Set reference transaction for payment
                If CreditCard.CCNumber = "-Ref Transaction-" Then
                    oPayment.SetValue("ReferenceTransactionNumber", CreditCard.ReferenceTransactionNumber)
                    oPayment.SetValue("ReferenceExpiration", CreditCard.ReferenceExpiration)
                End If
                ''RashmiP, issue 9976, (Unable to Make A Payment When CVV Is Checked) 09/30/10
                oPayment.SetAddValue("_xCCSecurityNumber", CreditCard.CCSecurityNumber)
                oPayment.SetValue("PaymentLevelID", User1.GetValue("GLPaymentLevelID"))
                oPayment.SetValue("Comments", "Created through the CGI e-Business Suite")
                oPayment.SetValue("CurrencyTypeID", iCurrencyID)
                ' to have an automatic conversion of quotes to regular
                ' orders, set the Convert flag to true.
                oPayment.SetAddValue("_xConvertQuotesToRegularOrder", "1")
                If oPayment.Fields("PaymentInformationID").EmbeddedObjectExists Then
                    Dim oOrderPayInfo As PaymentInformation = DirectCast(oPayment.Fields("PaymentInformationID").EmbeddedObject, PaymentInformation)
                    oOrderPayInfo.CreditCardSecurityNumber = CreditCard.CCSecurityNumber
                    oOrderPayInfo.SetValue("SaveForFutureUse", CreditCard.SaveCardforFutureUse)
                    'Anil B for issue 15442 on 22/04/2013
                    'Set CC Partial Number for payment
                    oOrderPayInfo.SetValue("CCPartial", CreditCard.CCPartial)
                End If
                Dim sPayOrderID As String = String.Empty
                For i = 0 To UBound(arPay) - 1
                    With oPayment.SubTypes("PaymentLines").Add
                        .SetValue("Amount", arPay(i).PayAmount)
                        .SetValue("OrderID", arPay(i).OrderID)
                    End With

                    If i = 0 Then
                        sPayOrderID = CStr(arPay(i).OrderID)
                    Else
                        sPayOrderID = sPayOrderID + "," + CStr(arPay(i).OrderID)
                    End If

                Next
                Dim bPaymentSaved As Boolean = oPayment.Save(False)
                If bPaymentSaved Then
                    Session("sPayOrderID") = sPayOrderID
                End If

                Return bPaymentSaved
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return False

            End Try
        End Function

        Protected Sub chkMakePayment_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
            'Dim flag As Boolean = False
            Dim chk1 As CheckBox = CType(sender, CheckBox)
            'check header value for select all functionality
            Dim header As GridHeaderItem = TryCast(grdOrderDetails.MasterTableView.GetItems(GridItemType.Header)(0), GridHeaderItem)
            Dim chkAllMakePayment As CheckBox = DirectCast(header.FindControl("chkAllMakePayment"), CheckBox)
            Dim grow As GridDataItem = DirectCast(chk1.NamingContainer, GridDataItem)
            Dim i As Integer = grow.DataSetIndex Mod grdOrderDetails.PageSize
            Dim txtPaymentaMT As TextBox = (DirectCast(grdOrderDetails.Items(i).FindControl("txtPayAmt"), TextBox))
            Dim sSymCurrancy As String = ViewState("sCuurSymbol").ToString().Trim()
            If chk1.Checked = True Then
                Dim textBoxText As String = (DirectCast(grdOrderDetails.Items(i).FindControl("lblBalanceAmount"), Label)).Text
                txtPaymentaMT.Text = textBoxText.ToString().Replace(sSymCurrancy, "")
                txtPaymentaMT.Focus()

            Else
                txtPaymentaMT.Text = "0.00"
            End If
            Dim iSumTotalPayAmmount As Decimal
            Dim TotalPayAmmount As Decimal
            Dim chkflag As Boolean = True
            For Each row As GridDataItem In grdOrderDetails.Items
                chk1 = DirectCast(row.FindControl("chkMakePayment"), CheckBox)
                If chk1.Checked = True Then
                    Dim sSymCurr As String = ViewState("sCuurSymbol").ToString().Trim()
                    TotalPayAmmount = getTotal(row)
                    iSumTotalPayAmmount += TotalPayAmmount
                    setTotal(iSumTotalPayAmmount, sSymCurr)
                Else
                    chkflag = False
                    If chkAllMakePayment.Checked = True Then
                        chkAllMakePayment.Checked = False
                    End If
                End If
            Next

            CalculateTotal(iSumTotalPayAmmount)
            'check flag for selce all checked or not
            If chkflag Then
                chkAllMakePayment.Checked = True
            End If
            For Each row As GridDataItem In grdOrderDetails.Items
                chk1 = DirectCast(row.FindControl("chkMakePayment"), CheckBox)
                SaveCheckedValues()
            Next
        End Sub

        Dim iTotleAmmount As Decimal
        Dim iTotalBalance As Decimal

        Protected Sub grdOrderDetails_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdOrderDetails.ItemDataBound
            Dim sFormDec As String
            Dim ImgFlag As New Image
            'Suraj issue 14877 2/27/13 date time filtering
            Dim dateColumns As New List(Of String)
            Try
                'Add datecolumn uniqueName in list for Date format
                dateColumns.Add("GridDateTimeColumnOrderDate")
                CommonMethods.FormatedDateOnGrid(dateColumns, e.Item)
                If e.Item.ItemType = GridItemType.Item OrElse e.Item.ItemType = GridItemType.AlternatingItem Then
                    If DataBinder.Eval(e.Item.DataItem, "CompanyAdministratorComments").Equals(System.DBNull.Value) = False AndAlso String.IsNullOrEmpty(DataBinder.Eval(e.Item.DataItem, "CompanyAdministratorComments").ToString().Trim) = False Then

                        ImgFlag = CType(e.Item.FindControl("ImgFlag"), Image)
                        If ImgFlag IsNot Nothing Then
                            If String.IsNullOrEmpty(PaymentNotificationImage) = False Then
                                ImgFlag.ImageUrl = PaymentNotificationImage
                                ImgFlag.Visible = True
                            End If
                        End If
                    End If
                End If

                If e.Item.ItemType = DataControlRowType.DataRow Then
                    iTotleAmmount += Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "GrandTotal"))

                End If
                Dim sSymCurr As String = ViewState("sCuurSymbol").ToString().Trim()
                sFormDec = String.Format("{0:n2}", iTotleAmmount)
                Dim txtPayAmt As TextBox = DirectCast(e.Item.FindControl("txtPayAmt"), TextBox)
                If txtPayAmt IsNot Nothing Then
                    sFormDec = String.Format("{0:n2}", Convert.ToDecimal(txtPayAmt.Text))
                    txtPayAmt.Text = sFormDec
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub


        Protected Sub txtPayAmt_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
            Try
                Dim flag As Boolean = False
                Dim sSymCurr As String = ViewState("sCuurSymbol").ToString().Trim()
                Dim txtPaymentaMT As TextBox = CType(sender, TextBox)

                Dim header As GridHeaderItem = TryCast(grdOrderDetails.MasterTableView.GetItems(GridItemType.Header)(0), GridHeaderItem)
                Dim chkAllMakePayment As CheckBox = DirectCast(header.FindControl("chkAllMakePayment"), CheckBox)
                Dim grow As GridDataItem = DirectCast(txtPaymentaMT.NamingContainer, GridDataItem)
                'Dim i As Integer = grow.DataSetIndex
                Dim i As Integer = grow.DataSetIndex Mod grdOrderDetails.PageSize
                Dim chk1 As CheckBox = (DirectCast(grdOrderDetails.Items(i).FindControl("chkMakePayment"), CheckBox))
                If Not txtPaymentaMT.Text = "" Then
                    'check for payment value is negative or less than 0
                    If Convert.ToDecimal(txtPaymentaMT.Text) = 0 Or Convert.ToDecimal(txtPaymentaMT.Text) < 0 Then
                        chk1.Checked = False
                        txtPaymentaMT.Text = "0.00"
                    ElseIf Convert.ToDecimal(txtPaymentaMT.Text) > 0 Then
                        chk1.Checked = True
                    End If
                Else
                    chk1.Checked = False
                    txtPaymentaMT.Text = "0.00"
                End If

                Dim iSumTotalPayAmmount As Decimal
                Dim TotalPayAmmount As Decimal
                Dim chkflag As Boolean = True
                For Each row As GridDataItem In grdOrderDetails.Items
                    chk1 = DirectCast(row.FindControl("chkMakePayment"), CheckBox)
                    If chk1.Checked = True Then
                        sSymCurr = ViewState("sCuurSymbol").ToString().Trim()
                        TotalPayAmmount = getTotal(row)
                        iSumTotalPayAmmount += TotalPayAmmount
                        setTotal(iSumTotalPayAmmount, sSymCurr)
                    Else
                        chkflag = False
                        If chkAllMakePayment.Checked = True Then
                            chkAllMakePayment.Checked = False
                        End If
                    End If
                Next
                CalculateTotal(iSumTotalPayAmmount)
                If chkflag Then
                    chkAllMakePayment.Checked = True
                End If
                For Each row As GridDataItem In grdOrderDetails.Items
                    chk1 = DirectCast(row.FindControl("chkMakePayment"), CheckBox)
                    If chk1.Checked = True Then
                        flag = True
                    End If
                Next
                SaveCheckedValues()
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try

        End Sub

        Protected Sub btnok_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnok.Click
            radpaymentmsg.VisibleOnPageLoad = False
        End Sub
        Protected Sub ToggleSelectedState(ByVal sender As Object, ByVal e As EventArgs)
            Try
                Dim headerCheckBox As CheckBox = CType(sender, CheckBox)
                If Not headerCheckBox.Checked Then
                    For Each dataItem As GridDataItem In grdOrderDetails.MasterTableView.Items
                        CType(dataItem.FindControl("chkMakePayment"), CheckBox).Checked = headerCheckBox.Checked
                        dataItem.Selected = headerCheckBox.Checked
                    Next
                End If
                Dim txtPayAmt As TextBox
                Dim iSumTotalPayAmmount As Decimal
                Dim txtPayAmmount As String
                Dim sTotleAmmount As String
                Dim iTotalAmmount As Decimal
                Dim sSymCurr As String = ViewState("sCuurSymbol").ToString().Trim()
                For Each row As GridDataItem In grdOrderDetails.Items
                    Dim chk1 As CheckBox = DirectCast(row.FindControl("chkMakePayment"), CheckBox)
                    txtPayAmt = DirectCast(row.FindControl("txtPayAmt"), TextBox)
                    If chk1.Checked = True Then
                        Dim textBoxText As String = (DirectCast(row.FindControl("txtPayAmt"), TextBox)).Text
                        txtPayAmt.Text = textBoxText.ToString().Replace(sSymCurr, "")
                        txtPayAmmount = txtPayAmt.Text
                        sTotleAmmount = txtPayAmmount.ToString().Replace(sSymCurr, "")
                        iTotalAmmount = Decimal.Parse(sTotleAmmount).ToString("0.00")
                        iSumTotalPayAmmount = iSumTotalPayAmmount + iTotalAmmount
                        'suraj S Issue 16036 5/2/13, add comma for amount
                        txtTotal.Text = sSymCurr & String.Format("{0:n2}", iSumTotalPayAmmount)

                    Else
                        If headerCheckBox.Checked Then
                            CType(row.FindControl("chkMakePayment"), CheckBox).Checked = headerCheckBox.Checked
                            row.Selected = headerCheckBox.Checked
                            Dim textBoxText As String = (DirectCast(row.FindControl("lblBalanceAmount"), Label)).Text
                            txtPayAmt.Text = textBoxText.ToString().Replace(sSymCurr, "")
                            txtPayAmmount = txtPayAmt.Text
                            sTotleAmmount = txtPayAmmount.ToString().Replace(sSymCurr, "")
                            iTotalAmmount = Decimal.Parse(sTotleAmmount).ToString("0.00")
                            iSumTotalPayAmmount = iSumTotalPayAmmount + iTotalAmmount
                            'suraj S Issue 16036 5/2/13, add comma for amount
                            txtTotal.Text = sSymCurr & String.Format("{0:n2}", iSumTotalPayAmmount)
                        Else
                            txtPayAmt = DirectCast(row.FindControl("txtPayAmt"), TextBox)
                            txtPayAmt.Text = "0.00"
                        End If
                    End If
                Next
                SaveCheckedValues()
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub LoadCurrency()

            Dim sSQL As String, dtcurrency As Data.DataTable

            Try
                'sSQL = " select distinct CurrencySymbol, CurrencyType from " & Database & "..vwOrders INNER JOIN " & Database & "..vwCurrencyTypes ct ON CurrencyTypeID=ct.ID   WHERE ShipToID IN (SELECT distinct ID FROM " & Database & "..vwPersons WHERE CompanyID IN ( SELECT ID FROM vwCompanies WHERE ID=" & User1.CompanyID & ")) AND Balance > 0 AND OrderStatus IN ('Taken', 'Shipped') And OrderType IN ('Regular', 'Quotation')"
                Dim sOrderStatus As String = Request.QueryString("OrderStatus")
                Dim sWhere As String = ""
                If String.IsNullOrEmpty(sOrderStatus) = False Then
                    sOrderStatus = sOrderStatus.ToUpper.Trim
                    If sOrderStatus = "PARTLYPAID" Then
                        sWhere = " AND o.Balance < o.GrandTotal "
                    ElseIf sOrderStatus = "UNPAID" Then
                        sWhere = " AND o.Balance = o.GrandTotal "
                    End If
                    Dim sDate As String = Request.QueryString("Date")
                    If String.IsNullOrEmpty(sDate) = False AndAlso IsDate(sDate) Then
                        sWhere = sWhere & " AND Month(o.OrderDate)=" & CType(sDate, Date).Month() & " AND Year(o.OrderDate)=" & CType(sDate, Date).Year() & " "
                    End If
                End If

                sSQL = "SELECT Distinct CurrencySymbol,o.CurrencyType,o.CurrencyTypeID FROM " & _
                          Database & "..vwOrders o " & _
                        "INNER JOIN " & Database & "..vwCurrencyTypes ct ON o.CurrencyTypeID=ct.ID " & _
                        " WHERE o.BillToCompanyID = " & User1.CompanyID & _
                        " AND o.Balance > 0 AND o.OrderStatus IN ('Taken', 'Shipped','Back-Ordered') And o.OrderType IN ('Regular', 'Quotation','Back-Order') " & sWhere & ""

                dtcurrency = DataAction.GetDataTable(sSQL)

                radcurrency.DataSource = dtcurrency
                radcurrency.DataBind()
                For j = 0 To dtcurrency.Rows.Count - 1
                    If dtcurrency(j)("CurrencySymbol").ToString.Trim() = "$" Then
                        radcurrency.SelectedIndex = j
                        Exit For
                    Else
                        radcurrency.SelectedIndex = 0
                    End If
                Next

                If dtcurrency.Rows.Count > 1 Then
                    lblfilter.Visible = True
                    radcurrency.Visible = True
                Else
                    lblfilter.Visible = False
                    radcurrency.Visible = False
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        'Protected Sub radcurrency_ItemsRequested(ByVal sender As Object, ByVal e As RadComboBoxItemsRequestedEventArgs)

        '    LoadCurrency()
        'End Sub

        'Protected Sub radcurrency_ItemDataBound(ByVal sender As Object, ByVal e As RadComboBoxItemEventArgs)
        '    'set the Text and Value property of every item
        '    'here you can set any other properties like Enabled, ToolTip, Visible, etc.
        '    e.Item.Text = (DirectCast(e.Item.DataItem, DataRowView))("CurrencyType").ToString()
        '    e.Item.Value = (DirectCast(e.Item.DataItem, DataRowView))("CurrencySymbol").ToString()
        'End Sub


        'Protected Sub radcurrency_DataBound(ByVal sender As Object, ByVal e As EventArgs)
        '    'set the initial footer label
        '    CType(radcurrency.Footer.FindControl("RadComboItemsCount"), Literal).Text = Convert.ToString(radcurrency.Items.Count)

        'End Sub

        'Protected Sub radcurrency_selected(ByVal sender As Object, ByVal e As RadComboBoxSelectedIndexChangedEventArgs) Handles radcurrency.SelectedIndexChanged
        '    Session("Currency") = e.Value
        '    Session("orderdt") = Nothing
        '    LoadOrders()
        '    grdOrderDetails.DataBind()
        '    ViewState("CHECKED_ITEMS") = Nothing
        '    setTotal("00.0", Session("Currency"))
        'End Sub

        Protected Sub grdOrderDetails_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grdOrderDetails.ItemCommand
            Dim sCommand As String
            Dim lOrderID As Long
            sCommand = Convert.ToString(e.CommandName)

            If sCommand = "ADDEditComments" Then
                lOrderID = Convert.ToInt64(e.CommandArgument)
                If lOrderID > 0 Then
                    BindComments(lOrderID)
                    lblOrderID.Text = Convert.ToString(lOrderID)
                    Hidden.Value = "true"
                    radGAReviewComments.VisibleOnPageLoad = True
                End If
            End If
        End Sub

        Protected Sub BindComments(ByVal lOrderID As Long)
            Try
                Dim sSQL As String
                Dim sGAReviewComments As String

                sSQL = "SELECT CompanyAdministratorComments FROM " & Database & "..vwOrders O where ID=" & lOrderID

                sGAReviewComments = Convert.ToString(DataAction.ExecuteScalar(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache))
                If Not String.IsNullOrEmpty(sGAReviewComments) Then
                    txtGAReviewComments.Text = sGAReviewComments
                Else
                    txtGAReviewComments.Text = ""
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try

        End Sub

        Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
            Dim lOrderID As Long
            Dim oOrderGE As AptifyGenericEntityBase
            Dim lPersonID As Long
            radGAReviewComments.VisibleOnPageLoad = False
            lOrderID = Convert.ToInt32(lblOrderID.Text)
            oOrderGE = AptifyApplication.GetEntityObject("Orders", lOrderID)
            If oOrderGE IsNot Nothing Then
                With oOrderGE
                    lPersonID = Convert.ToInt64(.GetValue("BillToID"))
                    .SetValue("CompanyAdministrator", User1.PersonID)
                    .SetValue("CompanyAdministratorComments", txtGAReviewComments.Text.Trim)
                End With

                If oOrderGE.Save Then
                    If String.IsNullOrEmpty(oOrderGE.GetValue("CompanyAdministratorComments").ToString()) = False Then
                        'Turn on Flag
                        EnableNotificationFlag(lOrderID)
                        'Send Email
                        SendTaskMail(lPersonID, lOrderID)
                    Else
                        'Turn on Flag
                        DisableNotificationFlag(lOrderID)
                    End If
                End If
            End If
            Hidden.Value = "true"
        End Sub
        'Neha changes for Issue:14972,04/29/13
        Private Sub EnableNotificationFlag(ByVal lOrderID As Long)
            Dim index As Long = -1
            Dim lblOrderID As Label = New Label
            For Each item As GridDataItem In grdOrderDetails.MasterTableView.Items
                lblOrderID = CType(item.FindControl("ID"), Label)
                index = lblOrderID.Text
                If index = lOrderID Then
                    Dim ImgFlag As Image = DirectCast(item.FindControl("ImgFlag"), Image)
                    ImgFlag.ImageUrl = PaymentNotificationImage
                    ImgFlag.Visible = True
                    Exit Sub
                End If
            Next
        End Sub
        'Neha changes for Issue:14972,04/29/13
        Private Sub DisableNotificationFlag(ByVal lOrderID As Long)
            Dim index As Long = -1
            Dim lblOrderID As Label = New Label
            For Each item As GridDataItem In grdOrderDetails.MasterTableView.Items
                lblOrderID = CType(item.FindControl("ID"), Label)
                index = lblOrderID.Text
                If index = lOrderID Then
                    Dim ImgFlag As Image = DirectCast(item.FindControl("ImgFlag"), Image)
                    ImgFlag.ImageUrl = PaymentNotificationImage
                    If ImgFlag.Visible = True Then
                        ImgFlag.Visible = False
                        Exit Sub
                    End If

                End If
            Next
        End Sub

        Protected Sub SendTaskMail(ByVal lPersonID As Long, ByVal oOrderID As Long)
            Try
                Dim lProcessFlowID As Long
                Dim sProcessFlow As String = "Notification to Review Order Comments"
                'Get the Process Flow ID to be used for sending the Downloadable Order Confirmation Email
                Dim sSQL As String = "SELECT ID FROM " & Database & "..vwProcessFlows WHERE Name='" & sProcessFlow & "'"
                Dim oProcessFlowID As Object = DataAction.ExecuteScalar(sSQL, IAptifyDataAction.DSLCacheSetting.UseCache)
                If oProcessFlowID IsNot Nothing AndAlso IsNumeric(oProcessFlowID) Then
                    lProcessFlowID = CLng(oProcessFlowID)
                    Dim context As New AptifyContext
                    context.Properties.AddProperty("OrderID", oOrderID)
                    context.Properties.AddProperty("AssignedByID", EBusinessGlobal.WebEmployeeID(Page.Application))
                    sSQL = "Select Top 1 Email1 from " & Database & "..vwEmployees where ID = " & Convert.ToInt64(AssignedTOID)
                    Dim sAssignedTOEmailID As String = DataAction.ExecuteScalar(sSQL, IAptifyDataAction.DSLCacheSetting.UseCache)
                    If String.IsNullOrEmpty(sAssignedTOEmailID) = True Then
                        ExceptionManagement.ExceptionManager.Publish(New Exception("Process flow Failed to send Notification. EmailID does not exists for AssignedTO Employee."))
                    End If
                    context.Properties.AddProperty("AssignedToID", Convert.ToInt64(AssignedTOID))
                    context.Properties.AddProperty("AssignedToEmailID", sAssignedTOEmailID)
                    context.Properties.AddProperty("TaskStatus", TaskStatus)
                    context.Properties.AddProperty("TaskPriority", TaskPriority)
                    context.Properties.AddProperty("TaskTypeID", TaskType)
                    context.Properties.AddProperty("TaskDescription", TaskDescription)
                    Dim oResult As ProcessFlowResult
                    oResult = ProcessFlowEngine.ExecuteProcessFlow(Me.AptifyApplication, lProcessFlowID, context)
                    If Not oResult.IsSuccess Then
                        ExceptionManagement.ExceptionManager.Publish(New Exception("Process flow Failed to send Notification. Please refer event handler for more details."))
                    End If
                Else
                    ExceptionManagement.ExceptionManager.Publish(New Exception("Message Template to send remove company Linkage Email is not found in the system."))
                End If

            Catch ex As ArgumentException
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub BtnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
            radGAReviewComments.VisibleOnPageLoad = False
        End Sub


        Protected Sub grdOrderDetails_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdOrderDetails.ItemCreated
            Dim chkMakePayment As CheckBox = DirectCast(e.Item.FindControl("chkMakePayment"), CheckBox)
            Dim lblOrderNo As HyperLink = DirectCast(e.Item.FindControl("lblOrderNo"), HyperLink)
            Dim txtPayAmt As TextBox = DirectCast(e.Item.FindControl("txtPayAmt"), TextBox)

            If chkMakePayment IsNot Nothing Then
                'Check in the ViewState
                If ViewState("CHECKED_ITEMS") IsNot Nothing Then
                    Dim orderdetails As Dictionary(Of Integer, Decimal) = DirectCast(ViewState("CHECKED_ITEMS"), Dictionary(Of Integer, Decimal))

                    Dim dataItem As DataRowView = DirectCast(e.Item.DataItem, System.Data.DataRowView)

                    If dataItem IsNot Nothing Then
                        Dim OrderNo As Long = dataItem("ID")
                        If orderdetails.ContainsKey(OrderNo) = True Then
                            chkMakePayment.Checked = True
                            txtPayAmt.Text = orderdetails(OrderNo)

                        Else
                            chkMakePayment.Checked = False
                        End If
                    End If
                End If
            End If
        End Sub
        'Neha changes for Issue:14972,04/29/13
        Private Sub SaveCheckedValues()
            'Dim orderdetails As New ArrayList
            Dim orderdetails As New Dictionary(Of Integer, Decimal)
            Dim total As Decimal = 0
            Dim index As Long = -1
            Dim lTotAmt As Decimal = 0.0
            Dim sSymCurr As String = ViewState("sCuurSymbol").ToString().Trim()
            Dim lblOrderID As Label = New Label
            For Each item As GridDataItem In grdOrderDetails.MasterTableView.Items
                lblOrderID = CType(item.FindControl("ID"), Label)
                index = lblOrderID.Text
                Dim result As Boolean = DirectCast(item.FindControl("chkMakePayment"), CheckBox).Checked
                Dim txtPayAmt As TextBox = DirectCast(item.FindControl("txtPayAmt"), TextBox)

                'Check in the ViewState
                If ViewState("CHECKED_ITEMS") IsNot Nothing Then
                    orderdetails = DirectCast(ViewState("CHECKED_ITEMS"), Dictionary(Of Integer, Decimal))
                End If

                'If result Then 'if checkbox is checked.
                'remove any existing entry
                If orderdetails.ContainsKey(index) Then
                    orderdetails.Remove(index)
                End If
                If result Then
                    Dim lPay As Decimal = Convert.ToDecimal(txtPayAmt.Text)
                    orderdetails.Add(index, lPay)
                End If
            Next
            For i = 0 To orderdetails.Count - 1
                total += orderdetails.Values(i)
            Next
            setTotal(total, sSymCurr)
            If orderdetails IsNot Nothing AndAlso orderdetails.Count > 0 Then
                ViewState("CHECKED_ITEMS") = orderdetails
            End If
        End Sub
        'There would be another method to loop through the current grid object and get currentPageTotal Value
        ' This method will be called from two places. One from checkbox and one from textbox lost focus event
        ' This method will call below method at last statement

        '#3 step
        'Optimization 
        ' Showing busy indicator
        'Neha changes for Issue:14972,04/29/13
        Private Sub CalculateTotal(ByVal currentPageTotal As Decimal)
            Dim sSymCurr As String = ViewState("sCuurSymbol").ToString().Trim()
            Dim index As Long = -1
            Dim orderdetails As New Dictionary(Of Integer, Decimal)
            Dim total As Decimal = 0.0
            Dim lblOrderID As Label = New Label
            If ViewState("CHECKED_ITEMS") IsNot Nothing Then
                orderdetails = DirectCast(ViewState("CHECKED_ITEMS"), Dictionary(Of Integer, Decimal))
            End If
            If orderdetails IsNot Nothing Then
                Dim iTotal As Int64 = orderdetails.Count
                For i = iTotal - 1 To 0 Step -1
                    For Each item As GridDataItem In grdOrderDetails.MasterTableView.Items
                        lblOrderID = CType(item.FindControl("ID"), Label)
                        index = lblOrderID.Text
                        Dim result As Boolean = DirectCast(item.FindControl("chkMakePayment"), CheckBox).Checked
                        If index = orderdetails.Keys(i) Then
                            orderdetails.Remove(index)
                        End If
                    Next
                Next
                For index = 0 To orderdetails.Count - 1
                    total += orderdetails.Values(index)
                Next
                total = total + currentPageTotal
                setTotal(total, sSymCurr)
            End If
        End Sub
        'Neha changes for Issue:14972,04/29/13
        Protected Sub grdOrderDetails_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdOrderDetails.DataBound
            Dim index As Long = -1
            Dim flag As Boolean = True
            Dim header As GridHeaderItem = TryCast(grdOrderDetails.MasterTableView.GetItems(GridItemType.Header)(0), GridHeaderItem)
            Dim chkAllMakePayment As CheckBox = DirectCast(header.FindControl("chkAllMakePayment"), CheckBox)
            Dim lblOrderID As Label = New Label
            For Each item As GridDataItem In grdOrderDetails.MasterTableView.Items
                lblOrderID = CType(item.FindControl("ID"), Label)
                index = lblOrderID.Text
                Dim result As Boolean = DirectCast(item.FindControl("chkMakePayment"), CheckBox).Checked
                If Not result Then
                    flag = False
                End If
            Next


            If flag = False Then
                chkAllMakePayment.Checked = False
            Else
                If grdOrderDetails.Items.Count <= 0 Then
                    chkAllMakePayment.Checked = False
                Else
                    chkAllMakePayment.Checked = True
                End If

            End If

            SaveCheckedValues()

        End Sub
        Private Sub setTotal(ByVal iSumTotalPayAmmount As Decimal, ByVal sSymCurr As String)

            If Not iSumTotalPayAmmount = 0 Then
                'suraj S Issue 16036 5/2/13, add comma for amount
                txtTotal.Text = sSymCurr & String.Format("{0:n2}", iSumTotalPayAmmount)
            Else
                txtTotal.Text = sSymCurr & "0.00"
            End If
        End Sub


        Private Function getTotal(ByRef row As GridDataItem) As Decimal
            Dim iSumTotalPayAmmount As Decimal
            Dim iTotalAmmount As Decimal
            Dim txtPayAmt As TextBox = DirectCast(row.FindControl("txtPayAmt"), TextBox)
            iTotalAmmount = Decimal.Parse(txtPayAmt.Text).ToString("0.00")
            iSumTotalPayAmmount = iSumTotalPayAmmount + iTotalAmmount
            Return iSumTotalPayAmmount
        End Function


        Private Sub AddExpression()
            Dim ExpOrderSort As New GridSortExpression
            ExpOrderSort.FieldName = "Name"
            ExpOrderSort.SetSortOrder("Ascending")
            grdOrderDetails.MasterTableView.SortExpressions.AddSortExpression(ExpOrderSort)
        End Sub



        Protected Sub radcurrency_selected(ByVal sender As Object, ByVal e As System.EventArgs) Handles radcurrency.SelectedIndexChanged
            Dim sSQL As String = "SELECT CurrencySymbol FROM " & _
                                      Database & "..vwCurrencyTypes where ID=" & radcurrency.SelectedValue().Trim()
            Dim dt = DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                sCuurSymbol = dt.Rows(0)("CurrencySymbol").ToString().Trim
                ViewState("Currency") = sCuurSymbol
            End If
            ViewState.Remove("orderdt")
            LoadOrders()
            grdOrderDetails.DataBind()
            ViewState("CHECKED_ITEMS") = Nothing
            setTotal("00.0", ViewState("Currency"))
        End Sub
    End Class
End Namespace
