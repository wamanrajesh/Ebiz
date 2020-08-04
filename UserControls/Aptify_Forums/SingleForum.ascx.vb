'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports System.Data
Imports Aptify.Framework.BusinessLogic.GenericEntity

Namespace Aptify.Framework.Web.eBusiness.Forums
    Partial Class SingleForum
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "SingleForum"

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

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

        End Sub

        ''' <summary>
        ''' This method returns a forum ID for a discussion forum found to be linked
        ''' to the specified record in the specified entity. If no forum is linked to the 
        ''' specified entity and record id, value of -1 is returned. To create a new linked
        ''' forum us the <see cref="CreateNewLinkedForum">CreateNewLinkedForum()</see> method.
        ''' </summary>
        ''' <param name="Entity"></param>
        ''' <param name="RecordID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetLinkedForumID(ByVal Entity As String, ByVal RecordID As Long) As Long
            Dim ssql As String, lValue As Object
            ssql = "SELECT ISNULL(MAX(DiscussionForumID),-1) FROM " & _
                   Me.AptifyApplication.GetEntityBaseDatabase("Discussion Forums") & _
                   "..vwDiscussionForumLinks WHERE EntityID=" & _
                   Me.AptifyApplication.GetEntityID(Entity) & _
                   " AND RecordID=" & RecordID
            lValue = Me.DataAction.ExecuteScalar(ssql, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
            If Not lValue Is Nothing And CLng(lValue) > 0 Then
                Return CLng(lValue)
            Else
                Return -1
            End If
        End Function

        ''' <summary>
        ''' This method will create a new discussion forum in the database with the
        ''' provided parameters and link it to the specified entity record ID.
        ''' </summary>
        ''' <param name="Name"></param>
        ''' <param name="Description"></param>
        ''' <param name="StartDate"></param>
        ''' <param name="EndDate"></param>
        ''' <param name="LinkedEntity"></param>
        ''' <param name="LinkedRecordID"></param>
        ''' <returns>True if succesful, False otherwise</returns>
        ''' <remarks></remarks>
        Public Overridable Function CreateNewLinkedForum(ByVal Name As String, _
                                                         ByVal Description As String, _
                                                         ByVal StartDate As DateTime, _
                                                         ByVal EndDate As DateTime, _
                                                         ByVal LinkedEntity As String, _
                                                         ByVal LinkedRecordID As Long, _
                                                         Optional ByRef NewForumID As Long = -1, _
                                                         Optional ByRef LastError As String = "") As Boolean
            Try
                Dim oForum As Aptify.Framework.BusinessLogic.GenericEntity.AptifyGenericEntityBase
                oForum = Me.AptifyApplication.GetEntityObject("Discussion Forums", -1)
                oForum.SetValue("Name", Name)
                oForum.SetValue("Description", Description)
                oForum.SetValue("Type", "Forum")
                oForum.SetValue("Browsable", "0")
                oForum.SetValue("Access", "Registered")
                oForum.SetValue("Status", "Active")
                oForum.SetValue("StartDate", StartDate)
                oForum.SetValue("EndDate", EndDate)
                With oForum.SubTypes("DiscussionForumLinks").Add
                    .SetValue("EntityID", AptifyApplication.GetEntityID(LinkedEntity))
                    .SetValue("RecordID", LinkedRecordID)
                End With
                If oForum.Save(False) Then
                    NewForumID = oForum.RecordID
                    Return True
                Else
                    LastError = "An error took place while creating the forum. The message is below. If this problem persists, please contact web site technical support.<br><br><pre>" & oForum.LastError & "</pre>"
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Function

        Public Sub LoadForum(ByVal lForumID As Long)
            If lForumID > 0 Then
                Me.ForumID = lForumID
                Me.ForumMessageGrid.LoadGrid(Me.ForumID)
            End If
        End Sub

        Protected Sub ForumMessageGrid1_MessageSelected(ByVal MessageID As Long) Handles ForumMessageGrid.MessageSelected
            If MessageID > 0 Then
                Me.Message.LoadMessage(MessageID)
                Me.Message.Visible = True
            Else
                Me.Message.Visible = False
            End If
            Me.CreateMessage.Visible = False
        End Sub

        Protected Sub ForumMessageGrid1_NewMessage() Handles ForumMessageGrid.NewMessage
            Me.Message.Visible = False
            Me.CreateMessage.Visible = True
            Me.CreateMessage.NewMessage(Me.ForumID)
        End Sub

        Protected Sub ForumMessageGrid1_ReplyToMessage(ByVal MessageID As Long) Handles ForumMessageGrid.ReplyToMessage
            Me.Message.Visible = False
            Me.CreateMessage.Visible = True
            Me.CreateMessage.ReplyTo(Me.ForumID, MessageID)
        End Sub

        Protected Sub CreateMessage1_MessageCancelled() Handles CreateMessage.MessageCancelled
            Me.LoadForum(Me.ForumID)
        End Sub

        Protected Sub CreateMessage1_MessagePosted(ByVal MessageID As Long) Handles CreateMessage.MessagePosted
            Me.LoadForum(Me.ForumID)
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'set control properties from XML file if needed
            SetProperties()
        End Sub
    End Class
End Namespace