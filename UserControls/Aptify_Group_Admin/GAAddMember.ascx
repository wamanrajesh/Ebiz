<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="GAAddMember.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.CustomerService.GAAddMember" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<cc2:User ID="User1" runat="server" />
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<script language="javascript" type="text/javascript">
    function Hidelable() {
        window.document.getElementById('<%= lblMessage.ClientID %>').style.display = "none"

    }
</script>
<div class="align-right">
    <asp:Label ID="lblNote" runat="server" CssClass="note" Text="Fields marked with * are mandatory."></asp:Label>
</div>
<div >
    <asp:GridView ID="grdAddMember" AutoGenerateColumns="false" runat="server" ShowFooter="false"  SkinID="ForumsDataGrid"
        AllowPaging="false" CssClass="gridview-table">
      <HeaderStyle CssClass="grid-viewheader" />
        <FooterStyle CssClass="grid-footer" />
        <RowStyle CssClass="grid-item-style" />
        <PagerStyle CssClass="paging-style" />
        <Columns>
            <asp:BoundField DataField="RowNumber" HeaderText="Row Number" Visible="false" />
            <asp:TemplateField>
                <HeaderStyle />
                <ItemStyle />
                <HeaderTemplate>
                    <asp:Label ID="lblFname" Text="First Name" runat="server"></asp:Label>
                    <asp:Label ID="lblFnameAsterisk" Text="*" runat="server"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:TextBox ID="txtFName" runat="server"></asp:TextBox>
                </ItemTemplate>
                <%--<FooterStyle CssClass="GridFooter" />
            <FooterTemplate>
                <asp:LinkButton ID="lnkAddRow" runat="server" Text="Add Row" OnClick="lnkAddRow_Click" ForeColor="#d07b0c"></asp:LinkButton>
            </FooterTemplate>--%>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderStyle />
                <HeaderTemplate>
                    <asp:Label ID="lblLname" Text="Last Name" runat="server"></asp:Label>
                    <asp:Label ID="lblLnameAsterisk" Text="*" runat="server"></asp:Label>
                </HeaderTemplate>
                <ItemStyle />
                <ItemTemplate>
                    <asp:TextBox ID="txtLName" runat="server"></asp:TextBox>
                </ItemTemplate>
                <%--<FooterStyle CssClass="GridFooter" />
            <FooterTemplate>
                 <asp:Label ID="lblAddMultiple" runat="server" Text="Add Multiple Rows:" ForeColor="Black"></asp:Label>
                <asp:DropDownList ID="drpRows" runat="server">
                    <asp:ListItem Text="5" Value="5"></asp:ListItem>
                    <asp:ListItem Text="10" Value="10"></asp:ListItem>
                    <asp:ListItem Text="15" Value="15"></asp:ListItem>
                </asp:DropDownList>
            </FooterTemplate>--%>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderStyle />
                <HeaderTemplate>
                    <asp:Label ID="lblTitle" Text="Title" runat="server"></asp:Label>
                </HeaderTemplate>
                <ItemStyle />
                <ItemTemplate>
                    <asp:TextBox ID="txtTitle" runat="server"></asp:TextBox>
                </ItemTemplate>
                <%--<FooterStyle CssClass="GridFooter" />
            <FooterTemplate>
                 <asp:Button ID="btnInsertRows" runat="server" Text="Insert" OnClick="btnInsertRows_Click" />
            </FooterTemplate>--%>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderStyle />
                <HeaderTemplate>
                    <asp:Label ID="lblEmail" Text="Email" runat="server"></asp:Label>
                    <asp:Label ID="lblEmailAsterisk" Text="*" runat="server"></asp:Label>
                </HeaderTemplate>
                <ItemStyle />
                <ItemTemplate>
                    <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                </ItemTemplate>
                <FooterStyle />
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderStyle />
                <HeaderTemplate>
                    <asp:Label ID="lblCompany" Text="Company" runat="server"></asp:Label>
                </HeaderTemplate>
                <ItemStyle />
                <ItemTemplate>
                    <asp:Label ID="lblGACompany" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Company")%>'></asp:Label>
                </ItemTemplate>
                <%--<FooterStyle CssClass="GridFooter" />
            <FooterTemplate>
                <asp:Label ID="lblResult" runat="server" Text='' ForeColor="Red"></asp:Label>
            </FooterTemplate>--%>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderStyle />
                <HeaderTemplate>
                    <asp:Label ID="lblCreateWebUser" Text="Create Web User?" runat="server"></asp:Label>
                </HeaderTemplate>
                <ItemStyle />
                <ItemTemplate>
                    <asp:CheckBox ID="chkCreateWebUser" runat="server" Checked="true"></asp:CheckBox>
                </ItemTemplate>
                <%-- <FooterStyle CssClass="GridFooter" HorizontalAlign="Right" />
            <FooterTemplate>
                <asp:Button ID="btnDeleteAll" runat="server" Text="Delete All" OnClick="btnDeleteAll_Click" />
            </FooterTemplate>--%>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderStyle />
                <HeaderTemplate>
                    <asp:Label ID="lblDelete" Text="Delete" runat="server"></asp:Label>
                </HeaderTemplate>
                <ItemStyle />
                <ItemTemplate>
                    <asp:ImageButton ID="btndelete" runat="server" ImageUrl="~/Images/Delete.png" CommandName="Delete"
                        CommandArgument='<%#Eval("RowNumber") %>' />
                </ItemTemplate>
                <%--<FooterStyle CssClass="GridFooter" />
            <FooterTemplate>
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
            </FooterTemplate>--%>
            </asp:TemplateField>
        </Columns>
        <RowStyle />
    </asp:GridView>
