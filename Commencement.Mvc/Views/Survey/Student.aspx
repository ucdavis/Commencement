<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.MVC.Controllers.SurveyViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%: Model.Survey.Name %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2 style="color: red;">Please complete the below exit survey to be given access to request extra tickets.</h2>
    <h2><%: Model.Survey.Name %></h2>
    
    <% using (Html.BeginForm()) { %>
        <%= Html.AntiForgeryToken() %>
        <%: Html.Partial("_Survey") %>
        <input type="submit" value="Submit Survey" class="button" style="margin-top: 1em;"/>
    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    <%: Html.Partial("_SurveyHeaders") %>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>
