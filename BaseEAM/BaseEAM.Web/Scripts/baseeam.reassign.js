(function ($) {
    $.widget('baseeam.reassign', {
        options: {
            entityId: 0,
            entityType: ''
        },

        _create: function () {
            var that = this;
            this.element.bind('click', function (e) {
                e.preventDefault();
                $.get('/Lookup/MLUserView', function (data) {
                    showBSModal({
                        title: "Reassign",
                        size: "large",
                        body: data,
                        actions: [{
                            label: 'Reassign',
                            cssClass: 'btn-success',
                            onClick: function (e) {
                                that._reassign(e);
                            }
                        }, {
                            label: 'Cancel',
                            cssClass: 'btn-danger',
                            onClick: function (e) {
                                $(e.target).parents('.modal').modal('hide');
                            }
                        }]
                    });
                });
            });
        },

        _reassign: function (e) {
            var that = this;
            var postData = {
                entityId: this.options.entityId,
                entityType: this.options.entityType,
                selectedIds: usersCheckboxHandler.selectedIds
            };
            addAntiForgeryToken(postData);
            $.ajax({
                cache: false,
                type: "POST",
                url: '/Common/Reassign',
                data: postData,
                success: function (data) {
                    if (data && data.Errors) {
                        showErrors(data.Errors, '#ml-users-error');
                    } else {
                        //hide modal popup
                        $(e.target).parents('.modal').modal('hide');

                        //refresh assignedUsers text
                        $('#assignedUsers').html(data.assignedUsers);

                        //fire status changed event
                        window.EventBroker.publish(BaseEam.Events.StatusChanged, null);
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    showBSModal({ title: "Error", body: thrownError });
                },
                traditional: true
            });
        }
    });
}(jQuery));