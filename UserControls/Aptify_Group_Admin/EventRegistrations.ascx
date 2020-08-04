<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="EventRegistrations.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.CustomerService.EventRegistrations" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Src="../Aptify_General/DatePicker.ascx" TagName="DatePicker" TagPrefix="uc1" %>
<!-- content start -->
<telerik:radajaxmanager id="RadAjaxManager1" runat="server">
    <ajaxsettings>
        <telerik:AjaxSetting AjaxControlID="RadMonthYearPicker1">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="RadMonthYearPicker"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
    </ajaxsettings>
</telerik:radajaxmanager>
<!-- content end -->
<div class="table-div">
    <telerik:radtabstrip id="RadTabEventStrip" skin="Sunset" selectedindex="0" runat="server"
        multipageid="RadMultiPageUpcomingRegistration" enableembeddedskins="true">
        <tabs>
            <telerik:RadTab id="t1" runat="server" Selected="True" Text="Upcoming Events" PageViewID="PvUpcomingRegistration">
            </telerik:RadTab>
            <telerik:RadTab id="t2" runat="server" Text="Past Events" PageViewID="PvPastRegistration">
            </telerik:RadTab>
        </tabs>
    </telerik:radtabstrip>
</div>
<%--<div class="CSSForTdBottom" style="padding-top: 6px !important;">
    <asp:Image ID="Image2" CssClass="ImgBarlineWidth" ImageUrl="~/Images/BarLine.png"
        runat="server" />
