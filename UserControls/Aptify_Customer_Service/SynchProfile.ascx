<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SynchProfile.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.CustomerService.SynchProfile" %>
<%@ Register TagPrefix="uc1" TagName="SocialNetworkingIntegrationControlSF4" Src="~/UserControls/Aptify_General/SocialNetworkingIntegrationControlSF4.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessLogin" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
 <%--“This version of SyncProfile is for use with Sitefinity”--%>
<div class="sync-profile-main-div">
     <div class="row-div">
         <asp:CheckBox ID="chkUseSocialMediaPhoto" runat="server" 
             Text="Use my LinkedIn Photo for this Profile."   
             AutoPostBack="True" />
    </div>
     <div class="row-div">
        <asp:Label ID="lblSyncMessage" runat="server" 
            Text="Sync your profile with LinkedIn."></asp:Label>
    </div>
    <div class="row-div">
        <uc1:SocialNetworkingIntegrationControlSF4 ID="SocialNetworkingIntegrationControlSF4"
            runat="server" />
    </div>
    <div class="row-div">
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
       <ContentTemplate> 
        <div id="tblsync" runat="server">
            <div class="row-div clearfix">
                <div class="label-div w70 ">
                    <asp:Label ID="lbl2" runat="server" Text="Account Sync: "></asp:Label>
                </div>
                <div class="field-div1">
                    <asp:Label ID="lblActivateStatus" runat="server" ></asp:Label>
                </div>
                </div>
       <div class="row-div clearfix">
         <div class="label-div w70">
            &nbsp;</div>
                 <div class="field-div1">
                    <asp:LinkButton ID="lnkDeactivate" runat="server" Text="Deactivate" ValidationGroup="sync"></asp:LinkButton>
                </div>
         
        </div>
        </div>
         </ContentTemplate>            
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="lnkDeactivate" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="chkUseSocialMediaPhoto" 
                                    EventName="CheckedChanged" />
                            </Triggers>
            </asp:UpdatePanel> 
       <div class="row-div label">
            <asp:HyperLink ID="hypSocialNetworkSynchText" runat="server" Target="_new" CssClass="required-label"></asp:HyperLink>
        </div>
    </div>
</div>
<cc1:AptifyWebUserLogin ID="WebUserLogin1" runat="server" Visible="False" />
<cc2:User ID="User1" runat="server" />
