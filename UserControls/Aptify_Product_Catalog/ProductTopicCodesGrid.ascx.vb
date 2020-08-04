'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.Application
Imports Telerik.Web.UI

Namespace Aptify.Framework.Web.eBusiness.ProductCatalog
    Partial Class ProductTopicCodesGrid
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "ProductTopicCodesGrid"

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            'if values are not provide directly or from the XML file, set default values for inherited properties since 
            'control requires them to properly function
            If String.IsNullOrEmpty(Me.QueryStringRecordIDParameter) Then Me.QueryStringRecordIDParameter = "ID"
        End Sub

        Public Sub LoadGrid(ByVal ProductID As Long)
            Dim sSQL As String
            Dim dt, dt2 As New System.Data.DataTable

            Try
                sSQL = "SELECT DISTINCT(tc.ID),tc.Name, tc.Description, tc.WebEnabled " & _
                       "FROM " & AptifyApplication.GetEntityBaseDatabase("Topic Codes") & _
                       "..vwTopicCodes tc INNER JOIN " & _
                       AptifyApplication.GetEntityBaseDatabase("Topic Codes") & _
                       "..vwTopicCodeLinks tcl ON tc.ID=tcl.TopicCodeID WHERE tcl.EntityID=" & _
                       "(SELECT ID FROM " & AptifyApplication.GetEntityBaseDatabase("Entities") & _
                       "..vwEntities " & _
                       "WHERE Name='Products') AND RecordID=" & ProductID & _
                       " AND tcl.Status = 'Active' AND tc.WebEnabled <> 'Not Available' " & _
                       " ORDER BY tc.Name ASC"
                dt = DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)

                ''RashmiP,11/15/11, Issue 11893, Only Selected and Web Enabled by Service Topics should Appear on Web Site
                If Not dt Is Nothing Then
                    Dim dt1 As System.Data.DataTable
                    Dim dr1 As System.Data.DataRow
                    dt2.Columns.Add("ID")
                    dt2.Columns.Add("Name")
                    dt2.Columns.Add("Description")
                    For Each dr As System.Data.DataRow In dt.Rows
                        If CStr(dr.Item("WebEnabled")) = "By Service" Then
                            sSQL = "SELECT WebEnabled FROM " & _
                            AptifyApplication.GetEntityBaseDatabase("Topic Codes") & "..vwTopicCodeEntities " & _
                            " WHERE EntityID_Name = 'Products' AND Status = 'Active' AND WebEnabled = 'True' And TopicCodeID = " & DirectCast(dr.Item(0), Integer)
                            dt1 = DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)
                            If dt1.Rows.Count > 0 Then
                                dr1 = dt2.NewRow
                                dr1("ID") = dr("ID")
                                dr1("Name") = dr("Name")
                                dr1("Description") = dr("Description")
                                dt2.Rows.Add(dr1)
                            End If
                        Else
                            dr1 = dt2.NewRow
                            dr1("ID") = dr("ID")
                            dr1("Name") = dr("Name")
                            dr1("Description") = dr("Description")
                            dt2.Rows.Add(dr1)
                        End If
                    Next
                End If

                If Not dt2 Is Nothing AndAlso _
                   dt2.Rows.Count > 0 Then
                    grdMain.DataSource = dt2
                    ' grdMain.DataBind()
                    Me.Visible = True
                Else
                    Me.Visible = False
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        'Neha, issue 14456, 03/15/13, added method for assending order sorting on first time gridload(for first column)
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not IsPostBack Then
                AddExpression()
            End If
            SetProperties()
            Try
                Me.SetControlRecordIDFromParam()
                If Me.ControlRecordID > 0 Then
                    Me.LoadGrid(Me.ControlRecordID)
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

      
 
        ''' <summary>
        ''' Nalini issue 12436 date:1/12/2011
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub grdMain_PageIndexChanging(ByVal sender As Object, ByVal e As GridPageChangedEventArgs) Handles grdMain.PageIndexChanged
            ''grdMain.PageIndex = e.NewPageIndex
            Me.LoadGrid(Me.ControlRecordID)
        End Sub
        Protected Sub grdMain_PageIndexChanging(ByVal sender As Object, ByVal e As GridPageSizeChangedEventArgs) Handles grdMain.PageSizeChanged
            ''grdMain.PageIndex = e.NewPageIndex
            Me.LoadGrid(Me.ControlRecordID)
        End Sub
        Protected Sub grdMain_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdMain.NeedDataSource
            Me.LoadGrid(Me.ControlRecordID)
            ''grdStudents.Rebind()
        End Sub
        'neha,issue 14456,03/15/13,method for sorting assending order on first time gridload(for first column)
        Private Sub AddExpression()
            Dim expression1 As New GridSortExpression
            expression1.FieldName = "Name"
            expression1.SetSortOrder("Ascending")
            grdMain.MasterTableView.SortExpressions.AddSortExpression(expression1)
        End Sub

    End Class
End Namespace
