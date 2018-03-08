(function ($) {
    $.widget('baseeam.visual', {
        options: {
            cellId: ''
        },

        _create: function () {
            var that = this;
            this.options.cellId = this.element.attr('id');
            this.refreshVisual(true);
            this.element.find('#delete')
                    .bind('click', function (e) {
                        e.preventDefault();
                        //no need to render chart-filter
                        that._deleteVisual();
                    });
        },

        refreshVisual: function (renderFilter) {
            var that = this,
                searchValues = this.element.find(':input').serialize();

            var postData = {
                cellId: this.options.cellId,
                searchValues: searchValues
            };
            addAntiForgeryToken(postData);
            $.ajax({
                cache: false,
                type: "POST",
                url: "/Dashboard/GetUserDashboardVisual",
                data: postData,
                success: function (data) {
                    that._displayVisual(data, renderFilter);
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    showBSModal({ title: 'ERROR', body: thrownError });
                },
                traditional: true
            });
        },

        _displayVisual: function (data, renderFilter) {
            var that = this;
            if (!data) {
                this.addVisualButton = $('<div style="height:290px"><button type="button" class="btn-lg btn-primary btn-master chart-add-button" title="Add Visual">' +
                    '<i class="fa fa-plus-square"></i></button></div>');

                this.addVisualButton.appendTo('#' + that.options.cellId + ' .chart-stage')
                    .bind('click', function (e) {
                        e.preventDefault();
                        $.get('/Dashboard/SLVisualView?cellId=' + that.options.cellId, function (data) {
                            showBSModal({
                                title: "Select Visuals",
                                body: data,
                                actions: [{
                                    label: 'Cancel',
                                    cssClass: 'btn-danger',
                                    onClick: function (e) {
                                        $(e.target).parents('.modal').modal('hide');
                                    }
                                }]
                            });
                        });
                    });
            } else {
                this.element.find('.chart-title > span').html(data.Name);
                if (renderFilter == true) {
                    this.element.find('.chart-filter').append(data.VisualFilterHtml);
                }
                // unbind first
                this.element.find('#refresh').unbind('click');

                // then bind again
                this.element.find('#refresh')
                    .bind('click', function (e) {
                        e.preventDefault();
                        //no need to render chart-filter
                        that.refreshVisual(false);
                    });
                //display chart
                this._displayChart(data);
            }
        },

        _displayChart: function (data) {
            var el = document.getElementById(this.options.cellId).querySelector('.chart-stage');
            switch (data.Type) {
                //Area
                case 0:
                    this.dataviz = new Dataviz()
                        .height(300)
                        .el(el)
                        .type('area')
                        .call(function () {
                            this.dataset.matrix = data.VisualJsonData;
                        })
                        .render();
                    break;
                    //Area_Spline
                case 1:
                    this.dataviz = new Dataviz()
                        .height(300)
                        .el(el)
                        .type('area-spline')
                        .call(function () {
                            this.dataset.matrix = data.VisualJsonData;
                        })
                        .render();
                    break;
                    //Area_Step
                case 2:
                    this.dataviz = new Dataviz()
                        .height(300)
                        .el(el)
                        .type('area-step')
                        .call(function () {
                            this.dataset.matrix = data.VisualJsonData;
                        })
                        .render();
                    break;
                    //Bar
                case 3:
                    this.dataviz = new Dataviz()
                        .height(300)
                        .el(el)
                        .type('bar')
                        .call(function () {
                            this.dataset.matrix = data.VisualJsonData;
                        })
                        .render();
                    break;
                    //Bar_Stacked
                case 4:
                    this.dataviz = new Dataviz()
                        .height(300)
                        .el(el)
                        .type('bar')
                        .stacked(true)
                        .call(function () {
                            this.dataset.matrix = data.VisualJsonData;
                        })
                        .render();
                    break;
                    //Donut
                case 5:
                    this.dataviz = new Dataviz()
                        .height(300)
                        .el(el)
                        .type('donut')
                        .call(function () {
                            this.dataset.matrix = data.VisualJsonData;
                        })
                        .render();
                    break;
                    //Gauge
                case 6:
                    this.dataviz = new Dataviz()
                        .height(300)
                        .el(el)
                        .type('gauge')
                        .call(function () {
                            this.dataset.matrix = data.VisualJsonData;
                        })
                        .render();
                    break;
                    //Line
                case 7:
                    this.dataviz = new Dataviz()
                        .height(300)
                        .el(el)
                        .type('line')
                        .call(function () {
                            this.dataset.matrix = data.VisualJsonData;
                        })
                        .render();
                    break;
                    //Line_Stacked
                case 8:
                    this.dataviz = new Dataviz()
                        .height(300)
                        .el(el)
                        .type('line')
                        .stacked(true)
                        .call(function () {
                            this.dataset.matrix = data.VisualJsonData;
                        })
                        .render();
                    break;
                    //Pie
                case 9:
                    this.dataviz = new Dataviz()
                        .height(300)
                        .el(el)
                        .type('piechart')
                        .call(function () {
                            this.dataset.matrix = data.VisualJsonData;
                        })
                        .render();
                    break;
                    //Spline
                case 10:
                    this.dataviz = new Dataviz()
                        .height(300)
                        .el(el)
                        .type('spline')
                        .call(function () {
                            this.dataset.matrix = data.VisualJsonData;
                        })
                        .render();
                    break;
                    //Step
                case 11:
                    this.dataviz = new Dataviz()
                        .height(300)
                        .el(el)
                        .type('step')
                        .call(function () {
                            this.dataset.matrix = data.VisualJsonData;
                        })
                        .render();
                    break;
                    //Metric
                case 12:
                    this.dataviz = new Dataviz()
                        .height(290)
                        .el(el)
                        .type('metric')
                        .prepare()
                        .call(function () {
                            this.dataset.matrix = data.VisualJsonData;
                        })
                        .render();
                    break;
                default:
                    break;
            }
        },

        _deleteVisual: function (cellId) {
            var that = this;
            var postData = {
                cellId: this.options.cellId
            };
            addAntiForgeryToken(postData);
            $.ajax({
                cache: false,
                type: "POST",
                url: "/Dashboard/DeleteUserDashboardVisual",
                data: postData,
                success: function (data) {
                    that.element.find('.chart-title > span').html('Cell Title');
                    that.element.find('.chart-filter').html('');
                    that.element.find('.chart-stage').html('');

                    that.refreshVisual(true);
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    showBSModal({ title: 'ERROR', body: thrownError });
                },
                traditional: true
            });
        }
    });
}(jQuery));