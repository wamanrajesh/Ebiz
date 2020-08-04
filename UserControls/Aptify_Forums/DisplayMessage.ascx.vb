'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports System.Data
Imports Aptify.Framework.BusinessLogic.GenericEntity

Namespace Aptify.Framework.Web.eBusiness.Forums
    Partial Class DisplayMessageControl
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_FORUM_PAGE As String = "ForumPage"
        Protected Const ATTRIBUTE_FORUM_HOME_PAGE As String = "ForumHomePage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "DisplayMessage"

#Region "DisplayMessage Specific Properties"
        ''' <summary>
        ''' Forum page url
        ''' </summary>
        Public Overridable Property ForumPage() As String
            Get
                If Not ViewState(ATTRIBUTE_FORUM_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_FORUM_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_FORUM_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' ForumHome page url
        ''' </summary>
        Public Overridable Property ForumHomePage() As String
            Get
                If Not ViewState(ATTRIBUTE_FORUM_HOME_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_FORUM_HOME_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_FORUM_HOME_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
#End Region

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
            Try
                If Not IsPostBack Then
                    Dim dt As DataTable, sSQL As String
                    Dim lID As Long
                    '12/04/2007 Tamasa, Code Changed for issue 5330.
                    '24 Jan 2008 Hrushikesh Jog, For issue 5217
                    '16 July 2008 Pirisino Chris, for removing hardcoded query string reading
                    ' Changes made to get the query string name from a property set by CMS
                    ' Changes made by CP 7/14/2008
                    Dim sID As String = Request.QueryString(Me.QueryStringRecordIDParameter)

                    If Me.IsQueryStringEncrypted Then
                        sID = Aptify.Framework.Web.Common.WebCryptography.Decrypt(sID)
                    End If

                    If IsNumeric(sID) Then
                        lID = CLng(sID)
                    Else
                        Throw New FormatException
                    End If

                    sSQL = "SELECT ID,Name,Description FROM " & _
                           Database & "..vwDiscussionForums WHERE " & _
                           "ID=(SELECT ForumID FROM " & Database & _
                           "..vwDiscussionForumMessages WHERE ID=" & lID & ") AND " & _
                           ForumsControl.GetForumAccessControlWhereSQL(User1.UserID, Database)

                    dt = DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                    If dt.Rows.Count > 0 Then
                        lblDiscussionForum.Text = CStr(dt.Rows.Item(0).Item("Name"))
                        lblDescription.Text = CStr(dt.Rows.Item(0).Item("Description"))
                        If String.IsNullOrEmpty(ForumPage) Then
                            Me.lnkForum.Enabled = False
                            Me.lnkForum.ToolTip = "ForumPage property has not been set."
                        Else
                            lnkForum.NavigateUrl = ForumPage & "?ID=" & HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(CStr(dt.Rows.Item(0).Item("ID"))))
                        End If

                        Message.LoadMessage(lID)
                    Else
                        ' Msg Not Found or Not Available
                        Message.Visible = False
                        lblDiscussionForum.Text = "Forum/Message Not Found or Not Available"
                        If String.IsNullOrEmpty(ForumHomePage) Then
                            Me.lnkForum.Enabled = False
                            Me.lnkForum.ToolTip = "ForumHomePage property has not been set."
                        Else
                            lnkForum.NavigateUrl = ForumHomePage
                        End If
                    End If
                    End If
            Catch ex As FormatException
                ' changed code to use config settings instead of hardcoded paths
                'Response.Redirect(Me.GetSecurityErrorPageFromXML & "?Message=" & Server.UrlEncode("Message not Found"))
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(ForumHomePage) Then
                'since value is the 'default' check the XML file for possible custom setting
                ForumHomePage = Me.GetLinkValueFromXML(ATTRIBUTE_FORUM_HOME_PAGE)
            End If
            If String.IsNullOrEmpty(ForumPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                ForumPage = Me.GetLinkValueFromXML(ATTRIBUTE_FORUM_PAGE)
            End If

            'if values are not provide directly or from the XML file, set default values for inherited properties since 
            'control requires them to properly function
            If String.IsNullOrEmpty(Me.QueryStringRecordIDParameter) Then Me.QueryStringRecordIDParameter = "ID"
        End Sub

    End Class
End Namespace
