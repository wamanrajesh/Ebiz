'Aptify e-Business 5.5.1, July 2013
Imports Telerik.Web.UI
Imports Telerik.Charting
Imports System.Drawing
Imports System.Collections
Imports System.Data
Imports Aptify.Framework.DataServices

Namespace Aptify.Framework.Web.eBusiness
    Partial Class MembershipExpireStatus

        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_PARTLY_PAID_ORDER_URL As String = "PartlyPaidOrderUrl"
        Protected Const ATTRIBUTE_PAID_ORDER_URL As String = "PaidOrderUrl"
        Protected Const ATTRIBUTE_UNPAID_ORDER_URL As String = "UnpaidOrderUrl"
        Protected Const ATTRIBUTE_MEMBERSHIP_GOING_TO_EXPIRE_URL As String = "MembershipGoingToExpireUrl"
        Protected Const ATTRIBUTE_MEMBERSHIP_REMAINS_ACTIVE_URL As String = "MembershipRemainsActiveUrl"
        Protected Const ATTRIBUTE_LOGIN_PAGE As String = "LoginPage" 'Added by Sandeep for Issue 15051 on 12/03/2013

#Region "Properties"

        ''' <summary>
        ''' Partly Paid Order Url
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Property PartlyPaidOrderUrl() As String
            Get
                If Not ViewState(ATTRIBUTE_PARTLY_PAID_ORDER_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_PARTLY_PAID_ORDER_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_PARTLY_PAID_ORDER_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        ''' <summary>
        ''' Paid Order Url
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Property PaidOrderUrl() As String
            Get
                If Not ViewState(ATTRIBUTE_PAID_ORDER_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_PAID_ORDER_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_PAID_ORDER_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        ''' <summary>
        ''' Unpaid Order Url
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Property UnpaidOrderUrl() As String
            Get
                If Not ViewState(ATTRIBUTE_UNPAID_ORDER_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_UNPAID_ORDER_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_UNPAID_ORDER_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        ''' <summary>
        ''' Membership Going To ExpireUrl
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Property MembershipGoingToExpireUrl() As String
            Get
                If Not ViewState(ATTRIBUTE_MEMBERSHIP_GOING_TO_EXPIRE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_MEMBERSHIP_GOING_TO_EXPIRE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_MEMBERSHIP_GOING_TO_EXPIRE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        ''' <summary>
        ''' Membership Remails Active Url
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Property MembershipRemainsActiveUrl() As String
            Get
                If Not ViewState(ATTRIBUTE_MEMBERSHIP_REMAINS_ACTIVE_URL) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_MEMBERSHIP_REMAINS_ACTIVE_URL))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_MEMBERSHIP_REMAINS_ACTIVE_URL) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
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

        Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
            ''Dim meeting = db.p_SelectMeetings.ToList()
            SetProperties()
            If Not IsPostBack Then
                PopulateDateDropdown()
                PopulateChart1()
                PopulateChart2()
            End If

            Session.Add("MembershipExpStatus", DropDownList1.SelectedItem.Value)
        
            lnkUnpaid.NavigateUrl = UnpaidOrderUrl & "?OrderStatus=UnPaid&Date=" & CType(DropDownList2.SelectedItem.Value, Date).ToShortDateString
            lnkPaid.NavigateUrl = PaidOrderUrl & "?OrderStatus=Paid&Date=" & CType(DropDownList2.SelectedItem.Value, Date).ToShortDateString
            lnkParty.NavigateUrl = PartlyPaidOrderUrl & "?OrderStatus=PartlyPaid&Date=" & CType(DropDownList2.SelectedItem.Value, Date).ToShortDateString
            'Amruta Issue 14878,03/11/2013,URL set to redirect on company directory page from groupadmin dashboard membership expiration status area
            A1.NavigateUrl = MembershipGoingToExpireUrl & "?MembershipStatus=GoingToExpire&Month=" & DropDownList1.SelectedItem.Value
            A2.NavigateUrl = MembershipRemainsActiveUrl & "?MembershipStatus=RemainsActive&Month=" & DropDownList1.SelectedItem.Value
            If user1.UserID < 0 Then
                Response.Redirect(LoginPage) 'Added by Sandeep for Issue 15051 on 12/03/2013
            End If

        End Sub

        Protected Overrides Sub SetProperties()
            If String.IsNullOrEmpty(PartlyPaidOrderUrl) Then
                PartlyPaidOrderUrl = Me.GetLinkValueFromXML(ATTRIBUTE_PARTLY_PAID_ORDER_URL)
            End If
            If String.IsNullOrEmpty(PaidOrderUrl) Then
                PaidOrderUrl = Me.GetLinkValueFromXML(ATTRIBUTE_PAID_ORDER_URL)
            End If
            If String.IsNullOrEmpty(UnpaidOrderUrl) Then
                UnpaidOrderUrl = Me.GetLinkValueFromXML(ATTRIBUTE_UNPAID_ORDER_URL)
            End If
            If String.IsNullOrEmpty(MembershipGoingToExpireUrl) Then
                MembershipGoingToExpireUrl = Me.GetLinkValueFromXML(ATTRIBUTE_MEMBERSHIP_GOING_TO_EXPIRE_URL)
            End If
            If String.IsNullOrEmpty(MembershipRemainsActiveUrl) Then
                MembershipRemainsActiveUrl = Me.GetLinkValueFromXML(ATTRIBUTE_MEMBERSHIP_REMAINS_ACTIVE_URL)
            End If
            'Added by Sandeep for Issue 15051 on 12/03/2013
            If String.IsNullOrEmpty(LoginPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                LoginPage = Me.GetLinkValueFromXML(ATTRIBUTE_LOGIN_PAGE)
            End If
        End Sub

        Public Sub PopulateChart1()
            Dim companyID As Integer = -1
            Dim oGE As Aptify.Applications.Persons.PersonsEntity = DirectCast(Me.AptifyApplication.GetEntityObject("Persons", user1.PersonID), Aptify.Applications.Persons.PersonsEntity)

            If oGE IsNot Nothing Then
                companyID = oGE.CompanyID
            End If

            RadChart1.Clear()
            'Dim sSQL, sSql1, sSql2, sSql3, sSql4 As String, dt, dt1, dt2, dt3, dt4 As DataTable 

            Dim sSQL As String
            Dim month As Integer

            If DropDownList1.SelectedItem.Value = 90 Then
                month = 3
            Else
                month = 6
            End If

            'sSQL = Me.AptifyApplication.GetEntityBaseDatabase("Subscriptions") & _
            '       "..spGetMembershipExpirationStatus @Days='" & DropDownList1.SelectedItem.Value & "',@CompanyID=" & companyID

            sSQL = Me.AptifyApplication.GetEntityBaseDatabase("vwPersons") & ".." & _
                  " spGetMembershipStatusCount " & CType(month, String) & "," & CType(companyID, String) & "," & CType(user1.PersonID, String)

            Dim dtMembershipExpirationStatus As DataTable = Me.DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)

            Dim membershipGoingToExpire As Integer = 0
            Dim membershipRemainsActive As Integer = 0

            If dtMembershipExpirationStatus IsNot Nothing AndAlso dtMembershipExpirationStatus.Rows.Count > 0 Then
                membershipGoingToExpire = dtMembershipExpirationStatus.Rows(0)("MembershipGoingToExpire")
                membershipRemainsActive = dtMembershipExpirationStatus.Rows(0)("MembershipRemainsActive")
            End If

            'If DropDownList1.SelectedItem.Value = 90 Then



            '    'Dilip issue 12717 FOr changing top
            '    'Nalini Issue 13274 for adding person photo

            '    sSQL = "select COUNT(*) Appear from " & Database & "..vwSubscriptions vs where EndDate > DATEADD(day,90,GETDATE()) and vs.SubscriberCompanyID=" & companyID
            '    sSql1 = "select COUNT(*) expired from " & Database & "..vwSubscriptions vs where EndDate < DATEADD(day,90,GETDATE()) and vs.SubscriberCompanyID=" & companyID
            '    dt = DataAction.GetDataTable(sSQL)
            '    dt1 = DataAction.GetDataTable(sSql1)
            '    Appear = dt.Rows(0)("Appear")
            '    expired = dt1.Rows(0)("expired")
            'End If
            'If DropDownList1.SelectedItem.Value = 180 Then



            '    'Dilip issue 12717 FOr changing top
            '    'Nalini Issue 13274 for adding person photo

            '    sSQL = "select COUNT(*) Appear from " & Database & "..vwSubscriptions vs where EndDate > DATEADD(day,180,GETDATE()) and vs.SubscriberCompanyID=" & companyID
            '    sSql1 = "select COUNT(*) expired from " & Database & "..vwSubscriptions vs where EndDate < DATEADD(day,180,GETDATE()) and vs.SubscriberCompanyID=" & companyID
            '    dt = DataAction.GetDataTable(sSQL)
            '    dt1 = DataAction.GetDataTable(sSql1)
            '    Appear = dt.Rows(0)("Appear")
            '    expired = dt1.Rows(0)("expired")
            'End If

            Dim cseries As ChartSeries = New ChartSeries("", ChartSeriesType.Pie)
            Dim csItem As ChartSeriesItem = Nothing
            Dim leg As New ChartLegend
            Dim lblitem As New LabelItem("ss")
            leg.AddLabel(lblitem)
            For i As Integer = 0 To 0
                csItem =
                New ChartSeriesItem(0, Convert.ToDouble(membershipGoingToExpire))
                'csItem.ActiveRegion.Url =
                '"Expire.aspx"
                'csItem.ActiveRegion.Attributes =
                '"_blank"
                'csItem.ActiveRegion.Tooltip =
                '"Expired"
                csItem.Name =
                "Expired"
                ' csItem.Appearance.FillStyles.MainColor.GetHashCode()
                cseries.Items.Add(csItem)
            Next
            For i As Integer = 0 To 0
                csItem =
                New ChartSeriesItem(0, Convert.ToDouble(membershipRemainsActive))
                'csItem.ActiveRegion.Url =
                '"Expire.aspx"
                'csItem.ActiveRegion.Tooltip =
                '"Appear"
                'csItem.ActiveRegion.Attributes =
                '"_blank"
                ' csItem.Appearance.FillStyle.MainColor.GetHashCode
                csItem.Name =
                "Appear"
                cseries.Items.Add(csItem)
            Next
            RadChart1.AddChartSeries(cseries)
            RadChart1.IntelligentLabelsEnabled = True
            Dim colorexpire As String = RandomQBColor()
            Dim colorattended As String = RandomQBColor()

            RadChart1.Series(0).Items(0).Appearance.FillStyle.MainColor = System.Drawing.Color.Orange
            RadChart1.Series(0).Items(0).Appearance.FillStyle.SecondColor = System.Drawing.Color.Orange
            RadChart1.Series(0).Items(1).Appearance.FillStyle.MainColor = System.Drawing.Color.Green
            RadChart1.Series(0).Items(1).Appearance.FillStyle.SecondColor = System.Drawing.Color.Green
            ' RadChart1.DataBind()

            RadChart1.Series(0).Items(0).Label.Appearance.Visible = False
            RadChart1.Series(0).Items(1).Label.Appearance.Visible = False

            'divexpire.Style.Add("background-color", System.Drawing.Color.Bisque.ToString())
            'divRemain.Style.Add("background-color", System.Drawing.Color.Red.ToString())
            A1.Text = "Going to Expire (" + membershipGoingToExpire.ToString() + ")"
            A2.Text = "Remains Active (" + membershipRemainsActive.ToString() + ")"
            'A1.NavigateUrl = Request.Url.AbsoluteUri & "#"
            'A2.NavigateUrl = Request.Url.AbsoluteUri & "#"


            'Amruta IssueId:14878 
            If (membershipGoingToExpire = 0) Then
                A1.Style.Add("text-decoration", "none")
                A1.Style.Add("cursor", "default")
                A1.Enabled = False
            Else
                A1.Style.Add("text-decoration", "underline")
                A1.Style.Add("cursor", "pointer")
                A1.Enabled = True
            End If

            If (membershipRemainsActive = 0) Then
                A2.Style.Add("text-decoration", "none")
                A2.Style.Add("cursor", "default")
                A2.Enabled = False
            Else
                A2.Style.Add("text-decoration", "underline")
                A2.Style.Add("cursor", "pointer")
                A2.Enabled = True
            End If

        End Sub
        Public Sub PopulateChart2()
            Dim companyID As Integer = -1
            Dim oGE As Aptify.Applications.Persons.PersonsEntity = DirectCast(Me.AptifyApplication.GetEntityObject("Persons", user1.PersonID), Aptify.Applications.Persons.PersonsEntity)

            If oGE IsNot Nothing Then
                companyID = oGE.CompanyID
            End If

            RadChart2.Clear()
            Dim sSQL As String

            sSQL = Me.AptifyApplication.GetEntityBaseDatabase("Orders") & _
                   "..spGetOrderStatusSummary @OrderDate='" & DropDownList2.SelectedItem.Value & "',@CompanyID=" & companyID
            Dim dtOrderStatusSummary As DataTable = Me.DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)

            Dim partlyPaidCount As Integer = 0
            Dim paidCount As Integer = 0
            Dim expired11 As Integer = 0

            If dtOrderStatusSummary IsNot Nothing AndAlso dtOrderStatusSummary.Rows.Count > 0 Then
                partlyPaidCount = dtOrderStatusSummary.Rows(0)("PartlyPaidCount")
                paidCount = dtOrderStatusSummary.Rows(0)("PaidCount")
                expired11 = dtOrderStatusSummary.Rows(0)("UnpaidCount")
            End If


            Dim cseries1 As ChartSeries = New ChartSeries("", ChartSeriesType.Pie)
            Dim csItem1 As ChartSeriesItem = Nothing
            Dim leg1 As New ChartLegend
            Dim lblitem1 As New LabelItem("ss")
            leg1.AddLabel(lblitem1)
            ''If Appear1 <> 0 And expired1 <> 0 And expired11 <> 0 Then
            For i As Integer = 0 To 0
                csItem1 =
                New ChartSeriesItem(0, Convert.ToDouble(partlyPaidCount))
                'csItem1.ActiveRegion.Url =
                '"Expire.aspx"
                'csItem1.ActiveRegion.Attributes =
                '"_blank"
                'csItem1.ActiveRegion.Tooltip =
                '"Expired"
                csItem1.Name =
                "Expired"
                ' csItem.Appearance.FillStyles.MainColor.GetHashCode()
                cseries1.Items.Add(csItem1)
            Next
            For i As Integer = 0 To 0
                csItem1 =
                New ChartSeriesItem(0, Convert.ToDouble(paidCount))
                'csItem1.ActiveRegion.Url =
                '"Expire.aspx"
                'csItem1.ActiveRegion.Tooltip =
                '"Appear"
                'csItem1.ActiveRegion.Attributes =
                '"_blank"
                ' csItem.Appearance.FillStyle.MainColor.GetHashCode
                csItem1.Name =
                "Appear"
                cseries1.Items.Add(csItem1)
            Next
            For i As Integer = 0 To 0
                csItem1 =
                New ChartSeriesItem(0, Convert.ToDouble(expired11))
                'csItem1.ActiveRegion.Url =
                '"Expire.aspx"
                'csItem1.ActiveRegion.Tooltip =
                '"Appear"
                'csItem1.ActiveRegion.Attributes =
                '"_blank"
                ' csItem.Appearance.FillStyle.MainColor.GetHashCode
                csItem1.Name =
                "Appear"
                cseries1.Items.Add(csItem1)
            Next
            RadChart2.AddChartSeries(cseries1)
            RadChart2.IntelligentLabelsEnabled = True
            Dim colorexpire1 As String = RandomQBColor()
            Dim colorattended1 As String = RandomQBColor()

            RadChart2.Series(0).Items(0).Appearance.FillStyle.MainColor = System.Drawing.Color.Orange
            RadChart2.Series(0).Items(0).Appearance.FillStyle.SecondColor = System.Drawing.Color.Orange
            RadChart2.Series(0).Items(1).Appearance.FillStyle.MainColor = System.Drawing.Color.Green
            RadChart2.Series(0).Items(1).Appearance.FillStyle.SecondColor = System.Drawing.Color.Green
            RadChart2.Series(0).Items(2).Appearance.FillStyle.MainColor = System.Drawing.Color.Red
            RadChart2.Series(0).Items(2).Appearance.FillStyle.SecondColor = System.Drawing.Color.Red


            RadChart2.Series(0).Items(0).Label.Appearance.Visible = False
            RadChart2.Series(0).Items(1).Label.Appearance.Visible = False
            RadChart2.Series(0).Items(2).Label.Appearance.Visible = False


            If (partlyPaidCount = 0) Then
                lnkParty.Style.Add("text-decoration", "none")
                lnkParty.Style.Add("cursor", "default")
                lnkParty.Enabled = False
            Else
                lnkParty.Style.Add("text-decoration", "underline")
                lnkParty.Style.Add("cursor", "pointer")
                lnkParty.Enabled = True
            End If
            If (paidCount = 0) Then
                lnkPaid.Style.Add("text-decoration", "none")
                lnkPaid.Style.Add("cursor", "default")
                lnkPaid.Enabled = False
            Else
                lnkPaid.Style.Add("text-decoration", "underline")
                lnkPaid.Style.Add("cursor", "pointer")
                lnkPaid.Enabled = True
            End If
            If (expired11 = 0) Then
                lnkUnpaid.Style.Add("text-decoration", "none")
                lnkUnpaid.Style.Add("cursor", "default")
                lnkUnpaid.Enabled = False
            Else
                lnkUnpaid.Style.Add("text-decoration", "underline")
                lnkUnpaid.Style.Add("cursor", "pointer")
                lnkUnpaid.Enabled = True
            End If

            lnkParty.Text = "Partly Paid  (" + partlyPaidCount.ToString() + ")"
            lnkPaid.Text = "Paid  (" + paidCount.ToString() + ")"
            lnkUnpaid.Text = "Unpaid  (" + expired11.ToString() + ")"
            'lnkUnpaid.NavigateUrl = "~/OrdersManagement/AdminOrderDetail?OrderStatus=UnPaid&Date=" & CType(DropDownList2.SelectedItem.Value, Date).ToShortDateString
            'lnkPaid.NavigateUrl = "~/OrdersManagement/OrderHistory_Admin?Date=" & CType(DropDownList2.SelectedItem.Value, Date).ToShortDateString
            'lnkParty.NavigateUrl = "~/OrdersManagement/AdminOrderDetail?OrderStatus=PartlyPaid&Date=" & CType(DropDownList2.SelectedItem.Value, Date).ToShortDateString
            ''Else
            'div3.Visible = False
            'div4.Visible = False
            'div5.Visible = False
            '' End If


        End Sub
        Private Sub PopulateDateDropdown()
            Try
                'Uncomment to populate dropdown with only meeting dates
                'Dim sSQL As String = "SELECT DISTINCT CAST(CAST(StartDate AS VARCHAR(11)) AS DATETIME) AS StartDate FROM " + AptifyApplication.GetEntityBaseDatabase("Meetings") + ".." + AptifyApplication.GetEntityBaseView("Meetings") + " WHERE StartDate >= GETDATE() ORDER BY StartDate"
                'Dim oDA As DataAction = New DataAction(AptifyApplication.UserCredentials)
                'Dim odt As DataTable = oDA.GetDataTable(sSQL)

                'cmbDate.DataSource = odt
                'cmbDate.DataTextField = "StartDate"
                'cmbDate.DataValueField = "StartDate"
                'cmbDate.DataBind()

                Dim dt As Date = Date.Today

                For i As Integer = 1 To 12
                    'cmbDate.Items.Add(New ListItem(MonthName(dt.Month) + " " + dt.Year.ToString))
                    DropDownList2.Items.Add(New ListItem(dt.ToString("MMMM, yyyy"), dt.ToString))

                    dt = Date.Today.AddMonths(-i)
                Next

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Private m_Rnd As New Random
        ' Return a random QB color.
        Public Function RandomQBColor() As String
            Dim color_num As Integer = m_Rnd.Next(27, 173)
            Dim colorslist As New ArrayList
            colorslist = RandomRGBColors(colorslist)
            Return colorslist.Item(color_num).ToString()
        End Function
        ' Return a random RGB color.
        Public Function RandomRGBColor() As System.Drawing.Color
            Return System.Drawing.Color.FromArgb(255, _
            m_Rnd.Next(0, 255), _
            m_Rnd.Next(0, 255), _
            m_Rnd.Next(0, 255))
        End Function
        Public Function RandomRGBColors(ByVal lst As ArrayList) As ArrayList
            For Each knownColor As KnownColor In [Enum].GetValues(GetType(KnownColor))
                lst.Add(
                String.Format("{0}", knownColor))
            Next
            Return lst

        End Function


        Protected Sub RadChart1_PrePaint(ByVal sender As Object, ByVal args As EventArgs)
            ' Do not post back from the legend
            'If RadChart1.Series(0).Name <> "Months" Then
            'RadChart1.Legend.Items(0).ActiveRegion.Url = "www.google.com"
            'RadChart1.Legend.Items(0).Marker.ActiveRegion.Url = "www.google.com"
            'RadChart1.Legend.ActiveRegion.Url = "www.google.com"
            'End If
        End Sub

        Protected Sub DropDownList2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownList2.SelectedIndexChanged
            PopulateChart2()
        End Sub

        Protected Sub DropDownList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownList1.SelectedIndexChanged
            PopulateChart1()
            Session.Add("MembershipExpStatus", DropDownList1.SelectedItem.Value)

        End Sub
    End Class
End Namespace
