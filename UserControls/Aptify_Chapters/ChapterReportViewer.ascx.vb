'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports System.Data
Imports System.Web
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web


Namespace Aptify.Framework.Web.eBusiness.Chapters
    Partial Class ChapterReportViewerControl
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CHAPTER_REDIRECT_PAGE As String = "ChapterRedirectPage"
        Protected Const ATTRIBUTE_CHAPTER_ID_QUERYSTRING_NAME As String = "ChapterIDQueryStringName"
        Protected Const ATTRIBUTE_REPORT_PAGE_REDIRECT_URL As String = "ReportPageRedirectURL"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "ChapterReportViewer"

#Region "Chapter Specific Properties"

        Private m_sChapterPage As String
        Private m_ChapterIDQueryStringName As String
        Private m_sReportPage As String

        <System.ComponentModel.Category("Chapter Specific Properties")> _
        <System.ComponentModel.DefaultValue("ID")> _
        Public Property ChapterIDQueryString() As String
            Get
                If String.IsNullOrEmpty(m_ChapterIDQueryStringName) Then
                    m_ChapterIDQueryStringName = "ID"
                End If
                Return m_ChapterIDQueryStringName
            End Get
            Set(ByVal value As String)
                m_ChapterIDQueryStringName = value
            End Set
        End Property

        Private Property ChapterID() As Long
            Get
                If ViewState.Item("ID") IsNot Nothing Then
                    Return CLng(ViewState.Item("ID"))
                Else
                    Return -1
                End If
            End Get
            Set(ByVal value As Long)
                ViewState.Item("ID") = value
            End Set
        End Property

        <System.ComponentModel.Category("Chapter Specific Properties")> _
        Public Property ChapterRedirectPage() As String
            Get
                Return m_sChapterPage
            End Get
            Set(ByVal value As String)
                m_sChapterPage = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        <System.ComponentModel.Category("Chapter Specific Properties")> _
        Public Property ReportPageRedirectURL() As String
            Get
                Return m_sReportPage
            End Get
            Set(ByVal value As String)
                m_sReportPage = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
#End Region

