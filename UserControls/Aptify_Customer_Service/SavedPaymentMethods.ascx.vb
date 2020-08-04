'Aptify e-Business 5.5.1/5.5.2 Hotfix Issue 20458, December 2014
Option Strict On
Option Explicit On
Imports Aptify.Applications.Accounting
Imports System.Data
Imports Aptify.Framework.Web.eBusiness
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Telerik.Web.UI
Imports Aptify.Applications.OrderEntry.Payments
Imports Aptify.Framework.Application


Namespace Aptify.Framework.Web.eBusiness.CustomerService
    Partial Class SavedPaymentMethods
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "SavedPaymentMethods"
        Protected Const ATTRIBUTE_VISA_ENABLED_IMG As String = "VisaEnabled"
        Protected Const ATTRIBUTE_VISA_DISABLED_IMG As String = "VisaDisabled"
        Protected Const ATTRIBUTE_MASTERCARD_ENABLED_IMG As String = "MasterCardEnabled"
        Protected Const ATTRIBUTE_MASTERCARD_DISABLED_IMG As String = "MasterCardDisabled"
        Protected Const ATTRIBUTE_AMERICANEXPRESS_ENABLED_IMG As String = "AmericanExpressEnabled"
        Protected Const ATTRIBUTE_AMERICANEXPRESS_DISABLED_IMG As String = "AmericanExpressDisabled"
        Protected Const ATTRIBUTE_DISCOVER_ENABLED_IMG As String = "DiscoverEnabled"
        Protected Const ATTRIBUTE_DISCOVER_DISABLED_IMG As String = "DiscoverDisabled"
        Protected Const ATTRIBUTE_DELETE_IMG As String = "DeleteImage"
        Protected Const ATTRIBUTE_EDIT_IMG As String = "EditImage"
        Dim bIsValidCard As Boolean = True


        Dim m_oGESPM As CommonMethods
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                m_oGESPM = New CommonMethods(DataAction, AptifyApplication, User1, Database)
                RequiredFieldValidator1.ErrorMessage = "Required"
                RequiredFieldValidator3.ErrorMessage = "Required"
                'Anil B Issue 15409 on 08-04-2013
                'Enabled the date control
                bIsValidCard = True
                SetDateDesabled(True)
                SetProperties()
                If Not IsPostBack Then
                    'Suraj Issue 14450 3/22/13 ,this method use to apply the odrering of rad grid first column
                    AddExpressionSavePayment()
                    Session("IsPageRefresh") = Server.UrlDecode(System.DateTime.Now.ToString())
                    FillGrid()
                    LoadCreditCardInfo()
                    InitializeControlsValues()
                End If
                'Anil B Issue 10254 on 20/04/2013
                'Set credit card desabled
                If IsPostBack Then
                    If txtCCNumber.Text.Trim = "" Then
                        SetCardDesabled()
                    End If
                End If
            Catch ex As Exception

            End Try
        End Sub
        Private Sub InitializeControlsValues()
            ' set default values
            ' For the year field, we load in the current year until +15 in the future
            'Anil B Issue 10254 on 20/04/2013
            ' For the year field, we load in the current year until +100 in the future
            Dim iYear As Integer
            For iYear = Now.Year To Now.Year + 100
                dropdownYear.Items.Add(iYear.ToString())
            Next
            CCExpireDate = DateAdd(DateInterval.Year, 1, Date.Now).ToShortDateString
            Me.txtCCNumber.Text = ""
            Me.txtCCSecurityNumber.Text = ""
            lblError.Text = ""
            cmbCreditCard.ClearSelection()
            ImgVisa.ImageUrl = VisaDisabled
            ImgMasterCard.ImageUrl = MasterCardDisabled
            ImgAmericanExpress.ImageUrl = AmericanExpressDisabled
            ImgDiscover.ImageUrl = DiscoverDisabled
            cmbCreditCard.Visible = False

        End Sub

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

        Private Property DeleteImage As String
            Get
                If Not ViewState.Item(ATTRIBUTE_DELETE_IMG) Is Nothing Then
                    Return CStr(ViewState.Item(ATTRIBUTE_DELETE_IMG))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState.Item(ATTRIBUTE_DELETE_IMG) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        Private Property EditImage As String
            Get
                If Not ViewState.Item(ATTRIBUTE_EDIT_IMG) Is Nothing Then
                    Return CStr(ViewState.Item(ATTRIBUTE_EDIT_IMG))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState.Item(ATTRIBUTE_EDIT_IMG) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        Public Property CCExpireDate() As String
            Get
                EnsureChildControls()
                ' Build the appropriate string of the date information
                ' to pass back to the parent
                Dim dExpDate As Date
                dExpDate = CDate(dropdownMonth.SelectedValue & "/1/" & _
                                   dropdownYear.SelectedValue)
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
                    dropdownYear.SelectedIndex = d.Year - Now.Year
                Else
                    dropdownMonth.SelectedIndex = Today.Month - 1
                    dropdownYear.SelectedIndex = (Today.Year + 1) - Now.Year
                End If
            End Set
        End Property

