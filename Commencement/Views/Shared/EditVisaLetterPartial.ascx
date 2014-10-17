﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Commencement.Core.Domain.VisaLetter>" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>
<%@ Import Namespace="Commencement.Resources" %>

<%--look at Edit Student for examples--%>

    <%: Html.ValidationSummary(true) %>

    <% using (Html.BeginForm()) { %>
    <%: Html.AntiForgeryToken() %>


    <ul class="registration_form">
        <li><strong>Student Id:</strong>
            <%: Model.Student.StudentId %>
            <%: Html.HiddenFor(a=>a.Student.StudentId) %>
            <%: Html.HiddenFor(a=>a.Student.Pidm) %>
        </li>
        <li><strong>Email:</strong>
            <%: Model.Student.Email %>
        </li>
        <li><strong>First Name:</strong>
            <%if (ViewBag.AllowChange != null && ViewBag.AllowChange == false) {%>
                <%: Html.TextBoxFor(a=>a.StudentFirstName, new{ @disabled = "disabled" }) %>
            <% } else { %>
                <%: Html.TextBoxFor(a=>a.StudentFirstName) %>
            <% } %>
            <%= Html.ValidationMessageFor(a=>a.StudentFirstName) %>
        </li>
      
        <li><strong>Last Name:</strong>
            <%if (ViewBag.AllowChange != null && ViewBag.AllowChange == false) {%>
                <%: Html.TextBoxFor(a=>a.StudentLastName, new{ @disabled = "disabled" }) %>
            <% } else { %>
                <%: Html.TextBoxFor(a=>a.StudentLastName) %>
            <% } %>
            <%= Html.ValidationMessageFor(a=>a.StudentLastName) %>
        </li>
        
        <li><strong>College Name:</strong>
            <%if (ViewBag.AllowChange != null && ViewBag.AllowChange == false) {%>
                <%: Model.CollegeName %>
                <%: Html.HiddenFor(a => a.CollegeCode, new{id = "doNotChosen"}) %>
            <% } else { %>
                <%: Html.DropDownListFor(a=>a.CollegeCode, SelectLists.CollegeNames) %>
            <% } %>
            
            <%= Html.ValidationMessageFor(a=>a.CollegeCode) %>
        </li>
        
        <li><strong>Major Name:</strong>
            <%if (ViewBag.AllowChange != null && ViewBag.AllowChange == false) {%>
                <%: Html.TextBoxFor(a=>a.MajorName, new{ @disabled = "disabled" }) %>
            <% } else { %>
                <%: Html.TextBoxFor(a=>a.MajorName) %>
            <% } %>
            <%= Html.ValidationMessageFor(a=>a.MajorName) %>
        </li>

        <li><strong>Your Gender:</strong>
            <%if (ViewBag.AllowChange != null && ViewBag.AllowChange == false) {%>
                <label><%: Html.RadioButton("Gender", 'M', new{ @disabled = "disabled" })%> Male</label>
                <label><%: Html.RadioButton("Gender", 'F', new{ @disabled = "disabled" })%> Female</label>
            <% } else { %>
                <label><%: Html.RadioButton("Gender", 'M')%> Male</label>
                <label><%: Html.RadioButton("Gender", 'F')%> Female</label>
            <% } %>
                        

            
            <%= Html.ValidationMessageFor(a=>a.Gender) %>
        </li>
        
                <li><strong>Ceremony:</strong>
            
                <label><%: Html.RadioButton("Ceremony", 'S')%> Spring</label>
                <label><%: Html.RadioButton("Ceremony", 'F')%> Fall</label>
            
            <%= Html.ValidationMessageFor(a=>a.Ceremony) %>
        </li>
        
        <li><strong>Relative's Title:</strong>
            <%: Html.DropDownListFor(a=>a.RelativeTitle, SelectLists.PersonPrefixes) %>
            <%= Html.ValidationMessageFor(a=>a.RelativeTitle) %>
        </li>
        
        <li><strong>Relative's First Name:</strong>
            <%: Html.TextBoxFor(a=>a.RelativeFirstName) %>
            <%= Html.ValidationMessageFor(a=>a.RelativeFirstName) %>
        </li>
        
        <li><strong>Relative's Last Name:</strong>
            <%: Html.TextBoxFor(a=>a.RelativeLastName) %>
            <%= Html.ValidationMessageFor(a=>a.RelativeLastName) %>
        </li>
        
        <li><strong>Relationship To You:</strong>
            <%: Html.TextBoxFor(a=>a.RelationshipToStudent) %>
            <%= Html.ValidationMessageFor(a=>a.RelationshipToStudent) %>
        </li>

        <li><strong>Relative's Mailing Address:</strong>
            <%: Html.TextAreaFor(a=>a.RelativeMailingAddress) %>
            <%= Html.ValidationMessageFor(a=>a.RelativeMailingAddress) %>
        </li>
        <li><strong>&nbsp;</strong>
            <%: Html.SubmitButton("Submit", "Save", new { @class="button" })%>
        </li>
        </ul>   
    <% } %>
    
    <link href="<%: Url.Content("~/Content/chosen.css") %>" rel="stylesheet" type="text/css"/>
    <script type="text/javascript" src="<%: Url.Content("~/Scripts/jquery.chosen.min.js") %>"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            
            $("#CollegeCode").chosen({ no_results_text: "No results matched" }); //TODO: Test this.
            $('#RelativeTitle').chosen({ no_results_text: "No results matched" }); //TODO: Test this.
           
        });
    </script>

    <style type="text/css">
        #Majors {display : inline-block;}
    </style>