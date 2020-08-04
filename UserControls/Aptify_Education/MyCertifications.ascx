<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="MyCertifications.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.Education.MyCertificationsControl" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

    <div id="tblMain" runat="server" class="table-div clearfix">
         <div class="row-div clearfix">
            <div class="float-left w80">
                <asp:DropDownList runat="server" ID="cmbType" AutoPostBack="True" Font-Size="9pt">
                    <asp:ListItem Selected="True" Value="Granted">Granted</asp:ListItem>
                    <asp:ListItem Value="Cancelled">Cancelled</asp:ListItem>
                    <asp:ListItem Value="Expired">Expired</asp:ListItem>
                    <asp:ListItem>All</asp:ListItem>
                    <asp:ListItem>Declared</asp:ListItem>
                </asp:DropDownList>
             </div>
              <div class="float-right w18">
                <asp:HyperLink ID="hlnkSubmitNewCEU" runat="server" BorderColor="Transparent" Font-Bold="True"
                    Font-Size="14px" >Submit New CEU</asp:HyperLink>
               </div>
          </div>
      <div class="row-div clearfix">
                <asp:UpdatePanel ID="UppanelGrid" runat="server">
                    <ContentTemplate>
                         <rad:RadGrid ID="grdMyCertifications" runat="server" AutoGenerateColumns="False"
                            AllowPaging="true" AllowFilteringByColumn="true" SortingSettings-SortedDescToolTip="Sorted Descending"
                            SortingSettings-SortedAscToolTip="Sorted Ascending">
                            <GroupingSettings CaseSensitive="false" />
                            <MasterTableView AllowFilteringByColumn="true" AllowSorting="true" AllowNaturalSort="false">
                                <Columns>
                                    <rad:GridTemplateColumn HeaderText="Certification" DataField="Title" SortExpression="Title"
                                        FilterControlWidth="80%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="lnkTitle" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Title") %>'
                                                NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"CertificationUrl") %>'></asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="10pt" />
                                    </rad:GridTemplateColumn>
                                    <rad:GridTemplateColumn HeaderText="Type" DataField="Type" SortExpression="Type"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"
                                        FilterControlWidth="80%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Type") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="10pt" />
                                    </rad:GridTemplateColumn>
                                    <rad:GridDateTimeColumn UniqueName="GridDateTimeColumnStartDate" AllowSorting="true"
                                        Visible="True" HeaderText="Date Granted" DataField="DateGranted" SortExpression="DateGranted"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ReadOnly="true" ShowFilterIcon="false"
                                        DataType="System.DateTime" EnableTimeIndependentFiltering="true" ItemStyle-Width ="15%" FilterControlWidth="80%">
                                    </rad:GridDateTimeColumn>
                                    <rad:GridDateTimeColumn UniqueName="GridDateTimeColumnEndDate" AllowSorting="true"
                                        Visible="True" HeaderText="Expiration Date" DataField="ExpirationDate" SortExpression="ExpirationDate"
                                        ReadOnly="true" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false"
                                        DataType="System.DateTime" EnableTimeIndependentFiltering="true" ItemStyle-Width ="15%" FilterControlWidth="80%">
                                    </rad:GridDateTimeColumn>
                                    <rad:GridTemplateColumn HeaderText="Status" DataField="Status" SortExpression="Status"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"
                                        FilterControlWidth="80%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Status") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="10pt" />
                                    </rad:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </rad:RadGrid>
                    </ContentTemplate>
                </asp:UpdatePanel>
       </div>
   </div>
    <cc1:User runat="server" ID="User1" />
