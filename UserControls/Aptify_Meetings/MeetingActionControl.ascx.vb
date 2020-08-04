'Aptify e-Business 5.5.1, July 2013
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Applications.ProductSetup
Imports System.Data
Imports Aptify.Framework

Partial Class Files_MeetingActionControl
    Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced

    Protected Const ATTRIBUTE_GENERAL_IMAGE_URL As String = "GeneralImage"
    Protected Const ATTRIBUTE_SCHEDULE_IMAGE_URL As String = "ScheduleImage"
    Protected Const ATTRIBUTE_SPEAKERS_IMAGE_URL As String = "SpeakersImage"
    Protected Const ATTRIBUTE_TRAVEL_IMAGE_URL As String = "TravelImage"
    Protected Const ATTRIBUTE_FORUM_IMAGE_URL As String = "ForumImage"
    Protected Const ATTRIBUTE_REGISTER_IMAGE_URL As String = "RegisterImage"
    Protected Const ATTRIBUTE_PRODUCT_DISPLAY_PAGE As String = "ProductDisplayPage"
    Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "MeetingActionControl"
    'SKB 12/1/2010 Issue 7294: People You May know functionality
    Protected Const ATTRIBUTE_PEOPLEYOUMAYKNOW_IMAGE_URL As String = "PeopleYouMayKnowImage"

