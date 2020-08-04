<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="WebServicesDefault.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.WebServices.DefaultControl" %>

<div class="table-div">
   <div class="web-title"">
         Aptify e-Business XML Web Services
          </div><hr/>
          <div class="web-title-p">
          <p>XML Web services are the fundamental 
              building blocks in the move to distributed computing on the Internet. 
              Applications are constructed using multiple XML Web services from various 
              sources that work together regardless of where they reside or how they were 
              implemented.<br /><br />
          Aptify and Aptify Enterprise Web 
              Architecture Components form a layer of advanced technology atop Microsoft 
              .NET, ASP.NET and the XML Web Services Engine in .NET.<br /><br />
          The power of Aptify and Aptify EWA, 
              enable 100% of your Aptify-based applications to directly be leveraged to 
              the web via .NET Web Forms and .NET XML Web Services.<br /></p>
          </div>
      
    <div class="row-div label">For examples of XML Web Services, 
                select from the options below:</div>
        <div class="row-div">
          <ul>
            <li>
              <asp:HyperLink ID="lnkOrderInfo" runat="server">Web Service: Get Order Information</asp:HyperLink>
            </li>
            <li>
              <asp:HyperLink ID="lnkUpdatePerson" runat="server">Web Service: Update Person Information</asp:HyperLink>
            </li>
            <li>
              <asp:HyperLink ID="lnkProductListing" runat="server">Web Service: Get Product Listing</asp:HyperLink>
            </li>
          </ul>
          <hr/>
        </div>
</div>
