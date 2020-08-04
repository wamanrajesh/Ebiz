<%--Aptify e-Business 5.5.1, July 2013--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="RequestInfoComplete.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.MarketPlace.RequestInfoComplete" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<cc3:User ID="User1" runat="server" />

<div id="tblRequestInfoResult" class="marketplace-main-div" runat="server">
    <div class="row-div label">
        Thank you for using International Demo Association MarketPlace!
    </div>
     <hr />
    <div class="row-div">
        <asp:Label ID="lblSuccess" runat="server" Text="Thank you for using the International Demo Association MarketPlace. Your request has been expedited for the listing(s) you have selected. You should be receiving feedback soon on your requests. If you do not, please visit our site again and review your requests."></asp:Label>
        <asp:Label ID="lblFailure" runat="server" Text="A failure occurred during the information request. Please try again later an if the prolem continues, contact our web technical support." CssClass="error-msg-label"></asp:Label>
    </div>
</div>
