<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.ExtraTicketPetitionModel>"
    MasterPageFile="~/Views/Shared/Site.Master" %>
<%@ Import Namespace="Commencement.Core.Resources" %>

<asp:Content runat="server" ID="Content" ContentPlaceHolderID="TitleContent">
    Extra Ticket Petition</asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="HeaderContent">
</asp:Content>
<asp:Content runat="server" ID="Content2" ContentPlaceHolderID="MainContent">
    <h1>Extra Ticket Petition</h1>
    
    <% if (Model.DisclaimerStartDate < DateTime.Now) { %>
        <h2><%= StaticValues.Txt_ExtraTicketRequestDisclaimer %></h2>
    <% } %>
    
    <%= Html.ValidationSummary("Please correct all errors below") %>
    <%= Html.ClientSideValidation<Commencement.Core.Domain.ExtraTicketPetition>("ExtraTicketPetition") %>
    
    <h2>Student Information</h2>
    
    <ul class="registration_form">
        <li class="prefilled">
            <strong>Student Id:</strong> 
            <span><%= Html.Encode(Model.Registration.Student.StudentId) %></span>
        </li>
        <li class="prefilled"><strong>Name:</strong> <span><%= Html.Encode(Model.Registration.Student.FullName) %></span> </li>
        <li class="prefilled"><strong>Major:</strong> <span><%= Html.Encode(Model.Registration.Major.Name) %></span> </li>
        <li class="prefilled"><strong>Ceremony:</strong> <span><%= Html.Encode(Model.Registration.Ceremony.DateTime) %></span> </li>
        <li class="prefilled"><strong>Delivery Method:</strong> <span><%= Html.Encode(Model.Registration.TicketDistributionMethod) %></span></li>
        <li class="prefilled"><strong>Original # Tickets Requested:</strong>
            <span><%= Html.Encode(Model.Registration.NumberTickets) %></span>
        </li>
    </ul>
            
    <% using (Html.BeginForm()) { %>
        <%= Html.AntiForgeryToken() %>
        
        <ul class="registration_form">
            <li><strong>Extra Tickets:</strong>
                <select id="numberTickets" name="numberTickets">
                    <% for (int i = 1; i <= Model.Registration.Ceremony.ExtraTicketPerStudent; i++)
                       { %>
                       
                       <% var selected = i == Model.ExtraTicketPetition.NumberTickets; %>
                    
                        <% if (selected) {%>
                            <option value="<%= i %>" selected="selected"><%= string.Format("{0:00}", i) %></option>
                        <% } else {%>
                            <option value="<%= i %>"><%= string.Format("{0:00}", i) %></option>
                        <% } %>

                    <% } %>
                </select>
            </li>
            <li><strong></strong><input type="submit" value="Submit" /></li>
        </ul>
    <% } %>
    
</asp:Content>




