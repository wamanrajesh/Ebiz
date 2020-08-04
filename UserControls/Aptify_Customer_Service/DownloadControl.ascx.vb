''Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports System.Data
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.Application
Imports System.IO
Imports System.Web.UI
Imports Microsoft.Win32

'Navin Prasad Issue 9388

Namespace Aptify.Framework.Web.eBusiness.CustomerService
    Partial Class DownloadControl
        Inherits BaseUserControlAdvanced
        Protected Const ATTRIBUTE_CONTROL_DEFAULT_NAME As String = "DownloadControl"
        Protected Const ATTRIBUTE_LOGIN_PAGE As String = "LoginPage"
        Protected Const ATTRIBUTE_ORDER_CONFIRMATION_PAGE = "OrderConfirmationPage"
        Protected Const ATTRIBUTE_MAX_DOWNLOAD_REACHED_TEXT = "MaxDownloadReachedText"
        Protected Const ATTRIBUTE_DOWNLOAD_EXPIRED_TEXT = "DownloadExpiredText"
        Protected Const ATTRIBUTE_DOWNLOAD_NOT_AVAILABLE_TEXT = "DownloadNotAvailableText"
        Protected Const ATTRIBUTE_DOWNLOAD_DATA_NOT_FOUND_TEXT = "DownloadDataNotFoundText"
        Protected Const ATTRIBUTE_DOWNLOAD_CONFIRMATION_TEXT = "DownloadConfirmationText"
        Protected Const ATTRIBUTE_DOWNLOAD_ERROR_TEXT = "DownloadErrorText"

#Region "Properties"
        Public Overridable Property LoginPage() As String
            Get
                If Not ViewState(ATTRIBUTE_LOGIN_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_LOGIN_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_LOGIN_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        Public Overridable Property OrderConfirmationPage() As String
            Get
                If Not ViewState(ATTRIBUTE_ORDER_CONFIRMATION_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_ORDER_CONFIRMATION_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_ORDER_CONFIRMATION_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        Public Overridable Property MaxDownloadReachedText() As String
            Get
                If Not ViewState(ATTRIBUTE_MAX_DOWNLOAD_REACHED_TEXT) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_MAX_DOWNLOAD_REACHED_TEXT))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_MAX_DOWNLOAD_REACHED_TEXT) = value
            End Set
        End Property
        Public Overridable Property DownloadExpiredText() As String
            Get
                If Not ViewState(ATTRIBUTE_DOWNLOAD_EXPIRED_TEXT) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_DOWNLOAD_EXPIRED_TEXT))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_DOWNLOAD_EXPIRED_TEXT) = value
            End Set
        End Property
        Public Overridable Property DownloadNotAvailableText() As String
            Get
                If Not ViewState(ATTRIBUTE_DOWNLOAD_NOT_AVAILABLE_TEXT) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_DOWNLOAD_NOT_AVAILABLE_TEXT))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_DOWNLOAD_NOT_AVAILABLE_TEXT) = value
            End Set
        End Property
        Public Overridable Property DownloadDataNotFoundText() As String
            Get
                If Not ViewState(ATTRIBUTE_DOWNLOAD_DATA_NOT_FOUND_TEXT) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_DOWNLOAD_DATA_NOT_FOUND_TEXT))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_DOWNLOAD_DATA_NOT_FOUND_TEXT) = value
            End Set
        End Property
        Public Overridable Property DownloadConfirmationText() As String
            Get
                If Not ViewState(ATTRIBUTE_DOWNLOAD_CONFIRMATION_TEXT) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_DOWNLOAD_CONFIRMATION_TEXT))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_DOWNLOAD_CONFIRMATION_TEXT) = value
            End Set
        End Property
        Public Overridable Property DownloadErrorText() As String
            Get
                If Not ViewState(ATTRIBUTE_DOWNLOAD_ERROR_TEXT) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_DOWNLOAD_ERROR_TEXT))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_DOWNLOAD_ERROR_TEXT) = value
            End Set
        End Property
