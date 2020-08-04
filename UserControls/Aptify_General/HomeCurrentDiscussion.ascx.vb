'Aptify e-Business 5.5.1, July 2013
Imports System.Data
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.Application
Imports Telerik.Sitefinity.Services

Namespace Aptify.Framework.Web.eBusiness.Aptify_General
    Partial Class HomeCurrentDiscussion
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "HomeCurrentDiscussion"
        Protected Const ATTRIBUTE_CONTORL_FORUMPAGE As String = "ForumPage"
        Protected Const ATTRIBUTE_PROFILE_THUMBNAIL_WIDTH As String = "ProfileThumbNailWidth"
        Protected Const ATTRIBUTE_PROFILE_THUMBNAIL_HEIGHT As String = "ProfileThumbNailHeight"
        Protected Const ATTRIBUTE_BLANK_IMG_URL As String = "RadBlankImage"

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'Condition added by Suvarna for IssueId-13107
            'This control will not be available to edit in sitefinity. instead message will be displayed.
            If Not SystemManager.IsDesignMode Then
            SetProperties()
            LoadForumGrid()
            Else
                lblsfMessage.Text = "This control is not configured to run in design time."
            End If

        End Sub

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = "HomeCurrentDiscussion"
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(HomeCurrentDiscussion) Then
                'since value is the 'default' check the XML file for possible custom setting
                HomeCurrentDiscussion = Me.GetLinkValueFromXML(ATTRIBUTE_CONTORL_DEFAULT_NAME)
                If String.IsNullOrEmpty(HomeCurrentDiscussion) Then

                End If


            End If
            If String.IsNullOrEmpty(ForumPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                ForumPage = Me.GetLinkValueFromXML(ATTRIBUTE_CONTORL_FORUMPAGE)
                If String.IsNullOrEmpty(ForumPage) Then

                End If


            End If
            If String.IsNullOrEmpty(RadBlankImage) Then
                RadBlankImage = Me.GetLinkValueFromXML(ATTRIBUTE_BLANK_IMG_URL)
            End If

        End Sub
        Public Overridable Property HomeCurrentDiscussion() As String
            Get
                If Not ViewState(ATTRIBUTE_CONTORL_DEFAULT_NAME) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_CONTORL_DEFAULT_NAME))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_CONTORL_DEFAULT_NAME) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        Public Overridable Property ForumPage() As String
            Get
                If Not ViewState(ATTRIBUTE_CONTORL_FORUMPAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_CONTORL_FORUMPAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_CONTORL_FORUMPAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

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
        Public Overridable Property RadBlankImage() As String
            Get
                If Not ViewState(ATTRIBUTE_BLANK_IMG_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_BLANK_IMG_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_BLANK_IMG_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        Private Sub LoadForumGrid()
            Dim sSQL As String, dt As DataTable
            Try
                'Dilip issue 12717 FOr changing top
                'Nalini Issue 13274 for adding person photo
                sSQL = "SELECT Top 3 *,VP.Photo FROM " & _
                       AptifyApplication.GetEntityBaseDatabase("Discussion Forum Messages") & _
                       ".." & AptifyApplication.GetEntityBaseView("Discussion Forum Messages") & " VM join " & _
                       AptifyApplication.GetEntityBaseDatabase("Web Users") & ".." & AptifyApplication.GetEntityBaseView("Web Users") & " WU on VM.WebUserID=WU.ID join vwPersons VP on WU.LinkID=VP.ID where ParentID is null order by DateEntered desc "
                dt = DataAction.GetDataTable(sSQL)
                lstCurrentDiscussion.DataSource = dt
                lstCurrentDiscussion.DataBind()
           

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try

        End Sub
        'Protected Overrides Sub SetProperties()
        '    If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
        '    'call base method to set parent properties
        '    MyBase.SetProperties()
        'End Sub

        Protected Sub lstCurrentDiscussion_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewCommandEventArgs) Handles lstCurrentDiscussion.ItemCommand
            If e.CommandName = "disscussion" Then
                If User1.UserID > 0 Then
                    Dim sIDLogin As String = System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(e.CommandArgument))
                    Response.Redirect(ForumPage + "?ID=" + sIDLogin)
                Else
                    Dim sID As String = System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(e.CommandArgument))
                    'Suraj Issue 15370,7/31/13 revert back changes application variale to session variable
                    Session("ReturnToPage") = ForumPage + "?ID=" + sID
                    ' Suraj S Issue 15370, 8/1/13 if the user is not login then it will redirect to the loging page and passing the query string   . 
                    Response.Redirect(HomeCurrentDiscussion + "?ID=" + sID + "?ReturnURL=" & System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(HomeCurrentDiscussion)))
                End If

            End If
        End Sub
        ''' <summary>
        ''' Nalini Issue 13274
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        ''' 'neha issue 16001, 05/07/2013, Applied aspect ratio concept for mataining image height and width.
        Protected Sub lstCurrentDiscussion_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles lstCurrentDiscussion.ItemDataBound
            Dim Imagediscussion As Telerik.Web.UI.RadBinaryImage
            Dim Linkbuttonsubject As New LinkButton
            Linkbuttonsubject = CType(e.Item.FindControl("lnkTopic"), LinkButton)
            If (DataBinder.Eval(e.Item.DataItem, "Subject").ToString().Length < 50) Then
                Linkbuttonsubject.Text = DataBinder.Eval(e.Item.DataItem, "Subject")
            Else
                Linkbuttonsubject.Text = DataBinder.Eval(e.Item.DataItem, "Subject").ToString().Substring(0, 50) + ".."
            End If

            If e.Item Is Nothing OrElse e.Item.FindControl("imgProfileRad") Is Nothing Then
                Exit Sub
            End If
            Imagediscussion = CType(e.Item.FindControl("imgProfileRad"), Telerik.Web.UI.RadBinaryImage)
            Imagediscussion.ImageUrl = Me.FixLinkForVirtualPath(RadBlankImage)
            Imagediscussion.DataBind()
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
                    Imagediscussion.DataValue = profileImageByte
                    Imagediscussion.DataBind()
                Else
                    Imagediscussion.ImageUrl = Me.FixLinkForVirtualPath(RadBlankImage)
                    Imagediscussion.DataBind()
                End If
            End If
        End Sub
    End Class

End Namespace
