<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Mvc.Controllers.ViewModels.RegistrationDataViewModel>" %>
<%@ Import Namespace="Commencement.Mvc.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Commencement | Registration Data
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="btn">
    <li><%= Html.ActionLink<ReportController>(a=>a.Index(), "Back to List")  %></li></ul>

    <h2>Registration Data</h2>

    <div class="t-widget t-grid">
        <table cellspacing="0">
            <thead>
                <tr class="top-header">
                    <th colspan="2" class="t-header">Ceremony Info</th>
                    <th colspan="2" class="t-header">Registrations</th>
                    <th colspan="2" class="t-header">Registration Petition</th>
                    <th colspan="2" class="t-header">Extra Ticket Petition</th>
                    <th colspan="3" class="t-header t-last-header">Tickets</th>
                </tr>
                <tr>
                    <th class="t-header">Term</th>
                    <th class="t-header">Time</th>
                    <th class="t-header">Total</th>
                    <th class="t-header">Cancelled</th>
                    <th class="t-header">Submitted</th>
                    <th class="t-header">Approved</th>
                    <th class="t-header">Submitted</th>
                    <th class="t-header">Approved</th>
                    <th class="t-header">Pavilion</th>
                    <th class="t-header">Ballroom</th>
                    <th class="t-header t-last-header">By Petition</th>
                </tr>
            </thead>
            <tbody>
                <% foreach (var a in Model.RegistrationData) { %>
                    <tr>
                        <td><%: a.TermCode.Name %></td>
                        <td><%: a.Ceremony.DateTime.ToShortTimeString() %></td>
                        <td><%: a.Registrants %></td>
                        <td><%: a.CancelledRegistrants %></td>
                        <td><%: a.RegistrationPetitionsSubmitted %></td>
                        <td><%: a.RegistrationPetitionsApproved %></td>
                        <td><%: a.ExtraTicketPetitionsSubmitted %></td>
                        <td><%: a.ExtraTicketPetitionsApproved %></td>
                        <td><%: a.TicketsPavilion %></td>
                        <td><%: a.TicketsBallroom.HasValue ? a.TicketsBallroom.Value.ToString() : "n/a" %></td>
                        <td><%: a.TicketsByPetition %></td>
                    </tr>            
                <% } %>
            </tbody>
        </table>
    </div>




<%--    <% foreach(var rd in Model.RegistrationData) { %>
    
        <div class="RegistrationData_Container" style="border: solid 1px;">
            <ul class="registration_form">
                <li><strong>Term:</strong><%= rd.TermCode.Name %></li>
                <li><strong>Ceremony Time:</strong><%= rd.Ceremony.DateTime.ToString("g") %></li>
                
                <li><strong># of Registrants:</strong><%= rd.Registrants %></li>
                <li><strong># of Cancelled Registrants:</strong><%= rd.CancelledRegistrants %></li>
                <li><strong># of Registration Petitions Submitted:</strong><%= rd.RegistrationPetitionsSubmitted %></li>
                <li><strong># of Registration Petitions Approved:</strong><%= rd.RegistrationPetitionsApproved %></li>
                <li><strong># of Tickets Requested with Registration:</strong><%= rd.TicketsRequested %></li>
                <li><strong># of Tickets Approved by Petition:</strong><%= rd.ExtraTicketsRequested %></li>
                <li><strong>Total Tickets Approved:</strong><%= rd.TotalTickets %></li>
                
            </ul>
        </div>
    
    <% } %>--%>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

    <style type="text/css">
        th, td {padding: 5px; font-size: 13px; text-align: center;}
        .t-grid .top-header .t-header {font-weight: bold;}
        .t-grid .t-header{text-align: center;}
    </style>

    <script type="text/javascript">
        $(function () {
            $("table tr:even").addClass("t-alt");
        });
    </script>

</asp:Content>
