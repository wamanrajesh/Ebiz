'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Framework.Application
Imports Aptify.Framework.BusinessLogic
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.AttributeManagement
Imports System.Data

Namespace Aptify.Framework.Web.eBusiness.Committees
    Partial Class CommitteeTermControl
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced

        Protected Const ATTRIBUTE_GENERAL_IMAGE_SMALL As String = "GeneralImageSmall"
        Protected Const ATTRIBUTE_MEMBERS_IMAGE_SMALL As String = "MembersImageSmall"
        Protected Const ATTRIBUTE_FORUM_IMAGE_SMALL As String = "ForumImageSmall"
        Protected Const ATTRIBUTE_DOCUMENTS_IMAGE_SMALL As String = "DocumentsImageSmall"
        Protected Const ATTRIBUTE_GENERAL_IMAGE As String = "GeneralImage"
        Protected Const ATTRIBUTE_MEMBERS_IMAGE As String = "MembersImage"
        Protected Const ATTRIBUTE_FORUM_IMAGE As String = "ForumImage"
        Protected Const ATTRIBUTE_DOCUMENTS_IMAGE As String = "DocumentsImage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "CommitteeTerm"
        Protected Const ATTRIBUTE_DATATABLE_COMMITTEE_TERM_MEMBERS As String = "dtComitteeTermMembers"
        Private m_sView As String