</div>--%>
<div>
    <telerik:radmultipage id="RadMultiPageUpcomingRegistration" runat="server" selectedindex="0">
        <telerik:RadPageView ID="PvUpcomingRegistration" runat="server">
        <div class="selected-tab-container">
             <div class="row-div clearfix">
                <div class="float-left label w12">Select Month:</div>
                <div class="float-left">
                <telerik:RadMonthYearPicker ToolTip="Select a Month" Skin="Default" ID="dtpMonthYearPicker" runat="server" 
                AutoPostBack="True" Culture="en-US" OnSelectedDateChanged="RadMonthYearPicker_SelectedDateChanged">
                    <dateinput id="DateInput1" runat="server" autopostback="True" dateformat="MMMM, yyyy"
                        displaydateformat="MMMM, yyyy" displaytext="" type="text" value="">
                    </dateinput>
                    <datepopupbutton hoverimageurl="" tooltip="Select a Month" imageurl="" />
                </telerik:RadMonthYearPicker>
                </div>
                <div class="float-left w15">
                <span class="label padding-left-right">OR </span><span class="label"> Select Events:</span>
                </div>
                <div class="float-left">
                <asp:DropDownList ID="ddlEvent" runat="server" AutoPostBack="True" ToolTip="Select a Event">
                </asp:DropDownList>
                </div>
            </div>
            <div class="row-div">
                <telerik:RadGrid ID="grdResults" runat="server" AllowPaging="True" AllowSorting="True"
                    EnableViewState="true" AllowFilteringByColumn="true" AutoGenerateColumns="False"
                    OnNeedDataSource="grdResults_NeedDataSource" CellSpacing="0" AllowMultiRowSelection="False"
                    SortingSettings-SortedDescToolTip="Sorted Descending" SortingSettings-SortedAscToolTip="Sorted Ascending"
                    DataKeyNames="ProductID" GridLines="None" >
                    <groupingsettings casesensitive="false" />
                    <pagerstyle mode="NextPrevAndNumeric"></pagerstyle>
                    <mastertableview datakeynames="MeetingID" allowfilteringbycolumn="True" allowmulticolumnsorting="false"
                        allownaturalsort="false">
                        <%--Amruta,Issue 15349 ,3/25/2013,Changed message from "No child records to display" to "Nothing to display" for child grid--%>
                        <DetailTables>
                            <telerik:GridTableView DataKeyNames="MeetingID" runat="server" AllowFilteringByColumn="false"
                                NoDetailRecordsText="Nothing to display" AllowSorting="false">
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Session" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains"
                                        AllowFiltering="false" DataField="MeetingTitle"  ShowFilterIcon="false"
                                        SortExpression="MeetingTitle">
                                        <HeaderStyle CssClass="w25"/>
                                        <ItemTemplate>
                                            <asp:HyperLink ID="lnkWebName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"MeetingTitle") %>'
                                                NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"MeetingUrl") %>'></asp:HyperLink>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridDateTimeColumn UniqueName="GridDateTimeColumnStartDate" AllowSorting="true"
                                        AllowFiltering="false"  Visible="True" HeaderText="Date"
                                        DataField="MonthDate" SortExpression="MonthDate" ShowFilterIcon="false" EnableTimeIndependentFiltering="true">
                                        <ItemStyle VerticalAlign="top"/>
                                        <HeaderStyle CssClass="w20"/>
                                    </telerik:GridDateTimeColumn>
                                    <telerik:GridTemplateColumn HeaderText="Venue" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains"
                                        AllowFiltering="false" DataField="Venue" ShowFilterIcon="false"
                                        SortExpression="Venue">
                                        <HeaderStyle CssClass="w30"/>
                                        <ItemTemplate>
                                            <asp:Label ID="lblLocation" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Venue") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="top" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Registered Members Count" AllowFiltering="false"
                                        UniqueName="RegisteredMembersCount"  ShowFilterIcon="false">
                                        <HeaderStyle HorizontalAlign="Center" CssClass="w25"/>
                                        <ItemTemplate>
                                            <div class="row-div clearfix">
                                                <div class="float-left w33-3">
                                                    <center>
                                                    <asp:HyperLink ID="lnkMConfirmedPast" runat="server" CssClass="label" NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"AdminConfirmedUrl") %>'
                                                    Text='<%# DataBinder.Eval(Container.DataItem,"Confirmed")%>'></asp:HyperLink>
                                                    </center>
                                                </div>
                                                <div class="float-left w33-3">
                                                    <center>
                                                    <asp:HyperLink ID="lnkMWaitList0" runat="server" CssClass="label"  NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"AdminWaitListUrl") %>'
                                                    Text='<%# DataBinder.Eval(Container.DataItem,"WaitList") %>'></asp:HyperLink>
                                                    </center>
                                                </div>
                                                <div class="float-left w33-3">
                                                    <center>
                                                    <asp:HyperLink ID="lnksMAll0" runat="server" CssClass="label"  NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"AdminAllUrl") %>'
                                                    Text='<%# DataBinder.Eval(Container.DataItem,"sAll") %>'></asp:HyperLink>
                                                    </center>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="top" />
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </telerik:GridTableView>
                        </DetailTables>
                        <Columns>
                            <telerik:GridTemplateColumn AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                DataField="MeetingTitle" HeaderText="Event/Session"
                                ShowFilterIcon="false" SortExpression="MeetingTitle">
                                <HeaderStyle CssClass="w25"/>
                                <ItemStyle />
                                <ItemTemplate>
                                    <asp:HyperLink ID="lnkMeetingTitle" runat="server" NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"AdminMeetingTitleUrl") %>'
                                        Text='<%# DataBinder.Eval(Container.DataItem,"MeetingTitle") %>'></asp:HyperLink>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridDateTimeColumn UniqueName="GridDateTimeColumnStartDate" AllowSorting="true"
                                 AllowFiltering="false" Visible="True" HeaderText="Date"
                                DataField="MonthDate" SortExpression="MonthDate" ShowFilterIcon="false" EnableTimeIndependentFiltering="true">
                                <ItemStyle CssClass="w20"/>
                            </telerik:GridDateTimeColumn>
                            <telerik:GridBoundColumn AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                DataField="Venue" HeaderText="Venue" ShowFilterIcon="false"
                                SortExpression="Venue">
                                <ItemStyle CssClass="w30"/>
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn AutoPostBackOnFilter="true" DataField="ProductID" AllowFiltering="true"
                                HeaderText="Registered Members Count" SortExpression="ProductID" UniqueName="TemplateColumn">
                                <ItemStyle CssClass="w25"/>
                                <HeaderTemplate>
                                    <div class="row-div ">
                                                Registered Members Count                                        
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div class="row-div clearfix">
                                        <div class="float-left w33-3">
                                            <center>
                                                <asp:HyperLink CssClass="label" ID="lnkMConfirmed" runat="server" NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"AdminConfirmedUrl") %>'
                                                    Text='<%# DataBinder.Eval(Container.DataItem,"Confirmed")%>'></asp:HyperLink>
                                            </center>
                                        </div>
                                        <div class="float-left w33-3">
                                            <center>
                                                <asp:HyperLink CssClass="label" ID="lnkMWaitList" runat="server" NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"AdminWaitListUrl") %>'
                                                     Text='<%# DataBinder.Eval(Container.DataItem,"WaitList") %>'></asp:HyperLink>
                                            </center>
                                        </div>
                                        <div class="float-left w33-3">
                                        <center>
                                            <asp:HyperLink CssClass="label" ID="lnksMAll" runat="server" NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"AdminAllUrl") %>'
                                                 Text='<%# DataBinder.Eval(Container.DataItem,"sAll") %>'></asp:HyperLink>
                                                 </center>
                                        </div>
                                    </div>
                                </ItemTemplate>
                                <FilterTemplate>
                                    <div class="row-div clearfix">
                                        <div class="float-left w33-3">
                                            <center>
                                                <asp:Label ID="lblid" runat="server"  Text="Confirmed"></asp:Label>
                                            </center>
                                        </div>
                                        <div class="float-left w33-3">
                                        <center>
                                            <asp:Label ID="Label1" runat="server"  Text="WaitList"></asp:Label>
                                            </center>
                                        </div>
                                        <div class="float-left w33-3">
                                            <center>
                                                <asp:Label ID="Label2" runat="server" Text="All"></asp:Label>
                                            </center>
                                        </div>
                                    </div>
                                </FilterTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <EditFormSettings>
                            <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                            </EditColumn>
                        </EditFormSettings>
                    </mastertableview>
                    <filtermenu enableimagesprites="False">
                    </filtermenu>
                </telerik:RadGrid>
            </div>
        </div>
        </telerik:RadPageView>

        <telerik:RadPageView ID="PvPastRegistration" runat="server">            
        <div class="selected-tab-container">
            <div class="row-div clearfix">                    
                <div class="float-left label w12">Select Month:</div>
                <div class="float-left">
                <telerik:RadMonthYearPicker ToolTip="Select a Month" Skin="Default" ID="RadMonthYearPickerPast" runat="server" 
                AutoPostBack="True" Culture="en-US" OnSelectedDateChanged="RadMonthYearPickerPast_SelectedDateChanged">
                    <dateinput id="DateInput2" runat="server" autopostback="True" dateformat="MMMM, yyyy"
                        displaydateformat="MMMM, yyyy" displaytext="" type="text" value="">
                    </dateinput>
                    <datepopupbutton hoverimageurl="" imageurl="" tooltip="Select a Month" />
                </telerik:RadMonthYearPicker>
                </div>
                <div class="float-left w15">
                <span class="label padding-left-right">OR </span><span class="label"> Select Events:</span>
                </div>
                <div class="float-left">
                <asp:DropDownList ID="ddlPastEvent" ToolTip="Select a Event" runat="server" AutoPostBack="True">
                </asp:DropDownList>
                </div>
            </div>
                       
            <div class="row-div">
                <telerik:RadGrid ID="grdResultsPast" runat="server" AllowFilteringByColumn="True"
                    SortingSettings-SortedDescToolTip="Sorted Descending" SortingSettings-SortedAscToolTip="Sorted Ascending"
                    AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" CellSpacing="0"
                    DataKeyNames="ProductID" GridLines="None">
                    <groupingsettings casesensitive="false" />
                    <mastertableview datakeynames="MeetingID" allowfilteringbycolumn="true" allowsorting="true"
                        allownaturalsort="false">
                        <DetailTables>
                            <telerik:GridTableView DataKeyNames="MeetingID" runat="server" AllowFilteringByColumn="false"
                                NoDetailRecordsText="Nothing to display" AllowSorting="false">
                                <Columns>
                                    <telerik:GridTemplateColumn ItemStyle-CssClass="child" HeaderText="Session" AutoPostBackOnFilter="false"
                                        CurrentFilterFunction="Contains" DataField="MeetingTitle" 
                                        ShowFilterIcon="false" SortExpression="MeetingTitle">
                                        <HeaderStyle CssClass="w25"/>
                                        <ItemTemplate>
                                            <asp:HyperLink ID="lnkWebName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"MeetingTitle") %>'
                                                NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"MeetingUrl") %>'></asp:HyperLink>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridDateTimeColumn UniqueName="GridDateTimeColumnStartDate" AllowSorting="true"
                                        AllowFiltering="false" Visible="True" HeaderText="Date"
                                        DataField="MonthDate" SortExpression="MonthDate" ShowFilterIcon="false" EnableTimeIndependentFiltering="true">
                                        <HeaderStyle CssClass="w20"/>
                                    </telerik:GridDateTimeColumn>
                                    <telerik:GridTemplateColumn HeaderText="Venue" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains"
                                        DataField="Venue" ShowFilterIcon="false" SortExpression="Venue">
                                        <HeaderStyle CssClass="w30"/>
                                        <ItemTemplate>
                                            <asp:Label ID="lblLocation" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Venue") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="top" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Registered Members Count" ShowFilterIcon="false">
                                        <HeaderStyle HorizontalAlign="Center" CssClass="w25"/>
                                        <ItemTemplate>
                                            <div class="row-div clearfix">
                                                <div class="float-left w33-3">
                                                    <center>
                                                        <asp:HyperLink ID="lnkMConfirmedPast" CssClass="label" runat="server" NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"AdminConfirmedUrl") %>'
                                                                Text='<%# DataBinder.Eval(Container.DataItem,"Confirmed")%>'></asp:HyperLink>
                                                    </center>
                                                </div>
                                                <div class="float-left w33-3">
                                                    <center>
                                                        <asp:HyperLink ID="lnkMWaitList0" CssClass="label" runat="server" NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"AdminWaitListUrl") %>'
                                                            Text='<%# DataBinder.Eval(Container.DataItem,"WaitList") %>'></asp:HyperLink>
                                                    </center>
                                                </div>
                                                <div class="float-left w33-3">
                                                    <center>
                                                        <asp:HyperLink ID="lnksMAll0" CssClass="label" runat="server"  NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"AdminAllUrl") %>'
                                                            Text='<%# DataBinder.Eval(Container.DataItem,"sAll") %>'></asp:HyperLink>
                                                    </center>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="top" />
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </telerik:GridTableView>
                        </DetailTables>
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="ID" HeaderButtonType="TextButton" SortExpression="MeetingID"
                                DataField="Productid" UniqueName="ID" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                                ShowFilterIcon="false" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="Product" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Productid") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn DataField="MeetingTitle" HeaderText="Event/Session" 
                                SortExpression="MeetingTitle" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                ShowFilterIcon="false">
                                <ItemStyle  CssClass="w25"/>
                                <ItemTemplate>
                                    <asp:HyperLink ID="lnkMeetingTitlePast" runat="server" NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"AdminMeetingTitleUrlPast") %>'
                                        Text='<%# DataBinder.Eval(Container.DataItem,"MeetingTitle") %>'></asp:HyperLink>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridDateTimeColumn UniqueName="GridDateTimeColumnStartDate" AllowSorting="true"
                                    AllowFiltering="false" Visible="True" HeaderText="Date"
                                DataField="MonthDate" SortExpression="MonthDate" ShowFilterIcon="false" EnableTimeIndependentFiltering="true">
                                <ItemStyle  CssClass="w20"/>
                            </telerik:GridDateTimeColumn>
                            <telerik:GridBoundColumn DataField="Venue" HeaderText="Venue"
                                SortExpression="Venue" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                ShowFilterIcon="false">
                                <ItemStyle  CssClass="w30"/>
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn AllowFiltering="true" FilterControlAltText="test" HeaderStyle-HorizontalAlign="Center"
                                HeaderText="Registered Members Count" SortExpression="ProductID" UniqueName="TemplateColumn">
                                <ItemStyle  CssClass="w25"/>
                                <HeaderTemplate>
                                    <div class="row-div" id="tblMRegisteredPast">
                                        Registered Members Count
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div class="row-div clearfix">
                                        <div class="float-left w33-3">
                                            <center>
                                                <asp:HyperLink ID="lnkMConfirmedPast" CssClass="label" runat="server" NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"AdminConfirmedUrlPast") %>'
                                                    Text='<%# DataBinder.Eval(Container.DataItem,"Confirmed")%>'></asp:HyperLink>
                                            </center>
                                        </div>
                                        <div class="float-left w33-3">
                                            <center>
                                                <asp:HyperLink ID="lnkMWaitList0" CssClass="label" runat="server" NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"AdminWaitListUrlPast") %>'
                                                    Text='<%# DataBinder.Eval(Container.DataItem,"WaitList") %>'></asp:HyperLink>
                                            </center>
                                        </div>
                                        <div class="float-left w33-3">
                                            <center>
                                                <asp:HyperLink ID="lnksMAll0" CssClass="label" runat="server" NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"AdminAllUrlPast") %>'
                                                    Text='<%# DataBinder.Eval(Container.DataItem,"sAll") %>'></asp:HyperLink>
                                            </center>
                                        </div>
                                    </div>
                                </ItemTemplate>
                                <FilterTemplate>
                                    <div class="row-div clearfix">
                                        <div class="float-left w33-3">
                                            <center>
                                                    <asp:Label ID="Label4" runat="server"  Text="Confirmed"></asp:Label>
                                                </center>
                                        </div>
                                        <div class="float-left w33-3">
                                            <center>
                                                   
                                                    <asp:Label ID="Label3" runat="server" Text="WaitList"></asp:Label>
                                                   
                                            </center>
                                        </div>
                                        <div class="float-left w33-3">
                                            <center>
                                                <asp:Label ID="Label1" runat="server" Text="All"></asp:Label></center>
                                        </div>
                                    </div>
                                </FilterTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </mastertableview>
                </telerik:RadGrid>
            </div>
        </div>
        </telerik:RadPageView>
    </telerik:radmultipage>
</div>
<div>
    <asp:Label ID="lblMessage" runat="server"></asp:Label>
    <cc2:user id="User1" runat="server" />
</div>
