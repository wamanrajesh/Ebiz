<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Curriculum.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.Education.CurriculumControl" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessLogin" %>
<div class="row-div">
    <asp:Label ID="lblError" runat="server" Text="Error" CssClass="error-msg-label" Visible="False"></asp:Label>
</div>
<div id="tblMain" runat="server">
    <div class="row-div clearfix">
        Select from the lists below and click "Show Curriculum" to view your status against
        the requirements.
    </div>
    <div class="row-div clearfix">
        <div class="field-div1 padding-left">
            <asp:DropDownList runat="server" ID="cmbCategory" AutoPostBack="true" ToolTip="Select a category from this list to filter the course catalog" />
        </div>
        <div class="field-div1 padding-left">
            <asp:DropDownList runat="server" ID="cmbCurriculum" AutoPostBack="false" ToolTip="Select a Curriculum from this list to display the course requirements" />
        </div>
        <div class="field-div1 padding-left">
            <asp:Button ID="btnLoadCurriculum" runat="server" Text="Show Curriculum" CssClass="submit-Btn" />
        </div>
    </div>
      <div class="row-div w80 clearfix">
            <asp:Table ID="tblCurriculum" runat="server" >
            </asp:Table>
        </div>
</div>
<cc3:aptifywebuserlogin id="WebUserLogin1" runat="server" visible="False"></cc3:aptifywebuserlogin>
