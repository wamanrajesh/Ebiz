'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

#Region "Namespace"

Imports Aptify.Framework.DataServices
Imports System.IO
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports System.Data
Imports Telerik.Web.UI
Imports System.Web
Imports System.Web.UI.WebControls
#End Region

Namespace Aptify.Framework.Web.eBusiness.CustomerService
    Partial Class EventListing
        Inherits BaseUserControlAdvanced
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "EventListing"
        Protected Const ATTRIBUTE_EVENTREG_PAGE As String = "EventRegListing"
        Private Const GET_EVENT_REGDETAILS_VIEWNAME As String = "vwEventRegDetails"
        'Neha Issue 14810,03/09/13, Declared properties for RadBinaryimage
        Protected Const ATTRIBUTE_PERSON_BLANK_IMG As String = "RadBlankImage"
        Protected Const ATTRIBUTE_PROFILE_THUMBNAIL_WIDTH As String = "ProfileThumbNailWidth"
        Protected Const ATTRIBUTE_PROFILE_THUMBNAIL_HEIGHT As String = "ProfileThumbNailHeight"
        Protected Const ATTRIBUTE_LOGIN_PAGE As String = "LoginPage" 'Added by Sandeep for Issue 15051 on 12/03/2013
        Protected Const ATTRIBUTE_MEMBERS_DT As String = "dtMembers"

