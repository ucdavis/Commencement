<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.RegistrationModel>" %>
<%@ Import Namespace="Commencement.Controllers" %>
<%@ Import Namespace="Commencement.Controllers.Filters" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Commencement | Student Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="btn">
    <li>
        <% if (Request.QueryString["Registration"]!= null && Convert.ToBoolean(Request.QueryString["Registration"])) { %>
            <%= Html.ActionLink<AdminController>(a=>a.Registrations(null, null, null, null, null, null), "Back to List") %>
        <% } else { %>
            <%= Html.ActionLink<AdminController>(a=>a.Students(null, null, null, null, null), "Back to List") %>
        <% } %>
    </li>
    </ul>

    <div class="page_bar">
        <div class="col1"><h2>Student Details</h2></div>
        <div class="col2">
            <%: Html.ActionLink<AdminController>(a => a.EditStudent(Model.Student.Id), "Edit Student", new { @class="button" })%>
            <% if (Model.Participations.Any()) { %>
                <%: Html.ActionLink<AdminController>(a => a.RegisterForStudent(Model.Student.Id), "Edit Registration", new { @class = "button" })%>
            <% } %>
            <%: Html.ActionLink<AdminController>(a => a.Block(Model.Student.Id), "Block from Registration", new { @class = "button" })%>
            <% if (User.IsInRole(RoleNames.RoleEmulationUser)) { %>
                <%= Html.ActionLink<AccountController>(a => a.Emulate(Model.Student.Login), "Emulate", new { @class = "button" })%>
            <% } %>
        </div>
    </div>

    <% if (Model.Student.SjaBlock) { %>
        <div class="ui-state-error ui-corner-all message">
            SJA has asked that we not allow this student to walk.
        </div>
    <% } %>
    <% if (Model.Student.Blocked) { %>
        <div class="ui-state-error ui-corner-all message">
            Student has been blocked from registration.
        </div>
    <% } %>

    <fieldset>
        
        <legend>Student</legend>

        <% Html.RenderPartial("StudentInformationPartial", Model.Student); %>

        <!-- Display ceremony override if the override is defined -->
        <% if (Model.Student.Ceremony != null) { %>
        <ul class="registration_form">
            <li class="prefilled"><strong>Ceremony Override:</strong>
                                  <span><%= Html.Encode(Model.Student.Ceremony.DateTime.ToString("g")) %></span>
            </li>
        </ul>
    <% } %>

    </fieldset>

    <% if(Model.Registration != null) { %>
    
        <fieldset>
    
            <legend>Contact Information</legend>

            <!-- Student is registered, show that information -->
            <% Html.RenderPartial("RegistrationDisplay", Model.Registration); %>

        </fieldset>


        <% if (Model.Registration.RegistrationParticipations.Any()) { %>
        
            <fieldset>
            
                <% if (Model.Registration.RegistrationParticipations.Count() == 1) { %>
                    <legend>Ceremony</legend>
                <% } else { %>
                    <legend>Ceremonies</legend>
                <% } %>

                <% foreach(var a in Model.Registration.RegistrationParticipations) { %>
                    <% Html.RenderPartial("RegisteredCeremonyDisplay", a); %>
                <% } %>

            </fieldset>

        <% } %>

        <% if (Model.Registration.RegistrationPetitions.Any()) { %>
            <h2>Petition Information</h2>
            <% foreach (var a in Model.Registration.RegistrationPetitions) { %>
                <% Html.RenderPartial("RegistrationPetitionDisplay", a); %>
            <% } %>
        <% } %> 
    
    <% } else { %>
        <fieldset>
        
            <legend>Registration</legend>

            Student has not registered.

        </fieldset>
    <% } %>    

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

    <style type="text/css">
        .page_bar .col1 {width: 30%;}
        .page_bar .col2 {width: 70%;}
    </style>

</asp:Content>
