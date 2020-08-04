<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ViewCertification.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.Education.ViewCertificationControl" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>

<div id="tblMain" runat="server" class="table-div clearfix">
    <div class="errormsg-div">
    <asp:Label runat="server" ID="lblError" Visible="false"></asp:Label>
    </div>
    
<div class="table-div" runat="server" id="TABLE1" onclick="return TABLE1_onclick()">
	<div class="row-div ">
		<asp:Label runat="server" ID="lblTitle" Font-Size="16pt" />
	</div>
	<div class="row-div clearfix ">
		<div class="label-div w19">
			 Certificate #:
		</div>
		<div class="field-div1 w79">
			<asp:Label runat="server" ID="lblID" />
		</div>
	</div>
	<div class="row-div clearfix ">
		<div class="label-div w19">
			 Certificant:
		</div>
		<div class="field-div1 w79">
			<asp:Label runat="server" ID="lblCertificant" />
		</div>
	</div>
	<div class="row-div clearfix ">
		<div class="label-div w19">
			 Certification Type:
		</div>
		<div class="field-div w79">
			<asp:Label runat="server" ID="lblType" />
		</div>
        <div class="label-div w19">&nbsp;</div>
        <div class="field-div1 w79">
			<asp:HyperLink  ID="lnkType" runat="server">
			<asp:Label runat="server" ID="lblTypeDetails" />
            </asp:HyperLink>
			
		</div>
	</div>
	<div class="row-div clearfix">
		<div class="label-div w19">
			 Granted On:
		</div>
		<div class="field-div1 w79">
			<asp:Label runat="server" ID="lblDateGranted" />
		</div>
	</div>
	<div class="row-div clearfix ">
		<div class="label-div w19">
			 Expires On:
		</div>
		<div class="field-div1 w79">
			<asp:Label runat="server" ID="lblDateExpires" />
		</div>
	</div>
	<div class="row-div clearfix ">
		<div class="label-div w19">
			 Status:
		</div>
		<div class="field-div1 w79">
			<asp:Label runat="server" ID="lblStatus" />
		</div>
	</div>
</div>
 </div>
<cc1:User runat="server" ID="User1" />

