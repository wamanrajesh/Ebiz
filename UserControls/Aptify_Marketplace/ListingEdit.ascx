<%--Aptify e-Business 5.5.1, July 2013--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ListingEdit.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.MarketPlace.ListingEdit" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessShoppingCart" %>
<div id="tblListingInfo" class="marketplace-main-div">
    <div class="row-div clearfix">
        <div class="label-div w28">
            Company Name:</div>
        <div class="field-div1 w70">
            <asp:Label ID="lblCompanyName" runat="server"></asp:Label></div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w28 ">
            Company Contact:</div>
        <div class="field-div1 w70">
            <asp:Label ID="lblCompanyContact" runat="server"></asp:Label>
        </div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w28">
            <span class="required-label">*</span>Name:</div>
        <div class="field-div1 w70">
            <asp:TextBox ID="txtListingName" runat="server"></asp:TextBox>
            <span><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please specify the MarketPlace Listing Name."
                Display="Dynamic" ControlToValidate="txtListingName" CssClass="Error"></asp:RequiredFieldValidator></span>
        </div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w28 ">
            Listing Type:</div>
        <div class="field-div1 w70">
            <asp:DropDownList ID="cboListingType" runat="server" AutoPostBack="True">
            </asp:DropDownList>
        </div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w28 ">
            Category:
        </div>
        <div class="field-div1 w70">
            <asp:DropDownList ID="cboCategories" runat="server">
            </asp:DropDownList>
        </div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w28 ">
            Offering Type:</div>
        <div class="field-div1 w70">
            <asp:DropDownList ID="cboOfferingType" runat="server">
            </asp:DropDownList>
        </div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w28">
            <span class="required-label">*</span>Plain Text Description:</div>
        <div class="field-div1 w70">
            <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="txt-restrict-resize"></asp:TextBox></div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w28 ">
            Vendor Product Information URL:</div>
        <div class="field-div1 w70">
            <asp:TextBox ID="txtVendorURL" runat="server"></asp:TextBox></div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w28">
            Request Information Email:
        </div>
        <div class="field-div1 w70">
            <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox></div>
    </div>
</div>
<p>
    <cc3:user id="User1" runat="server" />
    <cc1:aptifyshoppingcart id="ShoppingCart1" runat="server" />
</p>
