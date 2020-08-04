'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On
Imports System.Data
Imports Aptify.Framework.DataServices



Namespace Aptify.Framework.Web.eBusiness
    Partial Class UpcomingEvents
        Inherits eBusiness.BaseUserControlAdvanced
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "UpcomingEvents"
       
        Protected Const ATTRIBUTE_MEETING_PAGE As String = "MeetingPage"
        Protected Const ATTRIBUTE_MEETING_ALL_PAGE As String = "MeetingAllPage"
        Protected Const ATTRIBUTE_SHOWMEETINGSLINKTOCLASS_DEFAULT_NAME As String = "ShowMeetingsLinkToClass"
    
        Public Overridable Property MeetingPage() As String
            Get
                If Not ViewState(ATTRIBUTE_MEETING_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_MEETING_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_MEETING_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        Public Overridable Property MeetingAllPage() As String
            Get
                If Not ViewState(ATTRIBUTE_MEETING_ALL_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_MEETING_ALL_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_MEETING_ALL_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        Protected Overrides Sub SetProperties()
            Dim bShowMeeting As Boolean
            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            MyBase.SetProperties()

            If String.IsNullOrEmpty(MeetingPage) Then
                MeetingPage = Me.GetLinkValueFromXML(ATTRIBUTE_MEETING_PAGE)
            End If

            If String.IsNullOrEmpty(MeetingAllPage) Then
                MeetingAllPage = Me.GetLinkValueFromXML(ATTRIBUTE_MEETING_ALL_PAGE)
            End If
            bShowMeeting = ShowMeetingsLinkToClass()
        End Sub

        'Nalini issue 11290
        Protected Overridable ReadOnly Property ShowMeetingsLinkToClass() As Boolean
            Get
                If Not ViewState.Item(ATTRIBUTE_SHOWMEETINGSLINKTOCLASS_DEFAULT_NAME) Is Nothing Then
                    Return CBool(ViewState.Item(ATTRIBUTE_SHOWMEETINGSLINKTOCLASS_DEFAULT_NAME))
                Else
                    Dim value As String = Me.GetGlobalAttributeValue(ATTRIBUTE_SHOWMEETINGSLINKTOCLASS_DEFAULT_NAME)
                    If Not String.IsNullOrEmpty(value) Then
                        Select Case UCase(value)
                            Case "TRUE", "FALSE", "0", "1"
                                ViewState.Item(ATTRIBUTE_SHOWMEETINGSLINKTOCLASS_DEFAULT_NAME) = CBool(value)
                            Case Else
                                ViewState.Item(ATTRIBUTE_SHOWMEETINGSLINKTOCLASS_DEFAULT_NAME) = False
                        End Select
                    Else
                        ViewState.Item(ATTRIBUTE_SHOWMEETINGSLINKTOCLASS_DEFAULT_NAME) = False
                    End If
                End If
            End Get
        End Property

        Private Sub LoadSchedule()
            Dim sSQL As String, dt As DataTable, sDB As String
            Dim bShowMeeting As Boolean
            bShowMeeting = ShowMeetingsLinkToClass()
            Try

                sDB = Me.AptifyApplication.GetEntityBaseDatabase("Meetings")
                sSQL = " SELECT top 3  p.ID ProductID,p.WebName,p.WebImage, p.Description,p.WebDescription, " & _
                       " m.StartDate,m.EndDate,CASE ISNULL(m.State,'') WHEN '' " & _
                       " THEN m.Place + ' - ' + m.City + ', ' + m.Country ELSE " & _
                       " m.Place + ' - ' + m.City + ', ' + m.State + ' ' + m.Country END Location, " & _
                       " m.StartDate,m.EndDate,pc.WebName WebCategoryName " & _
                       " FROM " & sDB & "..vwProducts p INNER JOIN " & _
                       sDB & "..vwMeetings m ON p.ID=m.ProductID " & _
                       " INNER JOIN " & sDB & "..vwProductCategories pc " & _
                       " ON pc.ID=p.CategoryID " & _
                       " WHERE ISNULL(p.ParentID,-1)<=0 AND pc.WebEnabled=1 AND p.WebEnabled=1"
                'Dilip(changes) for upcoming event
                sSQL &= " AND DATEDIFF(dd,GETDATE(),m.StartDate) >= 0"
           
                If Not bShowMeeting Then
                    sSQL &= "  AND  ISNULL(p.ClassID ,-1) <=0 "
                End If
                sSQL &= " ORDER BY m.StartDate, m.EndDate "

                dt = DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)

                If Not dt Is Nothing And dt.Rows.Count > 0 Then
                    repEvents.DataSource = dt
                    repEvents.DataBind()
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        'Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        '    AddHandler repEvents.ItemCommand, AddressOf repEvents_ItemCommand
        'End Sub


        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            SetProperties()
            Me.SetControlRecordIDFromParam()
            If Not IsPostBack Then
                LoadSchedule()
            End If


        End Sub




        Protected Sub repEvents_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles repEvents.ItemDataBound
            If (e.Item.ItemType = ListItemType.Item) OrElse (e.Item.ItemType = ListItemType.AlternatingItem) Then
                With CType(e.Item.FindControl("lnkEventName"), HyperLink)
                    'DirectCast(grdMeetings.Columns(0), HyperLinkColumn).DataNavigateUrlFormatString = MeetingPage & "?ID={0:F0}"
                    .NavigateUrl = String.Format(MeetingPage & "?ID={0:F0}", DataBinder.Eval(e.Item.DataItem, "ProductID").ToString)

                    If DataBinder.Eval(e.Item.DataItem, "WebName") IsNot Nothing AndAlso Not IsDBNull(DataBinder.Eval(e.Item.DataItem, "WebName")) Then
                        If DataBinder.Eval(e.Item.DataItem, "WebName").ToString.Length < 30 Then
                            .Text = DataBinder.Eval(e.Item.DataItem, "WebName").ToString
                        Else
                            .Text = DataBinder.Eval(e.Item.DataItem, "WebName").ToString.Substring(0, 30) & "..."
                        End If
                    End If
                End With

                If DataBinder.Eval(e.Item.DataItem, "WebImage") IsNot Nothing AndAlso Not IsDBNull(DataBinder.Eval(e.Item.DataItem, "WebImage")) AndAlso DataBinder.Eval(e.Item.DataItem, "WebImage") IsNot String.Empty Then
                    Dim img As Image = CType(e.Item.FindControl("EventImage"), Image)
                    If img IsNot Nothing Then
                        img.ImageUrl = CStr(DataBinder.Eval(e.Item.DataItem, "WebImage"))
                    End If
                Else
                    e.Item.FindControl("trEventImage").Visible = False
                End If
                Dim lbl As New label
                If e.Item.FindControl("lblDate") IsNot Nothing AndAlso DataBinder.Eval(e.Item.DataItem, "StartDate") IsNot Nothing AndAlso Not IsDBNull(DataBinder.Eval(e.Item.DataItem, "StartDate")) AndAlso DataBinder.Eval(e.Item.DataItem, "EndDate") IsNot Nothing AndAlso Not IsDBNull(DataBinder.Eval(e.Item.DataItem, "EndDate")) Then
                    lbl = CType(e.Item.FindControl("lblDate"), Label)
                    Dim strStartDate As String = DataBinder.Eval(e.Item.DataItem, "StartDate", "{0:MMM dd, yyyy}")
                    Dim strEndate As String = DataBinder.Eval(e.Item.DataItem, "EndDate", "{0:MMM dd, yyyy}")
                    If strStartDate = strEndate Then
                        lbl.Text = DataBinder.Eval(e.Item.DataItem, "StartDate", "{0:MMM dd, yyyy}")
                    Else
                        lbl.Text = DataBinder.Eval(e.Item.DataItem, "StartDate", "{0:MMM dd, yyyy}") + " - " + DataBinder.Eval(e.Item.DataItem, "EndDate", "{0:MMM dd, yyyy}")
                    End If

                End If

                lbl = CType(e.Item.FindControl("lblPlace"), Label)

                If DataBinder.Eval(e.Item.DataItem, "Location") IsNot Nothing AndAlso Not IsDBNull(DataBinder.Eval(e.Item.DataItem, "Location")) AndAlso DataBinder.Eval(e.Item.DataItem, "Location") IsNot String.Empty Then
                    If lbl IsNot Nothing Then
                        lbl.Text = CStr(DataBinder.Eval(e.Item.DataItem, "Location"))
                    End If
                End If
                If DataBinder.Eval(e.Item.DataItem, "WebDescription") IsNot Nothing AndAlso Not IsDBNull(DataBinder.Eval(e.Item.DataItem, "WebDescription")) AndAlso DataBinder.Eval(e.Item.DataItem, "WebDescription") IsNot String.Empty Then
                    Dim ltr As Literal = CType(e.Item.FindControl("ltrdescription"), Literal)
                    If ltr IsNot Nothing Then
                        ltr.Text = CStr(DataBinder.Eval(e.Item.DataItem, "WebDescription"))
                    End If
                Else
                    e.Item.FindControl("trEventdesc").Visible = False
                End If

            End If
            If (e.Item.ItemType = ListItemType.Footer) Then
                With CType(e.Item.FindControl("linkViewAll"), HyperLink)
                    .NavigateUrl = MeetingAllPage
                End With
            End If


        End Sub

    End Class

End Namespace
