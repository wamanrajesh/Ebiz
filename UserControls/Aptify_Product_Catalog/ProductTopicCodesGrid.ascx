<%--Aptify e-Business 5.5.1, July 2013--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ProductTopicCodesGrid.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.ProductCatalog.ProductTopicCodesGrid" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<h6 runat="server" class="textfont">
    Product Keywords</h6>
<div class="content-container clearfix">
    <%--Navin Prasad Issue 11032--%>
    <%--Nalini Issue 12436 date:01/12/2011--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <%--Neha Changes for Issue 14456--%>
            <rad:RadGrid ID="grdMain" AutoGenerateColumns="False" runat="server" AllowPaging="true" AllowFilteringByColumn="true" SortingSettings-SortedDescToolTip="Sorted Descending" SortingSettings-SortedAscToolTip="Sorted Ascending">
              <GroupingSettings CaseSensitive="false" />
                <MasterTableView AllowFilteringByColumn="true" AllowSorting="true" AllowNaturalSort="false">
                    <Columns>
                        <rad:GridBoundColumn DataField="Name" HeaderText="Name" SortExpression="Name" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="250px" ItemStyle-Width="250px" FilterControlWidth="80%" />
                        <rad:GridBoundColumn DataField="Description" HeaderText="Description" SortExpression="" AutoPostBackOnFilter="true" HeaderStyle-Width="250px" ItemStyle-Width="250px" CurrentFilterFunction="Contains" ShowFilterIcon="false" AllowSorting="false" FilterControlWidth="80%" />
                        <rad:GridBoundColumn Visible="False" DataField="ID" />
                    </Columns>
                </MasterTableView>
            </rad:RadGrid>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