#End Region

        Protected Overrides Sub SetProperties()
            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTROL_DEFAULT_NAME
            MyBase.SetProperties()
            If String.IsNullOrEmpty(LoginPage) Then
                LoginPage = Me.GetGlobalAttributeValue(ATTRIBUTE_LOGIN_PAGE)
            End If
            If String.IsNullOrEmpty(OrderConfirmationPage) Then
                OrderConfirmationPage = Me.GetGlobalAttributeValue(ATTRIBUTE_ORDER_CONFIRMATION_PAGE)
            End If

            If String.IsNullOrEmpty(MaxDownloadReachedText) Then
                MaxDownloadReachedText = Me.GetPropertyValueFromXML(ATTRIBUTE_MAX_DOWNLOAD_REACHED_TEXT)
            End If

            If String.IsNullOrEmpty(DownloadExpiredText) Then
                DownloadExpiredText = Me.GetPropertyValueFromXML(ATTRIBUTE_DOWNLOAD_EXPIRED_TEXT)
            End If
            If String.IsNullOrEmpty(DownloadNotAvailableText) Then
                DownloadNotAvailableText = Me.GetPropertyValueFromXML(ATTRIBUTE_DOWNLOAD_NOT_AVAILABLE_TEXT)
            End If
            If String.IsNullOrEmpty(DownloadDataNotFoundText) Then
                DownloadDataNotFoundText = Me.GetPropertyValueFromXML(ATTRIBUTE_DOWNLOAD_DATA_NOT_FOUND_TEXT)
            End If
            If String.IsNullOrEmpty(DownloadConfirmationText) Then
                DownloadConfirmationText = Me.GetPropertyValueFromXML(ATTRIBUTE_DOWNLOAD_CONFIRMATION_TEXT)
            End If
            If String.IsNullOrEmpty(DownloadErrorText) Then
                DownloadErrorText = Me.GetPropertyValueFromXML(ATTRIBUTE_DOWNLOAD_ERROR_TEXT)
            End If
        End Sub
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            SetProperties()
            If Not Me.IsPostBack Then
                If User1.UserID > 0 Then
                    ViewDownloadableProducts()
                Else
                    'Suraj Issue 15370,7/31/13 revert back changes application variale to session variable
                    Session("ReturnToPage") = Request.RawUrl
                    'Suraj S Issue 15370, 8/1/13 here we are getting the ReturnToPageURL in "URL" QueryString and passing on login page. 
                    Response.Redirect(LoginPage + "?ReturnURL=" & System.Web.HttpUtility.UrlEncode(Aptify.Framework.Web.Common.WebCryptography.Encrypt(Request.RawUrl)))
                End If
            End If
        End Sub

        Protected Overridable Function CheckProductAvailableforDownload(ByVal lProductID As Long, ByVal lDownloadItemID As Long, ByVal lOrderID As Long, ByVal lPersonID As Long, ByRef iAvailableStatusCode As Integer) As Boolean
            Dim isProductAvailableForDownload As Boolean = True
            iAvailableStatusCode = 0
            Try
                Dim sSQL As String = ""
                sSQL = "select ID,OrderID,ProductID,DownloadItemID,ShipDate,DownloadExpirationDays,DownloadExpirationDate,MaxNumDownload,NumOfDownloads from " +
                  AptifyApplication.GetEntityBaseDatabase("ProductDownloads") + ".." + AptifyApplication.GetEntityBaseView("ProductDownloads") +
                " where OrderID='" + CStr(lOrderID) + "'and ProductID='" + CStr(lProductID) + "'and DownloadItemID='" + CStr(lDownloadItemID) + "'and PersonID='" + CStr(lPersonID) + "'"
                Dim dt As DataTable = Nothing
                dt = DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                    Dim iProductCount As Integer = 0
                    Dim i As Integer = 0
                    For Each rw As DataRow In dt.Rows
                        sSQL = "select count(ID) from " + AptifyApplication.GetEntityBaseDatabase("Products") + ".." + AptifyApplication.GetEntityBaseView("Products") + " where ID='" + CStr(rw("ProductID")) + "'"
                        iProductCount = CInt(DataAction.ExecuteScalar(sSQL))
                        If IsNumeric(iProductCount) AndAlso iProductCount > 0 Then
                            Dim dtProductDownloads As DataTable = Nothing
                            Dim dtDownloadItem As DataTable = Nothing
                            sSQL = "select ID,MaxNumDownload,NumOfDownloads,DownloadExpirationDate from " + AptifyApplication.GetEntityBaseDatabase("ProductDownloads") + ".." + AptifyApplication.GetEntityBaseView("ProductDownloads") + " where ID='" + CStr(rw("ID")) + "'"
                            dtProductDownloads = DataAction.GetDataTable(sSQL)
                            sSQL = "select  ID,Active from " + AptifyApplication.GetEntityBaseDatabase("DownloadItems") + ".." + AptifyApplication.GetEntityBaseView("DownloadItems") + " where ID='" + CStr(rw("DownloadItemID")) + "'"
                            dtDownloadItem = DataAction.GetDataTable(sSQL)
                            If dtDownloadItem IsNot Nothing AndAlso dtProductDownloads IsNot Nothing AndAlso dtDownloadItem.Rows.Count > 0 AndAlso dtProductDownloads.Rows.Count > 0 Then
                                'The Maximum Number of Downloads has been reached. 
                                'For example, if the product has a max download of 3, 
                                'and this gets passed to the Product Download record, 
                                'then the Download button area should be disabled after 
                                'the customer downloads the item for the third time. When 
                                'the user returns to the page, the Download column should read “Max Downloads Reached”
                                If CLng(dtProductDownloads.Rows(0)("MaxNumDownload")) <> 0 Then '0 =unlimited download
                                    If CLng(dtProductDownloads.Rows(0)("MaxNumDownload")) <= CLng(dtProductDownloads.Rows(0)("NumOfDownloads")) Then
                                        iAvailableStatusCode = 1
                                        isProductAvailableForDownload = False
                                    End If
                                End If
                                '	The Product Download record’s Expiration Date has passed. 
                                'Can this be based on the local time zone of the user or is it 
                                'based on the web server? If it’s the web server, we’ll need a statement 
                                'on the My Download pages that downloads expire as of midnight of the 
                                'organization’s time zone (such as “Note: Downloads expire on the specified 
                                'date as of midnight eastern time.) In this case, the Download column should read “Download Expired”
                                If dtProductDownloads.Rows(0)("DownloadExpirationDate") IsNot Nothing AndAlso IsDate(dtProductDownloads.Rows(0)("DownloadExpirationDate")) Then
                                    If Today.Date > CDate(dtProductDownloads.Rows(0)("DownloadExpirationDate")) Then
                                        iAvailableStatusCode = 2
                                        isProductAvailableForDownload = False
                                    End If
                                End If
                                'The Download Item is inactive. If Is Active is unchecked, 
                                'then the Download column should read “Download Not Avaialble”
                                If CBool(dtDownloadItem.Rows(0)("Active")) = False Then
                                    iAvailableStatusCode = 3
                                    isProductAvailableForDownload = False
                                End If
                            End If
                        End If
                        i = i + 1
                    Next
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
            Return isProductAvailableForDownload
        End Function

        ''' <summary>
        ''' This Function will Return the Downloadble product record for a person .
        ''' </summary>
        ''' <param name="lPersonID"></param>
        ''' <param name="sErrorString"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Overridable Function GetDownloadProducts(ByVal lPersonID As Long, ByRef sErrorString As String) As DataTable
            Dim dt As DataTable = New DataTable
            Try
                Dim m_sDatabase As String = AptifyApplication.UserCredentials.EntitiesDatabase
                Dim sSQL As String = m_sDatabase & ".dbo.spGetDownloadProductsforUser"
                Dim params(0) As IDataParameter
                params(0) = DataAction.GetDataParameter("@ShipToID", SqlDbType.BigInt, lPersonID)
                dt = DataAction.GetDataTableParametrized(sSQL, CommandType.StoredProcedure, params)
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                sErrorString = ex.Message
            End Try
            Return dt
        End Function
        Protected Overridable Sub ViewDownloadableProducts()
            Try
            Dim sErrorString As String = ""
            Dim dt As DataTable = GetDownloadProducts(User1.PersonID, sErrorString)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                grdDownload.Visible = True
                grdDownload.DataSource = dt
                    grdDownload.DataBind()
                    grdDownload.AllowPaging = False
                Dim i As Integer = 0
                For Each rw As DataRow In dt.Rows
                    Dim iAvailableStatusCode As Integer = 0
                    If Not CheckProductAvailableforDownload(CLng(rw("ProductID")), CLng(rw("DownloadItemID")), CLng(rw("OrderID")), User1.PersonID, iAvailableStatusCode) Then
                            grdDownload.Items.Item(i).FindControl("btnDownload").Visible = False
                            grdDownload.Items.Item(i).FindControl("lblDMessage").Visible = True
                            Dim lbl As Label = CType(grdDownload.Items.Item(i).FindControl("lblDMessage"), Label)
                        If lbl IsNot Nothing Then
                            Select Case iAvailableStatusCode
                                Case 1
                                    lbl.Text = MaxDownloadReachedText
                                Case 2
                                    lbl.Text = DownloadExpiredText
                                Case 3
                                    lbl.Text = DownloadNotAvailableText
                            End Select
                        End If
                    End If
                    'set the url for order confirmation url
                        Dim hyp As HyperLink = CType(grdDownload.Items.Item(i).FindControl("hypOrderID"), HyperLink)
                    If hyp IsNot Nothing Then
                        hyp.NavigateUrl = OrderConfirmationPage & "?ID=" & CStr(rw("OrderID"))
                    End If
                    'set the download Confirmation Message
                        Dim btn As Button = CType(grdDownload.Items.Item(i).FindControl("btnDownload"), Button)
                    If btn IsNot Nothing Then
                        btn.Attributes.Add("onclick", "javascript:return confirm('" + DownloadConfirmationText + "')")
                    End If
                    i = i + 1
                    Next
                    'suraj Issue 15287, 4/9/13,Filter grid should appear on the page when there are no records available .
            ElseIf sErrorString <> String.Empty Then
                    'exception occured 
                    grdDownload.Visible = True
                    grdDownload.DataSource = dt
                    grdDownload.DataBind()
                Else
                    grdDownload.Visible = True
                    grdDownload.DataSource = dt
                    grdDownload.DataBind()
                   
                End If
            Catch ex As Exception
                ExceptionManagement.ExceptionManager.Publish(ex)
                lblError.Text = DownloadErrorText
                grdDownload.Visible = False
                lblError.Visible = True
            End Try
        End Sub

        Protected Overridable Function DownloadFile(ByVal lAttachmentID As Long, ByVal lProductID As Long, ByRef sErrorString As String) As Boolean
            Try
                Dim objAttachment As New Aptify.Framework.Application.AptifyAttachments(Me.AptifyApplication, "DownloadItems", lAttachmentID)
                Dim fileName As String = CStr(DataAction.ExecuteScalar("select Name from " + AptifyApplication.GetEntityBaseDatabase("Attachments") + ".." + AptifyApplication.GetEntityBaseView("Attachments") + " where id=" & lAttachmentID))
                Dim sdownloadPath As String = Path.GetTempPath
                sdownloadPath = sdownloadPath & fileName
                objAttachment.GetAttachmentFile(lAttachmentID, sdownloadPath)
                If File.Exists(sdownloadPath) Then
                    Dim name As String = System.IO.Path.GetFileName(fileName)
                    Dim ext As String = System.IO.Path.GetExtension(fileName)
                    Dim sContenttype As String = GetAttachemntContentType(ext)
                    Response.AppendHeader("content-disposition", "attachment; filename=" + fileName)
                    Response.ContentType = sContenttype
                    Response.WriteFile(sdownloadPath)
                    Return True
                Else
                    sErrorString = "Not able to download the file to " + Path.GetTempPath
                    Return False
                End If
            Catch ex As Exception
                ExceptionManagement.ExceptionManager.Publish(ex)
                Return False
            End Try
        End Function
        ''' <summary>
        ''' This function returns the content type for response header based on file extension
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Overridable Function GetAttachemntContentType(ByVal sFileExtension As String) As String
            Dim filecontenttype As String = ""
            Dim sDefaultContentType As String = "application/unknown"
            Try
                Dim regKey, fileextkey As RegistryKey
                filecontenttype = sDefaultContentType
                If Registry.ClassesRoot IsNot Nothing Then
                    regKey = Registry.ClassesRoot
                End If
                If regKey IsNot Nothing Then
                    fileextkey = regKey.OpenSubKey(sFileExtension)
                    filecontenttype = CStr(fileextkey.GetValue("Content Type", sDefaultContentType))
                End If
            Catch ex As Exception
                ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
            Try
            If filecontenttype IsNot Nothing AndAlso filecontenttype.Equals(sDefaultContentType) Then
                If sFileExtension IsNot Nothing Then
                    Select Case sFileExtension.ToLower()
                        Case ".htm", ".html"
                            filecontenttype = "text/HTML"
                            Exit Select
                        Case ".txt"
                            filecontenttype = "text/plain"
                            Exit Select
                        Case ".doc", ".rtf"
                            filecontenttype = "Application/msword"
                            Exit Select
                        Case ".pdf"
                            filecontenttype = "Application/pdf"
                            Exit Select
                        Case ".exe"
                            filecontenttype = "Application/exe"
                            Exit Select
                        Case ".xml"
                            filecontenttype = "Application/xml"
                            Exit Select
                        Case ".mp3"
                            filecontenttype = "Application/mp3"
                            Exit Select
                        Case ".mp4"
                            filecontenttype = "Application/mp4"
                            Exit Select
                        Case ".wmv"
                            filecontenttype = "Application/wmv"
                            Exit Select
                        Case ".mpeg"
                            filecontenttype = "Application/mpeg"
                            Exit Select
                        Case ".jpeg", ".jpg"
                            filecontenttype = "image/jpeg"
                            Exit Select
                        Case ".flv"
                            filecontenttype = "Application/flv"
                            Exit Select
                        Case ".dat"
                            filecontenttype = "Application/dat"
                            Exit Select
                        Case ".rm"
                            filecontenttype = "Application/rm"
                        Case ".7z"
                            filecontenttype = "Application/7z"
                        Case ".zip"
                            filecontenttype = "Application/zip"
                        Case ".rar"
                            filecontenttype = "Application/rar"
                            Exit Select
                    End Select
                End If
                End If
            Catch ex As Exception
                ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
            Return filecontenttype
        End Function

        Protected Sub grdDownload_RowCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grdDownload.ItemCommand
            Try
                If e.CommandName = "Download" Then
                    ViewDownloadableProducts()
                    Dim commandArgs As String() = e.CommandArgument.ToString().Split(New Char() {","c})
                    Dim iAvailableStatusCode As Integer = 0
                    If CheckProductAvailableforDownload(CLng(commandArgs(1)), CLng(commandArgs(2)), CLng(commandArgs(3)), User1.PersonID, iAvailableStatusCode) Then
                        Dim sErrorString As String = ""
                        If DownloadFile(CLng(commandArgs(0)), CLng(commandArgs(1)), sErrorString) Then
                            DownloadComplete(CLng(commandArgs(1)), CLng(commandArgs(3)))
                        Else
                            lblError.Text = sErrorString
                            lblError.Visible = True
                            grdDownload.Visible = False
                        End If
                    Else
                        Response.Redirect(Request.Url.ToString)
                    End If
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Overridable Sub DownloadComplete(ByVal lProductID As Long, ByVal lOrderID As Long)
            Try
                Dim sSQL As String = String.Empty
                Dim dt As DataTable = Nothing
                sSQL = "select ID from  " + AptifyApplication.GetEntityBaseDatabase("ProductDownloads") + ".." + AptifyApplication.GetEntityBaseView("ProductDownloads") + " where OrderID='" + CStr(lOrderID) + "'and ProductID='" + CStr(lProductID) + "' and PersonID='" + CStr(User1.PersonID) + "'"
                dt = DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                    Dim oProductDownloadGE As AptifyGenericEntityBase
                    Dim oProductDownloadHistoryGE As AptifyGenericEntityBase
                    oProductDownloadGE = AptifyApplication.GetEntityObject("ProductDownloads", CLng(dt.Rows(0)(0)))
                    If oProductDownloadGE IsNot Nothing Then
                        oProductDownloadHistoryGE = oProductDownloadGE.SubTypes("ProductDownloadHistory").Add()
                        If oProductDownloadHistoryGE IsNot Nothing Then
                            oProductDownloadHistoryGE.SetValue("DownloadDate", DateTime.UtcNow)
                            oProductDownloadHistoryGE.SetValue("IPAddress", Request.UserHostAddress())
                            If hdOffset.Value <> String.Empty Then
                                oProductDownloadHistoryGE.SetValue("TimeZone", (-(CLng(hdOffset.Value) / 60)))
                            End If
                        End If
                        If oProductDownloadGE.GetValue("NumOfDownloads") IsNot Nothing Then
                            oProductDownloadGE.SetValue("NumOfDownloads", Convert.ToInt32(oProductDownloadGE.GetValue("NumOfDownloads")) + 1)
                        End If
                        oProductDownloadGE.Save()
                    End If
                End If
                Response.End()
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

    End Class

End Namespace

