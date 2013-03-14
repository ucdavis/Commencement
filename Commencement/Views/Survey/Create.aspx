<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.SurveyCreateViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create Survey
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Create Survey</h2>
    
    <%= Html.ValidationSummary("Please correct all errors below") %>

    <% using (Html.BeginForm("Create", "Survey", FormMethod.Post)) { %>
        
        <%= Html.AntiForgeryToken() %>
        
        <fieldset>
            
            <legend>Settings</legend>
            
            <ul class="registration_form">
                <li><strong>Name<span>*</span></strong>
                    <input type="text" id="name" name="name" class="required" maxlength="50"/>
                </li>
            </ul>
            
        </fieldset>
        
        <fieldset>
            
            <legend>Questions</legend>

            <table style="text-align: left; width: 100%;">
                <thead>
                    <tr>
                        <th>Prompt</th>
                        <th>Type</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody data-bind="foreach: questions">
                    <tr>
                        <td>
                            <textarea data-bind="value: prompt, attr: {'name': 'questions[' + $index() + '].Prompt'}" class="required" style="width: 575px; height: 80px;"></textarea>
                        </td>
                        <td>
                            <span data-bind="text: fieldType().name"></span>
                            <input type="hidden" data-bind="value: fieldType().id, attr: {'name': 'questions[' + $index() + '].FieldTypeId'}"/>
                        </td>
                        <td>
                            <button class="buttons" data-bind="click: $root.popValidators">Validator</button>
                            <button class="buttons" data-bind="click: $root.popOptions, visible: fieldType().hasOptions">Option</button>
                            <button class="buttons" data-bind="click: $root.removeQuestion">X</button>
                        </td>
                    </tr>
                    <tr class="option-row" data-bind="visible: fieldType().hasOptions">
                        <td colspan="3">
                            <span class="warning" data-bind="visible: options().length == 0">This question requires one or more options specified.</span>
                            <ul data-bind="foreach:options">
                                <li><span data-bind="text: $data"></span>
                                    <input type="hidden" data-bind="value: $data, attr: {'name': 'questions['+ $parentContext.$index() +'].Options['+$index()+']'}"/>
                                    <button class="buttons" data-bind="click: $parent.removeOption">X</button>
                                </li>
                            </ul>
                        </td>
                    </tr>
                    <tr class="validator-row" data-bind="visible: validators().length > 0">
                        <td colspan="3">
                            <ul data-bind="foreach:validators">
                                <li><span data-bind="text: name"></span>
                                    <input type="hidden" data-bind="value: id, attr: {'name': 'questions['+ $parentContext.$index() +'].ValidatorIds['+$index()+']'}"/>
                                    <button class="buttons" data-bind="click: $parent.removeValidator">X</button>
                                </li>
                            </ul>
                        </td>
                    </tr>
                </tbody>
                <tfoot>
                    <tr>
                        <td><textarea id="new-prompt" data-bind="value: prompt" style="width: 575px; height: 80px;"></textarea></td>
                        <td><select id="fieldType" data-bind="value: fieldType, options: fieldTypes, optionsText: 'name'"></select></td>
                        <td><button class="buttons" data-bind="click: addQuestion">Add</button></td>
                    </tr>
                </tfoot>
            </table>            

        </fieldset>
        
        <fieldset>
            <ul class="registration_form">
                <li><strong>&nbsp;</strong>
                    <input id="form-submit" type="submit" class="buttons" value="Create Survey"/>
                    <%: Html.ActionLink("Cancel", "Index") %>
                </li>
            </ul>
        </fieldset>

    <% } %>
    
    <div id="modal-validator" title="Select Validator(s)">
        
        <select id="validators" data-bind="value: selectedValidator, options: validators, optionsText: 'name'"></select>
        <button class="buttons" data-bind="click: addValidator">Add</button>

    </div>
    
    <div id="modal-option" title="Add Option">
        
        <input id="newoption" type="text" data-bind="value: newOption"/>
        <button class="buttons" data-bind="click: addOption">Add</button>

    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    
    <script type="text/javascript" src="<%: Url.Content("~/Scripts/knockout-2.2.0.js") %>"></script>
    
    <script type="text/javascript">

        var fieldTypes = <%= new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(Model.SurveyFieldTypes.Select(a => new {id=a.Id, name=a.Name, hasOptions = a.HasOptions})) %>;
        var validators = <%= new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(Model.SurveyFieldValidators.Select(a => new {id=a.Id, name=a.Name})) %>;

        (function(commencement, $, undefined) {

            "use strict";

            var options = {};

            commencement.options = function(o) { $.extend(options, o); };

            commencement.init = function() {
                createModels();
                ko.applyBindings(commencement.Survey);

                configureModals();
                //configureSave();
            };

            function createModels() {

                commencement.Question = function(prompt, fieldType) {
                    var self = this;

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
                        self.questions.push(new commencement.Question(self.prompt(), self.fieldType()));
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

    <style type="text/css">
        .buttons { padding: .5em 1em;}
        .warning { color: red;}
        .error { margin-left: 7px;color: red;}
        
        .option-row td, .validator-row td { min-height: 50px;background-color: #F2F2F2;padding: .5em; }
        .option-row ul, .validator-row ul { list-style: none; }
        .option-row li, .validator-row li { float: left;display: block;width: 50%; }
    </style>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>