<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="MemberCertificationDetails.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.MemberCertificationDetails" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessGlobal" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<script language="javascript" type="text/javascript">
    // 'Anil B For issue 14344 on 17-04-2013
    //            'Open print report on a new tab
    function openNewWin(url) {

        var x = window.open(url);

        x.focus();

    }
</script>
<div class="table-div">
    <div class="row-div clearfix">
        <div class="float-left w49">
            <div class="row-div clearfix">
                <div class="label-div w25">
                    <asp:Label ID="lblName" runat="server" Text="Name:"></asp:Label>
                </div>
                <div class="field-div1 w74">
                    <asp:Label ID="lblMemberName" runat="server" Text=""></asp:Label>
                </div>
            </div>
            <div class="row-div clearfix">
                <div class="label-div w25">
                    <asp:Label ID="lblTitle" runat="server" Text="Title:"></asp:Label>
                </div>
                <div class="field-div1 w74">
                    <asp:Label ID="lblMemberTitle" runat="server" Text=""></asp:Label>
                </div>
            </div>
            <div class="row-div clearfix">
                <div class="label-div w25">
                    <asp:Label ID="lblCompany" runat="server" Text="Company:"></asp:Label>
                </div>
                <div class="field-div1 w74">
                    <asp:Label ID="lblMemberCompany" runat="server" Text=""></asp:Label>
                </div>
            </div>
            <div class="row-div clearfix">
                <div class="label-div w25">
                    <asp:Label ID="lblTotalUnitCount" runat="server" Text="Total Units:"></asp:Label>
                </div>
                <div class="field-div1 w74">
                    <asp:Label ID="lblMemberTotalUnitCount" runat="server" Text=""></asp:Label>
                </div>
            </div>
            <div id="trSearch" runat="server">
            <div class="row-div clearfix">
                <div class="label-div w25">
                    Granted On:
                </div>
                <div class="field-div1 w74">
                    <telerik:raddatepicker id="txtStartDate" runat="server">
            </telerik:raddatepicker>
                </div>
                </div>
                <div class="row-div clearfix">
                <div class="label-div w25">
                    Expires On:</div>
                <div class="field-div1 w74">
                    <telerik:raddatepicker id="txtExpiresOn" runat="server">
            </telerik:raddatepicker>
                </div>
                </div>
                <div class="row-div clearfix">
                <div class="label-div emptyspace w25">&nbsp;</div>
                <div class="field-div1 w74">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="submit-Btn" UseSubmitBehavior="false" />
                </div>
                </div>
            </div>
        </div>
        <div class="float-left w49">
            <div class="float-left">
                <asp:Button ID="btnNewCEUSubmission" UseSubmitBehavior="false" runat="server" Text="Submit New CEU"
                    CssClass="submit-Btn" />
                <asp:Button ID="btnPrint" runat="server" UseSubmitBehavior="false" Text="Print Report"
                    CssClass="submit-Btn" TabIndex="10000" />
            </div>
        </div>
    </div>
    <div class="row-div clearfix">
        <asp:Label ID="lblmsg" runat="server" Text=""></asp:Label>
    </div>
    <asp:UpdatePanel ID="upGridPanel" runat="server">
        <ContentTemplate>
            <div class="table-div" id="tblMain" runat="server">
                <div class="row-div">
                    <asp:Label ID="lblActiveCirtification" runat="server" CssClass="grd-title" Text="Active Certifications:"></asp:Label>
                </div>
                <div class="row-div">
                    <rad:radgrid id="grdMembersActiveCertifications" runat="server" autogeneratecolumns="False"
                        allowpaging="true" pagerstyle-pagesizelabeltext="Records Per Page" allowfilteringbycolumn="true">
                            <GroupingSettings CaseSensitive="false" />
                            <MasterTableView AllowFilteringByColumn="true" AllowSorting="true">
                                <Columns>
                                    <rad:GridTemplateColumn HeaderText="Certification" DataField="Title" SortExpression="Title"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false">
                                        <ItemStyle></ItemStyle>
                                        <ItemTemplate>
                                            <asp:HyperLink ID="lblCertification" CssClass="name-link" runat="server"
                                                Text='<%# DataBinder.Eval(Container.DataItem,"Title") %>'></asp:HyperLink><br />
                                        </ItemTemplate>
                                    </rad:GridTemplateColumn>
                                    <rad:GridTemplateColumn HeaderText="Requirement" DataField="Course" SortExpression="Course"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                        <ItemStyle></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblRequirement" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Course") %>'></asp:Label>
                                        </ItemTemplate>
                                    </rad:GridTemplateColumn>
                                    <rad:GridTemplateColumn HeaderText="Units" DataField="Units" SortExpression="Units"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false">
                                        <ItemStyle></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblUnits" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Units") %>'></asp:Label>
                                        </ItemTemplate>
                                    </rad:GridTemplateColumn>
                                    <rad:GridTemplateColumn HeaderText="Status" DataField="Status" SortExpression="Status"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                        <ItemStyle></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Status") %>'></asp:Label>
                                        </ItemTemplate>
                                    </rad:GridTemplateColumn>
                                    <rad:GridDateTimeColumn DataField="DateGranted" UniqueName="GridDateTimeColumnACertificationGranted"
                                        AllowFiltering="false" HeaderText="Granted On" SortExpression="DateGranted" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                                        DataType="System.DateTime" ShowFilterIcon="false" EnableTimeIndependentFiltering="true" />
                                    <rad:GridDateTimeColumn DataField="ExpirationDate" UniqueName="GridDateTimeColumnACertificationExpiration"
                                        AllowFiltering="false" HeaderText="Expires On" SortExpression="ExpirationDate" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                                        ShowFilterIcon="false" EnableTimeIndependentFiltering="true" />
                                </Columns>
                            </MasterTableView>
                        </rad:radgrid>
                </div>
                <div class="row-div">
                    <asp:Label ID="lblDeActiveCirtification" runat="server" CssClass="grd-title" Text="Inactive Certifications:"></asp:Label>
                </div>
                <div class="row-div">
                    <rad:radgrid id="grdMembersDEeActiveCertifications" runat="server" autogeneratecolumns="False"
                        allowpaging="true" pagerstyle-pagesizelabeltext="Records Per Page" allowfilteringbycolumn="true">
                            <GroupingSettings CaseSensitive="false" />
                            <MasterTableView AllowFilteringByColumn="true" AllowSorting="true">
                                <Columns>
                                    <rad:GridTemplateColumn HeaderText="Certification" DataField="Title" SortExpression="Title"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false">
                                        <ItemStyle></ItemStyle>
                                        <ItemTemplate>
                                            <asp:HyperLink ID="lblCertification" CssClass="name-link" runat="server"
                                                Text='<%# DataBinder.Eval(Container.DataItem,"Title") %>'></asp:HyperLink><br />
                                        </ItemTemplate>
                                    </rad:GridTemplateColumn>
                                    <rad:GridTemplateColumn HeaderText="Requirement" DataField="Course" SortExpression="Course"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                        <ItemStyle></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblRequirement" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Course") %>'></asp:Label>
                                        </ItemTemplate>
                                    </rad:GridTemplateColumn>
                                    <rad:GridTemplateColumn HeaderText="Units" DataField="Units" SortExpression="Units"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false">
                                        <ItemStyle></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblUnits" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Units") %>'></asp:Label>
                                        </ItemTemplate>
                                    </rad:GridTemplateColumn>
                                    <rad:GridTemplateColumn HeaderText="Status" DataField="Status" SortExpression="Status"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                        <ItemStyle></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Status") %>'></asp:Label>
                                        </ItemTemplate>
                                    </rad:GridTemplateColumn>
                                  
                                    <rad:GridDateTimeColumn DataField="DateGranted" UniqueName="GridDateTimeColumnDACertificationGranted"
                                        HeaderText="Granted On" SortExpression="DateGranted" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                                        DataType="System.DateTime" AllowFiltering="false" ShowFilterIcon="false" EnableTimeIndependentFiltering="true" />
                                    <rad:GridDateTimeColumn DataField="ExpirationDate" UniqueName="GridDateTimeColumnDACertificationExpiration"
                                        HeaderText="Expires On" SortExpression="ExpirationDate" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                                        DataType="System.DateTime" AllowFiltering="false" ShowFilterIcon="false" EnableTimeIndependentFiltering="true" />
                                </Columns>
                            </MasterTableView>
                        </rad:radgrid>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <cc1:user runat="server" id="User1" />
</div>
