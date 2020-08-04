'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports Aptify.Framework.ExceptionManagement
Imports Aptify.Applications.Accounting
Imports Aptify.Applications.Accounting.Currency
Imports Aptify.Applications.OrderEntry

''' <summary>
''' Purpose of this control is to refresh Aptify object caches in order for changes
''' made at the Core product level to be available without the need to restart IIS.
''' </summary>
Partial Class RefreshAptifyCache
    Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim oApp As Aptify.Framework.Application.AptifyApplication = Me.AptifyApplication

            'refresh logic for Entity, Filter Rule, Country and Culture String metadata.
            Dim oEntityMetaData As Aptify.Framework.BusinessLogic.MetadataSingleton = _
                    Aptify.Framework.BusinessLogic.MetadataSingleton.Instance(True)

            Aptify.Framework.BusinessLogic.FilterRules.FilterRuleCache.ClearCache()

            Dim oCountryMetaData As Aptify.Framework.BusinessLogic.CountryMetadataSingleton = _
                    Aptify.Framework.BusinessLogic.CountryMetadataSingleton.Instance(True)

            Dim oCultureStringMetaData As Aptify.Framework.BusinessLogic.CultureStringMetadataSingleton = _
               Aptify.Framework.BusinessLogic.CultureStringMetadataSingleton.Instance(True)


            'AptifyOrdersEntity
            EmployeeOrderPermissionCache.Instance(oApp).Clear()
            SalesTaxRateGLAccountCache.Instance(oApp).Clear()
            ShipmentTypeGLAccountCache.Instance(oApp).Clear()

            'With OrderProductCache.Instance(oApp)
            '    .ClearProductCache()
            '    .ClearProductTypeCache()
            'End With

            'CurrencyTypeCache
            Aptify.Framework.Application.CurrencyTypeCache.Instance(oApp).Clear()

            'AptifySalesTax
            TaxJurisdictionCache.Instance(oApp).Clear()
            'OrganizationTaxAddressCache.Instance(oApp).Clear()

            'AptifyShippingCalculator
            ShipmentTypeCache.Instance(oApp).Clear()

            lblMessage.ForeColor = Drawing.Color.Green
            lblMessage.Text = "The cached Aptify data has been cleared."
            lblMessage.Visible = True
        Catch ex As Exception
            ExceptionManager.Publish(ex)

            lblMessage.ForeColor = Drawing.Color.Red
            lblMessage.Text = "An error occurred while attempting to refresh the cached Aptify data within E-Business.  Error: " & ex.Message
        End Try
    End Sub

End Class
