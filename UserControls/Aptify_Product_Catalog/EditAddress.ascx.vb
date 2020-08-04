'Aptify e-Business 5.5.1, July 2013
Option Explicit On

Imports System.Data
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.Application

Namespace Aptify.Framework.Web.eBusiness.ProductCatalog
    Partial Class EditAddressControl
        Inherits BaseUserControlAdvanced
        Protected Const ATTRIBUTE_ADMIN_EDIT_PROFILE As String = "AdminEditprofileUrl"
        Protected Const ATTRIBUTE_CANCEL_BUTTON_PAGE As String = "CancelButtonPage"
        Protected Const ATTRIBUTE_ADD_EDIT_ADDRESS_BUTTON_PAGE As String = "AddEditAddressButtonPage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "EditAddress"
        Protected Const ATTRIBUTE_PERSONS_VIEWSTATE As String = "vwPersons"
        Protected Const ATTRIBUTE_DEFAULT_RETURN_PAGE As String = "DefaultReturnPage"

        Dim PersonID As Long

#Region "Group Admin Specific Edit Profile"
        ''' <summary>
        ''' Meeting page url
        ''' </summary>

        Public Overridable Property AdminEditProfile() As String
            Get
                If Not ViewState(ATTRIBUTE_ADMIN_EDIT_PROFILE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_ADMIN_EDIT_PROFILE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_ADMIN_EDIT_PROFILE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
#End Region
#Region "EditAddress Specific Properties"
        ''' <summary>
        ''' CancelButton page url
        ''' </summary>
        Public Overridable Property CancelButtonPage() As String
            Get
                If Not ViewState(ATTRIBUTE_CANCEL_BUTTON_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_CANCEL_BUTTON_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_CANCEL_BUTTON_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' AddEditAddressButton page url
        ''' </summary>
        Public Overridable Property AddEditAddressButtonPage() As String
            Get
                If Not ViewState(ATTRIBUTE_ADD_EDIT_ADDRESS_BUTTON_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_ADD_EDIT_ADDRESS_BUTTON_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_ADD_EDIT_ADDRESS_BUTTON_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        Public Overridable Property DefaultReturnPage() As String
            Get
                If Not ViewState(ATTRIBUTE_DEFAULT_RETURN_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_DEFAULT_RETURN_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_DEFAULT_RETURN_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

#End Region

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(AdminEditProfile) Then
                'since value is the 'default' check the XML file for possible custom setting
                AdminEditProfile = Me.GetLinkValueFromXML(ATTRIBUTE_ADMIN_EDIT_PROFILE)
                If String.IsNullOrEmpty(AdminEditProfile) Then
                    Me.grdperson.Enabled = False
                    Me.grdperson.ToolTip = "Admin Edit Profile property has not been set."
                End If
            End If

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(CancelButtonPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                CancelButtonPage = Me.GetLinkValueFromXML(ATTRIBUTE_CANCEL_BUTTON_PAGE)
                If String.IsNullOrEmpty(CancelButtonPage) Then
                    Me.cmdCancel.Enabled = False
                    Me.cmdCancel.ToolTip = "CancelButtonPage property has not been set."
                End If
            End If
            If String.IsNullOrEmpty(AddEditAddressButtonPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                AddEditAddressButtonPage = Me.GetLinkValueFromXML(ATTRIBUTE_ADD_EDIT_ADDRESS_BUTTON_PAGE)
                If String.IsNullOrEmpty(AddEditAddressButtonPage) Then
                    Me.cmdSave.Enabled = False
                    Me.cmdSave.ToolTip = "AddEditAddressButtonPage property has not been set."
                End If
            End If
            If String.IsNullOrEmpty(DefaultReturnPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                DefaultReturnPage = Me.GetLinkValueFromXML(ATTRIBUTE_DEFAULT_RETURN_PAGE)
            End If
        End Sub
        '<%--Nalini Issue#12578--%>
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Dim sAction As String = String.Empty

            Try
                'set control properties from XML file if needed
                SetProperties()
                sAction = Request.QueryString("Action")
                If Not IsPostBack Then
                    LoadCombos()
                    If Len(sAction) > 0 Then
                        If sAction = "Edit" Then
                            cmdSave.Text = "Update"
                            lblAddressHeader.Text = "Edit Address"
                            LoadAddress()
                        End If
                    End If
                    If sAction = "New" Or sAction = "PersonAddress" Then
                        resetAddressFields()
                    End If
                    If (Session("PageIndex") IsNot Nothing) Then
                        grdperson.CurrentPageIndex = Session("PageIndex")
                    End If
                End If

                If UserIsGroupAdmin() AndAlso sAction = "PersonAddress" Then
                    LoadPerson()
                    disableAddressEdit()
                    grdperson.Visible = True
                    cmdSave.Visible = False
                    'Issue :222070 : Address name field visible set to false
                    lblName.Visible = False
                    txtName.Visible = False
                    cmdSelectAddress.Visible = True
                Else
                    grdperson.Visible = False
                    cmdSave.Visible = True
                    cmdSelectAddress.Visible = False
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Private Sub LoadAddress()
            ' load up the address
            Dim sType As String
            Dim iSequence As Integer

            Try
                sType = Request.QueryString("AddressType")
                iSequence = Request.QueryString("Sequence")
                If Len(sType) > 0 Then
                    trType.Visible = False
                    txtName.Enabled = False
                    Select Case sType
                        Case "Main"
                            SetAddress("", "Main Address")
                        Case "Billing"
                            SetAddress("Billing", "Billing Address")
                        Case "POBox"
                            SetAddress("POBox", "P.O. Box")
                        Case "Home"
                            SetAddress("Home", "Home Address")
                        Case "PersonAddress"
                            cmbType.Enabled = True
                            txtName.Enabled = True
                            LoadSubTypeAddress(iSequence, User1.PersonID)
                    End Select
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Private Sub LoadSubTypeAddress(ByVal iSequence As Integer, ByVal PersonID As Long)
            Dim oPerson As AptifyGenericEntityBase
            Dim oItem As System.Web.UI.WebControls.ListItem

            Try
                oPerson = AptifyApplication.GetEntityObject("Persons", PersonID)

                With oPerson.SubTypes("PersonAddress").Item(iSequence - 1)
                    txtName.Text = .GetValue("Name")
                    txtAddressLine1.Text = .GetValue("AddressLine1")
                    txtAddressLine2.Text = .GetValue("AddressLine2")
                    txtAddressLine3.Text = .GetValue("AddressLine3")
                    txtCity.Text = .GetValue("City")
                    txtZipCode.Text = .GetValue("ZipCode")

                    oItem = cmbCountry.Items.FindByText(.GetValue("Country"))
                    If Not oItem Is Nothing Then
                        oItem.Selected = True
                    End If

                    PopulateState()

                    oItem = cmbState.Items.FindByText(.GetValue("State"))
                    If Not oItem Is Nothing Then
                        oItem.Selected = True
                    End If
                End With
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Private Sub SetAddress(ByVal sPrefix As String, ByVal sType As String)
            Dim oItem As System.Web.UI.WebControls.ListItem

            Try
                txtName.Text = sType
                If sPrefix = "POBox" Then
                    txtAddressLine1.Text = User1.GetValue("POBoxLine1")
                    txtAddressLine2.Text = User1.GetValue("POBoxLine2")
                    txtAddressLine3.Text = User1.GetValue("POBoxLine3")
                Else
                    txtAddressLine1.Text = User1.GetValue(sPrefix & "AddressLine1")
                    txtAddressLine2.Text = User1.GetValue(sPrefix & "AddressLine2")
                    txtAddressLine3.Text = User1.GetValue(sPrefix & "AddressLine3")
                End If
                txtCity.Text = User1.GetValue(sPrefix & "City")
                txtZipCode.Text = User1.GetValue(sPrefix & "ZipCode")
                '11/27/07,Added by Tamasa,Issue 5222.
                oItem = cmbCountry.Items.FindByText(User1.GetValue(sPrefix & "Country"))
                If Not oItem Is Nothing Then
                    oItem.Selected = True
                End If
                PopulateState()
                'End
                oItem = cmbState.Items.FindByText(User1.GetValue(sPrefix & "State"))
                If Not oItem Is Nothing Then
                    oItem.Selected = True
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Private Sub LoadCombos()
            Dim sSQL As String
            Try
                sSQL = Database & "..spGetCountryList"
                cmbCountry.DataSource = DataAction.GetDataTable(sSQL)
                cmbCountry.DataBind()


                sSQL = "SELECT ID,Name FROM " & _
                       Database & "..vwAddressTypes " & _
                       " ORDER BY Name "
                cmbType.DataSource = DataAction.GetDataTable(sSQL)
                cmbType.DataBind()
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub


        Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click

            If Request.QueryString("ReturnToPage") = "" AndAlso UserIsGroupAdmin() AndAlso Request.QueryString("Action") = "PersonAddress" Then
                Response.Redirect(DefaultReturnPage)
            Else
                Response.Redirect(CancelButtonPage & "?Type=" & Request.QueryString("Type"))
            End If

        End Sub

        Private Sub cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave.Click
            Dim oPerson As AptifyGenericEntityBase
            Dim sType As String
            Dim iSequence As Integer
            Dim sAction As String

            Try
                sAction = Request.QueryString("Action")
                sType = Request.QueryString("AddressType")
                iSequence = Request.QueryString("Sequence")

                oPerson = AptifyApplication.GetEntityObject("Persons", User1.PersonID)

                If sAction = "Edit" Then
                    EditPersonAddress(oPerson, sType, iSequence)
                Else
                    AddAddressToPerson(oPerson, txtName.Text)
                End If

                If oPerson.Save(False) Then
                    Select Case sType
                        Case "Home"
                            SetUserObjectAddress("Home")
                        Case "Main"
                            SetUserObjectAddress("Main")
                        Case "Billing"
                            SetUserObjectAddress("Billing")
                        Case "POBox"
                            SetUserObjectAddress("POBox")
                        Case Else
                            ' do not need to do this for sub-type items
                            ' only values stored in the user object are
                            ' from the top-level entity
                    End Select
                    User1.SaveValuesToSessionObject(Page.Session)
                    Response.Redirect(AddEditAddressButtonPage & "?Type=" & Request.QueryString("Type"))
                Else
                    lblError.Visible = True
                    lblError.Text = "Error Adding Address: " & oPerson.LastError
                    lblError.ForeColor = Drawing.Color.Red
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub EditPersonAddress(ByRef oPerson As AptifyGenericEntityBase, _
                                      ByVal sType As String, _
                                      ByVal iSequence As Integer)
            Select Case sType
                Case "Home"
                    SetPersonObjectAddress(oPerson, "Home")
                Case "Main"
                    SetPersonObjectAddress(oPerson, "")
                Case "Billing"
                    SetPersonObjectAddress(oPerson, "Billing")
                Case "POBox"
                    SetPersonObjectAddress(oPerson, "POBox")
                Case Else
                    ' person address sub-type
                    SetPersonObjectAddress(oPerson.SubTypes("PersonAddress").Item(iSequence - 1), "", Me.txtName.Text)
            End Select
        End Sub

        Private Sub SetPersonObjectAddress(ByRef oPerson As AptifyGenericEntityBase, _
                                            ByVal sPrefix As String, _
                                            Optional ByVal AddressName As String = "")
            '01/22/08 Tamasa added for issue 5222(Bug Fixes).
            Dim bIsSubType As Boolean = False
            Dim oAddress As AptifyGenericEntityBase
            Dim sError As String = "Its error"
            Try
                If String.Compare(oPerson.EntityName, "Persons", True) = 0 Then
                    oAddress = oPerson.Fields(sPrefix & "AddressID").EmbeddedObject
                ElseIf String.Compare(oPerson.EntityName, "PersonAddress", True) = 0 Then
                    oAddress = oPerson.Fields("AddressID").EmbeddedObject
                    bIsSubType = True
                    oAddress.SetValue("AddressTypeID", cmbType.SelectedItem.Value)

                    oPerson.SetValue("Name", AddressName)
                Else
                    oAddress = oPerson.Fields("AddressID").EmbeddedObject
                End If

                With oAddress
                    .SetValue("Line1", txtAddressLine1.Text)
                    .SetValue("Line2", txtAddressLine2.Text)
                    .SetValue("Line3", txtAddressLine3.Text)
                    .SetValue("City", txtCity.Text)
                    .SetValue("StateProvince", CStr(IIf(cmbState.SelectedIndex >= 0, cmbState.SelectedItem.Text, "")))
                    .SetValue("PostalCode", txtZipCode.Text)
                    .SetValue("CountryCodeID", cmbCountry.SelectedValue.ToString)
                    .SetValue("Country", cmbCountry.SelectedItem.Text)
                End With

                If Not bIsSubType Then
                    If Not oPerson.Save(False) Then _
                            Throw New ApplicationException("Unable to save the Person's Address changes.  Error: " & oPerson.LastError)
                Else
                    If Not oPerson.ParentGE.Save(False) Then _
                            Throw New ApplicationException("Unable to save the Person's Address changes.  Error: " & oPerson.ParentGE.LastError)
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Private Sub SetUserObjectAddress(ByVal sPrefix As String)

            Try
                'Code Commented and added by Suvarna for IssueID- 13178
                If UCase(sPrefix) = UCase("POBox") Then
                    'AddressLine1 is "POBox" in table hence added. 
                    User1.SetValue("POBox", txtAddressLine1.Text)
                    User1.SetValue("POBoxLine2", txtAddressLine2.Text)
                    User1.SetValue("POBoxLine3", txtAddressLine3.Text)
                    User1.SetValue(sPrefix & "City", txtCity.Text)
                    User1.SetValue(sPrefix & "State", CStr(IIf(cmbState.SelectedIndex >= 0, cmbState.SelectedItem.Text, "")))
                    User1.SetValue(sPrefix & "ZipCode", txtZipCode.Text)
                    User1.SetValue(sPrefix & "Country", cmbCountry.SelectedItem.Text)
                    User1.SetAddValue(sPrefix & "CountryCodeID", cmbCountry.SelectedValue.ToString)
                ElseIf UCase(sPrefix) = UCase("Main") Then
                    User1.SetValue("AddressLine1", txtAddressLine1.Text)
                    User1.SetValue("AddressLine2", txtAddressLine2.Text)
                    User1.SetValue("AddressLine3", txtAddressLine3.Text)
                    User1.SetValue("City", txtCity.Text)
                    User1.SetValue("State", CStr(IIf(cmbState.SelectedIndex >= 0, cmbState.SelectedItem.Text, "")))
                    User1.SetValue("ZipCode", txtZipCode.Text)
                    User1.SetValue("Country", cmbCountry.SelectedItem.Text)
                    User1.SetAddValue("CountryCodeID", cmbCountry.SelectedValue.ToString)
                Else
                    User1.SetValue(sPrefix & "AddressLine1", txtAddressLine1.Text)
                    User1.SetValue(sPrefix & "AddressLine2", txtAddressLine2.Text)
                    User1.SetValue(sPrefix & "AddressLine3", txtAddressLine3.Text)
                    User1.SetValue(sPrefix & "City", txtCity.Text)
                    User1.SetValue(sPrefix & "State", CStr(IIf(cmbState.SelectedIndex >= 0, cmbState.SelectedItem.Text, "")))
                    User1.SetValue(sPrefix & "ZipCode", txtZipCode.Text)
                    User1.SetValue(sPrefix & "Country", cmbCountry.SelectedItem.Text)
                    User1.SetAddValue(sPrefix & "CountryCodeID", cmbCountry.SelectedValue.ToString)
                End If

                User1.Save()

                'If sPrefix = "POBox" Then
                '    User1.SetValue("POBoxLine1", txtAddressLine1.Text)
                '    User1.SetValue("POBoxLine2", txtAddressLine2.Text)
                '    User1.SetValue("POBoxLine3", txtAddressLine3.Text)
                'Else
                '    User1.SetValue(sPrefix & "AddressLine1", txtAddressLine1.Text)
                '    User1.SetValue(sPrefix & "AddressLine2", txtAddressLine2.Text)
                '    User1.SetValue(sPrefix & "AddressLine3", txtAddressLine3.Text)
                'End If
                'User1.SetValue(sPrefix & "City", txtCity.Text)
                'User1.SetValue(sPrefix & "State", CStr(IIf(cmbState.SelectedIndex >= 0, cmbState.SelectedItem.Text, "")))
                'User1.SetValue(sPrefix & "ZipCode", txtZipCode.Text)
                'User1.SetValue(sPrefix & "Country", cmbCountry.SelectedItem.Text)
                'User1.SetAddValue(sPrefix & "CountryCodeID", cmbCountry.SelectedValue.ToString)
                'User1.Save()
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub AddAddressToPerson(ByRef oPerson As AptifyGenericEntityBase, ByVal AddressName As String)
            Try
                Me.SetPersonObjectAddress(oPerson.SubTypes("PersonAddress").Add, "", AddressName)
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        '11/27/07,Added by Tamasa,Issue 5222.
        Private Sub PopulateState()
            Try
                Dim sSQL As String
                sSQL = Database & "..spGetStateList @CountryID=" & cmbCountry.SelectedValue.ToString
                cmbState.DataSource = DataAction.GetDataTable(sSQL)
                cmbState.DataTextField = "State"
                cmbState.DataValueField = "State"
                cmbState.DataBind()
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        '11/27/07,Added by Tamasa,Issue 5222.
        Protected Sub cmbCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCountry.SelectedIndexChanged
            PopulateState()
        End Sub
        Private Sub SetComboValue(ByRef cmb As System.Web.UI.WebControls.DropDownList, _
                                ByVal sValue As String)
            Dim i As Integer

            Try
                For i = 0 To cmb.Items.Count - 1
                    If String.Compare(cmb.Items(i).Value, sValue, True) = 0 Then
                        cmb.Items(i).Selected = True
                        Exit Sub
                    End If
                    '11/27/07,Added by Tamasa,Issue 5222.
                    If String.Compare(cmb.Items(i).Text, sValue, True) = 0 Then
                        cmb.Items(i).Selected = True
                        Exit Sub
                    End If
                    'End
                Next
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Private Sub PopulateStateAsCountry(ByRef cmbState As DropDownList, ByRef cmbCurrentCountry As DropDownList)
            Try
                Dim sSQL As String
                sSQL = Database & "..spGetStateList @CountryID=" & cmbCurrentCountry.SelectedValue.ToString
                cmbState.DataSource = DataAction.GetDataTable(sSQL)
                cmbState.DataTextField = "State"
                cmbState.DataValueField = "State"
                cmbState.DataBind()
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub LoadPerson()
            Dim dt As DataTable, sSQL As String, sDB As String
            Try
                If ViewState(ATTRIBUTE_PERSONS_VIEWSTATE) Is Nothing Then
                    Dim companyID As Integer = -1
                    Dim oGE As Aptify.Applications.Persons.PersonsEntity = DirectCast(Me.AptifyApplication.GetEntityObject("Persons", User1.PersonID), Aptify.Applications.Persons.PersonsEntity)


                    If oGE IsNot Nothing Then
                        companyID = oGE.CompanyID
                    End If
                    Dim objaptifyapp As New AptifyApplication()
                    sDB = objaptifyapp.GetEntityBaseDatabase("Persons")
                    sSQL = "select VP.ID PersonID, VP.FirstLast,VP.CompanyID,VP.Title,VP.Photo from " & sDB & _
                         "..vwPersons VP where VP.CompanyID = " & companyID.ToString & " And ID <> " & User1.PersonID.ToString


                    dt = Me.DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                    Dim dcolUrl As DataColumn = New DataColumn()
                    dcolUrl.Caption = "AdminEditprofileUrl"
                    dcolUrl.ColumnName = "AdminEditprofileUrl"

                    dt.Columns.Add(dcolUrl)
                    'Encrypt the person ID
                    Dim sTemUrl As String = ""
                    Dim index As Integer
                    Dim sValue As String = ""
                    Dim sSeparator As String()
                    Dim sNavigate As String = ""
                    If dt.Rows.Count > 0 Then
                        For Each rw As DataRow In dt.Rows

                            sTemUrl = AdminEditProfile + "?ID=" + rw("PersonID").ToString()
                            index = sTemUrl.IndexOf("=")
                            sValue = sTemUrl.Substring(index + 1)
                            sSeparator = sTemUrl.Split(CChar("="))
                            sNavigate = sSeparator(0)
                            sNavigate = sNavigate & "="
                            sNavigate = sNavigate & System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(sValue))
                            rw("AdminEditprofileUrl") = sNavigate + " &location=EditAddress&Type=" + Request.QueryString("Type")
                        Next
                    End If

                    If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                        Me.grdperson.DataSource = dt
                        ViewState(ATTRIBUTE_PERSONS_VIEWSTATE) = dt

                    Else
                        ViewState(ATTRIBUTE_PERSONS_VIEWSTATE) = dt
                        grdperson.DataSource = dt

                    End If
                Else
                    Dim tempdt As DataTable = CType(ViewState(ATTRIBUTE_PERSONS_VIEWSTATE), DataTable)
                    If Not tempdt Is Nothing AndAlso tempdt.Rows.Count > 0 Then
                        grdperson.DataSource = tempdt

                    Else
                        grdperson.DataSource = tempdt

                    End If
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub


        Public Sub rdbSelectPerson_OnCheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
            Dim rdbSelectPerson As RadioButton = CType(sender, RadioButton)
            Dim grdDataItem As Telerik.Web.UI.GridDataItem = CType(rdbSelectPerson.NamingContainer, Telerik.Web.UI.GridDataItem)
            Dim lblPersonID As Label = DirectCast(grdDataItem.FindControl("lblPersonID"), Label)
            If Not String.IsNullOrEmpty(lblPersonID.Text) AndAlso Convert.ToInt16(lblPersonID.Text) > 0 Then
                ViewState("Selected_Person") = lblPersonID.Text
            End If

            If rdbSelectPerson IsNot Nothing AndAlso rdbSelectPerson.Checked Then
                cmbType.SelectedIndex = 0
                LoadSelectedPersonAddredd(CLng(lblPersonID.Text), "")
                lblSelectedPersonID.Text = lblPersonID.Text
            End If
        End Sub

        Private Sub LoadSelectedPersonAddredd(ByVal PersonID As Long, ByVal PreFix As String)
            Dim oPerson As AptifyGenericEntityBase
            Dim oItem As System.Web.UI.WebControls.ListItem
            Dim AddressID As Long
            Dim AddressLine1 As String
            Dim AddressLine2 As String
            Dim AddressLine3 As String
            Try
                cmdSelectAddress.Enabled = True
                lblError.Visible = False
                oPerson = AptifyApplication.GetEntityObject("Persons", PersonID)
                AddressID = oPerson.GetValue(PreFix & "AddressID")
                AddressLine1 = oPerson.GetValue(PreFix & "AddressLine1")
                AddressLine2 = oPerson.GetValue(PreFix & "AddressLine2")
                AddressLine3 = oPerson.GetValue(PreFix & "AddressLine3")
                If AddressID <= 0 OrElse (AddressLine1 = "" And AddressLine2 = "" And AddressLine3 = "") Then

                    resetAddressFields()
                    ' lblError.Text = "Person does not have " & PreFix & " Address"
                    lblError.Text = "The selected person does not have an address on file. Please select the person to add/update the address."
                    lblError.Visible = True
                    lblError.ForeColor = Drawing.Color.Red
                    cmdSelectAddress.Enabled = False
                    Exit Sub
                End If
                With oPerson
                    txtName.Text = .GetValue("Name")
                    If PreFix = "POBox" Then
                        txtAddressLine1.Text = .GetValue("POBoxLine1")
                        txtAddressLine2.Text = .GetValue("POBoxLine2")
                        txtAddressLine3.Text = .GetValue("POBoxLine3")
                    Else
                        txtAddressLine1.Text = .GetValue(PreFix & "AddressLine1")
                        txtAddressLine2.Text = .GetValue(PreFix & "AddressLine2")
                        txtAddressLine3.Text = .GetValue(PreFix & "AddressLine3")
                        txtCity.Text = .GetValue(PreFix & "City")
                        txtZipCode.Text = .GetValue(PreFix & "ZipCode")
                    End If
                    oItem = cmbCountry.Items.FindByText(.GetValue(PreFix & "Country"))
                    If Not oItem Is Nothing AndAlso oItem.Text <> "" Then
                        cmbCountry.ClearSelection()
                        oItem.Selected = True
                    End If

                    PopulateState()

                    oItem = cmbState.Items.FindByText(.GetValue(PreFix & "State"))
                    If Not oItem Is Nothing AndAlso oItem.Text <> "" Then
                        cmbState.ClearSelection()
                        oItem.Selected = True
                    End If
                End With


            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub cmbType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbType.SelectedIndexChanged
            Dim sType As String = cmbType.SelectedItem.Text.Trim
            lblError.Visible = False
            If lblSelectedPersonID.Text <> "" Then
                Dim PersonID As Long = CLng(lblSelectedPersonID.Text)
                cmdSelectAddress.Enabled = False
                resetAddressFields()
                Select Case sType
                    Case "Business"
                        LoadSelectedPersonAddredd(PersonID, "")
                    Case "Home"
                        LoadSelectedPersonAddredd(PersonID, "Home")

                End Select
            End If

        End Sub

        Private Function UserIsGroupAdmin() As Boolean
            Try
                Dim sSQL As String
                Dim IsGroupAdmin As Boolean = False
                sSQL = "SELECT IsGroupAdmin FROM VWPERSONS WHERE ID = " & User1.PersonID

                IsGroupAdmin = CBool(DataAction.ExecuteScalar(sSQL))
                Return IsGroupAdmin
            Catch ex As Exception
                Return False
            End Try
        End Function

        Protected Sub cmdSelectAddress_Click(sender As Object, e As System.EventArgs) Handles cmdSelectAddress.Click
            Try
                Dim sType As String = Request.QueryString("Type")
                Dim sPreFix As String = cmbType.SelectedItem.Text.Trim
                Dim iOrderAddrType As AptifyShoppingCart.OrderAddressType
                Dim iPersonAddrType As AptifyShoppingCart.PersonAddressType

                If lblSelectedPersonID.Text <> "" Then

                    If sType = "Shipping" Then
                        iOrderAddrType = AptifyShoppingCart.OrderAddressType.Shipping
                    Else
                        iOrderAddrType = AptifyShoppingCart.OrderAddressType.Billing
                    End If
                    Select Case sPreFix
                        Case "Main"
                            iPersonAddrType = AptifyShoppingCart.PersonAddressType.Main
                        Case "Billing"
                            iPersonAddrType = AptifyShoppingCart.PersonAddressType.Billing
                        Case "POBox"
                            iPersonAddrType = AptifyShoppingCart.PersonAddressType.POBox
                        Case "Home"
                            iPersonAddrType = AptifyShoppingCart.PersonAddressType.Home

                    End Select

                    Dim PersonID As Long = CLng(lblSelectedPersonID.Text)

                    Dim oPerson As AptifyGenericEntityBase
                    oPerson = AptifyApplication.GetEntityObject("Persons", PersonID)
                    AddAddressToPerson(oPerson, txtName.Text)

                    ShoppingCart1.SetAddressForPerson(iOrderAddrType, iPersonAddrType, sPreFix, PersonID, 0, Page.Session, Page.User, Page.Application)
                    ShoppingCart1.SaveCart(Page.Session)
                    If Request.QueryString("ReturnToPage") = "" Then
                        Response.Redirect(DefaultReturnPage)
                    Else
                        Response.Redirect(Request.QueryString("ReturnToPage"))
                    End If
                Else
                    lblError.Text = "Please select person from grid."
                    lblError.Visible = True
                    lblError.ForeColor = Drawing.Color.Red
                End If

            Catch ex As System.Threading.ThreadAbortException
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub disableAddressEdit()
            txtName.Enabled = False
            txtAddressLine1.Enabled = False
            txtAddressLine2.Enabled = False
            txtAddressLine3.Enabled = False
            txtCity.Enabled = False
            cmbState.Enabled = False
            txtZipCode.Enabled = False
            cmbCountry.Enabled = False
        End Sub

        Protected Sub resetAddressFields()
            txtName.Text = ""
            txtAddressLine1.Text = ""
            txtAddressLine2.Text = ""
            txtAddressLine3.Text = ""
            txtCity.Text = ""
            txtZipCode.Text = ""
            cmbCountry.ClearSelection()
            cmbState.ClearSelection()
            SetComboValue(cmbCountry, "United States")
            PopulateStateAsCountry(cmbState, cmbCountry)
            SetComboValue(cmbState, "DC")
        End Sub


        Protected Sub grdperson_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles grdperson.ItemDataBound
            Dim rdbSelectPerson As RadioButton = DirectCast(e.Item.FindControl("rdbSelectPerson"), RadioButton)
            Dim lblPersonID As Label = DirectCast(e.Item.FindControl("lblPersonID"), Label)
            Dim sType As String = cmbType.SelectedItem.Text.Trim

            If (Request.QueryString("ID") <> "") Then
                For Each item As Telerik.Web.UI.GridDataItem In grdperson.MasterTableView.Items

                    Dim ID As Label = DirectCast(item.FindControl("lblPersonID"), Label)
                    If (ID.Text = Request.QueryString("ID").ToString()) Then
                        Dim chk As CheckBox = DirectCast(item.FindControl("rdbSelectPerson"), CheckBox)
                        chk.Checked = True
                        ViewState("Selected_Person") = ID.Text
                        lblSelectedPersonID.Text = ID.Text
                        If sType = "Business" Then
                            sType = ""
                        End If



                        LoadSelectedPersonAddredd(ID.Text, sType)
                        Exit For
                    End If

                Next

            End If

            If rdbSelectPerson IsNot Nothing AndAlso lblPersonID IsNot Nothing AndAlso ViewState("Selected_Person") IsNot Nothing Then
                If lblPersonID.Text = Convert.ToString(ViewState("Selected_Person")) Then
                    rdbSelectPerson.Checked = True
                    If sType = "Business" Then
                        sType = ""
                    End If
                    LoadSelectedPersonAddredd(lblPersonID.Text, sType)
                End If
            End If
        End Sub


        Protected Sub grdperson_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdperson.NeedDataSource
            If ViewState(ATTRIBUTE_PERSONS_VIEWSTATE) IsNot Nothing Then
                grdperson.DataSource = CType(ViewState(ATTRIBUTE_PERSONS_VIEWSTATE), DataTable)
            End If
        End Sub

        Protected Sub grdperson_PageIndexChanged(sender As Object, e As Telerik.Web.UI.GridPageChangedEventArgs) Handles grdperson.PageIndexChanged
            grdperson.CurrentPageIndex = e.NewPageIndex
            Session("PageIndex") = grdperson.CurrentPageIndex
            If ViewState(ATTRIBUTE_PERSONS_VIEWSTATE) IsNot Nothing Then
                grdperson.DataSource = CType(ViewState(ATTRIBUTE_PERSONS_VIEWSTATE), DataTable)

            End If
        End Sub
    End Class

End Namespace