'Aptify e-Business 5.5.1, July 2013
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Framework.Application
Imports Aptify.Framework.BusinessLogic
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.AttributeManagement
Imports System.Data
Imports Telerik.Web.UI

Namespace Aptify.Framework.Web.eBusiness
    Partial Class CompanyDirectoryGrid
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced

        Protected Const ATTRIBUTE_COMPANY_LISTING_PAGE As String = "CompanyListingPage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "CompanyDirectoryGrid"
        'Suraj Issue 14451 3/13/13 , ATTRIBUTE_CONTORL_ISCOMPANYSEARCH used for set the property "CheckAddExpressionApplicableForCompany"
        Protected ATTRIBUTE_CONTORL_ISCOMPANYSEARCH As Boolean = False


#Region "CompanyDirectoryGrid Specific Properties"
        ''' <summary>
        ''' CompanyListing page url
        ''' </summary>
        Public Overridable Property CompanyListingPage() As String
            Get
                If Not ViewState(ATTRIBUTE_COMPANY_LISTING_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_COMPANY_LISTING_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_COMPANY_LISTING_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
          'Suraj Issue 14451 3/13/13 , declare the prpoperty ,if the grid load first time By default the sorting will be Ascending or not
        Public Property CheckAddExpressionApplicableForCompany() As Boolean
            Get
                Return Me.ATTRIBUTE_CONTORL_ISCOMPANYSEARCH
            End Get
            Set(ByVal value As Boolean)
                Me.ATTRIBUTE_CONTORL_ISCOMPANYSEARCH = value
            End Set
        End Property

#End Region

#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub
        'NOTE: The following placeholder declaration is required by the Web Form Designer.
        'Do not delete or move it.
        Private designerPlaceholderDeclaration As System.Object

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()
        End Sub

#End Region

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(CompanyListingPage) Then
                CompanyListingPage = Me.GetLinkValueFromXML(ATTRIBUTE_COMPANY_LISTING_PAGE)
                If String.IsNullOrEmpty(CompanyListingPage) Then
                    Me.grdCompany.Columns.RemoveAt(0)
                    'Modified By   'Navin Prasad Issue 11032
                    grdCompany.Columns.Insert(0, New GridBoundColumn())
                    With DirectCast(grdCompany.Columns(0), GridBoundColumn)
                        .DataField = "Name"
                        .HeaderText = "Company"
                        .ItemStyle.ForeColor = Drawing.Color.Blue
                        .ItemStyle.Font.Underline = True
                    End With
                    Me.grdCompany.ToolTip = "CompanyListingPage property has not been set."
                End If
            End If
        End Sub

        Public Sub LoadDataTable(ByVal sSQL As String)
            ' save the SQL text of the table into viewstate
            ' so we can access it when the grid needs to be
            ' rebound
            ViewState("FirstParameter") = Aptify.Framework.Web.Common.WebCryptography.Encrypt(sSQL)
            ViewState("SecondParameter") = Aptify.Framework.Web.Common.WebCryptography.Encrypt("~SearchWithoutSearchText~")
            'Suraj Issue 14451 3/13/13 , if CheckAddExpressionApplicableOrNot property is true then call the AddExpression and the value for this set on search user control
            If CheckAddExpressionApplicableForCompany = True Then
                AddExpression()
            End If
            ' load the table starting with the first page
            LoadDataTable(0)
        End Sub

        Public Sub LoadDataTable(ByVal sSQL As String, ByVal sSearchText As String)
            ' save the SQL text of the table into viewstate
            ' so we can access it when the grid needs to be
            ' rebound
            ViewState("FirstParameter") = Aptify.Framework.Web.Common.WebCryptography.Encrypt(sSQL)
            ViewState("SecondParameter") = Aptify.Framework.Web.Common.WebCryptography.Encrypt(sSearchText)
            'Suraj Issue 14451 3/13/13 , if CheckAddExpressionApplicableOrNot property is true then call the AddExpression and the value for this set on search user control
            If CheckAddExpressionApplicableForCompany = True Then
                AddExpression()
            End If
            ' load the table starting with the first page
            LoadDataTable(0)
        End Sub

        Private Sub LoadDataTable(ByVal iPage As Integer)
            Dim sSQL As String
            Dim sSearchText As String
            Dim params(0) As IDataParameter
            Dim dt As DataTable

            'set control properties from XML file if needed
            SetProperties()

            Try
                If Not IsNothing(ViewState("FirstParameter")) Then
                    sSQL = Aptify.Framework.Web.Common.WebCryptography.Decrypt(ViewState("FirstParameter").ToString)
                    sSearchText = Aptify.Framework.Web.Common.WebCryptography.Decrypt(ViewState("SecondParameter").ToString)

                    If sSearchText = "~SearchWithoutSearchText~" Then
                        dt = Me.DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)
                    Else
                        params(0) = Me.DataAction.GetDataParameter("@SearchText", SqlDbType.NVarChar, 255, sSearchText)
                        dt = Me.DataAction.GetDataTableParametrized(sSQL, CommandType.Text, params)
                    End If

                    Dim dcolCompany As DataColumn = New DataColumn()
                    dcolCompany.Caption = "CompanyUrl"
                    dcolCompany.ColumnName = "CompanyUrl"
                    dt.Columns.Add(dcolCompany)

                    With grdCompany
                        If dt.Rows.Count > 0 Then
                            For Each rw As DataRow In dt.Rows
                                'Suraj Issue 14451,4/11/13,  Take a dynamic Id from Table
                                Dim CompanyUrl As String = Me.CompanyListingPage & "?ID=" & rw("ID") & ""
                                rw("CompanyUrl") = CompanyUrl
                            Next
                            'Navin Prasad Issue 11032
                            .CurrentPageIndex = iPage
                            .DataSource = dt
                            'Suraj Issue 14451,4/11/13,  This grid used on Browse by user control so we used this condition because we did not of DataBind method but it requires if this control used on anothere control
                            If CheckAddExpressionApplicableForCompany = True Then
                                .DataBind()
                            End If

                        End If
                        .Visible = (dt.Rows.Count > 0)
                        If dt.Rows.Count > 0 Then
                            lblNoResults.Visible = False
                        Else
                            lblNoResults.Visible = True
                        End If
                        'lblNoResults.Visible = (dt.Rows.Count = 0)
                    End With
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub grdCompany_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdCompany.NeedDataSource
            LoadDataTable(0)
        End Sub

        Protected Sub grdCompany_PageIndexChanging(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageSizeChangedEventArgs) Handles grdCompany.PageSizeChanged
            ''grdCompany.PageIndex = e.NewPageIndex
            LoadDataTable(grdCompany.CurrentPageIndex)
        End Sub

        Protected Sub grdCompany_RowDataBound(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles grdCompany.ItemDataBound
            Try
                Dim type As ListItemType = CType(e.Item.ItemType, ListItemType)
                If (TypeOf (e.Item) Is GridDataItem) Then
                    Dim lnk As HyperLink
                    Dim tempURL As String
                    Dim index As Integer
                    Dim sValue As String
                    Dim separator As String()

                    If Not String.IsNullOrEmpty(CompanyListingPage) Then
                        'Suraj Issue 14451 3/13/13 , Here remove  exeption due to "CType(e.Item.Cells(2).Controls(0)" this line  beacuse  HyperLink contain in position  controls(1)
                        lnk = CType(e.Item.Cells(2).Controls(1), HyperLink)
                        tempURL = lnk.NavigateUrl
                        index = tempURL.IndexOf("=")
                        sValue = tempURL.Substring(index + 1)
                        separator = lnk.NavigateUrl.Split(CChar("="))
                        lnk.NavigateUrl = separator(0)
                        lnk.NavigateUrl = lnk.NavigateUrl & "="
                        lnk.NavigateUrl = lnk.NavigateUrl & System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(sValue))
                    End If
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub grdCompany_PageIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageChangedEventArgs) Handles grdCompany.PageIndexChanged
            ' load the new appropriate page
            LoadDataTable(grdCompany.CurrentPageIndex)
        End Sub
      
        'Suraj Issue 14451 3/12/13 ,if the grid load first time By default the sorting will be Ascending for column Forum 
        Private Sub AddExpression()
            Dim expression1 As New GridSortExpression
            expression1.FieldName = "Name"
            expression1.SetSortOrder("Ascending")
            grdCompany.MasterTableView.SortExpressions.AddSortExpression(expression1)
        End Sub
    End Class
End Namespace
