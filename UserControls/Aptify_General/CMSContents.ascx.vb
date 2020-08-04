'Aptify e-Business 5.5.1, July 2013
Imports System.Xml
Imports Microsoft.VisualBasic
Imports System.Data
Imports Aptify.Framework.Web.eBusiness
Imports Aptify.Framework.Integration
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports System.Diagnostics


Namespace Aptify.Framework.Web.eBusiness
    Partial Class CMSContents
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "CMSContents"

        Protected Overrides Sub SetProperties()
            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()
        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
        End Sub

#Region "Constants"
        Private Const CNST_ENITYNAMECONTENTMANAGEMENTSYSTEMS = "Content Management Systems"
#End Region

#Region "Private Variables"

        Private m_oApplication As Aptify.Framework.Application.AptifyApplication
        Private m_lngCMSSystemID As Long = 1
        Private m_Interval As Integer
        Private m_DisplayCount As Integer = 1
        Private m_CMSFolders As String = ""
        Private m_CMSSubFolder As Boolean = False
        Private m_UserName As String
#End Region

#Region "Public Properties"

        Public Property ApplicationObject() As Aptify.Framework.Application.AptifyApplication
            Get
                Return m_oApplication
            End Get
            Set(ByVal value As Aptify.Framework.Application.AptifyApplication)
                m_oApplication = value
            End Set
        End Property

        Public Property CMSSystemID() As Long
            Get
                Return m_lngCMSSystemID
            End Get
            Set(ByVal value As Long)
                m_lngCMSSystemID = value
            End Set
        End Property

        Public Property Interval() As Integer
            Get
                Return m_Interval
            End Get
            Set(ByVal value As Integer)
                m_Interval = value
            End Set
        End Property

        Public Property DisplayCount() As Integer
            Get
                Return m_DisplayCount
            End Get
            Set(ByVal value As Integer)
                m_DisplayCount = value
            End Set
        End Property

        ''' <summary>
        ''' This property will get the list of Folders (Coma seperted) 
        ''' to allow contents to be shown on E-Business.
        ''' </summary>
        Public Property CMSFolders() As String
            Get
                Return m_CMSFolders
            End Get
            Set(ByVal value As String)
                m_CMSFolders = value
            End Set
        End Property

        Public Property CMSSubFolder() As Boolean
            Get
                Return m_CMSSubFolder
            End Get
            Set(ByVal value As Boolean)
                m_CMSSubFolder = value
            End Set
        End Property

