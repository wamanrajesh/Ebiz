<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="InstructorCenter.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.Education.InstructorCenterControl" %>
<%@ Register Src="InstructorValidator.ascx" TagName="InstructorValidator" TagPrefix="uc1" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>


<div class="table-div" id="tblMain" runat="server">
	<div class="row-div label header-title-Head-border">	    
		    Instructor Center		
	</div>
	<div class="row-div">
		<asp:HyperLink runat="server" ID="lnkInstructorClasses" CssClass="label" Text="My Class List" />
	</div>
	<div class="row-div">
		<asp:HyperLink runat="server" ID="lnkInstructorStudents" CssClass="label" Text="My Student List" />
	</div>
	<div class="row-div">
    	<asp:HyperLink runat="server" ID="lnkInstructorAuthCourses" CssClass="label" Text="Authorized Courses" />
	</div>
</div>
<uc1:InstructorValidator ID="InstructorValidator1" runat="server" />
