'Aptify e-Business 5.5.1, July 2013
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Framework.Application
Imports Aptify.Framework.BusinessLogic
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.AttributeManagement
Imports System.Data
Imports Telerik.Web.UI

Namespace Aptify.Framework.Web.eBusiness.Directories
    Partial Class PersonDirectoryGrid
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_PERSON_LISTING_PAGE As String = "PersonListingPage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "PersonDirectoryGrid"
        'Suraj Issue 14451 3/13/13 , ATTRIBUTE_CONTORL_ISPERSONSEARCH used for set the property "CheckAddExpressionApplicableForPerson"
        Protected ATTRIBUTE_CONTORL_ISPERSONSEARCH As Boolean = False

#Region "PersonDirectoryGrid Specific Properties"
        ''' <summary>
        ''' PersonListing page url
        ''' </summary>
        Public Overridable Property PersonListingPage() As String
            Get
                If Not ViewState(ATTRIBUTE_PERSON_LISTING_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_PERSON_LISTING_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_PERSON_LISTING_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        'Suraj Issue 14451 3/13/13 , declare the prpoperty ,if the grid load first time By default the sorting will be Ascending or not
        Public Property CheckAddExpressionApplicableForPerson() As Boolean
            Get
                Return Me.ATTRIBUTE_CONTORL_ISPERSONSEARCH
            End Get
            Set(ByVal value As Boolean)
                Me.ATTRIBUTE_CONTORL_ISPERSONSEARCH = value
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

            If String.IsNullOrEmpty(PersonListingPage) Then
                PersonListingPage = Me.GetLinkValueFromXML(ATTRIBUTE_PERSON_LISTING_PAGE)
                If String.IsNullOrEmpty(PersonListingPage) Then
                    Me.grdPerson.Columns.RemoveAt(0)
                    'Modified By   'Navin Prasad Issue 11032
                    grdPerson.Columns.Insert(0, New GridBoundColumn())
                    With DirectCast(grdPerson.Columns(0), GridBoundColumn)
                        .DataField = "FirstLast"
                        .HeaderText = "Member"
                        .ItemStyle.ForeColor = Drawing.Color.Blue
                        .ItemStyle.Font.Underline = True
                    End With
                    Me.grdPerson.ToolTip = "PersonListingPage property has not been set."
                End If
            End If


        End Sub
        Public Overloads Sub LoadDataTable(ByVal sSQL As String)
            ' save the SQL text of the table into viewstate
            ' so we can access it when the grid needs to be
            ' rebound
            ViewState("FirstParameter") = Aptify.Framework.Web.Common.WebCryptography.Encrypt(sSQL)
            ViewState("SecondParameter") = Aptify.Framework.Web.Common.WebCryptography.Encrypt("SearchWithoutSearchText")
            'Suraj Issue 14451 3/13/13 , if CheckAddExpressionApplicableOrNot property is true then call the AddExpression and the value for this set on search user control
            If CheckAddExpressionApplicableForPerson = True Then
                AddExpression()
            End If
            ' load the table starting with the first page
            LoadDataTable(0)
        End Sub

        Public Overloads Sub LoadDataTable(ByVal sSQL As String, ByVal sSearchText As String)
            ' save the SQL text of the table into viewstate
            ' so we can access it when the grid needs to be
            ' rebound
            ViewState("FirstParameter") = Aptify.Framework.Web.Common.WebCryptography.Encrypt(sSQL)
            ViewState("SecondParameter") = Aptify.Framework.Web.Common.WebCryptography.Encrypt(sSearchText)
            ' load the table starting with the first page
            'Suraj Issue 14451 3/13/13 , if CheckAddExpressionApplicableOrNot property is true then call the AddExpression and the value for this set on search user control
            If CheckAddExpressionApplicableForPerson = True Then
                AddExpression()
            End If
            LoadDataTable(0)

        End Sub

        Private Overloads Sub LoadDataTable(ByVal iPage As Integer)
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

                    If sSearchText = "SearchWithoutSearchText" Then
                        dt = Me.DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)
                    Else
                        params(0) = Me.DataAction.GetDataParameter("@SearchText", SqlDbType.NVarChar, 255, sSearchText)
                        dt = Me.DataAction.GetDataTableParametrized(sSQL, CommandType.Text, params)
                    End If

                    Dim dcolFirstLast As DataColumn = New DataColumn()
                    dcolFirstLast.Caption = "FirstLastUrl"
                    dcolFirstLast.ColumnName = "FirstLastUrl"
                    dt.Columns.Add(dcolFirstLast)


                    Dim dcolUrl As DataColumn = New DataColumn()
                    dcolUrl.Caption = "DataNavigateUrl"
                    dcolUrl.ColumnName = "DataNavigateUrl"
                    Dim rowcount As Integer = 0
                    dt.Columns.Add(dcolUrl)
                    If dt.Rows.Count > 0 Then
                        For Each rw As DataRow In dt.Rows
                            'Suraj Issue 14451,4/11/13,  Take a dynamic Id from Table
                            Dim FirstLastUrl As String = Me.PersonListingPage & "?ID=" & rw("ID") & ""
                            rw("FirstLastUrl") = FirstLastUrl
                            'For rowcount = 0 To dt.Rows.Count - 1
                            Dim tempURL As String = String.Format("mailto:{0}", dt.Rows(dt.Rows.IndexOf(rw))("Email1").ToString)
                            rw("DataNavigateUrl") = tempURL
                        Next
                    End If
                    With grdPerson
                        If dt.Rows.Count > 0 Then
                            'Navin Prasad Issue 11032
                            .CurrentPageIndex = iPage
                            .DataSource = dt
                            'Suraj Issue 14451,4/11/13,  This grid used on Browse by user control so we used this condition because we did not of DataBind method but it requires if this control used on anothere control
                            If CheckAddExpressionApplicableForPerson = True Then
                                .DataBind()
                            End If

                        End If
                        .Visible = (dt.Rows.Count > 0)
                        lblNoResults.Visible = (dt.Rows.Count = 0)
                    End With
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try

        End Sub

        Protected Sub grdPerson_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdPerson.NeedDataSource
            LoadDataTable(0)
        End Sub

        Protected Sub grdPerson_PageIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageSizeChangedEventArgs) Handles grdPerson.PageSizeChanged
            ' load the new appropriate page
            LoadDataTable(grdPerson.CurrentPageIndex)
        End Sub

        Protected Sub grdPerson_PageIndexChanging(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageChangedEventArgs) Handles grdPerson.PageIndexChanged
            'grdPerson.PageIndex = e.NewPageIndex
            LoadDataTable(grdPerson.CurrentPageIndex)
        End Sub

        Protected Sub grdPerson_RowDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdPerson.ItemDataBound
            Try
                Dim type As ListItemType = CType(e.Item.ItemType, ListItemType)
                If (TypeOf (e.Item) Is GridDataItem) Then
                    Dim lnk As HyperLink
                    Dim tempURL As String
                    Dim index As Integer
                    Dim sValue As String
                    Dim separator As String()
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
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

       
        'Suraj Issue 14451 3/12/13 ,if the grid load first time By default the sorting will be Ascending for column  
        Private Sub AddExpression()
            Dim expression1 As New GridSortExpression
            expression1.FieldName = "FirstLast"
            expression1.SetSortOrder("Ascending")
            grdPerson.MasterTableView.SortExpressions.AddSortExpression(expression1)
        End Sub
   
    
    End Class
End Namespace
