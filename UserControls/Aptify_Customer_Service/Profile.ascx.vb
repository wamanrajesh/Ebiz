'Aptify e-Business 5.5.1 SR1, June 2014
Option Explicit On
Option Strict On

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



Namespace Aptify.Framework.Web.eBusiness.CustomerService
    Partial Class ProfileControl
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced


        Protected Const ATTRIBUTE_PROCESSING_IMAGE_URL As String = "ProcessingImageURL"
        Protected Const ATTRIBUTE_PERSON_IMAGE_URL As String = "PersonImageURL"
        'Anil Changess for issue 12718
        Protected Const ATTRIBUTE_MEMBERSHIP_IMAGE_URL As String = "MembershipImageURL"
        'end
        Protected Const ATTRIBUTE_PWD_LENGTH As String = "minPwdLength"
        Protected Const ATTRIBUTE_PWD_UPPERCASE As String = "minPwdUpperCase"
        Protected Const ATTRIBUTE_PWD_LOWERCASE As String = "minPwdLowerCase"
        Protected Const ATTRIBUTE_PWD_NUMBERS As String = "minPwdNumbers"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "Profile"
        Protected Const ATTRIBUTE_PERSON_BLANK_IMG As String = "BlankImage"
        'Anil Add for issue 12835
        Protected ATTRIBUTE_IMAGEUPLOAD_PROFILE_TNCTEXT As String = "ImageUploadUserProfileText"
        Protected ATTRIBUTE_IMAGEUPLOAD_PROFILESAVE_TNCTEXT As String = "ImageUploadUserProfileSaveText"
        Protected Const ATTRIBUTE_PERSON_IMG_WIDTH As String = "ImageWidth"
        Protected Const ATTRIBUTE_PERSON_IMG_HEIGHT As String = "ImageHeight"
        Protected Const ATTRIBUTE_SYSTEM_NAME As String = "SocialNetworkSystemName"
        Protected Const ATTRIBUTE_PROFILE_PHOTO_FILENAME As String = "ProfilePhotoFileName"
        ''ISSUEID 3240 - Added SuvarnaD  
        Protected Const ATTRIBUTE_PROFILE_ALERT_DUPLICATEPERSONVALIDATION As String = "DuplicatePersonValidation"
        'end 
        Protected m_bRemovePhoto As Boolean
        Protected m_sblankImage As String
        Dim m_lEntityID As Long
        Dim m_lRecordID As String
        'Anil Bisen issue 12835 
        Protected m_iImageWidth As Integer = -1
        Protected m_iImageHeight As Integer = -1
        Protected m_sAlert As String = String.Empty
        Dim m_sEntityName As String = "Persons"
        Dim ImageData() As Byte
        'Sachin K Issue 18138
        Protected Const ATTRIBUTE_ADDRESS_VERIFY_BTN As String = "VerifyAddress"
        Protected b_AddressValue As String


