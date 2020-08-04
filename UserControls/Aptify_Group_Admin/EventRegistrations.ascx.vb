'Aptify e-Business 5.5.1, July 2013
#Region "Namespace"

Imports Aptify.Framework.DataServices
Imports System.IO
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports System.Data
Imports Telerik.Web.UI
Imports System.Web
Imports Telerik.Web
Imports Telerik
Imports System.Drawing
Imports Telerik.Web.UI.Calendar
#End Region
Namespace Aptify.Framework.Web.eBusiness.CustomerService
    Partial Class EventRegistrations
        Inherits BaseUserControlAdvanced
        Protected Const ATTRIBUTE_EVENTREGISTRATION_PAGE As String = "EventRegMeeting"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "EventRegistrations"
        Protected Const ATTRIBUTE_EVENTREG_PAGE As String = "EventRegistrationsDetailsURL"
        Protected Const ATTRIBUTE_BADGE_PAGE As String = "EditBadgeInformation"
        'Issue 14639 Add meeting page url by sachin
        Protected Const ATTRIBUTE_MEETING_PAGE As String = "MeetingPage"
        Dim hidden As New HiddenField
        Private Const GET_COUNTOF_REGISTERED_WAITING_PERSONS_VIEWNAME As String = "vwGetCountOfRegisteredWaitingPerson"
        Protected Const ATTRIBUTE_LOGIN_PAGE As String = "LoginPage" 'Added by Sandeep for Issue 15051 on 12/03/2013
        Protected Const ATTRIBUTE_SHOWMEETINGSLINKTOCLASS_DEFAULT_NAME As String = "ShowMeetingsLinkToClass"

