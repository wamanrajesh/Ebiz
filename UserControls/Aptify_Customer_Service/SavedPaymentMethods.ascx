<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SavedPaymentMethods.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.CustomerService.SavedPaymentMethods" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessShoppingCart" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<div class="table-div">
    <div class="row-div">
        <rad:radgrid id="grdSPM" runat="server" autogeneratecolumns="False" allowpaging="true"
            sortingsettings-sorteddesctooltip="Sorted Descending" sortingsettings-sortedasctooltip="Sorted Ascending"
            pagerstyle-pagesizelabeltext="Records Per Page">
					<GroupingSettings CaseSensitive="false" />
					
						<MasterTableView AllowSorting="True" NoMasterRecordsText="No Saved Payment Methods Available." AllowNaturalSort="false">
							
								<Columns>
									<rad:GridTemplateColumn HeaderText="Payment Type" DataField="PaymentType" SortExpression="PaymentType">
										<ItemTemplate>
											<asp:Label Visible="false" ID="lblPaymentTypeID" runat="server" Text='
											<%# DataBinder.Eval(Container.DataItem,"PaymentTypeID") %>
												'>
												</asp:Label>
												<asp:Label ID="lblPaymentType" runat="server" Text='
												<%# DataBinder.Eval(Container.DataItem,"PaymentType") %>
													'>
													</asp:Label>
												</ItemTemplate>
											</rad:GridTemplateColumn>
											<rad:GridTemplateColumn Visible="false">
												<ItemTemplate>
													<asp:Label ID="lblPersonID" runat="server" Text='
													<%# DataBinder.Eval(Container.DataItem,"PersonID") %>
														'>
														</asp:Label>
													</ItemTemplate>
												</rad:GridTemplateColumn>
												
													<rad:GridTemplateColumn HeaderText="Card Nickname" DataField="Name" SortExpression="Name">
														<ItemTemplate>
															<asp:Label ID="lblNameonCard" runat="server" Text='
															<%# DataBinder.Eval(Container.DataItem,"Name") %>
																'>
																</asp:Label>
															</ItemTemplate>
														</rad:GridTemplateColumn>
														<rad:GridTemplateColumn HeaderText="Card Number" DataField="CCPartial" SortExpression="CCPartial">
															<ItemTemplate>
																<asp:Label ID="lblCCPartial" runat="server" Text='
																<%# DataBinder.Eval(Container.DataItem,"CCPartial") %>
																	'>
																	</asp:Label>
																</ItemTemplate>
															</rad:GridTemplateColumn>
															
																<rad:GridTemplateColumn HeaderText="Expires on" DataField="ExpireOnDate" SortExpression="ExpireOnDate">
																	<ItemTemplate>
																		<asp:Label ID="lblExpireOnDate" runat="server" Text='
																		<%# DataBinder.Eval(Container.DataItem,"ExpireOn") %>
																			'>
																			</asp:Label>
																		</ItemTemplate>
																	</rad:GridTemplateColumn>
																	<rad:GridTemplateColumn Visible="false">
																		<ItemTemplate>
																			<asp:Label ID="lblCCAccountNumber" runat="server" Text='
																			<%# DataBinder.Eval(Container.DataItem,"CCAccountNumber") %>
																				'>
																				</asp:Label>
																				<asp:Label ID="lblID" runat="server" Text='
																				<%# DataBinder.Eval(Container.DataItem,"ID") %>
																					'>
																					</asp:Label>
																				</ItemTemplate>
																			</rad:GridTemplateColumn>
																			<rad:GridTemplateColumn>
																				<ItemTemplate>
																					<asp:ImageButton runat="server" ID="ImgEdit" CommandName="Update" ToolTip="Edit" CommandArgument='
																					<%# DataBinder.Eval(Container.DataItem,"ID") %>
																						' CausesValidation="false" />
																					</ItemTemplate>
																				</rad:GridTemplateColumn>
																				<rad:GridTemplateColumn>
																					<ItemTemplate>
																						<asp:ImageButton runat="server" ID="imgDelete" CommandName="Delete" ToolTip="Delete" CausesValidation="false" CommandArgument='
																						<%# DataBinder.Eval(Container.DataItem,"ID") %>
																							' />
																						</ItemTemplate>
																					</rad:GridTemplateColumn>
																				</Columns>
																			</MasterTableView>
																		</rad:radgrid>
    </div>
    <div class="row-div">
        <div class="align-right">
            <asp:Button CssClass="submit-Btn" runat="server" ID="btnAddNewCard" Text="Add New Card"
                CausesValidation="false" />
        </div>
    </div>
</div>
<rad:radwindow id="CreditcardWindow" runat="server" visibleonpageload="false" modal="true"
    behaviors="Move" title="Add New Card" CssClass="popup-customerservice-savepaymentmethod"  visiblestatusbar="false" skin="Default" iconurl="">
    <ContentTemplate>
        
        
