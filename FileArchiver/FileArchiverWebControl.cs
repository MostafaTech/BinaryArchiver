using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace MTech.FileArchiver
{
    [ToolboxData("<{0}:FileArchiverWebControl runat='server'></{0}:FileArchiverWebControl>")]
    public class FileArchiverWebControl : FileArchiverBaseWebControl
    {
        // Properties and variables
        public string ArchiveFile
        {
            get
            {
                if (ViewState["FileArchiverWebControl_ArchiveFile"] != null)
                    return ViewState["FileArchiverWebControl_ArchiveFile"].ToString();
                else
                    return "";
            }
            set
            {
                ViewState["FileArchiverWebControl_ArchiveFile"] = value;
            }
        }
        public FileArchiverCore archiver;
        public FileArchiverCore Archiver { get { return archiver; } }

        // Methods
        protected override void OnLoad(EventArgs e)
        {
            archiver = new FileArchiverCore(Page.MapPath(ArchiveFile));
            base.ArchiveFileAddress = ResolveUrl(ArchiveFile);
            base.OnLoad(e);
        }

        protected override string[] LoadFilesList()
        {
            string[] list = new string[archiver.Files.Length];
            for (int i = 0; i < list.Length; i++)
                list[i] = archiver.Files[i].Name;
            return list;
        }

        protected override void RemoveFile(string filename)
        {
            archiver.RemoveFile(filename);
        }
    }
}
