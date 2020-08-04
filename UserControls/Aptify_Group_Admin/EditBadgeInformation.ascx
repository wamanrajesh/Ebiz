<%--Aptify e-Business 5.5.1, July 2013--%>
<%@ Control Language="VB" AutoEventWireup="true" CodeFile="EditBadgeInformation.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.CustomerService.EditBadgeInformation" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<style>
    .rgCommandRow
    {
        display: none;
    }
</style>
<script language="javascript" type="text/javascript">

    function GetRadWindow() {

        var oWindow = null;

        if (window.radWindow)

            oWindow = window.RadWindow; //Will work in Moz in all cases, including clasic dialog      

        else if (window.frameElement.radWindow)

            oWindow = window.frameElement.radWindow; //IE (and Moz as well)      

        return oWindow;

    }

    function Close() {

        GetRadWindow().Close();

    }
    function check() {

        var Bname = document.getElementById("<%= txtBadgeName.ClientId %>").value;
        var Btitle = document.getElementById("<%= txtBadgeTitle.ClientId %>").value;
        var Bcompany = document.getElementById("<%= txtBadgeCompany.ClientId %>").value;
        
               
        if (Bname != '' || Btitle != '' || Bcompany != '') {

            document.getElementById("<%= txtTemp.ClientId %>").value = Bname;
            return false;

        }
        else {
            document.getElementById("<%= txtTemp.ClientId %>").value = '';
            return true;
        }

    }

 
