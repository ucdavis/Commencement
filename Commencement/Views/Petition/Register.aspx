<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.RegistrationPetitionModel>"
    MasterPageFile="~/Views/Shared/Site.Master" %>

<asp:Content runat="server" ID="Content" ContentPlaceHolderID="TitleContent">
    Commencement Petition Registration</asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="HeaderContent">
</asp:Content>
<asp:Content runat="server" ID="Content2" ContentPlaceHolderID="MainContent">
    <h1>
        Fall Commencement Ceremony</h1>
    <p>
        Some quick intriduction, two or three lines explaining what this is, what they need
        to do, the <strong>due date</strong>, and what will happen after they do this. We
        need to include a line stating <strong>all fields are required</strong></p>
    
    <%= Html.ValidationSummary("Please correct all errors below") %>
    <%= Html.ClientSideValidation<Commencement.Core.Domain.RegistrationPetition>("RegistrationPetition") %>
    
    <h2>
        Student Information</h2>
    <ul class="registrationPetition_form">
        <li class="prefilled"><strong>Student ID:</strong> <span>123456789</span> </li>
        <li class="prefilled"><strong>Name:</strong> <span>john jacob jingleheimer schmidt</span>
        </li>
        <li class="prefilled"><strong>Units Complted:</strong> <span>224.5</span> </li>
        <li class="prefilled"><strong>Major:</strong> <span>Life-ology</span> </li>
    </ul>
    <h2>
        Petition Information</h2>
        
    <% using (Html.BeginForm()) { %>
        <%= Html.AntiForgeryToken() %>
   
    <ul class="registrationPetition_form">
        <li><strong>Currently Registered: </strong>
            <%= Html.TextBox("Registered")%>
        </li>
        <li><strong>Reason for Petition: </strong> 
            <%= Html.TextAreaFor(x => x.RegistrationPetition.ExceptionReason, new { maxlength = 1000, size = 5 }) %>
            <%= Html.ValidationMessageFor(x => x.RegistrationPetition.ExceptionReason)%>
        </li>
      </ul>
    
      <h4>
        Transfer Units</h4>
      <ul class="registrationPetition_form">  
        <li><strong>From: </strong>
            <%= Html.TextBox("From")%>
        </li>
        <li><strong>Units: </strong>
         <%= Html.TextBox("RegistrationPetition.Units")%>
        </li>
        <li><strong>Term to Complete:</strong>
           <%= this.Select("RegistrationPetition.CompletionTerm").Options(Model.TermCodes, x => x.Id, x => x.Description)%>
        </li>
    </ul>
    
    
    <input type="submit" value="Petition Registration for Commencement" />
    
    <% } %>
    <h3>
        <%--Disclaimer about the page and the process. Disclaimer about the page and the process.--%>
    </h3>
</asp:Content>




