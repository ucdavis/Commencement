<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Core.Domain.RegistrationPetition>" %>
<%@ Import Namespace="Commencement.MVC.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Commencement | Registration Petition
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="btn">
        <li><%: Html.ActionLink<PetitionController>(a=>a.RegistrationPetitions(), "Back to List") %></li>
    </ul>

    <h2><%: Model.Registration.Student.FullName %></h2>

    <ul class="registration_form">
        <li><strong>Student Id:</strong>
            <%: Model.Registration.Student.StudentId %>
        </li>
        <li><strong>Major:</strong>
            <%: Model.MajorCode.Name %>
        </li>
        <li><strong>Units:</strong>
            <%: Model.Registration.Student.EarnedUnits %>
        </li>
        <li><strong>Ceremony:</strong>
            <%: Model.Ceremony.DateTime.ToString("g") %>
        </li>
        <li><strong># Tickets:</strong>
            <%: Model.NumberTickets %>
        </li>
        <li><strong>Ticket Distribution:</strong>
            <%: Model.TicketDistributionMethod != null ? Model.TicketDistributionMethod.Name : "n/a" %>
        </li>
        <li><strong>Date Submitted:</strong>
            <%: Model.DateSubmitted.ToString("g") %>
        </li>
        <li><strong>Status:</strong>
            <%: Model.Status %>
        </li>
        <li><strong>Petition Reason:</strong>
            <%: Model.ExceptionReason %>
        </li>
        <% if (Model.IsPending) { %>
        <li><strong></strong>
            <% using (Html.BeginForm("DecideRegistrationPetition", "Petition", FormMethod.Post, new {style="display:inline;"})) { %>
                <%: Html.Hidden("isApproved", true) %>
                <%: Html.Hidden("id", Model.Id) %>
                <%: Html.AntiForgeryToken() %>
                <%: Html.SubmitButton("NotApproved", "Approved", new { @class="button" })%>
            <% } %>
            <% using (Html.BeginForm("DecideRegistrationPetition", "Petition", FormMethod.Post, new {style="display:inline;"})) { %>
                <%: Html.Hidden("isApproved", false) %>
                <%: Html.Hidden("id", Model.Id) %>
                <%: Html.AntiForgeryToken() %>
                <%: Html.SubmitButton("Deny", "Deny", new { @class = "button" })%>
            <% } %>
            |
            <%: Html.ActionLink("Cancel", "RegistrationPetitions") %>
        </li>
        <% } %>
    </ul>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>