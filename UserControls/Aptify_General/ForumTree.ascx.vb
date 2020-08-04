'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports Aptify.Framework.BusinessLogic.GenericEntity

Imports Aptify.Framework
Imports Aptify.Framework.DataServices
Imports System.Data
Imports Telerik.Web.UI

Namespace Aptify.Framework.Web.eBusiness
    Partial Class ForumTree
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "ForumTree"

        Protected Overrides Sub SetProperties()
            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()
        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load 
            SetProperties()
            Try
                If Not IsPostBack() Then
                    LoadTree()
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub 

        ''' <summary>
        ''' Load DiscussionForum Tree
        ''' </summary>
        ''' <remarks></remarks>
        Protected Overridable Sub LoadTree()
            Try
                Dim sSQL As String = "SELECT ID, ISNULL(ParentID,-1) ParentID, Name FROM " + AptifyApplication.GetEntityBaseDatabase("Discussion Forums") + ".." + AptifyApplication.GetEntityBaseView("Discussion Forums") + " ORDER BY Name"
                Dim oDA As DataAction = New DataAction(AptifyApplication.UserCredentials)
                Dim odt As DataTable = oDA.GetDataTable(sSQL)

                Dim parentDataRows() As DataRow = odt.Select("ParentID=-1")

                If parentDataRows IsNot Nothing Then
                    For Each rw As DataRow In parentDataRows
                        Dim oNode As RadTreeNode = New RadTreeNode()
                        oNode.NodeTemplate = trvDiscussionForum.NodeTemplate
                        oNode.Value = rw("ID").ToString
                        trvDiscussionForum.Nodes.Add(oNode)
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

            If childDataRows IsNot Nothing Then
                For Each rw As DataRow In childDataRows
                    Dim oChildNode As RadTreeNode = New RadTreeNode()
                    oNode.NodeTemplate = trvDiscussionForum.NodeTemplate
                    oChildNode.Value = rw("ID").ToString
                    oNode.Nodes.Add(oChildNode)
                    Dim lbl As Label = CType(oChildNode.Controls.Item(1), Label)
                    lbl.Text = rw("Name").ToString
                    LoadChildNodes(oChildNode, odt)
                Next
            End If
        End Sub

        ''' <summary>
        ''' Get Selected DiscussionForum Ids as Comma Separated String
        ''' </summary>
        ''' <returns>String of Comma Separated DiscussionForum Ids</returns>
        ''' <remarks></remarks>
        Public Overridable Function GetSelectedDiscussionForumIds() As String
            Try
                Return GetSelectedDiscussionForumIds(trvDiscussionForum.Nodes)
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return ""
            End Try
        End Function

        ''' <summary>
        ''' Get Selected DiscussionForum Ids as Comma Separated String by going through TreeNodeCollection recursively
        ''' </summary>
        ''' <param name="Nodes">RadTreeNodeCollection</param>
        ''' <returns>String of Comma Separated DiscussionForum Ids</returns>
        ''' <remarks></remarks>
        Protected Overridable Function GetSelectedDiscussionForumIds(ByVal Nodes As Telerik.Web.UI.RadTreeNodeCollection) As String
            Dim i As Integer
            Dim sDiscussionForumIds As New StringBuilder(String.Empty)
            Dim sDiscussionForumId As String = ""

            Try
                If Nodes IsNot Nothing Then
                    For i = 0 To Nodes.Count - 1
                        With Nodes(i)
                            If .Checked = True Then
                                If sDiscussionForumIds.Length = 0 Then
                                    sDiscussionForumIds.Append(.Value)
                                Else
                                    sDiscussionForumIds.Append("," & .Value)
                                End If
                            End If
                        End With

                        sDiscussionForumId = GetSelectedDiscussionForumIds(Nodes(i).Nodes)

                        If String.IsNullOrEmpty(sDiscussionForumId) = False Then
                            If String.IsNullOrEmpty(sDiscussionForumIds.ToString()) = False Then
                                sDiscussionForumIds.Append("," & sDiscussionForumId)
                            Else
                                sDiscussionForumIds.Append(sDiscussionForumId)
                            End If
                        End If
                    Next

                End If
                Return sDiscussionForumIds.ToString
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return ""
            End Try
        End Function

        ''' <summary>
        ''' Uncheck All Nodes in the Tree
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Sub UncheckAllNodes()
            RecursiveSetCheckBoxes(trvDiscussionForum.Nodes, False)
        End Sub

        ''' <summary>
        ''' Check/Uncheck passed RadTreeNodeCollection and its Child Nodes according to the passed Boolean value
        ''' </summary>
        ''' <param name="oNodes">RadTreeNodeCollection to be Checked/Unchecked</param>
        ''' <param name="bChecked">True=Check all Nodes, Falue=Uncheck all Nodes</param>
        ''' <remarks></remarks>
        Protected Overridable Sub RecursiveSetCheckBoxes(ByRef oNodes As Telerik.Web.UI.RadTreeNodeCollection, ByVal bChecked As Boolean)
            Dim l As Integer
            Try
                If oNodes IsNot Nothing Then 
                    For l = 0 To oNodes.Count - 1
                        oNodes(l).Checked = bChecked
                        RecursiveSetCheckBoxes(oNodes(l).Nodes, bChecked)
                    Next
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

    End Class
End Namespace
