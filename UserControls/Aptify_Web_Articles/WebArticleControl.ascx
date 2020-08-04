<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="WebArticleControl.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.WebArticleControl"  %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="WebUserActivity" %>

<div class="table-div">
 <div class="row-div control-title">
    <asp:Label id="lblWebArticleName" runat="server">Article Name</asp:Label>
    </div>
    <div id="author" runat="server">
    <asp:Label id="lblAuthor" runat="server">lblAuthor</asp:Label>
    </div>
    <div class="row-div">
    <asp:Label id="lblDateWritten" runat="server">lblDateWritten</asp:Label>
    </div>
    <div class="row-div">
    <p<asp:Label id="lblWebArticle" runat="server">lblWebArticle</asp:Label></p></div>
    <cc1:WebUserActivity WebModule="General" id="WebUserActivity1" runat="server"></cc1:WebUserActivity>
</div>
