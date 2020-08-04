'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Namespace Aptify.Framework.Web.eBusiness
    Partial Class NameAddressBlock
        Inherits BaseUserControl

        Public Property Name() As String
            Get
                EnsureChildControls()
                Return lblName.Text
            End Get
            Set(ByVal Value As String)
                lblName.Text = Value
                brName.Visible = Len(Value) > 0
            End Set
        End Property

        Public Property AddressLine1() As String
            Get
                EnsureChildControls()
                Return lblAddressLine1.Text
            End Get
            Set(ByVal Value As String)
                lblAddressLine1.Text = Value
            End Set
        End Property
        Public Property AddressLine2() As String
            Get
                EnsureChildControls()
                Return lblAddressLine2.Text
            End Get
            Set(ByVal Value As String)
                lblAddressLine2.Text = Value
                brAddressLine2.Visible = Len(Value) > 0
            End Set
        End Property
        Public Property AddressLine3() As String
            Get
                EnsureChildControls()
                Return lblAddressLine3.Text
            End Get
            Set(ByVal Value As String)
                lblAddressLine3.Text = Value
                brAddressLine3.Visible = Len(Value) > 0
            End Set
        End Property

        Public Property City() As String
            Get
                EnsureChildControls()
                Return lblCity.Text
            End Get
            Set(ByVal Value As String)
                lblCity.Text = Value
                RefreshComma()
            End Set
        End Property
        Public Property State() As String
            Get
                EnsureChildControls()
                Return lblState.Text
            End Get
            Set(ByVal Value As String)
                lblState.Text = Value
            End Set
        End Property
        Public Property ZipCode() As String
            Get
                EnsureChildControls()
                Return lblZipCode.Text
            End Get
            Set(ByVal Value As String)
                lblZipCode.Text = Value
            End Set
        End Property
        Public Property Country() As String
            Get
                EnsureChildControls()
                Return lblCountry.Text
            End Get
            Set(ByVal Value As String)
                lblCountry.Text = Value
            End Set
        End Property

        Protected Sub RefreshComma()
            'There is a chance that this AddressBlock is empty. 
            'Only display the comma if a City preceeds the State
            If Me.City.Length > 0 Then
                Me.lblCityComma.Visible = True
            Else
                Me.lblCityComma.Visible = False
            End If
        End Sub

        Protected Sub NameAddressBlock_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            RefreshComma()
        End Sub
    End Class
End Namespace
