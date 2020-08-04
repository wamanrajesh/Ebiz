<%--Aptify e-Business 5.5.1, July 2013--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ViewListing.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.MarketPlace.ViewListing" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="WebUserActivity" %>
<%@ Register TagPrefix="uc1" TagName="ListingDisplay" Src="ListingDisplay.ascx" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<div runat="server" id="tblMain" class="row-div">
    <uc1:listingdisplay id="ListingDisplay" runat="server"></uc1:listingdisplay>
</div>
<cc1:webuseractivity webmodule="Marketplace" id="WebUserActivity1" runat="server" />
<cc3:user id="User1" runat="server" />
