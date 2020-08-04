'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports System.Data
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Telerik.Web.UI

Namespace Aptify.Framework.Web.eBusiness.Chapters
    Partial Class ChapterMeetingsControl
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CHAPTER_PAGE As String = "ChapterPage"
        Protected Const ATTRIBUTE_CHAPTER_PAGE_QUERYSTRING_NAME As String = "ChapterPageQueryStringName"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "ChapterMeetings"

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
            If String.IsNullOrEmpty(Me.RedirectURL) Then
                Me.grdMeetings.Enabled = False
                Me.grdMeetings.ToolTip = "RedirectURL property has not been set."
            End If

            'if values are not provide directly or from the XML file, set default values for inherited properties since 
            'control requires them to properly function
            If String.IsNullOrEmpty(Me.QueryStringRecordIDParameter) Then Me.QueryStringRecordIDParameter = "ID"

        End Sub

#Region "Page Specific Properties"

        Private m_sChapterPage As String
        Private m_sChapPageQueryString As String

        <System.ComponentModel.Category("Page Specific Properties")> _
        Public Property ChapterPage() As String
            Get
                Return m_sChapterPage
            End Get
            Set(ByVal value As String)
                m_sChapterPage = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        <System.ComponentModel.Category("Page Specific Properties")> _
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

        Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
            ''If Me.UseQueryString Then
            ''Me.RedirectURL = Me.RedirectURL & "?ID=-1&ChapterID=" & Request.QueryString("ID")
            ''End If
            ''Response.Redirect(Me.RedirectURL)

            SetControlRecordIDFromParam()
            Dim sRedirect As String = ""
            Dim sMtgID, sChaptID As String
            If Me.EncryptQueryStringValue Then
                sMtgID = System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt("-1"))
                sChaptID = System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(Me.ControlRecordID.ToString))
            Else
                sMtgID = "-1"
                sChaptID = Me.ControlRecordID.ToString
            End If
            sRedirect = Me.RedirectURL & "?ID=" & sMtgID & "&ChapterID=" & sChaptID
            Response.Redirect(sRedirect, False)
        End Sub
        'Navin Prasad Issue 11032
        'Private Sub grdMeetings_DeleteCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grdMeetings.DeleteCommand
        '    Dim oGE As AptifyGenericEntityBase

        '    oGE = AptifyApplication.GetEntityObject("Chapter Meetings", CLng(e.Item.Cells(0).Text))
        '    If oGE.Delete() Then
        '        LoadMeetings()
        '    Else
        '        lblError.Text = "Error Deleting Record: " & oGE.LastError
        '        lblError.Visible = True
        '    End If
        'End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'Create an object for Commonmethods
            Dim oCommonMethods As New Aptify.Framework.Web.eBusiness.CommonMethods(DataAction, AptifyApplication, User1, Database)

            'set control properties from XML file if needed
            SetProperties()

            ApplyStyles()
            lblError.Visible = False
            Try
                If Not IsPostBack Then
                    'Amruta IssueID 14448
                    AddExpression()
                    'Commented by Dipali for story no:14902
                    'If (Me.SetControlRecordIDFromQueryString AndAlso _
                    '        Me.SetControlRecordIDFromParam() AndAlso _
                    '        Me.ControlRecordID > 0) _
                    '        OrElse Me.IsPageInAdmin() Then
                    'End Commented  by Dipali for story no:14902
                    If Me.SetControlRecordIDFromQueryString AndAlso Me.SetControlRecordIDFromParam() Then
                        'Added by Suvarna for IssueID - 15158
                        ''verify if the logged in user is associated with that chapter and then only display the infomation,
                        ''otherwise redirect to security page.
                        If User1.UserID > 0 Then
                            Me.SetControlRecordIDFromParam()
                            If oCommonMethods.IsAuthorizedUser(User1.PersonID, Me.ControlRecordID) Then
                                LoadMeetings(False)
                            Else
                                Response.Redirect(Me.GetSecurityErrorPageFromXML & "?Message=Access to this Chapter Meeting is unauthorized.")
                            End If
                        End If
                    Else
                        Response.Redirect(Me.GetSecurityErrorPageFromXML & "?Message=Access to this Chapter Meeting is unauthorized.")
                    End If
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub LoadMeetings(ByVal flag As Boolean)
            Dim sSQL As String
            Dim dt As DataTable
           
            If Me.SetControlRecordIDFromParam() Then
                lblChapterName.Text = AptifyApplication.GetEntityRecordName("Companies", Me.ControlRecordID)

                sSQL = "SELECT ID, Name, Type, Description, StartDate, Status FROM " & _
                       Database & _
                       "..vwChapterMeetings WHERE " & _
                       "ChapterID=" & Me.ControlRecordID.ToString
                If cmbType.SelectedItem.Text = "Planned" Then
                    sSQL = sSQL & " AND Status='Planned'"
                ElseIf cmbType.SelectedItem.Text = "Past" Then
                    sSQL = sSQL & " AND Status<>'Planned'"
                End If
                sSQL = sSQL & " ORDER BY StartDate DESC,Name"

                dt = DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                'Navin Prasad Issue 11032
                ' DirectCast(grdMeetings.Columns(2), HyperLinkColumn).DataNavigateUrlFormatString = Me.RedirectURL & "?" & "ID={0}"
                'Dim ID As String

                'Dim hlink As Telerik.Web.UI.GridHyperLinkColumn = CType(grdMeetings.Columns(2), Telerik.Web.UI.GridHyperLinkColumn)
                'ID = hlink.DataNavigateUrlFields(CInt("ID"))
                'hlink.DataNavigateUrlFormatString = Me.RedirectURL & "?" & "ID={0}" & "&ChapterID=" + System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(Me.ControlRecordID.ToString))


                'Added by Dipali for story No 14902
                If dt.Rows.Count >= 1 Then

                    Dim dcolUrle As DataColumn = New DataColumn()
                    dcolUrle.Caption = "MeetingTitleUrl"
                    dcolUrle.ColumnName = "MeetingTitleUrl"

                    dt.Columns.Add(dcolUrle)
                    If dt.Rows.Count > 0 Then

                        For Each rw As DataRow In dt.Rows
                            rw("MeetingTitleUrl") = Me.RedirectURL + "?ID=" + System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(rw("ID").ToString)) + "&ChapterID=" + System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(Me.ControlRecordID.ToString))
                        Next
                    End If
                    'End

                    grdMeetings.DataSource = dt

                    If flag Then
                        grdMeetings.DataBind()
                    End If
                    lblNoMeetings.Visible = False
                    grdMeetings.Visible = True
                Else
                    lblNoMeetings.Visible = True
                    grdMeetings.Visible = False
                End If
            Else
                lblError.Text = "There was a problem loading the Chapter information."
                lblError.Visible = True
                lblNoMeetings.Visible = True
                grdMeetings.Visible = False
            End If

        End Sub

        'Private Sub LoadMeetings()
        '    Dim sSQL As String
        '    Dim dt As DataTable
        '    'Dim dcolUrle As DataColumn = New DataColumn()
        '    'dcolUrle.Caption = "MeetingTitleUrl"
        '    'dcolUrle.ColumnName = "MeetingTitleUrl"
        '    If Me.SetControlRecordIDFromParam() Then
        '        lblChapterName.Text = AptifyApplication.GetEntityRecordName("Companies", Me.ControlRecordID)

        '        sSQL = "SELECT ID, Name, Type, Description, StartDate, Status FROM " & _
        '               Database & _
        '               "..vwChapterMeetings WHERE " & _
        '               "ChapterID=" & Me.ControlRecordID.ToString
        '        If cmbType.SelectedItem.Text = "Planned" Then
        '            sSQL = sSQL & " AND Status='Planned'"
        '        ElseIf cmbType.SelectedItem.Text = "Past" Then
        '            sSQL = sSQL & " AND Status<>'Planned'"
        '        End If
        '        sSQL = sSQL & " ORDER BY StartDate DESC,Name"

        '        dt = DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
        '        ''Navin Prasad Issue 11032
        '        '' DirectCast(grdMeetings.Columns(2), HyperLinkColumn).DataNavigateUrlFormatString = Me.RedirectURL & "?" & "ID={0}"

        '        Dim hlink As Telerik.Web.UI.GridHyperLinkColumn = CType(grdMeetings.Columns(2), Telerik.Web.UI.GridHyperLinkColumn)
        '        hlink.DataNavigateUrlFormatString = Me.RedirectURL & "?" & "ID={0}" & "&ChapterID=" + Me.ControlRecordID.ToString



        '        'If dt.Rows.Count >= 1 Then
        '        '    dt.Columns.Add(dcolUrle)
        '        '    If dt.Rows.Count > 0 Then

        '        '        For Each rw As DataRow In dt.Rows
        '        '            rw("MeetingTitleUrl") = Me.RedirectURL + "?ID=" + rw("ID").ToString + "&ChapterID=" + Me.ControlRecordID.ToString
        '        '        Next
        '        '    End If


        '        'DirectCast(grdMeetings.Columns(2), HyperLinkColumn).DataNavigateUrlFormatString = Me.RedirectURL & "?" & "ID={0}"
        '        grdMeetings.DataSource = dt
        '        grdMeetings.DataBind()
        '        lblNoMeetings.Visible = False
        '        grdMeetings.Visible = True
        '    Else
        '        lblNoMeetings.Visible = True
        '        grdMeetings.Visible = False
        '    End If
        '    Else
        '    lblError.Text = "There was a problem loading the Chapter information."
        '    lblError.Visible = True
        '    lblNoMeetings.Visible = True
        '    grdMeetings.Visible = False
        '    End If

        'End Sub

        Private Sub cmbType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbType.SelectedIndexChanged

            LoadMeetings(True)

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

        Protected Sub grdMeetings_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grdMeetings.ItemCommand
            If e.CommandName = "Delete" Then
                Dim oGE As AptifyGenericEntityBase
                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                grdMeetings.Items(index).FindControl("index")
                'Dim lbl As Label = CType(grdMeetings.SelectedRow.FindControl("index"), Label)
                Dim lbl As Label = CType(grdMeetings.Items(index).FindControl("lblID"), Label)
                oGE = AptifyApplication.GetEntityObject("Chapter Meetings", CLng(lbl.Text))
                If oGE.Delete() Then
                    LoadMeetings(False)
                Else
                    lblError.Text = "Error Deleting Record: " & oGE.LastError
                    lblError.Visible = True
                    lblError.ForeColor = Drawing.Color.Red

                End If
            End If

        End Sub

        Protected Sub grdMeetings_PageIndexChanging(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageChangedEventArgs) Handles grdMeetings.PageIndexChanged
            ''grdMeetings.PageIndex = e.NewPageIndex
            LoadMeetings(False)
        End Sub
        'Navin Prasad Issue 11032
        'Protected Sub grdMeetings_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdMeetings.ItemDataBound
        '    Try
        '        If Me.EncryptQueryStringValue Then
        '            Dim type As ListItemType = e.Item.ItemType
        '            If e.Item.ItemType = ListItemType.Item Or _
        '                    e.Item.ItemType = ListItemType.AlternatingItem Then
        '                Dim lnk As HyperLink
        '                Dim tempURL As String
        '                Dim index As Integer
        '                Dim sValue As String = "0"
        '                Dim separator As String()

        '                lnk = CType(e.Item.Cells(2).Controls(0), HyperLink)
        '                tempURL = lnk.NavigateUrl
        '                index = tempURL.IndexOf("=")
        '                sValue = tempURL.Substring(index + 1)
        '                separator = lnk.NavigateUrl.Split(CChar("="))
        '                lnk.NavigateUrl = separator(0)
        '                lnk.NavigateUrl = lnk.NavigateUrl & "="
        '                lnk.NavigateUrl = lnk.NavigateUrl & System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(sValue))
        '            End If
        '        End If
        '    Catch ex As Exception
        '        Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        '    End Try
        'End Sub



        'Commented by Dipali on 21-Oct-2012
        'Protected Sub grdMeetings_RowDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdMeetings.ItemDataBound
        '    Try
        '        'If Me.EncryptQueryStringValue Then
        '        '    'Dim type As ListItemType = CType(e.Row.RowType, ListItemType)
        '        '    'If CType(e.Row.RowType, ListItemType) = ListItemType.Item Or _
        '        '    '        CType(e.Row.RowType, ListItemType) = ListItemType.AlternatingItem Then
        '        '    If e.Item.ItemType = Telerik.Web.UI.GridItemType.AlternatingItem Or e.Item.ItemType = Telerik.Web.UI.GridItemType.Item Then
        '        '        Dim lnk As HyperLink
        '        '        Dim tempURL As String
        '        '        Dim index As Integer
        '        '        Dim sValue As String = "0"
        '        '        Dim separator As String()

        '        '        lnk = CType(e.Item.Cells(2).Controls(0), HyperLink)
        '        '        tempURL = lnk.NavigateUrl
        '        '        index = tempURL.IndexOf("=")
        '        '        sValue = tempURL.Substring(index + 1)
        '        '        separator = lnk.NavigateUrl.Split(CChar("="))
        '        '        lnk.NavigateUrl = separator(0)
        '        '        lnk.NavigateUrl = lnk.NavigateUrl & "="
        '        '        lnk.NavigateUrl = lnk.NavigateUrl & System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(sValue))
        '        '        'DirectCast(grdMeetings.Columns(2), HyperLinkColumn).DataNavigateUrlFormatString = Me.RedirectURL & "?" & "ID={0}"
        '        '    End If
        '        'End If
        '    Catch ex As Exception
        '        Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        '    End Try
        'End Sub

        Protected Sub grdMeetings_RowDeleting(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridDeletedEventArgs) Handles grdMeetings.ItemDeleted

        End Sub

        'Amruta Issue 14448 4/10/13 ,if the grid load first time By default the sorting will be Ascending for column Name 
        Private Sub AddExpression()
            Dim expression1 As New GridSortExpression
            expression1.FieldName = "Name"
            expression1.SetSortOrder("Ascending")
            grdMeetings.MasterTableView.SortExpressions.AddSortExpression(expression1)
        End Sub

        Protected Sub grdMeetings_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdMeetings.NeedDataSource
            LoadMeetings(False)
        End Sub

        Protected Sub grdMeetings_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdMeetings.ItemDataBound
            Try
                'Amruta IssueID 14448,For dateformate and a tooltip for DatePopupButton as well as the GridDateTimeColumnStartDate textbox 
                If (TypeOf (e.Item) Is GridDataItem) Then
                    Dim dateColumns As New List(Of String)
                    'Add datecolumn uniqueName in list for Date format
                    dateColumns.Add("GridDateTimeColumnStartDate")
                    CommonMethods.FormatedDateOnGrid(dateColumns, e.Item)
                End If

                If TypeOf e.Item Is GridFilteringItem Then
                    Dim filterItem As GridFilteringItem = DirectCast(e.Item, GridFilteringItem)
                    Dim gridDateTimeColumnStartDate As RadDatePicker = DirectCast(filterItem("GridDateTimeColumnStartDate").Controls(0), RadDatePicker)
                    gridDateTimeColumnStartDate.ToolTip = "Enter a filter date"
                    gridDateTimeColumnStartDate.DatePopupButton.ToolTip = "Select a filter date"
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
    End Class
End Namespace
