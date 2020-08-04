'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports System.Data
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Applications.Accounting
Imports Aptify.Framework.Application
Imports Aptify.Framework.BusinessLogic
Imports Aptify.Applications.OrderEntry
Imports Aptify.Framework.DataServices
Imports Aptify.Applications.Fundraising
Imports System.Drawing



Namespace Aptify.Framework.Web.eBusiness.Fundraising
    Partial Class Fundraising
        Inherits BaseUserControlAdvanced
        'Suraj IssueID 15014
        Protected ATTRIBUTE_USEFUNDRASING As Integer = -1
        Protected ATTRIBUTE_IDEFAULTFUNDRAISINGCAMPAINGID As Integer = -1
        Protected ATTRIBUTE_SALERT As String = String.Empty
        Protected Const ATTRIBUTE_LOGIN_PAGE As String = "LoginPage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "Fundraising"
        'Navin Prasad Issue 12943
        Protected Const ATTRIBUTE_BILL_ME_LATER As String = "BillMeLaterDisplayText"
        'Amruta IssueID 15014
        Protected Const ATTRIBUTE_USE_FUND_RASINING As String = "UseFundRaising"
        Protected Const ATTRIBUTE_DEFAULT_FUND_RASINING_CAMPAIGN_ID As String = "DefaultFundraisingCampaignID"
        'Protected Const ATTRIBUTE_FUND_RASINING_CAMPAIGN_MESSAGE As String = "FundraisingCampaignMessage"
        Dim ErrorString As String = String.Empty



#Region "Fundraising Specific Properties"
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
        'Anil B for issue 16002 on 30-05-2013
        'Neha changes for Issue 16157, 06/17/2013, set property for fundraising and DefaultFundraisingCampaignID.
        Protected m_iUseFundraising As Integer
        Public Overridable Property UseFundRaising() As Integer
            Get
                Return m_iUseFundraising
            End Get
            Set(ByVal value As Integer)
                m_iUseFundraising = value
            End Set
        End Property
        Protected m_iDefaultFundraisingCampaignID As Integer
        Public Overridable Property DefaultFundraisingCampaignID() As Integer
            Get
                Return m_iDefaultFundraisingCampaignID
            End Get
            Set(ByVal value As Integer)
                m_iDefaultFundraisingCampaignID = value
            End Set
        End Property


        Public Overridable Property FundraisingCampaignMessage() As String
            Get
                If Not ViewState(ATTRIBUTE_SALERT) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_SALERT))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_SALERT) = Me.FixLinkForVirtualPath(value)
            End Set

        End Property
        'Amruta Issue 15014 end
