<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ViewClass.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.Education.ViewClassControl" %>
<%@ Register Src="../Aptify_General/RecordAttachments.ascx" TagName="RecordAttachments"
    TagPrefix="uc3" %>
<%@ Register Src="InstructorValidator.ascx" TagName="InstructorValidator" TagPrefix="uc2" %>
<%@ Register Src="../Aptify_Forums/SingleForum.ascx" TagName="SingleForum" TagPrefix="uc1" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="uc4" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessShoppingCart" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<div class="errormsg-div">
    <asp:Label runat="server" ID="lblError" Visible="false"></asp:Label>
</div>
<script type="text/javascript">
    function _do_open_content(url) {
        playerWindow = window.open(url, '_aptify_e_learning_content', 'toolbar=no,menubar=no,location=no,directories=no,status=no,resizable=yes,scrollbars=no');
    }

    function Reloadwindow() {
        window.location.reload();
    }
</script>
<div id="tblMain" runat="server" class="table-div">
    <div class="header-title-bottom-border label">
        <asp:Label runat="server" ID="lblName" />
    </div>
    
    <div class="row-div top-margin clearfix" id="trDescription" runat="server">
    
       <div class="float-left w10">
        <div class="row-div">
            <asp:Label runat="server" ID="lblDescription" />
        </div>
        </div>
       <div class="float-right w40">
            <div class="row-div clearfix ">
                <img id="imgSchedule" runat="server" alt="Schedule" src="" border="0"  />
                <b>Schedule</b><br />
            </div>
            <div class="row-div clearfix ">
                <div class="label-div ">
                    <i>Starts</i>
                </div>
                <div class="field-div">
                    <asp:Label CssClass="MeetingDates" runat="server" ID="lblStartDate" />
                </div>
            </div>
            <div class="row-div clearfix">
                <div class="label-div">
                    <i>Ends</i>
                </div>
                <div class="field-div">
                    <asp:Label CssClass="MeetingDates" runat="server" ID="lblEndDate" />
                </div>
            </div>
        </div>
     </div>
        <div class="row-div clearfix "  runat="server" id="trStudentStatus" visible="false">
            <asp:Label runat="server" ID="lblStudentStatus"  />
            <br />
            <asp:Label runat="server" ID="lblRegisterDates" />
        </div>
        
    <div class="header-title-bottom-border"></div>
    <div class="table-div top-margin clearfix" id="trContent" runat="server">
         <div class="row-div clearfix float-left w18">
                <div class="row-div">
                    <img runat="server" id="imgGenInfoSmall" src="" alt="General Info" border="0" />
                     &nbsp;
                    <asp:HyperLink runat="server" ID="lnkGeneral" Text="General" ToolTip="View general information about the class" />
                </div>
                <div class="row-div" id="trInstructors" runat="server">
                    <img runat="server" id="imgInstructorSmall" src="" alt="Instructor Info" />
                    &nbsp;
                    <asp:HyperLink runat="server" ID="lnkInstructorInfo" Text="Instructor Info" 
                        ToolTip="View information about the instructor of this class" />
                </div>
                <div class="row-div">
                    <img runat="server" id="imgSyllabusSmall" src="" alt="Syllabus" border="0" />
                    &nbsp;
                    <asp:HyperLink runat="server" ID="lnkSyllabus" Text="Syllabus" ToolTip="View details about the class" />
                </div>
                <div class="row-div" id="trNotes" runat="server">
                    <img runat="server" id="imgNotesSmall" src="" alt="Notes" border="0"  />
                    &nbsp;
                    <asp:HyperLink runat="server" ID="lnkNotes" Text="My Notes" ToolTip="View your own notes about the class" />
                </div>
                <div class="row-div" id="trForum" runat="server">
                    <img runat="server" id="imgForumSmall" src="" alt="Discussion Forum" border="0" />
                    &nbsp;
                    <asp:HyperLink runat="server" ID="lnkForum" Text="Discussion" ToolTip="Discussion Forum with instructor and other students" />
                </div>
                <div class="row-div" id="trDocuments" runat="server">
                    <img runat="server" id="imgDocumentSmall" src="" alt="Documents" border="0" />
                    &nbsp;
                    <asp:HyperLink runat="server" ID="lnkDocuments" Text="Documents" 
                        ToolTip="View documents posted by the instructor" />
                </div>
                <div class="row-div" id="trStudents" runat="server" visible="false">
                    <img runat="server" id="imgStudentSmall" src="" alt="Students" border="0" />
                    &nbsp;
                    <asp:HyperLink runat="server" ID="lnkStudents" Text="Students" ToolTip="View list of all students registered for this class (Instructors Only)" />
                </div>
                <div class="row-div" id="trRegister" runat="server">
                    <img runat="server" id="imgRegisterSmall" src="" alt="Register for Class" border="0" />
                    &nbsp;
                    <asp:HyperLink runat="server" ID="lnkRegister" Text="Register!" 
                        ToolTip="Register for this class now by clicking on this link..." />
                </div>
                <div class="row-div" id="trRegisterMeeting" runat="server">
                    <img runat="server" id="imgRegisterSmall2" src="" alt="Register for Class" border="0" />
                    <asp:LinkButton ID="lnkRegisterMeeting" runat="server">
			           Register
                    </asp:LinkButton>
                </div>
                <div class="row-div">
                    <asp:Label ID="lblPrerequisiteCheck" Visible="False" runat="server">
                    </asp:Label>
                </div>
        </div>
        <div class="float-right w79 left-border" runat="server" id="tdExtContent">
            <asp:Image runat="server" ID="imgTitle" CssClass="middle-img" AlternateText="Class Information"
                ImageUrl="" />
            <asp:Label runat="server" ID="lblTitle" /><br />
            <asp:Label runat="server" ID="lblDetails"  />&nbsp;
            <asp:Label runat="server" ID="lblNote" ></asp:Label>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <contenttemplate>
                        <rad:RadGrid ID="grdSyllabus" runat="server" AutoGenerateColumns="False" AllowPaging="true"
                            AllowFilteringByColumn="true" SortingSettings-SortedDescToolTip="Sorted Descending" AllowSorting="false"
                            SortingSettings-SortedAscToolTip="Sorted Ascending">
                            <GroupingSettings CaseSensitive="false" />
                            <MasterTableView AllowFilteringByColumn="true" AllowSorting="true" AllowNaturalSort="false">
                                <Columns>
                                    <rad:GridTemplateColumn HeaderText="Item" DataField="WebName" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"
                                        >
                                        <ItemTemplate>
                                            <asp:HyperLink ID="lnkWebName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"WebName") %>'
                                                NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"WebURLUrl") %>'></asp:HyperLink>
                                        </ItemTemplate>
                                    </rad:GridTemplateColumn>
                                    <rad:GridTemplateColumn HeaderText="Description" DataField="WebDescription" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"
                                        >
                                        <ItemTemplate>
                                            <asp:Label ID="lblWebDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"WebDescription") %>'></asp:Label>
                                        </ItemTemplate>
                                    </rad:GridTemplateColumn>
                                    <rad:GridTemplateColumn HeaderText="Type" DataField="Type" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Type") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" />
                                    </rad:GridTemplateColumn>
                            
                                    <rad:GridBoundColumn DataField="Duration" DataFormatString="{0:F0} min" HeaderText="Duration"
                                        AllowSorting="false" AllowFiltering="false" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false"  />
                                    <rad:GridBoundColumn DataField="CourseStatus" HeaderText="Status"  AllowSorting="false" 
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"
                                         />
                                </Columns>
                            </MasterTableView>
                        </rad:RadGrid>
                        <rad:RadGrid ID="grdStudents" runat="server" AutoGenerateColumns="False" AllowPaging="true"
                            AllowFilteringByColumn="true">
                            <MasterTableView AllowFilteringByColumn="true" AllowSorting="true" AllowNaturalSort="false">
                                <Columns>
                                    <rad:GridTemplateColumn DataField="LastName" HeaderText="Last" SortExpression="LastName"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"
                                        >
                                        <ItemTemplate>
                                            <asp:HyperLink ID="lnkLastName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"LastName") %>'
                                                NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"LastNameUrl") %>'></asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle />
                                    </rad:GridTemplateColumn>
                                    <rad:GridTemplateColumn DataField="FirstName" HeaderText="First" SortExpression="FirstName"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"
                                        >
                                        <ItemTemplate>
                                            <asp:HyperLink ID="lnkFirstName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"FirstName") %>'
                                                NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"FirstNameUrl") %>'></asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle />
                                    </rad:GridTemplateColumn>
                                    <rad:GridDateTimeColumn UniqueName="GridDateTimeColumnDateRegistered" AllowSorting="true"
                                        Visible="True" HeaderText="DateRegistered" DataField="DateRegistered" SortExpression="DateRegistered"
                                        ReadOnly="true" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false"
                                        DataType="System.DateTime" EnableTimeIndependentFiltering="true" >
                                    </rad:GridDateTimeColumn>
                                    <rad:GridDateTimeColumn UniqueName="GridDateTimeColumnDateCompleted" AllowSorting="true"
                                        Visible="True" HeaderText="DateCompleted" DataField="DateCompleted" SortExpression="DateCompleted"
                                        ReadOnly="true" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false"
                                        DataType="System.DateTime" EnableTimeIndependentFiltering="true" >
                                    </rad:GridDateTimeColumn>
                                    <rad:GridDateTimeColumn UniqueName="GridDateTimeColumnDateAvailable" AllowSorting="true"
                                        Visible="True" HeaderText="DateAvailable" DataField="DateAvailable" SortExpression="DateAvailable"
                                        ReadOnly="true" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false"
                                        DataType="System.DateTime" EnableTimeIndependentFiltering="true" >
                                    </rad:GridDateTimeColumn>
                                    <rad:GridDateTimeColumn UniqueName="GridDateTimeColumnDateExpires" AllowSorting="true"
                                        Visible="True" HeaderText="DateExpires" DataField="DateExpires" SortExpression="DateExpires"
                                        ReadOnly="true" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false"
                                        DataType="System.DateTime" EnableTimeIndependentFiltering="true" >
                                    </rad:GridDateTimeColumn>
                                    <rad:GridTemplateColumn DataField="Status" HeaderText="Status" SortExpression="Status"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Status") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle  />
                                    </rad:GridTemplateColumn>
                                    <rad:GridTemplateColumn DataField="Score" HeaderText="Score" SortExpression="ScoreUrl"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false"
                                        ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label ID="lblScore" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ScoreUrl") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle />
                                    </rad:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </rad:RadGrid>
                        </contenttemplate>
            </asp:UpdatePanel>
            <asp:Panel ID="pnlForum" runat="server">
                <uc1:SingleForum ID="SingleForum" runat="server" />
                <br />
                <asp:Button CssClass="submit-Btn" runat="server" ID="btnCreateForum" Text="Create Forum" />
            </asp:Panel>
            <asp:Panel ID="pnlDocuments" runat="server">
                <uc3:RecordAttachments ID="RecordAttachments" runat="server" />
            </asp:Panel>
            <asp:Panel ID="pnlNotes" Visible="false" runat="server">
                <br />
                <div runat="server" id="divStudentNotes">
                    <asp:Button CssClass="submit-Btn" runat="server" ID="btnEditStudentNotes" Text="Edit" />
                    <asp:Button CssClass="submit-Btn" runat="server" Visible="false" ID="btnSaveStudentNotes"
                        Text="Save" />
                    <asp:Button CssClass="submit-Btn" runat="server" Visible="false" ID="btnCancelStudentNotes"
                        Text="Cancel" />
                    <asp:Label runat="server" ID="lblStudentNotesMessage" Visible="false"></asp:Label><br />
                    <asp:TextBox runat="server" Visible="false" ID="txtStudentNotes" TextMode="multiLine"></asp:TextBox>
                    <br />
                </div>
                <pre>
                <asp:Literal runat="server" ID="lblStudentNotes"></asp:Literal></pre>
            </asp:Panel>
            <div class="table-div" runat="server" id="tblInstructor">
                <div class="row-div clearfix">
                    <asp:Label runat="server" CssClass="label" ID="lblInstructor" />
                </div>
                <div class="row-div clearfix">
                    <div class="label-div">
                        Location
                    </div>
                    <div class="field-div1">
                        <asp:Label runat="server" ID="lblInstructorLocation" />
                    </div>
                </div>
                <div class="row-div clearfix">
                    <div class="label-div">
                        Email
                    </div>
                    <div class="field-div1">
                        <a href="" id="lnkInstructorEmail" runat="server">
                            <asp:Label runat="server" ID="lblInstructorEmail" /></a><br />
                        <br />
                    </div>
                </div>
                <div class="row-div clearfix" runat="server" id="trInstructorNotes">
                    <div class="row-div-bottom-line">
                        <img runat="server" id="imgInstrutorNotes" src="" alt="Notes Icon" class="middle-img"/>                    
                        <b>Instructor Notes</b><br />
                        The course instructor has recorded the following notes for the students of this
                        class.
                    </div>
                    <div runat="server" visible="false" id="divEditInstructorNotes">
                        <asp:Button CssClass="submit-Btn" runat="server" ID="btnEditInstructorNotes" Text="Edit" />
                        <asp:Button CssClass="submit-Btn" runat="server" Visible="false" ID="btnSaveInstructorNotes"
                            Text="Save" />
                        <asp:Button CssClass="submit-Btn" runat="server" Visible="false" ID="btnCancelInstructorNotes"
                            Text="Cancel" />
                        <asp:Label runat="server" ID="lblInstructorNotesMessage" Visible="false"></asp:Label><br />
                        <asp:TextBox runat="server" Visible="false" ID="txtInstructorNotes" TextMode="multiLine"
                            Width="400px" Height="200px"></asp:TextBox>
                        <br />
                    </div>
                    <pre>
                       <asp:Literal runat="server" ID="lblInstructorNotes"></asp:Literal></pre>
                </div>
            </div>
        </div>
        
    </div>
    <cc3:User ID="User1" runat="server" />
<uc2:InstructorValidator ID="InstructorValidator1" runat="server" />
<uc4:AptifyShoppingCart ID="ShoppingCart1" runat="server" Visible="False" />
</div>


