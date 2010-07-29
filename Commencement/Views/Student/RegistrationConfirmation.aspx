<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Commencement.Core.Domain.Registration>" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content runat="server" ID="Content" ContentPlaceHolderID="TitleContent">Registration Confirmation</asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="HeaderContent"></asp:Content>
<asp:Content runat="server" ID="Content2" ContentPlaceHolderID="MainContent">

<h2>
    Your registration was successfull for <%= Html.Encode(Model.Ceremony.Name) %>.  
</h2>
<h3>
    Please print this page for your records.
    <a href="#">Alumni Survey</a>
</h3>

<% Html.RenderPartial("RegistrationDisplay", Model); %>

<h3>Reminder that registration for the Commencement Ceremony does not mean you have completed all
    requirements necessary for your degree completion.
</h3>

</asp:Content>