<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" Inherits="Aptify.Framework.Web.eBusiness.AbstractManagement.ReviewAbstracts"
    CodeFile="ReviewAbstracts.ascx.vb" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EbusinessGlobal" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEbusinessUser" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<div class="table-div" id="tblmain" runat="server">
    <div class="row-div">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <%--issue 14447 Prasad removed three step sorting added tooltip added date column--%>
                <rad:RadGrid ID="grdMain" runat="server" AutoGenerateColumns="false" AllowPaging="true"
                    AllowFilteringByColumn="true" SortingSettings-SortedDescToolTip="Sorted Descending"
                    SortingSettings-SortedAscToolTip="Sorted Ascending">
                    <GroupingSettings CaseSensitive="false" />
                    <MasterTableView AllowFilteringByColumn="true" AllowSorting="true" AllowNaturalSort="false">
                        <Columns>
                            <rad:GridTemplateColumn HeaderText="Subject" DataField="Subject" SortExpression="Subject"
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                <ItemTemplate>
                                    <asp:HyperLink ID="lnkSubject" runat="server" Text='
											<%# DataBinder.Eval(Container.DataItem,"Subject") %>
												' NavigateUrl='
												<%# DataBinder.Eval(Container.DataItem,"DataNavigateUrl") %>
													'>
                                    </asp:HyperLink>
                                </ItemTemplate>
                            </rad:GridTemplateColumn>
                            <rad:GridDateTimeColumn DataField="DateSubmitted" UniqueName="GridDateTimeColumnDateSubmitted"
                                HeaderText="Date Submitted" SortExpression="DateSubmitted" AutoPostBackOnFilter="true"
                                CurrentFilterFunction="EqualTo" ShowFilterIcon="false" DataType="System.DateTime"
                                EnableTimeIndependentFiltering="true"   />
                            <rad:GridBoundColumn DataField="Title" HeaderText="Title" SortExpression="Title"
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                            <rad:GridBoundColumn DataField="Category" HeaderText="Category" SortExpression="Category"
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                            <rad:GridBoundColumn DataField="SubmittedBy" HeaderText="Submitted By" SortExpression="SubmittedBy"
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                        </Columns>
                    </MasterTableView>
                </rad:RadGrid>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>
<cc3:User ID="AptifyEbusinessUser1" runat="server"></cc3:User>
