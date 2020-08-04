<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="OrderHistory_Admin.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.CustomerService.OrderHistory_Admin" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<div class="table-div" id="divTop" runat="server">
    <asp:UpdatePanel ID="updPanelGrid" runat="server">
        <ContentTemplate>
            <telerik:RadGrid ID="grdMain" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                AllowMultiRowSelection="False" AllowPaging="True" GridLines="None" SortingSettings-SortedDescToolTip="Sorted Descending"
                SortingSettings-SortedAscToolTip="Sorted Ascending" AllowFilteringByColumn="true">
                <MasterTableView DataKeyNames="ID" AllowMultiColumnSorting="false" AllowNaturalSort="false"
                    AllowFilteringByColumn="true">
                    <DetailTables>
                        <telerik:GridTableView DataKeyNames="ID" Name="ChildGrid" runat="server" AllowNaturalSort="false"
                            SortingSettings-SortedDescToolTip="Sorted Descending" AllowFilteringByColumn="false"
                            NoDetailRecordsText="Nothing to display" SortingSettings-SortedAscToolTip="Sorted Ascending"
                            AllowPaging="false">
                            <Columns>
                                <rad:GridTemplateColumn HeaderText="Product" SortExpression="Product" DataField="Product">
                                    <ItemTemplate>
                                        <asp:Label ID="Product" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Product") %>'></asp:Label>
                                    </ItemTemplate>
                                </rad:GridTemplateColumn>
                                <rad:GridBoundColumn DataField="ProductType" HeaderText="Type" SortExpression="ProductType" />
                                <rad:GridBoundColumn DataField="Description" HeaderText="Description" />
                                <rad:GridBoundColumn DataField="Quantity" HeaderText="Quantity" />
                                <rad:GridTemplateColumn HeaderText="Price" SortExpression="Price" DataField="Price"
                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDetailPrice" runat="server" Text='<%#GetFormattedCurrency(Container,"Price") %>'></asp:Label>
                                    </ItemTemplate>
                                </rad:GridTemplateColumn>
                                <rad:GridTemplateColumn HeaderText="Discount" SortExpression="Discount" DataField="Discount"
                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDetailPrice" runat="server" Text='<%#GetFormattedCurrency(Container,"Discount") %>'></asp:Label>
                                    </ItemTemplate>
                                </rad:GridTemplateColumn>
                            </Columns>
                        </telerik:GridTableView>
                    </DetailTables>
                    <Columns>
                        <rad:GridTemplateColumn HeaderText="Order#" HeaderButtonType="TextButton" SortExpression="ID"
                            DataField="ID" UniqueName="ID" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                            ShowFilterIcon="false">
                            <ItemTemplate>
                                <asp:HyperLink ID="Product" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ID") %>'
                                    NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"LinkUrl") %>'></asp:HyperLink>
                            </ItemTemplate>
                        </rad:GridTemplateColumn>
                        <rad:GridDateTimeColumn DataField="OrderDate" UniqueName="GridDateTimeColumnOrderDate"
                            HeaderText="Date" SortExpression="OrderDate" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                            DataType="System.DateTime" ShowFilterIcon="false" EnableTimeIndependentFiltering="true" />
                        <rad:GridBoundColumn DataField="BillToName" HeaderText="Bill to Person" AutoPostBackOnFilter="true"
                            CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                        <rad:GridBoundColumn DataField="ShipToName" HeaderText="Ship to Person" AutoPostBackOnFilter="true"
                            CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                        <rad:GridBoundColumn Visible="false" DataField="CurrencyType" HeaderText="Currency"
                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                        <rad:GridBoundColumn DataField="OrderStatus" HeaderText="Shipping Status" AutoPostBackOnFilter="true"
                            CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                        <rad:GridBoundColumn DataField="ShipType" HeaderText="Shipment Method" AutoPostBackOnFilter="true"
                            CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                        <rad:GridBoundColumn DataField="PayType" HeaderText="Payment Method" AutoPostBackOnFilter="true"
                            CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                        <rad:GridTemplateColumn HeaderText="Total" SortExpression="CALC_GrandTotal" DataField="CALC_GrandTotal"
                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                            <ItemTemplate>
                                <asp:Label ID="lblCALC_GrandTotal" runat="server" Text='<%#GetFormattedCurrency(Container,"CALC_GrandTotal") %>'></asp:Label>
                            </ItemTemplate>
                        </rad:GridTemplateColumn>
                        <rad:GridTemplateColumn HeaderText="Balance" SortExpression="balance" DataField="balance"
                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                            <ItemTemplate>
                                <asp:Label ID="lblbalance" runat="server" Text='<%#GetFormattedCurrency(Container,"balance") %>'></asp:Label>
                            </ItemTemplate>
                        </rad:GridTemplateColumn>
                        <rad:GridBoundColumn DataField="OrderParty" HeaderText="Order Party" SortExpression="OrderParty"
                            FilterControlWidth="80%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                            ShowFilterIcon="false" />
                        <rad:GridBoundColumn DataField="PaymentParty" HeaderText="Payment Party" SortExpression="PaymentParty"
                            FilterControlWidth="80%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                            ShowFilterIcon="false" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<cc1:User runat="server" ID="User1" />
