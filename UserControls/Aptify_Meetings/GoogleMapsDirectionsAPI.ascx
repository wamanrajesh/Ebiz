<%--Aptify e-Business 5.5.1, July 2013--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="GoogleMapsDirectionsAPI.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.GoogleMapsDirectionsAPI" %>

<div id="GoogleMapDirections" runat="server"></div>

<script type="text/javascript">
//<![CDATA[

    function _nco_gdir() {
        this.fromAddress = "<%=FromAddress%>";
        this.meetingAddress = "<%=MeetingAddress%>";
        this.mapElementID = "<%=MapElementId%>";
        this.autoLoad = "<%=AutoLoad.ToString().ToLower() %>";
        this.directions = null;
        var self = this;
        google.load("maps", "2"); //load version 2 of GoogleMaps API

        this.handleErrors = function () {

            if (self.divMap) self.divMap.innerHTML = "";
            if (self.directions && self.directions.getStatus().code == G_GEO_UNKNOWN_ADDRESS) {
                alert("Driving directions are not available for the address you entered.\nThis may be because the from address is relatively new, or it may be incorrect.");
            }
            else {
                alert("We are currently unable to display directions for the specified addresses.");
            }
        };

        this.loadDirections = function () {
            if (!self.directions) {
                if (self.mapElementID.length > 0) {
                    self.divMap = document.getElementById(self.mapElementID);

                    if (self.divMap != null) {
                        self.dirMap = new google.maps.Map2(self.divMap);
                        self.dirMap.addControl(new google.maps.LargeMapControl());
                        self.dirMap.addControl(new google.maps.ScaleControl());
                    }
                    else
                        self.dirMap = null;
                }

                self.dirpanel = document.getElementById("<%=GoogleMapDirections.ClientID%>");
                self.directions = new google.maps.Directions(self.dirMap, self.dirpanel);
                google.maps.Event.addListener(self.directions, "error", self.handleErrors);
            }

            //Clear Directions, if any
            self.dirpanel.innerHTML = "";

            //if(this.divMap!=null)
            // this.divMap.innerHTML = "Loading Map... Please wait";

            var ft = "from: " + self.fromAddress + " to: " + self.meetingAddress;

            self.directions.load(ft);

        };

        if (this.fromAddress.length > 0 && this.meetingAddress.length > 0 && this.autoLoad)
            google.setOnLoadCallback(this.loadDirections); //The google way to register to window.onload
    }

    _nco_dd = new _nco_gdir();
            
//]]>
</script>