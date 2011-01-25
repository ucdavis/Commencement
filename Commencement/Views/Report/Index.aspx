<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.ReportViewModel>" %>
<%@ Import Namespace="Commencement.Controllers" %>
<%@ Import Namespace="Commencement.Controllers.Services" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Commencement | Reports
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="btn">
    <li><%= Html.ActionLink<AdminController>(a=>a.Index(), "Home")  %></li></ul>

    <h2>Reporting</h2>
    <ul class="reporting">
        <li><strong>Term:</strong>
            <%= this.Select("termCode").Options(Model.TermCodes, x=>x.Id, x=>x.Description).Selected(Model.TermCode.Id) %>
        </li>
        
        <li>
            <strong>
                <% using (Html.BeginForm("GetReport", "Report", FormMethod.Get)) { %>
                    <a href="#" class="submit_anchor">Total Registered Students</a>
                    <%= Html.Hidden("Report", ReportController.Report.TotalRegisteredStudents) %>
                    <%= Html.Hidden("termCode", Model.TermCode.Id, new {@class = "term_value"}) %>
                <% } %>
            </strong>
            
            List of all registered students for the selected term.
        </li>
        
        <li>
            <strong>
                <% using (Html.BeginForm("GetReport", "Report", FormMethod.Get)) { %>
                    <a href="#" class="submit_anchor">Total Registration Petition</a>
                    <%= Html.Hidden("Report", ReportController.Report.TotalRegistrationPetitions) %>
                    <%= Html.Hidden("termCode", Model.TermCode.Id, new {@class = "term_value"}) %>
                <% } %>
            </strong>
            
            List of all registration petitions for the selected term.
        </li>
        
        <li>
            <strong>
                <% using (Html.BeginForm("GetReport", "Report", FormMethod.Get)) { %>
                    <a href="#" class="submit_anchor">Sum of all Tickets</a>
                    <%= Html.Hidden("Report", ReportController.Report.SumOfAllTickets) %>
                    <%= Html.Hidden("termCode", Model.TermCode.Id, new {@class = "term_value"}) %>
                <% } %>
            </strong>          
            
            Summary report of all tickets.
        </li>
        
        <li>
            <strong>
                <% using (Html.BeginForm("GetReport", "Report", FormMethod.Get)) { %>
                    <a href="#" class="submit_anchor">Special Needs Request</a>
                    <%= Html.Hidden("Report", ReportController.Report.SpecialNeedsRequest) %>
                    <%= Html.Hidden("termCode", Model.TermCode.Id, new {@class = "term_value"}) %>
                <% } %>
            </strong>          
            
            Special needs report for the selected term.
        </li>
        
        <li>
            <strong>
                <% using (Html.BeginForm("GetReport", "Report", FormMethod.Get)) { %>
                    <a href="#" class="submit_anchor">Registrars Report</a>
                    <%= Html.Hidden("Report", ReportController.Report.RegistrarsReport) %>
                    <%= Html.Hidden("termCode", Model.TermCode.Id, new {@class = "term_value"}) %>
                <% } %>
            </strong>          
            
            Registrar's Report for the selected term.
        </li>
        
        <li>
            <strong>
                <% using (Html.BeginForm("GetReport", "Report", FormMethod.Get)) { %>
                    <a href="#" class="submit_anchor">Ticket Sign Out Sheet</a>
                    <%= Html.Hidden("Report", ReportController.Report.TicketSignOutSheet) %>
                    <%= Html.Hidden("termCode", Model.TermCode.Id, new {@class = "term_value"}) %>
                <% } %>
            </strong>          
            
            Ticket sign out sheet for the selected term.
        </li>
        
        <li><strong><%= Html.ActionLink<ReportController>(a=>a.RegistrationData(), "Registration Data") %></strong>
            Statistics of all past and present terms broken down by ceremony.
        </li>
    </ul>

    <h2>Label Printing</h2>
    <ul class="reporting">
        <li>
            <strong><%= Html.ActionLink<ReportController>(a => a.GenerateAveryLabels(TermService.GetCurrent().Id, true, false), "Print Pending Mailing Labels")%></strong> - This will print all pending labels for mailing that need to be printed and will update records so that they will not be printed in this list again.
        </li>
        <li>
            <strong><%= Html.ActionLink<ReportController>(a => a.GenerateAveryLabels(TermService.GetCurrent().Id, false, false), "Print Pending Pickup Labels")%></strong>
            <br />
            This will print all pending labels for pickup that need to be printed and will update records so that they will not be printed in this list again.
        </li>
        <li>
            <strong><%= Html.ActionLink<ReportController>(a => a.GenerateAveryLabels(TermService.GetCurrent().Id, true, true), "Print All Mailing Labels")%></strong>
            <br />
            This will print all mailing labels for the current term, regardless of whether they have been printed already.
        </li>
        <li>
            <strong><%= Html.ActionLink<ReportController>(a => a.GenerateAveryLabels(TermService.GetCurrent().Id, false, true), "Print All Pickup Labels")%></strong>
            <br />
            This will print all pickup labels for the current term, regardless of whether they have been printed already.
        </li>
    </ul>

    

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    <script type="text/javascript">
        $(function() {
            $(".submit_anchor").click(function() { $(this).parent("form").submit(); });
            //            $(".term_value").val($("#termCode").val());
            $("#termCode").change(function() { $(".term_value").val($("#termCode").val()); });
        });
    </script>
</asp:Content>
