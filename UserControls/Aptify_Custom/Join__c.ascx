<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Join__c.ascx.vb" ViewStateMode="Enabled"
    Inherits="Aptify.Framework.Web.eBusiness.ProductCatalog.Join__c" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<script type="text/javascript">
    function RadioButtonChecked(current) {

        for (i = 0; i < document.forms[0].elements.length; i++) {
            elm = document.forms[0].elements[i]
            if (elm.type == 'radio') {
                elm.checked = false;
            }
        }
        current.checked = true;
    }

</script>
<div>
    <h4 runat="server" id="lblHeader" class="label" visible="false"></h4>
</div>
<div id="divMain" runat="server">
    <div>
        <div>
            <asp:UpdatePanel ID="UpdatePanelgridMain" runat="server">
                <ContentTemplate>
                    <rad:RadGrid ID="gridMain" runat="server" AutoGenerateColumns="False" ShowHeader="true"
                        ShowFooter="false" EnableTheming="true" EmptyDataText="No Products to display"
                        AllowPaging="true" Visible="true">
                        <MasterTableView>
                            <Columns>
                                <rad:GridTemplateColumn>
                                    <ItemTemplate>
                                        <asp:RadioButton ID="radioButton" runat="server" Onclick="RadioButtonChecked(this)" />
                                    </ItemTemplate>
                                </rad:GridTemplateColumn>
                                <rad:GridBoundColumn HeaderText="ID" DataField="ID" UniqueName="ID">
                                </rad:GridBoundColumn>
                                <rad:GridBoundColumn HeaderText="Name" DataField="Name">
                                </rad:GridBoundColumn>
                                <rad:GridBoundColumn HeaderText="Price" DataField="Price">
                                </rad:GridBoundColumn>
                                <rad:GridBoundColumn HeaderText="Description" DataField="Description">
                                </rad:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                    </rad:RadGrid>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="right-container w80">&nbsp;</div>
        <div class="right-container w80">
            <asp:Label ID="lblCampaign" runat="server" Text="Enter Campaign Code"></asp:Label>
            <asp:TextBox ID="txtCampaignCode" runat="server"></asp:TextBox>
            <asp:Button ID="btnContinue" runat="server" Text="Continue" OnClick="btnContinue_Click" />
        </div>
    </div>
</div>
<br />
