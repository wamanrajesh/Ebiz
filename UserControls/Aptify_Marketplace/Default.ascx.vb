'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports System.Data
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Telerik.Web.UI

Namespace Aptify.Framework.Web.eBusiness.MarketPlace
    Partial Class _Default
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_BROWSE_BY_PAGE As String = "BrowseByPage"
        Protected Const ATTRIBUTE_VIEW_LISTING_PAGE As String = "ViewListingPage"
        Protected Const ATTRIBUTE_NEW_LISTING_PAGE As String = "NewListingPage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "Default"
        Protected Const ATTRIBUTE_CUSTOMERSERVICELISTINGDT_VIEWSTATE As String = "CustomerServiceListingdt"



#Region "Default Specific Properties"
        ''' <summary>
        ''' BrowseBy page url
        ''' </summary>
        Public Overridable Property BrowseByPage() As String
            Get
                If Not ViewState(ATTRIBUTE_BROWSE_BY_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_BROWSE_BY_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_BROWSE_BY_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' ViewListing page url
        ''' </summary>
        Public Overridable Property ViewListingPage() As String
            Get
                If Not ViewState(ATTRIBUTE_VIEW_LISTING_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_VIEW_LISTING_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_VIEW_LISTING_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' NewListing page url
        ''' </summary>
        Public Overridable Property NewListingPage() As String
            Get
                If Not ViewState(ATTRIBUTE_NEW_LISTING_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_NEW_LISTING_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_NEW_LISTING_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
#End Region

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(BrowseByPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                BrowseByPage = Me.GetLinkValueFromXML(ATTRIBUTE_BROWSE_BY_PAGE)
                If String.IsNullOrEmpty(BrowseByPage) Then
                    Me.lnkBrowseAll.Enabled = False
                    Me.lnkBrowseListing.Enabled = False
                    Me.lnkBrowseVendor.Enabled = False
                    Me.lnkBrowseAll.ToolTip = "BrowseByPage property has not been set."
                    Me.lnkBrowseListing.ToolTip = "BrowseByPage property has not been set."
                    Me.lnkBrowseVendor.ToolTip = "BrowseByPage property has not been set."
                Else
                    'RashmiP, 8/11/2010, Removed "?" mark because it is already in BrowseByPage.
                    Me.lnkBrowseAll.NavigateUrl = BrowseByPage & "BrowseBy=All"
                    Me.lnkBrowseListing.NavigateUrl = BrowseByPage & "BrowseBy=Listing"
                    Me.lnkBrowseVendor.NavigateUrl = BrowseByPage & "BrowseBy=Vendor"
                End If
            Else
                Me.lnkBrowseAll.NavigateUrl = BrowseByPage & "BrowseBy=All"
                Me.lnkBrowseListing.NavigateUrl = BrowseByPage & "BrowseBy=Listing"
                Me.lnkBrowseVendor.NavigateUrl = BrowseByPage & "BrowseBy=Vendor"
            End If
            If String.IsNullOrEmpty(ViewListingPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                ViewListingPage = Me.GetLinkValueFromXML(ATTRIBUTE_VIEW_LISTING_PAGE)
                If String.IsNullOrEmpty(ViewListingPage) Then
                    Me.grdListings.Enabled = False
                    Me.grdListings.ToolTip = "ViewListingPage property has not been set."
                End If
            End If
            If String.IsNullOrEmpty(NewListingPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                NewListingPage = Me.GetLinkValueFromXML(ATTRIBUTE_NEW_LISTING_PAGE)
                If String.IsNullOrEmpty(NewListingPage) Then
                    Me.cmdNewLisitng.Enabled = False
                    Me.cmdNewLisitng.ToolTip = "NewListingPage property has not been set."
                End If
            End If
        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
            Try
                If Request.QueryString("BrowseBy") = "All" OrElse _
                   Request.QueryString("BrowseBy") = "" Then
                    lstBrowse.Visible = False
                Else
                    lstBrowse.Visible = True
                End If
                If Not IsPostBack() Then
                    LoadLetters()
                    'Suraj issue14454 4/5/13 ,this method use to apply the odrering of rad grid first column
                    AddExpressionMarketPlaceDefault()
                    LoadListings()
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Function LoadListings() As Boolean
            Dim sSQL As String
            Dim dt As DataTable

            Try
                'Anil B for issue 15302 on 23/04/2013
                SetProperties()
                'Suraj issue 14454 2/15/13 , check the view state is nothing or not if the page load first time viewstate will be nothing but after bostback view state will conatin the datatable
                If ViewState(ATTRIBUTE_CUSTOMERSERVICELISTINGDT_VIEWSTATE) Is Nothing Then

                    sSQL = "SELECT Company 'Vendor',0 'Select',Name 'Listing',PlainTextDescription " & _
                           "'Description',CompanyID,ID,'" & ViewListingPage & "?ID='+CONVERT(nvarchar(15),ID) 'ListingURL', " & _
                           "VendorProductURL FROM " & _
                            Database & "..vwMarketPlaceListings"

                    If Request.QueryString("BrowseBy") = "Listing" Then
                        sSQL = sSQL & " WHERE Name LIKE '" & lstBrowse.SelectedItem.Value & "%' ORDER BY Name"
                        lblDisplayTitle.Text = "Browse MarketPlace Listings - By Listing Name"
                    ElseIf Request.QueryString("BrowseBy") = "Vendor" Then
                        sSQL = sSQL & " WHERE Company LIKE '" & lstBrowse.SelectedItem.Value & "%' ORDER BY Company, Name"
                        lblDisplayTitle.Text = "Browse MarketPlace Listings - By Vendor Name"
                    Else
                        sSQL &= " ORDER BY Company, Name"
                        lblDisplayTitle.Text = "Browse MarketPlace Listings - All"
                    End If

                    dt = DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)

                    ''DirectCast(grdListings.Columns(1), Telerik.Web.UI.GridHyperLinkColumn).DataNavigateUrlFormatString = ViewListingPage & "?ID={0}"

                    UpdateDataTable(dt, "ListingURL")
                    UpdateVendorURL(dt)
                    'Suraj issue 14454 2/15/13 , if when page first time load here we store the datatable in to a viewstate
                    ViewState(ATTRIBUTE_CUSTOMERSERVICELISTINGDT_VIEWSTATE) = dt

                    grdListings.DataSource = dt
                    grdListings.DataBind()
                    If Not dt Is Nothing AndAlso _
                       dt.Rows.Count > 0 Then
                        grdListings.Visible = True
                        lblNoResults.Visible = False
                    Else
                        grdListings.Visible = False
                        lblNoResults.Visible = True

                    End If
                Else
                    'Suraj issue 14454 2/15/13 , after postback viewstate will assign for gridview
                    grdListings.DataSource = CType(ViewState(ATTRIBUTE_CUSTOMERSERVICELISTINGDT_VIEWSTATE), DataTable)
                    grdListings.DataBind()
                End If


            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Function

        Protected Sub grdListings_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdListings.NeedDataSource
            If ViewState(ATTRIBUTE_CUSTOMERSERVICELISTINGDT_VIEWSTATE) IsNot Nothing Then
                grdListings.DataSource = CType(ViewState(ATTRIBUTE_CUSTOMERSERVICELISTINGDT_VIEWSTATE), DataTable)
            End If
        End Sub

        Private Sub imgCategory_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
            'Change option images appropriately, RN 3/9/2003.
            ViewState(ATTRIBUTE_CUSTOMERSERVICELISTINGDT_VIEWSTATE) = Nothing
            LoadListings()
        End Sub

        Private Sub cmdNewLisitng_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNewLisitng.Click
            Response.Redirect(NewListingPage)
        End Sub
        Private Sub LoadLetters()
            ' load up all letters in the alphabet
            ' ASCII characters are sqeuential in coding
            ' sequence from A to Z
            Dim i As Integer
            Try
                For i = Asc("A") To Asc("Z")
                    lstBrowse.Items.Add(Chr(i))
                Next
                If lstBrowse.Items.Count > 0 Then
                    lstBrowse.Items(0).Selected = True
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub lstBrowse_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstBrowse.SelectedIndexChanged
            'LoadListings()
            'Suraj issue 14454 4/24/13 ,after postback assign Nothing to view state for getting search result
            ViewState(ATTRIBUTE_CUSTOMERSERVICELISTINGDT_VIEWSTATE) = Nothing
            LoadListings()
        End Sub

        Protected Sub UpdateDataTable(ByVal dt As Data.DataTable, ByVal columnName As String)
            ' Changes made for to allow encrypting and decrypting the URL.
            ' Changes made by Hrushikesh Jog

            Try
                Dim sURL As String
                Dim sTempURL As String
                Dim index As Integer
                Dim sValue As String
                Dim separator As String()
                For Each dr As Data.DataRow In dt.Rows
                    sURL = dr(columnName).ToString()
                    sTempURL = dr(columnName).ToString()
                    index = sTempURL.IndexOf("=")
                    sValue = sTempURL.Substring(index + 1)
                    sValue = System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(sValue))
                    separator = sURL.Split(CChar("="))
                    sURL = separator(0)
                    sURL = sURL & "="
                    sURL = sURL & sValue
                    dr(columnName) = sURL
                Next
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub UpdateVendorURL(ByVal dt As Data.DataTable)
            ' Changes made for to allow encrypting and decrypting the URL.
            ' Changes made by Hrushikesh Jog

            Try
                'ProperURL(CStr(dt.Rows(0).Item("VendorProductURL")), URLType.HTTP)
                Dim sURL As String
                Dim columnname As String = "VendorProductURL"
                For Each dr As Data.DataRow In dt.Rows
                    sURL = dr(columnname).ToString()
                    dr(columnname) = ProperURL(sURL, URLType.HTTP)
                Next
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Enum URLType
            HTTP
            MailTo
        End Enum

        Private Function ProperURL(ByVal url As String, ByVal type As URLType) As String
            Dim sPrefix As String = ""
            Select Case type
                Case URLType.HTTP : sPrefix = "http://"
                Case URLType.MailTo : sPrefix = "mailto:"
            End Select

            If Not url.StartsWith(sPrefix) Then
                Return sPrefix & url
            Else
                Return url
            End If
        End Function

        ''' <summary>
        ''' Nalini 12436 date:1/12/2011
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub grdListings_PageIndexChanging(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageChangedEventArgs) Handles grdListings.PageIndexChanged
            ''grdListings.PageIndex = e.NewPageIndex
            LoadListings()
        End Sub
        Protected Sub grdListings_PageIndexChanging(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageSizeChangedEventArgs) Handles grdListings.PageSizeChanged
            ''grdListings.PageIndex = e.NewPageIndex
            LoadListings()
        End Sub
        'Suraj Issue 14454 4/5/13 ,if the grid load first time By default the sorting will be Ascending for column  
        Private Sub AddExpressionMarketPlaceDefault()
            Dim expressionCompanyMembership As New GridSortExpression
            expressionCompanyMembership.FieldName = "Vendor"
            expressionCompanyMembership.SetSortOrder("Ascending")
            grdListings.MasterTableView.SortExpressions.AddSortExpression(expressionCompanyMembership)
        End Sub
    End Class
End Namespace
