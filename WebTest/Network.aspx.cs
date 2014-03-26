using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Network : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        archiver1.ArchiveFileUrl = "http://localhost:1153/WebTest/archive.bin";
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string hash = MTech.FileArchiver.Security.AddUser("mostafa", "123");
        archiver1.HashCode = hash;
        archiver1.Reload();
    }
}