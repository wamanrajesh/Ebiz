'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports Aptify.Framework.Application
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.BusinessLogic.Security
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports System.Data
Imports Telerik.Web.UI
Imports Aptify.Applications.OrderEntry
Namespace Aptify.Framework.Web.eBusiness.CustomerService
    Partial Class CompanyMembership
        Inherits BaseUserControlAdvanced
        Protected Const ATTRIBUTE_ADMIN_EDIT_PROFILE As String = "AdminEditprofileUrl"
        Protected Const ATTRIBUTE_SAVE_BUTTON_REDIRECT As String = "SaveButtonRedirectToCart"
        Protected Const ATTRIBUTE_GRID_TEXT As String = "GridInformationText"
        'Neha Issue 16001,05/07/13, Declared properties for RadBinaryimage
        Protected Const ATTRIBUTE_PERSON_BLANK_IMG As String = "RadBlankImage"
        Protected Const ATTRIBUTE_PROFILE_THUMBNAIL_WIDTH As String = "ProfileThumbNailWidth"
        Protected Const ATTRIBUTE_PROFILE_THUMBNAIL_HEIGHT As String = "ProfileThumbNailHeight"
        Protected Const ATTRIBUTE_LOGIN_PAGE As String = "LoginPage" 'Added by Sandeep for Issue 15051 on 12/03/2013
        'Suraj Issue 14302,3/14/13  Declared global variable for bind the rad combobox if the count is 0 then bind the radcombo box
        Dim radComboBoxDataBoundcount As Integer = 0
        'Suraj Issue 14302,3/14/13  view state name
        Protected Const ATTRIBUTE_PURCHASEMEMBERSHIP_VIEWSTATE As String = "PurchaseMembershipdt"
        'Suraj Issue 14450 3/23/13 ,declare the property
        Protected Const ATTRIBUTE_CHECKED_SUBSCRIPTION = "CheckedSubscriptionFormGrid"




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

