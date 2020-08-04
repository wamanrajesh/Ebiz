'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.Application
Imports System.Data


Namespace Aptify.Framework.Web.eBusiness.ProductCatalog
    Partial Class ProdListingGrid
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced

        Protected Const ATTRIBUTE_SHOW_HEADER_IF_EMPTY As String = "ShowHeaderIfEmpty"
        Protected Const ATTRIBUTE_HEADER_TEXT_PAGE As String = "HeaderText"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "ProdListingGrid"
        'Suvarna D IssueID-12745 to implement webImage feature to display images, IssueId 12735- to add header for product category navbar on Jan 19, 2012     
        Protected Const ATTRIBUTE_HEADER_PRODCAT_PAGE As String = "ProdCatHeader"
        Protected Const ATTRIBUTE_IMAGE_NOT_AVAILABLE_URL As String = "ImageNotAvailable"
        'End of addition by Suvarna D IssueID-12745 to implement webImage feature to display images on Jan 19, 2012
        Protected Const ATTRIBUTE_SHOWMEETINGSLINKTOCLASS_DEFAULT_NAME As String = "ShowMeetingsLinkToClass"


#Region "ProdListingGrid Specific Properties"
        ''' <summary>
        ''' If set to False(default), the header is not shown if there are no records in the product listing grid, if set to True, the header is always shown
        ''' </summary>
        Public Property ShowHeaderIfEmpty() As Boolean
            Get
                If ViewState(ATTRIBUTE_SHOW_HEADER_IF_EMPTY) IsNot Nothing Then
                    Return CBool(ViewState(ATTRIBUTE_SHOW_HEADER_IF_EMPTY))
                Else
                    Return False
                End If
            End Get
            Set(ByVal value As Boolean)
                ViewState(ATTRIBUTE_SHOW_HEADER_IF_EMPTY) = value
            End Set
        End Property
        ''' <summary>
        ''' Displays text on the top of the control in a header
        ''' </summary>
        Public Property HeaderText() As String
            Get
                If ViewState(ATTRIBUTE_HEADER_TEXT_PAGE) IsNot Nothing Then
                    Return ViewState(ATTRIBUTE_HEADER_TEXT_PAGE).ToString
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_HEADER_TEXT_PAGE) = value
            End Set
        End Property

        Public Property NavigateURLFormatField() As String
            Get
                Dim o As Object
                o = ViewState.Item("NavigateURLFormatField")
                If o Is Nothing Then
                    Return ""
                Else
                    Return CStr(o)
                End If
            End Get
            Set(ByVal Value As String)
                ViewState.Add("NavigateURLFormatField", Value)
            End Set
        End Property

        Public Property ProdCatHeader() As String
            Get
                If ViewState(ATTRIBUTE_HEADER_PRODCAT_PAGE) IsNot Nothing Then
                    Return ViewState(ATTRIBUTE_HEADER_PRODCAT_PAGE).ToString
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_HEADER_PRODCAT_PAGE) = value
            End Set
        End Property

        ''' <summary>
        ''' 'Suvarna D IssueID-12745 to implement webImage feature to display images on Jan 19, 2012
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
        'Nalini issue 11290
        Protected Overridable ReadOnly Property ShowMeetingsLinkToClass() As Boolean
            Get
                If Not ViewState.Item(ATTRIBUTE_SHOWMEETINGSLINKTOCLASS_DEFAULT_NAME) Is Nothing Then
                    Return CBool(ViewState.Item(ATTRIBUTE_SHOWMEETINGSLINKTOCLASS_DEFAULT_NAME))
                Else
                    Dim value As String = Me.GetGlobalAttributeValue(ATTRIBUTE_SHOWMEETINGSLINKTOCLASS_DEFAULT_NAME)
                    If Not String.IsNullOrEmpty(value) Then
                        Select Case Ucase(value)
                            Case "TRUE", "FALSE", "0", "1"
                                ViewState.Item(ATTRIBUTE_SHOWMEETINGSLINKTOCLASS_DEFAULT_NAME) = CBool(value)
                            Case Else
                                ViewState.Item(ATTRIBUTE_SHOWMEETINGSLINKTOCLASS_DEFAULT_NAME) = False
                        End Select
                    Else
                        ViewState.Item(ATTRIBUTE_SHOWMEETINGSLINKTOCLASS_DEFAULT_NAME) = False
                    End If
                End If
            End Get
        End Property
        'End of addition by Suvarna D IssueID-12745 to implement webImage feature to display images on Jan 19, 2012

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
            If String.IsNullOrEmpty(HeaderText) Then
                'since value is the 'default' check the XML file for possible custom setting
                HeaderText = Me.GetLinkValueFromXML(ATTRIBUTE_HEADER_TEXT_PAGE)
            End If

            'Suvarna D IssueID-12745 to implement webImage feature to display images, IssueId 12735- to add header for product category navbaron Jan 19, 2012
            If String.IsNullOrEmpty(ProdCatHeader) Then
                'since value is the 'default' check the XML file for possible custom setting
                ProdCatHeader = Me.GetPropertyValueFromXML(ATTRIBUTE_HEADER_PRODCAT_PAGE)
            End If
            If String.IsNullOrEmpty(ImageNotAvailable) Then
                'since value is the 'default' check the XML file for possible custom setting
                ImageNotAvailable = Me.GetLinkValueFromXML(ATTRIBUTE_IMAGE_NOT_AVAILABLE_URL)
            End If
            'End of addition by Suvarna D IssueID-12745 to implement webImage feature to display images on Jan 19, 2012
        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
            'set control properties from XML file if needed
            SetProperties()

            Me.SetControlRecordIDFromParam()
            If Me.ControlRecordID > 0 Then
                LoadGrid(ControlRecordID)
            End If
            lblProdCatHeader.InnerText = Me.ProdCatHeader

        End Sub

        Public Function GetGridRowCount() As Long
            'Navin Prasad Issue 11032
            GetGridRowCount = grdMain.Items.Count
        End Function
        Public Function GetProductPrice(ByVal lProductID As Long) As Aptify.Applications.OrderEntry.IProductPrice.PriceInfo
            ' Implement This function
            Return ShoppingCart1.GetUserProductPrice(lProductID, 1)
        End Function

        Public Sub LoadGrid(ByVal CategoryID As Long, _
                            Optional ByVal ExcludeProductID As Long = -1)
            Dim sSQL As String
            Dim dt As System.Data.DataTable
            Try
                'Suvarna Deshmukh IssueID-12433,12430 and 12434 On Dec 19,2011
                'commented and added new code to get a product code from SQL
                ''sSQL = "SELECT ID,WebName,WebDescription, ProductCategory" & _
                sSQL = "SELECT ID,WebName,WebDescription, ProductCategory, code, WebImage " & _
                       "FROM " & AptifyApplication.GetEntityBaseDatabase("Products") & _
                       "..vwProducts " & _
                       "WHERE ID<>" & ExcludeProductID & " AND " & _
                       "IsSold=1 AND WebEnabled=1 AND TopLevelItem=1 AND " & _
                       "ISNULL(ParentID,-1)=-1 AND CategoryID=" & CategoryID

                If Not ShowMeetingsLinkToClass Then
                    sSQL &= "  AND  ISNULL(ClassID ,-1) <=0 "
                End If

                sSQL &= " ORDER BY Name ASC"
                dt = DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)

                If dt IsNot Nothing Then
                    If dt.Rows.Count > 0 Then
                        'Navin Prasad Issue 11032
                        Dim arr() As String
                        arr = New String() {"ID"}


                        ''grdMain.DataKeyNames = arr
                        grdMain.DataSource = dt


                        'Suvarna Deshmukh IssueID-12433,12430 and 12434 On Dec 16,2011
                        'commented and added a Row_DataBound event to add navigation url to each link
                        'With DirectCast(grdMain.Columns(0), HyperLinkField)
                        '    If Not String.IsNullOrEmpty(NavigateURLFormatField) Then
                        '        .DataNavigateUrlFormatString = NavigateURLFormatField
                        '    Else
                        '        grdMain.Enabled = False
                        '        grdMain.ToolTip = "NavigateURLFormatField property not set via container control."
                        '    End If
                        'End With
                        'End of Addition by Suvarna Deshmukh IssueID-12433,12430 and 12434 On Dec 13,2011                         


                        grdMain.Visible = True
                        If Len(Me.HeaderText) > 0 Then
                            lblHeader.Visible = True
                        Else
                            lblHeader.Visible = False
                        End If
                    Else
                        grdMain.Visible = False
                        ''Code Commetented by Suvarna D IssueID-12735 Product Category Mouse hover should display sub categories. on Jan 18, 2012
                        'Product list grid and feature proudct has been added to same dive hence code commented
                        'divMain.Visible = False
                        ''End of Code Commetented by Suvarna D IssueID-12735 Product Category Mouse hover should display sub categories. on Jan 18, 2012
                        Me.lblHeader.Visible = Me.ShowHeaderIfEmpty
                    End If
                Else
                    grdMain.Visible = False
                    ''Code Commetented by Suvarna D IssueID-12735 Product Category Mouse hover should display sub categories. on Jan 18, 2012
                    'Product list grid and feature proudct has been added to same dive hence code commented
                    'divMain.Visible = False
                    ''End of Code Commetented by Suvarna D IssueID-12735 Product Category Mouse hover should display sub categories. on Jan 18, 2012
                    Me.lblHeader.Visible = Me.ShowHeaderIfEmpty
                End If
                lblHeader.InnerText = GenerateHeaderText(dt, Me.HeaderText)

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Overridable Function GenerateHeaderText(ByVal dt As DataTable, ByVal HeaderTextFormat As String) As String
            Try
                Dim bDone As Boolean = False, sTemp As String = HeaderTextFormat
                Dim iStart As Integer, iEnd As Integer, sField As String, sVal As String

                While Not bDone
                    If sTemp.Contains("{") Then
                        iStart = sTemp.IndexOf("{")
                        iEnd = sTemp.IndexOf("}", iStart + 1)
                        sField = sTemp.Substring(iStart + 1, iEnd - iStart - 1)
                        If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                            Try
                                sVal = CStr(dt.Rows(0).Item(sField))
                            Catch ex As Exception
                                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                                sVal = ex.Message
                            End Try
                        Else
                            sVal = ""
                        End If
                        If iStart > 1 Then
                            sTemp = sTemp.Substring(0, iStart - 1) & sVal & sTemp.Substring(iEnd + 1)
                        Else
                            sTemp = sVal & sTemp.Substring(iEnd + 1)
                        End If
                    Else
                        bDone = True
                    End If
                End While
                Return sTemp
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return ex.Message
            End Try
        End Function

        Protected Sub grdMain_RowDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdMain.ItemDataBound

            Try
                'Dim type As ListItemType = CType(e.Item.ItemType, ListItemType)
                'If CType(e.Item.ItemType, ListItemType) = ListItemType.Item Or _
                '        CType(e.Item.ItemType, ListItemType) = ListItemType.AlternatingItem Then
                If (TypeOf e.Item Is Telerik.Web.UI.GridDataItem) Then
                    Dim dataItem As Telerik.Web.UI.GridDataItem = CType(e.Item, Telerik.Web.UI.GridDataItem)
                    'Dilip issue 12719 19/1/2012 declare lblItemCode 
                    Dim lblItemCode As Label = CType(e.Item.FindControl("lblItemCode"), Label)

                    With DirectCast(e.Item.FindControl("lnkProduct"), Hyperlink)
                        If Not String.IsNullOrEmpty(NavigateURLFormatField) Then
                            .NavigateUrl = String.Format(NavigateURLFormatField, DataBinder.Eval(e.Item.DataItem, "ID").ToString)

                        Else
                            grdMain.Enabled = False
                            grdMain.ToolTip = "NavigateURLFormatField property not set."
                        End If
                    End With

                    With DirectCast(e.Item.FindControl("lblItemCodeVal"), Label)
                        If Not String.IsNullOrEmpty(DataBinder.Eval(e.Item.DataItem, "Code").ToString()) Then
                            .Text = DataBinder.Eval(e.Item.DataItem, "Code").ToString()
                            'Dilip issue 12719 19/1/2012 to visiblity code on the availability of Item code
                            .Visible = True
                            If lblItemCode IsNot Nothing Then
                                lblItemCode.Visible = True
                            End If
                        Else
                            .Visible = False
                            If lblItemCode IsNot Nothing Then
                                lblItemCode.Visible = False

                            End If
                            'Dilip issue 12719 write the visibility code here ..
                        End If
                    End With

                    With DirectCast(e.Item.FindControl("lblPriceForYouVal"), Label)
                        If Not String.IsNullOrEmpty(DataBinder.Eval(e.Item.DataItem, "ID").ToString()) Then
                            Dim oPrice As Aptify.Applications.OrderEntry.IProductPrice.PriceInfo = Me.GetProductPrice(CLng(DataBinder.Eval(e.Item.DataItem, "ID").ToString()))
                            .Text = Format(oPrice.Price, ShoppingCart1.GetCurrencyFormat(oPrice.CurrencyTypeID))
                        End If
                    End With

                    'Suvarna D IssueID-12745 to implement webImage feature to display images on Jan 19, 2012
                    With DirectCast(e.Item.FindControl("ImgProd"), Image)
                        If Not IsDBNull(DataBinder.Eval(e.Item.DataItem, "WebImage")) AndAlso _
                   Len(DataBinder.Eval(e.Item.DataItem, "WebImage")) > 0 Then
                            .ImageUrl = DataBinder.Eval(e.Item.DataItem, "WebImage").ToString
                        Else
                            .ImageUrl = ImageNotAvailable
                        End If
                    End With
                    'End of addition by Suvarna D IssueID-12745 to implement webImage feature to display images on Jan 19, 2012
                End If
            Catch ex As Exception

            End Try

        End Sub

        Protected Sub grdMain_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdMain.NeedDataSource
            LoadGrid(CLng(Request.QueryString("ID")))
        End Sub


        ''' <summary>
        ''' Nalini issue 12436 date:1/12/2011
        ''' </summary>Here is the problem for paging because the control record id is comes from product category page and on paging event the control record is not found.
        ''' <remarks></remarks>
        Protected Sub grdMain_PageIndexChanging(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageChangedEventArgs) Handles grdMain.PageIndexChanged
            ''grdMain.PageIndex = e.NewPageIndex
            LoadGrid(CLng(Request.QueryString("ID")))
        End Sub
    End Class
End Namespace
