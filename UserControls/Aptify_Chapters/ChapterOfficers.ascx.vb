'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports System.Data
Imports Telerik.Web.UI

Namespace Aptify.Framework.Web.eBusiness.Chapters
    Partial Class ChapterOfficersControl
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CHAPTER_PAGE As String = "ChapterPage"
        Protected Const ATTRIBUTE_CHAPTER_PAGE_QUERYSTRING_NAME As String = "ChapterPageQueryStringName"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "ChapterOfficers"
        Protected Const ATTRIBUTE_DATATABLE_CHAPTER_ROLE As String = "dtChapterRole"

#Region "Chapter Specific Properties"

        Private m_sChapterPage As String
        Private m_sChapPageQueryString As String

        <System.ComponentModel.Category("Chapter Specific Properties")> _
        Public Property ChapterPage() As String
            Get
                Return m_sChapterPage
            End Get
            Set(ByVal value As String)
                m_sChapterPage = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        <System.ComponentModel.Category("Chapter Specific Properties")> _
        <System.ComponentModel.DefaultValue("ID")> _
        Public Property ChapterPageQueryStringName() As String
            Get
                If String.IsNullOrEmpty(m_sChapPageQueryString) Then
                    m_sChapPageQueryString = "ID"
                End If
                Return m_sChapPageQueryString
            End Get
            Set(ByVal value As String)
                m_sChapPageQueryString = value
            End Set
        End Property
