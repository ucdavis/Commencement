<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Commencement.Core.Domain.Student>" %>


<ul class="registration_form">
    <li class="prefilled"><strong>Name:</strong><span><%= Html.Encode(Model.FullName) %></span>
    </li>
    <li class="prefilled"><strong>Student ID:</strong><span><%= Html.Encode(Model.StudentId) %></span>
    </li>
    <li class="prefilled"><strong>Units Completed:</strong><span><%= Html.Encode(Model.TotalUnits) %></span>
    </li>
    <li class="prefilled"><strong>Major:</strong><span><%: Model.StrMajors %></span>
    </li>
</ul>