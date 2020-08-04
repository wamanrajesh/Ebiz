<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SubscriptionProductDetails.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.ProductCatalog.SubscriptionProductDetails" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessShoppingCart" %>
<div class="errormsg-div">
    <asp:Label ID="lblError" runat="server" Text="Label" Visible="False"></asp:Label></div>
<div id="tblMain" class="table-div" runat="server">
    <div class="row-div clearfix">
        <div class="label-div w19">
            Product:
        </div>
        <div class="field-div1 w79">
            <asp:Label ID="lblproduct" runat="server" />
        </div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w19">
            Auto Renew:
        </div>
        <div class="field-div1 w79">
            <asp:CheckBox ID="chkAutoRenew" runat="server" />
        </div>
    </div>
    <div class="row-div clearfix">
        <div class="w19 emptyspace float-left">
            &nbsp;
        </div>
        <div class="w79 float-left">
           &nbsp; <asp:Button ID="btnUpdate" runat="server" Text="OK" CssClass="submit-Btn" />
        </div>
    </div>
</div>
<cc1:aptifyshoppingcart id="ShoppingCart1" runat="server" />
