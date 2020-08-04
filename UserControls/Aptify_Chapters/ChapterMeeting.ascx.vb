'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports System.Data
Imports Aptify.Framework.BusinessLogic.GenericEntity

Namespace Aptify.Framework.Web.eBusiness.Chapters
    Partial Class ChapterMeetingControl
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CHAPTER_REDIRECT_PAGE As String = "ChapterRedirectPage"
        Protected Const ATTRIBUTE_CHAPTER_PAGE_QUERYSTRING_NAME As String = "ChapterPageQueryStringName"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "ChapterMeeting"


#Region "Chapter Specific Properties"

        Private m_sChapterPage As String
        Private m_sChapPageQueryString As String
        Private m_ChapterIDQueryStringName As String

        Private Property ChapterID() As Long
            Get
                If ViewState.Item("ChapterID") IsNot Nothing Then
                    Return CLng(ViewState.Item("ChapterID"))
                Else
                    Return -1
                End If
            End Get
            Set(ByVal value As Long)
                ViewState.Item("ChapterID") = value
            End Set
        End Property

        <System.ComponentModel.Category("Chapter Specific Properties")> _
        Public Property ChapterRedirectPage() As String
            Get
                Return m_sChapterPage
            End Get
            Set(ByVal value As String)
                m_sChapterPage = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        <System.ComponentModel.Category("Chapter Specific Properties")> _
        <System.ComponentModel.DefaultValue("ID")> _
        Public Property ChapterPageQueryStringName() As String
            Get
                If String.IsNullOrEmpty(m_sChapPageQueryString) Then
                    m_sChapPageQueryString = "ID"
                End If
                Return m_sChapPageQueryString
            End Get
            Set(ByVal value As String)
                m_sChapPageQueryString = value
            End Set
        End Property

