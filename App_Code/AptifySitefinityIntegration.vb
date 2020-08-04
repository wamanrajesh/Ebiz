'Aptify e-Business 5.5.1, July 2013
Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports Telerik.Sitefinity.Security
Imports Telerik.Sitefinity.Security.Model
Imports System.Linq
Imports System
Imports Telerik.Sitefinity.Model

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class AptifySitefinityIntegration
    Inherits System.Web.Services.WebService

    ''' <summary>
    ''' Returns a count of all users currently in Sitefinity
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    '''Neha Changes for Issue 17549
    '''Provided security for Sitefinity Integration WebService from REST client or any other third party or unauthorized users call.
    '''only administartor should have permission for calling this webservice methods.
    <WebMethod()> _
    Public Function NumberOfUsers(ByVal AdminUserName As String, ByVal AdminPassword As String) As Integer
        Try
            Dim validate = SecurityManager.AuthenticateUser(UserManager.GetDefaultProviderName(), AdminUserName, AdminPassword, True)
            If validate = UserLoggingReason.Success OrElse validate = UserLoggingReason.UserAlreadyLoggedIn Then
                Dim oMemCollection As IQueryable(Of MembershipUser)
                Dim oManager As Telerik.Sitefinity.Security.UserManager = Telerik.Sitefinity.Security.UserManager.GetManager
                oMemCollection = oManager.GetUsers()

                If oMemCollection IsNot Nothing And oMemCollection.Count > 0 Then
                    Return oMemCollection.Count
                Else
                    Return 0
                End If
            End If
        Catch ex As Exception
            Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            Return 0
        End Try
    End Function

    ''' <summary>
    ''' Returns an array of all Generic Content Tags that are in the system
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    'Neha Changes for Issue 17549
    <WebMethod()> _
    Public Function GetTopicCodeInfo(ByVal AdminUserName As String, ByVal AdminPassword As String) As String()()
        Try
            Dim validate = SecurityManager.AuthenticateUser(UserManager.GetDefaultProviderName(), AdminUserName, AdminPassword, True)
            If validate = UserLoggingReason.Success OrElse validate = UserLoggingReason.UserAlreadyLoggedIn Then
                Dim i As Integer
                Dim tManager As Telerik.Sitefinity.Taxonomies.TaxonomyManager = Telerik.Sitefinity.Taxonomies.TaxonomyManager.GetManager
                Dim tagTaxonomy As Telerik.Sitefinity.Taxonomies.Model.ITaxonomy = tManager.GetTaxonomy(Telerik.Sitefinity.Taxonomies.TaxonomyManager.TagsTaxonomyId)
                Dim aString(tagTaxonomy.Taxa.Count)() As String
                For i = 0 To tagTaxonomy.Taxa.Count - 1
                    aString(i) = New String() {tagTaxonomy.Taxa(i).Id.ToString(), tagTaxonomy.Taxa(i).Name, tagTaxonomy.Taxa(i).Name, ""}
                Next
                Return aString
            End If
        Catch ex As Exception
            Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Returns a array of all current Sitefinity Users 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    '''Neha Changes for Issue 17549
    <WebMethod()> _
    Public Function GetAllUsers(ByVal AdminUserName As String, ByVal AdminPassword As String) As String()()
        Try
            Dim validate = SecurityManager.AuthenticateUser(UserManager.GetDefaultProviderName(), AdminUserName, AdminPassword, True)
            If validate = UserLoggingReason.Success OrElse validate = UserLoggingReason.UserAlreadyLoggedIn Then
                Dim oMemCollection As IQueryable(Of MembershipUser)
                Dim oManager As Telerik.Sitefinity.Security.UserManager = Telerik.Sitefinity.Security.UserManager.GetManager
                oManager.Provider.SuppressSecurityChecks = True
                oMemCollection = oManager.GetUsers()

                Dim i As Integer

                If oMemCollection IsNot Nothing And oMemCollection.Count > 0 Then

                    Dim ie As System.Collections.IEnumerator
                    ie = oMemCollection.GetEnumerator()

                    Dim aString(oMemCollection.Count)() As String

                    For i = 0 To oMemCollection.Count - 1
                        ie.MoveNext()
                        aString(i) = New String() {CType(ie.Current, MembershipUser).ProviderUserKey.ToString(), CType(ie.Current, MembershipUser).UserName, "", "", "", CType(ie.Current, MembershipUser).Email, "0"}
                    Next

                    Return aString
                Else
                    Return Nothing
                End If
            End If
        Catch ex As Exception
            Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' This function will create a new Sitefinity user based on the parameters passed in.
    ''' </summary>
    ''' <param name="sUserName"></param>
    ''' <param name="sPWD"></param>
    ''' <param name="sEmail"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    '''Neha Changes for Issue 17549
    <WebMethod()> _
    Public Function CreateUser(ByVal AdminUserName As String, ByVal AdminPassword As String, ByVal sUserName As String, ByVal sPWD As String, ByVal sEmail As String) As String
        Try
            Dim validate = SecurityManager.AuthenticateUser(UserManager.GetDefaultProviderName(), AdminUserName, AdminPassword, True)
            If validate = UserLoggingReason.Success OrElse validate = UserLoggingReason.UserAlreadyLoggedIn Then
                Dim oMemUser As MembershipUser
                Dim oStatus As MembershipCreateStatus
                Dim oManager As Telerik.Sitefinity.Security.UserManager = Telerik.Sitefinity.Security.UserManager.GetManager
                ''Sitefinity added various statuses about failure/security checks like invalid password, invalid email id but we don’t support any error logging. 
                ''Currently we are suppressing security checks Telerik.Sitefinity.Security.UserManager.GetManager.Provider.SuppressSecurityChecks = True. '
                'This is too specific to Sitefinity but just thought to inform you. We need to document this.
                oManager.Provider.SuppressSecurityChecks = True
                oMemUser = oManager.CreateUser(sUserName, sPWD, sEmail, "", "", True, Nothing, oStatus)
                If oMemUser IsNot Nothing AndAlso oMemUser.ProviderName <> "" Then
                    oManager.SaveChanges()
                    Return oMemUser.ProviderUserKey.ToString
                ElseIf Not String.IsNullOrEmpty(oStatus) Then
                    Convert.ToString(oStatus)
                Else

                    Return ""
                End If
            End If
        Catch ex As Exception
            Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            Return ""
        End Try
    End Function

    ''' <summary>
    ''' This function will create a new Sitefinity user based on the parameters passed in.
    ''' </summary>
    ''' <param name="sUserName"></param>
    ''' <param name="FirstName"></param>
    ''' <param name="LastName"></param>
    ''' <param name="sPWD"></param>
    ''' <param name="sEmail"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    '''Neha Changes for Issue 17549
    <WebMethod()> _
    Public Function CreateNewUser(ByVal AdminUserName As String, ByVal AdminPassword As String, ByVal sUserName As String, ByVal FirstName As String, ByVal LastName As String, ByVal sPWD As String, ByVal sEmail As String) As String
        Try
            Dim validate = SecurityManager.AuthenticateUser(UserManager.GetDefaultProviderName(), AdminUserName, AdminPassword, True)
            If validate = UserLoggingReason.Success OrElse validate = UserLoggingReason.UserAlreadyLoggedIn Then
                'Navin Prasad Issue 12533
                Dim ProfileMngr As UserProfileManager
                Dim usrProfile As UserProfile
                Dim oMemUser As MembershipUser
                Dim oStatus As MembershipCreateStatus
                Dim oManager As Telerik.Sitefinity.Security.UserManager = Telerik.Sitefinity.Security.UserManager.GetManager
                ''Sitefinity added various statuses about failure/security checks like invalid password, invalid email id but we don’t support any error logging. 
                ''Currently we are suppressing security checks Telerik.Sitefinity.Security.UserManager.GetManager.Provider.SuppressSecurityChecks = True. '
                'This is too specific to Sitefinity but just thought to inform you. We need to document this.
                oManager.Provider.SuppressSecurityChecks = True
                oMemUser = oManager.CreateUser(sUserName, sPWD, sEmail, "", "", True, Nothing, oStatus)
                If oMemUser IsNot Nothing AndAlso oMemUser.ProviderName <> "" Then
                    Dim oSitefinityUser As Telerik.Sitefinity.Security.Model.User = DirectCast(oMemUser, Telerik.Sitefinity.Security.Model.User)
                    If oSitefinityUser IsNot Nothing Then
                        oSitefinityUser.FirstName = FirstName
                        oSitefinityUser.LastName = LastName
                        'SKB Adding to make it as backend user
                        oSitefinityUser.IsBackendUser = True
                    End If
                    oManager.SaveChanges()
                    ProfileMngr = UserProfileManager.GetManager()
                    usrProfile = ProfileMngr.CreateProfile(oSitefinityUser, GetType(SitefinityProfile).FullName)
                    Telerik.Sitefinity.Model.DataExtensions.SetValue(usrProfile, "FirstName", FirstName)
                    Telerik.Sitefinity.Model.DataExtensions.SetValue(usrProfile, "LastName", LastName)
                    ProfileMngr.SaveChanges()
                    Return oMemUser.ProviderUserKey.ToString
                ElseIf Not String.IsNullOrEmpty(oStatus) Then
                    Convert.ToString(oStatus)
                Else
                    Return ""
                End If
            End If
        Catch ex As Exception
            Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            Return ""
        End Try
    End Function
    ''' <summary>
    ''' THis function will check if the username provided already exists in Sitefinity
    ''' </summary>
    ''' <param name="sUserName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    '''Neha Changes for Issue 17549
    <WebMethod()> _
    Public Function UserNameExists(ByVal AdminUserName As String, ByVal AdminPassword As String, ByVal sUserName As String) As Boolean
        Try
            Dim sAuthenticated As Boolean = False
            Dim validate = SecurityManager.AuthenticateUser(UserManager.GetDefaultProviderName(), AdminUserName, AdminPassword, True)
            If validate = UserLoggingReason.Success OrElse validate = UserLoggingReason.UserAlreadyLoggedIn Then
                Dim oMemUser As MembershipUser
                Dim oManager As Telerik.Sitefinity.Security.UserManager = Telerik.Sitefinity.Security.UserManager.GetManager
                oMemUser = oManager.GetUser(sUserName)
                If oMemUser IsNot Nothing AndAlso oMemUser.UserName = sUserName Then
                    sAuthenticated = True
                Else
                    sAuthenticated = False
                End If
            End If
            Return sAuthenticated
        Catch ex As Exception
            Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' This function will add a Sitefinity User to a Sitefinity Role 
    ''' </summary>
    ''' <param name="sCMSUserName"></param>
    ''' <param name="sGroupName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    '''Neha Changes for Issue 17549
    <WebMethod()> _
    Public Function AddUserToGroup(ByVal AdminUserName As String, ByVal AdminPassword As String, ByVal sCMSUserName As String, ByVal sGroupName As String) As Boolean
        Try
            Dim sAuthenticated As Boolean = False
            Dim validate = SecurityManager.AuthenticateUser(UserManager.GetDefaultProviderName(), AdminUserName, AdminPassword, True)
            If validate = UserLoggingReason.Success OrElse validate = UserLoggingReason.UserAlreadyLoggedIn Then
                Dim oMemUser As MembershipUser
                Dim oManager As Telerik.Sitefinity.Security.UserManager = Telerik.Sitefinity.Security.UserManager.GetManager
                oMemUser = oManager.GetUser(sCMSUserName)
                Dim oRole As Telerik.Sitefinity.Security.Model.Role
                Dim oRoleManager As Telerik.Sitefinity.Security.RoleManager = Telerik.Sitefinity.Security.RoleManager.GetManager
                oRole = oRoleManager.GetRole(sGroupName)
                oRoleManager.AddUserToRole(oMemUser, oRole)
                oRoleManager.SaveChanges()
                oManager.SaveChanges()
                sAuthenticated = True
            End If
            Return sAuthenticated
        Catch ex As Exception
            Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Returns the Sitefinity GUID in string format for the UserID passed in. If it doesn't find
    ''' a matching user in Sitefinity it will return empty string
    ''' </summary>
    ''' <param name="sUserName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    '''Neha Changes for Issue 17549
    <WebMethod()> _
    Public Function GetSiteFinityUserIDFromUserName(ByVal AdminUserName As String, ByVal AdminPassword As String, ByVal sUserName As String) As String
        Try
            Dim validate = SecurityManager.AuthenticateUser(UserManager.GetDefaultProviderName(), AdminUserName, AdminPassword, True)
            If validate = UserLoggingReason.Success OrElse validate = UserLoggingReason.UserAlreadyLoggedIn Then
                Dim oMemUser As MembershipUser
                Dim oManager As Telerik.Sitefinity.Security.UserManager = Telerik.Sitefinity.Security.UserManager.GetManager
                oMemUser = oManager.GetUser(sUserName)

                If oMemUser IsNot Nothing AndAlso oMemUser.ProviderName <> "" Then
                    Return oMemUser.ProviderUserKey.ToString
                Else
                    Return ""
                End If
            End If
        Catch ex As Exception
            Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            Return ""
        End Try
    End Function

    ''' <summary>
    ''' Function will login Sitefinity user. To implement custom login procedure enter code and return either 0 or 1
    ''' depending outcome.
    ''' Return 0: Login Failed
    ''' Return 1: Login Succeeded
    ''' Return 2: Use standard Forms authentication
    ''' </summary>
    ''' <param name="UserName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    '''Neha Changes for Issue 17549
    <WebMethod()> _
    Public Function LoginSitefinityUser(ByVal AdminUserName As String, ByVal AdminPassword As String, ByVal UserName As String, ByVal Password As String) As Int32
        Dim validate = SecurityManager.AuthenticateUser(UserManager.GetDefaultProviderName(), AdminUserName, AdminPassword, True)
        If validate = UserLoggingReason.Success OrElse validate = UserLoggingReason.UserAlreadyLoggedIn Then
            Return 2
        Else
            Return 0
        End If
    End Function

    ''' <summary>
    ''' Validates username and password.
    ''' </summary>
    ''' <param name="UserName"></param>
    ''' <param name="Password"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    '''Neha Changes for Issue 17549
    <WebMethod()> _
    Public Function ValidateSitefinityUser(ByVal AdminUserName As String, ByVal AdminPassword As String, ByVal UserName As String, ByVal Password As String) As Boolean
        Try
            Dim sAuthenticated As Boolean = False
            Dim validate = SecurityManager.AuthenticateUser(UserManager.GetDefaultProviderName(), AdminUserName, AdminPassword, True)
            If validate = UserLoggingReason.Success OrElse validate = UserLoggingReason.UserAlreadyLoggedIn Then
                Dim oManager As Telerik.Sitefinity.Security.UserManager = Telerik.Sitefinity.Security.UserManager.GetManager
                Return oManager.ValidateUser(UserName, Password)
                sAuthenticated = True
            End If
            Return sAuthenticated
        Catch ex As Exception
            Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            Return False
        End Try
    End Function
    ''' <summary>
    ''' Function to synchronize the Aptify Web User Groups with Sitefinity Roles.
    ''' </summary>
    ''' <param name="CMSUserName"> CMS User Name</param>
    ''' <param name="UserGroups">Aptify Web User Groups separated by '^' symbol </param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    '''Neha Changes for Issue 17549
    <WebMethod()> _
    Public Function SynchUserGroups(ByVal AdminUserName As String, ByVal AdminPassword As String, ByVal CMSUserName As String, ByVal UserGroups As String) As Boolean
        'SKB Issue 12416 01/04/2012
        Dim strRetVal As String = ""
        Dim arrRoles() As String
        Dim iCount As Integer
        Dim oManager As UserManager = UserManager.GetManager()
        Dim oRoleMgrDefault As RoleManager = RoleManager.GetManager("AppRoles")
        Dim oRoleMgr As RoleManager = RoleManager.GetManager()
        Dim listUserRoles As System.Linq.IQueryable(Of Telerik.Sitefinity.Security.Model.Role)
        Dim oRole As Telerik.Sitefinity.Security.Model.Role
        Dim dtNow As DateTime = DateTime.UtcNow
        Dim sIPAddr As String = HttpContext.Current.Request.UserHostAddress
        Dim oMemUser As User
        Dim lActualAptifyRoles As Long = 0, lRolesMatching As Long = 0, lSitefinityRolesNotMatchingAptify = 0
        Dim ProfileMngr As UserProfileManager = UserProfileManager.GetManager()
        Try
            Dim sAuthenticated As Boolean = False
            Dim validate = SecurityManager.AuthenticateUser(UserManager.GetDefaultProviderName(), AdminUserName, AdminPassword, True)
            If validate = UserLoggingReason.Success OrElse validate = UserLoggingReason.UserAlreadyLoggedIn Then
                oManager.Provider.SuppressSecurityChecks = True
                oRoleMgr.Provider.SuppressSecurityChecks = True

                ProfileMngr.Provider.SuppressSecurityChecks = True

                oMemUser = DirectCast(oManager.GetUser(CMSUserName), Telerik.Sitefinity.Security.Model.User)

                'Get the user list

                listUserRoles = oRoleMgr.GetRoles().Concat(oRoleMgrDefault.GetRoles())
                ''listUserRoles.oRoleMgrDefault.GetRoles()

                '' oRoleMgr.GetRolesForUser(oMemUser.Id)
                'Split the Roles
                If UserGroups.Trim.Length > 0 Then
                    arrRoles = UserGroups.Split(CChar("^"))
                    If arrRoles.Length > 0 Then
                        'Get Total Roles in Aptify
                        lActualAptifyRoles = arrRoles.Count
                        'Check if Roles count at both does not match
                        'Navin Prasad Issue 12416 
                        If Not listUserRoles Is Nothing Then
                            For Each oRole In listUserRoles
                                If oRole.Name.Trim.ToLower <> "administrators" AndAlso oRole.Name.Trim.ToLower <> "backendusers" Then
                                    If oRoleMgr.RoleExists(oRole.Name.Trim) Then
                                        If oRoleMgr.IsUserInRole(oMemUser.Id, oRole.Name.Trim) Then
                                            'Check whether the role exists in the Aptify Roles
                                            Dim bRoleMatch As Boolean = False
                                            For iCount = 0 To arrRoles.GetUpperBound(0)
                                                Dim strURole As String = arrRoles(iCount).Trim
                                                If strURole.Trim.Length > 0 Then
                                                    If strURole.Trim = oRole.Name.Trim Then
                                                        lRolesMatching += 1
                                                        bRoleMatch = True
                                                    End If
                                                End If
                                            Next
                                            'User role does not match increment the count
                                            If Not bRoleMatch Then lSitefinityRolesNotMatchingAptify += 1
                                        End If
                                    End If
                                End If
                            Next
                        End If
                    End If
                End If
                'if roles count at Aptify does not have exact matching at sitefinity or Sitefinity have more roles than Aptify 
                'then remove the roles and add only matching roles
                'Navin Prasad Issue 11221
                If lRolesMatching <> lActualAptifyRoles OrElse lSitefinityRolesNotMatchingAptify > 0 OrElse listUserRoles.AsEnumerable.Count() > 0 Then
                    If Not listUserRoles Is Nothing Then
                        For Each oRole In listUserRoles
                            If oRole.Name.Trim.ToLower <> "administrators" AndAlso oRole.Name.Trim.ToLower <> "backendusers" Then
                                If oRoleMgr.RoleExists(oRole.Name.Trim) Then
                                    If oRoleMgr.IsUserInRole(oMemUser.Id, oRole.Name.Trim) Then
                                        oRoleMgr.RemoveUserFromRole(oMemUser.Id, oRole)
                                        oRoleMgr.SaveChanges()
                                    End If
                                End If
                            End If
                        Next
                    End If

                    If arrRoles IsNot Nothing Then
                        For iCount = 0 To arrRoles.GetUpperBound(0)
                            Dim strURole As String = arrRoles(iCount).Trim
                            If strURole.Trim.Length > 0 Then
                                'Check if Role exists
                                If Not oRoleMgr.RoleExists(strURole.Trim) Then
                                    'Role does not exists, Create it
                                    oRoleMgr.CreateRole(strURole.Trim)
                                    oRoleMgr.SaveChanges()

                                    Dim objRole As Role = oRoleMgr.GetRole(strURole.Trim)
                                    'Add User to newly created role
                                    If Not oRoleMgr.IsUserInRole(oMemUser.Id, strURole.Trim) Then
                                        oRoleMgr.AddUserToRole(oMemUser, objRole)
                                        oRoleMgr.SaveChanges()
                                    End If

                                Else
                                    'Role Exist but user need to be added to the Role
                                    Dim objRole As Role = oRoleMgr.GetRole(strURole.Trim)

                                    If Not oRoleMgr.IsUserInRole(oMemUser.Id, strURole.Trim) Then
                                        oRoleMgr.AddUserToRole(oMemUser, objRole)
                                        oRoleMgr.SaveChanges()
                                    End If
                                End If
                            End If
                        Next
                    End If
                End If
                'Save the Manager changes
                oManager.SaveChanges()
                sAuthenticated = True
            End If
            Return sAuthenticated
        Catch ex As Exception
            Aptify.Framework.ExceptionManagement.ExceptionManager.Publish(ex)
            Return False
        End Try
    End Function
End Class
