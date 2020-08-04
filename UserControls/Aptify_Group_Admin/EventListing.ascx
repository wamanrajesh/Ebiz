<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="EventListing.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.CustomerService.EventListing" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="Rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<div class="table-div">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row-div">
                <asp:Label ID="lblRegistrationResult" runat="server" ></asp:Label>
            </div>
            <div class="row-div label">
                <asp:Label ID="lblSelections" runat="server"></asp:Label>
            </div>
            <div class="row-div">
                <Rad:RadGrid ID="RadgrdWaitingList" runat="server" AutoGenerateColumns="False" EnableViewState="true"
                    GridLines="None" CellSpacing="0" AllowPaging="True" SortingSettings-SortedDescToolTip="Sorted Descending"
                    SortingSettings-SortedAscToolTip="Sorted Ascending" AllowSorting="True" AllowFilteringByColumn="True">
                    <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                    <GroupingSettings CaseSensitive="false" />
                    <MasterTableView DataKeyNames="Subscriber" AllowMultiColumnSorting="False" AllowNaturalSort="false">
                        <CommandItemSettings ExportToPdfText="Export to PDF" />
                        <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                        </ExpandCollapseColumn>
                        <Columns>
                            <Rad:GridTemplateColumn Visible="false" AllowFiltering="false">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkRenewal" runat="server" OnCheckedChanged="ToggleRowSelection"
                                        AutoPostBack="True" />
                                </ItemTemplate>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="headerChkbox" runat="server" OnCheckedChanged="ToggleSelectedState"
                                        AutoPostBack="True" />
                                </HeaderTemplate>
                            </Rad:GridTemplateColumn>
                            <Rad:GridBoundColumn UniqueName="OrderID" HeaderText="Order ID" DataField="OrderID"
                                AutoPostBackOnFilter="true" SortExpression="OrderID" CurrentFilterFunction="Contains"
                                ShowFilterIcon="false">
                                <ItemStyle />
                            </Rad:GridBoundColumn>
                            <Rad:GridTemplateColumn Visible="false" DataField="AttendeeID" UniqueName="AttendeeID"
                                HeaderText="ID" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                ShowFilterIcon="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblId" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"AttendeeID") %>'> </asp:Label>
                                </ItemTemplate>
                            </Rad:GridTemplateColumn>
                            <Rad:GridTemplateColumn DataField="Subscriber" SortExpression="Subscriber" UniqueName="Subscriber"
                                HeaderText="Name" AllowFiltering="true" AutoPostBackOnFilter="true"
                                CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                <ItemTemplate>
                                    <%-- Neha,issue 16001,5/07/13, added css for image heightwidth and allignment of Name,Title,Adderess --%>
                                    <div class="row-div clearfix">
                                        
                                            <Rad:RadBinaryImage ID="RadBinaryImgPhoto" runat="server"
                                                AutoAdjustImageControlSize="false" ResizeMode="Fill" CssClass="img-float"/>
                                        
                                        
                                            <asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Subscriber") %>'></asp:Label><br />
                                            <asp:Label ID="lblTitle" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Title") %>'></asp:Label><br />
                                       
                                    </div>
                                </ItemTemplate>
                            </Rad:GridTemplateColumn>
                            <Rad:GridBoundColumn UniqueName="City" HeaderText="City" DataField="City" AutoPostBackOnFilter="true"
                                CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                            <Rad:GridBoundColumn UniqueName="MeetingTitle" HeaderText="Meeting Name" DataField="MeetingTitle"
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                            <Rad:GridTemplateColumn DataField="Status" UniqueName="Status" HeaderText="Status"
                                SortExpression="Status" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                ShowFilterIcon="false">
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                <HeaderStyle Font-Bold="true"></HeaderStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblstatus" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Status") %>'> </asp:Label>
                                </ItemTemplate>
                            </Rad:GridTemplateColumn>
                            <Rad:GridBoundColumn Visible="false" DataField="OrderDetailID" HeaderText="OrderDetailID"
                                SortExpression="OrderDetailID" UniqueName="OrderDetailID" AutoPostBackOnFilter="true"
                                CurrentFilterFunction="Contains" ShowFilterIcon="false">
                            </Rad:GridBoundColumn>
                            <Rad:GridTemplateColumn HeaderText="Badge Information" Visible="false" UniqueName="TemplateEditColumn"
                                AllowFiltering="false">
                                <ItemTemplate>
                                    <asp:LinkButton ID="PreviewLink" runat="server" Visible="false" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"OrderDetailID") %>'
                                        CommandName="Preview" Text="Preview"></asp:LinkButton>
                                    <asp:LinkButton ID="EditBagdeLink" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"OrderDetailID") %>'
                                        CommandName="EditBagde" Text="Preview/Edit" CausesValidation="false"></asp:LinkButton>
                                    <asp:Label ID="lblNotApplicable" runat="server" Text="Not Applicable" Visible="false"></asp:Label>
                                </ItemTemplate>
                            </Rad:GridTemplateColumn>
                        </Columns>
                        <EditFormSettings>
                            <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                            </EditColumn>
                        </EditFormSettings>
                        <ItemStyle />
                    </MasterTableView>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </Rad:RadGrid>
            </div>
            <div class="row-div">
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="row-div">
        <asp:Button ID="BtnBack" runat="server" class="submit-Btn" Text="Back" />
    </div>
