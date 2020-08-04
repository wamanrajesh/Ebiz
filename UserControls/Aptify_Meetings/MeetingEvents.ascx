<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="MeetingEvents.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.Meetings.MeetingEvents" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessShoppingCart" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<cc1:User ID="User1" runat="server" Visible="False" />
<cc2:AptifyShoppingCart ID="ShoppingCart1" runat="server" Visible="False" />
<div style="padding: 15px; font-family: Segoe UI, Arial, Helvetica;">
    <div id="EventsAndMeetings">
        <div id="EventsAndMeetingsLeftContainer">
            <div id="TitleDiv" class="MeetingTitle" style="margin-bottom: 0.4em;">
                <asp:Label ID="lblTitle" runat="server"></asp:Label>
            </div>
            <div id="DetailDiv" style="font-size: small; float: left; width: 99%;">
                <div id="tblDetail" runat="server" style="font-size: 12px">
                    <div runat="server" visible="false" id="trSessionParent">
                        <div style="font-weight: bold">
                            Part of:
                        </div>
                        <div>
                            <asp:HyperLink runat="server" ID="lnkParent">
                                <asp:Label runat="server" ID="lblParent" />
                            </asp:HyperLink>
                        </div>
                    </div>
                    <div style="font-weight: bold; width: 20%;">
                        Date:
                    </div>
                    <div style="width: 75%;">
                        <asp:Label ID="lblDate" runat="Venue:"></asp:Label>
                        <asp:Label ID="lblVenue" runat="server"></asp:Label>&nbsp;&nbsp;<asp:Image runat="server"
                            ID="VenueDirection" ImageUrl="~/Images/get-diRegistration Fee" />: </td><td>
                                <asp:Label ID="lblTotalPrice" runat="server"></asp:Label>
                    </div>
                </div>
            </div>
            <div id="DescriptionDiv" style="font-size: 11px; font-weight: normal; float: left;
                width: 100%; margin-top: 1.5em; margin-bottom: 1.5em; text-align: justify;">
                <asp:Label ID="lblDescription" runat="server"></asp:Label>
            </div>
            <br />
            <div id="SpeakerDetails">
                <div style="color: #7a582d; font-size: 16px; margin-bottom: 0.2em;">
                    <asp:Image runat="server" ID="Image1" ImageUrl="~/Images/speaker-icon.png" CssClass="MiddleImage" />
                    Speaker Details
                </div>
                <div id="SpeakerGrid">
                    <asp:Label runat="server" ID="lblSpeakers" Visible="false"></asp:Label>
                    <rad:RadGrid ID="grdSpeakers" runat="server" AutoGenerateColumns="False" Width="100%"
                        SortingSettings-SortedDescToolTip="Sorted Descending" SortingSettings-SortedAscToolTip="Sorted Ascending"
                        GridLines="Horizontal" AllowPaging="true" BorderColor="#bdab8c" BorderStyle="Solid"
                        BorderWidth="1px">
                        <GroupingSettings CaseSensitive="false" />
                        <MasterTableView AllowSorting="true" AllowNaturalSort="false">
                            <Columns>
                                <rad:GridBoundColumn DataField="FirstName" HeaderText=" First Name" HeaderStyle-CssClass="leftAlign"
                                    SortExpression="FirstName">
                                    <ItemStyle Font-Size="12px" CssClass="leftAlign" />
                                </rad:GridBoundColumn>
                                <rad:GridBoundColumn DataField="LastName" HeaderText="Last Name" SortExpression="LastName">
                                    <ItemStyle Font-Size="12px" />
                                </rad:GridBoundColumn>
                                <rad:GridBoundColumn DataField="Title" HeaderText="Title" SortExpression="Title">
                                    <ItemStyle Font-Size="12px" />
                                </rad:GridBoundColumn>
                                <rad:GridBoundColumn DataField="Type" HeaderText="Type" SortExpression="Type">
                                    <ItemStyle Font-Size="12px" />
                                </rad:GridBoundColumn>
                                <rad:GridDateTimeColumn UniqueName="GridDateTimeColumnStartDate" AllowSorting="true"
                                    FilterControlWidth="260px" Visible="True" HeaderText="Start Date & Time" DataField="StartDate"
                                    SortExpression="StartDate" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                                    ReadOnly="true" ShowFilterIcon="false" DataType="System.DateTime" EnableTimeIndependentFiltering="true">
                                    <ItemStyle Width="260px" />
                                </rad:GridDateTimeColumn>
                                <rad:GridDateTimeColumn UniqueName="GridDateTimeColumnEndDate" AllowSorting="true"
                                    FilterControlWidth="260px" Visible="True" HeaderText="End Date & Time" DataField="EndDate"
                                    SortExpression="EndDate" ReadOnly="true" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                                    ShowFilterIcon="false" DataType="System.DateTime" EnableTimeIndependentFiltering="true">
                                    <ItemStyle Width="260px" />
                                </rad:GridDateTimeColumn>
                            </Columns>
                            <HeaderStyle CssClass="GridViewHeader" Height="28px" />
                            <ItemStyle CssClass="GridItemStyle" />
                            <PagerStyle Mode="NumericPages" Position="Bottom" HorizontalAlign="Right" Height="28px" />
                        </MasterTableView>
                    </rad:RadGrid>
                </div>
            </div>
            <br />
            <div id="ScheduleDetails">
                <div style="color: #7a582d; font-size: 16px; margin-bottom: 0.2em;">
                    <asp:Image runat="server" ID="Image2" ImageUrl="~/Images/Aptify_UC_Img/schedule-icon.png"
                        CssClass="MiddleImage" />
                    Schedule Details
                </div>
                <div id="ScheduleGrid">
                    <asp:Label ID="lblRelatedMeetingSessions" runat="server" Visible="false"></asp:Label>
                    <rad:RadGrid AllowPaging="true" ID="grdRelatedMeetingSessions" runat="server" PageSize="5"
                        AutoGenerateColumns="False" Width="100%" GridLines="Horizontal" BorderColor="#bdab8c"
                        BorderStyle="Solid" BorderWidth="1px">
                        <GroupingSettings CaseSensitive="false" />
                        <MasterTableView AllowSorting="true">
                            <Columns>
                                <rad:GridTemplateColumn>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkHeaderSession" runat="server" Checked="true" CssClass="leftAlign" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSession" runat="server" Checked="true" CssClass="leftAlign" />
                                    </ItemTemplate>
                                </rad:GridTemplateColumn>
                                <rad:GridHyperLinkColumn DataTextField="Name" HeaderText="Title" DataNavigateUrlFields="ProductID"
                                    ItemStyle-ForeColor="#009DD5">
                                    <ItemStyle Font-Size="12px" ForeColor="#009DD5" />
                                </rad:GridHyperLinkColumn>
                                <rad:GridDateTimeColumn UniqueName="GridDateTimeColumnStartDate" AllowSorting="true"
                                    FilterControlWidth="150px" Visible="True" HeaderText="Start Date & Time" DataField="StartDate"
                                    SortExpression="StartDate" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                                    ReadOnly="true" ShowFilterIcon="false" DataType="System.DateTime" EnableTimeIndependentFiltering="true">
                                    <ItemStyle Font-Size="12px" />
                                </rad:GridDateTimeColumn>
                                <rad:GridDateTimeColumn UniqueName="GridDateTimeColumnEndDate" AllowSorting="true"
                                    FilterControlWidth="150px" Visible="True" HeaderText="End Date & Time" DataField="EndDate"
                                    SortExpression="EndDate" ReadOnly="true" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                                    ShowFilterIcon="false" DataType="System.DateTime" EnableTimeIndependentFiltering="true">
                                    <ItemStyle Font-Size="12px" />
                                </rad:GridDateTimeColumn>
                                <rad:GridBoundColumn DataField="Place" HeaderText="Place">
                                    <ItemStyle Font-Size="12px" />
                                </rad:GridBoundColumn>
                                <rad:GridBoundColumn DataField="Price" HeaderText="Registration Price" HeaderStyle-HorizontalAlign="Right"
                                    HeaderStyle-CssClass="rightAlign">
                                    <ItemStyle Font-Size="12px" HorizontalAlign="Right" CssClass="rightAlign" />
                                </rad:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                    </rad:RadGrid>
                    <div id="EventRegistration" style="margin-top: 0.4em;">
                        <asp:Button ID="btnRegister" runat="server" Text="Register" CssClass="RegisterButton" />
                        <asp:Button ID="btnBack" runat="server" Text="&lt;&nbsp;Back" CssClass="BackButton" />
                    </div>
                </div>
                <div id="EventsAndMeetingSpacer">
                    &nbsp;
                </div>
                <div id="EventsAndMeetingsRightContainer">
                    <div runat="server" id="pnlPeopleYouMayKnow">
                        <div class="RightPaneHeader" style="font-weight: bold; font-size: 12px; height: 28px;
                            padding-left: 4px;">
                            <asp:Image runat="server" ID="img2" ImageUrl="~/Images/people-icon.png" CssClass="MiddleImage" />
                            <asp:Label runat="server" ID="Label1" Text=" People at Meeting" />
                        </div>
                        <div class="RightPaneScrollBorder">
                            <asp:Panel ID="pnl" runat="server">
                                <asp:Label runat="server" ID="lblPeopleYouMayKnow" Visible="False"></asp:Label>
                                <asp:Repeater ID="repPeopleYouMayKnow" runat="server">
                                    <ItemTemplate>
                                        <div style="width: 85%; margin: 0.2em 0.1em 0.4em 0.2em;">
                                            <asp:Image ID="imgProfile" runat="server" CssClass="PeopleImage imgmember" />
                                            <asp:HyperLink ID="lnkName" runat="server" CssClass="PeopleLink"></asp:HyperLink><asp:HyperLink
                                                ID="RelatedPersonCompanyName" runat="server" Visible="false"></asp:HyperLink><asp:CheckBox
                                                    ID="chkPersonDirExclude" runat="server" Visible="false" />
                                            <asp:CheckBox ID="chkCompanyDirExclude" runat="server" Visible="false" />
                                        </div>
                                        <hr class="PeopleSeparator" />
                                    </ItemTemplate>
                                </asp:Repeater>
                            </asp:Panel>
                        </div>
                    </div>
                    <div id="TravelDiscount" style="margin-top: 1em;">
                        <div class="RightPaneHeader" style="font-weight: bold; font-size: 12px; height: 28px;
                            padding-left: 4px;">
                            <asp:Image runat="server" ID="Image3" ImageUrl="~/Images/travel-icon.png" CssClass="MiddleImage" />
                            <asp:Label runat="server" ID="lblTravelDiscounts" Text="Travel Discounts" />
                        </div>
                        <div class="RightPaneBorder">
                            <asp:Label runat="server" ID="lblTravel" Visible="false"></asp:Label><asp:Repeater
                                ID="repTravelDiscounts" runat="server">
                                <ItemTemplate>
                                    <div width="98%" style="font-size: 11px; margin: 4px;">
                                        <div style="font-weight: bold;">
                                            Hotel:
                                        </div>
                                        <div style="padding-right: 0.5em; text-align: right;">
                                            <asp:Image runat="server" ID="getDirection" ImageUrl="~/Images/get-dir.png" ToolTip="Get direction" />
                                        </div>
                                        <div>
                                            <asp:Label ID="lblHotelName" runat="server"></asp:Label>
                                        </div>
                                        <div>
                                            <asp:Label ID="lblGroupOffer" runat="server"></asp:Label>
                                        </div>
                                        <div style="font-weight: bold;">
                                            From:
                                        </div>
                                        <div>
                                            <asp:Label ID="lblStartDate" runat="server"></asp:Label>To:
                                            <asp:Label ID="lblEndDate" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <hr class="PeopleSeparator" />
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
