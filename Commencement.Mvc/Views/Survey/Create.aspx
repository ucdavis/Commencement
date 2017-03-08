<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.MVC.Controllers.SurveyCreateViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create Survey
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Create Survey</h2>
    
    <%= Html.ValidationSummary("Please correct all errors below") %>

    <% using (Html.BeginForm("Create", "Survey", FormMethod.Post)) { %>
        
        <%= Html.AntiForgeryToken() %>
        
        <%: Html.Partial("_SurveyForm") %>

    <% } %>
    
    <%: Html.Partial("_surveyFormModals") %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    
    <%: Html.Partial("_SurveyFormHeaders") %>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>