<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Commencement.Core.Domain.Registration>" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content runat="server" ID="Content" ContentPlaceHolderID="TitleContent">Display Registration</asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="HeaderContent"></asp:Content>
<asp:Content runat="server" ID="Content2" ContentPlaceHolderID="MainContent">
    <h2>Your registration for <%= Html.Encode(Model.Ceremony.Name) %></h2>
    
    <% if ((bool)ViewData["CanEditRegistration"]) { %>
        
        <%= Html.ActionLink("Edit Your Registration", "EditRegistration", new { id = Model.Id }) %>
    
    <% } %>
    
    <% Html.RenderPartial("RegistrationDisplay", Model); %>
        
</asp:Content>

