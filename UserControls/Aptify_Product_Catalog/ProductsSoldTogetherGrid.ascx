<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ProductsSoldTogetherGrid.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.ProductCatalog.ProductsSoldTogether" %>
<asp:Label ID="lblError" runat="server"></asp:Label>
<asp:DataList ID="DataList1" runat="server" GridLines="Horizontal" HorizontalAlign="Left"
    RepeatColumns="4" RepeatDirection="Horizontal" Width="100%" BorderColor="#FFC080"
    BorderStyle="Solid" BorderWidth="1px">
    <HeaderTemplate>
        Continue Shopping: People who bought this also bought...
    </HeaderTemplate>
    <ItemTemplate>
        <div class="clearfix">
            <div class="row-div clearfix">
                <a href='<%# DataBinder.Eval(Container.DataItem,"ProdPageURL") %>' runat="server"
                    id="lnkRelImage">
                    <img alt="ImageUrl" src='<%# DataBinder.Eval(Container.DataItem,"ProdImageURL") %>' /></a>
            </div>
            <div class="row-div clearfix">
                <a href='<%# DataBinder.Eval(Container.DataItem,"ProdPageURL") %>' runat="server"
                    id="lnkRelName">
                    <asp:Label ID="lblProdName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Label></a>
            </div>
            <div class="row-div clearfix">
                <asp:Label ID="lblGridDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"gridDescription") %>'></asp:Label>
            </div>
        </div>
    </ItemTemplate>
    <FooterTemplate>
    </FooterTemplate>
    <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" />
    <HeaderStyle BackColor="#FFC080" BorderColor="#FFC080" />
</asp:DataList>
