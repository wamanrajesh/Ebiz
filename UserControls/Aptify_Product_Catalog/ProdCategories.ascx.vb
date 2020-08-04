'Aptify e-Business 5.5.1, July 2013
Option Strict On
Option Explicit On
Imports System.Data
Imports System.IO
Imports Aptify.Framework.Application
Imports Aptify.Framework.DataServices

Namespace Aptify.Framework.Web.eBusiness.ProductCatalog
    Partial Class ProdCategories
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced

        Protected Const ATTRIBUTE_PARENT_ID As String = "ParentID"
        Protected Const ATTRIBUTE_SHOW_HEADER_IF_EMPTY As String = "ShowHeaderIfEmpty"
        Protected Const ATTRIBUTE_HEADER_TEXT_PAGE As String = "HeaderText"
        Protected Const ATTRIBUTE_PRODUCT_CATEGORY_PAGE As String = "ProductCategoryPage"
        Protected Const ATTRIBUTE_PRODUCT_CATEGORY_IMAGE_URL As String = "ProductCategoryImage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "ProdCategories"
        ''Suvarna IssueID 12430, 12433 & 12434 - New Constants are added to display headers on Dec 22, 2011
        Protected Const ATTRIBUTE_HEADER_TEXT_PAGE_NAME As String = "PageHeaderText"
        Protected Const ATTRIBUTE_HEADER_WELCOME_TEXT As String = "WelcomeText"
        Protected Const ATTRIBUTE_HEADER_PRODCAT_PAGE As String = "ProdCatHeader"
        Protected Const ATTRIBUTE_IMAGE_SIDEIMAGE_URL As String = "ShowSideImage"
        ''End of addition by Suvarna



