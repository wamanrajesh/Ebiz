<%--Aptify e-Business 5.5.1, July 2013--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="FollowUs.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.FollowUs" %>
<div id="followusdiv" class="tabel-div middle-img" >
    <span>
        <%--Dilip issue 12717--%>
        <asp:Label ID="lblfollow" runat="server" Text="Follow us at"></asp:Label>
    </span>
    <span>
        <asp:HyperLink ID="lnkFaceBook" runat="server" Target="_blank">
            <asp:Image ID="ImgFaceBook" runat="server" />
        </asp:HyperLink>
    </span>
    <span>
        <asp:HyperLink ID="lnkTwitter" runat="server" Target="_blank">
            <asp:Image ID="ImgTwitter" runat="server" />
        </asp:HyperLink>
    </span>
    <span>
        <asp:HyperLink ID="lnkLinkedIn" runat="server" Target="_blank">
            <asp:Image ID="imgLinkedIn" runat="server" />
        </asp:HyperLink>
    </span>    
</div>
