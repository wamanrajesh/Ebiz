'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports System.Data
Imports Telerik.Web.UI

Namespace Aptify.Framework.Web.eBusiness.Education
    Partial Class ClassRegistrationControl
        Inherits eBusiness.BaseUserControlAdvanced

        Protected Const ATTRIBUTE_VIEW_CLASS_PAGE As String = "ViewClassPage"
        Protected Const ATTRIBUTE_SUBMIT_REGISTRATION_PAGE As String = "SubmitRegistrationPage"
        Protected Const ATTRIBUTE_REGISTERED_COURSES_PAGE As String = "RegisteredCoursesPage"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "ClassRegistration"
        'Navin Prasad Issue 12943
        Protected Const ATTRIBUTE_BILL_ME_LATER As String = "BillMeLaterDisplayText"
        Protected Const ATTRIBUTE_CONFIRMATION_PAGE As String = "OrderConfirmationPage"
        Private m_ProductType As ProductType


#Region "ClassRegistration Specific Properties"
        ''' <summary>
        ''' ViewClass page url
        ''' </summary>
        Public Overridable Property ViewClassPage() As String
            Get
                If Not ViewState(ATTRIBUTE_VIEW_CLASS_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_VIEW_CLASS_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_VIEW_CLASS_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' SubmitRegistration page url
        ''' </summary>
        Public Overridable Property SubmitRegistrationPage() As String
            Get
                If Not ViewState(ATTRIBUTE_SUBMIT_REGISTRATION_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_SUBMIT_REGISTRATION_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_SUBMIT_REGISTRATION_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''' <summary>
        ''' RegisteredCourses page url
        ''' </summary>
        Public Overridable Property RegisteredCoursesPage() As String
            Get
                If Not ViewState(ATTRIBUTE_REGISTERED_COURSES_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_REGISTERED_COURSES_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_REGISTERED_COURSES_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
        ''RashmiP, issue 6781
        Public Overridable ReadOnly Property BillMeLaterDisplayText() As String
            Get
                If Not ViewState.Item(ATTRIBUTE_BILL_ME_LATER) Is Nothing Then
                    Return CStr(ViewState.Item(ATTRIBUTE_BILL_ME_LATER))
                Else
                    Dim value As String = Me.GetGlobalAttributeValue(ATTRIBUTE_BILL_ME_LATER)
                    If Not String.IsNullOrEmpty(value) Then
                        ViewState.Item(ATTRIBUTE_BILL_ME_LATER) = value
                    End If
                    Return value
                End If
            End Get
        End Property
        ''' <summary>
        ''' RashmiP, OrderConfirmation page url
        ''' </summary>
        Public Overridable Property OrderConfirmationPage() As String
            Get
                If Not ViewState(ATTRIBUTE_CONFIRMATION_PAGE) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_CONFIRMATION_PAGE))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_CONFIRMATION_PAGE) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property
