'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports System.Data
Imports Aptify.Framework.BusinessLogic.GenericEntity

Namespace Aptify.Framework.Web.eBusiness.Chapters
    Partial Class ChapterEditControl
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "ChapterEdit"
        'suraj Issue 12234 Notice that Non Login User able to save the  changes .
        Protected Const ATTRIBUTE_LOGIN_PAGE As String = "LoginPage"

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'Create an object for Commonmethods
            Dim oCommonMethods As New Aptify.Framework.Web.eBusiness.CommonMethods(DataAction, AptifyApplication, User1, Database)
            'set control properties from XML file if needed
            SetProperties()
            Try
                If Not IsPostBack Then
                    'Suraj Issue 13234 check user is login or not
                    If User1.UserID > 0 Then
                        
                        If (Me.SetControlRecordIDFromQueryString AndAlso _
                           Me.SetControlRecordIDFromParam() AndAlso _
                           Me.ControlRecordID > 0) _
                           OrElse Me.IsPageInAdmin() Then
                            'lnkChapter.HRef = "Chapter.aspx?ID=" & Request.QueryString("ID")
                            ''Added by Suvarna for IssueID - 15158
                            ''verify if the logged in user is associated with that chapter and then only display the infomation,
                            ''otherwise redirect to security page.
                            If oCommonMethods.IsAuthorizedUser(User1.PersonID, Me.ControlRecordID) Then
                                LoadChapter()
                            Else
                                Response.Redirect(Me.GetSecurityErrorPageFromXML & "?Message=Access to this Chapter is unauthorized.")
                            End If
                        Else
                            Response.Redirect(Me.GetSecurityErrorPageFromXML & "?Message=Access to this Chapter is unauthorized.")
                        End If
                       
                    Else
                        'Suraj Issue 15370,7/31/13 revert back changes application variale to session variable
                        Session("ReturnToPage") = Request.RawUrl
                        Response.Redirect(LoginPage + "?ReturnURL=" & System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(Request.RawUrl)))

                    End If

                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        'suraj Issue 12234 Notice that Non Login User able to save the  changes .
#Region "Chapter Specific Properties"
        Public Overridable Property LoginPage() As String
            Get
                If Not ViewState(ATTRIBUTE_LOGIN_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_LOGIN_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_LOGIN_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
#End Region

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(Me.RedirectURL) Then
                Me.cmdSave.Enabled = False
                Me.cmdChapter.Enabled = False
                Me.cmdSave.ToolTip = "RedirectURL property has not been set."
                Me.cmdChapter.ToolTip = "RedirectURL property has not been set."
            End If
            'Suraj Issue 13234
            If String.IsNullOrEmpty(LoginPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                LoginPage = Me.GetLinkValueFromXML(ATTRIBUTE_LOGIN_PAGE)
            End If

            'if values are not provide directly or from the XML file, set default values for inherited properties since 
            'control requires them to properly function
            If String.IsNullOrEmpty(Me.QueryStringRecordIDParameter) Then Me.QueryStringRecordIDParameter = "ID"
            If String.IsNullOrEmpty(Me.RedirectIDParameterName) Then Me.RedirectIDParameterName = "ReportID"

        End Sub


        Private Sub LoadCountryCombo()
            Dim sSQL As String

            Try
                If cmbCountry.Items.Count = 0 Then
                    sSQL = Database & "..spGetCountryList"
                    cmbCountry.Attributes.Add("AptifyListSQL", sSQL)
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub LoadChapter()
            Dim colParams(0) As Data.IDataParameter
            Dim ID As Data.IDataParameter = Nothing
            Dim bIsQueryStringValid As Boolean = False

            Try

                If Me.SetControlRecordIDFromParam() Then
                    Dim oGE As AptifyGenericEntityBase
                    oGE = Me.AptifyApplication.GetEntityObject("Companies", ControlRecordID)

                    If Not oGE Is Nothing Then
                        LoadCountryCombo()
                        Me.LoadDataFromGE(oGE)
                        LoadStateCombo()
                    Else
                        'tblChapter.Visible = False
                        lblError.Visible = True
                        lblError.Text = "Chapter Record Not Available"
                    End If
                Else
                    lblError.Visible = True
                    lblError.Text = "Chapter Record Not Available"
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try

        End Sub

        Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
            Dim oGE As AptifyGenericEntityBase
            Dim bRedirect As Boolean = False

            Try
                'lCompanyID = CLng(Request.QueryString("ID"))
                If Me.ControlRecordID > 0 Then

                    oGE = AptifyApplication.GetEntityObject("Companies", Me.ControlRecordID)
                    Me.TransferDataToGE(oGE)
                    If oGE.Save(False) Then
                        bRedirect = True
                    Else
                        lblError.Visible = True
                        lblError.Text = oGE.LastError()
                    End If
                Else
                    lblError.Visible = True
                    lblError.Text = "No Chapter Loaded."
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try

            'CP 7/15/2008 Moved Redirect outside of Try Catch Block
            If bRedirect Then
                Me.RedirectUsingPropertyValues(oGE.RecordID)
            End If

        End Sub

        '11/27/07,Added by Tamasa,Issue 5222.
        Private Sub LoadStateCombo()
            Try
                Dim sSQL As String, oItem As ListItem
                sSQL = Database & "..spGetStateList @CountryID=" & cmbCountry.SelectedValue.ToString
                cmbState.DataSource = Me.DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                cmbState.DataBind()
                oItem = cmbState.Items.FindByText(Me.ViewState("State").ToString)
                If Not oItem Is Nothing Then
                    oItem.Selected = True
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        '11/27/07,Added by Tamasa,Issue 5222.
        Protected Sub cmbCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCountry.SelectedIndexChanged
            LoadStateCombo()
        End Sub

        Protected Sub cmdChapter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChapter.Click
            Me.RedirectUsingPropertyValues(Me.ControlRecordID)
        End Sub
    End Class
End Namespace
