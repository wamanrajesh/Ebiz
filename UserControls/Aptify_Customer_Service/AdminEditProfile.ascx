<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="AdminEditProfile.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.CustomerService.AdminEditProfile" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessLogin" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="cc4" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessShoppingCart" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="cc2" TagName="Topiccode" Src="~/UserControls/Aptify_General/TopicCodeViewer.ascx" %>
<script type="text/javascript" language="javascript">
    function ShowImage(ImageURL) {
        if (document.getElementById(ImageURL) != undefined) {
            document.getElementById(ImageURL).style.visibility = "visible";
        }
    }

    function UploadImage() {
        //Neha issue no 14430,03/11/13, disable Apply button 
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
    // Neha issue no 14430, 03/11/13, added Function(enable apply button and applied class)
    function EnabledImageSaveButton() {
        var button = document.getElementById('<%= btnSaveProfileImage.ClientID %>');
        button.className = "submit-Btn";
        button.disabled = false;
    }
    //End   
</script>
<asp:Literal ID="ltlImageEditorStyle" runat="server"></asp:Literal>
<div id="Container" class="profile-main-div clearfix">
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <div class="left-container w18">
                <div id="UpperDiv" class="ProfileUpperDiv">
                    <div id="tblProfileImg">
                        <div class="profile-image">
                            <asp:Image ID="imgProfile" runat="server" ClientIDMode="AutoID" CssClass="AdminProfileImageBorder" />
                        </div>
                        <asp:Image ID="imgEditProfileImage" runat="server" ImageUrl="~/Images/Edit.png" Style="width: 12px" />
                        <asp:LinkButton ID="lbtnOpenProfileImage" CssClass="edit-lnk-btn" runat="server" Text="Edit"
                            CausesValidation="false" />
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="lbtnOpenProfileImage" />
            <%--<asp:AsyncPostBackTrigger ControlID="btnRemovePhoto"  EventName="Click" />
            <asp:PostBackTrigger ControlID="btnUpload" />--%>
        </Triggers>
    </asp:UpdatePanel>
    <div id="tblMain" runat="server" class="right-container w80">
        <div class="row-div">
            <asp:Label ID="lblmsg" runat="server" Text="" ForeColor="#d07b0c"></asp:Label>
            <div align="right">
                <asp:Label ID="lblemail" runat="server" Text="Click the Send Notification button to notify this person that you have updated his or her profile"></asp:Label>
                <asp:Button ID="btnsend" runat="server" Text="Send Notification" class="submit-Btn" />
            </div>
        </div>
        <div class="personal-info-container">
            <div class="profile-title-bg clearfix">
                <div class="personal-info-title float-left">
                    Personal Information</div>
                <div class="float-right">
                    <asp:Image ID="EditImage1" runat="server" ImageUrl="~/Images/Edit.png" />
                    <asp:LinkButton ID="btnOpenPopup" CssClass="edit-lnk-btn" runat="server" Text="Edit"
                        OnClick="btnOpenPopup_Click" />
                </div>
            </div>
            <div class="profile-info-data">
                <div class="row-div clearfix">
                    <div class="label-div w19">
                        <asp:Label ID="lblName" runat="server">First Name:</asp:Label>
                    </div>
                    <div class="field-div1 w79">
                        <asp:Label ID="lblEditFirstName" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="row-div clearfix">
                    <div class="label-div w19">
                        <asp:Label ID="Label4" runat="server">Last Name:</asp:Label>
                    </div>
                    <div class="field-div1 w79">
                        <asp:Label ID="lblEditLastName" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="row-div clearfix">
                    <div class="label-div w19">
                        <asp:Label ID="lblCompany" runat="server">Company:</asp:Label>
                    </div>
                    <div class="field-div1 w79">
                        <%-- <asp:TextBox ID="txtCompany" CssClass="txtBoxEditProfile" runat="server"></asp:TextBox>--%>
                        <asp:Label ID="lblEditCompany" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="row-div clearfix" id="trTitle" runat="server">
                    <div class="label-div w19">
                        <asp:Label ID="lblTitle" runat="server">Title:</asp:Label>
                    </div>
                    <div class="field-div1 w79">
                        <%--<asp:TextBox ID="txtTitle" CssClass="txtBoxEditProfile" runat="server"></asp:TextBox>--%>
                        <asp:Label ID="lblJobFunction" runat="server"></asp:Label>
                    </div>
                </div>
                <%-- Amruta IssueID 14307--%>
                <div class="row-div clearfix" id="trEmail" runat="server">
                    <div class="label-div w19">
                        <asp:Label ID="lblEmailID" runat="server">Email:</asp:Label>
                    </div>
                    <div class="field-div1 w79">
                        <asp:Label ID="lblEmailAddress" runat="server"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
        <div class="contact-info-container">
            <div class="profile-title-bg clearfix">
                <div class="contact-info-title float-left">
                    Contact Information</div>
                <div class="float-right">
                    <asp:Image ID="EditImage2" runat="server" ImageUrl="~/Images/Edit.png" />
                    <asp:LinkButton ID="contact" CssClass="edit-lnk-btn" runat="server" Text="Edit"></asp:LinkButton>
                </div>
            </div>
            <div class="profile-info-data">
                <div class="row-div clearfix">
                    <div class="label-div w19" id="lblPerAddressTitle" runat="server">
                        Preferred Address:
                    </div>
                    <div class="field-div1 w79">
                        <asp:Label ID="lblPerferredAddress" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="w98 padding-all clearfix">
                    <div class="float-left w25">
                        <div id="tdBusinessAdd" class="label" runat="server">
                            Business Address:
                        </div>
                        <div id="tdBusinessAddVal" runat="server">
                            <asp:Label ID="BusinessAddressval" runat="server"></asp:Label>
                            <asp:Label ID="BusinessAdd1" runat="server"></asp:Label>
                            <asp:Label ID="BusinessAdd2" runat="server"></asp:Label>
                            <asp:Label ID="BusinessAdd3" runat="server"></asp:Label>
                            <asp:Label ID="BusinessCityState" runat="server"></asp:Label>
                            <asp:Label ID="BusinessCountry" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="float-left w25">
                        <div id="tdHomeAdd" class="label" runat="server">
                            Home Address:
                        </div>
                        <div class="" id="tdHomeAddVal" runat="server">
                            <asp:Label ID="HomeAddressval" runat="server" Visible="false"></asp:Label>
                            <asp:Label ID="HomeAdd1" runat="server"></asp:Label>
                            <asp:Label ID="HomeAdd2" runat="server"></asp:Label>
                            <asp:Label ID="HomeAdd3" runat="server"></asp:Label>
                            <asp:Label ID="HomeCityState" runat="server"></asp:Label>
                            <asp:Label ID="HomeCountry" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="float-left w25">
                        <div id="tdBillingAdd" class="label" runat="server">
                            Billing Address:
                        </div>
                        <div id="tdBillingAddVal" runat="server">
                            <asp:Label ID="BillingAddressval" Visible="false" runat="server"></asp:Label>
                            <asp:Label ID="BillingAdd1" runat="server"></asp:Label>
                            <asp:Label ID="BillingAdd2" runat="server"></asp:Label>
                            <asp:Label ID="BillingAdd3" runat="server"></asp:Label>
                            <asp:Label ID="BillingCityState" runat="server"></asp:Label>
                            <asp:Label ID="BillingAddCountry" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="float-left w25">
                        <div id="tdPoboxAdd" class="label" runat="server">
                            PO Box Address:
                        </div>
                        <div id="tdPoboxAddVal" runat="server">
                            <asp:Label ID="PoBoxAddress" runat="server" Visible="false"></asp:Label>
                            <asp:Label ID="PoBoxAdd1" runat="server"></asp:Label>
                            <asp:Label ID="PoBoxAdd2" runat="server"></asp:Label>
                            <asp:Label ID="PoBoxAdd3" runat="server"></asp:Label>
                            <asp:Label ID="PoBoxCityState" runat="server"></asp:Label>
                            <asp:Label ID="PoBoxCountry" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
                <div class="row-div clearfix">
                    <div class="label-div w19" id="lblphonetitle" runat="server">
                        (Area Code) Phone:
                    </div>
                    <div class="field-div1">
                        <asp:Label ID="lblphoneVal" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="row-div clearfix">
                    <div class="label-div w19" id="lblFaxtitle" runat="server">
                        (Area Code) Fax:
                    </div>
                    <div class="field-div1">
                        <asp:Label ID="lblFaxVal" runat="server"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
        <div class="row-div clearfix" runat="server" id="trWebAccount">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="personal-info-container">
                        <div class="profile-title-bg">
                            <div id="Div1" runat="server" class="membership-info-title">
                                Membership Information
                            </div>
                        </div>
                        <div class="profile-info-data">
                            <div class="row-div clearfix">
                                <div class="label-div w19">
                                    <asp:Label ID="lblmembershipType" runat="server">Membership Type:</asp:Label>
                                </div>
                                <div class="field-div1">
                                    <asp:Label ID="lblMemberTypeVal" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row-div clearfix">
                                <div class="label-div w19">
                                    <asp:Label ID="lblStartDate" runat="server">Start Date:</asp:Label>
                                </div>
                                <div class="field-div1">
                                    <asp:Label ID="lblStartDateVal" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row-div clearfix">
                                <div class="label-div w19">
                                    <asp:Label ID="lblEndDate" runat="server">End Date:</asp:Label>
                                </div>
                                <div class="field-div1">
                                    <asp:Label ID="lblEndDateVal" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row-div clearfix">
                                <div class="label-div w19">
                                    <asp:Label ID="lblStatus" runat="server">Status:</asp:Label>
                                </div>
                                <div class="field-div1">
                                    <asp:Label ID="lblStatusVal" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="topicofInterest-info-container">
            <div class="profile-title-bg clearfix">
                <div class="topicofInterest-info-title float-left">
                    Topics of Interest</div>
                <div class="float-right">
                    <asp:Image runat="server" ID="EditImage3" ImageUrl="~/Images/Edit.png" />
                    <asp:LinkButton ID="btnTopicIntrest" runat="server" Text="Edit" CssClass="edit-lnk-btn"
                        OnClick="btnTopicIntrest_Click" />
                </div>
            </div>
            <div class="profile-info-data">
                <div class="row-div clearfix">
                    <div class="topic-list-div">
                        <asp:Label ID="lblTopicIntrest" runat="server"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<cc1:user id="User1" runat="server">
</cc1:user>
<cc3:aptifywebuserlogin id="WebUserLogin1" runat="server">
</cc3:aptifywebuserlogin>
<cc4:aptifyshoppingcart id="ShoppingCart1" runat="server" totalitemsremovedbyremoveitem="0" />
<rad:radwindow id="radwindowPopup" runat="server" cssclass="popup-win-adminedit-personalInfo" 
    modal="True" visiblestatusbar="False" behaviors="None" title="Personal Information"
    behavior="None" skin="Default" iconurl="~/Images/personal-icon.png">
    <contenttemplate>
    <panel id="PersonalInfoPanel" runat="server">
        <div class="row-div clearfix top-margin">
              <div class="row-div clearfix">
                    <div class="label-div w30">
                    <span class="RequiredField"> *</span>
                        <asp:Label ID="lblFirstName" Text="First Name:" runat="server"></asp:Label>
                    </div>
                    <div class="field-div1 w50">
                    <asp:TextBox ID="txtEditFirstName" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtEditFirstName"
                    ValidationGroup="EditProfileControl" ErrorMessage="First Name Required" Display="Dynamic"
                    ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>
               </div>
                <div class="row-div clearfix">
                    <div class="label-div w30">
                        <span class="RequiredField"> *</span>
                        <asp:Label ID="lblLastName" Text="Last Name:" runat="server"></asp:Label>
                    </div>
                    <div class="field-div1 w50">
                        <asp:TextBox ID="txtEditLastName" runat="server" ></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtEditLastName"
                        ValidationGroup="EditProfileControl" ErrorMessage="Last Name Required" ForeColor="Red"
                        Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row-div clearfix">
                    <div class="label-div w30">
                        <asp:Label ID="lblEditJobFunction" Text="Title:" runat="server"></asp:Label>
                    </div>
                    <div class="field-div1 w50"> 
                        <asp:TextBox ID="txtEditJobFunction" runat="server" ></asp:TextBox>
                    </div>
                </div>
                <div class="row-div clearfix">
                    <div class="label-div w30">
                    &nbsp;
                    </div>
                    <div class="field-div1 w68 top-margin">
                        <asp:Button ID="btnOk" class="submit-Btn" runat="server" Text="Save" OnClick="btnOk_Click" ValidationGroup="EditProfileControl"/>
                        <asp:Button ID="Button1" runat="server" Text="Cancel" class="submit-Btn" OnClick="btnCancel_Click" />
                    </div>
                </div>   
        </div>       
	   </panel>
    </contenttemplate>
</rad:radwindow>
<rad:radwindow id="radwindowcontact" runat="server" class="popup-win-adminedit-Contactinfo"
    modal="True" skin="Default" visiblestatusbar="False" behaviors="None" title="Contact Information"
    behavior="None" iconurl="~/Images/contact-icon.png">
    <contenttemplate>
        <div class="popup-contact-info-container">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <div class="top-margin row-div clearfix">
                        <div class="label-div w18">
                            Address Type:
                        </div>
                        <div class="field-div1 w80">
                        <div class="float-left w48">
                            <asp:DropDownList ID="ddlAddressType" runat="server"
                                AutoPostBack="True">
                                <asp:ListItem>Business Address</asp:ListItem>
                                <asp:ListItem>Home Address</asp:ListItem>
                                <asp:ListItem>Billing Address</asp:ListItem>
                                <asp:ListItem>PO Box Address</asp:ListItem>
                            </asp:DropDownList>
                            </div>
                            <div class="float-left w48 clearfix"><asp:CheckBox ID="chkPrefAddress" runat="server" Text="Preferred Address"
                                AutoPostBack="True" />
                                </div>
                        </div>
                    </div>
                    <!-- Address Line Rows -->
                    <div class="row-div clearfix" id="trAddressLine1" runat="server">
                        <div class="label-div w18" id="Td1" runat="server">
                            <asp:Label ID="lblAddress" runat="server" >Address:</asp:Label>
                        </div>
                        <div class="field-div1 w80">
                            <asp:TextBox ID="txtAddressLine1" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row-div clearfix" id="trHomeAddressLine1" runat="server" visible="False">
                        <div class="label-div w18" id="Td3" runat="server">
                            <asp:Label ID="Label2" runat="server" Font-Bold="true">Address:</asp:Label>
                        </div>
                        <div class="field-div1 w80">
                            <asp:TextBox ID="txtHomeAddressLine1" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row-div clearfix" id="trBillingAddressLine1" runat="server" visible="False">
                        <div class="label-div w18">
                            <asp:Label ID="Label3" runat="server">Address:</asp:Label>
                        </div>
                        <div class="field-div1 w80">
                            <asp:TextBox ID="txtBillingAddressLine1" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row-div clearfix" id="trPOBoxAddressLine1" runat="server" visible="False">
                        <div class="label-div w18">
                            <asp:Label ID="Label8" runat="server" >Address:</asp:Label>
                        </div>
                        <div class="field-div1 w80">
                            <asp:TextBox ID="txtPOBoxAddressLine1" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <!-- Address Line 2 Rows -->
                    <div class="row-div clearfix" id="trAddressLine2" runat="server">
                        <div class="label-div emptyspace w18">
                            &nbsp;
                        </div>
                        <div class="field-div1 w80">
                            <asp:TextBox ID="txtAddressLine2" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row-div clearfix" id="trHomeAddressLine2" runat="server" visible="False">
                        <div class="label-div emptyspace w18">
                            &nbsp;
                        </div>
                        <div class="field-div1 w80">
                            <asp:TextBox ID="txtHomeAddressLine2" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row-div clearfix" id="trBillingAddressLine2" runat="server" visible="False">
                        <div class="label-div emptyspace w18">
                            &nbsp;
                        </div>
                        <div class="field-div1 w80">
                            <asp:TextBox ID="txtBillingAddressLine2" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row-div clearfix" id="trPOBoxAddressLine2" runat="server" visible="False">
                        <div class="label-div emptyspace w18">
                            &nbsp;
                        </div>
                        <div class="field-div1 w80">
                            <asp:TextBox ID="txtPOBoxAddressLine2" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <!-- Address Line 3 Rows -->
                    <div class="row-div clearfix" id="trAddressLine3" runat="server">
                        <div class="label-div emptyspace w18">
                            &nbsp;
                        </div>
                        <div class="field-div1 w80">
                            <asp:TextBox ID="txtAddressLine3"  runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row-div clearfix" id="trHomeAddressLine3" runat="server" visible="False">
                        <div class="label-div emptyspace w18">
                            &nbsp;
                        </div>
                        <div class="field-div1 w80">
                            <asp:TextBox ID="txtHomeAddressLine3"  runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row-div clearfix" id="trBillingAddressLine3" runat="server" visible="False">
                        <div class="label-div emptyspace w18">
                            &nbsp;
                        </div>
                        <div class="field-div1 w80">
                            <asp:TextBox ID="txtBillingAddressLine3" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row-div clearfix" id="trPOBoxAddressLine3" runat="server" visible="False">
                        <div class="label-div emptyspace w18">
                            &nbsp;
                        </div>
                        <div class="field-div1 w80">
                            <asp:TextBox ID="txtPOBoxAddressLine3" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row-div clearfix" id="trCity" runat="server">
                        <div class="label-div w18">
                            <asp:Label ID="lblCityStateZip" runat="server" Font-Bold="true">City:</asp:Label>
                        </div>
                        <div class="field-div1 w32">
                            <asp:TextBox ID="txtCity"  runat="server"></asp:TextBox>
                            </div>
                            <div class="label-div1 w12-5">
                                <asp:Label ID="Label5" runat="server" Font-Bold="true">State:</asp:Label>
                            </div>
                            <div class="field-div2 w32">
                            <asp:DropDownList ID="cmbState"  runat="server">
                            </asp:DropDownList>
                            </div>
                        
                    </div>
                    <div class="row-div clearfix" id="trHomeCity" runat="server" visible="False">
                        <div class="label-div w18">
                            <asp:Label ID="Label9" runat="server">City:</asp:Label>
                        </div>
                            <div class="field-div1 w32">
                            <asp:TextBox ID="txtHomeCity" runat="server"></asp:TextBox>
                            </div>
                            <div class="label-div1 w12-5">
                                <asp:Label ID="Label111" runat="server" Font-Bold="true">State:</asp:Label>
                            </div>
                            <div class="field-div2 w32">
                            <asp:DropDownList ID="cmbHomeState" runat="server">
                            </asp:DropDownList>
                            </div>
                        
                    </div>
                    <div class="row-div clearfix" id="trBillingCity" runat="server" visible="False">
                        <div class="label-div w18">
                            <asp:Label ID="Label10" runat="server" >City:</asp:Label>
                        </div>
                        <div class="field-div1 w32">
                            <asp:TextBox ID="txtBillingCity" runat="server"></asp:TextBox>
                           </div>
                            <div class="label-div1 w12-5">
                                <asp:Label ID="Label22" runat="server" >State:</asp:Label>
                            </div>
                            <div class="field-div2 w32">
                            <asp:DropDownList ID="cmbBillingState" runat="server">
                            </asp:DropDownList>
                            </div>
                        
                    </div>
                    <div class="row-div clearfix" id="trPOBoxCity" runat="server" visible="False">
                        <div class="label-div w18">
                            <asp:Label ID="Label11" runat="server" >City:</asp:Label>
                        </div>
                        <div class="field-div1 w32">
                            <asp:TextBox ID="txtPOBoxCity" runat="server"></asp:TextBox>
                           </div>
                            <div class="label-div1 w12-5">
                                <asp:Label ID="Label14" runat="server" BorderColor="Black" Font-Bold="true">State:</asp:Label>
                            </div>
                            <div class="field-div2 w32">
                            <asp:DropDownList ID="cmbPOBoxState" runat="server">
                            </asp:DropDownList>
                            </div>
                        
                    </div>
                    <div class="row-div clearfix" id="trCountry" runat="server">
                        <div class="label-div w18">
                            <asp:Label ID="lblCountry" runat="server" Font-Bold="true">Country:</asp:Label>
                        </div>
                        <div class="field-div1 w32">
                            <asp:DropDownList CssClass="w74" ID="cmbCountry" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cmbCountry_SelectedIndexChanged">
                            </asp:DropDownList>
                            </div>
                            <div class="label-div1 w12-5">
                                <asp:Label ID="Label23" runat="server" >ZIP Code:</asp:Label>
                            </div>
                            <div class="field-div2 w32">
                            <asp:TextBox ID="txtZipCode" runat="server"></asp:TextBox>
                            </div>
                        
                    </div>
                    <div class="row-div clearfix" id="trHomeCountry" runat="server" visible="False">
                        <div class="label-div w18">
                            <asp:Label ID="Label13" runat="server" >Country:</asp:Label>
                        </div>
                        <div class="field-div1 w32">
                            <asp:DropDownList ID="cmbHomeCountry" runat="server" AutoPostBack="True">
                            </asp:DropDownList>
                            </div>
                            <div class="label-div1 w12-5">
                                <asp:Label ID="Label24" runat="server" >ZIP Code:</asp:Label>
                            </div>
                            <div class="field-div2 w32">
                            <asp:TextBox ID="txtHomeZipCode"  runat="server"></asp:TextBox>
                            </div>
                     </div>
                    
                    <div class="row-div clearfix" id="trBillingCountry" runat="server" visible="False">
                        <div class="label-div w18">
                            <asp:Label ID="Label16" runat="server" >Country:</asp:Label>
                        </div>
                        <div class="field-div1 w32">
                            <asp:DropDownList ID="cmbBillingCountry" runat="server" AutoPostBack="True">
                            </asp:DropDownList>
                           </div>
                            <div class="label-div1 w12-5">
                                <asp:Label ID="Label19" runat="server" >ZIP Code:</asp:Label>
                            </div>
                            <div class="field-div2 w32">
                            <asp:TextBox ID="txtBillingZipCode"  runat="server"></asp:TextBox>
                            </div>
                        
                    </div>
                    <div class="row-div clearfix" id="trPOBoxCountry" runat="server" visible="False">
                        <div class="label-div w18">
                            <asp:Label ID="Label17" runat="server" >Country:</asp:Label>
                        </div>
                        <div class="field-div1 w32">
                            <asp:DropDownList ID="cmbPOBoxCountry" runat="server" AutoPostBack="True">
                            </asp:DropDownList>
                            </div>
                            <div class="label-div1 w12-5">
                                <asp:Label ID="Label20" runat="server" >ZIP Code:</asp:Label>
                            </div>
                            <div class="field-div2 w32">
                            <asp:TextBox ID="txtPOBoxZipCode" runat="server"></asp:TextBox>
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
                         <div class="field-div2 w73">
                            <rad:RadMaskedTextBox ID="txtPhone"  runat="server"
                                Mask="###-####" >
                            </rad:RadMaskedTextBox>
                            <asp:Label ID="lblphonemsg" runat="server" ForeColor="Red"></asp:Label></div>
                        </div>
                    </div>
                    <div class="row-div clearfix">
                        <div class="label-div w18">
                            <asp:Label ID="lblFax" runat="server" >(Area Code) Fax:</asp:Label>
                        </div>
                        <div class="field-div1 w32">
                        <div class="field-div1 w25">
                            <rad:RadMaskedTextBox ID="txtFaxAreaCode" 
                                runat="server" Mask="(###)" >
                            </rad:RadMaskedTextBox></div>
                         <div class="field-div2 w73">
                            <rad:RadMaskedTextBox ID="txtFaxPhone" runat="server" 
                                Mask="###-####" >
                            </rad:RadMaskedTextBox>
                            <asp:Label ID="lblFaxMsg" runat="server" Text="Please Enter Fax Phone"
                                Visible="false" ForeColor="Red"></asp:Label></div>
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
        <div class="align-right">
            <asp:Button ID="btncontactsave" class="submit-Btn" OnClientClick="return validatephone();"
                runat="server" Text="Save" OnClick="btncontactsave_Click" />&nbsp;&nbsp;
            <asp:Button ID="btncontactcancel" runat="server" Text="Cancel" class="submit-Btn"
                OnClick="btncontactcancel_Click" />&nbsp;
        </div>
    </contenttemplate>
</rad:radwindow>
<rad:radwindow id="radtopicintrest" runat="server" class="popup-win-adminedit-topicinterest"
    modal="True" rendermode="Lightweight" visiblestatusbar="False" behaviors="None"
    title="Topics of Interest" behavior="None" iconurl="~/Images/topic-of-int.png"
    skin="Default">
    <contenttemplate>
        <div >
            <div class="row-div clearfix">
                <cc2:Topiccode ID="TopiccodeViewer" runat="server" />
            </div>
            <div class="row-div clearfix">
                <div class="label-div w50">
                    &nbsp;
                </div>
                <div class="field-div1 w40">
                    <asp:Button ID="btnSaveIntrest" runat="server" Text="Save"  class="submit-Btn"
                        OnClick="btnSaveIntrest_Click" />
                    <asp:Button ID="btnCancelIntrest" runat="server" Text="Cancel" class="submit-Btn"
                        OnClick="btnCancelIntrest_Click" />
                </div>
            </div>
        </div>
    </contenttemplate>
</rad:radwindow>
<rad:radwindow id="radwindowProfileImage" runat="server" class="popup-win-profile-profileImage"
    modal="True" visiblestatusbar="False" behaviors="None" title="Profile Image"
    behavior="None" skin="Default" iconurl="~/Images/personal-icon.png">
    <contenttemplate>
        <panel id="ProfileImagePanel" runat="server">
         <div class="table-div">
        <div class="row-div">
       
            <div>
                <asp:Label ID="LableImageUploadText" runat="server"  > </asp:Label>              
                <div style="display: none">
                    <asp:Label ID="LableImageSaveIndicator" runat="server" 
                        Visible="false"></asp:Label>
                </div>
            </div>
          
                <div>
                    <rad:radupload runat="server" id="radUploadProfilePhoto" controlobjectsvisibility="None"
                        onclientfileselected="UploadImage" localization-select="Browse..." allowedfileextensions=".gif, .jpg, .bmp, png"/>
                        &nbsp;&nbsp;<asp:Button ID="btnRemovePhoto" runat="server" CausesValidation="False"
                        Text="Remove" CssClass="submit-Btn" />
                    <asp:Button ID="btnUpload" runat="server" CssClass="submit-Btn" CausesValidation="False"
                        Text="Upload" Style="display: none" />
                </div>
           
        </div>
        <div class="row-div clearfix">
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
           <div class="row-div clearfix" >
            <asp:Label ID="lblCropTip" runat="server" Font-Size="8pt" Font-Names="Segoe UI" Text="After cropping a photo, click Crop and then Apply."></asp:Label>
            <asp:Label ID="lblIEUserMsg" runat="server" Font-Size="8pt" Font-Names="Segoe UI"
                Text="Internet Explorer users may need to refresh the image before cropping."></asp:Label>
                </div>
        </div>
        <div  align="center" class="top-margin"">
               
                <asp:Button ID="btnCropImage" CssClass="submit-Btn" runat="server" Text="Crop"
                    OnClientClick="return CropImage();" />           
                    
                    
                <asp:Button ID="btnSaveProfileImage" CssClass="submit-Btn" runat="server"
                    Text="Apply" CausesValidation="false" UseSubmitBehavior="false" />
               
                <asp:Button ID="btnCancelProfileImage"  CssClass="submit-Btn" runat="server"
                 Text="Cancel" OnClick="btnCancelProfileImage_Click" CausesValidation="false" />
                 
            </div>
            </div>
       
   
	   </panel>
    </contenttemplate>
</rad:radwindow>
