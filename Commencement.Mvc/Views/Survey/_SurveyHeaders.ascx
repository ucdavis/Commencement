<script type="text/javascript">
    $(function () {
        $('.boolother').change(function () {

            var name = $(this).attr('name');
            var type = $(this).attr('type');
            var controls = $('input[name="' + name + '"]');

            // typing into the text box, auto mark yes
            if (type == 'text') {
                $.each(controls, function (index, item) {
                    if ($(item).val() == "Yes") {
                        $(item).attr('checked', true);
                    }
                });
            }

            // selected no, blank the text box
            if (type == 'radio' && $(this).val() == 'No') {
                $.each(controls, function (index, item) {
                    if ($(item).attr('type') == 'text') {
                        $(item).val('');
                    }
                });
            }
        });

    });
</script>

<style type="text/css">
    #survey-container { width: 675px; }
    .question { margin: 1em; }
    .prompt { font-weight: bold;width: 100%;margin-bottom: .5em;line-height: 18px;}
    .group-header { font-size: x-large;font-weight: bold;border-bottom: 1px solid black;padding-bottom: .25em; }
        
    label.radio, label.checkbox { display: block;margin: .25em;}
    div.option { margin: .5em 0;}
    select { min-width: 450px;}
    textarea { width: 100%;}
    input[type='text'] { min-width: 300px;}
        
    fieldset {
        padding: 0;
        margin: 0;
        border-bottom: 0px;
        border-right: 0px;
        border-left: 0px;
        padding-left: 1em;
    }
    fieldset p { margin: .5em;}
        
    #error-container { color: red;font-weight: bold; }
    #error-container ul { margin-top: 1em;}
    #error-container li { margin-left: 20px;}
</style>