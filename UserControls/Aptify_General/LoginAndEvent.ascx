<%--Aptify e-Business 5.5.1, July 2013--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="LoginAndEvent.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.LoginAndEvent" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register Src="~/UserControls/Aptify_Meetings/UpcomingEvents.ascx" TagName="UpcomingEvents"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/Aptify_General/LoginSF4.ascx" TagName="LoginSF4"
    TagPrefix="uc2" %>

<div class="row-div" id="trLogin" runat="server">
    <uc2:LoginSF4 ID="LoginSF4" runat="server" />
</div>
<div id="trEvents" runat="server">
    <uc1:UpcomingEvents ID="UpcomingEvents" runat="server" />
</div>
<cc1:User ID="User1" runat="server" />