#Region "MeetingActionControl specific properties"
    ''' <summary>
    ''' GeneralImage url
    ''' </summary>
    Public Overridable Property GeneralImage() As String
        Get
            If Not ViewState(ATTRIBUTE_GENERAL_IMAGE_URL) Is Nothing Then
                Return CStr(ViewState(ATTRIBUTE_GENERAL_IMAGE_URL))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            ViewState(ATTRIBUTE_GENERAL_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
        End Set
    End Property
    ''' <summary>
    ''' ScheduleImage page url
    ''' </summary>
    Public Overridable Property ScheduleImage() As String
        Get
            If Not ViewState(ATTRIBUTE_SCHEDULE_IMAGE_URL) Is Nothing Then
                Return CStr(ViewState(ATTRIBUTE_SCHEDULE_IMAGE_URL))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            ViewState(ATTRIBUTE_SCHEDULE_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
        End Set
    End Property
    ''' <summary>
    ''' SpeakersImage url
    ''' </summary>
    Public Overridable Property SpeakersImage() As String
        Get
            If Not ViewState(ATTRIBUTE_SPEAKERS_IMAGE_URL) Is Nothing Then
                Return CStr(ViewState(ATTRIBUTE_SPEAKERS_IMAGE_URL))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            ViewState(ATTRIBUTE_SPEAKERS_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
        End Set
    End Property
    ''' <summary>
    ''' TravelImage url
    ''' </summary>
    Public Overridable Property TravelImage() As String
        Get
            If Not ViewState(ATTRIBUTE_TRAVEL_IMAGE_URL) Is Nothing Then
                Return CStr(ViewState(ATTRIBUTE_TRAVEL_IMAGE_URL))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            ViewState(ATTRIBUTE_TRAVEL_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
        End Set
    End Property

    ''' <summary>
    ''' ForumImage url
    ''' </summary>
    Public Overridable Property ForumImage() As String
        Get
            If Not ViewState(ATTRIBUTE_FORUM_IMAGE_URL) Is Nothing Then
                Return CStr(ViewState(ATTRIBUTE_FORUM_IMAGE_URL))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            ViewState(ATTRIBUTE_FORUM_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
        End Set
    End Property
    ''' <summary>
    ''' PeopleYouMayKnowImage url
    ''' </summary>
    Public Overridable Property PeopleYouMayKnowImage() As String
        Get
            If Not ViewState(ATTRIBUTE_PEOPLEYOUMAYKNOW_IMAGE_URL) Is Nothing Then
                Return CStr(ViewState(ATTRIBUTE_PEOPLEYOUMAYKNOW_IMAGE_URL))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            ViewState(ATTRIBUTE_PEOPLEYOUMAYKNOW_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
        End Set
    End Property
    ''' <summary>
    ''' RegisterImage url
    ''' </summary>
    Public Overridable Property RegisterImage() As String
        Get
            If Not ViewState(ATTRIBUTE_REGISTER_IMAGE_URL) Is Nothing Then
                Return CStr(ViewState(ATTRIBUTE_REGISTER_IMAGE_URL))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            ViewState(ATTRIBUTE_REGISTER_IMAGE_URL) = Me.FixLinkForVirtualPath(value)
        End Set
    End Property
    ''' <summary>
    ''' ProductDisplay page url
    ''' </summary>
    Public Overridable Property ProductDisplayPage() As String
        Get
            If Not ViewState(ATTRIBUTE_PRODUCT_DISPLAY_PAGE) Is Nothing Then
                Return CStr(ViewState(ATTRIBUTE_PRODUCT_DISPLAY_PAGE))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            ViewState(ATTRIBUTE_PRODUCT_DISPLAY_PAGE) = Me.FixLinkForVirtualPath(value)
        End Set
    End Property
#End Region
    Protected Overrides Sub SetProperties()

        If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
        'call base method to set parent properties
        MyBase.SetProperties()

        If String.IsNullOrEmpty(GeneralImage) Then
            'since value is the 'default' check the XML file for possible custom setting
            GeneralImage = Me.GetLinkValueFromXML(ATTRIBUTE_GENERAL_IMAGE_URL)
            imgGeneral.Src = GeneralImage
        End If
        If String.IsNullOrEmpty(ScheduleImage) Then
            'since value is the 'default' check the XML file for possible custom setting
            ScheduleImage = Me.GetLinkValueFromXML(ATTRIBUTE_SCHEDULE_IMAGE_URL)
            imgSchedule.Src = ScheduleImage
        End If
        If String.IsNullOrEmpty(SpeakersImage) Then
            'since value is the 'default' check the XML file for possible custom setting
            SpeakersImage = Me.GetLinkValueFromXML(ATTRIBUTE_SPEAKERS_IMAGE_URL)
            imgSpeakers.Src = SpeakersImage
        End If
        If String.IsNullOrEmpty(TravelImage) Then
            'since value is the 'default' check the XML file for possible custom setting
            TravelImage = Me.GetLinkValueFromXML(ATTRIBUTE_TRAVEL_IMAGE_URL)
            imgTravel.Src = TravelImage
        End If
        If String.IsNullOrEmpty(ForumImage) Then
            'since value is the 'default' check the XML file for possible custom setting
            ForumImage = Me.GetLinkValueFromXML(ATTRIBUTE_FORUM_IMAGE_URL)
            imgForum.Src = ForumImage
        End If
        If String.IsNullOrEmpty(RegisterImage) Then
            'since value is the 'default' check the XML file for possible custom setting
            RegisterImage = Me.GetLinkValueFromXML(ATTRIBUTE_REGISTER_IMAGE_URL)
            imgRegister.ImageUrl = RegisterImage
        End If
        If String.IsNullOrEmpty(ProductDisplayPage) Then
            'since value is the 'default' check the XML file for possible custom setting
            ProductDisplayPage = Me.GetLinkValueFromXML(ATTRIBUTE_PRODUCT_DISPLAY_PAGE)
            If String.IsNullOrEmpty(ProductDisplayPage) Then
                lnkNewMeeting.Enabled = False
                lnkNewMeeting.ToolTip = "ProductDisplayPage property not set."
            End If
        End If   
        'SKB 12/1/2010 Issue 7294: People You May know functionality
        If String.IsNullOrEmpty(PeopleYouMayKnowImage) Then
            'since value is the 'default' check the XML file for possible custom setting
            PeopleYouMayKnowImage = Me.GetLinkValueFromXML(ATTRIBUTE_PEOPLEYOUMAYKNOW_IMAGE_URL)
            imgPeopleYouMayKnow.Src = PeopleYouMayKnowImage
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'set control properties from XML file if needed
        SetProperties()
        If Not IsPostBack Then
            If Me.SetControlRecordIDFromParam() Then
                CheckGroupAdmin()
                SetupMenu()
            End If
        End If
    End Sub

    Protected Sub lnkRegister_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkRegister.Click

        Try
            If ShoppingCart1.AddToCart(CLng(Request.QueryString("ID")), False, , , Page.Session) Then
                Dim sProductPage As String, sOrderPage As String
                If ShoppingCart1.GetProductTypeWebPages(CLng(Request.QueryString("ID")), sProductPage, sOrderPage) Then
                    If Len(sOrderPage) > 0 Then
                        ' special order page. redirect there now
                        Dim oOrder As AptifyGenericEntityBase
                        oOrder = ShoppingCart1.GetOrderObject(Page.Session, Page.User, Page.Application)
                        ShoppingCart1.SaveCart(Page.Session)
                        Response.Redirect(sOrderPage & "?OL=" & _
                                          oOrder.SubTypes("OrderLines").Count - 1)
                    End If
                End If
                lblRegistrationResult.Text = "This meeting has been successfully added to the shopping cart."
            Else
                lnkRegister.Visible = False
                imgRegister.Visible = False
                lblRegistrationResult.ForeColor = Drawing.Color.Red
                lblRegistrationResult.Font.Bold = True
                lblRegistrationResult.Text = "ERROR: This meeting cannot be added to the shopping cart."
                lblRegistrationResult.Visible = True
            End If
        Catch ex As Exception
            Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        End Try

    End Sub

    Private Sub SetupMenu()
        Dim sSQL As String
        Dim dt As DataTable

        'Set Link Paths
        Dim sBase As String = Me.Request.Url.AbsolutePath & "?ID=" & Request.QueryString("ID") & "&View="
        lnkGeneral.NavigateUrl = sBase & "General"
        lnkSpeakers.NavigateUrl = sBase & "Speakers"
        lnkSchedule.NavigateUrl = sBase & "Schedule"
        lnkForum.NavigateUrl = sBase & "Forum"
        lnkTravel.NavigateUrl = sBase & "Travel"
        'SKB 12/1/2010 Issue 7294: People You May know functionality
        lnkPeopleYouMayKnow.NavigateUrl = sBase & "PeopleYouMayKnow"

        'Set Product Price
        'HP Issue#8598:  Product object is not required here since this triggers the Filter Rule which in turn will fail due to BillToPerson or some 
        '                other required Orders object not being available in this context. Instead get the price for the user from ShoppingCart.GetUserProductPrice
        'Dim oProduct As Aptify.Applications.ProductSetup.ProductObject
        Dim oPrice As New Aptify.Applications.OrderEntry.IProductPrice.PriceInfo
        'oProduct = CType(AptifyApplication.GetEntityObject("Products", CLng(Request.QueryString("ID"))), ProductObject)
        'oProduct.GetPrice(oPrice, CLng(Request.QueryString("ID")), 1, User1.PersonID, CurrencyTypeID:=User1.PreferredCurrencyTypeID)
        oPrice = Me.ShoppingCart1.GetUserProductPrice(CLng(Request.QueryString("ID")))

        lblPrice.Text = Format(oPrice.Price, User1.PreferredCurrencyFormat)

        If Me.User1.PersonID > 0 Then
            'HP Issue#8598:  Pricing rule is working correctly however the ShoppingCart1.GetProductMemberSavings method does not have an Order object 
            '                which in turn would have the objects, i.e. BillToPerson, that are required in the Filter Rule therefore no price is returned
            '                and displayed savings is incorrect. In this situation we will extract the non-member price for comparison to the
            '                price set earlier in this method thru the ShoppingCart's GetSingleProductNonMemberCost method
            'Dim dSavings As Decimal = Me.ShoppingCart1.GetProductMemberSavings(Page.User, CLng(Request.QueryString("ID")))
            Dim oNonMemberPrice As New Aptify.Applications.OrderEntry.IProductPrice.PriceInfo
            oNonMemberPrice.Price = Me.ShoppingCart1.GetSingleProductNonMemberCost(Page.User, CLng(Request.QueryString("ID")))
            Dim dSavings As Decimal = oNonMemberPrice.Price - oPrice.Price
            'End Issue#8598

            'Display Member Savings if > 0
            If dSavings > 0 Then
                Me.lblMemSavings.Text = "(" & Format(dSavings, User1.PreferredCurrencyFormat)
                Me.lblMemSavings.Text &= " member savings)"
                Me.lblMemSavings.Visible = True
            Else
                Me.lblMemSavings.Visible = False
            End If
        Else
            Me.lblMemSavings.Visible = False
        End If

        sSQL = "Select m.ID,m.ProductID,p.WebEnabled,p.ID,ParentID = isnull(p.ParentID,-1),m.StatusID,m.EndDate,p.DateAvailable,p.AvailableUntil," & _
                " p.RequireInventory, m.AvailSpace FROM " & _
                       AptifyApplication.GetEntityBaseDatabase("Products") & _
                       "..vwProducts p INNER JOIN " & _
                       AptifyApplication.GetEntityBaseDatabase("Meetings") & _
                       "..vwMeetings m ON p.ID=m.ProductID " & _
                       " WHERE p.WebEnabled=1 AND m.ProductID=" & Me.ControlRecordID

        dt = Me.DataAction.GetDataTable(sSQL)
        If dt IsNot Nothing Then
            If dt.Rows.Count > 0 Then
                '12/05/06 MAS
                'Use helper function ValidateProductAvailability() to determine if this product is available

                With dt.Rows(0)

                    Dim sError As String = "" 'Error string used to display reason product is unavailable to user

                    Dim bLnkRegisterEnabled As Boolean = True 'flag to determine if Registration Link should be enabled
                    ValidateProductAvailability(dt.Rows(0), bLnkRegisterEnabled, sError)

                    'set lnkRegister based on product availability
                    If Not bLnkRegisterEnabled Then
                        lnkRegister.Visible = False
                        imgRegister.Visible = False
                        lblMemSavings.Visible = False
                    End If

                    'display error message if product is unavailable
                    If sError <> "" Then
                        sError &= "<br>" 'add line break
                        lblMeetingStatus.Text = sError
                        lblMeetingStatus.Visible = True
                    End If

                    NewProductAvailable(dt)

                    If Me.User1.PersonID > 0 Then
                        trForum.Visible = True
                    Else
                        trForum.Visible = False
                    End If
                    If CLng(.Item("ParentID")) > 0 Then
                        trRegister.Visible = False
                    Else
                        trRegister.Visible = True
                    End If
                End With

            End If
        End If

    End Sub

    Private Sub NewProductAvailable(ByVal dt2 As DataTable)
        'check to see if a newer version of this product can be offered
        Dim lNewerProductID As Long
        Dim oNewerProductID As Object
        Dim sSQL As String
        Dim dt As DataTable
        Dim lCurrentProductID As Long
        With dt2.Rows(0)
            lCurrentProductID = CLng(.Item("ProductID"))
            sSQL = "SELECT " & AptifyApplication.GetEntityBaseDatabase("Products") & _
            ".dbo.fnGetLatestVersionProductID(" & lCurrentProductID & ")"
            oNewerProductID = DataAction.ExecuteScalar(sSQL)
            If IsNumeric(oNewerProductID) Then
                lNewerProductID = CLng(oNewerProductID)
            Else
                lNewerProductID = -1
            End If
        End With
        'display link to the latest valid version of this product if one exists
        If lNewerProductID > 0 AndAlso lNewerProductID <> lCurrentProductID Then
            sSQL = "SELECT Name, WebName, WebEnabled FROM " & _
                    AptifyApplication.GetEntityBaseDatabase("Products") & _
                    "..vwProducts WHERE ID=" & lNewerProductID
            dt = DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
            If CBool(dt.Rows(0).Item("WebEnabled")) Then
                'a newer product has been found that can be linked to
                lnkNewMeeting.Text = "Click here for the "
                If Not IsDBNull(dt.Rows(0).Item("WebName")) _
                   AndAlso CStr(dt.Rows(0).Item("WebName")) <> "" Then
                    lnkNewMeeting.Text &= CStr(dt.Rows(0).Item("WebName"))
                Else
                    lnkNewMeeting.Text &= CStr(dt.Rows(0).Item("Name"))
                End If
                lnkNewMeeting.PostBackUrl = ProductDisplayPage & "?ID=" & lNewerProductID
                lnkNewMeeting.Visible = True
            End If
        End If


    End Sub

    '12/05/06 MAS
    Private Sub ValidateProductAvailability(ByRef dr As DataRow, ByRef bEnableLink As Boolean, ByRef sErrorMessage As String)
        'Properties checked to determine meeting availability:
        '1. Meeting Status (meeting.StatusID)
        '2. Date Meeting ends (meeting.EndDate)
        '3. Date Product is Available for purchase (product.DateAvailable)
        '4. Date Registration expires (product.AvailableUntil)
        '5. Meeting space limited (product.RequireInventory)
        '6. Meeting has open seats (meeting.AvailSpace)
        'IF all of the above conditions pass, then this meeting product is available for purchase
        'Precondition: bEnableLink should be passed in as true on the first call to ValidateProductAvailability()
        'Postcondition: bEnableLink = true if the current product can be purchased; false if not
        '               sError = message to user regarding this product's availability

        Dim dToday As Date = Today()

        With dr
            '1. meeting status?
            '   meeting.StatusID : 1 = Planned, 2 = Occurred, 3 = Cancelled
            If (CInt(.Item("StatusID")) = 3) Then
                sErrorMessage = "This event has been cancelled."
                bEnableLink = False

                '2.Has the meeting already occurred? 
                'NOTE: meeting.EndDate is a required field. If the meeting entity is changed
                '      to not require this field be specified, this logic will fail.
            ElseIf (CInt(.Item("StatusID")) = 2) Or (CDate(.Item("EndDate")) < dToday) Then
                'sErrorMessage = "This meeting has already occurred."
                bEnableLink = False
            Else 'StatusID = 1 and meeting still valid
                '3. Is this meeting availble for purchase yet?
                If Not IsDBNull(.Item("DateAvailable")) _
                   AndAlso CStr(.Item("DateAvailable")) <> "" _
                   AndAlso CDate(.Item("DateAvailable")) > dToday Then
                    sErrorMessage = "Registration for this event is not availble until " & _
                                     CDate(.Item("DateAvailable")).ToLongDateString & "."
                    bEnableLink = False

                    '4. Has Registration for this meeting expired?
                ElseIf Not IsDBNull(.Item("AvailableUntil")) _
                       AndAlso CStr(.Item("AvailableUntil")) <> "" _
                       AndAlso CDate(.Item("AvailableUntil")) < dToday Then
                    'sErrorMessage = "Registration for this meeting ended on " & _
                    '                 CDate(.Item("DateAvailable")).ToLongDateString & "."
                    bEnableLink = False

                    '5 and 6. Is there space available for another registrant?
                ElseIf CBool(.Item("RequireInventory")) _
                       AndAlso Not IsDBNull(.Item("AvailSpace")) _
                       AndAlso CInt(.Item("AvailSpace")) < 1 Then
                    sErrorMessage = "Registration is full. If you choose to continue, you will be added to the Waiting List."

                Else
                    'Else this meeting is available for Registration
                End If
            End If
        End With
    End Sub

    ''RashmiP, Issue 14326, Group Admin
    Sub CheckGroupAdmin()
        If User1.PersonID <= 0 Then
            HLGroupReg.Visible = False
            imgRegisterGroup.Visible = False
            Exit Sub
        Else
            lblFrimAdminLogin.Visible = False
            lnkLogin.Visible = False
        End If

        Dim dt As DataTable
        dt = Me.DataAction.GetDataTable("Select ID from " & Database & "..vwPersonFunctions Where PersonID=" & User1.PersonID & " and FunctionID_Name='Firm Administrator'")

        If Not dt Is Nothing And dt.Rows.Count > 0 Then
            'Sachin: Case# 13580 & 13586: Based on Product Web Page Type and Web page name, 
            'redirect web user to particular web page

            Dim dtWebPage As DataTable = DataAction.GetDataTable("Select WebPageType, WebProductPage from vwProducts where Id =" & CLng(Request.QueryString("ID")))

            If Not dtWebPage Is Nothing AndAlso dtWebPage.Rows.Count > 0 _
                AndAlso Not dtWebPage.Rows(0)("WebPageType") Is Nothing _
                AndAlso Not dtWebPage.Rows(0)("WebPageType") Is DBNull.Value _
                AndAlso System.Convert.ToString(dtWebPage.Rows(0)("WebPageType")) = "Custom" _
                AndAlso Not dtWebPage.Rows(0)("WebProductPage") Is Nothing _
                AndAlso Not dtWebPage.Rows(0)("WebProductPage") Is DBNull.Value _
                AndAlso System.Convert.ToString(dtWebPage.Rows(0)("WebProductPage")) <> String.Empty Then

                Dim oOrder As AptifyGenericEntityBase
                oOrder = ShoppingCart1.GetOrderObject(Page.Session, Page.User, Page.Application)
                ShoppingCart1.SaveCart(Page.Session)
                Dim sOrderPage As String = System.Convert.ToString(dtWebPage.Rows(0)("WebProductPage"))
                HLGroupReg.NavigateUrl = sOrderPage & "?ID=" & Me.ControlRecordID
                HLGroupReg.Visible = True
                imgRegisterGroup.Visible = True
                imgRegister.Visible = False
                lnkRegister.Visible = False
                Me.lblMemSavings.Visible = False

            Else
                ' HLGroupReg.NavigateUrl = MeetingRegistrationSelectRegistrant & "?ID=" & Me.ControlRecordID
                HLGroupReg.Visible = True
                imgRegisterGroup.Visible = True
                imgRegister.Visible = False
                lnkRegister.Visible = False
                Me.lblMemSavings.Visible = False
            End If
        Else
            imgRegisterGroup.Visible = False
            HLGroupReg.Visible = False
        End If
    End Sub
End Class
