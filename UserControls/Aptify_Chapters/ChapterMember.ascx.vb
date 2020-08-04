'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports System.Data
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.BusinessLogic.GenericEntity

Namespace Aptify.Framework.Web.eBusiness.Chapters
    ''' <summary>
    ''' This user control displays a specific chapter member record to the user and allows the user to modify the record.
    ''' </summary>
    ''' <remarks></remarks>
    Partial Class ChapterMember
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "ChapterMember"

        Protected Overrides Sub SetProperties()
            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            'if values are not provide directly or from the XML file, set default values for inherited properties since 
            'control requires them to properly function
            If String.IsNullOrEmpty(Me.QueryStringRecordIDParameter) Then Me.QueryStringRecordIDParameter = "MemberID"

        End Sub
        ''' <summary>
        ''' This event is raised whenever a member record is saved by the control
        ''' </summary>
        ''' <remarks></remarks>
        Public Event Saved()

        Private Sub LoadCountryCombo()
            Dim sSQL As String
            Dim dt As DataTable
            Try

                'sSQL = AptifyApplication.GetEntityBaseDatabase("Addresses") & _
                '       "..spGetCountryList"
                'dt = DataAction.GetDataTable(sSQL)
                'cmbCountry.DataSource = dt
                'cmbCountry.DataTextField = "Country"
                'cmbCountry.DataValueField = "ID"
                'cmbCountry.DataBind()

                If cmbCountry.Items.Count = 0 Then
                    sSQL = Database & "..spGetCountryList"
                    cmbCountry.Attributes.Add("AptifyListSQL", sSQL)
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        ''' <summary>
        ''' Loads a particular member record
        ''' </summary>
        ''' <param name="MemberID">Person ID</param>
        ''' <remarks></remarks>
        Public Sub LoadMember(ByVal MemberID As Long)
            ' load up the member information into the fields
            Dim oGE As Aptify.Framework.BusinessLogic.GenericEntity.AptifyGenericEntityBase
            Me.EnsureChildControls()

            Try
                ViewState.Add("MemberID", MemberID)
                oGE = Me.AptifyApplication.GetEntityObject("Persons", MemberID)
                ViewState.Add("State", oGE.GetValue("State"))
                LoadDataFromGE(oGE)
                Me.LoadStateCombo()
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
            ' update the database from the text fields
            Dim oGE As AptifyGenericEntityBase
            Dim lMemberID As Long

            Try
                lMemberID = CLng(ViewState.Item("MemberID"))
                oGE = AptifyApplication.GetEntityObject("Persons", lMemberID)
                Me.TransferDataToGE(oGE)
                'oGE.SetValue("Country", cmbCountry.SelectedItem.Text)
                'oGE.SetValue("CountryCodeID", cmbCountry.SelectedValue)
                'oGE.SetValue("State", cmbState.SelectedItem.Text)
                If oGE.Save(False) Then
                    Dim sReturnToPageURL As String
                    'Suraj Issue 15370,7/31/13 revert back changes application variale to session variable
                    If Session("ReturnToPage") IsNot Nothing Then
                        sReturnToPageURL = Session("ReturnToPage").ToString()
                        Response.Redirect(sReturnToPageURL)
                    End If
                    RaiseEvent Saved()
                    lblError.Visible = False
                Else
                    lblError.Text = oGE.LastError
                    lblError.Visible = True
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()

            If Not IsPostBack Then
                'If (Me.SetControlRecordIDFromQueryString AndAlso _
                '        Me.SetControlRecordIDFromParam() AndAlso _
                '        Me.ControlRecordID > 0) _
                '        OrElse Me.IsPageInAdmin() Then
                '    LoadCountryCombo()
                '    LoadStateCombo()
                'Else
                '    Response.Redirect(System.Configuration.ConfigurationManager.AppSettings("virtualdir") & _
                '                   "SecurityError.aspx?Message=Access to this Chapter is unauthorized.")
                'End If

                '20090128 MAS: because this control is used inside another control (and can also be used in its own page)
                '              I needed to handle the querystring logic manually (because the page properties in the 
                '              BasicUserControlAdvanced do not work for a control within a control.
                Try
                    Dim bLoadControl As Boolean = False
                    'QueryStringRecordIDParameter = "MemberID"
                    If Request.QueryString(Me.QueryStringRecordIDParameter) Is Nothing Then
                        bLoadControl = True
                    End If
                    If Not bLoadControl Then
                        Dim sQueryStringValue As String = Request.QueryString(Me.QueryStringRecordIDParameter).ToString()
                        If Len(sQueryStringValue) > 0 Then
                            'If Me.IsQueryStringEncrypted OrElse Not IsNumeric(sQueryStringValue) Then
                            If Me.IsQueryStringEncrypted Then
                                ' attempt to decrypt
                                Try
                                    sQueryStringValue = Aptify.Framework.Web.Common.WebCryptography.Decrypt(sQueryStringValue)
                                    bLoadControl = True
                                    Me.ControlRecordID = CLng(sQueryStringValue)
                                Catch ex As Exception
                                    Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                                End Try
                            ElseIf IsNumeric(sQueryStringValue) Then
                                bLoadControl = True
                                Me.ControlRecordID = CLng(sQueryStringValue)
                            End If
                        End If
                    End If
                    If bLoadControl OrElse Me.IsPageInAdmin() Then
                        LoadCountryCombo()
                        LoadStateCombo()
                        LoadMember(Me.ControlRecordID)
                    Else
                        Response.Redirect(Me.GetSecurityErrorPageFromXML & "?Message=Access to this Chapter Member is unauthorized..")
                    End If
                Catch ex As Exception
                    Response.Redirect(Me.GetSecurityErrorPageFromXML & "?Message=Access to this Chapter Member is unauthorized.")
                End Try
            End If
        End Sub

        '11/27/07,Added by Tamasa,Issue 5222.
        Private Sub LoadStateCombo()
            Try
                Dim sSQL As String, oItem As ListItem
                If cmbCountry.SelectedValue IsNot Nothing AndAlso cmbCountry.SelectedValue.ToString.Length > 0 Then
                    sSQL = Database & "..spGetStateList @CountryID=" & cmbCountry.SelectedValue.ToString
                    cmbState.DataSource = Me.DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)
                    cmbState.DataBind()
                    oItem = cmbState.Items.FindByText(Me.ViewState("State").ToString)
                    If Not oItem Is Nothing Then
                        oItem.Selected = True
                    End If
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        '11/27/07,Added by Tamasa,Issue 5222.
        Protected Sub cmbCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCountry.SelectedIndexChanged
            LoadStateCombo()
        End Sub
    End Class
End Namespace
