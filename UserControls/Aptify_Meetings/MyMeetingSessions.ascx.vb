'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Applications.ProductSetup
Imports System.Data
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.Application
Imports Aptify.Framework.BusinessLogic.ProcessPipeline
Imports System.Net.Mail
Imports System.IO

Namespace Aptify.Framework.Web.eBusiness.Meetings
    Partial Class MyMeetingSessions
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced

        Protected Const ATTRIBUTE_MEETING_PAGE As String = "MeetingPage"
        Protected Const ATTRIBUTE_PRINT_MEETING_SESSIONS_PAGE As String = "PrintMeetingSessionsPage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "MyMeetingSessions"
        Protected Const ATTRIBUTE_SHOWMEETINGSLINKTOCLASS_DEFAULT_NAME As String = "ShowMeetingsLinkToClass"
        Protected Const ATTRIBUTE_Mail_Attachment_Path As String = "MailAttachmentPath"
        'Amruta IssueId 14380,To get selected meeting strat date,Mainmeeting title 
        Protected Const ATTRIBUTE_MAINMEETING_TITLE As String = "MainMeetingTitle"

#Region "MeetingsCalendar Specific Properties"
        ''' <summary>
        ''' Meeting page url
        ''' </summary>
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
        ''' Print Meeting Sessions Page url
        ''' </summary>
        Public Overridable Property PrintMeetingSessionsPage() As String
            Get
                If Not ViewState(ATTRIBUTE_PRINT_MEETING_SESSIONS_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_PRINT_MEETING_SESSIONS_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_PRINT_MEETING_SESSIONS_PAGE) = Me.FixLinkForVirtualPath(value)
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
                    Return CBool(ViewState.Item(ATTRIBUTE_SHOWMEETINGSLINKTOCLASS_DEFAULT_NAME))
                End If
            End Get
        End Property

        ''' <summary>
        ''' Mail Attachment Path
        ''' </summary>
        Public Overridable Property MailAttachmentPath() As String
            Get
                If Not ViewState(ATTRIBUTE_Mail_Attachment_Path) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_Mail_Attachment_Path))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_Mail_Attachment_Path) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

