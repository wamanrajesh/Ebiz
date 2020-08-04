<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="AdminOrderDetail.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.ProductCatalog.AdminOrderDetail" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessGlobal" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="uc1" TagName="CreditCard" Src="../Aptify_General/CreditCard.ascx" %>
<script language="javascript" type="text/javascript">

    function validatePage() {
        var isValid = Page_ClientValidate();
        var button = document.getElementById('<%= cmdMakePayment.ClientID %>');
       var buttonText = button.value;
       button.disabled = true;
       button.value = 'Submitting...';

       button.disabled = isValid;

       if (isValid == false) {
           button.value = buttonText;
       }
   }

   function UpdateItemCountField(sender, args) {
       //set the footer text
       sender.get_dropDownElement().lastChild.innerHTML = "A total of " + sender.get_items().get_count() + " items";
   }

   function SubmittingSave() {
       var isValid = Page_ClientValidate();
       var button = document.getElementById('<%= BtnSave.ClientID %>');
         var hdnTemp = document.getElementById('<%= Hidden.ClientID %>');
         button.disabled = true;
         button.value = 'Submitting...';
         button.style.cursor = "auto";
         button.disabled = isValid;

         if (hdnTemp.value == "true") {
             hdnTemp.value = "false"
             button.click();
             hdnTemp.value = "true"
         }
     }

    <%--Anil Issue 6640--%>
    window.history.forward(1);

</script>
<div>
    <asp:UpdateProgress ID="updateProcessingIndicator" runat="server" DisplayAfter="0">
        <ProgressTemplate>
        <div class="processing-div">
            <div class="processing">
                Please wait...
            </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</div>
