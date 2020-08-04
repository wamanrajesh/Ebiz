'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports Telerik.Web.UI
Imports System.IO

Namespace Aptify.Framework.Web.eBusiness.Education
    Partial Class CEUSubmissionControl
        Inherits eBusiness.BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CERTIFICATION_PAGE As String = "CertificationsPage"
        Protected Const ATTRIBUTE_SUBMISSION_COMPLETE_PAGE As String = "SubmissionCompletePage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "CEUSubmission"

#Region "CEUSubmission Specific Properties"
        ''' <summary>
        ''' MyCertifications page url
        ''' </summary>
        Public Property CertificationsPage() As String
            Get
                If Not ViewState(ATTRIBUTE_CERTIFICATION_PAGE) Is Nothing Then
                    Return ViewState(ATTRIBUTE_CERTIFICATION_PAGE).ToString()
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_CERTIFICATION_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' SubmissionComplete page url
        ''' </summary>
        Public Overridable Property SubmissionCompletePage() As String
            Get
                If Not ViewState(ATTRIBUTE_SUBMISSION_COMPLETE_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_SUBMISSION_COMPLETE_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_SUBMISSION_COMPLETE_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
#End Region

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
            If Not IsPostBack Then
                If IsPageInAdmin() Then
                    Me.Visible = False
                Else
                    'If Not Me.User1.PersonID > 0 Then
                    '    'Require(Login)
                    '    Session.Add("ReturnToPage", Request.RawUrl)
                    '    Response.Redirect(Me.LoginPageURL)
                    'ElseIf Not IsPostBack Then
                    'AutoFill(Member)'s Name
                    Me.txtMember.Text = Me.User1.FirstName & " " & Me.User1.LastName

                    LoadCEUType() 'Load CEU Type dropdown box
                    ResetErrorLabels() 'Clear all Error Labels
                    'End If
                End If
            End If
        End Sub

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(CertificationsPage) Then
                CertificationsPage = Me.GetLinkValueFromXML(ATTRIBUTE_CERTIFICATION_PAGE)
                If String.IsNullOrEmpty(CertificationsPage) Then
                    Me.lnkGoBack.Enabled = False
                    Me.lnkGoBack.ToolTip = "CertificationsPage property has not been set."
                End If
            End If
            If String.IsNullOrEmpty(SubmissionCompletePage) Then
                SubmissionCompletePage = Me.GetLinkValueFromXML(ATTRIBUTE_SUBMISSION_COMPLETE_PAGE)
                If String.IsNullOrEmpty(SubmissionCompletePage) Then
                    SubmissionCompletePage = Request.ApplicationPath
                End If
            End If
        End Sub

        Protected Overridable Sub LoadCEUType()
            Try
                Dim sSQL As String, dt As Data.DataTable
                sSQL = "SELECT c.Name, c.ID " & _
                     "FROM " & Me.AptifyApplication.GetEntityBaseDatabase("Course Categories") & _
                              ".." & Me.AptifyApplication.GetEntityBaseView("Course Categories") & " cc " & _
                     "INNER JOIN " & Me.AptifyApplication.GetEntityBaseDatabase("Courses") & _
                              ".." & Me.AptifyApplication.GetEntityBaseView("Courses") & " c " & _
                              " ON cc.ID = c.CategoryID " & _
                     "WHERE (cc.CEUCategory = 1) "
                sSQL &= "AND c.WebEnabled=1 ORDER BY c.Name"

                dt = Me.DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                    Me.drpTitle.Items.Clear()
                    For Each dr As Data.DataRow In dt.Rows
                        Me.drpTitle.Items.Add(New ListItem(dr(0).ToString, dr(1).ToString))
                    Next
                End If
            Catch ex As Exception
                Me.lblErrorSubmit.Text = "No Applicable Courses found."
                Me.lblErrorSubmit.Visible = True
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Overridable Function ValidateForm() As Boolean
            Try
                Dim bValid As Boolean = True
                If Not IsNumeric(Me.txtUnitsEarned.Text) Then
                    Me.lblErrorUnitsEarned.Text = "Enter Valid Units"
                    Me.lblErrorUnitsEarned.Visible = True
                    bValid = False
                End If
                If Me.txtTitle.Text.Length = 0 Then
                    Me.lblErrorTitle.Text = "Enter the Certification's Title"
                    Me.lblErrorTitle.Visible = True
                    bValid = False
                End If
                If CLng(Me.drpTitle.SelectedValue) < 1 Then
                    Me.lblErrorCEUType.Text = "Select a Course"
                    Me.lblErrorCEUType.Visible = True
                    bValid = False
                End If
                Return bValid
            Catch ex As Exception
                Me.lblErrorSubmit.Text = "There was an error submitting this form. Please try again later."
                Me.lblErrorSubmit.Visible = True
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return False
            End Try
        End Function

        Protected Overridable Sub ResetErrorLabels()
            Try
                Me.lblErrorCEUType.Text = ""
                Me.lblErrorCEUType.Visible = False
                Me.lblErrorFile.Text = ""
                Me.lblErrorFile.Visible = False
                Me.lblErrorMember.Text = ""
                Me.lblErrorMember.Visible = False
                Me.lblErrorStatus.Text = ""
                Me.lblErrorStatus.Visible = False
                Me.lblErrorTitle.Text = ""
                Me.lblErrorTitle.Visible = False
                Me.lblErrorUnitsEarned.Text = ""
                Me.lblErrorUnitsEarned.Visible = False
                Me.lblErrorSubmit.Text = ""
                Me.lblErrorSubmit.Visible = False
                Me.lblSubmitSuccess.Text = ""
                Me.lblSubmitSuccess.Visible = False
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Overridable Sub ClearForm()
            Me.txtTitle.Text = ""
            Me.txtUnitsEarned.Text = ""
            Me.drpTitle.SelectedIndex = 0
        End Sub

        Protected Sub inptSubmit_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles inptSubmit.Click
            Try
                Me.ResetErrorLabels()
                If ValidateForm() Then
                    Dim oCertGE As Aptify.Framework.BusinessLogic.GenericEntity.AptifyGenericEntityBase
                    oCertGE = Me.AptifyApplication.GetEntityObject("Certifications", -1)

                    With oCertGE
                        'Load Attachment if one was selected
                        'Amruta Issue:14903  display message if selected file is not valid and changed fileupload control name
                        If radCEUDocumentUpload.UploadedFiles.Count > 0 Then
                            'Amruta Issue 14903,20/3/2013,Save Certification details if uploaded file is valid
                            Dim sFile As String, sName As String = radCEUDocumentUpload.UploadedFiles(0).FileName

                            Dim file1 As UploadedFile = radCEUDocumentUpload.UploadedFiles(0)
                            Dim sFileExtension As String = String.Empty
                            sFileExtension = file1.GetName
                            Dim sFileUpload As String = Path.GetExtension(sFileExtension)

                            If sFileUpload.Trim().ToLower() = ".txt" OrElse sFileUpload.Trim().ToLower() = ".png" OrElse sFileUpload.Trim().ToLower() = ".jpg" OrElse sFileUpload.Trim().ToLower() = ".doc" OrElse sFileUpload.Trim().ToLower() = ".docx" OrElse sFileUpload.Trim().ToLower() = ".pdf" OrElse sFileUpload.Trim().ToLower() = ".bmp" OrElse sFileUpload.Trim().ToLower() = ".gif" Then
                                'amruta
                                .SetValue("Type", "External")
                                .SetValue("StudentID", Me.User1.PersonID)
                                .SetValue("DateGranted", Me.dtpDateGranted.SelectedDate)
                                .SetValue("DateStarted", Me.dtpDateStarted.SelectedDate)
                                .SetValue("Units", Me.txtUnitsEarned.Text)
                                .SetValue("Title", Me.txtTitle.Text)
                                .SetValue("Status", Me.lblStatus.Text)

                                'Vijay Sitlani - made the changes to resolve Issue 5137.
                                If Not (Me.dtpExpirationDate.SelectedDate = Today.Date) Then
                                    'Expiration Date is not required
                                    .SetValue("ExpirationDate", dtpExpirationDate.SelectedDate)
                                End If
                                .SetValue("CourseID", Me.drpTitle.SelectedValue)
                                If Not .Save() Then
                                    Me.lblErrorSubmit.Text = "There was an error submitting this form. Please try again later."
                                    Me.lblErrorSubmit.Visible = True
                                End If
                                Dim oAttach As New Aptify.Framework.Application.AptifyAttachments(Me.AptifyApplication, .EntityName, .RecordID)

                                sFile = System.Environment.GetEnvironmentVariable("TEMP") & "\" & sName
                                If System.IO.File.Exists(sFile) Then
                                    System.IO.File.Delete(sFile)
                                End If
                                radCEUDocumentUpload.UploadedFiles(0).SaveAs(sFile)

                                If oAttach.AddAttachment(sFile, Me.AptifyApplication.GetEntityRecordIDFromRecordName("Attachment Categories", "CEUs"), "File Uploaded with online CEU submission on " & Today.ToString) Then
                                    'Remove the temporary file
                                    If System.IO.File.Exists(sFile) Then
                                        System.IO.File.Delete(sFile)
                                    End If
                                    If Not .Save() Then
                                        Me.lblErrorSubmit.Text = "There was an error submitting this form. Please try again later."
                                        Me.lblErrorSubmit.Visible = True
                                    End If
                                Else
                                    Me.lblErrorSubmit.Text = "Error Uploading File. Please reselect file and try again."
                                    Me.lblErrorSubmit.Visible = True
                                End If
                            Else
                                lblErrorFile.Visible = True
                                Me.lblErrorFile.Text = "Please select valid file and try again."
                                Me.lblErrorSubmit.Visible = True
                            End If
                        Else
                            .SetValue("Type", "External")
                            .SetValue("StudentID", Me.User1.PersonID)
                            .SetValue("DateGranted", Me.dtpDateGranted.SelectedDate)
                            .SetValue("DateStarted", Me.dtpDateStarted.SelectedDate)
                            .SetValue("Units", Me.txtUnitsEarned.Text)
                            .SetValue("Title", Me.txtTitle.Text)
                            .SetValue("Status", Me.lblStatus.Text)

                            'Vijay Sitlani - made the changes to resolve Issue 5137.
                            If Not (Me.dtpExpirationDate.SelectedDate = Today.Date) Then
                                'Expiration Date is not required
                                .SetValue("ExpirationDate", dtpExpirationDate.SelectedDate)
                            End If
                            .SetValue("CourseID", Me.drpTitle.SelectedValue)
                            If Not .Save() Then
                                Me.lblErrorSubmit.Text = "There was an error submitting this form. Please try again later."
                                Me.lblErrorSubmit.Visible = True
                            End If
                        End If
                   
                        If Not Me.lblErrorSubmit.Visible Then
                            'We will use the SubbmissionComplete page to display successful message
                            'and redirect to MyCertifications page
                            'Create redirectstring
                            Dim sRedirect As String = SubmissionCompletePage '"~/SubmissionComplete.aspx"
                            'Add Message
                            sRedirect &= "?Message=" & Server.UrlEncode("CEU Certificate submission was successful.")
                            'Add Time to display page
                            sRedirect &= "&Timer=" & Server.UrlEncode("4")
                            'Add Redirect Page
                            sRedirect &= "&RedirectPage=" & Server.UrlEncode(Me.CertificationsPage)
                            'Dim sRedirect1 As String = Me.FixLinkForVirtualPath(sRedirect)
                            Response.Redirect(sRedirect)
                        End If

                    End With
                Else
                    Me.lblErrorSubmit.Text = "Form Incomplete"
                    Me.lblErrorSubmit.Visible = True
                End If
            Catch ex As Exception
                Me.lblErrorSubmit.Text = "There was an error submitting this form. Please try again later."
                Me.lblErrorSubmit.Visible = True
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try

        End Sub

        Protected Sub lnkGoBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkGoBack.Click
            Response.Redirect(CertificationsPage)
        End Sub

    End Class
End Namespace
