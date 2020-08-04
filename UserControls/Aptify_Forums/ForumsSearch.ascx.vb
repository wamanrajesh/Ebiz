'Aptify e-Business 5.5.1, July 2013
Imports System.Data
Imports Telerik.Web.UI

Namespace Aptify.Framework.Web.eBusiness.Forums
    Partial Class SearchControl
        Inherits BaseUserControlAdvanced
        Protected Const ATTRIBUTE_FORUMS_HOME_PAGE As String = "ForumsHomePage"
        Protected Const ATTRIBUTE_GRID_FORUM_PAGE As String = "GridForumPage"
        Protected Const ATTRIBUTE_GRID_DISPLAY_MESSAGE_PAGE As String = "GridDisplayMessagePage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "ForumsSearch"
        Protected Const ATTRIBUTE_FORUMSEARCH_VIEWSTATE As String = "ForumSearchsdt"


#Region "ForumsSearch Specific Properties"
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
        ''' GridForum page url
        ''' </summary>
        Public Overridable Property GridForumPage() As String
            Get
                If Not ViewState(ATTRIBUTE_GRID_FORUM_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_GRID_FORUM_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_GRID_FORUM_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' GridDisplayMessage page url
        ''' </summary>
        Public Overridable Property GridDisplayMessagePage() As String
            Get
                If Not ViewState(ATTRIBUTE_GRID_DISPLAY_MESSAGE_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_GRID_DISPLAY_MESSAGE_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_GRID_DISPLAY_MESSAGE_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
#End Region

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(ForumsHomePage) Then
                'since value is the 'default' check the XML file for possible custom setting
                ForumsHomePage = Me.GetLinkValueFromXML(ATTRIBUTE_FORUMS_HOME_PAGE)
                If String.IsNullOrEmpty(ForumsHomePage) Then
                    Me.lnkForumsHome.Enabled = False
                    Me.lnkForumsHome.ToolTip = "ForumsHomePage property has not been set."
                    Me.lnkForumsHome.Font.Underline = True
                Else
                    Me.lnkForumsHome.NavigateUrl = ForumsHomePage
                End If
            Else
                Me.lnkForumsHome.NavigateUrl = ForumsHomePage
            End If
            If String.IsNullOrEmpty(GridForumPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                GridForumPage = Me.GetLinkValueFromXML(ATTRIBUTE_GRID_FORUM_PAGE)
                If String.IsNullOrEmpty(GridForumPage) Then
                    Me.grdResults.Columns.RemoveAt(0)
                    'Navin Prasad Issue 11032
                    grdResults.Columns.Insert(0, New GridBoundColumn())
                    With DirectCast(grdResults.Columns(0), GridBoundColumn)
                        .DataField = "Forum"
                        .HeaderText = "Forum"
                        .ItemStyle.ForeColor = Drawing.Color.Blue
                        .ItemStyle.Font.Underline = True
                    End With
                    Me.grdResults.ToolTip = "GridForumPage and/or GridDisplayMessagePage property has not been set."
                Else
                    'Navin Prasad Issue 11032
                    'DirectCast(grdResults.Columns(0), HyperLinkColumn).DataNavigateUrlFormatString = GridForumPage & "?ID={0}"
                End If
            End If
            If String.IsNullOrEmpty(GridDisplayMessagePage) Then
                'since value is the 'default' check the XML file for possible custom setting
                GridDisplayMessagePage = Me.GetLinkValueFromXML(ATTRIBUTE_GRID_DISPLAY_MESSAGE_PAGE)
                If String.IsNullOrEmpty(GridDisplayMessagePage) Then
                    Me.grdResults.Columns.RemoveAt(2)
                    'Navin Prasad Issue 11032
                    grdResults.Columns.Insert(2, New GridBoundColumn())
                    With DirectCast(grdResults.Columns(2), GridBoundColumn)
                        .DataField = "Subject"
                        .HeaderText = "Subject"
                        .ItemStyle.ForeColor = Drawing.Color.Blue
                        .ItemStyle.Font.Underline = True
                    End With
                    Me.grdResults.ToolTip = "GridForumPage and/or GridDisplayMessagePage property has not been set."
                Else
                    'Navin Prasad Issue 11032
                    ' DirectCast(grdResults.Columns(2), HyperLinkColumn).DataNavigateUrlFormatString = GridDisplayMessagePage & "?ID={0}"
                End If
            End If
        End Sub

        '12/04/2007 Tamasa,Code Changed for issue 5330.
        Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
            'Suraj issue 14455 3/5/13 Here we assign nothing to the view state because every selected index result will be a diffrent.
            ViewState(ATTRIBUTE_FORUMSEARCH_VIEWSTATE) = Nothing
            'Suraj issue 14455 3/5/13 ,this method use to apply the odrering of rad grid first column
            AddExpression()
            LoadgrdResults()
        End Sub
        'Suraj issue 14455 2/20/13 date time filtering
        Protected Sub grdResults_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdResults.ItemDataBound
            Dim dateColumns As New List(Of String)
            'Add datecolumn uniqueName in list for Date format
            dateColumns.Add("GridDateTimeColumnDateEntered")
            CommonMethods.FormatedDateOnGrid(dateColumns, e.Item)
        End Sub
        Protected Sub grdResults_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdResults.NeedDataSource
            If User1.UserID > 0 Then
                LoadgrdResults()
            End If
        End Sub


        ''' <summary>
        ''' Nalini 12436 date:1/12/2011
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub LoadgrdResults()
            Dim sSQL As String
            Dim iFilterCount As Integer = 0
            Dim dt As DataTable
            'Suraj issue 14455 2/20/13 , check the view state is nothing or not if the page load first time viewstate will be nothing but after bostback view state will conatin the datatable
            If ViewState(ATTRIBUTE_FORUMSEARCH_VIEWSTATE) Is Nothing Then
                sSQL = "SELECT dfm.ForumID, " & _
                        " dfm.Forum, " & _
                        " dfm.Subject, " & _
                        " CASE WHEN DataLength(dfm.Body) > 350 THEN SUBSTRING(dfm.Body,0,350) + ' ...' ELSE dfm.Body END As 'Body', " & _
                        " dfm.DateEntered, " & _
                        " dfm.ID " & _
                       "FROM " & Database & "..vwDiscussionForumMessages dfm " & _
                       "INNER JOIN " & Database & "..vwDiscussionForums df ON dfm.ForumID = df.ID " & _
                       "WHERE dfm.Status='Posted' " & _
                        " AND df.Status='Active'" & _
                        " AND (df.StartDate<=GETDATE() OR ISNULL(df.StartDate, '19000101') = '19000101') " & _
                        " AND (df.EndDate>=GETDATE() OR ISNULL(df.EndDate, '19000101') = '19000101') "

                Dim sTopicCodeId As String = ForumTree.GetSelectedDiscussionForumIds()

                If String.IsNullOrEmpty(sTopicCodeId) = False Then
                    sSQL &= " AND (df.Access IN ('Anonymous', 'Registered') " & _
                            " OR (df.Access='Restricted' " & _
                            " AND EXISTS (SELECT dfwg.ID FROM vwDiscussionForumWebGroups dfwg " & _
                            " INNER JOIN vwWebUserGroups wug ON dfwg.WebGroupID = wug.WebGroupID " & _
                            " WHERE dfwg.DiscussionForumID IN (" & sTopicCodeId & ") AND wug.WebUserID = " & Me.User1.UserID & ")))"
                Else
                    sSQL &= " AND df.Access IN ('Anonymous', 'Registered') "
                End If


                Dim params() As IDataParameter
                Dim paramSubject As IDataParameter
                Dim paramBody As IDataParameter

                If Len(txtTitle.Text) > 0 Then
                    paramSubject = Me.DataAction.GetDataParameter("@Subject", SqlDbType.NVarChar, 255, "%" & txtTitle.Text & "%")
                    sSQL = sSQL & " AND dfm.Subject LIKE @Subject "
                    iFilterCount += 1
                End If
                If txtBody.Text.Length > 0 Then
                    paramBody = Me.DataAction.GetDataParameter("@Body", SqlDbType.NVarChar, -1, "%" & txtBody.Text & "%")
                    sSQL = sSQL & " AND dfm.Body LIKE @Body "
                    iFilterCount += 1
                End If
                If Not chkAllForums.Checked AndAlso String.IsNullOrEmpty(sTopicCodeId) = False Then
                    sSQL = sSQL & " AND dfm.ForumID IN (" & sTopicCodeId & ") "
                    iFilterCount += 1
                End If

                If CLng(cmbRecency.SelectedItem.Value) > 0 Then
                    sSQL = sSQL & " AND DateEntered>=DateAdd(dd,-" & _
                           cmbRecency.SelectedItem.Value() & ",GETDATE()) "
                End If
                sSQL = sSQL & " ORDER BY Forum,DateEntered DESC"

                If iFilterCount > 0 Then
                    lblError.Visible = True

                    If paramSubject IsNot Nothing AndAlso paramBody IsNot Nothing Then
                        ReDim params(1)
                        params(0) = paramSubject
                        params(1) = paramBody
                    ElseIf paramSubject IsNot Nothing Then
                        ReDim params(0)
                        params(0) = paramSubject
                    ElseIf paramBody IsNot Nothing Then
                        ReDim params(0)
                        params(0) = paramBody
                    End If

                    dt = DataAction.GetDataTableParametrized(sSQL, CommandType.Text, params)

                    Dim dcolUrl As DataColumn = New DataColumn()
                    dcolUrl.Caption = "DataNavigateUrl"
                    dcolUrl.ColumnName = "DataNavigateUrl"

                    Dim dcolUrll As DataColumn = New DataColumn()
                    dcolUrll.Caption = "DataNavigateUrlSub"
                    dcolUrll.ColumnName = "DataNavigateUrlSub"

                    dt.Columns.Add(dcolUrl)
                    dt.Columns.Add(dcolUrll)
                    If dt.Rows.Count > 0 Then
                        For Each rw As DataRow In dt.Rows
                            Dim tempURL As String = GridForumPage & "?ID" & "=" & rw("ForumID")


                            Dim index As Integer = tempURL.IndexOf("=")
                            Dim sValue As String = tempURL.Substring(index + 1)
                            Dim separator As String() = tempURL.Split(CChar("="))
                            Dim navigate As String = separator(0)
                            navigate = navigate & "="
                            navigate = navigate & System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(sValue))
                            rw("DataNavigateUrl") = navigate
                        Next
                    End If
                    If dt.Rows.Count > 0 Then
                        For Each rw As DataRow In dt.Rows
                            Dim tempURL As String = GridDisplayMessagePage & "?ID" & "=" & rw("ID")


                            Dim index As Integer = tempURL.IndexOf("=")
                            Dim sValue As String = tempURL.Substring(index + 1)
                            Dim separator As String() = tempURL.Split(CChar("="))
                            Dim navigate As String = separator(0)
                            navigate = navigate & "="
                            navigate = navigate & System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(sValue))
                            rw("DataNavigateUrlSub") = navigate
                        Next
                    End If

                    If dt.Rows.Count > 0 Then
                        grdResults.DataSource = dt
                        grdResults.DataBind()
                        'Navin Prasad Issue 11032
                        'Suraj issue 14455 2/15/13 , if when page first time load here we store the datatable in to a viewstate
                        ViewState(ATTRIBUTE_FORUMSEARCH_VIEWSTATE) = dt


                        tblResults.Visible = True
                        grdResults.Visible = True
                        tblInner.Visible = False
                        lblError.Visible = False
                    Else
                        lblError.Text = "No Messages Found" ' - " & sSQL
                        lblError.Visible = True
                        tblResults.Visible = False
                    End If
                Else
                    lblError.Text = "Please enter at least one search criteria" ' - " & sSQL
                    lblError.Visible = True
                    tblResults.Visible = False
                End If
            Else
                'Suraj issue 14455 2/20/13 , after postback viewstate will assign for gridview
                grdResults.DataSource = CType(ViewState(ATTRIBUTE_FORUMSEARCH_VIEWSTATE), DataTable)
                '22/09/14 Sheetal Commented for issue 20049:Ebusiness:Search on Forum Does not work .
                'grdResults.DataBind()
                Dim tempFserchdt As DataTable = CType(ViewState(ATTRIBUTE_FORUMSEARCH_VIEWSTATE), DataTable)
                If tempFserchdt.Rows.Count > 0 Then
                    tblResults.Visible = True
                    grdResults.Visible = True
                    tblInner.Visible = False
                    lblError.Visible = False
                Else
                    lblError.Text = "No Messages Found" ' - " & sSQL
                    lblError.Visible = True
                    tblResults.Visible = False
                End If
               
            End If
        End Sub




        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
            If Not IsPostBack Then
                'Suraj issue 14455 3/5/13 ,this method use to apply the odrering of rad grid first column
                AddExpression()
                tblResults.Visible = False
                If Request.QueryString("ForumID") IsNot Nothing AndAlso Request.QueryString("Search") IsNot Nothing Then
                    Dim lForumID As Long
                    If IsNumeric(Request.QueryString("ForumID")) Then
                        lForumID = CLng(Request.QueryString("ForumID"))
                    Else
                        lForumID = CLng(Aptify.Framework.Web.Common.WebCryptography.Decrypt(CStr(Request.QueryString("ForumID"))))
                    End If
                    Me.chkAllForums.Checked = False
                    ForumTree.Visible = True
                    txtBody.Text = Request.QueryString("Search")
                    Me.cmbRecency.SelectedIndex = Me.cmbRecency.Items.Count - 1 ' set to the last item - all history
                    cmdSearch_Click(sender, e)
                End If

            End If
            'HP Issue#9063:  moving this block out of the non-postback check since on each postback this needs to be evaluated
            If chkAllForums.Checked Then
                ForumTree.Visible = False
                ForumTree.UncheckAllNodes()
            Else
                ForumTree.Visible = True
            End If
        End Sub

        Private Sub chkAllForums_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAllForums.CheckedChanged
            If chkAllForums.Checked Then
                'treeForums.Visible = False
                ForumTree.Visible = False
                ForumTree.UncheckAllNodes()
            Else
                'treeForums.Visible = True
                ForumTree.Visible = True
            End If
        End Sub

        Private Sub cmdChangeSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdChangeSearch.Click
            tblResults.Visible = False
            tblInner.Visible = True
        End Sub

     

        ''' <summary>
        ''' Nalini 12436 date:1/12/2011
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub grdResults_PageIndexChanging(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageChangedEventArgs) Handles grdResults.PageIndexChanged
            ''grdResults.PageIndex = e.NewPageIndex
            LoadgrdResults()
        End Sub
        Protected Sub grdResults_PageIndexChanging(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageSizeChangedEventArgs) Handles grdResults.PageSizeChanged
            ''grdResults.PageIndex = e.NewPageIndex
            LoadgrdResults()
        End Sub
        'Suraj Issue 14455 3/5/13 ,if the grid load first time By default the sorting will be Ascending for column Forum 
        Private Sub AddExpression()
            Dim expression1 As New GridSortExpression
            expression1.FieldName = "Forum"
            expression1.SetSortOrder("Ascending")
            grdResults.MasterTableView.SortExpressions.AddSortExpression(expression1)
        End Sub
    End Class
End Namespace
