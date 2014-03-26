using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace MTech.FileArchiver
{
    public class FileArchiverCore
    {
        public string ArchiveFileName { get; set; }
        private ArchiveFile[] files = new ArchiveFile[0];
        public ArchiveFile[] Files { get {return files;} }
        private long end_of_headers = 0;
        private long ContentSection = 10240;
        private byte VersionMajor = 0;
        private byte VersionMinor = 0;

        public FileArchiverCore(string archivefile)
        {
            this.ArchiveFileName = archivefile;
            Reload();
        }

        public void Reload()
        {
            // Load list of files
            FileStream fs = null;
            if (File.Exists(ArchiveFileName))
            {
                fs = new FileStream(ArchiveFileName, FileMode.Open, FileAccess.Read);
                if (fs.Length > 0)
                {
                    BinaryReader reader = new BinaryReader(fs);
                    VersionMajor = reader.ReadByte();
                    VersionMinor = reader.ReadByte();
                    if (VersionMajor == 1 && VersionMinor == 0) ReadVersion_1_0(reader);
                }
            }
            else
            {
                fs = new FileStream(ArchiveFileName, FileMode.Create, FileAccess.Write);
                fs.SetLength(ContentSection);
                BinaryWriter writer = new BinaryWriter(fs);
                // write version
                writer.Write(Version.Major);
                writer.Write(Version.Minor);
                // write count
                writer.Write(0);
            }
            end_of_headers = fs.Position;
            // Set the file begin byte address in archive
            long end_of_last = ContentSection;
            for (int i = 0; i < files.Length; i++)
            {
                files[i].Begin = end_of_last;
                end_of_last += files[i].Length;
            }
            // close the archive
            fs.Close();
            fs.Dispose();
        }

        private void ReadVersion_1_0(BinaryReader reader)
        {
            // read count
            int count = reader.ReadInt32();
            // read file infos
            files = new ArchiveFile[count];
            for (int i = 0; i < count; i++)
            {
                files[i] = new ArchiveFile();
                files[i].Name = reader.ReadString();
                files[i].Length = reader.ReadInt64();
            }
        }

        public void AddFile(string filename)
        {
            // open the file
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            AddFile(filename, fs);
        }

        public void AddFile(string filename, string newfilename)
        {
            // open the file
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            AddFile(newfilename, fs);
        }

        public void AddFile(string filename, Stream stream)
        {
            FileStream fs = new FileStream(ArchiveFileName, FileMode.Open, FileAccess.ReadWrite);
            BinaryReader reader = new BinaryReader(fs);
            BinaryWriter writer = new BinaryWriter(fs);

            // update files count
            fs.Seek(2, SeekOrigin.Begin); // jump over the version part
            writer.Write(files.Length + 1);
            // write filename
            fs.Seek(end_of_headers, SeekOrigin.Begin);
            writer.Write(Path.GetFileName(filename));
            // write file size
            byte[] file2_length = BitConverter.GetBytes(stream.Length);
            fs.Write(file2_length, 0, file2_length.Length);
            // goto end of archive
            end_of_headers = fs.Position;
            fs.Seek(0, SeekOrigin.End);
            // write the file
            byte[] copy_buffer;
            int ih = (int)(stream.Length / 10240);
            for (int i = 0; i <= ih; i++)
            {
                int remaining_bytes_length = (int)(stream.Length - stream.Position);
                if (remaining_bytes_length < 10240)
                    copy_buffer = new byte[remaining_bytes_length];
                else
                    copy_buffer = new byte[10240];
                stream.Read(copy_buffer, 0, copy_buffer.Length);
                fs.Write(copy_buffer, 0, copy_buffer.Length);
            }
            // close files
            stream.Close();
            stream.Dispose();
            fs.Close();
            fs.Dispose();

            Reload();
        }

        public void RemoveFile(string filename)
        {
            int file_index = GetFileIndex(filename);
            if (file_index >= 0)
            {
                FileStream fs = null;
                try
                {
                    fs = new FileStream(ArchiveFileName, FileMode.Open, FileAccess.ReadWrite);
                    BinaryReader reader = new BinaryReader(fs);
                    BinaryWriter writer = new BinaryWriter(fs);

                    // update files count
                    fs.Seek(2, SeekOrigin.Begin); // jump over the version part
                    writer.Write(files.Length - 1);

                    // remove filename & size
                    for (int i = 0; i < file_index; i++)
                    {
                        reader.ReadString();
                        reader.ReadInt64();
                    }
                    long filename_pos = fs.Position;
                    int filename_length = ((int)reader.ReadByte()) + 9;
                    fs.Seek(filename_length - 1, SeekOrigin.Current);
                    // move rest of headers to the beginning of removing filename
                    byte[] rest_of_headers = new byte[end_of_headers - fs.Position];
                    fs.Read(rest_of_headers, 0, rest_of_headers.Length);
                    fs.Seek(filename_pos, SeekOrigin.Begin);
                    fs.Write(rest_of_headers, 0, rest_of_headers.Length);
                    rest_of_headers = null;
                    int bytes_to_contentsection = (int)(ContentSection - fs.Position);
                    for (int i = 0; i < bytes_to_contentsection; i++)
                        writer.Write((byte)(0));

                    // remove file contents
                    long content_end = files[file_index].Begin + files[file_index].Length;
                    long last_copy_pos = content_end;
                    long last_paste_pos = files[file_index].Begin;
                    byte[] copy_buffer;
                    int ih = (int)((fs.Length - content_end) / 10240);
                    for (int i = 0; i <= ih; i++)
                    {
                        int remaining_bytes_length = (int)(fs.Length - last_copy_pos);
                        if (remaining_bytes_length < 10240)
                            copy_buffer = new byte[remaining_bytes_length];
                        else
                            copy_buffer = new byte[10240];
                        // copy
                        fs.Seek(last_copy_pos, SeekOrigin.Begin);
                        fs.Read(copy_buffer, 0, copy_buffer.Length);
                        last_copy_pos = fs.Position;
                        // paste
                        fs.Seek(last_paste_pos, SeekOrigin.Begin);
                        fs.Write(copy_buffer, 0, copy_buffer.Length);
                        last_paste_pos = fs.Position;
                    }

                    // reduce the file size
                    fs.SetLength(last_paste_pos);
                }
                catch (Exception ex) { }
                finally
                {
                    // close files
                    if (fs != null)
                    {
                        fs.Close();
                        fs.Dispose();
                    }
                }
            }

            Reload();
        }

        public void ExtractFile(string filename, string extractionpath)
        {
            byte[] file_contents = GetFileContents(filename);
            if (file_contents != null)
            {
                // write the file contents
                File.WriteAllBytes(extractionpath + filename, file_contents);
            }
        }

        public int GetFileIndex(string filename)
        {
            int file_index = -1;
            for (int i = 0; i < files.Length; i++)
                if (files[i].Name == filename)
                    file_index = i;
            return file_index;
        }

        public byte[] GetFileContents(string filename)
        {
            int file_index = GetFileIndex(filename);
            if (file_index >= 0)
            {
                FileStream fs = new FileStream(ArchiveFileName, FileMode.Open, FileAccess.Read);
                BinaryReader reader = new BinaryReader(fs);
                fs.Seek(files[file_index].Begin, SeekOrigin.Begin);

                // read the file
                byte[] file_contents = new byte[files[file_index].Length];
                reader.Read(file_contents, 0, file_contents.Length);
                fs.Close();
                fs.Dispose();

                return file_contents;
            }
            return null;
        }

        public Bitmap GetIcon(string filename, string width, string fixed_height)
        {
            string file_ext = Path.GetExtension(filename);
            Bitmap resbitmap = Resources.FileTypes.etc;
            string imagefileext = ".jpg|.jpeg|.bmp|.png|.gif|.tif|.tiff|";
            if (imagefileext.IndexOf(file_ext + "|") >= 0)
                resbitmap = GetImageThumbnail(filename, width, fixed_height);
            else if (file_ext == ".pdf")
                resbitmap = Resources.FileTypes.pdf;
            else if (file_ext == ".doc" || file_ext == ".docx")
                resbitmap = Resources.FileTypes.doc;
            else if (file_ext == ".xls" || file_ext == ".xlsx")
                resbitmap = Resources.FileTypes.xls;
            else if (file_ext == ".txt")
                resbitmap = Resources.FileTypes.txt;

            return resbitmap;
        }

        public Bitmap GetImageThumbnail(string filename, string width, string fixed_height)
        {
            MemoryStream memsource = null;
            Bitmap fullbitmap = null;
            Bitmap resultbitmap = null;
            Graphics new_g = null;
            try
            {
                // Create a Bitmap from the source image
                memsource = new MemoryStream(GetFileContents(filename));
                fullbitmap = new Bitmap(memsource);

                // Calculate the width and height
                int output_width = width != "" ? Convert.ToInt32(width) : 100;
                int output_height = 100;
                int imageWidth = output_width;
                float scale = (float)imageWidth / (float)fullbitmap.Width;
                int imageHeight = Convert.ToInt32(fullbitmap.Height * scale);
                if (fixed_height == "f") output_height = imageHeight;

                resultbitmap = new Bitmap(output_width, output_height, PixelFormat.Format32bppPArgb);
                new_g = Graphics.FromImage(resultbitmap);
                new_g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                new_g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                new_g.FillRectangle(Brushes.Transparent, -1, -1, output_width + 1, output_height + 1);
                if (fullbitmap.Width > imageWidth && fullbitmap.Height > imageHeight)
                    new_g.DrawImage(fullbitmap, -1, (output_height - imageHeight), imageWidth + 1, imageHeight + 1);
                else
                    new_g.DrawImage(fullbitmap, (imageWidth - fullbitmap.Width) / 2, (imageHeight - fullbitmap.Height) / 2, fullbitmap.Width, fullbitmap.Height);
            }
            catch { }
            finally
            {
                if (fullbitmap != null) fullbitmap.Dispose();
                if (new_g != null) new_g.Dispose();
                memsource.Close();
                memsource.Dispose();
            }
            return resultbitmap;
        }
    }

    public class ArchiveFile
    {
        public string Name = "";
        public long Length = 0;
        public long Begin = 0;
    }
}
