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


Namespace Aptify.Framework.Web.eBusiness.ProductCatalog

    Partial Class AdminPaymentSummary
        Inherits BaseUserControlAdvanced
        Dim sCuurSymbol As String
        Dim arPay() As PayInfo
        Protected Const ATTRIBUTE_ADMIN_ORDER_DETAIL As String = "AdminOrderDetail"


   
        Public Overridable Property AdminOrderDetail() As String
            Get
                If Not ViewState(ATTRIBUTE_ADMIN_ORDER_DETAIL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_ADMIN_ORDER_DETAIL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_ADMIN_ORDER_DETAIL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(AdminOrderDetail) Then
                'since value is the 'default' check the XML file for possible custom setting
                AdminOrderDetail = Me.GetLinkValueFromXML(ATTRIBUTE_ADMIN_ORDER_DETAIL)
                If String.IsNullOrEmpty(AdminOrderDetail) Then
                    Me.grdOrderSummary.Enabled = False
                    Me.grdOrderSummary.ToolTip = "OrderConfirmationURL property has not been set."
                End If
            End If
        End Sub
        Private Sub LoadOrders()

            If Session("sPayOrderID") IsNot Nothing AndAlso CStr(Session("sPayOrderID")) <> String.Empty Then

                Dim sPayOrderID As String = CStr(Session("sPayOrderID"))
                If sPayOrderID <> String.Empty Then
                    'Dim sSQL As String = "select * from Orders where ID in('" + sPayOrderID + "')"
                    'Suraj S Issue 15195 ,4/8/13 ,amount field provide a comma eg:1000 to 1,000
                    Dim sSQL = "SELECT o.BIllToName Name,o.ID,CONVERT(VARCHAR,GrandTotal,1)As GrandTotal,CONVERT(VARCHAR,Balance,1) As Balance, convert(numeric(10,2), Balance) as PayAmount, " & _
             "CurrencySymbol, NumDigitsAfterDecimal, c.Name as CompanyName FROM " & _
             Database & "..vwOrders o " & _
             "INNER JOIN " & Database & "..vwCurrencyTypes ct ON o.CurrencyTypeID=ct.ID " & _
             "LEFT OUTER JOIN " & Database & "..vwCompanies c on c.ID = o.ShipToCompanyID " & _
             " WHERE o.ShipToID IN (SELECT distinct ID FROM vwPersons WHERE CompanyID IN ( SELECT ID FROM vwCompanies WHERE ID=" & User1.CompanyID & " OR RootCompanyID=" & User1.CompanyID & " ))" & _
             " AND  o.ID in(" + sPayOrderID + ") AND o.OrderStatus IN ('Taken', 'Shipped') And o.OrderType IN ('Regular', 'Quotation') "

                    Dim dt As DataTable = DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)
                    Session("dt") = dt
                    If dt.Rows.Count > 0 Then
                        sCuurSymbol = dt.Rows(0).Item("CurrencySymbol").ToString()
                        ViewState("sCuurSymbol") = sCuurSymbol

                        grdOrderSummary.DataSource = dt

                    End If
                End If
            End If

        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'Neha, issue 14456, 03/15/13 ,added method for sort order(assending) for rad grid first column
            If Not IsPostBack Then
                AddExpression()
                LoadOrders()
            End If
            SetProperties()
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

        Private Class PayInfo
            Public OrderID As Long
            Public PayAmount As Decimal
            Public Balance As Decimal
        End Class

        Protected Sub cmdback_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdback.Click
            MyBase.Response.Redirect(AdminOrderDetail, False)
        End Sub
        'Neha, issue 14456, 03/15/13, added needdatasource event for databinding, fitering,sorting rad grid
        Protected Sub grdOrderSummary_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdOrderSummary.NeedDataSource
            LoadOrders()
        End Sub
        'for assending order sorting on first time gridload(for first column) 
        Private Sub AddExpression()
            Dim expression1 As New GridSortExpression
            expression1.FieldName = "Name"
            expression1.SetSortOrder("Ascending")
            grdOrderSummary.MasterTableView.SortExpressions.AddSortExpression(expression1)
        End Sub
    End Class
End Namespace
