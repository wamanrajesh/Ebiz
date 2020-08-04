<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ChapterEdit.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.Chapters.ChapterEditControl" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="uc1" TagName="ChapterMember" Src="ChapterMember.ascx" %>
<%@ Register TagPrefix="uc1" TagName="NameAddressBlock" Src="../Aptify_General/NameAddressBlock.ascx" %>

   <div class="chaptermain-div clearfix">
	<div class="row-div-bottom-line" id="trHeader" runat="server">
    <div class="control-title">
		    <asp:label id="lblChapterName" Runat="server">
				Edit Chapter
			</asp:label>
            </div>
    </div>
	<div class="row-div clearfix">
			<asp:Label ID="lblError" Runat="server" Visible="False">
			</asp:Label>
	</div>
	<div class="row-div clearfix">
		<div class="label-div w19">
			<asp:Label ID="lblName" Runat="server">
				Chapter Name
			</asp:Label>
		</div>
		<div class="field-div1 w79">
			<asp:TextBox ID="txtName" AptifyDataField="Name" Runat="server">
			</asp:TextBox>
		</div>
	</div>
	<div class="row-div clearfix">
        <div class="label-div w19">
            <asp:Label ID="lblAddress" runat="server">
				Address
            </asp:Label>
        </div>
        <div class="field-div1 w79">
            <asp:TextBox AptifyDataField="AddressLine1" ID="txtAddressLine1" runat="server">
            </asp:TextBox>
        </div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div emptyspace w19">
            &nbsp;
        </div>
        <div class="field-div1 w79">
            <asp:TextBox ID="txtAddressLine2" AptifyDataField="AddressLine2" runat="server">
            </asp:TextBox>
        </div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div emptyspace w19">
            &nbsp;
        </div>
        <div class="field-div1 w79">
            <asp:TextBox ID="txtAddressLine3" AptifyDataField="AddressLine3" runat="server">
            </asp:TextBox>
        </div>
    </div>
	<div class="row-div  clearfix ">
		<div class="label-div w19">
			<asp:Label ID="lblCityStateZip" Runat="server">
				City, State ZIP
			</asp:Label>
		</div>
		<div class="field-div1 common-input w79">
			<asp:TextBox AptifyDataField="City" id="txtCity" Runat="server">
			</asp:TextBox>
			<asp:dropdownlist AptifyDataField="State" AptifyListTextField="State" AptifyListValueField="State" id="cmbState" Runat="server" DataValueField="State" DataTextField="State" />
			<asp:TextBox AptifyDataField="ZipCode" id="txtZipCode" Runat="server"/>
		</div>
	</div>
	<div class="row-div clearfix">
		<div class="label-div w19">
			<asp:Label ID="lblCountry" Runat="server">
				Country
			</asp:Label>
		</div>
		<div class="field-div1 dropdown w79">
			<asp:dropdownlist  AptifyDataField="CountryCodeID" AptifyListTextField="Country" AptifyListValueField="ID" id="cmbCountry" Runat="server" DataValueField="ID" DataTextField="Country" AutoPostBack="true" />
		</div>
	</div>
	<div class="row-div clearfix">
		<div class="label-div w19">
			<asp:Label ID="lblTaxID" Runat="server">
				Tax ID
			</asp:Label>
		</div>
		<div class="field-div1 w79">
			<asp:TextBox ID="txtTaxID" AptifyDataField="FedTaxID" Runat="server">
			</asp:TextBox>
		</div>
	</div>
	<div class="row-div clearfix" id="trFooter" runat="server">
   <div class="label-div w19">
                &nbsp;</div>
		<div class="field-div1 w79">
			<asp:Button id="cmdSave" runat="server" Text="Save" CssClass="submit-Btn">
			</asp:Button>
			<asp:Button ID="cmdChapter" runat="server" Text="Go To Chapter" CssClass="submit-Btn" />
		</div>
	</div>
    <cc3:user id="User1" runat="server" />
</div>


