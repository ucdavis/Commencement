<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Commencement.Controllers.ViewModels.CeremonyViewModel>" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>

    <link href="<%= Url.Content("~/Content/anytimec.css") %>" type="text/css" rel="Stylesheet" />
    <link href="<%= Url.Content("~/Content/ui.multiselect.css") %>" type="text/css" rel="Stylesheet" />
    
    <script src="<%= Url.Content("~/Scripts/jquery.anytime.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/ui.multiselect.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.livequery.js") %>" type="text/javascript"></script>
    <script type="text/javascript" src='<%: Url.Content("~/Scripts/jquery.bt.min.js") %>'></script>
    <link href="<%= Url.Content("~/Content/jquery.bt.css") %>" rel="Stylesheet" type="text/css" media="screen" />

    <script type="text/javascript">
        $(function () {
            //$("#Ceremony_DateTime").AnyTime_picker();
            $("#CeremonyDate").datepicker();

            $(".ceremony_time").change(function () {
                var date = $("#CeremonyDate").val();
                var hour = $("#CeremonyHour").val();
                var minute = $("#CeremonyMinutes").val();
                var ampm = $("#CeremonyAmPm").val();

                $("#Ceremony_DateTime").val(date + " " + hour + ":" + minute + ":00 " + ampm);
            });

            $("#Ceremony_PrintingDeadline").datepicker();
            $("#Ceremony_RegistrationBegin").datepicker();
            $("#Ceremony_RegistrationDeadline").datepicker();
            $("#Ceremony_ExtraTicketBegin").datepicker();
            $("#Ceremony_ExtraTicketDeadline").datepicker();

            $("#CeremonyMajors").multiselect();



            $(".college").change(function () { GetMajors(); });
        });

        function GetMajors() {
            var url = '<%= Url.Action("GetMajors", "Ceremony") %>';
            $.each($(".college:checked"), function (index, item) {
                if (index == 0) url = url + "?colleges=" + $(item).val();
                else url = url + "&colleges=" + $(item).val();

            });

            $.getJSON(url, function (result) {

                if (result == null || result.length <= 0) $("li.node").remove();
                else {
                    var $select = $("<select>").addClass("newData").attr("name", "CeremonyMajors").attr("id", "CeremonyMajors").attr("multiple", "multiple").hide();

                    $.each(result, function (index, item) {
                        var option = $("<option>").val(item.Id).html(item.Name);
                        $select.append(option);
                    });

                    $select.insertAfter($("#CeremonyMajors"));
                    $("#CeremonyMajors").remove();

                    var $available = $select.siblings("div").find("ul.available").css("height", "280px");
                    var $selected = $select.siblings("div").find("ul.selected").css("height", "280px");

                    $select.siblings("div").css("width", "920px");
                    $select.siblings("div").find(".selected").css("width", "450px");
                    $select.siblings("div").find(".available").css("width", "450px");
                }
            });
            
        }

    </script>
    <script type="text/javascript">
        function myCustomOnChangeHandler(inst) {
            //            alert("Some one modified something");
            //            alert("The HTML is now:" + inst.getBody().innerHTML);
            tinyMCE.triggerSave(); //Needed because of client side validation
        }
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#Ceremony_ConfirmationText").enableTinyMce({ script_location: '<%= Url.Content("~/Scripts/tiny_mce/tiny_mce.js") %>', overrideWidth: '900', overrideHeight: '250', overrideOnchange: 'myCustomOnChangeHandler' }); //, overrideShowPreview: 'preview,', overridePlugin_preview_pageurl: '<%= Url.Content("~/Static/Preview.html") %>' });
            $.each($(".showBtRight"), function (index, item) {
                $(item).bt({ trigger: ['focus', 'blur'], positions: ['right'] });
            });
            $.each($(".showBtTop"), function (index, item) {
                $(item).bt({ trigger: ['focus', 'blur'], positions: ['top'] });
            });
        });
       
    </script>

    <ul class="registration_form">
        <li>
            <strong>Term Code:</strong> 
            <%: Html.Hidden("Term", Model.TermCode.Id) %>
            <%: Model.TermCode.Name %>
        </li>
        <li>
            <strong>Date/Time of Ceremony:</strong>
            <%= Html.TextBox("CeremonyDate", Model.Ceremony.DateTime.ToString("d"), new {@class="ceremony_time"}) %>
            
            <%= this.Select("CeremonyHour").Class("ceremony_time").Options(Model.Hours,x=>x,x=>x).Selected(Model.Ceremony.DateTime.Hour > 12 ? Model.Ceremony.DateTime.Hour - 12 : Model.Ceremony.DateTime.Hour)%>

            <%= this.Select("CeremonyMinutes").Class("ceremony_time").Options(Model.Minutes,x=>x,x=>x).Selected(Model.Ceremony.DateTime.Minute) %>

            <%= this.Select("CeremonyAmPm").Class("ceremony_time").Options(Model.AmPm).Selected(Model.Ceremony.DateTime.ToString("tt")) %>

