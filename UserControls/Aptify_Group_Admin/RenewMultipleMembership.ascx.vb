﻿'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports System.Data
Imports Aptify.Framework.Web.eBusiness
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Applications.OrderEntry
Imports Telerik.Web.UI
Imports Aptify.Applications.Accounting

Namespace Aptify.Framework.Web.eBusiness
    Partial Class RenewMultipleMembership
        Inherits BaseUserControlAdvanced
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "RenewMultipleMembership"

        Protected Const ATTRIBUTE_PROFILE_THUMBNAIL_WIDTH As String = "ProfileThumbNailWidth"
        Protected Const ATTRIBUTE_PROFILE_THUMBNAIL_HEIGHT As String = "ProfileThumbNailHeight"

        Protected Const ATTRIBUTE_PERSON_BLANK_IMG As String = "RadBlankImage"
        Protected Const ATTRIBUTE_ACTIVE_IMG As String = "ActiveImage"
        Protected Const ATTRIBUTE_EXPIRING_SOON_IMG As String = "ExpiringSoonImage"
        Protected Const ATTRIBUTE_EXPIRED_IMG As String = "ExpiredImage"
        Protected Const ATTRIBUTE_DT_MEMBERSHIP As String = "dtMembership"
        Protected Const ATTRIBUTE_SUSCRIPTION_ORDER As String = "SubscriptionOrder"
        Protected Const ATTRIBUTE_SELECTED_ROWS As String = "SelectedRows"
        Protected Const ATTRIBUTE_LOGIN_PAGE As String = "LoginPage" 'Added by Sandeep for Issue 15051 on 12/03/2013

        ''' <summary>
        ''' ProfileThumbNailWidth
        ''' </summary>
        Public Overridable ReadOnly Property ProfileThumbNailWidth() As Integer
            Get
                Try
                    If Not String.IsNullOrEmpty(Me.GetGlobalAttributeValue(ATTRIBUTE_PROFILE_THUMBNAIL_WIDTH)) Then
                        If Not IsNumeric(Me.GetGlobalAttributeValue(ATTRIBUTE_PROFILE_THUMBNAIL_WIDTH)) Then
                            Throw New Exception("Incorrect entry for <Global>...<" & ATTRIBUTE_PROFILE_THUMBNAIL_WIDTH & ">, a numeric value is required. " & _
                                                "Please confirm the entry is correctly input in the 'Aptify_UC_Navigation.config' file.")
                        Else
                            Return CInt(Me.GetGlobalAttributeValue(ATTRIBUTE_PROFILE_THUMBNAIL_WIDTH))
                        End If
                    Else
                        Return 0
                    End If
                Catch ex As Exception
                    Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                    Return 0
                End Try
            End Get
        End Property


        ''' <summary>
        ''' ProfileThumbNailHeight
        ''' </summary>
        Public Overridable ReadOnly Property ProfileThumbNailHeight() As Integer
            Get
                Try
                    If Not String.IsNullOrEmpty(Me.GetGlobalAttributeValue(ATTRIBUTE_PROFILE_THUMBNAIL_HEIGHT)) Then
                        If Not IsNumeric(Me.GetGlobalAttributeValue(ATTRIBUTE_PROFILE_THUMBNAIL_HEIGHT)) Then
                            Throw New Exception("Incorrect entry for <Global>...<" & ATTRIBUTE_PROFILE_THUMBNAIL_HEIGHT & ">, a numeric value is required. " & _
                                                "Please confirm the entry is correctly input in the 'Aptify_UC_Navigation.config' file.")
                        Else
                            Return CInt(Me.GetGlobalAttributeValue(ATTRIBUTE_PROFILE_THUMBNAIL_HEIGHT))
                        End If
                    Else
                        Return 0
                    End If
                Catch ex As Exception
                    Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                    Return 0
                End Try
            End Get
        End Property

        ''' <summary>
        ''' BlankImage
        ''' </summary>
        Public Property RadBlankImage() As String
            Get
                If ViewState.Item("RadBlankImage") IsNot Nothing Then
                    Return ViewState.Item("RadBlankImage").ToString()
                Else
                    Return String.Empty
                End If

            End Get
            Set(ByVal value As String)
                ViewState.Item("RadBlankImage") = value
            End Set
        End Property

        ''' <summary>
        ''' ActiveImage
        ''' </summary>
        Public Property ActiveImage() As String
            Get
                If ViewState.Item("ActiveImage") IsNot Nothing Then
                    Return ViewState.Item("ActiveImage").ToString()
                Else
                    Return ""
                End If

            End Get
            Set(ByVal value As String)
                ViewState.Item("ActiveImage") = value
            End Set
        End Property

        ''' <summary>
        ''' ExpiringSoonImage
        ''' </summary>
        Public Property ExpiringSoonImage() As String
            Get
                If ViewState.Item("ExpiringSoonImage") IsNot Nothing Then
                    Return ViewState.Item("ExpiringSoonImage").ToString()
                Else
                    Return ""
                End If

            End Get
            Set(ByVal value As String)
                ViewState.Item("ExpiringSoonImage") = value
            End Set
        End Property

        ''' <summary>
        ''' ExpiredImage
        ''' </summary>
        Public Property ExpiredImage() As String
            Get
                If ViewState.Item("ExpiredImage") IsNot Nothing Then
                    Return ViewState.Item("ExpiredImage").ToString()
                Else
                    Return ""
                End If

            End Get
            Set(ByVal value As String)
                ViewState.Item("ExpiredImage") = value
            End Set
        End Property
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

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            SetProperties()
            If Not IsPostBack Then
                'Suraj issue 15287 4/5/13 ,this method use to apply the odrering of rad grid first column
                AddExpressionMultipleMembership()
                LoadMemberships(True)
                CreditCard.LoadCreditCardInfo()
                ShowBillMeLater()
                ClearCreditCardControl()
                chkMakePayment.Checked = False
                radWindowPayment.VisibleOnPageLoad = False
            End If
            CreditCard.SetchkSaveforFutureUse = False
            If User1.UserID < 0 Then
                Response.Redirect(LoginPage) 'Added by Sandeep for Issue 15051 on 12/03/2013
            End If
            upnlMain.Update()
        End Sub
