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
    Partial Class DirectoriesCenter
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced

        Protected Const ATTRIBUTE_BROWSE_PAGE As String = "BrowsePage"
        Protected Const ATTRIBUTE_MARKET_PLACE_PAGE As String = "MarketPlacePage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "DirectoriesCenter"

#Region "DirectoriesCenter Specific Properties"
        ''' <summary>
        ''' Browse page url
        ''' </summary>
        Public Overridable Property BrowsePage() As String
            Get
                If Not ViewState(ATTRIBUTE_BROWSE_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_BROWSE_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_BROWSE_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' MarketPlace page url
        ''' </summary>
        Public Overridable Property MarketPlacePage() As String
            Get
                If Not ViewState(ATTRIBUTE_MARKET_PLACE_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_MARKET_PLACE_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_MARKET_PLACE_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
#End Region

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
        End Sub

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(BrowsePage) Then
                BrowsePage = Me.GetLinkValueFromXML(ATTRIBUTE_BROWSE_PAGE)
                If String.IsNullOrEmpty(BrowsePage) Then
                    Me.memberBrowse.Enabled = False
                    Me.memberBrowse.ToolTip = "BrowsePage property has not been set."
                    Me.companyBrowse.Enabled = False
                    Me.companyBrowse.ToolTip = "BrowsePage property has not been set."
                Else
                    Me.memberBrowse.NavigateUrl = BrowsePage & "?Type=Person"
                    Me.companyBrowse.NavigateUrl = BrowsePage & "?Type=Company"
                End If
            Else
                Me.memberBrowse.NavigateUrl = BrowsePage & "?Type=Person"
                Me.companyBrowse.NavigateUrl = BrowsePage & "?Type=Company"
            End If

            If String.IsNullOrEmpty(MarketPlacePage) Then
                MarketPlacePage = Me.GetLinkValueFromXML(ATTRIBUTE_MARKET_PLACE_PAGE)
                If String.IsNullOrEmpty(MarketPlacePage) Then
                    Me.marketBrowse.Enabled = False
                    Me.marketBrowse.ToolTip = "MarketPlacePage property has not been set."
                Else
                    Me.marketBrowse.NavigateUrl = MarketPlacePage
                End If
            Else
                Me.marketBrowse.NavigateUrl = MarketPlacePage
            End If

        End Sub

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

    End Class
End Namespace