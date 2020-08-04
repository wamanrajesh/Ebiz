<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="DownloadControl.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.CustomerService.DownloadControl" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="EBusinessShoppingCart" Namespace="Aptify.Framework.Web.eBusiness"
    TagPrefix="cc1" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<div class="table-div" id="divTop" runat="server">
    <script language="javascript" type="text/javascript">
        function GetClientUTC() {
            var now = new Date()
            var offset = now.getTimezoneOffset();
            document.getElementById('<%= hdOffset.ClientID%>').value = offset
        }
    </script>
    <asp:HiddenField ID="hdOffset" runat="server" />
    <div class="row-div ">
        <script language="javascript" type="text/javascript">
            GetClientUTC();
        </script>
        <div class="errormsg-div">
        <asp:Label ID="lblError" runat="server"></asp:Label></div>
    </div>
    <div class="row-div">
        <asp:Panel ID="pnlShowDownload" runat="server" Visible="true" >
            <rad:RadGrid ID="grdDownload" runat="server" AutoGenerateColumns="False" onrowcommand="grdDownload_RowCommand"
                AllowPaging="false">
                <MasterTableView>
                    <NoRecordsTemplate>
                        No Downloads Available.
                    </NoRecordsTemplate>
                    <Columns>
                        <rad:GridBoundColumn DataField="ProductName" HeaderText="Product" />
                        <rad:GridBoundColumn DataField="FileName" HeaderText="File Name" />
                        <rad:GridBoundColumn DataField="OrderDate" HeaderText="Order Date" DataFormatString="{0:MM/dd/yyyy}" />
                        <rad:GridTemplateColumn HeaderText="Order ID" DataField="OrderID">
                            <ItemTemplate>
                                <asp:HyperLink ID="hypOrderID" runat="server" Text='
										<%# Eval("OrderID") %>
											'>
                                </asp:HyperLink>
                            </ItemTemplate>
                        </rad:GridTemplateColumn>
                        <rad:GridTemplateColumn HeaderText="Download">
                            <ItemStyle/>
                            <ItemTemplate>
                                <asp:Button CssClass="submit-Btn" ID="btnDownload" runat="server" Text="Download"
                                    CommandArgument='
											<%# Eval("AttachmentID") & "," & Eval("ProductID") & "," & Eval("DownloadItemID") & "," & Eval("OrderID")%>
												' CommandName="Download" />
                                <asp:Label ID="lblDMessage" runat="server" Visible="false">
                                </asp:Label>
                            </ItemTemplate>
                        </rad:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </rad:RadGrid>
        </asp:Panel>
    </div>
 
</div>
<cc1:AptifyShoppingCart runat="Server" ID="ShoppingCart1" />
<cc1:User ID="User1" runat="server"></cc1:User>