#Region "CommitteeTerm Specific Properties"
        ''' <summary>
        ''' GeneralImageSmall url
        ''' </summary>
        Public Overridable Property GeneralImageSmall() As String
            Get
                If Not ViewState(ATTRIBUTE_GENERAL_IMAGE_SMALL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_GENERAL_IMAGE_SMALL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_GENERAL_IMAGE_SMALL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' MembersImageSmall url
        ''' </summary>
        Public Overridable Property MembersImageSmall() As String
            Get
                If Not ViewState(ATTRIBUTE_MEMBERS_IMAGE_SMALL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_MEMBERS_IMAGE_SMALL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_MEMBERS_IMAGE_SMALL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' ForumImageSmall url
        ''' </summary>
        Public Overridable Property ForumImageSmall() As String
            Get
                If Not ViewState(ATTRIBUTE_FORUM_IMAGE_SMALL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_FORUM_IMAGE_SMALL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_FORUM_IMAGE_SMALL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' DocumentsImageSmall url
        ''' </summary>
        Public Overridable Property DocumentsImageSmall() As String
            Get
                If Not ViewState(ATTRIBUTE_DOCUMENTS_IMAGE_SMALL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_DOCUMENTS_IMAGE_SMALL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_DOCUMENTS_IMAGE_SMALL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' GeneralImage url
        ''' </summary>
        Public Overridable Property GeneralImage() As String
            Get
                If Not ViewState(ATTRIBUTE_GENERAL_IMAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_GENERAL_IMAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_GENERAL_IMAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' MembersImage url
        ''' </summary>
        Public Overridable Property MembersImage() As String
            Get
                If Not ViewState(ATTRIBUTE_MEMBERS_IMAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_MEMBERS_IMAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_MEMBERS_IMAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' ForumImage url
        ''' </summary>
        Public Overridable Property ForumImage() As String
            Get
                If Not ViewState(ATTRIBUTE_FORUM_IMAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_FORUM_IMAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_FORUM_IMAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' DocumentsImage url
        ''' </summary>
        Public Overridable Property DocumentsImage() As String
            Get
                If Not ViewState(ATTRIBUTE_DOCUMENTS_IMAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_DOCUMENTS_IMAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_DOCUMENTS_IMAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
#End Region


        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
            If Not IsPostBack Then
                SetLeftNavImageLinks()
                LoadCommitteeTerm()
            End If
        End Sub

        'Protected Sub grdMembers_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdMembers.NeedDataSource
        '    LoadCommitteeTerm()
        'End Sub

        Protected Overrides Sub SetProperties()
            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            'if property value is the 'default' check the XML file for possible custom setting
            If String.IsNullOrEmpty(GeneralImageSmall) Then
                GeneralImageSmall = Me.GetLinkValueFromXML(ATTRIBUTE_GENERAL_IMAGE_SMALL)
            End If
            If String.IsNullOrEmpty(MembersImageSmall) Then
                MembersImageSmall = Me.GetLinkValueFromXML(ATTRIBUTE_MEMBERS_IMAGE_SMALL)
            End If
            If String.IsNullOrEmpty(ForumImageSmall) Then
                ForumImageSmall = Me.GetLinkValueFromXML(ATTRIBUTE_FORUM_IMAGE_SMALL)
            End If
            If String.IsNullOrEmpty(DocumentsImageSmall) Then
                DocumentsImageSmall = Me.GetLinkValueFromXML(ATTRIBUTE_DOCUMENTS_IMAGE_SMALL)
            End If
            'larger images
            If String.IsNullOrEmpty(GeneralImage) Then
                GeneralImage = Me.GetLinkValueFromXML(ATTRIBUTE_GENERAL_IMAGE)
            End If
            If String.IsNullOrEmpty(MembersImage) Then
                MembersImage = Me.GetLinkValueFromXML(ATTRIBUTE_MEMBERS_IMAGE)
            End If
            If String.IsNullOrEmpty(ForumImage) Then
                ForumImage = Me.GetLinkValueFromXML(ATTRIBUTE_FORUM_IMAGE)
            End If
            If String.IsNullOrEmpty(DocumentsImage) Then
                DocumentsImage = Me.GetLinkValueFromXML(ATTRIBUTE_DOCUMENTS_IMAGE)
            End If

            'if values are not provide directly or from the XML file, set default values for inherited properties since 
            'control requires them to properly function
            If String.IsNullOrEmpty(Me.QueryStringRecordIDParameter) Then Me.QueryStringRecordIDParameter = "ID"

        End Sub

        Private Sub SetLeftNavImageLinks()
            generalSmall.Src = GeneralImageSmall
            memberSmall.Src = MembersImageSmall
            documentsSmall.Src = DocumentsImageSmall
            forumSmall.Src = ForumImageSmall
        End Sub

        Private Sub SetupViewType()
            Try
                m_sView = "General"

                ' Changes made for to allow encrypting and decrypting the URL.
                ' Changes made by Hrushikesh Jog
                Dim sView As String
                sView = Request.QueryString("View")
                sView = Aptify.Framework.Web.Common.WebCryptography.Decrypt(sView)

                If sView <> "" Then
                    Select Case CStr(sView).ToUpper.Trim
                        Case "MEMBERS"
                            m_sView = "Members"
                        Case "FORUM"
                            m_sView = "Forum"
                        Case "DOCUMENTS"
                            m_sView = "Documents"
                    End Select
                End If
            Catch ex As FormatException
                Response.Redirect(Me.GetSecurityErrorPageFromXML & "?Message=" & Server.UrlEncode("Committee Term not available"))
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Private Function IsCommitteeTermMember(ByVal PersonID As Long, _
                                               ByVal CommitteeTermID As Long) As Boolean
            Try
                Dim sSQL As String, lValue As Object
                sSQL = "SELECT COUNT(*) FROM " & Me.Database & "..vwCommitteeTermMembers " & _
                       " WHERE StartDate<=GETDATE() AND (EndDate='' OR EndDate>=GETDATE()) " & _
                       " AND CommitteeTermID=" & CommitteeTermID & " AND MemberID=" & PersonID
                lValue = Me.DataAction.ExecuteScalar(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)
                If Not lValue Is Nothing Then
                    Return CLng(lValue) > 0
                Else
                    Return False
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return False
            End Try
        End Function
        Private Sub SetupView(ByVal CommitteeTermID As Long)
            Try
                ' based on the view requested, render the approriate view
                lnkGeneral.Font.Bold = False
                lnkForum.Font.Bold = False
                lnkMembers.Font.Bold = False
                lnkDocuments.Font.Bold = False
                pnlGeneral.Visible = False
                Me.grdMembers.Visible = False
                Me.SingleForum1.Visible = False

                ' Changes made for to allow encrypting and decrypting the URL.
                ' Changes made by Hrushikesh Jog
                Dim eID As String 'encrypted value of ID
                Dim eView As String 'encrypted value of view

                If IsCommitteeTermMember(User1.PersonID, CommitteeTermID) Then
                    trForum.Visible = True
                    trDocuments.Visible = True
                Else
                    If m_sView = "Forum" Or m_sView = "Documents" Then
                        m_sView = "General"
                    End If
                    trForum.Visible = False
                    trDocuments.Visible = False
                End If

                LoadGeneral(CommitteeTermID) ' always load general first b/c other tabs may use this information at times.

                Select Case m_sView
                    Case "Forum"
                        Me.lblTitle.Text = "Discussion Forum for " & lblTerm.Text
                        imgTitle.Src = ForumImage
                        lnkForum.Font.Bold = True
                        Me.SingleForum1.Visible = True
                        LoadForum(CommitteeTermID)
                    Case "Members"
                        Me.lblTitle.Text = "Member List"
                        imgTitle.Src = MembersImage
                        lnkMembers.Font.Bold = True
                        grdMembers.Visible = True
                        LoadMembers(CommitteeTermID)
                    Case "Documents"
                        Me.lblTitle.Text = "Committee Document Library"
                        imgTitle.Src = DocumentsImage
                        lnkDocuments.Font.Bold = True
                        RecordAttachments.Visible = True
                        RecordAttachments.AllowAdd = True
                        RecordAttachments.LoadAttachments("Committee Terms", CommitteeTermID)
                    Case Else
                        Me.lblTitle.Text = "General Information"
                        imgTitle.Src = GeneralImage
                        lnkGeneral.Font.Bold = True
                        pnlGeneral.Visible = True
                End Select

                eID = System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(CommitteeTermID.ToString()))

                lnkGeneral.NavigateUrl = Me.Request.Url.AbsolutePath & "?ID=" & eID & "&View=" & System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt("General"))
                lnkForum.NavigateUrl = Me.Request.Url.AbsolutePath & "?ID=" & eID & "&View=" & System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt("Forum"))
                lnkMembers.NavigateUrl = Me.Request.Url.AbsolutePath & "?ID=" & eID & "&View=" & System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt("Members"))
                lnkDocuments.NavigateUrl = Me.Request.Url.AbsolutePath & "?ID=" & eID & "&View=" & System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt("Documents"))
            Catch ex As FormatException
                Response.Redirect(Me.GetSecurityErrorPageFromXML & "?Message=" & Server.UrlEncode("Committee Term not available"))
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)

            End Try
        End Sub

        Private Sub LoadGeneral(ByVal CommitteeTermID As Long)
            Dim sSQL As String, dt As DataTable
            sSQL = "SELECT CommitteeName, Name, StartDate,EndDate,Goals,Accomplishments,Director FROM " & Database & _
                           "..vwCommitteeTerms " & _
                            "WHERE ID=" & CommitteeTermID

            dt = DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)

            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                With dt.Rows(0)
                    lblCommittee.Text = CStr(.Item("CommitteeName"))
                    lblTerm.Text = CStr(.Item("Name"))
                    lblDirector.Text = CStr(.Item("Director"))
                    ''RashmiP, 11/15/11, Issue 6454, When Committee Term Has No Start And/Or End Date
                    If Not IsDate(.Item("StartDate")) OrElse CDate(.Item("StartDate")) = DateSerial(1900, 1, 1) Then
                        lblStartDate.Text = ""
                    Else
                        lblStartDate.Text = CDate(.Item("StartDate")).ToShortDateString
                    End If

                    If Not IsDate(.Item("EndDate")) OrElse CDate(.Item("EndDate")) = DateSerial(1900, 1, 1) Then
                        lblEndDate.Text = ""
                    Else
                        lblEndDate.Text = CDate(.Item("EndDate")).ToShortDateString
                    End If
                    'Added check for Null on fields that Allow Null
                    If Not IsDBNull(.Item("Goals")) Then
                        lblGoals.Text = CStr(.Item("Goals"))
                    End If
                    If Not IsDBNull(.Item("Accomplishments")) Then
                        lblAccomplishments.Text = CStr(.Item("Accomplishments"))
                    End If
                End With
            End If

        End Sub

        Private Sub LoadMembers(ByVal CommitteeTermID As Long)
            Dim sSQL As String, dt As DataTable
            Try
                If ViewState(ATTRIBUTE_DATATABLE_COMMITTEE_TERM_MEMBERS) IsNot Nothing Then
                    grdMembers.DataSource = ViewState(ATTRIBUTE_DATATABLE_COMMITTEE_TERM_MEMBERS)
                Else
                    sSQL = "Select ctm.Member, ctm.Title,ctm.StartDate, EndDate ,per.Email1 FROM " & _
                           Database & _
                           "..vwCommitteeTermMembers ctm Inner Join " & Database & "..vwpersons per ON ctm.MemberID=per.ID " & _
                            "WHERE ctm.CommitteeTermID=" & CommitteeTermID
                    dt = DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)

                    Try
                        If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                            grdMembers.DataSource = dt
                            ViewState(ATTRIBUTE_DATATABLE_COMMITTEE_TERM_MEMBERS) = dt
                            grdMembers.DataBind()
                        End If
                    Catch ex As Exception
                        Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                    End Try
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub LoadForum(ByVal CommitteeTermID As Long)
            Dim lForumID As Long
            lForumID = SingleForum1.GetLinkedForumID("Committee Terms", CommitteeTermID)
            If lForumID > 0 Then
                SingleForum1.LoadForum(lForumID)
            Else

                Dim dtStartDate As Date = DateSerial(1900, 1, 1)
                Dim dtEndDate As Date = dtStartDate

                If IsDate(lblStartDate.Text) Then
                    dtStartDate = CDate(lblStartDate.Text)
                End If

                If IsDate(lblEndDate.Text) Then
                    dtEndDate = DateAdd(DateInterval.Day, 15, CDate(lblEndDate.Text))
                End If

                If SingleForum1.CreateNewLinkedForum("Committee Term Forum - " & _
                                                     lblTerm.Text & " (" & _
                                                     CommitteeTermID & ")", _
                                                     "Forum for Committee Term Member Use", _
                                                     dtStartDate, _
                                                     dtEndDate, _
                                                     "Committee Terms", _
                                                     CommitteeTermID, _
                                                     lForumID) Then
                    SingleForum1.LoadForum(lForumID)
                Else
                    lblDetails.Text = "Error loading forum for committee term"
                End If
            End If
        End Sub
        Private Sub LoadCommitteeTerm()
            'Anil B for issue 15302 on 23/04/2013
            SetProperties()
            Try
                If (Me.SetControlRecordIDFromQueryString AndAlso _
                        Me.SetControlRecordIDFromParam()) _
                        OrElse Me.IsPageInAdmin() Then
                    SetupViewType()
                    SetupView(Me.ControlRecordID)
                Else
                    Response.Redirect(Me.GetSecurityErrorPageFromXML & "?Message=" & Server.UrlEncode("Committee Term not available"))
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub grdMembers_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdMembers.ItemDataBound
            Dim dateColumns As New List(Of String)
            'Add datecolumn uniqueName in list for Date format
            dateColumns.Add("GridDateTimeColumnStartDate")
            dateColumns.Add("GridDateTimeColumnEndDate")
            CommonMethods.FormatedDateOnGrid(dateColumns, e.Item)

        End Sub

        Protected Sub grdMembers_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdMembers.NeedDataSource
            LoadCommitteeTerm()
        End Sub

        Protected Sub grdMembers_PageIndexChanging(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageChangedEventArgs) Handles grdMembers.PageIndexChanged
            ''grdMembers.PageIndex = e.NewPageIndex
            LoadCommitteeTerm()
        End Sub
        Protected Sub grdMembers_PageIndexChanging(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageSizeChangedEventArgs) Handles grdMembers.PageSizeChanged
            ''grdMembers.PageIndex = e.NewPageIndex
            LoadCommitteeTerm()
        End Sub
    End Class
End Namespace