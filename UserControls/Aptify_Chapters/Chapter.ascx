<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Chapter.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.Chapters.ChapterControl"
    Debug="true" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="uc1" TagName="ChapterMember" Src="ChapterMember.ascx" %>
<%@ Register TagPrefix="uc2" TagName="NameAddressBlock" Src="../Aptify_General/NameAddressBlock.ascx" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<div class="table-div clearfix" runat="server" id="tblTopLevel">
    <div class="row-div-bottom-line">
    <div class="control-title">
        <asp:Label ID="lblChapterName" runat="server">
        </asp:Label>
    </div>
    </div>
    <div class="top-margin clearfix">
        <div class="label-div-left-align">
            <asp:HyperLink ID="lnkReports" runat="server">
					Reports |
            </asp:HyperLink>
        </div>
        <div class="label-div-left-align">
            <asp:HyperLink ID="lnkLocation" runat="server">
					Edit Chapter |
            </asp:HyperLink>
        </div>
        <div class="label-div-left-align">
            <asp:HyperLink ID="lnkOfficers" runat="server">
					Officers |
            </asp:HyperLink></div>
        <div class="label-div-left-align">
            <asp:HyperLink ID="lnkMeetings" runat="server">
					Meetings
            </asp:HyperLink>
        </div>
    </div>
    <div class="row-div clearfix">
        <uc2:nameaddressblock id="blkAddress" runat="server">
            </uc2:nameaddressblock>
    </div>
    <div class="row-div clearfix">
    <div class="label-div">
        Members
    </div>
    <div class="field-div1 "> <asp:Button ID="cmdNew" runat="server" Text="Add Member(s)" CssClass="submit-Btn">
        </asp:Button></div></div>
    <div class="row-div">
       <rad:radgrid id="grdMembers" runat="server" autogeneratecolumns="false" CssClass="row-div"
            allowpaging="true" allowfilteringbycolumn="true" sortingsettings-sorteddesctooltip="Sorted Descending"
            sortingsettings-sortedasctooltip="Sorted Ascending">
                <GroupingSettings CaseSensitive="false" />
                <MasterTableView AllowFilteringByColumn="true" AllowSorting="true" AllowNaturalSort="false">
                    <Columns>
                        <rad:GridTemplateColumn HeaderText="ID" DataField="ID" SortExpression="ID" AutoPostBackOnFilter="true"
                            CurrentFilterFunction="EqualTo" ShowFilterIcon="false">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnSelect" runat="server" CommandName="Select" Text='
												<%# DataBinder.Eval(Container.DataItem,"ID") %>
													' />
                            </ItemTemplate>
                        </rad:GridTemplateColumn>
                        <rad:GridBoundColumn DataField="FirstName" HeaderText="First Name" SortExpression="FirstName"
                            FooterText="" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"/>
                        <rad:GridBoundColumn DataField="LastName" HeaderText="Last Name" SortExpression="LastName"
                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"/>
                        <rad:GridBoundColumn DataField="Title" HeaderText="Title" SortExpression="Title"
                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"/>
                        <rad:GridBoundColumn DataField="Email" HeaderText="Email" SortExpression="Email"
                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"/>
                        <rad:GridBoundColumn Visible="false" DataField="ID" SortExpression="ID" HeaderText="ID"
                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"/>
                    </Columns>
                </MasterTableView>
            </rad:radgrid>
    </div>
    <cc3:user id="User1" runat="server" />
</div>

