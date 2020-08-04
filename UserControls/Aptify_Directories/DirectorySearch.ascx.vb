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
    Partial Class SearchControl
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "DirectoriesSearch"

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

        Private m_bCompany As Boolean

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                ' Changes made for to allow encrypting and decrypting the URL.
                ' Changes made by Hrushikesh Jog
                If Not IsPostBack Then
                    SetupPage()
                End If
                Dim sCompany As String = Request.QueryString("Type")
                sCompany = Aptify.Framework.Web.Common.WebCryptography.Decrypt(sCompany)
                m_bCompany = Trim$(UCase$(sCompany)) = "COMPANY"

            Catch ex As FormatException
                ' changed code to use config settings instead of hardcoded paths
                Response.Redirect(Me.GetSecurityErrorPageFromXML & "?Message=" & Server.UrlEncode("Directory Listing not available"))
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Overrides Sub SetProperties()
            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()
        End Sub

        Private Sub SetupPage()
            'set control properties from XML file if needed
            SetProperties()
            Try
                If m_bCompany Then
                    lblHeader.Text = "Company Directory Search"
                Else
                    lblHeader.Text = "Member Directory Search"
                End If

                txtSearch.Text = ""
                If m_bCompany Then
                    PersonDirectoryGrid.Visible = False
                    CompanyDirectoryGrid.Visible = True
                Else
                    CompanyDirectoryGrid.Visible = False
                    PersonDirectoryGrid.Visible = True
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            Finally
            End Try
        End Sub

        Private Sub DoSearch(ByVal sSearchText As String)
            Dim sSQL As String
            If m_bCompany Then
                'Issue 5102
                'sSQL = "SELECT * FROM " & _
                '       AptifyApplication.GetEntityBaseDatabase("Companies") & _
                '       "..vwCompanies WHERE Name LIKE '%" & sSearchText & _
                '       "%' AND DirExclude=0 ORDER BY Name"
                sSQL = "SELECT ID, Name, WebSite, " & _
                       "CASE WHEN MailExclude=1 THEN '' ELSE AddressLine1 END 'AddressLine1', " & _
                       "CASE WHEN MailExclude=1 THEN '' ELSE City END 'City', " & _
                       "CASE WHEN MailExclude=1 THEN '' ELSE State END 'State', " & _
                       "CASE WHEN MailExclude=1 THEN '' ELSE ZipCode END 'ZipCode' " & _
                       "FROM " & AptifyApplication.GetEntityBaseDatabase("Companies") & "..vwCompanies " & _
                       "WHERE Name LIKE @SearchText " & _
                       "AND DirExclude=0 ORDER BY Name"
            Else
                'Issue 5102
                'sSQL = "SELECT * FROM " & _
                '       AptifyApplication.GetEntityBaseDatabase("Persons") & _
                '       "..vwPersons WHERE NameWCompany LIKE '%" & sSearchText & _
                '       "%' AND DirExclude=0 ORDER BY NameWCompany"
                sSQL = "SELECT ID, FirstLast, Company, " & _
                       "CASE WHEN MailExclude=1 THEN '' ELSE AddressLine1 END 'AddressLine1', " & _
                       "CASE WHEN MailExclude=1 THEN '' ELSE City END 'City', " & _
                       "CASE WHEN MailExclude=1 THEN '' ELSE State END 'State', " & _
                       "CASE WHEN MailExclude=1 THEN '' ELSE ZipCode END 'ZipCode', " & _
                       "CASE WHEN EmailExclude=1 THEN '' ELSE Email1 END 'Email1' " & _
                       "FROM " & AptifyApplication.GetEntityBaseDatabase("Persons") & "..vwPersons " & _
                       "WHERE NameWCompany LIKE @SearchText " & _
                       "AND DirExclude=0 ORDER BY NameWCompany"
            End If

            '2009-02-11 RJK - Contains search
            sSearchText = "%" & sSearchText & "%"
            If m_bCompany Then
                'Suraj Issue 14451 3/13/13 , set property true which is declare on CompanyDirectoryGrid for rad grid first columnsorting will be Ascending 
                CompanyDirectoryGrid.CheckAddExpressionApplicableForCompany = True
                CompanyDirectoryGrid.LoadDataTable(sSQL, sSearchText)
            Else
                'Suraj Issue 14451 3/13/13 , set property true which is declare on PersonDirectoryGrid for rad grid first columnsorting will be Ascending 
                PersonDirectoryGrid.CheckAddExpressionApplicableForPerson = True
                PersonDirectoryGrid.LoadDataTable(sSQL, sSearchText)
            End If
            CompanyDirectoryGrid.Visible = m_bCompany
            PersonDirectoryGrid.Visible = Not m_bCompany
        End Sub

        Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
            If txtSearch.Text.Length > 0 Then
                ' if a post back occurs and the search field is filled,
                ' do the search. This allows the user to just hit Enter
                ' to have the search fire.
                DoSearch(txtSearch.Text)
            End If
        End Sub
    End Class
End Namespace