<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ChapterSearch.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.Chapters.ChapterSearchControl" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="WebUserActivity" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="PageSecurity" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Panel ID="pnlserachchapter" runat="server" DefaultButton="cmdSearch">
    <div class="chaptermain-div">
        <div class="row-div clearfix">
            <div class="label-div w19">
                <asp:Label ID="Label1" runat="server" Text="Chapter Name Contains">
                </asp:Label>
            </div>
            <div class="field-div1 editchapter w79">
                <asp:TextBox ID="txtName" runat="server">
                </asp:TextBox>
            </div>
        </div>
        <div class="row-div clearfix">
            <div class="label-div w19 ">
                <asp:Label ID="Label2" runat="server" Text="Chapter Type">
                </asp:Label>
            </div>
            <div class="field-div1 editchapter w79">
                <asp:DropDownList ID="cmbCategory" DataValueField="ID" DataTextField="WebName" runat="server">
                </asp:DropDownList>
            </div>
        </div>
        <div class="row-div clearfix">
            <div class="label-div w19">
                &nbsp;
            </div>
            <div class="field-div1 w79">
                <asp:Button ID="cmdSearch" runat="server" Text="Find Chapters" CssClass="submit-Btn">
                </asp:Button>
            </div>
        </div>
        <div class="row-div1 clearfix">
            <div class="label-div w19">
                &nbsp;
            </div>
            <div class="errormsg-div clearfix w79">
                <asp:Label ID="lblError" runat="server" Visible="False">
                </asp:Label>
            </div>
        </div>
    </div>
    <div class="row-div" id="trResults" runat="server">
        <asp:UpdatePanel ID="updPanelGrid" runat="server">
            <ContentTemplate>
                <rad:radgrid id="grdResults" runat="server" autogeneratecolumns="False"
                    allowpaging="true" allowfilteringbycolumn="true" sortingsettings-sorteddesctooltip="Sorted Descending"
                    sortingsettings-sortedasctooltip="Sorted Ascending" CssClass="row-div">
                                 <GroupingSettings CaseSensitive="false" />
                                <MasterTableView AllowFilteringByColumn="true" AllowSorting="true" AllowNaturalSort="false">
                                    <Columns>
                                        <rad:GridTemplateColumn HeaderText="Chapter" DataField="Name" SortExpression="Name"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:HyperLink Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>' ID="lnkName"
                                                    runat="server" NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"DataNavigateUrl") %>' />
                                            </ItemTemplate>
                                        </rad:GridTemplateColumn>
                                        <rad:GridBoundColumn DataField="CompanyType" HeaderText="Chapter Type" SortExpression="CompanyType"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                                    </Columns>
                                </MasterTableView>
                            </rad:radgrid>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <cc2:pagesecurity id="PageSecurity1" runat="server" webmodule="Chapter Management" />
    <cc1:webuseractivity id="WebUserActivity1" runat="server" webmodule="Chapter Management" />
    <cc3:user id="User1" runat="server" />
</asp:Panel>
