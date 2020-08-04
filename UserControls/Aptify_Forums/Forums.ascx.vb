'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Namespace Aptify.Framework.Web.eBusiness.Forums
    Partial Class ForumsControl
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_FORUM_PAGE As String = "ForumPage"
        Protected Const ATTRIBUTE_FORUM_IMAGE_ROOT_URL As String = "ForumImageRootURL"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "Forums"

#Region "Forums Specific Properties"
        ''' <summary>
        ''' ForumImage url
        ''' </summary>
        Public Overridable Property ForumImage() As String
            Get
                If Not ViewState(ATTRIBUTE_FORUM_IMAGE_ROOT_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_FORUM_IMAGE_ROOT_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_FORUM_IMAGE_ROOT_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
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
        Public Property ParentID() As Long
            Get
                Dim o As Object
                o = ViewState.Item("ParentID")
                If o Is Nothing Then
                    Return -1
                Else
                    If CLng(o) <= 0 Then
                        Return -1
                    Else
                        Return CLng(o)
                    End If
                End If
            End Get
            Set(ByVal Value As Long)
                ViewState.Add("ParentID", Value)
            End Set
        End Property

        Public Overridable ReadOnly Property DataList() As DataList
            Get
                Return lstForums
            End Get
        End Property
#End Region

        Private m_bLoaded As Boolean = False

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
            'If Not IsPostBack Then
            If Me.SetControlRecordIDFromQueryString AndAlso _
                Me.SetControlRecordIDFromParam() Then
                ParentID = Me.ControlRecordID
            Else
                ParentID = -1
            End If
            'auto load
            LoadForums(False)
            'End If
        End Sub

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(ForumPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                ForumPage = Me.GetLinkValueFromXML(ATTRIBUTE_FORUM_PAGE)
            End If
            If String.IsNullOrEmpty(ForumImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                ForumImage = Me.GetLinkValueFromXML(ATTRIBUTE_FORUM_IMAGE_ROOT_URL)
            End If

            'if values are not provide directly or from the XML file, set default values for inherited properties since 
            'control requires them to properly function
            If String.IsNullOrEmpty(Me.QueryStringRecordIDParameter) Then Me.QueryStringRecordIDParameter = "ID"

        End Sub

        ''' <summary>
        ''' If AutoSwitchToSingleChildForum is set to True, if a parent has only one sub-forum, the child forum becomes the parent automatically.
        ''' </summary>
        ''' <param name="AutoSwitchToSingleChildForum"></param>
        ''' <remarks></remarks>
        Public Sub LoadForums(Optional ByVal AutoSwitchToSingleChildForum As Boolean = True, _
                              Optional ByVal ShowParentLabel As Boolean = True)
            Try

                If Me.ParentID > 1 Then
                    Me.trHeaderRow.Visible = True
                Else
                    Me.trHeaderRow.Visible = False
                End If

                m_bLoaded = True
                Dim dt As Data.DataTable

                dt = DataAction.GetDataTable(GetForumFilterSQL(User1.UserID, Database, ParentID), _
                                             DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                If dt.Rows.Count = 1 AndAlso AutoSwitchToSingleChildForum Then
                    ParentID = CLng(dt.Rows(0).Item("ID"))
                    LoadForums(AutoSwitchToSingleChildForum)
                Else
                    lstForums.Visible = False

                End If

                LoadForumTable(dt)

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub LoadForumTable(ByVal dt As Data.DataTable)

            If dt.Rows.Count > 0 Then

                Dim sBaseString As String
                Dim oTableRow As HtmlTableRow
                Dim oCell As HtmlTableCell
                sBaseString = "<a href=""" & ForumPage & "?ID=<<ID>>""><img align=""middle"" src=""" & ForumImage & "<<Image>>""> <font size=3><b><<Title>></b></font></a><br><<Description>>"
                Dim sLine As String

                With Me.tabForums

                    For Each oRow As Data.DataRow In dt.Rows
                        sLine = sBaseString
                        sLine = sLine.Replace("<<ID>>", System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(oRow("ID").ToString)))
                        sLine = sLine.Replace("<<Image>>", oRow("Type").ToString.Trim & ".gif")
                        sLine = sLine.Replace("<<Title>>", oRow("NameWCount").ToString)
                        sLine = sLine.Replace("<<Description>>", oRow("Description").ToString)

                        oTableRow = New HtmlTableRow
                        oCell = New HtmlTableCell
                        oCell.InnerHtml = sLine
                        oTableRow.Cells.Add(oCell)
                        .Rows.Add(oTableRow)
                    Next

                End With
            Else
                Me.Visible = False
                Me.tabForums.Visible = False
            End If

        End Sub

        Public Function GetDiscussionForumParentID(ByVal DiscussionForumID As Long) As Long
            Dim sSQL As String, lValue As Object

            Try
                sSQL = "SELECT ParentID FROM " & Database & "..vwDiscussionForums WHERE ID=" & DiscussionForumID
                lValue = DataAction.ExecuteScalar(sSQL)
                If Not lValue Is Nothing Then
                    Return CLng(lValue)
                Else
                    Return -1
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return -1
            End Try

        End Function

        ''' <summary>
        ''' Returns SQL statement that returns a list of forums that are available for the specified user.
        ''' </summary>
        ''' <param name="WebUserID"></param>
        ''' <param name="Database"></param>
        ''' <param name="ParentID">Optional parameter - if specified only sub-forums of the specified Forum ID are returned</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetForumFilterSQL(ByVal WebUserID As Long, ByVal Database As String, Optional ByVal ParentID As Long = -1) As String
            Dim sSQL As String
            sSQL = "SELECT df.ID,df.Name,df.Description,df.Type, " & _
                    "CASE df.Type WHEN 'Category' THEN df.Name ELSE " & _
                    "df.Name + ' (' + CONVERT(nvarchar(10),(SELECT COUNT(*) FROM " & Database & _
                    "..vwDiscussionForumMessages WHERE ForumID=df.ID)) + ')' END NameWCount FROM " & _
                    Database & "..vwDiscussionForums df WHERE " & _
                    " df.Status='Active'  AND df.Browsable=1 AND ISNULL(df.ParentID,-1)=" & ParentID & _
                    " AND " & GetForumAccessControlWhereSQL(WebUserID, Database, "df")
            sSQL &= " ORDER BY df.Type DESC, df.Name"
            Return sSQL
        End Function

        ''' <summary>
        ''' This method returns the WHERE clause component that restricts access to only those forums a user should have access to
        ''' </summary>
        ''' <param name="WebUserID"></param>
        ''' <param name="Database"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetForumAccessControlWhereSQL(ByVal WebUserID As Long, ByVal Database As String, Optional ByVal TablePrefix As String = "") As String
            Dim sSQL As String
            If TablePrefix <> "" Then
                TablePrefix &= "."
            End If
            sSQL = " (" & TablePrefix & "Access='Anonymous'"
            If WebUserID > 0 Then
                sSQL &= " OR " & TablePrefix & "Access='Registered' OR (" & TablePrefix & "Access='Restricted' AND " & TablePrefix & "ID IN (SELECT DiscussionForumID FROM " & _
                        Database & "..vwDiscussionForumWebGroups WHERE WebGroupID IN (SELECT WebGroupID FROM " & Database & _
                        "..vwWebUserGroups WHERE WebUserID=" & WebUserID & ") )) )"
            Else
                sSQL &= ")"
            End If
            Return sSQL
        End Function


    End Class
End Namespace
