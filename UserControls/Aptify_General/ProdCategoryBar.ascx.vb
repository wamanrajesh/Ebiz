'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports System.Data
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.Application
Imports Telerik.Web.UI

Namespace Aptify.Framework.Web.eBusiness

    Partial Class ProdCategoryBar
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced

        Protected Const ATTRIBUTE_PRODUCT_CATEGORY_PAGE As String = "ProductCategoryPage"
        Protected Const ATTRIBUTE_IMAGE_NOT_AVAILABLE_URL As String = "DefaultProdCategoryImage"

#Region "ProdCategoryBar Specific Properties"
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

        Public Overridable Property DefaultProdCategoryImage() As String
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
#End Region

        Protected Overrides Sub SetProperties()
            If String.IsNullOrEmpty(ProductCategoryPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                ProductCategoryPage = Me.GetLinkValueFromXML(ATTRIBUTE_PRODUCT_CATEGORY_PAGE)
            End If

            If String.IsNullOrEmpty(DefaultProdCategoryImage) Then
                'since value is the 'default' check the XML file for possible custom setting
                DefaultProdCategoryImage = Me.GetLinkValueFromXML(ATTRIBUTE_IMAGE_NOT_AVAILABLE_URL)
            End If
        End Sub

        'Public Property ExpandDelay() As Integer
        '    Get
        '        Return Me.BaseMenu.ExpandDelay
        '    End Get
        '    Set(ByVal Value As Integer)
        '        Me.BaseMenu.ExpandDelay = Value
        '    End Set
        'End Property
        'Public Property ExpandDuration() As Integer
        '    Get
        '        Return Me.BaseMenu.ExpandDuration
        '    End Get
        '    Set(ByVal Value As Integer)
        '        Me.BaseMenu.ExpandDuration = Value
        '    End Set
        'End Property
        'Public Property ExpandSlide() As SlideType
        '    Get
        '        Return Me.BaseMenu.ExpandSlide
        '    End Get
        '    Set(ByVal Value As SlideType)
        '        Me.BaseMenu.ExpandSlide = Value
        '    End Set
        'End Property
        'Public Property ExpandTransition() As TransitionType
        '    Get
        '        Return Me.BaseMenu.ExpandTransition
        '    End Get
        '    Set(ByVal Value As TransitionType)
        '        Me.BaseMenu.ExpandTransition = Value
        '    End Set
        'End Property
        'Public Property ExpandTransitionCustomFilter() As String
        '    Get
        '        Return Me.BaseMenu.ExpandTransitionCustomFilter
        '    End Get
        '    Set(ByVal Value As String)
        '        Me.BaseMenu.ExpandTransitionCustomFilter = Value
        '    End Set
        'End Property


        'Public Property CollapseDelay() As Integer
        '    Get
        '        Return Me.BaseMenu.CollapseDelay
        '    End Get
        '    Set(ByVal Value As Integer)
        '        Me.BaseMenu.CollapseDelay = Value
        '    End Set
        'End Property
        'Public Property CollapseDuration() As Integer
        '    Get
        '        Return Me.BaseMenu.CollapseDuration
        '    End Get
        '    Set(ByVal Value As Integer)
        '        Me.BaseMenu.CollapseDuration = Value
        '    End Set
        'End Property
        'Public Property CollapseSlide() As SlideType
        '    Get
        '        Return Me.BaseMenu.CollapseSlide
        '    End Get
        '    Set(ByVal Value As SlideType)
        '        Me.BaseMenu.CollapseSlide = Value
        '    End Set
        'End Property
        'Public Property CollapseTransition() As TransitionType
        '    Get
        '        Return Me.BaseMenu.CollapseTransition
        '    End Get
        '    Set(ByVal Value As TransitionType)
        '        Me.BaseMenu.CollapseTransition = Value
        '    End Set
        'End Property
        'Public Property CollapseTransitionCustomFilter() As String
        '    Get
        '        Return Me.BaseMenu.CollapseTransitionCustomFilter
        '    End Get
        '    Set(ByVal Value As String)
        '        Me.BaseMenu.CollapseTransitionCustomFilter = Value
        '    End Set
        'End Property

        Dim m_oUser As New User
        ''' <summary>
        ''' This method returns an instance of the Aptify e-Business User Object
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Overridable Function GetUserObject() As User

            Try
                m_oUser.LoadValuesFromSessionObject(Page.Session)
                Return m_oUser
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return Nothing
            End Try
        End Function

        Public Function DoLoad() As Boolean
            ' Load up the menu from the database by
            Dim da As DataAction
            Dim oApp As AptifyApplication
            Dim g As EBusinessGlobal
            Try
                g = New EBusinessGlobal
                da = New DataAction(g.GetLoginCredentials(Page.Application, Page.User))
                'HP Issue#7812: replace all AptifyApplication references to that provided by EBusinessGlobal
                'oApp = New AptifyApplication(da.UserCredentials)
                oApp = Me.AptifyApplication

                'Me.BaseMenu.Items.Clear()

                Me.LoadMenu(Nothing, da, oApp, Me.MenuID)
                'suvarna Code to display the categories product wise.
                'If Request.QueryString("ID") IsNot Nothing Then
                '    Me.LoadMenu(Nothing, da, oApp, CType(Request.QueryString("ID").ToString, Long))
                'Else
                '    Me.LoadMenu(Nothing, da, oApp, Me.MenuID)
                'End If
                Return True
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return False
            End Try
        End Function

        'Private Sub LoadMenu(ByRef ParentMenu As MenuItem, _
        '                     ByVal da As DataAction, _
        '                     ByVal app As AptifyApplication, _
        '                     ByVal lParentID As Long)

        '    Dim sSQL As String
        '    Dim lUserID As Long
        '    Dim sText As String
        '    Dim oMenu As MenuItem
        '    Dim sVirtualDir As String
        '    Dim sTargetURL As String

        '    Try
        '        Dim mnuItems As MenuItemCollection
        '        If ParentMenu Is Nothing Then
        '            mnuItems = Me.BaseMenu.Items
        '        Else
        '            mnuItems = ParentMenu.Items
        '        End If

        '        ' Sapna DJ Issue #12545 - Investigate Ways to Reduce Size of Session Objects - Code clean up (reduced number of load session object call by adding the if condition)
        '        If Not Session("UserLoggedIn") Is Nothing AndAlso CBool(Session("UserLoggedIn")) = True Then
        '            If (m_oUser.UserID = -1) Then
        '                lUserID = GetUserObject.UserID
        '            Else
        '                lUserID = m_oUser.UserID
        '            End If
        '        End If
        '        ' ADDED RELATIVE VIRTUAL DIRECTORY SUPPORT          RFB 7/25/2003
        '        '
        '        ' In the case that we used URLs that rely on needing to know
        '        ' the base virtual directory, we need to support the fact that
        '        ' the virtual directory may change. The configuration file
        '        ' 'web.config' now stores a setting containing the correct
        '        ' virtual directory, but web modules base URL's and web menu
        '        ' image URL's were hardcoded. Web modules now support the use
        '        ' of the tag string %VirtualDir%, which we replace 
        '        ' in this function with the correct string.
        '        '
        '        ' %VirtualDir% is expected to not have a slash on its end for
        '        ' better readability to the end user, but the configuration definition
        '        ' is expected to always have this slash, so we lop it off
        '        ' below before replacing the key.
        '        '   sVirtualDir = System.Configuration.ConfigurationManager.AppSettings("virtualdir").TrimEnd("/"c)
        '        sVirtualDir = Request.ApplicationPath.TrimEnd("/"c)

        '        sSQL = "SELECT ID,WebName,WebDescription, webImage as ImageURL FROM " & _
        '               AptifyApplication.GetEntityBaseDatabase("Product Categories") & _
        '               "..vwProductCategories WHERE " & _
        '               "ISNULL(ParentID,-1)=" & lParentID & " AND WebEnabled=1 " & _
        '               "ORDER BY WebName"

        '        Dim dt As DataTable = da.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)

        '        ' load up the menus
        '        For Each row As DataRow In dt.Rows
        '            sText = row.Item("WebName").ToString
        '            oMenu = New MenuItem
        '            mnuItems.Add(oMenu)

        '            If lParentID <= 0 Then
        '                oMenu.LookId = "TopItemLook"
        '            End If
        '            oMenu.Text = sText

        '            oMenu.Value = row.Item("ID").ToString

        '            If Not String.IsNullOrEmpty(ProductCategoryPage) Then
        '                oMenu.NavigateUrl = String.Format(ProductCategoryPage & "?ID={0}", row.Item("ID").ToString)
        '            Else
        '                Throw (New Exception("ProductCategoryPage property is not set"))
        '            End If

        '            If Not (IsDBNull(row.Item("ImageURL"))) AndAlso CStr((row.Item("ImageURL"))) <> "" Then
        '                ' load up the ImageURL data and replace any %VirtualDir% usage
        '                sTargetURL = Replace(row.Item("ImageURL").ToString, _
        '                                     "%VirtualDir%", sVirtualDir, , , _
        '                                     CompareMethod.Text)

        '                'this is an image so we should give it a look
        '                oMenu.Look.LeftIconUrl = sTargetURL
        '            ElseIf Not String.IsNullOrEmpty(DefaultProdCategoryImage) Then
        '                oMenu.Look.LeftIconUrl = DefaultProdCategoryImage
        '            Else
        '                Throw (New Exception("DefaultProdCategoryImage property is not set"))
        '            End If
        '        Next

        '        If dt IsNot Nothing Then
        '            dt.Dispose()
        '        End If

        '        If lParentID <= 0 Then
        '            '8/27/06 RJK - Added a placeholder menu item at the end of the list to 
        '            'stop the proportional resizing of the menu items.
        '            'Only for the top-level Menu 
        '            'oMenu = New MenuItem
        '            'mnuItems.Add(oMenu)

        '            'oMenu.LookId = "SpacerItemLook"
        '            'oMenu.Text = " "
        '            'oMenu.Width = System.Web.UI.WebControls.Unit.Percentage(100)
        '            'oMenu.Value = "-1"
        '        End If

        '        For Each oMenuItem As MenuItem In mnuItems
        '            ' recursive call to children menus
        '            If oMenuItem.Value <> "-1" AndAlso oMenuItem.LookId <> "MenuBreak" Then
        '                LoadMenu(oMenuItem, _
        '                         da, _
        '                        app, _
        '                         CLng(oMenuItem.Value))
        '            End If
        '        Next

        '    Catch ex As Exception
        '        Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        '    End Try
        'End Sub

        Private Sub LoadMenu(ByRef ParentMenu As RadMenuItem, _
                             ByVal da As DataAction, _
                             ByVal app As AptifyApplication, _
                             ByVal lParentID As Long)
            Dim sSQL As String
            Dim lUserID As Long
            Dim sText As String
            Dim oMenu As RadMenuItem
            Dim sVirtualDir As String
            Dim sTargetURL As String

            Try
                Dim mnuItems As RadMenuItemCollection
                If ParentMenu Is Nothing Then
                    mnuItems = Me.RadMenu1.Items
                Else
                    mnuItems = ParentMenu.Items
                End If

                ' Sapna DJ Issue #12545 - Investigate Ways to Reduce Size of Session Objects - Code clean up (reduced number of load session object call by adding the if condition)
                If Not Session("UserLoggedIn") Is Nothing AndAlso CBool(Session("UserLoggedIn")) = True Then
                    If (m_oUser.UserID = -1) Then
                        lUserID = GetUserObject.UserID
                    Else
                        lUserID = m_oUser.UserID
                    End If
                End If
                ' ADDED RELATIVE VIRTUAL DIRECTORY SUPPORT          RFB 7/25/2003
                '
                ' In the case that we used URLs that rely on needing to know
                ' the base virtual directory, we need to support the fact that
                ' the virtual directory may change. The configuration file
                ' 'web.config' now stores a setting containing the correct
                ' virtual directory, but web modules base URL's and web menu
                ' image URL's were hardcoded. Web modules now support the use
                ' of the tag string %VirtualDir%, which we replace 
                ' in this function with the correct string.
                '
                ' %VirtualDir% is expected to not have a slash on its end for
                ' better readability to the end user, but the configuration definition
                ' is expected to always have this slash, so we lop it off
                ' below before replacing the key.
                '   sVirtualDir = System.Configuration.ConfigurationManager.AppSettings("virtualdir").TrimEnd("/"c)
                sVirtualDir = Request.ApplicationPath.TrimEnd("/"c)

                sSQL = "SELECT ID,WebName,WebDescription, webImage as ImageURL FROM " & _
                       AptifyApplication.GetEntityBaseDatabase("Product Categories") & _
                       "..vwProductCategories WHERE " & _
                       "ISNULL(ParentID,-1)=" & lParentID & " AND WebEnabled=1 " & _
                       "ORDER BY WebName"

                Dim dt As DataTable = da.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)

                ' load up the menus
                For Each row As DataRow In dt.Rows
                    sText = row.Item("WebName").ToString
                    oMenu = New RadMenuItem
                    mnuItems.Add(oMenu)

                    If lParentID <= 0 Then
                        '' oMenu.LookId = "TopItemLook"
                    End If
                    oMenu.Text = sText

                    oMenu.Value = row.Item("ID").ToString

                    If Not String.IsNullOrEmpty(ProductCategoryPage) Then
                        oMenu.NavigateUrl = String.Format(ProductCategoryPage & "?ID={0}", row.Item("ID").ToString)
                    Else
                        Throw (New Exception("ProductCategoryPage property is not set"))
                    End If

                    If Not (IsDBNull(row.Item("ImageURL"))) AndAlso CStr((row.Item("ImageURL"))) <> "" Then
                        ' load up the ImageURL data and replace any %VirtualDir% usage
                        sTargetURL = Replace(row.Item("ImageURL").ToString, _
                                             "%VirtualDir%", sVirtualDir, , , _
                                             CompareMethod.Text)

                        'this is an image so we should give it a look
                        oMenu.ImageUrl = sTargetURL
                    ElseIf Not String.IsNullOrEmpty(DefaultProdCategoryImage) Then
                        oMenu.ImageUrl = DefaultProdCategoryImage
                    Else
                        Throw (New Exception("DefaultProdCategoryImage property is not set"))
                    End If
                Next

                If dt IsNot Nothing Then
                    dt.Dispose()
                End If

                If lParentID <= 0 Then
                    '8/27/06 RJK - Added a placeholder menu item at the end of the list to 
                    'stop the proportional resizing of the menu items.
                    'Only for the top-level Menu 
                    oMenu = New RadMenuItem
                    mnuItems.Add(oMenu)

                    ''oMenu.LookId = "SpacerItemLook"
                    oMenu.Text = " "
                    'oMenu.Width = System.Web.UI.WebControls.Unit.Percentage(100)
                    oMenu.Value = "-1"
                End If

                For Each oMenuItem As RadMenuItem In mnuItems
                    ' recursive call to children menus
                    If oMenuItem.Value <> "-1" Then
                        LoadMenu(oMenuItem, _
                                 da, _
                                app, _
                                 CLng(oMenuItem.Value))
                    End If
                Next

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Public Property MenuID() As Long
            Get
                Dim o As Object
                o = ViewState.Item("MenuID")
                If Not IsNumeric(o) Then
                    Return -1
                Else
                    If CLng(o) <= 0 Then
                        Return -1
                    Else
                        Return CLng(o)
                    End If
                End If
            End Get
            Set(ByVal Value As Long)
                ViewState.Add("MenuID", Value)
            End Set
        End Property

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            'Anil B for issue 15302 on 04-04-2013
            'If page is postback then dont allow to call DoLoad function
            If Not IsPostBack Then
                MyBase.OnLoad(e)
                DoLoad()
            End If

        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                SetProperties()
                If Page.Request.RawUrl.IndexOf("cmspagemode=preview") > 0 _
                OrElse Page.Request.RawUrl.IndexOf("cmspagemode=edit") > 0 Then
                    Me.Visible = False
                Else
                    Me.Visible = True
                End If

            Catch ex As Exception

            End Try
        End Sub

        'Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        '    Me.OnLoad(e)
        'End Sub
    End Class
End Namespace