<div id="tblAddCard" runat="server" class="table-div">
	<div class="row-div clearfix">
		
			<div class="label-div">
				Enter your card information:
			</div>	
		
	</div>
	<div class="row-div clearfix">
		<div class="label-div w28">
			Credit Card:			
		</div>
		<div class="field-div1 w71">
			<asp:DropDownList ID="cmbCreditCard" runat="server" AppendDataBoundItems="True" Visible="false" CausesValidation="false">
			</asp:DropDownList>
			
						<rad:RadBinaryImage ID="ImgVisa" runat="server" CssClass="creditcardImg" AutoAdjustImageControlSize="false">
						</rad:RadBinaryImage>
			
			
						<rad:RadBinaryImage ID="ImgMasterCard" runat="server" CssClass="creditcardImg" AutoAdjustImageControlSize="false">
						</rad:RadBinaryImage>
			
			
						<rad:RadBinaryImage ID="ImgAmericanExpress" runat="server" CssClass="creditcardImg" AutoAdjustImageControlSize="false">
						</rad:RadBinaryImage>
			
			
						<rad:RadBinaryImage ID="ImgDiscover" runat="server" CssClass="creditcardImg" AutoAdjustImageControlSize="false">
						</rad:RadBinaryImage>
					
				</div>
			</div>
	
	
	<div class="row-div clearfix">
		<div class="label-div w28">
			<em class="required-label">
				*
			</em>
			
				Card Number:
			
		</div>
		<div class="field-div1 w71">
			<asp:TextBox ID="txtCCNumber" runat="server" AutoComplete="Off" AutoPostBack="true" EnableViewState="False" CausesValidation="false">
			</asp:TextBox>
			<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCCNumber" Enabled="True" ErrorMessage="Required" CssClass="error-msg-label" Display="Dynamic">
			</asp:RequiredFieldValidator>
			<br />
		</div>
	</div>
	<div class="row-div clearfix">
		<div class="label-div w28">
			
				<em class="required-label">
					*
				</em>
				Card Nickname:
				
			</div>
			<div class="field-div1 w71">
				<asp:TextBox runat="server" ID="txtName">
				</asp:TextBox>
				<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtName" Enabled="True" ErrorMessage="Required" CssClass="error-msg-label" Display="Dynamic">
				</asp:RequiredFieldValidator>
			</div>
		</div>
		<div class="row-div clearfix">
			<div class="label-div w28">
				<em class="required-label">
					*
				</em>
				Security # :
				
			</div>
			<div class="field-div1 w71">
				<asp:TextBox ID="txtCCSecurityNumber" runat="server" Width="20%" EnableViewState="false" AutoComplete="Off" MaxLength="3">
				</asp:TextBox>
				<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Required" ControlToValidate="txtCCSecurityNumber" Enabled="True" CssClass="error-msg-label" Display="Dynamic">
				</asp:RequiredFieldValidator>
			</div>
		</div>
		<div class="row-div clearfix">
			<div class="label-div w28">
				Expiration Date:							</div>
			<div class="field-div1 w71">
				<asp:DropDownList ID="dropdownMonth" runat="server">
					<asp:ListItem Value="1">
						January
					</asp:ListItem>
					<asp:ListItem Value="2">
						February
					</asp:ListItem>
					<asp:ListItem Value="3">
						March
					</asp:ListItem>
					<asp:ListItem Value="4">
						April
					</asp:ListItem>
					<asp:ListItem Value="5">
						May
					</asp:ListItem>
					<asp:ListItem Value="6">
						June
					</asp:ListItem>
					<asp:ListItem Value="7">
						July
					</asp:ListItem>
					<asp:ListItem Value="8">
						August
					</asp:ListItem>
					<asp:ListItem Value="9">
						September
					</asp:ListItem>
					<asp:ListItem Value="10">
						October
					</asp:ListItem>
					<asp:ListItem Value="11">
						November
					</asp:ListItem>
					<asp:ListItem Value="12">
						December
					</asp:ListItem>
				</asp:DropDownList>
				<asp:DropDownList ID="dropdownDay" runat="server" Visible="False">
				</asp:DropDownList>
				<asp:DropDownList ID="dropdownYear" runat="server" >
				</asp:DropDownList>
				<asp:CustomValidator ID="vldExpirationDate" runat="server" ControlToValidate="dropdownDay" ErrorMessage="Invalid Date" Display="Dynamic" CssClass="error-msg-label">
				</asp:CustomValidator>
			</div>
		</div>
		<div class="row-div">
			
				<asp:Label ID="lblError" CssClass="error-msg-label" runat="server">
				</asp:Label>
			
		</div>
		<div class="row-div clearfix">
        <div class="label-div w28">&nbsp;</div>
			<div class="field-div1 w71">
				
				<asp:HiddenField ID="SPMID" runat="server" />
				<asp:Button class="submitBtn" runat="server" ID="btnAdd" Text="Add your card" OnClick="btnAdd_Click" />
				<asp:Button class="submitBtn" runat="server" ID="btnCancel" Text="Cancel" CausesValidation="false" />
                </div>
			
		</div>
	</div>
	 
        
    </ContentTemplate>
</rad:radwindow>
<cc3:user id="User1" runat="server" />
<cc2:aptifyshoppingcart id="ShoppingCart1" runat="server" visible="False" />
