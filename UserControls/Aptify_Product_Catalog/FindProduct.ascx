<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="FindProduct.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.ProductCatalog.FindProductControl" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

    <%--Nalini Issue 12436 date:01/12/2011--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlmain" runat="server" DefaultButton="cmdSearch">
          
                <div class="table-div">
	                <div class="row-div1 clearfix">
		                <div class="field-div1 w32" >
			                <asp:TextBox ID="txtName" CssClass="w80" runat="server" />
			                <asp:TextBox ID="txtDescription" CssClass="w80" runat="server" Visible="false" />
		                </div>
		                <div class="field-div1 w32" >
			                <asp:DropDownList runat="server" CssClass="w80" ID="cmbCategory" DataTextField="WebName" DataValueField="ID" >
			                </asp:DropDownList>
		                </div>
		                <div class="field-div1 w32" >
			                <asp:Button CssClass="submitBtn" ID="cmdSearch" runat="server" Text="Find Products">
			                </asp:Button>
		                </div>
	                </div>
	                <div class="row-div clearfix">
		                <asp:Label ID="lblError" runat="server" Visible="False">
			            </asp:Label>
		            </div>
	                <div class="clearfix" id="trNoResults" runat="server">
		                 The system could not locate any matching records. Please try again.
		            </div>
                </div>
            </asp:Panel>
            <div id="trResults" runat="server">
                <%--Neha Changes for Issue 14456--%>
                <rad:RadGrid ID="grdResults" runat="server" AutoGenerateColumns="False" AllowPaging="true" AllowFilteringByColumn="true" SortingSettings-SortedDescToolTip="Sorted Descending" SortingSettings-SortedAscToolTip="Sorted Ascending">
                  <GroupingSettings CaseSensitive="false" />
                    <GroupingSettings CaseSensitive="false" />
                    <MasterTableView AllowFilteringByColumn="true" AllowSorting="true" AllowNaturalSort="false">
                       <Columns>
                            <rad:GridTemplateColumn HeaderText="Product" DataField="WebName" FilterListOptions="VaryByDataType"  SortExpression="WebName" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"  ShowFilterIcon="false" FilterControlWidth="80%">
                              <ItemTemplate> 
                                  <asp:HyperLink ID="lnkWebName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"WebName") %>'
                                        NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"ViewProductPageUrl") %>'></asp:HyperLink>
                              </ItemTemplate>
                                <ItemStyle CssClass="w30" />
                            </rad:GridTemplateColumn>
                            <rad:GridTemplateColumn HeaderText="Description" DataField="WebDescription" FilterListOptions="VaryByDataType" SortExpression="" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"  ShowFilterIcon="false" FilterControlWidth="80%">
                              <ItemTemplate>
                                    <asp:Literal ID="ltDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"WebDescription") %>'></asp:Literal>
                                </ItemTemplate>
                                <ItemStyle CssClass="w40" />
                            </rad:GridTemplateColumn>
                            <rad:GridTemplateColumn HeaderText="Category" DataField="Category" FilterListOptions="VaryByDataType"  SortExpression="Category" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" FilterControlWidth="80%">
                               <ItemTemplate>
                                    <asp:HyperLink ID="lnkCategory" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Category") %>'
                                        NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"ViewProductCatagoryPageUrl") %>'></asp:HyperLink>
                                </ItemTemplate>
                                <ItemStyle CssClass="w30" />
                            </rad:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </rad:RadGrid>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

