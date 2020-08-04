'Aptify e-Business 5.5.1, July 2013

Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Applications.ProductSetup
Imports System.Data

Namespace Aptify.Framework.Web.eBusiness

    Partial Class MeetingDirections
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "MeetingDirections"

        Protected Overrides Sub SetProperties()
            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            If String.IsNullOrEmpty(Me.QueryStringRecordIDParameter) Then Me.QueryStringRecordIDParameter = "ID"
            If String.IsNullOrEmpty(Me.IsQueryStringEncrypted) Then Me.IsQueryStringEncrypted = False
            If String.IsNullOrEmpty(Me.SetControlRecordIDFromQueryString) Then Me.SetControlRecordIDFromQueryString = True
            MyBase.SetProperties()


        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            SetProperties()
            ' Page.Master.FindControl("navigation").Visible = False
            If Not IsPostBack Then
                Me.SetControlRecordIDFromParam()
                If Me.ControlRecordID > 0 Then
                    LoadDirections()
                ElseIf Request.QueryString("ID") IsNot Nothing Then
                    ' only do this if query string was provided, otherwise we are in design time
                    Throw New Exception("Security Validation Error - Invalid ID parameter")
                End If
            End If

        End Sub

        Protected Sub btnGetDirections_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetDirections.Click
            'Dilip changes Issue #13054 

            Dim fromAddress As String = txtfromStreet.Text & " " & txtfromCityStateZip.Text
            Dim toAddress As String = lblMeetingAddress.Text & " " & lblMeetingCityStateZip.Text

            'If txtfromStreet.Text.Length > 0 Then
            '    fromAddress = txtfromStreet.Text & " " & txtfromCityStateZip.Text
            'End If

            'If lblMeetingCityStateZip.Text.Length > 0 Then
            '    toAddress = lblMeetingAddress.Text & " " & lblMeetingCityStateZip.Text
            'End If
            'If lblMeetingAddress.Text.Length > 0 Then
            '    toAddress = lblMeetingAddress.Text + ", " & toAddress
            'End If

            GoogleMapsDirectionsAPI.FromAddress = fromAddress
            GoogleMapsDirectionsAPI.MeetingAddress = toAddress
            GoogleMapsDirectionsAPI.AutoLoad = True
        End Sub

        Private Sub LoadDirections()
            Dim fromStreetAddress = String.Empty
            Dim fromCityStateZip = String.Empty
            Dim meetingStreetAddress = String.Empty
            Dim meetingCityStateZip = String.Empty
            Dim fromAddress = String.Empty
            Dim meetingAddress = String.Empty
            Dim oDataAction = Me.DataAction

            Try
                Dim dt As DataTable = oDataAction.GetDataTable("select ID from vwMeetings where ProductID=" + Me.ControlRecordID.ToString)
                Dim oGE As AptifyGenericEntityBase
                Dim oPersonGE As AptifyGenericEntityBase

                If dt.Rows.Count > 0 Then
                    oGE = AptifyApplication.GetEntityObject("Meetings", CType(dt.Rows(0)("ID"), Long))
                Else
                    oGE = AptifyApplication.GetEntityObject("Meetings", -1)
                End If

                If User1.UserID > 0 Then
                    oPersonGE = AptifyApplication.GetEntityObject("Persons", User1.PersonID)
                Else
                    oPersonGE = AptifyApplication.GetEntityObject("Persons", -1)
                End If

                'Dilip Changes Issue#13056

                If oPersonGE IsNot Nothing Then

                    If oPersonGE.GetValue("AddressLine1") IsNot Nothing And oPersonGE.GetValue("AddressLine1") IsNot String.Empty Then
                        fromStreetAddress += Convert.ToString(oPersonGE.GetValue("AddressLine1").ToString()) + " "
                    End If
                    If oPersonGE.GetValue("AddressLine2") IsNot Nothing And oPersonGE.GetValue("AddressLine2") IsNot String.Empty Then
                        fromStreetAddress += Convert.ToString(oPersonGE.GetValue("AddressLine2").ToString()) + " "
                    End If
                    If oPersonGE.GetValue("AddressLine3") IsNot Nothing And oPersonGE.GetValue("AddressLine3") IsNot String.Empty Then
                        fromStreetAddress += Convert.ToString(oPersonGE.GetValue("AddressLine3").ToString()) + " "
                    End If

                    txtfromStreet.Text = fromStreetAddress

                    If oPersonGE.GetValue("City") IsNot Nothing And oPersonGE.GetValue("City") IsNot String.Empty Then
                        fromCityStateZip += Convert.ToString(oPersonGE.GetValue("City").ToString()) + ","
                    End If
                    If oPersonGE.GetValue("State") IsNot Nothing And oPersonGE.GetValue("State") IsNot String.Empty Then
                        fromCityStateZip += Convert.ToString(oPersonGE.GetValue("State").ToString()) + " "
                    End If
                    If oPersonGE.GetValue("ZipCode") IsNot Nothing And oPersonGE.GetValue("ZipCode") IsNot String.Empty Then
                        fromCityStateZip += Convert.ToString(oPersonGE.GetValue("ZipCode").ToString()) + " "
                    End If
                    If oPersonGE.GetValue("Country") IsNot Nothing And oPersonGE.GetValue("Country") IsNot String.Empty Then
                        fromCityStateZip += Convert.ToString(oPersonGE.GetValue("Country").ToString())
                    End If

                    txtfromCityStateZip.Text = fromCityStateZip
                End If
                If oGE IsNot Nothing Then
                    If oGE.GetValue("AddressLine1") IsNot Nothing And oGE.GetValue("AddressLine1") IsNot String.Empty Then
                        meetingStreetAddress += Convert.ToString(oGE.GetValue("AddressLine1").ToString()) + " "
                    End If
                    If oGE.GetValue("AddressLine2") IsNot Nothing And oGE.GetValue("AddressLine2") IsNot String.Empty Then
                        meetingStreetAddress += Convert.ToString(oGE.GetValue("AddressLine2").ToString()) + " "
                    End If
                    If oGE.GetValue("AddressLine3") IsNot Nothing And oGE.GetValue("AddressLine3") IsNot String.Empty Then
                        meetingStreetAddress += Convert.ToString(oGE.GetValue("AddressLine3").ToString()) + " "
                    End If

                    lblMeetingAddress.Text = meetingStreetAddress

                    If oGE.GetValue("City") IsNot Nothing And oGE.GetValue("City") IsNot String.Empty Then
                        meetingCityStateZip += Convert.ToString(oGE.GetValue("City").ToString()) + ","
                    End If
                    If oGE.GetValue("State") IsNot Nothing And oGE.GetValue("State") IsNot String.Empty Then
                        meetingCityStateZip += Convert.ToString(oGE.GetValue("State").ToString()) + " "
                    End If
                    If oGE.GetValue("ZipCode") IsNot Nothing And oGE.GetValue("ZipCode") IsNot String.Empty Then
                        meetingCityStateZip += Convert.ToString(oGE.GetValue("ZipCode").ToString()) + " "
                    End If
                    If oGE.GetValue("Country") IsNot Nothing And oGE.GetValue("Country") IsNot String.Empty Then
                        meetingCityStateZip += Convert.ToString(oGE.GetValue("Country").ToString())
                    End If

                    lblMeetingCityStateZip.Text = meetingCityStateZip
                End If
                'Dilip changes Issue #13054 
                'From Address
                If fromCityStateZip.Length > 0 AndAlso fromStreetAddress.Length > 0 Then
                    fromAddress = fromStreetAddress & " " & fromCityStateZip
                Else
                    If fromStreetAddress.Length > 0 Then
                        fromAddress = fromStreetAddress
                    Else
                        fromAddress = "22102"
                        txtfromCityStateZip.Text = fromAddress
                    End If
                End If

                'Meeting Address
                If meetingCityStateZip.Length > 0 AndAlso meetingStreetAddress.Length > 0 Then
                    meetingAddress = meetingStreetAddress & " " & meetingCityStateZip
                Else
                    If meetingStreetAddress.Length > 0 Then
                        meetingAddress = meetingStreetAddress
                    Else
                        meetingAddress = "22102"
                        lblMeetingCityStateZip.Text = meetingAddress
                    End If
                End If

                GoogleMapsDirectionsAPI.FromAddress = fromAddress
                GoogleMapsDirectionsAPI.MeetingAddress = meetingAddress
                GoogleMapsDirectionsAPI.AutoLoad = True

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
    End Class
End Namespace