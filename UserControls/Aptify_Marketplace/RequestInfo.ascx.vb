'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports System.Data
Imports Aptify.Framework.BusinessLogic.GenericEntity

Namespace Aptify.Framework.Web.eBusiness.MarketPlace
    Partial Class RequestInfo
        Inherits BaseUserControlAdvanced

        Private m_lListingID As Long

        Protected Const ATTRIBUTE_REQUEST_COMPLETE_PAGE As String = "RequestCompletePage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "RequestInfo"

#Region "RequestInfo Specific Properties"
        ''' <summary>
        ''' RequestComplete page url
        ''' </summary>
        Public Overridable Property RequestCompletePage() As String
            Get
                If Not ViewState(ATTRIBUTE_REQUEST_COMPLETE_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_REQUEST_COMPLETE_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_REQUEST_COMPLETE_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
#End Region

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(RequestCompletePage) Then
                'since value is the 'default' check the XML file for possible custom setting
                RequestCompletePage = Me.GetLinkValueFromXML(ATTRIBUTE_REQUEST_COMPLETE_PAGE)
                If String.IsNullOrEmpty(RequestCompletePage) Then
                    Me.btnSubmit.Enabled = False
                    Me.btnSubmit.ToolTip = "RequestCompletePage property has not been set."
                End If
            End If

            'if values are not provide directly or from the XML file, set default values for inherited properties since 
            'control requires them to properly function
            If String.IsNullOrEmpty(Me.QueryStringRecordIDParameter) Then Me.QueryStringRecordIDParameter = "ID"
            If String.IsNullOrEmpty(Me.RedirectIDParameterName) Then Me.RedirectIDParameterName = "ID"

        End Sub
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
            Try
                Response.Expires = -1
                'If Not IsPostBack() Then
                '    If Not IsNumeric(Request.QueryString("ID")) Then
                '        'Response.Redirect(ConfigurationSettings.AppSettings("virtualdir") & _
                '        '                  "SecurityError.aspx?Message=Access to this listing is unauthorized.")
                '    End If
                '    CLng(Request.QueryString("ID"))
                'End If
                If Not IsPostBack() Then
                    If Me.SetControlRecordIDFromQueryString AndAlso _
                            Me.SetControlRecordIDFromParam() AndAlso _
                            Me.ControlRecordID <= 0 _
                            AndAlso Not Me.IsPageInAdmin() Then
                        ' no valid ID was passed in
                        'Response.Redirect(Me.GetSecurityErrorPageFromXML & "?Message=Access to this listing is unauthorized.")
                    End If
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
            Try
                If Me.SetControlRecordIDFromQueryString AndAlso _
                        Me.SetControlRecordIDFromParam() AndAlso _
                        Me.ControlRecordID > 0 Then
                    Dim lID As Long = Me.ControlRecordID

                    Dim oGE As AptifyGenericEntityBase
                    oGE = AptifyApplication.GetEntityObject("MarketPlace Information Requests", -1)
                    With oGE
                        'DateRequested
                        'Status = Requested (default, so skip)
                        'PersonID
                        'MarketPlaceListingID
                        'PersonComments

                        .SetValue("DateRequested", Now.ToShortDateString)
                        .SetValue("Status", "Requested")
                        .SetValue("PersonID", User1.PersonID)
                        .SetValue("MarketPlaceListingID", lID)
                        .SetValue("PersonComments", Me.txtComments.Text)
                    End With

                    'Me.EncryptQueryStringValue = True
                    Me.RedirectURL = RequestCompletePage
                    If oGE.Save(False) Then
                        Me.RedirectUsingPropertyValues(oGE.RecordID)
                    Else
                        Me.RedirectUsingPropertyValues(0)
                    End If
                Else
                    'what to do here?
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
    End Class
End Namespace
