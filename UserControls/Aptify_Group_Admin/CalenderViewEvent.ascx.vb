'Aptify e-Business 5.5.1, July 2013
Imports Telerik.Web.UI
Imports System.Data
Namespace Aptify.Framework.Web.eBusiness.CustomerService
    Partial Class CalenderViewEvent
        Inherits BaseUserControlAdvanced
  
        Dim sSQL As String, dt As DataTable
        Protected Const ATTRIBUTE_LOGIN_PAGE As String = "LoginPage" 'Added by Sandeep for Issue 15051 on 12/03/2013
#Region "Public Properties"

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

        Private Sub LoadMeetings(ByVal EventType As String, ByVal personID As String)

            Try

                If Not String.IsNullOrEmpty(AptifyApplication.GetEntityAttribute("Meetings", "GetRegistredEvents")) Then
                    Dim sSpName As String = CStr(AptifyApplication.GetEntityAttribute("Meetings", "GetRegistredEvents"))
                    sSQL = "exec " + sSpName + "'" + EventType + "','" + personID.ToString + "','" + User1.CompanyID.ToString + "'"
                    'sSQL = "Exec " & sSpName & "'" & EventType & "', " & "'" & PersonID & "', '" & User1.CompanyID & "'"
                End If
                'sSQL = "SELECT distinct Productid, MeetingTitle,StartDate,EndDate,Place " & _
                '          "FROM " & _
                '          Database & _
                '          "..vwGetCountOfRegisteredWaitingPerson where CompanyID=" & Convert.ToString(User1.CompanyID)

                dt = Me.DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)



                EventCalenderScheduler.DataSource = dt
                EventCalenderScheduler.DataKeyField = "MeetingTitle"
                EventCalenderScheduler.DataSubjectField = "MeetingTitle"
                EventCalenderScheduler.DataDescriptionField = "Place"
                EventCalenderScheduler.DataStartField = "StartDate"
                EventCalenderScheduler.DataEndField = "EndDate"

                'EventCalenderScheduler. = "StartDate"
                'EventCalenderScheduler.DataEndField = "EndDate"
                EventCalenderScheduler.TimeZoneOffset = New TimeSpan(0, 0, 0)
                EventCalenderScheduler.DataBind()
                'EventCalenderScheduler.TimeZoneID = "UTC 0"
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Private Sub LoadComboEvent()
            sSQL = "SELECT * " & _
                        "FROM " & _
                 Database & _
                      "..vwProductCategories where DefaultProductType ='Meeting'"

            dt = Me.DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
            CmbEventtype.DataSource = dt
            CmbEventtype.DataTextField = "Name"
            CmbEventtype.DataValueField = "Name"
            CmbEventtype.DataBind()
            CmbEventtype.Items.Insert(0, New RadComboBoxItem("", ""))
        End Sub
        Private Sub LoadComboPersons()
            sSQL = "SELECT * " & _
                        "FROM " & _
                 Database & _
                      "..vwPersons vp where vp.CompanyID = " & User1.CompanyID.ToString & " Order by FirstLast"

            dt = Me.DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
            cmbPerson.DataSource = dt
            cmbPerson.DataTextField = "FirstLast"
            cmbPerson.DataValueField = "ID"
            cmbPerson.DataBind()
            cmbPerson.Items.Insert(0, New RadComboBoxItem("", ""))
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'Added by Sandeep for Issue 15051 on 12/03/2013
            If String.IsNullOrEmpty(LoginPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                LoginPage = Me.GetLinkValueFromXML(ATTRIBUTE_LOGIN_PAGE)
            End If

            If Not IsPostBack Then
                LoadMeetings("", "")
                LoadComboEvent()
                LoadComboPersons()
            End If
            If User1.UserID < 0 Then
                Response.Redirect(LoginPage) 'Added by Sandeep for Issue 15051 on 12/03/2013
            End If
            CmbEventtype.Visible = True
            cmbPerson.Visible = True
            spanFilter.Visible = True
        End Sub


        Protected Sub CmbEventtype_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles CmbEventtype.SelectedIndexChanged
            LoadMeetings(CmbEventtype.SelectedValue, cmbPerson.SelectedValue)
        End Sub

        Protected Sub cmbPerson_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles cmbPerson.SelectedIndexChanged
            LoadMeetings(CmbEventtype.SelectedValue, cmbPerson.SelectedValue)
        End Sub

        Protected Sub EventCalenderScheduler_AppointmentClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.SchedulerEventArgs) Handles EventCalenderScheduler.AppointmentClick
            CmbEventtype.Visible = False
            cmbPerson.Visible = False
            spanFilter.Visible = False
        End Sub

        Protected Function GetDateTimeString(ByVal dateTime As DateTime) As String
            Dim strDateTime As String = ""
            If dateTime.Hour <> 0 OrElse dateTime.Minute <> 0 Then
                strDateTime = dateTime.ToString()
            Else
                strDateTime = dateTime.ToString("MM/dd/yyyy")
            End If

            Return strDateTime
        End Function




    End Class
End Namespace
