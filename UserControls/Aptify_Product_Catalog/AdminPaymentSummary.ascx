<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="AdminPaymentSummary.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.ProductCatalog.AdminPaymentSummary" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessGlobal" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<div style="min-height: 25px">
    <asp:Label ID="lblshowmessage" Font-Bold="true" Text="Your payment was made successfully!"
        runat="server" ForeColor="blue"></asp:Label>
</div>
<%--Neha, issue 14456,03/15/13, added filtering for the all column--%>
<asp:UpdatePanel ID="UpdatePanelgrdOrderSummary" runat="server">
    <ContentTemplate>
        <rad:RadGrid ID="grdOrderSummary" runat="server" AutoGenerateColumns="False" AllowPaging="true"
            AllowSorting="true" PagerStyle-PageSizeLabelText="Records Per Page" SortingSettings-SortedDescToolTip="Sorted Descending"
            SortingSettings-SortedAscToolTip="Sorted Ascending" AllowFilteringByColumn="true">
            <mastertableview allowfilteringbycolumn="true" allowsorting="true" allownaturalsort="false">
                <Columns>
                    <rad:GridBoundColumn HeaderText="ID" DataField="ID" Visible="false" ItemStyle-CssClass="IdWidth" />
                    <rad:GridTemplateColumn HeaderText="Name" DataField="Name" SortExpression="Name"
                        CurrentFilterFunction="Contains" ShowFilterIcon="false" FilterControlWidth="80%"
                        AutoPostBackOnFilter="true">
                        <HeaderStyle Width="120px" />
                        <ItemStyle HorizontalAlign="Center" CssClass="LeftAlign" Width="120px"></ItemStyle>
                        <ItemTemplate>
                            <asp:Label ID="lblMemberName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Label>
                        </ItemTemplate>
                    </rad:GridTemplateColumn>
                    <rad:GridTemplateColumn HeaderText="Order #" DataField="ID" SortExpression="ID" CurrentFilterFunction="EqualTo"
                        ShowFilterIcon="false" FilterControlWidth="100%" AutoPostBackOnFilter="true">
                        <HeaderStyle Width="60px" />
                        <ItemStyle Width="60px" CssClass="LeftAlign" />
                        <ItemTemplate>
                            <asp:Label ID="lblOrderNo" runat="server" Target="_blank" Text='<%# DataBinder.Eval(Container.DataItem,"ID") %>'></asp:Label>
                        </ItemTemplate>
                    </rad:GridTemplateColumn>
                    <rad:GridTemplateColumn HeaderText="Total" HeaderStyle-CssClass="rightAlign" DataField="GrandTotal"
                        SortExpression="GrandTotal" CurrentFilterFunction="EqualTo" ShowFilterIcon="false"
                        FilterControlWidth="100%" AutoPostBackOnFilter="true">
                        <ItemStyle CssClass="rightAlign" Width="60px" />
                        <HeaderStyle Width="60px" />
                        <ItemTemplate>
                            <asp:Label ID="lblGrandTotal" runat="server" Text='<%#GetFormattedCurrency(Container, "GrandTotal")%>'></asp:Label>
                        </ItemTemplate>
                    </rad:GridTemplateColumn>
                    <rad:GridTemplateColumn HeaderText="Balance" HeaderStyle-CssClass="rightAlign" DataField="Balance"
                        SortExpression="Balance" CurrentFilterFunction="EqualTo" ShowFilterIcon="false"
                        FilterControlWidth="100%" AutoPostBackOnFilter="true">
                        <ItemStyle CssClass="rightAlign" Width="60px" />
                        <HeaderStyle Width="60px" />
                        <ItemTemplate>
                            <asp:Label ID="lblBalanceAmount" runat="server" Text='<%#GetFormattedCurrency(Container, "Balance")%>'></asp:Label>
                        </ItemTemplate>
                    </rad:GridTemplateColumn>
                    <rad:GridTemplateColumn HeaderText="Currency Symbol" Visible="false" DataField="CurrencySymbol">
                        <ItemStyle Width="10px" />
                        <HeaderStyle Width="10px" />
                        <ItemTemplate>
                            <asp:Label ID="lblCurrencySymbol" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"CurrencySymbol") %>'></asp:Label>
                        </ItemTemplate>
                    </rad:GridTemplateColumn>
                </Columns>
            </mastertableview>
        </rad:RadGrid>
    </ContentTemplate>
</asp:UpdatePanel>
<div>
    <br />
    <asp:Button CssClass="submitBtn" ID="cmdback" TabIndex="1" runat="server" Height="26px"
        Text="Back"></asp:Button>
</div>
<cc2:User runat="Server" ID="User1" />
