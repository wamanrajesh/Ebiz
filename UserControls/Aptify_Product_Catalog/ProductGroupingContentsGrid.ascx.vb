'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports Aptify.Framework.DataServices
Imports Aptify.Framework.Application
Imports Aptify.Applications.OrderEntry
Imports Aptify.Framework.Web.eBusiness.ProductCatalog
Imports System.Web
Imports System.Data
Imports Telerik.Web.UI

Namespace Aptify.Framework.Web.eBusiness.ProductCatalog
    Partial Class ProductGroupingContentsGrid
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced

        Private ShoppingCart1 As Aptify.Framework.Web.eBusiness.AptifyShoppingCart
        Private sTotalPrice As String
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "ProductGroupingContentsGrid"
        'Neha, Issue 14456, 3/18/13 , ATTRIBUTE_CONTORL_PRODUCT_GROUPING_CONTENTS used for set the property "CheckAddExpressionForProduct"
        Protected ATTRIBUTE_CONTORL_PRODUCT_GROUPING_CONTENTS As Boolean = False


#Region "ProductGroupingContentsGrid Specific Properties"
        Public Property NavigateURLFormatField() As String
            Get
                Dim o As Object
                o = ViewState.Item("NavigateURLFormatField")
                If o Is Nothing Then
                    Return ""
                Else
                    Return CStr(o)
                End If
            End Get
            Set(ByVal Value As String)
                ViewState.Add("NavigateURLFormatField", Value)
            End Set
        End Property

        Public Property ShoppingCart() As Aptify.Framework.Web.eBusiness.AptifyShoppingCart
            Get
                Return ShoppingCart1
            End Get
            Set(ByVal value As Aptify.Framework.Web.eBusiness.AptifyShoppingCart)
                ShoppingCart1 = value
            End Set
        End Property

        Public Property FormattedGroupPrice() As String
            Get
                Return sTotalPrice
            End Get
            Set(ByVal value As String)
                sTotalPrice = value
            End Set
        End Property
        'Neha, Issue 14456 3/18/13 , declare the prpoperty ,Grid load first time By default the sorting will be Ascending 
        Public Property CheckAddExpressionForProduct() As Boolean
            Get
                Return Me.ATTRIBUTE_CONTORL_PRODUCT_GROUPING_CONTENTS
            End Get
            Set(ByVal value As Boolean)
                Me.ATTRIBUTE_CONTORL_PRODUCT_GROUPING_CONTENTS = value
            End Set
        End Property

