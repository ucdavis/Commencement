<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.RegistrationPetitionModel>"
    MasterPageFile="~/Views/Shared/Site.Master" %>

<asp:Content runat="server" ID="Content" ContentPlaceHolderID="TitleContent">
    Commencement Petition Registration</asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="HeaderContent">
</asp:Content>
<asp:Content runat="server" ID="Content2" ContentPlaceHolderID="MainContent">
    <h1>Fall Commencement Ceremony</h1>
    <p>
        Some quick intriduction, two or three lines explaining what this is, what they need
        to do, the <strong>due date</strong>, and what will happen after they do this. We
        need to include a line stating <strong>all fields are required</strong></p>
    
    <%= Html.ValidationSummary("Please correct all errors below") %>
    <%= Html.ClientSideValidation<Commencement.Core.Domain.RegistrationPetition>("RegistrationPetition") %>
    
    <h2>Student Information</h2>
    <ul class="registration_form">
        <li class="prefilled"><strong>Student ID:</strong><span><%= Html.Encode(Model.SearchStudent.Id)%></span> </li>
        <li class="prefilled"><strong>Name:</strong><span><%= Html.Encode(Model.SearchStudent.FullName) %></span></li>
        <li class="prefilled"><strong>Email:</strong><span><%= Html.Encode(Model.SearchStudent.Email) %></span></li>
        <li class="prefilled"><strong>Units Complted:</strong><span><%= Html.Encode(Model.SearchStudent.HoursEarned)%></span> <span>224.5</span> </li>
        <li class="prefilled"><strong>Major:</strong> <span><%= Html.Encode(Model.SearchStudent.MajorCode)%></span></li>
    </ul>
    
    <h2>Petition Information</h2>
    <% using (Html.BeginForm()) { %>
        <%= Html.AntiForgeryToken() %>
   
        <%= Html.Hidden("RegistrationPetition.Pidm", Model.SearchStudent.Pidm) %>
        <%= Html.Hidden("RegistrationPetition.StudentId", Model.SearchStudent.Id) %>
        <%= Html.Hidden("RegistrationPetition.FirstName", Model.SearchStudent.FirstName) %>
        <%= Html.Hidden("RegistrationPetition.LastName", Model.SearchStudent.LastName) %>
        <%= Html.Hidden("RegistrationPetition.Email", Model.SearchStudent.Email)%>
        <%= Html.Hidden("RegistrationPetition.Login", Model.SearchStudent.LoginId)%>
        <%= Html.Hidden("RegistrationPetition.MajorCode", Model.SearchStudent.MajorCode)%>
        <%= Html.Hidden("RegistrationPetition.Units", Model.SearchStudent.HoursEarned)%>
   
   
        <ul class="registration_form">
            <li><strong>Currently Registered: </strong>
                <%= Html.TextBoxFor(x=>x.RegistrationPetition.CurrentlyRegistered)%>
                <%= Html.ValidationMessageFor(x=>x.RegistrationPetition.CurrentlyRegistered) %>
            </li>
            <li><strong>Reason for Petition: </strong> 
                <%= Html.TextAreaFor(x => x.RegistrationPetition.ExceptionReason, new { maxlength = 1000, size = 5 }) %>
                <%= Html.ValidationMessageFor(x => x.RegistrationPetition.ExceptionReason)%>
            </li>
            <li><strong>Term to Complete:</strong>
               <%= this.Select("RegistrationPetition.CompletionTerm").Options(Model.TermCodes, x => x.Id, x => x.Description).Selected(!string.IsNullOrEmpty(Model.RegistrationPetition.CompletionTerm) ? Model.RegistrationPetition.CompletionTerm : string.Empty)%>
            </li>
          </ul>
    
      <h2>Transfer Units</h2>
      
      <ul class="registration_form">  
        <li><strong>From: </strong>
            <%= Html.TextBoxFor(x=>x.RegistrationPetition.TransferUnitsFrom) %>
            <%= Html.ValidationMessageFor(x=>x.RegistrationPetition.TransferUnitsFrom) %>
        </li>
        <li><strong>Transfer Units: </strong>
         <%= Html.TextBoxFor(x=>x.RegistrationPetition.TransferUnits) %>
         <%= Html.ValidationMessageFor(x=>x.RegistrationPetition.TransferUnits) %>
        </li>

    </ul>
    
    <br/>
        <%= Html.SubmitButton("submit", "Submit Petition") %>
    
    <% } %>
    <h3>
        <%--Disclaimer about the page and the process. Disclaimer about the page and the process.--%>
    </h3>
</asp:Content>




