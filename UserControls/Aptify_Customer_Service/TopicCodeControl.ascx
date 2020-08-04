<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="TopicCodeControl.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.CustomerService.TopicCodeControl" %>
<%@ Register TagPrefix="radTree" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessShoppingCart" %>
<%@ Register TagPrefix="cc6" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="uc1" TagName="TopicCodeViewer" Src="~/UserControls/Aptify_General/TopicCodeViewer.ascx" %>
<div class="Table-div">
    <uc1:TopicCodeViewer ID="TopicCodeViewer" runat="server">
    </uc1:TopicCodeViewer>
</div>
<cc6:User ID="User1" runat="server" />
