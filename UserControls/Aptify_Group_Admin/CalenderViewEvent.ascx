<%--Aptify e-Business 5.5.1, July 2013--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="CalenderViewEvent.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.CustomerService.CalenderViewEvent" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<style type="text/css">
    #content ul
    {
        font-size: 11px;
        padding: -1px 50px 10px 35px !important;
    }
    
    #ctl00_MainContentAreaPlaceHolder_CalenderViewEvent_EventCalenderScheduler table
    {
        float: left !important;
    }
    .rsAptContent
    {
        /*background-color: #be9b6c !important; /* border:1px solid #856742 !important;*/
    }
    .rsAptIncustom
    {
        /*background-color: #be9b6c !important; /*border:1px solid #856742 !important;
        background-image:none !important;*/
    }
    #content ul li
    {
        padding: 0px 0px -1px !important;
    }
    .RadScheduler .rsAptOut > .rsAptMid, .RadScheduler .rsAptMid > .rsAptIn, .RadScheduler .rsAptIn > .rsAptContent
    {
        cursor: pointer !important;
    }
    #qsfexAdvEditWrapper
    {
        /*  background: #EEE none repeat scroll 0 0;
        border: 1px solid #666666;*/
        padding: 20px;
        height: 90px !important;
    }
    #qsfexAdvEditInnerWrapper
    {
        /*border: 1px solid #777;*/
        padding-top: 5px;
    }
    .RadScheduler div.rsApt a.rsAptDelete
    {
        background-image: url('Images/delete.gif');
        background-position: 0;
        right: 4px;
        top: 4px;
        width: 16px;
        z-index: 52;
    }
    
    .RadScheduler .rsApt .rsAptDelete:hover
    {
        background-image: url('Images/delete_hover.gif') !important;
    }
    
    .RadScheduler .rsCustomAppointmentContainerInner
    {
        display: block;
        padding: 5px 0 0 5px;
    }
    
    #qsfexAdvEditWrapper *, .RadScheduler .rsCustomAppointmentContainerInner *
    {
        position: relative;
        z-index: 2;
    }
    
    .rsCustomAppointmentContainer
    {
        width: 100% !important;
        height: 100% !important;
    }
    
    .rsCustomAppointmentContainer div, #qsfexAdvEditWrapper .qsfexAdvAppType
    {
        position: absolute;
        width: 187px;
        height: 100%;
        border-top: 1px solid transparent;
        top: 1px;
        right: -2px;
        z-index: 1;
    }
    
    #qsfexAdvEditWrapper .qsfexAdvAppType, .rsTemplateWrapper .rsCustomAppointmentContainer div
    {
        top: -1px;
        right: 0;
    }
    
    .rsDayView .rsCustomAppointmentContainer h2
    {
        position: relative;
        z-index: 50;
        font: bold 15px Arial, sans-serif; /*padding: 15px;*/
    }
    
    .qsfexAdvEditControlWrapper textarea
    {
        border: 1px solid #777;
        font: 11px arial,sans-serif;
        margin: 5px 8px;
        opacity: 0.8;
        -moz-opacity: 0.8;
        filter: alpha(opacity=80);
    }
    
    .qsfexAdvEditControlWrapper
    {
        width: 100%;
        padding-bottom: 5px;
    }
    
    #InlineInsertTemplate textarea, #InlineEditTemplate textarea
    {
        border: 1px solid #AFB4C5;
        font: 11px Arial, sans-serif;
        line-height: 20px;
        float: left;
    }
    
    .RadScheduler .qsfexAdvEditControlWrapper textarea
    {
        line-height: 20px;
    }
    
    #InlineInsertTemplate img, #InlineEditTemplate img
    {
        padding: 0 0 0 6px;
        width: 22px;
        height: 22px;
    }
    
    .rsWeekView .rsCustomAppointmentContainer h2, .rsMonthView .rsCustomAppointmentContainer h2
    {
        margin: 2px;
        padding: 0;
        font: normal 11px Arial, sans-serif;
    }
    
    .RadScheduler div.rsAptContent
    {
        overflow: visible;
    }
    
    .rsAptContent .technical h2
    {
        color: #486309;
    }
    
    .rsDayView .rsAptContent .technical div, .rsDayView .rsAptEditFormWrapper .technical div, #qsfexAdvEditWrapper .technical .qsfexAdvAppType
    {
        background: url('Images/technical.png') no-repeat top right;
    }
    
    #qsfexAdvEditWrapper div.technical, .rsAptType_technical
    {
        background: #D0ECBB;
    }
    
    * html .rsDayView .rsAptContent .technical div, * html .rsDayView .rsAptEditFormWrapper .technical div, * html #qsfexAdvEditWrapper .technical .qsfexAdvAppType
    {
        filter: progid:DXImageTransform.Microsoft.AlphaImageLoader (src= 'Images/technical.png' , sizingMethod= 'crop' );
    }
    
    .rsAptContent .code_review h2
    {
        color: #375970;
    }
    
    .rsDayView .rsAptContent .code_review div, .rsDayView .rsAptEditFormWrapper .code_review div, #qsfexAdvEditWrapper .code_review .qsfexAdvAppType
    {
        background: url('Images/code_review.png') no-repeat top right;
    }
    
    #qsfexAdvEditWrapper div.code_review, .rsAptType_code_review
    {
        background: #BBD0EC;
    }
    
    * html .rsDayView .rsAptContent .code_review div, * html .rsDayView .rsAptEditFormWrapper .code_review div, * html #qsfexAdvEditWrapper .code_review .qsfexAdvAppType
    {
        filter: progid:DXImageTransform.Microsoft.AlphaImageLoader (src= 'Images/code_review.png' , sizingMethod= 'crop' );
    }
    
    .rsAptContent .specification_review h2
    {
        color: #744b24;
    }
    
    .rsDayView .rsAptContent .specification_review div, .rsDayView .rsAptEditFormWrapper .specification_review div, #qsfexAdvEditWrapper .specification_review .qsfexAdvAppType
    {
        background: url('Images/specification_review.png') no-repeat top right;
    }
    
    #qsfexAdvEditWrapper div.specification_review, .rsAptType_specification_review
    {
        background: #EDD5B7;
    }
    
    * html .rsDayView .rsAptContent .specification_review div, * html .rsDayView .rsAptEditFormWrapper .specification_review div, * html #qsfexAdvEditWrapper .specification_review .qsfexAdvAppType
    {
        filter: progid:DXImageTransform.Microsoft.AlphaImageLoader (src= 'Images/specification_review.png' , sizingMethod= 'crop' );
    }
    
    * html #qsfexAdvEditWrapper #qsfexAdvEditInnerWrapper .qsfexAdvAppType
    {
        background: none;
    }
    
    * html .rsDayView .rsAptContent .rsCustomAppointmentContainer div, * html .rsDayView .rsAptEditFormWrapper .rsCustomAppointmentContainer div
    {
        border: 0;
        top: 2px;
        background: none;
        padding-top: 2px;
    }
    
    * html .rsDayView .rsAptEditFormWrapper .rsCustomAppointmentContainer div
    {
        padding: 0;
        top: 0;
        height: 132%;
    }
    
    .AppointmentTypeSelector
    {
        width: 22px;
        height: 14px;
        border: 1px solid #fff;
        display: block;
        float: left;
    }
    
    .AppointmentTypeSelectorTable
    {
        width: 160px;
        height: 22px;
    }
    
    .AppointmentTypeSelectorTable td
    {
        border: 0 !important;
    }
    
    .rsCustomAppointmentContainerInner .AppointmentTypeSelectorTable, .rsAdvancedEditLink
    {
        float: left;
    }
    
    * + html .rsCustomAppointmentContainerInner .AppointmentTypeSelectorTable, * + html .rsAdvancedEditLink
    {
        float: none;
        display: inline;
    }
    
    * html .rsCustomAppointmentContainerInner .AppointmentTypeSelectorTable, * html .rsAdvancedEditLink
    {
        overflow: hidden;
        float: none;
        display: inline;
    }
    
    .RadScheduler a.rsAdvancedEditLink
    {
        color: #333;
        padding: 3px;
        vertical-align: top;
        display: inline-block;
    }
    
    .AppointmentTypeSelectorTable input, .AppointmentTypeSelectorTable label
    {
        float: left;
        clear: none;
    }
    
    .AppointmentTypeSelectorTable input
    {
        margin: 2px;
        height: 13px;
    }
    
    .RadScheduler .inline-label
    {
        float: left;
        clear: left;
        width: 90px;
        color: #333;
        display: inline-block;
        vertical-align: middle;
        padding: 2px 3px 2px 8px;
    }
    
    .repeatCheckBox label
    {
        vertical-align: text-top;
    }
    
    #qsfexAdvEditWrapper
    {
        /* background: #EEE none repeat scroll 0 0;*/
        border: 1px solid #666666;
        padding: 20px;
    }
    
    #qsfexAdvEditInnerWrapper
    {
        /*border: 1px solid #777;*/
        padding-top: 5px;
    }
    
    .qsfexAdvEditControlWrapper input.riTextBox
    {
        margin-bottom: 2px;
    }
    
    #RadScheduler1_Form_RepeatCheckBox
    {
        margin: 3px 3px 3px 8px !important;
    }
    
    * html .RadScheduler .rsApt
    {
        filter: none !important;
    }
    
    
    a.rsToday:first-child
    {
        text-transform: uppercase;
    }
    
    .RadScheduler .rsMonthView .rsWrap
    {
        z-index: 0 !important;
    }
    .RadScheduler .rsHeader
    {
        z-index: 1 !important;
    }   
    .paddingScheduler
    {
        padding-left: 5px !important;
    }
    .paddingSchedulerEND
    {
        padding-left: 12px !important;
    }
    .paddingSchedulerLocation
    {
        padding-left: 14px !important;
    } 
  </style> 
