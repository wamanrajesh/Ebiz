<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="vb" AutoEventWireup="false" CodeFile="itemsincart.ascx.vb"
    Inherits="aptify.framework.web.ebusiness.itemsincart" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessShoppingCart" %>


<div id="tblItemcount">
    <asp:Label ID="lblViewcart" runat="server" Text="View Cart" ></asp:Label>
    <img id="imgCart" src="" runat="server" />
    <asp:Label ID="lblItemsInCart" runat="server" Text="Label" CssClass="label"></asp:Label>
</div>
<cc2:AptifyShoppingCart ID="ShoppingCart1" runat="server"></cc2:AptifyShoppingCart>
