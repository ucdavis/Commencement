<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.SurveyStatsViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Survey Results
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Results | <%: Model.Survey.Name %></h2>
    
    <p>
        <% using (Html.BeginForm("Results", "Survey", FormMethod.Get)) { %>
            <select id="ceremonyId" name="ceremonyId">
                <option value="">--Select a Ceremony--</option>
                <%foreach (var c in Model.Ceremonies) { %>
                <option value="<%: c.Id %>" <%: Model.Ceremony != null && Model.Ceremony.Id == c.Id ? "selected" : string.Empty %> ><%: c.Name %>(<%: c.TermCode.Name %>)</option>
                <% } %>
            </select>
            <input type="submit" class="button"/>
        <% } %>
    </p>
    <% if (Model.Ceremony != null) { 
           var responsecount = (decimal)Model.Ceremony.RegistrationSurveys.Count;
           var participationcount = (decimal)Model.Ceremony.RegistrationParticipations.Count(a => !a.Cancelled);
           %>
        <p style="margin: 1em 0;"><strong># Responses: <%: responsecount %></strong></p>
        <p style="margin: 1em 0;">
            <strong># Registrations: <%: participationcount %>
            <% if (participationcount > 0) { %>
                ( <%: Math.Floor((responsecount / participationcount) * 100) %>% )
            <% } %>
        </strong>
        </p>
        <p style="margin: 1em 0;"><a href="<%: Url.Action("Export", new {id=Model.Survey.Id, ceremonyId=Model.Ceremony.Id}) %>" class="button">Export</a></p>

        <% foreach (var question in Model.Stats) { %>
    
            <div class="question">
            
                <div class="prompt"><%: question.Item1.Prompt %></div>
            
                <div class="results-container">
            
                    <%--Show bargraph or just text answers--%>
                    <% if (question.Item1.SurveyFieldType.FixedAnswers) { %>
                
                        <ul>
                        <% foreach (DictionaryEntry entry in question.Item2) { %>
                            <li><strong><%: entry.Key %>: </strong><%: entry.Value %></li>
                        <% } %>
                        </ul>

                    <% } else { %>
                
                        <% foreach (DictionaryEntry response in question.Item2) { %>
                    
                            <ul>
                                <li><%: response.Key %></li>
                            </ul>

                        <% } %>

                    <% } %>
                
                </div>

            </div>

        <% } %>
    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    
    <style type="text/css">
        .question {
             border: 1px solid #F2F2F2;
             padding: 1em;
             margin: 1em;
             
            -moz-box-shadow: 3px 3px 4px #E3E3E3;
            -webkit-box-shadow: 3px 3px 4px #E3E3E3;
            box-shadow: 3px 3px 4px #E3E3E3;
            /* For IE 8 */
            -ms-filter: "progid:DXImageTransform.Microsoft.Shadow(Strength=4, Direction=135, Color='#E3E3E3')";
            /* For IE 5.5 - 7 */
            filter: progid:DXImageTransform.Microsoft.Shadow(Strength=4, Direction=135, Color='#E3E3E3');
        }
        .prompt {
            font-size: large;
            font-weight: bold;
            line-height: 20px;
            padding-bottom: .5em;
            border-bottom: 1px solid #E3E3E3;
        }
        .results-container {
            margin: 1em;
        }
        .results-container li {
            margin: .5em;
        }
    </style>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>
