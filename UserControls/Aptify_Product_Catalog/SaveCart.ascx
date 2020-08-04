<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SaveCart.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.ProductCatalog.SaveCartControl" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessShoppingCart" %>
<div class="table-div" id="tblMain" runat="server">
    <div class="row-div" id="trInfo" runat="server">
        <asp:Label ID="lblInfo" runat="server">Enter a name and description below and the cart will be saved for your future convenience.</asp:Label>
    </div>
    <div class="row-div" id="trResult" runat="server">
        <asp:Label ID="lblResult" runat="server" Visible="False" Font-Bold="True"></asp:Label>
    </div>
    <div class="row-div clearfix" id="trName" runat="server">
        <div class="label-div w19">
            <asp:Label ID="lblName" runat="server"><b>Cart Name:</b></asp:Label>
        </div>
        <div class="field-div1 w80">
            <asp:TextBox ID="txtName" runat="server" TextMode="SingleLine"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Name Is Required"
                ControlToValidate="txtName" ForeColor="Red"></asp:RequiredFieldValidator>
        </div>
    </div>
    <div class="row-div clearfix" id="trDescription" runat="server">
        <div class="label-div w19">
            <asp:Label ID="lblDescription" runat="server"><b>Description:</b>
            </asp:Label>
        </div>
        <div class="field-div1 w80">
            <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine"></asp:TextBox>
        </div>
    </div>
    <div class="row-div clearfix" id="trSave" runat="server">
        <div class="label-div w19">
            &nbsp;</div>
        <div class="field-div1">
            <asp:Button ID="cmdSaveCurrent" runat="server" Text="Update Current Cart" CssClass="submit-Btn" Visible="False">
            </asp:Button>
            <asp:Button ID="cmdSave" runat="server" Text="Save As New Cart" CssClass="submit-Btn" Visible="False" />
            <asp:Button ID="cmdCancel" runat="server" Text="Cancel" CssClass="submit-Btn" CausesValidation="false" />
        </div>
    </div>
</div>
<cc1:AptifyShoppingCart runat="server" ID="ShoppingCart1" />
