<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ChapterManagementControl.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.Chapters.ChapterManagementControl" %>
<div class="row-div" id="tblMain" runat="server">
    <div class="label">
        <asp:HyperLink ID="lnkChapters" runat="server">
					My Chapters
        </asp:HyperLink>
    </div>
        Shows the chapters that you are linked to and allows you to view and, if you have
        the required level of permissions, to edit the membership roster for the chapters.
<br />
<br />
</div>
<div class="row-div">
    <div class="label">
        <asp:HyperLink ID="lnkChapterSearch" runat="server">
					Find Chapters
        </asp:HyperLink>
    </div>
        Search for chapters by name, location, and other attributes
</div>
