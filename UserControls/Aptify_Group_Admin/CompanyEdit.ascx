<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="CompanyEdit.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.CustomerService.CompanyEdit" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="cc2" TagName="Topiccode" Src="~/UserControls/Aptify_General/TopicCodeViewer.ascx" %>
<%-- 'Anil B for issue 14320 on 09/04/2013
      Used update panel to avaid full loading of full control--%>
<asp:UpdatePanel ID="UpdatePanel2" runat="server">
    <ContentTemplate>
        <div class="contact-info-container">
            <div class="contact-info-title clearfix">
                <div class="float-left">
                    Company Address</div>
                <div class="float-right right-margin">
                    <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/Edit.png" />
                    <asp:LinkButton ID="contact" CssClass="edit-lnk-btn" runat="server" Text="Edit" />
                </div>
            </div>
            <div class="profile-info-data">
                <div class="row-div clearfix">
                    <div class="float-left w33-3">
                        <div id="tdBusinessAdd" runat="server" class="label">
                            Street Address:
                        </div>
                        <div id="tdStreetAddVal" runat="server">
                            <asp:Label ID="StreetAddressval" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="float-left w33-3">
                        <div id="tdHomeAdd" runat="server" class="label">
                            Billing Address:
                        </div>
                        <div class="RightColumn tdCompanyAddressAlign" id="tdPoboxAddVal" runat="server">
                            <asp:Label ID="BillingAddressval" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="float-left w33-3">
                        <div id="tdPoboxAdd" runat="server" class="label">
                            PO Box Address:
                        </div>
                        <div class="RightColumn tdCompanyAddressAlign" id="tdBillingAddVal" runat="server">
                            <asp:Label ID="PoboxAddressval" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
                <div>
                    <div class="row-div clearfix">
                        <div runat="server" id="tdPhone" class="label-div w18">
                            (Area Code) Phone:
                        </div>
                        <div class="field-div1 w80">
                            <asp:Label ID="lblphoneVal" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="row-div clearfix">
                        <div runat="server" id="tdFax" class="label-div w18">
                            (Area Code) Fax:
                        </div>
                        <div class="field-div1 w80">
                            <asp:Label ID="lblFaxVal" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div id="tremail" runat="server" class="row-div clearfix">
                        <div runat="server" id="tdemail" class="label-div w18">
                            Email:
                        </div>
                        <div class="field-div1 w80">
                            <asp:Label ID="lblPrimaryEmail" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div id="trWebSite" runat="server" class="row-div clearfix">
                        <div runat="server" id="tdWebsite" class="label-div w18">
                            Website:
                        </div>
                        <div class="field-div1 w80">
                            <asp:Label ID="lblWebsite" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div>
            <div id="trWebAccount" runat="server" class="membership-info-container">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div class="membership-info-title">
                            Membership Information
                        </div>
                        <div class="profile-info-data">
                            <div class="row-div clearfix">
                                <div class="label-div w18">
                                    <asp:Label ID="lblmembershipType" runat="server">Membership Type:</asp:Label>
                                </div>
                                <div class="field-div1 w80">
                                    <asp:Label ID="lblMemberTypeVal" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row-div clearfix">
                                <div class="label-div w18">
                                    <asp:Label ID="lblStartDate" runat="server">Start Date:</asp:Label>
                                </div>
                                <div class="field-div1 w80">
                                    <asp:Label ID="lblStartDateVal" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row-div clearfix">
                                <div class="label-div w18">
                                    <asp:Label ID="lblEndDate" runat="server">End Date:</asp:Label>
                                </div>
                                <div class="field-div1 w80">
                                    <asp:Label ID="lblEndDateVal" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row-div clearfix">
                                <div class="label-div w18">
                                    <asp:Label ID="lblStatus" runat="server">Status:</asp:Label>
                                </div>
                                <div class="field-div1 w80">
                                    <asp:Label ID="lblStatusVal" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <%--<asp:AsyncPostBackTrigger ControlID="txtUserID" EventName="TextChanged" />--%>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="topicofInterest-info-container">
                <%--Amruta IssueID 14320--%>
                <div class="topicofInterest-info-title clearfix">
                    <asp:Label ID="lbltopicofInterest" runat="server" Text="Topics of Interest" ToolTip="This area displays the top level topics currently associated with your company. To see a complete list of topics or to change your company's selections, click Edit to load the topic tree view."></asp:Label>
                    <div class="float-right right-margin">
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Edit.png" />
                        <asp:LinkButton ID="btnTopicIntrest" runat="server" CssClass="edit-lnk-btn" Text="Edit"
                            Style="margin-left: 0px" ToolTip="This area displays the top level topics currently associated with your company. To see a complete list of topics or to change your company's selections, click Edit to load the topic tree view." />
                    </div>
                </div>
                <div class="profile-info-data">
                    <asp:Label ID="lblTopicIntrest" runat="server"></asp:Label>
                    <%--   <asp:Image ID="tooptip" runat="server" ImageUrl="~/Images/Topic-tooltip-icon.png"
                                        ToolTip="This area displays the top level topics currently associated with your company. To see a complete list of topics or to change your company's selections, click Edit to load the topic tree view." />
                    --%>
                </div>
            </div>
        </div>
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
                <rad:radwindow id="RadWindow1" runat="server" class="popup-company-address" modal="True" visiblestatusbar="False"
                    behaviors="None" title="Address Information" skin="Default">
                    <ContentTemplate>
                    <div class="popup-contact-info-container">
                        <asp:UpdatePanel ID="updatepnl" runat="server">
                            <ContentTemplate>
                                
                                    <div class="row-div clearfix top-margin">
                                        <div class="label-div w18">
                                            Address Type:
                                        </div>
                                        <div class="field-div1 w80" align="left">
                                        <div class="float-left w48">
                                            <asp:DropDownList ID="ddlAddressType"  runat="server"
                                                AutoPostBack="True" OnSelectedIndexChanged="selectedindex">
                                                <asp:ListItem Text="Street Address" Value="Street Address" runat="server"></asp:ListItem>
                                                <asp:ListItem Text="Billing Address" Value="Billing Address" runat="server"></asp:ListItem>
                                                <asp:ListItem Text="PO Box Address" Value="PObox Adress" runat="server"></asp:ListItem>
                                            </asp:DropDownList>
                                             </div>
                                             <div class="float-left w48 clearfix"><asp:CheckBox ID="chkPrefAddress" runat="server" Text="Preferred Address"
                                                AutoPostBack="True" Visible="false" /></div>
                                        </div>
                                       
                                    </div>
                                    <div id="trAddressLine1" runat="server" class="row-div clearfix">
                                        <div id="Td1" class="label-div w18" runat="server">
                                            <asp:Label ID="lblAddress" runat="server" >Address:</asp:Label>
                                        </div>
                                        <div id="Td2" class="field-div1 w80" runat="server" >
                                            <asp:TextBox ID="txtAddressLine1" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <%-- Amruta , Issue No 14320,used separate controls for each addressline--%>
                                    <div id="trBillingAddressLine1" runat="server" visible="False" class="row-div clearfix">
                                        <div class="label-div w18" runat="server">
                                            <asp:Label ID="lblBillingAddress" runat="server">Address:</asp:Label>
                                        </div>
                                        <div class="field-div1 w80" colspan="2" runat="server">
                                            <asp:TextBox ID="txtBillingAddressLine1" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div id="trPOBoxAddressLine1" runat="server" visible="False" class="row-div clearfix">
                                        <div class="label-div w18" runat="server">
                                            <asp:Label ID="lblPOBoxAddress" runat="server">Address:</asp:Label>
                                        </div>
                                        <div class="field-div1 w80" colspan="2" runat="server">
                                            <asp:TextBox ID="txtPOBoxAddressLine1" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <!-- Address Line 2 Rows -->
                                    <div id="trAddressLine2" runat="server" class="row-div clearfix">
                                        <div id="Td9" class="label-div emptyspace w18" runat="server">
                                            &nbsp;
                                        </div>
                                        <div id="Td10" class="field-div1 w80" colspan="2" runat="server">
                                            <asp:TextBox ID="txtAddressLine2" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div id="trBillingAddressLine2" runat="server" visible="False" class="row-div clearfix">
                                        <div class="label-div emptyspace w18" runat="server">
                                            &nbsp;
                                        </div>
                                        <div class="field-div1 w80" colspan="2" runat="server">
                                            <asp:TextBox ID="txtBillingAddressLine2" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div id="trPOBoxAddressLine2" runat="server" visible="False" class="row-div clearfix">
                                        <div class="label-div emptyspace w18" runat="server">
                                            &nbsp;
                                        </div>
                                        <div class="field-div1 w80" colspan="2" runat="server">
                                            <asp:TextBox ID="txtPOBoxAddressLine2" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <!-- Address Line 3 Rows -->
                                    <div id="trAddressLine3" runat="server" class="row-div clearfix">
                                        <div id="Td17" class="label-div emptyspace w18" runat="server">
                                            &nbsp;
                                        </div>
                                        <div id="Td18" class="field-div1 w80" colspan="2" runat="server" >
                                            <asp:TextBox ID="txtAddressLine3" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div id="trBillingAddressLine3" runat="server" visible="False" class="row-div clearfix">
                                        <div class="label-div emptyspace w18" runat="server">
                                            &nbsp;
                                        </div>
                                        <div class="field-div1 w80" colspan="2" runat="server">
                                            <asp:TextBox ID="txtBillingAddressLine3" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div id="trPOBoxAddressLine3" runat="server" visible="False" class="row-div clearfix">
                                        <div class="label-div emptyspace w18" runat="server">
                                            &nbsp;
                                        </div>
                                        <div class="field-div1 w80" colspan="2" runat="server">
                                            <asp:TextBox ID="txtPOBoxAddressLine3" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div id="trCity" runat="server" class="row-div clearfix">
                                        <div id="Td25" class="label-div w18" runat="server">
                                            <asp:Label ID="lblCityStateZip" runat="server" Font-Bold="true">City:</asp:Label>
                                        </div>
                                        <div id="Td26" class="field-div1 w32" runat="server">
                                            <asp:TextBox ID="txtCity"  runat="server"></asp:TextBox>
                                        </div>
                                        <div class="label-div1 w12-5">
                                            <asp:Label ID="Label5" runat="server">State:</asp:Label>
                                        </div>
                                        <div class="field-div2 w32">
                                            <asp:DropDownList  ID="cmbState" runat="server">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div id="trBillingCity" runat="server" visible="False" class="row-div clearfix">
                                        <div class="label-div w18" runat="server">
                                            <asp:Label ID="lblBillingCity" runat="server" >City:</asp:Label>
                                        </div>
                                        <div class="field-div1 w32" runat="server">
                                            <asp:TextBox ID="txtBillingCity" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="label-div1 w12-5">
                                            <asp:Label ID="lblBillingState" runat="server" >State:</asp:Label>
                                        </div>
                                        <div class="field-div2 w32">
                                            <asp:DropDownList ID="cmbBillingState" runat="server">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div id="trPOBoxCity" runat="server" visible="False" class="row-div clearfix">
                                        <div class="label-div w18" colspan="1" runat="server">
                                            <asp:Label ID="lblPOBoxCity" runat="server" >City:</asp:Label>
                                        </div>
                                        <div class="field-div1 w32" runat="server">
                                            <asp:TextBox ID="txtPOBoxCity"  runat="server"></asp:TextBox>
                                        </div>
                                        <div class="label-div1 w12-5">
                                            <asp:Label ID="lblPOBoxState" runat="server" >State:</asp:Label>
                                        </div>
                                        <div class="field-div2 w32">
                                            <asp:DropDownList ID="cmbPOBoxState" runat="server">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div id="trCountry" runat="server" class="row-div clearfix">
                                        <div id="Td33" class="label-div w18" colspan="1" runat="server">
                                            <asp:Label ID="lblCountry" runat="server" >Country:</asp:Label>
                                        </div>
                                        <div id="Td34" class="field-div1 w32" runat="server" >
                                            <asp:DropDownList CssClass="w84" ID="cmbCountry" runat="server" AutoPostBack="True" EnableScreenBoundaryDetection="false" OnSelectedIndexChanged="cmbCountry_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="label-div1 w12-5">
                                            <asp:Label ID="Label23" runat="server" >ZIP Code:</asp:Label>
                                        </div>
                                        <div class="field-div2 w32">
                                            <asp:TextBox ID="txtzip" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div id="trBillingCountry" runat="server" visible="False" class="row-div clearfix">
                                        <div class="label-div w18" runat="server">
                                            <asp:Label ID="lblBillingCountry" runat="server" >Country:</asp:Label>
                                        </div>
                                        <div class="field-div1 w32" runat="server">
                                            <asp:DropDownList ID="cmbBillingCountry" CssClass="w84" runat="server" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="label-div1 w12-5">
                                            <asp:Label ID="lblBillingZipCode" runat="server" >ZIP Code:</asp:Label>
                                        </div>
                                        <div class="field-div2 w32">
                                            <asp:TextBox ID="txtBillingZipCode" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div id="trPOBoxCountry" runat="server" visible="False" class="row-div clearfix">
                                        <div class="label-div w18" runat="server">
                                            <asp:Label ID="lblPOBoxCountry" runat="server">Country:</asp:Label>
                                        </div>
                                        <div class="field-div1 w32" runat="server">
                                            <asp:DropDownList ID="cmbPOBoxCountry" CssClass="w84" runat="server" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="label-div1 w12-5">
                                            <asp:Label ID="lblPOBoxZipCode" runat="server" >ZIP Code:</asp:Label>
                                        </div>
                                        <div class="field-div2 w32">
                                            <asp:TextBox ID="txtPOBoxZipCode"  runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div id="tr1" runat="server" class="row-div clearfix">
                                        <div id="Td3" class="label-div w18" runat="server">
                                            <asp:Label ID="lblEmail" runat="server" >Email:</asp:Label>
                                        </div>
                                        <div id="Td4" align="left" class="field-div1 w32" runat="server" >
                                            <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="label-div1 w12-5">
                                            <asp:Label ID="Label2" runat="server" >Website:</asp:Label>
                                        </div>
                                        <div class="field-div2 w32">
                                            <asp:TextBox ID="txtWebsite" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                <div class="row-div clearfix">
                                    <div class="label-div emptyspace w18">&nbsp;
                                    </div>
                                    <div align="left" class="field-div1 w80" runat="server" >
                                        <%--Suraj Issue 15210 ,2/19/13 RegularExpressionValidator validator --%>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="Dynamic"
                                            ValidationExpression="[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+(?:[A-Z]{2}|com|COM|org|ORG|net|NET|edu|EDU|gov|GOV|mil|MIL|biz|BIZ|info|INFO|mobi|MOBI|name|NAME|aero|AERO|asia|ASIA|jobs|JOBS|museum|MUSEUM|in|IN|co|CO)\b"
                                            ControlToValidate="txtEmail" ErrorMessage="Invalid Email Format" ValidationGroup="VldAdressInfo"
                                            CssClass="error-msg-label"></asp:RegularExpressionValidator>
                                    </div>
                                </div>
                                <div class="row-div clearfix">
                                    <div class="label-div w18">
                                        <asp:Label ID="lblPhone" runat="server" >(Area Code) Phone:</asp:Label>
                                    </div>
                                    <%--Neha Issue 14750 ,02/26/13, added css for phoneareacode--%>
                                    <div class="field-div1 w32">
                                    <div class="field-div1 w25">
                                        <rad:RadMaskedTextBox ID="txtPhoneAreaCode"
                                            runat="server" Mask="(###)" >
                                        </rad:RadMaskedTextBox></div>
                                        <div class="field-div2 w73"><rad:RadMaskedTextBox ID="txtPhone" runat="server"
                                            Mask="###-####">
                                        </rad:RadMaskedTextBox></div>
                                    </div>
                                </div>
                                <div class="row-div clearfix">
                                    <div class="label-div w18">
                                        <asp:Label ID="lblFax" runat="server" Font-Bold="true">(Area Code) Fax:</asp:Label>
                                    </div>
                                    <div class="field-div1 w32">
                                    <div class="field-div1 w25">
                                        <rad:RadMaskedTextBox ID="txtFaxAreaCode" 
                                            runat="server" Mask="(###)" >
                                        </rad:RadMaskedTextBox></div>
                                       <div class="field-div2 w73"> <rad:RadMaskedTextBox ID="txtFaxPhone" runat="server" 
                                            Mask="###-####" >
                                        </rad:RadMaskedTextBox></div>
                                    </div>
                                </div>
                                </div>
                                <%-- Amruta IssueID : 14320 --%>
                                <div class="align-center">
                                    <asp:Button ID="btnsave" Text="Save" runat="server" CssClass="submit-Btn"
                                        OnClick="btnsave_Click" ValidationGroup="VldAdressInfo" />
                                    <asp:Button ID="btnCancel" Text="Cancel" runat="server" CssClass="submit-Btn"
                                        OnClick="btnCancel_Click" />
                                    <%--
                                </div> </tr>--%>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlAddressType" />
                                <asp:AsyncPostBackTrigger ControlID="cmbCountry" />
                                <asp:AsyncPostBackTrigger ControlID="cmbState" />
                                <asp:PostBackTrigger ControlID="btnsave" />
                                <asp:PostBackTrigger ControlID="btnCancel" />
                                <asp:AsyncPostBackTrigger ControlID="cmbBillingCountry" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="cmbPOBoxCountry" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                     
                    </ContentTemplate>
                   
                   </rad:radwindow>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <rad:radwindow id="radtopicintrest" runat="server" modal="True" class="popup-company-topic"
                    visiblestatusbar="False" behaviors="None"  title="Topics of Interest"
                    behavior="None" iconurl="~/Images/topic-of-int.png" skin="Default">
                    <ContentTemplate>
                     
                                <div>
                                    <cc2:Topiccode ID="TopiccodeViewer" runat="server" />
                                </div>
                       
                                <div class="align-center">
                                    <asp:Button ID="btnSaveIntrest" runat="server" Text="Save" class="submit-Btn"
                                        OnClick="btnSaveIntrest_Click" />
                                    <asp:Button ID="btnCancelIntrest" runat="server" Text="Cancel" class="submit-Btn"
                                        OnClick="btnCancelIntrest_Click" />
                                </div>
                       
                    </ContentTemplate>
                </rad:radwindow>
            </ContentTemplate>
        </asp:UpdatePanel>
    </ContentTemplate>
</asp:UpdatePanel>
<cc1:user id="User1" runat="server"></cc1:user>
