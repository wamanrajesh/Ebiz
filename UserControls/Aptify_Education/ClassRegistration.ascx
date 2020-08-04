<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ClassRegistration.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.Education.ClassRegistrationControl" %>
<%@ Register Src="../Aptify_General/CreditCard.ascx" TagName="CreditCard" TagPrefix="uc1" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="cc4" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessShoppingCart" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%--Amruta , Issue 15457, 4/24/2013, Function to hide label on click delete button of grid --%>
<script type="text/javascript" language="javascript">
    function HideLabel() {
        document.getElementById('<%=lblMsg.ClientID%>').style.display = 'none';
    }
</script>
<div id="tblMain" class="table-div" runat="server">
    <%--<div class="row-div clearfix border-all" >
			 Class Registration
	</div>--%>
    <div class="row-div header-title-bottom-border">
        <asp:Label ID="lblInstructions" runat="server">
        </asp:Label>
    </div>

    <div class="float-left w12 right-margin padding-left-right" style="background:#ebd4ba;">
        <div class="row-div clearfix ">
            <div class="label">
                Class
            </div>
            <div >
                <asp:HyperLink Style="text-decoration: underline" runat="server" ID="lnkClassNum">
                    <asp:Label ID="lblClassNum" runat="server" />
                </asp:HyperLink>
                <asp:HiddenField ID="lblProductID" runat="Server" />
            </div>
        </div>
        <div class="row-div clearfix ">
            <div class="label">
                Course
            </div>
            <div >
                <asp:Label ID="lblCourse" runat="server" />
            </div>
        </div>
        <div class="row-div clearfix ">
            <div class="label">
                Type
            </div>
            <div>
                <asp:Label ID="lblType" runat="server" />
            </div>
        </div>
        <div class="row-div clearfix ">
            <div class="label">
                Start Date
            </div>
            <div>
                <asp:Label ID="lblStartDate" runat="server" />
            </div>
        </div>
        <div class="row-div clearfix ">
            <div class="label">
                End Date
            </div>
            <div>
                <asp:Label ID="lblEndDate" runat="server" />
            </div>
        </div>
        <div class="row-div clearfix " runat="server" id="trInstructor">
            <div class="label">
                Instructor
            </div>
            <div>
                <asp:Label ID="lblInstructor" runat="server" />
            </div>
        </div>
        <div class="row-div clearfix ">
            <div class="label">
                Location
            </div>
            <div>
                <asp:Label ID="lblLocation" runat="server" />
            </div>
        </div>
        <div class="row-div clearfix ">
            <div class="label">
               Price
            </div>
            <div>
                <asp:Label ID="lblPrice" runat="server" />
            </div>
        </div>
    </div>
    <div class="float-right w85">
        <asp:Panel ID="CreaditCardInfo" runat="server">
            Enter class registation payment information below
            <br />
            <uc1:creditcard id="CreditCard" runat="server" showpaymenttypeselection="true" />
            <br />
        </asp:Panel>
        <asp:Panel ID="RegistrationGrid" runat="server">
            <b>Registrants </b>
            <asp:UpdatePanel ID="updPanelGrid" runat="server">
                <ContentTemplate>
                    <rad:radgrid id="grdStudents" runat="server" autogeneratecolumns="false">
							<MasterTableView>
								<Columns>
									<rad:GridTemplateColumn HeaderText="First Name" DataField="FirstName" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
										<ItemTemplate>
											<asp:TextBox ID="txtFirstName" runat="server" Text='
											<%# DataBinder.Eval(Container.DataItem,"FirstName") %>
												'>
												</asp:TextBox>
											</ItemTemplate>
										</rad:GridTemplateColumn>
										<rad:GridTemplateColumn HeaderText="Last Name" DataField="LastName" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" >
											<ItemTemplate>
												<asp:TextBox ID="txtLastName"  runat="server" Text='
												<%# DataBinder.Eval(Container.DataItem,"LastName") %>
													'>
													</asp:TextBox>
												</ItemTemplate>
											</rad:GridTemplateColumn>
											<rad:GridTemplateColumn HeaderText="Title" DataField="Title" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
												<ItemTemplate>
													<asp:TextBox ID="txtTitle" runat="server" Text='
													<%# DataBinder.Eval(Container.DataItem,"Title") %>
														'>
														</asp:TextBox>
													</ItemTemplate>
												</rad:GridTemplateColumn>
												<rad:GridTemplateColumn HeaderText="Email" DataField="Email" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
													<ItemTemplate>
														<asp:TextBox ID="txtEmail" runat="server" Text='
														<%# DataBinder.Eval(Container.DataItem,"Email") %>
															'>
															</asp:TextBox>
														</ItemTemplate>
													</rad:GridTemplateColumn>
													<rad:GridTemplateColumn HeaderText="Delete" AllowFiltering="false">
														<ItemTemplate>
															<asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/Delete.png" CommandName="Delete" OnClientClick="HideLabel()" CommandArgument="
															<%# CType(Container, GridDataItem ).RowIndex %>
																" />
															</ItemTemplate>
														</rad:GridTemplateColumn>
													</Columns>
												</MasterTableView>
											</rad:radgrid>
                </ContentTemplate>
            </asp:UpdatePanel>
            <br />
            <asp:Button ID="btnMoreRows" runat="server" CssClass="submit-Btn" Text="Add New Row" />
            <asp:Button runat="server" ID="btnSaveRegistration" Text="Submit Registration" CssClass="submit-Btn" />
        </asp:Panel>
        <center>
            <asp:Button runat="server" ID="btnSaveRegistrationPaid" Text="Submit Registration"
                CssClass="submit-Btn" />
        </center>
        <br />
        <asp:Label ID="lblMsg" runat="server" CssClass="error-msg-label" />
    </div>
    <div class="clear"></div>
</div>

<!--<asp:Label ID="lblError" runat="server" ForeColor="Maroon" Visible="False" />-->
<cc3:user runat="server" id="User1" />
<cc4:aptifyshoppingcart id="ShoppingCart1" runat="server" />
