<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Surveys.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.Knowledge.Surveys" %>
<%@ Register TagPrefix="cc1" Assembly="AptifyKnowledgeWebControls" Namespace="Aptify.Framework.Web.eBusiness.Knowledge.Controls" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<div class="table-div review">
    <cc1:QuestionTreeListControl ID="QuestionTreeList" runat="server" AllowSorting="True"
        AutoGenerateColumns="False" PersonID="-1" AlternatingItemStyle-BackColor="White"
        ItemStyle-BackColor="#e5e2dd" Width="100%" BorderStyle="None" CssClass="review" >
        <HeaderStyle/>
        <FooterStyle/>
        <ItemStyle/>
        <Columns>
            <asp:TemplateColumn HeaderText="Category">
                <HeaderStyle CssClass="border-none"/>
                <ItemStyle CssClass="border-none" />
                <ItemTemplate>
                    <%#GetCategoryName(Container)%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Name">
                <HeaderStyle CssClass="border-none" />
                <ItemStyle CssClass="border-none label" />
                <ItemTemplate>
                    <%# GetQuestionTreeName(Container) %>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Description">
                <HeaderStyle CssClass="border-none" />
                <ItemStyle CssClass="border-none" />
                <ItemTemplate>
                    <%# GetQuestionTreeDescription(Container) %>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn Visible="False" HeaderText="Tracking Type">
                <HeaderStyle CssClass="border-none" />
                <ItemStyle CssClass="border-none" />
                <ItemTemplate>
                    <%# GetTrackingTypeName(Container) %>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn Visible="False" HeaderText="Complete">
                <HeaderStyle CssClass="border-none" />
                <ItemStyle CssClass="border-none" />
                <ItemTemplate>
                    <%# GetSessionIsCompleted(Container) %>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn Visible="False" HeaderText="Start New">
                <HeaderStyle CssClass="border-none" />
                <ItemStyle CssClass="border-none" />
                <ItemTemplate>
                    <%# GetNewResultLink(Container) %>
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </cc1:QuestionTreeListControl>
    <cc3:User runat="server" ID="User1" />
</div>
