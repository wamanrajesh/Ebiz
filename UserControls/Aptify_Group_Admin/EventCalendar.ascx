<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="EventCalendar.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.Admin.EventCalendar" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<script language="javascript" type="text/javascript">
    function DateClick(sender, args) {
        if (args.get_renderDay().IsSelected)
            args.set_cancel(true);
    } 
</script>
<asp:UpdatePanel ID="update1" runat="server">
    <ContentTemplate>
       
            <div>
                <div align="right">
                    <telerik:RadCalendar ID="RadEventCalendar" runat="server" ViewSelectorText="x" AutoPostBack="True"
                        EnableMultiSelect="False" Width="100%">
                        <WeekendDayStyle CssClass="rcWeekend"></WeekendDayStyle>
                        <CalendarTableStyle CssClass="rcMainTable"></CalendarTableStyle>
                        <OtherMonthDayStyle CssClass="rcOtherMonth"></OtherMonthDayStyle>
                        <OutOfRangeDayStyle CssClass="rcOutOfRange"></OutOfRangeDayStyle>
                        <DisabledDayStyle CssClass="rcDisabled"></DisabledDayStyle>
                        <SelectedDayStyle CssClass="rcSelected"></SelectedDayStyle>
                        <DayOverStyle CssClass="rcHover"></DayOverStyle>
                        <FastNavigationStyle CssClass="RadCalendarMonthView RadCalendarMonthView_Sunset">
                        </FastNavigationStyle>
                        <ViewSelectorStyle CssClass="rcViewSel"></ViewSelectorStyle>
                        <ClientEvents OnDateClick="DateClick" />
                    </telerik:RadCalendar>
                </div>
                <div class="calendar-msg-txt">
                    <asp:Label ID="lblcaldateinfo" runat="server" Text="Click on dates in the Calendar and view registered events below:"></asp:Label>
                </div>
                <div class="calendar-msg-txt1">
                    <asp:Label ID="lblmsg" runat="server" ></asp:Label>
                </div>
                <div class="calendar-msg-txt2">
                    <asp:Label ID="lblSelectedDate" runat="server"></asp:Label>
                </div>
            </div>
            <div id="firsteventdetails" runat="server">
                <div id="first" class="top-margin">
                    <asp:Label ID="lblEventName" runat="server" Font-Size="9pt" Font-Bold="True" ForeColor="#906227"></asp:Label>
                </div>
                <div align="left" class="top-margin">
                    <asp:Label ID="lblRegisteredCount" runat="server" Font-Bold="True" Text="Registered:"
                        Font-Size="8pt" ForeColor="#333333"></asp:Label>
                </div>
                <div align="left" >
                    <asp:Label ID="lblRegisteredCt" runat="server" ForeColor="#2e9b09"></asp:Label>
                </div>
                <div align="left" class="top-margin">
                    <asp:Label ID="lbleventwaitlist" runat="server" Font-Bold="True" Text="Waitlist:"
                        Font-Size="8pt" ForeColor="#333333"></asp:Label>
                </div>
                <div align="left" >
                    <asp:Label ID="lblWaitlistCount" runat="server" ForeColor="#f72c04"></asp:Label>
                </div>
                <div align="left" class="top-margin">
                    <asp:ImageButton ID="imgbtnrefresh" runat="server" Font-Size="9pt" ImageUrl="~/Images/Refresh.png" />
                </div>
            </div>
            <div id="secondeventdetails" runat="server">
               
                <div colspan="3" class="top-margin">
                            <asp:Image ID="imgdivider" runat="server" ImageUrl="~/Images/Line_daashboard.png"
                                />
                        </div>
                   
                <div colspan="3" class="top-margin">
                            <asp:Label ID="lblMeetingTitle" runat="server" Font-Size="9pt" Font-Bold="True" ForeColor="#906227"></asp:Label>
                        </div>
                    
                <div align="left" class="top-margin">
                            <asp:Label ID="lblMeetingRegistered" runat="server" Font-Bold="True" Text="Registered:"
                                Font-Size="8pt" ForeColor="#333333"></asp:Label>
                        </div>
                
                <div align="left" >
                            <asp:Label ID="lblMRegisteredCt" runat="server" ForeColor="#2e9b09"></asp:Label>
                        </div>
                  
                <div align="left" class="top-margin">
                            <asp:Label ID="lblMeetingWaitlist" runat="server" Font-Bold="True" Text="Waitlist:"
                                Font-Size="8pt" ForeColor="#333333"></asp:Label>
                        </div>

                <div align="left" >
                            <asp:Label ID="lblMWaitlistCount" runat="server" ForeColor="#f72c04"></asp:Label>
                        </div>

                <div align="left" class="top-margin">
                            <asp:ImageButton ID="imgbtnRefresh2" runat="server" Font-Size="9pt" ImageUrl="~/Images/Refresh.png" />
                        </div>
               
            </div>
       
    </ContentTemplate>
</asp:UpdatePanel>
<cc1:User ID="user1" runat="server" />