#Region "Properties and Methods"
        Private Const m_c_sPrefix As String = "__aptify_shoppingCart_"
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

        Public Property ProcessingImage() As String
            Get
                If ViewState.Item("ProcessingImage") IsNot Nothing Then
                    Return ViewState.Item("ProcessingImage").ToString()
                Else
                    Return ""
                End If

            End Get
            Set(ByVal value As String)
                ViewState.Item("ProcessingImage") = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        'Anil Changess for issue 12718
        Public Property MembershipImageURL() As String
            Get
                If ViewState.Item("MembershipImageURL") IsNot Nothing Then
                    Return ViewState.Item("MembershipImageURL").ToString()
                Else
                    Return ""
                End If

            End Get
            Set(ByVal value As String)
                ViewState.Item("MembershipImageURL") = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        'end


        ''' <summary>
        ''' Minimum password length as provided in config file, default value is 6 when config file is not set
        ''' </summary>
        Public Overridable ReadOnly Property MinPwdLength() As Integer
            Get
                If ViewState.Item(ATTRIBUTE_PWD_LENGTH) IsNot Nothing Then
                    Return CInt(ViewState.Item(ATTRIBUTE_PWD_LENGTH))
                Else
                    Dim value As Integer = Me.GetGlobalAttributeIntegerValue(ATTRIBUTE_PWD_LENGTH)
                    If value <> -1 Then
                        ViewState.Item(ATTRIBUTE_PWD_LENGTH) = value
                        Return CInt(value)
                    Else
                        Return 6 'default value when nothing is set in config file
                    End If
                End If
            End Get
        End Property
        ''' <summary>
        ''' Minimum number of UpperCase letters required in password as provided in config file, default value is 1 when config file is not set
        ''' </summary>
        Public Overridable ReadOnly Property MinPwdUpperCase() As Integer
            Get
                If ViewState.Item(ATTRIBUTE_PWD_UPPERCASE) IsNot Nothing Then
                    Return CInt(ViewState.Item(ATTRIBUTE_PWD_UPPERCASE))
                Else
                    Dim value As Integer = Me.GetGlobalAttributeIntegerValue(ATTRIBUTE_PWD_UPPERCASE)
                    If value <> -1 Then
                        ViewState.Item(ATTRIBUTE_PWD_UPPERCASE) = value
                        Return CInt(value)
                    Else
                        Return 1 'default value when nothing is set in config file
                    End If
                End If
            End Get
        End Property
        ''' <summary>
        ''' Minimum number of LowerCase letters required in password as provided in config file, default value is 1 when config file is not set
        ''' </summary>
        Public Overridable ReadOnly Property MinPwdLowerCase() As Integer
            Get
                If ViewState.Item(ATTRIBUTE_PWD_LOWERCASE) IsNot Nothing Then
                    Return CInt(ViewState.Item(ATTRIBUTE_PWD_LOWERCASE))
                Else
                    Dim value As Integer = Me.GetGlobalAttributeIntegerValue(ATTRIBUTE_PWD_LOWERCASE)
                    If value <> -1 Then
                        ViewState.Item(ATTRIBUTE_PWD_LOWERCASE) = value
                        Return CInt(value)
                    Else
                        Return 1 'default value when nothing is set in config file
                    End If
                End If
            End Get
        End Property
        ''' <summary>
        ''' Minimum number of numeric letters required in password as provided in config file, default value is 1 when config file is not set
        ''' </summary>
        Public Overridable ReadOnly Property MinPwdNumbers() As Integer
            Get
                If ViewState.Item(ATTRIBUTE_PWD_NUMBERS) IsNot Nothing Then
                    Return CInt(ViewState.Item(ATTRIBUTE_PWD_NUMBERS))
                Else
                    Dim value As Integer = Me.GetGlobalAttributeIntegerValue(ATTRIBUTE_PWD_NUMBERS)
                    If value <> -1 Then
                        ViewState.Item(ATTRIBUTE_PWD_NUMBERS) = value
                        Return CInt(value)
                    Else
                        Return 1 'default value when nothing is set in config file
                    End If
                End If
            End Get
        End Property
        ''' <summary>
        ''' Rashmi P
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
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
        'Anil Bisen issue 12835 
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

        'HP Issue#9078: function added for password complexity
        Private Function IsPasswordComplex(ByVal password As String) As Boolean
            Dim result As Boolean = False
            'Aparna for issue 12964 for showing password length validation
            lblpasswordlengthError.Text = ""
            If password.Length >= MinPwdLength Then
                result = System.Text.RegularExpressions.Regex.IsMatch(password, "(?=(.*[A-Z].*){" & MinPwdUpperCase & ",})(?=(.*[a-z].*){" & MinPwdLowerCase & ",})(?=(.*\d.*){" & MinPwdNumbers & ",})")
            End If
            'Aparna for issue 12964 for showing password length validation
            If Not result Then
                lblpasswordlengthError.Text = "Password must be a minimum length of " & MinPwdLength.ToString & " with at least " & _
                                MinPwdLowerCase & " lower-case letter(s) and " & MinPwdUpperCase & " upper-case letter(s) and " & _
                                MinPwdNumbers & " number(s)."
            End If
            Return result
        End Function
        Private Function IsPasswordComplexPopup(ByVal password As String) As Boolean
            Dim result As Boolean = False
            'Aparna for issue 12964 for showing password length validation
            lblerrorLength.Text = ""
            If password.Length >= MinPwdLength Then
                result = System.Text.RegularExpressions.Regex.IsMatch(password, "(?=(.*[A-Z].*){" & MinPwdUpperCase & ",})(?=(.*[a-z].*){" & MinPwdLowerCase & ",})(?=(.*\d.*){" & MinPwdNumbers & ",})")
            End If
            'Aparna for issue 12964 for showing password length validation
            If Not result Then
                'Neha, Issue 12591,03/05/12,Done Changes for showing ErrorMeassage
                lblerrorLength.Text = "<span style='font-weight: bold; color:red; font-size:11px;'>The password criteria has not been met. Please try again.</span> " & "<br/>" &
                "<span style='font-style:italic; font-size: 7pt; font-weight: bold;'>Password must be a minimum length of " & MinPwdLength.ToString & " with at least " & _
                 MinPwdLowerCase & " lower-case letter(s) and " & MinPwdUpperCase & " upper-case letter(s) and " & MinPwdNumbers & " number(s).</span>"
            End If
            Return result
        End Function

        ''' <summary>
        ''' funtion Compare the country name from Social Network to Local DB country name
        ''' then set combo value
        ''' </summary>
        Private Sub SetCountry(ByVal SocialNetworkCountryCode As String)
            Dim dt As DataTable
            Dim dr As Data.DataRow
            Dim sSql As String

            sSql = "SELECT COUNTRY FROM " & AptifyApplication.GetEntityBaseDatabase("Countries") & ".." _
                      & AptifyApplication.GetEntityBaseView("Countries") & " WHERE ISOCODE = '" & SocialNetworkCountryCode & "'"
            dt = DataAction.GetDataTable(sSql)
            If dt IsNot Nothing Then
                dr = dt.Rows.Item(0)
                SetComboValue(cmbCountry, CStr(dr("COUNTRY")))
            End If
        End Sub

        'Anil Add for Issuie 12835 
        Protected Overridable ReadOnly Property ImageUploadUserProfileSaveText() As String
            Get
                If Not Session.Item(ATTRIBUTE_IMAGEUPLOAD_PROFILESAVE_TNCTEXT) Is Nothing Then
                    Return CStr(Session.Item(ATTRIBUTE_IMAGEUPLOAD_PROFILESAVE_TNCTEXT))
                Else
                    Dim value As String = Me.GetGlobalAttributeValue(ATTRIBUTE_IMAGEUPLOAD_PROFILESAVE_TNCTEXT)
                    If Not String.IsNullOrEmpty(value) Then
                        Session.Item(ATTRIBUTE_IMAGEUPLOAD_PROFILESAVE_TNCTEXT) = value
                        Return value
                    Else
                        Return String.Empty
                    End If
                End If
            End Get

        End Property

        Protected Overridable ReadOnly Property ImageUploadUserProfileText() As String
            Get
                If Not Session.Item(ATTRIBUTE_IMAGEUPLOAD_PROFILE_TNCTEXT) Is Nothing Then
                    Return CStr(Session.Item(ATTRIBUTE_IMAGEUPLOAD_PROFILE_TNCTEXT))
                Else
                    Dim value As String = Me.GetGlobalAttributeValue(ATTRIBUTE_IMAGEUPLOAD_PROFILE_TNCTEXT)
                    If Not String.IsNullOrEmpty(value) Then
                        Session.Item(ATTRIBUTE_IMAGEUPLOAD_PROFILE_TNCTEXT) = value
                        Return value
                    Else
                        Return String.Empty
                    End If
                End If
            End Get

        End Property

        'Anil 10258 
        Public ReadOnly Property SocialNetworkObject() As SocialNetworkIntegrationBase
            Get
                If Session("SocialNetwork") IsNot Nothing Then
                    Return DirectCast(Session("SocialNetwork"), SocialNetworkIntegrationBase)
                Else
                    If SocialNetworkSystemName <> "" AndAlso WebUserLogin1.User.UserID > 0 Then
                        Session("SocialNetwork") = SocialNetwork.SocialNetworkInstance(SocialNetworkSystemName, AptifyApplication, WebUserLogin1.User.UserID, Nothing, False)
                    End If
                End If
                Return DirectCast(Session("SocialNetwork"), SocialNetworkIntegrationBase)
            End Get
        End Property
        Public Overridable ReadOnly Property SocialNetworkSystemName() As String
            Get
                If Not Session.Item(ATTRIBUTE_SYSTEM_NAME) Is Nothing Then
                    Return CStr(Session.Item(ATTRIBUTE_SYSTEM_NAME))
                Else
                    Dim value As String = Me.GetGlobalAttributeValue(ATTRIBUTE_SYSTEM_NAME)
                    If Not String.IsNullOrEmpty(value) Then
                        Session.Item(ATTRIBUTE_SYSTEM_NAME) = value
                        Return value
                    Else
                        Return String.Empty
                    End If
                End If
            End Get

        End Property
        'end
        ''' <summary>
        ''' ''' ''ISSUEID 3240 - Added SuvarnaD  
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>

        Public Overridable Property DuplicatePersonValidation() As String
            Get
                Return m_sAlert
            End Get
            Set(ByVal value As String)
                m_sAlert = value
            End Set
        End Property
        ''' <summary>
        ''' Sachin K Issue 18138
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AddressVerifyBtn() As String
            Get
                Return b_AddressValue
            End Get
            Set(ByVal value As String)
                b_AddressValue = value
            End Set
        End Property

#End Region

#Region "Page Events"
        'Neha changes for issue 15312, 05/07/13
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            txtFirstName.Focus()
            'Anil Changess for issue 12835 
            If IsPostBack AndAlso txtPassword.Text <> String.Empty Then
                txtPassword.Attributes("value") = txtPassword.Text
                txtRepeatPWD.Attributes("value") = txtRepeatPWD.Text
            End If
            'End

            SetProperties()
            m_lEntityID = CLng(AptifyApplication.GetEntityID("Persons"))

            'Javascript to show image when address type is changed...
            Dim imgURL As String
            imgProcessing.Src = ProcessingImage
            imgURL = Replace(tblMain.Parent.UniqueID, "$", "_") & "_" & imgProcessing.ID


            ddlAddressType.Attributes.Add("OnChange", "javascript:ShowImage('" & imgURL & "')")
            chkPrefAddress.Attributes.Add("OnClick", "javascript:ShowImage('" & imgURL & "')")



            btnRemovePhoto.Visible = False
            radImageEditor.Enabled = False
            ShowCropButton(False)

            If HiddenField1.Value = "1" Then
                m_bRemovePhoto = True
            End If
            If IsPostBack AndAlso imgProfile.ImageUrl <> String.Empty AndAlso ProfilePhotoFileName <> "" Then
                Dim sRandom As String = New Random().Next().ToString()
                imgProfile.ImageUrl = PersonImageURL & ProfilePhotoFileName & "?" & sRandom

                If imgProfile.ImageUrl.Substring(0, imgProfile.ImageUrl.Length - (sRandom.Length + 1)) <> PersonImageURL & BlankImage Then
                    btnRemovePhoto.Visible = True
                    radImageEditor.Enabled = True
                End If
            End If
            ImgMembershipe.ImageUrl = MembershipImageURL
            'end

            If Not IsPostBack Then
                If imgProfile.ImageUrl = "" Then
                    imgProfile.ImageUrl = PersonImageURL & BlankImage
                End If
                SetSocialNetworkControls()
                'ViewState("ShowImageLableIndicator") = ""
                LoadForm()
                LoadTopicCodes()
                loadmemberinfo()
                lblerrorLength.Text = ""
                lblPasswordsuccess.Text = ""

                ltlImageEditorStyle.Text = "<style type=""text/css""> #" & radImageEditor.ClientID & "_ToolsPanel { display: none !important; } #" & radwindowProfileImage.ClientID & "_C { overflow: hidden !important; }</style>"

                'Sachin Issue 18138
                If CBool(AddressVerifyBtn) Then
                    btnVerify.Visible = True
                End If

            End If

        End Sub

        Protected Overrides Sub SetProperties()
            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()
            If String.IsNullOrEmpty(ProcessingImage) Then
                ProcessingImage = Me.GetLinkValueFromXML(ATTRIBUTE_PROCESSING_IMAGE_URL)
            End If
            'Anil Changess for issue 12718
            If String.IsNullOrEmpty(MembershipImageURL) Then
                MembershipImageURL = Me.GetLinkValueFromXML(ATTRIBUTE_MEMBERSHIP_IMAGE_URL)
            End If
            'end
            If String.IsNullOrEmpty(PersonImageURL) Then
                PersonImageURL = Me.GetLinkValueFromXML(ATTRIBUTE_PERSON_IMAGE_URL)
            End If
            If String.IsNullOrEmpty(BlankImage) Then
                BlankImage = Me.GetPropertyValueFromXML(ATTRIBUTE_PERSON_BLANK_IMG)
            End If

            'Anil Add for issue 12835
            If Not String.IsNullOrEmpty(ImageUploadUserProfileText) Then
                LableImageUploadText.Text = ImageUploadUserProfileText
            End If

            If Not String.IsNullOrEmpty(ImageUploadUserProfileSaveText) Then
                LableImageSaveIndicator.Text = ImageUploadUserProfileSaveText
            End If

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
            'end
            ''ISSUEID 3240 - Added SuvarnaD  
            If String.IsNullOrEmpty(DuplicatePersonValidation) Then
                DuplicatePersonValidation = Me.GetPropertyValueFromXML(ATTRIBUTE_PROFILE_ALERT_DUPLICATEPERSONVALIDATION)
                If Not String.IsNullOrEmpty(DuplicatePersonValidation) Then
                    lblAlert.Text = DuplicatePersonValidation
                End If
            End If
            'SACHIN ISSUE 18138
            If String.IsNullOrEmpty(AddressVerifyBtn) Then
                AddressVerifyBtn = Me.GetPropertyValueFromXML(ATTRIBUTE_ADDRESS_VERIFY_BTN)
            End If

        End Sub
        Protected Overridable Sub SetSocialNetworkControls()
            'Anil Changess for Issue 12835
            If SocialNetworkObject IsNot Nothing AndAlso SocialNetworkObject.IsConnected AndAlso SocialNetworkObject.UserProfile.SynchronizeProfile Then
                LoadSocialNetworkProfilePhoto()

            End If
        End Sub

#End Region

#Region "Load Data Methods"

        Private Sub LoadForm()
            Try
                PopulateDropDowns()

                If User1.UserID > 0 Then
                    '  lblPageTitle.Text = "Edit Profile"
                    'lblProfileTitle.Text = "Edit User Profile"
                    'lnkCheckAvailable.Visible = False
                    LoadUserInfo()
                    trWebAccount.Visible = False
                    trUserID.Visible = True
                    valPWDMatch.Enabled = False
                    valPWDRequired.EnableClientScript = False
                    lblPWD.Visible = False
                    lblRepeatPWD.Visible = False
                    txtPassword.Visible = False
                    txtRepeatPWD.Visible = False
                    lblPasswordHintQuestion.Visible = False
                    lblPasswordHintAnswer.Visible = False
                    cmbPasswordQuestion.Visible = False
                    txtPasswordHintAnswer.Visible = False
                    valPasswordHintRequired.EnableClientScript = False

                ElseIf SocialNetworkObject IsNot Nothing AndAlso SocialNetworkObject.IsConnected Then
                    LoadSocialNetworkProfile()
                    ''Added by Suvarna for Issue ID - 19705
                    trUserID.Visible = False
                Else
                    '  lblPageTitle.Text = "New User Signup"
                    ' example of page-level default values
                    'Commented out default values for country
                    SetComboValue(cmbCountry, "United States")
                    PopulateState(cmbState, cmbCountry) '11/27/07,Added by Tamasa,Issue 5222.
                    SetComboValue(cmbState, "DC")
                    'lblProfileTitle.Text = "New User Profile"
                    trWebAccount.Visible = True
                    trUserID.Visible = False
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub PopulateDropDowns()
            Dim sSQL As String
            Dim dt As DataTable
            Try
                ''IssueID -15385 - Condition added to remove "Company Administrator" function from drop down and also added <Select a Function> item to dropdown
                sSQL = "SELECT -1 as 'ID', '<Select a Function>' as 'Name' UNION SELECT ID,Name FROM " & _
                       AptifyApplication.GetEntityBaseDatabase("Functions") & _
                       "..vwFunctions WHERE UPPER(Name) <> Upper('Company Administrator') ORDER BY Name"
                cmbPrimaryFunction.DataSource = DataAction.GetDataTable(sSQL)
                cmbPrimaryFunction.DataTextField = "Name"
                cmbPrimaryFunction.DataValueField = "ID"
                cmbPrimaryFunction.DataBind()

                sSQL = AptifyApplication.GetEntityBaseDatabase("Addresses") & _
                       "..spGetCountryList"
                dt = DataAction.GetDataTable(sSQL)
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

        Private Sub LoadUserInfo()

            Try

                txtFirstName.Text = User1.GetValue("FirstName")
                txtLastName.Text = User1.GetValue("LastName")
                txtCompany.Text = User1.GetValue("Company")
                txtTitle.Text = User1.GetValue("Title")
                txtEmail.Text = User1.GetValue("Email")
                ''RashmiP, Load user's picture
                LoadProfilePicture(Nothing)
                'Load Address Info
                LoadAddresses()
                SetComboValue(ddlAddressType, User1.GetValue("PreferredAddress"))
                DisplayAddress(User1.GetValue("PreferredAddress"))
                chkPrefAddress.Checked = True
                chkPrefAddress.Enabled = False
                Me.PreferredAddress = User1.GetValue("PreferredAddress")

                txtPhoneAreaCode.Text = User1.GetValue("PhoneAreaCode")
                txtPhone.Text = User1.GetValue("Phone")
                txtFaxAreaCode.Text = User1.GetValue("FaxAreaCode")
                txtFaxPhone.Text = User1.GetValue("FaxPhone")

                txtUserID.Text = User1.WebUserStringID
                lblUserID.Text = User1.WebUserStringID

                txtUserID.Enabled = False
                txtPassword.Text = User1.Password
                txtRepeatPWD.Text = txtPassword.Text

                ''IssueID -15385 - Condition added to remove "Company Administrator" function from drop down and also added <Select a Function> item to dropdown
                ''Following cluse is added to display the <Select a fucntion> in dropdown when in smart client primary function of the user is "Company Administrator" 
                If String.Compare(User1.GetValue("PrimaryFunctionID"), "Company Administrator", True) = 0 Then
                    SetComboValue(cmbPrimaryFunction, "-1")
                Else
                    SetComboValue(cmbPrimaryFunction, User1.GetValue("PrimaryFunctionID"))
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        ''' <summary>
        ''' RashmiP, Date: 05/26/2011, Issue 11265:Show Photo on Profile Page and Allow Users to Upload Photos
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub LoadProfilePicture(ByVal ImageName As String)
            Dim sSQL As String = ""
            Dim dt As DataTable
            Dim sBaseURL As String
            m_lRecordID = User1.PersonID.ToString
            sSQL = "Select Photo from vwPersons Where ID = " & m_lRecordID

            dt = DataAction.GetDataTable(sSQL)
            'Sheetal#17400 dated 290615:Updated If condition to support cancel button functionality
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                'If Not dt Is Nothing Then
                If Not IsDBNull(dt.Rows(0)("Photo")) Then
                    'Nalini 11265:Added the delete functionality of last profile picture of that specific person and added Guid for the unique name of the profile image
                    DeleteDownloadedPhotos()
                    GetNewProfilePhotoFileName()
                    Dim uniqueGuid As String = System.Guid.NewGuid.ToString
                    Dim ImagePath As String = ProfilePhotoMapPath & ProfilePhotoFileName
                    Dim newImgData() As Byte
                    ImageData = DirectCast(dt.Rows(0)("Photo"), [Byte]())
                    If ImageData.Length > 0 Then
                        Dim client As New System.Net.WebClient
                        client.UploadData(ImagePath, "POST", ImageData)
                        'Anil Bisen issue 12835 
                        UpdateImageSize(ImageData)
                        imgProfile.ImageUrl = PersonImageURL & ProfilePhotoFileName & "?" & New Random().Next().ToString()
                        radImageEditor.ImageUrl = PersonImageURL & ProfilePhotoFileName

                        'ViewState("RefreshImageURL") = imgProfile.ImageUrl
                        'ViewState("ShowImageLableIndicator") = imgProfile.ImageUrl
                        newImgData = ConvertImagetoByte(ImagePath, True)
                        If CompareTwoImageBytes(newImgData) Then
                            btnRemovePhoto.Visible = False
                            radImageEditor.Enabled = False
                            ShowCropButton(False)
                        Else
                            btnRemovePhoto.Visible = True
                            radImageEditor.Enabled = True
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
        'Anil Bisen issue 12835 
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
        Protected Overridable Function UpdateImageSize(ByRef ImageByte As Byte()) As Boolean
            Try
                Dim NewImgeWidth As Integer
                Dim originalImage As System.Drawing.Image
                If ImageByte IsNot Nothing AndAlso ImageByte.Length > 0 Then
                    imgProfile.Width = ImageWidth
                    imgProfile.Height = ImageHeight
                    ' convert imageByte to virtual Image
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
        'end


        ''' <summary>
        ''' Rashmi P, Convert Image into bytes 
        ''' </summary>
        ''' <param name="spath"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
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
        ''RashmiP, Compare two byte
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

        Protected Overridable Sub LoadSocialNetworkProfilePhoto()
            If SocialNetworkObject.UserProfile IsNot Nothing Then
                With SocialNetworkObject.UserProfile
                    Dim spath1 As [String] = System.Web.HttpContext.Current.Server.MapPath(PersonImageURL)
                    If .PictureURL IsNot Nothing Then
                        'Navin Prasad Issue 10258
                        If .UseSocialMediaPhoto = True Then
                            imgProfile.ImageUrl = .PictureURL
                            'Anil Issue 10258
                            btnRemovePhoto.Visible = True

                            If Not .SynchronizeProfile AndAlso WebUserLogin1.User.UserID >= 0 Then
                                LableImageSaveIndicator.Visible = True
                            End If

                            DownloadSocialMediaPhoto(.PictureURL)
                        Else
                            imgProfile.ImageUrl = ProfilePhotoBlankImage
                            btnRemovePhoto.Visible = False
                        End If
                    Else
                        imgProfile.ImageUrl = ProfilePhotoBlankImage
                        btnRemovePhoto.Visible = False
                    End If
                End With
            End If
        End Sub


        'RashmiP Load data from LinkedIn profile.
        Private Sub LoadSocialNetworkProfile()
            Try

                m_lRecordID = User1.PersonID.ToString

                If SocialNetworkObject.IsConnected AndAlso SocialNetworkObject.UserProfile IsNot Nothing Then
                    With SocialNetworkObject.UserProfile
                        txtFirstName.Text = .FirstName
                        txtLastName.Text = .LastName
                        txtCompany.Text = .CurrentCompany
                        If Trim(Convert.ToString(.Title)) <> "" Then
                            txtTitle.Text = .Title
                        Else
                            txtTitle.Text = .Headline
                        End If
                        txtAddressLine1.Text = .Location.Street
                        txtCity.Text = .Location.City
                        txtZipCode.Text = .Location.ZipCode
                        txtUserID.Text = .EBusinessUser.WebUserStringID
                        LoadSocialNetworkProfilePhoto()
                        txtRepeatPWD.Text = txtPassword.Text
                        SetCountry(.Location.Country)
                        If cmbCountry.Text <> "" Then
                            PopulateState(cmbState, cmbCountry)
                        End If
                        SetComboValue(cmbState, .Location.State)
                    End With
                End If
            Catch ex As Exception

            End Try
        End Sub

        Private Sub RemovePersonsPhoto()
            Try
                Dim fileName As [String] = ""
                Dim sFile As [String] = ""
                If SocialNetworkObject IsNot Nothing Then
                    fileName = m_lEntityID.ToString() & "_" & SocialNetworkObject.UserProfile.ProfileID & ".jpg"
                    Dim spath1 As [String] = System.Web.HttpContext.Current.Server.MapPath(PersonImageURL)
                    sFile = spath1 + fileName
                    If File.Exists(sFile) Then
                        File.Delete(sFile)
                    End If
                End If
            Catch ex As Exception

            End Try
        End Sub

        Private Sub LoadTopicCodes()
            Try
                'Aparna issue 15141,14496 
                Dim sSQL As String = "Select TC.ID, TC.ParentID, TC.Name, TC.WebEnabled FROM " + AptifyApplication.GetEntityBaseDatabase("Topic Codes") + ".." + AptifyApplication.GetEntityBaseView("Topic Codes") + " TC inner join " + AptifyApplication.GetEntityBaseDatabase("TopicCodeEntities") + ".." + AptifyApplication.GetEntityBaseView("TopicCodeEntities") + " TCE on TC.ID = TCE.TopicCodeID Where ISNULL(TC.ParentID,-1)=-1 AND TC.Status='Active' AND TCE.EntityID_Name='" & m_sEntityName & "' AND TCE.Status='Active' AND TCE.WebEnabled=1"
                Dim dt As DataTable = DataAction.GetDataTable(sSQL)
                If dt.Rows.Count > 0 Then
                    Dim litem As System.Web.UI.WebControls.ListItem
                    For Each dtRow As DataRow In dt.Rows
                        litem = New System.Web.UI.WebControls.ListItem
                        litem.Text = dtRow("Name").ToString
                        litem.Value = dtRow("ID").ToString
                        cblTopicofInterest.Items.Add(litem)
                    Next
                End If
                Dim icount As Long = 0
                Dim oLink As AptifyGenericEntityBase
                For Each itm As System.Web.UI.WebControls.ListItem In cblTopicofInterest.Items
                    With itm
                        sSQL = GetNodeCheckSQL(CLng(.Value))
                        icount = CInt(DataAction.ExecuteScalar(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache))
                        If icount > 0 Then
                            sSQL = GetNodeLinkSQL(CLng(.Value))
                            Dim lLinkID As Long = CLng(DataAction.ExecuteScalar(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache))
                            oLink = AptifyApplication.GetEntityObject("Topic Code Links", lLinkID)
                            If oLink IsNot Nothing And oLink.GetValue("Status") IsNot Nothing Then
                                itm.Selected = CBool(IIf(CStr(oLink.GetValue("Status")) = "Active", True, False))
                            End If
                        End If
                    End With
                Next
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

#End Region

#Region "Save Methods"


        Private Function DoSave() As Boolean
            ' This function will update the user information by passing
            ' in the data to the User object and requesting it to save
            'Dim bAddressChanged As Boolean = False

            Try
                'Navin Prasad Issue 12341
                Dim bNewUser As Boolean = False
                bNewUser = User1.UserID <= 0
                User1.SetValue("FirstName", txtFirstName.Text)
                User1.SetValue("LastName", txtLastName.Text)
                User1.SetValue("Title", txtTitle.Text)
                User1.SetValue("Email", txtEmail.Text)
                User1.SetAddValue("Email1", txtEmail.Text)
                User1.SetValue("Company", txtCompany.Text)

                SetUserPersonPhoto(ProfilePhotoMapPath & ProfilePhotoFileName)
                'If String.Compare(txtAddressLine1.Text, User1.GetValue("AddressLine1"), True) <> 0 OrElse _
                '        String.Compare(txtAddressLine2.Text, User1.GetValue("AddressLine2"), True) <> 0 OrElse _
                '        String.Compare(txtAddressLine3.Text, User1.GetValue("AddressLine3"), True) <> 0 OrElse _
                '        String.Compare(txtCity.Text, User1.GetValue("City"), True) <> 0 OrElse _
                '        String.Compare(CStr(IIf(cmbState.SelectedIndex >= 0, cmbState.SelectedItem.Text, "")), User1.GetValue("State"), True) <> 0 OrElse _
                '        String.Compare(txtZipCode.Text, User1.GetValue("ZipCode"), True) <> 0 OrElse _
                '        String.Compare(CStr(IIf(cmbCountry.SelectedIndex >= 0, cmbCountry.SelectedItem.Text, "")), User1.GetValue("Country"), True) <> 0 Then
                '    bAddressChanged = True
                'End If

                ''RashmiP, Issue 5051, 8/3/2011. Set City, State, County according to Postal code.
                'Navin Prasad Issue 5051
                Dim sCounty As String = ""
                Me.DoPostalCodeLookup(txtZipCode.Text, CStr(IIf(cmbCountry.SelectedIndex >= 0, cmbCountry.SelectedValue, "")), sCounty, txtCity, cmbState)

                User1.SetValue("AddressLine1", txtAddressLine1.Text)
                User1.SetValue("AddressLine2", txtAddressLine2.Text)
                User1.SetValue("AddressLine3", txtAddressLine3.Text)
                User1.SetValue("City", txtCity.Text)
                User1.SetValue("State", CStr(IIf(cmbState.SelectedIndex >= 0, cmbState.SelectedValue, "")))
                User1.SetValue("ZipCode", txtZipCode.Text)
                User1.SetValue("CountryCodeID", CLng(IIf(cmbCountry.SelectedIndex >= 0, cmbCountry.SelectedItem.Value, ""))) '11/27/07,Added by Tamasa,Issue 5222.
                User1.SetValue("Country", CStr(IIf(cmbCountry.SelectedIndex >= 0, cmbCountry.SelectedValue, "")))
                User1.SetAddValue("County", sCounty)
                'Navin Prasad Issue 5051
                sCounty = ""
                Me.DoPostalCodeLookup(txtHomeZipCode.Text, CStr(IIf(cmbHomeCountry.SelectedIndex >= 0, cmbHomeCountry.SelectedValue, "")), sCounty, txtHomeCity, cmbHomeState)


                User1.SetValue("HomeAddressLine1", txtHomeAddressLine1.Text)
                User1.SetValue("HomeAddressLine2", txtHomeAddressLine2.Text)
                User1.SetValue("HomeAddressLine3", txtHomeAddressLine3.Text)

                User1.SetValue("HomeCity", txtHomeCity.Text)
                User1.SetValue("HomeState", CStr(IIf(cmbHomeState.SelectedIndex >= 0, cmbHomeState.SelectedValue, "")))
                User1.SetValue("HomeZipCode", txtHomeZipCode.Text)
                'User1.SetValue("HomeCountryCodeID", CLng(IIf(cmbHomeCountry.SelectedIndex >= 0, cmbHomeCountry.SelectedItem.Value, ""))) '11/27/07,Added by Tamasa,Issue 5222.
                User1.SetValue("HomeCountry", CStr(IIf(cmbHomeCountry.SelectedIndex >= 0, cmbHomeCountry.SelectedValue, "")))
                User1.SetAddValue("HomeCounty", sCounty)

                'Navin Prasad Issue 5051
                sCounty = ""
                Me.DoPostalCodeLookup(txtBillingZipCode.Text, CStr(IIf(cmbBillingCountry.SelectedIndex >= 0, cmbBillingCountry.SelectedValue, "")), sCounty, txtBillingCity, cmbBillingState)
                User1.SetValue("BillingAddressLine1", txtBillingAddressLine1.Text)
                User1.SetValue("BillingAddressLine2", txtBillingAddressLine2.Text)
                User1.SetValue("BillingAddressLine3", txtBillingAddressLine3.Text)
                User1.SetValue("BillingCity", txtBillingCity.Text)
                User1.SetValue("BillingState", CStr(IIf(cmbBillingState.SelectedIndex >= 0, cmbBillingState.SelectedValue, "")))
                User1.SetValue("BillingZipCode", txtBillingZipCode.Text)
                'User1.SetValue("BillingCountryCodeID", CLng(IIf(cmbBillingCountry.SelectedIndex >= 0, cmbBillingCountry.SelectedItem.Value, ""))) '11/27/07,Added by Tamasa,Issue 5222.
                User1.SetValue("BillingCountry", CStr(IIf(cmbBillingCountry.SelectedIndex >= 0, cmbBillingCountry.SelectedValue, "")))
                User1.SetAddValue("BillingCounty", sCounty)

                'Navin Prasad Issue 5051
                sCounty = ""
                Me.DoPostalCodeLookup(txtPOBoxZipCode.Text, CStr(IIf(cmbPOBoxCountry.SelectedIndex >= 0, cmbPOBoxCountry.SelectedValue, "")), sCounty, txtPOBoxCity, cmbPOBoxState)
                User1.SetValue("POBox", txtPOBoxAddressLine1.Text)
                User1.SetValue("POBoxLine2", txtPOBoxAddressLine2.Text)
                User1.SetValue("POBoxLine3", txtPOBoxAddressLine3.Text)
                User1.SetValue("POBoxCity", txtPOBoxCity.Text)
                User1.SetValue("POBoxState", CStr(IIf(cmbPOBoxState.SelectedIndex >= 0, cmbPOBoxState.SelectedValue, "")))
                User1.SetValue("POBoxZipCode", txtPOBoxZipCode.Text)
                'User1.SetValue("POBoxCountryCodeID", CLng(IIf(cmbPOBoxCountry.SelectedIndex >= 0, cmbPOBoxCountry.SelectedItem.Value, ""))) '11/27/07,Added by Tamasa,Issue 5222.
                User1.SetValue("POBoxCountry", CStr(IIf(cmbPOBoxCountry.SelectedIndex >= 0, cmbPOBoxCountry.SelectedValue, "")))
                User1.SetAddValue("POBoxCounty", sCounty)
                'Sheetal Issue 21545
                If Me.PreferredAddress <> "" Then
                    User1.SetValue("PreferredAddress", Me.PreferredAddress)
                    User1.SetValue("PreferredBillingAddress", Me.PreferredAddress)
                    User1.SetValue("PreferredShippingAddress", Me.PreferredAddress)
                End If
                User1.SetValue("PhoneAreaCode", txtPhoneAreaCode.Text)
                User1.SetValue("Phone", txtPhone.TextWithLiterals)
                User1.SetValue("FaxAreaCode", txtFaxAreaCode.Text)
                User1.SetValue("FaxPhone", txtFaxPhone.TextWithLiterals)

                'User1.SetValue("Photo", )
                'CPirisino: If no functions are defined then don't try to save primary function
                If cmbPrimaryFunction.SelectedItem.Value IsNot Nothing Then
                    User1.SetValue("PrimaryFunctionID", CLng(IIf(cmbPrimaryFunction.SelectedIndex >= 0, cmbPrimaryFunction.SelectedItem.Value, "-1")))
                End If

                User1.SetValue("WebUserStringID", txtUserID.Text)

                If User1.UserID <= 0 Then
                    User1.SetValue("Password", txtPassword.Text)
                    User1.SetValue("PasswordHint", cmbPasswordQuestion.SelectedItem.Text)
                    User1.SetValue("PasswordHintAnswer", txtPasswordHintAnswer.Text)
                End If
                User1.SaveValuesToSessionObject(Page.Session) ' need explicit call due to page redirect possibilities

                If User1.Save() Then
                    'RashmiP..LinkedIn Syncronize to PersonExternalAccount
                    Dim sError As String = ""
                    If SocialNetworkObject IsNot Nothing AndAlso SocialNetworkObject.IsConnected Then
                        SocialNetworkObject.UserProfile.EBusinessUser = User1

                        If SocialNetworkObject.UserProfile.SynchronizePersonExternalAccount(sError) Then
                            If bNewUser AndAlso SocialNetworkObject.UserProfile.SynchronizeProfile Then
                                'SKB Issue 12341 12/01/2011
                                Dim sSQL As String = "SELECT ID FROM " & Database & "..vwProcessFlows WHERE Name='Social Network User Profile Synchronization'"
                                Dim oProcessFlowID As Object = DataAction.ExecuteScalar(sSQL, IAptifyDataAction.DSLCacheSetting.UseCache)
                                If oProcessFlowID IsNot Nothing AndAlso IsNumeric(oProcessFlowID) Then
                                    Dim lProcessFlowID As Long = CLng(oProcessFlowID)
                                    If lProcessFlowID > 0 Then
                                        Dim oContext As New AptifyContext
                                        Dim result As ProcessFlowResult
                                        oContext.Properties.AddProperty("PersonExternalAccountID", SocialNetworkObject.UserProfile.PersonExternalAccountID)
                                        result = ProcessFlowEngine.ExecuteProcessFlow(Me.AptifyApplication, lProcessFlowID, oContext)
                                        If Not result.IsSuccess Then
                                            ExceptionManagement.ExceptionManager.Publish(New Exception("Unable to synchronize social network profile for user '" & User1.WebUserStringID & "'"))
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If

                    AddUpdateTopicCode()

                    '2/4/08 RJK - If the Shopping Cart has started an Order, reset the Address based on the information provided.
                    'If bAddressChanged Then
                    Dim sOrderXML As String
                    If Session.Item(m_c_sPrefix & "OrderXML") IsNot Nothing Then
                        sOrderXML = Session.Item(m_c_sPrefix & "OrderXML").ToString

                        If sOrderXML.Length > 0 Then
                            '20090123 MAS: update the address based on preferred address

                            Dim prefShip As AptifyShoppingCart.PersonAddressType
                            Dim prefBill As AptifyShoppingCart.PersonAddressType

                            Dim UserPrefShip As String = User1.GetValue("PreferredShippingAddress").Trim.ToLower
                            If UserPrefShip.Contains("home") Then
                                prefShip = AptifyShoppingCart.PersonAddressType.Home
                            ElseIf UserPrefShip.Contains("business") Then
                                prefShip = AptifyShoppingCart.PersonAddressType.Main
                            ElseIf UserPrefShip.Contains("billing") Then
                                prefShip = AptifyShoppingCart.PersonAddressType.Billing
                            ElseIf UserPrefShip.Contains("po") Then
                                prefShip = AptifyShoppingCart.PersonAddressType.POBox
                            Else
                                prefShip = AptifyShoppingCart.PersonAddressType.Main
                            End If

                            Dim UserPrefBill As String = User1.GetValue("PreferredBillingAddress").Trim.ToLower
                            If UserPrefBill.Contains("home") Then
                                prefBill = AptifyShoppingCart.PersonAddressType.Home
                            ElseIf UserPrefBill.Contains("business") Then
                                prefBill = AptifyShoppingCart.PersonAddressType.Main
                            ElseIf UserPrefBill.Contains("billing") Then
                                prefBill = AptifyShoppingCart.PersonAddressType.Billing
                            ElseIf UserPrefBill.Contains("po") Then
                                prefBill = AptifyShoppingCart.PersonAddressType.POBox
                            Else
                                prefBill = AptifyShoppingCart.PersonAddressType.Main
                            End If

                            Me.ShoppingCart1.UpdateOrderAddress(AptifyShoppingCart.OrderAddressType.Shipping, prefShip, 0, Me.Session, Me.Application)
                            Me.ShoppingCart1.UpdateOrderAddress(AptifyShoppingCart.OrderAddressType.Billing, prefBill, 0, Me.Session, Me.Application)
                        End If
                    End If
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return False
            End Try
        End Function

#End Region

#Region "Address Methods"

        '11/27/07,Added by Tamasa,Issue 5222.
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
        '11/27/07,Added by Tamasa,Issue 5222.
        Protected Sub cmbCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCountry.SelectedIndexChanged
            PopulateState(cmbState, cmbCountry)
            txtZipCode.Focus()
        End Sub

        '9/22/08, Added by CPirisino for multi address support
        Protected Sub cmbHomeCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbHomeCountry.SelectedIndexChanged
            PopulateState(cmbHomeState, cmbHomeCountry)
        End Sub
        Protected Sub cmbBillingCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbBillingCountry.SelectedIndexChanged
            PopulateState(cmbBillingState, cmbBillingCountry)
        End Sub
        Protected Sub cmbPOBoxCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbPOBoxCountry.SelectedIndexChanged
            PopulateState(cmbPOBoxState, cmbPOBoxCountry)
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
                        If User1.PersonID < 1 Then
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
                        If User1.PersonID < 1 Then
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
                        If User1.PersonID < 1 Then
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
                        If User1.PersonID < 1 Then
                            cmbPOBoxCountry.ClearSelection()
                            SetComboValue(cmbPOBoxCountry, "United States")
                            PopulateState(cmbPOBoxState, cmbPOBoxCountry)
                        End If

                End Select

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub LoadAddresses()
            Try
                If User1.GetValue("AddressID") IsNot Nothing Or User1.GetValue("AddressLine1") IsNot Nothing Then
                    txtAddressLine1.Text = User1.GetValue("AddressLine1")
                    txtAddressLine2.Text = User1.GetValue("AddressLine2")
                    txtAddressLine3.Text = User1.GetValue("AddressLine3")
                    txtCity.Text = User1.GetValue("City")
                    txtZipCode.Text = User1.GetValue("ZipCode")
                End If

                'Put inside If statement if you don't want to default the address to US
                SetComboValue(cmbCountry, IIf(User1.GetValue("Country") = "", "United States", User1.GetValue("Country")).ToString)
                PopulateState(cmbState, cmbCountry)
                SetComboValue(cmbState, User1.GetValue("State"))


                If User1.GetValue("HomeAddressID") IsNot Nothing Or User1.GetValue("HomeAddressLine1") IsNot Nothing Then
                    txtHomeAddressLine1.Text = User1.GetValue("HomeAddressLine1")
                    txtHomeAddressLine2.Text = User1.GetValue("HomeAddressLine2")
                    txtHomeAddressLine3.Text = User1.GetValue("HomeAddressLine3")
                    txtHomeCity.Text = User1.GetValue("HomeCity")
                    txtHomeZipCode.Text = User1.GetValue("HomeZipCode")
                End If

                'Populate Home country or default to United States
                SetComboValue(cmbHomeCountry, IIf(User1.GetValue("HomeCountry") = "", "United States", User1.GetValue("HomeCountry")).ToString)
                PopulateState(cmbHomeState, cmbHomeCountry)
                SetComboValue(cmbHomeState, User1.GetValue("HomeState"))

                If User1.GetValue("BillingAddressID") IsNot Nothing Or User1.GetValue("BillingAddressLine1") IsNot Nothing Then
                    txtBillingAddressLine1.Text = User1.GetValue("BillingAddressLine1")
                    txtBillingAddressLine2.Text = User1.GetValue("BillingAddressLine2")
                    txtBillingAddressLine3.Text = User1.GetValue("BillingAddressLine3")
                    txtBillingCity.Text = User1.GetValue("BillingCity")
                    txtBillingZipCode.Text = User1.GetValue("BillingZipCode")
                End If

                'Populate Billing country or default to United States
                SetComboValue(cmbBillingCountry, IIf(User1.GetValue("BillingCountry") = "", "United States", User1.GetValue("BillingCountry")).ToString)
                PopulateState(cmbBillingState, cmbBillingCountry)
                SetComboValue(cmbBillingState, User1.GetValue("BillingState"))

                If User1.GetValue("POBoxAddressID") IsNot Nothing Or User1.GetValue("POBox") IsNot Nothing Then
                    txtPOBoxAddressLine1.Text = User1.GetValue("POBox")
                    txtPOBoxAddressLine2.Text = User1.GetValue("POBoxLine2")
                    txtPOBoxAddressLine3.Text = User1.GetValue("POBoxLine3")
                    txtPOBoxCity.Text = User1.GetValue("POBoxCity")
                    txtPOBoxZipCode.Text = User1.GetValue("POBoxZipCode")
                End If

                'Populate pobox country or default to united states
                SetComboValue(cmbPOBoxCountry, IIf(User1.GetValue("POBoxCountry") = "", "United States", User1.GetValue("POBoxCountry")).ToString)
                PopulateState(cmbPOBoxState, cmbPOBoxCountry)
                SetComboValue(cmbPOBoxState, User1.GetValue("POBoxState"))

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Protected Sub ddlAddressType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlAddressType.SelectedIndexChanged

            Me.DisplayAddress(ddlAddressType.SelectedItem.Value)
            'lblCurrentAddress.Text = ddlAddressType.SelectedItem.Value
            'lblMePreferredAddress.Text = Me.PreferredAddress
            If Me.PreferredAddress = ddlAddressType.SelectedItem.Value Then
                chkPrefAddress.Checked = True
                chkPrefAddress.Enabled = False
                'Anil Changess For Issue 12835
                txtAddressLine1.Focus()
            Else
                chkPrefAddress.Checked = False
                chkPrefAddress.Enabled = True
                chkPrefAddress.Focus()
                'End
            End If

        End Sub

        Protected Sub chkPrefAddress_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkPrefAddress.CheckedChanged
            Me.PreferredAddress = ddlAddressType.SelectedItem.Value
            chkPrefAddress.Enabled = False
        End Sub


        'Navin Prasad Issue 5051
        ''' <summary>
        ''' RashmiP, Issue 5051, 8/3/2011. Set City, State, County according to Postal code.
        ''' </summary>
        ''' <param name="PostalCode"></param>
        ''' <param name="Country"></param>
        ''' 
        Protected Overridable Sub DoPostalCodeLookup(ByRef PostalCode As String, ByRef Country As String, ByRef County As String, ByRef txt As TextBox, ByRef cmb As DropDownList)
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

                    ''RashmiP, removed assigned Area code.
                    County = sCounty

                End If

            Catch ex As Exception

            End Try
        End Sub



#End Region

#Region "Button Events"

        Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
            Dim bNewUser As Boolean
            Try
                bNewUser = User1.UserID <= 0
                Page.Validate()
                'IssueId-3240 Suvarna D on 7-Feb-2013 - Validate if record coflicts in db
                If Not bNewUser Then
                    Dim sSQL As String = String.Empty
                    sSQL = "SELECT VP.Email1" & _
                    " FROM " & AptifyApplication.GetEntityBaseDatabase("Persons") & _
                    "..vwPersons VP " & _
                    " where VP.ID=" & User1.PersonID
                    Dim sPersonEmailID As String = Convert.ToString(Me.DataAction.ExecuteScalar(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache))

                    If Not String.IsNullOrEmpty(sPersonEmailID) Then
                        If String.Compare(sPersonEmailID.ToUpper(), txtEmail.Text.Trim, True) <> 0 Then
                            If Not ValidatePerson() Then
                                radDuplicateUser.VisibleOnPageLoad = True
                                Exit Sub
                            End If
                        End If
                    End If
                Else
                    If Not ValidatePerson() Then
                        radDuplicateUser.VisibleOnPageLoad = True
                        Exit Sub
                    End If
                End If

                lblError.Visible = False
                'Aparna for issue 12964 for showing password length validation
                lblpasswordlengthError.Visible = False
                If User1.UserID <= 0 Then
                    If txtPassword.Text <> txtRepeatPWD.Text Or Trim$(txtPassword.Text) = "" Then
                        lblError.Text = "Password fields must match and must not be blank."
                        lblError.Visible = True
                        lblError.ForeColor = Drawing.Color.Red
                        Exit Sub
                    Else
                        'HP Issue#9078: check if password meets complexity requirements
                        'Aparna for issue 12964 for showing password length validation
                        If Not IsPasswordComplex(txtPassword.Text) Then
                            lblpasswordlengthError.Visible = True
                            Exit Sub
                        End If
                    End If
                End If
                If DoSave() Then
                    Dim bOK As Boolean
                    Session("LoadSocialNetworkPhoto") = False
                    If bNewUser Then
                        bOK = WebUserLogin1.Login(User1.WebUserStringID, txtPassword.Text, Page.User)
                        ' Sapna DJ 12/27/2011- Issue #12545 - Investigate Ways to Reduce Size of Session Objects
                        Session("UserLoggedIn") = True
                        ''Login To Siteifnity
                        Sitefinity4xSSO1.Sitefinity40SSO(txtUserID.Text, txtPassword.Text)
                    Else
                        bOK = True
                    End If
                    If bOK Then
                        ''RashmiP, Remove persons Photo with -1 ID.
                        Me.RemovePersonsPhoto()
                        'Suraj Issue 15370,7/31/13 revert back changes application variale to session variable
                        If Len(Session("ReturnToPage")) > 0 Then
                            Dim sTemp As String
                            sTemp = CStr(Session("ReturnToPage"))
                            Session("ReturnToPage") = "" ' used only once
                            Response.Redirect(sTemp)
                        Else
                            'HP - Issue 8285, the 'virtualdir' setting is incorrect in web.config replacing with IIS internal mapping
                            'Response.Redirect(System.Configuration.ConfigurationManager.AppSettings("virtualdir"))
                            'Response.Redirect(Request.ApplicationPath, False)
                            Response.Redirect(Request.ApplicationPath)
                        End If
                    Else
                        lblError.Text = "Error logging in"
                        lblError.Visible = True
                        lblError.ForeColor = Drawing.Color.Red
                    End If
                Else
                    lblError.Text = User1.GetLastError()
                    If lblError.Text.IndexOf(lblError.Text, StringComparison.InvariantCultureIgnoreCase) >= 0 Then
                        lblError.Text &= "  Try a different User ID."
                        lblError.ForeColor = Drawing.Color.Red
                    End If
                    lblError.Visible = True
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try

        End Sub

        Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            'SKB Issue 13162 fixed the session object issue
            If User1.UserID <= 0 Then
                Session("SocialNetwork") = Nothing
            End If
            Response.Redirect(Page.Request.ApplicationPath)
        End Sub
        Protected Overridable Sub UploadBlankPhoto()
            m_bRemovePhoto = True
            HiddenField1.Value = "1"
            btnRemovePhoto.Visible = False
            LableImageSaveIndicator.Visible = False
            If User1.UserID > 0 Then
                LableImageSaveIndicator.Visible = True
            End If

            radImageEditor.Enabled = False
            ShowCropButton(False)

            imgProfile.Width = 142
            imgProfile.Height = 142
            DeleteDownloadedPhotos()
            imgProfile.ImageUrl = ProfilePhotoBlankImage & "?" & New Random().Next().ToString()
            radImageEditor.ImageUrl = ProfilePhotoBlankImage
            ViewState(ATTRIBUTE_PROFILE_PHOTO_FILENAME) = BlankImage
            UseSocialMediaPhoto(False)

        End Sub
        Protected Sub btnRemovePhoto_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemovePhoto.Click
            UploadBlankPhoto()
            radwindowProfileImage.VisibleOnPageLoad = False
            SetUserPersonPhoto(radImageEditor.ImageUrl)
        End Sub
        Protected Overridable Sub UseSocialMediaPhoto(ByVal Flag As Boolean)
            If SocialNetworkObject IsNot Nothing AndAlso SocialNetworkObject.IsConnected Then
                SocialNetworkObject.UserProfile.UseSocialMediaPhoto = Flag
                SyncProfile.UseSocialMediaPhoto = Flag
            End If
        End Sub
        Protected Sub lnkCheckAvailable_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtUserID.TextChanged
            lblError.Text = ""
            lblError.Visible = False
            If txtUserID.Text IsNot String.Empty Then
                Dim sSQL As String = "select ID from vwwebusers where UserID='" + txtUserID.Text + "'"
                Dim dt As DataTable
                dt = DataAction.GetDataTable(sSQL)
                With lblError
                    If dt IsNot Nothing And dt.Rows.Count > 0 Then
                        .Text = "Not Available."
                        .ForeColor = Drawing.Color.Red

                        'AnilChangess for Issue 12835
                        txtPassword.Enabled = False
                        txtRepeatPWD.Enabled = False
                        txtPassword.Attributes("value") = ""
                        txtRepeatPWD.Attributes("value") = ""
                        txtPassword.Text = ""
                        txtRepeatPWD.Text = ""
                        txtUserID.Focus()
                    Else
                        .Text = "Available."
                        .ForeColor = Drawing.Color.Green
                        txtPassword.Enabled = True
                        txtRepeatPWD.Enabled = True
                        txtPassword.Focus()
                        'End
                    End If
                    .Visible = True
                    .Font.Bold = True
                End With
            End If
        End Sub

#End Region


#Region "Topic Codes Method"

        Private Function GetNodeCheckSQL(ByVal lTopicCodeID As Long) As String
            Return "SELECT COUNT(*) FROM " & _
                   GetNodeBaseSQL(lTopicCodeID)
        End Function

        Private Function GetNodeLinkSQL(ByVal lTopicCodeID As Long) As String
            Return "SELECT ID FROM " & _
                   GetNodeBaseSQL(lTopicCodeID)
        End Function
        Private Function GetNodeBaseSQL(ByVal lTopicCodeID As Long) As String
            Dim sSQL As String
            sSQL = Database & ".." & _
                   "vwTopicCodeLinks WHERE TopicCodeID=" & lTopicCodeID & _
                   " AND EntityID=(SELECT ID FROM " & _
                   Database & _
                   "..vwEntities WHERE Name='Persons') " & _
                   " AND RecordID=" & User1.PersonID
            Return sSQL
        End Function

        Private Function AddTopicCodeLink(ByVal TopicCodeID As Long) As Boolean
            Dim oLink As AptifyGenericEntityBase
            Try
                oLink = AptifyApplication.GetEntityObject("Topic Code Links", -1)
                oLink.SetValue("TopicCodeID", TopicCodeID)
                oLink.SetValue("RecordID", User1.PersonID)
                oLink.SetValue("EntityID", AptifyApplication.GetEntityID("Persons"))
                oLink.SetValue("Status", "Active")
                oLink.SetValue("Value", "Yes")
                oLink.SetValue("DateAdded", Date.Today)
                Return oLink.Save(False)
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Function

        Private Function UpdateTopicCodeLink(ByVal TopicCodeID As Long, _
                                             ByVal Active As Boolean) As Boolean
            Dim sSQL As String
            Dim lLinkID As Long
            Dim oLink As AptifyGenericEntityBase

            Try
                sSQL = GetNodeLinkSQL(TopicCodeID)
                lLinkID = CLng(DataAction.ExecuteScalar(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache))
                oLink = AptifyApplication.GetEntityObject("Topic Code Links", lLinkID)
                If Active Then
                    oLink.SetValue("Status", "Active")
                    oLink.SetValue("Value", "Yes")
                Else
                    oLink.SetValue("Status", "Inactive")
                    oLink.SetValue("Value", "No")
                End If
                Return oLink.Save(False)
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Function

        Private Sub AddUpdateTopicCode()
            Dim iCount As Long
            Dim bOK As Boolean
            For Each itm As System.Web.UI.WebControls.ListItem In cblTopicofInterest.Items
                With itm
                    bOK = True
                    Dim sSQL As String = GetNodeCheckSQL(CLng(.Value))
                    iCount = CInt(DataAction.ExecuteScalar(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache))
                    If (.Selected And iCount = 0) Then
                        bOK = bOK And AddTopicCodeLink(CLng(.Value))
                    ElseIf (iCount <> 0 AndAlso (Not .Selected Or .Selected)) Then
                        bOK = bOK And UpdateTopicCodeLink(CLng(.Value), .Selected)
                    End If
                End With
            Next
        End Sub
#End Region

        'Anil Add Code for Issue 12835
        Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click
            UploadProfilePhoto()
        End Sub

        'Anil End

        Protected Overridable Sub UploadProfilePhoto()

            If (radUploadProfilePhoto.UploadedFiles IsNot Nothing AndAlso radUploadProfilePhoto.UploadedFiles.Count <> 0) Then
                If SocialNetworkObject IsNot Nothing Then
                    SocialNetworkObject.UserProfile.UseSocialMediaPhoto = False
                End If
                DeleteDownloadedPhotos()
                GetNewProfilePhotoFileName()
                'Dim uniqueGuid As String = System.Guid.NewGuid.ToString
                Dim ImageFile As String = ProfilePhotoFileName
                Dim ImageFilewithPath As String = ProfilePhotoMapPath & ImageFile
                Try
                    radUploadProfilePhoto.UploadedFiles(0).SaveAs(ProfilePhotoMapPath & ImageFile)
                Catch ex As Exception
                End Try
                If SetUserPersonPhoto(ImageFilewithPath) Then
                    imgProfile.ImageUrl = PersonImageURL & ImageFile & "?" & New Random().Next().ToString()
                    radImageEditor.ImageUrl = PersonImageURL & ImageFile
                    btnRemovePhoto.Visible = True
                    radImageEditor.Enabled = True

                    UseSocialMediaPhoto(False)
                    LableImageSaveIndicator.Visible = True
                Else
                    Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(New Exception(ImageFilewithPath & " File does not exists."))
                End If
            Else
                'If imgProfile.ImageUrl <> ProfilePhotoBlankImage AndAlso imgProfile.ImageUrl <> String.Empty Then
                '    ViewState("RefreshImageURL") = imgProfile.ImageUrl
                'End If
            End If

        End Sub
        Protected Overridable Function SetUserPersonPhoto(ByVal ImageFile As String) As Boolean
            Dim fInfo As New FileInfo(ImageFile)
            If fInfo.Exists AndAlso ProfilePhotoFileName <> BlankImage Then
                If ImageFile IsNot Nothing AndAlso Not String.IsNullOrEmpty(ImageFile) Then
                    User1.PersonPhoto = ConvertImagetoByte(ImageFile, False)
                    Return True
                End If
            Else
                User1.PersonPhoto = Nothing
            End If
        End Function
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
        Protected Overridable Function DownloadSocialMediaPhoto(ByVal PictureURL As String) As String
            DeleteDownloadedPhotos()
            GetNewProfilePhotoFileName()
            Dim wc As New System.Net.WebClient
            Dim ImagePath As String = ProfilePhotoMapPath & ProfilePhotoFileName
            wc.DownloadFile(PictureURL, ImagePath)
            'Anil Bisen issue 12835 
            ' Open a file that is to be loaded into a byte array
            Dim oFile As System.IO.FileInfo
            oFile = New System.IO.FileInfo(ImagePath)
            If oFile.Exists Then
                User1.PersonPhoto = ConvertImagetoByte(ImagePath, False)
            Else
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(New Exception(ImagePath & " File does not exists."))
            End If
        End Function
        Protected Overridable Sub DeleteDownloadedPhotos()
            'Anil Changess for Issue 12835 
            Dim fileEntries As String() = Directory.GetFiles(Server.MapPath(PersonImageURL), m_lEntityID & "_" & User1.PersonID & "*.jpg", SearchOption.TopDirectoryOnly)
            For Each fileName As String In fileEntries
                File.Delete(fileName.ToString)
            Next
            'Anil Changess for Issue 12835 
            fileEntries = Directory.GetFiles(Server.MapPath(PersonImageURL), m_lEntityID & "_*" & Me.Session.SessionID + "*.jpg", SearchOption.TopDirectoryOnly)
            For Each fileName As String In fileEntries
                File.Delete(fileName.ToString)
            Next
        End Sub
        Private Function ClientScript() As Object
            Throw New NotImplementedException
        End Function

        Private Function this() As Object
            Throw New NotImplementedException
        End Function

        Private Function IsNothing() As Object
            Throw New NotImplementedException
        End Function

        Protected Sub lnkChangePwd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkChangePwd.Click
            radwinPassword.VisibleOnPageLoad = True
            lblerrorLength.Text = ""
        End Sub
        'Neha for issue 12591 for Profile Page Errormessage
        Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
            Dim intpassword As Integer
            If Not IsPasswordComplexPopup(txtNewPassword.Text) Then
                lblerrorLength.Visible = True
                CompareValidator.Enabled = True
                Exit Sub
            End If
            intpassword = WebUserLogin1.UpdateUserPassword(User1.WebUserStringID, txtoldpassword.Text, txtNewPassword.Text, Nothing, Nothing, Page.User)
            If (intpassword = 0) Then
                CompareValidator.Enabled = True
                lblPasswordsuccess.Text = "Password Updated Successfully!"
            End If
            If (intpassword = 1) Then
                CompareValidator.Enabled = True
                lblerrorLength.Text = "<span style='color:red'>No user match or there is no access to the encryption key.</span>"
                Exit Sub
            End If
            If (intpassword = 2) Then
                CompareValidator.Enabled = False
                lblerrorLength.Text = "<span style='color:red'>The Current Password you entered is incorrect. Please try again.</span>"
                Exit Sub
            End If
            If (intpassword = 4) Then
                CompareValidator.Enabled = True
                lblerrorLength.Text = "<span style='color:red'>Current and New Password should not same.</span>"
                Exit Sub
            End If
            If (intpassword = 3) Then
                CompareValidator.Enabled = True
                lblerrorLength.Text = "<span style='color:red'>Password updation failed!</span>"
                Exit Sub
            End If
            radwinPassword.VisibleOnPageLoad = False
        End Sub

        Protected Sub btnCancelpop_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelpop.Click
            radwinPassword.VisibleOnPageLoad = False
        End Sub
        ''' <summary>
        ''' IssuID - 3240 Added by Suvarna D 
        ''' returns boolean value
        ''' Returns False  when 1 or more records already Exists in database
        ''' Returns true when entered FirstName, last name and Email matches with the record in database. This case will be 
        ''' Applicable iif there is single record exists in the database
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function ValidatePerson() As Boolean
            Dim sSpName As String = ""
            Dim sSQL As String = ""
            Dim iCnt As Int32 = 0
            If Not String.IsNullOrEmpty(AptifyApplication.GetEntityAttribute("Persons", "ValidateDuplicatePersonRecord")) Then
                sSpName = CStr(AptifyApplication.GetEntityAttribute("Persons", "ValidateDuplicatePersonRecord"))
            End If

            sSQL = "Exec " & sSpName & " '" & txtFirstName.Text.Trim.Replace("'", "''") & "', '" & txtLastName.Text.Trim.Replace("'", "''") & "', '" & txtEmail.Text.Trim.Replace("'", "''") & "'"

            iCnt = Convert.ToInt32(DataAction.ExecuteScalar(sSQL))

            If iCnt = 0 Then
                Return False
            ElseIf iCnt = 1 Then
                If Not String.IsNullOrEmpty(AptifyApplication.GetEntityAttribute("Persons", "CheckIFPersonExists")) Then
                    sSpName = CStr(AptifyApplication.GetEntityAttribute("Persons", "CheckIFPersonExists"))
                End If

                Dim dt As DataTable = Nothing
                sSQL = "Exec " & sSpName & " '" & txtFirstName.Text.Trim.Replace("'", "''") & "', '" & txtLastName.Text.Trim.Replace("'", "''") & "', '" & txtEmail.Text.Trim.Replace("'", "''''") & "', '" & txtCompany.Text.Trim.Replace("'", "''") & "'"
                dt = DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)

                If dt.Rows.Count >= 1 Then
                    Return False
                ElseIf dt.Rows.Count = 0 Then
                    Return True
                End If
            End If
        End Function

        Protected Sub btnok_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnok.Click
            radDuplicateUser.VisibleOnPageLoad = False
        End Sub

        Private Sub loadmemberinfo()
            'Dim oPersonGE As AptifyGenericEntityBase = Nothing
            'oPersonGE = AptifyApplication.GetEntityObject("Persons", User1.PersonID)

            'If oPersonGE IsNot Nothing Then

            '    oPersonGE.GetValue("MemberType")



            'End If

            Dim objaptifyapp As New AptifyApplication()
            Dim sDB = objaptifyapp.GetEntityBaseDatabase("Persons")
            Dim sSQL = "select distinct VP.ID,VP.FirstName,VP.LastName,VP.Email,VP.MemberType,Case  When Convert(Varchar(12),VP.JoinDate,107)='Jan 01, 1900' Then 'N/A' When Convert(Varchar(12),VP.JoinDate,107)is null  Then 'N/A' else Convert(Varchar(12),VP.JoinDate,107)  end JoinDate,(isnull( Convert(Varchar (12),VP.DuesPaidThru,107),'N/A'))  DuesPaidThru " & _
            " from " & sDB & _
            " ..vwPersons VP " & _
            " where VP.ID=" & User1.PersonID

            Dim dt = Me.DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
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
                    ElseIf Convert.ToDateTime(rw("DuesPaidThru").ToString()) > Date.Now() AndAlso Convert.ToDateTime(rw("DuesPaidThru").ToString()) < Date.Now().AddDays(90) Then
                        rw("Status") = "Expiring"
                    ElseIf Convert.ToDateTime(rw("DuesPaidThru").ToString()) < Date.Now() Then
                        rw("Status") = "Expired"
                    End If
                Next
            End If
            ''Membership Information
            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then

                If dt.Rows.Item(0)("MemberType").ToString() = "" Then
                    lblmembershipType.Visible = False
                Else
                    lblmembershipType.Visible = True
                    lblMemberTypeVal.Text = ""
                    lblMemberTypeVal.Text = dt.Rows(0)("MemberType").ToString()

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
            Else
                trmemberinfo.Visible = False
            End If
        End Sub

        Protected Sub lbtnOpenProfileImage_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnOpenProfileImage.Click
            'LoadForm()
            radwindowProfileImage.VisibleOnPageLoad = True
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
            imgProfile.ImageUrl = PersonImageURL & ProfilePhotoFileName & "?" & New Random().Next().ToString()
            radwindowProfileImage.VisibleOnPageLoad = False
        End Sub

        Protected Sub ShowCropButton(ByVal isVisible As Boolean)
            If isVisible = True Then
                btnCropImage.Style.Add("display", "inline")
            Else
                btnCropImage.Style.Add("display", "none")
            End If
        End Sub