#Region "WhatsNew Specific Properties"
        ''' <summary>
        ''' Evennt page url
        ''' </summary>
        Public Overridable Property EventRegMeeting() As String
            Get
                If Not ViewState.Item(ATTRIBUTE_EVENTREGISTRATION_PAGE) Is Nothing Then
                    Return CStr(ViewState.Item(ATTRIBUTE_EVENTREGISTRATION_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState.Item(ATTRIBUTE_EVENTREGISTRATION_PAGE) = value
            End Set
        End Property

        ''' <summary>
        ''' Meeting page url
        ''' </summary>
        Public Overridable Property MeetingPage() As String
            Get
                If Not ViewState.Item(ATTRIBUTE_MEETING_PAGE) Is Nothing Then
                    Return CStr(ViewState.Item(ATTRIBUTE_MEETING_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState.Item(ATTRIBUTE_MEETING_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
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
        Public Overridable Property EditBadgeInformation() As String
            Get
                If Not ViewState.Item(ATTRIBUTE_BADGE_PAGE) Is Nothing Then
                    Return CStr(ViewState.Item(ATTRIBUTE_BADGE_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                'ViewState.Item(ATTRIBUTE_BADGE_PAGE) = value
                ViewState(ATTRIBUTE_BADGE_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' Product page url
        ''' </summary>
        Public Overridable Property EventRegistrationsDetailsURL() As String
            Get
                If Not ViewState.Item(ATTRIBUTE_EVENTREG_PAGE) Is Nothing Then
                    Return CStr(ViewState.Item(ATTRIBUTE_EVENTREG_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                'ViewState.Item(ATTRIBUTE_EVENTREG_PAGE) = value
                ViewState(ATTRIBUTE_EVENTREG_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        Protected Overridable ReadOnly Property ShowMeetingsLinkToClass() As Boolean
            Get
                If Not ViewState.Item(ATTRIBUTE_SHOWMEETINGSLINKTOCLASS_DEFAULT_NAME) Is Nothing Then
                    Return CBool(ViewState.Item(ATTRIBUTE_SHOWMEETINGSLINKTOCLASS_DEFAULT_NAME))
                Else
                    Dim value As String = Me.GetGlobalAttributeValue(ATTRIBUTE_SHOWMEETINGSLINKTOCLASS_DEFAULT_NAME)
                    If Not String.IsNullOrEmpty(value) Then
                        Select Case UCase(value)
                            Case "TRUE", "FALSE", "0", "1"
                                ViewState.Item(ATTRIBUTE_SHOWMEETINGSLINKTOCLASS_DEFAULT_NAME) = CBool(value)
                            Case Else
                                ViewState.Item(ATTRIBUTE_SHOWMEETINGSLINKTOCLASS_DEFAULT_NAME) = False
                        End Select
                    Else
                        ViewState.Item(ATTRIBUTE_SHOWMEETINGSLINKTOCLASS_DEFAULT_NAME) = False
                    End If
                End If
            End Get
        End Property
#End Region


        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(EventRegistrationsDetailsURL) Then
                EventRegistrationsDetailsURL = Me.GetLinkValueFromXML(ATTRIBUTE_EVENTREG_PAGE)
                If String.IsNullOrEmpty(EventRegistrationsDetailsURL) Then
                    Me.grdResults.Enabled = False
                    Me.grdResults.ToolTip = "EventRegistrationsDetailsURL property has not been set."
                End If
            End If
            'Issue 14639 Set Meeting page url property
            If String.IsNullOrEmpty(MeetingPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                MeetingPage = Me.GetLinkValueFromXML(ATTRIBUTE_MEETING_PAGE)
                If String.IsNullOrEmpty(MeetingPage) Then
                    Me.grdResults.Enabled = False
                    Me.grdResults.ToolTip = "MeetingPage property has not been set."
                End If
            End If
            If String.IsNullOrEmpty(EventRegMeeting) Then
                EventRegMeeting = Me.GetLinkValueFromXML(ATTRIBUTE_EVENTREGISTRATION_PAGE)
                If String.IsNullOrEmpty(EventRegMeeting) Then
                    Me.grdResults.Enabled = False
                    Me.grdResults.ToolTip = "EventRegistrationsURL property has not been set."
                End If
            End If
            If String.IsNullOrEmpty(EditBadgeInformation) Then
                EditBadgeInformation = Me.GetLinkValueFromXML(ATTRIBUTE_BADGE_PAGE)
                If String.IsNullOrEmpty(EditBadgeInformation) Then
                    Me.grdResults.Enabled = False
                    Me.grdResults.ToolTip = "EventURL property has not been set."
                End If
            End If
            'Added by Sandeep for Issue 15051 on 12/03/2013
            If String.IsNullOrEmpty(LoginPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                LoginPage = Me.GetLinkValueFromXML(ATTRIBUTE_LOGIN_PAGE)
            End If
            Dim bShowMeeting As Boolean = ShowMeetingsLinkToClass()
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Try
                SetProperties()
                If Not IsPostBack Then
                    AddExpression()
                    hidden.Value = "Future"
                    dtpMonthYearPicker.MinDate = Date.Today
                    RadMonthYearPickerPast.MaxDate = Date.Today
                    GetEvent()
                    GetEventPast()
                    GetCountEventRegistrations(-1)


                End If
                If User1.UserID < 0 Then
                    Response.Redirect(LoginPage) 'Added by Sandeep for Issue 15051 on 12/03/2013
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
#Region "Functions and Methods"
        Private Sub GetEvent()
            Dim sSQL As String, dt As DataTable
            Dim bShowMeeting As Boolean = ShowMeetingsLinkToClass()
            Try
                'Suraj Issue 15404 -9-19-13 ,upcomming event sort by start date asc
                sSQL = "SELECT distinct vwp.Productid,  case when (vp.WebName is null or vp.WebName = '') 	then vp.Name 	Else vp.WebName End as MeetingTitle,  vp.ClassID,vwp.StartDate " & _
                          "FROM " & _
                          Database & _
                          ".." & GET_COUNTOF_REGISTERED_WAITING_PERSONS_VIEWNAME & " vwp Inner join vwProducts vp on vwp.ProductID =vp.ID WHERE vwp.EndDate>=CAST(CAST(getdate() As VARCHAR(12)) AS DATETIME) AND vwp.StartDate>=CAST(CAST(getdate() As VARCHAR(12)) AS DATETIME) and vwp.parentMeetingID is null and vp.WebEnabled=1 AND vwp.CompanyID=" & Convert.ToString(User1.CompanyID)
                ''Rashmi P, issue 15456: e-Business: GroupAdmin Dashboard: Show Meetings Associated With Class Shows Up In The Events Section
                If Not bShowMeeting Then
                    sSQL &= "  AND  ISNULL(vp.ClassID ,-1) <=0 "
                End If
                sSQL &= " Order by StartDate"
                Dim itemid As New RadComboBoxItem()
                itemid.Text = "--Select Upcoming Event--"
                itemid.Value = "-1"
                dt = Me.DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                ddlEvent.Items.Clear()
                ddlEvent.Items.Add(New ListItem("--Select Upcoming Event--", "-1"))
                If Not dt Is Nothing Then
                    For Each dr As DataRow In dt.Rows
                        ddlEvent.Items.Add(New ListItem(dr("MeetingTitle").ToString.Trim, dr("Productid").ToString))
                    Next
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub GetEventPast()
            Dim sSQL As String, dt As DataTable
            Dim bShowMeeting As Boolean = ShowMeetingsLinkToClass()
            Try
                'Suraj Issue 15404 -9-19-13 ,past event sort by end date desc
                sSQL = "SELECT distinct vwp.Productid,case when (vp.WebName is null or vp.WebName = '') 	then vp.Name 	Else vp.WebName End as MeetingTitle,vwp.EndDate " & _
                          "FROM " & _
                          Database & _
                           ".." & GET_COUNTOF_REGISTERED_WAITING_PERSONS_VIEWNAME & " vwp Inner join vwProducts vp on vwp.ProductID =vp.ID WHERE vwp.StartDate<CAST(CAST(getdate() As VARCHAR(12)) AS DATETIME) AND vwp.parentMeetingID is null and vp.WebEnabled=1 AND vwp.CompanyID=" & Convert.ToString(User1.CompanyID)
                ''Rashmi P, issue 15456: e-Business: GroupAdmin Dashboard: Show Meetings Associated With Class Shows Up In The Events Section
                If Not bShowMeeting Then
                    sSQL &= "  AND  ISNULL(vp.ClassID ,-1) <=0 "
                End If
                sSQL &= " Order by EndDate Desc"
                Dim itemid As New RadComboBoxItem()
                itemid.Text = "--Select Past Event--"
                itemid.Value = "-1"
                dt = Me.DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                ddlPastEvent.Items.Clear()
                ddlPastEvent.Items.Add(New ListItem("--Select Past Event--", "-1"))
                If Not dt Is Nothing Then
                    For Each dr As DataRow In dt.Rows
                        ddlPastEvent.Items.Add(New ListItem(dr("MeetingTitle").ToString.Trim, dr("Productid").ToString))
                    Next
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Public Sub GetCountEventRegistrations(ByVal SkipDataBind As Integer)
            Dim sSQL As String = ""
            Dim MeetingStatus As String = ""
            Dim MeetingID As Integer = 0
            Dim CompanyID As Integer
            Dim MonthYear As String
            Dim dt As New DataTable
            Dim bShowMeeting As Boolean = ShowMeetingsLinkToClass()
            CompanyID = Convert.ToString(User1.CompanyID)
            If hidden.Value = "" Then
                MeetingStatus = "Future"
            Else
                MeetingStatus = hidden.Value
            End If
            Try
                MeetingID = Convert.ToInt64(ddlEvent.SelectedValue)
                If MeetingID = -1 Then
                    Dim params(3) As IDataParameter
                    If dtpMonthYearPicker.SelectedDate IsNot Nothing Then
                        MonthYear = String.Format("{0:MM/dd/yyyy}", dtpMonthYearPicker.SelectedDate.ToString())
                        params(3) = Me.DataAction.GetDataParameter("@IsDate", SqlDbType.Bit, 1)
                    Else
                        MonthYear = String.Format("{0:MM/dd/yyyy}", Date.Today.ToString())
                        params(3) = Me.DataAction.GetDataParameter("@IsDate", SqlDbType.Bit, 0)
                    End If

                    sSQL = Database & ".." & "spGetCountEventRegistrations"
                    params(0) = Me.DataAction.GetDataParameter("@CompanyID", SqlDbType.Int, Convert.ToInt64(User1.CompanyID))
                    params(1) = Me.DataAction.GetDataParameter("@CheckDate", SqlDbType.Date, CDate(MonthYear))
                    params(2) = Me.DataAction.GetDataParameter("@MeetingStatus", SqlDbType.NVarChar, MeetingStatus)
                    dt = Me.DataAction.GetDataTableParametrized(sSQL, CommandType.StoredProcedure, params)
                    ''Rashmi P, issue 15456: e-Business: GroupAdmin Dashboard: Show Meetings Associated With Class Shows Up In The Events Section
                    If dt IsNot Nothing Then
                        Dim dtClone As DataTable = dt.Clone
                        If bShowMeeting = False Then

                            Dim dr2 As DataRow() = dt.Select("ClassID is null")

                            For Each row As DataRow In dr2
                                dtClone.ImportRow(row)
                            Next
                            dt.Rows.Clear()
                            dt = dtClone.Copy
                        End If
                    End If
                Else
                    MonthYear = String.Format("{0:MM/dd/yyyy}", dtpMonthYearPicker.SelectedDate.ToString())
                    sSQL = Database & ".." & "spGetCountEventRegistrationsOfMeeting"
                    Dim params(3) As IDataParameter
                    params(0) = Me.DataAction.GetDataParameter("@CompanyID", SqlDbType.Int, Convert.ToInt64(User1.CompanyID))
                    params(1) = Me.DataAction.GetDataParameter("@CheckDate", SqlDbType.Date, CDate(MonthYear))
                    params(2) = Me.DataAction.GetDataParameter("@MeetingStatus", SqlDbType.NVarChar, MeetingStatus)
                    params(3) = Me.DataAction.GetDataParameter("@ProdId", SqlDbType.Int, MeetingID)
                    dt = Me.DataAction.GetDataTableParametrized(sSQL, CommandType.StoredProcedure, params)
                End If
                If dt.Rows.Count > 0 Then
                    Dim dcolUrle As DataColumn = New DataColumn()
                    dcolUrle.Caption = "AdminMeetingTitleUrl"
                    dcolUrle.ColumnName = "AdminMeetingTitleUrl"
                    dt.Columns.Add(dcolUrle)
                    If dt.Rows.Count > 0 Then
                        For Each rw As DataRow In dt.Rows
                            rw("AdminMeetingTitleUrl") = MeetingPage + "?back='back'&ID=" + rw("ProductID").ToString
                        Next
                    End If
                    Dim dcolUrl As DataColumn = New DataColumn()
                    dcolUrl.Caption = "AdminConfirmedUrl"
                    dcolUrl.ColumnName = "AdminConfirmedUrl"
                    dt.Columns.Add(dcolUrl)
                    If dt.Rows.Count > 0 Then
                        For Each rw As DataRow In dt.Rows
                            rw("AdminConfirmedUrl") = EventRegistrationsDetailsURL + "?Confirmed=" + rw("ProductID").ToString  '+ "&MonthYear=" + MonthYear
                        Next
                    End If
                    Dim dcolUrla As DataColumn = New DataColumn()
                    dcolUrla.Caption = "AdminWaitListUrl"
                    dcolUrla.ColumnName = "AdminWaitListUrl"
                    dt.Columns.Add(dcolUrla)
                    If dt.Rows.Count > 0 Then
                        For Each rw As DataRow In dt.Rows
                            rw("AdminWaitListUrl") = EventRegistrationsDetailsURL + "?WaitList=" + rw("ProductID").ToString  '+ "&MonthYear=" + MonthYear
                        Next
                    End If
                    Dim dcolUrlb As DataColumn = New DataColumn()
                    dcolUrlb.Caption = "AdminAllUrl"
                    dcolUrlb.ColumnName = "AdminAllUrl"
                    dt.Columns.Add(dcolUrlb)
                    If dt.Rows.Count > 0 Then
                        For Each rw As DataRow In dt.Rows
                            rw("AdminAllUrl") = EventRegistrationsDetailsURL + "?sAll=" + rw("ProductID").ToString  '+ "&MonthYear=" + MonthYear
                        Next
                    End If
                    'Suraj S Issue 16155 , 5/6/13 , resolve the sorting problem for upcoming events because we check the following conditions and remove the privious lines of code.
                    If dt.Rows.Count > 0 Then
                        lblMessage.Visible = False
                        grdResults.DataSource = dt
                        grdResults.Visible = True
                        lblMessage.Text = ""
                    Else
                        grdResults.Visible = False
                        grdResults.DataSource = dt
                        If SkipDataBind <> -1 Then
                            grdResults.DataBind()
                        End If
                        AddExpression()
                    End If
                Else
                    grdResults.Visible = False
                    lblMessage.Visible = True
                    lblMessage.Text = "There are no meeting registrations for this month. "
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Public Sub GetCountEventRegistrationsPast(ByVal SkipDataBind As Integer)
            Dim sSQL As String = ""
            Dim MeetingStatus As String = ""
            Dim MeetingID As Integer = 0
            Dim CompanyID As Integer
            Dim MonthYear As String
            Dim dt As New DataTable
            CompanyID = Convert.ToString(User1.CompanyID)
            Dim bShowMeeting As Boolean = ShowMeetingsLinkToClass()
            If hidden.Value = "" Then
                MeetingStatus = "Past"
            Else
                MeetingStatus = hidden.Value
            End If
            Try
                MeetingID = Convert.ToInt64(ddlPastEvent.SelectedValue)
                If MeetingID = -1 Then
                    Dim params(3) As IDataParameter
                    If RadMonthYearPickerPast.SelectedDate IsNot Nothing Then
                        MonthYear = String.Format("{0:MM/dd/yyyy}", RadMonthYearPickerPast.SelectedDate.ToString())
                        params(3) = Me.DataAction.GetDataParameter("@IsDate", SqlDbType.Bit, 1)
                    Else
                        MonthYear = String.Format("{0:MM/dd/yyyy}", Date.Today.ToString())
                        params(3) = Me.DataAction.GetDataParameter("@IsDate", SqlDbType.Bit, 0)
                    End If

                    sSQL = Database & ".." & "spGetCountEventRegistrations"
                    params(0) = Me.DataAction.GetDataParameter("@CompanyID", SqlDbType.Int, Convert.ToInt64(User1.CompanyID))
                    params(1) = Me.DataAction.GetDataParameter("@CheckDate", SqlDbType.Date, CDate(MonthYear))
                    params(2) = Me.DataAction.GetDataParameter("@MeetingStatus", SqlDbType.NVarChar, MeetingStatus)
                    dt = Me.DataAction.GetDataTableParametrized(sSQL, CommandType.StoredProcedure, params)
                    ''Rashmi P, issue 15456: e-Business: GroupAdmin Dashboard: Show Meetings Associated With Class Shows Up In The Events Section
                    If dt IsNot Nothing Then
                        Dim dtClone As DataTable = dt.Clone
                        If bShowMeeting = False Then

                            Dim dr2 As DataRow() = dt.Select("ClassID is null")

                            For Each row As DataRow In dr2
                                dtClone.ImportRow(row)
                            Next
                            dt.Rows.Clear()
                            dt = dtClone.Copy
                        End If
                    End If
                Else
                    sSQL = Database & ".." & "spGetCountEventRegistrationsOfMeeting"
                    MonthYear = String.Format("{0:MM/dd/yyyy}", RadMonthYearPickerPast.SelectedDate.ToString())
                    Dim params(3) As IDataParameter
                    params(0) = Me.DataAction.GetDataParameter("@CompanyID", SqlDbType.Int, Convert.ToInt64(User1.CompanyID))
                    params(1) = Me.DataAction.GetDataParameter("@CheckDate", SqlDbType.Date, CDate(MonthYear))
                    params(2) = Me.DataAction.GetDataParameter("@MeetingStatus", SqlDbType.NVarChar, MeetingStatus)
                    params(3) = Me.DataAction.GetDataParameter("@ProdId", SqlDbType.Int, MeetingID)
                    dt = Me.DataAction.GetDataTableParametrized(sSQL, CommandType.StoredProcedure, params)
                End If
                If dt.Rows.Count > 0 Then
                    grdResultsPast.Visible = True
                    lblMessage.Text = ""
                    Past(dt)
                Else
                    grdResultsPast.Visible = False
                    grdResultsPast.DataSource = dt
                    If SkipDataBind <> -1 Then
                        grdResultsPast.DataBind()
                    End If
                    AddExpression()
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
#End Region
        Public Sub Past(ByVal dt As DataTable)
            Dim MonthYear As String
            MonthYear = String.Format("{0:MM/dd/yyyy}", RadMonthYearPickerPast.SelectedDate.ToString())
            Dim dcolUrl As DataColumn = New DataColumn()
            dcolUrl.Caption = "AdminConfirmedUrlPast"
            dcolUrl.ColumnName = "AdminConfirmedUrlPast"
            dt.Columns.Add(dcolUrl)
            If dt.Rows.Count > 0 Then
                For Each rw As DataRow In dt.Rows
                    rw("AdminConfirmedUrlPast") = EventRegistrationsDetailsURL + "?Confirmed=" + rw("ProductID").ToString  '+ "&PMonthYear=" + MonthYear
                Next
            End If
            Dim dcolUrla As DataColumn = New DataColumn()
            dcolUrla.Caption = "AdminWaitListUrlPast"
            dcolUrla.ColumnName = "AdminWaitListUrlPast"
            dt.Columns.Add(dcolUrla)
            If dt.Rows.Count > 0 Then
                For Each rw As DataRow In dt.Rows
                    rw("AdminWaitListUrlPast") = EventRegistrationsDetailsURL + "?WaitList=" + rw("ProductID").ToString ' + "&PMonthYear=" + MonthYear
                Next
            End If
            Dim dcolUrlb As DataColumn = New DataColumn()
            dcolUrlb.Caption = "AdminAllUrlPast"
            dcolUrlb.ColumnName = "AdminAllUrlPast"
            dt.Columns.Add(dcolUrlb)
            If dt.Rows.Count > 0 Then
                For Each rw As DataRow In dt.Rows
                    rw("AdminAllUrlPast") = EventRegistrationsDetailsURL + "?sAll=" + rw("ProductID").ToString  '+ "&PMonthYear=" + MonthYear
                Next
            End If
            Dim dcolUrld As DataColumn = New DataColumn()
            dcolUrld.Caption = "AdminMeetingTitleUrlPast"
            dcolUrld.ColumnName = "AdminMeetingTitleUrlPast"
            dt.Columns.Add(dcolUrld)
            If dt.Rows.Count > 0 Then
                For Each rw As DataRow In dt.Rows
                    rw("AdminMeetingTitleUrlPast") = MeetingPage + "?back='back'&ID=" + rw("ProductID").ToString
                Next
            End If

            If dt.Rows.Count > 0 Then
                lblMessage.Visible = False
                grdResultsPast.DataSource = dt
            Else
                lblMessage.Visible = True
                lblMessage.Text = "There are no meeting registrations for this month."
            End If
        End Sub
        Public Sub Upcoming(ByVal dt As DataTable)
            Dim dcolUrl As DataColumn = New DataColumn()
            dcolUrl.Caption = "AdminConfirmedUrl"
            dcolUrl.ColumnName = "AdminConfirmedUrl"
            dt.Columns.Add(dcolUrl)
            If dt.Rows.Count > 0 Then
                For Each rw As DataRow In dt.Rows
                    rw("AdminConfirmedUrl") = EventRegMeeting + "?Confirmed=" + rw("ProductID").ToString
                Next
            End If
            Dim dcolUrla As DataColumn = New DataColumn()
            dcolUrla.Caption = "AdminWaitListUrl"
            dcolUrla.ColumnName = "AdminWaitListUrl"
            dt.Columns.Add(dcolUrla)
            If dt.Rows.Count > 0 Then
                For Each rw As DataRow In dt.Rows
                    rw("AdminWaitListUrl") = EventRegMeeting + "?WaitList=" + rw("ProductID").ToString
                Next
            End If
            Dim dcolUrlb As DataColumn = New DataColumn()
            dcolUrlb.Caption = "AdminAllUrl"
            dcolUrlb.ColumnName = "AdminAllUrl"
            dt.Columns.Add(dcolUrlb)
            If dt.Rows.Count > 0 Then
                For Each rw As DataRow In dt.Rows
                    rw("AdminAllUrl") = EventRegMeeting + "?sAll=" + rw("ProductID").ToString
                Next
            End If
            Dim dcolUrld As DataColumn = New DataColumn()
            dcolUrld.Caption = "AdminMeetingTitleUrl"
            dcolUrld.ColumnName = "AdminMeetingTitleUrl"
            dt.Columns.Add(dcolUrld)
            If dt.Rows.Count > 0 Then
                For Each rw As DataRow In dt.Rows
                    rw("AdminMeetingTitleUrl") = EventRegistrationsDetailsURL + "?ProductID=" + rw("ProductID").ToString
                Next
            End If
            If dt.Rows.Count > 0 Then
                lblMessage.Visible = False
                grdResultsPast.DataSource = dt
            Else
                lblMessage.Visible = True
                lblMessage.Text = "There are no meeting registrations for this month. "
            End If
        End Sub

        Protected Sub grdResults_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdResults.NeedDataSource
            GetCountEventRegistrations(-1)
        End Sub
        Private Function OrderConfirmationURL() As String
            Throw New NotImplementedException
        End Function
        'Neha changes for issue 16818,for upcoming event fiter not working, 06/21/2013
        Protected Sub ddlEvent_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlEvent.SelectedIndexChanged
            Dim dt As Date = Convert.ToDateTime(getCurrentEventDetails(ddlEvent.SelectedValue))
            If dt < Date.Today Then
                dtpMonthYearPicker.MinDate = dt
            End If
            dtpMonthYearPicker.SelectedDate = dt
            GetCountEventRegistrations(0)
            grdResults.Rebind()

        End Sub
        Protected Sub grdResultsPast_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdResultsPast.NeedDataSource
            GetCountEventRegistrationsPast(-1)
        End Sub
        Protected Sub RadTabEventStrip_TabClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadTabStripEventArgs) Handles RadTabEventStrip.TabClick
            Dim value As String = Convert.ToString(e.Tab.Text)
            hidden.Value = ""
            Select Case value
                Case "Upcoming Events"
                    hidden.Value = "Future"
                    GetCountEventRegistrations(0)
                    'Suraj Issue 15404 -9-19-13 ,comment the below code
                    'grdResults.Rebind()
                    AddExpression()
                Case "Past Events"
                    hidden.Value = "Past"
                    GetEventPast()
                    GetCountEventRegistrationsPast(0)
                    grdResultsPast.Rebind()
                    AddExpression()
                Case Else
            End Select

        End Sub
        'Neha changes for issue 16818,for upcoming event fiter not working, 06/21/2013
        Protected Sub RadMonthYearPicker_SelectedDateChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs) Handles dtpMonthYearPicker.SelectedDateChanged
            GetCountEventRegistrations(0)
            grdResults.Rebind()

        End Sub
        Protected Sub RadMonthYearPickerPast_SelectedDateChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs) Handles RadMonthYearPickerPast.SelectedDateChanged
            GetCountEventRegistrationsPast(0)
            grdResultsPast.Rebind()
        End Sub
        Protected Sub grdResults_PageIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageChangedEventArgs) Handles grdResults.PageIndexChanged
            grdResults.CurrentPageIndex = e.NewPageIndex

            GetCountEventRegistrations(-1)

        End Sub
        Protected Sub grdResultsPast_PageIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageChangedEventArgs) Handles grdResultsPast.PageIndexChanged
            grdResultsPast.CurrentPageIndex = e.NewPageIndex
            GetCountEventRegistrationsPast(-1)
        End Sub
        ''' <summary>
        ''' Get start Date of Selected event
        ''' </summary>
        ''' <param name="SelectedEventID">Is the param of selectd event</param>
        ''' <returns>Start Date of Selected event</returns>
        ''' <remarks></remarks>
        Private Function getCurrentEventDetails(ByVal SelectedEventID As Integer) As String
            If SelectedEventID <> -1 Then
                Dim sSQL As String
                sSQL = "SELECT StartDate " & _
                          "FROM " & _
                          Database & _
                           ".." & GET_COUNTOF_REGISTERED_WAITING_PERSONS_VIEWNAME & " WHERE ProductID= " & SelectedEventID
                Return Me.DataAction.ExecuteScalar(sSQL)
            Else
                Return Date.Today
            End If
        End Function
        Protected Sub ddlPastEvent_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPastEvent.SelectedIndexChanged
            RadMonthYearPickerPast.SelectedDate = getCurrentEventDetails(ddlPastEvent.SelectedValue)
            GetCountEventRegistrationsPast(0)
            grdResultsPast.Rebind()
        End Sub
        ''' <summary>
        ''' This event for show the upcoming meeting session 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdResults_DetailTableDataBind(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridDetailTableDataBindEventArgs) Handles grdResults.DetailTableDataBind
            Dim dtDeatil As DataTable
            Dim sSQL As String = ""
            Dim MeetingStatus As String = ""
            Dim CompanyID As Integer
            Dim MonthYear As String
            Dim dt As New DataTable
            MonthYear = String.Format("{0:MM/dd/yyyy}", dtpMonthYearPicker.SelectedDate.ToString())
            CompanyID = Convert.ToString(User1.CompanyID)
            If hidden.Value = "" Then
                MeetingStatus = "Future"
            Else
                MeetingStatus = hidden.Value
            End If

            Dim dataItem As Telerik.Web.UI.GridDataItem = CType(e.DetailTableView.ParentItem, Telerik.Web.UI.GridDataItem)
            Dim ss As Boolean = e.DetailTableView.ParentItem.Expanded
            Dim Id As Integer = 0

            If Not IsDBNull(dataItem.GetDataKeyValue("MeetingID")) Then
                Id = dataItem.GetDataKeyValue("MeetingID")
            End If

            sSQL = Database & ".." & "[spGetCountEventRegistrationsOfSession]"
            Dim params(1) As IDataParameter
            params(0) = Me.DataAction.GetDataParameter("@ProdId", SqlDbType.Int, Id)
            params(1) = Me.DataAction.GetDataParameter("@CompanyID", SqlDbType.Int, User1.CompanyID)
            dtDeatil = Me.DataAction.GetDataTableParametrized(sSQL, CommandType.StoredProcedure, params)
            Dim dcol As DataColumn = New DataColumn()
            Dim dcolUrl As DataColumn = New DataColumn()
            dcolUrl.Caption = "MeetingUrl"
            dcolUrl.ColumnName = "MeetingUrl"
            dtDeatil.Columns.Add(dcolUrl)

            If dtDeatil.Rows.Count > 0 Then
                For Each rw As DataRow In dtDeatil.Rows
                    rw("MeetingUrl") = MeetingPage + "?back='back'&ID=" + rw("ProductID").ToString()
                Next
            End If

            Dim dcolConfirmedUrl As DataColumn = New DataColumn()
            dcolConfirmedUrl.Caption = "AdminConfirmedUrl"
            dcolConfirmedUrl.ColumnName = "AdminConfirmedUrl"
            dtDeatil.Columns.Add(dcolConfirmedUrl)

            If dtDeatil.Rows.Count > 0 Then
                For Each rw As DataRow In dtDeatil.Rows
                    rw("AdminConfirmedUrl") = EventRegistrationsDetailsURL + "?Confirmed=" + rw("ProductID").ToString + "&MonthYear=" + MonthYear
                Next
            End If

            Dim dcolWaitListUrl As DataColumn = New DataColumn()
            dcolWaitListUrl.Caption = "AdminWaitListUrl"
            dcolWaitListUrl.ColumnName = "AdminWaitListUrl"
            dtDeatil.Columns.Add(dcolWaitListUrl)

            If dtDeatil.Rows.Count > 0 Then
                For Each rw As DataRow In dtDeatil.Rows
                    rw("AdminWaitListUrl") = EventRegistrationsDetailsURL + "?WaitList=" + rw("ProductID").ToString + "&MonthYear=" + MonthYear
                Next
            End If

            Dim dcolAllUrl As DataColumn = New DataColumn()
            dcolAllUrl.Caption = "AdminAllUrl"
            dcolAllUrl.ColumnName = "AdminAllUrl"
            dtDeatil.Columns.Add(dcolAllUrl)

            If dtDeatil.Rows.Count > 0 Then
                For Each rw As DataRow In dtDeatil.Rows
                    rw("AdminAllUrl") = EventRegistrationsDetailsURL + "?sAll=" + rw("ProductID").ToString + "&MonthYear=" + MonthYear
                Next
            End If
            e.DetailTableView.DataSource = dtDeatil
        End Sub
        ''' <summary>
        ''' This event for show the past meeting session 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdResultsPast_DetailTableDataBind(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridDetailTableDataBindEventArgs) Handles grdResultsPast.DetailTableDataBind
            Dim dtDeatil As DataTable
            Dim sSQL As String = ""
            Dim MeetingStatus As String = ""
            Dim CompanyID As Integer
            Dim MonthYear As String
            Dim dt As New DataTable
            MonthYear = String.Format("{0:MM/dd/yyyy}", RadMonthYearPickerPast.SelectedDate.ToString())
            CompanyID = Convert.ToString(User1.CompanyID)

            If hidden.Value = "" Then
                MeetingStatus = "Future"
            Else
                MeetingStatus = hidden.Value
            End If

            Dim dataItem As Telerik.Web.UI.GridDataItem = CType(e.DetailTableView.ParentItem, Telerik.Web.UI.GridDataItem)
            Dim ss As Boolean = e.DetailTableView.ParentItem.Expanded
            Dim Id As Integer = 0

            If Not IsDBNull(dataItem.GetDataKeyValue("MeetingID")) Then
                Id = dataItem.GetDataKeyValue("MeetingID")
            End If

            sSQL = Database & ".." & "[spGetCountEventRegistrationsOfSession]"
            Dim params(1) As IDataParameter
            params(0) = Me.DataAction.GetDataParameter("@ProdId", SqlDbType.Int, Id)
            params(1) = Me.DataAction.GetDataParameter("@CompanyID", SqlDbType.Int, User1.CompanyID)
            dtDeatil = Me.DataAction.GetDataTableParametrized(sSQL, CommandType.StoredProcedure, params)
            Dim dcol As DataColumn = New DataColumn()
            Dim dcolUrl As DataColumn = New DataColumn()
            dcolUrl.Caption = "MeetingUrl"
            dcolUrl.ColumnName = "MeetingUrl"
            dtDeatil.Columns.Add(dcolUrl)

            If dtDeatil.Rows.Count > 0 Then
                For Each rw As DataRow In dtDeatil.Rows
                    rw("MeetingUrl") = MeetingPage + "?back='back'&ID=" + rw("ProductID").ToString()
                Next
            End If

            Dim dcolConfirmedUrl As DataColumn = New DataColumn()
            dcolConfirmedUrl.Caption = "AdminConfirmedUrl"
            dcolConfirmedUrl.ColumnName = "AdminConfirmedUrl"
            dtDeatil.Columns.Add(dcolConfirmedUrl)

            If dtDeatil.Rows.Count > 0 Then
                For Each rw As DataRow In dtDeatil.Rows
                    rw("AdminConfirmedUrl") = EventRegistrationsDetailsURL + "?Confirmed=" + rw("ProductID").ToString + "&MonthYear=" + MonthYear + "&PAST=" + MonthYear
                Next
            End If

            Dim dcolWaitListUrl As DataColumn = New DataColumn()
            dcolWaitListUrl.Caption = "AdminWaitListUrl"
            dcolWaitListUrl.ColumnName = "AdminWaitListUrl"
            dtDeatil.Columns.Add(dcolWaitListUrl)

            If dtDeatil.Rows.Count > 0 Then
                For Each rw As DataRow In dtDeatil.Rows
                    rw("AdminWaitListUrl") = EventRegistrationsDetailsURL + "?WaitList=" + rw("ProductID").ToString + "&MonthYear=" + MonthYear + "&PAST=" + MonthYear
                Next
            End If

            Dim dcolAllUrl As DataColumn = New DataColumn()
            dcolAllUrl.Caption = "AdminAllUrl"
            dcolAllUrl.ColumnName = "AdminAllUrl"
            dtDeatil.Columns.Add(dcolAllUrl)

            If dtDeatil.Rows.Count > 0 Then
                For Each rw As DataRow In dtDeatil.Rows
                    rw("AdminAllUrl") = EventRegistrationsDetailsURL + "?sAll=" + rw("ProductID").ToString + "&MonthYear=" + MonthYear + "&PAST=" + MonthYear
                Next
            End If
            e.DetailTableView.DataSource = dtDeatil

        End Sub
        Private Sub AddExpression()
            Dim ExpOrderSort As New GridSortExpression
            'Suraj Issue 15404 -9-19-13 ,upcomming event sort by start date asc and past event sort by desc so here we set when the event load first time then order tool tip set by as per our requirement 
            ExpOrderSort.FieldName = "MonthDate"
            If Not IsPostBack OrElse hidden.Value = "Future" OrElse hidden.Value = " " OrElse hidden.Value = "" OrElse hidden.Value = Nothing Then
                ExpOrderSort.SetSortOrder("Ascending")
            Else
                ExpOrderSort.SetSortOrder("Descending")
            End If
            grdResults.MasterTableView.SortExpressions.AddSortExpression(ExpOrderSort)
            grdResultsPast.MasterTableView.SortExpressions.AddSortExpression(ExpOrderSort)
            TryCast(grdResults.MasterTableView.GetColumn("TemplateColumn"), GridTemplateColumn).SortExpression = String.Empty
            TryCast(grdResultsPast.MasterTableView.GetColumn("TemplateColumn"), GridTemplateColumn).SortExpression = String.Empty
        End Sub
    End Class
End Namespace

