<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="DisplayMessage.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.Forums.DisplayMessageControl" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagName="message" TagPrefix="uc1" Src="Message.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Forums" Src="Forums.ascx" %>
<%@ Register Src="ForumTitle.ascx" TagName="ForumTitle" TagPrefix="uc2" %>
<div runat="server" id="tblMain" class="rTable">
    <div class="rTableCell">
        <asp:Label ID="lblDiscussionForum" runat="server" />
    </div>
    <div class="rTableCell">
        <asp:HyperLink runat="server" ID="lnkForum" />
        <br />
        <asp:Label ID="lblDescription" runat="server">
        </asp:Label>
        <uc1:message ID="Message" runat="server"></uc1:message>
    </div>
</div>
<cc2:User runat="server" ID="User1" />
