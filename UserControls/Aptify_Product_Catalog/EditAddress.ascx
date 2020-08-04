<%--Aptify e-Business 5.5.1, July 2013--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="EditAddress.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.ProductCatalog.EditAddressControl" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessShoppingCart" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%--Nalini Issue#12578--%>
<script type="text/javascript">
    function SetUniqueRadioButton(current) {

        for (i = 0; i < document.forms[0].elements.length; i++) {
            elm = document.forms[0].elements[i]
            if (elm.type == 'radio') {
                elm.checked = false;
            }

        }

        current.checked = true;
    }

</script>
<div id="tblMain" runat="server" class="table-div">
    <div class="row-div">
        <h1>
            <asp:Label ID="lblAddressHeader" runat="server" Text=" Add New Address"></asp:Label></h1>
        <asp:Label ID="lblError" runat="server" Visible="False"></asp:Label>
        <asp:Label ID="lblSelectedPersonID" runat="server" Visible="false"></asp:Label>
    </div>
    <div class="row-div">
        <rad:RadGrid ID="grdperson" runat="server" AllowPaging="true" AllowSorting="true"
                AutoGenerateColumns="false" 
            AllowFilteringByColumn="true" SortingSettings-SortedDescToolTip="Sorted Descending"
            SortingSettings-SortedAscToolTip="Sorted Ascending" AllowMultiRowSelection="false" OnItemDataBound="grdperson_ItemDataBound">
            <GroupingSettings CaseSensitive="false" />
            <MasterTableView AllowNaturalSort="false" ClientDataKeyNames="PersonID" AllowFilteringByColumn="true" AllowSorting="true" >
                <Columns>
                    <rad:GridTemplateColumn HeaderText="Select" AllowFiltering="false">
                        <ItemTemplate>
                            <asp:RadioButton ID="rdbSelectPerson" runat="server" AutoPostBack="true" OnCheckedChanged="rdbSelectPerson_OnCheckedChanged"
                                GroupName="PersonsGroup" Onclick="SetUniqueRadioButton(this)" />
                        </ItemTemplate>
                    </rad:GridTemplateColumn>
                    <rad:GridTemplateColumn HeaderText="PersonID" Visible="false" AllowSorting="true" AllowFiltering="true" >
                        <ItemTemplate>
                            <asp:Label ID="lblPersonID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PersonID") %>'></asp:Label>
                        </ItemTemplate>
                    </rad:GridTemplateColumn>
                    <rad:GridTemplateColumn HeaderText="Person" DataField="FirstLast" SortExpression="FirstLast" AllowSorting ="true" AllowFiltering="true"
                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                        <ItemTemplate>
                                             <asp:HyperLink ID="lblFirstLast" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "FirstLast") %>'  NavigateUrl='
															<%# DataBinder.Eval(Container.DataItem,"AdminEditprofileUrl") %>'/>
                        </ItemTemplate>
                    </rad:GridTemplateColumn>
                    <rad:GridTemplateColumn HeaderText="Title" DataField="Title" SortExpression="Title"
                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                        <ItemTemplate>
                            <asp:Label ID="lblAttendeeTitle" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Title") %>'></asp:Label>
                        </ItemTemplate>
                    </rad:GridTemplateColumn>
                </Columns>
            </MasterTableView>
        </rad:RadGrid>
    </div>
    <div class="edit-address">
    <div id="trType" runat="server" class="row-div clearfix">
        <div class="label-div w25">
            <asp:Label runat="server" ID="lblType">Type</asp:Label>
        </div>
        <div class="field-div1 w74">
            <asp:DropDownList runat="server" ID="cmbType" DataTextField="Name" DataValueField="ID" AutoPostBack = "true"
                Width="145px">
            </asp:DropDownList>
        </div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w25">
            <asp:Label runat="server" ID="lblName">Address Name</asp:Label>
        </div>
        <div class="field-div1 w74">
            <asp:TextBox runat="server" ID="txtName"></asp:TextBox>
        </div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w25">
            <asp:Label runat="server" ID="lblAddress"> Address1</asp:Label>
        </div>
        <div class="field-div1 w74">
            <asp:TextBox runat="server" ID="txtAddressLine1"></asp:TextBox>
        </div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w25">
            <asp:Label runat="server" ID="Label1"> Address2</asp:Label>
        </div>
        <div class="field-div1 w74">
            <asp:TextBox runat="server" ID="txtAddressLine2"></asp:TextBox>
        </div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w25">
            <asp:Label runat="server" ID="Label2"> Address3</asp:Label>
        </div>
        <div class="field-div1 w74">
            <asp:TextBox runat="server" ID="txtAddressLine3"></asp:TextBox>
        </div>
    </div>
    <div class="row-div csz clearfix">
        <div class="label-div w25">
            <asp:Label runat="server" ID="lblCityStateZip">City, State ZIP</asp:Label>
        </div>
        <div class="field-div1 w74">
            <asp:TextBox runat="server" ID="txtCity"></asp:TextBox>
            <asp:DropDownList runat="server" ID="cmbState" DataTextField="State" DataValueField="State">
            </asp:DropDownList>
            <asp:TextBox runat="server" ID="txtZipCode"></asp:TextBox>
        </div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w25">
            <asp:Label runat="server" ID="lblCountry">Country</asp:Label>
        </div>
        <div class="field-div1 w74">
            <asp:DropDownList runat="server" ID="cmbCountry" DataTextField="Country" DataValueField="ID"
                AutoPostBack="true">
            </asp:DropDownList>
        </div>
    </div>
    <div class="row-div clearfix">
        <div class="label-div w25">
            &nbsp;</div>
        <div class="field-div1 w74">
            <asp:Button runat="server" ID="cmdSelectAddress" Text="Use this address"></asp:Button>
            <asp:Button runat="server" ID="cmdSave" Text="Add"></asp:Button>
            <asp:Button ID="cmdCancel" runat="server" Text="Cancel"></asp:Button>
        </div>
    </div>
    <cc2:User runat="Server" ID="User1" />
    <cc1:AptifyShoppingCart runat="server" ID="ShoppingCart1" />
</div>
 </div>
