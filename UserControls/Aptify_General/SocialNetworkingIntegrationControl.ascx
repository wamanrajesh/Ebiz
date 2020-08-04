<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SocialNetworkingIntegrationControl.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.SocialNetworkingIntegrationControl" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessLogin" %>
<script src="http://static.ak.connect.facebook.com/js/api_lib/v0.4/FeatureLoader.js.php"
    type="text/javascript">
    FB.init("ae446c7f667124e901d2cb504c4232a8", "xd_receiver.htm");
</script>
<div id="tblSocialNetwork" runat="server">
    <div class="row-div">
        <a id="lnkLinkedIn" runat="server">
            <img alt="Click here to login to the site using your LinkedIn credentials." id="imgSocialNetwork"
                src="#" runat="server" />
        </a>
    </div>
    <div class="row-div">
        <asp:Label ID="lblError" runat="server" CssClass="error-msg-label" Visible="False">
        </asp:Label>
    </div>
</div>
<cc1:AptifyWebUserLogin ID="WebUserLogin1" runat="server" Visible="False" Height="94px"
    Width="235px"></cc1:AptifyWebUserLogin>
