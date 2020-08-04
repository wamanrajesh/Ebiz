<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="MyChapters.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.Chapters.MyChaptersControl" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

   	<div class="row-div">
			<asp:UpdatePanel ID="updPanelGrid" runat="server">
				<ContentTemplate>
					<rad:RadGrid ID="grdChapters" runat="server" AutoGenerateColumns="False" AllowPaging="true" AllowFilteringByColumn="true" 
                    SortingSettings-SortedDescToolTip ="Sorted Descending" SortingSettings-SortedAscToolTip ="Sorted Ascending" CssClass="row-div">
						<GroupingSettings CaseSensitive = "false" />
						<MasterTableView AllowSorting="true" AllowNaturalSort="false">
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
										<rad:GridBoundColumn DataField="Role" HeaderText="Role" SortExpression="Role" AutoPostBackOnFilter="true" 
                                        CurrentFilterFunction="Contains" ShowFilterIcon="false"/>
									</Columns>
								</MasterTableView>
							</rad:RadGrid>
						</ContentTemplate>
					</asp:UpdatePanel>
                    <cc3:User ID="User1" runat="server" />
				</div>
		
		
    

