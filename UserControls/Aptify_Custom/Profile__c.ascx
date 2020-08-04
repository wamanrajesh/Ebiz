<%--Aptify e-Business 5.5.3, July 2015--%>
<%--This Control is specific to Sitefinity Site--%>
<%@ Register Src="~/UserControls/Aptify_General/Sitefinity4xSSO.ascx" TagName="Sitefinity4xSSO" TagPrefix="uc2" %>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Profile__c.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.CustomerService.ProfileControl" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessLogin" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="cc4" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessShoppingCart" %>
<%@ Register TagName="SyncProfile" TagPrefix="uc1" Src="~/UserControls/Aptify_Customer_Service/SynchProfile.ascx" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<script type="text/javascript" language="javascript">
    //Anil  for Issue 6640
    window.history.forward(1);
    //Neha, issue 12591 Added function for Issue 12591
    function ShowvalidationErrorMsg() {
        var cmpValidator = document.getElementById('<%=CompareValidator.ClientID%>');
        if (document.getElementById('<%= txtoldpassword.ClientID %>').value.length == 0 ||
            window.document.getElementById('<%= txtNewPassword.ClientID %>').value.length == 0
            || window.document.getElementById('<%= txtRepeat.ClientID %>').value.length == 0) {
            window.document.getElementById('<%= lblErrormessage.ClientID %>').innerHTML = "All the above fields are mandatory.";
            window.document.getElementById('<%= lblErrormessage.ClientID %>').style.display = "block";
            window.document.getElementById('<%= lblerrorLength.ClientID %>').style.display = "none";
            ValidatorEnable(cmpValidator, false);
            return false;
        }
        else {
            //Neha, Issue 12591,03/06/13, added condition for Passwordvalidation
            if (window.document.getElementById('<%= txtNewPassword.ClientID %>').value != window.document.getElementById('<%= txtRepeat.ClientID %>').value) {
                ValidatorEnable(cmpValidator, true);
                window.document.getElementById('<%= lblErrormessage.ClientID %>').style.display = "none";
                return false;
            }
        }
    }
    //End 

    function ShowImage(ImageURL) {
        if (document.getElementById(ImageURL) != undefined) {
            document.getElementById(ImageURL).style.visibility = "visible";
        }
    }

    //Anil Add function for Issue 12835
    function UploadImage() {
        //Neha issue no 14430,03/01/13, disable Apply button 
        var button = document.getElementById('<%= btnSaveProfileImage.ClientID %>');
        button.disabled = true;
        button.value = 'Submitting...';
        var result = false;
        var upfile = document.getElementById('<%= radUploadProfilePhoto.ClientID%>' + 'file0').value;

        if (upfile != "") {
            var accept = "png,gif,jpg,jpeg".split(',');
            var getExtention = upfile.split('.');
            var extention = getExtention[getExtention.length - 1];
            for (i = 0; i < accept.length; i++) {
                if (accept[i].toUpperCase() == extention.toUpperCase()) {
                    result = true;
                    break;
                }
            }
            if (!result) {
                //alert("allowed file extention are png,gif,jpg,jpeg");
            }
            else {
                document.getElementById('<%= btnUpload.ClientID%>').click();
            }
        }
        else {
            alert("select image to Upload");
        }
        return result;
    }

    function ZoomBestFit() {
        var $ = $telerik.$;
        var imEditor = $find("<%= radImageEditor.ClientID %>");
        var imgProfile = imEditor.getImage();
        if (imgProfile.height > 320 || imgProfile.width > 400) {
            imEditor.zoomBestFit();
        }
        else {
            imEditor.zoomImage(100, true);
        }
    }

    function CropImage() {
        var $ = $telerik.$;
        var imageEditor = $find("<%= radImageEditor.ClientID %>");

        if (typeof (imageEditor._currentToolWidget) != "undefined") {
            if (typeof (imageEditor._currentToolWidget._cropBtn) != "undefined") {
                imageEditor._currentToolWidget._cropBtnClick();
                imageEditor._currentToolWidget._cancelBtnClick();
                imageEditor._currentToolWidget.close();
            }
        }

        return false;
    }

    function ShowCropButton(isVisible) {
        var btnCropImage = document.getElementById("<%= btnCropImage.ClientID %>");
        if (isVisible == true) {
            btnCropImage.style.display = "inline";
        }
        else {
            btnCropImage.style.display = "none";
        }
    }

    function OnClientToolsDialogClosed(sender, eventArgs) {
        ShowCropButton(false);
    }

    function OnClientImageChanging(sender, eventArgs) {
        if (eventArgs.get_commandName() == "Crop") {
            ShowCropButton(false);

        }
        else if (eventArgs.get_commandName() == "Reset") {
            ShowCropButton(false);

        }
    }

    function OnClientCommandExecuted(sender, eventArgs) {
        //Suraj S Issue 16495 , here we find the browse if the browser is Google crome it will return the 36 index for other ie and mozill it will return -1
        //this is for crome browser
        var browserName = navigator.userAgent.indexOf("AppleWebKit");
        if (eventArgs.get_commandName() == "Crop") {
            if (browserName > -1) {
                ZoomBestFit();
            }
            ShowCropButton(true);
        }
        //Suraj S Issue 16495 ,  if the browser is crome then call  the reset code here 
        if (browserName > -1) {
            if (eventArgs.get_commandName() == "Reset") {
                ShowCropButton(true);
            }
        }

    }

    // Neha issue no 14430, 03/01/13, added Function(enable apply button and applied class)
    function EnabledImageSaveButton() {
        var button = document.getElementById('<%= btnSaveProfileImage.ClientID %>');
        button.className = "submit-Btn";
        button.disabled = false;
    }
    //End   

