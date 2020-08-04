'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On
 
Imports System.Data

Namespace Aptify.Framework.Web.eBusiness.Knowledge
    Partial Class Surveys
        Inherits BaseUserControlAdvanced

        Public Const cnstTargetPage As String = "TargetPage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "Surveys"

        Private _TargetPage As String
        Private m_bStartNewDataExists As Boolean = False

#Region "Surveys Specific Properties"
        Public Property TargetPage() As String
            Get
                Dim obj As Object = ViewState(cnstTargetPage)
                Return CType(IIf(obj Is Nothing, String.Empty, obj), String)

            End Get
            Set(ByVal Value As String)
                ViewState(cnstTargetPage) = Me.FixLinkForVirtualPath(Value)
            End Set
        End Property
#End Region

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(TargetPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                TargetPage = Me.GetLinkValueFromXML(cnstTargetPage)
            End If
        End Sub

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            m_bStartNewDataExists = False

            QuestionTreeList.UserCredentials = Me.AptifyApplication.UserCredentials
            QuestionTreeList.PersonID = User1.PersonID
        End Sub

        Public Function GetCategoryName(ByVal Container As WebControls.DataGridItem) As String
            Return QuestionTreeList.GetCategoryName(CType(Container.DataItem, Data.DataRowView).Row)
        End Function

        Public Function GetQuestionTreeDescription(ByVal Container As WebControls.DataGridItem) As String
            Return QuestionTreeList.GetQuestionTreeDescription(CType(Container.DataItem, Data.DataRowView).Row)
        End Function

        Public Function GetQuestionTreeName(ByVal Container As WebControls.DataGridItem) As String
            Dim SB As System.Text.StringBuilder
            Dim Row As DataRow = CType(Container.DataItem, Data.DataRowView).Row
            With QuestionTreeList
                'check to see if this person has ever taken a questiontree
                If (QuestionTreeList.GetParticipantID(Row) < 1 Or QuestionTreeList.PersonID < 1) Then
                    'they have not so show the link
                    SB = New System.Text.StringBuilder("<span class='" & .cnstQuestionTreeName & "'>")
                    SB.Append("<a href='" & TargetPage)
                    SB.Append("?" & .cnstQuestionTree & "=" & .GetQuestionTreeID(Row))
                    SB.Append("&" & .cnstIsComplete & "=" & GetSessionIsCompleted(Container))
                    SB.Append("&" & .cnstResult & "=" & .GetResultID(Row))
                    SB.Append("&" & .cnstParticipant & "=" & .GetParticipantID(Row))
                    SB.Append("'>")
                    SB.Append(.GetQuestionTreeName(Row))
                    SB.Append("</a></span>")
                    Return SB.ToString
                Else
                    'the have so check to see if the survey was anonymous
                    If .GetTrackingTypeName(Row) = .cnstTrackingAnonymous Then
                        'it was anonymous
                        'so we won't have results just show the survey name
                        Return .GetQuestionTreeName(Row)
                    Else
                        'it wasn't anonymous, so we should have results ot let them finish
                        SB = New System.Text.StringBuilder("<span class='" & .cnstQuestionTreeName & "'>")
                        SB.Append("<a href='" & TargetPage)
                        SB.Append("?" & .cnstQuestionTree & "=" & .GetQuestionTreeID(Row))
                        SB.Append("&" & .cnstIsComplete & "=" & GetSessionIsCompleted(Container))
                        SB.Append("&" & .cnstResult & "=" & .GetResultID(Row))
                        SB.Append("&" & .cnstParticipant & "=" & .GetParticipantID(Row))
                        SB.Append("'>")
                        SB.Append(.GetQuestionTreeName(Row))
                        SB.Append("</a></span>")
                        Return SB.ToString
                    End If
                End If
            End With
        End Function

        Public Function GetNewResultLink(ByVal Container As WebControls.DataGridItem) As String
            Dim SB As System.Text.StringBuilder
            Dim Row As DataRow = CType(Container.DataItem, Data.DataRowView).Row
            With QuestionTreeList
                'check to see if we have ever viewed this question tree
                If QuestionTreeList.GetParticipantID(Row) < 1 Or QuestionTreeList.PersonID < 1 Then
                    'it has never been taken so don't worry about showing this link
                    Return Nothing
                Else
                    'check to see if the QuestionTree is open for new results
                    If .GetQuestionTreeStatus(Row) = .cnstStatusInProgress And _
                     .GetQuestionTreeStartDate(Row) <= DateTime.Now And _
                     (.GetQuestionTreeEndDate(Row) >= DateTime.Now Or .GetQuestionTreeEndDate(Row) = DateSerial(1900, 1, 1)) Then
                        'it is open for new results
                        'check to see if we will allow the user to create a new link
                        If .GetAllowDuplicates(Row) And _
                        (.GetTrackingTypeName(Row) = .cnstTrackingAnonymous Or .GetSessionIsComplete(Row)) Then
                            SB = New System.Text.StringBuilder("<span class='" & .cnstQuestionTreeName & "'>")
                            SB.Append("<a href='" & TargetPage)
                            SB.Append("?" & .cnstQuestionTree & "=" & .GetQuestionTreeID(Row))
                            SB.Append("&" & .cnstIsComplete & "=0")
                            SB.Append("&" & .cnstResult & "=-1")
                            SB.Append("&" & .cnstParticipant & "=-1")
                            SB.Append("'>")
                            SB.Append(.GetQuestionTreeName(Row))
                            SB.Append("</a></span>")

                            m_bStartNewDataExists = True

                            Return SB.ToString
                        Else
                            'we won't allow duplicates
                            Return Nothing
                        End If
                    Else
                        'it is not open for new result
                        Return Nothing
                    End If
                End If
            End With
        End Function

        Public Function GetSessionDateCreated(ByVal Container As WebControls.DataGridItem) As String
            Return QuestionTreeList.GetSessionDateCreated(CType(Container.DataItem, Data.DataRowView).Row)
        End Function

        Public Function GetSessionDateUpdated(ByVal Container As WebControls.DataGridItem) As String
            Return QuestionTreeList.GetSessionDateUpdated(CType(Container.DataItem, Data.DataRowView).Row)
        End Function

        Public Function GetTrackingTypeName(ByVal Container As WebControls.DataGridItem) As String
            Return QuestionTreeList.GetTrackingTypeName(CType(Container.DataItem, Data.DataRowView).Row)
        End Function

        Public Function GetSessionIsCompleted(ByVal Container As WebControls.DataGridItem) As Boolean
            Dim Row As DataRow = CType(Container.DataItem, Data.DataRowView).Row
            If QuestionTreeList.GetParticipantID(Row) < 1 Or QuestionTreeList.PersonID < 1 Then
                Return False
            Else
                If QuestionTreeList.GetQuestionTreeStartDate(Row) > DateTime.Now Or _
                (QuestionTreeList.GetQuestionTreeEndDate(Row) < DateTime.Now And QuestionTreeList.GetQuestionTreeEndDate(Row) <> DateSerial(1900, 1, 1)) Or _
                QuestionTreeList.GetSessionIsComplete(Row) Or _
                QuestionTreeList.GetQuestionTreeStatus(Row) = Aptify.Framework.Web.eBusiness.Knowledge.Controls.QuestionTreeListControl.cnstStatusComplete Then
                    Return True
                Else
                    If QuestionTreeList.GetTrackingTypeName(Row) = Aptify.Framework.Web.eBusiness.Knowledge.Controls.QuestionTreeListControl.cnstTrackingAnonymous Then
                        Return True
                    Else
                        Return False
                    End If
                End If
            End If
        End Function

        Public Function GetCSSClass(ByVal Container As WebControls.DataGridItem) As String
            Dim Row As DataRow = CType(Container.DataItem, Data.DataRowView).Row
            With QuestionTreeList
                If .GetParticipantID(Row) < 1 Or .PersonID < 1 Then
                    'check for topic codes
                    If .GetTopicCodeMatch(Row) <> 0 Then
                        'we had a topic code match
                        Return Aptify.Framework.Web.eBusiness.Knowledge.Controls.QuestionTreeListControl.cnstTopicCodeMatch
                    Else
                        'we didn't have a topic code match
                        Return Aptify.Framework.Web.eBusiness.Knowledge.Controls.QuestionTreeListControl.cnstNeverTaken
                    End If
                Else
                    If .GetQuestionTreeStartDate(Row) > DateTime.Now OrElse _
                    (.GetQuestionTreeEndDate(Row) < DateTime.Now And .GetQuestionTreeEndDate(Row) <> DateSerial(1900, 1, 1)) OrElse _
                    .GetSessionIsComplete(Row) OrElse _
                    .GetQuestionTreeStatus(Row) = Aptify.Framework.Web.eBusiness.Knowledge.Controls.QuestionTreeListControl.cnstStatusComplete Then
                        Return Aptify.Framework.Web.eBusiness.Knowledge.Controls.QuestionTreeListControl.cnstStatusComplete
                    Else
                        If .GetTrackingTypeName(Row) = Aptify.Framework.Web.eBusiness.Knowledge.Controls.QuestionTreeListControl.cnstTrackingAnonymous Then
                            Return Aptify.Framework.Web.eBusiness.Knowledge.Controls.QuestionTreeListControl.cnstTrackingAnonymous
                        Else
                            Return Aptify.Framework.Web.eBusiness.Knowledge.Controls.QuestionTreeListControl.cnstNotComplete
                        End If
                    End If
                End If
            End With
        End Function

        Public Function SetContainerCssClass(ByVal Container As WebControls.DataGridItem, ByVal ClassName As String) As String
            Container.CssClass = ClassName
            Return Nothing
        End Function

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'set control properties from XML file if needed
            SetProperties()
            For Each Column As WebControls.TemplateColumn In QuestionTreeList.Columns
                Select Case Column.HeaderText
                    Case "Category", "Name", "Description"
                        Column.Visible = True
                    Case "Start New"
                        Column.Visible = m_bStartNewDataExists
                    Case Else
                        Column.Visible = User1.PersonID > 0
                End Select
            Next
        End Sub
    End Class
End Namespace
