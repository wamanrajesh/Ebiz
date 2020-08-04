'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Framework.Application
Imports Aptify.Framework.BusinessLogic
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.AttributeManagement
Imports System.Data

Namespace Aptify.Framework.Web.eBusiness.Directories
    Partial Class CompanyListingControl
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "CompanyListing"

#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()
        End Sub

#End Region


        Private m_iCompanyID As Integer

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try

            
                SetProperties()

                If Not IsPostBack Then
                    If (Me.SetControlRecordIDFromQueryString() _
                            AndAlso Me.SetControlRecordIDFromParam() _
                            AndAlso Me.ControlRecordID > 0) _
                            OrElse Me.IsPageInAdmin Then
                        SetupPage()
                    Else
                        Response.Redirect(Me.GetSecurityErrorPageFromXML & "?Message=" & Server.UrlEncode("Company Listing not available"))
                    End If
                End If
            Catch ex As FormatException
                ' changed code to use config settings instead of hardcoded paths
                Response.Redirect(Me.GetSecurityErrorPageFromXML & "?Message=" & Server.UrlEncode("Company Listing not available"))
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub SetupPage()
       

            Dim sSQL As String
            Dim dt As DataTable

            ' Changes made for to allow encrypting and decrypting the URL.
            ' Changes made by Hrushikesh Jog
            Me.SetControlRecordIDFromParam()
            sSQL = "SELECT Name, AddressLine1=isnull(AddressLine1,''), AddressLine2=isnull(AddressLine2,''), AddressLine3=isnull(AddressLine3,''), City=isnull(City,''), State=isnull(State,''), ZipCode=isnull(ZipCode,''), Country=isnull(Country,''), " & _
                    "MainEmail=isnull(MainEmail,''), InfoEmail=isnull(InfoEmail,''), JobsEmail=isnull(JobsEmail,''), " & _
                    "MainCountryCode=isnull(MainCountryCode,''), MainAreaCode=isnull(MainAreaCode,''), MainPhone=isnull(MainPhone,''), " & _
                    "MainFaxCountryCode=isnull(MainFaxCountryCode,''), MainFaxAreaCode=isnull(MainFaxAreaCode,''), MainFaxNumber=isnull(MainFaxNumber,'') " & _
                    "FROM " & _
                   Database & _
                   "..vwCompanies WHERE ID=" & _
                   Me.ControlRecordID & " AND DirExclude=0"

            dt = DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)

            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                tblMain.Visible = True
                lblError.Visible = False
                lblCompanyName.Text = CStr(dt.Rows(0).Item("Name"))

                lblAddress.Text = CStr(dt.Rows(0).Item("AddressLine1")) & "<BR>" & _
                CStr(IIf(CStr(dt.Rows(0).Item("AddressLine2")) <> "", CStr(dt.Rows(0).Item("AddressLine2")) & "<BR>", "")) & _
                CStr(IIf(CStr(dt.Rows(0).Item("AddressLine3")) <> "", CStr(dt.Rows(0).Item("AddressLine3")) & "<BR>", "")) & _
                CStr(dt.Rows(0).Item("City")) & ", " & _
                CStr(dt.Rows(0).Item("State")) & " " & _
                CStr(dt.Rows(0).Item("ZipCode")) & "<BR>" & _
                CStr(dt.Rows(0).Item("Country"))

                If Not IsDBNull(dt.Rows(0).Item("MainEmail")) Then
                    lblMainEmail.Text = CStr(dt.Rows(0).Item("MainEmail"))
                End If
                If Not IsDBNull(dt.Rows(0).Item("InfoEmail")) Then
                    lblInfoEmail.Text = CStr(dt.Rows(0).Item("InfoEmail"))
                End If
                If Not IsDBNull(dt.Rows(0).Item("JobsEmail")) Then
                    lblJobsEmail.Text = CStr(dt.Rows(0).Item("JobsEmail"))
                End If
                If Not IsDBNull(dt.Rows(0).Item("MainCountryCode")) Then
                    lblPhone.Text = CStr(dt.Rows(0).Item("MainCountryCode")) & " " & _
                                    CStr(dt.Rows(0).Item("MainAreaCode")) & " " & _
                                    CStr(dt.Rows(0).Item("MainPhone"))
                End If

                If Not IsDBNull(dt.Rows(0).Item("MainFaxCountryCode")) Then
                    lblFax.Text = CStr(dt.Rows(0).Item("MainFaxCountryCode")) & " " & _
                    CStr(dt.Rows(0).Item("MainFaxAreaCode")) & " " & _
                    CStr(dt.Rows(0).Item("MainFaxNumber")) & " "
                End If
            Else
                tblMain.Visible = False
                lblError.Visible = True
            End If

        End Sub

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            'if values are not provide directly or from the XML file, set default values for inherited properties since 
            'control requires them to properly function
            If String.IsNullOrEmpty(Me.QueryStringRecordIDParameter) Then Me.QueryStringRecordIDParameter = "ID"
        End Sub

    End Class
End Namespace