'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Framework.Application
Imports Aptify.Framework.BusinessLogic
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.AttributeManagement
Imports System.Data
Imports Telerik.Web.UI

Namespace Aptify.Framework.Web.eBusiness.Committees
    Partial Class CommitteeListingControl
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "CommitteeListing"
        Protected Const ATTRIBUTE_DATATABLE_COMMITTEE As String = "dtCommittee"


        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
            If Not IsPostBack Then
                LoadCommittees()
                AddExpression()
            End If
        End Sub

        Private Sub AddExpression()
            Dim expression1 As New GridSortExpression
            expression1.FieldName = "Name"
            expression1.SetSortOrder("Ascending")
            grdCommittees.MasterTableView.SortExpressions.AddSortExpression(expression1)
        End Sub

        Protected Sub grdCommittees_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdCommittees.ItemDataBound
            Dim dateColumns As New List(Of String)
            'Add datecolumn uniqueName in list for Date format
            dateColumns.Add("GridDateTimeColumnDateFounded")
            CommonMethods.FormatedDateOnGrid(dateColumns, e.Item)
        End Sub

        Protected Sub grdResults_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdCommittees.NeedDataSource
            If ViewState(ATTRIBUTE_DATATABLE_COMMITTEE) IsNot Nothing Then
                grdCommittees.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_COMMITTEE), DataTable)
            End If
        End Sub

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()
        End Sub

        Private Sub LoadCommittees()

            Try
                If ViewState(ATTRIBUTE_DATATABLE_COMMITTEE) IsNot Nothing Then
                    grdCommittees.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_COMMITTEE), DataTable)
                    grdCommittees.DataBind()
                Else
                    Dim sDatabase As String = AptifyApplication.GetEntityBaseDatabase("Committees")
                    Dim sSQL As String
                    Dim dt As DataTable

                    If Not Me.SetControlRecordIDFromParam() Then
                        sSQL = "Select ID,Name,Description,Goals,DateFounded FROM " & _
                               sDatabase & _
                               "..vwCommittees WHERE IsActive = 1 ORDER BY Name"
                    Else
                        sSQL = "SELECT ID,Name,Description,Goals,DateFounded FROM " & _
                               sDatabase & _
                               "..vwCommittees WHERE ParentID=" & Me.ControlRecordID.ToString & " ORDER BY Name"
                    End If

                    dt = DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)
                    If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                        grdCommittees.DataSource = dt
                        ViewState(ATTRIBUTE_DATATABLE_COMMITTEE) = dt
                        grdCommittees.DataBind()
                    End If
                End If

            Catch ex As FormatException
                ' changed code to use config settings instead of hardcoded paths
                Response.Redirect(Me.GetSecurityErrorPageFromXML & "?Message=" & Server.UrlEncode("Parameter must be numeric"))
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try

        End Sub

        Protected Sub grdCommittees_PageIndexChanging(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageChangedEventArgs) Handles grdCommittees.PageIndexChanged
            grdCommittees.CurrentPageIndex = e.NewPageIndex
            If ViewState(ATTRIBUTE_DATATABLE_COMMITTEE) IsNot Nothing Then
                grdCommittees.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_COMMITTEE), DataTable)
            End If
        End Sub
    End Class
End Namespace
