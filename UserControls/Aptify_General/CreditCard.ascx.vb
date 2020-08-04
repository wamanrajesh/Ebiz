'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On
Imports Aptify.Applications.Accounting
Imports System.Data
Imports System.Globalization
Namespace Aptify.Framework.Web.eBusiness
    ''' <summary>
    ''' Aptify Credit Card User Control for e-Business
    ''' </summary>
    ''' <remarks></remarks>
    Partial Class CreditCard
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "CreditCard"
        Protected Const ATTRIBUTE_BILL_ME_LATER As String = "BillMeLaterDisplayText"
        Protected Const ATTRIBUTE_VISA_ENABLED_IMG As String = "VisaEnabled"
        Protected Const ATTRIBUTE_VISA_DISABLED_IMG As String = "VisaDisabled"
        Protected Const ATTRIBUTE_MASTERCARD_ENABLED_IMG As String = "MasterCardEnabled"
        Protected Const ATTRIBUTE_MASTERCARD_DISABLED_IMG As String = "MasterCardDisabled"
        Protected Const ATTRIBUTE_AMERICANEXPRESS_ENABLED_IMG As String = "AmericanExpressEnabled"
        Protected Const ATTRIBUTE_AMERICANEXPRESS_DISABLED_IMG As String = "AmericanExpressDisabled"
        Protected Const ATTRIBUTE_DISCOVER_ENABLED_IMG As String = "DiscoverEnabled"
        Protected Const ATTRIBUTE_DISCOVER_DISABLED_IMG As String = "DiscoverDisabled"
        'Suraj Issue 15014 4/24/13 , ATTRIBUTE_CONTORL_IsBillMeLaterDisable used for set the property "DisableBillMeLater"
        Protected ATTRIBUTE_CONTORL_IsBillMeLaterDisable As Boolean = False
        'Anil B for Credit Card recognization Performance on 21/jun/2013
        Protected Const ATTRIBUTE_PAYMENT_INFO_OBJECT As String = "PaymentInfo"
        Protected Const ATTRIBUTE_ENABLE_PAYMENTTYPE_SELECTION = "EnablePaymentTypeSelection"

        Dim m_iPOPaymentType As Integer = 0
        Dim m_iUserCreditStatus As Integer = 0
        Dim m_lUserCreditLimit As Long = 0
        Dim m_iCompanyCreditStatus As Integer = 0
        Dim m_lCompanyCreditLimit As Long = 0
        Dim m_bCreditChecklimit As Boolean = False
        Dim m_oPaymentInfo As Aptify.Applications.Accounting.PaymentInformation = Nothing
        Dim m_oGESPM As CommonMethods
        Dim bSaveForFuture As Boolean = True
        'Anil B Issue 10254 on 07-03-2013
        Dim bFromSavePaymentMethod As Boolean = False
        Protected Overrides Sub SetProperties()
            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME

            'call base method to set parent properties
            MyBase.SetProperties()
            If Not String.IsNullOrEmpty(AptifyApplication.GetEntityAttribute("Web Shopping Carts", "POPaymentTypeID")) Then
                m_iPOPaymentType = CInt(AptifyApplication.GetEntityAttribute("Web Shopping Carts", "POPaymentTypeID"))
            End If

            ''Visa Enabled Image
            If String.IsNullOrEmpty(VisaEnabled) Then
                VisaEnabled = Me.GetLinkValueFromXML(ATTRIBUTE_VISA_ENABLED_IMG)
            End If
            ''Visa Disabled Image
            If String.IsNullOrEmpty(VisaDisabled) Then
                VisaDisabled = Me.GetLinkValueFromXML(ATTRIBUTE_VISA_DISABLED_IMG)
            End If
            If String.IsNullOrEmpty(MasterCardEnabled) Then
                MasterCardEnabled = Me.GetLinkValueFromXML(ATTRIBUTE_MASTERCARD_ENABLED_IMG)
            End If

            If String.IsNullOrEmpty(MasterCardDisabled) Then
                MasterCardDisabled = Me.GetLinkValueFromXML(ATTRIBUTE_MASTERCARD_DISABLED_IMG)
            End If
            If String.IsNullOrEmpty(AmericanExpressEnabled) Then
                AmericanExpressEnabled = Me.GetLinkValueFromXML(ATTRIBUTE_AMERICANEXPRESS_ENABLED_IMG)
            End If
            If String.IsNullOrEmpty(AmericanExpressDisabled) Then
                AmericanExpressDisabled = Me.GetLinkValueFromXML(ATTRIBUTE_AMERICANEXPRESS_DISABLED_IMG)
            End If
            If String.IsNullOrEmpty(DiscoverEnabled) Then
                DiscoverEnabled = Me.GetLinkValueFromXML(ATTRIBUTE_DISCOVER_ENABLED_IMG)
            End If
            If String.IsNullOrEmpty(DiscoverDisabled) Then
                DiscoverDisabled = Me.GetLinkValueFromXML(ATTRIBUTE_DISCOVER_DISABLED_IMG)
            End If

            m_EnablePaymentTypeSelection = CBool(Me.GetPropertyValueFromXML(ATTRIBUTE_ENABLE_PAYMENTTYPE_SELECTION))

        End Sub

