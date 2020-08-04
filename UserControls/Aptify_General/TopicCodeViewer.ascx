<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="TopicCodeViewer.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.TopicCodeViewer" %>
<%@ Register TagPrefix="cc4" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessHierarchyTree" %>
<%@ Register TagPrefix="cc6" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="radTree" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<script type="text/javascript">
    Telerik.Web.UI.RadTreeView.prototype._onKeyDown = function (e) { } 
</script>
<asp:UpdatePanel ID="update1" runat="server">
    <ContentTemplate>
        <div id="tblMain" runat="server" class="table-div">
            <div id="tdtopic" runat="server" class="row-div">
                <asp:Label runat="server" ID="lblUser" Visible="false">
                </asp:Label>
                <asp:Label runat="server" ID="lblinterest">
                </asp:Label>
            </div>
            <div id="tdtopiccode" runat="server" class="top-margin">
                <asp:Label ID="Topicskeep" runat="server" Text="Topics keep track of what areas you are most interested in. By providing this information to the site, the features of each area of the site will reflect your interests more directly and allow us to serve you better.">
                </asp:Label>
            </div>
            <div class="top-margin">
                <asp:Label ID="MsgBlankTopiccode" runat="server" Text="There is no Topic Code available for selection at this time.">
                </asp:Label>
            </div>
            <div class="row-div">
                <asp:Button ID="cmdSave" CssClass="submit-Btn" runat="server" Text="Save"></asp:Button>
                <asp:Button ID="cmdCheckAll" runat="server" CssClass="submit-Btn" Text="Check All">
                </asp:Button>
                <asp:Button ID="cmdClearAll" runat="server" CssClass="submit-Btn" Text="Clear"></asp:Button>
                <asp:Label ID="lblDescription" runat="server" Visible="False">
                </asp:Label>
            </div>
            <div class="row-div">
                <radTree:RadTreeView ID="trvTopicCodes" runat="server" CheckBoxes="True"
                    ClientIDMode="Static" ExpandAnimation-Type="None" CausesValidation="false">
                    <NodeTemplate>
                        <asp:Label ID="lblTopicCode" runat="server">
                        </asp:Label>
                        <asp:DropDownList ID="ddlTopicCode" runat="server">
                        </asp:DropDownList>
                        <asp:TextBox ID="txtTopicCode" runat="server">
                        </asp:TextBox>
                    </NodeTemplate>
                </radTree:RadTreeView>
                <asp:Label ID="lblEntityID" runat="server" Visible="false" CssClass="label">
                    </asp:Label>
                    <asp:Label ID="lblRecordID" runat="server" Visible="false" CssClass="label">
                    </asp:Label>
                    <asp:Label ID="lblEntityName" runat="server" Visible="false" CssClass="label">
                    </asp:Label>
              </div>
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="trvTopicCodes" EventName="" />
    </Triggers>
</asp:UpdatePanel>
<cc6:User ID="User1" runat="server" />
