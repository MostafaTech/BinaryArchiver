
function MT_CreateUploader(id, action, callback_success, callback_error) {
    var __is_ie = function () { return navigator.userAgent.indexOf('MSIE') != -1; }
    var __is_firefox = function () { return navigator.userAgent.indexOf('Firefox') != -1; }
    var __container, __iframe, __form, __file, __label, __spinner;

    // container element
    __container = document.getElementById("mtuploader_container");
    if (!__container) {
        __container = document.createElement("div");
        __container.id = "mtuploader_container";
        __container.setAttribute("style", "display:none;");
        document.body.appendChild(__container);
    }
    
    // iframe
    __iframe = document.getElementById("mtuploader_iframe");
    if (!__iframe) {
        __iframe = document.createElement("iframe");
        __iframe.id = __iframe.name = "mtuploader_iframe";
        __iframe.setAttribute("src", "javascript:false");
        __iframe.onload = function () {
            __spinner.style.display = "none";
            var doc = __iframe.contentDocument ? __iframe.contentDocument : __iframe.contentWindow.document;
            var doc_json = eval('(' + doc.body.innerText + ')');
            if (doc_json.status == "success")
                if (callback_success) callback_success();
                else
                    if (callback_error) callback_error(doc_json.msg);
        };
        __container.appendChild(__iframe);
    }

    // form
    __form = document.getElementById("mtuploader_form");
    if (!__form) {
        __form = document.createElement("form");
        __form.id = __form.name = "mtuploader_form";
        __form.setAttribute("method", "post");
        __form.setAttribute("enctype", "multipart/form-data");
        __form.setAttribute("target", "mtuploader_iframe");
        __container.appendChild(__form);
    }

    // unique number
    var __number = mtuploader_MakeUniqueNumber();

    // file
    __file = document.createElement("input");
    __file.setAttribute("type", "file");
    __file.id = __file.name = "mtuploader_file_" + __number;
    __file.setAttribute("action", action);
    __file.onchange = function () {
        __form.setAttribute("action", this.getAttribute("action"));
        __form.submit();
        __spinner.style.display = "";
    };
    __form.appendChild(__file);

    // label
    __label = document.getElementById(id);
    if (__is_ie())
        __label.setAttribute("for", __file.name);
    else
        __label.onclick = function () { __file.click(); };

    // spinner
    __spinner = document.createElement("span");
    __spinner.id = "mtuploader_spinner_" + __number;
    __label.parentNode.insertBefore(__spinner, __label.nextSibling); // insertAfter!
}

function mtuploader_MakeUniqueNumber() {
    var __form = document.getElementById("mtuploader_form");
    var files_count = 0;
    for (i = 0; i < __form.childNodes.length; i++)
        if (__form.childNodes[i].tagName == "INPUT" &&
            __form.childNodes[i].getAttribute("type") == "file")
            files_count++;
    return String(files_count);
}
