<%--Aptify e-Business 5.5.1, July 2013--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Default.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.MarketPlace._Default" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="WebUserActivity" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<div id="tblMain" runat="server" class="marketplace-main-div">
    <div class="row-div">
        <p>
            The MarketPlace is an interactive community that will allow you to access information
            about products and services available from the leaders in the industry. We work
            with a large number of industry leaders to provide this MarketPlace to simplify
            your search for vendors of the products and services that you need.
        </p>
        <hr />
        <asp:Button CssClass="submit-Btn" ID="cmdNewLisitng" runat="server" Text="Create New Listing">
        </asp:Button>
    </div>
    <div class="row-div label" id="tblDisplay" runat="server">
    Browse By:
    </div>
    <div class="row-div1">
        <asp:HyperLink ID="lnkBrowseAll" runat="server">All</asp:HyperLink><br />
        <asp:HyperLink ID="lnkBrowseVendor" runat="server">Vendor</asp:HyperLink><br />
        <asp:HyperLink ID="lnkBrowseListing" runat="server">Listing</asp:HyperLink><br />
    </div>
    <div class="row-div1 top-margin clearfix">
        <div class="float-left label w29"><asp:Label ID="lblDisplayTitle" runat="server">Browse MarketPlace Listings</asp:Label></div>
        <div class="field-div1 w70"><asp:ListBox ID="lstBrowse" runat="server" Rows="1" AutoPostBack="True"></asp:ListBox></div>
    </div>
    <div class="errormsg-div" >
        <asp:Label ID="lblNoResults" runat="server" Visible="False">No Records match the search criteria.</asp:Label></div>
        <div class="row-div">
        <asp:UpdatePanel ID="UppanelGrid" runat="server">
            <ContentTemplate>
                <rad:radgrid id="grdListings" runat="server" autogeneratecolumns="False" allowpaging="true"
                    allowfilteringbycolumn="true" allowsorting="true" sortingsettings-sorteddesctooltip="Sorted Descending"
                    sortingsettings-sortedasctooltip="Sorted Ascending">
                        <GroupingSettings CaseSensitive="false" />
                        <MasterTableView AllowFilteringByColumn="true" AllowSorting="true" AllowNaturalSort="false">
                            <Columns>
                                <rad:GridBoundColumn DataField="Vendor" HeaderText="Vendor" SortExpression="Vendor"
                                    ShowFilterIcon="false" CurrentFilterFunction="Contains"
                                    AutoPostBackOnFilter="true" />
                                <rad:GridTemplateColumn HeaderText="Listing" DataField="Listing" SortExpression="Listing"
                                    CurrentFilterFunction="Contains" ShowFilterIcon="false" AutoPostBackOnFilter="true">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="lnkName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Listing") %>'
                                            NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"ListingURL") %>'></asp:HyperLink>
                                    </ItemTemplate>
                                </rad:GridTemplateColumn>
                                <rad:GridBoundColumn DataField="Description" HeaderText="Description" SortExpression=" "
                                    AllowSorting="false" HeaderTooltip="" ShowFilterIcon="false"
                                    CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" />
                            </Columns>
                        </MasterTableView>
                    </rad:radgrid>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <cc1:webuseractivity id="WebUserActivity1" runat="server" webmodule="MarketPlace" />
</div>
