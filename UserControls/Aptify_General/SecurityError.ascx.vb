'Aptify e-Business 5.5.2 Hotfix Issue 17302, December 2014
Option Explicit On
Option Strict On

Namespace Aptify.Framework.Web.eBusiness
    Partial Class SecurityError
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "SecurityError"
        'Navin Prasad Issue# 12849
        Protected Const ATTRIBUTE_LOGIN_PAGE As String = "LoginPage"

#Region "SecurityPage specific properties"
        ''' <summary>
        ''' LoginPage url
        ''' </summary>
        Public Overridable Property LoginPage() As String
            Get
                If Not ViewState(ATTRIBUTE_LOGIN_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_LOGIN_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_LOGIN_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
#End Region

        Protected Overrides Sub SetProperties()
            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(LoginPage) Then
                LoginPage = Me.GetGlobalAttributeValue(ATTRIBUTE_LOGIN_PAGE)
            End If
        End Sub
        'Neha Changes for Issue 17302, SecurityError.aspx Vulnerable To Javascript Injection Attack
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Dim Str As String
            'set control properties from XML file if needed
            SetProperties()
            Try
                If Not IsPostBack Then
                    If User.UserID > 0 Then
                        If Request.QueryString("Message") <> "" Then
                            Str = System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Decrypt(Request.QueryString("Message")))
                            lblMessage.Text = Str.Replace("%2b", " ")
                            lblMessage.Visible = True
                        End If
                    Else
                        Response.Redirect(LoginPage)
                    End If
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
    End Class
End Namespace
