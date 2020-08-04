'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports Aptify.Framework.BusinessLogic.GenericEntity

Namespace Aptify.Framework.Web.eBusiness.Forums
    Partial Class AddForumControl
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "AddForum"

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'set control properties from XML file if needed
            SetProperties()
            ApplyStyles()
        End Sub

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(Me.RedirectURL) Then
                Me.cmdCreateForum.Enabled = False
                Me.cmdCreateForum.ToolTip = "RedirectURL property has not been set."
            End If

        End Sub

        Private Sub cmdCreateForum_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreateForum.Click
            ' This is the code that will add in a new forum recrod to CGI
            Dim oGE As AptifyGenericEntityBase
            Dim lParentID As Long

            Try
                ' Changes made for to allow encrypting and decrypting the URL.
                ' Changes made by Hrushikesh Jog
                Dim sParentID As String = Request.QueryString.Item("ParentID")
                sParentID = Aptify.Framework.Web.Common.WebCryptography.Decrypt(sParentID)

                oGE = AptifyApplication.GetEntityObject("Discussion Forums", -1)
                Me.TransferDataToGE(oGE)
                If Len(sParentID) > 0 Then
                    lParentID = CLng(sParentID)
                Else
                    lParentID = -1
                End If
                oGE.SetValue("ParentID", CStr(lParentID))

                If oGE.Save(False) Then

                    'CP 7/11/2008 Build Redirect URL from Properties
                    If Me.AppendRecordIDToRedirectURL Then
                        If Me.EncryptQueryStringValue Then
                            Me.RedirectURL &= "?" & Me.RedirectIDParameterName & "=" & System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(oGE.RecordID.ToString))
                        Else
                            Me.RedirectURL &= "?" & Me.RedirectIDParameterName & "=" & oGE.RecordID.ToString
                        End If
                    End If

                    'preferably redirect to Forum Center page
                    Response.Redirect(Me.RedirectURL)

                Else
                    lblError.Text = oGE.LastError()
                    lblError.Visible = True
                End If

            Catch ex As FormatException
                ' changed code to use config settings instead of hardcoded paths
                Response.Redirect(Me.GetSecurityErrorPageFromXML & "?Message=" & Server.UrlEncode("Forum not available"))

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
    End Class
End Namespace
