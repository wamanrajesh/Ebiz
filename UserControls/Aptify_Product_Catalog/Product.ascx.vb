'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports Aptify.Framework.Application
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Applications.OrderEntry
Imports System.Data
Imports System.Diagnostics

Namespace Aptify.Framework.Web.eBusiness.ProductCatalog
    Partial Public Class ProductControl
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced

        Protected Const ATTRIBUTE_PRODUCT_CATEGORY_LINK_STRING_NON_ROOT_PAGE As String = "ProductCategoryLinkStringNonRootPage"
        Protected Const ATTRIBUTE_PRODUCT_CATEGORY_LINK_STRING_ROOT_PAGE As String = "ProductCategoryLinkStringRootPage"
        Protected Const ATTRIBUTE_PRODUCT_GROUPING_CONTENTS_VIEW_PRODUCT_PAGE As String = "GroupingContentsViewProductPage"
        Protected Const ATTRIBUTE_IMAGE_NOT_AVAILABLE_URL As String = "ImageNotAvailable"
        Protected Const ATTRIBUTE_ITEM_NOT_AVAILABLE_IMAGE_URL As String = "ItemNotAvailableImage"
        Protected Const ATTRIBUTE_ADD_TO_CART_IMAGE_URL As String = "AddToCartImage"
        Protected Const ATTRIBUTE_VIEW_CART_IMAGE_URL As String = "ViewmycartImage" 'RashmiP, Issue 9989, 09/15/10
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "Product"
        Protected Const ATTRIBUTE_VIEW_CART_PAGE As String = "ViewcartPage"
        Protected Const ATTRIBUTE_VIEW_CLASS_PAGE As String = "ViewClassPage" 'RashmiP, Issue 11290 [Meeting/Class Integration]
        Protected Const ATTRIBUTE_LOGIN_PAGE As String = "LoginPage" 'Neha changes for Issue 18752,05/15/2014
        Private objOrder As OrdersEntity