#Region "Private Methods"


        Public Sub LoadCreditCardInfo()
            Dim sSQL As Text.StringBuilder = New Text.StringBuilder
            Dim dt As Data.DataTable
            Try
                'Anil B Issue 10254 on 07-03-2013
                'Change the query for credite card load
                sSQL.Append("SELECT PT.ID,CC.Name FROM " & Database & _
                         "..vwPaymentTypes PT inner join " & Database & "..vwCreditCardTypes CC on PT.CreditCardTypeID= CC.ID WHERE PT.Active=1 AND PT.WebEnabled=1 AND " & _
                          "(PT.Type='Credit Card' OR PT.Type='Credit Card Reference Transaction') " & _
                          " UNION SELECT '-1',' ' ORDER BY CC.Name")

                dt = DataAction.GetDataTable(sSQL.ToString)
                cmbCreditCard.DataSource = dt
                cmbCreditCard.DataTextField = "Name"
                cmbCreditCard.DataValueField = "ID"
                cmbCreditCard.DataBind()


            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Function DeleteSPM(ByVal SPMID As Integer) As Boolean
            Try
                Dim oGESPM As AptifyGenericEntityBase
                Dim sLastError As String = ""
                Dim oGE As Aptify.Applications.Persons.PersonsEntity = DirectCast(Me.AptifyApplication.GetEntityObject("Persons", User1.PersonID), Aptify.Applications.Persons.PersonsEntity)
                If oGE IsNot Nothing Then
                    oGESPM = oGE.SubTypes("PersonSavedPaymentMethods").Find("ID", SPMID)
                    oGESPM.Delete()
                    If oGE.Save(False, sLastError) Then
                        Return True
                    End If
                    Return True
                End If
                Return False
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return False
            End Try
        End Function

        Private Sub FillGrid()
            m_oGESPM = New CommonMethods(DataAction, AptifyApplication, User1, Database)
            Dim DT As DataTable = m_oGESPM.LoadSaveSavedPaymentMethods()
            Try
                'suraj Issue 15287, 4/9/13,Filter grid should appear on the page when there are no records available .
                If Not DT Is Nothing AndAlso DT.Rows.Count > 0 Then
                    Me.grdSPM.DataSource = DT
                Else
                    Me.grdSPM.DataSource = DT
                    DT = Nothing
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        'Anil B Issue 10254 on 06/05/2013
        'Updated function create SPM thrugh the subtype of person entity
        Private Function SaveSavedPaymentMethods(ByVal SPMID As Integer) As Boolean
            Dim bAllowGUI As Boolean = True
            Dim sErrorString As String = ""
            Dim RefTranse As String = ""
            Dim RefExpirDate As Date
            Dim bSaveResult As Boolean = True
            Dim oApp As New Aptify.Framework.Application.AptifyApplication
            Dim sLastError As String = ""
            Dim oOrderPayInfo As PaymentInformation
            Try
                Dim oGE As Aptify.Applications.Persons.PersonsEntity = DirectCast(Me.AptifyApplication.GetEntityObject("Persons", User1.PersonID), Aptify.Applications.Persons.PersonsEntity)
                If oGE IsNot Nothing Then
                    With oGE.SubTypes("PersonSavedPaymentMethods").Add()
                        If SPMID = -1 Then
                            .SetValue("PersonID", User1.PersonID)
                            .SetValue("PaymentTypeID", CLng(cmbCreditCard.SelectedItem.Value))
                            .SetValue("CCAccountNumber", txtCCNumber.Text)
                            .SetValue("IsActive", 1)
                            If .Fields("PaymentInformationID").EmbeddedObjectExists Then
                                'Anil B Issue 10254 on 07-03-2013
                                'Add Code to set CCPartial for referal transaction
                                oOrderPayInfo = DirectCast(.Fields("PaymentInformationID").EmbeddedObject, PaymentInformation)
                                If oOrderPayInfo IsNot Nothing Then
                                    oOrderPayInfo.CreditCardSecurityNumber = txtCCSecurityNumber.Text
                                    .SetValue("CCPartial", oOrderPayInfo.GetCCPartial(CStr(txtCCNumber.Text)))
                                End If
                            End If
                        End If
                        .SetValue("Name", txtName.Text)
                        .SetValue("CCExpireDate", CCExpireDate)
                        .SetValue("StartDate", Now)
                        .SetValue("EndDate", CCExpireDate)
                        ''Rashmi P, Commented below code as it is generating two sets of Authorization/Void transactions in PayPal Report.
                        ''Verified not effecting anything in existing functionality.
                        'If .Save(False, sLastError) Then
                        'Anil B change for 10737 on 13/03/2013
                        'Set Reference Transaction Number if payment type is reference transaction for SPM
                        'If .Save(False, sLastError) Then
                        'If CheckForReferenceTransaction(CLng(cmbCreditCard.SelectedItem.Value)) Then
                        '    SetTransactionNumber(oGE, RefTranse, RefExpirDate)
                        '    '.SetValue("ReferenceTransactionNumber", RefTranse)
                        '    '.SetValue("ReferenceExpiration", RefExpirDate)
                        'End If
                        txtName.Text = String.Empty
                        cmbCreditCard.ClearSelection()
                        txtCCNumber.Text = String.Empty
                        txtCCSecurityNumber.Text = String.Empty
                        'End If
                    End With

                    If oGE.Save(False, sLastError) Then
                        Return True
                    End If
                    Return False
                End If
                Return False
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return False
            End Try
        End Function
        'Anil B Issue 10254 on 06/05/2013
        'Create function to update the SPM from Ebusiness site.
        Private Function UpdateSavedPaymentMethods(ByVal SPMID As Integer) As Boolean
            Dim sLastError As String = ""
            Dim RefTranse As String = ""
            Try
                Dim oGE As Aptify.Applications.Persons.PersonsEntity = DirectCast(Me.AptifyApplication.GetEntityObject("Persons", User1.PersonID), Aptify.Applications.Persons.PersonsEntity)
                If oGE IsNot Nothing Then
                    With oGE.SubTypes("PersonSavedPaymentMethods").Find("ID", SPMID)
                        .SetValue("Name", txtName.Text)
                        .SetValue("CCExpireDate", CCExpireDate)
                        If oGE.Save(False, sLastError) Then
                            txtName.Text = String.Empty
                            cmbCreditCard.ClearSelection()
                            txtCCNumber.Text = String.Empty
                            txtCCSecurityNumber.Text = String.Empty
                            Return True
                        End If
                    End With
                End If
                Return False
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return False
            End Try
        End Function

        Protected Overrides Sub SetProperties()
            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME

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
            If String.IsNullOrEmpty(DeleteImage) Then
                DeleteImage = Me.GetLinkValueFromXML(ATTRIBUTE_DELETE_IMG)
            End If
            If String.IsNullOrEmpty(EditImage) Then
                EditImage = Me.GetLinkValueFromXML(ATTRIBUTE_EDIT_IMG)
            End If
        End Sub

