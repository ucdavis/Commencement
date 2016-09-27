<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.ExtraTicketPetitionModel>"
    MasterPageFile="~/Views/Shared/Site.Master" %>
<%@ Import Namespace="Commencement.Core.Resources" %>
<%@ Import Namespace="Commencement.Controllers" %>

<asp:Content runat="server" ID="Content" ContentPlaceHolderID="TitleContent">
    Commencement | Extra Ticket Petition
</asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="HeaderContent">

    <script type="text/javascript" src="<%: Url.Content("~/Scripts/jquery.jqEasyCharCounter.min.js") %>"></script>

    <script type="text/javascript">
        $(function () {

            $(".petition-reason").jqEasyCounter({'maxChars': 100, 'maxCharsWarning': 80});

        });
    </script>

</asp:Content>

<asp:Content runat="server" ID="Content3" ContentPlaceHolderID="logoContent">
</asp:Content>

<asp:Content runat="server" ID="Content2" ContentPlaceHolderID="MainContent">
    <h1>Extra Ticket Petition</h1>
       
    <ul class="btn">
        <li><%: Html.ActionLink<StudentController>(a=>a.DisplayRegistration(), "Back") %></li>
    </ul>

    <%= Html.ValidationSummary("Please correct all errors below") %>
    
    <fieldset>
        
        <legend>Student Information</legend>
        
        <ul class="registration_form">
            <li class="prefilled">
                <strong>Student Id: </strong>
                <%: Model.Registration.Student.StudentId %>
            </li>
            <li class="prefilled">
                <strong>Name:</strong>
                <%: Model.Registration.Student.FullName %>
            </li>
        </ul>

    </fieldset>
    
    <% using (Html.BeginForm()) { %>
    
        <%: Html.AntiForgeryToken() %>
        
        <% var registrations = Model.Registration.RegistrationParticipations.Where(x => !x.Cancelled).ToList();
            var counter = 0;
            for (int j = 0; j < registrations.Count; j++) {
                var participation = registrations[j];
            %>
    
            <fieldset>
                
                <legend><%: participation.Ceremony.CeremonyName %></legend>
                
                    <% if (!Model.AvailableParticipationIds.Contains(participation.Id)) { %>
                        <div class="ui-state-error message">
                        <% if (participation.ExtraTicketPetition != null) { %>
                                This ceremony is not available for extra ticket petition because you have previously submitted a petition.
                        <% } else if (DateTime.Now < participation.Ceremony.ExtraTicketBegin) { %>
                                This ceremony is not available for extra ticket petition.  Please return on <%: participation.Ceremony.ExtraTicketBegin.ToString("d") %> to submit a petition.
                        <% } else if (DateTime.Now > participation.Ceremony.ExtraTicketDeadline.AddDays(1)) { %>
                                This ceremony is not available for extra ticket petitions.  The deadline was on <%: participation.Ceremony.ExtraTicketDeadline.ToString("d") %>.
                        <% } %>
                        </div>
                    <% } %>

                    <ul class="registration_form">
                    <li class="prefilled"><strong>Major:</strong>
                        <%: participation.Major.Name %>
                    </li>
                    <li class="prefilled"><strong>Ceremony Time:</strong>
                        <%: participation.Ceremony.DateTime.ToString("g") %>
                    </li>
                    <li class="prefilled">
                        <strong>Delivery Method:</strong>
                        <%: participation.TicketDistributionMethod.Name %>
                    </li>
                    <li class="prefilled hastip" style="display: inline-block;" title="# tickets originally requested when registering."><strong># Tickets Requested:</strong>
                        <%: participation.NumberTickets %>
                    </li>
                    
                    <% if (Model.AvailableParticipationIds.Contains(participation.Id))
                       {
                           var postModel = Model.ExtraTicketPetitionPostModels.Where(a => a.RegistrationParticipation == participation).FirstOrDefault();
                       %>

                        <%: Html.Hidden(string.Format("extraTicketPetitions[{0}].Ceremony", counter), participation.Ceremony.Id)%>
                        <%: Html.Hidden(string.Format("extraTicketPetitions[{0}].RegistrationParticipation", counter), participation.Id)%>

                        <li><strong>Extra Tickets:</strong>
                            <select id="<%: string.Format("extraTicketPetitions[{0}]_NumberTickets", counter)  %>" name="<%: string.Format("extraTicketPetitions[{0}].NumberTickets", counter) %>">
                                <% for (int i = 0; i <= participation.Ceremony.ExtraTicketPerStudent; i++)
                                   { %>
                                    <% if (postModel != null && postModel.NumberTickets == i)
                                       { %>
                                        <option value="<%: i %>" selected="selected"><%: i%></option>
                                    <% }
                                       else
                                       { %>
                                        <option value="<%: i %>"><%: i%></option>
                                    <% } %>
                                <% } %>
                            </select>
                        </li>

                        <% if (participation.Ceremony.HasStreamingTickets)
                           { %>
                            <li><strong>Streaming Tickets:</strong>
                                <select id="<%: string.Format("extraTicketPetitions[{0}]_NumberStreamingTickets", counter) %>" name="<%: string.Format("extraTicketPetitions[{0}].NumberStreamingTickets", counter) %>">
                                    <% for (int i = 0; i <= participation.Ceremony.ExtraTicketPerStudent; i++)
                                       { %>
                                        <% if (postModel != null && postModel.NumberStreamingTickets == i)
                                           { %>
                                            <option value="<%: i %>" selected="selected"><%: i%></option>
                                        <% }
                                           else
                                           { %>
                                            <option value="<%: i %>"><%: i%></option>
                                        <% } %>
                                    <% } %>
                                </select>
                            </li>
                        <% }
                           else
                           { %>
                            <%: Html.Hidden(string.Format("extraTicketPetitions[{0}]_NumberStreamingTickets", counter), 0)%>
                        <% } %>

                        <li>
                            <strong>Reason:</strong>
                            <%: Html.TextArea(string.Format("extraTicketPetitions[{0}].Reason", counter), participation.ExtraTicketPetition != null ? participation.ExtraTicketPetition.Reason : string.Empty, new { @style = "width:400px;", @class = "petition-reason" })%>
                            <%: Html.ValidationMessage("Reason" + participation.Ceremony.Id) %>
                        </li>
                        <% } %>
                    
                

                </ul>

            </fieldset>
            
            <%  counter++; %>

        <!-- End of for loop -->
        <% } %>
        
        <fieldset>
            <ul class="registration_form">
                <li><strong>&nbsp;</strong>
                    <input type="submit" value="Submit" class="button"/>
                    |
                    <%: Html.ActionLink<StudentController>(a=>a.DisplayRegistration(), "Cancel") %>
                </li>                
            </ul>
        </fieldset>

    <!-- End of html form -->    
    <% } %>
    


</asp:Content>