</script>
<asp:Literal ID="ltlImageEditorStyle" runat="server"></asp:Literal>
<div id="Container" class="profile-main-div clearfix">
    <div id="TitleDiv" class="required-text-div">
        <asp:Label ID="Label12" runat="server">* designates required fields</asp:Label>
    </div>
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <div class="left-container w18">
                <div class="profile-image">
                    <div>
                        <asp:Image ID="imgProfile" runat="server" ImageUrl="" ClientIDMode="AutoID" />
                    </div>
                    <asp:Image ID="imgEditProfileImage" runat="server" ImageUrl="~/Images/Edit.png" />
                    <asp:LinkButton ID="lbtnOpenProfileImage" CssClass="edit-lnk-btn" runat="server"
                        Text="Edit" CausesValidation="false" />
                </div>
                <div class="profile-social-div">
                    <uc1:syncprofile runat="server" id="SyncProfile" />
                </div>
                <div class="profile-joinICE-img-div" id="divMembership" runat="server">
                    <asp:Image ID="ImgMembershipe" runat="server" ImageUrl="" />
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="lbtnOpenProfileImage" />
        </Triggers>
    </asp:UpdatePanel>
    <div class="right-container w80" id="tblMain" runat="server">
        <div>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" Visible="false"></asp:ValidationSummary>
            <asp:Label ID="Label1" runat="server" BackColor="Transparent" ForeColor="Red" Visible="False"></asp:Label>
            <asp:Label ID="lblPasswordsuccess" runat="server" ForeColor="Blue"></asp:Label>
        </div>
        <div class="personal-info-container">
            <div class="profile-title-bg">
                <div class="personal-info-title">
                    Personal Information
                </div>
            </div>
            <div class="profile-info-data">
                <div id="trUserID" runat="server" class="row-div clearfix">
                    <div class="label-div w18">
                        <span class="required-label">*</span><asp:Label ID="Label7" runat="server">User ID:</asp:Label>
                    </div>
                    <div class="field-div1 w80">
                        <asp:Label ID="lblUserID" runat="server"></asp:Label>
                        <asp:LinkButton ID="lnkChangePwd" runat="server" Text="Change Password?" CausesValidation="false"></asp:LinkButton>
                    </div>
                </div>
                <div class="row-div clearfix">
                    <div class="label-div w18">
                        <span class="required-label">*</span><asp:Label ID="lblName" runat="server">First Name:</asp:Label>
                    </div>
                    <div class="field-div1 w80">
                        <asp:TextBox SkinID="RequiredTextBox" ID="txtFirstName" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtFirstName"
                            ValidationGroup="ProfileControl" ErrorMessage="First Name Required" Display="Dynamic"
                            CssClass="required-label"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row-div clearfix">
                    <div class="label-div w18">
                        <span class="required-label">*</span><asp:Label ID="Label4" runat="server">Last Name:</asp:Label>
                    </div>
                    <div class="field-div1 w80">
                        <asp:TextBox SkinID="RequiredTextBox" ID="txtLastName" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtLastName"
                            ValidationGroup="ProfileControl" ErrorMessage="Last Name Required" ForeColor="Red"
                            Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row-div clearfix">
                    <div class="label-div w18">
                        <span class="required-label">*</span><asp:Label ID="lblEmail" runat="server">Email:</asp:Label>
                    </div>
                    <div class="field-div1 w80">
                        <asp:TextBox ID="txtEmail" runat="server" SkinID="RequiredTextBox"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEmail"
                            ErrorMessage="Email Required" ValidationGroup="ProfileControl" ForeColor="Red"
                            Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="regexEmailValid" runat="server" Display="Dynamic"
                            ValidationExpression="[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+(?:[A-Z]{2}|com|COM|org|ORG|net|NET|edu|EDU|gov|GOV|mil|MIL|biz|BIZ|info|INFO|mobi|MOBI|name|NAME|aero|AERO|asia|ASIA|jobs|JOBS|museum|MUSEUM|in|IN|co|CO)\b"
                            ControlToValidate="txtEmail" ErrorMessage="Invalid Email Format" ValidationGroup="ProfileControl"
                            ForeColor="Red"></asp:RegularExpressionValidator>
                    </div>
                </div>
                <div class="row-div clearfix">
                    <div class="label-div w18">
                        <asp:Label ID="lblCompany" runat="server">Company:</asp:Label>
                    </div>
                    <div class="field-div1 w80">
                        <asp:TextBox ID="txtCompany" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="row-div clearfix">
                    <div class="label-div w18">
                        <asp:Label ID="lblTitle" runat="server">Title:</asp:Label>
                    </div>
                    <div class="field-div1 w80">
                        <asp:TextBox ID="txtTitle" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="row-div clearfix">
                    <div class="label-div w18">
                        <asp:Label ID="lblPrimaryFunction" runat="server">Primary Job Function:</asp:Label>
                    </div>
                    <div class="field-div1 w80">
                        <asp:DropDownList ID="cmbPrimaryFunction" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
        <div class="webAccount-info-container" id="trWebAccount" runat="server">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="profile-title-bg">
                        <div class="webAccount-info-title">
                            Web Account Information
                        </div>
                    </div>
                    <div class="profile-info-data">
                        <div class="row-div clearfix">
                            <div class="label-div w18">
                                <asp:Label ID="lblWebUID" runat="server"><span class="required-label">*</span>User ID:</asp:Label></span>
                            </div>
                            <div class="field-div1 w80">
                                <asp:TextBox ID="txtUserID" runat="server" CssClass="txtBoxPasswordProfile" SkinID="RequiredTextBox"
                                    AutoPostBack="True"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtUserID"
                                    ValidationGroup="ProfileControl" ErrorMessage="User ID Required" ForeColor="Red"
                                    Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                            </div>
                        </div>
                        <div class="row-div clearfix">
                            <div class="label-div w18">
                                <asp:Label ID="lblPWD" runat="server"><span class="required-label">*</span>Password:</asp:Label>
                            </div>
                            <div class="field-div1 w80">
                                <asp:TextBox ID="txtPassword" runat="server" SkinID="RequiredTextBox" TextMode="Password"></asp:TextBox>
                                <asp:CompareValidator ID="valPWDMatch" runat="server" ControlToValidate="txtRepeatPWD"
                                    ErrorMessage="Passwords Must Match" ControlToCompare="txtPassword" ForeColor="Red"
                                    Display="Dynamic"></asp:CompareValidator>
                                <asp:Label ID="lblpasswordlengthError" runat="server" ForeColor="Red"></asp:Label>
                            </div>
                        </div>
                        <div class="row-div clearfix">
                            <div class="label-div w18">
                                <asp:Label ID="lblRepeatPWD" runat="server"><span class="required-label">*</span>Repeat Password:</asp:Label>
                            </div>
                            <div class="field-div1 w80">
                                <asp:TextBox ID="txtRepeatPWD" runat="server" TextMode="Password" SkinID="RequiredTextBox"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="valPWDRequired" runat="server" ControlToValidate="txtPassword"
                                    ErrorMessage="Password Required" ValidationGroup="ProfileControl" ForeColor="Red"
                                    Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row-div clearfix">
                            <div class="label-div w18">
                                <asp:Label ID="lblPasswordHintQuestion" runat="server"><span class="required-label">*</span>Hint Question:</asp:Label>
                            </div>
                            <div class="field-div1 w80">
                                <asp:DropDownList ID="cmbPasswordQuestion" runat="server">
                                    <asp:ListItem Value="My favorite color is?" Selected="True">My favorite color is?</asp:ListItem>
                                    <asp:ListItem Value="My mother's maiden name is?">My mother's maiden name is?</asp:ListItem>
                                    <asp:ListItem Value="I went to which high school?">I went to which high school?</asp:ListItem>
                                    <asp:ListItem Value="I was born in which city?">I was born in which city?</asp:ListItem>
                                    <asp:ListItem Value="My pet's name is?">My pet's name is?</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row-div clearfix">
                            <div class="label-div w18">
                                <asp:Label ID="lblPasswordHintAnswer" runat="server"><span class="required-label">*</span>Password Hint Answer:</asp:Label>
                            </div>
                            <div class="field-div1 w80">
                                <asp:TextBox ID="txtPasswordHintAnswer" runat="server" SkinID="RequiredTextBox" CssClass="txtBoxPasswordProfile"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="valPasswordHintRequired" runat="server" ControlToValidate="txtPasswordHintAnswer"
                                    ValidationGroup="ProfileControl" ErrorMessage="Hint Answer Required" ForeColor="Red"
                                    Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="txtUserID" EventName="TextChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="contact-info-container">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <div class="profile-title-bg">
                        <div class="contact-info-title">
                            Contact Information
                        </div>
                    </div>
                    <div class="profile-info-data">
                        <div class="row-div clearfix">
                            <div class="label-div w18">
                                Select Address Type:
                            </div>
                            <div class="field-div1 w80">
                                <div class="float-left w48">
                                    <asp:DropDownList ID="ddlAddressType" runat="server" CssClass="w49" AutoPostBack="true">
                                        <asp:ListItem>Business Address</asp:ListItem>
                                        <asp:ListItem>Home Address</asp:ListItem>
                                        <asp:ListItem>Billing Address</asp:ListItem>
                                        <asp:ListItem>PO Box Address</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <img runat="server" style="visibility: hidden;" id="imgProcessing" alt="Animated processing image URL not set" />
                                <div class="float-left w48 clearfix">
                                    <asp:CheckBox ID="chkPrefAddress" CssClass="w49" runat="server" Text="Preferred Address"
                                        AutoPostBack="True" />

                                         <%--  Added  Issue 18138--%>
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnVerify" runat="server" Text="Verify" CssClass="submit-Btn" Visible="false"  />
                                    <rad:RadWindow ID="radAddressMessage" runat="server" 
                                        Modal="True"  VisibleStatusbar="False" Behaviors="None"
                                        Title="Suggested Address" Skin="Default" Class="popup-win-profile-VerifyAddress">
                                        <ContentTemplate>
                                            <div runat="server" id="dvAddressMsg">
                                                <center>
                                                    <asp:Label runat="server" ID="lblAddressVerify" Visible="false"></asp:Label>
                                                    <br />
                                                </center>
                                            </div>
                                            <div runat="server" id="dvAddressSuggested" visible="false" class="top-margin clearfix">
                                            </div>
                                            <div>
                                                <center>
                                                   <asp:Button runat="server" ID="btnVerificationClosed" Text="Apply" CssClass="submit-Btn top-margin"
                                                        />
                                                        <asp:Button runat ="server" ID ="btnVerificationCancel" Text ="Cancel" CssClass="submit-Btn top-margin" />
                                                </center>
                                            </div>
                                        </ContentTemplate>
                                    </rad:RadWindow>
                                    <%--  End--%>
                                </div>
                            </div>
                        </div>
                        <div class="row-div clearfix" id="trAddressLine1" runat="server">
                            <div class="label-div w18">
                                <asp:Label ID="lblAddress" runat="server">Address:</asp:Label>
                            </div>
                            <div class="field-div1 w80">
                                <asp:TextBox ID="txtAddressLine1" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row-div clearfix" id="trHomeAddressLine1" runat="server" visible="false">
                            <div class="label-div w18">
                                <asp:Label ID="Label2" runat="server">Address:</asp:Label>
                            </div>
                            <div class="field-div1 w80">
                                <asp:TextBox ID="txtHomeAddressLine1" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row-div clearfix" id="trBillingAddressLine1" runat="server" visible="false">
                            <div class="label-div w18">
                                <asp:Label ID="Label3" runat="server">Address:</asp:Label>
                            </div>
                            <div class="field-div1 w80">
                                <asp:TextBox ID="txtBillingAddressLine1" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row-div clearfix" id="trPOBoxAddressLine1" runat="server" visible="false">
                            <div class="label-div w18">
                                <asp:Label ID="Label8" runat="server">Address:</asp:Label>
                            </div>
                            <div class="field-div1 w80">
                                <asp:TextBox ID="txtPOBoxAddressLine1" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row-div clearfix" id="trAddressLine2" runat="server">
                            <div class="label-div emptyspace w18">
                                &nbsp;
                            </div>
                            <div class="field-div1 w80">
                                <asp:TextBox ID="txtAddressLine2" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row-div clearfix" id="trHomeAddressLine2" runat="server" visible="false">
                            <div class="label-div emptyspace w18">
                                &nbsp;
                            </div>
                            <div class="field-div1 w80">
                                <asp:TextBox ID="txtHomeAddressLine2" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row-div clearfix" id="trBillingAddressLine2" runat="server" visible="false">
                            <div class="label-div emptyspace w18">
                                &nbsp;
                            </div>
                            <div class="field-div1 w80">
                                <asp:TextBox ID="txtBillingAddressLine2" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row-div clearfix" id="trPOBoxAddressLine2" runat="server" visible="false">
                            <div class="label-div emptyspace w18">
                                &nbsp;
                            </div>
                            <div class="field-div1 w80">
                                <asp:TextBox ID="txtPOBoxAddressLine2" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row-div clearfix" id="trAddressLine3" runat="server">
                            <div class="label-div emptyspace w18">
                                &nbsp;
                            </div>
                            <div class="field-div1 w80">
                                <asp:TextBox ID="txtAddressLine3" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row-div clearfix" id="trHomeAddressLine3" runat="server" visible="false">
                            <div class="label-div emptyspace w18">
                                &nbsp;
                            </div>
                            <div class="field-div1 w80">
                                <asp:TextBox ID="txtHomeAddressLine3" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row-div clearfix" id="trBillingAddressLine3" runat="server" visible="false">
                            <div class="label-div emptyspace w18">
                                &nbsp;
                            </div>
                            <div class="field-div1 w80">
                                <asp:TextBox ID="txtBillingAddressLine3" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row-div clearfix" id="trPOBoxAddressLine3" runat="server" visible="false">
                            <div class="label-div emptyspace w18">
                                &nbsp;
                            </div>
                            <div class="field-div1 w80">
                                <asp:TextBox ID="txtPOBoxAddressLine3" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row-div clearfix" id="trCity" runat="server">
                            <div class="label-div w18">
                                <asp:Label ID="lblCityStateZip" runat="server">City:</asp:Label>
                            </div>
                            <div class="field-div1 w32">
                                <asp:TextBox ID="txtCity" runat="server"></asp:TextBox>
                            </div>
                            <div class="label-div1 w12-5">
                                <asp:Label ID="Label5" runat="server">State:</asp:Label>
                            </div>
                            <div class="field-div2 w32">
                                <asp:DropDownList ID="cmbState" runat="server" CssClass="w99">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row-div clearfix" id="trHomeCity" runat="server" visible="false">
                            <div class="label-div w18">
                                <asp:Label ID="Label9" runat="server">City:</asp:Label>
                            </div>
                            <div class="field-div1 w32">
                                <asp:TextBox ID="txtHomeCity" runat="server"></asp:TextBox>
                            </div>
                            <div class="label-div1 w12-5">
                                <asp:Label ID="Label111" runat="server">State:</asp:Label>
                            </div>
                            <div class="field-div2 w32">
                                <asp:DropDownList ID="cmbHomeState" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row-div clearfix" id="trBillingCity" runat="server" visible="false">
                            <div class="label-div w18">
                                <asp:Label ID="Label10" runat="server">City:</asp:Label>
                            </div>
                            <div class="field-div1 w32">
                                <asp:TextBox ID="txtBillingCity" runat="server"></asp:TextBox>
                            </div>
                            <div class="label-div1 w12-5">
                                <asp:Label ID="Label22" runat="server">State:</asp:Label>
                            </div>
                            <div class="field-div2 w32">
                                <asp:DropDownList ID="cmbBillingState" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row-div clearfix" id="trPOBoxCity" runat="server" visible="false">
                            <div class="label-div w18">
                                <asp:Label ID="Label11" runat="server">City:</asp:Label>
                            </div>
                            <div class="field-div1 w32">
                                <asp:TextBox ID="txtPOBoxCity" runat="server"></asp:TextBox>
                            </div>
                            <div class="label-div1 w12-5">
                                <asp:Label ID="Label14" runat="server">State:</asp:Label>
                            </div>
                            <div class="field-div2 w32">
                                <asp:DropDownList ID="cmbPOBoxState" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row-div clearfix" id="trCountry" runat="server">
                            <div class="label-div w18">
                                <asp:Label ID="lblCountry" runat="server">Country:</asp:Label>
                            </div>
                            <div class="field-div1 w32">
                                <asp:DropDownList ID="cmbCountry" runat="server" AutoPostBack="true">
                                </asp:DropDownList>
                            </div>
                            <div class="label-div1 w12-5">
                                <asp:Label ID="Label23" runat="server">ZIP Code:</asp:Label>
                            </div>
                            <div class="field-div2 w32">
                                <asp:TextBox ID="txtZipCode" class="field-div" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row-div clearfix" id="trHomeCountry" runat="server" visible="false">
                            <div class="label-div w18">
                                <asp:Label ID="Label13" runat="server">Country:</asp:Label>
                            </div>
                            <div class="field-div1 w32">
                                <asp:DropDownList ID="cmbHomeCountry" runat="server" AutoPostBack="true">
                                </asp:DropDownList>
                            </div>
                            <div class="label-div1 w12-5">
                                <asp:Label ID="Label24" runat="server">ZIP Code:</asp:Label>
                            </div>
                            <div class="field-div2 w32">
                                <asp:TextBox ID="txtHomeZipCode" class="field-div" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row-div clearfix" id="trBillingCountry" runat="server" visible="false">
                            <div class="label-div w18">
                                <asp:Label ID="Label16" runat="server">Country:</asp:Label>
                            </div>
                            <div class="field-div1 w32">
                                <asp:DropDownList ID="cmbBillingCountry" runat="server" AutoPostBack="true">
                                </asp:DropDownList>
                            </div>
                            <div class="label-div1 w12-5">
                                <asp:Label ID="Label19" runat="server">ZIP Code:</asp:Label>
                            </div>
                            <div class="field-div2 w32">
                                <asp:TextBox ID="txtBillingZipCode" class="field-div" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row-div clearfix" id="trPOBoxCountry" runat="server" visible="false">
                            <div class="label-div w18">
                                <asp:Label ID="Label17" runat="server">Country:</asp:Label>
                            </div>
                            <div class="field-div1 w32">
                                <asp:DropDownList ID="cmbPOBoxCountry" runat="server" AutoPostBack="true">
                                </asp:DropDownList>
                            </div>
                            <div class="label-div1 w12-5">
                                <asp:Label ID="Label20" runat="server">ZIP Code:</asp:Label>
                            </div>
                            <div class="field-div2 w32">
                                <asp:TextBox ID="txtPOBoxZipCode" class="field-div" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row-div clearfix">
                            <div class="label-div w18">
                                <asp:Label ID="lblPhone" runat="server">(Area Code) Phone:</asp:Label>
                            </div>
                            <div class="field-div1 w32">
                            <div class="field-div1 w25">
                                <rad:radmaskedtextbox id="txtPhoneAreaCode" runat="server" mask="(###)">
                                </rad:radmaskedtextbox></div>
                                <div class="field-div2 w73">
                           <rad:radmaskedtextbox id="txtPhone" runat="server" mask="###-####">
                                </rad:radmaskedtextbox></div></div>
                             
                             
                           
                        </div>
                        <div class="row-div clearfix">
                            <div class="label-div w18">
                                <asp:Label ID="lblFax" runat="server">(Area Code) Fax:</asp:Label>
                            </div>
                            <div class="field-div1 w32">
                            <div class="field-div1 w25">
                                <rad:radmaskedtextbox id="txtFaxAreaCode" runat="server" mask="(###)">
                                </rad:radmaskedtextbox></div>
                            <div class="field-div2 w73"><rad:radmaskedtextbox id="txtFaxPhone" runat="server" mask="###-####">
                                </rad:radmaskedtextbox></div>
                             </div>
                             
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlAddressType" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="cmbCountry" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="chkPrefAddress" EventName="CheckedChanged" />
                    <asp:AsyncPostBackTrigger ControlID="cmbBillingCountry" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="cmbHomeCountry" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="cmbPOBoxCountry" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div id="trmemberinfo" runat="server" class="membership-info-container">
            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                <ContentTemplate>
                    <div class="profile-title-bg">
                        <div runat="server" class="membership-info-title">
                            Membership Information
                        </div>
                    </div>
                    <div class="profile-info-data">
                        <div class="row-div clearfix">
                            <div class="label-div">
                                <asp:Label ID="lblmembershipType" runat="server">Membership Type:</asp:Label>
                            </div>
                            <div class="field-div">
                                <asp:Label ID="lblMemberTypeVal" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="row-div clearfix">
                            <div class="label-div">
                                <asp:Label ID="lblStartDate" runat="server">Start Date:</asp:Label>
                            </div>
                            <div class="field-div">
                                <asp:Label ID="lblStartDateVal" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="row-div clearfix">
                            <div class="label-div">
                                <asp:Label ID="lblEndDate" runat="server">End Date:</asp:Label>
                            </div>
                            <div class="field-div">
                                <asp:Label ID="lblEndDateVal" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="row-div clearfix">
                            <div class="label-div">
                                <asp:Label ID="lblStatus" runat="server">Status:</asp:Label>
                            </div>
                            <div class="field-div">
                                <asp:Label ID="lblStatusVal" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="topicofInterest-info-container">
            <div class="profile-title-bg">
                <div class="topicofInterest-info-title">
                    Select Topics of Interest to You
                </div>
            </div>
            <div class="profile-info-data">
                <div class="topic-list-div">
                    <asp:CheckBoxList ID="cblTopicofInterest" runat="server" RepeatColumns="4" RepeatDirection="Horizontal">
                    </asp:CheckBoxList>
                </div>
            </div>
        </div>
        <div class="rDivRow">
            <asp:Button ID="btnSubmit" runat="server" CssClass="submit-Btn" Text="Submit" ValidationGroup="ProfileControl" />
            &nbsp;<asp:Button ID="btnCancel" runat="server" CssClass="submit-Btn" Text="Cancel"
                CausesValidation="False" />
            <asp:HiddenField ID="HiddenField1" runat="server" />
        </div>
    </div>
