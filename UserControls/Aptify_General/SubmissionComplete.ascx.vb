'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Namespace Aptify.Framework.Web.eBusiness
    ''' <summary>
    ''' This is a temporary page that should be used to display a timed message to the user
    ''' then will automatically redirect to a new page.
    ''' The Message (keyword = Message), Time to display page (keyword = Timer), and Page 
    ''' to user for Redirect (keyword = RedirecPage) should all be specified in the QueryString. 
    ''' If no Message is set, a default 'Your Submission was Successful.' will display. 
    ''' If no Time is set, then the default 5 seconds will be used. 
    ''' If no Redirect Page is specified, then the User will be redirected to the home page (default.aspx).
    ''' Each variable value should be URLEncoded.
    ''' </summary>
    ''' <remarks></remarks>
    Partial Class SubmissionCompleteControl
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControl

        ''' <summary>
        ''' The redirect to the page should include a QueryString with 'Message', 'Timer', and 'RedirectPage'
        ''' to specify what this page should display, for how long and what page to redirect to.
        ''' Each variable value should be URLEncoded.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>        
        ''' <example>This is an example call to this page including the QueryString creation:
        ''' <c>
        ''' Dim sRedirect As String = "../SubmissionComplete.aspx"
        ''' 'Add Message
        ''' sRedirect &amp;= "?Message=" &amp; Server.UrlEncode("CEU Certificate submission was successful.")
        ''' 'Add Time to display page
        ''' sRedirect &amp;= "&amp;Timer=" &amp; Server.UrlEncode("4")
        ''' 'Add Redirect Page
        ''' sRedirect &amp;= "&amp;RedirectPage=" &amp; Server.UrlEncode("/Education/MyCertifications.aspx")
        ''' Response.Redirect(sRedirect)
        ''' </c>
        ''' </example>
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try

                If Not IsPostBack Then

                    Dim sMessage As String
                    Dim lTime As Long
                    Dim sPage As String

                    If Request.QueryString("Message") <> "" Then
                        'Get Display(Message)
                        sMessage = Server.UrlDecode(Request.QueryString("Message"))

                    Else
                        'Display Default Message
                        sMessage = "Your Submission was Successful."
                    End If

                    'Set Display Time
                    If Request.QueryString("Timer") <> "" AndAlso IsNumeric(Request.QueryString("Timer")) Then
                        'Get Time to display this page (Timer)
                        lTime = CLng(Server.UrlDecode(Request.QueryString("Timer")))
                    Else
                        'Use Default of 5 seconds
                        lTime = 5
                    End If

                    'Set Redirect Page
                    If Request.QueryString("RedirectPage") <> "" Then
                        'Get Page to Redirect after timer expires
                        sPage = Server.UrlDecode(Request.QueryString("RedirectPage"))
                    Else
                        'Use Home page as Default 
                        sPage = Request.ApplicationPath
                    End If

                    '<META HTTP-EQUIV="Refresh" CONTENT="5;URL=http://www.yoursite.com/page.aspx">

                    'Dynamically create the HTML tag that will allow the redirection after X seconds
                    Dim metaTag As New System.Web.UI.HtmlControls.HtmlMeta
                    metaTag.HttpEquiv = "Refresh"
                    metaTag.Content = lTime.ToString & ";URL=" & sPage
                    Me.Page.Header.Controls.Add(metaTag)

                    'Set the Message
                    lblMessage.Text = sMessage

                    'Attempt to extract page from provided RedirectPage
                    Dim loc As Integer
                    Dim GoToPage As String = sPage
                    loc = InStr(sPage, "/")
                    While (loc > 0)
                        GoToPage = Mid(GoToPage, loc + 1)
                        loc = InStr(GoToPage, "/")
                    End While
                    loc = InStr(GoToPage, ".")
                    If loc > 0 Then
                        GoToPage = Mid(GoToPage, 1, loc - 1)
                    End If
                    '** Check to make sure Page is a valid string to be used! **

                    'Set the redirect message
                    lblRedirectMessage.Text = "You will be redirected to " & GoToPage & " in " & lTime.ToString & " seconds. " & _
                                              "Click <a href=""" & sPage & """>here</a> to go now."
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
    End Class
End Namespace
