<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="MemberCEUSubmission.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.MemberCEUSubmission" %>
<%@ Register Src="../Aptify_General/DatePicker.ascx" TagName="DatePicker" TagPrefix="uc2" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Src="../Aptify_General/RecordAttachments.ascx" TagName="RecordAttachments"
    TagPrefix="uc1" %>
<%@ Register Assembly="AptifyEBusinessUser" Namespace="Aptify.Framework.Web.eBusiness"
    TagPrefix="cc1" %>
<div id="tblMain" class="border-color" runat="server">
    <div class="header-title-bg">
        Submit New CEU Record
    </div>
    <div class="padding-all">
        <div class="row-div clearfix">
            <div class="label-div w25">
                Type:
            </div>
            <div class="field-div1 w74">
                CEU Submitted Via Member Portal
            </div>
        </div>
        <div class="row-div clearfix">
            <div class="label-div w25">
                Member:
            </div>
            <div class="field-div1 w74">
                <asp:Label ID="txtMember" runat="server" Enabled="False">
                </asp:Label>
                <span class="Error">
                    <asp:Label ID="lblErrorMember" runat="server">
					Error
                    </asp:Label>
                </span>
            </div>
        </div>
        <div class="row-div clearfix">
            <div class="label-div w25">
                Date Started:
            </div>
            <div class="field-div1 w74">
                <telerik:raddatepicker id="dtpDateStarted" cssclass="datePickerCEU" width="185px"
                    runat="server">
            </telerik:raddatepicker>
            </div>
        </div>
        <div class="row-div clearfix">
            <div class="label-div w25">
                Date Granted:
            </div>
            <div class="field-div1 w74">
                <telerik:raddatepicker id="dtpDateGranted" cssclass="datePickerCEU" width="185px"
                    runat="server">
            </telerik:raddatepicker>
            </div>
        </div>
        <div class="row-div clearfix">
            <div class="label-div w25">
                Units Earned:
            </div>
            <div class="field-div1 w74">
                <asp:TextBox ID="txtUnitsEarned" runat="server" CssClass="txtCUESubmission">
                </asp:TextBox>
                <span class="Error">
                    <asp:Label ID="lblErrorUnitsEarned" runat="server">
					Error
                    </asp:Label>
                </span>
            </div>
        </div>
        <div class="row-div clearfix">
            <div class="label-div w25">
                Title:
            </div>
            <div class="field-div1 w74">
                <asp:TextBox ID="txtTitle" runat="server" CssClass="txtCUESubmission">
                </asp:TextBox>
                <span class="Error">
                    <asp:Label ID="lblErrorTitle" runat="server">
					Error
                    </asp:Label>
                </span>
            </div>
        </div>
        <div class="row-div clearfix">
            <div class="label-div w25">
                CEU Type:
            </div>
            <div class="field-div1 w74">
                <asp:DropDownList ID="drpTitle" runat="server" DataTextField="Name" CssClass="txtCUESubmission"
                    DataValueField="ID">
                </asp:DropDownList>
                <span class="Error">
                    <asp:Label ID="lblErrorCEUType" runat="server">
					Error
                    </asp:Label>
                </span>
            </div>
        </div>
        <div class="row-div clearfix">
            <div class="label-div w25">
                Status:
            </div>
            <div class="field-div1 w74">
                <asp:Label ID="lblStatus" runat="server" Text="Declared">
                </asp:Label>
                <span class="Error">
                    <asp:Label ID="lblErrorStatus" runat="server">
					Error
                    </asp:Label>
                </span>
            </div>
        </div>
        <div class="row-div clearfix">
            <div class="label-div w25">
                Expiration Date:
            </div>
            <div class="field-div1 w74">
                <telerik:raddatepicker id="dtpExpirationDate" cssclass="datePickerCEU" width="185px"
                    runat="server">
            </telerik:raddatepicker>
            </div>
        </div>
        <div class="row-div clearfix">
            <div class="label-div w25">
                Document:
            </div>
            <div class="field-div1 w74">
                <telerik:radasyncupload id="radCEUDocumentUpload" viewstatemode="Enabled" localization-select="Browse..."
                    runat="server" localization-remove="Remove" maxfileinputscount="1" cssclass="radFileUploadCEUSubmission">
            </telerik:radasyncupload>
                <span class="Error">
                    <asp:Label ID="lblErrorFile" runat="server">
					Error
                    </asp:Label>
                </span>
            </div>
        </div>
        <div class="row-div clearfix">
            <div class="label-div w25">
                &nbsp;
            </div>
            <div class="field-div1 w74">
                <asp:Button runat="server" ID="inptSubmit" name="tstButton" Text="Submit" CssClass="submit-Btn" />
                <asp:LinkButton ID="lnkGoBack" runat="server">
				Return to Member Certifications
                </asp:LinkButton>
            </div>
        </div>
        <div class="row-div clearfix">
            <span class="Error">
                <asp:Label ID="lblErrorSubmit" runat="server" Visible="false">
					Error
                </asp:Label>
            </span>
            <asp:Label ID="lblSubmitSuccess" runat="server" Visible="False">
				Success
            </asp:Label>
        </div>
    </div>
</div>
<cc1:user id="User1" runat="server" />
