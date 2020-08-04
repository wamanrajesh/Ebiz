'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On
Imports Aptify.Framework.Web.eBusiness


Namespace Aptify.Framework.Web.eBusiness.Meetings
    ''' <summary>
    ''' This control is used to validate if the current user is a speaker or not
    ''' </summary>
    ''' <remarks></remarks>
    Partial Class SpeakerValidator
        Inherits BaseUserControl
        Public Function IsCurrentUserSpeaker() As Boolean
            Return IsSpeaker(User1.PersonID)
        End Function
        Public Function IsSpeaker(ByVal PersonID As Long) As Boolean
            Dim sSQL As String, lValue As Object
            Try
                sSQL = "SELECT COUNT(*) FROM " & _
                       Me.AptifyApplication.GetEntityBaseDatabase("Meetings") & _
                       "..vwMeetingSpeakers ms WHERE SpeakerID=" & PersonID
                lValue = Me.DataAction.ExecuteScalar(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                If Not lValue Is Nothing Then
                    Return CLng(lValue) > 0
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return False
            End Try
        End Function

        Public Overridable Function ValidateMeetingSpeaker(ByVal MeetingID As Long) As Boolean
            Return ValidateMeetingSpeaker(MeetingID, User1.PersonID)
        End Function
        Public Overridable Function ValidateMeetingSpeaker(ByVal MeetingID As Long, _
                                                           ByVal SpeakerID As Long) As Boolean
            Dim sSQL As String, lCount As Object
            Try
                sSQL = "SELECT COUNT(*) FROM " & _
                       AptifyApplication.GetEntityBaseDatabase("Meetings") & _
                       "..vwMeetingSpeakers WHERE MeetingID=" & MeetingID & _
                       " AND SpeakerID=" & SpeakerID
                lCount = Me.DataAction.ExecuteScalar(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                If Not lCount Is Nothing Then
                    Return CLng(lCount) > 0
                Else
                    Return False
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Returns the internal instance of the User Control
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable ReadOnly Property User() As eBusiness.User
            Get
                Return Me.User1
            End Get
        End Property
    End Class
End Namespace
