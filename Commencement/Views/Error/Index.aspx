<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ErrorViewModel>" %>
<%@ Import Namespace="Commencement.Controllers" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    

    <% if (Model.ErrorType == ErrorController.ErrorType.UnauthorizedAccess) { %>
    
        <h2>
            Commencement Registration for the College of Agricultural & Environmental Sciences
        </h2>
        
        <p>
            According to our records you are not eligible for participation in the <%= TermService.GetCurrent().Name %> ceremony. Students
            who have not commpleted the 150 units prior to beginning of Fall (Term Before Current), must file a petition.
        </p>
        <p>
            If you would like to petition to participate in the ceremony please complete 
            <%= Html.ActionLink<PetitionController>(a=>a.Register(), "this form") %>.
            <%--<a href="<%= Url.Content("~/Forms/registration_petition.pdf") %>">this form</a> --%>
            <%--(If you are having trouble, right click on the link and choose "save as" to download the file) and submit it to commencement@caes.ucdavis.edu.--%>
        </p>
    
    <% } else { %>
        <h2><%= Html.Encode(Model.Title) %></h2>
    
        <p>
            <%= Html.Encode(Model.Description) %>
        </p>
    
    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
