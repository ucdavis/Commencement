<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.SurveyViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%: Model.Survey.Name %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <% var name = "answers[{0}].Answer"; %>

    <h2><%: Model.Survey.Name %></h2>
    
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

    <% using (Html.BeginForm()) { %>
    
    <%= Html.AntiForgeryToken() %>

    <div id="survey-container">
    <% for (var i = 0; i < Model.Survey.SurveyFields.Count; i++) {
           var question = Model.Survey.SurveyFields[i];
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
                            <input type="checkbox" name="<%: string.Format(name, i) %>" value="<%:o.Name %>" <%: answer == o.Name ? "checked=\"checked\"" : string.Empty %> />
                            <%: o.Name %> 
                        </label>
                   <% } %>

                <% break;
                   case "drop down" : %>
                   
                   <select name="<%: string.Format(name, i) %>" class="<%: question.ValidationClasses %>">
                       <% foreach(var o in question.SurveyFieldOptions) { %>
                            <option value="<%: o.Name %>" <%: answer == o.Name ? "selected" : string.Empty %>><%: o.Name %></option>
                       <% } %>
                   </select>

                <% break;
                   case "date" : %>
                   
                    <input type="text" class="date" name="<%: string.Format(name, i) %>" value="<%: answer %>"/>

                <% break;
                   case "boolean/other" : %>
                   
                   <label class="radio"><%: Html.RadioButton(string.Format(name, i), "Yes", answer == "Yes", new {@class="boolother"}) %> Yes</label>
                   <label class="radio"><%: Html.RadioButton(string.Format(name, i), "No", answer == "No", new { @class = "boolother" })%> No</label>

                   <% foreach (var o in question.SurveyFieldOptions) { %>
                        <div class="option"><%: o.Name %></div>
                        <input type="text" name="<%: string.Format(name, i) %>" class="boolother" value="<%: answer != "Yes" && answer != "No" ? answer : string.Empty %>"/>
                   <% } %>
                <% break; %>
            <% } %>

        </div>

    <% } %>
    
    </div>
    
    <hr />
    <input type="submit" value="Submit Survey" class="button" style="margin-top: 1em;"/>

    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    
    <script type="text/javascript">
        $(function () {
            //$('form').validate();

            $('.boolother').change(function () {
                var name = $(this).attr('name');
                var type = $(this).attr('type');

                var controls = $('input[name="' + name + '"]');

                // the textbox is being used, clear the radio's
                if (type == 'text') {
                    $.each(controls, function (index, item) {
                        if ($(item).attr('type') == 'radio') {
                            $(item).attr('checked', false);
                        }
                    });
                } else {
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
        
        label.radio { display: block;}
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
    </style>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>
