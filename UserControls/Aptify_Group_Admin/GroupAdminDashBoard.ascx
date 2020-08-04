<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="GroupAdminDashBoard.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.Admin.GroupAdminDashBoard" %>
<%@ Register Src="ManageMyGroup.ascx" TagName="ManageMyGroup" TagPrefix="uc1" %>
<%@ Register Src="MembershipExpireStatus.ascx" TagName="MembershipExpireStatus" TagPrefix="uc2" %>
<%@ Register Src="UpcomingEventsRegistrationChart.ascx" TagName="UpcomingEventsRegistrationChart"
    TagPrefix="uc3" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register Src="EventCalendar.ascx" TagName="EventCalendar" TagPrefix="uc4" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<div class="agd-board">
    <div class="left-container w18">
        <uc1:managemygroup id="ManageMyGroup1" runat="server" />
    </div>
    <div class="middle-container w58">
        <div>
            <uc2:membershipexpirestatus id="MembershipExpireStatus1" runat="server" />
        </div>
        <div class="top-margin">
            <uc3:upcomingeventsregistrationchart id="UpcomingEventsRegistrationChart1" runat="server" />
        </div>
    </div>
    <div class="right-container w20">
        <uc4:eventcalendar id="EventCalendar1" runat="server" />
    </div>
    <div class="clear">
    </div>
</div>
<cc2:user id="User1" runat="server" />
