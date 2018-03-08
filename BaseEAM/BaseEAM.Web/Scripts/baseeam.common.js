//Use this for Add New button in the outer (main) form of the page
function addNewRecord(actionUrl, redirectUrl) {
    $.ajax({
        cache: false,
        type: "POST",
        url: actionUrl,
        data: addAntiForgeryToken(),
        success: function (data) {
            setLocation(redirectUrl + '/' + data.Id);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            showBSModal({ title: "Error", body: thrownError });
        },
        traditional: true
    });
};

//Use this for Save button in the outer (main) form of the page
function saveForm(formId, actionUrl, callback) {
    var $form = $("#" + formId);

    // Find disabled inputs, and remove the "disabled" attribute
    var disabled = $form.find(':input:disabled').removeAttr('disabled');
    // serialize the form
    var postData = $form.serializeJSON({ checkboxUncheckedValue: "false" });
    // re-disabled the set of inputs that you previously enabled
    disabled.attr('disabled', 'disabled');

    addAntiForgeryToken(postData);
    $.ajax({
        cache: false,
        type: "POST",
        url: actionUrl,
        data: postData,
        success: function (data) {
            if (data && data.Errors) {
                showErrors(data.Errors);
            } else {
                refreshDirtyTracking();

                if (callback && data) {
                    callback(data);
                } else if (callback) {
                    callback(true);
                }
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            showBSModal({ title: "Error", body: thrownError });
        },
        traditional: true
    });
};

//If selector = null or undefined
//then use the css selector
function showErrors(errors, selector) {
    var container;
    if (selector == null || selector == undefined) {
        container = $("#validation-summary");
    } else {
        container = $(selector);
    }

    if ((typeof errors) == 'string') {
        //single error
        //display the message
        container.append(errors);
        container.show();
    } else {
        //array of errors
        var message = '<ul>';
        //create a message containing all errors.
        $.each(errors, function (key, value) {
            if (value.errors) {
                $.each(value.errors, function (index, error) {
                    message += '<li>';
                    message += error;
                    message += '</li>';
                });
            }
        });
        message += '</ul>';

        //display the message
        container.append(message);
        container.show();
    }
}

//clear all errors in the page
function clearAllErrors() {
    var items = $(".validation-summary-errors");
    items.each(function (i, el) {
        el = $(el);
        el.html('');
        el.hide();
    });
};

function setLocation(url) {
    window.location.href = url;
}

function OpenWindow(query, w, h, scroll) {
    var l = (screen.width - w) / 2;
    var t = (screen.height - h) / 2;

    winprops = 'resizable=1, height=' + h + ',width=' + w + ',top=' + t + ',left=' + l + 'w';
    if (scroll) winprops += ',scrollbars=1';
    var f = window.open(query, "_blank", winprops);
}

function baseSplit(s) {
    if (s == '') {
        return [];
    } else {
        return s.split(';');
    }
}

function showThrobber(message) {
    $('.throbber-header').html(message);
    window.setTimeout(function () {
        $(".throbber").show();
    }, 1000);
}

function bindBootstrapTabSelectEvent(tabsId) {
    $('#' + tabsId + ' > ul li a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        var tabName = $(e.target).attr("data-tab-name");
        $("#selected-tab-name").val(tabName);
    });
}

function display_kendoui_grid_error(e) {
    if (e.errors) {
        showErrors(e.errors);
        //ignore empty error
    } else if (e.errorThrown) {
        showBSModal({ title: "ERROR", body: 'Error happened' });
    }
}

// CSRF (XSRF) security
function addAntiForgeryToken(data) {
    //if the object is undefined, create a new one.
    if (!data) {
        data = {};
    }
    //add token
    var tokenInput = $('input[name=__RequestVerificationToken]');
    if (tokenInput.length) {
        data.__RequestVerificationToken = tokenInput.val();
    }
    return data;
};

function saveUserPreferences(url, name, value) {
    var postData = {
        name: name,
        value: value
    };
    addAntiForgeryToken(postData);
    $.ajax({
        cache: false,
        url: url,
        type: 'post',
        data: postData,
        dataType: 'json',
        error: function (xhr, ajaxOptions, thrownError) {
            alert(BaseEam.Messages.SavePreferencesFailed);
        }
    });
};

//scroll to top
(function ($) {
    $.fn.backTop = function () {
        var backBtn = this;

        var position = 1000;
        var speed = 900;

        $(document).scroll(function () {
            var pos = $(window).scrollTop();

            if (pos >= position) {
                backBtn.fadeIn(speed);
            } else {
                backBtn.fadeOut(speed);
            }
        });

        backBtn.click(function () {
            $("html, body").animate({ scrollTop: 0 }, 900);
        });
    }
}(jQuery));

// Ajax activity indicator bound to ajax start/stop document events
$(document).ajaxStart(function () {
    //This is a central point to clear the error messages.
    clearAllErrors();

    $('#ajaxBusy').show();
}).ajaxStop(function () {
    $('#ajaxBusy').hide();
});

//Global error handling for authentication & authorization errors
$(document).ajaxError(function (e, xhr) {
    if (xhr.status == 401)
        window.location = "/User/Login";
    else if (xhr.status == 403)
        showBSModal({ title: "ERROR", body: BaseEam.Messages.PermissionRequired });
});

