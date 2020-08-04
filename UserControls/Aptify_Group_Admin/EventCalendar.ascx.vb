'Aptify e-Business 5.5.1, July 2013
Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Web
Imports System.Web.SessionState
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports Telerik.Web.UI.Calendar
Imports Telerik.Web.UI.RadCalendar
Imports System.Globalization

Namespace Aptify.Framework.Web.eBusiness.Admin
    Partial Class EventCalendar
        'Inherits System.Web.UI.UserControl
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If Not IsPostBack Then
                firsteventdetails.Visible = False
                secondeventdetails.Visible = False
                RadEventCalendar.SelectedDate = DateTime.Today

                SelectedDateMeetingDetails()


            End If
        End Sub


        Protected Sub RadEventCalendar_SelectionChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.Calendar.SelectedDatesEventArgs) Handles RadEventCalendar.SelectionChanged

            SelectedDateMeetingDetails()


        End Sub

        Protected Sub SelectedDateMeetingDetails()
            Dim sSQL As String
            Dim oDT As DataTable
            Dim counter As Int16

            lblSelectedDate.Text = RadEventCalendar.SelectedDate.ToString("dddd, MMMM dd, yyyy")
            lblSelectedDate.Font.Size = "10"
            'lblSelectedDate.Text = CDate(userselecteddate)
            counter = 1

            ' where Condition changes and table to view name change(Meeting to vwMeetings) amruta
            Try
                '' code commented for IssueID - 12351 to remove Hard code AptifyDB reference.
                If (RadEventCalendar.SelectedDate.Equals(DateTime.Today)) Then

                    sSQL = "SELECT COUNT(DISTINCT CASE WHEN AtS.Name='Registered' THEN OMD.ID ELSE NULL END) Registered, " & _
              "    COUNT(DISTINCT CASE WHEN AtS.Name='Waiting' THEN OMD.ID ELSE NULL END) WaitList, " & _
              "    (TM.TotalMembers - " & _
              "    COUNT(DISTINCT CASE WHEN AtS.Name='Registered' THEN OMD.ID ELSE NULL END) - " & _
              "    COUNT(DISTINCT CASE WHEN AtS.Name='Waiting' THEN OMD.ID ELSE NULL END)) [Not Registered], " & _
              "    M.MeetingTitle, M.ProductID FROM " & AptifyApplication.GetEntityBaseDatabase("Companies") & "..vwCompanies C   " & _
              "    JOIN vwPersons P ON P.CompanyID = C.ID JOIN " & AptifyApplication.GetEntityBaseDatabase("OrderMeetDetail") & "..vwOrderMeetDetail OMD ON OMD.AttendeeID = P.ID " & _
              "    JOIN vwAttendeeStatus AtS ON Ats.ID = OMD.StatusID " & _
              "    JOIN vwMeetings M ON OMD.ProductID = M.ProductID " & _
              "    JOIN vwProducts Pro ON M.ProductID = Pro.ID" & _
              "    JOIN " & _
              "    ( " & _
              "          SELECT C.ID CompanyID, COUNT(*) TotalMembers FROM " & AptifyApplication.GetEntityBaseDatabase("Companies") & "..vwCompanies C " & _
              "                 JOIN vwPersons P ON P.CompanyID = C.ID " & _
              "          WHERE C.ID=" & user1.CompanyID & " " & _
              "          GROUP BY C.ID " & _
              "    ) AS TM ON TM.CompanyID=C.ID " & _
              " WHERE '" & RadEventCalendar.SelectedDate.ToString() & "'  " & _
              " BETWEEN M.StartDate AND M.EndDate " & _
              "      AND Pro.WebEnabled=1 " & _
              "GROUP BY C.ID, M.ID, M.MeetingTitle, TM.TotalMembers, " & _
              "    M.ProductID " & _
              " ORDER BY M.MeetingTitle"


                    '' sSQL = "select Top 2 StartDate,MeetingTitle,TotalRegistrants,TotalWaitList from vwMeetings where cast(cast(StartDate as varchar(11)) as datetime)='" & DateTime.Today & "'"

                Else
                    sSQL = "SELECT COUNT(DISTINCT CASE WHEN AtS.Name='Registered' THEN OMD.ID ELSE NULL END) Registered, " & _
            "    COUNT(DISTINCT CASE WHEN AtS.Name='Waiting' THEN OMD.ID ELSE NULL END) WaitList, " & _
            "    (TM.TotalMembers - " & _
            "    COUNT(DISTINCT CASE WHEN AtS.Name='Registered' THEN OMD.ID ELSE NULL END) - " & _
            "    COUNT(DISTINCT CASE WHEN AtS.Name='Waiting' THEN OMD.ID ELSE NULL END)) [Not Registered], " & _
            "    M.MeetingTitle, M.ProductID FROM " & AptifyApplication.GetEntityBaseDatabase("Companies") & "..vwCompanies C   " & _
            "    JOIN vwPersons P ON P.CompanyID = C.ID JOIN " & AptifyApplication.GetEntityBaseDatabase("OrderMeetDetail") & "..vwOrderMeetDetail OMD ON OMD.AttendeeID = P.ID " & _
            "    JOIN vwAttendeeStatus AtS ON Ats.ID = OMD.StatusID " & _
            "    JOIN vwMeetings M ON OMD.ProductID = M.ProductID " & _
            "    JOIN vwProducts Pro ON M.ProductID = Pro.ID" & _
            "    JOIN " & _
            "    ( " & _
            "          SELECT C.ID CompanyID, COUNT(*) TotalMembers FROM " & AptifyApplication.GetEntityBaseDatabase("Companies") & "..vwCompanies C " & _
            "                 JOIN vwPersons P ON P.CompanyID = C.ID " & _
            "          WHERE C.ID=" & user1.CompanyID & " " & _
            "          GROUP BY C.ID " & _
            "    ) AS TM ON TM.CompanyID=C.ID " & _
            " WHERE '" & RadEventCalendar.SelectedDate.ToString() & "'  " & _
            " BETWEEN M.StartDate AND M.EndDate " & _
            "      AND Pro.WebEnabled=1 " & _
            "GROUP BY C.ID, M.ID, M.MeetingTitle, TM.TotalMembers, " & _
            "    M.ProductID " & _
            " ORDER BY M.MeetingTitle"
                    '' sSQL = "select Top 2 StartDate,MeetingTitle,TotalRegistrants,TotalWaitList from vwMeetings where cast(cast(StartDate as varchar(11)) as datetime)='" & RadEventCalendar.SelectedDate.ToString() & "'"

                End If

                oDT = Me.DataAction.GetDataTable(sSQL)

                If oDT.Rows.Count.Equals(0) Then
                    lblmsg.Visible = True

                    firsteventdetails.Visible = False
                    secondeventdetails.Visible = False
                    'lblSelectedDate.Font.Size = "10"
                    'lblSelectedDate.Text = "Meeting(s) not available for this date."
                    lblmsg.Text = "Meeting(s) not available"
                    lblSelectedDate.Visible = False

                Else
                    lblSelectedDate.Visible = True
                    lblmsg.Visible = False
                    For value As Integer = 0 To oDT.Rows.Count - 1

                        If (value.Equals(0)) Then
                            firsteventdetails.Visible = True
                            lblEventName.Text = CStr(oDT.Rows(value)("MeetingTitle"))
                            lblRegisteredCt.Text = CStr(oDT.Rows(value)("Registered"))
                            lblWaitlistCount.Text = CStr(oDT.Rows(value)("WaitList"))
                            secondeventdetails.Visible = False

                        Else
                            secondeventdetails.Visible = True
                            lblMeetingTitle.Text = CStr(oDT.Rows(value)("MeetingTitle"))
                            lblMRegisteredCt.Text = CStr(oDT.Rows(value)("Registered"))
                            lblMWaitlistCount.Text = CStr(oDT.Rows(value)("WaitList"))
                        End If
                    Next


                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub


        'Protected Sub imgbtnrefresh_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnrefresh.Click
        '    SelectedDateMeetingDetails()
        'End Sub

        'Protected Sub imgbtnRefresh2_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnRefresh2.Click
        '    SelectedDateMeetingDetails()
        'End Sub

        'Protected Sub ImageButton1_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        '    SelectedDateMeetingDetails()
        'End Sub

        Protected Sub imgbtnrefresh_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgbtnrefresh.Click
            SelectedDateMeetingDetails()
        End Sub
    End Class
End Namespace
