'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

#Region "Namespace"
Imports Aptify.Framework.DataServices
Imports System.IO
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports System.Data
Imports Telerik.Web.UI
Imports System.Web
Imports Aptify.Framework.Application
Imports Aptify.Framework.BusinessLogic.Security
Imports Aptify.Applications.OrderEntry
#End Region

Namespace Aptify.Framework.Web.eBusiness
    Partial Class DisableAutoRenewalMemberships
        Inherits BaseUserControlAdvanced
        Protected Const ATTRIBUTE_LOGIN_PAGE As String = "LoginPage" 'Added by Sandeep for Issue 15051 on 12/03/2013
#Region "Public Properties"
        'Added by Sandeep for Issue 15051 on 12/03/2013
        Public Overridable Property LoginPage() As String
            Get
                If Not ViewState(ATTRIBUTE_LOGIN_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_LOGIN_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_LOGIN_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
#End Region

#Region "Page Events"

#End Region


#Region "Methods and Functions"
        Private Shared sProductsBaseView As String = ""
        Private Shared sMembertypesBaseView As String = ""
        Private Shared sSubscriptionsBaseView As String = ""

        Private Shared Function ProductsBaseView(ByVal Application As AptifyApplication) As String
            If sProductsBaseView = "" Then
                sProductsBaseView = Application.GetEntityBaseView("Products")
            End If
            Return sProductsBaseView
        End Function

        Private Shared Function SubscriptionsBaseView(ByVal Application As AptifyApplication) As String

            If sSubscriptionsBaseView = "" Then
                sSubscriptionsBaseView = Application.GetEntityBaseView("Subscriptions")
            End If
            Return sSubscriptionsBaseView

        End Function
        Private Shared Function MembertypesBaseView(ByVal Application As AptifyApplication) As String
            If sMembertypesBaseView = "" Then
                sMembertypesBaseView = Application.GetEntityBaseView("Member Types")
            End If
            Return sMembertypesBaseView
        End Function
        Protected Overridable Function ShowMembershipProduct() As DataTable
            Dim dt_m As DataTable, sSQL As String
            Try

                sSQL = "Select p.ID,p.WebName,p.IsSubscription from " & Database & ".." & ProductsBaseView(Me.AptifyApplication) & " p inner join " & Database & ".." & MembertypesBaseView(Me.AptifyApplication) & " mn p.MemberTypeID= m.ID where p.DefaultDuesProduct=1 and p.WebEnabled=1 and  p.IsSold=1 AND p.TopLevelItem=1 and (QuantityAvailable>=1 or RequireInventory=0) and (isnull(p.DateAvailable,getdate()) <=GETDATE() and isnull(p.AvailableUntil,GETDATE()) >=GETDATE()) and m.DefaultType='persons'"
                dt_m = Me.DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                If Not dt_m Is Nothing AndAlso dt_m.Rows.Count > 0 Then
                    Me.grdAutoRenewalMemberships.DataSource = dt_m
                    Return dt_m
                Else
                    Return Nothing
                End If
            Catch ex As Exception
                Return Nothing
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Function
        Public Function GetSubscriptionsByCompanyID() As DataTable
            Try
                Dim sSQL As String
                Dim dt As DataTable
                sSQL = "SELECT * FROM " & _
                      Database & ".." & SubscriptionsBaseView(Me.AptifyApplication) & " where SubscriberCompanyID=" & Convert.ToString(User1.CompanyID)
                dt = Me.DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                Return dt
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return Nothing
            End Try
        End Function

        Protected Sub ToggleRowSelection(ByVal sender As Object, ByVal e As EventArgs)
            Try
                TryCast(TryCast(sender, CheckBox).NamingContainer, GridItem).Selected = TryCast(sender, CheckBox).Checked
                Dim checkHeader As Boolean = True
                For Each dataItem As GridDataItem In grdAutoRenewalMemberships.MasterTableView.Items
                    If Not TryCast(dataItem.FindControl("chkRenewal"), CheckBox).Checked Then
                        checkHeader = False
                        Exit For
                    Else
                        Dim selectedItem As GridDataItem = DirectCast(grdAutoRenewalMemberships.SelectedItems(0), GridDataItem)
                        Dim value As String = selectedItem("ProductID").Text
                        lblSelections.Text = value
                        lblSelections.Visible = True
                    End If
                Next
                Dim headerItem As GridHeaderItem = TryCast(grdAutoRenewalMemberships.MasterTableView.GetItems(GridItemType.Header)(0), GridHeaderItem)
                TryCast(headerItem.FindControl("headerChkbox"), CheckBox).Checked = checkHeader
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Protected Sub ToggleSelectedState(ByVal sender As Object, ByVal e As EventArgs)
            Try

                Dim headerCheckBox As CheckBox = TryCast(sender, CheckBox)
                For Each dataItem As GridDataItem In grdAutoRenewalMemberships.MasterTableView.Items
                    TryCast(dataItem.FindControl("chkRenewal"), CheckBox).Checked = headerCheckBox.Checked
                    dataItem.Selected = headerCheckBox.Checked
                    If headerCheckBox.Checked = True Then
                        Dim selectedItem As GridDataItem = DirectCast(grdAutoRenewalMemberships.SelectedItems(0), GridDataItem)
                        Dim value As String = Convert.ToString(dataItem.FindControl("ProductID"))
                        lblSelections.Text = value
                        lblSelections.Visible = True
                    End If
                Next
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub


#End Region

        Protected Sub grdAutoRenewalMemberships_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles grdAutoRenewalMemberships.ItemDataBound

            ' If "Flag" Is Session("Flag") Then
            Dim ddlAutoRenewMem As New DropDownList
            Dim lblAutoRenewMem As New Label
            Dim dt As DataTable
            dt = GetSubscriptionsByCompanyID()
            Dim sSubbool As String
            For Each dataItem As GridDataItem In grdAutoRenewalMemberships.Items
                Dim chkperson As CheckBox = CType(dataItem.FindControl("chkperson"), CheckBox)
                ddlAutoRenewMem = DirectCast(dataItem.FindControl("ddlAutoRenew"), DropDownList)
                lblAutoRenewMem = DirectCast(dataItem.FindControl("lblAutoRenew"), Label)
                sSubbool = lblAutoRenewMem.Text.Trim
                If sSubbool = "False" Then
                    ddlAutoRenewMem.SelectedValue = CStr(0)
                Else
                    ddlAutoRenewMem.SelectedValue = CStr(1)
                End If
            Next
            Try
                Dim imagememberid As New Image
                imagememberid = CType(e.Item.FindControl("RadBinaryImgPhoto"), Image)
                If Not (DataBinder.Eval(e.Item.DataItem, "photo")) Is Nothing Then
                    If IsDBNull(DataBinder.Eval(e.Item.DataItem, "photo")) Then
                        imagememberid.ImageUrl = "~/Images/blankphoto.gif"
                    Else
                        If (DirectCast(DataBinder.Eval(e.Item.DataItem, "photo"), Byte()).Length() > 0) Then
                            Dim base64String As String = Convert.ToBase64String(DirectCast(DataBinder.Eval(e.Item.DataItem, "photo"), Byte()), 0, DirectCast(DataBinder.Eval(e.Item.DataItem, "photo"), Byte()).Length())
                            imagememberid.ImageUrl = "data:image/png;base64," & base64String
                        Else
                            imagememberid.ImageUrl = "~/Images/blankphoto.gif"
                        End If
                    End If
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

     
        Protected Sub grdAutoRenewalMemberships_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdAutoRenewalMemberships.NeedDataSource
            Try
                Dim dt As DataTable
                If Not e.IsFromDetailTable Then
                    dt = GetSubscriptionsByCompanyID()
                    If dt.Rows.Count > 0 Then
                        grdAutoRenewalMemberships.DataSource = dt
                    End If
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub grdAutoRenewalMemberships_PageIndexChanged(sender As Object, e As Telerik.Web.UI.GridPageChangedEventArgs) Handles grdAutoRenewalMemberships.PageIndexChanged
            Dim dt As DataTable
            grdAutoRenewalMemberships.CurrentPageIndex = e.NewPageIndex
            dt = GetSubscriptionsByCompanyID()
            If dt.Rows.Count > 0 Then
                grdAutoRenewalMemberships.DataSource = dt
            End If
        End Sub

        Protected Sub btnUpdateMemberships_Click(sender As Object, e As System.EventArgs) Handles btnUpdateMemberships.Click
            Dim bSubscriptionSelected As Boolean = False
            Dim chkRenewal As New CheckBox
            Dim sSubscriptionsID As Long
            Dim iIDBool As Boolean
            Dim IsSuccess As Boolean
            Dim oOrgOMD As AptifyGenericEntityBase
            Dim ddlAutoRenewMem As New DropDownList
            For Each dataItem As GridDataItem In grdAutoRenewalMemberships.MasterTableView.Items
                chkRenewal = CType(dataItem.FindControl("chkRenewal"), CheckBox)
                If (chkRenewal.Checked) Then
                    bSubscriptionSelected = True
                    Exit For
                End If
            Next

            If Not bSubscriptionSelected Then
                'Nothing selected - Nothing to do.
                lblmsg.Text = "Please select one or more memberships to enable/disable auto renewal and click " & lblSelections.Text & "."
                lblmsg.ForeColor = System.Drawing.Color.Crimson
                lblmsg.Visible = True
            Else
                lblmsg.Text = ""
                lblSelections.Text = ""
                lblmsg.ForeColor = Nothing
                lblmsg.Visible = False

                For Each dataItem As GridDataItem In grdAutoRenewalMemberships.Items
                    chkRenewal = CType(dataItem.FindControl("chkRenewal"), CheckBox)
                    If (chkRenewal.Checked) Then
                        sSubscriptionsID = CLng(CType(dataItem.FindControl("lblSubscriptionsID"), Label).Text)
                        ddlAutoRenewMem = DirectCast(dataItem.FindControl("ddlAutoRenew"), DropDownList)
                        If ddlAutoRenewMem IsNot Nothing AndAlso ddlAutoRenewMem.Text = "1" Then
                            iIDBool = True
                        Else
                            iIDBool = False
                        End If
                        IsSuccess = UpdateAutoRenewMemberships(sSubscriptionsID, iIDBool)
                        If IsSuccess = False Then
                            Exit For
                        End If
                    End If
                Next
                If IsSuccess = True Then
                    grdAutoRenewalMemberships.Rebind()
                    lblSelections.Font.Bold = True
                    lblSelections.Visible = True
                    Me.lblSelections.Text = "Memberships have been Updated successfully!."
                    UserListDialog.VisibleOnPageLoad = True
                Else

                    lblSelections.Font.Bold = True
                    lblSelections.Visible = True
                    Me.lblSelections.Text = "Memberships have been failed to update!."
                    UserListDialog.VisibleOnPageLoad = False
                End If
            End If

        End Sub

        Private Function UpdateAutoRenewMemberships(ByVal iId As Long, ByVal ibool As Boolean) As Boolean
            Dim oOrgOMD As AptifyGenericEntityBase
            If iId > 0 Then
                oOrgOMD = Me.AptifyApplication.GetEntityObject("Subscriptions", iId)
                Dim sErrorString As String = ""
                With oOrgOMD
                    .SetValue("AutoRenew", ibool)
                    If Not .Save(sErrorString) Then
                        lblSelections.ForeColor = Drawing.Color.Red
                        lblSelections.Font.Bold = True
                        lblSelections.Visible = True
                        Me.lblSelections.Text = "There was an error submitting this form. Please try again later." + sErrorString
                        Return False
                    End If
                End With
            End If
            Return True
        End Function

        Private Function Nullable() As Object
            Throw New NotImplementedException
        End Function

        Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
            Session("Flag") = ""
            If Not IsPostBack Then
                Session("Flag") = "Flag"
            End If
            'Added by Sandeep for Issue 15051 on 12/03/2013
            If String.IsNullOrEmpty(LoginPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                LoginPage = Me.GetLinkValueFromXML(ATTRIBUTE_LOGIN_PAGE)
            End If
            If User1.UserID < 0 Then
                Response.Redirect(LoginPage) 'Added by Sandeep for Issue 15051 on 12/03/2013
            End If
        End Sub

        Protected Sub grdAutoRenewalMemberships_PageSizeChanged(sender As Object, e As Telerik.Web.UI.GridPageSizeChangedEventArgs) Handles grdAutoRenewalMemberships.PageSizeChanged

        End Sub

        Protected Sub btnok_Click(sender As Object, e As System.EventArgs) Handles btnok.Click
            UserListDialog.VisibleOnPageLoad = False
        End Sub
    End Class
End Namespace

