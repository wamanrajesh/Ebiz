'Aptify e-Business 5.5.1, July 2013
Imports Microsoft.VisualBasic

Imports System.Data
Imports Aptify.Framework.Application
Imports Aptify.Framework.DataServices
Imports Aptify.Applications.OrderEntry
Imports System.Collections.Generic
Imports System.Io

Namespace Aptify.Framework.Web.eBusiness
    Public Class CommonMethods
        Dim m_DataAction As Aptify.Framework.DataServices.DataAction
        Dim m_AptifyApplication As Aptify.Framework.Application.AptifyApplication
        Dim m_User As Aptify.Framework.Web.eBusiness.User
        Dim m_Database As String

        Public Sub New()

        End Sub
        Private ReadOnly Property Database As String
            Get
                Return m_Database
            End Get
        End Property
        Private ReadOnly Property DataAction As Aptify.Framework.DataServices.DataAction
            Get
                Return m_DataAction
            End Get
        End Property
        Private ReadOnly Property AptifyApplication As Aptify.Framework.Application.AptifyApplication
            Get
                Return m_AptifyApplication
            End Get
        End Property
        Private ReadOnly Property User As Aptify.Framework.Web.eBusiness.User
            Get
                Return m_User
            End Get
        End Property
        Public Sub New(DataAction As Aptify.Framework.DataServices.DataAction, AptifyApplication As Aptify.Framework.Application.AptifyApplication, _
                       User As Aptify.Framework.Web.eBusiness.User, Database As String)
            m_DataAction = DataAction
            m_Database = Database
            m_AptifyApplication = AptifyApplication
            m_User = User
        End Sub
        Public Sub New(DataAction As Aptify.Framework.DataServices.DataAction)
            m_DataAction = DataAction
        End Sub
        ''' <summary>
        ''' Rashmi P, Issue 5133, 12/6/12 Add ShipmentType Selection.
        ''' </summary>
        ''' <remarks></remarks>
        Public Function LoadShipmentType(PersonCountryCodeID As Integer) As DataTable

            Dim dt As DataTable

            Dim sSql As String, params(0) As IDataParameter

            Try
                sSql = Database & ".." & "spGetShipmentTypes"

                params(0) = Me.DataAction.GetDataParameter("@CountryID", SqlDbType.Int, PersonCountryCodeID)

                dt = Me.DataAction.GetDataTableParametrized(sSql, CommandType.StoredProcedure, params)

                Return dt
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Function


        ''' <summary>
        ''' Convert Byte Array to an Image object
        ''' </summary>
        ''' <param name="byteArrayIn">Byte Array to be converted to Image object</param>
        ''' <returns>Image object</returns>
        ''' <remarks></remarks>
        Public Function byteArrayToImage(ByVal byteArrayIn As Byte()) As Drawing.Image
            Try
                'Maulik Patel, 03/13/2013. Added a check if image length is zero or null. We don't want to do anything.
                If Not byteArrayIn Is Nothing AndAlso byteArrayIn.Length > 0 Then
                    Dim ms As New IO.MemoryStream(byteArrayIn)
                    Dim returnImage As Drawing.Image = Nothing
                    If Not ms Is Nothing Then
                        returnImage = Drawing.Image.FromStream(ms)
                    End If
                    Return returnImage
                End If
                Return Nothing
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return Nothing
            End Try
        End Function

        ''' <summary>
        ''' Resizes the passed Image according to the specified width and height and returns the resized Image
        ''' </summary>
        ''' <param name="imgToResize">Image to be resized</param>
        ''' <param name="width">Resized width</param>
        ''' <param name="height">Resized height</param>
        ''' <returns>Resized Image object</returns>
        ''' <remarks></remarks>
        Public Shared Function resizeImage(ByVal imgToResize As Drawing.Image, ByVal width As Integer, ByVal height As Integer) As Drawing.Image
            Try
                Dim b As New Drawing.Bitmap(width, height)
                Dim g As Drawing.Graphics = Drawing.Graphics.FromImage(DirectCast(b, Drawing.Image))
                g.InterpolationMode = Drawing.Drawing2D.InterpolationMode.HighQualityBicubic

                If Not imgToResize Is Nothing Then
                    g.DrawImage(imgToResize, 0, 0, width, height)
                End If

                g.Dispose()

                Return DirectCast(b, Drawing.Image)
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return Nothing
            End Try
        End Function

        ''' <summary>
        ''' Resizes the passed Image according to the specified width and height and returns the resized Image as Byte Array
        ''' </summary>
        ''' <param name="imgToResize">Image to be resized</param>
        ''' <param name="width">Resized width</param>
        ''' <param name="height">Resized height</param>
        ''' <returns>Resized Image as Byte Array</returns>
        ''' <remarks></remarks>
        Public Shared Function resizeImageAndGetAsByte(ByVal imgToResize As Drawing.Image, ByVal width As Integer, ByVal height As Integer) As Byte()
            Try
                Dim ms As New IO.MemoryStream()
                Dim img As Drawing.Image = resizeImage(imgToResize, width, height)
                If Not img Is Nothing AndAlso Not imgToResize Is Nothing Then
                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Png)
                End If
                Return ms.ToArray()
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return Nothing
            End Try
        End Function
        ''' <summary>
        ''' Rashmi P, Issue 10254, Apply Saved Payment Method in Ebusiness, 31/12/12
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function LoadSaveSavedPaymentMethods() As Data.DataTable
            Dim sSQL As String
            Dim params1(0) As Data.IDataParameter
            Dim DT As Data.DataTable
            Try

                sSQL = Database & ".." & "spGetPersonSavedPaymentMethods"

                params1(0) = Me.DataAction.GetDataParameter("@PersonID", Data.SqlDbType.Int, User.PersonID)

                DT = Me.DataAction.GetDataTableParametrized(sSQL, Data.CommandType.StoredProcedure, params1)
                'suraj Issue 15287, 4/9/13,Filter grid should appear on the page when there are no records available .so remove the condition
                Return DT
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return Nothing
            End Try
        End Function
        ''' <summary>
        ''' Rashmi P, Issue 5133
        ''' Funtion returs True, if IncludeInShippingCalculation is True for Product.
        ''' </summary>
        ''' <param name="lProductID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IncludeInShipping(lProductID As Long) As Boolean
            Try
                Dim dt As DataTable
                Dim sSQL As String

                sSQL = "SELECT IncludeInShippingCalculation FROM " & _
                       AptifyApplication.GetEntityBaseDatabase("Products") & _
                       "..vwProducts WHERE ID=" & lProductID
                dt = DataAction.GetDataTable(sSQL, IAptifyDataAction.DSLCacheSetting.BypassCache)

                If (dt.Rows.Count() > 0) Then

                    Return CBool(dt.Rows(0).Item("IncludeInShippingCalculation"))
                End If
                Return False
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return False
            End Try
        End Function
        ''' <summary>
        ''' Suraj, Issue 15210
        ''' Funtion returs True, if the email id is valid.
        ''' </summary>
        ''' <param name="emailAddress"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function EmailAddressCheck(ByVal emailAddress As String) As Boolean
            Try
                'Suraj Issue 15210, 3/19/13 This is logic start to check the email having more than one (.) in continiously format
                'following regular expression which is not working in code behind but for regularexpression validator control its    
                'working properly so for that purpose we need to apply explicitly logic here
                Dim arrayEmailChar() As Char = emailAddress
                Dim iSValidFirstdot As Boolean
                Dim iSValidSecoddot As Boolean
                Dim indexCount = 0
                For index = 0 To arrayEmailChar.Length - 1
                    If arrayEmailChar(index) = "." Then
                        If indexCount = 0 Then
                            iSValidFirstdot = True
                            indexCount = index
                        Else
                            If indexCount - index = -1 AndAlso indexCount <> index Then
                                iSValidSecoddot = True
                            Else
                                indexCount = 0
                            End If
                        End If
                    Else
                        If indexCount - index = -1 Then
                            indexCount = 0
                        End If
                    End If
                    If iSValidFirstdot AndAlso iSValidSecoddot Then
                        Return False
                        Exit For
                    End If
                Next
                'end logic
                Dim pattern As String = "[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+(?:[A-Z]{2}|com|COM|org|ORG|net|NET|edu|EDU|gov|GOV|mil|MIL|biz|BIZ|info|INFO|mobi|MOBI|name|NAME|aero|AERO|asia|ASIA|jobs|JOBS|museum|MUSEUM|in|IN|co|CO)\b"
                Dim emailAddressMatch As Match = Regex.Match(emailAddress, pattern)
                If emailAddressMatch.Success Then
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return False
            End Try
        End Function
        '''RashmiP, Funtion return prodcut price depending on persons membership.
        Protected Overridable Function GetProductPrice(ProductID As Long, PersonID As Integer) As Long
            Try
                Dim oProduct As Aptify.Applications.ProductSetup.ProductObject
                Dim oPrice As New IProductPrice.PriceInfo
                oProduct = DirectCast(AptifyApplication.GetEntityObject("Products", ProductID), Aptify.Applications.ProductSetup.ProductObject)
                oProduct.ProductPriceObject.GetPrice(oPrice, ProductID, 1, PersonID, ProductGE:=oProduct, CurrencyTypeID:=User.PreferredCurrencyTypeID)
                Return CLng(oPrice.Price)
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Function

        Public Shared Sub FormatedDateOnGrid(ByVal dateDetails As List(Of String), ByVal RowItem As Telerik.Web.UI.GridItem)
            Try
                If RowItem.ItemType = Telerik.Web.UI.GridItemType.Item OrElse RowItem.ItemType = Telerik.Web.UI.GridItemType.AlternatingItem Then
                    For index = 0 To dateDetails.Count - 1
                        Dim ditem As Telerik.Web.UI.GridDataItem = CType(RowItem, Telerik.Web.UI.GridDataItem)
                        Dim cell As TableCell = CType(ditem(dateDetails(index).Trim()), TableCell)
                        ' To show blank field if the date is Null or 1/1/1900
                        If String.IsNullOrEmpty(cell.Text) Or cell.Text.Contains("1/1/1900") = True Or cell.Text.Trim() = "&nbsp;" Then
                            cell.Text = " "
                            'End If
                            'To remove the 12:00 AM
                        ElseIf cell.Text.Contains("12:00:00 AM") Then
                            Dim value As DateTime = Convert.ToDateTime(cell.Text)
                            cell.Text = String.Format("{0:MMMM dd, yyyy}", value)
                        Else
                            Dim value As DateTime = Convert.ToDateTime(cell.Text)
                            cell.Text = String.Format("{0:MMMM dd, yyyy hh:mm tt}", value)
                        End If
                    Next
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        ''' <summary>
        ''' Verify if the user is associated with chapter. 
        ''' Added by Suvarna for IssueId-15158
        ''' </summary>
        ''' <param name="lPersonID"></param>
        ''' <param name="lChapterID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsAuthorizedUser(ByVal lPersonID As Long, ByVal lChapterID As Long) As Boolean
            Try
                Dim sSQL As String = ""
                Dim iChapterCount As Int32

                sSQL = AptifyApplication.GetEntityBaseDatabase("Persons") & _
                           "..spIsAuthorizedChapterUser @PersonID=" & lPersonID & ", @ChapterID=" & lChapterID
                iChapterCount = DataAction.ExecuteScalar(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)

                ''if user is not Athorized return false else True
                If iChapterCount = 0 Then
                    Return False
                ElseIf iChapterCount > 0 Then
                    Return True
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return False
            End Try
        End Function
        ' Added by neha for issue 16001 image issue for telerik grids, 05/07/2013
        'convert imageByte to virtual Image
        Public Sub getResizedImageHeightandWidth(ByRef profileImage As Drawing.Image, ByRef profileImageByte As Byte(), ByVal ProfileThumbNailWidth As Integer, ByVal ProfileThumbNailHeight As Integer, ByRef aspratioWidth As Integer)
            If profileImageByte IsNot Nothing AndAlso profileImageByte.Length > 0 Then
                Dim sMemstrm As MemoryStream = New MemoryStream(profileImageByte)
                Dim lVirtualImageHeight As Long
                Dim lVirtualImageWidth As Long
                If sMemstrm IsNot Nothing Then
                    Dim originalImage As System.Drawing.Image = System.Drawing.Image.FromStream(sMemstrm)
                    If originalImage IsNot Nothing Then
                        lVirtualImageHeight = originalImage.Height
                        lVirtualImageWidth = originalImage.Width
                    End If
                End If
                Dim aspratio As AspectRatio = New AspectRatio()
                aspratio.WidthAndHeight(lVirtualImageWidth, lVirtualImageHeight, ProfileThumbNailWidth, ProfileThumbNailHeight)
                aspratioWidth = aspratio.Width
            End If
        End Sub
        ''Rashmi: This method return true if user is Group Administrato.........................
        Public Function UserIsGroupAdmin(ByVal lPersonID As Long) As Boolean
            Try
                Dim sSQL As String
                Dim IsGroupAdmin As Boolean = False
                sSQL = "SELECT IsGroupAdmin FROM VWPERSONS WHERE ID = " & lPersonID

                IsGroupAdmin = CBool(DataAction.ExecuteScalar(sSQL))
                Return IsGroupAdmin
            Catch ex As Exception
                Return False
            End Try
        End Function
    End Class
End Namespace

