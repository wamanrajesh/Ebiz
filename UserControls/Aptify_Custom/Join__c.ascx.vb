
Option Explicit On
Option Strict On
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.Application
Imports System.Data
Imports Telerik.Web.UI


Namespace Aptify.Framework.Web.eBusiness.ProductCatalog
    Partial Class Join__c
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "Join__c"
        Protected Const ATTRIBUTE_PROFILE_PAGE As String = "ProfilePage"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
            SetProperties()
            If Not IsPostBack Then
                LoadGrid()
            End If
        End Sub

        Public Function GetGridRowCount() As Long
            GetGridRowCount = gridMain.Items.Count
        End Function

        Public Sub LoadGrid()
            Dim sSQL As String
            Dim dt As System.Data.DataTable
            Try

                sSQL = "spGetDuesProducts__c"
                dt = DataAction.GetDataTable(sSQL)

                If dt IsNot Nothing Then
                    If dt.Rows.Count > 0 Then

                        gridMain.DataSource = dt
                        gridMain.DataBind()

                    Else
                        gridMain.Visible = False

                    End If
                End If

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Public Overridable Property ProfilePage() As String
            Get
                If Not ViewState(ATTRIBUTE_PROFILE_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_PROFILE_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_PROFILE_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties

            MyBase.SetProperties()

            If String.IsNullOrEmpty(ProfilePage) Then
                'since value is the 'default' check the XML file for possible custom setting
                ProfilePage = Me.GetLinkValueFromXML(ATTRIBUTE_PROFILE_PAGE)
            End If

        End Sub

        Protected Sub btnContinue_Click(sender As Object, e As EventArgs)

            Dim strProductID As String = ""
            Dim strCampaignCode As String

            If gridMain.Items.Count > 0 Then
                For Each item As GridDataItem In gridMain.MasterTableView.Items
                    Dim result As Boolean = DirectCast(item.FindControl("radioButton"), RadioButton).Checked
                    If result Then
                        strProductID = item("ID").Text
                    End If
                Next
            End If

            strCampaignCode = txtCampaignCode.Text

            Response.Redirect(ProfilePage)

        End Sub
    End Class
End Namespace
