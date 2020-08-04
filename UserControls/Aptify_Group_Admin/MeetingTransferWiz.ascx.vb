'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports System.Data
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.Application
Imports Aptify.Applications.OrderEntry
Imports Aptify.Framework.BusinessLogic.ProcessPipeline
Imports Telerik.Web.UI
Imports System.Collections.Generic
Imports Aptify.Applications.Accounting

Namespace Aptify.Framework.Web.eBusiness
    ''' <summary>
    ''' RashmiP, 14335, Meeting Transfer
    ''' </summary>
    ''' <remarks></remarks>
    Partial Class MeetingTransferWiz
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "MeetingTransferWiz"
        Protected Const ATTRIBUTE_COMPANY_LOGO_IMAGE_URL As String = "GACompanyLogo"
        Protected Const ATTRIBUTE_BLANK_IMG_URL As String = "RadBlankImage"
        Protected Const ATTRIBUTE_COMPANY_ADDRESS As String = "CompanyAddress"
        Protected Const ATTRIBUTE_DT_MEETING_ORDER As String = "dtMeetingsOrders"
        Protected Const ATTRIBUTE_DT_MEETING_REGISTRATION As String = "dtMeetingRegistrants"
        Protected Const ATTRIBUTE_DT_NEW_MEETING As String = "dtNewMeetingGrid"
        Protected Const ATTRIBUTE_PAID_AMOUNT As String = "PaidAmount"
        Protected Const ATTRIBUTE_PRICE_DIFFERENCE As String = "PriceDifference"
        Protected Const ATTRIBUTE_NEW_ORDERID As String = "NewOrderID"
        Protected Const ATTRIBUTE_TRANSFER_FEE As String = "TransferFees"
        Protected Const ATTRIBUTE_TRANSFER_FEE_PRODUCT_ID As String = "TransferFeeProductID"
        Protected Const ATTRIBUTE_PREVIOUS_MEETING As String = "Previous_Meeting"
        Protected Const ATTRIBUTE_ATTENDEE_ID As String = "AttendeeID"
        Protected Const ATTRIBUTE_NEW_MEETING As String = "New_Meeting"
        'Neha Issue 14810,03/13/13, Declared properties for RadBinaryimage
        Protected Const ATTRIBUTE_PROFILE_THUMBNAIL_WIDTH As String = "ProfileThumbNailWidth"
        Protected Const ATTRIBUTE_PROFILE_THUMBNAIL_HEIGHT As String = "ProfileThumbNailHeight"
        Dim CurrencyCache As CurrencyTypeCache
        Dim m_oWizObject As ScheduledMeetingTransferWizObject
        Protected Const ATTRIBUTE_LOGIN_PAGE As String = "LoginPage" 'Added by Sandeep for Issue 15051 on 12/03/2013
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
            InitializeProperties()
            SetProperties()
            If Not IsPostBack Then
                AddExpression()
                FillMeetingsOrders()
                CreditCard.LoadCreditCardInfo()
                ShowBillMeLater()
                chkMakePayment.Checked = False
                tblTransferConfirmation.Visible = False
                CreditcardWindow.VisibleOnPageLoad = True
            End If
            CreditCard.SetchkSaveforFutureUse = False
            upnlMeetingRegistrant.Update()
            upnlNewMeetings.Update()
            upnlUpcomingMeeting.Update()
            If User1.UserID < 0 Then
                Response.Redirect(LoginPage) 'Added by Sandeep for Issue 15051 on 12/03/2013
            End If

        End Sub

#Region "Property"
        Public Overridable ReadOnly Property TransferFeeProductID() As String
            Get
                If Not ViewState.Item(ATTRIBUTE_TRANSFER_FEE_PRODUCT_ID) Is Nothing Then
                    Return CStr(ViewState.Item(ATTRIBUTE_TRANSFER_FEE_PRODUCT_ID))
                Else
                    Dim value As String = Me.GetGlobalAttributeValue(ATTRIBUTE_TRANSFER_FEE_PRODUCT_ID)
                    If Not String.IsNullOrEmpty(value) Then
                        ViewState.Item(ATTRIBUTE_TRANSFER_FEE_PRODUCT_ID) = value
                    End If
                    Return value
                End If
            End Get

        End Property
        Public Overridable ReadOnly Property TransferFees() As String
            Get
                If Not ViewState.Item(ATTRIBUTE_TRANSFER_FEE) Is Nothing Then
                    Return CStr(ViewState.Item(ATTRIBUTE_TRANSFER_FEE))
                Else
                    Dim value As String = Me.GetGlobalAttributeValue(ATTRIBUTE_TRANSFER_FEE)
                    If Not String.IsNullOrEmpty(value) Then
                        ViewState.Item(ATTRIBUTE_TRANSFER_FEE) = value
                    End If
                    Return value
                End If
            End Get

        End Property

        Public Overridable Property GACompanyLogo() As String
            Get
                If Not ViewState(ATTRIBUTE_COMPANY_LOGO_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_COMPANY_LOGO_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_COMPANY_LOGO_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        Public Overridable Property RadBlankImage() As String
            Get
                If Not ViewState(ATTRIBUTE_BLANK_IMG_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_BLANK_IMG_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_BLANK_IMG_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        Public Overridable Property CompanyAddress() As String
            Get
                If Not ViewState(ATTRIBUTE_COMPANY_ADDRESS) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_COMPANY_ADDRESS))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_COMPANY_ADDRESS) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        'Neha, Issue 14810, 03/13/13, Overrided properties for Rdabinaryimage
        Public Overridable ReadOnly Property ProfileThumbNailWidth() As Integer
            Get
                Try
                    If Not String.IsNullOrEmpty(Me.GetGlobalAttributeValue(ATTRIBUTE_PROFILE_THUMBNAIL_WIDTH)) Then
                        If Not IsNumeric(Me.GetGlobalAttributeValue(ATTRIBUTE_PROFILE_THUMBNAIL_WIDTH)) Then
                            Throw New Exception("Incorrect entry for <Global>...<" & ATTRIBUTE_PROFILE_THUMBNAIL_WIDTH & ">, a numeric value is required. " & _
                                                "Please confirm the entry is correctly input in the 'Aptify_UC_Navigation.config' file.")
                        Else
                            Return CInt(Me.GetGlobalAttributeValue(ATTRIBUTE_PROFILE_THUMBNAIL_WIDTH))
                        End If
                    Else
                        Return 0
                    End If
                Catch ex As Exception
                    Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                    Return 0
                End Try
            End Get
        End Property
        Public Overridable ReadOnly Property ProfileThumbNailHeight() As Integer
            Get
                Try
                    If Not String.IsNullOrEmpty(Me.GetGlobalAttributeValue(ATTRIBUTE_PROFILE_THUMBNAIL_HEIGHT)) Then
                        If Not IsNumeric(Me.GetGlobalAttributeValue(ATTRIBUTE_PROFILE_THUMBNAIL_HEIGHT)) Then
                            Throw New Exception("Incorrect entry for <Global>...<" & ATTRIBUTE_PROFILE_THUMBNAIL_HEIGHT & ">, a numeric value is required. " & _
                                                "Please confirm the entry is correctly input in the 'Aptify_UC_Navigation.config' file.")
                        Else
                            Return CInt(Me.GetGlobalAttributeValue(ATTRIBUTE_PROFILE_THUMBNAIL_HEIGHT))
                        End If
                    Else
                        Return 0
                    End If
                Catch ex As Exception
                    Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                    Return 0
                End Try
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
#End Region

