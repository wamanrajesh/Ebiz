<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="MyMeetingSessions.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.Meetings.MyMeetingSessions" %>
<%@ Register Assembly="AptifyEBusinessUser" Namespace="Aptify.Framework.Web.eBusiness"
    TagPrefix="cc1" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<div class="row-div">
    <rad:radscheduler id="EventCalenderScheduler" runat="server" allowinsert="false"
        allowdelete="false" enableembeddedbasestylesheet="true" enabletheming="true"
        selectedview="TimelineView" overflowbehavior="Scroll" onnavigationcommand="EventCalenderScheduler_NavigationCommand"
        datakeyfield="ProductID" datasubjectfield="MeetingTitle" datastartfield="StartDate"
        dataendfield="EndDate" showfooter="false" localization-allday="All Day">
        <monthview adaptiverowheight="true" visibleappointmentsperday="10" />
        <appointmenttemplate>
                    <%# Eval("Subject") %>
                </appointmenttemplate>
        <exportsettings openinnewwindow="true" filename="MyMeetingSessions">
                    <Pdf PageTitle="My Meeting Sessions" Author="EBusiness" Creator="EBusiness" Title="My Meeting Sessions" PaperSize="A4" PageLeftMargin="0" PageRightMargin="0" PageWidth="11.69in" PageHeight="8.27in"></Pdf>
                </exportsettings>
    </rad:radscheduler>
</div>
<div class="row-div">
    <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="submit-Btn" />
    <asp:Button ID="btnEmail" runat="server" Text="Email" CssClass="submit-Btn" />
    <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="submit-Btn" />
</div>
<div class="row-div label">
    <asp:Label ID="lblMessage" runat="server"></asp:Label></div>
<asp:Literal ID="ltlPrint" runat="server" EnableViewState="false"></asp:Literal>
<cc1:user id="User1" runat="server">
</cc1:user>