</div>
<cc2:User ID="User1" runat="server" />
<asp:UpdatePanel runat="server" ID="UpEditBadgeInfo" UpdateMode="Always">
    <ContentTemplate>
        <Rad:RadWindow ID="UserListDialog"  runat="server"
            Modal="True" Skin="Default" VisibleStatusbar="False" Behaviors="None" 
             IconUrl=" " Title="Badge Information" CssClass="popup-badge-information">
            <ContentTemplate>
                <div class="tbale-div">
                        <div class="row-div">
                            <div class="lblBadgeInformation">
                            </div>
                        </div>
                        <div class="row-div clearfix">
                            <div class="label-div w25">
                                <span class="RequiredField">*</span>
                                Name:
                            </div>
                            <div class="field-div1 w74">
                                <asp:TextBox ID="txtBadgeName"  TabIndex="1" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row-div clearfix">
                            <div class="label-div w25">
                                 <span class="RequiredField">*</span>
                                Title:
                            </div>
                            <div class="field-div1 w74">
                                <asp:TextBox ID="txtBadgeTitle" runat="server"  TabIndex="2"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row-div clearfix">
                            <div class="label-div w25 ">
                                <span class="RequiredField">*</span>Company:
                            </div>
                            <div class="field-div1 w74">
                                <asp:TextBox ID="txtBadgeCompany" runat="server"  TabIndex="3"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row-div clearfix" runat="server" id="trMsg" visible="true">
                           
                            <div class="field-div1 ">
                                <asp:TextBox ID="txtTemp" Style="display: none;" CausesValidation="true" runat="server"
                                   TabIndex="3"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3"
                                    ValidationGroup="BadgeGroup" runat="server" ControlToValidate="txtTemp" 
                                    ErrorMessage="All the above fields are mandatory."></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row-div clearfix">
                            <div class="label-div w25">
                                &nbsp;
                            </div>
                            <div class="field-div1 w74">
                                <asp:Button ID="BtnUpdate" OnClientClick="return javascript:check();" class="submitBtn"
                                    TabIndex="27" runat="server" Text="Save" ValidationGroup="BadgeGroup"
                                    CausesValidation="true" />
                                <asp:Button ID="BtnCancel" runat="server" Text="Cancel" class="submit-Btn"
                                    TabIndex="28" OnClientClick="OnClientClick();" />
                            </div>
                        </div>
                  
                </div>
            </ContentTemplate>
        </Rad:RadWindow>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="UserListDialog" />
    </Triggers>
</asp:UpdatePanel>
