<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.RegistrationDataViewModel>" %>
<%@ Import Namespace="Commencement.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	RegistrationData
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="btn">
    <li><%= Html.ActionLink<ReportController>(a=>a.Index(), "Back to List")  %></li></ul>

    <h2>Registration Data</h2>

    <% foreach(var rd in Model.RegistrationData) { %>
    
        <div class="RegistrationData_Container" style="border: solid 1px;">
            <ul class="registration_form">
                <li><strong>Term:</strong><%= rd.TermCode.Name %></li>
                <li><strong>Ceremony Time:</strong><%= rd.Ceremony.DateTime.ToString("g") %></li>
                
                <li><strong># of Registrants:</strong><%= rd.Registrants %></li>
                <li><strong># of Cancelled Registrants:</strong><%= rd.CancelledRegistrants %></li>
                <li><strong># of Registration Petitions Submitted:</strong><%= rd.RegistrationPetitionsSubmitted %></li>
                <li><strong># of Registration Petitions Approved:</strong><%= rd.RegistrationPetitionsApproved %></li>
                <li><strong># of Tickets Requested with Registration:</strong><%= rd.TicketsRequested %></li>
                <li><strong># of Tickets Requested by Petition:</strong><%= rd.ExtraTicketsRequested %></li>
                <li><strong>Total Tickets Approved:</strong><%= rd.TotalTickets %></li>
                
            </ul>
        </div>
    
    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
