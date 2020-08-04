'Aptify e-Business 5.5.1, July 2013
Option Explicit On

Imports System.Data
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Applications.ProductSetup
Imports System.IO
Imports Telerik.Web.UI

Namespace Aptify.Framework.Web.eBusiness.Meetings
    Partial Class MeetingEvents
        Inherits eBusiness.BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "MeetingEvents"
        Protected Const ATTRIBUTE_PARENT_MEETING_PAGE As String = "ParentMeetingPage"
        Protected Const ATTRIBUTE_MEETING_EVENTS_PAGE As String = "MeetingEventsPage"

        Protected Const ATTRIBUTE_PERSON_IMAGE_URL As String = "PersonImageURL"
        Protected Const ATTRIBUTE_PERSON_BLANK_IMG As String = "BlankImage"
        Protected Const ATTRIBUTE_PEOPLEYOUMAYKNOW_TITLE_IMAGE_URL As String = "PeopleYouMayKnowTitleImage"
        Protected Const ATTRIBUTE_PERSON_LISTING_PAGE As String = "PersonListingPage"
        Protected Const ATTRIBUTE_COMPANY_LISTING_PAGE As String = "CompanyListingPage"
        Protected Const ATTRIBUTE_MEETING_DIRECTION_URL As String = "MeetingDirectionURL"
        Protected Const ATTRIBUTE_LOGIN_PAGE As String = "LoginPage"
        Protected DefaultPageSize As Integer
        Protected m_sblankImage As String
        Dim m_lEntityID As Long
        Dim m_lRecordID As String

        Enum PeopleYouMayKnowColumn
            Person
            Company
            PersonDirExclude
            CompanyDirExclude
        End Enum

        Public Overridable Property ParentMeetingPage() As String
            Get
                If Not ViewState(ATTRIBUTE_PARENT_MEETING_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_PARENT_MEETING_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_PARENT_MEETING_PAGE) = Me.FixLinkForVirtualPath(value)
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
        Public Overridable Property MeetingEventsPage() As String
            Get
                If Not ViewState(ATTRIBUTE_MEETING_EVENTS_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_MEETING_EVENTS_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_MEETING_EVENTS_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        Public Overridable Property MeetingDirectionURL() As String
            Get
                If Not ViewState(ATTRIBUTE_MEETING_DIRECTION_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_MEETING_DIRECTION_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_MEETING_DIRECTION_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        Public Overridable Property PeopleYouMayKnowTitleImage() As String
            Get
                If Not ViewState(ATTRIBUTE_PEOPLEYOUMAYKNOW_TITLE_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_PEOPLEYOUMAYKNOW_TITLE_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_PEOPLEYOUMAYKNOW_TITLE_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        Public Overridable Property PersonListingPage() As String
            Get
                If Not ViewState(ATTRIBUTE_PERSON_LISTING_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_PERSON_LISTING_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_PERSON_LISTING_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' Company page url
        ''' </summary>
        Public Overridable Property CompanyListingPage() As String
            Get
                If Not ViewState(ATTRIBUTE_COMPANY_LISTING_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_COMPANY_LISTING_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_COMPANY_LISTING_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        Public Property PersonImageURL() As String
            Get
                If ViewState.Item("PersonImageURL") IsNot Nothing Then
                    Return ViewState.Item("PersonImageURL").ToString()
                Else
                    Return ""
                End If

            End Get
            Set(ByVal value As String)
                ViewState.Item("PersonImageURL") = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        Public Property BlankImage() As String
            Get
                If ViewState.Item("BlankImage") IsNot Nothing Then
                    Return ViewState.Item("BlankImage").ToString()
                Else
                    Return ""
                End If

            End Get
            Set(ByVal value As String)
                ViewState.Item("BlankImage") = value
            End Set
        End Property

        Protected Overrides Sub SetProperties()
            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            If String.IsNullOrEmpty(Me.QueryStringRecordIDParameter) Then Me.QueryStringRecordIDParameter = "ID"
            If String.IsNullOrEmpty(Me.IsQueryStringEncrypted) Then Me.IsQueryStringEncrypted = False
            If String.IsNullOrEmpty(Me.SetControlRecordIDFromQueryString) Then Me.SetControlRecordIDFromQueryString = True
            MyBase.SetProperties()

            If String.IsNullOrEmpty(ParentMeetingPage) Then
                ParentMeetingPage = Me.GetLinkValueFromXML(ATTRIBUTE_PARENT_MEETING_PAGE)
            End If

            If String.IsNullOrEmpty(MeetingEventsPage) Then
                MeetingEventsPage = Me.GetLinkValueFromXML(ATTRIBUTE_MEETING_EVENTS_PAGE)
            End If

            If String.IsNullOrEmpty(PersonImageURL) Then
                PersonImageURL = Me.GetLinkValueFromXML(ATTRIBUTE_PERSON_IMAGE_URL)
            End If

            If String.IsNullOrEmpty(MeetingDirectionURL) Then
                MeetingDirectionURL = Me.GetLinkValueFromXML(ATTRIBUTE_MEETING_DIRECTION_URL)
            End If

            If String.IsNullOrEmpty(BlankImage) Then
                BlankImage = Me.GetLinkValueFromXML(ATTRIBUTE_PERSON_BLANK_IMG)
            End If

            If String.IsNullOrEmpty(PeopleYouMayKnowTitleImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                PeopleYouMayKnowTitleImage = Me.GetLinkValueFromXML(ATTRIBUTE_PEOPLEYOUMAYKNOW_TITLE_IMAGE_URL)
            End If
            If String.IsNullOrEmpty(PersonListingPage) Then
                PersonListingPage = Me.GetLinkValueFromXML(ATTRIBUTE_PERSON_LISTING_PAGE)

            End If
            If String.IsNullOrEmpty(CompanyListingPage) Then
                CompanyListingPage = Me.GetLinkValueFromXML(ATTRIBUTE_COMPANY_LISTING_PAGE)

            End If

            If ConfigurationManager.AppSettings("PageSize") Is Nothing Then
                DefaultPageSize = 10
            Else
                DefaultPageSize = CInt(ConfigurationManager.AppSettings("PageSize").ToString)
            End If
        End Sub

        Private Sub LoadTravel()
            Dim dt As DataTable, sSQL As String, sDB As String, lID As Long
            Try
                lID = Me.ControlRecordID
                sDB = AptifyApplication.GetEntityBaseDatabase("Meetings")
                sSQL = "SELECT mh.* FROM " & sDB & _
                       "..vwMeetingHotels mh INNER JOIN " & _
                       sDB & "..vwMeetings m ON mh.MeetingID=m.ID " & _
                       "WHERE " & _
                       " m.ProductID=" & lID & " ORDER BY mh.HotelID_Name"
                dt = DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                    Me.repTravelDiscounts.DataSource = dt
                    Me.repTravelDiscounts.DataBind()
                    lblTravel.Visible = False
                    repTravelDiscounts.Visible = True
                Else
                    lblTravel.Visible = True
                    lblTravel.Text = "No hotels have been associated with this event."
                    repTravelDiscounts.Visible = False
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub repTravelDiscounts_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles repTravelDiscounts.ItemDataBound
            If (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then

                Dim lbl As Label
                lbl = CType(e.Item.FindControl("lblHotelName"), Label)
                lbl.Text = DataBinder.Eval(e.Item.DataItem, "HotelID_Name").ToString

                lbl = CType(e.Item.FindControl("lblGroupOffer"), Label)
                lbl.Text = DataBinder.Eval(e.Item.DataItem, "SpecialOffering").ToString

                lbl = CType(e.Item.FindControl("lblStartDate"), Label)
                lbl.Text = DataBinder.Eval(e.Item.DataItem, "OfferStartDate", "{0:MM/dd/yyyy}")

                lbl = CType(e.Item.FindControl("lblEndDate"), Label)
                lbl.Text = DataBinder.Eval(e.Item.DataItem, "OfferEndDate", "{0:MM/dd/yyyy}")

            End If

        End Sub

        Private Sub LoadMeeting()

            Dim dt As DataTable, sSQL As String
            Try
                sSQL = "SELECT m.*,ISNULL(p.ParentID,-1) ParentID, par.WebName ParentWebName, CASE ISNULL(m.State,'') WHEN '' THEN " & _
                       "m.City + ', ' + m.Country ELSE m.City + ', ' + " & _
                       "m.State + ' ' + m.Country END Location, p.WebName, " & _
                       "p.WebDescription,p.WebLongDescription,p.WebImage, " & _
                       "p.WebEnabled, p.DateAvailable, p.AvailableUntil, " & _
                       "p.RequireInventory FROM " & _
                       AptifyApplication.GetEntityBaseDatabase("Products") & _
                       "..vwProducts p INNER JOIN " & _
                       AptifyApplication.GetEntityBaseDatabase("Meetings") & _
                       "..vwMeetings m ON p.ID=m.ProductID LEFT OUTER JOIN " & _
                       AptifyApplication.GetEntityBaseDatabase("Products") & _
                       "..vwProducts par on p.ParentID=par.ID  " & _
                       " WHERE p.WebEnabled=1 AND m.ProductID=" & _
                      Me.ControlRecordID 'must be parameterized
                dt = DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)

                Dim dcol As DataColumn = New DataColumn()
                dcol.Caption = "Price"
                dcol.ColumnName = "Price"
                dt.Columns.Add(dcol)
                If dt.Rows.Count > 0 Then
                    Dim oPrice As New Aptify.Applications.OrderEntry.IProductPrice.PriceInfo
                    For Each rw As DataRow In dt.Rows
                        oPrice = Me.ShoppingCart1.GetUserProductPrice(CLng(rw("ProductID")))
                        rw("Price") = Format(oPrice.Price, User1.PreferredCurrencyFormat)
                    Next
                End If
                If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                    With dt.Rows(0)

                        'Member Savings:


                        ViewState("MeetingStartDate") = .Item("StartDate")
                        ViewState("MeetingEndDate") = .Item("EndDate")


                        If CLng(.Item("ParentID")) > 0 Then
                            lblParent.Text = CStr(.Item("ParentWebName"))
                            lnkParent.NavigateUrl = ParentMeetingPage & "?View=Schedule&ID=" & CStr(.Item("ParentID"))
                            trSessionParent.Visible = True
                            lblDate.Text = CDate(.Item("StartDate")).ToLongDateString & " - " & _
                                            CDate(.Item("StartDate")).ToShortTimeString & " to " & _
                                            CDate(.Item("EndDate")).ToShortTimeString
                        Else
                            trSessionParent.Visible = False
                            lblDate.Text = CDate(.Item("StartDate")).ToLongDateString & " to " & _
                                            CDate(.Item("EndDate")).ToLongDateString
                        End If

                        lblTitle.Text = .Item("WebName").ToString.Trim
                        lblDescription.Text = .Item("WebDescription").ToString.Trim
                        lblVenue.Text = .Item("Place").ToString.Trim
                        linkVenueDirection.NavigateUrl = MeetingDirectionURL + "?ID=" + CStr(.Item("ProductID"))
                        If .Item("Price") = 0 Then
                            lblTotalPrice.Text = "Free"
                        Else
                            lblTotalPrice.Text = .Item("Price").ToString.Trim
                        End If

                        '  lblTotalPrice.Text = Format(.Item("Price").ToString.Trim, User1.PreferredCurrencyFormat)

                        'lblVenue.Text = .Item("Location").ToString.Trim
                        ' lblDescription.Text = .Item("WebLongDescription").ToString.Trim

                        ' SetupView(dt.Rows(0))
                    End With
                Else
                    lblTitle.Text = "Event Not Available!"
                    tblDetail.Visible = False
                    btnRegister.Visible = False
                    btnBack.Visible = False

                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub LoadSessions()
            Dim dt As DataTable, sSQL As String, sDB As String, lID As Long
            Session("MyDT") = Nothing
            Try
                lID = Me.ControlRecordID
                sDB = AptifyApplication.GetEntityBaseDatabase("Meetings")
                sSQL = "exec spGetRelatedMeetingSessions @ProductID=" + lID.ToString
                dt = DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                Dim dcol As DataColumn = New DataColumn()
                dcol.Caption = "Price"
                dcol.ColumnName = "Price"
                dt.Columns.Add(dcol)
                If dt.Rows.Count > 0 Then
                    Dim oPrice As New Aptify.Applications.OrderEntry.IProductPrice.PriceInfo
                    For Each rw As DataRow In dt.Rows
                        oPrice = Me.ShoppingCart1.GetUserProductPrice(CLng(rw("ProductID")))
                        If oPrice.Price = 0 Then
                            rw("Price") = "Free "
                        Else
                            rw("Price") = Format(oPrice.Price, User1.PreferredCurrencyFormat)
                        End If

                    Next
                End If
                DirectCast(grdRelatedMeetingSessions.Columns(1), GridHyperLinkColumn).DataNavigateUrlFormatString = MeetingEventsPage & "?ID={0:F0}"
                Session("MyDT") = dt
                If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                    Me.grdRelatedMeetingSessions.PageSize = DefaultPageSize
                    Me.grdRelatedMeetingSessions.DataSource = Session("MyDT")
                    Me.grdRelatedMeetingSessions.DataBind()
                    lblRelatedMeetingSessions.Visible = False
                    grdRelatedMeetingSessions.Visible = True
                Else
                    lblRelatedMeetingSessions.Visible = True
                    lblRelatedMeetingSessions.Text = "No Session have been associated with this event."
                    grdRelatedMeetingSessions.Visible = False
                End If
                With grdRelatedMeetingSessions
                    If (.PageCount > 1) Then
                        .PagerStyle.Visible = True
                    Else
                        .PagerStyle.Visible = False
                    End If

                    If .Items.Count > 0 Then
                        For i As Integer = 0 To .Items.Count
                            Dim strTime As String = CDate(dt.Rows(i)("StartDate")).TimeOfDay.ToString
                            If strTime = "00:00:00" Then
                                .Items(i).Cells(1).Text = FormatDateTime(CDate(dt.Rows(i)("StartDate")), DateFormat.ShortDate)
                            End If
                            strTime = CDate(dt.Rows(i)("EndDate")).TimeOfDay.ToString
                            If strTime = "00:00:00" Then
                                .Items(i).Cells(2).Text = FormatDateTime(CDate(dt.Rows(i)("EndDate")), DateFormat.ShortDate)
                            End If
                        Next
                    End If
                End With
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub LoadSpeakers()
            Dim dt As DataTable, sSQL As String, sDB As String, lID As Long
            Session("DTSPEAKERS") = Nothing
            Try
                lID = Me.ControlRecordID
                sDB = AptifyApplication.GetEntityBaseDatabase("Meetings")
                sSQL = "SELECT m.StartDate,m.EndDate,ms.Title,ms.Type," & _
                       "p.LastName,p.FirstName FROM " & sDB & _
                       "..vwMeetingSpeakers ms INNER JOIN " & sDB & _
                       "..vwPersons p ON ms.SpeakerID=p.ID INNER JOIN " & _
                       sDB & "..vwMeetings m ON ms.MeetingID=m.ID " & _
                       "WHERE ms.Status IN ('Accepted','Completed') AND "
                'HP Issue#8516:  when at the top level meeting extract all speakers for all child sessions, 
                '                otherwise when at a session only extract speakers for that session.
                'If String.IsNullOrEmpty(lblParent.Text) Then
                '    sSQL &= " dbo.fnProductLevelsBelow(m.ProductID," & lID & ")>=0 "
                'Else
                sSQL &= "m.ProductID =" & lID & " "
                ' End If
                sSQL &= "ORDER BY m.MeetingTitle,p.LastName,p.FirstName"

                dt = DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                Session("DTSPEAKERS") = dt
                If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                    Me.grdSpeakers.PageSize = DefaultPageSize
                    Me.grdSpeakers.DataSource = Session("DTSPEAKERS")
                    Me.grdSpeakers.DataBind()
                    lblSpeakers.Visible = False
                    grdSpeakers.Visible = True
                Else
                    lblSpeakers.Visible = True
                    lblSpeakers.Text = "No speakers have been associated with this event."
                    grdSpeakers.Visible = False
                End If
                With grdSpeakers
                    If (.PageCount > 1) Then
                        .PagerStyle.Visible = True
                    Else
                        .PagerStyle.Visible = False
                    End If
                    If .Items.Count > 0 Then
                        For i As Integer = 0 To .Items.Count
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

        Private Sub LoadPeopleYouMayKnow()
            Dim dt As DataTable, sSQL As String, sDB As String, lID As Long
            Try
                lID = Me.ControlRecordID
                sDB = AptifyApplication.GetEntityBaseDatabase("Meetings")
                sSQL = "Exec " & sDB & ".dbo.spPeopleYouMayKnow " & User1.PersonID & ", " & lID & " "
                dt = DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then

                    repPeopleYouMayKnow.DataSource = dt
                    repPeopleYouMayKnow.DataBind()
                    lblPeopleYouMayKnow.Visible = False
                    repPeopleYouMayKnow.Visible = True
                Else
                    lblPeopleYouMayKnow.Visible = True
                    lblPeopleYouMayKnow.Text = "People you know have not been associated with this event."
                    repPeopleYouMayKnow.Visible = False
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub LoadProfilePicture(ByVal lPersonID As Long, ByVal img As Image)

            Dim sSQL As String = ""
            Dim dt As DataTable
            m_lRecordID = User1.PersonID.ToString
            sSQL = "Select Photo from vwPersons Where ID = " & lPersonID
            dt = DataAction.GetDataTable(sSQL)
            If Not dt Is Nothing Then
                If Not IsDBNull(dt.Rows(0)("Photo")) Then

                    Dim ImagePath As String = Server.MapPath(PersonImageURL) & lPersonID.ToString & "_" & m_lRecordID & ".jpg"

                    Dim ImageData(), newImgData() As Byte

                    ImageData = DirectCast(dt.Rows(0)("Photo"), [Byte]())
                    If ImageData.Length > 0 Then
                        Dim client As New System.Net.WebClient
                        client.UploadData(ImagePath, "POST", ImageData)
                        img.ImageUrl = PersonImageURL & lPersonID.ToString & "_" & m_lRecordID & ".jpg"
                        newImgData = ConvertImagetoByte(ImagePath)
                    Else
                        img.ImageUrl = PersonImageURL & BlankImage

                    End If
                Else
                    img.ImageUrl = PersonImageURL & BlankImage

                End If

            Else
                img.ImageUrl = PersonImageURL & BlankImage

            End If
           
        End Sub

        Private Function ConvertImagetoByte(ByVal spath As String) As Byte()
            Try
                Dim sFile As String
                sFile = spath
                Dim fInfo As New FileInfo(sFile)
                Dim len As Long = fInfo.Length
                Dim imgData() As Byte
                Using Stream As New FileStream(sFile, FileMode.Open)
                    imgData = New Byte(Convert.ToInt32(len - 1)) {}
                    Stream.Read(imgData, 0, CInt(len))
                End Using
                Return imgData
            Catch ex As Exception

            End Try

        End Function
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            SetProperties()
            If Not IsPostBack Then
                Me.SetControlRecordIDFromParam()
                If User1.UserID > 0 Then
                    If Me.ControlRecordID > 0 Then
                        LoadMeeting()
                        LoadSessions()
                        LoadSpeakers()
                        LoadPeopleYouMayKnow()
                        LoadTravel()
                    ElseIf Request.QueryString("ID") IsNot Nothing Then
                        ' only do this if query string was provided, otherwise we are in design time
                        Throw New Exception("Security Validation Error - Invalid ID parameter")
                    End If
                Else
                    'Suraj Issue 15370,7/31/13 revert back changes application variale to session variable
                    Session("ReturnToPage") = Request.RawUrl
                    ' Suraj S Issue 15370, 8/1/13 here we are getting the ReturnToPageURL in "URL" QueryString and passing on login page. 
                    Response.Redirect(LoginPage + "?ReturnURL=" & System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(Request.RawUrl)))
                End If
            End If
        End Sub

        'Protected Sub grdPeopleYouMayKnow_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdPeopleYouMayKnow.ItemDataBound
        '    ' Changes made for to allow encrypting and decrypting the URL.

        '    Try
        '        Dim type As ListItemType = e.Item.ItemType
        '        If (e.Item.ItemType = ListItemType.Item Or _
        '        e.Item.ItemType = ListItemType.AlternatingItem) Then
        '            If Not CBool(e.Item.Cells(PeopleYouMayKnowColumn.PersonDirExclude).Text) Then
        '                Dim lnkPerson As HyperLink
        '                Dim tempURL As String
        '                Dim index As Integer
        '                Dim sValue As String
        '                Dim separator As String()

        '                lnkPerson = CType(e.Item.Cells(PeopleYouMayKnowColumn.Person).Controls(0), HyperLink)
        '                tempURL = lnkPerson.NavigateUrl
        '                index = tempURL.IndexOf("=")
        '                sValue = tempURL.Substring(index + 1)
        '                separator = lnkPerson.NavigateUrl.Split(CChar("="))
        '                lnkPerson.NavigateUrl = separator(0)
        '                lnkPerson.NavigateUrl = lnkPerson.NavigateUrl & "="
        '                lnkPerson.NavigateUrl = lnkPerson.NavigateUrl & System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(sValue))
        '            Else
        '                'remove Hyperlink if exclude from Directory
        '                Dim lnk As HyperLink
        '                lnk = CType(e.Item.Cells(PeopleYouMayKnowColumn.Person).Controls(0), HyperLink)
        '                lnk.NavigateUrl = ""
        '                lnk.ForeColor = Drawing.Color.Black
        '                lnk.Font.Underline = False
        '            End If
        '            If Not CBool(e.Item.Cells(PeopleYouMayKnowColumn.CompanyDirExclude).Text) Then
        '                Dim lnkCompanies As HyperLink
        '                Dim tempURL As String
        '                Dim index As Integer
        '                Dim sValue As String
        '                Dim separator As String()

        '                lnkCompanies = CType(e.Item.Cells(PeopleYouMayKnowColumn.Company).Controls(0), HyperLink)
        '                tempURL = lnkCompanies.NavigateUrl
        '                index = tempURL.IndexOf("=")
        '                sValue = tempURL.Substring(index + 1)
        '                separator = lnkCompanies.NavigateUrl.Split(CChar("="))
        '                lnkCompanies.NavigateUrl = separator(0)
        '                lnkCompanies.NavigateUrl = lnkCompanies.NavigateUrl & "="
        '                lnkCompanies.NavigateUrl = lnkCompanies.NavigateUrl & System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(sValue))
        '            Else
        '                'remove Hyperlink if exclude from Directory
        '                Dim lnk As HyperLink
        '                lnk = CType(e.Item.Cells(PeopleYouMayKnowColumn.Company).Controls(0), HyperLink)
        '                lnk.NavigateUrl = ""
        '                lnk.ForeColor = Drawing.Color.Black
        '                lnk.Font.Underline = False
        '            End If
        '        End If
        '    Catch ex As Exception
        '        Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        '    End Try


        'End Sub

      

        'Protected Sub grdRelatedMeetingSessions_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles grdRelatedMeetingSessions.PageIndexChanged

        '    Try
        '        grdRelatedMeetingSessions.CurrentPageIndex = e.NewPageIndex
        '        grdRelatedMeetingSessions.DataSource = Session("MyDT")
        '        grdRelatedMeetingSessions.DataBind()
        '    Catch ex As Exception
        '        Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        '    End Try
        'End Sub

        Protected Sub repPeopleYouMayKnow_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles repPeopleYouMayKnow.ItemDataBound

            Try
                If (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then

                    Dim img As Image
                    Dim lnk As HyperLink
                    Dim chbox As CheckBox
                    img = CType(e.Item.FindControl("imgProfile"), Image)

                    LoadProfilePicture(CType(DataBinder.Eval(e.Item.DataItem, "RelatedPersonID").ToString, Long), img)


                    lnk = CType(e.Item.FindControl("lnkName"), HyperLink)
                    lnk.Text = DataBinder.Eval(e.Item.DataItem, "Name").ToString
                    lnk.NavigateUrl = String.Format(Me.PersonListingPage & "?ID={0}", DataBinder.Eval(e.Item.DataItem, "RelatedPersonID").ToString)


                    lnk = CType(e.Item.FindControl("RelatedPersonCompanyName"), HyperLink)
                    lnk.Text = DataBinder.Eval(e.Item.DataItem, "RelatedPersonCompanyName").ToString
                    lnk.NavigateUrl = String.Format(Me.CompanyListingPage & "?ID={0}", DataBinder.Eval(e.Item.DataItem, "CompanyID").ToString)


                    chbox = CType(e.Item.FindControl("chkPersonDirExclude"), CheckBox)
                    chbox.Checked = CType(DataBinder.Eval(e.Item.DataItem, "PersonDirExclude").ToString, Boolean)


                    chbox = CType(e.Item.FindControl("chkCompanyDirExclude"), CheckBox)
                    chbox.Checked = CType(DataBinder.Eval(e.Item.DataItem, "CompanyDirExclude").ToString, Boolean)



                    'encrypt and decrypt url


                    If Not CBool(CType(e.Item.FindControl("chkPersonDirExclude"), CheckBox).Text) Then
                        Dim lnkPerson As HyperLink
                        Dim tempURL As String
                        Dim index As Integer
                        Dim sValue As String
                        Dim separator As String()

                        lnkPerson = CType(e.Item.FindControl("lnkName"), HyperLink)
                        tempURL = lnkPerson.NavigateUrl
                        index = tempURL.IndexOf("=")
                        sValue = tempURL.Substring(index + 1)
                        separator = lnkPerson.NavigateUrl.Split(CChar("="))
                        lnkPerson.NavigateUrl = separator(0)
                        lnkPerson.NavigateUrl = lnkPerson.NavigateUrl & "="
                        lnkPerson.NavigateUrl = lnkPerson.NavigateUrl & System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(sValue))
                    Else
                        'remove Hyperlink if exclude from Directory
                        '  Dim lnk As HyperLink
                        lnk = CType(e.Item.FindControl("lnkName"), HyperLink)
                        lnk.NavigateUrl = ""
                        lnk.ForeColor = Drawing.Color.Black
                        lnk.Font.Underline = False
                    End If
                    If Not CBool(CType(e.Item.FindControl("chkCompanyDirExclude"), CheckBox).Text) Then
                        Dim lnkCompanies As HyperLink
                        Dim tempURL As String
                        Dim index As Integer
                        Dim sValue As String
                        Dim separator As String()

                        lnkCompanies = CType(e.Item.FindControl("RelatedPersonCompanyName"), HyperLink)
                        tempURL = lnkCompanies.NavigateUrl
                        index = tempURL.IndexOf("=")
                        sValue = tempURL.Substring(index + 1)
                        separator = lnkCompanies.NavigateUrl.Split(CChar("="))
                        lnkCompanies.NavigateUrl = separator(0)
                        lnkCompanies.NavigateUrl = lnkCompanies.NavigateUrl & "="
                        lnkCompanies.NavigateUrl = lnkCompanies.NavigateUrl & System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(sValue))
                    Else
                        'remove Hyperlink if exclude from Directory
                        ' Dim lnk As HyperLink
                        lnk = CType(e.Item.FindControl("RelatedPersonCompanyName"), HyperLink)
                        lnk.NavigateUrl = ""
                        lnk.ForeColor = Drawing.Color.Black
                        lnk.Font.Underline = False
                    End If





                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try

        End Sub

        'Protected Sub grdSpeakers_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles grdSpeakers.PageIndexChanged
        '    Try
        '        grdSpeakers.CurrentPageIndex = e.NewPageIndex
        '        grdSpeakers.DataSource = Session("DTSPEAKERS")
        '        grdSpeakers.DataBind()
        '        '  grdRelatedMeetingSessions.DataSource = Session("MyDT")
        '        '  grdRelatedMeetingSessions.DataBind()
        '    Catch ex As Exception
        '        Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        '    End Try
        'End Sub

        Protected Sub btnRegister_Click(sender As Object, e As System.EventArgs) Handles btnRegister.Click

            Try
                If ShoppingCart1.AddToCart(CLng(Request.QueryString("ID")), False, , , Page.Session) Then
                    Dim sProductPage As String, sOrderPage As String
                    If ShoppingCart1.GetProductTypeWebPages(CLng(Request.QueryString("ID")), sProductPage, sOrderPage) Then
                        If Len(sOrderPage) > 0 Then
                            ' special order page. redirect there now
                            Dim oOrder As AptifyGenericEntityBase
                            oOrder = ShoppingCart1.GetOrderObject(Page.Session, Page.User, Page.Application)
                            ShoppingCart1.SaveCart(Page.Session)
                            Response.Redirect(sOrderPage & "?OL=" & _
                                              oOrder.SubTypes("OrderLines").Count - 1)
                        End If
                    End If
                    ''lblRegistrationResult.Text = "This meeting has been successfully added to the shopping cart."
                Else
                    ''lnkRegister.Visible = False
                    ''imgRegister.Visible = False
                    ''lblRegistrationResult.ForeColor = Drawing.Color.Red
                    ''lblRegistrationResult.Font.Bold = True
                    ''lblRegistrationResult.Text = "ERROR: This meeting cannot be added to the shopping cart."
                    ''lblRegistrationResult.Visible = True
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub grdSpeakers_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdSpeakers.ItemDataBound
            Try
                Dim dateColumns As New List(Of String)
                'Add datecolumn uniqueName in list for Date format
                dateColumns.Add("GridDateTimeColumnEndDate")
                dateColumns.Add("GridDateTimeColumnStartDate")
                CommonMethods.FormatedDateOnGrid(dateColumns, e.Item)
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub grdRelatedMeetingSessions_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdRelatedMeetingSessions.ItemDataBound
            Try
                Dim dateColumns As New List(Of String)
                'Add datecolumn uniqueName in list for Date format
                dateColumns.Add("GridDateTimeColumnEndDate")
                dateColumns.Add("GridDateTimeColumnStartDate")
                CommonMethods.FormatedDateOnGrid(dateColumns, e.Item)
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
    End Class
End Namespace


