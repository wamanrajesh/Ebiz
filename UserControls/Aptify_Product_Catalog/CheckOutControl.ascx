<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="CheckoutControl.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.ProductCatalog.CheckoutControl" %>
<%@ Register TagPrefix="uc1" TagName="CartGrid" Src="CartGrid.ascx" %>
<%@ Register TagPrefix="uc1" TagName="OrderSummary" Src="OrderSummary.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ShippingControl" Src="ShippingControl.ascx" %>
<%@ Register TagPrefix="uc2" TagName="NameAddressBlock" Src="../Aptify_General/NameAddressBlock.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessShoppingCart" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%--Nalini Issue#12578--%>
<div id="tblMain" runat="server" class="rTable">
    <div class="row-div clearfix">
        <asp:Label ID="lblGotItems" runat="server">
            <p class="label">Please review and submit your order</p>
            <asp:Label ID="lblcheckoutMsg" runat="server"></asp:Label>
        </asp:Label>
        <asp:Label ID="lblNoItems" runat="server" Visible="False" ForeColor="Maroon">There are no items in your shopping cart.</asp:Label>
        <br />
        <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
        <p> 
                <cc2:User ID="User1" runat="server" />            
        </p>
    </div>
    <div class="row-div clearfix" id="tblRowMain" runat="server">
    <div  class="row-div clearfix">
        <span>
        <asp:Panel ID="pnlOrdertype" runat="server" GroupingText="Select Type of Order" Visible="false" >
            <asp:RadioButton ID="rbCompany" runat="server" Text="Corporate Order" GroupName ="OrderType" AutoPostBack=true />
            <asp:RadioButton ID="rbIndividual" Text="Personal Order" runat="server"  GroupName ="OrderType" AutoPostBack=true  />
        </asp:Panel>
         
        </span>
    </div>
      
        <div class="float-left w49 shipping-detail-div">
            <uc1:ShippingControl ID="ShippingControl" runat="server" />
        </div>
        <div class="float-right w49 order-summary-div">
            <div class="order-summary-title">
                Order Summary
            </div>
            <div class="order-summary-data-div float-right w52">
                <uc1:OrderSummary ID="OrderSummary1" runat="server" />
            </div>
            <div class="clear"></div>
        </div>
    </div>
    <div>
        <div class="row-div clearfix">
            <asp:Image ID="imgShoppingCart" runat="server" ImageUrl="~/Images/shoppingCartShipping.gif" />
            <strong><font size="2">Items in shopping cart</font></strong>
            <%--Rashmi P, Issue 5133, Add ShipmentType Selection --%>
            <div class="rTableCell" id="tdShipment" runat="server">
                <strong><font size="2">Shipping Method:</font></strong>&nbsp;<asp:DropDownList runat="server"
                    ID="ddlShipmentType" AutoPostBack="true">
                </asp:DropDownList>
            </div>
        </div>
        <div class="row-div clearfix">
            <uc1:CartGrid ID="CartGrid" runat="server"></uc1:CartGrid>
        </div>
        <div class="row-div clearfix" align="right">
            <asp:Button ID="cmdUpdateCart" runat="server" Text="Update" CssClass="submit-Btn"></asp:Button>
            <asp:Button ID="cmdNextStep" runat="server" CssClass="submit-Btn" Text="Next Step>>">
            </asp:Button>
        </div>
    </div>
</div>
<cc1:AptifyShoppingCart ID="ShoppingCart" runat="server" />
