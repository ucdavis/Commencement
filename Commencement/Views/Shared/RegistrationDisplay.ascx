<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Commencement.Core.Domain.Registration>" %>
<h2>
    Student Information</h2>
<ul class="registration_form">
    <li class="prefilled"><strong>Name:</strong><span><%= Html.Encode(Model.Student.FullName) %></span>
    </li>
    <li class="prefilled"><strong>Student ID:</strong><span><%= Html.Encode(Model.Student.StudentId) %></span>
    </li>
    <li class="prefilled"><strong>Units Complted:</strong><span><%= Html.Encode(Model.Student.Units) %></span>
    </li>
    <li class="prefilled"><strong>Major:</strong><span><%= Html.Encode(Model.Major.Name)%></span></li>
</ul>
<h2>
    Contact Information</h2>
<ul class="registration_form">
    <li><strong>Address Line 1:</strong><span><%= Html.Encode(Model.Address1) %></span>
    </li>
    <li><strong>Address Line 2:</strong><span><%= Html.Encode(Model.Address2) %></span>
    </li>
    <li><strong>City:</strong><span><%= Html.Encode(Model.City) %></span> </li>
    <li><strong>State:</strong><span><%= Html.Encode(Model.State) %></span> </li>
    <li><strong>Zip Code:</strong><span><%= Html.Encode(Model.Zip) %></span> </li>
    <li class="prefilled"><strong>Email Address:</strong> <span>
        <%= Html.Encode(Model.Student.Email) %></span> </li>
    <% if (!string.IsNullOrEmpty(Model.Email))
       { %>
    <li><strong>Secondary Email Address:</strong><span><%= Html.Encode(Model.Email)%></span>
    </li>
    <% } %>
</ul>
<h2>
    Ceremony Information</h2>
<ul class="registration_form">
    <li><strong>Tickets Requested:</strong><span><%= Html.Encode(Model.TotalTickets) %></span>
    </li>
    <li>
        <strong>Ticket Distribution Method:</strong>
        <span><%= Html.Encode(Model.TicketDistributionMethod) %></span>
    </li>
    <li class="prefilled"><strong>Ceremony Date:</strong> <span>
        <%= Html.Encode(string.Format("{0}", Model.Ceremony.DateTime.ToString("g"))) %></span> </li>
        <li class="prefilled"><strong>Special Requests:</strong>
            <%= Html.Encode(Model.Comments) %>
        </li>
</ul>

<% if (Model.ExtraTicketPetition != null) { %>
    <h2>Extra Ticket Petition</h2>
    <ul class="registration_form">
        <li><strong>Number of Extra Tickets Requested:</strong><%= Html.Encode(Model.ExtraTicketPetition.NumberTickets) %></li>
        <li><strong>Status:</strong>
            <%= Html.Encode(Model.ExtraTicketPetition.IsPending ? "Pending Decision" : (Model.ExtraTicketPetition.IsApproved ? "Approved" : "Denied")) %>
        </li>
    </ul>
<% } %>