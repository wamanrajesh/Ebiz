'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Namespace Aptify.Framework.Web.eBusiness.Chapters
    Partial Class ChapterManagementControl
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_MY_CHAPTERS_PAGE As String = "MyChaptersPage"
        Protected Const ATTRIBUTE_FIND_CHAPTERS_PAGE As String = "FindChaptersPage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "ChapterManagementControl"

#Region "ChapterManagementControl Specific Properties"
        ''' <summary>
        ''' ChapterSearch page url
        ''' </summary>
        Public Overridable Property FindChaptersPage() As String
            Get
                If Not ViewState(ATTRIBUTE_FIND_CHAPTERS_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_FIND_CHAPTERS_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_FIND_CHAPTERS_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' MyChapters page url
        ''' </summary>
        Public Overridable Property MyChaptersPage() As String
            Get
                If Not ViewState(ATTRIBUTE_MY_CHAPTERS_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_MY_CHAPTERS_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_MY_CHAPTERS_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
#End Region

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(MyChaptersPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                MyChaptersPage = Me.GetLinkValueFromXML(ATTRIBUTE_MY_CHAPTERS_PAGE)
                If String.IsNullOrEmpty(MyChaptersPage) Then
                    Me.lnkChapters.Enabled = False
                    Me.lnkChapters.ToolTip = "MyChaptersPage property has not been set."
                Else
                    Me.lnkChapters.NavigateUrl = MyChaptersPage
                End If
            Else
                Me.lnkChapters.NavigateUrl = MyChaptersPage
            End If

            If String.IsNullOrEmpty(FindChaptersPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                FindChaptersPage = Me.GetLinkValueFromXML(ATTRIBUTE_FIND_CHAPTERS_PAGE)
                If String.IsNullOrEmpty(FindChaptersPage) Then
                    Me.lnkChapterSearch.Enabled = False
                    Me.lnkChapterSearch.ToolTip = "FindChaptersPage property has not been set."
                Else
                    Me.lnkChapterSearch.NavigateUrl = FindChaptersPage
                End If
            Else
                Me.lnkChapterSearch.NavigateUrl = FindChaptersPage
            End If
        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
        End Sub
    End Class
End Namespace

