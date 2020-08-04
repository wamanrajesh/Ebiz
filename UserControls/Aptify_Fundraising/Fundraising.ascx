<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Fundraising.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.Fundraising.Fundraising" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessShoppingCart" %>
<%@ Register TagPrefix="uc1" TagName="CreditCard" Src="~/UserControls/Aptify_General/CreditCard.ascx" %>
<div class="fundraising-main-div">
    <div id="tblMain" runat="server">
        <div class="row-div">
            <asp:Label ID="lblMsg" Visible="False" runat="server">
            </asp:Label>
        </div>
        <div id="tblInner" runat="server">
       <div class="row-div">
            Thank you for your interest in supporting our organization. Please select your contribution
            amount and the project you want to support.
        </div>
        <div class="row-div top-margin clearfix">
            <div class="label-div w19">
            <asp:Label ID="lblFund" runat="server">
					Project
                    </asp:Label>
            </div>
            <div class="field-div1 w79">
                <asp:DropDownList ID="cmbFunds" runat="server" DataValueField="ID"
                    DataTextField="WebName">
                </asp:DropDownList>
            </div>
        </div>
        <div class="row-div clearfix">
            <div class="label-div w19 label">
             <asp:Label ID="lblAmount" runat="server">
					Amount
              </asp:Label>
             <asp:Label ID="lblCurrencySymbol" runat="server">
                    </asp:Label>
            </div>
            <div class="field-div1 w79">
                <asp:TextBox ID="txtAmount" runat="server">
                </asp:TextBox>
                <asp:CompareValidator ID="CompareValidator1" runat="server" Display="Dynamic" Type="Double"
                    ValueToCompare="0.00" Operator="GreaterThan" ErrorMessage="Amount Must Be Greater Than 0"
                    ControlToValidate="txtAmount" CssClass="required-label">
                </asp:CompareValidator>
                <asp:RequiredFieldValidator ID="RequiredField" Display="Dynamic" runat="server" ControlToValidate="txtAmount"
                    ErrorMessage="Please Enter Amount" CssClass="required-label">
                </asp:RequiredFieldValidator>
            </div>
        </div>
         </div>
    </div>
<div id="paymentarea" runat="server" class="top-margin">
    <fieldset class="border-color-gray w48">
        <legend class="label">Payment Information</legend>
        <uc1:CreditCard ID="CreditCard" runat="server"></uc1:CreditCard>
        <div class="row-div clearfix">
          <div class="label-div w25">
            &nbsp;</div>
            <div class="field-div1 w74">
            <asp:Button ID="cmdSubmit" runat="server" Text="Submit Your Donation" CssClass="submit-Btn" /></div>
        </div>
    </fieldset>
</div>
</div>
<cc1:User ID="User1" runat="server" />

<cc3:AptifyShoppingCart ID="ShoppingCart1" runat="server" />
