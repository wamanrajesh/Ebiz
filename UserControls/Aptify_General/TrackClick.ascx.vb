'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports Aptify.Framework.Web.eBusiness
Imports System.Data

Namespace Aptify.Framework.Web.eBusiness
    Partial Class TrackClick
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_SITE_HOME_PAGE As String = "HomePage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "TrackClick"


#Region "TrackClick Specific Properties"
        ''' <summary>
        ''' Home page url
        ''' </summary>
        Public Overridable Property HomePage() As String
            Get
                If Not ViewState(ATTRIBUTE_SITE_HOME_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_SITE_HOME_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_SITE_HOME_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
#End Region

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(HomePage) Then
                'since value is the 'default' check the XML file for possible custom setting
                HomePage = Me.GetLinkValueFromXML(ATTRIBUTE_SITE_HOME_PAGE)
                If String.IsNullOrEmpty(HomePage) Then
                    'if no value provided then redirect to appication root
                    HomePage = Request.ApplicationPath
                End If
            End If
        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
            Try
                If Not Me.IsPageInAdmin() Then

                    Dim sWebUserID As String = ""
                    Dim sSource As String = ""
                    Dim sSQL As String
                    Dim lPersonID As Long = -1
                    Dim sRedirect As String = ""
                    Dim sEmail As String
                    Dim lWebUserID As Long = -1

                    If Request.QueryString("Source") IsNot Nothing Then
                        sSource = Trim(Request.QueryString("Source"))
                    ElseIf Session("Source") IsNot Nothing Then
                        sSource = Session("Source").ToString
                    End If

                    If Request.QueryString("Email") IsNot Nothing AndAlso Len(Request.QueryString("Email")) > 0 Then
                        sEmail = CStr(Request.QueryString("Email"))
                        Dim oParams(0) As System.Data.IDataParameter
                        oParams(0) = Me.DataAction.GetDataParameter("@Email", Data.SqlDbType.NVarChar, 100, sEmail.Trim)
                        If Len(sEmail) > 0 Then
                            Dim dt As DataTable
                            sSQL = "SELECT ID FROM " & AptifyApplication.GetEntityBaseDatabase("Persons") & "..vwPersons WHERE Email = @Email ORDER BY ID ASC"
                            dt = Me.DataAction.GetDataTableParametrized(sSQL, Data.CommandType.Text, oParams)
                            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                                lPersonID = CLng(dt.Rows(0).Item("ID"))
                            End If
                        End If
                    End If

                    If Request.QueryString("WebUserID") IsNot Nothing Then
                        sWebUserID = Trim(CStr(Request.QueryString("WebUserID")))
                        If Len(sWebUserID) > 0 Then
                            Dim dt As DataTable
                            Dim oParams(0) As System.Data.IDataParameter
                            oParams(0) = Me.DataAction.GetDataParameter("@WebUserID", Data.SqlDbType.NVarChar, 100, sWebUserID.Trim)

                            sSQL = "SELECT ID,LinkID FROM " & AptifyApplication.GetEntityBaseDatabase("WebUsers") & "..vwWebUsers WHERE UserID = @WebUserID"
                            dt = MyBase.DataAction.GetDataTableParametrized(sSQL, Data.CommandType.Text, oParams)
                            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                                lPersonID = CLng(dt.Rows(0).Item("LinkID"))
                                lWebUserID = CLng(dt.Rows(0).Item("ID"))
                            End If
                        End If
                    End If

                    If Request.QueryString("PersonID") IsNot Nothing AndAlso CLng(Request.QueryString("PersonID")) > 0 Then
                        lPersonID = CLng(Request.QueryString("PersonID"))
                    End If

                    If Request.QueryString("Redirect") IsNot Nothing Then
                        sRedirect = CStr(Request.QueryString("Redirect"))
                    ElseIf Session("Redirect") IsNot Nothing Then
                        sRedirect = Session("Redirect").ToString
                    End If

                    If Len(sSource) > 0 OrElse lPersonID > 0 OrElse Len(sWebUserID) > 0 Then '01/22/08 Code changed for 5223, for only logging in the clicks from other source.
                        Dim oParams(5) As IDataParameter
                        oParams(0) = Me.DataAction.GetDataParameter("@ID", Data.SqlDbType.Int)
                        oParams(0).Direction = ParameterDirection.Output

                        oParams(1) = Me.DataAction.GetDataParameter("@Date", Data.SqlDbType.DateTime, DateTime.Now)
                        oParams(2) = Me.DataAction.GetDataParameter("@Source", Data.SqlDbType.NVarChar, 100, sSource)
                        oParams(3) = Me.DataAction.GetDataParameter("@PersonID", Data.SqlDbType.Int, lPersonID)
                        oParams(4) = Me.DataAction.GetDataParameter("@WebUserID", Data.SqlDbType.NVarChar, 100, sWebUserID)
                        oParams(5) = Me.DataAction.GetDataParameter("@RedirectURL", Data.SqlDbType.NVarChar, -1, sRedirect)

                        'Suvarna Issue 12351 12/01/2011 Commented and added for Dynamic DB Name chage
                        ''sSQL = "APTIFY..spCreateWebClickthrough" 
                        sSQL = AptifyApplication.GetEntityBaseDatabase("WebClickthrough") & "..spCreateWebClickthrough"

                        ' Track the click-through person id so that we can auto-fill forms for people
                        If Len(sEmail) > 0 Then
                            Session("ClickThroughEmail") = sEmail
                        End If
                        DataAction.ExecuteNonQueryParametrized(sSQL, CommandType.StoredProcedure, oParams)
                    End If
                    If Len(sSource) >= 3 Then
                        Select Case sSource.Trim.Substring(0, 2)
                            Case "IT", "MM", "PB", "EV", "SM", "EX"
                                Session("ClickThroughSource") = "Aptify Email"
                            Case Else
                                Session("ClickThroughSource") = sSource.Trim
                        End Select
                    End If

                    If Len(sRedirect) > 0 Then
                        Response.Redirect(sRedirect, True)
                    Else
                        Response.Redirect(HomePage, True)
                    End If
                End If
            Catch ex2 As System.Threading.ThreadAbortException
                'Expected exception. This is thrown due to the Redirect. Do Nothing on this exception.
            Catch ex1 As Aptify.Framework.DataServices.CheckConstraintViolation
                'This exception may have been thrown if one of the parameters used to create the WebClickThrough violate a constraint
                'Redirect to homepage
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex1)
                Response.Redirect(HomePage, True)
            Catch ex As Exception
                'Some other unexpected exception has occurred. Publish error and redirect to homepage.
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Response.Redirect(HomePage, True)
            End Try
        End Sub
    End Class

End Namespace
