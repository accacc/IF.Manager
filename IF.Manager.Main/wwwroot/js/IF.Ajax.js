
$(document).on("click", "a[if-ajax=true]", function (e) {

    e.preventDefault();
    var ajaxOptions = GetAjaxOptions(this);
    ajaxOptions.url = this.href;
    IFAjax.Init(ajaxOptions);
    IFAjax.Send();

});


$(document).on("click", "button[if-ajax-form-submit=true]", function (e)
{
    e.preventDefault();
    debugger;
    var form = $(this).parents('form:first');
    var formData = form.serializeObject();

    //var formData = form.serialize();

    var ajaxOptions = GetAjaxOptions(this);
    ajaxOptions.url = $(this).attr("if-ajax-action");
    ajaxOptions.data = formData;
    ExtendData(ajaxOptions, this);
    IFAjax.Init(ajaxOptions);
    IFAjax.Send();
});

function GetAjaxOptions(element) {

    var ajaxOptions = {

        updateid: $(element).attr("if-ajax-update-id"),
        //dataType: $(element).attr("if-ajax-data-type"),
        modalid: $(element).attr("if-ajax-modal-id"),
        refreshGrid: $(element).attr("if-ajax-refresh-grid"),
        showDialog: $(element).attr("if-ajax-show-dialog"),
        insertionMode: $(element).attr("if-ajax-insertion-mode"),
        method: $(element).attr("if-ajax-method"),
        enctype: $(element).attr("if-ajax-enctype"),
        CloseModalAfterSuccess: $(element).attr("if-ajax-close-modal-on-success"),
        gridViewId: $(element).attr("if-ajax-gridview-id"),
        onErrorFunc: $(element).attr("if-ajax-onerror-func"),
        onSuccessFunc: $(element).attr("if-ajax-onsuccess-func"),
        onSuccessReload: $(element).attr("if-ajax-onsuccess-reload"),
        confirm: $(element).attr("if-ajax-confirm"),
        onBeforeFunc: $(element).attr("if-ajax-onbefore-func"),
        onCompleteFunc: $(element).attr("if-ajax-oncomplete-func"),
        onSuccessRefresh: $(element).attr("if-ajax-on-success-refresh"),
        onSuccessRefreshAction: $(element).attr("if-ajax-on-success-refresh-action"),
        onSuccessRefreshUpdateId: $(element).attr("if-ajax-on-success-refresh-updateid"),
        antiForgeryToken: $(element).attr("if-anti-forgery-token"),        
        data: {}
    };
    return ajaxOptions;
}

function ExtendData(ajaxOptions,element) {
    if ($(element).attr("if-ajax-extradatafunc")) {
        debugger;
        var extraData = eval($(element).attr("if-ajax-extradatafunc"));
        //ajaxOptions.dataType ="json";
        $.extend(ajaxOptions.data, extraData);
    }

    if ($(element).attr("if-ajax-data-extra-by-id")) {
        debugger;
        var id = $(element).attr("if-ajax-data-extra-by-id");
        var value = $("#" + id).val();
        ajaxOptions.data[id] = value;

    }

    if (ajaxOptions.antiForgeryToken !== undefined) {
        //alert(ajaxOptions.antiForgeryToken);
        ajaxOptions.data["__RequestVerificationToken"] = ajaxOptions.antiForgeryToken;

    }
}

