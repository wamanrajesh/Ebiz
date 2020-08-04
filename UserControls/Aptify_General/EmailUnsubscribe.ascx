<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="EmailUnsubscribe.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.Unsubscribe" %>
 <div class="emailunsubscribe-main-div">
    
        <div class="row-div clearfix" id="tblRequest" runat="server">
            <asp:Label ID="lblinfo" runat="server" Text=""></asp:Label>
        
    <div class="row-div clearfix">
        <div class="label-div w19">
           <span class="required-label">*</span> Email:
        </div>
        <div class="field-div1 w79">
            <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="validateEmail" runat="server" ControlToValidate="txtEmail"
                Display="Dynamic" CssClass="error-msg-label" ErrorMessage="Email is required"></asp:RequiredFieldValidator>
            <asp:Label ID="lblInvalidEmail" runat="server" CssClass="error-msg-label"></asp:Label>
        </div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w19">
            Comments:
        </div>
        <div class="field-div1 w79">
            <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" class="txt-restrict-resize"></asp:TextBox>
        </div>
    </div>
    <div class="row-div clearfix">
    <div class="label-div w19">
            &nbsp;
        </div>
        <div class="w79">
            <asp:Button ID="btnSubmit" runat="server" CssClass="submit-Btn" Text="Submit" />
            <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="submit-Btn" />
        </div>
    </div>
     
     </div>
    <div class="row-div" id="tblresponse" runat="server">
        <div id="trResponseHeader" class="row-div" runat="server">
            You will receive an email shortly with a link that must be clicked in order to complete
            the unsubscribe process.
        </div>
        
        <div class="row-div top-margin clearfix">
            <div class="label-div w19">
                <asp:Label ID="lblEmail" runat="server" Text="Email:"></asp:Label>
            </div>
            <div class="field-div1 w79">
                <asp:Label ID="lblResEmail" runat="server"></asp:Label>
            </div>
        </div>
        <div class="row-div top-margin clearfix">
            <div class="label-div w19">
                <asp:Label ID="lblComments" runat="server" Text="Comments:"></asp:Label>
            </div>
            <div class="field-div1 w79">
                <asp:Label ID="lblResComments" runat="server"></asp:Label>
            </div>
        </div>
    </div>
    </div>