#Region "Private Methhods"
        Private Sub InitializeProperties()
            Try
                lblError.Text = ""

            Catch ex As Exception

            End Try
        End Sub
        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME

            MyBase.SetProperties()

            If String.IsNullOrEmpty(GACompanyLogo) Then
                GACompanyLogo = Me.GetLinkValueFromXML(ATTRIBUTE_COMPANY_LOGO_IMAGE_URL)
            End If
            If Not String.IsNullOrEmpty(GACompanyLogo) Then
                Me.companyLogo.ImageUrl = GACompanyLogo
            End If
            If String.IsNullOrEmpty(RadBlankImage) Then
                RadBlankImage = Me.GetLinkValueFromXML(ATTRIBUTE_BLANK_IMG_URL)
            End If
            If String.IsNullOrEmpty(CompanyAddress) Then
                Dim strVirtualPath As String = Request.ApplicationPath.ToString & "/"
                CompanyAddress = Me.GetLinkValueFromXML(ATTRIBUTE_COMPANY_ADDRESS)
                Me.lblcompanyAddress.Text = CompanyAddress.Substring(strVirtualPath.Length, CompanyAddress.Length - strVirtualPath.Length)
            End If
            'Added by Sandeep for Issue 15051 on 12/03/2013
            If String.IsNullOrEmpty(LoginPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                LoginPage = Me.GetLinkValueFromXML(ATTRIBUTE_LOGIN_PAGE)
            End If
        End Sub
        Private Sub FillMeetingsOrders()
            Try
                Dim spGetMeetingOrders As String = "spGetDistinctMeetingOrders"
                Dim SQL As String
                Dim params(0) As IDataParameter
                Dim DT As Data.DataTable
                Dim NextButton As Button = CType(WizardMeetingTransfer.FindControl("StepNavigationTemplateContainerID").FindControl("StepNextButton"), Button)
                SQL = Database & ".." & spGetMeetingOrders

                params(0) = Me.DataAction.GetDataParameter("@CompanyID", SqlDbType.Int, User1.CompanyID)

                DT = Me.DataAction.GetDataTableParametrized(SQL, CommandType.StoredProcedure, params)

                If DT IsNot Nothing AndAlso DT.Rows.Count > 0 Then
                    grdUpcomingMeeting.DataSource = DT
                    grdUpcomingMeeting.DataBind()
                    lblStep1.Text = "Step 1: Select a Meeting/Session"
                    lblStep1Msg.Text = "Meetings/Sessions Registrations"
                    NextButton.Enabled = True
                    WizardMeetingTransfer.Visible = True
                    If DT.Rows.Count = 1 Then
                        Dim opt As RadioButton = CType(grdUpcomingMeeting.Items(0).FindControl("optSelectMeeting"), RadioButton)
                        opt.Checked = True
                    End If
                    ViewState(ATTRIBUTE_DT_MEETING_ORDER) = DT
                Else
                    lblStep1.Text = "No Upcoming/Ongoing Meeting Available."
                    lblStep1Msg.Text = ""
                    NextButton.Enabled = False
                    grdUpcomingMeeting.DataSource = DT
                    grdUpcomingMeeting.DataBind()
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub FillNewMeetingGrid()
            Try
                AddExpression()
                Dim MeetingID As Long = 0
                Dim rowIndex As Integer
                Dim spGetTransMeetingList As String = "spGetTransMeetingList"
                Dim sSQL As String
                Dim dt, dtSelectedItem As DataTable
                Dim params(0) As IDataParameter
                Dim NextButton As Button = CType(WizardMeetingTransfer.FindControl("StepNavigationTemplateContainerID").FindControl("StepNextButton"), Button)

                sSQL = Database & ".." & spGetTransMeetingList
                If ViewState(ATTRIBUTE_PREVIOUS_MEETING) IsNot Nothing Then
                    dtSelectedItem = DirectCast(ViewState(ATTRIBUTE_PREVIOUS_MEETING), DataTable)
                End If
                If dtSelectedItem IsNot Nothing AndAlso dtSelectedItem.Rows.Count > 0 Then
                    MeetingID = CLng(dtSelectedItem.Rows(0).Item("MeetingID"))
                End If

                params(0) = Me.DataAction.GetDataParameter("@MeetingID", SqlDbType.Int, MeetingID)

                dt = Me.DataAction.GetDataTableParametrized(sSQL, CommandType.StoredProcedure, params)

                If dt.Rows.Count > 0 Then

                    grdNewMeetings.DataSource = dt
                    grdNewMeetings.DataBind()
                    Label1.Text = "Step 3: Select a Meeting/Session"
                    Label2.Visible = True
                    NextButton.Enabled = True
                    ViewState(ATTRIBUTE_DT_NEW_MEETING) = dt
                Else
                    grdNewMeetings.DataSource = dt
                    grdNewMeetings.DataBind()
                    Label1.Text = "No Upcoming/Ongoing Meeting Available"
                    Label2.Visible = False
                    NextButton.Enabled = False
                End If
                dtSelectedItem = Nothing
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub


        Private Function FindMeeting(ByVal MeetingID As Integer) As String

            Dim sSQL As String

            Try
                sSQL = "Select Case When P.WebName ='' Then P.Name  Else P.WebName End As MEETING " & _
                      " FROM " & Database & "..VWMEETINGS M Inner join vwProducts P on P.ID = M.ProductID " & _
                " WHERE M.PRODUCTID = " & MeetingID
                Return CStr(DataAction.ExecuteScalar(sSQL))


            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try

        End Function

        Private Function FindPersonByID(ByVal ID As Int32) As String
            Try
                Dim SQL As String
                Dim params(0) As IDataParameter
                Dim DT As Data.DataTable

                SQL = Database & ".." & "spFindPersonByID"
                If ID > 0 Then
                    params(0) = Me.DataAction.GetDataParameter("@ID", SqlDbType.BigInt, ID)
                End If

                DT = Me.DataAction.GetDataTableParametrized(SQL, CommandType.StoredProcedure, params)
                If DT IsNot Nothing AndAlso DT.Rows.Count > 0 Then
                    Return CStr(DT.Rows(0).Item("Name"))
                End If
                DT = Nothing
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Function

        Private Sub FillMeetingRegistrants(ByVal MeetingID As Integer)
            Try
                AddExpression()
                Dim SQL As String
                Dim params(1) As IDataParameter
                Dim DT As Data.DataTable
                Dim NextButton As Button = CType(WizardMeetingTransfer.FindControl("StepNavigationTemplateContainerID").FindControl("StepNextButton"), Button)

                SQL = Database & ".." & "spMeetingRegistrantForMeetingTran"

                params(0) = Me.DataAction.GetDataParameter("@CompanyID", SqlDbType.Int, User1.CompanyID)
                params(1) = Me.DataAction.GetDataParameter("@MeetingID", SqlDbType.Int, MeetingID)

                DT = Me.DataAction.GetDataTableParametrized(SQL, CommandType.StoredProcedure, params)
                If DT IsNot Nothing AndAlso DT.Rows.Count > 0 Then
                    grdMeetingRegistrant.DataSource = DT
                    grdMeetingRegistrant.DataBind()
                    lblStep2.Text = "Step 2: Select an Attendee"
                    NextButton.Enabled = True
                    lblMeetingTitle.Visible = True
                    ViewState(ATTRIBUTE_DT_MEETING_REGISTRATION) = DT
                Else
                    grdMeetingRegistrant.DataSource = DT
                    grdMeetingRegistrant.DataBind()
                    lblStep2.Text = "No Attendee Available for Selected Meeting."
                    NextButton.Enabled = False
                    lblMeetingTitle.Visible = False
                End If
                If grdMeetingRegistrant IsNot Nothing AndAlso grdMeetingRegistrant.Items.Count = 1 Then
                    Dim opt As RadioButton = CType(grdMeetingRegistrant.Items(0).FindControl("optSelectAttendee"), RadioButton)
                    opt.Checked = True
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try

        End Sub
        Private Function FinishTransfer() As Boolean
            Try

                Dim oWizObject As ScheduledMeetingTransferWizObject = Nothing
                Dim bSuccess As Boolean
                Dim lPricedifference As Decimal = 0
                Dim sError As String = ""

                oWizObject = SetTransferWizardObject()

                If oWizObject IsNot Nothing Then
                    lPricedifference = oWizObject.TransferOrderCreditAmount
                    bSuccess = oWizObject.TransferOrder()
                End If
                If bSuccess Then
                    ViewState(ATTRIBUTE_NEW_ORDERID) = CStr(oWizObject.TransferOrderGE.ID)
                    If chkMakePayment.Checked Then
                        If CLng(oWizObject.TransferOrderGE.ID) > 0 Then
                            PostPayment(oWizObject.TransferOrderGE.ID, lPricedifference)
                        Else
                            PostPayment(oWizObject.OriginalOrderID, lPricedifference)
                        End If
                    End If
                Else
                    lblError.Text = oWizObject.LastError
                    If oWizObject.LastError.Contains("There must be at least one order line per order.") Then
                        lblError.Text = "Meeting is not available to transfer."
                    End If
                End If

                Return bSuccess
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return False
            End Try
        End Function

        ''Funtion returns ScheduledMeetingTransferWiz Object
        Protected Overridable Function SetTransferWizardObject() As ScheduledMeetingTransferWizObject
            Dim OrigOrderGE As OrdersEntity = Nothing
            Dim IsSession As Boolean
            Dim lParentID As Long
            Dim lPricedifference As Long = 0
            Dim sError As String = ""
            Dim rowIndex As Integer = 0
            Dim dtSelectedItem As DataTable = Nothing
            Dim AttendeeID As Long = 0
            Dim OriOrderID As Long = 0
            Dim StatusID As Integer = 0
            Dim OriOrderLineID As Long = 0

            If m_oWizObject Is Nothing Then
                m_oWizObject = New ScheduledMeetingTransferWizObject(Me.AptifyApplication)
                With m_oWizObject

                    Dim MeetingID As Long
                    If ViewState(ATTRIBUTE_PREVIOUS_MEETING) IsNot Nothing Then
                        dtSelectedItem = DirectCast(ViewState(ATTRIBUTE_PREVIOUS_MEETING), DataTable)
                    End If
                    If dtSelectedItem IsNot Nothing AndAlso dtSelectedItem.Rows.Count > 0 Then
                        MeetingID = CLng(dtSelectedItem.Rows(0).Item("MeetingID"))
                        IsSession = CBool(dtSelectedItem.Rows(0).Item("IsSession"))
                        If IsSession Then
                            lParentID = CLng(dtSelectedItem.Rows(0).Item("ParentID"))
                        End If
                    End If

                    dtSelectedItem = Nothing


                    If ViewState(ATTRIBUTE_ATTENDEE_ID) IsNot Nothing Then
                        dtSelectedItem = DirectCast(ViewState(ATTRIBUTE_ATTENDEE_ID), DataTable)
                    End If
                    If dtSelectedItem IsNot Nothing AndAlso dtSelectedItem.Rows.Count > 0 Then
                        AttendeeID = CLng(dtSelectedItem.Rows(0).Item("AttendeeID"))
                        OriOrderID = CLng(dtSelectedItem.Rows(0).Item("OriOrderID"))
                        StatusID = CInt(dtSelectedItem.Rows(0).Item("StatusID"))
                        OriOrderLineID = CLng(dtSelectedItem.Rows(0).Item("OriOrderLineID"))
                        OrigOrderGE = (CType(AptifyApplication.GetEntityObject("Orders", OriOrderID), OrdersEntity))
                    End If


                    Dim NewMeetingID As Long
                    If ViewState(ATTRIBUTE_NEW_MEETING) IsNot Nothing AndAlso CInt(ViewState(ATTRIBUTE_NEW_MEETING)) > 0 Then
                        NewMeetingID = CInt(ViewState(ATTRIBUTE_NEW_MEETING))
                    End If

                    .OriginalAttendeeID = AttendeeID
                    .OriginalMeetingID = MeetingID
                    .FirstAttendeeID = AttendeeID
                    .FirstMeetingID = NewMeetingID
                    .OriginalOrderID = OrigOrderGE.ID
                    .NewBillToCompanyID = CInt(OrigOrderGE.BillToCompanyID)
                    .NewShipToCompanyID = CInt(OrigOrderGE.ShipToCompanyID)
                    .NewBillToID = CInt(OrigOrderGE.BillToID)
                    .NewShipToID = CInt(OrigOrderGE.ShipToID)
                    .EmployeeID = EBusinessGlobal.WebEmployeeID(Page.Application)
                    .LoadOrderInfo()
                    .SuppressEmailConfirmations = True

                    If Not String.IsNullOrEmpty(TransferFees) AndAlso Not String.IsNullOrEmpty(TransferFeeProductID) Then
                        If CDec(TransferFees) > 0 Then
                            .CancellationFeeProductID = CLng(TransferFeeProductID)
                            .CancellationFee = CDec(TransferFees)
                        Else
                            .CancellationFeeProductID = 0
                            .CancellationFee = 0
                        End If
                    End If

                    If OrigOrderGE.OrderStatus = OrderStatus.Shipped Then
                        .SetQuantityCancelled(OriOrderLineID, 1)
                        .CreateCancellationOrder = True
                        If OrigOrderGE.CALC_GrandTotal >= 0 Then
                            .IsOnAccount = True
                        End If
                    Else
                        .CreateCancellationOrder = False
                    End If

                    For Each oLine As MeetingOrderLineInfo In .OrderLineData.Values
                        If oLine.OrderLineID = OriOrderLineID Then
                            oLine.IsSelected = True
                            oLine.AttendeeID = AttendeeID
                            oLine.NewAttendeeID = AttendeeID
                            oLine.ProductID = MeetingID
                            oLine.NewMeetingID = NewMeetingID
                            oLine.StatusID = StatusID
                            oLine.IsSession = IsSession
                            If IsSession Then
                                oLine.ParentProductID = lParentID
                            End If

                        End If

                    Next

                    .ConfigureTransferOrder()
                End With
            End If
            dtSelectedItem = Nothing
            Return m_oWizObject
        End Function

        Private Sub PostPayment(ByVal OrderID As Long, ByVal PayAmount As Decimal)
            ' post the payment to the database using the CGI GE
            Dim oPayment As AptifyGenericEntityBase
            Try
                oPayment = AptifyApplication.GetEntityObject("Payments", -1)
                oPayment.SetValue("EmployeeID", EBusinessGlobal.WebEmployeeID(Page.Application))
                oPayment.SetValue("PersonID", User1.PersonID)
                oPayment.SetValue("CompanyID", User1.CompanyID)
                oPayment.SetValue("PaymentDate", Date.Today)
                oPayment.SetValue("DepositDate", Date.Today)
                oPayment.SetValue("EffectiveDate", Date.Today)

                If CreditCard.BillMeLaterChecked Then
                    oPayment.SetValue("PaymentTypeID", CreditCard.PaymentTypeID)
                    oPayment.SetValue("PONumber", CreditCard.PONumber)
                    ViewState(ATTRIBUTE_PAID_AMOUNT) = Nothing
                Else
                    oPayment.SetValue("PaymentTypeID", CreditCard.PaymentTypeID)
                    oPayment.SetValue("CCAccountNumber", CreditCard.CCNumber)
                    oPayment.SetValue("CCExpireDate", CreditCard.CCExpireDate)
                    'oPayment.SetValue("CCSecurityNumber", CreditCard.CCSecurityNumber)
                    oPayment.SetAddValue("_xCCSecurityNumber", CreditCard.CCSecurityNumber) ' Neha changes for issue 16675, 06/07/2013
                    'Anil B change for 10254 on 23/04/2013
                    'Set reference transaction for payment
                    If CreditCard.CCNumber = "-Ref Transaction-" Then
                        oPayment.SetValue("ReferenceTransactionNumber", CreditCard.ReferenceTransactionNumber)
                        oPayment.SetValue("ReferenceExpiration", CreditCard.ReferenceExpiration)
                    End If
                    oPayment.SetValue("PaymentLevelID", User1.GetValue("GLPaymentLevelID"))
                    oPayment.SetValue("Comments", "Created through the CGI e-Business Suite")
                    'Neha,Issue 16675, set temperory variable for  Accounting->paymentinformation tab record history (security number),06/25/2013
                    oPayment.SetAddValue("_xCCSecurityNumber", CreditCard.CCSecurityNumber)
                    Dim oOrderPayInfo As PaymentInformation = DirectCast(oPayment.Fields("PaymentInformationID").EmbeddedObject, PaymentInformation)
                    oOrderPayInfo.SetAddValue("_xCCSecurityNumber", CreditCard.CCSecurityNumber)
                    oPayment.SetAddValue("_xConvertQuotesToRegularOrder", "1")
                End If

                With oPayment.SubTypes("PaymentLines").Add
                    .SetValue("Amount", PayAmount)
                    .SetValue("OrderID", OrderID)
                End With

                oPayment.Save(False)
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)

            End Try
        End Sub

        Private Sub ValidateforBalance()
            Try

                CreditcardWindow.VisibleOnPageLoad = False
                chkMakePayment.Checked = False
                Dim oWizObj As ScheduledMeetingTransferWizObject = Nothing

                oWizObj = SetTransferWizardObject()
                If oWizObj IsNot Nothing Then

                    If oWizObj.TransferOrderCreditAmount > 0 Then
                        lblNewPrice.Text = "Price Difference Amount: " & Format(oWizObj.TransferOrderCreditAmount, oWizObj.CurrencyFormatString)
                        lblBalance.Text = "Price Difference Amount: " & Format(oWizObj.TransferOrderCreditAmount, oWizObj.CurrencyFormatString)
                        CreditcardWindow.VisibleOnPageLoad = True

                        ViewState(ATTRIBUTE_PAID_AMOUNT) = Format(oWizObj.TransferOrderCreditAmount, oWizObj.CurrencyFormatString)

                    ElseIf oWizObj.TransferOrderCreditAmount < 0 Then

                        lblNewPrice.Text = "You have a credit of " & Format(oWizObj.TransferOrderCreditAmount, oWizObj.CurrencyFormatString) & ", which we will Keep On Account for your company"
                        lblBalance.Text = Format(oWizObj.TransferOrderCreditAmount, oWizObj.CurrencyFormatString)
                        ViewState(ATTRIBUTE_PAID_AMOUNT) = Nothing
                        CreditcardWindow.VisibleOnPageLoad = False
                        chkMakePayment.Checked = False
                    End If

                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try


        End Sub

        'Private Function GetProductPrice(ProductID As Long, PersonID As Integer) As Long
        '    Try
        '        Dim oCommonMethod As CommonMethods = New CommonMethods(DataAction, AptifyApplication, User1, Database)
        '        Return oCommonMethod.GetProductPrice(ProductID, PersonID)
        '    Catch ex As Exception

        '    End Try
        'End Function
        Private Sub DisplayMessage()
            Try
                Dim MeetingID As Long
                Dim newMeeting, oldMeeting, person As String
                Dim rowIndex As Integer = 0
                Dim dtSelectedItem As DataTable = Nothing

                If ViewState(ATTRIBUTE_PREVIOUS_MEETING) IsNot Nothing Then
                    dtSelectedItem = DirectCast(ViewState(ATTRIBUTE_PREVIOUS_MEETING), DataTable)
                End If
                If dtSelectedItem IsNot Nothing AndAlso dtSelectedItem.Rows.Count > 0 Then
                    MeetingID = CLng(dtSelectedItem.Rows(0).Item("MeetingID"))
                End If
                oldMeeting = FindMeeting(CInt(MeetingID))


                Dim NewMeetingID As Long
                If ViewState(ATTRIBUTE_NEW_MEETING) IsNot Nothing AndAlso CInt(ViewState(ATTRIBUTE_NEW_MEETING)) > 0 Then
                    NewMeetingID = CInt(ViewState(ATTRIBUTE_NEW_MEETING))
                End If
                newMeeting = FindMeeting(CInt(NewMeetingID))


                Dim AttendeeID As Long
                If ViewState(ATTRIBUTE_ATTENDEE_ID) IsNot Nothing Then
                    dtSelectedItem = DirectCast(ViewState(ATTRIBUTE_ATTENDEE_ID), DataTable)
                End If
                If dtSelectedItem IsNot Nothing AndAlso dtSelectedItem.Rows.Count > 0 Then
                    AttendeeID = CLng(dtSelectedItem.Rows(0).Item("AttendeeID"))
                End If
                person = FindPersonByID(CInt(AttendeeID))

                lblFinishmessage.Text = "You are registering " & person & " from " & oldMeeting & " to " & newMeeting & ". If this is correct, please click Finish to continue."
                SetTransferWizardObject()
                ValidateforBalance()
                dtSelectedItem = Nothing
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub LoadConfirmationHeader()
            Dim sSQL As String, dt As Data.DataTable
            Dim sCurrencyFormat As String
            Dim dtSelectedItem As DataTable = Nothing
            Try
                CurrencyCache = CurrencyTypeCache.Instance(Me.AptifyApplication)
                sCurrencyFormat = CurrencyCache.CurrencyType(CInt(Me.User1.PreferredCurrencyTypeID)).FormatString

                Dim AttendeeID As Long = 0
                Dim OriOrderID As Long = 0

                If ViewState(ATTRIBUTE_ATTENDEE_ID) IsNot Nothing Then
                    dtSelectedItem = DirectCast(ViewState(ATTRIBUTE_ATTENDEE_ID), DataTable)
                End If

                If dtSelectedItem IsNot Nothing AndAlso dtSelectedItem.Rows.Count > 0 Then
                    AttendeeID = CLng(dtSelectedItem.Rows(0).Item("AttendeeID"))
                    OriOrderID = CLng(dtSelectedItem.Rows(0).Item("OriOrderID"))
                End If

                lblOriginalAttendee.Text = ""
                lblOrderID.Text = ""
                lblOrderBalance.Text = Format$(0, sCurrencyFormat)
                lblOrderTotal.Text = Format$(0, sCurrencyFormat)


                If ViewState(ATTRIBUTE_NEW_ORDERID) IsNot Nothing AndAlso CInt(ViewState(ATTRIBUTE_NEW_ORDERID)) > 0 Then
                    sSQL = "select  o.ID,o.Balance,o.GrandTotal, o.CurrencyTypeID from vwOrders o " _
                       & "WHERE o.ID = " & CStr(ViewState(ATTRIBUTE_NEW_ORDERID))
                Else
                    sSQL = "select  o.ID,o.Balance,o.GrandTotal, o.CurrencyTypeID from vwOrders o " _
                                                          & "WHERE o.ID = " & CStr(OriOrderID)
                End If

                dt = DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)

                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then

                    With dt.Rows(0)

                        lblOriginalAttendee.Text = FindPersonByID(CInt(AttendeeID))

                        If .Item("ID") IsNot Nothing AndAlso Not IsDBNull(.Item("ID")) Then
                            lblOrderID.Text = CStr(.Item("ID"))
                        End If
                        If IsNumeric(.Item("CurrencyTypeID")) Then
                            sCurrencyFormat = CurrencyCache.CurrencyType(CInt(.Item("CurrencyTypeID"))).FormatString
                        End If
                        If .Item("Balance") IsNot Nothing AndAlso Not IsDBNull(.Item("Balance")) Then
                            lblOrderBalance.Text = Format$(CLng(.Item("Balance")), sCurrencyFormat)

                        End If

                        If .Item("GrandTotal") IsNot Nothing AndAlso Not IsDBNull(.Item("GrandTotal")) Then
                            lblOrderTotal.Text = Format$(CLng(.Item("GrandTotal")), sCurrencyFormat)
                        End If

                    End With


                End If


                If Not ViewState(ATTRIBUTE_PAID_AMOUNT) Is Nothing Then
                    lblAmountPaid.Text = ViewState(ATTRIBUTE_PAID_AMOUNT).ToString
                Else
                    lblAmountPaid.Text = Format$(0, sCurrencyFormat)
                End If
                dtSelectedItem = Nothing
                dt = Nothing
                ViewState.Remove("CurrencyFormat")
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Function SaveMeetingTransferRecord() As Long
            Try
                Dim MeetingID As Long
                Dim dtSelectedItem As DataTable = Nothing

                If ViewState(ATTRIBUTE_PREVIOUS_MEETING) IsNot Nothing Then
                    dtSelectedItem = DirectCast(ViewState(ATTRIBUTE_PREVIOUS_MEETING), DataTable)
                End If
                If dtSelectedItem IsNot Nothing AndAlso dtSelectedItem.Rows.Count > 0 Then
                    MeetingID = CLng(dtSelectedItem.Rows(0).Item("MeetingID"))
                End If

                Dim AttendeeID As Long = 0
                Dim OriOrderID As Long = 0
                Dim StatusID As Integer = 0
                Dim OriOrderLineID As Long = 0
                Dim NewMeetingID As Long = 0

                If ViewState(ATTRIBUTE_ATTENDEE_ID) IsNot Nothing Then
                    dtSelectedItem = DirectCast(ViewState(ATTRIBUTE_ATTENDEE_ID), DataTable)
                End If
                If dtSelectedItem IsNot Nothing AndAlso dtSelectedItem.Rows.Count > 0 Then
                    AttendeeID = CLng(dtSelectedItem.Rows(0).Item("AttendeeID"))
                    OriOrderID = CLng(dtSelectedItem.Rows(0).Item("OriOrderID"))
                    StatusID = CInt(dtSelectedItem.Rows(0).Item("StatusID"))
                    OriOrderLineID = CLng(dtSelectedItem.Rows(0).Item("OriOrderLineID"))
                End If

                If ViewState(ATTRIBUTE_NEW_MEETING) IsNot Nothing AndAlso CInt(ViewState(ATTRIBUTE_NEW_MEETING)) > 0 Then
                    NewMeetingID = CInt(ViewState(ATTRIBUTE_NEW_MEETING))
                End If

                Dim oMeetingTransfer As AptifyGenericEntityBase
                oMeetingTransfer = AptifyApplication.GetEntityObject("Meeting Transfer Emails", -1)
                oMeetingTransfer.SetValue("PreAttendeeID", CInt(AttendeeID))
                oMeetingTransfer.SetValue("AdminID", CInt(User1.PersonID))
                oMeetingTransfer.SetValue("CancelledOrderID", CInt(OriOrderID))
                If ViewState(ATTRIBUTE_NEW_ORDERID) IsNot Nothing AndAlso CInt(ViewState(ATTRIBUTE_NEW_ORDERID)) > 0 Then
                    oMeetingTransfer.SetValue("NewOrderID", CInt(ViewState(ATTRIBUTE_NEW_ORDERID)))
                Else
                    oMeetingTransfer.SetValue("NewOrderID", CInt(OriOrderID))
                End If
                oMeetingTransfer.SetValue("MeetingID", CInt(MeetingID))
                oMeetingTransfer.SetValue("NewMeetingID", CInt(NewMeetingID))
                If oMeetingTransfer.Save(False) Then
                    Return oMeetingTransfer.RecordID
                End If
                Return -1
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return -1
            End Try
        End Function
        Private Function GetMessageTemplateID(ByVal sTemplateName As String) As Integer
            Try
                Dim sSql As String
                Dim dt As DataTable
                sSql = "Select ID from vwMessageTemplates where name  = '" & sTemplateName & "'"
                dt = DataAction.GetDataTable(sSql)
                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                    Return CInt(dt.Rows(0).Item("ID"))
                End If
                Return -1
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return -1
            End Try
        End Function
        Private Sub DeleteRecord(ByVal MeetingTransferID As Long)
            Try
                Dim sSql As String
                sSql = "DELETE FROM MeetingTransferEmail WHERE ID = " & MeetingTransferID
                DataAction.ExecuteNonQuery(sSql)
            Catch ex As Exception

            End Try
        End Sub

        Private Sub ShowBillMeLater()
            Dim iPrevPaymentTypeID As Integer
            Dim iPOPaymentType As Integer = 0
            Dim sError As String
            Dim oOrder As Aptify.Applications.OrderEntry.OrdersEntity

            Try
                If Not String.IsNullOrEmpty(AptifyApplication.GetEntityAttribute("Web Shopping Carts", "POPaymentTypeID")) Then
                    iPOPaymentType = CInt(AptifyApplication.GetEntityAttribute("Web Shopping Carts", "POPaymentTypeID"))
                End If
                Dim dr As Data.DataRow = User1.CompanyDataRow
                CreditCard.UserCreditStatus = CInt(User1.GetValue("CreditStatusID"))
                CreditCard.UserCreditLimit = CLng(User1.GetValue("CreditLimit"))
                oOrder = ShoppingCart1.GetOrderObject(Session, Page.User, Application)
                If iPOPaymentType > 0 Then
                    If dr IsNot Nothing Then
                        CreditCard.CompanyCreditStatus = CInt(dr.Item("CreditStatusID"))
                        CreditCard.CompanyCreditLimit = CLng(dr.Item("CreditLimit"))
                    End If
                    If oOrder IsNot Nothing Then
                        iPrevPaymentTypeID = CInt(oOrder.GetValue("PayTypeID"))
                        oOrder.SetValue("PayTypeID", iPOPaymentType)
                        CreditCard.CreditCheckLimit = ShoppingCart1.CreditCheckObject.CheckCredit(CType(oOrder, Aptify.Applications.OrderEntry.OrdersEntity), sError)
                    End If
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            Finally
                ''oOrder.SetValue("PayTypeID", iPrevPaymentTypeID)
            End Try

        End Sub


        Private Sub SaveCheckedValues(ByVal Type As String)
            Dim dtPreMeeting, dtAttendee As DataTable
            Dim NewMeetingID As String = ""
            'Dim index As Long = -1
            Dim lblAttendeeID As Label = New Label
            Dim lblNewMeetingID As Label = New Label

            Dim index As String = "-1"

            dtPreMeeting = New DataTable
            dtPreMeeting.Columns.Add("MeetingID")
            dtPreMeeting.Columns.Add("IsSession")
            dtPreMeeting.Columns.Add("ParentID")

            Dim primaryKey(0) As DataColumn
            primaryKey(0) = dtPreMeeting.Columns("MeetingID")
            dtPreMeeting.PrimaryKey = primaryKey

            dtAttendee = New DataTable

            dtAttendee.Columns.Add("AttendeeID")
            dtAttendee.Columns.Add("OriOrderID")
            dtAttendee.Columns.Add("StatusID")
            dtAttendee.Columns.Add("OriOrderLineID")

            primaryKey(0) = dtAttendee.Columns("AttendeeID")
            dtAttendee.PrimaryKey = primaryKey


            If ViewState(ATTRIBUTE_PREVIOUS_MEETING) IsNot Nothing Then
                dtPreMeeting = DirectCast(ViewState(ATTRIBUTE_PREVIOUS_MEETING), DataTable)
            End If
            If ViewState(ATTRIBUTE_ATTENDEE_ID) IsNot Nothing Then
                dtAttendee = DirectCast(ViewState(ATTRIBUTE_ATTENDEE_ID), DataTable)
            End If
            If ViewState(ATTRIBUTE_NEW_MEETING) IsNot Nothing Then
                NewMeetingID = CStr(ViewState(ATTRIBUTE_NEW_MEETING))
            End If
            Dim lblMeetingID As Label = New Label
            Dim dr As DataRow = dtPreMeeting.NewRow
            Select Case Type
                Case "PreMeeting"
                    If grdUpcomingMeeting.Items.Count > 0 Then
                        For Each item As GridDataItem In grdUpcomingMeeting.MasterTableView.Items
                            lblMeetingID = CType(item.FindControl("lblMeetingID"), Label)
                            index = (lblMeetingID.Text)
                            'index = item("MeetingID").Text
                            Dim result As Boolean = DirectCast(item.FindControl("optSelectMeeting"), RadioButton).Checked

                            If result Then
                                dtPreMeeting.Rows.Clear()
                                dr.Item("MeetingID") = CType(item.FindControl("lblMeetingID"), Label).Text
                                dr.Item("IsSession") = CType(item.FindControl("chkIsSession"), CheckBox).Checked
                                dr.Item("ParentID") = CType(item.FindControl("lblParentID"), Label).Text

                                If Not dtPreMeeting.Rows.Contains(index) Then
                                    dtPreMeeting.Rows.Add(dr)
                                End If
                            End If
                        Next
                    End If

                Case "Attendee"
                    dr = dtAttendee.NewRow
                    If grdMeetingRegistrant.Items.Count > 0 Then
                        For Each item As GridDataItem In grdMeetingRegistrant.MasterTableView.Items
                            lblAttendeeID = CType(item.FindControl("lblAttendeeID"), Label)
                            index = lblAttendeeID.Text
                            Dim result As Boolean = DirectCast(item.FindControl("optSelectAttendee"), RadioButton).Checked
                            If result Then
                                dtAttendee.Rows.Clear()
                                dr.Item("AttendeeID") = CType(item.FindControl("lblAttendeeID"), Label).Text
                                dr.Item("OriOrderID") = CType(item.FindControl("lblOrderID"), Label).Text
                                dr.Item("StatusID") = CType(item.FindControl("lblStatusID"), Label).Text
                                dr.Item("OriOrderLineID") = CType(item.FindControl("lblOrderLineID"), Label).Text
                                If Not dtAttendee.Rows.Contains(index) Then
                                    dtAttendee.Rows.Add(dr)
                                End If
                            End If
                        Next
                    End If

                Case "NewMeeting"
                    If grdNewMeetings.Items.Count > 0 Then
                        For Each item As GridDataItem In grdNewMeetings.MasterTableView.Items
                            lblNewMeetingID = CType(item.FindControl("lblNewMeetingID"), Label)
                            index = lblNewMeetingID.Text
                            Dim result As Boolean = DirectCast(item.FindControl("optSelectNewMeeting"), RadioButton).Checked
                            If result Then
                                If CBool(String.Compare(NewMeetingID, (index))) Then
                                    NewMeetingID = CStr(index)
                                End If
                            End If
                        Next
                    End If
            End Select


            If dtPreMeeting IsNot Nothing Then
                ViewState(ATTRIBUTE_PREVIOUS_MEETING) = dtPreMeeting
            End If
            If dtAttendee IsNot Nothing Then
                ViewState(ATTRIBUTE_ATTENDEE_ID) = dtAttendee
            End If
            If NewMeetingID IsNot Nothing AndAlso NewMeetingID <> "" Then
                ViewState(ATTRIBUTE_NEW_MEETING) = NewMeetingID
            End If
            dtPreMeeting = Nothing
            dtAttendee = Nothing
        End Sub

        ''Rashmi p, procedure reset all the fields of credit card
        Private Sub ClearCreditCardControl()
            CreditCard.CCNumber = ""
            CreditCard.CCSecurityNumber = ""
            CreditCard.CCExpireDate = CStr(Now.Date)
            CreditCard.PONumber = ""
            CreditCard.BillMeLaterChecked = False
            CreditCard.SelectCardType("")
            CreditCard.SetchkSaveforFutureUse = False
        End Sub

        Private Sub AddExpression()
            Dim expression As New GridSortExpression
            expression.FieldName = "StartDate"
            expression.SetSortOrder("Ascending")
            grdUpcomingMeeting.MasterTableView.SortExpressions.AddSortExpression(expression)

            expression = New GridSortExpression
            expression.FieldName = "AttendeeID_FirstLast"
            expression.SetSortOrder("Ascending")
            grdMeetingRegistrant.MasterTableView.SortExpressions.AddSortExpression(expression)

            expression = New GridSortExpression
            expression.FieldName = "StartDate"
            expression.SetSortOrder("Ascending")
            grdNewMeetings.MasterTableView.SortExpressions.AddSortExpression(expression)

        End Sub

