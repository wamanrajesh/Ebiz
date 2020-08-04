'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports System.Data
Imports Telerik.Web.UI
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Framework
Imports Aptify.Framework.DataServices 

Namespace Aptify.Framework.Web.eBusiness.Chapters
    Partial Class ReportsControl
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CHAPTER_REDIRECT_PAGE As String = "ChapterRedirectPage"
        Protected Const ATTRIBUTE_CHAPTER_PAGE_QUERYSTRING_NAME As String = "ChapterPageQueryStringName"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "Reports"
        'Added by Suvarna for IssueID -  15158
        Protected Const ATTRIBUTE_SECURITYERROR_PAGE As String = "securityErrorPage"


#Region "Chapter Specific Properties"

        Private m_sChapterPage As String = ""
        Private m_sChapPageQueryString As String

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
        <System.ComponentModel.DefaultValue("ID")> _
        Public Property ChapterPageQueryStringName() As String
            Get
                If String.IsNullOrEmpty(m_sChapPageQueryString) Then
                    m_sChapPageQueryString = "ID"
                End If
                Return m_sChapPageQueryString
            End Get
            Set(ByVal value As String)
                m_sChapPageQueryString = value
            End Set
        End Property
        ''Added by Suvarna for Issue ID - 15158 used implementation as of on Chapter Controls done by Suraj for 13029
        Public Overridable ReadOnly Property securityErrorPage() As String
            Get
                If Not ViewState.Item(ATTRIBUTE_SECURITYERROR_PAGE) Is Nothing Then
                    Return CStr(ViewState.Item(ATTRIBUTE_SECURITYERROR_PAGE))
                Else
                    'Suraj Issue 13029 take a sequrity page url from web.config file
                    Dim value As String = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings("SecurityErrorPageURL"))
                    If Not String.IsNullOrEmpty(value) Then
                        ViewState.Item(ATTRIBUTE_SECURITYERROR_PAGE) = value
                        Return value
                    Else
                        Return ""
                    End If
                End If
            End Get
        End Property

