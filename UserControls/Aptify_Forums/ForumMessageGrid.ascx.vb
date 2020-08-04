'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports System.Data
Imports Aptify.Framework.BusinessLogic.GenericEntity

Namespace Aptify.Framework.Web.eBusiness.Forums
    Partial Class ForumMessageGrid
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_EXPAND_IMAGE_URL As String = "GridExpandImage"
        Protected Const ATTRIBUTE_COLLAPSE_IMAGE_URL As String = "GridCollapseImage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "ForumMessageGrid"

        Public Event MessageSelected(ByVal MessageID As Long)
        Public Event NewMessage()
        Public Event ReplyToMessage(ByVal MessageID As Long)

#Region "ForumMessageGrid Specific Properties"
        ''' <summary>
        ''' GridExpandImage url
        ''' </summary>
        Public Overridable Property GridExpandImage() As String
            Get
                If Not ViewState(ATTRIBUTE_EXPAND_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_EXPAND_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_EXPAND_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' GridCollapseImage url
        ''' </summary>
        Public Overridable Property GridCollapseImage() As String
            Get
                If Not ViewState(ATTRIBUTE_COLLAPSE_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_COLLAPSE_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_COLLAPSE_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        Public ReadOnly Property SelectedMessageID() As Long
            Get
                If grdMain.SelectedIndex >= 0 Then
                    'Navin Prasad Issue 11032
                    Dim lbl As Label = CType(grdMain.SelectedRow.FindControl("lblID"), Label)
                    Return CLng(lbl.Text)
                End If
            End Get
        End Property

        Private Property ForumID() As Long
            Get
                If ViewState.Item("ForumID") IsNot Nothing Then
                    Return CLng(ViewState.Item("ForumID"))
                Else
                    Return -1
                End If
            End Get
            Set(ByVal value As Long)
                ViewState.Item("ForumID") = value
            End Set
        End Property
#End Region

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(GridExpandImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                GridExpandImage = Me.GetLinkValueFromXML(ATTRIBUTE_EXPAND_IMAGE_URL)
            End If
            If String.IsNullOrEmpty(GridCollapseImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                GridCollapseImage = Me.GetLinkValueFromXML(ATTRIBUTE_COLLAPSE_IMAGE_URL)
            End If

        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'set control properties from XML file if needed
            SetProperties()
            If Not IsPostBack Then
                If Me.SetControlRecordIDFromQueryString AndAlso _
                    Me.SetControlRecordIDFromParam() Then
                    LoadHeader()
                    LoadGrid(Me.ControlRecordID)
                End If
            End If
        End Sub

        '01/29/08 Added by tamasa for issue 5330 to support access. Extended functionality
        Private Function GetAccessInformation() As String
            Dim sSQL As String
            Dim sAccess As String
            Try
                sSQL = "Select Access from " & Database & "..vwDiscussionForums Where ID =" & Me.ForumID.ToString()
                sAccess = DataAction.ExecuteScalar(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache).ToString
                If Not sAccess Is Nothing Then
                    Return sAccess
                Else
                    Return ""
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return ""
            End Try
        End Function
        'End

        Private Sub LoadHeader()
            Dim sSQL As String
            Dim dt As DataTable

            sSQL = "Select ID, Name, Description from " & Me.AptifyApplication.GetEntityBaseDatabase("Discussion Forums") & _
                    ".dbo.vwDiscussionForums where id = " & Me.ControlRecordID.ToString
            dt = Me.DataAction.GetDataTable(sSQL)

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                'lblForumName.Text = "Forum: " & dt.Rows(0).Item("Name").ToString()
                'lblDescription.Text = "Description: " & dt.Rows(0).Item("Description").ToString()

            End If
        End Sub

        Public Sub LoadGrid(ByVal lForumID As Long)
            Dim sSQL As String
            Dim dt As DataTable
            Dim m_sAccess As String

            Try
                Me.ForumID = lForumID

                '01/29/08 Added by tamasa, for issue 5330 enhancement.
                m_sAccess = Me.GetAccessInformation()
                Select Case m_sAccess.Trim()
                    Case "Anonymous"
                        InnerLoadGrid()
                        If Me.User1.UserID > 0 Then
                            Me.cmdNewPost.Visible = True
                        Else
                            Me.cmdNewPost.Visible = False
                            Me.cmdReply.Visible = False
                        End If
                    Case "Registered"
                        If Me.User1.UserID > 0 Then
                            InnerLoadGrid()
                            Me.cmdNewPost.Visible = True
                        End If
                    Case "Restricted"
                        sSQL = Database & "..spGetUserDiscussionForumPermissions @DiscussionForumID=" & _
                       lForumID & ", @WebUserID=" & Me.User1.UserID
                        dt = DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)

                        If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                            With dt.Rows(0)
                                If CBool(.Item("CanRead")) Then
                                    InnerLoadGrid()
                                    Me.cmdNewPost.Visible = CBool(.Item("CanPost"))
                                    Me.cmdReply.Visible = CBool(.Item("CanReply"))
                                Else
                                    Me.cmdNewPost.Visible = False
                                    Me.cmdReply.Visible = False
                                    Me.grdMain.Visible = False
                                    Me.cmbViewType.Visible = False
                                End If
                            End With
                        End If
                End Select
                'End
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Protected Overridable Sub InnerLoadGrid()
            Dim sSQL As String
            Dim dt As DataTable
            Dim iRow As Integer
            Dim iRowsInserted As Integer

            Try
                sSQL = "SELECT ID,'' Spaces,Subject,WebUserName,WebUserNameWCompany,DateEntered,ChildCount,-1 ParentPos,0 Expanded " & _
                           "FROM " & Database & "..vwDiscussionForumMessages " & _
                           "WHERE ForumID=" & Me.ForumID.ToString() & " "
                sSQL = sSQL & " AND Status='Posted'"

                sSQL = sSQL & _
                        " And ForumID in (Select Id from vwDiscussionForums " & _
                        " where Status='Active' " & _
                        " AND (StartDate<=GetDate() or convert(nvarchar(20),StartDate,112)=19000101) " & _
                        " AND (EndDate>=GETDATE() or convert(nvarchar(20),EndDate,112)=19000101)) "

                If cmbViewType.SelectedItem.Text = "Threaded View" Then
                    ' if in threaded view, only show top level messages,
                    ' otherwise, show all messages
                    sSQL = sSQL & " AND ISNULL(ParentID,-1)=-1 "
                End If
                sSQL = sSQL & "ORDER BY DateEntered DESC"

                dt = DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                'Added for issue 5330
                If dt.Rows.Count > 0 Then
                    grdMain.Visible = True
                Else
                    grdMain.Visible = False
                End If
                'End
                If cmbViewType.SelectedItem.Text = "Threaded View" Then
                    grdMain.Columns(0).Visible = True

                    iRow = 0
                    While iRow < dt.Rows.Count
                        If CInt(dt.Rows(iRow).Item("ChildCount")) > 0 Then
                            iRowsInserted = 0
                            LoadSubRows(dt, iRow, CLng(dt.Rows(iRow).Item("ID")), 1, iRowsInserted, iRow)
                            '2009-02-10 MAS: Hack to fix reply count
                            dt.Rows(iRow).Item("ChildCount") = iRowsInserted
                            iRow += 1 + iRowsInserted
                        Else
                            iRow = iRow + 1
                        End If
                    End While
                Else
                    grdMain.Columns(0).Visible = False

                    '2009-02-10 MAS: Hack to fix reply count
                    'iRow = 0
                    'While iRow < dt.Rows.Count
                    For Each row As DataRow In dt.Rows
                        If CInt(row.Item("ChildCount")) > 0 Then
                            Dim count As Integer = 0
                            GetChildCount(CLng(row.Item("ID")), count)
                            '2009-02-10 MAS: Hack to fix reply count
                            row.Item("ChildCount") = count
                        End If
                        'End While
                    Next
                End If
                'Navin Prasad Issue 11032

                Dim arr(0) As String
                arr(0) = "ID"
                grdMain.DataKeyNames = arr
                grdMain.DataSource = dt
                grdMain.DataBind()

                If dt.Rows.Count > 0 Then
                    Dim i As Integer
                    'Navin Prasad Issue 11032
                    For i = 0 To grdMain.Rows.Count - 1
                        Dim lbl As Label = CType(grdMain.Rows(i).FindControl("lblParentPos"), Label)
                        If Val(lbl.Text) >= 0 Then
                            grdMain.Rows(i).Visible = False
                        End If
                    Next
                    grdMain.SelectedIndex = 0
                    RaiseEvent MessageSelected(Me.SelectedMessageID)
                    cmdReply.Visible = True
                Else
                    cmdReply.Visible = False
                    RaiseEvent MessageSelected(-1)
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Private Sub LoadSubRows(ByVal dt As DataTable, _
                                ByVal InsertPos As Integer, _
                                ByVal ParentID As Long, _
                                ByVal Depth As Integer, _
                                ByRef NumRowsInserted As Integer, _
                                ByVal ParentPos As Integer)
            Dim i As Integer
            Dim dtSub As DataTable
            Dim dr As DataRow
            Dim sSQL As String
            Dim sSpaces As String

            Try
                sSQL = "SELECT ID,Subject,WebUserName,WebUserNameWCompany,DateEntered,ChildCount " & _
                       "FROM " & Database & "..vwDiscussionForumMessages " & _
                       "WHERE ParentID=" & ParentID & " AND Status='Posted' ORDER BY DateEntered DESC"
                dtSub = DataAction.GetDataTable(sSQL)
                sSpaces = ""
                For i = 1 To Depth
                    sSpaces = sSpaces & "&nbsp;&nbsp;&nbsp;"
                Next
                For i = 0 To dtSub.Rows.Count - 1
                    dr = dt.NewRow()
                    dr.Item("ID") = dtSub.Rows(i).Item("ID")
                    dr.Item("Spaces") = sSpaces
                    dr.Item("Subject") = dtSub.Rows(i).Item("Subject")
                    dr.Item("WebUserName") = dtSub.Rows(i).Item("WebUserName")
                    dr.Item("WebUserNameWCompany") = dtSub.Rows(i).Item("WebUserNameWCompany")
                    dr.Item("DateEntered") = dtSub.Rows(i).Item("DateEntered")
                    dr.Item("ChildCount") = dtSub.Rows(i).Item("ChildCount")
                    dr.Item("ParentPos") = ParentPos
                    dr.Item("Expanded") = 0
                    NumRowsInserted += 1
                    dt.Rows.InsertAt(dr, InsertPos + NumRowsInserted)
                    If CInt(dr.Item("ChildCount")) > 0 Then
                        LoadSubRows(dt, InsertPos, CLng(dr.Item("ID")), Depth + 1, NumRowsInserted, InsertPos + NumRowsInserted)
                    End If
                Next
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub GetChildCount(ByVal ParentID As Long, _
                                  ByRef Count As Integer)
            Dim i As Integer
            Dim dtSub As DataTable
            Dim sSQL As String
            Try
                sSQL = "SELECT ID,ChildCount " & _
                       "FROM " & Database & "..vwDiscussionForumMessages " & _
                       "WHERE ParentID=" & ParentID & " AND Status='Posted' ORDER BY DateEntered DESC"
                dtSub = DataAction.GetDataTable(sSQL)
                For i = 0 To dtSub.Rows.Count - 1
                    Count += 1
                    If CInt(dtSub.Rows(i).Item("ChildCount")) > 0 Then
                        GetChildCount(CLng(dtSub.Rows(i).Item("ID")), Count)
                    End If
                Next
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub grdMain_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles grdMain.SelectedIndexChanged
            cmdReply.Visible = True
            RaiseEvent MessageSelected(Me.SelectedMessageID)
        End Sub

        Private Sub cmdNewPost_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNewPost.Click
            RaiseEvent NewMessage()
        End Sub

        Private Sub cmdReply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReply.Click
            RaiseEvent ReplyToMessage(Me.SelectedMessageID)
        End Sub


        Private Sub cmbViewType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbViewType.SelectedIndexChanged
            LoadGrid(Me.ForumID)
        End Sub
        'Navin Prasad Issue 11032

        'Private Sub grdMain_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles grdMain.PageIndexChanged
        '    LoadGrid(Me.ForumID)
        'End Sub

        'Private Sub grdMain_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grdMain.ItemCommand
        '    Select Case e.CommandName
        '        Case "Expand"
        '            ShowChildRows(e.Item.ItemIndex, True, False)
        '        Case "Collapse"
        '            ShowChildRows(e.Item.ItemIndex, False, True)
        '    End Select
        'End Sub

        Private Sub ShowChildRows(ByVal ParentPos As Integer, ByVal Show As Boolean, ByVal Recursive As Boolean)
            Dim i As Integer
            ' start at lParentPos b/c we can't have smaller parent id

            Try
                'Nalini Issue 12458
                'Navin Prasad Issue 11032
                For i = ParentPos To grdMain.Rows.Count - 1
                    Dim lbl As Label = CType(grdMain.Rows(i).FindControl("lblParentPos"), Label)
                    If CInt(lbl.Text) = ParentPos Then
                        grdMain.Rows(i).Visible = Show
                        If Recursive Then
                            ShowChildRows(i, Show, True)
                        End If
                    End If
                Next
                If Show Then
                    'set expanded column to 1
                    Dim lblexp As Label = CType(grdMain.Rows(ParentPos).FindControl("lblExpanded"), Label)
                    lblexp.Text = "1"

                    ' item 1 - img plus
                    ' item 3 - img minus
                    grdMain.Rows(ParentPos).Cells(0).Controls.Item(1).Visible = False
                    grdMain.Rows(ParentPos).Cells(0).Controls.Item(3).Visible = True And _
                            Val(grdMain.Rows(ParentPos).Cells(4).Text) > 0
                Else
                    'set expanded column to 0
                    grdMain.Rows(ParentPos).Cells(7).Text = "0"

                    ' item 1 - img plus
                    ' item 3 - img minus
                    grdMain.Rows(ParentPos).Cells(0).Controls.Item(1).Visible = True And _
                            Val(grdMain.Rows(ParentPos).Cells(4).Text) > 0
                    grdMain.Rows(ParentPos).Cells(0).Controls.Item(3).Visible = False
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        'Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click

        '    Response.Redirect(Me.RedirectURL & "?" & Me.RedirectIDParameterName & "=" & HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(CStr(Me.ControlRecordID))) & "&Search=" & txtSearch.Text)
        'End Sub

        'Navin Prasad Issue 11032
        Protected Sub grdMain_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdMain.PageIndexChanged
            LoadGrid(Me.ForumID)
        End Sub

        'Navin Prasad Issue 11032
        Protected Sub grdMain_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdMain.RowCommand
            Select Case e.CommandName
                Case "Expand"
                    ' ShowChildRows(grdMain.SelectedRow.RowIndex, True, False)
                    ShowChildRows(CInt(e.CommandArgument), True, False)
                Case "Collapse"
                    ' ShowChildRows(grdMain.SelectedRow.RowIndex, False, True)
                    ShowChildRows(CInt(e.CommandArgument), False, True)
            End Select
        End Sub
    End Class
End Namespace
