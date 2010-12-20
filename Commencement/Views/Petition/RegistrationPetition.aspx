<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Core.Domain.RegistrationPetition>" %>
<%@ Import Namespace="Commencement.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	RegistrationPetition
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="btn">
        <li><%= Html.ActionLink<PetitionController>(a=>a.RegistrationPetitions(), "Back to List") %></li>
        <% if (Model.IsPending) {%>
            <li>
                <% using (Html.BeginForm("DecideRegistrationPetition", "Petition", FormMethod.Post)) { %>
                
                    <%= Html.AntiForgeryToken() %>
                    <%: Html.Hidden("id", Model.Id) %>
                    <%: Html.Hidden("isApproved", true) %>
                    <%: Html.SubmitButton("Approve", "Approve") %>
                
                <% }%>
            </li>
            <li>
                <% using (Html.BeginForm("DecideRegistrationPetition", "Petition", FormMethod.Post)) { %>
                
                    <%= Html.AntiForgeryToken() %>
                    <%: Html.Hidden("id", Model.Id) %>
                    <%: Html.Hidden("isApproved", false) %>
                    <%: Html.SubmitButton("Deny", "Deny") %>
                
                <% }%>
            </li>
        <% } %>
    </ul>

    <h2><%= Html.Encode(Model.Registration.Student.FullName) %></h2>
      
    <h3>Student Information</h3>
    <ul class="registration_form">
        <li><strong>Student Id:</strong><%= Html.Encode(Model.Registration.Student.StudentId) %></li>
        <li><strong>Email:</strong><%= Html.Encode(Model.Registration.Student.Email) %></li>
        <% if (!string.IsNullOrEmpty(Model.Registration.Email)) { %><li><strong>Secondary Email:</strong><%: Html.Encode(Model.Registration.Email) %></li><% } %>
        <li><strong>Major:</strong><%= Html.Encode(Model.MajorCode.MajorName) %></li>
        <li><strong>Units:</strong><%= Html.Encode(Model.Registration.Student.TotalUnits)%></li>
        <li><strong>Ceremony:</strong><%: Model.Ceremony.DateTime.ToString("g") %></li>
    </ul>
    
    <h3>Petition Information</h3>
<%--    <ul class="registration_form">
        <li><strong>Date Submitted:</strong><%= Html.Encode(Model.DateSubmitted.ToString("g")) %></li>
        <li><strong>Date Decision:</strong><%= Html.Encode(Model.DateDecision != null ? Model.DateDecision.ToString() : string.Empty) %></li>
        <li><strong>Approved:</strong><%= Html.Encode(Model.IsPending ? "Pending" : (Model.IsApproved ? "Yes" : "No")) %></li>
        <li><strong>Reason for Petition:</strong><%= Html.Encode(Model.ExceptionReason) %></li>
        <li><strong>Term to Complete:</strong><%= Html.Encode(Model.TermCodeComplete.Description) %></li>
        <li><strong>Transfer Units From*:</strong><%= Html.Encode(Model.TransferUnitsFrom) %></li>
        <li><strong>Transfer Units*:</strong><%= Html.Encode(Model.TransferUnits) %></li>
    </ul>--%>
    <% Html.RenderPartial("RegistrationPetitionDisplay", Model); %>

    <h3>* Signifies transfer units the student feels are still pending.</h3>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
