<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Commencement.Core.Domain.Registration>" MasterPageFile="~/Views/Shared/Site.Master" %>
<%@ Import Namespace="Commencement.Controllers" %>
<asp:Content runat="server" ID="Content" ContentPlaceHolderID="TitleContent">Registration Confirmation</asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="HeaderContent"></asp:Content>

<asp:Content runat="server" ID="Content3" ContentPlaceHolderID="logoContent">
</asp:Content>


<asp:Content runat="server" ID="Content2" ContentPlaceHolderID="MainContent">

<h2>
    <%--Your registration was successfull for <%= Html.Encode(Model.Ceremony.Name) %>.  --%>
</h2>
<h3 style="font-size:large;">You are required to complete the following exit survey <a href="<%= ConfigurationManager.AppSettings["SurveyUrl"] %>">here</a></h3>
<h3>
If you would like to request extra tickets, please fill out <%= Html.ActionLink<PetitionController>(a=>a.ExtraTicketPetition(Model.Id), "this form.") %>
    <br />Please print this page for your records.
    
</h3>


<% Html.RenderPartial("RegistrationDisplay", Model); %>

<h3>Reminder that registration for the Commencement Ceremony does not mean you have completed all
    requirements necessary for your degree completion.
</h3>

</asp:Content>
