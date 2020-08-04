'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports Aptify.Framework.Web.eBusiness
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports System.Data

Namespace Aptify.Framework.Web.eBusiness.ProductCatalog
    Partial Class SaveCartControl
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "SaveCart"
        Protected Const ATTRIBUTE_CANCEL_BUTTON_PAGE As String = "CancelButtonPage"

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()
            If String.IsNullOrEmpty(CancelButtonPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                CancelButtonPage = Me.GetLinkValueFromXML(ATTRIBUTE_CANCEL_BUTTON_PAGE)
                If String.IsNullOrEmpty(CancelButtonPage) Then
                    Me.cmdCancel.Enabled = False
                    Me.cmdCancel.ToolTip = "CancelButtonPage property has not been set."
                End If
            End If
        End Sub
        Public Overridable Property CancelButtonPage() As String
            Get
                If Not ViewState(ATTRIBUTE_CANCEL_BUTTON_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_CANCEL_BUTTON_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_CANCEL_BUTTON_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        ''' <summary>
        ''' This button click will save the current shopping cart as a new cart.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
            Try
                Page.Validate()

                Dim lCartID As Long
                lCartID = ShoppingCart1.SaveShoppingCart(txtName.Text, txtDescription.Text, -1)
                If lCartID > 0 Then
                    lblResult.Text = "Cart Saved: For your reference the Cart ID is " & lCartID & _
                                     "<BR>In the future, if you would like to use this saved cart, go to Customer Service and select 'Open Cart' from the options"
                    lblInfo.Visible = False
                    lblResult.ForeColor = Drawing.Color.Green
                    lblResult.Visible = True
                    trName.Visible = False
                    trDescription.Visible = False
                    trSave.Visible = False
                Else
                    lblResult.Visible = True
                    lblResult.Text = "Error Saving Cart"
                    lblResult.ForeColor = Drawing.Color.Red
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
            Response.Redirect(CancelButtonPage)
        End Sub

        ''' <summary>
        ''' This button click will save the current shopping cart as the user's currently loaded shopping cart.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub cmdSaveCurrent_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSaveCurrent.Click
            Try
                Page.Validate()

                Dim lCartID As Long
                lCartID = ShoppingCart1.SaveShoppingCart(txtName.Text, txtDescription.Text)
                If lCartID > 0 Then
                    lblResult.Text = "Cart Saved: For your reference the Cart ID is " & lCartID & _
                                     "<BR>In the future, if you would like to use this saved cart, go to Customer Service and select 'Open Cart' from the options"
                    lblInfo.Visible = False
                    lblResult.ForeColor = Drawing.Color.Green
                    lblResult.Visible = True
                    trName.Visible = False
                    trDescription.Visible = False
                    trSave.Visible = False
                Else
                    lblResult.Visible = True
                    lblResult.Text = "Error Saving Cart"
                    lblResult.ForeColor = Drawing.Color.Red
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
            If Not IsPostBack() Then
                If CLng(Request.QueryString("ShoppingCartID")) > 0 Then
                    'Load Cart Name & Description if needed.
                    Dim sSQL As String
                    Dim dt As DataTable
                    Dim colParams(0) As Data.IDataParameter
                    Dim ShoppingCartID As Data.IDataParameter = Nothing

                    sSQL = "SELECT * FROM " & Database & _
                           "..vwWebShoppingCarts WHERE ID = @ShoppingCartID"

                    Try
                        ShoppingCartID = DataAction.GetDataParameter("@ShoppingCartID", SqlDbType.BigInt, Request.QueryString("ShoppingCartID"))
                    Catch ex As Exception
                        Throw New ArgumentException("Parameter must be numeric.", "ShoppingCartID")
                    End Try

                    colParams(0) = ShoppingCartID

                    dt = DataAction.GetDataTableParametrized(sSQL, CommandType.Text, colParams)

                    If dt.Rows.Count = 1 Then
                        txtName.Text = CStr(dt.Rows(0).Item("Name"))
                        txtDescription.Text = CStr(dt.Rows(0).Item("Description"))
                    End If

                    cmdSaveCurrent.Visible = True
                    cmdSave.Visible = True
                Else
                    cmdSaveCurrent.Visible = False
                    cmdSave.Visible = True
                End If
            End If
        End Sub
    End Class
End Namespace
