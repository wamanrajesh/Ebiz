'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On
Imports Aptify.Applications.OrderEntry
Imports System.Collections
Imports System.Collections.Generic
Imports System.Linq

Namespace Aptify.Framework.Web.eBusiness.ProductCatalog
    Partial Class HeaderControl
        Inherits BaseUserControlAdvanced
        Protected Const ATTRIBUTE_SIGNOUTIMAGE_URL As String = "SignOutImage"
        'Neha Issue 14408,01/24/13, declare homepage property
        Protected Const ATTRIBUTE_HOME_PAGE As String = "HomePage"

#Region "Properties"
        'Neha Issue 14408,01/24/13, added property for HomePage
        Public Overridable Property HomePage() As String
            Get
                If Not ViewState(ATTRIBUTE_HOME_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_HOME_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_HOME_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        Public Overridable Property SignOutImage() As String
            Get
                If Not ViewState(ATTRIBUTE_SIGNOUTIMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_SIGNOUTIMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_SIGNOUTIMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
#End Region
        Protected Overrides Sub SetProperties()
            MyBase.SetProperties()

            If String.IsNullOrEmpty(SignOutImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                SignOutImage = Me.GetLinkValueFromXML(ATTRIBUTE_SIGNOUTIMAGE_URL)
                If Not String.IsNullOrEmpty(SignOutImage) Then
                    'ImgLogout.ImageUrl = SignOutImage
                End If
            End If
            'Neha Issue 14408,01/24/13,added homepage url
            If String.IsNullOrEmpty(HomePage) Then
                HomePage = Me.GetLinkValueFromXML(ATTRIBUTE_HOME_PAGE)
            End If

        End Sub

        ''' <summary>
        ''' This event is raised whenever a user logs out
        ''' </summary>
        ''' <remarks></remarks>
        Public Event UserLoggedOut()


        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            SetProperties()

            If User1.UserID > 0 Then
                tdUserName.Visible = True
                tdSignout.Visible = True
                lblUserName.Text = User1.FirstName & " " & User1.LastName & "!"
                LoadGroupAdmin()

            Else
                tdUserName.Visible = False
                tdSignout.Visible = False
            End If

            If Not Page.IsPostBack Then
                Clearecatche()
            End If
        End Sub
        Private Sub LoadGroupAdmin()
            Dim sSQL As String

            Dim IsGroupAdmin As Boolean
            Try
                sSQL = "SELECT IsGroupAdmin FROM VWPERSONS WHERE ID = " & User1.PersonID

                IsGroupAdmin = CBool(DataAction.ExecuteScalar(sSQL))

                If IsGroupAdmin Then
                    lblGrpAdmin.Visible = True
                    lblGrpAdmin.Text = "Company Administrator"  'Chage made by Sandeep for 15169 on 17/01/2012
                    lblCampany.Text = "(" + User1.Company + ")"
                Else
                    lblGrpAdmin.Visible = False
                    lblCampany.Visible = False
                    lblGrpAdmin.Text = ""
                End If

            Catch ex As Exception

            End Try
        End Sub

        'Protected Sub ImgLogout_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImgLogout.Click
        '    '' Use the AptifyWebLogin Component to do this
        '    'Dim bLoggedOut As Boolean = False
        '    'Try
        '    '    If WebUserLogin.Logout() Then

        '    '        ShoppingCart.Clear()
        '    '        WebUserLogin.ClearAutoLoginCookie(Page.Response)
        '    '        'HP Issue#9078: clear and delete session
        '    '        Session.Clear()
        '    '        Session.Abandon()
        '    '        bLoggedOut = True
        '    '        'RashmiP, Call Clear Cookies function
        '    '        ClearCookies()
        '    '        Session("SocialNetwork") = Nothing
        '    '    End If
        '    'Catch ex As Exception
        '    '    Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        '    'End Try
        '    'If bLoggedOut Then
        '    '    OnUserLoggedOut()
        '    'End If

        'End Sub

        'RashmiP, Function clears cookies
        Private Sub ClearCookies()
            Dim i As Integer
            Dim aCookie As HttpCookie

            Dim limit As Integer = Request.Cookies.Count - 1
            For i = 0 To limit
                aCookie = Request.Cookies(i)
                aCookie.Expires = DateTime.Now.AddDays(-1)
                Response.Cookies.Add(aCookie)
            Next

        End Sub

        Protected Overridable Sub OnUserLoggedOut()
            'Issue 10258
            DeleteTempImages()
            RaiseEvent UserLoggedOut()
            'Neha Issue 14408,01/24/13,redirect to HomePage
            If HomePage IsNot Nothing AndAlso HomePage <> String.Empty Then
                Response.Redirect(HomePage)
            End If
            Response.Redirect(Request.RawUrl)
        End Sub


        'Issue 10258
        Protected Overridable Sub DeleteTempImages()
            Try
                Dim fileEntries As String()
                Dim xml As New System.Xml.XmlDocument()
                Dim Spath As String = Server.MapPath(Request.ApplicationPath)
                If Spath IsNot String.Empty Then
                    xml.Load(Spath + "\Aptify_UC_Navigation.config")
                End If
                Dim controlName As [String] = "Profile1"
                Dim linkName As [String] = "PersonImageURL"
                Dim PersonImageURL As [String] = xml.SelectSingleNode("//UserControl[@type != '' and @name ='" & controlName & "']/links/link[@name='" & linkName & "']").Attributes("value").Value
                If PersonImageURL <> [String].Empty Then
                    fileEntries = System.IO.Directory.GetFiles(Spath + PersonImageURL, "*_" & Convert.ToString(User1.PersonID) & "*.jpg", System.IO.SearchOption.TopDirectoryOnly)
                    For Each filename As [String] In fileEntries
                        System.IO.File.Delete(filename)
                    Next
                    fileEntries = System.IO.Directory.GetFiles(Spath + PersonImageURL, "*_" & Convert.ToString(Me.Session.SessionID) & "*.jpg", System.IO.SearchOption.TopDirectoryOnly)
                    For Each filename As [String] In fileEntries
                        System.IO.File.Delete(filename)
                    Next
                End If
            Catch ex As Exception
                ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Protected Sub ImgLogout_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ImgLogout.Click
            Dim bLoggedOut As Boolean = False
            Try
                If WebUserLogin.Logout() Then
                    'ltlSscript.Text = "<script type=""text/javascript"">window.history.go(-(window.history.length - 1));</script>"
                    Dim authCookie As New HttpCookie(FormsAuthentication.FormsCookieName, String.Empty)
                    authCookie.Expires = DateTime.Now.AddDays(-1)
                    HttpContext.Current.Response.Cookies.Add(authCookie)

                    ' clean session cookie    
                    Dim sessionCookie As New HttpCookie("ASP.NET_SessionId", String.Empty)
                    sessionCookie.Expires = DateTime.Now.AddDays(-1)
                    HttpContext.Current.Response.Cookies.Add(sessionCookie)
                    'Suraj Issue 15370,7/31/13 revert back changes application variale to session variable
                    Session.Remove("ReturnToPage")
                    ShoppingCart.Clear()
                    WebUserLogin.ClearAutoLoginCookie(Page.Response)
                    Session.RemoveAll()
                    Clearecatche()
                    'HP Issue#9078: clear and delete session
                    Session.Clear()
                    Session.Abandon()
                    FormsAuthentication.SignOut()
                    bLoggedOut = True
                    'RashmiP, Call Clear Cookies function
                    ClearCookies()
                    Session("SocialNetwork") = Nothing
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
            If bLoggedOut Then
                OnUserLoggedOut()
            End If
        End Sub
        Private Sub Clearecatche()
            Try

                Response.Cache.SetValidUntilExpires(False)
                Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1))
                Response.Cache.SetCacheability(HttpCacheability.NoCache)
                Response.Cache.SetNoStore()
                Response.Buffer = True
                Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
                Response.Expires = -1500
                Response.CacheControl = "no-cache"
                Response.AddHeader("Cache-control", "no-store, must-revalidate, private,no-cache")
                Response.AddHeader("Pragma", "no-cache")
                Response.AddHeader("Expires", "0")
                Response.Cache.SetAllowResponseInBrowserHistory(False)
                Response.ClearHeaders()
                Response.AppendHeader("Cache-Control", "no-cache")
                Response.AppendHeader("Cache-Control", "private")
                Response.AppendHeader("Cache-Control", "no-store")
                Response.AppendHeader("Cache-Control", "must-revalidate")
                Response.AppendHeader("Cache-Control", "max-stale=0")
                Response.AppendHeader("Cache-Control", "post-check=0")
                Response.AppendHeader("Cache-Control", "pre-check=0")
                Response.AppendHeader("Pragma", "no-cache")
                Response.AppendHeader("Keep-Alive", "timeout=3, max=993")
                Response.AppendHeader("Expires", "0")

                HttpContext.Current.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1))
                HttpContext.Current.Response.Cache.SetValidUntilExpires(False)
                HttpContext.Current.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches)
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache)
                HttpContext.Current.Response.Cache.SetNoStore()
                ' clean auth cookie


                'Dim entry As DictionaryEntry
                'For Each entry In System.Web.HttpContext.Current.Cache
                '    System.Web.HttpContext.Current.Cache.Remove(CStr(entry.Key))
                'Next
                'lblMsg.Text = "Cache is cleared!"
            Catch ex As Exception

                'lblMsg.Text = "Cache is not cleared!"

            End Try
        End Sub
    End Class

End Namespace

