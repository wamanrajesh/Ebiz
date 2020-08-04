<%--Aptify e-Business 5.5.1, July 2013--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="DisableAutoRenewalMemberships.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.DisableAutoRenewalMemberships" %>
<%@ Register TagPrefix="Uc1" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="Uc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="EBusinessShoppingCart" %>
<script language="javascript" type="text/javascript">

    function GetRadWindow() {

        var oWindow = null;

        if (window.radWindow)

            oWindow = window.RadWindow; //Will work in Moz in all cases, including clasic dialog      

        else if (window.frameElement.radWindow)

            oWindow = window.frameElement.radWindow; //IE (and Moz as well)      

        return oWindow;

    }

    function Close() {

        GetRadWindow().Close();

    }          
    
</script>
<div>
                   <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                    
                     <div>
                    <asp:label ID="lblmsg" runat="server" Font-Bold="true" Text=""></asp:label>
                    </div>
                    <table id="Table1" runat="server" width="100%" class="data-form">
                    <tr><td><div>
                   <asp:label ID="lblGridInfo" runat="server" Font-Bold="true" Text=""></asp:label>
                      
                   </div></td></tr>
                    <tr>
                    <td>
                    <rad:RadGrid ID="grdAutoRenewalMemberships" runat="server"  AutoGenerateColumns="False" AllowPaging="true"
                         AllowFilteringByColumn="True">
                         <GroupingSettings CaseSensitive="false" />
                         <MasterTableView AllowFilteringByColumn="true"  AllowSorting="true">
                      
                            <Columns>
                            <rad:GridTemplateColumn AllowFiltering="false"    >
                              <ItemStyle HorizontalAlign="Center" CssClass="gridAlign" ></ItemStyle>
                              <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"  />
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkRenewal" runat="server"   OnCheckedChanged="ToggleRowSelection"
                                                AutoPostBack="True" />
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="headerChkbox" runat="server" OnCheckedChanged="ToggleSelectedState"
                                                AutoPostBack="True" />
                                        </HeaderTemplate>
                                    </rad:GridTemplateColumn>

                                    <rad:GridTemplateColumn DataField="SubscriptionsID" UniqueName="SubscriptionsID"  HeaderText="Subscriptions ID" SortExpression="SubscriptionsID" FilterControlWidth="90px"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false" >
                                        <ItemStyle HorizontalAlign="Left"  CssClass="gridAlign" ></ItemStyle>
                              <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <ItemTemplate>
                                          <asp:Label ID="lblSubscriptionsID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"SubscriptionsID") %>'> </asp:Label>
                                        </ItemTemplate>
                                        </rad:GridTemplateColumn>

                                          <rad:GridTemplateColumn Visible="false" DataField="ProductID" UniqueName="ProductID"  HeaderText="ProductID">
                                        <ItemTemplate>
                                          <asp:Label ID="lblId" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ProductID") %>'> </asp:Label>
                                        </ItemTemplate>
                                        </rad:GridTemplateColumn>
                                    
                                    <rad:GridTemplateColumn DataField="Subscriber" UniqueName="Subscriber" HeaderText="Name" SortExpression="Subscriber" FilterControlWidth="180px"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" >
                                          <ItemStyle HorizontalAlign="Center" Width="200px" CssClass="LeftAlign"></ItemStyle>
                                       <HeaderStyle HorizontalAlign="left" VerticalAlign="Top" />     
                                        <ItemTemplate>
                                          
                                            
                                                <div>
                                                    <table>
                                                        <tr>
                                                            <td >
                                                               
                                                                      <asp:Image ID="RadBinaryImgPhoto" CssClass="imgmember" runat="server" />

                                                            </td>
                                                                                                         <td class="memberListtd">
                                               <asp:Label ID="lblSubscriber" CssClass="namelink" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Subscriber") %>'></asp:Label><br />
                                                    <asp:Label ID="lblTitle" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"title") %>'></asp:Label><br />
                                                    <asp:Label ID="lbladdress" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"city") %>'> </asp:Label>
                                                </td>
   
                                                        </tr>
                                                    </table>
                                                </div>
                                          
                                        </ItemTemplate>
                                    </rad:GridTemplateColumn>

                     <rad:GridTemplateColumn HeaderText="Email"    ItemStyle-Wrap="true" HeaderStyle-Width="150px"  DataField="Email"  SortExpression="Email" FilterControlWidth="150px"  AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                    <ItemStyle HorizontalAlign="left"  VerticalAlign="Middle" Width="150px" CssClass="Emailstyle" Wrap="true"></ItemStyle>
                                      <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle" />
                                    <ItemTemplate>            
                                       <asp:Label ID="lblEmail"  HeaderText="Email" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Email") %>'> </asp:Label>                                     
                                    </ItemTemplate>
                                    <HeaderStyle Width="150px" VerticalAlign="Top" />
                                </rad:GridTemplateColumn> 

                                     <rad:GridTemplateColumn DataField="MembershipType" FilterControlWidth="110px" UniqueName="MembershipType"  HeaderText="Membership Type" SortExpression="MembershipType"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                        <ItemStyle  VerticalAlign="Middle" />
                                          <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle" />
                                        <ItemTemplate>
                                          <asp:Label ID="lblMembershipType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"MembershipType") %>'> </asp:Label>
                                        </ItemTemplate>
                                        </rad:GridTemplateColumn>
                                  
                                    <rad:GridBoundColumn UniqueName="EndDate"  HeaderText="End Date" FilterControlWidth="100px" DataField="EndDate"  SortExpression="EndDate"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"/>
                                                      
                               <%-- <rad:GridTemplateColumn HeaderTooltip="Apply this product for all members"  HeaderText="Membership Type" AllowFiltering="false" UniqueName="MemberType">
                                    <ItemStyle HorizontalAlign="left" CssClass="gridAlign"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="left" />
                                    <HeaderTemplate>
                                    Select Product                                 
                                    <asp:DropDownList ID="ddlHeaderMemberType"  CssClass="ddlStyleMeeting" Height="18px" runat="server" OnSelectedIndexChanged="ddlHeaderMemberTypeChanged" AutoPostBack="True">
                                    </asp:DropDownList>
                                    </HeaderTemplate>                                  
                                    <ItemTemplate>
                                    <asp:DropDownList ID="ddlMemberType"  runat="server" Width="150px"  CssClass="ddlStyleMeeting"  OnSelectedIndexChanged="ddlMemberTypeChanged" AutoPostBack="True">
                                    </asp:DropDownList>                                   
                                    </ItemTemplate>
                                </rad:GridTemplateColumn>  --%>                           
                               <%--   <rad:GridTemplateColumn HeaderText="Price" AllowFiltering="false" UniqueName="Price">
                                    <ItemStyle HorizontalAlign="Right"  CssClass="gridAlign"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Right" VerticalAlign="Top"   Width="150px"/>                                                                
                                    <ItemTemplate>
                                    <asp:Label ID="lblPrice" runat="server" ></asp:Label>                                                                    
                                    </ItemTemplate>
                                </rad:GridTemplateColumn>--%>
                                 <%--  <rad:GridTemplateColumn HeaderText="Auto Renew" AllowFiltering="false">
                               <HeaderStyle Width="130px" VerticalAlign="Top" />
                                    <ItemStyle HorizontalAlign="left"  CssClass="gridAlign"></ItemStyle>                                   
                              >--%>

                    <rad:GridTemplateColumn DataField="AutoRenew" HeaderText="AutoRenew" AllowFiltering="false"  UniqueName="AutoRenew">  
                    <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle" />
                         <ItemTemplate>
                                      
                                        <asp:DropDownList ID="ddlAutoRenew" CssClass="ddlStyleMeeting" AutoPostBack="true" runat="server" >                                                                         
                                                <asp:ListItem Value="0"  Text="No"></asp:ListItem>                        
                                                <asp:ListItem Value="1"  Text="Yes"></asp:ListItem>                                               
                                        </asp:DropDownList>  
                                        <asp:Label ID="lblAutoRenew" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem,"AutoRenew") %>'> </asp:Label>                                     
                                    </ItemTemplate>
                      
                    </rad:GridTemplateColumn> 



                              <%--  </rad:GridTemplateColumn>      --%>                        
                            </Columns>
                        </MasterTableView>
                    </rad:RadGrid>
                  <%--  </div>          --%>                    
                   </td>
                   </tr>           
            </table>  
            
                           <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                          <rad:RadWindow ID="UserListDialog" runat="server" Width="300px" Height="120px" Modal="True"
                BackColor="#f4f3f1" Skin="Default" VisibleStatusbar="False" Behaviors="None" ForeColor="#BDA797"
                Title="Disable Auto Renewal Memberships" Behavior="None" >
                <ContentTemplate>
                    <table width="100%" cellpadding="0" cellspacing="0" style="background-color: #f4f3f1;
                        height: 100%; padding-left: 5px; padding-right: 5px; padding-top: 5px;">
                        <tr>
                            <td align="center">
                                <asp:Label ID="lblSelections" runat="server" Font-Bold="true" ></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Button ID="btnok" runat="server" Text="OK" Width="70px" class="submitBtn" 
                                    ValidationGroup="ok" />&nbsp;&nbsp;
                              </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </rad:RadWindow>
            </ContentTemplate></asp:UpdatePanel>
  
                    </ContentTemplate>  
                    </asp:UpdatePanel>




                  <table width="97%">
 <tr>
                <td align="right" >            
                   <div style="float:right;padding:10px;" >                    
                        <asp:Button ID="btnUpdateMemberships" CssClass="submitBtn" runat="server" Text="Update" />                    
                    </div>         </td>
            </tr>
 </table>
</div>






<Uc1:User ID="User1" runat="server"></Uc1:User>
<Uc2:AptifyShoppingCart ID="ShoppingCart1" runat="server" Visible="False"></Uc2:AptifyShoppingCart>