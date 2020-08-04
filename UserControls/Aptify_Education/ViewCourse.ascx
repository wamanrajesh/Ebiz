<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ViewCourse.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.Education.ViewCourseControl" %>
<%@ Register Src="ClassScheduleControl.ascx" TagName="ClassScheduleControl" TagPrefix="uc1" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:UpdatePanel ID="UppanelGrid" runat="server">
    <contenttemplate>
         <asp:Label runat="server" ID="lblError" Visible="false">Security Access Error</asp:Label>  
<div  class="table-div" runat="server" id="tblMain">
    <div class="row-div"  >
		    <asp:HyperLink runat="server" ID="lnkCategory" Text="" CssClass="label" ToolTip="View other courses in this category" />
	    	<asp:Label runat="server" ID="lblName" CssClass="label" />
    </div>
	<div class="row-div clear header-title-bottom-border">
     <asp:Label runat="server" ID="lblDescription" />
    <br />
	<asp:Label runat="server" ID="lblPrice" />
    </div>
    <div class="table-div top-margin clearfix">
		<div class="float-left w18 clearfix" >
				<div class="row-div ">
						<img runat="server" id="imgGenInfoSmall" class="middle-img" src="" alt="General Info" />
						<asp:HyperLink runat="server" ID="lnkGeneral" Text="General" ToolTip="View general information about the course" />
				</div>
				<div class="row-div ">
						<img runat="server" id="imgScheduleSmall" class="middle-img" src="" alt="Class Schedule" />
						<asp:HyperLink runat="server" ID="lnkSchedule" Text="Class Schedule" ToolTip="View upcoming classes offered" />
				</div>
				<div class="row-div ">
						<img runat="server" id="imgSyllabusSmall" src="" alt="Syllabus" />
						<asp:HyperLink runat="server" ID="lnkSyllabus" Text="Syllabus" ToolTip="View a standard syllabus for the course" />
				</div>
				<div class="row-div ">
						<img runat="server" id="imgPrereqSmall" class="middle-img" src="" alt="Prerequisites" />
						<asp:HyperLink runat="server" ID="lnkPrerequisites" Text="Prerequisites" ToolTip="View a list of prerequisite courses that are required to be completed before registering for this course." />
						
				</div>
				<div class="row-div " id="trInstructors" runat="server">
						<img runat="server" id="imgInstructorSmall" class="middle-img" src="" alt="Instructors" />
						<asp:HyperLink runat="server" ID="lnkInstructors" Text="Instructors" ToolTip="View a list of instructors who actively teach this course" />
				</div>
				<div class="row-div ">
						<img runat="server" id="imgLocationsSmall" class="middle-img" src="" alt="Locations" />
						<asp:HyperLink runat="server" ID="lnkLocations" Text="Locations" ToolTip="View a list of locations where this course is taught" />
					
				</div>
            </div>
		<div id="tdExtContent" runat="server"  class="float-right w79 left-border">
			<img runat="server" id="imgTitle" class="middle-img" src="" alt="Course Information"  />
			<asp:Label runat="server" CssClass="label" ID="lblTitle" />
			<br />
			<asp:Label runat="server" ID="lblDetails" />
			<br />
					<rad:RadGrid ID="grdSyllabus" runat="server" AutoGenerateColumns="False" AllowPaging="true" AllowFilteringByColumn="true" AllowSorting="true" SortingSettings-SortedDescToolTip="Sorted Descending" SortingSettings-SortedAscToolTip="Sorted Ascending">
						<GroupingSettings CaseSensitive="false" />
						<MasterTableView AllowFilteringByColumn="true" AllowSorting="true" AllowNaturalSort="false">
							<Columns>
								<rad:GridTemplateColumn HeaderText="Item" DataField="WebName" SortExpression="WebName" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" >
									<ItemTemplate>
										<asp:Label ID="lnkWebName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"WebName") %>'>
											</asp:Label>
										</ItemTemplate>
										<ItemStyle />
									</rad:GridTemplateColumn>
									<rad:GridTemplateColumn HeaderText="Description" DataField="WebDescription" SortExpression="" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" >
										<ItemTemplate>
											<asp:Label ID="lblWebDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"WebDescription") %>'>
												</asp:Label>
											</ItemTemplate>
											<ItemStyle VerticalAlign="Top" />
										</rad:GridTemplateColumn>
										<rad:GridTemplateColumn HeaderText="Type" DataField="Type" SortExpression="Type" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" >
											<ItemTemplate>
												<asp:Label ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Type") %>'>													</asp:Label>
												</ItemTemplate>
												<ItemStyle VerticalAlign="Top" />
											</rad:GridTemplateColumn>
												<rad:GridBoundColumn DataField="Duration" DataFormatString="{0:F0} min" HeaderText="Duration" AllowSorting="false" AllowFiltering="false" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
											</Columns>
										</MasterTableView>
									</rad:RadGrid>
									<rad:RadGrid ID="grdPrerequisites" runat="server" AutoGenerateColumns="False" AllowPaging="true" AllowFilteringByColumn="true" AllowSorting="true" SortingSettings-SortedDescToolTip="Sorted Descending" SortingSettings-SortedAscToolTip="Sorted Ascending">
										<GroupingSettings CaseSensitive="false" />
										<MasterTableView AllowFilteringByColumn="true" AllowSorting="true" AllowNaturalSort="false">
											<Columns>
												<rad:GridTemplateColumn HeaderText="Prerequisite" DataField="WebName" SortExpression="WebName" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" >
													<HeaderStyle CssClass="radViewCoursesPrerequisitesColumn" />
													<ItemStyle CssClass="radViewCoursesPrerequisitesColumn" />
													<ItemTemplate>
														<asp:HyperLink ID="lnkWebName" runat="server" Text='
														<%# DataBinder.Eval(Container.DataItem,"WebName") %>
															' NavigateUrl='
															<%# DataBinder.Eval(Container.DataItem,"IDUrl") %>
																'>
																</asp:HyperLink>
															</ItemTemplate>
														</rad:GridTemplateColumn>
														<rad:GridTemplateColumn HeaderText="Description" DataField="WebDescription" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="" ShowFilterIcon="false" >
															<HeaderStyle CssClass="radViewCoursesPrerequisitesDescColumn" />
															<ItemStyle CssClass="radViewCoursesPrerequisitesDescColumn" />
															<ItemTemplate>
																<asp:Literal ID="ltDescription" runat="server" Text='
																<%# DataBinder.Eval(Container.DataItem,"WebDescription") %>
																	'>
																	</asp:Literal>
																</ItemTemplate>
															</rad:GridTemplateColumn>
														</Columns>
													</MasterTableView>
												</rad:RadGrid>
												<rad:RadGrid ID="grdInstructors" runat="server" AutoGenerateColumns="False" AllowPaging="true" AllowFilteringByColumn="true" AllowSorting="true" SortingSettings-SortedDescToolTip="Sorted Descending" SortingSettings-SortedAscToolTip="Sorted Ascending">
													<GroupingSettings CaseSensitive="false" />
													<MasterTableView AllowFilteringByColumn="true" AllowSorting="true" AllowNaturalSort="false">
														<Columns>
															<rad:GridBoundColumn DataField="Name" HeaderText="Instructor" SortExpression="Name" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"  />
															<rad:GridBoundColumn DataField="Location" HeaderText="Location" SortExpression="Location" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"  />
															<rad:GridTemplateColumn HeaderText="Email" DataField="Email1" SortExpression="Email1" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" >
																<ItemTemplate>
																	<asp:HyperLink ID="lnkEmail" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Email1") %>' NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"Email1Url") %>'>
																			</asp:HyperLink>
																		</ItemTemplate>
																	</rad:GridTemplateColumn>
																</Columns>
															</MasterTableView>
														</rad:RadGrid>
														<rad:RadGrid ID="grdLocations" runat="server" AutoGenerateColumns="False" AllowPaging="true" AllowFilteringByColumn="true" AllowSorting="true" SortingSettings-SortedDescToolTip="Sorted Descending" SortingSettings-SortedAscToolTip="Sorted Ascending">
															<GroupingSettings CaseSensitive="false" />
															<MasterTableView AllowFilteringByColumn="true" AllowSorting="true" AllowNaturalSort="false">
																<Columns>
																	<rad:GridBoundColumn DataField="Name" HeaderText="Name" SortExpression="Name" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
																	<rad:GridBoundColumn DataField="Location" HeaderText="Location" SortExpression="Location" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"  />
																</Columns>
															</MasterTableView>
														</rad:RadGrid>
														<asp:Panel runat="server" ID="pnlSchedule">
															<uc1:ClassScheduleControl ID="ClassScheduleControl" runat="server" CourseVisible="false" CategoryVisible="false" InstructorVisible="false" LocationVisible="true" />
														</asp:Panel>
													</div>
												</div>
                                    		</div>
				      <cc3:User runat="server" ID="User1" />
        </contenttemplate>
</asp:UpdatePanel>
