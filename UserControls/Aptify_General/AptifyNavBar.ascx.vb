'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On
Imports Aptify.Framework.Web.eBusiness
Imports Telerik.Sitefinity.Web
Imports Telerik.Web.UI
Imports Telerik.Sitefinity


Namespace Aptify.PublicWebSite
    Partial Class AptifyNavBar
        Inherits BaseUserControlAdvanced

        Private m_bShowChildren As Boolean = False
        Private m_bHideIfEmpty As Boolean = True

        Protected Const ATTRIBUTE_NAV_TITLE As String = "NavTitle"
        Protected Const ATTRIBUTE_HIDE_IF_EMPTY As String = "HideIfEmpty"
        Protected Const ATTRIBUTE_SHOW_CHILDREN As String = "ShowChildren"
        Protected Const ATTRIBUTE_SHOW_CURRENT_PAGE As String = "ShowCurrentPage"
        Protected Const ATTRIBUTE_LINK_URL_PREFIX As String = "LinkURLPrefix"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "AptifyNavBar"

#Region "AptifyNavBar Specific Properties"
        Public Overridable Property ShowChildren() As Boolean
            Get
                Return m_bShowChildren
            End Get
            Set(ByVal value As Boolean)
                m_bShowChildren = value
            End Set
        End Property

        Public Overridable Property HideIfEmpty() As Boolean
            Get
                Return m_bHideIfEmpty
            End Get
            Set(ByVal value As Boolean)
                m_bHideIfEmpty = value
            End Set
        End Property
        ''' <summary>
        ''' Set to True if showing peer (same level) pages and you want to display the current page in the list
        ''' </summary>
        Public Overridable Property ShowCurrentPage() As Boolean
            Get
                If ViewState(ATTRIBUTE_SHOW_CURRENT_PAGE) IsNot Nothing Then
                    Return CBool(ViewState(ATTRIBUTE_SHOW_CURRENT_PAGE))
                Else
                    Return False
                End If
            End Get
            Set(ByVal value As Boolean)
                ViewState.Add(ATTRIBUTE_SHOW_CURRENT_PAGE, value)
            End Set
        End Property
        ''' <summary>
        ''' Link URL Prefix - prefix the URL for each link with this string
        ''' </summary>
        Public Overridable Property LinkURLPrefix() As String
            Get
                If ViewState(ATTRIBUTE_LINK_URL_PREFIX) IsNot Nothing Then
                    Return ViewState(ATTRIBUTE_LINK_URL_PREFIX).ToString
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState.Add(ATTRIBUTE_LINK_URL_PREFIX, value)
            End Set
        End Property

        Public Overridable Property NavTitle() As String
            Get
                Return lblTitle.Text
            End Get
            Set(ByVal value As String)
                lblTitle.Text = value
            End Set
        End Property
#End Region

        Protected Overrides Sub SetProperties()
            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(NavTitle) Then
                'since value is the 'default' check the XML file for possible custom setting
                NavTitle = Me.GetPropertyValueFromXML(ATTRIBUTE_NAV_TITLE)
            End If
            If String.IsNullOrEmpty(LinkURLPrefix) Then
                'since value is the 'default' check the XML file for possible custom setting
                LinkURLPrefix = Me.GetPropertyValueFromXML(ATTRIBUTE_LINK_URL_PREFIX)
                If String.IsNullOrEmpty(LinkURLPrefix) Then
                    'set prefix to root path if nothing is provided
                    LinkURLPrefix = Request.ApplicationPath
                End If
            End If

            Dim sAttribute As String
            Try
                If HideIfEmpty = True Then
                    'since value is the 'default' check the XML file for possible custom setting
                    sAttribute = ATTRIBUTE_HIDE_IF_EMPTY
                    If Not String.IsNullOrEmpty(Me.GetPropertyValueFromXML(sAttribute)) Then
                        HideIfEmpty = CBool(Me.GetPropertyValueFromXML(sAttribute))
                    End If
                End If
                If ShowChildren = False Then
                    'since value is the 'default' check the XML file for possible custom setting
                    sAttribute = ATTRIBUTE_SHOW_CHILDREN
                    If Not String.IsNullOrEmpty(Me.GetPropertyValueFromXML(sAttribute)) Then
                        ShowChildren = CBool(Me.GetPropertyValueFromXML(sAttribute))
                    End If
                End If
                If ShowCurrentPage = False Then
                    'since value is the 'default' check the XML file for possible custom setting
                    sAttribute = ATTRIBUTE_SHOW_CURRENT_PAGE
                    If Not String.IsNullOrEmpty(Me.GetPropertyValueFromXML(sAttribute)) Then
                        ShowCurrentPage = CBool(Me.GetPropertyValueFromXML(sAttribute))
                    End If
                End If
            Catch ex As Exception
                If TypeOf ex Is InvalidCastException Then
                    Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(InvalidCastExceptionForBooleanProperties(sAttribute, ex.Message))
                Else
                    Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                End If
            End Try

        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'set control properties from XML file if needed
            SetProperties()
            Try
                Dim values As New ArrayList()
                Dim oNodes As SiteMapNodeCollection

                If SitefinitySiteMap.GetCurrentNode.HasChildNodes Then
                    oNodes = SitefinitySiteMap.GetCurrentNode.ChildNodes
                Else
                    'siblings
                    oNodes = SitefinitySiteMap.GetCurrentNode.ParentNode.ChildNodes
                End If
                Dim i As Integer = 0

                For Each n As PageSiteNode In oNodes
                    If n.ShowInNavigation Then
                        If String.Compare(Request.RawUrl, LinkURLPrefix & n.Url.Replace("~", ""), True) = 0 Then
                            If Me.ShowCurrentPage Then
                                values.Add(New URLInfo(n.Title, LinkURLPrefix & n.Url.Replace("~", ""), "active"))
                            End If
                        Else
                            values.Add(New URLInfo(n.Title, LinkURLPrefix & n.Url.Replace("~", ""), ""))
                        End If
                    End If
                    i += 1
                Next

                If i > 0 Then
                    Repeater1.DataSource = values
                    Repeater1.DataBind()
                ElseIf m_bHideIfEmpty Then
                    theContainer.Visible = False
                End If

            Catch ex As Exception
            End Try
        End Sub

        Protected Sub Repeater1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles Repeater1.ItemDataBound
            Try

                Dim node As PageSiteNode = TryCast(e.Item.DataItem, PageSiteNode)
                e.Item.Visible = node.ShowInNavigation
            Catch ex As Exception

            End Try
        End Sub
    End Class


    Public Class URLInfo

        Private sTitle As String
        Private sURL As String
        Private sCssClass As String

        Public Sub New(ByVal Title As String, ByVal URL As String, ByVal CssClass As String)
            Me.sTitle = Title
            Me.sURL = URL
            Me.sCssClass = CssClass
        End Sub

        Public Property Title() As String
            Get
                Return sTitle
            End Get
            Set(ByVal Title As String)
                sTitle = Title
            End Set
        End Property

        Public Property URL() As String
            Get
                Return sURL
            End Get
            Set(ByVal URL As String)
                sURL = URL
            End Set
        End Property

        Public Property CssClass() As String
            Get
                Return sCssClass
            End Get
            Set(ByVal CssClass As String)
                sCssClass = CssClass
            End Set
        End Property

    End Class
End Namespace
