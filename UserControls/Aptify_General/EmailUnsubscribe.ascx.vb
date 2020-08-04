'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Namespace Aptify.Framework.Web.eBusiness
    Partial Class Unsubscribe
        Inherits BaseUserControlAdvanced
        Protected ATTRIBUTE_EmailUnsubscribeOrganizationName As String = "OrganizationName"
        'Dim OrganizationName As String
        Protected Overridable ReadOnly Property EmailUnsubscribeOrganizationName() As String
            Get
                If Not Session.Item(ATTRIBUTE_EmailUnsubscribeOrganizationName) Is Nothing Then
                    Return CStr(Session.Item(ATTRIBUTE_EmailUnsubscribeOrganizationName))
                Else
                    Dim value As String = Me.GetGlobalAttributeValue(ATTRIBUTE_EmailUnsubscribeOrganizationName)
                    If Not String.IsNullOrEmpty(value) Then
                        Session.Item(ATTRIBUTE_EmailUnsubscribeOrganizationName) = value
                        Return value
                    Else
                        Return String.Empty
                    End If
                End If
            End Get
        End Property
        Protected Overrides Sub SetProperties()
            If Not String.IsNullOrEmpty(EmailUnsubscribeOrganizationName) Then
                ATTRIBUTE_EmailUnsubscribeOrganizationName = EmailUnsubscribeOrganizationName
            End If
        End Sub


        Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click

            Try
                Dim sbEmail As New System.Text.StringBuilder
                'Dim oMailMessage As New Mail.MailMessage
                Dim sCode As String

                ' ensure on the server that all of the validation criteria has been met
                If Page.IsValid Then
                    ' build the email body using a string builder

                    Dim sConcatParam As String = "?"

                    With sbEmail
                        .AppendLine("<font size=""2"" face=""Arial"">Thank you for your request. Please click on the below link to confirm your request to unsubscribe from Aptify email messages<br/><br/>")

                        ' sConcatParam will contain either "&" or "? depending on URL.
                        ' Changes made to resolve Issue 5307 by Vijay Sitlani on 01-23-2008.
                        If Me.Request.Url.AbsoluteUri.IndexOf("?") > 0 Then
                            sConcatParam = "&"
                        End If

                        sCode = System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(txtEmail.Text))
                        .AppendLine("<a href=""" & Me.Request.Url.AbsoluteUri & sConcatParam & "ConfirmationCode=" & sCode & """>Click here to confirm your unsubscribe request</a><br/><br/>")
                        .AppendLine("<b>You entered the following comments:</b></font>")
                        .AppendLine("<pre>" & txtComments.Text & "</pre>")
                    End With

                    ' build the email message
                    Dim obj As System.Net.Mail.SmtpClient = New System.Net.Mail.SmtpClient
                    Dim Mailmsg As New System.Net.Mail.MailMessage
                    Mailmsg.To.Clear()

                    Mailmsg.To.Add(New System.Net.Mail.MailAddress(Me.txtEmail.Text))
                    Mailmsg.From = New System.Net.Mail.MailAddress(System.Configuration.ConfigurationManager.AppSettings("MailFrom"))
                    Mailmsg.Bcc.Add(New System.Net.Mail.MailAddress(System.Configuration.ConfigurationManager.AppSettings("MailBCC")))
                    Mailmsg.BodyEncoding = System.Text.Encoding.Default
                    Mailmsg.Subject = System.Configuration.ConfigurationManager.AppSettings("MailSubject")
                    Mailmsg.Body = sbEmail.ToString
                    Mailmsg.IsBodyHtml = True
                    obj.UseDefaultCredentials = CBool(System.Configuration.ConfigurationManager.AppSettings("UseDefaultCredentials"))
                    If Not obj.UseDefaultCredentials Then
                        Dim basicAuthenticationInfo As New System.Net.NetworkCredential(System.Configuration.ConfigurationManager.AppSettings("MailUserName"), System.Configuration.ConfigurationManager.AppSettings("MailPassword"))
                        obj.Credentials = basicAuthenticationInfo

                    End If
                    'Put your own, or your ISPs, mail server name onthis next line

                    Try
                        Dim oPersons As System.Collections.Generic.List(Of Long) = Me.GetPersonIDFromEmail(txtEmail.Text)
                        If oPersons IsNot Nothing AndAlso oPersons.Count > 0 Then
                            Dim oGE As Aptify.Framework.BusinessLogic.GenericEntity.AptifyGenericEntityBase
                            oGE = AptifyApplication.GetEntityObject("Contact Log", -1)
                            oGE.SetValue("Description", "Unsubscribe Request on Web - Email Confirmation Request Sent")
                            oGE.SetValue("Details", Mailmsg.Body)
                            oGE.SetValue("Direction", "Inbound")
                            oGE.SetValue("TypeID", 3)
                            oGE.SetValue("CategoryID", 5)
                            For Each lPersonID As Long In oPersons
                                With oGE.SubTypes("ContactLogLinks").Add
                                    .SetValue("AltID", lPersonID)
                                    .SetValue("EntityID", AptifyApplication.GetEntityID("Persons"))
                                End With
                            Next
                            oGE.Save(False)

                            obj.Host = System.Configuration.ConfigurationManager.AppSettings("MailServer")
                            obj.Send(Mailmsg)


                            'Vijay Sitlani
                            ' Send the email using the .NET Framework
                            'Mail.SmtpMail.SmtpServer = System.Configuration.ConfigurationManager.AppSettings("MailServer")
                            'Mail.SmtpMail.Send(oMailMessage)

                            ' Sends user an email ENDS
                            ' Response Table visible set
                            tblRequest.Visible = False
                            tblresponse.Visible = True
                            lblInvalidEmail.Visible = False
                            lblInvalidEmail.Text = ""
                        Else
                            ' Vijay Sitlani
                            ' Display an error message indicating invalid email address.

                            ' Sends user an email ENDS
                            ' Response Table visible set
                            lblInvalidEmail.Visible = True
                            lblInvalidEmail.Text = "Email does not exist."
                        End If
                    Catch ex As Exception
                        Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                    End Try
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try

            ' Labels Set
            lblResEmail.Text = txtEmail.Text
            lblResComments.Text = txtComments.Text
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            SetProperties()
            EmailUnsubscribeInfo()
            If Not IsPostBack Then
                If Len(Request.QueryString("ConfirmationCode")) > 0 Then
                    Dim sEmail As String = Request.QueryString("ConfirmationCode")
                    Try
                        sEmail = Aptify.Framework.Web.Common.WebCryptography.Decrypt(sEmail)
                    Catch ex As Exception
                        ' Bad encryption
                        sEmail = ""
                    End Try
                    tblresponse.Visible = True
                    tblRequest.Visible = False
                    lblComments.Visible = False
                    trResponseHeader.Visible = False
                    If ProcessConfirmation(sEmail) Then
                        lblResEmail.Text = sEmail
                        lblResComments.Text = "Your request was processed and you have been removed from all Aptify email distributions. Thank you for your request."
                    Else
                        If Len(sEmail) > 0 Then
                            lblResEmail.Text = sEmail
                        Else
                            lblResEmail.Text = ""
                            lblEmail.Visible = False
                        End If
                        lblResComments.Text = "Your request was NOT processed. Your email address was either not found in the database or there was an internal processing error. Aptify representatives will review this request in the next 2 business days and ensure that you are removed from email distributions. If you receive any additional messages after 2 business days, please let us know. Thank you for your request."
                        If Len(sEmail) > 0 Then
                            SendFailureEmail(sEmail)
                        End If
                    End If
                ElseIf Len(Request.QueryString("EK")) > 0 Then
                    Dim sEK As String = Trim(Request.QueryString("EK"))
                    Dim sEmail As String, sKey As String
                    If sEK.Contains("__") Then
                        sEmail = sEK.Substring(0, sEK.IndexOf("__"))
                        sKey = sEK.Substring(sEmail.Length + 2)
                        ' Confirm the key is equal to the checksum of the asci characters
                        ' KEY VALUE HAS THIS FORMAT
                        ' CHECKSUM_LEN
                        ' Checksum value is the ASCII character values of each character in the email address, the LEN is the length of the email
                        ' Make sure the key is valid
                        txtEmail.Text = sEmail
                        If sKey.Contains("_") Then
                            Dim sChecksum As String, sLen As String
                            sChecksum = sKey.Substring(0, sKey.IndexOf("_"))
                            sLen = sKey.Substring(sKey.IndexOf("_") + 1)
                            If IsNumeric(sChecksum) And IsNumeric(sLen) Then
                                Dim iCheckSum As Integer = CInt(sChecksum)
                                Dim iLen As Integer = CInt(sLen)
                                Dim iCalcCS As Integer = 0
                                For Each c As Char In sEmail
                                    iCalcCS += Asc(c)
                                Next
                                If iLen = sEmail.Length AndAlso iCalcCS = iCheckSum Then
                                    Response.Redirect(Request.Path & "?ConfirmationCode=" & _
                                                      System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(sEmail)))
                                End If
                            End If
                        End If
                    End If
                Else
                    ' ordinary request
                    tblresponse.Visible = False
                End If
            End If
        End Sub

        Protected Overridable Sub SendFailureEmail(ByVal sEmail As String)
            Dim sbEmail As New System.Text.StringBuilder
            Dim oMailMessage As New Mail.MailMessage

            Try
                ' build the email body using a string builder
                With sbEmail
                    .AppendLine("An error took place attempting to unsubscribe " & sEmail & " from email messages. Please review this ASAP. Thank you.")
                End With

                ' build the email message
                With oMailMessage
                    .From = ConfigurationSettings.AppSettings("MailFromFailure")
                    ' Sends an email to Sales

                    .To = ConfigurationSettings.AppSettings("MailToFailure")
                    .Subject = ConfigurationSettings.AppSettings("MailSubjectFailure")
                    ' Sends and email to myself.
                    .Cc = ConfigurationSettings.AppSettings("MailCCFailure")
                    .Body = sbEmail.ToString
                    .BodyFormat = Mail.MailFormat.Html
                    .Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", ConfigurationSettings.AppSettings("MailSMTPAuthenticate")) 'basic authentication
                    .Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", ConfigurationSettings.AppSettings("MailUserName")) 'set your username here
                    .Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", ConfigurationSettings.AppSettings("MailPassword")) 'set your password here
                End With

                ' Send the email using the .NET Framework
                Mail.SmtpMail.SmtpServer = ConfigurationSettings.AppSettings("MailServer")
                Mail.SmtpMail.Send(oMailMessage)

                ' Sends user an email ENDS
                ' Response Table visible set
                tblRequest.Visible = False
                tblresponse.Visible = True
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Overridable Function ProcessConfirmation(ByVal sEmail As String) As Boolean
            Try
                Dim oPersons As System.Collections.Generic.List(Of Long) = GetPersonIDFromEmail(sEmail)
                If oPersons IsNot Nothing AndAlso _
                   oPersons.Count > 0 Then
                    Dim oContactGE As Aptify.Framework.BusinessLogic.GenericEntity.AptifyGenericEntityBase
                    oContactGE = AptifyApplication.GetEntityObject("Contact Log", -1)
                    oContactGE.SetValue("Description", "Unsubscribe from Emails Processed")
                    oContactGE.SetValue("Direction", "Inbound")
                    oContactGE.SetValue("TypeID", 3)
                    oContactGE.SetValue("CategoryID", 5)

                    Dim oGE As Aptify.Framework.BusinessLogic.GenericEntity.AptifyGenericEntityBase
                    Dim iItemsSaved As Integer = 0
                    For Each lPersonID As Long In oPersons
                        oGE = Me.AptifyApplication.GetEntityObject("Persons", lPersonID)
                        If Not CBool(oGE.GetValue("EmailExclude")) Then
                            oGE.SetValue("EmailExclude", "1")
                            If oGE.Save(False) Then
                                iItemsSaved += 1
                            End If
                        End If
                        With oContactGE.SubTypes("ContactLogLinks").Add
                            .SetValue("AltID", lPersonID)
                            .SetValue("EntityID", AptifyApplication.GetEntityID("Persons"))
                        End With
                    Next

                    Return oContactGE.Save(False) ' Always save the contact log, even if we already had a process previously, this logs the fact that they clicked and requested this.
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return False
            End Try
        End Function

        Protected Overridable Function GetPersonIDFromEmail(ByVal Email As String) As System.Collections.Generic.List(Of Long)
            Try
                Dim oList As New System.Collections.Generic.List(Of Long)
                If Len(Email) > 0 Then
                    'Hrushikesh Jog. Use of parameterized query for issue # 6594. (SQL injection vulnerabilities.)
                    'Hrushikesh Jog. Use of parameterized query for issue # 6594. (SQL injection vulnerabilities.)
                    Dim oParams() As System.Data.IDataParameter
                    ReDim oParams(0)

                    oParams(0) = Me.DataAction.GetDataParameter("@Email", Data.SqlDbType.NVarChar, 100, Email.Trim)

                    Dim dt As System.Data.DataTable, sSQL As String
                    'Suvarna Issue 12351 12/01/2011 Commented and added for Dynamic DB Name chage
                    'sSQL = "SELECT ID FROM APTIFY..vwPersons WHERE Email1= @Email OR Email2 = @Email OR Email3 = @Email"
                    sSQL = "SELECT ID FROM " & AptifyApplication.GetEntityBaseDatabase("Persons") & "..vwPersons WHERE Email1= @Email OR Email2 = @Email OR Email3 = @Email"

                    'Hrushikesh Jog. Use of parameterized query for issue # 6594. (SQL injection vulnerabilities.)
                    'dt = Me.DataAction.GetDataTable(sSQL, Aptify.Framework.DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                    dt = Me.DataAction.GetDataTableParametrized(sSQL, Data.CommandType.Text, oParams)

                    If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                        For Each dr As System.Data.DataRow In dt.Rows
                            oList.Add(CLng(dr(0)))
                        Next
                    End If
                End If
                Return oList
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return Nothing
            End Try
        End Function

        Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
            lblInvalidEmail.Visible = False
            txtComments.Text = ""
        End Sub
        'Suraj Issue 13221, 2/19/13 , add the proper description for email unsubscribe page and add the oraganization name from nav file.
        Protected Overridable Sub EmailUnsubscribeInfo()
            lblinfo.Text = "Please enter your email address if you would like to be removed from " & ATTRIBUTE_EmailUnsubscribeOrganizationName & " email distributions. After you complete this form, an email will be sent to the address specified below with a link that " & "<u>" &
"must be clicked to confirm removal from the list." & "</u>" & " Please check your email after you complete this form and make sure to click on the link in the email to complete the  unsubscribe process. " & "<br /><br />" & "The reason for this extra step is to ensure verification of the email account owner. " & _
 "<br />" & " Thank you for completing this form and we apologize for any inconvenience that prior messages may have caused. " & "<br /><br />"

           
        End Sub
    End Class
End Namespace
