<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Renewals.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.CustomerService.RenewalsControl" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessShoppingCart" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<div id="tblMain" runat="server" class="table-div">
    <div class="row-div">
        Use this page to renew your subscriptions and memberships
    </div>
    <div class="row-div">
        <asp:Label ID="lblSelections" runat="server" Visible="False">
        </asp:Label>
    </div>
    <div class="row-div">
        <asp:UpdatePanel ID="updPanelGrid" runat="server">
            <ContentTemplate>
                <rad:radgrid id="grdMain" runat="server" autogeneratecolumns="False" allowpaging="true"
                    sortingsettings-sorteddesctooltip="Sorted Descending" sortingsettings-sortedasctooltip="Sorted Ascending"
                    allowfilteringbycolumn="true">
                        <GroupingSettings CaseSensitive="false" />
                        <MasterTableView AllowSorting="true" AllowFilteringByColumn="true" AllowNaturalSort="false">
                            <NoRecordsTemplate>
                                No Renew Memberships and Subscriptions Available.
                            </NoRecordsTemplate>
                            <Columns>
                                <rad:GridTemplateColumn HeaderText="Renewal" AllowFiltering="false" >
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkRenewal" runat="server" />
                                        <asp:Label ID="lblSubscriptionID" Visible="false" runat="server" Text='
										<%# Eval("ID") %>
											'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </rad:GridTemplateColumn>
                                <rad:GridTemplateColumn HeaderText="ProductID" Visible="False" SortExpression="ProductID"
                                     AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                                    ShowFilterIcon="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProductID" Text='
											<%# DataBinder.Eval(Container.DataItem,"ProductID") %>
												' runat="server" />
                                    </ItemTemplate>
                                </rad:GridTemplateColumn>
                                <rad:GridTemplateColumn HeaderText="PurchaseType" Visible="False" SortExpression=" "
                                     AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                    ShowFilterIcon="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPurchaseType" Text='
												<%# DataBinder.Eval(Container.DataItem,"PurchaseType") %>
													' runat="server" />
                                    </ItemTemplate>
                                </rad:GridTemplateColumn>
                                <rad:GridTemplateColumn HeaderText="Product" Visible="False" SortExpression="Product"
                                     AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                    ShowFilterIcon="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProductName" Text='
													<%# DataBinder.Eval(Container.DataItem,"Product") %>
														' runat="server" />
                                    </ItemTemplate>
                                </rad:GridTemplateColumn>
                                <rad:GridBoundColumn HeaderText="ProductID" Visible="False" SortExpression="ProductID"
                                    />
                                <rad:GridBoundColumn DataField="PurchaseType" HeaderText="Purchase Type" SortExpression="PurchaseType"
                                     AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                    ShowFilterIcon="false" />
                                <rad:GridBoundColumn DataField="Product" HeaderText="Product" SortExpression="Product"
                                     AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                    ShowFilterIcon="false" />
                                <rad:GridDateTimeColumn DataField="EndDate" UniqueName="GridDateTimeColumnEndDate"
                                    HeaderText="Paid Through" 
                                    SortExpression="EndDate" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                                    DataType="System.DateTime" ShowFilterIcon="false" EnableTimeIndependentFiltering="true" />
                            </Columns>
                        </MasterTableView>
                    </rad:radgrid>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="row-div top-margin">
        <asp:Button CssClass="submit-Btn" ID="RenewButton" runat="server" Text="Renew Selected Items">
        </asp:Button>
        <asp:Button CssClass="submit-Btn" ID="CancelButton" runat="server" Text="Cancel">
        </asp:Button>
    </div>
    <div class="row-div">
        <asp:Label ID="lblAdded" runat="server" Visible="False">
        </asp:Label>
    </div>
</div>
<cc1:user id="User1" runat="server" />
<cc3:aptifyshoppingcart id="ShoppingCart1" runat="Server" />
