<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="MeetingRegistrationSelectRegistrant.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.MeetingRegistrationSelectRegistrant" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessShoppingCart" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:UpdateProgress ID="updatePanelMain" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="updatePanelButton">
    <progresstemplate>
            <div class="processing-div">
                <div class="processing">
                    Please wait...
                </div>
            </div>
        </progresstemplate>
</asp:UpdateProgress>
<div class="row-div">
    <img alt="Web Image" src="" runat="server" id="imgWebImage" visible="false" />
    <asp:Label runat="server" ID="lblName" CssClass="label"/>
</div>
<div class="row-div">
    <asp:Label runat="server" ID="lblDates" CssClass="label"/>
</div>
<div runat="server" visible="false" id="trSessionParent" class="row-div">
    <asp:HiddenField runat="server" ID="hfParentID" />
    Part of:
    <asp:HyperLink runat="server" ID="lnkParent">
        <asp:Label runat="server" ID="lblParent" />
    </asp:HyperLink>
</div>
<div class="row-div">
    <asp:Label runat="server" ID="lblPlace" />
    <asp:Label runat="server" ID="lblLocation" />
</div>
<div class="row-div">
    <asp:Label runat="server" ID="lblText" Text=""></asp:Label><asp:Label runat="server"
        ID="lblAvailableSpace"></asp:Label>
</div>
<div class="row-div">
    <asp:Label runat="server" ID="lblMessage" Text=""></asp:Label>
</div>
<div class="errormsg-div">
    <asp:Label ID="lblError" runat="server" Visible="False"></asp:Label>
