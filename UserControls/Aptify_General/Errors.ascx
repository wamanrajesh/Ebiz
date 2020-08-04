<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Errors.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.ErrorsPageControl" %>
<div class="content-container clearfix" id="Table1">
    <%-- Navin Prasad Issue 11032--%>
    <%--Nalini Issue 12436 date:01/12/2011--%>
    <asp:UpdatePanel ID="UppanelGrid" runat="server">
        <ContentTemplate>
            <asp:GridView ID="grdErrors" runat="server" Width="100%" Height="86px" BorderColor="#DEDFDE"
                BorderStyle="None" BorderWidth="1px" BackColor="White" CellPadding="4" GridLines="Vertical"
                ForeColor="Black" Font-Size="9pt" AllowPaging="True" PageSize="25" AutoGenerateColumns="False">
                <PagerStyle HorizontalAlign="Right" ForeColor="Black" BackColor="#F7F7DE" />
                <PagerSettings Position="TopAndBottom" />
                <Columns>
                    <asp:BoundField DataField="DateTime" HeaderText="Date" ReadOnly="True" />
                    <asp:BoundField DataField="Description" HeaderText="Description" ReadOnly="True" />
                    <asp:BoundField DataField="Source" HeaderText="Source" ReadOnly="True" />
                </Columns>
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="grdErrors" EventName="PageIndexChanging" />
        </Triggers>
    </asp:UpdatePanel>
    <%--<asp:DataGrid id="grdErrors" runat="server" Width="100%" Height="86px" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" BackColor="White" CellPadding="4" GridLines="Vertical" ForeColor="Black" Font-Size="9pt" AllowPaging="True" PageSize="25" AutoGenerateColumns="False">
 <PagerStyle HorizontalAlign="Right" ForeColor="Black" BackColor="#F7F7DE" Position="TopAndBottom"></PagerStyle>
 <Columns>
 <asp:BoundColumn DataField="DateTime" HeaderText="Date" ReadOnly="True"></asp:BoundColumn>
 <asp:BoundColumn DataField="Description" HeaderText="Description" ReadOnly="True"></asp:BoundColumn>
 <asp:BoundColumn DataField="Source" HeaderText="Source" ReadOnly="True"></asp:BoundColumn>
 </Columns>
 </asp:DataGrid>--%>
 </div>
