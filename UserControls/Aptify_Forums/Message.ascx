<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Message.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.Forums.Message" %>
<%@ Register Src="../Aptify_General/RecordAttachments.ascx" TagName="RecordAttachments"
    TagPrefix="uc1" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<div id="tblMain" runat="server" class="table-div">
    <div class="row-div clearfix">
        <div class="label-div w19">
            Subject:
        </div>
        <div class="field-div w80">
            <asp:Label ID="lblSubject" runat="server">
            </asp:Label>
        </div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w19">
            Sent:
        </div>
        <div class="field-div w80">
            <asp:Label ID="lblSent" runat="server">
            </asp:Label>
        </div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w19">
            From:
        </div>
        <div class="field-div w80">
            <asp:Label ID="lblFrom" runat="server">
            </asp:Label>
        </div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w19">
            Attachments:
        </div>
        <div class="field-div w80">
            <asp:Label ID="lblAttachments" runat="server">
            </asp:Label>
            <br />
            <asp:Button ID="btnAttachments" runat="server" CssClass="submit-Btn" Text="View/Add"
                Visible="false" />
        </div>
    </div>
    <div class="row-div">
        <asp:Label ID="lblBody" runat="server">
        </asp:Label>
        <asp:Label ID="lblError" runat="server" Visible="False" CssClass="error-msg-label">
        </asp:Label>
    </div>
    <div runat="server" id="trAttachments" visible="false">
        <uc1:recordattachments id="RecordAttachments" runat="server" allowview="true" />
    </div>
    <cc2:user runat="server" id="User1" />
</div>
