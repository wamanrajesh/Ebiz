<%@ WebHandler Language="VB" Class="RSSFeed" %>
 
Imports System.Web

Public Class RSSFeed  
    Inherits Aptify.Framework.Web.Common.RSS.RSSFeedHandler
    
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