<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.RegistrationModel>" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content runat="server" ID="Content" ContentPlaceHolderID="TitleContent">Edit registration</asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="HeaderContent"></asp:Content>

<asp:Content runat="server" ID="Content3" ContentPlaceHolderID="logoContent">
</asp:Content>

<asp:Content runat="server" ID="Content2" ContentPlaceHolderID="MainContent">

<h1><%= Model.Ceremony.Name %></h1>
    <p>
        Welcome back <%= Html.Encode(Model.Student.FirstName) %>.  
    </p>
    
    <%= Html.ValidationSummary("Please correct all errors below") %>
    <%= Html.ClientSideValidation<Commencement.Core.Domain.Registration>("Registration") %>
    
    <% using (Html.BeginForm()) { %>
        <%= Html.AntiForgeryToken() %>

    <% Html.RenderPartial("RegistrationEditForm"); %>    
    
    <input type="submit" value="Update Registration" />
    
    <% } %>


</asp:Content>
