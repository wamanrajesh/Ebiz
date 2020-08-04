<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="vb" AutoEventWireup="false" CodeFile="CreateMessage.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.Forums.CreateMessage" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessGlobal" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<div class="errormsg-div">
<asp:Label ID="lblError" runat="server" Visible="False"></asp:Label></div>
<div class="forum-createmsg-main-div">
<div class="row-div label">
    New Post
</div>
<div class="row-div label">
    Subject:
</div>
<div class="row-div">
    <asp:TextBox ID="txtSubject" runat="server">
    </asp:TextBox>
</div>
<div class="row-div label">
    Message:
</div>
<div class="row-div">
    <asp:TextBox ID="txtBody" MultiLine="True" Wrap="true" runat="server" TextMode="MultiLine"
        CssClass="txt-restrict-resize" />
</div>
<div class="row-div">
    <asp:Button runat="server" ID="cmdSave" Text="Save" CssClass="submit-Btn" />
    <asp:Button runat="server" ID="cmdCancel" Text="Cancel" CssClass="submit-Btn" />
</div>
<cc2:user runat="server" id="User1" />
</div>