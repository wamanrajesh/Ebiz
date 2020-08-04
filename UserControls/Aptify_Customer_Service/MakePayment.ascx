<%--Aptify e-Business 5.5.2 Hotfix, Issue 20575, January 2015--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="MakePayment.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.CustomerService.MakePaymentControl" %>
<%@ Register TagPrefix="uc1" TagName="CreditCard" Src="../Aptify_General/CreditCard.ascx" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessShoppingCart" %>

<script language="javascript" type="text/javascript">

    function validatePage() {
        var isValid = Page_ClientValidate();
        var button = document.getElementById('<%= cmdPay.ClientID %>');
        var buttonText = button.value;
        button.disabled = true;
        button.value = 'Submitting...';

        button.disabled = isValid;

        if (isValid == false) {
            button.value = buttonText;
        }
    }

    <%--Anil Issue 6640--%>
    window.history.forward(1);

</script>


<div class="table-div">
    <div id="tblMain" runat="server" class="row-div">
        <div id="paymentMade" visible="false" runat="server" class="errormsg-div">
            <asp:Label ID="lblMessage"  runat="server">
            </asp:Label>
        </div>
    </div>
    <div class="row-div">
        <rad:RadGrid ID="grdMain" AutoGenerateColumns="False" runat="server" AllowPaging="false"
            EnableLinqExpressions="false" SortingSettings-SortedDescToolTip="Sorted Descending"
            SortingSettings-SortedAscToolTip="Sorted Ascending" AllowFilteringByColumn="true">
            <HeaderStyle  Font-Bold="true" />
            <GroupingSettings CaseSensitive="false" />
            <MasterTableView AllowFilteringByColumn="true" AllowSorting="true" AllowNaturalSort="false"
                NoMasterRecordsText="No Payment Available.">
                <PagerStyle AlwaysVisible="true" />
                <Columns>
                    <rad:GridHyperLinkColumn DataNavigateUrlFields="ID" DataTextField="ID" HeaderText="Order #"
                        SortExpression="ID" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                        ShowFilterIcon="false" />
                    <rad:GridDateTimeColumn DataField="OrderDate" UniqueName="GridDateTimeColumnOrderDate"
                        HeaderText="Date" SortExpression="OrderDate"
                        AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" DataType="System.DateTime"
                        ShowFilterIcon="false" EnableTimeIndependentFiltering="true" />
                    <rad:GridTemplateColumn HeaderText="Total" DataField="GrandTotal" SortExpression="GrandTotal"
                        AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                        ShowFilterIcon="false">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%#GetFormattedCurrency(Container, "GrandTotal")%>'>
                            </asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%#GetFormattedCurrency(Container, "GrandTotal")%>'>
                            </asp:TextBox>
                        </EditItemTemplate>
                        <ItemStyle HorizontalAlign="Right" />
                        <HeaderStyle HorizontalAlign="Right" />
                    </rad:GridTemplateColumn>
                    <rad:GridTemplateColumn HeaderText="Balance" DataField="Balance" SortExpression="Balance"
                        AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                        ShowFilterIcon="false">
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%#GetFormattedCurrency(Container, "Balance")%>'>
                            </asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox2" runat="server" Text='<%#GetFormattedCurrency(Container, "Balance")%>'>
                            </asp:TextBox>
                        </EditItemTemplate>
                        <ItemStyle HorizontalAlign="Right" />
                        <HeaderStyle HorizontalAlign="Right" />
                    </rad:GridTemplateColumn>
                    <rad:GridTemplateColumn HeaderText="Pay Amount" DataField="Balance" SortExpression="Balance"
                         AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo"
                        ShowFilterIcon="false">
                        <ItemTemplate>
                                                              <asp:Label ID="lblCurrSymbol" runat="server" Text='
																	<%# DataBinder.Eval(Container.DataItem,"CurrencySymbol") %>'>
                                         </asp:Label>
                            <input id="txtPayAmt" type="text" size="23" runat="server"
                                value='<%#DataBinder.Eval(Container.DataItem,"Balance")%>' />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtPayAmt"
                                ErrorMessage="Pay amount required" runat="server" CssClass="required-label" ></asp:RequiredFieldValidator>

                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right" />
                        <HeaderStyle HorizontalAlign="Right" />
                    </rad:GridTemplateColumn>
                    <rad:GridTemplateColumn HeaderText="Currency Symbol" Visible="false" DataField="CurrencySymbol"
                        SortExpression="CurrencySymbol" AutoPostBackOnFilter="true"
                        CurrentFilterFunction="EqualTo" ShowFilterIcon="false">
                        <ItemTemplate>
                            <asp:Label ID="lblCurrencySymbol" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"CurrencySymbol") %>'></asp:Label>
                        </ItemTemplate>
                    </rad:GridTemplateColumn>
                    <rad:GridTemplateColumn HeaderText="Currency Digits" Visible="false" DataField="NumDigitsAfterDecimal"
                        SortExpression="NumDigitsAfterDecimal" AutoPostBackOnFilter="true"
                        CurrentFilterFunction="EqualTo" ShowFilterIcon="false">
                        <ItemTemplate>
                            <asp:Label ID="lblNumDigitsAfterDecimal" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"NumDigitsAfterDecimal") %>'></asp:Label>
                        </ItemTemplate>
                    </rad:GridTemplateColumn>
                    <rad:GridTemplateColumn HeaderText="Actual Balance" Visible="false" DataField="Balance"
                        SortExpression="Balance" AutoPostBackOnFilter="true"
                        CurrentFilterFunction="EqualTo" ShowFilterIcon="false">
                        <ItemTemplate>
                            <asp:Label ID="lblBalance" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Balance") %>'></asp:Label>
                        </ItemTemplate>
                    </rad:GridTemplateColumn>
                </Columns>
            </MasterTableView>
        </rad:RadGrid>
        <asp:UpdatePanel ID="updPanelGrid" runat="server" UpdateMode="Always">
            <ContentTemplate>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Label ID="lblNoRecords" runat="server" Visible="False"><strong>No unpaid orders exist for this account.</strong></asp:Label>
    </div>
    <div class="row-div">    
        <uc1:CreditCard ID="CreditCard" runat="server"></uc1:CreditCard>
    </div>
    <div class="row-div clearfix">
    <div class="label-div w19">&nbsp;</div>
    <div class="field-div1 w80">
        <asp:Button CssClass="submit-Btn" ID="cmdPay" runat="server" Text="Make Payment" UseSubmitBehavior="false" OnClientClick="validatePage();">
        </asp:Button>
      </div>
        
    </div>
    <div class="row-div">
     <asp:Label ID="lblError" runat="server" CssClass="error-msg-label" Visible="False">
        </asp:Label></div>
    <cc2:User ID="User1" runat="server" />
    <cc1:AptifyShoppingCart runat="Server" ID="ShoppingCart1" />
</div>
