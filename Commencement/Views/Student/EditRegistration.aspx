<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.RegistrationModel>" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content runat="server" ID="Content" ContentPlaceHolderID="TitleContent">Edit registration</asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="HeaderContent"></asp:Content>
<asp:Content runat="server" ID="Content2" ContentPlaceHolderID="MainContent">

<h1><%= Model.Ceremony.Name %></h1>
    <p>
        Some quick intriduction, two or three lines explaining what this is, what they need
        to do, the <strong>due date</strong>, and what will happen after they do this. We
        need to include a line stating <strong>all fields are required</strong></p>
    
    <%= Html.ValidationSummary("Please correct all errors below") %>
    <%= Html.ClientSideValidation<Commencement.Core.Domain.Registration>("Registration") %>
    
    <% using (Html.BeginForm()) { %>
        <%= Html.AntiForgeryToken() %>

    <% Html.RenderPartial("RegistrationEditForm"); %>    
    
    <input type="submit" value="Register for Commencement" />
    
    <% } %>


</asp:Content>
