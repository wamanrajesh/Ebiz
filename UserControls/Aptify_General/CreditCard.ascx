<%--Aptify e-Business 5.5.1, July 2013--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="CreditCard.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.CreditCard"
    Debug="true" %>
<%@ Register Src="../Aptify_Product_Catalog/CartGrid2.ascx" TagName="CartGrid2" TagPrefix="uc1" %>
<%@ Register Assembly="EBusinessShoppingCart" Namespace="Aptify.Framework.Web.eBusiness"
    TagPrefix="cc1" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!--RashmiP issue 6781 -->
<script type="text/javascript">
    function ShowHideControls() {
        var chkBillMeLater = document.getElementById('<%= chkBillMeLater.ClientID %>');
        var Validator1 = document.getElementById('<%= RequiredFieldValidator1.ClientID %>');
        var Validator2 = document.getElementById('<%= RequiredFieldValidator2.ClientID %>');

        var tblMain = document.getElementById('<%= tblMain.ClientID %>');
        var tblPO = document.getElementById('<%= tblPONum.ClientID %>');


        if (chkBillMeLater.checked == true) {
            ValidatorEnable(Validator1, false);
            ValidatorEnable(Validator2, false);
            tblMain.style.display = "none";
            tblPO.style.display = "block";
        }
        else {
            tblMain.style.display = "block";
            tblPO.style.display = "none";
            ValidatorEnable(Validator1, true);
            ValidatorEnable(Validator2, true);
        }
    }

    //    Anil B Issue 10254 on 07-03-2013
    //    Set the Sequirity textbox desabled on a leave event of the Card Number
    function SecurityDesabled() {
        var txtSecurity = document.getElementById('<%= txtCCNumber.ClientID %>');
        var hdnCCPartialNumber = document.getElementById('<%= hdnCCPartialNumber.ClientID %>');
        var txtCCSecurityNumber = document.getElementById('<%= txtCCSecurityNumber.ClientID %>');
        if (txtSecurity.value != hdnCCPartialNumber.value && txtSecurity.value != "") {
            txtCCSecurityNumber.disabled = true;
        }

    }
    //    Anil B Issue 10254 on 07-03-2013
    //    Set the Sequirity textbox enabled on a Change event of the Card Number
    function SecurityEnabled() {
        var txtSecurity = document.getElementById('<%= txtCCNumber.ClientID %>');
        var hdnCCPartialNumber = document.getElementById('<%= hdnCCPartialNumber.ClientID %>');
        hdnCCPartialNumber.value = txtSecurity.value;
        txtCCSecurityNumber.disabled = false;
    }     
