<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Calendar.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.Meetings.Calendar" %>
<%@ Register Assembly="AptifyEBusinessUser" Namespace="Aptify.Framework.Web.eBusiness"
    TagPrefix="cc1" %>
<div class="table-div">
<div class="row-div clearfix">
    <div class="float-left ">
        <asp:HyperLink ID="MeetingGridPage" runat="server" Text="Grid View" />
    </div>
    <div class="align-right">
        <asp:Label ID="Label1" runat="server" CssClass="label" Text="Select Month/Year:"></asp:Label>&nbsp;
        <asp:DropDownList ID="cmbMonth" runat="server">
            <asp:ListItem Value="1">January</asp:ListItem>
            <asp:ListItem Value="2">February</asp:ListItem>
            <asp:ListItem Value="3">March</asp:ListItem>
            <asp:ListItem Value="4">April</asp:ListItem>
            <asp:ListItem Value="5">May</asp:ListItem>
            <asp:ListItem Value="6">June</asp:ListItem>
            <asp:ListItem Value="7">July</asp:ListItem>
            <asp:ListItem Value="8">August</asp:ListItem>
            <asp:ListItem Value="9">September</asp:ListItem>
            <asp:ListItem Value="10">October</asp:ListItem>
            <asp:ListItem Value="11">November</asp:ListItem>
            <asp:ListItem Value="12">December</asp:ListItem>
        </asp:DropDownList>
        &nbsp;<asp:DropDownList ID="cmbYear" runat="server">
            <asp:ListItem>2003</asp:ListItem>
            <asp:ListItem>2004</asp:ListItem>
            <asp:ListItem>2005</asp:ListItem>
            <asp:ListItem>2006</asp:ListItem>
            <asp:ListItem>2007</asp:ListItem>
            <asp:ListItem>2008</asp:ListItem>
            <asp:ListItem>2009</asp:ListItem>
            <asp:ListItem>2010</asp:ListItem>
            <asp:ListItem>2011</asp:ListItem>
            <asp:ListItem>2012</asp:ListItem>
            <asp:ListItem>2013</asp:ListItem>
            <asp:ListItem>2014</asp:ListItem>
            <asp:ListItem>2015</asp:ListItem>
            <asp:ListItem>2016</asp:ListItem>
            <asp:ListItem>2017</asp:ListItem>
        </asp:DropDownList>
       <asp:Button ID="btnUpdate" runat="server" Text="Go" CssClass="submit-Btn" />
    </div>
  </div>
    <div class="calendar-div">
        <asp:Calendar ID="Calendar1" runat="server" NextPrevFormat="FullMonth" 
            NextMonthText="" PrevMonthText="" SelectMonthText="" SelectWeekText="" CellSpacing="1"
            CssClass="calendar-menu">
            <SelectedDayStyle CssClass="calendar-selected-day" />
            <WeekendDayStyle CssClass="calendar-day calendar-align" />
            <TodayDayStyle CssClass="calendar-today-day" />
            <OtherMonthDayStyle CssClass="calendar-other-month-day calendar-align" />
            <DayStyle CssClass="calendar-day calendar-align" />
            <NextPrevStyle Font-Strikeout="False" Wrap="True" CssClass="calendar-next-prv " />
            <DayHeaderStyle CssClass="label" />
            <TitleStyle CssClass="calendar-title" />
        </asp:Calendar>
    </div>
    <div class="row-div top-margin">
    <%--Kinjal: Below code is not in use --%>
       <%-- <asp:Label ID="Label5" runat="server" Text="Category: " Visible="False"></asp:Label>&nbsp;<asp:DropDownList
            ID="cmbBottomCategory" runat="server" Width="129px" Visible="False">
            <asp:ListItem>All</asp:ListItem>
            <asp:ListItem>Internal</asp:ListItem>
            <asp:ListItem>External</asp:ListItem>
        </asp:DropDownList>
        <noscript>
        </noscript>--%>
        <div class="align-right">
            <asp:Label ID="Label4" runat="server" CssClass="label" Text="Select Month/Year: "></asp:Label>&nbsp;
            <asp:DropDownList ID="cmbBottomMonth" runat="server">
                <asp:ListItem Value="1">January</asp:ListItem>
                <asp:ListItem Value="2">February</asp:ListItem>
                <asp:ListItem Value="3">March</asp:ListItem>
                <asp:ListItem Value="4">April</asp:ListItem>
                <asp:ListItem Value="5">May</asp:ListItem>
                <asp:ListItem Value="6">June</asp:ListItem>
                <asp:ListItem Value="7">July</asp:ListItem>
                <asp:ListItem Value="8">August</asp:ListItem>
                <asp:ListItem Value="9">September</asp:ListItem>
                <asp:ListItem Value="10">October</asp:ListItem>
                <asp:ListItem Value="11">November</asp:ListItem>
                <asp:ListItem Value="12">December</asp:ListItem>
            </asp:DropDownList>
            &nbsp;<asp:DropDownList ID="cmbBottomYear" runat="server">
                <asp:ListItem>2003</asp:ListItem>
                <asp:ListItem>2004</asp:ListItem>
                <asp:ListItem>2005</asp:ListItem>
                <asp:ListItem>2006</asp:ListItem>
                <asp:ListItem>2007</asp:ListItem>
                <asp:ListItem>2008</asp:ListItem>
                <asp:ListItem>2009</asp:ListItem>
                <asp:ListItem>2010</asp:ListItem>
                <asp:ListItem>2011</asp:ListItem>
                <asp:ListItem>2012</asp:ListItem>
                <asp:ListItem>2013</asp:ListItem>
                <asp:ListItem>2014</asp:ListItem>
                <asp:ListItem>2015</asp:ListItem>
                <asp:ListItem>2016</asp:ListItem>
                <asp:ListItem>2017</asp:ListItem>
            </asp:DropDownList>
            <asp:Button ID="cmbBottomGo" runat="server" Text="Go" CssClass="submit-Btn" />&nbsp;
        </div>
    </div>
</div>
<cc1:user id="User1" runat="server"></cc1:user>
