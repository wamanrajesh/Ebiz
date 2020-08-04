'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On
Imports Telerik.Web.UI
Imports System.Data
Namespace Aptify.Framework.Web.eBusiness.Education
    Partial Class MyCertificationsControl
        Inherits eBusiness.BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CEU_SUBMISSION_PAGE As String = "CEUSubmissionPage"
        Protected Const ATTRIBUTE_VIEW_CERTIFICATION_PAGE As String = "ViewCertificationPage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "MyCertifications"
        Protected Const ATTRIBUTE_DATATABLE_CERTIFICATION As String = "dtCertification"

#Region "MyCertifications Specific Properties"
        ''' <summary>
        ''' CEUSubmission page url
        ''' </summary>
        Public Overridable Property CEUSubmissionPage() As String
            Get
                If Not ViewState(ATTRIBUTE_CEU_SUBMISSION_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_CEU_SUBMISSION_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_CEU_SUBMISSION_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' ViewCertification page url
        ''' </summary>
        Public Overridable Property ViewCertificationPage() As String
            Get
                If Not ViewState(ATTRIBUTE_VIEW_CERTIFICATION_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_VIEW_CERTIFICATION_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_VIEW_CERTIFICATION_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
#End Region

        Protected Overridable Sub OnPageLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'set control properties from XML file if needed
            SetProperties()
            If Not IsPostBack Then
                AddExpression()
                ' Changes made to get the query string name from a property set by CMS
                ' Changes made by CP 7/14/2008
                Dim sID As String = Request.QueryString(Me.QueryStringRecordIDParameter)
                If Me.IsQueryStringEncrypted Then
                    sID = Aptify.Framework.Web.Common.WebCryptography.Decrypt(sID)
                End If
                If sID IsNot Nothing Then
                    Select Case sID.ToUpper.Trim
                        Case "GRANTED"
                            Me.cmbType.SelectedIndex = 0
                        Case "CANCELLED"
                            Me.cmbType.SelectedIndex = 1
                        Case "EXPIRED"
                            Me.cmbType.SelectedIndex = 2
                        Case "DECLARED"
                            Me.cmbType.SelectedIndex = 4
                        Case Else 'By Default display ALL certifications
                            Me.cmbType.SelectedIndex = 3
                    End Select
                Else
                    'By Default display ALL certifications
                    Me.cmbType.SelectedIndex = 3
                End If
                Me.LoadGrid()
            End If
        End Sub

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(CEUSubmissionPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                CEUSubmissionPage = Me.GetLinkValueFromXML(ATTRIBUTE_CEU_SUBMISSION_PAGE)
                If String.IsNullOrEmpty(CEUSubmissionPage) Then
                    Me.hlnkSubmitNewCEU.Font.Underline = True
                    Me.hlnkSubmitNewCEU.Enabled = False
                    Me.hlnkSubmitNewCEU.ToolTip = "CEUSubmissionPage property has not been set."
                Else
                    Me.hlnkSubmitNewCEU.NavigateUrl = CEUSubmissionPage
                End If
            Else
                Me.hlnkSubmitNewCEU.NavigateUrl = CEUSubmissionPage
            End If

            If String.IsNullOrEmpty(ViewCertificationPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                ViewCertificationPage = Me.GetLinkValueFromXML(ATTRIBUTE_VIEW_CERTIFICATION_PAGE)
                If String.IsNullOrEmpty(ViewCertificationPage) Then
                    Me.grdMyCertifications.Enabled = False
                    Me.grdMyCertifications.ToolTip = "ViewCertificationPage property has not been set."
                Else
                    'Navin Prasad Issue 11032
                    'DirectCast(grdMyCertifications.Columns(0), HyperLinkColumn).DataNavigateUrlFormatString = ViewCertificationPage & "?CertificationID={0:F0}"
                End If
            End If

        End Sub

        ''' <summary>
        ''' This method loads the grid on the page, override the method functionality to alter the grid loading functionality
        ''' </summary>
        ''' <remarks></remarks>
        Protected Overridable Sub LoadGrid()
            Dim sSQL As String
            Dim dt As Data.DataTable
            Try
                If ViewState(ATTRIBUTE_DATATABLE_CERTIFICATION) IsNot Nothing Then
                    grdMyCertifications.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_CERTIFICATION), Data.DataTable)
                    grdMyCertifications.DataBind()
                    Exit Sub
                End If
                'Suraj Issue 14452 5/6/13,  change date CONVERT format because we are using the eg:Jan 24, 1998 this format
                sSQL = "SELECT c.ID, c.Title, c.Type, " & _
                       "CONVERT(NVARCHAR(12), c.DateGranted, 107) 'DateGranted', " & _
                       "CONVERT(NVARCHAR(12), c.ExpirationDate, 107) 'ExpirationDate', " & _
                       "c.Status FROM " & _
                       AptifyApplication.GetEntityBaseDatabase("Certifications") & _
                       "..vwCertifications c WHERE StudentID=" & User1.PersonID
                If cmbType.SelectedValue.ToUpper.Trim <> "" AndAlso cmbType.SelectedValue.ToUpper.Trim <> "ALL" Then
                    sSQL &= " AND c.Status='" & cmbType.SelectedValue.Trim & "'"
                End If
                sSQL &= " ORDER BY c.DateGranted DESC"
                dt = DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                'dt.Columns("ExpirationDate").DataType = System.String
                For Each row As Data.DataRow In dt.Rows
                    If row.Item("ExpirationDate") Is Nothing _
                            OrElse IsDBNull(row.Item("ExpirationDate")) _
                            OrElse CDate(row.Item("ExpirationDate")).Year = 1900 Then
                        'Suraj Issue 14452 , 5/10/13 filtering not working if we assing  row.Item("ExpirationDate") = "" because its consider as a string  so thats why we apply nothing for proper filtering
                        row.Item("ExpirationDate") = Nothing
                    End If
                Next


                Dim dcolUrl As DataColumn = New DataColumn()
                dcolUrl.Caption = "CertificationUrl"
                dcolUrl.ColumnName = "CertificationUrl"

                dt.Columns.Add(dcolUrl)
                Dim dcolUrll As DataColumn = New DataColumn()
                dcolUrll.Caption = "ExpirationDaterow"
                dcolUrll.ColumnName = "ExpirationDaterow"

                dt.Columns.Add(dcolUrll)

                Dim dcolUrllabel As DataColumn = New DataColumn()
                dcolUrllabel.Caption = "DateGrantedrow"
                dcolUrllabel.ColumnName = "DateGrantedrow"

                dt.Columns.Add(dcolUrllabel)
                If dt.Rows.Count > 0 Then


                    For Each rw As DataRow In dt.Rows
                        rw("CertificationUrl") = Me.ViewCertificationPage + "?CertificationID=" + rw("ID").ToString()
                        rw("ExpirationDaterow") = String.Format("{0:d}", rw("ExpirationDate").ToString)
                        rw("DateGrantedrow") = String.Format("{0:d}", rw("DateGranted").ToString)
                    Next
                End If


                Me.grdMyCertifications.DataSource = dt
                grdMyCertifications.DataBind()
                ViewState(ATTRIBUTE_DATATABLE_CERTIFICATION) = dt

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub cmbType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbType.SelectedIndexChanged
            ViewState(ATTRIBUTE_DATATABLE_CERTIFICATION) = Nothing
            LoadGrid()
        End Sub

        Protected Sub grdMyCertifications_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdMyCertifications.ItemDataBound
            'Suraj Issue  14452 ,5/3/13  ,we provide a tool tip for DatePopupButton as well as the GridDateTimeColumnEndDate textbox and GridDateTimeColumnEndDate textbox
            If TypeOf e.Item Is GridFilteringItem Then
                Dim filterItem As GridFilteringItem = DirectCast(e.Item, GridFilteringItem)
                Dim GridDateTimeColumnEndDate As RadDatePicker = DirectCast(filterItem("GridDateTimeColumnEndDate").Controls(0), RadDatePicker)
                GridDateTimeColumnEndDate.ToolTip = "Enter a filter date"
                GridDateTimeColumnEndDate.DatePopupButton.ToolTip = "Select a filter date"
                Dim gridDateTimeColumnStartDate As RadDatePicker = DirectCast(filterItem("GridDateTimeColumnStartDate").Controls(0), RadDatePicker)
                gridDateTimeColumnStartDate.ToolTip = "Enter a filter date"
                gridDateTimeColumnStartDate.DatePopupButton.ToolTip = "Select a filter date"
            End If
        End Sub
        'Suraj Issue 14829 4/29/13,  Remove "ItemDataBound" Event because we remove time from grid 

        ''' <summary>
        ''' Nalini 12436 date:1/12/2011
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub grdMyCertifications_PageIndexChanging(ByVal sender As Object, ByVal e As GridPageChangedEventArgs) Handles grdMyCertifications.PageIndexChanged
            grdMyCertifications.CurrentPageIndex = e.NewPageIndex
            If ViewState(ATTRIBUTE_DATATABLE_CERTIFICATION) IsNot Nothing Then
                grdMyCertifications.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_CERTIFICATION), Data.DataTable)
            End If
        End Sub
        Protected Sub grdMyCertifications_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdMyCertifications.NeedDataSource
            If ViewState(ATTRIBUTE_DATATABLE_CERTIFICATION) IsNot Nothing Then
                grdMyCertifications.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_CERTIFICATION), Data.DataTable)
            End If
        End Sub


        Private Sub AddExpression()
            Dim ExpOrderSort As New GridSortExpression
            ExpOrderSort.FieldName = "Title"
            ExpOrderSort.SetSortOrder("Ascending")
            grdMyCertifications.MasterTableView.SortExpressions.AddSortExpression(ExpOrderSort)
        End Sub

        Protected Sub grdMyCertifications_PageSizeChanged(sender As Object, e As Telerik.Web.UI.GridPageSizeChangedEventArgs) Handles grdMyCertifications.PageSizeChanged
            If ViewState(ATTRIBUTE_DATATABLE_CERTIFICATION) IsNot Nothing Then
                grdMyCertifications.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_CERTIFICATION), Data.DataTable)
            End If
        End Sub
    End Class
End Namespace
