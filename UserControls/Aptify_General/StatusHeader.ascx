<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="StatusHeader.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.ProductCatalog.HeaderControl" %>
<%@ Register TagPrefix="uc1" TagName="ItemsInCart" Src="~/UserControls/Aptify_Product_Catalog/ItemsInCart.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessLogin" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessShoppingCart" %>
<div id="tblHeaderMain" runat="server" class="table-div clearfix">
    <div id="tdUserName" runat="server" class="float-left">
        <span class="label">Welcome:</span>
        <asp:Label ID="lblUserName" runat="server" Text="User Name!" CssClass="label">
        </asp:Label>
        <asp:Label ID="lblGrpAdmin" runat="server" >
        </asp:Label>
        <asp:Label ID="lblCampany" runat="server" >
        </asp:Label>
    </div>
    
    <div id="tdSignout" runat="server" class="float-right label pt3">
        <asp:LinkButton ID="ImgLogout" runat="server"  Text="Sign out"
            CausesValidation="false">
        </asp:LinkButton>
    </div>
    <div runat="server" id="tdItemInCart" class="right-margin  float-right">
        <div id="tblItemincart" runat="server" >
            <uc1:ItemsInCart ID="ItemsInCart" runat="server" />
        </div>
    </div>
</div>
<cc1:User ID="User1" runat="server" />
<cc2:AptifyWebUserLogin ID="WebUserLogin" runat="server" />
<cc2:AptifyShoppingCart ID="ShoppingCart" runat="server"></cc2:AptifyShoppingCart>
