<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="AddForum.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.Forums.AddForumControl" %>
<div id="tblMain" runat="server" class="data-form">
    <div class="rTableCell">
        <asp:Label ID="Label1" runat="server">
				Name:
        </asp:Label>
    </div>
    <div class="rTableCell">
        <asp:TextBox ID="txtName" AptifyDataField="Name" runat="server" Width="200px">
        </asp:TextBox>
    </div>
    <div class="rTableCell">
        <asp:Label ID="Label2" runat="server">
				Description:
        </asp:Label>
    </div>
    <div class="rTableCell">
        <asp:TextBox ID="txtDescription" AptifyDataField="Description" runat="server" TextMode="MultiLine"
            Width="300px" Height="150px">
        </asp:TextBox>
    </div>
    <div class="rTableCell">
        <asp:Button ID="cmdCreateForum" runat="server" Text="Create Forum" CssClass="submitBtn">
        </asp:Button>
        <asp:Label ID="lblError" runat="server" Visible="False">
        </asp:Label>
    </div>
</div>
