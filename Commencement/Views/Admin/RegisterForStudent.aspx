<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.RegistrationModel>" %>
<%@ Import Namespace="Commencement.Controllers" %>
<%@ Import Namespace="Commencement.Core.Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	RegisterForStudent
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="btn">
        <li>
            <%= Html.ActionLink<AdminController>(a=>a.StudentDetails(Model.Student.Id, null), "Back to Student") %>
        </li>
    </ul>

    <h2>RegisterForStudent</h2>

    <%= Html.ValidationSummary("Please correct all errors below") %>
    <%= Html.ClientSideValidation<Commencement.Core.Domain.Registration>("Registration") %>
    
    <% using (Html.BeginForm()) { %>
        <%= Html.AntiForgeryToken() %>

        <h2>Student Information</h2>
        <% Html.RenderPartial("StudentInformationPartial", Model.Student); %>
    
        <h2>Contact Information</h2>    
        <% Html.RenderPartial("ContactEditPartial"); %>        

        <h2>Ceremony Information</h2>
        <%--<% Html.RenderPartial("CeremonyEditPartial"); %>--%>
        <% foreach (var a in Model.Participations) { %>

            <%: Html.Hidden(string.Format("ceremonyParticipations[{0}].Ceremony", a.Index), a.Ceremony.Id) %>
            <%: Html.Hidden(string.Format("ceremonyParticipations[{0}].Major", a.Index), a.Major.Id) %>
            <%: Html.Hidden(string.Format("ceremonyParticipations[{0}].NeedsPetition", a.Index), a.NeedsPetition) %>

            <fieldset>
                <legend>Commencement for <%: a.Major.Name %></legend>

                <ul class="registration_form">
                <% if (a.NeedsPetition) { %>
                    <li style="border: 1px solid red; color: Red;">
                        This student does not meet requirements for registration for this ceremony and would normally be required to petition.
                    </li>
                <% } %>
                <% if (!a.Edit) { %>
                    <li>
                        <input type="checkbox" id="<%: string.Format("ceremonyParticipations[{0}]_Participate", a.Index) %>" name="<%: string.Format("ceremonyParticipations[{0}].Participate", a.Index) %>" value="true" <%: a.Participate ? "checked" : string.Empty %> />
                        Register for this ceremony
                    </li>
                <% } else { %>
                     <li>
                        <input type="radio" id="Radio1" name="<%: string.Format("ceremonyParticipations[{0}].Participate", a.Index) %>" value="true" <%: a.Participate ? "checked" : string.Empty %> />
                        Register for this ceremony
                        <br />
                        <input type="radio" id="<%: string.Format("ceremonyParticipations[{0}]_Cancel", a.Index) %>" name="<%: string.Format("ceremonyParticipations[{0}].Cancel", a.Index) %>" value="true" <%: a.Cancel ? "checked" : string.Empty %> />
                        Cacel this registration
                    </li>
                <% } %>

                <li>
                    <strong>Date/Time: </strong> <%: a.Ceremony.DateTime.ToString("g") %>
                </li>
                <li>
                    <strong>Major: </strong><%: a.Major.Name %>
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
        <% Html.RenderPartial("SpecialNeedsPartial"); %>
    
        <h3>
        <div class="legaldisclaimer">
        <%= Html.CheckBox("gradTrack", Model.Registration.GradTrack) %><label for="gradTrack">Student has authorized information to be released to Grad Track</label>
        </div>
        </h3>

        <input type="submit" value="Update Registration" />
    
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

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>
