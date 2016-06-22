<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Commencement.Controllers.SurveyCreateViewModel>" %>

<style type="text/css">
    .buttons { padding: .5em 1em;}
    .warning { color: red;}
    .error { margin-left: 7px;color: red;}
        
    .option-row td, .validator-row td { min-height: 50px;background-color: #F2F2F2;padding: .5em; }
    .option-row ul, .validator-row ul { list-style: none; }
    .option-row li, .validator-row li { float: left;display: block;width: 50%; }
    
    .bordered-row hr { margin: 10px 0;color: #f3f3f3;}
</style>

<script type="text/javascript" src="<%: Url.Content("~/Scripts/knockout-2.2.0.js") %>"></script>
    
<script type="text/javascript">

    var fieldTypes = <%= new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(Model.SurveyFieldTypes.Select(a => new {id=a.Id, name=a.Name, hasOptions = a.HasOptions})) %>;
    var validators = <%= new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(Model.SurveyFieldValidators.Select(a => new {id=a.Id, name=a.Name})) %>;
    var questions = <%= new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(Model.Survey.SurveyFields.Select(a => 
                        new
                            {
                                id=a.Id,
                                prompt=a.Prompt, 
                                fieldtype = a.SurveyFieldType.Id, 
                                options = a.SurveyFieldOptions.Select(b => new {id=b.Id, name=b.Name}),
                                validators = a.SurveyFieldValidators.Select(b => new {id=b.Id, name=b.Name})
                            })) %>;

    (function(commencement, $, undefined) {

        "use strict";

        var options = {};

        commencement.options = function(o) { $.extend(options, o); };

        commencement.init = function() {
            createModels();
            ko.applyBindings(commencement.Survey);

            configureModals();
            //configureSave();
            configureLoad();
        };

        function createModels() {

            commencement.Question = function(prompt, fieldType, qid) {
                var self = this;

                self.id = ko.observable(qid);
                self.prompt = ko.observable(prompt);
                self.fieldType = ko.observable(fieldType);
                self.validators = ko.observableArray();
                self.options = ko.observableArray();

                self.validatorList = ko.computed(function() {
                    var names = $.map(self.validators(), function(val, index) {
                        return val.name;
                    });
                    return names.join();
                });

                self.removeValidator = function(validator) {
                    self.validators.remove(validator);
                };

                self.removeOption = function(option) {
                    self.options.remove(option);
                };
            };

            commencement.Survey = new function() {
                var self = this;

                // list of questions
                self.questions = ko.observableArray();

                // list of lookups
                self.fieldTypes = ko.observableArray(fieldTypes);
                self.validators = ko.observableArray(validators);

                // fields for new question
                self.prompt = ko.observable();
                self.fieldType = ko.observable();

                // fields for new validator
                self.validatorQuestion = ko.observable();
                self.selectedValidator = ko.observable();

                // fields for new option
                self.optionQuestion = ko.observable();
                self.newOption = ko.observable();

                // add question to table
                self.addQuestion = function() {
                    self.questions.push(new commencement.Question(self.prompt(), self.fieldType(), -1));
                    self.prompt('');
                        
                    $('#new-prompt').focus();
                };

                // remove question from table
                self.removeQuestion = function(question) {
                    self.questions.remove(question);
                };

                // show the validators pop up
                self.popValidators = function(question) {
                    self.validatorQuestion(question);
                    $('#modal-validator').dialog('open');
                };

                // add the validator
                self.addValidator = function() {
                    var validator = self.selectedValidator();
                    self.validatorQuestion().validators.push(validator);

                    $('#validators').focus();
                };

                self.popOptions = function(question) {
                    self.optionQuestion(question);
                    $('#modal-option').dialog('open');
                };

                self.addOption = function() {
                    self.optionQuestion().options.push(self.newOption());
                    self.newOption('');

                    $('#newoption').focus();
                };
            };
        }

        function configureModals() {
            $('#modal-validator').dialog({
                autoOpen: false, modal: true,
                close: function() { $('#new-prompt').focus(); }
            });
            $('#modal-option').dialog({ autoOpen: false, modal: true, close: function() {
                $('#new-prompt').focus();
            }
            });    
        }
        
        function configureLoad() {
            if (questions.length > 0) {
                for (var i = 0; i < questions.length; i++) {

                    var ft;
                    for(var j = 0; j < commencement.Survey.fieldTypes().length; j++)
                    {
                        if (commencement.Survey.fieldTypes()[j].id == questions[i].fieldtype) {
                            ft = commencement.Survey.fieldTypes()[j];
                        }
                    }

                    var question = new commencement.Question(questions[i].prompt, ft, questions[i].id);

                    if (questions[i].validators.length > 0) {
                        for (var j = 0; j < questions[i].validators.length; j++) {
                            
                            for(var k = 0; k < commencement.Survey.validators().length; k++)
                            {
                                if (commencement.Survey.validators()[k].id == questions[i].validators[j].id) {
                                    question.validators.push(commencement.Survey.validators()[k]);
                                }
                            }
                        }
                    }
                    
                    if (questions[i].options.length > 0) {
                        
                        for (var j = 0; j < questions[i].options.length; j++) {
                            question.options.push(questions[i].options[j].name);
                        }

                    }
                    
                    commencement.Survey.questions.push(question);
                }
            }
        }

    }(window.Commencement = window.commencement || {}, jQuery));

    Commencement.options({
        postUrl: '<%: Url.Action("Create") %>',
        redirectUrl: '<%: Url.Action("Index") %>'
    });

    $(function() {
        Commencement.init();
        $('form').validate();

        $('#form-submit').click(function(e) {
            e.preventDefault();

            var questions = Commencement.Survey.questions();

            var flag = true;
            $.each(questions, function(index, item) {
                if (item.fieldType().hasOptions && item.options().length == 0) {
                    flag = false;
                }
            });
                
            if (flag) {
                $(this).parents('form').submit();
            } else {
                alert("There are one or more questions with that require options but do not have any specified.");
            }

        });
    });

</script>