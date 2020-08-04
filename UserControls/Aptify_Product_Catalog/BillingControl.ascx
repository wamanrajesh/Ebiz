<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="BillingControl.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.ProductCatalog.BillingControl" %>
<%@ Register TagPrefix="uc1" TagName="CartGrid2" Src="CartGrid2.ascx" %>
<%@ Register TagPrefix="uc2" TagName="OrderSummary" Src="OrderSummary.ascx" %>
<%@ Register TagPrefix="uc3" TagName="NameAddressBlock" Src="../Aptify_General/NameAddressBlock.ascx" %>
<%@ Register TagPrefix="uc4" TagName="CreditCard" Src="../Aptify_General/CreditCard.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessShoppingCart" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<script language="javascript" type="text/javascript">
    window.history.forward(1);
</script>
<%--Nalini Issue#12578--%>
<div class="table-div" id="tblMain" runat="server">
    <div class="row-div">
        <asp:Label ID="lblError" runat="server" Text="lblError" Visible="False"></asp:Label>
    </div>
    <div class="row-div">
        
            <asp:Label ID="lblGotItems" runat="server">
 <p class="label">Please review and submit your order</p>
 Your default shipping address and other settings are shown below. Use
 the buttons to make any changes. When you're done, click the "Complete Order"
 button.
            </asp:Label>
            <asp:Label ID="lblNoItems" runat="server" Visible="False" ForeColor="Maroon">
 There are no items in your shopping cart.
            </asp:Label>
                <cc2:User ID="User2" runat="server" />
            
    </div>
    <div class="row-div clearfix" id="tblRowMain" runat = "server">
        <div class="float-left w49 billing-detail-div">
            <div class="billing-title">
                Billing Details
            </div>
            <div class="billing-data-div">
                <div class="row-div clearfix">
                    <div class="label-div w25">
                        <strong>Billing To: </strong>
                    </div>
                    <div class="field-div1">
                        <uc3:NameAddressBlock ID="NameAddressBlock" runat="server" />
                    </div>
                </div>
                <div class="row-div clearfix">
                    <div class="label-div w25">
                        &nbsp;</div>
                    <div class="field-div1">
                        <%-- <asp:HyperLink ID="lnkChangeAddress" runat="server">--%>
                        <%--<img id="imgChangeAddress" runat="server" alt="Change Bill Address" src="" border="0" />--%>
                        <%-- </asp:HyperLink>--%>
                        <asp:Button CssClass="submit-Btn" ID="lnkChangeAddress" runat="server" Text="Change Address"
                            CausesValidation="false" />
                    </div>
                </div>
            </div>
        </div>
        <div class="float-right w49 payment-information-div">
            <div class="payment-title">
                Payment Information
            </div>
            <div class="payment-data-div">
                <uc4:CreditCard ID="CreditCard" runat="server"></uc4:CreditCard>
            </div>
        </div>
        <div class="clear"></div>
        <div class="row-div">
            <asp:Image ID="imgShoppingCart" runat="server" ImageUrl="~/Images/shoppingCartShipping.gif" />
            <span class="label">Items in shopping cart</span>
        </div>
        <%--Rashmi P, Issue 5133, Add ShipmentType Selection --%>
        <div id="tdShipment" runat="server">
            <strong><font size="2">Shipping Method:</font></strong>&nbsp;<asp:DropDownList runat="server"
                ID="ddlShipmentType" AutoPostBack="true">
            </asp:DropDownList>
        </div>
        <div class="row-div">
            <uc1:CartGrid2 ID="CartGrid2" runat="server"></uc1:CartGrid2>
        </div>
        <div class="float-right w29" align="right">
            <uc2:OrderSummary ID="OrderSummary" runat="server" />
        </div>
        </div>
        <div class="clear"></div>
        <div class="row-div" align="right">
            <asp:Button CssClass="submit-Btn" ID="cmdBack" runat="server" Text="<< Back" CausesValidation="False" />
            <asp:Button CssClass="submit-Btn" ID="cmdPlaceOrder" TabIndex="1" runat="server" Text="Complete Order">
            </asp:Button>
        </div>
 </div>
<cc1:AptifyShoppingCart runat="Server" ID="ShoppingCart1" />
<cc2:User runat="Server" ID="User1" />
