<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.StudentDisplayRegistrationViewModel>" MasterPageFile="~/Views/Shared/Site.Master" %>
<%@ Import Namespace="Commencement.Controllers" %>
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

    <h2>Registered Ceremony</h2>
    <% foreach(var a in Model.Registration.RegistrationParticipations) { %>
        <% Html.RenderPartial("RegisteredCeremonyDisplay", a); %>
        <hr />
    <% } %>
    
</asp:Content>