<div id="EventCalenderSchedulerdiv" runat="server" >
    <div id="divfilter" style="padding-bottom: 10px;">
        <span id="spanFilter" runat="server">Filter By: </span>
        <rad:RadComboBox ID="CmbEventtype" runat="server" AutoPostBack="true" EmptyMessage="Select Event Type" MarkFirstMatch="true"></rad:RadComboBox>
        <rad:RadComboBox ID="cmbPerson" runat="server" AutoPostBack="true" EmptyMessage="Select Person" MarkFirstMatch="true"></rad:RadComboBox>
    </div>
   <%-- amruta IssueID 14347 CSS for radscheduler height and scrollbar property set--%>
    <rad:RadScheduler ID="EventCalenderScheduler" runat="server" CssClass="eee .radSchedulerCalendarViewEventHeight" AllowInsert="false" AllowDelete="false" RowHeight="50px" EnableEmbeddedBaseStylesheet="true" EnableTheming="true" SelectedView="MonthView" Localization-AllDay="All Day" ShowFooter="false" OverflowBehavior="Scroll">
       <MonthView AdaptiveRowHeight="true" VisibleAppointmentsPerDay="10"/>
         <AdvancedEditTemplate>
            <div>
                <div class="BorderDiv">
                    <table cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <%--  Anil Changess for issue 12718--%>
                            <td class="tdMeetingDetailsSchedulerCalendarViewEvent" colspan="2">
                                <table width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td width="2%">
                                            &nbsp;
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="Label6" runat="server" Text='<%# Bind("Subject") %>'></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <%--  end--%>
                        </tr>
                    </table>
                </div>
               <div id="qsfexAdvEditWrapper">
                    <div id="qsfexAdvEditInnerWrapper">
                        <div class="qsfexAdvAppType">
                        </div>
                        <div class="qsfexAdvEditControlWrapper">
                            <asp:Label ID="Label1" AssociatedControlID="TitleTextBox" runat="server" Font-Bold="true">Description:</asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="TitleTextBox" runat="server" Text='<%# Bind("Subject") %>'></asp:Label><br />
                        </div>
                        <div class="qsfexAdvEditControlWrapper">
                            <asp:Label ID="Label2" AssociatedControlID="StartInput" runat="server" Font-Bold="true"
                                CssClass="paddingScheduler">Start Date:</asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="StartInput" Text='<%# GetDateTimeString(Eval("Start")) %>' runat="server"></asp:Label>
                            <br />
                        </div>
                        <div class="qsfexAdvEditControlWrapper">
                            <asp:Label ID="Label3" AssociatedControlID="EndInput" runat="server" Font-Bold="true"
                                CssClass="paddingSchedulerEND">End Date:</asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="EndInput" Text='<%# GetDateTimeString(Eval("End")) %>' runat="server"></asp:Label>
                            <br />
                        </div>
                        <div class="qsfexAdvEditControlWrapper">
                            <asp:Label ID="Label4" AssociatedControlID="Label5" runat="server" Font-Bold="true"
                                CssClass="paddingSchedulerLocation">Location:</asp:Label>&nbsp;&nbsp;
                            <asp:Label ID="Label5" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                            <br />
                        </div>
                        <div class="qsfexAdvEditControlWrapper" style="text-align: right;">
                            <asp:Button ID="UpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                                Text="Back" CssClass="submitBtn" />
                        </div>
                    </div>
                </div>
            </div>
        </AdvancedEditTemplate>
    </rad:RadScheduler>
    <cc2:User ID="User1" runat="server" />
</div>
