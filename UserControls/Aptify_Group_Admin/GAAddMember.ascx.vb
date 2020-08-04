'Aptify e-Business 5.5.1, July 2013
Option Explicit On

Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Framework.Application
Imports Aptify.Framework.DataServices
Imports System.Data
Imports Telerik.Web.UI
Imports Aptify.Framework.BusinessLogic.ProcessPipeline
Imports System.IO
Imports Aptify.Framework.BusinessLogic.CultureUtility
Imports System.Data.OleDb
Imports Microsoft.Office.Interop
Imports Excel

Namespace Aptify.Framework.Web.eBusiness.CustomerService
    Partial Class GAAddMember
        Inherits BaseUserControlAdvanced
        Protected m_columncount As Integer = -1
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "GAAddMember"
        Protected Const ATTRIBUTE_Excel_Template_ As String = "ExcelTemplateURL"
        Protected Const ATTRIBUTE_Excel_FileName_ As String = "ExcelFileNameURL"
        Protected Const ATTRIBUTE_Excel_TemplateFolder_ As String = "ExcelTemplateFolderURL"
        Protected Const ATTRIBUTE_Excel_Export_ As String = "ExcelExportURL"
        Protected Const ATTRIBUTE_Column_Count As String = "ColumnCount"
        Protected Const ATTRIBUTE_LOGIN_PAGE As String = "LoginPage" 'Added by Sandeep for Issue 15051 on 12/03/2013
        Enum GridColIndex
            RowNumber = 0
            FirstName = 1
            LastName
            Title
            Email
            Company
            CreateWebUser
            'ImgFailedRec
            Delete
        End Enum

        Private iInsertedData As Integer = 0
        Public Property InsertedData() As Integer
            Get
                Return iInsertedData
            End Get
            Set(ByVal Value As Integer)
                iInsertedData = Value
            End Set
        End Property
        Public Property ColumnCount() As Integer
            Get
                Return m_columncount
            End Get
            Set(ByVal value As Integer)
                m_columncount = value
            End Set
        End Property



        Public Overridable Property ExcelTemplateURL() As String
            Get
                If Not ViewState(ATTRIBUTE_Excel_Template_) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_Excel_Template_))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_Excel_Template_) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        Public Overridable Property ExcelFileNameURL() As String
            Get
                If Not ViewState(ATTRIBUTE_Excel_FileName_) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_Excel_FileName_))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                'ViewState(ATTRIBUTE_Excel_FileName_) = Me.FixLinkForVirtualPath(value)
                ViewState(ATTRIBUTE_Excel_FileName_) = value
            End Set
        End Property

        Public Overridable Property ExcelTemplateFolderURL() As String
            Get
                If Not ViewState(ATTRIBUTE_Excel_TemplateFolder_) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_Excel_TemplateFolder_))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_Excel_TemplateFolder_) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        Public Overridable Property ExcelExportURL() As String
            Get
                If Not ViewState(ATTRIBUTE_Excel_Export_) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_Excel_Export_))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_Excel_Export_) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        'Added by Sandeep for Issue 15051 on 12/03/2013
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


        Protected Overrides Sub SetProperties()
            If ColumnCount = -1 Then
                Dim sWidth As String = ""
                sWidth = Me.GetPropertyValueFromXML(ATTRIBUTE_Column_Count)
                If IsNumeric(sWidth) Then
                    ColumnCount = CInt(sWidth)
                End If
            End If

            If String.IsNullOrEmpty(ExcelTemplateURL) Then
                'since value is the 'default' check the XML file for possible custom setting
                ExcelTemplateURL = Me.GetLinkValueFromXML(ATTRIBUTE_Excel_Template_)
                If String.IsNullOrEmpty(ExcelTemplateURL) Then

                End If
            End If

            If String.IsNullOrEmpty(ExcelFileNameURL) Then
                'since value is the 'default' check the XML file for possible custom setting
                ExcelFileNameURL = Me.GetLinkValueFromXML(ATTRIBUTE_Excel_FileName_)
                If String.IsNullOrEmpty(ExcelFileNameURL) Then
                End If
            End If

            If String.IsNullOrEmpty(ExcelTemplateFolderURL) Then
                'since value is the 'default' check the XML file for possible custom setting
                ExcelTemplateFolderURL = Me.GetLinkValueFromXML(ATTRIBUTE_Excel_TemplateFolder_)
                If String.IsNullOrEmpty(ExcelTemplateFolderURL) Then
                End If
            End If

            If String.IsNullOrEmpty(ExcelExportURL) Then
                'since value is the 'default' check the XML file for possible custom setting
                ExcelExportURL = Me.GetLinkValueFromXML(ATTRIBUTE_Excel_Export_)
                If String.IsNullOrEmpty(ExcelExportURL) Then
                End If
            End If
            'Added by Sandeep for Issue 15051 on 12/03/2013
            If String.IsNullOrEmpty(LoginPage) Then
                'since value is the 'default' check the XML file for possible custom setting
                LoginPage = Me.GetLinkValueFromXML(ATTRIBUTE_LOGIN_PAGE)
            End If

        End Sub
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            SetProperties()

            If Not IsPostBack Then
                SetInitialRow()
            End If
            If User1.UserID < 0 Then
                Response.Redirect(LoginPage) 'Added by Sandeep for Issue 15051 on 12/03/2013
            End If
            If User1.UserID < 0 Then
                Response.Redirect(LoginPage) 'Added by Sandeep for Issue 15051 on 12/03/2013
            End If
        End Sub

        Private Sub SetInitialRow()

            Dim dt As DataTable = New DataTable()

            Dim dr As DataRow = Nothing

            dt.Columns.Add(New DataColumn("RowNumber", GetType(String)))
            dt.Columns.Add(New DataColumn("first Name", GetType(String)))
            dt.Columns.Add(New DataColumn("Last Name", GetType(String)))
            dt.Columns.Add(New DataColumn("Title", GetType(String)))
            dt.Columns.Add(New DataColumn("Email", GetType(String)))
            dt.Columns.Add(New DataColumn("Company", GetType(String)))
            dt.Columns.Add(New DataColumn("Create Web User?", GetType(Boolean)))
            'Anil B , Issue 16516 on 23-may-2013
            'Remove code to serialize if state server is enabled
            'dt.Columns.Add(New DataColumn("ImgFailedRec", GetType(String)))
            'dt.Columns.Add(New DataColumn("Delete", GetType(ImageButton)))
            dr = dt.NewRow()

            dr("RowNumber") = 1
            dr("First Name") = String.Empty
            dr("Last Name") = String.Empty
            dr("Title") = String.Empty
            dr("Email") = String.Empty
            dr("Company") = User1.Company
            dr("Create Web User?") = True
            'dr("ImgFailedRec") = String.Empty

            'Anil B , Issue 16516 on 23-may-2013
            'Remove code to serialize if state server is enabled
            'dr("Delete") = New ImageButton()

            dt.Rows.Add(dr)

            'dr = dt.NewRow();

            'Store the DataTable in ViewState

            Session("CurrentTable") = dt



            grdAddMember.DataSource = dt
            grdAddMember.AllowPaging = False
            grdAddMember.DataBind()
            lblResult.Text = ""
        End Sub

        Private Sub AddNewRowToGrid()

            Dim rowIndex As Integer = 0

            If Session("CurrentTable") IsNot Nothing Then


                Dim dtCurrentTable As DataTable = DirectCast(Session("CurrentTable"), DataTable)
                Dim drCurrentRow As DataRow = Nothing

                If dtCurrentTable.Rows.Count > 0 Then
                    For i As Integer = 1 To dtCurrentTable.Rows.Count
                        'extract the TextBox values
                        Dim txtFname As TextBox = DirectCast(grdAddMember.Rows(rowIndex).Cells(GridColIndex.FirstName).FindControl("txtFname"), TextBox)
                        Dim txtLName As TextBox = DirectCast(grdAddMember.Rows(rowIndex).Cells(GridColIndex.LastName).FindControl("txtLName"), TextBox)
                        Dim txtTitle As TextBox = DirectCast(grdAddMember.Rows(rowIndex).Cells(GridColIndex.Title).FindControl("txtTitle"), TextBox)
                        Dim txtEmail As TextBox = DirectCast(grdAddMember.Rows(rowIndex).Cells(GridColIndex.Email).FindControl("txtEmail"), TextBox)
                        Dim lblGACompany As Label = DirectCast(grdAddMember.Rows(rowIndex).Cells(GridColIndex.Company).FindControl("lblGACompany"), Label)
                        Dim chkCreateWebUser As CheckBox = DirectCast(grdAddMember.Rows(rowIndex).Cells(GridColIndex.CreateWebUser).FindControl("chkCreateWebUser"), CheckBox)
                        'Dim lblImgFailedRec As Label = DirectCast(grdAddMember.Rows(rowIndex).Cells(GridColIndex.ImgFailedRec).FindControl("lblImgFailedRec"), Label)
                        'Anil B , Issue 16516 on 23-may-2013
                        'Remove code to serialize if state server is enabled
                        'Dim btndelete As ImageButton = DirectCast(grdAddMember.Rows(rowIndex).Cells(GridColIndex.Delete).FindControl("btndelete"), ImageButton)

                        drCurrentRow = dtCurrentTable.NewRow()
                        drCurrentRow("RowNumber") = i + 1

                        dtCurrentTable.Rows(i - 1)("First Name") = txtFname.Text
                        dtCurrentTable.Rows(i - 1)("Last Name") = txtLName.Text
                        dtCurrentTable.Rows(i - 1)("Title") = txtTitle.Text
                        dtCurrentTable.Rows(i - 1)("Email") = txtEmail.Text
                        dtCurrentTable.Rows(i - 1)("Company") = User1.Company

                        If chkCreateWebUser.Checked = True Then
                            dtCurrentTable.Rows(i - 1)("Create Web User?") = True
                        ElseIf chkCreateWebUser.Checked = False Then
                            dtCurrentTable.Rows(i - 1)("Create Web User?") = False
                        End If

                        'dtCurrentTable.Rows(i - 1)("ImgFailedRec") = lblImgFailedRec.Text
                        'Anil B , Issue 16516 on 23-may-2013
                        'Remove code to serialize if state server is enabled
                        'dtCurrentTable.Rows(i - 1)("Delete") = btndelete

                        rowIndex += 1
                    Next

                    dtCurrentTable.Rows.Add(drCurrentRow)
                    Session("CurrentTable") = dtCurrentTable
                    grdAddMember.DataSource = dtCurrentTable
                    grdAddMember.AllowPaging = False
                    grdAddMember.DataBind()
                End If
            Else
                Response.Write("ViewState is null")
            End If
            'Set Previous Data on Postbacks
            lblResult.Text = ""
            SetPreviousData()
        End Sub

        Private Sub SetPreviousData()
            Dim rowIndex As Integer = 0

            If Session("CurrentTable") IsNot Nothing Then
                Dim dt As DataTable = DirectCast(Session("CurrentTable"), DataTable)

                If dt.Rows.Count > 0 Then
                    For i As Integer = 0 To dt.Rows.Count - 1
                        Dim txtFname As TextBox = DirectCast(grdAddMember.Rows(rowIndex).Cells(GridColIndex.FirstName).FindControl("txtFname"), TextBox)
                        Dim txtLName As TextBox = DirectCast(grdAddMember.Rows(rowIndex).Cells(GridColIndex.LastName).FindControl("txtLName"), TextBox)
                        Dim txtTitle As TextBox = DirectCast(grdAddMember.Rows(rowIndex).Cells(GridColIndex.Title).FindControl("txtTitle"), TextBox)
                        Dim txtEmail As TextBox = DirectCast(grdAddMember.Rows(rowIndex).Cells(GridColIndex.Email).FindControl("txtEmail"), TextBox)
                        Dim lblGACompany As Label = DirectCast(grdAddMember.Rows(rowIndex).Cells(GridColIndex.Company).FindControl("lblGACompany"), Label)
                        Dim chkCreateWebUser As CheckBox = DirectCast(grdAddMember.Rows(rowIndex).Cells(GridColIndex.CreateWebUser).FindControl("chkCreateWebUser"), CheckBox)
                        'Dim lblImgFailedRec As Label = DirectCast(grdAddMember.Rows(rowIndex).Cells(GridColIndex.ImgFailedRec).FindControl("lblImgFailedRec"), Label)

                        'Anil B , Issue 16516 on 23-may-2013
                        'Remove code to serialize if state server is enabled
                        'Dim btndelete As ImageButton = DirectCast(grdAddMember.Rows(rowIndex).Cells(GridColIndex.Delete).FindControl("btndelete"), ImageButton)


                        txtFname.Text = Convert.ToString(dt.Rows(i)("First Name"))
                        txtLName.Text = Convert.ToString(dt.Rows(i)("Last Name"))
                        txtTitle.Text = Convert.ToString(dt.Rows(i)("Title"))
                        txtEmail.Text = Convert.ToString(dt.Rows(i)("Email"))
                        lblGACompany.Text = User1.Company

                        If dt.Rows(i)("Create Web User?").ToString().ToUpper() = "TRUE" Then
                            chkCreateWebUser.Checked = True
                        ElseIf dt.Rows(i)("Create Web User?").ToString().ToUpper() = "FALSE" Then
                            chkCreateWebUser.Checked = False
                        End If

                        'Anil B , Issue 16516 on 23-may-2013
                        'Remove code to serialize if state server is enabled
                        'btndelete.CommandArgument = rowIndex

                        rowIndex += 1
                    Next
                End If
            End If
        End Sub

        Private Sub SetFailedData()
            Dim rowIndex As Integer = 0
            Dim itotItems As Integer = grdAddMember.Rows.Count
            Dim iFailedItems As Integer = 0
            Dim bdtBlank As Boolean = False

            If Session("FailedDataTable") IsNot Nothing Then
                Dim dt As DataTable = DirectCast(Session("FailedDataTable"), DataTable)
                grdAddMember.DataSource = dt
                grdAddMember.AllowPaging = False
                grdAddMember.DataBind()

                If dt.Rows.Count > 0 Then
                    For i As Integer = 0 To dt.Rows.Count - 1
                        Dim txtFname As TextBox = DirectCast(grdAddMember.Rows(rowIndex).Cells(GridColIndex.FirstName).FindControl("txtFname"), TextBox)
                        Dim txtLName As TextBox = DirectCast(grdAddMember.Rows(rowIndex).Cells(GridColIndex.LastName).FindControl("txtLName"), TextBox)
                        Dim txtTitle As TextBox = DirectCast(grdAddMember.Rows(rowIndex).Cells(GridColIndex.Title).FindControl("txtTitle"), TextBox)
                        Dim txtEmail As TextBox = DirectCast(grdAddMember.Rows(rowIndex).Cells(GridColIndex.Email).FindControl("txtEmail"), TextBox)
                        Dim lblGACompany As Label = DirectCast(grdAddMember.Rows(rowIndex).Cells(GridColIndex.Company).FindControl("lblGACompany"), Label)
                        Dim chkCreateWebUser As CheckBox = DirectCast(grdAddMember.Rows(rowIndex).Cells(GridColIndex.CreateWebUser).FindControl("chkCreateWebUser"), CheckBox)
                        'Dim lblImgFailedRec As Label = DirectCast(grdAddMember.Rows(rowIndex).Cells(GridColIndex.ImgFailedRec).FindControl("lblImgFailedRec"), Label)

                        'Anil B , Issue 16516 on 23-may-2013
                        'Remove code to serialize if state server is enabled
                        'Dim btndelete As ImageButton = DirectCast(grdAddMember.Rows(rowIndex).Cells(GridColIndex.Delete).FindControl("btndelete"), ImageButton)


                        txtFname.Text = Convert.ToString(dt.Rows(i)("First Name"))
                        txtLName.Text = Convert.ToString(dt.Rows(i)("Last Name"))
                        txtTitle.Text = Convert.ToString(dt.Rows(i)("Title"))
                        txtEmail.Text = Convert.ToString(dt.Rows(i)("Email"))
                        lblGACompany.Text = User1.Company

                        If dt.Rows(i)("Create Web User?").ToString().ToUpper() = "TRUE" Then
                            chkCreateWebUser.Checked = True
                        ElseIf dt.Rows(i)("Create Web User?").ToString().ToUpper() = "FALSE" Then
                            chkCreateWebUser.Checked = False
                        End If

                        'Anil B , Issue 16516 on 23-may-2013
                        'Remove code to serialize if state server is enabled
                        ' btndelete = dt.Rows(i)("Delete")

                        rowIndex += 1

                    Next

                    'Dim lblResult As Label = DirectCast(grdAddMember.FooterRow.FindControl("lblResult"), Label)
                    If dt.Rows.Count > 0 And dt.Rows.Count = itotItems AndAlso InsertedData = 0 Then
                        'bdtBlank = verifyAllblankdata(dt)
                        'If bdtBlank Then
                        '    lblResult.Text = "Please Enter Valid Data"
                        'Else
                        '    lblResult.Text = "Record with this Email ID is already exists."
                        'End If
                        lblResult.Text = "Record with this Email ID already exists."
                    ElseIf dt.Rows.Count = 0 Then
                        lblResult.Text = "All records are uploaded Successfully!"
                    ElseIf dt.Rows.Count > 0 And dt.Rows.Count <> itotItems Then
                        'itotItems = itotItems - dt.Rows.Count
                        lblResult.Text = "No. of People Added: " & InsertedData & "<br>" & "No. of People Failed to Add: " & dt.Rows.Count & " (Email ID Already Exists.)"
                    End If

                End If
            End If

        End Sub

        Protected Sub lnkAddRow_Click(ByVal sender As Object, ByVal e As EventArgs)
            AddNewRowToGrid()
        End Sub

        Protected Sub btnInsertRows_Click(ByVal sender As Object, ByVal e As EventArgs)
            Dim iRows As Integer
            iRows = Convert.ToInt32(drpRows.SelectedValue)
            For i As Integer = 0 To iRows - 1
                AddNewRowToGrid()
            Next

        End Sub

        Private Sub getGridData()
            Dim dt As DataTable = New DataTable()
            Dim dr As DataRow = Nothing

            'Add Colums to data table
            dt.Columns.Add(New DataColumn("RowNumber", GetType(String)))
            dt.Columns.Add(New DataColumn("first Name", GetType(String)))
            dt.Columns.Add(New DataColumn("Last Name", GetType(String)))
            dt.Columns.Add(New DataColumn("Title", GetType(String)))
            dt.Columns.Add(New DataColumn("Email", GetType(String)))
            dt.Columns.Add(New DataColumn("Company", GetType(String)))
            dt.Columns.Add(New DataColumn("Create Web User?", GetType(Boolean)))
            'dtFailedData.Columns.Add(New DataColumn("ImgFailedRec", GetType(String)))

            'Anil B , Issue 16516 on 23-may-2013
            'Remove code to serialize if state server is enabled
            'dt.Columns.Add(New DataColumn("Delete", GetType(ImageButton)))

            For rowIndex As Integer = 0 To grdAddMember.Rows.Count - 1
                'extract the TextBox values
                Dim txtFname As TextBox = DirectCast(grdAddMember.Rows(rowIndex).Cells(GridColIndex.FirstName).FindControl("txtFname"), TextBox)
                Dim txtLName As TextBox = DirectCast(grdAddMember.Rows(rowIndex).Cells(GridColIndex.LastName).FindControl("txtLName"), TextBox)
                Dim txtTitle As TextBox = DirectCast(grdAddMember.Rows(rowIndex).Cells(GridColIndex.Title).FindControl("txtTitle"), TextBox)
                Dim txtEmail As TextBox = DirectCast(grdAddMember.Rows(rowIndex).Cells(GridColIndex.Email).FindControl("txtEmail"), TextBox)
                Dim lblGACompany As Label = DirectCast(grdAddMember.Rows(rowIndex).Cells(GridColIndex.Company).FindControl("lblGACompany"), Label)
                Dim chkCreateWebUser As CheckBox = DirectCast(grdAddMember.Rows(rowIndex).Cells(GridColIndex.CreateWebUser).FindControl("chkCreateWebUser"), CheckBox)

                'Anil B , Issue 16516 on 23-may-2013
                'Remove code to serialize if state server is enabled
                'Dim btndelete As ImageButton = DirectCast(grdAddMember.Rows(rowIndex).Cells(GridColIndex.Delete).FindControl("btndelete"), ImageButton)

                dr = dt.NewRow()
                dr("RowNumber") = rowIndex

                dr("First Name") = txtFname.Text
                dr("Last Name") = txtLName.Text
                dr("Title") = txtTitle.Text
                dr("Email") = txtEmail.Text
                dr("Company") = User1.Company

                If chkCreateWebUser.Checked = True Then
                    dr("Create Web User?") = True
                ElseIf chkCreateWebUser.Checked = False Then
                    dr("Create Web User?") = False
                End If

                'dtCurrentTable.Rows(i - 1)("ImgFailedRec") = lblImgFailedRec.Text

                'Anil B , Issue 16516 on 23-may-2013
                'Remove code to serialize if state server is enabled
                'dr("Delete") = btndelete

                dt.Rows.Add(dr)
            Next
            Session("CurrentTable") = Nothing
            Session("CurrentTable") = dt
        End Sub

        Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As EventArgs)
            Dim dtfailedRows As DataTable = New DataTable()
            Dim itotItems As Integer = grdAddMember.Rows.Count
            Session("FailedDataTable") = Nothing
            'Validate  Entered Data before Inserting into database
            If Not ValidateEnteredData() Then
                lblResult.Text = " Incomplete or Invalid information."
            Else
                For i As Integer = 0 To grdAddMember.Rows.Count - 1
                    If validateAndInsertData(i) Then
                    End If
                Next

                'Dim lblResult As Label = DirectCast(grdAddMember.FooterRow.FindControl("lblResult"), Label)
                If Not Session("FailedDataTable") Is Nothing Then
                    dtfailedRows = DirectCast(Session("FailedDataTable"), DataTable)
                    If dtfailedRows.Rows.Count > 0 Then
                        SetFailedData()
                    Else
                        SetInitialRow()
                        If dtfailedRows.Rows.Count > 0 And dtfailedRows.Rows.Count <> itotItems Then
                            itotItems = itotItems - dtfailedRows.Rows.Count
                            lblResult.Text = "No. of People Added: " & InsertedData & "<br>" & "No. of People Failed to Add: " & dtfailedRows.Rows.Count & " (Email ID Already Exists.)"
                        End If
                    End If
                Else
                    If InsertedData > 0 Then
                        SetInitialRow()
                        lblResult.Text = "No. of People Added: " & InsertedData
                    Else
                        lblResult.Text = "Please Enter Valid Data."
                    End If
                End If
                getGridData()
            End If
        End Sub

        Protected Sub btnDeleteAll_Click(ByVal sender As Object, ByVal e As EventArgs)
            SetInitialRow()
        End Sub

        Public Function ValidateEnteredData() As Boolean
            Dim bValidRecords As Boolean = True
            Try
                For rowIndex As Integer = 0 To grdAddMember.Rows.Count - 1
                    'extract the TextBox values
                    Dim txtFname As TextBox = DirectCast(grdAddMember.Rows(rowIndex).Cells(GridColIndex.FirstName).FindControl("txtFname"), TextBox)
                    Dim txtLName As TextBox = DirectCast(grdAddMember.Rows(rowIndex).Cells(GridColIndex.LastName).FindControl("txtLName"), TextBox)
                    Dim txtTitle As TextBox = DirectCast(grdAddMember.Rows(rowIndex).Cells(GridColIndex.Title).FindControl("txtTitle"), TextBox)
                    Dim txtEmail As TextBox = DirectCast(grdAddMember.Rows(rowIndex).Cells(GridColIndex.Email).FindControl("txtEmail"), TextBox)
                    Dim lblGACompany As Label = DirectCast(grdAddMember.Rows(rowIndex).Cells(GridColIndex.Company).FindControl("lblGACompany"), Label)
                    Dim chkCreateWebUser As CheckBox = DirectCast(grdAddMember.Rows(rowIndex).Cells(GridColIndex.CreateWebUser).FindControl("chkCreateWebUser"), CheckBox)

                    'Anil B , Issue 16516 on 23-may-2013
                    'Remove code to serialize if state server is enabled
                    'Dim btndelete As ImageButton = DirectCast(grdAddMember.Rows(rowIndex).Cells(GridColIndex.Delete).FindControl("btndelete"), ImageButton)

                    If Not (txtFname.Text.Trim <> "" AndAlso txtLName.Text.Trim <> "" AndAlso txtEmail.Text.Trim <> "") Then
                        bValidRecords = False
                        Exit For
                    ElseIf CommonMethods.EmailAddressCheck(txtEmail.Text.Trim) = False Then
                        bValidRecords = False
                        Exit For
                    End If
                Next

                Return bValidRecords

            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Function validateAndInsertData(ByVal iRowIndex As Integer) As Boolean
            Dim dtFailedData As DataTable = New DataTable()
            Dim dr As DataRow = Nothing
            Dim sSQL As String = String.Empty
            Dim lWebUserID As Long
            Dim lPrevRecCountOfPersons As Long
            Dim lNewRecCountOfPersons As Long

            'Add Colums to data table
            dtFailedData.Columns.Add(New DataColumn("RowNumber", GetType(String)))
            dtFailedData.Columns.Add(New DataColumn("first Name", GetType(String)))
            dtFailedData.Columns.Add(New DataColumn("Last Name", GetType(String)))
            dtFailedData.Columns.Add(New DataColumn("Title", GetType(String)))
            dtFailedData.Columns.Add(New DataColumn("Email", GetType(String)))
            dtFailedData.Columns.Add(New DataColumn("Company", GetType(String)))
            dtFailedData.Columns.Add(New DataColumn("Create Web User?", GetType(Boolean)))
            'dtFailedData.Columns.Add(New DataColumn("ImgFailedRec", GetType(String)))

            'Anil B , Issue 16516 on 23-may-2013
            'Remove code to serialize if state server is enabled
            'dtFailedData.Columns.Add(New DataColumn("Delete", GetType(ImageButton)))

            Dim RecordID As Long = -1
            Dim txtFname As TextBox = DirectCast(grdAddMember.Rows(iRowIndex).Cells(GridColIndex.FirstName).FindControl("txtFname"), TextBox)
            Dim txtLName As TextBox = DirectCast(grdAddMember.Rows(iRowIndex).Cells(GridColIndex.LastName).FindControl("txtLName"), TextBox)
            Dim txtTitle As TextBox = DirectCast(grdAddMember.Rows(iRowIndex).Cells(GridColIndex.Title).FindControl("txtTitle"), TextBox)
            Dim txtEmail As TextBox = DirectCast(grdAddMember.Rows(iRowIndex).Cells(GridColIndex.Email).FindControl("txtEmail"), TextBox)
            Dim lblGACompany As Label = DirectCast(grdAddMember.Rows(iRowIndex).Cells(GridColIndex.Company).FindControl("lblGACompany"), Label)
            Dim chkCreateWebUser As CheckBox = DirectCast(grdAddMember.Rows(iRowIndex).Cells(GridColIndex.CreateWebUser).FindControl("chkCreateWebUser"), CheckBox)
            'Dim lblImgFailedRec As Label = DirectCast(grdAddMember.Rows(iRowIndex).Cells(GridColIndex.ImgFailedRec).FindControl("lblImgFailedRec"), Label)

            'Anil B , Issue 16516 on 23-may-2013
            'Remove code to serialize if state server is enabled
            'Dim btndelete As ImageButton = DirectCast(grdAddMember.Rows(iRowIndex).Cells(GridColIndex.Delete).FindControl("btndelete"), ImageButton)


            Try
                If (txtFname.Text.Trim <> "" AndAlso txtLName.Text.Trim <> "" AndAlso txtEmail.Text.Trim <> "") Then
                    'Not to create Web User then
                    If chkCreateWebUser.Checked = False Then
                        lPrevRecCountOfPersons = GetMaxUserID()
                        RecordID = User1.FindCreatePerson(txtFname.Text.Trim & "/" & txtLName.Text.Trim & "/" & txtEmail.Text.Trim)
                        If RecordID > -1 Then
                            lNewRecCountOfPersons = GetMaxUserID()
                            If lNewRecCountOfPersons > lPrevRecCountOfPersons Then
                                Dim oPersonGE As AptifyGenericEntityBase
                                oPersonGE = AptifyApplication.GetEntityObject("Persons", RecordID)
                                If oPersonGE IsNot Nothing Then
                                    'Here get count of record inserted
                                    InsertedData += 1
                                    oPersonGE.SetValue("Title", txtTitle.Text.Trim)
                                    If oPersonGE.Save(False) Then
                                    End If
                                End If
                            Else
                                ''TODO: if failed to add record add it to data table
                                dtFailedData = AddFailedDataTodt(dtFailedData, txtFname.Text.Trim, txtLName.Text.Trim, txtTitle.Text.Trim, txtEmail.Text.Trim, User1.Company, False)
                            End If
                        Else
                            ''TODO: if failed to add record add it to data table
                            dtFailedData = AddFailedDataTodt(dtFailedData, txtFname.Text.Trim, txtLName.Text.Trim, txtTitle.Text.Trim, txtEmail.Text.Trim, User1.Company, False)
                            'lblImgFailedRec.Visible = TruefbNewUser
                        End If
                    ElseIf chkCreateWebUser.Checked = True Then ' if web user to be created
                        Dim user As New Web.eBusiness.User
                        Dim bNewUser As Boolean = False
                        Dim sTempPWD As String = GetRandomPasswordUsingGUID(7)
                        bNewUser = user.UserID <= 0
                        user.SetValue("FirstName", txtFname.Text.Trim)
                        user.SetValue("LastName", txtLName.Text.Trim)
                        user.SetValue("Title", txtTitle.Text.Trim)
                        user.SetValue("Email", txtEmail.Text.Trim)
                        user.SetAddValue("Email1", txtEmail.Text.Trim)
                        user.SetValue("Company", lblGACompany.Text)
                        user.SetValue("WebUserStringID", txtEmail.Text.Trim)
                        user.SetValue("Password", sTempPWD)

                        ''Added by Suvarna for Issue ID - 15377 
                        '' SaveUser function will be used for Group admin specific Add Member Functionality.
                        user.SaveValuesToSessionObject(Page.Session) ''Explicit call given
                        ''Added by Suvarna For Manual Record upload performance improvement.
                        'Anil B for 14317 on 16/05/2013
                        If user.SaveUser(Page.Session, False) Then
                            ''Here get count of record inserted
                            InsertedData += 1
                            sSQL = "select ID from vwWebUsers where LinkID =" & Convert.ToString(user.PersonID)
                            lWebUserID = Convert.ToInt64(DataAction.ExecuteScalar(sSQL))
                            SendWebCredentialsMail(user.PersonID, lWebUserID, sTempPWD)
                        Else
                            ''TODO: if failed to add record add it to data table
                            dtFailedData = AddFailedDataTodt(dtFailedData, txtFname.Text.Trim, txtLName.Text.Trim, txtTitle.Text.Trim, txtEmail.Text.Trim, User1.Company, True)
                        End If
                    End If
                Else
                    If grdAddMember.Rows.Count = 1 Then
                        lblResult.Text = "Please Enter Valid Data."
                    End If
                End If

                If dtFailedData.Rows.Count > 0 Then
                    Session("FailedDataTable") = dtFailedData
                End If
            Catch ex As ArgumentException
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Function

        Private Function GetMaxUserID() As Long
            Try
                Dim sSQL As String
                sSQL = "select max(ID) from  " & _
                    AptifyApplication.GetEntityBaseDatabase("Persons") + ".." + AptifyApplication.GetEntityBaseView("Persons")
                Return CLng(DataAction.ExecuteScalar(sSQL))
            Catch ex As ArgumentException
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Function

        Public Function GetRandomPasswordUsingGUID(ByVal length As Integer) As String
            'Get the GUID
            Dim guidResult As String = System.Guid.NewGuid().ToString()

            'Remove the hyphens
            guidResult = guidResult.Replace("-", String.Empty)

            'Make sure length is valid
            If length <= 0 OrElse length > guidResult.Length Then
                Throw New ArgumentException("Length must be between 1 and " & guidResult.Length)
            End If

            'Return the first length bytes
            Return guidResult.Substring(0, length)
        End Function

        'Anil B , Issue 16516 on 23-may-2013
        'Remove code to serialize if state server is enabled
        Public Function AddFailedDataTodt(ByRef dtFailedRowTable As DataTable, ByVal sFName As String, ByVal sLName As String, ByVal sTitle As String, ByVal sEmail As String, ByVal sCompnay As String, ByVal bWebUser As Boolean) As DataTable
            If Session("FailedDataTable") IsNot Nothing Then
                dtFailedRowTable = Nothing
                dtFailedRowTable = DirectCast(Session("FailedDataTable"), DataTable)
            End If

            Dim drFailedRow As DataRow = Nothing
            drFailedRow = dtFailedRowTable.NewRow()
            If dtFailedRowTable.Rows.Count = 0 Then
                drFailedRow("RowNumber") = dtFailedRowTable.Rows.Count
            ElseIf dtFailedRowTable.Rows.Count > 0 Then
                drFailedRow("RowNumber") = dtFailedRowTable.Rows.Count + 1
            End If

            drFailedRow("First Name") = sFName
            drFailedRow("Last Name") = sLName
            drFailedRow("Title") = sTitle
            drFailedRow("Email") = sEmail
            drFailedRow("Company") = User1.Company
            'drFailedRow("ImgFailedRec") = slbl


            If bWebUser = True Then
                drFailedRow("Create Web User?") = True
            ElseIf bWebUser = False Then
                drFailedRow("Create Web User?") = False
            End If

            'Anil B , Issue 16516 on 23-may-2013
            'Remove code to serialize if state server is enabled
            'drFailedRow("Delete") = btndel

            dtFailedRowTable.Rows.Add(drFailedRow)

            Return dtFailedRowTable
        End Function

        Protected Sub SendWebCredentialsMail(ByVal lPersonID As Long, ByVal lWebUserID As Long, ByVal sTempPWD As String)
            Try
                Dim lProcessFlowID As Long
                'Get the Process Flow ID to be used for sending the Downloadable Order Confirmation Email
                ''New property added to support one way hashing functionality - IssueId- 17790.
                ''Hence New processflow is designed and used.
                'Dim sSQL As String = "SELECT ID FROM " & Database & "..vwProcessFlows WHERE Name='GA_AddMembers'"
                Dim sSQL As String = "SELECT ID FROM " & Database & "..vwProcessFlows WHERE Name='Add Members to Company'"

                Dim oProcessFlowID As Object = DataAction.ExecuteScalar(sSQL, IAptifyDataAction.DSLCacheSetting.UseCache)
                If oProcessFlowID IsNot Nothing AndAlso IsNumeric(oProcessFlowID) Then
                    lProcessFlowID = CLng(oProcessFlowID)
                    Dim context As New AptifyContext
                    context.Properties.AddProperty("PersonID", lPersonID)
                    context.Properties.AddProperty("WebUserID", lWebUserID)
                    ''New property added to support one way hashing functionality - IssueId- 17790
                    context.Properties.AddProperty("WebUserTempPWD", sTempPWD)
                    Dim oResult As ProcessFlowResult
                    oResult = ProcessFlowEngine.ExecuteProcessFlow(Me.AptifyApplication, lProcessFlowID, context)
                    If Not oResult.IsSuccess Then
                        ExceptionManagement.ExceptionManager.Publish(New Exception("Process flow to send Web Credentials through Email. Please refer event handler for more details."))
                    End If
                Else
                    ExceptionManagement.ExceptionManager.Publish(New Exception("Message Template to send Web Credentials Email is not found in the system."))
                End If

            Catch ex As ArgumentException
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub grdAddMember_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles grdAddMember.RowDeleting
            'Amruta, Issue 16872, 4/7/2013 ,To delete row from Add New People grid.
            Try
                getGridData()
                If Session("CurrentTable") IsNot Nothing Then
                    Dim dt As DataTable = DirectCast(Session("CurrentTable"), DataTable)
                    If dt.Rows.Count > 0 Then
                        dt.PrimaryKey = New DataColumn() {dt.Columns("RowNumber")}
                        If dt.Rows.Count = 1 Then

                        ElseIf dt.Rows.Contains(e.RowIndex) Then
                            dt.Rows(CInt(e.RowIndex)).Delete()
                        End If

                        'After removal - refresh the session data as well
                        Session("CurrentTable") = Nothing
                        Session("CurrentTable") = dt
                        grdAddMember.DataSource = dt
                        grdAddMember.AllowPaging = False
                        grdAddMember.DataBind()
                        SetPreviousData()
                    Else
                        SetInitialRow()
                    End If
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Public Function verifyAllblankdata(ByRef dt As DataTable) As Boolean
            Dim bBlanck As Boolean = False

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    If (Convert.ToString(dt.Rows(i)(GridColIndex.FirstName)).Trim = "" And Convert.ToString(dt.Rows(i)(GridColIndex.LastName)).Trim = "" And Convert.ToString(dt.Rows(i)(GridColIndex.Title)).Trim = "" And Convert.ToString(dt.Rows(i)(GridColIndex.Email)).Trim = "") Then
                        bBlanck = True
                    Else
                        bBlanck = False
                        Return bBlanck
                    End If
                Next
            End If
            Return bBlanck
        End Function
        ' Aparna changes story 14317(Group Administrator: Ability to add multiple new persons (non-member) records using Excel)
        Protected Sub Download_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Download.Click
            Try
                'Download Excel file from "~/ExcelTemplate/AddPerson.xlsx" path
                Dim file As New FileInfo(Server.MapPath(ExcelTemplateURL))
                If file.Exists Then
                    Response.Clear()
                    Response.ClearHeaders()
                    Response.ClearContent()
                    'use  file name "ExcelFileNameURL" from config file
                    Response.AddHeader("content-disposition", "attachment; filename=" & ExcelFileNameURL)
                    Response.AddHeader("Content-Type", "application/Excel")
                    Response.ContentType = "application/vnd.xls"
                    Response.AddHeader("Content-Length", Convert.ToString(file.Length))
                    Response.WriteFile(file.FullName)
                    Response.[End]()
                Else
                    Response.Write("This file does not exist.")
                End If
            Catch ex As ArgumentException
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        'upload Excel file function
        Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As EventArgs)
            Try

                Dim dtReadExcel As DataTable = Nothing
                Dim sFileExtension As String = String.Empty
                Dim fileNameExt As String = String.Empty
                Dim Ext As String = String.Empty
                Dim uploadFlag As Boolean = False

                If (xlsUpload.UploadedFiles IsNot Nothing AndAlso xlsUpload.UploadedFiles.Count <> 0) Then
                    uploadFlag = True
                    Dim file1 As UploadedFile = xlsUpload.UploadedFiles(0)
                    sFileExtension = file1.GetName
                    Dim sFileUpload As String = Path.GetExtension(sFileExtension)
                    If sFileUpload.Trim().ToLower() = ".xls" OrElse sFileUpload.Trim().ToLower() = ".xlsx" Then
                        ' Save excel file into Server sub dir XlsUploadFile  
                        ' to catch excel file downloading permission    
                        'file for "ExcelTemplateFolderURL"  from "~/XlsUploadFile/" folder
                        '& User1.UserID & "_" & DateTime.Today.Ticks & ".xlsx"
                        fileNameExt = file1.GetNameWithoutExtension
                        Ext = file1.GetExtension

                        'This code is handling "Refresh"/F5 of page. 
                        'When user does browser refresh after file uploaded, browser re-sends the request and upload the file again.
                        ' So, we are handling that particular case here.
                        Try
                            xlsUpload.UploadedFiles(0).SaveAs(Server.MapPath(ExcelTemplateFolderURL & fileNameExt & User1.UserID & "_" & DateTime.Today.Ticks & Ext))
                        Catch ex As FileNotFoundException
                            Me.lblMessage.Text = "Please upload correct file. Either extension is wrong or file is not selected."
                            lblUploadedExcel.Visible = False
                            ExportExcel.Visible = False
                            radImgSmallExcel.Visible = False
                            Exit Sub
                        End Try

                        Dim sUploadedFile As String = (Server.MapPath(ExcelTemplateFolderURL & fileNameExt & User1.UserID & "_" & DateTime.Today.Ticks & Ext))
                        Try
                            dtReadExcel = xlsInsert(sUploadedFile)
                            Dim dtValidation As DataTable = ValidateAndInsertDataExcel(dtReadExcel, uploadFlag)
                            'Export to excel
                            ExportDataTableToExcel(dtValidation)
                        Catch generatedExceptionName As Exception
                            uploadFlag = False
                            Me.lblMessage.Text = "System uploading Error."
                            lblUploadedExcel.Visible = False
                            ExportExcel.Visible = False
                            radImgSmallExcel.Visible = False
                        End Try
                        ' Delete upload exel  file in sub dir  'XlsUploadFile' no need to keep...
                        File.Delete(sUploadedFile)
                        xlsUpload.UploadedFiles.RemoveAt(0)
                    End If
                    If uploadFlag = True Then
                        Me.lblMessage.Text = "File has been successfully uploaded."
                        ExportExcel.Visible = True
                        radImgSmallExcel.Visible = True
                        lblUploadedExcel.Visible = True
                    End If
                ElseIf Convert.ToString(Me.lblMessage.Text).Trim <> "Invalid File Format" Then
                    Me.lblMessage.Text = "Please upload correct file. Either extension is wrong or file is not selected."
                    lblUploadedExcel.Visible = False
                    ExportExcel.Visible = False
                    radImgSmallExcel.Visible = False
                End If
            Catch ex As ArgumentException
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        'Export uploaded excel file
        Private Sub ExportDataTableToExcel(ByVal dtExportExcel As DataTable)
            Try
                Using ExcelPackage = New OfficeOpenXml.ExcelPackage()
                    Dim ws = ExcelPackage.Workbook.Worksheets.Add("Person-Info")
                    ws.Column(1).Width = 25
                    ws.Column(2).Width = 25
                    ws.Column(3).Width = 25
                    ws.Column(4).Width = 40
                    ws.Column(5).Width = 20
                    ws.Column(6).Width = 60
                    ws.Cells("A1").LoadFromDataTable(dtExportExcel, True)
                    'ExcelExportURL  link file path  "~/ExportFiles/AddPerson_" set in config file
                    Using fileStream = File.Create(Server.MapPath(ExcelExportURL & User1.UserID & "_" & DateTime.Today.Ticks & ".xlsx"))
                        ExcelPackage.SaveAs(fileStream)
                    End Using
                End Using
            Catch ex As ArgumentException
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        'validation for check file format by header
        Private Function ValidateExcelFieldFormat(ByVal dtExcelFormat As DataTable) As Boolean
            Try
                Dim bIsValid As Boolean = False
                Dim lstColumnHeader As New List(Of String)
                'to check if all columns exists
                If dtExcelFormat IsNot Nothing Then
                    For Each dcol As DataColumn In dtExcelFormat.Columns

                        Dim sColumnName As String = dcol.ColumnName.Trim

                        Select Case sColumnName.ToLower()
                            Case "firstname", "lastname", "title", "email", "create web user?"
                                AddColumnNameToList(lstColumnHeader, sColumnName)
                        End Select

                    Next
                    ' to check if all columns exists, check count
                    If lstColumnHeader.Count = ColumnCount Then
                        bIsValid = True
                    Else
                        bIsValid = False
                    End If
                End If
                Return bIsValid
            Catch ex As ArgumentException
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return False
            End Try
        End Function

        'Add colun to list to check same column exist if no then only add
        Private Sub AddColumnNameToList(ByVal lstColumnHeader As List(Of String), ByVal sColumnName As String)
            If lstColumnHeader.Contains(sColumnName) = False Then
                lstColumnHeader.Add(sColumnName)
            End If
        End Sub

        'Read provided Excel file using DLL and return dt
        Private Function xlsInsert(ByVal pth As String) As System.Data.DataTable
            Dim dtReadExcel As DataTable = Nothing
            Dim result As DataSet = Nothing
            Dim dtRow As Data.DataRow = Nothing
            Try
                Dim stream As FileStream = File.Open(pth, FileMode.Open, FileAccess.Read)
                Dim excelReader As IExcelDataReader = Nothing

                If Path.GetExtension(pth).ToLower().Equals(".xls") Then
                    '1. Reading from a binary Excel file ('97-2003 format; *.xls)
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream)
                    'strcon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & pth & ";Extended Properties=""Excel 8.0;HDR=YES;"""
                ElseIf Path.GetExtension(pth).ToLower().Equals(".xlsx") Then
                    '2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream)
                    'strcon = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & pth & ";Extended Properties=""Excel 12.0;HDR=YES;"""
                End If
                '3. DataSet - The result of each spreadsheet will be created in the result.Tables
                result = excelReader.AsDataSet()
                '4. DataSet - Create column names from first row
                'excelReader.IsFirstRowAsColumnNames = True
                dtReadExcel = excelReader.AsDataSet().Tables(0)
                dtRow = dtReadExcel.Rows(6)
                'read header row and assign name as header to dt 
                For index As Integer = 0 To dtReadExcel.Columns.Count - 1
                    If String.IsNullOrEmpty(Convert.ToString(dtRow(index)).Trim) = False AndAlso dtReadExcel.Columns.Contains(Convert.ToString(dtRow(index)).Trim) = False Then
                        dtReadExcel.Columns(index).ColumnName = Convert.ToString(dtRow(index)).Trim()
                    Else
                        dtReadExcel.Columns(index).ColumnName = "Column" & (index + 1)
                    End If
                Next
                'Remove header row which already assign to heading for dt
                dtReadExcel.Rows.Remove(dtRow)
                'Delete row which include instructions ie. first 5 rows
                For i = 0 To ColumnCount
                    dtRow = dtReadExcel.Rows(0)
                    dtReadExcel.Rows.Remove(dtRow)
                Next
                excelReader.Close()
                excelReader = Nothing
                Return dtReadExcel
            Catch ex As ArgumentException
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return dtReadExcel
            End Try

        End Function

        'check validation and add record and remark accordingly
        Private Function ValidateAndInsertDataExcel(ByVal dtvalidation As DataTable, ByRef uploadFlag As Boolean) As DataTable
            Dim dtFailedData As DataTable = New DataTable()
            Dim dr As DataRow = Nothing
            Dim sSQL As String = String.Empty
            Dim lWebUserID As Long
            Dim RecordID As Long = -1
            Dim lFirstPersonRecordID As Long
            Dim lSecondPersonRecordID As Long
            Try
                If ValidateExcelFieldFormat(dtvalidation) = False Then
                    Me.lblMessage.Text = "Invalid File Format."
                    uploadFlag = False
                    lblUploadedExcel.Visible = False
                    ExportExcel.Visible = False
                    radImgSmallExcel.Visible = False
                Else
                    If Not dtvalidation Is Nothing AndAlso dtvalidation.Rows.Count > 0 Then
                        If dtvalidation.Columns.Contains("Remark") = False Then
                            Dim columnRemark As DataColumn = New DataColumn()
                            columnRemark.Caption = "Remark"
                            columnRemark.ColumnName = "Remark"
                            dtvalidation.Columns.Add(columnRemark)
                        End If
                        'xlsx remove blank column from dt
                        If dtvalidation.Rows.Count > 0 Then
                            Dim lstEmptyRows As New List(Of DataRow)
                            For Each row As DataRow In dtvalidation.Rows
                                Dim colContainsData As Boolean = False
                                For Each column As DataColumn In dtvalidation.Columns
                                    If String.IsNullOrEmpty(Convert.ToString(row(column)).Trim) = False Then
                                        colContainsData = True
                                        Exit For
                                    End If
                                Next
                                If colContainsData = False Then
                                    lstEmptyRows.Add(row)
                                End If
                            Next
                            If lstEmptyRows.Count > 0 Then
                                For Each row As DataRow In lstEmptyRows
                                    dtvalidation.Rows.Remove(row)
                                Next
                                dtvalidation.AcceptChanges()
                            End If
                        End If
                        'create recored into person entity Not to create Web User 
                        If dtvalidation.Rows.Count > 0 Then
                            For Each row As DataRow In dtvalidation.Rows

                                If (Convert.ToString(row("FirstName")).Trim <> "" AndAlso Convert.ToString(row("LastName")).Trim <> "" AndAlso Convert.ToString(row("Email")).Trim <> "") Then
                                    'if Create Web User contain data other than 0 or 1 then it conver to 0
                                    If IsNumeric(Convert.ToString(row("Create Web User?")).Trim) = False Then
                                        row("Create Web User?") = 0
                                    ElseIf Convert.ToInt32(row("Create Web User?")) <> 1 Then
                                        row("Create Web User?") = 0
                                    End If
                                    'check Email validation.
                                    If CommonMethods.EmailAddressCheck(Convert.ToString(row("Email")).Trim) Then
                                        'check creaate web user
                                        If Convert.ToInt16(Convert.ToString(row("Create Web User?")).Trim) = 0 Then
                                            lFirstPersonRecordID = GetMaxUserID()
                                            RecordID = User1.FindCreatePerson(Convert.ToString(row("FirstName")).Trim & "/" & Convert.ToString(row("LastName")).Trim & "/" & Convert.ToString(row("Email")).Trim)
                                            If RecordID > -1 Then
                                                lSecondPersonRecordID = GetMaxUserID()
                                                'check record already exist
                                                If lSecondPersonRecordID <> -1 AndAlso lFirstPersonRecordID = lSecondPersonRecordID Then
                                                    row("Remark") = "Record failed to upload. Email-ID already in use, please use a different Email-ID."
                                                Else

                                                    Dim oPersonGE As AptifyGenericEntityBase
                                                    oPersonGE = AptifyApplication.GetEntityObject("Persons", RecordID)
                                                    If oPersonGE IsNot Nothing Then
                                                        'Here get count of record inserteds
                                                        InsertedData += 1
                                                        ' If Not Convert.ToString(row("Title")).Trim = "" Then
                                                        oPersonGE.SetValue("Title", row("Title").ToString.Trim)
                                                        If oPersonGE.Save(False) Then
                                                            row("Remark") = "Record successfully created."
                                                        Else
                                                            row("Remark") = "Record failed to upload. Internal failure, please try again later."
                                                        End If
                                                        'Else
                                                        ' row("Remark") = "Record failed to upload. Internal failure, please try again later."
                                                        ' End If
                                                End If
                                                End If
                                            Else
                                                row("Remark") = "Record failed to upload. Internal failure, please try again later."
                                            End If
                                            'create recored into person entity and also create Web User
                                        ElseIf Convert.ToInt16(Convert.ToString(row("Create Web User?")).Trim) = 1 Then ' if web user to be created
                                            Dim user As New Web.eBusiness.User
                                            Dim bNewUser As Boolean = False
                                            Dim sTempPWD As String = GetRandomPasswordUsingGUID(7)
                                            bNewUser = user.UserID <= 0
                                            user.SetValue("FirstName", Convert.ToString(row("FirstName")).Trim)
                                            user.SetValue("LastName", Convert.ToString(row("LastName")).Trim)
                                            user.SetValue("Title", Convert.ToString(row("Title")).Trim)
                                            user.SetValue("Email", Convert.ToString(row("Email")).Trim)
                                            user.SetAddValue("Email1", Convert.ToString(row("Email")).Trim)
                                            user.SetValue("Company", User1.Company)
                                            user.SetValue("WebUserStringID", Convert.ToString(row("Email")).Trim)
                                            user.SetValue("Password", sTempPWD)

                                            ''Added by Suvarna for Issue ID - 15377 
                                            '' SaveUser function will be used for Group admin specific Add Member Functionality.
                                            user.SaveValuesToSessionObject(Page.Session) ''Explicit call given
                                            'Anil B for 14317 on 16/05/2013
                                            If user.SaveUser(Page.Session, False) Then
                                                ''Here get count of record inserted
                                                row("Remark") = "Record successfully created."
                                                InsertedData += 1
                                                sSQL = "select ID from vwWebUsers where LinkID =" & Convert.ToString(user.PersonID).Trim
                                                lWebUserID = Convert.ToInt64(DataAction.ExecuteScalar(sSQL))
                                                SendWebCredentialsMail(user.PersonID, lWebUserID, sTempPWD)
                                            Else
                                                row("Remark") = "Record failed to upload. Email-ID already in use, please use a different Email-ID."
                                            End If
                                        End If
                                    Else
                                        row("Remark") = "Record failed to upload. Invalid Email-ID, please use a valid Email-ID."
                                    End If
                                Else
                                    If IsNumeric(Convert.ToString(row("Create Web User?")).Trim) = False Then
                                        row("Create Web User?") = 0
                                    ElseIf Convert.ToInt32(row("Create Web User?")) <> 1 Then
                                        row("Create Web User?") = 0
                                    End If
                                    row("Remark") = "failed to upload. Incomplete information, please fill all the required fields(FirstName, LastName and Email-ID)."
                                End If
                            Next
                        End If
                    End If
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
            Return dtvalidation
        End Function

        'Export Excel file link to user to download uploaded file with remark
        Protected Sub ExportExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ExportExcel.Click
            Try
                Dim file As New FileInfo(Server.MapPath(ExcelExportURL & User1.UserID & "_" & DateTime.Today.Ticks & ".xlsx"))
                If file.Exists Then
                    Response.Clear()
                    Response.ClearHeaders()
                    Response.ClearContent()
                    'ExcelFileNameURL  link file path  "AddPerson.xlsx" set in config file
                    Response.AddHeader("content-disposition", "attachment; filename=" & ExcelFileNameURL)
                    Response.AddHeader("Content-Type", "application/Excel")
                    Response.ContentType = "application/vnd.xls"
                    Response.AddHeader("Content-Length", Convert.ToString(file.Length).Trim)
                    Response.WriteFile(file.FullName)
                    Response.[End]()
                Else
                    Response.Write("This file does not exist.")
                End If
                lblMessage.Visible = False
            Catch ex As ArgumentException
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        

    End Class
End Namespace
