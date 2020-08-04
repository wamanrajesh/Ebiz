<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ProdCategories.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.ProductCatalog.ProdCategories" %>
<%@ Register TagPrefix="uc1" TagName="RecommentedProduct" Src="RelatedProductsGrid.ascx" %>
<%@ Register TagPrefix="uc1" TagName="FeaturedProduct" Src="FeaturedProducts.ascx" %>
<%@ Register TagPrefix="uc1" TagName="FindProduct" Src="FindProduct.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ProdCategoryBar" Src="~/UserControls/Aptify_General/ProdCategoryBar.ascx" %>
<h1 runat="server" id="lblHeader">
    ICE Store
</h1>
<div id="outerDiv" class="product-container-div clearfix">
    <h2 runat="server" id="lblPageHeaderText">
    </h2>
    <div id="ProductCategoryDiv" class="left-container w15">
        <h6 runat="server" class="browse-product" id="lblProdCatHeader">
        </h6>
        <div>
            <uc1:prodcategorybar id="ProdCategoryBar" runat="server" />
        </div>
    </div>
    <div class="middle-container w66">
        <h2 runat="server" id="lblWelcomeText">
        </h2>
        <div class="find-product-div product-detail-margin">
            <uc1:findproduct id="FindProduct" headertext="{Find Product}" showheaderifempty="False"
                runat="server" />
        </div>
        <div>
            <uc1:featuredproduct id="FeaturedProducts" headertext="{Featured Product}" showheaderifempty="False"
                runat="server" />
        </div>
    </div>
    <div class="right-container w15">
        <asp:Image ID="ImgSideImage" runat="server" Height="100px" ImageUrl="" />
    </div>
</div>
