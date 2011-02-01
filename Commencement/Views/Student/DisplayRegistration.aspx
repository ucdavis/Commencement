<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.StudentDisplayRegistrationViewModel>" MasterPageFile="~/Views/Shared/Site.Master" %>
<%@ Import Namespace="Commencement.Controllers" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>
<asp:Content runat="server" ID="Content" ContentPlaceHolderID="TitleContent">Display Registration</asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="HeaderContent"></asp:Content>

<asp:Content runat="server" ID="Content3" ContentPlaceHolderID="logoContent">
</asp:Content>

<asp:Content runat="server" ID="Content2" ContentPlaceHolderID="MainContent">
       
    <ul class="btn">
        <% if (Model.CanEditRegistration) { %>
            <li><%= Html.ActionLink<StudentController>(a=>a.EditRegistration(Model.Registration.Id), "Edit Registation") %></li>
        <% } %>
        <% if (Model.CanPetitionForExtraTickets) { %>
            <li><%= Html.ActionLink<PetitionController>(a=>a.ExtraTicketPetition(Model.Registration.Id), "Extra Ticket Petition") %></li>
        <% } %>    
    </ul>

    <h2>Your commencement registration for <%= Html.Encode(Model.Registration.TermCode.Name) %></h2>
           
    <h2>Student Information</h2>
    <% Html.RenderPartial("StudentInformationPartial", Model.Registration.Student); %>
        
    <h2>Contact Information</h2>
    <% Html.RenderPartial("RegistrationDisplay", Model.Registration); %>

    <% if (Model.Registration.RegistrationParticipations.Count > 0) { %>
    <h2>Registered Ceremony</h2>
    <% foreach(var a in Model.Registration.RegistrationParticipations) { %>
        <!-- only display this message if it loads within 2 minutes of the registartion date -->
        <% if (DateTime.Now.Subtract(a.DateRegistered).TotalMinutes <= 2) { %>
        <div class="confirmation-container">
            <%= a.Ceremony.ConfirmationText %>
        </div>
        <% } %>
        <% Html.RenderPartial("RegisteredCeremonyDisplay", a); %>
        <hr />
    <% } %>
    <% } %>

    <% if (Model.Registration.RegistrationPetitions.Count > 0) { %>
    <h2>Petitioned Ceremony</h2>
    <% foreach(var a in Model.Registration.RegistrationPetitions) { %>
        <% Html.RenderPartial("RegistrationPetitionDisplay", a); %>
        <hr />
    <% } %>
    <% } %>
    
</asp:Content>

