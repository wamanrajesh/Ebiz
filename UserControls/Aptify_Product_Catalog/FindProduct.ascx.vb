'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports System.Data
Imports Telerik.Web.UI

Namespace Aptify.Framework.Web.eBusiness.ProductCatalog
    Partial Class FindProductControl
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_VIEW_PRODUCT_PAGE As String = "ViewProductPage"
        Protected Const ATTRIBUTE_VIEW_PRODUCT_CATEGORY_PAGE As String = "ViewProductCatagoryPage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "FindProduct"
        Protected Const ATTRIBUTE_SHOWMEETINGSLINKTOCLASS_DEFAULT_NAME As String = "ShowMeetingsLinkToClass"
        Protected Const ATTRIBUTE_DATATABLE_PRODUCTS As String = "dtProducts"

#Region "FindProduct Specific Properties"
        ''' <summary>
        ''' ViewProduct page url
        ''' </summary>
        Public Overridable Property ViewProductPage() As String
            Get
                If Not ViewState(ATTRIBUTE_VIEW_PRODUCT_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_VIEW_PRODUCT_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_VIEW_PRODUCT_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' ViewProductCatagory page url
        ''' </summary>
        Public Overridable Property ViewProductCatagoryPage() As String
            Get
                If Not ViewState(ATTRIBUTE_VIEW_PRODUCT_CATEGORY_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_VIEW_PRODUCT_CATEGORY_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_VIEW_PRODUCT_CATEGORY_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        'Nalini issue 11290
        Protected Overridable ReadOnly Property ShowMeetingsLinkToClass() As Boolean
            Get
                If Not ViewState.Item(ATTRIBUTE_SHOWMEETINGSLINKTOCLASS_DEFAULT_NAME) Is Nothing Then
                    Return CBool(ViewState.Item(ATTRIBUTE_SHOWMEETINGSLINKTOCLASS_DEFAULT_NAME))
                Else
                    Dim value As String = Me.GetGlobalAttributeValue(ATTRIBUTE_SHOWMEETINGSLINKTOCLASS_DEFAULT_NAME)
                    If Not String.IsNullOrEmpty(value) Then
                        Select Case Ucase(value)
                            Case "TRUE", "FALSE", "0", "1"
                                ViewState.Item(ATTRIBUTE_SHOWMEETINGSLINKTOCLASS_DEFAULT_NAME) = CBool(value)
                            Case Else
                                ViewState.Item(ATTRIBUTE_SHOWMEETINGSLINKTOCLASS_DEFAULT_NAME) = False
                        End Select
                    Else
                        ViewState.Item(ATTRIBUTE_SHOWMEETINGSLINKTOCLASS_DEFAULT_NAME) = False
                    End If
                End If
            End Get
        End Property
#End Region

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(ViewProductPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                ViewProductPage = Me.GetLinkValueFromXML(ATTRIBUTE_VIEW_PRODUCT_PAGE)
                If String.IsNullOrEmpty(ViewProductPage) Then
                    'Navin Prasad Issue 11032

                    'grdResults.Columns.RemoveAt(0)
                    'grdResults.Columns.Insert(0, New BoundField())
                    'With DirectCast(grdResults.Controls(0), BoundColumn)
                    '    .DataField = "WebName"
                    '    .HeaderText = "Product"
                    '    .ItemStyle.ForeColor = Drawing.Color.Blue
                    '    .ItemStyle.Font.Underline = True
                    'End With
                    grdResults.ToolTip = "ProductViewPage property has not been set."
                Else
                    '     DirectCast(grdResults.Columns(0), HyperLinkColumn).DataNavigateUrlFormatString = ViewProductPage & "?ID={0}"
                End If
            Else
                '  DirectCast(grdResults.Columns(0), HyperLinkColumn).DataNavigateUrlFormatString = ViewProductPage & "?ID={0}"
            End If
            If String.IsNullOrEmpty(ViewProductCatagoryPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                ViewProductCatagoryPage = Me.GetLinkValueFromXML(ATTRIBUTE_VIEW_PRODUCT_CATEGORY_PAGE)
                If String.IsNullOrEmpty(ViewProductCatagoryPage) Then
                    'Navin Prasad Issue 11032
                    'grdResults.Columns.RemoveAt(2)
                    'grdResults.Columns.Insert(2, New BoundField())
                    'With DirectCast(grdResults.Columns(2), DataGridColumn)
                    '    .DataField = "Category"
                    '    .HeaderText = "Category"
                    '    .ItemStyle.ForeColor = Drawing.Color.Blue
                    '    .ItemStyle.Font.Underline = True
                    'End With
                    grdResults.ToolTip = "ViewProductCatagoryPage property has not been set."
                Else
                    ' DirectCast(grdResults.Columns(2), HyperLinkColumn).DataNavigateUrlFormatString = ViewProductCatagoryPage & "?ID={0}"
                End If
            Else
                ' DirectCast(grdResults.Columns(2), HyperLinkColumn).DataNavigateUrlFormatString = ViewProductCatagoryPage & "?ID={0}"
            End If
        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
            Try
                If Not IsPostBack Then
                    ' start out not showing the table results
                    Dim oItem As ListItem
                    oItem = New ListItem("<All Categories>", "-1")
                    trResults.Visible = False
                    trNoResults.Visible = False                             ' RFB - 6/30/2003
                    cmbCategory.Items.Add(oItem)
                    LoadCategories(-1, "")
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub LoadCategories(ByVal lParentID As Long, ByVal sSpaces As String)
            Dim sSQL As String
            Dim i As Integer
            Dim dt As DataTable
            Dim oItem As System.Web.UI.WebControls.ListItem

            Try
                sSQL = "SELECT ID,WebName FROM " & _
                       Database & "..vwProductCategories " & _
                       "WHERE WebEnabled=1 AND "

                If lParentID > 0 Then
                    sSQL &= "ParentID = " & lParentID
                Else
                    sSQL &= "ParentID IS NULL"
                End If

                sSQL &= "  ORDER BY WebName "

                dt = DataAction.GetDataTable(sSQL)

                For i = 0 To dt.Rows.Count - 1
                    oItem = New ListItem(sSpaces & CStr(dt.Rows(i).Item("WebName")), CStr(dt.Rows(i).Item("ID")))
                    cmbCategory.Items.Add(oItem)
                    ' recursive call
                    LoadCategories(CLng(oItem.Value), sSpaces & "---")
                Next
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        'Neha, issue 14456, 03/15/13, added method for assending order sorting on first time gridload(for first column)
        Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
            AddExpression()
            ViewState(ATTRIBUTE_DATATABLE_PRODUCTS) = Nothing
            ShowResultTable()
        End Sub

        Protected Sub grdResults_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdResults.NeedDataSource
            If ViewState(ATTRIBUTE_DATATABLE_PRODUCTS) IsNot Nothing Then
                grdResults.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_PRODUCTS), DataTable)
            End If
        End Sub

        Private Sub ShowResultTable()
            ' search by name, category and 
            Dim sName As String
            Dim sDescription As String
            Dim lCategoryID As Long
            Dim sSQL As String
            Dim dt As DataTable
            'HP Issue#6594: parameterize input for defense against SQL injections
            Dim pName As Data.IDataParameter = Nothing
            Dim pDescription As Data.IDataParameter = Nothing
            Dim colParams(1) As Data.IDataParameter

            Try
                If ViewState(ATTRIBUTE_DATATABLE_PRODUCTS) IsNot Nothing Then
                    grdResults.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_PRODUCTS), DataTable)
                    grdResults.DataBind()
                    Exit Sub
                End If
                lCategoryID = CLng(cmbCategory.SelectedItem.Value)
                sName = Trim$(txtName.Text)
                sDescription = Trim$(txtDescription.Text)
                If Len(sName) > 0 Or Len(sDescription) > 0 Or lCategoryID > 0 Then
                    'HP Issue#6594: parameterize input for defense against SQL injections
                    pName = DataAction.GetDataParameter("@Name", SqlDbType.NVarChar, txtName.Text)
                    pDescription = DataAction.GetDataParameter("@Description", SqlDbType.NVarChar, txtDescription.Text)
                    colParams(0) = pName
                    colParams(1) = pDescription
                    sSQL = GetSearchSQL(sName, sDescription, lCategoryID)
                    'dt = DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                    dt = DataAction.GetDataTableParametrized(sSQL, CommandType.Text, colParams)

                    Dim dcolViewProductPageUrl As DataColumn = New DataColumn()
                    dcolViewProductPageUrl.Caption = "ViewProductPageUrl"
                    dcolViewProductPageUrl.ColumnName = "ViewProductPageUrl"
                    dt.Columns.Add(dcolViewProductPageUrl)
                    Dim dcolViewProductCatagoryPageUrl As DataColumn = New DataColumn()
                    dcolViewProductCatagoryPageUrl.Caption = "ViewProductCatagoryPageUrl"
                    dcolViewProductCatagoryPageUrl.ColumnName = "ViewProductCatagoryPageUrl"
                    dt.Columns.Add(dcolViewProductCatagoryPageUrl)

                    If dt.Rows.Count > 0 Then


                        For Each rw As DataRow In dt.Rows
                            rw("ViewProductPageUrl") = ViewProductPage + "?ID=" + rw("ID").ToString
                            rw("ViewProductCatagoryPageUrl") = ViewProductCatagoryPage + "?ID=" + rw("CategoryID").ToString
                        Next


                    End If

                    Dim rowcounter As Integer = 0
                    If dt.Rows.Count > 0 Then
                        grdResults.DataSource = dt
                        grdResults.DataBind()
                        ViewState(ATTRIBUTE_DATATABLE_PRODUCTS) = dt
                        trResults.Visible = True
                        trNoResults.Visible = False             ' RFB - 6/30/2003
                    Else
                        trResults.Visible = False
                        trNoResults.Visible = True              ' RFB - 6/30/2003
                    End If
                    lblError.Visible = False
                Else
                    trResults.Visible = False
                    lblError.Text = "Please enter in at least one search element above"
                    lblError.Visible = True
                    trNoResults.Visible = False
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Function GetSearchSQL(ByVal Name As String, _
                                      ByVal Description As String, _
                                      ByVal CategoryID As Long) As String
            Dim sSQL As String
            Dim iCount As Integer
            'Neha Changes Issue 11172
            Try
                sSQL = "SELECT p.ID,p.WebName,p.WebDescription,p.CategoryID,pc.WebName Category " & _
                      "FROM " & Database & "..vwProducts p INNER JOIN " & _
                      Database & "..vwProductCategories pc ON p.CategoryID=pc.ID " & _
                      "WHERE p.WebEnabled=1 AND p.IsSold=1 AND p.TopLevelItem=1 "
                ' Nalini issue 11290
                If Not ShowMeetingsLinkToClass Then
                    sSQL = sSQL & (" AND  ISNULL(p.ClassID ,-1) <=0  ")
                End If
                If Len(Name) > 0 Then
                    'HP Issue#6594: parameterize input for defense against SQL injections
                    'sSQL = sSQL & "AND p.WebName like '%" & Name & "%' "
                    sSQL = sSQL & "AND p.WebName like '%' + @Name + '%' "
                    iCount = iCount + 1
                End If
                If Len(Description) > 0 Then
                    'If iCount > 0 Then
                    '   sSQL = sSQL & " AND "
                    'End If
                    'HP Issue#6594: parameterize input for defense against SQL injections
                    'sSQL = sSQL & "AND p.WebDescription like '%" & Description & "%' "
                    sSQL = sSQL & "AND p.WebDescription like '%' + @Description + '%' "
                    iCount = iCount + 1
                End If
                If CategoryID > 0 Then
                    'If iCount > 0 Then
                    '   sSQL = sSQL & " AND "
                    'End If
                    sSQL = sSQL & "AND p.CategoryID=" & CategoryID
                    iCount = iCount + 1
                End If
                sSQL = sSQL & " ORDER BY pc.WebName,p.WebName"
                Return sSQL
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return Nothing
            End Try
        End Function
        ''' <summary>
        ''' Nalini 12436 date:1/12/2011
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub grdResults_PageIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageChangedEventArgs) Handles grdResults.PageIndexChanged
            'grdResults.PageIndex = e.NewPageIndex
            ShowResultTable()
        End Sub
        Protected Sub grdResults_PageIndexChanging(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageSizeChangedEventArgs) Handles grdResults.PageSizeChanged
            'grdResults.PageIndex = e.NewPageIndex
            ShowResultTable()
        End Sub
        'neha,issue 14456,03/15/13,method for sorting assending order on first time gridload(for first column)
        Private Sub AddExpression()
            Dim expression1 As New GridSortExpression
            expression1.FieldName = "WebName"
            expression1.SetSortOrder("Ascending")
            grdResults.MasterTableView.SortExpressions.AddSortExpression(expression1)
        End Sub
    End Class
End Namespace
