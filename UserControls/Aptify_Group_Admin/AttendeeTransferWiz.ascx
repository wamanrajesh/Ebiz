<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="AttendeeTransferWiz.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.AttendeeTransferWiz" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="uc1" TagName="CreditCard" Src="../Aptify_General/CreditCard.ascx" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessShoppingCart" %>
<script type="text/javascript">
    function SetUniqueRadioButton(current) {

        for (i = 0; i < document.forms[0].elements.length; i++) {
            elm = document.forms[0].elements[i]
            if (elm.type == 'radio') {
                elm.checked = false;
            }
        }

        current.checked = true;
    }

</script>
<div class="wizard-div">
<div class="table-div">
    <div class="row-div errormsg-div">
        <asp:Label ID="lblError" runat="server">
        </asp:Label>
    </div>
    <div class="row-div clearfix">
        <asp:Wizard ID="WizardMeetingTransfer" runat="server">
            <SideBarButtonStyle />
            <SideBarStyle VerticalAlign="Top" CssClass="admin-wizard-sidebar" />
            <HeaderStyle></HeaderStyle>
            <SideBarButtonStyle />
            <NavigationButtonStyle CssClass="submit-Btn" />
            <NavigationButtonStyle BorderColor="ActiveCaption" />
            <WizardSteps>
                <asp:WizardStep ID="WizardStep1" runat="server" Title="1. Select a Meeting ">
                    <div class="wizard-data-margin ">
                        <div class="row-div label">
                            <asp:Label ID="lblStep1" runat="server" Text="Step 1: Select a Meeting/Session">
                            </asp:Label>
                        </div>
                        <asp:Label ID="lblStep1Msg" runat="server" Text="Meetings/Sessions Registrations">
                        </asp:Label>
                        <asp:UpdatePanel ID="upnlUpcomingMeeting" runat="server" ChildrenAsTriggers="false"
                            UpdateMode="Conditional">
                            <ContentTemplate>
                                <rad:radgrid id="grdUpcomingMeeting" runat="server" allowpaging="true" allowsorting="true"
                                    sortingsettings-sorteddesctooltip="Sorted Descending" sortingsettings-sortedasctooltip="Sorted Ascending"
                                    gridlines="None" autopostback="true" allowfilteringbycolumn="true" autogeneratecolumns="false"
                                    onitemcreated="grdUpcomingMeeting_GridItemCreated">
                                    <GroupingSettings CaseSensitive="false" />
                                    <MasterTableView AllowNaturalSort="false" ClientDataKeyNames="MeetingID">
                                        <NoRecordsTemplate>
                                            No Meeting Available.
                                        </NoRecordsTemplate>
                                        <Columns>
                                            <rad:GridTemplateColumn HeaderText="Select" AllowFiltering="false">
                                                <ItemTemplate>
                                                    <asp:RadioButton ID="optSelectMeeting" runat="server" Onclick="SetUniqueRadioButton(this)" />
                                                </ItemTemplate>
                                            </rad:GridTemplateColumn>
                                            <rad:GridBoundColumn DataField="MeetingID" Visible="false">
                                            </rad:GridBoundColumn>
                                            <rad:GridTemplateColumn HeaderText="MeetingID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMeetingID" runat="server" Text='
																	<%# DataBinder.Eval(Container.DataItem, "MeetingID") %>
																		'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </rad:GridTemplateColumn>
                                            <rad:GridTemplateColumn Visible="True" HeaderText="Meeting" DataField="Meeting" SortExpression="Meeting"
                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"
                                               >
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMeetingName" runat="server" Text='
																		<%# DataBinder.Eval(Container.DataItem, "Meeting") %>
																			' />
                                                </ItemTemplate>
                                            </rad:GridTemplateColumn>
                                            <rad:GridTemplateColumn HeaderText="Is Session" AllowFiltering="false">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkIsSession" runat="server" Checked='
																			<%# DataBinder.Eval(Container.DataItem, "IsSession") %>
																				' Enabled="false" />
                                                </ItemTemplate>
                                            </rad:GridTemplateColumn>
                                            <rad:GridDateTimeColumn Visible="True" HeaderText="Start Date" DataField="StartDate"
                                                UniqueName="GridDateTimeColumnStartDate" SortExpression="StartDate" AutoPostBackOnFilter="true"
                                                CurrentFilterFunction="EqualTo" ShowFilterIcon="false" DataType="System.DateTime"
                                                EnableTimeIndependentFiltering="true"  FilterControlToolTip="Select a Filter Date">
                                            </rad:GridDateTimeColumn>
                                            <rad:GridTemplateColumn Visible="true" HeaderText="Venue" DataField="VENUE" SortExpression="VENUE"
                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"
                                               >
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMeetingVenue" runat="server" Text='
																				<%# DataBinder.Eval(Container.DataItem, "VENUE") %>
																					' />
                                                </ItemTemplate>
                                            </rad:GridTemplateColumn>
                                            <rad:GridTemplateColumn HeaderText="ParentID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblParentID" runat="server" Text='
																					<%# DataBinder.Eval(Container.DataItem, "ParentID") %>
																						'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </rad:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                </rad:radgrid>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="grdUpcomingMeeting" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </asp:WizardStep>
                <asp:WizardStep ID="WizardStep2" runat="server" Title="2. Select an Existing Attendee ">
                    <div class="wizard-data-margin ">
                        <div class="row-div label">
                            <asp:Label ID="lblStep2" runat="server" Text="Step 2: Select an Attendee">
                            </asp:Label>
                        </div>
                        <asp:Label ID="lblMeetingTitle" runat="server" Text="">
                        </asp:Label>
                        <asp:UpdatePanel ID="upnlMeetingRegistrant" runat="server" ChildrenAsTriggers="false"
                            UpdateMode="Conditional">
                            <ContentTemplate>
                                <rad:radgrid id="grdMeetingRegistrant" runat="server" allowpaging="true" allowsorting="true"
                                    sortingsettings-sorteddesctooltip="Sorted Descending" sortingsettings-sortedasctooltip="Sorted Ascending"
                                    autopostback="true" allowfilteringbycolumn="true" autogeneratecolumns="false"
                                    cellspacing="0" gridlines="None" onitemcreated="grdMeetingRegistrant_GridItemCreated">
                                    <GroupingSettings CaseSensitive="false" />
                                    <MasterTableView  AllowNaturalSort="false" ClientDataKeyNames="AttendeeID">
                                        <NoRecordsTemplate>
                                            No Attendee Available.
                                        </NoRecordsTemplate>
                                        <Columns>
                                            <rad:GridTemplateColumn HeaderText="Select" AllowFiltering="false">
                                                <ItemTemplate>
                                                    <asp:RadioButton ID="optSelectAttendee" runat="server" Onclick="SetUniqueRadioButton(this)" />
                                                </ItemTemplate>
                                            </rad:GridTemplateColumn>
                                            <rad:GridTemplateColumn HeaderText="Photo" AllowFiltering="false">
                                                <ItemTemplate>
                                                    <div>
                                                        <rad:RadBinaryImage ID="ImgAttendeePhoto" runat="server"  AutoAdjustImageControlSize="false">
                                                        </rad:RadBinaryImage>
                                                    </div>
                                                </ItemTemplate>
                                            </rad:GridTemplateColumn>
                                            <rad:GridBoundColumn DataField="AttendeeID" Visible="false">
                                            </rad:GridBoundColumn>
                                            <rad:GridTemplateColumn HeaderText="ID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAttendeeID" runat="server" Text='
																						<%# DataBinder.Eval(Container.DataItem, "AttendeeID") %>
																							'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </rad:GridTemplateColumn>
                                            <rad:GridTemplateColumn HeaderText="Attendees" DataField="AttendeeID_FirstLast" SortExpression="AttendeeID_FirstLast"
                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAttendeeName" runat="server" Text='
																							<%# DataBinder.Eval(Container.DataItem, "AttendeeID_FirstLast") %>
																								'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </rad:GridTemplateColumn>
                                            <rad:GridTemplateColumn HeaderText="Title" DataField="Title" SortExpression="Title"
                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAttendeeTitle" runat="server" Text='
																								<%# DataBinder.Eval(Container.DataItem, "Title") %>
																									'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </rad:GridTemplateColumn>
                                            <rad:GridTemplateColumn HeaderText="Status" DataField="AttendeeStatus_Name" SortExpression="AttendeeStatus_Name"
                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAttendeeStatus" runat="server" Text='
																									<%# DataBinder.Eval(Container.DataItem, "AttendeeStatus_Name") %>
																										'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </rad:GridTemplateColumn>
                                            <rad:GridTemplateColumn Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStatusID" runat="server" Text='
																										<%# DataBinder.Eval(Container.DataItem, "StatusID") %>
																											'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </rad:GridTemplateColumn>
                                            <rad:GridTemplateColumn Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOrderID" runat="server" Text='
																											<%# DataBinder.Eval(Container.DataItem, "OrderID") %>
																												'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </rad:GridTemplateColumn>
                                            <rad:GridTemplateColumn Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblComapanyID" runat="server" Text='
																												<%# DataBinder.Eval(Container.DataItem, "CompanyID") %>
																													'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </rad:GridTemplateColumn>
                                            <rad:GridTemplateColumn Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOrderLineID" runat="server" Text='
																													<%# DataBinder.Eval(Container.DataItem, "OrderDetailID") %>
																														'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </rad:GridTemplateColumn>
                                            <rad:GridTemplateColumn HeaderText="City" DataField="City" SortExpression="City"
                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCity" runat="server" Text='
																														<%# DataBinder.Eval(Container.DataItem, "City") %>
																															'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </rad:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                </rad:radgrid>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="grdMeetingRegistrant" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </asp:WizardStep>
                <asp:WizardStep ID="WizardStep3" runat="server" Title="3. Replace Attendee ">
                    <div runat="server" id="DivStep3" class="wizard-data-margin ">
                        <div class="row-div label">
                            <asp:Label ID="lblStep3ReplaceAttendee" runat="server" Text="Step 3: Replace Attendee">
                            </asp:Label>
                        </div>
                        <asp:Label ID="lblStep3" runat="server" Text="Select a person who is not currently registered to attend the meeting in place of the attendee you selected in Step 2.">
                        </asp:Label>
                        <asp:UpdatePanel ID="upnlWaitingList" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                            <ContentTemplate>
                                <rad:radgrid id="grdWaitingList" runat="server" allowpaging="true" allowfilteringbycolumn="true"
                                    allowsorting="true" sortingsettings-sorteddesctooltip="Sorted Descending" sortingsettings-sortedasctooltip="Sorted Ascending"
                                    autopostback="true" autogeneratecolumns="false" cellspacing="0" gridlines="None"
                                    onitemcreated="grdWaitingList_GridItemCreated">
                                    <GroupingSettings CaseSensitive="false" />
                                    <MasterTableView AllowNaturalSort="false" ClientDataKeyNames="AttendeeID">
                                        <NoRecordsTemplate>
                                            No Attendee Available.
                                        </NoRecordsTemplate>
                                        <Columns>
                                            <rad:GridTemplateColumn HeaderText="Select" AllowFiltering="false">
                                                <ItemTemplate>
                                                    <asp:RadioButton ID="optSelectNewAttendee" runat="server" Onclick="SetUniqueRadioButton(this)" />
                                                </ItemTemplate>
                                            </rad:GridTemplateColumn>
                                            <rad:GridTemplateColumn HeaderText="Photo" AllowFiltering="false">
                                                <ItemTemplate>
                                                    <div>
                                                        <rad:RadBinaryImage ID="ImgNewAttendeePhoto" runat="server"
                                                            AutoAdjustImageControlSize="false"></rad:RadBinaryImage>
                                                    </div>
                                                </ItemTemplate>
                                            </rad:GridTemplateColumn>
                                            <rad:GridBoundColumn DataField="AttendeeID" Visible="false">
                                            </rad:GridBoundColumn>
                                            <rad:GridTemplateColumn HeaderText="ID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNewAttendeeID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "AttendeeID") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </rad:GridTemplateColumn>
                                            <rad:GridTemplateColumn HeaderText="Attendees" DataField="AttendeeID_FirstLast" SortExpression="AttendeeID_FirstLast"
                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNewAttendeeName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "AttendeeID_FirstLast") %>'>                                                   </asp:Label>
                                                </ItemTemplate>
                                            </rad:GridTemplateColumn>
                                            <rad:GridTemplateColumn HeaderText="Title" DataField="Title" SortExpression="Title"
                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNewAttendeeTitle" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Title") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </rad:GridTemplateColumn>
                                            <rad:GridTemplateColumn HeaderText="City" DataField="City" SortExpression="City"
                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCity" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "City") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </rad:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                </rad:radgrid>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="grdWaitingList" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </asp:WizardStep>
                <asp:WizardStep ID="WizardStep4" runat="server" Title="4. Review and Confirm Replacement">
                    <div class="wizard-data-margin ">
                        <div class="row-div label">
                            <asp:Label ID="lblStep4" runat="server" CssClass="label" Text="Step 4: Review and Confirm Replacement">
                            </asp:Label>
                        </div>
                        <div class="row-div label">
                            <asp:Label ID="lblNewPrice" Text="" runat="server">
                            </asp:Label>
                        </div>
                        <div class="row-div label">
                            <asp:Label ID="lblFinishmessage" Text="" runat="server">
                            </asp:Label>
                        </div>
                        <rad:radwindow id="CreditcardWindow" runat="server" visibleonpageload="false" modal="true"
                            behaviors="Move" title="Payment Information" visiblestatusbar="false" skin="Default"
                            iconurl="" class="payment-information-popup">
                            <ContentTemplate>
                                <div class="row-div">
                                    <asp:Label ID="lblBalance" runat="server" >
                                    </asp:Label>                                    
                                </div>
                                <div class="row-div clearfix">
                                    <uc1:CreditCard ID="CreditCard" runat="server" />
                                </div>
                                <div class="row-div">
                                    <asp:CheckBox ID="chkMakePayment" runat="server" Visible="false" />
                                    <asp:Button ID="btnOK" runat="server" Text="Make Payment" CssClass="submit-Btn" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false"
                                        CssClass="submit-Btn" />
                                </div>
                            </ContentTemplate>
                        </rad:radwindow>
                    </div>
                </asp:WizardStep>
            </WizardSteps>
        </asp:Wizard>
    </div>
