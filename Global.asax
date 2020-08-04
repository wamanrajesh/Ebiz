<%@ Application Inherits="Aptify.Framework.Web.eBusiness.EBusinessGlobal" Language="C#" %>

<script runat="server">

	public override string GetVaryByCustomString(HttpContext context, string custom)
	{
    // Added this by after dsicussion with sitefinity team
        if (custom.Equals("cms", StringComparison.CurrentCultureIgnoreCase))
        {
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                return HttpContext.Current.Request.Path;
            }
        }
		return base.GetVaryByCustomString(context, custom);	
	}

    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup

    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    {
        // DONE IN WEB.CONFIG
        //Exception ex = Server.GetLastError();
        //if (ex is HttpException)
        //{
        //    HttpException httpEx = ex as HttpException;
        //    if (httpEx.ErrorCode == 403 || httpEx.Source.StartsWith("Telerik.Cms"))
        //    {
        //        Response.Redirect("~/Sitefinity/nopermissions.aspx");
        //        Server.ClearError();
        //    }
        //}
        //else if (ex is Telerik.Security.SecurityApplicationException)
        //{
        //    Response.Redirect("~/Sitefinity/nopermissions.aspx");
        //    Server.ClearError();
        //}
    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started
        base.Session_Start(sender, e);
    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
</script>
