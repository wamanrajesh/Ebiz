'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On
Imports Aptify.Framework.Web.eBusiness


Namespace Aptify.Framework.Web.eBusiness.Education
    ''' <summary>
    ''' This control is used to validate if the current user is an instructor or not
    ''' </summary>
    ''' <remarks></remarks>
    Partial Class InstructorValidator
        Inherits BaseUserControl


        Public Function IsCurrentUserInstructor() As Boolean
            Return IsInstructor(User1.PersonID)
        End Function

        Public Function IsInstructor(ByVal PersonID As Long) As Boolean
            Dim sSQL As String, lValue As Object
            Try
                sSQL = "SELECT COUNT(*) FROM " & _
                       Me.AptifyApplication.GetEntityBaseDatabase("Courses") & _
                       "..vwCourseInstructors ci WHERE InstructorID=" & PersonID & _
                       " AND Status='Active'"
                lValue = Me.DataAction.ExecuteScalar(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                If Not lValue Is Nothing Then
                    Return CLng(lValue) > 0
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return False
            End Try
        End Function

        Public Overridable Function ValidateClassInstructor(ByVal ClassID As Long) As Boolean
            Return ValidateClassInstructor(ClassID, User1.PersonID)
        End Function
        Public Overridable Function ValidateClassInstructor(ByVal ClassID As Long, ByVal InstructorID As Long) As Boolean
            Dim sSQL As String, lCount As Object
            Try
                sSQL = "SELECT COUNT(*) FROM " & _
                       AptifyApplication.GetEntityBaseDatabase("Class Registrations") & _
                       "..vwClasses WHERE ID=" & ClassID & _
                       " AND InstructorID=" & InstructorID
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
