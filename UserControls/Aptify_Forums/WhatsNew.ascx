<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="WhatsNew.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.Forums.WhatsNewControl" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<div id="tblMain" runat="server" class="table-div">
    <div class="row-div">
        <div class="row-div">
            <span class="Title">
                <asp:Label ID="lblDiscussionForum" runat="server">
                </asp:Label>
            </span>
        </div>
        <div class="row-div">
            <asp:Panel ID="PnlPosting" runat="server">
                Show postings in the last
                <asp:DropDownList ID="cmbRecency" runat="server">
                    <asp:ListItem Value="1" Selected="True">
									1 Day
                    </asp:ListItem>
                    <asp:ListItem Value="2">
									2 Days
                    </asp:ListItem>
                    <asp:ListItem Value="5">
									5 Days
                    </asp:ListItem>
                    <asp:ListItem Value="7">
									1 Week
                    </asp:ListItem>
                    <asp:ListItem Value="14">
									2 Weeks
                    </asp:ListItem>
                    <asp:ListItem Value="30">
									1 Month
                    </asp:ListItem>
                    <asp:ListItem Value="90">
									3 Months
                    </asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="cmdRefresh" runat="server" Text="Refresh" CssClass="submit-Btn"></asp:Button>
            </asp:Panel>
        </div>
        <div class="row-div">
            <asp:UpdatePanel ID="UppanelGrid" runat="server">
                <ContentTemplate>
                    <rad:radgrid id="grdResults" runat="server" autogeneratecolumns="False" allowpaging="true"
                        sortingsettings-sorteddesctooltip="Sorted Descending" sortingsettings-sortedasctooltip="Sorted Ascending"
                        allowfilteringbycolumn="true">
                                <GroupingSettings CaseSensitive="false" />
                                <MasterTableView AllowSorting="true" AllowNaturalSort="false" AllowFilteringByColumn="true">
                                    <Columns>
                                        <rad:GridTemplateColumn HeaderText="Forum" DataField="Forum" SortExpression="Forum"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="lnkForum" runat="server" Text='
																		<%# DataBinder.Eval(Container.DataItem,"Forum") %>
																			' NavigateUrl='
																			<%# DataBinder.Eval(Container.DataItem,"DataNavigateUrl") %>
																				'>
                                                </asp:HyperLink>
                                            </ItemTemplate>
                                        </rad:GridTemplateColumn>
                                        <rad:GridDateTimeColumn DataField="MostRecentPostingDate" UniqueName="GridDateTimeColumnMostRecentPostingDate"
                                            HeaderText="Most Recent" 
                                            SortExpression="MostRecentPostingDate" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                                            DataType="System.DateTime" ShowFilterIcon="false" EnableTimeIndependentFiltering="true"
                                            FilterListOptions="VaryByDataType" />
                                           <%-- Issue 19889 Search does not work where numeric values are entered Added by Sachin K 01/09/2014--%>
                                        <rad:GridBoundColumn DataField="NumNewPostings" HeaderText="# New Postings" 
                                            ItemStyle-HorizontalAlign="Right" SortExpression="NumNewPostings" DataType="System.Int32"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false"
                                            HeaderStyle-HorizontalAlign="Right" />
                                    </Columns>
                                </MasterTableView>
                            </rad:radgrid>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</div>
<cc1:user id="User1" runat="server" />
