<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="WebArticles.ascx.vb"
    Inherits="Aptify.Framework.Web.eBusiness.WebArticles" Debug="true" %>
<%@ Register Src="../Aptify_General/RSSFeed.ascx" TagName="RSSFeed" TagPrefix="uc1" %>

<div class="table-div">
    <div class="news-header-title clearfix">
        <div class="float-left">
        <asp:image runat="server" id="img2" imageurl="~/Images/news-updates-icon.png" CssClass="middle-img" />
        News Updates
        </div>
        <div class="float-right">
            <uc1:rssfeed id="RSSFeed" runat="server" channelid='<%# GetChannelID() %>' rsstitle="Web Articles"
                visible='<%#IsRSSVisible() %>' />
        </div>
    </div>
    <div class="row-div">
        <asp:repeater id="lstArticles" runat="server">
            <itemtemplate>
                    <div class="row-div label">
                       <asp:HyperLink ID="articleLink" runat="server"></asp:HyperLink>
                     </div>
                    <div id="StarterDiv">
                        <asp:Label ID="lblDate" runat="server"></asp:Label>
                         <hr/>
                    </div>
                </itemtemplate>
            <footertemplate>
                    </ul>
                </footertemplate>
        </asp:repeater>
    </div>
</div>
