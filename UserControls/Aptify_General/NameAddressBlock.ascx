<%--Aptify e-Business 5.5.1, July 2013--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="NameAddressBlock.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.NameAddressBlock" %>
<div>
	<asp:label id="lblName" runat="server"></asp:label><asp:Literal ID="brName" Runat="server" Text="<br>"></asp:Literal>
	<asp:label id="lblAddressLine1" runat="server">AddreesLine1</asp:label><br/>
	<asp:label id="lblAddressLine2" runat="server">AddressLine2</asp:label>
    <asp:literal id="brAddressLine2" runat="server" Text="<br>"></asp:literal>
    <asp:label id="lblAddressLine3" runat="server">AddressLine3</asp:label>
    <asp:literal id="brAddressLine3" runat="server" Text="<br>"></asp:literal>
    <asp:label id="lblCity" runat="server">City</asp:label>
    <asp:Label ID="lblCityComma" runat="server" Text=",">&nbsp;</asp:Label>
    <asp:label id="lblState" runat="server">State</asp:label>
	<asp:label id="lblZipCode" runat="server">ZipCode</asp:label><br/>
	<asp:label id="lblCountry" runat="server">Country</asp:label><br/>
</div>