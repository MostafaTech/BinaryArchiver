using System;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;

namespace MTech.FileArchiver
{
    public class HttpHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            string root_abs_path = VirtualPathUtility.ToAbsolute("~/");
            string archive_file_vpath = "~/" + context.Request.RawUrl.Substring(root_abs_path.Length).Replace(".fa.aspx?", "?");
            archive_file_vpath = archive_file_vpath.Substring(0, archive_file_vpath.IndexOf("?"));
            string archive_file = context.Server.MapPath(archive_file_vpath);
            string filename = (!string.IsNullOrEmpty(context.Request.QueryString["n"]) ? context.Request.QueryString["n"] : "");
            string requesttype = (!string.IsNullOrEmpty(context.Request.QueryString["t"]) ? context.Request.QueryString["t"] : "");
            string hashcode = (!string.IsNullOrEmpty(context.Request.QueryString["u"]) ? context.Request.QueryString["u"] : "");
            string width = (!string.IsNullOrEmpty(context.Request.QueryString["w"]) ? context.Request.QueryString["w"] : "");
            string fixed_height = (!string.IsNullOrEmpty(context.Request.QueryString["fh"]) ? context.Request.QueryString["fh"] : "t");
            string watermark = (!string.IsNullOrEmpty(context.Request.QueryString["wm"]) ? context.Request.QueryString["wm"] : "0");
            try
            {
                if (Security.CheckSecurity(hashcode))
                {
                    FileArchiverCore core = new FileArchiverCore(archive_file);
                    if (requesttype == "thumb")
                    {
                        context.Response.ContentType = "image/png";
                        Bitmap thumb = core.GetIcon(filename, width, fixed_height);
                        System.IO.MemoryStream memresult = new System.IO.MemoryStream();
                        thumb.Save(memresult, System.Drawing.Imaging.ImageFormat.Png);
                        memresult.WriteTo(context.Response.OutputStream);
                        thumb.Dispose();
                        memresult.Dispose();
                        thumb = null;
                        memresult = null;
                    }
                    if (requesttype == "full")
                    {
                        byte[] contents = core.GetFileContents(filename);
                        AddWatermark(ref contents);
                        context.Response.BinaryWrite(contents);
                        context.Response.ContentType = MimeTypes.GetMimeType(System.IO.Path.GetExtension(filename));
                    }
                    if (requesttype == "down")
                    {
                        byte[] contents = core.GetFileContents(filename);
                        AddWatermark(ref contents);
                        context.Response.BinaryWrite(contents);
                        context.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                    }
                    if (requesttype == "up")
                    {
                        if (context.Request.Files.Count > 0)
                        {
                            core.AddFile(
                                context.Request.Files[0].FileName,
                                context.Request.Files[0].InputStream);
                            context.Response.Write("{status:'success'}");
                        }
                    }
                    if (requesttype == "del")
                    {
                        core.RemoveFile(filename);
                        context.Response.Write("{status:'success'}");
                    }
                    if (requesttype == "list")
                    {
                        string files = "";
                        for (int i = 0; i < core.Files.Length; i++)
                            files += core.Files[i].Name + "|";
                        if (files.EndsWith("|")) files = files.Remove(files.Length - 1);
                        context.Response.Write(files);
                    }
                }
                else
                {
                    throw new Exception("Not logged in.");
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("{status:'error',msg:'" + ex.Message.Replace("'", "\\'").Replace("\\", "\\\\") + "',detail:'" + ex.ToString().Replace("'", "\\'").Replace("\\", "\\\\") + "'}");
            }
        }

        public void AddWatermark(ref byte[] contents)
        {
            string watermark_enabled = System.Configuration.ConfigurationManager.AppSettings["FileArchiver.Watermark.Enabled"];
            string watermark_filepath = System.Configuration.ConfigurationManager.AppSettings["FileArchiver.Watermark.FilePath"];
            watermark_filepath = HttpContext.Current.Server.MapPath(watermark_filepath);
            string watermark_transparency = System.Configuration.ConfigurationManager.AppSettings["FileArchiver.Watermark.Transparency"];
            string copyright = HttpContext.Current.Request.QueryString["cr"];
            copyright = (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["cr"]) ? HttpContext.Current.Request.QueryString["cr"] : "");
            if (string.IsNullOrEmpty(watermark_transparency)) watermark_transparency = "0.5";
            if (watermark_enabled == "1")
            {
                System.IO.MemoryStream mem = new System.IO.MemoryStream(contents);
                System.IO.MemoryStream mem2 = new System.IO.MemoryStream();
                Image img = Bitmap.FromStream(mem);
                Graphics g = Graphics.FromImage(img);
                Image imgWatermark = Bitmap.FromFile(watermark_filepath);
                int tiles_count_x = (img.Width / imgWatermark.Width) + 1;
                int tiles_count_y = (img.Height / imgWatermark.Height) + 1;
                Image imgBigWatermark = new Bitmap(
                    imgWatermark.Width * tiles_count_x, 
                    imgWatermark.Height * tiles_count_y, 
                    PixelFormat.Format32bppPArgb);
                Graphics gBigWatermark = Graphics.FromImage(imgBigWatermark);
                for (int i = 0; i < tiles_count_x; i++)
                {
                    for (int j = 0; j < tiles_count_y; j++)
                    {
                        gBigWatermark.DrawImage(imgWatermark, 
                            i * imgWatermark.Width, j * imgWatermark.Height, 
                            imgWatermark.Width, imgWatermark.Height);
                    }
                }
                // Rotation
                imgBigWatermark = ImageManipulation.ImageTranslate(imgBigWatermark, 0, 0, imgBigWatermark.Width, imgBigWatermark.Height, -30);
                // Transparency
                ImageAttributes imageAttributes = ImageManipulation.GetTransparencyAttributes(Convert.ToSingle(watermark_transparency));
                // Drawing
                int watermark_pos_x = -((imgBigWatermark.Width - img.Width) / 2);
                int watermark_pos_y = -((imgBigWatermark.Height - img.Height) / 2);
                g.DrawImage(imgBigWatermark,
                    new Rectangle(watermark_pos_x, watermark_pos_y, imgBigWatermark.Width, imgBigWatermark.Height),
                    0, 0, imgBigWatermark.Width, imgBigWatermark.Height,
                    GraphicsUnit.Pixel, imageAttributes);
                // Copyright
                if (copyright != "")
                {
                    Font copyright_font = new Font("Tahoma", 13);
                    g.DrawString(copyright, copyright_font, Brushes.Black, 20f, img.Height - 50);
                }
                // Save
                img.Save(mem2, ImageFormat.Jpeg);
                contents = mem2.ToArray();
                // Disposing
                gBigWatermark.Dispose();
                imgBigWatermark.Dispose();
                imgWatermark.Dispose();
                g.Dispose();
                img.Dispose();
                mem.Close();
                mem.Dispose();
                mem2.Close();
                mem2.Dispose();
            }
        }
    }
}
