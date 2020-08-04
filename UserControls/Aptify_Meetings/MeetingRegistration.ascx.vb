'Aptify e-Business 5.5.1, July 2013
' IMPORTANT: If you're sessions aren't saving see the SaveSessions method and make sure you
'             are referencing the outermost html element as a server control.

Option Explicit On
Option Strict On

Imports System.Data
Imports Aptify.Framework.Web.eBusiness
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Applications.OrderEntry
Imports Telerik.Web.UI
Imports System.Web.UI.Control
Imports Aptify.Framework.DataServices
Imports System.Collections.Generic


Namespace Aptify.Framework.Web.eBusiness.ProductCatalog
    Partial Class MeetingRegistrationControl
        Inherits BaseUserControlAdvanced
        Protected Const ATTRIBUTE_MEETING_PAGE As String = "MeetingPage"
        Protected Const ATTRIBUTE_VIEW_SESSION_WITH_EXPECTED_PARAMETER_PAGE As String = "ViewSessionPageWithExpectedParameter"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "MeetingRegistration"
        Protected Const ATTRIBUTE_CONFIRMATION_PAGE As String = "OrderConfirmationPage"
        Protected Const ATTRIBUTE_LOGIN_PAGE As String = "LoginPage"
        Dim sAttendeeFistName As String = ""
        Dim sAttendeeLastName As String = ""
        ''ISSUEID 3240 - Added by SuvarnaD Alert which says duplicate record.  
        Protected Const ATTRIBUTE_ALERT_DUPLICATEPERSONVALIDATION As String = "DuplicatePersonValidation"
        Protected m_sAlert As String = String.Empty
        Dim descSelectedSessionForAttendee As New Dictionary(Of Integer, List(Of Integer))
        Dim dtAttendeeInfo As New DataTable
        Protected Const ATTRIBUTE_ATTENDEE_SESSION As String = "dtAttendeeInfo"
        Protected Const ATTRIBUTE_ATTENDEE_SELECTEDMEETING As String = "SelectedMeetingSession"
        Protected Const ATTRIBUTE_ATTENDEE_MEETINGPRODUCTID As String = "MeetingProductID"
        Protected Const ATTRIBUTE_ALLSESSION = "Sessiondt"
        Protected Const ATTRIBUTE_CHECKED_SESSION As String = "MeetingSessionsCheckedvalues"
        Protected Const ATTRIBUTE_SESSION_DT As String = "dtSessionItems"
        Protected Const ATTRIBUTE_SESION_STATUS As String = "SessionStatus"




