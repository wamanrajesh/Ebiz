<%--Aptify e-Business 5.5.1, July 2013--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="RequestInfo.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.MarketPlace.RequestInfo" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<cc3:User ID="User1" runat="server" />

    <div id="tblListingInfo" class="marketplace-main-div" runat="server">
        <div class="row-div">
            Enter any comments that you would like to send along with this request below:
        </div>
        <div class="row-div">
            <asp:TextBox ID="txtComments" runat="server"  TextMode="MultiLine" CssClass="txt-restrict-resize"></asp:TextBox>
        </div>
        <div class="row-div">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit Request" CssClass="submit-Btn">
            </asp:Button>
        </div>
    </div>
