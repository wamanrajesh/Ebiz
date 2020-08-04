'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports System.Data
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.Application

Namespace Aptify.Framework.Web.eBusiness
    Partial Class WebArticleControl
        Inherits BaseUserControlAdvanced

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            LoadWebArticle()
        End Sub

        Private Function LoadWebArticle() As Boolean
            Dim sSQL As String
            Dim dt As DataTable

            Try
                sSQL = Me.AptifyApplication.GetEntityBaseDatabase("Web Articles") & _
                       "..spGetWebArticles @ID=" & Request.QueryString("ID")
                dt = Me.DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)

                If dt.Rows.Count = 1 Then
                    If UCase$(Trim$(CStr(dt.Rows(0).Item("Type")))) = "URLLINK" Then
                        'Simply redirect the specified URL:
                        Response.Redirect(CStr(dt.Rows(0).Item("URLLink")))
                    Else
                        lblWebArticleName.Text = CStr(dt.Rows(0).Item("Name"))
                        If CBool(dt.Rows(0).Item("ShowAuthorInfo")) Then
                            author.Visible = True
                            lblAuthor.Text = CStr(dt.Rows(0).Item("Author"))
                        Else
                            author.Visible = False
                        End If
                        lblDateWritten.Text = CStr(dt.Rows(0).Item("DateWritten"))
                        lblWebArticle.Text = CStr(dt.Rows(0).Item("HTMLText"))
                    End If
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            Finally
                dt = Nothing
            End Try
        End Function
    End Class
End Namespace
