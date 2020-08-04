'Aptify e-Business 5.5.1, July 2013

Option Explicit On
Option Strict On

Imports Aptify.Framework.Application
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.BusinessLogic.Security
Imports System.Data

Namespace Aptify.Framework.Web.eBusiness.CustomerService
    Partial Class StandardProfileControl
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "StandardProfile"

        Private Const m_c_sPrefix As String = "__aptify_shoppingCart_"


        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()

            If Not IsPostBack Then
                LoadForm()
            End If
        End Sub

        Protected Overrides Sub SetProperties()
            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()
        End Sub

        Private Sub LoadForm()
            Try
                PopulateDropDowns()
                If User1.UserID > 0 Then
                    'lblProfileTitle.Text = "Edit User Profile"
                    LoadUserInfo()
                    valPWDMatch.Enabled = False
                    valPWDRequired.EnableClientScript = False
                    lblPWD.Visible = False
                    lblRepeatPWD.Visible = False
                    txtPassword.Visible = False
                    txtRepeatPWD.Visible = False
                    lblPasswordHintQuestion.Visible = False
                    lblPasswordHintAnswer.Visible = False
                    cmbPasswordQuestion.Visible = False
                    txtPasswordHintAnswer.Visible = False
                    valPasswordHintRequired.EnableClientScript = False
                Else
                    ' example of page-level default values
                    SetComboValue(cmbCountry, "United States")
                    PopulateState() '11/27/07,Added by Tamasa,Issue 5222.
                    SetComboValue(cmbState, "DC")
                    'lblProfileTitle.Text = "New User Profile"
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub PopulateDropDowns()
            Dim sSQL As String

            Try
                sSQL = "SELECT ID,Name FROM " & _
                       AptifyApplication.GetEntityBaseDatabase("Functions") & _
                       "..vwFunctions ORDER BY Name"
                cmbPrimaryFunction.DataSource = DataAction.GetDataTable(sSQL)
                cmbPrimaryFunction.DataTextField = "Name"
                cmbPrimaryFunction.DataValueField = "ID"
                cmbPrimaryFunction.DataBind()

                sSQL = AptifyApplication.GetEntityBaseDatabase("Addresses") & _
                       "..spGetCountryList"
                cmbCountry.DataSource = DataAction.GetDataTable(sSQL)
                cmbCountry.DataTextField = "Country"
                cmbCountry.DataValueField = "ID"
                cmbCountry.DataBind()
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub LoadUserInfo()

            Try
                txtFirstName.Text = User1.GetValue("FirstName")
                txtLastName.Text = User1.GetValue("LastName")
                txtCompany.Text = User1.GetValue("Company")
                txtTitle.Text = User1.GetValue("Title")
                txtAddressLine1.Text = User1.GetValue("AddressLine1")
                txtAddressLine2.Text = User1.GetValue("AddressLine2")
                txtAddressLine3.Text = User1.GetValue("AddressLine3")
                txtEmail.Text = User1.GetValue("Email")
                txtCity.Text = User1.GetValue("City")
                txtZipCode.Text = User1.GetValue("ZipCode")

                txtPhoneAreaCode.Text = User1.GetValue("PhoneAreaCode")
                txtPhone.Text = User1.GetValue("Phone")
                txtFaxAreaCode.Text = User1.GetValue("FaxAreaCode")
                txtFaxPhone.Text = User1.GetValue("FaxPhone")

                txtUserID.Text = User1.WebUserStringID
                txtUserID.Enabled = False
                txtPassword.Text = User1.Password
                txtRepeatPWD.Text = txtPassword.Text
                '11/27/07,Added by Tamasa,Issue 5222.
                SetComboValue(cmbCountry, User1.GetValue("Country"))
                PopulateState()
                'End
                SetComboValue(cmbState, User1.GetValue("State"))
                SetComboValue(cmbPrimaryFunction, User1.GetValue("PrimaryFunctionID"))
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub SetComboValue(ByRef cmb As System.Web.UI.WebControls.DropDownList, _
                                  ByVal sValue As String)
            Dim i As Integer

            Try
                For i = 0 To cmb.Items.Count - 1
                    If String.Compare(cmb.Items(i).Value, sValue, True) = 0 Then
                        cmb.Items(i).Selected = True
                        Exit Sub
                    End If
                    '11/27/07,Added by Tamasa,Issue 5222.
                    If String.Compare(cmb.Items(i).Text, sValue, True) = 0 Then
                        cmb.Items(i).Selected = True
                        Exit Sub
                    End If
                    'End
                Next
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Function DoSave() As Boolean
            ' This function will update the user information by passing
            ' in the data to the User object and requesting it to save
            Dim bAddressChanged As Boolean = False

            Try
                User1.SetValue("FirstName", txtFirstName.Text)
                User1.SetValue("LastName", txtLastName.Text)
                User1.SetValue("Title", txtTitle.Text)
                User1.SetValue("Email", txtEmail.Text)
                User1.SetAddValue("Email1", txtEmail.Text)
                User1.SetValue("Company", txtCompany.Text)

                If String.Compare(txtAddressLine1.Text, User1.GetValue("AddressLine1"), True) <> 0 OrElse _
                        String.Compare(txtAddressLine2.Text, User1.GetValue("AddressLine2"), True) <> 0 OrElse _
                        String.Compare(txtAddressLine3.Text, User1.GetValue("AddressLine3"), True) <> 0 OrElse _
                        String.Compare(txtCity.Text, User1.GetValue("City"), True) <> 0 OrElse _
                        String.Compare(CStr(IIf(cmbState.SelectedIndex >= 0, cmbState.SelectedItem.Text, "")), User1.GetValue("State"), True) <> 0 OrElse _
                        String.Compare(txtZipCode.Text, User1.GetValue("ZipCode"), True) <> 0 OrElse _
                        String.Compare(CStr(IIf(cmbCountry.SelectedIndex >= 0, cmbCountry.SelectedItem.Text, "")), User1.GetValue("Country"), True) <> 0 Then
                    bAddressChanged = True
                End If

                User1.SetValue("AddressLine1", txtAddressLine1.Text)
                User1.SetValue("AddressLine2", txtAddressLine2.Text)
                User1.SetValue("AddressLine3", txtAddressLine3.Text)
                User1.SetValue("City", txtCity.Text)
                User1.SetValue("State", CStr(IIf(cmbState.SelectedIndex >= 0, cmbState.SelectedItem.Text, "")))
                User1.SetValue("ZipCode", txtZipCode.Text)
                User1.SetValue("CountryCodeID", CLng(IIf(cmbCountry.SelectedIndex >= 0, cmbCountry.SelectedItem.Value, ""))) '11/27/07,Added by Tamasa,Issue 5222.
                User1.SetValue("Country", CStr(IIf(cmbCountry.SelectedIndex >= 0, cmbCountry.SelectedItem.Text, "")))
                User1.SetValue("PhoneAreaCode", txtPhoneAreaCode.Text)
                User1.SetValue("Phone", txtPhone.Text)
                User1.SetValue("FaxAreaCode", txtFaxAreaCode.Text)
                User1.SetValue("FaxPhone", txtFaxPhone.Text)
                User1.SetValue("PrimaryFunctionID", CLng(IIf(cmbPrimaryFunction.SelectedIndex >= 0, cmbPrimaryFunction.SelectedItem.Value, "-1")))
                User1.SetValue("WebUserStringID", txtUserID.Text)

                If User1.UserID <= 0 Then
                    User1.SetValue("Password", txtPassword.Text)
                    User1.SetValue("PasswordHint", cmbPasswordQuestion.SelectedItem.Text)
                    User1.SetValue("PasswordHintAnswer", txtPasswordHintAnswer.Text)
                End If
                User1.SaveValuesToSessionObject(Page.Session) ' need explicit call due to page redirect possibilities

                If User1.Save() Then
                    '2/4/08 RJK - If the Shopping Cart has started an Order, reset the Address based on the information provided.
                    If bAddressChanged Then
                        Dim sOrderXML As String
                        If Session.Item(m_c_sPrefix & "OrderXML") IsNot Nothing Then
                            sOrderXML = Session.Item(m_c_sPrefix & "OrderXML").ToString

                            If sOrderXML.Length > 0 Then
                                Me.ShoppingCart1.UpdateOrderAddress(AptifyShoppingCart.OrderAddressType.Shipping, AptifyShoppingCart.PersonAddressType.Main, 0, Me.Session, Me.Application)
                                Me.ShoppingCart1.UpdateOrderAddress(AptifyShoppingCart.OrderAddressType.Billing, AptifyShoppingCart.PersonAddressType.Main, 0, Me.Session, Me.Application)
                            End If
                        End If
                    End If

                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return False
            End Try
        End Function
        Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
            ' Dim bNewUser As Boolean

            Try
                'bNewUser = User1.UserID <= 0

                Page.Validate()
                lblError.Visible = False
                'If User1.UserID <= 0 Then
                '    If txtPassword.Text <> txtRepeatPWD.Text Or Trim$(txtPassword.Text) = "" Then
                '        lblError.Text = "Password fields must match and must not be blank"
                '        lblError.Visible = True
                '        Exit Sub
                '    End If
                'End If
                If DoSave() Then
                    'Dim bOK As Boolean
                    'If bNewUser Then
                    '    bOK = WebUserLogin1.Login(User1.WebUserStringID, txtPassword.Text, Page.User)
                    'Else
                    '    bOK = True
                    'End If
                    'If bOK Then
                    '   Commenting out following lines of code resolved Issue 5583.
                    '   Changes made by Vijay Sitlani on 11-22-2007

                    'Suraj Issue 15370,7/31/13 revert back changes application variale to session variable
                    If Len(Session("ReturnToPage")) > 0 Then
                        Dim sTemp As String
                        sTemp = CStr(Session("ReturnToPage"))
                        Session("ReturnToPage") = "" ' used only once
                        Response.Redirect(sTemp)
                    Else
                        Response.Redirect(Me.Request.ApplicationPath)
                    End If

                    'Else
                    '    lblError.Text = "Error logging in"
                    '    lblError.Visible = True
                    'End If
                Else
                    lblError.Text = User1.GetLastError()
                    If lblError.Text.IndexOf(lblError.Text, StringComparison.InvariantCultureIgnoreCase) >= 0 Then
                        lblError.Text &= "  Try a different User ID."
                    End If

                    lblError.Visible = True
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        '11/27/07,Added by Tamasa,Issue 5222.
        Private Sub PopulateState()
            Try
                Dim sSQL As String
                sSQL = Database & "..spGetStateList @CountryID=" & cmbCountry.SelectedValue.ToString
                cmbState.DataSource = DataAction.GetDataTable(sSQL)
                cmbState.DataTextField = "State"
                cmbState.DataValueField = "State"
                cmbState.DataBind()
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        '11/27/07,Added by Tamasa,Issue 5222.
        Protected Sub cmbCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCountry.SelectedIndexChanged
            PopulateState()
        End Sub

        Protected Sub LinkButton2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton2.Click
            Dim bNewUser As Boolean

            Try
                bNewUser = User1.UserID <= 0

                Page.Validate()
                lblError.Visible = False
                If User1.UserID <= 0 Then
                    If txtPassword.Text <> txtRepeatPWD.Text Or Trim$(txtPassword.Text) = "" Then
                        lblError.Text = "Password fields must match and must not be blank"
                        lblError.Visible = True
                        Exit Sub
                    End If
                End If
                If DoSave() Then
                    Dim bOK As Boolean
                    If bNewUser Then
                        bOK = WebUserLogin1.Login(User1.WebUserStringID, txtPassword.Text, Page.User)
                    Else
                        bOK = True
                    End If
                    If bOK Then
                        '   Commenting out following lines of code resolved Issue 5583.
                        '   Changes made by Vijay Sitlani on 11-22-2007
                        'Suraj Issue 15370,7/31/13 revert back changes application variale to session variable
                        If Len(Session("ReturnToPage")) > 0 Then
                            Dim sTemp As String
                            sTemp = CStr(Session("ReturnToPage"))
                            Session("ReturnToPage") = "" ' used only once
                            Response.Redirect(sTemp)
                        Else
                            Response.Redirect(Me.Request.ApplicationPath)
                        End If

                    Else
                        lblError.Text = "Error logging in"
                        lblError.Visible = True
                    End If
                Else
                    lblError.Text = User1.GetLastError()
                    If lblError.Text.IndexOf(lblError.Text, StringComparison.InvariantCultureIgnoreCase) >= 0 Then
                        lblError.Text &= "  Try a different User ID."
                    End If

                    lblError.Visible = True
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try

        End Sub

        Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
            Response.Redirect(Page.Request.ApplicationPath)

        End Sub
    End Class
End Namespace
