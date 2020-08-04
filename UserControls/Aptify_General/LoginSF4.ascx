<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Register Src="Sitefinity4xSSO.ascx" TagName="Sitefinity4xSSO" TagPrefix="uc2" %>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="LoginSF4.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.WebLogin" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessShoppingCart" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessLogin" %>
<%@ Register TagPrefix="uc1" TagName="SocialNetworkingIntegrationControlSF4" Src="~/UserControls/Aptify_General/SocialNetworkingIntegrationControlSF4.ascx" %>
<meta http-equiv="Cache-Control" content="no-cache">
<meta http-equiv="Pragma" content="no-cache">
<meta http-equiv="Expires" content="0">
<asp:Panel ID="pnllogin" runat="server" DefaultButton="cmdLogin">
    <div id="loginTop" class="news-list" runat="server">
        <div>
            <div class="float-left login-header">
                <asp:Image ID="img" runat="server" ImageUrl="~/Images/ICE Login Icon.png" />ICE
                Login
            </div>
            <div class="float-right">
                <asp:CheckBox ID="chkAutoLogin" runat="server" ToolTip="Check this box if you would like the site to automatically log you in next time you visit."
                    Text=" Keep me signed in" AutoPostBack="true"></asp:CheckBox>
            </div>
            <div class="clear">
            </div>
        </div>
        <div class="login-container-home">
            <asp:Literal runat="server" ID="litLoginLabel" />
            <div id="tblLogin" runat="server" class="table-div">
                <div>
                    <asp:Label ID="lblError" ForeColor="Crimson" runat="server"></asp:Label>
                </div>
                <div class="row-div clearfix">
                    <div class="label-div w25">
                        <asp:Label ID="lblUserID" runat="server">User ID:</asp:Label>&nbsp;
                    </div>
                    <div class="field-div1 w74">
                        <asp:TextBox ID="txtUserID" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="row-div clearfix">
                    <div class="label-div w25">
                        <asp:Label ID="lblPassword" runat="server">Password:</asp:Label>&nbsp;
                    </div>
                    <div class="field-div1 w74">
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
                    </div>
                </div>
                <div class="tablecontrolsfontLogin">
                    <asp:UpdatePanel ID="pnl1" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lbl" runat="server" Visible="false"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="chkAutoLogin" EventName="" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="row-div top-margin clearfix">
                    <div class="label-div w25">
                        &nbsp;</div>
                    <div class="field-div1 w74">
                        <asp:Button ID="cmdLogin" runat="server" CssClass="submit-Btn" Text="Sign In"></asp:Button>
                        <span>
                            <asp:HyperLink ID="hlinkForgotUID" runat="server"><font color="#fd4310" size="1.9px">Forgot User Name or Password?</font></asp:HyperLink></span>
                    </div>
                </div>
                <div class="between-div">
                    <span class="between-span">OR </span>
                </div>
                <div class="row-div clearfix">
                    <div class="label-div">
                        <asp:Label ID="Label1" runat="server">Sign in with</asp:Label>
                    </div>
                    <div class="field-div">
                        <uc1:SocialNetworkingIntegrationControlSF4 ID="SocialNetworkingIntegrationControlSF4"
                            runat="server" />
                    </div>
                </div>
                <div align="right">
                    <asp:LinkButton ID="cmdNewUser" runat="server" Text="New User Signup!" CssClass="ButtonLink"
                        SkinID="Test"></asp:LinkButton>
                </div>
                <cc1:AptifyWebUserLogin ID="WebUserLogin1" runat="server" Width="175px" Height="9px"
                    Visible="False">
                </cc1:AptifyWebUserLogin>
            </div>
            <div id="tblWelcome" class="table-div" runat="server">
                <div>
                    <asp:Label ID="lblWelcome" runat="server">Welcome,</asp:Label>
                    <asp:Button ID="cmdLogOut" runat="server" Width="60px" CausesValidation="False" Text="Logout" />
                </div>
            </div>
            <cc2:AptifyShoppingCart ID="ShoppingCartLogin" runat="server">
            </cc2:AptifyShoppingCart>
        </div>
    </div>
</asp:Panel>
<uc2:Sitefinity4xSSO ID="Sitefinity4xSSO1" runat="server" />
