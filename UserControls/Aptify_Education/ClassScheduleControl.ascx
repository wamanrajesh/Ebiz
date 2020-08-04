<%--Aptify e-Business 5.5.1, July 2013--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ClassScheduleControl.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.Education.ClassScheduleControl" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>

<table runat="server" id="tblSchedule" class="table-div">
    <tr>
        <td class="SmallHeader">Category</td>
        <td class="SmallHeader">Course</td>
        <td class="SmallHeader">Date</td>
        <td class="SmallHeader">Type</td>
        <td runat="server" id="tdLocationHeader" class="SmallHeader">Location</td>
        <td runat="server" id="tdInstructorHeader" class="SmallHeader">Instructor</td>
    </tr>    
    <tr>
        <td>
            <asp:DropDownList Font-Size="9pt" ID="cmbCategory" runat="server" AutoPostBack="true" />
        </td>
        <td>
            <asp:DropDownList Font-Size="9pt" ID="cmbCourse" runat="server"  AutoPostBack="true" />
        </td>
        <td>
            <asp:DropDownList Font-Size="9pt" ID="cmbStartDate" runat="server" AutoPostBack="true" >
                <asp:ListItem Selected="true" Value="0" Text="Anytime"></asp:ListItem>
                <asp:ListItem Value="1" Text="Next 30 Days"></asp:ListItem>
                <asp:ListItem Value="2" Text="Next 2 months"></asp:ListItem>
                <asp:ListItem Value="3" Text="Next 3 months"></asp:ListItem>
                <asp:ListItem Value="6" Text="Next 6 months"></asp:ListItem>
                <asp:ListItem Value="9" Text="Next 9 months"></asp:ListItem>
                <asp:ListItem Value="12" Text="Next 12 months"></asp:ListItem>
                <asp:ListItem Value="18" Text="Next 18 months"></asp:ListItem>
                <asp:ListItem Value="24" Text="Next 24 months"></asp:ListItem>
            </asp:DropDownList>
        </td>
        <td>
            <asp:DropDownList Font-Size="9pt" ID="cmbType" runat="server"  AutoPostBack="true" />
        </td>
        <td  runat="server" id="tdLocationCombo">
            <asp:DropDownList Font-Size="9pt" ID="cmbLocation" runat="server"  AutoPostBack="true" />
        </td>
        <td  runat="server" id="tdInstructorCombo">
            <asp:DropDownList Font-Size="9pt" ID="cmbInstructor" runat="server"  AutoPostBack="true" />
        </td>
    </tr>    
</table>
<cc1:user id="User1" runat="server"></cc1:user>
