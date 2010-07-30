<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Commencement.Core.Domain.Student>" %>

    <h2>Student Information</h2>
    <ul class="registration_form">
        <li class="prefilled"><strong>Name:</strong> <span><%= Html.Encode(Model.FullName) %></span>
        </li>
        <li class="prefilled"><strong>Student ID:</strong> <span><%= Html.Encode(Model.StudentId) %></span> </li>
        <li class="prefilled"><strong>Units Complted:</strong> <span><%= Html.Encode(Model.Units) %></span> </li>
        <li class="prefilled">
            <strong>Major:</strong>
            <span>
                <%= Html.Encode(Model.StrMajors)%>
            </span>
        </li>
    </ul>