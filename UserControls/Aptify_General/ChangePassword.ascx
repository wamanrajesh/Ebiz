<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ChangePassword.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.ChangePassword" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessLogin" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="cc4" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessShoppingCart" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%--Neha Issue 14408,02/19/13, added function for validation--%>
<script type="text/javascript" language="javascript">
    function ShowvalidationErrorMsg() {
        var cmpValidator = document.getElementById('<%=CompareValidator.ClientID%>');
        if (document.getElementById('<%= txtoldpassword.ClientID %>').value.length == 0 ||
            window.document.getElementById('<%= txtNewPassword.ClientID %>').value.length == 0
            || window.document.getElementById('<%= txtRepeat.ClientID %>').value.length == 0) {
            window.document.getElementById('<%= lblErrormessage.ClientID %>').innerHTML = "All the above fields are mandatory."
            window.document.getElementById('<%= lblErrormessage.ClientID %>').style.display = "Block";
            window.document.getElementById('<%= lblerrorLength.ClientID %>').style.display = "none"
            ValidatorEnable(cmpValidator, false);
            return false;
        }
        else {
            //Neha, Issue 14408,03/20/13, added condition for Passwordvalidation
            if (window.document.getElementById('<%= txtNewPassword.ClientID %>').value != window.document.getElementById('<%= txtRepeat.ClientID %>').value) {
                ValidatorEnable(cmpValidator, true);
                window.document.getElementById('<%= lblErrormessage.ClientID %>').style.display = "none";
                return false;
            }
        }
    }
</script>
<asp:UpdatePanel ID="updatepnl" runat="server">
    <contenttemplate>
        <div class="row-div clearfix">
            <div class="label-div w31">
                &nbsp;</div>
            <div class="field-div">
                <asp:Label ID="lblpwdmsg" runat="server"></asp:Label>
            </div>
        </div>
        <asp:Label ID="Label6" runat="server"></asp:Label>
        <div class="row-div clearfix">
            <div class="label-div w31">
                <asp:Label ID="Label15" runat="server"><span class="required-label">*</span>Current Password:</asp:Label>
            </div>
            <div class="field-div">
                <asp:TextBox ID="txtoldpassword" runat="server"  TextMode="Password"></asp:TextBox>
            </div>
        </div>
        <div class="row-div clearfix">
            <div class="label-div w31">
                <asp:Label ID="lblPassword" runat="server"> <span class="required-label">*</span>New Password:</asp:Label>
            </div>
            <div class="field-div">
                <asp:TextBox ID="txtNewPassword" runat="server"  TextMode="Password"></asp:TextBox>
            </div>
        </div>
        <div class="row-div clearfix">
            <div class="label-div w31">
                <asp:Label ID="Label18" runat="server"><span class="required-label">*</span>Repeat Password:</asp:Label>
            </div>
            <div class="field-div">
                <asp:TextBox ID="txtRepeat" runat="server"  TextMode="Password"></asp:TextBox>
            </div>
        </div>
        <div class="row-div clearfix">
            <div class="label-div w31">
                &nbsp;</div>
            <div class="field-div">
                <asp:Button ID="btnSave" runat="server" Text="Save" OnClientClick="return ShowvalidationErrorMsg()"
                    CssClass="submit-Btn" />
                <asp:Button ID="btnCancelpop" runat="server" Text="Cancel" CssClass="submit-Btn" CausesValidation="false" />
            </div>
        </div>
         
         <div class="row-div1 clearfix ">
          <div class="label-div w31">
                &nbsp;</div>
            <div class="row-div w60 float-left">
            <asp:Label ID="lblerrorLength" runat="server" CssClass="error-msg-label"></asp:Label>
        </div>
        </div>

        <div class="row-div clearfix">
          <div class="label-div w31">
                &nbsp;</div>
           <div class="row-div">
            <asp:Label ID="lblErrormessage" runat="server" Text="" CssClass="error-msg-label"></asp:Label>
            <asp:CompareValidator ID="CompareValidator" runat="server" CssClass="error-msg-label"
                ControlToValidate="txtRepeat" ControlToCompare="txtNewPassword" ErrorMessage="The new passwords must match. Please try again."></asp:CompareValidator>
        </div>
         </div>
     </contenttemplate>
    <triggers>
        <asp:PostBackTrigger ControlID="btnSave" />
        <asp:PostBackTrigger ControlID="btnCancelpop" />
    </triggers>
</asp:UpdatePanel>
<cc1:user id="User1" runat="server">
</cc1:user>
<cc3:aptifywebuserlogin id="WebUserLogin1" runat="server">
</cc3:aptifywebuserlogin>
<cc4:aptifyshoppingcart id="ShoppingCart1" runat="server" />