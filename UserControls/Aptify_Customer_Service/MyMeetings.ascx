<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="MyMeetings.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.CustomerService.MyMeetings" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<script type="text/javascript">
    function HideControl() {
        var Open = document.getElementById("<%= Open.ClientID %>");
        var HiddenOpen = document.getElementById("<%= HiddenOpen.ClientID %>")
        var hdnShowNotification = document.getElementById("<%= hdnShowNotification.ClientID %>");
        hdnShowNotification.value = "HideControl";

        if (HiddenOpen.value == "None") {
            Open.style.display = "inherit";
            HiddenOpen.value = "inherit";
        }
        else {

            Open.style.display = "None";
            HiddenOpen.value = "None";
        }

    }

    function HideNotification() {
        var hdnShowNotification = document.getElementById("<%= hdnShowNotification.ClientID %>");
        if (hdnShowNotification.value != "HideControl" && hdnShowNotification.value != "ShowNotification") {
            var Open = document.getElementById("<%= Open.ClientID %>");
            var HiddenOpen = document.getElementById("<%= HiddenOpen.ClientID %>")
            Open.style.display = "None";
            HiddenOpen.value = "None";
        }

        hdnShowNotification.value = "";
    }

    function ShowNotification() {
        var hdnShowNotification = document.getElementById("<%= hdnShowNotification.ClientID %>");
        if (hdnShowNotification.value != "CloseNotification") {
            var Open = document.getElementById("<%= Open.ClientID %>");
            var HiddenOpen = document.getElementById("<%= HiddenOpen.ClientID %>")
            Open.style.display = "inherit";
            HiddenOpen.value = "inherit";

            hdnShowNotification.value = "ShowNotification";
        }
    }

    window.onload = function () {
        document.body.onclick = HideNotification;
    }

    function CloseNotification() {
        var hdnShowNotification = document.getElementById("<%= hdnShowNotification.ClientID %>");
        var Open = document.getElementById("<%= Open.ClientID %>");
        var HiddenOpen = document.getElementById("<%= HiddenOpen.ClientID %>")
        Open.style.display = "None";
        HiddenOpen.value = "None";
        hdnShowNotification.value = "CloseNotification";
    }

    window.onload = function () {
        document.body.onclick = HideNotification;
    }



</script>
<div class="notification-container clearfix ">
    <ul>
        <li id="Li1"><a>Meetings Notification </a>
            <span class="notification-bubble" onclick="HideControl();" title="Notifications">
                <asp:Label ID="lblTotalNotificationCount" runat="server" Text="0"></asp:Label>
            </span>
        <div id="Open" runat="server" class="notification-list-wrapper notification-div-position"
        style="display: none" onclick="ShowNotification();">
            <div class="notification-list-div">
                <div class="close-notification">
                     <img id="ImgClose" runat="server" onclick="CloseNotification();" />
                </div>
                <div class="notification-div clearfix">
                    <asp:HiddenField ID="hdnShowNotification" runat="server" Value="HideControl" />
                    <asp:Label runat="server" ID="lblNotificationDivHeading" CssClass="notification-label"
                        Text="Your Registered Event"></asp:Label>
                
            
                    <asp:HiddenField ID="HiddenOpen" runat="server" Value="None" />
                    <asp:BulletedList ID="blListUpcomingMeetings" BulletStyle="NotSet" runat="server"
                        CssClass="notification-list-menu">
                    </asp:BulletedList>
                </div>
                <div class="notification-div clearfix">
                    <asp:Label runat="server" ID="lblNotificationForYou" CssClass="notification-label"
                        Text="Upcoming Event"></asp:Label>
            
                    <asp:BulletedList ID="blListMeetingsforyou" BulletStyle="NotSet" runat="server" CssClass="notification-list-menu">
                    </asp:BulletedList>
                </div>
            </div>
        </div>
        
        
        </li>
    </ul>
</div>

