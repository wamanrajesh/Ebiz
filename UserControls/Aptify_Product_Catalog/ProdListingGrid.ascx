<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ProdListingGrid.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.ProductCatalog.ProdListingGrid" %>
<%@ Register TagPrefix="uc1" TagName="FeaturedProduct" Src="FeaturedProducts.ascx" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessShoppingCart" %>
<%@ Register TagPrefix="uc1" TagName="ProdCategoryBar" Src="~/UserControls/Aptify_General/ProdCategoryBar.ascx" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<div class="top-margin"><h4 runat="server" id="lblHeader" class="label" visible="false">
</h4>
</div>
<div class="table-div" id="divMain" runat="server">
    <div class="row-div clearfix">
        <div id="ProdNavbar" runat="server" class="left-container w18">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                   
                        <h6 runat="server" id="lblProdCatHeader" class="browse-product">
                        </h6>
                        <div>
                            <uc1:ProdCategoryBar ID="ProdCategoryBar" runat="server" />
                        </div>
                   
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="right-container w80">
            <asp:UpdatePanel ID="UpdatePanelgrdMain" runat="server">
                <ContentTemplate>
                    <rad:RadGrid ID="grdMain" runat="server" AutoGenerateColumns="False" ShowHeader="False"
                        ShowFooter="false" EnableTheming="true" EmptyDataText="No Products to display"
                        AllowPaging="true">
                        <MasterTableView>
                            <Columns>
                                <rad:GridTemplateColumn >
                                    <ItemTemplate>
                                        <asp:Image ID="ImgProd" runat="server"/>
                                    </ItemTemplate>
                                </rad:GridTemplateColumn>
                                <rad:GridTemplateColumn>
                                    <ItemTemplate>
                                       
                                            <asp:HyperLink ID="lnkProduct" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"WebName") %>'
                                                Font-Bold="true"></asp:HyperLink><br />
                                        
                                        <asp:Label ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"WebDescription") %>'></asp:Label>
                                        
                                    </ItemTemplate>
                                </rad:GridTemplateColumn>
                                <rad:GridTemplateColumn>
                                    <ItemTemplate>
                                        
                                             <div class="row-div clearfix">
                                                <div class="label-div">
                                                    <asp:Label ID="lblPriceForYou" runat="server" Text="Price:" ></asp:Label>
                                                </div>
                                                <div class="field-div1 padding-left-right">
                                                    <asp:Label ID="lblPriceForYouVal" runat="server" ></asp:Label>
                                                </div>
                                            </div>
                                        <div class="row-div clearfix">
                                                <div class="label-div">
                                                    <asp:Label ID="lblItemCode" runat="server" Text="Item Code:"  Visible="false"></asp:Label>
                                                </div>
                                                <div class="field-div1 padding-left-right">
                                                    <asp:Label ID="lblItemCodeVal" runat="server" Visible="false"></asp:Label>
                                                </div>
                                            </div>
                                       
                                    </ItemTemplate>
                                </rad:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </rad:RadGrid>
                </ContentTemplate>
            </asp:UpdatePanel>
            <uc1:FeaturedProduct ID="FeaturedProducts" HeaderText="{Featured Product}" ShowHeaderIfEmpty="False"
                runat="server" />
        </div>
    </div>
</div>
<br />
<cc2:AptifyShoppingCart ID="ShoppingCart1" runat="server" Visible="False"></cc2:AptifyShoppingCart>
