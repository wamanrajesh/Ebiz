'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports System.Data
Imports Aptify.Framework.BusinessLogic.GenericEntity

Namespace Aptify.Framework.Web.eBusiness.MarketPlace
    Partial Class EditListing
        Inherits BaseUserControlAdvanced

        Private m_lListingID As Long

        Protected Const ATTRIBUTE_TOPIC_CODES_PAGE As String = "TopicCodesPage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "EditListing"

#Region "EditListing Specific Properties"
        ''' <summary>
        ''' TopicCodes page url
        ''' </summary>
        Public Overridable Property TopicCodesPage() As String
            Get
                If Not ViewState(ATTRIBUTE_TOPIC_CODES_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_TOPIC_CODES_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_TOPIC_CODES_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
#End Region

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(TopicCodesPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                TopicCodesPage = Me.GetLinkValueFromXML(ATTRIBUTE_TOPIC_CODES_PAGE)
                If String.IsNullOrEmpty(TopicCodesPage) Then
                    Me.lnkTopicCodes.Enabled = False
                    Me.lnkTopicCodes.ToolTip = "TopicCodesPage property has not been set."
                Else
                    Me.lnkTopicCodes.NavigateUrl = TopicCodesPage
                End If
            Else
                Me.lnkTopicCodes.NavigateUrl = TopicCodesPage
            End If
            If String.IsNullOrEmpty(Me.RedirectURL) Then
                Me.cmdSubmit.Enabled = False
                Me.cmdSubmit.ToolTip = "RedirectURL property has not been set."
            End If

            'if values are not provide directly or from the XML file, set default values for inherited properties since 
            'control requires them to properly function
            If String.IsNullOrEmpty(Me.QueryStringRecordIDParameter) Then Me.QueryStringRecordIDParameter = "ID"
            If String.IsNullOrEmpty(Me.RedirectIDParameterName) Then Me.RedirectIDParameterName = "ID"

        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
            Try
                Response.Expires = -1
                If Not IsPostBack() Then
                    If User1.Company.Trim.Length = 0 Then
                        Response.Redirect(Me.GetSecurityErrorPageFromXML & "?Message=You must be affiliated with a company before " & _
                                          "you can post or modify Marketplace listings. Please modify your " & _
                                          "profile through Customer Service, log-out, and log-in again.")
                    End If

                    If Me.SetControlRecordIDFromQueryString AndAlso _
                            Me.SetControlRecordIDFromParam() AndAlso _
                            Me.ControlRecordID <= 0 AndAlso _
                            Not Me.IsPageInAdmin() Then
                        'If Not IsNumeric(Request.QueryString("ID")) Then
                        ' remove hard-coded virtual directory RFB 7/25/03
                        'Response.Redirect(Me.GetSecurityErrorPageFromXML & "?Message=Access to this listing is unauthorized.")
                    End If

                    m_lListingID = Me.ControlRecordID
                    If Not ValidateOwnership(m_lListingID) Then
                        ' remove hard-coded virtual directory RFB 7/25/03
                        'Response.Redirect(Me.GetSecurityErrorPageFromXML & "?Message=You do not have permissions to edit the MarketPlace Listing.")
                    End If

                    ConfigureControls(m_lListingID)
                    Dim lblinterest As Label = CType(TopicCodeViewer.FindControl("lblinterest"), Label)
                    lblinterest.Text = "For search purposes, please select one or more topics that apply to your listing."
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Function ConfigureControls(ByVal lListingID As Long) As Boolean
            Try
                ListingEdit.LoadListing(lListingID)

                With TopicCodeViewer
                    .EntityName = "MarketPlace Listings"
                    .RecordID = lListingID
                    .ButtonDisplay = True
                End With
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Function

        Private Sub cmdSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
            'If Page.IsValid Then
            EditListingOrder()
            'End If
        End Sub

        Private Function EditListingOrder() As Boolean
            Dim oGE As AptifyGenericEntityBase

            Try
                m_lListingID = Me.ControlRecordID

                oGE = AptifyApplication.GetEntityObject("MarketPlace Listings", m_lListingID)

                With oGE
                    .SetValue("Name", ListingEdit.Name)
                    .SetValue("ListingTypeID", ListingEdit.ListingTypeID)
                    .SetValue("CategoryID", ListingEdit.CategoryID)
                    .SetValue("OfferingType", ListingEdit.OfferingType)
                    .SetValue("RequestInfoEmail", ListingEdit.RequestInfoEmail)
                    .SetValue("VendorProductURL", ListingEdit.VendorProductURL)
                    .SetValue("PlainTextDescription", ListingEdit.Description)
                    .SetValue("HTMLDescription", ListingEdit.Description)
                End With

                If oGE.Save(False) Then
                    TopicCodeViewer.EntityName = "MarketPlace Listings"
                    TopicCodeViewer.RecordID = oGE.RecordID
                    TopicCodeViewer.SaveTopicCode()
                    'TopicCodeViewer.SaveChanges()
                    'Response.Redirect("ViewListing.aspx?ID=" & oGE.RecordID)
                    'Me.EncryptQueryStringValue = True
                    'Me.RedirectURL = "ViewListing.aspx"
                    'Me.RedirectIDParameterName = "ID"
                    'Me.AppendRecordIDToRedirectURL = True
                    If (TopicCodeViewer.LastError = "") Then
                    Me.RedirectUsingPropertyValues(oGE.RecordID)
                    End If
                    TopicCodeViewer.LastError = ""
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Function

        Private Function ValidateOwnership(ByVal lMPListingID As Long) As Boolean
            'This function validates whether the user has the right to modify
            'the listing.  RN 3/12/2003

            Dim sSQL As String
            Dim dt As DataTable

            Try
                sSQL = "SELECT ContactID FROM " & Database & _
                       "..vwMarketPlaceListings WHERE ID = " & lMPListingID

                dt = DataAction.GetDataTable(sSQL)

                If Not dt.Rows.Count = 0 Then
                    If CLng(dt.Rows(0).Item("ContactID")) = User1.PersonID Then
                        ValidateOwnership = True
                    Else
                        ValidateOwnership = False
                    End If
                Else
                    ValidateOwnership = False
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Function
    End Class
End Namespace
