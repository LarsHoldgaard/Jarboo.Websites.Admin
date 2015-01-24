/*!
 * jQuery Plugin MVC Select update ajax
 *
 * @date 01 aug 2013
 * @version: 1.0
 * @author Frank Bonnet
 *
 * @dependencies:
 *	jquery.min.js
 */

(function ($) {

    /*
    * Plugin info
    *
    * The name of the plugin is used for the $.data
    * namespace of the selected element
    */
    var pluginInfo = {
        namespace: 'bonnet.ajax.dropdownlist',
        version: '1.0'
    }

    var main = {
        options: {
            id: pluginInfo.namespace,
            events: {
                beforeUpdate: null,
                afterUpdate: null
            },
            disableWhenEmpty: false,
            target: null,
            url: null
        },

        /*
        * Initialize the plugin. _create is only called once when the main object is created.
        *
        * @attribute this.options
        * @return    void
        */
        _create: function (options) {
            var _self = this;
            _self.setOptions(options);

            if (null === _self.options.target) {
                _self.options.target = $(document.getElementById(_self.element.attr("data-bonnet-ajax-update-target")));
            }

            if (null == _self.options.url) {
                _self.options.url = _self.element.attr("data-bonnet-ajax-update-url");
            }

            if (_self.element.attr("data-bonnet-ajax-update-disable-target-when-empty") == "True") {
                _self.options.disableWhenEmpty = true;
            }

            // Bind event
            _self.element.change(function () {             
                _self.update.apply(_self);
            });
        },

        /*
        * Merge options with this.options
        * 
        * @attribute options
        * @return    void
        */
        setOptions: function (options) {
            if ('object' === typeof options) {
                $.extend(true, this.options, options);
            }
        },

        /*
        * Get the complete array with options
        *
        * @attribute void
        * @return	 self.options
        */
        getOptions: function () {
            return this.options;
        },

        /*
        * Override or set one option in self.options
        *
        * @attribute string key
        * @attribute mix value
        * @return void
        */
        setOption: function (key, value) {
            self.options[key] = value;
        },

        /*
        * get one option in self.options
        *
        * @attribute string key
        * @return self.options.key
        */
        getOption: function (key) {
            return self.options.key;
        },

        /*
        * Get the current plugin version
        *
        * @attribute void
        * @return    pluginInfo.version
        */
        getVersion: function () {
            return pluginInfo.version;
        },

        /**
        * Updates the target
        * 
        * @attribute void
        * @return void
        */
        update: function () {
            var _self = this;
            
            // Call user defined function
            if (typeof _self.options.events.beforeUpdate === "function") {
                _self.options.events.beforeUpdate.apply(_self);
            }

            $.ajax({
                type: "GET",
                url: _self.options.url,
                data: {
                    Origin: _self.element.attr("id"),
                    Target: _self.options.target.attr("id"),
                    Value: _self.element.val()
                },
                success: function (data) {
                    // Select update
                    if (_self.options.target.is("select")) {
                        _self._updateSelect.apply(_self, [data]);
                    }

                    // Call user defined function
                    if (typeof _self.options.events.afterUpdate === "function") {
                        _self.options.events.afterUpdate.apply(_self, [data]);
                    }

                    _self.options.target.change();
                }
            });
        },

        /**
         * Update select
         * 
         * @attribute json|html
         * @return void
         */
        _updateSelect: function (data) {
            var _self = this;

            // Json
            if (typeof data === "object") {
                _self.options.target.html("");
                $(data).each(function () {
                    var _option = $("<option>");
                    _option.val(this.Value);
                    _option.text(this.Text);
                    
                    if (this.Selected) {
                        _option.attr("selected", "selected");
                    }

                    if (this.Disabled) {
                        _option.attr("disabled", "disabled");
                    }

                    _self.options.target.append(_option);
                });
            }

            // Html
            else {
                _self.options.target.html(data);
            }

            // Disable when empty
            if (_self.options.disableWhenEmpty && (data == null || typeof data.length === "undefined" || data.length == 0 || (data.length == 1 && data[0].Value == ""))) {
                _self.options.target.attr("disabled", "disabled");
            }

            else {
                _self.options.target.removeAttr("disabled");
            }
        }
    }

    /*
    * The place where the function is called
    * There is no need to change this
    */
    $.fn.BonnetAjaxUpdate = function (method) {
        if (method instanceof String && '_' == method.charAt(0)) {
            return this; // Disallow calls to private methods
        }

        var e = this;
        var response = e;
        var args;
        /*
        * Construct instance of plugin
        *
        * Instance is saved to $.data for retreival of instance on
        * next call
        */
        if (!method || 'object' === typeof method) {
            e.each(function () {
                var instance = $.data(
                    this, pluginInfo.namespace);
                if (!instance) {
                    instance = $.data(
                        this, pluginInfo.namespace, $.extend(true, {}, new Object(main)));
                    instance.element = $(this);
                }
                response = instance._create.apply(instance, args);
            });
        }
        /*
	    * Manualy call method
	    */
        else if (main[method]) {
            e.each(function () {
                var instance = $.data(
                    this, pluginInfo.namespace);
                args = [].slice.call(args, 1);
                response = instance[method].apply(instance, args);
            });
        }
        return response;
    }
})(jQuery);

// Initialize plugin
$(function () {
    $("[data-bonnet-ajax-update-enabled]").BonnetAjaxUpdate();
});
