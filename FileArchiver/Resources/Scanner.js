
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

function Archiver_Scanner_Init() {
    var _container = document.getElementById('scanner_container');
    var _control = '';
    if (Archiver_Env.isIE) {
        _control += "<object id='scanner_obj' style='width:0px;height:0px'";
        _control += "codebase='docuNetClientTools.CAB#version=2.0.0.0' ";
        _control += " classid='clsid:2A073362-91C9-4EBB-93F9-070C4C1E0ACF' viewastext>";
        _control += " </object>";
    } else {
        _control += "<embed id='scanner_obj' style='display: inline; width:0px;height:0px' type='application/DocuNetClientTools.cImageProcessing' ";
        _control += "pluginspage='docuNetClientTools.CAB#version=2.0.0.0'></embed>";
    }
    _container.innerHTML = _control;
}

function Archiver_Scanner_Start(options) {
    var scanner_obj = document.getElementById('scanner_obj');
    try {
        scanner_obj.HTTP.HostName = options.HostName; //Archiver_Env.getServerAddress();
        scanner_obj.HTTP.Port = options.HostPort; //Archiver_Env.getServerPort();
        scanner_obj.HTTP.Path = options.Path; //'/webtest/scan/upload.ashx';
        scanner_obj.ScanImageType = "jpg";
        scanner_obj.HTTP.SID = options.SID;
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
        alert('Error:\n' + err);
    }
}

// init the scanner

//function ScanPage(action, sid)
//{
//    var success = true;
//    try
//    {
//        var oImageProc = new ActiveXObject("DocuNetClientTools.cImageProcessing");
//        //oImageProc = document.getElementById("oImageProc");
//        if ( oImageProc == null )
//        {
//            throw "لطفا پکیج پیش نیاز را نصب نمایید.";
//        }
//        oImageProc.HTTP.HostName = GetServerAddress();
//        oImageProc.HTTP.Port = GetServerPort();
//        oImageProc.HTTP.Path = action;
//        oImageProc.ScanImageType = "jpg";
//        oImageProc.HTTP.SID = sid;
//        oImageProc.JPEGQuality = 75;
//        var res = oImageProc.scanUpload(true);
//        //if (res != null)
//        {
//            if (res == "count{(0)}")
//                throw "دستگاه اسکنر به رایانه متصل نمیباشد";
//        }
//        if ( oImageProc.Error.Number != 0 ) throw oImageProc.Error.Number + "\n" + oImageProc.Error.Description;
//    }
//    catch(err)
//    {
//        if (!err.message)
//            alert(err);
//        else
//            alert(err.message);
//        success = false;
//    }
//    finally
//    {
//        oImageProc = null;
//        if (success)
//            return true;
//        else
//            return false;
//    }
//}

//function GetServerAddress()
//{
//	return location.hostname;
//}

//function GetServerPort()
//{
//	var serverPort = location.port;
//	if (serverPort == "")
//		serverPort = 80;
//	else
//		serverPort = parseInt(serverPort, 10);
//	return serverPort;
//}
