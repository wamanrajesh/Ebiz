<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Reports.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.Chapters.ReportsControl" %>
<%@ Register TagPrefix="cc4" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessHierarchyTree" %>
<%@ Register TagPrefix="cc3" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="radTree" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<div class="table-div">
    <div class="row-div">
        <asp:LinkButton ID="lnkChapter" runat="server">
				Go To Chapter
			</asp:LinkButton>
    </div>
    <div class="row-div clearfix">
        <div class="left-container w18">
            <radtree:radtreeview id="trvReports" runat="server" checkboxes="False" onnodeclick="trvReports_NodeClicked"
                clientidmode="Static" expandanimation-type="None" causesvalidation="false">
				<NodeTemplate>
					<asp:Label ID="lblReport" runat="server">
					</asp:Label>
				</NodeTemplate>
			</radtree:radtreeview>
        </div>
        <div class="right-container w80">
            <asp:UpdatePanel ID="updPanelGrid" runat="server">
                <contenttemplate>
					        <asp:GridView ID="grdReports" runat="server" AutoGenerateColumns="False" CssClass="w100 gridview-table" AlternatingRowStyle-BackColor="White">
						        <HeaderStyle CssClass="grid-viewheader"/>        
                                <FooterStyle CssClass="grid-footer" />
                                <RowStyle CssClass="grid-item-style" />           
                                <PagerStyle CssClass="paging-style"/> 
                                <Columns>
							        <asp:TemplateField HeaderText="Report">
								        <ItemTemplate>
									        <asp:HyperLink ID="lnkReport" runat="server">
									        </asp:HyperLink>
								        </ItemTemplate>
							        </asp:TemplateField>
							        <asp:BoundField DataField="Description" HeaderText="Description" />
							        <asp:TemplateField Visible="false" HeaderText="ID">
								        <ItemTemplate>
									        <asp:Label ID="lblID" runat="server" text= '<%#Eval("ID") %>'></asp:Label>
									        </ItemTemplate>
								        </asp:TemplateField>
							        </Columns>
						        </asp:GridView>
					        </contenttemplate>
                <triggers>
						        <asp:AsyncPostBackTrigger ControlID = "grdReports" EventName="PageIndexChanging" />
					        </triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <cc3:user id="User1" runat="server" />
</div>
