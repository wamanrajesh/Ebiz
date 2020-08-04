'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports System.Data
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Applications.OrderEntry

Namespace Aptify.Framework.Web.eBusiness.Chapters
    Partial Class AddMembersControl
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "AddMembers"

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
            Try
                If Not IsPostBack Then
                    ''New Function created by Suvarna D for IssueID 12436 on Dec 1, 2011
                    ''To support paging separate grid bind function has been created
                    LoadAddMember()
                    'End of Addition IssueID: 12436
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try


        End Sub


        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(Me.RedirectURL) Then
                Me.cmdSubmit.Enabled = False
                Me.cmdSubmit.ToolTip = "RedirectURL property has not been set."
            End If

            'if values are not provide directly or from the XML file, set default values for inherited properties since 
            'control requires them to properly function
            If String.IsNullOrEmpty(Me.QueryStringRecordIDParameter) Then Me.QueryStringRecordIDParameter = "ID"
            If String.IsNullOrEmpty(Me.RedirectIDParameterName) Then Me.RedirectIDParameterName = "ReportID"

        End Sub

        Private Sub cmdAddRow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddRow.Click
            Dim dt As DataTable

            Try
                dt = UpdateGetTable() ' copy grid to the datatable
                dt.Rows.Add(dt.NewRow)
                grdMembers.DataSource = dt
                grdMembers.DataBind()
                ViewState.Add("DataTable", dt)
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub


        Private Function UpdateGetTable() As DataTable
            Dim dt As DataTable
            Dim oText As WebControls.TextBox
            Dim i As Integer
            Dim j, k As Integer

            Try
                dt = CType(ViewState.Item("DataTable"), DataTable)
                For i = 0 To dt.Rows.Count - 1
                    Dim iCount As Integer = grdMembers.Items(i).Cells.Count
                    k = 0
                    For j = 2 To iCount - 2
                        oText = CType(grdMembers.Items(i).Cells(j).Controls(1), TextBox)
                        dt.Rows(i).Item(k) = oText.Text
                        k += 1
                    Next
                Next
                ViewState.Add("DataTable", dt)
                Return dt
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return Nothing
            End Try
        End Function

        Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
            ' Process the new members, by processing the order on the 
            ' system. This is temporary code that should be updated for
            ' each client for their appropriate logic for chapter member
            ' adds
            Dim oOrder As OrdersEntity
            Dim oPerson As AptifyGenericEntityBase
            Dim lChapterID As Long
            Dim dt As DataTable
            Dim dt1 As DataTable
            Dim sSQL As String
            Dim i As Integer, j As Integer

            Try
                'If Not IsNumeric(Me.ControlRecordID) Then
                '    Throw New ArgumentException("Parameter must be numeric.", "ID")
                'End If
                lChapterID = ControlRecordID
                oOrder = DirectCast(ShoppingCart1.GetNewOrderObject(), OrdersEntity)
                oPerson = AptifyApplication.GetEntityObject("Persons", -1)

                'If this chapter has any Chapter Membership Products associated with it,
                'we should create an order automatically.  First, run query to make 
                'that determination.  If there are products, then create the order and 
                'add a line for each product.  RN May 20, 2003.
                sSQL = "SELECT ProductID FROM " & Database & ".." & _
                        "vwChapterMembershipProducts WHERE ChapterID = " & lChapterID
                dt1 = DataAction.GetDataTable(sSQL)
                'Suraj Issue 15154,02/01/2013. boolean variable check the data is valid ot not
                'Suraj Issue 15154,02/15/2013 after creating person record check person  link to the order so bIsSetupOrderDetails boolean variable check it
                Dim bIsValidData As Boolean = True
                Dim bIsSetupOrderDetails As Boolean = True
                If dt1.Rows.Count > 0 Then
                    SetupOrderHeader(oOrder, lChapterID)
                    dt = UpdateGetTable()
                    'Suraj Issue 15154,02/01/2013.ValidateData function  check the data is valid ot not if valid it returns true else false
                    bIsValidData = ValidateData(dt)
                    'Suraj Issue 15154,02/01/2013.if the data is valid perform next operation
                    If bIsValidData Then
                        For i = 0 To dt.Rows.Count - 1
                            ' Instead of a create person function, we use FindCreatePerson
                            ' to either locate or create the person record as necessary.
                            ' - Richard Bowman - 7/1/2003
                            FindCreatePerson(oPerson, dt.Rows(i))
                            For j = 0 To dt1.Rows.Count - 1
                                bIsSetupOrderDetails = SetupOrderDetails(oOrder, oPerson, lChapterID, CLng(dt1.Rows(j).Item("ProductID")))
                            Next j
                        Next i
                        If bIsSetupOrderDetails Then
                            oOrder.SetValue("OrderStatusID", "1")
                            oOrder.SetValue("OrderLevelID", User1.GetValue("GLOrderLevelID"))
                            '8/30/06 RJK - Force a Purchase Order.
                            oOrder.SetValue("PayTypeID", 1)
                            oOrder.SetValue("PONumber", "Add Chapter Members")

                            If oOrder.Save(False) Then
                                oOrder.ShipEntireOrder(False)

                                ' get a list of all subscriptions that are linked to this
                                ' order and manually link them to this chapter. This is
                                ' a temporary block of code as the next rev of the Order
                                ' Object will automatically flow down the Chapter ID from
                                ' the OrderDetail to the Subscription record
                                UpdateSubscriptions(oOrder.RecordID, lChapterID)
                                lblError.Visible = False
                            Else
                                lblError.Visible = True
                                If Len(oOrder.LastError) > 0 Then
                                    lblError.Text = oOrder.LastError()
                                Else
                                    ' I know this isn't spectacular, but at least it lets the
                                    ' user know that an error occured, unlike before when GetLastError()
                                    ' was returning a blank string.
                                    '   Author: Richard Bowman               Date:   6/30/2003
                                    lblError.Text = "An unknown error has occured when processing this request."
                                End If
                            End If
                        Else
                            lblError.Visible = True
                            lblError.Text = "An unknown error has occured when processing this request."
                        End If

                    End If
                Else
                    'No products associated with this chapter ... no order to create.
                    'Ravi Nagarajan, May 20, 2003.
                    dt = UpdateGetTable()
                    'Suraj Issue 15154,02/01/2013.ValidateData function  check the data is valid ot not if valid it returns true else false
                    bIsValidData = ValidateData(dt)
                    'Suraj Issue 15154,02/01/2013. if bIsValidData true then perform next operation
                    If bIsValidData Then
                        For i = 0 To dt.Rows.Count - 1
                            FindCreatePerson(oPerson, dt.Rows(i))
                        Next
                    End If
                    'lblError.Visible = False
                End If

                ' If an error has been set in the code above to be displayed to the user
                ' we need to not redirect so the user can actually see it and respond
                ' to it.
                ' - Richard Bowman - 6/30/2003
                trSuccess.Visible = Not lblError.Visible
                trAddMembers.Visible = lblError.Visible
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub UpdateSubscriptions(ByVal OrderID As Long, _
                                        ByVal ChapterID As Long)
            Dim sSQL As String
            Dim dt As DataTable
            Dim oSub As AptifyGenericEntityBase
            Dim i As Integer

            Try
                sSQL = "SELECT DISTINCT(SubscriptionID) ID FROM " & _
                               Database & "..vwSubscriptionPurchases " & _
                               " WHERE OrderID=" & OrderID
                dt = DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                For i = 0 To dt.Rows.Count - 1
                    oSub = AptifyApplication.GetEntityObject("Subscriptions", CLng(dt.Rows(i).Item(0)))
                    oSub.SetValue("ChapterID", ChapterID)
                    oSub.Save(False)
                Next
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Protected Overridable Function SetupOrderDetails(ByVal OrderGE As OrdersEntity, _
                                      ByVal PersonGE As AptifyGenericEntityBase, _
                                      ByVal ChapterID As Long, _
                                      ByVal ProductID As Long) As Boolean
            Try
                ' create a person record and then link to the order
                If PersonGE.RecordID > 0 Then
                    Dim geOrderLines As System.Collections.Generic.List(Of OrderLinesEntity) = OrderGE.AddProduct(ProductID, 1)

                    If geOrderLines Is Nothing OrElse geOrderLines.Count = 0 Then

                    Else
                        With geOrderLines(0)
                            .SetValue("Description", "Membership: " & _
                                                     CStr(PersonGE.GetValue("FirstName")) & " " & _
                                                     CStr(PersonGE.GetValue("LastName")))

                            .SetValue("SubscriberID", PersonGE.RecordID)
                            .SetValue("ChapterID", ChapterID)
                        End With

                    End If
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return False
            End Try

        End Function

        Private Sub SetupOrderHeader(ByRef oOrder As OrdersEntity, _
                                     ByVal lChapterID As Long)


            Try
                oOrder.SetValue("ShipToID", User1.PersonID)
                oOrder.SetValue("BillToID", User1.PersonID)
                oOrder.SetValue("ShipToCompanyID", lChapterID)
                oOrder.SetValue("BillToCompanyID", lChapterID)
                oOrder.SetValue("EmployeeID", Me.EBusinessGlobal.WebEmployeeID(Page.Application))
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        'Suraj Issue 15154,02/01/2013.ValidateData function  check the data is valid ot not if valid it returns true else false
        Private Function ValidateData(ByVal dt As DataTable) As Boolean

            Try
                Dim bIsValidData As Boolean = True
                For i = 0 To dt.Rows.Count - 1
                    'Suraj Issue 15154,02/01/2013.thi s condition check the first name and last name is empty nor null 
                    'Suraj Issue 15154,03/21/2013. here we change the condition check the first name and last name is empty nor null  if any one both of them is empty or null it will through the error message 
                    If String.IsNullOrEmpty(CStr(dt.Rows(i).Item("FirstName"))) Or String.IsNullOrEmpty(CStr(dt.Rows(i).Item("LastName"))) Or CStr(dt.Rows(i).Item("LastName")).Length = 0 Or CStr(dt.Rows(i).Item("FirstName")).Length = 0 Then
                        bIsValidData = False
                        lblError.Visible = True
                        lblError.Text = "Blank value is not ok for First Name. Blank value is not ok for Last Name."
                        Exit For
                    ElseIf Not String.IsNullOrEmpty(CStr(dt.Rows(i).Item("Email"))) Then
                        'Suraj Issue 15154,02/01/2013.thi s EmailAddressCheck function  check the email is valid or not
                        bIsValidData = CommonMethods.EmailAddressCheck(CStr(dt.Rows(i).Item("Email")))
                        If bIsValidData = False Then
                            lblError.Text = "One or more members have incorrect E-Mail address."
                            lblError.Visible = True
                            Exit For
                        End If
                    End If
                Next i
                Return bIsValidData
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return False
            End Try
        End Function

        ''' <summary>
        ''' This function searches for an existing person using the first and last
        ''' name entered. If found, it returns the entity representing this person.
        ''' If not, a new entity is created and populated with the appropriate
        ''' data fields.
        ''' </summary>
        ''' <param name="PersonGE">AptifyGenericEntityBase loaded with a Person record</param>
        ''' <param name="dr">DataRow representing person's information</param>
        ''' <returns>Boolean    : True, on success</returns>
        ''' <remarks></remarks>

        Protected Overridable Function FindCreatePerson(ByVal PersonGE As AptifyGenericEntityBase, _
                                                        ByVal dr As DataRow) As Boolean
            Dim sSQL As String
            Dim dt As DataTable

            Try
                ' The previous code in this function (it was called CreatePerson
                ' then) automatically assumed we should build a new Persons
                ' record. However, if that person already exists, it failed when
                ' the duplicate person check occured. The correct functionality
                ' is to search for an existing person to use before creating a
                ' new record.

                sSQL = "SELECT ID FROM " & Database & _
                       "..vwPersons WHERE FirstName = '" & CStr(dr.Item("FirstName")).Replace("'", "''") & _
                       "' AND LastName='" & CStr(dr.Item("LastName")).Replace("'", "''") & "' AND Email1='" & CStr(dr.Item("Email")).Replace("'", "''") & "'"

                dt = DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                If dt.Rows.Count > 0 Then
                    ' We've got a match on the name
                    PersonGE.Load(CLng(dt.Rows(0).Item(0)))
                    PersonGE.SetValue("Title", dr.Item("Title"))
                    Return PersonGE.Save(False)
                Else
                    PersonGE.NewRecord()
                    PersonGE.SetValue("FirstName", dr.Item("FirstName"))
                    PersonGE.SetValue("LastName", dr.Item("LastName"))
                    PersonGE.SetValue("Title", dr.Item("Title"))
                    PersonGE.SetValue("Email1", dr.Item("Email"))
                    'Return PersonGE.Save(False)
                    Dim sError As String = String.Empty
                    If PersonGE.Save(False, sError) Then
                        'Suraj Issue 15154, 01/01/2013 if the record save it return true
                        lblError.Visible = False
                        Return True
                    Else
                        'Suraj Issue 15154 ,01/01/2013 if the record not saved it return false also get the last error message 
                        lblError.Visible = True
                        If String.IsNullOrEmpty(sError) Then
                            lblError.Text = PersonGE.LastError()
                        Else
                            lblError.Text = sError
                        End If
                        Return False
                    End If
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            Finally
                dt = Nothing
            End Try
        End Function

        Private Sub btnSuccess_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSuccess.Click
            Me.RedirectUsingPropertyValues(Me.ControlRecordID)
        End Sub



        ''New Function created by Suvarna D for IssueID 12436 on Dec 1, 2011
        ''To support paging separate grid bind function has been created
        Protected Sub LoadAddMember()
            Try
                If ViewState("DataTable") IsNot Nothing Then
                    grdMembers.DataSource = CType(ViewState("DataTable"), DataTable)
                    grdMembers.DataBind()
                    Exit Sub
                End If

                If (Me.SetControlRecordIDFromQueryString AndAlso _
                            Me.SetControlRecordIDFromParam() AndAlso _
                            Me.ControlRecordID > 0) _
                            OrElse Me.IsPageInAdmin() Then
                    lblChapterName.Text = AptifyApplication.GetEntityRecordName("Companies", CLng(Me.ControlRecordID))

                    Dim dt As New DataTable()
                    dt.Columns.Add("FirstName")
                    dt.Columns.Add("LastName")
                    dt.Columns.Add("Title")
                    dt.Columns.Add("Email")

                    dt.Rows.Add(dt.NewRow)

                    grdMembers.DataSource = dt
                    grdMembers.DataBind()

                    ViewState.Add("DataTable", dt)

                    ' ensure Success Row is not visible yet
                    trSuccess.Visible = False
                Else
                    'Me.tblMain.Visible = False
                    Me.lblErrorMain.Text = "Error Loading Chapter Data"
                    lblErrorMain.Visible = True
                    Throw New ArgumentException("Parameter must be numeric.", "ID")
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        ''Rashmi P, Issue 14448
        Protected Sub grdMembers_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grdMembers.ItemCommand

            If e.CommandName = "Delete" Then
                Dim index As Integer = e.Item.ItemIndex
                Dim dt As DataTable
                Try
                    dt = UpdateGetTable()
                    dt.Rows(index).Delete()
                    grdMembers.DataSource = dt
                    grdMembers.DataBind()
                Catch ex As Exception
                    Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                End Try
            End If
        End Sub

        Protected Sub grdMembers_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdMembers.NeedDataSource
            If ViewState("DataTable") IsNot Nothing Then
                grdMembers.DataSource = CType(ViewState("DataTable"), DataTable)
            End If
        End Sub

        Protected Sub grdMembers_PageIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageChangedEventArgs) Handles grdMembers.PageIndexChanged
            grdMembers.CurrentPageIndex = e.NewPageIndex
            If ViewState("DataTable") IsNot Nothing Then
                grdMembers.DataSource = CType(ViewState("DataTable"), DataTable)
            End If
        End Sub
    End Class
End Namespace
