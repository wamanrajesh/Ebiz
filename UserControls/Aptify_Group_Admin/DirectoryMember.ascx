<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="DirectoryMember.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.CustomerService.DirectoryMember" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessGlobal" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <contenttemplate>
        <div class="table-div">
            <div id="tblmember" runat="server">
                <div class="row-div">
                    <rad:RadGrid ID="grdmember" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                        PagerStyle-PageSizeLabelText="Records Per Page" AllowFilteringByColumn="True"
                         GridLines="None" OnItemCreated="grdmember_GridItemCreated">
                        <GroupingSettings CaseSensitive="false" />
                        <MasterTableView AllowFilteringByColumn="true" AllowSorting="true">
                            <CommandItemSettings ExportToPdfText="Export to PDF" />
                            <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                            </ExpandCollapseColumn>
                            <Columns>
                                <rad:GridBoundColumn HeaderText="ID" DataField="ID" Visible="False" 
                                    CurrentFilterFunction="EqualTo" ShowFilterIcon="false" SortExpression="ID" AutoPostBackOnFilter="true"
                                    UniqueName="ID">
                                    <ItemStyle />
                                </rad:GridBoundColumn>
                                <rad:GridTemplateColumn HeaderText="Member" DataField="FirstLast" SortExpression="FirstLast"
                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"
                                    UniqueName="DirectoryMemberName">
                                    <ItemStyle></ItemStyle>
                                    <ItemTemplate>
                                        <div class="row-div clearfix">
                                            
                                                <rad:RadBinaryImage ID="imgmember" runat="server" AutoAdjustImageControlSize="false" CssClass="img-float"/>
                                            <div>
                                                <asp:HyperLink ID="lblMember" CssClass="name-link" runat="server" Text='
														<%# DataBinder.Eval(Container.DataItem,"FirstLast") %>
															' NavigateUrl='
															<%# DataBinder.Eval(Container.DataItem,"AdminEditprofileUrl") %>
																'>
                                                </asp:HyperLink>
                                                <br />
                                                <asp:Label ID="lblMemberTitle" runat="server" Text='
																<%# DataBinder.Eval(Container.DataItem,"title") %>
																	'>
                                                </asp:Label>
                                                <br />
                                                <asp:Label ID="lbladdress" runat="server" Text='
																	<%# DataBinder.Eval(Container.DataItem,"address") %>
																		'>
                                                </asp:Label>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </rad:GridTemplateColumn>
                                <rad:GridTemplateColumn HeaderText="Member" DataField="FirstLast" SortExpression="FirstLast"
                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"
                                    Visible="false" UniqueName="MemberName">
                                    <ItemStyle></ItemStyle>
                                    <ItemTemplate>
                                        <asp:HyperLink ID="lblMemberName" CssClass="name-link" runat="server" Text='
															<%# DataBinder.Eval(Container.DataItem,"FirstLast") %>
																' NavigateUrl='
																<%# DataBinder.Eval(Container.DataItem,"AdminEditprofileUrl") %>
																	'>
                                        </asp:HyperLink>
                                        <br />
                                    </ItemTemplate>
                                </rad:GridTemplateColumn>
                                <rad:GridTemplateColumn HeaderText="Email" DataField="Email" SortExpression="Email"
                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                    <ItemStyle>
                                    </ItemStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmail" HeaderText="Email" runat="server" Text='
																	<%# DataBinder.Eval(Container.DataItem,"Email") %>
																		'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </rad:GridTemplateColumn>
                                <rad:GridBoundColumn HeaderText="Membership Type" DataField="MemberType" 
                                    SortExpression="MemberType" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                    ShowFilterIcon="false" UniqueName="MemberShipType">
                                    <ItemStyle/>
                                </rad:GridBoundColumn>
                                <rad:GridBoundColumn HeaderText="Start Date" DataField="JoinDate" 
                                    SortExpression="JoinDate" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                    ShowFilterIcon="false" UniqueName="StartDate">
                                    <ItemStyle />
                                </rad:GridBoundColumn>
                                <rad:GridBoundColumn HeaderText="End Date" DataField="DuesPaidThru"
                                    SortExpression="DuesPaidThru" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                    ShowFilterIcon="false" UniqueName="EndDate">
                                    <ItemStyle />
                                </rad:GridBoundColumn>
                                <rad:GridTemplateColumn HeaderText="Status" SortExpression="Status" AutoPostBackOnFilter="true"
                                    CurrentFilterFunction="Contains" ShowFilterIcon="false" DataField="Status">
                                    <ItemStyle></ItemStyle>
                                    <ItemTemplate>
                                        <div class="row-div clearfix">
                                            
                                                <rad:RadBinaryImage ID="imgstaus" runat="server" />
                                            
                                            
                                                <asp:Label ID="lblstatus" runat="server" Text='
																					<%# DataBinder.Eval(Container.DataItem,"Status") %>
																						'>
                                                </asp:Label>
                                            
                                        </div>
                                    </ItemTemplate>
                                </rad:GridTemplateColumn>
                                <rad:GridTemplateColumn HeaderText="Remove" AllowFiltering="false" UniqueName="Remove">
                                    <ItemStyle>
                                    </ItemStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="lblPersonID" runat="server" Text='
																			<%# DataBinder.Eval(Container.DataItem,"ID") %>
																				' Visible="false">
                                        </asp:Label>
                                        <asp:CheckBox ID="chkRmvCompLink" runat="server" AutoPostBack="true" ToolTip="Remove Person From Company" />
                                    </ItemTemplate>
                                </rad:GridTemplateColumn>
                            </Columns>
                            <EditFormSettings>
                                <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                </EditColumn>
                            </EditFormSettings>
                        </MasterTableView>
                        <PagerStyle PageSizeLabelText="Records Per Page" />
                        <FilterMenu EnableImageSprites="False">
                        </FilterMenu>
                    </rad:RadGrid>
                </div>
                <div class="row-div errormsg-div">
                    <asp:Label ID="lblError" runat="server" Visible="false">
                    </asp:Label>
                </div>
                <div class="row-div align-right" >
                    <asp:Button ID="btnRmvCompLink" Visible="false" runat="server" Text="Remove From Company"
                        CssClass="submit-Btn" />
                </div>
                <rad:RadWindow ID="radConfirm" runat="server" Modal="True"
                    Skin="Default" VisibleStatusbar="False" Behaviors="None"
                    IconUrl="~/Images/Alert.png" Title="Alert" Behavior="None" class="popup-rad-confirm">
                    <ContentTemplate>
                        <div class=" div-row label align-center">
                            <asp:Label ID="lblConfirm" runat="server" Text="">
                            </asp:Label>
                        </div>
                        <div class="div-row top-margin align-center" >
                            <asp:Button ID="btnConfirm" runat="server" Text="Yes" class="submit-Btn" OnClick="btnConfirm_Click"
                                ValidationGroup="ok" />
                            <asp:Button ID="btnNo" runat="server" Text="No" class="submit-Btn" OnClick="btnNo_Click"
                                ValidationGroup="ok" />
                        </div>
                    </ContentTemplate>
                </rad:RadWindow>
                <rad:RadWindow ID="radRCValidation" runat="server" class="popup-rad-confirm" Modal="True"
                    Skin="Default" VisibleStatusbar="False" Behaviors="None"
                    IconUrl="~/Images/Alert.png" Title="Alert" Behavior="None">
                    <ContentTemplate>
                        <div class="div-row label align-center">
                            <asp:Label ID="lblRCValidation" runat="server" Text="">
                            </asp:Label>
                        </div>
                        <div class="div-row top-margin align-center" >
                            <asp:Button ID="btnradRCValidation" runat="server" Text="OK" class="submit-Btn" OnClick="btnNo_Click"
                                ValidationGroup="ok" />
                        </div>
                    </ContentTemplate>
                </rad:RadWindow>
            </div>
    </contenttemplate>
</asp:UpdatePanel>
<cc2:user id="User1" runat="server"></cc2:user>
