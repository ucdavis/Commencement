<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Core.Domain.TransferRequest>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Review Transfer Request
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Review Transfer Request</h2>
    
    <ul class="registration_form">
        <li class="prefilled">
            <strong>Student Id</strong>
            <%: Model.RegistrationParticipation.Registration.Student.StudentId %>
        </li>
        <li class="prefilled">
            <strong>Name</strong>
            <%: Model.RegistrationParticipation.Registration.Student.FullName %>
        </li>
        <li class="prefilled">
            <strong>Registered Major</strong>
            <%: Model.RegistrationParticipation.Major.MajorName %>
        </li>
        <li class="prefilled">
            <strong>Registered Ceremony</strong>
            <%: string.Format("{0} ({1})", Model.RegistrationParticipation.Ceremony.CeremonyName, Model.RegistrationParticipation.Ceremony.DateTime) %>
        </li>
        
        <li class="prefilled">
            <strong>Requested Ceremony</strong>
            <%: string.Format("{0} ({1})", Model.Ceremony.CeremonyName, Model.Ceremony.DateTime) %>
        </li>
        <li class="prefilled">
            <strong>Reason</strong>
            <%: Model.Reason %>
        </li>
    
        <li class="prefilled">
            <strong>Tickets Requested</strong>
            <%: Model.RegistrationParticipation.NumberTickets %>
        </li>
        
        <% if (Model.RegistrationParticipation.ExtraTicketPetition != null) { %>
            
            <li class="prefilled">
                <% if (Model.RegistrationParticipation.ExtraTicketPetition.IsPending) { %>
                    <strong>Extra Tickets Requested</strong>
                    <span class="tickets"><strong>Pavilion:</strong> <%: Model.RegistrationParticipation.ExtraTicketPetition.NumberTicketsRequested %></span>
                    <% if (Model.RegistrationParticipation.ExtraTicketPetition.NumberTicketsRequestedStreaming > 0) { %>
                        <span class="tickets"><strong>Ballroom:</strong> <%: Model.RegistrationParticipation.ExtraTicketPetition.NumberTicketsRequestedStreaming %></span>
                    <% } %>
                    
                <% } else { %>
                    <strong>Extra Tickets Approved</strong>
                    <span class="tickets"><strong>Pavilion:</strong> <%: Model.RegistrationParticipation.ExtraTicketPetition.NumberTickets %></span>
                    <% if (Model.RegistrationParticipation.ExtraTicketPetition.NumberTicketsStreaming > 0) { %>
                        <span class="tickets"><strong>Ballroom:</strong> <%: Model.RegistrationParticipation.ExtraTicketPetition.NumberTicketsStreaming %></span>
                    <% } %>
                <% } %>
                
            </li>

        <% } %>
        
        <li>
            <strong></strong>
<%--            <% using(Html.BeginForm("Review", "TransferRequest", new {Approved = true}, FormMethod.Post, new { style="display: inline;"})) { %>
                <%: Html.AntiForgeryToken() %>
                <input type="submit" value="Approve" class="button"/>
            <% } %>
            <% using(Html.BeginForm("Review", "TransferRequest", new {Approved = false}, FormMethod.Post, new { style="display: inline;"})) { %>
                <%: Html.AntiForgeryToken() %>
                <input type="submit" value="Deny" class="button"/>
            <% } %>--%>
            |
            <%: Html.ActionLink("Cancel", "Index") %>
        </li>
    </ul>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    
    <style type="text/css">
        .registration_form .tickets strong { width: 75px; }
        .tickets { margin-right: 20px; }
    </style>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>