</div>
<div class="row-div clearfix top-margin">
    <%--<asp:LinkButton ID="lnkAddRow" runat="server" Text="Add Row" OnClick="lnkAddRow_Click" ForeColor="#d07b0c"></asp:LinkButton>--%>
    <div>
        <asp:Label ID="lblAddMultiple" runat="server" Text="Add Rows:" CssClass="label"></asp:Label>
        <asp:DropDownList ID="drpRows" runat="server">
            <asp:ListItem Text="1" Value="1"></asp:ListItem>
            <asp:ListItem Text="2" Value="2"></asp:ListItem>
            <asp:ListItem Text="3" Value="3"></asp:ListItem>
            <asp:ListItem Text="4" Value="4"></asp:ListItem>
            <asp:ListItem Text="5" Value="5"></asp:ListItem>
            <asp:ListItem Text="10" Value="10"></asp:ListItem>
            <asp:ListItem Text="15" Value="15"></asp:ListItem>
        </asp:DropDownList>
        <asp:Button ID="btnInsertRows" runat="server" Text="Insert" OnClick="btnInsertRows_Click"
            CssClass="submit-Btn" />
        <asp:Label ID="lblResult" runat="server" Text='' class="note"></asp:Label>
        <asp:Button ID="btnDeleteAll" runat="server" Text="Delete All" OnClick="btnDeleteAll_Click"
            CssClass="submit-Btn" />
        <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click"
            CssClass="submit-Btn" />
    </div>
</div>
<%--  Aparna 14317 Excel upload--%>
<div class="top-margin bottom-margin label">
    <asp:Label runat="server" Text="OR"></asp:Label>
</div>
<div class="border-color padding-all clearfix ">
    <div class="float-left w33-3">
        <div class="label bottom-margin">
            <asp:Label ID="lblExceltemplate" Text="Upload Records using Excel Template" runat="server"></asp:Label>
        </div>
        <div class="float-left">
            <rad:RadBinaryImage ID="imgExcelTemplet" ImageUrl="~/Images/Excel_ICON.png" runat="server" />
        </div>
        <div class="float-left">
            <asp:LinkButton ID="Download" CssClass="name-link" runat="server" Text="Download Template"
                OnClick="Download_Click" />
        </div>
        <div class="clear">
        </div>
    </div>
    <div class="float-left w33-3">
        <div class="row-div label">
            <asp:Label ID="lblUpload" Text="Upload your Excel file" runat="server"></asp:Label>
        </div>

   
            <telerik:RadAsyncUpload ID="xlsUpload" Localization-Select="Browse..." runat="server"
                Localization-Remove="Remove" ControlObjectsVisibility="None" MaxFileInputsCount="1"
                OnClientFileSelected="Hidelable">
                <FileFilters>
                    <telerik:FileFilter Extensions="xls,xlsx" />
                </FileFilters>
            </telerik:RadAsyncUpload>
         <div class="float-left">
                <asp:Button ID="btnUpload" runat="server" Text="Upload" CssClass="submit-Btn" OnClick="btnUpload_Click" />
            </div>
  <div class="clear"></div>
    </div>
    <div class="float-left w33-3">
        <div class="label">
            <asp:Label ID="lblUploadedExcel" Visible="false" Text="Uploaded Excel file" runat="server"></asp:Label>
        </div>
        <div>
            <asp:LinkButton CssClass="name-link" ID="ExportExcel" runat="server" Text="AddPerson.xlsx"
                OnClick="ExportExcel_Click" Visible="false" />
            <rad:RadBinaryImage ID="radImgSmallExcel" ImageUrl="~/Images/Excel_ICO.png" runat="server"
                Visible="false" />
        </div>
        <div class="error-msg-label">
            <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
        </div>
    </div>
    <div class="clear"></div>
</div>
