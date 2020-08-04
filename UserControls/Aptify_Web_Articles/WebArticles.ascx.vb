'Aptify e-Business 5.5.1, July 2013
Option Strict On
Option Explicit On

Imports System.Data
Imports Aptify.Framework.Application
Imports Aptify.Framework.DataServices

Namespace Aptify.Framework.Web.eBusiness
    Partial Class WebArticles
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced

        Protected Const ATTRIBUTE_ARTICLE_DISPLAY_PAGE As String = "ArticleDisplayPage"
        Protected Const ATTRIBUTE_RSS_CHANNEL_ID As String = "RssChannelID"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "WebArticles"
        Protected Const ATTRIBUTE_ARTICLE_ALL_PAGE As String = "ArticleAllPage"

        Private lRSSChannelID As Long = 0

        ''' <summary>
        ''' To include RSS Feed functionality a RSSChannelID must be specified with a value greater than 0. If this property is set to 0 then the RSS Feed button will not display.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("To include RSS Feed functionality a RSSChannelID must be specified with a value greater than 0. If this property is set to 0 then the RSS Feed button will not display.")> _
        Public Property RSSChannelID() As Long
            Get
                Return lRSSChannelID
            End Get
            Set(ByVal value As Long)
                lRSSChannelID = value
            End Set
        End Property

        ''' <summary>
        ''' RedirectURL
        ''' </summary>
        Public Overridable Property ArticleDisplayPage() As String
            Get
                If Not ViewState(ATTRIBUTE_ARTICLE_DISPLAY_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_ARTICLE_DISPLAY_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_ARTICLE_DISPLAY_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property


        Public Overridable Property ArticleAllPage() As String
            Get
                If Not ViewState(ATTRIBUTE_ARTICLE_ALL_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_ARTICLE_ALL_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_ARTICLE_ALL_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        Public Function GetChannelID() As Long
            Return lRSSChannelID
        End Function

        Public Function IsRSSVisible() As Boolean
            'Return False

            'CP 2/5/09: Show/Hide RSS Control based on setting the channel ID.
            If RSSChannelID > 0 Then
                Return True
            Else
                Return False
            End If

        End Function


        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
            LoadArticles()
            'Dilip  changes for removing View all link 2/16/2012

            'linkViewAll.NavigateUrl = ArticleAllPage
        End Sub

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(ArticleDisplayPage) Then
                ArticleDisplayPage = Me.GetLinkValueFromXML(ATTRIBUTE_ARTICLE_DISPLAY_PAGE)
            End If
            If String.IsNullOrEmpty(ArticleAllPage) Then
                ArticleAllPage = Me.GetLinkValueFromXML(ATTRIBUTE_ARTICLE_ALL_PAGE)
            End If
            'Navin Prasad Issue 13106
            If RSSChannelID = 0 Then
                'since value is the 'default' check the XML file for possible custom setting
                If Not String.IsNullOrEmpty(Me.GetPropertyValueFromXML(ATTRIBUTE_RSS_CHANNEL_ID)) Then
                    RSSChannelID = CLng(Me.GetPropertyValueFromXML(ATTRIBUTE_RSS_CHANNEL_ID))
                    RSSFeed.ChannelID = RSSChannelID
                End If
            End If
        End Sub

        Public Sub DoLoad()
            LoadArticles()
        End Sub
        Private Sub LoadArticles()
            Dim sSQL As String, dt As DataTable, oUser As New eBusiness.User
            Try
                oUser.LoadValuesFromSessionObject(Session)
                sSQL = AptifyApplication.GetEntityBaseDatabase("Web Articles") & _
                       "..spGetPersonTopWebArticles @PersonID=" & _
                       oUser.PersonID
                dt = DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)
                lstArticles.DataSource = dt
                lstArticles.DataBind()
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub lstArticles_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles lstArticles.ItemDataBound

            If (e.Item.ItemType = ListItemType.Item) Or (e.Item.ItemType = ListItemType.AlternatingItem) Then

                If Not String.IsNullOrEmpty(ArticleDisplayPage) Then
                    With CType(e.Item.FindControl("articleLink"), HyperLink)
                        .NavigateUrl = ArticleDisplayPage & "?ID=" & DataBinder.Eval(e.Item.DataItem, "ID").ToString
                        .Text = DataBinder.Eval(e.Item.DataItem, "Name").ToString 
                    End With

                    Dim lbl As Label = CType(e.Item.FindControl("lblDate"), Label)
                    lbl.Text = DataBinder.Eval(e.Item.DataItem, "DateWritten", "{0:MMM dd, yyyy}")
                 
                Else
                    With CType(e.Item.FindControl("articleLink"), HyperLink)
                        .Enabled = False
                        .ToolTip = "ArticleDisplayPage property has not been set."
                    End With
                End If
            End If
        End Sub
    End Class
End Namespace
