﻿'Aptify e-Business 5.5.1, July 2013
Namespace Aptify.Framework.Web.eBusiness
    Partial Class LoginAndEvent
        Inherits BaseUserControlAdvanced

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'If User1.UserID > 0 Then
            '    mvMain.ActiveViewIndex = 1
            'Else
            '    mvMain.ActiveViewIndex = 0
            'End If
            If User1.UserID > 0 Then
                trLogin.Visible = False
            Else
                trLogin.Visible = True
            End If
        End Sub
    End Class

End Namespace

