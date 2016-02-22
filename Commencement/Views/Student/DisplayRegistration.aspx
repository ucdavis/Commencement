<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.StudentDisplayRegistrationViewModel>" MasterPageFile="~/Views/Shared/Site.Master" %>
<%@ Import Namespace="Commencement.Controllers" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>
<asp:Content runat="server" ID="Content" ContentPlaceHolderID="TitleContent">Display Registration</asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="HeaderContent">

<style type="text/css">
.btn {top:55px;}
</style>

</asp:Content>

<asp:Content runat="server" ID="Content3" ContentPlaceHolderID="logoContent">
</asp:Content>

<asp:Content runat="server" ID="Content2" ContentPlaceHolderID="MainContent">
       
    <div class="page_bar">
        <div class="col1" style="width: 55%;"><h2>Your commencement registration for <%= Html.Encode(Model.Registration.TermCode.Name) %></h2></div>
        <div class="col2" style="width: 45%;">
        <% if (Model.CanEditRegistration) { %>
            <%= Html.ActionLink<StudentController>(a=>a.EditRegistration(Model.Registration.Id), "Edit Registration", new {@class="button"}) %>
        <% } %>
        <% if (Model.CanPetitionForExtraTickets) { %>
            <%= Html.ActionLink<PetitionController>(a=>a.ExtraTicketPetition(Model.Registration.Id), "Extra Ticket Petition", new {@class="button"}) %>
        <% } %>    
        </div>
    </div>

    <fieldset>
        
        <legend>Student Information</legend>
        
        <% Html.RenderPartial("StudentInformationPartial", Model.Registration.Student); %>

    </fieldset>
           
    <fieldset>
        
        <legend>Contact Information</legend>
        
        <% Html.RenderPartial("RegistrationDisplay", Model.Registration); %>

    </fieldset>
        
    <% if (Model.Registration.RegistrationParticipations.Count > 0) { %>        
    
        <fieldset>
            
            <legend>Registered <%: Model.Registration.RegistrationParticipations.Count == 1 ? "Ceremony" : "Ceremonies" %></legend>
            
            <% foreach(var a in Model.Registration.RegistrationParticipations) { %>
                <!-- only display this message if it loads within 2 minutes of the registartion date -->
                <% if (DateTime.Now.Subtract(a.DateRegistered).TotalMinutes <= 2) { %>
                <div class="confirmation-container ui-corner-all ui-state-highlight">
                    <%= a.Ceremony.ConfirmationText %>
                </div>
                <% } %>
                <% Html.RenderPartial("RegisteredCeremonyDisplay", a); %>
            <% } %>

        </fieldset>

    <% } %>
    
    <% if (Model.Registration.RegistrationPetitions.Count > 0) { %>
    
        <fieldset>
            
            <legend>Petitioned <%: Model.Registration.RegistrationPetitions.Count == 1 ? "Ceremony" : "Ceremonies" %></legend>
            
            <% foreach(var a in Model.Registration.RegistrationPetitions) { %>
                <% Html.RenderPartial("RegistrationPetitionDisplay", a); %>
            <% } %>

        </fieldset>

    <% } %> 
    
<%--    <!-- Open up exit survey for each valid ceremony -->
    <% if (Model.Registration.RegistrationParticipations.Any()) { %>
        <% foreach (var a in Model.Registration.RegistrationParticipations.Where(a => !string.IsNullOrEmpty(a.Ceremony.SurveyUrl))) { %>
            <% if (DateTime.Now.Subtract(a.DateRegistered).TotalMinutes <= 2) { %>
            <script type="text/javascript">
                window.open('<%: a.Ceremony.SurveyUrl %>', '_<%: a.Ceremony.Id %>');
            </script>
            <% } %>
        <% } %>     
    <% } %>--%>

</asp:Content>

