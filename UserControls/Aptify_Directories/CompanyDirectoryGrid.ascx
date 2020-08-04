<%--Aptify e-Business 5.5.1, July 2013--%>
<%@ Control Language="vb" AutoEventWireup="false" CodeFile="CompanyDirectoryGrid.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.CompanyDirectoryGrid" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessGlobal" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:UpdatePanel ID="updPanelGrid" runat="server">
    <ContentTemplate>
        <rad:RadGrid ID="grdCompany" runat="server" AutoGenerateColumns="False" AllowPaging="true"
            SortingSettings-SortedDescToolTip="Sorted Descending" SortingSettings-SortedAscToolTip="Sorted Ascending"
            AllowFilteringByColumn="true">
            <GroupingSettings CaseSensitive="false" />
            <MasterTableView AllowSorting="true" AllowNaturalSort="false">
                <Columns>
                    <rad:GridTemplateColumn HeaderText="Company" DataField="Name" SortExpression="Name"
                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                        ShowFilterIcon="false">
                        <ItemTemplate>
                            <asp:HyperLink ID="lnkMember" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'
                                NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"CompanyUrl") %>'></asp:HyperLink>
                        </ItemTemplate>
                    </rad:GridTemplateColumn>
                    <rad:GridBoundColumn DataField="AddressLine1" HeaderText="Address" SortExpression="AddressLine1"
                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                    <rad:GridBoundColumn DataField="City" HeaderText="City" SortExpression="City" AutoPostBackOnFilter="true"
                        CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                    <rad:GridBoundColumn DataField="State" HeaderText="State" SortExpression="State"
                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                    <rad:GridBoundColumn DataField="ZipCode" HeaderText="Zip Code" SortExpression="ZipCode"
                        AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false" />
                    <rad:GridTemplateColumn HeaderText="Web Site" DataField="WebSite" SortExpression="WebSite"
                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                        ShowFilterIcon="false">
                        <ItemTemplate>
                            <asp:HyperLink ID="lnkMember" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"WebSite") %>'
                                Target="_blank" NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"WebSite") %>'></asp:HyperLink>
                        </ItemTemplate>
                    </rad:GridTemplateColumn>
                </Columns>
            </MasterTableView>
        </rad:RadGrid>
    </ContentTemplate>
</asp:UpdatePanel>

<div class="errormsg-div">
<asp:Label ID="lblNoResults" runat="server" Visible="True">No results are available.</asp:Label></div>

