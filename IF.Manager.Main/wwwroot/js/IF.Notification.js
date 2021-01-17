function isdefined(variable) {
    return (typeof (window[variable]) === "undefined") ? false : true;
}

$(document).ready(function () {

    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "rtl": false,
        "positionClass": "toast-top-center-screen",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": 300,
        "hideDuration": 1000,
        "timeOut": 0,
        "extendedTimeOut": 0,
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }

    handleAjaxMessages();

    if (isdefined('messageContext')) {
        displayMessage(messageContext.Context, messageContext.Type);
    }

});

function handleAjaxMessages() {
    $(document).ajaxSuccess(function (event, request) {
        checkAndHandleMessageFromHeader(request);
    }).ajaxError(function (event, request) {
        toastr.error("Error");
        debugger;
        if (request.responseText !== undefined && request.responseText !== '') {
            try {
                messageContext = $.parseJSON(request.responseText);
                displayMessage(messageContext, "error");
            } catch (e) {
                ShowMessage(request.responseText, "error");
                alert(request.responseText);
            }

           
        }

    });
}

function checkAndHandleMessageFromHeader(request) {
    var messagesRaw = request.getResponseHeader('X-Message');
    if (messagesRaw) {
        var messageContext = $.parseJSON(messagesRaw);
        var messageType = request.getResponseHeader('X-Message-Type');
        displayMessage(messageContext, messageType);
    }
}

function decode_utf8(s) {

    return decodeURIComponent(escape(s));

}


function unicodeToChar(text) {
    return text.replace(/\\u[\dA-F]{4}/gi,
        function (match) {
            return String.fromCharCode(parseInt(match.replace(/\\u/g, ''), 16));
        });
}

function displayMessage(context, messageType) {

    $.each(context.Messages, function (i, message) {

        ShowMessage(message, messageType);

    });
}

function ShowMessage(message,messageType) {
    message = unicodeToChar(message);
    messageType = messageType.toLowerCase();

    if (messageType === "success") {
        title = "Success";
        toastr.success(message);

    } else if (messageType === "warning") {
        title = "Warning";
        toastr.warning(message);
    } else if (messageType === "error") {
        title = "Error";
        toastr.error(message);
    } else {
        title = "Error";
        toastr.error(message);
    }
}

function isJson(str) {
    try {
        JSON.parse(str);
    } catch (e) {
        return false;
    }
    return true;
}