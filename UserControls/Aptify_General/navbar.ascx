<%--Aptify e-Business 5.5.1, July 2013--%>
<%@ Control Language="vb" AutoEventWireup="false" Inherits="Aptify.Framework.Web.eBusiness.NavBar"
    CodeFile="NavBar.ascx.vb" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%--Suraj S Issue#17638, 12/24/2013,Resove problem e-Business Web Menu Does Not Work on Tablets for this we remove the " OnItemClick="RadMenu1_click" event and use the EnableTextHTMLEncoding property  --%>
<rad:RadMenu ID="RadMenu1" runat="server" EnableRoundedCorners="true" EnableShadows="true" EnableTextHTMLEncoding="true">
</rad:RadMenu>


    <cc1:User ID="User1" runat="server" /><%--'Added By Sandeep for Issue 15051 on 13/03/2013--%>
<%--<div class="contentbread">
    <telerik:RadSiteMap ID="BreadCrumbSiteMap" runat="server" DataTextField="Text" DataNavigateUrlField="NavigateUrl"
        >
        <DefaultLevelSettings ListLayout-RepeatDirection="Horizontal" SeparatorText="/" Layout="Flow" />
    </telerik:RadSiteMap>
   
</div>--%>
