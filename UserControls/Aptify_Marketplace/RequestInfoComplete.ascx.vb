'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports System.Data

Namespace Aptify.Framework.Web.eBusiness.MarketPlace
    Partial Class RequestInfoComplete
        Inherits BaseUserControlAdvanced

        Private m_lListingID As Long
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "RequestInfoComplete"

        Protected Overrides Sub SetProperties()
            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            'if values are not provide directly or from the XML file, set default values for inherited properties since 
            'control requires them to properly function
            If String.IsNullOrEmpty(Me.QueryStringRecordIDParameter) Then Me.QueryStringRecordIDParameter = "ID"

        End Sub
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
            Try
                Response.Expires = -1
                If Not IsPostBack() Then
                    If (Me.SetControlRecordIDFromQueryString AndAlso _
                            Me.SetControlRecordIDFromParam()) _
                            OrElse Me.IsPageInAdmin() Then
                        If Me.ControlRecordID > 0 Then
                            'Request Info sumbission was successful
                            Me.lblFailure.Visible = False
                            Me.lblSuccess.Visible = True
                        Else
                            'Request save failed
                            Me.lblFailure.Visible = True
                            Me.lblSuccess.Visible = False
                        End If
                    Else
                        Response.Redirect(Me.GetSecurityErrorPageFromXML & "?Message=Access to this listing is unauthorized. Page parameters possibly incorrect.")
                    End If
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
    End Class
End Namespace