#End Region

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'Create an object for Commonmethods
            Dim oCommonMethods As New Aptify.Framework.Web.eBusiness.CommonMethods(DataAction, AptifyApplication, User1, Database)
            'set control properties from XML file if needed
            SetProperties()
            Try
                If Not IsPostBack Then
                    ''Added by Suvarna for IssueID - 15158
                    ''verify if the logged in user is associated with that chapter and then only display the infomation,
                    ''otherwise redirect to security page.
                    If User1.UserID > 0 Then
                        Me.SetControlRecordIDFromParam()
                        If oCommonMethods.IsAuthorizedUser(User1.PersonID, Me.ControlRecordID) Then
                            LoadTree()
                            If trvReports.Nodes.Count > 0 Then
                                trvReports.Nodes(0).Selected = True
                                LoadGrid()
                            End If
                        Else
                            Response.Redirect(System.Configuration.ConfigurationManager.AppSettings("virtualdir") & securityErrorPage & "?Message=Access to this Chapter is unauthorized.")
                        End If
                    End If

                   
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(ChapterRedirectPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                ChapterRedirectPage = Me.GetLinkValueFromXML(ATTRIBUTE_CHAPTER_REDIRECT_PAGE)
                If String.IsNullOrEmpty(ChapterRedirectPage) Then
                    Me.lnkChapter.Enabled = False
                    Me.lnkChapter.ToolTip = "ChapterRedirectPage property has not been set."
                End If
            End If
            If ChapterPageQueryStringName = "ID" Then
                'since value is the 'default' check the XML file for possible custom setting
                If Not String.IsNullOrEmpty(Me.GetPropertyValueFromXML(ATTRIBUTE_CHAPTER_PAGE_QUERYSTRING_NAME)) Then
                    ChapterPageQueryStringName = Me.GetPropertyValueFromXML(ATTRIBUTE_CHAPTER_PAGE_QUERYSTRING_NAME)
                End If
            End If
            If String.IsNullOrEmpty(Me.RedirectURL) Then
                Me.grdReports.Enabled = False
                Me.grdReports.ToolTip = "RedirectURL property has not been set."
            End If

            'if values are not provide directly or from the XML file, set default values for inherited properties since 
            'control requires them to properly function
            If String.IsNullOrEmpty(Me.QueryStringRecordIDParameter) Then Me.QueryStringRecordIDParameter = "ID"
            If String.IsNullOrEmpty(Me.RedirectIDParameterName) Then Me.RedirectIDParameterName = "ReportID"

        End Sub 

        Private Sub LoadGrid()
            Dim sSQL As String, dt As DataTable

            sSQL = "SELECT ID,Name,Description FROM " & _
                       Database & "..vwChapterReports" & _
                       " WHERE CategoryID=" & CStr(trvReports.SelectedNode.Value) & _
                       " AND Active=1 ORDER BY DisplaySequence"
            dt = DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
            'Navin Prasad Issue 11145
            Dim dcol As DataColumn = New DataColumn()
            dcol.Caption = "ReportURL"
            dcol.ColumnName = "ReportURL"
            dt.Columns.Add(dcol)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                For Each rw As DataRow In dt.Rows
                    Dim lReportID As Long = -1
                    lReportID = CLng(rw(0))
                    Me.SetControlRecordIDFromParam()
                    Dim sRedirect As String = ""
                    If Me.EncryptQueryStringValue Then
                        sRedirect = Me.RedirectURL & "?" & Me.RedirectIDParameterName & "=" & _
                            System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(lReportID.ToString)) & _
                            "&" & Me.ChapterPageQueryStringName & "=" & _
                            System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(ControlRecordID.ToString))
                    Else
                        sRedirect = Me.RedirectURL & "?" & Me.RedirectIDParameterName & "=" & lReportID.ToString & "&" & Me.ChapterPageQueryStringName & "=" & Me.ControlRecordID.ToString
                    End If

                    rw("ReportURL") = sRedirect
                Next
            End If
            grdReports.DataSource = dt
            grdReports.DataBind()

        End Sub

        Protected Sub grdReports_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdReports.PageIndexChanging
            grdReports.PageIndex = e.NewPageIndex
            LoadGrid()
        End Sub

        Private Sub grdReports_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles grdReports.SelectedIndexChanged
            Dim lReportID As Long
            'Navin Prasad Issue 11032
            Dim lbl As Label = CType(grdReports.SelectedRow.FindControl("lblID"), Label)
            '  lReportID = CLng(grdReports.SelectedRow.Cells(2).Text)
            lReportID = CLng(lbl.Text)
            Me.SetControlRecordIDFromParam()

            Dim sRedirect As String = ""
            If Me.EncryptQueryStringValue Then
                sRedirect = Me.RedirectURL & "?" & Me.RedirectIDParameterName & "=" & _
                    System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(lReportID.ToString)) & _
                    "&" & Me.ChapterPageQueryStringName & "=" & _
                    System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(ControlRecordID.ToString))
            Else
                sRedirect = Me.RedirectURL & "?" & Me.RedirectIDParameterName & "=" & lReportID.ToString & "&" & Me.ChapterPageQueryStringName & "=" & Me.ControlRecordID.ToString
            End If
            Response.Redirect(sRedirect)
        End Sub

        Private Sub lnkChapter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkChapter.Click
            Me.SetControlRecordIDFromParam()

            Dim sRedirect As String = ""
            If Me.EncryptQueryStringValue Then
                sRedirect = Me.ChapterRedirectPage & "?" & Me.ChapterPageQueryStringName & "=" & _
                    System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(ControlRecordID.ToString))
            Else
                sRedirect = Me.ChapterRedirectPage & "?" & Me.ChapterPageQueryStringName & "=" & Me.ControlRecordID
            End If
            Response.Redirect(sRedirect)
        End Sub

        'Navin Prasad Issue 11145
        Protected Sub grdReports_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdReports.DataBound
            Dim tbl As DataTable = CType(grdReports.DataSource, DataTable)
            If tbl IsNot Nothing Then
                For Each rw As GridViewRow In grdReports.Rows
                    Dim lnk As HyperLink = CType(rw.FindControl("lnkReport"), HyperLink)
                    If lnk IsNot Nothing Then
                        lnk.Text = CStr(tbl.Rows(rw.RowIndex)("Name"))
                        lnk.NavigateUrl = CStr(tbl.Rows(rw.RowIndex)("ReportURL"))
                    End If

                Next
            End If
        End Sub

        ''' <summary>
        ''' Load Report Tree
        ''' </summary>
        ''' <remarks></remarks>
        Protected Overridable Sub LoadTree()
            Try
                Dim sSQL As String = "SELECT ID, ISNULL(ParentID,-1) ParentID, Name FROM " + AptifyApplication.GetEntityBaseDatabase("Chapter Report Categories") + ".." + AptifyApplication.GetEntityBaseView("Chapter Report Categories") + " ORDER BY Name"
                Dim oDA As DataAction = New DataAction(AptifyApplication.UserCredentials)
                Dim odt As DataTable = oDA.GetDataTable(sSQL)

                Dim parentDataRows() As DataRow = odt.Select("ParentID=-1")

                If parentDataRows IsNot Nothing Then
                    For Each rw As DataRow In parentDataRows
                        Dim oNode As RadTreeNode = New RadTreeNode()
                        oNode.NodeTemplate = trvReports.NodeTemplate
                        oNode.Value = rw("ID").ToString
                        trvReports.Nodes.Add(oNode)
                        Dim lbl As Label = CType(oNode.Controls.Item(1), Label)
                        lbl.Text = rw("Name").ToString
                        LoadChildNodes(oNode, odt)
                    Next
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        ''' <summary>
        ''' Load the passed Parent Node with its Child Nodes
        ''' </summary>
        ''' <param name="oNode">Parent Node</param>
        ''' <param name="odt">DataTable containing information for creating Child Nodes</param>
        ''' <remarks></remarks>
        Protected Overridable Sub LoadChildNodes(ByVal oNode As RadTreeNode, ByVal odt As DataTable)
            Dim childDataRows() As DataRow = odt.Select("ParentID=" & oNode.Value)

            If childDataRows IsNot Nothing AndAlso childDataRows.Count > 0 Then
                For Each rw As DataRow In childDataRows
                    Dim oChildNode As RadTreeNode = New RadTreeNode()
                    oNode.NodeTemplate = trvReports.NodeTemplate
                    oChildNode.Value = rw("ID").ToString
                    oNode.Nodes.Add(oChildNode)
                    Dim lbl As Label = CType(oChildNode.Controls.Item(1), Label)
                    lbl.Text = rw("Name").ToString
                    LoadChildNodes(oChildNode, odt)
                Next
            End If
        End Sub

        Protected Sub trvReports_NodeClicked(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadTreeNodeEventArgs)
            LoadGrid()
        End Sub

    End Class
End Namespace
