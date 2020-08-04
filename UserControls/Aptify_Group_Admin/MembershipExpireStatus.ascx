<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="MembershipExpireStatus.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.MembershipExpireStatus" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessLogin" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Charting" Assembly="Telerik.Web.UI" %>
<asp:UpdatePanel ID="updatepnl1" runat="server">
    <ContentTemplate>
        <div class="chart-container-div float-left w48">
            <div class="chart-title">
                <asp:Label ID="Label1" runat="server" Text="Membership Expiration Status" Font-Bold="true"></asp:Label>
            </div>
            <div class="chart-duration-div">
                <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="true">
                    <asp:ListItem Text="Coming 3 Months" Value="90">
                    </asp:ListItem>
                    <asp:ListItem Text="Coming 6 Months" Value="180">
                    </asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="chart-data-div float-left">
                <rad:RadChart ID="RadChart1" runat="server" AutoLayout="true" ChartTitle-Appearance-Border-PenStyle="Dash"
                    ChartTitle-Appearance-Border-Visible="false" ChartTitle-Appearance-CompositionType="RowImageText"
                    ChartTitle-Appearance-Dimensions-Width="50px" ChartTitle-Marker-ActiveRegion-Tooltip="Hello"
                    Skin="WebBlue" PlotArea-Appearance-Border-Width="0" ChartTitle-Appearance-FillStyle-FillType="Gradient"
                    Legend-Visible="false" Legend-ActiveRegion-Attributes="hello" ChartTitle-Marker-Appearance-FillStyle-MainColor="Gray"
                    Legend-ActiveRegion-Tooltip="hh" ChartTitle-Appearance-FillStyle-MainColor="Green"
                    PlotArea-Appearance-FillStyle-MainColor="#fafafa" PlotArea-Appearance-FillStyle-SecondColor="#fafafa"
                    PlotArea-EmptySeriesMessage-TextBlock-Text="No Results Found">
                    <ChartTitle Visible="false" Appearance-Dimensions-Width="10" TextBlock-Appearance-TextProperties-Font="bold"
                        Appearance-FillStyle-MainColor="#e0e0e0" Appearance-FillStyle-SecondColor="#e0e0e0">
                    </ChartTitle>
                    <Appearance Border-Visible="false" FillStyle-MainColor="white" FillStyle-SecondColor="white">
                    </Appearance>
                    <Series>
                    </Series>
                </rad:RadChart>
                <div class="padding-left-right legend-div">
                    <div id="div1" runat="server" class="chart-orange-mark">
                    </div>
                    <div class="float-left">
                        <asp:HyperLink ID="A1" runat="server" CssClass="LeftChartHyperLink"></asp:HyperLink>
                    </div>
                    <div class="clear">
                    </div>
                    <div id="div2" runat="server" class="chart-green-mark">
                    </div>
                    <div class="float-left">
                        <asp:HyperLink ID="A2" runat="server" CssClass="LeftChartHyperLink"></asp:HyperLink>
                    </div>
                    <div class="clear">
                    </div>
                </div>
                <div class="clear">
                </div>
            </div>
        </div>
        <div class="float-left w2">
            &nbsp;</div>
        <div class="chart-container-div float-left w48">
            <div class="chart-title">
                <asp:Label ID="Label2" runat="server" Text="Order Status Summary" Font-Bold="true"></asp:Label>
            </div>
            <div class="chart-duration-div">
                <asp:DropDownList ID="DropDownList2" runat="server" AutoPostBack="true">
                </asp:DropDownList>
            </div>
            <div class="chart-data-div float-left">
                <rad:RadChart ID="RadChart2" runat="server" AutoLayout="true" ChartTitle-Appearance-Border-PenStyle="Dash"
                    ChartTitle-Appearance-Border-Visible="false" ChartTitle-Appearance-CompositionType="RowImageText"
                    ChartTitle-Appearance-Dimensions-Width="50px" ChartTitle-Marker-ActiveRegion-Tooltip="Hello"
                    Skin="WebBlue" PlotArea-Appearance-Border-Width="0" ChartTitle-Appearance-FillStyle-FillType="Gradient"
                    Legend-Visible="false" Legend-ActiveRegion-Attributes="hello" ChartTitle-Marker-Appearance-FillStyle-MainColor="Gray"
                    Legend-ActiveRegion-Tooltip="hh" ChartTitle-Appearance-FillStyle-MainColor="Green"
                    PlotArea-Appearance-FillStyle-MainColor="#fafafa" PlotArea-Appearance-FillStyle-SecondColor="#fafafa"
                    PlotArea-EmptySeriesMessage-TextBlock-Text="No Results Found">
                    <ChartTitle Visible="false" Appearance-Dimensions-Width="10" TextBlock-Appearance-TextProperties-Font="bold"
                        Appearance-FillStyle-MainColor="#e0e0e0" Appearance-FillStyle-SecondColor="#e0e0e0">
                    </ChartTitle>
                    <Appearance Border-Visible="false" FillStyle-MainColor="white" FillStyle-SecondColor="white">
                    </Appearance>
                    <Series>
                    </Series>
                </rad:RadChart>
                <div class="padding-left-right legend-div">
                    <div id="div3" runat="server" class="chart-orange-mark">
                    </div>
                    <div class="float-left">
                        <asp:HyperLink ID="lnkParty" runat="server" CssClass="RightChartHyperLink"></asp:HyperLink>
                    </div>
                    <div class="clear">
                    </div>
                    <div id="div4" runat="server" class="chart-green-mark">
                    </div>
                    <div class="float-left">
                        <asp:HyperLink ID="lnkPaid" runat="server" CssClass="RightChartHyperLink"></asp:HyperLink>
                    </div>
                    <div class="clear">
                    </div>
                    <div id="div5" runat="server" class="chart-red-mark">
                    </div>
                    <div class="float-left">
                        <asp:HyperLink ID="lnkUnpaid" runat="server" CssClass="RightChartHyperLink"></asp:HyperLink>
                    </div>
                    <div class="clear">
                    </div>
                </div>
                <div class="clear">
                </div>
            </div>
        </div>
        <div class="clear">
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<cc1:User ID="user1" runat="server" />
