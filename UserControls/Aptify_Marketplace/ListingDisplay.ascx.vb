'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports System.Data

Namespace Aptify.Framework.Web.eBusiness.MarketPlace
    Partial Class ListingDisplay
        Inherits BaseUserControlAdvanced


        Protected Enum URLType
            HTTP
            MailTo
        End Enum

        Private m_lListingID As Long

        Protected Const ATTRIBUTE_EDIT_LISTING_PAGE As String = "EditListingPage"
        Protected Const ATTRIBUTE_REQUEST_INFO_PAGE As String = "RequestInfoPage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "ListingDisplay"

#Region "ListingDisplay Specific Properties"
        ''' <summary>
        ''' ChapterSearch page url
        ''' </summary>
        Public Overridable Property EditListingPage() As String
            Get
                If Not ViewState(ATTRIBUTE_EDIT_LISTING_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_EDIT_LISTING_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_EDIT_LISTING_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' MyChapters page url
        ''' </summary>
        Public Overridable Property RequestInfoPage() As String
            Get
                If Not ViewState(ATTRIBUTE_REQUEST_INFO_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_REQUEST_INFO_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_REQUEST_INFO_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
#End Region

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(EditListingPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                EditListingPage = Me.GetLinkValueFromXML(ATTRIBUTE_EDIT_LISTING_PAGE)
                If String.IsNullOrEmpty(EditListingPage) Then
                    Me.btnEdit.Enabled = False
                    Me.btnEdit.ToolTip = "EditListingPage property has not been set."
                End If
            End If
            If String.IsNullOrEmpty(RequestInfoPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                RequestInfoPage = Me.GetLinkValueFromXML(ATTRIBUTE_REQUEST_INFO_PAGE)
                If String.IsNullOrEmpty(RequestInfoPage) Then
                    Me.btnRequest.Enabled = False
                    Me.btnRequest.ToolTip = "RequestInfoPage property has not been set."
                End If
            End If
        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
        End Sub

        ''' <summary>
        ''' Displays a specific listing
        ''' </summary>
        ''' <param name="ListingID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function DisplayListing(ByVal ListingID As Long) As Boolean
            Dim sSQL As String
            Dim dt As DataTable

            Try
                Me.ControlRecordID = ListingID

                sSQL = Database & "..spGetMarketPlaceListing @ID=" & ListingID
                dt = DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)

                If dt.Rows.Count > 0 Then
                    If Not IsDBNull(dt.Rows(0).Item("Company")) Then
                        lblCompanyName.Text = CStr(dt.Rows(0).Item("Company"))
                    End If
                    If Not IsDBNull(dt.Rows(0).Item("Contact")) Then
                        lblCompanyContact.Text = CStr(dt.Rows(0).Item("Contact"))
                    End If
                    If Not IsDBNull(dt.Rows(0).Item("Name")) Then
                        lblName.Text = CStr(dt.Rows(0).Item("Name"))
                    End If
                    If Not IsDBNull(dt.Rows(0).Item("ListingType")) Then
                        lblListingType.Text = CStr(dt.Rows(0).Item("ListingType"))
                    End If
                    If Not IsDBNull(dt.Rows(0).Item("Category")) Then
                        lblCategory.Text = CStr(dt.Rows(0).Item("Category"))
                    End If
                    If Not IsDBNull(dt.Rows(0).Item("OfferingType")) Then
                        lblOfferingType.Text = CStr(dt.Rows(0).Item("OfferingType"))
                    End If
                    If Not IsDBNull(dt.Rows(0).Item("PlainTextDescription")) Then
                        lblDescription.Text = CStr(dt.Rows(0).Item("PlainTextDescription"))
                    End If
                    If Not IsDBNull(dt.Rows(0).Item("VendorProductURL")) Then
                        lnkCompanyURL.NavigateUrl = ProperURL(CStr(dt.Rows(0).Item("VendorProductURL")), URLType.HTTP)
                    End If
                    If Not IsDBNull(dt.Rows(0).Item("VendorProductURL")) Then
                        lnkCompanyURL.Text = ProperURL(CStr(dt.Rows(0).Item("VendorProductURL")), URLType.HTTP)
                    End If
                    If Not IsDBNull(dt.Rows(0).Item("RequestInfoEmail")) Then
                        lnkEmail.NavigateUrl = ProperURL(CStr(dt.Rows(0).Item("RequestInfoEmail")), URLType.MailTo)
                    End If
                    If Not IsDBNull(dt.Rows(0).Item("RequestInfoEmail")) Then
                        lnkEmail.Text = CStr(dt.Rows(0).Item("RequestInfoEmail"))
                    End If
                    'If Not IsDBNull(dt.Rows(0).Item("HTMLDescription")) Then
                    '    lblHTML.Text = CStr(dt.Rows(0).Item("HTMLDescription"))
                    'End If
                    If Not IsDBNull(dt.Rows(0).Item("ContactID")) _
                            AndAlso CLng(dt.Rows(0).Item("ContactID")) = User1.PersonID Then
                        btnEdit.Visible = True
                        btnRequest.Visible = False
                    Else
                        btnEdit.Visible = False
                        btnRequest.Visible = True
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

        Private Function ProperURL(ByVal url As String, ByVal type As URLType) As String
            Dim sPrefix As String = ""
            Select Case type
                Case URLType.HTTP : sPrefix = "http://"
                Case URLType.MailTo : sPrefix = "mailto:"
            End Select

            If Not url.StartsWith(sPrefix) Then
                Return sPrefix & url
            Else
                Return url
            End If
        End Function

        Private Sub btnEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEdit.Click
            Me.EncryptQueryStringValue = True
            Me.RedirectURL = EditListingPage
            Me.RedirectIDParameterName = "ID"
            Me.AppendRecordIDToRedirectURL = True
            Me.RedirectUsingPropertyValues(Me.ControlRecordID)
        End Sub

        Protected Sub btnRequest_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRequest.Click
            Me.EncryptQueryStringValue = True
            Me.RedirectURL = RequestInfoPage
            Me.RedirectIDParameterName = "ID"
            Me.AppendRecordIDToRedirectURL = True
            Me.RedirectUsingPropertyValues(Me.ControlRecordID)
        End Sub
    End Class
End Namespace
