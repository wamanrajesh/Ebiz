<%--Aptify e-Business 5.5.1, July 2013--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Product.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.ProductCatalog.ProductControl" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness.ProductCatalog"
    Assembly="ProductCategoryLinkString" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessHierarchyTree" %>
<%@ Register TagPrefix="uc1" TagName="ProductTopicCodesGrid" Src="ProductTopicCodesGrid.ascx" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessShoppingCart" %>
<%@ Register TagPrefix="cc6" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="uc1" TagName="ProductGroupingContentsGrid" Src="ProductGroupingContentsGrid.ascx" %>
<%@ Register TagPrefix="uc1" TagName="RelatedProductsGrid" Src="RelatedProductsGrid.ascx" %>
<%@ Register TagPrefix="uc1" TagName="FeaturedProduct" Src="FeaturedProducts.ascx" %>
<cc1:ProductCategoryLinkString ID="ProductCategoryLinkString1" runat="server" HyperlinkRootCategory="True"
    RootCategoryText="All Categories" class="label"></cc1:ProductCategoryLinkString>
<h3 class="top-margin">
    <asp:Label ID="lblName" class="label" runat="server"></asp:Label></h3>

<asp:Label ID="lblMsg" runat="server"></asp:Label>
<div class="table-div">
    <div class="row-div clearfix">
        <div class="product-image-div">
            <asp:Image ID="imgProduct" runat="server" width="150px" >
            </asp:Image>
        </div>
        <%-- Anil changess for issue 12996  --%>
        <div class="product-detail-div w64">
            <%-- Aparna issue 9025,9042 for Add panel to hide controls for non-web enabled product--%>
            <div class="table-div" id="productdetailpanel" runat="server">
                <div class="row-div clearfix">
                    <div class="label-div w25">
                        <asp:Label ID="lblAvailable" runat="server" Text="Available:" ></asp:Label>
                    </div>
                    <div class="field-div1 w74">
                        <asp:Label ID="lblavailval" runat="server" ForeColor="Green" >In Stock</asp:Label>
                        <img alt="Item Not Available" id="imgNotAvailable" src="" visible="false" runat="server" />
                    </div>
                </div>
                <div class="row-div clearfix">
                    <div class="label-div w25">
                        <asp:Label ID="lblQuantity" runat="server" Text="Quantity:"></asp:Label>
                    </div>
                    <div class="field-div1 w74">
                        <asp:TextBox ID="txtQuantity" runat="server" CssClass="w15">1</asp:TextBox>
                        <asp:Label ID="lblSellingUnits" runat="server">Selling Units</asp:Label>
                    </div>
                </div>
                <div class="row-div clearfix">
                    <div class="label-div w25">
                        <asp:Label ID="lblPricing" runat="server" Text="Price:"></asp:Label>
                    </div>
                    <div class="field-div1 w74">
                        <asp:Label ID="lblPrice" runat="server" Text="-" ></asp:Label>
                    </div>
                </div>
                <div class="row-div clearfix">
                    <div class="label-div w25">
                        <asp:Label ID="lblChkAutoRenew" runat="server" Text="Auto Renew:" ></asp:Label>
                    </div>
                    <div class="field-div1 w74">
                        <asp:CheckBox ID="ChkAutoRenew" runat="server" />
                    </div>
                </div>
                <div class="row-div clearfix">
                    <div class="label-div w25">
                        &nbsp;</div>
                    <div class="field-div1 w74">
                        <asp:Button ID="lnkAddToCart" runat="server" Text="Add To Cart" CssClass="submit-Btn" />
                    </div>
                </div>
                <div class="row-div clearfix">
                    <div class="label-div w25">
                        &nbsp;</div>
                    <div class="field-div1 w74">
                        <asp:Label ID="lblAdded" Visible="False" runat="server"></asp:Label>                        
                    </div>
                </div>
                <div class="row-div clearfix">
                    <div class="label-div w25">&nbsp;</div>
                    <div class="field-div w74">
                        <asp:Button ID="lnkViewCart" runat="server" Text="View My Cart" CssClass="submit-Btn" />
                    </div>
                </div>
                <%-- Suvarna D Control alignment done for the IssueId 12720 on 19 Jan , 2012 --%>
                <div class="row-div clearfix">
                    <asp:Label ID="lblMemSavings" runat="server" ForeColor="DarkGreen" Visible="false"></asp:Label>
                </div>
                <div class="row-div clearfix" runat="server" id="trProductCode">
                    <div class="float-left">
                        <asp:Label ID="lblItemCode" runat="server" Text="Item Code:"></asp:Label>
                    </div>
                    <div class="float-left">
                        <asp:Label ID="lblCode" runat="server" Text="-"></asp:Label>
                    </div>
                </div>
                <%-- Suvarna D Aliggnment done for the IssueId 12720 on 19 Jan , 2012 --%>
                <div class="row-div clearfix">
                    <div class="float-left">
                        <asp:Label ID="lblNote" runat="server" Text="*Note: " Visible="False"></asp:Label>
                    </div>
                    <div class="float-left">
                        <asp:Label ID="lblProductMessage" runat="server" Visible="False">Label</asp:Label>
                    </div>
                </div>
                <%-- Suvarna D Aliggnment done for the IssueId 12720 on 19 Jan , 2012 --%>
                <div class="row-div">
                    <asp:Label ID="lblNewerProduct" runat="server" Text="A newer version of this product is available: " Visible="False"></asp:Label>
                </div>
                <div class="row-div">
                    <asp:Button ID="btnNewVersion" runat="server" Text="View latest product version" SkinID="Test" Visible="false" />
                </div>
                
            </div>
        </div>
        
    </div>
    <div class="row-div">
        <asp:Label ID="lblSummary" CssClass="label" runat="server" Text="Summary:" ></asp:Label>
    </div>
    <div class="row-div">
        <asp:Label ID="lblDescription" runat="server" Text="-"></asp:Label>
    </div>
    <div class="row-div">
        <asp:Label ID="lblprodDesc" runat="server" CssClass="label" Text="Product Description:"></asp:Label>
    </div>
    <div class="row-div">
        <asp:Label ID="lblLongDescription" runat="server" Text="Not Available" />
    </div>
    <div class="row-div">
        <uc1:ProductGroupingContentsGrid ID="ProductGroupingContentsGrid" runat="server"
            Visible="false" />
    </div>
</div>
<cc6:User ID="User1" runat="server" />
<cc2:AptifyShoppingCart ID="ShoppingCart1" runat="server" Visible="False"></cc2:AptifyShoppingCart>