#Region "Credit Card Control Properties"

        ''' <summary>
        ''' Credit Card Number
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Dim m_CCNumber As String
        'Anil B Issue 10254 on 07-03-2013
        Dim m_ReferenceTransaction As String
        Dim m_ExpirationDate As Date
        Public Property CCNumber() As String
            Get
                EnsureChildControls()
                If IsNumeric(txtCCNumber.Text) Then
                    Return txtCCNumber.Text
                Else
                    Return CStr(hfCCNumber.Value)
                End If
            End Get
            Set(ByVal Value As String)
                EnsureChildControls()
                If IsNumeric(txtCCNumber.Text) Then
                    txtCCNumber.Text = Value
                Else
                    hfCCNumber.Value = Value
                End If
            End Set
        End Property
        'Anil B Issue 10254 on 20/04/2013
        'Define property for CCPrtian number
        Public Property CCPartial() As String
            Get
                Return txtCCNumber.Text
            End Get
            Set(ByVal Value As String)
                txtCCNumber.Text = Value
            End Set
        End Property
        'Anil B Issue 10254 on 07-03-2013
        'Define property for Reference transaction number for perticuler SPM
        Public Property ReferenceTransactionNumber() As String
            Get
                If ViewState.Item("ReferenceTransactionNumber") IsNot Nothing Then
                    Return ViewState.Item("ReferenceTransactionNumber").ToString()
                Else
                    Return ""
                End If
            End Get
            Set(ByVal value As String)
                ViewState.Item("ReferenceTransactionNumber") = value
            End Set
        End Property
        'Anil B Issue 10254 on 07-03-2013
        'Define property for  transaction Expiration date for perticuler SPM
        Public Property ReferenceExpiration() As Date
            Get
                If ViewState.Item("ReferenceExpiration") IsNot Nothing Then
                    Return CDate(ViewState.Item("ReferenceExpiration"))
                End If
            End Get
            Set(ByVal value As Date)
                ViewState.Item("ReferenceExpiration") = value
            End Set
        End Property
        ''' <summary>
        ''' Credit Card Security Number
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CCSecurityNumber() As String
            '   Added property for Credit Card security number feature on e-business site.
            '   Change made by Vijay Sitlani for Issue 5369 
            Get
                EnsureChildControls()
                Return txtCCSecurityNumber.Text
            End Get
            Set(ByVal Value As String)
                EnsureChildControls()
                txtCCSecurityNumber.Text = Value
            End Set
        End Property
        'HP Issue#8972: need ability to disable validators if shopping cart total due is zero
        ''' <summary>
        ''' Credit Card Expiration validator setting
        ''' </summary>
        Public Property CCNumberValidatorSetting() As Boolean
            Get
                Return RequiredFieldValidator1.Enabled
            End Get
            Set(ByVal Value As Boolean)
                RequiredFieldValidator1.Enabled = Value
            End Set
        End Property
        'HP Issue#8972: need ability to disable validators if shopping cart total due is zero
        ''' <summary>
        ''' Credit Card Security Number validator setting
        ''' </summary>
        Public Property CCSecurityNumberValidatorSetting() As Boolean
            Get
                Return RequiredFieldValidator2.Enabled
            End Get
            Set(ByVal Value As Boolean)
                RequiredFieldValidator2.Enabled = Value
            End Set
        End Property
        ''' <summary>
        ''' Credit Card Expiration Date
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CCExpireDate() As String
            Get
                EnsureChildControls()
                ' Build the appropriate string of the date information
                ' to pass back to the parent
                Dim dExpDate As Date
                'Issue 20655 : Incorrect Expiry Date Shows Up On the Order And Paypal when setting UK culture
                Dim vExpDate As String
                vExpDate = dropdownMonth.SelectedValue & "/1/" & _
                                    dropdownYear.SelectedValue
                ' dExpDate = CDate(dropdownMonth.SelectedValue & "/1/" & _
                '                  dropdownYear.SelectedValue)
                Dim usCulture As CultureInfo = New CultureInfo("en-US")
                dExpDate = DateTime.Parse(vExpDate, usCulture.DateTimeFormat)

                dExpDate = DateAdd(DateInterval.Month, 1, dExpDate)
                dExpDate = DateAdd(DateInterval.Day, -1, dExpDate)
                Return dExpDate.ToString

            End Get
            Set(ByVal Value As String)
                Dim d As DateTime

                Me.EnsureChildControls()
                ' Break apart the passed in date values
                ' and select the appropriate fields

                If IsDate(Value) Then
                    d = CDate(Value)
                    dropdownMonth.SelectedIndex = d.Month - 1
                    'dropdownDay.SelectedIndex = d.Day - 1
                    dropdownYear.SelectedIndex = d.Year - Now.Year
                Else
                    dropdownMonth.SelectedIndex = Today.Month - 1
                    'dropdownDay.SelectedIndex = Today.Day - 1
                    dropdownYear.SelectedIndex = (Today.Year + 1) - Now.Year
                End If
            End Set
        End Property

        ''' <summary>
        ''' Payment Type ID
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PaymentTypeID() As Long
            Get
                EnsureChildControls()
                If chkBillMeLater.Checked Then
                    Return m_iPOPaymentType
                Else
                    If cmbCreditCard.SelectedItem Is Nothing Then
                        Return -1
                    Else
                        Return CLng(cmbCreditCard.SelectedItem.Value)
                    End If
                End If
            End Get
            Set(ByVal Value As Long)
                Try
                    EnsureChildControls()
                    Dim oItem As System.Web.UI.WebControls.ListItem
                    oItem = cmbCreditCard.Items.FindByValue(CStr(Value))
                    If Not oItem Is Nothing Then
                        oItem.Selected = True
                    End If
                Catch ex As Exception
                    Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                End Try
            End Set
        End Property

        Public Property ShowPaymentTypeSelection() As Boolean
            Get
                Me.EnsureChildControls()
                Return PaymentTypeSelection.Visible
            End Get
            Set(ByVal Value As Boolean)
                Me.EnsureChildControls()
                PaymentTypeSelection.Visible = Value
            End Set
        End Property

        ''RashmiP Issue 6781
        Public Property PONumber() As String
            Get
                EnsureChildControls()
                Return txtPONumber.Text
            End Get
            Set(ByVal Value As String)
                EnsureChildControls()
                txtPONumber.Text = Value
            End Set
        End Property
        Public Property BillMeLaterChecked() As Boolean
            Get
                If chkBillMeLater.Checked Then
                    Return True
                Else
                    Return False
                End If
            End Get
            Set(ByVal value As Boolean)
                chkBillMeLater.Checked = value
            End Set
        End Property
        Public Property UserCreditStatus() As Integer
            Get
                Return m_iUserCreditStatus
            End Get
            Set(ByVal value As Integer)
                m_iUserCreditStatus = value
            End Set
        End Property
        Public Property UserCreditLimit() As Long
            Get
                Return m_lUserCreditLimit
            End Get
            Set(ByVal value As Long)
                m_lUserCreditLimit = value
            End Set
        End Property
        Public Property CompanyCreditStatus() As Integer
            Get
                Return m_iCompanyCreditStatus
            End Get
            Set(ByVal value As Integer)
                m_iCompanyCreditStatus = value
            End Set
        End Property
        Public Property CompanyCreditLimit() As Long
            Get
                Return m_lCompanyCreditLimit
            End Get
            Set(ByVal value As Long)
                m_lCompanyCreditLimit = value
            End Set
        End Property

        Public Property CreditCheckLimit() As Boolean
            Get
                Return m_bCreditChecklimit
            End Get
            Set(ByVal value As Boolean)
                m_bCreditChecklimit = value
                chkBillMeLater.Visible = m_bCreditChecklimit
                lblBillMelater.Visible = m_bCreditChecklimit
            End Set
        End Property
        ''RashmiP, issue 6781
        Public Overridable ReadOnly Property BillMeLaterDisplayText() As String
            Get
                If Not ViewState.Item(ATTRIBUTE_BILL_ME_LATER) Is Nothing Then
                    Return CStr(ViewState.Item(ATTRIBUTE_BILL_ME_LATER))
                Else
                    Dim value As String = Me.GetGlobalAttributeValue(ATTRIBUTE_BILL_ME_LATER)
                    If Not String.IsNullOrEmpty(value) Then
                        ViewState.Item(ATTRIBUTE_BILL_ME_LATER) = value
                    End If
                    Return value
                End If
            End Get

        End Property

        Private Property VisaEnabled() As String
            Get
                If Not ViewState.Item(ATTRIBUTE_VISA_ENABLED_IMG) Is Nothing Then
                    Return CStr(ViewState.Item(ATTRIBUTE_VISA_ENABLED_IMG))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState.Item(ATTRIBUTE_VISA_ENABLED_IMG) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        Private Property VisaDisabled() As String
            Get
                If Not ViewState.Item(ATTRIBUTE_VISA_DISABLED_IMG) Is Nothing Then
                    Return CStr(ViewState.Item(ATTRIBUTE_VISA_DISABLED_IMG))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState.Item(ATTRIBUTE_VISA_DISABLED_IMG) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        Private Property MasterCardEnabled() As String
            Get
                If Not ViewState.Item(ATTRIBUTE_MASTERCARD_ENABLED_IMG) Is Nothing Then
                    Return CStr(ViewState.Item(ATTRIBUTE_MASTERCARD_ENABLED_IMG))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState.Item(ATTRIBUTE_MASTERCARD_ENABLED_IMG) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        Private Property MasterCardDisabled() As String
            Get
                If Not ViewState.Item(ATTRIBUTE_MASTERCARD_DISABLED_IMG) Is Nothing Then
                    Return CStr(ViewState.Item(ATTRIBUTE_MASTERCARD_DISABLED_IMG))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState.Item(ATTRIBUTE_MASTERCARD_DISABLED_IMG) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        Private Property AmericanExpressEnabled() As String
            Get
                If Not ViewState.Item(ATTRIBUTE_AMERICANEXPRESS_ENABLED_IMG) Is Nothing Then
                    Return CStr(ViewState.Item(ATTRIBUTE_AMERICANEXPRESS_ENABLED_IMG))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState.Item(ATTRIBUTE_AMERICANEXPRESS_ENABLED_IMG) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        Private Property AmericanExpressDisabled() As String
            Get
                If Not ViewState.Item(ATTRIBUTE_AMERICANEXPRESS_DISABLED_IMG) Is Nothing Then
                    Return CStr(ViewState.Item(ATTRIBUTE_AMERICANEXPRESS_DISABLED_IMG))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState.Item(ATTRIBUTE_AMERICANEXPRESS_DISABLED_IMG) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        Private Property DiscoverEnabled() As String
            Get
                If Not ViewState.Item(ATTRIBUTE_DISCOVER_ENABLED_IMG) Is Nothing Then
                    Return CStr(ViewState.Item(ATTRIBUTE_DISCOVER_ENABLED_IMG))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState.Item(ATTRIBUTE_DISCOVER_ENABLED_IMG) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        Private Property DiscoverDisabled() As String
            Get
                If Not ViewState.Item(ATTRIBUTE_DISCOVER_DISABLED_IMG) Is Nothing Then
                    Return CStr(ViewState.Item(ATTRIBUTE_DISCOVER_DISABLED_IMG))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState.Item(ATTRIBUTE_DISCOVER_DISABLED_IMG) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        Public ReadOnly Property SaveCardforFutureUse As Boolean
            Get
                If chkSaveforFutureUse.Checked Then
                    Return True
                Else
                    Return False
                End If
            End Get

        End Property
        'Anil B for issue 15374 on 02/04/2013
        'Property for future use checkbox 
        Public Property SetchkSaveforFutureUse() As Boolean
            Get
                Return bSaveForFuture
            End Get
            Set(ByVal value As Boolean)
                bSaveForFuture = value
                chkSaveforFutureUse.Visible = bSaveForFuture
            End Set
        End Property
        'Suraj Issue 15014 4/24/13 , declare the prpoperty, this property set on fundraising page for disable the bill me later check box
        Public Property DisableBillMeLater() As Boolean
            Get
                Return Me.ATTRIBUTE_CONTORL_IsBillMeLaterDisable
            End Get
            Set(ByVal value As Boolean)
                Me.ATTRIBUTE_CONTORL_IsBillMeLaterDisable = value
            End Set
        End Property

        'Anil B for Credit Card recognization Performance on 21/jun/2013
        Public Property PaymentInfo() As Aptify.Applications.Accounting.PaymentInformation
            Get
                If Session(ATTRIBUTE_PAYMENT_INFO_OBJECT) Is Nothing Then
                    Session(ATTRIBUTE_PAYMENT_INFO_OBJECT) = DirectCast(ShoppingCart1.GetOrderObject(Session, Page.User, Application).Fields("PaymentInformationID").EmbeddedObject, PaymentInformation)
                End If
                Return CType(Session(ATTRIBUTE_PAYMENT_INFO_OBJECT), PaymentInformation)
            End Get
            Set(ByVal value As Aptify.Applications.Accounting.PaymentInformation)
                Session(ATTRIBUTE_PAYMENT_INFO_OBJECT) = value
            End Set
        End Property

        Dim m_CompanyPayemt As Boolean
        Public Property CompanyPayment() As Boolean
            Get
                Return m_CompanyPayemt
            End Get
            Set(value As Boolean)
                m_CompanyPayemt = value
            End Set
        End Property

        Dim m_IndividualPayment As Boolean
        Public Property IndividualPayment() As Boolean
            Get
                Return m_IndividualPayment
            End Get
            Set(value As Boolean)
                m_IndividualPayment = value
            End Set
        End Property

        Dim m_EnablePaymentTypeSelection As Boolean
        Public ReadOnly Property EnablePaymentTypeSelection() As Boolean
            Get
                Return m_EnablePaymentTypeSelection
            End Get
        End Property

