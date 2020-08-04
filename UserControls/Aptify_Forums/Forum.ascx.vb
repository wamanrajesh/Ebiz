'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports System.Data

Namespace Aptify.Framework.Web.eBusiness.Forums
    Partial Class ForumControl
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "Forum"

	'12/04/2007 Tamasa,Code changed for issue 5330.
        Private m_lForumID As Long = -1
        Private m_bRead As Boolean
        Private m_bPost As Boolean
        Private m_bReply As Boolean


        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                'set control properties from XML file if needed
                SetProperties()
                If Not IsPostBack Then

                    If Me.SetControlRecordIDFromQueryString AndAlso _
                        Me.SetControlRecordIDFromParam() Then
                        LoadForum()
                    End If
                End If

                If Me.ControlRecordID < 0 Then
                    Me.Visible = False
                End If
            Catch ex As FormatException
                ' changed code to use config settings instead of hardcoded paths
                'Response.Redirect(Me.GetSecurityErrorPageFromXML & "?Message=" & Server.UrlEncode("Message not Found"))

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub


        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            'if values are not provide directly or from the XML file, set default values for inherited properties since 
            'control requires them to properly function
            If String.IsNullOrEmpty(Me.QueryStringRecordIDParameter) Then Me.QueryStringRecordIDParameter = "ID"

        End Sub

        Private Sub LoadForum()
            Try
                ' load up the forum information on the labels
                Dim sSQL As String
                Dim dt As DataTable
                sSQL = "SELECT df.Name,df.Description,df.ParentID,df.Parent,(SELECT ISNULL(COUNT(*),0) FROM " & _
                       Database & "..vwDiscussionForums WHERE ParentID=df.ID) SubForumCount FROM " & _
                       Database & "..vwDiscussionForums df WHERE ID=" & Me.ControlRecordID.ToString & " AND " & _
                       ForumsControl.GetForumAccessControlWhereSQL(User1.UserID, Database, "df")

                dt = DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                If dt.Rows.Count > 0 Then
                    'lblDiscussionForum.Text = CStr(dt.Rows(0).Item("Name"))
                    'lblDescription.Text = CStr(dt.Rows(0).Item("Description"))
                    If Not IsDBNull(dt.Rows(0).Item("ParentID")) AndAlso _
                       CLng(dt.Rows(0).Item("ParentID")) > 0 Then
                        'lblParent.Text = CStr(dt.Rows(0).Item("Parent"))
                        'lnkParent.HRef = Request.Path & "?ID=" & HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(CStr(dt.Rows(0).Item("ParentID"))))
                    Else
                        'lblParent.Text = "Top Level"
                        'lnkParent.HRef = "Default.aspx"
                    End If

                    'If CLng(dt.Rows(0).Item("SubForumCount")) > 0 Then
                    '    trSubForums.Visible = True
                    '    subForums.ParentID = Me.ControlRecordID
                    '    subForums.LoadForums(False, False)
                    'Else
                    '    trSubForums.Visible = False
                    'End If
                    ' load up the grid
                    LoadMessage(-1)
                    ForumMessageGrid.LoadGrid(Me.ControlRecordID)
                    CreateMessage.Visible = False
                Else
                    'lblDiscussionForum.Text = "Forum Not Found or Not Available"
                    'lblDescription.Visible = False
                    'lblParent.Visible = False
                    'trSubForums.Visible = False
                    ForumMessageGrid.Visible = False
                    CreateMessage.Visible = False
                    Me.Message.Visible = False
                    'Me.parForumLabel.Visible = False
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub ForumMessageGrid_MessageSelected(ByVal lMessageID As Long) Handles ForumMessageGrid.MessageSelected
            LoadMessage(lMessageID)
        End Sub

        Private Sub LoadMessage(ByVal lMessageID As Long)
            If lMessageID > 0 Then
                Message.LoadMessage(lMessageID)
                trMessage.Visible = True
            Else
                trMessage.Visible = False
            End If
        End Sub

        Private Sub ForumMessageGrid_NewMessage() Handles ForumMessageGrid.NewMessage
            CreateMessage.Visible = True
            Message.Visible = False
            trMessage.Visible = True
            CreateMessage.NewMessage(Me.ControlRecordID)
        End Sub

        Private Sub ForumMessageGrid_ReplyToMessage(ByVal MessageID As Long) Handles ForumMessageGrid.ReplyToMessage
            CreateMessage.Visible = True
            Message.Visible = False
            trMessage.Visible = True
            CreateMessage.ReplyTo(Me.ControlRecordID, MessageID)
        End Sub

        Private Sub CreateMessage_MessageCancelled() Handles CreateMessage.MessageCancelled
            If ForumMessageGrid.SelectedMessageID > 0 Then
                Message.Visible = True
            End If
            CreateMessage.Visible = False
        End Sub

        Private Sub CreateMessage_MessagePosted(ByVal lMessageID As Long) Handles CreateMessage.MessagePosted
            ForumMessageGrid.LoadGrid(Me.ControlRecordID)
            Message.Visible = True
            CreateMessage.Visible = False
        End Sub
 
    End Class
End Namespace
