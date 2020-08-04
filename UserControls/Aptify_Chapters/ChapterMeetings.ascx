<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ChapterMeetings.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.Chapters.ChapterMeetingsControl" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="cc5" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessShoppingCart" %>
<%@ Register TagPrefix="uc1" TagName="ChapterMember" Src="ChapterMember.ascx" %>
<%@ Register TagPrefix="uc1" TagName="NameAddressBlock" Src="../Aptify_General/NameAddressBlock.ascx" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<div class="chaptermain-div clearfix" id="tblMain" runat="server">
    <div class="errormsg-div clearfix">
        <asp:Label ID="lblError" runat="server">
        </asp:Label>
    </div>
    <div class="row-div-bottom-line">
    <div class="control-title">
        <asp:Label ID="lblChapterName" runat="server">
        </asp:Label>
    </div>
    </div>
    <div class="row-div dropdown">
    <div class="chapter-meeting-div">
        <asp:DropDownList ID="cmbType" runat="server" AutoPostBack="True" CssClass="w10">
            <asp:ListItem Value="Planned" Selected="True">Planned</asp:ListItem>
            <asp:ListItem Value="Past">Past</asp:ListItem>
            <asp:ListItem Value="All">All</asp:ListItem>
        </asp:DropDownList>
        <asp:Button ID="cmdAdd" runat="server" Text="Add Meeting" CssClass="submit-Btn">
        </asp:Button>
        </div>
        <div class="row-div top-margin">
            <asp:UpdatePanel ID="updPanelGrid" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <rad:radgrid id="grdMeetings" runat="server" datakeynames="ID" autogeneratecolumns="false"
                        allowpaging="true" allowfilteringbycolumn="true" CssClass="row-div">
						<GroupingSettings CaseSensitive="false" />
						<MasterTableView AllowFilteringByColumn="true" AllowSorting="true">
							<Columns>
								<rad:GridTemplateColumn Visible="false">
									<ItemTemplate>
										<asp:Label ID="lblID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ID") %>'>
											</asp:Label>
										</ItemTemplate>
									</rad:GridTemplateColumn>
									<telerik:GridTemplateColumn AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" DataField="Name" 
                                    HeaderText="Meeting" ShowFilterIcon="false" SortExpression="Name">
										<HeaderStyle />
										<ItemStyle/>
										<ItemTemplate>
											<asp:HyperLink ID="lnkMeetingTitle" runat="server" NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"MeetingTitleUrl") %>' Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'>
													</asp:HyperLink>
												</ItemTemplate>
											</telerik:GridTemplateColumn>
											<rad:GridBoundColumn DataField="Type" HeaderText="Type" SortExpression="Type" AutoPostBackOnFilter="true" 
                                            CurrentFilterFunction="Contains" ShowFilterIcon="false"/>
											<rad:GridBoundColumn DataField="Description" HeaderText="Description" SortExpression="Description" 
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" AllowSorting="false" />
											<rad:GridDateTimeColumn UniqueName="GridDateTimeColumnStartDate" AllowSorting="true" Visible="True" HeaderText="Start Date" DataField="StartDate" 
                                            SortExpression="StartDate" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false" DataType="System.DateTime" EnableTimeIndependentFiltering="true">
												<ItemStyle/>
											</rad:GridDateTimeColumn>
											<rad:GridBoundColumn DataField="Status" HeaderText="Status" SortExpression="Status" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" 
                                            ShowFilterIcon="false"/>
											<rad:GridTemplateColumn HeaderText="Delete" AllowFiltering="false">
												<ItemTemplate>
													<asp:ImageButton ID="btnDelete" runat="server" ImageUrl="~/Images/Delete.png" CommandName="Delete" CommandArgument="<%# CType(Container, GridDataItem).ItemIndex%>" />
													</ItemTemplate>
												</rad:GridTemplateColumn>
											</Columns>
										</MasterTableView>
									</rad:radgrid>
                    <asp:Label ID="lblNoMeetings" runat="server" Visible="False">No Meetings</asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="row-div">
        <asp:LinkButton ID="lnkChapter" runat="server">Go To Chapter</asp:LinkButton>
    </div>
    <cc5:aptifyshoppingcart runat="server" id="ShoppingCart1" />
    <cc3:user id="User1" runat="server" />
</div>
