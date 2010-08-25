<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Core.Domain.RegistrationPetition>" %>
<%@ Import Namespace="Commencement.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	RegistrationPetition
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="btn">
        <li><%= Html.ActionLink<PetitionController>(a=>a.Index(), "Back to List") %></li>
        <% if (Model.IsPending) {%>
            <li><%= Html.ActionLink<PetitionController>(a=>a.DecideRegistrationPetition(Model.Id, true), "Approve") %></li>
            <li><%= Html.ActionLink<PetitionController>(a=>a.DecideRegistrationPetition(Model.Id, false), "Deny") %></li>
        <% } %>
    </ul>

    <h2><%= Html.Encode(Model.FullName) %></h2>
    
    <ul class="registration_form">
        <li><strong>Date Submitted:</strong><%= Html.Encode(Model.DateSubmitted.ToString("g")) %></li>
        <li><strong>Date Decision:</strong><%= Html.Encode(Model.DateDecision != null ? Model.DateDecision.ToString() : string.Empty) %></li>
        <li><strong>Approved:</strong><%= Html.Encode(Model.IsPending ? "Pending" : (Model.IsApproved ? "Yes" : "No")) %></li>
    </ul>
    
    <ul class="registration_form">
        <li><strong>Student Id:</strong><%= Html.Encode(Model.StudentId) %></li>
        <li><strong>Email:</strong><%= Html.Encode(Model.Email) %></li>
        <li><strong>Major:</strong><%= Html.Encode(Model.MajorCode.Name) %></li>
        <li><strong>Units:</strong><%= Html.Encode(Model.Units) %></li>
    </ul>
    
    <ul class="registration_form">
        <li><strong>Currently Registered:</strong><%= Html.Encode(Model.CurrentlyRegistered) %></li>
        <li><strong>Reason for Petition:</strong><%= Html.Encode(Model.ExceptionReason) %></li>
        <li><strong>Term to Complete:</strong><%= Html.Encode(Model.CompletionTerm) %></li>
        <li><strong>Transfer Units From*:</strong><%= Html.Encode(Model.TransferUnitsFrom) %></li>
        <li><strong>Transfer Units*:</strong><%= Html.Encode(Model.TransferUnits) %></li>
    </ul>
    
    <h3>* Signifies transfer units the student feels are still pending.</h3>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
