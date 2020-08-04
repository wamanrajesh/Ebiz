<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="OpenCart.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.CustomerService.OpenCartControl" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessShoppingCart" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<div class="table-div" id="tblMain" runat="server">
   
        <asp:UpdatePanel ID="updPanelGrid" runat="server">
            <ContentTemplate>
                <rad:RadGrid ID="grdSavedCarts" runat="server" AutoGenerateColumns="False" AllowPaging="true"
                    SortingSettings-SortedDescToolTip="Sorted Descending" SortingSettings-SortedAscToolTip="Sorted Ascending"
                    AllowFilteringByColumn="true">
                    <GroupingSettings CaseSensitive="false" />
                    <MasterTableView AllowSorting="true" AllowFilteringByColumn="true" AllowNaturalSort="false">
                        <NoRecordsTemplate>
                            No Cart Available.
                        </NoRecordsTemplate>
                        <Columns>
                            <rad:GridTemplateColumn HeaderText="Name" DataField="Name" SortExpression="Name"
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                <ItemTemplate>
                                    <asp:HyperLink ID="lnkName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'
                                        NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"ShoppingCartUrl") %>'></asp:HyperLink>
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" />
                                <ItemStyle Wrap="True" />
                            </rad:GridTemplateColumn>
                            <rad:GridTemplateColumn HeaderText="Description" DataField="Description" SortExpression=""
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblDescription" Text='<%# DataBinder.Eval(Container.DataItem,"Description") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                                <ItemStyle Wrap="True" />
                            </rad:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </rad:RadGrid>
            </ContentTemplate>
        </asp:UpdatePanel>
 
</div>
<cc1:User runat="server" ID="User1" />
<cc3:AptifyShoppingCart runat="server" ID="ShoppingCart1" />