</div>
<div id="tblTransferConfirmation" runat="server" class="order-confirmation">
    <div class="table-div">
        <div class="row-div">
            <asp:Label ID="lblCompleteMsg" runat="server" Text="">
            </asp:Label>
            <br />
            <asp:Label ID="Label2" runat="server" Text=" Select attendee to send Attendee Transfer Confirmation mail.">
            </asp:Label>
        </div>
        <div class="row-div success-msg">
            <asp:Label ID="SendEmailLabel" runat="server" Text="">
            </asp:Label>
        </div>
        <div class="row-div">
            <div class="row-div">
                <asp:CheckBox ID="chkSendMailtoAttendee" runat="server" Text=" Previous Attendee"
                    Onclick="ShownHideBtn()" />
            </div>
            <div class="row-div">
                <asp:CheckBox ID="chkSendMailtoNewAttendee" runat="server" Text=" New Attendee" Onclick="ShownHideBtn()" />
            </div>
            <div class="row-div">
                <asp:Button ID="btnSendMail" runat="server" Text="Send Mail" CssClass="submit-Btn"
                    ClientIDMode="Static" />
            </div>
        </div>
    </div>
    <div class="border-all padding-all clearfix">
        <div class="float-left w33-3">
            <asp:Image runat="server" ID="companyLogo" alt="CompanyLogo image URL not set" />
        </div>
        <div class="float-left label w33-3">
            <asp:Label ID="lblcompanyAddress" runat="server" Text="">
            </asp:Label>
        </div>
        <div class="float-left w33-3">
            <div class="label-div">
                Phone:
            </div>
            <div class="field-div1">
                (202) <span style="display: none;">_ </span>555-1234
            </div>
            <div class="clear"></div>
            <div class="label-div1">
                Fax:
            </div>
            <div class="field-div2">
                (202) <span style="display: none;">_ </span>555-4321
            </div>
            <div class="clear">
            </div>
        </div>
        <div class="clear"></div>
    </div>
    <div id="tblRowMain" runat="server" class="row-div clearfix">
        <div class="float-left w49">
            <div class="row-div clearfix">
                <div class="label-div w29">
                    Original Attendee:
                </div>
                <div class="field-div1 w70">
                    <asp:Label ID="lblOriginalAttendee" runat="server">
                    </asp:Label>
                </div>
            </div>
            <div class="row-div clearfix">
                <div class="label-div w29">
                    Order Number:
                </div>
                <div class="field-div1 w70">
                    <asp:Label ID="lblOriginalOrderID" runat="server">
                    </asp:Label>
                </div>
            </div>
            <div class="row-div clearfix">
                <div class="label-div w29">
                    Order Total:
                </div>
                <div class="field-div1 w70">
                    <asp:Label ID="lblOriOrderTotal" runat="server">
                    </asp:Label>
                </div>
            </div>
            <div class="row-div clearfix">
                <div class="label-div w29">
                    Order Balance:
                </div>
                <div class="field-div1 w70">
                    <asp:Label runat="server" ID="lblOriOrderBalance">
                    </asp:Label>
                </div>
            </div>
        </div>
        <div class="float-right w49">
            <div class="row-div clearfix">
                <div class="label-div w29">
                    New Attendee:
                </div>
                <div class="field-div1 w70">
                    <asp:Label runat="server" ID="lblNewAttendee">
                    </asp:Label>
                </div>
            </div>
            <div class="row-div clearfix">
                <div class="label-div w29">
                    <b>Order Number: </b>
                </div>
                <div class="field-div1 w70">
                    <asp:Label ID="lblNewOrderID" runat="server">
                    </asp:Label>
                    <br />
                </div>
            </div>
            <div class="row-div clearfix">
                <div class="label-div w29">
                    <b>Order Total: </b>
                </div>
                <div class="field-div1 w70">
                    <asp:Label ID="lblNewOrderTotal" runat="server">
                    </asp:Label>
                </div>
            </div>
            <div class="row-div clearfix">
                <div class="label-div w29">
                    <b>Order Balance: </b>
                </div>
                <div class="field-div1 w70">
                    <asp:Label ID="lblNewOrderBalance" runat="server">
                    </asp:Label>
                </div>
            </div>
            <div class="row-div clearfix">
                <div class="label-div w29">
                    <b>Amount Paid: </b>
                </div>
                <div class="field-div1 w70">
                    <asp:Label ID="lblAmountPaid" runat="server" Text="">
                    </asp:Label>
                </div>
            </div>
        </div>
    </div>
</div>
<cc1:user id="User1" runat="server" />
<cc2:aptifyshoppingcart runat="Server" id="ShoppingCart1" />
</div>
<script type="text/javascript">

    function ShownHideBtn() {
        var chk1 = document.getElementById('<%=chkSendMailtoAttendee.ClientID %>');
        var chk2 = document.getElementById('<%=chkSendMailtoNewAttendee.ClientID %>');
        var btn = document.getElementById('<%=btnSendMail.ClientID %>');
        btn.style.display = "none";
        if (chk1.checked == true || chk2.checked == true) {
            btn.style.display = "block";
        }
        else {
            btn.style.display = "none";
        }

    }
    ShownHideBtn();
</script>
