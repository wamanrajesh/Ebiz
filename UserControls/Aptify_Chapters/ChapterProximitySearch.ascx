<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ChapterProximitySearch.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.Chapters.ChapterProximitySearchControl" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="WebUserActivity" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="PageSecurity" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Panel ID="pnlsearchchapterproximity" runat="server" DefaultButton="cmdSearch">
    <div class="chapter-proximity-search-div">
        <div class="row-div dropdown clearfix">
            <div class="label-div w19">
                <asp:Label ID="Label1" runat="server" Text="Locate Chapters Within">
                </asp:Label>
            </div>
            <div class="field-div1 w79">
                <asp:DropDownList ID="cmbMiles" DataValueField="ID" DataTextField="WebName" runat="server">
                </asp:DropDownList>
            </div>
        </div>
        <div class="row-div clearfix">
            <div class="label-div w19">
                <asp:Label ID="Label2" runat="server" Text="Miles of Zip Code">
                </asp:Label>
            </div>
            <div class="field-div1 textboxsize w79">
                <asp:TextBox ID="txtZipCode" runat="server">
                </asp:TextBox>
            </div>
        </div>
        <div class="row-div clearfix">
            <div class="label-div w19">
                &nbsp;
            </div>
            <div class="field-div1 w79">
                <asp:Button ID="cmdSearch" runat="server" Text="Find Chapters" CssClass="submit-Btn">
                </asp:Button>
            </div>
        </div>
        <div class="row-div">
            <div class="label-div w19">
                &nbsp;
            </div>
            <div class="errormsg-div clearfix w79">
                <asp:Label ID="lblError" runat="server" Visible="false">
                </asp:Label>
            </div>
        </div>
    </div>
    <div id="trResults" runat="server" class="row-div">
        <asp:UpdatePanel ID="updPanelGrid" runat="server">
            <ContentTemplate>
                <rad:radgrid id="grdProxResults" runat="server" autogeneratecolumns="False" allowpaging="true"
                    allowfilteringbycolumn="true" sortingsettings-sorteddesctooltip="sorted descending"
                    sortingsettings-sortedasctooltip="sorted ascending" CssClass="row-div">
								<GroupingSettings CaseSensitive="false" />
								<MasterTableView AllowFilteringByColumn="true" AllowSorting="true" AllowNaturalSort="false">
									<Columns>
										<rad:GridTemplateColumn HeaderText="Chapter" DataField="Name" SortExpression="Name" AutoPostBackOnFilter="true" 
                                        CurrentFilterFunction="Contains" ShowFilterIcon="false">
											<ItemTemplate>
												<asp:HyperLink Text='
												<%# DataBinder.Eval(Container.DataItem,"Name") %>
													' ID="lnkName" runat="server" NavigateUrl='
													<%# DataBinder.Eval(Container.DataItem,"DataNavigateUrl") %>
														' />
													</ItemTemplate>
												</rad:GridTemplateColumn>
												<rad:GridBoundColumn DataField="CompanyType" HeaderText="Chapter Type" SortExpression="CompanyType" 
                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"/>
												<rad:GridNumericColumn DataField="Distance" HeaderText="Distance" SortExpression="Distance" 
                                                AutoPostBackOnFilter="true" ShowFilterIcon="false" AllowFiltering="false" />
											</Columns>
										</MasterTableView>
									</rad:radgrid>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <cc2:pagesecurity id="PageSecurity1" runat="server" webmodule="Chapter Management" />
    <cc1:webuseractivity id="WebUserActivity1" runat="server" webmodule="Chapter Management" />
    <cc3:user id="User1" runat="server" />
</asp:Panel>