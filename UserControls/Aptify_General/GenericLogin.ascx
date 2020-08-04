<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="GenericLogin.ascx.vb"
    Inherits="UserControls_Aptify_General_GenericLogin" %>
<%@ Register Src="~/UserControls/Aptify_General/LoginSF4.ascx" TagName="LoginSF4"
    TagPrefix="uc2" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<div class="row-div clearfix">
    <asp:Label ID="lblstat" runat="server" Text="Currently You are not logged in! Please Login Now."></asp:Label>
</div>
<div class="generic-login-div" style="margin:auto">
    
        <uc2:LoginSF4 ID="LoginSF4" runat="server" />
    
</div>
<cc2:User runat="Server" ID="User1" />