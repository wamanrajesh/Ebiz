<%--Aptify e-Business 5.5.1, July 2013--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="NewListing.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.MarketPlace.NewListing" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="WebUserActivity" %>
<%@ Register TagPrefix="uc1" TagName="ListingEdit" Src="ListingEdit.ascx" %>
<%@ Register TagPrefix="uc1" TagName="TopicCodeViewer" Src="~/UserControls/Aptify_General/TopicCodeViewer.ascx" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="cc5" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessShoppingCart" %>
<%@ Register TagPrefix="uc2" TagName="CreditCard" Src="../Aptify_General/CreditCard.ascx" %>
<div>
    <div id="tblMain" runat="server">
        <uc1:listingedit id="ListingEdit" runat="server"></uc1:listingedit>
    </div>
    <div class="row-div clearfix">
        <div class="float-left w49">
            <div class="row-div">
                <fieldset id="fieldsetListing" runat="server" class="border-color-gray">
                    <legend class="label">Select Topic Codes:</legend>
                    <div>
                        <uc1:topiccodeviewer id="TopicCodeViewer" runat="server"></uc1:topiccodeviewer>
                    </div>
                </fieldset>
            </div>
        </div>
        <div class="float-right w49">
            <div class="row-div">
                <fieldset class="border-color-gray">
                    <legend class="label">Enter Payment Information:</legend>
                    <div>
                        <uc2:creditcard id="CreditCard" runat="server"></uc2:creditcard>
                    </div>
                </fieldset>
            </div>
        </div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w28">
            &nbsp;
        </div>
        <div class="float-left w70">
            <asp:Button ID="cmdSubmit" runat="server" Text="Submit New Listing" CssClass="submit-Btn">
            </asp:Button></div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w28">
            &nbsp;
        </div>
        <div class="float-left errormsg-div w70">
            <asp:Label ID="lblSubmitError" runat="server"></asp:Label></div>
    </div>
    <cc1:webuseractivity id="WebUserActivity1" runat="server" webmodule="MarketPlace" />
    <cc3:user id="User1" runat="server" />
    <cc5:aptifyshoppingcart id="ShoppingCart1" runat="server" />
</div>
