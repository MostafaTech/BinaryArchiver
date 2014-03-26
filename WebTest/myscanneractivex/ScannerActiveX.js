/* 
 * ScannerActiveX, by Mostafa Rowghanian
 */

var Archiver = {

    // Environment
    Env: {
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
    },

    Scanner: {

        // Events
        OnError: function (errorID, errorText, jserr) {
            console.log(jserr);
        },

        // Variables
        __object: null,
        listDevices: null,

        // 
        __AttachEvents_IE: function () {
            this.__object.attachEvent('OnError', this.OnError);
        },
        __AttachEvents_NonIE: function () {
            var output = '';
            output += "event_OnError=\"Archiver.Scanner.OnError\" ";
            return output;
        },
        __LoadingDevices: function () {
            if (!this.listDevices) return;
            var _jsonDevices = this.GetDevices();
            if (_jsonDevices == '') return;
            var _data = eval('(' + _jsonDevices + ')');
            var _listDevices = document.getElementById(this.listDevices);
            _listDevices.options.length = 0;
            for (_key in _data) {
                var _opt = document.createElement('option');
                _opt.value = _key;
                _opt.innerHTML = _data[_key];
                _listDevices.appendChild(_opt);
            }
        },

        // Init the object
        init: function (_container) {
            var _clsid = "A2DFD591-7731-4C44-A891-D2A89C0397AF";
            var _codebase = "ScannerActiveX.CAB#version=1.0.0.5";
            var _progid = "ScannerActiveX.ScannerUI";
            var _control = "";
            _container = document.getElementById(_container);
            if (Archiver.Env.isIE) {
                // LPK file
                _control +=
                    "<OBJECT CLASSID=\"clsid:5220cb21-c88d-11cf-b347-00aa00a28331\">" +
                    "<PARAM NAME=\"LPKPath\" VALUE=\"ScannerActiveX.lpk\">" +
                    "</OBJECT>\n";
                // 
                _control +=
                    "<object id='objScanner' width='20px' height='20px' " +
                    "classid='clsid:" + _clsid + "' codebase='" + _codebase + "'></object>";
            } else {
                // load the activex-host
                _control +=
                    "<object id='objScanner' width='20px' height='20px' " +
                    "type='application/x-itst-activex' " +
                    "clsid='{" + _clsid + "}' progid='" + _progid + "' codeBaseURL='" + _codebase + "'" +
                    this.__AttachEvents_NonIE() +
                    "></object>";
            }
            _container.innerHTML = _control;
            window.setTimeout(function () {
                Archiver.Scanner.__object = document.getElementById('objScanner');
                if (Archiver.Env.isIE) Archiver.Scanner.__AttachEvents_IE();
                Archiver.Scanner.__LoadingDevices();
            }, 500);
        },
        GetDevices: function () {
            try {
                return this.__object.GetDevices();
            }
            catch (err) {
                this.OnError(err.ErrorNumber, err.Message, err);
                return "";
            }
        },
        GetSelectedDeviceID: function () {
            var _listDevices = document.getElementById(this.listDevices);
            return _listDevices.options[0].value;
        },
        ScanAndUpload: function (DeviceID, HostName, HostPort, UploadPath, Cookie) {
            try {
                this.__object.ScanAndUpload(DeviceID, HostName, HostPort, UploadPath, Cookie);
            }
            catch (err) {
                this.OnError(err.ErrorNumber, err.Message, err);
            }
        },
        Scan: function () {
            try {
                console.log('Scan hit');
                var DeviceID = '';
                this.__object.Scan(DeviceID);
            }
            catch (err) {
                this.OnError(err.ErrorNumber, err.Message, err);
            }
        },
        Upload: function (HostName, HostPort, UploadPath, Cookie) {
            try {
                this.__object.Upload(HostName, HostPort, UploadPath, Cookie);
            }
            catch (err) {
                this.OnError(err.ErrorNumber, err.Message, err);
            }
        },
        Rotate: function (angle) {
            try {
                this.__object.RotateImage(angle);
            }
            catch (err) {
                this.OnError(err.ErrorNumber, err.Message);
            }
        },
        LoadClientImage: function (_clientPath) {
            this.__object.LoadImage(_clientPath);
        },
        SelectDevice: function () {
            this.__object.SelectDevice('');
        }
    }
};
