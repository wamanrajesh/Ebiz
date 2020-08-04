<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ChapterReportViewer.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.Chapters.ChapterReportViewerControl" Debug = "true" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="cc4" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessHierarchyTree" %>
<div class="table-div" id="tblMain" runat="server">
	<div class="row-div">
		<asp:label id="lblTitle" runat="server">
	    </asp:label>
	</div>
	<div class="errormsg-div clearfix">
    	<asp:Label ID="lblError" runat="server">
		</asp:Label>
    </div>
</div>
<asp:linkbutton id="lnkChapter"  CssClass="lnk-chapter-report-viewer" Runat="server">Go To Chapter</asp:linkbutton>
<asp:linkbutton id="lnkReports"  CssClass="lnk-chapter-report-viewer" Runat="server">Chapter Reports</asp:linkbutton>
<asp:Panel ID="pnl" runat="server" CssClass="row-div" ScrollBars="Both"  >
    <CR:crystalreportviewer id="rptViewerMain" runat="server"  AutoDataBind="True" />
        </asp:Panel>
    <cc3:User id="User1" runat="server" />