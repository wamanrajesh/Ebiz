'Aptify e-Business 5.5.1, July 2013

Imports System.Data
Imports Telerik.Web.UI

Namespace Aptify.Framework.Web.eBusiness.Chapters
    Partial Class ChapterSearchControl
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "ChapterSearch"
        Protected Const ATTRIBUTE_DATATABLE_CHAPTER_SEARCH As String = "dtChapterSearch"

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
            If Not IsPostBack Then
                ' start out not showing the table results
                Dim oItem As ListItem
                oItem = New ListItem("<All Types>", "-1")
                trResults.Visible = False
                cmbCategory.Items.Add(oItem)
                'Amruta Issue 14448
                AddExpression()
                LoadChapterTypes()
            End If
        End Sub

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(Me.RedirectURL) Then
                Me.grdResults.Enabled = False
                Me.grdResults.ToolTip = "RedirectURL property has not been set."
            End If

            'if values are not provide directly or from the XML file, set default values for inherited query
            'parameter properties since control requires them to properly function
            If String.IsNullOrEmpty(Me.RedirectIDParameterName) Then Me.RedirectIDParameterName = "ID"

        End Sub

        Private Sub LoadChapterTypes()
            Dim sSQL As String
            Dim i As Integer
            Dim dt As DataTable
            Dim oItem As System.Web.UI.WebControls.ListItem

            Try
                sSQL = "SELECT DISTINCT ID,Name FROM " & Database & _
                       "..vwCompanyTypes "

                dt = DataAction.GetDataTable(sSQL)
                For i = 0 To dt.Rows.Count - 1
                    oItem = New ListItem(CStr(dt.Rows(i).Item("Name")), _
                                         CStr(dt.Rows(i).Item("ID")))
                    cmbCategory.Items.Add(oItem)
                Next
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
            AddExpression()
            ViewState(ATTRIBUTE_DATATABLE_CHAPTER_SEARCH) = Nothing
            LoadResults()
        End Sub

        Private Function GetSearchResults(ByVal Name As String, _
                                            ByVal CompanyTypeID As Long) As DataTable
            Dim sSQL As String
            Try
                Dim params(2) As IDataParameter
                params(0) = Me.DataAction.GetDataParameter("@PersonID", SqlDbType.Int, User1.PersonID)
                params(1) = Me.DataAction.GetDataParameter("@Name", SqlDbType.NVarChar, 100, "%" & Name & "%")
                params(2) = Me.DataAction.GetDataParameter("@CompanyTypeID", SqlDbType.Int, CompanyTypeID)

                sSQL = Database & "..spFindChapters"

                Return Me.DataAction.GetDataTableParametrized(sSQL, CommandType.StoredProcedure, params)
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return Nothing
            End Try
        End Function

        Protected Sub grdResults_PageIndexChanging(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageChangedEventArgs) Handles grdResults.PageIndexChanged
            grdResults.CurrentPageIndex = e.NewPageIndex
            If ViewState(ATTRIBUTE_DATATABLE_CHAPTER_SEARCH) IsNot Nothing Then
                grdResults.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_CHAPTER_SEARCH), DataTable)
            End If
        End Sub
        'Navin Prasad Issue 11032
        'Protected Sub grdResults_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdResults.ItemDataBound
        '    Try
        '        If Me.EncryptQueryStringValue Then
        '            Dim type As ListItemType = e.Item.ItemType
        '            If e.Item.ItemType = ListItemType.Item Or _
        '                    e.Item.ItemType = ListItemType.AlternatingItem Then
        '                Dim lnk As HyperLink
        '                Dim tempURL As String
        '                Dim index As Integer
        '                Dim sValue As String = "0"
        '                Dim separator As String()

        '                lnk = CType(e.Item.Cells(0).Controls(0), HyperLink)
        '                tempURL = lnk.NavigateUrl
        '                index = tempURL.IndexOf("=")
        '                sValue = tempURL.Substring(index + 1)
        '                separator = lnk.NavigateUrl.Split(CChar("="))
        '                lnk.NavigateUrl = separator(0)
        '                lnk.NavigateUrl = lnk.NavigateUrl & "="
        '                lnk.NavigateUrl = lnk.NavigateUrl & System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(sValue))
        '            End If
        '        End If
        '    Catch ex As Exception
        '        Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        '    End Try
        'End Sub

        'Protected Sub grdResults_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdResults.RowDataBound
        '    Try
        '        If Me.EncryptQueryStringValue Then
        '            Dim type As ListItemType = CType(e.Row.RowType, ListItemType)
        '            If CType(e.Row.RowType, ListItemType) = ListItemType.Item Or _
        '                    CType(e.Row.RowType, ListItemType) = ListItemType.AlternatingItem Then
        '                Dim lnk As HyperLink
        '                Dim tempURL As String
        '                Dim index As Integer
        '                Dim sValue As String = "0"
        '                Dim separator As String()

        '                lnk = CType(e.Row.Cells(0).Controls(0), HyperLink)
        '                tempURL = lnk.NavigateUrl
        '                index = tempURL.IndexOf("=")
        '                sValue = tempURL.Substring(index + 1)
        '                separator = lnk.NavigateUrl.Split(CChar("="))
        '                lnk.NavigateUrl = separator(0)
        '                lnk.NavigateUrl = lnk.NavigateUrl & "="
        '                lnk.NavigateUrl = lnk.NavigateUrl & System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(sValue))
        '            End If
        '        End If
        '    Catch ex As Exception
        '        Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        '    End Try
        'End Sub
        ''New Function created by Suvarna D for IssueID 12436
        ''To support paging separate grid bind fuction has been created
        Protected Sub LoadResults()
            ' search by name, category and 
            Dim sName As String
            Dim lCompanyType As Long
            Dim sSQL As String
            Dim dt As DataTable

            Try
                If ViewState(ATTRIBUTE_DATATABLE_CHAPTER_SEARCH) IsNot Nothing Then
                    grdResults.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_CHAPTER_SEARCH), DataTable)
                    grdResults.DataBind()
                    Exit Sub
                End If
                lCompanyType = CLng(cmbCategory.SelectedItem.Value)
                sName = Trim$(txtName.Text)
                If Len(sName) > 0 Or lCompanyType > 0 Then
                    dt = Me.GetSearchResults(sName, lCompanyType)
                    'Navin Prasad Issue 11032
                    Dim dcolUrl As DataColumn = New DataColumn()
                    dcolUrl.Caption = "DataNavigateUrl"
                    dcolUrl.ColumnName = "DataNavigateUrl"

                    dt.Columns.Add(dcolUrl)
                    If dt.Rows.Count > 0 Then
                        For Each rw As DataRow In dt.Rows
                            Dim tempURL As String = Me.RedirectURL & "?" & Me.RedirectIDParameterName & "=" & rw("ID")


                            Dim index As Integer = tempURL.IndexOf("=")
                            Dim sValue As String = tempURL.Substring(index + 1)
                            Dim separator As String() = tempURL.Split(CChar("="))
                            Dim navigate As String = separator(0)
                            navigate = navigate & "="
                            navigate = navigate & System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(sValue))
                            rw("DataNavigateUrl") = navigate
                        Next
                    End If


                    If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                        grdResults.DataSource = dt
                        grdResults.DataBind()
                        trResults.Visible = True
                        lblError.Visible = False
                        ViewState(ATTRIBUTE_DATATABLE_CHAPTER_SEARCH) = dt
                    Else
                        lblError.Text = "No records found."
                        lblError.Visible = True
                        trResults.Visible = False
                    End If

                Else
                    trResults.Visible = False
                    lblError.Text = "Please enter in at least one search element above"
                    lblError.Visible = True
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        ''End of Addition IssueID: 12436
        Protected Sub grdResults_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdResults.NeedDataSource
            If ViewState(ATTRIBUTE_DATATABLE_CHAPTER_SEARCH) IsNot Nothing Then
                grdResults.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_CHAPTER_SEARCH), DataTable)
            End If
        End Sub

        'Amruta Issue 14448 ,4/9/13 ,if the grid load first time By default the sorting will be Ascending for column Name 
        Private Sub AddExpression()
            Dim expression1 As New GridSortExpression
            expression1.FieldName = "Name"
            expression1.SetSortOrder("Ascending")
            grdResults.MasterTableView.SortExpressions.AddSortExpression(expression1)
        End Sub
    End Class
End Namespace
