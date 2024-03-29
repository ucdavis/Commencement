﻿(function($) {

    // attach this new method to jQuery
    $.fn.extend({

        enableTinyMce: function(options) {

            var settings = $.extend({
                script_location: "../../Scripts/tiny_mce/tiny_mce.js",
                overrideHeight: "400",
                overrideWidth: "400",
                overrideOnchange: "",
                //http://www.matthewkenny.com/2009/05/tinymce-and-jquery-validation/
                initInstanceCallback: 
                    function (editor) {
                        //Get the textarea
                        var container = $('#' + editor.editorId);

                        //Get the form submit buttons for the textarea
                        $(editor.formElement).find("input[type=submit]").click(
                            function (event) {
                                container.val(editor.getContent());
                            }
                        );
                    }
            }, options);


            // iterate through each of the objects passed in to generate the calendar
            return this.each(function(index, item) {

                // add the rich text editor
                $(item).tinymce({
                    script_url: settings.script_location,
                    // General options
                    theme: "advanced",
                    plugins: "safari,style,save,searchreplace,print,contextmenu,paste",

                    // Theme options
                    theme_advanced_buttons1: "print,|,bold,italic,underline,|,fontsizeselect,search,replace,|,pastetext,pasteword,|,undo,redo,|,bullist,numlist,|,myButton",
                    theme_advanced_buttons2: "",
                    theme_advanced_buttons3: "",
                    theme_advanced_toolbar_location: "top",
                    theme_advanced_toolbar_align: "left",
                    theme_advanced_statusbar_location: "bottom",
                    theme_advanced_resizing: false,

                    // dimensions stuff
                    height: settings.overrideHeight,
                    width: settings.overrideWidth,

                    // Example content CSS (should be your site CSS)
                    //content_css: "css/Main.css",

                    // Drop lists for link/image/media/template dialogs
                    template_external_list_url: "js/template_list.js",
                    external_link_list_url: "js/link_list.js",
                    external_image_list_url: "js/image_list.js",
                    media_external_list_url: "js/media_list.js",

                    onchange_callback : settings.overrideOnchange,
                    init_instance_callback: settings.initInstanceCallback
                });

            });

        } // end of gooCal
    }); // end of $.fn.extend
})(jQuery);



//tinyMCE.init({
//    mode: 'textareas',
//    theme: 'advanced',
//    theme_advanced_buttons1: 'bold,italic,underline',
//    theme_advanced_buttons2: '',
//    theme_advanced_buttons3: '',
//    theme_advanced_toolbar_location: 'top',
//    theme_advanced_toolbar_align: 'left',
//    theme_advanced_statusbar_location: 'bottom',
//    init_instance_callback: "initialiseInstance"
//});

