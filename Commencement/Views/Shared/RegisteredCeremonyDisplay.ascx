<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Commencement.Core.Domain.RegistrationParticipation>" %>

<ul class="registration_form">
    <li><strong>Major: </strong><%: Model.Major.MajorName %></li>
    <li><strong>Date Registered:</strong><%: string.Format("{0:MM/dd/yyyy hh:mm tt}", Model.DateRegistered) %></li>
    <li><strong>Last Update:</strong><%: string.Format("{0:MM/dd/yyyy hh:mm tt}", Model.DateUpdated) %></li>
    <li><strong>Tickets Requested:</strong><span><%= Html.Encode(Model.NumberTickets) %></span>
    </li>
    <li>
        <strong>Ticket Distribution Method:</strong>
        <span><%= Html.Encode(Model.TicketDistribution) %></span>
    </li>
    <li class="prefilled"><strong>Ceremony Date:</strong> <span>
        <%= Html.Encode(string.Format("{0}", Model.Ceremony.DateTime.ToString("g"))) %></span> </li>
    <% if (Model.ExtraTicketPetition != null) { %>
            <% if (!Model.ExtraTicketPetition.IsPending && Model.ExtraTicketPetition.IsApproved) { %>
                <li><strong># Extra Tickets Approved:</strong>
                <%: Model.ExtraTicketPetition.NumberTickets %></li>
                <li><strong># Streaming Tickets Approved:</strong>
                <%: Model.ExtraTicketPetition.NumberTicketsStreaming %></li>
            <% } else { %>
                <li><strong># Extra Tickets Requested:</strong>
                <%: Model.ExtraTicketPetition.NumberTicketsRequested %></li>
            <% } %>
        <li>
            <strong>Status:</strong>
            <%: Model.ExtraTicketPetition.IsPending ? "Pending Decision" : (Model.ExtraTicketPetition.IsApproved ? "Approved" : "Denied") %>
        </li>
    <% } else { %>
            <li>
                <strong>Extra Tickets:</strong>
                No extra ticket petition has been submitted for this registration.
            </li>
    <% } %>
</ul>