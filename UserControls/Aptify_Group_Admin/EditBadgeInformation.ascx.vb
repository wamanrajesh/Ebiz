'Aptify e-Business 5.5.1, July 2013
#Region "Namespace"

Imports Aptify.Framework.DataServices
Imports System.IO
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports System.Data
Imports Telerik.Web.UI
Imports System.Web
#End Region
Namespace Aptify.Framework.Web.eBusiness.CustomerService
    Partial Class EditBadgeInformation
        Inherits BaseUserControlAdvanced
        'vwEventRegDetails

        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "EditBadgeInformation"
        Protected Const ATTRIBUTE_EVENTREG_PAGE As String = "EditRegMeeting"
        Protected Const ATTRIBUTE_LOGIN_PAGE As String = "LoginPage" 'Added by Sandeep for Issue 15051 on 12/03/2013
        Protected Const ATTRIBUTE_SHOWMEETINGSLINKTOCLASS_DEFAULT_NAME As String = "ShowMeetingsLinkToClass"
        Protected Const ATTRIBUTE_DATATABLE_EVENTREG As String = "dtEventReg"
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

        Public Overridable Property EditRegMeeting() As String
            Get
                If Not ViewState.Item(ATTRIBUTE_EVENTREG_PAGE) Is Nothing Then
                    Return CStr(ViewState.Item(ATTRIBUTE_EVENTREG_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ' ViewState.Item(ATTRIBUTE_EVENTREG_PAGE) = value
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
        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(EditRegMeeting) Then
                EditRegMeeting = Me.GetLinkValueFromXML(ATTRIBUTE_EVENTREG_PAGE)
                If String.IsNullOrEmpty(EditRegMeeting) Then
                    Me.RadgrdBadgeInformation.Enabled = False
                    Me.RadgrdBadgeInformation.ToolTip = "RegistrationsURL property has not been set."
                End If
            End If
            'Added by Sandeep for Issue 15051 on 12/03/2013
            If String.IsNullOrEmpty(LoginPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                LoginPage = Me.GetLinkValueFromXML(ATTRIBUTE_LOGIN_PAGE)
            End If
            Dim bShowMeeting As Boolean = ShowMeetingsLinkToClass()
        End Sub

#Region "Functions and Methods"
        Public Sub BindEventRegDetails(ByVal Productid As String)
            Try
                Dim sSQL As String
                Dim dt As DataTable
                Dim bShowMeeting As Boolean = ShowMeetingsLinkToClass()

                If ViewState(ATTRIBUTE_DATATABLE_EVENTREG) IsNot Nothing Then
                    RadgrdBadgeInformation.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_EVENTREG), Data.DataTable)
                    RadgrdBadgeInformation.DataBind()
                    Exit Sub
                End If

                sSQL = "SELECT * " & _
                           "FROM " & _
                           Database & _
                           "..vwEventRegDetails where CompanyID=" & Convert.ToString(User1.CompanyID) & _
                         " and ProductId=" & Productid
                If Not bShowMeeting Then
                    sSQL &= "  AND  ISNULL(vp.ClassID ,-1) <=0 "
                End If
                dt = DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)

                If dt.Rows.Count > 0 Then
                    lblRegistrationTitle.Text = ""
                    RadgrdBadgeInformation.DataSource = dt
                    RadgrdBadgeInformation.DataBind()
                    ViewState(ATTRIBUTE_DATATABLE_EVENTREG) = dt
                Else
                    lblRegistrationTitle.Text = "Registrations information is not available for this month and year! "
                    RadgrdBadgeInformation.Visible = False
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try

        End Sub



        Private Function UpdateRegistration() As Boolean

            If txtBadgeName.Text.Trim().Length > 0 And txtBadgeTitle.Text.Trim().Length > 0 And txtBadgeCompany.Text.Trim().Length > 0 Then
                'trMsg.Style.Add("display", "none")
                'trMsg.Visible = False
                Dim oOrgOMD As AptifyGenericEntityBase
                Dim iId As Long

                iId = GetOrderMeetDetailID(Session("OrderDetailID"))

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
                'trMsg.Visible = True
                Return False
            End If
            Return True
        End Function
        Public Function GetOrderMeetDetailID(ByVal iId As Long) As Long
            Dim NewID As Int32 = 0
            'Dim OrderlineId As Int32 = 0
            'OrderlineId = GetOrderDetailID()
            Dim sql As String
            sql = "SELECT ID  from " + Database + "..vwOrderMeetDetail where OrderDetailID=" & iId
            Try
                If iId > 0 Then
                    NewID = CInt(DataAction.ExecuteScalar(sql))
                End If
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try


            Return NewID
        End Function

        Public Function GetOrderDetailID() As Integer
            Dim NewID As Int32 = 0
            Dim OrderId As Int32
            OrderId = GetOrderID()

            Dim sSQL As String
            sSQL = "SELECT OrderDetailID  from " + Database + "..vwGetCountOfRegisteredWaitingPerson where CompanyID=" & Convert.ToString(User1.CompanyID) & _
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
            sSQL = "SELECT OrderID  from " + Database + "..vwGetOrderId where CompanyID=" & Convert.ToString(User1.CompanyID) & _
                         " and ProductId=" & Session("Productid").ToString & _
                           " and AttendeeID=" & Session("AttendeeID").ToString
            Try
                NewID = CInt(DataAction.ExecuteScalar(sSQL))

            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try


            Return NewID
        End Function


