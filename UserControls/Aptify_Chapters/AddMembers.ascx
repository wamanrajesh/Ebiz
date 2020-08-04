<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="AddMembers.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.Chapters.AddMembersControl" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="uc1" TagName="ChapterMember" Src="ChapterMember.ascx" %>
<%@ Register TagPrefix="uc1" TagName="NameAddressBlock" Src="../Aptify_General/NameAddressBlock.ascx" %>
<%@ Register TagPrefix="cc4" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessShoppingCart" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<div class="errormsg-div clearfix">
<asp:Label ID="lblErrorMain" runat="server" Visible="False"></asp:Label></div>
<div id="tblMain" runat="server" class="table-div clearfix">
     <div class="row-div-bottom-line">
    <div class="control-title">
        <asp:Label ID="lblChapterName" runat="server">
        </asp:Label>
    </div>
    </div>
    <div class="row-div clearfix" runat="server" id="trSuccess">
        <p>
            Your submission was accepted. Please press Continue to return to the main chapter
            screen.
        </p>
        <p>
            <asp:Button ID="btnSuccess" runat="server" Text="Continue" CssClass="submit-Btn">
            </asp:Button>
        </p>
    </div>
    <div class="row-div" runat="server" id="trAddMembers">
        <asp:Button ID="cmdAddRow" AccessKey="A" CssClass="submit-Btn" runat="server" Text="Add Row"
            DESIGNTIMEDRAGDROP="120"></asp:Button>
        <asp:Button ID="cmdSubmit" runat="server" Text="Submit New Members" CssClass="submit-Btn">
        </asp:Button>
        <div class="errormsg-div clearfix">
        <asp:Label ID="lblError" runat="server" Visible="False">
        </asp:Label></div>
        <div class="top-margin">
            <asp:UpdatePanel ID="updPanelGrid" runat="server">
                <ContentTemplate>
                    <rad:radgrid id="grdMembers" runat="server" autogeneratecolumns="False" allowpaging="false"
                        CssClass="row-div" allowfilteringbycolumn="false">
											<GroupingSettings CaseSensitive="false" />
											<MasterTableView AllowSorting="false">
												<Columns>
													<rad:GridTemplateColumn HeaderText="First Name" DataField="FirstName" 
                                                    SortExpression="FirstName" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
														<ItemTemplate>
															<asp:TextBox ID="txtFirstName" runat="server" Text='
															<%# DataBinder.Eval(Container.DataItem,"FirstName") %>
																'>
																</asp:TextBox>
															</ItemTemplate>
														</rad:GridTemplateColumn>
														<rad:GridTemplateColumn HeaderText="Last Name" DataField="LastName" SortExpression="LastName" AutoPostBackOnFilter="true" 
                                                        CurrentFilterFunction="Contains" ShowFilterIcon="false">
															<ItemTemplate>
																<asp:TextBox ID="txtLastName" runat="server" Text='
																<%# DataBinder.Eval(Container.DataItem,"LastName") %>
																	'>
																	</asp:TextBox>
																</ItemTemplate>
															</rad:GridTemplateColumn>
															<rad:GridTemplateColumn HeaderText="Title" DataField="Title" SortExpression="Title" AutoPostBackOnFilter="true" 
                                                            CurrentFilterFunction="Contains" ShowFilterIcon="false">
																<ItemTemplate>
																	<asp:TextBox ID="txtTitle" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Title") %>'>
																		</asp:TextBox>
																	</ItemTemplate>
																</rad:GridTemplateColumn>
																<rad:GridTemplateColumn HeaderText="Email" DataField="Email" SortExpression="Email" AutoPostBackOnFilter="true" 
                                                                CurrentFilterFunction="Contains" ShowFilterIcon="false">
																	<ItemTemplate>
																		<asp:TextBox ID="txtEmail" runat="server" Text='
																		<%# DataBinder.Eval(Container.DataItem,"Email") %>
																			'>
																			</asp:TextBox>
																		</ItemTemplate>
																	</rad:GridTemplateColumn>
																	<rad:GridTemplateColumn HeaderText="Delete" AllowFiltering="false">
																		<ItemTemplate>
																			<asp:ImageButton ID="Button1" runat="server" ImageUrl="~/Images/Delete.png" CommandName="Delete" CommandArgument="
																			<%# CType(Container, GridDataItem).RowIndex %>
																				" />
																			</ItemTemplate>
																		</rad:GridTemplateColumn>
																	</Columns>
																</MasterTableView>
															</rad:radgrid>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</div>
<cc4:aptifyshoppingcart runat="Server" id="ShoppingCart1" />
<cc3:user id="User1" runat="server" />
