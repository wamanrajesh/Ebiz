'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Namespace Aptify.Framework.Web.eBusiness.ProductCatalog
    Partial Class ProductCategoryControl
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced

        Protected Const ATTRIBUTE_PRODUCT_CATEGORY_LINK_STRING_NON_ROOT_PAGE As String = "ProductCategoryLinkStringNonRootPage"
        Protected Const ATTRIBUTE_PRODUCT_CATEGORY_LINK_STRING_ROOT_PAGE As String = "ProductCategoryLinkStringRootPage"
        Protected Const ATTRIBUTE_PRODUCT_LISTING_GRID_VIEW_PRODUCT_PAGE As String = "ProductListingGridViewProductPage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "ProductCategory"

#Region "ProductCategory Specific Properties"
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
        ''' ProductListingGridViewProduct page url
        ''' </summary>
        Public Overridable Property ProductListingGridViewProductPage() As String
            Get
                If Not ViewState(ATTRIBUTE_PRODUCT_LISTING_GRID_VIEW_PRODUCT_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_PRODUCT_LISTING_GRID_VIEW_PRODUCT_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_PRODUCT_LISTING_GRID_VIEW_PRODUCT_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
#End Region

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

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
            If String.IsNullOrEmpty(ProductListingGridViewProductPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                ProductListingGridViewProductPage = Me.GetLinkValueFromXML(ATTRIBUTE_PRODUCT_LISTING_GRID_VIEW_PRODUCT_PAGE)
            End If

            'if values are not provide directly or from the XML file, set default values for inherited properties since 
            'control requires them to properly function
            If String.IsNullOrEmpty(Me.QueryStringRecordIDParameter) Then Me.QueryStringRecordIDParameter = "ID"
        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
            Try
                ' ensure that the ProductCategoryLinkString1 control
                ' does not use a hard-coded path            RFB 7/25/03
                'ProductCategoryLinkString1.RootCategoryURL = ProductCategoryLinkStringRootPage

                If Not IsPostBack Then
                    Me.SetControlRecordIDFromParam()

                    With Me.ProductCategoryLinkString1
                        .CategoryID = Me.ControlRecordID
                        If Not String.IsNullOrEmpty(ProductCategoryLinkStringNonRootPage) Then
                            .URL = ProductCategoryLinkStringNonRootPage
                            .Separator = ":"
                        Else
                            .Enabled = False
                            .ToolTip = "ProductCategoryLinkStringNonRootPage property not set."
                        End If
                    End With

                    LoadGrid(Me.ControlRecordID)
                    ''Code Commetented by Suvarna D IssueID-12735 Product Category Mouse hover should display sub categories. on Jan 18, 2012
                    'User control product categories has been removed from this page hence code commented
                    'ProdCategories.ParentID = Me.ControlRecordID
                    ''End of Code Commetented by Suvarna D IssueID-12735 Product Category Mouse hover should display sub categories. on Jan 18, 2012


                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub LoadGrid(ByVal lCategoryID As Long)
            Try
                If Not String.IsNullOrEmpty(ProductListingGridViewProductPage) Then
                    ProdListingGrid.NavigateURLFormatField = ProductListingGridViewProductPage & "?ID={0}"
                Else
                    ProdListingGrid.NavigateURLFormatField = String.Empty
                End If
                'Suvarna D IssueID-12745 to implement webImage feature to display images on Jan 19, 2012
                'Code commented and added to get a controlrecordid on productlistinggrid control
                'ProdListingGrid.LoadGrid(lCategoryID)
                If ProdListingGrid.SetControlRecordIDFromQueryString() Then
                    ProdListingGrid.LoadGrid(lCategoryID)
                End If
                'End of addition by Suvarna D IssueID-12745 to implement webImage feature to display images on Jan 19, 2012


            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
  
    End Class
End Namespace
