'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On
Imports System.Data
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Applications.ProductSetup
Imports Aptify.Framework


Namespace Aptify.Framework.Web.eBusiness
    Partial Class EventsHome
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced


        Protected Const ATTRIBUTE_EVENTS_PAGE As String = "EventsPage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "EventsHome"
        Protected Const ATTRIBUTE_MEETINGS_CALENDAR_PAGE As String = "MeetingCalendarViewPage"

#Region "EventsHome Specific Properties"
        Public Overridable Property EventsPage() As String
            Get
                If Not ViewState(ATTRIBUTE_EVENTS_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_EVENTS_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_EVENTS_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' MeetingsCalendarView page url
        ''' </summary>
        Public Overridable Property MeetingsCalendarViewPage() As String
            Get
                If Not ViewState(ATTRIBUTE_MEETINGS_CALENDAR_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_MEETINGS_CALENDAR_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_MEETINGS_CALENDAR_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
#End Region


        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(EventsPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                EventsPage = Me.GetLinkValueFromXML(ATTRIBUTE_EVENTS_PAGE)
                If String.IsNullOrEmpty(EventsPage) Then
                    Me.grdMeetings.Enabled = False
                    Me.grdMeetings.ToolTip = "MeetingPage property has not been set."
                End If
            End If
            If String.IsNullOrEmpty(MeetingsCalendarViewPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                MeetingsCalendarViewPage = Me.GetLinkValueFromXML(ATTRIBUTE_MEETINGS_CALENDAR_PAGE)
                If Not String.IsNullOrEmpty(MeetingsCalendarViewPage) Then
                    Me.MeetingsCalendarPage.NavigateUrl = MeetingsCalendarViewPage
                Else
                    Me.MeetingsCalendarPage.Enabled = False
                    Me.MeetingsCalendarPage.ToolTip = "MeetingsCalendarViewPage property has not been set."
                End If
            End If
        End Sub


        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'set control properties from XML file if needed
            SetProperties()
            If Not IsPostBack Then
                LoadCategories()
                LoadSchedule()
            End If
        End Sub

        Private Sub LoadCategories()
            Dim sSQL As String, dt As DataTable
            Try
                sSQL = "SELECT ID, WebName  FROM " & _
                       Me.AptifyApplication.GetEntityBaseDatabase("Products") & _
                       "..vwProductCategories WHERE WebEnabled=1 AND ID IN (" & _
                       "SELECT CategoryID FROM " & _
                       Me.AptifyApplication.GetEntityBaseDatabase("Products") & _
                       "..vwProducts WHERE ProductType='Meeting' And ISNULL(ParentID,-1)=-1) ORDER BY WebName"
                dt = Me.DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                cmbCategory.Items.Clear()
                cmbCategory.Items.Add(New ListItem("All", "-1"))
                If Not dt Is Nothing Then
                    For Each dr As DataRow In dt.Rows
                        cmbCategory.Items.Add(New ListItem(dr("WebName").ToString.Trim, dr("ID").ToString))
                    Next
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Private Sub LoadSchedule()
            Session("DTSCHEDULE") = Nothing
            Dim sSQL As String, dt As DataTable, sDB As String
            Try
                sDB = Me.AptifyApplication.GetEntityBaseDatabase("Meetings")

                sSQL = " SELECT p.ID ProductID,p.WebName, p.Description," & _
                       " m.StartDate,m.EndDate,CASE ISNULL(m.State,'') WHEN '' " & _
                       " THEN m.Place + ' - ' + m.City + ', ' + m.Country ELSE " & _
                       " m.Place + ' - ' + m.City + ', ' + m.State + ' ' + m.Country END Location, " & _
                       " m.StartDate,m.EndDate,pc.WebName WebCategoryName " & _
                       " FROM " & sDB & "..vwProducts p INNER JOIN " & _
                       sDB & "..vwMeetings m ON p.ID=m.ProductID " & _
                       " INNER JOIN " & sDB & "..vwProductCategories pc " & _
                       " ON pc.ID=p.CategoryID " & _
                       " WHERE ISNULL(p.ParentID,-1)<=0 AND pc.WebEnabled=1 AND p.WebEnabled=1 "
                If Val(cmbCategory.SelectedValue) > 0 Then
                    sSQL &= " AND pc.ID=" & cmbCategory.SelectedValue
                End If
                If String.Compare(Me.cmbStatus.SelectedValue, "All", True) <> 0 Then
                    Select Case cmbStatus.SelectedValue.ToUpper
                        Case "FUTURE"
                            sSQL &= " AND m.StartDate>GETDATE() "
                        Case "PAST"
                            sSQL &= " AND m.EndDate<GETDATE() "
                        Case "CURRENT"
                            sSQL &= " AND m.StartDate<=GETDATE() and m.EndDate>=GETDATE() "
                    End Select
                End If
                sSQL &= " ORDER BY m.StartDate DESC, m.EndDate DESC"
                dt = Me.DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                Dim dcol As DataColumn = New DataColumn()
                dcol.Caption = "Price"
                dcol.ColumnName = "Price"
                dt.Columns.Add(dcol)
                If dt.Rows.Count > 0 Then
                    Dim oPrice As New Aptify.Applications.OrderEntry.IProductPrice.PriceInfo
                    For Each rw As DataRow In dt.Rows
                        oPrice = Me.ShoppingCart1.GetUserProductPrice(CLng(rw("ProductID")))
                        If oPrice.Price = 0 Then
                            rw("Price") = "Free"
                        Else
                            rw("Price") = Format(oPrice.Price, User1.PreferredCurrencyFormat)
                        End If
                       
                    Next
                End If
                Session("DTSCHEDULE") = dt

                DirectCast(grdMeetings.Columns(0), HyperLinkColumn).DataNavigateUrlFormatString = EventsPage & "?ID={0:F0}"
                If ConfigurationManager.AppSettings("PageSize") Is Nothing Then
                    Me.grdMeetings.PageSize = 10
                Else
                Me.grdMeetings.PageSize = CInt(ConfigurationManager.AppSettings("PageSize").ToString)
                End If

                Me.grdMeetings.DataSource = Session("DTSCHEDULE")
                Me.grdMeetings.DataBind()

                With grdMeetings
                    If (.PageCount > 1) Then
                        .PagerStyle.Visible = True
                    Else
                        .PagerStyle.Visible = False
                    End If
                    If .Items.Count > 0 Then
                        Dim i As Integer
                        For i = 0 To .Items.Count
                            Dim strTime As String = CDate(dt.Rows(i)("StartDate")).TimeOfDay.ToString
                            If strTime = "00:00:00" Then
                                .Items(i).Cells(4).Text = FormatDateTime(CDate(dt.Rows(i)("StartDate")), DateFormat.ShortDate)
                            End If
                            strTime = CDate(dt.Rows(i)("EndDate")).TimeOfDay.ToString
                            If strTime = "00:00:00" Then
                                .Items(i).Cells(5).Text = FormatDateTime(CDate(dt.Rows(i)("EndDate")), DateFormat.ShortDate)
                            End If
                        Next
                    End If
                End With


            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub cmbCategory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCategory.SelectedIndexChanged
            LoadSchedule()
        End Sub

        Protected Sub cmbStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStatus.SelectedIndexChanged
            LoadSchedule()
        End Sub



        Protected Sub grdMeetings_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdMeetings.ItemDataBound
            If e.Item.ItemType = ListItemType.Item Then
                With CType(e.Item.FindControl(""), HyperLink)

                End With
            End If
        End Sub

        Protected Sub grdMeetings_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles grdMeetings.PageIndexChanged
            Try
                grdMeetings.CurrentPageIndex = e.NewPageIndex
                grdMeetings.DataSource = Session("DTSCHEDULE")
                grdMeetings.DataBind()
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
    End Class
End Namespace

