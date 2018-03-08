/// <version>2013.04.14</version>
/// <summary>Works with the Kendo UI 2013 Q1 and jQuery 1.9.1</summary>
(function (kendo, $) {

    /*
     *
     * ExtComboBoxTreeView
     *
     */

    var ExtComboBoxTreeView = kendo.ui.Widget.extend({
        _uid: null,
        _treeview: null,
        _combobox: null,

        init: function (element, options) {
            var that = this;
            //we have 2 modes: 'search' & 'normal'
            //default mode = 'normal'
            that.options.mode = 'normal';
            //update to outside options => we can access when refresh
            that.options.info.id = options.info.id;
            that.options.info.hierarchyIdPath = options.info.hierarchyIdPath;
            that.options.info.hierarchyNamePath = options.info.hierarchyNamePath;
            that.options.selectedId = parseInt(options.info.id);

            kendo.ui.Widget.fn.init.call(that, element, options);

            that._uid = new Date().getTime();

            $(element).append(kendo.format("<input id='extComboBox{0}' class='k-ext-combobox' style='width:600px'/><button id='ext-search{0}' value='Search'><i class='fa fa-search' aria-hidden='true'></i></button><button id='ext-refresh{0}' value='Refresh'><i class='fa fa-refresh' aria-hidden='true'></i></button><button id='ext-clear{0}' value='Clear'><i class='fa fa-eraser' aria-hidden='true'></i></button>", that._uid));
            $(element).append(kendo.format("<div id='extTreeView{0}' class='k-ext-treeview' style='z-index:1;'/>", that._uid));

            // Create the combobox.
            that._combobox = $(kendo.format("#extComboBox{0}", that._uid)).kendoComboBox({
                dataSource: [{ text: "", value: "" }],
                dataTextField: "text",
                dataValueField: "value",
                open: function (e) {
                    //to prevent the combobox from opening or closing. A bug was found when clicking on the combobox to 
                    //"close" it. The default combobox was visible after the treeview had closed.
                    e.preventDefault();

                    // If the treeview is not visible, then make it visible.
                    if (!$treeviewRootElem.hasClass("k-custom-visible")) {
                        // Position the treeview so that it is below the combobox.
                        $treeviewRootElem.css({
                            "top": $comboboxRootElem.position().top + $comboboxRootElem.height(),
                            "left": $comboboxRootElem.position().left
                        });

                        // Display the treeview.
                        $treeviewRootElem.slideToggle('fast', function () {
                            that._combobox.close();
                            $treeviewRootElem.addClass("k-custom-visible");
                        });

                        //expand and select node only in 'normal' mode
                        if (that.options.mode == 'normal' && that.options.selectedId > 0) {
                            //expandidlist need to be an array of int
                            var expandidlist = [];
                            var str = that.options.info.hierarchyIdPath.split(">");
                            for (var i in str) {
                                expandidlist.push(parseInt(str[i]));
                            }
                            //then we expand and select node after expanded
                            that._treeview.expandPath(expandidlist, function () {
                                var getitem = that._treeview.dataSource.get(that.options.selectedId);
                                var selectitem = that._treeview.findByUid(getitem.uid);
                                that._treeview.select(selectitem);
                            });
                            //if leaf node = selectedId => we select it manually
                            //if (expandidlist[expandidlist.length - 1] == that.options.selectedId) {
                            //    var getitem = that._treeview.dataSource.get(that.options.selectedId);
                            //    var selectitem = that._treeview.findByUid(getitem.uid);
                            //    that._treeview.select(selectitem);
                            //}
                        }
                    }
                }
            }).data("kendoComboBox");

            if (that.options.comboBoxWidth) {
                that._combobox._inputWrapper.width(that.options.comboBoxWidth);
            }

            var $comboboxRootElem = $(that._combobox.element).closest("span.k-combobox");

            //Bind event handler to search button
            $(kendo.format("#ext-search{0}", that._uid)).click(function (e) {
                e.preventDefault();
                //reload tree with searchName
                var searchName = $comboboxRootElem.find("input.k-input").val();
                if (searchName === undefined || searchName === '') {
                    alert('Please input text to search.')
                }
                else {
                    that.options.mode = 'search';
                    that._treeview.dataSource.read({ searchName: searchName });
                }
            });

            function searchCompleted(e) {
                //after search completed, set mode to 'search' and open the treeview
                if (that.options.mode == 'search') {
                    if (!$treeviewRootElem.hasClass("k-custom-visible")) {
                        // Display the treeview.
                        $treeviewRootElem.slideToggle('fast', function () {
                            that._combobox.close();
                            $treeviewRootElem.addClass("k-custom-visible");
                        });
                    }
                }
            }

            //Bind event handler to clear button
            $(kendo.format("#ext-clear{0}", that._uid)).click(function (e) {
                e.preventDefault();

                $comboboxRootElem.find("input.k-input").val('');
                //update selected id to hiddenInput
                $("#" + that.options.info.hidId).val('0');
                $("#" + that.options.info.hidHierarchyIdPath).val('');
                $("#" + that.options.info.hidHierarchyNamePath).val('');
            });

            //Bind event handler to refresh button
            $(kendo.format("#ext-refresh{0}", that._uid)).click(function (e) {
                e.preventDefault();
                //reset options
                that.refresh(options.info.id, options.info.hierarchyIdPath, options.info.hierarchyNamePath);
                //reset treeview
                that._treeview.dataSource.read();
            });

            // Create the treeview.
            that._treeview = $(kendo.format("#extTreeView{0}", that._uid)).kendoTreeView(options.treeview).data("kendoTreeView");
            //dataSource.read() doesn't support callback
            //so register change event to know when search is completed
            that._treeview.dataSource.bind("change", searchCompleted);
            that._treeview.bind("select", function (e) {
                // When a node is selected, display the text for the node in the combobox, hide the treeview and set the mode back to 'normal'.
                //$comboboxRootElem.find("span.k-input").text($(e.node).children("div").text());
                $comboboxRootElem.find("input.k-input").val(this.dataItem(e.node).HierarchyNamePath);
                //update selected id to hiddenInput
                $("#" + that.options.info.hidId).val(this.dataItem(e.node).id);
                $("#" + that.options.info.hidHierarchyIdPath).val(this.dataItem(e.node).HierarchyIdPath);
                $("#" + that.options.info.hidHierarchyNamePath).val(this.dataItem(e.node).HierarchyNamePath);
                that.options.selectedId = this.dataItem(e.node).id;
                that.options.info.hierarchyIdPath = this.dataItem(e.node).HierarchyIdPath;
                that.options.info.hierarchyNamePath = this.dataItem(e.node).HierarchyNamePath;
                if (that.options.mode == 'search') {
                    that.options.mode = 'normal';
                    that._treeview.dataSource.read();
                }
                $treeviewRootElem.slideToggle('fast', function () {
                    $treeviewRootElem.removeClass("k-custom-visible");
                    that.trigger("select", e);
                });
            });

            //auto select tree view node            
            if (that.options.selectedId > 0) {
                $comboboxRootElem.find("input.k-input").val(that.options.info.hierarchyNamePath);
            }

            var $treeviewRootElem = $(that._treeview.element).closest("div.k-treeview");

            // Hide the treeview.
            $treeviewRootElem
                .width($comboboxRootElem.width())
                .css({
                    "border": "1px solid grey",
                    "display": "none",
                    "position": "absolute",
                    "background-color": that._combobox.list.css("background-color")
                });

            $(document).click(function (e) {
                // Ignore clicks on the treetriew.
                if ($(e.target).closest("div.k-treeview").length === 0) {
                    // If visible, then close the treeview.
                    if ($treeviewRootElem.hasClass("k-custom-visible")) {
                        $treeviewRootElem.slideToggle('fast', function () {
                            $treeviewRootElem.removeClass("k-custom-visible");
                        });
                    }
                }
            });
        },

        comboBoxList: function () {
            return this._combobox;
        },

        treeview: function () {
            return this._treeview;
        },

        enable: function () {
            this._combobox.enable(true);
        },

        disable: function () {
            this._combobox.enable(false);
        },

        refresh: function (id, hierarchyIdPath, hierarchyNamePath) {
            this.options.selectedId = parseInt(id);
            this.options.info.id = id;
            this.options.info.hierarchyIdPath = hierarchyIdPath;
            this.options.info.hierarchyNamePath = hierarchyNamePath;

            $("#" + this.options.info.hidId).val(id);
            $("#" + this.options.info.hidHierarchyIdPath).val(hierarchyIdPath);
            $("#" + this.options.info.hidHierarchyNamePath).val(hierarchyNamePath);
            var $comboboxRootElem = $(this._combobox.element).closest("span.k-combobox");
            $comboboxRootElem.find("input.k-input").val(this.options.info.hierarchyNamePath);
        },

        options: {
            selectedId: 0,
            info: {},
            mode: "normal",
            name: "ExtComboBoxTreeView"
        }
    });
    kendo.ui.plugin(ExtComboBoxTreeView);

    /*
     *
     * ExtDropDownTreeView
     *
     */

    var ExtDropDownTreeView = kendo.ui.Widget.extend({
        _uid: null,
        _treeview: null,
        _dropdown: null,

        init: function (element, options) {
            var that = this;

            kendo.ui.Widget.fn.init.call(that, element, options);

            that._uid = new Date().getTime();

            $(element).append(kendo.format("<input id='extDropDown{0}' class='k-ext-dropdown' style='width: 600px;'/>", that._uid));
            $(element).append(kendo.format("<div id='extTreeView{0}' class='k-ext-treeview' style='z-index:1;'/>", that._uid));

            // Create the dropdown.
            that._dropdown = $(kendo.format("#extDropDown{0}", that._uid)).kendoDropDownList({
                dataSource: [{ text: "", value: "" }],
                dataTextField: "text",
                dataValueField: "value",
                open: function (e) {
                    //to prevent the dropdown from opening or closing. A bug was found when clicking on the dropdown to 
                    //"close" it. The default dropdown was visible after the treeview had closed.
                    e.preventDefault();
                    // If the treeview is not visible, then make it visible.
                    if (!$treeviewRootElem.hasClass("k-custom-visible")) {
                        // Position the treeview so that it is below the dropdown.
                        $treeviewRootElem.css({
                            "top": $dropdownRootElem.position().top + $dropdownRootElem.height(),
                            "left": $dropdownRootElem.position().left
                        });
                        // Display the treeview.
                        $treeviewRootElem.slideToggle('fast', function () {
                            that._dropdown.close();
                            $treeviewRootElem.addClass("k-custom-visible");
                        });
                    }
                }
            }).data("kendoDropDownList");

            if (options.dropDownWidth) {
                that._dropdown._inputWrapper.width(options.dropDownWidth);
            }

            var $dropdownRootElem = $(that._dropdown.element).closest("span.k-dropdown");

            // Create the treeview.
            that._treeview = $(kendo.format("#extTreeView{0}", that._uid)).kendoTreeView(options.treeview).data("kendoTreeView");
            that._treeview.bind("select", function (e) {
                // When a node is selected, display the text for the node in the dropdown and hide the treeview.
                //$dropdownRootElem.find("span.k-input").text($(e.node).children("div").text());
                $dropdownRootElem.find("span.k-input").text(this.dataItem(e.node).HierarchyNamePath);
                $treeviewRootElem.slideToggle('fast', function () {
                    $treeviewRootElem.removeClass("k-custom-visible");
                    that.trigger("select", e);
                });
            });

            var $treeviewRootElem = $(that._treeview.element).closest("div.k-treeview");

            // Hide the treeview.
            $treeviewRootElem
                .width($dropdownRootElem.width())
                .css({
                    "border": "1px solid grey",
                    "display": "none",
                    "position": "absolute",
                    "background-color": that._dropdown.list.css("background-color")
                });

            $(document).click(function (e) {
                // Ignore clicks on the treetriew.
                if ($(e.target).closest("div.k-treeview").length === 0) {
                    // If visible, then close the treeview.
                    if ($treeviewRootElem.hasClass("k-custom-visible")) {
                        $treeviewRootElem.slideToggle('fast', function () {
                            $treeviewRootElem.removeClass("k-custom-visible");
                        });
                    }
                }
            });
        },

        dropDownList: function () {
            return this._dropdown;
        },

        treeview: function () {
            return this._treeview;
        },

        options: {
            name: "ExtDropDownTreeView"
        }
    });
    kendo.ui.plugin(ExtDropDownTreeView);

    /*
     *
     * ExtDialog
     *
     */

    var ExtDialog = kendo.ui.Window.extend({
        _buttonTemplate: kendo.template('<div class="k-ext-dialog-buttons" style="position:absolute; bottom:10px; text-align:center; width:#= parseInt(width) - 14 #px;"><div style="display:inline-block"># $.each (buttons, function (idx, button) { # <button class="k-button" style="margin-right:5px; width:100px;">#= button.name #</button> # }) # </div></div>'),
        _contentTemplate: kendo.template('<div class="k-ext-dialog-content" style="height:#= parseInt(height) - 55 #px;; width:#= parseInt(width) - 14 #px;overflow:auto;">'),

        init: function (element, options) {
            /// <signature>
            ///   <summary>
            ///   Initialize the dialog.
            ///   </summary>
            /// </signature>

            var that = this;

            options.visible = options.visible || false;

            kendo.ui.Window.fn.init.call(that, element, options);
            $(element).data("kendoWindow", that);

            var html = $(element).html();
            $(element).html(that._contentTemplate(options));
            $(element).find("div.k-ext-dialog-content").append(html);

            $(element).after(that._buttonTemplate(options));

            $.each(options.buttons, function (idx, button) {
                if (button.click) {
                    $($(element).parent().find(".k-ext-dialog-buttons .k-button")[idx]).on("click", { handler: button.click }, function (e) {
                        e.data.handler({ button: this, dialog: that });
                    });
                }
            });

            that.bind("resize", function (e) {
                that.resizeDialog();
            });
        },

        resizeDialog: function () {
            var that = this;
            var $dialog = $(that.element);
            var width = $dialog.width();
            var height = $dialog.height();
            $dialog.parent().find(".k-ext-dialog-buttons").width(width);
            $dialog.parent().find(".k-ext-dialog-content").width(width).height(height - 39);
        },

        options: {
            name: "ExtDialog"
        }
    });
    kendo.ui.plugin(ExtDialog);



    /*
     *
     * AlertDialog
     *
     */

    kendo.ui.ExtAlertDialog = {
        show: function (options) {
            return new $.Deferred(function (deferred) {
                var dialog = null;

                if ($("#extAlertDialog").length > 0) {
                    $("#extAlertDialog").parent().remove();
                }

                options = $.extend({
                    width: "300px",
                    height: "100px",
                    buttons: [{
                        name: "OK",
                        click: function (e) {
                            dialog.close();
                            deferred.resolve({ button: "OK" });
                        }
                    }],
                    modal: true,
                    visible: false,
                    message: "",
                    icon: "k-ext-information" //k-ext-information, k-ext-question, k-ext-warning, k-ext-error
                }, options);

                $(document.body).append(kendo.format("<div id='extAlertDialog' style='position:relative;'><div style='display:inline-block'>{0}</div></div>", options.message.replace(/\r\n|\n\r|\r|\n/gi, "<br />")));
                dialog = $("#extAlertDialog").kendoExtDialog(options).data("kendoExtDialog");

                $("#extAlertDialog").parent().find("div.k-window-titlebar div.k-window-actions").empty();
                dialog.center().open();
            });
        },

        hide: function () {
            $("#extAlertDialog").data("kendoExtDialog").close();
        }
    };

    /*
     *
     * OkCancelDialog
     *
     */

    kendo.ui.ExtOkCancelDialog = {
        show: function (options) {
            return new $.Deferred(function (deferred) {
                if ($("#extOkCancelDialog").length > 0) {
                    $("#extOkCancelDialog").parent().remove();
                }

                options = $.extend({
                    width: "300px",
                    height: "100px",
                    buttons: [{
                        name: "OK",
                        click: function (e) {
                            $("#extOkCancelDialog").data("kendoExtDialog").close();
                            deferred.resolve({ button: "OK" });
                        }
                    }, {
                        name: "Cancel",
                        click: function (e) {
                            $("#extOkCancelDialog").data("kendoExtDialog").close();
                            deferred.resolve({ button: "Cancel" });
                        }
                    }],
                    modal: true,
                    visible: false,
                    message: "",
                    icon: "k-ext-information"
                }, options);

                $(document.body).append(kendo.format("<div id='extOkCancelDialog' style='position:relative;'><div style='display:inline-block;'>{0}</div></div>", options.message.replace(/\r\n|\n\r|\r|\n/gi, "<br />")));
                $("#extOkCancelDialog").kendoExtDialog(options);
                $("#extOkCancelDialog").parent().find("div.k-window-titlebar div.k-window-actions").empty();
                $("#extOkCancelDialog").data("kendoExtDialog").center().open();
            });
        }
    };

})(window.kendo, window.kendo.jQuery);