<%--            <select id="CeremonyHour" class="ceremony_time">
                <% for (int i = 1; i < 13; i++ ) { %>
                    <% var hour = Model.Ceremony.DateTime.Hour > 12 ? Model.Ceremony.DateTime.Hour - 12 : Model.Ceremony.DateTime.Hour;
                       var flag = hour == i;
                       var selected = flag ? "selected" : string.Empty;
                       %>
                    <option value="<%= i %>" '<%= selected %>' ><%: i %></option>

                    
                <% } %>
            </select>
            
            <select id="CeremonyMinutes" class="ceremony_time">
                <option value="00" "<%= Model.Ceremony.DateTime.Minute == 0 ? "selected" : string.Empty %>">00</option>
                <option value="30" "<%= Model.Ceremony.DateTime.Minute == 30 ? "selected" : string.Empty %>">30</option>
            </select>
            
            <select id="CeremonyAmPm" class="ceremony_time">
                <option value="AM" "<%= Model.Ceremony.DateTime.ToString("tt") == "AM" ? "selected" : string.Empty %>" >AM</option>
                <option value="PM" "<%= Model.Ceremony.DateTime.ToString("tt") == "PM" ? "selected" : string.Empty %>" >PM</option>
            </select>--%>
            
            <%= Html.HiddenFor(x => x.Ceremony.DateTime)%>
            <%= Html.ValidationMessageFor(x=>x.Ceremony.DateTime) %>
        </li>
        <li>
            <strong>Location:</strong>
            <%= Html.TextBoxFor(x=>x.Ceremony.Location) %>
            <%= Html.ValidationMessageFor(x=>x.Ceremony.Location) %>
        </li>
        <li>
            <strong>Tickets per Student:</strong>
            <%= Html.TextBoxFor(x=>x.Ceremony.TicketsPerStudent) %>
            <%= Html.ValidationMessageFor(x=>x.Ceremony.TicketsPerStudent) %>
        </li>
        <li>
            <strong>Extra Ticket Request Max Per Student</strong>
            <%= Html.TextBoxFor(x=>x.Ceremony.ExtraTicketPerStudent) %>
            <%= Html.ValidationMessageFor(x=>x.Ceremony.ExtraTicketPerStudent) %>
        </li>
        <li>
            <strong>Total Tickets:</strong>
            <%= Html.TextBoxFor(x => x.Ceremony.TotalTickets, new { @class = "showBtRight", @title = "Total number of tickets available to everyone." })%>
            <%= Html.ValidationMessageFor(x=>x.Ceremony.TotalTickets) %>            
        </li>
        <li>
            <strong>Total Streaming Tickets:</strong>
            <%: Html.TextBoxFor(x => x.Ceremony.TotalStreamingTickets, new { @class = "showBtRight", @title = "Total number of tickets for streaming tickets" })%>
            <%: Html.ValidationMessageFor(x=>x.Ceremony.TotalStreamingTickets) %>
        </li>
        <li>
            <strong>Minimum Units:</strong>
            <%: Html.TextBoxFor(x => x.Ceremony.MinUnits, new { @class = "showBtRight", @title = "Minimum number of units required to register." })%>
            <%: Html.ValidationMessageFor(x=>x.Ceremony.MinUnits) %>
        </li>
        <li>
            <strong>Petition Threshold:</strong>
            <%: Html.TextBoxFor(x => x.Ceremony.PetitionThreshold, new { @class = "showBtRight", @title = "Minimum number of units to be allowed to submit registration petition" })%>
            <%: Html.ValidationMessageFor(x=>x.Ceremony.PetitionThreshold) %>
        </li>
        <li>
            <strong>Registration Begin:</strong>
            <%: Html.TextBox("Ceremony.RegistrationBegin", Model.Ceremony.RegistrationBegin.ToString("d")) %>
            <%: Html.ValidationMessageFor(x=>x.Ceremony.RegistrationBegin) %>
        </li>
        <li>
            <strong>Registration Closure:</strong>
            <%: Html.TextBox("Ceremony.RegistrationDeadline", Model.Ceremony.RegistrationDeadline.ToString("d"), new { @class = "showBtTop", @title = "Registration will be blocked after this date." })%>
            <%= Html.ValidationMessageFor(x=>x.Ceremony.RegistrationDeadline) %>
        </li>
        <li>
            <strong>Program Printing Deadline:</strong>
            <%: Html.TextBox("Ceremony.PrintingDeadline", Model.Ceremony.PrintingDeadline.ToString("d"), new { @class = "showBtTop", @title = "Registration will continue to be open past this date." })%>
            <%= Html.ValidationMessageFor(x=>x.Ceremony.PrintingDeadline) %>
        </li>
        <li>
            <strong>Extra Ticket Request Begin:</strong>
            <%: Html.TextBox("Ceremony.ExtraTicketBegin", Model.Ceremony.ExtraTicketBegin.ToString("d")) %>
            <%: Html.ValidationMessageFor(x=>x.Ceremony.ExtraTicketBegin) %>
        </li>
        <li>
            <strong>Extra Ticket Request Deadline:</strong>
            <%: Html.TextBox("Ceremony.ExtraTicketDeadline", Model.Ceremony.ExtraTicketDeadline.ToString("d"), new { @class = "showBtTop", @title = "Last date to accept extra ticket requests" })%>
            <%= Html.ValidationMessageFor(x=>x.Ceremony.ExtraTicketDeadline) %>
        </li>
        <li>
            <strong><%: Html.LabelFor(a => a.Ceremony.ConfirmationText, DisplayOptions.HumanizeAndColon) %></strong>
            <%: Html.TextAreaFor(a => a.Ceremony.ConfirmationText, new {@style="width:900px;"}) %>
            <%= Html.ValidationMessageFor(a => a.Ceremony.ConfirmationText) %>
        </li>
        <li>
            <strong>Colleges:</strong>

            <%= this.CheckBoxList("Colleges").Options(Model.Colleges).ItemClass("college") %>
        </li>

        <li>
            <% if (Model.Majors != null) { %>
            <%= Html.ListBox("CeremonyMajors", Model.Majors, new {style="width:900px; height: 300px;"}) %>
            <% } else { %>
            <select id="CeremonyMajors" style="width: 200px;" name="CeremonyMajors" multiple="multiple"></select>
            <% } %>
        </li>
        
        <li>
            <strong></strong>
            <input type="submit" value="Submit" />
        </li>
    </ul>
