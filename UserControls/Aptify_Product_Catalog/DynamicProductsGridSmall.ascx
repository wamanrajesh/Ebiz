<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="DynamicProductsGridSmall.ascx.vb"
    Inherits="DynamicProductsGrid" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<div class="content-container clearfix">
    <asp:Label runat="server" ID="lblError" /><br />
    <asp:DataList ID="DataList1" runat="server">
        <HeaderTemplate>
            <asp:Label runat="server" ID="lblHeader"></asp:Label>
        </HeaderTemplate>
        <ItemTemplate>
            <div class="content-container">
                <div class="row-div clearfix">
                    <a href='<%# DataBinder.Eval(Container.DataItem,"ProdPageURL") %>'>
                        <img alt="ImageUrl" id="imgProduct" runat="server" src='<%# DataBinder.Eval(Container.DataItem,"ProdImageURL") %>' /></a>
                </div>
                <div class="row-div clearfix">
                    <a href='<%# DataBinder.Eval(Container.DataItem,"ProdPageURL") %>'>
                        <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Label></a>
                </div>
                <div class="row-div clearfix">
                    <asp:Label ID="lblProdName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"GridDescription") %>'></asp:Label>
                </div>
            </div>
        </ItemTemplate>
        <FooterTemplate>
        </FooterTemplate>
    </asp:DataList>
    <cc1:User runat="server" ID="User1" />
</div>
