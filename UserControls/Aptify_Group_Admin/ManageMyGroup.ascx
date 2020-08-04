<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ManageMyGroup.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.ManageMyGroup" %>
    <%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<div class="ColorDivBGAdmin">   
    <div class="row-div-bottom-line">
        <asp:image id="imgAddMember" runat="server" class="middle-img" imageurl="~/Images/Add_member.png" />
        <asp:hyperlink id="lnkAddMember" runat="server" text="Add Members"></asp:hyperlink>
    </div>
    <div class="row-div-bottom-line">
        <asp:image id="Image13" runat="server" class="middle-img" imageurl="~/Images/Purchase_membership.png" />
        <asp:hyperlink id="lnkPurchaseMembership" runat="server" text="Purchase Membership"></asp:hyperlink>
    </div>
    <div class="row-div-bottom-line">
        <asp:image id="Image14" runat="server" class="middle-img" imageurl="~/Images/Renew_membership.png" />
        <asp:hyperlink id="lnkRenewMembership" runat="server" text="Renew Membership" ></asp:hyperlink>
    </div>
    
    <div class="row-div-bottom-line">
        <asp:image id="Image12" runat="server" class="middle-img" imageurl="~/Images/Company_Profile.png" />
        <asp:hyperlink id="lnkCompanyProfile" runat="server" text="Company Info" ></asp:hyperlink>
    </div>
    <div class="row-div-bottom-line">
        <asp:image id="Image16" runat="server" class="middle-img" imageurl="~/Images/Company Directory.png" />
        <asp:hyperlink id="lnkCmpDirectory" runat="server" text="Company Directory" ></asp:hyperlink>
    </div>
    <div class="row-div-bottom-line">
        <asp:image id="Image15" runat="server" class="middle-img" imageurl="~/Images/Order_history.png" />
        <asp:hyperlink id="lnkOrderHistory" runat="server" text="Order History" ></asp:hyperlink>
    </div>
    <div class="row-div-bottom-line">
        <asp:image id="Image20" runat="server" class="middle-img" imageurl="~/Images/Pay off orders.png" />
        <asp:hyperlink id="lnkPayOffOrder" runat="server" text="Pay Off Orders " ></asp:hyperlink>
    </div>    
    <div class="row-div-bottom-line">
        <asp:image id="Image17" runat="server" class="middle-img" imageurl="~/Images/Event registration.png" />
        <asp:hyperlink id="lnkEventRegistration" runat="server" text="Event Registration"></asp:hyperlink>
    </div>
    <div class="row-div-bottom-line">
        <asp:image id="Image18" runat="server" class="middle-img" imageurl="~/Images/Meeting_Transfer.png" />
        <asp:hyperlink id="lnkMeetingAttendee" runat="server" text="Substitute Attendee "></asp:hyperlink>
    </div>
    <div class="row-div-bottom-line">
        <asp:image id="Image19" runat="server" class="middle-img" imageurl="~/Images/Meeting_Transfer1.png" />
        <asp:hyperlink id="lnkMeetingTransfer" runat="server" text="Meeting Transfer "></asp:hyperlink>
    </div>
  <cc2:user id="User1" runat="server" />
</div>
