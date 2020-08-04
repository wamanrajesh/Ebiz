<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="FeaturedProducts.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.ProductCatalog.FeaturedProductsControl" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessShoppingCart" %>
<div class="product-featured-title-div">
    <h2 id="H1" runat="server" class="textfontsub">
        Featured Products</h2>
</div>
<div id="divGrid" runat="server" class="table-div">
    <div class="row-div clearfix">
        The products shown below have been specially selected for you based on your areas
        of interest and previous purchases.
    </div>
    <div class="table-div clearfix">
        <asp:DataList ID="grdFeaturedProducts" runat="server" HorizontalAlign="Left">
            <ItemTemplate>
                <div class="row-div-bottom-dotted-line clearfix">
                    <div class="product-image-div">
                        <asp:Image ID="ImgProd" runat="server" CssClass="Image" />
                    </div>
                    <%--Aparna add Literal tag for showing data in proper format--%>
                    <div class="product-detail-div1">
                        <asp:HyperLink ID="lnkName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'
                            Font-Bold="true">
                        </asp:HyperLink>
                        <br />
                        <asp:Literal ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Description")%>'>
                        </asp:Literal>
                    </div>
                </div>
            </ItemTemplate>
        </asp:DataList>
    </div>
</div>
<cc1:User runat="server" ID="User1" />
<div id="noData" runat="server" class="content-container clearfix">
    <div class="table-div">
        From time to time, products will be listed here based on your areas of interest
        and past purchases.
    </div>
</div>
