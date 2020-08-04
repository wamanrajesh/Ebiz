<%--Aptify e-Business 5.5.1, July 2013--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ForumTree.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.ForumTree" %>
<%@ Register TagPrefix="cc4" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessHierarchyTree" %>
<%@ Register TagPrefix="cc6" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="radTree" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<script type="text/javascript">
    Telerik.Web.UI.RadTreeView.prototype._onKeyDown = function (e) { } 
</script>
<asp:UpdatePanel ID="update1" runat="server">
    <ContentTemplate>
        <div>
            <radTree:RadTreeView ID="trvDiscussionForum" runat="server" Margin="8" CheckBoxes="True" 
                ClientIDMode="Static" ExpandAnimation-Type="None" CausesValidation="false" CssClass="for-tree-structure-without-leftpadding" >
                <NodeTemplate>
                    <asp:Label ID="lblDiscussionForum" runat="server"></asp:Label> 
                </NodeTemplate>
            </radTree:RadTreeView> 
        </div>
    </ContentTemplate>
    <Triggers >
        <asp:AsyncPostBackTrigger ControlID="trvDiscussionForum" EventName="" />
    </Triggers>
</asp:UpdatePanel>
<cc6:User ID="User1" runat="server" />