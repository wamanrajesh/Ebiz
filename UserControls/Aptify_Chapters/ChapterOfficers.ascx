<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ChapterOfficers.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.Chapters.ChapterOfficersControl" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="uc1" TagName="ChapterMember" Src="ChapterMember.ascx" %>
<%@ Register TagPrefix="uc1" TagName="NameAddressBlock" Src="../Aptify_General/NameAddressBlock.ascx" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<div class="chapter-officers-div clearfix">
<div class="row-div-bottom-line">
    <div class="control-title">
        <asp:Label ID="lblChapterName" runat="server">
        </asp:Label>
    </div>
    </div>
    <div class="row-div dropdown">
        <asp:DropDownList ID="cmbType" runat="server" AutoPostBack="True">
            <asp:ListItem Value="Current" Selected="True">Current</asp:ListItem>
            <asp:ListItem Value="Past">Past</asp:ListItem>
            <asp:ListItem Value="All">All</asp:ListItem>
        </asp:DropDownList>
    </div>
    <div class="row-div">
        <asp:UpdatePanel ID="updPanelGrid" runat="server">
            <ContentTemplate>
                <rad:radgrid id="grdRoles" runat="server" autogeneratecolumns="false" allowpaging="true"
                    allowfilteringbycolumn="true" CssClass="row-div">
						<GroupingSettings CaseSensitive="false" />
						<MasterTableView AllowSorting="true" AllowFilteringByColumn="true">
							<Columns>
								<rad:GridBoundColumn DataField="ChapterRoleType" HeaderText="Role" SortExpression="ChapterRoleType" 
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"/>
								<rad:GridBoundColumn DataField="Person" HeaderText="Person" SortExpression="Person" AutoPostBackOnFilter="true" 
                                CurrentFilterFunction="Contains" ShowFilterIcon="false"/>
								<rad:GridDateTimeColumn UniqueName="GridDateTimeColumnStartDate" AllowSorting="true" Visible="True" 
                                HeaderText="Start Date" DataField="StartDate" SortExpression="StartDate" AutoPostBackOnFilter="true" 
                                CurrentFilterFunction="EqualTo" ShowFilterIcon="false" DataType="System.DateTime" EnableTimeIndependentFiltering="true" 
                                DataFormatString="{0:MMMM dd, yyyy}">
									<ItemStyle/>
								</rad:GridDateTimeColumn>
								<rad:GridDateTimeColumn UniqueName="GridDateTimeColumnEndDate" AllowSorting="true" Visible="True" HeaderText="End Date" 
                                DataField="EndDate" SortExpression="EndDate" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" 
                                ShowFilterIcon="false" EnableTimeIndependentFiltering="true" DataType="System.DateTime" 
                                DataFormatString="{0:MMMM dd, yyyy}">
									<ItemStyle/>
								</rad:GridDateTimeColumn>
							</Columns>
						</MasterTableView>
					</rad:radgrid>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="row-div">
        <asp:LinkButton ID="lnkChapter" runat="server">Go To Chapter</asp:LinkButton>
        <div class="errormsg-div clearfix">
            <asp:Label ID="lblError" runat="server">
            </asp:Label></div>
    </div>
    <cc3:user id="User1" runat="server" />
</div>