#End Region

        'Added by Vijay Soni for Issue#5416	(Start)
        ' This thye will help to contain Product Type
        Private Enum ProductType
            ClassType = 1
            GeneralType = 2
        End Enum

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(ViewClassPage) Then
                ViewClassPage = Me.GetLinkValueFromXML(ATTRIBUTE_VIEW_CLASS_PAGE)
                If String.IsNullOrEmpty(ViewClassPage) Then
                    Me.lnkClassNum.Enabled = False
                    Me.lnkClassNum.ToolTip = "ViewClassPage property has not been set."
                Else
                    Me.lnkClassNum.NavigateUrl = ViewClassPage
                End If
            Else
                Me.lnkClassNum.NavigateUrl = ViewClassPage
            End If
            If String.IsNullOrEmpty(SubmitRegistrationPage) Then
                SubmitRegistrationPage = Me.GetLinkValueFromXML(ATTRIBUTE_SUBMIT_REGISTRATION_PAGE)
                If String.IsNullOrEmpty(SubmitRegistrationPage) Then
                    Me.btnSaveRegistration.Enabled = False
                    Me.btnSaveRegistration.ToolTip = "SubmitRegistrationPage property has not been set."
                End If
            End If
            'For completeness only, this is used in the DEAD CODE section and will not be called
            If String.IsNullOrEmpty(RegisteredCoursesPage) Then
                RegisteredCoursesPage = Me.GetLinkValueFromXML(ATTRIBUTE_REGISTERED_COURSES_PAGE)
                If String.IsNullOrEmpty(RegisteredCoursesPage) Then
                    RegisteredCoursesPage = Request.ApplicationPath
                End If
            End If
            If String.IsNullOrEmpty(OrderConfirmationPage) Then
                OrderConfirmationPage = Me.GetLinkValueFromXML(ATTRIBUTE_CONFIRMATION_PAGE)
            End If
        End Sub

        'Added by Vijay Soni for Issue#5416	(End)
        Protected Function AddStudent(ByVal dr As DataRow, ByVal OrderGE As Aptify.Applications.OrderEntry.OrdersEntity) As Boolean
            Try
                Dim sStatus As String = CStr(AptifyApplication.Entity("Class Registrations").EntityMetaData.Fields("Status").DefaultValue)
                With OrderGE.AddProduct(CLng(lblProductID.Value)).Item(0)
                    .SetValue("Description", Me.lblCourse.Text & " - " & CStr(dr("LastName")) & ", " & CStr(dr("FirstName")))
                    'Modified by Vijay Soni (Start)
                    If Not IsNothing(.ExtendedOrderDetailEntity) Then
                        .ExtendedOrderDetailEntity.SetValue("ClassID", Me.Request("ClassID"))
                        'Modified by Vijay Soni (End)

                        'HP Issue#8783: removed personID check since cases where name of logged in user is changed, instead of deleting and adding 
                        '               a new row, the personID does not get changed therefore the wrong person gets registered for the class, always
                        '               validate the student record with FindCreatePerson()
                        'If dr("PersonID") IsNot Nothing AndAlso _
                        '   False = IsDBNull(dr("PersonID")) AndAlso _
                        '   CLng(dr("PersonID")) > 0 Then
                        '    .ExtendedOrderDetailEntity.SetValue("StudentID", dr("PersonID"))
                        'Else
                        .ExtendedOrderDetailEntity.SetValue("StudentID", FindCreatePerson(dr))
                        'End If
                        ''''commented by RashmiP, issue 14047, Class Registration Status Should Respect Entity Default value.
                        'If Not IsNothing(lblStartDate.Text) AndAlso lblStartDate.Text.Trim.Length > 0 _
                        'AndAlso CDate(lblStartDate.Text) <= Date.Now Then ' modified by Vijay Soni for Issue#5416 on Dec 07, 2007
                        '    .ExtendedOrderDetailEntity.SetValue("Status", "In-Progress")
                        'Else
                        '    .ExtendedOrderDetailEntity.SetValue("Status", "Registered")
                        'End If
                        .ExtendedOrderDetailEntity.SetValue("Status", sStatus)
                        .ExtendedOrderDetailEntity.SetValue("DateAvailable", Now)
                        'HP: save object data which will be used by the shoppingCart.PlaceOrder() to load the extended Order Detail separately
                        .SetAddValue("__ExtendedAttributeObjectData", .ExtendedOrderDetailEntity.GetObjectData(False))
                    End If
                End With
                Return True
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return False
            End Try
        End Function
        Protected Overridable Function FindCreatePerson(ByVal dr As DataRow) As Long
            Try
                'Modified by Vijay Soni for Issue#5416 on Jan 30, 2008(Start)
                Dim lValue As Object, sbrPersonId As New StringBuilder
                'HP Issue#9261:  correcting apostrophe issue and possible sql injection by using parameters
                Dim col(1) As Data.IDataParameter
                Dim pFname As Data.IDataParameter = Nothing
                Dim pLname As Data.IDataParameter = Nothing
                pFname = Me.DataAction.GetDataParameter("@FName", SqlDbType.NVarChar, CStr(dr("FirstName")))
                pLname = Me.DataAction.GetDataParameter("@LName", SqlDbType.NVarChar, CStr(dr("LastName")))
                col(0) = pFname
                col(1) = pLname

                With sbrPersonId
                    .Length = 0
                    .Append("SELECT TOP 1 ID FROM ")
                    .Append(Me.AptifyApplication.GetEntityBaseDatabase("Persons").ToString + "..")
                    .Append(Me.AptifyApplication.GetEntityBaseView("Persons").ToString)
                    .Append(" WHERE Email1='" + CStr(dr("Email")) + "' OR ")
                    .Append(" (CompanyID=" + User1.CompanyID.ToString)
                    'HP Issue#9261: correcting apostrophe issue and possible sql injection by using parameters
                    '.Append(" AND FirstName='" + CStr(dr("FirstName")) + "'")
                    '.Append(" AND LastName='" + CStr(dr("LastName")) + "')")
                    .Append(" AND FirstName = @FName ")
                    .Append(" AND LastName = @Lname) ")
                    .Append(" Order By")
                    .Append(" Case WHEN CompanyID = " + User1.CompanyID.ToString + " AND ")
                    .Append(" Email1 = '" + CStr(dr("Email")) + "' THEN 0")
                    .Append(" WHEN Email1 = '" + CStr(dr("Email")) + "' THEN 1")
                    .Append(" WHEN (CompanyID=" + User1.CompanyID.ToString)
                    'HP Issue#9261: correcting apostrophe issue and possible sql injection by using parameters
                    '.Append(" AND FirstName='" + CStr(dr("FirstName")) + "'")
                    '.Append(" AND LastName='" + CStr(dr("LastName")) + "'")
                    .Append(" AND FirstName = @FName ")
                    .Append(" AND LastName = @Lname ")
                    .Append("And CompanyID = " + User1.CompanyID.ToString + ") THEN 2  ")
                    .Append("End Asc, ID Asc")
                End With

                'sSQL = "SELECT MIN(ID) FROM " & Me.AptifyApplication.GetEntityBaseDatabase("Persons") & _
                '       "..vwPersons WHERE CompanyID=" & User1.CompanyID & " AND " & _
                '       "( (FirstName='" & CStr(dr("FirstName")) & "' AND LastName='" & _
                '       CStr(dr("LastName")) & "') OR Email1='" & CStr(dr("Email")) & "')"
                'Modified by Vijay Soni for Issue#5416 on Jan 30, 2008(End)

                'HP Issue#9261: correcting apostrophe issue and possible sql injection by using parameters
                'lValue = Me.DataAction.ExecuteScalar(sbrPersonId.ToString, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                lValue = Me.DataAction.ExecuteScalarParametrized(sbrPersonId.ToString, CommandType.Text, col)

                If lValue IsNot Nothing AndAlso IsNumeric(lValue) AndAlso CLng(lValue) > 0 Then
                    Return CLng(lValue)
                Else
                    Dim oPerson As Aptify.Framework.BusinessLogic.GenericEntity.AptifyGenericEntityBase
                    oPerson = Me.AptifyApplication.GetEntityObject("Persons", -1)
                    oPerson.SetValue("CompanyID", Me.User1.CompanyID)
                    oPerson.SetValue("FirstName", CStr(dr("FirstName")))
                    oPerson.SetValue("LastName", CStr(dr("LastName")))
                    oPerson.SetValue("Title", CStr(dr("Title")))
                    oPerson.SetValue("Email1", CStr(dr("Email")))
                    If oPerson.Save(False) Then
                        Return oPerson.RecordID
                    End If
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Function
        'Added by Vijay Soni for Issue#5416(Start)
        Private Function ValidateRegistrants() As Boolean
            Dim txtFirstName, txtLastName, txtEmailAddress As TextBox
            Dim blnRetrunData As Boolean
            'Navin Prasad Issue 11032
            Dim gvRow As GridDataItem
            For Each gvRow In grdStudents.Items
                txtFirstName = TryCast(gvRow.FindControl("txtFirstName"), TextBox)
                txtLastName = TryCast(gvRow.FindControl("txtLastName"), TextBox)
                txtEmailAddress = TryCast(gvRow.FindControl("txtEmail"), TextBox)

                'Amruta ,Issue 15457 ,4/24/2013,To display message on label
                If txtFirstName.Text.Trim.Length <= 0 Or txtLastName.Text.Trim.Length <= 0 Or txtEmailAddress.Text.Trim.Length <= 0 Then
                    lblMsg.Visible = True
                    lblMsg.Text = "One or more registrants have blank First Name, Last Name or E-Mail provided." & "<br/>" & " Please correct the information and resubmit the Registration."

                    Return False
                Else
                    'Suraj S Issue 15210 ,2/7/13 use Comman Function for email validation
                    Dim bIsValidEmail As Boolean = False
                    bIsValidEmail = CommonMethods.EmailAddressCheck(txtEmailAddress.Text)
                    If (bIsValidEmail) = False Then
                        lblMsg.Visible = True
                        lblMsg.Text = "One or more registrants have incorrect E-Mail address."
                        Return False
                    End If

                End If
            Next

            Return True
        End Function

        'Added by Vijay Soni for Issue#5416(End)

        Protected Sub btnSaveRegistration_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveRegistration.Click, btnSaveRegistrationPaid.Click
            Try
                Dim oOrderGE As Aptify.Applications.OrderEntry.OrdersEntity
                ''RashmiP, Issue 14047, 11/29/30
                Dim sStatus As String = CStr(AptifyApplication.Entity("Class Registrations").EntityMetaData.Fields("Status").DefaultValue)
                'Added by Vijay Soni for Issue#5416	(Start) 
                'START DEAD CODE
                If CreditCard.Visible Then
                    Dim oReg As Aptify.Applications.Education.ClassRegistrationGE
                    oReg = CType(Me.AptifyApplication.GetEntityObject("Class Registrations", -1), Aptify.Applications.Education.ClassRegistrationGE)
                    oReg.SetValue("ClassID", Me.Request("ClassID"))
                    oReg.SetValue("StudentID", User1.PersonID)
                    'RashmiP issue 6781, 09/20/10
                    If CreditCard.BillMeLaterChecked Then
                        If String.IsNullOrEmpty(CreditCard.PONumber) Then
                            oReg.SetValue("PONumber", BillMeLaterDisplayText)
                        Else
                            oReg.SetValue("PONumber", CreditCard.PONumber)

                        End If
                        oReg.SetValue("PaymentTypeID", CreditCard.PaymentTypeID)
                    Else
                        Page.Validate()
                        oReg.SetValue("PaymentTypeID", CreditCard.PaymentTypeID)
                        oReg.SetValue("CCAccountNumber", CreditCard.CCNumber)
                        oReg.SetValue("CCExpireDate", CreditCard.CCExpireDate)
                        'Anil B change for 10254 on 23/04/2013
                        'Set reference transaction for payment
                        If CreditCard.CCNumber = "-Ref Transaction-" Then
                            oReg.SetValue("ReferenceTransactionNumber", CreditCard.ReferenceTransactionNumber)
                            oReg.SetValue("ReferenceExpiration", CreditCard.ReferenceExpiration)
                        End If
                    End If
                    oReg.SetValue("EnrollmentTypeID", GetMinEnrollmentTypeID())
                    oReg.SetValue("Status", sStatus)
                    oReg.SetValue("DateAvailable", Now)

                    If oReg.Save(False) Then
                        ''Commented by RashmiP, Issue 14047, Class Registration Status Should Respect Entity Default
                        'If CDate(lblStartDate.Text) <= Date.Now Then
                        '    oReg.SetValue("Status", "In-Progress")
                        '    If oReg.Save(False) Then
                        '        Response.Redirect(RegisteredCoursesPage)
                        '    End If
                        'Else
                        '    Response.Redirect(RegisteredCoursesPage)
                        'End If
                        Response.Redirect(RegisteredCoursesPage)
                    Else
                        lblError.Text = oReg.LastError
                        lblError.Visible = True
                    End If
                    'END DEAD CODE
                Else
                    'Added by Vijay Soni for Issue#5416	(End)
                    If ValidateRegistrants() Then
                        oOrderGE = Me.ShoppingCart1.GetOrderObject(Session, Page.User, Application)
                        Dim dt As DataTable
                        dt = UpdateGetTable()
                        For i As Integer = 0 To dt.Rows.Count - 1
                            If Not AddStudent(dt.Rows(i), oOrderGE) Then
                                Exit Sub
                            End If
                        Next
                        Me.ShoppingCart1.SaveCart(Me.Session)
                        lblError.Visible = False
                        ''RashmiP, Issue 10287, 9/22/11, Redirect to Order confirmation for Free Class
                        If oOrderGE.GrandTotal = 0 Then
                            CompleteOrderforFreeClass()
                        Else
                            Response.Redirect(SubmitRegistrationPage)
                        End If

                    End If
                End If
                'Added by Vijay Soni for Issue#5416

            Catch ex As Exception
                lblError.Text = "Processing Error: " & ex.Message
                lblError.Visible = True
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        'Added by Vijay Soni for Issue#5416	(Start)
        Private Function GetMinEnrollmentTypeID() As Long
            Dim sSQL As String, lValue As Object
            Try
                sSQL = "SELECT MIN(ID) FROM " & _
                       Me.AptifyApplication.GetEntityBaseDatabase("Courses") & _
                       "..vwEnrollmentTypes"
                lValue = Me.DataAction.ExecuteScalar(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.UseCache)
                If Not lValue Is Nothing Then
                    Return CLng(lValue)
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return -1
            End Try
        End Function
        'Added by Vijay Soni for Issue#5416	(End)
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            lblMsg.Visible = False
            SetProperties()
            If Not IsPostBack Then

                LoadClassInfo()
                Me.SetupGrid()

            End If
        End Sub

        Private Sub SetupGrid()
            Try
                If ViewState("DataTable") IsNot Nothing Then
                    grdStudents.DataSource = CType(ViewState("DataTable"), DataTable)
                    grdStudents.DataBind()
                    Exit Sub
                End If
                Dim dt As New Data.DataTable()
                'Dim oOrderGE As Aptify.Framework.BusinessLogic.GenericEntity.AptifyGenericEntityBase 'Added by Vijay Soni for Issue#5416
                dt.Columns.Add("PersonID")
                dt.Columns.Add("FirstName")
                dt.Columns.Add("LastName")
                dt.Columns.Add("Title")
                dt.Columns.Add("Email")

                dt.Rows.Add(dt.NewRow)
                dt.Rows(0).Item(0) = User1.PersonID
                dt.Rows(0).Item(1) = User1.FirstName
                dt.Rows(0).Item(2) = User1.LastName
                dt.Rows(0).Item(3) = User1.Title
                dt.Rows(0).Item(4) = User1.Email

                Me.ShoppingCart1.SaveCart(Me.Session)

                grdStudents.DataSource = dt
                grdStudents.DataBind()

                ViewState.Add("DataTable", dt)
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub LoadClassInfo()
            Try
                Dim sSQL As String, dt As Data.DataTable
                Dim p As Aptify.Applications.ProductSetup.ProductObject 'Added by Vijay Soni for Issue#5416
                ''Added by Vijay Soni for Issue#5416	(Start)
                If Not IsNumeric(Request.QueryString("ClassID")) Then
                    lblCourse.Text = "Error - No Course Found!"
                    Throw New ArgumentException("Parameter must be numeric.", "ClassID")
                End If
                'Added by Vijay Soni for Issue#5416	(End)
                'sSQL = "SELECT c.*,p.LastName + ', ' + p.FirstName InstructorName FROM " & _
                '       Me.AptifyApplication.GetEntityBaseDatabase("Classes") & _
                '       "..vwClasses c INNER JOIN " & _
                '       Me.AptifyApplication.GetEntityBaseDatabase("Persons") & _
                '       "..vwPersons p ON c.InstructorID=p.ID WHERE c.ID=" & Request.QueryString("ClassID")
                'Sheetal 16/07/15: For Issue 21368 :Prevent Sql Injection Attack: Used parameterrized sql is best practice
                Dim params(0) As IDataParameter
                params(0) = Me.DataAction.GetDataParameter("@ClassID", SqlDbType.Int, Request.QueryString("ClassID"))

                sSQL = Me.AptifyApplication.GetEntityBaseDatabase("Classes") & "..spLoadClassInfo"
                dt = Me.DataAction.GetDataTableParametrized(sSQL, CommandType.StoredProcedure, params)


                '  dt = Me.DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)


                If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                    With dt.Rows(0)
                        lblClassNum.Text = .Item("ClassNumber").ToString
                        lblProductID.Value = .Item("ProductID").ToString
                        lnkClassNum.NavigateUrl &= "?ClassID=" & .Item("ID").ToString
                        lblCourse.Text = .Item("Course").ToString
                        lblType.Text = .Item("Type").ToString
                        If Not IsNothing(IsDBNull(.Item("EndDate"))) AndAlso _
                        Not IsDBNull(.Item("StartDate")) _
                        AndAlso IsDate(.Item("StartDate")) _
                        AndAlso CDate(.Item("StartDate")).Equals(New Date(1900, 1, 1)) = False Then
                            lblStartDate.Text = CDate(.Item("StartDate")).ToShortDateString
                        Else
                            lblStartDate.Text = ""
                        End If
                        If Not IsNothing(IsDBNull(.Item("EndDate"))) AndAlso _
                        Not IsDBNull(.Item("EndDate")) _
                        AndAlso IsDate(.Item("EndDate")) _
                        AndAlso CDate(.Item("EndDate")).Equals(New Date(1900, 1, 1)) = False Then
                            lblEndDate.Text = CDate(.Item("EndDate")).ToShortDateString
                        Else
                            lblEndDate.Text = ""
                        End If

                        If CBool(.Item("ShowInstructorInfo")) Then
                            lblInstructor.Text = .Item("InstructorName").ToString
                        Else
                            trInstructor.Visible = False
                        End If
                        lblLocation.Text = .Item("School").ToString

                        p = CType(Me.AptifyApplication.GetEntityObject("Products", CLng(.Item("ProductID"))), Aptify.Applications.ProductSetup.ProductObject)

                        'HP Issue#8598:  Pricing rule is working correctly however the ProductObject.GetPrice method does not have an Order object 
                        '                which in turn would have the objects, i.e. BillToPerson, that are required in the Filter Rule therefore no price is returned
                        '                and no price is displayed. In this situation we will extract the price thru the ShoppingCart's GetUserProductPrice method
                        'Dim pr As New Aptify.Applications.OrderEntry.IProductPrice.PriceInfo
                        'If p.GetPrice(pr, CLng(.Item("ProductID")), 1, User1.PersonID, CurrencyTypeID:=User1.PreferredCurrencyTypeID) = Applications.OrderEntry.IProductPrice.PriceOutcome.Exist Then
                        '    lblPrice.Text = Format(pr.Price, User1.PreferredCurrencyFormat)
                        'End If
                        Dim price As Decimal = DirectCast(Me.ShoppingCart1.GetUserProductPrice(CLng(.Item("ProductID"))), Aptify.Applications.OrderEntry.IProductPrice.PriceInfo).Price
                        If price > 0 Then
                            lblPrice.Text = Format(price, User1.PreferredCurrencyFormat)
                        End If
                        'End Issue#8598

                    End With
                    ShowBillMeLater() 'RashmiP issue 6781, to show "Bill Me Later" option
                    Me.CreditCard.LoadCreditCardInfo() 'Added by Vijay Soni for Issue#5416
                Else
                    lblCourse.Text = "Error - No Course Found!"
                    Me.btnSaveRegistration.Visible = False
                    Me.CreditCard.Visible = False 'Added by Vijay Soni for Issue#5416
                End If
                'Added by Vijay Soni for Issue#5416	(Start)
                If Not IsNothing(lblProductID.Value) Then
                    If Not IsNothing(p.GetValue("ProductType")) AndAlso p.GetValue("ProductType").ToString.ToUpper = "CLASS" Then
                        m_ProductType = ProductType.ClassType
                        RegistrationGrid.Visible = True
                        btnSaveRegistrationPaid.Visible = False
                        CreaditCardInfo.Visible = False
                        lblInstructions.Text = "Please verify the information for the class below, if it is correct, hit ""Submit"" to continue your registration."
                        lblError.Visible = False
                        btnSaveRegistration.Visible = True
                    ElseIf Not IsNothing(p.GetValue("ProductType")) AndAlso p.GetValue("ProductType").ToString.ToUpper = "GENERAL" Then
                        m_ProductType = ProductType.GeneralType
                        RegistrationGrid.Visible = False
                        btnSaveRegistrationPaid.Visible = True
                        CreaditCardInfo.Visible = True
                        lblInstructions.Text = "Please verify the information for the class below, if it is correct, enter your credit card information below and hit ""Submit"" to complete your registration."
                        lblError.Visible = False
                        btnSaveRegistration.Visible = True
                    Else
                        RegistrationGrid.Visible = False
                        btnSaveRegistrationPaid.Visible = True
                        CreaditCardInfo.Visible = False
                        lblError.Visible = True
                        btnSaveRegistration.Visible = False
                        lblInstructions.Text = "<font color='red'>Product associated with class is not supported as class product, Please check the Product Type.</font>"
                        Throw New Exception("Product associated with class is not supported as class product, Please check the Product Type.")
                    End If
                End If

                'Added by Vijay Soni for Issue#5416	(End)

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub btnMoreRows_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMoreRows.Click
            Dim dt As DataTable

            Try
                dt = UpdateGetTable() ' copy grid to the datatable
                dt.Rows.Add(dt.NewRow)
                grdStudents.DataSource = dt
                grdStudents.DataBind()

                ViewState.Add("DataTable", dt)
                lblError.Visible = False
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Function UpdateGetTable() As DataTable
            Dim dt As DataTable
            Dim oText, oText2 As WebControls.TextBox
            Dim i As Integer
            Dim j As Integer

            Try
                dt = CType(ViewState.Item("DataTable"), DataTable)
                For i = 0 To dt.Rows.Count - 1
                    'For j = 1 To dt.Columns.Count - 1
                    '    'Navin Prasad Issue 11032
                    '    oText = CType(grdStudents.Rows(i).Cells(j).Controls(1), TextBox)

                    '    dt.Rows(i).Item(j) = oText.Text
                    'Next
                    If grdStudents.Items(i).Cells.Count > 0 Then
                        Dim iCount As Integer = grdStudents.Items(i).Cells.Count
                        Dim k As Integer = 1
                        For j = 2 To iCount - 2

                            oText2 = CType(grdStudents.Items(i).Cells(j).Controls(1), TextBox)
                            dt.Rows(i).Item(k) = oText2.Text
                            k += 1
                        Next
                    End If

                Next
                ViewState.Add("DataTable", dt)
                lblError.Visible = False
                Return dt
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return Nothing
            End Try
        End Function
        'Commented By   'Navin Prasad Issue 11032
        'Protected Sub grdStudents_DeleteCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grdStudents.DeleteCommand
        '    Dim dt As DataTable

        '    Try
        '        dt = UpdateGetTable()
        '        If dt.Rows.Count > 1 Then
        '            dt.Rows(e.Item.ItemIndex()).Delete()
        '            grdStudents.DataSource = dt
        '            grdStudents.DataBind()
        '            lblError.Visible = False
        '        Else
        '            Me.lblError.Text = "It is not possible to remove all lines from the registration."
        '            lblError.Visible = True
        '        End If
        '    Catch ex As Exception
        '        Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        '    End Try
        'End Sub


        ''' <summary>
        ''' RashmiP issue 6781, 09/27/10
        ''' Funtion set properties of credit card, if Company and User's credit Status is approved and credit limit is availabe 
        ''' contion check if payment type is Bill Me Later. 
        ''' </summary>
        Private Sub ShowBillMeLater()
            Dim iPOPaymentType As Integer
            Dim iPrevPaymentTypeID As Integer
            Dim sError As String
            Dim oOrder As Aptify.Applications.OrderEntry.OrdersEntity
            Try
                iPOPaymentType = CInt(AptifyApplication.GetEntityAttribute("Web Shopping Carts", "POPaymentTypeID"))
                Dim dr As Data.DataRow = User1.CompanyDataRow
                CreditCard.UserCreditStatus = CInt(User1.GetValue("CreditStatusID"))
                CreditCard.UserCreditLimit = CLng(User1.GetValue("CreditLimit"))
                If iPOPaymentType > 0 Then
                    If dr IsNot Nothing Then
                        CreditCard.CompanyCreditStatus = CInt(dr.Item("CreditStatusID"))
                        CreditCard.CompanyCreditLimit = CLng(dr.Item("CreditLimit"))
                    End If
                    oOrder = ShoppingCart1.GetOrderObject(Session, Page.User, Application)
                    If oOrder IsNot Nothing Then
                        iPrevPaymentTypeID = CInt(oOrder.GetValue("PayTypeID"))
                        oOrder.SetValue("PayTypeID", iPOPaymentType)
                        CreditCard.CreditCheckLimit = ShoppingCart1.CreditCheckObject.CheckCredit(CType(oOrder, Aptify.Applications.OrderEntry.OrdersEntity), sError)
                    End If
                End If
            Catch ex As Exception

            Finally
                If oOrder IsNot Nothing Then
                    oOrder.SetValue("PayTypeID", iPrevPaymentTypeID)
                End If

            End Try

        End Sub
        ''RashmiP, Issue 10287,9/22/11 Complete Order of Class Registration for Free Class
        Private Sub CompleteOrderforFreeClass()

            Dim lOrderID As Long, sError As String
            Dim iPOPaymentType As Integer
            Try
                If Not String.IsNullOrEmpty(AptifyApplication.GetEntityAttribute("Web Shopping Carts", "POPaymentTypeID")) Then
                    iPOPaymentType = CInt(AptifyApplication.GetEntityAttribute("Web Shopping Carts", "POPaymentTypeID"))
                Else
                    iPOPaymentType = 1
                End If
                With ShoppingCart1.GetOrderObject(Session, Page.User, Application)
                    .SetValue("PayTypeID", iPOPaymentType)
                    ShoppingCart1.SaveCart(Session)
                    lOrderID = ShoppingCart1.PlaceOrder(Session, Application, Page.User, sError)
                End With
                If lOrderID > 0 Then
                    Response.Redirect(OrderConfirmationPage & "?ID=" & lOrderID, False)
                End If


            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        'Protected Sub grdStudents_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdStudents.PageIndexChanging
        '    grdStudents.PageIndex = e.NewPageIndex
        '    SetupGrid()
        'End Sub

        'Navin Prasad Issue 11032
        'Protected Sub grdStudents_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdStudents.RowCommand
        '    If e.CommandName = "Delete" Then
        '        Dim dt As DataTable
        '        Try
        '            dt = UpdateGetTable()
        '            If dt.Rows.Count > 1 Then
        '                dt.Rows(CInt(e.CommandArgument)).Delete()
        '                grdStudents.DataSource = dt
        '                grdStudents.DataBind()
        '                lblError.Visible = False
        '            Else
        '                Me.lblError.Text = "It is not possible to remove all lines from the registration."
        '                lblError.Visible = True
        '            End If
        '        Catch ex As Exception
        '            Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        '        End Try
        '    End If
        'End Sub

        'Protected Sub grdStudents_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles grdStudents.RowDeleting

        'End Sub

        Protected Sub grdStudents_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grdStudents.ItemCommand
            Dim iLine As Integer = e.Item.ItemIndex
            If e.CommandName = "Delete" Then
                Dim dt As DataTable
                Try
                    dt = UpdateGetTable()
                    If dt.Rows.Count > 1 Then
                        dt.Rows(CInt(iLine)).Delete()
                        grdStudents.DataSource = dt
                        grdStudents.DataBind()
                        lblError.Visible = False
                    Else
                        Me.lblError.Text = "It is not possible to remove all lines from the registration."
                        lblError.Visible = True
                    End If
                Catch ex As Exception
                    Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                End Try
            End If
        End Sub

        Protected Sub grdStudents_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdStudents.NeedDataSource
            If ViewState("DataTable") IsNot Nothing Then
                grdStudents.DataSource = CType(ViewState("DataTable"), DataTable)
            End If
        End Sub

    End Class
End Namespace