<asp:Label ID="lblrecmsg" runat="server" Visible="False" Text="Item not found" class="error-msg-label"></asp:Label>
<div id="payoffdiv" runat="server">
    <asp:UpdatePanel ID="UpdatepnlorderDetail" runat="server">
        <ContentTemplate>
            <div id="tblmember" runat="server">
                <div id="Td1" runat="server" class="row-div ">
                    Pay Open Invoices for My Company
                </div>
                <div class="row-div clearfix">
                    <div class="label-div w19">
                        <asp:Label ID="lblfilter" Text=" Currency Type:" runat="server" >
                        </asp:Label>
                    </div>
                    <div class="field-div1 w80">
                        <asp:DropDownList ID="radcurrency" AutoPostBack="true" DataTextField="CurrencyType"
                             DataValueField="CurrencyTypeID" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row-div">
                    <asp:Label ID="lblError" runat="server" Visible="False" class="error-msg-label">
                    </asp:Label>
                </div>
                <div class="row-div">
                    <rad:RadGrid ID="grdOrderDetails" runat="server" AutoGenerateColumns="False" SortingSettings-SortedDescToolTip="Sorted Descending"
                        SortingSettings-SortedAscToolTip="Sorted Ascending" PagerStyle-PageSizeLabelText="Records Per Page"
                        AllowPaging="True">
                        <MasterTableView AllowFilteringByColumn="true">
                            <Columns>
                                <rad:GridTemplateColumn AllowFiltering="false">
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:Image ID="ImgFlag" runat="server" Visible="false" ToolTip="Flagged for Review" />
                                    </ItemTemplate>
                                </rad:GridTemplateColumn>
                                <rad:GridTemplateColumn AllowFiltering="false">
                                    <ItemStyle HorizontalAlign="Center" ></ItemStyle>
                                    <HeaderStyle HorizontalAlign="center" />
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkAllMakePayment" runat="server" OnCheckedChanged="ToggleSelectedState"
                                            AutoPostBack="True" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chkMakePayment" OnCheckedChanged="chkMakePayment_CheckedChanged"
                                            AutoPostBack="true"></asp:CheckBox>
                                        <asp:Label ID="ID" Text='
								<%# DataBinder.Eval(Container.DataItem,"ID") %>
									' runat="server" Visible="false">
                                         </asp:Label>
                                    </ItemTemplate>
                                </rad:GridTemplateColumn>
                                <rad:GridTemplateColumn HeaderText="Name" DataField="Name" SortExpression="Name" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                    ShowFilterIcon="false">
                                    <HeaderStyle/>
                                    <ItemStyle HorizontalAlign="Center" >
                                    </ItemStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="lblMemberName" runat="server" Text='
									<%# DataBinder.Eval(Container.DataItem,"Name") %>
										'>
                                         </asp:Label>
                                    </ItemTemplate>
                                </rad:GridTemplateColumn>
                                <rad:GridBoundColumn HeaderText="City" SortExpression="City" AutoPostBackOnFilter="true"
                                    CurrentFilterFunction="Contains" ShowFilterIcon="false" DataField="City" />
                                <rad:GridTemplateColumn HeaderText="Order #" DataField="ID" SortExpression="ID" AutoPostBackOnFilter="true"
                                    CurrentFilterFunction="EqualTo" ShowFilterIcon="false">
                                    <HeaderStyle/>
                                    <ItemStyle />
                                    <ItemTemplate>
                                        <asp:HyperLink ID="lblOrderNo" NavigateUrl='
										<%# DataBinder.Eval(Container.DataItem,"OrderConfirmationURL") %>
											' runat="server" CssClass="namelink" Target="_blank" Text='
											<%# DataBinder.Eval(Container.DataItem,"ID") %>
												'>
                                         </asp:HyperLink>
                                    </ItemTemplate>
                                </rad:GridTemplateColumn>
                                <rad:GridDateTimeColumn DataField="OrderDate" UniqueName="GridDateTimeColumnOrderDate"
                                    HeaderText="Date" SortExpression="OrderDate"
                                    AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" DataType="System.DateTime"
                                    ShowFilterIcon="false" EnableTimeIndependentFiltering="true" />
                                <rad:GridBoundColumn HeaderText="Product(s)" ItemStyle-Wrap="true" DataField="Product" SortExpression="Product"
                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"/>
                                <rad:GridTemplateColumn HeaderStyle-HorizontalAlign="right" HeaderText="Total" SortExpression="GrandTotal"
                                    DataField="GrandTotal" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                    ShowFilterIcon="false">
                                    <ItemStyle />
                                    <HeaderStyle/>
                                    <ItemTemplate>
                                        <asp:Label ID="lblGrandTotal" runat="server" Text='
												<%#GetFormattedCurrency(Container, "GrandTotal")%>
													'>
                                         </asp:Label>
                                    </ItemTemplate>
                                </rad:GridTemplateColumn>
                                <rad:GridTemplateColumn HeaderStyle-HorizontalAlign="right" HeaderText="Balance"
                                    SortExpression="Balance" DataField="Balance" AutoPostBackOnFilter="true"
                                    CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                    <ItemStyle />
                                    <HeaderStyle />
                                    <ItemTemplate>
                                        <asp:Label ID="lblBalanceAmount" runat="server" Text='
													<%#GetFormattedCurrency(Container, "Balance")%>
														'>
                                         </asp:Label>
                                    </ItemTemplate>
                                </rad:GridTemplateColumn>
                                <rad:GridTemplateColumn HeaderText="Currency Symbol" Visible="false" DataField="CurrencySymbol">
                                    <ItemStyle/>
                                    <HeaderStyle />
                                    <ItemTemplate>
                                        <asp:Label ID="lblCurrencySymbol" runat="server" Text='
														<%# DataBinder.Eval(Container.DataItem,"CurrencySymbol") %>
															'>
                                         </asp:Label>
                                    </ItemTemplate>
                                </rad:GridTemplateColumn>
                                <rad:GridTemplateColumn HeaderText="Comments" AllowFiltering="false">
                                    <ItemStyle ></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkAddComments" runat="server" Text="Add/Edit" CommandArgument='
															<%# DataBinder.Eval(Container.DataItem,"ID") %>
																' CommandName="ADDEditComments" CausesValidation="false"
                                            Visible="true">
                                         </asp:LinkButton>
                                    </ItemTemplate>
                                </rad:GridTemplateColumn>
                                <rad:GridTemplateColumn HeaderText="Pay Amount" AllowFiltering="false">
                                    <ItemStyle >
                                    </ItemStyle>
                                    <HeaderStyle />
                                    <ItemTemplate>
                                        <asp:Label ID="lblNumDigitsAfterDecimal" Visible="false" runat="server" Text='
																<%# DataBinder.Eval(Container.DataItem,"NumDigitsAfterDecimal") %>
																	'>
                                         </asp:Label>
                                        <asp:Label ID="lblCurrSymbol" runat="server" Text='
																	<%# DataBinder.Eval(Container.DataItem,"CurrencySymbol") %>
																		'>
                                         </asp:Label>
                                        <asp:TextBox ID="txtPayAmt" CssClass="w74" size="15" runat="server" Text="0.00" OnTextChanged="txtPayAmt_TextChanged" AutoPostBack="true">
                                         </asp:TextBox>
                                    </ItemTemplate>
                                </rad:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </rad:RadGrid>
                </div>
                <div class="row-div clearfix">
                    <div class="label-div w90">
                        <asp:Label ID="lblTotal" Text="Total: " runat="server">
                        </asp:Label>
                    </div>
                    <div class="float-right">
                        <asp:Label ID="txtTotal" runat="server">
                        </asp:Label>
                    </div>
                </div>
                <div class="row-div">
                    <telerik:RadWindow ID="radGAReviewComments" runat="server" Modal="True" Skin="Default" VisibleStatusbar="False"
                        Behaviors="None" IconUrl="~/Images/Comments.png" class="popup-review-comments"
                        Title="Review Comments">
                        <ContentTemplate>
                            <asp:UpdatePanel runat="server" ID="UpEditBadgeInfo">
                                <ContentTemplate>
                                    <div class="table-div">
                                        
                                            <div class="row-div">
                                                <asp:Label ID="lblOrderID" runat="server" Text="" Visible="false">
                                                </asp:Label>
                                            </div>
                                        
                                        <div class="row-div clearfix">
                                            <div class="label-div w25">
                                                Comments: 
                                            </div>
                                            <div class="field-div1 w74">
                                                <asp:TextBox ID="txtGAReviewComments" runat="server" TextMode="MultiLine" CssClass="txt-restrict-resize">
                                                </asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row-div clearfix">
                                            <div class="label-div w25">&nbsp;</div>
                                            <div class="field-div1 w74">
                                                <asp:Button ID="BtnSave" class="submit-Btn" runat="server" Text="Save" CausesValidation="false"
                                                    ValidationGroup="grpAddComments" />
                                                <asp:Button ID="BtnCancel" runat="server" Text="Cancel" class="submit-Btn" CausesValidation="false" />
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="BtnSave" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </telerik:RadWindow>
                </div>
            </div>
            
            <div class="label">
                Payment Information 
            </div>
            <div class="row-div clearfix">
                <uc1:CreditCard ID="CreditCard" runat="server" />
            </div>
            
            <rad:RadWindow ID="radpaymentmsg" runat="server" class="popup-adminOrder-paymentmsg"  Modal="True"
                 Skin="Default" VisibleStatusbar="False" Behaviors="None"
                 Title="Order Payment" Behavior="None">
                <ContentTemplate>
                    <div class="row-div label">
                        <asp:Label ID="lblpayment" runat="server" Text=" Your payment was made successfully!">
                         </asp:Label>
                    </div>
                    <div class="row-div">
                        <asp:Button ID="btnok" runat="server" Text="OK" class="submit-Btn" OnClick="btnok_Click"
                            ValidationGroup="ok" />
                    </div>
                </ContentTemplate>
            </rad:RadWindow>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="radpaymentmsg" />
        </Triggers>
    </asp:UpdatePanel>
    <div class="row-div">        
        <asp:Button CssClass="submit-Btn" ID="cmdMakePayment" TabIndex="1" runat="server"
           Text="Make Payment" UseSubmitBehavior="false" OnClientClick="validatePage();">
        </asp:Button>
    </div>
</div>
<cc2:User runat="Server" ID="User1" />
<asp:HiddenField runat="server" ID="Hidden" Value="true" />
