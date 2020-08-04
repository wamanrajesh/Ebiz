'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports System
Imports System.Data
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.Application
Imports Aptify.Framework.BusinessLogic.ProcessPipeline
Imports Aptify.Applications.OrderEntry
Imports Telerik.Web.UI

Namespace Aptify.Framework.Web.eBusiness
    Partial Class MemberCertifications
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CEU_SUBMISSION_PAGE As String = "CEUSubmissionPage"
        Protected Const ATTRIBUTE_VIEW_CERTIFICATION_PAGE As String = "ViewCertificationPage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "MyCertifications"
        Protected Const ATTRIBUTE_ADMIN_EDIT_PROFILE As String = "AdminEditprofileUrl"
        Protected Const ATTRIBUTE_LOGIN_PAGE As String = "LoginPage"
        'Neha Issue 14810,03/09/13, Declared properties for RadBinaryimage
        Protected Const ATTRIBUTE_PERSON_BLANK_IMG As String = "RadBlankImage"
        Protected Const ATTRIBUTE_PROFILE_THUMBNAIL_WIDTH As String = "ProfileThumbNailWidth"
        Protected Const ATTRIBUTE_PROFILE_THUMBNAIL_HEIGHT As String = "ProfileThumbNailHeight"
        Protected Const ATTRIBUTE_DATATABLE_CERTIFICATION As String = "dtMemCertification"




#Region "MyCertifications Specific Properties"
        ''' <summary>
        ''' CEUSubmission page url
        ''' </summary>
        Public Overridable Property CEUSubmissionPage() As String
            Get
                If Not ViewState(ATTRIBUTE_CEU_SUBMISSION_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_CEU_SUBMISSION_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_CEU_SUBMISSION_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' ViewCertification page url
        ''' </summary>
        Public Overridable Property ViewCertificationPage() As String
            Get
                If Not ViewState(ATTRIBUTE_VIEW_CERTIFICATION_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_VIEW_CERTIFICATION_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_VIEW_CERTIFICATION_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        Public Overridable Property AdminEditProfile() As String
            Get
                If Not ViewState(ATTRIBUTE_ADMIN_EDIT_PROFILE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_ADMIN_EDIT_PROFILE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_ADMIN_EDIT_PROFILE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' Login page url
        ''' </summary>
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
        'Neha, Issue 14810, 03/09/13, Overrided properties for Radbinaryimage
        ''' <summary>
        ''' ProfileThumbNailWidth
        ''' </summary>
        Public Overridable ReadOnly Property ProfileThumbNailWidth() As Integer
            Get
                Try
                    If Not String.IsNullOrEmpty(Me.GetGlobalAttributeValue(ATTRIBUTE_PROFILE_THUMBNAIL_WIDTH)) Then
                        If Not IsNumeric(Me.GetGlobalAttributeValue(ATTRIBUTE_PROFILE_THUMBNAIL_WIDTH)) Then
                            Throw New Exception("Incorrect entry for <Global>...<" & ATTRIBUTE_PROFILE_THUMBNAIL_WIDTH & ">, a numeric value is required. " & _
                                                "Please confirm the entry is correctly input in the 'Aptify_UC_Navigation.config' file.")
                        Else
                            Return CInt(Me.GetGlobalAttributeValue(ATTRIBUTE_PROFILE_THUMBNAIL_WIDTH))
                        End If
                    Else
                        Return 0
                    End If
                Catch ex As Exception
                    Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                    Return 0
                End Try
            End Get
        End Property
        ''' <summary>
        ''' ProfileThumbNailHeight
        ''' </summary>
        Public Overridable ReadOnly Property ProfileThumbNailHeight() As Integer
            Get
                Try
                    If Not String.IsNullOrEmpty(Me.GetGlobalAttributeValue(ATTRIBUTE_PROFILE_THUMBNAIL_HEIGHT)) Then
                        If Not IsNumeric(Me.GetGlobalAttributeValue(ATTRIBUTE_PROFILE_THUMBNAIL_HEIGHT)) Then
                            Throw New Exception("Incorrect entry for <Global>...<" & ATTRIBUTE_PROFILE_THUMBNAIL_HEIGHT & ">, a numeric value is required. " & _
                                                "Please confirm the entry is correctly input in the 'Aptify_UC_Navigation.config' file.")
                        Else
                            Return CInt(Me.GetGlobalAttributeValue(ATTRIBUTE_PROFILE_THUMBNAIL_HEIGHT))
                        End If
                    Else
                        Return 0
                    End If
                Catch ex As Exception
                    Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                    Return 0
                End Try
            End Get
        End Property
        ''' <summary>
        ''' RadBlankImage
        ''' </summary>
        Public Overridable Property RadBlankImage() As String
            Get
                If Not ViewState(ATTRIBUTE_PERSON_BLANK_IMG) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_PERSON_BLANK_IMG))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_PERSON_BLANK_IMG) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
