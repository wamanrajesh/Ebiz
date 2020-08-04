'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports Aptify.Framework.Application
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.BusinessLogic.Security
Imports System.Data
Imports Telerik.Web.UI

Namespace Aptify.Framework.Web.eBusiness.CustomerService
    Partial Class OrderHistoryControl
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_ORDER_CONFIRMATION_PAGE As String = "OrderConfirmationURL"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "OrderHistory"
        Protected Const ATTRIBUTE_DATATABLE_ORDER As String = "dtOrderHistory"

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
#End Region

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
            If Not IsPostBack Then
                'Anil B for issue 15302 on 23/04/2013
                LoadGrid()
                'Suraj Issue 14450 3/22/13 ,this method use to apply the odrering of rad grid first column
                AddExpressionOrderHistory()
            End If
        End Sub

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
        End Sub

        Private Sub LoadGrid()
            ' load the grid with the user's past ordere history
            Dim sSQL As String, dt As Data.DataTable
            Dim vCompanyId As String
            Try
                If ViewState(ATTRIBUTE_DATATABLE_ORDER) IsNot Nothing Then
                    grdMain.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_ORDER), DataTable)
                    grdMain.DataBind()
                    Exit Sub
                End If
                'Suraj S Issue 15195 ,4/8/13 ,amount field provide a comma eg:1000 to 1,000
                'sSQL = "SELECT *" & _
                '      "FROM " & Database & _
                '    "..vwPersonOrderHistory" & _
                '      " WHERE BillToID = " & User1.PersonID
                'Sheetal Issue 22102
                sSQL = "select distinct ID, BillToID,ORDERDATE,SHIPDATE,CURRENCYTYPE,CALC_GRANDTOTAL,SHIPTRACKINGNUM,SHIPTYPE,ORDERSTATUS,ORDERPARTY,ORDERTYPEID," & _
                        "Paymentparty= case when vwPersonOrderHistory.ID in(select distinct id from vwPersonOrderHistory vp where billtoid=" & User1.PersonID & "  or vp.PAYMENTPARTY='Company' and vp.PAYMENTPARTY='Individual' GROUP BY vp.Id having COUNT(distinct(vp.PAYMENTPARTY)) > 1 )  THEN 'Company/Individual' ELSE Paymentparty END " & _
                        " from vwPersonOrderHistory where billtoid=" & User1.PersonID

                If CLng(cmbOrderType.SelectedItem.Value) > 0 Then
                    sSQL = sSQL & " AND OrderTypeID= " & cmbOrderType.SelectedItem.Value
                End If
                sSQL = sSQL & " ORDER BY ID, BillToID,ORDERDATE,SHIPDATE,CURRENCYTYPE,CALC_GRANDTOTAL,SHIPTRACKINGNUM,SHIPTYPE,ORDERSTATUS,ORDERPARTY,ORDERTYPEID DESC"

                dt = DataAction.GetDataTable(sSQL)
                'Navin Prasad Issue 11032
                Dim hlink As GridHyperLinkColumn = CType(grdMain.Columns(0), GridHyperLinkColumn)
                hlink.DataNavigateUrlFormatString = Me.OrderConfirmationURL & "?ID={0}"
                'DirectCast(grdMain.Columns(0), HyperLinkColumn).DataNavigateUrlFormatString = Me.OrderConfirmationURL & "?ID={0}"

                grdMain.DataSource = dt
                grdMain.DataBind()
                ViewState(ATTRIBUTE_DATATABLE_ORDER) = dt

                'Sheetal 7/7/15 #21887 Hide payment party column if user is not associated with company
                sSQL = "SELECT COMPANYID FROM " & Database & _
                   "  ..VWPERSONS WHERE ID=" & User1.PersonID
                vCompanyId = DataAction.ExecuteScalar(sSQL).ToString
                If (String.IsNullOrEmpty(vCompanyId)) Then
                    Me.grdMain.Columns(8).Visible = False
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub cmbOrderType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbOrderType.SelectedIndexChanged
            ViewState(ATTRIBUTE_DATATABLE_ORDER) = Nothing
            LoadGrid()

            'Suraj Issue 14450 3/22/13 ,this method use to apply the odrering of rad grid first column
            AddExpressionOrderHistory()
        End Sub
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

        Protected Sub grdMain_PageIndexChanging(ByVal sender As Object, ByVal e As GridPageChangedEventArgs) Handles grdMain.PageIndexChanged
            grdMain.CurrentPageIndex = e.NewPageIndex
            If ViewState(ATTRIBUTE_DATATABLE_ORDER) IsNot Nothing Then
                grdMain.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_ORDER), DataTable)
            End If
        End Sub
       
        Protected Sub grdMain_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdMain.NeedDataSource
            If ViewState(ATTRIBUTE_DATATABLE_ORDER) IsNot Nothing Then
                grdMain.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_ORDER), DataTable)
            End If
        End Sub
        'Suraj Issue 14450 3/22/13 ,if the grid load first time By default the sorting will be Ascending for column Forum 
        Private Sub AddExpressionOrderHistory()
            Dim expression1 As New GridSortExpression
            expression1.FieldName = "ID"
            expression1.SetSortOrder("Ascending")
            grdMain.MasterTableView.SortExpressions.AddSortExpression(expression1)
        End Sub
    End Class
End Namespace