#End Region


        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'Create an object for Commonmethods
            Dim oCommonMethods As New Aptify.Framework.Web.eBusiness.CommonMethods(DataAction, AptifyApplication, User1, Database)
            'set control properties from XML file if needed
            SetProperties()
            If Not IsPostBack Then
                'Amruta IssueID 14448
                AddExpression()
                If (Me.SetControlRecordIDFromQueryString AndAlso _
                    Me.SetControlRecordIDFromParam() AndAlso _
                    Me.ControlRecordID > 0) OrElse Me.IsPageInAdmin() Then
                    'Added by Suvarna for IssueID - 15158
                    ''verify if the logged in user is associated with that chapter and then only display the infomation,
                    ''otherwise redirect to security page.
                    If User1.UserID > 0 Then
                        If oCommonMethods.IsAuthorizedUser(User1.PersonID, Me.ControlRecordID) Then
                            LoadOfficers()
                        Else
                            Response.Redirect(Me.GetSecurityErrorPageFromXML & "?Message=Access to this Chapter is unauthorized.")
                        End If
                    End If
                Else
                    Response.Redirect(Me.GetSecurityErrorPageFromXML & "?Message=Access to this Chapter is unauthorized.")
                End If
            End If
        End Sub

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(ChapterPage) Then
                ChapterPage = Me.GetLinkValueFromXML(ATTRIBUTE_CHAPTER_PAGE)
                If String.IsNullOrEmpty(ChapterPage) Then
                    Me.lnkChapter.Enabled = False
                    Me.lnkChapter.ToolTip = "ChapterPage property has not been set."
                End If
            End If
            If ChapterPageQueryStringName = "ID" Then
                'since value is the 'default' check the XML file for possible custom setting
                If Not String.IsNullOrEmpty(Me.GetPropertyValueFromXML(ATTRIBUTE_CHAPTER_PAGE_QUERYSTRING_NAME)) Then
                    ChapterPageQueryStringName = Me.GetPropertyValueFromXML(ATTRIBUTE_CHAPTER_PAGE_QUERYSTRING_NAME)
                End If
            End If

            'if values are not provide directly or from the XML file, set default values for inherited properties since 
            'control requires them to properly function
            If String.IsNullOrEmpty(Me.QueryStringRecordIDParameter) Then Me.QueryStringRecordIDParameter = "ID"

        End Sub

        Private Sub LoadOfficers()
            Dim sSQL As String
            Dim dt As DataTable

            Try
                If ViewState(ATTRIBUTE_DATATABLE_CHAPTER_ROLE) IsNot Nothing Then
                    grdRoles.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_CHAPTER_ROLE), DataTable)
                    grdRoles.DataBind()
                    Exit Sub
                End If
                If Me.SetControlRecordIDFromParam() Then
                    'sSQL = "SELECT ChapterRoleType, Person, StartDate, EndDate FROM " & _
                    '       Database & _
                    '       "..vwChapterRoles WHERE ChapterID=" & Me.ControlRecordID.ToString

                    sSQL = "SELECT Chapter, ChapterRoleType, Person, StartDate, " & _
                            "CASE EndDate WHEN '1900-01-01 00:00:00.000' THEN NULL ELSE EndDate END 'EndDate'" & _
                            " FROM " & Database & "..vwChapterRoles WHERE ChapterID=" & Me.ControlRecordID.ToString
                    If cmbType.SelectedItem.Text = "Current" Then
                        sSQL = sSQL & " AND StartDate<=GETDATE() AND (EndDate IS NULL OR EndDate='' OR EndDate>=GETDATE())"
                    ElseIf cmbType.SelectedItem.Text = "Past" Then
                        sSQL = sSQL & " AND EndDate IS NOT NULL AND EndDate<=GETDATE() AND EndDate<>''"
                    End If

                    sSQL = sSQL & " ORDER BY ChapterRoleType,StartDate"

                    dt = DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)

                    If dt.Rows.Count > 0 Then
                        lblChapterName.Text = CStr(dt.Rows(0).Item("Chapter")) & _
                                              " Officers"
                        grdRoles.DataSource = dt
                        grdRoles.DataBind()
                        ViewState(ATTRIBUTE_DATATABLE_CHAPTER_ROLE) = dt
                        grdRoles.Visible = True
                        ' Remove error is currently displayed
                        lblError.Text = ""
                        lblError.Visible = False
                    Else
                        ' error\
                        lblError.Text = "No Chapter Roles Exist For The Specified Chapter"
                        lblError.Visible = True
                        grdRoles.Visible = False
                    End If

                Else
                    lblError.Text = "Chapter not loaded"
                    lblError.Visible = True
                    grdRoles.Visible = False
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try

        End Sub

        Private Sub cmbType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbType.SelectedIndexChanged
            '' grdRoles.Rebind()
            ViewState(ATTRIBUTE_DATATABLE_CHAPTER_ROLE) = Nothing
            LoadOfficers()
        End Sub

        Private Sub lnkChapter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkChapter.Click
            SetControlRecordIDFromParam()
            Dim sRedirect As String = ""
            If Me.EncryptQueryStringValue Then
                sRedirect = Me.ChapterPage & "?" & Me.ChapterPageQueryStringName & "=" & _
                    System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(Me.ControlRecordID.ToString))
            Else
                sRedirect = Me.ChapterPage & "?" & Me.ChapterPageQueryStringName & "=" & Me.ControlRecordID.ToString
            End If
            Response.Redirect(sRedirect)
        End Sub

        Protected Sub grdRoles_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdRoles.ItemDataBound
            'Amruta IssueID 14448, A tooltip for DatePopupButton as well as the GridDateTimeColumnStartDate textbox 
            Try
                If TypeOf e.Item Is GridFilteringItem Then
                    Dim filterItem As GridFilteringItem = DirectCast(e.Item, GridFilteringItem)
                    Dim gridDateTimeColumnStartDate As RadDatePicker = DirectCast(filterItem("GridDateTimeColumnStartDate").Controls(0), RadDatePicker)
                    gridDateTimeColumnStartDate.ToolTip = "Enter a filter date"
                    gridDateTimeColumnStartDate.DatePopupButton.ToolTip = "Select a filter date"

                    Dim gridDateTimeColumnRegisteredDate As RadDatePicker = DirectCast(filterItem("GridDateTimeColumnEndDate").Controls(0), RadDatePicker)
                    gridDateTimeColumnRegisteredDate.ToolTip = "Enter a filter date"
                    gridDateTimeColumnRegisteredDate.DatePopupButton.ToolTip = "Select a filter date"
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try

        End Sub

        Protected Sub grdRoles_PageIndexChanging(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageChangedEventArgs) Handles grdRoles.PageIndexChanged
            ''grdRoles.PageIndex = e.NewPageIndex
            If ViewState(ATTRIBUTE_DATATABLE_CHAPTER_ROLE) IsNot Nothing Then
                grdRoles.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_CHAPTER_ROLE), DataTable)
            End If
        End Sub
        Protected Sub grdRoles_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdRoles.NeedDataSource
            If ViewState(ATTRIBUTE_DATATABLE_CHAPTER_ROLE) IsNot Nothing Then
                grdRoles.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_CHAPTER_ROLE), DataTable)
            End If
        End Sub

        'Amruta Issue 14448 4/9/13 ,if the grid load first time By default the sorting will be Ascending for column Forum 
        Private Sub AddExpression()
            Dim expression1 As New GridSortExpression
            expression1.FieldName = "ChapterRoleType"
            expression1.SetSortOrder("Ascending")
            grdRoles.MasterTableView.SortExpressions.AddSortExpression(expression1)
        End Sub
    End Class
End Namespace
