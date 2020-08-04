<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="MeetingsHome.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.Meetings.MeetingsHome" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessShoppingCart" %>
<%@ Register TagPrefix="cc6" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div id="tblMain" runat="server" class="table-div">
            <div class="meetingmain-div clearfix">
                <div class="label-div w25">
                    Meeting Category :
                </div>
                <div class="field-div1">
                    <asp:DropDownList runat="server" ID="cmbCategory" AutoPostBack="true">
                    </asp:DropDownList>
                </div>
                <div class="field-div2">
                    <asp:DropDownList runat="server" ID="cmbStatus" AutoPostBack="true">
                        <asp:ListItem Text="Upcoming">
                        </asp:ListItem>
                        <asp:ListItem Text="Past">
                        </asp:ListItem>
                        <asp:ListItem Text="All">
                        </asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="calendar-view-text clearfix">
                    <asp:HyperLink ID="MeetingsCalendarPage" runat="server" Text="Calendar View" />
                </div>
            </div>
            <div class="row-div label clearfix">
                <asp:Label ID="lblMessage" runat="server">
                </asp:Label>
            </div>
            <div class="meeting-grid-div">
                <rad:radgrid id="grdMeetings" runat="server" autogeneratecolumns="False" allowpaging="True"
                    sortingsettings-sorteddesctooltip="Sorted Descending" sortingsettings-sortedasctooltip="Sorted Ascending"
                    allowfilteringbycolumn="True" allowsorting="True">
					<GroupingSettings CaseSensitive="false" />
					<SortingSettings SortedAscToolTip="Sorted Ascending" SortedDescToolTip="Sorted Descending" />
					<MasterTableView DataKeyNames="ID" AllowFilteringByColumn="True" AllowNaturalSort="false" AllowSorting="True" AllowPaging="True">
						<DetailTables>
									<telerik:GridTableView DataKeyNames="ID" Name="ChildGrid" Width="100%" runat="server" AllowFilteringByColumn="false" AllowNaturalSort="false" 
                                    SortingSettings-SortedDescToolTip="Sorted Descending" SortingSettings-SortedAscToolTip="Sorted Ascending" AllowPaging="false" 
                                    NoDetailRecordsText="Nothing to display">
										<CommandItemSettings ExportToPdfText="Export to PDF" />
										<Columns>
											<rad:GridTemplateColumn HeaderText="Session" SortExpression="WebName" AllowFiltering="false" DataField="WebName" 
                                            CurrentFilterFunction="Contains" ShowFilterIcon="false">
												<ItemTemplate>
													<asp:HyperLink ID="lnkWebName" runat="server" Text='
													<%# DataBinder.Eval(Container.DataItem,"WebName") %>
														' NavigateUrl='
														<%# DataBinder.Eval(Container.DataItem,"MeetingUrl") %>
															'>
															</asp:HyperLink>
														</ItemTemplate>
														<ItemStyle />
														<HeaderStyle />
													</rad:GridTemplateColumn>
													<rad:GridTemplateColumn HeaderText="Category" SortExpression="WebCategoryName" DataField="WebCategoryName" 
                                                    AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false">
														<ItemTemplate>
															<asp:Label ID="lblWebCategoryName" runat="server" Text='
															<%# DataBinder.Eval(Container.DataItem,"WebCategoryName") %>
																'>
																</asp:Label>
															</ItemTemplate>
															<ItemStyle  />
                                                            <HeaderStyle />
														</rad:GridTemplateColumn>
															<rad:GridDateTimeColumn AllowFiltering="false" SortExpression="AvailableUntil" 
                                                            UniqueName="GridDateTimeColumnRegisteredDateDetails" Visible="True" HeaderText="Registered By Date" 
                                                            DataField="AvailableUntil" AutoPostBackOnFilter="false" CurrentFilterFunction="EqualTo" ShowFilterIcon="false" DataType="System.DateTime" 
                                                            EnableTimeIndependentFiltering="true">
																<ItemStyle />
                                                                <HeaderStyle />
															</rad:GridDateTimeColumn>
																<rad:GridTemplateColumn HeaderText="Description" DataField="Smalldesc" AllowFiltering="false" 
                                                                AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" SortExpression="" HeaderTooltip="" 
                                                                ShowFilterIcon="false">
																	<ItemTemplate>
																		<asp:Label ID="lblSmalldesc" runat="server" Text='
																		<%# DataBinder.Eval(Container.DataItem,"Smalldesc") %>
																			'>
																			</asp:Label>
																			<rad:RadToolTip ID="RadToolTip1" runat="server" TargetControlID="lblSmalldesc" Animation="Slide" 
                                                                            RelativeTo="Element" Position="BottomCenter" RenderInPageRoot="true">
																				<%# DataBinder.Eval(Container.DataItem, "VerboseDescription")%>
																				</rad:RadToolTip>
																			</ItemTemplate>
																			<ItemStyle />
                                                                            <HeaderStyle />
																		</rad:GridTemplateColumn>
																		<rad:GridTemplateColumn HeaderText="Location" SortExpression="Location" DataField="Location" 
                                                                        AllowFiltering="false" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="false">
																			<ItemTemplate>
																				<asp:Label ID="lblLocation" runat="server" Text='
																				<%# DataBinder.Eval(Container.DataItem,"Location") %>
																					'>
																					</asp:Label>
																				</ItemTemplate>
																				<ItemStyle />
                                                                                <HeaderStyle />
																			</rad:GridTemplateColumn>
																				<rad:GridDateTimeColumn UniqueName="GridDateTimeColumnStartDateDetails" SortExpression="StartDate" 
                                                                                AllowFiltering="false" Visible="True" HeaderText="Start Date" DataField="StartDate" AutoPostBackOnFilter="true" 
                                                                                CurrentFilterFunction="EqualTo" ReadOnly="true" ShowFilterIcon="false" DataType="System.DateTime" 
                                                                                EnableTimeIndependentFiltering="true">
																					<ItemStyle />
																					<HeaderStyle />
																				</rad:GridDateTimeColumn>
																					<rad:GridDateTimeColumn UniqueName="GridDateTimeColumnEndDateDetails" SortExpression="EndDate"
                                                                                     AllowFiltering="false" Visible="True" HeaderText="End Date" DataField="EndDate" ReadOnly="true" 
                                                                                     AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false" DataType="System.DateTime" 
                                                                                     EnableTimeIndependentFiltering="true">
																						<ItemStyle />
                                                                                        <HeaderStyle />
																					</rad:GridDateTimeColumn>
																					<rad:GridTemplateColumn HeaderText="Price" SortExpression="Price" DataField="Price" 
                                                                                    AllowFiltering="false" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
																						<ItemStyle >
																						</ItemStyle>
																						<HeaderStyle />
																						<ItemTemplate>
																							<asp:Label ID="lblPrice" runat="server" Text='
																							<%# DataBinder.Eval(Container.DataItem,"Price") %>
																								'>
																								</asp:Label>
																							</ItemTemplate>
																							<ItemStyle />
                                                                                            <HeaderStyle />
																						</rad:GridTemplateColumn>
																						<rad:GridTemplateColumn HeaderText="Rating" DataField="MeetingRate" AllowFiltering="false">
																							<ItemStyle CssClass="w13">
																							</ItemStyle>
																							<HeaderStyle />
																							<ItemTemplate>
																								<rad:RadRating ID="radRateIDMain" runat="server" Value='
																								<%# DataBinder.Eval(Container.DataItem,"MeetingRate") %>
																									' Skin="Default" Enabled="false" Precision="Half">
																									</rad:RadRating>
																									<center>
																										<asp:Label ID="lblRatingDetails" runat="server" Text="Not Yet Rated">
																										</asp:Label>
																									</center>
																								</ItemTemplate>
                                                                                                <ItemStyle />
																								<HeaderStyle />
																							</rad:GridTemplateColumn>
																						</Columns>
																					</telerik:GridTableView>
																				</DetailTables>
																				<CommandItemSettings ExportToPdfText="Export to PDF" />
																				<Columns>
																					<rad:GridTemplateColumn HeaderText="ID" HeaderButtonType="TextButton" SortExpression="MeetingID" 
                                                                                    DataField="ID" UniqueName="ID" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false" Visible="false">
																						<ItemTemplate>
																							<asp:Label ID="Product" runat="server" Text='
																							<%# DataBinder.Eval(Container.DataItem,"ID") %>
																								'>
																								</asp:Label>
																							</ItemTemplate>
																						</rad:GridTemplateColumn>
																						<rad:GridTemplateColumn HeaderText="Meeting" AllowFiltering="true" DataField="WebName" 
                                                                                        SortExpression="WebName" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
																							<ItemTemplate>
																								<asp:HyperLink ID="lnkWebName" runat="server" Text='
																								<%# DataBinder.Eval(Container.DataItem,"WebName") %>
																									' NavigateUrl='
																									<%# DataBinder.Eval(Container.DataItem,"MeetingUrl") %>
																										'>
																										</asp:HyperLink>
																									</ItemTemplate>
																									<ItemStyle />
																									<HeaderStyle />
																								</rad:GridTemplateColumn>
																								<rad:GridTemplateColumn HeaderText="Category" DataField="WebCategoryName" 
                                                                                                AllowFiltering="true" SortExpression="WebCategoryName" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
																									<ItemTemplate>
																										<asp:Label ID="lblWebCategoryName" runat="server" Text='
																										<%# DataBinder.Eval(Container.DataItem,"WebCategoryName") %>
																											'>
																											</asp:Label>
																										</ItemTemplate>
																										<ItemStyle  />
																										<HeaderStyle />
																									</rad:GridTemplateColumn>
																									<rad:GridDateTimeColumn UniqueName="GridDateTimeColumnRegisteredDate" 
                                                                                                    AllowSorting="true" Visible="True" HeaderText="Registered By Date" DataField="AvailableUntil" 
                                                                                                    SortExpression="AvailableUntil" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" 
                                                                                                    ShowFilterIcon="false" DataType="System.DateTime" EnableTimeIndependentFiltering="true">
																										<ItemStyle />
                                                                                                        <HeaderStyle />
																									</rad:GridDateTimeColumn>
																										<rad:GridTemplateColumn HeaderText="Description" DataField="Smalldesc" 
                                                                                                        AllowFiltering="true" SortExpression="" HeaderTooltip="" AutoPostBackOnFilter="true" 
                                                                                                        CurrentFilterFunction="Contains" ShowFilterIcon="false">
																											<ItemTemplate>
																												<asp:Label ID="lblSmalldesc" runat="server" Text='
																												<%# DataBinder.Eval(Container.DataItem,"Smalldesc") %>
																													'>
																													</asp:Label>
																													<rad:RadToolTip ID="RadToolTip1" runat="server" TargetControlID="lblSmalldesc" 
                                                                                                                    Animation="Slide" RelativeTo="Element" Position="BottomCenter" RenderInPageRoot="true">
																														<%# DataBinder.Eval(Container.DataItem, "VerboseDescription")%>
																														</rad:RadToolTip>
																													</ItemTemplate>
																													<ItemStyle />
																													<HeaderStyle />
																												</rad:GridTemplateColumn>
																												<rad:GridTemplateColumn HeaderText="Location" DataField="Location" AllowFiltering="true" 
                                                                                                                SortExpression="Location" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" 
                                                                                                                ShowFilterIcon="false">
																													<ItemTemplate>
																														<asp:Label ID="lblLocation" runat="server" Text='
																														<%# DataBinder.Eval(Container.DataItem,"Location") %>
																															'>
																															</asp:Label>
																														</ItemTemplate>
																														<ItemStyle />
																														<HeaderStyle />
																													</rad:GridTemplateColumn>
																													<rad:GridDateTimeColumn UniqueName="GridDateTimeColumnStartDate" AllowSorting="true"  
                                                                                                                    Visible="True" HeaderText="Start Date" DataField="StartDate" SortExpression="StartDate" 
                                                                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false" 
                                                                                                                    DataType="System.DateTime" EnableTimeIndependentFiltering="true">
																														<ItemStyle />
																														<HeaderStyle />
																													</rad:GridDateTimeColumn>
																													<rad:GridDateTimeColumn UniqueName="GridDateTimeColumnEndDate" AllowSorting="true" 
                                                                                                                    AllowFiltering="false"  Visible="True" HeaderText="End Date" DataField="EndDate" 
                                                                                                                    SortExpression="EndDate" ReadOnly="true" AutoPostBackOnFilter="false" 
                                                                                                                    CurrentFilterFunction="EqualTo" ShowFilterIcon="false" DataType="System.DateTime" 
                                                                                                                    EnableTimeIndependentFiltering="true">
																														<HeaderStyle />
																														<ItemStyle />
																													</rad:GridDateTimeColumn>
																													<rad:GridTemplateColumn HeaderText="Price" DataField="Price" AllowFiltering="true" 
                                                                                                                    SortExpression="Price" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" 
                                                                                                                    ShowFilterIcon="false">
																														<ItemStyle >
																														</ItemStyle>
																														<HeaderStyle />
																														<ItemTemplate>
																															<asp:Label ID="lblPrice" runat="server" Text='
																															<%# DataBinder.Eval(Container.DataItem,"Price") %>
																																'>
																																</asp:Label>
																															</ItemTemplate>
																															<HeaderStyle />
																															<ItemStyle />
																														</rad:GridTemplateColumn>
																														<rad:GridTemplateColumn HeaderText="Rating" DataField="MeetingRate" AllowFiltering="false" >
																															<ItemStyle CssClass="w13">
																															</ItemStyle>
																															<HeaderStyle />
																															<ItemTemplate>
																																<rad:RadRating ID="radRateID" runat="server" Value='
																																<%# DataBinder.Eval(Container.DataItem,"MeetingRate") %>
																																	' Skin="Default" Enabled="false" Precision="Half">
																																	</rad:RadRating>
																																	<center>
																																		<asp:Label ID="lblpendingrating" runat="server" Text="Not Yet Rated" >
																																		</asp:Label>
																																	</center>
																																</ItemTemplate>
																															</rad:GridTemplateColumn>
																														</Columns>
																													</MasterTableView>
																												</rad:radgrid>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<cc6:user id="User1" runat="server" />
<cc2:aptifyshoppingcart id="ShoppingCart1" runat="server" visible="False"></cc2:aptifyshoppingcart>
