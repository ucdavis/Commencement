<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Commencement.Controllers.ViewModels.CeremonyViewModel>" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>

    <link href="<%= Url.Content("~/Content/ui.multiselect.css") %>" type="text/css" rel="Stylesheet" />
    
    <script src="<%= Url.Content("~/Scripts/ui.multiselect.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.livequery.js") %>" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            $(".ceremony_time").change(function () {
                var date = $("#CeremonyDate").val();
                var hour = $("#CeremonyHour").val();
                var minute = $("#CeremonyMinutes").val();
                var ampm = $("#CeremonyAmPm").val();

                $("#Ceremony_DateTime").val(date + " " + hour + ":" + minute + ":00 " + ampm);
            });

            $("#CeremonyMajors").multiselect();

            $(".college").change(function () { GetMajors(); });
        });

        function GetMajors() {
            
            var url = '<%= Url.Action("GetMajors", "Ceremony") %>';
            $.each($(".college:checked"), function (index, item) {
                if (index == 0) url = url + "?colleges=" + $(item).val();
                else url = url + "&colleges=" + $(item).val();

            });

            // maintain the list of selected majors
            var options = $("#CeremonyMajors option:selected");
            var selected = $.map(options, function (item, index) {
                return $(item).val();
            });
            $.getJSON(url, function (result) {

                if (result == null || result.length <= 0) $("li.node").remove();
                else {
                    //var $select = $("<select>").addClass("newData").attr("name", "CeremonyMajors").attr("id", "CeremonyMajors").attr("multiple", "multiple").hide();

                    var $select = $("<select>").attr("id", "CeremonyMajors").attr("name", "CeremonyMajors").attr("multiple", "multiple");
                    $select.css("width", "900px").css("height", "300px").hide();

                    $.each(result, function (index, item) {
                        var option = $("<option>").val(item.Id).html(item.Name);

                        if ($.inArray(item.Id, selected) != -1) {
                            option.attr("selected", "selected");
                        }

                        $select.append(option);
                    });

                    $select.insertAfter($("#CeremonyMajors"));
                    $("#CeremonyMajors").remove();

                    var $available = $select.siblings("div").find("ul.available").css("height", "280px");
                    var $selected = $select.siblings("div").find("ul.selected").css("height", "280px");

                    $select.siblings("div").css("width", "920px");
                    $select.siblings("div").find(".selected").css("width", "450px");
                    $select.siblings("div").find(".available").css("width", "450px");

                    $select.multiselect();
                }
            });
            
        }

    </script>
    <script type="text/javascript">
        function myCustomOnChangeHandler(inst) {
            tinyMCE.triggerSave(); //Needed because of client side validation
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#Ceremony_ConfirmationText").enableTinyMce({ script_location: '<%= Url.Content("~/Scripts/tiny_mce/tiny_mce.js") %>', overrideWidth: '900', overrideHeight: '250', overrideOnchange: 'myCustomOnChangeHandler' });
        });
    </script>

    <style type="text/css">
        .ceremony_time {min-width: 60px;}
    </style>

    <fieldset id="details">
    
        <legend>Ceremony Details</legend>

        <ul class="registration_form">
            <li>
                <strong><%: Html.LabelFor(a => a.Ceremony.TermCode, DisplayOptions.HumanizeAndColon)%></strong> 
                <%: Html.Hidden("Term", Model.TermCode.Id) %>
                <%: Model.TermCode.Name %>
            </li>
            <li><strong><%: Html.LabelFor(a => a.Ceremony.Name, DisplayOptions.HumanizeAndColon) %></strong>
                <%: Html.TextBoxFor(a => a.Ceremony.Name, new {@class="hastip", title="Name to be displayed, if this is left blank will display 'Commencement for MajorName'"}) %>
                <%: Html.ValidationMessageFor(x => x.Ceremony.Name) %>
            </li>
            <li>
                <strong><%: Html.LabelFor(a => a.Ceremony.DateTime, DisplayOptions.HumanizeAndColon) %><span>*</span></strong>
                <%= Html.TextBox("CeremonyDate", Model.Ceremony.DateTime.ToString("d"), new {@class="ceremony_time date"}) %>
            
                <%= this.Select("CeremonyHour").Class("ceremony_time").Options(Model.Hours,x=>x,x=>x).Selected(Model.Ceremony.DateTime.Hour > 12 ? Model.Ceremony.DateTime.Hour - 12 : Model.Ceremony.DateTime.Hour)%>

                <%= this.Select("CeremonyMinutes").Class("ceremony_time").Options(Model.Minutes,x=>x,x=>x).Selected(Model.Ceremony.DateTime.Minute) %>

                <%= this.Select("CeremonyAmPm").Class("ceremony_time").Options(Model.AmPm).Selected(Model.Ceremony.DateTime.ToString("tt")) %>
          
                <%= Html.HiddenFor(x => x.Ceremony.DateTime)%>
                <%= Html.ValidationMessageFor(x=>x.Ceremony.DateTime) %>
            </li>
            <li>
                <strong><%: Html.LabelFor(a => a.Ceremony.Location, DisplayOptions.HumanizeAndColon) %><span>*</span></strong>
                <%= Html.TextBoxFor(x=>x.Ceremony.Location) %>
                <%= Html.ValidationMessageFor(x=>x.Ceremony.Location) %>
            </li>
            <li>
                <strong><%: Html.LabelFor(a => a.Ceremony.MinUnits, DisplayOptions.HumanizeAndColon) %><span>*</span></strong>
                <%: Html.TextBoxFor(x => x.Ceremony.MinUnits, new { @class = "hastip", @title = "Minimum number of units required to register." })%>
                <%: Html.ValidationMessageFor(x=>x.Ceremony.MinUnits) %>
            </li>
            <li>
                <strong><%: Html.LabelFor(a => a.Ceremony.PetitionThreshold, DisplayOptions.HumanizeAndColon) %><span>*</span></strong>
                <%: Html.TextBoxFor(x => x.Ceremony.PetitionThreshold, new { @class = "hastip", @title = "Minimum number of units to be allowed to submit registration petition" })%>
                <%: Html.ValidationMessageFor(x=>x.Ceremony.PetitionThreshold) %>
            </li>
        </ul>

    </fieldset>
    
    <fieldset>
        
        <legend>Website Links</legend>
        
        <ul class="registration_form">
            <li><strong><%: Html.LabelFor(a => a.Ceremony.WebsiteUrl, DisplayOptions.HumanizeAndColon) %></strong>
                <%: Html.TextBoxFor(a => a.Ceremony.WebsiteUrl, new {@class="hastip", title="Url to an informational web page for Commencement."}) %>                
                <%: Html.ValidationMessageFor(a => a.Ceremony.WebsiteUrl) %>
            </li>
            <li><strong><%: Html.LabelFor(a => a.Ceremony.SurveyUrl, DisplayOptions.HumanizeAndColon) %></strong>
                <%: Html.TextBoxFor(a => a.Ceremony.SurveyUrl, new {@class="hastip", title="Url to a survey, that will redirect students to fill out."}) %>
                <%: Html.ValidationMessageFor(a => a.Ceremony.SurveyUrl) %>
            </li>
            <li><strong>-- OR Internal Survey--</strong></li>
            <li><strong>Survey:</strong>
                <%= this.Select("Ceremony.Survey").Options(Model.Surveys,x=>x.Id, x=>x.Name).FirstOption("--Select a Survey--").Selected(Model.Ceremony.Survey != null ? Model.Ceremony.Survey.Id.ToString() : string.Empty) %>
            </li>
        </ul>

    </fieldset>

    <fieldset id="tickets">
    
        <legend>Tickets</legend>

        <ul class="registration_form">
            <li>
                <strong><%: Html.LabelFor(a => a.Ceremony.TicketsPerStudent, DisplayOptions.HumanizeAndColon) %><span>*</span></strong>
                <%= Html.TextBoxFor(x => x.Ceremony.TicketsPerStudent, new { @class="hastip", @title = "Maximum # of tickets a student can request." })%>
                <%= Html.ValidationMessageFor(x=>x.Ceremony.TicketsPerStudent) %>
            </li>
            <li>
                <strong><%: Html.LabelFor(a => a.Ceremony.ExtraTicketPerStudent, DisplayOptions.HumanizeAndColon) %><span>*</span></strong>
                <%= Html.TextBoxFor(x => x.Ceremony.ExtraTicketPerStudent, new { @class="hastip", @title = "Maximum # of extra tickets a student can petition for." })%>
                <%= Html.ValidationMessageFor(x=>x.Ceremony.ExtraTicketPerStudent) %>
            </li>
            <li>
                <strong><%: Html.LabelFor(a => a.Ceremony.TotalTickets, DisplayOptions.HumanizeAndColon) %><span>*</span></strong>
                <%= Html.TextBoxFor(x => x.Ceremony.TotalTickets, new { @class = "hastip", @title = "Total number of tickets available to everyone." })%>
                <%= Html.ValidationMessageFor(x=>x.Ceremony.TotalTickets) %>            
            </li>
            <li>
                <strong><%: Html.LabelFor(a => a.Ceremony.TotalStreamingTickets, DisplayOptions.HumanizeAndColon) %></strong>
                <%: Html.TextBoxFor(x => x.Ceremony.TotalStreamingTickets, new { @class = "hastip", @title = "Total number of tickets for streaming tickets" })%>
                <%: Html.ValidationMessageFor(x=>x.Ceremony.TotalStreamingTickets) %>
            </li>        
        </ul>

    </fieldset>

    <fieldset id="dates">
    
        <legend>Dates</legend>

        <ul class="registration_form">
            <li>
                <strong>Program Deadline:<span>*</span></strong>
                <%: Html.TextBox("Ceremony.PrintingDeadline", Model.Ceremony.PrintingDeadline.ToString("d"), new { @class = "hastip date", @title = "Deadline for the program printing.  A warning will come up that says registering after this date may result in name not appearing in program." })%>
                <%= Html.ValidationMessageFor(x=>x.Ceremony.PrintingDeadline) %>
            </li>
            <li><strong>Extra Ticket Requests:<span>*</span></strong>
                <%: Html.TextBox("Ceremony.ExtraTicketBegin", Model.Ceremony.ExtraTicketBegin.ToString("d"), new { @class="hastip date", @title = "Extra ticket requests will begin on this date." })%>
                <%: Html.ValidationMessageFor(x=>x.Ceremony.ExtraTicketBegin) %>
                through
                <%: Html.TextBox("Ceremony.ExtraTicketDeadline", Model.Ceremony.ExtraTicketDeadline.ToString("d"), new { @class = "hastip date", @title = "Last date to accept extra ticket requests" })%>
                <%= Html.ValidationMessageFor(x=>x.Ceremony.ExtraTicketDeadline) %>
            </li>       
        </ul>

    </fieldset>

    <fieldset id="ticket-distribution">
    
        <legend>Ticket Distribution Method(s)</legend>

        <% if (Model.TicketDistributionMethods != null) { %>
            <%= this.CheckBoxList("TicketDistributionMethods").Options(Model.TicketDistributionMethods).Class("radio_list") %>
        <% } else { %>
            <select id="TicketDistributionMethods" style="width: 200px;" name="TicketDistributionMethods" multiple="multiple"></select>
        <% } %>

    </fieldset>

    <fieldset id="confirmation-text">
    
        <legend><%: Html.LabelFor(a => a.Ceremony.ConfirmationText, DisplayOptions.HumanizeAndColon) %><span class="required">*</span></legend>

        <%: Html.TextAreaFor(a => a.Ceremony.ConfirmationText, new {@style="width:900px;"}) %>
        <%= Html.ValidationMessageFor(a => a.Ceremony.ConfirmationText) %>
    </fieldset>

    <fieldset id="College-Majors">
    
        <legend>College/Majors</legend>

            <strong>Colleges:</strong>

            <%= this.CheckBoxList("Colleges").Options(Model.Colleges).Class("radio_list").ItemClass("college") %>

            <strong>Majors:</strong>

            <% if (Model.Majors != null) { %>
            <%= Html.ListBox("CeremonyMajors", Model.Majors, new {style="width:900px; height: 300px;"}) %>
            <% } else { %>
            <select id="CeremonyMajors" style="width: 900px; height: 300px;" name="CeremonyMajors" multiple="multiple"></select>
            <% } %>

    </fieldset>
    <fieldset>
    <ul class="registration_form">
        <li>
            <strong></strong>
            <input type="submit" value="Save" class="button" />
            |
            <%: Html.ActionLink("Cancel", "Index", new { }, new { })%>
        </li>
    </ul>
    </fieldset>