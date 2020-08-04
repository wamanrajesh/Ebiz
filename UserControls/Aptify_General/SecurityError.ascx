<%--Aptify e-Business 5.5.1, July 2013--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SecurityError.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.SecurityError"  %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>

<div class="tabel-div clearfix">
  <asp:Label ID="lblMessage" Runat="server" Visible="False"></asp:Label>
</div>
<cc2:User runat="server" ID="User" />