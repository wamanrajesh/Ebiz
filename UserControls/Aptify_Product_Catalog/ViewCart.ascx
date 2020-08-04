<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ViewCart.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.ProductCatalog.ViewCartControl" %>
<%@ Register TagPrefix="uc1" TagName="CartGrid" Src="CartGrid.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%--Amruta, Issue 16769, Added ProcessIndicator.--%>

    <asp:UpdateProgress ID="updatePanelMain" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="updatePanelButton">
        <ProgressTemplate>
            
                    <div class="processing-div">
                        <div class="processing">
                            Please wait...
                        </div>
                    </div>
                
        </ProgressTemplate>
    </asp:UpdateProgress>

<div class="row-div">
    <uc1:CartGrid ID="CartGrid" runat="server"></uc1:CartGrid>
</div>
<div class="row-div" runat="server" id="tblRowNoItems">
    No Items In Cart
</div>
<div class="clearfix">
    <div class="float-left w52" id="divCampaign" runat="server">
        <div class="row-div clearfix tablecontrolsfontLogin">
            <p class="campaignboxViewcart" style="padding-left: 7px; padding-top: 7px;">
                <asp:Label ID="lblCampaignMsg" runat="server" Visible="False">[Msg]</asp:Label>
                <asp:Label ID="lblCampaignInstructions" runat="server" CssClass="label">If you have a campaign code, please enter:</asp:Label>
                <asp:TextBox ID="txtCampaign" runat="server" Width="70px"></asp:TextBox>
                <asp:Button CssClass="submit-Btn" ID="cmdApplyCampaign" runat="server" Text="Apply">
                </asp:Button>
                <asp:Button CssClass="submit-Btn" ID="cmdRemoveCampaign" runat="server" Text="Remove Campaign"
                    Visible="False"></asp:Button>
            </p>
        </div>
    </div>
    <div class="float-right view-cart-price-div"  id="divTotals" runat="server">        
            
            <div class="row-div clearfix">
                <div class="float-left label right-margin" align="right">
                    <asp:Label ID="Label4" runat="server">Sub-Total:</asp:Label>
                </div>
                <div class="float-right">
                    <asp:Label ID="lblSubTotal" runat="server"></asp:Label>
                </div>
            
            </div>
            
            <div class="row-div clearfix">
                <div class="float-left label right-margin" align="right">
                    <asp:Label ID="Label3" runat="server">Tax:</asp:Label>
                </div>
                <div class="float-right">
                    <asp:Label ID="lblTax" runat="server"></asp:Label>
                </div>
            
            </div>
           
            <div class="row-div clearfix">
                <div class="float-left label right-margin" align="right">
                    <asp:Label ID="Label2" runat="server">Shipping:</asp:Label>
                </div>
                <div class="float-right">
                    <asp:Label ID="lblShipping" runat="server"></asp:Label>
                </div>
            
            </div>
            
              <div class="row-div clearfix">  
                <div class="float-left label right-margin" align="right">
                    <asp:Label ID="Label1" runat="server">Total:</asp:Label>
                </div>
                <div class="float-right">
                    <asp:Label ID="lblGrandTotal" runat="server" ForeColor="#fd4310"></asp:Label>
                </div>
           
            </div>
            <%--HP - Issue 6465, per Ravi the following verbiage should be used for member savings--%>
            <span runat="server" id="spnSavings" visible="false">
                <asp:Label ID="lblTotalSavings" runat="server" ForeColor="Green">000</asp:Label>
            </span>
        </div>    
        <div class="clear"></div>
</div>
<%--   Issue Id # 12577 Nalini added Horrizontal Line
     <div id="divhr" runat="server" style="float: left;">
        <hr />
    </div> --%>
<div id="tblbuttons" runat="server">
    <hr />
    <%--Rashmi P, Issue 5133, Add ShipmentType Selection --%>
    <div class="row-div clearfix" runat="server" id="tdShipment">
        <strong><font size="2">Shipping Method:</font></strong>&nbsp;<asp:DropDownList runat="server"
            ID="ddlShipmentType" AutoPostBack="true">
        </asp:DropDownList>
        <%--Sandeep, Issue 5133, Add font size --%>
    </div>
    <div class="row-div clearfix">
        <div class="float-left w48">
            <asp:Button CssClass="submit-Btn" ID="cmdShop" runat="server" Text="Continue Shopping"></asp:Button>
        </div>
        <div class="float-right align-right w48">
            <asp:UpdatePanel ID="updatePanelButton" runat="server" ChildrenAsTriggers="True">
                <ContentTemplate>
                    <asp:Button CssClass="submit-Btn" ID="cmdUpdateCart" runat="server" Text="Update"></asp:Button>
                    <asp:Button CssClass="submit-Btn" ID="cmdSaveCart" runat="server" Text="Save Cart"></asp:Button>
                    <asp:Button CssClass="submit-Btn" ID="cmdCheckOut" runat="server" Text="Check Out"></asp:Button>
                </ContentTemplate>
            </asp:UpdatePanel>
            <cc1:User ID="User1" runat="server" />
        </div>
    </div>
</div>
