'Aptify e-Business 5.5.1, July 2013
Option Explicit On

Namespace Aptify.Framework.Web.eBusiness.AbstractManagement
    Partial Class AbstractsCenter
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced

        Protected Const ATTRIBUTE_SUBMIT_ABSTRACT_PAGE As String = "SubmitAbstractPage"
        Protected Const ATTRIBUTE_VIEW_ABSTRACT_PAGE As String = "ViewAbstractPage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "AbstractsCenter"

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
        End Sub


        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(SubmitAbstractPage) Then
                SubmitAbstractPage = Me.GetLinkValueFromXML(ATTRIBUTE_SUBMIT_ABSTRACT_PAGE)
                If String.IsNullOrEmpty(SubmitAbstractPage) Then
                    Me.lnkAbstracts.Enabled = False
                    Me.lnkAbstracts.ToolTip = "SubmitAbstractPage property has not been set."
                Else
                    Me.lnkAbstracts.NavigateUrl = SubmitAbstractPage
                End If
            Else
                Me.lnkAbstracts.NavigateUrl = SubmitAbstractPage
            End If

            If String.IsNullOrEmpty(ViewAbstractPage) Then
                ViewAbstractPage = Me.GetLinkValueFromXML(ATTRIBUTE_VIEW_ABSTRACT_PAGE)
                If String.IsNullOrEmpty(ViewAbstractPage) Then
                    Me.lnkReview.Enabled = False
                    Me.lnkReview.ToolTip = "ViewAbstractPage property has not been set."
                Else
                    Me.lnkReview.NavigateUrl = ViewAbstractPage
                End If
            Else
                Me.lnkReview.NavigateUrl = ViewAbstractPage
            End If

        End Sub

#Region "AbstractsCenter Specific Properties"
        ''' <summary>
        ''' SubmitAbstract page url
        ''' </summary>
        Public Overridable Property SubmitAbstractPage() As String
            Get
                If Not ViewState(ATTRIBUTE_SUBMIT_ABSTRACT_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_SUBMIT_ABSTRACT_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_SUBMIT_ABSTRACT_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' ViewAbstract page url
        ''' </summary>
        Public Overridable Property ViewAbstractPage() As String
            Get
                If Not ViewState(ATTRIBUTE_VIEW_ABSTRACT_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_VIEW_ABSTRACT_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_VIEW_ABSTRACT_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
#End Region

    End Class
End Namespace
