<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Meeting.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.Meetings.MeetingControl" %>
<%@ Register Src="../Aptify_Forums/SingleForum.ascx" TagName="SingleForum" TagPrefix="uc1" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessShoppingCart" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register Src="MeetingActionControl.ascx" TagName="MeetingActionControl" TagPrefix="uc1" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<script language="javascript" type="text/javascript">

    function openNewWin(url) {

        var x = window.open(url);

        x.focus();

    }
</script>
<div class="meeting-div clearfix">
    <div class="left-container w73">
        <asp:Label ID="lblRegistrationResult" runat="server"></asp:Label>
        <div id="MeetingTitle" class="meeting-title-div">
            <img alt="Web Image" src="" runat="server" id="imgWebImage" visible="false" />
            <asp:Label runat="server" ID="lblName" />
            <rad:radrating id="RadRatingTotal" runat="server" skin="Default" enabled="false"
                precision="Exact">
                    </rad:radrating>
            <asp:Label ID="totalrating" runat="server"></asp:Label>
        </div>
        <div id="MeetingDetail" class="meeting-detail-div">
            <div runat="server" visible="false" id="trSessionParent" class="row-div1 clearfix">
                <div class="label-div w15">
                    Part of:
                </div>
                <div class="w84">
                    <asp:HyperLink runat="server" ID="lnkParent">
                        <asp:Label runat="server" ID="lblParent" />
                    </asp:HyperLink>
                </div>
            </div>
            <div class="row-div1 clearfix">
                <div class="label-div w15">
                    Date:
                </div>
                <div class="w84">
                    <asp:Label ID="lblDates" runat="server" />
                </div>
            </div>
            <div class="row-div1 clearfix">
                <div class="label-div w15">
                    Venue:
                </div>
                <div class="w84">
                    <asp:Label runat="server" ID="lblPlace" />
                    <asp:Label runat="server" ID="lblLocation" />
                    &nbsp; <span>
                        <asp:Image runat="server" ID="VenueDirection" ImageUrl="~/images/get-dir.png" CssClass="middle-img" />
                        &nbsp;
                        <asp:HyperLink ID="linkVenueDirection" runat="server" Target="_blank">
					Get Directions
                        </asp:HyperLink>
                    </span>
                </div>
            </div>
            <div class="row-div1 clearfix">
                <div class="label-div w15">
                    Total Price:
                </div>
                <div class="w84">
                    <asp:Label ID="lblTotalPrice" runat="server"></asp:Label>
                    <asp:Label ID="lblMemSavings" runat="server"></asp:Label>
                </div>
                <div id="trMetingRegStatus" runat="server" visible="false" class="row-div1 clearfix">
                    <div class="label-div w15">
                        Status:
                    </div>
                    <div class="w84">
                        <asp:Label ID="lblStatus" runat="server"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="row-div label">
                <asp:Label ID="lblMeetingStatus" runat="server"></asp:Label>
            </div>
            <asp:LinkButton ID="lnkNewMeeting" runat="server">
            </asp:LinkButton>
            <div class="meeting-web-description-div">
                <asp:Label runat="server" ID="lblWebDescription" />
            </div>
            <div>
                <span id="SpanRate" runat="server" class="meeting-table-header-font">Your Rating</span>
                <rad:radrating id="RadmeetingRate" runat="server" skin="Default" autopostback="true"
                    precision="Half">
                    </rad:radrating>
            </div>
            <div id="SpanShare" class="meeting-sociallink">
                Share with your social networks (limit 140 characters)
                <rad:radsocialshare id="RadSocialShareMeetings" runat="server" skin="Default">
                    <MainButtons>
                        <rad:RadSocialButton SocialNetType="ShareOnFacebook"></rad:RadSocialButton>
                        <rad:RadSocialButton SocialNetType="ShareOnTwitter"></rad:RadSocialButton>
                        <rad:RadCompactButton></rad:RadCompactButton>
                    </MainButtons>
                    <CompactButtons>
                        <rad:RadSocialButton SocialNetType="LinkedIn"></rad:RadSocialButton>
                        <rad:RadSocialButton SocialNetType="GoogleBookmarks"></rad:RadSocialButton>
                        <rad:RadSocialButton SocialNetType="Tumblr"></rad:RadSocialButton>
                        <rad:RadSocialButton SocialNetType="MailTo"></rad:RadSocialButton>
                    </CompactButtons>
                </rad:radsocialshare>
            </div>
        </div>
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
                <div id="SpeakerDetails" class="top-margin">
                    <div class="row-div meeting-speaker-detail-header">
                        Speaker Details
                    </div>
                    <div runat="server" id="pnlSpeakers">
                        <asp:Label runat="server" ID="lblSpeakers" Visible="false">
                        </asp:Label>
                        <rad:radgrid id="grdSpeakers" runat="server" autogeneratecolumns="False" allowpaging="true"
                            sortingsettings-sorteddesctooltip="Sorted Descending" sortingsettings-sortedasctooltip="Sorted Ascending"
                            allowfilteringbycolumn="true">
					<GroupingSettings CaseSensitive="false" />
					<GroupingSettings CaseSensitive="false" />
					<GroupingSettings CaseSensitive="false" />
					<GroupingSettings CaseSensitive="false" />
					<MasterTableView AllowFilteringByColumn="true" AllowSorting="true" AllowNaturalSort="false">
						<Columns>
							<rad:GridTemplateColumn HeaderText="First Name" AllowFiltering="true" DataField="FirstName" SortExpression="FirstName" 
                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
								<ItemTemplate>
									<asp:Label ID="lblFirstName" runat="server" Text='
									<%# DataBinder.Eval(Container.DataItem,"FirstName") %>
										'>
										</asp:Label>
									</ItemTemplate>
									<ItemStyle/>
								</rad:GridTemplateColumn>
								<rad:GridTemplateColumn HeaderText="Last Name" AllowFiltering="true" DataField="LastName" SortExpression="LastName" 
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
									<ItemTemplate>
										<asp:Label ID="lblLastName" runat="server" Text='
										<%# DataBinder.Eval(Container.DataItem,"LastName") %>
											'>
											</asp:Label>
										</ItemTemplate>
										<ItemStyle/>
									</rad:GridTemplateColumn>
									<rad:GridTemplateColumn HeaderText="Title" AllowFiltering="true" DataField="Title" SortExpression="Title" 
                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
										<ItemTemplate>
											<asp:Label ID="lblTitle" runat="server" Text='
											<%# DataBinder.Eval(Container.DataItem,"Title") %>
												'>
												</asp:Label>
											</ItemTemplate>
											<ItemStyle/>
										</rad:GridTemplateColumn>
										<rad:GridBoundColumn DataField="Type" HeaderText="Type" AllowFiltering="true" SortExpression="Type" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                                       
									</Columns>
								</MasterTableView>
							</rad:radgrid>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div id="ScheduleDetails" class=" row-div top-margin shedule-detail-header">
                    Schedule Details
                </div>
                <div class="row-div">
                    <asp:Label ID="lblInfo" runat="server" Text="Select session for meeting registration"
                        CssClass="registration-info1">
                    </asp:Label>
                    <asp:Button runat="server" ID="btnMySessionCalendar" Text="Show My Session Calendar"
                        CssClass="submit-Btn" Visible="false" />
                </div>
                <div class="row-div">
                    <div runat="server" id="pnlSchedule">
                        <asp:Label runat="server" ID="lblSchedule" Visible="false">
                        </asp:Label>
                        <asp:Label ID="lblAdded" Visible="False" runat="server"></asp:Label>
                        <rad:radgrid id="grdSchedule" runat="server" autogeneratecolumns="False" allowpaging="true"
                            allowfilteringbycolumn="true" allowsorting="true">
						<GroupingSettings CaseSensitive="false" />
						<GroupingSettings CaseSensitive="false" />
						<GroupingSettings CaseSensitive="false" />
						<GroupingSettings CaseSensitive="false" />
						<MasterTableView AllowFilteringByColumn="true" AllowSorting="true">
							<Columns>
								<%--anil Issue 14381--%>
									<rad:GridTemplateColumn HeaderText="Select" AllowFiltering="false">
										<ItemStyle>
										</ItemStyle>
										<HeaderStyle/>
										<HeaderTemplate>
											<asp:CheckBox ID="chkAllSession" runat="server" OnCheckedChanged="ToggleSelectedState" AutoPostBack="True" />
										</HeaderTemplate>
										<ItemTemplate>
											<asp:CheckBox ID="chkSession" runat="server" />
										</ItemTemplate>
									</rad:GridTemplateColumn>
									
												<rad:GridTemplateColumn HeaderText="Session" AllowFiltering="true" DataField="WebName" 
                                                SortExpression="WebName" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" 
                                                ShowFilterIcon="false">
													<ItemTemplate>
														<asp:HyperLink ID="lnkWebName" runat="server" Text='
														<%# DataBinder.Eval(Container.DataItem,"WebName") %>
															' NavigateUrl='
															<%# DataBinder.Eval(Container.DataItem,"MeetingUrl") %>
																'>
																</asp:HyperLink>
															</ItemTemplate>
															<ItemStyle/>
														</rad:GridTemplateColumn>
														<rad:GridDateTimeColumn  HeaderText="Start Date" DataField="StartDate" AllowFiltering="true" SortExpression="StartDate" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false" DataType="System.DateTime" EnableTimeIndependentFiltering="True" UniqueName="GridDateTimeColumnStartDate" />
														<rad:GridDateTimeColumn HeaderText="End Date" DataField="EndDate" AllowFiltering="true" SortExpression="EndDate" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false" DataType="System.DateTime" EnableTimeIndependentFiltering="True" UniqueName="GridDateTimeColumnEndDate" />
														<rad:GridTemplateColumn HeaderText="Place" AllowFiltering="true" DataField="Location" SortExpression="Location" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="false">
															<ItemTemplate>
																<asp:Label ID="lblLocation" runat="server" Text='
																<%# DataBinder.Eval(Container.DataItem,"Location") %>
																	'>
																	</asp:Label>
																	<asp:Label ID="lblProductID" runat="server" Visible="false" Text='
																	<%# DataBinder.Eval(Container.DataItem,"ProductID") %>
																		'>
																		</asp:Label>
																	</ItemTemplate>
																	<ItemStyle/>
																</rad:GridTemplateColumn>
																<rad:GridBoundColumn DataField="Price" HeaderText="Price" AllowFiltering="true" SortExpression="Price" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
															</Columns>
														</MasterTableView>
													</rad:radgrid>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div runat="server" id="trRegister" class="top-margin">
            <asp:Button runat="server" ID="btnRegister" Text="Register Individual" CssClass="submit-Btn" />
            <asp:Button runat="server" ID="btnRegisterGroup" Text="Register Group" CssClass="submit-Btn" />
            <asp:Button runat="server" ID="btnBack" Text="Back" CssClass="submit-Btn" />
        </div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div id="ForumDiv" runat="server">
                    <div class="meeting-discussionforumheader top-margin">
                        Discussion Forum for&nbsp;
                        <asp:Label ID="lblProductName" runat="server">
                        </asp:Label>
                    </div>
                    <div runat="server" id="pnlForum">
                        <asp:Label runat="server" ID="lblForum" Visible="false">
                        </asp:Label>
                        <br />
                        <uc1:singleforum id="SingleForum" runat="server" />
                        <p>
                        </p>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="SingleForum" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <div class="right-container w25" id="Div1" runat="server">
                <div class="meeting-peopleheader">
                    People at Meeting
                </div>
                <div class="meeting-rightpanescrollborder padding-left">
                    <asp:Panel ID="pnl" runat="server">
                        <asp:Label runat="server" ID="lblPeopleYouMayKnow" Visible="False">
                        </asp:Label>
                        <asp:Repeater ID="repPeopleYouMayKnow" runat="server">
                            <ItemTemplate>
                                <div class="row-div-bottom-line clearfix">
                                    <div class="meeting-member-img">
                                        <rad:radbinaryimage id="imgProfileRad" runat="server" autoadjustimagecontrolsize="false"
                                            resizemode="Fill" />
                                    </div>
                                    <div>
                                        <asp:HyperLink ID="lnkName" runat="server">
                                        </asp:HyperLink>
                                        <asp:HyperLink ID="RelatedPersonCompanyName" runat="server" Visible="false">
                                        </asp:HyperLink>
                                        <asp:CheckBox ID="chkPersonDirExclude" runat="server" Visible="false" />
                                        <asp:CheckBox ID="chkCompanyDirExclude" runat="server" Visible="false" />
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </asp:Panel>
                </div>
                <div id="TravelDiscount" class="meeting-traveldiscountheader">
                    Travel Discounts
                </div>
                <div class="meeting-rightpaneborder padding-left">
                    <asp:Label runat="server" ID="lblTravel" Visible="false">
                    </asp:Label>
                    <asp:Repeater ID="repTravelDiscounts" runat="server">
                        <ItemTemplate>
                            <div class="row-div-bottom-line clearfix">
                                <div class="label-div-left-align w99">
                                    Hotel:
                                </div>
                                <div class="w99">
                                    <asp:Label ID="lblHotelName" runat="server">
                                    </asp:Label>
                                </div>
                                <div class="w99">
                                    <asp:Label ID="lblGroupOffer" runat="server">
                                    </asp:Label>
                                </div>
                                <div class="row-div clearfix">
                                    <div class="label-div-left-align">
                                        From:
                                    </div>
                                    <div class="field-div1">
                                        <asp:Label ID="lblStartDate" runat="server">
                                        </asp:Label></div>
                                    <div class="label-div-left-align">
                                        To:
                                    </div>
                                    <div class="field-div2">
                                        <asp:Label ID="lblEndDate" runat="server">
                                        </asp:Label></div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                <div id="RelatedEvents">
                    <div runat="server" id="RelatedEventsHeader">
                        <div runat="server" id="tdRelatedEventsHeader" class="meeting-relatedeventsheader">
                            Related Events
                        </div>
                    </div>
                    <div class="meeting-rightpaneborder padding-left" runat="server" id="RightPaneBorder">
                        <asp:Label ID="lblRelatedEvents" runat="server">
                        </asp:Label>
                        <asp:Repeater ID="repRelatedEvents" runat="server">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkNewProductName" runat="server">
                                </asp:LinkButton>
                                <div class="row-div clearfix">
                                    <div class="label-div-left-align">
                                        Category:</div>
                                    <div class="field-div1">
                                        <asp:Label ID="lblCategory" runat="server">
                                        </asp:Label></div>
                                </div>
                                <div class="row-div clearfix">
                                    <div class="label-div-left-align">
                                        Description:</div>
                                    <div class="field-div1">
                                        <asp:Label ID="lblDescription" runat="server">
                                        </asp:Label></div>
                                </div>
                                <div class="row-div clearfix">
                                    <div class="label-div-left-align">
                                        Location:</div>
                                    <div class="field-div1">
                                        <asp:Label ID="lblLocation" runat="server">
                                        </asp:Label></div>
                                </div>
                                <div class="row-div clearfix">
                                    <div class="label-div-left-align">
                                        Start Date & Time:</div>
                                    <div class="field-div1">
                                        <asp:Label ID="lblStartDate" runat="server">
                                        </asp:Label></div>
                                </div>
                                <div class="row-div clearfix">
                                    <div class="label-div-left-align">
                                        End Date & Time:</div>
                                    <div class="field-div1">
                                        <asp:Label ID="lblEndDate" runat="server">
                                        </asp:Label></div>
                                </div>
                                <div class="row-div clearfix">
                                    <div class="label-div-left-align">
                                        Registration Price:</div>
                                    <div class="field-div1">
                                        <asp:Label ID="lblRegPrice" runat="server">
                                        </asp:Label></div>
                                </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanelSessionConflictError" runat="server">
        <ContentTemplate>
            <rad:radwindow id="radErrorMessage" runat="server" modal="True" 
                CssClass="popup-win-meeting-conflicts" skin="Default" visiblestatusbar="False"
                behaviors="None" iconurl="~/Images/Alert.png" title="Alert"
                behavior="None">
                    <ContentTemplate>
                        <div class="row-div errormsg-div">
                            <asp:ListView ID="lstErrorMessage" runat="server">
                                <ItemTemplate>
                                    <asp:Label ID="lblErrorMessage" runat="server" Text='<% #eval("ErrorMessage") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:ListView>
                        </div>

                        <div class="row-div">
                            <asp:Button ID="btnPopUpOk" runat="server" Text="OK" class="submit-Btn"
                                ValidationGroup="ok" />
                        </div>
                    </ContentTemplate>
                </rad:radwindow>
        </ContentTemplate>
    </asp:UpdatePanel>
    <cc1:user id="User1" runat="server" />
    <cc2:aptifyshoppingcart id="ShoppingCart1" runat="server"
        visible="False"></cc2:aptifyshoppingcart>
</div>