</script>
<%--Nalini issue#12578--%>
<asp:UpdatePanel ID="upnlCreditCard" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
    <%--Sandeep issue#14671 20/02/2013--%>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="cmbSavedPaymentMethod" EventName="" />
        <asp:AsyncPostBackTrigger ControlID="txtCCNumber" EventName="" />
        <asp:AsyncPostBackTrigger ControlID="chkBillMeLater" EventName="" />
        <%--Sandeep issue#14671 20/02/2013--%>
    </Triggers>
    <ContentTemplate>
        <div class="padding-all credit-card">
            <div runat="server" id="Table1">
                <div class="row-div clearfix">
                    <span>
                            <asp:Panel  ID = "pnlPaymentType" runat ="server" GroupingText="Select Payment Type" >
                                <asp:RadioButton ID="rbCompanyPayment" runat="server" Text="Corporate Payment" GroupName="PaymentType"
                                    AutoPostBack="true" />
                                <asp:RadioButton ID="rbIndividualPayment" Text="Personal Payment" runat="server"
                                    GroupName="PaymentType" AutoPostBack="true" />
                         </asp:Panel>
                    </span>
                </div>
                <div class="row-div clearfix">
                    <div class="label-div w25">
                        <asp:Label ID="lblBillMelater" runat="server"></asp:Label>
                    </div>
                    <div class="field-div1 w74">
                        <asp:CheckBox ID="chkBillMeLater" runat="server" Text="" TextAlign="Left" OnCheckedChanged="chkBillMeLater_CheckedChanged"
                            AutoPostBack="true" />
                    </div>
                </div>
            </div>
            <%--Anil B Issue 10254 on 07-03-2013
    Set the design thrugh css--%>
            <div id="tblMain" runat="server">
                <div class="row-div clearfix">
                    <div class="label-div w25">
                        <asp:Label ID="lblSavedPayment" runat="server" Text="Select your Card:"></asp:Label></div>
                    <div class="field-div1 w74">
                        <asp:DropDownList ID="cmbSavedPaymentMethod" runat="server" AutoPostBack="true">
                        </asp:DropDownList>
                    </div>
                </div>
                <div runat="server" id="PaymentTypeSelection">
                    <div class="row-div clearfix">
                        <div class="label-div w25">
                            Credit Card:</div>
                        <div class="field-div1 w74">
                            <rad:RadBinaryImage ID="ImgVisa" runat="server" AutoAdjustImageControlSize="false">
                            </rad:RadBinaryImage>
                            <rad:RadBinaryImage ID="ImgMasterCard" runat="server" AutoAdjustImageControlSize="false">
                            </rad:RadBinaryImage>
                            <rad:RadBinaryImage ID="ImgAmericanExpress" runat="server" AutoAdjustImageControlSize="false">
                            </rad:RadBinaryImage>
                            <rad:RadBinaryImage ID="ImgDiscover" runat="server" AutoAdjustImageControlSize="false">
                            </rad:RadBinaryImage>
                            <asp:Label ID="lbl1" Text="Accepted Cards" runat="server"></asp:Label>
                            <asp:DropDownList ID="cmbCreditCard" runat="server" AppendDataBoundItems="True" Visible="false">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="row-div clearfix cno-div" id="trCardNum" runat="server">
                    <div class="label-div w25">
                        <span class="required-label">*</span>
                        <asp:Label ID="lblCardNo" runat="server" Text="Card Number:"></asp:Label>
                    </div>
                    <div class="field-div1 w74">
                        <asp:TextBox ID="txtCCNumber" runat="server" AutoComplete="Off" AutoPostBack="true"
                            OnTextChanged="txtCCNumber_TextChanged" EnableViewState="False" Width="155px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCCNumber"
                            Enabled="True" ErrorMessage="Credit Card # Required" CssClass="required-label"
                            Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row-div clearfix secret-div" id="trSecurity" runat="server">
                    <div class="label-div w25">
                        <span class="required-label">*</span> Security # :</div>
                    <div class="field-div1 w74">
                        <asp:TextBox ID="txtCCSecurityNumber" runat="server" EnableViewState="false" AutoComplete="Off"
                            MaxLength="4"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Security # Required"
                            ControlToValidate="txtCCSecurityNumber" Enabled="True" CssClass="required-label"
                            Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row-div clearfix" id="trExpiryDate" runat="server">
                    <div class="label-div w25">
                        Expiration Date:</div>
                    <div class="field-div1 w74">
                        <asp:DropDownList ID="dropdownMonth" runat="server">
                            <asp:ListItem Value="1">January</asp:ListItem>
                            <asp:ListItem Value="2">February</asp:ListItem>
                            <asp:ListItem Value="3">March</asp:ListItem>
                            <asp:ListItem Value="4">April</asp:ListItem>
                            <asp:ListItem Value="5">May</asp:ListItem>
                            <asp:ListItem Value="6">June</asp:ListItem>
                            <asp:ListItem Value="7">July</asp:ListItem>
                            <asp:ListItem Value="8">August</asp:ListItem>
                            <asp:ListItem Value="9">September</asp:ListItem>
                            <asp:ListItem Value="10">October</asp:ListItem>
                            <asp:ListItem Value="11">November</asp:ListItem>
                            <asp:ListItem Value="12">December</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="dropdownDay" runat="server" Visible="False">
                        </asp:DropDownList>
                        <asp:DropDownList ID="dropdownYear" runat="server">
                        </asp:DropDownList>
                        <asp:CustomValidator ID="vldExpirationDate" runat="server" ControlToValidate="dropdownDay"
                            ErrorMessage="The date selected is not valid." Display="Dynamic" CssClass="error-msg-label"></asp:CustomValidator>
                    </div>
                </div>
                <div>
                    <asp:HiddenField ID="hfCCNumber" runat="server" />
                    <asp:CheckBox ID="chkSaveforFutureUse" Text=" Save for Future Use" runat="server" />
                </div>
                <div id="trError" runat="server">
                    <asp:Button ID="btnUpload" CssClass="submit-Btn" runat="server" CausesValidation="False"
                        Text="" Style="display: none" />
                    <%--Anil B, Issue 10254, 20/04/2013--%>
                    <asp:UpdatePanel ID="upnlError" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true"
                        CssClass="error-msg-label">
                        <ContentTemplate>
                            <asp:Label ID="lblError" runat="server" Text="" CssClass="error-msg-label"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div id="tblPONum" runat="server" class="row-div clearfix">
                <div class="label-div w25">
                    PO Number:</div>
                <div class="field-div1 w74">
                    <asp:TextBox ID="txtPONumber" runat="server" EnableViewState="False" AutoComplete="Off"
                        MaxLength="25"></asp:TextBox>
                </div>
            </div>
            <asp:HiddenField ID="hdnCCPartialNumber" ViewStateMode="Enabled" runat="server" />
            <cc1:AptifyShoppingCart runat="Server" ID="ShoppingCart1" />
            <cc2:User runat="Server" ID="User1" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
