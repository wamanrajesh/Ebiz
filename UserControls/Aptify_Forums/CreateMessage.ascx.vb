'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Framework.Application
Imports Aptify.Framework.BusinessLogic
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.AttributeManagement
Imports System.Data


Namespace Aptify.Framework.Web.eBusiness.Forums
    Partial Class CreateMessage
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "CreateMessage"

#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()
        End Sub

#End Region

        Private Property ParentID() As Long
            Get
                If ViewState.Item("ParentID") IsNot Nothing Then
                    Return CLng(ViewState.Item("ParentID"))
                Else
                    Return -1
                End If
            End Get
            Set(ByVal value As Long)
                ViewState.Item("ParentID") = value

            End Set
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
        Public Event MessagePosted(ByVal MessageID As Long)
        Public Event MessageCancelled()

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
            If Not Page.IsPostBack Then
                'Issue 4348 - 12/06/2006 MAS
                'track this session to disable refresh
                Session("SessionID") = Server.UrlEncode(System.DateTime.Now.ToString())
                If Me.SetControlRecordIDFromQueryString AndAlso _
                Me.SetControlRecordIDFromParam() Then
                    NewMessage(Me.ControlRecordID)
                End If

            End If
        End Sub

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

        End Sub

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            'Issue 4348 - 12/06/2006 MAS
            'assign the SessionID to ViewState (server-side)
            ViewState("SessionID") = Session("SessionID")
        End Sub


        Public Overridable Sub NewMessage(ByVal lForumID As Long)

            Try
                Me.ParentID = -1
                Me.ForumID = lForumID
                lblError.Visible = False
                txtSubject.Text = ""
                txtBody.Text = ""

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Public Overridable Sub ReplyTo(ByVal lForumID As Long, ByVal MessageID As Long)
            ' reply to a specific message'
            Try
                Me.ParentID = MessageID
                Me.ForumID = lForumID

                '8/30/06 RJK - Default Subject to the Parent's Subject with RE:
                If Me.ParentID > 0 Then
                    Dim sSQL As String = "SELECT TOP 1 Subject FROM " & Me.AptifyApplication.GetEntityBaseDatabase("Discussion Forum Messages") & ".." & Me.AptifyApplication.GetEntityBaseView("Discussion Forum Messages") & " WHERE ID=" & MessageID.ToString
                    Dim oSubject As Object = Me.DataAction.ExecuteScalar(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)

                    If Not IsDBNull(oSubject) Then
                        Dim sSubject As String = oSubject.ToString

                        If sSubject.Length > 0 Then
                            If sSubject.Length < 3 OrElse String.Compare(sSubject.Substring(0, 3), "RE:", True) <> 0 Then
                                txtSubject.Text = "RE: " & sSubject.Trim
                            Else
                                txtSubject.Text = sSubject.Trim
                            End If
                        Else
                            txtSubject.Text = ""
                        End If
                    Else
                        txtSubject.Text = ""
                    End If
                Else
                    txtSubject.Text = ""
                End If


                txtBody.Text = ""
                lblError.Visible = False

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave.Click
            ' save the new message

            Dim oGE As AptifyGenericEntityBase
            Try
                'Issue 4348 - 12/06/2006 MAS
                'do not allow refresh/repost of submit
                If Session("SessionID").ToString() = ViewState("SessionID").ToString() Then
                    oGE = AptifyApplication.GetEntityObject("Discussion Forum Messages", -1)

                    ' transfer the data to the object...
                    oGE.SetValue("Subject", txtSubject.Text)
                    oGE.SetValue("Body", txtBody.Text)
                    oGE.SetValue("ForumID", Me.ForumID)
                    oGE.SetValue("ParentID", Me.ParentID)
                    oGE.SetValue("WebUserID", User1.UserID)
                    oGE.SetValue("DateEntered", Date.Now)

                    If oGE.Save(False) Then
                        lblError.Visible = False
                        RaiseEvent MessagePosted(oGE.GetRecordVersion)

                        'Issue 4348 - 12/06/2006 MAS
                        'reset sessionID so refresh will fail.
                        Session("SessionID") = Server.UrlEncode(System.DateTime.Now.ToString())
                    Else
                        lblError.Text = "Error: " & oGE.LastError
                        lblError.ForeColor = System.Drawing.Color.Red
                        lblError.Visible = True
                    End If
                Else
                    'Issue 4348 - 12/06/2006 MAS
                    'MessagePosted Event will LoadGrid and disable visibility of CreateMessage control
                    RaiseEvent MessagePosted(-1)
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                lblError.Text = "Error: " & ex.Message
                lblError.ForeColor = System.Drawing.Color.Red
                lblError.Visible = True
            End Try

        End Sub

        Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
            RaiseEvent MessageCancelled()
        End Sub
    End Class
End Namespace