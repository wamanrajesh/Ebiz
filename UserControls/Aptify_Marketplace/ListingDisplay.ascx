<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ListingDisplay.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.MarketPlace.ListingDisplay" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<cc3:user id="User1" runat="server" />
<div class="row-div clearfix">
    <div class="label-div w22">
        Company Name:
    </div>
    <div class="field-div1 w40">
        <asp:Label ID="lblCompanyName" runat="server"></asp:Label>
    </div>
    <div class="float-left">
        <asp:Button ID="btnEdit" runat="server" Text="Edit Listing" CssClass="submit-Btn">
        </asp:Button>
        <asp:Button ID="btnRequest" runat="server" Text="Request Information" CssClass="submit-Btn">
        </asp:Button>
    </div>
</div>
<div class="row-div clearfix">
    <div class="label-div w22">
        Company Contact:
    </div>
    <div class="field-div1 w76">
        <asp:Label ID="lblCompanyContact" runat="server"></asp:Label>
    </div>
</div>
<div class="row-div clearfix">
    <div class="label-div w22">
        Name:
    </div>
    <div class="field-div1 w76">
        <asp:Label ID="lblName" runat="server"></asp:Label></div>
</div>
<div class="row-div clearfix">
    <div class="label-div w22 ">
        Listing Type:</div>
    <div class="field-div1 w76">
        <asp:Label ID="lblListingType" runat="server"></asp:Label></div>
</div>
<div class="row-div clearfix">
    <div class="label-div w22 ">
        Category:
    </div>
    <div class="field-div1 w76">
        <asp:Label ID="lblCategory" runat="server"></asp:Label></div>
</div>
<div class="row-div clearfix">
    <div class="label-div w22">
        Offering Type:</div>
    <div class="field-div1 w76">
        <asp:Label ID="lblOfferingType" runat="server"></asp:Label></div>
</div>
<div class="row-div clearfix">
    <div class="label-div w22 ">
        Description:</div>
    <div class="field-div1 w76">
        <asp:Label ID="lblDescription" runat="server"></asp:Label></div>
</div>
<div class="row-div clearfix">
    <div class="label-div w22 ">
        Vendor Web Site:</div>
    <div class="field-div1 w76">
        <asp:HyperLink ID="lnkCompanyURL" runat="server"></asp:HyperLink></div>
</div>
<div class="row-div clearfix">
    <div class="label-div w22">
        Request Information Email:
    </div>
    <div class="field-div1 w76">
        <asp:HyperLink ID="lnkEmail" runat="server"></asp:HyperLink></div>
</div>

