<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ExpoRegistration.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.ProductCatalog.ExpoRegistrationControl" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessShoppingCart" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<script language="javascript" type="text/javascript">
    //Anil B for issue 15012 
    // Dynamically handle blank value for Area code and Telephone
    function EnableDisablePhoneValidation() {      
        var txtAreaCode = document.getElementById('<%= txtAreaCode.ClientID %>');
        var txtTelephone = document.getElementById('<%= txtTelephone.ClientID %>');
        var rfvAreaCode = document.getElementById('<%= rfvAreaCode.ClientID %>');
        var rfvTelephone = document.getElementById('<%= rfvTelephone.ClientID %>');
        var areaCodeValue = txtAreaCode.value;
        var telephoneValue = txtTelephone.value;

        var iAreaCodeTextCount = 0;
        var iTelephoneTextCount = 0;
        for (var i = 0; i < areaCodeValue.length; i++) {
            if (!isNaN(areaCodeValue[i])) {
                iAreaCodeTextCount++;
            }
        }

        for (var i = 0; i < telephoneValue.length; i++) {
            if (!isNaN(telephoneValue[i])) {
                iTelephoneTextCount++;
            }
        }

        if (iTelephoneTextCount == 0 && iAreaCodeTextCount == 0) {
            ValidatorEnable(rfvAreaCode, false);
            ValidatorEnable(rfvTelephone, false);            
        }
        else {
            if (iAreaCodeTextCount == 0) {
                ValidatorEnable(rfvAreaCode, true);
                ValidatorEnable(rfvTelephone, false);               
            }
            else {
                ValidatorEnable(rfvAreaCode, false);
                ValidatorEnable(rfvTelephone, true);                
            }
        }
    }

     <%--Suraj Issue 15012 ,2/25/13 ifthe user enter secondary email validation fire, then sendmail label message display none and ifthe user enter Personal Information forSecondary Contact then validation will be enable   --%>
    function ChkSecondaryInfoIsValid() 
    {
    if ( document.getElementById("<%=txtSCFName.ClientID%>").value !="" || document.getElementById("<%=txtSCLName.ClientID%>").value !="" || document.getElementById("<%=txtSCEmail.ClientID%>").value !="" )
       {
         ValidatorEnable(document.getElementById('<%= RequiredFieldValidator4.ClientID %>'), true);
         ValidatorEnable(document.getElementById('<%= RequiredFieldValidator5.ClientID %>'), true);
         ValidatorEnable(document.getElementById('<%= RequiredFieldValidator6.ClientID %>'), true);
   
       }
    else
      {
         ValidatorEnable(document.getElementById('<%= RequiredFieldValidator4.ClientID %>'), false);
         ValidatorEnable(document.getElementById('<%= RequiredFieldValidator5.ClientID %>'), false);
         ValidatorEnable(document.getElementById('<%= RequiredFieldValidator6.ClientID %>'), false);
    
      }
<%--  Suraj Issue 15210,2/8/13 ifthe page is not validate then lblError display none --%>
    if (Page_ClientValidate("VldSaveBooth"))
        {
            return true;
        }
        else 
        {
            document.getElementById("<%=lblError.ClientID%>").style.display = 'none';
            return false;
        }
    }
        
