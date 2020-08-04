'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Framework.Application
Imports Aptify.Framework.BusinessLogic
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.AttributeManagement
Imports System.Data

Namespace Aptify.Framework.Web.eBusiness.AbstractManagement
    Partial Class ReviewSingleAbstract
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "ReviewSingleAbstract"


#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()
        End Sub

#End Region


        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
            If Not IsPostBack Then
                LoadData()
            End If
        End Sub

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            'if values are not provide directly or from the XML file, set default values for inherited properties since 
            'control requires them to properly function
            If String.IsNullOrEmpty(Me.QueryStringRecordIDParameter) Then Me.QueryStringRecordIDParameter = "ReportID"

        End Sub

        Private Sub LoadData()
            Dim bIsError As Boolean = False

            Try
                ' Changes made to get the query string name from a property set by CMS
                ' Changes made by CP 7/14/2008
                If Me.SetControlRecordIDFromParam() Then
                    Dim oGE As AptifyGenericEntityBase
                    oGE = Me.AptifyApplication.GetEntityObject("Abstracts", Me.ControlRecordID)
                    LoadDataFromGE(oGE)
                ElseIf Request.QueryString(Me.QueryStringRecordIDParameter) Is Nothing Then
                    lblError.Text = "Invalid Record ID"
                    lblError.Visible = True
                Else
                    bIsError = True
                End If

            Catch ex As Exception
                bIsError = True
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try

            If bIsError Then
                Response.Redirect(Me.GetSecurityErrorPageFromXML & "?Message=" & Server.UrlEncode("Absract not available, possible error in Event Log."))
            End If
        End Sub
    End Class
End Namespace