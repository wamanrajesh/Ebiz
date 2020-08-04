'Aptify e-Business 5.5.1, July 2013

Namespace Aptify.Framework.Web.eBusiness
    Partial Class BecomeMemberControl
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "BecomeMemberControl"
        Protected Const ATTRIBUTE_BECOMEMEMBER_URL As String = "BecomeMemberUrl"
        Protected Const ATTRIBUTE_PARAMETERNAME As String = "ParameterName"
        Protected Const ATTRIBUTE_PARAMETERVALUE As String = "ParameterValue"


        Public Overridable Property BecomeMemberUrl() As String
            Get
                If Not ViewState(ATTRIBUTE_BECOMEMEMBER_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_BECOMEMEMBER_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_BECOMEMEMBER_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        Public Overridable Property ParameterName() As String
            Get
                If Not ViewState(ATTRIBUTE_PARAMETERNAME) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_PARAMETERNAME))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_PARAMETERNAME) = value
            End Set
        End Property
        Public Overridable Property ParameterValue() As String
            Get
                If Not ViewState(ATTRIBUTE_PARAMETERVALUE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_PARAMETERVALUE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_PARAMETERVALUE) = value
            End Set
        End Property

        Protected Overrides Sub SetProperties()
            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            MyBase.SetProperties()

            If String.IsNullOrEmpty(BecomeMemberUrl) Then
                BecomeMemberUrl = Me.GetLinkValueFromXML(ATTRIBUTE_BECOMEMEMBER_URL)
            End If
            If String.IsNullOrEmpty(ParameterName) Then
                ParameterName = Me.GetPropertyValueFromXML(ATTRIBUTE_PARAMETERNAME)
            End If
            If String.IsNullOrEmpty(ParameterValue) Then
                ParameterValue = Me.GetPropertyValueFromXML(ATTRIBUTE_PARAMETERVALUE)
            End If
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            SetProperties()
           
        End Sub

        Protected Sub btnJoin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnJoin.Click
            Response.Redirect(BecomeMemberUrl + "?" + ParameterName + "=" + ParameterValue)
        End Sub
    End Class
End Namespace