</script>
<div class="table-div">
    <cc2:aptifyshoppingcart id="ShoppingCart1" runat="server" visible="False" />
    <%-- <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>--%>
    <div class="table-div expo-registration" runat="server" id="tblMain">
        <div class="row-div">
            <%--Suraj Issue 15012 ,2/22/13 Add a validation Group and add css class ExpoRegistrationerror for lblError --%>
            <asp:ValidationSummary ID="ValSummary" Visible="false" ValidationGroup="VldSaveBooth"
                runat="server" />
            <asp:Label ID="lblError" runat="server" Visible="False"></asp:Label>
        </div>
        <%--Suraj Issue 15012 ,2/22/13 Add row to disply ExpoRegProductName --%>
        <div class="row-div">
            <asp:Label ID="lblProductName" runat="server"></asp:Label>
        </div>
        <div class="row-div label">
            Exhibitor Information
        </div>
        <div class="row-div clearfix">
            <div class="label-div w19">
                <span class="required-label">*</span>
                <asp:Label ID="lblExh" runat="server" Text="Company:"></asp:Label>
            </div>
            <div class="field-div1 w80">
                <asp:TextBox ID="txtExhibitor" runat="server"></asp:TextBox>
                <%--Suraj Issue 15012 ,2/22/13 Add ValidationGroup --%>
                <asp:RequiredFieldValidator ID="ReqtxtExhibitor" ValidationGroup="VldSaveBooth" runat="server"
                    ControlToValidate="txtExhibitor" ErrorMessage="Please fill in the Exhibitor"
                    CssClass="error-msg-label"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="row-div-bottom-line label">
            Personal Information for Primary Contact
        </div>
        <div class="row-div clearfix">
            <div class="label-div w19">
                <span class="required-label">*</span> First Name:
            </div>
            <div class="field-div1 w80">
                <asp:TextBox ID="txtPCFName" runat="server"></asp:TextBox>
                <%--Suraj Issue 15012 ,2/22/13 Add ValidationGroup --%>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="VldSaveBooth"
                    runat="server" ControlToValidate="txtPCFName" ErrorMessage="Primary contact First Name Required"
                    CssClass="error-msg-label"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="row-div clearfix">
            <div class="label-div w19">
                <span class="required-label">*</span> Last Name:
            </div>
            <div class="field-div1 w80">
                <asp:TextBox ID="txtPCLName" runat="server"></asp:TextBox>
                <%--Suraj Issue 15012 ,2/22/13 Add ValidationGroup --%>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="VldSaveBooth"
                    runat="server" Text="*" ControlToValidate="txtPCLName" ErrorMessage="Primary contact last name required"
                    CssClass="error-msg-label"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="row-div clearfix">
            <div class="label-div w19">
                <span class="required-label">*</span> Email:
            </div>
            <div class="field-div1 w80">
                <asp:TextBox ID="txtPCEmail" runat="server"></asp:TextBox>
                <%--Suraj Issue 15012 ,2/22/13 Add ValidationGroup --%>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="VldSaveBooth"
                    runat="server" ControlToValidate="txtPCEmail" ErrorMessage="Primary contact email name required"
                    CssClass="error-msg-label"></asp:RequiredFieldValidator>
                <%-- Suraj Issue 15210, 2/6/13 validate email id --%>
                <asp:RegularExpressionValidator ID="regexPCEmailValid" ValidationGroup="VldSaveBooth"
                    runat="server" ValidationExpression="[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+(?:[A-Z]{2}|com|COM|org|ORG|net|NET|edu|EDU|gov|GOV|mil|MIL|biz|BIZ|info|INFO|mobi|MOBI|name|NAME|aero|AERO|asia|ASIA|jobs|JOBS|museum|MUSEUM|in|IN|co|CO)\b"
                    ControlToValidate="txtPCEmail" Display="None" ErrorMessage="Invalid Primary contact email "></asp:RegularExpressionValidator>
            </div>
        </div>
        <div class="row-div-bottom-line label">
            
            Personal Information for Secondary Contact
        </div>
        <div class="row-div clearfix">
            <div class="label-div w19">
                First Name:
            </div>
            <div class="field-div1 w80">
                <asp:TextBox ID="txtSCFName" runat="server"></asp:TextBox>
                <%--Suraj Issue 15012 ,2/22/13 Add RequiredFieldValidator for Secondary FName --%>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ValidationGroup="VldSaveBooth"
                    runat="server" ControlToValidate="txtSCFName" ErrorMessage="Secondary contact First Name Required"
                    CssClass="error-msg-label"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="row-div clearfix">
            <div class="label-div w19">
                Last Name:
            </div>
            <div class="field-div1 w80">
                <asp:TextBox ID="txtSCLName" runat="server"></asp:TextBox>
                <%--Suraj Issue 15012 ,2/22/13 Add RequiredFieldValidator for Secondary LName --%>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ValidationGroup="VldSaveBooth"
                    runat="server" ControlToValidate="txtSCLName" ErrorMessage="Secondary contact last name required"
                    CssClass="error-msg-label"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="row-div clearfix">
            <div class="label-div w19">
                Email:
            </div>
            <div class="field-div1 w80">
                <asp:TextBox ID="txtSCEmail" runat="server"></asp:TextBox>
                <%-- Suraj Issue 15012, 2/6/13 asterisk mark if the secondary email is required --%>
                <asp:Label ID="lblasterisk" runat="server" Visible="false"></asp:Label>
                <%--Suraj Issue 15012 ,2/25/13 Add RequiredFieldValidator for Secondary Email --%>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ValidationGroup="VldSaveBooth"
                    runat="server" ControlToValidate="txtSCEmail" ErrorMessage="Secondary contact email name required"
                    CssClass="error-msg-label"></asp:RequiredFieldValidator>
                <%-- Suraj Issue 15210, 2/6/13 validate email id --%>
                <asp:RegularExpressionValidator ID="regexSCEmailValid" ValidationGroup="VldSaveBooth"
                    runat="server" ValidationExpression="[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+(?:[A-Z]{2}|com|COM|org|ORG|net|NET|edu|EDU|gov|GOV|mil|MIL|biz|BIZ|info|INFO|mobi|MOBI|name|NAME|aero|AERO|asia|ASIA|jobs|JOBS|museum|MUSEUM|in|IN|co|CO)\b"
                    ControlToValidate="txtSCEmail" Display="None" ErrorMessage="Invalid Secondary contact email "></asp:RegularExpressionValidator>
            </div>
        </div>
        <div class="row-div-bottom-line label">
            
            Contact Information
        </div>
        <div class="row-div clearfix">
            <div class="label-div w19">
                (Area Code) Phone:
            </div>
            <div class="field-div1 w31">
            <div class="field-div1 w20">
                <rad:radmaskedtextbox id="txtAreaCode" runat="server" mask="(###)">
                        <clientevents onvaluechanged="EnableDisablePhoneValidation" />
                    </rad:radmaskedtextbox></div>
                    <div class="field-div2 w78">
                <rad:radmaskedtextbox id="txtTelephone" runat="server" mask="###-####">
                        <clientevents onvaluechanged="EnableDisablePhoneValidation" />
                    </rad:radmaskedtextbox>
                    </div>
                <div class="clear"></div>
                <asp:RequiredFieldValidator ID="rfvAreaCode" runat="server" ControlToValidate="txtAreaCode"
                    ErrorMessage="Area Code Required" CssClass="error-msg-label" Visible="false"></asp:RequiredFieldValidator>
                <%--Suraj Issue 15012 ,2/25/13 Add ValidationGroup --%>
                <asp:RequiredFieldValidator ID="rfvTelephone" ValidationGroup="VldSaveBooth" runat="server"
                    ControlToValidate="txtTelephone" ErrorMessage="Phone Number Required" CssClass="error-msg-label"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="row-div-bottom-line label">
            
            Booth Information
        </div>
        <div class="row-div clearfix">
            <div class="label-div w19">
                Booth #:
            </div>
            <div class="field-div1 w80">
                <asp:DropDownList ID="cmbBooth" runat="server" AutoPostBack="True">
                </asp:DropDownList>
                <asp:Label ID="lblBooth" runat="server" Text="label not used" Visible="False"></asp:Label>
            </div>
        </div>
        <div class="row-div clearfix">
            <div class="label-div w19">
                Booth Name:
            </div>
            <div class="field-div1 w80">
                <asp:TextBox ID="txtBoothName" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="row-div clearfix">
            <div class="label-div w19">
                Booth Description:
            </div>
            <div class="field-div1 w80">
                <asp:TextBox ID="txtBoothDescription" runat="server" TextMode="MultiLine"></asp:TextBox>
            </div>
        </div>
        <div class="row-div clearfix">
            <div class="label-div w19">
                Weight Required:
            </div>
            <div class="field-div1 w80">
                <asp:TextBox ID="txtWeightRequired" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="row-div clearfix">
            <div class="label-div w19">
                Booth Options:
            </div>
            <div class="field-div1 w80">
                <div>
                    <asp:CheckBox ID="chkElectric" runat="server" DESIGNTIMEDRAGDROP="141" Text="Requires Electricity">
                    </asp:CheckBox>
                </div>
                <div>
                    <asp:CheckBox ID="chkWater" runat="server" Text="Requires Water"></asp:CheckBox></div>
                <div>
                    <asp:CheckBox ID="chkAir" runat="server" Text="Requires Compressed Air"></asp:CheckBox></div>
                <div>
                    <asp:CheckBox ID="chkGas" runat="server" Text="Requires Gas Hookup"></asp:CheckBox></div>
                <asp:CheckBox ID="chkDrain" runat="server" Text="Requires Drain"></asp:CheckBox>
                <div class="row-div clearfix">
                    <div class="label-div">
                        Your Price:
                    </div>
                    <div class="field-div1 w80">
                        <asp:Label ID="lblPrice" runat="server">Price</asp:Label>
                    </div>
                </div>
                <div class="row-div clearfix">
                    <asp:Label ID="lblSurcharge" Visible="False" runat="server"></asp:Label>
                </div>
            </div>
            <div class="row-div clearfix">
                <div class="label-div w19">
                    &nbsp;</div>
                <div class="field-div1 w80">
                    <%--Suraj Issue 15012 ,2/25/13 Add ValidationGroup and add OnClientClick="return ChkSecondaryInfoIsValid();" function for checking the validation --%>
                    <asp:Button ID="cmdSave" runat="server" ValidationGroup="VldSaveBooth" CssClass="submitBtn"
                        Text="Save" OnClientClick="return ChkSecondaryInfoIsValid();"></asp:Button>
                </div>
            </div>
        </div>
    </div>
    <%--  </ContentTemplate>
    </asp:UpdatePanel>--%>
    <div class="table-div" runat="server" id="tblMain1">
        <asp:Label ID="lblExhibitorID" runat="server" Visible="False" DESIGNTIMEDRAGDROP="37"></asp:Label>
        <asp:Label ID="lblExhibitorName" runat="server" DESIGNTIMEDRAGDROP="37" Visible="False"></asp:Label>
        <asp:Label ID="lblContactID" runat="server" DESIGNTIMEDRAGDROP="139" Visible="False"></asp:Label>
        <asp:Label ID="lblContactName" runat="server" DESIGNTIMEDRAGDROP="139" Visible="False"></asp:Label>
        <asp:Label ID="lblPrimaryEmail" runat="server" DESIGNTIMEDRAGDROP="139" Visible="False"></asp:Label>
        <asp:Label ID="lblSecondaryContactID" runat="server" Visible="False" Width="98px"></asp:Label>
        <asp:Label ID="lblSecondaryContactName" runat="server" Visible="False"></asp:Label>
        <asp:Label ID="lblSecondaryEmail" runat="server" Visible="False"></asp:Label>
        <asp:Label ID="lblBoothQty" runat="server" Text="BoothQty" Visible="False"></asp:Label>
        <asp:Label ID="lblUnitPrice" runat="server" Text="UnitPrice" Visible="False"></asp:Label>
    </div>
    <rad:radwindow id="radDuplicateUser" runat="server" width="650px" height="120px"
        modal="True" skin="Default" backcolor="#f4f3f1" visiblestatusbar="False" behaviors="None"
        forecolor="#BDA797" iconurl="~/Images/Alert.png" title="Alert" behavior="None">
        <contenttemplate>
                <div>
                    <div align="left">
                                <asp:Label ID="lblAlert" runat="server" Text="" Font-Bold="true"></asp:Label>
                    </div>
                </div>
                <div align="center">
                    <asp:Button ID="btnok" runat="server" Text="OK" class="submit-Btn" OnClick="btnok_Click"
                        ValidationGroup="ok" />&nbsp;&nbsp;
                </div>
        </contenttemplate>
    </rad:radwindow>
    <cc3:user id="User1" runat="server" />
</div>