</script>
<div>
</div>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div style="padding: 10px;">
            <table width="100%">
                <tr>
                    <td align="left">
                        <asp:Label ID="lblRegistrationResult" class="MeetingHeader" runat="server" Style="font-weight: 700"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Label ID="lblRegistrationTitle" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadGrid OnItemCreated="RadgrdBadgeInformation_ItemCreated" DataKeyNames="AttendeeID"
                            CommandArgument="OrderDetailID" ID="RadgrdBadgeInformation" runat="server" AllowPaging="True"
                            Width="100%" AllowFilteringByColumn="true">
                            <GroupingSettings CaseSensitive="false" />
                            <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                            <MasterTableView AutoGenerateColumns="False" DataKeyNames="OrderDetailID" ClientDataKeyNames="AttendeeID"
                                CommandItemDisplay="Top" AllowFilteringByColumn="true" AllowSorting="true">
                                <CommandItemSettings ShowAddNewRecordButton="false" />
                                <Columns>
                                    <telerik:GridBoundColumn Visible="false" DataField="AttendeeID" HeaderText="Attendee ID"
                                        ReadOnly="True" SortExpression="AttendeeID" UniqueName="AttendeeID" AutoPostBackOnFilter="true"
                                        CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="OrderID" HeaderText="Order ID" SortExpression="OrderID"
                                        UniqueName="OrderID" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn DataField="Name" UniqueName="Name" HeaderText="Name"
                                        SortExpression="Name" FilterControlWidth="200px" AllowFiltering="true" AutoPostBackOnFilter="true"
                                        CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                        <ItemTemplate>
                                            <div>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <div>
                                                                <asp:Image ID="RadBinaryImgPhoto" CssClass="imgmember" runat="server" />
                                                                <%-- <telerik:RadBinaryImage runat="server" ID="RadBinaryImgPhoto" DataValue='<%#IIf(Typeof(Eval("photo")) is DBNull, Nothing, Eval("photo"))%>'
                                                                AutoAdjustImageControlSize="false" Width="60px" Height="60px" ToolTip='<%#Eval("Name", "Photoof {0}") %>'
                                                                AlternateText='<%#Eval("Name", "Photoof {0}") %>' />--%></div>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Label><br />
                                                            <asp:Label ID="lblTitle" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Title") %>'></asp:Label><br />
                                                            <%--
                                                        <ul >
                                                            <li >
                                                                <label>
                                                                </label>
                                                                <%#Eval("Name")%>
                                                            </li>
                                                            <li>
                                                                <label>
                                                                </label>
                                                                <%#Eval("Title")%>
                                                            </li>
                                                        </ul>--%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="Email" HeaderText="Email" ItemStyle-CssClass="Emailstyle"
                                        SortExpression="Email" FilterControlWidth="200px" UniqueName="Email" AutoPostBackOnFilter="true"
                                        CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="City" HeaderText="City" SortExpression="City"
                                        UniqueName="City" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Status" HeaderText="Status" SortExpression="Status"
                                        UniqueName="Status" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false">
                                    </telerik:GridBoundColumn>
                                    <%--  <telerik:GridTemplateColumn HeaderText="Status" SortExpression="Status" AutoPostBackOnFilter="true"
                                CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                <ItemStyle HorizontalAlign="Center" Width="50px" CssClass="LeftAlign"></ItemStyle>
                                <ItemTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblstatus" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Status") %>'> </asp:Label>
                                            </td>
                                            <td>
                                                <telerik:RadBinaryImage ID="imgestatusID" Visible="false" CssClass="imgstaus" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>--%>
                                    <telerik:GridBoundColumn Visible="false" DataField="OrderDetailID" HeaderText="OrderDetailID"
                                        SortExpression="OrderDetailID" UniqueName="OrderDetailID" AutoPostBackOnFilter="true"
                                        CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn HeaderText="Badge Information" UniqueName="TemplateEditColumn"
                                        AllowFiltering="false">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="PreviewLink" runat="server" Visible="false" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"OrderDetailID") %>'
                                                CommandName="Preview" Text="Preview"></asp:LinkButton>
                                            <asp:LinkButton ID="EditBagdeLink" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"OrderDetailID") %>'
                                                CommandName="EditBagde" Text="Preview/Edit" CausesValidation="false"></asp:LinkButton>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                            <ClientSettings>
                                <Selecting AllowRowSelect="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:UpdatePanel runat="server" ID="UpEditBadgeInfo" UpdateMode="Conditional">
                            <ContentTemplate>
                                <telerik:RadWindow ID="UserListDialog" CssClass="EditBadgeInforwIcon" runat="server"
                                    Modal="True" Skin="Default" BackColor="#f4f3f1" VisibleStatusbar="False" Behaviors="None"
                                    Height="180px" Width="280px" ForeColor="#C59933" IconUrl=" " Title="Badge Information">
                                    <ContentTemplate>
                                        <div style="background-color: #f4f3f1; padding: 5px;">
                                            <table cellpadding="0" cellspacing="0" style="background-color: #f4f3f1;">
                                                <tr>
                                                    <td class="lblBadgeInformation" colspan="3">
                                                    </td>
                                                    <%--  <td></td>--%>
                                                </tr>
                                                <tr style="padding-bottom: 5px;">
                                                    <td style="padding-bottom: 5px;" class="rightAlign">
                                                        <b></b>
                                                    </td>
                                                    <td style="padding-bottom: 5px;" class="rightAlign">
                                                        <b>
                                                            <asp:Label runat="server" ID="lbl" Text="*" ForeColor="red"></asp:Label>Name:</b>
                                                    </td>
                                                    <td style="padding-bottom: 5px;" class="width: 268px">
                                                        &nbsp;<asp:TextBox ID="txtBadgeName" Width="180px" TabIndex="1" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr style="padding-bottom: 5px;">
                                                    <td style="padding-bottom: 5px;" class="rightAlign">
                                                        <b></b>
                                                    </td>
                                                    <td style="padding-bottom: 5px;" class="rightAlign">
                                                        <b>
                                                            <asp:Label runat="server" ID="Label1" Text="*" ForeColor="red"></asp:Label>Title:</b>
                                                    </td>
                                                    <td style="padding-bottom: 5px;" class="width: 268px">
                                                        &nbsp;<asp:TextBox ID="txtBadgeTitle" runat="server" Width="180px" TabIndex="2"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr style="padding-bottom: 5px;">
                                                    <td style="padding-bottom: 5px;" class="rightAlign">
                                                        <b></b>
                                                    </td>
                                                    <td style="padding-bottom: 5px;" class="rightAlign">
                                                        <b>
                                                            <asp:Label runat="server" ID="Label2" Text="*" ForeColor="red"></asp:Label>Company:</b>
                                                    </td>
                                                    <td style="padding-bottom: 5px;" class="width: 268px">
                                                        &nbsp;<asp:TextBox ID="txtBadgeCompany" runat="server" Width="180px" TabIndex="3"></asp:TextBox>
                                                       
                                                    </td>
                                                </tr>
                                                <tr runat="server" id="trMsg" visible="true">
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtTemp" Style="display:none;" CausesValidation="true" runat="server" Width="180px" TabIndex="3"></asp:TextBox>
                                                        <asp:RequiredFieldValidator Style="margin-left: 66px;" ID="RequiredFieldValidator3"
                                                            ValidationGroup="BadgeGroup" runat="server" ControlToValidate="txtTemp" ForeColor="Red"
                                                            ErrorMessage="All the above fields are mandatory."></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td align="right" style="padding-top: 6px;" class="width: 268px;">
                                                        <asp:Button ID="BtnUpdate" OnClientClick="return javascript:check();" class="submitBtn"
                                                            TabIndex="27" runat="server" Width="80px" Text="Save" ValidationGroup="BadgeGroup"
                                                            CausesValidation="true" />
                                                        <asp:Button ID="BtnCancel" Width="80px" runat="server" Text="Cancel" class="submitBtn"
                                                            TabIndex="28" OnClientClick="OnClientClick();" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </ContentTemplate>
                                </telerik:RadWindow>
                            </ContentTemplate>
                            <%-- OnClick="BtnUpdate_Click"
          OnClick="BtnCancel_Click"--%>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="UserListDialog" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
            </table>
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="RadgrdBadgeInformation" />
    </Triggers>
</asp:UpdatePanel>
<div>
    <cc2:User ID="User1" runat="server" />
    <table style="padding: 10px;">
        <tr>
            <td>
                <asp:Button ID="BtnBack" runat="server" CssClass="submitBtn" Text="Back To Registrations" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
    </table>
</div>
