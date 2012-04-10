<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Commencement.Core.Domain.RegistrationParticipation>" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>

<div class="ceremony ui-corner-all">

    <div class="title ui-widget-header ui-corner-top"><%: string.Format("{0} ({1})", Model.Ceremony.Name, Model.Ceremony.DateTime) %></div>

    <ul class="registration_form">
        <li><strong>Status:</strong><%: Model.Cancelled ? "Cancelled" : "Registered" %></li>
        <li><strong>Major:</strong><%: Model.Major.Name %></li>
        <li><strong>Tickets Requested:</strong><span><%= Html.Encode(Model.NumberTickets) %></span>
        </li>
        <li>
            <strong>Ticket Distribution:</strong>
            <span><%= Html.Encode(Model.TicketDistributionMethod != null ? Model.TicketDistributionMethod.Name : "n/a") %></span>
        </li>
        <% if (Model.ExtraTicketPetition != null) { %>
                <% if (!Model.ExtraTicketPetition.IsPending && Model.ExtraTicketPetition.IsApproved) { %>
                    <li><strong># Extra Tickets Approved:</strong>
                    <%: Model.ExtraTicketPetition.NumberTickets %></li>
                    <li><strong># Streaming Tickets Approved:</strong>
                    <%: Model.ExtraTicketPetition.NumberTicketsStreaming %></li>
                <% } else { %>
                    <li><strong># Extra Tickets Requested:</strong>
                    <%: Model.ExtraTicketPetition.TotalTicketsRequested %></li>
                <% } %>

                <li><strong>Reason:</strong><%: Model.ExtraTicketPetition.Reason %></li>
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

        <li><strong>Date Registered:</strong></li>
        <li><strong>Last Update:</strong></li>
    </ul>

    <div class="foot ui-corner-bottom">
        <span>Registered: <%: string.Format("{0:MM/dd/yyyy hh:mm tt}", Model.DateRegistered) %></span>
        <span style="float: right;">Last Update: <%: string.Format("{0:MM/dd/yyyy hh:mm tt}", Model.DateUpdated) %></span>
    </div>

</div>
    
