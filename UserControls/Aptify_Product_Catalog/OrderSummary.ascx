<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="OrderSummary.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.ProductCatalog.OrderSummaryControl" %>
<%@ Register TagPrefix="uc1" TagName="CartGrid" Src="CartGrid.ascx" %>
<%@ Register TagPrefix="uc2" TagName="NameAddressBlock" Src="../Aptify_General/NameAddressBlock.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessShoppingCart" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<script language="javascript" type="text/javascript">
    window.history.forward(1);
</script>
<%--Nalini Issue#12578--%>
<div class="table-div">
    <div class="row-div clearfix"><div class="float-left label right-margin" align="right">
            Sub-Total:
        </div>
        <div class="float-right">
            <asp:Label ID="lblSubTotal" runat="server"></asp:Label>
        </div>
        
    </div>
    <div class="row-div clearfix"><div class="float-left label right-margin" align="right">
            Shipping &amp; Handling:</div>
        <div class="float-right">
            <asp:Label ID="lblShipping" runat="server"></asp:Label>
        </div>
        
    </div>
    <div class="row-div clearfix"><div class="float-left label right-margin" align="right">
            Tax:</div>
        <div class="float-right">
            <asp:Label ID="lblTax" runat="server"></asp:Label></div>
        
    </div>
    <div class="row-div clearfix"><div class="float-left label right-margin" align="right">
            Total:
        </div>
        <div class="float-right">
            <asp:Label ID="lblTotal" runat="server"></asp:Label></div>
        
    </div>
</div>
<cc1:AptifyShoppingCart runat="Server" ID="ShoppingCart1" />