</div>
<rad:radwindow id="radwinPassword" runat="server" class="popup-win-profile-changepassword"
    modal="True" visiblestatusbar="False" behaviors="None" title="Change Password"
    skin="Default">
    <ContentTemplate>
        <asp:UpdatePanel ID="updatepnl" runat="server">
            <ContentTemplate>
                <div id="tblLogin" class="top-margin">
                    <asp:Label ID="Label6" runat="server"></asp:Label>
                    <div id="tblData">
                        <div class="row-div clearfix ">
                         <div class="label-div w34">
                            <span class="required-label">*</span>
                            <asp:Label ID="Label15" runat="server">Current Password:</asp:Label>
                            </div>
                            <div class="field-div1 w54">
                            <asp:TextBox ID="txtoldpassword" runat="server" TextMode="Password"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row-div clearfix ">
                          <div class="label-div w34">
                            <span class="required-label">*</span>
                             <asp:Label ID="lblPassword" runat="server">New Password:</asp:Label>
                            </div>
                            <div class="field-div1 w54">
                                <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row-div clearfix ">
                         <div class="label-div w34">
                            <span class="required-label">*</span>
                            <asp:Label ID="Label18" runat="server">Repeat Password:</asp:Label>
                             </div>
                            <div class="field-div1 w54">
                            <asp:TextBox ID="txtRepeat" runat="server" TextMode="Password"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="top-margin align-center">
                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClientClick="return ShowvalidationErrorMsg()"
                            CssClass="submit-Btn" />
                        <asp:Button ID="btnCancelpop" runat="server" Text="Cancel" CssClass="submit-Btn" CausesValidation="false"
                             />
                    </div>
                </div>
                <div>
                    <div class="tdValidationErrormessage">
                        <asp:Label ID="lblErrormessage" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="tdValidationcolor tdValidationErrormessage">
                        <asp:CompareValidator ID="CompareValidator" runat="server" ControlToValidate="txtRepeat"
                            Display="Dynamic" ControlToCompare="txtNewPassword" ErrorMessage="The new passwords must match. Please try again."></asp:CompareValidator>
                    </div>
                    <div class="tdCompairvalidationErrormessage">
                        <asp:Label ID="lblerrorLength" runat="server"></asp:Label>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnSave" />
                <asp:PostBackTrigger ControlID="btnCancelpop" />
            </Triggers>
        </asp:UpdatePanel>
    </ContentTemplate>
