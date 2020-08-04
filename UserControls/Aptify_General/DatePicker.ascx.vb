'Aptify e-Business 5.5.1, July 2013
Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Globalization
Imports System.Text
Imports System.Web
Imports System.Web.SessionState
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls

' ''User Control changed by RashmiP, Date 7/19/2011, For more information, please refer below link
' ''http//aspnetajax.componentart.com/control-specific/calendar/features/picker_calendarUserControl/WebForm1.aspx


Partial Class DatePicker
    Inherits System.Web.UI.UserControl
    Private controlMappings As New Hashtable()

    Private Function GetControlMappingsScript() As String
        Dim sb As New StringBuilder()
        sb.AppendLine("<script type=""text/javascript"">")
        sb.AppendLine("if (!window.controlMappings) window.controlMappings = {};")
        For Each keyvalue As DictionaryEntry In Me.controlMappings
            Dim key As String = DirectCast(keyvalue.Key, String)
            Dim value As String = DirectCast(keyvalue.Value, String)
            sb.Append("window.controlMappings.").Append(key).Append(" = '").Append(value).Append("';" & vbLf)
        Next
        sb.AppendLine("</script>")
        Return sb.ToString()
    End Function

    Public Property SelectedDate() As DateTime
        Get
            Return dtpPicker.SelectedDate
        End Get
        Set(ByVal value As DateTime)
            dtpPicker.SelectedDate = value
        End Set
    End Property
    Private Sub Page_Load(sender As Object, e As EventArgs)
        If Not Page.IsPostBack Then
            'dtpCalendar.SelectedDate = InlineAssignHelper(dtpPicker.SelectedDate, DateTime.Today)
        End If
        'Me.controlMappings(Me.dtpCalendar.ClientId) = Me.dtpPicker.ClientId
        'Me.controlMappings(Me.dtpPicker.ClientId) = Me.dtpCalendar.ClientId
        'Me.controlMappings(Me.Td1.ClientID) = Me.dtpCalendar.ClientId
        'Me.controlMappings(Me.imgCalendar.ClientID) = Me.dtpCalendar.ClientId
        'Me.ControlMappingScriptSpot.Text = Me.GetControlMappingsScript()
    End Sub

#Region "Web Form Designer generated code"
    Protected Overrides Sub OnInit(e As EventArgs)
        '
        ' CODEGEN: This call is required by the ASP.NET Web Form Designer.
        '
        InitializeComponent()
        MyBase.OnInit(e)
    End Sub

    ''' <summary>
    ''' Required method for Designer support - do not modify
    ''' the contents of this method with the code editor.
    ''' </summary>
    Private Sub InitializeComponent()
        AddHandler Me.Load, New System.EventHandler(AddressOf Me.Page_Load)
    End Sub
    Private Shared Function InlineAssignHelper(Of T)(ByRef target As t, value As t) As t
        target = value
        Return value
    End Function
#End Region

End Class
