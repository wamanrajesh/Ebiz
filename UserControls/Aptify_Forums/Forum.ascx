<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Forum.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.Forums.ForumControl" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="uc1" TagName="Message" Src="Message.ascx" %>
<%@ Register TagPrefix="uc2" TagName="CreateMessage" Src="CreateMessage.ascx" %>
<%@ Register TagPrefix="uc3" TagName="ForumMessageGrid" Src="ForumMessageGrid.ascx" %>
<%@ Register TagPrefix="uc4" TagName="Forums" Src="Forums.ascx" %>
<%@ Register TagPrefix="uc5" TagName="ForumTitle" Src="ForumTitle.ascx" %>
<%@ Register Src="ForumTitle.ascx" TagName="ForumTitle" TagPrefix="uc3" %>
<div class="table-div">
    <div class="row-div">
        <div class="row-div">
            <uc3:ForumMessageGrid ID="ForumMessageGrid" runat="server" StyleMainTable="false" />
        </div>
        <div id="trMessage" runat="server" class="row-div">
            <uc1:Message ID="Message" runat="server" StyleMainTable="false" />
            <uc2:CreateMessage StyleMainTable="false" ID="CreateMessage" runat="server" Visible="False">
            </uc2:CreateMessage>
        </div>
    </div>
    <cc2:User ID="User1" runat="server" />
</div>