#Region "Group Admin Specific Edit Profile"
        ''' <summary>
        ''' Meeting page url
        ''' </summary>
        Public Overridable Property AdminEditProfile() As String
            Get
                If Not ViewState(ATTRIBUTE_ADMIN_EDIT_PROFILE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_ADMIN_EDIT_PROFILE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_ADMIN_EDIT_PROFILE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        Public Overridable Property SaveButtonRedirectToCart() As String
            Get
                If Not ViewState(ATTRIBUTE_SAVE_BUTTON_REDIRECT) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_SAVE_BUTTON_REDIRECT))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_SAVE_BUTTON_REDIRECT) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        Public Property GridInformationText() As String
            Get
                If ViewState(ATTRIBUTE_GRID_TEXT) IsNot Nothing Then
                    Return ViewState(ATTRIBUTE_GRID_TEXT).ToString
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_GRID_TEXT) = value
            End Set
        End Property
        Public Overridable Property RadBlankImage() As String
            Get
                If Not ViewState(ATTRIBUTE_PERSON_BLANK_IMG) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_PERSON_BLANK_IMG))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_PERSON_BLANK_IMG) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        'Neha, Issue 14810, 03/09/13, Overrided properties for Radbinaryimage
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
#End Region

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(SaveButtonRedirectToCart) Then
                'since value is the 'default' check the XML file for possible custom setting
                SaveButtonRedirectToCart = Me.GetLinkValueFromXML(ATTRIBUTE_SAVE_BUTTON_REDIRECT)
                If String.IsNullOrEmpty(SaveButtonRedirectToCart) Then
                    'Me.cmdSave.Enabled = False
                    'Me.cmdSave.ToolTip = "SaveButtonRedirect property has not been set."
                End If
            End If

            If String.IsNullOrEmpty(GridInformationText) Then
                'since value is the 'default' check the XML file for possible custom setting
                GridInformationText = Me.GetPropertyValueFromXML(ATTRIBUTE_GRID_TEXT)
            End If
            If String.IsNullOrEmpty(RadBlankImage) Then
                RadBlankImage = Me.GetLinkValueFromXML(ATTRIBUTE_PERSON_BLANK_IMG)
            End If
            'Added by Sandeep for Issue 15051 on 12/03/2013
            If String.IsNullOrEmpty(LoginPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                LoginPage = Me.GetLinkValueFromXML(ATTRIBUTE_LOGIN_PAGE)
            End If

        End Sub
        'Suraj Issue 14302 2/4/13 Find the RadCombobox And  bind it. 
        'use use this logic because of when we bind this radcombo in itemcreated event
        'then sorting and filtering gives a problem as well as when the page gets load 
        'only at that time we bind the header rad combobox
        Protected Sub RadddlHeaderMemberType_DataBound(ByVal sender As Object, ByVal e As EventArgs)
            If radComboBoxDataBoundcount = 0 Then
                radComboBoxDataBoundcount = +1
                Dim RadddlHeaderMemberType = DirectCast(sender, RadComboBox)
                Dim dtproduct As DataTable = ShowMembershipProduct()
                RadddlHeaderMemberType.DataSource = dtproduct
                RadddlHeaderMemberType.DataTextField = "webname"
                RadddlHeaderMemberType.DataValueField = "id"
                RadddlHeaderMemberType.DataBind()
                RadddlHeaderMemberType.AutoPostBack = True
                dtproduct = Nothing
            End If
        End Sub
        Protected Sub LoadPerson()
            Dim dt As DataTable, sSQL As String, sDB As String
            Try
                'Suraj issue 14302 3/14/13 , check the view state is nothing or not if the page load first time viewstate will be nothing but after bostback view state will conatin the datatable
                If ViewState(ATTRIBUTE_PURCHASEMEMBERSHIP_VIEWSTATE) Is Nothing Then
                    Dim companyID As Integer = -1
                    Dim oGE As Aptify.Applications.Persons.PersonsEntity = DirectCast(Me.AptifyApplication.GetEntityObject("Persons", User1.PersonID), Aptify.Applications.Persons.PersonsEntity)


                    If oGE IsNot Nothing Then
                        companyID = oGE.CompanyID
                    End If
                    Dim objaptifyapp As New AptifyApplication()
                    'Suraj issue 14302 4/2/13 , before	The page  only displays people with member type = “Non-Member”. So, if a create a new member type record with “Is Member” flag set to No, the person linked to this member type is not getting displayed on the grid. before itLooks like the filtering is done by name of member type and not based on “Is Member” flag but currently we perform filtering on “Is Member”.
                    sDB = objaptifyapp.GetEntityBaseDatabase("Persons")
                    sSQL = "select c.ID,c.FirstLast,c.Email,c.CompanyID,c.MemberType,c.Title,c.Photo,c.address,c.MemberTypeID from (" & _
                  "select ROW_NUMBER() over(partition by vp.id order by vp.id desc) as row, vp.id,(VP.FirstName + ' ' + VP.LastName) as FirstLast ,VP.Email,vp.CompanyID,VP.MemberType,Vp.title,VP.MemberTypeID,Vp.photo," & _
                  "address=case when vp.City ='' or vp.State= '' then (isnull(VP.city,'')+isnull(vp.State,'')) else " & _
                  "(Vp.city +',' + vp.state)  end" & _
                   " from   " & sDB & _
                         "..vwPersons VP) as c INNER JOIN ..vwMemberTypes as m  on m.ID  = c.MemberTypeID where c.row=1 and c.CompanyID=" + companyID.ToString + " and  m.IsMember=0 and m.IsActive=1"


                    dt = Me.DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                    Dim dcolUrl As DataColumn = New DataColumn()
                    dcolUrl.Caption = "AdminEditprofileUrl"
                    dcolUrl.ColumnName = "AdminEditprofileUrl"

                    dt.Columns.Add(dcolUrl)
                    'If dt.Rows.Count > 0 Then
                    '    For Each rw As DataRow In dt.Rows
                    '        rw("AdminEditprofileUrl") = AdminEditProfile + "?ID=" + rw("ID").ToString
                    '    Next
                    'End If
                    If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                        Me.grdperson.DataSource = dt
                        'Suraj issue 14302 3/14/13 , if when page first time load here we store the datatable in to a viewstate
                        ViewState(ATTRIBUTE_PURCHASEMEMBERSHIP_VIEWSTATE) = dt
                        'Suraj Issue 14302,  12/3/13  Remove Me.grdperson.DataBind()
                        'Suraj issue 15287 4/5/13 , remove  'grdperson.Visible = False and 'grdperson.Visible = true because we are using "NoRecordsTemplate" for  display no record found msg
                        btnPurchaseMemberships.Visible = True
                    Else
                        'Suraj issue 15287 4/5/13 , remove  'grdperson.Visible = False and 'grdperson.Visible = False because we are using "NoRecordsTemplate" for  display no record found msg
                        ViewState(ATTRIBUTE_PURCHASEMEMBERSHIP_VIEWSTATE) = dt
                        grdperson.DataSource = dt
                        lblGridInfo.Visible = False
                        btnPurchaseMemberships.Visible = False
                    End If
                Else
                    'Suraj issue 14302 3/14/13 , after postback viewstate will assign for gridview
                    Dim tempdt As DataTable = CType(ViewState(ATTRIBUTE_PURCHASEMEMBERSHIP_VIEWSTATE), DataTable)
                    If Not tempdt Is Nothing AndAlso tempdt.Rows.Count > 0 Then
                        grdperson.DataSource = tempdt
                        btnPurchaseMemberships.Visible = True
                        'Suraj issue 15287 4/5/13 , remove  'grdperson.Visible = False and 'grdperson.Visible = true because we are using "NoRecordsTemplate" for  display no record found msg
                    Else
                        'Suraj issue 15287 4/5/13 , remove  'grdperson.Visible = False and 'grdperson.Visible = False because we are using "NoRecordsTemplate" for  display no record found msg
                        grdperson.DataSource = tempdt
                        lblGridInfo.Visible = False
                        btnPurchaseMemberships.Visible = False
                    End If
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Protected Sub ToggleSelectedState(ByVal sender As Object, ByVal e As EventArgs)
            Try
                Dim headerCheckBox As CheckBox = CType(sender, CheckBox)
                For Each dataItem As GridDataItem In grdperson.MasterTableView.Items
                    CType(dataItem.FindControl("chkperson"), CheckBox).Checked = headerCheckBox.Checked
                    dataItem.Selected = headerCheckBox.Checked
                    'Dim chkperson As CheckBox = CType(dataItem.FindControl("chkperson"), CheckBox)
                    'Dim row As GridDataItem = DirectCast(chkperson.Parent.Parent, GridDataItem)
                    'Dim idx As Integer = row.DataSetIndex
                    'Dim ddlMembershipeProduct As DropDownList = CType(grdperson.Items(idx).FindControl("ddlMemberType"), DropDownList)
                    'Dim lblPrice As Label = CType(grdperson.Items(idx).FindControl("lblPrice"), Label)
                    'Dim oPrice As IProductPrice.PriceInfo = Me.GetProductPrice(CLng(ddlMembershipeProduct.SelectedValue))
                    'lblPrice.Text = Format(oPrice.Price, ShoppingCart1.GetCurrencyFormat(oPrice.CurrencyTypeID))
                Next
                If lblmsg.Visible = True Then
                    If headerCheckBox.Checked = True Then
                        lblmsg.Visible = False
                    End If
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Protected Sub chkSelectChanged(ByVal sender As Object, ByVal e As EventArgs)
            Try
                'Suraj Issue 14302,4/17/13 , Maintain check box state after click on grid check box
                Dim ChkSelectPerson As CheckBox = CType(sender, CheckBox)
                Dim header As GridHeaderItem = TryCast(grdperson.MasterTableView.GetItems(GridItemType.Header)(0), GridHeaderItem)
                Dim chkAllMakePayment As CheckBox = DirectCast(header.FindControl("chkAllPerson"), CheckBox)
                Dim grow As GridDataItem = DirectCast(ChkSelectPerson.NamingContainer, GridDataItem)
                Dim i As Integer = grow.DataSetIndex Mod grdperson.PageSize
                If ChkSelectPerson.Checked = True Then
                    lblmsg.Visible = False
                End If
                Dim chkflag As Boolean = True
                For Each row As GridDataItem In grdperson.Items
                    ChkSelectPerson = DirectCast(row.FindControl("chkperson"), CheckBox)
                    If ChkSelectPerson.Checked = True Then

                    Else
                        chkflag = False
                        If chkAllMakePayment.Checked = True Then
                            chkAllMakePayment.Checked = False
                        End If
                    End If
                Next
                If chkflag Then
                    chkAllMakePayment.Checked = True
                End If
                For Each row As GridDataItem In grdperson.Items
                    ChkSelectPerson = DirectCast(row.FindControl("chkperson"), CheckBox)
                    SaveCheckedValues()
                Next
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Public Function GetProductPrice(ByVal lProductID As Long) As IProductPrice.PriceInfo
            ' Implement This function
            Return ShoppingCart1.GetUserProductPrice(lProductID, 1)
        End Function
        Protected Function SetProductPrice(ByVal ProductID As Long, ByRef AutoRenewed As Boolean) As String
            Try
                Dim oGE As Aptify.Applications.ProductSetup.ProductObject = DirectCast(Me.AptifyApplication.GetEntityObject("Products", ProductID), Aptify.Applications.ProductSetup.ProductObject)
                If oGE IsNot Nothing Then
                    Dim oPrice As IProductPrice.PriceInfo = Me.GetProductPrice(CLng(ProductID))
                    'lblPrice.Text = Format(oPrice.Price, ShoppingCart1.GetCurrencyFormat(oPrice.CurrencyTypeID))
                    'oGE.GetObjectPrice(
                    'oPrice.CurrencyTypeID()
                    'oGE.GetObjectPrice(
                    Dim Price As String = CStr(oGE.GetPrice(User1.PersonID, oPrice.CurrencyTypeID))
                    AutoRenewed = CBool(oGE.GetValue("IsSubscription"))
                    Return Price
                End If
            Catch ex As Exception
            End Try
        End Function

        Protected Function IsAutoRenewed(ByVal ProductID As Long) As Boolean
            Try
                Dim b_AutoRenewed As Boolean
                Dim oGE As Aptify.Applications.ProductSetup.ProductObject = DirectCast(Me.AptifyApplication.GetEntityObject("Products", ProductID), Aptify.Applications.ProductSetup.ProductObject)
                If oGE IsNot Nothing Then
                    'Dim oPrice As IProductPrice.PriceInfo = Me.GetProductPrice(CLng(ProductID))
                    'lblPrice.Text = Format(oPrice.Price, ShoppingCart1.GetCurrencyFormat(oPrice.CurrencyTypeID))
                    'oGE.GetObjectPrice(
                    'oPrice.CurrencyTypeID()
                    'oGE.GetObjectPrice(
                    'Dim Price As String = CStr(oGE.GetPrice(User1.PersonID, oPrice.CurrencyTypeID))
                    b_AutoRenewed = CBool(oGE.GetValue("IsSubscription"))
                    Return b_AutoRenewed
                End If
            Catch ex As Exception
            End Try
        End Function

        Protected Sub ddlMemberTypeChanged(ByVal sender As Object, ByVal e As EventArgs)
            Try
                Dim ddlMemberType As DropDownList = DirectCast(sender, DropDownList)
                Dim row As GridDataItem = DirectCast(ddlMemberType.Parent.Parent, GridDataItem)
                Dim strIndex As String = grdperson.MasterTableView.CurrentPageIndex.ToString()
                Dim idx As Integer = row.DataSetIndex
                Dim strFinalidx As Integer = 0
                If CInt(strIndex) > 0 Then
                    strFinalidx = idx - (CInt(strIndex) * 10)
                    Dim lblPrice As Label = CType(grdperson.Items(strFinalidx).FindControl("lblPrice"), Label)
                    Dim ddlAutoRenew As DropDownList = CType(grdperson.Items(strFinalidx).FindControl("ddlAutoRenew"), DropDownList)
                    Dim oPrice As IProductPrice.PriceInfo = Me.GetProductPrice(CLng(ddlMemberType.SelectedValue))
                    lblPrice.Text = Format(oPrice.Price, ShoppingCart1.GetCurrencyFormat(oPrice.CurrencyTypeID))
                    Dim b_chkAutoRenew As Boolean = False
                    b_chkAutoRenew = IsAutoRenewed(CLng(ddlMemberType.SelectedValue))
                    If b_chkAutoRenew = False Then
                        ddlAutoRenew.Visible = False
                    Else
                        ddlAutoRenew.Visible = True
                    End If
                Else
                    Dim lblPrice As Label = CType(grdperson.Items(idx).FindControl("lblPrice"), Label)
                    Dim ddlAutoRenew As DropDownList = CType(grdperson.Items(idx).FindControl("ddlAutoRenew"), DropDownList)
                    Dim oPrice As IProductPrice.PriceInfo = Me.GetProductPrice(CLng(ddlMemberType.SelectedValue))
                    lblPrice.Text = Format(oPrice.Price, ShoppingCart1.GetCurrencyFormat(oPrice.CurrencyTypeID))
                    Dim b_chkAutoRenew As Boolean = False
                    b_chkAutoRenew = IsAutoRenewed(CLng(ddlMemberType.SelectedValue))
                    If b_chkAutoRenew = False Then
                        ddlAutoRenew.Visible = False
                    Else
                        ddlAutoRenew.Visible = True
                    End If
                End If


            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Overridable Function ShowMembershipProduct() As DataTable
            Dim dt_m As DataTable, sSQL As String
            Try
                'Suraj issue 14302 4/2/13 , 5/2/13 I revert back the changes  m.DefaultType <> 'Companies' to m.DefaultType = 'Persons'
                'sSQL = "Select id, webname,IsSubscription from " & Database & "..vwProducts where CategoryID=1 and WebEnabled=1 and  IsSold=1 AND TopLevelItem=1 "
                sSQL = "Select p.ID,p.WebName,p.IsSubscription from " & Database & "..vwProducts p inner join " & Database & "..vwmembertypes m on p.MemberTypeID= m.ID where p.DefaultDuesProduct=1 and p.WebEnabled=1 and  p.IsSold=1 AND p.TopLevelItem=1 and (QuantityAvailable>=1 or RequireInventory=0) and (isnull(p.DateAvailable,getdate()) <=GETDATE() and (isnull(p.AvailableUntil,GETDATE()) >=GETDATE())or p.AvailableUntil='Jan  1 1900' ) and (m.DefaultType ='persons' AND m.IsMember=1 AND m.IsActive=1)"
                dt_m = Me.DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                If Not dt_m Is Nothing AndAlso dt_m.Rows.Count > 0 Then
                    Me.grdperson.DataSource = dt_m
                    Return dt_m
                Else
                    Return Nothing
                End If
            Catch ex As Exception
                Return Nothing
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Function
        'Suraj Issue 14302,4/17/13 , Maintain check box state after click on grid check box
        Protected Sub grdperson_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdperson.DataBound
            Dim index As Long = -1
            Dim flag As Boolean = True
            Dim header As GridHeaderItem = TryCast(grdperson.MasterTableView.GetItems(GridItemType.Header)(0), GridHeaderItem)
            Dim chkAllPerson As CheckBox = DirectCast(header.FindControl("chkAllPerson"), CheckBox)
            For Each item As GridDataItem In grdperson.MasterTableView.Items
                Dim result As Boolean = DirectCast(item.FindControl("chkperson"), CheckBox).Checked
                If Not result Then
                    flag = False
                End If
            Next
            If flag = False Then
                chkAllPerson.Checked = False
            Else
                If grdperson.Items.Count <= 0 Then
                    chkAllPerson.Checked = False
                Else
                    chkAllPerson.Checked = True
                End If

            End If
            SaveCheckedValues()
        End Sub


        'Neha, Issue 14810, 03/09/13,used Radbinaryimage and Resize the Image
        Protected Sub grdperson_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdperson.ItemDataBound
            Try
                Dim chkSubscription As CheckBox = DirectCast(e.Item.FindControl("chkperson"), CheckBox)
                Dim ddlDeliveryType As DropDownList = DirectCast(e.Item.FindControl("ddlMemberType"), DropDownList)
                Dim ddlAutoRenew As DropDownList = DirectCast(e.Item.FindControl("ddlAutoRenew"), DropDownList)
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
                                chkSubscription.Checked = CBool(lstSubValue(2))
                                ddlDeliveryType.SelectedValue = lstSubValue(0)
                                ddlAutoRenew.SelectedValue = lstSubValue(1)


                            End If
                        End If
                    End If
                    If chkSubscription.Checked = True Then
                        chkSubscription.Checked = True
                        chkSubscription.Enabled = False
                    End If
                End If
                'Neha Changes for issue 16001, 05/07/13
                Dim imagememberid As RadBinaryImage = Nothing
                If e.Item Is Nothing OrElse e.Item.FindControl("imgmember") Is Nothing Then
                    Exit Sub
                End If
                imagememberid = CType(e.Item.FindControl("imgmember"), RadBinaryImage)
                imagememberid.ImageUrl = Me.FixLinkForVirtualPath(RadBlankImage)
                imagememberid.DataBind()
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
                        imagememberid.DataValue = profileImageByte
                        imagememberid.DataBind()
                    Else
                        imagememberid.ImageUrl = Me.FixLinkForVirtualPath(RadBlankImage)
                        imagememberid.DataBind()
                    End If
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
            Dim dsproduct As DataTable = ShowMembershipProduct()
            Try
                Dim ddl As New DropDownList
                Dim ddlAutoRenew As New DropDownList
                Dim lblPrice As New Label
                'Dim imgestatusID As Telerik.Web.UI.RadBinaryImage
                ddl = CType(e.Item.FindControl("ddlMemberType"), DropDownList)
                ddlAutoRenew = CType(e.Item.FindControl("ddlAutoRenew"), DropDownList)
                lblPrice = CType(e.Item.FindControl("lblPrice"), Label)
                If ddl IsNot Nothing AndAlso ddlAutoRenew IsNot Nothing AndAlso lblPrice IsNot Nothing Then
                    If dsproduct IsNot Nothing AndAlso dsproduct.Rows.Count > 0 Then
                        ddl.DataSource = dsproduct
                        ddl.DataValueField = "id"
                        ddl.DataTextField = "webname"
                        ddl.DataBind()
                    Else
                        grdperson.Enabled = False
                        lblmsg.Text = "No Product is available for enroll new memberships"
                        lblmsg.ForeColor = Drawing.Color.Red
                        lblmsg.Visible = True
                        btnPurchaseMemberships.Enabled = False
                    End If
                    If ddl.Items.Count > 0 Then
                        Dim row As GridDataItem = DirectCast(ddl.Parent.Parent, GridDataItem)
                        Dim idx As Integer = row.DataSetIndex
                        'Dim lblPrice As Label = CType(grdperson.Items(idx).FindControl("lblPrice"), Label)
                        Dim oPrice As IProductPrice.PriceInfo = Me.GetProductPrice(CLng(ddl.SelectedValue))
                        lblPrice.Text = Format(oPrice.Price, ShoppingCart1.GetCurrencyFormat(oPrice.CurrencyTypeID))
                        'lblPrice.Text = SetProductPrice(CLng(ddl.SelectedValue), b_Autorenewed)
                        Dim b_chkAutoRenew As Boolean = False
                        b_chkAutoRenew = IsAutoRenewed(CLng(ddl.SelectedValue))
                        If b_chkAutoRenew = False Then
                            ddlAutoRenew.Visible = False
                        Else
                            ddlAutoRenew.Visible = True
                        End If
                    End If
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub btnPurchaseMemberships_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPurchaseMemberships.Click
            lblmsg.Visible = False
            BuyMembershipe()
        End Sub
        Protected Overridable Sub BuyMembershipe()
            Dim sProductPage As String = ""
            Dim sOrderPage As String = ""
            Dim oOrder As OrdersEntity
            Dim bCombineLines As Boolean = False
            Dim iQty As Integer = 1
            Dim ProductID As Long
            Dim b_Autorenewer As Boolean = False
            Dim iCount As Integer = 0
            'Added by Suraj For Issue 14671 on 27/02/2013
            Dim lstSubsciptionValue As List(Of String)
            Dim dicSubscriptionDetails As New Dictionary(Of Integer, List(Of String))
            Try
                SaveCheckedValues()
                If CType(ViewState(ATTRIBUTE_CHECKED_SUBSCRIPTION), Dictionary(Of Integer, List(Of String))) IsNot Nothing Then
                    dicSubscriptionDetails = CType(ViewState(ATTRIBUTE_CHECKED_SUBSCRIPTION), Dictionary(Of Integer, List(Of String)))
                End If
                For Each Subcriptions As KeyValuePair(Of Integer, List(Of String)) In dicSubscriptionDetails
                    lstSubsciptionValue = Subcriptions.Value
                    ProductID = CLng(lstSubsciptionValue(0))
                    If CStr(lstSubsciptionValue(1)) = "Yes" Then
                        b_Autorenewer = True
                    Else
                        b_Autorenewer = False
                    End If
                    If CBool(lstSubsciptionValue(2)) = True Then
                        iCount = iCount + 1
                        If ShoppingCart1.AddToCartMembershipProduct(ProductID, CLng((Subcriptions.Key)), b_Autorenewer, bCombineLines, iQty) Then
                            If ShoppingCart1.GetProductTypeWebPages(ProductID, sProductPage, sOrderPage) Then
                                If Len(sOrderPage) > 0 Then
                                    oOrder = ShoppingCart1.GetOrderObject(Page.Session, Page.User, Page.Application)
                                    'ShoppingCart1.SaveCart(Page.Session)
                                End If
                                'Me.SetTotalCartQty()
                            End If
                        Else
                        End If
                    End If

                Next
                ShoppingCart1.SaveCart(Page.Session)
                If iCount > 0 Then
                    MyBase.Response.Redirect(SaveButtonRedirectToCart, False)
                Else
                    lblmsg.Text = "Select a person."
                    lblmsg.ForeColor = Drawing.Color.Red
                    lblmsg.Visible = True
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try

        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'Suraj issue 14302 3/14/13 ,this method use to apply the odrering of rad grid first column
            If Not IsPostBack Then
                AddExpression()
            End If
            SetProperties()
            lblGridInfo.Text = GridInformationText
            If User1.UserID < 0 Then
                Response.Redirect(LoginPage) 'Added by Sandeep for Issue 15051 on 12/03/2013
            End If
        End Sub

        'Suraj Issue No 14302 , 2/4/13 When the RadComboBox Header selected 
        Protected Sub ddlHeaderMemberType_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs)
            Dim bAutoRenew As Boolean = False
            bAutoRenew = IsAutoRenewed(CLng(e.Value))
            Dim oPrice As IProductPrice.PriceInfo = Me.GetProductPrice(CLng(e.Value))
            Dim sFormatedPrice As String = Format(oPrice.Price, ShoppingCart1.GetCurrencyFormat(oPrice.CurrencyTypeID))
            Dim ddl As DropDownList = New DropDownList
            Dim lblPrice As Label = New Label
            Dim ddlAutoRenew As DropDownList = New DropDownList
            Session("slectedValue") = e.Value
            For Each dataItem As GridDataItem In grdperson.Items
                ddl = TryCast(dataItem.FindControl("ddlMemberType"), DropDownList)
                ddl.SelectedValue = e.Value
                lblPrice = CType(dataItem.FindControl("lblPrice"), Label)
                lblPrice.Text = sFormatedPrice
                ddlAutoRenew = CType(dataItem.FindControl("ddlAutoRenew"), DropDownList)
                ddlAutoRenew.Visible = bAutoRenew
            Next
        End Sub

        Protected Sub grdperson_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdperson.NeedDataSource
            If User1 IsNot Nothing AndAlso User1.PersonID > 0 Then
                LoadPerson()
            End If

            'Suraj Issue 14450 3/23/13 ,maintain the checkedstate of values 
            SaveCheckedValues()
        End Sub

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            LoadPerson()
        End Sub
        'Suraj Issue 14302 3/14/13 ,if the grid load first time By default the sorting will be Ascending for column Forum 
        Private Sub AddExpression()
            Dim expressionCompanyMembership As New GridSortExpression
            expressionCompanyMembership.FieldName = "ID"
            expressionCompanyMembership.SetSortOrder("Ascending")
            grdperson.MasterTableView.SortExpressions.AddSortExpression(expressionCompanyMembership)
        End Sub

        'Suraj Issue 14302 4/2/13 ,maintain the check box values 
        Private Sub SaveCheckedValues()
            'Dim orderdetails As New ArrayList
            Dim dicSubscriptionDetails As New Dictionary(Of Integer, List(Of String))
            Dim index As Integer = -1
            Dim result As Boolean
            Dim lstSubValueList As List(Of String)
            Dim ddlDeliveryType As DropDownList
            Dim ddlAutoRenew As DropDownList
            For Each item As Telerik.Web.UI.GridDataItem In grdperson.MasterTableView.Items
                index = CInt(DirectCast(item.FindControl("lblPersonID"), Label).Text)
                result = DirectCast(item.FindControl("chkperson"), CheckBox).Checked
                DirectCast(item.FindControl("chkperson"), CheckBox).Enabled = True
                ddlDeliveryType = DirectCast(item.FindControl("ddlMemberType"), DropDownList)
                ddlAutoRenew = DirectCast(item.FindControl("ddlAutoRenew"), DropDownList)
                If ViewState(ATTRIBUTE_CHECKED_SUBSCRIPTION) IsNot Nothing Then
                    dicSubscriptionDetails = DirectCast(ViewState(ATTRIBUTE_CHECKED_SUBSCRIPTION), Dictionary(Of Integer, List(Of String)))
                End If
                If dicSubscriptionDetails.ContainsKey(index) Then
                    dicSubscriptionDetails.Remove(index)
                End If
                lstSubValueList = New List(Of String)
                lstSubValueList.Add(ddlDeliveryType.SelectedValue)
                lstSubValueList.Add(ddlAutoRenew.SelectedValue)
                lstSubValueList.Add(CStr(result))
                dicSubscriptionDetails.Add(index, lstSubValueList)
            Next
            If dicSubscriptionDetails IsNot Nothing AndAlso dicSubscriptionDetails.Count > 0 Then
                ViewState(ATTRIBUTE_CHECKED_SUBSCRIPTION) = dicSubscriptionDetails
            End If
        End Sub
       
        Protected Sub grdperson_PageIndexChanging(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageChangedEventArgs) Handles grdperson.PageIndexChanged
            LoadPerson()
            'Suraj Issue 14450 3/23/13 ,maintain the checkedstate of values 
            SaveCheckedValues()
        End Sub
    End Class
End Namespace

