'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports System.Data
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Telerik.Web.UI

Namespace Aptify.Framework.Web.eBusiness.Chapters
    Partial Class ChapterControl
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_REPORTS_PAGE As String = "ChapterReportsPage"
        Protected Const ATTRIBUTE_CHAPTER_EDIT_PAGE As String = "ChaperEditPage"
        Protected Const ATTRIBURE_CHAPTER_OFFICERS_PAGE As String = "ChapterOfficersPage"
        Protected Const ATTRIBUTE_CHAPTER_MEETINGS_PAGE As String = "ChapterMeetingsPage"
        Protected Const ATTRIBUTE_CHAPTER_MEMBERS_PAGE As String = "ChapterMembersPage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "Chapter"
        Protected Const ATTRIBUTE_SECURITYERROR_PAGE As String = "securityErrorPage"
        Protected Const ATTRIBUTE_LOGIN_PAGE As String = "LoginPage"
        Protected Const ATTRIBUTE_DATATABLE_CHAPTER_MEMBER As String = "dtChapterMember"


#Region "Chapter Specific Properties"

        <System.ComponentModel.Category("Chapter Specific Properties")> _
        Public Property ChapterReportsPage() As String
            Get
                If Not ViewState(ATTRIBUTE_REPORTS_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_REPORTS_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_REPORTS_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        <System.ComponentModel.Category("Chapter Specific Properties")> _
        Public Property ChaperEditPage() As String
            Get
                If Not ViewState(ATTRIBUTE_CHAPTER_EDIT_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_CHAPTER_EDIT_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_CHAPTER_EDIT_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        <System.ComponentModel.Category("Chapter Specific Properties")> _
        Public Property ChapterOfficersPage() As String
            Get
                If Not ViewState(ATTRIBURE_CHAPTER_OFFICERS_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBURE_CHAPTER_OFFICERS_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBURE_CHAPTER_OFFICERS_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        <System.ComponentModel.Category("Chapter Specific Properties")> _
        Public Property ChapterMeetingsPage() As String
            Get
                If Not ViewState(ATTRIBUTE_CHAPTER_MEETINGS_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_CHAPTER_MEETINGS_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_CHAPTER_MEETINGS_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        <System.ComponentModel.Category("Chapter Specific Properties")> _
        Public Property ChapterMembersPage() As String
            Get
                If Not ViewState(ATTRIBUTE_CHAPTER_MEMBERS_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_CHAPTER_MEMBERS_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_CHAPTER_MEMBERS_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        Public Overridable ReadOnly Property securityErrorPage() As String
            Get
                If Not ViewState.Item(ATTRIBUTE_SECURITYERROR_PAGE) Is Nothing Then
                    Return CStr(ViewState.Item(ATTRIBUTE_SECURITYERROR_PAGE))
                Else
                    
                    'Suraj Issue 13029 take a sequrity page url from web.config file
                    Dim value As String = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings("SecurityErrorPageURL"))
                    If Not String.IsNullOrEmpty(value) Then
                        ViewState.Item(ATTRIBUTE_SECURITYERROR_PAGE) = value
                        Return value
                    Else
                        Return ""
                    End If
                End If
            End Get

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

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'Create an object for Commonmethods
            Dim oCommonMethods As New Aptify.Framework.Web.eBusiness.CommonMethods(DataAction, AptifyApplication, User1, Database)
            'set inherited control properties from XML file if needed
            SetProperties()
            If Not IsPostBack Then
                'Amruta IssueID 14448
                AddExpression()
                If User1.UserID > 0 Then
                    'Added by Dipali on 12/13/12 - Story No 13234,
                    ''Added by Suvarna for IssueID - 15158
                    ''verify if the logged in user is associated with that chapter and then only display the infomation,
                    ''otherwise redirect to security page.
                    If Me.SetControlRecordIDFromQueryString AndAlso Me.SetControlRecordIDFromParam() AndAlso Me.ControlRecordID > 0 Then
                        If oCommonMethods.IsAuthorizedUser(User1.PersonID, Me.ControlRecordID) Then
                            DoLoad(True, True)
                        Else
                            Response.Redirect(System.Configuration.ConfigurationManager.AppSettings("virtualdir") & securityErrorPage & "?Message=Access to this Chapter is unauthorized.")
                        End If
                    Else
                        Response.Redirect(System.Configuration.ConfigurationManager.AppSettings("virtualdir") & securityErrorPage & "?Message=Access to this Chapter is unauthorized.")
                    End If
                Else
                    'Suraj Issue 15370,7/31/13 revert back changes application variale to session variable
                    Session("ReturnToPage") = Request.RawUrl
                    Response.Redirect(Me.LoginPage + "?ReturnURL=" & System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(Request.RawUrl)))

                End If


            End If

        End Sub

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(ChapterReportsPage) Then
                ChapterReportsPage = Me.GetLinkValueFromXML(ATTRIBUTE_REPORTS_PAGE)
                If String.IsNullOrEmpty(ChapterReportsPage) Then
                    Me.lnkReports.Enabled = False
                    Me.lnkReports.ToolTip = "ChapterReportsPage property has not been set."
                Else
                    Me.lnkReports.NavigateUrl = ChapterReportsPage
                End If
            Else
                Me.lnkReports.NavigateUrl = ChapterReportsPage
            End If

            If String.IsNullOrEmpty(ChaperEditPage) Then
                ChaperEditPage = Me.GetLinkValueFromXML(ATTRIBUTE_CHAPTER_EDIT_PAGE)
                If String.IsNullOrEmpty(ChaperEditPage) Then
                    Me.lnkLocation.Enabled = False
                    Me.lnkLocation.ToolTip = "ChaperEditPage property has not been set."
                Else
                    Me.lnkLocation.NavigateUrl = ChaperEditPage
                End If
            Else
                Me.lnkLocation.NavigateUrl = ChaperEditPage
            End If

            If String.IsNullOrEmpty(ChapterMeetingsPage) Then
                ChapterMeetingsPage = Me.GetLinkValueFromXML(ATTRIBUTE_CHAPTER_MEETINGS_PAGE)
                If String.IsNullOrEmpty(ChapterMeetingsPage) Then
                    Me.lnkMeetings.Enabled = False
                    Me.lnkMeetings.ToolTip = "ChapterMeetingsPage property has not been set."
                Else
                    Me.lnkMeetings.NavigateUrl = ChapterMeetingsPage
                End If
            Else
                Me.lnkMeetings.NavigateUrl = ChapterMeetingsPage
            End If

            If String.IsNullOrEmpty(ChapterOfficersPage) Then
                ChapterOfficersPage = Me.GetLinkValueFromXML(ATTRIBURE_CHAPTER_OFFICERS_PAGE)
                If String.IsNullOrEmpty(ChapterOfficersPage) Then
                    Me.lnkOfficers.Enabled = False
                    Me.lnkOfficers.ToolTip = "ChapterOfficersPage property has not been set."
                Else
                    Me.lnkOfficers.NavigateUrl = ChapterOfficersPage
                End If
            Else
                Me.lnkOfficers.NavigateUrl = ChapterOfficersPage
            End If

            If String.IsNullOrEmpty(ChapterMembersPage) Then
                ChapterMembersPage = Me.GetLinkValueFromXML(ATTRIBUTE_CHAPTER_MEMBERS_PAGE)
                If String.IsNullOrEmpty(ChapterMembersPage) Then
                    Me.grdMembers.Enabled = False
                    Me.grdMembers.ToolTip = "ChapterMembersPage property has not been set."
                End If
            End If
            If String.IsNullOrEmpty(Me.RedirectURL) Then
                Me.cmdNew.Enabled = False
                Me.cmdNew.ToolTip = "RedirectURL property has not been set."
            End If
            'Suraj Issue 13234
            If String.IsNullOrEmpty(Me.LoginPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                Me.LoginPage = Me.GetLinkValueFromXML(ATTRIBUTE_LOGIN_PAGE)
            End If
            'if values are not provide directly or from the XML file, set default values for inherited properties since 
            'control requires them to properly function
            If String.IsNullOrEmpty(Me.QueryStringRecordIDParameter) Then Me.QueryStringRecordIDParameter = "ID"
            If String.IsNullOrEmpty(Me.RedirectIDParameterName) Then Me.RedirectIDParameterName = "ID"
        End Sub

        Private Sub DoLoad(ByVal bLoadAddressInfo As Boolean, ByVal bLoadChapterMember As Boolean)
            Me.SetControlRecordIDFromParam()
            InternalLoad(bLoadAddressInfo, bLoadChapterMember)
        End Sub

        Private Sub InternalLoad(ByVal bLoadAddressInfo As Boolean, ByVal bLoadChapterMember As Boolean)
            Dim lChapterID As Long, sChapterID As String

            lChapterID = Me.ControlRecordID

            If bLoadAddressInfo Then
                LoadAddressInfo(lChapterID)
            End If

            If Me.EncryptQueryStringValue Then
                sChapterID = System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(CStr(lChapterID)))
            Else
                sChapterID = CStr(lChapterID)
            End If

            LoadMembers(lChapterID, bLoadChapterMember)

            lnkReports.NavigateUrl &= "?ID=" & sChapterID
            lnkLocation.NavigateUrl &= "?ID=" & sChapterID
            lnkOfficers.NavigateUrl &= "?ID=" & sChapterID
            lnkMeetings.NavigateUrl &= "?ID=" & sChapterID


        End Sub

        Private Sub LoadAddressInfo(ByVal lChapterID As Long)
            Dim sSQL As String, dt As DataTable

            Try
                sSQL = "SELECT Name,AddressID, AddressLine1,AddressLine2," & _
                       "AddressLine3,City,State,ZipCode,Country FROM " & _
                       Database & _
                       "..vwCompanies WHERE ID=" & lChapterID
                dt = DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                If dt.Rows.Count = 1 Then
                    lblChapterName.Text = CStr(dt.Rows(0).Item("Name"))
                    If Not IsDBNull(dt.Rows(0).Item("AddressID")) Then
                        blkAddress.AddressLine1 = CStr(dt.Rows(0).Item("AddressLine1"))
                        blkAddress.AddressLine2 = CStr(dt.Rows(0).Item("AddressLine2"))
                        blkAddress.AddressLine3 = CStr(dt.Rows(0).Item("AddressLine3"))
                        blkAddress.City = CStr(dt.Rows(0).Item("City"))
                        blkAddress.State = CStr(dt.Rows(0).Item("State"))
                        blkAddress.ZipCode = CStr(dt.Rows(0).Item("ZipCode"))
                        blkAddress.Country = CStr(dt.Rows(0).Item("Country"))
                    End If
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub LoadMembers(ByVal lChapterID As Long, ByVal bLoadChapterMember As Boolean)
            Dim sSQL As String
            Dim dt As DataTable

            Try
                If ViewState(ATTRIBUTE_DATATABLE_CHAPTER_MEMBER) IsNot Nothing Then
                    grdMembers.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_CHAPTER_MEMBER), DataTable)
                    grdMembers.DataBind()
                Else
                    sSQL = Database & _
                           "..spGetChapterMembers @ChapterID=" & lChapterID
                    dt = DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                    ViewState(ATTRIBUTE_DATATABLE_CHAPTER_MEMBER) = dt
                    grdMembers.DataSource = dt

                    grdMembers.DataBind()
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub grdMembers_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdMembers.NeedDataSource
            Dim lChapterID As Long
            lChapterID = Me.ControlRecordID
            'Suraj Issue 15154, 4/17/13, this method used to Descript the query string
            DoLoad(True, True)
            If ViewState(ATTRIBUTE_DATATABLE_CHAPTER_MEMBER) IsNot Nothing Then
                grdMembers.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_CHAPTER_MEMBER), DataTable)
            End If
        End Sub

        Protected Sub grdMembers_PageIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageChangedEventArgs) Handles grdMembers.PageIndexChanged
            '' grdMembers.PageIndex = e.NewPageIndex
            Dim lChapterID As Long
            lChapterID = Me.ControlRecordID
            'Suraj Issue 15154, 4/17/13, this method used to Descript the query string
            DoLoad(True, True)
            If ViewState(ATTRIBUTE_DATATABLE_CHAPTER_MEMBER) IsNot Nothing Then
                grdMembers.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_CHAPTER_MEMBER), DataTable)
            End If
        End Sub

        'CP 2/2/09 ISSUE 8214. Remove chapter edit control.
        Private Sub grdMembers_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles grdMembers.SelectedIndexChanged
            LoadChapterMember()
        End Sub

        'CP 2/2/09 ISSUE 8214. Remove chapter edit control.
        Private Sub LoadChapterMember()
            Try


                If Not (grdMembers.SelectedIndexes Is Nothing) Then
                    'Suraj Issue 15370,7/31/13 revert back changes application variale to session variable
                    Session("ReturnToPage") = Request.RawUrl
                    Dim btnlnk As LinkButton
                    For Each item As GridDataItem In grdMembers.SelectedItems
                        btnlnk = CType(item.FindControl("btnSelect"), LinkButton)
                        Response.Redirect(ChapterMembersPage & "?MemberID=" & System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(btnlnk.Text)))

                    Next

                    ''Dim btnlnk As LinkButton = CType(grdMembers.SelectedIndexes.FindControl("btnSelect"), LinkButton)
                    'ChapterMember1.LoadMember(CLng(grdMembers.SelectedItem.Cells(5).Text))
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub


        'CP 2/2/09 ISSUE 8214. Remove chapter edit control.
        'Private Sub ChapterMember1_Saved() Handles ChapterMember1.Saved
        '    Dim iIndex As Integer

        '    Try
        '        iIndex = grdMembers.SelectedIndex
        '        DoLoad(False, False)
        '        grdMembers.SelectedIndex = iIndex
        '        LoadChapterMember()
        '    Catch ex As Exception
        '        Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        '    End Try
        'End Sub

        Private Sub cmdNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNew.Click
            Try
                Me.RedirectUsingPropertyValues(Me.ControlRecordID)
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        'Private Sub LoadMembers()
        '    Throw New NotImplementedException
        'End Sub

        'Amruta Issue 14448 4/9/13 ,if the grid load first time By default the sorting will be Ascending for column Forum 
        Private Sub AddExpression()
            Dim expression1 As New GridSortExpression
            expression1.FieldName = "ID"
            expression1.SetSortOrder("Ascending")
            grdMembers.MasterTableView.SortExpressions.AddSortExpression(expression1)
        End Sub
    End Class
End Namespace
