using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace MTScanner
{
    [ProgId("MTScannerUI")]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [Guid("DCB8EBBA-DF06-4FFE-9F2D-2119C9034677")]
    [ComVisible(true)]
    public partial class MTScannerUI : UserControl
    {
        Bytescout.Scan.Scan _scan;
        string _filename = "C:\\" + System.IO.Path.GetRandomFileName().Replace(".", "") + ".jpg";

        public MTScannerUI()
        {
            InitializeComponent();
            _scan = new Bytescout.Scan.Scan();
            _scan.TransferFinished += new Bytescout.Scan.TransferFinishedEventHandler(_scan_TransferFinished);
        }

        public void btnScan_Click(object sender, EventArgs e)
        {
            StartScan();
        }

        public void _scan_TransferFinished(Bytescout.Scan.Scan sender, System.Collections.ArrayList scannedImages)
        {
            btnScan.Text = "Scanned";
            //if (scannedImages.Count > 0)
            //{
            //    Image img;
            //    if (scannedImages[0] is Image)
            //    {
            //        img = (Image)scannedImages[0];
            //    }
            //    else
            //    {
            //        img = Image.FromFile((string)scannedImages[0]);
            //    }
            //    pictureBox1.Image = img;
            //}
        }

        [ComVisible(true)]
        public void StartScan()
        {
            //_scan.SelectDevice(this.Handle);
            _scan.SaveTo = Bytescout.Scan.SaveTo.ImageObject;
            _scan.OutputFormat = Bytescout.Scan.OutputFormat.JPEG;
            //_scan.OutputFolder = "C:\\";
            _scan.AcquireImagesAsync(this.Handle);
        }

        public void btnSend_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(_filename))
            {
                byte[] contents = System.IO.File.ReadAllBytes(_filename);
                System.Net.WebClient wc = new System.Net.WebClient();
                wc.UploadData("http://localhost:1153/WebTest/bytescout/Default.aspx", contents);
            }
        }
    }
}
