'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports System
Imports System.Data
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.Application
Imports Aptify.Framework.BusinessLogic.ProcessPipeline
Imports Aptify.Applications.OrderEntry
Imports Telerik.Web.UI

Namespace Aptify.Framework.Web.eBusiness
    Partial Class MemberCertificationDetails
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced
        Protected Const ATTRIBUTE_CEU_SUBMISSION_PAGE As String = "CEUSubmissionPage"
        Protected Const ATTRIBUTE_VIEW_CERTIFICATION_PAGE As String = "ViewCertificationPage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "MyCertifications"
        Protected Const ATTRIBUTE_CONTORL_CPEREPORT_PAGE As String = "CPETranscriptReport"
        Protected Const ATTRIBUTE_LOGIN_PAGE As String = "LoginPage"
        Dim sUrl As String
#Region "MemberCertificationDetails Specific Properties"
        ''' <summary>
        ''' CEUSubmission page url
        ''' </summary>
        Public Overridable Property CEUSubmissionPage() As String
            Get
                If Not ViewState(ATTRIBUTE_CEU_SUBMISSION_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_CEU_SUBMISSION_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_CEU_SUBMISSION_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' ViewCertification page url
        ''' </summary>
        Public Overridable Property ViewCertificationPage() As String
            Get
                If Not ViewState(ATTRIBUTE_VIEW_CERTIFICATION_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_VIEW_CERTIFICATION_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_VIEW_CERTIFICATION_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        Public Overridable Property CPETranscriptReport() As String
            Get
                If Not ViewState(ATTRIBUTE_CONTORL_CPEREPORT_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_CONTORL_CPEREPORT_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_CONTORL_CPEREPORT_PAGE) = Me.FixLinkForVirtualPath(value)
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

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()
            If String.IsNullOrEmpty(CEUSubmissionPage) Then
                CEUSubmissionPage = Me.GetLinkValueFromXML(ATTRIBUTE_CEU_SUBMISSION_PAGE)
            End If

            If String.IsNullOrEmpty(ViewCertificationPage) Then
                ViewCertificationPage = Me.GetLinkValueFromXML(ATTRIBUTE_VIEW_CERTIFICATION_PAGE)
            End If

            If String.IsNullOrEmpty(CPETranscriptReport) Then
                CPETranscriptReport = Me.GetLinkValueFromXML(ATTRIBUTE_CONTORL_CPEREPORT_PAGE)
                If String.IsNullOrEmpty(CPETranscriptReport) Then
                End If
            End If

            If String.IsNullOrEmpty(LoginPage) Then
                LoginPage = Me.GetLinkValueFromXML(ATTRIBUTE_LOGIN_PAGE)
            End If

        End Sub

        Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
            SetProperties()
            If Not IsPostBack Then
                If User1.UserID > 0 Then
                    If Request.QueryString("ID") IsNot Nothing AndAlso IsNumeric(Request.QueryString("ID")) Then
                        ViewState("PersonID") = Request.QueryString("ID")
                        btnPrint.Visible = True
                        trSearch.Visible = True
                        LoadMemberInfo()
                        'Anil B For issue 14344 on 28-03-2013
                        'set sorting for grid
                        AddExpression()
                    End If
                Else
                    Session.Add("ReturnToPage", Request.RawUrl)
                    Response.Redirect(LoginPage)
                End If
            End If
            'Anil B For issue 14344 on 17-04-2013
            'Open print report on a new tab
            Session.Add("xReportStartDate", txtStartDate.SelectedDate)
            Session.Add("xReportExpiredOn", txtExpiresOn.SelectedDate)
            sUrl = CPETranscriptReport & "?ID=" & CInt(ViewState("PersonID"))
            btnPrint.OnClientClick = "javascript: openNewWin('" & sUrl & "'); return false;"
        End Sub
        ''' <summary>
        '''this Method is used to load the member information with certification unit
        ''' </summary>
        Protected Overridable Sub LoadMemberInfo()
            Try
                Dim sSQL As String
                Dim params(0) As IDataParameter
                Dim m_sDatabase As String = AptifyApplication.GetEntityBaseDatabase("Certifications")
                Dim dt As Data.DataTable
                Dim oGE As Aptify.Applications.Persons.PersonsEntity
                sSQL = "EXEC spGetStudentCertificationCount @StudentID=" & CInt(ViewState("PersonID"))
                dt = Me.DataAction.GetDataTable(sSQL)
                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                    lblMemberName.Text = Convert.ToString(dt.Rows(0).Item("FirstLast"))
                    ViewState("FirstLast") = Convert.ToString(dt.Rows(0).Item("FirstLast"))
                    lblMemberTitle.Text = Convert.ToString(dt.Rows(0).Item("Title"))
                    lblMemberCompany.Text = Convert.ToString(dt.Rows(0).Item("Company"))
                    lblMemberTotalUnitCount.Text = Convert.ToString(dt.Rows(0).Item("Unit"))
                Else
                    oGE = DirectCast(Me.AptifyApplication.GetEntityObject("Persons", CInt(ViewState("PersonID"))), Aptify.Applications.Persons.PersonsEntity)
                    lblMemberName.Text = oGE.FirstLast
                    ViewState("FirstLast") = oGE.FirstLast
                    lblMemberTitle.Text = oGE.Title
                    'Anil B For issue 14344 on 28-03-2013
                    'set company name
                    lblMemberCompany.Text = Convert.ToString(oGE.Company)
                    lblMemberTotalUnitCount.Text = "0"
                    trSearch.Visible = False
                End If
                dt = Nothing
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        ''' <summary>
        '''this Method is used to load Active certification data
        ''' </summary>
        Protected Overridable Sub LoadActiveCertificationsGrid(bSearch As Boolean)
            Try
                Dim sSQL As String
                Dim params(2) As IDataParameter
                Dim dt As Data.DataTable
                Dim m_sDatabase As String = AptifyApplication.GetEntityBaseDatabase("Certifications")
                params(0) = Me.DataAction.GetDataParameter("@PersonID", SqlDbType.BigInt, CInt(ViewState("PersonID")))
                'Anil B For issue 14344 on 28-03-2013
                'change parameter name for procedure
                If txtStartDate.SelectedDate IsNot Nothing Then
                    params(1) = Me.DataAction.GetDataParameter("@GrantedOn", SqlDbType.Date, txtStartDate.SelectedDate)
                Else
                    params(1) = Me.DataAction.GetDataParameter("@GrantedOn", SqlDbType.Date, System.DBNull.Value)
                End If
                If txtExpiresOn.SelectedDate IsNot Nothing Then
                    params(2) = Me.DataAction.GetDataParameter("@ExpiredOn", SqlDbType.Date, txtExpiresOn.SelectedDate)
                Else
                    params(2) = Me.DataAction.GetDataParameter("@ExpiredOn", SqlDbType.Date, System.DBNull.Value)
                End If
                sSQL = m_sDatabase & "..spGetCompanyMemberActiveCertificationsDetails"
                dt = Me.DataAction.GetDataTableParametrized(sSQL, CommandType.StoredProcedure, params)
                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                    Me.grdMembersActiveCertifications.DataSource = dt
                    'Anil B for 14344
                    'Set condition for Grid Load
                    If bSearch Then
                        Me.grdMembersActiveCertifications.DataBind()
                    End If
                    Me.grdMembersActiveCertifications.Visible = True
                    lblActiveCirtification.Visible = True
                    'Anil B For issue 14344 on 28-03-2013
                    'set condition for button
                    btnPrint.Visible = True
                    lblmsg.Text = ""
                Else
                    grdMembersActiveCertifications.Visible = False
                    lblActiveCirtification.Visible = False
                End If
                dt = Nothing
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        ''' <summary>
        '''this Method is used to load Expired certification data
        ''' </summary>
        Protected Overridable Sub LoadDeActiveCertificationsGrid(bSearch As Boolean)
            Try

                Dim sSQL As String
                Dim params(2) As IDataParameter
                Dim dt As Data.DataTable
                Dim m_sDatabase As String = AptifyApplication.GetEntityBaseDatabase("Certifications")
                params(0) = Me.DataAction.GetDataParameter("@PersonID", SqlDbType.BigInt, CInt(ViewState("PersonID")))
                If txtStartDate.SelectedDate IsNot Nothing Then
                    'Anil B For issue 14344 on 28-03-2013
                    'change parameter name for procedure
                    params(1) = Me.DataAction.GetDataParameter("@GrantedOn", SqlDbType.Date, txtStartDate.SelectedDate)
                Else
                    params(1) = Me.DataAction.GetDataParameter("@GrantedOn", SqlDbType.Date, System.DBNull.Value)
                End If
                If txtExpiresOn.SelectedDate IsNot Nothing Then
                    params(2) = Me.DataAction.GetDataParameter("@ExpiredOn", SqlDbType.Date, txtExpiresOn.SelectedDate)
                Else
                    params(2) = Me.DataAction.GetDataParameter("@ExpiredOn", SqlDbType.Date, System.DBNull.Value)
                End If
                sSQL = m_sDatabase & "..spGetCompanyMemberDeActiveCertificationsDetails"
                dt = Me.DataAction.GetDataTableParametrized(sSQL, CommandType.StoredProcedure, params)
                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                    Me.grdMembersDEeActiveCertifications.DataSource = dt
                    'Anil B for 14344
                    'Set condition for Grid Load
                    If bSearch Then
                        Me.grdMembersDEeActiveCertifications.DataBind()
                    End If
                    Me.grdMembersDEeActiveCertifications.Visible = True
                    lblDeActiveCirtification.Visible = True
                    btnPrint.Visible = True
                    lblmsg.Text = ""
                Else
                    lblDeActiveCirtification.Visible = False
                    grdMembersDEeActiveCertifications.Visible = False
                    If grdMembersActiveCertifications.Visible = False Then
                        If ViewState("FirstLast") IsNot Nothing Then
                            lblmsg.Text = "Certification record not found for " + CStr(ViewState("FirstLast"))
                            btnPrint.Visible = False
                        End If
                    End If
                End If
                dt = Nothing
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Protected Sub grdMembersActiveCertifications_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdMembersActiveCertifications.NeedDataSource
            'Anil B for 14344
            'Set condition for Grid Load
            LoadActiveCertificationsGrid(False)
        End Sub
        Protected Sub grdMembersDEeActiveCertifications_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdMembersDEeActiveCertifications.NeedDataSource
            'Anil B for 14344
            'Set condition for Grid Load
            LoadDeActiveCertificationsGrid(False)
        End Sub
        Protected Sub btnPrint_Click(sender As Object, e As System.EventArgs) Handles btnPrint.Click
            Try
                'Anil B For issue 14344 on 17-04-2013
                'Open print report on a new tab
                Session.Add("xReportStartDate", txtStartDate.SelectedDate)
                Session.Add("xReportExpiredOn", txtExpiresOn.SelectedDate)
                sUrl = CPETranscriptReport & "?ID=" & CInt(ViewState("PersonID"))
                btnPrint.OnClientClick = "javascript: openNewWin('" & sUrl & "'); return false;"
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Protected Sub btnSearch_Click(sender As Object, e As System.EventArgs) Handles btnSearch.Click
            Try
                'Anil B for 14344
                'Set condition for Grid Load
                LoadActiveCertificationsGrid(True)
                LoadDeActiveCertificationsGrid(True)
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Protected Sub btnNewCEUSubmission_Click(sender As Object, e As System.EventArgs) Handles btnNewCEUSubmission.Click
            Try
                If CStr(ViewState("PersonID")) IsNot Nothing AndAlso CInt(ViewState("PersonID")) <> -1 Then
                    MyBase.Response.Redirect(CEUSubmissionPage + "?ID=" + CStr(ViewState("PersonID")), False)
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        'Anil B For issue 14344 on 28-03-2013
        'Add function for set sorting
        Private Sub AddExpression()
            Dim ExpOrderSort As New GridSortExpression
            ExpOrderSort.FieldName = "Title"
            ExpOrderSort.SetSortOrder("Ascending")
            grdMembersActiveCertifications.MasterTableView.SortExpressions.AddSortExpression(ExpOrderSort)
            grdMembersDEeActiveCertifications.MasterTableView.SortExpressions.AddSortExpression(ExpOrderSort)
        End Sub
    End Class
End Namespace


