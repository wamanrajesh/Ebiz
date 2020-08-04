<%--Aptify e-Business 5.5.1 SR1, June 2014--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="QuestionTree.ascx.vb" Inherits="Aptify.Framework.Web.eBusiness.Surveys.QuestionTreeControl" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness.Knowledge.Controls" Assembly="AptifyKnowledgeWebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Aptify.Framework.Web.eBusiness" Assembly="AptifyEBusinessUser" %>

<div class="table-div">
    <script language="javascript" type="text/javascript">
	    window.history.forward(1);
    </script>
	
	<cc2:QuestionTreeControl id="ctlQuestionTree" runat="server" />
	
    <cc2:User ID="User1" runat="server" />
</div>