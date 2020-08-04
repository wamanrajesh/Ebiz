<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="MemberCertifications.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.MemberCertifications" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessGlobal" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<div class="table-div">
    <div>
        <asp:Label ID="lblmsg" runat="server" CssClass="label" Text=""></asp:Label>
    </div>
    <div id="tblMain" class="row-div">
        <asp:UpdatePanel ID="upGridPanel" runat="server">
            <ContentTemplate>
                <%-- Anil B For issue 14344 on 28-03-2013
                    Set icon for sorting also set filtering --%>
                <rad:RadGrid ID="grdMembersCertifications" runat="server" AutoGenerateColumns="False"
                    SortingSettings-SortedDescToolTip="Sorted Descending" AllowSorting="True" Width="99%"
                    EnableViewState="true" AllowPaging="true" PageSize="5" PagerStyle-PageSizeLabelText="Records Per Page"
                    SortingSettings-SortedAscToolTip="Sorted Ascending" AllowFilteringByColumn="true">
                    <GroupingSettings CaseSensitive="false" />
                    <MasterTableView AllowFilteringByColumn="true" AllowNaturalSort="false" AllowMultiColumnSorting="false">
                        <Columns>
                            <rad:GridTemplateColumn HeaderText="Member" DataField="FirstLast" 
                                SortExpression="FirstLast" AutoPostBackOnFilter="true"
                                CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                <ItemStyle HorizontalAlign="Left" ></ItemStyle>
                                <ItemTemplate>
                                    <%-- Neha,issue 16001,5/07/13, added class for image heightwidth and allignment of Name,Title,Adderess --%>
                                    <div class="row-div clearfix">
                                        
                                            <rad:RadBinaryImage ID="imgmember" runat="server" AutoAdjustImageControlSize="false"
                                                ResizeMode="Fill" CssClass="img-float"></rad:RadBinaryImage>
                                        
                                        
                                            <asp:HyperLink ID="lblMember" CssClass="name-link" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"FirstLast") %>'
                                                NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"AdminEditprofileUrl") %>'></asp:HyperLink><br />
                                            <asp:Label ID="lblMemberTitle" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"title") %>'></asp:Label>
                                            <br />
                                            <asp:Label ID="lbladdress" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"address") %>'> </asp:Label>
                                        
                                    </div>
                                </ItemTemplate>
                            </rad:GridTemplateColumn>
                            <rad:GridTemplateColumn HeaderText="Certification Count" 
                                CurrentFilterFunction="EqualTo" DataField="TotalCirtification" SortExpression="TotalCirtification"
                                AutoPostBackOnFilter="true" ShowFilterIcon="false">
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblCount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"TotalCirtification") %>'></asp:Label>
                                </ItemTemplate>
                            </rad:GridTemplateColumn>
                            <rad:GridTemplateColumn HeaderText="Total Unit" DataField="UnitTotal" SortExpression="UnitTotal"
                                AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                                ShowFilterIcon="false" >
                                <ItemStyle HorizontalAlign="Left" ></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalUnit" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"UnitTotal") %>'></asp:Label>
                                </ItemTemplate>
                            </rad:GridTemplateColumn>
                            <rad:GridTemplateColumn HeaderText="Email" DataField="Email" SortExpression="Email"
                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                ShowFilterIcon="false" >
                                <ItemStyle HorizontalAlign="Left" ></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblDateGranted" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Email") %>'></asp:Label>
                                </ItemTemplate>
                            </rad:GridTemplateColumn>
                            <rad:GridTemplateColumn HeaderText="Submit New CEU" AllowFiltering="false">
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:HyperLink ID="hlnkNewCEU" CssClass="name-link" runat="server" Text="Submit New CEU"
                                        NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"CEUSubmissionPage") %>'></asp:HyperLink><br />
                                </ItemTemplate>
                            </rad:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </rad:RadGrid>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <cc1:User runat="server" ID="User1" />
</div>
