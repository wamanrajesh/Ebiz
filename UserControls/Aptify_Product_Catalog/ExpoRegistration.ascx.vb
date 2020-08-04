'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports System.Data
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports Aptify.Applications.OrderEntry
Imports Aptify.Framework.DataServices



Namespace Aptify.Framework.Web.eBusiness.ProductCatalog
    Partial Class ExpoRegistrationControl
        Inherits BaseUserControlAdvanced

        Protected Const ATTRIBUTE_SAVE_BUTTON_REDIRECT As String = "SaveButtonRedirect"
        Protected Const ATTRIBUTE_CONTORL_DEFAULT_NAME As String = "ExpoRegistration"
        ''ISSUEID 3240 - Added SuvarnaD  
        Protected Const ATTRIBUTE_ALERT_DUPLICATEPERSONVALIDATION As String = "DuplicatePersonValidation"
        Protected m_sAlert As String = String.Empty

#Region "ExpoRegistration Specific Properties"
        ''' <summary>
        ''' SaveButtonRedirect page url
        ''' </summary>
        Public Overridable Property SaveButtonRedirect() As String
            Get
                If Not ViewState(ATTRIBUTE_SAVE_BUTTON_REDIRECT) Is Nothing Then
                    Return CStr(ViewState(ATTRIBUTE_SAVE_BUTTON_REDIRECT))
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState(ATTRIBUTE_SAVE_BUTTON_REDIRECT) = Me.FixLinkForVirtualPath(value)
            End Set
        End Property

        ''' <summary>
        ''' ''' ''ISSUEID 3240 - Added SuvarnaD  
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>

        Public Overridable Property DuplicatePersonValidation() As String
            Get
                Return m_sAlert
            End Get
            Set(ByVal value As String)
                m_sAlert = value
            End Set
        End Property
