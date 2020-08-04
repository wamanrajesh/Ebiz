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
Imports System.Data.SqlClient

Namespace Aptify.Framework.Web.eBusiness
    Partial Class MeetingRegistrationSelectRegistrant
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "MeetingRegistrationSelectRegistrant"
        Protected Const ATTRIBUTE_MEETING_DETAIL_PAGE As String = "MeetingDetail"
        Protected Const ATTRIBUTE_ADMIN_EDIT_PROFILE As String = "AdminEditprofileUrl"
        Protected Const ATTRIBUTE_DT_WAITING_LIST As String = "dtWaitingList"
        Protected Const ATTRIBUTE_SELECTED_ROWS As String = "SelectedRecords"
        Protected Const ATTRIBUTE_ROW_INDEX As String = "rowIndex"
        Protected Const ATTRIBUTE_MEETING_SESSION As String = "MeetingSessions"
        'Neha Issue 14810,03/26/13, Declared properties for RadBinaryimage
        Protected Const ATTRIBUTE_PERSON_BLANK_IMG As String = "RadBlankImage"
        Protected Const ATTRIBUTE_PROFILE_THUMBNAIL_WIDTH As String = "ProfileThumbNailWidth"
        Protected Const ATTRIBUTE_PROFILE_THUMBNAIL_HEIGHT As String = "ProfileThumbNailHeight"

        Public Overridable Property MeetingDetail() As String
            Get
                If Not ViewState(ATTRIBUTE_MEETING_DETAIL_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_MEETING_DETAIL_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_MEETING_DETAIL_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        Public Overridable Property AdminEditProfile() As String
            Get
                If Not ViewState(ATTRIBUTE_ADMIN_EDIT_PROFILE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_ADMIN_EDIT_PROFILE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_ADMIN_EDIT_PROFILE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        'Neha, Issue 14810, 03/26/13, Overrided properties for Rdabinaryimage
        ''' <summary>
        ''' BlankImage
        ''' </summary>
        Public Overridable Property RadBlankImage() As String
            Get
                If Not ViewState(ATTRIBUTE_PERSON_BLANK_IMG) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_PERSON_BLANK_IMG))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_PERSON_BLANK_IMG) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

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
        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(MeetingDetail) Then
                'since value is the 'default' check the XML file for possible custom setting
                MeetingDetail = Me.GetLinkValueFromXML(ATTRIBUTE_MEETING_DETAIL_PAGE)
            End If

            If String.IsNullOrEmpty(AdminEditProfile) Then
                'since value is the 'default' check the XML file for possible custom setting
                AdminEditProfile = Me.GetLinkValueFromXML(ATTRIBUTE_ADMIN_EDIT_PROFILE)
                If String.IsNullOrEmpty(AdminEditProfile) Then
                    Me.grdmember.Enabled = False
                    Me.grdmember.ToolTip = "Admin Edit Profile property has not been set."
                End If
            End If
            If String.IsNullOrEmpty(RadBlankImage) Then
                RadBlankImage = Me.GetLinkValueFromXML(ATTRIBUTE_PERSON_BLANK_IMG)
            End If

        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
            lblError.Text = ""
            If Request.QueryString("OL") IsNot Nothing Then
                Session("OL") = Request.QueryString("OL").ToString()
            End If
            If Not IsPostBack Then
                If CheckFirmAdmin() = True Then
                    If Request.QueryString("ID") IsNot Nothing Then
                        If IsNumeric(Request.QueryString("ID")) Then
                            LoadMember()
                            GetMeetingInfo()
                        End If
                    End If

                End If
            End If

        End Sub

        Protected Sub grdmember_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grdmember.ItemCommand

            Dim BadgeInfo As String

            BadgeInfo = Convert.ToString(e.CommandName)
            If BadgeInfo = "EditBagde" Then

                EnableRadWin()
                Session(ATTRIBUTE_ROW_INDEX) = e.Item.ItemIndex
                txtBadgeName.Text = CType(grdmember.Items(e.Item.ItemIndex).FindControl("lblBadgeName"), Label).Text
                txtBadgeTitle.Text = CType(grdmember.Items(e.Item.ItemIndex).FindControl("lblBadgeTitle"), Label).Text
                txtBadgeCompany.Text = CType(grdmember.Items(e.Item.ItemIndex).FindControl("lblBadgeCompany"), Label).Text
                UserListDialog.VisibleOnPageLoad = True

            ElseIf BadgeInfo <> "Filter" Then

            ElseIf BadgeInfo = "Page" Then
            End If


        End Sub
        'Neha, Issue 14810, 03/26/13,used Radbinaryimage 
        Protected Sub grdmember_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdmember.ItemDataBound
            Try
                Dim imagememberid As RadBinaryImage = Nothing
                If e.Item Is Nothing OrElse e.Item.FindControl("imgmember") Is Nothing Then
                    Exit Sub
                End If
                imagememberid = CType(e.Item.FindControl("imgmember"), RadBinaryImage)
                'set the location of RadBlankImage to display in radbinaryimage control
                imagememberid.ImageUrl = Me.FixLinkForVirtualPath(RadBlankImage)
                imagememberid.DataBind()

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
                        imagememberid.DataValue = profileImageByte
                        imagememberid.DataBind()
                    Else
                        imagememberid.ImageUrl = Me.FixLinkForVirtualPath(RadBlankImage)
                        imagememberid.DataBind()
                    End If
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            UserListDialog.VisibleOnPageLoad = False

            txtBadgeName.Text = ""
            txtBadgeCompany.Text = ""
            txtBadgeTitle.Text = ""
            UpEditBadgeInfo.Update()

        End Sub

        Public Sub EnableRadWin()
            txtBadgeName.Enabled = True
            txtBadgeCompany.Enabled = True
            txtBadgeTitle.Enabled = True
            btnUpdate.Visible = True

        End Sub

        'Sub CheckCartForExisting(ByVal ProductID As Long)
        '    Try
        '        Dim oOrder As OrdersEntity
        '        Dim oOrderLine As OrderLinesEntity

        '        Dim bSameProduct As Boolean
        '        Dim lstAtendeeID As New ArrayList

        '        oOrder = ShoppingCart1.GetOrderObject(Me.Session, Page.User, Me.Application)

        '        For Each oOrderLine In oOrder.SubTypes("OrderLines")
        '            If oOrderLine.ProductID = ProductID Then
        '                bSameProduct = True
        '            End If
        '        Next
        '        If bSameProduct = False Then
        '            Session(ATTRIBUTE_SELECTED_ROWS) = Nothing
        '        End If
        '    Catch ex As Exception
        '        Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        '    End Try
        'End Sub


        Sub LoadMeetingDetailFromXML(ByVal MeetingDetailGE As AptifyGenericEntityBase, ByVal MeetingObjectXML As String)
            If MeetingObjectXML IsNot Nothing AndAlso _
               Len(MeetingObjectXML) > 0 Then
                MeetingDetailGE.Load("|" & MeetingObjectXML)
            End If
        End Sub

        Function CheckFirmAdmin() As Boolean

            If User1.PersonID < 0 Then
                Return False
            End If

            Dim sSQL As String
            Dim IsGroupAdmin As Boolean
            Try
                sSQL = "SELECT IsGroupAdmin FROM VWPERSONS WHERE ID = " & User1.PersonID

                IsGroupAdmin = CBool(DataAction.ExecuteScalar(sSQL))

                If IsGroupAdmin Then
                    Return True
                Else
                    Return False
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Function



        Private Sub LoadMember()
            Dim sSQL As String
            Dim params1(0), params2(1) As IDataParameter
            Dim DT As Data.DataTable
            Try


                Dim MeetingID As Long = CLng(Request.QueryString("ID"))

                sSQL = Database & ".." & "spWaitingList"

                params2(0) = Me.DataAction.GetDataParameter("@CompanyID", SqlDbType.Int, User1.CompanyID)
                params2(1) = Me.DataAction.GetDataParameter("@MeetingID", SqlDbType.Int, CInt(MeetingID))
                DT = Me.DataAction.GetDataTableParametrized(sSQL, CommandType.StoredProcedure, params2)


                Dim dcolUrl As DataColumn = New DataColumn()

                dcolUrl.Caption = "AdminEditprofileUrl"
                dcolUrl.ColumnName = "AdminEditprofileUrl"


                DT.Columns.Add(dcolUrl)
                If DT.Rows.Count > 0 Then
                    For Each rw As DataRow In DT.Rows
                        rw("AdminEditprofileUrl") = AdminEditProfile + "?ID=" + rw("AttendeeID").ToString()
                    Next
                End If


                If Not DT Is Nothing AndAlso DT.Rows.Count > 0 Then

                    Me.grdmember.DataSource = DT
                    Me.grdmember.DataBind()

                    grdmember.Visible = True
                    lblAvailableSpace.Visible = True
                    btnSubmit.Visible = True
                    lblText.Text = ""
                    Session(ATTRIBUTE_DT_WAITING_LIST) = DT
                Else

                    lblAvailableSpace.Visible = False
                    btnSubmit.Visible = False
                    grdmember.Visible = False
                    lblText.Text = "No Attendee Available for this Meeting."
                End If



            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                lblAvailableSpace.Visible = False
                btnSubmit.Visible = False
                grdmember.Visible = False
                lblText.Text = "No Attendee Available for this Meeting."
            End Try
        End Sub

        Sub GetMeetingInfo()
            Try
                If Request.QueryString("ID") <> "" Then
                    If IsNumeric(Request.QueryString("ID")) Then
                        LoadMeeting(CLng(Request.QueryString("ID")))
                    End If
                ElseIf Request.QueryString("OL") <> "" Then
                    Dim oOrder As OrdersEntity
                    Dim oOrderLine As OrderLinesEntity
                    Dim iLine As Integer
                    iLine = CInt(Request.QueryString("OL"))
                    oOrder = ShoppingCart1.GetOrderObject(Me.Session, Page.User, Me.Application)
                    oOrderLine = CType(oOrder.SubTypes("OrderLines").Item(iLine), OrderLinesEntity)
                    LoadMeeting(oOrderLine.ProductID)
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub LoadMeeting(ByVal ProductID As Long)
            ' load information for the meeting onto the screen
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
                       ProductID 'must be parameterized



                dt = DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                    With dt.Rows(0)

                        If CLng(.Item("ParentID")) > 0 Then
                            hfParentID.Value = CStr(.Item("ParentID"))
                            lblParent.Text = CStr(.Item("ParentWebName"))
                            lnkParent.NavigateUrl = MeetingDetail & "?View=Schedule&ID=" & CStr(.Item("ParentID"))
                            trSessionParent.Visible = True
                            lblDates.Text = CDate(.Item("StartDate")).ToLongDateString & " - " & _
                                            CDate(.Item("StartDate")).ToShortTimeString & " to " & _
                                            CDate(.Item("EndDate")).ToShortTimeString
                        Else
                            trSessionParent.Visible = False
                            lblDates.Text = CDate(.Item("StartDate")).ToLongDateString & " to " & _
                                            CDate(.Item("EndDate")).ToLongDateString
                        End If

                        lblName.Text = .Item("WebName").ToString.Trim
                        lblPlace.Text = .Item("Place").ToString.Trim
                        lblLocation.Text = .Item("Location").ToString.Trim

                        If grdmember.Items.Count = 0 Then
                            Exit Sub
                        End If

                        If ProdInventoryLedgerExist(CLng(.Item("ProductID"))) Then

                            lblText.Text = "Available Space: "

                            If CInt(.Item("AvailSpace")) > 0 Then
                                lblAvailableSpace.ForeColor = Drawing.Color.Green
                                lblMessage.Text = "You can register up to " & CStr(.Item("AvailSpace")) & " people for the meeting. Any additional registrations will be added to the Wait List."
                                lblMessage.Font.Bold = False
                            Else
                                lblAvailableSpace.ForeColor = Drawing.Color.Red
                                lblMessage.Text = "There is no space currently available for this meeting. Any attendee you register will be added to the Wait List."
                                lblMessage.Font.Bold = True
                            End If
                            lblAvailableSpace.Text = .Item("AvailSpace").ToString.Trim

                        End If
                    End With
                Else
                    lblAvailableSpace.Visible = False
                    lblName.Text = "Event Not Available!"
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub


        Private Function GetRegisteredStatusID(ByVal sStatus As String) As Long
            Dim sSQL As String
            sSQL = "SELECT ID FROM " & Database & _
                   "..vwAttendeeStatus WHERE Name= '" & sStatus & "'"
            Return CLng(DataAction.ExecuteScalar(sSQL))
        End Function



        Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
            Try

                Dim rowIndex As Integer = CInt(Session(ATTRIBUTE_ROW_INDEX))
                With grdmember

                    CType(.Items(rowIndex).FindControl("lblBadgeName"), Label).Text = txtBadgeName.Text
                    CType(.Items(rowIndex).FindControl("lblBadgeTitle"), Label).Text = txtBadgeTitle.Text
                    CType(.Items(rowIndex).FindControl("lblBadgeCompany"), Label).Text = txtBadgeCompany.Text

                End With

                UserListDialog.VisibleOnPageLoad = False
                UpEditBadgeInfo.Update()

            Catch ex As Exception

            End Try
        End Sub

        Protected Sub grdmember_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdmember.NeedDataSource
            If Session(ATTRIBUTE_DT_WAITING_LIST) IsNot Nothing Then
                grdmember.DataSource = CType(Session(ATTRIBUTE_DT_WAITING_LIST), DataTable)
            End If
        End Sub

        Protected Sub grdmember_PageIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageChangedEventArgs) Handles grdmember.PageIndexChanged
            SaveCheckedValues()
            UpEditBadgeInfo.Update()
        End Sub

        Protected Sub grdmember_PageSizeChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageSizeChangedEventArgs) Handles grdmember.PageSizeChanged

            If Session(ATTRIBUTE_DT_WAITING_LIST) IsNot Nothing Then
                grdmember.DataSource = CType(Session(ATTRIBUTE_DT_WAITING_LIST), DataTable)
            End If
            UpEditBadgeInfo.Update()
        End Sub

        Protected Sub grdmember_GridItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs)
            'SaveCheckedValues()
            'PopulateCheckedValues()
            Dim dtSelectedRecords As DataTable = Nothing
            Dim lstExistingAttendee As ArrayList = Nothing
            If Session(ATTRIBUTE_SELECTED_ROWS) IsNot Nothing Then
                dtSelectedRecords = CType(Session(ATTRIBUTE_SELECTED_ROWS), DataTable)
            End If
            Dim chkRegistrant As CheckBox = DirectCast(e.Item.FindControl("chkRegistrant"), CheckBox)
            Dim lblPersonID As Label = DirectCast(e.Item.FindControl("lblPersonID"), Label)
            If chkRegistrant IsNot Nothing Then

                Dim dataItem As DataRowView = DirectCast(e.Item.DataItem, System.Data.DataRowView)
                Dim personID As Long, i As Integer = 0
                If dtSelectedRecords IsNot Nothing Then
                    If dataItem IsNot Nothing Then
                        personID = CLng(dataItem("AttendeeID"))
                        If dtSelectedRecords.Rows.Contains(personID) Then
                            chkRegistrant.Checked = True
                        Else
                            chkRegistrant.Checked = False
                        End If
                    End If
                End If

            End If
        End Sub

        Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
            Dim lProductID As Long = -1
            Dim bSaveRegistration, bChecked As Boolean
            Dim oOrderLine As OrderLinesEntity
            Dim oMeetingDetail As AptifyGenericEntityBase
            Dim oOrderGE As OrdersEntity
            Dim iCount As Integer, bSameProduct As Boolean
            Dim lstAtendeeID As New List(Of Integer)
            Dim dtSelectedRecords As DataTable = Nothing
            Dim dtRow As DataRow = Nothing
            Dim sSQL As String = String.Empty
            Dim lstSubProdID As New List(Of Integer)
            Dim drSubProdID As SqlDataReader = Nothing
            bChecked = False
            iCount = 1
            Try
                SaveCheckedValues()
                If Session(ATTRIBUTE_SELECTED_ROWS) IsNot Nothing Then
                    dtSelectedRecords = CType(Session(ATTRIBUTE_SELECTED_ROWS), DataTable)
                End If
                If dtSelectedRecords.Rows.Count > 0 Then
                    bChecked = True
                End If
                'For i As Integer = 0 To grdmember.Items.Count - 1
                '    If CType(grdmember.Items(i).FindControl("chkRegistrant"), CheckBox).Checked = True Then
                '        bChecked = True
                '        Exit For
                '    End If
                'Next
                If bChecked = False Then
                    lblError.Visible = True
                    lblError.ForeColor = Drawing.Color.Red
                    lblError.Text = "Select at least one Attendee."
                    Exit Sub
                End If
                oOrderGE = Me.ShoppingCart1.GetOrderObject(Session, Page.User, Application)
                lProductID = CLng(Request.QueryString("ID"))
                ''Get Sub Product IDs
                sSQL = " Select SubProductID from vwProductParts PP Inner join vwProducts P  on  PP.ProductID = p.ID" & _
                    " where P.ID = " & lProductID & _
                    " And p.ProductKitTypeID = (Select ID from vwProductKitTypes where Name = 'Product Grouping') "
                drSubProdID = CType(DataAction.ExecuteDataReader(sSQL), SqlDataReader)

                While drSubProdID.Read()
                    lstSubProdID.Add(drSubProdID.GetInt32(drSubProdID.GetOrdinal("SubProductID")))
                End While

                For i = 0 To oOrderGE.SubTypes("OrderLines").Count - 1
                    oOrderLine = CType(oOrderGE.SubTypes("OrderLines").Item(i), OrderLinesEntity)
                    If oOrderLine.ProductID = lProductID OrElse lstSubProdID.Contains(CInt(oOrderLine.ProductID)) Then
                        oMeetingDetail = oOrderLine.ExtendedOrderDetailEntity
                        LoadMeetingDetailFromXML(oMeetingDetail, CStr(oOrderLine.GetValue("__ExtendedAttributeObjectData")))
                        bSameProduct = True
                        If oMeetingDetail IsNot Nothing Then
                            lstAtendeeID.Add(CInt(oMeetingDetail.GetValue("AttendeeID")))
                        End If
                    End If
                Next
                Dim SeesionList As New List(Of Integer)
                If Session(ATTRIBUTE_MEETING_SESSION) IsNot Nothing Then
                    SeesionList = CType(Session(ATTRIBUTE_MEETING_SESSION), List(Of Integer))
                End If


                If dtSelectedRecords IsNot Nothing AndAlso dtSelectedRecords.Rows.Count > 0 Then

                    'For Each row As DataRow In dtSelectedRecords.Rows
                    '    Dim AttendeeID As String = CStr(row.Item("AttendeeID"))
                    '    If Not lstAtendeeID.Contains(CLng(AttendeeID)) Then
                    '        If AddRegistrants(row, iCount, oOrderGE) Then
                    '            bSaveRegistration = True
                    '        End If
                    '        iCount += 1
                    '    Else
                    '        bSaveRegistration = True
                    '    End If
                    'NextlProductID
                    oOrderGE = oOrderGE.SaveMeetingSessionSForAttendees(dtSelectedRecords, SeesionList, lstAtendeeID, lProductID, lblAvailableSpace.Text, lblName.Text, User1.PreferredCurrencyTypeID)
                    bSaveRegistration = True
                    bChecked = True
                    Me.ShoppingCart1.SaveCart(Me.Session)
                Else
                    bChecked = False
                End If
                If bChecked = True Then
                    If bSaveRegistration = True Then
                        Session.Remove(ATTRIBUTE_ROW_INDEX)
                        Session.Remove(ATTRIBUTE_SELECTED_ROWS)
                        Session.Remove(ATTRIBUTE_MEETING_SESSION)
                        UpEditBadgeInfo.Update()
                        MyBase.Response.Redirect(RedirectURL, False)
                    End If
                End If
            Catch ex As Exception

                Session.Remove(ATTRIBUTE_ROW_INDEX)
                Session.Remove(ATTRIBUTE_SELECTED_ROWS)
                Session.Remove(ATTRIBUTE_MEETING_SESSION)
                UpEditBadgeInfo.Update()
                lblError.Visible = True
                lblError.ForeColor = Drawing.Color.Red
                lblError.Text = "Meeting is not available"
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Protected Function AddRegistrants(ByVal drSelectedRow As DataRow, ByVal iCount As Integer, ByVal OrderGE As Aptify.Applications.OrderEntry.OrdersEntity) As Boolean
            Try
                Dim lProductID As Long = CLng(Request.QueryString("ID"))
                Dim sAttendeeFirstLast As String = ""
                Dim sAttendeeEmail As String = ""
                Dim bSuccess As Boolean = False
                Dim sBadgeName As String = ""
                Dim sBadgeTitle As String = ""
                Dim sBadgeCompany As String = ""
                Dim sAttendeeID As String = ""
                Dim dprice As Double = 0.0
                Dim oPrice As New IProductPrice.PriceInfo
                Dim oProduct As Aptify.Applications.ProductSetup.ProductObject

                'With grdmember
                '    sAttendeeFirstLast = CType(.Items(IrowIndex).FindControl("lblMember"), System.Web.UI.WebControls.HyperLink).Text
                '    sAttendeeEmail = CType(.Items(IrowIndex).FindControl("lblEmail"), Label).Text
                '    sBadgeName = DirectCast(.Items(IrowIndex).FindControl("lblBadgeName"), Label).Text
                '    sBadgeTitle = DirectCast(.Items(IrowIndex).FindControl("lblBadgeTitle"), Label).Text
                '    sBadgeCompany = DirectCast(.Items(IrowIndex).FindControl("lblBadgeCompany"), Label).Text
                '    sAttendeeID = CType(.Items(IrowIndex).FindControl("lblPersonID"), Label).Text

                'End With
                With drSelectedRow
                    sAttendeeFirstLast = CType(drSelectedRow.Item("AttendeeFirstLast"), String)
                    sAttendeeEmail = CType(drSelectedRow.Item("AttendeeEmail"), String)
                    sBadgeName = CType(drSelectedRow.Item("BadgeName"), String)
                    sBadgeTitle = CType(drSelectedRow.Item("BadgeTitle"), String)
                    sBadgeCompany = CType(drSelectedRow.Item("BadgeCompany"), String)
                    sAttendeeID = CType(drSelectedRow.Item("AttendeeID"), String)

                End With

                oProduct = DirectCast(AptifyApplication.GetEntityObject("Products", lProductID), Aptify.Applications.ProductSetup.ProductObject)
                With OrderGE.AddProduct(lProductID).Item(0)
                    .GetPrice(oPrice, lProductID, 1, CLng(sAttendeeID), ProductGE:=oProduct, CurrencyTypeID:=User1.PreferredCurrencyTypeID)


                    .SetValue("Description", "Registration for " & sAttendeeFirstLast)
                    .SetValue("Price", oPrice.Price)
                    If Not IsNothing(.ExtendedOrderDetailEntity) Then
                        .ExtendedOrderDetailEntity.SetValue("ProductID", CStr(lProductID))
                        .ExtendedOrderDetailEntity.SetValue("AttendeeID", sAttendeeID)
                        .ExtendedOrderDetailEntity.SetValue("MeetingName", lblName.Text)


                        .ExtendedOrderDetailEntity.SetValue("AttendeeID_Email", sAttendeeEmail)
                        .ExtendedOrderDetailEntity.SetAddValue("AttendeeFirstLast", sAttendeeFirstLast)


                        .ExtendedOrderDetailEntity.SetValue("BadgeName", sBadgeName)
                        .ExtendedOrderDetailEntity.SetValue("BadgeCompanyName", sBadgeCompany)
                        .ExtendedOrderDetailEntity.SetValue("BadgeTitle", sBadgeTitle)

                        If ProdInventoryLedgerExist(lProductID) Then

                            If iCount > CInt(lblAvailableSpace.Text) Or CInt(lblAvailableSpace.Text) = 0 Then
                                .ExtendedOrderDetailEntity.SetValue("StatusID", GetRegisteredStatusID("Waiting"))
                            Else
                                .ExtendedOrderDetailEntity.SetValue("StatusID", GetRegisteredStatusID("Registered"))
                            End If
                        Else
                            .ExtendedOrderDetailEntity.SetValue("StatusID", GetRegisteredStatusID("Registered"))
                        End If


                        .SetAddValue("__ExtendedAttributeObjectData", .ExtendedOrderDetailEntity.GetObjectData(False))


                        If Not SaveMeetingSessions(OrderGE, CLng(sAttendeeID), sAttendeeFirstLast, iCount + 1, sBadgeName, sBadgeCompany, sBadgeTitle) Then

                            Return False
                            Exit Function
                        End If
                    End If
                End With
                Return True
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                lblError.Text = "Meeting is not available"
                Return False
            End Try
        End Function
        Private Function ProdInventoryLedgerExist(ByVal ProductID As Long) As Boolean
            Try
                Dim sSQL As String
                sSQL = "SELECT ID FROM " & Database & _
                       "..vwProdInvLedger where ProductID =  " & ProductID
                Return CBool(DataAction.ExecuteScalar(sSQL))

            Catch ex As Exception

            End Try
        End Function

        Protected Overridable Function SaveMeetingSessions(ByVal OrderGE As OrdersEntity, _
                              ByVal AttendeeID As Long, ByVal sAttendeeFirstLast As String, _
                              ByVal ParentSequence As Integer, ByVal sBadgeName As String, ByVal sBadgeCompany As String, ByVal sBadgeTitle As String) As Boolean
            Try
                ' go through the check boxes and add one line to the order for each such item
                Dim lProductID As Long, sBaseKey As String, i As Integer
                Dim oParInfo As New Generic.List(Of ParentChildInfo)
                Dim oOrderLine As OrderLinesEntity
                'Dim oOrder As OrdersEntity
                'Dim iLine As Integer
                'oOrder = ShoppingCart1.GetOrderObject(Page.Session, Page.User, Page.Application)
                'iLine = CInt(oOrder.SubTypes("OrderLines").Count - 1)
                'oOrderLine = CType(OrderGE.SubTypes("OrderLines").Item(iLine), OrderLinesEntity)

                'BuildParentInfo(OrderGE, oParInfo, ParentSequence)
                Dim SeesionList As New List(Of Integer)
                If Session(ATTRIBUTE_MEETING_SESSION) IsNot Nothing Then
                    SeesionList = CType(Session(ATTRIBUTE_MEETING_SESSION), List(Of Integer))
                    Dim lProductSessionID As Long
                    Try
                        For Each lProductSessionID In SeesionList
                            oOrderLine = OrderGE.AddProduct(lProductSessionID).Item(0)
                            With oOrderLine
                                .SetValue("Description", "Registration for " & sAttendeeFirstLast)
                                .ExtendedOrderDetailEntity.SetValue("AttendeeID", AttendeeID)
                                .ExtendedOrderDetailEntity.SetValue("BadgeName", sBadgeName)
                                .ExtendedOrderDetailEntity.SetValue("BadgeCompanyName", sBadgeCompany)
                                .ExtendedOrderDetailEntity.SetValue("BadgeTitle", sBadgeTitle)
                                .SetAddValue("__ExtendedAttributeObjectData", .ExtendedOrderDetailEntity.GetObjectData(False))
                            End With
                        Next
                    Catch ex As Exception
                        Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                        lblError.Visible = True
                        lblError.ForeColor = Drawing.Color.Red
                        lblError.Text = "Session is not available"
                    End Try

                End If



                'Dim iNumFound As Integer = 0
                'i = 0
                'While i < OrderGE.SubTypes("OrderLines").Count
                '    With OrderGE.SubTypes("OrderLines").Item(i)
                '        If CInt(.GetValue("ParentSequence")) = ParentSequence Then

                '            If i <> ParentSequence + iNumFound Then
                '                OrderGE.SubTypes("OrderLines").MoveObject(i, ParentSequence + iNumFound, AptifyMoveObjectOptions.aptifyShift)
                '            Else
                '                i += 1
                '            End If
                '            iNumFound += 1
                '        Else
                '            i += 1
                '        End If
                '    End With
                'End While

                'AdjustParentInfo(OrderGE, oParInfo)
                Return True
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Function
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
        ''RashmiP,5/1/2013, 15440 Code not required for Group Admin. Removed RefreshSession() and MatchAndDeleteSession() Methods

        'This method is used to save the checkedstate of values
        Private Sub SaveCheckedValues()
            Dim userdetails As New ArrayList()
            Dim dtSelectedRecord As DataTable
            Dim index As String = ""
            dtSelectedRecord = New DataTable

            dtSelectedRecord.Columns.Add("AttendeeFirstLast")
            dtSelectedRecord.Columns.Add("AttendeeEmail")
            dtSelectedRecord.Columns.Add("BadgeName")
            dtSelectedRecord.Columns.Add("BadgeTitle")
            dtSelectedRecord.Columns.Add("BadgeCompany")
            dtSelectedRecord.Columns.Add("AttendeeID")
            Dim primaryKey(0) As DataColumn
            primaryKey(0) = dtSelectedRecord.Columns("AttendeeID")
            dtSelectedRecord.PrimaryKey = primaryKey

            If Session(ATTRIBUTE_SELECTED_ROWS) IsNot Nothing Then
                dtSelectedRecord = CType(Session(ATTRIBUTE_SELECTED_ROWS), DataTable)
            End If

            For Each item As GridDataItem In grdmember.MasterTableView.Items
                index = CType(item.FindControl("lblPersonID"), Label).Text

                Dim result As Boolean = DirectCast(item.FindControl("chkRegistrant"), CheckBox).Checked

                Dim dr As DataRow = dtSelectedRecord.NewRow
                If result Then

                    dr.Item("AttendeeFirstLast") = CType(item.FindControl("lblMember"), System.Web.UI.WebControls.Label).Text
                    dr.Item("AttendeeEmail") = CType(item.FindControl("lblEmail"), Label).Text
                    dr.Item("BadgeName") = CType(item.FindControl("lblBadgeName"), Label).Text
                    dr.Item("BadgeTitle") = CType(item.FindControl("lblBadgeTitle"), Label).Text
                    dr.Item("BadgeCompany") = CType(item.FindControl("lblBadgeCompany"), Label).Text
                    dr.Item("AttendeeID") = CType(item.FindControl("lblPersonID"), Label).Text
                    If Not dtSelectedRecord.Rows.Contains(index) Then
                        dtSelectedRecord.Rows.Add(dr)
                    End If
                Else
                    If dtSelectedRecord.Rows.Contains(index) Then
                        dr = dtSelectedRecord.Rows.Find(index)
                        dtSelectedRecord.Rows.Remove(dr)
                    End If
                End If
            Next

            Session(ATTRIBUTE_SELECTED_ROWS) = dtSelectedRecord

        End Sub

        Private Class ParentChildInfo
            Public ParentLine As OrderLinesEntity
            Public SubLines As New Generic.List(Of OrderLinesEntity)
        End Class


    End Class
End Namespace
