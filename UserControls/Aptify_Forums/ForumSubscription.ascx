<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ForumSubscription.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.Forums.ForumSubscription" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:UpdatePanel ID="updatePanelHeader" runat="server">
    <contenttemplate>
           
                <div class="row-div">
                    <asp:CheckBox ID="chkEnableAll" runat="server" OnCheckedChanged="chkEnableAll_CheckedChanged"
                        AutoPostBack="true" />
                    <asp:Label ID="lblEnable" runat="server" Text="Enable All Forum Subscriptions">
                    </asp:Label>
                </div>
                <div class="row-div clearfix">
                <div class="label-div">
                    <asp:Label ID="lblResults" runat="server">
                    </asp:Label>
                   </div>
                   <div class="field-div1">
                     <asp:HyperLink runat="server" ID="lnkForumsHome" Text="Forums Home" Visible="false">
                    </asp:HyperLink>
                    </div>
                </div>
           
        </contenttemplate>
    <triggers>
            <asp:AsyncPostBackTrigger ControlID="cmdSave" EventName="Click" />
        </triggers>
</asp:UpdatePanel>
<div class="row-div">
    <asp:UpdatePanel ID="updPanelGrid" runat="server" UpdateMode="Always">
        <contenttemplate>
              <div class="row-div">
                <asp:Button CssClass="submit-Btn" ID="cmdSave" runat="server" Text="Save Changes">
                </asp:Button></div>
                <rad:RadGrid ID="grdForumSubscriptions" runat="server" AutoGenerateColumns="False"
                    DataKeyNames="Name" SortingSettings-SortedDescToolTip="Sorted Descending" SortingSettings-SortedAscToolTip="Sorted Ascending"
                    AllowPaging="true" AllowFilteringByColumn="True" AllowSorting="true"
                    AutoPostBackOnFilter="true">
                    <GroupingSettings CaseSensitive="false" />
                    <MasterTableView AllowSorting="true" AllowNaturalSort="false" AllowFilteringByColumn="true">
                        <Columns>
                            <rad:GridTemplateColumn HeaderText="Subscription" AllowFiltering="false" >
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSubscription" runat="server"></asp:CheckBox>
                                </ItemTemplate>
                                <ItemStyle/>
                            </rad:GridTemplateColumn>
                            <rad:GridBoundColumn DataField="Name" HeaderText="Name"
                                SortExpression="Name" CurrentFilterFunction="Contains"
                                ShowFilterIcon="false" AutoPostBackOnFilter="true" />
                            <rad:GridBoundColumn DataField="Parent" HeaderText="Category"
                                SortExpression="Parent" CurrentFilterFunction="Contains"
                                ShowFilterIcon="false" AutoPostBackOnFilter="true" />
                            <rad:GridTemplateColumn HeaderText="Delivery Type" AllowFiltering="false">
                                <ItemStyle/>
                                <ItemTemplate>
                                    <asp:Label ID="lblDeliveryType" runat="server" Visible="false" Text='<%# Eval("DeliveryType") %>'></asp:Label>
                                    <asp:DropDownList ID="ddlDeliveryType" runat="server" >
                                        <asp:ListItem>Daily Digest</asp:ListItem>
                                        <asp:ListItem>Realtime</asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </rad:GridTemplateColumn>
                            <rad:GridTemplateColumn HeaderText="Forum Type" AllowFiltering="false" >
                                <ItemTemplate>
                                    <asp:Image ID="imgForumType" runat="server"></asp:Image>
                                </ItemTemplate>
                                <ItemStyle/>
                            </rad:GridTemplateColumn>
                            <rad:GridTemplateColumn HeaderText="ID" SortExpression="ID" >
                                <ItemTemplate>
                                    <asp:Label ID="lblforumID" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                </ItemTemplate>
                            </rad:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </rad:RadGrid>
            </contenttemplate>
    </asp:UpdatePanel>
</div>
<div class="row-div clearfix">
    <div class="label-div">
        <asp:Label ID="lblEndDate" runat="server" Text="End Date (mm/dd/yyyy)(optional)"
            Visible="false"></asp:Label></div>
    <div class="field-div1">
        <asp:TextBox ID="txtboxendDate" runat="server" Visible="false">
        </asp:TextBox></div>
    <cc1:user id="User1" runat="server" />
</div>

