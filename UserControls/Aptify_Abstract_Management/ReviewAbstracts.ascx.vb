
'Aptify e-Business 5.5.1, July 2013
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Framework.Application
Imports Aptify.Framework.BusinessLogic
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.AttributeManagement
Imports System.Data
Imports Telerik.Web.UI

Namespace Aptify.Framework.Web.eBusiness.AbstractManagement

    Partial Class ReviewAbstracts
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "ReviewAbstracts"
        Protected Const ATTRIBUTE_DATATABLE_ABSTRACT As String = "dtAbstract"



#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()
            SetProperties()
        End Sub

#End Region

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
            Try
                If Not IsPostBack Then
                    LoadGrid()
                    AddExpression()
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try

        End Sub

        Private Sub AddExpression()
            Dim expression1 As New GridSortExpression
            expression1.FieldName = "Subject"
            expression1.SetSortOrder("Ascending")
            grdMain.MasterTableView.SortExpressions.AddSortExpression(expression1)
        End Sub


        Protected Sub grdMain_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdMain.ItemDataBound
            Dim dateColumns As New List(Of String)
            'Add datecolumn uniqueName in list for Date format
            dateColumns.Add("GridDateTimeColumnDateSubmitted")
            CommonMethods.FormatedDateOnGrid(dateColumns, e.Item)
            'Suraj S Issue Id 16565,5/22/13  ,we provide a tool tip for DatePopupButton as well as the GridDateTimeColumnStartDate textbox   
            If TypeOf e.Item Is GridFilteringItem Then
                Dim filterItem As GridFilteringItem = DirectCast(e.Item, GridFilteringItem)
                Dim gridDateTimeColumnStartDate As RadDatePicker = DirectCast(filterItem("GridDateTimeColumnDateSubmitted").Controls(0), RadDatePicker)
                gridDateTimeColumnStartDate.ToolTip = "Enter a filter date"
                gridDateTimeColumnStartDate.DatePopupButton.ToolTip = "Select a filter date"
            End If
        End Sub
        Protected Sub grdMain_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdMain.NeedDataSource
            If ViewState(ATTRIBUTE_DATATABLE_ABSTRACT) IsNot Nothing Then
                grdMain.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_ABSTRACT), DataTable)
            End If
        End Sub

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(Me.RedirectURL) Then
                Me.grdMain.Enabled = False
                Me.grdMain.ToolTip = "RedirectURL property has not been set."
            End If

            'if values are not provide directly or from the XML file, set default values for inherited properties since 
            'control requires them to properly function
            If String.IsNullOrEmpty(Me.RedirectIDParameterName) Then Me.RedirectIDParameterName = "ReportID"

        End Sub


        Private Sub LoadGrid()
            Dim sDatabase As String = AptifyApplication.GetEntityBaseDatabase("Abstracts")
            Dim sSQL As String = ""
            Dim sID As String
            Dim dt As DataTable

            Try
                SetProperties()
                'Suraj S Issue Id 16565,5/22/13  Remove the from here 'AddExpression()
                If ViewState(ATTRIBUTE_DATATABLE_ABSTRACT) IsNot Nothing Then
                    grdMain.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_ABSTRACT), DataTable)
                    grdMain.DataBind()
                Else
                    sID = Request.QueryString("ID")
                    If sID Is Nothing Then
                        sSQL = "SELECT ID,Subject,Title,SubmittedBy,DateSubmitted,Category FROM " & _
                           sDatabase & _
                           "..vwAbstracts ORDER BY DateSubmitted,SubmittedBy"
                    End If
                    dt = Me.DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)

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

                    'Navin Prasad Issue 11032
                    If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                        grdMain.DataSource = dt
                        grdMain.DataBind()
                        ViewState(ATTRIBUTE_DATATABLE_ABSTRACT) = dt
                    End If
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub grdMain_PageIndexChanging(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageChangedEventArgs) Handles grdMain.PageIndexChanged
            grdMain.CurrentPageIndex = e.NewPageIndex
            If ViewState(ATTRIBUTE_DATATABLE_ABSTRACT) IsNot Nothing Then
                grdMain.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_ABSTRACT), DataTable)
            End If
        End Sub
        Protected Sub grdMain_PageSizeChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageSizeChangedEventArgs) Handles grdMain.PageSizeChanged
            If ViewState(ATTRIBUTE_DATATABLE_ABSTRACT) IsNot Nothing Then
                grdMain.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_ABSTRACT), DataTable)
            End If
        End Sub

        'Navin Prasad Issue 11032
        'Protected Sub grdMain_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdMain.ItemDataBound

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
        'Protected Sub grdMain_DataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdMain.ItemDataBound
        'For Each rw As Telerik.Web.UI.GridDataItem In grdMain.Items
        '    Try
        '        Dim type As ListItemType = CType(rw.ItemType, ListItemType)
        '        If (TypeOf e.Item Is Telerik.Web.UI.GridDataItem) Then
        '            Dim lnk As HyperLink
        '            Dim tempURL As String
        '            Dim index As Integer
        '            Dim sValue As String
        '            Dim separator As String()
        '            lnk = CType(rw.Cells(0).Controls(0), HyperLink)
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
        'Next
        'End Sub
    End Class
End Namespace
