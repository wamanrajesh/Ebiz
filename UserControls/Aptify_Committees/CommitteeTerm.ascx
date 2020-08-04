<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="vb" AutoEventWireup="false" Inherits="Aptify.Framework.Web.eBusiness.Committees.CommitteeTermControl"
    CodeFile="CommitteeTerm.ascx.vb" %>
<%@ Register Src="../Aptify_General/RecordAttachments.ascx" TagName="RecordAttachments"
    TagPrefix="uc2" %>
<%@ Register Src="../Aptify_Forums/SingleForum.ascx" TagName="SingleForum" TagPrefix="uc1" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessGlobal" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEbusinessUser" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<div class="table-div" runat="server" id="tblMain">
    <div class="row-div header-title-bottom-border clearfix">
        <asp:Label ID="lblCommittee" runat="server" CssClass="control-title">
        </asp:Label>
        <br />
        <asp:Label ID="lblTerm" runat="server">
        </asp:Label>
    </div>
    <div class="row-div clearfix">
        <div class="float-left w15">
            <div class="row-div clearfix">
                <img runat="server" id="generalSmall" src="" alt="General Info" />
                <asp:HyperLink runat="server" ID="lnkGeneral" Text="General" ToolTip="View general information about the committee term" />
            </div>
            <div class="row-div clearfix">
                <img runat="server" id="memberSmall" src="" alt="Member List" />
                <asp:HyperLink runat="server" ID="lnkMembers" Text="Members" ToolTip="View members in this committee term" />
            </div>
            <div runat="server" id="trForum" class="row-div clearfix">
                <img runat="server" id="forumSmall" src="" alt="Forum" />
                <asp:HyperLink runat="server" ID="lnkForum" Text="Forum" ToolTip="Discussion forum with committee members" />
            </div>
            <div runat="server" id="trDocuments" class="row-div clearfix">
                <img runat="server" id="documentsSmall" src="" alt="Documents" />
                <asp:HyperLink runat="server" ID="lnkDocuments" Text="Documents" ToolTip="Document Library for committee members" />
            </div>
        </div>
        <div class="float-right w83 left-border" runat="server" id="tdExtContent">
            <img runat="server" id="imgTitle" src="" alt="Committee Information" Class="middle-img" />
            <asp:Label runat="server" CssClass="control-title" ID="lblTitle" />
            <br />
            <asp:Label runat="server" ID="lblDetails" />
            <br />
            <div runat="server" id="pnlGeneral" visible="false">
                <div class="row-div clearfix">
                    <div class="label-div w19">
                       Director: 
                    </div>
                    <div class="field-div1 w80">
                        <asp:Label runat="server" ID="lblDirector" />
                    </div>
                </div>
                <div class="row-div clearfix">
                    <div class="label-div w19">
                        Start Date: 
                    </div>
                    <div class="field-div1 w80">
                        <asp:Label runat="server" ID="lblStartDate" />
                    </div>
                </div>
                <div class="row-div clearfix">
                    <div class="label-div w19">
                        End Date: 
                    </div>
                    <div class="field-div1 w80">
                        <asp:Label runat="server" ID="lblEndDate" />
                    </div>
                </div>
                <div class="row-div clearfix">
                    <div class="label-div w19">
                        Goals:
                    </div>
                    <div class="field-div1 w80">
                        <asp:Label runat="server" ID="lblGoals" />
                    </div>
                </div>
                <div class="row-div clearfix">
                    <div class="label-div w19">
                       Accomplishments: 
                    </div>
                    <div class="field-div1 w80">
                        <asp:Label runat="server" ID="lblAccomplishments" />
                    </div>
                </div>
            </div>
            <asp:UpdatePanel ID="updPanelGrid" runat="server">
                <contenttemplate> 
                        <rad:RadGrid ID="grdMembers" runat="server" AutoGenerateColumns="False" AllowPaging="true" AllowFilteringByColumn="false"
                                Visible="false" SortingSettings-SortedDescToolTip ="Sorted Descending" SortingSettings-SortedAscToolTip="Sort
                                Ascending">
                                 <GroupingSettings CaseSensitive="false" /> <MasterTableView AllowSorting="true" AllowNaturalSort="false"> 
                                 <Columns>
                                        <rad:GridBoundColumn DataField="Member" HeaderText="Member" SortExpression="Member" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false" /> 
                                        <rad:GridBoundColumn DataField="Title" HeaderText="Title" SortExpression="Title" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false" />
                                        <rad:GridDateTimeColumn DataField="StartDate" UniqueName="GridDateTimeColumnStartDate"
                                        HeaderText="Start" SortExpression="StartDate" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                                        ShowFilterIcon="false" EnableTimeIndependentFiltering ="true" DataType="System.DateTime"/>
                                        <rad:GridDateTimeColumn DataField="EndDate" UniqueName="GridDateTimeColumnEndDate" HeaderText="End" SortExpression="EndDate" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                                        ShowFilterIcon="false" EnableTimeIndependentFiltering="true" DataType="System.DateTime" /> 
                                        <rad:GridHyperLinkColumn DataNavigateUrlFields="Email1" DataNavigateUrlFormatString="mailto:{0}" DataTextField="Email1" HeaderText="Email" SortExpression="Email1" AutoPostBackOnFilter="true"
                                        CurrentFilterFunction="Contains" ShowFilterIcon="false" /> 
                                    </Columns>
                                </MasterTableView>
                        </rad:RadGrid> 
                </contenttemplate>
            </asp:UpdatePanel>
            <uc1:SingleForum ID="SingleForum1" runat="server" Visible="false" />
            <uc2:RecordAttachments ID="RecordAttachments" Visible="false" runat="server">
            </uc2:RecordAttachments>
        </div>
    </div>
</div>
<cc3:User runat="server" ID="User1" />