#Region "Original Page_Load Not Used"
        'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' Do this on all page hits
        'Dim dt As DataTable
        'Dim sFile As String
        'Dim sSQL As String
        'Dim sFormula As String
        ''Dim sServerName As String
        ''Dim iEndPos As Integer
        'Dim sVirtualDir As String
        'Dim iItem As Integer

        'If Me.SetControlRecordIDFromQueryString() AndAlso _
        '        Me.SetControlRecordIDFromParam() _
        '        OrElse Me.IsPageInAdmin() Then

        '    sSQL = "SELECT URL,Name,SelectionFormula," & _
        '           "DisplayGroupTree FROM " & _
        '           Database & _
        '           "..vwChapterReports WHERE " & _
        '           "ID=" & Me.ControlRecordID.ToString

        '    dt = DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)

        '    If dt.Rows.Count > 0 Then
        '        ' added %VirtualDir% support to this URL
        '        ' 7/30/2003 - RFB
        '        'sVirtualDir = ConfigurationSettings.AppSettings("virtualdir").TrimEnd("/"c) 'obsolete method to access AppSettings
        '        sVirtualDir = System.Configuration.ConfigurationManager.AppSettings("virtualdir").TrimEnd("/"c)

        '        'Issue 4362 MAS
        '        '%20 was making Server.MapPath() fail
        '        sVirtualDir = Replace(sVirtualDir, "%20", " ")
        '        sFile = Replace(CStr(dt.Rows(0).Item("URL")), _
        '                        "%VirtualDir%", sVirtualDir, , , _
        '                        CompareMethod.Text)
        '        sFile = Server.MapPath(sFile)
        '        sFormula = MarkupFormula(CStr(dt.Rows(0).Item("SelectionFormula")))
        '        rptViewerMain.SelectionFormula = sFormula
        '        rptViewerMain.DisplayGroupTree = CBool(dt.Rows(0).Item("DisplayGroupTree"))
        '        rptViewerMain.ReportSource = sFile

        '        ' get the name of the server to setup for the report
        '        ' added 7/30/2003 - RFB
        '        'sServerName = ConfigurationSettings.AppSettings("connectionstring")

        '        'HP - Issue 8237, Servername is not required for report to work, it actually caused validation errors
        '        'sServerName = System.Web.Configuration.WebConfigurationManager.AppSettings("AptifyDBServer") '2006/12/11 MAS
        '        'iEndPos = sServerName.IndexOf(";")
        '        'If iEndPos >= 0 Then '2006/12/11 MAS
        '        '    sServerName = sServerName.Substring(7, iEndPos - 7)
        '        'End If
        '        ''If sServerName = "(local)" Then sServerName = Server.MachineName
        '        'sServerName = Replace(sServerName.ToLower, "(local)", Server.MachineName) '2006/12/11 MAS

        '        For iItem = 0 To rptViewerMain.LogOnInfo.Count - 1
        '            'HP - Issue 8237, Servername is not required for report to work, it actually caused validation errors. 
        '            'Setting the connectionInfo to integrated security bypasses report security credentials
        '            rptViewerMain.LogOnInfo.Item(iItem).ConnectionInfo.IntegratedSecurity = True
        '            'rptViewerMain.LogOnInfo.Item(iItem).ConnectionInfo.ServerName = sServerName
        '        Next

        '        'HP - Issue 8237, no need to refresh reports and cause validation
        '        rptViewerMain.RefreshReport()

        '        lblTitle.Text = CStr(dt.Rows(0).Item("Name"))
        '    Else
        '        lblError.Text = "No results were found"
        '        lblError.Visible = True
        '    End If
        'Else
        '    lblError.Text = "Chapter was not loaded"
        '    lblError.Visible = True
        'End If

        'End Sub
