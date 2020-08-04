'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports Aptify.Framework.Application
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.BusinessLogic.Security
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports System.Data
Imports Telerik.Web.UI
Imports System.Collections.Generic


Namespace Aptify.Framework.Web.eBusiness.CustomerService
    Partial Class RenewalsControl
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CANCEL_BUTTON_REDIRECT As String = "CancelButtonRedirect"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "Renewals"
        Protected Const ATTRIBUTE_SELECTED_ROWS As String = "SelectedRows"
        'Suraj Issue 14450 3/23/13 ,declare the property
        Protected Const ATTRIBUTE_CHECKED_SUBSCRIPTION = "CheckedSubscriptionGrid"
        Protected Const ATTRIBUTE_DATATABLE_SUBSCRIPTIONS As String = "dtSubscriptions"

#Region "Renewals Specific Properties"
        ''' <summary>
        ''' CancelRedirect page url
        ''' </summary>
        Public Overridable Property CancelRedirectURL() As String
            Get
                If Not ViewState(ATTRIBUTE_CANCEL_BUTTON_REDIRECT) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_CANCEL_BUTTON_REDIRECT))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_CANCEL_BUTTON_REDIRECT) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
#End Region


        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
            If Not IsPostBack Then
                LoadGrid()
                'Suraj Issue 14450 3/22/13 ,this method use to apply the odrering of rad grid first column
                AddExpressionRenewal()
            End If
        End Sub

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(Me.RedirectURL) Then
                Me.RenewButton.Enabled = False
                Me.RenewButton.ToolTip = "RedirectURL property has not been set."
            End If
            If String.IsNullOrEmpty(CancelRedirectURL) Then
                CancelRedirectURL = Me.GetLinkValueFromXML(ATTRIBUTE_CANCEL_BUTTON_REDIRECT)
                If String.IsNullOrEmpty(CancelRedirectURL) Then
                    Me.CancelButton.Enabled = False
                    Me.CancelButton.ToolTip = "CancelRedirectURL property has not been set."
                End If
            End If
        End Sub

        Private Sub LoadGrid()
            Dim SQL As String
            Dim params(0) As IDataParameter
            Dim DT As Data.DataTable

            lblAdded.Text = ""
            lblAdded.ForeColor = Nothing
            lblAdded.Visible = False

            Try
                If ViewState(ATTRIBUTE_DATATABLE_SUBSCRIPTIONS) IsNot Nothing Then
                    grdMain.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_SUBSCRIPTIONS), DataTable)
                    grdMain.DataBind()
                    Exit Sub
                End If
                SQL = Database & _
                      "..spGetSubscriptionsByPersonID"

                params(0) = Me.DataAction.GetDataParameter("@PersonID", SqlDbType.BigInt, User1.PersonID)
                DT = Me.DataAction.GetDataTableParametrized(SQL, CommandType.StoredProcedure, params)

                If DT IsNot Nothing AndAlso DT.Rows.Count > 0 AndAlso Not String.IsNullOrEmpty(Me.RedirectURL) Then
                    RenewButton.Enabled = True
                Else
                    RenewButton.Enabled = False
                End If

                grdMain.DataSource = DT
                grdMain.DataBind()
                ViewState(ATTRIBUTE_DATATABLE_SUBSCRIPTIONS) = DT

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub RenewButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RenewButton.Click
            Try
                Dim dgItem As DataGridItem
                'Navin Prasad Issue 11032
                Dim gvRow As GridDataItem
                Dim chkSelected As CheckBox
                Dim strProductID As String
                Dim strProductName As String
                Dim strPurchaseType As String
                Dim oOrder As AptifyGenericEntityBase
                Dim count As Integer
                Dim dtSelectedRecords As DataTable = Nothing

                Dim bSubscriptionSelected As Boolean = False
                'Suraj Issue 14450 3/23/13 ,maintain the checkedstate of values 
                SaveCheckedValues()
                If ViewState(ATTRIBUTE_SELECTED_ROWS) IsNot Nothing Then
                    dtSelectedRecords = CType(ViewState(ATTRIBUTE_SELECTED_ROWS), DataTable)
                End If

                If dtSelectedRecords IsNot Nothing AndAlso dtSelectedRecords.Rows.Count > 0 Then
                    bSubscriptionSelected = RowSelected(dtSelectedRecords)
                End If
                ' Modified By   'Navin Prasad Issue 11032
                'Check to see that at least one 
                'For Each gvRow In grdMain.Items
                If dtSelectedRecords IsNot Nothing AndAlso dtSelectedRecords.Rows.Count > 0 Then
                    bSubscriptionSelected = True
                End If
                If Not bSubscriptionSelected Then
                    'Nothing selected - Nothing to do.
                    lblAdded.Text = "Please select one or more items to renew and click " & RenewButton.Text & "."
                    lblAdded.ForeColor = System.Drawing.Color.Crimson
                    lblAdded.Visible = True
                Else
                    lblAdded.Text = ""
                    lblAdded.ForeColor = Nothing
                    lblAdded.Visible = False

                    oOrder = ShoppingCart1.GetOrderObject(Page.Session, Page.User, Page.Application)
                    If dtSelectedRecords IsNot Nothing AndAlso dtSelectedRecords.Rows.Count > 0 Then
                        For i As Integer = 0 To dtSelectedRecords.Rows.Count - 1
                            strProductID = CStr(dtSelectedRecords.Rows(i).Item("ProductID"))
                            strProductName = CStr(dtSelectedRecords.Rows(i).Item("ProductName"))
                            strPurchaseType = CStr(dtSelectedRecords.Rows(i).Item("PurchaseType"))
                            lblSelections.Text += "ID: <b>" & strProductID & "</b><br>"
                            ShoppingCart1.AddToCart(CLng(strProductID), False, , , Me.Session)
                            oOrder.SubTypes("OrderLines").Item(count).SetValue("Description", "Renewal: " + strProductName)
                            oOrder.SubTypes("OrderLines").Item(count).SetValue("PurchaseType", strPurchaseType)
                            lblAdded.Text = "Product Added to Cart"
                            lblAdded.Visible = True
                            count = count + 1
                        Next
                    End If
                    ShoppingCart1.SaveCart(Page.Session)
                    Response.Redirect(Me.RedirectURL)
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub CancelButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelButton.Click
            Response.Redirect(Me.CancelRedirectURL)
        End Sub

        Protected Sub grdMain_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdMain.ItemCreated

            Dim chkSubscriber As CheckBox = DirectCast(e.Item.FindControl("chkRenewal"), CheckBox)
            Dim dataItem As DataRowView
            Dim SubscriptionID As Long, i As Integer = 0


            Dim dtSelectedRecords As DataTable = Nothing
            Dim lstExistingAttendee As ArrayList = Nothing
            If ViewState(ATTRIBUTE_SELECTED_ROWS) IsNot Nothing Then
                dtSelectedRecords = CType(ViewState(ATTRIBUTE_SELECTED_ROWS), DataTable)
            End If

            If chkSubscriber IsNot Nothing Then

                dataItem = DirectCast(e.Item.DataItem, System.Data.DataRowView)

                If dtSelectedRecords IsNot Nothing Then
                    If dataItem IsNot Nothing Then
                        SubscriptionID = CLng(dataItem("ID"))
                        If dtSelectedRecords.Rows.Contains(SubscriptionID) Then
                            chkSubscriber.Checked = True
                        Else
                            chkSubscriber.Checked = False
                        End If
                    End If
                End If

            End If
        End Sub
        'Suraj issue 14450 2/12/13 date time filtering
        Protected Sub grdMain_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdMain.ItemDataBound
            Dim dateColumns As New List(Of String)
            'Add datecolumn uniqueName in list for Date format
            dateColumns.Add("GridDateTimeColumnEndDate")
            CommonMethods.FormatedDateOnGrid(dateColumns, e.Item)
            'Suraj Issue 14450 3/22/13 ,we provide a tool tip for DatePopupButton as well as the GridDateTimeColumnStartDate textbox   
            If TypeOf e.Item Is GridFilteringItem Then
                Dim filterItem As GridFilteringItem = DirectCast(e.Item, GridFilteringItem)
                Dim gridDateTimeColumnStartDate As RadDatePicker = DirectCast(filterItem("GridDateTimeColumnEndDate").Controls(0), RadDatePicker)
                gridDateTimeColumnStartDate.ToolTip = "Enter a filter date"
                gridDateTimeColumnStartDate.DatePopupButton.ToolTip = "Select a filter date"
            End If
        End Sub

        Protected Sub grdMain_PageIndexChanging(ByVal sender As Object, ByVal e As GridPageChangedEventArgs) Handles grdMain.PageIndexChanged
            grdMain.CurrentPageIndex = e.NewPageIndex
            If ViewState(ATTRIBUTE_DATATABLE_SUBSCRIPTIONS) IsNot Nothing Then
                grdMain.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_SUBSCRIPTIONS), DataTable)
            End If
            'Suraj Issue 14450 3/23/13 ,maintain the checkedstate of values 
            SaveCheckedValues()
        End Sub
        Protected Sub grdMain_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdMain.NeedDataSource
            If ViewState(ATTRIBUTE_DATATABLE_SUBSCRIPTIONS) IsNot Nothing Then
                grdMain.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_SUBSCRIPTIONS), DataTable)
            End If
            'Suraj Issue 14450 3/23/13 ,maintain the checkedstate of values 
            SaveCheckedValues()
        End Sub
        'Suraj Issue 14450 3/22/13 ,if the grid load first time By default the sorting will be Ascending for column Forum 
        Private Sub AddExpressionRenewal()
            Dim expression1 As New GridSortExpression
            expression1.FieldName = "PurchaseType"
            expression1.SetSortOrder("Ascending")
            grdMain.MasterTableView.SortExpressions.AddSortExpression(expression1)
        End Sub
        Protected Function RowSelected(ByVal dt As DataTable) As Boolean
            Try

                Dim bSubscriptionSelected As Boolean = False
                If dt.Rows.Count > 0 Then
                    bSubscriptionSelected = True
                End If
                Return bSubscriptionSelected
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return False
            End Try
        End Function
        'Suraj Issue 14450 3/23/13 ,This method is used to save the checkedstate of values 
        Private Sub SaveCheckedValues()
            Dim userdetails As New ArrayList()
            Dim index As Long = -1
            Dim dtSelectedRecord As DataTable
            dtSelectedRecord = New DataTable
            dtSelectedRecord.Columns.Add("SubscriptionID")
            dtSelectedRecord.Columns.Add("ProductName")
            dtSelectedRecord.Columns.Add("PurchaseType")
            dtSelectedRecord.Columns.Add("ProductID")
            Dim primaryKey(0) As DataColumn
            primaryKey(0) = dtSelectedRecord.Columns("SubscriptionID")
            dtSelectedRecord.PrimaryKey = primaryKey

            If ViewState(ATTRIBUTE_SELECTED_ROWS) IsNot Nothing Then
                dtSelectedRecord = CType(ViewState(ATTRIBUTE_SELECTED_ROWS), DataTable)
            End If
            For Each item As GridDataItem In grdMain.MasterTableView.Items
                index = CLng(CType(item.FindControl("lblSubscriptionID"), Label).Text)
                Dim result As Boolean = DirectCast(item.FindControl("chkRenewal"), CheckBox).Checked
                Dim dr As DataRow = dtSelectedRecord.NewRow
                If result Then
                    dr.Item("SubscriptionID") = CType(item.FindControl("lblSubscriptionID"), Label).Text
                    dr.Item("ProductName") = CType(item.FindControl("lblProductName"), Label).Text
                    dr.Item("PurchaseType") = CType(item.FindControl("lblPurchaseType"), Label).Text
                    dr.Item("ProductID") = CType(item.FindControl("lblProductID"), Label).Text
                    If Not dtSelectedRecord.Rows.Contains(index) Then
                        dtSelectedRecord.Rows.Add(dr)
                    End If
                Else
                    If dtSelectedRecord.Rows.Contains(index) Then
                        dr = dtSelectedRecord.Rows.Find(index)
                        dtSelectedRecord.Rows.Remove(dr)
                    End If
                End If
            Next
            If dtSelectedRecord IsNot Nothing AndAlso dtSelectedRecord.Rows.Count > 0 Then
                ViewState(ATTRIBUTE_SELECTED_ROWS) = dtSelectedRecord
            End If
        End Sub
    End Class
End Namespace
