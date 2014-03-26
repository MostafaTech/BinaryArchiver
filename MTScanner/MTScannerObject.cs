using System;
using System.Runtime.InteropServices;

namespace MTScanner
{
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IMTScannerObject
    {
        [ComVisible(true)]
        [DispId(0x00001000)]
        void StartScan();
    }

    [ProgId("MTScannerObject")]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [Guid("8271095B-6A00-4742-AF68-BD50ABF17A46")]
    [ComVisible(true)]
    public class MTScannerObject : System.Windows.Forms.Control
    {
        Bytescout.Scan.Scan _scan;
        string _filename = "C:\\" + System.IO.Path.GetRandomFileName().Replace(".", "") + ".jpg";

        public MTScannerObject()
        {
            _scan = new Bytescout.Scan.Scan();
            _scan.TransferFinished += new Bytescout.Scan.TransferFinishedEventHandler(_scan_TransferFinished);
        }

        public void _scan_TransferFinished(Bytescout.Scan.Scan sender, System.Collections.ArrayList scannedImages)
        {
            if (scannedImages.Count > 0)
            {
            //    if (scannedImages[0] is Image)
            //    {
            //        img = (Image)scannedImages[0];
            //    }
            //    else
            //    {
            //        img = Image.FromFile((string)scannedImages[0]);
            //    }
            //    pictureBox1.Image = img;
            }
        }

        [ComVisible(true)]
        public void StartScan()
        {
            //_scan.SelectDevice(this.Handle);
            _scan.SaveTo = Bytescout.Scan.SaveTo.File;
            _scan.OutputFormat = Bytescout.Scan.OutputFormat.JPEG;
            _scan.OutputFolder = "C:\\";

            //GCHandle handle = GCHandle.Alloc(this, GCHandleType.Pinned);
            //IntPtr ptr = handle.AddrOfPinnedObject();
            _scan.AcquireImagesAsync(this.Handle);
        }
    }
}
