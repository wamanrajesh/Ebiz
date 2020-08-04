'Aptify e-Business 5.5.1, July 2013
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Framework.Application
Imports Aptify.Framework.BusinessLogic
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.AttributeManagement
Imports System.Data
Imports Telerik.Web.UI

Namespace Aptify.Framework.Web.eBusiness.Committees

    Partial Class MyCommitteesControl
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced

        Protected Const ATTRIBUTE_COMMITTEE_TYPE_QUERYSTRING_NAME As String = "CommitteeTypeQueryStringName"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "MyCommittees"
        Protected Const ATTRIBUTE_DATATABLE_COMMITTEES As String = "dtCommittees"


#Region "My Committees Page Properties"
        Private m_sType As String

        <System.ComponentModel.Category("My Committees Page Properties")> _
        Public Property CommitteeTypeQueryStringName() As String
            Get
                Return m_sType
            End Get
            Set(ByVal value As String)
                m_sType = value
            End Set
        End Property
#End Region

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()

            Try
                If Not IsPostBack Then
                    If Len(Request.QueryString(Me.CommitteeTypeQueryStringName)) > 0 Then

                        Dim oItem As System.Web.UI.WebControls.ListItem
                        oItem = cmbType.Items.FindByValue(Request.QueryString(Me.CommitteeTypeQueryStringName))
                        If Not oItem Is Nothing Then
                            oItem.Selected = True
                        End If
                    End If
                    LoadCommittees()
                    'Anil B for issues 144499 on 05-04-2013
                    'Add sorting symbol
                    AddExpression()
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            Finally
            End Try
        End Sub

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(CommitteeTypeQueryStringName) Then
                CommitteeTypeQueryStringName = Me.GetPropertyValueFromXML(ATTRIBUTE_COMMITTEE_TYPE_QUERYSTRING_NAME)
                If String.IsNullOrEmpty(CommitteeTypeQueryStringName) Then
                    CommitteeTypeQueryStringName = "Type"
                End If
            End If
            If String.IsNullOrEmpty(Me.RedirectURL) Then
                Me.grdCommittees.Enabled = False
                Me.grdCommittees.ToolTip = "RedirectURL property has not been set."
            End If

        End Sub

        Protected Sub grdCommittees_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdCommittees.NeedDataSource
            If ViewState(ATTRIBUTE_DATATABLE_COMMITTEES) IsNot Nothing Then
                grdCommittees.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_COMMITTEES), DataTable)
            End If
        End Sub

        Private Sub LoadCommittees()
            Try
                If ViewState(ATTRIBUTE_DATATABLE_COMMITTEES) IsNot Nothing Then
                    grdCommittees.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_COMMITTEES), DataTable)
                    grdCommittees.DataBind()
                    Exit Sub
                End If
                If User1.PersonID > 0 Then
                    Dim sDatabase As String = AptifyApplication.GetEntityBaseDatabase("Committees")
                    Dim sSQL As String
                    Dim dt As DataTable
                    'HP Issue 8242 part 2, added extra filter to only retrieve active commitees
                    sSQL = "SELECT ct.ID CommitteeTermID," & _
                               "c.Name Committee,ct.Name CommitteeTerm,ctm.Title, " & _
                                   "StartDate = CASE WHEN ctm.StartDate <= '1900-01-01' THEN ct.StartDate " & _
                                   "ELSE ctm.StartDate END, " & _
                                   "EndDate = CASE WHEN ctm.EndDate <= '1900-01-01' THEN ct.EndDate " & _
                                   "ELSE ctm.EndDate END " & _
                                   "FROM " & _
                                   sDatabase & "..vwCommitteeTerms ct   " & _
                                   "INNER JOIN " & _
                                   sDatabase & "..vwCommittees c ON " & _
                                   "ct.CommitteeID=c.ID " & _
                                   "INNER JOIN " & _
                                   sDatabase & "..vwCommitteeTermMembers ctm" & _
                                   " ON ct.ID=ctm.CommitteeTermID " & _
                                   "WHERE ctm.MemberID=" & User1.PersonID & " AND IsActive = 1 "
                    '8/1/06 RJK - Added Committee Term Member Date Filters as well.
                    '
                    'ISSUE 4343
                    '12/4/06 MAS - Altered Committee Term where clause to correctly display past and future.
                    Select Case cmbType.SelectedItem.Text
                        '' RashmiP, Issue 6454, Date 8/10/11. Changed conditions in Queries, for Current, Furture, Past conditions 
                        '' to get Committee Terms without Start Date and End Date.
                        ''Nalini issue 6454
                        Case "Current"
                            ' RFB - updated to allow the EndDate to not be set.
                            '       Date: 6/30/2003
                            ''sSQL = sSQL & " AND ct.StartDate<=GETDATE() AND (ct.EndDate >=GETDATE() OR ct.EndDate = '')" & _
                            ''                " AND ctm.StartDate<=GETDATE() AND (ctm.EndDate >=GETDATE() OR ctm.EndDate = '')"
                            sSQL = sSQL & " AND (ct.StartDate <= CONVERT(date, GETDATE()) OR ct.StartDate = '' )" & _
                                        " AND (ct.EndDate >= CONVERT(date, GETDATE()) OR ct.EndDate = '' OR ct.EndDate is null) " & _
                                        "AND ctm.StartDate <= CONVERT(date, GETDATE())  AND (ctm.EndDate >= CONVERT(date, GETDATE()) OR ctm.EndDate = '')"

                        Case "Future"
                            sSQL = sSQL & " AND (ct.StartDate > GETDATE()" & _
                                            " OR ctm.StartDate > GETDATE())" & _
                                            " OR ct.StartDate ='' " & _
                                            " AND (ct.EndDate > GETDATE() OR ct.EndDate = '')"
                            ''sSQL = sSQL & " AND (ct.StartDate > GETDATE()" & _
                            ''               " OR ctm.StartDate > GETDATE())" & _
                            ''               " AND (ct.EndDate > GETDATE() OR ct.EndDate = '')"'in case ct.end < today but ctm.start > today
                            'issue 4561 has been created to enforce date validation for committee terms in the Committee addon.
                            'once this is in place, the last future filter (AND (ct.EndDate > GETDATE() OR ct.EndDate = '') can be removed.
                        Case "Past"
                            'If ctm.EndDate = '' (1/1/1990) then ct.EndDate is assumed to be the Member's EndDate.
                            ''sSQL = sSQL & " AND ((ct.EndDate <= GETDATE() AND ct.EndDate<>'' AND ct.StartDate < GETDATE()) " & _
                            ''                " OR (ctm.EndDate <= GETDATE() AND ctm.EndDate <>''AND ctm.StartDate < GETDATE()))"

                            sSQL = sSQL & " AND (ct.StartDate < CONVERT(date, GETDATE()) OR ct.StartDate = '' OR ct.STARTDATE = '1900-01-01')" & _
                            "  AND (ct.EndDate < CONVERT(date,GETDATE()) OR ct.EndDate = '' OR ct.EndDate IS NULL ) " & _
                            "AND ctm.StartDate < CONVERT(date, GETDATE()) AND (ctm.EndDate < CONVERT(date,GETDATE()) OR ctm.EndDate = '' ) "

                    End Select

                    sSQL = sSQL & " ORDER BY c.Name DESC"
                    dt = DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)

                    Dim dcolUrl As DataColumn = New DataColumn()
                    dcolUrl.Caption = "DataNavigateUrl"
                    dcolUrl.ColumnName = "DataNavigateUrl"

                    dt.Columns.Add(dcolUrl)
                    If dt.Rows.Count > 0 Then
                        For Each rw As DataRow In dt.Rows
                            Dim tempURL As String = Me.RedirectURL & "?ID=" & rw("CommitteeTermID")


                            Dim index As Integer = tempURL.IndexOf("=")
                            Dim sValue As String = tempURL.Substring(index + 1)
                            Dim separator As String() = tempURL.Split(CChar("="))
                            Dim navigate As String = separator(0)
                            navigate = navigate & "="
                            navigate = navigate & System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(sValue))
                            rw("DataNavigateUrl") = navigate
                        Next
                    End If

                    If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                        lblNoCommittees.Visible = True
                        grdCommittees.Visible = False
                    Else
                        lblNoCommittees.Visible = False
                        grdCommittees.Visible = True
                    End If
                    grdCommittees.DataSource = dt
                    grdCommittees.DataBind()
                    ViewState(ATTRIBUTE_DATATABLE_COMMITTEES) = dt
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub cmbType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbType.SelectedIndexChanged
            'Anil B for issues 144499 on 05-04-2013
            'Add sorting symbol
            AddExpression()
            ViewState(ATTRIBUTE_DATATABLE_COMMITTEES) = Nothing
            LoadCommittees()
        End Sub

        Protected Sub grdCommittees_PageIndexChanging(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageChangedEventArgs) Handles grdCommittees.PageIndexChanged
            grdCommittees.CurrentPageIndex = e.NewPageIndex
            If ViewState(ATTRIBUTE_DATATABLE_COMMITTEES) IsNot Nothing Then
                grdCommittees.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_COMMITTEES), DataTable)
            End If
        End Sub
        'Anil B for issues 144499 on 05-04-2013
        'Add Function for sorting symbol
        Private Sub AddExpression()
            Dim ExpOrderSort As New GridSortExpression
            ExpOrderSort.FieldName = "Committee"
            ExpOrderSort.SetSortOrder("Ascending")
            grdCommittees.MasterTableView.SortExpressions.AddSortExpression(ExpOrderSort)
        End Sub
		 '  Navin Prasad Issue 11032
        ' Changes made for to allow encrypting and decrypting the URL.
        ' Changes made by Hrushikesh Jog
        'Protected Sub grdCommittees_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdCommittees.ItemDataBound
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

        'Protected Sub grdCommittees_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdCommittees.RowDataBound
        '    Try
        '        Dim type As ListItemType = CType(e.Row.RowType, ListItemType)
        '        If CType(e.Row.RowType, ListItemType) = ListItemType.Item Or _
        '        CType(e.Row.RowType, ListItemType) = ListItemType.AlternatingItem Then
        '            Dim lnk As HyperLink
        '            Dim tempURL As String
        '            Dim index As Integer
        '            Dim sValue As String
        '            Dim separator As String()

        '            lnk = CType(e.Row.Cells(0).Controls(0), HyperLink)
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
    End Class
End Namespace
