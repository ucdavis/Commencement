<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Commencement.Controllers.ViewModels.RegistrationModel>" %>

<script type="text/javascript" src="<%: Url.Content("~/Scripts/jquery.jqEasyCharCounter.min.js") %>"></script>
<script>
    $(function () {
        $("textarea.petition").jqEasyCounter({'maxChars': 500, 'maxCharsWarning':550});
    });
</script>

    <% foreach (var a in Model.Participations) { %>
        <div class="ceremony ui-corner-all">
            <div class="title ui-corner-top"><%: a.Ceremony.CeremonyName %></div>

            <%: Html.Hidden(string.Format("ceremonyParticipations[{0}].Ceremony", a.Index), a.Ceremony.Id) %>
            <%--<%: Html.Hidden(string.Format("ceremonyParticipations[{0}].Major", a.Index), a.Major.Id) %>--%>
            <%: Html.Hidden(string.Format("ceremonyParticipations[{0}].NeedsPetition", a.Index), a.NeedsPetition) %>
            
            <div class="ui-state-highlight message">
                <ul class="registration_form">
                <% if (a.NeedsPetition) { %>
                    <li>
                        <input type="checkbox" id="<%: string.Format("ceremonyParticipations[{0}]_Petition", a.Index) %>" name="<%: string.Format("ceremonyParticipations[{0}].Petition", a.Index) %>" value="true" <%: a.Petition ? "checked" : string.Empty %> />
                        I would like to petition for this ceremony.
                    </li>
                    <li>
                        <strong>Petition Reason:</strong>
                        <textarea type="text" id="<%: string.Format("ceremonyParticipations[{0}]_PetitionReason", a.Index) %>" name="<%: string.Format("ceremonyParticipations[{0}].PetitionReason", a.Index) %>" cols="60" rows="4" class="petition" ></textarea>
                    </li>
                    <li>
                        <strong>Completion Term:</strong>
                        <select id="<%:string.Format("ceremonyParticipations[{0}]_CompletionTerm", a.Index) %>" name="<%:string.Format("ceremonyParticipations[{0}].CompletionTerm", a.Index) %>">
                            <% foreach (var b in Model.FutureTerms) { %>
                                <option value="<%: b.Id %>"><%: b.Description %></option>
                            <% } %>
                        </select>
                    </li>
                    <li>
                        <strong>Transfer College:</strong>
                        <input type="text" id="<%: string.Format("ceremonyParticipations[{0}]_TransferCollege", a.Index) %>" name="<%: string.Format("ceremonyParticipations[{0}].TransferCollege", a.Index) %>" style="width:200px;" />
                    </li>
                    <li>
                        <strong>Transfer Units:</strong>
                        <input type="text" id="<%: string.Format("ceremonyParticipations[{0}]_TransferUnits", a.Index) %>" name="<%: string.Format("ceremonyParticipations[{0}].TransferUnits", a.Index) %>" />
                    </li>
                <% } %>
                <% else if (!a.Edit) { %>
                    <li>
                        <input type="checkbox" id="<%: string.Format("ceremonyParticipations[{0}]_Participate", a.Index) %>" name="<%: string.Format("ceremonyParticipations[{0}].Participate", a.Index) %>" value="true" <%: a.Participate ? "checked" : string.Empty %> />
                        I would like to participate in this ceremony.
                    </li>
                <% } else { %>
                    <li>
                        <input type="radio" id="<%: string.Format("ceremonyParticipations[{0}]_Participate", a.Index) %>" name="<%: string.Format("ceremonyParticipations[{0}].Participate", a.Index) %>" value="true" <%: a.Participate ? "checked" : string.Empty %> />
                        I would like to participate in this ceremony.
                    </li><li>
                        <input type="radio" id="<%: string.Format("ceremonyParticipations[{0}]_Cancel", a.Index) %>" name="<%: string.Format("ceremonyParticipations[{0}].Cancel", a.Index) %>" value="true" <%: a.Cancel ? "checked" : string.Empty %> />
                        I would like to cancel this registration.  I understand that this will forfeit my tickets and if I change my mind I may not be able to receive the same amount of tickets.

                        <%: Html.Hidden(string.Format("ceremonyParticipations[{0}].ParticipationId", a.Index), a.ParticipationId) %>
                    </li>
                <% } %>
                </ul>
            </div>

            <ul class="registration_form">

                <li>
                    <strong>Date/Time: </strong> 
                    <%: a.Ceremony.DateTime.ToString("g") %>
                    <span id="time_disclaimer">*The time of your ceremony is not guaranteed.</span>
                </li>

                <li>
                    <strong>Ticket Distribution:</strong>
                    <% if (a.Ceremony.TicketDistributionMethods.Count > 0) { %>
                    <%= this.RadioSet(string.Format("ceremonyParticipations[{0}].TicketDistributionMethod", a.Index)).Options(a.Ceremony.TicketDistributionMethods, x=>x.Id, x=>x.Name).Selected(a.TicketDistributionMethod != null ? a.TicketDistributionMethod.Id : string.Empty).Class("radio_list") %>
                    <% } else { %>
                    Please consult the Dean's Office FAQ
                    <% } %>
                    
                </li>

                <li>
                    <strong>Major: </strong>
                    <%= this.Select(string.Format("ceremonyParticipations[{0}].Major", a.Index)).Options(a.Ceremony.Majors.Where(b => b.ConsolidationMajor == null), x=>x.Id, x=>x.Name).Selected(a.Major.MajorId).Class("hastip").Attr("title", "The major you would like to walk with.") %>
                </li>
                <li>
                    <strong>Tickets Requested:</strong>

                    <select id="<%: string.Format("ceremonyParticipations[{0}]_Tickets", a.Index) %>" name="<%: string.Format("ceremonyParticipations[{0}].Tickets", a.Index) %>" >
                        <% for (int i = 1; i <= a.Ceremony.TicketsPerStudent; i++) { %>
                            <% if (i == a.Tickets) { %>
                            <% } %>

                            <option value="<%: i %>" <%: i == a.Tickets ? "selected=\"selected\"" : string.Empty %> ><%: string.Format("{0:00}", i) %></option>
                        <% } %>
                    </select>
                </li>
            </ul>
            
            <div class="foot ui-corner-bottom"></div>
        </div>
    <% } %>