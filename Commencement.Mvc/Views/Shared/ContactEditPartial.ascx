<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Commencement.Mvc.Controllers.ViewModels.RegistrationModel>" %>

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
        <li><strong>Address Line 1:<span>*</span></strong>
            <%= Html.TextBoxFor(x => x.Registration.Address1, new { maxlength = 200 })%>
            <%= Html.ValidationMessageFor(x=>x.Registration.Address1) %>
        </li>
        <li><strong>Address Line 2:</strong>
            <%= Html.TextBoxFor(x => x.Registration.Address2, new { maxlength = 200 })%>
            <%= Html.ValidationMessageFor(x=>x.Registration.Address2) %>
        </li>
        <li><strong>City:<span>*</span></strong>
            <%= Html.TextBoxFor(x => x.Registration.City, new { maxlength = 100 })%>
            <%= Html.ValidationMessageFor(x=>x.Registration.City) %>
        </li>
        <li><strong>State:<span>*</span></strong>
            <%= this.Select("Registration.State").Options(Model.States, x => x.Id, x => x.Name).Selected("CA")%>
        </li>
        <li><strong>Zip Code:<span>*</span></strong>
            <%= Html.TextBoxFor(x => x.Registration.Zip, new { maxlength = 15, size = 5 }) %>
            <%= Html.ValidationMessageFor(x=>x.Registration.Zip) %>
        </li>
        <li class="prefilled"><strong>Email:</strong>
            <span><%= Html.Encode(Model.Student.Email) %></span>
        </li>
        <li><strong>Additional Recipients Email:</strong>
            <%= Html.TextBoxFor(x=>x.Registration.Email, new { maxlength = 100}) %>
            <%= Html.ValidationMessageFor(x=>x.Registration.Email) %>
            <span>You can list a family member or others email address to receive copies of all your commencement email notifications.</span>
        </li>
        <li><strong>How To Say Your Name Phonetically:</strong>
            <%= Html.TextBoxFor(x=>x.Registration.Phonetic, new { maxlength = 150}) %>
            <%= Html.ValidationMessageFor(x=>x.Registration.Phonetic) %>
        </li>
    </ul>