#End Region

        Protected Overrides Sub SetProperties()

            If String.IsNullOrEmpty(Me.ID) Then Me.ID = ATTRIBUTE_CONTORL_DEFAULT_NAME
            'call base method to set parent properties
            MyBase.SetProperties()

            If String.IsNullOrEmpty(SaveButtonRedirect) Then
                'since value is the 'default' check the XML file for possible custom setting
                SaveButtonRedirect = Me.GetLinkValueFromXML(ATTRIBUTE_SAVE_BUTTON_REDIRECT)
                If String.IsNullOrEmpty(SaveButtonRedirect) Then
                    Me.cmdSave.Enabled = False
                    Me.cmdSave.ToolTip = "SaveButtonRedirect property has not been set."
                End If
            End If

            ''ISSUEID 3240 - Added SuvarnaD  
            If String.IsNullOrEmpty(DuplicatePersonValidation) Then
                DuplicatePersonValidation = Me.GetPropertyValueFromXML(ATTRIBUTE_ALERT_DUPLICATEPERSONVALIDATION)
                If Not String.IsNullOrEmpty(DuplicatePersonValidation) Then
                    lblAlert.Text = DuplicatePersonValidation
                End If
            End If
        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'set control properties from XML file if needed
            SetProperties()
            If Not IsPostBack Then
                'Suraj Issue 15012, 2/25/13, Get a Product Name
                If Not Session("ProductID") Is Nothing AndAlso CLng(Session("ProductID")) > 0 Then
                    ProductName(CLng(Session("ProductID")))
                End If
                'Anil B For Issue 15012
                EnableDisablePhoneValidation()
                If Len(Request.QueryString("OL")) > 0 AndAlso _
                   Not IsNumeric(Request.QueryString("OL")) Then
                    Throw New ArgumentException("Parameter must be numeric.", "OL")
                End If
                If Len(Request.QueryString("OL")) > 0 Then
                    LoadExpoInfo()
                End If
            End If
        End Sub

        Private Function LoadExpoInfo() As Boolean
            Try
                'ASSUMPTION - This page is called from a URL that has the
                '             order line in it that is needing editing
                Dim oExpoDetail As AptifyGenericEntityBase
                Dim lProductID As Long
                Dim iLine As Integer
                Dim sExpoObjectData As String
                Dim oOrder As AptifyGenericEntityBase
                Dim oOrderLine As AptifyGenericEntityBase
                Dim bOrderlineFound As Boolean = False 'Anil Issue 15012
                Me.lblBoothQty.Text = "1" 'reset quantity



                Dim iItem As Integer, iItemForUpdate As Integer
                iLine = CInt(Request.QueryString("OL"))
                oOrder = ShoppingCart1.GetOrderObject(Me.Session, Me.Page.User, Me.Application)

                'Anil Issue 12660

                If Session("ProductID") IsNot Nothing AndAlso CLng(Session("ProductID")) > -1 Then
                    iItemForUpdate = 0
                    For iItem = 0 To oOrder.SubTypes("OrderLines").Count - 1
                        If CLng(oOrder.SubTypes("OrderLines").Item(iItem).GetValue("ProductID")) = CLng(Session("ProductID")) AndAlso _
                           CLng(oOrder.SubTypes("OrderLines").Item(iItem).GetValue("ParentSequence")) <= 0 Then
                            bOrderlineFound = True
                            iItemForUpdate = iItem
                        End If
                    Next
                End If
                'Anil B For issue 15012
                If bOrderlineFound Then
                    ViewState("ILine") = iItemForUpdate
                Else
                    iItemForUpdate = iLine
                    ViewState("ILine") = iLine
                End If

                oOrderLine = oOrder.SubTypes("OrderLines").Item(iItemForUpdate)
                lProductID = CLng(oOrderLine.GetValue("ProductID"))
                LoadBoothList(lProductID)

                Dim oPrice As IProductPrice.PriceInfo
                oPrice = ShoppingCart1.GetUserProductPrice(lProductID)
                lblPrice.Text = Format(oPrice.Price, ShoppingCart1.GetCurrencyFormat(oPrice.CurrencyTypeID))
                lblUnitPrice.Text = oPrice.Price.ToString
                Dim iQuantity As Integer = CInt(oOrderLine.GetValue("Quantity"))
                ViewState.Add("ProductQuantity", iQuantity)

                If "Expo" = ShoppingCart1.GetProductType(lProductID) Then
                    sExpoObjectData = CStr(oOrderLine.GetValue("__ExtendedAttributeObjectData"))
                    oExpoDetail = AptifyApplication.GetEntityObject("OrderBoothDetail", -1)
                    If Len(sExpoObjectData) > 0 Then
                        LoadExpoDetailFromXML(oExpoDetail, sExpoObjectData)
                    End If

                    ' load up the data in the page from the oExpoDetail object
                    LoadExpoDetail(oExpoDetail)
                    oExpoDetail = Nothing
                Else
                    lblError.Text = "The selected line does not exist or is not a expo product"
                    lblError.Visible = True
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Function
        Private Sub LoadExpoDetailFromXML(ByVal ExpoDetailGE As AptifyGenericEntityBase, _
                                          ByVal ExpoObjectXMLData As String)
            Try
                ExpoDetailGE.Load("|" & ExpoObjectXMLData)
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Sub LoadBoothList(ByVal ProductID As Long)
            Try
                Dim sSQL As String
                Dim dt1 As DataTable = New DataTable
                Dim dt2 As DataTable

                'HP - Issue 8243...Initialization will take place here so datatable is ready for use when sql result is null
                dt1.Columns.Add(New DataColumn("ID", GetType(Long)))
                dt1.Columns.Add(New DataColumn("Name", GetType(String)))

                'Issue 4345 MAS
                'Changed SQL to only display booths from a floorplan that has a status of "Assign" (2 or 3)
                'Also now retrieve Surcharge information
                sSQL = "SELECT ID, Name FROM " & _
                       Database & _
                       "..vwBooths " & _
                       "WHERE FloorplanID IN(SELECT FloorplanID FROM " & _
                       Database & _
                       "..vwExpoFloorplans ef " & _
                       "INNER JOIN " & _
                       Database & _
                       "..vwExpos e ON ef.ExpoID=e.ID " & _
                       "INNER JOIN " & _
                       Database & _
                       "..vwFloorplans fp ON ef.FloorplanID=fp.ID " & _
                       "WHERE e.ProductID=" & ProductID & _
                       " AND (fp.FloorplanStatusID=2" & _
                       " OR fp.FloorplanStatusID=3)) " & _
                       "AND IsOccupied=0 " & _
                       "ORDER BY Name"

                dt2 = DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)

                If dt2 IsNot Nothing AndAlso dt2.Rows.Count > 0 Then
                    'HP - Issue 8243...Moved initialization to begining of method call to make available throughout 
                    'dt1.Columns.Add(New DataColumn("ID", GetType(Long)))
                    'dt1.Columns.Add(New DataColumn("Name", GetType(String)))

                    Dim dr1 As DataRow
                    dr1 = dt1.Rows.Add
                    dr1.Item("ID") = 0
                    dr1.Item("Name") = "Select a booth"
                    For Each dtRow As DataRow In dt2.Rows
                        dr1 = dt1.Rows.Add
                        dr1.Item("ID") = dtRow.Item("ID")
                        dr1.Item("Name") = dtRow.Item("Name")
                    Next
                    cmbBooth.DataTextField = "Name"
                    cmbBooth.DataValueField = "ID"
                    cmbBooth.DataSource = dt1
                    cmbBooth.DataBind()
                    Me.lblBooth.Visible = False 'Booth selection not required at time of order
                    'sSQL = "SELECT ID,Name,SurchargeType,Description,PreferredBoothSurcharge,SurchargePercent,NumUnits FROM " & _
                    '     Database & "..vwBooths " & _
                    '     "WHERE ID=" & dtRow.Item("ID")
                Else
                    'No Booths available for selection
                    Dim dr1 As DataRow
                    dr1 = dt1.Rows.Add
                    dr1.Item("ID") = 0
                    dr1.Item("Name") = "No Booths Available"
                    cmbBooth.DataTextField = "Name"
                    cmbBooth.DataValueField = "ID"
                    cmbBooth.DataSource = dt1
                    cmbBooth.DataBind()
                    Me.lblBooth.Visible = False
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Private Function LoadExpoDetail(ByVal ExpoDetailGE As AptifyGenericEntityBase) As Boolean
            Try
                Dim sSQL As String
                Dim lID As Long
                Dim dt As DataTable = Nothing

                lID = CLng(ExpoDetailGE.GetValue("ExhibitorID"))
                If lID > 0 Then
                    sSQL = "SELECT Name FROM " & _
                           Database & _
                           "..vwCompanies WHERE ID=" & lID
                    txtExhibitor.Text = CStr(DataAction.ExecuteScalar(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache))
                    lblExhibitorID.Text = CStr(lID)
                Else
                    txtExhibitor.Text = User1.Company
                    lblExhibitorID.Text = CStr(User1.CompanyID)
                End If
                Me.lblExhibitorName.Text = Me.txtExhibitor.Text

                lID = CLng(ExpoDetailGE.GetValue("ContactID"))
                If lID > 0 Then
                    'Code Commented and added by Suvarna for IssueId 3240
                    'sSQL = "SELECT FirstLast FROM " & _
                    '       Database & _
                    '       "..vwPersons WHERE ID=" & lID
                    'txtContact.Text = CStr(DataAction.ExecuteScalar(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache))
                    sSQL = "SELECT FirstName,LastName,Email1,FirstLast FROM " & _
                           Database & _
                           "..vwPersons WHERE ID=" & lID
                    dt = DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                    txtPCFName.Text = Convert.ToString(dt.Rows(0)("FirstName"))
                    txtPCLName.Text = Convert.ToString(dt.Rows(0)("LastName"))
                    txtPCEmail.Text = Convert.ToString(dt.Rows(0)("Email1"))
                    lblContactID.Text = CStr(lID)
                Else
                    'Code Commented and added by Suvarna for IssueId 3240
                    'txtContact.Text = User1.GetValue("FirstName") & "" & User1.GetValue("LastName")
                    txtPCFName.Text = User1.GetValue("FirstName")
                    txtPCLName.Text = User1.GetValue("LastName")
                    txtPCEmail.Text = User1.GetValue("Email1")
                    lblContactID.Text = CStr(User1.PersonID)
                End If
                Me.lblContactName.Text = txtPCFName.Text.Trim & "" & txtPCLName.Text.Trim
                Me.lblPrimaryEmail.Text = txtPCEmail.Text.Trim

                lID = CLng(ExpoDetailGE.GetValue("SecondaryContactID"))
                If lID > 0 Then
                    'Code Commented and added by Suvarna for IssueId 3240
                    'sSQL = "SELECT FirstLast FROM " & _
                    '       Database & _
                    '       "..vwPersons WHERE ID=" & lID
                    'txtSecondaryContact.Text = CStr(DataAction.ExecuteScalar(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache))
                    sSQL = "SELECT FirstName,LastName,Email1,FirstLast FROM " & _
                           Database & _
                           "..vwPersons WHERE ID=" & lID
                    dt = DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)
                    txtSCFName.Text = Convert.ToString(dt.Rows(0)("FirstName"))
                    txtSCLName.Text = Convert.ToString(dt.Rows(0)("LastName"))
                    txtSCEmail.Text = Convert.ToString(dt.Rows(0)("Email1"))
                    lblSecondaryContactID.Text = CStr(lID)
                End If
                Me.lblSecondaryContactName.Text = txtSCFName.Text.Trim & "" & txtSCLName.Text.Trim
                Me.lblSecondaryEmail.Text = txtSCEmail.Text.Trim
                txtAreaCode.Text = CStr(ExpoDetailGE.GetValue("AreaCode"))
                txtTelephone.Text = CStr(ExpoDetailGE.GetValue("PhoneNumber"))
                txtWeightRequired.Text = CStr(ExpoDetailGE.GetValue("WeightRequired"))
                chkElectric.Checked = CBool(IIf(Val(ExpoDetailGE.GetValue("NeedsElectric")) = 0, False, True))
                chkWater.Checked = CBool(IIf(Val(ExpoDetailGE.GetValue("NeedsWater")) = 0, False, True))
                chkGas.Checked = CBool(IIf(Val(ExpoDetailGE.GetValue("NeedsGas")) = 0, False, True))
                chkAir.Checked = CBool(IIf(Val(ExpoDetailGE.GetValue("NeedsCompressedAir")) = 0, False, True))
                chkDrain.Checked = CBool(IIf(Val(ExpoDetailGE.GetValue("NeedsDrain")) = 0, False, True))
                ViewState("BoothID") = CLng(ExpoDetailGE.GetValue("BoothID"))
                If Val(ExpoDetailGE.GetValue("BoothID")) > 0 Then
                    Dim i As Integer
                    For i = 0 To cmbBooth.Items.Count - 1
                        If cmbBooth.Items.Item(i).Value = _
                          CStr(ExpoDetailGE.GetValue("BoothID")) Then
                            cmbBooth.SelectedIndex = i
                            'Update Price with the selected Booth
                            'calling this event will cause the Price to be set
                            cmbBooth_SelectedIndexChanged(Me, New EventArgs)
                            Exit For
                        End If
                    Next
                End If
                'Suraj Issue 15012, 3/4/13, if the product name is not null or empty then assing value to a lable of product
                If Not String.IsNullOrEmpty(CStr((ExpoDetailGE.GetValue("_XProductName")))) Then
                    lblProductName.Text = CStr(ExpoDetailGE.GetValue("_XProductName"))
                End If
                txtBoothName.Text = CStr(ExpoDetailGE.GetValue("BoothNameAlpha"))
                txtBoothDescription.Text = CStr(ExpoDetailGE.GetValue("Description"))
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Function

        Private Sub cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave.Click
            Try
                'First validate all required fields have been entered
                If ValidateOrderBoothDetail() Then
                    Dim oExpoDetail As AptifyGenericEntityBase
                    Dim iLine As Integer
                    Dim sData As String
                    'Code added by Suvarna for IssueID - 3240
                    Dim sPrimaryContactName As String = String.Empty
                    Dim sSecondaryContactName As String = String.Empty

                    Dim lProductID As Long
                    Dim oOrderLine As OrderLinesEntity
                    Dim oOrder As OrdersEntity
                    Dim sSourcePage As String
                    Dim sCheckoutPage As String
                    sSourcePage = CStr(Request.QueryString("PrevPage"))
                    iLine = CInt(ViewState("ILine"))
                    oOrder = ShoppingCart1.GetOrderObject(Page.Session, Page.User, Page.Application)


                    'Anil Issue 12660
                    oOrderLine = CType(oOrder.SubTypes("OrderLines").Item(iLine), OrderLinesEntity)
                    lProductID = CLng(oOrderLine.GetValue("ProductID"))

                    oExpoDetail = CType(AptifyApplication.GetEntityObject("OrderBoothDetail", -1), ExtendedOrderDetailGE)
                    sData = CStr(oOrder.SubTypes("OrderLines").Item(iLine).GetValue("__ExtendedAttributeObjectData"))
                    If Len(sData) > 0 Then
                        LoadExpoDetailFromXML(oExpoDetail, sData)
                    End If

                    If Me.txtExhibitor.Text <> Me.lblExhibitorName.Text Then
                        'The ExhibitorID has not yet been set
                        lblExhibitorID.Text = CStr(User1.FindCreateCompany(txtExhibitor.Text))
                    End If
                    oExpoDetail.SetValue("ExhibitorID", lblExhibitorID.Text)

                    'Code Commented and added by Suvarna for IssueID - 3240
                    'If Me.txtContact.Text <> Me.lblContactName.Text Then
                    '    lblContactID.Text = CStr(User1.FindCreatePerson(txtContact.Text))
                    'End If
                    'oExpoDetail.SetValue("ContactID", lblContactID.Text)

                    'If Me.txtSecondaryContact.Text <> Me.lblSecondaryContactName.Text Then
                    '    lblSecondaryContactID.Text = CStr(User1.FindCreatePerson(txtSecondaryContact.Text))
                    'End If
                    sPrimaryContactName = txtPCFName.Text.Trim & "" & txtPCLName.Text.Trim
                    sSecondaryContactName = txtSCFName.Text.Trim & "" & txtSCLName.Text.Trim
                    'Anil B for issue 15012 add condition if email is change
                    If sPrimaryContactName <> Me.lblContactName.Text OrElse lblPrimaryEmail.Text <> txtPCEmail.Text Then
                        If Not ValidatePerson(txtPCFName.Text.Trim, txtPCLName.Text.Trim, txtPCEmail.Text.Trim) Then
                            radDuplicateUser.VisibleOnPageLoad = True
                            Exit Sub
                        Else
                            lblContactID.Text = CStr(User1.FindCreatePerson(txtPCFName.Text.Trim & "/" & txtPCLName.Text.Trim & "/" & txtPCEmail.Text.Trim))
                        End If

                    End If
                    oExpoDetail.SetValue("ContactID", lblContactID.Text)
                    'Anil B for issue 15012 add condition if email is change
                    'Suraj Issue 15012, 2/28/13, check if the secondary Fname,Lname and email is null or empty then assign -1 for lblSecondaryContactID.Text because  FindCreatePerson not return -1 it returns 1 
                    If Not String.IsNullOrEmpty(txtSCFName.Text) AndAlso Not String.IsNullOrEmpty(txtSCFName.Text) AndAlso Not String.IsNullOrEmpty(txtSCFName.Text) Then
                        If sSecondaryContactName <> Me.lblSecondaryContactName.Text OrElse lblSecondaryEmail.Text <> txtSCEmail.Text Then
                            If Not ValidatePerson(txtSCFName.Text.Trim, txtSCLName.Text.Trim, txtSCEmail.Text.Trim) Then
                                radDuplicateUser.VisibleOnPageLoad = True
                                Exit Sub
                            Else
                                lblSecondaryContactID.Text = CStr(User1.FindCreatePerson(txtSCFName.Text.Trim & "/" & txtSCLName.Text.Trim & "/" & txtSCEmail.Text.Trim))
                            End If
                        End If
                    Else
                        lblSecondaryContactID.Text = "-1"
                    End If

                    oExpoDetail.SetValue("SecondaryContactID", lblSecondaryContactID.Text)

                    oExpoDetail.SetValue("AreaCode", txtAreaCode.Text)
                    oExpoDetail.SetValue("PhoneNumber", txtTelephone.Text)
                    oExpoDetail.SetValue("BoothNameAlpha", txtBoothName.Text)
                    oExpoDetail.SetValue("Description", txtBoothDescription.Text)
                    oExpoDetail.SetValue("WeightRequired", txtWeightRequired.Text)

                    If cmbBooth.SelectedItem IsNot Nothing AndAlso CInt(cmbBooth.SelectedValue) > 0 Then
                        oExpoDetail.SetValue("BoothID", cmbBooth.SelectedValue)
                    Else
                        oExpoDetail.SetValue("BoothID", -1)
                    End If

                    oExpoDetail.SetValue("NeedsElectric", IIf(chkElectric.Checked, 1, 0))
                    oExpoDetail.SetValue("NeedsGas", IIf(chkGas.Checked, 1, 0))
                    oExpoDetail.SetValue("NeedsWater", IIf(chkWater.Checked, 1, 0))
                    oExpoDetail.SetValue("NeedsDrain", IIf(chkDrain.Checked, 1, 0))
                    oExpoDetail.SetValue("NeedsCompressedAir", IIf(chkAir.Checked, 1, 0))
                    'Suraj Issue 15012, 2/25/13, Store Temp _XProductName field in oExpoDetail for getting the  ProductName
                    oExpoDetail.SetAddValue("_XProductName", lblProductName.Text)

                    Page.Validate()

                    Dim iItem As Integer, iItemForUpdate As Integer
                    'Anil Issue 12660
                    iItemForUpdate = 0
                    Dim bFound As Boolean = False
                    If Session("ProductID") IsNot Nothing AndAlso CLng(Session("ProductID")) > -1 Then
                        With oOrder
                            For iItem = 0 To .SubTypes("OrderLines").Count - 1
                                If CLng(.SubTypes("OrderLines").Item(iItem).GetValue("ProductID")) = CLng(Session("ProductID")) AndAlso _
                                   CLng(.SubTypes("OrderLines").Item(iItem).GetValue("ParentSequence")) <= 0 Then
                                    oOrderLine = CType(.SubTypes("OrderLines").Item(iItem), OrderLinesEntity)
                                    If oOrderLine.GetValue("_XBoothID") IsNot Nothing AndAlso CInt(oOrderLine.GetValue("_XBoothID")) = CInt(cmbBooth.SelectedValue) Then
                                        iLine = iItem
                                        bFound = True
                                    End If
                                End If
                            Next
                        End With
                    End If

                    If CLng(ViewState("BoothID")) = CLng(cmbBooth.SelectedValue) Or CLng(ViewState("BoothID")) = -1 Or CLng(cmbBooth.SelectedValue) = 0 Then
                        With oOrder.SubTypes("OrderLines").Item(iLine)
                            sData = oExpoDetail.GetObjectData(False)
                            'Some booths are more than one unit. 
                            'The Booth Quantity must be set accordingly.
                            If CInt(Me.lblBoothQty.Text) > 1 Then
                                .SetValue("Quantity", CInt(Me.lblBoothQty.Text))
                            Else
                                .SetValue("Quantity", 1)
                            End If
                            .SetAddValue("__ExtendedAttributeObjectData", sData)
                            .SetAddValue("_XBoothID", cmbBooth.SelectedValue)
                            'Explicitly set item price as per unit. 
                            'Order will multiply by Quantity for correct extended price.
                            .SetValue("Price", CDec(lblUnitPrice.Text))
                            .SetAddValue("__XPriceEXPO", CDec(lblUnitPrice.Text))
                        End With
                    Else

                        If bFound OrElse sSourcePage = "ViewCart" OrElse sSourcePage = "CheckOut" Then
                            With oOrder.SubTypes("OrderLines").Item(iLine)
                                sData = oExpoDetail.GetObjectData(False)
                                'Some booths are more than one unit. 
                                'The Booth Quantity must be set accordingly.
                                If CInt(Me.lblBoothQty.Text) > 1 Then
                                    .SetValue("Quantity", CInt(Me.lblBoothQty.Text))
                                Else
                                    .SetValue("Quantity", 1)
                                End If
                                .SetAddValue("__ExtendedAttributeObjectData", sData)
                                .SetAddValue("_XBoothID", cmbBooth.SelectedValue)
                                .SetValue("Price", CDec(lblUnitPrice.Text))
                                .SetAddValue("__XPriceEXPO", CDec(lblUnitPrice.Text))
                            End With
                        Else
                            oOrderLine = oOrder.AddProduct(lProductID).Item(0)

                            With oOrderLine
                                sData = oExpoDetail.GetObjectData(False)
                                If CInt(Me.lblBoothQty.Text) > 1 Then
                                    .SetValue("Quantity", CInt(Me.lblBoothQty.Text))
                                Else
                                    .SetValue("Quantity", 1)
                                End If
                                .SetAddValue("_XBoothID", cmbBooth.SelectedValue)
                                .SetAddValue("__ExtendedAttributeObjectData", sData)
                                .SetValue("Price", CDec(lblUnitPrice.Text))
                                .SetAddValue("__XPriceEXPO", CDec(lblUnitPrice.Text))

                            End With
                        End If
                    End If
                    With oOrder.SubTypes("OrderLines").Item(iLine)
                        If ViewState("ProductQuantity") IsNot Nothing Then
                            If .GetValue("_XOldQuantity") IsNot Nothing Then
                                .SetValue("Quantity", .GetValue("_XOldQuantity"))
                            End If
                        End If
                    End With
                    Session("ProductID") = -1
                    ShoppingCart1.SaveCart(Session)
                    Response.Redirect(SaveButtonRedirect, False)
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Function ValidateOrderBoothDetail() As Boolean
            'Verify the OrderBoothDetail required fields are filled in. 
            'OrderBoothDetail entity requires the following fields
            'ExhibitorID, ContactID, BoothID (if available)
            Dim bResult As Boolean = True
            Dim sErrorMsg As String = ""
            'Reset error messages first
            Me.lblError.Text = ""
            Me.lblError.Visible = False

            'Business Rules do not required a Booth to be selected at time of order
            'If no booths are available for selection we will still process this order
            'If cmbBooth.Items.Count > 1 AndAlso Not CInt(cmbBooth.SelectedValue) > 0 Then
            '    'Booths are available for selection, yet one was not selected
            '    sErrorMsg = "Please select a Booth"
            '    Me.lblBooth.Font.Bold = True
            'End If


            If Me.txtExhibitor.Text = "" Then
                'Exhibitor is a required field
                IIf(sErrorMsg.Length > 0, "<br>", "")
                sErrorMsg &= "Please fill in the Exhibitor"
            End If

            'Primary Contact
            'If Me.txtContact.Text = "" Then
            '    'Primary Contact is a required field
            '    IIf(sErrorMsg.Length > 0, "<br>", "")
            '    sErrorMsg &= "Please fill in the Primary Contact"
            '    Me.lblContact.Font.Bold = True
            'End If
            If Me.txtPCFName.Text.Trim = "" Or txtPCLName.Text.Trim = "" Or txtPCEmail.Text.Trim = "" Then
                'Primary Contact is a required field
                IIf(sErrorMsg.Length > 0, "<br>", "")
                sErrorMsg &= "Please fill in the Primary Contact"
            Else
                'If Not ValidatePerson(txtPCFName.Text.Trim, txtPCLName.Text.Trim, txtPCEmail.Text.Trim) Then
                '    radDuplicateUser.VisibleOnPageLoad = True
                '    bResult = False
                '    Return bResult
                '    Exit Function
                'End If
            End If

            If Not (Me.txtSCFName.Text.Trim = "" Or txtSCLName.Text.Trim = "") Then
                'Secondary Contact is a required field
                If txtSCEmail.Text.Trim = "" Then
                    IIf(sErrorMsg.Length > 0, "<br>", "")
                    sErrorMsg &= "Please fill in the Secondary Contact Email ID"
                    'Suraj Issue 15210, 2/6/13 asterisk mark if the secondary email is required it will visible 
                    lblasterisk.Visible = True
                Else
                    'If Not ValidatePerson(txtSCFName.Text.Trim, txtSCLName.Text.Trim, txtSCEmail.Text.Trim) Then
                    '    radDuplicateUser.VisibleOnPageLoad = True
                    '    bResult = False
                    '    Return bResult
                    '    Exit Function
                    'End If
                End If
            End If

            'Anil B Issue 15012 Moved this logic into client side javascript function EnableDisablePhoneValidation

            'HP - Issue 8243, when a phone number is provided then make sure that the area code and phone number fileds are provided, otherwise 
            'order fails to save since area code and phone numbers are required fields in the PhoneNumbers entity
            'If Trim(txtAreaCode.Text).Length = 0 And Trim(txtTelephone.Text).Length > 0 Then
            '    Me.lblPhoneError.Text = "* Area Code Required"
            '    Me.lblPhoneError.Visible = True
            '    bResult = False
            'ElseIf Trim(txtAreaCode.Text).Length > 0 And Trim(txtTelephone.Text).Length = 0 Then
            '    lblPhoneError.Text = "* Phone Nubmer Required"
            '    lblPhoneError.Visible = True
            '    bResult = False
            'End If      

            If sErrorMsg.Length > 0 Then
                'A required field was not filled out
                Me.lblError.Text = sErrorMsg
                Me.lblError.Visible = True
                bResult = False
            End If

            Return bResult
        End Function

        'Issue 4345 MAS
        'When a booth is selected it is checked for a surcharge. If a surcharge exists the price listed on the page and the order's orderline price are both updated.
        Protected Sub cmbBooth_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbBooth.SelectedIndexChanged
            Try
                'Clear the surcharge label
                lblSurcharge.Text = ""
                lblSurcharge.Visible = False
                lblError.Text = ""
                lblError.Visible = False

                Me.lblBoothQty.Text = "1" 'Clear the Required Booth Units

                If (CInt(cmbBooth.SelectedValue) > 0) Then
                    Dim sSQL As String, dt As DataTable
                    Dim lProductID As Long
                    Dim iLine As Integer
                    Dim oOrder As AptifyGenericEntityBase
                    Dim oOrderLine As AptifyGenericEntityBase
                    If ViewState("ILine") IsNot Nothing AndAlso IsNumeric(ViewState("ILine")) AndAlso CInt(ViewState("ILine")) > -1 Then
                        iLine = CInt(ViewState("ILine"))
                    ElseIf Request.QueryString("OL") IsNot Nothing AndAlso IsNumeric(Request.QueryString("OL")) AndAlso CInt(Request.QueryString("OL")) > -1 Then
                        iLine = CInt(Request.QueryString("OL"))
                    End If


                    oOrder = ShoppingCart1.GetOrderObject(Me.Session, Me.Page.User, Me.Application)
                    oOrderLine = oOrder.SubTypes("OrderLines").Item(iLine)
                    lProductID = CLng(oOrderLine.GetValue("ProductID"))

                    '1. does the chosen booth have a surcharge?
                    '   query the db for surcharge info on booth selected to determine if a surcharge exists.
                    sSQL = "SELECT ID,Name,SurchargeType,Description,PreferredBoothSurcharge,SurchargePercent,NumUnits FROM " & _
                           Database & "..vwBooths " & _
                           "WHERE ID=" & cmbBooth.SelectedValue
                    dt = DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache)

                    Dim oPrice As New Applications.OrderEntry.IProductPrice.PriceInfo


                    ' Added by Dipali for Issue:5127
                    If Not IsDBNull(dt.Rows(0).Item("Name")) Then
                        Me.txtBoothName.Text = dt.Rows(0).Item("Name").ToString
                    End If
                    'Suraj Issue 15012, 2/25/13, if the user choose the booth form dropdown menu so The OrderBoothDetail.Description field (called Booth Description on the form) should be blank (not pre-populated) 
                    Me.txtBoothDescription.Text = ""
                    Dim BoothQty As Integer = 1
                    If Not IsDBNull(dt.Rows(0).Item("NumUnits")) Then
                        BoothQty = CInt(dt.Rows(0).Item("NumUnits"))
                        Me.lblBoothQty.Text = dt.Rows(0).Item("NumUnits").ToString
                    End If
                    oPrice = ShoppingCart1.GetUserProductPrice(lProductID, BoothQty)
                    oPrice.Price = oPrice.Price * BoothQty
                    Dim dSurcharge As Decimal = 0
                    With dt.Rows(0)
                        '2. calculate surcharge
                        If CStr(.Item("SurchargeType")) = "Flat Amount" Then
                            dSurcharge = CDec(.Item("PreferredBoothSurcharge"))
                        ElseIf CDec(.Item("SurchargePercent")) > 0 Then
                            dSurcharge = oPrice.Price / CDec(.Item("SurchargePercent"))
                        End If

                        '3. add surcharge fee to price table
                        lblPrice.Text = Format(oPrice.Price + dSurcharge, ShoppingCart1.GetCurrencyFormat(oPrice.CurrencyTypeID))
                        lblUnitPrice.Text = ((oPrice.Price + dSurcharge) / BoothQty).ToString 'Unit price including surcharge per unit required to 
                        If dSurcharge > 0 Then
                            'Only display the Surcharge if one exists
                            lblSurcharge.Text = "Includes " & _
                                                Format(dSurcharge, ShoppingCart1.GetCurrencyFormat(oPrice.CurrencyTypeID)) & _
                                                " surcharge."
                            lblSurcharge.Visible = True
                        End If

                        '4. change the line item price in the order object
                        Dim tempPrice As Decimal = oPrice.Price
                        tempPrice += dSurcharge
                        'oOrderLine.SetValue("Price", tempPrice)
                    End With
                Else
                    Dim oPrice As IProductPrice.PriceInfo = Me.GetProductPrice(CLng(Session("ProductID")))
                    lblUnitPrice.Text = Format(oPrice.Price, ShoppingCart1.GetCurrencyFormat(oPrice.CurrencyTypeID))
                    lblPrice.Text = Format(oPrice.Price, ShoppingCart1.GetCurrencyFormat(oPrice.CurrencyTypeID))
                    Me.txtBoothName.Text = ""
                    Me.txtBoothDescription.Text = ""
                    lblError.Text = "Please Select a Booth"
                    lblError.Visible = True
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Public Function GetProductPrice(ByVal lProductID As Long) As IProductPrice.PriceInfo
            ' Implement This function
            Return ShoppingCart1.GetUserProductPrice(lProductID, 1)
        End Function
        ''' <summary>
        ''' IssuID - 3240 Added by Suvarna D
        ''' Check if Duplicate Email Id added
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function ValidatePerson(ByVal sFirstName As String, ByVal sLastName As String, ByVal sEmail As String) As Boolean
            Dim sSpName As String = ""
            Dim sSQL As String = ""

            If Not String.IsNullOrEmpty(AptifyApplication.GetEntityAttribute("Persons", "ValidateDuplicatePersonRecord")) Then
                sSpName = CStr(AptifyApplication.GetEntityAttribute("Persons", "ValidateDuplicatePersonRecord"))
            End If

            sSQL = "Exec " & sSpName & " '" & sFirstName.Replace("'", "''") & "', '" & sLastName.Replace("'", "''") & "', '" & sEmail.Replace("'", "''") & "'"
            Dim iCnt As Int32 = 0

            iCnt = Convert.ToInt32(DataAction.ExecuteScalar(sSQL))

            If iCnt = 0 Then
                Return False
            ElseIf iCnt = 1 Then
                Return True
            End If
        End Function

        Private Function RemoveMeetingOrder() As Boolean
            Try
                Dim oOrder As OrdersEntity, oOrderLine As OrderLinesEntity
                Dim iLine As Integer
                iLine = CInt(Request.QueryString("OL"))
                oOrder = ShoppingCart1.GetOrderObject(Page.Session, Page.User, Page.Application)
                oOrderLine = CType(oOrder.SubTypes("OrderLines").Item(iLine), OrderLinesEntity)
                oOrder.RemoveOrderLine(oOrderLine)
                ShoppingCart1.SaveCart(Page.Session)
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Protected Sub btnok_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnok.Click
            radDuplicateUser.VisibleOnPageLoad = False
        End Sub

        'Anil B for Issue 15012
        Protected Sub EnableDisablePhoneValidation()
            If txtAreaCode.Text.Trim = "" AndAlso txtTelephone.Text.Trim = "" Then
                rfvAreaCode.Enabled = False
                rfvTelephone.Enabled = False
            Else
                If txtAreaCode.Text.Trim = "" Then
                    rfvAreaCode.Enabled = True
                    rfvTelephone.Enabled = False
                Else
                    rfvAreaCode.Enabled = False
                    rfvTelephone.Enabled = True
                End If
            End If
        End Sub
        'Suraj Issue 15012, 2/25/13, Add Method to get a Product Name by using productID
        Private Sub ProductName(ByVal ProductID As Long)
            Try
                lblProductName.Text = CStr(AptifyApplication.GetEntityRecordName("Products", ProductID))
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
    End Class
End Namespace
