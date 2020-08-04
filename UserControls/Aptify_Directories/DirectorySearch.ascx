<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="vb" AutoEventWireup="false" Inherits="Aptify.Framework.Web.eBusiness.Directories.SearchControl"
    CodeFile="DirectorySearch.ascx.vb" EnableViewState="true" ViewStateMode="Enabled" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessGlobal" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEbusinessUser" %>
<%@ Register TagPrefix="uc1" TagName="CompanyDirectoryGrid" Src="CompanyDirectoryGrid.ascx" %>
<%@ Register TagPrefix="uc1" TagName="PersonDirectoryGrid" Src="PersonDirectoryGrid.ascx" %>
<div class="directories-main-div">
    <asp:Panel ID="pnlsearch" runat="server" DefaultButton="cmdSearch">
        <div class="control-title">
            <asp:Label ID="lblHeader" runat="server" Text="Search Directory Page">
            </asp:Label>
        </div>
        <div class="row-div top-margin clearfix">
            <div class="label-div w18">
                Search by Name:
            </div>
            <div class="field-div1 w80">
                <div class="float-left w30">
                    <asp:TextBox ID="txtSearch" runat="server" EnableViewState="true">
                    </asp:TextBox></div>
                <div class="float-left w50 clearfix">
                    <asp:Button ID="cmdSearch" runat="server" Text="Search" CssClass="submit-Btn" OnClientClick="GetSearch()"
                        OnClick="cmdSearch_Click"></asp:Button></div>
            </div>
        </div>
        <div class="row-div clearfix">
            <uc1:companydirectorygrid id="CompanyDirectoryGrid" runat="server">
                    </uc1:companydirectorygrid>
        </div>
        <div class="row-div clearfix">
            <uc1:persondirectorygrid id="PersonDirectoryGrid" runat="server">
                        </uc1:persondirectorygrid>
        </div>
    </asp:Panel>
</div>
