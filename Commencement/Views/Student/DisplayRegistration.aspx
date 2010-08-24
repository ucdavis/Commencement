<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Commencement.Core.Domain.Registration>" MasterPageFile="~/Views/Shared/Site.Master" %>
<%@ Import Namespace="Commencement.Controllers" %>
<asp:Content runat="server" ID="Content" ContentPlaceHolderID="TitleContent">Display Registration</asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="HeaderContent"></asp:Content>
<asp:Content runat="server" ID="Content2" ContentPlaceHolderID="MainContent">
    
    <ul class="btn">
        <% if ((bool)ViewData["CanEditRegistration"]) { %>
        
            <li><%= Html.ActionLink("Edit Your Registration", "EditRegistration", new { id = Model.Id }) %></li>
    
        <% } %>
        <li><%= Html.ActionLink<PetitionController>(a => a.ExtraTicketPetition(Model.Id), "Extra Ticket Petition") %></li>
    </ul>
    
    
    <h2>Your registration for <%= Html.Encode(Model.Ceremony.Name) %></h2>
    
    
    <% Html.RenderPartial("RegistrationDisplay", Model); %>
        
</asp:Content>

