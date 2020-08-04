'Aptify e-Business 5.5.1, July 2013
Imports Telerik.Web.UI
Imports System.Data
Imports System.Collections.Generic


Namespace Aptify.Framework.Web.eBusiness.CustomerService
    Partial Class OrderHistory_Admin
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_ORDER_CONFIRMATION_PAGE As String = "OrderConfirmationURLAdmin"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "OrderHistory_Admin"
        Protected Const ATTRIBUTE_ORDERHISTORYADMIN_VIEWSTATE As String = "OrderHistoryAdminsdt"
        Protected Const ATTRIBUTE_LOGIN_PAGE As String = "LoginPage" 'Added by Sandeep for Issue 15051 on 12/03/2013

        Dim flag As Boolean = False
        Dim iOldIndex As Integer = -1
        Dim iNewindex As Integer = -1


#Region "OrderHistory Specific Properties"
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

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(OrderConfirmationURL) Then
                OrderConfirmationURL = Me.GetLinkValueFromXML(ATTRIBUTE_ORDER_CONFIRMATION_PAGE)
                If String.IsNullOrEmpty(OrderConfirmationURL) Then
                    Me.grdMain.Enabled = False
                    Me.grdMain.ToolTip = "OrderConfirmationURL property has not been set."
                End If
            End If
            'Added by Sandeep for Issue 15051 on 12/03/2013
            If String.IsNullOrEmpty(LoginPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                LoginPage = Me.GetLinkValueFromXML(ATTRIBUTE_LOGIN_PAGE)
            End If


        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                ''LoadGrid()
                SetProperties()
                'flag = False
                If Not IsPostBack Then
                    'Suraj issue 14877 3/1/13 ,this method use to apply the odrering of rad grid first column
                    AddExpression()
                    'Session("Value") = ""
                    'Session("ProdValue") = ""
                    'Session("OldValue") = -1
                    'LoadPersons()
                    'LoadProdCategory()
                    LoadGrid()
                End If
                If User1.UserID < 0 Then
                    Response.Redirect(LoginPage) 'Added by Sandeep for Issue 15051 on 12/03/2013
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
           
        End Sub
    
        Private Sub LoadGrid()
            'Anil B for issue 14343 on 03/04/2013
            'Set Sorting option for detail grid
            AddDetailExpression()
            ' load the grid with the user's past ordere history
            Dim sSQL As String, dt As Data.DataTable
            Dim sOrderStatus As String = ""
            Dim sDate As String = ""
            Try
                'Suraj issue 14877 2/20/13 , check the view state is nothing or not if the page load first time viewstate will be nothing but after bostback view state will conatin the datatable
                If ViewState(ATTRIBUTE_ORDERHISTORYADMIN_VIEWSTATE) Is Nothing Then
                    Dim sWhere As String = ""
                    If Request.QueryString("OrderStatus") IsNot Nothing AndAlso Request.QueryString("OrderStatus").ToString() <> "" Then
                        sOrderStatus = Request.QueryString("OrderStatus")
                    End If


                    If String.IsNullOrEmpty(sOrderStatus) = False Then
                        sOrderStatus = sOrderStatus.ToUpper.Trim

                        If sOrderStatus = "PAID" Then
                            sWhere = " AND o.Balance = 0 "
                            If Request.QueryString("Date") IsNot Nothing AndAlso Request.QueryString("Date").ToString() <> "" Then
                                sDate = Request.QueryString("Date")
                            End If
                            If String.IsNullOrEmpty(sDate) = False AndAlso IsDate(sDate) Then
                                sWhere = sWhere & " AND Month(o.OrderDate)=" & CType(sDate, Date).Month() & " AND Year(o.OrderDate)=" & CType(sDate, Date).Year() & " AND o.OrderStatus IN ('Taken', 'Shipped') AND o.OrderType IN ('Regular', 'Quotation') "
                            End If
                        End If

                    End If
                    'Suraj S Issue 15195 ,4/8/13 ,amount field provide a comma eg:1000 to 1,000
                    'Sheetal issue 22102
                    sSQL = "select distinct o.BillToCompany,o.ID,o.OrderDate,o.PayType,CONVERT(VARCHAR,o.balance,1) As 'balance',ct.CurrencySymbol,ct.NumDigitsAfterDecimal,o.CurrencyType,o.ShipDate,CONVERT(VARCHAR,CALC_GrandTotal,1)as 'CALC_GrandTotal',o.OrderStatus,o.ShipType,o.ShipTrackingNum,o.BillToName,o.ShipToName " & _
                        ",o.OrderParty ,Paymentparty= CASE WHEN o.Id in (SELECT distinct vo.Id from " & Database & "..vwOrders vo left outer join " & Database & "..vwPaymentPartyDetail vp on vp.OrderId=vo.id where vo.BillToCompanyID= " & User1.CompanyID & " or vp.PAYMENTPARTY='Company' and vp.PAYMENTPARTY='Individual' GROUP BY vo.Id  having COUNT(distinct(vp.PAYMENTPARTY)) > 1 )   THEN 'Company/Individual' ELSE ppd.Paymentparty END  " & _
                           " FROM " & Database & _
                           "..vwOrders o  INNER JOIN  " & Database & "..vwCurrencyTypes ct ON o.CurrencyTypeID=ct.ID " & _
                           " left outer join vwPaymentPartyDetail ppd " & _
                            " on ppd.OrderId = o.ID " & _
                           "where (o.BillToCompanyID= " & User1.CompanyID & ")  " & sWhere


                    'If (Session("Value") <> "" AndAlso Session("Value") <> "0") Then
                    '    sSQL = sSQL & " And o.BillToID=" + Session("Value").ToString()
                    'End If
                    'If (Session("ProdValue") <> "" AndAlso Session("ProdValue") <> "0") Then

                    '    sSQL = sSQL & " And o.Line1_ProductCategory='" + Session("ProdValue").ToString() + "'"
                    'End If
                    sSQL = sSQL & " ORDER BY o.OrderDate DESC, o.ID DESC"

                    dt = DataAction.GetDataTable(sSQL)
                    If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                        'Dim hlink As GridHyperLinkColumn = CType(grdMain.Columns(0), GridHyperLinkColumn)
                        'hlink.DataNavigateUrlFormatString = Me.OrderConfirmationURL & "?ID={0}"
                        Dim dcolUrl As DataColumn = New DataColumn()
                        dcolUrl.Caption = "LinkUrl"
                        dcolUrl.ColumnName = "LinkUrl"

                        dt.Columns.Add(dcolUrl)
                        If dt.Rows.Count > 0 Then

                            For Each rw As DataRow In dt.Rows
                                rw("LinkUrl") = Me.OrderConfirmationURL + "?ID=" + rw("ID").ToString
                            Next
                        End If
                    End If
                    grdMain.DataSource = dt
                    'Anil B for issue 14343 on 03/04/2013
                    'Remove DataBinding
                    'grdMain.DataBind()
                    'Suraj issue 14877 2/15/13 , if when page first time load here we store the datatable in to a viewstate
                    ViewState(ATTRIBUTE_ORDERHISTORYADMIN_VIEWSTATE) = dt
                Else
                    'Suraj issue 14877 2/20/13 , after postback viewstate will assign for gridview
                    grdMain.DataSource = ViewState(ATTRIBUTE_ORDERHISTORYADMIN_VIEWSTATE)
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub


        'Private Sub LoadGridCombo()
        '    ' load the grid with the user's past ordere history
        '    Dim sSQL As String, dt As Data.DataTable

        '    Try
        '        sSQL = "select o.BillToCompany,o.ID,o.OrderDate,o.ShipDate,convert(decimal(28,2),o.CALC_GrandTotal)as 'CALC_GrandTotal',o.OrderStatus,o.ShipType,o.ShipTrackingNum,o.BillToName " & _
        '               " FROM " & Database & _
        '               "..vwOrders o where o.BillToCompanyID= " & User1.CompanyID

        '        If (Session("Value") <> "" AndAlso Session("Value") <> "0") Then
        '            sSQL = sSQL & " And o.BillToID=" + Session("Value").ToString()
        '        End If
        '        If (Session("ProdValue") <> "" AndAlso Session("ProdValue") <> "0") Then

        '            sSQL = sSQL & " And o.Line1_ProductCategory='" + Session("ProdValue").ToString() + "'"
        '        End If
        '        sSQL = sSQL & " union select o.BillToCompany,o.ID,o.OrderDate,o.ShipDate,convert(decimal(28,2),o.CALC_GrandTotal)as 'CALC_GrandTotal',o.OrderStatus,o.ShipType,o.ShipTrackingNum,o.BillToName " & _
        '                " FROM " & Database & _
        '                "..vwOrders as o  inner join " & Database & "..vwPersons as vp on o.BillToID=vp.ID " & _
        '                "where vp.CompanyID= " & User1.CompanyID
        '        If (Session("Value") <> "" AndAlso Session("Value") <> "0") Then
        '            sSQL = sSQL & " And o.BillToID=" + Session("Value").ToString()
        '        End If
        '        If (Session("ProdValue") <> "" AndAlso Session("ProdValue") <> "0") Then
        '            sSQL = sSQL & " And o.Line1_ProductCategory='" + Session("ProdValue").ToString() + "'"
        '        End If
        '        sSQL = sSQL & " ORDER BY o.OrderDate DESC, o.ID DESC"
        '        dt = DataAction.GetDataTable(sSQL)
        '        'Dim hlink As GridHyperLinkColumn = CType(grdMain.Columns(0), GridHyperLinkColumn)
        '        'hlink.DataNavigateUrlFormatString = Me.OrderConfirmationURL & "?ID={0}"
        '        Dim dcolUrl As DataColumn = New DataColumn()
        '        dcolUrl.Caption = "LinkUrl"
        '        dcolUrl.ColumnName = "LinkUrl"

        '        dt.Columns.Add(dcolUrl)
        '        If dt.Rows.Count > 0 Then

        '            For Each rw As DataRow In dt.Rows
        '                rw("LinkUrl") = Me.OrderConfirmationURL + "?ID=" + rw("ID").ToString
        '            Next
        '        End If

        '        grdMain.DataSource = dt
        '        ''grdMain.DataBind()
        '    Catch ex As Exception
        '        Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        '    End Try
        'End Sub


        'Private Sub LoadPersons()
        '    ' load the grid with the user's past ordere history
        '    Dim sSQL As String, dtPersons As Data.DataTable

        '    Try
        '        sSQL = "select ID,FirstLast,Title from " & Database & "..vwPersons where CompanyID= " & User1.CompanyID

        '        dtPersons = DataAction.GetDataTable(sSQL)

        '        RadComboBox1.DataSource = dtPersons


        '        RadComboBox1.DataBind()
        '        RadComboBox1.Items.Insert(0, New RadComboBoxItem("", "0"))
        '    Catch ex As Exception
        '        Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        '    End Try
        'End Sub

        'Private Sub LoadProdCategory()
        '    ' load the grid with the user's past ordere history
        '    Dim sSQL As String, dtProdCategory As Data.DataTable

        '    Try
        '        sSQL = "select distinct ProductCategory from  " & Database & "..vwproducts "

        '        dtProdCategory = DataAction.GetDataTable(sSQL)

        '        RadComboBox2.DataSource = dtProdCategory
        '        RadComboBox2.DataBind()
        '        RadComboBox2.Items.Insert(0, New RadComboBoxItem("", "0"))
        '    Catch ex As Exception
        '        Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        '    End Try
        'End Sub
        'Suraj issue 14450 2/12/13 date time filtering
        Protected Sub grdMain_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdMain.ItemDataBound
            'Anil B for issue 14343 on 03/04/2013
            'If open detail grid do not need to call date formate function
            If TypeOf e.Item Is GridDataItem AndAlso e.Item.OwnerTableView.Name <> "ChildGrid" Then
                Dim dateColumns As New List(Of String)
                'Add datecolumn uniqueName in list for Date format
                dateColumns.Add("GridDateTimeColumnOrderDate")
                CommonMethods.FormatedDateOnGrid(dateColumns, e.Item)
            End If
        End Sub


        Protected Sub grdMain_DetailTableDataBind(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridDetailTableDataBindEventArgs) Handles grdMain.DetailTableDataBind
            AddDetailExpression()
            Dim sSQL As String, dtDeatil As Data.DataTable
            Dim dataItem As Telerik.Web.UI.GridDataItem = CType(e.DetailTableView.ParentItem, Telerik.Web.UI.GridDataItem)
            Dim ss As Boolean = e.DetailTableView.ParentItem.Expanded
            Dim Id As Integer = dataItem.GetDataKeyValue("ID")
            'Anil B for issue 14343 on 03/04/2013
            'Updated query for currency symbol
            'Suraj S Issue 16036 ,5/2/13 ,amount field provide a comma eg:1000 to 1,000
            sSQL = "select OD.ID,OD.Product,OD.ProductType,ct.CurrencySymbol,OD.OrderCurrencyTypeID,OD.Description,OD.Quantity,CONVERT(VARCHAR,OD.Price,1) As 'Price',OD.Discount,ct.NumDigitsAfterDecimal from " & Database & "..vwOrderDetails OD INNER JOIN " & Database & "..vwCurrencyTypes ct ON OD.OrderCurrencyTypeID=ct.ID where OrderID = " & Id
            
            dtDeatil = DataAction.GetDataTable(sSQL)
            e.DetailTableView.DataSource = dtDeatil
            'If (e.DetailTableView.ExpandCollapseColumn.Display = False) Then
            '    e.DetailTableView.ExpandCollapseColumn.Display = True
            '    grdMain.MasterTableView.Items(e.DetailTableView.ParentItem.ItemIndex).Expanded = False
            'Else
            '    e.DetailTableView.ExpandCollapseColumn.Display = False
            '    grdMain.MasterTableView.Items(e.DetailTableView.ParentItem.ItemIndex).Expanded = True
            'End If

            'Session("OldValue") = iNewindex
        End Sub

        'Protected Sub RadGrid1_ItemCommand(ByVal source As Object, ByVal e As GridCommandEventArgs) Handles grdMain.ItemCommand
        '    If e.CommandName = RadGrid.ExpandCollapseCommandName And TypeOf e.Item Is GridDataItem Then
        '        DirectCast(e.Item, GridDataItem).ChildItem.FindControl("InnerContainer").Visible = Not e.Item.Expanded
        '        Dim sSQL As String, dtDeatil As Data.DataTable
        '        Dim Id As Integer =
        '        sSQL = "select ID,Product,ProductType,Description,Quantity,Price,Discount from " & Database & "..vwOrderDetails where OrderID = " & Id
        '        dtDeatil = DataAction.GetDataTable(sSQL)

        '    End If
        'End Sub

        'Protected Sub RadGrid1_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles grdMain.ItemCreated
        '    If TypeOf e.Item Is GridNestedViewItem Then
        '        ''e.Item.FindControl("InnerContainer").Visible = (DirectCast(e.Item, GridNestedViewItem)).ParentItem.Expanded
        '    End If
        'End Sub

        'Protected Sub RadGrid1_ItemCommand(ByVal source As Object, ByVal e As GridCommandEventArgs) Handles grdMain.ItemCommand
        '    If e.CommandName = RadGrid.ExpandCollapseCommandName And TypeOf e.Item Is GridDataItem Then
        '        DirectCast(e.Item, GridDataItem).ChildItem.FindControl("InnerContainer").Visible = Not e.Item.Expanded
        '    End If
        'End Sub

       
        Protected Sub RadComboBox1_ItemDataBound(ByVal sender As Object, ByVal e As RadComboBoxItemEventArgs)
            'set the Text and Value property of every item
            'here you can set any other properties like Enabled, ToolTip, Visible, etc.
            AddDetailExpression()
            e.Item.Text = (DirectCast(e.Item.DataItem, DataRowView))("FirstLast").ToString()
            e.Item.Value = (DirectCast(e.Item.DataItem, DataRowView))("ID").ToString()
        End Sub
        'Protected Sub RadComboBox2_ItemDataBound(ByVal sender As Object, ByVal e As RadComboBoxItemEventArgs)
        '    'set the Text and Value property of every item
        '    'here you can set any other properties like Enabled, ToolTip, Visible, etc.
        '    e.Item.Text = (DirectCast(e.Item.DataItem, DataRowView))("ProductCategory").ToString()
        '    e.Item.Value = (DirectCast(e.Item.DataItem, DataRowView))("ProductCategory").ToString()
        'End Sub
        'Protected Sub RadComboBox1_DataBound(sender As Object, e As EventArgs)
        '    'set the initial footer label
        '    CType(RadComboBox1.Footer.FindControl("RadComboItemsCount"), Literal).Text = Convert.ToString(RadComboBox1.Items.Count)

        'End Sub
        'Protected Sub RadComboBox2_DataBound(sender As Object, e As EventArgs)
        '    'set the initial footer label
        '    CType(RadComboBox2.Footer.FindControl("RadComboItemsCount1"), Literal).Text = Convert.ToString(RadComboBox2.Items.Count)
        'End Sub

        'Protected Sub RadComboBox1_ItemsRequested(ByVal sender As Object, ByVal e As RadComboBoxItemsRequestedEventArgs)
        '    'Dim sql As String = "SELECT * from Customers WHERE ContactName LIKE '" + e.Text + "%'"

        '    'SessionDataSource1.SelectCommand = sql
        '    'RadComboBox1.DataBind()
        '    LoadPersons()
        'End Sub
        'Protected Sub RadComboBox2_ItemsRequested(ByVal sender As Object, ByVal e As RadComboBoxItemsRequestedEventArgs)
        '    'Dim sql As String = "SELECT * from Customers WHERE ContactName LIKE '" + e.Text + "%'"

        '    'SessionDataSource1.SelectCommand = sql
        '    'RadComboBox1.DataBind()
        '    LoadProdCategory()
        'End Sub
        'Protected Sub RadComboBox1_selected(ByVal sender As Object, ByVal e As RadComboBoxSelectedIndexChangedEventArgs) Handles RadComboBox1.SelectedIndexChanged
        '    'LoadGrid(e.Value)
        '    Session("Value") = e.Value
        '    'grdMain.Rebind()
        '    LoadGrid()
        'End Sub
        'Protected Sub RadComboBox2_selected(ByVal sender As Object, ByVal e As RadComboBoxSelectedIndexChangedEventArgs) Handles RadComboBox2.SelectedIndexChanged
        '    'LoadGrid(e.Value)
        '    Session("ProdValue") = e.Value
        '    'grdMain.Rebind()
        '    LoadGrid()
        'End Sub
        Protected Function GetFormattedCurrency(ByVal Container As Object, ByVal sField As String) As String
            Dim sCurrencySymbol As String
            Dim iNumDecimals As Integer
            Dim sCurrencyFormat As String
            Dim sCurrencyValue As String
            Dim sCurrencyFormateForNegative As String

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

                'Anil B for 14343 on 20-03-2013
                'Add condition to handle negative value
                ' format the string using the currency format created
                If IsNumeric(Container.DataItem(sField)) AndAlso Container.DataItem(sField) >= 0 Then
                    Return String.Format(sCurrencyFormat, Container.DataItem(sField))
                Else
                    sCurrencyValue = Convert.ToString((Container.DataItem(sField)))
                    sCurrencyValue = sCurrencyValue.Replace("-", "")
                    sCurrencyFormateForNegative = "(" & String.Format(sCurrencyFormat, sCurrencyValue) & ")"
                    Return sCurrencyFormateForNegative
                End If

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
        'Suraj issue 14877 2/20/13 ,if the grid load first time By default the sorting will be Ascending for column Order #
        Private Sub AddExpression()
            Dim expression1 As New GridSortExpression
            expression1.FieldName = "ID"
            expression1.SetSortOrder("Ascending")
            grdMain.MasterTableView.SortExpressions.AddSortExpression(expression1)
        End Sub
        'Anil B for issue 14343 on 03/04/2013
        'Add function to enable sorting on detail grid
        Private Sub AddDetailExpression()
            Dim DetailExpression As New GridSortExpression
            DetailExpression.FieldName = "Product"
            DetailExpression.SetSortOrder("Ascending")
            grdMain.MasterTableView.DetailTables.Item(0).SortExpressions.AddSortExpression(DetailExpression)
        End Sub
        ''Issue 16807, Rashmi P
        Protected Sub grdMain_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdMain.NeedDataSource
            If ViewState(ATTRIBUTE_ORDERHISTORYADMIN_VIEWSTATE) IsNot Nothing Then
                grdMain.DataSource = CType(ViewState(ATTRIBUTE_ORDERHISTORYADMIN_VIEWSTATE), DataTable)
            End If
        End Sub

        Protected Sub grdMain_PageIndexChanged(sender As Object, e As Telerik.Web.UI.GridPageChangedEventArgs) Handles grdMain.PageIndexChanged
            If ViewState(ATTRIBUTE_ORDERHISTORYADMIN_VIEWSTATE) IsNot Nothing Then
                grdMain.DataSource = CType(ViewState(ATTRIBUTE_ORDERHISTORYADMIN_VIEWSTATE), DataTable)
                grdMain.CurrentPageIndex = e.NewPageIndex
            End If
        End Sub

        Protected Sub grdMain_PageSizeChanged(sender As Object, e As Telerik.Web.UI.GridPageSizeChangedEventArgs) Handles grdMain.PageSizeChanged
            If ViewState(ATTRIBUTE_ORDERHISTORYADMIN_VIEWSTATE) IsNot Nothing Then
                grdMain.DataSource = CType(ViewState(ATTRIBUTE_ORDERHISTORYADMIN_VIEWSTATE), DataTable)
            End If
        End Sub
    End Class
End Namespace
