<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="RenewMultipleSubscription.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.RenewMultipleSubscription" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="uc1" TagName="CreditCard" Src="../Aptify_General/CreditCard.ascx" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessShoppingCart" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<div class="table-div">
    <asp:UpdatePanel ID="upnlMain" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <contenttemplate>
            <div id="tblmember" runat="server" class="row-div">
                    This page displays active subscriptions that are available for renewal. You can
                    also enable or disable Auto Renewal for a particular subscription from this screen.
                    <br />
                    Please note that you cannot renew subscriptions that currently have Auto Renewal
                    enabled, since these subscriptions will automatically renew on their expiration
                    date.
                    <br />
                    Also, you cannot enable Auto Renewal for a subscription that is past due. To enable
                    auto renewal in this scenario, you should use the Renew Subscription option to renew
                    the subscription now and you can enable auto renewal for the future during the check-out
                    process.
                 
                <div class="row-div top-margin">
                    <rad:RadGrid ID="grdmember" runat="server" AutoGenerateColumns="False" AllowPaging="true"
                        PagerStyle-PageSizeLabelText="Records Per Page" AllowFilteringByColumn="true"
                        OnItemCreated="grdmember_GridItemCreated" SortingSettings-SortedDescToolTip="Sorted Descending"
                        SortingSettings-SortedAscToolTip="Sorted Ascending">
                        <GroupingSettings CaseSensitive="false" />
                        <MasterTableView AllowFilteringByColumn="true" AllowSorting="true" NoMasterRecordsText="No Subscriptions Available."
                            AllowNaturalSort="false" ClientDataKeyNames="ID">
                            <NoRecordsTemplate>
                                No Subscriptions Available.
                            </NoRecordsTemplate>
                            <Columns>
                                <rad:GridTemplateColumn HeaderText="" AllowFiltering="false">
                                    <ItemStyle></ItemStyle>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSubscriber" runat="server" />
                                    </ItemTemplate>
                                </rad:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="ID" Visible="false">
                                </telerik:GridBoundColumn>
                                <rad:GridTemplateColumn Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPersonID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"RecipientID") %>'></asp:Label>
                                    </ItemTemplate>
                                </rad:GridTemplateColumn>
                                <rad:GridTemplateColumn HeaderText="Name" DataField="Recipient" SortExpression="Recipient"
                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                    ShowFilterIcon="false">
                                    <ItemStyle></ItemStyle>
                                    <ItemTemplate>
                                        <div class="row-div">
                                            <div class="label-div">
                                                <rad:RadBinaryImage ID="imgmember" runat="server" AutoAdjustImageControlSize="false"
                                                    ResizeMode="Fill"></rad:RadBinaryImage>
                                            </div>
                                            <div class="field-div1">
                                                <asp:Label ID="lblMember" CssClass="name-link" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Recipient") %>'></asp:Label><br />
                                                <asp:Label ID="lblMemberTitle" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Title") %>'></asp:Label><br />
                                                <asp:Label ID="lblCity" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"City") %>'> </asp:Label>
                                                <asp:Label ID="lblCountry" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Country") %>'> </asp:Label>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </rad:GridTemplateColumn>
                                <rad:GridTemplateColumn HeaderText="Subscription Type" SortExpression="PurchaseType"
                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Custom" ShowFilterIcon="false"
                                    DataField="PurchaseType">
                                    <ItemStyle></ItemStyle>
                                    <ItemTemplate>
                                        <div valign="top">
                                            <asp:Label ID="lblPurchaseType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"PurchaseType") %>'> </asp:Label>
                                        </div>
                                    </ItemTemplate>
                                </rad:GridTemplateColumn>
                                <rad:GridTemplateColumn HeaderText="Subscription ID" SortExpression="ID" AutoPostBackOnFilter="true"
                                    CurrentFilterFunction="Custom" ShowFilterIcon="false" DataField="ID">
                                    <ItemStyle></ItemStyle>
                                    <ItemTemplate>
                                        <div valign="top">
                                            <asp:Label ID="lblSubscriptionID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ID") %>'> </asp:Label>
                                        </div>
                                    </ItemTemplate>
                                </rad:GridTemplateColumn>
                                <rad:GridBoundColumn HeaderText="Subscribed For" AllowFiltering="true" SortExpression="Product"
                                    DataField="Product" UniqueName="Product" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                    ShowFilterIcon="false">
                                    <ItemStyle></ItemStyle>
                                </rad:GridBoundColumn>
                                <rad:GridDateTimeColumn HeaderText="Subscription Expire On" SortExpression="EndDate"
                                    UniqueName="GridDateTimeColumnStartDate" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                                    ShowFilterIcon="false" FilterControlToolTip="Enter a filter date" FilterControlWidth="100px"
                                    DataType="System.DateTime" DataField="EndDate" DataFormatString="{0:MMMM dd, yyyy }"
                                    PickerType="DatePicker">
                                    <ItemStyle></ItemStyle>
                                </rad:GridDateTimeColumn>
                                <rad:GridTemplateColumn HeaderText="Auto Renewal" AllowFiltering="false">
                                    <ItemStyle></ItemStyle>
                                    <ItemTemplate>
                                        
                                            <asp:Label ID="lblOnOff" runat="server" />
                                        
                                    </ItemTemplate>
                                </rad:GridTemplateColumn>
                                <rad:GridTemplateColumn Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProduct" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Product") %>'></asp:Label>
                                    </ItemTemplate>
                                </rad:GridTemplateColumn>
                                <rad:GridTemplateColumn Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProductID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ProductID") %>'></asp:Label>
                                        <asp:Label ID="lblExpiryDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ExpiryDate") %>'></asp:Label>
                                    </ItemTemplate>
                                </rad:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </rad:RadGrid>
                </div>
            </div>
        </contenttemplate>
    </asp:UpdatePanel>
    <div class="row-div">
        <asp:Button runat="server" ID="btnRenew" Text="Renew Subscription" class="submit-Btn"
            CausesValidation="False" />
        <asp:Button ID="btnRenewalOn" Text="Turn Auto Renewal On" runat="server" class="submit-Btn"
            CausesValidation="False" />
        <asp:Button ID="btnRenewalOff" Text="Turn Auto Renewal Off" runat="server" class="submit-Btn"
            CausesValidation="False" />
    </div>
    <rad:radwindow id="CreditcardWindow" runat="server" visibleonpageload="false" modal="true"
        behaviors="Move" title="Payment Information" visiblestatusbar="false" skin="Default"
        iconurl="" class="payment-information-popup">
        <ContentTemplate>
            
                <div class="row-div">
                    <asp:Label ID="lblMessage" runat="server" Text=" Please Enter Payment Information to Create Standing Orders."></asp:Label>
                </div>
                <div class="row-div">
                    <uc1:CreditCard ID="CreditCard" runat="server" />
                </div>
                <div class="row-div align-center">
                    <asp:CheckBox ID="chkMakePayment" runat="server" Visible="false" />
                    <asp:Button ID="btnMakePayment" runat="server" Text="Make Payment" CssClass="submit-Btn" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" CssClass="submit-Btn" />
                </div>
            
        </ContentTemplate>
    </rad:radwindow>
    <rad:radwindow id="radWindowMessage" runat="server" class="popup-rad-confirm" visibleonpageload="false" modal="true"
        skin="Default" behaviors="Move" title="" visiblestatusbar="false" iconurl="">
        <ContentTemplate>
                 <div class="row-div errormsg-div align-center">
                    <asp:Label ID="lblError" runat="server"></asp:Label>
                </div>
                <div class="row-div align-center">
                    <asp:Button ID="btnOk" runat="server" Text="Ok" CssClass="submit-Btn" CausesValidation="false" />
                </div>
            
        </ContentTemplate>
    </rad:radwindow>
    <cc3:user id="User1" runat="server" />
    <cc2:aptifyshoppingcart id="ShoppingCart1" runat="server" visible="False" />
</div>

