'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.Application

Namespace Aptify.Framework.Web.eBusiness.ProductCatalog
    Partial Class RelatedProductsGrid
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced

        Protected Const ATTRIBUTE_GRID_NAVIGATION_PAGE As String = "GridNavigationPage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "RelatedProductsGrid"
        'Suvarna D IssueID-12745 to implement webImage feature to display images on Jan 19, 2012
        Protected Const ATTRIBUTE_IMAGE_NOT_AVAILABLE_URL As String = "ImageNotAvailable"
        'End of addition by Suvarna D IssueID-12745 to implement webImage feature to display images on Jan 19, 2012


#Region "RelatedProductsGrid Specific Properties"
        ''' <summary>
        ''' GridNavigation page url
        ''' </summary>
        Public Overridable Property GridNavigationPage() As String
            Get
                If Not ViewState(ATTRIBUTE_GRID_NAVIGATION_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_GRID_NAVIGATION_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_GRID_NAVIGATION_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        'Public Property NavigateURLFormatField() As String
        '    Get
        '        Dim o As Object
        '        o = ViewState.Item("NavigateURLFormatField")
        '        If o Is Nothing Then
        '            Return String.Empty
        '        Else
        '            Return CStr(o)
        '        End If
        '    End Get
        '    Set(ByVal Value As String)
        '        ViewState.Add("NavigateURLFormatField", Value)
        '    End Set
        'End Property

        ''' <summary>
        ''' 'Suvarna D IssueID-12745 to implement webImage feature to display images on Jan 19, 2012
        ''' </summary>
        Public Overridable Property ImageNotAvailable() As String
            Get
                If Not ViewState(ATTRIBUTE_IMAGE_NOT_AVAILABLE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_IMAGE_NOT_AVAILABLE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_IMAGE_NOT_AVAILABLE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        'End of addition by Suvarna D IssueID-12745 to implement webImage feature to display images on Jan 19, 2012

#End Region

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(GridNavigationPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                GridNavigationPage = Me.GetLinkValueFromXML(ATTRIBUTE_GRID_NAVIGATION_PAGE)
            End If

            'if values are not provide directly or from the XML file, set default values for inherited properties since 
            'control requires them to properly function
            If String.IsNullOrEmpty(Me.QueryStringRecordIDParameter) Then Me.QueryStringRecordIDParameter = "ID"

            'Suvarna D IssueID-12745 to implement webImage feature to display images on Jan 19, 2012
            If String.IsNullOrEmpty(ImageNotAvailable) Then
                'since value is the 'default' check the XML file for possible custom setting
                ImageNotAvailable = Me.GetLinkValueFromXML(ATTRIBUTE_IMAGE_NOT_AVAILABLE_URL)
            End If
            'End of addition by Suvarna D IssueID-12745 to implement webImage feature to display images on Jan 19, 2012

        End Sub

        Public Sub LoadGrid(ByVal ProductID As Long)
            Dim sSQL As String, sDB As String = AptifyApplication.GetEntityBaseDatabase("Products")
            Dim dt As System.Data.DataTable

            Try
                sSQL = "Execute " & sDB & "..spGetRelatedProductsWeb " & ProductID.ToString()
                dt = DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)

                If dt.Rows.Count > 0 Then
                    'Dim oCol As HyperLinkColumn
                    'oCol = CType(grdMain.Columns.Item(0), HyperLinkColumn)
                    'oCol.DataNavigateUrlFormatString = NavigateURLFormatField

                    'Suvarna Deshmukh IssueID-12433,12430 and 12434 On Dec 16,2011
                    'commented and added a Row_DataBound event to add navigation url to each link
                    'Navin Prasad Issue 11032
                    'With DirectCast(grdMain.Columns(0), HyperLinkField)
                    '    If Not String.IsNullOrEmpty(GridNavigationPage) Then
                    '        .DataNavigateUrlFormatString = GridNavigationPage & "?ID={0}"
                    '    Else
                    '        grdMain.Enabled = False
                    '        grdMain.ToolTip = "GridNavigationPage property not set."
                    '    End If
                    'End With
                    'End by Suvarna Deshmukh IssueID-12433,12430 and 12434 On Dec 16,2011

                    grdMain.DataSource = dt
                    grdMain.DataBind()
                Else
                    Me.Visible = False
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
            If Not IsPostBack Then
                If Me.SetControlRecordIDFromParam() Then
                    LoadGrid(Me.ControlRecordID)
                End If

            End If

        End Sub

        'Protected Sub grdMain_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdMain.RowDataBound

        '    Try
        '        Dim type As ListItemType = CType(e.Row.RowType, ListItemType)
        '        If CType(e.Row.RowType, ListItemType) = ListItemType.Item Or _
        '                CType(e.Row.RowType, ListItemType) = ListItemType.AlternatingItem Then

        '            With DirectCast(e.Row.FindControl("lnkProduct"), HyperLink)
        '                If Not String.IsNullOrEmpty(GridNavigationPage) Then
        '                    .NavigateUrl = String.Format(GridNavigationPage & "?ID={0}", DataBinder.Eval(e.Row.DataItem, "ProductID").ToString)
        '                Else
        '                    grdMain.Enabled = False
        '                    grdMain.ToolTip = "GridNavigationPage property not set."
        '                End If
        '            End With
        '        End If
        '    Catch ex As Exception

        '    End Try

        'End Sub

        Protected Sub grdMain_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles grdMain.ItemDataBound

            If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
                Dim itemID As Integer = e.Item.ItemIndex

                With DirectCast(e.Item.FindControl("lnkProduct"), HyperLink)
                    If Not String.IsNullOrEmpty(GridNavigationPage) Then
                        .NavigateUrl = String.Format(GridNavigationPage & "?ID={0}", DataBinder.Eval(e.Item.DataItem, "ProductID").ToString)
                    Else
                        grdMain.Enabled = False
                        grdMain.ToolTip = "GridNavigationPage property not set."
                    End If

                End With

                'Suvarna D IssueID-12745 to implement webImage feature to display images on Jan 19, 2012
                With DirectCast(e.Item.FindControl("ImgProd"), Image)
                    If Not IsDBNull(DataBinder.Eval(e.Item.DataItem, "ProdImageURL")) AndAlso _
                   Len(DataBinder.Eval(e.Item.DataItem, "ProdImageURL")) > 0 Then
                            .ImageUrl = DataBinder.Eval(e.Item.DataItem, "ProdImageURL").ToString
                    Else
                        .ImageUrl = ImageNotAvailable
                    End If
                End With

                ''Rashmip, Issue 13150
                Dim lbldescription As Label
                lbldescription = DirectCast(e.Item.FindControl("lblDescription"), Label)
                If Not IsDBNull(DataBinder.Eval(e.Item.DataItem, "PromptText")) AndAlso _
                  DataBinder.Eval(e.Item.DataItem, "PromptText").ToString <> "" Then
                    lbldescription.Visible = False
                Else
                    lbldescription.Visible = True
                End If

                'End of addition by Suvarna D IssueID-12745 to implement webImage feature to display images on Jan 19, 2012


            End If

        End Sub

        ' ''' <summary>
        ' ''' Nalini Issue 12436 date:1/12/2011
        ' ''' </summary>
        ' ''' <remarks></remarks>
        'Protected Sub grdMain_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdMain.PageIndexChanging
        '    grdMain.PageIndex = e.NewPageIndex
        '    LoadGrid(Me.ControlRecordID)
        'End Sub
    End Class
End Namespace
