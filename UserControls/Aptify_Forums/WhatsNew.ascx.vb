'Aptify e-Business 5.5.1, July 2013

Imports System.Data
Imports Telerik.Web.UI

Namespace Aptify.Framework.Web.eBusiness.Forums
    ''' <summary>
    ''' This page displays the latest postings across forums in a summary format
    ''' </summary>
    ''' <remarks></remarks>
    Partial Class WhatsNewControl
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_FORUM_PAGE As String = "GridForumPage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "WhatsNew"
        Protected Const ATTRIBUTE_FORUMWHATSNEW_VIEWSTATE As String = "ForumWhatsNewdt"


#Region "WhatsNew Specific Properties"
        ''' <summary>
        ''' GridForum page url
        ''' </summary>
        Public Overridable Property GridForumPage() As String
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
#End Region

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(GridForumPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                GridForumPage = Me.GetLinkValueFromXML(ATTRIBUTE_FORUM_PAGE)
                If String.IsNullOrEmpty(GridForumPage) Then
                    Me.grdResults.Enabled = False
                    Me.grdResults.ToolTip = "GridForumPage property has not been set."
                End If
            End If
        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
            'set control properties from XML file if needed
            'Suraj issue 14455 3/5/13 ,this method use to apply the odrering of rad grid first column
            If Not IsPostBack Then
                AddExpression()
                'Suraj S Issue 16087 ,5/17/13, add the following method for binding the records if the page load first  time we are passing the false value for this methode
                'because we need to call databind methode for gid
                LoadGrid(False)
            End If
            SetProperties()
            If Me.User1.UserID > 0 Then
                lblDiscussionForum.Text = ""
                PnlPosting.Visible = True
            Else
                PnlPosting.Visible = False
                lblDiscussionForum.Text = "Forum Not Found or Not Available"
            End If
        End Sub
        'Suraj issue 14455 2/19/13 date time filtering
        Protected Sub grdResults_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdResults.ItemDataBound
            Dim dateColumns As New List(Of String)
            'Add datecolumn uniqueName in list for Date format
            dateColumns.Add("GridDateTimeColumnMostRecentPostingDate")
            CommonMethods.FormatedDateOnGrid(dateColumns, e.Item)
        End Sub

        Protected Sub grdResults_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdResults.NeedDataSource
            If Me.User1.UserID > 0 Then
                'Suraj S Issue 16087, 5/17/13, we are passint the true as a parameter for loadGrid ,because this event call for sorting and filtering 
                'so , at the time of filtering and sorting we doesn’t requrired the databind() methode for grid
                LoadGrid(True)
            Else
                PnlPosting.Visible = False
                lblDiscussionForum.Text = "Forum Not Found or Not Available"
            End If
        End Sub
        'Suraj S Issue 16087, 5/17/13, we add one parameter as boolean for soting and filtering
        Protected Sub LoadGrid(ByVal isSortingFiltering As Boolean)
            ' This function will load up the grid by running a query on
            ' the database based on the information selected by the user
            Dim sSQL As String
            Dim dt As DataTable
            Dim iDaysBack As Integer
            Try
                SetProperties()
                'Suraj issue 14455 2/20/13 , check the view state is nothing or not if the page load first time viewstate will be nothing but after bostback view state will conatin the datatable
                If ViewState(ATTRIBUTE_FORUMWHATSNEW_VIEWSTATE) Is Nothing Then
                    iDaysBack = CInt(cmbRecency.SelectedItem.Value)
                    sSQL = Database & _
                           "..spGetRecentForumPostingInfo @NumDaysBack=" & _
                           iDaysBack
                    dt = DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                    'Navin Prasad Issue 11032
                    ' DirectCast(grdResults.Columns(0), HyperLinkColumn).DataNavigateUrlFormatString = GridForumPage & "?ID={0}"
                    Dim dcolUrl As DataColumn = New DataColumn()
                    dcolUrl.Caption = "DataNavigateUrl"
                    dcolUrl.ColumnName = "DataNavigateUrl"

                    dt.Columns.Add(dcolUrl)
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
                    grdResults.DataSource = dt
                    'Suraj S Issue 16087, 5/17/13, if isSortingFiltering is false then we call grdResults.DataBind() methode 
                    If isSortingFiltering = False Then
                        grdResults.DataBind()
                    End If
                    'Suraj issue 14455 2/15/13 , if when page first time load here we store the datatable in to a viewstate
                    ViewState(ATTRIBUTE_FORUMWHATSNEW_VIEWSTATE) = dt
                    'Suraj S Issue 16087, 5/17/13, when the datatable count is zero bfore that we are adding one blank row for datatable but right now we remove that functinality
                Else
                    'Suraj issue 14455 2/20/13 , after postback viewstate will assign for gridview
                    Dim tempFWNewdt As DataTable = CType(ViewState(ATTRIBUTE_FORUMWHATSNEW_VIEWSTATE), DataTable)
                    grdResults.DataSource = tempFWNewdt
                    'Suraj S Issue 16087, 5/17/13, if isSortingFiltering is false then we call grdResults.DataBind() methode 
                    If isSortingFiltering = False Then
                        grdResults.DataBind()
                    End If

                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub cmdRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRefresh.Click
            'Suraj issue 14455 3/5/13 Here we assign nothing to the view state because every click of refresh button result will be a diffrent.
            ViewState(ATTRIBUTE_FORUMWHATSNEW_VIEWSTATE) = Nothing
            'Suraj S Issue 16087 ,5/17/13, remove the rebind methode and add the LoadGrid method for binding the records 
            LoadGrid(False)
        End Sub

        'Navin Prasad Issue 11032
        ' Changes made for to allow encrypting and decrypting the URL.
        ' Changes made by Hrushikesh Jog
        'Protected Sub grdResults_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles 	grdResults.ItemDataBound
        '    ' Changes made for to allow encrypting and decrypting the URL.
        '    ' Changes made by Hrushikesh Jog
        '    Try
        '        Dim type As ListItemType = e.Item.ItemType
        '        If e.Item.ItemType = ListItemType.Item Or _
        '        e.Item.ItemType = ListItemType.AlternatingItem Then
        '            Dim lnk As HyperLink
        '            Dim tempURL As String
        '            Dim index As Integer
        '            Dim sValue As String
        '            Dim separator As String()

        '            lnk = CType(e.Item.Cells(0).Controls(0), HyperLink)
        '            tempURL = lnk.NavigateUrl
        '            index = tempURL.IndexOf("=")
        '            sValue = tempURL.Substring(index + 1)
        '            separator = lnk.NavigateUrl.Split(CChar("="))
        '            lnk.NavigateUrl = separator(0)
        '            lnk.NavigateUrl = lnk.NavigateUrl & "="
        '            lnk.NavigateUrl = lnk.NavigateUrl & System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(sValue))


        '        End If
        '    Catch ex As Exception
        '        Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        '    End Try
        'End Sub
        'Navin Prasad Issue 11032

        'Protected Sub grdResults_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdResults.DataBound

        '    Dim dt As DataTable = CType(CType(sender, GridView).DataSource, DataTable)
        '    'Nalini
        '    Dim rowcounter As Integer = 0
        '    For Each row As GridViewRow In grdResults.Items
        '        Dim lnk As HyperLink = CType(row.FindControl("lnkForum"), HyperLink)
        '        lnk.NavigateUrl = String.Format(GridForumPage & "?ID={0}", dt.Rows((grdResults.PageIndex * grdResults.PageSize) + rowcounter)("ForumID").ToString)

        '        rowcounter = rowcounter + 1
        '    Next
        '    Try
        '        For Each row As GridViewRow In grdResults.Rows
        '            Dim type As ListItemType = CType(row.RowType, ListItemType)
        '            If CType(row.RowType, ListItemType) = ListItemType.Item Or _
        '           CType(row.RowType, ListItemType) = ListItemType.AlternatingItem Then
        '                Dim lnk As HyperLink
        '                Dim tempURL As String
        '                Dim index As Integer
        '                Dim sValue As String
        '                Dim separator As String()
        '                lnk = CType(row.FindControl("lnkForum"), HyperLink)
        '                tempURL = lnk.NavigateUrl
        '                index = tempURL.IndexOf("=")
        '                sValue = tempURL.Substring(index + 1)
        '                separator = lnk.NavigateUrl.Split(CChar("="))
        '                lnk.NavigateUrl = separator(0)
        '                lnk.NavigateUrl = lnk.NavigateUrl & "="
        '                lnk.NavigateUrl = lnk.NavigateUrl & System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(sValue))
        '            End If
        '        Next
        '    Catch ex As Exception
        '        Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        '    End Try
        'End Sub

        ''' <summary>
        ''' Nalini 12436 date:1/12/2011
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub grdResults_PageIndexChanging(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageChangedEventArgs) Handles grdResults.PageIndexChanged
            ''grdResults.PageIndex = e.NewPageIndex
            LoadGrid(False)
        End Sub
        Protected Sub grdResults_PageIndexChanging(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageSizeChangedEventArgs) Handles grdResults.PageSizeChanged
            ''grdResults.PageIndex = e.NewPageIndex
            LoadGrid(False)
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
