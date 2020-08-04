<%--Aptify e-Business 5.5.1, July 2013--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="DatePicker.ascx.vb" Inherits="DatePicker" %>
<link href="../include/CalendarStyle.css" rel="stylesheet" type="text/css" />
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<div class="rTableCell" id="Td1" runat="server">
    <rad:RadDatePicker ID="dtpPicker" runat="server" MinDate="1900-01-01" Width="100px">
    </rad:RadDatePicker>
</div>

<asp:Literal runat="server" ID="ControlMappingScriptSpot">
</asp:Literal>