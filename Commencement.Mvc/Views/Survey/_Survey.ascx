<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Commencement.Mvc.Controllers.SurveyViewModel>" %>

<% var name = "answers[{0}].Answer";
   var multiName = "answers[{0}].Answers";
   var questions = Model.Survey.SurveyFields.OrderBy(a => a.Order).ToList();
   %>

<% if (Model.Errors != null && Model.Errors.Any()) { %>
    <div id="error-container">
        <p>The following questions require an answer:</p>
        <ul>
            <% foreach (var e in Model.Errors) { %>
                <li><%: e %></li>    
            <% } %>
        </ul>
    </div>
<% } %>

<div id="survey-container">
<% for (var i = 0; i < questions.Count; i++) {
        var question = questions[i];
        var answer = Model.AnswerPosts != null ? Model.AnswerPosts.First(a => a.QuestionId == question.Id).Answer : string.Empty;
        %>

    <input type="hidden" value="<%: question.Id %>" name="answers[<%:i%>].QuestionId"/>
    
    <% if (question.SurveyFieldType.Name.ToLower() == "group header") { %>
        
        <fieldset>
            <legend><%: question.Prompt %></legend>
                
            <% foreach (var o in question.SurveyFieldOptions) { %>
                <p><%: o.Name %></p>
            <% } %>
                
        </fieldset>

    <% } %>

    <div class="question">
            
        <% if (question.SurveyFieldType.Name.ToLower() != "group header"){ %>
            <div class="prompt">
                <%: question.Prompt %>
                <% if (question.SurveyFieldValidators.Select(a => a.Name.ToLower()).Contains("required")) { %>
                    <span class="required">*</span>
                <% } %>
            </div>
        <% } %>
            
            
        <% switch (question.SurveyFieldType.Name.ToLower()) { %>
            <% case "text box" : %>
                <input type="text" value="<%: answer %>" name="<%: string.Format(name, i) %>" class="<%: question.ValidationClasses %>" />
            <% break;
                case "text area" : %>
                <textarea rows="3" cols="50" name="<%: string.Format(name, i) %>" ><%: answer %></textarea>
            <% break;
                case "boolean" : %>
                   
                <label class="radio"><%: Html.RadioButton(string.Format(name, i), "Yes", answer == "Yes") %> Yes</label>
                <label class="radio"><%: Html.RadioButton(string.Format(name, i), "No", answer == "No") %> No</label>

            <% break;
                case "radio buttons" : %>
                   
                <% foreach (var o in question.SurveyFieldOptions) { %>
                    <label class="radio"><%: Html.RadioButton(string.Format(name, i), o.Name, answer == o.Name) %> <%: o.Name %> </label>
                <% } %>

            <% break;
                case "checkbox list" : %>
                   
                <% foreach (var o in question.SurveyFieldOptions) { %>
                    <label class="checkbox">
                        <input type="checkbox" name="<%: string.Format(multiName, i) %>" value="<%:o.Name %>" <%: answer == o.Name ? "checked=\"checked\"" : string.Empty %> />
                        <%: o.Name %> 
                    </label>
                <% } %>

            <% break;
                case "drop down" : %>
                   
                <select name="<%: string.Format(name, i) %>" class="<%: question.ValidationClasses %>">
                    <option value="">--Select From the List--</option>
                    <% foreach(var o in question.SurveyFieldOptions) { %>
                        <option value="<%: o.Name %>" <%: answer == o.Name ? "selected" : string.Empty %>><%: o.Name %></option>
                    <% } %>
                </select>

            <% break;
                case "date" : %>
                   
                <input type="text" class="date" name="<%: string.Format(name, i) %>" value="<%: answer %>"/>

            <% break;
                case "boolean/other" : %>
                   
                <label class="radio"><%: Html.RadioButton(string.Format(multiName, i), "Yes", answer == "Yes", new {@class="boolother"}) %> Yes</label>
                <label class="radio"><%: Html.RadioButton(string.Format(multiName, i), "No", answer == "No", new { @class = "boolother" })%> No</label>

                <% foreach (var o in question.SurveyFieldOptions) { %>
                    <div class="option"><%: o.Name %></div>
                    <input type="text" name="<%: string.Format(multiName, i) %>" class="boolother" value="<%: answer != "Yes" && answer != "No" ? answer : string.Empty %>"/>
                <% } %>
            <% break; %>
        <% } %>

    </div>

<% } %>
    
</div>
    
<hr />