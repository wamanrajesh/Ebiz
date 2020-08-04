<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="InstructorAuthorizedCourses.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.Education.InstructorAuthorizedCoursesControl" %>
<%@ Register Src="InstructorValidator.ascx" TagName="InstructorValidator" TagPrefix="uc1" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<div class="row-div errormsg-div">
    <asp:Label runat="server" ID="lblError" Visible="false"></asp:Label>
    </div>
  <div id="tblmain" runat="server" class="table-div">
	<div class="row-div-bottom-line label">
		<asp:Label runat="server"  ID="lblName" Text="List of Authorized Courses" />
	</div>	
      <div class="row-div">
                   <asp:UpdatePanel ID="updPanelGrid" runat="server">
                    <ContentTemplate>
                      <rad:RadGrid ID="grdCourses" runat="server" AutoGenerateColumns="False" AllowPaging="true"
                            AllowFilteringByColumn="true" SortingSettings-SortedDescToolTip="Sorted Descending"
                            SortingSettings-SortedAscToolTip="Sorted Ascending">
                            <GroupingSettings CaseSensitive="false" />
                            <MasterTableView AllowFilteringByColumn="true" AllowSorting="true" AllowNaturalSort="false">
                                <Columns>
                                    <rad:GridHyperLinkColumn DataTextField="WebName" DataNavigateUrlFields="ID" HeaderText="Course"
                                        DataNavigateUrlFormatString="~/Education/{0}" SortExpression="WebName" AutoPostBackOnFilter="true"
                                        CurrentFilterFunction="Contains" ShowFilterIcon="false"  />
                                    <rad:GridTemplateColumn HeaderText="Instructor Status" DataField="Status" SortExpression="Status"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblstatus" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Status") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle  />
                                    </rad:GridTemplateColumn>
                                    <rad:GridTemplateColumn HeaderText="Description" DataField="WebDescription" SortExpression=""
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblWebDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"WebDescription") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle />
                                    </rad:GridTemplateColumn>
                                    <rad:GridTemplateColumn HeaderText="# Units"  DataField="Units" SortExpression="Units" HeaderStyle-HorizontalAlign="right"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"
                                        ItemStyle-HorizontalAlign="Right" >
                                        
                                        <ItemTemplate>
                                            <asp:Label ID="lblUnits" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Units") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle />
                                    </rad:GridTemplateColumn>
                                    <rad:GridBoundColumn DataField="TotalPartDuration" HeaderText="Duration (min)" DataFormatString="{0:F0}" HeaderStyle-HorizontalAlign="right"
                                         SortExpression="TotalPartDuration" AutoPostBackOnFilter="true"
                                        CurrentFilterFunction="EqualTo" ShowFilterIcon="false" ItemStyle-HorizontalAlign="Right" />
                                </Columns>
                            </MasterTableView>
                        </rad:RadGrid>
                    </ContentTemplate>
                </asp:UpdatePanel>
           </div>
    </div>
    <uc1:InstructorValidator ID="InstructorValidator1" runat="server" />

