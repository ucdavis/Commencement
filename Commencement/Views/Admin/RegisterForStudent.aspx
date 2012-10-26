<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.AdminRegisterForStudentViewModel>" %>
<%@ Import Namespace="Commencement.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Commencement | Register For Student
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="btn">
        <li>
            <%= Html.ActionLink<AdminController>(a=>a.StudentDetails(Model.RegistrationModel.Student.Id, null), "Back to Student") %>
        </li>
    </ul>

    <h2>Register for Student</h2>

    <%= Html.ValidationSummary("Please correct all errors below") %>
    <%= Html.ClientSideValidation<Commencement.Core.Domain.Registration>("Registration") %>
    
    <% using (Html.BeginForm()) { %>
        <%= Html.AntiForgeryToken() %>

        <fieldset>
        
            <legend>Student</legend>

            <% Html.RenderPartial("StudentInformationPartial", Model.RegistrationModel.Student); %>

        </fieldset>

        <fieldset>
        
            <legend>Contact Information</legend>

            <% Html.RenderPartial("ContactEditPartial", Model.RegistrationModel); %>        

        </fieldset>
    
        <fieldset>
        
            <legend>Ceremony Information</legend>

                <% foreach (var a in Model.RegistrationModel.Participations) { %>

                    <div class="ceremony ui-corner-all">
                        <div class="title ui-widget-header ui-corner-top">
                            <%: string.Format("Commencement for {0}", a.Major.Name) %>
                        </div>

                        <%: Html.Hidden(string.Format("ceremonyParticipations[{0}].NeedsPetition", a.Index), a.NeedsPetition) %>
                        <%: Html.Hidden(string.Format("ceremonyParticipations[{0}].ParticipationId", a.Index), a.ParticipationId) %>
                        
                        <div class="ui-state-focus message">
                            <ul class="registration_form">
                                <!-- Student needs to petition, display a message for them -->
                                <% if (a.NeedsPetition) { %>
                                    <li style="border: 1px solid red; color: Red;">
                                        This student does not meet requirements for registration for this ceremony and would normally be required to petition.
                                    </li>
                                <% } %>

                                <!-- Hasn't registered yet. -->
                                <% if (!a.ParticipationId.HasValue) { %>
                                    <li>
                                        <input type="checkbox" id="<%: string.Format("ceremonyParticipations[{0}]_Participate", a.Index) %>" name="<%: string.Format("ceremonyParticipations[{0}].Participate", a.Index) %>" value="true" <%: a.Participate ? "checked" : string.Empty %> />
                                        Register for this ceremony
                                    </li>
                
                                <!-- registered already, wanting to edit/cancel. -->
                                <% } else { %>
                                     <li>
                                        <input type="radio" id="Radio1" name="<%: string.Format("ceremonyParticipations[{0}].Participate", a.Index) %>" value="true" <%: a.Participate ? "checked" : string.Empty %> />
                                        Register for this ceremony
                                        <br/>
                                        <br/>
                                        <input type="radio" id="<%: string.Format("ceremonyParticipations[{0}]_Cancel", a.Index) %>" name="<%: string.Format("ceremonyParticipations[{0}].Cancel", a.Index) %>" value="true" <%: a.Cancel ? "checked" : string.Empty %> />
                                        Cancel this registration
                                     </li>
                                <% } %>                                
                            </ul>
                        </div>

                        <ul class="registration_form">
                


                        <li>
                            <strong>Ticket Distribution:</strong>

                            <% if (a.Ceremony.TicketDistributionMethods.Any()) { %>
                            <%= this.RadioSet(string.Format("ceremonyParticipations[{0}].TicketDistributionMethod", a.Index)).Options(a.Ceremony.TicketDistributionMethods, x=>x.Id, x=>x.Name).Selected(a.TicketDistributionMethod != null ? a.TicketDistributionMethod.Id : string.Empty).Class("radio_list") %>
                            <% } else { %>
                            No ticket distribution method has been selected.
                            <% } %>
                        </li>

                        <li>
                            <strong>Date/Time: </strong> 

                            <%= this.Select(string.Format("ceremonyParticipations[{0}].Ceremony", a.Index)).Options(Model.Ceremonies, x=>x.Id, x=>x.DateTime).Selected(a.Ceremony.Id)%>
                        </li>
                        <li>
                            <strong>Major: </strong>

                            <%= this.Select(string.Format("ceremonyParticipations[{0}].Major", a.Index)).Options(Model.Majors, x=>x.Id, x=>x.Name).Selected(a.Major.Id) %>

                        </li>
                        <li>
                            <strong>Tickets Requested:</strong>

                            <select id="<%: string.Format("ceremonyParticipations[{0}]_Tickets", a.Index) %>" name="<%: string.Format("ceremonyParticipations[{0}].Tickets", a.Index) %>" >
                                <% for (int i = 1; i <= a.Ceremony.TicketsPerStudent; i++) { %>
                                    <option value="<%: i %>" <%: i == a.Tickets ? "selected=\"selected\"" : string.Empty %> ><%: string.Format("{0:00}", i) %></option>
                                <% } %>
                            </select>
                        </li>
                        </ul>

                        <div class="foot ui-corner-bottom">&nbsp;</div>

                    </div>

                <% } %>
        </fieldset>

        <fieldset>
        
            <legend>Special Needs</legend>

            <ul class="registration_form">
                <li><strong>&nbsp;</strong>
                <% Html.RenderPartial("SpecialNeedsPartial", Model.RegistrationModel); %>
                </li>
            </ul>

        </fieldset>

        <fieldset>
        
            <legend>Classic Photography</legend>

            <ul class="registration_form">
                <li><strong>&nbsp;</strong>
                <%= Html.CheckBox("gradTrack", Model.RegistrationModel.Registration.GradTrack) %><label for="gradTrack">Student has authorized information to be released to Classic Photography</label>
                </li>
            </ul>
        </fieldset>

        <fieldset>
        
            <ul class="registration_form">
                <li><strong>&nbsp;</strong>
                    <input type="submit" value="Update Registration" class="button" />
                    |
                    <%= Html.ActionLink<AdminController>(a=>a.StudentDetails(Model.RegistrationModel.Student.Id, null), "Cancel") %>
                </li>
            </ul>

        </fieldset>

        
    
    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("input:radio").click(function () {
                $(this).siblings("input:radio").attr('checked', false);
            });
        });
    </script>
</asp:Content>

