<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Forums.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.Forums.ForumsControl" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="PageSecurity" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<div runat="server" id="trHeaderRow" class="meeting-discussionforumheader">
        Sub Forums
</div>
<div class="row-div">
    <asp:DataList ID="lstForums" SkinID="lstForums2" runat="server">
        <HeaderTemplate>
        </HeaderTemplate>
        <ItemTemplate>
            <div>
                <%="<img src=""" & Me.ForumImage%><%# CStr(DataBinder.Eval(Container.DataItem, "Type")).Trim & ".gif"">"%>
                <%#"<a href=" & Chr(34) & Me.ForumPage & "?ID=" & _
                    CStr(DataBinder.Eval(Container.DataItem, "ID")) & Chr(34) & ">"%>
                <%#DataBinder.Eval(Container.DataItem, "NameWCount")%>
                </a>
                <br>
                <%# DataBinder.Eval(Container.DataItem,"Description")%>
            </div>
        </ItemTemplate>
        <FooterTemplate>
        </FooterTemplate>
    </asp:DataList>
</div>
<div class="tabForums">
    <table id="tabForums" runat="server">
    </table>
</div>
<cc2:User ID="User1" runat="server" />
