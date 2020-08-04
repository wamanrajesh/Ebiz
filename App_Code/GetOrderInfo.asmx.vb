'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Option Strict On

Imports System.Web
Imports System.ComponentModel
Imports System.Web.Services
Imports System.Web.Services.Protocols

Namespace Aptify.Framework.Web.eBusiness.WebServices

    ''' <summary>
    ''' This web service class provides an example for returning basic order information
    ''' through a SOAP based XML web service in Aptify e-Business.
    ''' </summary>
    ''' <remarks></remarks>
    <WebService(Namespace:="http://tempuri.org/")> _
    <WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Public Class GetOrderInfo
        Inherits BaseWebService

        ''' <summary>
        ''' Enumerated type for tracking the possible shipment status values for an order.
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum ShipStatus
            Pending
            Shipped
            Backordered
            Cancelled
        End Enum

        ''' <summary>
        ''' Gets the current balance of an order
        ''' </summary>
        ''' <param name="OrderID">Order ID to retreive the balance for</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <WebMethod(), _
         Description("Gets the current balance of an order")> _
        Public Overridable Function GetBalance(<System.ComponentModel.Description("Order ID to retreive the balance for")> ByVal OrderID As Long) As Decimal
            Return CDec(InternalGetValue(OrderID, "Balance"))
        End Function

        ''' <summary>
        ''' Returns the due date for a given order
        ''' </summary>
        ''' <param name="OrderID">Order ID to retreive the due date for</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <WebMethod()> Public Overridable Function GetDueDate(ByVal OrderID As Long) As Date
            Return CDate(InternalGetValue(OrderID, "DueDate"))
        End Function

        ''' <summary>
        ''' Returns the date a particular order was shipped
        ''' </summary>
        ''' <param name="OrderID">Order ID to retreive the ship date for</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <WebMethod()> Public Overridable Function GetShipDate(ByVal OrderID As Long) As Date
            Return CDate(InternalGetValue(OrderID, "ShipDate"))
        End Function

        ''' <summary>
        ''' Returns the shipment status for a specific order.
        ''' </summary>
        ''' <param name="OrderID">Order ID to retreive the ship status for</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <WebMethod()> Public Overridable Function GetShipStatus(ByVal OrderID As Long) As ShipStatus
            Select Case CLng(InternalGetValue(OrderID, "OrderStatusID"))
                Case 1
                    Return ShipStatus.Pending
                Case 2
                    Return ShipStatus.Shipped
                Case 3
                    Return ShipStatus.Backordered
                Case 4
                    Return ShipStatus.Cancelled
            End Select
        End Function

        Protected Overridable Function InternalGetValue(ByVal OrderID As Long, ByVal sField As String) As String
            Dim sSQL As String
            sSQL = "SELECT " & sField & " FROM " & _
                   Database & "..vwOrders WHERE ID=" & OrderID
            Return CStr(DataAction.ExecuteScalar(sSQL, DataServices.IAptifyDataAction.DSLCacheSetting.BypassCache))
        End Function
    End Class
End Namespace
