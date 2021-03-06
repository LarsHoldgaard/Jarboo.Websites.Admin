﻿$(function() {
    // When rending an underscore template, we want top-level
    // variables to be referenced as part of an object. For
    // technical reasons (scope-chain search), this speeds up
    // rendering; however, more importantly, this also allows our
    // templates to look / feel more like our server-side
    // templates that use the rc (Request Context / Colletion) in
    // order to render their markup.
    _.templateSettings.variable = "rc";
    var deleteBtnTemplate = _.template($("#deteleBtnTemplete").html());

    $(document).on("submit", ".delete-btn-form", function () {
        return confirm("Are you sure you want to delete this item?");
    });

    $(".input-daterange").datepicker({});
    $("input.datepicker").each(function () {
        var $this = $(this);
        $this.datepicker({
            startDate: $this.data("start-date"),
            endDate: $this.data("end-date")
        });
    });

    function formatHoursInput() {
        var $input = $(this);

        var text = $input.val();
        if (!text) {
            return;
        }

        var number = parseFloat(text);
        if (isNaN(number)) {
            number = 0;
        }

        number = (number * 2).toFixed() / 2;

        $input.val(number);
    }
    $("input[data-hours]").blur(formatHoursInput);

    $(".dataTable").each(function () {
        var $this = $(this);
        var onError = function () {
            $this.replaceWith('<div class="alert alert-danger" role="alert">Coudn\'t load table data.</div>');
        }

        var token = $this.parent().find('input[name="__RequestVerificationToken"]').val();

        var configUrl = $this.data("config-url");

        if (configUrl) {
            $.ajax({
                url: configUrl,
                dataType: "json",
                success: function (config) {
                    console.log('datatable config: ', config);

                    var ajax = config.ajax;
                    if (config.ajax) {
                        config.ajax = function (data, callback, settings) {
                            data.__RequestVerificationToken = token;

                            $.ajax({
                                url: ajax.url,
                                dataType: "json",
                                type: ajax.type,
                                data: data,
                                success: function(data) {
                                    callback(data);
                                },
                                error: onError
                            });
                        };
                    }

                    if (config.columns) {
                        for (var i = 0; i < config.columns.length; i++) {
                            var column = config.columns[i];
                            if (!column.type) {
                                continue;
                            }

                            column.render = getColumnRender(column.type, token);
                        }
                    }

                    var $filter = $this.parent().find(".data-table-filter");
                    if ($filter.length) {
                        config.initComplete = function () {
                            $filter.detach();
                            $filter.appendTo($this.parent().find(".dataTables_filter"));
                            $filter.show();

                            $this.parent().find(".dataTables_filter > label").hide();
                        };
                        var initUrl = ajax.url;
                        config.preDrawCallback = function () {
                            ajax.url = initUrl + (initUrl.indexOf("?") == -1 ? "?" : "&");
                            ajax.url += $filter.find("input, textarea, select").serialize().replace(/=on\b/, "=true");
                        }
                        $filter.on("change", function () {
                            $this.DataTable().draw();
                        });
                    }

                    $this.dataTable(config);
                },
                error: onError
            });
        } else {
            $this.dataTable({
                "pageLength": 25
            });
        }
    });

    function getColumnRender(columnType, token) {
        switch (columnType) {
            case "TaskLink":
                {
                    return function (data, type, row, meta) {
                        if (type != "display") {
                            return data[1];
                        }

                        return "<a href='/Tasks/View/" + data[0] + "'>" + data[1] + "</a>";
                    }
                }
            case "TaskStepLink":
                {
                    return function (data, type, row, meta) {
                        if (type != "display") {
                            return data[1];
                        }

                        return "<a href='/Tasks/Steps/" + data[0] + "'>" + data[1] + "</a>";
                    }
                }
            case "ProjectLink":
                {
                    return function (data, type, row, meta) {
                        if (type != "display") {
                            return data[1];
                        }

                        return "<a href='/Projects/View/" + data[0] + "'>" + data[1] + "</a>";
                    }
                }
            case "ExternalLink":
                {
                    return function (data, type, row, meta) {
                        if (type != "display") {
                            return null;
                        }

                        return "<a target='_blank' href='" + data + "'>Link <span class='glyphicon glyphicon-share' aria-hidden='true'></span></a>";
                    }
                }
            case "DeleteBtn":
                {
                    return function (data, type, row, meta) {
                        if (type != "display") {
                            return null;
                        }
                        var deleteBtnData = {
                            action: data[1],
                            token: token,
                            id: data[0],
                            returnUrl: window.location.pathname + window.location.search
                        };
                        return deleteBtnTemplate(deleteBtnData);
                    }
                }
        }

        return undefined;
    }
 
    $('#Taskview_dt_basic_question').dataTable({
        "order": [[3, "desc"]],
         "bDestroy": true
    });
   
    $('[data-morris-chart-src]').each(function () {
        var $this = $(this);

        var id = this.id;
        var type = $this.data("morris-chart-type");
        var src = $this.data("morris-chart-src");

        $.get(src, function (config) {
            config.element = id;
            config.xLabelFormat = function(d) {
                return (d.getMonth() + 1) + '/' + d.getDate();
            };
            switch (type) {
                case "line":
                    new Morris.Line(config);
                    break;
                case "area":
                    new Morris.Area(config);
                    break;
                case "donut":
                    new Morris.Donut(config);
                    break;
                case "bar":
                    new Morris.Bar(config);
                    break;
            }
        });
    });
});