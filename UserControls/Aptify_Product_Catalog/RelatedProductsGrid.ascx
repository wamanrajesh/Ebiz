<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="RelatedProductsGrid.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.ProductCatalog.RelatedProductsGrid" %>
<h2 id="H1" runat="server" class="textfontsub">
    Related Products</h2>
<div id="divGrid" runat="server" class="table-div">
    <%--Suvarna Deshmukh IssueID-12433,12430 and 12434 On Dec 13,2011 commented and added to implement new designs for ebusiness--%>
    <asp:DataList ID="grdMain" runat="server" HorizontalAlign="Left">
        <ItemTemplate>
            <div class="row-div-bottom-dotted-line clearfix">
                <div class="product-image-div">
                    <asp:Image ID="ImgProd" runat="server" CssClass="Image" />
                </div>
                <%--<div class="rTableCell"></div>--%>
                <div class="product-detail-div1">
                    <asp:HyperLink ID="lnkProduct" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"WebName") %>'
                        Font-Bold="true"></asp:HyperLink>
                    <div>
                        <asp:Label ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"GridDescription") %>'
                            Font-Size="12px"></asp:Label>
                    </div>
                    <%--Rashmi P, Issue 13150,8/21/12, The prompt text is not displayed for related product --%>
                    <div>
                        <asp:Label ID="lblWebPrompttext" runat="server" Text='<%# databinder.eval(container.dataitem,"PromptText") %>'></asp:Label>
                    </div>
                </div>
            </div>
        </ItemTemplate>
    </asp:DataList>
</div>