//Global PNotify setting
PNotify.prototype.options.styling = "bootstrap3";

function decode(str) {
    try {
        if (str)
            return decodeURIComponent(escape(str));
    }
    catch (e) {
        return str;
    }
    return str;
}

function displayNotification(message, type) {
    new PNotify({
        title: '',
        text: message,
        width: '350px',
        type: type == 'success' ? 'success' : 'error'
    });
}

// Handle ajax notifications
$(document)
    .ajaxSuccess(function (ev, xhr) {
        var msg = xhr.getResponseHeader('X-Message');
        if (msg) {
            displayNotification(decode(msg), xhr.getResponseHeader('X-Message-Type'));
        }
    })
    .ajaxError(function (ev, xhr) {
        var msg = xhr.getResponseHeader('X-Message');
        if (msg) {
            displayNotification(decode(msg), xhr.getResponseHeader('X-Message-Type'));
        }
        else {
            try {
                var data = JSON.parse(xhr.responseText);
                if (data.error && data.message) {
                    displayNotification(decode(data.message), "error");
                }
            }
            catch (ex) {
                displayNotification(xhr.responseText, "error");
            }
        }
    }
);

//dirty tracking
$(function () {
    // Enable dirty tracking on all forms
    $form = $('form[class!="eam-popup"]');

    if ($form !== undefined && $form !== null && $form.length > 0) {
        //don't track for panel-search
        $form.find('div.panel-search').find(':input').addClass('ays-ignore');

        $form.areYouSure();

        var isNew = $form.find('#IsNew').val();
        if (isNew == 'True') {
            $form.addClass('dirty');
        } else {
            $form.find('button[id="save"]').attr('disabled', 'disabled');
        }

        //register change events only if IsNew == false
        //the form is always dirty if IsNew == true
        if (isNew == 'False') {
            registerDirtyChangeEvents($form);
        }
    }
});

function registerDirtyChangeEvents($form) {
    $form.on('dirty.areYouSure', function () {
        // Enable save button only as the form is dirty.
        $(this).find('button[id="save"]').removeAttr('disabled');
    });
    $form.on('clean.areYouSure', function () {
        // Form is clean so nothing to save - disable the save button.
        $(this).find('button[id="save"]').attr('disabled', 'disabled');
    });
}

function refreshDirtyTracking() {
    //after saving entire form succesfully
    //we need to set form's state as clean
    //and start tracking again
    $('form').trigger('reinitialize.areYouSure');
    // Form is clean so nothing to save - disable the save button.
    $form.find('button[id="save"]').attr('disabled', 'disabled');

    //handling for IsNew
    var isNew = $form.find('#IsNew').val();
    if (isNew == 'True') {
        //set IsNew to false (it's not garbage anymore)
        $form.find('#IsNew').val('False');
        registerDirtyChangeEvents($form);
        $form.find('button[id="cancel"]').hide();
    }
}

//bootstrap 3 modal wrapper
showBSModal = function showBSModal(options) {

    var options = $.extend({
        title: '',
        body: '',
        remote: false,
        backdrop: 'static',
        size: false,
        onShow: false,
        onHide: false,
        actions: false
    }, options);

    self.onShow = typeof options.onShow == 'function' ? options.onShow : function () { };
    self.onHide = typeof options.onHide == 'function' ? options.onHide : function () { };

    if (self.$modal == undefined) {
        self.$modal = $('<div class="modal fade"><div class="modal-dialog"><div class="modal-content"></div></div></div>').appendTo('body');
        self.$modal.on('shown.bs.modal', function (e) {
            self.onShow.call(this, e);
        });
        self.$modal.on('hidden.bs.modal', function (e) {
            self.onHide.call(this, e);
            //remove the modal html so we won't care 
            //about dirty change tracking for newly added input tag
            $(this).remove(); //remove the modal html so we won't care about dirty change tracking
        });
    }

    var modalClass = {
        small: "modal-sm",
        large: "modal-lg"
    };

    self.$modal.data('bs.modal', false);
    self.$modal.find('.modal-dialog').removeClass().addClass('modal-dialog ' + (modalClass[options.size] || ''));
    self.$modal.find('.modal-content').html('<div class="modal-header"><button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button><h4 class="modal-title">${title}</h4></div><div class="modal-body">${body}</div><div class="modal-footer"></div>'.replace('${title}', options.title).replace('${body}', options.body));

    var footer = self.$modal.find('.modal-footer');
    if (Object.prototype.toString.call(options.actions) == "[object Array]") {
        for (var i = 0, l = options.actions.length; i < l; i++) {
            options.actions[i].onClick = typeof options.actions[i].onClick == 'function' ? options.actions[i].onClick : function () { };
            $('<button type="button" class="btn ' + (options.actions[i].cssClass || '') + '">' + (options.actions[i].label || '{Label Missing!}') + '</button>').appendTo(footer).on('click', options.actions[i].onClick);
        }
    } else {
        $('<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>').appendTo(footer);
    }

    self.$modal.modal(options);
}

//Lookup helper
setupCascadeLookup = function (parentFieldName, valueFieldId, textFieldId) {
    $('#' + parentFieldName).change(function (e) {
        e.preventDefault();
        $('#' + valueFieldId).val('');
        $('#' + textFieldId).val('');
    })
}

