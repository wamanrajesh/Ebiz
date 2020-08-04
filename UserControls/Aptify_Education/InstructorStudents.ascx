<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="InstructorStudents.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.Education.InstructorStudentsControl" %>
<%@ Register Src="InstructorValidator.ascx" TagName="InstructorValidator" TagPrefix="uc1" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<div id="tblMain" runat="server" class="table-div">
    <div class="row-div">
        <asp:DropDownList ID="cmbDate" runat="server" AutoPostBack="true">
            <asp:ListItem Selected="true" Value="*" Text="Current Students"></asp:ListItem>
            <asp:ListItem Value="-" Text="Prior Students"></asp:ListItem>
            <asp:ListItem Value="+" Text="Future Students"></asp:ListItem>
            <asp:ListItem Value="-6" Text="Last 6 Months"></asp:ListItem>
            <asp:ListItem Value="-12" Text="Last 12 Months"></asp:ListItem>
            <asp:ListItem Value="6" Text="Next 6 Months"></asp:ListItem>
            <asp:ListItem Value="12" Text="Next 12 Months"></asp:ListItem>
            <asp:ListItem Value="" Text="All Dates"></asp:ListItem>
        </asp:DropDownList>
    </div>
    <div class="row-div">
        <asp:UpdatePanel ID="updPanelGrid" runat="server">
            <ContentTemplate>
                <rad:radgrid id="grdStudents" runat="server" autogeneratecolumns="False" allowpaging="true"
                    allowsorting="true" allowfilteringbycolumn="true" sortingsettings-sorteddesctooltip="Sorted Descending"
                    sortingsettings-sortedasctooltip="Sorted Ascending">
                            <GroupingSettings CaseSensitive="false" />
                            <MasterTableView AllowFilteringByColumn="true" AllowSorting="true" AllowNaturalSort="false">
                                <Columns>
                                    <rad:GridTemplateColumn HeaderText="Course" DataField="Course" AutoPostBackOnFilter="true"
                                        SortExpression="Course" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="lnkCourse" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Course") %>'
                                                NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"ClassUrl") %>'></asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle />
                                    </rad:GridTemplateColumn>
                                    <rad:GridDateTimeColumn UniqueName="GridDateTimeColumnEndDate" DataField="StartDate"
                                        AllowSorting="true" Visible="True" HeaderText="Date" SortExpression="StartDate"
                                        ReadOnly="true" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false"
                                        DataType="System.DateTime" EnableTimeIndependentFiltering="true" >
                                        <ItemStyle />
                                    </rad:GridDateTimeColumn>
                                    <rad:GridTemplateColumn HeaderText="Last" DataField="LastName" AutoPostBackOnFilter="true"
                                        SortExpression="LastName" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLastName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"LastName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle />
                                    </rad:GridTemplateColumn>
                                    <rad:GridTemplateColumn HeaderText="First" DataField="FirstName" AutoPostBackOnFilter="true"
                                        SortExpression="FirstName" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFirstName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"FirstName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle />
                                    </rad:GridTemplateColumn>
                                    <rad:GridTemplateColumn HeaderText="Email" DataField="Email1" AutoPostBackOnFilter="true"
                                        SortExpression="Email1" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="lnkMail" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Email1") %>'
                                                NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"MailUrl") %>'></asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="10pt" />
                                    </rad:GridTemplateColumn>
                                    <rad:GridTemplateColumn HeaderText="Status" DataField="Status" AutoPostBackOnFilter="true"
                                        SortExpression="Status" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Status") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" />
                                    </rad:GridTemplateColumn>
                                    <rad:GridTemplateColumn HeaderText="Score" DataField="Score" AutoPostBackOnFilter="true"
                                        SortExpression="Score" CurrentFilterFunction="EqualTo" ShowFilterIcon="false"
                                        ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label ID="lblScore" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Score") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" />
                                    </rad:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </rad:radgrid>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>
<uc1:instructorvalidator id="InstructorValidator1" runat="server" />
