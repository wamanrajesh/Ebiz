<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" Inherits="Aptify.Framework.Web.eBusiness.AbstractManagement.ReviewSingleAbstract"
    CodeFile="ReviewSingleAbstract.ascx.vb" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EbusinessGlobal" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEbusinessUser" %>
<div class="table-div" runat="server">
    <div class="errormsg-div">
        <asp:Label ID="lblError" runat="server" Text="Label" Visible="False"></asp:Label></div>
    <div class="row-div clearfix">
        <div class="label-div w19">
            Subject
        </div>
        <div class="field-div1 w80">
            <asp:Label ID="lblSubject" runat="server" AptifyDataField="Subject" />
        </div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w19">
            Submitted
        </div>
        <div class="field-div1 w80">
            <asp:Label ID="lblSubmittedBy" runat="server" AptifyDataField="SubmittedBy" />
        </div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w19">
            Date
        </div>
        <div class="field-div1 w80">
            <asp:Label ID="lblDateSubmitted" runat="server" AptifyDataField="DateSubmitted" />
        </div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w19">
            Title
        </div>
        <div class="field-div1 w80">
            <asp:Label ID="lblTitle" runat="server" AptifyDataField="Title" />
        </div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w19">
            Category
        </div>
        <div class="field-div1 w80">
            <asp:Label ID="lblCategory" runat="server" AptifyDataField="Category" />
        </div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w19">
            Summary
        </div>
        <div class="field-div1 w80">
            <asp:Label ID="lblSummary" runat="server" AptifyDataField="Summary" />
        </div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w19">
            Body
        </div>
        <div class="field-div1 w80">
            <asp:Label ID="lblBody" runat="server" AptifyDataField="Body" />
        </div>
    </div>
</div>
<cc3:User ID="AptifyEbusinessUser1" runat="server"></cc3:User>
