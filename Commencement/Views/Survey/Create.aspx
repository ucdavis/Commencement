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

            <table>
                <tbody data-bind="foreach: Questions">
                    <tr>
                        <td><input type="text" data-bind="value: prompt"/></td>
                        <td>
                            <span data-bind="text: FieldType().name"></span>
                            <input type="hidden" data-bind="value: FieldType().id"/>
                        </td>
                        <td>
                            <a href="#" class="button" data-bind="click: $root.removeQuestion">Remove</a>
                        </td>
                    </tr>
                </tbody>
            </table>            
            
            <div id="newQ">
                
                <table>
                    <tr>
                        <td><input type="text" data-bind="value: prompt"/></td>
                        <td><select id="fieldType" data-bind="value: fieldType, options: FieldTypes, optionsText: 'name'"></select></td>
                        <td><a href="#" class="button" data-bind="click: addQuestion">Add</a></td>
                    </tr>
                </table>

            </div>

        </fieldset>

    <% }%>

    
    <select id="validator" data-bind="options: Validators, optionsText: 'name'"></select>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    
    <script type="text/javascript" src="<%: Url.Content("~/Scripts/knockout-2.2.0.js") %>"></script>
    
    <script type="text/javascript">

        var fieldTypes = <%= new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(Model.SurveyFieldTypes.Select(a => new {id=a.Id, name=a.Name})) %>;
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

                commencement.FieldType = function(id, name) {
                    var self = this;

                    self.Id = ko.observable(id);
                    self.name = ko.observable(name);
                };

                commencement.Validator = function(id, name) {
                    var self = this;

                    self.Id = ko.observable(id);
                    self.name = ko.observable(name);
                };

                commencement.Question = function(prompt, fieldType, validator) {
                    var self = this;

                    self.prompt = ko.observable(prompt);
                    self.FieldType = ko.observable(fieldType);
                    self.Validator = ko.observable(validator);
                };

                commencement.Survey = function() {
                    var self = this;

                    self.Questions = ko.observableArray();
                    self.FieldTypes = ko.observableArray(fieldTypes);
                    self.Validators = ko.observableArray(validators);

                    self.prompt = ko.observable();
                    self.fieldType = ko.observable();

                    self.addQuestion = function() {
                        self.Questions.push(new commencement.Question(self.prompt(), self.fieldType()));
                    };


                    self.removeQuestion = function(question) {
                        self.Questions.remove(question);
                    };
                };
            }

        }(window.Commencement = window.commencement || {}, jQuery));

        $(function() {
            Commencement.init();
        });

    </script>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>
