<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Commencement.Controllers.SurveyCreateViewModel>" %>

<fieldset>
            
    <legend>Settings</legend>
            
    <ul class="registration_form">
        <li><strong>Name<span>*</span></strong>
            <input type="text" id="name" name="name" class="required" maxlength="50" value="<%: Model.Survey.Name %>"/>
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
                    <input type="hidden" data-bind="value:id, attr: {'name': 'questions[' + $index() + '].Id'}"/>
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