#Region "Private Methods"
        ''procedure reset all the fields of credit card
        Private Sub ClearCreditCardControl()
            CreditCard.CCNumber = ""
            CreditCard.CCSecurityNumber = ""
            CreditCard.CCExpireDate = CStr(Now.Date)
            CreditCard.PONumber = ""
            CreditCard.BillMeLaterChecked = False
            CreditCard.SelectCardType("")
            CreditCard.SetchkSaveforFutureUse = False
        End Sub
        ''Loads Membership Details
        Protected Overridable Sub LoadMemberships(ByVal bindGrid As Boolean)

            Dim sSQL As String
            Dim params1(1) As IDataParameter
            Dim DT As Data.DataTable
            Try
                lblError.Text = ""
                sSQL = Database & ".." & "spGetSubscriptionsforAdmin"

                params1(0) = Me.DataAction.GetDataParameter("@CompanyID", SqlDbType.Int, User1.CompanyID)
                params1(1) = Me.DataAction.GetDataParameter("@DefaultDuesProduct", SqlDbType.Int, 1)

                DT = Me.DataAction.GetDataTableParametrized(sSQL, CommandType.StoredProcedure, params1)
                Dim dvMembershipRenewal As New DataView

                If DT IsNot Nothing Then
                    dvMembershipRenewal = DT.DefaultView
                    dvMembershipRenewal.Sort = "ExpiryDate ASC, Recipient ASC, ProductID ASC"
                    ViewState(ATTRIBUTE_DT_MEMBERSHIP) = DT
                End If


                radGridMembership.DataSource = dvMembershipRenewal

                If bindGrid = True Then
                    radGridMembership.DataBind()
                End If

                If DT Is Nothing OrElse DT.Rows.Count = 0 Then
                    btnRenew.Visible = False
                    btnRenewalOff.Visible = False
                    btnRenewalOn.Visible = False
                    'Suraj issue 15287 4/5/13 , remove the  'radGridMembership.DataSource = Nothing and used the following  because we are using the   "NoRecordsTemplate" for  display no record found msg
                    radGridMembership.DataSource = dvMembershipRenewal
                    radGridMembership.DataBind()
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        ''Procedured used to Turn Membership On
        Protected Overridable Sub SetAutoRenewOn(ByVal dtSelectedRecords As DataTable)
            Try
                Dim oGE As Aptify.Applications.Subscriptions.SubscriptionEntity

                Dim OnOff As String
                Dim SubscriptionID As String
                If dtSelectedRecords.Rows.Count > 0 Then
                    For Each row As DataRow In dtSelectedRecords.Rows
                        OnOff = CType(row.Item("AutoRenew"), String)
                        SubscriptionID = CType(row.Item("SubscriptionID"), String)
                        If OnOff = "Off" Then
                            oGE = DirectCast(Me.AptifyApplication.GetEntityObject("Subscriptions", CLng(SubscriptionID)), Aptify.Applications.Subscriptions.SubscriptionEntity)
                            If Not oGE Is Nothing Then
                                If oGE.AutoRenewStandingOrderID = -1 Then
                                    CreateStandingOrder(oGE)
                                Else
                                    UpdateStandingOrder(oGE)
                                End If
                                If Not oGE.Save(False) Then
                                    ExceptionManagement.ExceptionManager.Publish(New Exception(oGE.LastError))
                                End If
                            End If

                        End If
                    Next
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        ''Procedure Create standing order if not exist while turning on Membership.
        Protected Overridable Sub CreateStandingOrder(ByRef oGE As Aptify.Applications.Subscriptions.SubscriptionEntity)
            Try
                Dim oGEStandingOrder As AptifyGenericEntityBase
                oGEStandingOrder = Me.AptifyApplication.GetEntityObject("Standing Orders", -1)

                With oGEStandingOrder

                    .SetValue("ShipToID", oGE.SubscriberID)
                    .SetValue("ShipToCompanyID", oGE.SubscriberCompanyID)
                    .SetValue("BillToID", User1.PersonID)
                    .SetValue("BillToCompanyID", User1.CompanyID)
                    .SetValue("DateCreated", Now)
                    .SetValue("FulfillmentStartDate", Now.Date)
                    .SetValue("Status", CStr(AptifyApplication.Entity("Standing Orders").EntityMetaData.Fields("Status").DefaultValue))
                    .SetValue("PurchaseType", CStr(oGE.GetValue("PurchaseType")))
                    .SetValue("CurrencyTypeID", User1.PreferredCurrencyTypeID)
                    .SetValue("Type", "Frequency")
                    .SetValue("Frequency", "Annually")
                    .SetValue("EmployeeID", EBusinessGlobal.WebEmployeeID(Page.Application))
                    .SetValue("ShipTypeID", 1)
                    .SetValue("OrderSourceID", 1)
                    .SetValue("Comments", "Generated by Subscription Save on " & Format(Now, "MM/dd/yyyy hh:mm:ss tt"))
                    .SetValue("OrderLevelID", 1)
                    With .SubTypes("StandingOrSchedule").Add
                        .SetValue("ScheduledDate", oGE.EndDate)
                    End With
                    With .SubTypes("StandingOrProd").Add
                        .SetValue("ProductID", oGE.ProductID)
                        .SetValue("Quantity", oGE.Quantity)
                        .SetValue("SubscriberID", oGE.RecipientID)
                        .SetValue("PriceType", CStr(AptifyApplication.Entity("StandingOrProd").EntityMetaData.Fields("PriceType").DefaultValue))
                    End With
                End With
                If oGEStandingOrder.Fields("PaymentInformationID").EmbeddedObject IsNot Nothing Then
                    With oGEStandingOrder.Fields("PaymentInformationID").EmbeddedObject
                        If chkMakePayment.Checked Then
                            If CreditCard.BillMeLaterChecked Then
                                .SetValue("PONumber", CreditCard.PONumber)
                                .SetValue("PaymentTypeID", CreditCard.PaymentTypeID)
                            Else
                                .SetValue("PaymentTypeID", CreditCard.PaymentTypeID)
                                .SetValue("CCAccountNumber", CreditCard.CCNumber)
                                .SetValue("CCExpireDate", CreditCard.CCExpireDate)
                                'Suraj Issue 16258 , 5/16/13 , set the security  CreditCardSecurityNumber to payment info property 
                                Dim oOrderPayInfo As PaymentInformation = DirectCast(oGEStandingOrder.Fields("PaymentInformationID").EmbeddedObject, PaymentInformation)
                                oOrderPayInfo.CreditCardSecurityNumber = CreditCard.CCSecurityNumber
                                'Anil B change for 10737 on 13/03/2013
                                'Add condition if payment type is transaction
                                If CreditCard.CCNumber = "-Ref Transaction-" Then
                                    .SetValue("ReferenceTransactionNumber", CreditCard.ReferenceTransactionNumber)
                                    .SetValue("ReferenceExpiration", CreditCard.ReferenceExpiration)
                                    'Anil B for Issue 16214 on 09/05/2013
                                    'Set CCPartial Number
                                    .SetValue("CCPartial", CreditCard.CCPartial)
                                End If
                            End If
                        Else
                            .SetValue("PONumber", CreditCard.PONumber)
                            .SetValue("PaymentTypeID", CreditCard.PaymentTypeID)
                        End If

                    End With
                End If
                If oGEStandingOrder.Save(False) Then
                    oGE.SetValue("AutoRenewStandingOrderID", oGEStandingOrder.RecordID)
                    oGE.SetValue("AutoRenew", True)
                Else
                    ExceptionManagement.ExceptionManager.Publish(New Exception(oGEStandingOrder.LastError))
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        ''Procedure updates Standing order to set Membership turn on.
        Protected Overridable Sub UpdateStandingOrder(ByRef oGE As Aptify.Applications.Subscriptions.SubscriptionEntity)
            Dim oGEStandingOrder As AptifyGenericEntityBase
            oGEStandingOrder = Me.AptifyApplication.GetEntityObject("Standing Orders", oGE.AutoRenewStandingOrderID)
            With oGEStandingOrder
                .SetValue("Status", CStr(AptifyApplication.Entity("Standing Orders").EntityMetaData.Fields("Status").DefaultValue))
                .SetValue("DateExpires", Nothing)
                If .Save(False) Then
                    oGE.SetValue("AutoRenew", True)
                Else
                    ExceptionManagement.ExceptionManager.Publish(New Exception(.LastError))
                End If
            End With
        End Sub

        ''Function returns true if standingorder status set to inactive and expiredate set to today's date to set Membership off.
        Protected Overridable Function SetAutoRenewTrunOff(ByRef oGE As Aptify.Applications.Subscriptions.SubscriptionEntity) As Boolean
            Try
                Dim bSucess As Boolean
                Dim oGEStandingOrder As AptifyGenericEntityBase
                oGEStandingOrder = Me.AptifyApplication.GetEntityObject("Standing Orders", oGE.AutoRenewStandingOrderID)
                With oGEStandingOrder
                    .SetValue("Status", "Inactive")
                    .SetValue("DateExpires", Now)
                    If .Save(False) Then
                        oGE.SetValue("AutoRenew", False)
                        bSucess = True
                    Else
                        bSucess = False
                        ExceptionManagement.ExceptionManager.Publish(New Exception(.LastError))
                    End If

                End With
                Return bSucess
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return False
            End Try
        End Function

        Protected Overridable Sub ShowBillMeLater()
            Dim iPrevPaymentTypeID As Integer
            Dim iPOPaymentType As Integer = 0
            Dim sError As String
            Dim oOrder As Aptify.Applications.OrderEntry.OrdersEntity

            Try
                Dim sAttr As String = AptifyApplication.GetEntityAttribute("Web Shopping Carts", "POPaymentTypeID")
                If Not String.IsNullOrEmpty(sAttr) Then
                    iPOPaymentType = CInt(sAttr)
                End If
                Dim dr As Data.DataRow = User1.CompanyDataRow
                CreditCard.UserCreditStatus = CInt(User1.GetValue("CreditStatusID"))
                CreditCard.UserCreditLimit = CLng(User1.GetValue("CreditLimit"))
                oOrder = ShoppingCart1.GetOrderObject(Session, Page.User, Application)
                If iPOPaymentType > 0 Then
                    If dr IsNot Nothing Then
                        CreditCard.CompanyCreditStatus = CInt(dr.Item("CreditStatusID"))
                        CreditCard.CompanyCreditLimit = CLng(dr.Item("CreditLimit"))
                    End If
                    If oOrder IsNot Nothing Then
                        iPrevPaymentTypeID = CInt(oOrder.GetValue("PayTypeID"))
                        oOrder.SetValue("PayTypeID", iPOPaymentType)
                        CreditCard.CreditCheckLimit = ShoppingCart1.CreditCheckObject.CheckCredit(CType(oOrder, Aptify.Applications.OrderEntry.OrdersEntity), sError)
                    End If
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            Finally

            End Try

        End Sub

        ''Function Returns True if Membership has Qutation Order Linked.
        Protected Overridable Function GetSubscriptionOrdersLinkID(ByVal SubscriptionID As Integer) As Long

            Dim sSQL As String
            Dim params(0) As IDataParameter
            Dim DT As Data.DataTable
            Dim ivalue As Integer
            Try

                sSQL = Database & ".." & "spGetSubscriptionOrdersLink"

                params(0) = Me.DataAction.GetDataParameter("@SubscriptionID", SqlDbType.Int, SubscriptionID)
                DT = Me.DataAction.GetDataTableParametrized(sSQL, CommandType.StoredProcedure, params)
                If DT IsNot Nothing AndAlso DT.Rows.Count > 0 Then
                    For Each dr As DataRow In DT.Rows
                        If CInt(dr.Item("OrderStatusID")) = OrderStatus.Taken AndAlso CInt(dr.Item("OrderTypeID")) = OrderType.Quotation Then
                            ivalue = CInt(dr.Item("ID"))
                            Exit For
                        End If
                    Next
                End If
                DT = Nothing
                ivalue = 0
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                DT = Nothing
                ivalue = 0
            End Try
            Return ivalue
        End Function

        ''Funtion returns true is any Row is already have Membership enabled (or AntoRenew On)
        Protected Overridable Function SelectedRowsHasAutoRenewEnabled() As Boolean
            Dim bAutoRenewEnabled As Boolean = False
            Dim gvRow As GridDataItem
            Dim chkSelected As CheckBox
            Dim lblOnOff As Label

            For Each gvRow In radGridMembership.Items
                chkSelected = CType(gvRow.FindControl("chkSubscriber"), CheckBox)
                lblOnOff = CType(gvRow.FindControl("lblOnOff"), Label)

                If chkSelected.Checked AndAlso lblOnOff.Text = "On" Then
                    bAutoRenewEnabled = True
                    Exit For
                End If
            Next

            Return bAutoRenewEnabled
        End Function

        ''Function returns True if any Membership is Expired
        Protected Overridable Function SelectedRowsHasExpired() As Boolean
            Dim bIsExpired As Boolean = False
            Dim gvRow As GridDataItem
            Dim chkSelected As CheckBox
            Dim lblExpiryDate As Label

            For Each gvRow In radGridMembership.Items
                chkSelected = CType(gvRow.FindControl("chkSubscriber"), CheckBox)
                lblExpiryDate = CType(gvRow.FindControl("lblExpiryDate"), Label)

                If chkSelected.Checked AndAlso CType(lblExpiryDate.Text, DateTime) < DateTime.Today Then
                    bIsExpired = True
                    Exit For
                End If
            Next

            Return bIsExpired
        End Function

        Protected Function RowSelected(ByVal dt As DataTable) As Boolean
            Try

                Dim bSubscriptionSelected As Boolean = False
                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                    bSubscriptionSelected = True
                End If
                Return bSubscriptionSelected
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return False
            End Try
        End Function

        'This method is used to save the checkedstate of values
        Private Sub SaveCheckedValues()
            Dim userdetails As New ArrayList()
            Dim index As Long = -1


            Dim dtSelectedRecord As DataTable

            dtSelectedRecord = New DataTable

            dtSelectedRecord.Columns.Add("ProductID")
            dtSelectedRecord.Columns.Add("ProductName")
            dtSelectedRecord.Columns.Add("PurchaseType")
            dtSelectedRecord.Columns.Add("SubscriptionID")
            dtSelectedRecord.Columns.Add("SubscriberID")
            dtSelectedRecord.Columns.Add("AutoRenew")

            Dim primaryKey(0) As DataColumn
            primaryKey(0) = dtSelectedRecord.Columns("SubscriptionID")
            dtSelectedRecord.PrimaryKey = primaryKey

            If ViewState(ATTRIBUTE_SELECTED_ROWS) IsNot Nothing Then
                dtSelectedRecord = CType(ViewState(ATTRIBUTE_SELECTED_ROWS), DataTable)
            End If

            For Each item As GridDataItem In radGridMembership.MasterTableView.Items
                index = CLng(CType(item.FindControl("lblSubscriptionID"), Label).Text)

                Dim result As Boolean = DirectCast(item.FindControl("chkSubscriber"), CheckBox).Checked

                Dim dr As DataRow = dtSelectedRecord.NewRow
                If result Then

                    dr.Item("ProductID") = CType(item.FindControl("lblProductID"), Label).Text
                    dr.Item("ProductName") = CType(item.FindControl("lblProduct"), Label).Text
                    dr.Item("PurchaseType") = CType(item.FindControl("lblPurchaseType"), Label).Text
                    dr.Item("SubscriptionID") = CType(item.FindControl("lblSubscriptionID"), Label).Text
                    dr.Item("SubscriberID") = CType(item.FindControl("lblPersonID"), Label).Text
                    dr.Item("AutoRenew") = CType(item.FindControl("lblOnOff"), Label).Text
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
#End Region
        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME

            If String.IsNullOrEmpty(RadBlankImage) Then
                RadBlankImage = Me.GetLinkValueFromXML(ATTRIBUTE_PERSON_BLANK_IMG)
            End If
            If String.IsNullOrEmpty(ActiveImage) Then
                ActiveImage = Me.GetLinkValueFromXML(ATTRIBUTE_ACTIVE_IMG)
            End If
            If String.IsNullOrEmpty(ExpiringSoonImage) Then
                ExpiringSoonImage = Me.GetLinkValueFromXML(ATTRIBUTE_EXPIRING_SOON_IMG)
            End If
            If String.IsNullOrEmpty(ExpiredImage) Then
                ExpiredImage = Me.GetLinkValueFromXML(ATTRIBUTE_EXPIRED_IMG)
            End If
            'Added by Sandeep for Issue 15051 on 12/03/2013
            If String.IsNullOrEmpty(LoginPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                LoginPage = Me.GetLinkValueFromXML(ATTRIBUTE_LOGIN_PAGE)
            End If

            'call base method to set parent properties
            MyBase.SetProperties()
        End Sub

        Protected Sub radGridMembership_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles radGridMembership.ItemDataBound
            Try
                Dim rdImage As RadBinaryImage = Nothing

                If e.Item Is Nothing OrElse e.Item.FindControl("imgmember") Is Nothing Then
                    Exit Sub
                End If

                rdImage = CType(e.Item.FindControl("imgmember"), RadBinaryImage)
                rdImage.ImageUrl = Me.FixLinkForVirtualPath(RadBlankImage)
                rdImage.DataBind()

                'Resizes the passed Image according to the specified width and height and returns the resized Image
                If Not IsDBNull(DataBinder.Eval(e.Item.DataItem, "Photo")) Then
                    Dim commonMethods As New Aptify.Framework.Web.eBusiness.CommonMethods()
                    Dim profileImage As Drawing.Image = Nothing
                    Dim width As Integer = ProfileThumbNailWidth
                    Dim height As Integer = ProfileThumbNailHeight
                    Dim aspratioWidth As Integer

                    Dim profileImageByte As Byte() = DirectCast(DataBinder.Eval(e.Item.DataItem, "Photo"), Byte())
                    If profileImageByte IsNot Nothing AndAlso profileImageByte.Length > 0 Then
                        commonMethods.getResizedImageHeightandWidth(profileImage, profileImageByte, ProfileThumbNailWidth, ProfileThumbNailHeight, aspratioWidth)
                        profileImage = commonMethods.byteArrayToImage(profileImageByte)
                        profileImageByte = commonMethods.resizeImageAndGetAsByte(profileImage, aspratioWidth, height)
                        rdImage.DataValue = profileImageByte
                        rdImage.DataBind()
                    Else
                        rdImage.ImageUrl = Me.FixLinkForVirtualPath(RadBlankImage)
                        rdImage.DataBind()
                    End If
                End If
                Dim lblOnOff As Label = CType(e.Item.FindControl("lblOnOff"), Label)

                If Not IsDBNull(DataBinder.Eval(e.Item.DataItem, "AutoRenew")) Then
                    If CType(DataBinder.Eval(e.Item.DataItem, "AutoRenew"), Boolean) = True Then
                        lblOnOff.Text = "On"
                    Else
                        lblOnOff.Text = "Off"
                    End If
                End If
                Dim lblAddress As Label = CType(e.Item.FindControl("lblCity"), Label)
                If Not IsDBNull(DataBinder.Eval(e.Item.DataItem, "City")) AndAlso String.IsNullOrEmpty((DataBinder.Eval(e.Item.DataItem, "City")).ToString()) = False Then
                    lblAddress.Text = lblAddress.Text & ","
                End If

                Dim lblstatus As Label = CType(e.Item.FindControl("lblstatus"), Label)
                Dim imgStatus As Image = CType(e.Item.FindControl("imgStatus"), Image)

                If CType(DataBinder.Eval(e.Item.DataItem, "EndDate"), DateTime) > Date.Now().AddDays(90) Then
                    imgStatus.ImageUrl = Me.FixLinkForVirtualPath(ActiveImage)
                    lblstatus.Text = "Active"
                ElseIf CType(DataBinder.Eval(e.Item.DataItem, "EndDate"), DateTime) > Date.Now() AndAlso CType(DataBinder.Eval(e.Item.DataItem, "EndDate"), DateTime) < Date.Now().AddDays(90) Then
                    imgStatus.ImageUrl = Me.FixLinkForVirtualPath(ExpiringSoonImage)
                    lblstatus.Text = "Expiring"
                ElseIf CType(DataBinder.Eval(e.Item.DataItem, "EndDate"), DateTime) < Date.Now() Then
                    imgStatus.ImageUrl = Me.FixLinkForVirtualPath(ExpiredImage)
                    lblstatus.Text = "Expired"
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub btnRenewalOn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRenewalOn.Click
            ClearCreditCardControl()
            Dim bShowPaymentcontrol As Boolean
            Dim oGE As Aptify.Applications.Subscriptions.SubscriptionEntity
            Dim OnOff As String
            Dim SubscriptionID As String
            Dim dtSelectedRecords As DataTable = Nothing
            Try
                SaveCheckedValues()
                Dim bSubscriptionSelected As Boolean = False
                If ViewState(ATTRIBUTE_SELECTED_ROWS) IsNot Nothing Then
                    dtSelectedRecords = CType(ViewState(ATTRIBUTE_SELECTED_ROWS), DataTable)
                End If
                If dtSelectedRecords IsNot Nothing AndAlso dtSelectedRecords.Rows.Count > 0 Then
                    bSubscriptionSelected = RowSelected(dtSelectedRecords)
                End If
                If bSubscriptionSelected = False Then
                    lblError.Text = "Please select at least one membership record."
                    radWindowMessage.VisibleOnPageLoad = True
                    Exit Sub
                End If
                radWindowPayment.VisibleOnPageLoad = False

                If SelectedRowsHasExpired() = True Then
                    lblError.Text = "One or more selected Memberships have Expired. Auto Renew such Memberships by Renewing and enabling Auto Renewal during checkout."
                    radWindowMessage.VisibleOnPageLoad = True
                Else

                    If dtSelectedRecords IsNot Nothing AndAlso dtSelectedRecords.Rows.Count > 0 Then
                        For Each row As DataRow In dtSelectedRecords.Rows
                            OnOff = CType(row.Item("AutoRenew"), String)
                            SubscriptionID = CType(row.Item("SubscriptionID"), String)

                            If OnOff = "Off" Then
                                oGE = DirectCast(Me.AptifyApplication.GetEntityObject("Subscriptions", CLng(SubscriptionID)), Aptify.Applications.Subscriptions.SubscriptionEntity)
                                If Not oGE Is Nothing Then
                                    If oGE.AutoRenewStandingOrderID = -1 Then
                                        bShowPaymentcontrol = True
                                    End If
                                End If
                            End If
                        Next

                        lblError.Text = ""
                        radWindowMessage.VisibleOnPageLoad = False

                        If bShowPaymentcontrol = True Then
                            radWindowPayment.VisibleOnPageLoad = True
                        Else
                            radWindowPayment.VisibleOnPageLoad = False
                            SetAutoRenewOn(dtSelectedRecords)
                            ViewState.Remove(ATTRIBUTE_SELECTED_ROWS)
                            dtSelectedRecords = Nothing
                        End If
                    End If
                    LoadMemberships(True)
                End If
                upnlMain.Update()

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            Try
                radWindowPayment.VisibleOnPageLoad = False
                ClearCreditCardControl()
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub btnMakePayment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMakePayment.Click
            Try
                Dim dtSelectedRecords As DataTable = Nothing
                chkMakePayment.Checked = True
                SaveCheckedValues()
                If ViewState(ATTRIBUTE_SELECTED_ROWS) IsNot Nothing Then
                    dtSelectedRecords = CType(ViewState(ATTRIBUTE_SELECTED_ROWS), DataTable)
                End If
                If dtSelectedRecords IsNot Nothing AndAlso dtSelectedRecords.Rows.Count > 0 Then
                    SetAutoRenewOn(dtSelectedRecords)
                End If

                radWindowPayment.VisibleOnPageLoad = False
                ClearCreditCardControl()
                dtSelectedRecords = Nothing
                ViewState.Remove(ATTRIBUTE_SELECTED_ROWS)
                LoadMemberships(True)
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub btnRenew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRenew.Click
            Try
                Dim strProductID As String
                Dim strProductName As String
                Dim strPurchaseType, strSubscriberID, strSubscriptionID As String
                Dim oOrder As AptifyGenericEntityBase
                Dim count As Integer, ExistingOrderID As Long
                Dim dtSelectedRecords As DataTable = Nothing

                Dim bSubscriptionSelected As Boolean = False
                radWindowPayment.VisibleOnPageLoad = False

                SaveCheckedValues()
                If ViewState(ATTRIBUTE_SELECTED_ROWS) IsNot Nothing Then
                    dtSelectedRecords = CType(ViewState(ATTRIBUTE_SELECTED_ROWS), DataTable)
                End If

                If dtSelectedRecords IsNot Nothing AndAlso dtSelectedRecords.Rows.Count > 0 Then
                    bSubscriptionSelected = RowSelected(dtSelectedRecords)
                End If

                If Not bSubscriptionSelected Then
                    lblError.Text = "Please select one or more items to renew and click " & btnRenew.Text & "."
                    radWindowMessage.VisibleOnPageLoad = True
                ElseIf SelectedRowsHasAutoRenewEnabled() = True Then
                    lblError.Text = "One or more selected records has Auto Renewal ""On"". Cannot Renew Membership for records with Auto Renewal ""On""."
                    radWindowMessage.VisibleOnPageLoad = True
                Else
                    lblError.Text = ""
                    radWindowMessage.VisibleOnPageLoad = False

                    oOrder = ShoppingCart1.GetOrderObject(Page.Session, Page.User, Page.Application)

                    If dtSelectedRecords IsNot Nothing AndAlso dtSelectedRecords.Rows.Count > 0 Then

                        For Each Row As DataRow In dtSelectedRecords.Rows

                            strProductID = CStr(Row.Item("ProductID"))
                            strProductName = CStr(Row.Item("ProductName"))
                            strPurchaseType = CStr(Row.Item("PurchaseType"))
                            strSubscriptionID = CStr(Row.Item("SubscriptionID"))
                            strSubscriberID = CStr(Row.Item("SubscriberID"))

                            ShoppingCart1.AddToCart(CLng(strProductID), False)

                            ExistingOrderID = GetSubscriptionOrdersLinkID(CInt(strSubscriptionID))
                            If ExistingOrderID > 0 Then
                                oOrder.SubTypes("OrderLines").Item(count).SetAddValue("_xExistingOrderID", ExistingOrderID)
                            End If
                            'Suraj Issue 16516 we remove the _xRenewalStatus temp fieldso insteade of we set the value for Comments which is required on billing control for mailing purpose
                            oOrder.SubTypes("OrderLines").Item(count).SetValue("Comments", "RENEWMEMBERSHIP")
                            oOrder.SubTypes("OrderLines").Item(count).SetValue("Description", "Renewal: " + strProductName)
                            oOrder.SubTypes("OrderLines").Item(count).SetValue("PurchaseType", strPurchaseType)
                            oOrder.SubTypes("OrderLines").Item(count).SetValue("SubscriberID", CInt(strSubscriberID))

                            count = count + 1

                        Next
                    End If
                    ShoppingCart1.SaveCart(Session)
                    'Suraj Issue 16516  remove  Session(ATTRIBUTE_SUSCRIPTION_ORDER) = oOrder
                    ViewState.Remove(ATTRIBUTE_SELECTED_ROWS)
                    dtSelectedRecords = Nothing
                    Response.Redirect(Me.RedirectURL)
                End If
                upnlMain.Update()
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub btnRenewalOff_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRenewalOff.Click

            Dim oGE As Aptify.Applications.Subscriptions.SubscriptionEntity
            Dim OnOff As String
            Dim SubscriptionID As String
            Dim dtSelectedRecords As DataTable = Nothing
            Try
                SaveCheckedValues()
                Dim bSubscriptionSelected As Boolean = False
                If ViewState(ATTRIBUTE_SELECTED_ROWS) IsNot Nothing Then
                    dtSelectedRecords = CType(ViewState(ATTRIBUTE_SELECTED_ROWS), DataTable)
                End If
                If dtSelectedRecords IsNot Nothing AndAlso dtSelectedRecords.Rows.Count > 0 Then
                    bSubscriptionSelected = RowSelected(dtSelectedRecords)
                End If
                If bSubscriptionSelected = False Then
                    lblError.Text = "Please select at least one membership record."
                    radWindowMessage.VisibleOnPageLoad = True
                    Exit Sub
                End If
                radWindowPayment.VisibleOnPageLoad = False
                If dtSelectedRecords IsNot Nothing AndAlso dtSelectedRecords.Rows.Count > 0 Then
                    For Each row As DataRow In dtSelectedRecords.Rows
                        OnOff = CType(row.Item("AutoRenew"), String)
                        SubscriptionID = CType(row.Item("SubscriptionID"), String)
                        If OnOff = "On" Then
                            oGE = DirectCast(Me.AptifyApplication.GetEntityObject("Subscriptions", CLng(SubscriptionID)), Aptify.Applications.Subscriptions.SubscriptionEntity)
                            If Not oGE Is Nothing Then
                                If SetAutoRenewTrunOff(oGE) Then
                                    If Not oGE.Save(False) Then
                                        ExceptionManagement.ExceptionManager.Publish(New Exception(oGE.LastError))
                                    End If

                                End If
                            End If
                        End If
                    Next
                    ViewState.Remove(ATTRIBUTE_SELECTED_ROWS)
                    dtSelectedRecords = Nothing
                    LoadMemberships(True)
                End If
                ViewState.Remove(ATTRIBUTE_SELECTED_ROWS)
                dtSelectedRecords = Nothing
                upnlMain.Update()

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
            lblError.Text = ""
            radWindowMessage.VisibleOnPageLoad = False
        End Sub

        Protected Sub radGridMembership_PageIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageChangedEventArgs) Handles radGridMembership.PageIndexChanged
            SaveCheckedValues()
            If ViewState(ATTRIBUTE_DT_MEMBERSHIP) IsNot Nothing Then
                radGridMembership.DataSource = CType(ViewState(ATTRIBUTE_DT_MEMBERSHIP), DataTable)
                radGridMembership.CurrentPageIndex = e.NewPageIndex
            End If
            upnlMain.Update()
        End Sub

        Protected Sub radGridMembership_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles radGridMembership.NeedDataSource
            Try
                If ViewState(ATTRIBUTE_DT_MEMBERSHIP) IsNot Nothing Then
                    radGridMembership.DataSource = CType(ViewState(ATTRIBUTE_DT_MEMBERSHIP), DataTable)
                End If
                upnlMain.Update()
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub radGridMembership_PageSizeChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageSizeChangedEventArgs) Handles radGridMembership.PageSizeChanged
            If ViewState(ATTRIBUTE_DT_MEMBERSHIP) IsNot Nothing Then
                radGridMembership.DataSource = CType(ViewState(ATTRIBUTE_DT_MEMBERSHIP), DataTable)
            End If
            upnlMain.Update()
        End Sub

        Protected Sub radGridMembership_GridItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs)

            Dim chkSubscriber As CheckBox = DirectCast(e.Item.FindControl("chkSubscriber"), CheckBox)
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

        'Suraj Issue 15287 4/5/13 ,if the grid load first time By default the sorting will be Ascending for column Forum 
        Private Sub AddExpressionMultipleMembership()
            Dim expressionCompanyMembership As New GridSortExpression
            expressionCompanyMembership.FieldName = "ID"
            expressionCompanyMembership.SetSortOrder("Ascending")
            radGridMembership.MasterTableView.SortExpressions.AddSortExpression(expressionCompanyMembership)
        End Sub


    End Class
End Namespace

