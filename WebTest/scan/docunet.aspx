<%@ Page Language="C#" AutoEventWireup="true" CodeFile="docunet.aspx.cs" Inherits="scan_docunet" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
<form id="form1" runat="server">

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
//    (function () {
//        // Detecting the OS
//        if (navigator.appVersion.indexOf("Win") != -1) Archiver_Env.OSName = "Windows";
//        if (navigator.appVersion.indexOf("Mac") != -1) Archiver_Env.OSName = "Mac";
//        if (navigator.appVersion.indexOf("X11") != -1) Archiver_Env.OSName = "UNIX";
//        if (navigator.appVersion.indexOf("Linux") != -1) Archiver_Env.OSName = "Linux";
//        // Detecting the Browser
//        (function () {
//            var nVer = navigator.appVersion;
//            var nAgt = navigator.userAgent;
//            var browserName = navigator.appName;
//            var fullVersion = '' + parseFloat(navigator.appVersion);
//            var majorVersion = parseInt(navigator.appVersion, 10);
//            var nameOffset, verOffset, ix;

//            // In Opera, the true version is after "Opera" or after "Version"
//            if ((verOffset = nAgt.indexOf("Opera")) != -1) {
//                browserName = "Opera";
//                fullVersion = nAgt.substring(verOffset + 6);
//                if ((verOffset = nAgt.indexOf("Version")) != -1)
//                    fullVersion = nAgt.substring(verOffset + 8);
//            }
//            // In MSIE, the true version is after "MSIE" in userAgent
//            else if ((verOffset = nAgt.indexOf("MSIE")) != -1) {
//                browserName = "Microsoft Internet Explorer";
//                fullVersion = nAgt.substring(verOffset + 5);
//            }
//            // In Chrome, the true version is after "Chrome" 
//            else if ((verOffset = nAgt.indexOf("Chrome")) != -1) {
//                browserName = "Chrome";
//                fullVersion = nAgt.substring(verOffset + 7);
//            }
//            // In Safari, the true version is after "Safari" or after "Version" 
//            else if ((verOffset = nAgt.indexOf("Safari")) != -1) {
//                browserName = "Safari";
//                fullVersion = nAgt.substring(verOffset + 7);
//                if ((verOffset = nAgt.indexOf("Version")) != -1)
//                    fullVersion = nAgt.substring(verOffset + 8);
//            }
//            // In Firefox, the true version is after "Firefox" 
//            else if ((verOffset = nAgt.indexOf("Firefox")) != -1) {
//                browserName = "Firefox";
//                fullVersion = nAgt.substring(verOffset + 8);
//            }
//            // In most other browsers, "name/version" is at the end of userAgent 
//            else if ((nameOffset = nAgt.lastIndexOf(' ') + 1) <
//          (verOffset = nAgt.lastIndexOf('/'))) {
//                browserName = nAgt.substring(nameOffset, verOffset);
//                fullVersion = nAgt.substring(verOffset + 1);
//                if (browserName.toLowerCase() == browserName.toUpperCase()) {
//                    browserName = navigator.appName;
//                }
//            }
//            // trim the fullVersion string at semicolon/space if present
//            if ((ix = fullVersion.indexOf(";")) != -1)
//                fullVersion = fullVersion.substring(0, ix);
//            if ((ix = fullVersion.indexOf(" ")) != -1)
//                fullVersion = fullVersion.substring(0, ix);

//            majorVersion = parseInt('' + fullVersion, 10);
//            if (isNaN(majorVersion)) {
//                fullVersion = '' + parseFloat(navigator.appVersion);
//                majorVersion = parseInt(navigator.appVersion, 10);
//            }
//            // set in Env
//            Archiver_Env.BrowserName = browserName;
//            Archiver_Env.BrowserVersion = majorVersion;
//        })();
//    })();
</script>

<div id="scanner_container"></div>
<input type="button" value="start scan" onclick="scanner_scan();" />

<script type="text/javascript">
    function Archiver_Scanner_Init() {
        var _container = document.getElementById('scanner_container');
        var _control = '';
        if (Archiver_Env.isIE()) {
            _control += "<object id='scanner_obj' style='width:0px;height:0px'";
            _control += "codebase='docuNetClientTools.CAB#version=2.0.0.0' ";
            _control += " classid='clsid:2A073362-91C9-4EBB-93F9-070C4C1E0ACF' viewastext>";
            _control += " </object>";
        } else {
            _control += "<embed id='scanner_obj' style='display: inline; width:0px;height:0px' type='application/DocuNetClientTools.cImageProcessing' ";
            _control += "pluginspage='docuNetClientTools.CAB#version=2.0.0.0'></embed>";
        }
//        _control += "<object id='scanner_obj' style='width:0px;height:0px'";
//        _control += "codebase='docuNetClientTools.CAB#version=2.0.0.0' ";
//        _control += " classid='clsid:2A073362-91C9-4EBB-93F9-070C4C1E0ACF' viewastext>";
//        _control += "<embed type='application/DocuNetClientTools.cImageProcessing' ";
//        _control += "pluginspage=''></embed>";
//        _control += " </object>";
        _container.innerHTML = _control;
    }
    function Archiver_Scanner_Start() {
        var scanner_obj = document.getElementById('scanner_obj');
        try {
            scanner_obj.HTTP.HostName = Archiver_Env.getServerAddress();
            scanner_obj.HTTP.Port = 1153; // Archiver_Env.getServerPort();
            scanner_obj.HTTP.Path = '/webtest/scan/upload.ashx';
            scanner_obj.ScanImageType = "jpg";
            scanner_obj.HTTP.SID = 'sid';
            scanner_obj.JPEGQuality = 75;
            var res = scanner_obj.scanUpload(true);
            //if (res != null)
            {
                if (res == "count{(0)}")
                    throw "دستگاه اسکنر به رایانه متصل نمیباشد";
            }
            if (scanner_obj.Error.Number != 0)
                throw scanner_obj.Error.Number + "\n" + scanner_obj.Error.Description;
        }
        catch (err) {
            //alert('Error:\n' + err.Data);
            throw err;
        }
    }
    //Archiver_Scanner_Init()();
</script>
</form>
</body>
</html>
