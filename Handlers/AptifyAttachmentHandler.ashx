<%@ WebHandler Language="VB" Class="AptifyAttachmentHandler" %>

Imports System
Imports System.Web
Imports Aptify.Framework.Web.Common

Public Class AptifyAttachmentHandler
    Inherits Aptify.Framework.Web.Common.AttachmentHandler2
    
    Public Overrides Sub ProcessRequest(ByVal context As HttpContext)
        Try
            Dim g As New Aptify.Framework.Web.eBusiness.EBusinessGlobal

            MyBase.UserCredentials = g.GetAptifyApplication(context.Application, _
                                                            context.User).UserCredentials
            MyBase.ProcessRequest(context)
        Catch ex As Exception
            Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            Try
                context.Response.Write("<pre>Error processing request<hr>" & ex.Message & "<hr>" & ex.StackTrace & "</pre>")
            Catch ex2 As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Try
    End Sub
End Class