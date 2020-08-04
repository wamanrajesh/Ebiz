'Aptify e-Business 5.5.1, July 2013
Imports System.Data

Partial Class DynamicProductsGrid
    Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced

#Region "Product Grid Properties"

    Private m_sStoredProcName As String = ""
    Private m_sGridSkinID As String = ""
    Private m_UseRecordIDInSP As Boolean = True
    Private m_iNumColumns As Integer
    Private m_dlRepeatDirection As System.Web.UI.WebControls.RepeatDirection
    Private m_sHeaderName As String = ""
    Private m_sGridStyleName As String = ""
    Private m_sGridHeaderStyleName As String = ""
    Private m_sGridItemStyleName As String = ""
    Private m_sGridFooterStyleName As String = ""
    Private m_imgHeight As Integer
    Private m_imgWidth As Integer
    Private m_LoadType As String


    Public Enum StoredProcLoadType
        Category
        FeaturedProduct
    End Enum

    <System.ComponentModel.Category("Aptify Product Grid Properties")> _
    Public Property GridType() As StoredProcLoadType
        Get
            Return m_LoadType
        End Get
        Set(ByVal value As StoredProcLoadType)
            m_LoadType = value
        End Set
    End Property

    <System.ComponentModel.Category("Aptify Product Grid Properties")> _
    Public Property ImageHeight() As Integer
        Get
            Return m_imgHeight
        End Get
        Set(ByVal value As Integer)
            m_imgHeight = value
        End Set
    End Property

    <System.ComponentModel.Category("Aptify Product Grid Properties")> _
    Public Property ImageWidth() As Integer
        Get
            Return m_imgWidth
        End Get
        Set(ByVal value As Integer)
            m_imgWidth = value
        End Set
    End Property


    <System.ComponentModel.Category("Aptify Product Grid Properties")> _
    Public Property GridStyleName() As String
        Get
            Return m_sGridStyleName
        End Get
        Set(ByVal value As String)
            m_sGridStyleName = value
        End Set
    End Property

    <System.ComponentModel.Category("Aptify Product Grid Properties")> _
    Public Property GridHeaderStyleName() As String
        Get
            Return m_sGridHeaderStyleName
        End Get
        Set(ByVal value As String)
            m_sGridHeaderStyleName = value
        End Set
    End Property

    <System.ComponentModel.Category("Aptify Product Grid Properties")> _
    Public Property GridItemStyleName() As String
        Get
            Return m_sGridItemStyleName
        End Get
        Set(ByVal value As String)
            m_sGridItemStyleName = value
        End Set
    End Property

    <System.ComponentModel.Category("Aptify Product Grid Properties")> _
    Public Property GridFooterStyleName() As String
        Get
            Return m_sGridFooterStyleName
        End Get
        Set(ByVal value As String)
            m_sGridFooterStyleName = value
        End Set
    End Property

    <System.ComponentModel.Category("Aptify Product Grid Properties")> _
    Public Property HeaderName() As String
        Get
            Return m_sHeaderName
        End Get
        Set(ByVal value As String)
            m_sHeaderName = value
        End Set
    End Property

    <System.ComponentModel.Category("Aptify Product Grid Properties")> _
    Public Property StoredProcName() As String
        Get
            Return m_sStoredProcName
        End Get
        Set(ByVal value As String)
            m_sStoredProcName = value
        End Set
    End Property

    <System.ComponentModel.Category("Aptify Product Grid Properties")> _
    Public Property GridSkinID() As String
        Get
            Return Me.m_sGridSkinID
        End Get
        Set(ByVal value As String)
            Me.m_sGridSkinID = value
        End Set
    End Property

    <System.ComponentModel.Category("Aptify Product Grid Properties")> _
    Public Property StoredProcUsesRecordIDAsParameter() As Boolean
        Get
            Return Me.m_UseRecordIDInSP
        End Get
        Set(ByVal value As Boolean)
            Me.m_UseRecordIDInSP = value
        End Set
    End Property
    <System.ComponentModel.Category("Aptify Product Grid Properties")> _
    Public Property NumberOfColumns() As Integer
        Get
            Return m_iNumColumns
        End Get
        Set(ByVal value As Integer)
            m_iNumColumns = value
        End Set
    End Property
    <System.ComponentModel.Category("Aptify Product Grid Properties")> _
    Public Property GridRepeatDirection() As System.Web.UI.WebControls.RepeatDirection
        Get
            Return Me.m_dlRepeatDirection

        End Get
        Set(ByVal value As System.Web.UI.WebControls.RepeatDirection)
            Me.m_dlRepeatDirection = value

        End Set
    End Property

#End Region

    Protected Overridable Sub SetGridStyles()
        Me.DataList1.RepeatColumns = Me.NumberOfColumns
        Me.DataList1.RepeatDirection = Me.GridRepeatDirection
        Me.DataList1.Attributes.Add("Class", Me.GridStyleName)
        Me.DataList1.HeaderStyle.CssClass = Me.GridHeaderStyleName
        Me.DataList1.ItemStyle.CssClass = Me.GridItemStyleName
        Me.DataList1.FooterStyle.CssClass = Me.GridFooterStyleName
    End Sub

    Public Sub LoadGrid()
        Dim sSQL As String
        Dim dt As DataTable

        Try
            If Me.m_sStoredProcName <> "" Then
                sSQL = "Execute " & Me.AptifyApplication.GetEntityBaseDatabase("Products") & ".dbo." & Me.StoredProcName

                If Me.GridType = StoredProcLoadType.FeaturedProduct Then
                    sSQL &= " @WebUserID = " & User1.UserID
                ElseIf Me.StoredProcUsesRecordIDAsParameter Then
                    If Me.GridType = StoredProcLoadType.Category Then
                        sSQL &= " " & Me.ControlRecordID.ToString
                    End If
                End If
                
                dt = Me.DataAction.GetDataTable(sSQL)

                If dt IsNot Nothing Then
                    If dt.Rows.Count > 0 Then
                        Me.SetGridStyles()
                        Me.DataList1.DataSource = dt
                        Me.DataList1.DataBind()
                        Me.DataList1.Visible = True
                    End If
                End If
            End If
        Catch ex As Exception
            Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            Me.lblError.text = ex.Message
        End Try
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Me.StoredProcUsesRecordIDAsParameter Then
                If Not Me.SetControlRecordIDFromParam() Then
                    Me.ControlRecordID = -1
                    Me.HeaderName = "Featured Items"
                End If
            End If
            LoadGrid()
        End If
    End Sub

    Protected Sub DataList1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles DataList1.ItemCommand

    End Sub


    Protected Sub DataList1_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles DataList1.ItemCreated
        If e.Item.ItemType = ListItemType.Header Then
            Dim lblHead As Label = CType(e.Item.FindControl("lblHeader"), Label)
            lblHead.Text = Me.HeaderName
        End If


    End Sub

    Protected Sub DataList1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles DataList1.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            If e.Item.FindControl("imgProduct") IsNot Nothing Then

                Dim img As HtmlControls.HtmlImage = CType(e.Item.FindControl("imgProduct"), HtmlControls.HtmlImage)
                img.Height = Me.ImageHeight
                'img.ImageAlign = ImageAlign.Bottom
                'img.BorderWidth = New System.Web.UI.WebControls.Unit(0)
                img.Width = Me.ImageWidth
                img.Border = 0

            End If

        End If
    End Sub
End Class
