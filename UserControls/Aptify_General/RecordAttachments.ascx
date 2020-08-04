<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="RecordAttachments.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.RecordAttachments" Debug="true" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessGlobal" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<cc2:User runat="server" ID="User1" />
<style>
    .RadButton_Sunset.rbSkinnedButton, .RadButton_Sunset .rbDecorated, .RadButton_Sunset.rbVerticalButton, .RadButton_Sunset.rbVerticalButton .rbDecorated, .RadButton_Sunset .rbSplitRight, .RadButton_Sunset .rbSplitLeft
    {
        background-image: none !important;
    }
</style>
<asp:Label ID="lblError" runat="server" Visible="False" CssClass="error-msg-label"></asp:Label>
<script type="text/javascript">
    function _do_window_open(url) {
        window.open(url, '_aptify_attachment_content', 'toolbar=no,menubar=yes,location=no,directories=no,status=no,resizable=yes,scrollbars=yes');
    }
</script>
<div class="table-div">
    <div runat="server" id="trGrid" class="row-div clearfix">
            <%-- Navin Prasad Issue 11032--%>
            <%--Nalini Issue 12436 date:01/12/2011--%>
            <%-- Navin Prasad Issue 12865 --%>
             <div class="row-div">
            <asp:UpdatePanel ID="UppanelGrid" runat="server">
                <ContentTemplate>
               <%-- 'Anil B for issues 144499 on 05-04-2013
                        Remove Sorting--%>
                    <rad:RadGrid ID="grdAttachments" runat="server" AutoGenerateColumns="false" AllowPaging="true"
                        AllowSorting="False" AllowFilteringByColumn="false">
                        <MasterTableView>
                            <Columns>
                                <rad:GridTemplateColumn Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                    </ItemTemplate>
                                </rad:GridTemplateColumn>
                                <rad:GridTemplateColumn>
                                    <ItemTemplate>
                                        <asp:HyperLink ID="lblFileImage" runat="server" NavigateUrl='<%#  Bind("EncryptedURL")%>'></asp:HyperLink>
                                    </ItemTemplate>
                                    <ItemStyle Wrap="true" />
                                </rad:GridTemplateColumn>
                                <rad:GridTemplateColumn HeaderText="File">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="lblFile" runat="server" Text='<%# Eval("Name") %>' NavigateUrl='<%#  Bind("EncryptedURL")%>'></asp:HyperLink>
                                    </ItemTemplate>
                                    <ItemStyle Wrap="true" />
                                </rad:GridTemplateColumn>
                                <rad:GridBoundColumn DataField="Description" AllowSorting="false" ItemStyle-Wrap="true"
                                    HeaderText="Description" />
                                <rad:GridBoundColumn DataField="DateUpdated" AllowSorting="false" HeaderText="Updated On"
                                    DataFormatString="{0:d}" />
                                <rad:GridBoundColumn DataField="FileSize" AllowSorting="false" HeaderText="Size"
                                    HeaderStyle-HorizontalAlign="right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0} KB" />
                                <rad:GridTemplateColumn ItemStyle-CssClass="grdAttachRecordAlign">
                                    <ItemTemplate>
                                        <rad:RadButton CssClass="submit-Btn grdAttachRecordAlign" ID="btn" runat="server"
                                            Text="Delete" CommandName="Delete" CommandArgument='<%# CType(Container, GridDataItem).ItemIndex %>'>
                                        </rad:RadButton>
                                    </ItemTemplate>
                                </rad:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </rad:RadGrid>
                    
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="grdAttachments" />
                </Triggers>
            </asp:UpdatePanel>
            </div>
       </div>
    <div runat="server" id="trAdd" visible="false" class="row-div clearfix">
        <div class="row-div">
            To upload a file, fill in the information shown below
            </div>
                <div class="row-div clearfix">
                    <div class="label-div w18">
                        File:
                    </div>
                    <div class="field-div1 w79">
                        <asp:FileUpload ID="FileUpload1" runat="server" />
                    </div>
               </div>
                <div class="row-div clearfix">
                  <div class="label-div w18">
                        Description:
                    </div>
                    <div class="field-div1 w79">
                        <asp:TextBox ID="txtDescription" runat="server" />
                    </div>
                </div>
                 <div class="row-div clearfix">
                     <div class="label-div emptyspace w18">
                        &nbsp;
                    </div>
                     <div class="field-div1 w79">
                     <asp:Button CssClass="submit-Btn" runat="server" ID="btnAdd" Text="Upload" />
                      </div>
                </div>
            </div>
       </div>