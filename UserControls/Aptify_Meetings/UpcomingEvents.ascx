<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="UpcomingEvents.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.UpcomingEvents" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
    <asp:Repeater ID="repEvents" runat="server">
        <HeaderTemplate>
            <div class="upcoming-header-title">
                <asp:Image runat="server" ID="Image1" ImageUrl="~/Images/event-icon.png" CssClass="middle-img"/>
                <asp:Label runat="server" ID="Label2" Text="Upcoming Events"/>
            </div>
        </HeaderTemplate>
        <ItemTemplate>
            <div class="row-div-bottom-line">
                <div id="trEventImage" runat="server">
                    <asp:Image ID="EventImage" runat="server" />
                </div>
                <div class="upcoming-meeting-title">
                    <asp:HyperLink ID="lnkEventName" commandname="EventName" commandargument='<% #Eval("ProductID") %>'
                        runat="server"></asp:HyperLink>
                </div>
                <div class="upcoming-meeting-dateplace">
                    <asp:Label ID="lblDate" runat="server"></asp:Label>
                </div>
                <div class="upcoming-meeting-dateplace">
                    <asp:Label ID="lblPlace" runat="server"></asp:Label>
                </div>
                <div id="trEventdesc" runat="server" class="upcoming-meeting-description">
                    <asp:Literal ID="ltrdescription" runat="server"></asp:Literal>
                </div>                
            </div>
        </ItemTemplate>
        <FooterTemplate>
            <asp:HyperLink ID="linkViewAll" CommandArgument="ViewAllLink" runat="server" class="upcoming-viewall-link float-right">
                View All </asp:HyperLink>
            <div class="clear"></div>
        </FooterTemplate>
    </asp:Repeater>
    
    <cc2:user runat="server" id="User1" />

