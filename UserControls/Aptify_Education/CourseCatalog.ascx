<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="CourseCatalog.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.Education.CourseCatalogControl" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessLogin" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

    <div id="tblMain" runat="server" class="table-div">
        <div class="row-div">
            <asp:DropDownList runat="server" ID="cmbCategory" AutoPostBack="true" ToolTip="Select a category from this list to filter the course catalog" />
            <asp:CheckBox ID="chkSubCat" runat="server" AutoPostBack="True" Text="Include Sub-Categories"
                ToolTip="Check this box to include sub-categories of the selected category" />
        </div>
        <div class="row-div">
            <%-- 'Navin Prasad Issue 11032--%>
            <%--Update Panel added by Suvarna D IssueID: 12436 on Dec 1, 2011 --%>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <contenttemplate>
							<%--Neha Changes for Issue 14452--%>
								<rad:RadGrid ID="grdFilteredCourses" runat="server" AutoGenerateColumns="False" AllowPaging="true" AllowFilteringByColumn="true" SortingSettings-SortedDescToolTip="Sorted Descending" SortingSettings-SortedAscToolTip="Sorted Ascending">
									<GroupingSettings CaseSensitive="false"/>
									<MasterTableView AllowFilteringByColumn="true" AllowSorting="true" AllowNaturalSort="false">
										<Columns>
											<rad:GridTemplateColumn HeaderText="Category" DataField="Category" SortExpression="Category" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" >
												<ItemTemplate>
													<asp:HyperLink ID="lnkCategory" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Category") %>' NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"CategoryUrl") %>'>
													</asp:HyperLink>
												</ItemTemplate>
												<ItemStyle />
											</rad:GridTemplateColumn>
											<rad:GridTemplateColumn HeaderText="Course" DataField="WebName" SortExpression="WebName" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" >
												<ItemTemplate>
													<asp:HyperLink ID="lnkWebName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"WebName") %>' NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"CourseUrl") %>'>
													</asp:HyperLink>
												</ItemTemplate>
												<ItemStyle />
											</rad:GridTemplateColumn>
											<rad:GridTemplateColumn HeaderText="Scope" DataField="Scope" SortExpression="Scope" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" >
												<ItemTemplate>
													<asp:Label ID="lblScope" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Scope") %>'>
													</asp:Label>
												</ItemTemplate>
												<ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
											</rad:GridTemplateColumn>
											<rad:GridTemplateColumn HeaderText="Description" DataField="WebDescription" SortExpression="" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" >
												<HeaderStyle />
													<ItemStyle />
														<ItemTemplate>
															<asp:Label ID="lblWebDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"WebDescription") %>'>
															</asp:Label>
														</ItemTemplate>
													<ItemStyle />
											</rad:GridTemplateColumn>
											<rad:GridTemplateColumn HeaderText="Units" DataField="Units" SortExpression="Units" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"  ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
													<ItemTemplate>
															<asp:Label ID="lblUnits" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Units") %>'>
															</asp:Label>
													</ItemTemplate>
													<ItemStyle />
											</rad:GridTemplateColumn>
											<rad:GridBoundColumn DataField="TotalPartDuration" HeaderText="Duration" DataFormatString="{0:F0} min" ItemStyle-Font-Size="12px" AllowFiltering = "false" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
										</Columns>
									</MasterTableView>
								</rad:RadGrid>
						</contenttemplate>
            </asp:UpdatePanel>
        </div>
        <div class="row-div ">
            <%--Update Panel added by Suvarna D IssueID: 12436 on Dec 1, 2011 --%>
            <asp:UpdatePanel ID="updPanelGrid" runat="server">
                <contenttemplate>
						<rad:RadGrid ID="grdCourses" runat="server" AutoGenerateColumns="False" AllowPaging="true" SortingSettings-SortedDescToolTip="Sorted Descending" SortingSettings-SortedAscToolTip="Sorted Ascending">
                            <GroupingSettings CaseSensitive="false"/>
							<MasterTableView AllowFilteringByColumn="true" AllowSorting="true" AllowNaturalSort="false">
								<Columns>
									    <rad:GridTemplateColumn HeaderText="Category" DataField="Category" SortExpression="Category" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" >
										    <ItemTemplate>
											    <asp:HyperLink ID="lnkCategory" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Category") %>' NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"CategoryUrl") %>'>
											    </asp:HyperLink>
										    </ItemTemplate>
											    <ItemStyle />
							    	    </rad:GridTemplateColumn>
									    <rad:GridTemplateColumn HeaderText="Course" DataField="WebName" SortExpression="WebName" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" >
										    <ItemTemplate>
								    		    <asp:HyperLink ID="lnkWebName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"WebName") %>' NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"CourseUrl") %>'></asp:HyperLink>
										    </ItemTemplate>
										    <ItemStyle />
							    	    </rad:GridTemplateColumn>
									    <rad:GridTemplateColumn HeaderText="Description" DataField="WebDescription" SortExpression="" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" >
                                    	    <HeaderStyle />
											    <ItemStyle />
												    <ItemTemplate>
													    <asp:Label ID="lblWebDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"WebDescription") %>'>
													    </asp:Label>
												    </ItemTemplate>
											    <ItemStyle />
						   		        </rad:GridTemplateColumn>
									    <rad:GridTemplateColumn HeaderText="Units" DataField="Units" SortExpression="Units" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
										    <ItemTemplate>
											    <asp:Label ID="lblUnits" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Units") %>'></asp:Label>
                                            </ItemTemplate>
										    <ItemStyle  />
								        </rad:GridTemplateColumn>
									    <rad:GridBoundColumn DataField="TotalPartDuration" HeaderText="Duration" DataFormatString="{0:F0} min" SortExpression="TotalPartDuration" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"/>
										    <rad:GridTemplateColumn HeaderText="Duration" DataField="TotalPartDuration" SortExpression="TotalPartDuration" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
											    <ItemTemplate>
												    <asp:Label ID="lblTotalPartDuration" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"TotalPartDuration") %>'>
												    </asp:Label>
											    </ItemTemplate>
											    <ItemStyle />
										   </rad:GridTemplateColumn>
						 </Columns>
					</MasterTableView>
				</rad:RadGrid>
			</contenttemplate>
         </asp:UpdatePanel>
        </div>
        <div class="row-div">
            <asp:Label ID="lblError" runat="server" Text="Error" Visible="False">
            </asp:Label>
        </div>
    </div>
    <cc3:AptifyWebUserLogin ID="WebUserLogin1" runat="server" Visible="False"></cc3:AptifyWebUserLogin>

