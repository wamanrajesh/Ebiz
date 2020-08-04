<%--Aptify e-Business 5.5.1, July 2013--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="EditListing.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.MarketPlace.EditListing" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="WebUserActivity" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="PageSecurity" %>
<%@ Register TagPrefix="uc1" TagName="ListingDisplay" Src="ListingDisplay.ascx" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="uc2" TagName="TopicCodeViewer" Src="~/UserControls/Aptify_General/TopicCodeViewer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ListingEdit" Src="ListingEdit.ascx" %>
<div class="marketplace-main-div">
    <div id="tblMain" runat="server" class="row-div">
        <uc1:listingedit id="ListingEdit" runat="server"></uc1:listingedit>
    </div>
    <div class="row-div">
        <fieldset class="border-color-gray">
            <legend class="label">Select Topic Codes:</legend>
            <div>
                <uc2:topiccodeviewer id="TopicCodeViewer" runat="server"></uc2:topiccodeviewer>
                <asp:HyperLink runat="server" ID="lnkTopicCodes"></asp:HyperLink>
            </div>
        </fieldset>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w28">
            &nbsp;
        </div>
        <div class="float-left w70">
            <asp:Button ID="cmdSubmit" runat="server" Text="Submit Changes" CssClass="submit-Btn">
            </asp:Button>
        </div>
    </div>
    <cc3:user id="User1" runat="server" />
    <cc1:webuseractivity id="WebUserActivity1" runat="server" webmodule="MarketPlace" />
</div>
