''Aptify e-Business 5.5.1, July 2013
Imports Aptify.Framework.Application
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.BusinessLogic.Security
Imports System.Data
Imports System.IO
Imports Aptify.Framework.Web.eBusiness.SocialNetworkIntegration
Imports Aptify.Framework.BusinessLogic.ProcessPipeline
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports System.Linq
Imports System.Drawing
Imports System.Web
Imports System.Web.UI.Control
Imports Telerik.Web.UI

Namespace Aptify.Framework.Web.eBusiness.CustomerService
    Partial Class AdminEditProfile
        Inherits BaseUserControlAdvanced
        Dim dt As DataTable
        Protected Const ATTRIBUTE_PERSON_BLANK_IMG As String = "BlankImage"
        Protected m_sblankImage As String
        Protected m_iImageWidth As Integer = -1
        Protected m_iImageHeight As Integer = -1
        Protected Const ATTRIBUTE_PERSON_IMG_WIDTH As String = "ImageWidth"
        Protected Const ATTRIBUTE_PERSON_IMG_HEIGHT As String = "ImageHeight"
        Protected Const ATTRIBUTE_PERSON_IMAGE_URL As String = "PersonImageURL"
        Protected Const ATTRIBUTE_PROFILE_PHOTO_FILENAME As String = "ProfilePhotoFileName"
        Protected ATTRIBUTE_IMAGEUPLOAD_PROFILE_TNCTEXT As String = "ImageUploadUserProfileText"
        Protected ATTRIBUTE_IMAGEUPLOAD_PROFILESAVE_TNCTEXT As String = "ImageUploadUserProfileSaveText"
        Protected Const ATTRIBUTE_LOGIN_PAGE As String = "LoginPage" 'Added by Sandeep for Issue 15051 on 12/03/2013
        Dim m_lEntityID As Long
        Dim m_lRecordID As String
        Dim m_sEntityName As String = "Persons"
        'Issue 15387 Anil B on 05-04-2013
        Protected Const ATTRIBUTE_SECURITYERROR_PAGE As String = "securityErrorPage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "AdminEditProfile"
        Protected Const ATTRIBUTE_EDIT_ADDRESS As String = "EditAddressPage"
        Dim ImageData() As Byte



        Private Property PreferredAddress() As String
            Get
                If ViewState.Item("PreferredAddress") IsNot Nothing Then
                    Return ViewState.Item("PreferredAddress").ToString()
                Else
                    Return ""
                End If

            End Get
            Set(ByVal value As String)
                ViewState.Item("PreferredAddress") = value
            End Set
        End Property
        Public Property PersonImageURL() As String
            Get
                If ViewState.Item("PersonImageURL") IsNot Nothing Then
                    Return ViewState.Item("PersonImageURL").ToString()
                Else
                    Return ""
                End If

            End Get
            Set(ByVal value As String)
                ViewState.Item("PersonImageURL") = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        Public Property BlankImage() As String
            Get
                Return m_sblankImage
            End Get
            Set(ByVal value As String)
                m_sblankImage = value
            End Set
        End Property

        Public Property ImageWidth() As Integer
            Get
                Return m_iImageWidth
            End Get
            Set(ByVal value As Integer)
                m_iImageWidth = value
            End Set
        End Property
        Public Property ImageHeight() As Integer
            Get
                Return m_iImageHeight
            End Get
            Set(ByVal value As Integer)
                m_iImageHeight = value
            End Set
        End Property
        Public ReadOnly Property ProfilePhotoBlankImage As String
            Get
                Return PersonImageURL & BlankImage
            End Get
        End Property
        Protected ReadOnly Property ProfilePhotoFileName() As String
            Get
                Return Convert.ToString(ViewState(ATTRIBUTE_PROFILE_PHOTO_FILENAME))
            End Get
        End Property
        Protected Overridable Function GetNewProfilePhotoFileName() As String
            Dim uniqueGuid As String = System.Guid.NewGuid.ToString
            If User1.PersonID <= 0 Then
                ViewState(ATTRIBUTE_PROFILE_PHOTO_FILENAME) = m_lEntityID & "_" & Me.Session.SessionID & "_" & uniqueGuid & ".jpg"
            Else
                ViewState(ATTRIBUTE_PROFILE_PHOTO_FILENAME) = m_lEntityID & "_" & User1.PersonID & "_" & uniqueGuid & ".jpg"
            End If
            Return Convert.ToString(ViewState(ATTRIBUTE_PROFILE_PHOTO_FILENAME))
        End Function
        Protected ReadOnly Property ProfilePhotoMapPath() As String
            Get
                Return Server.MapPath(PersonImageURL)
            End Get
        End Property

        Protected Overridable ReadOnly Property ImageUploadUserProfileSaveText() As String
            Get
                If Not ViewState.Item(ATTRIBUTE_IMAGEUPLOAD_PROFILESAVE_TNCTEXT) Is Nothing Then
                    Return CStr(ViewState.Item(ATTRIBUTE_IMAGEUPLOAD_PROFILESAVE_TNCTEXT))
                Else
                    Dim value As String = Me.GetGlobalAttributeValue(ATTRIBUTE_IMAGEUPLOAD_PROFILESAVE_TNCTEXT)
                    If Not String.IsNullOrEmpty(value) Then
                        ViewState.Item(ATTRIBUTE_IMAGEUPLOAD_PROFILESAVE_TNCTEXT) = value
                        Return value
                    Else
                        Return String.Empty
                    End If
                End If
            End Get

        End Property

        Protected Overridable ReadOnly Property ImageUploadUserProfileText() As String
            Get
                If Not ViewState.Item(ATTRIBUTE_IMAGEUPLOAD_PROFILE_TNCTEXT) Is Nothing Then
                    Return CStr(ViewState.Item(ATTRIBUTE_IMAGEUPLOAD_PROFILE_TNCTEXT))
                Else
                    Dim value As String = Me.GetGlobalAttributeValue(ATTRIBUTE_IMAGEUPLOAD_PROFILE_TNCTEXT)
                    If Not String.IsNullOrEmpty(value) Then
                        ViewState.Item(ATTRIBUTE_IMAGEUPLOAD_PROFILE_TNCTEXT) = value
                        Return value
                    Else
                        Return String.Empty
                    End If
                End If
            End Get

        End Property
        'Added by Sandeep for Issue 15051 on 12/03/2013
        Public Overridable Property LoginPage() As String
            Get
                If Not ViewState(ATTRIBUTE_LOGIN_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_LOGIN_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_LOGIN_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        Public Overridable Property EditAddressPage() As String
            Get
                If Not ViewState(ATTRIBUTE_EDIT_ADDRESS) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_EDIT_ADDRESS))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_EDIT_ADDRESS) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ' Issue 15387 05-04-2013 by Anil B
        ''' <summary>
        ''' Security Error Page page url
        ''' </summary>
        Public Overridable ReadOnly Property SecurityErrorPage() As String
            Get
                If Not ViewState(ATTRIBUTE_SECURITYERROR_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_SECURITYERROR_PAGE))
                Else
                    Dim value As String = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings("SecurityErrorPageURL"))
                    If Not String.IsNullOrEmpty(value) Then
                        ViewState(ATTRIBUTE_SECURITYERROR_PAGE) = value
                        Return value
                    Else
                        Return String.Empty
                    End If
                End If
            End Get
        End Property

        Protected Sub btnOpenPopup_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOpenPopup.Click
            LoadForm()
            radwindowPopup.VisibleOnPageLoad = True
            LoadProfilePicture(Nothing)
            UpdateImageSize(ImageData)
            'Amruta IssueID 14623
            lblmsg.Visible = False
        End Sub
        Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOk.Click
            If txtEditFirstName.Text = "" Or txtEditLastName.Text = "" Then
                radwindowPopup.VisibleOnPageLoad = True
            Else
                DoSave()
                radwindowPopup.VisibleOnPageLoad = False
            End If
        End Sub

        Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
            'txtEditFirstName.Text = ""
            'txtEditLastName.Text = ""
            'txtEditJobFunction.Text = ""
            'LoadForm()
            radwindowPopup.VisibleOnPageLoad = False
        End Sub

        Private Sub LoadForm()
            Try
                'For Each ctl As Control In Me.Controls
                '    If ctl IsNot Nothing AndAlso ctl.GetType() Is DropDownList Then

                '    End If
                'Next

                PopulateDropDowns()

                If CType(Session.Item("PersonID"), Long) > 0 Then
                    LoadUserInfo()
                Else

                    SetComboValue(cmbCountry, "United States")
                    PopulateState(cmbState, cmbCountry)
                    SetComboValue(cmbState, "DC")
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Private Sub SetComboValue(ByRef cmb As DropDownList, _
                                 ByVal sValue As String)
            Dim i As Integer

            Try
                For i = 0 To cmb.Items.Count - 1
                    If String.Compare(cmb.Items(i).Value, sValue, True) = 0 Then
                        cmb.Items(i).Selected = True
                        Exit Sub
                    End If

                    If String.Compare(cmb.Items(i).Text, sValue, True) = 0 Then
                        cmb.Items(i).Selected = True
                        Exit Sub
                    End If

                Next
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
#Region "Address Methods"

        Private Sub PopulateState(ByRef cmbPopulateState As DropDownList, ByRef cmbCurrentCountry As DropDownList)
            Try
                Dim sSQL As String
                sSQL = Database & "..spGetStateList @CountryID=" & cmbCurrentCountry.SelectedValue.ToString
                cmbPopulateState.DataSource = DataAction.GetDataTable(sSQL)
                cmbPopulateState.DataTextField = "State"
                cmbPopulateState.DataValueField = "State"
                cmbPopulateState.DataBind()
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub cmbCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCountry.SelectedIndexChanged
            PopulateState(cmbState, cmbCountry)
            txtZipCode.Focus()
        End Sub


        Protected Sub cmbHomeCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbHomeCountry.SelectedIndexChanged
            PopulateState(cmbHomeState, cmbHomeCountry)
        End Sub
        Protected Sub cmbBillingCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbBillingCountry.SelectedIndexChanged
            PopulateState(cmbBillingState, cmbBillingCountry)
        End Sub
        Protected Sub cmbPOBoxCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbPOBoxCountry.SelectedIndexChanged
            PopulateState(cmbPOBoxState, cmbPOBoxCountry)
        End Sub

        Private Sub PopulateDropDowns()
            Dim sSQL As String
            Dim dt As DataTable
            Try

                sSQL = AptifyApplication.GetEntityBaseDatabase("Addresses") & _
                       "..spGetCountryList"
                dt = DataAction.GetDataTable(sSQL)
                dt.Rows(0).Delete()
                cmbCountry.DataSource = dt
                cmbCountry.DataTextField = "Country"
                cmbCountry.DataValueField = "ID"
                cmbCountry.DataBind()


                cmbHomeCountry.DataSource = dt
                cmbHomeCountry.DataTextField = "Country"
                cmbHomeCountry.DataValueField = "ID"
                cmbHomeCountry.DataBind()

                cmbBillingCountry.DataSource = dt
                cmbBillingCountry.DataTextField = "Country"
                cmbBillingCountry.DataValueField = "ID"
                cmbBillingCountry.DataBind()

                cmbPOBoxCountry.DataSource = dt
                cmbPOBoxCountry.DataTextField = "Country"
                cmbPOBoxCountry.DataValueField = "ID"
                cmbPOBoxCountry.DataBind()

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
#End Region


        Private Sub LoadUserInfo()
            Try
                'Neha changes for 16177
                LoadInformation()
                ddlAddressType.ClearSelection()
                SetComboValue(ddlAddressType, dt.Rows.Item(0)("PreferredAddress").ToString().Trim())
                DisplayAddress(dt.Rows.Item(0)("PreferredAddress").ToString().Trim())
                chkPrefAddress.Checked = True
                chkPrefAddress.Enabled = False
                Me.PreferredAddress = dt.Rows.Item(0)("PreferredAddress").ToString().Trim()
                LoadProfilePicture(Nothing)

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Overrides Sub SetProperties()

            'Anil B for issue 15387 on 05-04-2013
            'Set Descript url ID
            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(PersonImageURL) Then
                PersonImageURL = Me.GetLinkValueFromXML(ATTRIBUTE_PERSON_IMAGE_URL)
            End If
            If String.IsNullOrEmpty(BlankImage) Then
                BlankImage = Me.GetPropertyValueFromXML(ATTRIBUTE_PERSON_BLANK_IMG)
            End If

            If Not String.IsNullOrEmpty(ImageUploadUserProfileText) Then
                LableImageUploadText.Text = ImageUploadUserProfileText
            End If
            If String.IsNullOrEmpty(EditAddressPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                EditAddressPage = Me.GetLinkValueFromXML(ATTRIBUTE_EDIT_ADDRESS)
            End If
            'If Not String.IsNullOrEmpty(ImageUploadUserProfileSaveText) Then
            '    LableImageSaveIndicator.Text = ImageUploadUserProfileSaveText
            'End If

            If ImageWidth = -1 Then
                Dim sWidth As String = ""
                sWidth = Me.GetPropertyValueFromXML(ATTRIBUTE_PERSON_IMG_WIDTH)
                If IsNumeric(sWidth) Then
                    ImageWidth = CInt(sWidth)
                End If
            End If
            If ImageHeight = -1 Then
                Dim sHeight As String = ""
                sHeight = Me.GetPropertyValueFromXML(ATTRIBUTE_PERSON_IMG_HEIGHT)
                If IsNumeric(sHeight) Then
                    ImageHeight = CInt(sHeight)
                End If
            End If
            'Added by Sandeep for Issue 15051 on 12/03/2013
            If String.IsNullOrEmpty(LoginPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                LoginPage = Me.GetLinkValueFromXML(ATTRIBUTE_LOGIN_PAGE)
            End If

        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            SetProperties()
            'Anil B for issue 15387 on 05-04-2013
            'Set Descript url ID
            If Me.SetControlRecordIDFromQueryString() AndAlso Me.SetControlRecordIDFromParam() Then
                Session("PersonID") = Me.ControlRecordID.ToString()
            Else
                Session("PersonID") = -1
            End If
            btnRemovePhoto.Visible = False
            radImageEditor.Enabled = False
            ShowCropButton(False)

            If IsPostBack AndAlso imgProfile.ImageUrl <> String.Empty AndAlso ProfilePhotoFileName <> "" Then
                Dim sRandom As String = New Random().Next().ToString()
                imgProfile.ImageUrl = PersonImageURL & ProfilePhotoFileName & "?" & sRandom

                'If imgProfile.ImageUrl <> PersonImageURL & BlankImage Then
                If imgProfile.ImageUrl.Substring(0, imgProfile.ImageUrl.Length - (sRandom.Length + 1)) <> PersonImageURL & BlankImage Then
                    btnRemovePhoto.Visible = True
                    radImageEditor.Enabled = True
                    'ShowCropButton(True)
                End If
            End If

            If Not IsPostBack Then
                'Anil B for issue 15387 on 05-04-2013
                'Check person belongs to company
                If Session("PersonID") IsNot Nothing AndAlso IsNumeric(Session("PersonID")) AndAlso CLng(Session("PersonID")) <> -1 AndAlso IsCompanyPerson(Session("PersonID")) Then
                    LoadForm()
                    'Amruta 14307 start
                    TopiccodeViewer.EntityName = "Persons"
                    TopiccodeViewer.RecordID = Session.Item("PersonID")
                    TopiccodeViewer.ButtonDisplay = True
                    TopiccodeViewer.lbldispaly = False
                    LoadTopicCodesParent(m_sEntityName, Session.Item("PersonID").ToString())
                    'Amruta 14307 end
                    ltlImageEditorStyle.Text = "<style type=""text/css""> #" & radImageEditor.ClientID & "_ToolsPanel { display: none !important; } #" & radwindowProfileImage.ClientID & "_C { overflow: hidden !important; }</style>"
                Else
                    Response.Redirect(System.Configuration.ConfigurationManager.AppSettings("virtualdir") & SecurityErrorPage & "?Message=Access to this Person is unauthorized.")
                End If
            End If
            If User1.UserID < 0 Then
                Response.Redirect(LoginPage) 'Added by Sandeep for Issue 15051 on 12/03/2013
            End If

        End Sub

        Protected Sub ShowCropButton(ByVal isVisible As Boolean)
            If isVisible = True Then
                btnCropImage.Style.Add("display", "inline")
            Else
                btnCropImage.Style.Add("display", "none")
            End If
        End Sub




        Protected Sub contact_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles contact.Click
            LoadForm()
            radwindowcontact.VisibleOnPageLoad = True
            LoadProfilePicture(Nothing)
            UpdateImageSize(ImageData)
            'Amruta IssueID 14623
            lblmsg.Visible = False
        End Sub



        Protected Sub btnSaveIntrest_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveIntrest.Click
            TopiccodeViewer.SaveTopicCode()
            radtopicintrest.VisibleOnPageLoad = False
            LoadTopicCodesParent(m_sEntityName, Session.Item("PersonID").ToString())
        End Sub



        Private Sub LoadAddresses()
            Try
                If dt.Rows.Item(0) IsNot Nothing Then


                    If dt.Rows.Item(0)("AddressID") IsNot Nothing Or dt.Rows.Item(0)("BusinessAddressLine1") IsNot Nothing Then
                        txtAddressLine1.Text = dt.Rows.Item(0)("BusinessAddressLine1").ToString()
                        txtAddressLine2.Text = dt.Rows.Item(0)("BusinessAddressLine2").ToString()
                        txtAddressLine3.Text = dt.Rows.Item(0)("BusinessAddressLine3").ToString()
                        txtCity.Text = dt.Rows.Item(0)("Businesscity").ToString()
                        txtZipCode.Text = dt.Rows.Item(0)("BusinessZipCode").ToString()
                    End If

                    'Put inside If statement if you don't want to default the address to US
                    SetComboValue(cmbCountry, IIf(dt.Rows.Item(0)("BusinessCountry").ToString() = "", "United States", dt.Rows.Item(0)("BusinessCountry")).ToString)
                    PopulateState(cmbState, cmbCountry)
                    SetComboValue(cmbState, dt.Rows.Item(0)("BusinessState").ToString())

                    If dt.Rows.Item(0)("HomeAddressID") IsNot Nothing Or dt.Rows.Item(0)("HomeAddressLine1") IsNot Nothing Then

                        txtHomeAddressLine1.Text = dt.Rows.Item(0)("HomeAddressLine1").ToString()
                        txtHomeAddressLine2.Text = dt.Rows.Item(0)("HomeAddressLine2").ToString()
                        txtHomeAddressLine3.Text = dt.Rows.Item(0)("HomeAddressLine3").ToString()
                        txtHomeCity.Text = dt.Rows.Item(0)("HomeCity").ToString()
                        txtHomeZipCode.Text = dt.Rows.Item(0)("HomeZipCode").ToString()
                    End If

                    'Populate Home country or default to United States
                    SetComboValue(cmbHomeCountry, IIf(dt.Rows.Item(0)("HomeCountry").ToString() = "", "United States", dt.Rows.Item(0)("HomeCountry")).ToString)
                    PopulateState(cmbHomeState, cmbHomeCountry)
                    SetComboValue(cmbHomeState, dt.Rows.Item(0)("HomeState").ToString())

                    If dt.Rows.Item(0)("BillingAddressID") IsNot Nothing Or dt.Rows.Item(0)("BillingAddressLine1") IsNot Nothing Then
                        txtBillingAddressLine1.Text = dt.Rows.Item(0)("BillingAddressLine1").ToString()
                        txtBillingAddressLine2.Text = dt.Rows.Item(0)("BillingAddressLine2").ToString()
                        txtBillingAddressLine3.Text = dt.Rows.Item(0)("BillingAddressLine3").ToString()
                        txtBillingCity.Text = dt.Rows.Item(0)("BillingCity").ToString()
                        txtBillingZipCode.Text = dt.Rows.Item(0)("BillingZipCode").ToString()
                    End If

                    'Populate Billing country or default to United States
                    SetComboValue(cmbBillingCountry, IIf(dt.Rows.Item(0)("BillingCountry").ToString() = "", "United States", dt.Rows.Item(0)("BillingCountry")).ToString)
                    PopulateState(cmbBillingState, cmbBillingCountry)
                    SetComboValue(cmbBillingState, dt.Rows.Item(0)("BillingState").ToString())

                    If dt.Rows.Item(0)("POBoxAddressID") IsNot Nothing Or dt.Rows.Item(0)("POBox") IsNot Nothing Then
                        txtPOBoxAddressLine1.Text = dt.Rows.Item(0)("POBox").ToString()
                        txtPOBoxAddressLine2.Text = dt.Rows.Item(0)("POBoxLine2").ToString()
                        txtPOBoxAddressLine3.Text = dt.Rows.Item(0)("POBoxLine3").ToString()
                        txtPOBoxCity.Text = dt.Rows.Item(0)("POBoxCity").ToString()
                        txtPOBoxZipCode.Text = dt.Rows.Item(0)("POBoxZipCode").ToString()
                    End If

                    'Populate pobox country or default to united states
                    SetComboValue(cmbPOBoxCountry, IIf(dt.Rows.Item(0)("POBoxCountry").ToString() = "", "United States", dt.Rows.Item(0)("POBoxCountry")).ToString)
                    PopulateState(cmbPOBoxState, cmbPOBoxCountry)
                    SetComboValue(cmbPOBoxState, dt.Rows.Item(0)("POBoxState").ToString())
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Protected Sub ddlAddressType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlAddressType.SelectedIndexChanged

            'LoadInformation()
            'PopulateDropDowns()
            Me.DisplayAddress(ddlAddressType.SelectedItem.Value)
            If Me.PreferredAddress = ddlAddressType.SelectedItem.Value Then
                chkPrefAddress.Checked = True
                chkPrefAddress.Enabled = False
                txtAddressLine1.Focus()
            Else
                chkPrefAddress.Checked = False
                chkPrefAddress.Enabled = True
                chkPrefAddress.Focus()
            End If

        End Sub
        Private Sub DisplayAddress(ByVal sAddrType As String)
            Try

                Select Case sAddrType
                    Case "Business Address"

                        trAddressLine1.Visible = True
                        trAddressLine2.Visible = True
                        trAddressLine3.Visible = True
                        trCity.Visible = True
                        'trState.Visible = True
                        'trZipCode.Visible = True
                        trCountry.Visible = True

                        '*** Uncomment if you want to default address to US. ***
                        If CType(Session.Item("PersonID"), Long) < 1 Then
                            cmbCountry.ClearSelection()
                            SetComboValue(cmbCountry, "United States")
                            PopulateState(cmbState, cmbCountry)
                        End If

                        trHomeAddressLine1.Visible = False
                        trHomeAddressLine2.Visible = False
                        trHomeAddressLine3.Visible = False
                        trHomeCity.Visible = False
                        trHomeCountry.Visible = False
                        'trHomeState.Visible = False
                        'trHomeZipCode.Visible = False

                        trBillingAddressLine1.Visible = False
                        trBillingAddressLine2.Visible = False
                        trBillingAddressLine3.Visible = False
                        trBillingCity.Visible = False
                        trBillingCountry.Visible = False
                        'trBillingState.Visible = False
                        'trBillingZipCode.Visible = False

                        trPOBoxAddressLine1.Visible = False
                        trPOBoxAddressLine2.Visible = False
                        trPOBoxAddressLine3.Visible = False
                        trPOBoxCity.Visible = False
                        trPOBoxCountry.Visible = False
                        'trPOBoxState.Visible = False
                        'trPOBoxZipCode.Visible = False

                    Case "Home Address"
                        trAddressLine1.Visible = False
                        trAddressLine2.Visible = False
                        trAddressLine3.Visible = False
                        trCity.Visible = False
                        'trState.Visible = False
                        'trZipCode.Visible = False
                        trCountry.Visible = False

                        trHomeAddressLine1.Visible = True
                        trHomeAddressLine2.Visible = True
                        trHomeAddressLine3.Visible = True
                        trHomeCity.Visible = True
                        trHomeCountry.Visible = True
                        'trHomeState.Visible = True
                        'trHomeZipCode.Visible = True

                        '*** Uncomment if you want to default address to US. ***
                        If CType(Session.Item("PersonID"), Long) < 1 Then
                            cmbHomeCountry.ClearSelection()
                            SetComboValue(cmbHomeCountry, "United States")
                            PopulateState(cmbHomeState, cmbHomeCountry)
                        End If


                        trBillingAddressLine1.Visible = False
                        trBillingAddressLine2.Visible = False
                        trBillingAddressLine3.Visible = False
                        trBillingCity.Visible = False
                        trBillingCountry.Visible = False
                        'trBillingState.Visible = False
                        'trBillingZipCode.Visible = False

                        trPOBoxAddressLine1.Visible = False
                        trPOBoxAddressLine2.Visible = False
                        trPOBoxAddressLine3.Visible = False
                        trPOBoxCity.Visible = False
                        trPOBoxCountry.Visible = False
                        'trPOBoxState.Visible = False
                        'trPOBoxZipCode.Visible = False
                    Case "Billing Address"
                        trAddressLine1.Visible = False
                        trAddressLine2.Visible = False
                        trAddressLine3.Visible = False
                        trCity.Visible = False
                        'trState.Visible = False
                        'trZipCode.Visible = False
                        trCountry.Visible = False

                        trHomeAddressLine1.Visible = False
                        trHomeAddressLine2.Visible = False
                        trHomeAddressLine3.Visible = False
                        trHomeCity.Visible = False
                        trHomeCountry.Visible = False
                        'trHomeState.Visible = False
                        'trHomeZipCode.Visible = False

                        trBillingAddressLine1.Visible = True
                        trBillingAddressLine2.Visible = True
                        trBillingAddressLine3.Visible = True
                        trBillingCity.Visible = True
                        trBillingCountry.Visible = True
                        'trBillingState.Visible = True
                        'trBillingZipCode.Visible = True

                        '*** Uncomment if you want to default address to US. ***
                        If CType(Session.Item("PersonID"), Long) < 1 Then
                            cmbBillingCountry.ClearSelection()
                            SetComboValue(cmbBillingCountry, "United States")
                            PopulateState(cmbBillingState, cmbBillingCountry)
                        End If


                        trPOBoxAddressLine1.Visible = False
                        trPOBoxAddressLine2.Visible = False
                        trPOBoxAddressLine3.Visible = False
                        trPOBoxCity.Visible = False
                        trPOBoxCountry.Visible = False
                        'trPOBoxState.Visible = False
                        'trPOBoxZipCode.Visible = False
                    Case "PO Box Address"
                        trAddressLine1.Visible = False
                        trAddressLine2.Visible = False
                        trAddressLine3.Visible = False
                        trCity.Visible = False
                        'trState.Visible = False
                        'trZipCode.Visible = False
                        trCountry.Visible = False

                        trHomeAddressLine1.Visible = False
                        trHomeAddressLine2.Visible = False
                        trHomeAddressLine3.Visible = False
                        trHomeCity.Visible = False
                        trHomeCountry.Visible = False
                        'trHomeState.Visible = False
                        'trHomeZipCode.Visible = False

                        trBillingAddressLine1.Visible = False
                        trBillingAddressLine2.Visible = False
                        trBillingAddressLine3.Visible = False
                        trBillingCity.Visible = False
                        trBillingCountry.Visible = False
                        'trBillingState.Visible = False
                        'trBillingZipCode.Visible = False

                        trPOBoxAddressLine1.Visible = True
                        trPOBoxAddressLine2.Visible = True
                        trPOBoxAddressLine3.Visible = True
                        trPOBoxCity.Visible = True
                        trPOBoxCountry.Visible = True
                        'trPOBoxState.Visible = True
                        'trPOBoxZipCode.Visible = True

                        '*** Uncomment if you want to default address to US. ***
                        If CType(Session.Item("PersonID"), Long) < 1 Then
                            cmbPOBoxCountry.ClearSelection()
                            SetComboValue(cmbPOBoxCountry, "United States")
                            PopulateState(cmbPOBoxState, cmbPOBoxCountry)
                        End If

                End Select

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub btnTopicIntrest_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTopicIntrest.Click
            radtopicintrest.VisibleOnPageLoad = True
            'Amruta,IssueID 14307,To load users topic code interest
            TopiccodeViewer.LoadTree()
            Dim lbl As Label
            lbl = TopiccodeViewer.FindControl("Topicskeep")
            lbl.Text = Nothing
            'Amruta IssueID 14623
            lblmsg.Visible = False
        End Sub

        Private Sub LoadInformation()
            Dim sSQL As String, sDB As String
            Dim strBusiness As String = ""
            Dim strBilling As String = ""
            Dim strHome As String = ""
            Dim strPoBox As String = ""
            Dim strphoneAreaCode As String = ""
            Dim strFaxAreaCode As String = ""

            Try
                Dim objaptifyapp As New AptifyApplication()
                sDB = objaptifyapp.GetEntityBaseDatabase("Persons")
                sSQL = "select VP.ID,VP.FirstName,VP.LastName,VP.Email,VP.MemberType,VP.Title,VP.Photo,VP.PreferredAddress," & _
               "VP.BillingAddressLine1,VP.BillingAddressLine2,VP.BillingAddressLine3,VP.BillingAddressLine4,VP.BillingCity, VP.BillingState, VP.BillingCountry,VP.BillingZipCode, VP.HomeAddressLine1," & _
               " VP.HomeAddressLine2, VP.HomeAddressLine3,VP.HomeAddressLine4,VP.HomeCity,VP.HomeCountry,VP.HomeState,VP.HomeZipCode," & _
                "VP.POBox,VP.POBoxLine2, VP.POBoxLine3, VP.POBoxLine4, VP.POBoxCity, VP.POBoxState, VP.POBoxCountry, VP.POBoxZipCode, VP.AddressID,VP.BillingAddressID,VP.HomeAddressID,cast(VP.POBoxAddressID as varchar(100))as POBoxAddressID,VP.Company," & _
            "BillingAddress= VP.BillingAddressLine1+ '<br/>'+VP.BillingAddressLine2+ '<br/>'+VP.BillingAddressLine3+ '<br/>'+   " & _
              " case when VP.BillingCity='' then '' else VP.billingcity  end  + " & _
             "case When VP.BillingState = '' then '' else  ',' + VP.BillingState  end +" & _
               "case When VP.BillingCountry = '' then '' else ',' + VP.BillingCountry  end +" & _
             "case When VP.BillingZipCode = '' then '' else  '-' +VP.BillingZipCode end," & _
           "HomeAddress=VP.HomeAddressLine1+ '<br/>'+VP.HomeAddressLine2+ '<br/>'+VP.HomeAddressLine3+ '<br/>'+ " & _
          "case when VP.HomeCity='' then '' else VP.HomeCity  end  + " & _
          "case When VP.HomeState = '' then '' else ',' + VP.HomeState  end +" & _
          "case When VP.HomeCountry = '' then ''  else  ',' + VP.HomeCountry  end +" & _
            "case When VP.HomeZipCode = '' then '' else  '-' + VP.HomeZipCode end," & _
           "BusinessAddress=VP.AddressLine1+ '<br/>'+ VP.AddressLine2+ '<br/>'+VP.AddressLine3+ '<br/>'+" & _
          "case when VP.City='' then '' else VP.City  end  + " & _
          "case When VP.State = '' then '' else  ',' + VP.State  end +" & _
           " case When VP.Country = '' then '' else ',' + VP.Country  end +" & _
            "case When VP.ZipCode = '' then '' else  '-' + VP.ZipCode end," & _
          "POAddress=VP.POBox + '<br/>'+cast(VP.POBoxAddressID as varchar(100))+ '<br/>'+VP.POBoxLine2+ '<br/>'+VP.POBoxLine3+ '<br/>'+" & _
          "case when VP.POBoxCity='' then ''  else VP.POBoxCity  end  + " & _
          " case When VP.POBoxState = '' then '' else  ',' + VP.POBoxState  end +case " & _
          "When VP.POBoxCountry = '' then '' else ',' + VP.POBoxCountry  end +" & _
           "case When VP.POBoxZipCode = '' then '' else  '-' + VP.POBoxZipCode end,VP.Phone,VP.PhoneAreaCode,VP.FaxPhone,VP.FaxAreaCode" & _
           " ,vp.id,VP.LastName,VP.FirstName,VP.Email,VP.MemberType,Vp.title,Vp.photo " & _
             ",address=case when vp.City ='' or vp.State= '' then (isnull(VP.city,'')+isnull(vp.State,'')) else (Vp.city +',' + vp.state)  end," & _
           "Case  When Convert(Varchar(12),VP.JoinDate,107)='Jan 01, 1900' Then 'N/A' When Convert(Varchar(12),VP.JoinDate,107)is null  Then 'N/A' else Convert(Varchar(12),VP.JoinDate,107)  end JoinDate," & _
           "(isnull( Convert(Varchar (12),VP.DuesPaidThru,107),'N/A'))  DuesPaidThru  ,VP.PreferredAddress ,VP.BillingAddressLine1, VP.BillingAddressLine2," & _
           "VP.BillingAddressLine3,VP.BillingAddressLine4,VP.BillingCity,VP.BillingState,VP.BillingCountry, " & _
       "  VP.BillingZipCode,vp.HomeAddressLine1,VP.HomeAddressLine2, VP.HomeAddressLine3, vp.HomeAddressLine4,VP.HomeCity,vp.HomeCountry, VP.HomeState," & _
       "  vp.HomeZipCode,VP.AddressLine1 as BusinessAddressLine1,VP.AddressLine2 as  BusinessAddressLine2,VP.AddressLine3 as BusinessAddressLine3, " & _
       " VP.AddressLine4 as BusinessAddressLine4,VP.City as  Businesscity,VP.State as BusinessState,VP.Country as BusinessCountry, VP.ZipCode as BusinessZipCode , " & _
       "VP.POBox,VP.POBoxAddressID,VP.POBoxLine2,VP.POBoxLine3,VP.POBoxLine4,VP.POBoxCity,VP.POBoxState,VP.POBoxCountry,VP.POBoxZipCode,VP.AddressID," & _
                "VP.BillingAddressID, VP.HomeAddressID, VP.Company, vp.Phone, VP.PhoneAreaCode, VP.FaxPhone, VP.FaxAreaCode" & _
       " from " & sDB & _
       " ..vwPersons VP where VP.ID=" & CType(Session.Item("PersonID"), Long)

                dt = Me.DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                Dim dcolstatus As DataColumn = New DataColumn()
                dcolstatus.Caption = "Status"
                dcolstatus.ColumnName = "Status"
                dt.Columns.Add(dcolstatus)
                If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                    For Each rw As DataRow In dt.Rows
                        If rw("DuesPaidThru").ToString() = "N/A" OrElse rw("JoinDate").ToString() = "N/A" Then
                            rw("Status") = "Unavailable"
                        ElseIf Convert.ToDateTime(rw("DuesPaidThru").ToString()) = Convert.ToDateTime(rw("JoinDate").ToString()).AddDays(-1) Then
                            rw("DuesPaidThru") = rw("JoinDate")
                            rw("Status") = "Expired"
                        ElseIf Convert.ToDateTime(rw("DuesPaidThru").ToString()) > Date.Now().AddDays(90) Then
                            rw("Status") = "Active"
                        ElseIf Convert.ToDateTime(rw("DuesPaidThru").ToString()) > Date.Now() AndAlso rw("DuesPaidThru") < Date.Now().AddDays(90) Then
                            rw("Status") = "Expiring"
                        ElseIf Convert.ToDateTime(rw("DuesPaidThru").ToString()) < Date.Now() Then
                            rw("Status") = "Expired"
                        End If
                    Next
                End If

                LoadProfilePicture(Nothing)
                ''Personal Information
                Session("PersonID") = dt.Rows.Item(0)("ID").ToString()
                lblEditFirstName.Text = dt.Rows.Item(0)("FirstName").ToString()
                txtEditFirstName.Text = dt.Rows.Item(0)("FirstName").ToString()
                lblEditLastName.Text = dt.Rows.Item(0)("LastName").ToString()
                txtEditLastName.Text = dt.Rows.Item(0)("LastName").ToString()
                lblEditCompany.Text = dt.Rows.Item(0)("Company").ToString()
                If dt.Rows.Item(0)("Title").ToString() = "" Then
                    lblTitle.Visible = False
                    lblJobFunction.Visible = False
                    trTitle.Visible = False
                Else
                    trTitle.Visible = True
                    lblTitle.Visible = True
                    lblJobFunction.Visible = True
                    lblJobFunction.Text = dt.Rows.Item(0)("Title").ToString()
                    txtEditJobFunction.Text = dt.Rows.Item(0)("Title").ToString()
                End If
                'Amruta IssueID 14307 start
                If dt.Rows.Item(0)("Email").ToString() = "" Then
                    lblEmailID.Visible = False
                    lblEmailAddress.Visible = False
                    trEmail.Visible = False
                Else
                    trEmail.Visible = True
                    lblEmailID.Visible = True
                    lblEmailAddress.Visible = True
                    lblEmailAddress.Text = dt.Rows.Item(0)("Email").ToString()
                End If
                'Amruta IssueID 14307 end

                ''Membership Information
                If dt.Rows.Item(0)("MemberType").ToString() = "" Then
                    lblmembershipType.Visible = False
                Else
                    lblmembershipType.Visible = True
                    lblMemberTypeVal.Text = dt.Rows.Item(0)("MemberType").ToString()

                End If

                If dt.Rows.Item(0)("JoinDate").ToString() = "" Then
                    lblStartDate.Visible = False
                Else
                    lblStartDate.Visible = True
                    lblStartDateVal.Text = dt.Rows.Item(0)("JoinDate").ToString()
                End If

                If dt.Rows.Item(0)("JoinDate").ToString() = "" Then
                    lblEndDate.Visible = False
                Else
                    lblEndDate.Visible = True
                    lblEndDateVal.Text = dt.Rows.Item(0)("DuesPaidThru").ToString()
                End If
                If dt.Rows.Item(0)("Status").ToString() = "" Then
                    lblStatus.Visible = False
                Else
                    lblStatus.Visible = True
                    lblStatusVal.Text = dt.Rows.Item(0)("Status").ToString()
                End If

                '' Contact Information
                If dt.Rows.Item(0)("BusinessAddress").ToString() = "" Then
                    tdBusinessAdd.Visible = False
                    tdBusinessAddVal.Visible = False
                Else
                    tdBusinessAdd.Visible = True
                    tdBusinessAddVal.Visible = True
                End If
                If dt.Rows.Item(0)("HomeAddress").ToString() = "" Then
                    tdHomeAdd.Visible = False
                    tdHomeAddVal.Visible = False
                Else
                    tdHomeAdd.Visible = True
                    tdHomeAddVal.Visible = True
                End If
                If dt.Rows.Item(0)("BillingAddress").ToString() = "" Then
                    tdBillingAdd.Visible = False
                    tdBillingAddVal.Visible = False
                Else
                    tdBillingAdd.Visible = True
                    tdBillingAddVal.Visible = True
                End If
                If dt.Rows.Item(0)("POAddress").ToString() = "" Then
                    tdPoboxAdd.Visible = False
                    tdPoboxAddVal.Visible = False
                Else
                    tdPoboxAdd.Visible = True
                    tdPoboxAddVal.Visible = True
                End If

                ''Business Address
                If dt.Rows.Item(0)("BusinessAddressLine1").ToString() = "" Then

                    BusinessAdd1.Style.Add("display", "none")
                Else

                    BusinessAdd1.Text = dt.Rows.Item(0)("BusinessAddressLine1").ToString() + "<br/>"
                    BusinessAdd1.Style.Add("display", "block")
                End If
                If dt.Rows.Item(0)("BusinessAddressLine2").ToString() = "" Then

                    BusinessAdd2.Style.Add("display", "none")
                Else

                    BusinessAdd2.Text = dt.Rows.Item(0)("BusinessAddressLine2").ToString() + "<br/>"
                    BusinessAdd2.Style.Add("display", "block")
                End If
                If dt.Rows.Item(0)("BusinessAddressLine3").ToString() = "" Then

                    BusinessAdd3.Style.Add("display", "none")
                Else
                    BusinessAdd3.Text = dt.Rows.Item(0)("BusinessAddressLine3").ToString() + "<br/>"
                    BusinessAdd3.Style.Add("display", "block")
                End If

                If Not dt.Rows.Item(0)("Businesscity").ToString() = "" Then
                    If dt.Rows.Item(0)("BusinessState").ToString() = "" AndAlso dt.Rows.Item(0)("BusinessZipCode").ToString() = "" Then
                        BusinessCityState.Text = dt.Rows.Item(0)("Businesscity").ToString() + "<br/>"
                    Else
                        BusinessCityState.Text = dt.Rows.Item(0)("Businesscity").ToString() + ", " + dt.Rows.Item(0)("BusinessState").ToString() + " " + dt.Rows.Item(0)("BusinessZipCode").ToString() + "<br/>"
                    End If
                Else
                    If dt.Rows.Item(0)("BusinessState").ToString() = "" AndAlso dt.Rows.Item(0)("BusinessZipCode").ToString() = "" Then
                        BusinessCityState.Text = ""
                    Else
                        BusinessCityState.Text = dt.Rows.Item(0)("BusinessState").ToString() + " " + dt.Rows.Item(0)("BusinessZipCode").ToString() + "<br/>"
                    End If
                End If
                BusinessCountry.Text = dt.Rows.Item(0)("BusinessCountry").ToString()

                ''Home Address

                If dt.Rows.Item(0)("HomeAddressLine1").ToString() = "" Then
                    HomeAdd1.Text = ""
                    HomeAdd1.Style.Add("display", "none")
                Else

                    HomeAdd1.Text = dt.Rows.Item(0)("HomeAddressLine1").ToString()
                    HomeAdd1.Style.Add("display", "block")
                End If
                If dt.Rows.Item(0)("HomeAddressLine2").ToString() = "" Then
                    HomeAdd2.Text = ""
                    HomeAdd2.Style.Add("display", "none")
                Else
                    HomeAdd2.Text = dt.Rows.Item(0)("HomeAddressLine2").ToString() + "<br/>"
                    HomeAdd2.Style.Add("display", "block")
                End If
                If dt.Rows.Item(0)("HomeAddressLine3").ToString() = "" Then
                    HomeAdd3.Text = ""
                    HomeAdd3.Style.Add("display", "none")
                Else
                    HomeAdd3.Text = dt.Rows.Item(0)("HomeAddressLine3").ToString() + "<br/>"
                    HomeAdd3.Style.Add("display", "block")
                End If
                If Not dt.Rows.Item(0)("HomeCity").ToString() = "" Then
                    If dt.Rows.Item(0)("HomeState").ToString() = "" AndAlso dt.Rows.Item(0)("HomeZipCode").ToString() = "" Then
                        HomeCityState.Text = dt.Rows.Item(0)("HomeCity").ToString() + "<br/>"
                    Else
                        HomeCityState.Text = dt.Rows.Item(0)("HomeCity").ToString() + ", " + dt.Rows.Item(0)("HomeState").ToString() + " " + dt.Rows.Item(0)("HomeZipCode").ToString() + "<br/>"
                    End If
                Else
                    If dt.Rows.Item(0)("HomeState").ToString() = "" AndAlso dt.Rows.Item(0)("HomeZipCode").ToString() = "" Then
                        HomeCityState.Text = ""
                    Else
                        HomeCityState.Text = dt.Rows.Item(0)("HomeState").ToString() + " " + dt.Rows.Item(0)("HomeZipCode").ToString() + "<br/>"
                    End If
                End If
                HomeCountry.Text = dt.Rows.Item(0)("HomeCountry").ToString()

                'Billing Address
                If dt.Rows.Item(0)("BillingAddressLine1").ToString() = "" Then
                    BillingAdd1.Text = ""
                    BillingAdd1.Style.Add("display", "none")
                Else
                    BillingAdd1.Text = dt.Rows.Item(0)("BillingAddressLine1").ToString() + "<br/>"
                    BillingAdd1.Style.Add("display", "block")
                End If
                If dt.Rows.Item(0)("BillingAddressLine2").ToString() = "" Then
                    BillingAdd2.Text = ""
                    BillingAdd2.Style.Add("display", "none")
                Else
                    BillingAdd2.Text = dt.Rows.Item(0)("BillingAddressLine2").ToString() + "<br/>"
                    BillingAdd2.Style.Add("display", "block")
                End If
                If dt.Rows.Item(0)("BillingAddressLine3").ToString() = "" Then
                    BillingAdd3.Text = ""
                    BillingAdd3.Style.Add("display", "none")
                Else
                    BillingAdd3.Text = dt.Rows.Item(0)("BillingAddressLine3").ToString() + "<br/>"
                    BillingAdd3.Style.Add("display", "block")
                End If

                If Not dt.Rows.Item(0)("BillingCity").ToString() = "" Then
                    If dt.Rows.Item(0)("BillingState").ToString() = "" AndAlso dt.Rows.Item(0)("BillingZipCode").ToString() = "" Then
                        BillingCityState.Text = dt.Rows.Item(0)("BillingCity").ToString() + "<br/>"
                    Else
                        BillingCityState.Text = dt.Rows.Item(0)("BillingCity").ToString() + ", " + dt.Rows.Item(0)("BillingState").ToString() + " " + dt.Rows.Item(0)("BillingZipCode").ToString() + "<br/>"
                    End If
                Else
                    If dt.Rows.Item(0)("BillingState").ToString() = "" AndAlso dt.Rows.Item(0)("BillingZipCode").ToString() = "" Then
                        BillingCityState.Text = ""
                    Else
                        BillingCityState.Text = dt.Rows.Item(0)("BillingState").ToString() + " " + dt.Rows.Item(0)("BillingZipCode").ToString() + "<br/>"
                    End If
                End If

                BillingAddCountry.Text = dt.Rows.Item(0)("BillingCountry").ToString()

                'POBox Address
                If dt.Rows.Item(0)("POBox").ToString() = "" Then
                    PoBoxAdd1.Text = ""
                    PoBoxAdd1.Style.Add("display", "none")
                Else
                    PoBoxAdd1.Text = dt.Rows.Item(0)("POBox").ToString() + "<br/>"
                    PoBoxAdd1.Style.Add("display", "block")
                End If
                If dt.Rows.Item(0)("POBoxLine2").ToString() = "" Then
                    PoBoxAdd2.Text = ""
                    PoBoxAdd2.Style.Add("display", "none")
                Else
                    PoBoxAdd2.Text = dt.Rows.Item(0)("POBoxLine2").ToString() + "<br/>"
                    PoBoxAdd2.Style.Add("display", "block")
                End If
                If dt.Rows.Item(0)("POBoxLine3").ToString() = "" Then
                    PoBoxAdd3.Text = ""
                    PoBoxAdd3.Style.Add("display", "none")
                Else
                    PoBoxAdd3.Text = dt.Rows.Item(0)("POBoxLine3").ToString() + "<br/>"
                    PoBoxAdd3.Style.Add("display", "block")
                End If

                If Not dt.Rows.Item(0)("POBoxCity").ToString() = "" Then
                    If dt.Rows.Item(0)("POBoxState").ToString() = "" AndAlso dt.Rows.Item(0)("POBoxZipCode").ToString() = "" Then
                        PoBoxCityState.Text = dt.Rows.Item(0)("POBoxCity").ToString() + "<br/>"
                    Else
                        PoBoxCityState.Text = dt.Rows.Item(0)("POBoxCity").ToString() + ", " + dt.Rows.Item(0)("POBoxState").ToString() + " " + dt.Rows.Item(0)("POBoxZipCode").ToString() + "<br/>"
                    End If
                Else
                    If dt.Rows.Item(0)("POBoxState").ToString() = "" AndAlso dt.Rows.Item(0)("POBoxZipCode").ToString() = "" Then
                        PoBoxCityState.Text = ""
                    Else
                        PoBoxCityState.Text = dt.Rows.Item(0)("POBoxState").ToString() + " " + dt.Rows.Item(0)("POBoxZipCode").ToString() + "<br/>"
                    End If
                End If
                PoBoxCountry.Text = dt.Rows.Item(0)("POBoxCountry").ToString()

                If dt.Rows.Item(0)("PreferredAddress").ToString() = "" Then
                    lblPerAddressTitle.Visible = False
                Else
                    lblPerAddressTitle.Visible = True
                    lblPerferredAddress.Text = dt.Rows.Item(0)("PreferredAddress").ToString().Trim()
                End If
                If dt.Rows.Item(0)("PhoneAreaCode").ToString().Trim() = "" AndAlso (dt.Rows.Item(0)("Phone").ToString().Trim() = "-" Or dt.Rows.Item(0)("Phone").ToString().Trim() = "") Then
                    lblphonetitle.Visible = True
                    lblphoneVal.Visible = True
                    lblphoneVal.Text = "Not Provided"
                Else
                    lblphonetitle.Visible = True
                    lblphoneVal.Visible = True
                    txtPhoneAreaCode.Text = dt.Rows.Item(0)("PhoneAreaCode").ToString().Trim()
                    txtPhone.Text = dt.Rows.Item(0)("Phone").ToString().Trim()
                    If dt.Rows.Item(0)("PhoneAreaCode").ToString().Trim() = "" Then
                        strphoneAreaCode = ""
                    Else
                        strphoneAreaCode = "(" & dt.Rows.Item(0)("PhoneAreaCode").ToString().Trim() & ") "
                    End If
                    'Amruta IssueID 14307 phone number with a dash after the first three digits
                    If (dt.Rows.Item(0)("Phone").ToString().Trim().Contains("-")) Then
                        lblphoneVal.Text = strphoneAreaCode & dt.Rows.Item(0)("Phone").ToString().Trim()
                    Else
                        lblphoneVal.Text = strphoneAreaCode & dt.Rows.Item(0)("Phone").ToString().Trim().Substring(0, 3) & "-" & dt.Rows.Item(0)("Phone").ToString().Trim().Substring(3)
                    End If
                End If


                If dt.Rows.Item(0)("FaxAreaCode").ToString().Trim() = "" AndAlso (dt.Rows.Item(0)("FaxPhone").ToString().Trim() = "-" Or dt.Rows.Item(0)("FaxPhone").ToString().Trim() = "") Then
                    lblFaxtitle.Visible = True
                    lblFaxVal.Visible = True
                    lblFaxVal.Text = "Not Provided"
                Else
                    lblFaxtitle.Visible = True
                    lblFaxVal.Visible = True
                    txtFaxAreaCode.Text = dt.Rows.Item(0)("FaxAreaCode").ToString().Trim()
                    txtFaxPhone.Text = dt.Rows.Item(0)("FaxPhone").ToString().Trim()
                    If dt.Rows.Item(0)("FaxAreaCode").ToString().Trim() = "" Then
                        strFaxAreaCode = ""
                    Else
                        strFaxAreaCode = "(" & dt.Rows.Item(0)("FaxAreaCode").ToString().Trim() & ") "
                    End If
                    'Amruta IssueID 14307 fax number with a dash after the first three digits
                    If (dt.Rows.Item(0)("FaxPhone").ToString().Trim().Contains("-")) Then
                        lblFaxVal.Text = strFaxAreaCode & dt.Rows.Item(0)("FaxPhone").ToString().Trim()
                    Else
                        lblFaxVal.Text = strFaxAreaCode & dt.Rows.Item(0)("FaxPhone").ToString().Trim().Substring(0, 3) & "-" & dt.Rows.Item(0)("FaxPhone").ToString().Trim().Substring(3)
                    End If
                End If
                LoadAddresses()
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
#Region "Save Methods"


        Private Function DoSave() As Boolean
            Try
                Dim sCounty As String = ""
                'Dim m_oApp As AptifyApplication
                Dim oGE As Aptify.Applications.Persons.PersonsEntity = DirectCast(Me.AptifyApplication.GetEntityObject("Persons", CType(Session.Item("PersonID"), Long)), Aptify.Applications.Persons.PersonsEntity)
                With oGE
                    oGE.SetValue("FirstName", txtEditFirstName.Text)
                    oGE.SetValue("LastName", txtEditLastName.Text)
                    oGE.SetValue("Title", txtEditJobFunction.Text)

                    Me.DoPostalCodeLookup(txtZipCode.Text, CStr(IIf(cmbCountry.SelectedIndex >= 0, cmbCountry.SelectedValue, "")), sCounty, txtCity, cmbState)
                    oGE.SetValue("AddressLine1", txtAddressLine1.Text)
                    oGE.SetValue("AddressLine2", txtAddressLine2.Text)
                    oGE.SetValue("AddressLine3", txtAddressLine3.Text)
                    oGE.SetValue("City", txtCity.Text)
                    oGE.SetValue("State", CStr(IIf(cmbState.SelectedIndex >= 0, cmbState.SelectedValue, "")))
                    oGE.SetValue("ZipCode", txtZipCode.Text)
                    oGE.SetValue("CountryCodeID", CLng(IIf(cmbCountry.SelectedIndex >= 0, cmbCountry.SelectedItem.Value, ""))) '11/27/07,Added by Tamasa,Issue 5222.
                    oGE.SetValue("Country", CStr(IIf(cmbCountry.SelectedIndex >= 0, cmbCountry.SelectedValue, "")))
                    oGE.SetAddValue("County", sCounty)
                    sCounty = ""
                    Me.DoPostalCodeLookup(txtHomeZipCode.Text, CStr(IIf(cmbHomeCountry.SelectedIndex >= 0, cmbHomeCountry.SelectedValue, "")), sCounty, txtHomeCity, cmbHomeState)

                    oGE.SetValue("HomeAddressLine1", txtHomeAddressLine1.Text)
                    oGE.SetValue("HomeAddressLine2", txtHomeAddressLine2.Text)
                    oGE.SetValue("HomeAddressLine3", txtHomeAddressLine3.Text)
                    oGE.SetValue("HomeCity", txtHomeCity.Text)
                    oGE.SetValue("HomeState", CStr(IIf(cmbHomeState.SelectedIndex >= 0, cmbHomeState.SelectedValue, "")))
                    oGE.SetValue("HomeZipCode", txtHomeZipCode.Text)
                    oGE.SetValue("HomeCountryCodeID", CLng(IIf(cmbHomeCountry.SelectedIndex >= 0, cmbHomeCountry.SelectedItem.Value, "")))
                    oGE.SetValue("HomeCountry", CStr(IIf(cmbHomeCountry.SelectedIndex >= 0, cmbHomeCountry.SelectedValue, "")))
                    oGE.SetAddValue("HomeCounty", sCounty)
                    sCounty = ""
                    Me.DoPostalCodeLookup(txtBillingZipCode.Text, CStr(IIf(cmbBillingCountry.SelectedIndex >= 0, cmbBillingCountry.SelectedValue, "")), sCounty, txtBillingCity, cmbBillingState)
                    oGE.SetValue("BillingAddressLine1", txtBillingAddressLine1.Text)
                    oGE.SetValue("BillingAddressLine2", txtBillingAddressLine2.Text)
                    oGE.SetValue("BillingAddressLine3", txtBillingAddressLine3.Text)
                    oGE.SetValue("BillingCity", txtBillingCity.Text)
                    oGE.SetValue("BillingState", CStr(IIf(cmbBillingState.SelectedIndex >= 0, cmbBillingState.SelectedValue, "")))
                    oGE.SetValue("BillingZipCode", txtBillingZipCode.Text)
                    oGE.SetValue("BillingCountryCodeID", CLng(IIf(cmbBillingCountry.SelectedIndex >= 0, cmbBillingCountry.SelectedItem.Value, "")))
                    oGE.SetValue("BillingCountry", CStr(IIf(cmbBillingCountry.SelectedIndex >= 0, cmbBillingCountry.SelectedValue, "")))
                    oGE.SetAddValue("BillingCounty", sCounty)

                    sCounty = ""
                    Me.DoPostalCodeLookup(txtPOBoxZipCode.Text, CStr(IIf(cmbPOBoxCountry.SelectedIndex >= 0, cmbPOBoxCountry.SelectedValue, "")), sCounty, txtPOBoxCity, cmbPOBoxState)
                    oGE.SetValue("POBox", txtPOBoxAddressLine1.Text)
                    oGE.SetValue("POBoxLine2", txtPOBoxAddressLine2.Text)
                    oGE.SetValue("POBoxLine3", txtPOBoxAddressLine3.Text)
                    oGE.SetValue("POBoxCity", txtPOBoxCity.Text)
                    oGE.SetValue("POBoxState", CStr(IIf(cmbPOBoxState.SelectedIndex >= 0, cmbPOBoxState.SelectedValue, "")))
                    oGE.SetValue("POBoxZipCode", txtPOBoxZipCode.Text)
                    oGE.SetValue("POBoxCountryCodeID", CLng(IIf(cmbPOBoxCountry.SelectedIndex >= 0, cmbPOBoxCountry.SelectedItem.Value, "")))
                    oGE.SetValue("POBoxCountry", CStr(IIf(cmbPOBoxCountry.SelectedIndex >= 0, cmbPOBoxCountry.SelectedValue, "")))
                    oGE.SetAddValue("POBoxCounty", sCounty)
                    oGE.SetValue("PreferredAddress", Me.PreferredAddress)
                    oGE.SetValue("PreferredBillingAddress", Me.PreferredAddress)
                    oGE.SetValue("PreferredShippingAddress", Me.PreferredAddress)
                    'Amruta IssueID 14307 02/26/2013, To save fax and phone area code, number
                    If txtPhoneAreaCode.Text = "" AndAlso txtPhone.Text = "" Then
                        oGE.SetValue("PhoneID", -1)
                    Else
                        oGE.Fields("PhoneID").EmbeddedObject.SetValue("AreaCode", txtPhoneAreaCode.Text)
                        oGE.Fields("PhoneID").EmbeddedObject.SetValue("Phone", txtPhone.TextWithLiterals)
                    End If
                    If txtFaxPhone.Text = "" AndAlso txtFaxAreaCode.Text = "" Then
                        oGE.SetValue("FaxID", -1)
                    Else
                        oGE.Fields("FaxID").EmbeddedObject.SetValue("AreaCode", txtFaxAreaCode.Text)
                        oGE.Fields("FaxID").EmbeddedObject.SetValue("Phone", txtFaxPhone.TextWithLiterals)
                    End If
                    'Amruta Issue 14307,4/9/13, To prevent processflow(company Administrator) to execute
                    .SetAddValue("_xNewGroupAdminObjFlag", True)
                End With
                Dim sErrorString As String = ""
                oGE.Save(sErrorString)
                LoadForm()

                Return True

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return False
            End Try
        End Function

#End Region
        Protected Overridable Sub DoPostalCodeLookup(ByRef PostalCode As String, ByRef Country As String, ByRef County As String, ByRef txt As TextBox, ByVal cmb As DropDownList)
            Dim sAreaCode As String = Nothing, sCounty As String = Nothing, sCity As String = Nothing
            Dim sState As String = Nothing, sCongDist As String = Nothing, sCountry As String = Nothing
            Dim ISOCountry As String
            Try
                Dim oPostalCode As New Aptify.Framework.BusinessLogic.Address.AptifyPostalCode(Me.AptifyApplication.UserCredentials)
                Dim oAddressInfo As New Aptify.Framework.BusinessLogic.Address.AddressInfo(Me.AptifyApplication)

                ISOCountry = oAddressInfo.GetISOCode(CLng(Country))

                If oPostalCode.GetPostalCodeInfo(PostalCode, ISOCountry, _
                                        sCity, sState, _
                                        sAreaCode, , sCounty, , , , , , , , _
                                        sCongDist, sCountry, AllowGUI:=True) Then
                    If txt IsNot Nothing Then
                        If String.IsNullOrWhitespace(txt.Text) Then
                            txt.Text = sCity
                        End If

                    End If
                    If cmb IsNot Nothing Then
                        cmb.SelectedValue = sState
                    End If


                    County = sCounty

                End If

            Catch ex As Exception

            End Try
        End Sub
        Protected Sub chkPrefAddress_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkPrefAddress.CheckedChanged
            Me.PreferredAddress = ddlAddressType.SelectedItem.Value
            chkPrefAddress.Enabled = False
        End Sub

        Protected Sub btnCancelIntrest_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelIntrest.Click
            radtopicintrest.VisibleOnPageLoad = False
        End Sub

        Protected Sub btncontactsave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncontactsave.Click
            'If (Not txtPhoneAreaCode.Text = "") AndAlso (txtPhone.Text = "") Then
            '    lblphonemsg.Visible = True
            'Else
            '    lblphonemsg.Visible = False
            DoSave()
            If Request.QueryString("location") = "EditAddress" Then
                Dim Type As String = Request.QueryString("Type")
                ' Session("PageIndex") = Index
                Response.Redirect(EditAddressPage & "?ID=" & Session("PersonID") & "&Action=PersonAddress&Type=" & Type)
            End If
            radwindowcontact.VisibleOnPageLoad = False
            'End If

        End Sub

        Protected Sub btncontactcancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncontactcancel.Click
            radwindowcontact.VisibleOnPageLoad = False
            If Request.QueryString("location") = "EditAddress" Then
                Dim Type As String = Request.QueryString("Type")
                Response.Redirect(EditAddressPage & "?ID=" & Session("PersonID") & "&Action=PersonAddress&Type=" & Type)
            End If
        End Sub


        Private Sub LoadProfilePicture(ByVal ImageName As String)

            Dim sSQL As String = ""
            Dim dt As DataTable
            Dim sBaseURL As String
            Dim m_lRecordID As Integer

            m_lRecordID = CType(Session.Item("PersonID"), Long)
            sSQL = "Select Photo from vwPersons Where ID = " & m_lRecordID

            dt = DataAction.GetDataTable(sSQL)
            If Not dt Is Nothing Then
                If Not IsDBNull(dt.Rows(0)("Photo")) Then
                    DeleteDownloadedPhotos()
                    GetNewProfilePhotoFileName()
                    Dim uniqueGuid As String = System.Guid.NewGuid.ToString
                    Dim ImagePath As String = ProfilePhotoMapPath & ProfilePhotoFileName
                    Dim newImgData() As Byte
                    ImageData = DirectCast(dt.Rows(0)("Photo"), [Byte]())
                    If ImageData.Length > 0 Then
                        Dim client As New System.Net.WebClient
                        client.UploadData(ImagePath, "POST", ImageData)

                        UpdateImageSize(ImageData)
                        imgProfile.ImageUrl = PersonImageURL & ProfilePhotoFileName & "?" & New Random().Next().ToString()
                        radImageEditor.ImageUrl = PersonImageURL & ProfilePhotoFileName
                        newImgData = ConvertImagetoByte(ImagePath, True)
                        If CompareTwoImageBytes(newImgData) Then
                            btnRemovePhoto.Visible = False
                            radImageEditor.Enabled = False
                            ShowCropButton(False)
                        Else
                            btnRemovePhoto.Visible = True
                            radImageEditor.Enabled = True
                            'ShowCropButton(True)
                        End If

                    Else
                        SetBlankImage()
                    End If
                Else
                    SetBlankImage()
                End If
            Else
                SetBlankImage()
            End If
            'Anil End

        End Sub
        Protected Overridable Function UpdateImageSize(ByRef ImageByte As Byte()) As Boolean
            Try
                Dim NewImgeWidth As Integer
                Dim originalImage As System.Drawing.Image
                If ImageByte IsNot Nothing AndAlso ImageByte.Length > 0 Then
                    imgProfile.Width = ImageWidth '142 assigned
                    imgProfile.Height = ImageHeight '142 assigned
                    ' conrt imageByte to virtual Image
                    Dim sMemstrm As MemoryStream = New MemoryStream(ImageByte)
                    If sMemstrm IsNot Nothing Then
                        originalImage = System.Drawing.Image.FromStream(sMemstrm)
                        If originalImage IsNot Nothing Then
                            Dim lVirtualImageHeight As Long
                            Dim lVirtualImageWidth As Long
                            lVirtualImageHeight = originalImage.Height  'original image height
                            lVirtualImageWidth = originalImage.Width
                            NewImgeWidth = originalImage.Width
                            If ImageWidth = originalImage.Width Then
                                NewImgeWidth = originalImage.Width - 1
                            End If
                            'original image width 
                            Dim commonMethods As New Aptify.Framework.Web.eBusiness.CommonMethods()
                            Dim aspratio As AspectRatio = New AspectRatio()
                            aspratio.WidthAndHeight(NewImgeWidth, originalImage.Height, ImageWidth, ImageHeight)
                            originalImage = commonMethods.byteArrayToImage(ImageByte)
                            ImageByte = commonMethods.resizeImageAndGetAsByte(originalImage, aspratio.Width, ImageHeight)
                            'If aspratio.Width = 0 Then
                            '    aspratio.Width = originalImage.Width
                            'End If
                            'If originalImage.Height < ImageHeight AndAlso originalImage.Width < ImageWidth Then
                            imgProfile.Width = originalImage.Width
                            imgProfile.Height = originalImage.Height
                            'Else
                            imgProfile.Width = aspratio.Width
                            imgProfile.Height = ImageHeight

                        End If
                    End If
                End If
                Return True
            Catch ex As Exception
                ExceptionManagement.ExceptionManager.Publish(ex)
                Return False
            End Try

        End Function



        Protected Overridable Sub SetBlankImage()
            imgProfile.ImageUrl = PersonImageURL & BlankImage & "?" & New Random().Next().ToString()
            radImageEditor.ImageUrl = PersonImageURL & BlankImage
            ViewState(ATTRIBUTE_PROFILE_PHOTO_FILENAME) = BlankImage
            imgProfile.Width = m_iImageWidth
            imgProfile.Height = m_iImageHeight
            btnRemovePhoto.Visible = False
            radImageEditor.Enabled = False
            ShowCropButton(False)
        End Sub
        Private Function ConvertImagetoByte(ByVal spath As String, ByVal bIspageLoad As Boolean) As Byte()
            Try
                Dim sFile As String
                sFile = spath
                Dim fInfo As New FileInfo(sFile)
                If fInfo.Exists Then
                    Dim len As Long = fInfo.Length
                    Dim imgData() As Byte
                    Using Stream As New FileStream(sFile, FileMode.Open)
                        imgData = New Byte(Convert.ToInt32(len - 1)) {}
                        Stream.Read(imgData, 0, CInt(len))
                    End Using
                    ' User1.PersonPhoto = imgData
                    'If Not bIspageLoad Then
                    If IsPostBack Then
                        UpdateImageSize(imgData)
                    End If

                    Return imgData
                Else
                    Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(New Exception(sFile & " File does not exists."))
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try

        End Function

        Private Function CheckMemberType() As Boolean
            Try



                Return True
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return False
            End Try

        End Function
        Protected Overridable Sub DeleteDownloadedPhotos()

            Dim fileEntries As String() = Directory.GetFiles(Server.MapPath(PersonImageURL), m_lEntityID & "_" & User1.PersonID & "*.jpg", SearchOption.TopDirectoryOnly)
            For Each fileName As String In fileEntries
                File.Delete(fileName.ToString)
            Next

            fileEntries = Directory.GetFiles(Server.MapPath(PersonImageURL), m_lEntityID & "_*" & Me.Session.SessionID + "*.jpg", SearchOption.TopDirectoryOnly)
            For Each fileName As String In fileEntries
                File.Delete(fileName.ToString)
            Next
        End Sub

        Protected Sub btnsend_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsend.Click
            Try

                Dim lProcessFlowID As Long
                'Get the Process Flow ID to be used for sending the Downloadable Order Confirmation Email
                'Amruta Issue 14623 ProcessFlow name change
                Dim sSQL As String = "SELECT ID FROM " & Database & "..vwProcessFlows WHERE Name='Send Email Notification for Profile Edits'"
                Dim oProcessFlowID As Object = DataAction.ExecuteScalar(sSQL, IAptifyDataAction.DSLCacheSetting.UseCache)
                If oProcessFlowID IsNot Nothing AndAlso IsNumeric(oProcessFlowID) Then
                    lProcessFlowID = CLng(oProcessFlowID)
                    Dim context As New AptifyContext
                    context.Properties.AddProperty("PersonID", CLng(Session("PersonID")))
                    Dim oResult As ProcessFlowResult
                    oResult = ProcessFlowEngine.ExecuteProcessFlow(Me.AptifyApplication, lProcessFlowID, context)
                    If Not oResult.IsSuccess Then
                        ExceptionManagement.ExceptionManager.Publish(New Exception("Process flow to send Web Credentials through Email. Please refer event handler for more details."))
                        lblmsg.Text = ""
                        lblmsg.Visible = False
                    Else
                        lblmsg.Text = "Notification sent successfully"
                        lblmsg.Visible = True
                    End If

                Else
                    ExceptionManagement.ExceptionManager.Publish(New Exception("Message Template to send Web Credentials Email is not found in the system."))
                End If

            Catch ex As ArgumentException
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try


        End Sub

        Protected Sub lbtnOpenProfileImage_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnOpenProfileImage.Click
            'LoadForm()
            radwindowProfileImage.VisibleOnPageLoad = True
            'LoadProfilePicture(Nothing)
            'Amruta IssueID 14623
            lblmsg.Visible = False
        End Sub

        Protected Sub btnCancelProfileImage_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelProfileImage.Click
            radwindowProfileImage.VisibleOnPageLoad = False
            LoadProfilePicture(Nothing)
            UpdateImageSize(ImageData)
        End Sub

        Protected Sub btnSaveProfileImage_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveProfileImage.Click
            Dim sExtension As String = System.IO.Path.GetExtension(PersonImageURL & ProfilePhotoFileName).ToString()
            radImageEditor.SaveEditableImage(ProfilePhotoFileName.Substring(0, ProfilePhotoFileName.Length - sExtension.Length), True)
            SetUserPersonPhoto(ProfilePhotoMapPath & ProfilePhotoFileName)
            radwindowProfileImage.VisibleOnPageLoad = False
        End Sub

        Protected Overridable Function SetUserPersonPhoto(ByVal ImageFile As String) As Boolean
            Try

                Dim fInfo As New FileInfo(ImageFile)
                Dim oGE As Aptify.Applications.Persons.PersonsEntity = DirectCast(Me.AptifyApplication.GetEntityObject("Persons", CType(Session.Item("PersonID"), Long)), Aptify.Applications.Persons.PersonsEntity)
                Dim imageSet As Boolean = False

                If fInfo.Exists AndAlso ProfilePhotoFileName <> BlankImage Then
                    If ImageFile IsNot Nothing AndAlso Not String.IsNullOrEmpty(ImageFile) Then
                        oGE.SetValue("Photo", ConvertImagetoByte(ImageFile, False))
                        imageSet = True
                        'Return True
                    End If
                Else
                    oGE.SetValue("Photo", Nothing)
                End If
                'Amruta Issue 14307,4/9/13, To prevent processflow(company Administrator) to execute
                oGE.SetAddValue("_xNewGroupAdminObjFlag", True)

                Dim sErrorString As String = ""
                oGE.Save(sErrorString)
                'radwindowProfileImage.VisibleOnPageLoad = False


                Return imageSet


            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Function

        Private Function CompareTwoImageBytes(ByVal newImage As Byte()) As Boolean
            Try
                Dim blankimg As Byte()
                Dim strblankimg As String
                strblankimg = System.Web.HttpContext.Current.Server.MapPath(PersonImageURL) & BlankImage
                blankimg = ConvertImagetoByte(strblankimg, False)
                Dim bCompare As Boolean = True

                For i As Integer = 0 To newImage.Length - 1
                    If newImage(i) <> blankimg(i) Then
                        Return False
                    End If
                Next
                Return bCompare
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Function

        Protected Overridable Sub UploadBlankPhoto()
            'm_bRemovePhoto = True
            'HiddenField1.Value = "1"
            btnRemovePhoto.Visible = False
            radImageEditor.Enabled = False
            ShowCropButton(False)
            'LableImageSaveIndicator.Visible = False
            'If User1.UserID > 0 Then
            '    LableImageSaveIndicator.Visible = True
            'End If

            imgProfile.Width = 142
            imgProfile.Height = 142
            DeleteDownloadedPhotos()
            imgProfile.ImageUrl = ProfilePhotoBlankImage & "?" & New Random().Next().ToString()
            radImageEditor.ImageUrl = ProfilePhotoBlankImage
            ViewState(ATTRIBUTE_PROFILE_PHOTO_FILENAME) = BlankImage
            'UseSocialMediaPhoto(False)

        End Sub

        Protected Sub btnRemovePhoto_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemovePhoto.Click
            UploadBlankPhoto()
            radwindowProfileImage.VisibleOnPageLoad = False
            SetUserPersonPhoto(radImageEditor.ImageUrl)
        End Sub

        Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click
            UploadProfilePhoto()
        End Sub

        Protected Overridable Sub UploadProfilePhoto()
            Dim ImageFilewithPath As String = ""

            Try
            If (radUploadProfilePhoto.UploadedFiles IsNot Nothing AndAlso radUploadProfilePhoto.UploadedFiles.Count <> 0) Then
                'If SocialNetworkObject IsNot Nothing Then
                '    SocialNetworkObject.UserProfile.UseSocialMediaPhoto = False
                'End If
                DeleteDownloadedPhotos()
                GetNewProfilePhotoFileName()
                Dim ImageFile As String = ProfilePhotoFileName
                    ImageFilewithPath = ProfilePhotoMapPath & ImageFile
                Try
                    radUploadProfilePhoto.UploadedFiles(0).SaveAs(ProfilePhotoMapPath & ImageFile)
                Catch ex As Exception
                End Try
                    'If SetUserPersonPhoto(ImageFilewithPath) Then
                imgProfile.ImageUrl = PersonImageURL & ImageFile & "?" & New Random().Next().ToString()
                radImageEditor.ImageUrl = PersonImageURL & ImageFile
                btnRemovePhoto.Visible = True
                radImageEditor.Enabled = True
                'ShowCropButton(True)
                'UseSocialMediaPhoto(False)
                'LableImageSaveIndicator.Visible = True
                'Else

                'End If
            Else
                'If imgProfile.ImageUrl <> ProfilePhotoBlankImage AndAlso imgProfile.ImageUrl <> String.Empty Then
                '    ViewState("RefreshImageURL") = imgProfile.ImageUrl
                'End If
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(New Exception(ImageFilewithPath & " File does not exists."))
            End Try

        End Sub

        'Amruta Issue 14307 function to get topic code parent
        Public Overridable Function LoadTopicCodesParent(ByVal sEntityName As String, ByVal sRecordID As String) As String
            'Aparna issue 15141,14496 
            Dim dt As New DataTable()
            Dim strbuildtopic As New StringBuilder()
            Dim strbuildtopics As New StringBuilder()
            Dim sSQL As String = "SELECT TC.ID, TC.Name, TCL.Status FROM " + AptifyApplication.GetEntityBaseDatabase("Topic Codes") + ".." + AptifyApplication.GetEntityBaseView("Topic Codes") + " TC INNER JOIN " + AptifyApplication.GetEntityBaseDatabase("TopicCodeEntities") + ".." + AptifyApplication.GetEntityBaseView("TopicCodeEntities") + " TCE ON TC.ID = TCE.TopicCodeID INNER JOIN " + AptifyApplication.GetEntityBaseDatabase("Topic Code Links") + ".." + AptifyApplication.GetEntityBaseView("Topic Code Links") + " TCL ON TCL.EntityID = TCE.EntityID AND TCL.TopicCodeID = TC.ID AND TCL.RecordID = " & sRecordID & " WHERE ISNULL(TC.ParentID,-1)=-1 AND TC.Status='Active' AND TCL.Status='Active' AND TCE.EntityID_Name='" & sEntityName & "' AND TCE.Status='Active' AND TCE.WebEnabled=1 "
            dt = DataAction.GetDataTable(sSQL)
            For Each dtRow As DataRow In dt.Rows
                strbuildtopic.Append(dtRow("Name").ToString() + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;")
            Next
            If strbuildtopic.ToString().Length > 0 Then
                strbuildtopics.Append(strbuildtopic.ToString().Remove(strbuildtopic.ToString().Length - 1, 1))
            End If
            lblTopicIntrest.Text = strbuildtopics.ToString()

            Return String.Empty
        End Function
        'Anil B for issue 15387 on 05-04-2013
        'Check person belongs to company
        Private Function IsCompanyPerson(ByVal PersonID As Long) As Boolean
            Try
                Dim sSql As String = ""
                Dim oResult As Object
                sSql = "Select CompanyID From " & AptifyApplication.GetEntityBaseDatabase("Persons") & ".." & AptifyApplication.GetEntityBaseView("Persons") & " WHERE  ID = " & PersonID
                oResult = DataAction.ExecuteScalar(sSql)
                If oResult IsNot Nothing AndAlso IsNumeric(oResult) AndAlso oResult <> -1 AndAlso User1.CompanyID = oResult Then
                    Return True
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return False
            End Try
            Return False
        End Function
    End Class
End Namespace
