<%--Aptify e-Business 5.5.1, July 2013--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ProductGroupingContentsGrid.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.ProductCatalog.ProductGroupingContentsGrid" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Label ID="lblTitle" runat="server" Text="ProductNamePlaceHolder's Contents"></asp:Label><br />
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <%--Neha Changes for Issue 14456--%>
        <rad:RadGrid ID="grdMain" AutoGenerateColumns="False" runat="server" AllowSorting="True"
            AllowPaging="true" DataKeyNames="ID" AllowFilteringByColumn="true">
            <GroupingSettings CaseSensitive="false" />
            <MasterTableView AllowFilteringByColumn="true" AllowNaturalSort="false">
                <Columns>
                    <rad:GridTemplateColumn HeaderText="Product" AutoPostBackOnFilter="true" DataField="WebName"
                        CurrentFilterFunction="Contains" ShowFilterIcon="false" SortExpression="WebName"
                        FilterControlWidth="80%">
                        <ItemTemplate>
                            <asp:HyperLink ID="lnkWebName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"WebName") %>'
                                NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"WebNameUrl") %>'></asp:HyperLink>
                        </ItemTemplate>
                        <HeaderStyle Width="35%" />
                    </rad:GridTemplateColumn>
                  <rad:GridTemplateColumn HeaderText="Description" AutoPostBackOnFilter="true" DataField="WebDescription"
                        CurrentFilterFunction="Contains" ShowFilterIcon="false" SortExpression="" FilterControlWidth="80%">
                        <ItemTemplate>
                            <asp:Label ID="lblWebDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"WebDescription") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Width="45%" />
                    </rad:GridTemplateColumn>
                    <rad:GridTemplateColumn HeaderText="Quantity" AllowFiltering="false">
                        <ItemTemplate>
                            <asp:Label ID="lblQuantity" runat="server" Text='<%# Eval("Quantity","{0:n0}" ) %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" HorizontalAlign="Right" />
                        <FooterStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" HorizontalAlign="Right" />
                        <HeaderStyle Font-Bold="true" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" Width="10%" />
                    </rad:GridTemplateColumn>
                    <rad:GridTemplateColumn HeaderText="Price" AllowFiltering="false">
                        <ItemTemplate>
                            <asp:Label ID="lblPrice" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"PriceUrl") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" HorizontalAlign="Right" />
                        <HeaderStyle HorizontalAlign="Right" />
                        <FooterStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" HorizontalAlign="Right" />
                        <HeaderStyle Font-Bold="true" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" Width="10%" />
                    </rad:GridTemplateColumn>
                    <rad:GridTemplateColumn HeaderText="ProductID" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblProductID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ProductID") %>'></asp:Label>
                        </ItemTemplate>
                    </rad:GridTemplateColumn>
                </Columns>
            </MasterTableView>
        </rad:RadGrid>
    </ContentTemplate>
</asp:UpdatePanel>
