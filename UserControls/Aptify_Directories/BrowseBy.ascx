<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="vb" AutoEventWireup="false" Inherits="Aptify.Framework.Web.eBusiness.Directories.BrowseByControl"
    CodeFile="BrowseBy.ascx.vb" Debug="true" %>
<%@ Register Src="PersonDirectoryGrid.ascx" TagName="PersonDirectoryGrid" TagPrefix="uc2" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessGlobal" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEbusinessUser" %>
<%@ Register TagPrefix="uc1" TagName="CompanyDirectoryGrid" Src="CompanyDirectoryGrid.ascx" %>
<div class="row-div">
    <div class="control-title">
        Browse By:
        <asp:Label runat="server" ID="lblBrowseBy">
        </asp:Label>
    </div>
</div>
<div class="row-div top-margin clearfix">
    <div class="left-container">
        <asp:ListBox ID="lstBrowse" runat="server" AutoPostBack="True" Rows="15" OnSelectedIndexChanged="lstBrowse_SelectedIndexChanged"></asp:ListBox>
    </div>
    <div class="float-right w80">
        <div class="label_underline">
            Results
        </div>
        <div class="row-div">
            <uc2:persondirectorygrid id="PersonDirectoryGrid" runat="server" />
            <uc1:companydirectorygrid id="CompanyDirectoryGrid" runat="server" />
        </div>
    </div>
</div>

