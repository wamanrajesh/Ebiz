<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="vb" AutoEventWireup="false" Inherits="Aptify.Framework.Web.eBusiness.Directories.CompanyListingControl" CodeFile="CompanyListing.ascx.vb" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessGlobal" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEbusinessUser" %>
<div class="row-div-bottom-line" id="tblMain" runat="server">
        <div class="control-title">
            <asp:Label ID="lblCompanyName" runat="server">
				COMPANY NAME
            </asp:Label>
        </div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w19">
            Address:
        </div>
        <div class="field-div1 w79">
            <asp:Label ID="lblAddress" runat="server">
            </asp:Label>
        </div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w19">
            Main Email:
        </div>
        <div class="field-div1 w79">
            <asp:Label ID="lblMainEmail" runat="server">
            </asp:Label>
        </div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w19">
            Info Email:
        </div>
        <div class="field-div1 w79">
            <asp:Label ID="lblInfoEmail" runat="server">
            </asp:Label>
        </div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w19">
            Jobs Email:
        </div>
        <div class="field-div1 w79">
            <asp:Label ID="lblJobsEmail" runat="server">
            </asp:Label>
        </div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w19">
            Phone:
        </div>
        <div class="field-div1 w79">
            <asp:Label ID="lblPhone" runat="server">
            </asp:Label>
        </div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w19">
            Fax:
        </div>
        <div class="field-div1 w79">
            <asp:Label ID="lblFax" runat="server">
            </asp:Label>
        </div>
    </div>

<div class="row-div clearfix">
    <div class="label-div w19">
        &nbsp;
    </div>
    <div class="errormsg-div w79 clearfix">
        <asp:Label ID="lblError" runat="server" Text="No record available" Visible="false"></asp:Label></div>
</div>