#End Region

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Dim dt As DataTable
            Dim sFile As String
            Dim sSQL As String
            Dim sGroupTree As Boolean
			
			Dim sSelectionFormula As String
            Dim sChapter As String
            'set control properties from XML file if needed
            SetProperties()

            If Me.SetControlRecordIDFromQueryString() AndAlso Me.SetControlRecordIDFromParam() OrElse Me.IsPageInAdmin() Then

                sSQL = "SELECT URL,Name,SelectionFormula," & _
                       "DisplayGroupTree FROM " & _
                       Database & _
                       "..vwChapterReports WHERE " & _
                       "ID=" & Me.ControlRecordID.ToString

                dt = DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)

                If dt.Rows.Count > 0 Then
                    sFile = CStr(dt.Rows(0).Item("URL"))
                    sGroupTree = CBool(dt.Rows(0).Item("DisplayGroupTree"))
                    lblTitle.Text = CStr(dt.Rows(0).Item("Name"))
                    ConfigureCrystalReports(sFile, sGroupTree)
					'Added by Dipali  issue No.13163
					sChapter = GetChapters()
                    sSelectionFormula = CStr(dt.Rows(0).Item("SelectionFormula"))

                    If String.IsNullOrEmpty(sSelectionFormula) Then
						rptViewerMain.RefreshReport()
					Else
                        If sSelectionFormula.Contains("CompanyId") Then
                            sSelectionFormula = sSelectionFormula.Replace("{0}", sChapter)
                        End If
                        rptViewerMain.SelectionFormula = sSelectionFormula
                        rptViewerMain.RefreshReport()
                    End If
                Else
                    lblError.Text = "No results were found"
                    lblError.Visible = True
                End If

            Else
                lblError.Text = "Chapter was not loaded"
                lblError.Visible = True
            End If

        End Sub

		  Function GetChapters() As String
            ' this function loads up the data grid with chapter information
            Dim sSQL, Chapters As String, dt As DataTable

            Try
                sSQL = Database & _
                       "..spGetPersonChapters @PersonID=" & User1.PersonID
                dt = DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                Chapters = dt.Rows(0)("ID").ToString
                Return Chapters
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Function
		
        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(ChapterRedirectPage) Then
                ChapterRedirectPage = Me.GetLinkValueFromXML(ATTRIBUTE_CHAPTER_REDIRECT_PAGE)
                If String.IsNullOrEmpty(ChapterRedirectPage) Then
                    Me.lnkChapter.Enabled = False
                    Me.lnkChapter.ToolTip = "ChapterRedirectPage property has not been set."
                End If
            End If
            If ChapterIDQueryString = "ID" Then
                'since value is the 'default' check the XML file for possible custom setting
                If Not String.IsNullOrEmpty(Me.GetPropertyValueFromXML(ATTRIBUTE_CHAPTER_ID_QUERYSTRING_NAME)) Then
                    ChapterIDQueryString = Me.GetPropertyValueFromXML(ATTRIBUTE_CHAPTER_ID_QUERYSTRING_NAME)
                End If
            End If
            If String.IsNullOrEmpty(ReportPageRedirectURL) Then
                ReportPageRedirectURL = Me.GetLinkValueFromXML(ATTRIBUTE_REPORT_PAGE_REDIRECT_URL)
                If String.IsNullOrEmpty(ReportPageRedirectURL) Then
                    Me.lnkReports.Enabled = False
                    Me.lnkReports.ToolTip = "ReportPageRedirectURL property has not been set."
                End If
            End If

            'if values are not provide directly or from the XML file, set default values for inherited properties since 
            'control requires them to properly function
            If String.IsNullOrEmpty(Me.QueryStringRecordIDParameter) Then Me.QueryStringRecordIDParameter = "ReportID"

        End Sub

        Private Function MarkupFormula(ByVal sFormula As String) As String
            Try
                If Len(sFormula) > 0 Then
                    Dim sOutput As String
                    Dim lID As Long
                    sOutput = sFormula

                    Dim sChapterID As String
                    Dim sQueryStringValue As String = Request.QueryString(Me.ChapterIDQueryString).ToString()
                    If Me.EncryptQueryStringValue Then
                        sChapterID = Aptify.Framework.Web.Common.WebCryptography.Decrypt(sQueryStringValue)
                    Else
                        sChapterID = sQueryStringValue
                    End If

                    If IsNumeric(sChapterID) Then
                        Me.ChapterID = CLng(sChapterID)
                        lID = Me.ChapterID
                        sOutput = Replace(sOutput, "<%ChapterID%>", lID.ToString)
                        lID = User1.PersonID
                        sOutput = Replace(sOutput, "<%PersonID%>", lID.ToString)
                        lID = User1.UserID
                        sOutput = Replace(sOutput, "<%UserID%>", lID.ToString)

                        Return sOutput
                    Else
                        Return ""
                    End If

                Else
                    Return ""
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return Nothing
            End Try
        End Function

        Private Sub lnkChapter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkChapter.Click
            'If IsNumeric(Request.QueryString(Me.ChapterIDQueryString)) Then
            '    Me.ChapterID = CLng(Request.QueryString(Me.ChapterIDQueryString))
            'End If
            'Response.Redirect(Me.ChapterRedirectPage & "?" & Me.ChapterPageQueryStringName & "=" & Me.ChapterID.ToString)

            Dim sChapterID As String
            Dim sQueryStringValue As String = Request.QueryString(Me.ChapterIDQueryString).ToString()
            If Me.EncryptQueryStringValue Then
                sChapterID = Aptify.Framework.Web.Common.WebCryptography.Decrypt(sQueryStringValue)
            Else
                sChapterID = sQueryStringValue
            End If

            If IsNumeric(sChapterID) Then
                Me.ChapterID = CLng(sChapterID)
            End If

            Dim sRedirect As String = ""
            If Me.EncryptQueryStringValue Then
                sRedirect = Me.ChapterRedirectPage & "?" & Me.ChapterIDQueryString & "=" & _
                    System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(ChapterID.ToString))
            Else
                sRedirect = Me.ChapterRedirectPage & "?" & Me.ChapterIDQueryString & "=" & ChapterID.ToString
            End If
            Response.Redirect(sRedirect)
        End Sub

        Private Sub lnkReports_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkReports.Click
            'If IsNumeric(Request.QueryString(Me.ChapterIDQueryString)) Then
            '    Me.ChapterID = CLng(Request.QueryString(Me.ChapterIDQueryString))
            'End If
            'Response.Redirect(Me.ReportPageRedirectURL & "?" & Me.ChapterPageQueryStringName & "=" & Me.ChapterID.ToString)

            Dim sChapterID As String
            Dim sQueryStringValue As String = Request.QueryString(Me.ChapterIDQueryString).ToString()
            If Me.EncryptQueryStringValue Then
                sChapterID = Aptify.Framework.Web.Common.WebCryptography.Decrypt(sQueryStringValue)
            Else
                sChapterID = sQueryStringValue
            End If

            If IsNumeric(sChapterID) Then
                Me.ChapterID = CLng(sChapterID)
            End If

            Dim sRedirect As String = ""
            If Me.EncryptQueryStringValue Then
                'HP - Issue 8273, link breaks chain since property Me.ChapterPageQueryStringName is placing 'ChapterId' as the 
                'identifier when it should be 'ID' therefore replaceing with Me.ChapterIDQueryString to correct
                'sRedirect = Me.ReportPageRedirectURL & "?" & Me.ChapterPageQueryStringName & "=" & _'
                sRedirect = Me.ReportPageRedirectURL & "?" & Me.ChapterIDQueryString & "=" & _
                System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(ChapterID.ToString))
            Else
                'HP - Issue 8273, link breaks chain since property Me.ChapterPageQueryStringName is placing 'ChapterId' as the 
                'identifier when it should be 'ID' therefore replaceing with Me.ChapterIDQueryString to correct
                'sRedirect = Me.ReportPageRedirectURL & "?" & Me.ChapterPageQueryStringName & "=" & ChapterID.ToString
                sRedirect = Me.ReportPageRedirectURL & "?" & Me.ChapterIDQueryString & "=" & ChapterID.ToString
            End If
            Response.Redirect(sRedirect)
        End Sub

        Private Sub ConfigureCrystalReports(ByVal report As String, ByVal grpTree As Boolean)

            Dim sServerName As String = System.Configuration.ConfigurationManager.AppSettings("AptifyDBServer").ToString
            Dim sDatabaseName As String = System.Configuration.ConfigurationManager.AppSettings("AptifyUsersDB").ToUpper
            Dim myConnectionInfo As ConnectionInfo = New ConnectionInfo()

            myConnectionInfo.ServerName = sServerName
            myConnectionInfo.DatabaseName = sDatabaseName
            myConnectionInfo.IntegratedSecurity = True

            Dim rptdoc As ReportDocument = New ReportDocument()
            Dim reportPath As String = Server.MapPath(report)
            rptdoc.Load(reportPath)
            rptViewerMain.ReportSource = rptdoc
            rptViewerMain.DisplayGroupTree = grpTree
            'turn off Cyrstal's logo
            rptViewerMain.HasCrystalLogo = False
            SetDBLogonForReport(myConnectionInfo, rptdoc)
        End Sub

        Private Sub SetDBLogonForReport(ByVal myConnectionInfo As ConnectionInfo, ByVal myReportDocument As ReportDocument)
            Dim myTables As Tables = myReportDocument.Database.Tables
            For Each myTable As CrystalDecisions.CrystalReports.Engine.Table In myTables
                Dim myTableLogonInfo As TableLogOnInfo = myTable.LogOnInfo
                myTableLogonInfo.ConnectionInfo = myConnectionInfo
                myTable.ApplyLogOnInfo(myTableLogonInfo)
            Next
        End Sub

    End Class
End Namespace