#Region "Event"
        Public Overridable Property EventRegListing() As String
            Get
                If Not ViewState.Item(ATTRIBUTE_EVENTREG_PAGE) Is Nothing Then
                    Return CStr(ViewState.Item(ATTRIBUTE_EVENTREG_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_EVENTREG_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        'Neha, Issue 14810, 03/09/13, Overrided properties for Rdabinaryimage
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
        Protected Overrides Sub SetProperties()
            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()
            If String.IsNullOrEmpty(EventRegListing) Then
                EventRegListing = Me.GetLinkValueFromXML(ATTRIBUTE_EVENTREG_PAGE)
            End If
            If String.IsNullOrEmpty(RadBlankImage) Then
                RadBlankImage = Me.GetLinkValueFromXML(ATTRIBUTE_PERSON_BLANK_IMG)
            End If
            'Added by Sandeep for Issue 15051 on 12/03/2013
            If String.IsNullOrEmpty(LoginPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                LoginPage = Me.GetLinkValueFromXML(ATTRIBUTE_LOGIN_PAGE)
            End If
        End Sub
        'Neha, Issue 14810, 03/09/13,used Radbinaryimage and Resize the Image
        Protected Sub RadgrdWaitingList_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles RadgrdWaitingList.ItemDataBound
            Try
                Dim lblstatus As New Label
                lblstatus = CType(e.Item.FindControl("lblstatus"), Label)
                If Not lblstatus Is Nothing AndAlso Not lblstatus.Text.Trim() = "Registered" Then
                    Dim PreviewLink As New LinkButton
                    Dim EditBagdeLink As New LinkButton
                    Dim lblNotApplicable As New Label
                    PreviewLink = CType(e.Item.FindControl("PreviewLink"), LinkButton)
                    EditBagdeLink = CType(e.Item.FindControl("EditBagdeLink"), LinkButton)
                    lblNotApplicable = CType(e.Item.FindControl("lblNotApplicable"), Label)
                    lblNotApplicable.Visible = True
                    EditBagdeLink.Visible = False
                End If
                Dim imgPhoto As RadBinaryImage = Nothing
                If e.Item Is Nothing OrElse e.Item.FindControl("RadBinaryImgPhoto") Is Nothing Then
                    Exit Sub
                End If
                'set the location of BlankImage to display in radbinaryimage control
                imgPhoto = CType(e.Item.FindControl("RadBinaryImgPhoto"), RadBinaryImage)
                imgPhoto.ImageUrl = Me.FixLinkForVirtualPath(RadBlankImage)
                imgPhoto.DataBind()
                'Neha Changes for issue 16001,05/07/13
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
                        imgPhoto.DataValue = profileImageByte
                        imgPhoto.DataBind()
                    Else
                        imgPhoto.ImageUrl = Me.FixLinkForVirtualPath(RadBlankImage)
                        imgPhoto.DataBind()
                    End If
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub RadgrdWaitingList_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles RadgrdWaitingList.NeedDataSource
            Try
                If Not e.IsFromDetailTable Then
                    If ViewState(ATTRIBUTE_MEMBERS_DT) IsNot Nothing Then
                        RadgrdWaitingList.DataSource = CType(ViewState(ATTRIBUTE_MEMBERS_DT), DataTable)
                    End If
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Protected Sub RadgrdWaitingList_PageIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageChangedEventArgs) Handles RadgrdWaitingList.PageIndexChanged
            ' RadgrdWaitingList.CurrentPageIndex = e.NewPageIndex
            If ViewState(ATTRIBUTE_MEMBERS_DT) IsNot Nothing Then
                RadgrdWaitingList.DataSource = CType(ViewState(ATTRIBUTE_MEMBERS_DT), DataTable)
            End If
        End Sub
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                SetProperties()
                If Not IsPostBack Then
                    Session("Status") = ""
                    Session("Productid") = ""
                    ViewState("sDateString") = ""
                    lblSelections.Text = ""
                    lblSelections.Visible = True
                    AddExpression()
                    If (Len(Request.QueryString("Confirmed")) = 0 AndAlso Len(Request.QueryString("WaitList")) = 0 AndAlso Len(Request.QueryString("sAll")) = 0 _
                        AndAlso Len(Request.QueryString("MonthYear")) = 0 AndAlso Len(Request.QueryString("PMonthYear")) = 0) Then
                        lblSelections.Text = "AttendeeID was not found in the " _
                            & "QueryString collection!"
                    Else
                        Dim Productid, sSQL, FUTUREDATE, PASTDATE As String
                        If Len(Request.QueryString("MonthYear")) > 0 Then
                            FUTUREDATE = Request.QueryString("MonthYear")
                            sSQL &= " AND CONVERT(CHAR(7),CONVERT(date,'" + FUTUREDATE + "'), 120)=CONVERT(CHAR(7),StartDate,120)"
                            sSQL &= "  AND (StartDate>=GETDATE() Or (StartDate<GETDATE() AND EndDate>=GETDATE())) "
                            ViewState("sDateString") = sSQL
                        End If
                        If Len(Request.QueryString("PAST")) > 0 Then
                            PASTDATE = Request.QueryString("PAST")
                            sSQL = String.Empty
                            sSQL &= " AND CONVERT(CHAR(7),CONVERT(date,'" + PASTDATE + "'), 120)=CONVERT(CHAR(7),StartDate,120)"
                            sSQL &= "AND (StartDate<GETDATE()) "
                            PASTDATE = Request.QueryString("PAST") + " AND EndDate<GETDATE() "
                            ViewState("sDateString") = sSQL
                        End If
                        If Len(Request.QueryString("Confirmed")) > 0 Then
                            Productid = Request.QueryString("Confirmed")
                            Session("Productid") = Productid
                            Session("Status") = "Confirmed"
                            GetMeetingEventList("Confirmed", Productid)
                        End If
                        If Len(Request.QueryString("WaitList")) > 0 Then
                            Productid = Request.QueryString("WaitList")
                            Session("Productid") = Productid
                            Session("Status") = "WaitList"
                            GetMeetingEventList("WaitList", Productid)
                        End If
                        If Len(Request.QueryString("sAll")) > 0 Then
                            Productid = Request.QueryString("sAll")
                            Session("Productid") = Productid
                            Session("Status") = "sAll"
                            GetMeetingEventList("sAll", Productid)
                        End If
                        If Len(Request.QueryString("NotRegistered")) > 0 Then
                            Productid = Request.QueryString("NotRegistered")
                            Session("Productid") = Productid
                            Session("Status") = "NotRegistered"
                            GetMeetingEventList("NotRegistered", Productid)
                        End If
                    End If
                End If
                If User1.UserID < 0 Then
                    Response.Redirect(LoginPage) 'Added by Sandeep for Issue 15051 on 12/03/2013
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
#End Region
#Region "Functions and Methods"
       
        Public Sub GetMeetingEventList(ByVal Status As String, ByVal Productid As String)
            Try
                Dim sSQL As String = String.Empty
                Dim dt As DataTable
                Select Case Status
                    Case "Confirmed"
                        RadgrdWaitingList.MasterTableView.GetColumn("TemplateEditColumn").Visible = True
                        lblSelections.Text = "Confirmed Members"
                        sSQL = "SELECT ERD.OrderDetailID, EL.OrderID,EL.AttendeeID,EL.Subscriber,EL.Title,EL.City,EL.Status,p.WebName MeetingTitle,vp.Photo  FROM " & Database & _
                            "..vwEventList EL INNER JOIN " & GET_EVENT_REGDETAILS_VIEWNAME & " ERD ON EL.OrderID=ERD.OrderID AND EL.AttendeeID=ERD.AttendeeID INNER JOIN " & Me.AptifyApplication.GetEntityBaseDatabase("Products") & ".." & Me.AptifyApplication.GetEntityBaseView("Products") & " p on El.ProductID =p.ID Inner Join vwPersons vp on El.AttendeeID=vp.ID    where EL.status='Registered' AND EL.CompanyID=" & Convert.ToString(User1.CompanyID) + ViewState("sDateString").ToString & _
                         " AND ERD.ProductId=" & Productid & "And EL.ProductID = " & Productid
                    Case "WaitList"
                        RadgrdWaitingList.MasterTableView.GetColumn("TemplateEditColumn").Visible = False
                        lblSelections.Text = "Members in waiting list"
                        sSQL = "SELECT ERD.OrderDetailID, EL.OrderID,EL.AttendeeID,EL.Subscriber,EL.Title,EL.City,EL.Status,p.WebName MeetingTitle,vp.Photo  " & _
                          "FROM " & _
                          Database & _
                          "..vwEventList EL INNER JOIN " & GET_EVENT_REGDETAILS_VIEWNAME & " ERD ON EL.OrderID=ERD.OrderID AND EL.AttendeeID=ERD.AttendeeID INNER JOIN " & Me.AptifyApplication.GetEntityBaseDatabase("Products") & ".." & Me.AptifyApplication.GetEntityBaseView("Products") & " p on El.ProductID =p.ID Inner Join vwPersons vp on El.AttendeeID=vp.ID     WHERE  EL.status='Waiting' AND EL.CompanyID=" & Convert.ToString(User1.CompanyID) + ViewState("sDateString").ToString & _
                         " AND EL.ProductId=" & Productid & " AND ERD.ProductId=" & Productid
                    Case "NotRegistered"
                        RadgrdWaitingList.MasterTableView.GetColumn("TemplateEditColumn").Visible = False
                        lblSelections.Text = "Non-Registered Members"
                        Dim sSpName As String = "spNonRegisteredMembersForMeeting"
                        sSQL = "Exec " & sSpName & "'" & User1.CompanyID & "', '" & Productid & "'"
                    Case Else
                        RadgrdWaitingList.MasterTableView.GetColumn("TemplateEditColumn").Visible = True
                        lblSelections.Text = "Members"
                        sSQL = "SELECT  ERD.OrderDetailID,EL.OrderID,EL.AttendeeID,EL.Subscriber,EL.Title,EL.City,EL.Status,p.WebName MeetingTitle,vp.Photo  " & _
                           "FROM " & _
                           Database & _
                               "..vwEventList EL INNER JOIN " & GET_EVENT_REGDETAILS_VIEWNAME & " ERD ON EL.OrderID = ERD.OrderID AND EL.AttendeeID = ERD.AttendeeID INNER JOIN " & Me.AptifyApplication.GetEntityBaseDatabase("Products") & ".." & Me.AptifyApplication.GetEntityBaseView("Products") & " p on El.ProductID =p.ID Inner Join vwPersons vp on El.AttendeeID=vp.ID      WHERE  (EL.status='Waiting' or EL.status='Registered') AND EL.CompanyID=" & Convert.ToString(User1.CompanyID) + ViewState("sDateString").ToString & _
                         " AND EL.ProductId=" & Productid & " and EL.CompanyID=" & Convert.ToString(User1.CompanyID) + ViewState("sDateString").ToString & _
                         " AND ERD.ProductId=" & Productid
                End Select
                dt = DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)
                If dt.Rows.Count > 0 Then
                    lblMessage.Text = ""
                    RadgrdWaitingList.DataSource = dt
                    RadgrdWaitingList.DataBind()
                    ViewState(ATTRIBUTE_MEMBERS_DT) = dt
                Else
                    lblSelections.Text = ""
                    lblSelections.Visible = False
                    lblMessage.Visible = True
                    lblMessage.Text = "There are no members for this month."
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Protected Sub ToggleRowSelection(ByVal sender As Object, ByVal e As EventArgs)
            Try
                TryCast(TryCast(sender, CheckBox).NamingContainer, GridItem).Selected = TryCast(sender, CheckBox).Checked
                Dim checkHeader As Boolean = True
                For Each dataItem As GridDataItem In RadgrdWaitingList.MasterTableView.Items
                    If Not TryCast(dataItem.FindControl("chkRenewal"), CheckBox).Checked Then
                        checkHeader = False
                        Exit For
                    Else
                        Dim selectedItem As GridDataItem = DirectCast(RadgrdWaitingList.SelectedItems(0), GridDataItem)
                        Dim value As String = selectedItem("ID").Text
                        lblSelections.Text = value
                        lblSelections.Visible = True
                    End If
                Next
                Dim headerItem As GridHeaderItem = TryCast(RadgrdWaitingList.MasterTableView.GetItems(GridItemType.Header)(0), GridHeaderItem)
                TryCast(headerItem.FindControl("headerChkbox"), CheckBox).Checked = checkHeader
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Protected Sub ToggleSelectedState(ByVal sender As Object, ByVal e As EventArgs)
            Try
                Dim headerCheckBox As CheckBox = TryCast(sender, CheckBox)
                For Each dataItem As GridDataItem In RadgrdWaitingList.MasterTableView.Items
                    TryCast(dataItem.FindControl("chkRenewal"), CheckBox).Checked = headerCheckBox.Checked
                    dataItem.Selected = headerCheckBox.Checked
                    If headerCheckBox.Checked = True Then
                        Dim selectedItem As GridDataItem = DirectCast(RadgrdWaitingList.SelectedItems(0), GridDataItem)
                        Dim value As String = Convert.ToString(dataItem.FindControl("ID"))
                        lblSelections.Text = value
                        lblSelections.Visible = True
                    End If
                Next
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
#End Region
        Protected Sub BtnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnBack.Click
            ViewState.Remove(ATTRIBUTE_MEMBERS_DT)
            Response.Redirect(EventRegListing, False)
        End Sub

        Public Sub BindBadgeDetails(ByVal OrderDetailID As Integer)
            Try
                Dim sSQL As String
                Dim dt As DataTable
                sSQL = "SELECT BadgeName, BadgeCompanyName, BadgeTitle " & _
                           "FROM " & _
                           Database & _
                           ".." & GET_EVENT_REGDETAILS_VIEWNAME & " WHERE CompanyID=" & Convert.ToString(User1.CompanyID) & _
                         " AND OrderDetailID=" & OrderDetailID
                dt = DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)
                If dt.Rows.Count > 0 Then
                    If Not IsDBNull(dt.Rows(0)("BadgeName")) Then
                        txtBadgeName.Text = Trim(CStr(dt.Rows(0)("BadgeName")))
                    End If
                    If Not IsDBNull(dt.Rows(0)("BadgeCompanyName")) Then
                        txtBadgeCompany.Text = Trim(CStr(dt.Rows(0)("BadgeCompanyName")))
                    End If
                    If Not IsDBNull(dt.Rows(0)("BadgeTitle")) Then
                        txtBadgeTitle.Text = Trim(CStr(dt.Rows(0)("BadgeTitle")))
                    End If
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        ''' <summary>
        ''' Set enbled control
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub EnableRadWin()
            txtBadgeName.Enabled = True
            txtBadgeCompany.Enabled = True
            txtBadgeTitle.Enabled = True
            BtnUpdate.Visible = True
        End Sub
        ''' <summary>
        ''' Open Bladge information poup and bind bladge data to control
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub RadgrdWaitingList_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles RadgrdWaitingList.ItemCommand
            Session("AttendeeID") = ""
            Session("OrderDetailID") = ""
            Dim BadgeInfo As String
            Dim OrderDetailID As Integer
            BadgeInfo = Convert.ToString(e.CommandName)
            If BadgeInfo = "EditBagde" Then
                OrderDetailID = CInt(Convert.ToInt64(e.CommandArgument))
                Session("OrderDetailID") = Convert.ToInt64(OrderDetailID)
                BindBadgeDetails(OrderDetailID)
                EnableRadWin()
                UserListDialog.VisibleOnPageLoad = True
            End If
        End Sub
        Protected Sub BtnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
            UserListDialog.VisibleOnPageLoad = False
            txtBadgeName.Text = ""
            txtBadgeCompany.Text = ""
            txtBadgeTitle.Text = ""
            Session("Productid") = ""
            Session("AttendeeID") = ""
        End Sub
        ''' <summary>
        ''' Close Bladge information Popup
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub BtnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnUpdate.Click
            If UpdateRegistration() Then
                UserListDialog.VisibleOnPageLoad = False
            End If
        End Sub
        Public Function GetOrderMeetDetailID(ByVal iId As Long) As Long
            Dim NewID As Int32 = 0
            Dim sql As String
            sql = "SELECT ID  FROM " + Database + "..vwOrderMeetDetail WHERE OrderDetailID=" & iId
            Try
                If iId > 0 Then
                    NewID = CInt(DataAction.ExecuteScalar(sql))
                End If
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try
            Return NewID
        End Function
        ''' <summary>
        ''' Updated Bladge Information
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function UpdateRegistration() As Boolean
            If txtBadgeName.Text.Trim().Length > 0 And txtBadgeTitle.Text.Trim().Length > 0 And txtBadgeCompany.Text.Trim().Length > 0 Then
                Dim oOrgOMD As AptifyGenericEntityBase
                Dim iId As Long
                iId = GetOrderMeetDetailID(CLng(Session("OrderDetailID")))
                If iId > 0 Then
                    oOrgOMD = Me.AptifyApplication.GetEntityObject("OrderMeetingDetail", iId)
                    Dim sErrorString As String = ""
                    With oOrgOMD
                        .SetValue("BadgeName", Me.txtBadgeName.Text)
                        .SetValue("BadgeCompanyName", Convert.ToString(Me.txtBadgeCompany.Text.Trim))
                        .SetValue("BadgeTitle", Me.txtBadgeTitle.Text)
                        If Not .Save(sErrorString) Then
                            lblRegistrationResult.ForeColor = Drawing.Color.Red
                            lblRegistrationResult.Font.Bold = True
                            lblRegistrationResult.Visible = True
                            Me.lblRegistrationResult.Text = "There was an error submitting this form. Please try again later." + sErrorString
                            Return False
                        End If
                    End With
                End If
            Else
                Return False
            End If
            Return True
        End Function
        Public Function GetOrderDetailID() As Integer
            Dim NewID As Int32 = 0
            Dim OrderId As Int32
            OrderId = GetOrderID()
            Dim sSQL As String
            sSQL = "SELECT OrderDetailID  FORM " + Database + "..vwGetCountOfRegisteredWaitingPerson WHERE CompanyID=" & Convert.ToString(User1.CompanyID) & _
                         " and ProductId=" & Session("Productid").ToString & _
                          " and OrderID=" & OrderId.ToString & _
                           " and AttendeeID=" & Session("AttendeeID").ToString
            Try
                If OrderId > 0 Then
                    NewID = CInt(DataAction.ExecuteScalar(sSQL))
                End If
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try
            Return NewID
        End Function
        Public Function GetOrderID() As Integer
            Dim NewID As Int32 = 0
            Dim sSQL As String
            sSQL = "SELECT OrderID  FROM " + Database + "..vwGetOrderId WHERE CompanyID=" & Convert.ToString(User1.CompanyID) & _
                         " and ProductId=" & Session("Productid").ToString & _
                           " and AttendeeID=" & Session("AttendeeID").ToString
            Try
                NewID = CInt(DataAction.ExecuteScalar(sSQL))
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try
            Return NewID
        End Function
        Private Sub AddExpression()
            Dim expression1 As New GridSortExpression
            expression1.FieldName = "Subscriber"
            expression1.SetSortOrder("Ascending")
            RadgrdWaitingList.MasterTableView.SortExpressions.AddSortExpression(expression1)
        End Sub
        Protected Sub RadgrdWaitingList_PageSizeChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageSizeChangedEventArgs) Handles RadgrdWaitingList.PageSizeChanged
            If ViewState(ATTRIBUTE_MEMBERS_DT) IsNot Nothing Then
                RadgrdWaitingList.DataSource = CType(ViewState(ATTRIBUTE_MEMBERS_DT), DataTable)
            End If
        End Sub
    End Class
End Namespace

