<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Core.Domain.TransferRequest>" %>
<%@ Import Namespace="xVal.RuleProviders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Review Transfer Request
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Review Transfer Request</h2>
    
    <%= Html.ValidationSummary("Please correct all errors below") %>

    <% using(Html.BeginForm()) { %>
    
        <%: Html.AntiForgeryToken() %>

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
                <strong>Requested Major</strong>
                <%: Model.MajorCode.MajorName %>
            </li>

            <li class="prefilled">
                <strong>Tickets Requested</strong>
                <%: Html.TextBox("NumberTickets", Model.RegistrationParticipation.NumberTickets, new { @class = "ticketbox" }) %>
            </li>
        
            <% if (Model.RegistrationParticipation.ExtraTicketPetition != null) { %>
            
                <li class="prefilled">
                    <% if (Model.RegistrationParticipation.ExtraTicketPetition.IsPending) { %>
                        <strong>Extra Tickets Requested</strong>
                        <span class="tickets">
                            <strong>Pavilion:</strong> 
                            <%: Html.TextBox("NumberTicketsRequested", Model.RegistrationParticipation.ExtraTicketPetition.NumberTicketsRequested, new {@class="ticketbox"}) %>
                        </span>
                        <% if (Model.RegistrationParticipation.ExtraTicketPetition.NumberTicketsRequestedStreaming > 0) { %>
                            <span class="tickets">
                                <strong>Ballroom:</strong>
                                <%: Html.TextBox("NumberTicketsRequestedStreaming", Model.RegistrationParticipation.ExtraTicketPetition.NumberTicketsRequestedStreaming, new {@class="ticketbox"}) %>
                            </span>
                        <% } %>
                    
                    <% } else { %>
                        <strong>Extra Tickets Approved</strong>
                        <span class="tickets">
                            <strong>Pavilion:</strong> 
                            <%: Html.TextBox("NumberExtraTickets", Model.RegistrationParticipation.ExtraTicketPetition.NumberTickets, new {@class="ticketbox"}) %>
                        </span>
                        <% if (Model.RegistrationParticipation.ExtraTicketPetition.NumberTicketsStreaming > 0) { %>
                            <span class="tickets">
                                <strong>Ballroom:</strong> 
                                <%: Html.TextBox("NumberExtraTicketsStreaming", Model.RegistrationParticipation.ExtraTicketPetition.NumberTicketsStreaming, new {@class="ticketbox"}) %>
                            </span>
                        <% } %>
                    <% } %>
                
                </li>

            <% } %>
        
            <li>
                <strong>Approve Request</strong>
                <span class="checkbox">
                    <label style="margin-right: 10px;">
                        <input type="radio" name="approved" value="true" class="required"/> Approve
                    </label>
                    <label>
                        <input type="radio" name="approved" value="false" class="required"/> Decline
                    </label>
                </span>
            </li>

            <li>
                <strong></strong>
                    <input type="submit" value="Submit" class="button"/>
                    |
                    <%: Html.ActionLink("Cancel", "Index") %>
            </li>
        </ul>
    <% } %>    

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    
    <style type="text/css">
        .registration_form .tickets strong { width: 75px; }
        .tickets { margin-right: 20px; }
        
        input[type="text"].ticketbox { min-width: 25px;width: 25px; }
    </style>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>
