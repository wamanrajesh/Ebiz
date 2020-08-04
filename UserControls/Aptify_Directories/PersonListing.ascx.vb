'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Framework.Application
Imports Aptify.Framework.BusinessLogic
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.AttributeManagement
Imports System.Data

Namespace Aptify.Framework.Web.eBusiness
    Partial Class PersonListing
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "PersonListing"

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
            Try
                ' Changes made for to allow encrypting and decrypting the URL.
                ' Changes made by Hrushikesh Jog
                If Not IsPostBack Then
                    If (Me.SetControlRecordIDFromQueryString() _
                            AndAlso Me.SetControlRecordIDFromParam() _
                            AndAlso Me.ControlRecordID > 0) _
                            OrElse Me.IsPageInAdmin Then
                        SetupPage()
                    Else
                        Response.Redirect(Me.GetSecurityErrorPageFromXML & "?Message=" & Server.UrlEncode("Member Listing not available"))
                    End If
                End If
            Catch ex As FormatException
                ' changed code to use config settings instead of hardcoded paths
                Response.Redirect(Me.GetSecurityErrorPageFromXML & "?Message=" & Server.UrlEncode("Member Listing not available"))
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try

        End Sub

        Private Sub SetupPage()

            Dim sDatabase As String = AptifyApplication.GetEntityBaseDatabase("Persons")
            Dim sSQL As String
            Dim dt As DataTable

            sSQL = "SELECT * FROM " & _
                   sDatabase & _
                   "..vwPersons WHERE ID=" & _
                   Me.ControlRecordID.ToString & " AND DirExclude=0"

            dt = DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)

            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                lblPersonName.Text = dt.Rows(0).Item("FirstName").ToString & " " & _
                                        dt.Rows(0).Item("MiddleName").ToString & " " & _
                                        dt.Rows(0).Item("LastName").ToString & " "
                lblPersonTitle.Text = dt.Rows(0).Item("Title").ToString
                If Not IsDBNull(dt.Rows(0).Item("CompanyName")) Then
                    lblCompanyName.Text = dt.Rows(0).Item("CompanyName").ToString
                End If
                lblCompanyName.Text = dt.Rows(0).Item("CompanyName").ToString

                If IsDBNull(dt.Rows(0).Item("AddressLine1")) = False AndAlso CStr(dt.Rows(0).Item("AddressLine1")) <> "" Then
                    lblAddress.Text = lblAddress.Text & CStr(dt.Rows(0).Item("AddressLine1")) & "<BR>"
                End If
                If IsDBNull(dt.Rows(0).Item("AddressLine2")) = False AndAlso CStr(dt.Rows(0).Item("AddressLine2")) <> "" Then
                    lblAddress.Text = lblAddress.Text & CStr(dt.Rows(0).Item("AddressLine2")) & "<BR>"
                End If
                If IsDBNull(dt.Rows(0).Item("AddressLine3")) = False AndAlso CStr(dt.Rows(0).Item("AddressLine3")) <> "" Then
                    lblAddress.Text = lblAddress.Text & CStr(dt.Rows(0).Item("AddressLine3")) & "<BR>"
                End If
                If IsDBNull(dt.Rows(0).Item("City")) = False AndAlso CStr(dt.Rows(0).Item("City")) <> "" Then
                    lblAddress.Text = lblAddress.Text & CStr(dt.Rows(0).Item("City")) & ", "
                End If
                If IsDBNull(dt.Rows(0).Item("State")) = False AndAlso CStr(dt.Rows(0).Item("State")) <> "" Then
                    lblAddress.Text = lblAddress.Text & CStr(dt.Rows(0).Item("State")) & " "
                End If
                If IsDBNull(dt.Rows(0).Item("ZipCode")) = False AndAlso CStr(dt.Rows(0).Item("ZipCode")) <> "" Then
                    lblAddress.Text = lblAddress.Text & CStr(dt.Rows(0).Item("ZipCode")) & "<BR>"
                End If
                If IsDBNull(dt.Rows(0).Item("Country")) = False AndAlso CStr(dt.Rows(0).Item("Country")) <> "" Then
                    lblAddress.Text = lblAddress.Text & CStr(dt.Rows(0).Item("Country"))
                End If

                lblEmail.Text = dt.Rows(0).Item("Email").ToString

                If Not IsDBNull(dt.Rows(0).Item("PhoneCountryCode")) Then
                    lblPhone.Text = dt.Rows(0).Item("PhoneCountryCode").ToString & " " & _
                                    dt.Rows(0).Item("PhoneAreaCode").ToString & " " & _
                                    dt.Rows(0).Item("Phone").ToString & _
                                    dt.Rows(0).Item("PhoneExtension").ToString & " x "
                End If

                If Not IsDBNull(dt.Rows(0).Item("FaxCountryCode")) Then
                    lblFax.Text = dt.Rows(0).Item("FaxCountryCode").ToString & " " & _
                                    CStr(dt.Rows(0).Item("FaxAreaCode")) & " " & _
                                    CStr(dt.Rows(0).Item("FaxPhone")) & " "
                End If
                ControlVisible()
                lblResult.Text = ""
            Else
                ControlINVisible()
                lblResult.Text = "There is no record available for this profile."
            End If
        End Sub
        Public Sub ControlVisible()

            lblName.Visible = True
            lblPersonName.Visible = True
            lblCompanyName.Visible = True
            lblTitle.Visible = True
            lblCompany.Visible = True
            lblPersonTitle.Visible = True
            lblAddress.Visible = True
            lblPersonAddress.Visible = True
            lblPersonEmail.Visible = True
            lblEmail.Visible = True
            lblPhone.Visible = True
            lblPersonPhone.Visible = True
            lblPersonFax.Visible = True
            lblFax.Visible = True
        End Sub
        Public Sub ControlINVisible()

            lblName.Visible = False
            lblPersonName.Visible = False
            lblCompanyName.Visible = False
            lblTitle.Visible = False
            lblCompany.Visible = False
            lblPersonTitle.Visible = False
            lblAddress.Visible = False
            lblPersonAddress.Visible = False
            lblPersonEmail.Visible = False
            lblEmail.Visible = False
            lblPhone.Visible = False
            lblPersonPhone.Visible = False
            lblPersonFax.Visible = False
            lblFax.Visible = False
        End Sub
        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            'if values are not provide directly or from the XML file, set default values for inherited properties since 
            'control requires them to properly function
            If String.IsNullOrEmpty(Me.QueryStringRecordIDParameter) Then Me.QueryStringRecordIDParameter = "ID"
        End Sub

    End Class
End Namespace
