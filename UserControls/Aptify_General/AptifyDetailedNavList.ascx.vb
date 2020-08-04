'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On
Imports Aptify.Framework.Web.eBusiness
Imports Telerik.Sitefinity.Web
Imports Telerik.Web.UI
Imports Telerik.Sitefinity

Namespace Aptify.PublicWebSite
    Partial Class AptifyDetailedNavList
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_NUM_COLUMNS As String = "NumColumns"
        Protected Const ATTRIBUTE_BULLET_IMAGE As String = "BulletImage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "AptifyDetailedNavList"

        'Private m_bShowChildren As Boolean = False
        Private m_iNumCols As Integer = 2
        Private m_bHideIfEmpty As Boolean = True

#Region "AptifyDetailedNavList Specific Properties"
        Public Overridable Property NumColumns() As Integer
            Get
                Return m_iNumCols
            End Get
            Set(ByVal value As Integer)
                m_iNumCols = value
            End Set
        End Property

        'Public Overridable Property ShowChildren() As Boolean
        '    Get
        '        Return m_bShowChildren
        '    End Get
        '    Set(ByVal value As Boolean)
        '        m_bShowChildren = value
        '    End Set
        'End Property

        Private Property HideIfEmpty() As Boolean
            Get
                Return m_bHideIfEmpty
            End Get
            Set(ByVal value As Boolean)
                m_bHideIfEmpty = value
            End Set
        End Property

        ''' <summary>
        ''' Image location for the list item bullet
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Property BulletImage() As String
            Get
                If ViewState("BulletImage") IsNot Nothing Then
                    Return ViewState("BulletImage").ToString
                Else
                    Return ""
                End If
            End Get
            Set(ByVal value As String)
                ViewState.Add("BulletImage", Me.FixLinkForVirtualPath(value))
            End Set
        End Property

        '''' <summary>
        '''' Set to True if showing peer (same level) pages and you want to display the current page in the list
        '''' </summary>
        '''' <value></value>
        '''' <returns></returns>
        '''' <remarks></remarks>
        ''Public Overridable Property ShowCurrentPage() As Boolean
        '    Get
        '        If ViewState("ShowCurrentPage") IsNot Nothing Then
        '            Return CBool(ViewState("ShowCurrentPage"))
        '        Else
        '            Return False
        '        End If
        '    End Get
        '    Set(ByVal value As Boolean)
        '        ViewState.Add("ShowCurrentPage", value)
        '    End Set
        'End Property

        'Public Overridable Property NavTitle() As String
        '    Get
        '        Return lblTitle.Text
        '    End Get
        '    Set(ByVal value As String)
        '        lblTitle.Text = value
        '    End Set
        'End Property

        '''' <summary>
        '''' Link URL Prefix - prefix the URL for each link with this string
        '''' </summary>
        '''' <value></value>
        '''' <returns></returns>
        '''' <remarks></remarks>
        'Public Overridable Property LinkURLPrefix() As String
        '    Get
        '        If ViewState("LinkURLPrefix") IsNot Nothing Then
        '            Return ViewState("LinkURLPrefix").ToString
        '        Else
        '            Return ""
        '        End If
        '    End Get
        '    Set(ByVal value As String)
        '        ViewState.Add("LinkURLPrefix", value)
        '    End Set
        'End Property


#End Region

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'set control properties from XML file if needed
            SetProperties()
            Try
                'If Len(BulletImage) = 0 Then
                '    BulletImage = "../Images/features-bullet.gif"
                'End If

                Dim values As New ArrayList()
                Dim oNodes As SiteMapNodeCollection

                'oNodes = SitefinitySiteMap.GetCurrentNode.ChildNodes
                oNodes = SiteMap.CurrentNode.ParentNode.ChildNodes


                Dim i As Integer = 0, iCurrentCol As Integer = 0, oCell As HtmlTableCell, oLit As LiteralControl

                tblMain.Rows.Add(New HtmlTableRow())
                For j As Integer = 0 To m_iNumCols - 1
                    oCell = New HtmlTableCell()
                    oCell.Controls.Add(New LiteralControl("<ul class=""nobullets"">"))
                    oCell.VAlign = "top"
                    tblMain.Rows(0).Cells.Add(oCell)
                Next

                For Each n As PageSiteNode In oNodes
                    'If Me.ShowCurrentPage OrElse _
                    If n.ShowInNavigation Then
                        If String.Compare(n.Url.Replace("~", ""), SiteMap.CurrentNode.Url.Replace("~", ""), 0) <> 0 Then

                            'oLit = New LiteralControl("<li><img src=""" & BulletImage & """ align=""top"" />&nbsp;<a href=""" & Me.LinkURLPrefix & n.Url.Replace("~", "") & """>" & n.Title & "</a></li>")
                            oLit = New LiteralControl("<li><img src=""" & BulletImage & """ align=""top"" />&nbsp;<a href=""" & Me.FixLinkForVirtualPath(n.Url.Replace("~", "")) & """>" & n.Title & "</a></li>")
                            tblMain.Rows(0).Cells(iCurrentCol).Controls.Add(oLit)

                            i += 1
                            iCurrentCol += 1
                            If iCurrentCol > m_iNumCols - 1 Then
                                iCurrentCol = 0
                            End If
                        End If
                    End If
                Next
                If i = 0 AndAlso m_bHideIfEmpty Then
                    theContainer.Visible = False
                End If

                For j As Integer = 0 To m_iNumCols - 1
                    oCell = tblMain.Rows(0).Cells(j)
                    oCell.Controls.Add(New LiteralControl("</ul>"))
                Next

            Catch ex As Exception
            End Try
        End Sub

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(BulletImage) Then
                BulletImage = Me.GetLinkValueFromXML(ATTRIBUTE_BULLET_IMAGE)
            End If
            If NumColumns = 2 OrElse NumColumns = 0 Then
                'since value is the 'default' check the XML file for possible custom setting
                If Not String.IsNullOrEmpty(Me.GetPropertyValueFromXML(ATTRIBUTE_NUM_COLUMNS)) Then
                    NumColumns = CInt(Me.GetPropertyValueFromXML(ATTRIBUTE_NUM_COLUMNS))
                Else
                    NumColumns = 2
                End If
            End If

        End Sub

    End Class
End Namespace
