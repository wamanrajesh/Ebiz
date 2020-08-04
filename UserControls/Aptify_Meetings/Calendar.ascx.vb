'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Applications.ProductSetup
Imports System.Data
Imports Aptify.Framework.DataServices

Namespace Aptify.Framework.Web.eBusiness.Meetings
    Partial Class Calendar
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced

        Protected Const ATTRIBUTE_MEETING_PAGE As String = "MeetingPage"
        Protected Const ATTRIBUTE_MEETINGS_GRIDVIEW_PAGE As String = "MeetingGridViewPage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "MeetingsCalendar"
        Protected Const ATTRIBUTE_SHOWMEETINGSLINKTOCLASS_DEFAULT_NAME As String = "ShowMeetingsLinkToClass"

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
        ''' MeetingGridView page url
        ''' </summary>
        Public Overridable Property MeetingGridViewPage() As String
            Get
                If Not ViewState(ATTRIBUTE_MEETINGS_GRIDVIEW_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_MEETINGS_GRIDVIEW_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_MEETINGS_GRIDVIEW_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        'Nalini issue 11290
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

            If String.IsNullOrEmpty(MeetingPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                MeetingPage = Me.GetLinkValueFromXML(ATTRIBUTE_MEETING_PAGE)
            End If
            If String.IsNullOrEmpty(MeetingGridViewPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                MeetingGridViewPage = Me.GetLinkValueFromXML(ATTRIBUTE_MEETINGS_GRIDVIEW_PAGE)
                If Not String.IsNullOrEmpty(MeetingGridViewPage) Then
                    Me.MeetingGridPage.NavigateUrl = MeetingGridViewPage
                Else
                    Me.MeetingGridPage.Enabled = False
                    Me.MeetingGridPage.ToolTip = "MeetingGridViewPage property has not been set."
                End If
            End If
            Dim bShowMeeting As Boolean = ShowMeetingsLinkToClass
        End Sub

        'Amruta IssueID 15386,3/4/2013,Revert Meeting Center Calendar View Back to 5.5 Version
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'set control properties from XML file if needed
            SetProperties()
            If Not IsPostBack Then
                cmbYear.SelectedValue = DatePart(DateInterval.Year, Today).ToString
                cmbMonth.SelectedValue = DatePart(DateInterval.Month, Today).ToString
                cmbBottomYear.SelectedValue = DatePart(DateInterval.Year, Today).ToString
                cmbBottomMonth.SelectedValue = DatePart(DateInterval.Month, Today).ToString
            End If
        End Sub

        'Amruta IssueID 15386,3/4/2013,Revert Meeting Center Calendar View Back to 5.5 Version
        Protected Sub Calendar1_DayRender(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DayRenderEventArgs) Handles Calendar1.DayRender
            Dim sSQL As String
            Dim oDT As DataTable
            Dim dStartDate As Date
            Dim dEndDate As Date
            Dim dCalDate As Date
            Dim sMeetingLink As String
            Dim sStartTime As String, sEndtime As String
            Dim sLinkColor As String
            Dim dHeight As Double = 0
            Dim bEnabled As Boolean = True
            Dim link As HyperLink

            Try
                e.Cell.Height = 50
                e.Day.IsSelectable = False
                If e.Day.IsOtherMonth Then
                    'e.Cell.Controls.Clear()
                Else

                    sSQL = "Select 'Red' LinkColor, " & _
                                   "p.productcategoryid, m.productid, m.meetingtitle, m.status,'startdate' = CASE WHEN m.startdate > m.EndDate THEN m.EndDate ELSE m.startdate END, 'enddate'= CASE WHEN  LTRIM(RIGHT(CONVERT(VARCHAR(20), StartDate, 100), 7))='12:00AM' and LTRIM(RIGHT(CONVERT(VARCHAR(20), EndDate, 100), 7))='12:00AM' then Cast(m.EndDate AS varchar(11)) +' 12:00:01AM' Else m.EndDate END   from " & Me.Database & "..vwMeetings m " & _
                                   "inner join " & Me.Database & "..vwproducts p on p.id = m.productid " & _
                                   "left join " & Me.Database & "..vwmeetingattributes a on a.meetingid = m.id where p.webenabled=1  and isnull(p.parentid,-1)=-1"
                    If Not ShowMeetingsLinkToClass Then
                        sSQL &= (" AND  ISNULL(p.ClassID ,-1) <=0  ")
                    End If
                    sSQL &= "order by m.startdate"

                    oDT = Me.DataAction.GetDataTable(sSQL)
                    For Each oRow As DataRow In oDT.Rows
                        dStartDate = CDate(oRow("StartDate"))
                        dEndDate = CDate(oRow("EndDate"))
                        dCalDate = e.Day.Date
                        sLinkColor = CStr(oRow("LinkColor"))

                        If Trim(CStr(oRow("Status"))) = "Cancelled" Then
                            sLinkColor = System.Drawing.Color.Gainsboro.ToString
                        End If

                        If CDate(dCalDate.ToString("MM/dd/yyyy")) >= CDate(dStartDate.ToString("MM/dd/yyyy")) AndAlso CDate(dCalDate.ToString("MM/dd/yyyy")) <= CDate(dEndDate.ToString("MM/dd/yyyy")) AndAlso bEnabled Then

                            sStartTime = dStartDate.ToString("hh:mm tt")
                            sEndtime = dEndDate.ToString("hh:mm tt")

                            If sEndtime = "12:00 AM" Then
                                sEndtime = ""
                            Else
                                sEndtime = " - " & sEndtime
                            End If

                            sMeetingLink = MeetingPage & "?ID=" & CStr(oRow("productid"))

                            link = New HyperLink
                            With link
                                If String.IsNullOrEmpty(MeetingPage) Then
                                    .ToolTip = "MeetingPage property has not been set."
                                    .Enabled = False
                                Else
                                    .Enabled = True
                                End If
                                .NavigateUrl = sMeetingLink
                                '*** Uncomment to include meeting start and end time and Comment out the following line *** 
                                '.Text = "<br><u>" & sStartTime & sEndtime & ":<br>" & CStr(oRow("meetingtitle")) & "</u><br>"
                                '***
                                .Text = "<br><u>" & CStr(oRow("meetingtitle")) & "</u><br>"
                            End With

                            e.Cell.Controls.Add(link)
                            dHeight = Math.Max(dHeight, e.Cell.Height.Value)
                        End If
                    Next
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
            Calendar1.VisibleDate = CDate(cmbMonth.SelectedValue & "/1/" & cmbYear.SelectedValue)
            cmbBottomYear.SelectedValue = cmbYear.SelectedValue
            cmbBottomMonth.SelectedValue = cmbMonth.SelectedValue
        End Sub

        Protected Sub cmbBottomGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbBottomGo.Click
            Calendar1.VisibleDate = CDate(cmbBottomMonth.SelectedValue & "/1/" & cmbBottomYear.SelectedValue)
            cmbYear.SelectedValue = cmbBottomYear.SelectedValue
            cmbMonth.SelectedValue = cmbBottomMonth.SelectedValue
        End Sub

        'HP Issue#8376:  keep combo boxes in-sync with the current month being displayed
        Protected Sub Calendar1_VisibleMonthChanged(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MonthChangedEventArgs) Handles Calendar1.VisibleMonthChanged
            cmbYear.SelectedValue = DatePart(DateInterval.Year, e.NewDate).ToString
            cmbMonth.SelectedValue = DatePart(DateInterval.Month, e.NewDate).ToString
            cmbBottomYear.SelectedValue = DatePart(DateInterval.Year, e.NewDate).ToString
            cmbBottomMonth.SelectedValue = DatePart(DateInterval.Month, e.NewDate).ToString
        End Sub

    End Class
End Namespace