#End Region


        Protected Sub btnAddNewCard_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNewCard.Click
            Try

                If grdSPM.Items.Count = 1 Then
                    If CType(grdSPM.Items(0).FindControl("lblPaymentType"), Label).Text = String.Empty AndAlso CType(grdSPM.Items(0).FindControl("lblCCPartial"), Label).Text = String.Empty Then
                        grdSPM.Visible = False
                    End If
                End If


                txtName.Text = ""
                'Anil B Issue 10254 on 20/04/2013
                'cleare sequarity number
                txtCCSecurityNumber.Text = ""
                txtCCNumber.Text = ""
                lblError.Text = ""
                btnAdd.Text = "Add your Card"
                ImgVisa.ImageUrl = VisaDisabled
                ImgMasterCard.ImageUrl = MasterCardDisabled
                ImgAmericanExpress.ImageUrl = AmericanExpressDisabled
                ImgDiscover.ImageUrl = DiscoverDisabled
                CreditcardWindow.VisibleOnPageLoad = True
                txtCCNumber.Enabled = True
                'Anil B Issue 10254 on 20/04/2013
                'Reset date for SPM
                dropdownMonth.SelectedIndex = Today.Month - 1
                dropdownYear.SelectedIndex = (Today.Year + 1) - Now.Year
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try

        End Sub

        Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

            lblError.Text = ""
            CreditcardWindow.VisibleOnPageLoad = False

        End Sub

        Protected Sub grdSPM_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grdSPM.ItemCommand
            Try
                'AddExpressionSavePayment()
                CreditcardWindow.VisibleOnPageLoad = False
                Dim lPaymentTypeID As Long

                Dim iLine As Integer = e.Item.ItemIndex

                If Convert.ToString(e.CommandName) = "Delete" Then
                    If grdSPM.Items(iLine).FindControl("lblID") IsNot Nothing Then
                        Dim lblID As Label = TryCast(grdSPM.Items(iLine).FindControl("lblID"), Label)

                        If CInt(lblID.Text) <> 0 Then
                            If DeleteSPM(CInt(lblID.Text)) Then
                                grdSPM.DataSource = m_oGESPM.LoadSaveSavedPaymentMethods
                                grdSPM.DataBind()
                            End If
                        End If
                    End If
                End If
                If Convert.ToString(e.CommandName) = "Update" Then
                    'Anil B Issue 10254 on 20/04/2013
                    'cleare sequarity number
                    txtCCSecurityNumber.Text = ""
                    If grdSPM.Items(iLine).FindControl("lblID") IsNot Nothing Then
                        Dim lblID As Label = TryCast(grdSPM.Items(iLine).FindControl("lblID"), Label)
                        If CInt(lblID.Text) <> 0 Then
                            SPMID.Value = lblID.Text
                            txtName.Text = CType(grdSPM.Items(iLine).FindControl("lblNameonCard"), Label).Text
                            Dim oGESPM As Aptify.Framework.BusinessLogic.GenericEntity.AptifyGenericEntityBase
                            oGESPM = Me.AptifyApplication.GetEntityObject("PersonSavedPaymentMethods", CInt(SPMID.Value))
                            If Not oGESPM Is Nothing Then
                                lPaymentTypeID = CLng(oGESPM.GetValue("PaymentTypeID"))
                                cmbCreditCard.SelectedValue = CStr(lPaymentTypeID)
                                'Anil B change for 10737 on 13/03/2013
                                'Set CCPrtial Number to Card Number

                                txtCCNumber.Text = CStr(oGESPM.GetValue("CCPartial"))
                                If Not IsDBNull(oGESPM.GetValue("CCExpireDate")) Then
                                    Dim sExpireDate As Date = CDate(oGESPM.GetValue("CCExpireDate"))
                                    dropdownMonth.SelectedValue = CStr(sExpireDate.Month)
                                    dropdownYear.SelectedValue = CStr(sExpireDate.Year)
                                End If
                                SelectCardType(cmbCreditCard.SelectedItem.Text.Trim)
                            End If
                            btnAdd.Text = "Update"
                            CreditcardWindow.VisibleOnPageLoad = True
                            RequiredFieldValidator1.ErrorMessage = ""
                            RequiredFieldValidator3.ErrorMessage = ""
                            'Anil B change for 10737 on 13/03/2013
                            'Set error visibility
                            lblError.Visible = False
                            txtCCNumber.Enabled = False
                            'Anil B Issue 15409 on 08-04-2013
                            'If Payment type is reference transaction then desabled the date control
                            If CheckForReferenceTransaction(lPaymentTypeID) Then
                                SetDateDesabled(False)
                            End If
                        End If
                    End If
                End If


            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try

        End Sub

        Protected Sub grdSPM_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdSPM.ItemDataBound
            Try
                Dim lblDate As Label = CType(e.Item.FindControl("lblExpireOnDate"), Label)
                If Not IsDBNull(DataBinder.Eval(e.Item.DataItem, "ExpireOn")) Then
                    Dim strEndDate As String = CStr(DataBinder.Eval(e.Item.DataItem, "ExpireOn"))
                    If strEndDate <> "" AndAlso strEndDate.Contains("12:00AM") Then
                        lblDate.Text = strEndDate.Substring(0, strEndDate.Length - 7)
                    End If
                End If
                Dim imgDelete As ImageButton = CType(e.Item.FindControl("imgDelete"), ImageButton)
                imgDelete.ImageUrl = DeleteImage
                Dim imgEdit As ImageButton = CType(e.Item.FindControl("ImgEdit"), ImageButton)
                imgEdit.ImageUrl = EditImage
            Catch ex As Exception

            End Try
        End Sub

        Protected Sub grdSPM_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdSPM.NeedDataSource
            m_oGESPM = New CommonMethods(DataAction, AptifyApplication, User1, Database)
            Try

                grdSPM.DataSource = m_oGESPM.LoadSaveSavedPaymentMethods()

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub



        Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
            Try

                AddExpressionSavePayment()
                Dim bSuccess As Boolean
                'Anil B Issue 10254 on 20/04/2013
                If bIsValidCard Then
                    bIsValidCard = False
                    'Anil B Issue 10254 on 06/05/2013
                    'Create and update SPM
                    'Avoid to add record on page refresh
                    If Session("IsPageRefresh").ToString() = ViewState("IsPageRefresh").ToString() Then
                        If btnAdd.Text = "Update" Then
                            If UpdateSavedPaymentMethods(CInt(SPMID.Value)) Then
                                bSuccess = True
                                CreditcardWindow.VisibleOnPageLoad = False
                            End If
                        Else
                            If SaveSavedPaymentMethods(-1) Then
                                bSuccess = True
                                CreditcardWindow.VisibleOnPageLoad = False
                            End If
                        End If
                        If bSuccess Then
                            grdSPM.DataSource = m_oGESPM.LoadSaveSavedPaymentMethods
                            grdSPM.DataBind()
                            grdSPM.Visible = True
                        End If
                        Session("IsPageRefresh") = Server.UrlDecode(System.DateTime.Now.ToString())
                    End If
                    'Anil B for issue 10254 on 29-03-13
                    'set the credit card pop up box
                    CreditcardWindow.VisibleOnPageLoad = False
                    'Anil B Issue 10254 on 20/04/2013
                    'cleare sequiarity number
                    txtCCSecurityNumber.Text = ""
                    m_oGESPM = New CommonMethods(DataAction, AptifyApplication, User1, Database)
                    Dim DT As DataTable = m_oGESPM.LoadSaveSavedPaymentMethods()
                    If DT IsNot Nothing AndAlso DT.Rows.Count > 0 Then
                        grdSPM.DataSource = DT
                        grdSPM.DataBind()
                    End If
                    DT = Nothing
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try

        End Sub


        Private Sub vldExpirationDate_ServerValidate(ByVal source As System.Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles vldExpirationDate.ServerValidate
            args.IsValid = IsDate(CCExpireDate)
        End Sub


        Protected Sub txtCCNumber_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCCNumber.TextChanged
            Dim m_oPaymentInfo As Aptify.Applications.Accounting.PaymentInformation = Nothing
            Try

                Dim lPaymentID As Long
                Dim lCreditCard As Long
                lblError.Text = ""
                m_oPaymentInfo = DirectCast(ShoppingCart1.GetOrderObject(Session, Page.User, Application).Fields("PaymentInformationID").EmbeddedObject, PaymentInformation)
                If txtCCNumber.Text = "" Then
                    cmbCreditCard.SelectedValue = "-1"
                Else
                    'Anil B Issue 16167 on 07-05-2013
                    'Add Condition to check active payment card.
                    If m_oPaymentInfo.ValidateRange(Convert.ToString(txtCCNumber.Text), lPaymentID, lCreditCard) Then
                        'Anil B Issue 10254 on 07-03-2013
                        'Add Condition to check active payment card.
                        If lPaymentID > 0 AndAlso lCreditCard > 0 Then
                            cmbCreditCard.SelectedValue = CStr(lPaymentID)
                            SelectCardType(cmbCreditCard.SelectedItem.Text.Trim)
                            lblError.Text = ""
                            cmbCreditCard.Enabled = False
                            bIsValidCard = True
                        Else
                            bIsValidCard = False
                            cmbCreditCard.SelectedValue = "-1"
                            'Anil B Issue 10254 on 07-03-2013
                            'Change the message for Inactive card type
                            lblError.Text = "Invalid Card Type. Please try entering your card number again."
                            lblError.Visible = True
                            'Anil B Issue 10254 on 20/04/2013
                            'Desabled the credite card
                            SetCardDesabled()
                            cmbCreditCard.Enabled = True
                        End If
                    Else
                        cmbCreditCard.SelectedValue = "-1"
                        lblError.Text = "Card number does not match card type."
                        lblError.Visible = True
                        'Anil B Issue 10254 on 20/04/2013
                        'Desabled the credite card
                        SetCardDesabled()
                        cmbCreditCard.Enabled = True
                        bIsValidCard = False
                    End If

                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Private Sub SelectCardType(ByVal PaymentType As String)

            ImgVisa.ImageUrl = VisaDisabled
            ImgMasterCard.ImageUrl = MasterCardDisabled
            ImgAmericanExpress.ImageUrl = AmericanExpressDisabled
            ImgDiscover.ImageUrl = DiscoverDisabled

            Select Case (PaymentType)
                Case "Visa"
                    ImgVisa.ImageUrl = VisaEnabled
                    ImgVisa.DataBind()
                    'Anil B for issue 16167 Change card name
                Case "MasterCard"
                    ImgMasterCard.ImageUrl = MasterCardEnabled
                    ImgMasterCard.DataBind()
                Case "American Express"
                    ImgAmericanExpress.ImageUrl = AmericanExpressEnabled
                    ImgAmericanExpress.DataBind()
                Case "Discover"
                    ImgDiscover.ImageUrl = DiscoverEnabled
                    ImgDiscover.DataBind()
                Case Else
                    ImgVisa.ImageUrl = VisaDisabled
                    ImgMasterCard.ImageUrl = MasterCardDisabled
                    ImgAmericanExpress.ImageUrl = AmericanExpressDisabled
                    ImgDiscover.ImageUrl = DiscoverDisabled
            End Select

        End Sub

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            ViewState("IsPageRefresh") = Session("IsPageRefresh")
        End Sub
        'Anil B Issue 10254 on 07-03-2013
        'Add Function to create ZDA  
        Private Sub SetTransactionNumber(ByVal oGESPM As Aptify.Applications.Persons.PersonsEntity, ByRef RefTranse As String, ByRef RefExpirDate As Date)
            Dim bAllowGUI As Boolean = True
            Dim sErrorString As String = ""
            Dim ReferenceTransactionNumber As String = ""
            Dim ReferenceExpiration As Date
            Dim bSaveResult As Boolean = True
            Dim oApp As New Aptify.Framework.Application.AptifyApplication
            If PaymentsEntity.GetZDAReferenceTransactionDetails(oApp, _
                                                                 bAllowGUI, _
                                                                 sErrorString, _
                                                                 " ", _
                                                                 CLng(cmbCreditCard.SelectedItem.Value), _
                                                                 User1.PersonID, _
                                                                 User1.CompanyID, _
                                                                 CLng(oGESPM.GetValue("PaymentInformationID")), _
                                                                 txtCCNumber.Text, _
                                                                 CDate(oGESPM.GetValue("CCExpireDate")), _
                                                                 Convert.ToString(oGESPM.GetValue("CCPartial")), _
                                                                 ReferenceTransactionNumber, _
                                                                 ReferenceExpiration, _
                                                                 txtCCSecurityNumber.Text) Then
            End If
            'Anil B Issue 10254 on 06/05/2013
            'Return reference transaction no. and reference expiration date
            RefTranse = ReferenceTransactionNumber
            ReferenceExpiration = RefExpirDate
        End Sub
        'Suraj Issue 14450 3/22/13 ,if the grid load first time By default the sorting will be Ascending for column Forum 
        Private Sub AddExpressionSavePayment()
            Dim expression1 As New GridSortExpression
            expression1.FieldName = "PaymentType"
            expression1.SetSortOrder("Ascending")
            grdSPM.MasterTableView.SortExpressions.AddSortExpression(expression1)
        End Sub
        'Anil B Issue 15409 on 08-04-2013
        'Check weather Payment type is reference transaction or not
        Private Function CheckForReferenceTransaction(ByRef lPaymentTypeID As Long) As Boolean
            Try
                Dim sSQL, sPaymentType As String
                sSQL = "SELECT Type FROM  " & AptifyApplication.GetEntityBaseDatabase("Payment Types") & ".." & AptifyApplication.GetEntityBaseView("Payment Types") & " WHERE  ID = " & lPaymentTypeID
                sPaymentType = CStr(DataAction.ExecuteScalar(sSQL))
                If sPaymentType = "Credit Card Reference Transaction" Then
                    Return True
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
            Return False
        End Function
        'Anil B Issue 15409 on 08-04-2013
        'Enabled Desabled date control
        Private Sub SetDateDesabled(ByVal bflag As Boolean)
            dropdownMonth.Enabled = bflag
            dropdownDay.Enabled = bflag
            dropdownYear.Enabled = bflag
        End Sub
        'Anil B Issue 15409 on 08-04-2013
        'Desabled credite card control
        Private Sub SetCardDesabled()
            ImgVisa.ImageUrl = VisaDisabled
            ImgMasterCard.ImageUrl = MasterCardDisabled
            ImgAmericanExpress.ImageUrl = AmericanExpressDisabled
            ImgDiscover.ImageUrl = DiscoverDisabled
        End Sub


    End Class
End Namespace


