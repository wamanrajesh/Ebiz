<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="vb" AutoEventWireup="false" Inherits="Aptify.Framework.Web.eBusiness.PersonListing" CodeFile="PersonListing.ascx.vb"  %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessGlobal" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEbusinessUser" %>
<div class="row-div ">
    <asp:Label ID="lblResult" runat="server">
    </asp:Label>
</div>
<div class="row-div clearfix">
    <div class="label-div w19">
        <asp:Label ID="lblName" runat="server" Text="Name:"/>
    </div>
    <div class="field-div1 w79">
        <asp:Label ID="lblPersonName" runat="server">
        </asp:Label>
        <br />
    </div>
</div>
<div class="row-div clearfix">
    <div class="label-div w19">
        <asp:Label ID="lblCompany" runat="server" Text="Company:"/>
    </div>
    <div class="field-div1 w79">
        <asp:Label ID="lblCompanyName" runat="server">
        </asp:Label>
    </div>
</div>
<div class="row-div clearfix">
    <div class="label-div w19">
        <asp:Label ID="lblTitle" runat="server" Text="Title:" />
    </div>
    <div class="field-div1 w79">
        <asp:Label ID="lblPersonTitle" runat="server">
        </asp:Label>
    </div>
</div>
<div class="row-div clearfix">
    <div class="label-div w19">
        <asp:Label ID="lblPersonAddress" runat="server" Text="Address:"/>
    </div>
    <div class="field-div1 w79">
        <asp:Label ID="lblAddress" runat="server">
        </asp:Label>
    </div>
</div>
<div class="row-div clearfix">
    <div class="label-div w19">
        <asp:Label ID="lblPersonEmail" runat="server" Text="Email:"/>
    </div>
    <div class="field-div1 w79">
        <asp:Label ID="lblEmail" runat="server">
        </asp:Label>
    </div>
</div>
<div class="row-div clearfix">
    <div class="label-div w19">
        <asp:Label ID="lblPersonPhone" runat="server" Text="Phone:"/>
    </div>
    <div class="field-div1 w79">
        <asp:Label ID="lblPhone" runat="server">
        </asp:Label>
    </div>
</div>
<div class="row-div clearfix">
    <div class="label-div w19">
        <asp:Label ID="lblPersonFax" runat="server" Text="Fax:"/>
    </div>
    <div class="field-div1 w79">
        <asp:Label ID="lblFax" runat="server">
        </asp:Label>
    </div>
</div>