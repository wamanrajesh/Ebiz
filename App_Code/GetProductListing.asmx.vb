'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports System.Web
Imports System.ComponentModel
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Data

Namespace Aptify.Framework.Web.eBusiness.WebServices
    ''' <summary>
    ''' This web service class provides an example for returning product catalog information
    ''' through a SOAP based XML web service in Aptify e-Business.
    ''' </summary>
    ''' <remarks></remarks>
    <WebService(Namespace:="http://tempuri.org/")> _
    <WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Public Class GetProductListing
        Inherits BaseWebService

        ''' <summary>
        ''' Returns a listing of products within a  
        ''' given category. To return a listing of all 
        ''' products, set CategoryID=-1
        ''' </summary>
        ''' <param name="CategoryID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <WebMethod(), _
         Description("Returns a listing of products within a " & _
                     "given category. To return a listing of all " & _
                     "products, set CategoryID=-1")> _
        Public Function GetProductListing(ByVal CategoryID As Long) As String
            Dim sSQL As String

            If CategoryID <= 0 Then
                CategoryID = -1
            End If

            sSQL = "SELECT ID,WebName,WebDescription," & _
                   "WebLongDescription FROM " & _
                   Database & "..vwProducts WHERE WebEnabled=1"
            If CategoryID > 0 Then
                sSQL = sSQL & " AND CategoryID=" & CategoryID
            End If
            sSQL = sSQL & " ORDER BY WebName"
            Return ConvertTableToXML(DataAction.GetDataTable(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache))
        End Function

        ''' <summary>
        ''' This method returns an XML string based on the data trable provided
        ''' </summary>
        ''' <param name="dt"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Overridable Function ConvertTableToXML(ByVal dt As DataTable) As String
            Dim sOutput As New StringBuilder
            Dim i As Integer, j As Integer

            For i = 0 To dt.Rows.Count - 1
                sOutput.Append("<Product>")
                For j = 0 To dt.Columns.Count - 1
                    sOutput.Append("<" & dt.Columns(j).ColumnName & ">")
                    sOutput.Append(dt.Rows(i).Item(j))
                    sOutput.Append("</" & dt.Columns(j).ColumnName & ">")
                Next
                sOutput.Append("</Product>")
            Next
            Return sOutput.ToString
        End Function
    End Class
End Namespace