#End Region

        Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
            SetProperties()
            If Not IsPostBack Then
                'Anil B For issue 14344 on 28-03-2013
                'Add Expression for sorting
                AddExpression()
                If User1.UserID <= 0 Then
                    Session.Add("ReturnToPage", Request.Path)
                    Response.Redirect(LoginPage)
                End If
                LoadGrid()
            End If
        End Sub

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()
            If String.IsNullOrEmpty(CEUSubmissionPage) Then
                CEUSubmissionPage = Me.GetLinkValueFromXML(ATTRIBUTE_CEU_SUBMISSION_PAGE)
            End If

            If String.IsNullOrEmpty(CEUSubmissionPage) Then

                AdminEditProfile = Me.GetLinkValueFromXML(ATTRIBUTE_CEU_SUBMISSION_PAGE)
                If String.IsNullOrEmpty(AdminEditProfile) Then
                    Me.grdMembersCertifications.Enabled = False
                    Me.grdMembersCertifications.ToolTip = "CEU Submission property has not been set."
                End If
            End If

            If String.IsNullOrEmpty(ViewCertificationPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                ViewCertificationPage = Me.GetLinkValueFromXML(ATTRIBUTE_VIEW_CERTIFICATION_PAGE)
                If String.IsNullOrEmpty(ViewCertificationPage) Then
                    Me.grdMembersCertifications.Enabled = False
                    Me.grdMembersCertifications.ToolTip = "ViewCertificationPage property has not been set."
                End If
            End If

            If String.IsNullOrEmpty(AdminEditProfile) Then
                'since value is the 'default' check the XML file for possible custom setting
                AdminEditProfile = Me.GetLinkValueFromXML(ATTRIBUTE_ADMIN_EDIT_PROFILE)
                If String.IsNullOrEmpty(AdminEditProfile) Then
                    Me.grdMembersCertifications.Enabled = False
                    Me.grdMembersCertifications.ToolTip = "Admin Edit Profile property has not been set."
                End If
            End If
            If String.IsNullOrEmpty(LoginPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                LoginPage = Me.GetLinkValueFromXML(ATTRIBUTE_LOGIN_PAGE)
            End If
            If String.IsNullOrEmpty(RadBlankImage) Then
                RadBlankImage = Me.GetLinkValueFromXML(ATTRIBUTE_PERSON_BLANK_IMG)
            End If
        End Sub

        ''' <summary>
        ''' This method loads the grid on the page, override the method functionality to alter the grid loading functionality
        ''' </summary>
        ''' <remarks></remarks>
        Protected Overridable Sub LoadGrid()
            Try

                If ViewState(ATTRIBUTE_DATATABLE_CERTIFICATION) IsNot Nothing Then
                    grdMembersCertifications.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_CERTIFICATION), Data.DataTable)
                    grdMembersCertifications.DataBind()
                    Exit Sub
                End If
                Dim sSQL As String
                Dim m_sDatabase As String = AptifyApplication.GetEntityBaseDatabase("Certifications")
                Dim companyID As Integer
                Dim dt As Data.DataTable
                Dim dcolUrl As DataColumn = New DataColumn()
                Dim dCEUUrl As DataColumn = New DataColumn()
                Dim oGE As Aptify.Applications.Persons.PersonsEntity = DirectCast(Me.AptifyApplication.GetEntityObject("Persons", User1.PersonID), Aptify.Applications.Persons.PersonsEntity)
                If oGE IsNot Nothing Then
                    companyID = oGE.CompanyID
                End If
                sSQL = "EXEC spGetCompanyMemberCertifications @CompanyID=" & companyID
                dt = Me.DataAction.GetDataTable(sSQL)
                If dt IsNot Nothing Then
                    dcolUrl.Caption = "AdminEditprofileUrl"
                    dcolUrl.ColumnName = "AdminEditprofileUrl"
                    dCEUUrl.Caption = "CEUSubmissionPage"
                    dCEUUrl.ColumnName = "CEUSubmissionPage"
                    dt.Columns.Add(dcolUrl)
                    dt.Columns.Add(dCEUUrl)
                End If
                If dt.Rows.Count > 0 Then
                    For Each rw As DataRow In dt.Rows
                        rw("AdminEditprofileUrl") = AdminEditProfile + "?ID=" + rw("ID").ToString()
                        rw("CEUSubmissionPage") = CEUSubmissionPage + "?ID=" + rw("ID").ToString()
                    Next
                End If
                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                    Me.grdMembersCertifications.DataSource = dt
                    grdMembersCertifications.DataBind()
                    ViewState(ATTRIBUTE_DATATABLE_CERTIFICATION) = dt
                    grdMembersCertifications.Visible = True
                    lblmsg.Visible = False
                Else
                    grdMembersCertifications.Visible = False
                    lblmsg.Text = "Result Not Found"
                End If
                dt = Nothing
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        'Neha, Issue 14810, 03/09/13,used Radbinaryimage 
        Protected Sub grdMembersCertifications_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles grdMembersCertifications.ItemDataBound
            Try
                Dim imageMemberId As RadBinaryImage = Nothing

                If e.Item Is Nothing OrElse e.Item.FindControl("imgmember") Is Nothing Then
                    Exit Sub
                End If
                imageMemberId = CType(e.Item.FindControl("imgmember"), RadBinaryImage)
                'set the location of BlankImage to display in radbinaryimage control
                imageMemberId.ImageUrl = Me.FixLinkForVirtualPath(RadBlankImage)
                imageMemberId.DataBind()
                'Resizes the passed Image according to the specified width and height and returns the resized Image
                If Not IsDBNull(DataBinder.Eval(e.Item.DataItem, "Photo")) Then
                    Dim commonMethods As New Aptify.Framework.Web.eBusiness.CommonMethods()
                    Dim profileImage As Drawing.Image = Nothing
                    Dim width As Integer = ProfileThumbNailWidth
                    Dim height As Integer = ProfileThumbNailHeight
                    Dim aspratioWidth As Integer

                    Dim profileImageByte As Byte() = DirectCast(DataBinder.Eval(e.Item.DataItem, "Photo"), Byte())
                    If profileImageByte IsNot Nothing AndAlso profileImageByte.Length > 0 Then
                        commonMethods.getResizedImageHeightandWidth(profileImage, profileImageByte, ProfileThumbNailWidth, ProfileThumbNailHeight, aspratioWidth)
                        profileImage = commonMethods.byteArrayToImage(profileImageByte)
                        profileImageByte = commonMethods.resizeImageAndGetAsByte(profileImage, aspratioWidth, height)
                        imageMemberId.DataValue = profileImageByte
                        imageMemberId.DataBind()
                    Else
                        imageMemberId.ImageUrl = Me.FixLinkForVirtualPath(RadBlankImage)
                        imageMemberId.DataBind()
                    End If
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        ''' <summary>
        ''' This Event loads the grid  on the page
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub grdMembersCertifications_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdMembersCertifications.NeedDataSource
            If ViewState(ATTRIBUTE_DATATABLE_CERTIFICATION) IsNot Nothing Then
                grdMembersCertifications.DataSource = CType(ViewState(ATTRIBUTE_DATATABLE_CERTIFICATION), Data.DataTable)
            End If
        End Sub
        'Anil B For issue 14344 on 28-03-2013
        'Add Expression for sorting
        Private Sub AddExpression()
            Dim ExpOrderSort As New GridSortExpression
            ExpOrderSort.FieldName = "UnitTotal"
            ExpOrderSort.SetSortOrder("Descending")
            grdMembersCertifications.MasterTableView.SortExpressions.AddSortExpression(ExpOrderSort)
        End Sub
    End Class
End Namespace