#Region "ProdCategories Specific Properties"
        ''' <summary>
        ''' ProductCategory page url
        ''' </summary>
        Public Overridable Property ProductCategoryPage() As String
            Get
                If Not ViewState(ATTRIBUTE_PRODUCT_CATEGORY_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_PRODUCT_CATEGORY_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_PRODUCT_CATEGORY_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' ProductCategoryImage url
        ''' </summary>
        Public Property ProductCategoryImage() As String
            Get
                If ViewState(ATTRIBUTE_PRODUCT_CATEGORY_IMAGE_URL) IsNot Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_PRODUCT_CATEGORY_IMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_PRODUCT_CATEGORY_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' If set to False(default), the header is not shown if there are no records in the product listing grid, if set to True, the header is always shown
        ''' </summary>
        Protected m_iShowHeaderIfEmpty As Boolean
        Public Property ShowHeaderIfEmpty() As Boolean
            Get
                Return m_iShowHeaderIfEmpty
            End Get
            Set(ByVal value As Boolean)
                m_iShowHeaderIfEmpty = value
            End Set
        End Property
        ''' <summary>
        ''' Displays text on the top of the control in a header
        ''' </summary>
        Protected m_iHeaderText As String
        Public Property HeaderText() As String
            Get
                Return m_iHeaderText
            End Get
            Set(ByVal value As String)
                m_iHeaderText = value
            End Set
        End Property

        ''' <summary>
        ''' Sets the Parent Product CategoryID to be used for displaying sub-categories, if left blank, top level categories are shown.
        ''' </summary>
        ''' 
        Protected m_iParentID As Long
        Public Property ParentID() As Long
            Get
                If m_iParentID <= 0 Then
                    Return -1
                Else
                    Return m_iParentID
                End If
            End Get
            Set(ByVal Value As Long)
                If Value > 0 Then
                    m_iParentID = Value
                Else
                    m_iParentID = -1
                End If
            End Set
        End Property

        ''' <summary>
        ''' Displays Header on page
        ''' 
        ''' </summary>
        Protected m_iPageHeaderText As String
        Public Property PageHeaderText() As String
            Get
                Return m_iPageHeaderText
            End Get
            Set(ByVal value As String)
                m_iPageHeaderText = value
            End Set
        End Property
        Protected m_iWelcomeText As String
        Public Property WelcomeText() As String
            Get
                Return m_iWelcomeText
            End Get
            Set(ByVal value As String)
                m_iWelcomeText = value
            End Set
        End Property
        Protected m_iProdCatHeader As String
        Public Property ProdCatHeader() As String
            Get
                Return m_iProdCatHeader
            End Get
            Set(ByVal value As String)
                m_iProdCatHeader = value
            End Set
        End Property

        Public Overridable Property ShowSideImage() As String
            Get
                If Not ViewState(ATTRIBUTE_IMAGE_SIDEIMAGE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_IMAGE_SIDEIMAGE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_IMAGE_SIDEIMAGE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
#End Region

        Protected Overrides Sub SetProperties()
            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            Try
                If ShowHeaderIfEmpty = False Then
                    'since value is the 'default' check the XML file for possible custom setting
                    ShowHeaderIfEmpty = CBool(Me.GetPropertyValueFromXML(ATTRIBUTE_SHOW_HEADER_IF_EMPTY))
                End If
            Catch ex As Exception
                If TypeOf ex Is InvalidCastException Then
                    Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(InvalidCastExceptionForBooleanProperties(ATTRIBUTE_SHOW_HEADER_IF_EMPTY, ex.Message))
                Else
                    Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                End If
            End Try

            If ParentID = -1 Then
                'since value is the 'default' check the XML file for possible custom setting
                If Not String.IsNullOrEmpty(Me.GetLinkValueFromXML(ATTRIBUTE_PARENT_ID, False)) Then
                    ParentID = CLng(Me.GetPropertyValueFromXML(ATTRIBUTE_PARENT_ID, False))
                End If
            End If
            If String.IsNullOrEmpty(HeaderText) Then
                'since value is the 'default' check the XML file for possible custom setting
                HeaderText = Me.GetPropertyValueFromXML(ATTRIBUTE_HEADER_TEXT_PAGE)
            End If
            If String.IsNullOrEmpty(ProductCategoryPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                ProductCategoryPage = Me.GetLinkValueFromXML(ATTRIBUTE_PRODUCT_CATEGORY_PAGE)
            End If
            If String.IsNullOrEmpty(ProductCategoryImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                ProductCategoryImage = Me.GetLinkValueFromXML(ATTRIBUTE_PRODUCT_CATEGORY_IMAGE_URL)
            End If
            ''Suvarna IssueID 12430, 12433 & 12434 - New Constants are added to display headers and properties for added constants on Dec 22, 2011
            If String.IsNullOrEmpty(PageHeaderText) Then
                'since value is the 'default' check the XML file for possible custom setting
                PageHeaderText = Me.GetPropertyValueFromXML(ATTRIBUTE_HEADER_TEXT_PAGE_NAME)
            End If
            If String.IsNullOrEmpty(WelcomeText) Then
                'since value is the 'default' check the XML file for possible custom setting
                WelcomeText = Me.GetPropertyValueFromXML(ATTRIBUTE_HEADER_WELCOME_TEXT)
            End If
            If String.IsNullOrEmpty(ProdCatHeader) Then
                'since value is the 'default' check the XML file for possible custom setting
                ProdCatHeader = Me.GetPropertyValueFromXML(ATTRIBUTE_HEADER_PRODCAT_PAGE)
            End If

            If String.IsNullOrEmpty(ShowSideImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                ShowSideImage = Me.GetLinkValueFromXML(ATTRIBUTE_IMAGE_SIDEIMAGE_URL)
            End If
            ''End of addition by Suvarna
        End Sub

        Public Sub LoadCategories()
            Dim sSQL As String, dt As DataTable

            Try
                sSQL = "SELECT ID,WebName,WebDescription FROM " & _
                       AptifyApplication.GetEntityBaseDatabase("Product Categories") & _
                       "..vwProductCategories WHERE " & _
                       "ISNULL(ParentID,-1)=" & ParentID & " AND WebEnabled=1 " & _
                       "ORDER BY WebName"
                dt = DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)
                ''Code Commetented by Suvarna D IssueID-12735 Product Category Mouse hover should display sub categories. on Jan 18, 2012
                'lstCategories.DataSource = dt
                'lstCategories.DataBind()
                ''End of Code Commetented by Suvarna D IssueID-12735 Product Category Mouse hover should display sub categories. on Jan 18, 2012
                lblHeader.InnerText = Me.HeaderText
                ''Suvarna IssueID 12430, 12433 & 12434 - New Constants are added to display headers and also properties on Dec 22, 2011
                lblPageHeaderText.InnerText = Me.PageHeaderText
                lblWelcomeText.InnerText = Me.WelcomeText
                lblProdCatHeader.InnerText = Me.ProdCatHeader
                If Not String.IsNullOrEmpty(ShowSideImage) Then
                    ImgSideImage.ImageUrl = Me.ShowSideImage
                Else
                    Throw (New Exception("ShowSideImage property is not set"))
                End If

                ''End of addition by Suvarna
                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                    Me.lblHeader.Visible = lblHeader.InnerText.Length > 0
                Else
                    If ShowHeaderIfEmpty Then
                        Me.lblHeader.Visible = lblHeader.InnerText.Length > 0
                    Else
                        Me.lblHeader.Visible = False
                        Me.visible = False
                    End If
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
            LoadCategories()
        End Sub

        'Protected Sub lstCategories_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles lstCategories.ItemDataBound
        '    Dim ImagePath As String
        '    If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then

        '        'Suvarna Deshmukh IssueID-12433,12430 and 12434 On Dec 13,2011
        '        'commented and added new code to provide Image to featured product thr' webImage feature
        '        'DirectCast(e.Item.FindControl("imgProdCategory"), HtmlImage).Src = ProductCategoryImage
        '        ImagePath = Server.MapPath(ProductCategoryImage) & "\" & DataBinder.Eval(e.Item.DataItem, "WebName").ToString() & ".png"

        '        If File.Exists(ImagePath) Then
        '            DirectCast(e.Item.FindControl("imgProdCategory"), HtmlImage).Src = ProductCategoryImage & "/" & DataBinder.Eval(e.Item.DataItem, "WebName").ToString() & ".png"
        '        Else
        '            'DirectCast(e.Item.FindControl("imgProdCategory"), HtmlImage).Src = ProductCategoryImage & "/Default.png"
        '        End If
        '        'End of Addition by Suvarna Deshmukh IssueID-12433,12430 and 12434 On Dec 13,2011

        '        With DirectCast(e.Item.FindControl("lnkProdCategory"), HyperLink)
        '            If String.IsNullOrEmpty(ProductCategoryPage) Then
        '                .Enabled = False
        '                .ToolTip = "ProductCategoryPage property not set."
        '                .Font.Underline = True
        '            Else
        '                .NavigateUrl = ProductCategoryPage & "?ID=" & DataBinder.Eval(e.Item.DataItem, "ID").ToString
        '            End If
        '            .Text = DataBinder.Eval(e.Item.DataItem, "WebName").ToString()
        '        End With
        '    End If

        'End Sub

    End Class
End Namespace
