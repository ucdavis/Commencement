<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.SurveyCreateViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create Survey
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Create Survey</h2>
    
    <% using(Html.BeginForm()) { %>
        <%= Html.AntiForgeryToken() %>
        
        <fieldset>
            
            <legend>Settings</legend>
            
            <ul class="registration_form">
                <li><strong>Name<span>*</span></strong>
                    <%: Html.TextBoxFor(model => model.Survey.Name) %>
                </li>
            </ul>
            
        </fieldset>
        
        <fieldset>
            
            <legend>Questions</legend>

            <table style="text-align: left;">
                <thead>
                    <tr>
                        <th>Prompt</th>
                        <th>Type</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody data-bind="foreach: questions">
                    <tr>
                        <td><input type="text" data-bind="value: prompt"/></td>
                        <td>
                            <span data-bind="text: fieldType().name"></span>
                            <input type="hidden" data-bind="value: fieldType().id"/>
                        </td>
                        <td>
                            <button class="buttons" data-bind="click: $root.popValidators">+ Validator</button>
                            <button class="buttons" data-bind="click: $root.popOptions, visible: fieldType().hasOptions">+ Option</button>
                            <button class="buttons" data-bind="click: $root.removeQuestion">Remove</button>
                        </td>
                    </tr>
                    <tr class="option-row" data-bind="visible: options().length > 0">
                        <td colspan="3">
                            <ul data-bind="foreach:options">
                                <li><span data-bind="text: $data"></span>
                                    <button class="buttons" data-bind="click: $parent.removeOption">X</button>
                                </li>
                            </ul>
                        </td>
                    </tr>
                    <tr class="validator-row" data-bind="visible: validators().length > 0">
                        <td colspan="3">
                            <ul data-bind="foreach:validators">
                                <li><span data-bind="text: name"></span>
                                    <button class="buttons" data-bind="click: $parent.removeValidator">X</button>
                                </li>
                            </ul>
                        </td>
                    </tr>
                </tbody>
                <tfoot>
                    <tr>
                        <td><input type="text" data-bind="value: prompt"/></td>
                        <td><select id="fieldType" data-bind="value: fieldType, options: fieldTypes, optionsText: 'name'"></select></td>
                        <td><button class="buttons" data-bind="click: addQuestion">Add</button></td>
                    </tr>
                </tfoot>
            </table>            

        </fieldset>

    <% }%>
    
    <div id="modal-validator" title="Select Validator(s)">
        
        <select id="validators" data-bind="value: selectedValidator, options: validators, optionsText: 'name'"></select>
        <button class="buttons" data-bind="click: addValidator">Add</button>

    </div>
    
    <div id="modal-option" title="Add Option">
        
        <input type="text" data-bind="value: newOption"/>
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
                ko.applyBindings(new commencement.Survey());
            };

            function createModels() {

//                commencement.FieldType = function(id, name, hasOptions) {
//                    var self = this;

//                    self.id = id;
//                    self.name = name;
//                    self.hasOptions = hasOptions;
//                };

//                commencement.Validator = function(id, name) {
//                    var self = this;

//                    self.id = id;
//                    self.name = name;
//                };

//                commencement.options = function(id, name) {
//                    var self = this;

//                    self.id = id;
//                    self.name = name;
//                };

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

                commencement.Survey = function() {
                    var self = this;

                    self.questions = ko.observableArray();
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
                    };

                    self.popOptions = function(question) {
                        self.optionQuestion(question);
                        $('#modal-option').dialog('open');
                    };

                    self.addOption = function() {
                        self.optionQuestion().options.push(self.newOption());
                        self.newOption('');
                    };
                };
            }

        }(window.Commencement = window.commencement || {}, jQuery));

        $(function() {
            Commencement.init();

            $('#modal-validator').dialog({autoOpen: false, modal: true});
            $('#modal-option').dialog({ autoOpen: false, modal: true });
        });

    </script>

    <style type="text/css">
        .buttons { padding: .5em 1em;}
    </style>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>
