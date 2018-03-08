var BaseEam = BaseEam || {};
BaseEam.Grid = function () {
    //Normalize delete row & selected rows
    //parent - child relationship = FK relationship
    //Normalize MVC actions that have parameters: id, parentId, selectedIds
    var deleteRow = function (parentId, id, actionUrl, gridToRefresh, redirectUrl, confirmation) {
        if (confirmation == true) {
            $.when(kendo.ui.ExtOkCancelDialog.show({
                width: "350px",
                title: "WARNING!",
                message: 'Are you sure you want to delete this item?',
                icon: 'k-ext-warning'
            })).done(function (response) {
                if (response.button == "OK") {
                    var postData = {
                        parentId: parentId,
                        id: id
                    };
                    addAntiForgeryToken(postData);

                    $.ajax({
                        cache: false,
                        type: "POST",
                        url: actionUrl,
                        data: postData,
                        success: function (data) {
                            if (data && data.Errors) {
                                showErrors(data.Errors);
                                return;
                            }
                            if (gridToRefresh != "") {
                                //reload grid
                                var grid = $('#' + gridToRefresh).data('kendoGrid');
                                grid.dataSource.read();
                            }
                            if (redirectUrl != "") {
                                //before redirecting, clear the dirty flag of form
                                //so it will redirect even if the form has changes
                                $('form').removeClass('dirty');
                                window.location = redirectUrl;
                            }
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(thrownError);
                        },
                        traditional: true
                    });
                }
            });
        } else {
            var postData = {
                parentId: parentId,
                id: id
            };
            addAntiForgeryToken(postData);

            $.ajax({
                cache: false,
                type: "POST",
                url: actionUrl,
                data: postData,
                success: function (data) {
                    if (data && data.Errors) {
                        showErrors(data.Errors);
                        return;
                    }
                    if (gridToRefresh != "") {
                        //reload grid
                        var grid = $('#' + gridToRefresh).data('kendoGrid');
                        grid.dataSource.read();
                    }
                    if (redirectUrl != "") {
                        //before redirecting, clear the dirty flag of form
                        //so it will redirect even if the form has changes
                        $('form').removeClass('dirty');
                        window.location = redirectUrl;
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert(thrownError);
                },
                traditional: true
            });
        }

        return false;
    };

    var deleteSelectedRows = function (parentId, selectedIds, actionUrl, gridToRefresh, redirectUrl, confirmation) {
        if (selectedIds.length == 0) {
            $.when(kendo.ui.ExtAlertDialog.show({
                title: "ALERT!",
                message: 'Please select items to delete!',
                icon: 'k-ext-error'
            }))
                .done(function () {
                    return;
                });
        } else {
            if (confirmation == true) {
                $.when(kendo.ui.ExtOkCancelDialog.show({
                    title: "WARNING!",
                    message: 'Are you sure you want to delete this item?',
                    icon: 'k-ext-warning'
                })).done(function (response) {
                    if (response.button == "OK") {
                        var postData = {
                            parentId: parentId,
                            selectedIds: selectedIds
                        };
                        addAntiForgeryToken(postData);

                        $.ajax({
                            cache: false,
                            type: "POST",
                            url: actionUrl,
                            data: postData,
                            success: function (data) {
                                if (data && data.Errors) {
                                    showErrors(data.Errors);
                                    return;
                                }
                                //reset selectedIds
                                selectedIds.length = 0;

                                if (gridToRefresh != "") {
                                    //reload grid
                                    var grid = $('#' + gridToRefresh).data('kendoGrid');
                                    grid.dataSource.read();
                                }
                                if (redirectUrl != "") {
                                    window.location = redirectUrl;
                                }
                            },
                            error: function (xhr, ajaxOptions, thrownError) {
                                alert(thrownError);
                            },
                            traditional: true
                        });
                    }
                });
            } else {
                var postData = {
                    parentId: parentId,
                    selectedIds: selectedIds
                };
                addAntiForgeryToken(postData);

                $.ajax({
                    cache: false,
                    type: "POST",
                    url: actionUrl,
                    data: postData,
                    success: function (data) {
                        if (data && data.Errors) {
                            showErrors(data.Errors);
                            return;
                        }

                        //reset selectedIds
                        selectedIds.length = 0;

                        if (gridToRefresh != "") {
                            //reload grid
                            var grid = $('#' + gridToRefresh).data('kendoGrid');
                            grid.dataSource.read();
                        }
                        if (redirectUrl != "True") {
                            window.location = redirectUrl;
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(thrownError);
                    },
                    traditional: true
                });
            }

        }
        return false;
    };

    var showHideToolbar = function (gridSelector, allowCreate, allowUpdate, allowDelete) {
        $(gridSelector + " .k-grid-add").hide();
        $(gridSelector + " .k-grid-save-changes").hide();
        $(gridSelector + " .k-grid-cancel-changes").hide();
        if (allowCreate == 'True') {
            $(gridSelector + " .k-grid-add").show();
            $(gridSelector + " .k-grid-save-changes").show();
            $(gridSelector + " .k-grid-cancel-changes").show();
        } else if (allowUpdate == 'True' || allowDelete == 'True') {
            $(gridSelector + " .k-grid-save-changes").show();
            $(gridSelector + " .k-grid-cancel-changes").show();
        }
    }

    // If reloadData == false, don't reload grid data
    // If reloadData == undefined or true, reload grid data
    var saveChanges = function (actionUrl, gridSelector, errorSelector, reloadData) {
        var grid = $(gridSelector).data("kendoGrid"),
            parameterMap = grid.dataSource.transport.parameterMap;
        //get the new and the updated records
        var currentData = grid.dataSource.data();
        var updatedRecords = [];
        var createdRecords = [];
        for (var i = 0; i < currentData.length; i++) {
            if (currentData[i].isNew()) {
                //this record is new
                createdRecords.push(currentData[i].toJSON());
            } else if (currentData[i].dirty) {
                updatedRecords.push(currentData[i].toJSON());
            }
        }
        //this records are deleted
        var deletedRecords = [];
        for (var i = 0; i < grid.dataSource._destroyed.length; i++) {
            deletedRecords.push(grid.dataSource._destroyed[i].toJSON());
        }

        if (createdRecords.length == 0 && updatedRecords.length == 0 && deletedRecords.length == 0) {
            kendo.ui.ExtAlertDialog.show({ title: "ERROR", message: BaseEam.Messages.BatchRecordRequired, icon: 'k-ext-error' });
            return;
        }

        var data = {};
        $.extend(data, parameterMap({ updated: updatedRecords }), parameterMap({ deleted: deletedRecords }), parameterMap({ created: createdRecords }));
        addAntiForgeryToken(data);
        $.ajax({
            url: actionUrl,
            data: data,
            type: "POST",
            success: function (data) {
                if (data && data.Errors) {
                    showErrors(data.Errors, errorSelector);
                } else {
                    grid.dataSource._destroyed = [];
                    if (reloadData != false) {
                        grid.dataSource.read();
                    }

                    // fire grid_batch_saved event
                    window.EventBroker.publish('grid_batch_saved', null);
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                showBSModal({ title: "ERROR", body: thrownError });
            }
        });
    };

    //Used to map sort columns to format that server can understand
    //ex: sortMapping = [{from: '', to: ''}, {from: '', to: ''}]
    //sort has only one field now
    var mapSortColumns = function (sort, sortMapping) {
        if (sort && sort.length > 0 && sortMapping && sortMapping.length > 0) {
            $.each(sortMapping, function (index, value) {
                if (sort[0].field == value.from) {
                    sort[0].field = value.to;
                }
            });
        }
    };

    //wrap dirty field with a red flag
    var dirtyHtml = function (data, fieldName) {
        if (data.dirty && data.dirtyFields[fieldName]) {
            return "<span class='k-dirty'></span>"
        }
        else {
            return "";
        }
    };

    var timeEditor = function (container, model) {
        $('<input data-text-field="' + model.field + '" data-value-field="' + model.field + '" data-bind="value:' + model.field + '" data-format="' + model.format + '"/>')
            .appendTo(container)
            .kendoTimePicker({});
    }

    var dateTimeEditor = function (container, model) {
        $('<input data-text-field="' + model.field + '" data-value-field="' + model.field + '" data-bind="value:' + model.field + '" data-format="' + model.format + '"/>')
            .appendTo(container)
            .kendoDateTimePicker({});
    }

    var comboBoxEditor = function (container, model) {
        $("<input name = '" + model.field + "Name" + "' required data-required-msg='" + model.required_msg + "' data-bind='value:" + model.field + "'/>")
	        .appendTo(container)
	        .kendoComboBox({
	            autoBind: false,
	            dataTextField: "Name",
	            dataValueField: "Id",
	            filter: "contains",
	            minLength: 2,
	            change: function (e) {
	                //if not a valid value, clear and reset the filter
	                if (this.value() && this.selectedIndex == -1) {
	                    this.text('');
	                    this.dataSource.filter(null);
	                    this.dataSource.read();
	                }
	            },
	            dataSource: {
	                type: "json",
	                serverFiltering: true,
	                transport: {
	                    read: {
	                        url: model.url,
	                        type: "POST",
	                        dataType: "json",
	                    },
	                    parameterMap: function (data, action) {
	                        return addAntiForgeryToken({
	                            param: data.filter != null ? data.filter.filters[0].value : '',
	                            dbTable: model.dbTable,
	                            parentValue: model.parentValue,
	                            additionalValue: model.additionalValue
	                        });
	                    }
	                }
	            }
	        });
        $("<span class='k-invalid-msg' data-for='" + model.field + 'Name' + "'></span>").appendTo(container);
    };

    return {
        deleteRow: deleteRow,
        deleteSelectedRows: deleteSelectedRows,
        saveChanges: saveChanges,
        showHideToolbar: showHideToolbar,
        mapSortColumns: mapSortColumns,
        dirtyHtml: dirtyHtml,
        comboBoxEditor: comboBoxEditor,
        timeEditor: timeEditor,
        dateTimeEditor: dateTimeEditor
    };
}();
