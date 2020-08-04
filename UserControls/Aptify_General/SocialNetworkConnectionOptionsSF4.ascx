<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Register Src="Sitefinity4xSSO.ascx" TagName="Sitefinity4xSSO" TagPrefix="uc1" %>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SocialNetworkConnectionOptionsSF4.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.SocialNetworkConnectionOptions" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessLogin" %>
<%@ Register TagPrefix="uc1" TagName="SocialNetworkingIntegrationControlSF4" Src="~/UserControls/Aptify_General/SocialNetworkingIntegrationControlSF4.ascx" %>
<script type="text/javascript">
    function EnableDisableOptionControls(chk) {
        var txtUID = document.getElementById('<%= txtUserID.ClientID%>')
        var txtPWD = document.getElementById('<%= txtPassword.ClientID%>')
        var chkSynchronizeProfile = document.getElementById('<%= chkSynchronizeProfile.ClientID%>')
        if (chk.getAttribute("id") == '<%= rdoExistingUser.ClientID%>') {

            if (chk.checked) {
                txtUID.disabled = false;
                txtPWD.disabled = false;
                chkSynchronizeProfile.disabled = false;
            }
            else {
                txtUID.disabled = true;
                txtPWD.disabled = true;
                /* chkSynchronizeProfile.disabled = true; */
            }
        }
        else {
            txtUID.disabled = true;
            txtPWD.disabled = true;
            /*  chkSynchronizeProfile.disabled = true; */
        }
    }
    function EnableDisablePhotoOption(chk) {
        var chkUseSocialMediaPhoto = document.getElementById('<%= chkUseSocialMediaPhoto.ClientID%>')
        if (chk.getAttribute("id") == '<%=chkSynchronizeProfile.ClientID%>') {

            if (chk.checked) {

                chkUseSocialMediaPhoto.disabled = false;
            }
            else {
                chkUseSocialMediaPhoto.disabled = true;
                chkUseSocialMediaPhoto.checked = false;
            }
        }
    }
</script>
<div id="loginTop" class="table-div" runat="server">
    <div class="row-div clearfix">
        <h1 runat="server" id="lblLogin">
            Login
        </h1>
    </div>
    <div id="tblLogin" runat="server" class="row-div top-margin">
        <div class="row-div clearfix">
            <asp:RadioButton ID="rdoExistingUser" runat="server" GroupName="ConfirmationOption"
                Checked="true" />
            <asp:Label ID="LblrdoExistingUser" runat="server">
            </asp:Label>
        </div>
        <div id="tblData" runat="server" class="Social-Connection-OPtion-BlankSpace-TwoRow">
            <div class="row-div clearfix">
                <div class="label-div w19">
                    <asp:Label ID="lblUserID" runat="server">
							<b>
								User ID 
							</b>
                    </asp:Label>
                </div>
                <div class="field-div1 w80">
                    <asp:TextBox ID="txtUserID" runat="server">
                    </asp:TextBox>
                </div>
            </div>
            <div class="row-div clearfix">
                <div class="label-div w19">
                    <asp:Label ID="lblPassword" runat="server">
							<b>
								Password
							</b>
                    </asp:Label>
                </div>
                <div class="field-div1 w80">
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password">
                    </asp:TextBox>
                </div>
            </div>
            <div class="row-div">
                <div class="top-margin">
                    <asp:RadioButton ID="rdoNewUser" runat="server" GroupName="ConfirmationOption" />
                    <asp:Label ID="lblrdoNewUser" runat="server">
                    </asp:Label>
                </div>
                <div class="top-margin">
                    <asp:CheckBox ID="chkSynchronizeProfile" runat="server" TextAlign="right" />
                    <asp:Label ID="lblchkSynchronizeProfile" runat="server">
                    </asp:Label>
                </div>
                <div class="top-margin">
                    <asp:CheckBox ID="chkUseSocialMediaPhoto" runat="server" Enabled="False" />
                    <asp:Label ID="lblchkUseSocialMediaPhoto" runat="server">
                    </asp:Label>
                </div>
                <div class="top-margin">
                    <asp:CheckBox ID="chkRememberMe" runat="server" AutoPostBack="True" />
                    <asp:Label ID="lblchkRememberMe" runat="server" Text="Keep me signed in.">
                    </asp:Label>
                </div>
                <div class="top-margin">
                    <asp:UpdatePanel ID="pnl1" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lbl" runat="server" Visible="false">
                            </asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="chkRememberMe" EventName="" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="top-margin">
                    <asp:HyperLink ID="hypSocialNetworkSynchText" runat="server" Target="_new" ForeColor="Red">
                    </asp:HyperLink>
                </div>
                <div>
                    Need Help?
                    <asp:HyperLink ID="hypContactUS" runat="server" Target="_new">
							Contact Us
                    </asp:HyperLink>
                </div>
            </div>
        </div>
        <div class="row-div">
            <asp:Button ID="btnContinue" runat="server" Text="Continue" CssClass="submitBtn" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="submitBtn" />
            <asp:Label ID="lblError" ForeColor="Crimson" runat="server">
            </asp:Label>
        </div>
    </div>
</div>
<cc1:AptifyWebUserLogin ID="WebUserLogin1" runat="server" Width="175px" Height="9px"
    Visible="False"></cc1:AptifyWebUserLogin>
<uc1:Sitefinity4xSSO ID="Sitefinity4xSSO1" runat="server" />
<div style="display: none">
    <uc1:SocialNetworkingIntegrationControlSF4 ID="SocialNetworkingIntegrationControlSF4"
        runat="server" />
</div>
