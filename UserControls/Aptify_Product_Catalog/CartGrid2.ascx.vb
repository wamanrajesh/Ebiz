'Aptify e-Business 5.5.1, July 2013
Option Explicit On

Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Framework.Application
Imports Aptify.Framework.DataServices
Imports System.Data
Imports Telerik.Web.UI

Namespace Aptify.Framework.Web.eBusiness.ProductCatalog
    Partial Class CartGrid2
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_PRODUCT_VIEW_PAGE As String = "ProductViewPage"
        Protected Const ATTRIBUTE_CLASS_VIEW_PAGE As String = "ClassViewPage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "CartGrid2"

#Region "CartGrid2 Specific Properties"
        ''' <summary>
        ''' ProductView page url
        ''' </summary>
        Public Overridable Property ProductViewPage() As String
            Get
                If Not ViewState(ATTRIBUTE_PRODUCT_VIEW_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_PRODUCT_VIEW_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_PRODUCT_VIEW_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' ClassView page url
        ''' </summary>
        Public Overridable Property ClassViewPage() As String
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
#End Region

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(ProductViewPage) Or String.IsNullOrEmpty(ClassViewPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                ProductViewPage = Me.GetLinkValueFromXML(ATTRIBUTE_PRODUCT_VIEW_PAGE)
                ClassViewPage = Me.GetLinkValueFromXML(ATTRIBUTE_CLASS_VIEW_PAGE)
                If String.IsNullOrEmpty(ProductViewPage) Or String.IsNullOrEmpty(ClassViewPage) Then
                    'Me.grdMain.Columns.RemoveAt(1)
                    'grdMain.Columns.AddAt(1, New BoundColumn())
                    'With DirectCast(grdMain.Columns(1), BoundColumn)
                    '    .DataField = "Product"
                    '    .HeaderText = "Product"
                    '    .ItemStyle.ForeColor = Drawing.Color.Blue
                    '    .ItemStyle.Font.Underline = True
                    'End With
                    Me.grdMain.Enabled = False
                    Me.grdMain.ToolTip = "ProductViewPage and/or ClassViewPage properties has not been set."
                End If
            End If
        End Sub

#Region "Databound Template Fields"
        ' GetRowQuantityEnabled ---------------------------------------------
        '   Author:     Richard Bowman
        '   Date:       7/1/2003
        '
        ' This property is used by the grid to assess whether to make a
        ' row's quantity field enabled or disabled. The quantity column
        ' functions as a databound template column in the grid, so for
        ' each row in the grid, this method will be called.
        '
        '   Parameters: Container : the row of the grid this call references
        '   Returns:    Boolean   : True, if the quantity field should
        '                           be enabled
        '
        Public ReadOnly Property GetRowQuantityEnabled(ByVal Container As Object) As Boolean
            Get
                Dim sSQL As String
                Dim dt As DataTable
                Dim sProductType As String
                Try
                    ' find the ProductTypeID for the product in question
                    sSQL = "SELECT ProductType FROM " & _
                           AptifyApplication.GetEntityBaseDatabase("Products") & _
                           "..vwProducts WHERE " & _
                           "ID=" & CLng(Container.DataItem("ProductID"))
                    dt = DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)
                    sProductType = CStr(dt.Rows(0).Item("ProductType"))

                    ' if the control is a child of a kit or is a meeting item (type=1)
                    ' then this returns false, otherwise it returns true
                    Return (CLng(Container.DataItem("ParentSequence")) <= 0) _
                            And (sProductType <> "Meeting")
                Catch ex As Exception
                    Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Finally
                    dt = Nothing
                End Try
            End Get
        End Property

        ' GetQtyBorderStyle -----------------------------------------------
        '   Author:     Mark Smith
        '   Date:       11/30/2006
        '
        ' This property is used by the grid to assess whether to make 
        ' the borderstyle of a row's quantity field 3d or flat. The quantity 
        ' column functions as a databound template column in the grid, so for
        ' each row in the grid, this method will be called.
        '
        ' The TextBox BorderStyle is the enumerated value of the following list:
        '        1  = NotSet  - The border style is not set.
        '        2  = None    - No border
        '        3  = Dotted  - A dotted line border.
        '        4  = Dashed  - A dashed line border.
        '        5  = Solid   - A solid line border.
        '        6  = Double  - A solid double line border.
        '        7  = Groove  - A grooved border for a sunken border appearance.
        '        8  = Ridge   - A ridged border for a raised border appearance.
        '        9  = Inset   - An inset border for a sunken control appearance.
        '        10 = Outset  - An outset border for a raised control appearance. 
        '
        '   Parameters: Container : The web control being altered
        '   Returns:    Byte:   The enumerated value of the desired borderstyle
        '
        Public ReadOnly Property GetQtyBorderStyle(ByVal Container As Object) As Byte
            Get
                If GetRowQuantityEnabled(Container) Then
                    Return 0 'borderstyle = NotSet (default = Inset)
                Else
                    Return 1 'borderstyle = None
                End If
            End Get
        End Property

