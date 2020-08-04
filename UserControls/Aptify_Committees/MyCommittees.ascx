<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="vb" AutoEventWireup="false" Inherits="Aptify.Framework.Web.eBusiness.Committees.MyCommitteesControl"
    CodeFile="MyCommittees.ascx.vb" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessGlobal" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEbusinessUser" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
             <div class="committees-main-div">
             <div class="row-div dropdown">
                <asp:DropDownList ID="cmbType" runat="server" AutoPostBack="True">
                    <asp:ListItem Value="Current">Current</asp:ListItem>
                    <asp:ListItem Value="Future">Future</asp:ListItem>
                    <asp:ListItem Value="Past">Past</asp:ListItem>
                    <asp:ListItem Value="All">All</asp:ListItem>
                </asp:DropDownList> </div>  
                <div class="row-div top-margin">
                    <asp:UpdatePanel ID="updPanelGrid" runat="server">
                        <contenttemplate>
					<rad:RadGrid ID="grdCommittees" runat="server" AutoGenerateColumns="False" AllowPaging="true" 
                    AllowFilteringByColumn="true" SortingSettings-SortedDescToolTip ="Sorted Descending" 
                    SortingSettings-SortedAscToolTip="Sort Ascending">
						<GroupingSettings CaseSensitive="false" />
						<MasterTableView AllowSorting="true" AllowNaturalSort="false">
							<Columns>
								<rad:GridTemplateColumn DataField="Committee" SortExpression="Committee" HeaderText="Name" 
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" 
                                 >
									<ItemTemplate>
										<asp:HyperLink ID="lnkCommittee" runat="server" Text='
										<%# DataBinder.Eval(Container.DataItem,"Committee") %>
											' NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"DataNavigateUrl") %>' />
											</ItemTemplate>
										</rad:GridTemplateColumn>
										<rad:GridBoundColumn DataField="CommitteeTerm" HeaderText="Term" 
                                        SortExpression="CommitteeTerm" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" 
                                        ShowFilterIcon="false" />
										<rad:GridBoundColumn DataField="Title" HeaderText="Title" SortExpression="Title" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" 
                                        />
									</Columns>
								</MasterTableView>
							</rad:RadGrid>
						</contenttemplate>
                    </asp:UpdatePanel>
                    <div class="errormsg-div">
                    <asp:Label ID="lblNoCommittees" runat="server" Text="No qualifying Committees were found."
                        Visible="true">
                    </asp:Label>
                    </div>
                </div>
                <cc3:User runat="server" ID="User1" />
                     </div>
      
    
