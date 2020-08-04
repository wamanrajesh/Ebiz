<%--Aptify e-Business 5.5.1, July 2013--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="OrderHistory.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.CustomerService.OrderHistoryControl" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<div class="content-container clearfix" id="divTop" runat="server">
    <p>
        <asp:Label ID="Label1" runat="server"><b>Order Type</b></asp:Label>
        <asp:DropDownList ID="cmbOrderType" runat="server" AutoPostBack="True">
            <asp:ListItem Value="-1">All</asp:ListItem>
            <asp:ListItem Value="1">Regular</asp:ListItem>
            <asp:ListItem Value="4">Quotations</asp:ListItem>
            <asp:ListItem Value="3">Cancellations</asp:ListItem>
        </asp:DropDownList>
    </p>
    <p>
        <%--Navin Prasad Issue 11032--%>
        <%--Update Panel added by Suvarna D IssueID: 12436 on Dec 1, 2011 --%>
        <asp:UpdatePanel ID="updPanelGrid" runat="server">
            <ContentTemplate>
                <%--Suraj issue 14450 2/7/13  removed three step sorting ,added tooltip and added date column--%>
                <rad:RadGrid ID="grdMain" runat="server" AutoGenerateColumns="False" AllowPaging="true"
                    SortingSettings-SortedDescToolTip="Sorted Descending" SortingSettings-SortedAscToolTip="Sorted Ascending"
                    AllowFilteringByColumn="true">
                    <GroupingSettings CaseSensitive="false" />
                    <MasterTableView AllowFilteringByColumn="true" AllowSorting="true" AllowNaturalSort="false">
               <%-- 'Suraj Issue 15287 4/9/13, if the grid dont have any record then grid should visible and it should show "No recors " msg--%>
                        <NoRecordsTemplate>
                            No Order History Available.
                        </NoRecordsTemplate>
                        <Columns>
                            <rad:GridHyperLinkColumn Text="ID" DataNavigateUrlFields="ID" DataTextField="ID"
                                FilterControlWidth="80%" HeaderText="Order #" SortExpression="ID" AutoPostBackOnFilter="true"
                                CurrentFilterFunction="EqualTo" ShowFilterIcon="false" />
                            <rad:GridDateTimeColumn DataField="OrderDate" UniqueName="GridDateTimeColumnOrderDate"
                                HeaderText="Date" FilterControlWidth="170px" HeaderStyle-Width="170px" SortExpression="OrderDate"
                                AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" DataType="System.DateTime"
                                ShowFilterIcon="false" EnableTimeIndependentFiltering="true" FilterListOptions="VaryByDataType" />
                            <rad:GridBoundColumn DataField="CurrencyType" HeaderText="Currency" SortExpression="CurrencyType"
                                FilterControlWidth="80%" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                                ShowFilterIcon="false" />
                            <rad:GridTemplateColumn HeaderText="Total" DataField="CALC_GrandTotal" SortExpression="CALC_GrandTotal"
                                FilterControlWidth="80%" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                                ShowFilterIcon="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblCALC_GrandTotal" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"CALC_GrandTotal") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                                <HeaderStyle HorizontalAlign="Right" Width="60px" />
                            </rad:GridTemplateColumn>
                            <rad:GridBoundColumn DataField="OrderStatus" HeaderText="Shipping Status" SortExpression="OrderStatus"
                                FilterControlWidth="80%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                ShowFilterIcon="false" />
                            <rad:GridBoundColumn DataField="ShipType" HeaderText="Shipping Type" SortExpression="ShipType"
                                FilterControlWidth="80%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                ShowFilterIcon="false" />
                            <rad:GridBoundColumn DataField="ShipTrackingNum" HeaderText="Tracking #" SortExpression="ShipTrackingNum"
                                FilterControlWidth="80%" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                                ShowFilterIcon="false" />
                           <rad:GridBoundColumn DataField="OrderParty" HeaderText="Order Party" SortExpression="OrderParty"
                                FilterControlWidth="80%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                ShowFilterIcon="false" />
                                <rad:GridBoundColumn DataField="PaymentParty" HeaderText="Payment Party" SortExpression="PaymentParty"
                                FilterControlWidth="80%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                ShowFilterIcon="false" />
                        </Columns>
                    </MasterTableView>
                </rad:RadGrid>
            </ContentTemplate>
        </asp:UpdatePanel>
    </p>
    <cc1:User runat="server" ID="User1" />
</div>