#End Region

#Region "Controls Events"

        Protected Sub WizardMeetingTransfer_ActiveStepChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles WizardMeetingTransfer.ActiveStepChanged
            Dim stepNavTemplate As WebControl = TryCast(Me.WizardMeetingTransfer.FindControl("StepNavigationTemplateContainerID"), WebControl)
            Dim FinishTemplate As WebControl = TryCast(Me.WizardMeetingTransfer.FindControl("FinishNavigationTemplateContainerID"), WebControl)
            If stepNavTemplate IsNot Nothing Then
                Dim btnNext As Button = TryCast(stepNavTemplate.FindControl("StepNextButton"), Button)
                Dim btnPrevious As Button = TryCast(stepNavTemplate.FindControl("StepPreviousButton"), Button)
                If btnPrevious IsNot Nothing Then
                    btnPrevious.CausesValidation = False
                End If
                If btnNext IsNot Nothing Then
                    btnNext.CausesValidation = False
                End If
            End If
            If FinishTemplate IsNot Nothing Then
                Dim btnFinish As Button = TryCast(FinishTemplate.FindControl("FinishButton"), Button)
                If btnFinish IsNot Nothing Then
                    btnFinish.CausesValidation = False
                End If
            End If
            upnlMeetingRegistrant.Update()
            upnlNewMeetings.Update()
            upnlUpcomingMeeting.Update()
        End Sub

        Protected Sub WizardMeetingTransfer_FinishButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.WizardNavigationEventArgs) Handles WizardMeetingTransfer.FinishButtonClick
            Try
                Dim MeetingID As Long
                Dim dtSelectedItem As DataTable = Nothing

                Dim NewMeetingID As Long
                If ViewState(ATTRIBUTE_NEW_MEETING) IsNot Nothing AndAlso CInt(ViewState(ATTRIBUTE_NEW_MEETING)) > 0 Then
                    NewMeetingID = CInt(ViewState(ATTRIBUTE_NEW_MEETING))
                End If
                If NewMeetingID <= 0 Then
                    lblError.Text = "Select Meeting to replace."
                    e.Cancel = True
                Else

                    If ViewState(ATTRIBUTE_PREVIOUS_MEETING) IsNot Nothing Then
                        dtSelectedItem = DirectCast(ViewState(ATTRIBUTE_PREVIOUS_MEETING), DataTable)
                    End If
                    If dtSelectedItem IsNot Nothing AndAlso dtSelectedItem.Rows.Count > 0 Then
                        MeetingID = CLng(dtSelectedItem.Rows(0).Item("MeetingID"))
                    End If

                    Dim AttendeeID As Long = 0
                    If ViewState(ATTRIBUTE_ATTENDEE_ID) IsNot Nothing Then
                        dtSelectedItem = DirectCast(ViewState(ATTRIBUTE_ATTENDEE_ID), DataTable)
                    End If
                    If dtSelectedItem IsNot Nothing AndAlso dtSelectedItem.Rows.Count > 0 Then
                        AttendeeID = CLng(dtSelectedItem.Rows(0).Item("AttendeeID"))
                    End If
                    If FinishTransfer() Then
                        lblFinishmessage.Text = "Meeting transferred successfully."
                        lblCompleteMsg.Text = FindMeeting(CInt(MeetingID)) & " Meeting has been transferred to " & FindMeeting(CInt(NewMeetingID)) & " meeting for " & FindPersonByID(CInt(AttendeeID)) & "."
                        LoadConfirmationHeader()
                        tblTransferConfirmation.Visible = True
                        WizardMeetingTransfer.Visible = False
                    Else
                        e.Cancel = True
                        lblError.Visible = True
                    End If
                End If
                dtSelectedItem = Nothing
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            Finally
                ViewState(ATTRIBUTE_DT_MEETING_ORDER) = Nothing
                ViewState(ATTRIBUTE_DT_MEETING_REGISTRATION) = Nothing
                ViewState(ATTRIBUTE_DT_NEW_MEETING) = Nothing
                ViewState(ATTRIBUTE_PAID_AMOUNT) = Nothing
                ViewState(ATTRIBUTE_PRICE_DIFFERENCE) = Nothing
            End Try
        End Sub

        Protected Sub WizardMeetingTransfer_NextButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.WizardNavigationEventArgs) Handles WizardMeetingTransfer.NextButtonClick
            Dim icurrentStep As Int32
            icurrentStep = WizardMeetingTransfer.ActiveStepIndex
            Dim rowIndex As Integer = -1
            Dim dtSelectedItem As DataTable = Nothing
            Select Case (icurrentStep)
                Case 0
                    If grdUpcomingMeeting.Items.Count = 0 Then
                        e.Cancel = True
                        Exit Select
                    End If
                    SaveCheckedValues("PreMeeting")
                    If ViewState(ATTRIBUTE_PREVIOUS_MEETING) IsNot Nothing Then
                        dtSelectedItem = DirectCast(ViewState(ATTRIBUTE_PREVIOUS_MEETING), DataTable)
                    End If
                    If dtSelectedItem IsNot Nothing AndAlso dtSelectedItem.Rows.Count > 0 Then
                        Dim MeetingID As Integer
                        MeetingID = CInt(dtSelectedItem.Rows(0).Item("MeetingID"))
                        lblMeetingTitle.Text = FindMeeting(MeetingID)
                        FillMeetingRegistrants(MeetingID)
                    Else
                        lblError.Text = "Select Meeting to replace."
                        e.Cancel = True
                    End If

                Case 1
                    If grdMeetingRegistrant.Items.Count = 0 Then
                        e.Cancel = True
                        Exit Select
                    End If
                    SaveCheckedValues("Attendee")
                    dtSelectedItem = Nothing
                    If ViewState(ATTRIBUTE_ATTENDEE_ID) IsNot Nothing Then
                        dtSelectedItem = DirectCast(ViewState(ATTRIBUTE_ATTENDEE_ID), DataTable)
                    End If
                    If dtSelectedItem IsNot Nothing AndAlso dtSelectedItem.Rows.Count > 0 Then
                        FillNewMeetingGrid()
                    Else
                        lblError.Text = "Select an Attendee."
                        e.Cancel = True
                    End If

                Case 2
                    If grdNewMeetings.Items.Count = 0 Then
                        e.Cancel = True
                        Exit Select
                    End If
                    SaveCheckedValues("NewMeeting")
                    CreditCard.LoadSavedPayments()
                    If ViewState(ATTRIBUTE_NEW_MEETING) IsNot Nothing AndAlso CInt(ViewState(ATTRIBUTE_NEW_MEETING)) > 0 Then
                        rowIndex = CInt(ViewState(ATTRIBUTE_NEW_MEETING))
                    End If
                    If rowIndex < 0 Then
                        lblError.Text = "Select New Meeting."
                        e.Cancel = True
                    Else
                        DisplayMessage()
                    End If

            End Select
            upnlMeetingRegistrant.Update()
            upnlNewMeetings.Update()
            upnlUpcomingMeeting.Update()
        End Sub

        Protected Sub WizardMeetingTransfer_PreviousButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.WizardNavigationEventArgs) Handles WizardMeetingTransfer.PreviousButtonClick
            ClearCreditCardControl()
            Dim icurrentStep As Int32
            icurrentStep = WizardMeetingTransfer.ActiveStepIndex
            Select Case (icurrentStep)
                Case 0
                    SaveCheckedValues("PreMeeting")
                Case 1
                    SaveCheckedValues("Attendee")
                Case 2
                    SaveCheckedValues("NewMeeting")
            End Select
        End Sub

        Protected Sub WizardMeetingTransfer_SideBarButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.WizardNavigationEventArgs) Handles WizardMeetingTransfer.SideBarButtonClick
            e.Cancel = True
        End Sub

        Protected Sub grdUpcomingMeeting_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdUpcomingMeeting.ItemDataBound
            Dim dateColumns As New List(Of String)
            dateColumns.Add("GridDateTimeColumnStartDate")
            CommonMethods.FormatedDateOnGrid(dateColumns, e.Item)
        End Sub

        Protected Sub grdUpcomingMeeting_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdUpcomingMeeting.NeedDataSource
            If ViewState(ATTRIBUTE_DT_MEETING_ORDER) IsNot Nothing Then
                grdUpcomingMeeting.DataSource = CType(ViewState(ATTRIBUTE_DT_MEETING_ORDER), DataTable)
            End If
        End Sub
        Protected Sub grdUpcomingMeeting_PageIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageChangedEventArgs) Handles grdUpcomingMeeting.PageIndexChanged
            If ViewState(ATTRIBUTE_DT_MEETING_ORDER) IsNot Nothing Then
                grdUpcomingMeeting.DataSource = CType(ViewState(ATTRIBUTE_DT_MEETING_ORDER), DataTable)
                grdUpcomingMeeting.CurrentPageIndex = e.NewPageIndex
            End If
            SaveCheckedValues("PreMeeting")
        End Sub

        Protected Sub grdUpcomingMeeting_PageSizeChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageSizeChangedEventArgs) Handles grdUpcomingMeeting.PageSizeChanged
            If ViewState(ATTRIBUTE_DT_MEETING_ORDER) IsNot Nothing Then
                grdUpcomingMeeting.DataSource = CType(ViewState(ATTRIBUTE_DT_MEETING_ORDER), DataTable)
            End If
        End Sub

        Protected Sub grdUpcomingMeeting_GridItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs)

            Dim optSelectedMeeting As RadioButton = DirectCast(e.Item.FindControl("optSelectMeeting"), RadioButton)
            Dim lblMeetingID As Label = DirectCast(e.Item.FindControl("lblMeetingID"), Label)
            If optSelectedMeeting IsNot Nothing Then
                If ViewState(ATTRIBUTE_PREVIOUS_MEETING) IsNot Nothing Then
                    Dim dtPreMeeting As DataTable = DirectCast(ViewState(ATTRIBUTE_PREVIOUS_MEETING), DataTable)

                    Dim dataItem As DataRowView = DirectCast(e.Item.DataItem, System.Data.DataRowView)

                    If dataItem IsNot Nothing Then
                        Dim ID As String = CStr(dataItem("MeetingID"))

                        If dtPreMeeting IsNot Nothing Then

                            If dtPreMeeting.Rows.Contains(ID) Then
                                optSelectedMeeting.Checked = True
                            Else
                                optSelectedMeeting.Checked = False
                            End If

                        End If
                    End If
                End If
            End If
        End Sub
        'Neha Issue 14810,03/13/13, Resizes the passed Image as specified width and height and returns the resized Image
        Protected Sub grdMeetingRegistrant_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdMeetingRegistrant.ItemDataBound
            Try
                Dim rdImage As RadBinaryImage = Nothing
                If e.Item Is Nothing OrElse e.Item.FindControl("ImgAttendeePhoto") Is Nothing Then
                    Exit Sub
                End If
                rdImage = CType(e.Item.FindControl("ImgAttendeePhoto"), RadBinaryImage)
                rdImage.ImageUrl = RadBlankImage
                rdImage.DataBind()
                'Resizes the passed Image according to the specified width and height and returns the resized Image
                If Not IsDBNull(DataBinder.Eval(e.Item.DataItem, "Photo")) Then
                    Dim commonMethods As New Aptify.Framework.Web.eBusiness.CommonMethods()
                    Dim profileImage As Drawing.Image = Nothing
                    Dim width As Integer = ProfileThumbNailWidth
                    Dim height As Integer = ProfileThumbNailHeight
                    Dim aspratioWidth As Integer

                    Dim profileImageByte As Byte() = DirectCast(DataBinder.Eval(e.Item.DataItem, "Photo"), Byte())
                    If profileImageByte IsNot Nothing AndAlso profileImageByte.Length > 0 Then
                        commonMethods.getResizedImageHeightandWidth(profileImage, profileImageByte, ProfileThumbNailWidth, ProfileThumbNailHeight, aspratioWidth)
                        profileImage = commonMethods.byteArrayToImage(profileImageByte)
                        profileImageByte = commonMethods.resizeImageAndGetAsByte(profileImage, aspratioWidth, height)
                        rdImage.DataValue = profileImageByte
                        rdImage.DataBind()
                    Else
                        rdImage.ImageUrl = Me.FixLinkForVirtualPath(RadBlankImage)
                        rdImage.DataBind()
                    End If
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub grdMeetingRegistrant_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdMeetingRegistrant.NeedDataSource

            If ViewState(ATTRIBUTE_DT_MEETING_REGISTRATION) IsNot Nothing Then
                grdMeetingRegistrant.DataSource = CType(ViewState(ATTRIBUTE_DT_MEETING_REGISTRATION), DataTable)
            End If
        End Sub

        Protected Sub grdMeetingRegistrant_PageIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageChangedEventArgs) Handles grdMeetingRegistrant.PageIndexChanged
            If ViewState(ATTRIBUTE_DT_MEETING_REGISTRATION) IsNot Nothing Then
                grdMeetingRegistrant.DataSource = CType(ViewState(ATTRIBUTE_DT_MEETING_REGISTRATION), DataTable)
                grdMeetingRegistrant.CurrentPageIndex = e.NewPageIndex
            End If
            SaveCheckedValues("Attendee")
        End Sub


        Protected Sub grdMeetingRegistrant_PageSizeChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageSizeChangedEventArgs) Handles grdMeetingRegistrant.PageSizeChanged
            If ViewState(ATTRIBUTE_DT_MEETING_REGISTRATION) IsNot Nothing Then
                grdMeetingRegistrant.DataSource = CType(ViewState(ATTRIBUTE_DT_MEETING_REGISTRATION), DataTable)
            End If
        End Sub

        Protected Sub grdMeetingRegistrant_GridItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs)

            Dim optSelectedAttendee As RadioButton = DirectCast(e.Item.FindControl("optSelectAttendee"), RadioButton)
            Dim lblAttendeeID As Label = DirectCast(e.Item.FindControl("AttendeeID"), Label)
            If optSelectedAttendee IsNot Nothing Then
                If ViewState(ATTRIBUTE_ATTENDEE_ID) IsNot Nothing Then
                    Dim dtAttendee As DataTable = DirectCast(ViewState(ATTRIBUTE_ATTENDEE_ID), DataTable)

                    Dim dataItem As DataRowView = DirectCast(e.Item.DataItem, System.Data.DataRowView)

                    If dataItem IsNot Nothing Then
                        Dim ID As String = CStr(dataItem("AttendeeID"))
                        If dtAttendee IsNot Nothing Then
                            If dtAttendee.Rows.Contains(ID) Then
                                optSelectedAttendee.Checked = True
                            Else
                                optSelectedAttendee.Checked = False
                            End If

                        End If
                    End If
                End If
            End If
        End Sub

        Protected Sub grdNewMeetings_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdNewMeetings.ItemDataBound
            Dim dateColumns As New List(Of String)
            dateColumns.Add("GridDateTimeColumnStartDate")
            CommonMethods.FormatedDateOnGrid(dateColumns, e.Item)
        End Sub


        Protected Sub grdNewMeetings_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdNewMeetings.NeedDataSource
            If ViewState(ATTRIBUTE_DT_NEW_MEETING) IsNot Nothing Then
                grdNewMeetings.DataSource = CType(ViewState(ATTRIBUTE_DT_NEW_MEETING), DataTable)
            End If
        End Sub


        Protected Sub grdNewMeetings_PageIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageChangedEventArgs) Handles grdNewMeetings.PageIndexChanged
            If ViewState(ATTRIBUTE_DT_NEW_MEETING) IsNot Nothing Then
                grdNewMeetings.DataSource = CType(ViewState(ATTRIBUTE_DT_NEW_MEETING), DataTable)
                grdNewMeetings.CurrentPageIndex = e.NewPageIndex
            End If
            SaveCheckedValues("NewMeeting")
        End Sub

        Protected Sub grdNewMeetings_PageSizeChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageSizeChangedEventArgs) Handles grdNewMeetings.PageSizeChanged
            If ViewState(ATTRIBUTE_DT_NEW_MEETING) IsNot Nothing Then
                grdNewMeetings.DataSource = CType(ViewState(ATTRIBUTE_DT_NEW_MEETING), DataTable)
            End If
        End Sub

        Protected Sub grdNewMeetings_GridItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs)

            Dim optSelectedMeeting As RadioButton = DirectCast(e.Item.FindControl("optSelectNewMeeting"), RadioButton)
            Dim lblMeetingID As Label = DirectCast(e.Item.FindControl("lblNewMeetingID"), Label)
            If optSelectedMeeting IsNot Nothing Then

                If ViewState(ATTRIBUTE_NEW_MEETING) IsNot Nothing Then
                    Dim NewMeetingID As String = DirectCast(ViewState(ATTRIBUTE_NEW_MEETING), String)

                    Dim dataItem As DataRowView = DirectCast(e.Item.DataItem, System.Data.DataRowView)

                    If dataItem IsNot Nothing Then
                        Dim ID As String = CStr(dataItem("MeetingID"))
                        If Not CBool(String.Compare(NewMeetingID, ID)) Then
                            optSelectedMeeting.Checked = True
                        Else
                            optSelectedMeeting.Checked = False
                        End If
                    End If
                End If
            End If
        End Sub

        Protected Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
            chkMakePayment.Checked = True
            CreditcardWindow.VisibleOnPageLoad = False
            If CreditCard.BillMeLaterChecked Then
                ViewState(ATTRIBUTE_PAID_AMOUNT) = Nothing
            End If
            WizardMeetingTransfer_ActiveStepChanged(Nothing, Nothing)
        End Sub

        Protected Sub btnSendMail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSendMail.Click
            Try
                Dim lMessageTemplateID As Integer
                Dim lMeetingTransferID As Long
                lMeetingTransferID = SaveMeetingTransferRecord()
                If lMeetingTransferID > 0 Then

                    Dim sSQL As String = "SELECT Top 1 ID FROM " & Database & "..vwProcessFlows WHERE Name='Send Meeting Transfer Confirmation Email'"
                    Dim lProcessFlowID As Long = CLng(DataAction.ExecuteScalar(sSQL, IAptifyDataAction.DSLCacheSetting.UseCache))

                    Dim context As New AptifyContext

                    context.Properties.AddProperty("MeetingTransferID", lMeetingTransferID)
                    lMessageTemplateID = GetMessageTemplateID("Meeting Transfer Email")

                    If lMessageTemplateID = 0 Then
                        lblError.Text = "'Meeting Transfer Email' Message Template does not exist."
                    Else
                        context.Properties.AddProperty("MessageTemplateID", lMessageTemplateID)
                        Dim result As ProcessFlowResult
                        result = ProcessFlowEngine.ExecuteProcessFlow(Me.AptifyApplication, lProcessFlowID, context)
                        If result.IsSuccess Then
                            SendEmailLabel.ForeColor = Drawing.Color.Blue
                            SendEmailLabel.Text = "Meeting Transfer Confirmation Mail has been sent successfully."
                        Else
                            SendEmailLabel.ForeColor = Drawing.Color.Red
                            SendEmailLabel.Text = "Email failed. Contact Customer Service for help."
                        End If
                    End If

                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            CreditcardWindow.VisibleOnPageLoad = False
            ClearCreditCardControl()
            ViewState(ATTRIBUTE_PAID_AMOUNT) = Nothing
            WizardMeetingTransfer_ActiveStepChanged(Nothing, Nothing)
        End Sub
#End Region

    End Class
End Namespace


