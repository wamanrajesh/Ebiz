'Aptify e-Business 5.5.1, July 2013
'NOTE: This version of the Footer is for use with Sitefinity
Option Explicit On
Option Strict On

Imports System.Data
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.Application


Namespace Aptify.Framework.Web.eBusiness

    Partial Class Footer
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CONTROL_DEFAULT_NAME As String = "Footer"
        Protected Const ATTRIBUTE_COPYRIGHT_TEXT As String = "CopyRightText"

        Public Overridable Property CopyRightText() As String
            Get
                Return lblCopyrightText.Text
            End Get
            Set(ByVal value As String)
                lblCopyrightText.Text = value
            End Set
        End Property

        Protected Overrides Sub SetProperties()
            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTROL_DEFAULT_NAME

            If String.IsNullOrEmpty(CopyRightText) Then
                CopyRightText = Me.GetPropertyValueFromXML(ATTRIBUTE_COPYRIGHT_TEXT)
            End If

            MyBase.SetProperties()
        End Sub

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            MyBase.OnLoad(e)
            LoadWebMenu()
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
            SetProperties()
        End Sub

        Public Function LoadWebMenu() As Boolean

            Dim dataAction As DataAction
            Dim dataTable As DataTable
            Dim aptifyApplication As AptifyApplication
            Dim eBusinessGlobal As EBusinessGlobal
            Dim userID As Long
            Dim parentID As Long = -1
            Dim virtualDirPath As String = String.Empty
            Dim sqlWebMenu As String = String.Empty
            Dim rowCount As Integer = 0

            Try
                dataAction = New DataAction
                aptifyApplication = Me.AptifyApplication
                eBusinessGlobal = New EBusinessGlobal
                userID = GetUserObject.UserID
                virtualDirPath = Request.ApplicationPath.TrimEnd("/"c)
                sqlWebMenu = aptifyApplication.GetEntityBaseDatabase("Web Menus") & ".dbo.spGetUserWebMenus @ParentID=" & parentID & ",@WebUserID=" & userID
                dataTable = dataAction.GetDataTable(sqlWebMenu, IAptifyDataAction.DSLCacheSetting.BypassCache)

                For Each menuRow As DataRow In dataTable.Rows
                    Dim li As HtmlGenericControl = New HtmlGenericControl("li")
                    Dim link As HtmlGenericControl = New HtmlGenericControl("a")
                    Dim menuTargetURL As String = Replace(menuRow.Item("URL").ToString, "%VirtualDir%", virtualDirPath, , , CompareMethod.Text)
                    Dim menuBaseURL As String

                    If menuRow.Item("URLType").ToString.Trim.ToUpper = "ABSOLUTE" Then
                        link.Attributes.Add("href", menuTargetURL)
                    Else                        
                        menuBaseURL = Replace(menuRow.Item("BaseURL").ToString, "%VirtualDir%", virtualDirPath, , , CompareMethod.Text)
                        link.Attributes.Add("href", menuBaseURL & menuTargetURL)
                    End If

                    link.InnerText = menuRow.Item("DisplayName").ToString

                    rowCount = rowCount + 1

                    If rowCount = dataTable.Rows.Count Then
                        li.Attributes.Add("class", "last")
                    End If

                    li.Controls.Add(link)
                    footerMenu.Controls.Add(li)
                Next
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try

            Return True
        End Function

        Protected Overridable Function GetUserObject() As User
            Dim oUser As New User
            Try
                oUser.LoadValuesFromSessionObject(Page.Session)
                Return oUser
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return Nothing
            End Try
        End Function
    End Class
End Namespace
