<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.SurveyStatsViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Survey Results
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Results | <%: Model.Survey.Name %></h2>
    
    <% foreach (var question in Model.Stats) { %>
    
        <div class="question">
            
            <div class="prompt"><%: question.Item1.Prompt %></div>
            
            <div class="results-container">
            
                <%--Show bargraph or just text answers--%>
                <% if (question.Item1.SurveyFieldType.FixedAnswers) { %>
                
                    <%: Html.Partial("_RenderResponseChart", question) %>

                <% } %>
                
            </div>

        </div>

    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    
    <script type="text/javascript" src="http://code.highcharts.com/highcharts.js"></script>

    <style type="text/css">
        .question {}
        .prompt {}
        .results-container{}
    </style>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>
