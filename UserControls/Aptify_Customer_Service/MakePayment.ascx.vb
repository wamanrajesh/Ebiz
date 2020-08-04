''Aptify e-Business 5.5.1, July 2013
Option Explicit On

Imports Aptify.Framework.Application
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports System.Data
Imports System.Collections.Generic
Imports Telerik.Web.UI
Imports Aptify.Applications.Accounting
Imports Aptify.Applications.OrderEntry





Namespace Aptify.Framework.Web.eBusiness.CustomerService
    Partial Class MakePaymentControl
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_ORDER_CONFIRMATION_PAGE As String = "OrderConfirmationURL"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "MakePayment"
        Protected Const ATTRIBUTE_DATATABLE_ORDERS As String = "dtOrders"

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
#End Region


        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1))
            Response.Cache.SetCacheability(HttpCacheability.NoCache)
            Response.Cache.SetNoStore()
            'set control properties from XML file if needed
            SetProperties()

            Try
                If Not IsPostBack Then
                    LoadOrders()
                    'Suraj Issue 14450 3/22/13 ,this method use to apply the odrering of rad grid first column
                    AddExpression()
                    'HP Issue#9092: for page refresh purposes, display payment message
                    If lblNoRecords.Visible Then
                        If Request.QueryString("msg") IsNot Nothing Then
                            lblMessage.Text = "Payment was made!"
                            lblMessage.ForeColor = Drawing.Color.DarkGreen
                            lblMessage.Visible = True
                            paymentMade.Visible = True
                        End If
                    Else
                        If Request.QueryString("msg") IsNot Nothing Then
                            lblMessage.Text = Request.QueryString("msg").ToString
                            lblMessage.ForeColor = Drawing.Color.DarkGreen
                            lblMessage.Visible = True
                            paymentMade.Visible = True
                        End If
                    End If

                    CreditCard.LoadCreditCardInfo()
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub


        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(OrderConfirmationURL) Then
                OrderConfirmationURL = Me.GetLinkValueFromXML(ATTRIBUTE_ORDER_CONFIRMATION_PAGE)
                If String.IsNullOrEmpty(OrderConfirmationURL) Then
                    'Navin Prasad Issue 11032
                    Me.grdMain.Columns.RemoveAt(0)
                    grdMain.Columns.Insert(0, New Telerik.Web.UI.GridBoundColumn())
                    With DirectCast(grdMain.Columns(0), Telerik.Web.UI.GridBoundColumn)
                        .DataField = "ID"
                        .HeaderText = "Order #"
                        .ItemStyle.ForeColor = Drawing.Color.Blue
                        .ItemStyle.Font.Underline = True
                    End With
                    Me.grdMain.ToolTip = "OrderConfirmationURL property has not been set."
                Else
                    Dim hlink As Telerik.Web.UI.GridHyperLinkColumn = CType(grdMain.Columns(0), Telerik.Web.UI.GridHyperLinkColumn)
                    hlink.DataNavigateUrlFormatString = Me.OrderConfirmationURL & "?ID={0}"
                    'DirectCast(grdMain.Columns(0), HyperLinkColumn).DataNavigateUrlFormatString = Me.OrderConfirmationURL & "?ID={0}"
                End If
            Else
                Dim hlink As Telerik.Web.UI.GridHyperLinkColumn = CType(grdMain.Columns(0), Telerik.Web.UI.GridHyperLinkColumn)
                hlink.DataNavigateUrlFormatString = Me.OrderConfirmationURL & "?ID={0}"
                'DirectCast(grdMain.Columns(0), HyperLinkColumn).DataNavigateUrlFormatString = Me.OrderConfirmationURL & "?ID={0}"
            End If
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


        Private Sub LoadOrders()
            Dim dt As Data.DataTable
            Dim sSQL As String

            Try
                If ViewState(ATTRIBUTE_DATATABLE_ORDERS) IsNot Nothing Then
                    grdMain.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_ORDERS), DataTable)
                    grdMain.DataBind()
                    Exit Sub
                End If
                ' get a list of the orders w/ a balance for the current user
                'Suraj S Issue 15195 ,4/8/13 ,amount field provide a comma eg:1000 to 1,000
                sSQL = "SELECT o.ID,OrderDate,CONVERT(VARCHAR,GrandTotal,1)As GrandTotal,CONVERT(VARCHAR,Balance,1) As Balance," & _
                       "CurrencySymbol,NumDigitsAfterDecimal FROM " & _
                       Database & "..vwOrders o " & _
                       "INNER JOIN " & Database & _
                       "..vwCurrencyTypes ct ON o.CurrencyTypeID=ct.ID " & _
                       "WHERE BillToID=" & User1.PersonID & " AND " & _
                       "Balance > 0"
                dt = DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)
                'Suraj Issue 15287 4/9/13, if the grid dont have any record then grid should visible and it should show "No records " msg
                grdMain.DataSource = dt
                grdMain.DataBind()
                ViewState(ATTRIBUTE_DATATABLE_ORDERS) = dt
                cmdPay.Visible = (dt.Rows.Count > 0)
                'Anil B change for 10737 on 13/03/2013
                'Set Credit Card ID to load property form Navigation Config
                CreditCard.Visible = (dt.Rows.Count > 0)
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub


        Private Class PayInfo
            Public OrderID As Long
            Public PayAmount As Decimal
            Public Balance As Decimal
        End Class

        Private Sub cmdPay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPay.Click
            ' Save a payment to the database with the information the user provided
            Dim i As Integer
            Dim arPay() As PayInfo
            Dim iColPayAmt As Integer
            Dim iColOrderID As Integer
            Dim iColBalance As Integer
            Dim iColNumDigits As Integer
            Dim lblPay As New WebControls.Label
            Dim hlkOrderID As WebControls.HyperLink
            Dim txtPay As New HtmlControls.HtmlInputText


            Try
                'Anil B change for 10254 on 29/03/2013
                ReDim arPay(0)

                lblMessage.Visible = False

                For i = 0 To grdMain.Columns.Count - 1
                    If grdMain.Columns(i).HeaderText = "Pay Amount" Then
                        iColPayAmt = i
                    ElseIf grdMain.Columns(i).HeaderText = "Order #" Then
                        iColOrderID = i
                    ElseIf grdMain.Columns(i).HeaderText = "Actual Balance" Then
                        iColBalance = i
                    ElseIf grdMain.Columns(i).HeaderText = "Currency Digits" Then
                        iColNumDigits = i
                    End If
                Next
                'Navin Prasad Issue 11032
                For i = 0 To grdMain.Items.Count - 1
                    ' loop through each item in the grid to see if it was checked,
                    ' and store the order id, and amount
                    'Navin Prasad Issue 11032
                    ''RashmiP, Issue 11147, remove currency symbol from string. Because IsNumeric is not validating Currency symbol except $.
                    lblPay = grdMain.Items(i).Cells(iColBalance - 2).Controls(1)
                    txtPay = grdMain.Items(i).Cells(iColPayAmt + 2).Controls(3)

                    If txtPay.Value.Trim.Length > 0 Then
                        If IsNumeric(txtPay.Value) Then
                            'HP Issue#8941: only add payments where the user has put a value greater than zero
                            '               allowing payment for selected items
                            If CDec(txtPay.Value) > 0 Then
                                ReDim Preserve arPay(UBound(arPay) + 1)
                                arPay(UBound(arPay) - 1) = New PayInfo
                                With arPay(UBound(arPay) - 1)
                                    'Navin Prasad Issue 11032
                                    Dim lblNumDigits As Label = grdMain.Items(i).FindControl("lblNumDigitsAfterDecimal")
                                    Dim lblBal As Label = grdMain.Items(i).FindControl("lblBalance")
                                    hlkOrderID = grdMain.Items(i).Cells(iColOrderID + 2).Controls(0)
                                    .OrderID = hlkOrderID.Text
                                    .PayAmount = Math.Round(CDec(txtPay.Value), _
                                                              CInt(lblNumDigits.Text))
                                    .Balance = lblBal.Text
                                End With
                            End If
                        Else
                            lblError.Text = "Values entered must be valid currency quantities."
                            lblError.Visible = True
                            Exit Sub
                        End If
                    End If
                Next

                ' Now, go through and make sure that each item is validated correctly
                lblError.Visible = False
                If UBound(arPay) > 0 Then
                    For i = 0 To UBound(arPay) - 1
                        With arPay(i)
                            If .PayAmount <= 0 Then
                                lblError.Text = "All payments must be greater than zero"
                                lblError.Visible = True
                                Exit Sub
                            End If
                            If .PayAmount > .Balance Then
                                lblError.Text = "All payments must be less than the balance due"
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
                        'HP - Issue 8264, virbiage is invalid when no information is being listes
                        'lblMessage.Text = "Payment was made! Your updated order information is shown below."
                        'lblMessage.Text = "Payment successfull !"
                        'lblMessage.Visible = True
                        'Anil B change for 10737 on 13/03/2013
                        'Set Credit Card ID to load property form Navigation Config
                        CreditCard.CCNumber = ""
                        CreditCard.CCExpireDate = ""
                        CreditCard.CCSecurityNumber = ""
                        LoadOrders()
                        'HP Issue#9092: need to bypass page form re-posting when refresh button is pressed causing a payment each time
                        Response.Redirect(Request.Path & "?msg=Payment was made!  Your updated order information is shown below.", False)
                    Else
                        lblError.Text = "An error took place while processing your payment"
                        lblError.Visible = True
                    End If
                Else
                    lblError.Visible = True
                    'HP - Issue 8264, replace the default error message if payment amount is blank
                    'lblError.Text = "Please select a payment for at least one order"
                    lblError.Text = "All payments must be greater than zero"
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Function PostPayment(ByVal arPay() As PayInfo) As Boolean
            ' post the payment to the database using the CGI GE
            Dim oPayment As AptifyGenericEntityBase
            Dim oOrders As OrdersEntity
            Dim i As Integer

            Try
                oOrders = ShoppingCart1.GetOrderObject(Session, Page.User, Application)
                oPayment = AptifyApplication.GetEntityObject("Payments", -1)

                oPayment.SetValue("EmployeeID", EBusinessGlobal.WebEmployeeID(Page.Application))
                oPayment.SetValue("PersonID", User1.PersonID)

                ''Set company value based on the selected payment type
                If (CreditCard.EnablePaymentTypeSelection = True AndAlso CType(CreditCard.FindControl("rbCompanyPayment"), RadioButton).Checked = True) OrElse (CreditCard.EnablePaymentTypeSelection = False AndAlso User1.CompanyID > 0) Then
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
                'Anil B Issue 10254 on 20/04/2013
                'Set Sequirity number to payment object
                'oPayment.SetValue("CCSecurityNumber", CreditCard.CCSecurityNumber)
                oPayment.SetValue("PaymentLevelID", User1.GetValue("GLPaymentLevelID"))
                oPayment.SetValue("Comments", "Created through the CGI e-Business Suite")
                ' to have an automatic conversion of quotes to regular
                ' orders, set the Convert flag to true.
                oPayment.SetAddValue("_xConvertQuotesToRegularOrder", "1")
                'Anil B Issue 10254 on 20/04/2013
                'creates SPM
                If oPayment.Fields("PaymentInformationID").EmbeddedObjectExists Then
                    Dim oOrderPayInfo As PaymentInformation = DirectCast(oPayment.Fields("PaymentInformationID").EmbeddedObject, PaymentInformation)
                    oOrderPayInfo.CreditCardSecurityNumber = CreditCard.CCSecurityNumber
                    oOrderPayInfo.SetValue("SaveForFutureUse", CreditCard.SaveCardforFutureUse)
                    oOrderPayInfo.SetValue("CCPartial", CreditCard.CCPartial)
                End If
                For i = 0 To UBound(arPay) - 1
                    With oPayment.SubTypes("PaymentLines").Add
                        .SetValue("Amount", arPay(i).PayAmount)
                        .SetValue("OrderID", arPay(i).OrderID)
                    End With
                Next
                Return oPayment.Save(False)
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Function
        'Suraj issue 14450 2/12/13 date time filtering
        Protected Sub grdMain_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdMain.ItemDataBound
            Dim dateColumns As New List(Of String)
            'Add datecolumn uniqueName in list for Date format
            dateColumns.Add("GridDateTimeColumnOrderDate")
            CommonMethods.FormatedDateOnGrid(dateColumns, e.Item)
            'Suraj Issue 14450 3/22/13 ,we provide a tool tip for DatePopupButton as well as the GridDateTimeColumnStartDate textbox   
            If TypeOf e.Item Is GridFilteringItem Then
                Dim filterItem As GridFilteringItem = DirectCast(e.Item, GridFilteringItem)
                Dim gridDateTimeColumnStartDate As RadDatePicker = DirectCast(filterItem("GridDateTimeColumnOrderDate").Controls(0), RadDatePicker)
                gridDateTimeColumnStartDate.ToolTip = "Enter a filter date"
                gridDateTimeColumnStartDate.DatePopupButton.ToolTip = "Select a filter date"
            End If
        End Sub

        Protected Sub grdMain_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdMain.NeedDataSource
            If ViewState(ATTRIBUTE_DATATABLE_ORDERS) IsNot Nothing Then
                grdMain.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_ORDERS), DataTable)
                Exit Sub
            End If
        End Sub

        Protected Sub grdMain_PageIndexChanging(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageChangedEventArgs) Handles grdMain.PageIndexChanged
            grdMain.CurrentPageIndex = e.NewPageIndex
            If ViewState(ATTRIBUTE_DATATABLE_ORDERS) IsNot Nothing Then
                grdMain.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_ORDERS), DataTable)
                Exit Sub
            End If
        End Sub
        'Suraj Issue 14450 3/22/13 ,if the grid load first time By default the sorting will be Ascending for column Forum 
        Private Sub AddExpression()
            Dim expression1 As New GridSortExpression
            expression1.FieldName = "ID"
            expression1.SetSortOrder("Ascending")
            grdMain.MasterTableView.SortExpressions.AddSortExpression(expression1)
        End Sub

    End Class
End Namespace
