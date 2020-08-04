'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports System.Data
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Applications.OrderEntry
Imports System.Collections.Generic
Imports Aptify.Applications.Accounting
Imports Aptify.Applications.OrderEntry.Payments

Namespace Aptify.Framework.Web.eBusiness.MarketPlace
    Partial Class NewListing
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_VIEW_LISTING_PAGE As String = "ViewListingPage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "NewListing"
        'Navin Prasad Issue 12943
        Protected Const ATTRIBUTE_BILL_ME_LATER As String = "BillMeLaterDisplayText"

#Region "NewListing Specific Properties"
        ''' <summary>
        ''' ViewListing page url
        ''' </summary>
        Public Overridable Property ViewListingPage() As String
            Get
                If Not ViewState(ATTRIBUTE_VIEW_LISTING_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_VIEW_LISTING_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_VIEW_LISTING_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''RashmiP, issue 6781
        Public Overridable ReadOnly Property BillMeLaterDisplayText() As String
            Get
                If Not ViewState.Item(ATTRIBUTE_BILL_ME_LATER) Is Nothing Then
                    Return CStr(ViewState.Item(ATTRIBUTE_BILL_ME_LATER))
                Else
                    Dim value As String = Me.GetGlobalAttributeValue(ATTRIBUTE_BILL_ME_LATER)
                    If Not String.IsNullOrEmpty(value) Then
                        ViewState.Item(ATTRIBUTE_BILL_ME_LATER) = value
                    End If
                    Return value
                End If
            End Get
        End Property
#End Region

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(ViewListingPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                ViewListingPage = Me.GetLinkValueFromXML(ATTRIBUTE_VIEW_LISTING_PAGE)
                If String.IsNullOrEmpty(ViewListingPage) Then
                    Me.cmdSubmit.Enabled = False
                    Me.cmdSubmit.ToolTip = "ViewListingPage property has not been set."
                End If
            End If
          
        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed           
            SetProperties()
            'Anil B for issue 15374 on 02/04/2013
            'Set save for future use checkbox invisible
            CreditCard.SetchkSaveforFutureUse = False
            Try
                Response.Expires = -1
                If Not IsPostBack() Then
                    If User1.Company.Trim.Length = 0 _
                        AndAlso Not Me.IsPageInAdmin Then
                        Response.Redirect(Me.GetSecurityErrorPageFromXML & "?Message=You must be affiliated with a company before " & _
                                          "you can post or modify Marketplace listings. Please modify your " & _
                                          "profile through Customer Service, log-out, and log-in again.")
                    End If
                    Dim lblinterest As Label = CType(TopicCodeViewer.FindControl("lblinterest"), Label)
                    lblinterest.Text = "For search purposes, please select one or more topics that apply to your listing."
                    TopicCodeViewer.EntityName = "MarketPlace Listings"
                    TopicCodeViewer.RecordID = -1
                    TopicCodeViewer.ButtonDisplay = True
                    ListingEdit.SetNewListing()
                    Me.ShowBillMeLater() 'RashmiP, Issue 6781, 09/27/10
                    'Anil B change for 10737 on 13/03/2013
                    'Set Credit Card ID to load property form Navigation Config
                    CreditCard.LoadCreditCardInfo()
                    'Dim trvTopicCodes As Telerik.Web.UI.RadTreeView = CType(TopicCodeViewer.FindControl("trvTopicCodes"), Telerik.Web.UI.RadTreeView)
                    'If trvTopicCodes.IsEmpty Then
                    '    fieldsetListing.Visible = False
                    'Else
                    '    fieldsetListing.Visible = True
                    'End If
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Function CreateListingOrder() As Boolean
            'This function is responsible for creating a MarketPlace listing order 
            'based on the information provided by the user in the form.  RN 3/10/2003.
            Dim oGE As AptifyGenericEntityBase
            Try
                oGE = AptifyApplication.GetEntityObject("MarketPlace Listings", -1)

                With oGE
                    .SetValue("Name", ListingEdit.Name)
                    .SetValue("ListingTypeID", ListingEdit.ListingTypeID)
                    .SetValue("Status", "Requested")
                    .SetValue("StartDate", Now.ToString)
                    .SetValue("CategoryID", ListingEdit.CategoryID)
                    .SetValue("ContactID", User1.PersonID.ToString)
                    .SetValue("CompanyID", User1.CompanyID.ToString)
                    .SetValue("OfferingType", ListingEdit.OfferingType)

                    Dim oPrice As IProductPrice.PriceInfo = GetProductPrice(GetProductIDFromListingTypeID(ListingEdit.ListingTypeID))
                    .SetValue("Price", oPrice.Price)
                    .SetAddValue("CurrencyTypeID", oPrice.CurrencyTypeID)
                    .SetValue("RequestInfoEmail", ListingEdit.RequestInfoEmail)
                    .SetValue("VendorProductURL", ListingEdit.VendorProductURL)

                    .SetValue("PlainTextDescription", ListingEdit.Description)
                    .SetValue("HTMLDescription", ListingEdit.Description)

                    'RashmiP issue 6781, 09/20/10
                    'Anil B change for 10737 on 13/03/2013
                    'Set Credit Card ID to load property form Navigation Config
                    If CreditCard.BillMeLaterChecked Then
                        If String.IsNullOrEmpty(CreditCard.PONumber) Then
                            .SetValue("PONumber", BillMeLaterDisplayText)
                        Else
                            .SetValue("PONumber", CreditCard.PONumber)
                        End If
                        .SetValue("PaymentTypeID", CreditCard.PaymentTypeID)
                    Else
                        Page.Validate()
                        'Nalini Nanda - Issue 12796 - 01/24/2012
                        If ShoppingCart1.GetOrderObject(Session, Page.User, Application).Fields("PaymentInformationID").EmbeddedObjectExists Then
                            Dim oOrderPayInfo As PaymentInformation = DirectCast(ShoppingCart1.GetOrderObject(Session, Page.User, Application).Fields("PaymentInformationID").EmbeddedObject, PaymentInformation)
                            oOrderPayInfo = DirectCast(.Fields("PaymentInformationID").EmbeddedObject, PaymentInformation)
                            oOrderPayInfo.CreditCardSecurityNumber = CreditCard.CCSecurityNumber
                            'oOrderPayInfo.SetValue("CCPartial", oOrderPayInfo.GetCCPartial(CreditCard1.CCNumber))
                            .SetValue("PaymentTypeID", CreditCard.PaymentTypeID)
                            .SetValue("CCAccountNumber", CreditCard.CCNumber)
                            .SetValue("CCExpireDate", CreditCard.CCExpireDate)
                            .SetValue("CCPartial", oOrderPayInfo.GetCCPartial(Convert.ToString(CreditCard.CCNumber)))
                            '.SetValue("CCSecurityNumber", CreditCard.CCSecurityNumber) 'Added by sandeep for Issue 10675 on 11/04/2013
                            .SetAddValue("_xCCSecurityNumber", CreditCard.CCSecurityNumber) 'Neha changes for Issue 16675, 06/05/2013,Added CCSecurityNumber as a temperory field for not storing in record history.
                            'Anil B change for 10254 on 22/04/2013
                            'Add condition if payment type is transaction
                            If CreditCard.CCNumber = "-Ref Transaction-" Then
                                .SetValue("ReferenceTransactionNumber", CreditCard.ReferenceTransactionNumber)
                                .SetValue("ReferenceExpiration", CreditCard.ReferenceExpiration)
                            End If
                        End If

                    End If


                End With

                If oGE.Save(False) Then
                    TopicCodeViewer.RecordID = oGE.RecordID
                    TopicCodeViewer.SaveTopicCode()
                    'TopicCodeViewer.SaveChanges()
                    If (TopicCodeViewer.LastError = "") Then
                        Me.EncryptQueryStringValue = True
                        Me.RedirectURL = ViewListingPage
                        Me.RedirectIDParameterName = "ID"
                        Me.AppendRecordIDToRedirectURL = True
                        Me.RedirectUsingPropertyValues(oGE.RecordID)
                        Me.lblSubmitError.Visible = False
                    End If
                Else
                    If String.IsNullOrEmpty(oGE.LastError) Then
                        Me.lblSubmitError.Text = "Unable to submit MarketPlace Listing. Recheck entries and resubmit."
                    Else
                        Me.lblSubmitError.Text = oGE.LastError
                    End If
                    Me.lblSubmitError.Visible = True
                    TopicCodeViewer.LastError = ""
                    Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(New Exception("NEWLISTING SAVE ERROR: " & oGE.LastError))
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Function

        Private Sub cmdSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
            'Upon user clicking the submit button, create the order record.  RN 3/9/03.
            CreateListingOrder()
        End Sub

        Private Function GetProductPrice(ByVal ProductID As Long) As IProductPrice.PriceInfo
            GetProductPrice = ShoppingCart1.GetUserProductPrice(ProductID, 1)
        End Function

        Private Function GetProductIDFromListingTypeID(ByVal ListingTypeID As Long) As Long
            'This function will retrieve the Product ID associated with a listing type:
            Dim sSQL As String
            Dim dt As DataTable
            Try
                sSQL = "SELECT ProductID FROM " & Database & _
                       "..vwMarketPlaceListingTypes  " & _
                       "WHERE ID = " & ListingTypeID

                dt = DataAction.GetDataTable(sSQL)
                GetProductIDFromListingTypeID = CLng(dt.Rows(0).Item("ProductID"))
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Function
        ''' <summary>
        ''' RashmiP issue 6781, 09/27/10
        ''' Funtion set properties of credit card, if Company and User's credit Status is approved and credit limit is availabe 
        ''' contion check if payment type is Bill Me Later. 
        ''' </summary>
        Private Sub ShowBillMeLater()
            Dim iPOPaymentType As Integer
            Dim dr As Data.DataRow = User1.CompanyDataRow
            Try
                iPOPaymentType = CInt(AptifyApplication.GetEntityAttribute("Web Shopping Carts", "POPaymentTypeID"))
                'Anil B change for 10737 on 13/03/2013
                'Set Credit Card ID to load property form Navigation Config
                CreditCard.UserCreditStatus = CInt(User1.GetValue("CreditStatusID"))
                CreditCard.UserCreditLimit = CLng(User1.GetValue("CreditLimit"))
                If iPOPaymentType > 0 Then
                    If dr IsNot Nothing Then
                        CreditCard.CompanyCreditStatus = CInt(dr.Item("CreditStatusID"))
                        CreditCard.CompanyCreditLimit = CLng(dr.Item("CreditLimit"))
                    End If
                    CreditCard.CreditCheckLimit = CheckCreditLimit()
                End If
            Catch ex As Exception

            Finally

            End Try

        End Sub

        Private Function CheckCreditLimit() As Boolean
            Dim oOrderGE As OrdersEntity
            Dim lProductID As Long
            Dim oPrice As IProductPrice.PriceInfo
            Dim sError As String
            Dim bCreditLimit As Boolean
            Try
                lProductID = GetProductIDFromListingTypeID(ListingEdit.ListingTypeID)
                oPrice = GetProductPrice(lProductID)
                oOrderGE = CType(AptifyApplication.GetEntityObject("Orders", -1), OrdersEntity)
                With oOrderGE
                    .SetValue("BillToID", User1.PersonID)
                    .SetValue("BillToCompanyID", User1.CompanyID)
                    .SetValue("ShipToID", User1.PersonID)
                    .SetValue("ShipToCompanyID", User1.CompanyID)
                    .SetValue("OrderDate", Date.Now)

                    Dim orderLineGEs As List(Of OrderLinesEntity) = .AddProduct(lProductID, 1, False)

                    If orderLineGEs IsNot Nothing OrElse orderLineGEs.Count > 0 Then

                        With orderLineGEs(0)
                            .SetValue("Price", oPrice.Price)
                            .SetAddValue("CurrencyTypeID", oPrice.CurrencyTypeID)
                            .SetValue("UserPricingOverride", True)
                            .SetValue("Description", ListingEdit.Description)
                        End With

                        bCreditLimit = ShoppingCart1.CreditCheckObject.CheckCredit(oOrderGE, sError)
                    End If
                    Return bCreditLimit
                End With
            Catch ex As Exception

            End Try
        End Function
        ''RashmiP, Issue 6781. 02/02/11
        Protected Sub ListingEdit_ListingTypeChanged() Handles ListingEdit.ListingTypeChanged
            ShowBillMeLater()
        End Sub
    End Class
End Namespace