#End Region


        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            Dim oCMSIntegration As New CMSIntegrationSiteFinity

            Dim cmsSystemGE As AptifyGenericEntity
            'Dim strLines() As String

            If m_oApplication IsNot Nothing Then
                Try
                    'HP Issue#7812: replace all AptifyApplication references to that provided by EBusinessGlobal
                    'cmsSystemGE = m_oApplication.GetEntityObject(CNST_ENITYNAMECONTENTMANAGEMENTSYSTEMS, m_lngCMSSystemID)
                    cmsSystemGE = Me.AptifyApplication.GetEntityObject(CNST_ENITYNAMECONTENTMANAGEMENTSYSTEMS, m_lngCMSSystemID)
                    oCMSIntegration = TryCast(Aptify.Framework.Application.InstanceCreator.CreateInstanceFromName(cmsSystemGE.GetValue("CMSAssembly").ToString, cmsSystemGE.GetValue("CMSClass").ToString), CMSIntegrationInterface)
                    oCMSIntegration.Config(m_oApplication, m_lngCMSSystemID)

                    'Dim lngTopicCodeId As Long
                    Dim sJavaScript As String = ""
                    Dim dtContents As DataTable

                    m_UserName = GetUserObject.WebUserStringID

                    If m_UserName.Trim.Length <= 0 Then
                        lblContents.Visible = False
                    Else
                        Me.lblContents.Visible = True

                        'GetContentInfo returns a datatable containing two columns: TaxonomyID & ContentBlock (HTML)
                        'we are only concerned with the HTML that is returned
                        dtContents = oCMSIntegration.GetContentInfo(m_UserName, m_CMSFolders, m_CMSSubFolder)

                        '2008/02/13 MAS
                        If dtContents IsNot Nothing AndAlso dtContents.Rows IsNot Nothing Then
                            'If the DisplayCount property is not set, then all qualifying content will be displayed
                            If m_DisplayCount <= 0 Then
                                m_DisplayCount = dtContents.Rows.Count
                            End If

                            'There are two approaches to providing content for the CMS User Control. 
                            '  1) Random Static = content display is random, but does not change once on page until next refresh (interval=0)
                            '  2) Random Dynamic = content rotates on page - set by Interval property (interval > 0)
                            '  3) [NOT SUPPORTED] Ordered Static = content display is ordered by specified field

                            If Me.m_Interval = 0 Then
                                AddContent(dtContents)
                            Else
                                AddIntervalContent(dtContents)
                            End If
                        End If
                    End If
                Catch ex As Exception
                    Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                End Try
            End If
        End Sub

        Private Sub AddContent(ByVal dtContents As DataTable)
            Try
                Dim strTempContent As String = ""
                'Content will be static during viewing of page, only limited set of content needs to be provided
                If dtContents IsNot Nothing AndAlso dtContents.Rows.Count > 0 Then
                    'Add the required number of content items in a random order

                    If dtContents.Rows.Count < Me.DisplayCount Then
                        DisplayCount = dtContents.Rows.Count
                    End If

                    'The following logic ensures that content is not displayed more than once
                    Dim r As New Random(Now.Millisecond)
                    Dim arrContent(DisplayCount - 1) As Integer
                    Dim i As Integer = 0
                    'fill array with -1
                    For i = 0 To DisplayCount - 1
                        arrContent(i) = -1
                    Next
                    i = 0 'reset variable
                    Dim temp As Integer
                    While i < DisplayCount
                        temp = r.Next(dtContents.Rows.Count)
                        Dim bFound As Boolean = False
                        For Each index As Integer In arrContent
                            If index = temp Then
                                bFound = True
                                Exit For
                            End If
                        Next
                        If Not bFound Then
                            arrContent(i) = temp
                            i += 1
                        End If
                    End While

                    'Add the contents
                    For i = 0 To Me.m_DisplayCount - 1
                        strTempContent &= dtContents.Rows(arrContent(i)).Item("Content").ToString
                    Next
                Else
                    strTempContent = "<br/>"
                End If
                strTempContent = TranslateStringToJavaScript(strTempContent)

                Dim sbrJavaScript As New StringBuilder
                sbrJavaScript.Append("<script language=""JavaScript"">" + vbCrLf)
                sbrJavaScript.Append(" function " + Me.ClientID + "displayContent() { " + vbCrLf)
                sbrJavaScript.Append("     if (document.getElementById('" + lblContents.ClientID + "')!=null) { " + vbCrLf)
                sbrJavaScript.Append("             document.getElementById('" + lblContents.ClientID + "').innerHTML='" + strTempContent + "';" + vbCrLf)
                sbrJavaScript.Append("     }" + vbCrLf)
                sbrJavaScript.Append(" } " + vbCrLf)
                sbrJavaScript.Append(Me.ClientID + "displayContent();" + vbCrLf)
                sbrJavaScript.Append("</script>" + vbCrLf)
                Page.ClientScript.RegisterStartupScript(Me.GetType, Me.ClientID + "JSMain", sbrJavaScript.ToString)
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub AddIntervalContent(ByVal dtContents As DataTable)
            Try
                Dim strTempContent As String = ""
                Dim sbrJavaScript As New StringBuilder
                Dim intContentCount As Integer = 0

                sbrJavaScript.Append("<script language=""JavaScript"">" + vbCrLf)
                sbrJavaScript.Append(Me.ClientID + "Content_list = new Array();" + vbCrLf)
                'Content will rotate during viewing of page, so all content must be sent
                Dim intArrayCount As Integer = 0
                If dtContents.Rows.Count > 0 Then
                    For Each drContents As DataRow In dtContents.Rows
                        strTempContent = drContents("Content").ToString
                        strTempContent = TranslateStringToJavaScript(strTempContent)
                        sbrJavaScript.Append(Me.ClientID + "Content_list[" + intArrayCount.ToString + "]='" + strTempContent + "';" + vbCrLf)
                        intArrayCount += 1
                    Next
                Else
                    sbrJavaScript.Append(" var " + Me.ClientID + "Content_list = [];" + vbCrLf)
                    sbrJavaScript.Append(Me.ClientID + "Content_list[" + intArrayCount.ToString + "]='';" + vbCrLf)
                End If
                sbrJavaScript.Append(" var " + Me.ClientID + "number_of_Content = " + intArrayCount.ToString + ";" + vbCrLf)
                sbrJavaScript.Append(Me.GetRandomDisplayJavaScript)
                sbrJavaScript.Append("</script>" + vbCrLf)

                Page.ClientScript.RegisterStartupScript(Me.GetType, Me.ClientID + "JSMain", sbrJavaScript.ToString)
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub


        ''' <summary>
        ''' This method returns an instance of the Aptify e-Business User Object
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Overridable Function GetUserObject() As User
            Dim oUser As New User
            Try
                oUser.LoadValuesFromSessionObject(Page.Session)
                Return oUser
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return Nothing
            End Try
        End Function

        Public Function GetRandomDisplayJavaScript() As String
            Dim sbrJavaScript_New As New StringBuilder
            Try
                sbrJavaScript_New.Append(" function " + Me.ClientID + "getRandomContent() { " + vbCrLf)
                sbrJavaScript_New.Append("    var arrIndexes = []; //array of Indexes that will be used for data to be displayed" + vbCrLf)
                sbrJavaScript_New.Append("    var sHTML='';" + vbCrLf)
                sbrJavaScript_New.Append("    var iDisplayCount=" & Me.m_DisplayCount.ToString & ";" + vbCrLf + vbCrLf)
                sbrJavaScript_New.Append(" if (" + Me.ClientID + "Content_list != null) {" + vbCrLf)
                sbrJavaScript_New.Append("    var iArrayLength=" & Me.ClientID & "Content_list.length;" + vbCrLf + vbCrLf)
                sbrJavaScript_New.Append("    if (iArrayLength > 0) {" + vbCrLf)
                sbrJavaScript_New.Append("        if (iArrayLength < iDisplayCount) {" + vbCrLf)
                sbrJavaScript_New.Append("            iDisplayCount = iArrayLength;" + vbCrLf)
                sbrJavaScript_New.Append("        }" + vbCrLf)
                sbrJavaScript_New.Append("        var j;" + vbCrLf)
                sbrJavaScript_New.Append("        for (j=0;j<iDisplayCount;j++) {" + vbCrLf)
                sbrJavaScript_New.Append("            // loop until we have selected the number of items to display" + vbCrLf)
                sbrJavaScript_New.Append("            var nextElem;" + vbCrLf)
                sbrJavaScript_New.Append("            var found;" + vbCrLf)
                sbrJavaScript_New.Append("            do {" + vbCrLf)
                sbrJavaScript_New.Append("                found=0;" + vbCrLf)
                sbrJavaScript_New.Append("                nextElem = Math.floor(Math.random()*iArrayLength);" + vbCrLf)
                sbrJavaScript_New.Append("                var k;" + vbCrLf)
                sbrJavaScript_New.Append("                for (k=0;k<arrIndexes.length;k++) {" + vbCrLf)
                sbrJavaScript_New.Append("                    if (arrIndexes[k] == nextElem) {" + vbCrLf)
                sbrJavaScript_New.Append("                        found=1;" + vbCrLf)
                sbrJavaScript_New.Append("                        break;" + vbCrLf)
                sbrJavaScript_New.Append("                    }" + vbCrLf)
                sbrJavaScript_New.Append("                }" + vbCrLf)
                sbrJavaScript_New.Append("            }" + vbCrLf)
                sbrJavaScript_New.Append("            while (found==1)" + vbCrLf)
                sbrJavaScript_New.Append("            arrIndexes[j] = nextElem;" + vbCrLf)
                sbrJavaScript_New.Append("        }" + vbCrLf)
                sbrJavaScript_New.Append("        var m;" + vbCrLf)
                sbrJavaScript_New.Append("        for (m=0;m<arrIndexes.length;m++) {" + vbCrLf)
                sbrJavaScript_New.Append("            sHTML += " + Me.ClientID + "Content_list[arrIndexes[m]];" + vbCrLf)
                sbrJavaScript_New.Append("        }" + vbCrLf)
                sbrJavaScript_New.Append("    }" + vbCrLf)
                sbrJavaScript_New.Append(" }" + vbCrLf)
                sbrJavaScript_New.Append(" return sHTML;" + vbCrLf)
                sbrJavaScript_New.Append("}" + vbCrLf + vbCrLf)

                sbrJavaScript_New.Append(" function " + Me.ClientID + "displayContent() { " + vbCrLf)
                sbrJavaScript_New.Append("     var new_Content;" + vbCrLf)
                sbrJavaScript_New.Append("     if (" + Me.ClientID + "Content_list != null) { " + vbCrLf)
                sbrJavaScript_New.Append("         new_Content = " + Me.ClientID + "getRandomContent();" + vbCrLf)
                sbrJavaScript_New.Append("         if (document.getElementById('" + lblContents.ClientID + "')!=null) { " + vbCrLf)
                sbrJavaScript_New.Append("             if (new_Content != null) { " + vbCrLf)
                sbrJavaScript_New.Append("                 document.getElementById('" + lblContents.ClientID + "').innerHTML= new_Content;" + vbCrLf)
                sbrJavaScript_New.Append("             }" + vbCrLf)
                sbrJavaScript_New.Append("         }" + vbCrLf)
                sbrJavaScript_New.Append("     }" + vbCrLf)
                sbrJavaScript_New.Append("     var recur_call = '" + Me.ClientID + "displayContent()';" + vbCrLf)
                sbrJavaScript_New.Append("     setTimeout(recur_call, " + m_Interval.ToString + ");" + vbCrLf)
                sbrJavaScript_New.Append(" } ")
                sbrJavaScript_New.Append(Me.ClientID + "displayContent();")

                Return sbrJavaScript_New.ToString
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                'Return Nothing
            End Try
        End Function


        Protected Function TranslateStringToJavaScript(ByVal sMessage As String) As String
            Try
                sMessage = sMessage.Replace("\", "\\") 'catches all \, even if the \n is included.  this will need to be fixed
                sMessage = sMessage.Replace("'", "\'")
                sMessage = sMessage.Replace("""", "\""")
                sMessage = sMessage.Replace(System.Environment.NewLine, "\n")
                sMessage = sMessage.Replace(vbTab, "\t")
                sMessage = sMessage.Replace(Chr(10), " ")
                Return sMessage
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Function
    End Class
End Namespace