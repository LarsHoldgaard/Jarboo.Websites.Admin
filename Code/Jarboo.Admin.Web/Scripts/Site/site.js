$(function () {
    $(document).on("submit", ".delete-btn-form", function () {
        return confirm("Are you sure you want to delete this item?");
    });

    $(".input-daterange").datepicker({});

    $(".dataTable").each(function() {
        var $this = $(this);
        var onError = function() {
            $this.replaceWith('<div class="alert alert-danger" role="alert">Coudn\'t load table data.</div>');
        }

        var token = $this.parent().find('input[name="__RequestVerificationToken"]').val();

        var configUrl = $this.data("config-url");

        if (configUrl) {
            $.ajax({
                url: configUrl,
                dataType: "json",
                success: function(config) {
                    console.log(config);

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
                        };
                        var initUrl = ajax.url;
                        config.preDrawCallback = function () {
                            ajax.url = initUrl + (initUrl.indexOf("?") == -1 ? "?" : "&");
                            ajax.url += $filter.find("input, textarea, select").serialize().replace(/=on\b/, "=true");
                        }
                        $filter.on("change", function() {
                            $this.DataTable().draw();
                        });
                    }

                    $this.dataTable(config);
                },
                error: onError
            });
        } else {
            $this.dataTable();
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
                        
                        return "\
<form method='post' class='delete-btn-form pull-left' action='" + data[1] + "'>\
    <input type='hidden' value='" + token + "' name='__RequestVerificationToken'>\
    <input type='hidden' value='" + data[0] + "' name='id'>\
    <input type='hidden' value='" + window.location.pathname + window.location.search + "' name='returnUrl'>\
    <button type='submit' class='btn btn-danger btn-xs'><span aria-hidden='true' class='glyphicon glyphicon-remove-sign'></span> Delete</button>\
</form>";
                    }
                }
        }

        return undefined;
    }
});