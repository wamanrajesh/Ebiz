<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="MeetingRegistration.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.ProductCatalog.MeetingRegistrationControl" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessShoppingCart" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagName="Meetings" TagPrefix="uc1" Src="~/UserControls/Aptify_Meetings/Meeting.ascx" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<div>
    <asp:UpdateProgress ID="updateProcessingIndicator" runat="server" AssociatedUpdatePanelID="updatePanelMain"
        DisplayAfter="0">
        <progresstemplate>
            <div class="processing-div">
                <div class="processing">
                    Please wait...
                </div>
            </div>
        </progresstemplate>
    </asp:UpdateProgress>
</div>
<asp:UpdatePanel ID="updatePanelMain" runat="server" ChildrenAsTriggers="True">
    <contenttemplate>
        <asp:Label ID="lblError" runat="server" Visible="False" CssClass="error-msg-label"></asp:Label>
        <div id="tblInner" runat="server">
            <div>
                <cc2:aptifyshoppingcart id="ShoppingCart1" runat="server" visible="False" />
                <asp:Label ID="lblTopMessageInfo" runat="server" CssClass="label" Text=" Please verify the attendee and complete the form below. When done, click Add Attendee   for registrants to the meeting."></asp:Label>
                <asp:ValidationSummary ID="vldSummary" ValidationGroup="Done" runat="server" CssClass="error-msg-label"></asp:ValidationSummary>
            </div>
            <div>
                <asp:Label ID="lblMeetingRegistrationError" runat="server" Visible="false" CssClass="error-msg-label"></asp:Label>
            </div>
        </div>
        <div id="meetingTitle" class="row-div">
            <asp:Label runat="server" ID="lblMeeting" Class="control-title" />
        </div>
        <div>
            <div class="row-div clearfix">
                <div class="label-div w19">
                    <asp:Label ID="lblPriceInfo" Text="Price:" runat="server"></asp:Label>
                </div>
                <div class="field-div1 w80">
                    <asp:Label ID="lblPrice" runat="server"></asp:Label>
                </div>
            </div>
            <div class="row-div clearfix">
                <div class="label-div w19">
                    <asp:Label ID="lblAvailableSpaceText" runat="server" Text=""></asp:Label>
                </div>
                <div class="field-div1 w80">
                    <asp:Label ID="lblAvailableSpace" runat="server" Text=""></asp:Label>
                </div>
            </div>
            <div class="row-div">
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </div>
        </div>
        <div id="trAttendee" runat="server" class="clearfix">
                <div class="float-left label">
                    Add Attendee
                </div>
                <div class="float-right label">
                    <span class="required-label">*</span>
                    <asp:Label ID="lblMandetoryInfo" runat="server" Text="Mandatory fields"></asp:Label>
                </div>
            </div>
        <div class="table-div border-color add-attendee-container">            
            <div class="row-div padding-all">
                <div class="float-left w33-3 top-margin">
                    <div class="row-div clearfix">
                        <div class="label-div w29">
                            <span class="required-label">*</span>First Name:
                        </div>
                        <div class="field-div1 w70">
                            <asp:TextBox ID="txtFirstName" runat="server" TabIndex="1"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="vldAttendeeFname" runat="server" ValidationGroup="Done"
                                Display="None" ControlToValidate="txtFirstName" ErrorMessage="Please specify an attendee First Name." CssClass="error-msg-label"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row-div clearfix">
                        <div class="label-div w29">
                            <span class="required-label">*</span>Last Name:
                        </div>
                        <div class="field-div1 w70">
                            <asp:TextBox ID="txtLastName" runat="server" TabIndex="2"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="vldAttendeeLname" runat="server" ValidationGroup="Done"
                                Display="None" ControlToValidate="txtLastName" ErrorMessage="Please specify an attendee Last Name." CssClass="error-msg-label"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row-div clearfix">
                        <div class="label-div w29">
                            <span class="required-label">*</span>Email:
                        </div>
                        <div class="field-div1 w70">
                            <asp:TextBox ID="txtEmail" runat="server" TabIndex="3"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfValidatorEmail" runat="server" ValidationGroup="Done"
                                Display="None" ControlToValidate="txtEmail" ErrorMessage="Please specify an attendee Email Address." CssClass="error-msg-label">
                                </asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="reValidatorEmail" runat="server" ErrorMessage="Please enter a valid Email Address."
                                ControlToValidate="txtEmail" ValidationGroup="Done" Display="None" ValidationExpression="[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+(?:[A-Z]{2}|com|COM|org|ORG|net|NET|edu|EDU|gov|GOV|mil|MIL|biz|BIZ|info|INFO|mobi|MOBI|name|NAME|aero|AERO|asia|ASIA|jobs|JOBS|museum|MUSEUM|in|IN|co|CO)\b" CssClass="error-msg-label">
                                </asp:RegularExpressionValidator>
                        </div>
                    </div>
                </div>
                <div class="float-left w33-3">
                    <fieldset class="border-color right-margin5 padding-all">
                        <legend class="label">Badge Information</legend>
                        <div class="row-div clearfix">
                            <div class="label-div w29">
                                Name:
                            </div>
                            <div class="field-div1 w70">
                                <asp:TextBox ID="txtBadgeName" TabIndex="5" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row-div clearfix">
                            <div class="label-div w29">
                                Title:
                            </div>
                            <div class="field-div1 w70">
                                <asp:TextBox ID="txtBadgeTitle" runat="server" TabIndex="6"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row-div clearfix">
                            <div class="label-div w29">
                                Company:
                            </div>
                            <div class="field-div1 w70">
                                <asp:TextBox ID="txtCompany" runat="server" TabIndex="7"></asp:TextBox>
                            </div>
                        </div>
                    </fieldset>
                </div>
                <div class="float-left w33-3">
                    <fieldset class="border-color right-margin5 padding-all">
                        <legend class="label">Attendee Preferences</legend>
                        <div class="row-div clearfix">
                            <div class="label-div w29">
                                Food Pref:
                            </div>
                            <div class="field-div1 w70">
                                <asp:DropDownList ID="ddlFoodPreference" runat="server" TabIndex="8">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row-div clearfix">
                            <div class="label-div w29">
                                Travel Pref:
                            </div>
                            <div class="field-div1 w70">
                                <asp:DropDownList ID="ddlTravelPreference" runat="server" TabIndex="9">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row-div clearfix">
                            <div class="label-div w29">
                                Golf Handicap:
                            </div>
                            <div class="field-div1 w70">
                                <asp:TextBox ID="txtGolfHandicape" runat="server" TabIndex="10"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Please enter a numeric value for Golf Handicap."
                                    ControlToValidate="txtGolfHandicape" ValidationGroup="Done" Display="None" ValidationExpression="^\d+(\.\d{1,4})?$" CssClass="error-msg-label"></asp:RegularExpressionValidator>
                            </div>
                        </div>
                        <div class="row-div clearfix">
                            <div class="label-div w29">
                                Special Request:
                            </div>
                            <div class="field-div1 w70">
                                <asp:TextBox ID="txtSpecialRequest" runat="server" TabIndex="11"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row-div clearfix">
                            <div class="label-div w29">
                                Other Pref:
                            </div>
                            <div class="field-div1 w70">
                                <asp:TextBox ID="txtOtherPreference" runat="server" TabIndex="12"></asp:TextBox>
                            </div>
                        </div>
                    </fieldset>
                </div>
                <div class="clear">
                </div>
                <div>
                    <asp:Button ID="btnAddInfo" ToolTip="Add attendee record to grid" ValidationGroup="Done"
                        CssClass="submit-Btn" TabIndex="13" runat="server" Text="Add Attendee" />
                </div>
            </div>
        </div>
        <div class="top-margin label">
            <asp:Label ID="lblAddedAtendee" runat="server" Text="Added Attendees"></asp:Label>
        </div>
        <div>
            <asp:Panel ID="pnlAddMember" runat="server" ScrollBars="Auto">
                <asp:GridView ID="grdAddMember" AutoGenerateColumns="false" runat="server" ShowFooter="False"
                    AllowPaging="false" CssClass="w100 gridview-table">
                    <HeaderStyle CssClass="grid-viewheader"/>        
                                <FooterStyle CssClass="grid-footer" />
                                <RowStyle CssClass="grid-item-style" />           
                                <PagerStyle CssClass="paging-style"/> 
                    <Columns>
                        <asp:BoundField DataField="RowNumber" HeaderText="Row Number" Visible="false" />
                        <asp:TemplateField>
                            <HeaderStyle  />
                            <HeaderTemplate>
                                <asp:Label ID="lblFname" Text="Name" runat="server"></asp:Label>
                            </HeaderTemplate>
                            <ItemStyle VerticalAlign="Middle" />
                            <ItemTemplate>
                                <asp:Label ID="lblAtendeeFirstName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"first Name")%>'></asp:Label>
                                <asp:Label ID="lblAtendeeLastName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Last Name")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle />
                            <HeaderTemplate>
                                <asp:Label ID="lblEmail" Text="Email" runat="server"></asp:Label>
                            </HeaderTemplate>
                            <ItemStyle/>
                            <ItemTemplate>
                                <asp:Label ID="lblAttendeeEmail" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Email")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <HeaderStyle />
                            <HeaderTemplate>
                                <asp:Label ID="lblBadgeInformation" Text="Badge Information" runat="server"></asp:Label>
                            </HeaderTemplate>
                            <ItemStyle />
                            <ItemTemplate>
                                <asp:Label ID="lblBadgeName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Badge Information Name")%>'></asp:Label><br />
                                <asp:Label ID="lblBadgeTitle" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Badge Information Title")%>'></asp:Label><br />
                                <asp:Label ID="lblBadgeCompany" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Badge Information Company")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle />
                            <HeaderTemplate>
                                <asp:Label ID="lbllstSessions" Text="List of Sessions for Attendee" runat="server"></asp:Label>
                            </HeaderTemplate>
                            <ItemStyle />
                            <ItemTemplate>
                                <asp:LinkButton ID="lnklstSessonForAttendee" Text="View/Edit Sessions" runat="server"
                                    CommandName="EditSession" CommandArgument='<%#Eval("RowNumber") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle  />
                            <HeaderTemplate>
                                <asp:Label ID="lblEditInformation" Text="Edit Attendee Info" runat="server"></asp:Label>
                            </HeaderTemplate>
                            <ItemStyle />
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEditAttendee" runat="server" CommandName="EditRow" CommandArgument='<%#Eval("RowNumber") %>'
                                    ImageUrl="~/Images/Edit.png" />
                                <asp:LinkButton ID="lnkEditInfo" Text="Edit" runat="server" CssClass="label_underline"
                                    CommandName="EditRow" CommandArgument='<%#Eval("RowNumber") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle  />
                            <HeaderTemplate>
                                <asp:Label ID="lblDelete" Text="Delete Attendee" runat="server"></asp:Label>
                            </HeaderTemplate>
                            <ItemStyle  />
                            <ItemTemplate>
                                <asp:ImageButton ID="btndelete" runat="server" ImageUrl="~/Images/Delete.png" CommandName="DeleteRow"
                                    CommandArgument='<%#Eval("RowNumber") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <HeaderStyle />
                            <ItemTemplate>
                                <asp:Label ID="lblFoodPreference1" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Attendee FoodPreferenceID")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <HeaderStyle  />
                            <ItemTemplate>
                                <asp:Label ID="plblTravelPreference" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Attendee TravelPreferenceID")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <HeaderStyle  />
                            <ItemTemplate>
                                <asp:Label ID="plblGolfHandicap" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Attendee GolfHandicap")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <HeaderStyle  />
                            <ItemTemplate>
                                <asp:Label ID="plblSpecialRequest" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Attendee SpecialRequest")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <HeaderStyle  />
                            <ItemTemplate>
                                <asp:Label ID="plblotherPreference" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Attendee OtherPreference")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
        </div>
        <div class="row-div top-margin clearfix">
            <div class="float-left label">
                <asp:LinkButton ID="lnkMeetingPage" Text="Back to Event Page" runat="server"></asp:LinkButton></div>
            <div class="float-right">
                <asp:Button ID="btnAddRegistrant" ValidationGroup="Add" CssClass="submit-Btn" runat="server"
                    TabIndex="14" Text="Proceed to checkout" />
            </div>
            <div class="float-right">
                <asp:Button ID="btnUpdateAttendeeInfo" ValidationGroup="Done" CssClass="submit-Btn"
                    runat="server" TabIndex="15" Text="Update" /></div>
        </div>
        <div>
            <asp:Label ID="lblAdded" Visible="False" runat="server"></asp:Label>
        </div>
    </contenttemplate>
    <triggers>
        <asp:PostBackTrigger ControlID="btnAddRegistrant" />
        <asp:PostBackTrigger ControlID="lnkMeetingPage" />
        <asp:PostBackTrigger ControlID="btnUpdateAttendeeInfo" />
        <asp:PostBackTrigger ControlID="grdAddMember" />
    </triggers>
