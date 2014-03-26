using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace MTech.FileArchiver
{
    [ToolboxData("<{0}:FileArchiverNetworkWebControl runat='server'></{0}:FileArchiverNetworkWebControl>")]
    public class FileArchiverNetworkWebControl : FileArchiverBaseWebControl
    {
        // Properties and variables
        public string ArchiveFileUrl
        {
            get
            {
                if (ViewState["FileArchiverWebControl_ArchiveFileUrl"] != null)
                    return ViewState["FileArchiverWebControl_ArchiveFileUrl"].ToString();
                else
                    return "";
            }
            set
            {
                ViewState["FileArchiverWebControl_ArchiveFileUrl"] = value;
            }
        }

        // Methods
        protected override void OnLoad(EventArgs e)
        {
            base.ArchiveFileAddress = this.ArchiveFileUrl;
            base.OnLoad(e);
        }

        protected override string[] LoadFilesList()
        {
            System.Net.WebClient client = new System.Net.WebClient();
            string serviceurl = ArchiveFileUrl + ".fa.aspx?t=list&u=" + HashCode;
            string data = client.DownloadString(serviceurl);
            return data.Split("|".ToCharArray());
        }

        protected override void RemoveFile(string filename)
        {
            System.Net.WebClient client = new System.Net.WebClient();
            string serviceurl = ArchiveFileUrl + ".fa.aspx?t=del&u=" + HashCode + "&n=" + filename;
            string data = client.DownloadString(serviceurl);
        }
    }
}
