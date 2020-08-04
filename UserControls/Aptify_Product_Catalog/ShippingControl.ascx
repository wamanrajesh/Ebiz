<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ShippingControl.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.ProductCatalog.ShippingControl" %>
<%@ Register TagPrefix="uc1" TagName="CartGrid" Src="CartGrid.ascx" %>
<%@ Register TagPrefix="uc2" TagName="NameAddressBlock" Src="../Aptify_General/NameAddressBlock.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessShoppingCart" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%--Nalini Issue#12578--%>
<div class="shipping-title">
    Shipping Details
</div>
<div class="shipping-data-div">
    <div class="row-div clearfix">
        <div class="label-div w25">
            <strong><font size="2">Shipping To: </font></strong>
        </div>
        <div class="field-div1">
                <uc2:NameAddressBlock ID="NameAddressBlock" runat="server"></uc2:NameAddressBlock>           
        </div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w25">
            &nbsp;</div>
        <div class="field-div1">
            <asp:Button ID="lnkChangeAddress" runat="server" Text="Change Address" CssClass="submitBtn" />
              <asp:Button ID="lnkChangeShipTo" runat="server" Text="Change Ship To" CssClass="submitBtn" Visible = "false" />
        </div>
 
    </div>
</div>
<%-- <asp:HyperLink ID="lnkChangeAddress" runat="server">--%>
<%--<img runat="server" id="imgChangeAddress" alt="Change Ship Address" src="" border="0" />--%>
<%-- chnages by neha, Added css as per Shipping Details’s Change Address button, issue 16425,05/17/13--%>
<cc1:AptifyShoppingCart runat="Server" ID="ShoppingCart1" />
<cc2:User ID="User1" runat="server" />
