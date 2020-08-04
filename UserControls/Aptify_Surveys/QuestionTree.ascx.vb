'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On


Namespace Aptify.Framework.Web.eBusiness.Surveys
    Partial Class QuestionTreeControl
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_SURVEY_HOME_PAGE As String = "SurveyHomePage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "QuestionTree"

#Region "QuestionTree Specific Properties"
        ''' <summary>
        ''' SurveyHome page url
        ''' </summary>
        Public Overridable Property SurveyHomePage() As String
            Get
                If Not ViewState(ATTRIBUTE_SURVEY_HOME_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_SURVEY_HOME_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_SURVEY_HOME_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
#End Region

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(SurveyHomePage) Then
                'since value is the 'default' check the XML file for possible custom setting
                SurveyHomePage = Me.GetLinkValueFromXML(ATTRIBUTE_SURVEY_HOME_PAGE)
            End If
        End Sub

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            With ctlQuestionTree
                .UserCredentials = Me.AptifyApplication.UserCredentials
                .QuestionTreeID = CLng(Request("QuestionTreeID"))
                '.LoadStyleSheets()
                If Not IsPostBack Then
                    'this code should only be run on initialize
                    .IsComplete = CBool(Request("IsComplete"))
                    .ResultID = CLng(Request("KnowledgeResultID"))
                    .ParticipantID = CLng(Request("ParticipantID"))
                    .AccessPassword = Request("AccessPassword")
                    .PersonID = User1.PersonID
                End If
            End With
        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
            'This should be run everytim the page is loaded
            'With ctlQuestionTree
            '    .UserCredentials = Me.AptifyApplication.UserCredentials
            '    .QuestionTreeID = CLng(Request("QuestionTreeID"))
            '    .LoadStyleSheets()
            '    If Not IsPostBack Then
            '        'this code should only be run on initialize
            '        .IsComplete = CBool(Request("IsComplete"))
            '        .ResultID = CLng(Request("KnowledgeResultID"))
            '        .ParticipantID = CLng(Request("ParticipantID"))
            '        .AccessPassword = Request("AccessPassword")
            '        .PersonID = User1.PersonID
            '    End If
            'End With
        End Sub

        Private Sub QuestionTree_AfterComplete() Handles ctlQuestionTree.Completed
            Page.Session.Item("Pages") = Nothing
            Page.Session.Item("TmpResults") = Nothing
            Page.Session.Item("RequiredITems") = Nothing
            Page.Session.Item("RequiredID") = Nothing
            Page.Response.Redirect(SurveyHomePage)
        End Sub

        Private Sub QuestionTree_AccessDenied() Handles ctlQuestionTree.AccessDenied
            Page.Session.Item("Pages") = Nothing
            Page.Session.Item("TmpResults") = Nothing
            Page.Session.Item("RequiredITems") = Nothing
            Page.Session.Item("RequiredID") = Nothing
            Page.Response.Redirect(SurveyHomePage)
        End Sub

        Private Sub QuestionTree_NeedUser() Handles ctlQuestionTree.NeedUser
            Page.Session.Item("Pages") = Nothing
            Page.Session.Item("TmpResults") = Nothing
            Page.Session.Item("RequiredITems") = Nothing
            Page.Session.Item("RequiredID") = Nothing
            Page.Response.Redirect(SurveyHomePage)
        End Sub

        Protected Sub ctlQuestionTree_SessionUnavailable() Handles ctlQuestionTree.SessionUnavailable
            Page.Session.Item("Pages") = Nothing
            Page.Session.Item("TmpResults") = Nothing
            Page.Session.Item("RequiredITems") = Nothing
            Page.Session.Item("RequiredID") = Nothing
            Page.Response.Redirect(SurveyHomePage)
        End Sub
    End Class
End Namespace
