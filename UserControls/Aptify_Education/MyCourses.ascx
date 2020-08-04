<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="MyCourses.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.Education.MyCoursesControl" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<div class="table-div">
<div class="row-div">
    <asp:DropDownList runat="server" ID="cmbType" AutoPostBack="True">
        <asp:ListItem>Current/Future Courses</asp:ListItem>
        <asp:ListItem>Past Courses</asp:ListItem>
        <asp:ListItem>All Courses</asp:ListItem>
    </asp:DropDownList>
    </div>
    <asp:UpdatePanel ID="UppanelGrid" runat="server">
        <ContentTemplate>
           <rad:RadGrid ID="grdMyCourses" runat="server" AutoGenerateColumns="False" AllowPaging="true"
                AllowFilteringByColumn="true" SortingSettings-SortedDescToolTip="Sorted Descending"
                SortingSettings-SortedAscToolTip="Sorted Ascending">
                <GroupingSettings CaseSensitive="false" />
                <MasterTableView AllowFilteringByColumn="true" AllowSorting="true" AllowNaturalSort="false">
                    <Columns>
                        <rad:GridTemplateColumn HeaderText="Course" DataField="WebName"  SortExpression="WebName" AutoPostBackOnFilter="true" 
                            CurrentFilterFunction="Contains" ShowFilterIcon="false" AllowFiltering="true">
                            <ItemTemplate>
                                <asp:HyperLink ID="lnkWebName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"WebName") %>'
                                    NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"ClassUrl") %>'></asp:HyperLink>
                            </ItemTemplate>
                            <ItemStyle />
                        </rad:GridTemplateColumn>
                        <rad:GridTemplateColumn HeaderText="Status"  DataField="Status" SortExpression="Status" AutoPostBackOnFilter="true"
                            CurrentFilterFunction="Contains" ShowFilterIcon="false" >
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Status") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle />
                        </rad:GridTemplateColumn>
                        <rad:GridDateTimeColumn DataField="DateRegistered" UniqueName="GridDateTimeColumnEndDate"
                        HeaderText="Date Registered"   SortExpression="DateRegistered" AutoPostBackOnFilter="true"
                        CurrentFilterFunction="EqualTo" DataType="System.DateTime" ShowFilterIcon="false" DataFormatString="{0:MMMM dd, yyyy }"
                        EnableTimeIndependentFiltering="true" />
                        <rad:GridTemplateColumn  DataField="WebDescription" HeaderText="Description" SortExpression=""  AutoPostBackOnFilter="true" 
                            CurrentFilterFunction="Contains" ShowFilterIcon="false"  >
                            <ItemTemplate>
                                <asp:Label ID="lblWebDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"WebDescription") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle  />
                        </rad:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </rad:RadGrid>
        </ContentTemplate>
    </asp:UpdatePanel>
    <cc1:User runat="server" ID="User1" />
</div>
