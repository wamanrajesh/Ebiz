<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="CompanyMembership.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.CustomerService.CompanyMembership" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="cc4" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessShoppingCart" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<div class="purchasemembership-main-div">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row-div label">
                <asp:Label ID="lblGridInfo" runat="server" Text=""></asp:Label>
            </div>
            <div class="row-div label">
                <asp:Label ID="lblmsg" runat="server" Text=""></asp:Label>
            </div>
            <div id="Table1" runat="server">
                <rad:RadGrid ID="grdperson" runat="server" AutoGenerateColumns="False" AllowPaging="true"
                    SortingSettings-SortedDescToolTip="Sorted Descending" SortingSettings-SortedAscToolTip="Sorted Ascending"
                    AllowFilteringByColumn="True">
                    <GroupingSettings CaseSensitive="false" />
                    <MasterTableView AllowFilteringByColumn="true" AllowSorting="true" AllowNaturalSort="false">
                        <CommandItemSettings ExportToPdfText="Export to PDF" />
                        <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                        </ExpandCollapseColumn>
                        <NoRecordsTemplate>
                            No Members Available.
                        </NoRecordsTemplate>
                        <Columns>
                            <rad:GridTemplateColumn HeaderText="Remove" AllowFiltering="false">
                                <ItemStyle></ItemStyle>
                                <HeaderStyle/>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkAllPerson" runat="server" OnCheckedChanged="ToggleSelectedState"
                                        AutoPostBack="True" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkperson" runat="server" OnCheckedChanged="chkSelectChanged" AutoPostBack="True" />
                                </ItemTemplate>
                            </rad:GridTemplateColumn>
                            <rad:GridTemplateColumn HeaderText="Person ID" DataField="ID" SortExpression="ID"
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                <HeaderStyle />
                                <ItemStyle></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblPersonID" HeaderText="Person ID" runat="server" Text='
								<%# DataBinder.Eval(Container.DataItem,"ID") %>
									'>
                                    </asp:Label>
                                </ItemTemplate>
                            </rad:GridTemplateColumn>
                            <rad:GridTemplateColumn HeaderText="Member" DataField="FirstLast" SortExpression="FirstLast"
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                <ItemStyle></ItemStyle>
                                <HeaderStyle/>
                                <ItemTemplate>
                                    <div class="row-div clearfix">
                                        
                                            <rad:RadBinaryImage ID="imgmember" runat="server" CssClass="img-float" AutoAdjustImageControlSize="false"
                                                ResizeMode="Fill" />
                                        
                                            <asp:Label ID="lblMember" CssClass="name-link" runat="server" Text='
												<%# DataBinder.Eval(Container.DataItem,"FirstLast") %>
													'>
                                            </asp:Label>
                                            <br />
                                            <asp:Label ID="lblMemberTitle" runat="server" Text='
													<%# DataBinder.Eval(Container.DataItem,"title") %>
														'>
                                            </asp:Label>
                                            <br />
                                            <asp:Label ID="lbladdress" runat="server" Text='
														<%# DataBinder.Eval(Container.DataItem,"address") %>
															'>
                                            </asp:Label>                                        
                                    </div>
                                </ItemTemplate>
                            </rad:GridTemplateColumn>
                            <rad:GridTemplateColumn HeaderText="Email" ItemStyle-Wrap="true" DataField="Email"
                                SortExpression="Email" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                ShowFilterIcon="false">
                                <ItemStyle CssClass="email-style" Wrap="true"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblEmail" HeaderText="Email" runat="server" Text='
												<%# DataBinder.Eval(Container.DataItem,"Email") %>
													'>
                                    </asp:Label>
                                </ItemTemplate>
                                <HeaderStyle/>
                            </rad:GridTemplateColumn>
                            <rad:GridTemplateColumn HeaderTooltip="Apply this product for all members" HeaderText="Membership Type"
                                AllowFiltering="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false"
                                UniqueName="MemberType">
                                <ItemStyle></ItemStyle>
                                <FilterTemplate>
                                    <telerik:RadComboBox ID="RadddlHeaderMemberType" OnDataBound="RadddlHeaderMemberType_DataBound"
                                        AppendDataBoundItems="true" runat="server" OnSelectedIndexChanged="ddlHeaderMemberType_SelectedIndexChanged">
                                    </telerik:RadComboBox>
                                </FilterTemplate>
                                <HeaderTemplate>
                                    Select Product
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlMemberType" runat="server" OnSelectedIndexChanged="ddlMemberTypeChanged"
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </rad:GridTemplateColumn>
                            <rad:GridTemplateColumn HeaderText="Price" AllowFiltering="false" UniqueName="Price">
                                <ItemStyle></ItemStyle>
                                <HeaderStyle />
                                <ItemTemplate>
                                    <asp:Label ID="lblPrice" runat="server">
                                    </asp:Label>
                                </ItemTemplate>
                            </rad:GridTemplateColumn>
                            <rad:GridTemplateColumn HeaderText="Auto Renew" AllowFiltering="false">
                                <HeaderStyle />
                                <ItemStyle></ItemStyle>
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlAutoRenew" runat="server">
                                        <asp:ListItem Text="No">
                                        </asp:ListItem>
                                        <asp:ListItem Text="Yes">
                                        </asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </rad:GridTemplateColumn>
                        </Columns>
                        <EditFormSettings>
                            <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                            </EditColumn>
                        </EditFormSettings>
                    </MasterTableView>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </rad:RadGrid>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="row-div top-margin align-right">
        <asp:Button ID="btnPurchaseMemberships" CssClass="submit-Btn" runat="server" Text="Proceed to checkout" />
    </div>
</div>
<cc1:User ID="User1" runat="server"></cc1:User>
<cc4:AptifyShoppingCart ID="ShoppingCart1" runat="server" Visible="False"></cc4:AptifyShoppingCart>
