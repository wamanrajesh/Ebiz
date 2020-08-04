'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports System
Imports System.Data
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.Application
Imports Aptify.Framework.BusinessLogic.ProcessPipeline
Namespace Aptify.Framework.Web.eBusiness
    Partial Class CPEReports
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced
        Protected Const ATTRIBUTE_LOGIN_PAGE As String = "LoginPage" 'Added by Sandeep for Issue 15051 on 12/03/2013
#Region "Public Properties"

        'Added by Sandeep for Issue 15051 on 12/03/2013
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
            If EbusinesUser.UserID > 0 Then
                If Not Me.Request.QueryString("ID") Is Nothing AndAlso CInt(Me.Request.QueryString("ID")) <> -1 Then
                    Dim sPersonID As String = Me.Request.QueryString("ID")
                    ViewState("PersonID") = sPersonID
                Else
                    Exit Sub
                End If
                If Not IsPostBack Then
                    LoadMemberInfo()
                    LoadActiveCertificationsGrid()
                    LoadDeActiveCertificationsGrid()
                    Response.Write("<script>window.print();</script>")
                End If
            End If
            'Added by Sandeep for Issue 15051 on 12/03/2013
            If String.IsNullOrEmpty(LoginPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                LoginPage = Me.GetLinkValueFromXML(ATTRIBUTE_LOGIN_PAGE)
            End If
            If EbusinesUser.UserID < 0 Then
                Response.Redirect(LoginPage) 'Added by Sandeep for Issue 15051 on 12/03/2013
            End If

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
                sSQL = "EXEC spGetStudentCertificationCount @StudentID=" & CInt(ViewState("PersonID"))
                dt = Me.DataAction.GetDataTable(sSQL)
                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                    lblFirstLast.Text = Convert.ToString(dt.Rows(0).Item("FirstLast"))
                    lblTitle.Text = Convert.ToString(dt.Rows(0).Item("Title"))
                    lblCompany.Text = Convert.ToString(dt.Rows(0).Item("Company"))
                    lblTotalUnit.Text = "Total Unit: " & Convert.ToString(dt.Rows(0).Item("Unit"))
                    lblCurrentDate.Text = "Report Date: " & Date.Now.Date
                End If
                dt = Nothing
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        ''' <summary>
        '''this Method is used to load Active certification data
        ''' </summary>
        Protected Overridable Sub LoadActiveCertificationsGrid()
            Try
                Dim sSQL As String
                Dim params(2) As IDataParameter
                Dim dt As Data.DataTable
                Dim m_sDatabase As String = AptifyApplication.GetEntityBaseDatabase("Certifications")
                params(0) = Me.DataAction.GetDataParameter("@PersonID", SqlDbType.BigInt, CInt(ViewState("PersonID")))
                'Anil B for 14344
                'Change parameter name for SP
                If Session.Item("xReportStartDate") IsNot Nothing Then
                    params(1) = Me.DataAction.GetDataParameter("@GrantedOn", SqlDbType.Date, Session.Item("xReportStartDate"))
                Else
                    params(1) = Me.DataAction.GetDataParameter("@GrantedOn", SqlDbType.Date, System.DBNull.Value)
                End If
                If Session.Item("xReportExpiredOn") IsNot Nothing Then
                    params(2) = Me.DataAction.GetDataParameter("@ExpiredOn", SqlDbType.Date, Session.Item("xReportExpiredOn"))
                Else
                    params(2) = Me.DataAction.GetDataParameter("@ExpiredOn", SqlDbType.Date, System.DBNull.Value)
                End If
                sSQL = m_sDatabase & "..spGetCompanyMemberActiveCertificationsDetails"
                dt = Me.DataAction.GetDataTableParametrized(sSQL, CommandType.StoredProcedure, params)
                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                    Me.grdActiveCertifications.AllowPaging = False
                    Me.grdActiveCertifications.DataSource = dt
                    Me.grdActiveCertifications.DataBind()

                    Me.grdActiveCertifications.PagerSettings.Visible = False
                    Me.grdActiveCertifications.Visible = True
                    lblActiveCertification.Visible = True
                Else
                    grdActiveCertifications.Visible = False
                    lblActiveCertification.Visible = False
                End If
                dt = Nothing
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        ''' <summary>
        '''this Method is used to load Expired certification data
        ''' </summary>
        Protected Overridable Sub LoadDeActiveCertificationsGrid()
            Try
                Dim sSQL As String
                Dim params(2) As IDataParameter
                Dim dt As Data.DataTable
                Dim m_sDatabase As String = AptifyApplication.GetEntityBaseDatabase("Certifications")
                params(0) = Me.DataAction.GetDataParameter("@PersonID", SqlDbType.BigInt, CInt(ViewState("PersonID")))
                'Anil B for 14344
                'Change parameter name for SP
                If Session.Item("xReportStartDate") IsNot Nothing Then
                    params(1) = Me.DataAction.GetDataParameter("@GrantedOn", SqlDbType.Date, Session.Item("xReportStartDate"))
                Else
                    params(1) = Me.DataAction.GetDataParameter("@GrantedOn", SqlDbType.Date, System.DBNull.Value)
                End If
                If Session.Item("xReportExpiredOn") IsNot Nothing Then
                    params(2) = Me.DataAction.GetDataParameter("@ExpiredOn", SqlDbType.Date, Session.Item("xReportExpiredOn"))
                Else
                    params(2) = Me.DataAction.GetDataParameter("@ExpiredOn", SqlDbType.Date, System.DBNull.Value)
                End If
                sSQL = m_sDatabase & "..spGetCompanyMemberDeActiveCertificationsDetails"
                dt = Me.DataAction.GetDataTableParametrized(sSQL, CommandType.StoredProcedure, params)

                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                    Me.grdDeactiveCertifications.AllowPaging = False
                    Me.grdDeactiveCertifications.DataSource = dt
                    Me.grdDeactiveCertifications.DataBind()
                    Me.grdDeactiveCertifications.PagerSettings.Visible = False
                    Me.grdDeactiveCertifications.Visible = True
                    lblDeActiveCertification.Visible = True
                Else
                    lblDeActiveCertification.Visible = False
                    grdDeactiveCertifications.Visible = False
                    If grdActiveCertifications.Visible = False Then
                        lblmsg.Text = "Certification Not Found"
                    End If
                End If
                dt = Nothing
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
    End Class
End Namespace


