<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default2.aspx.cs" Inherits="myscanneractivex_Default2" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
</head>
<body>
<form id="form1" runat="server">

<script src="../scripts/jquery.js" type="text/javascript"></script>
<link href="../scripts/bootstrap/bootstrap.css" rel="stylesheet" type="text/css" />
<link href="../scripts/colorbox/colorbox.css" rel="stylesheet" type="text/css" />
<script src="../scripts/colorbox/jquery.colorbox-min.js" type="text/javascript"></script>
<script src="../scripts/colorbox/jquery.colorbox-fa.js" type="text/javascript"></script>
<script src="ScannerActiveX.js" type="text/javascript"></script>
<script type="text/javascript">

    function OpenScannerDialog() {
        $.colorbox({ inline: true, href: '#scan_dialog', width: '700px', height: '80%' });
        Archiver.Scanner.listDevices = 'lstDevices';
//        Archiver.Scanner.OnError = function (errorID, errorText) {
//            alert(errorID + "\n" + errorText);
//        };
        Archiver.Scanner.init('scanner_container');
    }
    
</script>

<input type="button" class="btn btn-primary" value="open scanner" onclick="OpenScannerDialog()" />

<div style="display:none;">
    <div id="scan_dialog">
        <div style="width:410px; float:left; display:inline-block;">
            <div id="scanner_container"></div>
        </div>
        <div style="width:220px; float:left; display:inline-block;">
            <div style="border:1px solid #ccc; padding:5px;">
                فهرست اسکنر ها:<br />
                <select id="lstDevices">
                    <option value="0">no devices</option>
                </select>
            </div>
            <input type="button" value="اسکن تصویر" 
                class="btn btn-primary btn-large" style="width:100%;" 
                onclick="Archiver.Scanner.Scan();" />
            <br />
            <input type="button" value="ارسال تصویر" 
                class="btn btn-primary btn-large" style="width:100%;" 
                onclick="Archiver.Scanner.Upload('localhost','1153','/WebTest/scan/upload.ashx','cookie');" />
            <br />
            <input type="button" value="انتخاب" 
                class="btn btn-primary btn-large" style="width:100%;" 
                onclick="Archiver.Scanner.SelectDevice();" />
            <br />
            <input type="button" value="پرخش به راست" 
                class="btn btn-primary btn-large" style="width:100%;" 
                onclick="Archiver.Scanner.Rotate(90);" />
            <br />
            <input type="button" value="پرخش به چپ" 
                class="btn btn-primary btn-large" style="width:100%;" 
                onclick="Archiver.Scanner.Rotate(270);" />
        </div>
    </div>
</div>

</form>
</body>
</html>
