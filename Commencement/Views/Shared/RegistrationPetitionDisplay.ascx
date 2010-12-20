<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Commencement.Core.Domain.RegistrationPetition>" %>

<ul class="registration_form">
    <li><strong>Status: </strong><%: Model.IsPending ? "Pending" : (Model.IsApproved ? "Approved" : "Denied")%></li>
    <li><strong>Major: </strong><%: Model.MajorCode.MajorName %></li>
    <li><strong>Date Submitted:</strong><%: string.Format("{0:MM/dd/yyyy hh:mm tt}", Model.DateSubmitted) %></li>
    <% if (!Model.IsPending) { %><li><strong>Last Update:</strong><%: string.Format("{0:MM/dd/yyyy hh:mm tt}", Model.DateDecision) %></li><% } %>
    <li><strong>Tickets Requested:</strong><span><%: Model.NumberTickets %></span>
    </li>
    <li class="prefilled"><strong>Ceremony Date:</strong><span><%= Html.Encode(string.Format("{0}", Model.Ceremony.DateTime.ToString("g"))) %></span> </li>
</ul>