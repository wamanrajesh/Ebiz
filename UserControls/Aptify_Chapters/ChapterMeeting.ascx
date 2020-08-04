<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ChapterMeeting.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.Chapters.ChapterMeetingControl" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="cc5" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessShoppingCart" %>
<%@ Register TagPrefix="uc1" TagName="ChapterMember" Src="ChapterMember.ascx" %>
<%@ Register TagPrefix="uc1" TagName="NameAddressBlock" Src="../Aptify_General/NameAddressBlock.ascx" %>
<div class="chaptermain-div clearfix" >
	<div class="errormsg-div clearfix">
			<asp:Label ID="lblError" runat="server">
			</asp:Label>
	</div>
     <div class="row-div-bottom-line">
	<div class="control-title">
			<asp:Label ID="lblChapterName" runat="server">
			</asp:Label>
	</div>
    </div>
	<div class="row-div clearfix">
		<div class="label-div w19 padding-top">
        <asp:Label ID="lblchapter" runat="server" CssClass="required-label">
			*
			</asp:Label>
			<asp:Label ID="lblName" runat="server">
				Name:
			</asp:Label>
		</div>
		<div class="field-div1 w79">
			<asp:TextBox ID="txtName" runat="server">
			</asp:TextBox>
		</div>
	</div>
	<div class="row-div clearfix">
		<div class="label-div w19 padding-top">
			<asp:Label ID="lblDescription" runat="server">
				Description:
			</asp:Label>
		</div>
		<div class="field-div1 w79">
			<asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="txt-restrict-resize">
			</asp:TextBox>
		</div>
	</div>
	<div class="row-div clearfix">
		<div class="label-div w19 padding-top">
			<asp:Label ID="lblType" runat="server">
				Type:
			</asp:Label>
		</div>
		<div class="field-div1 w79 ">
			<asp:DropDownList ID="cmbType" runat="server">
				<asp:ListItem Value="One-Time">One-Time</asp:ListItem>
				<asp:ListItem Value="Recurring">Recurring</asp:ListItem>
			</asp:DropDownList>
		</div>
	</div>
	<div class="row-div clearfix">
		
		<div class="label-div w19 padding-top">
			<asp:Label ID="lblStartDate" runat="server">
				Start Date/Time:
			</asp:Label>
		</div>
		<div class="field-div1 w79">
			<asp:TextBox ID="txtStartDate" runat="server">
			</asp:TextBox>
				    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="validation-message" runat="server" Display="Dynamic" ControlToValidate="txtStartDate" ErrorMessage="Please enter a valid Start Date/Time.">
				    </asp:RequiredFieldValidator>
					<asp:CustomValidator ID="vldStartDate" CssClass="validation-message" runat="server" Display="Dynamic" ControlToValidate="txtStartDate" ErrorMessage="Please enter a valid Start Date/Time.">
					</asp:CustomValidator>
				</div>
			</div>
			<div class="row-div clearfix">
				<div class="label-div w19 padding-top">
					<asp:Label ID="lblEndDate" runat="server">
						End Date/Time:
					</asp:Label>
				</div>
				<div class="field-div1 w79">
					<asp:TextBox ID="txtEndDate" runat="server">
					</asp:TextBox>
						<asp:CustomValidator ID="vldEndDate" runat="server" CssClass="validation-message" Display="Dynamic" ControlToValidate="txtEndDate" ErrorMessage="Please enter a valid End Date/Time.">
						</asp:CustomValidator>
					</div>
				</div>
				<div class="row-div clearfix">
					<div class="label-div w19 padding-top">
						<asp:Label ID="lblStatus" runat="server">
							Status:
						</asp:Label>
					</div>
					<div class="field-div1 w79">
						<asp:DropDownList ID="cmbStatus" runat="server">
							<asp:ListItem Value="Planned">Planned</asp:ListItem>
							<asp:ListItem Value="Completed">Completed</asp:ListItem>
							<asp:ListItem Value="Cancelled">Cancelled</asp:ListItem>
						</asp:DropDownList>
					</div>
				</div>
				<div class="row-div clearfix">
					<div class="label-div w19 padding-top">
						<asp:Label ID="lblLocation" runat="server">
							Location:
						</asp:Label>
					</div>
					<div class="field-div1 w79">
						<asp:TextBox ID="txtLocation" runat="server">
						</asp:TextBox>
					</div>
				</div>
				<div class="row-div clearfix">
					<div class="label-div w19 padding-top">
						<asp:Label ID="lblAddress1" runat="server">
							Address:
						</asp:Label>
					</div>
					<div class="field-div1 w79">
						<asp:TextBox ID="txtAddressLine1" runat="server">
						</asp:TextBox>
					</div>
				</div>
				<div class="row-div clearfix">
					<div class="label-div w19 padding-top">
						<asp:Label ID="lblCityStateZip" runat="server">
							City, State ZIP:
						</asp:Label>
					</div>
					<div class="field-div1 common-input w79">
						<asp:TextBox ID="txtCity" runat="server">
						</asp:TextBox>
						<asp:DropDownList ID="cmbState" runat="server" DataTextField="State" DataValueField="State">
						</asp:DropDownList>
						<asp:TextBox ID="txtZIP" runat="server">
						</asp:TextBox>
					</div>
				</div>
				<div class="row-div clearfix">
					<div class="label-div w19 padding-top">
						<asp:Label ID="lblCountry" runat="server">
							Country:
						</asp:Label>
					</div>
					<div class="field-div1 w79">
						<asp:DropDownList ID="cmbCountry" runat="server" DataTextField="Country" DataValueField="ID" AutoPostBack="True">
						</asp:DropDownList>
					</div>
				</div>
				<div class="row-div clearfix">
                	<div class="label-div w19">&nbsp;</div>
					<div class="field-div1 w79">
						<asp:Button ID="cmdSave" runat="server" Text="Save" CssClass="submit-Btn">
						</asp:Button>
						<asp:LinkButton ID="lnkChapter" runat="server">
							Go To Chapter
						</asp:LinkButton>
					</div>
				</div>
			</div>

		 <cc5:AptifyShoppingCart runat="server" ID="ShoppingCart1" />
    <cc3:User ID="User1" runat="server" />

