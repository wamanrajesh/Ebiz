<%--Aptify e-Business 5.5.1, July 2013--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="HomeCurrentDiscussion.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.Aptify_General.HomeCurrentDiscussion" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>

    <asp:ListView ID="lstCurrentDiscussion" runat="server">
        <LayoutTemplate>
            <div class="disscussion-header-title">
                <asp:Image runat="server" ID="img2" ImageUrl="~/Images/discussion-icon.png" CssClass="MiddleImage" />
                <asp:Label runat="server" ID="Label1" Text="Current Discussion" />
            </div>
            <div class="rTableRow" id="itemPlaceholder" runat="server">
            </div>
        </LayoutTemplate>
        <ItemTemplate>
            <%-- Neha changes Issue 16001,05/07/13--%>
        <div class="row-div-bottom-line clearfix">
            <div class="float-left right-margin" >
                <rad:RadBinaryImage ID="imgProfileRad" runat="server" AutoAdjustImageControlSize="false"
                    ResizeMode="Fill" CssClass="PeopleImage" />
            </div>
            <div class="float-left w47" >
                <asp:LinkButton ID="lnkTopic" runat="server" CommandName="disscussion" CommandArgument='<% #Eval("ForumID") %>'
                    Style="cursor: pointer;" ToolTip='<% #Eval("Subject") %>'></asp:LinkButton>
                <%-- <asp:HyperLink ID="lnkTopic" runat="server" Text='<% #Eval("Subject") %>' style="cursor:pointer;"></asp:HyperLink>--%>
            
                <div class="gray-font">
                    Started By:
                    <asp:Label ID="lblStarter" runat="server" Text='<% #eval("webUserName") %>'></asp:Label>
                </div>
                <%-- <div id="LastReplyDiv" class="tablelightfont" style="width:206px;">
 <asp:Label ID="lblLastReply" runat="server" Text='<% #Eval("Body") %>'></asp:Label>
 </div>--%>
            </div>
            <%--Dilip changes--%>
            <div class="float-right w25">
                <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/reply-icon.png" />
                <div>
                    <asp:Label ID="lblReplyCount" runat="server" Text='<% #Eval("ChildCount") %>'></asp:Label>
                    Replies
                </div>
            </div>
            </div>
            <%--<div class="rTableCell">
 <asp:Image ID="Image3" runat="server" ImageUrl="~/Images/Aptify_UC_Img/like-icon.png" />
 <div>
 <asp:Label ID="lblLikesCount" runat="server" Text="1 likes"></asp:Label>
 </div>
 </div>--%>

        </ItemTemplate>
    </asp:ListView>

<asp:Label ID="lblsfMessage" runat="server"></asp:Label>
<cc2:User runat="Server" ID="User1" />