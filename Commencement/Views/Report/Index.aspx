<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.ReportViewModel>" %>
<%@ Import Namespace="Commencement.Controllers" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="btn">
    <li><%= Html.ActionLink<AdminController>(a=>a.Index(), "Home")  %></li></ul>

    <h2>Reporting</h2>
    <ul class="">
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
        </li>
        
        <li>
            <strong>
                <% using (Html.BeginForm("GetReport", "Report", FormMethod.Get)) { %>
                    <a href="#" class="submit_anchor">Total Registration Petition</a>
                    <%= Html.Hidden("Report", ReportController.Report.TotalRegistrationPetitions) %>
                    <%= Html.Hidden("termCode", Model.TermCode.Id, new {@class = "term_value"}) %>
                <% } %>
            </strong>          
        </li>
        
        <li>
            <strong>
                <% using (Html.BeginForm("GetReport", "Report", FormMethod.Get)) { %>
                    <a href="#" class="submit_anchor">Sum of all Tickets</a>
                    <%= Html.Hidden("Report", ReportController.Report.SumOfAllTickets) %>
                    <%= Html.Hidden("termCode", Model.TermCode.Id, new {@class = "term_value"}) %>
                <% } %>
            </strong>          
        </li>
        
        <li>
            <strong>
                <% using (Html.BeginForm("GetReport", "Report", FormMethod.Get)) { %>
                    <a href="#" class="submit_anchor">Special Needs Request</a>
                    <%= Html.Hidden("Report", ReportController.Report.SpecialNeedsRequest) %>
                    <%= Html.Hidden("termCode", Model.TermCode.Id, new {@class = "term_value"}) %>
                <% } %>
            </strong>          
        </li>
        
        <li>
            <strong>
                <% using (Html.BeginForm("GetReport", "Report", FormMethod.Get)) { %>
                    <a href="#" class="submit_anchor">Registrars Report</a>
                    <%= Html.Hidden("Report", ReportController.Report.RegistrarsReport) %>
                    <%= Html.Hidden("termCode", Model.TermCode.Id, new {@class = "term_value"}) %>
                <% } %>
            </strong>          
        </li>
        
        <li><strong><%= Html.ActionLink<ReportController>(a=>a.RegistrationData(), "Registration Data") %></strong>
            Statistics of all past and present terms broken down by ceremony.
        </li>
    </ul>

    <h2>Label Printing</h2>
    <ul class="">
        <li>
            <strong><%= Html.ActionLink<ReportController>(a => a.GenerateAveryLabels(TermService.GetCurrent().Id, false), "Print Pending Labels")%></strong>
            This will print all pending labels that need to be printed and will update records so that they will not be printed in this list again.
        </li>
        <li>
            <strong><%= Html.ActionLink<ReportController>(a => a.GenerateAveryLabels(TermService.GetCurrent().Id, true), "Print All Labels")%></strong>
            This will print all labels for the current term, regardless of whether they have been printed already.
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