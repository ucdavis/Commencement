<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Commencement.Core.Domain.Registration>" MasterPageFile="~/Views/Shared/Site.Master" %>
<%@ Import Namespace="Commencement.Controllers" %>
<asp:Content runat="server" ID="Content" ContentPlaceHolderID="TitleContent">Display Registration</asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="HeaderContent"></asp:Content>
<asp:Content runat="server" ID="Content2" ContentPlaceHolderID="MainContent">
    <h2>Your registration for <%= Html.Encode(Model.Ceremony.Name) %></h2>
    
    <% if ((bool)ViewData["CanEditRegistration"]) { %>
        
        <%= Html.ActionLink("Edit Your Registration", "EditRegistration", new { id = Model.Id }) %>
    
    <% } %>

    <div id="extra_ticket_link"><a href="<%= Url.Content("~/Forms/extra_ticket_request.pdf") %>">Extra Ticket Request Form</a></div>
    
    <%--<%= Html.ActionLink<PetitionController>(a => a.ExtraTicketPetition(Model.Id), "Extra Ticket Petition") %>--%>
    
    <% Html.RenderPartial("RegistrationDisplay", Model); %>
        
</asp:Content>

