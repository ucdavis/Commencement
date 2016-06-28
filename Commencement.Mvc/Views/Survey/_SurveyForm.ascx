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
                <th align="center">Type</th>
                <th></th>
            </tr>
        </thead>
        <tbody data-bind="foreach: questions">
            <tr>
                <td>
                    <input type="hidden" data-bind="value:id, attr: {'name': 'questions[' + $index() + '].Id'}"/>
                    <textarea data-bind="value: prompt, attr: {'name': 'questions[' + $index() + '].Prompt'}" class="required" style="width: 575px; height: 80px;"></textarea>
                    <span class="field-validation-valid" data-bind="attr: { 'data-valmsg-for': 'questions[' + $index() + '].Prompt' }" data-valmsg-replace="true"></span>
                </td>
                <td align="center">
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
                        <li>
                            <button class="buttons" data-bind="click: $parent.removeOption">X</button>
                            <span data-bind="text: $data"></span>
                            <input type="hidden" data-bind="value: $data, attr: {'name': 'questions['+ $parentContext.$index() +'].Options['+$index()+']'}"/>
                        </li>
                    </ul>
                </td>
            </tr>
            <tr class="validator-row" data-bind="visible: validators().length > 0">
                <td colspan="3">
                    <ul data-bind="foreach:validators">
                        <li>
                            <button class="buttons" data-bind="click: $parent.removeValidator">X</button>
                            <span data-bind="text: name"></span>
                            <input type="hidden" data-bind="value: id, attr: {'name': 'questions['+ $parentContext.$index() +'].ValidatorIds['+$index()+']'}"/>
                        </li>
                    </ul>
                </td>
            </tr>
            <tr class="bordered-row"><td colspan="3"><hr /></td></tr>
        </tbody>
        <tfoot>
        </tfoot>
    </table>            

</fieldset>
        
<fieldset>
    <legend>New Question</legend>
    <table>
    <tr>
        <td><textarea id="new-prompt" data-bind="value: prompt" style="width: 575px; height: 80px;"></textarea></td>
        <td><select id="fieldType" data-bind="value: fieldType, options: fieldTypes, optionsText: 'name'"></select></td>
        <td><button class="buttons" data-bind="click: addQuestion">Add</button></td>
    </tr>
    </table>
</fieldset>

<fieldset>
    <ul class="registration_form">
        <li><strong>&nbsp;</strong>
            <input id="form-submit" type="submit" class="buttons" value="Save Survey"/>
            <%: Html.ActionLink("Cancel", "Index") %>
        </li>
    </ul>
</fieldset>

