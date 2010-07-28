<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Commencement.Controllers.ViewModels.RegistrationModel>" %>
<%@ Import Namespace="Resources" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>

    <input type="hidden" name="Registration.Ceremony" id="Registration_Ceremony" value="<%= Model.Registration.Ceremony == null ? 0 : Model.Registration.Ceremony.Id %>" />
    
    <h2>
        Student Information</h2>
    <ul class="registration_form">
        <li class="prefilled"><strong>Name:</strong> <span><%= Html.Encode(Model.Student.FullName) %></span>
        </li>
        <li class="prefilled"><strong>Student ID:</strong> <span><%= Html.Encode(Model.Student.StudentId) %></span> </li>
        <li class="prefilled"><strong>Units Complted:</strong> <span><%= Html.Encode(Model.Student.Units) %></span> </li>
        <li class="prefilled">
            <strong>Major:</strong>
            <span>
                <%= Html.Encode(Model.Registration.Major.Name)%>
            </span>
                <input type="hidden" value="<%= Model.Registration.Major.Id %>" name="Registration.Major" id="Registration_Major">
        </li>
        
    </ul>
    <h2>
        Contact Information</h2>    
        
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
            <%--<select><option value="">--</option><option value="AL">AL</option><option value="AK">AK</option><option value="AR">AR</option><option value="AZ">AZ</option><option value="CA">CA</option><option value="CO">CO</option><option value="CT">CT</option><option value="DC">DC</option><option value="DE">DE</option><option value="FL">FL</option><option value="GA">GA</option><option value="HI">HI</option><option value="ID">ID</option><option value="IA">IA</option><option value="IL">IL</option><option value="IN">IN</option><option value="KS">KS</option><option value="KY">KY</option><option value="LA">LA</option><option value="MA">MA</option><option value="MD">MD</option><option value="ME">ME</option><option value="MI">MI</option><option value="MO">MO</option><option value="MN">MN</option><option value="MS">MS</option><option value="MT">MT</option><option value="NC">NC</option><option value="ND">ND</option><option value="NE">NE</option><option value="NH">NH</option><option value="NJ">NJ</option><option value="NM">NM</option><option value="NY">NY</option><option value="NV">NV</option><option value="OH">OH</option><option value="OK">OK</option><option value="OR">OR</option><option value="PA">PA</option><option value="RI">RI</option><option value="SC">SC</option><option value="SD">SD</option><option value="TN">TN</option><option value="TX">TX</option><option value="UT">UT</option><option value="VA">VA</option><option value="VT">VT</option><option value="WA">WA</option><option value="WI">WI</option><option value="WV">WV</option><option value="WY">WY</option></select>--%>
        </li>
        <li><strong>Zip Code:</strong>
            <%= Html.TextBoxFor(x => x.Registration.Zip, new { maxlength = 5, size = 5 }) %>
            <%= Html.ValidationMessageFor(x=>x.Registration.Zip) %>
        </li>
        <li class="prefilled"><strong>Email Address:</strong> <span>student@ucdavis.edu</span>
        </li>
        <li><strong>Secondary Email Address:</strong>
            <%= Html.TextBoxFor(x=>x.Registration.Email) %>
            <%= Html.ValidationMessageFor(x=>x.Registration.Email) %>
        </li>
        <li>
            <strong>Ticket Acquisition:</strong>
            <%= this.RadioButton("Registration.MailTickets").Id("rMailTickets").Value(true).LabelAfter("Mail tickets to this address").Checked(Model.Registration.MailTickets) %>
            <%= this.RadioButton("Registration.MailTickets").Id("rPickupTickets").Value(false).LabelAfter("I will pick up my tickets").Checked(Model.Registration.MailTickets == false) %>
            
        </li>
    </ul>
    <h2>
        Ceremony Information</h2>
    <ul class="registration_form">
        <li><strong>Tickets Requested:</strong>
            <select id="Registration_NumberTickets" name="Registration.NumberTickets">
                <% for (int i = 1; i <= Model.Ceremony.TicketsPerStudent; i++)
                   { %>
                   
                   <% var selected = i == Model.Registration.NumberTickets; %>
                
                    <% if (selected) {%>
                        <option value="<%= i %>" selected="selected"><%= string.Format("{0:00}", i) %></option>
                    <% } else {%>
                        <option value="<%= i %>"><%= string.Format("{0:00}", i) %></option>
                    <% } %>

                <% } %>
            </select>
        </li>
        <li class="prefilled"><strong>Ceremony Date:</strong> <span><%= Html.Encode(string.Format("{0}", Model.Ceremony.DateTime)) %></span> </li>
        <li>
            <strong>Special Needs:</strong>
            <%= Html.TextAreaFor(x=>x.Registration.Comments, 10, 40, null) %>
            <%= Html.ValidationMessageFor(x=>x.Registration.Comments) %>
        </li>
    </ul>
    
    <h3>
        <%= Html.HtmlEncode(StaticValues.Txt_Disclaimer) %>
        
        <br />
        <label for="agreeToDisclaimer">I Agree</label> <%= Html.CheckBox("agreeToDisclaimer", new { @class = "required" }) %>
    </h3>