var IFAjax = {
    ajaxOptions: {},
    IsBlockUIEnabled: true,
    Init: function (opts) {
        IFAjax.ajaxOptions = opts;
    },
    Send: function () {

        if (IFAjax.ajaxOptions.confirm && !window.confirm(IFAjax.ajaxOptions.confirm)) {
            return;
        }

        $.ajax({
            url: IFAjax.ajaxOptions.url,
            data: IFAjax.ajaxOptions.data,
            type: IFAjax.ajaxOptions.method || 'GET',
            //dataType: IFAjax.ajaxOptions.dataType || "html",

            cache: false,
            beforeSend: function (xhr) {
                if (!isUndefined(IFAjax.ajaxOptions.onBeforeFunc)) {
                    getFunction(IFAjax.ajaxOptions.onBeforeFunc, []).apply(this, []);
                }
            },
            success: function (data) {
                if (IFAjax.ajaxOptions.showDialog) {
                    IFAjax.ShowDialog(data);
                }
                else if (data !== "") {
                    IFAjax.UpdateTarget(data, IFAjax.ajaxOptions.updateid);
                }

                if(IFAjax.ajaxOptions.onSuccessReload === "true"){
                    window.location.reload();
                }

                if (IFAjax.ajaxOptions.CloseModalAfterSuccess === "true") {
                    IFAjax.closeDialog();
                }

                if (IFAjax.ajaxOptions.onSuccessRefresh === "true") {

                    $.ajax({
                        url: IFAjax.ajaxOptions.onSuccessRefreshAction,
                        data: {},
                        success: function (data) {
                            IFAjax.UpdateTarget(data, IFAjax.ajaxOptions.onSuccessRefreshUpdateId);
                        }
                    });
                }

                if (!isUndefined(IFAjax.ajaxOptions.onSuccessFunc)) {
                    getFunction(IFAjax.ajaxOptions.onSuccessFunc, []).apply(this, []);
                }

            },
            complete: function () {
                if (!isUndefined(IFAjax.ajaxOptions.onCompleteFunc)) {
                    getFunction(IFAjax.ajaxOptions.onCompleteFunc, []).apply(this, []);
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                //alert(xhr.status);
                //alert(thrownError);
                if (!isUndefined(IFAjax.ajaxOptions.onErrorFunc)) {
                    getFunction(IFAjax.ajaxOptions.onErrorFunc, []).apply(this, []);
                }
            }

        });
    },
    UpdateTarget: function (data,updateid)
    {

        var id = "#" + updateid;

        var mode = (IFAjax.ajaxOptions.insertionMode || "").toUpperCase();

        $(id).each(function (i, update) {

            switch (mode) {
                case "BEFORE":
                    $(update).prepend(data);
                    break;
                case "AFTER":
                    $(update).append(data);
                    break;
                case "REPLACE-WITH":
                    $(update).replaceWith(data);
                    break;
                default:
                    $(update).html(data);
                    break;
            }
        });

    },
    ShowDialog: function (data)
    {
        var modalIdSelector = "#" + this.ajaxOptions.modalid;

        if ($(modalIdSelector).length === 0) {
            $("<div></div>")
                .attr('role', 'dialog')
                .attr('class', 'modal fade modal-fullscreen')
                .attr('id', this.ajaxOptions.modalid)
                .appendTo('body');
        }

        data = data.replace('if-ajax-close-modal-on-success=\"true\"', 'if-ajax-close-modal-on-success="true" ' + 'if-ajax-modal-id="' + this.ajaxOptions.modalid + '"');

        $(modalIdSelector).html("");

        $(modalIdSelector).html(data);

        $(modalIdSelector).on('hidden.bs.modal', function () {

            $(this).remove();

            if (IFAjax.ajaxOptions.refreshGrid) {
                IFAjax.refreshGrid();
            }

        });

        $(modalIdSelector).on('hidden.bs.modal', function (event) {
            if ($('.modal:visible').length) {
                $('body').addClass('modal-open');
            }
        });

        $(modalIdSelector).modal('show');
    },
    closeDialog: function () {
        var idSelector = "#" + this.ajaxOptions.modalid;
        $(idSelector).modal('hide');
    },
    blockUI: function () {
        if (this.IsBlockUIEnabled) {
            $.blockUI({
                baseZ: 100003,
                message: '<div class="fancybox-loading" />',
                css: { backgroundColor: 'rgba(255,255,255,-0.5)', border: "none", left: 0, right: 0, margin: "auto" }
            });
        }
    },
    unblockUI: function () {
        if (this.IsBlockUIEnabled) {
            $.unblockUI();
        }
    },
    blockUIDisable: function () {
        this.IsBlockUIEnabled = false;
    },
    blockUIEnable: function () {
        this.IsBlockUIEnabled = true;
    }
}

$(document).ajaxStart(function () {
    $.blockUI({
        baseZ: 100003,
        message: '<div class="fancybox-loading" />',
        css: {backgroundColor:'rgba(255,255,255,-0.5)',border:"none",left:0,right:0,margin:"auto"}
    })
}).ajaxStop(function () {
    $.unblockUI();
});

$.ajaxSetup({ cache: false, timeout: 2000000 });


$.fn.serializeObject = function () {
    var o = {};

    $(this).find('input[type="hidden"], input[type="text"], input[type="password"], input[type="checkbox"]:checked, input[type="radio"]:checked, select').each(function () {
        if ($(this).attr('type') == 'hidden') { //if checkbox is checked do not take the hidden field
            var $parent = $(this).parent();
            var $chb = $parent.find('input[type="checkbox"][name="' + this.name.replace(/\[/g, '\[').replace(/\]/g, '\]') + '"]');
            if ($chb != null) {
                if ($chb.prop('checked')) return;
            }
        }
        if (this.name === null || this.name === undefined || this.name === '')
            return;
        var elemValue = null;
        if ($(this).is('select'))
            elemValue = $(this).find('option:selected').val();
        else elemValue = this.value;
        if (o[this.name] !== undefined) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(elemValue || '');
        } else {
            o[this.name] = elemValue || '';
        }
    });
    return o;
}