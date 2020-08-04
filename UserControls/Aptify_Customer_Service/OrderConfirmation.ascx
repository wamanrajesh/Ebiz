<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="OrderConfirmation.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.CustomerService.OrderConfirmationControl" %>
<%@ Register TagPrefix="uc1" TagName="NameAddressBlock" Src="../Aptify_General/NameAddressBlock.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<script language="javascript" type="text/javascript">
    window.history.forward(1);
    <%--Suraj Issue 15210 ,2/41/13 ifemail validation fire the sendmail label message display none --%>
    function clearLabelValue() {
        if (Page_ClientValidate("OrderconformationEmail")) {
            return true;
        }
        else 
        {
            document.getElementById("<%=SendEmailLabel.ClientID%>").style.display = 'none';
            return false;

        }
    }
</script>
<div class="table-div" id="tblMain" runat="server">
    <div class="table-div">
        <div class="row-div">
            To email this Order Confirmation, enter the email address below and click the button.
            (Multiple email addresses should be separated by commas.)
        </div>
        <div class="row-div">
            <asp:TextBox ID="EmailOrderTextBox" runat="server" >
            </asp:TextBox>
            <asp:Button CssClass="submitBtn" ID="EmailOrderButton" ValidationGroup="OrderconformationEmail"
                runat="server" Text="Send" OnClientClick="return clearLabelValue();" />
        </div>
        <div class="row-div">
            <asp:RegularExpressionValidator ID="regexEmailValid" runat="server" Display="Dynamic"
                ValidationExpression="^([A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}(,|$))+"
                ControlToValidate="EmailOrderTextBox" ErrorMessage="Invalid Email Format" ValidationGroup="OrderconformationEmail"
                ForeColor="Red">
            </asp:RegularExpressionValidator>
            <asp:Label ID="SendEmailLabel" runat="server">
            </asp:Label>
        </div>
    </div>
    <!-- End Changes for Issue 4893 -->
    <div class="clearfix border-all padding-all" id="tblRowMain" runat="server">
        <div class="float-left w10 OrderConfirmationNoFontHeader">
            <img runat="server" src="" id="companyLogo" />
        </div>
        <div class="float-left w47 ">
            <b>
                <asp:Label ID="lblcompanyAddress" runat="server" Text="" Font-Size="Small">
                </asp:Label>
            </b>
        </div>
        <div class="float-right w40">
            <div class="row-div clearfix">
                <div class="label-div w29">
                    Phone:
                </div>
                <div class="field-div1 w70">
                    (202)555-1234
                </div>
                <div class="clear"></div>
            </div>
            <div class="row-div clearfix">
                <div class="label-div w29">
                    Fax:
                </div>
                <div class="field-div1 w70">
                    (202)555-4321
                </div>
                <div class="clear"></div>
            </div>
        </div>
        <div class="clear"></div>
    </div>
</div>
<div class="row-div clearfix">
    <div class="float-left w47 padding-all">
        <div class="row-div clearfix">
            <div class="label-div w29">
                Order Number:
            </div>
            <div class="field-div1 w68">
                <asp:Label ID="lblOrderID" runat="server">
                </asp:Label>
            </div>
            <div class="clear"></div>
        </div>
        <div class="row-div clearfix">
            <div class="label-div w29">
                Order Type:
            </div>
            <div class="field-div1 w68">
                <asp:Label ID="lblOrderType" runat="server">
                </asp:Label>
            </div>
            <div class="clear"></div>
        </div>
         <div class="row-div clearfix" id="divOrderParty" runat="server">
            <div class="label-div w29">
                Order Party:
            </div>
            <div class="field-div1 w68">
                <asp:Label ID="lblOrderParty" runat="server">
                </asp:Label>
            </div>
            <div class="clear"></div>
        </div>
        <div class="row-div clearfix">
            <div class="label-div w29">
                Status:
            </div>
            <div class="field-div1 w68">
                <asp:Label runat="server" ID="lblOrderStatus">
                </asp:Label>
            </div>
            <div class="clear"></div>
        </div>
        <div class="row-div clearfix">
            <div class="label-div w29">
                Payment Method:
            </div>
            <div class="field-div1 w68">
                <asp:Label runat="server" ID="lblPayType">
                </asp:Label>
            </div>
            <div class="clear"></div>
        </div>
        <div class="row-div clearfix ">
            <div class="label-div w29">
                Bill To:
            </div>
            <div class="field-div1 w68">
                <uc1:NameAddressBlock ID="blkBillTo" runat="server"></uc1:NameAddressBlock>
            </div>
            <div class="clear"></div>
        </div>
    </div>
    <div class="float-right w47 padding-all">
        <div class="row-div clearfix ">
            <div class="label-div w29">
                Customer Number:
            </div>
            <div class="field-div1 w68">
                <asp:Label runat="server" ID="lblBillToID">
                </asp:Label>
            </div>
            <div class="clear"></div>
        </div>
        <div class="row-div clearfix">
            <div class="label-div w29">
                Shipment Method:
            </div>
            <div class="field-div1 w68">
                <asp:Label ID="lblShipType" runat="server">
                </asp:Label>
                <div>
                    <asp:Label ID="lblShipTrackingNum" runat="server">
                    </asp:Label>
                </div>
            </div>
            <div class="clear"></div>
        </div>
        <div class="row-div clearfix" id="divPaymentParty" runat="server">
            <div class="label-div w29">
                Payment Party: 
            </div>
            <div class="field-div1 w68">
                <asp:Label ID="lblPaymentParty" runat="server">
                </asp:Label>
            </div>
            <div class="clear"></div>
        </div>
        <div class="row-div clearfix ">
            <div class="label-div w29">
                Date Shipped:
            </div>
            <div class="field-div1 w68">
                <asp:Label ID="lblShipDate" runat="server">
                </asp:Label>
            </div>
            <div class="clear"></div>
        </div>
         
        <div class="row-div clearfix">
            <div class="label-div w29">
                Ship To: 
            </div>
            <div class="field-div1 w68">
                <uc1:NameAddressBlock ID="blkShipTo" runat="server"></uc1:NameAddressBlock>
            </div>
            <div class="clear"></div>
        </div>
    </div>
