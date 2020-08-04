'Aptify e-Business 5.5.1, July 2013
Namespace Aptify.Framework.Web.eBusiness
    Partial Class ManageMyGroup
        Inherits BaseUserControlAdvanced
        Protected Const ATTRIBUTE_MANAGEMYGROUP_PAGE As String = "ManageMyGroup"
        Protected Const ATTRIBUTE_ADDMEMBER As String = "AddMember"
        Protected Const ATTRIBUTE_PURCHASEMEMBER As String = "PurchaseMember"
        Protected Const ATTRIBUTE_COMPANYPROFILE As String = "CompanyProfile"
        Protected Const ATTRIBUTE_RENEWMEMBER As String = "RenewMember"
        Protected Const ATTRIBUTE_ORDERHISTORY As String = "OrderHistory"
        Protected Const ATTRIBUTE_COMPANYDIRECTORY As String = "CompanyDirectory"

        Protected Const ATTRIBUTE_EVENTREGISTRATION As String = "EventRegistration"
        Protected Const ATTRIBUTE_SUBSTITUTEATTENDEE As String = "SubstituteAttendee"
        Protected Const ATTRIBUTE_MEETINGTRANSFER As String = "MeetingTransfer"
        Protected Const ATTRIBUTE_PAYOFFORDER As String = "PayOffOrder"
        Protected Const ATTRIBUTE_LOGIN_PAGE As String = "LoginPage" 'Added by Sandeep for Issue 15051 on 12/03/2013


