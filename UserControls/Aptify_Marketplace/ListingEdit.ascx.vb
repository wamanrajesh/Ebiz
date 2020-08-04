'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports System.Data
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Applications.OrderEntry

Namespace Aptify.Framework.Web.eBusiness.MarketPlace

    ''' <summary>
    ''' The purpose of this User Control is to allow a user to edit information related
    ''' to a new or existing MarketPlace listing.  
    ''' </summary>
    ''' <remarks></remarks>
    Partial Class ListingEdit
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "ListingEdit"
        Public Event ListingTypeChanged()

        Protected Overrides Sub SetProperties()
            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()
        End Sub

 
        Public Function SetNewListing() As Boolean
            'This function should clean out all existing fields to ensure that 
            'no cached information remains.  

            Try
                lblCompanyName.Text = User1.Company
                lblCompanyContact.Text = User1.FirstName & " " & User1.LastName
                PopulateListingTypes()
                PopulateCategories()
                PopulateOfferingTypes()
                cboListingType.Enabled = True
                txtListingName.Text = ""
                'txtHTMLDescription.Text = ""
                txtDescription.Text = ""
                txtVendorURL.Text = ""
                txtEmail.Text = ""
                SetNewListing = True
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Function

        Public Function LoadListing(ByVal ListingID As Long) As Boolean
            'This function is responsible for Loading up an existing marketplace listing record.
            'We have been passed a GE object, and now must load up the relevant fields
            'AND disable those fields which cannot be logically changed w/o impacting pricing
            'or some other attribute driving pricing.  RN 3/12/2003.

            Dim oItem As System.Web.UI.WebControls.ListItem
            Dim oMPListing As AptifyGenericEntityBase

            Try
                oMPListing = AptifyApplication.GetEntityObject("MarketPlace Listings", ListingID)
                cboListingType.Enabled = False

                lblCompanyName.Text = User1.Company
                lblCompanyContact.Text = User1.FirstName & " " & User1.LastName
                PopulateListingTypes()
                PopulateCategories()
                PopulateOfferingTypes()

                With oMPListing
                    txtListingName.Text = CStr(.GetValue("Name"))
                    txtEmail.Text = CStr(.GetValue("RequestInfoEmail"))
                    txtVendorURL.Text = CStr(.GetValue("VendorProductURL"))
                    txtDescription.Text = CStr(.GetValue("PlainTextDescription"))
                    'txtHTMLDescription.Text = CStr(.GetValue("HTMLDescription"))

                    oItem = cboListingType.Items.FindByValue(CStr(.GetValue("ListingTypeID")))
                    If Not oItem Is Nothing Then oItem.Selected = True

                    oItem = cboCategories.Items.FindByValue(CStr(.GetValue("CategoryID")))
                    If Not oItem Is Nothing Then oItem.Selected = True

                    oItem = cboOfferingType.Items.FindByValue(CStr(.GetValue("OfferingType")))
                    If Not oItem Is Nothing Then oItem.Selected = True
                End With

                Return True
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Function



        ''' <summary>
        ''' The purpose of this function is to populate a dropdown control
        ''' with all possible marketplace listing offering types. 
        ''' </summary>
        ''' <remarks></remarks>
        Protected Overridable Sub PopulateOfferingTypes()
            Dim sSQL As String
            Dim dt As DataTable

            Try
                sSQL = "SELECT efv.Value 'Value' FROM " & Database & "..vwEntityFieldValues efv " & _
                        "INNER JOIN " & Database & "..vwEntityFields ef  ON ef.EntityID=efv.EntityID AND ef.Sequence=efv.FieldSequence " & _
                        "INNER JOIN " & Database & "..vwEntities e ON e.ID=ef.EntityID " & _
                        "WHERE ef.Name = 'OfferingType' and e.Name = 'MarketPlace Listings'"

                dt = DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                cboOfferingType.DataSource = dt
                cboOfferingType.DataValueField = "Value"
                cboOfferingType.DataTextField = "Value"
                cboOfferingType.DataBind()
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        ''' <summary>
        ''' The purpose of this function is to populate a dropdown control with all possible MarketPlace Categories.   
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub PopulateCategories()
            Dim sSQL As String
            Dim dt As DataTable

            Try
                sSQL = "SELECT ID,Name FROM " & Database & _
                       "..vwMarketPlaceCategories"
                dt = DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                cboCategories.DataSource = dt
                cboCategories.DataTextField = "Name"
                cboCategories.DataValueField = "ID"
                cboCategories.DataBind()
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub


        ''' <summary>
        ''' The purpose of this function is to obtain a list of all
        ''' Marketplace Listing Types along with the appropriate price
        ''' for the linked product based on the user/company customer
        ''' type information.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub PopulateListingTypes()
            Dim sSQL As String
            Dim dt As DataTable
            Dim i As Integer
            Dim oPrice As IProductPrice.PriceInfo

            Try
                sSQL = "SELECT ID,Name,ProductID,'' NameWPrice FROM " & _
                       Database & _
                       "..vwMarketPlaceListingTypes "

                dt = DataAction.GetDataTable(sSQL)

                For i = 0 To dt.Rows.Count - 1
                    oPrice = Me.GetProductPrice(CLng(dt.Rows(i).Item("ProductID")))
                    dt.Rows(i).Item("NameWPrice") = CStr(dt.Rows(i).Item("Name")) & _
                                                    ":  " & Format(oPrice.Price, ShoppingCart1.GetCurrencyFormat(oPrice.CurrencyTypeID))
                Next

                With cboListingType
                    .DataSource = dt
                    .DataTextField = "NameWPrice"
                    .DataValueField = "ID"
                    .DataBind()
                End With
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Function GetProductPrice(ByVal ProductID As Long) As IProductPrice.PriceInfo
            Return ShoppingCart1.GetUserProductPrice(ProductID, 1)
        End Function

        Public Property Name() As String
            Get
                EnsureChildControls()
                Return txtListingName.Text
            End Get
            Set(ByVal Value As String)
                EnsureChildControls()
                txtListingName.Text = Value
            End Set
        End Property

        Public Property ListingTypeID() As Long
            Get
                EnsureChildControls()
                Return CLng(cboListingType.SelectedItem.Value)
            End Get
            Set(ByVal Value As Long)
                Dim oItem As System.Web.UI.WebControls.ListItem
                oItem = cboListingType.Items.FindByValue(CStr(Value))
                If Not oItem Is Nothing Then oItem.Selected = True
            End Set
        End Property

        Public Property CategoryID() As Long
            Get
                EnsureChildControls()
                Return CLng(cboCategories.SelectedItem.Value)
            End Get
            Set(ByVal Value As Long)
                Dim oItem As System.Web.UI.WebControls.ListItem
                oItem = cboCategories.Items.FindByValue(CStr(Value))
                If Not oItem Is Nothing Then oItem.Selected = True
            End Set
        End Property

        Public Property OfferingType() As String
            Get
                EnsureChildControls()
                Return cboOfferingType.SelectedItem.Value
            End Get
            Set(ByVal Value As String)
                Dim oItem As System.Web.UI.WebControls.ListItem
                oItem = cboOfferingType.Items.FindByValue(Value)
                If Not oItem Is Nothing Then oItem.Selected = True
            End Set
        End Property

        Public Property RequestInfoEmail() As String
            Get
                EnsureChildControls()
                Return txtEmail.Text
            End Get
            Set(ByVal Value As String)
                txtEmail.Text = Value
            End Set
        End Property

        Public Property VendorProductURL() As String
            Get
                EnsureChildControls()
                Return txtVendorURL.Text
            End Get
            Set(ByVal Value As String)
                txtVendorURL.Text = Value
            End Set
        End Property

        Public Property Description() As String
            Get
                EnsureChildControls()
                Return txtDescription.Text
            End Get
            Set(ByVal Value As String)
                txtDescription.Text = Value
            End Set
        End Property

        Public Property HTMLDescription() As String
            Get
                'EnsureChildControls()
                'Return txtHTMLDescription.Text
                Return ""
            End Get
            Set(ByVal Value As String)
                'txtHTMLDescription.Text = Value
            End Set
        End Property

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'set control properties from XML file if needed
            SetProperties()
        End Sub

        ''RashmiP, issue 6781, 02/02/11
        Protected Sub cboListingType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboListingType.SelectedIndexChanged
            RaiseEvent ListingTypeChanged()
        End Sub

    End Class
End Namespace
