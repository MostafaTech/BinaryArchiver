<%@ WebHandler Language="C#" Class="upload" %>

using System;
using System.Web;

public class upload : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.Expires = -1;
        context.Response.ContentType = "text/plain";

        if (context.Request.Files.Count > 0)
        {
            string _filename = System.IO.Path.GetRandomFileName().Replace(".", "") + ".jpg";
            context.Request.Files[0].SaveAs(context.Server.MapPath("~/scan/" + _filename));
        }

        context.Response.Write("200");
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}