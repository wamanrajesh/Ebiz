<%--Aptify e-Business 5.5.1, July 2013--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="LoginAndMember.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.LoginAndMember" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register Src="~/UserControls/Aptify_General/BecomeMemberControl.ascx" TagName="BecomeMemberControl"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/Aptify_General/LoginSF4.ascx" TagName="LoginSF4"
    TagPrefix="uc2" %>

<div class="row-div" id="trLogin" runat="server">
    <uc2:LoginSF4 ID="LoginSF4" runat="server" />
</div>
<div class="row-div" id="trEvents" runat="server">
    <uc1:BecomeMemberControl ID="BecomeMemberControl" runat="server" />
</div>
<cc1:User ID="User1" runat="server" />

