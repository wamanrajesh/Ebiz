<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="RenewMultipleMembership.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.RenewMultipleMembership" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="uc1" TagName="CreditCard" Src="../Aptify_General/CreditCard.ascx" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessShoppingCart" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<div class="table-div">
    <asp:UpdatePanel ID="upnlMain" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="tblmember" runat="server">
                <div class="row-div">
                    This page displays active memberships that are available for renewal. You can also
                    enable or disable Auto Renewal for a particular membership from this screen.
                    <br />
                    Please note that you cannot renew memberships that currently have Auto Renewal enabled,
                    since these memberships will automatically renew on their expiration date.
                    <br />
                    Also, you cannot enable Auto Renewal for a membership that is past due. To enable
                    auto renewal in this scenario, you should use the Renew Membership option to renew
                    the membership now and you can enable auto renewal for the future during the check-out
                    process.
                    <br />
                </div>
                <div class="row-div">
                    <telerik:RadGrid ID="radGridMembership" runat="server" AutoGenerateColumns="False"
                        GridLines="None" CellSpacing="0" AllowPaging="True" AllowSorting="True" AllowFilteringByColumn="True"
                        OnItemCreated="radGridMembership_GridItemCreated" SortingSettings-SortedDescToolTip="Sorted Descending"
                        SortingSettings-SortedAscToolTip="Sorted Ascending">
                        <GroupingSettings CaseSensitive="false" />
                        <MasterTableView DataKeyNames="ID" AllowFilteringByColumn="true" AllowSorting="true"
                            NoMasterRecordsText="No Memberships Available." AllowNaturalSort="false" ClientDataKeyNames="ID">
                            <CommandItemSettings ExportToPdfText="Export to PDF" />
                            <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                            </ExpandCollapseColumn>
                            <Columns>
                                <telerik:GridBoundColumn DataField="ID" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn HeaderText="" AllowFiltering="false">
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSubscriber" runat="server" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn DataField="ID" HeaderText="ID" SortExpression="ID" 
                                    AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSubscriptionID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ID") %>'> </asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <rad:GridTemplateColumn HeaderText="Name" DataField="Recipient" SortExpression="Recipient"
                                    AutoPostBackOnFilter="true"  CurrentFilterFunction="Contains"
                                    ShowFilterIcon="false">
                                    <ItemStyle HorizontalAlign="left"></ItemStyle>
                                    <ItemTemplate>
                                        <div class="row-div clearfix">
                                            <div class="label-div">
                                                <rad:RadBinaryImage ID="imgmember" runat="server" CssClass="PeopleImage" AutoAdjustImageControlSize="false">
                                                </rad:RadBinaryImage>
                                            </div>
                                            <div class="field-div1">
                                                <asp:Label ID="lblMember" CssClass="namelink" runat="server" Text='
					<%# DataBinder.Eval(Container.DataItem,"Recipient") %>
						'>
                                                </asp:Label>
                                                <br />
                                                <asp:Label ID="lblMemberTitle" runat="server" Text='
						<%# DataBinder.Eval(Container.DataItem,"Title") %>
							'>
                                                </asp:Label>
                                                <br />
                                                <asp:Label ID="lblCity" runat="server" Text='
							<%# DataBinder.Eval(Container.DataItem,"City") %>
								'>
                                                </asp:Label>
                                                <asp:Label ID="lblCountry" runat="server" Text='
								<%# DataBinder.Eval(Container.DataItem,"Country") %>
									'>
                                                </asp:Label>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </rad:GridTemplateColumn>
                                <telerik:GridTemplateColumn DataField="Product" UniqueName="MembershipType" 
                                    HeaderText="Membership Product" SortExpression="Product" AutoPostBackOnFilter="true"
                                    CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMembershipType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Product") %>'> </asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridDateTimeColumn HeaderText="Expiration Date" SortExpression="ExpiryDate"
                                    AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false"
                                    FilterControlWidth="100px" DataType="System.DateTime" HtmlEncode="false" DataField="ExpiryDate"
                                    DataFormatString="{0:MMMM dd, yyyy}" PickerType="DatePicker" UniqueName="GridDateTimeColumnStartDate"
                                    FilterControlToolTip="Enter a filter date" />
                                <telerik:GridTemplateColumn UniqueName="AutoRenewal" HeaderText="Auto Renewal" 
                                    ShowFilterIcon="false" AllowFiltering="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOnOff" runat="server" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn DataField="Status" UniqueName="Status" HeaderText="Status"
                                    AllowFiltering="false" ShowFilterIcon="false">
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    <HeaderStyle ></HeaderStyle>
                                    <ItemTemplate>
                                        <div class="row-div">
                                            <asp:Image ID="imgStatus" runat="server" />
                                            <asp:Label ID="lblstatus" runat="server">
                                            </asp:Label>
                                        </div>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <rad:GridTemplateColumn Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPurchaseType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"PurchaseType") %>'> </asp:Label>
                                        <asp:Label ID="lblProductID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ProductID") %>'></asp:Label>
                                        <asp:Label ID="lblProduct" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Product") %>'></asp:Label>
                                        <asp:Label ID="lblId" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ID") %>'> </asp:Label>
                                        <asp:Label ID="lblPersonID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"RecipientID") %>'></asp:Label>
                                        <asp:Label ID="lblExpiryDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ExpiryDate") %>'></asp:Label>
                                    </ItemTemplate>
                                </rad:GridTemplateColumn>
                            </Columns>
                            <EditFormSettings>
                                <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                </EditColumn>
                            </EditFormSettings>
                            <ItemStyle />
                            <NoRecordsTemplate>
                                No Memberships Available.
                            </NoRecordsTemplate>
                        </MasterTableView>
                        <FilterMenu EnableImageSprites="False">
                        </FilterMenu>
                    </telerik:RadGrid>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="row-div">
        <asp:Button runat="server" ID="btnRenew" Text="Renew Membership" class="submit-Btn"
            CausesValidation="False" />
        <asp:Button ID="btnRenewalOn" Text="Turn Auto Renewal On" runat="server" class="submit-Btn"
            CausesValidation="False" />
        <asp:Button ID="btnRenewalOff" Text="Turn Auto Renewal Off" runat="server" class="submit-Btn"
            CausesValidation="False" />
    </div>
    <rad:RadWindow ID="radWindowPayment" runat="server" VisibleOnPageLoad="false" Modal="true"
        Skin="Default" Behaviors="Move" class="payment-information-popup" Title="Payment Information" VisibleStatusbar="false"
        IconUrl="">
        <ContentTemplate>
            <div>
                <div>
                    <asp:Label ID="lblMessage" runat="server" Text=" Please Enter Payment Information to Create Standing Orders."></asp:Label>
                    <uc1:CreditCard ID="CreditCard" runat="server" />
                </div>
                <div>
                    <asp:CheckBox ID="chkMakePayment" runat="server" Visible="false" />
                    <asp:Button ID="btnMakePayment" runat="server" Text="Make Payment" CssClass="submit-Btn" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="submit-Btn" CausesValidation="false" />
                </div>
            </div>
        </ContentTemplate>
    </rad:RadWindow>
    <rad:RadWindow ID="radWindowMessage" runat="server" class="popup-rad-confirm" VisibleOnPageLoad="false" Modal="true"
        Skin="Default" Behaviors="Move" Title="" VisibleStatusbar="false" IconUrl="">
        <ContentTemplate>
            <div class="row-div errormsg-div align-center">
                <asp:Label ID="lblError" runat="server">
                </asp:Label>
            </div>
            <div class="row-div align-center">
                <asp:Button ID="btnOk" runat="server" Text="Ok" CssClass="submit-Btn" CausesValidation="false" />
            </div>
        </ContentTemplate>
    </rad:RadWindow>
    <cc3:User ID="User1" runat="server" />
    <cc2:AptifyShoppingCart ID="ShoppingCart1" runat="server" Visible="False" />
</div>
