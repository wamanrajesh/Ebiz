'Aptify e-Business 5.5.1, July 2013
Option Explicit On

Imports System.Data
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.Application

Namespace Aptify.Framework.Web.eBusiness.ProductCatalog
    Partial Class ChangeAddressControl
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_USE_ADDRESS_IMAGE_URL As String = "UseAddressImageUrl"
        Protected Const ATTRIBUTE_EDIT_ADDRESS_IMAGE_URL As String = "EditAddressImageUrl"
        Protected Const ATTRIBUTE_EDIT_ADDRESS_PAGE As String = "EditAddressPage"
        Protected Const ATTRIBUTE_NEW_ADDRESS_PAGE As String = "NewAddressPage"
        Protected Const ATTRIBUTE_DEFAULT_RETURN_PAGE As String = "DefaultReturnPage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "ChangeAddress"
        Protected Const ATTRIBUTE_USER_ADDRESS As String = "dtUserAddress"

#Region "ChapterManagementControl Specific Properties"
        ''' <summary>
        ''' UseAddressImage url
        ''' </summary>
        Public Overridable Property UseAddressImageUrl() As String
            Get
                If Not ViewState(ATTRIBUTE_USE_ADDRESS_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_USE_ADDRESS_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_USE_ADDRESS_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' EditAddressImage url
        ''' </summary>
        Public Overridable Property EditAddressImageUrl() As String
            Get
                If Not ViewState(ATTRIBUTE_EDIT_ADDRESS_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_EDIT_ADDRESS_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_EDIT_ADDRESS_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' EditAddress page url
        ''' </summary>
        Public Overridable Property EditAddressPage() As String
            Get
                If Not ViewState(ATTRIBUTE_EDIT_ADDRESS_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_EDIT_ADDRESS_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_EDIT_ADDRESS_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' NewAddress page url
        ''' </summary>
        Public Overridable Property NewAddressPage() As String
            Get
                If Not ViewState(ATTRIBUTE_NEW_ADDRESS_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_NEW_ADDRESS_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_NEW_ADDRESS_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' DefaultReturn page url
        ''' </summary>
        Public Overridable Property DefaultReturnPage() As String
            Get
                If Not ViewState(ATTRIBUTE_DEFAULT_RETURN_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_DEFAULT_RETURN_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_DEFAULT_RETURN_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
#End Region

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(UseAddressImageUrl) Then
                'since value is the 'default' check the XML file for possible custom setting
                UseAddressImageUrl = Me.GetLinkValueFromXML(ATTRIBUTE_USE_ADDRESS_IMAGE_URL)
            End If
            If String.IsNullOrEmpty(EditAddressImageUrl) Then
                'since value is the 'default' check the XML file for possible custom setting
                EditAddressImageUrl = Me.GetLinkValueFromXML(ATTRIBUTE_EDIT_ADDRESS_IMAGE_URL)
            End If
            If String.IsNullOrEmpty(EditAddressPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                EditAddressPage = Me.GetLinkValueFromXML(ATTRIBUTE_EDIT_ADDRESS_PAGE)
            End If
            If String.IsNullOrEmpty(DefaultReturnPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                DefaultReturnPage = Me.GetLinkValueFromXML(ATTRIBUTE_DEFAULT_RETURN_PAGE)
            End If
            If String.IsNullOrEmpty(NewAddressPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                NewAddressPage = Me.GetLinkValueFromXML(ATTRIBUTE_NEW_ADDRESS_PAGE)
                If String.IsNullOrEmpty(EditAddressPage) Then
                    Me.lnkNewAddress.Enabled = False
                    Me.lnkNewAddress.ToolTip = "NewAddressPage property has not been set."
                Else
                    Me.lnkNewAddress.NavigateUrl = EditAddressPage
                End If
            Else
                Me.lnkNewAddress.NavigateUrl = EditAddressPage
            End If
        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
            If Not IsPostBack Then
                lblType.Text = Request.QueryString("Type")
                lnkNewAddress.NavigateUrl = NewAddressPage & "?Action=New&Type=" & lblType.Text
                LoadAddresses()
            End If
        End Sub

        Private Function LoadTable() As DataTable

            Try

                Dim dt As New DataTable()
                If ViewState(ATTRIBUTE_USER_ADDRESS) IsNot Nothing Then
                    dt = CType(ViewState(ATTRIBUTE_USER_ADDRESS), DataTable)
                    Return dt
                End If
                ' This routine loads up all of the users addresses into 
                ' a single data table and then binds the list to the 
                ' addresses for the user to select from
                ConfigTable(dt) ' Set up the table columns

                ' First, load up all of the addresses from the vwPersons
                ' base view 
                LoadUserAddresses(dt)
                LoadPersonAddresses(dt)
                ViewState(ATTRIBUTE_USER_ADDRESS) = dt
                Return dt
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return Nothing
            End Try
        End Function
        '<%--Nalini Issue#12578--%>
        Private Sub LoadAddresses()
            lstAddress.DataSource = LoadTable()
            lstAddress.DataBind()

            ' find all of the images generated by the templated
            ' listign control and set the image url property so
            ' it they do not use a hard-coded path

        End Sub

        Private Sub LoadPersonAddresses(ByVal dt As DataTable)
            Dim sSQL As String
            Dim dr As DataRow, dtRead As DataTable

            Try
                ''Person ID has been added to SQL query for issueId - 21249
                sSQL = "SELECT pa.*, a.CountryCodeID AddressCountryCodeID FROM " & Database & _
                       "..vwPersonAddresses pa INNER JOIN " & Database & "..vwAddresses a ON pa.AddressID = a.ID WHERE PersonID = " & _
                       User1.PersonID & " ORDER BY Sequence"

                dtRead = DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)
                For Each drRead As DataRow In dtRead.Rows
                    'Dim brCount As Integer = 0
                    dr = dt.NewRow()
                    dr.Item("AddressID") = drRead.Item("AddressID")
                    dr.Item("Type") = "PersonAddress"
                    dr.Item("Sequence") = drRead.Item("Sequence")
                    dr.Item("AddressLine1") = drRead.Item("AddressLine1") & "<br>"
                    'dr.Item("AddressLine2") = drRead.Item("AddressLine2")
                    If (Len(drRead.Item("AddressLine2")) > 0) Then
                        dr.Item("AddressLine2") = drRead.Item("AddressLine2") & "<br>"
                    End If
                    'dr.Item("AddressLine3") = drRead.Item("AddressLine3")
                    If (Len(drRead.Item("AddressLine3")) > 0) Then
                        dr.Item("AddressLine3") = drRead.Item("AddressLine3") & "<br>"
                    End If
                    'Nalini Issue#13102
                    If (drRead.Item("City") <> "") Then
                        dr.Item("City") = drRead.Item("City") & ","
                    Else
                        dr.Item("City") = drRead.Item("City")
                    End If
                    'dr.Item("City") = drRead.Item("City") & ","
                    dr.Item("State") = drRead.Item("State")
                    dr.Item("ZipCode") = drRead.Item("ZipCode")
                    dr.Item("Country") = drRead.Item("Country")
                    dr.Item("CountryCodeID") = drRead.Item("AddressCountryCodeID")

                    dt.Rows.Add(dr)
                Next
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub LoadUserAddresses(ByRef dt As DataTable)

            Try
                Dim lMainAddressID As Long = -1
                Dim lHomeAddressID As Long = -1
                Dim lBillingAddressID As Long = -1
                Dim lPOBoxAddressID As Long = -1

                If IsNumeric(User1.GetValue("AddressID")) Then
                    lMainAddressID = CLng(User1.GetValue("AddressID"))
                End If

                If IsNumeric(User1.GetValue("HomeAddressID")) Then
                    lHomeAddressID = CLng(User1.GetValue("HomeAddressID"))
                End If

                If IsNumeric(User1.GetValue("BillingAddressID")) Then
                    lBillingAddressID = CLng(User1.GetValue("BillingAddressID"))
                End If

                If IsNumeric(User1.GetValue("POBoxAddressID")) Then
                    lPOBoxAddressID = CLng(User1.GetValue("POBoxAddressID"))
                End If


                Dim sSQL As String = "SELECT * FROM " & Database & "..vwAddresses WHERE ID IN ("
                Dim dr As DataRow, dtRead As DataTable
                Dim bProcessedMain As Boolean = False
                Dim bProcessedHome As Boolean = False
                Dim bProcessedBilling As Boolean = False
                Dim bProcessedPOBox As Boolean = False

                If lMainAddressID > 0 Then
                    sSQL &= lMainAddressID.ToString & ","
                End If

                If lHomeAddressID > 0 Then
                    sSQL &= lHomeAddressID.ToString & ","
                End If

                If lBillingAddressID > 0 Then
                    sSQL &= lBillingAddressID.ToString & ","
                End If

                If lPOBoxAddressID > 0 Then
                    sSQL &= lPOBoxAddressID.ToString & ","
                End If

                sSQL = sSQL.Substring(0, sSQL.Length - 1) & ")"

                dtRead = DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)
                For Each drRead As DataRow In dtRead.Rows
                    'Dim brCount As Integer = 0
                    If drRead.Item("Line1") IsNot Nothing AndAlso _
                            Not IsDBNull(drRead.Item("Line1")) AndAlso _
                            drRead.Item("Line1").ToString.Length > 0 Then

                        dr = dt.NewRow()
                        dr.Item("AddressID") = drRead.Item("ID")

                        If Not bProcessedMain AndAlso lMainAddressID = CLng(drRead.Item("ID")) Then
                            dr.Item("Type") = "Main"
                            bProcessedMain = True
                        ElseIf Not bProcessedHome AndAlso lHomeAddressID = CLng(drRead.Item("ID")) Then
                            dr.Item("Type") = "Home"
                            bProcessedHome = True
                        ElseIf Not bProcessedBilling AndAlso lBillingAddressID = CLng(drRead.Item("ID")) Then
                            dr.Item("Type") = "Billing"
                            bProcessedBilling = True
                        ElseIf Not bProcessedPOBox AndAlso lPOBoxAddressID = CLng(drRead.Item("ID")) Then
                            dr.Item("Type") = "POBox"
                            bProcessedPOBox = True
                        End If

                        dr.Item("Sequence") = 0
                        dr.Item("AddressLine1") = drRead.Item("Line1") & "<br>"
                        'dr.Item("AddressLine2") = drRead.Item("AddressLine2")
                        If (Len(drRead.Item("Line2")) > 0) Then
                            dr.Item("AddressLine2") = drRead.Item("Line2") & "<br>"
                        End If
                        'dr.Item("AddressLine3") = drRead.Item("AddressLine3")
                        If (Len(drRead.Item("Line3")) > 0) Then
                            dr.Item("AddressLine3") = drRead.Item("Line3") & "<br>"
                        End If
                        'Nalini Issue#13102
                        If (drRead.Item("City") <> "") Then
                            dr.Item("City") = drRead.Item("City") & ","
                        Else
                            dr.Item("City") = drRead.Item("City")
                        End If
                        'dr.Item("City") = drRead.Item("City") & ","
                        dr.Item("State") = drRead.Item("StateProvince")
                        dr.Item("ZipCode") = drRead.Item("PostalCode")
                        dr.Item("Country") = drRead.Item("Country")
                        dr.Item("CountryCodeID") = drRead.Item("CountryCodeID")

                        dt.Rows.Add(dr)
                    End If
                Next



            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        'Private Sub AddAddress(ByVal sType As String, _
        '                       ByVal sPrefix As String, _
        '                       ByRef dt As DataTable)
        '    Dim dr As DataRow

        '    Try
        '        If Len(User1.GetValue(sPrefix & "AddressLine1")) > 0 Then
        '            Dim brCount As Integer = 0
        '            ' only add addresses that are not blank
        '            dr = dt.NewRow()
        '            dr.Item("Type") = sType
        '            dr.Item("AddressLine1") = User1.GetValue(sPrefix & "AddressLine1") & "<br>"
        '            'dr.Item("AddressLine2") = User1.GetValue(sPrefix & "AddressLine2") & _
        '            '                          CStr(IIf(Len(Trim$(User1.GetValue(sPrefix & "AddressLine2"))) > 0, "<BR>", ""))
        '            If (Len(User1.GetValue("AddressLine2")) > 0) Then
        '                dr.Item("AddressLine2") = User1.GetValue("AddressLine2") & "<br>"
        '            End If
        '            'dr.Item("AddressLine3") = User1.GetValue(sPrefix & "AddressLine3") & _
        '            '                          CStr(IIf(Len(Trim$(User1.GetValue(sPrefix & "AddressLine3"))) > 0, "<BR>", ""))
        '            If (Len(User1.GetValue("AddressLine3")) > 0) Then
        '                dr.Item("AddressLine3") = User1.GetValue("AddressLine3") & "<br>"
        '            End If
        '            dr.Item("City") = User1.GetValue(sPrefix & "City")
        '            dr.Item("State") = User1.GetValue(sPrefix & "State")
        '            dr.Item("ZipCode") = User1.GetValue(sPrefix & "ZipCode")
        '            dr.Item("Country") = User1.GetValue(sPrefix & "Country")
        '            dt.Rows.Add(dr)
        '        End If
        '    Catch ex As Exception
        '        Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        '    End Try
        'End Sub

        Private Sub ConfigTable(ByRef dt As DataTable)
            Try
                dt.Columns.Add("AddressID")
                dt.Columns.Add("Type")
                dt.Columns.Add("Sequence") ' used for PersonAddress addresses
                dt.Columns.Add("AddressLine1")
                dt.Columns.Add("AddressLine2")
                dt.Columns.Add("AddressLine3")
                dt.Columns.Add("City")
                dt.Columns.Add("State")
                dt.Columns.Add("ZipCode")
                dt.Columns.Add("Country")
                dt.Columns.Add("CountryCodeID")
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub


        Private Sub lstAddress_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles lstAddress.ItemCommand
            If e.CommandSource.ID = "cmdEditAddress" Then
                EditAddress(e)
            Else
                SelectAddress(e)
            End If
        End Sub
        Private Sub EditAddress(ByRef e As System.Web.UI.WebControls.DataListCommandEventArgs)
            Dim sType As String
            Dim iSequence As Integer
            Dim dt As DataTable

            Try
                dt = LoadTable()
                With dt.Rows.Item(e.Item.ItemIndex)
                    ' check which one was updated
                    sType = .Item("Type")
                    If sType = "PersonAddress" Then
                        iSequence = .Item("Sequence")
                    Else
                        iSequence = 0
                    End If
                End With
                Response.Redirect(EditAddressPage & "?Type=" & Request.QueryString("Type") & "&Action=Edit&AddressType=" & Server.UrlEncode(sType) & "&Sequence=" & iSequence)
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub SelectAddress(ByRef e As System.Web.UI.WebControls.DataListCommandEventArgs)
            Dim sType As String
            Dim iSequence As Integer
            Dim dt As DataTable

            Try
                dt = LoadTable()
                With dt.Rows.Item(e.Item.ItemIndex)
                    ' check which one was updated
                    sType = .Item("Type")
                    If sType = "PersonAddress" Then
                        iSequence = .Item("Sequence")
                    End If
                End With
                If Len(sType) > 0 Then
                    Dim iOrderAddrType As AptifyShoppingCart.OrderAddressType
                    Dim iPersonAddrType As AptifyShoppingCart.PersonAddressType
                    If lblType.Text = "Shipping" Then
                        'Added by Sandeep for Issue 5133 on 09/04/2012
                        'Update Shipping type in Order when Address change and Adress have different country
                        iOrderAddrType = AptifyShoppingCart.OrderAddressType.Shipping
                        UpdateShippingType(dt, e)
                    Else
                        iOrderAddrType = AptifyShoppingCart.OrderAddressType.Billing
                    End If
                    Select Case sType
                        Case "Main"
                            iPersonAddrType = AptifyShoppingCart.PersonAddressType.Main
                            If lblType.Text = "Shipping" Then
                                UpdateShippingType(dt, e)
                            End If
                        Case "Billing"
                            iPersonAddrType = AptifyShoppingCart.PersonAddressType.Billing
                        Case "POBox"
                            iPersonAddrType = AptifyShoppingCart.PersonAddressType.POBox
                        Case "Home"
                            iPersonAddrType = AptifyShoppingCart.PersonAddressType.Home
                        Case "PersonAddress"
                            iPersonAddrType = AptifyShoppingCart.PersonAddressType.PersonAddressSubType


                    End Select

                    ShoppingCart1.SetAddressForPerson(iOrderAddrType, iPersonAddrType, sType, User1.PersonID, iSequence, Page.Session, Page.User, Page.Application)
                    ShoppingCart1.SaveCart(Session)

                    If Request.QueryString("ReturnToPage") = "" Then
                        Response.Redirect(DefaultReturnPage)
                    Else
                        Response.Redirect(Request.QueryString("ReturnToPage"))
                    End If
                End If
            Catch ex As Threading.ThreadAbortException
                'Do Nothing
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        'Added by Sandeep for Issue 5133 on 09/04/2012
        'Update Shipping type in Order when Address change and Adress have different country
        Protected Sub UpdateShippingType(ByVal dt As DataTable, ByRef e As System.Web.UI.WebControls.DataListCommandEventArgs)
            Dim oOrder As Aptify.Applications.OrderEntry.OrdersEntity
            oOrder = ShoppingCart1.GetOrderObject(Page.Session, Page.User, Page.Application)
            Dim oShipmentTypes As New Aptify.Framework.Web.eBusiness.CommonMethods(DataAction, AptifyApplication, User1, Database)
            Dim bIncludeInShipping As Boolean
            Dim dtShippingType As DataTable = Nothing
            If oOrder IsNot Nothing Then
                For Each oOrderLine As Aptify.Applications.OrderEntry.OrderLinesEntity In oOrder.SubTypes("OrderLines")
                    bIncludeInShipping = oShipmentTypes.IncludeInShipping(CLng(oOrderLine.GetValue("ProductID")))
                    If bIncludeInShipping = True Then
                        Exit For
                    End If
                Next
                If bIncludeInShipping Then
                    If CInt(oOrder.GetValue("ShipToCountryCodeID")) <> CInt(dt.Rows.Item(e.Item.ItemIndex).Item("CountryCodeID")) Then
                        oOrder.SetValue("ShipToCountryCodeID", CInt(dt.Rows.Item(e.Item.ItemIndex).Item("CountryCodeID")))
                        dtShippingType = oShipmentTypes.LoadShipmentType(CInt(oOrder.GetValue("ShipToCountryCodeID")))
                        oOrder.SetValue("ShipTypeID", dtShippingType(0)("ID"))
                        oOrder.CalculateOrderTotals(True, True)


                    End If
                End If
            End If
        End Sub
    End Class
End Namespace
