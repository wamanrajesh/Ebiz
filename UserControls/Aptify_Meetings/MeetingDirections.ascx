<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="MeetingDirections.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.MeetingDirections" %>
<%@ Register Src="GoogleMapsDirectionsAPI.ascx" TagName="GoogleMapsDirectionsAPI"
    TagPrefix="ucGMD" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
    <div id="Addresses" class="row-div clearfix">
        <div class="float-left w58 border-color">
        <div class="dir-title">
           Origin Address</div>
            <div class="row-div top-margin clearfix">
                <div class="label-div w19">
                    Address
                </div>
                <div class="field-div1 w79">
                    <asp:TextBox runat="server" ID="txtfromStreet" />
                </div>
            </div>
            <div class="row-div clearfix">
                <div class="label-div w19">
                    City, State, Zip
                </div>
                <div class="field-div1 w79">
                    <asp:TextBox runat="server" ID="txtfromCityStateZip" />
                    <asp:RequiredFieldValidator ID="rvCityStateZip" runat="server" ControlToValidate="txtfromCityStateZip"
                        ErrorMessage="Required" CssClass="error-msg-label" />
                </div>
            </div>
            <div class="row-div clearfix">
                <div class="label-div w19">
                    &nbsp;
                </div>
                <div class="field-div1 w79">
                    <asp:Button runat="server" ID="btnGetDirections" Text="Get Directions" CssClass="get-direction-button" />
                </div>
            </div>
        </div>
       <div class="float-left dir-arrow"></div>
        <div class="float-right w31 border-color">
            <div class="dir-title">
                Meeting Address</div>
            &nbsp;<asp:Label ID="lblMeetingAddress" runat="server" />
            <br />
            &nbsp;<asp:Label ID="lblMeetingCityStateZip" runat="server" />
        </div>
    </div>
    <div id="MapDirection" class="float-left w99 clearfix">
        <div class="div-googlemap" id="DirectionMap">
        </div>
            <ucgmd:googlemapsdirectionsapi id="GoogleMapsDirectionsAPI" mapelementid="DirectionMap"
        runat="server" />
    <cc1:user id="User1" runat="server"></cc1:user>
    </div>


