<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="vb" AutoEventWireup="false" Inherits="Aptify.Framework.Web.eBusiness.BrowseControl"
    CodeFile="Browse.ascx.vb" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessGlobal" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<div class="label">
    <asp:Label ID="lblHeader" runat="server">
				Browse Directory
    </asp:Label></div>
<div class="label">
    <asp:HyperLink ID="hrefBrowseName" runat="server">
					 Browse By Name
    </asp:HyperLink>
</div>
<div class="label">
    <asp:HyperLink ID="hrefBrowseState" runat="server">
					 Browse By State
    </asp:HyperLink>
</div>
<div class="label" runat="server" id="rowCompanyType">
    <asp:HyperLink ID="hrefBrowseCompanyType" runat="server">
					 Browse By Company Type
    </asp:HyperLink>
</div>
<div class="label">
    <asp:HyperLink ID="hrefBrowseRegion" runat="server">
				 Browse By Region
    </asp:HyperLink>
</div>
<div class="label">
    <asp:HyperLink ID="hrefSearch" runat="server">
				 Search...
    </asp:HyperLink>
</div>
