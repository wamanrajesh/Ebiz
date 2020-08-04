'Aptify e-Business 5.5.1, July 2013
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Framework
Imports Aptify.Framework.DataServices
Imports System.Data
Imports Telerik.Web.UI
Imports Telerik.Charting

Namespace Aptify.Framework.Web.eBusiness
    Partial Class UpcomingEventsRegistrationChart
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_REGISTERED_URL As String = "RegisteredUrl"
        Protected Const ATTRIBUTE_WAITLIST_URL As String = "WaitListUrl"
        Protected Const ATTRIBUTE_NOT_REGISTERED_URL As String = "NotRegisteredUrl"
        Protected Const ATTRIBUTE_LOGIN_PAGE As String = "LoginPage" 'Added by Sandeep for Issue 15051 on 12/03/2013
        Protected Const ATTRIBUTE_SHOWMEETINGSLINKTOCLASS_DEFAULT_NAME As String = "ShowMeetingsLinkToClass"
#Region "Properties"

        ''' <summary>
        ''' Registered Url
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Property RegisteredUrl() As String
            Get
                If Not ViewState(ATTRIBUTE_REGISTERED_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_REGISTERED_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_REGISTERED_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        ''' <summary>
        ''' WaitList Url
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Property WaitListUrl() As String
            Get
                If Not ViewState(ATTRIBUTE_WAITLIST_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_WAITLIST_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_WAITLIST_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        ''' <summary>
        ''' Not Registered Url
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Property NotRegisteredUrl() As String
            Get
                If Not ViewState(ATTRIBUTE_NOT_REGISTERED_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_NOT_REGISTERED_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_NOT_REGISTERED_URL) = Me.FixLinkForVirtualPath(value)
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

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            SetProperties()
            If Not Page.IsPostBack Then
                PopulateDateDropdown()
                PopulateEventsDropdown()
                BindChart()
            End If
            If user1.UserID < 0 Then
                Response.Redirect(LoginPage) 'Added by Sandeep for Issue 15051 on 12/03/2013
            End If
        End Sub

        Protected Overrides Sub SetProperties()
            If String.IsNullOrEmpty(RegisteredUrl) Then
                RegisteredUrl = Me.GetLinkValueFromXML(ATTRIBUTE_REGISTERED_URL)
            End If
            If String.IsNullOrEmpty(WaitListUrl) Then
                WaitListUrl = Me.GetLinkValueFromXML(ATTRIBUTE_WAITLIST_URL)
            End If
            If String.IsNullOrEmpty(NotRegisteredUrl) Then
                NotRegisteredUrl = Me.GetLinkValueFromXML(ATTRIBUTE_NOT_REGISTERED_URL)
            End If
            'Added by Sandeep for Issue 15051 on 12/03/2013
            If String.IsNullOrEmpty(LoginPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                LoginPage = Me.GetLinkValueFromXML(ATTRIBUTE_LOGIN_PAGE)
            End If
            Dim bShowMeeting As Boolean = ShowMeetingsLinkToClass()
        End Sub

        Private Sub PopulateDateDropdown()
            Try

                Dim dt As Date = Date.Today

                For i As Integer = 1 To 12
                    'cmbDate.Items.Add(New ListItem(MonthName(dt.Month) + " " + dt.Year.ToString))
                    cmbDate.Items.Add(New ListItem(dt.ToString("MMMM, yyyy"), dt.ToString))

                    dt = Date.Today.AddMonths(i)
                Next

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub PopulateEventsDropdown()
            Try
                Dim bShowMeeting As Boolean
                Dim sSQL As String
                bShowMeeting = ShowMeetingsLinkToClass()
                cmbEvent.Items.Clear()
                cmbEvent.Items.Add(New ListItem("------Select Events------", "0"))

                sSQL = Me.AptifyApplication.GetEntityBaseDatabase("Meetings") & _
                       "..spGetUpcomingEventsByMonth @EventDate='" & cmbDate.SelectedValue & "',@CompanyID=" & user1.CompanyID
                Dim dtUpcomingEvents As DataTable = Me.DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)

                If dtUpcomingEvents IsNot Nothing Then
                    If bShowMeeting = False Then
                        Dim dt2 As DataTable = dtUpcomingEvents.Clone
                        Dim dr2 As DataRow() = dtUpcomingEvents.Select("ClassID is null")
                        For Each row As DataRow In dr2
                            dt2.ImportRow(row)
                        Next
                        cmbEvent.DataSource = dt2
                    Else
                        cmbEvent.DataSource = dtUpcomingEvents
                    End If

                    cmbEvent.DataTextField = "MeetingTitle"
                    cmbEvent.DataValueField = "MeetingID"
                    cmbEvent.DataBind()
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub cmbDate_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbDate.SelectedIndexChanged
            PopulateEventsDropdown()
            BindChart()
        End Sub

        Protected Sub cmbEvent_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbEvent.SelectedIndexChanged
            BindChart()
        End Sub

        Private Sub BindChart()
            Dim sSQL As String
            Dim bShowMeeting As Boolean = ShowMeetingsLinkToClass()

            radChart.PlotArea.XAxis.DataLabelsColumn = "MeetingTitle"
            radChart.PlotArea.XAxis.Appearance.TextAppearance.AutoTextWrap = Telerik.Charting.Styles.AutoTextWrap.True

            sSQL = Me.AptifyApplication.GetEntityBaseDatabase("Meetings") & _
                   "..spGetUpcomingEventsRegistrationSummary @EventDate='" & cmbDate.SelectedValue & "',@CompanyID=" & user1.CompanyID & ",@MeetingID=" & cmbEvent.SelectedValue
            Dim dtUpcomingEventsRegistrationSummary As DataTable = Me.DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)

            ''Rashmi P, 4/28/2013, issue 15456: e-Business: GroupAdmin Dashboard: Show Meetings Associated With Class Shows Up In The Events Section
            Dim dtEventSummaryClone As DataTable = dtUpcomingEventsRegistrationSummary.Clone

            If dtUpcomingEventsRegistrationSummary IsNot Nothing Then
                If bShowMeeting = False Then
                    Dim dr2 As DataRow() = dtUpcomingEventsRegistrationSummary.Select("ClassID is null")
                    For Each row As DataRow In dr2
                        dtEventSummaryClone.ImportRow(row)
                    Next
                    dtUpcomingEventsRegistrationSummary.Rows.Clear()
                    dtUpcomingEventsRegistrationSummary = dtEventSummaryClone.Copy
                End If
                radChart.DataSource = dtUpcomingEventsRegistrationSummary
                radChart.DataBind()
            End If

            radChart.PlotArea.XAxis.DataLabelsColumn = "MeetingTitle"
            radChart.PlotArea.XAxis.Appearance.TextAppearance.AutoTextWrap = Telerik.Charting.Styles.AutoTextWrap.True
            radChart.PlotArea.XAxis.AutoScale = False

            If dtUpcomingEventsRegistrationSummary IsNot Nothing AndAlso dtUpcomingEventsRegistrationSummary.Rows.Count > 0 Then
                Select Case dtUpcomingEventsRegistrationSummary.Rows.Count
                    Case 1
                        radChart.Appearance.BarWidthPercent = 10
                    Case 2
                        radChart.Appearance.BarWidthPercent = 20
                    Case 3
                        radChart.Appearance.BarWidthPercent = 30
                    Case 4
                        radChart.Appearance.BarWidthPercent = 40
                    Case Else
                        radChart.Appearance.BarWidthPercent = 75
                End Select
            End If

            If radChart.Series.Count = 5 Then
                radChart.Series(3).Appearance.LegendDisplayMode = ChartSeriesLegendDisplayMode.Nothing
                radChart.Series(3).Clear()
                radChart.Series(4).Appearance.LegendDisplayMode = ChartSeriesLegendDisplayMode.Nothing
                radChart.Series(4).Clear()
            End If
            If radChart.Legend.Items.Count = 5 Then
                radChart.Legend.Items(3).Remove()
                radChart.Legend.Items(4).Remove()
            End If



            For i As Integer = 0 To radChart.Series.Count - 1

                For j As Integer = 0 To radChart.Series(i).Items.Count - 1

                    Select Case radChart.Series(i).Items(j).Parent.Name
                        Case "Registered"
                            'radChart.Series(i).Items(j).ActiveRegion.Url = "../Ebusiness/MeetingsEventsManagement/eventlisting?Confirmed=" & dtUpcomingEventsRegistrationSummary.Rows(j)("ProductID").ToString() & "&MonthYear=" & (CType(cmbDate.SelectedItem.Value(), Date)).ToString("MM/dd/yyyy")
                            radChart.Series(i).Items(j).ActiveRegion.Url = RegisteredUrl & "?Confirmed=" & dtUpcomingEventsRegistrationSummary.Rows(j)("ProductID").ToString() & "&MonthYear=" & (CType(cmbDate.SelectedItem.Value(), Date)).ToString("MM/dd/yyyy")
                            radChart.Series(i).Items(j).ActiveRegion.Tooltip = "Registered: " & dtUpcomingEventsRegistrationSummary.Rows(j)("Registered").ToString()
                        Case "WaitList"
                            'radChart.Series(i).Items(j).ActiveRegion.Url = "../Ebusiness/MeetingsEventsManagement/eventlisting?WaitList=" & dtUpcomingEventsRegistrationSummary.Rows(j)("ProductID").ToString() & "&MonthYear= " & (CType(cmbDate.SelectedItem.Value(), Date)).ToString("MM/dd/yyyy")
                            radChart.Series(i).Items(j).ActiveRegion.Url = WaitListUrl & "?WaitList=" & dtUpcomingEventsRegistrationSummary.Rows(j)("ProductID").ToString() & "&MonthYear= " & (CType(cmbDate.SelectedItem.Value(), Date)).ToString("MM/dd/yyyy")
                            radChart.Series(i).Items(j).ActiveRegion.Tooltip = "WaitList: " & dtUpcomingEventsRegistrationSummary.Rows(j)("WaitList").ToString()
                        Case "Not Registered"
                            'radChart.Series(i).Items(j).ActiveRegion.Url = "../Ebusiness/MeetingsEventsManagement/eventlisting?sAll=" & dtUpcomingEventsRegistrationSummary.Rows(j)("ProductID").ToString() & "&MonthYear=" & (CType(cmbDate.SelectedItem.Value(), Date)).ToString("MM/dd/yyyy")
                            radChart.Series(i).Items(j).ActiveRegion.Url = NotRegisteredUrl & "?NotRegistered=" & dtUpcomingEventsRegistrationSummary.Rows(j)("ProductID").ToString() & "&MonthYear=" & (CType(cmbDate.SelectedItem.Value(), Date)).ToString("MM/dd/yyyy")
                            radChart.Series(i).Items(j).ActiveRegion.Tooltip = "Not Registered: " & dtUpcomingEventsRegistrationSummary.Rows(j)("Not Registered").ToString()
                    End Select

                    radChart.Series(i).Items(j).Label.Appearance.Visible = False

                Next

            Next

            radChart.BackColor = System.Drawing.ColorTranslator.FromHtml("#F9F9F9")

            radChart.Legend.Appearance.ItemTextAppearance.TextProperties.Font = New System.Drawing.Font("", 9.0F, System.Drawing.FontStyle.Regular)

        End Sub



    End Class
End Namespace

