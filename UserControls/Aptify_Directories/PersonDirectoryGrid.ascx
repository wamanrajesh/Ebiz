<%--Aptify e-Business 5.5.1, July 2013--%>
<%@ Control Language="vb" AutoEventWireup="false" CodeFile="PersonDirectoryGrid.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.Directories.PersonDirectoryGrid" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessGlobal" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:UpdatePanel ID="updPanelGrid" runat="server">
    <ContentTemplate>
        <rad:RadGrid ID="grdPerson" AutoGenerateColumns="False" runat="server" AllowPaging="true"
            SortingSettings-SortedDescToolTip="Sorted Descending" SortingSettings-SortedAscToolTip="Sorted Ascending"
            AllowFilteringByColumn="true">
            <GroupingSettings CaseSensitive="false" />
            <MasterTableView AllowSorting="true" AllowNaturalSort="false">
                <Columns>
                    <rad:GridTemplateColumn HeaderText="Member" DataField="FirstLast" SortExpression="FirstLast" FilterControlWidth="80%"
                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                        <ItemTemplate>
                            <asp:HyperLink ID="lnkMember" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"FirstLast") %>'
                                NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"FirstLastUrl") %>'></asp:HyperLink>
                        </ItemTemplate>                      
                    </rad:GridTemplateColumn>                 
                    <rad:GridBoundColumn DataField="Company" HeaderText="Company" SortExpression="Company"
                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"
                        />
                    <rad:GridBoundColumn DataField="AddressLine1" HeaderText="Address" SortExpression="AddressLine1"
                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"
                        />
                    <rad:GridBoundColumn DataField="City" HeaderText="City" SortExpression="City" AutoPostBackOnFilter="true"
                        CurrentFilterFunction="Contains" ShowFilterIcon="false" FilterControlWidth="80%" />
                    <rad:GridBoundColumn DataField="State" HeaderText="State" SortExpression="State"
                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"
                        />
                    <rad:GridBoundColumn DataField="ZipCode" HeaderText="Zip Code" SortExpression="ZipCode"
                        AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false"
                         />
                    <rad:GridTemplateColumn HeaderText="Email" DataField="Email1" AutoPostBackOnFilter="true"
                        SortExpression="Email1" CurrentFilterFunction="Contains" ShowFilterIcon="false"
                        >
                        <ItemTemplate>
                            <asp:HyperLink ID="lnkMail" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Email1") %>'
                                NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"DataNavigateUrl") %>'></asp:HyperLink>
                        </ItemTemplate>
                    </rad:GridTemplateColumn>
                </Columns>
            </MasterTableView>
        </rad:RadGrid>
    </ContentTemplate>
</asp:UpdatePanel>
<div class="errormsg-div">
<asp:Label ID="lblNoResults" runat="server" Visible="false">No results are available.</asp:Label></div>