#End Region

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()
        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()


        End Sub
        'neha changes for Issue 14456
        Public Sub LoadGrid(ByVal ProductID As Long, ByVal ProductName As String)
            'Neha, Issue 14456, 3/18/13 , if CheckAddExpressionForProduct property is true then call the AddExpression and the value for this set on search user control
            If CheckAddExpressionForProduct = True Then
                AddExpression()
            End If

            Dim sSQL As String
            Dim dt As System.Data.DataTable
            Try
                If ViewState("ProductGroupingContentGridData") Is Nothing Then

                    ViewState("ProductName") = ProductName
                    ViewState("productID") = ProductID

                    'Query database to retrieve all data to enter into the datagrid
                    sSQL = "SELECT pa.SubProductID ProductID, p.WebName, " & _
                           "p.WebDescription, pa.Quantity " & _
                           "FROM " & AptifyApplication.GetEntityBaseDatabase("Products") & _
                           "..vwProductParts pa INNER JOIN " & _
                           AptifyApplication.GetEntityBaseDatabase("Products") & _
                           "..vwProducts p on pa.SubProductID=p.ID " & _
                           "WHERE pa.ProductID=" & ProductID & _
                           "ORDER BY pa.Sequence ASC"
                    dt = DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)

                    If dt.Rows.Count > 0 Then
                        'set table's title
                        lblTitle.Text = ProductName & " Contents"

                        'Add Prices to the datatable and set cumulative price
                        Dim dTotalPrice As Decimal = 0
                        dt.Columns.Add("Price", Type.GetType("System.String"))
                        Dim oPrice As IProductPrice.PriceInfo
                        For index As Integer = 0 To dt.Rows.Count - 1
                            oPrice = ShoppingCart1.GetUserProductPrice(CLng(dt.Rows(index).Item("ProductID")), CDec(dt.Rows(index).Item("Quantity")))
                            dTotalPrice += oPrice.Price * CDec(dt.Rows(index).Item("Quantity"))
                            dt.Rows(index).Item("Price") = Format(oPrice.Price, ShoppingCart1.GetCurrencyFormat(oPrice.CurrencyTypeID))
                        Next
                        Me.FormattedGroupPrice = ""
                        Me.FormattedGroupPrice = Format(dTotalPrice, ShoppingCart1.GetCurrencyFormat(oPrice.CurrencyTypeID))

                        Dim dcolUrl As DataColumn = New DataColumn()
                        dcolUrl.Caption = "WebNameUrl"
                        dcolUrl.ColumnName = "WebNameUrl"

                        dt.Columns.Add(dcolUrl)

                        Dim dcolUrlPrice As DataColumn = New DataColumn()
                        dcolUrlPrice.Caption = "PriceUrl"
                        dcolUrlPrice.ColumnName = "PriceUrl"

                        dt.Columns.Add(dcolUrlPrice)

                        If Not String.IsNullOrEmpty(NavigateURLFormatField) Then
                            If dt.Rows.Count > 0 Then

                                For Each rw As DataRow In dt.Rows
                                    rw("WebNameUrl") = String.Format(NavigateURLFormatField, rw("ProductID").ToString)
                                    rw("PriceUrl") = String.Format("{0:C}", rw("Price").ToString)
                                    ''lbl.Text = rw("PriceUrl")
                                Next
                            End If

                        End If
                        grdMain.DataSource = dt
                        ViewState("ProductGroupingContentGridData") = dt
                        'Navin Prasad Issue 11032

                        'With DirectCast(grdMain.Columns.Item(0), HyperLinkColumn)
                        '    If Not String.IsNullOrEmpty(NavigateURLFormatField) Then
                        '        .DataNavigateUrlFormatString = NavigateURLFormatField
                        '    Else
                        '        grdMain.Enabled = False
                        '        grdMain.ToolTip = "NavigateURLFormatField property not set via container control."
                        '    End If
                        'End With

                        grdMain.DataBind() 'fails on the Price, since no field named Price exists in the dt


                        Dim rowcounter As Integer = 0

                        'Navin Prasad Issue 11032

                        If Not String.IsNullOrEmpty(NavigateURLFormatField) Then
                            'For Each row As GridViewRow In grdMain.Items
                            '    Dim lnk As System.Web.UI.WebControls.HyperLink = CType(row.FindControl("lnkWebName"), System.Web.UI.WebControls.HyperLink)
                            '    lnk.NavigateUrl = String.Format(NavigateURLFormatField, dt.Rows((grdMain.CurrentPageIndex * grdMain.PageSize) + rowcounter)("ProductID").ToString)
                            '    Dim lbl As Label = CType(row.FindControl("lblPrice"), Label)
                            '    lbl.Text = String.Format("{0:C}", dt.Rows((grdMain.CurrentPageIndex * grdMain.PageSize) + rowcounter)("Price").ToString)
                            '    rowcounter = rowcounter + 1
                            'Next
                        Else
                            grdMain.Enabled = False
                            grdMain.ToolTip = "NavigateURLFormatField property not set via container control."
                        End If


                    Else
                        Me.Visible = False
                    End If

                Else
                    grdMain.DataSource = ViewState("ProductGroupingContentGridData")

                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        'neha changes for Issue 14456
        Protected Sub grdMain_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdMain.NeedDataSource
            Dim Id As Long = CLng(ViewState("productID"))
            Dim str As String = ViewState("ProductName").ToString()
            Me.LoadGrid(Id, str)
        End Sub
        'Added method for sort order(assending) for rad grid first column
        Private Sub AddExpression()
            Dim expression1 As New GridSortExpression
            expression1.FieldName = "WebName"
            expression1.SetSortOrder("Ascending")
            grdMain.MasterTableView.SortExpressions.AddSortExpression(expression1)
        End Sub

    End Class
End Namespace
