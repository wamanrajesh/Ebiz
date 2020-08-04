'Aptify e-Business 5.5.1, July 2013
Option Explicit On
Imports Telerik.Sitefinity.Security
Imports Aptify.Framework.Integration
Imports Aptify.Framework.BusinessLogic
Namespace Aptify.Framework.Web.eBusiness
    Partial Class Sitefinity4xSSO
        Inherits Aptify.Framework.Web.eBusiness.BaseUserControlAdvanced
        'SKB Issue 11120 Single Sign on for Sitefinity for 4.0
        Public Overridable Function Sitefinity40SSO(ByVal UserName As String, ByVal Password As String) As Boolean
            Sitefinity40SSO = False
            Dim IsSitefinityActive As Boolean = False
            Dim m_oCmsIntegrationGeneric As CMSIntegrationGeneric
            Dim lstAllCMSs As List(Of GenericEntity.AptifyGenericEntity)
            Dim m_oCmsIntegration As CMSIntegrationInterface
            Dim oRep As New ObjectRepository.AptifyObjectRepository(Me.AptifyApplication.UserCredentials)
            m_oCmsIntegrationGeneric = New CMSIntegrationGeneric()
            lstAllCMSs = m_oCmsIntegrationGeneric.GetCMSList(Me.AptifyApplication, True)
            Dim bResult As Boolean = False
            For Each cmsSystem As GenericEntity.AptifyGenericEntity In lstAllCMSs
                ' Check if CMS is Sitefinity then only 
                If CBool(cmsSystem.GetValue("Active")) Then
                    m_oCmsIntegration = TryCast(Aptify.Framework.Application.InstanceCreator.CreateInstanceFromName(cmsSystem.GetValue("CMSAssembly").ToString, cmsSystem.GetValue("CMSClass").ToString), CMSIntegrationInterface)
                    If TypeOf m_oCmsIntegration Is Aptify.Framework.Integration.CMSIntegrationSiteFinity Then _
                    IsSitefinityActive = True
                    'Navin Prasad Issue 12416 
                    If cmsSystem.GetValue("ID") IsNot Nothing Then
                        m_oCmsIntegration.Config(AptifyApplication, cmsSystem.GetValue("ID"))
                    End If
                    Exit For
                End If
            Next
            If IsSitefinityActive Then
                Dim oManager As Telerik.Sitefinity.Security.UserManager = Telerik.Sitefinity.Security.UserManager.GetManager
                If oManager.ValidateUser(UserName, Password) Then
                    Dim oMemUser As Telerik.Sitefinity.Security.Model.User
                    oMemUser = DirectCast(oManager.GetUser(UserName), Telerik.Sitefinity.Security.Model.User)
                    Dim membershipProvider As String = oMemUser.ProviderName
                    'Navin Prasad Issue 12416 
                    'Navin Prasad Issue 11221 ' commented the code 
                    Dim validate As UserLoggingReason
                    '  If Not oMemUser.IsLoggedIn Then
                    validate = Telerik.Sitefinity.Security.SecurityManager.AuthenticateUser(UserManager.GetDefaultProviderName(), UserName, Password, False, oMemUser)
                    'End If
                    If m_oCmsIntegration IsNot Nothing Then
                        m_oCmsIntegration.LoginCMSUser(UserName, Password, Page)
                    End If
                    Dim authenticated As Boolean
                    authenticated = validate = UserLoggingReason.Success
                    Return authenticated
                End If
            End If
        End Function
    End Class
End Namespace

