'Aptify e-Business 5.5.1, July 2013
Namespace Aptify.Framework.Web.eBusiness
    Partial Class GoogleMapsDirectionsAPI
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced

#Region "Protected Variables"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "GoogleMapsDirectionsAPI"
        Protected u_MapWidth As Unit
        Protected u_MapHeight As Unit
        Protected s_fromAddress As String = String.Empty
        Protected s_MeetingAddress As String = String.Empty
        Protected s_MapElementId As String = String.Empty
        Protected b_AutoLoad As Boolean = True
        Protected Const GOOGLE_MAPS_APIKEY As String = "GoogleMapsAPIKey"
        Protected Const GOOGLE_MAPS_APIURL As String = "GoogleMapsAPIURL"
#End Region

#Region "Public Propperties"

        Public Overridable Property MapWidth As Unit
            Get
                Return u_MapWidth
            End Get
            Set(ByVal value As Unit)
                u_MapWidth = value
            End Set
        End Property

        Public Overridable Property MapHeight As Unit
            Get
                Return u_MapHeight
            End Get
            Set(ByVal value As Unit)
                u_MapHeight = value
            End Set
        End Property

        Public Overridable Property MapElementId As String
            Get
                Return s_MapElementId
            End Get
            Set(ByVal value As String)
                s_MapElementId = value
            End Set
        End Property

        Public Overridable Property AutoLoad() As Boolean
            Get
                Return b_AutoLoad
            End Get
            Set(ByVal value As Boolean)
                b_AutoLoad = value
            End Set
        End Property

        Public Overridable Property GoogleMapsAPIKey() As String
            Get
                If Not ViewState(GOOGLE_MAPS_APIKEY) Is Nothing Then
                    Return CStr(ViewState(GOOGLE_MAPS_APIKEY))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(GOOGLE_MAPS_APIKEY) = value
            End Set
        End Property

        Public Overridable Property GoogleMapsAPIURL() As String
            Get
                If Not ViewState(GOOGLE_MAPS_APIURL) Is Nothing Then
                    Return CStr(ViewState(GOOGLE_MAPS_APIURL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(GOOGLE_MAPS_APIURL) = value
            End Set
        End Property

        Public Overridable Property FromAddress As String

            Get
                Return s_fromAddress
            End Get
            Set(ByVal value As String)
                s_fromAddress = value
            End Set
        End Property

        Protected Overrides Sub SetProperties()
            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            MyBase.SetProperties()
            If String.IsNullOrEmpty(GoogleMapsAPIKey) Then
                GoogleMapsAPIKey = Me.GetLinkValueFromXML(GOOGLE_MAPS_APIKEY)
            End If

            If String.IsNullOrEmpty(GoogleMapsAPIURL) Then
                GoogleMapsAPIURL = Me.GetLinkValueFromXML(GOOGLE_MAPS_APIURL)
            End If
        End Sub

        Public Overridable Property MeetingAddress As String

            Get
                Return s_MeetingAddress
            End Get
            Set(ByVal value As String)
                s_MeetingAddress = value
            End Set
        End Property
#End Region

#Region "Page Methods"

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                SetProperties()

                Dim s_GoogleMapApiURL As String = String.Empty

                If Request.Url.IsLoopback Then
                    s_GoogleMapApiURL = String.Format(GoogleMapsAPIURL, GoogleMapsAPIKey)
                Else
                    If GoogleMapsAPIKey.Length > 0 Then
                        s_GoogleMapApiURL = String.Format(GoogleMapsAPIURL, GoogleMapsAPIKey)
                    Else
                        GoogleMapDirections.InnerHtml = "Google API key for host: " + Request.Url.Host + " is not registered."
                    End If
                End If
                If s_GoogleMapApiURL IsNot String.Empty Then
                    Page.ClientScript.RegisterClientScriptInclude(Me.GetType(), "API_KEY_REFERENCE", s_GoogleMapApiURL)
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try

        End Sub

#End Region

    End Class
End Namespace
