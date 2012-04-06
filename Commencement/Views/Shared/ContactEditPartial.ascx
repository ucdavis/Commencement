<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Commencement.Controllers.ViewModels.RegistrationModel>" %>

    <script type="text/javascript">
        $(function () {

            // list of davis zip codes
            var zips = ['95616', '95617', '95618'];

            $('#Registration_City, #Registration_Zip').blur(function () {
                var city = $('#Registration_City').val().toLowerCase();
                var zip = $('#Registration_Zip').val();

                if (city == "" || zip == "") return false;

                var isdavisZip = $.inArray(zip, zips) >= 0;
                var isdavisCity = city == 'davis';

                // davis zip is entered, ask if they want the city to be updated to davis
                if (isdavisZip && !isdavisCity) {
                    if (confirm("Would you like to update the city to davis?")) {
                        $("#Registration_City").val("Davis");
                    }
                }
                if (isdavisCity && !isdavisZip) {
                    alert("Your zip code does not appear to be a davis zip code.");
                }
            });

        });
    </script>

    <ul class="registration_form">
        <li><strong>Address Line 1:</strong>
            <%= Html.TextBoxFor(x => x.Registration.Address1)%>
            <%= Html.ValidationMessageFor(x=>x.Registration.Address1) %>
        </li>
        <li><strong>Address Line 2:</strong>
            <%= Html.TextBoxFor(x => x.Registration.Address2)%>
            <%= Html.ValidationMessageFor(x=>x.Registration.Address2) %>
        </li>
        <li><strong>City:</strong>
            <%= Html.TextBoxFor(x => x.Registration.City)%>
            <%= Html.ValidationMessageFor(x=>x.Registration.City) %>
        </li>
        <li><strong>State:</strong>
            <%= this.Select("Registration.State").Options(Model.States, x => x.Id, x => x.Name).Selected("CA")%>
        </li>
        <li><strong>Zip Code:</strong>
            <%= Html.TextBoxFor(x => x.Registration.Zip, new { maxlength = 5, size = 5 }) %>
            <%= Html.ValidationMessageFor(x=>x.Registration.Zip) %>
        </li>
        <li class="prefilled"><strong>Email Address:</strong> <span><%= Html.Encode(Model.Student.Email) %></span>
        </li>
        <li><strong>Secondary Email Address:</strong>
            <%= Html.TextBoxFor(x=>x.Registration.Email) %>
            <%= Html.ValidationMessageFor(x=>x.Registration.Email) %>
        </li>
<%--        <li>
            <strong>Ticket Distribution Method:</strong>
            <%= this.RadioButton("Registration.MailTickets").Id("rMailTickets").Value(true).LabelAfter("Mail tickets to the address above").Checked(Model.Registration.MailTickets) %>
        </li>
        <li>
            <strong></strong>
            <% if (Model.Ceremonies.Min(a=>a.PrintingDeadline) > DateTime.Now) { %>
                <%= this.RadioButton("Registration.MailTickets").Id("rPickupTickets").Value(false).LabelAfter("I will pick up my tickets at the Arc Ticket Office").Checked(Model.Registration.MailTickets == false) %>
            <% } else { %>
                <%= this.RadioButton("Registration.MailTickets").Id("rPickupTickets").Value(false).LabelAfter("I will pick up my tickets in person.  See web site for ticket pickup details.").Checked(Model.Registration.MailTickets == false) %>
                <a href="http://caes.ucdavis.edu/NewsEvents/Events/Commencement/frequently-asked-questions#when-will-i-receive">here</a>
            <% } %>
        </li>--%>
    </ul>