#Region "Product Specific Properties"
        'Neha changes for Issue 18752,05/15/2014
        ''' <summary>
        ''' Login page url
        ''' </summary>
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


        ''' <summary>
        ''' ProductCategoryLinkStringNonRoot page url
        ''' </summary>
        Public Overridable Property ProductCategoryLinkStringNonRootPage() As String
            Get
                If Not ViewState(ATTRIBUTE_PRODUCT_CATEGORY_LINK_STRING_NON_ROOT_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_PRODUCT_CATEGORY_LINK_STRING_NON_ROOT_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_PRODUCT_CATEGORY_LINK_STRING_NON_ROOT_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' ProductCategoryLinkStringRoot page url
        ''' </summary>
        Public Overridable Property ProductCategoryLinkStringRootPage() As String
            Get
                If Not ViewState(ATTRIBUTE_PRODUCT_CATEGORY_LINK_STRING_ROOT_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_PRODUCT_CATEGORY_LINK_STRING_ROOT_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_PRODUCT_CATEGORY_LINK_STRING_ROOT_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' GroupingContentsViewProduct page url
        ''' </summary>
        Public Overridable Property GroupingContentsViewProductPage() As String
            Get
                If Not ViewState(ATTRIBUTE_PRODUCT_GROUPING_CONTENTS_VIEW_PRODUCT_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_PRODUCT_GROUPING_CONTENTS_VIEW_PRODUCT_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_PRODUCT_GROUPING_CONTENTS_VIEW_PRODUCT_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' ImageNotAvailable url
        ''' </summary>
        Public Overridable Property ImageNotAvailable() As String
            Get
                If Not ViewState(ATTRIBUTE_IMAGE_NOT_AVAILABLE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_IMAGE_NOT_AVAILABLE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_IMAGE_NOT_AVAILABLE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' ItemNotAvailableImage url
        ''' </summary>
        Public Overridable Property ItemNotAvailableImage() As String
            Get
                If Not ViewState(ATTRIBUTE_ITEM_NOT_AVAILABLE_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_ITEM_NOT_AVAILABLE_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_ITEM_NOT_AVAILABLE_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' AddToCartImage url
        ''' </summary>
        Public Overridable Property AddToCartImage() As String
            Get
                If Not ViewState(ATTRIBUTE_ADD_TO_CART_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_ADD_TO_CART_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_ADD_TO_CART_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' RashmiP, Issue 9989, 09/15/10
        ''' ViewCart Image url
        ''' </summary>
        Public Overridable Property ViewmycartImage() As String
            Get
                If Not ViewState(ATTRIBUTE_VIEW_CART_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_VIEW_CART_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_VIEW_CART_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        Private Property TotalCartQty() As Integer
            Get
                If Context.Items("TotalCartQty") IsNot Nothing Then
                    Return Convert.ToInt32(Context.Items("TotalCartQty"))
                Else
                    Return 0
                End If

            End Get
            Set(ByVal value As Integer)
                Context.Items("TotalCartQty") = value
            End Set
        End Property

        Private Property ProductID() As Long
            Get
                If ViewState.Item("ProductID") IsNot Nothing Then
                    Return CLng(ViewState.Item("ProductID"))
                Else
                    Return -1
                End If
            End Get
            Set(ByVal value As Long)
                ViewState.Item("ProductID") = value
            End Set
        End Property

        Private Property NewProductVersionID() As Long
            Get
                If Not ViewState.Item("NewProductVersionID") Is Nothing Then
                    Return CLng(ViewState.Item("NewProductVersionID"))
                Else
                    Return -1
                End If
            End Get
            Set(ByVal value As Long)
                ViewState.Item("NewProductVersionID") = value
            End Set
        End Property

        Public Overridable Property ViewcartPage() As String
            Get
                If Not ViewState(ATTRIBUTE_VIEW_CART_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_VIEW_CART_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_VIEW_CART_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        ''RashmiP, Issue 11290 [Meeting/Class Integration]
        Public Overridable Property ViewClassPage() As String
            Get
                If Not ViewState(ATTRIBUTE_VIEW_CLASS_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_VIEW_CLASS_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_VIEW_CLASS_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
#End Region

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties

            MyBase.SetProperties()
            'Neha changes for Issue 18752,05/15/2014
            If String.IsNullOrEmpty(LoginPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                LoginPage = Me.GetLinkValueFromXML(ATTRIBUTE_LOGIN_PAGE)
            End If

            If String.IsNullOrEmpty(ProductCategoryLinkStringRootPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                ProductCategoryLinkStringRootPage = Me.GetLinkValueFromXML(ATTRIBUTE_PRODUCT_CATEGORY_LINK_STRING_ROOT_PAGE)
                If String.IsNullOrEmpty(ProductCategoryLinkStringRootPage) Then
                    ProductCategoryLinkString1.Enabled = False
                    Me.ProductCategoryLinkString1.ToolTip = "ProductCategoryLinkStringRootPage property has not been set."
                Else
                    Me.ProductCategoryLinkString1.RootCategoryURL = ProductCategoryLinkStringRootPage
                End If
            Else
                Me.ProductCategoryLinkString1.RootCategoryURL = ProductCategoryLinkStringRootPage
            End If
            If String.IsNullOrEmpty(ProductCategoryLinkStringNonRootPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                ProductCategoryLinkStringNonRootPage = Me.GetLinkValueFromXML(ATTRIBUTE_PRODUCT_CATEGORY_LINK_STRING_NON_ROOT_PAGE)
            End If
            If String.IsNullOrEmpty(GroupingContentsViewProductPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                GroupingContentsViewProductPage = Me.GetLinkValueFromXML(ATTRIBUTE_PRODUCT_GROUPING_CONTENTS_VIEW_PRODUCT_PAGE)
            End If
            If String.IsNullOrEmpty(ImageNotAvailable) Then
                'since value is the 'default' check the XML file for possible custom setting
                ImageNotAvailable = Me.GetLinkValueFromXML(ATTRIBUTE_IMAGE_NOT_AVAILABLE_URL)
            End If
            If String.IsNullOrEmpty(ItemNotAvailableImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                ItemNotAvailableImage = Me.GetLinkValueFromXML(ATTRIBUTE_ITEM_NOT_AVAILABLE_IMAGE_URL)
                imgNotAvailable.Src = ItemNotAvailableImage
            End If
            If String.IsNullOrEmpty(AddToCartImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                AddToCartImage = Me.GetLinkValueFromXML(ATTRIBUTE_ADD_TO_CART_IMAGE_URL)
                'imgAddToCart.Src = AddToCartImage
            End If
            'if values are not provide directly or from the XML file, set default values for inherited properties since
            'control requires them to properly function
            If String.IsNullOrEmpty(Me.QueryStringRecordIDParameter) Then Me.QueryStringRecordIDParameter = "ID"

            'RashmiP, Issue 9989, 09/15/10
            If String.IsNullOrEmpty(ViewmycartImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                ViewmycartImage = Me.GetLinkValueFromXML(ATTRIBUTE_VIEW_CART_IMAGE_URL)
                'Nalini Issue 12734
                'imgViewCart.Src = ViewmycartImage
            End If
            If String.IsNullOrEmpty(ViewcartPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                ViewcartPage = Me.GetLinkValueFromXML(ATTRIBUTE_VIEW_CART_PAGE)
            End If
            ''RashmiP, Issue 11290 [Meeting/Class Integration]
            If String.IsNullOrEmpty(ViewClassPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                ViewClassPage = Me.GetLinkValueFromXML(ATTRIBUTE_VIEW_CLASS_PAGE)
            End If
            'RashmiP issue 9990, 09/24/10
            Dim oOrder As OrdersEntity
            oOrder = ShoppingCart1.GetOrderObject(Page.Session, Page.User, Page.Application)
            If oOrder IsNot Nothing Then
                If oOrder.SubTypes("OrderLines").Count > 0 Then
                    If Not String.IsNullOrEmpty(Request.QueryString("Val2")) Then
                        lnkViewCart.Visible = True
                        lblAdded.Visible = True
                        lblAdded.Text = Request.QueryString("Val2")
                        lblAdded.ForeColor = Drawing.Color.Blue
                    Else
                        lnkViewCart.Visible = False
                        lblAdded.Visible = False
                    End If
                Else
                    lblAdded.Visible = False
                    lnkViewCart.Visible = False
                End If
            Else
                lblAdded.Visible = False
                lnkViewCart.Visible = False
            End If
        End Sub

        Protected Overridable Sub SetDetailVisible(ByVal bFlag As Boolean)
            productdetailpanel.Visible = bFlag
            lblSummary.Visible = bFlag
            lblDescription.Visible = bFlag
            lblprodDesc.Visible = bFlag
            lblLongDescription.Visible = bFlag
        End Sub
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
            Try

                If Not IsPostBack Then
                    Dim dt As DataTable
                    Dim sSQL As String
                    'RashmiP, Issue 9511, 09/08/10
                    AddHandler lnkAddToCart.Click, AddressOf lnkAddToCart_ServerClick

                    If Me.SetControlRecordIDFromParam() Then
                        Me.ProductID = Me.ControlRecordID

                        sSQL = "SELECT * FROM " & _
                               AptifyApplication.GetEntityBaseDatabase("Products") & _
                               "..vwProducts WHERE ID=" & Me.ProductID
                        dt = DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)

                        ''Nalini 
                        If (dt.Rows.Count() > 0) Then
                            If dt.Rows(0)("IsSubscription").ToString() = "True" Then
                                ChkAutoRenew.Visible = True
                                lblChkAutoRenew.Visible = True
                            Else
                                ChkAutoRenew.Visible = False
                                lblChkAutoRenew.Visible = False
                            End If

                        End If
                        'Navin Prasad Issue 11172
                        ViewState("AvailableForSale") = False
                        If Not dt Is Nothing AndAlso _
                           dt.Rows.Count > 0 Then

                            If CBool(dt.Rows(0)("TopLevelItem")) AndAlso CBool(dt.Rows(0)("IsSold")) Then

                                ViewState("AvailableForSale") = True
                                If CheckProduct(dt) Then

                                    LoadControlValues(dt.Rows(0))

                                    'Load grid for product's subproducts
                                    If CInt(dt.Rows(0).Item("ProductKitTypeID")) <> 1 Then
                                        ProductGroupingContentsGrid.ShoppingCart = ShoppingCart1
                                        ProductGroupingContentsGrid.NavigateURLFormatField = GroupingContentsViewProductPage & "?ID={0}"
                                        'Neha, Issue 14456,3/18/13 , set property true,declared on productgroupingcontentsgrid control for rad grid first column Ascending order sorting   
                                        ProductGroupingContentsGrid.CheckAddExpressionForProduct = True
                                        ProductGroupingContentsGrid.LoadGrid(Me.ProductID, CStr(dt.Rows(0).Item("WebName")))
                                        ProductGroupingContentsGrid.Visible = True
                                        If CInt(dt.Rows(0).Item("ProductKitTypeID")) = 3 Then
                                            'Product Grouping is calculated differently (cumulative total of the product*qty)
                                            'so Get the price that was calculated from the Product Group listing
                                            lblPrice.Text = ProductGroupingContentsGrid.FormattedGroupPrice
                                        End If
                                    End If

                                    'Aparna issue 9025,9042 for product detail and AddToCart button show for web enabled product
                                    SetDetailVisible(True)

                                Else
                                    'Aparna issue 9025,9042 for product detail and AddToCart button hide for non-web enabled product
                                    SetDetailVisible(False)
                                    Throw New Exception("The requested product is not available on the web.")
                                End If
                            Else
                                SetDetailVisible(False)
                                Me.imgProduct.Visible = False
                                ViewState("AvailableForSale") = False
                                Throw New Exception(" The requested product is not available for sale.")
                            End If

                        Else
                            Throw New Exception()
                        End If
                    End If


                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                lblMsg.Text = "Error Loading Product ID: " & Request.QueryString("ID") & ". " & _
                               ex.Message
                ProductCategoryLinkString1.Visible = False
                'ProdListingGrid1.Visible = False
                'lblOtherProdsInCat.Visible = False
                'Me.table3.Visible = False
                Me.imgProduct.Visible = False
                'Me.ProductTopicCodesGrid1.Visible = False
                'Me.RelatedProductsGrid1.Visible = False
            Finally
                'Aparna issue 9025,9042 for product detail and AddToCart button hide for non-web enabled product
                Dim s As String = "Error Loading Product ID: " & Request.QueryString("ID") & ". "
                If lblMsg.Text.ToString().Contains("The requested product is not available on the web.") Then
                    Me.imgProduct.Visible = False
                ElseIf imgProduct.ImageUrl Is Nothing Or imgProduct.ImageUrl = "" Or imgProduct.Visible = False Then
                    Me.imgProduct.ImageUrl = ImageNotAvailable
                    Me.imgProduct.Visible = True
                End If
                If ViewState("AvailableForSale") IsNot Nothing Then
                    If CBool(ViewState("AvailableForSale")) = False Then
                        Me.imgProduct.Visible = False
                    End If
                End If
            End Try

        End Sub

        ''' <summary>
        ''' This method is responsible for checking to see if the product is web enabled and also determining the
        ''' appropriate page url to use for the product. To determine the appropriate URL, a stored procedure is
        ''' called that returns either the appropriate URL to redirect to or a blank value. If a blank value is
        ''' returned that means this generic product page is to be used, otherwise the URL returned should be redirected
        ''' to for viewing the product.
        ''' </summary>
        ''' <param name="dtProduct"></param>
        ''' <remarks></remarks>
        Private Function CheckProduct(ByVal dtProduct As DataTable) As Boolean
            Dim sSQL As String, sURL As Object
            Dim dt As DataTable
            Try
                With dtProduct.Rows(0)
                    If CBool(.Item("WebEnabled")) Then
                        sSQL = "..spGetProductWebURL @ProductID=" & dtProduct.Rows(0).Item("ID").ToString
                        sURL = DataAction.ExecuteScalar(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)

                        ''RashmiP, Check for classID, If ClassId exist for Meeting Type product then redirect to ViewClass Page
                        'Issue 11290 [Meeting/Class Integration]
                        sSQL = "SELECT ClassID FROM " & _
                       Me.AptifyApplication.GetEntityBaseDatabase("Product Types") & "..vwProducts WHERE ID=" & _
                           dtProduct.Rows(0).Item("ID").ToString
                        dt = DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)
                        If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                            If Not IsDBNull(dt.Rows(0).Item("ClassID")) Then
                                sURL = ViewClassPage & "?ClassID=" & CStr(dt.Rows(0).Item("ClassID"))
                                Response.Redirect(CStr(sURL).Trim)
                                Return True
                            End If
                        End If
                        'suraj Issue 14861 , 4/25/13 

                        If Not sURL Is Nothing AndAlso Len(sURL) > 0 Then
                            Response.Redirect(CStr(sURL).Trim & Me.Request.Url.Query, False)
                        End If
                        Return True
                    Else
                        Return False
                    End If
                End With
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return False
            End Try
        End Function

        Private Sub LoadControlValues(ByVal dr As DataRow)
            Dim sReason As String = ""
            Dim sWebErrorMsg As String = ""
            Try
                lblName.Text = CStr(dr.Item("WebName"))

                If Not IsDBNull(dr("WebDescription")) Then
                    lblDescription.Text = CStr(dr.Item("WebDescription"))
                Else
                    lblDescription.Text = ""
                End If

                If Not IsDBNull(dr("Code")) AndAlso CStr(dr("Code")) <> "" Then
                    lblCode.Text = CStr(dr.Item("Code"))
                Else
                    lblCode.Text = ""
                    trProductCode.Visible = False
                End If

                If Not IsDBNull(dr("WebLongDescription")) Then
                    lblLongDescription.Text = CStr(dr.Item("WebLongDescription"))
                Else
                    lblLongDescription.Text = ""
                End If

                If Not IsDBNull(dr("WebImage")) AndAlso _
                   Len(dr("WebImage")) > 0 Then
                    imgProduct.ImageUrl = CStr(dr.Item("WebImage"))
                Else
                    imgProduct.ImageUrl = ImageNotAvailable
                End If
                'imgProduct.Visible = Len(imgProduct.ImageUrl) > 0

                Dim oPrice As IProductPrice.PriceInfo = Me.GetProductPrice(CLng(dr.Item("ID")))


                lblPrice.Text = Format(oPrice.Price, ShoppingCart1.GetCurrencyFormat(oPrice.CurrencyTypeID))

                'Suvarna D IssueID-12720 to remove the label on Jan 19, 2012
                'Suvarna Deshmukh IssueID-12433,12430 and 12434 On Dec 16,2011
                'commented and added to assign and display value of label "Price For you"
                'lblPriceForYouVal.Text = Format(oPrice.Price, ShoppingCart1.GetCurrencyFormat(oPrice.CurrencyTypeID))
                ''End of addtion by Suvarna Deshmukh IssueID-12433,12430 and 12434 On Dec 16,2011
                'End of addition by Suvarna D IssueID-12720 to remove the label on Jan 19, 2012

                'Display Member Savings if > 0
                If Me.User1.PersonID > 0 Then
                    '20090126 MAS: using different logic for calculating Member Savings since the original way was not properly
                    '              calculating complex pricing rules.
                    '              NOTE: member savings can only be calculated for a User that is logged into the website, 
                    '                    since pricing may be based on the User's address.
                    'Dim dSavings As Decimal = Me.ShoppingCart1.GetProductMemberSavings(Me.Page.User, CLng(dr.Item("ID")))
                    Dim dSavings As Decimal
                    'Dim memCost As Decimal = ShoppingCart1.GetSingleProductMemberCost(Me.Page.User, CLng(dr.Item("ID")))
                    Dim nonmemCost As Decimal = ShoppingCart1.GetSingleProductNonMemberCost(Me.Page.User, CLng(dr.Item("ID")))
                    dSavings = nonmemCost - oPrice.Price
                    If dSavings > 0 Then
                        Me.lblMemSavings.Text = "(" & Format$(dSavings, ShoppingCart1.GetCurrencyFormat(oPrice.CurrencyTypeID))
                        Me.lblMemSavings.Text &= " member savings)"
                        Me.lblMemSavings.Visible = True
                    Else
                        Me.lblMemSavings.Visible = False
                    End If
                Else
                    Me.lblMemSavings.Visible = False
                End If

                If Not IsDBNull(dr("MinSellingUnits")) AndAlso _
                   CLng(dr("MinSellingUnits")) > 1 Then
                    lblSellingUnits.Text = "Min. Order Qty: " & CLng(dr("MinSellingUnits"))
                    lblSellingUnits.Visible = True
                Else
                    lblSellingUnits.Visible = False
                End If


                If ProductAvailable(dr, sReason) Then
                    'HP - Default text is sufficient only need to make sure label is visble
                    'lblAvailable.Text = "Yes"
                    lblAvailable.Visible = True
                    'Suvarna IssueId 12720 Dispaly msg when product available
                    lblavailval.Text = "In Stock"

                    lnkAddToCart.Visible = True
                    'lnkAddToCart.HRef = lnkAddToCart.HRef & "?ID=" & CLng(dr.Item("ID"))
                    'imgAddToCart.Visible = True
                    imgNotAvailable.Visible = False

                    ''RashmiP issue 9511, 11/1/2010, 
                    ' Check for Web Prerequisite Message before product added to cart. 
                    'Function return true if Web Pre-Rquisite error msg exist
                    If GetWebPrerequisiteMsg(dr, sWebErrorMsg) Then
                        lnkAddToCart.Visible = False
                        'imgAddToCart.Visible = False
                        lblAdded.ForeColor = Drawing.Color.Red
                        lblAdded.Visible = True
                        lblAdded.Text = sWebErrorMsg
                    Else
                        lblAvailable.Visible = True
                        lnkAddToCart.Visible = True
                        'imgAddToCart.Visible = True
                        'Suvarna IssueId 12720 Dispaly msg when product available
                        lblavailval.Text = "In Stock"
                    End If
                Else
                    'HP - Label text is irrelevant since using an image indicating non-availability, make sure label is not visible
                    'lblAvailable.Text = "No"
                    lblAvailable.Visible = False
                    'suvarna Deshmukh IssueID-12433,12430 and 12434 On Dec 16,2011
                    'Code added to not to display the value of the label "Available" when product is not available.
                    lblavailval.Visible = False
                    'End by suvarna Deshmukh IssueID-12433,12430 and 12434 On Dec 16,2011
                    lnkAddToCart.Visible = False
                    'imgAddToCart.Visible = False
                    'Set to false for not to dispaly Image as per Rakhi's suggestion
                    imgNotAvailable.Visible = False
                    lblProductMessage.Text = sReason
                    lblProductMessage.Visible = True
                    If Not String.IsNullOrEmpty(sReason) Then
                        lblNote.Visible = True
                    End If
                    'Suvarna IssueId 12720 Dispaly msg when product available
                    lblavailval.Text = "Not In Stock"
                    lblavailval.ForeColor = System.Drawing.Color.Red
                    imgNotAvailable.Visible = False
                    lblavailval.Visible = True
                    lblAvailable.Visible = True
                End If

                'check to see if a newer version of this product can be offered
                Dim sSQL As String
                Dim dt As DataTable
                Dim lNewerProductID As Long
                Dim oNewerProductID As Object
                Dim lCurrentProductID As Long = CLng(dr.Item("ID"))
                sSQL = "SELECT " & AptifyApplication.GetEntityBaseDatabase("Products") & _
                ".dbo.fnGetLatestVersionProductID(" & lCurrentProductID & ")"
                oNewerProductID = DataAction.ExecuteScalar(sSQL)
                If IsNumeric(oNewerProductID) Then
                    lNewerProductID = CLng(oNewerProductID)
                Else
                    lNewerProductID = -1
                End If


                'display link to the latest valid version of this product if one exists
                If lNewerProductID > 0 AndAlso lNewerProductID <> lCurrentProductID Then
                    sSQL = "SELECT Name, WebName, WebEnabled FROM " & _
                            AptifyApplication.GetEntityBaseDatabase("Products") & _
                            ".dbo.vwProducts WHERE ID=" & lNewerProductID
                    dt = DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                    If CBool(dt.Rows(0).Item("WebEnabled")) Then
                        'a newer product has been found that can be linked to
                        If Not IsDBNull(dt.Rows(0).Item("WebName")) _
                           AndAlso CStr(dt.Rows(0).Item("WebName")) <> "" Then
                            btnNewVersion.Text = CStr(dt.Rows(0).Item("WebName"))
                        Else
                            btnNewVersion.Text = CStr(dt.Rows(0).Item("Name"))
                        End If
                        Me.NewProductVersionID = lNewerProductID
                        'lnkNewerProduct.PostBackUrl = "http://www.google.com" '../ProductCatalog/Product.aspx?ID=" & lNewerProductID
                        lblNewerProduct.Visible = True
                        btnNewVersion.Visible = True
                        'lnkNewerProduct.Visible = True
                    End If
                End If


                With Me.ProductCategoryLinkString1
                    .CategoryID = CLng(dr("CategoryID"))
                    If Not String.IsNullOrEmpty(ProductCategoryLinkStringNonRootPage) Then
                        .URL = ProductCategoryLinkStringNonRootPage
                    Else
                        .Enabled = False
                        .ToolTip = "ProductCategoryLinkStringNonRootPage property not set."
                    End If
                    .Separator = ":"
                    .HyperlinkLastCategory = True
                    .Visible = True
                End With
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Function ProductAvailable(ByVal dr As DataRow, _
                                          ByRef Reason As String) As Boolean
            Dim lProductID As Long

            Try
                ' Richard Bowman - 1/5/2004
                ' Added support for the business logic of the Housing Module.
                ' If a particular housing product has been added to an order,
                ' it does not make sense for another order line to be added
                ' to the order. If the housing product is already associated
                ' with the order, it will cause ProductAvailable to return
                ' false so it cannot be ordered again.

                ' Also added functionality to pass back a "reason" string,
                ' which offers additional explanation about the return value.
                If Not IsNumeric(Request.QueryString("ID")) Then
                    Throw New ArgumentException("Parameter must be numeric.", "ID")
                End If
                lProductID = CLng(Request.QueryString("ID"))
                'Dim oOrderGE As AptifyGenericEntityBase
                'Dim i As Integer
                'If String.Compare(ShoppingCart1.GetProductType(lProductID), "Housing", True) = 0 Then
                '    ' check to see if a housing product already exists in the cart
                '    oOrderGE = ShoppingCart1.GetOrderObject(Page.Session, Page.User, Page.Application)
                '    For i = 0 To oOrderGE.SubTypes("OrderLines").Count - 1
                '        ' search each order line
                '        If CLng(oOrderGE.SubTypes("OrderLines").Item(i).GetValue("ProductID")) = lProductID Then
                '            ' found the product already
                '            ' return false to specify the housing product
                '            ' is not available to add to the cart again
                '            Reason = "A housing product cannot be added to your cart multiple times. To make multiple reservations, use the Details button of the current Housing Registration from your cart to add additional registrations."
                '            Return False
                '        End If
                '    Next
                'End If

                '8/30/06 RJK - Temporarily make Housing Products unavailable.
                'This was done because there were issues with Housing purchases not
                'creating Housing Reservation Detail records.
                If String.Compare(ShoppingCart1.GetProductType(lProductID), "Housing", True) = 0 Then
                    Reason = "Housing Products are not supported in this build."
                    Return False
                End If

                '2006/12/14 MAS
                'Expanded the product availability logic
                'Properties checked to determine product availability:
                '1. Product currently sold (product.IsSold)
                '2. Date product is available (product.DateAvailable)
                '3. Date product is available until (product.AvailableUntil)
                '4. Product Inventory required (product.RequireInventory)
                '5. Product Quantity on Hand (product.QuantityOnHand)
                'IF all of the above conditions pass, then this product is available for purchase
                Dim bAvailable As Boolean = True
                Dim dToday As Date = Today()

                With dr
                    '1. Product currently sold (product.IsSold)
                    If Not CBool(.Item("IsSold")) Then
                        Reason = "This Product is Not Available."
                        bAvailable = False

                    Else
                        '2. Date product is available (product.DateAvailable)
                        If Not IsDBNull(.Item("DateAvailable")) _
                           AndAlso CStr(.Item("DateAvailable")) <> "" _
                           AndAlso CDate(.Item("DateAvailable")) > dToday Then
                            Reason = "This product is not availble until " & _
                                             CDate(.Item("DateAvailable")).ToLongDateString & "."
                            bAvailable = False

                            '3. Date product is available until (product.AvailableUntil)
                        ElseIf Not IsDBNull(.Item("AvailableUntil")) _
                               AndAlso CStr(.Item("AvailableUntil")) <> "" _
                               AndAlso CDate(.Item("AvailableUntil")) < dToday _
                                AndAlso CStr(.Item("AvailableUntil")) <> "1/1/1900" Then  ''RashmiP, Issue 14938
                            'Reason = "This product's availability ended on " & _
                            '                 CDate(.Item("DateAvailable")).ToLongDateString & "."
                            bAvailable = False

                            '4. Product Inventory required (product.RequireInventory)
                            '5. Product Quantity on Hand (product.QuantityOnHand)

                            'HP Issue#8283: make availability based on QuantityAvailable instead of QuantityOnHand
                            'ElseIf CBool(.Item("RequireInventory")) _
                            '       AndAlso Not IsDBNull(.Item("QuantityOnHand")) _
                            '       AndAlso CInt(.Item("QuantityOnHand")) < 1 Then
                            '    Reason = "No more units of this product are available."
                            '    bAvailable = False
                        ElseIf CBool(.Item("RequireInventory")) _
                          AndAlso Not IsDBNull(.Item("QuantityAvailable")) _
                          AndAlso CInt(.Item("QuantityAvailable")) < 1 Then
                            Reason = "No more units of this product are available."
                            bAvailable = False

                        Else
                            'Else this product is available
                            bAvailable = True
                        End If
                    End If
                End With
                Return bAvailable

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Reason = "An error occured." & vbNewLine & ex.Message
                Return False
            End Try
        End Function

        Public Function GetProductPrice(ByVal lProductID As Long) As IProductPrice.PriceInfo
            ' Implement This function
            Return ShoppingCart1.GetUserProductPrice(lProductID, 1)
        End Function
        Public Function GetNonMemberProductPrice() As Decimal
            ' Implement This function
            Return 0
        End Function

        ''RashmiP issue 9511,11/1/2010, to show web pre-requisite message before product added to cart.
        Private Function GetWebPrerequisiteMsg(ByVal dr As DataRow, ByRef sWebErrorMsg As String) As Boolean
            Dim oOrder As New Aptify.Applications.OrderEntry.OrdersEntity
            oOrder = ShoppingCart1.GetOrderObject(Page.Session, Page.User, Page.Application)
            Dim lProductID As Long = CLng(Request.QueryString("ID"))
            Dim PrerequisitesOverridePromptResult As Microsoft.VisualBasic.MsgBoxResult
            Try
                If Not oOrder.ValidateProductPrerequisites(lProductID, CInt(dr.Item("QuantityAvailable")), PrerequisitesOverridePromptResult) Then
                    sWebErrorMsg = CStr(oOrder.WebProdPreRequisiteErrMsg)
                    Return True
                Else
                    sWebErrorMsg = Nothing
                    Return False
                End If
            Catch ex As Exception
                Return False
            End Try
        End Function

        Private Sub lnkAddToCart_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkAddToCart.Click
            Try
                'Neha changes for Issue 18752,05/15/2014
                If User1.UserID > 0 Then
                    Dim sProductPage As String = ""
                    Dim sOrderPage As String = ""
                    Dim oOrder As OrdersEntity
                    Dim bCombineLines As Boolean
                    Dim iQty As Integer
                    Dim lstProductID As List(Of Long) = Nothing
                    Dim bFound As Boolean = False
                    Dim iItemForUpdate As Integer = 0
                    Dim oProduct As Aptify.Applications.ProductSetup.ProductObject

                    '8/30/06 RJK - Temporarily make Housing Products unavailable.
                    'This was done because there were issues with Housing purchases not
                    'creating Housing Reservation Detail records.
                    'If Not IsNumeric(Request.QueryString("ID")) Then
                    '    Throw New ArgumentException("Parameter must be numeric.", "ID")
                    'End If
                    Dim strProductType As String = ShoppingCart1.GetProductType(Me.ProductID)
                    If String.Compare(strProductType, "Housing", True) = 0 Then
                        lblAdded.Text = "Housing Products are not supported in this build."
                        lblAdded.Visible = True
                    Else
                        If String.Compare(strProductType, "Meeting", True) = 0 OrElse _
                            String.Compare(strProductType, "Housing", True) = 0 Then

                            bCombineLines = False
                        Else
                            bCombineLines = True
                        End If

                        If IsNumeric(txtQuantity.Text) Then
                            iQty = CInt(txtQuantity.Text)
                        Else
                            lblAdded.Text = "Quantity must be numeric"
                            lblAdded.Visible = True
                            Exit Sub
                        End If
                        'Added by Sandeep for performance Issue 
                        oOrder = ShoppingCart1.GetOrderObject(Page.Session, Page.User, Page.Application)
                        lstProductID = ShoppingCart1.CreateProductIDList(oOrder)
                        If lstProductID Is Nothing Then
                            bFound = False
                            iItemForUpdate = 0
                        Else
                            If lstProductID.Contains(Me.ProductID) Then
                                bFound = True
                                For i As Integer = 0 To lstProductID.Count - 1
                                    If lstProductID(i) = Me.ProductID Then
                                        iItemForUpdate = i
                                        Exit For
                                    End If
                                Next
                            Else
                                If lstProductID.Count = 1 Or lstProductID Is Nothing Then
                                    bFound = False
                                    iItemForUpdate = lstProductID.Count
                                Else
                                    bFound = False
                                    iItemForUpdate = lstProductID.Count - 1
                                End If

                            End If
                        End If
                        'Anil Issue 14302
                        If ShoppingCart1.AddToCart(oOrder, Me.ProductID, Me.User1.PersonID, bFound, iItemForUpdate, bCombineLines, , iQty) Then
                            lstProductID = ShoppingCart1.CreateProductIDList(oOrder)
                            If ShoppingCart1.GetProductTypeWebPages(Me.ProductID, sProductPage, sOrderPage) Then
                                'oOrder = ShoppingCart1.GetOrderObject(Page.Session, Page.User, Page.Application)
                                Session("ProductID") = Me.ProductID
                                If oOrder.SubTypes("OrderLines").Count = 1 Then
                                    Dim bIncludeInShipping As Boolean
                                    Dim oShipmentTypes As New Aptify.Framework.Web.eBusiness.CommonMethods(DataAction, AptifyApplication, User1, Database)
                                    Dim dt As DataTable
                                    bIncludeInShipping = oShipmentTypes.IncludeInShipping(Me.ProductID)
                                    If bIncludeInShipping Then
                                        dt = oShipmentTypes.LoadShipmentType(CInt(oOrder.GetValue("ShipToCountryCodeID")))
                                        'Suraj Issue 16262, 5/13/13 , check the datatable rows is greater than zero or not
                                        If dt.Rows.Count > 0 Then
                                            oOrder.SetValue("ShipTypeID", dt.Rows(0)("ID"))
                                        End If
                                    End If
                                End If
                                If Len(sOrderPage) > 0 Then
                                    'Anil Issue 12660

                                    ' special order page. redirect there now




                                    'Dim iItemForUpdate As Integer = 0
                                    'If Session("ProductID") IsNot Nothing AndAlso CLng(Session("ProductID")) > -1 Then
                                    '    For iItem = 0 To oOrder.SubTypes("OrderLines").Count - 1
                                    '        If CLng(oOrder.SubTypes("OrderLines").Item(iItem).GetValue("ProductID")) = CLng(Session("ProductID")) AndAlso _
                                    '           CLng(oOrder.SubTypes("OrderLines").Item(iItem).GetValue("ParentSequence")) <= 0 Then
                                    '            'bFound = True
                                    '            iItemForUpdate = iItem
                                    '            'Exit For
                                    '        End If
                                    '    Next
                                    'End If


                                    'If ShoppingCart1.GetOrderObject(Page.Session, Page.User, Page.Application) IsNot Nothing AndAlso ShoppingCart1.GetOrderObject(Page.Session, Page.User, Page.Application).SubTypes("OrderLines") IsNot Nothing Then
                                    '    With ShoppingCart1.GetOrderObject(Page.Session, Page.User, Page.Application).SubTypes("OrderLines").Item(iItemForUpdate)
                                    '        .SetValue("AutoRenew", ChkAutoRenew.Checked)

                                    '    End With
                                    'End If
                                    'Me.SetTotalCartQty()
                                    ShoppingCart1.SaveCart(Me.ProductID, ChkAutoRenew.Checked, Page.Session)
                                    Response.Redirect(sOrderPage & "?OL=" & _
                                                      oOrder.SubTypes("OrderLines").Count - 1)
                                    'duplicate code
                                    'lblAdded.Text = "Product Added to Cart"
                                    'lblAdded.ForeColor = Drawing.Color.Blue
                                    'lblAdded.Visible = True


                                Else
                                    'Dim iItemForUpdate As Integer = 0
                                    'If Session("ProductID") IsNot Nothing AndAlso CLng(Session("ProductID")) > -1 Then
                                    '    For iItem = 0 To oOrder.SubTypes("OrderLines").Count - 1
                                    '        If CLng(oOrder.SubTypes("OrderLines").Item(iItem).GetValue("ProductID")) = CLng(Session("ProductID")) AndAlso _
                                    '           CLng(oOrder.SubTypes("OrderLines").Item(iItem).GetValue("ParentSequence")) <= 0 Then
                                    '            'bFound = True
                                    '            iItemForUpdate = iItem
                                    '            'Exit For
                                    '        End If
                                    '    Next
                                    'End If
                                    'If ShoppingCart1.GetOrderObject(Page.Session, Page.User, Page.Application) IsNot Nothing AndAlso ShoppingCart1.GetOrderObject(Page.Session, Page.User, Page.Application).SubTypes("OrderLines") IsNot Nothing Then
                                    '    With ShoppingCart1.GetOrderObject(Page.Session, Page.User, Page.Application).SubTypes("OrderLines").Item(iItemForUpdate)
                                    '        .SetValue("AutoRenew", ChkAutoRenew.Checked)

                                    '    End With
                                    'End If
                                    Me.SetTotalCartQty()
                                    Session("ProductID") = Nothing

                                    'Context.Items("ShoppingCartObj") = Me.ShoppingCart1
                                    ' no order page
                                    'duplicate code
                                    'lblAdded.Visible = True
                                    'lblAdded.ForeColor = Drawing.Color.Blue
                                    'lblAdded.Text = "Product Added to Cart"
                                End If

                                'Me.SetTotalCartQty()

                            End If
                            Session("ProductID") = Nothing

                            lblAdded.Text = "Product Added to Cart"
                            lblAdded.ForeColor = Drawing.Color.Blue
                            lblAdded.Visible = True
                            lnkViewCart.Visible = True
                            ShoppingCart1.SaveCart(Me.ProductID, ChkAutoRenew.Checked, Page.Session)
                            'Neha changes for Issue 18747,09/5/2014
                            If oOrder IsNot Nothing Then
                                ''Set Badge information when it is kit product and has a meeting as one of the sub product.
                                SetupExtendedOrderDetailsforkitType(oOrder)
                                ''added by Suvarna for issue Id  - 17187
                                ''Set Badge information when it is GROUPING product and has a meeting as one of the sub product.
                                oProduct = DirectCast(AptifyApplication.GetEntityObject("Products", Me.ProductID), Aptify.Applications.ProductSetup.ProductObject)
                                If oProduct.ProductKitType = ProductData.ProductKitType.ProductGroup Then
                                    SetupExtendedOrderDetailsforProdGroupType(oOrder)
                                End If
                                ShoppingCart1.SaveCart(Me.ProductID, ChkAutoRenew.Checked, Page.Session)
                            End If

                            MyBase.Response.Redirect(GroupingContentsViewProductPage + "?ID=" + Me.ProductID.ToString + "&Val2=" + "Product Added to Cart", False)
                        Else
                            Session("ProductID") = Nothing
                            lblAdded.ForeColor = Drawing.Color.Red
                            lblAdded.Visible = True
                            ''RashmiP, Issue 9511, 09/08/10
                            RemoveHandler lnkAddToCart.Click, AddressOf lnkAddToCart_ServerClick
                            If ShoppingCart1.WebProdPreRequisiteErrMsg = String.Empty Then
                                lblAdded.Text = "Unable to Add Product"
                            Else
                                lblAdded.Text = ShoppingCart1.WebProdPreRequisiteErrMsg
                            End If
                            lnkViewCart.Visible = False
                        End If
                    End If
                Else
                    Session("ReturnToPage") = Request.RawUrl
                    'Sheetal:19707:Added false parameter in response.redirect as thread is being aborted.
                    Response.Redirect(LoginPage + "?ReturnURL=" & System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(Request.RawUrl)), False)
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        'Added funtion for Issue 18747, 09/05/2014
        Private Sub SetupExtendedOrderDetailsforkitType(ByRef oOrder As OrdersEntity)
            Try
                Dim ProductGE As Aptify.Applications.ProductSetup.ProductObject
                Dim dtBadgeInformation As DataTable = Nothing
                Dim strAttendeeID As String = String.Empty
                Dim strBadgeName As String = String.Empty, strBadgeCompanyName As String = String.Empty, strBadgeTitle As String = String.Empty, strBadgeEmailID As String = String.Empty
                dtBadgeInformation = LoadBadgeInfo(Me.User1.PersonID)

                For Each ol As OrderLinesEntity In oOrder.SubTypes("OrderLines")
                    With ol
                        Dim iProductID As Long = CLng(ol.GetValue("ProductID"))
                        ProductGE = CType(AptifyApplication.GetEntityObject("Products", iProductID), Aptify.Applications.ProductSetup.ProductObject)
                        If iProductID > 0 Then
                            If "Meeting" = ShoppingCart1.GetProductType(iProductID) AndAlso ol.IsPartofKit = True AndAlso Convert.ToInt64(ol.ExtendedOrderDetailEntity.GetValue("AttendeeID")) = Me.User1.PersonID Then
                                If Not IsNothing(.ExtendedOrderDetailEntity) Then
                                    'Add Badge Information to OrdermeetingDetails
                                    If dtBadgeInformation IsNot Nothing AndAlso dtBadgeInformation.Rows.Count > 0 Then
                                        If String.IsNullOrEmpty(strBadgeName) Then
                                            strBadgeName = Convert.ToString(dtBadgeInformation.Rows(0)("FirstName"))
                                        End If
                                        If String.IsNullOrEmpty(strBadgeCompanyName) Then
                                            strBadgeCompanyName = Convert.ToString(dtBadgeInformation.Rows(0)("CompanyName"))
                                        End If
                                        If String.IsNullOrEmpty(strBadgeTitle) Then
                                            strBadgeTitle = Convert.ToString(dtBadgeInformation.Rows(0)("Title"))
                                        End If
                                        If String.IsNullOrEmpty(strBadgeEmailID) Then
                                            strBadgeEmailID = Convert.ToString(dtBadgeInformation.Rows(0)("EmailID"))
                                        End If
                                        If String.IsNullOrEmpty(strAttendeeID) Then
                                            strAttendeeID = Convert.ToString(dtBadgeInformation.Rows(0)("AttendeeID"))
                                        End If
                                    End If

                                End If
                                'Set the Badge information for the Meeting to OrdermeetingDetails
                                .ExtendedOrderDetailEntity.SetValue("BadgeName", strBadgeName)
                                .ExtendedOrderDetailEntity.SetValue("BadgeCompanyName", strBadgeCompanyName)
                                .ExtendedOrderDetailEntity.SetValue("BadgeTitle", strBadgeTitle)
                                .ExtendedOrderDetailEntity.SetValue("AttendeeID", strAttendeeID)
                                .ExtendedOrderDetailEntity.SetValue("AttendeeID_Email", strBadgeEmailID)
                                'Set values in temp field
                                .SetAddValue("__ExtendedAttributeObjectData", .ExtendedOrderDetailEntity.GetObjectData(False))
                            End If
                        End If
                    End With
                Next
            Catch ex As Exception
            End Try
        End Sub
        'Neha changes for Issue 18747, 09/05/2014
        ''Retrive Badge Information For meeting Order
        Protected Function LoadBadgeInfo(ByVal lAttendeeID As Long) As DataTable
            Dim sSQL As String = String.Empty
            Dim dtbadgeInfo As DataTable = Nothing
            sSQL = "SELECT VP.ID as AttendeeID,VP.FirstName,VP.Title,VC.Name As CompanyName,VP.Email1 as EmailID  FROM " & AptifyApplication.GetEntityBaseDatabase("Persons") & ".." & AptifyApplication.GetEntityBaseView("Persons") & " VP Left Outer join " & AptifyApplication.GetEntityBaseDatabase("Companies") & ".." & AptifyApplication.GetEntityBaseView("Companies") & " VC on VP.CompanyID =VC.ID Where VP.ID =" & lAttendeeID
            dtbadgeInfo = DataAction.GetDataTable(sSQL, Framework.DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
            Return dtbadgeInfo
        End Function
        Private Sub SetTotalCartQty()

            Dim oOrder As Aptify.Framework.BusinessLogic.GenericEntity.AptifyGenericEntityBase
            oOrder = Me.ShoppingCart1.GetOrderObject(Session, Page.User, Page.Application)
            Dim iItemForUpdate As Integer = 0
            Dim i As Integer = 0
            Dim qty As Integer = 0

            With oOrder.SubTypes("OrderLines")
                For i = 0 To .Count - 1
                    qty = qty + CInt(.Item(i).GetValue("Quantity"))
                    If CLng(.Item(i).GetValue("ProductID")) = CLng(Session("ProductID")) AndAlso _
                                           CLng(.Item(i).GetValue("ParentSequence")) <= 0 Then
                        oOrder.SubTypes("OrderLines").Item(i).SetValue("AutoRenew", ChkAutoRenew.Checked)
                    End If

                Next


            End With
            If oOrder IsNot Nothing AndAlso oOrder.SubTypes("OrderLines") IsNot Nothing Then
                With oOrder.SubTypes("OrderLines").Item(iItemForUpdate)

                End With
            End If

            Me.TotalCartQty = qty
        End Sub


        Protected Sub btnNewVersion_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewVersion.Click
            Response.Redirect(Me.FixTemplateSourceDirectoryPath(Me.Request.Path) & "?" & Me.QueryStringRecordIDParameter & "=" & Me.NewProductVersionID)
        End Sub

        'Protected Sub lnkViewCart_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkViewCart.Click
        '    Response.Redirect(ViewcartPage)
        'End Sub

        Protected Sub lnkViewCart_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkViewCart.Click
            Response.Redirect(ViewcartPage)
        End Sub

        Private Sub SetupExtendedOrderDetailsforProdGroupType(ByRef oOrder As OrdersEntity)
            Try
                Dim ProductGE As Aptify.Applications.ProductSetup.ProductObject
                Dim dtBadgeInformation As DataTable = Nothing
                Dim strAttendeeID As String = String.Empty
                Dim strBadgeName As String = String.Empty, strBadgeCompanyName As String = String.Empty, strBadgeTitle As String = String.Empty, strBadgeEmailID As String = String.Empty
                dtBadgeInformation = LoadBadgeInfo(Me.User1.PersonID)

                For Each ol As OrderLinesEntity In oOrder.SubTypes("OrderLines")
                    With ol
                        Dim iProductID As Long = CLng(ol.GetValue("ProductID"))
                        ProductGE = CType(AptifyApplication.GetEntityObject("Products", iProductID), Aptify.Applications.ProductSetup.ProductObject)
                        If iProductID > 0 Then
                            If "Meeting" = ShoppingCart1.GetProductType(iProductID) AndAlso Convert.ToInt64(ol.ExtendedOrderDetailEntity.GetValue("AttendeeID")) = Me.User1.PersonID Then
                                If Not IsNothing(.ExtendedOrderDetailEntity) Then
                                    'Add Badge Information to OrdermeetingDetails
                                    If dtBadgeInformation IsNot Nothing AndAlso dtBadgeInformation.Rows.Count > 0 Then
                                        If String.IsNullOrEmpty(strBadgeName) Then
                                            strBadgeName = Convert.ToString(dtBadgeInformation.Rows(0)("FirstName"))
                                        End If
                                        If String.IsNullOrEmpty(strBadgeCompanyName) Then
                                            strBadgeCompanyName = Convert.ToString(dtBadgeInformation.Rows(0)("CompanyName"))
                                        End If
                                        If String.IsNullOrEmpty(strBadgeTitle) Then
                                            strBadgeTitle = Convert.ToString(dtBadgeInformation.Rows(0)("Title"))
                                        End If
                                        If String.IsNullOrEmpty(strBadgeEmailID) Then
                                            strBadgeEmailID = Convert.ToString(dtBadgeInformation.Rows(0)("EmailID"))
                                        End If
                                        If String.IsNullOrEmpty(strAttendeeID) Then
                                            strAttendeeID = Convert.ToString(dtBadgeInformation.Rows(0)("AttendeeID"))
                                        End If
                                    End If

                                End If
                                'Set the Badge information for the Meeting to OrdermeetingDetails
                                .ExtendedOrderDetailEntity.SetValue("BadgeName", strBadgeName)
                                .ExtendedOrderDetailEntity.SetValue("BadgeCompanyName", strBadgeCompanyName)
                                .ExtendedOrderDetailEntity.SetValue("BadgeTitle", strBadgeTitle)
                                .ExtendedOrderDetailEntity.SetValue("AttendeeID", strAttendeeID)
                                .ExtendedOrderDetailEntity.SetValue("AttendeeID_Email", strBadgeEmailID)
                                'Set values in temp field
                                .SetAddValue("__ExtendedAttributeObjectData", .ExtendedOrderDetailEntity.GetObjectData(False))
                            End If
                        End If
                    End With
                Next
            Catch ex As Exception
            End Try
        End Sub
    End Class
End Namespace

