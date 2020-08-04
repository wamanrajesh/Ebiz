<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="MeetingActionControl.ascx.vb" Inherits="Files_MeetingActionControl" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessShoppingCart" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>

<div runat="server" id="tblMain">
    <div>
        <img runat="server" id="imgGeneral" src="" alt="General Info" title="General Info"
            border="0" align="middle" />&nbsp;<asp:hyperlink runat="server" id="lnkGeneral" text="General"
                tooltip="View general information about the meeting" />
    </div>
    <div>
        <img runat="server" id="imgSchedule" src="" alt="Meeting Schedule" title="Meeting Schedule"
            border="0" align="middle" />&nbsp;<asp:hyperlink runat="server" id="lnkSchedule"
                text="Schedule" font-size="10pt" tooltip="View meeting schedule" />
    </div>
    <div>
        <img runat="server" id="imgSpeakers" src="" alt="Speakers" title="Speakers" border="0"
            align="middle" />&nbsp;<asp:hyperlink runat="server" id="lnkSpeakers" text="Speakers"
                font-size="10pt" tooltip="View a list of speakers for the meeting" />
    </div>
    <div>
        <img runat="server" id="imgTravel" src="" alt="Travel" title="Travel" border="0"
            align="middle" />&nbsp;<asp:hyperlink runat="server" id="lnkTravel" text="Travel"
                font-size="10pt" tooltip="View travel information for the meeting." /><br />
    </div>
    <div id="trForum" runat="server">
        <img runat="server" id="imgForum" src="" alt="Discussion Forum" title="Discussion Forum"
            border="0" align="middle" />&nbsp;<asp:hyperlink runat="server" id="lnkForum" text="Forum"
                font-size="10pt" tooltip="View discussion forum for the meeting" /><br />
    </div>
    <div id="trPeopleYouMayKnow" runat="server">
        <img runat="server" id="imgPeopleYouMayKnow" src="" alt="People You May Know" title="People You May Know"
            border="0" align="middle" />&nbsp;<asp:hyperlink runat="server" id="lnkPeopleYouMayKnow"
                text="People at Meeting" font-size="10pt" tooltip="View People You May Know for the meeting" /><br />
    </div>
    <div id="trRegister" runat="server">
        <asp:image id="imgRegister" runat="server" tooltip="Register" alternatetext="Register"
            borderwidth="0" imagealign="AbsMiddle" />
        &nbsp;<asp:linkbutton id="lnkRegister" runat="server" tooltip="Register Online for the meeting...">Register Now -
             <asp:Label runat="server" ID="lblPrice" /></asp:linkbutton>&nbsp;<br />
        <br />
        <asp:image id="imgRegisterGroup" runat="server" tooltip="Register" alternatetext="Register"
            borderwidth="0" imagealign="AbsMiddle" />
        &nbsp;<asp:hyperlink runat="server" id="HLGroupReg" text="Register Group"></asp:hyperlink>&nbsp;<br />
        <asp:label runat="server" id="lblFrimAdminLogin" text="Firm administrators should login first to register group." />
        <asp:linkbutton id="lnkLogin" runat="server" tooltip="login first to register group..."
            text="Login"></asp:linkbutton>
        <asp:label id="lblMemSavings" runat="server" font-bold="True" font-size="Smaller"
            forecolor="Green" visible="False"></asp:label>
        <br />
        <br />
        <asp:label runat="server" id="lblRegistrationResult" visible="false"></asp:label>
        <asp:label id="lblMeetingStatus" runat="server" visible="False" font-bold="True"
            forecolor="Black"></asp:label>
        <br />
        <br />
        <asp:linkbutton id="lnkNewMeeting" runat="server" visible="False" tooltip="Newer event available!">Click here for the next occurence of this event.</asp:linkbutton>
    </div>
</div>

            <cc1:User ID="User1" runat="server" />
<cc2:AptifyShoppingCart id="ShoppingCart1" runat="server" Width="47px" Height="14px" Visible="False"></cc2:AptifyShoppingCart>
