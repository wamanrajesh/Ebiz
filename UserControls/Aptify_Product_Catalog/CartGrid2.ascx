<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="CartGrid2.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.ProductCatalog.CartGrid2" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessShoppingCart" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%--Nalini Issue 12436 date:01/12/2011--%>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
         <div id="tblCart" runat="server">
                <div class="row-div">
                    <%--Navin Prasad Issue 11032--%>
                    <rad:RadGrid ID="grdMain" runat="server" AutoGenerateColumns="False">
                        <MasterTableView>
                            <Columns>
                                <rad:GridTemplateColumn Visible="False" HeaderText="Product ID">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProductID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ProductID") %>' />
                                    </ItemTemplate>
                                </rad:GridTemplateColumn>
                                <%-- Neha Changes for issue 14456--%>
                                <rad:GridTemplateColumn HeaderText="Auto Renew" UniqueName="AutoRenew">
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="left" />
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chkRenew" Enabled="false"></asp:CheckBox>
                                    </ItemTemplate>
                                </rad:GridTemplateColumn>
                                <rad:GridTemplateColumn HeaderText="Product">
                                    <ItemTemplate>
                                        <b>
                                            <asp:LinkButton runat="server" PostBackUrl="" CausesValidation="false" ID="link"
                                                Text='<%# DataBinder.Eval(Container, "DataItem.WebName") %>' CommandName="Link"></asp:LinkButton></b>
                                    </ItemTemplate>
                                </rad:GridTemplateColumn>
                                
                                <rad:GridBoundColumn DataField="Description" HeaderText="Description" AllowSorting="false" />
                                <rad:GridTemplateColumn HeaderText="Quantity">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQuantity" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Quantity") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Right" Width="60px"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </rad:GridTemplateColumn>
                                <rad:GridTemplateColumn HeaderText="Unit Price">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPrice" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Price") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </rad:GridTemplateColumn>
                                <rad:GridTemplateColumn HeaderText="Total Price">
                                    <ItemTemplate>
                                        <asp:Label ID="lblExtended" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Extended") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </rad:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </rad:RadGrid>
               </div>
                <div class="row-div clearfix">
                    <asp:Label ID="lblError" runat="server" Visible="False"></asp:Label>               
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<cc2:AptifyShoppingCart ID="ShoppingCart1" runat="server" Visible="false"></cc2:AptifyShoppingCart>
