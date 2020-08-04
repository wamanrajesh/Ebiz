<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="CEUSubmission.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.Education.CEUSubmissionControl" %>
<%@ Register Src="../Aptify_General/DatePicker.ascx" TagName="DatePicker" TagPrefix="uc2" %>
<%@ Register Src="../Aptify_General/RecordAttachments.ascx" TagName="RecordAttachments"
    TagPrefix="uc1" %>
<%@ Register Assembly="AptifyEBusinessUser" Namespace="Aptify.Framework.Web.eBusiness"
    TagPrefix="cc1" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<script type="text/javascript" language="javascript">
    function disp_alert() {
        alert("I am an alert box!!");
    }
    function inptSubmit_onclick() {

    }
    <%--Amruta IssueID 14903 method to hide label --%>
    function HideLabel(sender, args) {
        var input = args.get_fileName();  
        var n = input.indexOf(".");
        var fileExtension = input.substr(n + 1);
        var extensionArrayList = new Array("png", "jpg", "txt", "doc", "docx", "pdf", "bmp","gif");
        for (var i = 0; i < extensionArrayList.length; i++) {
        if(fileExtension == extensionArrayList[i]) {        
                if (document.getElementById('<%=lblErrorFile.ClientID%>'))
                    document.getElementById('<%=lblErrorFile.ClientID%>').style.display = 'none';
                return false;
            }
        }    
    }
</script>
<%--Amruta IssueID 14903 Page alignment and upload contol--%>
<div class="border-color ceu-submission-main-div">
    <div id="tblMain" runat="server" class="table-div">
        <div class="header-title-bg">
            Submit New CEU Record
        </div>
        <div class="padding-all">
            <div class="row-div clearfix">
                <div class="label-div w19">
                    Type:
                </div>
                <div class="field-div w80">
                    <asp:Label ID="lbltype" runat="server" Text="CEU Submitted Via Member Portal" >
                    </asp:Label>
                </div>
            </div>
            <div class="row-div clearfix">
                <div class="label-div w19">
                    Member:
                </div>
                <div class="field-div1 w80">
                    <asp:TextBox ID="txtMember" runat="server" Enabled="False" >
                    </asp:TextBox>
                    <span class="Error">
                        <asp:Label CssClass="error-msg-label" ID="lblErrorMember" runat="server">
					Error
                        </asp:Label>
                    </span>
                </div>
            </div>
            <div class="row-div clearfix">
                <div class="label-div w19">
                    Date Started:
                </div>
                <div class="field-div w80">
                    <telerik:raddatepicker id="dtpDateStarted" runat="server">
                </telerik:raddatepicker>
                </div>
            </div>
            <div class="row-div clearfix">
                <div class="label-div w19">
                    Date Granted:
                </div>
                <div class="field-div1 w80">
                    <telerik:raddatepicker id="dtpDateGranted" runat="server">
                </telerik:raddatepicker>
                </div>
            </div>
            <div class="row-div clearfix">
                <div class="label-div w19">
                    Units Earned:
                </div>
                <div class="field-div1 w80">
                    <asp:TextBox ID="txtUnitsEarned" runat="server" >
                    </asp:TextBox>
                    <span class="Error">
                        <asp:Label CssClass="error-msg-label" ID="lblErrorUnitsEarned" runat="server" >
					Error
                        </asp:Label>
                    </span>
                </div>
            </div>
            <div class="row-div clearfix">
                <div class="label-div w19">
                    Title:
                </div>
                <div class="field-div1 w80">
                    <asp:TextBox ID="txtTitle" runat="server">
                    </asp:TextBox>
                    <span class="Error">
                        <asp:Label CssClass="error-msg-label" ID="lblErrorTitle" runat="server">
					Error
                        </asp:Label>
                    </span>
                </div>
            </div>
            <div class="row-div clearfix">
                <div class="label-div w19">
                    CEU Type:
                </div>
                <div class="field-div1 w80">
                    <asp:DropDownList ID="drpTitle" runat="server" DataTextField="Name" DataValueField="ID">
                    </asp:DropDownList>
                    <span class="Error">
                        <asp:Label ID="lblErrorCEUType" runat="server">
					Error
                        </asp:Label>
                    </span>
                </div>
            </div>
            <div class="row-div clearfix">
                <div class="label-div w19">
                    Status:
                </div>
                <div class="field-div1 w80">
                    <asp:Label ID="lblStatus" runat="server" Text="Declared">
                    </asp:Label>
                    <span class="Error">
                        <asp:Label CssClass="error-msg-label" ID="lblErrorStatus" runat="server">
					Error
                        </asp:Label>
                    </span>
                </div>
            </div>
            <div class="row-div clearfix">
                <div class="label-div w19">
                    Expiration Date:
                </div>
                <div class="field-div1 w80">
                    <telerik:raddatepicker id="dtpExpirationDate" runat="server">
                </telerik:raddatepicker>
                </div>
            </div>
            <%--Amruta Issue 14903,20/03/2013,Message for valid upload file type--%>
            <div class="row-div clearfix">
            <div class="label-div w19">
                        &nbsp;
                    </div>
                    <div class="field-div1 w80">
                Optional: Provide a document from the education content provider that shows proof
                of completion of this CEU. Your document format :TXT,JPG,DOC,DOCX,PNG,GIF,BMP,PDF.
                </div>
            </div>
            <div class="row-div clearfix">
                <div class="label-div w19">
                    Document:
                </div>
                <div class="field-div1 w80 clearfix">
                    <div class="float-left">
                    <telerik:radasyncupload id="radCEUDocumentUpload" localization-select="Browse..."
                        runat="server" localization-remove="Remove" maxfileinputscount="1"
                        onclientfileselected="HideLabel">
                    </telerik:radasyncupload>
                     <div class="float-left">
                    <asp:Image ID="tooptip" runat="server" ImageUrl="~/Images/Help.png" ToolTip="If you want to replace the file you uploaded, remove the existing file and then specify a new file." />
                    </div>
                    </div>
               
                    <div class="float-left error-div error-msg-label">                    
                        <asp:Label ID="lblErrorFile" runat="server">
						Error
                        </asp:Label>
                    </div>
                </div>
            </div>
            <div class="row-div clearfix">
                <div class="row-div">
                    <asp:Label ID="lblErrorSubmit" CssClass="error-msg-label" runat="server" Visible="false">
						Error
                    </asp:Label>
                    <asp:Label ID="lblSubmitSuccess" runat="server" Visible="False">
					Success
                    </asp:Label>
                </div>
                <div class="row-div clearfix">
                    <div class="label-div w19">
                        &nbsp;
                    </div>
                    <div class="field-div1 w80">
                        <asp:LinkButton ID="lnkGoBack" runat="server">
					Return to My Certifications
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
            <div class="row-div clearfix">
                <div class="label-div w19">
                    &nbsp;
                </div>
                <div class="field-div1 w80">
                    <asp:Button runat="server" ID="inptSubmit" name="tstButton" Text="Submit" CssClass="submit-Btn" />
                </div>
            </div>
        </div>
    </div>
    <cc1:user id="User1" runat="server" />
</div>
