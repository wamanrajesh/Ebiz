<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ForgotUID.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.ForgotUIDControl"  %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="WebUserActivity" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessLogin" %>

   
<div id="tblMain" runat="server" class="table-div">
	<div class="row-div">
        If you have forgotten your password, please enter your user ID and click submit.&nbsp;
        A temporary password will then be sent to the email address on file that is associated
        with the user account. &nbsp; This temporary password will only be valid for 24
        hours. &nbsp; If you have forgotten your user ID or do not receive the email with
        your temporary password, please contact
			<asp:HyperLink ID="mailAddress" runat="server">
			</asp:HyperLink>
			 for assistance.
			<hr />
			<%--Neha issue Id:15163 Add CssClass for ErrorMessage--%>
        <asp:Label ID="lblError" runat="server" CssClass="error-msg-label">
        </asp:Label>
				<div class="table-div" id="tblUserName" runat="server">
					<div class="row-div clearfix">
						<div class="label-div w25">
                    <span class="required-label">*</span><asp:Label ID="Label1" runat="server">
								User ID:
                    </asp:Label>
						</div>
						<div class="field-div1 w74">
                    <asp:TextBox ID="txtUID" runat="server" />
                    <asp:RequiredFieldValidator ID="valUID" runat="server" ControlToValidate="txtUID"
                        ErrorMessage=" Required Field" CssClass="error-msg-label" />
						</div>
					</div>
				</div>
				<div class="table-div" id="tblPWDHint" runat="server">
					<div class="row-div clearfix">
						<div class="label-div w25">
							<asp:label id="Label2" runat="server">
								Password Hint:
							</asp:label>
						</div>
						<div class="field-div1 w74">
							<asp:label id="lblHint" runat="server">
							</asp:label>
						</div>
					</div>
					<div class="row-div clearfix">
						<div class="label-div w25">
							<span class="required-label">*</span><asp:label id="Label3" runat="server">
								Answer:
							</asp:label>
						</div>
						<div class="field-div1 w74">
							<asp:textbox id="txtAnswer" runat="server">
							</asp:textbox>
							<asp:requiredfieldvalidator id="valAnswer" runat="server" ControlToValidate="txtAnswer"  ErrorMessage=" Required Field" CssClass="error-msg-label">
							</asp:requiredfieldvalidator>
						</div>
					</div>
				</div>
				<div class="table-div" id="tblPasswordChange" runat="server">
					<div class="row-div clearfix">
						<div class="label-div w25">
							<asp:label id="Label6" runat="server">
								User ID:
							</asp:label>
						</div>
						<div class="field-div1 w74">
							<asp:label id="lblUID" runat="server">
							</asp:label>
						</div>
					</div>
					<div class="row-div clearfix">
						<div class="label-div w25">
							<span class="required-label">*</span><asp:label id="Label4" runat="server">
								Password:
							</asp:label>
						</div>
						<div class="field-div1 w74">
							<asp:textbox id="txtPWD" runat="server" TextMode="Password">
							</asp:textbox>
							<asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" ControlToValidate="txtPWD" ErrorMessage=" Required Field" CssClass="error-msg-label">
							</asp:requiredfieldvalidator>
						</div>
					</div>
					<div class="row-div clearfix">
						<div class="label-div w25">
							<span class="required-label">*</span><asp:label id="Label5" runat="server">
								Repeat Password:
							</asp:label>
						</div>
						<div class="field-div1 w74">
							<asp:textbox id="txtRepeatPWD" runat="server" TextMode="Password">
							</asp:textbox>
							<asp:requiredfieldvalidator id="RequiredFieldValidator2" runat="server" ControlToValidate="txtRepeatPWD"  ErrorMessage=" Required Field" CssClass="error-msg-label">
							</asp:requiredfieldvalidator>
						</div>
					</div>
				</div>
			</div>
		</div>
		<div class="row-div">
			    <p>
        <asp:Button ID="cmdSubmit" runat="server" Text="Submit" CssClass="submit-Btn" />
				</p>
		</div>
<cc2:WebUserActivity ID="WebUserActivity1" runat="server" />
<cc1:AptifyWebUserLogin ID="WebUserLogin1" runat="server" />