#End Region

        Public Sub LoadCreditCardInfo()
            Dim sSQL As Text.StringBuilder = New Text.StringBuilder
            Dim dt As Data.DataTable
            Try

                'Ansar Shaikh - Issue 11986 - 12/27/2011
                'Anil B Issue 16167 on 07-05-2013
                'Changed query
                sSQL.Append("SELECT PT.ID,CC.Name FROM " & Database & _
                         "..vwPaymentTypes PT inner join " & Database & "..vwCreditCardTypes CC on PT.CreditCardTypeID= CC.ID WHERE PT.Active=1 AND PT.WebEnabled=1 AND " & _
                          "(PT.Type='Credit Card' OR PT.Type='Credit Card Reference Transaction') " & _
                          " UNION SELECT '-1',' ' ORDER BY CC.Name") ''Query changed by RashmiP, issue 9024

                dt = DataAction.GetDataTable(sSQL.ToString)
                cmbCreditCard.DataSource = dt
                cmbCreditCard.DataTextField = "Name"
                cmbCreditCard.DataValueField = "ID"
                cmbCreditCard.DataBind()
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub




        Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()

            'Anil B for Credit Card recognization Performance on 21/jun/2013
            If PaymentInfo IsNot Nothing Then
                m_oPaymentInfo = PaymentInfo
            End If
            Dim iDay As Integer
            Dim iYear As Integer
            'Anil B Issue 10254 on 07-03-2013
            'Set Enable Desabled Card and Error message 
            If IsPostBack AndAlso txtCCNumber.Text = String.Empty Then
                DesabledCreditCard()
                lblError.Visible = False
                bFromSavePaymentMethod = False
            End If
            'Anil B Issue 10254 on 07-03-2013
            'Set Javascript function to control
            txtCCNumber.Attributes.Add("onblur", "javascript:SecurityDesabled()")
            txtCCNumber.Attributes.Add(" onchange", "javascript:SecurityEnabled()")
            m_oGESPM = New CommonMethods(DataAction, AptifyApplication, User1, Database)

            Try

                If Not Me.IsPostBack Then
                    'RashmiP issue 6781
                    ' trPONum.Visible = False
                    hdnCCPartialNumber.Value = txtCCNumber.Text
                    chkBillMeLater.DataBind()
                    chkBillMeLater.Visible = False
                    lblBillMelater.Visible = False
                    lblBillMelater.Text = BillMeLaterDisplayText
                    ShowBillMeLater()
                    InitializeControlsValues()
                    LoadSavedPayments()
                    'Anil B for issue 10254 on 29-03-13
                    'set save for future use checkbox
                    chkSaveforFutureUse.Visible = False
                    ShowPaymentTypePanel()

                End If
                ShowHideControls()  'Added By Sandeep for Issue 14671 on 20/02/2013
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Private Sub InitializeControlsValues()
            ' set default values
            'Anil B Issue 10254 on 20/04/2013
            ' For the year field, we load in the current year until +100 in the future
            For iYear = Now.Year To Now.Year + 100
                dropdownYear.Items.Add(iYear.ToString())
            Next
            CCExpireDate = DateAdd(DateInterval.Year, 1, Date.Now).ToShortDateString
            Me.txtCCNumber.Text = ""
            Me.txtCCSecurityNumber.Text = ""
            lblError.Text = ""
            cmbCreditCard.ClearSelection()
            lblSavedPayment.Visible = False
            cmbSavedPaymentMethod.Visible = False
            ImgVisa.ImageUrl = VisaDisabled
            ImgMasterCard.ImageUrl = MasterCardDisabled
            ImgAmericanExpress.ImageUrl = AmericanExpressDisabled
            ImgDiscover.ImageUrl = DiscoverDisabled
            cmbCreditCard.Visible = False
            chkSaveforFutureUse.Visible = False
        End Sub
        Private Sub vldExpirationDate_ServerValidate(ByVal source As System.Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles vldExpirationDate.ServerValidate
            args.IsValid = IsDate(CCExpireDate)
        End Sub

        ''' <summary>
        ''' RashmiP issue 6781, 09/20/10
        ''' procedure set bill me later option true, if Company and User's credit Status is approved and credit limit is availabe 
        ''' contion check if payment type is Bill Me Later. Otherwise return False. 
        ''' </summary>
        Private Sub ShowBillMeLater()
            Dim sError As String
            Dim bSetVisible As Boolean
            'Suraj Issue 15014 4/24/13, if ATTRIBUTE_CONTORL_IsBillMeLaterDisable is True then remove the bill me later check box for fundraising page
            Try
                If m_iPOPaymentType > 0 Then
                    If CompanyCreditStatus = 2 AndAlso CompanyCreditLimit > 0 AndAlso CreditCheckLimit AndAlso ATTRIBUTE_CONTORL_IsBillMeLaterDisable = False Then
                        bSetVisible = True
                    ElseIf UserCreditStatus = 2 AndAlso UserCreditLimit > 0 AndAlso CreditCheckLimit AndAlso ATTRIBUTE_CONTORL_IsBillMeLaterDisable = False Then
                        bSetVisible = True
                    Else
                        bSetVisible = False
                    End If
                Else
                    bSetVisible = False
                End If
                chkBillMeLater.Visible = bSetVisible
                lblBillMelater.Visible = bSetVisible
            Catch ex As Exception

            End Try

        End Sub

        ''Rashmi, Issue 9024, Credit Card Validation
        'Anil B Issue 10254 on 20/04/2013
        Protected Sub txtCCNumber_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
            Try
                'Anil B Issue 10254 on 07-03-2013
                'If this Event call from Save payment dropedown then exit this event
                If bFromSavePaymentMethod OrElse cmbSavedPaymentMethod.SelectedIndex > 0 Then
                    Exit Sub
                End If
                Dim lPaymentID As Long
                Dim lCreditCard As Long
                lblError.Text = ""
                If txtCCNumber.Text = "" Then
                    cmbCreditCard.SelectedValue = "-1"
                    DesabledCreditCard()
                Else
                    'Anil B for Credit Card recognization Performance on 21/jun/2013
                    If PaymentInfo IsNot Nothing AndAlso PaymentInfo.ValidateRange(Convert.ToString(txtCCNumber.Text), lPaymentID, lCreditCard) Then
                        'Anil B Issue 16167 on 07-05-2013
                        'Add Condition to check active payment card.
                        If lPaymentID > 0 AndAlso lCreditCard > 0 Then
                            cmbCreditCard.SelectedValue = CStr(lPaymentID)
                            SelectCardType(CStr(cmbCreditCard.SelectedItem.Text).Trim)
                            SaveForFutureUse(CInt(lPaymentID))
                            txtCCSecurityNumber.Focus()
                            lblError.Text = ""
                            'Anil B Issue 10254 on 20/04/2013
                            'Set CCNumber when card number is numeric
                            If IsNumeric(txtCCNumber.Text) Then
                                CCNumber = txtCCNumber.Text
                            End If
                            'cmbSavedPaymentMethod.SelectedIndex = 0
                        Else
                            cmbCreditCard.SelectedValue = "-1"
                            DesabledCreditCard()
                            cmbCreditCard.Enabled = True
                            'Anil B Issue 16167 on 07-05-2013
                            'Displayed the error message
                            lblError.Text = "Invalid Card Type. Please try entering your card number again."
                            lblError.Visible = True
                        End If
                    Else
                        cmbCreditCard.SelectedValue = "-1"
                        SelectCardType("")
                        'Anil B Issue 10254 on 20/04/2013
                        'Displayed the error message
                        lblError.Text = "Card number does not match card type."
                        lblError.Visible = True
                        chkSaveforFutureUse.Visible = False
                    End If
                End If
                hdnCCPartialNumber.Value = txtCCNumber.Text
                upnlCreditCard.Update()
                upnlError.Update()
                txtCCSecurityNumber.Enabled = True
                ''Suraj Issue 16258 , 5/15/13 , set theCCSecurityNumber no
                If IsNumeric(txtCCSecurityNumber.Text) Then
                    CCSecurityNumber = txtCCSecurityNumber.Text
                End If
            Catch ex As Exception
            End Try
        End Sub

        'Anil B Issue 10254 on 07-03-2013
        'Function for Desabled credite card
        Private Sub DesabledCreditCard()
            ImgVisa.ImageUrl = VisaDisabled
            ImgMasterCard.ImageUrl = MasterCardDisabled
            ImgAmericanExpress.ImageUrl = AmericanExpressDisabled
            ImgDiscover.ImageUrl = DiscoverDisabled
        End Sub

        ''' <summary>
        ''' Rashmi P, Issue 10254, Apply Saved Payment Method in Ebusiness, 31/12/12
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        ''' 
        Public Sub LoadSavedPayments()
            Dim DT As Data.DataTable
            Try


                DT = m_oGESPM.LoadSaveSavedPaymentMethods
                If Not DT Is Nothing AndAlso DT.Rows.Count > 0 Then
                    cmbSavedPaymentMethod.DataSource = DT
                    cmbSavedPaymentMethod.DataTextField = "DispalyName"
                    cmbSavedPaymentMethod.DataValueField = "ID"
                    cmbSavedPaymentMethod.DataBind()
                    'Anil B Issue 10254 
                    'Add Condition to set first blank item to savepayment dropdown
                    cmbSavedPaymentMethod.Items.Insert(0, New ListItem("- Select -", String.Empty))
                    lblSavedPayment.Visible = True
                    cmbSavedPaymentMethod.Visible = True
                    If cmbSavedPaymentMethod.Items.Count > 0 Then
                        cmbSavedPaymentMethod.SelectedIndex = 0
                        cmbSavedPaymentMethod_SelectedIndexChanged(Nothing, Nothing)
                    End If
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub cmbSavedPaymentMethod_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSavedPaymentMethod.SelectedIndexChanged
            Try
                bFromSavePaymentMethod = True
                If cmbSavedPaymentMethod.Items.Count > 0 Then
                    lblError.Text = ""
                    If cmbSavedPaymentMethod.SelectedIndex > 0 Then
                        Dim oGESPM As Aptify.Framework.BusinessLogic.GenericEntity.AptifyGenericEntityBase
                        oGESPM = Me.AptifyApplication.GetEntityObject("PersonSavedPaymentMethods", CInt(cmbSavedPaymentMethod.SelectedValue))
                        If Not oGESPM Is Nothing Then
                            txtCCNumber.Text = CStr(oGESPM.GetValue("CCPartial"))
                            CCPartial = CStr(oGESPM.GetValue("CCPartial"))
                            cmbCreditCard.SelectedValue = CStr(oGESPM.GetValue("PaymentTypeID"))
                            'Anil B Issue 10254 on 20/04/2013
                            'Set payment typeid to property
                            PaymentTypeID = CLng((oGESPM.GetValue("PaymentTypeID")))
                            Dim sExpireDate As Date = CDate(oGESPM.GetValue("CCExpireDate"))
                            dropdownMonth.SelectedValue = CStr(sExpireDate.Month)
                            dropdownYear.SelectedValue = CStr(sExpireDate.Year)
                            CCNumber = CStr(oGESPM.GetValue("CCAccountNumber"))
                            'Anil B Issue 10254 on 07-03-2013
                            'Set Reference Transaction to the property
                            ReferenceTransactionNumber = CStr(oGESPM.GetValue("ReferenceTransactionNumber"))
                            'Anil B for issue 10254 on 29-03-13
                            'Check date is blank
                            If Not IsDBNull(oGESPM.GetValue("ReferenceExpiration")) AndAlso CStr(oGESPM.GetValue("ReferenceExpiration")).Trim <> "" Then
                                ReferenceExpiration = CDate(oGESPM.GetValue("ReferenceExpiration"))
                            End If
                            SelectCardType(cmbCreditCard.SelectedItem.Text.Trim)
                            SaveForFutureUse(CInt(oGESPM.GetValue("PaymentTypeID")))
                            SetEnableDesable(False)
                        End If
                    Else
                        'Anil B Issue 10254 on 07-03-2013
                        'Set Control
                        SetEnableDesable(True)
                        DesabledCreditCard()
                        txtCCNumber.Text = ""
                        'Anil B Issue 10254 on 20/04/2013
                        'Reset control values
                        txtCCSecurityNumber.Text = ""
                        dropdownMonth.SelectedIndex = Today.Month - 1
                        dropdownYear.SelectedIndex = (Today.Year + 1) - Now.Year
                    End If
                End If
                hdnCCPartialNumber.Value = txtCCNumber.Text
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        'Anil B Issue 10254 on 07-03-2013
        'Set Control Enabled desabled
        Private Sub SetEnableDesable(ByVal Flag As Boolean)
            'Anil B Issue 10254 on 20/04/2013
            'Set CCnumber readonly
            txtCCNumber.ReadOnly = Not Flag
            dropdownMonth.Enabled = Flag
            dropdownYear.Enabled = Flag
            chkSaveforFutureUse.Visible = False
        End Sub
        ''' <summary>
        ''' Rashmi P, Issue 10737
        ''' Select Payment Type
        ''' </summary>
        ''' <param name="PaymentType"></param>
        ''' <remarks></remarks>
        Public Sub SelectCardType(ByVal PaymentType As String)

            ImgVisa.ImageUrl = VisaDisabled
            ImgMasterCard.ImageUrl = MasterCardDisabled
            ImgAmericanExpress.ImageUrl = AmericanExpressDisabled
            ImgDiscover.ImageUrl = DiscoverDisabled

            Select Case (PaymentType)
                Case "Visa"
                    ImgVisa.ImageUrl = VisaEnabled
                    ImgVisa.DataBind()
                Case "MasterCard"
                    ImgMasterCard.ImageUrl = MasterCardEnabled
                    ImgMasterCard.DataBind()
                Case "American Express"
                    ImgAmericanExpress.ImageUrl = AmericanExpressEnabled
                    ImgAmericanExpress.DataBind()
                Case "Discover"
                    ImgDiscover.ImageUrl = DiscoverEnabled
                    ImgDiscover.DataBind()
            End Select

        End Sub
        ''' <summary>
        ''' Rashmi P, 10737
        ''' Funtion check and Return True if AllowSaveforFutureUse is true for Payment Type
        ''' </summary>
        ''' <param name="PaymentTypeID"></param>
        ''' <remarks></remarks>
        Private Sub SaveForFutureUse(ByVal PaymentTypeID As Integer)
            Try
                Dim sSql As String
                Dim params(0) As IDataParameter
                Dim dt As DataTable
                sSql = Database & ".." & "spGetPaymentType"
                params(0) = Me.DataAction.GetDataParameter("@ID", SqlDbType.Int, PaymentTypeID)
                dt = Me.DataAction.GetDataTableParametrized(sSql, CommandType.StoredProcedure, params)
                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                    'Anil B for issue 15374 on 02/04/2013
                    'Set save for future use checkbox invisible for some pages
                    If CBool(dt.Rows(0).Item("AllowSaveforFutureUse")) = True AndAlso SetchkSaveforFutureUse = True Then
                        chkSaveforFutureUse.Visible = True
                    Else
                        chkSaveforFutureUse.Visible = False
                    End If
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        'Added By Sandeep for Issue 14671 on 20/02/2013
        Protected Sub chkBillMeLater_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkBillMeLater.CheckedChanged
            ShowHideControls()
            'Anil B for issue 10254 on 29-03-13
            'Set control value
            If chkBillMeLater.Checked = False Then
                'Anil B Issue 10254 on 20/04/2013
                'check if SPM dropdown have items
                If cmbSavedPaymentMethod.Items.Count > 0 Then
                    cmbSavedPaymentMethod.SelectedIndex = 0
                End If
                dropdownMonth.SelectedIndex = Today.Month - 1
                dropdownYear.SelectedIndex = (Today.Year + 1) - Now.Year
                dropdownMonth.Enabled = True
                dropdownYear.Enabled = True
                chkSaveforFutureUse.Checked = False
            End If
        End Sub
        'Added By Sandeep for Issue 14671 on 20/02/2013
        Protected Sub ShowHideControls()
            If chkBillMeLater.Checked = True Then
                RequiredFieldValidator1.Enabled = False
                RequiredFieldValidator2.Enabled = False
                tblMain.Visible = False
                tblPONum.Visible = True
            Else
                tblMain.Visible = True
                tblPONum.Visible = False
                RequiredFieldValidator1.Enabled = True
                RequiredFieldValidator2.Enabled = True
                chkSaveforFutureUse.Visible = False
            End If
        End Sub

        Private Sub ShowPaymentTypePanel()
            Try

                If EnablePaymentTypeSelection AndAlso (User1.CompanyID > 0 OrElse UserIsGroupAdmin()) Then
                    pnlPaymentType.Visible = True

                    If IndividualPayment Then
                        rbIndividualPayment.Checked = True
                    ElseIf CompanyPayment Then
                        rbCompanyPayment.Checked = True
                    Else
                        rbCompanyPayment.Checked = True
                    End If

                    If Convert.ToString(Session("BillingOrderType")) = "INDIVIDUAL" Then
                        rbIndividualPayment.Checked = True
                    ElseIf Convert.ToString(Session("BillingOrderType")) = "COMPANY" Then
                        rbCompanyPayment.Checked = True
                    End If
                Else
                    pnlPaymentType.Visible = False
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Function UserIsGroupAdmin() As Boolean
            Try
                Return m_oGESPM.UserIsGroupAdmin(User1.PersonID)
            Catch ex As Exception
                Return False
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Function

        Protected Sub rbCompanyPayment_CheckedChanged(sender As Object, e As System.EventArgs) Handles rbCompanyPayment.CheckedChanged
            If rbCompanyPayment.Checked = True Then
                Session("BillingOrderType") = "COMPANY"
            End If
        End Sub

        Protected Sub rbIndividualPayment_CheckedChanged(sender As Object, e As System.EventArgs) Handles rbIndividualPayment.CheckedChanged
            If rbIndividualPayment.Checked = True Then
                Session("BillingOrderType") = "INDIVIDUAL"
            End If
        End Sub
    End Class
End Namespace
