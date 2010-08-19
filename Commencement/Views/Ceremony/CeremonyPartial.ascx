<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Commencement.Controllers.ViewModels.CeremonyViewModel>" %>

    <%--<script src="<%= Url.Content("~/Scripts/jquery.ui.datetimepicker.min.js") %>" type="text/javascript"></script>--%>
    <link href="<%= Url.Content("~/Content/anytimec.css") %>" type="text/css" rel="Stylesheet" />
    <link href="<%= Url.Content("~/Content/ui.multiselect.css") %>" type="text/css" rel="Stylesheet" />
    
    <script src="<%= Url.Content("~/Scripts/jquery.anytime.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/ui.multiselect.js") %>" type="text/javascript"></script>
    
    <script type="text/javascript">
        $(function() {
            //$("#Ceremony_DateTime").AnyTime_picker();
            $("#CeremonyDate").datepicker();

            $(".ceremony_time").change(function() {
                var date = $("#CeremonyDate").val();
                var hour = $("#CeremonyHour").val();
                var minute = $("#CeremonyMinutes").val();
                var ampm = $("#CeremonyAmPm").val();

                $("#Ceremony_DateTime").val(date + " " + hour + ":" + minute + ":00 " + ampm);
            });

            $("#Ceremony_PrintingDeadline").datepicker();
            $("#Ceremony_RegistrationDeadline").datepicker();
            $("#Ceremony_ExtraTicketDeadline").datepicker();

            $("#CeremonyMajors").multiselect();
        });

    </script>

    <ul class="registration_form">
        <li>
            <strong>Term Code:</strong>
            <%= this.Select("term").Options(Model.TermCodes, x => x.Id, x => x.Description)
                    .FirstOption("--Select a Term--")
                    .Selected(Model.Ceremony.TermCode != null ? Model.Ceremony.TermCode.Id : string.Empty)
                %>
        </li>
        <li>
            <strong>Date/Time of Ceremony:</strong>
            <%= Html.TextBox("CeremonyDate", Model.Ceremony.DateTime.ToString("d"), new {@class="ceremony_time"}) %>
            
            <select id="CeremonyHour" class="ceremony_time">
                <% for (int i = 1; i < 13; i++ ) { %>
                    <% var flag = Model.Ceremony.DateTime.Hour == i; %>
                    <option value="<%= i %>" "<%= flag ? "selected" : string.Empty %>"><%= i %></option>
                <% } %>
            </select>
            
            <select id="CeremonyMinutes" class="ceremony_time">
                <option value="00" "<%= Model.Ceremony.DateTime.Minute == 0 ? "selected" : string.Empty %>">00</option>
                <option value="30" "<%= Model.Ceremony.DateTime.Minute == 30 ? "selected" : string.Empty %>">30</option>
            </select>
            
            <select id="CeremonyAmPm" class="ceremony_time">
                <option value="AM" "<%= Model.Ceremony.DateTime.ToString("tt") == "AM" ? "selected" : string.Empty %>" >AM</option>
                <option value="PM" "<%= Model.Ceremony.DateTime.ToString("tt") == "PM" ? "selected" : string.Empty %>" >PM</option>
            </select>
            
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
            <strong>Total Tickets:</strong>
            <%= Html.TextBoxFor(x=>x.Ceremony.TotalTickets) %>
            <%= Html.ValidationMessageFor(x=>x.Ceremony.TotalTickets) %>
            
        </li>
        <li>
            <strong>Program Printing Deadline:</strong>
            <%= Html.TextBoxFor(x=>x.Ceremony.PrintingDeadline.ToString("d")) %>
            <%= Html.ValidationMessageFor(x=>x.Ceremony.PrintingDeadline) %>
            * Registration will continue to be open past this date.
        </li>
        <li>
            <strong>Registration Closure:</strong>
            <%= Html.TextBoxFor(x => x.Ceremony.RegistrationDeadline.ToString("d"))%>
            <%= Html.ValidationMessageFor(x=>x.Ceremony.RegistrationDeadline) %>
            * Registration will be blocked after this date.
        </li>
        <li>
            <strong>Extra Ticket Request Deadline:</strong>
            <%= Html.TextBoxFor(x=>x.Ceremony.ExtraTicketDeadline.ToString("d")) %>
            <%= Html.ValidationMessageFor(x=>x.Ceremony.ExtraTicketDeadline) %>
            * Last date to accept extra ticket requests
        </li>
        <li>
            <strong>Majors:</strong>
            <span>
                <%= Html.ListBox("CeremonyMajors", Model.Majors, new {style="width:700px"})%>
            </span>
        </li>
        
        <li>
            <strong></strong>
            <input type="submit" value="Submit" />
        </li>
    </ul>
