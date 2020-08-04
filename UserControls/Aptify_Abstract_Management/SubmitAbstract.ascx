<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="vb" AutoEventWireup="false" Inherits="Aptify.Framework.Web.eBusiness.AbstractManagement.SubmitAbstract"
    CodeFile="SubmitAbstract.ascx.vb" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessGlobal" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>

<div class="table-div abstract-container-div" runat="server" id="tblMain">
    <div class="row-div clearfix">
        <%-- aparna Issue 8271 for add fontname and size for textbox--%>
        <div class="label-div w19">
            Subject</div>
        <div class="field-div1 w80">
            <asp:TextBox ID="txtSubject" runat="server" AptifyDataField="Subject"></asp:TextBox></div>
    </div>
    <div class="row-div clearfix">
        <%-- aparna Issue 8271 for add fontname and size for textbox--%>
        <div class="label-div w19">
            Title</div>
        <div class="field-div1 w80">
            <asp:TextBox ID="txtTitle" runat="server" TextMode="MultiLine" CssClass="txt-restrict-resize" AptifyDataField="Title"></asp:TextBox></div>
    </div>
    <div class="row-div clearfix">
        <%-- aparna Issue 8271 for add fontname and size for textbox--%>
        <div class="label-div w19">
            Category</div>
        <div class="field-div1 w80">
            <asp:DropDownList ID="cmbCategory" runat="server" AptifyDataField="CategoryID" AptifyListTextField="Name"
                AptifyListValueField="ID" AptifyListSQL="">
            </asp:DropDownList>
        </div>
    </div>
    <div class="row-div clearfix">
        <%-- aparna Issue 8271 for add fontname and size for textbox--%>
        <div class="label-div w19">
            Summary</div>
        <div class="field-div1 w80">
            <asp:TextBox ID="txtSummary" runat="server" TextMode="MultiLine" CssClass="txt-restrict-resize" AptifyDataField="Summary"></asp:TextBox></div>
    </div>
    <div class="row-div clearfix">
        <%-- aparna Issue 8271 for add fontname and size for textbox--%>
        <div class="label-div w19">
            Body</div>
        <div class="field-div1 w80">
            <asp:TextBox ID="txtBody" runat="server" TextMode="MultiLine" CssClass="txt-restrict-resize" AptifyDataField="Body"></asp:TextBox></div>
    </div>
    <div class="row-div clearfix">
        <%-- Nalini Issue 12734--%>
        <div class="label-div w19">
            &nbsp;</div>
        <div>
            <asp:Button ID="cmdSubmit" runat="server" Text="Submit Abstract" CssClass="submit-btn">
            </asp:Button>
        </div>
    </div>
</div>
<asp:Label ID="lblMessage" runat="server" Visible="False" />
<cc3:User ID="AptifyEbusinessUser1" runat="server" />
