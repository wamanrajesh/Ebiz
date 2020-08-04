<%--Aptify e-Business 5.5.1, July 2013--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="CPEReports.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.CPEReports" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<style>
      .grdActiveCertTD
{
    border: 1px solid #eee;
    font-family:segoe ui,?arial,?sans-serif;
    font-size:12px;
    font-weight:400;
    font-style:normal;
    color:#000000;  
 
}
.grdActiveCertTH
{
    border: 1px solid #eee;
     font-family:segoe ui,?arial,?sans-serif;
    font-size:12px ! Important;
    font-weight:bold ! Important;;  
    font-style:normal ! Important;
    color:#000000 ! Important;   
}

.grdDeActiveCertTD
{
    border: 1px solid #eee;
    font-family:segoe ui,?arial,?sans-serif;
    font-size:12px;
    font-weight:400;
    font-style:normal;
    color:#000000;  
 
}
.grdDeActiveCertTH
{
    border: 1px solid #eee;
     font-family:Serif;
    font-size:12px;
   font-weight:bold;  
    font-style:normal;
    color:#000000;   
}


.lblCertificationReport
{
    font-family:segoe ui,?arial,?sans-serif; 
     font-size:14px;
      font-weight:400;
      font-style:normal;
    color:#000000;   
}
.memberInfoTD
{
    width:690px;
    
}
</style>
<div>
    <table>
        <tr>
            <td valign="top" class="memberInfoTD">
                <p>
                    <asp:Label ID="lblFirstLast" CssClass="lblCertificationReport" runat="server" Text=""></asp:Label>
                    <br />
                    <asp:Label ID="lblTitle" runat="server" Text="" CssClass="lblCertificationReport"></asp:Label>
                    <br />
                    <asp:Label ID="lblCompany" runat="server" Text="" CssClass="lblCertificationReport"></asp:Label>                    `
                    <br>
                    <asp:Label ID="lblTotalUnit" runat="server" Text="" CssClass="lblCertificationReport"></asp:Label>
                    <br>
                </p>
            </td>
            <td valign="top">
                <asp:Label ID="lblCurrentDate" runat="server" Text="" CssClass="lblCertificationReport"></asp:Label>
                <br />
            </td>
        </tr>
    </table>
    <div>
        <asp:Label ID="lblmsg" Text="" runat="server"></asp:Label></div>
    <div>
        <asp:Label ID="lblReportingRange" runat="server" Font-Bold="True" Font-Italic="True"></asp:Label></div>
    <div style="height: 10px">
    </div>
    <div>
        <asp:Label ID="lblActiveCertification" runat="server" Font-Bold="True" Font-Italic="True"
            Text="Active Certification:"></asp:Label></div>
    <div>
        <asp:GridView ID="grdActiveCertifications" AutoGenerateColumns="false" runat="server"
            ShowFooter="False" AllowPaging="false" CellSpacing="0" BorderWidth="1px" BorderStyle="Solid">
            <Columns>
                <asp:BoundField HeaderText="Certification" ItemStyle-CssClass="grdActiveCertTD" HeaderStyle-CssClass="grdActiveCertTH"  DataField="Title">
                    <ItemStyle Width="200px" />
                </asp:BoundField>
             <%--   'Anil B For issue 14344 on 17-04-2013
                'Remove unwanted column--%>
             
                <asp:BoundField HeaderText="Requirement" ItemStyle-CssClass="grdActiveCertTD" HeaderStyle-CssClass="grdActiveCertTH" DataField="Course">
                    <ItemStyle Width="120px" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Unit" ItemStyle-CssClass="grdActiveCertTD" HeaderStyle-CssClass="grdActiveCertTH" DataField="Units">
                    <ItemStyle Width="80px" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Status" ItemStyle-CssClass="grdActiveCertTD" HeaderStyle-CssClass="grdActiveCertTH" DataField="Status">
                    <ItemStyle Width="100px" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Granted On" ItemStyle-CssClass="grdActiveCertTD" HeaderStyle-CssClass="grdActiveCertTH" DataField="DateGranted">
                    <ItemStyle Width="100px" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Expires On" ItemStyle-CssClass="grdActiveCertTD" HeaderStyle-CssClass="grdActiveCertTH" DataField="ExpirationDate">
                    <ItemStyle Width="100px" />
                </asp:BoundField>
            </Columns>
        </asp:GridView>
    </div>
    <div style="height: 10px">
    </div>
    <div>
        <asp:Label ID="lblDeActiveCertification" runat="server" Font-Bold="True" Font-Italic="True"
            Text="Expired Certification:"></asp:Label></div>
    <div>
        <asp:GridView ID="grdDeactiveCertifications" AutoGenerateColumns="false" runat="server"
            ShowFooter="False" AllowPaging="false" CellSpacing="0" BorderWidth="1px" BorderStyle="Solid">
            <Columns>
                <asp:BoundField HeaderText="Certification" ItemStyle-CssClass="grdDeActiveCertTD" HeaderStyle-CssClass="grdDeActiveCertTH"  DataField="Title">
                    <ItemStyle Width="200px" />
                </asp:BoundField>
                 <%--   'Anil B For issue 14344 on 17-04-2013
                'Remove unwanted column--%>
                <asp:BoundField HeaderText="Requirement" ItemStyle-CssClass="grdDeActiveCertTD" HeaderStyle-CssClass="grdDeActiveCertTH" DataField="Course">
                    <ItemStyle Width="120px" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Unit" ItemStyle-CssClass="grdDeActiveCertTD" HeaderStyle-CssClass="grdDeActiveCertTH" DataField="Units">
                    <ItemStyle Width="80px" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Status" ItemStyle-CssClass="grdDeActiveCertTD" HeaderStyle-CssClass="grdDeActiveCertTH" DataField="Status">
                    <ItemStyle Width="100px" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Granted On" ItemStyle-CssClass="grdDeActiveCertTD" HeaderStyle-CssClass="grdDeActiveCertTH" DataField="DateGranted">
                    <ItemStyle Width="100px" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Expires On" ItemStyle-CssClass="grdDeActiveCertTD" HeaderStyle-CssClass="grdDeActiveCertTH" DataField="ExpirationDate">
                    <ItemStyle Width="100px" />
                </asp:BoundField>
            </Columns>
        </asp:GridView>
    </div>
</div>
<cc1:User runat="server" ID="EbusinesUser" />
