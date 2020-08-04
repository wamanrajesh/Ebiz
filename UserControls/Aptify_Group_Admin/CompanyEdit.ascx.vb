'Aptify e-Business 5.5.1, July 2013
Imports Telerik.Web.UI
Imports Aptify.Framework.Application
Imports Aptify.Framework.DataServices
Imports Aptify.Framework.BusinessLogic.Security
Imports System.Data
Imports System.IO
Imports Aptify.Framework.Web.eBusiness.SocialNetworkIntegration
Imports Aptify.Framework.BusinessLogic.ProcessPipeline
Imports Aptify.Framework.BusinessLogic.GenericEntity
Imports System.Linq
Imports System.Drawing

Namespace Aptify.Framework.Web.eBusiness.CustomerService
    Partial Class CompanyEdit
        Inherits BaseUserControlAdvanced
        Dim dt As DataTable
        Protected Const ATTRIBUTE_LOGIN_PAGE As String = "LoginPage" 'Added by Sandeep for Issue 15051 on 12/03/2013
#Region "PublicProperties"
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
#End Region
        Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
            If Not IsPostBack Then
                'Anil B for issue 14320 on 09/04/2013
                'Remove Unwanted code and use the Company ID property of user instead of session veriable
                PopulateDropDowns()
                SetComboValue(cmbCountry, "United States")
                PopulateState(cmbState, cmbCountry)

                LoadAddressDetails()

                DisplayAddress("Street Address")
                'PopulateState(cmbState, cmbCountry)
                '' LoadTopicCodes()
                TopiccodeViewer.EntityName = "Companies"
                TopiccodeViewer.RecordID = User1.CompanyID
                TopiccodeViewer.ButtonDisplay = True
                TopiccodeViewer.lbldispaly = False
                LoadTopicCodesParent("Companies", User1.CompanyID)
                'Added by Sandeep for Issue 15051 on 12/03/2013
                If String.IsNullOrEmpty(LoginPage) Then
                    'since value is the 'default' check the XML file for possible custom setting
                    LoginPage = Me.GetLinkValueFromXML(ATTRIBUTE_LOGIN_PAGE)
                End If
                If User1.UserID < 0 Then
                    Response.Redirect(LoginPage) 'Added by Sandeep for Issue 15051 on 12/03/2013
                End If
            End If

        End Sub

        Public Sub LoadAddressDetails()
            Dim sSQL As String

            Try

                'Anil B for issue 14320 on 09/04/2013
                'Used the Company ID property of user instead of session veriable
                sSQL = "select  distinct Top 1  VC.Name,VC.CompanyType,vc.BillingAddressLine1,VC.BillingAddressLine2,VC.BillingAddressLine3,VC.BillingAddressLine4,VC.BillingCity,vc.BillingState,vc.BillingCountry,vc.POBox,vc.POBoxLine2,vc.POBoxLine3,vc.POBoxLine4,vc.POBoxCity,vc.POBoxState,vc.POBoxCountry,VC.AddressLine1,VC.AddressLine2,VC.AddressLine3,VC.AddressLine4,VC.City,VC.State,VC.Country,VC.MemberType,Case When Convert(Varchar(12),VC.JoinDate,107)='Jan 01, 1900' Then 'N/A' When Convert(Varchar(12),VC.JoinDate,107)is null  Then 'N/A' else Convert(Varchar(12),VC.JoinDate,107) end as StartDate,(isnull(Convert(varchar(12),VC.DuesPaidThru,107),'N/A')) as EndDate,VC.MemberType,VC.Country, case when isnull(Ltrim(Rtrim(VC.MainAreaCode)),'') <> '' then '(' + Ltrim(Rtrim(VC.MainAreaCode)) + ') ' else '' end  + VC.MainPhone  As 'Phone',case when isnull(Ltrim(Rtrim(VC.MainFaxAreaCode)),'')<> '' then '(' + Ltrim(Rtrim(VC.MainFaxAreaCode)) + ') ' else '' end + VC.MainFaxNumber as 'Fax',VC.MainEmail,VC.WebSite,VC.ZipCode,vc.BillingZipCode,VC.POBoxZipCode  " & _
                     " FROM " & Database & _
                     "..vwCompanies VC join " & Database & "..vwPersons VP on VC.ID=VP.CompanyID  where VP.CompanyID=" & User1.CompanyID


                dt = DataAction.GetDataTable(sSQL)
                Dim dcolstatus As DataColumn = New DataColumn()
                dcolstatus.Caption = "Status"
                dcolstatus.ColumnName = "Status"


                dt.Columns.Add(dcolstatus)
                'Anil B for issue 14320 on 23-03-2013
                'Set membership expirationdate 
                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                    For Each rw As DataRow In dt.Rows
                        If rw("EndDate").ToString() = "N/A" OrElse rw("StartDate").ToString() = "N/A" Then
                            rw("Status") = "Unavailable"
                        ElseIf Convert.ToDateTime(rw("EndDate").ToString()) = Convert.ToDateTime(rw("StartDate").ToString()).AddDays(-1) Then
                            rw("EndDate") = rw("StartDate")
                            rw("Status") = "Expired"
                        ElseIf Convert.ToDateTime(rw("EndDate").ToString()) > Date.Now().AddDays(90) Then
                            rw("Status") = "Active"
                        ElseIf Convert.ToDateTime(rw("EndDate").ToString()) > Date.Now() AndAlso rw("EndDate") < Date.Now().AddDays(90) Then
                            rw("Status") = "Expiring"
                        ElseIf Convert.ToDateTime(rw("EndDate").ToString()) < Date.Now() Then
                            rw("Status") = "Expired"
                        End If
                    Next
                End If

                Dim strbuildStreet, strbuildBilling, strbuildPObox As New StringBuilder()
                If (dt.Rows.Count() > 0) Then
                    If Not dt.Rows(0)("AddressLine1").ToString() = "" Then
                        strbuildStreet.Append(dt.Rows(0)("AddressLine1").ToString() + "<br/>")
                    End If
                    If Not dt.Rows(0)("AddressLine2").ToString() = "" Then
                        strbuildStreet.Append(dt.Rows(0)("AddressLine2").ToString() + "<br/>")
                    End If
                    If Not dt.Rows(0)("AddressLine3").ToString() = "" Then
                        strbuildStreet.Append(dt.Rows(0)("AddressLine3").ToString() + "<br/>")
                    End If
                    If Not dt.Rows(0)("City").ToString() = "" Then
                        strbuildStreet.Append(dt.Rows(0)("City").ToString() + ", ")
                    End If
                    If Not dt.Rows(0)("State").ToString() = "" Then
                        strbuildStreet.Append(dt.Rows(0)("State").ToString() + " ")
                    End If
                    'Amruta 14320 start
                    If Not dt.Rows(0)("ZipCode").ToString() = "" Then
                        strbuildStreet.Append(dt.Rows(0)("ZipCode").ToString() + "<br/>")
                    End If
                    'Amruta 14320 end
                    If Not dt.Rows(0)("Country").ToString() = "" Then
                        strbuildStreet.Append(dt.Rows(0)("Country").ToString())
                    End If
                    If Not dt.Rows(0)("BillingAddressLine1").ToString() = "" Then
                        strbuildBilling.Append(dt.Rows(0)("BillingAddressLine1").ToString() + "<br/>")
                    End If
                    If Not dt.Rows(0)("BillingAddressLine2").ToString() = "" Then
                        strbuildBilling.Append(dt.Rows(0)("BillingAddressLine2").ToString() + "<br/>")
                    End If
                    If Not dt.Rows(0)("BillingAddressLine3").ToString() = "" Then
                        strbuildBilling.Append(dt.Rows(0)("BillingAddressLine3").ToString() + "<br/>")
                    End If
                    If Not dt.Rows(0)("BillingCity").ToString() = "" Then
                        strbuildBilling.Append(dt.Rows(0)("BillingCity").ToString() + ", ")
                    End If
                    If Not dt.Rows(0)("BillingState").ToString() = "" Then
                        strbuildBilling.Append(dt.Rows(0)("BillingState").ToString() + " ")
                    End If
                    'Amruta 14320 start
                    If Not dt.Rows(0)("BillingZipCode").ToString() = "" Then
                        strbuildBilling.Append(dt.Rows(0)("BillingZipCode").ToString() + "<br/>")
                    End If
                    'Amruta 14320 end

                    If Not dt.Rows(0)("BillingCountry").ToString() = "" Then
                        strbuildBilling.Append(dt.Rows(0)("BillingCountry").ToString())
                    End If
                    If Not dt.Rows(0)("POBox").ToString() = "" Then
                        strbuildPObox.Append(dt.Rows(0)("POBox").ToString() + "<br/>")
                    End If
                    If Not dt.Rows(0)("POBoxLine2").ToString() = "" Then
                        strbuildPObox.Append(dt.Rows(0)("POBoxLine2").ToString() + "<br/>")
                    End If
                    If Not dt.Rows(0)("POBoxLine3").ToString() = "" Then
                        strbuildPObox.Append(dt.Rows(0)("POBoxLine3").ToString() + "<br/>")
                    End If
                    If Not dt.Rows(0)("POBoxCity").ToString() = "" Then
                        strbuildPObox.Append(dt.Rows(0)("POBoxCity").ToString() + ", ")
                    End If
                    If Not dt.Rows(0)("POBoxState").ToString() = "" Then
                        strbuildPObox.Append(dt.Rows(0)("POBoxState").ToString() + " ")
                    End If

                    'Amruta 14320 start
                    If Not dt.Rows(0)("POBoxZipCode").ToString() = "" Then
                        strbuildPObox.Append(dt.Rows(0)("POBoxZipCode").ToString() + "<br/>")
                    End If
                    'Amruta 14320 end

                    If Not dt.Rows(0)("POBoxCountry").ToString() = "" Then
                        strbuildPObox.Append(dt.Rows(0)("POBoxCountry").ToString())
                    End If
                    If Not dt.Rows(0)("Phone").ToString() = "" Then
                        lblphoneVal.Text = dt.Rows(0)("Phone").ToString()
                    Else
                        lblphoneVal.Text = "Not Provided"
                    End If
                    If Not dt.Rows(0)("Fax").ToString() = "" Then
                        lblFaxVal.Text = dt.Rows(0)("Fax").ToString()
                    Else
                        lblFaxVal.Text = "Not Provided"
                    End If
                End If

                StreetAddressval.Text = strbuildStreet.ToString()
                BillingAddressval.Text = strbuildBilling.ToString()
                PoboxAddressval.Text = strbuildPObox.ToString()
                lblPrimaryEmail.Text = dt.Rows(0)("MainEmail").ToString()
                lblWebsite.Text = dt.Rows(0)("WebSite").ToString()
                lblMemberTypeVal.Text = dt.Rows(0)("MemberType").ToString()
                lblStartDateVal.Text = dt.Rows(0)("StartDate").ToString()
                lblEndDateVal.Text = dt.Rows(0)("EndDate").ToString()
                lblStatusVal.Text = dt.Rows(0)("Status").ToString()
                If (StreetAddressval.Text = "") Then
                    tdBusinessAdd.Visible = False
                    tdStreetAddVal.Visible = False
                    'tdBusinessAdd.Style.Add("display", "none")
                    'tdStreetAddVal.Style.Add("display", "none")
                Else
                    tdBusinessAdd.Visible = True
                    tdStreetAddVal.Visible = True
                    'tdBusinessAdd.Style.Add("display", "block")
                    'tdStreetAddVal.Style.Add("display", "block")
                End If
                If (BillingAddressval.Text = "") Then
                    tdHomeAdd.Visible = False
                    tdPoboxAddVal.Visible = False
                    'tdHomeAdd.Style.Add("display", "none")
                    'tdPoboxAddVal.Style.Add("display", "none")
                Else
                    tdHomeAdd.Visible = True
                    tdPoboxAddVal.Visible = True
                    'tdHomeAdd.Style.Add("display", "block")
                    'tdPoboxAddVal.Style.Add("display", "block")
                End If
                If (PoboxAddressval.Text = "") Then
                    tdPoboxAdd.Visible = False
                    tdBillingAddVal.Visible = False
                    'tdPoboxAdd.Style.Add("display", "none")
                    'tdBillingAddVal.Style.Add("display", "none")
                Else
                    tdPoboxAdd.Visible = True
                    tdBillingAddVal.Visible = True
                    'tdPoboxAdd.Style.Add("display", "block")
                    'tdBillingAddVal.Style.Add("display", "block")
                End If
                If lblphoneVal.Text = "" Then
                    'tdPhone.Visible = False 
                    'lblphoneVal.Visible = False
                    'tdPhone.Style.Add("display", "none")
                    'lblphoneVal.Style.Add("display", "none")
                Else
                    'tdPhone.Visible = True
                    'lblphoneVal.Visible = True
                    'tdPhone.Style.Add("display", "block")
                    'lblphoneVal.Style.Add("display", "block")
                End If
                If lblFaxVal.Text = "" Then
                    'tdFax.Visible = False
                    'lblFaxVal.Visible = False
                    'tdFax.Style.Add("display", "none")
                    'lblFaxVal.Style.Add("display", "none")
                Else
                    'tdFax.Visible = True
                    'lblFaxVal.Visible = True
                    'tdFax.Style.Add("display", "block")
                    'lblFaxVal.Style.Add("display", "block")
                End If
                'Amruta Issue 14320 start
                If lblPrimaryEmail.Text = "" Then
                    'tdemail.Visible = False
                    'lblPrimaryEmail.Visible = False
                    tremail.Visible = False
                    lblPrimaryEmail.Visible = False
                    'tdFax.Style.Add("display", "none")
                    'lblFaxVal.Style.Add("display", "none")
                Else
                    'tdemail.Visible = True
                    'lblPrimaryEmail.Visible = True
                    tremail.Visible = True
                    lblPrimaryEmail.Visible = True
                    'tdFax.Style.Add("display", "block")
                    'lblFaxVal.Style.Add("display", "block")
                End If
                If lblWebsite.Text = "" Then
                    'tdWebsite.Visible = False
                    'lblWebsite.Visible = True
                    trWebSite.Visible = False
                    lblWebsite.Visible = False
                    'tdWebsite.Style.Add("display", "none")
                    'lblWebsite.Style.Add("display", "none")
                Else
                    'tdWebsite.Visible = True
                    'lblWebsite.Visible = True
                    trWebSite.Visible = True
                    lblWebsite.Visible = True
                    'tdWebsite.Style.Add("display", "block")
                    'lblWebsite.Style.Add("display", "block")
                End If
                'Amruta Issue 14320 end
                LoadAddresses()
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub selectedindex(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlAddressType.SelectedIndexChanged
            ''lbl.Text = radcmbAddress.SelectedItem.Text
            DisplayAddress(ddlAddressType.SelectedItem.Text)
        End Sub
        Private Sub PopulateDropDowns()
            Dim sSQL As String
            Dim dt As DataTable
            Try


                sSQL = AptifyApplication.GetEntityBaseDatabase("Addresses") & _
                       "..spGetCountryList"
                dt = DataAction.GetDataTable(sSQL)
                dt.Rows(0).Delete()
                cmbCountry.DataSource = dt
                cmbCountry.DataTextField = "Country"
                cmbCountry.DataValueField = "ID"
                cmbCountry.DataBind()

                'Amruta ,Issue No 14320, 4/3/2013 ,Code to bind respected country data to DropDownList
                cmbBillingCountry.DataSource = dt
                cmbBillingCountry.DataTextField = "Country"
                cmbBillingCountry.DataValueField = "ID"
                cmbBillingCountry.DataBind()

                cmbPOBoxCountry.DataSource = dt
                cmbPOBoxCountry.DataTextField = "Country"
                cmbPOBoxCountry.DataValueField = "ID"
                cmbPOBoxCountry.DataBind()

            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Private Sub PopulateState(ByRef cmbPopulateState As DropDownList, ByRef cmbCurrentCountry As DropDownList)
            Try
                Dim sSQL As String
                If (cmbCurrentCountry.SelectedValue.ToString = "0") Then
                    cmbCurrentCountry.SelectedValue = 1
                End If
                sSQL = Database & "..spGetStateList @CountryID=" & cmbCurrentCountry.SelectedValue.ToString
                cmbPopulateState.DataSource = DataAction.GetDataTable(sSQL)
                cmbPopulateState.DataTextField = "State"
                cmbPopulateState.DataValueField = "State"
                cmbPopulateState.DataBind()
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Protected Sub cmbCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCountry.SelectedIndexChanged
            PopulateState(cmbState, cmbCountry)

        End Sub
        Private Sub DisplayAddress(ByVal sAddrType As String)
            Dim sSQL As String, dt As Data.DataTable
            Try
                'Anil B for issue 14320 on 09/04/2013
                'Used the Company ID property of user instead of session veriable
                sSQL = "select  distinct Top 1  VC.Name,VC.CompanyType,vc.BillingAddressLine1,VC.BillingAddressLine2,VC.BillingAddressLine3,VC.BillingAddressLine4,VC.BillingCity,vc.BillingState,vc.BillingCountry,vc.POBox,vc.POBoxLine2,vc.POBoxLine3,vc.POBoxLine4,vc.POBoxCity,vc.POBoxState,vc.POBoxCountry,VC.AddressLine1,VC.AddressLine2,VC.AddressLine3,VC.AddressLine4,VC.City,VC.State,VC.Country,VC.MemberType,Case When Convert(Varchar(12),VC.JoinDate,107)='Jan 01, 1900' Then 'N/A' When Convert(Varchar(12),VC.JoinDate,107)is null  Then 'N/A' else Convert(Varchar(12),VC.JoinDate,107) end as StartDate,(isnull(CAST (VC.DuesPaidThru as varchar(11)),'N/A')) as DuesPaidThru,VC.Country,VC.MainAreaCode , VC.MainPhone ,VC.MainFaxAreaCode , VC.MainFaxNumber,VC.MainEmail,VC.WebSite,VC.ZipCode,vc.BillingZipCode,VC.POBoxZipCode  " & _
                      " FROM " & Database & _
                      "..vwCompanies VC join " & Database & "..vwPersons VP on VC.ID=VP.CompanyID  where VP.CompanyID= " & User1.CompanyID

                dt = DataAction.GetDataTable(sSQL)
                'Amruta Issue 14320,4/3/2013, Code to visible respected addressline row and changed address line control to corrsponding Addressline control 
                Select Case sAddrType
                    Case "Street Address"
                        trAddressLine1.Visible = True
                        trAddressLine2.Visible = True
                        trAddressLine3.Visible = True
                        trCity.Visible = True
                        trCountry.Visible = True

                        trBillingAddressLine1.Visible = False
                        trBillingAddressLine2.Visible = False
                        trBillingAddressLine3.Visible = False
                        trBillingCity.Visible = False
                        trBillingCountry.Visible = False

                        trPOBoxAddressLine1.Visible = False
                        trPOBoxAddressLine2.Visible = False
                        trPOBoxAddressLine3.Visible = False
                        trPOBoxCity.Visible = False
                        trPOBoxCountry.Visible = False

                        txtAddressLine1.Text = dt.Rows(0)("AddressLine1").ToString()
                        txtAddressLine2.Text = dt.Rows(0)("AddressLine2").ToString()
                        txtAddressLine3.Text = dt.Rows(0)("AddressLine3").ToString()
                        'cmbCountry.SelectedItem.Text = dt.Rows(0)("Country").ToString()
                        If dt.Rows(0)("Country").ToString() = "" Then

                            cmbCountry.SelectedIndex = cmbCountry.Items.IndexOf(cmbCountry.Items.FindByText("United States"))
                        Else
                            'cmbCountry.SelectedItem.Text = dt.Rows(0)("Country").ToString()
                            cmbCountry.SelectedIndex = cmbCountry.Items.IndexOf(cmbCountry.Items.FindByText(dt.Rows(0)("Country").ToString()))
                        End If
                        'Anil B for issue 14320 on 23-03-2013
                        'Set selected value to the city dropdown
                        PopulateState(cmbState, cmbCountry)
                        cmbState.SelectedIndex = cmbState.Items.IndexOf(cmbState.Items.FindByText(dt.Rows(0)("State").ToString()))
                        txtCity.Text = dt.Rows(0)("City").ToString()
                        txtEmail.Text = dt.Rows(0)("MainEmail").ToString()
                        txtWebsite.Text = dt.Rows(0)("WebSite").ToString()
                        txtzip.Text = dt.Rows(0)("ZipCode").ToString()
                    Case "Billing Address"
                        trAddressLine1.Visible = False
                        trAddressLine2.Visible = False
                        trAddressLine3.Visible = False
                        trCity.Visible = False
                        trCountry.Visible = False

                        trBillingAddressLine1.Visible = True
                        trBillingAddressLine2.Visible = True
                        trBillingAddressLine3.Visible = True
                        trBillingCity.Visible = True
                        trBillingCountry.Visible = True

                        trPOBoxAddressLine1.Visible = False
                        trPOBoxAddressLine2.Visible = False
                        trPOBoxAddressLine3.Visible = False
                        trPOBoxCity.Visible = False
                        trPOBoxCountry.Visible = False
                        txtBillingAddressLine1.Text = dt.Rows(0)("BillingAddressLine1").ToString()
                        txtBillingAddressLine2.Text = dt.Rows(0)("BillingAddressLine2").ToString()
                        txtBillingAddressLine3.Text = dt.Rows(0)("BillingAddressLine3").ToString()

                        If dt.Rows(0)("BillingCountry").ToString() = "" Then
                            'cmbCountry.SelectedItem.Text = "United States"
                            cmbBillingCountry.SelectedIndex = cmbBillingCountry.Items.IndexOf(cmbBillingCountry.Items.FindByText("United States"))
                        Else
                            ' cmbCountry.SelectedItem.Text = dt.Rows(0)("BillingCountry").ToString()
                            cmbBillingCountry.SelectedIndex = cmbBillingCountry.Items.IndexOf(cmbBillingCountry.Items.FindByText(dt.Rows(0)("BillingCountry").ToString()))
                        End If
                        'Anil B for issue 14320 on 23-03-2013
                        'Set selected value to the city dropdown
                        PopulateState(cmbBillingState, cmbBillingCountry)
                        cmbBillingState.SelectedIndex = cmbBillingState.Items.IndexOf(cmbBillingState.Items.FindByText(dt.Rows(0)("BillingState").ToString()))

                        txtBillingCity.Text = dt.Rows(0)("BillingCity").ToString()
                        txtEmail.Text = dt.Rows(0)("MainEmail").ToString()
                        txtWebsite.Text = dt.Rows(0)("WebSite").ToString()
                        txtBillingZipCode.Text = dt.Rows(0)("BillingZipCode").ToString()
                    Case "PO Box Address"
                        trAddressLine1.Visible = False
                        trAddressLine2.Visible = False
                        trAddressLine3.Visible = False
                        trCity.Visible = False
                        trCountry.Visible = False

                        trBillingAddressLine1.Visible = False
                        trBillingAddressLine2.Visible = False
                        trBillingAddressLine3.Visible = False
                        trBillingCity.Visible = False
                        trBillingCountry.Visible = False


                        trPOBoxAddressLine1.Visible = True
                        trPOBoxAddressLine2.Visible = True
                        trPOBoxAddressLine3.Visible = True
                        trPOBoxCity.Visible = True
                        trPOBoxCountry.Visible = True

                        txtPOBoxAddressLine1.Text = dt.Rows(0)("POBox").ToString()
                        txtPOBoxAddressLine2.Text = dt.Rows(0)("POBoxLine2").ToString()
                        txtPOBoxAddressLine3.Text = dt.Rows(0)("POBoxLine3").ToString()
                        If dt.Rows(0)("POBoxCountry").ToString() = "" Then
                            'cmbCountry.SelectedItem.Text = "United States"
                            cmbPOBoxCountry.SelectedIndex = cmbPOBoxCountry.Items.IndexOf(cmbPOBoxCountry.Items.FindByText("United States"))
                        Else
                            ' cmbCountry.SelectedItem.Text = dt.Rows(0)("POBoxCountry").ToString()
                            cmbPOBoxCountry.SelectedIndex = cmbPOBoxCountry.Items.IndexOf(cmbPOBoxCountry.Items.FindByText(dt.Rows(0)("POBoxCountry").ToString()))
                        End If
                        'Anil B for issue 14320 on 23-03-2013
                        'Set selected value to the city dropdown
                        PopulateState(cmbPOBoxState, cmbPOBoxCountry)
                        cmbPOBoxState.SelectedIndex = cmbPOBoxState.Items.IndexOf(cmbPOBoxState.Items.FindByText(dt.Rows(0)("POBoxState").ToString()))

                        txtPOBoxCity.Text = dt.Rows(0)("POBoxCity").ToString()
                        txtEmail.Text = dt.Rows(0)("MainEmail").ToString()
                        txtWebsite.Text = dt.Rows(0)("WebSite").ToString()
                        txtPOBoxZipCode.Text = dt.Rows(0)("POBoxZipCode").ToString()

                End Select
                txtPhoneAreaCode.Text = dt.Rows(0)("MainAreaCode").ToString().Trim()
                txtPhone.Text = dt.Rows(0)("MainPhone").ToString()
                txtFaxAreaCode.Text = dt.Rows(0)("MainFaxAreaCode").ToString().Trim()
                'txtFaxAreaCode.Text = dt.Rows(0)("MainFaxAreaCode").ToString()
                txtFaxPhone.Text = dt.Rows(0)("MainFaxNumber").ToString()
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub

        Protected Sub contact_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles contact.Click
            DisplayAddress(ddlAddressType.SelectedItem.Text)
            RadWindow1.VisibleOnPageLoad = True
        End Sub
        Protected Sub btnsave_Click(sender As Object, e As System.EventArgs)
            Dim sCounty As String = ""
            'Anil B for issue 14320 on 09/04/2013
            'Used the Company ID property of user instead of session veriable

            ''\Dim oGE As Aptify.Applications.Companies = DirectCast(Me.AptifyApplication.GetEntityObject("Persons", User1.PersonID), Aptify.Applications.Companies)
            Dim oGE As Applications.CompanyObject.CompanyObject = DirectCast(Me.AptifyApplication.GetEntityObject("Companies", User1.CompanyID), Aptify.Applications.CompanyObject.CompanyObject)
            'Changed control to respected AddressLine control
            With oGE
                'Anil B for issue 14320 on 09/04/2013
                'Set the city and state on the zip code
                Me.DoPostalCodeLookup(txtzip.Text, CStr(IIf(cmbCountry.SelectedIndex >= 0, cmbCountry.SelectedValue, "")), sCounty, txtCity, cmbState)
                oGE.SetValue("AddressLine1", txtAddressLine1.Text)
                oGE.SetValue("AddressLine2", txtAddressLine2.Text)
                oGE.SetValue("AddressLine3", txtAddressLine3.Text)
                oGE.SetValue("City", txtCity.Text)
                oGE.SetValue("State", cmbState.SelectedItem.Text)
                'oGE.SetValue("Country", cmbCountry.SelectedItem.Text)
                oGE.SetValue("CountryCodeID", CLng(IIf(cmbCountry.SelectedIndex >= 0, cmbCountry.SelectedItem.Value, "")))
                oGE.SetValue("Country", CStr(IIf(cmbCountry.SelectedIndex >= 0, cmbCountry.SelectedValue, "")))
                oGE.SetValue("MainEmail", txtEmail.Text)
                oGE.SetValue("WebSite", txtWebsite.Text)
                oGE.SetValue("ZipCode", txtzip.Text)
                If txtPhoneAreaCode.Text.Trim() = "" AndAlso txtPhone.Text.Trim() = "" Then
                    oGE.SetValue("MainPhoneID", -1)
                Else
                    oGE.Fields("MainPhoneID").EmbeddedObject.SetValue("AreaCode", txtPhoneAreaCode.Text)
                    oGE.Fields("MainPhoneID").EmbeddedObject.SetValue("Phone", txtPhone.TextWithLiterals)
                    'oGE.SubTypes("CompanyPhone").Add().SetValue("AreaCode", txtPhoneAreaCode.Text)
                    'oGE.SubTypes("CompanyPhone").Add().SetValue("Phone", txtPhone.Text)
                End If


                If txtFaxPhone.Text = "" AndAlso txtFaxAreaCode.Text = "" Then
                    oGE.SetValue("MainFaxID", -1)
                Else
                    oGE.Fields("MainFaxID").EmbeddedObject.SetValue("AreaCode", txtFaxAreaCode.Text)
                    oGE.Fields("MainFaxID").EmbeddedObject.SetValue("Phone", txtFaxPhone.TextWithLiterals)
                End If
                sCounty = ""
                'Anil B for issue 14320 on 09/04/2013
                'Set the city and state on the zip code
                Me.DoPostalCodeLookup(txtBillingZipCode.Text, CStr(IIf(cmbBillingCountry.SelectedIndex >= 0, cmbBillingCountry.SelectedValue, "")), sCounty, txtBillingCity, cmbBillingState)
                oGE.SetValue("BillingAddressLine1", txtBillingAddressLine1.Text)
                oGE.SetValue("BillingAddressLine2", txtBillingAddressLine2.Text)
                oGE.SetValue("BillingAddressLine3", txtBillingAddressLine3.Text)
                oGE.SetValue("BillingCity", txtBillingCity.Text)
                'oGE.SetValue("BillingState", CStr(IIf(cmbBillingState.SelectedIndex >= 0, cmbBillingState.SelectedValue, "")))
                oGE.SetValue("BillingState", cmbBillingState.SelectedItem.Text)
                ''oGE.SetValue("BillingCountry", cmbCountry.SelectedItem.Text)
                'oGE.SetValue("BillingCountry", cmbCountry.SelectedItem.Value)
                oGE.SetValue("BillingCountryCodeID", CLng(IIf(cmbBillingCountry.SelectedIndex >= 0, cmbBillingCountry.SelectedItem.Value, "")))
                oGE.SetValue("BillingCountry", CStr(IIf(cmbBillingCountry.SelectedIndex >= 0, cmbBillingCountry.SelectedValue, "")))

                oGE.SetValue("MainEmail", txtEmail.Text)
                oGE.SetValue("WebSite", txtWebsite.Text)
                oGE.SetValue("BillingZipCode", txtBillingZipCode.Text)
                If txtPhoneAreaCode.Text.Trim() = "" AndAlso txtPhone.Text.Trim() = "" Then
                    oGE.SetValue("MainPhoneID", -1)
                Else
                    oGE.Fields("MainPhoneID").EmbeddedObject.SetValue("AreaCode", txtPhoneAreaCode.Text)
                    oGE.Fields("MainPhoneID").EmbeddedObject.SetValue("Phone", txtPhone.TextWithLiterals)
                    'oGE.SubTypes("CompanyPhone").Add().SetValue("AreaCode", txtPhoneAreaCode.Text)
                    'oGE.SubTypes("CompanyPhone").Add().SetValue("Phone", txtPhone.Text)
                End If


                If txtFaxPhone.Text = "" AndAlso txtFaxAreaCode.Text = "" Then
                    oGE.SetValue("MainFaxID", -1)
                Else
                    oGE.Fields("MainFaxID").EmbeddedObject.SetValue("AreaCode", txtFaxAreaCode.Text)
                    oGE.Fields("MainFaxID").EmbeddedObject.SetValue("Phone", txtFaxPhone.TextWithLiterals)
                End If
                sCounty = ""
                'Anil B for issue 14320 on 09/04/2013
                'Set the city and state on the zip code
                Me.DoPostalCodeLookup(txtPOBoxZipCode.Text, CStr(IIf(cmbPOBoxCountry.SelectedIndex >= 0, cmbPOBoxCountry.SelectedValue, "")), sCounty, txtPOBoxCity, cmbPOBoxState)
                oGE.SetValue("POBox", txtPOBoxAddressLine1.Text)
                oGE.SetValue("POBoxLine2", txtPOBoxAddressLine2.Text)
                oGE.SetValue("POBoxLine3", txtPOBoxAddressLine3.Text)
                oGE.SetValue("POBoxCity", txtPOBoxCity.Text)
                'oGE.SetValue("POBoxState", CStr(IIf(cmbPOBoxState.SelectedIndex >= 0, cmbPOBoxState.SelectedValue, "")))
                oGE.SetValue("POBoxState", cmbPOBoxState.SelectedItem.Text)
                ''oGE.SetValue("POBoxCountry", cmbCountry.SelectedItem.Text)
                oGE.SetValue("POBoxCountryCodeID", CLng(IIf(cmbPOBoxCountry.SelectedIndex >= 0, cmbPOBoxCountry.SelectedItem.Value, "")))
                oGE.SetValue("POBoxCountry", CStr(IIf(cmbPOBoxCountry.SelectedIndex >= 0, cmbPOBoxCountry.SelectedValue, "")))
                oGE.SetValue("MainEmail", txtEmail.Text)
                oGE.SetValue("WebSite", txtWebsite.Text)
                oGE.SetValue("POBoxZipCode", txtPOBoxZipCode.Text)
                If txtPhoneAreaCode.Text.Trim() = "" AndAlso txtPhone.Text.Trim() = "" Then
                    oGE.SetValue("MainPhoneID", -1)
                Else
                    oGE.Fields("MainPhoneID").EmbeddedObject.SetValue("AreaCode", txtPhoneAreaCode.Text)
                    oGE.Fields("MainPhoneID").EmbeddedObject.SetValue("Phone", txtPhone.TextWithLiterals)
                    'oGE.SubTypes("CompanyPhone").Add().SetValue("AreaCode", txtPhoneAreaCode.Text)
                    'oGE.SubTypes("CompanyPhone").Add().SetValue("Phone", txtPhone.Text)
                End If


                If txtFaxPhone.Text = "" AndAlso txtFaxAreaCode.Text = "" Then
                    oGE.SetValue("MainFaxID", -1)
                Else
                    oGE.Fields("MainFaxID").EmbeddedObject.SetValue("AreaCode", txtFaxAreaCode.Text)
                    oGE.Fields("MainFaxID").EmbeddedObject.SetValue("Phone", txtFaxPhone.TextWithLiterals)
                End If

            End With
            Dim sErrorString As String = ""
            oGE.Save(sErrorString)

            RadWindow1.VisibleOnPageLoad = False
            LoadAddressDetails()
        End Sub

        Private Sub SetComboValue(ByRef cmb As DropDownList, ByVal sValue As String)
            Dim i As Integer

            Try
                For i = 0 To cmb.Items.Count - 1
                    If String.Compare(cmb.Items(i).Value, sValue, True) = 0 Then
                        cmb.Items(i).Selected = True
                        Exit Sub
                    End If

                    If String.Compare(cmb.Items(i).Text, sValue, True) = 0 Then
                        cmb.Items(i).Selected = True
                        Exit Sub
                    End If

                Next
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        Protected Sub btnCancel_Click(sender As Object, e As System.EventArgs)
            RadWindow1.VisibleOnPageLoad = False
        End Sub

        Public Overridable Function LoadTopicCodesParent(ByVal sEntityID As String, ByVal sRecordID As String) As String

            Dim dt As New DataTable()
            Dim strbuildtopic As New StringBuilder()
            Dim strbuildtopics As New StringBuilder()
            Dim sSQL As String = "select VT.RootTopicCode from " & Database & "..vwTopicCodeLinks VL join " & Database & "..vwTopicCodes VT on VL.TopicCodeID=VT.ID where VL.EntityID= " + Convert.ToString(AptifyApplication.GetEntityID(sEntityID.ToString())) + " and VL.RecordID=" + sRecordID + "  and VT.ParentID=-1 and VL.Value= " + " 'Yes' "
            dt = DataAction.GetDataTable(sSQL)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    'strbuildtopic.Append(dt.Rows(i)("RootTopicCode").ToString() + ", ")
                    'Amruta IssueID 14320
                    strbuildtopic.Append(dt.Rows(i)("RootTopicCode").ToString() + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;")
                Next
            End If
            If strbuildtopic.ToString().Length > 0 Then
                strbuildtopics.Append(strbuildtopic.ToString().Remove(strbuildtopic.ToString().Length - 1, 1))
            End If

            lblTopicIntrest.Text = strbuildtopics.ToString()

            Return String.Empty
        End Function

        'Private Sub LoadTopicCodesParent()
        '    Try
        '        Dim sSQL As String = "select Top 10 ID,Name  from vwTopicCodes where ParentID='-1' and WebEnabled='Global' and Status='Active'"
        '        Dim dt As DataTable = DataAction.GetDataTable(sSQL)
        '        lblTopicIntrest.Text = String.Empty
        '        ''TopiccodeViewer.Items.Clear()
        '        If dt.Rows.Count > 0 Then
        '            Dim litem As System.Web.UI.WebControls.ListItem
        '            For Each rw As DataRow In dt.Rows
        '                litem = New System.Web.UI.WebControls.ListItem
        '                litem.Text = rw(1).ToString
        '                litem.Value = rw(0).ToString
        '                '' listTopicofInterest.Items.Add(litem)

        '            Next
        '        End If
        '        Dim icount As Long = 0
        '        Dim oLink As AptifyGenericEntityBase
        '        lblTopicIntrest.Text = ""
        '        Dim strbuild As New StringBuilder
        '        For Each itm As System.Web.UI.WebControls.ListItem In listTopicofInterest.Items
        '            With itm
        '                sSQL = GetNodeCheckSQL(CLng(.Value))
        '                icount = CInt(DataAction.ExecuteScalar(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache))
        '                If icount > 0 Then
        '                    sSQL = GetNodeLinkSQL(CLng(.Value))
        '                    Dim lLinkID As Long = CLng(DataAction.ExecuteScalar(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache))
        '                    oLink = AptifyApplication.GetEntityObject("Topic Code Links", lLinkID)
        '                    If oLink IsNot Nothing And oLink.GetValue("Status") IsNot Nothing Then
        '                        itm.Selected = CBool(IIf(CStr(oLink.GetValue("Status")) = "Active", True, False))
        '                        If CStr(oLink.GetValue("Status")) = "Active" Then
        '                            'lblTopicIntrest.Text += "     " & itm.Text
        '                            strbuild.Append(itm.Text + " &nbsp;&nbsp;&nbsp;&nbsp;")
        '                        End If

        '                    End If
        '                End If
        '            End With
        '        Next
        '        lblTopicIntrest.Text = strbuild.ToString()
        '    Catch ex As Exception
        '        Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        '    End Try
        'End Sub

        Private Function GetNodeCheckSQL(ByVal lTopicCodeID As Long) As String
            'Return "SELECT COUNT(*) FROM " & _
            '       GetNodeBaseSQL(lTopicCodeID)
        End Function

        'Private Function GetNodeLinkSQL(ByVal lTopicCodeID As Long) As String
        '    Return "SELECT ID FROM " & _
        '           GetNodeBaseSQL(lTopicCodeID)
        'End Function
        'Private Function GetNodeBaseSQL(ByVal lTopicCodeID As Long) As String
        '    Dim sSQL As String
        '    sSQL = Database & ".." & _
        '           "vwTopicCodeLinks WHERE TopicCodeID=" & lTopicCodeID & _
        '           " AND EntityID=(SELECT ID FROM " & _
        '           Database & _
        '           "..vwEntities WHERE Name='Companies') " & _
        '           " AND RecordID=" & CType(Session.Item("CompanyID"), Long)
        '    Return sSQL
        'End Function

        'Private Function AddTopicCodeLink(ByVal TopicCodeID As Long) As Boolean
        '    Dim oLink As AptifyGenericEntityBase
        '    Try
        '        oLink = AptifyApplication.GetEntityObject("Topic Code Links", -1)
        '        oLink.SetValue("TopicCodeID", TopicCodeID)
        '        oLink.SetValue("RecordID", CType(Session.Item("CompanyID"), Long))
        '        oLink.SetValue("EntityID", AptifyApplication.GetEntityID("Companies"))
        '        oLink.SetValue("Status", "Active")
        '        oLink.SetValue("Value", "Yes")
        '        oLink.SetValue("DateAdded", Date.Today)
        '        Return oLink.Save(False)
        '    Catch ex As Exception
        '        Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        '    End Try
        'End Function

        'Private Function UpdateTopicCodeLink(ByVal TopicCodeID As Long, _
        '                                     ByVal Active As Boolean) As Boolean
        '    Dim sSQL As String
        '    Dim lLinkID As Long
        '    Dim oLink As AptifyGenericEntityBase

        '    Try
        '        sSQL = GetNodeLinkSQL(TopicCodeID)
        '        lLinkID = CLng(DataAction.ExecuteScalar(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache))
        '        oLink = AptifyApplication.GetEntityObject("Topic Code Links", lLinkID)
        '        If Active Then
        '            oLink.SetValue("Status", "Active")
        '            oLink.SetValue("Value", "Yes")
        '        Else
        '            oLink.SetValue("Status", "Inactive")
        '            oLink.SetValue("Value", "No")
        '        End If
        '        Return oLink.Save(False)
        '    Catch ex As Exception
        '        Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
        '    End Try
        'End Function

        'Private Sub AddUpdateTopicCode()
        '    Dim iCount As Long
        '    Dim bOK As Boolean
        '    For Each itm As System.Web.UI.WebControls.ListItem In listTopicofInterest.Items
        '        With itm
        '            bOK = True
        '            Dim sSQL As String = GetNodeCheckSQL(CLng(.Value))
        '            iCount = CInt(DataAction.ExecuteScalar(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache))
        '            If (.Selected And iCount = 0) Then
        '                bOK = bOK And AddTopicCodeLink(CLng(.Value))

        '            ElseIf (iCount <> 0 AndAlso (Not .Selected Or .Selected)) Then
        '                bOK = bOK And UpdateTopicCodeLink(CLng(.Value), .Selected)
        '            End If
        '        End With
        '    Next
        'End Sub


        Protected Sub btnTopicIntrest_Click(sender As Object, e As System.EventArgs) Handles btnTopicIntrest.Click
            'LoadTopicCodes()
            'Anil B for issue 14320 on 23-03-2013
            'Load Topic of intrest
            radtopicintrest.VisibleOnPageLoad = True
            TopiccodeViewer.LoadTree()
            Dim lblTopicKeeps As Label
            lblTopicKeeps = TopiccodeViewer.FindControl("Topicskeep")
            lblTopicKeeps.Text = ""
        End Sub
        Protected Sub btnSaveIntrest_Click(sender As Object, e As System.EventArgs) Handles btnSaveIntrest.Click
            'AddUpdateTopicCode()
            'LoadTopicCodes()
            TopiccodeViewer.SaveTopicCode()
            radtopicintrest.VisibleOnPageLoad = False
            LoadTopicCodesParent("Companies", User1.CompanyID)
        End Sub
        Protected Sub btnCancelIntrest_Click(sender As Object, e As System.EventArgs) Handles btnCancelIntrest.Click
            'LoadTopicCodes()
            radtopicintrest.VisibleOnPageLoad = False
        End Sub

        Protected Sub cmbBillingCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbBillingCountry.SelectedIndexChanged
            PopulateState(cmbBillingState, cmbBillingCountry)
        End Sub
        Protected Sub cmbPOBoxCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbPOBoxCountry.SelectedIndexChanged
            PopulateState(cmbPOBoxState, cmbPOBoxCountry)
        End Sub

        Private Sub LoadAddresses()
            Try
                If dt.Rows.Item(0) IsNot Nothing Then


                    If dt.Rows.Item(0)("AddressLine1") IsNot Nothing Then
                        txtAddressLine1.Text = dt.Rows.Item(0)("AddressLine1").ToString()
                        txtAddressLine2.Text = dt.Rows.Item(0)("AddressLine2").ToString()
                        txtAddressLine3.Text = dt.Rows.Item(0)("AddressLine3").ToString()
                        txtCity.Text = dt.Rows.Item(0)("City").ToString()
                        txtzip.Text = dt.Rows.Item(0)("ZipCode").ToString()
                    End If

                    'Put inside If statement if you don't want to default the address to US
                    SetComboValue(cmbCountry, IIf(dt.Rows.Item(0)("Country").ToString() = "", "United States", dt.Rows.Item(0)("Country").ToString()))
                    PopulateState(cmbState, cmbCountry)
                    SetComboValue(cmbState, dt.Rows.Item(0)("State").ToString())

                    If dt.Rows.Item(0)("BillingAddressLine1") IsNot Nothing Then
                        txtBillingAddressLine1.Text = dt.Rows.Item(0)("BillingAddressLine1").ToString()
                        txtBillingAddressLine2.Text = dt.Rows.Item(0)("BillingAddressLine2").ToString()
                        txtBillingAddressLine3.Text = dt.Rows.Item(0)("BillingAddressLine3").ToString()
                        txtBillingCity.Text = dt.Rows.Item(0)("BillingCity").ToString()
                        txtBillingZipCode.Text = dt.Rows.Item(0)("BillingZipCode").ToString()
                    End If

                    'Populate Billing country or default to United States
                    SetComboValue(cmbBillingCountry, IIf(dt.Rows.Item(0)("BillingCountry").ToString() = "", "United States", dt.Rows.Item(0)("BillingCountry").ToString()))
                    PopulateState(cmbBillingState, cmbBillingCountry)
                    SetComboValue(cmbBillingState, dt.Rows.Item(0)("BillingState").ToString())

                    If dt.Rows.Item(0)("POBox") IsNot Nothing Then
                        txtPOBoxAddressLine1.Text = dt.Rows.Item(0)("POBox").ToString()
                        txtPOBoxAddressLine2.Text = dt.Rows.Item(0)("POBoxLine2").ToString()
                        txtPOBoxAddressLine3.Text = dt.Rows.Item(0)("POBoxLine3").ToString()
                        txtPOBoxCity.Text = dt.Rows.Item(0)("POBoxCity").ToString()
                        txtPOBoxZipCode.Text = dt.Rows.Item(0)("POBoxZipCode").ToString()
                    End If

                    'Populate pobox country or default to united states
                    SetComboValue(cmbPOBoxCountry, IIf(dt.Rows.Item(0)("POBoxCountry").ToString() = "", "United States", dt.Rows.Item(0)("POBoxCountry").ToString()))
                    PopulateState(cmbPOBoxState, cmbPOBoxCountry)
                    SetComboValue(cmbPOBoxState, dt.Rows.Item(0)("POBoxState").ToString())
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            End Try
        End Sub
        'Anil B for issue 14320 on 09/04/2013
        'Set the city and state on the zip code
        Protected Overridable Sub DoPostalCodeLookup(ByRef PostalCode As String, ByRef Country As String, ByRef County As String, ByRef txt As TextBox, ByVal cmb As DropDownList)
            Dim sAreaCode As String = Nothing, sCounty As String = Nothing, sCity As String = Nothing
            Dim sState As String = Nothing, sCongDist As String = Nothing, sCountry As String = Nothing
            Dim ISOCountry As String
            Try
                Dim oPostalCode As New Aptify.Framework.BusinessLogic.Address.AptifyPostalCode(Me.AptifyApplication.UserCredentials)
                Dim oAddressInfo As New Aptify.Framework.BusinessLogic.Address.AddressInfo(Me.AptifyApplication)

                ISOCountry = oAddressInfo.GetISOCode(CLng(Country))

                If oPostalCode.GetPostalCodeInfo(PostalCode, ISOCountry, _
                                        sCity, sState, _
                                        sAreaCode, , sCounty, , , , , , , , _
                                        sCongDist, sCountry, AllowGUI:=True) Then
                    If txt IsNot Nothing Then
                        If String.IsNullOrWhitespace(txt.Text) Then
                            txt.Text = sCity
                        End If

                    End If
                    If cmb IsNot Nothing Then
                        cmb.SelectedValue = sState
                    End If


                    County = sCounty

                End If

            Catch ex As Exception

            End Try
        End Sub
    End Class

End Namespace