#End Region


        Protected Sub RadgrdBadgeInformation_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles RadgrdBadgeInformation.ItemCreated

        End Sub

        'Protected Sub RadAjaxManager1_AjaxRequest(sender As Object, e As Telerik.Web.UI.AjaxRequestEventArgs) Handles RadAjaxManager1.AjaxRequest
        '    If e.Argument = "Rebind" Then
        '        RadgrdBadgeInformation.MasterTableView.SortExpressions.Clear()
        '        RadgrdBadgeInformation.MasterTableView.GroupByExpressions.Clear()
        '        RadgrdBadgeInformation.Rebind()
        '    ElseIf e.Argument = "RebindAndNavigate" Then
        '        RadgrdBadgeInformation.MasterTableView.SortExpressions.Clear()
        '        RadgrdBadgeInformation.MasterTableView.GroupByExpressions.Clear()
        '        RadgrdBadgeInformation.MasterTableView.CurrentPageIndex = RadgrdBadgeInformation.MasterTableView.PageCount - 1
        '        RadgrdBadgeInformation.Rebind()
        '    End If
        'End Sub


        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                SetProperties()

                ' Session("OrderDetailID") = ""
                If Not IsPostBack Then
                    'If CheckUser() = False Then
                    '    Response.Redirect("~/")
                    '    Exit Sub
                    'Else
                    Session("Productid") = ""
                    Dim ProductID As String
                    ProductID = Request.QueryString("ProductID").Trim
                    lblRegistrationResult.Text = Request.QueryString("MeetingTitle")
                    If ProductID > 0 Then
                        Session("Productid") = ProductID
                        BindEventRegDetails(ProductID)
                    End If

                    'End If
                End If
                If User1.UserID < 0 Then
                    Response.Redirect(LoginPage) 'Added by Sandeep for Issue 15051 on 12/03/2013
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Public Sub DisableRadWin()
            txtBadgeName.Enabled = False
            txtBadgeCompany.Enabled = False
            txtBadgeTitle.Enabled = False
            BtnUpdate.Visible = False
            '  BtnCancel.Visible = False
        End Sub

        Public Sub EnableRadWin()
            txtBadgeName.Enabled = True
            txtBadgeCompany.Enabled = True
            txtBadgeTitle.Enabled = True
            BtnUpdate.Visible = True
            ' BtnCancel.Visible = True
        End Sub
        Protected Sub BtnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
            UserListDialog.VisibleOnPageLoad = False
            'Dim stext As String
            'stext = "<script type='text/javascript'>Close()</" + "script>"
            txtBadgeName.Text = ""
            txtBadgeCompany.Text = ""
            txtBadgeTitle.Text = ""
            Session("Productid") = ""
            Session("AttendeeID") = ""

        End Sub

        Protected Sub BtnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnUpdate.Click
            If UpdateRegistration() Then
                UserListDialog.VisibleOnPageLoad = False
            End If
            ' Dim stext As String

            'stext = "<script type='text/javascript'>Close();</" + "script>"
        End Sub


        Protected Sub RadgrdBadgeInformation_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles RadgrdBadgeInformation.ItemCommand
            Session("AttendeeID") = ""
            Session("OrderDetailID") = ""
            Dim BadgeInfo As String
            Dim OrderDetailID As Integer
            BadgeInfo = Convert.ToString(e.CommandName)
            If BadgeInfo = "EditBagde" Then

                OrderDetailID = Convert.ToInt64(e.CommandArgument)
                Session("OrderDetailID") = Convert.ToInt64(OrderDetailID)
                BindBadgeDetails(OrderDetailID)
                EnableRadWin()

                UserListDialog.VisibleOnPageLoad = True

            ElseIf BadgeInfo <> "Filter" Then
                ' BindBadgeDetails(Session("OrderDetailID"))
                '    DisableRadWin()
            ElseIf BadgeInfo = "Page" Then
            End If

        End Sub


        Public Sub BindBadgeDetails(ByVal OrderDetailID As Integer)
            Try
                Dim sSQL As String
                Dim dt As DataTable
                Dim bShowMeeting As Boolean = ShowMeetingsLinkToClass()

                sSQL = "SELECT BadgeName, BadgeCompanyName, BadgeTitle " & _
                           "FROM " & _
                           Database & _
                           "..vwEventRegDetails where CompanyID=" & Convert.ToString(User1.CompanyID) & _
                         " and OrderDetailID=" & OrderDetailID
                If Not bShowMeeting Then
                    sSQL &= "  AND  ISNULL(vp.ClassID ,-1) <=0 "
                End If
                dt = DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)
                If dt.Rows.Count > 0 Then
                    If Not IsDBNull(dt.Rows(0)("BadgeName")) Then
                        txtBadgeName.Text = Trim(CStr(dt.Rows(0)("BadgeName")))
                    End If
                    If Not IsDBNull(dt.Rows(0)("BadgeCompanyName")) Then
                        txtBadgeCompany.Text = Trim(CStr(dt.Rows(0)("BadgeCompanyName"))) 'CStr(MeetingDetailGE.GetValue("BadgeCompanyName"))
                    End If
                    If Not IsDBNull(dt.Rows(0)("BadgeTitle")) Then
                        txtBadgeTitle.Text = Trim(CStr(dt.Rows(0)("BadgeTitle"))) 'CStr(MeetingDetailGE.GetValue("BadgeTitle"))
                    End If
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try

        End Sub


        Protected Sub RadgrdBadgeInformation_PageIndexChanged(sender As Object, e As Telerik.Web.UI.GridPageChangedEventArgs) Handles RadgrdBadgeInformation.PageIndexChanged
            RadgrdBadgeInformation.CurrentPageIndex = e.NewPageIndex
            If ViewState(ATTRIBUTE_DATATABLE_EVENTREG) IsNot Nothing Then
                RadgrdBadgeInformation.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_EVENTREG), Data.DataTable)
            End If
        End Sub

        Protected Sub RadgrdBadgeInformation_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles RadgrdBadgeInformation.NeedDataSource
            If ViewState(ATTRIBUTE_DATATABLE_EVENTREG) IsNot Nothing Then
                RadgrdBadgeInformation.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_EVENTREG), Data.DataTable)
            End If
        End Sub

        Protected Sub RadgrdBadgeInformation_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles RadgrdBadgeInformation.ItemDataBound

            Try
                Dim imgPhoto As New Image

                imgPhoto = CType(e.Item.FindControl("RadBinaryImgPhoto"), Image)

                If Not (DataBinder.Eval(e.Item.DataItem, "Photo")) Is Nothing Then

                    If IsDBNull(DataBinder.Eval(e.Item.DataItem, "Photo")) Then
                        imgPhoto.ImageUrl = "~/Images/blank photo.jpg"
                    Else
                        If (DirectCast(DataBinder.Eval(e.Item.DataItem, "Photo"), Byte()).Length() > 0) Then

                            Dim base64String As String = Convert.ToBase64String(DirectCast(DataBinder.Eval(e.Item.DataItem, "Photo"), Byte()), 0, DirectCast(DataBinder.Eval(e.Item.DataItem, "Photo"), Byte()).Length())
                            imgPhoto.ImageUrl = "data:image/png;base64," & base64String
                        Else
                            imgPhoto.ImageUrl = "~/Images/blank photo.jpg"
                        End If
                    End If
                End If

                If Not IsDBNull(DataBinder.Eval(e.Item.DataItem, "Status")) AndAlso Not (DataBinder.Eval(e.Item.DataItem, "Status")) Is Nothing Then

                    If DataBinder.Eval(e.Item.DataItem, "Status").ToString().Trim = "Active" Then
                        imgestatusID.Visible = True
                        imgestatusID.ImageUrl = "~/Images/active.png"
                    ElseIf DataBinder.Eval(e.Item.DataItem, "Status").ToString().Trim = "Expired" Then
                        imgestatusID.Visible = True
                        imgestatusID.ImageUrl = "~/Images/expire.png"
                    ElseIf DataBinder.Eval(e.Item.DataItem, "Status").ToString().Trim = "Expiring" Then
                        imgestatusID.Visible = True
                        imgestatusID.ImageUrl = "~/Images/Expiring-soon.png"

                    Else
                        imgestatusID.Visible = False

                    End If
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub BtnBack_Click(sender As Object, e As System.EventArgs) Handles BtnBack.Click
            MyBase.Response.Redirect(EditRegMeeting, False)
        End Sub

        Private Function imgestatusID() As Object
            Throw New NotImplementedException
        End Function

    End Class
End Namespace

