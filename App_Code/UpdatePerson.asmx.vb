'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports Aptify.Framework.BusinessLogic.GenericEntity

Namespace Aptify.Framework.Web.eBusiness.WebServices
    ''' <summary>
    ''' This web service class provides an example for updating basic person information
    ''' through a SOAP based XML web service in Aptify e-Business.
    ''' </summary>
    ''' <remarks></remarks>
    <WebService(Namespace:="http://tempuri.org/")> _
    <WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Public Class UpdatePerson
        Inherits BaseWebService

        ''' <summary>
        ''' Updates the address for a specific Person record.
        ''' </summary>
        ''' <param name="PersonID"></param>
        ''' <param name="AddressLine1"></param>
        ''' <param name="AddressLine2"></param>
        ''' <param name="AddressLine3"></param>
        ''' <param name="City"></param>
        ''' <param name="State"></param>
        ''' <param name="ZipCode"></param>
        ''' <param name="Country"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <WebMethod()> Public Function UpdateAddress(ByVal PersonID As Long, _
                                                    ByVal AddressLine1 As String, _
                                                    ByVal AddressLine2 As String, _
                                                    ByVal AddressLine3 As String, _
                                                    ByVal City As String, _
                                                    ByVal State As String, _
                                                    ByVal ZipCode As String, _
                                                    ByVal Country As String) As Boolean
            Try
                Dim oPerson As AptifyGenericEntityBase
                oPerson = AptifyApplication.GetEntityObject("Persons", PersonID)
                If Not oPerson Is Nothing AndAlso _
                   oPerson.RecordID = PersonID Then
                    oPerson.SetValue("AddressLine1", AddressLine1)
                    oPerson.SetValue("AddressLine2", AddressLine2)
                    oPerson.SetValue("AddressLine3", AddressLine3)
                    oPerson.SetValue("City", City)
                    oPerson.SetValue("State", State)
                    oPerson.SetValue("ZipCode", ZipCode)
                    oPerson.SetValue("Country", Country)
                    Return oPerson.Save(False)
                Else
                    Return False
                End If
            Catch ex As Exception
                Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
                Return False
            End Try
        End Function
    End Class
End Namespace
