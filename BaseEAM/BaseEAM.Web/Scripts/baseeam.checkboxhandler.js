var BaseEam = BaseEam || {};
BaseEam.CheckBoxHandler = function (gridId) {
    this.gridId = gridId;
    this.selectedIds = [];
    var that = this;

    this.init = function () {
        $('#' + this.gridId).on('change', '#' + this.gridId + '-mastercheckbox', (function () {
            $('#' + that.gridId + ' input.checkboxGroups').attr('checked', $(this).is(':checked')).change();
        }));

        //wire up checkboxes.
        $('#' + this.gridId).on('change', 'input[type=checkbox][id!=' + this.gridId + '-mastercheckbox]', function (e) {
            var $check = $(this);
            if ($check.is(":checked") == true) {
                var checked = jQuery.inArray($check.val(), that.selectedIds);
                if (checked == -1) {
                    //add id to selectedIds.
                    that.selectedIds.push($check.val());
                }
            }
            else {
                var checked = jQuery.inArray($check.val(), that.selectedIds);
                if (checked > -1) {
                    //remove id from selectedIds.
                    that.selectedIds.splice(checked, 1);
                }
            }
            that.updateMasterCheckbox(that.gridId);
        });
    };

    this.onDataBound = function (e) {
        $('#' + that.gridId + ' input[type=checkbox][id!=' + this.gridId + '-mastercheckbox]').each(function () {
            var currentId = $(this).val();
            var checked = jQuery.inArray(currentId, that.selectedIds);
            //set checked based on if current checkbox's value is in selectedIds.
            $(this).attr('checked', checked > -1);
        });

        that.updateMasterCheckbox(this.gridId);
    };

    this.updateMasterCheckbox = function () {
        var numChkBoxes = $('#' + this.gridId + ' input[type=checkbox][id!=' + this.gridId + '-mastercheckbox]').length;
        var numChkBoxesChecked = $('#' + this.gridId + ' input[type=checkbox][id!=' + this.gridId + '-mastercheckbox]:checked').length;
        $('#' + this.gridId + '-mastercheckbox').attr('checked', numChkBoxes == numChkBoxesChecked && numChkBoxes > 0);
    };
};