#Region " Specific Properties"
        ''' <summary>
        ''' Manage My Group page url
        ''' </summary>
        Public Overridable Property ManageMyGroup() As String
            Get
                If Not ViewState(ATTRIBUTE_MANAGEMYGROUP_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_MANAGEMYGROUP_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_MANAGEMYGROUP_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' Links for Menu Options
        ''' </summary>
        Public Overridable Property AddMember() As String
            Get
                If Not ViewState(ATTRIBUTE_ADDMEMBER) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_ADDMEMBER))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_ADDMEMBER) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        Public Overridable Property PurchaseMember() As String
            Get
                If Not ViewState(ATTRIBUTE_PURCHASEMEMBER) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_PURCHASEMEMBER))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_PURCHASEMEMBER) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        Public Overridable Property CompanyProfile() As String
            Get
                If Not ViewState(ATTRIBUTE_COMPANYPROFILE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_COMPANYPROFILE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_COMPANYPROFILE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        Public Overridable Property RenewMember() As String
            Get
                If Not ViewState(ATTRIBUTE_RENEWMEMBER) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_RENEWMEMBER))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_RENEWMEMBER) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        Public Overridable Property OrderHistory() As String
            Get
                If Not ViewState(ATTRIBUTE_ORDERHISTORY) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_ORDERHISTORY))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_ORDERHISTORY) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        Public Overridable Property CompanyDirectory() As String
            Get
                If Not ViewState(ATTRIBUTE_COMPANYDIRECTORY) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_COMPANYDIRECTORY))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_COMPANYDIRECTORY) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        Public Overridable Property EventRegistration() As String
            Get
                If Not ViewState(ATTRIBUTE_EVENTREGISTRATION) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_EVENTREGISTRATION))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_EVENTREGISTRATION) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        Public Overridable Property MeetingTransfer() As String
            Get
                If Not ViewState(ATTRIBUTE_MEETINGTRANSFER) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_MEETINGTRANSFER))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_MEETINGTRANSFER) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        Public Overridable Property SubstituteAttendee() As String
            Get
                If Not ViewState(ATTRIBUTE_SUBSTITUTEATTENDEE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_SUBSTITUTEATTENDEE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_SUBSTITUTEATTENDEE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        Public Overridable Property PayOffOrder() As String
            Get
                If Not ViewState(ATTRIBUTE_PAYOFFORDER) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_PAYOFFORDER))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_PAYOFFORDER) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        'Nalini issue 11290

        'Added by Sandeep for Issue 15051 on 12/03/2013
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

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_MANAGEMYGROUP_PAGE
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(ManageMyGroup) Then
                'since value is the 'default' check the XML file for possible custom setting
                ManageMyGroup = Me.GetLinkValueFromXML(ATTRIBUTE_MANAGEMYGROUP_PAGE)
                If String.IsNullOrEmpty(ManageMyGroup) Then
                    'Do Nothing
                End If
            End If
            If String.IsNullOrEmpty(AddMember) Then
                'since value is the 'default' check the XML file for possible custom setting
                AddMember = Me.GetLinkValueFromXML(ATTRIBUTE_ADDMEMBER)
                If String.IsNullOrEmpty(AddMember) Then
                    Me.lnkAddMember.Enabled = False
                    Me.lnkAddMember.ToolTip = "MeetingPage property has not been set."
                End If
            End If
            If String.IsNullOrEmpty(PurchaseMember) Then
                'since value is the 'default' check the XML file for possible custom setting
                PurchaseMember = Me.GetLinkValueFromXML(ATTRIBUTE_PURCHASEMEMBER)
                If String.IsNullOrEmpty(PurchaseMember) Then
                    Me.lnkPurchaseMembership.Enabled = False
                    Me.lnkPurchaseMembership.ToolTip = "MeetingPage property has not been set."
                End If
            End If
            If String.IsNullOrEmpty(CompanyProfile) Then
                'since value is the 'default' check the XML file for possible custom setting
                CompanyProfile = Me.GetLinkValueFromXML(ATTRIBUTE_COMPANYPROFILE)
                If String.IsNullOrEmpty(CompanyProfile) Then
                    Me.lnkCompanyProfile.Enabled = False
                    Me.lnkCompanyProfile.ToolTip = "MeetingPage property has not been set."
                End If
            End If
            If String.IsNullOrEmpty(RenewMember) Then
                'since value is the 'default' check the XML file for possible custom setting
                RenewMember = Me.GetLinkValueFromXML(ATTRIBUTE_RENEWMEMBER)
                If String.IsNullOrEmpty(RenewMember) Then
                    Me.lnkRenewMembership.Enabled = False
                    Me.lnkRenewMembership.ToolTip = "MeetingPage property has not been set."
                End If
            End If
            If String.IsNullOrEmpty(OrderHistory) Then
                'since value is the 'default' check the XML file for possible custom setting
                OrderHistory = Me.GetLinkValueFromXML(ATTRIBUTE_ORDERHISTORY)
                If String.IsNullOrEmpty(OrderHistory) Then
                    Me.lnkOrderHistory.Enabled = False
                    Me.lnkOrderHistory.ToolTip = "MeetingPage property has not been set."
                End If
            End If
            If String.IsNullOrEmpty(CompanyDirectory) Then
                'since value is the 'default' check the XML file for possible custom setting
                CompanyDirectory = Me.GetLinkValueFromXML(ATTRIBUTE_COMPANYDIRECTORY)
                If String.IsNullOrEmpty(OrderHistory) Then
                    Me.lnkCmpDirectory.Enabled = False
                    Me.lnkCmpDirectory.ToolTip = "MeetingPage property has not been set."
                End If
            End If
            If String.IsNullOrEmpty(EventRegistration) Then
                'since value is the 'default' check the XML file for possible custom setting
                EventRegistration = Me.GetLinkValueFromXML(ATTRIBUTE_EVENTREGISTRATION)
                If String.IsNullOrEmpty(EventRegistration) Then
                    Me.lnkEventRegistration.Enabled = False
                    Me.lnkEventRegistration.ToolTip = "MeetingPage property has not been set."
                End If
            End If
            If String.IsNullOrEmpty(SubstituteAttendee) Then
                'since value is the 'default' check the XML file for possible custom setting
                SubstituteAttendee = Me.GetLinkValueFromXML(ATTRIBUTE_SUBSTITUTEATTENDEE)
                If String.IsNullOrEmpty(SubstituteAttendee) Then
                    Me.lnkMeetingAttendee.Enabled = False
                    Me.lnkMeetingAttendee.ToolTip = "MeetingPage property has not been set."
                End If
            End If
            If String.IsNullOrEmpty(MeetingTransfer) Then
                'since value is the 'default' check the XML file for possible custom setting
                MeetingTransfer = Me.GetLinkValueFromXML(ATTRIBUTE_MEETINGTRANSFER)
                If String.IsNullOrEmpty(MeetingTransfer) Then
                    Me.lnkMeetingTransfer.Enabled = False
                    Me.lnkMeetingTransfer.ToolTip = "MeetingPage property has not been set."
                End If
            End If
            If String.IsNullOrEmpty(PayOffOrder) Then
                'since value is the 'default' check the XML file for possible custom setting
                PayOffOrder = Me.GetLinkValueFromXML(ATTRIBUTE_PAYOFFORDER)
                If String.IsNullOrEmpty(PayOffOrder) Then
                    Me.lnkPayOffOrder.Enabled = False
                    Me.lnkPayOffOrder.ToolTip = "MeetingPage property has not been set."
                End If
            End If
            'Added by Sandeep for Issue 15051 on 12/03/2013
            If String.IsNullOrEmpty(LoginPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                LoginPage = Me.GetLinkValueFromXML(ATTRIBUTE_LOGIN_PAGE)
            End If
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            SetProperties()
            If Not IsPostBack Then
                lnkAddMember.NavigateUrl = Me.GetLinkValueFromXML(ATTRIBUTE_ADDMEMBER)
                lnkCompanyProfile.NavigateUrl = Me.GetLinkValueFromXML(ATTRIBUTE_COMPANYPROFILE)
                lnkOrderHistory.NavigateUrl = Me.GetLinkValueFromXML(ATTRIBUTE_ORDERHISTORY)
                lnkPurchaseMembership.NavigateUrl = Me.GetLinkValueFromXML(ATTRIBUTE_PURCHASEMEMBER)
                lnkRenewMembership.NavigateUrl = Me.GetLinkValueFromXML(ATTRIBUTE_RENEWMEMBER)
                lnkCmpDirectory.NavigateUrl = Me.GetLinkValueFromXML(ATTRIBUTE_COMPANYDIRECTORY)
                lnkEventRegistration.NavigateUrl = Me.GetLinkValueFromXML(ATTRIBUTE_EVENTREGISTRATION)
                lnkMeetingAttendee.NavigateUrl = Me.GetLinkValueFromXML(ATTRIBUTE_SUBSTITUTEATTENDEE)
                lnkMeetingTransfer.NavigateUrl = Me.GetLinkValueFromXML(ATTRIBUTE_MEETINGTRANSFER)
                lnkPayOffOrder.NavigateUrl = Me.GetLinkValueFromXML(ATTRIBUTE_PAYOFFORDER)
            End If
            If User1.UserID < 0 Then
                Response.Redirect(LoginPage) 'Added by Sandeep for Issue 15051 on 12/03/2013
            End If
        End Sub
    End Class
End Namespace