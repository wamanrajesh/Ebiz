<%--Aptify e-Business 5.5.1, July 2013--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="StandardProfile.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.CustomerService.StandardProfileControl" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessLogin" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="cc4" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessShoppingCart" %>
    
   <div class="AptifyControl">
<table>
 <tr>
  <th colspan="2">
     Personal Information&nbsp;</th>
 </tr>
    <tr>
        <td colspan="2">
	<asp:validationsummary id="ValidationSummary2" runat="server"></asp:validationsummary>
	<asp:label id="Label3" runat="server" Visible="False"></asp:label>
	<asp:label id="Label2" runat="server"></asp:label>
        </td>
    </tr>

	    <tr>
		    <td><asp:label id="lblName" Runat="server">First Name</asp:label></td>
		    <td><asp:textbox id="txtFirstName" Runat="server"></asp:textbox>
		    <asp:requiredfieldvalidator id="RequiredFieldValidator3" runat="server" ControlToValidate="txtFirstName" ErrorMessage="First Name Required" Display="Dynamic"></asp:requiredfieldvalidator></td>
	    </tr>
	    <tr>
	        <td><asp:label id="Label4" Runat="server">Last Name</asp:label></td>
	        <td><asp:textbox id="txtLastName" Runat="server"></asp:textbox>
	        <asp:requiredfieldvalidator id="RequiredFieldValidator2" runat="server" ControlToValidate="txtLastName" ErrorMessage="Last Name Required" Display="Dynamic"></asp:requiredfieldvalidator></td>
	    </tr>
	    <tr>
		    <td><asp:label id="lblCompany" Runat="server">Company</asp:label></td>
		    <td><asp:textbox id="txtCompany" Runat="server"></asp:textbox></td>
	    </tr>
	    <tr>
		    <td><asp:label id="lblTitle" Runat="server">Title</asp:label></td>
		    <td><asp:textbox id="txtTitle" Runat="server"></asp:textbox></td>
	    </tr>
	    <tr>
		    <td><asp:label id="lblPrimaryFunction" Runat="server">Primary Job Function</asp:label></td>
		    <td><asp:dropdownlist id="cmbPrimaryFunction" Runat="server"></asp:dropdownlist></td>
	    </tr>
	   </table>
	 </div>
	   <div class="AptifyControl">
	   <table>	   
	   <tr>
	        <th colspan="2">Contact Information</th>
	   </tr> 
	    <tr>
		    <td><asp:label id="lblAddress" Runat="server">Address</asp:label></td>
		    <td><asp:textbox id="txtAddressLine1" Runat="server"></asp:textbox></td>
	
			    
	    </tr>
	    <tr>
	    <td></td>
	    <td><asp:textbox id="txtAddressLine2" Runat="server"></asp:textbox></td>
	    </tr>
	    
	    <tr>
	        <td></td>
	        <td><asp:textbox id="txtAddressLine3" Runat="server"></asp:textbox></td>
	    </tr>
	    <tr>
		    <td><asp:label id="lblCityStateZip" Runat="server">City</asp:label></td>
		    <td><asp:textbox id="txtCity" Runat="server"></asp:textbox></td>
	    </tr>
	    <tr>
	        <td><asp:label id="Label5" Runat="server">State</asp:label></td>
	        <td><asp:dropdownlist id="cmbState" Runat="server"></asp:dropdownlist></td>
	    </tr>
	    <tr>
	        <td><asp:label id="Label6" Runat="server">Zipcode</asp:label></td>
	        <td><asp:textbox id="txtZipCode" Runat="server"></asp:textbox></td>
	    </tr>
	    <tr>
		    <td><asp:label id="lblCountry" Runat="server">Country</asp:label></td>
		    <td><asp:dropdownlist id="cmbCountry" Runat="server" AutoPostBack="true" ></asp:dropdownlist></td>
	    </tr>
	    <tr>
		    <td><asp:label id="lblPhone" Runat="server">(Area Code) Phone</asp:label></td>
		    <td><asp:textbox CssClass="SmallWidthControl" id="txtPhoneAreaCode" Runat="server"></asp:textbox><asp:textbox id="txtPhone" CssClass="MediumWidthControl" Runat="server"></asp:textbox></td>
	    </tr>
	    <tr>
		    <td><asp:label id="lblFax" Runat="server">(Area Code) Fax</asp:label></td>
		    <td><asp:textbox id="txtFaxAreaCode" CssClass="SmallWidthControl" Runat="server"></asp:textbox><asp:textbox id="txtFaxPhone" Runat="server" CssClass="MediumWidthControl"></asp:textbox></td>
	    </tr>
	    <tr>
		    <td><asp:label id="lblEmail" Runat="server">Email</asp:label></td>
		    <td><asp:textbox id="txtEmail" Runat="server"></asp:textbox><asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email Required" Display="Dynamic"></asp:requiredfieldvalidator></td>
	    </tr>
	    <tr>
		    <td colspan="2"></td>
	    </tr>
	    </table>
	    </div>
	    <div class="AptifyControl">
	    <table>
	    <tr>
	        <th colspan="2">Account Information</th>
	        </tr>
	    <tr>
		    <td><asp:label id="lblWebUID" Runat="server">User ID</asp:label></td>
		    <td><asp:textbox id="txtUserID" Runat="server"></asp:textbox>
			<asp:requiredfieldvalidator id="RequiredFieldValidator4" runat="server" ControlToValidate="txtUserID" ErrorMessage="User ID Required" Display="Dynamic"></asp:requiredfieldvalidator></td>
	    </tr>
	    <tr>
		    <td><asp:label id="lblPWD" Runat="server">Password</asp:label></td>
		    <td><asp:textbox id="txtPassword" Runat="server" TextMode="Password"></asp:textbox>
			<asp:comparevalidator id="valPWDMatch" runat="server" ControlToValidate="txtRepeatPWD" ErrorMessage="Passwords Must Match" ControlToCompare="txtPassword" Display="Dynamic"></asp:comparevalidator></td>
	    </tr>
	    <tr>
		    <td><asp:label id="lblRepeatPWD" Runat="server">Repeat Password</asp:label></td>
		    <td><asp:textbox id="txtRepeatPWD" Runat="server" TextMode="Password"></asp:textbox>
			<asp:requiredfieldvalidator id="valPWDRequired" runat="server" ControlToValidate="txtPassword" ErrorMessage="Password Required" Display="Dynamic"></asp:requiredfieldvalidator></td>
	    </tr>
	    <tr>
		    <td><asp:label id="lblPasswordHintQuestion" Runat="server">Password Hint Question</asp:label></td>
		    <td><asp:dropdownlist id="cmbPasswordQuestion" Runat="server">
				    <asp:ListItem Value="My favorite color is?" Selected="True">My favorite color is?</asp:ListItem>
				    <asp:ListItem Value="My mother's maiden name is?">My mother's maiden name is?</asp:ListItem>
				    <asp:ListItem Value="I went to which high school?">I went to which high school?</asp:ListItem>
				    <asp:ListItem Value="I was born in which city?">I was born in which city?</asp:ListItem>
				    <asp:ListItem Value="My pet's name is?">My pet's name is?</asp:ListItem>
			    </asp:dropdownlist></td>
	    </tr>
	    <tr>
		    <td><asp:label id="lblPasswordHintAnswer" Runat="server">Password Hint Answer</asp:label></td>
		    <td><asp:textbox id="txtPasswordHintAnswer" Runat="server" Width="311"></asp:textbox>
			<asp:requiredfieldvalidator id="valPasswordHintRequired" runat="server" ControlToValidate="txtPasswordHintAnswer"
				    ErrorMessage="Hint Answer Required" Display="Dynamic"></asp:requiredfieldvalidator></td>
	    </tr>
	    <tr>
		    <td colspan="2">
                <asp:Label ID="lblError" runat="server"></asp:Label></td>
	    </tr>
	    <tr>
		    <td colspan="2"><asp:button id="cmdSave" runat="server" Text="Submit" Visible="false" CssClass="submitBtn"></asp:button></td>
	    </tr>
    </table>
</div>
<table width="90%">
    <tr>
        <td valign="top" align="right">
            <asp:LinkButton ID="LinkButton1" runat="server">Cancel</asp:LinkButton>&nbsp;
            <asp:LinkButton ID="LinkButton2" runat="server">Submit</asp:LinkButton>
        </td>
    </tr>
</table>    
    <cc1:User id="User1" runat="server"></cc1:User>
    <cc3:AptifyWebUserLogin id="WebUserLogin1" runat="server"></cc3:AptifyWebUserLogin>
    <cc4:AptifyShoppingCart id="ShoppingCart1" runat="server" />