#End Region

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(LoginPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                LoginPage = Me.GetLinkValueFromXML(ATTRIBUTE_LOGIN_PAGE)
            End If

            'Anil B for issue 16002 on 30-05-2013
            'Set Value from Navigation config
            If IsNumeric(DefaultFundraisingCampaignID) Then
                If IsNumeric(Me.GetPropertyValueFromXML(ATTRIBUTE_DEFAULT_FUND_RASINING_CAMPAIGN_ID)) Then
                    DefaultFundraisingCampaignID = CInt(Me.GetPropertyValueFromXML(ATTRIBUTE_DEFAULT_FUND_RASINING_CAMPAIGN_ID))
                End If
            End If
            If IsNumeric(UseFundRaising) Then
                If IsNumeric(Me.GetPropertyValueFromXML(ATTRIBUTE_USE_FUND_RASINING)) Then
                    UseFundRaising = CInt(Me.GetPropertyValueFromXML(ATTRIBUTE_USE_FUND_RASINING))
                End If
            End If

            'Amruta Issue 15014 start
            If ATTRIBUTE_USEFUNDRASING = -1 Then
                Dim sUseFundrasining As String = ""
                sUseFundrasining = Me.GetPropertyValueFromXML(ATTRIBUTE_USE_FUND_RASINING)
                If IsNumeric(sUseFundrasining) Then
                    ATTRIBUTE_USEFUNDRASING = CInt(sUseFundrasining)
                End If
            End If

            If ATTRIBUTE_IDEFAULTFUNDRAISINGCAMPAINGID = -1 Then
                Dim sDefaultFundraisingCampaignID As String = ""
                sDefaultFundraisingCampaignID = Me.GetPropertyValueFromXML(ATTRIBUTE_DEFAULT_FUND_RASINING_CAMPAIGN_ID)
                If IsNumeric(sDefaultFundraisingCampaignID) Then
                    ATTRIBUTE_IDEFAULTFUNDRAISINGCAMPAINGID = CInt(sDefaultFundraisingCampaignID)
                End If
            End If

            'If String.IsNullOrEmpty(FundraisingCampaignMessage) Then
            '    FundraisingCampaignMessage = Me.GetPropertyValueFromXML(ATTRIBUTE_FUND_RASINING_CAMPAIGN_MESSAGE)
            '    If Not String.IsNullOrEmpty(FundraisingCampaignMessage) Then
            '        lblMsg.Text = FundraisingCampaignMessage
            '    End If
            'End If
            'Amruta Issue 15014 end


        End Sub

        Public Function GetProductPrice(ByVal lProductID As Long) As IProductPrice.PriceInfo
            ' Implement This function
            Return ShoppingCart1.GetUserProductPrice(lProductID, 1)
        End Function

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
            'set control properties from XML file if needed
            SetProperties()
            'Anil B for issue 15374 on 02/04/2013
            'Set save for future use checkbox invisible
            'Suraj Issue 15014, 4/24/13, disable bill me later 
            DisableBillMeLater()
            CreditCard.SetchkSaveforFutureUse = False
            If Not IsPostBack Then
                If User1.UserID > 0 Then
                    ' There is a user logged in
                    LoadFunds()
                Else
                    ' No User is logged in
                    'Suraj Issue 15370,7/31/13 revert back changes application variale to session variable
                    Session("ReturnToPage") = Request.RawUrl
                    'Response.Redirect(Me.FixTemplateSourceDirectoryPath(Page.TemplateSourceDirectory) & "/Login.aspx")
                    ' Suraj S Issue 15370, 8/1/13 here we are getting the ReturnToPageURL in "URL" QueryString and passing on login page. 
                    Response.Redirect(LoginPage + "?ReturnURL=" & System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(Request.RawUrl)))
                End If
            End If
        End Sub
        Private Sub LoadFunds()
            Try
                ShowBillMeLater() ' RashmiP, Issue 6781, 09/27/10
                CreditCard.LoadCreditCardInfo()

                Dim sSQL As String
                Dim dt As DataTable

                'Anil B for issue 16002 on 30-05-2013
                'If UseFundRaising property value  is 1 then only populate product associated with the value of  DefaultFundraisingCampaignID 

                sSQL = "SELECT  Distinct p.ID, P.WebName,P.Name FROM " & Database & "..vwProducts P inner join " & Database & "..vwFundCampaignProducts FP on P.ID=FP.ProductID " & _
                       "WHERE WebEnabled=1 AND  RootCategory='Fundraising'"

                If UseFundRaising = 1 And DefaultFundraisingCampaignID > 0 Then
                    sSQL = sSQL & "  and FP.FundCampaignID= " & DefaultFundraisingCampaignID
                End If
                sSQL = sSQL & " ORDER BY P.Name,P.id"
                dt = DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)

                If dt.Rows.Count > 0 Then
                    cmbFunds.DataSource = dt
                    cmbFunds.DataBind()
                    tblInner.Visible = True
                    'Suraj S Issue 15014
                    SetPreffredCurrency()
                Else
                    lblMsg.Text = "There are no fundraising projects available."
                    lblMsg.Visible = True
                    tblInner.Visible = False
                    'Suraj S Issue 15014, 3/20/13 ,apply color for error message red 
                    lblMsg.ForeColor = Color.Red
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
            Dim oOrder As Aptify.Applications.OrderEntry.OrdersEntity



            Try
                'Suraj IssueID 15014, 3/20/13 We should have a user control level attribute in the Aptify_UC Nav config file that lets you continue to use
                ' the current method (if the organization doesn't have Fundraising installed) or create a pledge (if Fundraising is installed).
                'By default, we should set the new "UseFundraising" property to 0 which would be the existing functionality.
                'Then if an organization has a fundraising license, they can change the property to 1 and the control should create pledges instead. 
                'To identify a Fundraising Campaign for a pledge, we should add another property called something like DefaultFundraisingCampaignID where the organization 
                'can specify a fundraising campaign to use for web contributions
                If ATTRIBUTE_USEFUNDRASING.Equals(1) AndAlso ATTRIBUTE_IDEFAULTFUNDRAISINGCAMPAINGID > 0 Then
                    If CreatePledge() Then
                        lblMsg.Text = "Thank you for your generous contribution."
                        tblInner.Visible = False
                        lblMsg.Visible = True
                        lblMsg.ForeColor = Color.Black
                        'Amruta IssueID 15019
                        paymentarea.Visible = False
                    Else
                        lblMsg.Text = ErrorString
                        lblMsg.Visible = True
                        lblMsg.ForeColor = Color.Red
                    End If
                Else
                    ' validate the page
                    ' save the fundraising contribution
                    oOrder = DirectCast(ShoppingCart1.GetNewOrderObject(), Aptify.Applications.OrderEntry.OrdersEntity)
                    With oOrder
                        If CreditCard.BillMeLaterChecked Then
                            If String.IsNullOrEmpty(CreditCard.PONumber) Then
                                .SetValue("PONumber", BillMeLaterDisplayText)
                            Else
                                .SetValue("PONumber", CreditCard.PONumber)
                            End If
                            .SetValue("PayTypeID", CreditCard.PaymentTypeID)
                        Else
                            Page.Validate()
                            .SetValue("PayTypeID", CreditCard.PaymentTypeID)
                            .SetValue("CCAccountNumber", CreditCard.CCNumber)
                            .SetValue("CCExpireDate", CreditCard.CCExpireDate)



                            'Anil B change for 10254 on 22/04/2013
                            'Add condition if payment type is transaction
                            If CreditCard.CCNumber = "-Ref Transaction-" Then
                                .SetValue("ReferenceTransactionNumber", CreditCard.ReferenceTransactionNumber)
                                .SetValue("ReferenceExpiration", CreditCard.ReferenceExpiration)
                            End If
                            '#16606 code changes by Neha,_xCCSecurityNumber as a tem variable, so that it will not stored in orders record history
                            If Len(CreditCard.CCSecurityNumber) > 0 Then
                                .SetAddValue("_xCCSecurityNumber", CreditCard.CCSecurityNumber)
                            End If
                            If oOrder.Fields("PaymentInformationID").EmbeddedObjectExists Then
                                Dim oOrderPayInfo As PaymentInformation = DirectCast(oOrder.Fields("PaymentInformationID").EmbeddedObject, PaymentInformation)
                                oOrderPayInfo.CreditCardSecurityNumber = CreditCard.CCSecurityNumber
                                oOrderPayInfo.SetValue("CCPartial", oOrderPayInfo.GetCCPartial(CreditCard.CCNumber))
                            End If

                        End If
                    End With
                    With oOrder.SubTypes("OrderLines").Add()
                        .SetValue("ProductID", cmbFunds.SelectedItem.Value)
                        .SetValue("Price", txtAmount.Text)
                        .SetValue("UserPricingOverride", True)
                        .SetValue("PriceName", "Contribution")
                        .SetValue("Quantity", "1")
                    End With
                    If oOrder.Save(False) Then
                        Me.AutoShipWebOrder(oOrder)
                        'Amruta IssueID 15014 start
                        lblMsg.Text = "Thank you for your generous contribution."
                        tblInner.Visible = False
                        lblMsg.Visible = True
                        lblMsg.ForeColor = Color.Black
                        'Amruta IssueID 15019
                        paymentarea.Visible = False
                    Else
                        lblMsg.Text = oOrder.LastError
                        lblMsg.Visible = True
                        lblMsg.ForeColor = Color.Red
                    End If
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        'Amruta Issue 15014
        'Suraj S Issue 15014, 3/20/13, here we create object of Payment Information" and set the payment related values.
        'Private Function CreatePledge(ByVal OrderID As Integer, ByVal Amount As Integer, ByVal ProductID As Integer) As Boolean
        Private Function CreatePledge() As Boolean
            Dim CurrencyID As Long
            Dim ObjBase As AptifyGenericEntityBase = AptifyApplication.GetEntityObject("Pledges", -1)
            Dim m_oPayment As AptifyGenericEntityBase
            m_oPayment = AptifyApplication.GetEntityObject("Payment Information", -1)
            If CreditCard.BillMeLaterChecked Then
                m_oPayment.SetValue("PaymentTypeID", CreditCard.PaymentTypeID)
                m_oPayment.SetValue("PONumber", CreditCard.PONumber)
            Else
                m_oPayment.SetValue("PaymentTypeID", CreditCard.PaymentTypeID)
                m_oPayment.SetValue("CCAccountNumber", CreditCard.CCNumber)
                m_oPayment.SetValue("CCExpireDate", CreditCard.CCExpireDate)
                'Neha changes for issue 16157, set creditcard SecurityNumber for fundraising control
                m_oPayment.SetAddValue("_xCCSecurityNumber", CreditCard.CCSecurityNumber) '#16606 code changes by Neha for pledge orders
            End If

            Dim objContribution As ContributionPaymentObject = New ContributionPaymentObject
            objContribution.Config(AptifyApplication)
            objContribution.PaymentInformation = CType(m_oPayment, AptifyGenericEntity)
            Dim objPledgeHeader As ContributionPaymentObject.stPledgeHeader = New ContributionPaymentObject.stPledgeHeader
            If ATTRIBUTE_IDEFAULTFUNDRAISINGCAMPAINGID > 0 Then
                'Sachin IssueID 15014 which is set the currency 
                CurrencyID = SetPreffredCurrency()
            End If
            objPledgeHeader.lFundID = CLng(cmbFunds.SelectedValue)
            objPledgeHeader.IsCorporate = False
            objPledgeHeader.IsShowInDonorList = True
            objPledgeHeader.lContributorID = User1.PersonID
            objPledgeHeader.lCampaignID = ATTRIBUTE_IDEFAULTFUNDRAISINGCAMPAINGID
            objPledgeHeader.iCurrencyTypeID = CInt(CurrencyID)
            objPledgeHeader.dDateReceived = CDate(DateAndTime.Now.ToShortDateString)
            objPledgeHeader.dAmount = CDec(txtAmount.Text)
            objPledgeHeader.lEmployeeID = EBusinessGlobal.WebEmployeeID(Page.Application)
            objContribution.PledgeHeader = objPledgeHeader
            'Suraj IssueID 15014, 3/20/13 we called DoApplyPayment function which is create a pledge record and create a order againgst the pledge
            Return objContribution.DoApplyPayment(True, ErrorString)



        End Function

        Protected Overridable Sub AutoShipWebOrder(ByVal OrderGE As Aptify.Applications.OrderEntry.OrdersEntity)
            Dim autoShip As AutoShipmentSetting = Me.AutoShipSetting

            If autoShip = AutoShipmentSetting.Enabled Then
                If OrderGE.AvailableForShipping(True) = Aptify.Applications.OrderEntry.OrdersEntity.AutoShippingAvailabilityTypes.FullOrder Then
                    'Order qualifies for Autoshipping, does the user need to be prompted?
                    If Not OrderGE.ShipEntireOrder(False) Then
                        'essentially, autoshipping failures are logged but don't prevent the save of the order
                        Dim sMessage As String = "Autoshipment of Order " & OrderGE.RecordID & " failed.  The Order has been saved but not shipped.  The Order can be manually shipped at some point in the future."
                        Dim sMoreMessage As String = OrderGE.LastError

                        If sMoreMessage IsNot Nothing AndAlso sMoreMessage.Length > 0 Then
                            sMessage = sMessage & Environment.NewLine & "Error:  " & sMoreMessage
                        End If

                        ExceptionManagement.ExceptionManager.Publish(New Aptify.Applications.OrderEntry.OrderException(sMessage))
                    End If
                End If
            End If
        End Sub

        Protected Enum AutoShipmentSetting
            Unspecified = -1
            Disabled = 0
            Enabled = 1
        End Enum

        Private m_AutoShipSetting As AutoShipmentSetting = AutoShipSetting.Unspecified

        Protected Overridable Property AutoShipSetting() As AutoShipmentSetting
            Get
                If m_AutoShipSetting = AutoShipmentSetting.Unspecified Then
                    'Attempt to retrieve it from the Web Shopping Cart's Entity Attributes.
                    Dim sSetting As String = Me.AptifyApplication.GetEntityAttribute("Web Shopping Carts", "AutoShipNonFulfillmentProducts")
                    If sSetting Is Nothing OrElse sSetting.Length = 0 Then
                        m_AutoShipSetting = AutoShipmentSetting.Unspecified
                    ElseIf IsNumeric(sSetting) AndAlso CInt(sSetting) = 1 Then
                        m_AutoShipSetting = AutoShipmentSetting.Enabled
                    Else
                        m_AutoShipSetting = AutoShipmentSetting.Disabled
                    End If
                End If
                Return m_AutoShipSetting
            End Get
            Set(ByVal value As AutoShipmentSetting)
                m_AutoShipSetting = value
            End Set
        End Property

        ''' <summary>
        ''' RashmiP issue 6781, 09/27/10
        ''' Funtion set properties of credit card, if Company and User's credit Status is approved and credit limit is availabe 
        ''' contion check if payment type is Bill Me Later. 
        ''' </summary>
        Private Sub ShowBillMeLater()
            Dim iPOPaymentType As Integer
            Dim iPrevPaymentTypeID As Integer
            Dim sError As String
            Dim oOrder As Aptify.Applications.OrderEntry.OrdersEntity
            Try
                iPOPaymentType = CInt(AptifyApplication.GetEntityAttribute("Web Shopping Carts", "POPaymentTypeID"))
                Dim dr As Data.DataRow = User1.CompanyDataRow
                CreditCard.UserCreditStatus = CInt(User1.GetValue("CreditStatusID"))
                CreditCard.UserCreditLimit = CLng(User1.GetValue("CreditLimit"))
                If iPOPaymentType > 0 Then
                    If dr IsNot Nothing Then
                        CreditCard.CompanyCreditStatus = CInt(dr.Item("CreditStatusID"))
                        CreditCard.CompanyCreditLimit = CLng(dr.Item("CreditLimit"))
                    End If
                    oOrder = ShoppingCart1.GetOrderObject(Session, Page.User, Application)
                    If oOrder IsNot Nothing Then
                        iPrevPaymentTypeID = CInt(oOrder.GetValue("PayTypeID"))
                        oOrder.SetValue("PayTypeID", iPOPaymentType)
                        CreditCard.CreditCheckLimit = ShoppingCart1.CreditCheckObject.CheckCredit(CType(oOrder, Aptify.Applications.OrderEntry.OrdersEntity), sError)
                    End If
                End If
            Catch ex As Exception

            Finally
                oOrder.SetValue("PayTypeID", iPrevPaymentTypeID)
            End Try

        End Sub


        'sachin IssueID 15014, 02/14/2013. Get the person respected currency 
        ' Suraj S Issue 15014, 3/20/13 Change the condition because privious condion not handaling the DB null values so i change the condition
        Private Function SetPreffredCurrency() As Integer
            Dim sSQL As String = String.Empty
            Dim CurrencyID As Integer = -1
            Try
                'following statment check if the web user having  prefered currency or not if  having it will returns the currency id if not it will return the DB null
                sSQL = "Execute " & Me.AptifyApplication.GetEntityBaseDatabase("vwPersons") & ".." & "spGetPersonPreferredCurrency " & User1.PersonID
                'Suraj S Issue 15014, 3/20/13 Change the condition because privious condion not handaling the DB null values so i change the condition 
                'following statment check if the web user dosent have  prefered currency then it will check the company link prefered currency if  having it will returns the currency id if not it will return the DB null 
                If IsDBNull(DataAction.ExecuteScalar(sSQL)) Then
                    ' Sachin kalyankar issue No 15014 
                    sSQL = "select com.PreferredCurrencyTypeID from " & Database & ".." & AptifyApplication.GetEntityBaseView("Companies") & " com " & _
                             "Inner join " & Database & ".." & AptifyApplication.GetEntityBaseView("Persons") & " per ON com.ID = (select CompanyID from " & Database & ".." & AptifyApplication.GetEntityBaseView("Persons") & " where ID=" & User1.PersonID & ") " & _
                             "and per.ID =" & User1.PersonID
                    'following statment check if the web user dosent have  prefered currency  and company link also dosent any  prefered currency then it will the functional currency 
                    'for the organization associated with the e-Business user’s Employees record (typically this is US Dollar but may be always) also if the person is not link 
                    'company so for that we add "AndAlso CInt(DataAction.ExecuteScalar(sSQL)) <> 0 " this condition.
                    If (Not IsDBNull(DataAction.ExecuteScalar(sSQL))) AndAlso CInt(DataAction.ExecuteScalar(sSQL)) <> 0 Then
                        CurrencyID = CInt(DataAction.ExecuteScalar(sSQL))
                        sSQL = "select dbo.fnGetCurrencySymbol(" & CurrencyID & ")"
                        lblCurrencySymbol.Text = CStr(DataAction.ExecuteScalar(sSQL))
                        Return CurrencyID
                    Else
                        Dim EmployeeId As Integer
                        EmployeeId = CInt(EBusinessGlobal.WebEmployeeID(Page.Application))
                        sSQL = "select org.FunctionalCurrencyTypeID from " & Database & ".." & AptifyApplication.GetEntityBaseView("Organizations") & " org Inner join " & Database & ".." & AptifyApplication.GetEntityBaseView("Employees") & " Emp ON Emp.OrganizationID = org.ID " & _
                            "where Emp.ID=" & EmployeeId & " "
                        If Not IsDBNull(DataAction.ExecuteScalar(sSQL)) Then
                            CurrencyID = CInt(DataAction.ExecuteScalar(sSQL))
                            sSQL = "select dbo.fnGetCurrencySymbol(" & CurrencyID & ")"
                            lblCurrencySymbol.Text = CStr(DataAction.ExecuteScalar(sSQL))
                            Return CurrencyID
                        End If
                    End If
                Else
                    CurrencyID = CInt(DataAction.ExecuteScalar(sSQL))
                    sSQL = "select dbo.fnGetCurrencySymbol(" & CurrencyID & ")"
                    lblCurrencySymbol.Text = CStr(DataAction.ExecuteScalar(sSQL))
                    Return CurrencyID
                End If
            Catch ex As Exception
                Return CurrencyID
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Function
        'Suraj Issue 15014, 4/24/13, disable bill me later here we set the "DisableBillMeLater" property True .this property declare on credit card control
        Public Sub DisableBillMeLater()
            CreditCard.DisableBillMeLater = True
        End Sub
    End Class
End Namespace
