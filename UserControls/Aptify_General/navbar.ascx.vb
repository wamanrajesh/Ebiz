'Aptify e-Business 5.5.1 SR1, June 2014
Imports System.Data
Imports ComponentArt.Web.UI
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.Application
Imports Telerik.Web.UI
Imports Telerik.Web.Device.Detection

Namespace Aptify.Framework.Web.eBusiness

    Partial Class NavBar
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControl
        Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
            'Issue:19327:Added code for to show burger menu only on mobile/iphone size device 
            Dim screenSize As DeviceScreenSize = Detector.GetScreenSize(Request.UserAgent)
            'For Mobile and Iphone size we are setting render mode as mobile
            If screenSize = DeviceScreenSize.Small Then
                RadMenu1.RenderMode = RenderMode.Mobile
                RadMenu1.EnableAutoScroll = True
            End If

        End Sub
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not IsPostBack Then
                DoLoad()
            End If

        End Sub

        

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
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Function
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

        Private Sub LoadMenu(ByRef ParentMenu As RadMenuItem, _
                                   ByVal da As DataAction, _
                                   ByVal app As AptifyApplication, _
                                   ByVal lParentID As Long)
            Dim sSQL As String
            Dim lUserID As Long
            Dim sText As String
            Dim oMenu As RadMenuItem
            Dim sURLType As String
            Dim sBaseURL As String
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

                sSQL = app.GetEntityBaseDatabase("Web Menus") & ".dbo.spGetUserWebMenus @ParentID=" & lParentID & ",@WebUserID=" & lUserID

                Dim dt As DataTable = da.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)

                ' load up the menus
                For Each row As DataRow In dt.Rows
                    sText = row.Item("DisplayName").ToString
                    oMenu = New RadMenuItem
                    mnuItems.Add(oMenu)

                    If lParentID <= 0 Then
                        '' oMenu.LookId = "TopItemLook"
                    End If
                    oMenu.Text = sText

                    oMenu.Value = row.Item("ID").ToString

                    sURLType = row.Item("URLType").ToString.Trim.ToUpper

                    sTargetURL = Replace(row.Item("ImageURL").ToString, _
                                            "%VirtualDir%", sVirtualDir, , , _
                                            CompareMethod.Text)

                    'this is an image so we should give it a look
                    sBaseURL = Replace(row.Item("BaseURL").ToString, _
                                                 "%VirtualDir%", sVirtualDir, , , _
                                                 CompareMethod.Text)
                    ' get the target link URL (from the web menu) and replace any
                    ' %VirtualDir% entries with the appropriate entry
                    sTargetURL = Replace(row.Item("URL").ToString, _
                                                  "%VirtualDir%", sVirtualDir, , , _
                                                  CompareMethod.Text)
                    If sURLType = "ABSOLUTE" Then
                        oMenu.NavigateUrl = sTargetURL
                    Else
                        oMenu.NavigateUrl = sBaseURL & sTargetURL
                    End If

                    'If sURLType = "ABSOLUTE" Then
                    '    oMenu.ImageUrl = sTargetURL
                    'Else
                    '    oMenu.ImageUrl = sBaseURL & sTargetURL
                    'End If

                    If IsDBNull(row.Item("IsSeparator")) OrElse Not CBool(row.Item("IsSeparator")) Then
                    Else
                        oMenu.Value = "MenuBreakLook"
                        oMenu.Text = ""
                        'Sheetal: date:29/08/14:Removed Separator Menu Item as On mobile menu display is not correct subissue of 19327
                        oMenu.NavigateUrl = ""
                        oMenu.Remove()
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
                    'recursive call to children menus
                    ' Suraj Issue 14861 
                    If oMenuItem.Value <> "-1" AndAlso oMenuItem.Value <> "MenuBreakLook" Then
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

        'Private Sub DataBindBreadCrumbSiteMap(ByVal currentItem As RadMenuItem)
        '      Dim breadCrumbPath As New List(Of RadMenuItem)()
        '      While currentItem IsNot Nothing
        '          breadCrumbPath.Insert(0, currentItem)
        '          currentItem = TryCast(currentItem.Owner, RadMenuItem)
        '      End While
        '      BreadCrumbSiteMap.DataSource = breadCrumbPath
        '      BreadCrumbSiteMap.DataBind()
        '  End Sub
    End Class

End Namespace
