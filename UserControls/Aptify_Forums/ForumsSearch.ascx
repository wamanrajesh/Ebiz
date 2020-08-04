<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ForumsSearch.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.Forums.SearchControl"
    Debug="true" %>
<%@ Register TagPrefix="cc4" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="uc1" TagName="ForumTree" Src="~/UserControls/Aptify_General/ForumTree.ascx" %>
<div id="tblMain" runat="server" class="table-div">
    <div class="row-div">
        <asp:HyperLink runat="server" ID="lnkForumsHome" Text="Return to Forums">
        </asp:HyperLink>
        <div id="tblInner" runat="server">
            <div class="row-div errormsg-div">
                <asp:Label ID="lblError" runat="server" Visible="False">
								Please enter at least one search criteria below
                </asp:Label>
            </div>
            <div class="row-div clearfix">
                <div class="label-div w25">
                    Forum:
                </div>
                <div class="field-div1 w74">
                    <asp:CheckBox ID="chkAllForums" AutoPostBack="True" runat="server" Text=" All Forums"
                        Checked="True"></asp:CheckBox>
                    <uc1:forumtree id="ForumTree" runat="server"></uc1:forumtree>
                </div>
            </div>
            <div class="row-div clearfix">
                <div class="label-div w25">
                    Subject Containing:
                </div>
                <div class="field-div1 w74">
                    <asp:TextBox ID="txtTitle" runat="server">
                    </asp:TextBox>
                </div>
            </div>
            <div class="row-div clearfix">
                <div class="label-div w25">
                    Body Containing: 
                </div>
                <div class="field-div1 w74">
                    <asp:TextBox ID="txtBody" runat="server">
                    </asp:TextBox>
                </div>
            </div>
            <div class="row-div clearfix">
                <div class="label-div w25">
                    Search for Postings in the last:
                </div>
                <div class="field-div1 w74">
                    <asp:DropDownList ID="cmbRecency" runat="server">
                        <asp:ListItem Value="1">
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
                        <asp:ListItem Value="30" Selected="True">
									1 Month
                        </asp:ListItem>
                        <asp:ListItem Value="90">
									3 Months
                        </asp:ListItem>
                        <asp:ListItem Value="182">
									6 Months
                        </asp:ListItem>
                        <asp:ListItem Value="273">
									9 Months
                        </asp:ListItem>
                        <asp:ListItem Value="365">
									12 Months
                        </asp:ListItem>
                        <asp:ListItem Value="-1">
									All Postings
                        </asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row-div clearfix">
                <div class="label-div w25">
                    &nbsp;</div>
                <div class="field-div1 w74">
                    <asp:Button CssClass="submit-Btn" ID="cmdSearch" runat="server" Text="Search"></asp:Button>
                </div>
            </div>
        </div>
        <div runat="server" id="tblResults" class="row-div">
            <div class="rTableCell">
                Search Results 
                <br />
                <asp:Button CssClass="submit-Btn" ID="cmdChangeSearch" runat="server" Text="Change Search">
                </asp:Button>
            </div>
            <div class="row-div">
                <asp:UpdatePanel ID="UppanelGrid" runat="server">
                    <ContentTemplate>
                        <rad:radgrid id="grdResults" runat="server" visible="False" autogeneratecolumns="False"
                            sortingsettings-sorteddesctooltip="Sorted Descending" sortingsettings-sortedasctooltip="Sorted Ascending"
                            allowpaging="true" allowfilteringbycolumn="true" allowsorting="true">
                                <GroupingSettings CaseSensitive="false" />
                                <MasterTableView AllowSorting="true" AllowNaturalSort="false" AllowFilteringByColumn="true">
                                    <Columns>
                                        <rad:GridTemplateColumn HeaderText="Forum" DataField="Forum" SortExpression="Forum"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"
                                            >
                                            <ItemTemplate>
                                                <asp:HyperLink ID="lnkForum" runat="server" Text='
																		<%# DataBinder.Eval(Container.DataItem,"Forum") %>
																			' NavigateUrl='
																			<%# DataBinder.Eval(Container.DataItem,"DataNavigateUrl") %>
																				'>
                                                </asp:HyperLink>
                                            </ItemTemplate>
                                        </rad:GridTemplateColumn>
                                        <rad:GridDateTimeColumn DataField="DateEntered" UniqueName="GridDateTimeColumnDateEntered"
                                            HeaderText="Date" SortExpression="DateEntered" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                                            DataType="System.DateTime" ShowFilterIcon="false" EnableTimeIndependentFiltering="true"
                                            FilterListOptions="VaryByDataType" />
                                        <rad:GridTemplateColumn HeaderText="Subject" DataField="Subject" SortExpression="Subject"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="lnkSubject" runat="server" Text='
																				<%# DataBinder.Eval(Container.DataItem,"Subject") %>
																					' NavigateUrl='
																					<%# DataBinder.Eval(Container.DataItem,"DataNavigateUrlSub") %>
																						'>
                                                </asp:HyperLink>
                                            </ItemTemplate>
                                        </rad:GridTemplateColumn>
                                        <rad:GridBoundColumn DataField="Body" HeaderText="Body" AutoPostBackOnFilter="true"
                                            ShowFilterIcon="false" AllowSorting="false" />
                                    </Columns>
                                </MasterTableView>
                            </rad:radgrid>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</div>
<cc4:user runat="server" id="User1" />
