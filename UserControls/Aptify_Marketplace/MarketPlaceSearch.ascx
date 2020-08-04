<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="MarketPlaceSearch.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.MarketPlace.Search"
    Debug="true" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="WebUserActivity" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<div id="tblDisplay" runat="server" class="marketplace-main-div top-margin">
    <div id="trSearch" runat="server">
        <div class="row-div clearfix">
            <div class="label-div w19">
                Listing Name Contains:
            </div>
            <div class="field-div1 w79">
                <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="row-div clearfix">
            <div class="label-div w19">
                Vendor Name Contains:
            </div>
            <div class="field-div1 w79">
                <asp:TextBox ID="txtVendor" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="row-div clearfix">
            <div class="label-div w19">
                Description Contains:
            </div>
            <div class="field-div1 w79">
                <asp:TextBox ID="txtDescription" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="row-div clearfix">
            <div class="label-div w19">
               Posted In The Last: 
            </div>
            <div class="field-div1 w79">
                <asp:DropDownList ID="cmbRecency" runat="server">
                    <asp:ListItem Value="1">1 Day</asp:ListItem>
                    <asp:ListItem Value="2">2 Days</asp:ListItem>
                    <asp:ListItem Value="3">3 Days</asp:ListItem>
                    <asp:ListItem Value="5">5 Days</asp:ListItem>
                    <asp:ListItem Value="7">1 Week</asp:ListItem>
                    <asp:ListItem Value="14">2 Weeks</asp:ListItem>
                    <asp:ListItem Value="30">1 Month</asp:ListItem>
                    <asp:ListItem Value="60">2 Months</asp:ListItem>
                    <asp:ListItem Value="90">3 Months</asp:ListItem>
                    <asp:ListItem Value="180">6 Months</asp:ListItem>
                    <asp:ListItem Value="365">1 Year</asp:ListItem>
                    <asp:ListItem Value="730">2 Years</asp:ListItem>
                    <asp:ListItem Value="1095">3 Years</asp:ListItem>
                    <asp:ListItem Value="1460">4 Years</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="row-div clearfix">
            <div class="label-div w19">
                &nbsp;</div>
            <div class="field-div1 w79">
                <asp:Button CssClass="submit-Btn" ID="cmdSearch" Text="Search" runat="server"></asp:Button>
            </div>
        </div>
    </div>
    <div id="trResults" runat="server" class="row-div">
        <asp:UpdatePanel ID="UppanelGrid" runat="server">
            <ContentTemplate>
                <rad:radgrid id="grdListings" runat="server" allowpaging="true" autogeneratecolumns="False"
                    allowfilteringbycolumn="True" sortingsettings-sorteddesctooltip="Sorted Descending"
                    sortingsettings-sortedasctooltip="Sorted Ascending" allowsorting="true">
                        <GroupingSettings CaseSensitive="false" />
                        <MasterTableView AllowSorting="true" AllowFilteringByColumn="true" AllowNaturalSort="false">
                            <Columns>
                                <rad:GridTemplateColumn HeaderText="Vendor" DataField="Company" SortExpression="Company"
                                    CurrentFilterFunction="Contains" ShowFilterIcon="false" AutoPostBackOnFilter="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lnkVendorURL" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Company") %>'></asp:Label>
                                    </ItemTemplate>
                                </rad:GridTemplateColumn>
                                <rad:GridTemplateColumn HeaderText="Listing" DataField="Name" SortExpression="Name"
                                    CurrentFilterFunction="Contains" ShowFilterIcon="false" AutoPostBackOnFilter="true">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="lnkName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'
                                            NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"DataNavigateUrl") %>'></asp:HyperLink>
                                    </ItemTemplate>
                                </rad:GridTemplateColumn>
                                <rad:GridBoundColumn DataField="PlainTextDescription" HeaderText="Description" SortExpression=" "
                                    AllowSorting="false" HeaderTooltip="" CurrentFilterFunction="Contains" ShowFilterIcon="false"
                                    AutoPostBackOnFilter="true" />
                            </Columns>
                        </MasterTableView>
                    </rad:radgrid>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="row-div clearfix">
    <div class="label-div w19">
                &nbsp;</div>
    <div class="errormsg-div w79 ">
        <asp:Label ID="lblNoResults" runat="server" Visible="False">No Records match the search criteria.</asp:Label>
        <asp:Label ID="lblError" runat="server"></asp:Label></div>
    </div>
    <cc1:webuseractivity id="WebUserActivity1" runat="server" webmodule="MarketPlace" />
</div>