#End Region

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'Create an object for Commonmethods
            Dim oCommonMethods As New Aptify.Framework.Web.eBusiness.CommonMethods(DataAction, AptifyApplication, User1, Database)
            'set control properties from XML file if needed
            SetProperties()
            Try
                If Not IsPostBack Then
                    'Commented by Dipali for story no:14902.
                    'If (Me.SetControlRecordIDFromQueryString AndAlso _
                    '        Me.SetControlRecordIDFromParam() AndAlso _
                    '        Me.ControlRecordID > 0) _
                    '        OrElse Me.IsPageInAdmin() Then
                    'End   
                    If Me.SetControlRecordIDFromQueryString Then
                        'Added by Suvarna for IssueID - 15158
                        ''verify if the logged in user is associated with that chapter and then only display the infomation,
                        ''otherwise redirect to security page.
                        If User1.UserID > 0 Then
                            Me.SetControlRecordIDFromParam()
                            'Added by Suraj for IssueID - 15158 , 3/21/13 change made with suvarna for getting chapter id from query string
                            If Request.QueryString("ChapterID") IsNot Nothing Then
                                If oCommonMethods.IsAuthorizedUser(User1.PersonID, CLng(Aptify.Framework.Web.Common.WebCryptography.Decrypt(Request.QueryString("ChapterID")))) Then
                                    LoadForm()
                                Else
                                    Response.Redirect(Me.GetSecurityErrorPageFromXML & "?Message=Access to this Chapter Meeting is unauthorized.")
                                End If
                            Else
                                Response.Redirect(Me.GetSecurityErrorPageFromXML & "?Message=Access to this Chapter Meeting is unauthorized.")
                            End If
                        Else
                            Response.Redirect(Me.GetSecurityErrorPageFromXML & "?Message=Access to this Chapter Meeting is unauthorized.")
                        End If
                    End If
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(ChapterRedirectPage) Then
                ChapterRedirectPage = Me.GetLinkValueFromXML(ATTRIBUTE_CHAPTER_REDIRECT_PAGE)
                If String.IsNullOrEmpty(ChapterRedirectPage) Then
                    Me.lnkChapter.Enabled = False
                    Me.lnkChapter.ToolTip = "ChapterRedirectPage property has not been set."
                End If
            End If
            If ChapterPageQueryStringName = "ID" Then
                'since value is the 'default' check the XML file for possible custom setting
                If Not String.IsNullOrEmpty(Me.GetPropertyValueFromXML(ATTRIBUTE_CHAPTER_PAGE_QUERYSTRING_NAME)) Then
                    ChapterPageQueryStringName = Me.GetPropertyValueFromXML(ATTRIBUTE_CHAPTER_PAGE_QUERYSTRING_NAME)
                End If
            End If
            If String.IsNullOrEmpty(Me.RedirectURL) Then
                Me.cmdSave.Enabled = False
                Me.cmdSave.ToolTip = "RedirectURL property has not been set."
            End If

            'if values are not provide directly or from the XML file, set default values for inherited properties since 
            'control requires them to properly function
            If String.IsNullOrEmpty(Me.QueryStringRecordIDParameter) Then Me.QueryStringRecordIDParameter = "ID"
            If String.IsNullOrEmpty(Me.RedirectIDParameterName) Then Me.RedirectIDParameterName = "ID"

        End Sub

        Private Sub LoadCombos()
            Dim sSQL As String

            '2/6/08 RJK - Updated to work with new State Combo filtered by Country
            'sSQL = Database & "..spGetStateList"
            'cmbState.DataSource = DataAction.GetDataTable(sSQL)
            'cmbState.DataBind()

            sSQL = Database & "..spGetCountryList"
            cmbCountry.DataSource = DataAction.GetDataTable(sSQL)
            cmbCountry.DataBind()

        End Sub

        Private Sub LoadForm()
            Dim oGE As AptifyGenericEntityBase
            Dim lID As Long
            Dim oItem As ListItem
            Dim bPageLoaded As Boolean = True

            If Not Request.QueryString("ChapterID") Is Nothing Then
                Dim sQueryStringValue As String = Request.QueryString("ChapterID").ToString()
                Dim sMeetingIDValue As String = Request.QueryString("ID").ToString()
                If Me.EncryptQueryStringValue Then
                    Me.ChapterID = CLng(Aptify.Framework.Web.Common.WebCryptography.Decrypt(sQueryStringValue))
                    Me.ControlRecordID = CLng(Aptify.Framework.Web.Common.WebCryptography.Decrypt(sMeetingIDValue))
                ElseIf IsNumeric(sQueryStringValue) Then
                    Me.ChapterID = CLng(sQueryStringValue)
                Else
                    bPageLoaded = False
                End If
            End If

            If bPageLoaded Then
                lID = Me.ControlRecordID

                LoadCombos()

                oGE = AptifyApplication.GetEntityObject("Chapter Meetings", lID)
                If lID > 0 Then
                    Me.ChapterID = CLng(oGE.GetValue("ChapterID"))
                    LoadChapterInfo(Me.ChapterID)
                Else
                    LoadChapterInfo(Me.ChapterID)
                End If

                txtName.Text = CStr(oGE.GetValue("Name"))
                txtDescription.Text = CStr(oGE.GetValue("Description"))
                txtLocation.Text = CStr(oGE.GetValue("Location"))
                txtAddressLine1.Text = CStr(oGE.GetValue("AddressLine1"))
                txtCity.Text = CStr(oGE.GetValue("City"))
                txtZIP.Text = CStr(oGE.GetValue("ZipCode"))

                oItem = cmbCountry.Items.FindByText(CStr(oGE.GetValue("Country")))
                If Not oItem Is Nothing Then
                    oItem.Selected = True
                End If

                Me.PopulateState()

                oItem = cmbState.Items.FindByText(CStr(oGE.GetValue("State")))
                If Not oItem Is Nothing Then
                    oItem.Selected = True
                End If

                txtStartDate.Text = CStr(oGE.GetValue("StartDate"))
                txtEndDate.Text = CStr(oGE.GetValue("EndDate"))

                oItem = cmbType.Items.FindByText(CStr(oGE.GetValue("Type")))
                If Not oItem Is Nothing Then
                    oItem.Selected = True
                End If
                oItem = cmbStatus.Items.FindByText(CStr(oGE.GetValue("Status")))
                If Not oItem Is Nothing Then
                    oItem.Selected = True
                End If

            Else
                lblError.Text = "Page Error: Chapter Meeting does not exist or Chapter value is not correct"
                lblError.Visible = True
            End If
        End Sub

        Private Sub LoadChapterInfo(ByVal ChapterID As Long)
            lblChapterName.Text = AptifyApplication.GetEntityRecordName("Companies", ChapterID)
        End Sub

        Private Sub lnkChapter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkChapter.Click
            SetControlRecordIDFromParam()
            Dim sRedirect As String = ""
            If Me.EncryptQueryStringValue Then
                sRedirect = Me.ChapterRedirectPage & "?" & Me.ChapterPageQueryStringName & "=" & _
                    System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(Me.ChapterID.ToString))
            Else
                sRedirect = Me.ChapterRedirectPage & "?" & Me.ChapterPageQueryStringName & "=" & Me.ChapterID.ToString
            End If
            Response.Redirect(sRedirect)
        End Sub

        Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
            Dim oGE As AptifyGenericEntityBase
            Dim lID As Long
            Dim bRedirect As Boolean = False

            Try
                If Page.IsValid Then
                    lID = Me.ControlRecordID
                    oGE = AptifyApplication.GetEntityObject("Chapter Meetings", lID)
                    If lID <= 0 Then
                        oGE.SetValue("ChapterID", Me.ChapterID)
                    Else
                        Me.ChapterID = CLng(oGE.GetValue("ChapterID"))
                    End If

                    oGE.SetValue("Name", txtName.Text)
                    oGE.SetValue("Description", txtDescription.Text)
                    oGE.SetValue("Location", txtLocation.Text)
                    oGE.SetValue("AddressLine1", txtAddressLine1.Text)
                    oGE.SetValue("City", txtCity.Text)
                    oGE.SetValue("State", CStr(IIf(cmbState.SelectedIndex >= 0, cmbState.SelectedItem.Text, "")))
                    oGE.SetValue("ZipCode", txtZIP.Text)

                    oGE.Fields("AddressID").EmbeddedObject.SetValue("CountryCodeID", cmbCountry.SelectedValue)
                    oGE.SetValue("Country", cmbCountry.SelectedValue)

                    oGE.SetValue("StartDate", txtStartDate.Text)
                    oGE.SetValue("EndDate", txtEndDate.Text)
                    oGE.SetValue("Type", cmbType.SelectedItem.Value)
                    oGE.SetValue("Status", cmbStatus.SelectedItem.Value)
                    If oGE.Save(False) Then
                        bRedirect = True

                    Else
                        lblError.Text = oGE.LastError()
                        lblError.Visible = True
                    End If
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try

            If bRedirect Then
                Me.RedirectUsingPropertyValues(Me.ChapterID)
            End If
        End Sub

        Private Sub vldStartDate_ServerValidate(ByVal source As System.Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles vldStartDate.ServerValidate
            args.IsValid = IsDate(args.Value)
        End Sub

        Private Sub vldEndDate_ServerValidate(ByVal source As System.Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles vldEndDate.ServerValidate
            args.IsValid = IsDate(args.Value) AndAlso _
                           IsDate(txtStartDate.Text) AndAlso _
                           CDate(args.Value) > CDate(txtStartDate.Text)
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
    End Class
End Namespace
