'Aptify e-Business 5.5.1 SR1, June 2014
Option Explicit On
Option Strict On

Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Framework.Application
Imports Aptify.Framework.BusinessLogic
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.AttributeManagement
Imports System.Data

Namespace Aptify.Framework.Web.eBusiness.Directories
    Partial Class BrowseByControl
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "BrowseBy"

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
        Private m_sBrowseBy As String

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
            Try
                ' Changes made for to allow encrypting and decrypting the URL.
                ' Changes made by Hrushikesh Jog

                Dim sCompany As String = Request.QueryString("Type")
                Dim sBrowseBy As String = Request.QueryString("By")

                sCompany = Aptify.Framework.Web.Common.WebCryptography.Decrypt(sCompany)
                sBrowseBy = Aptify.Framework.Web.Common.WebCryptography.Decrypt(sBrowseBy)

                If (sCompany = "" Or sBrowseBy = "") Then
                    'Throw New FormatException
                End If

                m_bCompany = Trim$(UCase$(sCompany)) = "COMPANY"
                m_sBrowseBy = sBrowseBy
                If Not IsPostBack Then
                    SetupPage()
                End If

            Catch ex As FormatException
                ' changed code to use config settings instead of hardcoded paths - CP 7/14/2008
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

            Try
                If Trim$(UCase$(m_sBrowseBy)) = "NAME" And Not m_bCompany Then
                    lblBrowseBy.Text = "Last Name"
                Else
                    lblBrowseBy.Text = m_sBrowseBy
                End If

                If m_bCompany Then
                    PersonDirectoryGrid.Visible = False
                    CompanyDirectoryGrid.Visible = True
                Else
                    CompanyDirectoryGrid.Visible = False
                    PersonDirectoryGrid.Visible = True
                End If

                lstBrowse.Items.Clear()
                Select Case Trim$(UCase$(m_sBrowseBy))
                    Case "NAME"
                        LoadLetters()
                    Case "STATE"
                        LoadStates()
                    Case "REGION"
                        LoadRegions()
                    Case "COMPANYTYPE"
                        LoadCompanyTypes()
                End Select
                If lstBrowse.Items.Count > 0 Then
                    lstBrowse.Items(0).Selected = True
                End If
                LoadGrid()

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            Finally
            End Try
        End Sub

        Private Sub LoadGrid()
            ' This routine loads the grid up based on the browse selection
            Dim sSQL As String

            Try
                sSQL = GetGridSQL()
                If m_bCompany Then
                    'Suraj Issue 14451 3/13/13 , set property true which is declare on CompanyDirectoryGrid for rad grid first columnsorting will be Ascending 
                    CompanyDirectoryGrid.CheckAddExpressionApplicableForCompany = True
                    CompanyDirectoryGrid.LoadDataTable(sSQL)
                Else
                    'Suraj Issue 14451 3/13/13 , set property true which is declare on PersonDirectoryGrid for rad grid first columnsorting will be Ascending 
                    PersonDirectoryGrid.CheckAddExpressionApplicableForPerson = True
                    PersonDirectoryGrid.LoadDataTable(sSQL)
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            Finally
            End Try
        End Sub
        Private Function GetGridSQL() As String
            ' This routine generates a SQL statement based on the Browse Type
            ' and the type of browsing(company or person)
            Dim sDatabase As String = AptifyApplication.GetEntityBaseDatabase("Companies")
            Dim sSQL As String
            Try
                If m_bCompany Then
                    sSQL = "SELECT ID, Name, WebSite, " & _
                           "CASE WHEN MailExclude=1 THEN '' ELSE AddressLine1 END 'AddressLine1', " & _
                           "CASE WHEN MailExclude=1 THEN '' ELSE City END 'City', " & _
                           "CASE WHEN MailExclude=1 THEN '' ELSE State END 'State', " & _
                           "CASE WHEN MailExclude=1 THEN '' ELSE ZipCode END 'ZipCode' " & _
                           "FROM " & sDatabase & "..vwCompanies "
                    Select Case Trim$(UCase$(m_sBrowseBy))
                        Case "NAME"
                            sSQL = sSQL & " WHERE Name LIKE '" & lstBrowse.SelectedItem.Value & "%'"
                        Case "STATE"
                            'Suraj Issue 14451 3/13/13 , check the lisbox vale selected (No State) then  change the where condition if the  State is null or '' 
                            If CStr(lstBrowse.SelectedItem.Value) = "(No State)" Then
                                sSQL = sSQL & " WHERE (State = '' or State is NULL)"
                            Else
                                sSQL = sSQL & " WHERE State ='" & lstBrowse.SelectedItem.Value & "'"
                            End If
                        Case "REGION"
                            'Suraj Issue 14451 3/13/13 , we add the extra codition in where "Or OrganizationID=''"  
                            If CStr(lstBrowse.SelectedItem.Value) = "0" Then
                                sSQL = sSQL & " WHERE (OrganizationID Is Null Or OrganizationID='' Or OrganizationID<=0) "
                            Else
                                sSQL = sSQL & " WHERE OrganizationID=" & lstBrowse.SelectedItem.Value
                            End If

                        Case "COMPANYTYPE"
                            sSQL = sSQL & " WHERE CompanyTypeID=" & lstBrowse.SelectedItem.Value
                    End Select
                    sSQL = sSQL & " AND DirExclude=0 ORDER BY Name"
                Else
                    sDatabase = AptifyApplication.GetEntityBaseDatabase("Persons")
                    'Issue 5102
                    'sSQL = "SELECT ID,FirstLast,Company,AddressLine1,City,State,ZipCode,Email1 FROM " & _
                    '       sDatabase & "..vwPersons "
                    sSQL = "SELECT ID, FirstLast, Company, " & _
                           "CASE WHEN MailExclude=1 THEN '' ELSE AddressLine1 END 'AddressLine1', " & _
                           "CASE WHEN MailExclude=1 THEN '' ELSE City END 'City', " & _
                           "CASE WHEN MailExclude=1 THEN '' ELSE State END 'State', " & _
                           "CASE WHEN MailExclude=1 THEN '' ELSE ZipCode END 'ZipCode', " & _
                           "CASE WHEN EmailExclude=1 THEN '' ELSE Email1 END 'Email1' " & _
                           "FROM " & sDatabase & "..vwPersons "
                    Select Case Trim$(UCase$(m_sBrowseBy))
                        Case "NAME"
                            sSQL = sSQL & " WHERE LastName LIKE '" & lstBrowse.SelectedItem.Value & "%'"
                        Case "STATE"
                            'Suraj Issue 14451 3/13/13 , check the lisbox vale selected (No State) then  change the where condition if the  State is null or '' 
                            If CStr(lstBrowse.SelectedItem.Value) = "(No State)" Then
                                sSQL = sSQL & " WHERE (State = '' or State is NULL)"
                            Else
                                sSQL = sSQL & " WHERE State ='" & lstBrowse.SelectedItem.Value & "'"
                            End If
                        Case "REGION"
                            'Suraj Issue 14451 3/13/13 , we add the extra codition in where "Or OrganizationID=''"  
                            If CStr(lstBrowse.SelectedItem.Value) = "0" Then
                                sSQL = sSQL & " WHERE (OrganizationID Is Null Or OrganizationID='' Or OrganizationID<=0)"
                            Else
                                sSQL = sSQL & " WHERE OrganizationID=" & lstBrowse.SelectedItem.Value
                            End If
                    End Select
                    sSQL = sSQL & " AND DirExclude=0 ORDER BY LastName, FirstName"
                End If
                Return sSQL
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return ""
            End Try
        End Function

        Private Sub LoadLetters()
            ' load up all letters in the alphabet
            ' ASCII characters are sqeuential in coding
            ' sequence from A to Z
            Dim i As Integer
            Try
                For i = Asc("A") To Asc("Z")
                    lstBrowse.Items.Add(Chr(i))
                Next
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            Finally
            End Try
        End Sub

        Private Sub LoadStates()
            ' this routine loads up the states that
            ' are actually in the database, this prevents
            ' the user from selecting states where no 
            ' members exist.
            Dim sSQL As String
            Dim sDatabase As String = AptifyApplication.GetEntityBaseDatabase("Companies")
            Dim dt As DataTable

            Try
                'Suraj Issue 14451 3/13/13 , change the where condition if the  State is null or '' we show it is an "No State" for list box
                sSQL = "SELECT DISTINCT(State) State FROM "
                If m_bCompany Then
                    sSQL = sSQL & sDatabase & "..vwCompanies"
                Else
                    sDatabase = AptifyApplication.GetEntityBaseDatabase("Persons")
                    sSQL = sSQL & sDatabase & "..vwPersons"
                End If
                sSQL = sSQL & " WHERE DirExclude=0 and ( state <> '')  union select '(No State)' as State ORDER BY State"

                ' get the data table and bind to the list
                Try
                    dt = DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)
                    lstBrowse.DataTextField = "State"
                    lstBrowse.DataValueField = "State"
                    lstBrowse.DataSource = dt
                    lstBrowse.DataBind()
                Catch ex As Exception
                    Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                End Try
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub LoadCompanyTypes()
            Dim sSQL As String
            Dim sDatabase As String = AptifyApplication.GetEntityBaseDatabase("Companies")
            Dim dt As DataTable
            'Suraj Issue 14451 4/3/13 ,rollback my chages as per andrew suggetion CompanyTypeID is a required field in Companies. For this control, we can assume that it will never be blank.
            sSQL = "SELECT DISTINCT(CompanyType) CompanyType,CompanyTypeID FROM " & _
                   sDatabase & "..vwCompanies " & _
                   " WHERE DirExclude=0 ORDER BY CompanyType"
            Try
                dt = DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)

                lstBrowse.DataTextField = "CompanyType"
                lstBrowse.DataValueField = "CompanyTypeID"
                lstBrowse.DataSource = dt
                lstBrowse.DataBind()
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Private Sub LoadRegions()
            ' This routine loads up a distinct list
            ' of regions that the company/persons are linked
            ' to
            Dim sSQL As String
            Dim dt As DataTable

            If m_bCompany Then
                sSQL = GetCompanyRegionSQL()
            Else
                sSQL = GetPersonRegionSQL()
            End If

            Try
                dt = DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)

                lstBrowse.DataTextField = "Region"
                lstBrowse.DataValueField = "ID"
                lstBrowse.DataSource = dt
                lstBrowse.DataBind()
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Function GetPersonRegionSQL() As String
            Return GetRegionSQL("vwPersons", "Persons")
        End Function
        Private Function GetCompanyRegionSQL() As String
            Return GetRegionSQL("vwCompanies", "Companies")
        End Function
        Private Function GetRegionSQL(ByVal BaseView As String, _
                                      ByVal Entity As String) As String
            Dim sSQL As String

            sSQL = "SELECT DISTINCT(o.Name) Region,o.ID " & _
                    "FROM " & _
                   AptifyApplication.GetEntityBaseDatabase(Entity) & _
                   ".." & BaseView & " bv LEFT JOIN " & _
                   AptifyApplication.GetEntityBaseDatabase("Organizations") & _
                   "..vwOrganizations o ON bv.OrganizationID=o.ID " & _
                   "WHERE DirExclude=0 and (bv.OrganizationID <> '' AND bv.OrganizationID > 0) UNION SELECT '(No Region)' Region,'0' ID ORDER BY o.Name"
            Return sSQL
        End Function

        Protected Sub lstBrowse_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstBrowse.SelectedIndexChanged
            LoadGrid()
        End Sub
    End Class
End Namespace