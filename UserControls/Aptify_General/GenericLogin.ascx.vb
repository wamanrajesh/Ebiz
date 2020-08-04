'Aptify e-Business 5.5.1, July 2013
Partial Class UserControls_Aptify_General_GenericLogin
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Suraj S Issue 15370, 7/31/13 if the user is not login we are redirecting to the loging page if bymistake any one redirect to user for genric login the here we are  getting the ReturnToPageURL in "URL" QueryString and we storing this URL in to the Session("ReturnToPage")  
        If Request.QueryString("ReturnURL") IsNot Nothing Then
            Session("ReturnToPage") = Aptify.Framework.Web.Common.WebCryptography.Decrypt(Request.QueryString("ReturnURL"))
        End If
        If User1.UserID > 0 Then
            lblstat.Visible = False
        Else
            lblstat.Visible = True
        End If
    End Sub
End Class
