'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports System
Imports System.Data
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.Application
Imports Aptify.Framework.BusinessLogic.ProcessPipeline
Imports Aptify.Applications.OrderEntry
Imports System.Collections.Generic
Imports Telerik.Web.UI
Imports Aptify.Applications.Accounting

Namespace Aptify.Framework.Web.eBusiness
    ''' <summary>
    ''' Rashmi Panchal, Issue 14332, Date 10/3/2012
    ''' As a Group Administrator I should be able to replace a person with another person for a meeting or a session. 
    ''' </summary>
    ''' <remarks></remarks>
    Partial Class AttendeeTransferWiz
        Inherits BaseUserControlAdvanced
        Dim CurrencyCache As CurrencyTypeCache

        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "AttendeeTransferWiz"
        Protected Const ATTRIBUTE_COMPANY_LOGO_IMAGE_URL As String = "GACompanyLogo"
        Protected Const ATTRIBUTE_BLANK_IMG_URL As String = "RadBlankImage"
        Protected Const ATTRIBUTE_COMPANY_ADDRESS As String = "CompanyAddress"
        Protected Const ATTRIBUTE_DT_MEETING_ORDER As String = "dtMeetingsOrders"
        Protected Const ATTRIBUTE_DT_MEETING_REGISTRATION As String = "dtMeetingRegistrants"
        Protected Const ATTRIBUTE_DT_ATTENDEE As String = "dtAttendeeTable"
        Protected Const ATTRIBUTE_PAID_AMOUNT As String = "PaidAmount"
        Protected Const ATTRIBUTE_PRICE_DIFFERENCE As String = "PriceDifference"
        Protected Const ATTRIBUTE_TRANSFER_FEE As String = "TransferFees"
        Protected Const ATTRIBUTE_TRANSFER_FEE_PRODUCT_ID As String = "TransferFeeProductID"
        Protected Const ATTRIBUTE_NEW_ORDERID As String = "NewOrderID"
        Protected Const ATTRIBUTE_PREVIOUS_MEETING As String = "Previous_Meeting"
        Protected Const ATTRIBUTE_ATTENDEE_ID As String = "AttendeeID"
        Protected Const ATTRIBUTE_NEW_ATTENDEEID As String = "New_AttendeeID"
        'Neha Issue 14810,03/09/13, Declared properties for RadBinaryimage
        Protected Const ATTRIBUTE_PROFILE_THUMBNAIL_WIDTH As String = "ProfileThumbNailWidth"
        Protected Const ATTRIBUTE_PROFILE_THUMBNAIL_HEIGHT As String = "ProfileThumbNailHeight"
        Protected Const ATTRIBUTE_LOGIN_PAGE As String = "LoginPage" 'Added by Sandeep for Issue 15051 on 12/03/2013
        Dim m_oWizObject As ScheduledMeetingTransferWizObject

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
            InitializeProperties()
            SetProperties()
            If Not IsPostBack Then

                FillMeetingsOrders()
                CreditCard.LoadCreditCardInfo()
                ShowBillMeLater()
                chkMakePayment.Checked = False
                tblTransferConfirmation.Visible = False
            End If
            CreditCard.SetchkSaveforFutureUse = False
            If User1.UserID < 0 Then
                Response.Redirect(LoginPage) 'Added by Sandeep for Issue 15051 on 12/03/2013
            End If
            upnlMeetingRegistrant.Update()
            upnlWaitingList.Update()
            upnlUpcomingMeeting.Update()
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
        'Neha, Issue 14810, 03/09/13, Overrided properties for Rdabinaryimage
        ''' <summary>
        ''' ProfileThumbNailWidth
        ''' </summary>
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
        ''' <summary>
        ''' ProfileThumbNailHeight
        ''' </summary>
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
#End Region

#Region "Controls Events"

        Protected Sub grdUpcomingMeeting_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdUpcomingMeeting.ItemDataBound
            Dim dateColumns As New List(Of String)
            'Add datecolumn uniqueName in list for Date format

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
            lblError.Text = ""
            SaveCheckedValues("Meeting")
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

        Protected Sub grdMeetingRegistrant_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdMeetingRegistrant.ItemDataBound
            Try
                Dim rdImage As RadBinaryImage = Nothing
                If e.Item Is Nothing OrElse e.Item.FindControl("ImgAttendeePhoto") Is Nothing Then
                    Exit Sub
                End If
                rdImage = CType(e.Item.FindControl("ImgAttendeePhoto"), RadBinaryImage)
                rdImage.ImageUrl = RadBlankImage
                rdImage.DataBind()
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
        Protected Sub grdMeetingRegistrant_GridItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs)

            Dim optSelectedAttendee As RadioButton = DirectCast(e.Item.FindControl("optSelectAttendee"), RadioButton)
            Dim lblAttendeeID As Label = DirectCast(e.Item.FindControl("lblAttendeeID"), Label)
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
            lblError.Text = ""
            SaveCheckedValues("PreAttendee")
        End Sub


        Protected Sub grdMeetingRegistrant_PageSizeChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageSizeChangedEventArgs) Handles grdMeetingRegistrant.PageSizeChanged
            If ViewState(ATTRIBUTE_DT_MEETING_REGISTRATION) IsNot Nothing Then
                grdMeetingRegistrant.DataSource = CType(ViewState(ATTRIBUTE_DT_MEETING_REGISTRATION), DataTable)
            End If
        End Sub

        'Neha changes for issue 16001, used aspect ratio for telerik images
        Protected Sub grdWaitingList_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdWaitingList.ItemDataBound
            Try
                Dim rdImage As RadBinaryImage = Nothing
                If e.Item Is Nothing OrElse e.Item.FindControl("ImgNewAttendeePhoto") Is Nothing Then
                    Exit Sub
                End If
                rdImage = CType(e.Item.FindControl("ImgNewAttendeePhoto"), RadBinaryImage)
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

        Protected Sub grdWaitingList_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdWaitingList.NeedDataSource
            If ViewState(ATTRIBUTE_DT_ATTENDEE) IsNot Nothing Then
                grdWaitingList.DataSource = CType(ViewState(ATTRIBUTE_DT_ATTENDEE), DataTable)
            End If
        End Sub

        Protected Sub grdWaitingList_PageIndexChanging(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageChangedEventArgs) Handles grdWaitingList.PageIndexChanged
            If ViewState(ATTRIBUTE_DT_ATTENDEE) IsNot Nothing Then
                grdWaitingList.DataSource = CType(ViewState(ATTRIBUTE_DT_ATTENDEE), DataTable)
                grdWaitingList.CurrentPageIndex = e.NewPageIndex
            End If
            lblError.Text = ""
            SaveCheckedValues("NewAttendee")
        End Sub

        Protected Sub grdWaitingList_PageSizeChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageSizeChangedEventArgs) Handles grdWaitingList.PageSizeChanged
            If ViewState(ATTRIBUTE_DT_ATTENDEE) IsNot Nothing Then
                grdWaitingList.DataSource = CType(ViewState(ATTRIBUTE_DT_ATTENDEE), DataTable)
            End If
        End Sub

        Protected Sub grdWaitingList_GridItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs)

            Dim optSelectedAttendee As RadioButton = DirectCast(e.Item.FindControl("optSelectNewAttendee"), RadioButton)
            If optSelectedAttendee IsNot Nothing Then

                If ViewState(ATTRIBUTE_NEW_ATTENDEEID) IsNot Nothing Then
                    Dim NewAttendeeID As String = DirectCast(ViewState(ATTRIBUTE_NEW_ATTENDEEID), String)

                    Dim dataItem As DataRowView = DirectCast(e.Item.DataItem, System.Data.DataRowView)

                    If dataItem IsNot Nothing Then
                        Dim ID As String = CStr(dataItem("AttendeeID"))
                        If Not CBool(String.Compare(NewAttendeeID, ID)) Then
                            optSelectedAttendee.Checked = True
                        Else
                            optSelectedAttendee.Checked = False
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
            upnlWaitingList.Update()
            upnlUpcomingMeeting.Update()
        End Sub

        Protected Sub WizardMeetingTransfer_NextButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.WizardNavigationEventArgs) Handles WizardMeetingTransfer.NextButtonClick
            Try
                Dim icurrentStep As Int32
                icurrentStep = WizardMeetingTransfer.ActiveStepIndex
                Dim dtSelectedItem As DataTable = Nothing
                lblError.Visible = False
                Select Case (icurrentStep)
                    Case 0
                        If grdUpcomingMeeting.Items.Count = 0 Then
                            e.Cancel = True
                            Exit Select
                        End If
                        SaveCheckedValues("Meeting")
                        If ViewState(ATTRIBUTE_PREVIOUS_MEETING) IsNot Nothing Then
                            dtSelectedItem = DirectCast(ViewState(ATTRIBUTE_PREVIOUS_MEETING), DataTable)
                        End If
                        If dtSelectedItem IsNot Nothing AndAlso dtSelectedItem.Rows.Count > 0 Then
                            Dim MeetingID As Integer
                            MeetingID = CInt(dtSelectedItem.Rows(0).Item("MeetingID"))
                            lblMeetingTitle.Text = FindMeeting(CInt(MeetingID))
                            FillMeetingRegistrants(MeetingID)
                        Else
                            lblError.Text = "Select a Meeting."
                            lblError.Visible = True
                            e.Cancel = True
                        End If

                    Case 1
                        If grdMeetingRegistrant.Items.Count = 0 Then
                            e.Cancel = True
                            Exit Select
                        End If
                        SaveCheckedValues("PreAttendee")
                        dtSelectedItem = Nothing
                        If ViewState(ATTRIBUTE_ATTENDEE_ID) IsNot Nothing Then
                            dtSelectedItem = DirectCast(ViewState(ATTRIBUTE_ATTENDEE_ID), DataTable)
                        End If

                        If dtSelectedItem IsNot Nothing AndAlso dtSelectedItem.Rows.Count > 0 Then
                            FillWaitingList()
                        Else
                            lblError.Text = "Select an Attendee."
                            lblError.Visible = True
                            e.Cancel = True
                        End If

                    Case 2
                        If grdWaitingList.Items.Count = 0 Then
                            e.Cancel = True
                            Exit Select
                        End If
                        SaveCheckedValues("NewAttendee")
                        CreditCard.LoadSavedPayments()
                        Dim AttendeeID As Integer
                        If ViewState(ATTRIBUTE_NEW_ATTENDEEID) IsNot Nothing AndAlso CInt(ViewState(ATTRIBUTE_NEW_ATTENDEEID)) > 0 Then
                            AttendeeID = CInt(ViewState(ATTRIBUTE_NEW_ATTENDEEID))
                        End If
                        If AttendeeID <= 0 Then
                            lblError.Text = "Select New Attendee to Replace."
                            lblError.Visible = True
                            e.Cancel = True
                        Else
                            LoadStepFour()
                        End If

                End Select

                upnlMeetingRegistrant.Update()
                upnlWaitingList.Update()
                upnlUpcomingMeeting.Update()

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub


        Protected Sub WizardMeetingTransfer_FinishButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.WizardNavigationEventArgs) Handles WizardMeetingTransfer.FinishButtonClick
            Try
                Dim MeetingID As Long
                Dim sError As String
                Dim dtSelectedItem As DataTable = Nothing
                lblError.Visible = False

                If ViewState(ATTRIBUTE_PREVIOUS_MEETING) IsNot Nothing Then
                    dtSelectedItem = DirectCast(ViewState(ATTRIBUTE_PREVIOUS_MEETING), DataTable)
                End If
                If dtSelectedItem IsNot Nothing AndAlso dtSelectedItem.Rows.Count > 0 Then
                    MeetingID = CLng(dtSelectedItem.Rows(0).Item("MeetingID"))
                End If

                Dim meeting As String = FindMeeting(CInt(MeetingID))


                Dim AttendeeID As Long = 0
                If ViewState(ATTRIBUTE_ATTENDEE_ID) IsNot Nothing Then
                    dtSelectedItem = DirectCast(ViewState(ATTRIBUTE_ATTENDEE_ID), DataTable)
                End If
                If dtSelectedItem IsNot Nothing AndAlso dtSelectedItem.Rows.Count > 0 Then
                    AttendeeID = CLng(dtSelectedItem.Rows(0).Item("AttendeeID"))
                End If
                Dim firstAttendee As String = FindPersonByID(CInt(AttendeeID))

                Dim NewAttendeeID As Long = 0
                If ViewState(ATTRIBUTE_NEW_ATTENDEEID) IsNot Nothing AndAlso CInt(ViewState(ATTRIBUTE_NEW_ATTENDEEID)) > 0 Then
                    NewAttendeeID = CInt(ViewState(ATTRIBUTE_NEW_ATTENDEEID))
                End If
                If NewAttendeeID = 0 Then
                    lblError.Text = "Select an Attendee to replace."
                    e.Cancel = True
                Else

                    Dim newAttendee As String = FindPersonByID(CInt(NewAttendeeID))
                    If FinishTransferOrder(sError) Then
                        lblFinishmessage.Text = "Person have been replaced"
                        tblTransferConfirmation.Visible = True
                        lblCompleteMsg.Text = newAttendee & " has replaced  " & firstAttendee & " for " & meeting & " Meeting. Find more details below."
                        WizardMeetingTransfer.Visible = False
                        LoadConfirmationHeader()
                    Else
                        e.Cancel = True
                        lblError.Visible = True
                    End If

                End If

                upnlMeetingRegistrant.Update()
                upnlWaitingList.Update()
                upnlUpcomingMeeting.Update()
                dtSelectedItem = Nothing
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            Finally
                ViewState(ATTRIBUTE_DT_MEETING_ORDER) = Nothing
                ViewState(ATTRIBUTE_DT_ATTENDEE) = Nothing
                ViewState(ATTRIBUTE_DT_MEETING_REGISTRATION) = Nothing
                ViewState(ATTRIBUTE_PAID_AMOUNT) = Nothing
                ViewState(ATTRIBUTE_PRICE_DIFFERENCE) = Nothing
            End Try
        End Sub

        Protected Sub WizardMeetingTransfer_SideBarButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.WizardNavigationEventArgs) Handles WizardMeetingTransfer.SideBarButtonClick
            e.Cancel = True
        End Sub

        Protected Sub btnSendMail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSendMail.Click
            Try
                Dim lMeetingTransferID As Long, sSql As String, lMessageTemplateID As Integer
                lMeetingTransferID = SaveMeetingTransferRecord()
                If lMeetingTransferID < 0 Then
                    lblError.Text = "Error while saving Meeting Transfer Emails Record. Please check log"
                Else
                    If chkSendMailtoAttendee.Checked = True Then
                        sSql = "SELECT TOP 1 ID FROM " & Database & "..vwProcessFlows WHERE Name='Send Attendee Transfer Confirmation Email - PreAttendee'"
                        lMessageTemplateID = GetMessageTemplateID("Previous Attendee Transfer Email")
                        SendMail(lMeetingTransferID, sSql, lMessageTemplateID)
                    End If
                    If chkSendMailtoNewAttendee.Checked Then
                        sSql = "SELECT TOP 1 ID FROM " & Database & "..vwProcessFlows WHERE Name='Send Attendee Transfer Confirmation Email - NewAttendee'"
                        lMessageTemplateID = GetMessageTemplateID("New Attendee Transfer Email")
                        SendMail(lMeetingTransferID, sSql, lMessageTemplateID)
                    End If

                End If


            Catch ex As ArgumentException
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub



#End Region

#Region "Private Methods"
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

        Private Function FindPersonByID(ByVal ID As Int32) As String
            Try
                Dim SQL As String
                Dim params(0) As IDataParameter
                Dim DT As Data.DataTable

                SQL = Database & ".." & "spFindPersonByID"
                If ID > 0 Then
                    params(0) = Me.DataAction.GetDataParameter("@ID", SqlDbType.BigInt, ID)
                End If
                ''params(1) = Me.DataAction.GetDataParameter("@CompanyID", SqlDbType.BigInt, User1.CompanyID)
                DT = Me.DataAction.GetDataTableParametrized(SQL, CommandType.StoredProcedure, params)
                If DT IsNot Nothing AndAlso DT.Rows.Count > 0 Then
                    Return CStr(DT.Rows(0).Item("Name"))
                End If
                DT = Nothing
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return ""
            End Try
        End Function
        Private Sub LoadConfirmationHeader()
            Dim sSQL As String, dtOriOder, dtNewOrder, dtSelectedItem As Data.DataTable
            Dim sCurrencyFormat As String
            dtOriOder = Nothing
            dtNewOrder = Nothing
            dtSelectedItem = Nothing
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

                Dim NewAttendeeID As Long = 0
                If ViewState(ATTRIBUTE_NEW_ATTENDEEID) IsNot Nothing AndAlso CInt(ViewState(ATTRIBUTE_NEW_ATTENDEEID)) > 0 Then
                    NewAttendeeID = CInt(ViewState(ATTRIBUTE_NEW_ATTENDEEID))
                End If
                sSQL = "select  o.ID,o.Balance,o.GrandTotal, o.CurrencyTypeID from vwOrders o " _
                       & "WHERE o.ID = " & CStr(OriOrderID)
                dtOriOder = DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)

                If ViewState(ATTRIBUTE_NEW_ORDERID) IsNot Nothing AndAlso CLng(ViewState(ATTRIBUTE_NEW_ORDERID)) > 0 Then
                    sSQL = "select  o.ID,o.Balance,o.GrandTotal, o.CurrencyTypeID from vwOrders o " _
                                         & "WHERE o.ID = " & CStr(ViewState(ATTRIBUTE_NEW_ORDERID))
                    dtNewOrder = DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)
                End If

                lblOriginalAttendee.Text = FindPersonByID(CInt(AttendeeID))
                lblOriginalOrderID.Text = ""
                lblOriOrderBalance.Text = Format$(0, sCurrencyFormat)
                lblOriOrderTotal.Text = Format$(0, sCurrencyFormat)
                lblNewAttendee.Text = FindPersonByID(CInt(NewAttendeeID))
                lblNewOrderID.Text = ""
                lblNewOrderBalance.Text = Format$(0, sCurrencyFormat)
                lblNewOrderTotal.Text = Format$(0, sCurrencyFormat)

                If dtOriOder IsNot Nothing AndAlso dtOriOder.Rows.Count > 0 Then

                    With dtOriOder.Rows(0)

                        If .Item("ID") IsNot Nothing AndAlso Not IsDBNull(.Item("ID")) Then
                            lblOriginalOrderID.Text = CStr(.Item("ID"))
                        End If

                        If IsNumeric(.Item("CurrencyTypeID")) Then
                            sCurrencyFormat = CurrencyCache.CurrencyType(CInt(.Item("CurrencyTypeID"))).FormatString
                        End If
                        If .Item("Balance") IsNot Nothing AndAlso Not IsDBNull(.Item("Balance")) Then
                            lblOriOrderBalance.Text = Format$(CInt(.Item("Balance")), sCurrencyFormat)
                        End If

                        If .Item("GrandTotal") IsNot Nothing AndAlso Not IsDBNull(.Item("GrandTotal")) Then
                            lblOriOrderTotal.Text = Format$(CLng(.Item("GrandTotal")), sCurrencyFormat)
                        End If

                    End With
                End If

                If dtNewOrder IsNot Nothing AndAlso dtNewOrder.Rows.Count > 0 Then

                    With dtNewOrder.Rows(0)

                        If .Item("ID") IsNot Nothing AndAlso Not IsDBNull(.Item("ID")) Then
                            lblNewOrderID.Text = CStr(.Item("ID"))
                        End If

                        If IsNumeric(.Item("CurrencyTypeID")) Then
                            sCurrencyFormat = CurrencyCache.CurrencyType(CInt(.Item("CurrencyTypeID"))).FormatString
                        End If

                        If .Item("Balance") IsNot Nothing AndAlso Not IsDBNull(.Item("Balance")) AndAlso CLng(.Item("Balance")) > 0 Then
                            lblNewOrderBalance.Text = Format$(CInt(dtNewOrder.Rows(0).Item("Balance")), sCurrencyFormat)
                        End If

                        If .Item("GrandTotal") IsNot Nothing AndAlso Not IsDBNull(.Item("GrandTotal")) AndAlso CLng(.Item("GrandTotal")) > 0 Then
                            lblNewOrderTotal.Text = Format$(CLng(.Item("GrandTotal")), sCurrencyFormat)
                        End If

                    End With

                Else
                    If dtOriOder IsNot Nothing AndAlso dtOriOder.Rows.Count > 0 Then
                        With dtOriOder.Rows(0)

                            If .Item("ID") IsNot Nothing AndAlso Not IsDBNull(.Item("ID")) Then
                                lblNewOrderID.Text = CStr(.Item("ID"))
                            End If
                            If IsNumeric(.Item("CurrencyTypeID")) Then
                                sCurrencyFormat = CurrencyCache.CurrencyType(CInt(.Item("CurrencyTypeID"))).FormatString
                            End If

                            If .Item("Balance") IsNot Nothing AndAlso Not IsDBNull(.Item("Balance")) AndAlso CLng(.Item("Balance")) > 0 Then
                                lblNewOrderBalance.Text = Format$(CInt(.Item("Balance")), sCurrencyFormat)
                            End If

                            If .Item("GrandTotal") IsNot Nothing AndAlso Not IsDBNull(.Item("GrandTotal")) AndAlso CLng(.Item("GrandTotal")) > 0 Then
                                lblNewOrderTotal.Text = Format$(CLng(.Item("GrandTotal")), sCurrencyFormat)
                            End If

                        End With
                    End If
                End If


                If Not ViewState(ATTRIBUTE_PAID_AMOUNT) Is Nothing Then
                    lblAmountPaid.Text = ViewState(ATTRIBUTE_PAID_AMOUNT).ToString
                Else
                    lblAmountPaid.Text = Format$(0, sCurrencyFormat)
                End If
                dtNewOrder = Nothing
                dtOriOder = Nothing
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


        Private Sub FillMeetingsOrders()
            Try
                AddExpression()
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
                    WizardMeetingTransfer.Visible = True
                    lblStep1.Text = "Step 1: Select a Meeting/Session"
                    lblStep1Msg.Text = "Meetings/Sessions Registrations"
                    NextButton.Enabled = True
                    If DT.Rows.Count = 1 Then
                        Dim opt As RadioButton = CType(grdUpcomingMeeting.Items(0).FindControl("optSelectMeeting"), RadioButton)
                        opt.Checked = True

                    End If
                    ViewState(ATTRIBUTE_DT_MEETING_ORDER) = DT
                Else
                    lblStep1.Text = "No Upcoming/Ongoing Meeting Available."
                    lblStep1Msg.Text = ""
                    grdUpcomingMeeting.DataSource = DT
                    grdUpcomingMeeting.DataBind()
                    NextButton.Enabled = False
                End If
                DT = Nothing
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub FillMeetingRegistrants(ByVal MeetingID As Integer)
            Try
                AddExpression()
                Dim SQL As String
                Dim params(1) As IDataParameter
                Dim DT As Data.DataTable
                Dim NextButton As Button = CType(WizardMeetingTransfer.FindControl("StepNavigationTemplateContainerID").FindControl("StepNextButton"), Button)

                SQL = Database & ".." & "spMeetingRegistrantsForAttendeeTran"

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
                    lblStep2.Text = "No Attendee Available for Selected Meeting."
                    NextButton.Enabled = False
                    lblMeetingTitle.Visible = False
                    grdMeetingRegistrant.DataSource = DT
                    grdMeetingRegistrant.DataBind()
                End If
                If grdMeetingRegistrant IsNot Nothing AndAlso grdMeetingRegistrant.Items.Count = 1 Then
                    Dim opt As RadioButton = CType(grdMeetingRegistrant.Items(0).FindControl("optSelectAttendee"), RadioButton)
                    opt.Checked = True
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try

        End Sub
        Private Sub FillWaitingList()
            Try
                AddExpression()
                Dim SQL As String
                Dim params1(0), params2(1) As IDataParameter
                Dim DT, dAttendeeTable, dtSelectedItem As Data.DataTable
                Dim NextButton As Button = CType(WizardMeetingTransfer.FindControl("StepNavigationTemplateContainerID").FindControl("StepNextButton"), Button)
                dAttendeeTable = New DataTable
                dtSelectedItem = Nothing
                dAttendeeTable.Columns.Add("Photo", Type.GetType("System.Byte[]"))
                dAttendeeTable.Columns.Add("AttendeeID", Type.GetType("System.Int32"))
                dAttendeeTable.Columns.Add("AttendeeID_FirstLast", Type.GetType("System.String"))
                dAttendeeTable.Columns.Add("AttendeeStatus_Name", Type.GetType("System.String"))
                dAttendeeTable.Columns.Add("StatusID", Type.GetType("System.Int32"))
                dAttendeeTable.Columns.Add("OrderID", Type.GetType("System.Int32"))
                dAttendeeTable.Columns.Add("CompanyID", Type.GetType("System.Int32"))
                dAttendeeTable.Columns.Add("OrderDetailID", Type.GetType("System.Int32"))
                dAttendeeTable.Columns.Add("City", Type.GetType("System.String"))
                dAttendeeTable.Columns.Add("Title", Type.GetType("System.String"))


                Dim MeetingID As Long = 0
                If ViewState(ATTRIBUTE_PREVIOUS_MEETING) IsNot Nothing Then
                    dtSelectedItem = DirectCast(ViewState(ATTRIBUTE_PREVIOUS_MEETING), DataTable)
                End If
                If dtSelectedItem IsNot Nothing AndAlso dtSelectedItem.Rows.Count > 0 Then
                    MeetingID = CLng(dtSelectedItem.Rows(0).Item("MeetingID"))
                End If

                SQL = Database & ".." & "spWaitingList"

                params2(0) = Me.DataAction.GetDataParameter("@CompanyID", SqlDbType.Int, User1.CompanyID)
                params2(1) = Me.DataAction.GetDataParameter("@MeetingID", SqlDbType.Int, CInt(MeetingID))
                DT = Me.DataAction.GetDataTableParametrized(SQL, CommandType.StoredProcedure, params2)
                If DT IsNot Nothing AndAlso DT.Rows.Count > 0 Then

                    For Each row As DataRow In DT.Rows

                        Dim dr As DataRow = dAttendeeTable.NewRow
                        dr.Item("AttendeeID") = row.Item("AttendeeID")
                        dr.Item("AttendeeID_FirstLast") = row.Item("AttendeeID_FirstLast")
                        dr.Item("Title") = row.Item("Title")
                        dr.Item("City") = row.Item("City")
                        dr.Item("Photo") = row.Item("Photo")
                        dAttendeeTable.Rows.Add(dr)

                    Next
                    NextButton.Enabled = True
                    lblStep3ReplaceAttendee.Text = "Step 3: Replace Attendee"
                    lblStep3.Visible = True
                Else
                    lblStep3ReplaceAttendee.Text = "No Attendee Available."
                    NextButton.Enabled = False
                    lblStep3.Visible = False
                End If
                If dAttendeeTable.Rows.Count > 0 Then
                    ViewState(ATTRIBUTE_DT_ATTENDEE) = dAttendeeTable
                    grdWaitingList.DataSource = dAttendeeTable
                    grdWaitingList.DataBind()
                Else
                    grdWaitingList.DataSource = dAttendeeTable
                    grdWaitingList.DataBind()
                End If
                DT = Nothing
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub LoadStepFour()

            Try
                Dim dtSelectedItem As DataTable = Nothing
                Dim FirstAttendee, NewAttendee, Meeting As String
                Dim MeetingID As Long = 0
                Dim AttendeeID As Long = 0
                Dim NewAttendeeID As Long = 0

                If ViewState(ATTRIBUTE_PREVIOUS_MEETING) IsNot Nothing Then
                    dtSelectedItem = DirectCast(ViewState(ATTRIBUTE_PREVIOUS_MEETING), DataTable)
                End If
                If dtSelectedItem IsNot Nothing AndAlso dtSelectedItem.Rows.Count > 0 Then
                    MeetingID = CLng(dtSelectedItem.Rows(0).Item("MeetingID"))
                End If

                If ViewState(ATTRIBUTE_ATTENDEE_ID) IsNot Nothing Then
                    dtSelectedItem = DirectCast(ViewState(ATTRIBUTE_ATTENDEE_ID), DataTable)
                End If
                If dtSelectedItem IsNot Nothing AndAlso dtSelectedItem.Rows.Count > 0 Then
                    AttendeeID = CLng(dtSelectedItem.Rows(0).Item("AttendeeID"))
                End If

                If ViewState(ATTRIBUTE_NEW_ATTENDEEID) IsNot Nothing AndAlso CInt(ViewState(ATTRIBUTE_NEW_ATTENDEEID)) > 0 Then
                    NewAttendeeID = CInt(ViewState(ATTRIBUTE_NEW_ATTENDEEID))
                End If

                FirstAttendee = FindPersonByID(CInt(AttendeeID))
                NewAttendee = FindPersonByID(CInt(NewAttendeeID))
                Meeting = FindMeeting(CInt(MeetingID))

                lblFinishmessage.Text = "You are replacing " & FirstAttendee & " with " & NewAttendee & " for this meeting: " & Meeting & ". If this is correct, please click Finish to continue."
                SetTransferWizardObject()
                ValidateforBalance()
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try

        End Sub
        Private Sub ValidateforBalance()
            Try

                Dim lPricedifference As Decimal = 0
                Dim oWizObj As ScheduledMeetingTransferWizObject = Nothing

                oWizObj = SetTransferWizardObject()
                If oWizObj IsNot Nothing Then

                    If oWizObj.TransferOrderCreditAmount > 0 Then
                        lblNewPrice.Text = "Price Difference Amount: " & Format(oWizObj.TransferOrderCreditAmount, oWizObj.CurrencyFormatString)
                        lblBalance.Text = "Price Difference Amount: " & Format(oWizObj.TransferOrderCreditAmount, oWizObj.CurrencyFormatString)
                        CreditcardWindow.VisibleOnPageLoad = True
                        ViewState(ATTRIBUTE_PAID_AMOUNT) = Format(oWizObj.TransferOrderCreditAmount, oWizObj.CurrencyFormatString)
                    ElseIf oWizObj.TransferOrderCreditAmount < 0 Then

                        lblNewPrice.Text = "You have a credit of " & Format(oWizObj.TransferOrderCreditAmount, oWizObj.CurrencyFormatString) & ", which we will keep on account for your company"
                        lblBalance.Text = Format(oWizObj.TransferOrderCreditAmount, oWizObj.CurrencyFormatString)
                        CreditcardWindow.VisibleOnPageLoad = False
                        chkMakePayment.Checked = False
                        ViewState(ATTRIBUTE_PAID_AMOUNT) = Nothing
                    End If

                End If


            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try


        End Sub


        Private Function FinishTransferOrder(ByRef sError As String) As Boolean
            Try
                Dim oWizObject As ScheduledMeetingTransferWizObject
                Dim bSuccess As Boolean
                Dim lPricedifference As Decimal = 0

                oWizObject = SetTransferWizardObject()

                If oWizObject IsNot Nothing Then
                    lPricedifference = oWizObject.TransferOrderCreditAmount
                    bSuccess = oWizObject.TransferOrder()
                End If


                If bSuccess Then
                    ViewState(ATTRIBUTE_NEW_ORDERID) = CStr(oWizObject.TransferOrderGE.ID)
                    If chkMakePayment.Checked AndAlso lPricedifference > 0 Then
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
                Else
                    oPayment.SetValue("PaymentTypeID", CreditCard.PaymentTypeID)
                    oPayment.SetValue("CCAccountNumber", CreditCard.CCNumber)
                    oPayment.SetValue("CCExpireDate", CreditCard.CCExpireDate)
                    oPayment.SetValue("PaymentLevelID", User1.GetValue("GLPaymentLevelID"))
                    oPayment.SetValue("Comments", "Created through the CGI e-Business Suite")
                    'Anil B change for 10254 on 23/04/2013
                    'Set reference transaction for payment
                    If CreditCard.CCNumber = "-Ref Transaction-" Then
                        oPayment.SetValue("ReferenceTransactionNumber", CreditCard.ReferenceTransactionNumber)
                        oPayment.SetValue("ReferenceExpiration", CreditCard.ReferenceExpiration)
                    End If
                    'Neha, Issue 16675, Set temperory variable for CCsecuritynumber of  Accounting->paymentinformation tab to not stored in record history.
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
        Private Sub SendMail(ByVal lMeetingTransferID As Long, ByVal sSql As String, ByVal lMessageTemplateID As Integer)
            Try

                If lMeetingTransferID <> 0 Then

                    Dim lProcessFlowID As Long = CLng(DataAction.ExecuteScalar(sSql, IAptifyDataAction.DSLCacheSetting.UseCache))

                    Dim context As New AptifyContext

                    context.Properties.AddProperty("MeetingTransferID", lMeetingTransferID)

                    If lMessageTemplateID = 0 Then
                        lblError.Text = "Message Template does not exist."
                    Else
                        context.Properties.AddProperty("MessageTemplateID", lMessageTemplateID)
                        Dim result As ProcessFlowResult
                        result = ProcessFlowEngine.ExecuteProcessFlow(Me.AptifyApplication, lProcessFlowID, context)
                        If result.IsSuccess Then
                            SendEmailLabel.ForeColor = Drawing.Color.Blue
                            SendEmailLabel.Visible = True
                            SendEmailLabel.Text = "Email has been sent successfully."
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


        Private Function SaveMeetingTransferRecord() As Long
            Try
                Dim MeetingID As Long
                Dim rowIndex As Integer
                Dim dtSelectedItem As DataTable = Nothing
                MeetingID = 0
                Dim AttendeeID As Long = 0
                Dim OriOrderID As Long = 0
                Dim StatusID As Integer = 0
                Dim OriOrderLineID As Long = 0
                Dim NewAttendeeID As Long = 0

                If ViewState(ATTRIBUTE_PREVIOUS_MEETING) IsNot Nothing Then
                    dtSelectedItem = DirectCast(ViewState(ATTRIBUTE_PREVIOUS_MEETING), DataTable)
                End If
                If dtSelectedItem IsNot Nothing AndAlso dtSelectedItem.Rows.Count > 0 Then
                    MeetingID = CLng(dtSelectedItem.Rows(0).Item("MeetingID"))
                End If


                If ViewState(ATTRIBUTE_ATTENDEE_ID) IsNot Nothing Then
                    dtSelectedItem = DirectCast(ViewState(ATTRIBUTE_ATTENDEE_ID), DataTable)
                End If
                If dtSelectedItem IsNot Nothing AndAlso dtSelectedItem.Rows.Count > 0 Then
                    AttendeeID = CLng(dtSelectedItem.Rows(0).Item("AttendeeID"))
                    OriOrderID = CLng(dtSelectedItem.Rows(0).Item("OriOrderID"))
                    StatusID = CInt(dtSelectedItem.Rows(0).Item("StatusID"))
                    OriOrderLineID = CLng(dtSelectedItem.Rows(0).Item("OriOrderLineID"))
                End If
                If ViewState(ATTRIBUTE_NEW_ATTENDEEID) IsNot Nothing AndAlso CInt(ViewState(ATTRIBUTE_NEW_ATTENDEEID)) > 0 Then
                    NewAttendeeID = CInt(ViewState(ATTRIBUTE_NEW_ATTENDEEID))
                End If

                Dim oMeetingTransfer As AptifyGenericEntityBase
                oMeetingTransfer = AptifyApplication.GetEntityObject("Meeting Transfer Emails", -1)
                oMeetingTransfer.SetValue("PreAttendeeID", CInt(AttendeeID))
                oMeetingTransfer.SetValue("NewAttendeeID", CInt(NewAttendeeID))
                oMeetingTransfer.SetValue("AdminID", CInt(User1.PersonID))
                oMeetingTransfer.SetValue("CancelledOrderID", CInt(OriOrderID))
                If ViewState(ATTRIBUTE_NEW_ORDERID) IsNot Nothing AndAlso CInt(ViewState(ATTRIBUTE_NEW_ORDERID)) > 0 Then
                    oMeetingTransfer.SetValue("NewOrderID", CInt(ViewState(ATTRIBUTE_NEW_ORDERID)))
                Else
                    oMeetingTransfer.SetValue("NewOrderID", CInt(OriOrderID))
                End If
                oMeetingTransfer.SetValue("MeetingID", CInt(MeetingID))
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
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
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
            End Try

        End Sub

        ''Funtion returns ScheduledMeetingTransferWiz Object
        Protected Overridable Function SetTransferWizardObject() As ScheduledMeetingTransferWizObject
            Dim OrigOrderGE As OrdersEntity
            Dim IsSession As Boolean
            Dim lParentID As Long
            Dim lPricedifference As Long = 0
            Dim sError As String = ""
            Dim MeetingID As Long = 0
            Dim AttendeeID As Long = 0
            Dim OriOrderID As Long = 0
            Dim StatusID As Integer = 0
            Dim OriOrderLineID As Long = 0
            Dim NewAttendeeID As Long = 0
            Dim dtSelectedItem As DataTable = Nothing
            Try


                If m_oWizObject Is Nothing Then
                    m_oWizObject = New ScheduledMeetingTransferWizObject(Me.AptifyApplication)
                    With m_oWizObject
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

                        If ViewState(ATTRIBUTE_ATTENDEE_ID) IsNot Nothing Then
                            dtSelectedItem = DirectCast(ViewState(ATTRIBUTE_ATTENDEE_ID), DataTable)
                        End If
                        If dtSelectedItem IsNot Nothing AndAlso dtSelectedItem.Rows.Count > 0 Then
                            AttendeeID = CLng(dtSelectedItem.Rows(0).Item("AttendeeID"))
                            OriOrderID = CLng(dtSelectedItem.Rows(0).Item("OriOrderID"))
                            StatusID = CInt(dtSelectedItem.Rows(0).Item("StatusID"))
                            OriOrderLineID = CLng(dtSelectedItem.Rows(0).Item("OriOrderLineID"))
                        End If

                        If ViewState(ATTRIBUTE_NEW_ATTENDEEID) IsNot Nothing AndAlso CInt(ViewState(ATTRIBUTE_NEW_ATTENDEEID)) > 0 Then
                            NewAttendeeID = CInt(ViewState(ATTRIBUTE_NEW_ATTENDEEID))
                        End If

                        OrigOrderGE = (CType(AptifyApplication.GetEntityObject("Orders", OriOrderID), OrdersEntity))
                        .OriginalAttendeeID = AttendeeID
                        .OriginalMeetingID = MeetingID
                        .FirstAttendeeID = NewAttendeeID
                        .FirstMeetingID = MeetingID
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
                                oLine.NewAttendeeID = NewAttendeeID
                                oLine.NewMeetingID = MeetingID
                                oLine.ProductID = MeetingID
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

                Return m_oWizObject
                sError = m_oWizObject.LastError
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Function

        Private Sub SaveCheckedValues(ByVal Type As String)
            Dim dtPreMeeting, dtAttendee As DataTable
            Dim NewAttendeeID As String = ""

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
            If ViewState(ATTRIBUTE_NEW_ATTENDEEID) IsNot Nothing Then
                NewAttendeeID = CStr(ViewState(ATTRIBUTE_NEW_ATTENDEEID))
            End If
            Dim dr As DataRow = dtPreMeeting.NewRow
            Dim lblMeetingID As Label = New Label
            Dim lblAttendeeID As Label = New Label
            Select Case Type
                Case "Meeting"
                    If grdUpcomingMeeting.Items.Count > 0 Then

                        For Each item As GridDataItem In grdUpcomingMeeting.MasterTableView.Items
                            lblMeetingID = CType(item.FindControl("lblMeetingID"), Label)
                            index = lblMeetingID.Text
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
                Case "PreAttendee"
                    dr = dtAttendee.NewRow
                    If grdMeetingRegistrant.Items.Count > 0 Then
                        For Each item As GridDataItem In grdMeetingRegistrant.MasterTableView.Items
                            lblAttendeeID = CType(item.FindControl("lblAttendeeID"), Label)
                            index = lblAttendeeID.Text
                            'index = item("AttendeeID").Text
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

                Case "NewAttendee"
                    Dim lblNewAttendeeID As Label = New Label
                    If grdWaitingList.Items.Count > 0 Then
                        For Each item As GridDataItem In grdWaitingList.MasterTableView.Items
                            lblNewAttendeeID = CType(item.FindControl("lblNewAttendeeID"), Label)
                            index = lblNewAttendeeID.Text
                            'index = item("AttendeeID").Text
                            Dim result As Boolean = DirectCast(item.FindControl("optSelectNewAttendee"), RadioButton).Checked
                            If result Then
                                'NewAttendeeID = index
                                If CBool(String.Compare(NewAttendeeID, index)) Then
                                    NewAttendeeID = index
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
            If NewAttendeeID IsNot Nothing AndAlso NewAttendeeID <> "" Then
                ViewState(ATTRIBUTE_NEW_ATTENDEEID) = NewAttendeeID
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
            expression.FieldName = "AttendeeID_FirstLast"
            expression.SetSortOrder("Ascending")
            grdWaitingList.MasterTableView.SortExpressions.AddSortExpression(expression)

        End Sub

        Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            CreditcardWindow.VisibleOnPageLoad = False
            ClearCreditCardControl()
            ViewState(ATTRIBUTE_PAID_AMOUNT) = Nothing
            WizardMeetingTransfer_ActiveStepChanged(Nothing, Nothing)
        End Sub

        Protected Sub WizardMeetingTransfer_PreviousButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.WizardNavigationEventArgs) Handles WizardMeetingTransfer.PreviousButtonClick
            ClearCreditCardControl()
            Dim icurrentStep As Int32
            icurrentStep = WizardMeetingTransfer.ActiveStepIndex
            Select Case (icurrentStep)
                Case 0
                    SaveCheckedValues("Meeting")
                Case 1
                    SaveCheckedValues("PreAttendee")
                Case 2
                    SaveCheckedValues("NewAttendee")
            End Select
        End Sub

#End Region


    End Class


End Namespace


