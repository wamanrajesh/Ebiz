<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ForumsHome.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.Forums.ForumsHome" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="uc1" TagName="Forums" Src="Forums.ascx" %>
<div class="forum-main-div">
    <div id="tblInner" class="table-div">
        <div class="row-div">
            <uc1:Forums ID="Forums" runat="server"></uc1:Forums>
        </div>
        <div class="row-div">
            <img runat="server" id="imgSearch" src="" alt="Search Forums" />
            <asp:HyperLink runat="server" ID="lnkSearch">
                <asp:Label runat="server" ID="Label1" Text="Search Forums...">
                </asp:Label>
            </asp:HyperLink>
        </div>
        <div class="row-div">
            <img runat="server" id="imgSubscribe" src="" alt="Subscribe To Forums" />
            <asp:HyperLink runat="server" ID="lnkSubscribe">
                <asp:Label runat="server" ID="Label2" Text="Manage Email Subscriptions">
                </asp:Label>
            </asp:HyperLink>
        </div>
    </div>
<cc2:User ID="User1" runat="server" />
</div> 