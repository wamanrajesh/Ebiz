<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ChangeAddress.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.ProductCatalog.ChangeAddressControl" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessShoppingCart" %>
<div id="tblMain" runat="server">
    <div class="row-div">
        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
    </div>
    <div class="row-div">
        <div>
            <asp:HyperLink ID="lnkNewAddress" runat="server">Click here to add a new address</asp:HyperLink></div>
        <div class="top-margin">
            Choose a
            <asp:Label ID="lblType" runat="server">Shipping/Billing</asp:Label>&nbsp;Address
        </div>
    </div>
    <div class="row-div">
        <%--Nalini Issue#13102--%>
        <asp:DataList ID="lstAddress" runat="server" RepeatColumns="2" Width="100%">
            <SelectedItemStyle></SelectedItemStyle>
            <HeaderTemplate>
                <font class="tdchangeAddressBackground">Address Book</font>
            </HeaderTemplate>
            <AlternatingItemStyle></AlternatingItemStyle>
            <ItemStyle Width="350px" VerticalAlign="Bottom" HorizontalAlign="NotSet"></ItemStyle>
            <ItemTemplate>
                <div style="vertical-align: bottom;" class="divstylechangeAddress">
                    <font size="2"><b>
                        <%# DataBinder.Eval(Container.DataItem, "Type")%></b><br />
                        <%# DataBinder.Eval(Container.DataItem,"AddressLine1") %>
                        <%# DataBinder.Eval(Container.DataItem,"AddressLine2") %>
                        <%# DataBinder.Eval(Container.DataItem,"AddressLine3") %>
                        <%# DataBinder.Eval(Container.DataItem,"City") %>&nbsp;&nbsp;<%# DataBinder.Eval(Container.DataItem,"State") %>
                        <%# DataBinder.Eval(Container.DataItem,"ZipCode") %>
                        <br>
                        <%# DataBinder.Eval(Container.DataItem,"Country") %>
                    </font>
                    <%--Nalini Issue#12578--%>
                </div>
                <div class="top-margin">
                    <asp:Button ID="cmdUseAddress" AlternateText="Use this address" runat="server" CssClass="submit-Btn"
                        Text="Use this Address" />
                    <asp:Button ID="cmdEditAddress" AlternateText="Edit Address" runat="server" Text="Edit"
                        CssClass="submit-Btn" />
                </div>
            </ItemTemplate>
            <FooterStyle></FooterStyle>
            <HeaderStyle></HeaderStyle>
        </asp:DataList>
    </div>
</div>
<cc2:User runat="Server" ID="User1" />
<cc1:AptifyShoppingCart runat="server" ID="ShoppingCart1" />