</div>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <contenttemplate>
        <div id="tblmember" runat="server" class="row-div">
            <rad:RadGrid ID="grdmember" runat="server" AutoGenerateColumns="False" AllowPaging="true"
                PagerStyle-PageSizeLabelText="Records Per Page" AllowFilteringByColumn="true"
                AllowSorting="true" SortingSettings-SortedDescToolTip="Sorted Descending" SortingSettings-SortedAscToolTip="Sorted Ascending"
                OnItemCreated="grdmember_GridItemCreated">
                <GroupingSettings CaseSensitive="false" />
                <MasterTableView AllowFilteringByColumn="true" ClientDataKeyNames="AttendeeID" AllowNaturalSort="false">
                    <Columns>
                        <rad:GridTemplateColumn HeaderText="" AllowFiltering="false">
                            <ItemStyle HorizontalAlign="Center" CssClass="gridAlign"></ItemStyle>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkRegistrant" runat="server" />
                            </ItemTemplate>
                        </rad:GridTemplateColumn>
                        <rad:GridBoundColumn DataField="AttendeeID" Visible="false">
                        </rad:GridBoundColumn>
                        <rad:GridTemplateColumn Visible="False" DataField="AttendeeID">
                            <ItemTemplate>
                                <asp:Label ID="lblPersonID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"AttendeeID") %>'></asp:Label></ItemTemplate>
                        </rad:GridTemplateColumn>
                        <rad:GridTemplateColumn HeaderText="Member" DataField="AttendeeID_FirstLast" SortExpression="AttendeeID_FirstLast"
                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                            ShowFilterIcon="false">
                            <ItemStyle></ItemStyle>
                            <ItemTemplate>
                                <div class="row-div clearfix">
                                        <div class="label-div">
                                                <rad:RadBinaryImage ID="imgmember" runat="server" AutoAdjustImageControlSize="false">
                                                </rad:RadBinaryImage>
                                         </div>
                                        <div>
                                            <asp:Label ID="lblMember" CssClass="name-link" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"AttendeeID_FirstLast") %>'>
                                            </asp:Label><br />
                                            <asp:Label ID="lblMemberTitle" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Title") %>'></asp:Label><br />
                                            <asp:Label ID="lbladdress" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"City") %>'> </asp:Label>
                                       </div>
                                    </div>
                            </ItemTemplate>
                        </rad:GridTemplateColumn>
                        <rad:GridTemplateColumn HeaderText="Email" DataField="Email" SortExpression="Email"
                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                            <ItemStyle CssClass="email-style"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblEmail" HeaderText="Email" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Email") %>'> </asp:Label></ItemTemplate>
                                                               
                        </rad:GridTemplateColumn>
                        <rad:GridTemplateColumn HeaderText="Member Type" SortExpression="MemberType" AutoPostBackOnFilter="true"
                            CurrentFilterFunction="Contains" ShowFilterIcon="false" 
                            DataField="MemberType">
                            <ItemStyle></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblMemberType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"MemberType") %>'> </asp:Label></ItemTemplate>
                        </rad:GridTemplateColumn>
                        <rad:GridTemplateColumn HeaderText="Badge Information" AllowFiltering="false">
                            <ItemStyle />
                            <ItemTemplate>
                                <asp:LinkButton ID="PreviewLink" runat="server" Visible="false" CommandName="Preview"
                                    Text="Preview"></asp:LinkButton><asp:LinkButton ID="EditBagdeLink" runat="server"
                                        CommandName="EditBagde" Text="Preview/Edit"></asp:LinkButton></ItemTemplate>
                        </rad:GridTemplateColumn>
                        <rad:GridTemplateColumn Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBadgeName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"AttendeeID_FirstLast") %>'></asp:Label></ItemTemplate>
                        </rad:GridTemplateColumn>
                        <rad:GridTemplateColumn Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBadgeTitle" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Title") %>'>></asp:Label></ItemTemplate>
                        </rad:GridTemplateColumn>
                        <rad:GridTemplateColumn Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBadgeCompany" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Company") %>'>></asp:Label></ItemTemplate>
                        </rad:GridTemplateColumn>
                    </Columns>
                </MasterTableView></rad:RadGrid>
            <asp:UpdatePanel runat="server" ID="UpEditBadgeInfo" UpdateMode="Conditional" ChildrenAsTriggers="false">
                <ContentTemplate>
                    <rad:RadWindow ID="UserListDialog" runat="server" Modal="True" Skin="Default" class="popup-meeting-meetingregistration"
                        VisibleStatusbar="False" Behaviors="Move" IconUrl="" Title="Badge Information"
                        AutoSize="false" MinimizeIconUrl="" >
                        <ContentTemplate>
                            <div class="meeting-reg-main-div meeting-reg">
                                <div class="row-div clearfix">
                                    <div class="lblBadgeInformation" >
                                    </div>
                                </div>
                                <div class="row-div clearfix">
                                    <div class="label-div w19">
                                        Name:
                                    </div>
                                    <div class="field-div1 w79">
                                        <asp:TextBox ID="txtBadgeName" TabIndex="1" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row-div clearfix">
                                  <div class="label-div w19">
                                        Title:
                                    </div>
                                     <div class="field-div1 w79">
                                        <asp:TextBox ID="txtBadgeTitle" runat="server" TabIndex="2"></asp:TextBox>
                                     </div>
                                </div>
                                <div class="row-div clearfix">
                                   <div class="label-div w19">
                                        Company:
                                      </div>
                                    <div class="field-div1 w79">
                                        <asp:TextBox ID="txtBadgeCompany" runat="server" TabIndex="3"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row-div clearfix">
                                   <div class="label-div w19">
                                    &nbsp;
                                     </div>
                                    <div class="field-div1 w79">
                                        <asp:Button ID="btnUpdate" class="submit-Btn" TabIndex="27" runat="server" 
                                            Text="Save" ValidationGroup="EditProfileControl" />&nbsp;&nbsp;
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="submit-Btn"
                                            TabIndex="28" OnClientClick="OnClientClick();" />
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </rad:RadWindow>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="UserListDialog" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </contenttemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="updatePanelButton" runat="server" ChildrenAsTriggers="True">
    <contenttemplate>
        <div class="row-div clearfix">
            <asp:Button runat="server" ID="btnSubmit" Text="Register" CssClass="submit-Btn" />
        </div>
    </contenttemplate>
</asp:UpdatePanel>
<asp:HiddenField runat="server" ID="hfSessions" />
<cc3:user id="User1" runat="server" />
<cc2:aptifyshoppingcart id="ShoppingCart1" runat="server" visible="False" />
