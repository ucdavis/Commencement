<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<div id="modal-validator" title="Select Validator(s)">
        
    <select id="validators" data-bind="value: selectedValidator, options: validators, optionsText: 'name'"></select>
    <button class="buttons" data-bind="click: addValidator">Add</button>

</div>
    
<div id="modal-option" title="Add Option">
        
    <input id="newoption" type="text" data-bind="value: newOption"/>
    <button class="buttons" data-bind="click: addOption">Add</button>

</div>