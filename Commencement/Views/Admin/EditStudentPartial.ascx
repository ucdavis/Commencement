﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Commencement.Controllers.ViewModels.AdminEditStudentViewModel>" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>

    <%: Html.ValidationSummary() %>

    <% using (Html.BeginForm()) { %>
    <%: Html.AntiForgeryToken() %>


    <ul class="registration_form">
        <li><strong>Student Id:</strong>
            <%: Model.Student.StudentId %>
            <%: Html.HiddenFor(a=>a.Student.StudentId) %>
            <%: Html.HiddenFor(a=>a.Student.Pidm) %>
        </li>
        <li><strong>First Name:</strong>
            <%: Html.TextBoxFor(a=>a.Student.FirstName) %>
            <%: Html.ValidationMessageFor(a=>a.Student.FirstName) %>
        </li>
        <li><strong>MI:</strong>
            <%: Html.TextBoxFor(a=>a.Student.MI) %>
            <%: Html.ValidationMessageFor(a=>a.Student.MI) %>
        </li>        
        <li><strong>Last Name:</strong>
            <%: Html.TextBoxFor(a=>a.Student.LastName) %>
            <%: Html.ValidationMessageFor(a=>a.Student.LastName) %>
        </li>
        <li><strong>Earned Units:</strong>
            <%: Html.TextBoxFor(a=>a.Student.EarnedUnits) %>
            <%: Html.ValidationMessageFor(a=>a.Student.EarnedUnits) %>
        </li>
        <li><strong>Current Units:</strong>
            <%: Html.TextBoxFor(a=>a.Student.CurrentUnits) %>
            <%: Html.ValidationMessageFor(a=>a.Student.CurrentUnits) %>
        </li>
        <li><strong>Email:</strong>
            <%: Html.TextBoxFor(a=>a.Student.Email) %>
            <%: Html.ValidationMessageFor(a=>a.Student.Email) %>
        </li>
        <li><strong>Login:</strong>
            <%: Html.TextBoxFor(a=>a.Student.Login) %>
            <%: Html.ValidationMessageFor(a=>a.Student.Login) %>
        </li>
    </ul>

    Add Major: <%= this.Select("AddMajorDropDown").Options(Model.Majors, x=>x.Id, x=>x.Name).FirstOption("--Select a Major--") %> <%: Html.Button("AddMajor", "+", HtmlButtonType.Button, null, new {@class="AddMajor"}) %>
    <% Html.Grid(Model.Student.Majors)
           .Name("Majors")
           .Columns(col=>
                        {
                            col.Add(a =>
                                        {%>
                                            <%: Html.Button("RemoveMajor", "-", HtmlButtonType.Button, null, new {@class="RemoveMajor"}) %>
                                            <%: Html.Hidden("Student.Majors", a.Id) %>
                                        <%});
                            col.Bound(a => a.Major.Id).Title("Major Code");
                            col.Bound(a => a.Major.Name).Title("Major Name");
                        })
            .Render();
           %>

    <%: Html.SubmitButton("Submit", "Save") %>

    <% } %>





    <script type="text/javascript">
        $(document).ready(function () {
            $(".AddMajor").click(function () {
                var selectedOption = $("#AddMajorDropDown option:selected");

                var tr = $("<tr>");
                var cell1 = $("<td>");
                var cell2 = $("<td>");
                var cell3 = $("<td>");

                cell1.append($("<button>").attr("type", "button").text("-").attr("name", "RemoveMajor").addClass("RemoveMajor"));
                cell1.append($("<input>").attr("type", "hidden").attr("id", "Student_Majors").attr("name", "Student.Majors").val(selectedOption.val()));

                cell2.html(selectedOption.val());
                cell3.html(selectedOption[0].text).addClass("t-last");

                tr.append(cell1).append(cell2).append(cell3);
                if ($("#Majors table tbody tr").length % 2 == 1) tr.addClass("t-alt");
                $("#Majors table tbody").append(tr);
            });
            $(".RemoveMajor").live('click', function () {
                var count = $("#Majors table tbody tr").length;

                if (count > 1) {    // include header row in the count
                    $(this).parents("tr").remove();
                }
                else {
                    alert("Student must have at least one major.  You cannot remove all majors.");
                }
            });
        });
    </script>