</rad:radwindow>
<rad:radwindow id="radDuplicateUser" runat="server" class="popup-win-profile-DuplicateUser"
    modal="True" skin="Default" visiblestatusbar="False" behaviors="None" iconurl="~/Images/Alert.png"
    title="Alert" behavior="None">
    <ContentTemplate>
        <div>
            <div align="left">
                <asp:Label ID="lblAlert" runat="server" Text="The system has detected a conflict between the name and email address you entered and existing information we have on file. Please use a different email address or contact Customer Service for assistance."
                    Font-Bold="true"></asp:Label>
            </div>
            <div align="center">
                <asp:Button ID="btnok" runat="server" Text="OK"  class="submit-Btn" OnClick="btnok_Click"
                    ValidationGroup="ok" />&nbsp;&nbsp;
            </div>
        </div>
    </ContentTemplate>
</rad:radwindow>
<rad:radwindow id="radwindowProfileImage" runat="server" class="popup-win-profile-profileImage"
    modal="True" visiblestatusbar="False" behaviors="None" title="Profile Image"
    behavior="None" skin="Default" iconurl="~/Images/personal-icon.png">
    <ContentTemplate>
        <panel id="ProfileImagePanel" runat="server">
           <div>
        <div>
            <div>
                <asp:Label ID="LableImageUploadText" runat="server" > </asp:Label>
                <br />
                <div style="display: none">
                    <asp:Label ID="LableImageSaveIndicator" runat="server" 
                        Visible="false"></asp:Label>
                </div>
            </div>
            <div>
                <div>
                    <rad:radupload runat="server" id="radUploadProfilePhoto" controlobjectsvisibility="None"
                        onclientfileselected="UploadImage" localization-select="Browse..." allowedfileextensions=".gif, .jpg, .bmp, png"
                        />
                </div>
                <div>
                    &nbsp;&nbsp;<asp:Button ID="btnRemovePhoto" runat="server" CausesValidation="False"
                        Text="Remove" CssClass="submit-Btn" />
                    <asp:Button ID="btnUpload" runat="server" CssClass="submit-Btn" CausesValidation="False"
                         Text="Upload" Style="display: none"  />
                </div>
            </div>
        </div>
        <div>
            <rad:radimageeditor id="radImageEditor" runat="server"
                allowedsavinglocation="Server" onclientimagechanged="EnabledImageSaveButton"
                onclientimageload="ZoomBestFit" onclienttoolsdialogclosed="OnClientToolsDialogClosed"
                onclientimagechanging="OnClientImageChanging" onclientcommandexecuted="OnClientCommandExecuted"
                canvasmode="No">
                <Tools>
                <rad:ImageEditorToolGroup>
                    <rad:ImageEditorToolStrip CommandName="Undo"></rad:ImageEditorToolStrip>
                    <rad:ImageEditorToolStrip CommandName="Redo"></rad:ImageEditorToolStrip>
                    <rad:ImageEditorTool Text="Reset" CommandName="Reset" />
                    <rad:ImageEditorToolSeparator>
                    </rad:ImageEditorToolSeparator>
                    <rad:ImageEditorTool Text="ZoomIn" CommandName="ZoomIn" />
                    <rad:ImageEditorTool Text="ZoomOut" CommandName="ZoomOut" />
                    <rad:ImageEditorToolSeparator></rad:ImageEditorToolSeparator>
                    <rad:ImageEditorTool CommandName="Crop"></rad:ImageEditorTool>
                </rad:ImageEditorToolGroup>
                </Tools>
                </rad:radimageeditor>
            <br />
            <asp:Label ID="lblCropTip" runat="server"  Text="After cropping a photo, click Crop and then Apply."></asp:Label>
            <asp:Label ID="lblIEUserMsg" runat="server" 
                Text="Internet Explorer users may need to refresh the image before cropping."></asp:Label>
        </div>
        <div align="center" class="top-margin">
            
                <asp:Button ID="btnCropImage" class="submit-Btn" runat="server" Text="Crop"
                    OnClientClick="return CropImage();" />
            
            
                <asp:Button ID="btnSaveProfileImage" class="submit-Btn" runat="server" 
                    Text="Apply" CausesValidation="false"  UseSubmitBehavior="false" />
           
                <asp:Button ID="btnCancelProfileImage" runat="server" Text="Cancel"
                    class="submit-Btn" OnClick="btnCancelProfileImage_Click" CausesValidation="false"
                   />
           
        </div>
    </div>
	   </panel>
    </ContentTemplate>
</rad:radwindow>
<cc1:user id="User1" runat="server"></cc1:user>
<cc3:aptifywebuserlogin id="WebUserLogin1" runat="server"></cc3:aptifywebuserlogin>
<cc4:aptifyshoppingcart id="ShoppingCart1" runat="server" />
<uc2:Sitefinity4xSSO ID="Sitefinity4xSSO1" runat="server" />
