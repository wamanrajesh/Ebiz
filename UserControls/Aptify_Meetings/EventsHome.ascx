<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="EventsHome.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.EventsHome" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessShoppingCart" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<div class="Events" style="padding: 15px">
    <div class="EventHeading" style="font-size: 20px; color: #EA8140; margin-bottom:1em;">Events</div>
    
    <div class="EventsFilter" style="width:100%; margin-bottom:0.5em; vertical-align:middle; height:30px;">
        <span class="EventCategory" style="margin-right:1em;">
            Event Category:&nbsp;
            <asp:DropDownList runat="server" ID="cmbCategory" AutoPostBack="true" CssClass="cmbEventCategory">
            </asp:DropDownList>
        </span>
        <span class="EventType" style="margin-right:1em;">
            Event Type:&nbsp;
            <asp:DropDownList runat="server" ID="cmbStatus" AutoPostBack="true" CssClass="cmbEventType">
                <asp:ListItem Text="Current"></asp:ListItem>
                <asp:ListItem Selected="True" Text="Future"></asp:ListItem>
                <asp:ListItem Text="Past"></asp:ListItem>
                <asp:ListItem Text="All"></asp:ListItem>
            </asp:DropDownList>
        </span>
        <span class="MeetingsCalendar">
             <asp:HyperLink ID="MeetingsCalendarPage" runat="server" Text="Calendar View" ItemStyle-VerticalAlign="top"  />
        </span>
    </div>

    <div class="EventsGrid" style="width:100%; margin-top:0.5em;">
        <asp:DataGrid ID="grdMeetings" runat="server" AutoGenerateColumns="False" Width="100%"
            GridLines="Horizontal" AllowPaging="true" PagerStyle-Mode="NumericPages" PagerStyle-HorizontalAlign="Right"
            BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px">
            <AlternatingItemStyle BackColor="White" Height="28px" />
            <Columns>
                <asp:HyperLinkColumn DataTextField="WebName" DataNavigateUrlField="ProductID" HeaderText="Name"
                    ItemStyle-VerticalAlign="top" HeaderStyle-CssClass ="leftAlign">
                    <ItemStyle VerticalAlign="Top" CssClass="leftAlign"></ItemStyle>
                </asp:HyperLinkColumn>
                <asp:BoundColumn DataField="WebCategoryName" ItemStyle-VerticalAlign="top" HeaderText="Category">
                    <ItemStyle VerticalAlign="Top"></ItemStyle>
                </asp:BoundColumn>
                <asp:BoundColumn DataField="Description" ItemStyle-VerticalAlign="top" HeaderText="Description">
                    <ItemStyle VerticalAlign="Top"></ItemStyle>
                </asp:BoundColumn>
                <asp:BoundColumn DataField="Location" ItemStyle-VerticalAlign="top" HeaderText="Location">
                    <ItemStyle VerticalAlign="Top"></ItemStyle>
                </asp:BoundColumn>
                <asp:BoundColumn DataField="StartDate" ItemStyle-VerticalAlign="top" HeaderText="Start Date & Time">
                    <ItemStyle VerticalAlign="Top"></ItemStyle>
                </asp:BoundColumn>
                <asp:BoundColumn DataField="EndDate" ItemStyle-VerticalAlign="top" HeaderText="End Date & Time">
                    <ItemStyle VerticalAlign="Top"></ItemStyle>
                </asp:BoundColumn>
                <asp:BoundColumn DataField="Price" ItemStyle-VerticalAlign="top" HeaderText="Registration Price"
                    HeaderStyle-HorizontalAlign="Right"  HeaderStyle-CssClass="rightAlign" >
                    <ItemStyle VerticalAlign="Top" HorizontalAlign="Right"  CssClass="rightAlign" ></ItemStyle>
                </asp:BoundColumn>
            </Columns>
            <HeaderStyle CssClass="GridViewHeader" Height="28px" />
            <ItemStyle BackColor="#EAEAEA" Font-Size="12px" Height="28px" />
        </asp:DataGrid>
    </div>    
</div>

<cc1:User ID="User1" runat="server" />
<cc2:AptifyShoppingCart id="ShoppingCart1" runat="server" Visible="False"></cc2:AptifyShoppingCart>
