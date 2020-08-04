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
    Partial Class SubmitAbstract
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "SubmitAbstract"


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
                If Me.AptifyEbusinessUser1.PersonID > 0 Then
                    lblMessage.Visible = False
                    LoadCombo()
                    Me.LoadDataFromGE(Me, Me.AptifyApplication.GetEntityObject("Abstracts", -1))
                Else
                    Response.Redirect(Me.GetSecurityErrorPageFromXML & "?Message=" & Server.UrlEncode("You must be logged in to access this page"))
                End If

            End If
        End Sub

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(Me.RedirectURL) Then
                Me.cmdSubmit.Enabled = False
                Me.cmdSubmit.ToolTip = "RedirectURL property has not been set."
            End If

        End Sub

        Private Sub LoadCombo()
            ' This method sets the SQL for the combo dynamically since it changes based on the ID parameter
            Dim sDatabase As String = AptifyApplication.GetEntityBaseDatabase("Abstract Categories")
            Dim sSQL As String
            Dim sID As String

            sID = Request.QueryString("ID")
            If sID Is Nothing Then
                sSQL = "SELECT ID,Name FROM " & _
                       sDatabase & _
                       "..vwAbstractCategories ORDER BY Name"
            ElseIf IsNumeric(Request.QueryString("ID")) Then
                sSQL = "SELECT ID,Name FROM " & _
                       sDatabase & _
                       "..vwAbstractCategories WHERE ParentID=" & _
                       sID & " ORDER BY Name"
            Else
                Throw New ArgumentException("Parameter must be numeric.", "ID")
            End If
            Me.cmbCategory.Attributes.Add("AptifyListSQL", sSQL)
        End Sub


        Protected Sub cmdSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
            Dim oGE As AptifyGenericEntityBase = Nothing
            Dim bRedirect As Boolean = False

            Try
                oGE = AptifyApplication.GetEntityObject("Abstracts", -1)

                ' transfer the data to the object...
                TransferDataToGE(Me, oGE)

                oGE.SetValue("SubmittedByID", Me.AptifyEbusinessUser1.PersonID)
                oGE.SetValue("DateSubmitted", Date.Now)

                If oGE.Save(False) Then
                    bRedirect = True
                Else
                    'Throw New Exception("Unable to save the Abstract: <br/><br/>" & oGE.LastError)
                    Throw New Exception(oGE.LastError)
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)

                lblMessage.ForeColor = System.Drawing.Color.Red
                lblMessage.Visible = True
                lblMessage.Text = "Unable to save the Abstract: <br/><br/>" & Environment.NewLine & _
                                  ex.Message
            Finally
                'CP Redirect outside Try/Catch
                If bRedirect AndAlso oGE IsNot Nothing Then
                    RedirectUsingPropertyValues(oGE.RecordID)
                End If
            End Try
        End Sub

    End Class

End Namespace
