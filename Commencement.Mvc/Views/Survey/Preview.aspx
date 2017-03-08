<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.MVC.Controllers.SurveyViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Survey Preview
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <h2>Survey Preview | <%: Model.Survey.Name %></h2>
    
    <%: Html.Partial("_Survey") %>
    <%: Html.ActionLink("Back to List", "Index", "Survey", new {}, new {@class="button", style="margin-top: 1em;"}) %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    <%: Html.Partial("_SurveyHeaders") %>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>
