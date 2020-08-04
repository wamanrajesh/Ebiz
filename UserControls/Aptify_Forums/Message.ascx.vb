'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On
Imports System.Data

Namespace Aptify.Framework.Web.eBusiness.Forums
    Partial Class Message
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "Message"

#Region "Message Specific Properties"
        Public Overridable ReadOnly Property MessageID() As Long
            Get
                If Not ViewState("MessageID") Is Nothing Then
                    Return CLng(ViewState("MessageID"))
                Else
                    Return -1
                End If
            End Get
        End Property
#End Region

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            'if values are not provide directly or from the XML file, set default values for inherited properties since 
            'control requires them to properly function
            If String.IsNullOrEmpty(Me.QueryStringRecordIDParameter) Then Me.QueryStringRecordIDParameter = "ID"

        End Sub

        Public Sub LoadMessage(ByVal MessageID As Long)
            InnerLoadMessage(MessageID)
        End Sub
        Private Sub InnerLoadMessage(ByVal MessageID As Long)
            Dim sSQL As String, dt As DataTable, lValue As Object
            Try
                ViewState.Add("MessageID", MessageID)

                sSQL = "SELECT * FROM " & Database & _
                       "..vwDiscussionForumMessages WHERE " & _
                       "Status='Posted' AND ID=" & MessageID
                dt = DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                If dt.Rows.Count = 1 Then
                    lblSubject.Text = CStr(dt.Rows(0).Item("Subject"))
                    lblBody.Text = CStr(dt.Rows(0).Item("Body"))
                    lblFrom.Text = CStr(dt.Rows(0).Item("WebUserName"))
                    lblSent.Text = Format$(dt.Rows(0).Item("DateEntered"), "g")
                    lblError.Visible = False

                    'Navin Prasad Issue 8252

                    lValue = CountAttachments(MessageID)

                    'sSQL = "SELECT COUNT(*) FROM " & Database & _
                    '       "..vwAttachmentsWithoutBLOB WHERE EntityID=" & _
                    '       Me.AptifyApplication.GetEntityID("Discussion Forum Messages") & _
                    '       " AND RecordID=" & MessageID
                    'lValue = DataAction.ExecuteScalar(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                    'If lValue Is Nothing OrElse Not IsNumeric(lValue) Then
                    '    ' no attachments
                    '    lValue = 0
                    'End If
                    trAttachments.Visible = False
                    If CLng(lValue) > 0 Then
                        '  lblAttachments.Text = CLng(lValue) & " File" & IIf(CLng(lValue) > 1, "s", "").ToString & " attached"
                        If CLng(dt.Rows(0).Item("WebUserID")) = User1.UserID Then
                            btnAttachments.Text = "View/Add"
                        Else
                            btnAttachments.Text = "View"
                        End If
                        btnAttachments.Visible = True
                    Else
                        '  lblAttachments.Text = "No Attachments"
                        If CLng(dt.Rows(0).Item("WebUserID")) = User1.UserID Then
                            btnAttachments.Text = "Add"
                            btnAttachments.Visible = True
                        Else
                            btnAttachments.Visible = False
                        End If
                    End If
                Else
                    lblError.Text = "The message is not available"
                    lblError.Visible = True
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        'Navin Prasad Issue 8252
        Private Function CountAttachments(ByVal MessageID As Long) As Object
            Dim sSQL As String, lValue As Object
            sSQL = "SELECT COUNT(*) FROM " & Database & _
                      "..vwAttachmentsWithoutBLOB WHERE EntityID=" & _
                      Me.AptifyApplication.GetEntityID("Discussion Forum Messages") & _
                      " AND RecordID=" & MessageID
            lValue = DataAction.ExecuteScalar(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
            If lValue Is Nothing OrElse Not IsNumeric(lValue) Then
                ' no attachments
                lValue = 0
            End If

            If CLng(lValue) > 0 Then

                lblAttachments.Text = CLng(lValue) & " File" & IIf(CLng(lValue) > 1, "s", "").ToString & " attached"

            Else
                lblAttachments.Text = "No Attachments"

            End If
            Return lValue
        End Function

        'Private Sub HierarchyTree1_NodeClicked(ByVal sender As System.Object, ByVal e As Infragistics.WebUI.UltraWebNavigator.WebTreeNodeEventArgs)
        '    InnerLoadMessage(e.Node.Tag)
        'End Sub

        Protected Sub btnAttachments_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAttachments.Click
            trAttachments.Visible = True
            btnAttachments.Visible = False
            If btnAttachments.text.contains("Add") Then
                Me.RecordAttachments.AllowAdd = True
                Me.RecordAttachments.AllowDelete = True
            Else
                Me.RecordAttachments.AllowAdd = False
                Me.RecordAttachments.AllowDelete = False
            End If
            Me.RecordAttachments.LoadAttachments(Me.AptifyApplication.GetEntityID("Discussion Forum Messages"), Me.MessageID)
        End Sub
        'Navin Prasad Issue 8252
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
                'set control properties from XML file if needed
                SetProperties()
            If Me.MessageID > 0 Then
                CountAttachments(MessageID)
            End If
        End Sub
        'Navin Prasad Issue 8252
        Protected Sub Message_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

            Me.SetControlRecordIDFromParam()
            If Me.ControlRecordID > 0 AndAlso Me.MessageID < 0 Then
                Me.LoadMessage(Me.ControlRecordID)
            End If
            Page_Load(Nothing, Nothing)
        End Sub
    End Class
End Namespace
