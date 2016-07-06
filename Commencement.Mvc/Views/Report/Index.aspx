<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Mvc.Controllers.ViewModels.ReportViewModel>" %>
<%@ Import Namespace="Commencement.Mvc.Controllers" %>
<%@ Import Namespace="Commencement.Mvc.Controllers.Services" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Commencement | Reports
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="btn">
    <li><%= Html.ActionLink<AdminController>(a=>a.Index(), "Home")  %></li>
    </ul>
    
    <fieldset>
        
        <legend>Term</legend>
        
        <%= this.Select("termCode").Options(Model.TermCodes, x=>x.Id, x=>x.Name).Selected(Model.TermCode.Id) %>

    </fieldset>

    <fieldset>
        
        <legend>Reports</legend>
        
        <div class="report">
            
            <div class="title">
            <div class="col1"><span>Total Registered Students</span></div>
            <div class="col2">
                    <% using (Html.BeginForm("GetReport", "Report", FormMethod.Get)) { %>
                        <a href="#" class="submit_anchor button"><span class="ui-icon ui-icon-disk"></span>Download</a>
                        <%= Html.Hidden("Report", ReportController.Report.TotalRegisteredStudents) %>
                        <%= Html.Hidden("termCode", Model.TermCode.Id, new {@class = "term_value"}) %>
                    <% } %>
            </div>
            </div>
            
            <div class="description">
                List of all registered students for the selected term.
            </div>

        </div>

        <div class="report">
            
            <div class="title">
                <div class="col1">Total Registration By Major</div>
                <div class="col2">
                    <% using (Html.BeginForm("GetReport", "Report", FormMethod.Get)) { %>
                        <img src="<%: Url.Content("~/Images/ajax-loader.gif") %>" id="totalreg-loader" style="display: none;"/>
                        <%= this.Select("majorCode").Options(Model.MajorCodes,x=>x.Id,x=>x.Name).FirstOption("--Select a Major") %>
                        <a href="#" class="submit_anchor button"><span class="ui-icon ui-icon-disk"></span>Download</a>
                        <%= Html.Hidden("Report", ReportController.Report.TotalRegisteredByMajor) %>
                        <%= Html.Hidden("termCode", Model.TermCode.Id, new {@class="term_value"}) %>
                        
                    <% } %>
                </div>
            </div>

            <div class="description">n/a</div>

        </div>

        <div class="report">
            
            <div class="title">
                <div class="col1">Sum of All Tickets</div>
                <div class="col2">
                    <% using (Html.BeginForm("GetReport", "Report", FormMethod.Get)) { %>
                        <a href="#" class="submit_anchor button"><span class="ui-icon ui-icon-disk"></span>Download</a>
                        <%= Html.Hidden("Report", ReportController.Report.SumOfAllTickets) %>
                        <%= Html.Hidden("termCode", Model.TermCode.Id, new {@class = "term_value"}) %>
                    <% } %>    
                </div>
            </div>
            
            <div class="description">Summary report of all tickets.</div>

        </div>
        
        <div class="report">
            
            <div class="title">
                <div class="col1">Neulion Ticket Reports</div>
                <div class="col2">
                    <%= Html.ActionLink<TicketController>(a => a.Index(), "View", new { @class="button" })%>
                </div>
            </div>
            
            <div class="description">List of excel reports available for Neulion Tickets.</div>

        </div>
        
        <div class="report">
            
            <div class="title">
                <div class="col1">Special Needs Request</div>
                <div class="col2">
                    <% using (Html.BeginForm("GetReport", "Report", FormMethod.Get)) { %>
                        <a href="#" class="submit_anchor button"><span class="ui-icon ui-icon-disk"></span>Download</a>
                        <%= Html.Hidden("Report", ReportController.Report.SpecialNeedsRequest) %>
                        <%= Html.Hidden("termCode", Model.TermCode.Id, new {@class = "term_value"}) %>
                    <% } %>
                </div>
            </div>
            
            <div class="description">Special needs report for the selected term.</div>

        </div>
        
        <div class="report">
            
            <div class="title">
                <div class="col1">Registrars Report</div>
                <div class="col2">
                    <% using (Html.BeginForm("GetReport", "Report", FormMethod.Get)) { %>
                        <a href="#" class="submit_anchor button"><span class="ui-icon ui-icon-disk"></span>Download</a>
                        <%= Html.Hidden("Report", ReportController.Report.RegistrarsReport) %>
                        <%= Html.Hidden("termCode", Model.TermCode.Id, new {@class = "term_value"}) %>
                    <% } %>
                </div>
            </div>
            
            <div class="description">Registrar's Report for the selected term.</div>

        </div>
        
        <div class="report">
            
            <div class="title">
                <div class="col1">Ticket Sign Out Sheet</div>
                <div class="col2">                  
                    <% using (Html.BeginForm("GetReport", "Report", FormMethod.Get)) { %>
                        <a href="#" class="submit_anchor button"><span class="ui-icon ui-icon-disk"></span>Download</a>
                        <%= Html.Hidden("Report", ReportController.Report.TicketSignOutSheet) %>
                        <%= Html.Hidden("termCode", Model.TermCode.Id, new {@class = "term_value"}) %>
                    <% } %>
                </div>
            </div>
            
            <div class="description">Ticket sign out sheet for the selected term.</div>

        </div>
        
        <div class="report">
            
            <div class="title">
                <div class="col1">Major Count by Ceremony</div>
                <div class="col2">
                    <% using (Html.BeginForm("GetReport", "Report", FormMethod.Get)) { %>
                        <img src="<%: Url.Content("~/Images/ajax-loader.gif") %>" id="majorcount-loader" style="display: none;"/>
                        <%= this.Select("ceremony").Options(Model.Ceremonies,x=>x.Id,x=>string.Format("{0} ({1})", x.CeremonyName, x.DateTime.ToString("g")) ).FirstOption("--Select a Ceremony--") %>
                        <a href="#" class="submit_anchor button"><span class="ui-icon ui-icon-disk"></span>Download</a>
                        <%= Html.Hidden("Report", ReportController.Report.MajorCountByCeremony)%>
                        <%= Html.Hidden("termCode", Model.TermCode.Id, new {@class = "term_value"}) %>        
                    <% } %>
                </div>
            </div>
            
            <div class="description">n/a</div>

        </div>
        
        <div class="report">
            
            <div class="title">
                <div class="col1">Registration Data</div>
                <div class="col2">
                    <%= Html.ActionLink<ReportController>(a => a.RegistrationData(), "View", new { @class="button" })%>
                </div>
            </div>
            
            <div class="description">Statistics of all past and present terms broken down by ceremony.</div>

        </div>
        
        <div class="report">
            
            <div class="title">
                <div class="col1">Honors Report</div>
                <div class="col2">
                    <a href="<%= Url.Action("Honors", "Report") %>" class="submit_anchor button"><span class="ui-icon ui-icon-disk"></span>Download</a>
                </div>
            </div>
            
            <div class="description">Honors report for the given term</div>

        </div>
        
        <div class="report">
            <div class="title">
                <div class="col1">Registration Major Mismatch Report</div>
                <div class="col2">
                    <% using (Html.BeginForm("GetReport", "Report", FormMethod.Get)) { %>
                        <img src="<%: Url.Content("~/Images/ajax-loader.gif") %>" id="Img1" style="display: none;"/>
                        <a href="#" class="submit_anchor button"><span class="ui-icon ui-icon-disk"></span>Download</a>
                        <%= Html.Hidden("Report", ReportController.Report.RegistartionMajorMismatch)%>
                        <%= Html.Hidden("termCode", Model.TermCode.Id, new {@class = "term_value"}) %>        
                    <% } %>
                </div>
            </div>
            <div class="description">List of students who have a major registered for a ceremony that it does not belong to.</div>
        </div>

    </fieldset>
    
    <fieldset>
        
        <legend>Labels</legend>
        
        <div class="message ui-state-highlight">This will only print labels for the current term.</div>

        <div class="report">
            
            <div class="title">
                <div class="col1">Pending Mailing Labels</div>
                <div class="col2">
                    <%= Html.ActionLink<ReportController>(a => a.GenerateAveryLabels(TermService.GetCurrent().Id, true, false), "Download", new {@class="button"})%>
                </div>
            </div>
            
            <div class="description">
                This will print all pending labels for mailing that need to be printed and will update records so that they will not be printed in this list again.
            </div>

        </div>
        
        <div class="report">
            
            <div class="title">
                <div class="col1">Pending Pickup Labels</div>
                <div class="col2">
                    <%= Html.ActionLink<ReportController>(a => a.GenerateAveryLabels(TermService.GetCurrent().Id, false, false), "Download", new {@class="button"})%>
                </div>
            </div>
            
            <div class="description">
                This will print all pending labels for pickup that need to be printed and will update records so that they will not be printed in this list again.
            </div>

        </div>
        
        <div class="report">
            
            <div class="title">
                <div class="col1">All Mailing Labels</div>
                <div class="col2">
                    <%= Html.ActionLink<ReportController>(a => a.GenerateAveryLabels(TermService.GetCurrent().Id, true, true), "Download", new {@class="button"})%>
                </div>
            </div>
            
            <div class="description">
                This will print all mailing labels for the current term, regardless of whether they have been printed already.
            </div>

        </div>
        
        <div class="report">
            
            <div class="title">
                <div class="col1">All Pickup Labels</div>
                <div class="col2">
                    <%= Html.ActionLink<ReportController>(a => a.GenerateAveryLabels(TermService.GetCurrent().Id, false, true), "Download", new {@class="button"})%>
                </div>
            </div>
            
            <div class="description">
                This will print all pickup labels for the current term, regardless of whether they have been printed already.
            </div>

        </div>        

    </fieldset>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    <script type="text/javascript">

        var majorUrl = '<%: Url.Action("LoadMajorsForTerm") %>';
        var ceremonyUrl = '<%: Url.Action("LoadCeremoniesForTerm") %>';

        $(function () {
            $(".submit_anchor").click(function () { $(this).parent("form").submit(); });
            $("#termCode").change(function () {
                // set all the term values
                $(".term_value").val($("#termCode").val());

                // show the ajax loaders
                $("#totalreg-loader").show();
                $("#majorcount-loader").show();

                // reload all the drop downs
                $.getJSON(majorUrl, { term: $("#termCode").val() }, function (result) {
                    var $select = $("select#majorCode");

                    LoadOptions($select, result);
                });

                $.getJSON(ceremonyUrl, { term: $("#termCode").val() }, function (result) {
                    var $select = $("select#ceremony");

                    LoadOptions($select, result);
                });
            });
        });
        
        function LoadOptions($select, values) {

            // remove all the children, except the initial one
            $select.find("option").not("option[value='']").remove();

            $.each(values, function (index, item) {
                var option = $("<option>").val(item.Id).html(item.Name);
                $select.append(option);
            });

            // hide the loader image
            $select.siblings("img").hide();

        }

    </script>
    
    <style type="text/css">
        .report { padding: .5em;margin-bottom: 1em;}
        .report .title {border-bottom: 1px solid #00497B; padding-bottom: .5em;margin-bottom: 1em;}
        .report .title .col1 {display: inline-block; width: 29%; font-weight: bold;height: 21px;position: relative;color: #00497B;}
        .report .title .col1 span { position: absolute;top: 50%;}
        .report .title .col2 {display: inline-block; width: 70%; text-align: right;height: 35px;} 
        
        .ui-icon { display: inline-block;margin-right: 5px;}
        .ui-button-text { font-weight: normal;}
    </style>

</asp:Content>
