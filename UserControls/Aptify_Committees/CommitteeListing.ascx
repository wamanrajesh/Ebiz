<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="vb" AutoEventWireup="false" Inherits="Aptify.Framework.Web.eBusiness.Committees.CommitteeListingControl"
    CodeFile="CommitteeListing.ascx.vb" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessGlobal" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEbusinessUser" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<div class="committees-main-div">
    <asp:UpdatePanel ID="updPanelGrid" runat="server">
        <ContentTemplate>
            <rad:RadGrid ID="grdCommittees" runat="server" AutoGenerateColumns="False" AllowPaging="true"
                AllowFilteringByColumn="true" SortingSettings-SortedAscToolTip="Sorted Ascending" 
                SortingSettings-SortedDescToolTip ="Sorted Descending">
                <GroupingSettings CaseSensitive="false" />
                <MasterTableView AllowSorting="true" AllowNaturalSort="false">
                    <Columns>
                        <rad:GridBoundColumn DataField="Name" HeaderText="Name" SortExpression="Name" AutoPostBackOnFilter="true"
                            CurrentFilterFunction="Contains" ShowFilterIcon="false" 
                            />
                        <rad:GridBoundColumn DataField="Description" HeaderText="Description" SortExpression="Description"
                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" 
                             />
                        <rad:GridBoundColumn DataField="Goals" HeaderText="Goals" SortExpression="Goals"
                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" 
                            />
                        <rad:GridDateTimeColumn DataField="DateFounded" UniqueName="GridDateTimeColumnDateFounded" 
                        HeaderText="Date Founded" SortExpression="DateFounded" AutoPostBackOnFilter="true" 
                        CurrentFilterFunction="EqualTo" EnableTimeIndependentFiltering ="true" ShowFilterIcon="false" DataType="System.DateTime" />
                    </Columns>
                </MasterTableView>
            </rad:RadGrid>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>