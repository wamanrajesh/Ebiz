'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On
Imports Telerik.Web.UI
Imports System.IO

Namespace Aptify.Framework.Web.eBusiness
    Partial Class MemberCEUSubmission
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced
        Protected Const ATTRIBUTE_CERTIFICATION_PAGE As String = "CertificationsPage"
        Protected Const ATTRIBUTE_SUBMISSION_COMPLETE_PAGE As String = "SubmissionCompletePage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "CEUSubmission"
        Protected Const ATTRIBUTE_LOGIN_PAGE As String = "LoginPage"
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


        Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
            SetProperties()
            If Not IsPostBack Then
                If User1.UserID > 0 Then
                    ViewState("PersonID") = Request.QueryString("ID")
                    SetPerson()
                    LoadCEUType() 'Load CEU Type dropdown box
                    ResetErrorLabels() 'Clear all Error Labels
                Else
                    Session.Add("ReturnToPage", Request.RawUrl)
                    Response.Redirect(LoginPage)
                End If
            End If
        End Sub
        Protected Overridable Sub SetPerson()
            If ViewState("PersonID") IsNot Nothing AndAlso CLng(ViewState("PersonID")) > 0 Then
                Dim oGE As Aptify.Applications.Persons.PersonsEntity = DirectCast(Me.AptifyApplication.GetEntityObject("Persons", CLng(ViewState("PersonID"))), Aptify.Applications.Persons.PersonsEntity)
                Me.txtMember.Text = oGE.FirstLast
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
            If String.IsNullOrEmpty(LoginPage) Then
                LoginPage = Me.GetLinkValueFromXML(ATTRIBUTE_LOGIN_PAGE)
            End If
        End Sub
        ''' <summary>
        ''' This method used to load CEU Type
        ''' </summary>
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
        ''' <summary>
        ''' This method used to Validation 
        ''' </summary>
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
        ''' <summary>
        ''' This method used to cleare the error lable on a page
        ''' </summary>
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
        ''' <summary>
        ''' This method used to cleare the control
        ''' </summary>
        Protected Overridable Sub ClearForm()
            Me.txtTitle.Text = ""
            Me.txtUnitsEarned.Text = ""
            Me.drpTitle.SelectedIndex = 0
        End Sub
        ''' <summary>
        ''' This event used to save datafor certification 
        ''' </summary>
        Protected Sub inptSubmit_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles inptSubmit.Click
            Try
                Dim sFile As String, sName As String
                Me.ResetErrorLabels()
                If ValidateForm() Then
                    Dim oCertGE As Aptify.Framework.BusinessLogic.GenericEntity.AptifyGenericEntityBase
                    oCertGE = Me.AptifyApplication.GetEntityObject("Certifications", -1)
                    With oCertGE
                        .SetValue("Type", "External")
                        If ViewState("PersonID") IsNot Nothing AndAlso CLng(ViewState("PersonID")) > 0 Then
                            .SetValue("StudentID", CLng(ViewState("PersonID")))
                        End If
                        .SetValue("DateGranted", Me.dtpDateGranted.SelectedDate)
                        .SetValue("DateStarted", Me.dtpDateStarted.SelectedDate)
                        .SetValue("Units", Me.txtUnitsEarned.Text)
                        .SetValue("Title", Me.txtTitle.Text)
                        .SetValue("Status", Me.lblStatus.Text)
                        If Not (Me.dtpExpirationDate.SelectedDate = Today.Date) Then
                            .SetValue("ExpirationDate", dtpExpirationDate.SelectedDate)
                        End If
                        .SetValue("CourseID", Me.drpTitle.SelectedValue)
                        If Not .Save() Then
                            Me.lblErrorSubmit.Text = "There was an error submitting this form. Please try again later."
                            Me.lblErrorSubmit.Visible = True
                        Else
                            'Anil B For issue 14344 on 28-03-2013
                            'replace file asp upload control in to rad asyncupload control
                            If radCEUDocumentUpload.UploadedFiles.Count > 0 Then
                                sName = radCEUDocumentUpload.UploadedFiles(0).FileName
                                Dim file1 As UploadedFile = radCEUDocumentUpload.UploadedFiles(0)
                                Dim sFileExtension As String = String.Empty
                                sFileExtension = file1.GetName
                                Dim sFileUpload As String = Path.GetExtension(sFileExtension)
                                Dim oAttach As New Aptify.Framework.Application.AptifyAttachments(Me.AptifyApplication, .EntityName, .RecordID)

                                sFile = System.Environment.GetEnvironmentVariable("TEMP") & "\" & sName
                                If System.IO.File.Exists(sFile) Then
                                    System.IO.File.Delete(sFile)
                                End If
                                radCEUDocumentUpload.UploadedFiles(0).SaveAs(sFile)

                                If oAttach.AddAttachment(sFile, Me.AptifyApplication.GetEntityRecordIDFromRecordName("Attachment Categories", "CEUs"), "File Uploaded with online CEU submission on " & Today.ToString) Then
                                    If System.IO.File.Exists(sFile) Then
                                        System.IO.File.Delete(sFile)
                                    End If
                                Else
                                    Me.lblErrorSubmit.Text = "Error Uploading File. Please reselect file and try again."
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
                                sRedirect &= "?ID=" + CStr(ViewState("PersonID"))
                                Response.Redirect(sRedirect)
                            End If
                        End If
                    End With
                Else
                End If
            Catch ex As Exception
                Me.lblErrorSubmit.Text = "There was an error submitting this form. Please try again later."
                Me.lblErrorSubmit.Visible = True
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try

        End Sub
        ''' <summary>
        ''' This event used to redirect to member certification detail page
        ''' </summary>
        Protected Sub lnkGoBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkGoBack.Click
            Try
                If ViewState("PersonID") IsNot Nothing AndAlso CInt(ViewState("PersonID")) <> -1 Then
                    MyBase.Response.Redirect(CertificationsPage & "?ID=" + CStr(ViewState("PersonID")), False)
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try

        End Sub
    End Class
End Namespace

