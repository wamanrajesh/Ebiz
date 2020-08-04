'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports System.Data.SqlClient
Imports System.Data
Imports System.Configuration
Imports System.Collections.Generic
Imports Telerik.Web.UI


Namespace Aptify.Framework.Web.eBusiness.Forums
    Partial Class ForumSubscription
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_FORUMS_HOME_PAGE As String = "ForumsHomePage"
        Protected Const ATTRIBUTE_HAS_PARENT_IMAGE_URL As String = "HasParentImage"
        Protected Const ATTRIBUTE_NO_PARENT_IMAGE_URL As String = "NoParentImage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "ForumSubscription"
        Protected Const ATTRIBUTE_FORUMSUBSCRIPTION_VIEWSTATE As String = "ForumSubscriptionsdt"
        Protected Const ATTRIBUTE_CHECKED_SUBSCRIPTION = "CheckedSubscriptionFormGrid"


#Region "ForumSubscription Specific Properties"
        ''' <summary>
        ''' ForumsHome page url
        ''' </summary>
        Public Overridable Property ForumsHomePage() As String
            Get
                If Not ViewState(ATTRIBUTE_FORUMS_HOME_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_FORUMS_HOME_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_FORUMS_HOME_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' HasParentImage url
        ''' </summary>
        Public Overridable Property HasParentImage() As String
            Get
                If Not ViewState(ATTRIBUTE_HAS_PARENT_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_HAS_PARENT_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_HAS_PARENT_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' NoParentImage url
        ''' </summary>
        Public Overridable Property NoParentImage() As String
            Get
                If Not ViewState(ATTRIBUTE_NO_PARENT_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_NO_PARENT_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_NO_PARENT_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        Public Overridable ReadOnly Property UserId() As Long
            Get
                m_UserId = User1.UserID
                Return m_UserId
            End Get
        End Property

#End Region

        Protected m_UserId As Long
        Private dtMain As DataTable

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'Suraj issue 14455 4/25/13 ,this method use to apply the odrering of rad grid first column
            If Not IsPostBack Then
                AddExpression()
            End If
            'set control properties from XML file if needed
            SetProperties()
            lblResults.Text = String.Empty
            If Not IsPostBack Then
                FillGrid()
            End If
        End Sub


        Protected Overridable Sub FillGrid()
            Try
                Dim sSQL As String
                Dim dt As DataTable
                Dim dicSubscriptionDetails As New Dictionary(Of Integer, List(Of String))
                Dim lstSubsValue As List(Of String)
                'Suraj issue 14455, 2/20/13 , check the view state is nothing or not if the page load first time viewstate will be nothing but after bostback view state will conatin the datatable
                If ViewState(ATTRIBUTE_FORUMSUBSCRIPTION_VIEWSTATE) Is Nothing Then
                    sSQL = "select Subscribed=Case when fs.id IS NULL then 0 else " & _
                    "Case fs.Status when 'Active' then 1 else 0 end end, DeliveryType = " & _
                    "fs.DeliveryType,ForumType = df.Type,Name=df.Name,Parent=df.Parent, " & _
                    "ID=df.ID from vwdiscussionforums df left join " & _
                    "vwDiscussionForumSubscriptions fs on df.ID=fs.ForumID and fs.WebUserID= " & _
                    Me.User1.UserID.ToString() & " order by Parent,name"
                    dt = Me.DataAction.GetDataTable(sSQL)
                    'Navin Prasad Issue 9238
                    Session("SubGV") = New DataView(dt)
                    grdForumSubscriptions.DataSource = dt
                    ''grdForumSubscriptions.AllowPaging = False
                    grdForumSubscriptions.DataBind()
                    ViewState(ATTRIBUTE_FORUMSUBSCRIPTION_VIEWSTATE) = dt
                Else
                    'Suraj issue 14455 2/20/13 , after postback viewstate will assign for gridview
                    grdForumSubscriptions.DataSource = CType(ViewState(ATTRIBUTE_FORUMSUBSCRIPTION_VIEWSTATE), DataTable)
                    grdForumSubscriptions.DataBind()
                End If
                'Issue 13164
                If chkEnableAll.Checked AndAlso dt IsNot Nothing AndAlso dt.Rows.Count > 0 And grdForumSubscriptions IsNot Nothing Then
                    For Each rw As Telerik.Web.UI.GridDataItem In grdForumSubscriptions.Items
                        Dim chk As CheckBox = CType(rw.FindControl("chkSubscription"), CheckBox)
                        If chk IsNot Nothing Then
                            chk.Checked = True
                            chk.Enabled = False
                        End If
                    Next
                End If
                If grdForumSubscriptions IsNot Nothing AndAlso grdForumSubscriptions.Items.Count > 0 Then
                    For Each rw As Telerik.Web.UI.GridDataItem In grdForumSubscriptions.Items
                        If grdForumSubscriptions.Columns(4) IsNot Nothing Then
                            grdForumSubscriptions.Columns(4).Visible = False
                        End If
                        If grdForumSubscriptions.Columns(5) IsNot Nothing Then
                            grdForumSubscriptions.Columns(5).Visible = False
                        End If
                    Next
                End If
                'Added by Sandeep For Issue 14671 on 27/02/2013
                'For Seeting Initial values of FormSubscriptionGrid
                If Not IsPostBack Then
                    If dt IsNot Nothing Then
                        For Each dr As DataRow In dt.Rows
                            lstSubsValue = New List(Of String)
                            If CBool(dr("Subscribed")) Then
                                lstSubsValue.Add(CStr(dr("DeliveryType")))
                                lstSubsValue.Add(CStr(CBool(dr("Subscribed"))))
                                dicSubscriptionDetails.Add(CInt(dr("ID")), lstSubsValue)
                            End If
                        Next
                        ViewState(ATTRIBUTE_CHECKED_SUBSCRIPTION) = dicSubscriptionDetails
                    End If
                End If
            Catch ex As Exception
                ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Overrides Sub SetProperties()
            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(ForumsHomePage) Then
                'since value is the 'default' check the XML file for possible custom setting
                ForumsHomePage = Me.GetLinkValueFromXML(ATTRIBUTE_FORUMS_HOME_PAGE)
                Me.lnkForumsHome.Visible = False
                If String.IsNullOrEmpty(ForumsHomePage) Then
                    Me.lnkForumsHome.Enabled = False
                    Me.lnkForumsHome.ToolTip = "ForumsHomePage property has not been set."
                Else
                    Me.lnkForumsHome.NavigateUrl = ForumsHomePage
                End If
            Else
                Me.lnkForumsHome.NavigateUrl = ForumsHomePage
            End If

            If String.IsNullOrEmpty(HasParentImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                HasParentImage = Me.GetLinkValueFromXML(ATTRIBUTE_HAS_PARENT_IMAGE_URL)

            End If

        End Sub

        Private Sub PopulateColums(ByRef dtMain As DataTable)
            Dim oCol1 As New DataColumn
            Dim oCol2 As New DataColumn
            Dim oCol3 As New DataColumn
            Dim oCol4 As New DataColumn
            Dim oCol5 As New DataColumn
            Dim oCol6 As New DataColumn

            oCol1.DataType = System.Type.GetType("System.Boolean")
            oCol2.DataType = System.Type.GetType("System.String")
            oCol3.DataType = System.Type.GetType("System.String")
            oCol4.DataType = System.Type.GetType("System.String")
            oCol5.DataType = System.Type.GetType("System.String")
            oCol6.DataType = System.Type.GetType("System.Int64")

            oCol1.ColumnName = "Subscription"
            oCol2.ColumnName = "DeliveryType"
            oCol3.ColumnName = "ForumType"
            oCol4.ColumnName = "Name"
            oCol5.ColumnName = "Parent"
            oCol6.ColumnName = "ID"

            dtMain.Columns.Add(oCol1)
            dtMain.Columns.Add(oCol2)
            dtMain.Columns.Add(oCol3)
            dtMain.Columns.Add(oCol4)
            dtMain.Columns.Add(oCol5)
            dtMain.Columns.Add(oCol6)

        End Sub

        Private Function GetSubsData() As Data.DataTable
            Dim sb As New Text.StringBuilder
            With sb
                .AppendLine(" WITH ForumNodes(ParentID, Parent, ID, ForumLevel, SortStr) AS ")
                .AppendLine(" (     SELECT ParentID, Parent, ID, 0 AS ForumLevel, ")
                .AppendLine(" Cast ((Replicate('0', 3-Len(cast(ID as nvarchar))) + Cast(ID as nvarchar)) as nvarchar)")
                .AppendLine(" FROM vwDiscussionForums ")
                .AppendLine(" WHERE ParentID IS NULL ")
                .AppendLine(" UNION ALL")
                .AppendLine(" SELECT f.ParentID, f.Parent, f.ID, ForumLevel + 1, ")
                .AppendLine(" Cast((SortStr + Replicate('0', 3-Len(Cast(f.ID AS nvarchar))) + Cast(f.ID AS nvarchar)) as nvarchar) AS SortStr")
                .AppendLine(" FROM vwDiscussionForums f ")
                .AppendLine(" INNER JOIN ForumNodes n ON f.ParentID = n.ID)")
                .AppendLine(" SELECT --n.ID, ")
                .AppendLine(" CASE WHEN s.ForumID is null THEN 0 WHEN s.Status='Inactive' THEN 0 ELSE 1 END AS Subscribed, ")
                .AppendLine(" IsNull(s.DeliveryType, '') AS 'Delvery Type',")
                .AppendLine(" g.Type,g.Parent,g.ID, ")
                .AppendLine(" Name=CASE WHEN n.ForumLevel =0 THEN g.Name + ' Category'  ELSE  Replicate(' - - - ', n.ForumLevel) + g.Name END --, n.ForumLevel, n.SortStr  --use for debugging")
                .AppendLine(" FROM ForumNodes n ")
                .AppendLine(" INNER JOIN vwDiscussionForums g ON n.ID=g.ID ")
                .AppendLine(" LEFT OUTER JOIN vwDiscussionForumSubscriptions s ON s.ForumID=n.ID AND s.WebUserID=")
                .AppendLine(User1.UserID.ToString & " ")
                .AppendLine(" WHERE g.Status='Active' ")
                .AppendLine(" AND g.ID IN (SELECT wg.DiscussionForumID FROM vwdiscussionforumwebgroups wg INNER JOIN vwWebUserGroups gm ON gm.WebGroupID = wg.WebGroupID WHERE gm.WebUserID=")
                .AppendLine(User1.UserID.ToString & ") ")
                .AppendLine(" ORDER BY SortStr")
            End With
            Dim sSQL As String
            sSQL = " SELECT ParentID, Parent, ID, Name,0 as subscribed, 0 AS ForumLevel, " & _
                    " Cast ((Replicate('0', 3-Len(cast(ID as nvarchar))) + Cast(ID as nvarchar)) as nvarchar)" & _
                    " FROM vwDiscussionForums " & _
                    " WHERE ParentID IS NULL "
            Dim oDT As DataTable = DataAction.GetDataTable(sSQL)
            Return oDT

        End Function


        Private Sub PopulateGrid(ByVal lParentID As Long)

            Dim sSQL As String
            Dim dt As DataTable
            Dim i As Integer

            sSQL = "Select * from vwDiscussionForums where parentid=" & lParentID.ToString

            dt = Me.DataAction.GetDataTable(sSQL)

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    Dim dtRow As DataRow = dtMain.NewRow()
                    dtRow.BeginEdit()
                    dtRow("Subscription") = CBool(dt.Rows(i).Item("Subscription"))
                    dtRow("DeliveryType") = dt.Rows(i).Item("DeliveryType").ToString()
                    dtRow("ForumType") = dt.Rows(i).Item("ForumType").ToString()
                    dtRow("Name") = dt.Rows(i).Item("Name").ToString()
                    dtRow("Parent") = dt.Rows(i).Item("Parent").ToString()
                    dtRow("ID") = Convert.ToInt64(dt.Rows(i).Item("ID"))

                    dtRow.EndEdit()

                    dtMain.Rows.Add(dtRow)
                    dtMain.AcceptChanges()

                    PopulateGrid(CLng(dt.Rows(i).Item("ID")))

                Next
            Else
                Exit Sub
            End If

        End Sub


        ''' <summary>
        ''' Amruta:
        ''' Functionality for the save button_click
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
            ' This is the code that will setup/manage subscriptions for discussion forums in the database
            'Suraj issue 14455 4/25/13 ,this method use to apply the odrering of rad grid first column
            AddExpression()
            Dim oGE As AptifyGenericEntityBase

            Dim bOK As Boolean = False
            Dim delivery As String
            Dim ID As Integer
            Dim parentName As String
            Dim forumName As String
            'Added by Sandeep For Issue 14671 on 27/02/2013
            Dim lstSubsciptionValue As List(Of String)
            Dim dicSubscriptionDetails As New Dictionary(Of Integer, List(Of String))
            SaveCheckedValues()

            If CType(ViewState(ATTRIBUTE_CHECKED_SUBSCRIPTION), Dictionary(Of Integer, List(Of String))) IsNot Nothing Then
                dicSubscriptionDetails = CType(ViewState(ATTRIBUTE_CHECKED_SUBSCRIPTION), Dictionary(Of Integer, List(Of String)))
            End If

            Try
                If chkEnableAll.Checked = True Then
                    'Change by Sandeep For Issue 14671 on 27/02/2013
                    'After applying Paging only visible gridrows can be traveseable so change it to traverse in Dictionary Object
                    For Each Subcriptions As KeyValuePair(Of Integer, List(Of String)) In dicSubscriptionDetails
                        ID = Subcriptions.Key
                        lstSubsciptionValue = Subcriptions.Value
                        oGE = AptifyApplication.GetEntityObject("Discussion Forum Subscriptions", GetForumSubscriptionID(ID))
                        If oGE.RecordID > 0 Then    'If record already present
                            If oGE.GetValue("Status").ToString = "Inactive" Then
                                oGE.SetValue("Status", "Active")
                                oGE.SetValue("DateLastSent", Now)
                                If oGE.Save(False) Then
                                    bOK = True
                                End If
                                'Save changes to the entity record
                            End If
                        Else        ' If new record
                            'create new instance of the forum record and then set the values
                            oGE = AptifyApplication.GetEntityObject("Discussion Forum Subscriptions", -1)
                            oGE.SetValue("WebUserID", UserId)
                            delivery = lstSubsciptionValue(0)
                            oGE.SetValue("DeliveryType", delivery)
                            oGE.SetValue("Status", "Active")
                            oGE.SetValue("DateLastSent", Now)

                            If Len(oGE.GetValue("StartDate")) = 0 OrElse _
                               CDate(oGE.GetValue("StartDate")) > Now Then
                                oGE.SetValue("StartDate", Now)
                            End If
                            oGE.SetValue("ForumID", ID)
                            If oGE.Save(False) Then
                                bOK = True
                            End If
                        End If

                    Next

                Else
                    For Each Subcriptions As KeyValuePair(Of Integer, List(Of String)) In dicSubscriptionDetails
                        ID = Subcriptions.Key
                        lstSubsciptionValue = Subcriptions.Value
                        oGE = AptifyApplication.GetEntityObject("Discussion Forum Subscriptions", GetForumSubscriptionID(ID))
                        If CBool(lstSubsciptionValue(1)) = True Then
                            If oGE.RecordID <= 0 Then   'If Record not already present
                                'create new instance of the forum record and then set the values
                                'oGE = AptifyApplication.GetEntityObject("Discussion Forum Subscriptions", -1)

                                oGE.SetValue("WebUserID", UserId)
                                oGE.SetValue("StartDate", Now)
                                oGE.SetValue("ForumID", ID)

                                'parentName = (gridRow.Cells(4).Text).ToString
                                'If Not parentName = "&nbsp;" AndAlso Not parentName = "" Then
                                '    oGE.SetValue("SubscriptionScope", "Thread")
                                'Else
                                '    oGE.SetValue("SubscriptionScope", "Forum")
                                'End If

                            End If
                            'oGE.SetValue("EndDate", txtboxendDate.Text)
                            delivery = lstSubsciptionValue(0)
                            oGE.SetValue("DeliveryType", delivery)
                            oGE.SetValue("Status", "Active")
                            oGE.SetValue("DateLastSent", Now)
                            If oGE.Save(False) Then
                                bOK = True
                            End If
                        Else
                            If oGE.RecordID > 0 Then    'record already present
                                ' we have an existing record, disable it
                                'oGE.SetValue("EndDate", txtboxendDate.Text)
                                oGE.SetValue("Status", "Inactive")
                                If oGE.Save(False) Then
                                    bOK = True
                                End If
                                'Save changes to the entity record
                            End If
                        End If

                    Next

                End If

                If bOK Then
                    lblResults.Text = "Your selections are now saved... Return to"
                    lnkForumsHome.Visible = True
                    'RedirectUsingPropertyValues()
                Else
                    lblResults.Text = "Your selections failed to save"
                End If
                'Response.Redirect("SubscriptionsList.aspx")

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Overridable Function GetForumSubscriptionID(ByVal id As Integer) As Long
            Dim sSQL As String, lValue As Object
            Try
                'Suvarna Issue 12351 12/01/2011 Commented and added for Dynamic DB Name chage
                'sSQL = "SELECT ID FROM APTIFY..vwDiscussionForumSubscriptions WHERE WebUserID=" & UserId & _
                sSQL = "SELECT ID FROM " & AptifyApplication.GetEntityBaseDatabase("DiscussionForumSubscriptions") & "..vwDiscussionForumSubscriptions WHERE WebUserID=" & UserId & _
                       " AND ForumID=" & id.ToString

                lValue = DataAction.ExecuteScalar(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                If Not lValue Is Nothing AndAlso IsNumeric(lValue) Then
                    Return CLng(lValue)
                Else
                    Return -1
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Function

        Protected Sub grdForumSubscriptions_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdForumSubscriptions.ItemCreated
            Dim chkSubscription As CheckBox = DirectCast(e.Item.FindControl("chkSubscription"), CheckBox)
            Dim ddlDeliveryType As DropDownList = DirectCast(e.Item.FindControl("ddlDeliveryType"), DropDownList)
            Dim dicSubscriptionDetails As New Dictionary(Of Integer, List(Of String))
            Dim SubscriptionID As Integer
            Dim dataItem As DataRowView
            Dim lstSubValue As List(Of String)
            If chkSubscription IsNot Nothing Then
                If ViewState(ATTRIBUTE_CHECKED_SUBSCRIPTION) IsNot Nothing Then 'Added by Sandeep For Issue 14671 on 27/02/2013
                    dicSubscriptionDetails = DirectCast(ViewState(ATTRIBUTE_CHECKED_SUBSCRIPTION), Dictionary(Of Integer, List(Of String)))
                    dataItem = DirectCast(e.Item.DataItem, System.Data.DataRowView)
                    If dataItem IsNot Nothing Then
                        SubscriptionID = CInt(dataItem("ID"))
                        If dicSubscriptionDetails.ContainsKey(SubscriptionID) Then
                            lstSubValue = dicSubscriptionDetails(SubscriptionID)
                            chkSubscription.Checked = CBool(lstSubValue(1))
                            ddlDeliveryType.SelectedValue = lstSubValue(0)
                        End If
                    End If
                End If
                If chkEnableAll.Checked = True Then
                    chkSubscription.Checked = True
                    chkSubscription.Enabled = False
                End If
            End If
        End Sub

        ''' <summary>
        ''' Amruta:
        ''' Handler for binding the image field's image url property depending upon the Parent field
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdForumSubscriptions_RowDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdForumSubscriptions.ItemDataBound
            Try
                If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
                    'determine the image url value of the Image field
                    Dim parentType As String = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "Parent"))
                    If Not parentType.Length = 0 Then
                        CType(e.Item.FindControl("imgForumType"), Image).ImageUrl = HasParentImage
                    Else
                        CType(e.Item.FindControl("imgForumType"), Image).ImageUrl = NoParentImage
                    End If
                    If Not IsPostBack Then 'Change by Sandeep For Issue 14671 on 27/02/2013 Only first time reqquired
                        Dim IsSubscribed As Boolean = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "Subscribed"))
                        CType(e.Item.FindControl("chkSubscription"), WebControls.CheckBox).Checked = IsSubscribed
                        Dim lblDType As Label = CType(e.Item.FindControl("lblDeliveryType"), Label)
                        Dim ddlDT As DropDownList = CType(e.Item.FindControl("ddlDeliveryType"), DropDownList)
                        ddlDT.SelectedValue = lblDType.Text
                    End If
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub chkEnableAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkEnableAll.CheckedChanged
            Try
                'Suraj issue 14455 4/25/13 ,this method use to apply the odrering of rad grid first column
                AddExpression()
                Dim checkboxMain As CheckBox = CType(sender, CheckBox)
                Dim dicSubscriptionDetails As New Dictionary(Of Integer, List(Of String))
                If checkboxMain.Checked = True Then
                    For Each gridRow As Telerik.Web.UI.GridDataItem In grdForumSubscriptions.Items
                        Dim chechboxRow As CheckBox
                        chechboxRow = CType(gridRow.FindControl("chkSubscription"), CheckBox)
                        chechboxRow.Checked = True
                        chechboxRow.Enabled = False
                    Next
                    dicSubscriptionDetails = OnchkEnableAll("True") 'Added by Sandeep For Issue 14671 on 27/02/2013
                Else
                    For Each gridRow As Telerik.Web.UI.GridDataItem In grdForumSubscriptions.Items
                        Dim chechboxRow As CheckBox
                        chechboxRow = CType(gridRow.FindControl("chkSubscription"), CheckBox)
                        chechboxRow.Checked = False
                        chechboxRow.Enabled = True
                    Next
                    dicSubscriptionDetails = OnchkEnableAll("False")
                End If
                If dicSubscriptionDetails IsNot Nothing AndAlso dicSubscriptionDetails.Count > 0 Then
                    ViewState(ATTRIBUTE_CHECKED_SUBSCRIPTION) = dicSubscriptionDetails
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        'Added By Sandeep for issue 14671 on 26/02/2013
        'Saves values of visible rows.
        Private Sub SaveCheckedValues()
            Dim dicSubscriptionDetails As New Dictionary(Of Integer, List(Of String))
            Dim index As Integer = -1
            Dim result As Boolean
            Dim lstSubValueList As List(Of String)
            Dim ddlDeliveryType As DropDownList
            For Each item As Telerik.Web.UI.GridDataItem In grdForumSubscriptions.MasterTableView.Items
                index = CInt(DirectCast(item.FindControl("lblforumID"), Label).Text)
                result = DirectCast(item.FindControl("chkSubscription"), CheckBox).Checked
                ddlDeliveryType = DirectCast(item.FindControl("ddlDeliveryType"), DropDownList)
                If ViewState(ATTRIBUTE_CHECKED_SUBSCRIPTION) IsNot Nothing Then
                    dicSubscriptionDetails = DirectCast(ViewState(ATTRIBUTE_CHECKED_SUBSCRIPTION), Dictionary(Of Integer, List(Of String)))
                End If
                If dicSubscriptionDetails.ContainsKey(index) Then
                    dicSubscriptionDetails.Remove(index)
                End If
                lstSubValueList = New List(Of String)
                lstSubValueList.Add(ddlDeliveryType.SelectedValue)
                lstSubValueList.Add(CStr(result))
                dicSubscriptionDetails.Add(index, lstSubValueList)
            Next
            If dicSubscriptionDetails IsNot Nothing AndAlso dicSubscriptionDetails.Count > 0 Then
                ViewState(ATTRIBUTE_CHECKED_SUBSCRIPTION) = dicSubscriptionDetails
            End If
        End Sub
        'Added by Sandeep For Issue 14671 on 27/02/2013
        Private Function OnchkEnableAll(ByVal strStatus As String) As Dictionary(Of Integer, List(Of String))
            Dim dicSubscriptionDetails As New Dictionary(Of Integer, List(Of String))
            Dim lstSubValueList As List(Of String)
            Dim dtSuscription As DataTable
            If TryCast(ViewState(ATTRIBUTE_FORUMSUBSCRIPTION_VIEWSTATE), DataTable) IsNot Nothing Then
                dtSuscription = TryCast(ViewState(ATTRIBUTE_FORUMSUBSCRIPTION_VIEWSTATE), DataTable)
            End If
            If dtSuscription IsNot Nothing Then
                For Each dtrow As DataRow In dtSuscription.Rows
                    lstSubValueList = New List(Of String)
                    If IsDBNull(dtrow("DeliveryType")) Then
                        dtrow("DeliveryType") = "Daily Digest"
                    End If
                    lstSubValueList.Add(CStr(dtrow("DeliveryType")))
                    lstSubValueList.Add(strStatus)
                    dicSubscriptionDetails.Add(CInt(dtrow("ID")), lstSubValueList)
                Next
                dtSuscription = Nothing
            End If
            Return dicSubscriptionDetails
        End Function

        'Navin Prasad Issue 9238
        Protected Sub grdForumSubscriptions_Sorting(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridSortCommandEventArgs) Handles grdForumSubscriptions.SortCommand
            Try
                FillGrid()
                Dim dv As DataView = DirectCast(Session("SubGV"), DataView)
                If dv IsNot Nothing Then
                    If dv.Table.Rows.Count <> 0 Then
                        If DirectCast(Session("sortExpression"), String) IsNot Nothing Then
                            Dim sortData As String() = Session("sortExpression").ToString().Trim().Split(" "c)
                            If e.SortExpression = sortData(0) Then
                                If sortData(1) = "ASC" Then
                                    dv.Sort = e.SortExpression + " " & "DESC"
                                    Session("sortExpression") = e.SortExpression + " " & "DESC"
                                Else
                                    dv.Sort = e.SortExpression + " " & "ASC"
                                    Session("sortExpression") = e.SortExpression + " " & "ASC"
                                End If
                            Else
                                dv.Sort = e.SortExpression + " " & "ASC"
                                Session("sortExpression") = e.SortExpression + " " & "ASC"
                            End If
                        Else
                            dv.Sort = e.SortExpression + " " & "ASC"
                            Session("sortExpression") = e.SortExpression + " " & "ASC"
                        End If
                        Session("SubGV") = dv
                        grdForumSubscriptions.DataSource = dv
                        grdForumSubscriptions.DataBind()
                    End If
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Overridable Function ConvertSortDirectionToSql(ByVal sDirection As SortDirection) As String
            Dim newSortDirection As String = String.Empty
            Select Case sDirection
                Case SortDirection.Ascending
                    newSortDirection = "ASC"
                Case SortDirection.Descending
                    newSortDirection = "DESC"
            End Select
            Return newSortDirection
        End Function

        Protected Sub grdForumSubscriptions_PageIndexChanging(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageChangedEventArgs) Handles grdForumSubscriptions.PageIndexChanged
            'FillGrid()
            ''grdForumSubscriptions.PageIndex = e.NewPageIndex
            SaveCheckedValues() 'Added by Sandeep For Issue 14671 on 27/02/2013
        End Sub
        Protected Sub grdForumSubscriptions_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdForumSubscriptions.NeedDataSource
            If User1.UserID > 0 Then
                FillGrid()
            End If
        End Sub
        'Suraj Issue 14455 4/25/13 ,if the grid load first time By default the sorting will be Ascending for column Forum 
        Private Sub AddExpression()
            Dim expression1 As New GridSortExpression
            expression1.FieldName = "Name"
            expression1.SetSortOrder("Ascending")
            grdForumSubscriptions.MasterTableView.SortExpressions.AddSortExpression(expression1)
        End Sub
    End Class
End Namespace
