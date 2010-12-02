<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Commencement.Controllers.ViewModels.RegistrationModel>" %>
<%@ Import Namespace="Commencement.Core.Resources" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>

    <script type="text/javascript">
        $(document).ready(function () {
            $("input:radio").click(function () {
                $(this).siblings("input:radio").attr('checked', false);
            });
        });
    </script>

    <h2>
        Student Information</h2>
    <ul class="registration_form">
        <li class="prefilled"><strong>Name:</strong> <span><%= Html.Encode(Model.Student.FullName) %></span>
        </li>
        <li class="prefilled"><strong>Student ID:</strong> <span><%= Html.Encode(Model.Student.StudentId) %></span> 
            <%: Html.Hidden("Registration.Student", Model.Student.Id) %>
        </li>
        <li class="prefilled"><strong>Units Complted:</strong> <span><%= Html.Encode(Model.Student.TotalUnits) %></span> </li>
        <li class="prefilled">
            <strong>Available Major(s):</strong>
            <span>
                <%: string.Join(", ", Model.Student.StrMajors) %>
            </span>
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
        <li>
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
        </li>
    </ul>

    <h2>Ceremony Information</h2>

    <% foreach (var a in Model.Participations) { %>
        <fieldset>
            <legend>Commencement for <%: a.Major.MajorName %></legend>

            <%: Html.Hidden(string.Format("ceremonyParticipations[{0}].Ceremony", a.Index), a.Ceremony.Id) %>
            <%: Html.Hidden(string.Format("ceremonyParticipations[{0}].Major", a.Index), a.Major.Id) %>

            <ul class="registration_form">


                <% if (!a.Edit) { %>
                    <li>
                        <input type="checkbox" id="<%: string.Format("ceremonyParticipations[{0}]_Participate", a.Index) %>" name="<%: string.Format("ceremonyParticipations[{0}].Participate", a.Index) %>" value="true" <%: a.Participate ? "checked" : string.Empty %> />
                        I would like to participate in this ceremony.
                    </li>
                <% } else { %>
                    <li>
                        <input type="radio" id="<%: string.Format("ceremonyParticipations[{0}]_Participate", a.Index) %>" name="<%: string.Format("ceremonyParticipations[{0}].Participate", a.Index) %>" value="true" <%: a.Participate ? "checked" : string.Empty %> />
                        I would like to participate in this ceremony.
                        <br />
                        <input type="radio" id="<%: string.Format("ceremonyParticipations[{0}]_Cancel", a.Index) %>" name="<%: string.Format("ceremonyParticipations[{0}].Cancel", a.Index) %>" value="true" <%: a.Cancel ? "checked" : string.Empty %> />
                        I would like to cancel this registration.  I understand that this will forfeit my tickets and if I change my mind I may not be able to receive the same amount of tickets.
                    </li>
                <% } %>

                <li>
                    <strong>Date/Time: </strong> <%: a.Ceremony.DateTime.ToString("g") %>
                </li>
                <li>
                    <strong>Major: </strong><%: a.Major.MajorName %>
                </li>
                <li>
                    <strong>Tickets Requested:</strong>

                    <select id="<%: string.Format("ceremonyParticipations[{0}]_Tickets", a.Index) %>" name="<%: string.Format("ceremonyParticipations[{0}].Tickets", a.Index) %>" >
                        <% for (int i = 1; i < a.Ceremony.TicketsPerStudent; i++) { %>
                            <% if (i == a.Tickets) { %>
                            <% } %>

                            <option value="<%: i %>" <%: i == a.Tickets ? "selected=\"selected\"" : string.Empty %> ><%: string.Format("{0:00}", i) %></option>
                        <% } %>
                    </select>
                </li>
            </ul>
        </fieldset>
    <% } %>

    <h2>Special Needs</h2>

    <%= this.CheckBoxList("SpecialNeeds").Options(Model.SpecialNeeds).ItemClass("SpecialNeeds")%>