</div>
<div class="row-div">
    <asp:UpdatePanel ID="updPanelGrid" runat="server">
        <ContentTemplate>
            <rad:RadGrid ID="grdMain" runat="server" AutoGenerateColumns="False">
                <MasterTableView>
                    <Columns>
                        <rad:GridTemplateColumn HeaderText="Product">
                            <ItemTemplate>
                                <b>
                                    <asp:HyperLink runat="server" NavigateUrl="" ID="link" Text='
																	<%# DataBinder.Eval(Container, "DataItem.WebName") %>
																		'>
                                    </asp:HyperLink>
                                </b>
                            </ItemTemplate>
                        </rad:GridTemplateColumn>
                        <rad:GridBoundColumn DataField="Description" HeaderText="Description" />
                        <rad:GridTemplateColumn HeaderText="Quantity">
                            <HeaderStyle HorizontalAlign="Left" Width="57px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblQuantity" runat="server" Text='
																	<%# DataBinder.Eval(Container.DataItem,"Quantity") %>
																		'>
                                </asp:Label>
                            </ItemTemplate>
                        </rad:GridTemplateColumn>
                        <rad:GridTemplateColumn HeaderText="Unit Price">
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            <HeaderStyle CssClass="gridcolumnwidthorderprice" Width="60px" />
                            <ItemTemplate>
                                <asp:Label ID="lblPrice" runat="server">
                                </asp:Label>
                            </ItemTemplate>
                        </rad:GridTemplateColumn>
                        <rad:GridTemplateColumn HeaderText="Total Price">
                            <HeaderStyle HorizontalAlign="Right" CssClass="gridcolumnwidthorderTotalprice" Width="65px">
                            </HeaderStyle>
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblExtended" runat="server">
                                </asp:Label>
                            </ItemTemplate>
                        </rad:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </rad:RadGrid>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<div class="table-div" align="right">
    <div class="row-div clearfix">
        <div class="label-div w88">
            Sub-Total: 
        </div>
        <div class="float-right total-amount-right-margin">
            <asp:Label runat="server" ID="lblSubTotal">
            </asp:Label>
        </div>
        <div class="clear"></div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w88">
           Shipping/Handling:
        </div>
        <div class="float-right total-amount-right-margin">
            <asp:Label runat="server" ID="lblShipCharge">
            </asp:Label>
        </div>
        <div class="clear"></div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w88">
           Sales Tax: 
        </div>
        <div class="float-right total-amount-right-margin">
            <asp:Label runat="server" ID="lblSalesTax">
            </asp:Label>
        </div>
        <div class="clear"></div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w88">
            Grand Total:
        </div>
        <div class="float-right total-amount-right-margin">
            <asp:Label ID="lblGrandTotal" runat="server">
            </asp:Label>
        </div>
        <div class="clear"></div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w88">
          Payments: 
        </div>
        <div class="float-right total-amount-right-margin">
            <asp:Label ID="lblPayments" runat="server">
            </asp:Label>
        </div>
        <div class="clear"></div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w88">
           Balance: 
        </div>
        <div class="float-right total-amount-right-margin">
            <asp:Label ID="lblBalance" runat="server">
            </asp:Label>
        </div>
        <div class="clear"></div>
    </div>
</div>
<cc1:User runat="server" ID="User1" />