#End Region

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            MailAttachmentPath = Me.GetLinkValueFromXML(ATTRIBUTE_Mail_Attachment_Path)

            If String.IsNullOrEmpty(MeetingPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                MeetingPage = Me.GetLinkValueFromXML(ATTRIBUTE_MEETING_PAGE)
                PrintMeetingSessionsPage = Me.GetLinkValueFromXML(ATTRIBUTE_PRINT_MEETING_SESSIONS_PAGE)
            End If
            Dim bShowMeetingClasslink As Boolean = ShowMeetingsLinkToClass()

        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'set control properties from XML file if needed
            SetProperties()
            lblMessage.Text = ""

            If Not IsPostBack Then
                'IssueId 14380,to get Meeting ProductId
                If Request.QueryString("ID") IsNot Nothing Then
                    Session.Item("ID") = Request.QueryString("ID")
                End If

                LoadCalender()

                Dim url As String = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.IndexOf(Request.ApplicationPath)) & PrintMeetingSessionsPage
                url = url & "?Mode=Print&SelectedView=" & EventCalenderScheduler.SelectedView.ToString & "&SelectedDate=" & EventCalenderScheduler.SelectedDate.ToString
                'Amruta IssueID 14380,To open print dialog 
                btnPrint.OnClientClick = " javascript: window.print(); return false;"
                lblMessage.Text = String.Empty
                If Request.QueryString("Mode") IsNot Nothing AndAlso Request.QueryString("Mode").ToUpper = "PRINT" Then

                    EventCalenderScheduler.SelectedView = CType(System.Enum.Parse(GetType(Telerik.Web.UI.SchedulerViewType), Request.QueryString("SelectedView")), Telerik.Web.UI.SchedulerViewType)
                    EventCalenderScheduler.SelectedDate = CType(Request.QueryString("SelectedDate"), DateTime)

                    ltlPrint.Text = "<script language='javascript' type='text/javascript'>window.print()</script>"
                End If
            End If
        End Sub

        Private Sub LoadCalender()
            Dim sSQL As String
            Dim oDT As DataTable
            Dim iconfigAttribute As Integer


            'sSQL = "SELECT P.ProductCategoryID, M.ProductID, " & _
            '       "	'MeetingTitle' = (CASE WHEN PM.ID IS NOT NULL THEN ISNULL(PM.MeetingTitle, '') + ' : ' ELSE '' END) + M.MeetingTitle, " & _
            '       "	M.Status, M.StartDate, " & _
            '       "	'EndDate'= " & _
            '       "		CASE " & _
            '       "			WHEN LTRIM(RIGHT(CONVERT(VARCHAR(20), M.StartDate, 100), 7))='12:00AM' AND LTRIM(RIGHT(CONVERT(VARCHAR(20), M.EndDate, 100), 7))='12:00AM' THEN " & _
            '       "				CAST(M.EndDate AS VARCHAR(11)) +' 12:00:01AM' " & _
            '       "			ELSE M.EndDate " & _
            '       "		END, " & _
            '       "	'MeetingDescription' = 'Meeting: ' + (CASE WHEN PM.ID IS NOT NULL THEN ISNULL(PM.MeetingTitle, '') ELSE M.MeetingTitle END) + '<br/>' " & _
            '       "		+ (CASE WHEN PM.ID IS NOT NULL THEN ISNULL(CAST(PM.StartDate AS VARCHAR(11)), '') + '-' + ISNULL(CAST(PM.EndDate AS VARCHAR(11)), '') ELSE ISNULL(CAST(M.StartDate AS VARCHAR(11)), '') + '-' + ISNULL(CAST(M.EndDate AS VARCHAR(11)), '') END) " & _
            '       "		+ (CASE WHEN PM.ID IS NOT NULL THEN '<br/>Session: ' + ISNULL(M.MeetingTitle,'') + '<br/>'  ELSE '' END) " & _
            '       "		+ (CASE WHEN PM.ID IS NOT NULL THEN ISNULL(CAST(M.StartDate AS VARCHAR(11)), '') + '-' + ISNULL(CAST(M.EndDate AS VARCHAR(11)), '') ELSE '' END) " & _
            '       "FROM " & Me.Database & "..vwMeetings M " & _
            '       "	INNER JOIN " & Me.Database & "..vwProducts P ON P.ID = M.ProductID " & _
            '       "	INNER JOIN " & Me.Database & "..vwOrderMeetDetail O ON O.ProductID= M.ProductID " & _
            '       "	INNER JOIN " & Me.Database & "..vwAttendeeStatus S ON O.StatusID=S.ID " & _
            '       "	LEFT OUTER JOIN " & Me.Database & "..vwMeetings PM ON M.ParentMeetingID=PM.ID " & _
            '       "WHERE P.WebEnabled = 1 AND O.AttendeeID = " & User1.PersonID & " AND S.ID IN (1,2,3,4) "

            sSQL = "SELECT P.ProductCategoryID, M.ProductID, " & _
                   "	M.MeetingTitle, " & _
                   "	ISNULL(PM.ID, 0) ParentMeetingID, ISNULL(PM.MeetingTitle, '') ParentMeetingTitle, " & _
                   "	PM.StartDate ParentMeetingStartDate, PM.EndDate ParentMeetingEndDate, " & _
                   "	M.Status, M.StartDate, " & _
                   "	'EndDate'= " & _
                   "		CASE " & _
                   "			WHEN LTRIM(RIGHT(CONVERT(VARCHAR(20), M.StartDate, 100), 7))='12:00AM' AND LTRIM(RIGHT(CONVERT(VARCHAR(20), M.EndDate, 100), 7))='12:00AM' THEN " & _
                   "				CAST(M.EndDate AS VARCHAR(11)) +' 12:00:01AM' " & _
                   "			ELSE M.EndDate " & _
                   "		END " & _
                   "FROM " & Me.Database & "..vwMeetings M " & _
                   "	INNER JOIN " & Me.Database & "..vwProducts P ON P.ID = M.ProductID " & _
                   "	INNER JOIN " & Me.Database & "..vwOrderMeetDetail O ON O.ProductID= M.ProductID " & _
                   "	INNER JOIN " & Me.Database & "..vwAttendeeStatus S ON O.StatusID=S.ID " & _
                   "	LEFT OUTER JOIN " & Me.Database & "..vwMeetings PM ON M.ParentMeetingID=PM.ID " & _
                   "WHERE P.WebEnabled = 1 AND O.AttendeeID = " & User1.PersonID & " AND S.ID IN (1,2,3,4) " & " AND P.ID = " & CType(Session.Item("ID"), Integer) & " AND O.AttendeeStatus_Name <> " & " 'Cancelled' " & " UNION " & _
                  "SELECT P.ProductCategoryID, M.ProductID, " & _
                  "	M.MeetingTitle, " & _
                  "	ISNULL(PM.ID, 0) ParentMeetingID, ISNULL(PM.MeetingTitle, '') ParentMeetingTitle, " & _
                  "	PM.StartDate ParentMeetingStartDate, PM.EndDate ParentMeetingEndDate, " & _
                  "	M.Status, M.StartDate, " & _
                  "	'EndDate'= " & _
                  "		CASE " & _
                  "			WHEN LTRIM(RIGHT(CONVERT(VARCHAR(20), M.StartDate, 100), 7))='12:00AM' AND LTRIM(RIGHT(CONVERT(VARCHAR(20), M.EndDate, 100), 7))='12:00AM' THEN " & _
                  "				CAST(M.EndDate AS VARCHAR(11)) +' 12:00:01AM' " & _
                  "			ELSE M.EndDate " & _
                  "		END " & _
                  "FROM " & Me.Database & "..vwMeetings M " & _
                  "	INNER JOIN " & Me.Database & "..vwProducts P ON P.ID = M.ProductID " & _
                  "	INNER JOIN " & Me.Database & "..vwOrderMeetDetail O ON O.ProductID= M.ProductID " & _
                  "	INNER JOIN " & Me.Database & "..vwAttendeeStatus S ON O.StatusID=S.ID " & _
                  "	LEFT OUTER JOIN " & Me.Database & "..vwMeetings PM ON M.ParentMeetingID=PM.ID " & _
                  "WHERE P.WebEnabled = 1 AND O.AttendeeID = " & User1.PersonID & " AND S.ID IN (1,2,3,4) " & " AND P.ParentID = " & CType(Session.Item("ID"), Integer) & " AND O.AttendeeStatus_Name <> " & " 'Cancelled' "

            If Not ShowMeetingsLinkToClass Then
                sSQL &= (" AND  ISNULL(P.ClassID ,-1) <= 0  ")
            End If

            sSQL &= "ORDER BY M.StartDate"

            oDT = Me.DataAction.GetDataTable(sSQL)
            EventCalenderScheduler.DataSource = oDT
            EventCalenderScheduler.DataKeyField = "ProductID"
            EventCalenderScheduler.DataSubjectField = "MeetingTitle"
            'EventCalenderScheduler.DataSubjectField = "MeetingDescription"
            EventCalenderScheduler.DataDescriptionField = ""
            EventCalenderScheduler.DataStartField = "StartDate"
            EventCalenderScheduler.DataEndField = "EndDate"
            EventCalenderScheduler.DataBind()
        End Sub

        '''RAshmi P, Code commented not required 4/3/13
        ' ''Private Function CheckConfigAttributeforClass() As Integer
        ' ''    Try
        ' ''        Dim sSQL As String
        ' ''        Dim iAttributeValue As Integer
        ' ''        Dim dt As DataTable
        ' ''        sSQL = Database & "..spIdentifyMeetingWithClass "
        ' ''        dt = DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)
        ' ''        iAttributeValue = CInt(dt.Rows(0).Item("AttributeValue"))
        ' ''        Return iAttributeValue
        ' ''    Catch ex As Exception
        ' ''        Return 0
        ' ''        Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        ' ''    End Try
        ' ''End Function

        Protected Function GetDateTimeString(ByVal dateTime As DateTime) As String
            Dim strDateTime As String = ""
            If dateTime.Hour <> 0 OrElse dateTime.Minute <> 0 Then
                strDateTime = dateTime.ToString()
            Else
                strDateTime = dateTime.ToString("MM/dd/yyyy")
            End If

            Return strDateTime
        End Function

        Protected Sub EventCalenderScheduler_AppointmentClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.SchedulerEventArgs) Handles EventCalenderScheduler.AppointmentClick
            Dim strurl As String = MeetingPage & "?ID=" & e.Appointment.ID.ToString()
            Response.Redirect(strurl)
        End Sub

        Protected Sub EventCalenderScheduler_AppointmentDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.SchedulerEventArgs) Handles EventCalenderScheduler.AppointmentDataBound
            Dim rowView As DataRowView = CType(e.Appointment.DataItem, DataRowView)

            If rowView IsNot Nothing Then

                Dim strSubject As String = ""
                Dim ParentMeetingID As Integer = Integer.Parse(rowView("ParentMeetingID").ToString())

                If ParentMeetingID <> 0 Then
                    'strSubject = strSubject & "Meeting: " & rowView("ParentMeetingTitle").ToString() & "<BR/>"
                    strSubject = strSubject & "Session: " & rowView("MeetingTitle").ToString() & "<BR/>"
                Else
                    strSubject = strSubject & "Meeting: " & rowView("MeetingTitle").ToString() & "<BR/>"
                    ViewState(ATTRIBUTE_MAINMEETING_TITLE) = rowView("MeetingTitle").ToString()
                End If

                Dim startDate As DateTime = DateTime.Parse(rowView("StartDate").ToString())
                'startDate = DateTime.Parse(rowView("StartDate").ToString())
                Dim endDate As DateTime = DateTime.Parse(rowView("EndDate").ToString())

                If ParentMeetingID = 0 Then
                    EventCalenderScheduler.SelectedDate = startDate
                    EventCalenderScheduler.TimelineView.NumberOfSlots = 7
                End If
                If startDate.Year <> endDate.Year OrElse startDate.ToShortDateString = endDate.ToShortDateString Then
                    If startDate.ToShortTimeString = "12:00 AM" Then
                        strSubject = strSubject & startDate.ToString("MMM dd yyyy")
                    Else
                        strSubject = strSubject & startDate.ToString("MMM dd yyyy h:mm tt")
                    End If
                Else
                    If startDate.ToShortTimeString = "12:00 AM" Then
                        strSubject = strSubject & startDate.ToString("MMM dd")
                    Else
                        strSubject = strSubject & startDate.ToString("MMM dd h:mm tt")
                    End If
                End If

                If startDate.ToShortDateString <> endDate.ToShortDateString Then
                    If endDate.ToShortTimeString = "12:00 AM" Then
                        strSubject = strSubject & " - " & endDate.ToString("MMM dd yyyy")
                    Else
                        strSubject = strSubject & " - " & endDate.ToString("MMM dd yyyy h:mm tt")
                    End If
                Else
                    strSubject = strSubject & " - " & endDate.ToString("MMM dd yyyy h:mm tt")
                End If

                e.Appointment.Subject = strSubject
                e.Appointment.ToolTip = strSubject.Replace("<BR/>", vbNewLine)
            End If
        End Sub

        Protected Sub EventCalenderScheduler_NavigationCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.SchedulerNavigationCommandEventArgs)
            If e.Command = Telerik.Web.UI.SchedulerNavigationCommand.NavigateToSelectedDate Then
                'EventCalenderScheduler.SelectedView = Telerik.Web.UI.SchedulerViewType.DayView
                EventCalenderScheduler.SelectedDate = e.SelectedDate
            End If
            Select Case e.Command
                Case Telerik.Web.UI.SchedulerNavigationCommand.NavigateToSelectedDate
                    EventCalenderScheduler.SelectedDate = e.SelectedDate
                Case Telerik.Web.UI.SchedulerNavigationCommand.SwitchToDayView
                    EventCalenderScheduler.SelectedView = Telerik.Web.UI.SchedulerViewType.DayView
                Case Telerik.Web.UI.SchedulerNavigationCommand.SwitchToWeekView
                    EventCalenderScheduler.SelectedView = Telerik.Web.UI.SchedulerViewType.WeekView
                Case Telerik.Web.UI.SchedulerNavigationCommand.SwitchToMonthView
                    EventCalenderScheduler.SelectedView = Telerik.Web.UI.SchedulerViewType.MonthView
                Case Telerik.Web.UI.SchedulerNavigationCommand.SwitchToTimelineView
                    EventCalenderScheduler.SelectedView = Telerik.Web.UI.SchedulerViewType.TimelineView
            End Select
            LoadCalender()

            Dim url As String = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.IndexOf(Request.ApplicationPath)) & PrintMeetingSessionsPage
            url = url & "?Mode=Print&SelectedView=" & EventCalenderScheduler.SelectedView.ToString & "&SelectedDate=" & EventCalenderScheduler.SelectedDate.ToString
            'Amruta IssueID 14380,To open print dialog 
            btnPrint.OnClientClick = " javascript: window.print(); return false;"
            lblMessage.Text = String.Empty
        End Sub

        Protected Sub btnEmail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEmail.Click
            Dim lstAppointments As IList(Of Telerik.Web.UI.Appointment) = EventCalenderScheduler.Appointments.GetAppointmentsInRange(EventCalenderScheduler.VisibleRangeStart, EventCalenderScheduler.VisibleRangeEnd)
            Dim strMeetingSessionMessage As String = GetMeetingSessionMessage(lstAppointments)
            SendMeetingSessionMail(strMeetingSessionMessage, lstAppointments)
        End Sub

        Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
            Dim lstAppointments As IList(Of Telerik.Web.UI.Appointment) = EventCalenderScheduler.Appointments.GetAppointmentsInRange(EventCalenderScheduler.VisibleRangeStart, EventCalenderScheduler.VisibleRangeEnd)
            FixSchedulerEndDate(lstAppointments)
            Dim strICalendar As String = Telerik.Web.UI.RadScheduler.ExportToICalendar(lstAppointments)
            strICalendar = strICalendar.Replace("<BR/>", " ")
            Dim enc As New UTF8Encoding
            Dim arrBytData() As Byte = enc.GetBytes(strICalendar)
            Dim response As HttpResponse = Page.Response
            response.Clear()
            response.Buffer = True
            response.ContentType = "text/calendar"
            response.ContentEncoding = Encoding.UTF8
            response.Charset = "utf-8"
            response.AddHeader("Content-Disposition", _
                      "attachment;filename=""MyMeetingSessions.ics""")
            response.Write(strICalendar)
            response.[End]()
        End Sub

        Public Function GetMeetingSessionMessage(ByVal lstAppointments As IList(Of Telerik.Web.UI.Appointment)) As String
            Dim tblMeetingSession As New Table
            tblMeetingSession.CellSpacing = 0
            tblMeetingSession.BorderWidth = New Unit(1)
            tblMeetingSession.Width = New Unit("100%")

            Dim tcName As New TableCell


            For Each appointment In lstAppointments
                Dim trRow As New TableRow

                tcName = New TableCell
                tcName.Text = appointment.Subject
                tcName.BorderWidth = New Unit(1)
                trRow.Cells.Add(tcName)

                tblMeetingSession.Rows.Add(trRow)
            Next

            Dim sb As New StringBuilder()
            Dim stringWriter As New System.IO.StringWriter(sb)
            Dim htmlTextWriter As New HtmlTextWriter(stringWriter)
            tblMeetingSession.RenderControl(htmlTextWriter)

            Return sb.ToString
        End Function


        Public Sub SendMeetingSessionMail(ByVal strMeetingSessionMessage As String, ByVal lstAppointments As IList(Of Telerik.Web.UI.Appointment))
            Try
                'Get the Process Flow ID to be used for sending the Order Confirmation Email
                Dim sSQL As String = "SELECT ID FROM " & Database & "..vwProcessFlows WHERE Name='My Meeting Sessions'"
                Dim lProcessFlowID As Long = CLng(DataAction.ExecuteScalar(sSQL, IAptifyDataAction.DSLCacheSetting.UseCache))
                Dim context As New AptifyContext
                'context.Properties.AddProperty("FromEmail", System.Configuration.ConfigurationManager.AppSettings("MailFrom"))
                context.Properties.AddProperty("ToEmail", User1.Email)
                context.Properties.AddProperty("FirstName", User1.FirstName)
                context.Properties.AddProperty("MeetingSessionSchedule", strMeetingSessionMessage)
                context.Properties.AddProperty("MainMeetingTitle", ViewState(ATTRIBUTE_MAINMEETING_TITLE))
                FixSchedulerEndDate(lstAppointments)

                Dim strICalendar As String = Telerik.Web.UI.RadScheduler.ExportToICalendar(lstAppointments)
                strICalendar = strICalendar.Replace("<BR/>", " ")

                Dim enc As New UTF8Encoding
                Dim arrBytData() As Byte = enc.GetBytes(strICalendar)
                File.WriteAllBytes(Server.MapPath(MailAttachmentPath) & "MyMeetingSessions.ics", arrBytData)

                'save the data to a memory stream
                Dim ms As New MemoryStream(arrBytData)
                Dim attach As New Attachment(ms, "MyMeetingSessions.ics", "text/calendar")
                context.Properties.AddProperty("ICalendarAttachment", attach)

                context.Properties.AddProperty("ICalendarAttachment", Server.MapPath(MailAttachmentPath) & "MyMeetingSessions.ics")

                Dim result As ProcessFlowResult
                result = ProcessFlowEngine.ExecuteProcessFlow(Me.AptifyApplication, lProcessFlowID, context)
                If result.IsSuccess Then
                    lblMessage.ForeColor = Drawing.Color.Blue
                    lblMessage.Text = "An email has been sent to you with your meeting session details."
                Else
                    lblMessage.ForeColor = Drawing.Color.Red
                    lblMessage.Text = "Email Notification failed. Contact Customer Service for help."
                End If


            Catch ex As ArgumentException
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try

        End Sub

        Public Sub FixSchedulerEndDate(ByVal lstAppointments As IList(Of Telerik.Web.UI.Appointment))
            If lstAppointments IsNot Nothing Then
                For i As Integer = 0 To (lstAppointments.Count - 1)
                    Dim objAppointment As Telerik.Web.UI.Appointment = lstAppointments(i).Clone

                    If objAppointment.Start.Year = objAppointment.End.Year AndAlso objAppointment.Start.Month = objAppointment.End.Month AndAlso objAppointment.Start.Day = objAppointment.End.Day _
                            AndAlso objAppointment.Start.Hour = objAppointment.End.Hour AndAlso objAppointment.Start.Minute = objAppointment.End.Minute Then
                        objAppointment.End = objAppointment.Start
                    End If

                    lstAppointments(i) = objAppointment
                Next

                For Each objAppointment As Telerik.Web.UI.Appointment In lstAppointments
                    If objAppointment.Start.Year = objAppointment.End.Year AndAlso objAppointment.Start.Month = objAppointment.End.Month AndAlso objAppointment.Start.Day = objAppointment.End.Day _
                            AndAlso objAppointment.Start.Hour = objAppointment.End.Hour AndAlso objAppointment.Start.Minute = objAppointment.End.Minute Then
                        objAppointment.End = objAppointment.Start
                    End If
                Next
            End If
        End Sub
    End Class
End Namespace