#End Region
#Region "Application and DataAction Properties"
        Private m_oApp As AptifyApplication
        Private m_oDA As DataAction
        Public Overridable ReadOnly Property AptifyApplication() As AptifyApplication
            Get
                If m_oApp Is Nothing Then
                    Dim g As New EBusinessGlobal
                    m_oApp = g.GetAptifyApplication(Page.Application, Page.User)
                End If
                Return m_oApp
            End Get
        End Property
        Public Overridable ReadOnly Property DataAction() As DataAction
            Get
                If m_oDA Is Nothing Then
                    m_oDA = New DataAction(AptifyApplication.UserCredentials)
                End If
                Return m_oDA
            End Get
        End Property
#End Region

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
            If Not IsPostBack Then
                RefreshGrid()
            End If
        End Sub
        Public Sub RefreshGrid()
            Dim dt As New System.Data.DataTable()
            Dim oOrder As AptifyGenericEntityBase
            oOrder = ShoppingCart1.GetOrderObject(Page.Session, Page.User, Page.Application)
            Try
                grdMain.AllowPaging = False
                ShoppingCart1.SaveCart(Session)
                ShoppingCart1.FillCart(dt)
                Dim dcolUrl As DataColumn = New DataColumn()
                dcolUrl.Caption = "ProductUrl"
                dcolUrl.ColumnName = "ProductUrl"

                dt.Columns.Add(dcolUrl)
                If dt.Rows.Count > 0 Then
                    For Each rw As DataRow In dt.Rows
                        Dim intLoopIndex As Integer
                        For intLoopIndex = 0 To dt.Rows.Count - 1
                            rw("ProductUrl") = SetURLPerProductType(oOrder, intLoopIndex)
                        Next
                    Next
                End If
                If dt IsNot Nothing Then
                    grdMain.DataSource = dt
                    grdMain.DataBind()
                    ViewState("dtCart") = dt
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        'Navin Prasad Issue 11032
        Public ReadOnly Property Grid() As RadGrid
            Get
                Return grdMain
            End Get
        End Property
        Public ReadOnly Property Cart() As Aptify.Framework.Web.eBusiness.AptifyShoppingCart
            Get
                Return ShoppingCart1
            End Get
        End Property

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

        Public Function SaveShoppingCart(Optional ByVal Name As String = "", _
                                             Optional ByVal Description As String = "", _
                                             Optional ByVal lCartID As Long = 0) As Long
            Return ShoppingCart1.SaveShoppingCart(Name, Description, lCartID)
        End Function

        Public Sub LoadCart(ByVal lCartID As Long)
            ShoppingCart1.LoadShoppingCart(lCartID)
        End Sub
        Public Sub SaveCart()
            ShoppingCart1.SaveCart(Me.Session)
        End Sub
        'Navin Prasad Issue 11032

        'Protected Sub grdMain_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grdMain.ItemCommand
        '    Dim iLine As Integer, lProductID As Long
        '    Dim sPage As String = ""
        '    Dim oOrder As AptifyGenericEntityBase
        '    iLine = e.Item.ItemIndex
        '    oOrder = ShoppingCart1.GetOrderObject(Page.Session, Page.User, Page.Application)
        '    lProductID = CLng(oOrder.SubTypes("OrderLines").Item(iLine).GetValue("ProductID"))

        '    ' redirect to the appropriate page based on the product types
        '    ' table
        '    ShoppingCart1.GetProductTypeWebPages(lProductID, "", sPage)
        '    If Len(sPage) > 0 Then
        '        sPage = sPage & "?OL=" & iLine
        '        Response.Redirect(sPage)
        '    End If

        'End Sub
        'HP Issue#8621:  examine each orderline in order to properly set product links
        'Navin Prasad Issue 11032

        'Protected Sub grdMain_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdMain.ItemDataBound

        '    If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
        '        DirectCast(e.Item.FindControl("link"), HyperLink).NavigateUrl = _
        '        SetURLPerProductType(ShoppingCart1.GetOrderObject(Page.Session, Page.User, Page.Application), e.Item.ItemIndex)
        '    End If
        'End Sub

        'HP Issue#8621:  if the product type is of 'Class' then set the product link within the cart to open the appropriate class associated with the product
        Private Function SetURLPerProductType(ByVal orderGE As Aptify.Applications.OrderEntry.OrdersEntity, ByVal orderLine As Integer) As String
            Dim url As String = String.Empty
            Dim classId As Integer
            'Dim sql As String
            Dim prodId = CLng(orderGE.SubTypes("OrderLines").Item(orderLine).GetValue("ProductID"))

            If String.Compare(ShoppingCart1.GetProductType(prodId), "Class", True) = 0 Then

                Dim ole As Aptify.Applications.OrderEntry.OrderLinesEntity = orderGE.SubTypes("OrderLines")(orderLine)
                'load class information from object data
                ole.ExtendedOrderDetailEntity.Load("|" & CStr(ole.GetValue("__ExtendedAttributeObjectData")))
                classId = CInt(ole.ExtendedOrderDetailEntity.GetValue("ClassID"))

                If classId > 0 Then
                    url = ClassViewPage & "?ClassID=" & classId
                End If
            Else
                url = ProductViewPage & "?ID=" & prodId
            End If

            Return url
        End Function
 ''' <summary>
        ''' Nalini 12436 date:1/12/2011
        ''' </summary>
        ''' <remarks></remarks>
        'Protected Sub grdMain_PageIndexChanging(ByVal sender As Object, ByVal e As GridPageChangedEventArgs) Handles grdMain.PageIndexChanged
        '    ''grdMain.PageIndex = e.NewPageIndex
        '    RefreshGrid()
        'End Sub

        ''Protected Sub grdMain_RowCommand(ByVal sender As Object, ByVal e As GridCommandEventArgs) Handles grdMain.ItemCommand
        ''    If (e.CommandName = "Link") Then
        ''    Dim iLine As Integer, lProductID As Long
        ''    Dim sPage As String = ""
        ''        ''Dim oOrder As AptifyGenericEntityBase
        ''        iLine = e.CommandArgument
        ''        Dim lblProductID As Label = CType(e.Item.FindControl("lblProductID"), Label)
        ''        ''oOrder = ShoppingCart1.GetOrderObject(Page.Session, Page.User, Page.Application)
        ''        ''lProductID = CLng(oOrder.SubTypes("OrderLines").Item(iLine).GetValue("ProductID"))
        ''    ' redirect to the appropriate page based on the product types
        ''        ' table
        ''        lProductID = CLng(lblProductID.Text)
        ''    ShoppingCart1.GetProductTypeWebPages(lProductID, "", sPage)
        ''    If Len(sPage) > 0 Then
        ''        sPage = sPage & "?OL=" & iLine
        ''        Response.Redirect(sPage)
        ''    End If
        ''    End If



        ''End Sub

        Protected Sub grdMain_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdMain.DataBound
            'Nalini
            Dim rowcounter As Integer = 0
            Dim oOrder As AptifyGenericEntityBase
            oOrder = ShoppingCart1.GetOrderObject(Page.Session, Page.User, Page.Application)
            For Each row As GridDataItem In grdMain.Items
                'If (TypeOf (e.Item) Is GridDataItem) Then
                DirectCast(row.FindControl("link"), LinkButton).PostBackUrl = _
            SetURLPerProductType(oOrder, rowcounter)

                    rowcounter = rowcounter + 1
                'End If
            Next

        End Sub
        'Neha, issue 14456, 03/15/13,for databinding
        Protected Sub grdMain_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdMain.NeedDataSource
            If ViewState("dtCart") IsNot Nothing Then
                grdMain.DataSource = CType(ViewState("dtCart"), DataTable)
            End If
        End Sub
    End Class
End Namespace