#Region "MeetingRegistration Specific Properties"
        Private iInsertedData As Integer = 0
        Public Property InsertedData() As Integer
            Get
                Return iInsertedData
            End Get
            Set(ByVal Value As Integer)
                iInsertedData = Value
            End Set
        End Property
        ''' <summary>
        ''' ViewSessionPageWithExpectedParameter page url
        ''' </summary>
        Public Overridable Property ViewSessionPageWithExpectedParameter() As String
            Get
                If Not ViewState(ATTRIBUTE_VIEW_SESSION_WITH_EXPECTED_PARAMETER_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_VIEW_SESSION_WITH_EXPECTED_PARAMETER_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_VIEW_SESSION_WITH_EXPECTED_PARAMETER_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        Private Property AddAnother() As Boolean
            Get
                If ViewState.Item("AddAnother") IsNot Nothing Then
                    Return CBool(ViewState.Item("AddAnother"))
                Else
                    Return False
                End If
            End Get
            Set(ByVal value As Boolean)
                ViewState.Item("AddAnother") = value
            End Set
        End Property

        Public Overridable Property MeetingPage() As String
            Get
                If Not ViewState(ATTRIBUTE_MEETING_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_MEETING_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_MEETING_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' RashmiP, OrderConfirmation page url
        ''' </summary>
        Public Overridable Property OrderConfirmationPage() As String
            Get
                If Not ViewState(ATTRIBUTE_CONFIRMATION_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_CONFIRMATION_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_CONFIRMATION_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
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
        ''' ''' ''ISSUEID 3240 - Added SuvarnaD to display alert in case of duplicate record
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
#End Region

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(ViewSessionPageWithExpectedParameter) Then
                'since value is the 'default' check the XML file for possible custom setting
                ViewSessionPageWithExpectedParameter = Me.GetLinkValueFromXML(ATTRIBUTE_VIEW_SESSION_WITH_EXPECTED_PARAMETER_PAGE)
            End If
            If String.IsNullOrEmpty(OrderConfirmationPage) Then
                OrderConfirmationPage = Me.GetLinkValueFromXML(ATTRIBUTE_CONFIRMATION_PAGE)
            End If
            If String.IsNullOrEmpty(LoginPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                LoginPage = Me.GetLinkValueFromXML(ATTRIBUTE_LOGIN_PAGE)
            End If
            If String.IsNullOrEmpty(MeetingPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                MeetingPage = Me.GetLinkValueFromXML(ATTRIBUTE_MEETING_PAGE)
                If String.IsNullOrEmpty(MeetingPage) Then
                    'Me.grdMeetings.Enabled = False
                    'Me.grdMeetings.ToolTip = "MeetingPage property has not been set."
                End If
            End If
            ''ISSUEID 3240 - Added SuvarnaD to display alert in case of duplicate record 
            If String.IsNullOrEmpty(DuplicatePersonValidation) Then
                DuplicatePersonValidation = Me.GetPropertyValueFromXML(ATTRIBUTE_ALERT_DUPLICATEPERSONVALIDATION)
                If Not String.IsNullOrEmpty(DuplicatePersonValidation) Then
                    lblAlert.Text = DuplicatePersonValidation
                End If
            End If
        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            'ViewState("SelectedMeetingSession") = descSelectedSessionForAttendee
            'Amruta IssueID:14817
            'Amruta IssueID 14380
            Dim dtAttendeStatus As DataTable = Nothing
            SetProperties()
            If Not IsPostBack Then
                lblAddedAtendee.Visible = False
                If User1.UserID > 0 Then
                    If Request.QueryString("ID") IsNot Nothing AndAlso CInt(Request.QueryString("ID")) <> -1 AndAlso IsNumeric(Request.QueryString("ID")) Then
                        Session("OL") = Request.QueryString("ID").ToString()
                        ViewState("MeetingID") = Request.QueryString("ID").ToString()
                        'Added for IssueID 14817
                        GetMeetingInfo()

                        ShowRegistrationButton(True)
                        btnAddRegistrant.Visible = False
                    Else
                        If Request.QueryString("OL") IsNot Nothing AndAlso CInt(Request.QueryString("OL")) <> -1 AndAlso IsNumeric(Request.QueryString("OL")) Then
                            Dim iLine As Integer = CInt(Request.QueryString("OL"))
                            GetOrdrlineDetails(iLine)
                            ShowRegistrationButton(False)
                        End If
                    End If
                    SetInitialColumn()
                    FillDropDown()
                    'Amruta IssueID 14380,19/3/2013,To get user meeting status
                    dtAttendeStatus = LoadUserMeetingStatus()
                    If dtAttendeStatus IsNot Nothing Then
                        ClearText()
                    End If
                    Dim dicMeetingSessions As Dictionary(Of Integer, Boolean)
                    If ViewState(ATTRIBUTE_CHECKED_SESSION) Is Nothing Then
                        If DirectCast(Session("SessionStatus"), Dictionary(Of Integer, Boolean)) IsNot Nothing Then


                            dicMeetingSessions = DirectCast(Session("SessionStatus"), Dictionary(Of Integer, Boolean))


                            ViewState(ATTRIBUTE_CHECKED_SESSION) = dicMeetingSessions
                            Dim dicMeetingStatus As New Dictionary(Of Integer, Boolean)
                            Dim i As Integer = 0
                            For Each skey As KeyValuePair(Of Integer, Boolean) In dicMeetingSessions
                                If dicMeetingStatus.ContainsKey(i) Then
                                    dicMeetingStatus.Item(i) = skey.Value
                                Else
                                    dicMeetingStatus.Add(i, skey.Value)
                                    i = i + 1
                                End If


                            Next
                            If ViewState(ATTRIBUTE_SESION_STATUS) Is Nothing Then
                                ViewState(ATTRIBUTE_SESION_STATUS) = dicMeetingStatus
                            End If



                        End If

                        LoadMeetingSession()
                    End If
                Else
                    'Suraj Issue 15370,7/31/13 revert back changes application variale to session variable
                    Session("ReturnToPage") = Request.RawUrl
                    ' Suraj S Issue 15370, 8/1/13 here we are getting the ReturnToPageURL in "URL" QueryString and passing on login page. 
                    Response.Redirect(LoginPage + "?ReturnURL=" & System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(Request.RawUrl)))
                End If
            End If
        End Sub
        Private Sub ShowRegistrationButton(ByVal bValue As Boolean)
            btnAddInfo.Visible = bValue
            btnAddRegistrant.Visible = bValue
            btnUpdateAttendeeInfo.Visible = Not bValue
        End Sub
        Protected Overridable Sub GetOrdrlineDetails(ByVal iLine As Integer)
            Dim oOrderLine As OrderLinesEntity
            Dim oMeetingDetail As AptifyGenericEntityBase
            Dim oOrderGE As OrdersEntity
            Dim ProductGE As Aptify.Applications.ProductSetup.ProductObject
            oOrderGE = Me.ShoppingCart1.GetOrderObject(Session, Page.User, Application)
            oOrderLine = CType(oOrderGE.SubTypes("OrderLines").Item(iLine), OrderLinesEntity)
            If oOrderLine IsNot Nothing Then
                Dim iProductID As Long = CLng(oOrderLine.GetValue("ProductID"))
                Session("iProductID") = iProductID
                ViewState(ATTRIBUTE_ATTENDEE_MEETINGPRODUCTID) = iProductID
                If iProductID > 0 Then
                    If "Meeting" = ShoppingCart1.GetProductType(iProductID) Then
                        ProductGE = CType(AptifyApplication.GetEntityObject("Products", iProductID), Aptify.Applications.ProductSetup.ProductObject)
                        oMeetingDetail = oOrderLine.ExtendedOrderDetailEntity
                        lblMeeting.Text = GetProductWebName(iProductID)
                        'lblSelectedMeeting.Text = GetProductWebName(iProductID)
                        lblPrice.Text = Format(oOrderLine.GetValue("Price"), oOrderGE.CurrencyTypeFormat)

                        ''RashmiP,
                        If ProdInventoryLedgerExist(iProductID) Then
                            Dim lAvailableSpace As Integer = GetAvailableSpace(iProductID)
                            lblAvailableSpaceText.Text = "Available Space:"
                            lblAvailableSpace.Text = CStr(lAvailableSpace)
                            If lAvailableSpace = 0 Then
                                lblAvailableSpace.ForeColor = Drawing.Color.Red
                                lblMessage.Text = "Attendee register will be added to the Wait List."
                            Else
                                lblAvailableSpace.ForeColor = Drawing.Color.Green
                                lblMessage.Text = "Attendee register will be added to the Registered List."
                            End If
                        End If
                        If oMeetingDetail IsNot Nothing Then
                            Me.LoadMeetingDetailFromXML(oMeetingDetail, CStr(oOrderLine.GetValue("__ExtendedAttributeObjectData")))
                            Dim sAttendeeID As String, sSQL As String
                            Dim lAttendeeID As Long

                            sAttendeeID = CStr(oMeetingDetail.GetValue("AttendeeID"))
                            If Len(sAttendeeID) > 0 AndAlso IsNumeric(sAttendeeID) Then
                                lAttendeeID = CLng(sAttendeeID)
                            End If
                            If lAttendeeID > 0 Then
                                sSQL = "SELECT FirstName, LastName, FirstLast, Email1 FROM " & _
                                       Database & _
                                       "..vwPersons WHERE ID=" & lAttendeeID
                                Dim dt As DataTable = DataAction.GetDataTable(sSQL)
                                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                                    txtFirstName.Text = Trim(CStr(dt.Rows(0)("FirstName")))
                                    txtLastName.Text = Trim(CStr(dt.Rows(0)("LastName")))
                                    txtEmail.Text = Trim(CStr(dt.Rows(0)("Email1")))
                                Else
                                    txtFirstName.Text = User1.GetValue("FirstName")
                                    txtLastName.Text = User1.GetValue("LastName")
                                    txtEmail.Text = User1.GetValue("Email1")
                                End If
                            End If
                            txtBadgeName.Text = CStr(oMeetingDetail.GetValue("BadgeName"))
                            txtBadgeTitle.Text = CStr(oMeetingDetail.GetValue("BadgeTitle"))
                            txtCompany.Text = CStr(oMeetingDetail.GetValue("BadgeCompanyName"))
                            ddlFoodPreference.SelectedValue = CStr(oMeetingDetail.GetValue("FoodPreferenceID"))
                            ddlTravelPreference.SelectedValue = CStr(oMeetingDetail.GetValue("TravelPreferenceID"))
                            txtGolfHandicape.Text = CStr(oMeetingDetail.GetValue("GolfHandicap"))
                            txtOtherPreference.Text = CStr(oMeetingDetail.GetValue("Other"))
                            txtSpecialRequest.Text = CStr(oMeetingDetail.GetValue("SpecialRequest"))
                        End If
                    End If
                End If
            End If
        End Sub
        'Navin Prasad Issue 10944
        Private Sub RefreshSession(ByVal AttendeeID As Long)
            Dim lProductID As Long
            Dim iLine As Integer
            Dim oOrder As OrdersEntity
            Dim oOrderLine As OrderLinesEntity
            Dim ProductGE As Aptify.Applications.ProductSetup.ProductObject

            'Commented 22/11/2012
            ' iLine = CInt(Session("OL"))

            'Added for IssueId:14817
            If ViewState("ILine") IsNot Nothing AndAlso CInt(ViewState("ILine")) <> -1 Then
                iLine = CInt(ViewState("ILine"))
            End If

            oOrder = ShoppingCart1.GetOrderObject(Me.Session, Page.User, Me.Application)
            If oOrder.SubTypes("OrderLines") IsNot Nothing Then
                If oOrder.SubTypes("OrderLines").Count > 0 Then
                    oOrderLine = CType(oOrder.SubTypes("OrderLines").Item(iLine), OrderLinesEntity)
                    lProductID = CLng(oOrderLine.GetValue("ProductID"))
                End If
            End If
            If "Meeting" = ShoppingCart1.GetProductType(lProductID) Then
                ProductGE = CType(AptifyApplication.GetEntityObject("Products", lProductID), Aptify.Applications.ProductSetup.ProductObject)
                MatchAndDeleteSession(oOrder, lProductID, AttendeeID)
            Else
                lblError.Text = "The selected line does not exist or is not a meeting product"
                lblError.Visible = True
                tblInner.Visible = False
            End If

        End Sub

        Protected Overridable Sub MatchAndDeleteSession(ByVal OrderGE As OrdersEntity, ByVal ProductID As Long, ByVal AttendeeID As Long)
            Try
                Dim sSQL As String, dt As DataTable
                'Anil Issue 14381
                'trSessions.Visible = False
                sSQL = "SELECT p.ID,m.StartDate,m.EndDate,p.WebName,m.Place,p.WebDescription FROM " & _
                           Me.AptifyApplication.GetEntityBaseDatabase("Orders") & _
                           "..vwMeetings m INNER JOIN " & _
                           Me.AptifyApplication.GetEntityBaseDatabase("Orders") & _
                           "..vwProducts p ON m.ProductID=p.ID WHERE " & _
                           "dbo.fnProductLevelsBelow(p.ID," & ProductID & ")>0 AND " & _
                           "p.IsSold=1 ORDER BY m.StartDate,m.EndDate,p.WebName"
                dt = DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                ShoppingCart1.SaveCart(Session)
                OrderGE = ShoppingCart1.GetOrderObject(Session, Page.User, Page.Application)
                If dt.Rows.Count > 0 Then
                    For Each rw As DataRow In dt.Rows
                        Try
                            Dim bDone As Boolean = False, i As Integer, ol As OrderLinesEntity
                            i = 0
                            If OrderGE IsNot Nothing AndAlso OrderGE.SubTypes("OrderLines") IsNot Nothing Then
                                While i < OrderGE.SubTypes("OrderLines").Count
                                    ol = CType(OrderGE.SubTypes("OrderLines").Item(i), OrderLinesEntity)
                                    Dim oMeetingDetail As ExtendedOrderDetailGE
                                    oMeetingDetail = CType(ol, OrderLinesEntity).ExtendedOrderDetailEntity
                                    If oMeetingDetail IsNot Nothing AndAlso oMeetingDetail.GetValue("AttendeeID") IsNot Nothing Then
                                        If AttendeeID = CLng(oMeetingDetail.GetValue("AttendeeID")) Then
                                            If String.Compare(CStr(rw("ID")), CStr(ol.GetValue("ProductID"))) = 0 Then
                                                OrderGE.SubTypes("OrderLines").Remove(ol)
                                            End If
                                        End If
                                    End If
                                    i += 1
                                End While
                            End If

                        Catch ex As Exception
                            Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                        End Try
                    Next
                    ShoppingCart1.SaveCart(Session)
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        ' 23/11/2012 To get MeetingInfo new function IssueID(14817)
        Protected Overridable Sub GetMeetingInfo()
            'Dim ProductId As Long = CType(Request.QueryString("ID"), Long)
            Dim ProductId As Long = CType(Session("OL"), Long)
            ViewState(ATTRIBUTE_ATTENDEE_MEETINGPRODUCTID) = ProductId
            Try
                If ProductId <> -1 Then
                    lblMeeting.Text = GetProductWebName(ProductId)
                    Dim oPrice As IProductPrice.PriceInfo = Me.GetProductPrice(CLng(ProductId))
                    lblPrice.Text = Format(oPrice.Price, ShoppingCart1.GetCurrencyFormat(oPrice.CurrencyTypeID))
                    Dim lAvailableSpace As Integer = GetAvailableSpace(ProductId)


                    If ProdInventoryLedgerExist(ProductId) Then
                        lblAvailableSpaceText.Text = "Available Space:"
                        lblAvailableSpace.Text = CStr(lAvailableSpace)
                        If CInt(lAvailableSpace) > 0 Then
                            lblAvailableSpace.ForeColor = Drawing.Color.Green
                            lblMessage.Text = "You can register up to " & CStr(lAvailableSpace) & " people for the meeting. Any additional registrations will be added to the Wait List."
                            'Anil B for issue 15133 on 19-03-2013
                            'Set highlighted text normal when Registration available
                            lblMessage.Font.Bold = False
                        Else
                            lblAvailableSpace.ForeColor = Drawing.Color.Red
                            lblMessage.Text = "There is no space currently available for this meeting. Any attendee you register will be added to the Wait List."
                            'Anil B for issue 15133 on 19-03-2013
                            'Highlight the Text When Registration Is Full
                            lblMessage.Font.Bold = True
                        End If
                    End If

                    If User1.PersonID <> -1 Then
                        txtFirstName.Text = User1.FirstName
                        txtLastName.Text = User1.LastName
                        txtEmail.Text = User1.Email
                        txtBadgeName.Text = User1.FirstName
                        txtBadgeTitle.Text = User1.Title
                        txtCompany.Text = User1.Company
                    End If
                Else
                    lblError.Text = "The selected line does not exist or is not a meeting product."
                    lblError.Visible = True
                    tblInner.Visible = False
                End If
            Catch ex As System.ArgumentOutOfRangeException
                lblError.Text = "The selected line does not exist or is not a meeting product."
                lblError.Visible = True
                tblInner.Visible = False
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        'Added new function to get product Price IssueID:14817 22/11/2012
        Public Function GetProductPrice(ByVal lProductID As Long) As IProductPrice.PriceInfo
            Return ShoppingCart1.GetUserProductPrice(lProductID, 1)
        End Function

        Private Sub RemoveAttendeeSessions(ByVal OrderGE As OrdersEntity, ByVal ParentSequence As Long)
            Try
                Dim bDone As Boolean = False, i As Integer, ol As AptifyGenericEntityBase

                While i < OrderGE.SubTypes("OrderLines").Count
                    ol = OrderGE.SubTypes("OrderLines").Item(i)
                    If String.Compare(CStr(ol.GetValue("ParentSequence")), _
                                      CStr(ParentSequence)) = 0 Then
                        OrderGE.SubTypes("OrderLines").Remove(ol)
                    Else
                        i += 1
                    End If
                End While
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Private Function DoesAttendeeHaveSession(ByVal OrderGE As OrdersEntity, _
                                                ByVal ParentSequence As Long, _
                                                ByVal ProductID As Long) As Boolean
            Try
                'Navin Prasad Issue 10944
                ShoppingCart1.SaveCart(Session)
                OrderGE = ShoppingCart1.GetOrderObject(Session, Page.User, Page.Application)
                Dim oMeetingDetail As AptifyGenericEntityBase
                If OrderGE IsNot Nothing AndAlso OrderGE.SubTypes("OrderLines") IsNot Nothing Then
                    For Each ol As OrderLinesEntity In OrderGE.SubTypes("OrderLines")
                        'If String.Compare(CStr(ol.GetValue("ParentSequence")), CStr(ParentSequence)) = 0 AndAlso _
                        '   CLng(ol.GetValue("ProductID")) = ProductID Then
                        '    Return True
                        'End If
                        oMeetingDetail = CType(ol, OrderLinesEntity).ExtendedOrderDetailEntity
                        If oMeetingDetail IsNot Nothing AndAlso oMeetingDetail.GetValue("AttendeeID") IsNot Nothing Then
                            'Anil Issue 14381
                            'If lblAttendeeID.Text <> String.Empty And CLng(lblAttendeeID.Text) = CLng(oMeetingDetail.GetValue("AttendeeID")) Then
                            If CLng(ol.GetValue("ProductID")) = ProductID Then
                                Return True
                            End If
                            'End If
                        End If
                    Next
                End If

                Return False
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return False
            End Try
        End Function
        Private Sub LoadMeetingDetailFromXML(ByVal MeetingDetailGE As AptifyGenericEntityBase, _
                                             ByVal MeetingObjectXML As String)
            If MeetingObjectXML IsNot Nothing AndAlso _
               Len(MeetingObjectXML) > 0 Then
                MeetingDetailGE.Load("|" & MeetingObjectXML)
            End If
        End Sub

        Private Function LoadMeetingDetail(ByVal MeetingDetailGE As AptifyGenericEntityBase) As Boolean
            Dim sAttendeeID As String, sSQL As String
            Dim lAttendeeID As Long
            sAttendeeID = CStr(MeetingDetailGE.GetValue("AttendeeID"))
            If Len(sAttendeeID) > 0 And IsNumeric(sAttendeeID) Then
                lAttendeeID = CLng(sAttendeeID)
            End If
            If lAttendeeID > 0 Then
                'HP Issue#9090: split name field into distinct FirstName and LastName parts instead of a free form single textbox, therefore 
                '               add fields to sql for extraction and use datatable instead of scalar
                'Commented and added by Suvarna for IssueId 3240 , to get a Email address of Attendee
                'sSQL = "SELECT FirstName, LastName, FirstLast FROM " & _
                sSQL = "SELECT FirstName, LastName, FirstLast, Email1 FROM " & _
                       Database & _
                       "..vwPersons WHERE ID=" & lAttendeeID

                'HP Issue#9090: split name field into distinct FirstName and LastName parts instead of a free form single textbox
                'txtAttendee.Text = Trim(CStr(DataAction.ExecuteScalar(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)))
                Dim dt As DataTable = DataAction.GetDataTable(sSQL)
                txtFirstName.Text = Trim(CStr(dt.Rows(0)("FirstName")))
                txtLastName.Text = Trim(CStr(dt.Rows(0)("LastName")))
                txtEmail.Text = Trim(CStr(dt.Rows(0)("Email1")))
                'HP Issue#9090: determine if page is being called as a result of adding another attendee for the specified order line
            Else
                txtFirstName.Text = User1.GetValue("FirstName")
                txtLastName.Text = User1.GetValue("LastName")
                txtEmail.Text = User1.GetValue("Email1")
            End If
            txtBadgeName.Text = CStr(MeetingDetailGE.GetValue("BadgeName"))
            txtBadgeTitle.Text = CStr(MeetingDetailGE.GetValue("BadgeTitle"))
            txtCompany.Text = CStr(MeetingDetailGE.GetValue("BadgeCompanyName"))
        End Function


        Private Function CheckForConflict(ByRef oOrder As OrdersEntity, ByVal oOrderLine As OrderLinesEntity, _
                                          ByVal ProductID As Long, ByVal iLine As Integer, ByVal bIsTopLevel As Boolean) As Boolean

            Dim sConflictType As String
            Dim sSQL As String
            Dim lOrigProductID As Long
            lOrigProductID = CLng(oOrder.SubTypes("OrderLInes").Item(iLine).GetValue("ProductID"))

            'Check to see if Conflict Check is turned on
            sSQL = "select meetingconflictionchecker from " & Me.AptifyApplication.GetEntityBaseDatabase("Meetings") & _
                "..vwMeetings where productid = " & ProductID.ToString()
            sConflictType = Me.DataAction.ExecuteScalar(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.RefreshCache).ToString
            Return False
        End Function

        Private Function DoesConflictExist(ByVal lPersonID As Long, ByVal lProductID As Long, _
                                           ByVal oOrder As OrdersEntity, ByVal iLine As Integer, _
                                           ByRef lConflictOrderID As Long, ByVal bTopLevel As Boolean) As Boolean
            Dim sSQL As String
            Dim dt As DataTable
            Dim i As Integer
            Dim dt2 As DataTable
            Dim dCurStart As Date
            Dim dCurEnd As Date
            Dim dOLStart As Date
            Dim dOLEnd As Date
            Dim oMeetDetail As AptifyGenericEntityBase
            Dim oOrderLine As OrderLinesEntity
            Dim iStart As Integer
            Dim iEnd As Integer
            If bTopLevel Then
                sSQL = "Execute " & Me.AptifyApplication.GetEntityBaseDatabase("Meetings") & ".." & _
                        "spCheckMeetingConfliction " & lProductID.ToString & "," & lPersonID.ToString & ",-1"
                dt = Me.DataAction.GetDataTable(sSQL)

                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                    lConflictOrderID = CLng(dt.Rows(0).Item("OrderID"))
                    Return True
                    Exit Function
                End If
            Else
                If oOrder.SubTypes("OrderLines").Count < iLine + 2 Then
                    Return False
                End If
            End If

            If bTopLevel Then
                iStart = 0
                iEnd = iLine - 1
            Else
                iStart = iLine + 1
                iEnd = oOrder.SubTypes("OrderLines").Count - 1
            End If

            With oOrder.SubTypes("OrderLInes")
                For i = iStart To iEnd
                    'if orderline is meeting product and attendeeid = current attendee id
                    sSQL = "Select m.ID, m.StartDate, m.EndDate from " & Me.AptifyApplication.GetEntityBaseDatabase("Products") & _
                            "..vwProducts p join " & Me.AptifyApplication.GetEntityBaseDatabase("Meetings") & _
                            "..vwMeetings m on m.productid=p.id and m.productid=" & .Item(i).GetValue("ProductID").ToString()
                    If dt IsNot Nothing Then
                        dt.Clear()
                    End If

                    dt = Me.DataAction.GetDataTable(sSQL)

                    If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then

                        If bTopLevel Then

                            oOrderLine = CType(oOrder.SubTypes("OrderLines").Item(i), OrderLinesEntity)
                            oMeetDetail = oOrderLine.ExtendedOrderDetailEntity
                            Me.LoadMeetingDetailFromXML(oMeetDetail, CStr(oOrderLine.GetValue("__ExtendedAttributeObjectData")))

                            If CLng(oMeetDetail.GetValue("AttendeeID")) <> lPersonID Then
                                Continue For
                            End If

                        End If

                        sSQL = "Select m.ID, m.StartDate, m.EndDate from " & Me.AptifyApplication.GetEntityBaseDatabase("Products") & _
                                "..vwProducts p join " & Me.AptifyApplication.GetEntityBaseDatabase("Meetings") & _
                                "..vwMeetings m on m.productid=p.id and m.productid=" & lProductID.ToString()
                        dt2 = Me.DataAction.GetDataTable(sSQL)

                        If dt2 IsNot Nothing AndAlso dt2.Rows.Count > 0 Then
                            dCurStart = CDate(dt2.Rows(0).Item("StartDate"))
                            dCurEnd = CDate(dt2.Rows(0).Item("EndDate"))
                            dOLStart = CDate(dt.Rows(0).Item("StartDate"))
                            dOLEnd = CDate(dt.Rows(0).Item("EndDate"))


                            If CheckSessionConfliction(dOLStart, dOLEnd, dCurStart, dCurEnd) Then
                                lConflictOrderID = oOrder.RecordID
                                Return True
                            End If
                        End If
                    End If
                Next
            End With

            Return False

        End Function
        'Navin Prasad Issue 11488
        Private Function CheckSessionConfliction(ByVal StartDate As DateTime, ByVal EndDate As DateTime, ByVal StartDate_Compare As DateTime, ByVal EndDate_Compare As DateTime) As Boolean
            If StartDate = StartDate_Compare Then
                Return True
            End If
            If StartDate > StartDate_Compare Then
                If EndDate_Compare > StartDate Then
                    Return True
                End If
            Else
                If EndDate > StartDate_Compare Then
                    Return True
                End If
            End If
            Return False
        End Function

        Private Class ParentChildInfo
            Public ParentLine As OrderLinesEntity
            Public SubLines As New Generic.List(Of OrderLinesEntity)
        End Class
        Private Sub AdjustParentInfo(ByVal OrderGE As OrdersEntity, _
                                     ByVal ParInfo As Generic.List(Of ParentChildInfo))
            Dim iSeq As Integer
            For Each par As ParentChildInfo In ParInfo
                iSeq = 1 + OrderGE.SubTypes("OrderLines").IndexOf(par.ParentLine)
                For Each subline As OrderLinesEntity In par.SubLines
                    subline.SetValue("ParentSequence", iSeq)
                Next
            Next
        End Sub

        Private Function CheckForSessionConflict(ByVal oOrder As OrdersEntity, ByVal lProductID As Long) As Boolean
            Dim i As Integer
            Dim iLine As Integer
            Dim sSQL As String
            Dim dt As DataTable
            Dim dt2 As DataTable
            Dim dCurStart As Date
            Dim dCurEnd As Date
            Dim dOLStart As Date
            Dim dOLEnd As Date
            iLine = CInt(Session("OL"))
            With oOrder.SubTypes("OrderLines")

                If .Count >= iLine + 3 Then
                    For i = iLine + 1 To .Count - 1

                        sSQL = "Select m.ID, m.StartDate, m.EndDate from " & Me.AptifyApplication.GetEntityBaseDatabase("Products") & _
                                "..vwProducts p join " & Me.AptifyApplication.GetEntityBaseDatabase("Meetings") & _
                                "..vwMeetings m on m.productid=p.id and m.productid=" & .Item(i).GetValue("ProductID").ToString()

                        dt = Me.DataAction.GetDataTable(sSQL)

                        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then

                            sSQL = "Select m.ID, m.StartDate, m.EndDate from " & Me.AptifyApplication.GetEntityBaseDatabase("Products") & _
                                    "..vwProducts p join " & Me.AptifyApplication.GetEntityBaseDatabase("Meetings") & _
                                    "..vwMeetings m on m.productid=p.id and m.productid=" & lProductID.ToString()
                            dt2 = Me.DataAction.GetDataTable(sSQL)

                            If dt2 IsNot Nothing AndAlso dt2.Rows.Count > 0 Then
                                dCurStart = CDate(dt2.Rows(0).Item("StartDate"))
                                dCurEnd = CDate(dt2.Rows(0).Item("EndDate"))
                                dOLStart = CDate(dt.Rows(0).Item("StartDate"))
                                dOLEnd = CDate(dt.Rows(0).Item("EndDate"))
                                'Navin Prasad Issue 11488
                                'If dCurEnd > dOLStart And dCurStart <= dOLEnd Then
                                '    Return True
                                '    Exit Function
                                'End If
                                If CheckSessionConfliction(dOLStart, dOLEnd, dCurStart, dCurEnd) Then
                                    ' Exit Function
                                    Return True
                                End If

                            End If
                        End If


                    Next
                End If

            End With


            Return False
        End Function
        Private Function SaveSessions(ByVal OrderGE As OrdersEntity, _
                                 ByVal AttendeeID As Long, _
                                 ByVal ParentSequence As Integer) As Boolean
            Try
                ' go through the check boxes and add one line to the order for each such item
                Dim lProductID As Long, sBaseKey As String, i As Integer
                Dim oParInfo As New Generic.List(Of ParentChildInfo)
                Dim oOrderLine As OrderLinesEntity
                Dim iLine As Integer
                iLine = CInt(Session("OL"))

                oOrderLine = CType(OrderGE.SubTypes("OrderLines").Item(iLine), OrderLinesEntity)

                '*** IMPORTANT.. Need to adjust the line below to reference the outermost HTML element
                ' currently it's the updatepanel, but if that's removed than it's tblMain in the 
                ' stock product
                'sBaseKey = tblMain.Parent.UniqueID & "$chkSelect_"
                BuildParentInfo(OrderGE, oParInfo, ParentSequence)
                'Navin Prasad Issue 10944
                ' RemoveAttendeeSessions(OrderGE, ParentSequence)
                RefreshSession(AttendeeID)

                For i = 0 To Me.Request.Form.Keys.Count - 1
                    If Me.Request.Form.Keys.Item(i).Length > Len(sBaseKey) AndAlso _
                       String.Compare(Me.Request.Form.Keys.Item(i).Substring(0, _
                                        Len(sBaseKey)), sBaseKey, True) = 0 Then
                        ' got it
                        If String.Compare(Request.Form.Item(i), "on", True) = 0 Then
                            lProductID = CLng(Request.Form.Keys(i).Substring(Len(sBaseKey)))

                            If Me.CheckForConflict(OrderGE, oOrderLine, lProductID, iLine, False) Then
                                Return False
                                Exit Function
                            End If

                            oOrderLine = OrderGE.AddProduct(lProductID).Item(0)

                            With oOrderLine

                                .ExtendedOrderDetailEntity.SetValue("AttendeeID", AttendeeID)
                                .SetAddValue("__ExtendedAttributeObjectData", .ExtendedOrderDetailEntity.GetObjectData(False))
                            End With
                        End If
                    End If
                Next
                Dim iNumFound As Integer = 0
                i = 0
                While i < OrderGE.SubTypes("OrderLines").Count
                    With OrderGE.SubTypes("OrderLines").Item(i)
                        If CInt(.GetValue("ParentSequence")) = ParentSequence Then
                            ' got one, move it below parent sequence + iNumFound
                            If i <> ParentSequence + iNumFound Then
                                OrderGE.SubTypes("OrderLines").MoveObject(i, ParentSequence + iNumFound, AptifyMoveObjectOptions.aptifyShift)
                            Else
                                i += 1
                            End If
                            iNumFound += 1
                        Else
                            i += 1
                        End If
                    End With
                End While

                AdjustParentInfo(OrderGE, oParInfo)
                Return True

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Function

        Private Function GetProductWebName(ByVal ProductID As Long) As String
            Dim sSQL As String
            sSQL = "SELECT WebName FROM " & Database & _
                   "..vwProducts WHERE ID=" & ProductID
            Return CStr(DataAction.ExecuteScalar(sSQL))
        End Function

        ''RashmiP
        Private Function GetAvailableSpace(ByVal ProductID As Long) As Integer
            Dim sSQL As String
            sSQL = "SELECT AVAILSPACE FROM  " & Database & _
                   "..vwMEETINGS WHERE Productid = " & ProductID
            Return CInt(DataAction.ExecuteScalar(sSQL))
        End Function
        Private Function ProdInventoryLedgerExist(ByVal ProductID As Long) As Boolean
            Try
                Dim sSQL As String
                sSQL = "SELECT ID FROM " & Database & _
                       "..vwProdInvLedger where ProductID =  " & ProductID
                Return CBool(DataAction.ExecuteScalar(sSQL))
            Catch ex As Exception
                Return False
            End Try
        End Function
        ''Rashmi P, Find Available space for meeting, issue 17108.
        Private Function FindAvailableSpaceforMeeting(lproductID As Long, oOrderGE As AptifyGenericEntityBase) As Integer
            Try
                Dim iCount As Integer = 0
                If oOrderGE IsNot Nothing Then
                    For Each oOrderLine As OrderLinesEntity In oOrderGE.SubTypes("OrderLines")
                        If CLng(oOrderLine.ProductID) = lproductID Then
                            iCount += 1
                        End If
                    Next
                End If
                Return iCount - 1
            Catch ex As Exception

            End Try
        End Function

        ''RashmiP, Issue 10287,9/22/11 Complete Order of Meeting Registration for Free Meetings
        Private Sub CompleteOrderforFreeMeetings()

            Dim lOrderID As Long, sError As String
            Dim iPOPaymentType As Integer
            Try
                If Not String.IsNullOrEmpty(AptifyApplication.GetEntityAttribute("Web Shopping Carts", "POPaymentTypeID")) Then
                    iPOPaymentType = CInt(AptifyApplication.GetEntityAttribute("Web Shopping Carts", "POPaymentTypeID"))
                Else
                    iPOPaymentType = 1
                End If
                With ShoppingCart1.GetOrderObject(Session, Page.User, Application)
                    .SetValue("PayTypeID", iPOPaymentType)
                    ShoppingCart1.SaveCart(Session)
                    lOrderID = ShoppingCart1.PlaceOrder(Session, Application, Page.User, sError)
                End With
                If lOrderID > 0 Then
                    Response.Redirect(OrderConfirmationPage & "?ID=" & lOrderID)
                End If


            Catch ex As Exception

            End Try
        End Sub
        'Anil B for Issue 14381
        'Set a column for datatable
        Private Sub SetInitialColumn()
            Dim dr As DataRow = Nothing
            dtAttendeeInfo.Columns.Add(New DataColumn("RowNumber", GetType(String)))
            dtAttendeeInfo.Columns.Add(New DataColumn("first Name", GetType(String)))
            dtAttendeeInfo.Columns.Add(New DataColumn("Last Name", GetType(String)))
            dtAttendeeInfo.Columns.Add(New DataColumn("Email", GetType(String)))
            dtAttendeeInfo.Columns.Add(New DataColumn("Badge Information Name", GetType(String)))
            dtAttendeeInfo.Columns.Add(New DataColumn("Badge Information Title", GetType(String)))
            dtAttendeeInfo.Columns.Add(New DataColumn("Badge Information Company", GetType(String)))
            dtAttendeeInfo.Columns.Add(New DataColumn("Attendee FoodPreference", GetType(String)))
            dtAttendeeInfo.Columns.Add(New DataColumn("Attendee TravelPreference", GetType(String)))
            dtAttendeeInfo.Columns.Add(New DataColumn("Attendee GolfHandicap", GetType(String)))
            dtAttendeeInfo.Columns.Add(New DataColumn("Attendee SpecialRequest", GetType(String)))
            dtAttendeeInfo.Columns.Add(New DataColumn("Attendee OtherPreference", GetType(String)))
            dtAttendeeInfo.Columns.Add(New DataColumn("Attendee FoodPreferenceID", GetType(Integer)))
            dtAttendeeInfo.Columns.Add(New DataColumn("Attendee TravelPreferenceID", GetType(Integer)))
            'Anil B for issue 15138 on 20-May-2013
            'For XML Serialization when State Server Is Enabled
            'dtAttendeeInfo.Columns.Add(New DataColumn("Edit", GetType(LinkButton)))
            'dtAttendeeInfo.Columns.Add(New DataColumn("Delete", GetType(ImageButton)))
            Session(ATTRIBUTE_ATTENDEE_SESSION) = dtAttendeeInfo
        End Sub
        'Anil B for Issue 14381
        'Add logic to Edit delete the Attendee info and session.
        Private Sub AddNewRowToGridFromAttendee(ByVal RowIndex As Integer)
            Try
                Dim iRowIndex As Integer = 0
                Dim drCurrentRow As DataRow = Nothing
                If Session(ATTRIBUTE_ATTENDEE_SESSION) IsNot Nothing Then
                    dtAttendeeInfo = DirectCast(Session(ATTRIBUTE_ATTENDEE_SESSION), DataTable)
                    drCurrentRow = dtAttendeeInfo.NewRow()
                    drCurrentRow("RowNumber") = dtAttendeeInfo.Rows.Count + 1
                    If RowIndex >= 0 Then
                        iRowIndex = RowIndex
                    Else
                        dtAttendeeInfo.Rows.Add(drCurrentRow)
                        iRowIndex = dtAttendeeInfo.Rows.Count - 1
                    End If
                    dtAttendeeInfo.Rows(iRowIndex)("first Name") = IIf(RowIndex = -1, txtFirstName.Text, txtPopFirstName.Text)
                    dtAttendeeInfo.Rows(iRowIndex)("Last Name") = IIf(RowIndex = -1, txtLastName.Text, txtPopLastName.Text)
                    dtAttendeeInfo.Rows(iRowIndex)("Email") = IIf(RowIndex = -1, txtEmail.Text, txtPopEmail.Text)
                    dtAttendeeInfo.Rows(iRowIndex)("Badge Information Name") = IIf(RowIndex = -1, txtBadgeName.Text, txtPopBadgeName.Text)
                    dtAttendeeInfo.Rows(iRowIndex)("Badge Information Title") = IIf(RowIndex = -1, txtBadgeTitle.Text, txtPopBadgeTitle.Text)
                    dtAttendeeInfo.Rows(iRowIndex)("Badge Information Company") = IIf(RowIndex = -1, txtCompany.Text, txtPopBadgeCompany.Text)
                    dtAttendeeInfo.Rows(iRowIndex)("Attendee FoodPreference") = IIf(RowIndex = -1, ddlFoodPreference.Text, ddlPopFoodPreference.Text)
                    dtAttendeeInfo.Rows(iRowIndex)("Attendee TravelPreference") = IIf(RowIndex = -1, ddlTravelPreference.Text, ddlPopTravelPreference.Text)
                    dtAttendeeInfo.Rows(iRowIndex)("Attendee GolfHandicap") = IIf(RowIndex = -1, txtGolfHandicape.Text, txtPopGolfHandicap.Text)
                    dtAttendeeInfo.Rows(iRowIndex)("Attendee SpecialRequest") = IIf(RowIndex = -1, txtSpecialRequest.Text, txtPopSpecialRequest.Text)
                    dtAttendeeInfo.Rows(iRowIndex)("Attendee OtherPreference") = IIf(RowIndex = -1, txtOtherPreference.Text, txtPopOtherPreference.Text)
                    dtAttendeeInfo.Rows(iRowIndex)("Attendee FoodPreferenceID") = IIf(RowIndex = -1, ddlFoodPreference.SelectedValue, ddlPopFoodPreference.SelectedValue)

                    dtAttendeeInfo.Rows(iRowIndex)("Attendee TravelPreferenceID") = IIf(RowIndex = -1, ddlTravelPreference.SelectedValue, ddlPopTravelPreference.SelectedValue)
                    'Anil B for issue 15138 on 20-May-2013
                    'For XML Serialization when State Server Is Enabled
                    'dtAttendeeInfo.Rows(iRowIndex)("Edit") = New LinkButton
                    'dtAttendeeInfo.Rows(iRowIndex)("Delete") = New ImageButton
                    Session(ATTRIBUTE_ATTENDEE_SESSION) = dtAttendeeInfo
                    grdAddMember.DataSource = dtAttendeeInfo
                    grdAddMember.AllowPaging = False
                    grdAddMember.DataBind()
                    If ViewState(ATTRIBUTE_ATTENDEE_SELECTEDMEETING) IsNot Nothing Then
                        descSelectedSessionForAttendee = CType(ViewState(ATTRIBUTE_ATTENDEE_SELECTEDMEETING), Dictionary(Of Integer, List(Of Integer)))
                    End If
                    Dim lstSession As New List(Of Integer)
                    If Session("MeetingSessions") IsNot Nothing Then
                        lstSession = CType(Session("MeetingSessions"), List(Of Integer))
                    End If
                    DeleteMeetingSessionFromDictionary(CInt(dtAttendeeInfo.Rows.Count))
                    descSelectedSessionForAttendee.Add(CInt(dtAttendeeInfo.Rows.Count), lstSession)
                    ViewState(ATTRIBUTE_ATTENDEE_SELECTEDMEETING) = descSelectedSessionForAttendee
                    btnAddRegistrant.Visible = True
                    lblAddedAtendee.Visible = True
                    ClearText()
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        'Anil B for Issue 14381
        'Add logic to Edit delete the Attendee info and session.
        Protected Sub grdAddMembers_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdAddMember.RowCommand
            Dim grdIndex As Integer = CInt(e.CommandArgument)
            If e.CommandName = "DeleteRow" Then
                Try
                    If Session(ATTRIBUTE_ATTENDEE_SESSION) IsNot Nothing Then
                        Dim dt As DataTable = DirectCast(Session(ATTRIBUTE_ATTENDEE_SESSION), DataTable)
                        Dim iRowNumber As Integer
                        If dt.Rows.Count > 0 Then
                            btnAddRegistrant.Visible = True
                            For iDatatableIndex As Integer = dt.Rows.Count - 1 To 0 Step -1
                                iRowNumber = CInt(dt.Rows(iDatatableIndex)("RowNumber"))
                                If iRowNumber = grdIndex Then
                                    dt.Rows(iDatatableIndex).Delete()
                                    Exit For
                                End If
                            Next
                            DeleteMeetingSessionFromDictionary(grdIndex)
                            'After removal - refresh the session data as well
                            Session(ATTRIBUTE_ATTENDEE_SESSION) = Nothing
                            Session(ATTRIBUTE_ATTENDEE_SESSION) = dt
                            grdAddMember.AllowPaging = False
                            grdAddMember.DataSource = dt
                            grdAddMember.DataBind()
                            If dt.Rows.Count <= 0 Then
                                btnAddRegistrant.Visible = False
                                lblAddedAtendee.Visible = False
                            End If
                        Else
                            lblMeetingRegistrationError.Text = "Atleast one attendee is required for meeting registration."
                            lblMeetingRegistrationError.ForeColor = Drawing.Color.Red
                            lblMeetingRegistrationError.Visible = True
                        End If
                    End If
                Catch ex As Exception
                    Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                End Try
            ElseIf e.CommandName = "EditRow" Then
                Try
                    hgrdindex.Value = ""
                    If Session(ATTRIBUTE_ATTENDEE_SESSION) IsNot Nothing Then
                        Dim dt As DataTable = DirectCast(Session(ATTRIBUTE_ATTENDEE_SESSION), DataTable)
                        If dt.Rows.Count >= 1 Then
                            grdIndex = grdIndex - 1
                            hgrdindex.Value = CStr(grdIndex)
                            'extract the Grid values
                            Dim lblFname As Label = DirectCast(grdAddMember.Rows(grdIndex).FindControl("lblAtendeeFirstName"), Label)
                            Dim lblLName As Label = DirectCast(grdAddMember.Rows(grdIndex).FindControl("lblAtendeeLastName"), Label)
                            Dim lblEmail As Label = DirectCast(grdAddMember.Rows(grdIndex).FindControl("lblAttendeeEmail"), Label)
                            Dim lblBadgeName As Label = DirectCast(grdAddMember.Rows(grdIndex).FindControl("lblBadgeName"), Label)
                            Dim lblBadgeTitle As Label = DirectCast(grdAddMember.Rows(grdIndex).FindControl("lblBadgeTitle"), Label)
                            Dim lblBadgeCompany As Label = DirectCast(grdAddMember.Rows(grdIndex).FindControl("lblBadgeCompany"), Label)

                            Dim lblgFoodPreference As Label = DirectCast(grdAddMember.Rows(grdIndex).FindControl("lblFoodPreference1"), Label)
                            Dim lblgTravelPreference As Label = DirectCast(grdAddMember.Rows(grdIndex).FindControl("plblTravelPreference"), Label)
                            Dim lblgGolfHandicap As Label = DirectCast(grdAddMember.Rows(grdIndex).FindControl("plblGolfHandicap"), Label)
                            Dim lblgSpecialRequest As Label = DirectCast(grdAddMember.Rows(grdIndex).FindControl("plblSpecialRequest"), Label)
                            Dim lblgOtherPreference As Label = DirectCast(grdAddMember.Rows(grdIndex).FindControl("plblotherPreference"), Label)
                            'Anil B for issue 15138 on 20-May-2013
                            'For XML Serialization when State Server Is Enabled
                            'Dim lnkEdit As LinkButton = DirectCast(grdAddMember.Rows(grdIndex).FindControl("lnkEditInfo"), LinkButton)
                            'Dim btndelete As ImageButton = DirectCast(grdAddMember.Rows(grdIndex).FindControl("btndelete"), ImageButton)

                            txtPopFirstName.Text = lblFname.Text
                            txtPopLastName.Text = lblLName.Text
                            txtPopEmail.Text = lblEmail.Text
                            txtPopBadgeName.Text = lblBadgeName.Text
                            txtPopBadgeTitle.Text = lblBadgeTitle.Text
                            txtPopBadgeCompany.Text = lblBadgeCompany.Text
                            ddlPopFoodPreference.SelectedValue = lblgFoodPreference.Text
                            ddlPopTravelPreference.SelectedValue = lblgTravelPreference.Text
                            txtPopGolfHandicap.Text = lblgGolfHandicap.Text
                            txtPopSpecialRequest.Text = lblgSpecialRequest.Text
                            txtPopOtherPreference.Text = lblgOtherPreference.Text
                            popEditAttendee.VisibleOnPageLoad = True
                        End If
                    End If

                Catch ex As Exception
                    Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                End Try
            ElseIf e.CommandName = "EditSession" Then
                If grdMeetingSession.Items.Count > 0 Then
                    Dim pair As KeyValuePair(Of Integer, List(Of Integer))
                    Dim lstSessionList As List(Of Integer)
                    Dim lstpoductList As New List(Of Integer)
                    Dim lProductSessionID As Long
                    Dim lblSessionID As Label
                    Dim row As GridDataItem
                    Dim iIndex As Integer
                    Dim chkSession As CheckBox
                    Dim lblFname As Label
                    Dim lblLName As Label
                    Dim lblEmail As Label
                    Dim dicMeetingSessions As Dictionary(Of Integer, Boolean)
                    Dim dicMeetingStatus As New Dictionary(Of Integer, Boolean)
                    If ViewState(ATTRIBUTE_CHECKED_SESSION) IsNot Nothing Then
                        dicMeetingSessions = DirectCast(ViewState(ATTRIBUTE_CHECKED_SESSION), Dictionary(Of Integer, Boolean))
                    End If
                    radPopUpEditListSession.VisibleOnPageLoad = True
                    hdnGrdRowIndex.Value = CStr(grdIndex)
                    lblFname = DirectCast(grdAddMember.Rows(grdIndex - 1).FindControl("lblAtendeeFirstName"), Label)
                    lblLName = DirectCast(grdAddMember.Rows(grdIndex - 1).FindControl("lblAtendeeLastName"), Label)
                    lblEmail = DirectCast(grdAddMember.Rows(grdIndex - 1).FindControl("lblAttendeeEmail"), Label)
                    lblAttendeeName.Text = lblFname.Text + " " + lblLName.Text
                    lblEmailID.Text = lblEmail.Text
                    If ViewState(ATTRIBUTE_ATTENDEE_SELECTEDMEETING) IsNot Nothing Then
                        descSelectedSessionForAttendee = CType(ViewState(ATTRIBUTE_ATTENDEE_SELECTEDMEETING), Dictionary(Of Integer, List(Of Integer)))
                    End If
                    If ViewState(ATTRIBUTE_SESION_STATUS) IsNot Nothing Then
                        dicMeetingStatus = DirectCast(ViewState(ATTRIBUTE_SESION_STATUS), Dictionary(Of Integer, Boolean))
                    End If
                    If descSelectedSessionForAttendee.Count > 0 Then
                        ClereSession()
                        For Each pair In descSelectedSessionForAttendee
                            If pair.Key = grdIndex Then
                                lstSessionList = pair.Value
                                If descSelectedSessionForAttendee.Count > 0 Then
                                    For Each lProductSessionID In lstSessionList
                                        'For Each dataItem As GridDataItem In grdMeetingSession.Items
                                        '    lblSessionID = CType(dataItem.FindControl("lblProductID"), Label)
                                        '    row = DirectCast(lblSessionID.Parent.Parent, GridDataItem)
                                        '    iIndex = row.DataSetIndex
                                        '    chkSession = CType(grdMeetingSession.Items(iIndex).FindControl("chkSession"), CheckBox)
                                        '    If CLng(lblSessionID.Text) = lProductSessionID Then
                                        '        chkSession.Checked = True
                                        '    End If
                                        'Next
                                        If dicMeetingSessions.ContainsKey(CInt(lProductSessionID)) Then
                                            dicMeetingSessions.Item(CInt(lProductSessionID)) = True
                                        End If


                                        'For Each sPair As KeyValuePair(Of Integer, Boolean) In dicMeetingSessions
                                        '    If sPair.Key = lProductSessionID Then
                                        '        'dicMeetingSessions.Item(sPair.Key) = True
                                        '        'dicMeetingSessions.Remove(sPair.Key)
                                        '        'dicMeetingSessions.Add(CInt(lProductSessionID), True)
                                        '    End If
                                        'Next
                                    Next
                                    ViewState(ATTRIBUTE_CHECKED_SESSION) = dicMeetingSessions
                                    Dim i As Integer = 0
                                    For Each skey As KeyValuePair(Of Integer, Boolean) In dicMeetingSessions
                                        If Not lstpoductList.Contains(skey.Key) Then
                                            lstpoductList.Add(skey.Key)
                                        End If
                                        If dicMeetingStatus.ContainsKey(i) Then
                                            If lstSessionList.Contains(skey.Key) Then
                                                dicMeetingStatus.Item(i) = True
                                            Else
                                                dicMeetingStatus.Item(i) = False
                                            End If

                                        Else
                                            dicMeetingStatus.Add(i, skey.Value)

                                        End If
                                        i = i + 1

                                    Next
                                    For Each ipro As Integer In lstpoductList
                                        If lstSessionList.Contains(ipro) Then
                                            dicMeetingSessions.Item(ipro) = True
                                        Else
                                            dicMeetingSessions.Item(ipro) = False
                                        End If
                                    Next
                                    If ViewState(ATTRIBUTE_SESION_STATUS) Is Nothing Then
                                        ViewState(ATTRIBUTE_SESION_STATUS) = dicMeetingStatus
                                    End If
                                    For Each dataItem As GridDataItem In grdMeetingSession.Items
                                        lblSessionID = CType(dataItem.FindControl("lblProductID"), Label)
                                        row = DirectCast(lblSessionID.Parent.Parent, GridDataItem)
                                        chkSession = CType(dataItem.FindControl("chkSession"), CheckBox)
                                        chkSession.Checked = dicMeetingStatus.Item(row.DataSetIndex)
                                    Next
                                End If
                            End If
                        Next
                    End If
                Else
                    radMeetingSessionCountInfo.VisibleOnPageLoad = True
                End If
            End If
        End Sub
        Protected Sub btnDeleteAll_Click(ByVal sender As Object, ByVal e As EventArgs)
            SetInitialColumn()
        End Sub
        Protected Sub btnAddInfo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddInfo.Click
            lblMeetingRegistrationError.Text = ""
            lblMeetingRegistrationError.Visible = False
            'IssueID 3240 - Validate person before adding to grid.
            Dim icount As Int32
            icount = ValidatePerson()
            ''Case icount = 1  when 1 or more records already Exists in database
            ''else case icount = 0 when entered last name and Email matches with the record in database but first name do not matches. This case will be 
            ''Applicable iif there is single record exists in the database
            If icount = 1 Then
                radDuplicateUser.VisibleOnPageLoad = True
                Exit Sub
            ElseIf icount = 0 Then
                LoadMember()
                radSimilarRecords.VisibleOnPageLoad = True
                Exit Sub
            End If
            'Amruta ,IssueID 14380,Prevent user to register for meeting again if already register
            Dim dtAttendeeStatus As DataTable
            dtAttendeeStatus = LoadUserMeetingStatus()
            If dtAttendeeStatus IsNot Nothing Then
                For Each rw As DataRow In dtAttendeeStatus.Rows
                    If (txtFirstName.Text.ToUpper() = rw("FirstName").ToString().ToUpper() AndAlso txtLastName.Text.ToUpper() = rw("LastName").ToString().ToUpper() AndAlso txtEmail.Text.ToUpper() = rw("Email").ToString().ToUpper()) Then
                        lblError.Visible = True
                        lblError.Text = "You have already registered for this meeting."
                        ClearText()
                        Exit Sub
                    Else
                        lblError.Visible = False
                    End If
                Next
            End If
            ''Validate Record within a grid in case of new attendee having Email Id which is already added to grid.
            If ValidateGridRecords(txtEmail.Text.Trim.ToString(), -1) Then
                AddNewRowToGridFromAttendee(-1)
            Else
                radValidateGrdRec.VisibleOnPageLoad = True
            End If
        End Sub
        Protected Sub btnPopUpCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPopUpCancel.Click
            popEditAttendee.VisibleOnPageLoad = False
        End Sub
        'Anil B for Issue 14381 on 19-03-2013
        'Validate the field from Edit Atendee Info Pop-Up
        Private Function ValidateAttendeeFromEditPopUp() As Boolean
            If txtPopFirstName.Text.Trim = "" OrElse txtPopLastName.Text.Trim = "" OrElse txtPopEmail.Text.Trim = "" Then
                lblPopErrorMessage.Text = "Fields marked with * are mandatory"
                lblPopErrorMessage.Visible = True
                Return False
            Else
                lblPopErrorMessage.Visible = False
            End If
            If CommonMethods.EmailAddressCheck(txtPopEmail.Text) = False Then
                lblPopErrorMessage.Text = "Please enter a valid Email Address."
                lblPopErrorMessage.Visible = True
                Return False
            Else
                lblPopErrorMessage.Visible = False
            End If
            Return True
        End Function
        'Anil B for Issue 14381
        Protected Sub btnPopUpOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPopUpOk.Click
            Try
                If ValidateAttendeeFromEditPopUp() Then
                    Dim icount As Int32
                    Dim index As Integer = CInt(hgrdindex.Value)
                    ''Issue - 3240 added by Suvarna
                    ''Case icount = 1  when 1 or more records already Exists in database
                    ''else case icount = 0 when entered last name and Email matches with the record in database but first name do not matches. This case will be 
                    ''Applicable iif there is single record exists in the database
                    icount = ValidateEditPersonInfo()
                    If icount = 1 Then
                        radDuplicateUser.VisibleOnPageLoad = True
                        Exit Sub
                    ElseIf icount = 0 Then
                        LoadEditMemberInfo()
                        radSimilarRecords.VisibleOnPageLoad = True
                        Exit Sub
                    End If
                    ''Validate Record within a grid in case of new attendee having Email Id which is already added to grid.
                    If Not ValidateGridRecords(txtPopEmail.Text.Trim.ToString(), index) Then
                        radValidateGrdRec.VisibleOnPageLoad = True
                        Exit Sub
                    End If
                    If hgrdindex.Value <> "" Then
                        AddNewRowToGridFromAttendee(index)
                        popEditAttendee.VisibleOnPageLoad = False
                    End If
                Else
                    popEditAttendee.VisibleOnPageLoad = True
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        'Anil B for Issue 14381
        Protected Overridable Sub ClearText()
            Try
                txtFirstName.Text = ""
                txtLastName.Text = ""
                txtEmail.Text = ""
                txtBadgeName.Text = ""
                txtBadgeTitle.Text = ""
                txtCompany.Text = ""
                ddlFoodPreference.SelectedValue = "0"
                ddlTravelPreference.SelectedValue = "0"
                txtGolfHandicape.Text = ""
                txtSpecialRequest.Text = ""
                txtSpecialRequest.Text = ""
                txtOtherPreference.Text = ""
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Protected Sub lnkMeetingPage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkMeetingPage.Click
            If ViewState(ATTRIBUTE_ATTENDEE_MEETINGPRODUCTID) IsNot Nothing Then
                Dim sEventURl As String = MeetingPage + "?ID=" + CStr(ViewState(ATTRIBUTE_ATTENDEE_MEETINGPRODUCTID))
                Me.Response.Redirect(sEventURl, False)
            End If
        End Sub
        ''' <summary>
        ''' IssuID - 3240 Added by Suvarna D
        ''' returns boolean value
        ''' Returns 1  when 1 or more records already Exists in database
        ''' Returns = 0 when entered last name and Email matches with the record in database but first name do not matches. This case will be 
        ''' Applicable iif there is single record exists in the database
        ''' Returns -1 when New record or Single record exists for entered First Name, Last name and Email 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function ValidatePerson() As Int32
            Dim sSpName As String = ""
            Dim sSQL As String = ""
            Dim iCnt As Int32 = 0
            If Not String.IsNullOrEmpty(AptifyApplication.GetEntityAttribute("Persons", "ValidateMeetingAttendee")) Then
                sSpName = CStr(AptifyApplication.GetEntityAttribute("Persons", "ValidateMeetingAttendee"))
            End If
            sSQL = "Exec " & sSpName & " '" & txtFirstName.Text.Trim.Replace("'", "''") & "', '" & txtLastName.Text.Trim.Replace("'", "''") & "', '" & txtEmail.Text.Trim.Replace("'", "''") & "'"
            iCnt = Convert.ToInt32(DataAction.ExecuteScalar(sSQL))
            Return iCnt
        End Function
        ''' <summary>
        ''' IssuID - 3240 Added by Suvarna D
        ''' returns boolean value
        ''' Returns 1  when 1 or more records already Exists in database
        ''' Returns = 0 when entered last name and Email matches with the record in database but first name do not matches. This case will be 
        ''' Applicable iif there is single record exists in the database
        ''' Returns -1 when New record or Single record exists for entered First Name, Last name and Email 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function ValidateEditPersonInfo() As Int32
            Dim sSpName As String = ""
            Dim sSQL As String = ""
            Dim iCnt As Int32 = 0
            If Not String.IsNullOrEmpty(AptifyApplication.GetEntityAttribute("Persons", "ValidateMeetingAttendee")) Then
                sSpName = CStr(AptifyApplication.GetEntityAttribute("Persons", "ValidateMeetingAttendee"))
            End If

            sSQL = "Exec " & sSpName & " '" & txtPopFirstName.Text.Trim.Replace("'", "''") & "', '" & txtPopLastName.Text.Trim.Replace("'", "''") & "', '" & txtPopEmail.Text.Trim.Replace("'", "''") & "'"

            iCnt = Convert.ToInt32(DataAction.ExecuteScalar(sSQL))

            Return iCnt
        End Function

        Protected Sub btnok_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnok.Click
            radDuplicateUser.VisibleOnPageLoad = False
        End Sub

        Private Sub LoadMember()
            Dim dt As DataTable, sSQL As String
            Try
                sSQL = "select TOp 1 P.Photo, P.FirstLast from " & Database & "..vwPersons P where UPPER(P.LastName) = '" & txtLastName.Text.Trim.ToUpper.Replace("'", "''") & "' AND UPPER(P.Email1) = '" & txtEmail.Text.Trim.ToUpper.Replace("'", "''") & "'"
                dt = Me.DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                    Me.grdmember.DataSource = dt
                    Me.grdmember.DataBind()
                    grdmember.Visible = True
                Else
                    grdmember.Visible = False
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub LoadEditMemberInfo()
            Dim dt As DataTable, sSQL As String
            Try
                sSQL = "select TOp 1 P.Photo, P.FirstLast from " & Database & "..vwPersons P where UPPER(P.LastName) = '" & txtPopLastName.Text.Trim.ToUpper.Replace("'", "''") & "' AND UPPER(P.Email1) = '" & txtPopEmail.Text.Trim.ToUpper.Replace("'", "''") & "'"

                dt = Me.DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                    Me.grdmember.DataSource = dt
                    Me.grdmember.DataBind()
                    grdmember.Visible = True
                Else
                    grdmember.Visible = False
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub btnGetData_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetData.Click
            radSimilarRecords.VisibleOnPageLoad = False
            Dim AttendeeFName As Label = CType(grdmember.Rows(0).FindControl("lblMember"), Label)
            txtFirstName.Text = AttendeeFName.Text.Trim().Substring(0, AttendeeFName.Text.IndexOf(" "))
            txtPopFirstName.Text = AttendeeFName.Text.Trim().Substring(0, AttendeeFName.Text.IndexOf(" "))
        End Sub

        Protected Sub btnNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNo.Click
            radSimilarRecords.VisibleOnPageLoad = False
            radChangeEmail.VisibleOnPageLoad = True
        End Sub

        Protected Overridable Function FindCreatePerson(ByVal FirstName As String, ByVal LastName As String, ByVal Email As String) As Long
            Try
                'Modified by Vijay Soni for Issue#5416 on Jan 30, 2008(Start)
                Dim lValue As Object, sbrPersonId As New StringBuilder
                'HP Issue#9261:  correcting apostrophe issue and possible sql injection by using parameters               

                With sbrPersonId
                    .Length = 0
                    .Append("SELECT TOP 1 ID FROM ")
                    .Append(Me.AptifyApplication.GetEntityBaseDatabase("Persons").ToString + "..")
                    .Append(Me.AptifyApplication.GetEntityBaseView("Persons").ToString)
                    .Append(" WHERE Email1='" + Email.Replace("'", "''") + "'")
                    .Append(" AND FirstName = '" + FirstName.Replace("'", "''") + "' ")
                    .Append(" AND LastName = '" + LastName.Replace("'", "''") + "' ")
                End With
                lValue = Me.DataAction.ExecuteScalar(sbrPersonId.ToString())
                If lValue IsNot Nothing AndAlso IsNumeric(lValue) AndAlso CLng(lValue) > 0 Then
                    Return CLng(lValue)
                Else
                    Dim oPerson As Aptify.Framework.BusinessLogic.GenericEntity.AptifyGenericEntityBase
                    oPerson = Me.AptifyApplication.GetEntityObject("Persons", -1)
                    oPerson.SetValue("FirstName", FirstName)
                    oPerson.SetValue("LastName", LastName)
                    oPerson.SetValue("Email1", Email)
                    If oPerson.Save(False) Then
                        Return oPerson.RecordID
                    End If
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Function

        Protected Sub grdmember_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdmember.RowDataBound
            Try
                Dim imagememberid As New Image
                imagememberid = CType(e.Row.FindControl("imgmember"), Image)
                If Not (DataBinder.Eval(e.Row.DataItem, "photo")) Is Nothing Then

                    If IsDBNull(DataBinder.Eval(e.Row.DataItem, "photo")) Then
                        imagememberid.ImageUrl = "~/Images/blankphoto.gif"
                    Else
                        If (DirectCast(DataBinder.Eval(e.Row.DataItem, "photo"), Byte()).Length() > 0) Then
                            Dim base64String As String = Convert.ToBase64String(DirectCast(DataBinder.Eval(e.Row.DataItem, "photo"), Byte()), 0, DirectCast(DataBinder.Eval(e.Row.DataItem, "photo"), Byte()).Length())
                            imagememberid.ImageUrl = "data:image/png;base64," & base64String
                        Else
                            imagememberid.ImageUrl = "~/Images/blankphoto.gif"
                        End If
                    End If
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        ''' <summary>
        ''' IssuID - 3240 Added by Suvarna D validate if Record with EmailID already exists in grid.
        ''' </summary>
        ''' <param name="sEmail"></param>
        ''' <param name="iIndex"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Function ValidateGridRecords(ByVal sEmail As String, ByVal iIndex As Integer) As Boolean
            Dim bResult As Boolean = True
            For rowIndex As Integer = 0 To grdAddMember.Rows.Count - 1
                Dim lblEmail As Label = DirectCast(grdAddMember.Rows(rowIndex).FindControl("lblAttendeeEmail"), Label)
                If rowIndex <> iIndex Then
                    If String.Compare(lblEmail.Text.Trim.ToString(), sEmail, True) = 0 Then
                        bResult = False
                        Exit For
                    End If
                End If
            Next
            'Anil B For issue 16936 on 11 July 2013
            'Check if Attendee Already added to the cart.
            If bResult Then
                bResult = CheckAttendeeExistInCart(sEmail)
            End If
            Return bResult
        End Function
        'Anil B For issue 16936 on 11 July 2013
        'Function to Check Attendee is added to the cart.
        Private Function CheckAttendeeExistInCart(ByVal sEmail As String) As Boolean
            Dim bResult As Boolean = True
            Dim iItem As Integer
            Dim lAttendeeID As Long
            Dim sSQL As String
            Dim oOrderGE As Aptify.Applications.OrderEntry.OrdersEntity
            Dim sEmailID As String
            Dim lProductID As Long
            Dim oOrderLine As OrderLinesEntity
            Dim oMeetingDetail As AptifyGenericEntityBase
            Dim lMeetingID As Integer = CInt(ViewState("MeetingID"))
            oOrderGE = Me.ShoppingCart1.GetOrderObject(Session, Page.User, Application)
            For iItem = 0 To oOrderGE.SubTypes("OrderLines").Count - 1
                oOrderLine = CType(oOrderGE.SubTypes("OrderLines").Item(iItem), OrderLinesEntity)
                If oOrderLine IsNot Nothing Then
                    lProductID = CLng(oOrderLine.GetValue("ProductID"))
                    If "Meeting" = ShoppingCart1.GetProductType(lProductID) Then
                        oMeetingDetail = oOrderLine.ExtendedOrderDetailEntity
                        Me.LoadMeetingDetailFromXML(oMeetingDetail, CStr(oOrderLine.GetValue("__ExtendedAttributeObjectData")))
                        lAttendeeID = CLng(oMeetingDetail.GetValue("AttendeeID"))
                        'Anil B for Issue 17050 on 24-july-2013
                        If lAttendeeID > 0 Then
                            sSQL = "SELECT Email FROM " & Database & ".." & AptifyApplication.GetEntityBaseView("Persons") & " WHERE ID = " & lAttendeeID
                            sEmailID = CStr(DataAction.ExecuteScalar(sSQL))
                            If String.Compare(sEmailID.Trim, sEmail.Trim, True) = 0 AndAlso lMeetingID = lProductID Then
                                bResult = False
                                Exit For
                            End If
                        End If
                    End If
                End If
            Next
            Return bResult
        End Function

        Protected Sub btnVGROk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVGROk.Click
            radValidateGrdRec.VisibleOnPageLoad = False
        End Sub

        Protected Sub btnChangeEamilOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChangeEamilOk.Click
            radChangeEmail.VisibleOnPageLoad = False
        End Sub
        'Anil B for Issue 14381
        Protected Overridable Sub FillDropDown()
            Try
                Dim sSQLGetFoodPreference, sSQLGetTravelPreference As String
                Dim dt_FoodPreference As DataTable
                Dim dt_TravelPreference As DataTable
                sSQLGetFoodPreference = "EXEC spGetAllFoodPreference"
                sSQLGetTravelPreference = "EXEC spGetAllTravelPreference"
                dt_FoodPreference = Me.DataAction.GetDataTable(sSQLGetFoodPreference)
                If dt_FoodPreference IsNot Nothing AndAlso dt_FoodPreference.Rows.Count > 0 Then
                    ddlFoodPreference.DataSource = dt_FoodPreference
                    ddlFoodPreference.DataTextField = "Name"
                    ddlFoodPreference.DataValueField = "ID"
                    ddlFoodPreference.DataBind()

                    ddlPopFoodPreference.DataSource = dt_FoodPreference
                    ddlPopFoodPreference.DataTextField = "Name"
                    ddlPopFoodPreference.DataValueField = "ID"
                    ddlPopFoodPreference.DataBind()

                End If
                dt_TravelPreference = Me.DataAction.GetDataTable(sSQLGetTravelPreference)
                If dt_TravelPreference IsNot Nothing AndAlso dt_TravelPreference.Rows.Count > 0 Then
                    ddlTravelPreference.DataSource = dt_TravelPreference
                    ddlTravelPreference.DataTextField = "Name"
                    ddlTravelPreference.DataValueField = "ID"
                    ddlTravelPreference.DataBind()


                    ddlPopTravelPreference.DataSource = dt_TravelPreference
                    ddlPopTravelPreference.DataTextField = "Name"
                    ddlPopTravelPreference.DataValueField = "ID"
                    ddlPopTravelPreference.DataBind()
                End If   
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub


        Protected Sub btnAddRegistrant_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddRegistrant.Click

            Dim lProductID As Long = -1
            Dim bSaveRegistration As Boolean
            Dim oOrderGE As OrdersEntity
            Dim iCount As Integer = 1
            Dim lstAtendeeID As New ArrayList
            oOrderGE = Me.ShoppingCart1.GetOrderObject(Session, Page.User, Application)
            If ViewState(ATTRIBUTE_ATTENDEE_MEETINGPRODUCTID) IsNot Nothing Then
                lProductID = CLng(ViewState(ATTRIBUTE_ATTENDEE_MEETINGPRODUCTID))
            End If
            'IssueID:14817 Amruta
            If grdAddMember.Rows.Count > 0 Then

                For i As Integer = 0 To grdAddMember.Rows.Count - 1

                    If AddRegistrants(i, iCount, oOrderGE) Then
                        bSaveRegistration = True
                    End If

                    iCount += 1

                Next
                Me.ShoppingCart1.SaveCart(Me.Session)

            End If
            'Neha changes for Issue 18313,Product Prerequisites implementation for the meeting and session products.

            If ViewState(ATTRIBUTE_ATTENDEE_SELECTEDMEETING) IsNot Nothing Then
                descSelectedSessionForAttendee = CType(ViewState(ATTRIBUTE_ATTENDEE_SELECTEDMEETING), Dictionary(Of Integer, List(Of Integer)))
            End If
            Dim sWebMessage As String = ""
            Dim sMessageCount As Integer = 0
            If bSaveRegistration = True Then
                For Each pair In descSelectedSessionForAttendee
                    Dim sProductID As Long
                    Dim lstSessionList As List(Of Integer) = pair.Value
                    Dim lProductSessionID As Long
                    For Each lProductSessionID In lstSessionList
                        sProductID = lProductSessionID
                        ViewState("ProductID") = sProductID
                        If Not GetWebPrerequisiteMsg(sWebMessage) Then
                        Else
                            sMessageCount = sMessageCount + 1
                        End If
                    Next
                Next
               
                If sMessageCount = 0 Then
                    MyBase.Response.Redirect(RedirectURL, False)
                Else
                    Dim i As Integer, ol As OrderLinesEntity
                    While i < oOrderGE.SubTypes("OrderLines").Count
                        i = 0
                        ol = CType(oOrderGE.SubTypes("OrderLines").Item(i), OrderLinesEntity)

                        oOrderGE.SubTypes("OrderLines").Remove(ol)
                    End While
                    lblAdded.Visible = True
                    lblAdded.Text = sWebMessage
                    lblAdded.ForeColor = Drawing.Color.Red
                    Me.ShoppingCart1.SaveCart(Me.Session)
                End If
            End If
        End Sub
        'IssueID 14817 Amruta
        Protected Function AddRegistrants(ByVal IrowIndex As Integer, ByVal iCount As Integer, ByVal OrderGE As Aptify.Applications.OrderEntry.OrdersEntity) As Boolean
            Try
                Dim lProductID As Long = CLng(Request.QueryString("ID"))
                'Dim lProductID As Long = Session("OL")
                Dim sAttendeeFirstLast As String = ""
                Dim sAttendeeEmail As String = ""
                Dim bSuccess As Boolean = False
                Dim sBadgeName As String = ""
                Dim sBadgeTitle As String = ""
                Dim sBadgeCompany As String = ""
                Dim sAttendeeID As String = ""
                Dim sFoodPreference As String = ""
                Dim sTravelPreference As String = ""
                Dim sGolfHandicap As String = ""
                Dim sSpecialRequest As String = ""
                Dim sOtherPreference As String = ""

                Dim oPrice As New IProductPrice.PriceInfo
                Dim oProduct As Aptify.Applications.ProductSetup.ProductObject

                With grdAddMember
                    sAttendeeFirstLast = CType(.Rows(IrowIndex).FindControl("lblAtendeeFirstName"), Label).Text & " " & CType(.Rows(IrowIndex).FindControl("lblAtendeeLastName"), Label).Text
                    sAttendeeEmail = CType(.Rows(IrowIndex).FindControl("lblAttendeeEmail"), Label).Text
                    sBadgeName = DirectCast(.Rows(IrowIndex).FindControl("lblBadgeName"), Label).Text
                    sBadgeTitle = DirectCast(.Rows(IrowIndex).FindControl("lblBadgeTitle"), Label).Text
                    sBadgeCompany = DirectCast(.Rows(IrowIndex).FindControl("lblBadgeCompany"), Label).Text

                    sFoodPreference = DirectCast(.Rows(IrowIndex).FindControl("lblFoodPreference1"), Label).Text
                    sTravelPreference = DirectCast(.Rows(IrowIndex).FindControl("plblTravelPreference"), Label).Text
                    sGolfHandicap = DirectCast(.Rows(IrowIndex).FindControl("plblGolfHandicap"), Label).Text
                    sSpecialRequest = DirectCast(.Rows(IrowIndex).FindControl("plblSpecialRequest"), Label).Text
                    sOtherPreference = DirectCast(.Rows(IrowIndex).FindControl("plblotherPreference"), Label).Text

                    sAttendeeID = Convert.ToString(FindCreatePerson(CType(.Rows(IrowIndex).FindControl("lblAtendeeFirstName"), Label).Text, CType(.Rows(IrowIndex).FindControl("lblAtendeeLastName"), Label).Text, sAttendeeEmail))

                End With
                oProduct = DirectCast(AptifyApplication.GetEntityObject("Products", lProductID), Aptify.Applications.ProductSetup.ProductObject)

                With OrderGE.AddProduct(lProductID).Item(0)
                    If oProduct.ProductKitType = ProductData.ProductKitType.ProductGroup Then
                        SetupExtendedOrderDetailsforProdGroupType(OrderGE)
                    End If
                    .GetPrice(oPrice, lProductID, 1, CLng(sAttendeeID), ProductGE:=oProduct, CurrencyTypeID:=User1.PreferredCurrencyTypeID)
                    'Anil B for Issue 15369 on 04-May-2013
                    'Set the product order line description if allow description checkbox is checked from product
                    If .AllowDescriptionOverride Then
                        .DescriptionChanged = True
                        .SetValue("Description", "Registration for " & sAttendeeFirstLast)
                        .SetAddValue("_XEbizDescription", .GetValue("Description"))
                    End If
                    .SetValue("Price", oPrice.Price)
                    If Not IsNothing(.ExtendedOrderDetailEntity) Then
                        .ExtendedOrderDetailEntity.SetValue("ProductID", CStr(lProductID))
                        .ExtendedOrderDetailEntity.SetValue("AttendeeID", sAttendeeID)
                        .ExtendedOrderDetailEntity.SetValue("AttendeeID_Email", sAttendeeEmail)
                        .ExtendedOrderDetailEntity.SetAddValue("AttendeeFirstLast", sAttendeeFirstLast)
                        .ExtendedOrderDetailEntity.SetValue("BadgeName", sBadgeName)
                        .ExtendedOrderDetailEntity.SetValue("BadgeCompanyName", sBadgeCompany)
                        .ExtendedOrderDetailEntity.SetValue("BadgeTitle", sBadgeTitle)

                        .ExtendedOrderDetailEntity.SetValue("FoodPreferenceID", IIf(sFoodPreference <> "", sFoodPreference, ""))
                        .ExtendedOrderDetailEntity.SetValue("TravelPreferenceID", IIf(sTravelPreference <> "", sTravelPreference, ""))
                        .ExtendedOrderDetailEntity.SetValue("GolfHandicap", sGolfHandicap)
                        .ExtendedOrderDetailEntity.SetValue("SpecialRequest", sSpecialRequest)
                        .ExtendedOrderDetailEntity.SetValue("Other", sOtherPreference)

                        If ProdInventoryLedgerExist(lProductID) Then
                            Dim iAvalableSpace As Integer = 0
                            iAvalableSpace = FindAvailableSpaceforMeeting(lProductID, OrderGE)

                            If iAvalableSpace < CInt(lblAvailableSpace.Text) Then
                                iAvalableSpace = CInt(lblAvailableSpace.Text)
                            ElseIf iAvalableSpace >= CInt(lblAvailableSpace.Text) Then
                                iAvalableSpace = 0
                            End If

                            If iCount > iAvalableSpace Or iAvalableSpace = 0 Then
                                .ExtendedOrderDetailEntity.SetValue("StatusID", GetRegisteredStatusID("Waiting"))
                            Else
                                .ExtendedOrderDetailEntity.SetValue("StatusID", GetRegisteredStatusID("Registered"))
                            End If
                        Else
                            .ExtendedOrderDetailEntity.SetValue("StatusID", GetRegisteredStatusID("Registered"))
                        End If

                        .SetAddValue("__ExtendedAttributeObjectData", .ExtendedOrderDetailEntity.GetObjectData(False))
                        If Not SaveMeetingSessions(OrderGE, CLng(sAttendeeID), sAttendeeFirstLast, IrowIndex + 1, IrowIndex) Then
                            Return False
                            Exit Function
                        End If
                    End If
                End With
                Return True
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return False
            End Try
        End Function

        Private Function GetRegisteredStatusID(ByVal sStatus As String) As Long
            Dim sSQL As String
            sSQL = "SELECT ID FROM " & Database & _
                   "..vwAttendeeStatus WHERE Name= '" & sStatus & "'"
            Return CLng(DataAction.ExecuteScalar(sSQL))
        End Function
        'Anil B for Issue 14381
        'Registered selected meeting session for perticuler user
        Private Function SaveMeetingSessions(ByVal OrderGE As OrdersEntity, _
                              ByVal AttendeeID As Long, ByVal sAttendeeFirstLast As String, _
                              ByVal ParentSequence As Integer,
                              ByVal IrowIndex As Integer) As Boolean
            Try
                ' go through the check boxes and add one line to the order for each such item
                Dim lProductID As Long, sBaseKey As String, i As Integer
                Dim oParInfo As New Generic.List(Of ParentChildInfo)
                Dim oOrderLine As OrderLinesEntity
                Dim oOrder As OrdersEntity
                Dim pair As KeyValuePair(Of Integer, List(Of Integer))
                'Dim oMeetingDetail As AptifyGenericEntityBase
                'IssueID 14817 Amruta
                Dim iLine As Integer
                oOrder = ShoppingCart1.GetOrderObject(Page.Session, Page.User, Page.Application)
                iLine = CInt(oOrder.SubTypes("OrderLines").Count - 1)
                oOrderLine = CType(OrderGE.SubTypes("OrderLines").Item(iLine), OrderLinesEntity)
                Dim dtBadgeInformation As DataTable = Nothing
                BuildParentInfo(OrderGE, oParInfo, ParentSequence)
                dtBadgeInformation = LoadBadgeInfo(AttendeeID)
                If ViewState(ATTRIBUTE_ATTENDEE_SELECTEDMEETING) IsNot Nothing Then
                    descSelectedSessionForAttendee = CType(ViewState(ATTRIBUTE_ATTENDEE_SELECTEDMEETING), Dictionary(Of Integer, List(Of Integer)))
                End If
                'Neha changes for 18313
                Dim sWebErrorMsg As String = ""
                If descSelectedSessionForAttendee.Count > 0 Then
                    For Each pair In descSelectedSessionForAttendee
                        If IrowIndex + 1 = pair.Key Then
                            Dim lstSessionList As List(Of Integer) = pair.Value
                            Dim lProductSessionID As Long
                            For Each lProductSessionID In lstSessionList
                                lProductID = lProductSessionID
                                ViewState("ProductID") = lProductID
                                If Not GetWebPrerequisiteMsg(sWebErrorMsg) Then
                                    lblAdded.Visible = False
                                    oOrderLine = OrderGE.AddProduct(lProductID).Item(0)
                                    SetOrderLineData(oOrderLine, IrowIndex, AttendeeID, sAttendeeFirstLast, dtBadgeInformation)
                                End If
                            Next
                        End If
                    Next
                End If

                If oOrder IsNot Nothing Then
                    For Each oOrderLine In oOrder.SubTypes("OrderLines")
                        If oOrderLine.ExtendedOrderDetailEntity IsNot Nothing AndAlso oOrderLine.GetValue("__ExtendedAttributeObjectData") Is Nothing Then
                            SetOrderLineData(oOrderLine, IrowIndex, AttendeeID, sAttendeeFirstLast, dtBadgeInformation)
                        End If
                    Next
                End If
                dtBadgeInformation.Clear()
                dtBadgeInformation = Nothing
                Dim iNumFound As Integer = 0
                i = 0
                While i < OrderGE.SubTypes("OrderLines").Count
                    With OrderGE.SubTypes("OrderLines").Item(i)
                        If CInt(.GetValue("ParentSequence")) = ParentSequence Then

                            If i <> ParentSequence + iNumFound Then
                                OrderGE.SubTypes("OrderLines").MoveObject(i, ParentSequence + iNumFound, AptifyMoveObjectOptions.aptifyShift)
                            Else
                                i += 1
                            End If
                            iNumFound += 1
                        Else
                            i += 1
                        End If
                    End With
                End While

                AdjustParentInfo(OrderGE, oParInfo)
                Return True
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Function
        'Neha changes for Issue 18313,to show web pre-requisite message for the top level meeting product.
        Private Function GetWebPrerequisiteMsg(ByRef sWebErrorMsg As String) As Boolean
            Dim oOrder As New Aptify.Applications.OrderEntry.OrdersEntity
            oOrder = ShoppingCart1.GetOrderObject(Page.Session, Page.User, Page.Application)
            Dim sProductID As Long = CLng(ViewState("ProductID"))
            Dim PrerequisitesOverridePromptResult As Microsoft.VisualBasic.MsgBoxResult
            Try
                If Not oOrder.ValidateProductPrerequisites(sProductID, 1, PrerequisitesOverridePromptResult) Then
                    sWebErrorMsg = CStr(oOrder.WebProdPreRequisiteErrMsg)
                    Return True
                Else
                    sWebErrorMsg = Nothing
                    Return False
                End If
            Catch ex As Exception
                Return False
            End Try
        End Function
        Private Sub SetOrderLineData(ByVal oOrderLine As OrderLinesEntity, ByVal iRowIndex As Integer, ByVal AttendeeID As Long, ByVal sAttendeeFirstLast As String, ByVal dtBadgeInformation As DataTable)
            Try
                Dim lblBadgeName As Label = DirectCast(grdAddMember.Rows(iRowIndex).FindControl("lblBadgeName"), Label)
                Dim lblBadgeTitle As Label = DirectCast(grdAddMember.Rows(iRowIndex).FindControl("lblBadgeTitle"), Label)
                Dim lblBadgeCompany As Label = DirectCast(grdAddMember.Rows(iRowIndex).FindControl("lblBadgeCompany"), Label)
                Dim lblgFoodPreference As Label = DirectCast(grdAddMember.Rows(iRowIndex).FindControl("lblFoodPreference1"), Label)
                Dim lblgTravelPreference As Label = DirectCast(grdAddMember.Rows(iRowIndex).FindControl("plblTravelPreference"), Label)
                Dim lblgGolfHandicap As Label = DirectCast(grdAddMember.Rows(iRowIndex).FindControl("plblGolfHandicap"), Label)
                Dim lblgSpecialRequest As Label = DirectCast(grdAddMember.Rows(iRowIndex).FindControl("plblSpecialRequest"), Label)
                Dim lblgOtherPreference As Label = DirectCast(grdAddMember.Rows(iRowIndex).FindControl("plblotherPreference"), Label)
                Dim strBadgeName As String = String.Empty, strBadgeCompanyName As String = String.Empty, strBadgeTitle As String = String.Empty
                With oOrderLine

                    If .AllowDescriptionOverride Then
                        .DescriptionChanged = True
                        .SetValue("Description", "Registration for " & sAttendeeFirstLast)
                        .SetAddValue("_XEbizDescription", .GetValue("Description"))
                    End If
                    'IssueID 14817 Amruta
                    .ExtendedOrderDetailEntity.SetValue("FoodPreferenceID", IIf(lblgFoodPreference.Text <> "", lblgFoodPreference.Text, ""))
                    .ExtendedOrderDetailEntity.SetValue("TravelPreferenceID", IIf(lblgTravelPreference.Text <> "", lblgTravelPreference.Text, ""))
                    .ExtendedOrderDetailEntity.SetValue("GolfHandicap", lblgGolfHandicap.Text)
                    .ExtendedOrderDetailEntity.SetValue("SpecialRequest", lblgSpecialRequest.Text)
                    .ExtendedOrderDetailEntity.SetValue("Other", lblgOtherPreference.Text)
                    'Changes made by sandeep for Issue no 14437 on 13/02/2013
                    'Add Badge Information to OrdermeetingDetails
                    If dtBadgeInformation IsNot Nothing AndAlso dtBadgeInformation.Rows.Count > 0 Then
                        If String.IsNullOrEmpty(lblBadgeName.Text) Then
                            strBadgeName = Convert.ToString(dtBadgeInformation.Rows(0)("FirstLast"))
                        Else
                            strBadgeName = lblBadgeName.Text
                        End If
                        If String.IsNullOrEmpty(lblBadgeCompany.Text) Then
                            strBadgeCompanyName = Convert.ToString(dtBadgeInformation.Rows(0)("CompanyName"))
                        Else
                            strBadgeCompanyName = lblBadgeCompany.Text
                        End If
                        If String.IsNullOrEmpty(lblBadgeTitle.Text) Then
                            strBadgeTitle = Convert.ToString(dtBadgeInformation.Rows(0)("Title"))
                        Else
                            strBadgeTitle = lblBadgeTitle.Text
                        End If

                    Else
                        strBadgeName = lblBadgeName.Text
                        strBadgeCompanyName = lblBadgeCompany.Text
                        strBadgeTitle = lblBadgeTitle.Text
                    End If

                    .ExtendedOrderDetailEntity.SetValue("BadgeName", strBadgeName)
                    .ExtendedOrderDetailEntity.SetValue("BadgeCompanyName", strBadgeCompanyName)
                    .ExtendedOrderDetailEntity.SetValue("BadgeTitle", strBadgeTitle)
                    .ExtendedOrderDetailEntity.SetValue("AttendeeID", AttendeeID)
                    .SetAddValue("__ExtendedAttributeObjectData", .ExtendedOrderDetailEntity.GetObjectData(False))
                End With
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        'Private Sub AdjustParentInfo(ByVal OrderGE As OrdersEntity, _
        '                         ByVal ParInfo As Generic.List(Of ParentChildInfo))
        '    Dim iSeq As Integer
        '    For Each par As ParentChildInfo In ParInfo
        '        iSeq = 1 + OrderGE.SubTypes("OrderLines").IndexOf(par.ParentLine)
        '        For Each subline As OrderLinesEntity In par.SubLines
        '            subline.SetValue("ParentSequence", iSeq)
        '        Next
        '    Next
        'End Sub

        'Added by sandeep for Issue no 14437
        'Retrive Badge Information For meeting Order
        Protected Function LoadBadgeInfo(ByVal lAttendeeID As Long) As DataTable
            Dim sSQL As String = String.Empty
            Dim dtbadgeInfo As DataTable = Nothing
            sSQL = "SELECT VP.FirstLast,VP.Title,VC.Name As CompanyName, VP.Email1 as EmailID, VP.ID as AttendeeID  FROM " & AptifyApplication.GetEntityBaseDatabase("Persons") & ".." & AptifyApplication.GetEntityBaseView("Persons") & " VP Left Outer join " & AptifyApplication.GetEntityBaseDatabase("Companies") & ".." & AptifyApplication.GetEntityBaseView("Companies") & " VC on VP.CompanyID =VC.ID Where VP.ID =" & lAttendeeID
            dtbadgeInfo = DataAction.GetDataTable(sSQL, Framework.DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
            Return dtbadgeInfo
        End Function



        ''' <remarks></remarks>
        Private Sub BuildParentInfo(ByVal OrderGE As OrdersEntity, _
                                    ByVal ParInfo As Generic.List(Of ParentChildInfo), _
                                    ByVal ExcludeParentSequence As Integer)
            For Each ol As OrderLinesEntity In OrderGE.SubTypes("OrderLines")
                If (OrderGE.SubTypes("OrderLines").IndexOf(ol) + 1) <> _
                   ExcludeParentSequence Then
                    If ol.GetValue("ParentSequence") Is Nothing OrElse _
                       CInt(ol.GetValue("ParentSequence")) <= 0 Then
                        ' top level, find its children
                        ParInfo.Add(New ParentChildInfo)
                        With ParInfo.Item(ParInfo.Count - 1)
                            Dim iSeq As Integer
                            .ParentLine = ol
                            iSeq = OrderGE.SubTypes("OrderLines").IndexOf(ol) + 1
                            For Each subline As OrderLinesEntity In OrderGE.SubTypes("OrderLines")
                                If CInt(subline.GetValue("ParentSequence")) = iSeq Then
                                    .SubLines.Add(subline)
                                End If
                            Next
                        End With
                    End If
                End If
            Next
        End Sub
        'Anil B for Issue 14381
        'Load Meeting Session
        Private Sub LoadMeetingSession()
            Try
                Dim sSQL As String
                Dim params(0) As IDataParameter
                Dim m_sDatabase As String = AptifyApplication.GetEntityBaseDatabase("Certifications")
                Dim m_oDA As DataAction
                Dim dt As Data.DataTable
                Dim oPrice As New Aptify.Applications.OrderEntry.IProductPrice.PriceInfo
                Dim dcol As DataColumn = New DataColumn()
                m_oDA = New DataAction(Me.AptifyApplication.UserCredentials)
                sSQL = "EXEC spGetMeetingSessionsForRegistration @MeetingID=" & CInt(ViewState("MeetingID"))
                dt = m_oDA.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                    dcol.Caption = "Price"
                    dcol.ColumnName = "Price"
                    dt.Columns.Add(dcol)
                    For Each rw As DataRow In dt.Rows
                        oPrice = Me.ShoppingCart1.GetUserProductPrice(CLng(rw("ProductID")))
                        If oPrice.Price = 0 Then
                            rw("Price") = "Free "
                        Else
                            rw("Price") = Format(oPrice.Price, User1.PreferredCurrencyFormat)
                        End If
                    Next

                    Me.grdMeetingSession.DataSource = dt
                    grdMeetingSession.DataBind()
                    If ViewState(ATTRIBUTE_ALLSESSION) Is Nothing Then
                        ViewState(ATTRIBUTE_ALLSESSION) = dt
                    End If
                    ''grdMeetingSession.Visible = False
                    grdMeetingSession.Visible = True

                Else
                    grdMeetingSession.Visible = False
                End If

                SaveValuesforSession()

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        ''RashmiP, Issue MeetingClass Product Session not loaded.
        Private Sub SaveValuesforSession()
            Dim dicMeetingSessions As New Dictionary(Of Integer, Boolean)
            Dim index As Integer = -1
            Dim result As Boolean

            For Each item As Telerik.Web.UI.GridDataItem In grdMeetingSession.MasterTableView.Items
                index = CInt(DirectCast(item.FindControl("lblProductID"), Label).Text)
                result = DirectCast(item.FindControl("chkSession"), CheckBox).Checked
                If dicMeetingSessions.ContainsKey(index) Then
                    dicMeetingSessions.Remove(index)
                End If
                dicMeetingSessions.Add(index, result)
            Next

            If dicMeetingSessions IsNot Nothing AndAlso dicMeetingSessions.Count > 0 Then
                ViewState(ATTRIBUTE_CHECKED_SESSION) = dicMeetingSessions
            End If
        End Sub

        Protected Sub grdMeetingSession_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdMeetingSession.ItemCreated
            Dim chkSubscription As CheckBox = DirectCast(e.Item.FindControl("chkSession"), CheckBox)
            Dim chkAllSessions As CheckBox
            Dim lbProductID As Label = DirectCast(e.Item.FindControl("lblProductID"), Label)
            Dim dicSubscriptionDetails As New Dictionary(Of Integer, Boolean)
            Dim dicMeetingStatus As New Dictionary(Of Integer, Boolean)
            Dim iSessionID As Integer
            Dim dataItem As DataRowView
            If IsPostBack Then
                chkAllSessions = DirectCast(e.Item.FindControl("chkAllSession"), CheckBox)
                If chkAllSessions IsNot Nothing Then
                    If ViewState("CheckAll") IsNot Nothing And CBool(ViewState("CheckAll")) And chkAllSessions IsNot Nothing Then
                        chkAllSessions.Checked = True
                    Else
                        chkAllSessions.Checked = False
                    End If
                End If

            End If

            'If chkSubscription IsNot Nothing Then
            '    If ViewState(ATTRIBUTE_CHECKED_SESSION) IsNot Nothing Then 'Added by Sandeep For Issue 14671 on 27/02/2013
            '        dicSubscriptionDetails = DirectCast(ViewState(ATTRIBUTE_CHECKED_SESSION), Dictionary(Of Integer, Boolean))
            '        dataItem = DirectCast(e.Item.DataItem, System.Data.DataRowView)
            '        If dataItem IsNot Nothing Then
            '            iSessionID = CInt(dataItem("ProductID"))
            '            If dicSubscriptionDetails.ContainsKey(iSessionID) Then
            '                chkSubscription.Checked = dicSubscriptionDetails.Item(iSessionID)
            '            End If
            '        End If
            '    End If

            'End If
            If chkSubscription IsNot Nothing Then
                If ViewState(ATTRIBUTE_CHECKED_SESSION) IsNot Nothing Then
                    dicSubscriptionDetails = DirectCast(ViewState(ATTRIBUTE_CHECKED_SESSION), Dictionary(Of Integer, Boolean))
                    If ViewState(ATTRIBUTE_SESION_STATUS) IsNot Nothing Then
                        dicMeetingStatus = DirectCast(ViewState(ATTRIBUTE_SESION_STATUS), Dictionary(Of Integer, Boolean))
                        If dicMeetingStatus.ContainsKey(e.Item.DataSetIndex) Then
                            chkSubscription.Checked = dicMeetingStatus.Item(e.Item.DataSetIndex)
                        End If
                    End If


                End If

            End If

        End Sub

        Protected Sub grdMeetingSession_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdMeetingSession.ItemDataBound
            Try
                If (TypeOf (e.Item) Is GridDataItem) Then
                    Dim dateColumns As New List(Of String)
                    'Add datecolumn uniqueName in list for Date format
                    dateColumns.Add("gridDateTimeColumnStartDate")
                    dateColumns.Add("gridDateTimeColumnEndDate")
                    CommonMethods.FormatedDateOnGrid(dateColumns, e.Item)



                    Dim chkSubscription As CheckBox = DirectCast(e.Item.FindControl("chkSession"), CheckBox)
                    Dim dicSubscriptionDetails As New Dictionary(Of Integer, Boolean)
                    If chkSubscription IsNot Nothing Then
                        If ViewState(ATTRIBUTE_CHECKED_SESSION) IsNot Nothing Then 'Added by Sandeep For Issue 14671 on 27/02/2013
                            dicSubscriptionDetails = DirectCast(ViewState(ATTRIBUTE_CHECKED_SESSION), Dictionary(Of Integer, Boolean))
                            Dim dataItem As DataRowView
                            Dim iSessionID As Integer
                            dataItem = DirectCast(e.Item.DataItem, System.Data.DataRowView)
                            If dataItem IsNot Nothing Then
                                iSessionID = CInt(dataItem("ProductID"))
                                If dicSubscriptionDetails.ContainsKey(iSessionID) Then
                                    chkSubscription.Checked = dicSubscriptionDetails.Item(iSessionID)
                                End If
                            End If
                        End If

                    End If

                End If
                If TypeOf e.Item Is GridPagerItem Then
                    Dim pager As GridPagerItem = DirectCast(e.Item, GridPagerItem)
                    Dim PageSizeComboBox As RadComboBox = DirectCast(pager.FindControl("PageSizeComboBox"), RadComboBox)
                    Dim ChangePageSizeLabel As Label = DirectCast(pager.FindControl("ChangePageSizeLabel"), Label)
                    PageSizeComboBox.Visible = False
                    ChangePageSizeLabel.Visible = False
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub grdMeetingSession_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdMeetingSession.NeedDataSource
            If ViewState(ATTRIBUTE_ALLSESSION) IsNot Nothing Then
                grdMeetingSession.DataSource = CType(ViewState(ATTRIBUTE_ALLSESSION), DataTable)
            End If
        End Sub
        Private Sub SaveCheckedValues()
            Dim dicMeetingSessions As New Dictionary(Of Integer, Boolean)
            Dim index As Integer = -1
            Dim result As Boolean
            For Each item As Telerik.Web.UI.GridDataItem In grdMeetingSession.MasterTableView.Items
                index = CInt(DirectCast(item.FindControl("lblProductID"), Label).Text)
                result = DirectCast(item.FindControl("chkSession"), CheckBox).Checked
                If ViewState(ATTRIBUTE_CHECKED_SESSION) IsNot Nothing Then
                    dicMeetingSessions = DirectCast(ViewState(ATTRIBUTE_CHECKED_SESSION), Dictionary(Of Integer, Boolean))
                End If
                If dicMeetingSessions.ContainsKey(index) Then
                    dicMeetingSessions.Remove(index)
                End If
                dicMeetingSessions.Add(index, result)
            Next
            If dicMeetingSessions IsNot Nothing AndAlso dicMeetingSessions.Count > 0 Then
                ViewState(ATTRIBUTE_CHECKED_SESSION) = dicMeetingSessions
            End If
        End Sub
        'Anil B for Issue 14381
        'Ok Button for selected session window
        Protected Sub btnEditSession_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEditSession.Click
            Try
                radPopUpEditListSession.VisibleOnPageLoad = False
                Dim sConflictionOption As String = ""
                Dim ConflictFlag As Boolean = False
                Dim lstSession As New List(Of Integer)
                ' Dim chkSession As CheckBox
                ' Dim rowGridItem As GridDataItem
                ' Dim iIndex As Integer
                'Dim lblSessionID As Label
                Dim chkAll As CheckBox
                Dim dicMeetingSessions As New Dictionary(Of Integer, Boolean)
                SaveCheckedValues()
                sConflictionOption = GetConflictionOptionByProductID(CLng(ViewState("MeetingID")))
                If sConflictionOption.ToUpper() = "CONFLICT WARNING" OrElse sConflictionOption.ToUpper() = "CONFLICT PROHIBITED" Then
                    FindConflictedMeetingSessionForRegistration(ConflictFlag, sConflictionOption)
                    If ConflictFlag Then
                        radMeetingSessionConflictMessage.VisibleOnPageLoad = True
                        Exit Sub
                    End If
                End If
                If ViewState(ATTRIBUTE_ATTENDEE_SELECTEDMEETING) IsNot Nothing Then
                    descSelectedSessionForAttendee = CType(ViewState(ATTRIBUTE_ATTENDEE_SELECTEDMEETING), Dictionary(Of Integer, List(Of Integer)))
                End If
                If ViewState(ATTRIBUTE_CHECKED_SESSION) IsNot Nothing Then
                    dicMeetingSessions = DirectCast(ViewState(ATTRIBUTE_CHECKED_SESSION), Dictionary(Of Integer, Boolean))
                End If
                'For Each dataItem As GridDataItem In grdMeetingSession.Items
                '    chkSession = CType(dataItem.FindControl("chkSession"), CheckBox)
                '    If chkSession.Checked = True Then
                '        rowGridItem = DirectCast(chkSession.Parent.Parent, GridDataItem)
                '        iIndex = rowGridItem.DataSetIndex
                '        lblSessionID = CType(grdMeetingSession.Items(iIndex).FindControl("lblProductID"), Label)
                '        lstSession.Add(CInt(lblSessionID.Text))
                '    End If
                'Next
                For Each skey As KeyValuePair(Of Integer, Boolean) In dicMeetingSessions 'Added By sandeep For Issue 13879 on 06/05/2013
                    If skey.Value = True Then
                        If Not lstSession.Contains(skey.Key) Then
                            lstSession.Add(skey.Key)
                        End If

                    End If
                Next
                DeleteMeetingSessionFromDictionary(CInt(hdnGrdRowIndex.Value))
                descSelectedSessionForAttendee.Add(CInt(hdnGrdRowIndex.Value), lstSession)
                ViewState(ATTRIBUTE_ATTENDEE_SELECTEDMEETING) = descSelectedSessionForAttendee

                For Each headerItem As GridHeaderItem In grdMeetingSession.MasterTableView.GetItems(GridItemType.Header)
                    chkAll = DirectCast(headerItem.FindControl("chkAllSession"), CheckBox)
                    If chkAll IsNot Nothing Then
                        chkAll.Checked = False
                    End If
                Next
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Protected Sub btnCancelSession_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelSession.Click
            radPopUpEditListSession.VisibleOnPageLoad = False
        End Sub
        'Anil B for Issue 14381
        'Delete Selected Session from Dictionary control
        Private Sub DeleteMeetingSessionFromDictionary(ByVal Index As Integer)
            If ViewState(ATTRIBUTE_ATTENDEE_SELECTEDMEETING) IsNot Nothing Then
                descSelectedSessionForAttendee = CType(ViewState(ATTRIBUTE_ATTENDEE_SELECTEDMEETING), Dictionary(Of Integer, List(Of Integer)))
            End If
            For iIndex As Integer = descSelectedSessionForAttendee.Count - 1 To 0 Step -1
                Dim item = descSelectedSessionForAttendee.ElementAt(iIndex)
                Dim itemKey = item.Key
                If itemKey = Index Then
                    descSelectedSessionForAttendee.Remove(itemKey)
                End If
            Next
            ViewState(ATTRIBUTE_ATTENDEE_SELECTEDMEETING) = descSelectedSessionForAttendee
        End Sub
        'Anil B for Issue 14381
        Private Sub ClereSession()
            Dim chkSession As CheckBox
            For Each dataItem As GridDataItem In grdMeetingSession.Items
                chkSession = CType(dataItem.FindControl("chkSession"), CheckBox)
                chkSession.Checked = False
            Next
        End Sub
        'Anil B for Issue 14381
        'Update the current order line 
        Protected Sub btnUpdateAttendeeInfo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdateAttendeeInfo.Click
            Try
                If Request.QueryString("OL") IsNot Nothing AndAlso CInt(Request.QueryString("OL")) <> -1 AndAlso IsNumeric(Request.QueryString("OL")) Then
                    Dim iLine As Integer = CInt(Request.QueryString("OL"))
                    Dim oOrderLine As OrderLinesEntity
                    Dim oOrderGE As OrdersEntity
                    oOrderGE = Me.ShoppingCart1.GetOrderObject(Session, Page.User, Application)
                    oOrderLine = CType(oOrderGE.SubTypes("OrderLines").Item(iLine), OrderLinesEntity)
                    'Validate Person to avaoid duplicate records
                    ''Case icount = 1  when 1 or more records already Exists in database
                    ''else case icount = 0 when entered last name and Email matches with the record in database but first name do not matches. This case will be 
                    ''Applicable iif there is single record exists in the database
                    Dim icount As Int32
                    icount = ValidatePerson()
                    If icount = 1 Then
                        radDuplicateUser.VisibleOnPageLoad = True
                        Exit Sub
                    ElseIf icount = 0 Then
                        LoadMember()
                        radSimilarRecords.VisibleOnPageLoad = True
                        Exit Sub
                    End If
                    Dim sAttendeeID As String = Convert.ToString(FindCreatePerson(txtFirstName.Text, txtLastName.Text, txtEmail.Text))
                    Dim oGE As Aptify.Applications.Persons.PersonsEntity = DirectCast(Me.AptifyApplication.GetEntityObject("Persons", CLng(sAttendeeID)), Aptify.Applications.Persons.PersonsEntity)

                    If oGE IsNot Nothing Then
                        Dim sAttendeeFirstLast As String = oGE.FirstLast
                        With oOrderLine
                            .SetValue("Description", "Registration for " & sAttendeeFirstLast)
                            'IssueID 14817 Amruta
                            .ExtendedOrderDetailEntity.SetValue("FoodPreferenceID", IIf(ddlFoodPreference.Text <> "", ddlFoodPreference.Text, ""))
                            .ExtendedOrderDetailEntity.SetValue("TravelPreferenceID", IIf(ddlTravelPreference.Text <> "", ddlTravelPreference.Text, ""))
                            .ExtendedOrderDetailEntity.SetValue("GolfHandicap", txtGolfHandicape.Text)
                            .ExtendedOrderDetailEntity.SetValue("SpecialRequest", txtSpecialRequest.Text)
                            .ExtendedOrderDetailEntity.SetValue("Other", txtOtherPreference.Text)

                            .ExtendedOrderDetailEntity.SetValue("BadgeName", txtBadgeName.Text)
                            .ExtendedOrderDetailEntity.SetValue("BadgeCompanyName", txtCompany.Text)
                            .ExtendedOrderDetailEntity.SetValue("BadgeTitle", txtBadgeTitle.Text)
                            .ExtendedOrderDetailEntity.SetValue("AttendeeID", sAttendeeID)
                            .SetAddValue("__ExtendedAttributeObjectData", .ExtendedOrderDetailEntity.GetObjectData(False))
                        End With
                        Me.ShoppingCart1.SaveCart(Me.Session)
                    End If
                End If
                MyBase.Response.Redirect(RedirectURL, False)
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        'Anil B for Issue 14381
        'Select All check box functionality for session
        Protected Sub ToggleSelectedState(ByVal sender As Object, ByVal e As EventArgs)
            Try
                'Dim headerCheckBox As CheckBox = CType(sender, CheckBox)
                'For Each dataItem As GridDataItem In grdMeetingSession.MasterTableView.Items
                '    CType(dataItem.FindControl("chkSession"), CheckBox).Checked = headerCheckBox.Checked
                '    dataItem.Selected = headerCheckBox.Checked
                'Next
                Dim dicMeetingSessions As New Dictionary(Of Integer, Boolean)
                Dim headerCheckBox As CheckBox = CType(sender, CheckBox)
                Dim dtAllSession As DataTable
                For Each dataItem As GridDataItem In grdMeetingSession.MasterTableView.Items
                    CType(dataItem.FindControl("chkSession"), CheckBox).Checked = headerCheckBox.Checked
                    dataItem.Selected = headerCheckBox.Checked

                Next
                If ViewState(ATTRIBUTE_ALLSESSION) IsNot Nothing Then
                    dtAllSession = DirectCast(ViewState(ATTRIBUTE_ALLSESSION), DataTable)
                End If
                If dtAllSession IsNot Nothing AndAlso dtAllSession.Rows.Count > 0 Then
                    If headerCheckBox.Checked = True Then
                        ViewState("CheckAll") = True
                        For Each dr As DataRow In dtAllSession.Rows
                            If ViewState(ATTRIBUTE_CHECKED_SESSION) IsNot Nothing Then
                                dicMeetingSessions = DirectCast(ViewState(ATTRIBUTE_CHECKED_SESSION), Dictionary(Of Integer, Boolean))
                            End If
                            If dicMeetingSessions.ContainsKey(CInt(dr("ProductID"))) Then
                                dicMeetingSessions.Remove(CInt(dr("ProductID")))
                            End If
                            dicMeetingSessions.Add(CInt(dr("ProductID")), True)
                        Next
                    Else
                        ViewState("CheckAll") = False
                        For Each dr As DataRow In dtAllSession.Rows
                            If ViewState(ATTRIBUTE_CHECKED_SESSION) IsNot Nothing Then
                                dicMeetingSessions = DirectCast(ViewState(ATTRIBUTE_CHECKED_SESSION), Dictionary(Of Integer, Boolean))
                            End If
                            If dicMeetingSessions.ContainsKey(CInt(dr("ProductID"))) Then
                                dicMeetingSessions.Remove(CInt(dr("ProductID")))
                            End If
                            dicMeetingSessions.Add(CInt(dr("ProductID")), False)
                        Next
                    End If
                    dtAllSession = Nothing
                    ViewState(ATTRIBUTE_CHECKED_SESSION) = dicMeetingSessions

                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Protected Sub btnMeetingSessionCountInfo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMeetingSessionCountInfo.Click
            radMeetingSessionCountInfo.VisibleOnPageLoad = False
        End Sub
        'Anil B for Issue 14381
        'find  conflicted Option from meeting entity
        Private Function GetConflictionOptionByProductID(ByVal MeetingProductID As Long) As String
            Dim sSQL As String
            sSQL = "SELECT MeetingConflictionChecker FROM " & Database & ".." & AptifyApplication.GetEntityBaseView("Meetings") & " WHERE ProductID = " & MeetingProductID
            Return CStr(DataAction.ExecuteScalar(sSQL))
        End Function
        'Anil B for Issue 14381
        'find meeting dates for check conflicted session
        Private Sub GetMeetingDates(ByVal vProductID As String, ByRef Startdate As DateTime, ByRef Enddate As DateTime)
            Dim dtGetMeeting As New DataTable
            Dim sSQL As String
            sSQL = "Select ID, StartDate, EndDate from " & AptifyApplication.GetEntityBaseDatabase("Meetings") & ".." & AptifyApplication.GetEntityBaseView("Meetings") & " WHERE ProductID = " & CInt(vProductID)
            dtGetMeeting = Me.DataAction.GetDataTable(sSQL)
            If dtGetMeeting.Rows.Count > 0 Then
                Startdate = DirectCast(dtGetMeeting.Rows(0)("StartDate"), DateTime)
                Enddate = DirectCast(dtGetMeeting.Rows(0)("EndDate"), DateTime)
            End If
        End Sub
        'Anil B for Issue 14381
        'Implement to find conflicted session
        Private Sub FindConflictedMeetingSessionForRegistration(ByRef ConflictFlag As Boolean, ByVal sConflictionOption As String) 'Changes Made By Sandeep for Issue 13879 
            Try
                'Dim SessionList As New ArrayList
                'Dim drRow As DataRow
                'Dim dcColumn As DataColumn
                'Dim dtItems As New DataTable
                'Dim lblProduct As Label
                'Dim newStartdate, newEndDate, nextStartdate, nextEndDate As DateTime
                'Dim chkSession As CheckBox
                'Dim ErrorFlag As Boolean = False
                'Dim lblProductNext As Label
                'Dim chkSessionNext As CheckBox
                'Dim drFirstRow As DataRow
                'Dim dtGetMeeting As New DataTable
                'Dim sSQL As String
                'Dim errorMessage As String = String.Empty
                'SessionList.Clear()
                'If ViewState("Items") Is Nothing Then
                '    dcColumn = New DataColumn()
                '    dcColumn.DataType = Type.GetType("System.String")
                '    dcColumn.ColumnName = "ErrorMessage"
                '    dtItems.Columns.Add(dcColumn)
                '    ViewState("Items") = dtItems
                'End If
                'For Each dataItem As GridDataItem In grdMeetingSession.Items
                '    lblProduct = TryCast(dataItem.FindControl("lblProductID"), Label)
                '    chkSession = TryCast(dataItem.FindControl("chkSession"), CheckBox)
                '    If chkSession.Checked Then
                '        For Each dgItem As GridDataItem In grdMeetingSession.Items
                '            lblProductNext = TryCast(dgItem.FindControl("lblProductID"), Label)
                '            chkSessionNext = TryCast(dgItem.FindControl("chkSession"), CheckBox)
                '            If chkSessionNext.Checked And lblProduct.Text.Trim() <> lblProductNext.Text.Trim() Then
                '                GetMeetingDates(lblProduct.Text.Trim(), newStartdate, newEndDate)
                '                GetMeetingDates(lblProductNext.Text.Trim(), nextStartdate, nextEndDate)
                '                If CheckSessionConfliction(newStartdate, newEndDate, nextStartdate, nextEndDate) Then
                '                    ErrorFlag = True
                '                    ConflictFlag = True
                '                    If dtItems.Rows.Count = 0 Then
                '                        drFirstRow = dtItems.NewRow()
                '                        drFirstRow("ErrorMessage") = "You cannot continue the registration process because a scheduling conflict exists between two or more sessions.Please modify your selections and try again. We detected a conflict for the following sessions:"
                '                        dtItems.Rows.Add(drFirstRow)
                '                    End If
                '                    sSQL = "Select ID,ProductID_Name, StartDate, EndDate from " & AptifyApplication.GetEntityBaseDatabase("Products") & "..vwMeetings WHERE ProductID = " & CInt(lblProduct.Text)
                '                    dtGetMeeting = Me.DataAction.GetDataTable(sSQL)
                '                    errorMessage = Convert.ToString(dtGetMeeting.Rows(0)("ProductID_Name"))
                '                    drRow = dtItems.NewRow()
                '                    Dim vDuplicateMessage As Boolean = False
                '                    For Each dr As DataRow In dtItems.Rows
                '                        If dr("ErrorMessage").ToString() = errorMessage Then
                '                            vDuplicateMessage = True
                '                        End If
                '                    Next
                '                    If Not vDuplicateMessage Then
                '                        drRow("ErrorMessage") = errorMessage
                '                        dtItems.Rows.Add(drRow)
                '                    End If
                '                End If
                '            End If
                '        Next
                '    End If
                'Next
                'lstErrorMessage.DataSource = dtItems
                'lstErrorMessage.DataBind()
                'dtItems.Clear()
                'ViewState("Items") = Nothing
                Dim SessionList As New List(Of Integer)
                Dim dtItems As New DataTable
                Dim dtConfictSessions As DataTable = Nothing
                Dim dcColumn As DataColumn
                Dim dicMeetingSessions As New Dictionary(Of Integer, Boolean)
                If ViewState(ATTRIBUTE_SESSION_DT) Is Nothing Then
                    dcColumn = New DataColumn()
                    dcColumn.DataType = Type.GetType("System.String")
                    dcColumn.ColumnName = "ErrorMessage"
                    dtItems.Columns.Add(dcColumn)
                    ViewState(ATTRIBUTE_SESSION_DT) = dtItems
                End If
                If ViewState(ATTRIBUTE_CHECKED_SESSION) IsNot Nothing Then
                    dicMeetingSessions = DirectCast(ViewState(ATTRIBUTE_CHECKED_SESSION), Dictionary(Of Integer, Boolean))
                End If
                If dicMeetingSessions IsNot Nothing AndAlso dicMeetingSessions.Count > 0 Then
                    For Each MeetingSesion As KeyValuePair(Of Integer, Boolean) In dicMeetingSessions
                        If MeetingSesion.Value = True Then
                            Select Case sConflictionOption.ToUpper()
                                Case "NO CONFLICT VALIDATION"
                                    SessionList.Add(MeetingSesion.Key)

                                Case "CONFLICT WARNING"
                                    If Not CheckMeetingSessionConflict(MeetingSesion.Key.ToString(), ConflictFlag, dicMeetingSessions) Then
                                        SessionList.Add(MeetingSesion.Key)
                                    End If

                                Case "CONFLICT PROHIBITED"
                                    If Not CheckMeetingSessionConflict(MeetingSesion.Key.ToString(), ConflictFlag, dicMeetingSessions) Then
                                        SessionList.Add(MeetingSesion.Key)
                                    End If
                            End Select
                        End If
                    Next
                End If

                If ViewState(ATTRIBUTE_SESSION_DT) IsNot Nothing Then
                    dtConfictSessions = DirectCast(ViewState(ATTRIBUTE_SESSION_DT), DataTable)
                End If
                If dtConfictSessions.Rows.Count > 0 Then
                    lstErrorMessage.DataSource = dtConfictSessions
                    lstErrorMessage.DataBind()
                End If

                ViewState(ATTRIBUTE_SESSION_DT) = Nothing
                Session("MeetingSessions") = SessionList
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Private Function CheckMeetingSessionConflict(ByVal strProduct As String, ByRef ConflictFlag As Boolean, ByVal dicMeetingSessions As Dictionary(Of Integer, Boolean)) As Boolean
            Dim dtItems As DataTable = Nothing
            Dim dtGetMeeting As DataTable = Nothing
            Dim lblProductNext As Label
            Dim chkSessionNext As CheckBox
            Dim drFirstRow, drRow As DataRow
            Dim sSQL As String = String.Empty
            Dim errorMessage As String = String.Empty
            Dim newStartdate, newEndDate, nextStartdate, nextEndDate As DateTime
            Dim vDuplicateMessage As Boolean
            Dim bErrorFlag As Boolean = False
            If ViewState(ATTRIBUTE_SESSION_DT) IsNot Nothing Then
                dtItems = DirectCast(ViewState(ATTRIBUTE_SESSION_DT), DataTable)
            End If
            For Each MeetingSession As KeyValuePair(Of Integer, Boolean) In dicMeetingSessions
                If MeetingSession.Value = True And strProduct.Trim() <> MeetingSession.Key.ToString() Then
                    GetMeetingDates(strProduct.Trim(), newStartdate, newEndDate)
                    GetMeetingDates(MeetingSession.Key.ToString().Trim(), nextStartdate, nextEndDate)
                    If CheckSessionConfliction(newStartdate, newEndDate, nextStartdate, nextEndDate) Then
                        bErrorFlag = True
                        ConflictFlag = True
                        If dtItems IsNot Nothing AndAlso dtItems.Rows.Count = 0 Then
                            drFirstRow = dtItems.NewRow()
                            drFirstRow("ErrorMessage") = "You cannot continue the registration process because a scheduling conflict exists between two or more sessions. Please modify your selections and try again. We detected a conflict for the following sessions:"
                            dtItems.Rows.Add(drFirstRow)
                        End If
                        sSQL = "Select ID,ProductID_Name, StartDate, EndDate from " & AptifyApplication.GetEntityBaseDatabase("Products") & "..vwMeetings WHERE ProductID = " & CInt(strProduct)
                        dtGetMeeting = Me.DataAction.GetDataTable(sSQL)
                        errorMessage = Convert.ToString(dtGetMeeting.Rows(0)("ProductID_Name"))
                        drRow = dtItems.NewRow()
                        vDuplicateMessage = False
                        For Each dr As DataRow In dtItems.Rows
                            If dr("ErrorMessage").ToString() = errorMessage Then
                                vDuplicateMessage = True
                            End If
                        Next

                        If Not vDuplicateMessage Then
                            drRow("ErrorMessage") = errorMessage
                            dtItems.Rows.Add(drRow)
                        End If

                    End If
                End If
            Next

            Return bErrorFlag
        End Function
        'Anil B for 14381 
        Protected Sub btnMeetingSessionConflictOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMeetingSessionConflictOK.Click
            radMeetingSessionConflictMessage.VisibleOnPageLoad = False
            radPopUpEditListSession.VisibleOnPageLoad = True
        End Sub
        'Amruta, IssueID 14380 , Function to return user meeting registration status
        Private Function LoadUserMeetingStatus() As DataTable
            Dim sSQL As String = String.Empty
            Dim dtMeetingStatusDetails As DataTable = Nothing
            Dim iProductID As Integer
            iProductID = CInt(Request.QueryString("ID"))

            sSQL = "SELECT OM.AttendeeStatus_Name,OM.AttendeeID,OM.ProductID,P.FirstName,P.LastName,P.Email FROM " & AptifyApplication.GetEntityBaseDatabase("OrderMeetingDetail") & ".." & AptifyApplication.GetEntityBaseView("OrderMeetingDetail") & " OM INNER JOIN " & AptifyApplication.GetEntityBaseDatabase("Persons") & ".." & AptifyApplication.GetEntityBaseView("Persons") & " P ON P.ID = OM.AttendeeID " & " Where P.FirstName = '" & txtFirstName.Text.Trim.Replace("'", "''") & "' AND P.LastName = '" & txtLastName.Text.Trim.Replace("'", "''") & "' And P.Email = '" & txtEmail.Text.Trim.Replace("'", "''") & "' And OM.ProductID = " & iProductID
            dtMeetingStatusDetails = DataAction.GetDataTable(sSQL)
            If dtMeetingStatusDetails IsNot Nothing Then
                For Each rw As DataRow In dtMeetingStatusDetails.Rows
                    If rw("AttendeeStatus_Name").ToString() = "Registered" Or rw("AttendeeStatus_Name").ToString() = "Waiting" Or rw("AttendeeStatus_Name").ToString() = "Attended" Then
                        lblError.Visible = True
                        lblError.Text = "You have already registered for this meeting."
                        Return dtMeetingStatusDetails
                    End If
                Next
            End If
            dtMeetingStatusDetails = Nothing
            Return dtMeetingStatusDetails
        End Function

        Protected Sub grdMeetingSession_PageIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageChangedEventArgs) Handles grdMeetingSession.PageIndexChanged
            SaveCheckedValues()
        End Sub

        Protected Sub grdMeetingSession_PageSizeChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageSizeChangedEventArgs) Handles grdMeetingSession.PageSizeChanged
            SaveCheckedValues()
        End Sub

        Private Sub SetupExtendedOrderDetailsforProdGroupType(ByRef oOrder As OrdersEntity)
            Try
                Dim ProductGE As Aptify.Applications.ProductSetup.ProductObject
                Dim dtBadgeInformation As DataTable = Nothing
                Dim strAttendeeID As String = String.Empty
                Dim strBadgeName As String = String.Empty, strBadgeCompanyName As String = String.Empty, strBadgeTitle As String = String.Empty, strBadgeEmailID As String = String.Empty
                dtBadgeInformation = LoadBadgeInfo(Me.User1.PersonID)

                For Each ol As OrderLinesEntity In oOrder.SubTypes("OrderLines")
                    With ol
                        Dim iProductID As Long = CLng(ol.GetValue("ProductID"))
                        ProductGE = CType(AptifyApplication.GetEntityObject("Products", iProductID), Aptify.Applications.ProductSetup.ProductObject)
                        If iProductID > 0 Then
                            If "Meeting" = ShoppingCart1.GetProductType(iProductID) AndAlso Convert.ToInt64(ol.ExtendedOrderDetailEntity.GetValue("AttendeeID")) = Me.User1.PersonID Then
                                If Not IsNothing(.ExtendedOrderDetailEntity) Then
                                    'Add Badge Information to OrdermeetingDetails
                                    If dtBadgeInformation IsNot Nothing AndAlso dtBadgeInformation.Rows.Count > 0 Then
                                        If String.IsNullOrEmpty(strBadgeName) Then
                                            strBadgeName = Convert.ToString(dtBadgeInformation.Rows(0)("FirstLast"))
                                        End If
                                        If String.IsNullOrEmpty(strBadgeCompanyName) Then
                                            strBadgeCompanyName = Convert.ToString(dtBadgeInformation.Rows(0)("CompanyName"))
                                        End If
                                        If String.IsNullOrEmpty(strBadgeTitle) Then
                                            strBadgeTitle = Convert.ToString(dtBadgeInformation.Rows(0)("Title"))
                                        End If
                                        If String.IsNullOrEmpty(strBadgeEmailID) Then
                                            strBadgeEmailID = Convert.ToString(dtBadgeInformation.Rows(0)("EmailID"))
                                        End If
                                        If String.IsNullOrEmpty(strAttendeeID) Then
                                            strAttendeeID = Convert.ToString(dtBadgeInformation.Rows(0)("AttendeeID"))
                                        End If
                                    End If

                                End If
                                'Set the Badge information for the Meeting to OrdermeetingDetails
                                .ExtendedOrderDetailEntity.SetValue("BadgeName", strBadgeName)
                                .ExtendedOrderDetailEntity.SetValue("BadgeCompanyName", strBadgeCompanyName)
                                .ExtendedOrderDetailEntity.SetValue("BadgeTitle", strBadgeTitle)
                                .ExtendedOrderDetailEntity.SetValue("AttendeeID", strAttendeeID)
                                .ExtendedOrderDetailEntity.SetValue("AttendeeID_Email", strBadgeEmailID)
                                'Set values in temp field
                                .SetAddValue("__ExtendedAttributeObjectData", .ExtendedOrderDetailEntity.GetObjectData(False))
                            End If
                        End If
                    End With
                Next
            Catch ex As Exception
            End Try
        End Sub

    End Class
End Namespace
