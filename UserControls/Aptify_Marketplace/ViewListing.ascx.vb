'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports System.Data
Imports Aptify.Framework.BusinessLogic.GenericEntity

Namespace Aptify.Framework.Web.eBusiness.MarketPlace
    Partial Class ViewListing
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "ViewListing"

        Protected Overrides Sub SetProperties()
            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()
        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
            Try
                Response.Expires = -1
                If Not IsPostBack() Then
                    If Me.SetControlRecordIDFromQueryString AndAlso _
                            Me.SetControlRecordIDFromParam() AndAlso _
                            (Me.ControlRecordID > 0 OrElse Me.IsPageInAdmin()) Then
                        ListingDisplay.DisplayListing(Me.ControlRecordID)
                    Else
                        ' remove hard-coded virtual directory RFB 7/25/03
                        Response.Redirect(Me.GetSecurityErrorPageFromXML & "?Message=Access to this listing is unauthorized.")
                    End If
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
    End Class
End Namespace
