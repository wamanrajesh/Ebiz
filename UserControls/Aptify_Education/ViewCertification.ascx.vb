'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Namespace Aptify.Framework.Web.eBusiness.Education
    Partial Class ViewCertificationControl
        Inherits eBusiness.BaseUserControlAdvanced

        Protected Const ATTRIBUTE_VIEW_COURSE_PAGE As String = "ViewCoursePage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "ViewCertification"

#Region "ViewCertification Specific Properties"
        ''' <summary>
        ''' ViewCourse page url
        ''' </summary>
        Public Overridable Property ViewCoursePage() As String
            Get
                If Not ViewState(ATTRIBUTE_VIEW_COURSE_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_VIEW_COURSE_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_VIEW_COURSE_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
#End Region

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'set control properties from XML file if needed
            SetProperties()
            If Not IsPostBack Then
                If ValidateCertification() Then
                    LoadCertification()
                Else
                    Me.lblError.Visible = True
                    lblError.Text = "Unauthorized Access to Certification"
                End If
            End If
        End Sub

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(ViewCoursePage) Then
                'since value is the 'default' check the XML file for possible custom setting
                ViewCoursePage = Me.GetLinkValueFromXML(ATTRIBUTE_VIEW_COURSE_PAGE)
            End If

            'if values are not provide directly or from the XML file, set default values for inherited properties since 
            'control requires them to properly function
            If String.IsNullOrEmpty(Me.QueryStringRecordIDParameter) Then Me.QueryStringRecordIDParameter = "CertificationID"

        End Sub

        Protected Overridable Function ValidateCertification() As Boolean
            Dim sSQL As String, lVal As Object

            Try
                'Query String Name used to be CertificationID
                ' Changes made to get the query string name from a property set by CMS
                ' Changes made by CP 7/14/2008
                Me.SetControlRecordIDFromParam()
                If Me.ControlRecordID > 0 Then
                    sSQL = "SELECT ID FROM " & _
                           AptifyApplication.GetEntityBaseDatabase("Certifications") & _
                           "..vwCertifications WHERE ID=" & ControlRecordID & _
                           " AND StudentID=" & Me.User1.PersonID
                    lVal = Me.DataAction.ExecuteScalar(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                    If Not lVal Is Nothing Then
                        Return CLng(lVal) = Me.ControlRecordID
                    Else
                        Return False
                    End If
                End If
            Catch ae As ArgumentException
                Throw
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return False
            End Try
        End Function
        Protected Overridable Sub LoadCertification()
            Dim dt As Data.DataTable, sSQL As String

            Try
                'Query String Name used to be CertificationID
                ' Changes made to get the query string name from a property set by CMS
                ' Changes made by CP 7/14/2008
                Me.SetControlRecordIDFromParam()
                If ControlRecordID > 0 Then
                    sSQL = "SELECT c.*,p.FirstName + ' ' + p.LastName Certificant FROM " & _
                           Me.AptifyApplication.GetEntityBaseDatabase("Certifications") & _
                           "..vwCertifications c INNER JOIN " & Me.Database & "..vwPersons p ON c.StudentID=p.ID WHERE c.ID=" & Me.ControlRecordID
                    dt = Me.DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                    If dt IsNot Nothing AndAlso _
                       dt.Rows.Count > 0 Then
                        With dt.Rows(0)
                            lblID.Text = .Item("ID").ToString.Trim
                            lblCertificant.Text = .Item("Certificant").ToString.Trim
                            lblTitle.Text = .Item("Title").ToString.Trim
                            lblStatus.Text = .Item("Status").ToString.Trim
                            If String.Compare(lblStatus.Text, "Expired", True) = 0 Then
                                lblStatus.ForeColor = Drawing.Color.Red
                                lblStatus.Font.Bold = True
                            Else
                                lblStatus.ForeColor = Drawing.Color.Green
                                lblStatus.Font.Bold = False
                            End If
                            lblDateGranted.Text = CDate(.Item("DateGranted")).ToShortDateString
                            If Not IsDBNull(.Item("ExpirationDate")) AndAlso _
                               Not CDate("1/1/1900") = CDate(.Item("ExpirationDate")) Then
                                lblDateExpires.Text = CDate(.Item("ExpirationDate")).ToShortDateString
                            Else
                                lblDateExpires.Text = "<i>Never<i>"
                            End If
                            Select Case .Item("Type").ToString.Trim.ToUpper
                                Case "CURRICULUM"
                                    lblType.Text = "Curriculum"
                                    lblTypeDetails.Text = .Item("Curriculum").ToString.Trim
                                    '  lnkType.NavigateUrl = "" ' no link
                                    ' Amruta issue 13285 10/11/2012
                                    lnkType.NavigateUrl = ViewCoursePage & "?CourseID=" & .Item("CourseID").ToString

                                Case "EXTERNAL"
                                    lblType.Text = "External"
                                    '  lnkType.NavigateUrl = "" ' no link
                                    ' Amruta issue 13285 10/11/2012
                                    lnkType.NavigateUrl = ViewCoursePage & "?CourseID=" & .Item("CourseID").ToString

                                    lblTypeDetails.Text = .Item("Course").ToString.Trim
                                Case Else
                                    ' course
                                    lblType.Text = .Item("Type").ToString
                                    If String.IsNullOrEmpty(ViewCoursePage) Then
                                        Me.lnkType.Enabled = False
                                        Me.lnkType.ToolTip = "ViewCoursePage property has not been set."
                                    Else
                                        lnkType.NavigateUrl = ViewCoursePage & "?CourseID=" & .Item("CourseID").ToString
                                    End If
                                    lblTypeDetails.Text = .Item("Course").ToString.Trim
                            End Select
                        End With
                    End If
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub 
    End Class
End Namespace