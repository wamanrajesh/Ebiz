''Aptify e-Business 5.5.1, July 2013
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Applications.ProductSetup
Imports System.Data
Imports Aptify.Framework.DataServices
Imports System.IO
Imports Telerik.Web.UI
Imports System.Collections.Generic

Namespace Aptify.Framework.Web.eBusiness.CustomerService
    Partial Class MyMeetings
        Inherits eBusiness.BaseUserControlAdvanced
        Protected Const ATTRIBUTE_MEETING_PAGE As String = "MeetingPage"
        Protected Const ATTRIBUTE_CLOSE_IMAGE_URL As String = "ImgCloseButton"
        Protected Const ATTRIBUTE_PAGE_LOAD_SELECTION As String = "DefaultSql"
        Protected Const ATTRIBUTE_PAGE_SEARCH_INFO As String = "SearchInfo"
        Protected Const ATTRIBUTE_LOGIN_PAGE As String = "LoginPage"
        Protected IsDefaultSQL As Boolean = False


#Region "Properties"
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
        Public Property ImgCloseButton() As String
            Get
                If ViewState.Item("ImgCloseButton") IsNot Nothing Then
                    Return ViewState.Item("ImgCloseButton").ToString()
                Else
                    Return ""
                End If

            End Get
            Set(ByVal value As String)
                ViewState.Item("ImgCloseButton") = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        'Anil B For 11155 on 26-04-2013
        'Added following properties for configuring text on page and for login page
        Public Overridable Property DefaultSql() As String
            Get
                If Not ViewState(ATTRIBUTE_PAGE_LOAD_SELECTION) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_PAGE_LOAD_SELECTION))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_PAGE_LOAD_SELECTION) = value
            End Set
        End Property
        Public Overridable Property SearchInfo() As String
            Get
                If Not ViewState(ATTRIBUTE_PAGE_SEARCH_INFO) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_PAGE_SEARCH_INFO))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_PAGE_SEARCH_INFO) = value
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
#End Region
        Protected Overrides Sub SetProperties()
            'If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            If String.IsNullOrEmpty(ImgCloseButton) Then
                ImgCloseButton = Me.GetLinkValueFromXML(ATTRIBUTE_CLOSE_IMAGE_URL)
            End If

            If String.IsNullOrEmpty(MeetingPage) Then
                MeetingPage = Me.GetLinkValueFromXML(ATTRIBUTE_MEETING_PAGE)
                If String.IsNullOrEmpty(MeetingPage) Then
                    Me.grdMeetings.Enabled = False
                    Me.grdMeetings.ToolTip = "MeetingPage property has not been set."
                End If
            End If
            'Anil B For 11155 on 26-04-2013
            'Added following properties for configuring text on page and for login page
            If String.IsNullOrEmpty(DefaultSql) Then
                DefaultSql = Me.GetPropertyValueFromXML(ATTRIBUTE_PAGE_LOAD_SELECTION)
            End If
            If String.IsNullOrEmpty(SearchInfo) Then
                SearchInfo = Me.GetPropertyValueFromXML(ATTRIBUTE_PAGE_SEARCH_INFO)
                lblSearchInfo.Text = SearchInfo
            End If
            If String.IsNullOrEmpty(LoginPage) Then
                LoginPage = Me.GetLinkValueFromXML(ATTRIBUTE_LOGIN_PAGE)
            End If
        End Sub

        Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
            SetProperties()
            ImgClose.Src = ImgCloseButton
            If Not IsPostBack Then
                If User1.UserID > 0 Then
                    'Suraj Issue 14450 3/22/13 ,this method use to apply the odrering of rad grid first column
                    AddExpressionMyMeeting()
                    'Anil B For 11155 on 16-04-2013
                    'Set sorting for details grid
                    AddDetailExpression()
                    BindToolTipsForDateTimePicker()
                    IsDefaultSQL = True
                    LoadMyMeetings(False)
                    LoadAttendeeStatus()
                    LoadNotification()
                    'Anil B For 11155 on 26-04-2013
                    'Set date for page load condition 
                    txtStartDate.Clear()
                    txtEndDate.Clear()
                    txtStartDate.MinDate = New Date(1980, 1, 1)
                    txtStartDate.MaxDate = New Date(2099, 12, 30)
                    txtEndDate.MinDate = Date.Now.AddDays(-30)
                    txtEndDate.MaxDate = New Date(2099, 12, 30)
                Else
                    'Anil B For 11155 on 26-04-2013
                    'Redirect to login page if direct access without login
                    'Suraj Issue 15370,7/31/13 revert back changes application variale to session variable
                    Session("ReturnToPage") = Request.RawUrl
                    ' Suraj S Issue 15370, 8/1/13 here we are getting the ReturnToPageURL in "URL" QueryString and passing on login page. 
                    Response.Redirect(LoginPage + "?ReturnURL=" & System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(Request.RawUrl)))
                End If
            End If
        End Sub
        'Anil B For 11155 on 16-04-2013
        'Added parameter to fuction
        Protected Overridable Sub LoadMyMeetings(bSearch As Boolean)
            Try

                Dim sSQL As String, dt As DataTable, sDB As String
                sDB = Me.AptifyApplication.GetEntityBaseDatabase("Meetings")
                sSQL = GetSql()
                dt = Me.DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                Dim dcolUrl As DataColumn = New DataColumn()
                dcolUrl.Caption = "MeetingUrl"
                dcolUrl.ColumnName = "MeetingUrl"
                dt.Columns.Add(dcolUrl)
                If dt.Rows.Count > 0 Then
                    For Each rw As DataRow In dt.Rows
                        rw("MeetingUrl") = MeetingPage + "?ID=" + rw("ProductID").ToString
                    Next
                End If
                'Anil B For 11155 on 16-04-2013
                'Used Databind function on search button
                Me.grdMeetings.DataSource = dt
                If bSearch Then
                grdMeetings.DataBind()
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try

        End Sub

        Protected Overridable Sub LoadAttendeeStatus()
            Try
                ' Anil B for issue 11155 on 21-03-2013
                'Changed as per asp dropdownlist and change entity name
                Dim sSQL As String
                Dim sDatabase As String = AptifyApplication.GetEntityBaseDatabase("Attendee Status")
                Dim dtAttendeeStatus As Data.DataTable
                sSQL = sDatabase & "..spMeetingAttendeeStatus"
                sSQL = "EXEC spMeetingAttendeeStatus"
                dtAttendeeStatus = Me.DataAction.GetDataTable(sSQL)
                If dtAttendeeStatus IsNot Nothing AndAlso dtAttendeeStatus.Rows.Count > 0 Then
                    cmbStatus.DataSource = dtAttendeeStatus
                    cmbStatus.DataTextField = "Name"
                    cmbStatus.DataValueField = "Name"
                    cmbStatus.DataBind()
                    cmbCategory.Items.Insert(0, New ListItem(DefaultSql, String.Empty))
                End If
                dtAttendeeStatus.Clear()
                dtAttendeeStatus = Nothing
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Protected Sub cmbCategory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCategory.SelectedIndexChanged
            Try

                'Anil B For 11155 on 16-04-2013
                'Changed meeting categories
                Select Case cmbCategory.Text.ToUpper
                    Case "ALL"
                        txtStartDate.MinDate = New Date(1980, 1, 1)
                        txtStartDate.MaxDate = New Date(2099, 12, 30)
                        txtEndDate.MinDate = New Date(1980, 1, 1)
                        txtEndDate.MaxDate = New Date(2099, 12, 30)
                    Case "PAST"
                        txtStartDate.Clear()
                        txtEndDate.Clear()
                        txtStartDate.MinDate = New Date(1980, 1, 1)
                        txtStartDate.MaxDate = Date.Today.AddDays(-1)
                        txtEndDate.MinDate = New Date(1980, 1, 1)
                        txtEndDate.MaxDate = New Date(2099, 12, 30)
                    Case "UPCOMING"
                        txtStartDate.Clear()
                        txtEndDate.Clear()
                        txtStartDate.MinDate = Date.Now
                        txtStartDate.MaxDate = New Date(2099, 12, 30)
                        txtEndDate.MinDate = Date.Now
                        txtEndDate.MaxDate = New Date(2099, 12, 30)
                        'Anil B For 11155 on 26-04-2013
                        'Set date for page load condition
                    Case DefaultSql.Trim.ToUpper
                        txtStartDate.Clear()
                        txtEndDate.Clear()
                        txtStartDate.MinDate = Date.Now
                        txtStartDate.MaxDate = New Date(2099, 12, 30)
                        txtEndDate.MinDate = Date.Now.AddDays(-30)
                        txtEndDate.MaxDate = New Date(2099, 12, 30)
                End Select
            Catch ex As Exception
            End Try
        End Sub
        Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
            'Suraj Issue 14450 3/22/13 ,this method use to apply the odrering of rad grid first column
            'Anil B For 11155 on 16-04-2013
            'Added sorting for detail grid
            AddExpressionMyMeeting()
            AddDetailExpression()
            LoadMyMeetings(True)
        End Sub
        'Anil B For 11155 on 26-04-2013
        'Change filter logic
        Protected Overridable Function GetSql() As String
            Try
                'Anil B For 11155 on 16-04-2013
                'Changes the query according to the meeting parent or chiled
                Dim sSQL As String, sDB As String
                sDB = Me.AptifyApplication.GetEntityBaseDatabase("Meetings")
                'Anil B for issue 11155 on 3-05-2013
                'Updated  query 
                sSQL = "Select O.MeetingName,P.WebName,O.AttendeeID,o.OrderID,M.ID as MeetingID, O.AttendeeID_FirstLast,O.RegistrationType,S.Name Status,M.place Location,O.ProductID,M.StartDate,M.EndDate " & _
                                           "from " & sDB & "..vwOrderMeetDetail O inner join " & sDB & "..vwMeetings M on O.ProductID= M.ProductID " & _
                                           "inner join " & sDB & "..vwAttendeeStatus S on O.StatusID=S.ID " & _
                                           "inner join " & sDB & "..vwproducts P on P.ID=O.ProductID " & _
                                           " where O.AttendeeID = " & User1.PersonID & " and S.ShowonWeb=1 and (parentMeetingID is  null or parentMeetingID=-1)  "
                If IsDefaultSQL = True Then
                    sSQL &= "and (M.EndDate > = Convert(datetime,DATEADD(second, -1, DATEADD(DAY, 1,getdate()-30)), 121))"
                    IsDefaultSQL = False
                    Return sSQL
                Else
                    If cmbCategory.SelectedItem.Text.Trim = DefaultSql.Trim Then
                        sSQL &= "and (M.EndDate > = Convert(datetime,DATEADD(second, -1, DATEADD(DAY, 1,getdate()-30)), 121))"
                    End If
                    If cmbStatus.SelectedItem.Text.ToUpper.Trim <> "ALL" Then
                        sSQL &= "AND S.Name='" + cmbStatus.Text + "'"
                    End If
                    If txtStartDate.SelectedDate IsNot Nothing AndAlso txtEndDate.SelectedDate IsNot Nothing Then
                        sSQL &= " AND (m.StartDate>=  Convert(datetime,DATEADD(second, -1, '" + txtStartDate.SelectedDate + "'), 121) and EndDate<= Convert(datetime,DATEADD(second, -1, DATEADD(DAY, 1,'" + txtEndDate.SelectedDate + "')), 121))"
                    Else
                        If txtStartDate.SelectedDate IsNot Nothing OrElse txtEndDate.SelectedDate IsNot Nothing Then
                            If txtStartDate.SelectedDate IsNot Nothing Then
                                sSQL &= " AND m.StartDate>=  Convert(datetime,DATEADD(second, -1, '" + txtStartDate.SelectedDate + "'), 121)"
                            Else
                                'Anil B for issue 11155 on 3-05-2013
                                'Updated  query for end date search 
                                sSQL &= " AND  EndDate<=  Convert(datetime,DATEADD(second, -1, DATEADD(DAY, 1,'" + txtEndDate.SelectedDate + "')), 121)"

                            End If
                        Else
                            Select Case cmbCategory.Text.ToUpper
                                Case "UPCOMING"
                                    sSQL &= " AND DATEDIFF(dd,GETDATE(),m.StartDate) >= 0 "
                                    sSQL &= " ORDER BY m.StartDate , m.EndDate "
                                Case "PAST"
                                    sSQL &= " AND DATEDIFF(dd,GETDATE(),m.StartDate) < 0 "
                            End Select
                        End If
                    End If
                    IsDefaultSQL = False
                    Return sSQL
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Function
        Protected Overridable Sub LoadNotification()
            Try
                Dim iTotalNotificationCount As Integer = 0
                Dim sSQL As String, dt As DataTable, dts As DataTable, dtRecordChange As DataTable, sDB As String
                Dim dtUpcomingMeetingNotification As DataTable = New DataTable()
                Dim drNotifications As DataRow = Nothing
                dtUpcomingMeetingNotification.Columns.Add(New DataColumn("Description", GetType(String)))
                sDB = Me.AptifyApplication.GetEntityBaseDatabase("Meetings")
                sSQL = "EXEC spGetUserUpcomingMeetings " & User1.PersonID
                dt = Me.DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                dt.Columns.Add(New DataColumn("Description", GetType(String)))
                Dim dr As DataRow
                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                    For Each dr In dt.Rows
                        dr("Description") = dr("WebName") & ": From " & dr("StartDate") & "  to  " & dr("EndDate") & " - Your Status: " & dr("AttendeeStatus_Name")
                        drNotifications = dtUpcomingMeetingNotification.NewRow()
                        drNotifications("Description") = dr("WebName") & ": From " & dr("StartDate") & "  to  " & dr("EndDate") & " - Your Status: " & dr("AttendeeStatus_Name")
                        dtUpcomingMeetingNotification.Rows.Add(drNotifications)
                    Next
                End If
                dt = Nothing
                If dtUpcomingMeetingNotification IsNot Nothing AndAlso dtUpcomingMeetingNotification.Rows.Count > 0 Then
                    blListUpcomingMeetings.DataSource = dtUpcomingMeetingNotification
                    blListUpcomingMeetings.DataTextField = "Description"
                    blListUpcomingMeetings.DataBind()
                    lblNotificationDivHeading.Text = "Your Registered Events (" & dtUpcomingMeetingNotification.Rows.Count & ")"
                    iTotalNotificationCount = iTotalNotificationCount + dtUpcomingMeetingNotification.Rows.Count
                Else
                    lblNotificationDivHeading.Text = "Your Registered Event (0)"
                    iTotalNotificationCount = iTotalNotificationCount + 0
                End If
                dtUpcomingMeetingNotification = Nothing

                sSQL = "EXEC spGetMeetingsForYou " & User1.PersonID
                Dim dtMeetingsForYouNotification As DataTable = New DataTable()
                dtMeetingsForYouNotification.Columns.Add(New DataColumn("Description", GetType(String)))
                dts = Me.DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                dts.Columns.Add(New DataColumn("Description", GetType(String)))
                If dts IsNot Nothing AndAlso dts.Rows.Count > 0 Then
                    For Each dr In dts.Rows
                        dr("Description") = dr("WebName") & ": Last date of registration is " & dr("AvailableUntil")
                        drNotifications = dtMeetingsForYouNotification.NewRow()
                        drNotifications("Description") = dr("WebName") & ": Last date of registration is " & dr("AvailableUntil")
                        dtMeetingsForYouNotification.Rows.Add(drNotifications)
                    Next
                End If
                dts = Nothing

                If dtMeetingsForYouNotification IsNot Nothing AndAlso dtMeetingsForYouNotification.Rows.Count > 0 Then
                    blListMeetingsforyou.DataSource = dtMeetingsForYouNotification
                    blListMeetingsforyou.DataTextField = "Description"
                    blListMeetingsforyou.DataBind()
                    lblNotificationForYou.Text = "Upcoming Events (" & dtMeetingsForYouNotification.Rows.Count & ")"
                    iTotalNotificationCount = iTotalNotificationCount + dtMeetingsForYouNotification.Rows.Count
                Else
                    lblNotificationForYou.Text = "Upcoming Event (0)"
                    iTotalNotificationCount = iTotalNotificationCount + 0
                End If
                dtMeetingsForYouNotification = Nothing
                'Suraj S Issue 15426,4/15/13 , This  issue documents the need to back out the spGetMeetingRecordChange stored procedure and remove the Announcements section from the My Meetings page.
                lblTotalNotificationCount.Text = iTotalNotificationCount
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        'Suraj issue 14450 2/12/13 date time filtering
        Protected Sub grdMeetings_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdMeetings.ItemDataBound
            Dim dateColumns As New List(Of String)
            'Add datecolumn uniqueName in list for Date format
            dateColumns.Add("GridDateTimeColumnStartDate")
            dateColumns.Add("GridDateTimeColumnEndDate")
            CommonMethods.FormatedDateOnGrid(dateColumns, e.Item)
        End Sub
        Protected Sub grdMeetings_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdMeetings.NeedDataSource
            LoadMyMeetings(False)
        End Sub
        'Suraj Issue 14450 3/22/13 ,if the grid load first time By default the sorting will be Ascending for column Forum 
        Private Sub AddExpressionMyMeeting()
            'Anil B For 11155 on 16-04-2013
            'Changes sorting expression
            Dim expression1 As New GridSortExpression
            expression1.FieldName = "StartDate"
            expression1.SetSortOrder("Ascending")
            grdMeetings.MasterTableView.SortExpressions.AddSortExpression(expression1)
        End Sub
        'Anil B For 11155 on 16-04-2013
        'Function for datails grid sorting
        Private Sub AddDetailExpression()
            Dim DetailExpression As New GridSortExpression
            DetailExpression.FieldName = "StartDate"
            DetailExpression.SetSortOrder("Ascending")
            grdMeetings.MasterTableView.DetailTables.Item(0).SortExpressions.AddSortExpression(DetailExpression)
        End Sub
        'Anil B For 11155 on 26-04-2013
        'Changes tooltips
        Private Sub BindToolTipsForDateTimePicker()
            'Suraj Issue 14450 3/22/13 ,we provide a tool tip for DatePopupButton as well as the GridDateTimeColumnStartDate textbox   
            txtStartDate.ToolTip = "Show Meetings On or After the Date You Enter."
            txtStartDate.DatePopupButton.ToolTip = "Show Meetings On or After the Date You Enter."
            txtEndDate.ToolTip = "Show Meetings that End On or Before the Date You Enter."
            txtEndDate.DatePopupButton.ToolTip = "Show Meetings that End On or Before the Date You Enter."
        End Sub
        'Anil B For 11155 on 26-04-2013
        'Binding the details grid
        Protected Sub grdMeetings_DetailTableDataBind(sender As Object, e As Telerik.Web.UI.GridDetailTableDataBindEventArgs) Handles grdMeetings.DetailTableDataBind
            Try
                Dim dtDeatil As DataTable
                Dim sSQL, sOrderId, sDB As String
                Dim gDataItem As Telerik.Web.UI.GridDataItem = CType(e.DetailTableView.ParentItem, Telerik.Web.UI.GridDataItem)
                Dim bExpanded As Boolean = e.DetailTableView.ParentItem.Expanded
                sDB = Me.AptifyApplication.GetEntityBaseDatabase("Meetings")
                Dim iID As Integer = 0
                If Not IsDBNull(gDataItem.GetDataKeyValue("MeetingID")) Then
                    iID = gDataItem.GetDataKeyValue("MeetingID")
                End If
                sOrderId = e.DetailTableView.ParentItem("OrderID").Text
                'Anil B for issue 11155 on 3-05-2013
                'Updated  query for details grid
                sSQL = "Select O.MeetingName,P.WebName,O.AttendeeID,o.OrderID,M.ID as MeetingID, O.AttendeeID_FirstLast,O.RegistrationType,S.Name Status,M.place Location,O.ProductID,M.StartDate,M.EndDate " & _
                                              "from " & sDB & "..vwOrderMeetDetail O inner join " & sDB & "..vwMeetings M on O.ProductID= M.ProductID " & _
                                              "inner join " & sDB & "..vwAttendeeStatus S on O.StatusID=S.ID " & _
                                              "inner join " & sDB & "..vwproducts P on P.ID=O.ProductID " & _
                                              " where m.ParentMeetingID = " & iID & " and S.ShowonWeb=1  AND O.AttendeeID =" & User1.PersonID
                dtDeatil = Me.DataAction.GetDataTable(sSQL)
                If dtDeatil IsNot Nothing Then
                    Dim dcolUrl As DataColumn = New DataColumn()
                    dcolUrl.Caption = "MeetingUrl"
                    dcolUrl.ColumnName = "MeetingUrl"
                    dtDeatil.Columns.Add(dcolUrl)
                    If dtDeatil.Rows.Count > 0 Then
                        For Each rw As DataRow In dtDeatil.Rows
                            rw("MeetingUrl") = MeetingPage + "?ID=" + rw("ProductID").ToString
                        Next
                    End If
                    e.DetailTableView.DataSource = dtDeatil
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
    End Class
End Namespace

