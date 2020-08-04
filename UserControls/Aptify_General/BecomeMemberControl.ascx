<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="BecomeMemberControl.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.BecomeMemberControl" %>
<div class="row-div clearfix">
    <div class="float-left w18">
        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Globe.png" />
    </div>
    <div class="float-right w80 align-center">
        <div class="member-msg-font">
            <asp:Label ID="lblMemberMessage" runat="server" Text="Become an ICE Member"></asp:Label>
        </div>
        <div class="row-div">
            <asp:Button ID="btnJoin" runat="server" Text="Join Us Now!" CssClass="submit-Btn" /></div>
    </div>
</div>