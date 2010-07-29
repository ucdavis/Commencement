﻿(function($) {

    // attach this new method to jQuery
    $.fn.extend({

        enableTinyMce: function(options) {
            var script_location = "../../Scripts/tiny_mce/tiny_mce.js";

            var settings = $.extend({
                script_location: "../../Scripts/tiny_mce/tiny_mce.js"
            }, options);

            // iterate through each of the objects passed in to generate the calendar
            return this.each(function(index, item) {

                // add the rich text editor
                $(item).tinymce({
                    script_url: script_location,
                    // General options
                    theme: "advanced",
                    plugins: "safari,style,save,searchreplace,print,contextmenu,paste",

                    // Theme options
                    theme_advanced_buttons1: "print,|,bold,italic,underline,|,fontsizeselect",
                    theme_advanced_buttons2: "search,replace,|,undo,redo,|,bullist,numlist,|,myButton",
                    theme_advanced_buttons3: "",
                    theme_advanced_toolbar_location: "top",
                    theme_advanced_toolbar_align: "left",
                    theme_advanced_statusbar_location: "bottom",
                    theme_advanced_resizing: false,

                    // dimensions stuff
                    height: "400",
                    width: "400",

                    // Example content CSS (should be your site CSS)
                    //content_css: "css/Main.css",

                    // Drop lists for link/image/media/template dialogs
                    template_external_list_url: "js/template_list.js",
                    external_link_list_url: "js/link_list.js",
                    external_image_list_url: "js/image_list.js",
                    media_external_list_url: "js/media_list.js"
                });

            });

        } // end of gooCal
    }); // end of $.fn.extend
})(jQuery);