'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports Aptify.Framework.Application
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.BusinessLogic.Security
Imports System.Data

Namespace Aptify.Framework.Web.eBusiness.ProductCatalog
    Partial Class FeaturedProductsControl
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_VIEW_PRODUCT_PAGE As String = "ViewProductPage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "FeaturedProducts"
        'Suvarna D IssueID-12745 to implement webImage feature to display images on Jan 19, 2012
        Protected Const ATTRIBUTE_IMAGE_NOT_AVAILABLE_URL As String = "ImageNotAvailable"
        'End of addition by Suvarna D IssueID-12745 to implement webImage feature to display images on Jan 19, 2012

#Region "FeaturedProducts Specific Properties"
        ''' <summary>
        ''' ViewProduct page url
        ''' </summary>
        Public Overridable Property ViewProductPage() As String
            Get
                If Not ViewState(ATTRIBUTE_VIEW_PRODUCT_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_VIEW_PRODUCT_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_VIEW_PRODUCT_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        ''' <summary>
        ''' 'addition by Suvarna D IssueID-12745 to implement webImage feature to display images
        ''' </summary>
        Public Overridable Property ImageNotAvailable() As String
            Get
                If Not ViewState(ATTRIBUTE_IMAGE_NOT_AVAILABLE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_IMAGE_NOT_AVAILABLE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_IMAGE_NOT_AVAILABLE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''End of addition by Suvarna D IssueID-12745 to implement webImage feature to display images

#End Region

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(ViewProductPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                ViewProductPage = Me.GetLinkValueFromXML(ATTRIBUTE_VIEW_PRODUCT_PAGE)
                If String.IsNullOrEmpty(ViewProductPage) Then
                    Me.grdFeaturedProducts.Enabled = False
                    Me.grdFeaturedProducts.ToolTip = "ViewProductPage property has not been set."
                End If
            End If

            ''Addition by Suvarna D IssueID-12745 to implement webImage feature to display images on Jan 19, 2012
            If String.IsNullOrEmpty(ImageNotAvailable) Then
                'since value is the 'default' check the XML file for possible custom setting
                ImageNotAvailable = Me.GetLinkValueFromXML(ATTRIBUTE_IMAGE_NOT_AVAILABLE_URL)
            End If
            'End of addition by Suvarna D IssueID-12745 to implement webImage feature to display images on Jan 19, 2012
        End Sub


        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
            Try
                If Not IsPostBack() Then
                    'Dim sSQL As String
                    'Dim dt As DataTable

                    '' changed to use Config Settings for virtual directory location
                    'sSQL = " Exec " + Database.ToString + "..spGetFeaturedProductsWeb @WebUserID = " & User1.UserID

                    'dt = DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)
                    ''Navin Prasad Issue 11032

                    '' DirectCast(grdFeaturedProducts.Columns(0), HyperLinkColumn).DataNavigateUrlFormatString = ViewProductPage & "?ID={0}"

                    ''HP - Per issue 8222, do not display grid if empty 
                    'If dt.Rows.Count > 0 Then
                    '    grdFeaturedProducts.DataSource = dt
                    '    grdFeaturedProducts.DataBind()

                    '    'Navin Prasad Issue 11032

                    '    For Each row As GridViewRow In grdFeaturedProducts.Rows
                    '        Dim lnk As HyperLink = CType(row.FindControl("lnkName"), HyperLink)
                    '        lnk.NavigateUrl = String.Format(ViewProductPage & "?ID={0}", dt.Rows(row.RowIndex)("ID").ToString)
                    '    Next

                    '    grdFeaturedProducts.Visible = True
                    '    divGrid.Visible = True
                    '    noData.Visible = False
                    'Else
                    '    divGrid.Visible = False
                    '    noData.Visible = True
                    'End If
                    FeaturedProductsFill()
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        ''' <summary>
        ''' Added FeaturedProductsFill() function for grdFeaturedProducts fill,Updated By Nalini 
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub FeaturedProductsFill()
            Dim sSQL As String
            Dim dt As DataTable
            'Neha Changes for Issue 18891,27/05/2014
            Dim sSpName As String = String.Empty
            Try
                If Not String.IsNullOrEmpty(AptifyApplication.GetEntityAttribute("Web Shopping Carts", "GetFeaturedProductsStoredProcedure")) Then
                    sSpName = CStr(AptifyApplication.GetEntityAttribute("Web Shopping Carts", "GetFeaturedProductsStoredProcedure"))
                Else
                    sSpName = "spGetFeaturedProductsWeb"
                End If
                sSQL = " Exec " + Database.ToString + ".." + sSpName + " @WebUserID = " & User1.UserID

                dt = DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)
                'Navin Prasad Issue 11032

                ' DirectCast(grdFeaturedProducts.Columns(0), HyperLinkColumn).DataNavigateUrlFormatString = ViewProductPage & "?ID={0}"

                'HP - Per issue 8222, do not display grid if empty 
                If dt.Rows.Count > 0 Then
                    grdFeaturedProducts.DataSource = dt
                    grdFeaturedProducts.DataBind()

                    'Suvarna Deshmukh IssueID-12433,12430 and 12434 On Dec 13,2011
                    'commented and added new code to provide navigation url in datalist item
                    'Navin Prasad Issue 11032
                    'For Each row As GridViewRow In grdFeaturedProducts.Rows
                    '    Dim lnk As HyperLink = CType(row.FindControl("lnkName"), HyperLink)
                    'lnk.NavigateUrl = String.Format(ViewProductPage & "?ID={0}", dt.Rows(row.RowIndex)("ID").ToString)
                    'Next

                    For Each item As DataListItem In grdFeaturedProducts.Items
                        Dim itemID As Integer = item.ItemIndex
                        'Dim itemID As Integer = CInt(grdFeaturedProducts.DataKeys(index).ToString())

                        Dim lnk As HyperLink = CType(item.FindControl("lnkName"), HyperLink)
                        ''addition by Suvarna D IssueID-12745 to implement webImage feature to display images on Jan 19, 2012
                        Dim iImg As Image = CType(item.FindControl("ImgProd"), Image)
                        ''End of addition by Suvarna D IssueID-12745 to implement webImage feature to display images on Jan 19, 2012
                        lnk.NavigateUrl = String.Format(ViewProductPage & "?ID={0}", dt.Rows(itemID)("ID").ToString)

                        'Addition by Suvarna D IssueID-12745 to implement webImage feature to display images on Jan 19, 2012
                        If Not IsDBNull(dt.Rows(itemID)("ProdImageURL")) AndAlso _
                                Len(dt.Rows(itemID)("ProdImageURL")) > 0 Then
                            iImg.ImageUrl = CStr(dt.Rows(itemID)("ProdImageURL"))
                        Else
                            iImg.ImageUrl = ImageNotAvailable
                        End If
                        'End of addition by Suvarna D IssueID-12745 to implement webImage feature to display images on Jan 19, 2012
                    Next
                    'End of Addition by Suvarna Deshmukh IssueID-12433,12430 and 12434 On Dec 13,2011


                    grdFeaturedProducts.Visible = True
                    divGrid.Visible = True
                    noData.Visible = False
                Else
                    divGrid.Visible = False
                    noData.Visible = True
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
 
    End Class
End Namespace
