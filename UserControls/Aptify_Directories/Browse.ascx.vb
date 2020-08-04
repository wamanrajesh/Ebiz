'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Framework.Application
Imports Aptify.Framework.BusinessLogic
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.AttributeManagement
Imports System.Data

Namespace Aptify.Framework.Web.eBusiness
    Partial Class BrowseControl
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced

        Protected Const ATTRIBUTE_BROWSE_BY_REDIRECT_PAGE As String = "BrowseByRedirectPage"
        Protected Const ATTRIBUTE_SEARCH_REDIRECT_PAGE As String = "SearchRedirectPage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "Browse"

#Region "Browse Specific Properties"
        ''' <summary>
        ''' BrowseByRedirect page url
        ''' </summary>
        Public Overridable Property BrowseByRedirectPage() As String
            Get
                If Not ViewState(ATTRIBUTE_BROWSE_BY_REDIRECT_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_BROWSE_BY_REDIRECT_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_BROWSE_BY_REDIRECT_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' SearchRedirect page url
        ''' </summary>
        Public Overridable Property SearchRedirectPage() As String
            Get
                If Not ViewState(ATTRIBUTE_SEARCH_REDIRECT_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_SEARCH_REDIRECT_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_SEARCH_REDIRECT_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
#End Region

#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
        End Sub

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()
        End Sub

#End Region

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
            If Not IsPostBack Then
                SetupPage()
            End If
        End Sub

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(BrowseByRedirectPage) Then
                BrowseByRedirectPage = Me.GetLinkValueFromXML(ATTRIBUTE_BROWSE_BY_REDIRECT_PAGE)
                If String.IsNullOrEmpty(BrowseByRedirectPage) Then
                    Me.hrefBrowseName.Enabled = False
                    Me.hrefBrowseName.ToolTip = "BrowseByRedirectPage property has not been set."
                    Me.hrefBrowseRegion.Enabled = False
                    Me.hrefBrowseRegion.ToolTip = "BrowseByRedirectPage property has not been set."
                    Me.hrefBrowseState.Enabled = False
                    Me.hrefBrowseState.ToolTip = "BrowseByRedirectPage property has not been set."
                    Me.hrefBrowseCompanyType.Enabled = False
                    Me.hrefBrowseCompanyType.ToolTip = "BrowseByRedirectPage property has not been set."
                Else
                    Me.hrefBrowseName.NavigateUrl = BrowseByRedirectPage
                    Me.hrefBrowseRegion.NavigateUrl = BrowseByRedirectPage
                    Me.hrefBrowseState.NavigateUrl = BrowseByRedirectPage
                    Me.hrefBrowseCompanyType.NavigateUrl = BrowseByRedirectPage
                End If
            Else
                Me.hrefBrowseName.NavigateUrl = BrowseByRedirectPage
                Me.hrefBrowseRegion.NavigateUrl = BrowseByRedirectPage
                Me.hrefBrowseState.NavigateUrl = BrowseByRedirectPage
                Me.hrefBrowseCompanyType.NavigateUrl = BrowseByRedirectPage
            End If

            If String.IsNullOrEmpty(SearchRedirectPage) Then
                SearchRedirectPage = Me.GetLinkValueFromXML(ATTRIBUTE_SEARCH_REDIRECT_PAGE)
                If String.IsNullOrEmpty(SearchRedirectPage) Then
                    Me.hrefSearch.Enabled = False
                    Me.hrefSearch.ToolTip = "SearchRedirectPage property has not been set."
                Else
                    Me.hrefSearch.NavigateUrl = SearchRedirectPage
                End If
            Else
                Me.hrefSearch.NavigateUrl = SearchRedirectPage
            End If

        End Sub

        Private Sub SetupPage()
            Dim sType As String

            Try
                sType = Request.QueryString("Type")
                If Trim$(UCase$(sType)) = "COMPANY" Then
                    lblHeader.Text = "Browse Corporate Member Directory"
                    rowCompanyType.Visible = True
                Else
                    ' individual
                    rowCompanyType.Visible = False
                    lblHeader.Text = "Browse Individual Member Directory"
                End If

                Dim sRegion As String = System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt("Region"))
                Dim sName As String = System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt("Name"))
                Dim sState As String = System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt("State"))
                Dim sCompanyType As String = System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt("CompanyType"))
                sType = System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(sType))

                hrefSearch.NavigateUrl &= "?Type=" & sType

                hrefBrowseName.NavigateUrl &= "?By=" & sName & "&Type=" & sType
                hrefBrowseState.NavigateUrl &= "?By=" & sState & "&Type=" & sType
                hrefBrowseCompanyType.NavigateUrl &= "?By=" & sCompanyType & "&Type=" & sType
                hrefBrowseRegion.NavigateUrl &= "?By=" & sRegion & "&Type=" & sType

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            Finally
            End Try
        End Sub
    End Class
End Namespace
