'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports System.Data

Namespace Aptify.Framework.Web.eBusiness.Forums
    Partial Class ForumTitleControl
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_FORUMS_HOME_PAGE As String = "ForumsHomePage"
        Protected Const ATTRIBUTE_SUBSCRIPTIONS_PAGE As String = "SubscriptionsPage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "ForumTitle"

#Region "ForumTitle Specific Properties"
        ''' <summary>
        ''' ForumsHome page url
        ''' </summary>
        Public Overridable Property ForumsHomePage() As String
            Get
                If Not ViewState(ATTRIBUTE_FORUMS_HOME_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_FORUMS_HOME_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_FORUMS_HOME_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' Subscriptions page url
        ''' </summary>
        Public Overridable Property SubscriptionsPage() As String
            Get
                If Not ViewState(ATTRIBUTE_SUBSCRIPTIONS_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_SUBSCRIPTIONS_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_SUBSCRIPTIONS_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        Private Property ParentForumID() As Long
            Get
                If ViewState("ParentForumID") IsNot Nothing Then
                    Return CLng(ViewState("ParentForumID"))
                Else
                    Return -1
                End If
            End Get
            Set(ByVal value As Long)
                ViewState("ParentForumID") = value
            End Set
        End Property
#End Region
        
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
            Try
                If Not IsPostBack Then
                    If MyBase.SetControlRecordIDFromQueryString() AndAlso Me.SetControlRecordIDFromParam() Then
                        LoadForum()
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

            If String.IsNullOrEmpty(ForumsHomePage) Then
                'since value is the 'default' check the XML file for possible custom setting
                ForumsHomePage = Me.GetLinkValueFromXML(ATTRIBUTE_FORUMS_HOME_PAGE)
            End If
            If String.IsNullOrEmpty(SubscriptionsPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                SubscriptionsPage = Me.GetLinkValueFromXML(ATTRIBUTE_SUBSCRIPTIONS_PAGE)
                If String.IsNullOrEmpty(SubscriptionsPage) Then
                    Me.lnkSubscribe.Enabled = False
                    Me.lnkSubscribe.ToolTip = "SubscriptionsPage property has not been set."
                Else
                    Me.lnkSubscribe.NavigateUrl = SubscriptionsPage
                End If
            Else
                Me.lnkSubscribe.NavigateUrl = SubscriptionsPage
            End If
        End Sub

        Private Sub LoadForum()
            Try
                ' load up the forum information on the labels
                Dim sSQL As String
                Dim dt As DataTable
                sSQL = "SELECT df.Name,df.Description,df.ParentID,df.Parent,(SELECT ISNULL(COUNT(*),0) FROM " & _
                       Database & "..vwDiscussionForums WHERE ParentID=df.ID AND Status='Active') SubForumCount FROM " & _
                       Database & "..vwDiscussionForums df WHERE ID=" & Me.ControlRecordID.ToString & " AND " & _
                       ForumsControl.GetForumAccessControlWhereSQL(User1.UserID, Database, "df")

                dt = DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                If dt.Rows.Count > 0 Then

                    lblDiscussionForum.Text = CStr(dt.Rows(0).Item("Name"))
                    lblDescription.Text = CStr(dt.Rows(0).Item("Description"))

                    If Not IsDBNull(dt.Rows(0).Item("ParentID")) AndAlso _
                       CLng(dt.Rows(0).Item("ParentID")) > 0 Then
                        Me.ParentForumID = CLng(dt.Rows(0).Item("ParentID"))
                        lblParent.Text = CStr(dt.Rows(0).Item("Parent"))
                        lnkParent.HRef = Me.GetRedirectURLFromProperties(Me.ParentForumID)
                        'Issue 19890 Added by Sachin K 01/09/2014  
                        lnkParent.Attributes.Add("style", "cursor: text")
                        'lnkParent.HRef = Request.Path & "?ID=" & HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(CStr(dt.Rows(0).Item("ParentID"))))
                    Else
                        lblParent.Text = "Top Level"
                        lnkParent.HRef = ForumsHomePage
                    End If

                Else
                    lblDiscussionForum.Text = "Forum Not Found or Not Available"
                    lblDescription.Visible = False
                    lblParent.Visible = False
                    'trSubForums.Visible = False
                    Me.parForumLabel.Visible = False
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

    End Class
End Namespace
