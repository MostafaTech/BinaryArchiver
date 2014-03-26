using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace MTech.FileArchiver
{
    public abstract class FileArchiverBaseWebControl : Control, INamingContainer, ICallbackEventHandler
    {
        // Properties and variables
        public string ClientInstanceName
        {
            get
            {
                if (ViewState["FileArchiverWebControl_ClientInstanceName"] != null)
                    return ViewState["FileArchiverWebControl_ClientInstanceName"].ToString();
                else
                    return "";
            }
            set
            {
                ViewState["FileArchiverWebControl_ClientInstanceName"] = value;
            }
        }
        public string AdditionalSecurityCoockies
        {
            get
            {
                if (ViewState["FileArchiverWebControl_AdditionalSecurityCoockies"] != null)
                    return ViewState["FileArchiverWebControl_AdditionalSecurityCoockies"].ToString();
                else
                    return "";
            }
            set
            {
                ViewState["FileArchiverWebControl_AdditionalSecurityCoockies"] = value;
            }
        }
        public string HashCode
        {
            get
            {
                if (ViewState["FileArchiverWebControl_HashCode"] != null)
                    return ViewState["FileArchiverWebControl_HashCode"].ToString();
                else
                    return "";
            }
            set
            {
                ViewState["FileArchiverWebControl_HashCode"] = value;
            }
        }
        public bool EnableScan
        {
            get
            {
                if (ViewState["FileArchiverWebControl_EnableScan"] != null)
                    return Convert.ToBoolean(ViewState["FileArchiverWebControl_EnableScan"]);
                else
                    return true;
            }
            set
            {
                ViewState["FileArchiverWebControl_EnableScan"] = value;
            }
        }
        public string FilePrefix
        {
            get
            {
                if (ViewState["FileArchiverWebControl_FilePrefix"] != null)
                    return ViewState["FileArchiverWebControl_FilePrefix"].ToString();
                else
                    return "";
            }
            set
            {
                ViewState["FileArchiverWebControl_FilePrefix"] = value;
            }
        }
        public string HostNamePort
        {
            get
            {
                if (ViewState["FileArchiverWebControl_HostNamePort"] != null)
                    return ViewState["FileArchiverWebControl_HostNamePort"].ToString();
                else
                    return "";
            }
            set
            {
                ViewState["FileArchiverWebControl_HostNamePort"] = value;
            }
        }
        protected string ArchiveFileAddress { get; set; }
        private string[] Files { get; set; }
        private string output = "";

        // Methods
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Load files list
            Reload();

            string hostName = HostNamePort.Contains(":") ? HostNamePort.Substring(0, HostNamePort.IndexOf(':')) : HostNamePort;
            string hostPort = HostNamePort.Contains(":") ? HostNamePort.Substring(HostNamePort.IndexOf(':') + 1) : "80";

            // Register Styles and Scripts
            Page.ClientScript.RegisterClientScriptBlock(typeof(FileArchiverWebControl), "styles_css",
                "<link type=\"text/css\" rel=\"stylesheet\" href=\"" +
                Page.ClientScript.GetWebResourceUrl(typeof(FileArchiverCore),
                "MTech.FileArchiver.Resources.Styles.css") + "\" />");
            Page.ClientScript.RegisterClientScriptBlock(typeof(FileArchiverWebControl), "scripts_js",
                "<script type=\"text/javascript\" src=\"" +
                Page.ClientScript.GetWebResourceUrl(typeof(FileArchiverCore),
                "MTech.FileArchiver.Resources.Scripts.js") + "\"></" + "script>");
            //Page.ClientScript.RegisterClientScriptBlock(typeof(FileArchiverWebControl), "mtuploader_js",
            //    "<script type=\"text/javascript\" src=\"" +
            //    Page.ClientScript.GetWebResourceUrl(typeof(FileArchiverCore),
            //    "MTech.FileArchiver.Resources.mtuploader.js") + "\"></" + "script>");
            Page.ClientScript.RegisterClientScriptBlock(typeof(FileArchiverWebControl), "scanneractivex_js",
                "<script type=\"text/javascript\" src=\"" +
                Page.ClientScript.GetWebResourceUrl(typeof(FileArchiverCore),
                "MTech.FileArchiver.Resources.ScannerActiveX.js") + "\"></" + "script>");
            Page.ClientScript.RegisterClientScriptBlock(typeof(FileArchiverWebControl), "ctrl_scripts", (
                "<script type=\"text/javascript\">//<![CDATA[\n" +
                "function {CIN}_del(n){if(confirm('آیا از حذف این مورد اطمینان دارید؟')) " + GetCallbackScript("'{del:\\''+n+'\\',u:\\'{HC}\\'}'") + ";} \n" +
                "function {CIN}_reload(){" + GetCallbackScript("'{reload:\\'true\\',u:\\'{HC}\\'}'") + ";} \n" +
                "function {CIN}_callbackresult(d,c){var json=eval('('+d+')');" +
                " if(json.res=='body')document.getElementById('{CIN}_items').outerHTML=json.value;" +
                " if(json.res=='error')alert(json.value);" +
                "} \n" +
                "function {CIN}_opendialog() { $.colorbox({inline:true,href:'#scanner_dialog',width:'700px',height:'80%'}); }\n" +
                "var {CIN}_params={hostName:'" + hostName + "',hostPort:'" + hostPort + "',uploadPath:'" + ArchiveFileAddress + ".fa.aspx?t=up&u=" + HashCode + "',cookie:'" + Page.Session.SessionID + "; " + AdditionalSecurityCoockies + "',imageType:'img'};\n" +
                "//]]></script>")
                .Replace("{CIN}", ClientInstanceName)
                .Replace("{HC}", HashCode));
        }

        private void CreateButtons()
        {
            output += "<div id=\"{CIN}_buttons\" style=\"margin-bottom:10px;\">\n";
            output += "<a href=\"javascript:void\"><label id=\"{CIN}_btnUpload\" class=\"btnUpload\"></label></a>&nbsp;\n";
            if (EnableScan)
            {
                output += "<a href=\"javascript:void\" onclick=\"{CIN}_opendialog();\" class=\"btnScan\"></a>\n";
            }
            output += "</div>\n";
            //output += "<script type=\"text/javascript\">MT_CreateUploader('{CIN}_btnUpload', '" + ArchiveFileAddress + ".fa.aspx?t=up&u=" + HashCode + "', function(){ {CIN}_reload(); }, null);</" + "script>";
            output = output.Replace("{CIN}", ClientInstanceName);
            output += "\n";
        }

        private void CreateOutput()
        {
            // Prepare
            output += "<div id=\"{CIN}_items\">";

            // Items
            for (int i = 0; i < Files.Length; i++)
            {
                if (!Files[i].StartsWith(FilePrefix)) continue;
                output += "<div class=\"item\"><a href=\"" + ArchiveFileAddress + ".fa.aspx?t=full&u=" + HashCode + "&n=" + Files[i] + "\" target=\"_blank\" title=\"" + System.IO.Path.GetFileNameWithoutExtension(Files[i]) + "\">";
                output += "<img src=\"" + ArchiveFileAddress + ".fa.aspx?t=thumb&u=" + HashCode + "&n=" + Files[i] + "\" /></a><br />";
                output += "<div>";
                output += "<a href=\"javascript:{CIN}_del('" + Files[i] + "');\" class=\"icon delete\"></a>&nbsp;";
                output += "<a href=\"" + ArchiveFileAddress + ".fa.aspx?t=down&u=" + HashCode + "&n=" + Files[i] + "\" class=\"icon down\"></a>&nbsp;";
                output += "</div></div>";
            }

            // Fine
            output += "</div>";
            output = output.Replace("{CIN}", ClientInstanceName);
            output += "\n";
        }

        private void CreateAccessDeniedPanel()
        {
            output += "<div style=\"height:200px;text-align:center;padding-top:30px;\">";
            output += "<span class=\"access-denied\"></span><br /><span class=\"msg\">دسترسی به فایلهای این پرونده امکانپذیر نمیباشد.</span>";
            output += "</div>\n";
        }

        private void CreateScanDialog()
        {
            output += "<div style=\"display:none;\">";
            output += "<div id=\"scanner_dialog\">";
            output += "<div style=\"width:410px; float:left; display:inline-block;\">";
            output += " <div id=\"scanner_container\"></div>";
            output += "</div>";
            output += "<div style=\"width:220px; float:left; display:inline-block; text-align:center;\">";
            output += " <a href=\"javascript:;\" class=\"btn btn-primary btn-large\" style=\"width:90%;margin-bottom:5px;\" onclick=\"Archiver.Scanner.Scan({CIN}_params.imageType);\">اسکن تصویر</a><br />";
            output += " <a href=\"javascript:;\" class=\"btn btn-primary btn-large\" style=\"width:90%;margin-bottom:5px;\" onclick=\"Archiver.Scanner.Upload({CIN}_params.hostName,{CIN}_params.hostPort,{CIN}_params.uploadPath,{CIN}_params.cookie); $.colorbox.close(); {CIN}_reload();\">ارسال تصویر</a><br />";
            output += " <a href=\"javascript:;\" class=\"btn btn-primary btn-large\" style=\"width:90%;margin-bottom:5px;\" onclick=\"Archiver.Scanner.Rotate(90);\">چرخش به راست</a><br />";
            output += " <a href=\"javascript:;\" class=\"btn btn-primary btn-large\" style=\"width:90%;margin-bottom:5px;\" onclick=\"Archiver.Scanner.Rotate(270);\">چرخش به چپ</a>";
            output += "</div>";
            output += "</div>";
            output += "</div>\n";
            output = output.Replace("{CIN}", ClientInstanceName);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            output = "";
            output += "<div class=\"MTFileArchiverContainer\">\n";
            if (Security.CheckSecurity(HashCode))
            {
                CreateButtons();
                CreateOutput();
                CreateScanDialog();
            }
            else
            {
                CreateAccessDeniedPanel();
            }
            output += "<script>Archiver.Scanner.init('scanner_container');</" + "script>";
            output += "</div>\n";
            writer.Write(output);
            base.Render(writer);
        }

        protected virtual string[] LoadFilesList()
        {
            throw new Exception("Not implemented exception.");
        }

        protected virtual void RemoveFile(string filename)
        {
            throw new Exception("Not implemented exception.");
        }

        public void Reload()
        {
            Files = LoadFilesList();
        }

        // Callback
        private string callback_output = "";
        public string GetCallbackResult()
        {
            return callback_output;
        }

        public void RaiseCallbackEvent(string arg)
        {
            try
            {
                Dictionary<string, string> json = GetJson(arg);
                if (json.ContainsKey("del"))
                {
                    string filename = json["del"];
                    RemoveFile(filename);
                    Reload();
                }
                if (callback_output == "")
                {
                    CreateOutput();
                    output = output.Replace("'", "\\'");
                    callback_output = "{res:'body',value:'" + output + "'}";
                }
            }
            catch (Exception ex)
            {
                callback_output = "{res:'error',value:'" + ex.Message.Replace("'", "\\'") + "'}";
            }
        }

        public string GetCallbackScript(string method)
        {
            return Page.ClientScript.GetCallbackEventReference(this, method, ClientInstanceName + "_callbackresult", "''");
        }

        private Dictionary<string, string> GetJson(string json)
        {
            Dictionary<string, string> collection = new Dictionary<string, string>();
            if (json.StartsWith("{")) json = json.Substring(1);
            if (json.EndsWith("}")) json = json.Substring(0, json.Length - 1);
            string[] parts = json.Split(",".ToCharArray());
            for (int i = 0; i < parts.Length; i++)
            {
                string key = parts[i].Substring(0, parts[i].IndexOf(":"));
                if (key.StartsWith("'") || key.StartsWith("\"")) key = key.Substring(1);
                if (key.EndsWith("'") || key.EndsWith("\"")) key = key.Substring(0, key.Length - 1);
                string value = parts[i].Substring(parts[i].IndexOf(":") + 1);
                if (value.StartsWith("'") || value.StartsWith("\"")) value = value.Substring(1);
                if (value.EndsWith("'") || value.EndsWith("\"")) value = value.Substring(0, value.Length - 1);
                collection.Add(key, value);
            }
            return collection;
        }
    }
}
