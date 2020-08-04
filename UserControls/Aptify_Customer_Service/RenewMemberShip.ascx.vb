'Aptify e-Business 5.5.1, July 2013


Namespace Aptify.Framework.Web.eBusiness
    Partial Class RenewMemberShip
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "RenewMemberShip"
        Protected Const ATTRIBUTE_MEMBERSHIP_IMG_URL As String = "MemberShipImage"
        Protected Const ATTRIBUTE_MEMBERSHIP_URL As String = "MemberShipUrl"

#Region "Properties"
        Public Overridable Property MemberShipImage() As String
            Get
                If Not ViewState(ATTRIBUTE_MEMBERSHIP_IMG_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_MEMBERSHIP_IMG_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_MEMBERSHIP_IMG_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        Public Overridable Property MemberShipUrl() As String
            Get
                If Not ViewState(ATTRIBUTE_MEMBERSHIP_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_MEMBERSHIP_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_MEMBERSHIP_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property


#End Region
#Region "Procedures"
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                SetProperties()
                imgRenewMemberShip.ImageUrl = MemberShipImage

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Overrides Sub SetProperties()
            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            MyBase.SetProperties()
            If String.IsNullOrEmpty(MemberShipImage) Then
                MemberShipImage = Me.GetLinkValueFromXML(ATTRIBUTE_MEMBERSHIP_IMG_URL)
            End If

            If String.IsNullOrEmpty(MemberShipUrl) Then
                MemberShipUrl = Me.GetLinkValueFromXML(ATTRIBUTE_MEMBERSHIP_URL)
            End If

        End Sub

        Protected Sub imgRenewMemberShip_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgRenewMemberShip.Click
            Response.Redirect(MemberShipUrl)
        End Sub
#End Region


    End Class
End Namespace


