'Aptify e-Business 5.5.1, July 2013
'Option Explicit On
'Option Strict On
Imports System.Data
Imports Aptify.Framework.ItemRatings
Imports Aptify.Framework.Application
Imports Aptify.Framework.DataServices
Imports Telerik.Web.UI
Imports System.Collections.Generic

Namespace Aptify.Framework.Web.eBusiness.Meetings
    Partial Class MeetingsHome
        Inherits eBusiness.BaseUserControlAdvanced

        Protected Const ATTRIBUTE_MEETING_PAGE As String = "MeetingPage"
        Protected Const ATTRIBUTE_MEETINGS_CALENDAR_PAGE As String = "MeetingCalendarViewPage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "MeetingsHome"
        Protected Const ATTRIBUTE_LOGIN_PAGE As String = "LoginPage"
        Protected Const ATTRIBUTE_SHOWMEETINGSLINKTOCLASS_DEFAULT_NAME As String = "ShowMeetingsLinkToClass"
        Dim sSQL As String, dt As DataTable, sDB As String
        'Suraj issue 14457 , 3/5/13 , declare constant  for viewstate
        Protected Const ATTRIBUTE_MEETINGSHOMEDT_VIEWSTATE As String = "MeetingsHomedt"

#Region "MeetingsHome Specific Properties"
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

            'If String.IsNullOrEmpty(LoginPage) Then
            '    'since value is the 'default' check the XML file for possible custom setting
            '    LoginPage = Me.GetLinkValueFromXML(ATTRIBUTE_LOGIN_PAGE)
            '    If String.IsNullOrEmpty(LoginPage) Then
            '        Me.grdMeetings.Enabled = False
            '        Me.grdMeetings.ToolTip = "LoginPage property has not been set."
            '    End If
            'End If


            If String.IsNullOrEmpty(MeetingPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                MeetingPage = Me.GetLinkValueFromXML(ATTRIBUTE_MEETING_PAGE)
                If String.IsNullOrEmpty(MeetingPage) Then
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
            Dim bShowMeeting As Boolean = ShowMeetingsLinkToClass
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'set control properties from XML file if needed
            SetProperties()
            If Not IsPostBack Then
                'Suraj issue 14457 3/5/13 ,this method use to apply the odrering of rad grid first column
                AddExpression()
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
                cmbCategory.Items.Add(New System.Web.UI.WebControls.ListItem("<All Categories>", "-1"))
                If Not dt Is Nothing Then
                    For Each dr As DataRow In dt.Rows
                        cmbCategory.Items.Add(New System.Web.UI.WebControls.ListItem(dr("WebName").ToString.Trim, dr("ID").ToString))
                    Next
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Private Sub LoadSchedule()

            Try
                'Suraj issue 14457 , 3/5/13 , check the view state is nothing or not if the page load first time viewstate will be nothing but after bostback view state will conatin the datatable
                If ViewState(ATTRIBUTE_MEETINGSHOMEDT_VIEWSTATE) Is Nothing Then
                    SetProperties()
                    sDB = Me.AptifyApplication.GetEntityBaseDatabase("Meetings")
                    'Suraj Issue 14829 4/29/13, remove "Not Mention" if the AvailableUntil date is null we are showing blank date for maintain the uniformability
                    sSQL = " SELECT p.ID ProductID,p.WebName," & _
                     " m.StartDate, m.EndDate, m.place Location,isnull(m.MeetingID,0) as ID,'VerboseDescription'=case when m.VerboseDescription='' or m.VerboseDescription is null then 'No Description'  else m.VerboseDescription end,'AvailableUntil'=  CASE WHEN AvailableUntil IS NOT NULL THEN AvailableUntil  ELSE AvailableUntil END,'Smalldesc'=case when VerboseDescription='' or VerboseDescription is null then 'No Description'  else substring(VerboseDescription,0,35) end, 'AvailableUntil'=CASE WHEN AvailableUntil IS NOT NULL THEN CAST(AvailableUntil AS VARCHAR(11)) ELSE AvailableUntil END, " & _
                      " CONVERT(VARCHAR(12),m.StartDate,107) StartDate,CONVERT(VARCHAR(12),m.EndDate,107) EndDate,pc.WebName WebCategoryName " & _
                      " FROM " & sDB & "..vwProducts p INNER JOIN " & _
                           sDB & "..vwMeetings m ON p.ID=m.ProductID " & _
                           " INNER JOIN " & sDB & "..vwProductCategories pc " & _
                           " ON pc.ID=p.CategoryID " & _
                           " WHERE ISNULL(p.ParentID,-1)<=0 AND pc.WebEnabled=1 AND p.WebEnabled=1 "
                    ' Nalini issue 11290
                    If Not ShowMeetingsLinkToClass Then
                        sSQL &= (" AND  ISNULL(p.ClassID ,-1) <=0  ")
                    End If
                   
                    If Val(cmbCategory.SelectedValue) > 0 Then
                        sSQL &= " AND pc.ID=" & cmbCategory.SelectedValue
                    End If
                    If String.Compare(Me.cmbStatus.SelectedValue, "All", True) <> 0 Then
                        Select Case cmbStatus.SelectedValue.ToUpper
                            Case "UPCOMING"
                                sSQL &= " AND DATEDIFF(dd,GETDATE(),m.StartDate) >= 0 "
                                sSQL &= " ORDER BY m.StartDate , m.EndDate "
                            Case "PAST"
                                sSQL &= " AND DATEDIFF(dd,GETDATE(),m.StartDate) < 0 "
                        End Select
                    End If
                    If cmbStatus.SelectedValue.ToUpper <> "UPCOMING" Then
                        sSQL &= " ORDER BY m.StartDate DESC, m.EndDate DESC"
                    End If

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

                    Dim dcolUrl As DataColumn = New DataColumn()
                    dcolUrl.Caption = "MeetingUrl"
                    dcolUrl.ColumnName = "MeetingUrl"

                    dt.Columns.Add(dcolUrl)
                    If dt.Rows.Count > 0 Then

                        For Each rw As DataRow In dt.Rows
                            rw("MeetingUrl") = MeetingPage + "?ID=" + rw("ProductID").ToString
                        Next
                    End If
                    Dim dcolRating As DataColumn = New DataColumn()
                    dcolRating.Caption = "MeetingRate"
                    dcolRating.ColumnName = "MeetingRate"

                    dt.Columns.Add(dcolRating)
                    If dt.Rows.Count > 0 Then

                        For Each rw As DataRow In dt.Rows
                            Dim itemratinginputobj As New ItemRatingInput()

                            itemratinginputobj.ItemRatingURI = "MeetingEvaluation/EntityRecord/Products/" + rw("ProductID").ToString

                            itemratinginputobj.ItemRatingTypeURI = ""
                            itemratinginputobj.RatedItemURI = ""


                            Dim ItemRatingServiceInformationobj As New ItemRatingServiceInformation()
                            ItemRatingServiceInformationobj = GetItemRating(itemratinginputobj)
                            If (ItemRatingServiceInformationobj.ItemRatingDetails.PersonEntry Is Nothing) Then
                                If Not ItemRatingServiceInformationobj.ItemRatingDetails.RecordedValueAverage Is Nothing Then
                                    rw("MeetingRate") = Math.Round(Decimal.Parse(ItemRatingServiceInformationobj.ItemRatingDetails.RecordedValueAverage), 1).ToString()
                                Else
                                    rw("MeetingRate") = 0
                                End If


                            Else
                                If Not ItemRatingServiceInformationobj.ItemRatingDetails.RecordedValueAverage Is Nothing Then
                                    rw("MeetingRate") = Math.Round(Decimal.Parse(ItemRatingServiceInformationobj.ItemRatingDetails.RecordedValueAverage), 1).ToString()
                                Else
                                    rw("MeetingRate") = 0
                                End If
                            End If

                        Next
                    End If

                    If dt.Rows.Count > 0 Then
                        Me.grdMeetings.DataSource = dt
                        Me.grdMeetings.DataBind()
                        grdMeetings.Visible = True
                        lblMessage.Text = ""
                        'Suraj issue 14457 , 3/5/13 , if when page first time load here we store the datatable in to a viewstate
                        ViewState(ATTRIBUTE_MEETINGSHOMEDT_VIEWSTATE) = dt
                    Else
                        grdMeetings.DataSource = Nothing
                        grdMeetings.Visible = False
                        ViewState(ATTRIBUTE_MEETINGSHOMEDT_VIEWSTATE) = Nothing
                        lblMessage.Text = "No Data Available"
                    End If
                Else
                    'Suraj issue 14457 , 3/5/13 , after postback viewstate will assign for gridview
                    Dim tempFWNewdt As DataTable = CType(ViewState(ATTRIBUTE_MEETINGSHOMEDT_VIEWSTATE), DataTable)
                    If tempFWNewdt.Rows.Count > 0 Then
                        Me.grdMeetings.DataSource = CType(ViewState(ATTRIBUTE_MEETINGSHOMEDT_VIEWSTATE), DataTable)
                        grdMeetings.DataBind()
                    End If
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub cmbCategory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCategory.SelectedIndexChanged
            'Suraj issue 14457 , 3/5/13 , Here we assign nothing to the view state because every selected index result will be a diffrent.
            ViewState(ATTRIBUTE_MEETINGSHOMEDT_VIEWSTATE) = Nothing
            'Suraj issue 14457 3/5/13 ,this method use to apply the odrering of rad grid first column
            Try
                AddExpression()
                LoadSchedule()
            Catch ex As Exception

            End Try

        End Sub

        Protected Sub cmbStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStatus.SelectedIndexChanged
            'Suraj issue 14457 , 3/5/13 , Here we assign nothing to the view state because every selected index result will be a diffrent.
            ViewState(ATTRIBUTE_MEETINGSHOMEDT_VIEWSTATE) = Nothing
            'Suraj issue 14457 3/5/13 ,this method use to apply the odrering of rad grid first column
            Try
                AddExpression()
                LoadSchedule()
            Catch ex As Exception
            End Try

        End Sub

        Protected Sub grdMeetings_PageIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageChangedEventArgs) Handles grdMeetings.PageIndexChanged
            grdMeetings.CurrentPageIndex = e.NewPageIndex
            If ViewState(ATTRIBUTE_MEETINGSHOMEDT_VIEWSTATE) IsNot Nothing Then
                grdMeetings.DataSource = CType(ViewState(ATTRIBUTE_MEETINGSHOMEDT_VIEWSTATE), DataTable)
            End If
        End Sub
        Protected Sub grdMeetings_PagesizeChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageSizeChangedEventArgs) Handles grdMeetings.PageSizeChanged
            If ViewState(ATTRIBUTE_MEETINGSHOMEDT_VIEWSTATE) IsNot Nothing Then
                grdMeetings.DataSource = CType(ViewState(ATTRIBUTE_MEETINGSHOMEDT_VIEWSTATE), DataTable)
            End If
        End Sub

        Protected Sub grdMeetings_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdMeetings.NeedDataSource
            If ViewState(ATTRIBUTE_MEETINGSHOMEDT_VIEWSTATE) IsNot Nothing Then
                grdMeetings.DataSource = CType(ViewState(ATTRIBUTE_MEETINGSHOMEDT_VIEWSTATE), DataTable)
            End If
        End Sub

        Protected Sub grdMeetings_DetailTableDataBind(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridDetailTableDataBindEventArgs) Handles grdMeetings.DetailTableDataBind

            Dim sSQL As String, dtDeatil As Data.DataTable
            Dim dataItem As Telerik.Web.UI.GridDataItem = CType(e.DetailTableView.ParentItem, Telerik.Web.UI.GridDataItem)
            Dim ss As Boolean = e.DetailTableView.ParentItem.Expanded
            Dim Id As Integer = dataItem.GetDataKeyValue("ID")
            'Anil B for issue 15384 on 08/04/2013
            'Remove condition for meeting category
            'Suraj Issue 14829 4/29/13, remove Coversion of date,remove "Not Mention" if the AvailableUntil date is null we are showing blank date for maintain the uniformability
            sSQL = " SELECT p.ID ProductID,p.WebName," & _
                      "m.StartDate,m.EndDate,m.place Location,isnull(m.MeetingID,0) as ID,'VerboseDescription'=case when m.VerboseDescription='' or m.VerboseDescription is null then 'No Description'  else m.VerboseDescription end,'AvailableUntil'=CASE WHEN AvailableUntil IS NOT NULL THEN CAST(AvailableUntil AS VARCHAR(11)) ELSE AvailableUntil END,'Smalldesc'=case when VerboseDescription='' or VerboseDescription is null then 'No Description'  else substring(VerboseDescription,0,35) end, 'AvailableUntil'=CASE WHEN AvailableUntil IS NOT NULL THEN CAST(AvailableUntil AS VARCHAR(11)) ELSE AvailableUntil END,  " & _
                       " m.StartDate,m.EndDate,pc.WebName WebCategoryName " & _
                       " FROM " & sDB & "..vwProducts p INNER JOIN " & _
                       sDB & "..vwMeetings m ON p.ID=m.ProductID " & _
                       " INNER JOIN " & sDB & "..vwProductCategories pc " & _
                       " ON pc.ID=p.CategoryID " & _
                       " WHERE  pc.WebEnabled=1 AND p.WebEnabled=1 AND  m.ParentMeetingID = " + Id.ToString()

            If Not ShowMeetingsLinkToClass Then
                sSQL &= (" AND  ISNULL(p.ClassID ,-1) <=0  ")
            End If
            sSQL &= " ORDER BY m.StartDate DESC, m.EndDate DESC"


            'sSQL = "select p.ID,p.Name , p.WebName,p.WebEnabled , p.ProductCategory ,p.Description,m.Place,m.StartDate,m.EndDate,m.ParentMeetingID from " & Database & "..vwProducts p," & Database & "..vwMeetings m where p.ID=m.ProductID and  m.ParentMeetingID = " & Id
            dtDeatil = DataAction.GetDataTable(sSQL)
            Dim dcol As DataColumn = New DataColumn()
            dcol.Caption = "Price"
            dcol.ColumnName = "Price"
            dtDeatil.Columns.Add(dcol)
            If dtDeatil.Rows.Count > 0 Then
                Dim oPrice As New Aptify.Applications.OrderEntry.IProductPrice.PriceInfo
                For Each rw As DataRow In dtDeatil.Rows
                    oPrice = Me.ShoppingCart1.GetUserProductPrice(CLng(rw("ProductID")))
                    If oPrice.Price = 0 Then
                        rw("Price") = "Free"
                    Else
                        rw("Price") = Format(oPrice.Price, User1.PreferredCurrencyFormat)
                    End If

                Next
            End If

            Dim dcolUrl As DataColumn = New DataColumn()
            dcolUrl.Caption = "MeetingUrl"
            dcolUrl.ColumnName = "MeetingUrl"

            dtDeatil.Columns.Add(dcolUrl)
            If dtDeatil.Rows.Count > 0 Then

                For Each rw As DataRow In dtDeatil.Rows
                    rw("MeetingUrl") = MeetingPage + "?ID=" + rw("ProductID").ToString
                Next
            End If
            Dim dcolRating As DataColumn = New DataColumn()
            dcolRating.Caption = "MeetingRate"
            dcolRating.ColumnName = "MeetingRate"

            dtDeatil.Columns.Add(dcolRating)
            If dtDeatil.Rows.Count > 0 Then

                For Each rw As DataRow In dtDeatil.Rows
                    Dim itemratinginputobj As New ItemRatingInput()

                    itemratinginputobj.ItemRatingURI = "MeetingEvaluation/EntityRecord/Products/" + rw("ProductID").ToString
                    ' itemratinginputobj.PersonID = User1.PersonID
                    itemratinginputobj.ItemRatingTypeURI = ""
                    itemratinginputobj.RatedItemURI = ""
                    ' Dim a As Integer = RadmeetingRate.Value

                    Dim ItemRatingServiceInformationobj As New ItemRatingServiceInformation()
                    ItemRatingServiceInformationobj = GetItemRating(itemratinginputobj)
                    If (ItemRatingServiceInformationobj.ItemRatingDetails.PersonEntry Is Nothing) Then
                        If Not ItemRatingServiceInformationobj.ItemRatingDetails.RecordedValueAverage Is Nothing Then
                            rw("MeetingRate") = Math.Round(Decimal.Parse(ItemRatingServiceInformationobj.ItemRatingDetails.RecordedValueAverage), 1).ToString()
                            'RadRatingTotal.Value = ItemRatingServiceInformationobj.ItemRatingDetails.RecordedValueAverage
                            'totalrating.Text = "(Total: " + Math.Round(Decimal.Parse(ItemRatingServiceInformationobj.ItemRatingDetails.RecordedValueAverage), 1).ToString() + ")"
                        Else
                            'RadRatingTotal.Value = 0
                            'totalrating.Text = "(Total: " + "0" + ")"
                            rw("MeetingRate") = 0
                        End If


                    Else
                        If Not ItemRatingServiceInformationobj.ItemRatingDetails.RecordedValueAverage Is Nothing Then
                            rw("MeetingRate") = Math.Round(Decimal.Parse(ItemRatingServiceInformationobj.ItemRatingDetails.RecordedValueAverage), 1).ToString()
                            'RadRatingTotal.Value = ItemRatingServiceInformationobj.ItemRatingDetails.RecordedValueAverage
                            'totalrating.Text = "(Total: " + Math.Round(Decimal.Parse(ItemRatingServiceInformationobj.ItemRatingDetails.RecordedValueAverage), 1).ToString() + ")"
                        Else
                            rw("MeetingRate") = 0
                            'RadRatingTotal.Value = 0
                            'totalrating.Text = "(Total: " + "0" + ")"
                        End If
                    End If

                Next
            End If
            e.DetailTableView.DataSource = dtDeatil
            If e.DetailTableView.Items.Count > 0 Then

                e.DetailTableView.PagerStyle.AlwaysVisible = True
            Else
                e.DetailTableView.PagerStyle.AlwaysVisible = False
            End If


        End Sub
        Public Function GetItemRating(ByVal inputObjArgs As ItemRatingInput) As ItemRatingServiceInformation
            Dim objItemRatingServiceInfo As New ItemRatingServiceInformation()

            Try
                'Dim uc As UserCredentials = DALHelper.Helper.UserContext()
                Dim objApplication As New AptifyApplication
                Dim objRatingInfo As New ItemRatingInformation()

                Dim objRatings As New Ratings(objApplication)


                'check for required input arguments and raise exception if it is not available
                If (inputObjArgs.ItemRatingURI Is Nothing) AndAlso (inputObjArgs.ItemRatingTypeURI Is Nothing OrElse inputObjArgs.RatedItemURI Is Nothing) Then
                    'throw new Exception("The required input arguments are not provided.");
                    'objErrorInfo.IsError = True
                    'objErrorInfo.Message = "The required input arguments are not provided."
                    'objErrorInfo.Stack = ""
                    'objErrorInfo.InnerException = ""
                    'Throw New WebFaultException(Of ErrorInfo)(objErrorInfo, System.Net.HttpStatusCode.BadRequest)
                End If

                'Call various overload methods based on the input arguments provided
                'If inputObjArgs.ItemRatingTypeURI IsNot Nothing AndAlso inputObjArgs.RatedItemURI IsNot Nothing AndAlso inputObjArgs.PersonID > 0 Then
                '    objRatingInfo = objRatings.GetItemRating(inputObjArgs.ItemRatingTypeURI, inputObjArgs.RatedItemURI, inputObjArgs.PersonID)
                'ElseIf inputObjArgs.ItemRatingTypeURI IsNot Nothing AndAlso inputObjArgs.RatedItemURI IsNot Nothing Then
                '    objRatingInfo = objRatings.GetItemRating(inputObjArgs.ItemRatingTypeURI, inputObjArgs.RatedItemURI)
                If inputObjArgs.ItemRatingURI IsNot Nothing AndAlso inputObjArgs.PersonID >= 0 Then
                    objRatingInfo = objRatings.GetItemRating(inputObjArgs.ItemRatingURI, inputObjArgs.PersonID)
                Else
                    objRatingInfo = objRatings.GetItemRating(inputObjArgs.ItemRatingURI)
                End If

                objItemRatingServiceInfo.ItemRatingDetails = objRatingInfo
                'objItemRatingServiceInfo.ErrorInformation = objErrorInfo
                'Catch wEx As WebFaultException
                ' Throw New WebFaultException(Of ErrorInfo)(wEx.Detail, System.Net.HttpStatusCode.BadRequest)
            Catch ex As Exception

            End Try

            Return objItemRatingServiceInfo

        End Function


        Protected Sub grdMeetings_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdMeetings.ItemDataBound
            Try
                'Suraj Issue 14829  4/29/13, for Parent  grid show the time or no time if time=Midnight
                If TypeOf e.Item Is GridDataItem AndAlso e.Item.OwnerTableView.Name <> "ChildGrid" Then
                    Dim dateColumnsParent As New List(Of String)
                    'Add datecolumn uniqueName in list for Date format
                    dateColumnsParent.Add("GridDateTimeColumnRegisteredDate")
                    dateColumnsParent.Add("GridDateTimeColumnStartDate")
                    dateColumnsParent.Add("GridDateTimeColumnEndDate")
                    CommonMethods.FormatedDateOnGrid(dateColumnsParent, e.Item)
                End If
                'Suraj Issue 14829  4/29/13, for child grid  show the time or no time if time=Midnight
                If TypeOf e.Item Is GridDataItem AndAlso e.Item.OwnerTableView.Name = "ChildGrid" Then
                    Dim dateColumnschild As New List(Of String)
                    'Add datecolumn uniqueName in list for Date format
                    dateColumnschild.Add("GridDateTimeColumnRegisteredDateDetails")
                    dateColumnschild.Add("GridDateTimeColumnStartDateDetails")
                    dateColumnschild.Add("GridDateTimeColumnEndDateDetails")
                    CommonMethods.FormatedDateOnGrid(dateColumnschild, e.Item)
                End If

                'Suraj Issue 14457 3/12/13 ,we provide a tool tip for DatePopupButton as well as the GridDateTimeColumnStartDate textbox   
                If TypeOf e.Item Is GridFilteringItem Then
                    Dim filterItem As GridFilteringItem = DirectCast(e.Item, GridFilteringItem)
                    Dim gridDateTimeColumnStartDate As RadDatePicker = DirectCast(filterItem("GridDateTimeColumnStartDate").Controls(0), RadDatePicker)
                    gridDateTimeColumnStartDate.ToolTip = "Enter a filter date"
                    gridDateTimeColumnStartDate.DatePopupButton.ToolTip = "Select a filter date"
                    Dim gridDateTimeColumnRegisteredDate As RadDatePicker = DirectCast(filterItem("GridDateTimeColumnRegisteredDate").Controls(0), RadDatePicker)
                    gridDateTimeColumnRegisteredDate.ToolTip = "Enter a filter date"
                    gridDateTimeColumnRegisteredDate.DatePopupButton.ToolTip = "Select a filter date"

                End If
                'Added By sandeep For issue 15406
                Dim radmeetingrating As New RadRating
                Dim RadMettingratingDetails As New RadRating
                Dim vlblRatingpending As Label
                Dim vlblRatingpendingDetails As Label
                radmeetingrating = TryCast(e.Item.FindControl("radRateID"), RadRating)
                RadMettingratingDetails = TryCast(e.Item.FindControl("radRateIDMain"), RadRating)
                vlblRatingpending = TryCast(e.Item.FindControl("lblpendingrating"), Label)
                vlblRatingpendingDetails = TryCast(e.Item.FindControl("lblRatingDetails"), Label)
                If radmeetingrating IsNot Nothing Then
                    radmeetingrating.Precision = RatingPrecision.Exact
                    radmeetingrating.ItemCount = 5
                    If radmeetingrating.Value > 0 Then
                        vlblRatingpending.Visible = False
                    End If
                End If

                If RadMettingratingDetails IsNot Nothing Then
                    If RadMettingratingDetails.Value > 0 Then
                        vlblRatingpendingDetails.Visible = False
                    End If
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        'Suraj Issue 14457  3/5/13 ,if the grid load first time By default the sorting will be Ascending for Parent grid for column meeting and for child grid Ascending order for start date column 
        ' Suraj Issue 16115  5/6/13 ,The Schedule Details grid on meeting.aspx  The grid's sorting should default to Start Date in ascending order..
        Private Sub AddExpression()
            Dim MeetingSort As New GridSortExpression
            Dim MeetingSortForClild As New GridSortExpression
            MeetingSort.FieldName = "WebName"
            MeetingSort.SetSortOrder("Ascending")
            MeetingSortForClild.FieldName = "StartDate"
            MeetingSortForClild.SetSortOrder("Ascending")
            grdMeetings.MasterTableView.SortExpressions.AddSortExpression(MeetingSort)
            grdMeetings.MasterTableView.DetailTables.Item(0).SortExpressions.AddSortExpression(MeetingSortForClild)
        End Sub
    End Class
    Public Class ItemRatingInput
        Private m_ItemRatingURI As String
        Private m_ItemRatingTypeURI As String
        Private m_RatedItemURI As String
        Private m_RatingValue As Decimal
        Private m_PersonID As Integer
        Public Property ItemRatingURI As String
            Get
                Return m_ItemRatingURI
            End Get
            Set(ByVal value As String)
                m_ItemRatingURI = value
            End Set
        End Property
        Public Property ItemRatingTypeURI As String
            Get
                Return m_ItemRatingTypeURI
            End Get
            Set(ByVal value As String)
                m_ItemRatingTypeURI = value
            End Set
        End Property
        Public Property RatedItemURI As String
            Get
                Return m_RatedItemURI
            End Get
            Set(ByVal value As String)
                m_RatedItemURI = value
            End Set
        End Property
        Public Property RatingValue As Decimal
            Get
                Return m_RatingValue
            End Get
            Set(ByVal value As Decimal)
                m_RatingValue = value
            End Set
        End Property
        Public Property PersonID As Integer
            Get
                Return m_PersonID
            End Get
            Set(ByVal value As Integer)
                m_PersonID = value
            End Set
        End Property

    End Class
    Public Class ItemRatingServiceInformation
        Private m_ItemRatingDetails As ItemRatingInformation
        Public Property ItemRatingDetails As ItemRatingInformation
            Get
                Return m_ItemRatingDetails
            End Get
            Set(ByVal value As ItemRatingInformation)
                m_ItemRatingDetails = value
            End Set
        End Property
    End Class
End Namespace
