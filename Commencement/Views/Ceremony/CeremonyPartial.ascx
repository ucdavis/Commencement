<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Commencement.Controllers.ViewModels.CeremonyViewModel>" %>

    <script src="<%= Url.Content("~/Scripts/jquery.ui.datetimepicker.min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/ui.multiselect.js") %>" type="text/javascript"></script>
    
    <script type="text/javascript">
        $(function() {
            //$("#Ceremony_DateTime").datetimepicker();
            $("#Ceremony_RegistrationDeadline").datepicker();

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
            <strong>Date/Time:</strong>
            <%= Html.TextBoxFor(x=>x.Ceremony.DateTime) %>
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
            <strong>Registration Deadline:</strong>
            <%= Html.TextBoxFor(x => x.Ceremony.RegistrationDeadline.ToString("d"))%>
            <%= Html.ValidationMessageFor(x=>x.Ceremony.RegistrationDeadline) %>
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
