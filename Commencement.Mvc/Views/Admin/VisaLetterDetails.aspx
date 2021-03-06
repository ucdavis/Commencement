﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.AdminVisaDetailsModel>" %>
<%@ Import Namespace="Commencement.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Visa Letter Request
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <ul class="btn">
        <li><%: Html.ActionLink<AdminController>(a=>a.VisaLetters(null, null, null ,false), "Back to List of Letter Requests") %></li>
        <li><%: Html.ActionLink<AdminController>(a=>a.VisaLetterDecide(Model.VisaLetter.Id), "Edit") %></li>
    </ul>
   

    <h2>Visa Letter Request</h2>
    <% if (Model.VisaLetter.Student.SjaBlock) { %>
        <div class="ui-state-error ui-corner-all message">
            SJA has asked that we not allow this student to walk.
        </div>
    <% } %>
    <% if (Model.VisaLetter.Student.Blocked) { %>
        <div class="ui-state-error ui-corner-all message">
            Student has been blocked from registration.
        </div>
    <% } %>
    
    <% Html.RenderPartial("VisaLetterDetailsPartial", Model.VisaLetter); %>
    <fieldset>
        <legend>Approval Details</legend>
            <ul class="registration_form">
                <li><strong>Ceremony Date:</strong>
                    <%= Html.Encode(Model.VisaLetter.CeremonyDateTime.HasValue ? Model.VisaLetter.CeremonyDateTime.Value.Date.ToString("d") : string.Empty) %>
                </li>
                <li><strong>Last Edited By:</strong>
                    <%= Html.Encode(Model.VisaLetter.ApprovedBy) %>
                </li>  
                <li><strong>Last Edited:</strong>
                    <%= Html.Encode(Model.VisaLetter.LastUpdateDateTime) %>
                </li>  
                             
            </ul>
        </fieldset>

    <fieldset>
        <legend>Related Letters</legend>
        <% Html.RenderPartial("VisaLetterTablePartial", Model.RelatedLetters); %>
    </fieldset>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>