#Region "Issue 18138 Verify Address from melissa database"
        Private Function GetContryISOCode(ByVal CountryID As Long) As String
            Dim sSQL As String = String.Empty
            sSQL = "Select ISOCode from vwCountries where ID=" & CountryID
            Return CStr(DataAction.ExecuteScalar(sSQL))
        End Function
        Protected Sub btnVerify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVerify.Click
            Dim oAddressInfo As New Aptify.Framework.BusinessLogic.Address.AddressInfo(Me.AptifyApplication)
            Dim lst As Dictionary(Of String, String) = New Dictionary(Of String, String)
            If ddlAddressType.Items.Count = 0 Then
                Exit Sub
            End If
            btnVerificationCancel.Text = "Cancel"
            btnVerificationClosed.Visible = True
            If ddlAddressType.SelectedValue = "Business Address" Then
                lst.Add("Line1", txtAddressLine1.Text)
                lst.Add("Line2", txtAddressLine2.Text)
                lst.Add("Line3", txtAddressLine3.Text)

                lst.Add("City", txtCity.Text)
                lst.Add("Country", cmbCountry.SelectedItem.Text)

                If Not cmbCountry.SelectedValue = "" Then
                    lst.Add("CountryCode", GetContryISOCode(CLng(cmbCountry.SelectedValue)))
                Else
                    lst.Add("CountryCode", "")
                End If

                'cmbCountry.SelectedValue)
                lst.Add("PostalCode", txtZipCode.Text)
                txtAddressLine1.Focus()
            ElseIf ddlAddressType.SelectedValue = "Home Address" Then
                lst.Add("Line1", txtHomeAddressLine1.Text)
                lst.Add("Line2", txtHomeAddressLine2.Text)
                lst.Add("Line3", txtHomeAddressLine3.Text)

                lst.Add("City", txtHomeCity.Text)
                lst.Add("Country", cmbHomeCountry.SelectedItem.Text)

                If Not cmbHomeCountry.SelectedValue = "" Then
                    lst.Add("CountryCode", GetContryISOCode(CLng(cmbHomeCountry.SelectedValue)))
                Else
                    lst.Add("CountryCode", "")
                End If

                'cmbCountry.SelectedValue)
                lst.Add("PostalCode", txtHomeZipCode.Text)
                txtHomeAddressLine1.Focus()
            ElseIf ddlAddressType.SelectedValue = "Billing Address" Then
                lst.Add("Line1", txtBillingAddressLine1.Text)
                lst.Add("Line2", txtBillingAddressLine2.Text)
                lst.Add("Line3", txtBillingAddressLine3.Text)

                lst.Add("City", txtBillingCity.Text)
                lst.Add("Country", cmbBillingCountry.SelectedItem.Text)

                If Not cmbBillingCountry.SelectedValue = "" Then
                    lst.Add("CountryCode", GetContryISOCode(CLng(cmbBillingCountry.SelectedValue)))
                Else
                    lst.Add("CountryCode", "")
                End If

                'cmbCountry.SelectedValue)
                lst.Add("PostalCode", txtBillingZipCode.Text)
                txtBillingAddressLine1.Focus()
            ElseIf ddlAddressType.SelectedValue = "PO Box Address" Then
                lst.Add("Line1", txtPOBoxAddressLine1.Text)
                lst.Add("Line2", txtPOBoxAddressLine2.Text)
                lst.Add("Line3", txtPOBoxAddressLine3.Text)

                lst.Add("City", txtPOBoxCity.Text)
                lst.Add("Country", cmbPOBoxCountry.SelectedItem.Text)

                If Not cmbPOBoxCountry.SelectedValue = "" Then
                    lst.Add("CountryCode", GetContryISOCode(CLng(cmbPOBoxCountry.SelectedValue)))
                Else
                    lst.Add("CountryCode", "")
                End If

                'cmbCountry.SelectedValue)
                lst.Add("PostalCode", txtPOBoxZipCode.Text)
                txtPOBoxAddressLine1.Focus()
            End If

            Dim ms As Aptify.Framework.BusinessLogic.Address.AptifyMelissaDataWebservice = New Aptify.Framework.BusinessLogic.Address.AptifyMelissaDataWebservice
            Dim objAddressVerifyResult As Aptify.Framework.BusinessLogic.Address.AddressVerifyResult = New Aptify.Framework.BusinessLogic.Address.AddressVerifyResult


            Dim iAddressID As Int32 = 1
            ms.ProcessEbusinessAddressVerification(iAddressID, Me.AptifyApplication, lst)

            If (ms.List IsNot Nothing AndAlso ms.List.Count > 0) AndAlso CBool(BusinessLogic.Address.AddressVerifyResult.ChangeSuggested) Then
                radAddressMessage.VisibleOnPageLoad = True
                'Dim radAddressMessageHeight As Integer = 100
                'For index = 1 To ms.List.Count - 1
                '    radAddressMessageHeight += 15
                'Next
                'radAddressMessage.Height = radAddressMessageHeight
                'radAddressMessage.Width = 420

                Dim sTable As String = String.Empty
                sTable = CreatedSuggestedAddressTable(ms.List)

                If Not sTable = String.Empty Then
                    dvAddressMsg.Visible = False
                    dvAddressSuggested.Visible = True
                    dvAddressSuggested.InnerHtml = sTable
                Else
                    'radAddressMessage.Height = 100
                    radAddressMessage.VisibleOnPageLoad = True
                    dvAddressMsg.Visible = True
                    dvAddressSuggested.Visible = False
                    lblAddressVerify.Visible = True
                    lblAddressVerify.Text = "Address Verification Successed."
                    lblAddressVerify.ForeColor = Color.Green
                    btnVerificationCancel.Text = "OK"
                    btnVerificationClosed.Visible = False
                End If

            ElseIf (ms.List IsNot Nothing AndAlso ms.List.Count > 0) AndAlso (CBool(BusinessLogic.Address.AddressVerifyResult.Success) Or CBool(BusinessLogic.Address.AddressVerifyResult.ChangeSuggested)) Then
                radAddressMessage.VisibleOnPageLoad = True
                dvAddressMsg.Visible = True
                dvAddressSuggested.Visible = False
                lblAddressVerify.Visible = True
                lblAddressVerify.Text = "Address Verification Successed."
                lblAddressVerify.ForeColor = Color.Green
                btnVerificationCancel.Text = "OK"
                btnVerificationClosed.Visible = False
            Else
                radAddressMessage.VisibleOnPageLoad = True
                dvAddressMsg.Visible = True
                dvAddressSuggested.Visible = False
                lblAddressVerify.Visible = True
                lblAddressVerify.Text = "Address Verification Failed."
                lblAddressVerify.ForeColor = Color.Red
                btnVerificationCancel.Text = "OK"
                btnVerificationClosed.Visible = False
            End If
        End Sub

        Protected Sub btnVerificationClosed_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVerificationClosed.Click
            If dvAddressSuggested.Visible Then
                If Not ViewState("SuggestedList") Is Nothing Then
                    Dim lst As Dictionary(Of String, String) = CType(ViewState("SuggestedList"), Dictionary(Of String, String))
                    If ddlAddressType.Items.Count = 0 Then
                        Exit Sub
                    End If
                    If ddlAddressType.SelectedValue = "Business Address" Then
                        If lst.ContainsKey("Line1") Then
                            txtAddressLine1.Text = CStr(lst("Line1"))
                        End If
                        If lst.ContainsKey("Line2") Then
                            txtAddressLine2.Text = CStr(lst("Line2"))
                        End If
                        If lst.ContainsKey("Line3") Then
                            txtAddressLine3.Text = CStr(lst("Line3"))
                        End If
                        If lst.ContainsKey("City") Then
                            txtCity.Text = CStr(lst("City"))
                        End If
                        If lst.ContainsKey("Country") Then
                            SetComboValue(cmbCountry, CStr(lst("Country")))
                        End If
                        If lst.ContainsKey("PostalCode") Then
                            txtZipCode.Text = CStr(lst("PostalCode"))
                        End If
                        txtAddressLine1.Focus()
                    ElseIf ddlAddressType.SelectedValue = "Home Address" Then
                        If lst.ContainsKey("Line1") Then
                            txtHomeAddressLine1.Text = CStr(lst("Line1"))
                        End If
                        If lst.ContainsKey("Line2") Then
                            txtHomeAddressLine2.Text = CStr(lst("Line2"))
                        End If
                        If lst.ContainsKey("Line3") Then
                            txtHomeAddressLine3.Text = CStr(lst("Line3"))
                        End If
                        If lst.ContainsKey("City") Then
                            txtHomeCity.Text = CStr(lst("City"))
                        End If
                        If lst.ContainsKey("Country") Then
                            SetComboValue(cmbHomeCountry, CStr(lst("Country")))
                        End If
                        If lst.ContainsKey("PostalCode") Then
                            txtHomeZipCode.Text = CStr(lst("PostalCode"))
                        End If
                        txtHomeAddressLine1.Focus()
                    ElseIf ddlAddressType.SelectedValue = "Billing Address" Then
                        If lst.ContainsKey("Line1") Then
                            txtBillingAddressLine1.Text = CStr(lst("Line1"))
                        End If
                        If lst.ContainsKey("Line2") Then
                            txtBillingAddressLine2.Text = CStr(lst("Line2"))
                        End If
                        If lst.ContainsKey("Line3") Then
                            txtBillingAddressLine3.Text = CStr(lst("Line3"))
                        End If
                        If lst.ContainsKey("City") Then
                            txtBillingCity.Text = CStr(lst("City"))
                        End If
                        If lst.ContainsKey("Country") Then
                            SetComboValue(cmbBillingCountry, CStr(lst("Country")))
                        End If
                        If lst.ContainsKey("PostalCode") Then
                            txtBillingZipCode.Text = CStr(lst("PostalCode"))
                        End If
                        txtBillingAddressLine1.Focus()
                    ElseIf ddlAddressType.SelectedValue = "PO Box Address" Then
                        If lst.ContainsKey("Line1") Then
                            txtPOBoxAddressLine1.Text = CStr(lst("Line1"))
                        End If
                        If lst.ContainsKey("Line2") Then
                            txtPOBoxAddressLine2.Text = CStr(lst("Line2"))
                        End If
                        If lst.ContainsKey("Line3") Then
                            txtPOBoxAddressLine3.Text = CStr(lst("Line3"))
                        End If
                        If lst.ContainsKey("City") Then
                            txtPOBoxCity.Text = CStr(lst("City"))
                        End If
                        If lst.ContainsKey("Country") Then
                            SetComboValue(cmbPOBoxCountry, CStr(lst("Country")))
                        End If
                        If lst.ContainsKey("PostalCode") Then
                            txtPOBoxZipCode.Text = CStr(lst("PostalCode"))
                        End If
                        txtPOBoxAddressLine1.Focus()
                    End If

                End If
            End If
            radAddressMessage.VisibleOnPageLoad = False
        End Sub

        Protected Sub btnVerificationCancel_Click(sender As Object, e As System.EventArgs) Handles btnVerificationCancel.Click
            radAddressMessage.VisibleOnPageLoad = False
        End Sub
        Private Function CreatedSuggestedAddressTable(ByVal lst As Dictionary(Of String, String)) As String
            Dim bAddressSame As Boolean = False
            Dim TableHeading As StringBuilder = New StringBuilder
            Dim strTable As StringBuilder = New StringBuilder

            TableHeading.Append("<div class='row-div-bottom-line clearfix'>")
            TableHeading.Append("<div class='float-left w33-3 label'>")
            TableHeading.Append("Field")
            TableHeading.Append("</div>")
            TableHeading.Append("<div class='float-left w33-3 label'>")
            TableHeading.Append("Current")
            TableHeading.Append("</div>")
            TableHeading.Append("<div class='float-left w33-3 label'>")
            TableHeading.Append("Suggested")
            TableHeading.Append("</div>")
            TableHeading.Append("</div>")
            If ddlAddressType.SelectedValue = "Business Address" Then
                If lst.ContainsKey("Line1") AndAlso Not txtAddressLine1.Text = CStr(lst("Line1")) Then
                    strTable.Append(CreateRow("Line 1", txtAddressLine1.Text, CStr(lst("Line1"))))
                End If
                If lst.ContainsKey("Line2") AndAlso Not txtAddressLine2.Text = CStr(lst("Line2")) Then
                    strTable.Append(CreateRow("Line 2", txtAddressLine2.Text, CStr(lst("Line2"))))
                End If
                If lst.ContainsKey("Line3") AndAlso Not txtAddressLine3.Text = CStr(lst("Line3")) Then
                    strTable.Append(CreateRow("Line 3", txtAddressLine3.Text, CStr(lst("Line3"))))
                End If
                If lst.ContainsKey("PostalCode") AndAlso Not txtZipCode.Text = CStr(lst("PostalCode")) Then
                    strTable.Append(CreateRow("ZIP Code", txtZipCode.Text, CStr(lst("PostalCode"))))
                End If
                If lst.ContainsKey("City") AndAlso Not txtCity.Text = CStr(lst("City")) Then
                    strTable.Append(CreateRow("City", txtCity.Text, CStr(lst("City"))))
                End If
                If lst.ContainsKey("Country") AndAlso Not cmbCountry.SelectedItem.Text = CStr(lst("Country")) Then
                    strTable.Append(CreateRow("Contry", cmbCountry.SelectedItem.Text, CStr(lst("Country"))))
                End If
                ViewState("SuggestedList") = lst
                If Not strTable.ToString = String.Empty Then
                    TableHeading.Append("<div class='address-popup-data-container'>" & strTable.ToString() & "</div>")
                    Return TableHeading.ToString
                Else
                    Return strTable.ToString
                End If
            ElseIf ddlAddressType.SelectedValue = "Home Address" Then
                If lst.ContainsKey("Line1") AndAlso Not txtHomeAddressLine1.Text = CStr(lst("Line1")) Then
                    strTable.Append(CreateRow("Line 1", txtHomeAddressLine1.Text, CStr(lst("Line1"))))
                End If
                If lst.ContainsKey("Line2") AndAlso Not txtHomeAddressLine2.Text = CStr(lst("Line2")) Then
                    strTable.Append(CreateRow("Line 2", txtHomeAddressLine2.Text, CStr(lst("Line2"))))
                End If
                If lst.ContainsKey("Line3") AndAlso Not txtHomeAddressLine3.Text = CStr(lst("Line3")) Then
                    strTable.Append(CreateRow("Line 3", txtHomeAddressLine3.Text, CStr(lst("Line3"))))
                End If
                If lst.ContainsKey("PostalCode") AndAlso Not txtHomeZipCode.Text = CStr(lst("PostalCode")) Then
                    strTable.Append(CreateRow("ZIP Code", txtHomeZipCode.Text, CStr(lst("PostalCode"))))
                End If
                If lst.ContainsKey("City") AndAlso Not txtHomeCity.Text = CStr(lst("City")) Then
                    strTable.Append(CreateRow("City", txtHomeCity.Text, CStr(lst("City"))))
                End If
                If lst.ContainsKey("Country") AndAlso Not cmbHomeCountry.SelectedItem.Text = CStr(lst("Country")) Then
                    strTable.Append(CreateRow("Contry", cmbHomeCountry.SelectedItem.Text, CStr(lst("Country"))))
                End If
                ViewState("SuggestedList") = lst
                If Not strTable.ToString = String.Empty Then
                    TableHeading.Append("<div class='address-popup-data-container'>" & strTable.ToString() & "</div>")
                    Return TableHeading.ToString
                Else
                    Return strTable.ToString
                End If
            ElseIf ddlAddressType.SelectedValue = "Billing Address" Then
                If lst.ContainsKey("Line1") AndAlso Not txtBillingAddressLine1.Text = CStr(lst("Line1")) Then
                    strTable.Append(CreateRow("Line 1", txtBillingAddressLine1.Text, CStr(lst("Line1"))))
                End If
                If lst.ContainsKey("Line2") AndAlso Not txtBillingAddressLine2.Text = CStr(lst("Line2")) Then
                    strTable.Append(CreateRow("Line 2", txtBillingAddressLine2.Text, CStr(lst("Line2"))))
                End If
                If lst.ContainsKey("Line3") AndAlso Not txtBillingAddressLine3.Text = CStr(lst("Line3")) Then
                    strTable.Append(CreateRow("Line 3", txtBillingAddressLine3.Text, CStr(lst("Line3"))))
                End If
                If lst.ContainsKey("PostalCode") AndAlso Not txtBillingZipCode.Text = CStr(lst("PostalCode")) Then
                    strTable.Append(CreateRow("ZIP Code", txtBillingZipCode.Text, CStr(lst("PostalCode"))))
                End If
                If lst.ContainsKey("City") AndAlso Not txtBillingCity.Text = CStr(lst("City")) Then
                    strTable.Append(CreateRow("City", txtBillingCity.Text, CStr(lst("City"))))
                End If
                If lst.ContainsKey("Country") AndAlso Not cmbBillingCountry.SelectedItem.Text = CStr(lst("Country")) Then
                    strTable.Append(CreateRow("Contry", cmbBillingCountry.SelectedItem.Text, CStr(lst("Country"))))
                End If
                ViewState("SuggestedList") = lst
                If Not strTable.ToString = String.Empty Then
                    TableHeading.Append("<div class='address-popup-data-container'>" & strTable.ToString() & "</div>")
                    Return TableHeading.ToString
                Else
                    Return strTable.ToString
                End If
            ElseIf ddlAddressType.SelectedValue = "PO Box Address" Then
                If lst.ContainsKey("Line1") AndAlso Not txtPOBoxAddressLine1.Text = CStr(lst("Line1")) Then
                    strTable.Append(CreateRow("Line 1", txtPOBoxAddressLine1.Text, CStr(lst("Line1"))))
                End If
                If lst.ContainsKey("Line2") AndAlso Not txtPOBoxAddressLine2.Text = CStr(lst("Line2")) Then
                    strTable.Append(CreateRow("Line 2", txtPOBoxAddressLine2.Text, CStr(lst("Line2"))))
                End If
                If lst.ContainsKey("Line3") AndAlso Not txtPOBoxAddressLine3.Text = CStr(lst("Line3")) Then
                    strTable.Append(CreateRow("Line 3", txtPOBoxAddressLine3.Text, CStr(lst("Line3"))))
                End If
                If lst.ContainsKey("PostalCode") AndAlso Not txtPOBoxZipCode.Text = CStr(lst("PostalCode")) Then
                    strTable.Append(CreateRow("ZIP Code", txtPOBoxZipCode.Text, CStr(lst("PostalCode"))))
                End If
                If lst.ContainsKey("City") AndAlso Not txtPOBoxCity.Text = CStr(lst("City")) Then
                    strTable.Append(CreateRow("City", txtPOBoxCity.Text, CStr(lst("City"))))
                End If
                If lst.ContainsKey("Country") AndAlso Not cmbPOBoxCountry.SelectedItem.Text = CStr(lst("Country")) Then
                    strTable.Append(CreateRow("Contry", cmbPOBoxCountry.SelectedItem.Text, CStr(lst("Country"))))
                End If
                ViewState("SuggestedList") = lst
                If Not strTable.ToString = String.Empty Then
                    TableHeading.Append("<div class='address-popup-data-container'>" & strTable.ToString() & "</div>")
                    Return TableHeading.ToString
                Else
                    Return strTable.ToString
                End If
            End If


        End Function

        Private Function CreateRow(ByVal FieldName As String, ByVal CurrentValue As String, ByVal SuggestedValue As String) As String
            Dim sRow As StringBuilder = New StringBuilder
            sRow.Append("<div class='row-div-bottom-line clearfix'>")
            sRow.Append("<div class='float-left w33-3 label'>")
            sRow.Append(FieldName)
            sRow.Append("</div>")
            sRow.Append("<div class='float-left w33-3'>")
            sRow.Append(CurrentValue)
            sRow.Append("</div>")
            sRow.Append("<div class='float-left w33-3'>")
            sRow.Append(SuggestedValue)
            sRow.Append("</div>")
            sRow.Append("</div>")
            Return sRow.ToString()
        End Function

#End Region


    End Class
End Namespace
