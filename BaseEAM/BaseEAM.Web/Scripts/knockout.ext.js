/*
    Custom bindings for integrating knockout and kendo
    http://knockoutjs.com/documentation/custom-bindings.html
*/

ko.bindingHandlers.dateTimeEnabled = {
    update: function (element, valueAccessor, allBindings) {
        var value = valueAccessor();
        var valueUnwrapped = ko.unwrap(value);
        if (valueUnwrapped == true) {
            $(element).prop('disabled', false);
            $(element).data("kendoDateTimePicker").enable(true);
        } else {
            $(element).prop('disabled', true);
            $(element).data("kendoDateTimePicker").enable(false);
        }
    }
};

ko.bindingHandlers.dateEnabled = {
    update: function (element, valueAccessor, allBindings) {
        var value = valueAccessor();
        var valueUnwrapped = ko.unwrap(value);
        if (valueUnwrapped == true) {
            $(element).prop('disabled', false);
            $(element).data("kendoDatePicker").enable(true);
        } else {
            $(element).prop('disabled', true);
            $(element).data("kendoDatePicker").enable(false);
        }
    }
};

ko.bindingHandlers.timeEnabled = {
    update: function (element, valueAccessor, allBindings) {
        var value = valueAccessor();
        var valueUnwrapped = ko.unwrap(value);
        if (valueUnwrapped == true) {
            $(element).prop('disabled', false);
            $(element).data("kendoTimePicker").enable(true);
        } else {
            $(element).prop('disabled', true);
            $(element).data("kendoTimePicker").enable(false);
        }
    }
};

ko.bindingHandlers.comboboxEnabled = {
    //register dataBound event to enable/disable after data was bound
    //if not, it will clear the 'disabled' flag when data was bound
    init: function (element, valueAccessor, allBindings) {
        $(element).data("kendoComboBox").bind("dataBound", function (e) {
            var value = valueAccessor();
            var valueUnwrapped = ko.unwrap(value);
            if (valueUnwrapped == true) {
                $(element).data("kendoComboBox").enable(true);
            } else {
                $(element).data("kendoComboBox").enable(false);
            }
        });
    },
    update: function (element, valueAccessor, allBindings) {
        var value = valueAccessor();
        var valueUnwrapped = ko.unwrap(value);
        if (valueUnwrapped == true) {
            $(element).data("kendoComboBox").enable(true);
        } else {
            $(element).data("kendoComboBox").enable(false);
        }
    }
};

ko.bindingHandlers.gridDeleteRowEnabled = {
    //register dataBound event to enable/disable command button after data was bound
    init: function (element, valueAccessor, allBindings) {
        $(element).data("kendoGrid").bind("dataBound", function (e) {
            var value = valueAccessor();
            var valueUnwrapped = ko.unwrap(value);
            if (valueUnwrapped == true) {
                $(element).find('.deleteRow').prop('disabled', false);
            } else {
                $(element).find('.deleteRow').prop('disabled', true);
            }
        });
    },
    update: function (element, valueAccessor, allBindings) {
        //need valueAccessor() here so knockout can trigger binding
        //https://stackoverflow.com/questions/23878702/knockout-custom-binding-update-not-firing
        var value = valueAccessor();
        var valueUnwrapped = ko.unwrap(value);
        //refresh grid to trigger dataBound event
        $(element).data("kendoGrid").dataSource.page(1);
    }
};

ko.bindingHandlers.lookupEnabled = {
    update: function (element, valueAccessor, allBindings) {
        var value = valueAccessor();
        var valueUnwrapped = ko.unwrap(value);
        if (valueUnwrapped == true) {
            $(element).find('input').prop('disabled', false);
            $(element).find('button').prop('disabled', false);
        } else {
            $(element).find('input').prop('disabled', true);
            $(element).find('button').prop('disabled', true);
        }
    }
};