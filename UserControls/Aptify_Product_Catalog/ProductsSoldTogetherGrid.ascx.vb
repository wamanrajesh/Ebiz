'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.Application

Namespace Aptify.Framework.Web.eBusiness.ProductCatalog
    Partial Class ProductsSoldTogether
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "ProductsSoldTogetherGrid"

        Protected Overrides Sub SetProperties()
            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()
        End Sub

        Public Sub LoadGrid(ByVal ProductID As Long)
            Dim sSQL As String
            Dim dt As System.Data.DataTable

            Try
                sSQL = "Execute Aptify.dbo.spGetProductsSoldTogether " & ProductID.ToString()
                dt = DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)

                If dt.Rows.Count > 0 Then
                    DataList1.DataSource = dt
                    DataList1.DataBind()

                Else
                    lblError.Text = "dt has no rows"
                    lblError.Visible = True
                    'Me.Visible = False
                End If
            Catch ex As Exception
                lblError.Text = "error exec: " & ex.Message
                lblError.Visible = True

                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Public Property NavigateURLFormatField() As String
            Get
                Dim o As Object
                o = ViewState.Item("NavigateURLFormatField")
                If o Is Nothing Then
                    Return ""
                Else
                    Return CStr(o) & "?ID={0}"
                End If
            End Get
            Set(ByVal Value As String)
                ViewState.Add("NavigateURLFormatField", Value)
            End Set
        End Property

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()

            If Not IsPostBack Then
                If Me.SetControlRecordIDFromParam() Then
                    LoadGrid(Me.ControlRecordID)
                Else
                    lblError.Text = "set didn't work"
                    lblError.Visible = True
                End If

            End If

        End Sub


    End Class
End Namespace
