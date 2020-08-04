'Aptify e-Business 5.5.1, July 2013
Imports Aptify.Framework.Application
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.BusinessLogic.Security
Imports System.Data
Imports System.IO
Imports Aptify.Framework.Web.eBusiness.SocialNetworkIntegration
Imports Aptify.Framework.BusinessLogic.ProcessPipeline
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports System.Linq

Namespace Aptify.Framework.Web.eBusiness
    Partial Class ChangePassword
        Inherits BaseUserControlAdvanced
        Protected Const ATTRIBUTE_PWD_LENGTH As String = "minPwdLength"
        Protected Const ATTRIBUTE_PWD_UPPERCASE As String = "minPwdUpperCase"
        Protected Const ATTRIBUTE_PWD_LOWERCASE As String = "minPwdLowerCase"
        Protected Const ATTRIBUTE_PWD_NUMBERS As String = "minPwdNumbers"
        Protected Const ATTRIBUTE_SAVE_PWD As String = "SavePassword"
        Protected Const ATTRIBUTE_CANCEL_PWD As String = "CancelPassword"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "ChangePassword"
        ''Added By Suvarna For Issue Id - 19876
        Public sOldPWD As String = String.Empty

        Public Overridable ReadOnly Property MinPwdLength() As Integer
            Get
                If ViewState.Item(ATTRIBUTE_PWD_LENGTH) IsNot Nothing Then
                    Return CInt(ViewState.Item(ATTRIBUTE_PWD_LENGTH))
                Else
                    Dim value As Integer = Me.GetGlobalAttributeIntegerValue(ATTRIBUTE_PWD_LENGTH)
                    If value <> -1 Then
                        ViewState.Item(ATTRIBUTE_PWD_LENGTH) = value
                        Return CInt(value)
                    Else
                        Return 6 'default value when nothing is set in config file
                    End If
                End If
            End Get
        End Property
        ''' <summary>
        ''' Minimum number of UpperCase letters required in password as provided in config file, default value is 1 when config file is not set
        ''' </summary>
        Public Overridable ReadOnly Property MinPwdUpperCase() As Integer
            Get
                If ViewState.Item(ATTRIBUTE_PWD_UPPERCASE) IsNot Nothing Then
                    Return CInt(ViewState.Item(ATTRIBUTE_PWD_UPPERCASE))
                Else
                    Dim value As Integer = Me.GetGlobalAttributeIntegerValue(ATTRIBUTE_PWD_UPPERCASE)
                    If value <> -1 Then
                        ViewState.Item(ATTRIBUTE_PWD_UPPERCASE) = value
                        Return CInt(value)
                    Else
                        Return 1 'default value when nothing is set in config file
                    End If
                End If
            End Get
        End Property
        ''' <summary>
        ''' Minimum number of LowerCase letters required in password as provided in config file, default value is 1 when config file is not set
        ''' </summary>
        Public Overridable ReadOnly Property MinPwdLowerCase() As Integer
            Get
                If ViewState.Item(ATTRIBUTE_PWD_LOWERCASE) IsNot Nothing Then
                    Return CInt(ViewState.Item(ATTRIBUTE_PWD_LOWERCASE))
                Else
                    Dim value As Integer = Me.GetGlobalAttributeIntegerValue(ATTRIBUTE_PWD_LOWERCASE)
                    If value <> -1 Then
                        ViewState.Item(ATTRIBUTE_PWD_LOWERCASE) = value
                        Return CInt(value)
                    Else
                        Return 1 'default value when nothing is set in config file
                    End If
                End If
            End Get
        End Property
        ''' <summary>
        ''' Minimum number of numeric letters required in password as provided in config file, default value is 1 when config file is not set
        ''' </summary>
        Public Overridable ReadOnly Property MinPwdNumbers() As Integer
            Get
                If ViewState.Item(ATTRIBUTE_PWD_NUMBERS) IsNot Nothing Then
                    Return CInt(ViewState.Item(ATTRIBUTE_PWD_NUMBERS))
                Else
                    Dim value As Integer = Me.GetGlobalAttributeIntegerValue(ATTRIBUTE_PWD_NUMBERS)
                    If value <> -1 Then
                        ViewState.Item(ATTRIBUTE_PWD_NUMBERS) = value
                        Return CInt(value)
                    Else
                        Return 1 'default value when nothing is set in config file
                    End If
                End If
            End Get
        End Property

        Public Overridable Property SavePasswordPage() As String
            Get
                If Not ViewState(ATTRIBUTE_SAVE_PWD) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_SAVE_PWD))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_SAVE_PWD) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        Public Overridable Property CancelPasswordPage() As String
            Get
                If Not ViewState(ATTRIBUTE_CANCEL_PWD) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_CANCEL_PWD))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_CANCEL_PWD) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()



            If String.IsNullOrEmpty(SavePasswordPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                SavePasswordPage = Me.GetLinkValueFromXML(ATTRIBUTE_SAVE_PWD)

            End If

            If String.IsNullOrEmpty(CancelPasswordPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                CancelPasswordPage = Me.GetLinkValueFromXML(ATTRIBUTE_CANCEL_PWD)

            End If

        End Sub

        Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
            Dim intpassword As Integer
            'Neha Issue 14408,01/20/13, added style for errormessages  
            If Not IsPasswordComplexPopup(txtNewPassword.Text) Then
                lblerrorLength.Visible = True
                CompareValidator.Enabled = True
                Exit Sub
            End If
            ''Added By Suvarna for issue Id -19786
            If String.IsNullOrEmpty(txtoldpassword.Text) Then
                intpassword = WebUserLogin1.UpdateUserPassword(User1.WebUserStringID, soldPWD, txtNewPassword.Text, Nothing, Nothing, Page.User)
            Else
                intpassword = WebUserLogin1.UpdateUserPassword(User1.WebUserStringID, txtoldpassword.Text, txtNewPassword.Text, Nothing, Nothing, Page.User)
            End If

            If (intpassword = 0) Then
                CompareValidator.Enabled = True
                'lblPasswordsuccess.Text = "Password Updated Successfully"
                ''Added by Suvarna for Issue Id 17790
                If ClearTempPassword(User1.WebUserStringID) Then
                    ''End by Suvarna for Issue Id 17790
                    Me.Response.Redirect(SavePasswordPage, False)
                    ''Added by Suvarna for Issue Id 17790
                End If
                ''End by Suvarna for Issue Id 17790 
            End If
            If (intpassword = 1) Then
                CompareValidator.Enabled = True
                lblerrorLength.Text = "<span style='color:red'>No user match or there is no access to the encryption key.</span>"
                Exit Sub
            End If
            'Neha, Issue 14408, 03/20/13, changed validationmessage 
            If (intpassword = 2) Then
                CompareValidator.Enabled = False
                lblerrorLength.Text = "<span style='color:red'>The Current Password you entered is incorrect. Please try again.</span>"
                Exit Sub
            End If
            If (intpassword = 4) Then
                CompareValidator.Enabled = True
                lblerrorLength.Text = "<span style='color:red'>Current and New Password should not same.</span>"
                Exit Sub
            End If
            If (intpassword = 3) Then
                CompareValidator.Enabled = True
                lblerrorLength.Text = "<span style='color:red'>Password update failed. Please try again.</span>"
                Exit Sub
            End If
            'radwinPassword.VisibleOnPageLoad = False
        End Sub

        ''Added by Suvarna for Issue Id 17790
        Private Function ClearTempPassword(ByVal sUserID As String) As Boolean
            Dim bReturn As Boolean = False
            Try
                Dim lUserID = GetUserIDFromName(sUserID)
                If lUserID > 0 Then
                    Dim sSQL As String = "spClearWebUserTemporaryPassword"
                    Dim colParams(0) As Data.IDataParameter
                    colParams(0) = DataAction.GetDataParameter("@WebUserID", SqlDbType.BigInt, lUserID)
                    If CLng(DataAction.ExecuteScalarParametrized(sSQL, CommandType.StoredProcedure, colParams)) = 1 Then
                        bReturn = True
                    End If
                End If
            Catch ex As Exception
                ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
            Return bReturn
        End Function
        Private Function GetUserIDFromName(ByVal sUID As String) As Long
            Dim lResult As Long = 0
            Dim sSQL As String
            Dim colParams(0) As Data.IDataParameter
            Dim UserID As Data.IDataParameter = Nothing

            Try
                sSQL = "SELECT ID FROM " & Database & "..vwWebUsers WHERE UserID=@UserID"
                colParams(0) = DataAction.GetDataParameter("@UserID", SqlDbType.NVarChar, sUID)
                lResult = CLng(DataAction.ExecuteScalarParametrized(sSQL, CommandType.Text, colParams))
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
            Return lResult
        End Function
        ''End by Suvarna for Issue Id 17790
        Private Function IsPasswordComplexPopup(ByVal password As String) As Boolean
            Dim result As Boolean = False
            'Aparna for issue 12964 for showing password length validation
            lblerrorLength.Text = ""
            If password.Length >= MinPwdLength Then
                result = System.Text.RegularExpressions.Regex.IsMatch(password, "(?=(.*[A-Z].*){" & MinPwdUpperCase & ",})(?=(.*[a-z].*){" & MinPwdLowerCase & ",})(?=(.*\d.*){" & MinPwdNumbers & ",})")
            End If
            'Aparna for issue 12964 for showing password length validation
            'Neha Issue 14408,01/24/13 added Style for Password criteria
            If Not result Then
                lblerrorLength.Text = "<span style='font-weight: bold; color:red; font-size:11px;'>The password criteria has not been met. Please try again.</span> " & "<br/>" &
                                      "<span style='font-style:italic; font-size: 7.3pt; font-weight: bold;'>Password must be a minimum length of " & MinPwdLength.ToString & " with at least " & _
                                       MinPwdLowerCase & " lower-case letter(s) and " & MinPwdUpperCase & " upper-case letter(s) and " & MinPwdNumbers & " number(s).</span>"
            End If
            Return result
        End Function
        Protected Sub btnCancelpop_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelpop.Click
            'radwinPassword.VisibleOnPageLoad = False
            Me.Response.Redirect(CancelPasswordPage, False)
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            '  SetProperties()
            If Not IsPostBack Then
                ''Added by Suvarna for Issue Id 17790 
                SetProperties()
                ''End by Suvarna for Issue Id 17790 
                lblerrorLength.Text = ""
                lblpwdmsg.Text = "Please Change Your Password."
            End If
            ''Added by Suvarna for Issue Id 17790 
            If Not Request.QueryString("o1") Is Nothing Then
                ''added by Suvarna for issue ID - 19786
                soldPWD = System.Web.HttpUtility.UrlDecode(Aptify.Framework.Web.Common.WebCryptography.Decrypt(Request.QueryString("o1").ToString()))
                txtoldpassword.Attributes.Add("value", System.Web.HttpUtility.UrlDecode(Aptify.Framework.Web.Common.WebCryptography.Decrypt(Request.QueryString("o1").ToString())))
                txtoldpassword.Enabled = "false"
            End If
            ''End by Suvarna for Issue Id 17790 
        End Sub
    End Class
End Namespace
