<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ChapterMember.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.Chapters.ChapterMember" %>
<div class="chaptermain-div" id="tblMain" runat="server">
    <div class="row-div-bottom-line">
    <div class="control-title">
          Chapter Member
     </div>
     </div>
    <div class="row-div clearfix top-margin">
        <div class="label-div w19">
            <asp:Label ID="lblName" runat="server">
				Name:
            </asp:Label>
        </div>
        <div class="field-div1 common-input w79">
            <asp:TextBox AptifyDataField="FirstName" ID="txtFirstName" runat="server">
            </asp:TextBox>
            <asp:TextBox AptifyDataField="MiddleName" ID="txtMiddleName" runat="server" />
            <asp:TextBox AptifyDataField="LastName" ID="txtLastName" runat="server" />
        </div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w19">
            <asp:Label ID="lblTitle" runat="server">
				Title:
            </asp:Label>
        </div>
        <div class="field-div1 w79">
            <asp:TextBox AptifyDataField="Title" ID="txtTitle" runat="server" />
        </div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w19">
            <asp:Label ID="lblAddress" runat="server">
				Address:
            </asp:Label>
        </div>
        <div class="field-div1 w79">
            <asp:TextBox AptifyDataField="AddressLine1" ID="txtAddressLine1" runat="server">
            </asp:TextBox>
            <br />
            <asp:TextBox ID="txtAddressLine2" AptifyDataField="AddressLine2" runat="server">
            </asp:TextBox>
            <br />
            <asp:TextBox ID="txtAddressLine3" AptifyDataField="AddressLine3" runat="server">
            </asp:TextBox>
        </div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w19">
            <asp:Label ID="lblCityStateZip" runat="server">
				City, State ZIP:
            </asp:Label>
        </div>
        <div class="field-div1 common-input w79">
            <asp:TextBox AptifyDataField="City" ID="txtCity" runat="server">
            </asp:TextBox>
            <asp:DropDownList AptifyDataField="State" AptifyListTextField="State"
                AptifyListValueField="State" ID="cmbState" runat="server" DataValueField="State"
                DataTextField="State" />
            <asp:TextBox AptifyDataField="ZipCode" ID="txtZipCode" runat="server" />
        </div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w19">
            <asp:Label ID="lblCountry" runat="server">
				Country:
            </asp:Label>
        </div>
        <div class="field-div1 w79">
            <asp:DropDownList AptifyDataField="CountryCodeID" AptifyListTextField="Country"
                AptifyListValueField="ID" ID="cmbCountry" runat="server" DataValueField="ID"
                DataTextField="Country" AutoPostBack="true" />
        </div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w19">
            <asp:Label ID="lblPhone" runat="server">
				Phone:
            </asp:Label>
        </div>
        <div class="field-div1 common-input w79">
            <asp:TextBox AptifyDataField="PhoneCountryCode" ID="txtPhoneCountryCode" runat="server"
                 />
            <asp:TextBox AptifyDataField="PhoneAreaCode" ID="txtPhoneAreaCode" runat="server"
                 />
            <asp:TextBox AptifyDataField="Phone" ID="txtPhone" runat="server" />
        </div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w19">
            <asp:Label ID="lblFax" runat="server">
				Fax:
            </asp:Label>
        </div>
        <div class="field-div1 common-input w79">
            <asp:TextBox AptifyDataField="FaxCountryCode" ID="txtFaxCountryCode" runat="server"/>
            <asp:TextBox AptifyDataField="FaxAreaCode" ID="txtFaxAreaCode" runat="server" />
            <asp:TextBox AptifyDataField="FaxPhone" ID="txtFaxPhone" runat="server" />
        </div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w19">
            <asp:Label ID="lblEmail" runat="server">
				Email:
            </asp:Label>
        </div>
        <div class="field-div1 w79">
            <asp:TextBox AptifyDataField="Email1" ID="Email1" runat="server" />
            <asp:RegularExpressionValidator ID="regexEmailValid" runat="server" Display="Dynamic"
                ValidationExpression="[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+(?:[A-Z]{2}|com|COM|org|ORG|net|NET|edu|EDU|gov|GOV|mil|MIL|biz|BIZ|info|INFO|mobi|MOBI|name|NAME|aero|AERO|asia|ASIA|jobs|JOBS|museum|MUSEUM|in|IN|co|CO)\b"
                ControlToValidate="Email1" ErrorMessage="Invalid Email Format" ForeColor="Red">
            </asp:RegularExpressionValidator>
        </div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w19">
            &nbsp;
        </div>
        <div class="field-div1 w79">
            <asp:Button ID="cmdSave" Text="Save" runat="server" CssClass="submit-Btn" />
        </div>
    </div>
     <div class="row-div clearfix">
        <div class="label-div w19">
           &nbsp;
        </div>
        <div class="errormsg-div w79">
            <asp:Label ID="lblError" runat="server" Visible="False" />
        </div>
    </div>
</div>
