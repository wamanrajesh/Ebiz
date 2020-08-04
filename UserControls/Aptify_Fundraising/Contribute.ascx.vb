'Aptify e-Business 5.5.1 SR1, June 2014

Namespace Aptify.Framework.Web.eBusiness
    Partial Class Contribute
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "Contribute"
        Protected Const ATTRIBUTE_CONTRIBUTE_IMG_URL As String = "ContributeImage"
        Protected Const ATTRIBUTE_CONTRIBUTE_URL As String = "ContributeUrl"

#Region "Properties"

        Public Overridable Property ContributeImage() As String
            Get
                If Not ViewState(ATTRIBUTE_CONTRIBUTE_IMG_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_CONTRIBUTE_IMG_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_CONTRIBUTE_IMG_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        'Navin Prasad Issue 12542
        Public Overridable Property ContributeUrl() As String
            Get
                If Not ViewState(ATTRIBUTE_CONTRIBUTE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_CONTRIBUTE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_CONTRIBUTE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property


#End Region

#Region "Procedures"

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                SetProperties()
                ''imgContribute.ImageUrl = ContributeImage
                lnkContribure.NavigateUrl = ContributeUrl

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub




        Protected Overrides Sub SetProperties()
            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            MyBase.SetProperties()
            If String.IsNullOrEmpty(ContributeImage) Then
                ContributeImage = Me.GetLinkValueFromXML(ATTRIBUTE_CONTRIBUTE_IMG_URL)
            End If

            If String.IsNullOrEmpty(ContributeUrl) Then
                ContributeUrl = Me.GetLinkValueFromXML(ATTRIBUTE_CONTRIBUTE_URL)
            End If

        End Sub
#End Region
    End Class

End Namespace