<div>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel3"
        DisplayAfter="0">
        <ProgressTemplate>
            <div class="processing-div">
                <div class="processing">
                Please wait...
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</div>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="True">
    <ContentTemplate>
        <div id="tblMain" runat="server" class="mymeeting-main-div">
            <div class="row-div label">
                
                    <asp:Label ID="lblSearchInfo" runat="server">
                    </asp:Label>
                
            </div>
            <div class="row-div top-margin clearfix">
                <div class="label-div">
                    Select Meeting:
                </div>
                <div class="field-div1">
                    <asp:DropDownList runat="server" ID="cmbCategory" AutoPostBack="true" CssClass="DDLTelMyMeeting">
                        <asp:ListItem Text="All">
                        </asp:ListItem>
                        <asp:ListItem Text="Upcoming">
                        </asp:ListItem>
                        <asp:ListItem Text="Past">
                        </asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="label-div1">
                    Select Status:
                </div>
                <div class="field-div2">
                    <asp:DropDownList runat="server" ID="cmbStatus" CssClass="DDLTelMyMeeting">
                    </asp:DropDownList>
                </div>
                <div class="label-div1">
                    Start Date:</div>
                <div class="field-div2 w15">
                    <telerik:raddatepicker id="txtStartDate" runat="server">
				<DatePopupButton ToolTip="" />
			</telerik:raddatepicker>
                    <telerik:radtooltip id="RadToolTipStartDate" runat="server" isclientid="true" text="Find Meetings that start on or after the specified date and time."
                        autoclosedelay="20000" />
                </div>
                <div class="label-div1">
                    End Date:
                </div>
                <div class="field-div2 w15">
                    <telerik:raddatepicker id="txtEndDate" runat="server" title="title" tooltip="Find Meetings that End on or before the specified date."
                         CssClass="datePicker">
			</telerik:raddatepicker>
                    <telerik:radtooltip id="RadToolTipEndtDate" runat="server" isclientid="true" text="Find Meetings that End on or before the specified date and time."
                        autoclosedelay="20000" />
                </div>
                <div class="label-div1">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="True">
                        <ContentTemplate>
                            <asp:Button runat="server" ID="btnSearch" CssClass="submit-Btn" Text="Search" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSearch" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="row-div top-margin ">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server" ChildrenAsTriggers="True">
                    <ContentTemplate>
                        <rad:radgrid id="grdMeetings" runat="server" autogeneratecolumns="False" allowfilteringbycolumn="False"
                            allowpaging="true" sortingsettings-sorteddesctooltip="Sorted Descending" sortingsettings-sortedasctooltip="Sorted Ascending"
                            pagesize="5" pagerstyle-pagesizelabeltext="Records Per Page" allowsorting="True">
						<GroupingSettings CaseSensitive="false" />
						<SortingSettings SortedAscToolTip="Sorted Ascending" SortedDescToolTip="Sorted Descending" />
						<MasterTableView DataKeyNames="MeetingID" AllowNaturalSort="false" AllowSorting="True" AllowPaging="True">
							<DetailTables>
								<telerik:GridTableView DataKeyNames="MeetingID" Width="100%" runat="server" AllowFilteringByColumn="false" AllowNaturalSort="false" SortingSettings-SortedAscToolTip="Sorted Ascending" SortingSettings-SortedDescToolTip="Sorted Descending" NoDetailRecordsText="Nothing to display">
									<Columns>
										<rad:GridTemplateColumn HeaderText="Name" DataField="WebName" FilterControlWidth="200px" SortExpression="WebName" HeaderStyle-Width="250px" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
											<ItemTemplate>
												<asp:HyperLink ID="lnkWebName" runat="server" Text='
												<%# DataBinder.Eval(Container.DataItem,"WebName") %>
													' NavigateUrl='
													<%# DataBinder.Eval(Container.DataItem,"MeetingUrl") %>
														'>
														</asp:HyperLink>
													</ItemTemplate>
												</rad:GridTemplateColumn>
												<rad:GridTemplateColumn HeaderText="Location" DataField="Location" FilterControlWidth="150px" SortExpression="Location" HeaderStyle-Width="150px" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
													<ItemTemplate>
														<asp:Label ID="lblLocation" runat="server" Text='
														<%# DataBinder.Eval(Container.DataItem,"Location") %>
															'>
															</asp:Label>
														</ItemTemplate>
														<ItemStyle VerticalAlign="top" />
													</rad:GridTemplateColumn>
													<rad:GridDateTimeColumn DataField="StartDate" UniqueName="GridDateTimeColumnStartDate" HeaderText="Start Date" FilterControlWidth="150px" SortExpression="StartDate" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" DataType="System.DateTime" ShowFilterIcon="false" />
													<rad:GridDateTimeColumn DataField="EndDate" UniqueName="GridDateTimeColumnEndDate" HeaderText="End Date" FilterControlWidth="150px" SortExpression="EndDate" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" DataType="System.DateTime" ShowFilterIcon="false" EnableTimeIndependentFiltering="true" />
													<rad:GridTemplateColumn HeaderText="Status" DataField="Status" AllowFiltering="true" FilterControlWidth="100px" HeaderStyle-Width="100px" SortExpression="Status" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
														<ItemTemplate>
															<asp:Label ID="lblStatus" runat="server" Text='
															<%# DataBinder.Eval(Container.DataItem,"Status") %>
																'>
																</asp:Label>
															</ItemTemplate>
															<ItemStyle VerticalAlign="top" />
														</rad:GridTemplateColumn>
													</Columns>
												</telerik:GridTableView>
											</DetailTables>
											<Columns>
												<rad:GridTemplateColumn HeaderText="Name" DataField="WebName" FilterControlWidth="200px" SortExpression="WebName" HeaderStyle-Width="250px" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
													<ItemTemplate>
														<asp:HyperLink ID="lnkWebName" runat="server" Text='
														<%# DataBinder.Eval(Container.DataItem,"WebName") %>
															' NavigateUrl='
															<%# DataBinder.Eval(Container.DataItem,"MeetingUrl") %>
																'>
																</asp:HyperLink>
															</ItemTemplate>
														</rad:GridTemplateColumn>
														<rad:GridTemplateColumn HeaderText="Location" DataField="Location" FilterControlWidth="150px" SortExpression="Location" HeaderStyle-Width="150px" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
															<ItemTemplate>
																<asp:Label ID="lblLocation" runat="server" Text='
																<%# DataBinder.Eval(Container.DataItem,"Location") %>
																	'>
																	</asp:Label>
																</ItemTemplate>
																<ItemStyle VerticalAlign="top" />
															</rad:GridTemplateColumn>
															<rad:GridDateTimeColumn DataField="StartDate" UniqueName="GridDateTimeColumnStartDate" HeaderText="Start Date" FilterControlWidth="150px" SortExpression="StartDate" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" DataType="System.DateTime" ShowFilterIcon="false" />
															<rad:GridDateTimeColumn DataField="EndDate" UniqueName="GridDateTimeColumnEndDate" HeaderText="End Date" FilterControlWidth="150px" SortExpression="EndDate" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" DataType="System.DateTime" ShowFilterIcon="false" EnableTimeIndependentFiltering="true" />
															<rad:GridTemplateColumn HeaderText="Status" DataField="Status" AllowFiltering="true" FilterControlWidth="100px" HeaderStyle-Width="100px" SortExpression="Status" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
																<ItemTemplate>
																	<asp:Label ID="lblStatus" runat="server" Text='
																	<%# DataBinder.Eval(Container.DataItem,"Status") %>
																		'>
																		</asp:Label>
																	</ItemTemplate>
																	<ItemStyle VerticalAlign="top" />
																</rad:GridTemplateColumn>
																<rad:GridBoundColumn DataField="OrderID" HeaderText="Comment Text" Visible="false">
																</rad:GridBoundColumn>
															</Columns>
														</MasterTableView>
													</rad:radgrid>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="cmbCategory" />
    </Triggers>
</asp:UpdatePanel>


<cc1:user id="User1" runat="server" />
