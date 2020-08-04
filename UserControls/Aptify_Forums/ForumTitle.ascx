<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ForumTitle.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.Forums.ForumTitleControl" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="WebUserActivity" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="PageSecurity" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="uc1" TagName="Forums" Src="Forums.ascx" %>
<div class="top-margin row-div">
    <div class="left-container w40">
        <span class="label-div-left-align">
            <asp:Label ID="lblDiscussionForum" runat="server" Font-Bold="true" ForeColor="Black" Font-Size="Large"></asp:Label>
        </span>
        <br />
        <span class="gray-font">
            <asp:Label ID="parForumLabel" runat="server" Text="Parent Forum: " />
            <a runat="server" id="lnkParent">
                <asp:Label ID="lblParent" runat="Server"></asp:Label></a> </span>
    </div>
    <div class="row-div clearfix">
        <span class="label-div">
            <asp:HyperLink runat="server" ID="lnkSubscribe" title="Click here to manage all Aptify Forums subscriptions" Font-Size="Medium">Manage Subscriptions</asp:HyperLink></span>
    </div>
    <div class="row-div clearfix">
        <span class="gray-font">
            <asp:Label ID="lblDescription" runat="server"></asp:Label>
        </span>
    </div>
</div>
<cc2:User ID="User1" runat="server" />
