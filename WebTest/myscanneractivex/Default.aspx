<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="myscanneractivex_Default" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title></title>
</head>
<body>
<form id="form1" runat="server">

<script src="../scripts/jquery.js" type="text/javascript"></script>
<link href="../scripts/bootstrap/bootstrap.css" rel="stylesheet" type="text/css" />
<link href="../scripts/colorbox/colorbox.css" rel="stylesheet" type="text/css" />
<script src="../scripts/colorbox/jquery.colorbox-min.js" type="text/javascript"></script>
<script src="../scripts/colorbox/jquery.colorbox-fa.js" type="text/javascript"></script>
<link href="../scripts/valums/fileuploader.css" rel="stylesheet" type="text/css" />
<script src="../scripts/valums/fileuploader.js" type="text/javascript"></script>

<script type="text/javascript">
    var Archiver_Env = {
        getOSName: function () {
            if (navigator.appVersion.indexOf("Win") != -1) return "Windows";
            if (navigator.appVersion.indexOf("Mac") != -1) return "Mac";
            if (navigator.appVersion.indexOf("X11") != -1) return "UNIX";
            if (navigator.appVersion.indexOf("Linux") != -1) return "Linux";
            return "Unknown OS";
        },
        isFirefox: (navigator.userAgent.toLowerCase().indexOf("gecko") != -1),
        isOpera: (navigator.userAgent.toLowerCase().indexOf("opera") != -1),
        isIE: (!this.isOpera && navigator.userAgent.toLowerCase().indexOf("msie") != -1),
        isChrome: (navigator.userAgent.toLowerCase().indexOf("chrome") != -1),
        getServerAddress: function () {
            return location.hostname;
        },
        getServerPort: function () {
            var serverPort = location.port;
            if (serverPort == "")
                serverPort = 80;
            else
                serverPort = parseInt(serverPort, 10);
            return serverPort;
        }
    };

    function Archiver_Scanner_Init(listDevices) {
        var _clsid = "47B9D63E-1179-4971-9EC3-65E0F9F387A9";
        var _codebase = "ScannerActiveX.CAB#version=1.0.0.5";
        var _progid = "ScannerActiveX.ScannerUI";
        var _container = document.getElementById('scanner_container');
        var _control = "<object id='scanner_obj' width='400px' height='300px' ";
        if (Archiver_Env.isIE) {
            _control += "classid='clsid:" + _clsid + "' codebase='" + _codebase + "'>";
            _control += "</object>";
            _container.innerHTML = _control;
            //window.setTimeout(null, 500);
            //Archiver_Scanner_AttachEvents_IE();
        } else {
            _control += "type='application/x-itst-activex' ";
            _control += "clsid='{" + _clsid + "}' progid='" + _progid + "' codeBaseURL='" + _codebase + "'></object>";
            _container.innerHTML = _control;
        }
        // Loading Scanner Devices
        Archiver_Scanner_LoadingDevices(listDevices);
    }

    function Archiver_Scanner_AttachEvents_IE() {
        var scanner_obj = document.getElementById('scanner_obj');
        scanner_obj.attachEvent('OnError', Archiver_Scanner_OnError);
    }

    function Archiver_Scanner_AttachEvents_FF() {
        var output = '';
        output += "event_OnError=\"Archiver_Scanner_OnDevicesLoaded\" ";
        return output;
    }

    function Archiver_Scanner_LoadingDevices(listDevices) {
        if (listDevices) {
            var scanner_obj = document.getElementById('scanner_obj');
            var jsonDevices = scanner_obj.GetDevices();
            var data = eval('(' + jsonDevices + ')');
            listDevices = document.getElementById(listDevices);
            listDevices.options.length = 0;
            for (key in data) {
                var opt = document.createElement('option');
                opt.value = key;
                opt.innerHTML = data[key];
                listDevices.appendChild(opt);
            }
        }
    }
    function Archiver_Scanner_OnError(errorID, errorText) {
        alert(errorID + '\n' + errorText);
    }

    function Archiver_Scanner_Start(HostName,HostPort,UploadPath,Cookie) {
        var scanner_obj = document.getElementById('scanner_obj');
        try {
            scanner_obj.ScanAndUpload(HostName, HostPort, UploadPath, Cookie, 'jpg');
            //if (res != null)
//            {
//                if (res == "count{(0)}")
//                    throw "دستگاه اسکنر به رایانه متصل نمیباشد";
//            }
//            if (scanner_obj.Error.Number != 0)
//                throw scanner_obj.Error.Number + "\n" + scanner_obj.Error.Description;
        }
        catch (err) {
            alert('Error:\n' + err);
        }
    }

    var jui = { hostName: 'localhost', hostPort: '1153', 
                           path: '/webtest/scan/upload.ashx', cookie: 'cookie' };
    function Archiver_Scanner_Scan() {
        var scanner_obj = document.getElementById('scanner_obj');
        try {
            scanner_obj.Scan('jpg');
        }
        catch (err) {
            alert('Error:\n' + err);
        }
    }
    function Archiver_Scanner_Upload() {
        var scanner_obj = document.getElementById('scanner_obj');
        try {
            scanner_obj.Upload(jui.hostName, jui.hostPort, jui.path, jui.cookie);
        }
        catch (err) {
            alert('Error:\n' + err);
        }
    }
    function Archiver_Scanner_Rotate(angle) {
        var scanner_obj = document.getElementById('scanner_obj');
        try {
            scanner_obj.RotateImage(angle);
        }
        catch (err) {
            alert('Error:\n' + err);
        }
    }
    function OpenScannerDialog() {
        $.colorbox({ inline: true, href: '#scan_dialog', width: '700px', height: '80%' });
        Archiver_Scanner_Init('lstDevices');
    }
    function LoadTestImage() {
        var scanner_obj = document.getElementById('scanner_obj');
        scanner_obj.LoadImage('c:\\12.jpg');
    }
</script>

<input type="button" value="اسکن اسناد" class="btn btn-primary" onclick="OpenScannerDialog()" />
<a href="javascript:;" class="btn btn-primary" onclick="$.colorbox({href:'http://www.google.com/images/srpr/logo11w.png'});">google</a>

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
                onclick="Archiver_Scanner_Scan();" />
            <br />
            <input type="button" value="ارسال تصویر" 
                class="btn btn-primary btn-large" style="width:100%;" 
                onclick="Archiver_Scanner_Upload();" />
            <br />
            <input type="button" value="لود تصویر آزمایشی" 
                class="btn btn-primary btn-large" style="width:100%;" 
                onclick="LoadTestImage();" />
            <br />
            <input type="button" value="پرخش به راست" 
                class="btn btn-primary btn-large" style="width:100%;" 
                onclick="Archiver_Scanner_Rotate(90);" />
            <br />
            <input type="button" value="پرخش به چپ" 
                class="btn btn-primary btn-large" style="width:100%;" 
                onclick="Archiver_Scanner_Rotate(270);" />
        </div>
    </div>
</div>

<script type="text/javascript">

</script>

</form>
</body>
</html>
