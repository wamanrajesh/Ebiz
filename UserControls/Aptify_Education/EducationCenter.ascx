<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="EducationCenter.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.Education.EducationCenter" %>
<%@ Register Src="InstructorValidator.ascx" TagName="InstructorValidator" TagPrefix="uc1" %>
<%@ Register Src="InstructorCenter.ascx" TagName="InstructorCenter" TagPrefix="uc1" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<div class="table-div">
    <div class="row-div clearfix">
        <div class="float-left right-margin w58">
            <img runat="server" id="imgMyCouses" alt="My Courses" src="" class="middle-img" />
            <asp:HyperLink runat="server" ID="lnkMyCourses" CssClass="label" Text="My Courses" />
            <br />
            View courses that you have registered for, are in process of completing, or have
            previously completed
            <br />
        </div>
        <div class="float-left w38">
            <img runat="server" id="imgMyCerts" alt="My Certifications" src="" class="middle-img"/>
            <asp:HyperLink runat="server" ID="lnkMyCerts" CssClass="label" Text="My Certifications" />
            <br />
            View current and past certifications that you have earned
            <br />
        </div>
    </div>
    <div class="row-div clearfix">
        <div class="float-left right-margin w58">
            <img runat="server" id="imgCatalog" alt="Course Catalog" src="" class="middle-img" />
            <asp:HyperLink runat="server" ID="lnkCatalog" CssClass="label"  Text="Course Catalog" />
            <br />
            Browse the course catalog to find the right educational opportunities for your needs
            <br />
        </div>
        <div class="float-left w38">
            <img runat="server" id="imgClassSchedule" alt="Class Schedule" src="" class="middle-img" />
            <asp:HyperLink runat="server" ID="lnkClassSchedule" CssClass="label" Text="Class Schedule" />
            <br />
            Browse a complete list of current and future scheduled classes
        </div>
    </div>
    <div class="row-div clearfix">
        <div class="float-left right-margin w58">
            <img runat="server" id="imgCurricula" alt="Curricula" src="" class="middle-img" />
            <asp:HyperLink runat="server" ID="lnkCurricula" CssClass="label" Text="Curricula" />
            <br />
            Browse the available Curricula provided by this institution and compare to your current course progress
            <br />
        </div>
    </div>
    <div id="trInstructorCenter" runat="server" class="row-div">        
        <uc1:instructorcenter runat="server" id="InstructorCenter" />        
    </div>
    <uc1:instructorvalidator id="InstructorValidator1" runat="server" />
</div>
