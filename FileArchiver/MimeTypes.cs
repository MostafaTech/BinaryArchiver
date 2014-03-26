using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTech.FileArchiver
{
    public class MimeTypes
    {

        public static string GetMimeType(string ext)
        {
            for (int i = 0; i < MimeTypesTable.Length; i++)
                if (MimeTypesTable[i].Extention == ext)
                    return MimeTypesTable[i].Mime;
            return "";
        }

        public string Extention;
        public string Mime;
        public MimeTypes(string extention, string mime)
        {
            this.Extention = extention;
            this.Mime = mime;
        }

        public static MimeTypes[] MimeTypesTable = {
            new MimeTypes(".jpg","image/jpeg"), new MimeTypes(".jpeg","image/jpeg"),
            new MimeTypes(".gif","image/gif"),  new MimeTypes(".png","image/png"),
            new MimeTypes(".tif","image/tiff"), new MimeTypes(".tiff","image/tiff"),
            new MimeTypes(".txt","text/plain"), new MimeTypes(".bmp","image/bmp"),
            new MimeTypes(".zip","application/x-compressed"), new MimeTypes(".pdf","application/pdf"),
            new MimeTypes(".doc","application/msword"),       new MimeTypes(".docx","application/msword"),
            new MimeTypes(".xls","application/excel"),        new MimeTypes(".xlsx","application/excel"),
            new MimeTypes(".ppt","application/mspowerpoint"), new MimeTypes(".pptx","application/mspowerpoint")
        };
    }
}