</asp:UpdatePanel>
<asp:UpdatePanel ID="UpdatePanel4" runat="server">
    <contenttemplate>
        <rad:radwindow id="popEditAttendee" runat="server" class="edit-attendee-popup" modal="True"
            skin="Default" visiblestatusbar="False" behaviors="None" title="Edit Attendee"
            behavior="None" iconurl="~/Images/Attendee_16.png">
            <ContentTemplate>
                <div class="padding-all edit-attendee-container">
                    <div class="row-div">
                        <asp:Label ID="lblPopErrorMessage" Text="" Visible="false" CssClass="error-msg-label" runat="server"></asp:Label>
                    </div>
                    <div class="row-div clearfix">
                    <div class="float-left w49">
                        <div class="row-div clearfix">
                            <div class="label-div w25">
                                <span class="required-label">*</span> First Name:
                            </div>
                            <div class="field-div1 w74">
                                <asp:TextBox ID="txtPopFirstName" TabIndex="20"
                                    runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row-div clearfix">
                            <div class="label-div w25">
                                <span class="required-label">*</span> Last Name:
                            </div>
                            <div class="field-div1 w74">
                                <asp:TextBox ID="txtPopLastName" runat="server" TabIndex="21"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row-div clearfix">
                            <div class="label-div w25">
                                <span class="required-label">*</span> Email:
                            </div>
                            <div class="field-div1 w74">
                                <asp:TextBox ID="txtPopEmail" runat="server" TabIndex="22"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"  CssClass="error-msg-label" ErrorMessage="Please enter a valid Email Address."
                                    ControlToValidate="txtPopEmail" ValidationGroup="EditProfileControl" Display="None"
                                    ValidationExpression="[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+(?:[A-Z]{2}|com|COM|org|ORG|net|NET|edu|EDU|gov|GOV|mil|MIL|biz|BIZ|info|INFO|mobi|MOBI|name|NAME|aero|AERO|asia|ASIA|jobs|JOBS|museum|MUSEUM|in|IN|co|CO)\b"></asp:RegularExpressionValidator>
                            </div>
                        </div>
                    </div>
                    <div class="float-right w49">
                        <div class="label brown-txt bottom-margin">Badge Information</div>
                       <div class="row-div clearfix">
                            <div class="label-div w25">
                                Name:
                            </div>
                            <div class="field-div1 w74">
                                <asp:TextBox ID="txtPopBadgeName" TabIndex="24" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row-div clearfix">
                            <div class="label-div w25">
                                Title:
                            </div>
                            <div class="field-div1 w74">
                                <asp:TextBox ID="txtPopBadgeTitle" runat="server" TabIndex="25"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row-div clearfix">
                            <div class="label-div w25">
                                Company:
                            </div>
                            <div class="field-div1 w74">
                                <asp:TextBox ID="txtPopBadgeCompany" runat="server" TabIndex="26"></asp:TextBox>
                            </div>
                        </div>
                        
                    </div>
                    </div>
                    <div class="clear"></div>
                    <div class="row-div clearfix">
                        <div class="label brown-txt bottom-margin" >Attendee Preference</div>
                        
                    <div class="float-left w49">
                        <div class="row-div clearfix">
                            <div class="label-div w25">
                                Food Pref:
                            </div>
                            <div class="field-div1 w74">
                                <asp:DropDownList ID="ddlPopFoodPreference" runat="server" TabIndex="27">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row-div clearfix">
                            <div class="label-div w25">
                                Golf Handicap:
                            </div>
                            <div class="field-div1 w74">
                                <asp:TextBox ID="txtPopGolfHandicap" runat="server" TabIndex="29"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row-div clearfix">
                            <div class="label-div w25">
                                Other Pref:
                            </div>
                            <div class="field-div1 w74">
                                <asp:TextBox ID="txtPopOtherPreference" runat="server" TabIndex="31"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="float-right w49">
                        <div class="row-div clearfix">
                            <div class="label-div w25">
                                Travel Pref:
                            </div>
                            <div class="field-div1 w74">
                                <asp:DropDownList ID="ddlPopTravelPreference" runat="server" TabIndex="28">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row-div clearfix">
                            <div class="label-div w25">
                                Special Request:
                            </div>
                            <div class="field-div1 w74">
                                <asp:TextBox ID="txtPopSpecialRequest" runat="server" TabIndex="30"></asp:TextBox>
                            </div>
                        </div>
                        
                    </div> 
                    <div class="clear"></div>
                    <div class="row-div top-margin clearfix">
                            <div class="align-right">
                                <asp:Button ID="btnPopUpOk" class="submit-Btn" TabIndex="32" runat="server"
                                    Text="Save" ValidationGroup="DoneOnEdit" />
                                <asp:Button ID="btnPopUpCancel" runat="server" Text="Cancel" class="submit-Btn"
                                    TabIndex="33" />
                            </div>
                            <div>
                                <asp:HiddenField runat="server" ID="hgrdindex" />
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </rad:radwindow>
        <rad:radwindow id="radDuplicateUser" runat="server" class="alert-popup" modal="True"
            skin="Default" visiblestatusbar="False" behaviors="None" iconurl="~/Images/Alert.png"
            title="Alert" behavior="None">
            <ContentTemplate>
                <div class="row-div">
                    <div class="label">
                            <asp:Label ID="lblAlert" runat="server" Text=""></asp:Label>
                        </div>
                    <div class="align-center">
                            <asp:Button ID="btnok" runat="server" Text="OK"  class="submit-Btn" OnClick="btnok_Click"
                                ValidationGroup="ok" />
                        </div>
                </div>
            </ContentTemplate>
        </rad:radwindow>
        <rad:radwindow id="radSimilarRecords" runat="server" CssClass="popup-rad-similar-records"
            skin="Default" modal="True" visiblestatusbar="False" behaviors="None"
            iconurl="~/Images/crossdelete.png" title="Error" behavior="None">
            <ContentTemplate>
                <div class="row-div">
                  This Email ID Already Exists. Is this the person you are looking for?
                </div>
                <div class="row-div clearfix">
                   
                        <asp:GridView ID="grdmember" AutoGenerateColumns="false" runat="server" ShowFooter="False"
                            AllowPaging="true" PageSize="5">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderText="ID" Visible="false" />
                                <asp:TemplateField HeaderText="Member">
                                    <ItemTemplate>
                                        <div class="label-div ">
                                            <asp:Image ID="imgmember"  runat="server" />
                                        </div>
                                        <div class="label">
                                            <asp:Label ID="lblMember" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"FirstLast") %>'></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                   
                </div>
                <div class="row-div clearfix">
                    <div class="align-right">
                        <asp:Button ID="btnGetData" runat="server" Text="Yes" class="submit-Btn" />
                        <asp:Button ID="btnNo" runat="server" Text="No" class="submit-Btn" />
                    </div>
                </div>
            </ContentTemplate>
        </rad:radwindow>
        <rad:radwindow id="radValidateGrdRec" runat="server" CssClass="popup-validate-grd-rec"
            modal="True" skin="Default" visiblestatusbar="False" behaviors="None"
            iconurl="~/Images/Alert.png" title="Alert" behavior="None">
            <ContentTemplate>
             
                    <div class="row-div clearfix">
                        <div class="align-left label">
                            <asp:Label ID="Label1" runat="server" Text="This Attendee is already added. Please use a different email address or contact Customer Service for assistance."></asp:Label>
                        </div>
                   
                    <div class="align-center label">
                        <asp:Button ID="btnVGROk" runat="server" Text="OK" class="submit-Btn" OnClick="btnVGROk_Click" ValidationGroup="ok" />
                       
                    </div>
                </div>
            </ContentTemplate>
        </rad:radwindow>
        <rad:radwindow id="radChangeEmail" runat="server" CssClass="popup-change-email" modal="True"
            skin="Default" visiblestatusbar="False" behaviors="None" iconurl="~/Images/Alert.png" title="Alert" behavior="None">
            <ContentTemplate>
                <div class="row-div">
                    <div class="align-center label">
                        <asp:Label ID="Label2" runat="server" Text="Please use different Email ID."></asp:Label>
                    </div>
                    <div class="align-center">
                        <asp:Button ID="btnChangeEamilOk" runat="server" Text="OK" class="submit-Btn"
                            OnClick="btnChangeEamilOk_Click" ValidationGroup="ok" />
                    </div>
                </div>
            </ContentTemplate>
        </rad:radwindow>
        <rad:radwindow id="radPopUpEditListSession" runat="server" 
            CssClass="popup-edit-list-session" skin="Default" modal="True" 
            visiblestatusbar="False" behaviors="None" iconurl="~/Images/edit-sessions.png"
            title="Edit List of Sessions" behavior="None">
            <ContentTemplate>
               
                     <div class="row-div clearfix">
                       <div class="label-div w18">
                            Attendee Name:
                        </div>
                         <div class="field-div1 w20">
                            <asp:Label ID="lblAttendeeName" runat="server"></asp:Label>
                        </div>
                        <div class="label-div1 w12-5">
                            Email ID:
                        </div>
                        <div class="field-div2 w32">
                            <asp:Label ID="lblEmailID" runat="server"></asp:Label>
                        </div>
                     </div>
                    <div>
                        <asp:HiddenField ID="hdnGrdRowIndex" runat="server" Visible="false" Value="" />
                    </div>
                    <div Class="grid_height">
                        <rad:RadGrid ID="grdMeetingSession" runat="server" AutoGenerateColumns="False"
                            AllowFilteringByColumn="false" AllowSorting="True" AllowPaging="true" PagerStyle-AlwaysVisible="true">
                            <GroupingSettings CaseSensitive="false" />
                            <GroupingSettings CaseSensitive="false" />
                            <GroupingSettings CaseSensitive="false" />
                            <GroupingSettings CaseSensitive="false" />
                            <MasterTableView AllowFilteringByColumn="false" AllowSorting="True">
                                <Columns>
                                    <rad:GridTemplateColumn HeaderText="Select" AllowFiltering="false">
                                        <ItemStyle></ItemStyle>
                                        <HeaderStyle/>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkAllSession" runat="server" OnCheckedChanged="ToggleSelectedState"
                                                AutoPostBack="True" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSession" runat="server" />
                                        </ItemTemplate>
                                    </rad:GridTemplateColumn>
                                    <rad:GridTemplateColumn HeaderText="Session" AllowFiltering="true" DataField="WebName"
                                        SortExpression="WebName" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="lnkWebName"  runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"WebName") %>'></asp:HyperLink></ItemTemplate>
                                        <ItemStyle/>
                                    </rad:GridTemplateColumn>
                                    <rad:GridBoundColumn UniqueName="gridDateTimeColumnStartDate" HeaderText="Start Date & Time"
                                        DataField="StartDate" AllowFiltering="true"  SortExpression="StartDate"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                                    <rad:GridBoundColumn UniqueName="gridDateTimeColumnEndDate" HeaderText="End Date & Time"
                                        DataField="EndDate" AllowFiltering="true"  SortExpression="EndDate"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                                    <rad:GridTemplateColumn HeaderText="Place" AllowFiltering="true" DataField="Location"
                                        SortExpression="Location" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLocation" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Location") %>'></asp:Label><asp:Label
                                                ID="lblProductID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem,"ProductID") %>'></asp:Label></ItemTemplate>
                                        <ItemStyle  />
                                    </rad:GridTemplateColumn>
                                    <rad:GridBoundColumn DataField="Price" HeaderText="Registration Price" 
                                         AllowFiltering="true" SortExpression="Price" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false" />
                                </Columns>
                            </MasterTableView>
                        </rad:RadGrid>
                    </div>
                    <div class="row-div top-margin align-right">
                        <asp:Button ID="btnEditSession" CssClass="submit-Btn" TabIndex="32"
                            runat="server" Text="OK" />
                        <asp:Button ID="btnCancelSession" runat="server" Text="Cancel" CssClass="submit-Btn"
                            TabIndex="33" />
                    </div>
            
            </ContentTemplate>
        </rad:radwindow>
        <rad:radwindow id="radMeetingSessionCountInfo" runat="server" CssClass="popup-meeting-session-count-info"
            modal="True" skin="Default"  visiblestatusbar="False" behaviors="None"
            iconurl="~/Images/Alert.png" title="Alert" behavior="None">
            <ContentTemplate>
                <div class="tblEditAtendee" cellpadding="0" cellspacing="0">
                  
                        <div class="align-left label">
                            <asp:Label ID="lblMeetingCountZero" runat="server" Text="There are no sessions associated with this meeting."></asp:Label>
                        </div>
                 
                        <div class="align-center">
                            <asp:Button ID="btnMeetingSessionCountInfo" runat="server" Text="OK" class="submit-Btn"
                                ValidationGroup="ok" />
                        </div>
                  
                </div>
            </ContentTemplate>
        </rad:radwindow>
        <rad:radwindow id="radMeetingSessionConflictMessage" runat="server" modal="True"
            CssClass="popup-win-meeting-conflicts" skin="Default" visiblestatusbar="False" behaviors="None"  iconurl="~/Images/Alert.png"
            title="Alert" behavior="None">
            <ContentTemplate>
                <div class="errormsg-div">
                    <asp:ListView ID="lstErrorMessage" runat="server">
                        <ItemTemplate>
                            <asp:Label ID="lblErrorMessage" runat="server" Text='<% #eval("ErrorMessage") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
                <div class="row-div">
                    <asp:Button ID="btnMeetingSessionConflictOK" runat="server" Text="OK" class="submit-Btn"
                        ValidationGroup="ok" />
                </div>
            </ContentTemplate>
        </rad:radwindow>
    </contenttemplate>
</asp:UpdatePanel>
<cc3:user id="User1" runat="server" />
