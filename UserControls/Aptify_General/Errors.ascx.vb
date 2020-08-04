'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports System.Data
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Framework.ExceptionManagement

Namespace Aptify.Framework.Web.eBusiness

    Partial Class ErrorsPageControl
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "Errors"

        Protected Overrides Sub SetProperties()
            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()
        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()

            'Dim dt As New DataTable()
            'Dim oSessionException As SessionException
            'Try

            '    dt.Columns.Add("DateTime", GetType(Date))
            '    dt.Columns.Add("Description", GetType(String))
            '    dt.Columns.Add("Source", GetType(String))
            '    dt.Columns.Add("SessionException", GetType(Object))

            '    Try
            '        For i As Integer = ExceptionManager.SessionExceptions.Count - 1 To 0 Step -1
            '            oSessionException = ExceptionManager.SessionExceptions.Item(i)

            '            If oSessionException IsNot Nothing AndAlso oSessionException.Exception IsNot Nothing Then
            '                With dt.Rows.Add
            '                    .Item("DateTime") = oSessionException.TimeStamp
            '                    .Item("Description") = oSessionException.Exception.Message
            '                    .Item("Source") = oSessionException.Exception.Source
            '                    .Item("SessionException") = oSessionException
            '                End With
            '            End If
            '        Next
            '    Catch ex As Exception
            '        With dt.Rows.Add
            '            .Item("DateTime") = Date.Now
            '            .Item("Description") = "An error occurred while loading the Exceptions Grid.  Results may not be complete."
            '            .Item("Source") = "ErrorsPageControl"
            '        End With
            '    End Try

            '    grdErrors.DataSource = dt
            '    grdErrors.DataBind()

            'Catch ex As Exception
            '    Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            'End Try
            LoadgrdErrors()
        End Sub
        ''' <summary>
        ''' Nalini 12436 date:1/12/2011
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub LoadgrdErrors()
            Dim dt As New DataTable()
            Dim oSessionException As SessionException
            Try

                dt.Columns.Add("DateTime", GetType(Date))
                dt.Columns.Add("Description", GetType(String))
                dt.Columns.Add("Source", GetType(String))
                dt.Columns.Add("SessionException", GetType(Object))

                Try
                    For i As Integer = ExceptionManager.SessionExceptions.Count - 1 To 0 Step -1
                        oSessionException = ExceptionManager.SessionExceptions.Item(i)

                        If oSessionException IsNot Nothing AndAlso oSessionException.Exception IsNot Nothing Then
                            With dt.Rows.Add
                                .Item("DateTime") = oSessionException.TimeStamp
                                .Item("Description") = oSessionException.Exception.Message
                                .Item("Source") = oSessionException.Exception.Source
                                .Item("SessionException") = oSessionException
                            End With
                        End If
                    Next
                Catch ex As Exception
                    With dt.Rows.Add
                        .Item("DateTime") = Date.Now
                        .Item("Description") = "An error occurred while loading the Exceptions Grid.  Results may not be complete."
                        .Item("Source") = "ErrorsPageControl"
                    End With
                End Try

                grdErrors.DataSource = dt
                grdErrors.DataBind()

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        ''' <summary>
        ''' Nalini 12436 date:1/12/2011
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub grdErrors_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdErrors.PageIndexChanging
            grdErrors.PageIndex = e.NewPageIndex
            LoadgrdErrors()
        End Sub
    End Class
End Namespace

