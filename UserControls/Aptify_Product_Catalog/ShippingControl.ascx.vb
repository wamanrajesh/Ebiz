'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports System.Data
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.Application

Namespace Aptify.Framework.Web.eBusiness.ProductCatalog
    Partial Class ShippingControl
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CHANGE_ADDRESS_IMAGE_URL As String = "ChangeAddressImage"
        Protected Const ATTRIBUTE_SHIPPING_CHANGE_ADDRESS_PAGE As String = "ShippingChangeAddressPage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "ShippingControl"
        Protected Const ATTRIBUTE_SHIPTO_ADDRESS_PAGE As String = "ChangeShipToAddressPage"

#Region "ShippingControl Specific Properties"
        ''' <summary>
        ''' ChangeAddressImage url
        ''' </summary>
        Public Overridable Property ChangeAddressImage() As String
            Get
                If Not ViewState(ATTRIBUTE_CHANGE_ADDRESS_IMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_CHANGE_ADDRESS_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_CHANGE_ADDRESS_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' ShippingChangeAddress page url
        ''' </summary>
        Public Overridable Property ShippingChangeAddressPage() As String
            Get
                If Not ViewState(ATTRIBUTE_SHIPPING_CHANGE_ADDRESS_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_SHIPPING_CHANGE_ADDRESS_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_SHIPPING_CHANGE_ADDRESS_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' NewAddress page url
        ''' </summary>
        Public Overridable Property ChangeShipToAddress() As String
            Get
                If Not ViewState(ATTRIBUTE_SHIPTO_ADDRESS_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_SHIPTO_ADDRESS_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_SHIPTO_ADDRESS_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
#End Region
        '<%--Nalini Issue#12578--%>
        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(ChangeAddressImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                ChangeAddressImage = Me.GetLinkValueFromXML(ATTRIBUTE_CHANGE_ADDRESS_IMAGE_URL)
            End If

            If String.IsNullOrEmpty(ShippingChangeAddressPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                ShippingChangeAddressPage = Me.GetLinkValueFromXML(ATTRIBUTE_SHIPPING_CHANGE_ADDRESS_PAGE)
                'Me.lnkChangeAddress.ImageUrl = ChangeAddressImage
                If String.IsNullOrEmpty(ShippingChangeAddressPage) Then
                    Me.lnkChangeAddress.Enabled = False
                    Me.lnkChangeAddress.ToolTip = "ShippingChangeAddressPage property has not been set."
                Else
                    Me.lnkChangeAddress.PostBackUrl = ShippingChangeAddressPage & "?Type=Shipping"
                End If
            Else
                Me.lnkChangeAddress.PostBackUrl = ShippingChangeAddressPage & "?Type=Shipping"
            End If
            If String.IsNullOrEmpty(ChangeShipToAddress) Then
                ChangeShipToAddress = Me.GetLinkValueFromXML(ATTRIBUTE_SHIPTO_ADDRESS_PAGE)
                If UserIsGroupAdmin() Then
                    lnkChangeShipTo.Visible = True
                    lnkChangeShipTo.PostBackUrl = ChangeShipToAddress & "?Action=PersonAddress&Type=Shipping"
                    Dim oOrder As AptifyGenericEntity
                    oOrder = ShoppingCart1.GetOrderObject(Session, Page.User, Application)
                    Dim lShipToPersonId As Long = CLng(oOrder.GetValue("ShipToID"))
                    If lShipToPersonId > 0 Then
                        Me.lnkChangeAddress.PostBackUrl = ShippingChangeAddressPage & "?Type=Shipping&ShipToPersonID=" & lShipToPersonId
                    End If
                End If
            End If
        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            ''lnkChangeShipTo.Visible = False
            SetProperties()
            Try
                If Not IsPostBack Then
                    If User1.UserID > 0 Then
                        ' There is a user logged in, go to the cart
                        LoadShipAddress()
                    End If
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub LoadShipAddress()
            Try
                Dim oOrder As AptifyGenericEntityBase
                oOrder = ShoppingCart1.GetOrderObject(Session, Page.User, Application)
                '02/05/08 RJK - Added back the check to see if the Ship To and Bill To Addresses have
                'been set.  Otherwise, it is not possible to change the address to a non-default Address.
                'For the 5583 Issue related to User Profile changes, the Profile page has been updated to refresh
                'the Order's Ship To And Bill To Addresses if the Address is changed on the Profile.


                If Len(oOrder.GetValue("ShipToAddrLine1")) = 0 Then
                    'LoadDefaultAddress("ShipTo", oOrder)
                    LoadDefaultAddress("ShipTo", oOrder, User1.GetValue("PreferredShippingAddress"))
                End If
                ' Vijay Sitlani - Changes made to partially resolve the bug reported by Alina for Issue 5583
                ' Changes made on 01-25-2008
                If Len(oOrder.GetValue("BillToAddrLine1")) = 0 Then
                    'LoadDefaultAddress("BillTo", oOrder)
                    LoadDefaultAddress("BillTo", oOrder, User1.GetValue("PreferredBillingAddress"))
                End If

                With User1
                    If CLng(oOrder.GetValue("ShipToID")) > 0 Then
                        NameAddressBlock.Name = CStr(oOrder.GetValue("ShipToName"))
                    Else
                        NameAddressBlock.Name = .FirstName & " " & .LastName
                    End If
                    If Len(.Company) > 0 Then
                        NameAddressBlock.Name = NameAddressBlock.Name & "/" & .Company
                    End If

                End With
                With oOrder
                    NameAddressBlock.AddressLine1 = CStr(.GetValue("ShipToAddrLine1"))
                    NameAddressBlock.AddressLine2 = CStr(.GetValue("ShipToAddrLine2"))
                    NameAddressBlock.AddressLine3 = CStr(.GetValue("ShipToAddrLine3"))
                    NameAddressBlock.City = CStr(.GetValue("ShipToCity"))
                    NameAddressBlock.State = CStr(.GetValue("ShipToState"))
                    NameAddressBlock.ZipCode = CStr(.GetValue("ShipToZipCode"))
                    NameAddressBlock.Country = CStr(.GetValue("ShipToCountry"))
                End With
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub LoadDefaultAddress(ByVal sPrefix As String, _
                                       ByRef oOrder As AptifyGenericEntityBase)
            Try
                With User1
                    oOrder.SetValue(sPrefix & "AddrLine1", .GetValue("AddressLine1"))
                    oOrder.SetValue(sPrefix & "AddrLine2", .GetValue("AddressLine2"))
                    oOrder.SetValue(sPrefix & "AddrLine3", .GetValue("AddressLine3"))
                    oOrder.SetValue(sPrefix & "City", .GetValue("City"))
                    oOrder.SetValue(sPrefix & "State", .GetValue("State"))
                    oOrder.SetValue(sPrefix & "ZipCode", .GetValue("ZipCode"))
                    oOrder.SetValue(sPrefix & "Country", .GetValue("Country"))
                    oOrder.SetValue(sPrefix & "AreaCode", .GetValue("PhoneAreaCode"))
                    oOrder.SetValue(sPrefix & "Phone", .GetValue("Phone"))
                End With
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub LoadDefaultAddress(ByVal sPrefix As String, _
                                     ByRef oOrder As AptifyGenericEntityBase, _
                                     ByVal PrefAddress As String)
            Try
                With User1
                    If PrefAddress.Trim.ToLower.Contains("home") Then
                        oOrder.SetValue(sPrefix & "AddrLine1", .GetValue("HomeAddressLine1"))
                        oOrder.SetValue(sPrefix & "AddrLine2", .GetValue("HomeAddressLine2"))
                        oOrder.SetValue(sPrefix & "AddrLine3", .GetValue("HomeAddressLine3"))
                        oOrder.SetValue(sPrefix & "City", .GetValue("HomeCity"))
                        oOrder.SetValue(sPrefix & "State", .GetValue("HomeState"))
                        oOrder.SetValue(sPrefix & "ZipCode", .GetValue("HomeZipCode"))
                        oOrder.SetValue(sPrefix & "Country", .GetValue("HomeCountry"))
                    ElseIf PrefAddress.Trim.ToLower.Contains("business") Then
                        oOrder.SetValue(sPrefix & "AddrLine1", .GetValue("AddressLine1"))
                        oOrder.SetValue(sPrefix & "AddrLine2", .GetValue("AddressLine2"))
                        oOrder.SetValue(sPrefix & "AddrLine3", .GetValue("AddressLine3"))
                        oOrder.SetValue(sPrefix & "City", .GetValue("City"))
                        oOrder.SetValue(sPrefix & "State", .GetValue("State"))
                        oOrder.SetValue(sPrefix & "ZipCode", .GetValue("ZipCode"))
                        oOrder.SetValue(sPrefix & "Country", .GetValue("Country"))
                    ElseIf PrefAddress.Trim.ToLower.Contains("billing") Then
                        oOrder.SetValue(sPrefix & "AddrLine1", .GetValue("BillingAddressLine1"))
                        oOrder.SetValue(sPrefix & "AddrLine2", .GetValue("BillingAddressLine2"))
                        oOrder.SetValue(sPrefix & "AddrLine3", .GetValue("BillingAddressLine3"))
                        oOrder.SetValue(sPrefix & "City", .GetValue("BillingCity"))
                        oOrder.SetValue(sPrefix & "State", .GetValue("BillingState"))
                        oOrder.SetValue(sPrefix & "ZipCode", .GetValue("BillingZipCode"))
                        oOrder.SetValue(sPrefix & "Country", .GetValue("BillingCountry"))
                    ElseIf PrefAddress.Trim.ToLower.Contains("po") Then
                        oOrder.SetValue(sPrefix & "AddrLine1", .GetValue("POBox"))
                        oOrder.SetValue(sPrefix & "AddrLine2", .GetValue("POBoxAddressLine2"))
                        oOrder.SetValue(sPrefix & "AddrLine3", .GetValue("POBoxAddressLine3"))
                        oOrder.SetValue(sPrefix & "City", .GetValue("POBoxCity"))
                        oOrder.SetValue(sPrefix & "State", .GetValue("POBoxState"))
                        oOrder.SetValue(sPrefix & "ZipCode", .GetValue("POBoxZipCode"))
                        oOrder.SetValue(sPrefix & "Country", .GetValue("POBoxCountry"))
                    Else
                        oOrder.SetValue(sPrefix & "AddrLine1", .GetValue("AddressLine1"))
                        oOrder.SetValue(sPrefix & "AddrLine2", .GetValue("AddressLine2"))
                        oOrder.SetValue(sPrefix & "AddrLine3", .GetValue("AddressLine3"))
                        oOrder.SetValue(sPrefix & "City", .GetValue("City"))
                        oOrder.SetValue(sPrefix & "State", .GetValue("State"))
                        oOrder.SetValue(sPrefix & "ZipCode", .GetValue("ZipCode"))
                        oOrder.SetValue(sPrefix & "Country", .GetValue("Country"))
                    End If

                    oOrder.SetValue(sPrefix & "AreaCode", .GetValue("PhoneAreaCode"))
                    oOrder.SetValue(sPrefix & "Phone", .GetValue("Phone"))
                End With
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Private Function UserIsGroupAdmin() As Boolean
            Try
                Dim oGESPM As CommonMethods = New CommonMethods(DataAction)
                Return oGESPM.UserIsGroupAdmin(User1.PersonID)
            Catch ex As Exception
                Return False
            End Try
        End Function
    End Class
End Namespace
