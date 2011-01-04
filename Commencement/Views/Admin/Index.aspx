<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="Commencement.Controllers" %>
<%@ Import Namespace="Commencement.Controllers.Filters" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Admin Home
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <ul class="front_menu">
        <li class="left">
            <a href="<%= Url.Action("Index", "Ceremony") %>"><img src="<%= Url.Content("~/Images/ceremony.png") %>" /><br />Ceremony List</a>
        </li>
        <li class="left">
            <a href="<%= Url.Action("Students", "Admin") %>"><img src="<%= Url.Content("~/Images/students.png") %>" /><br />Students</a>
        </li>
        <li>
            <a href="<%= Url.Action("Registrations", "Admin") %>"><img src="<%= Url.Content("~/Images/registrations.png") %>" /><br />Registrations</a>
        </li>
        <li class="left">
            <a href="<%= Url.Action("Index", "Petition") %>"><img src="<%= Url.Content("~/Images/pending_petition.png") %>" /><br />Pending Petitions</a>
        </li>
        <li class="left">
            <a href="<%= Url.Action("Index", "Report") %>"><img src="<%= Url.Content("~/Images/report.png") %>" /><br />Reporting</a>
        </li>
        <% if (User.IsInRole(RoleNames.RoleAdmin)) { %>
        <li>
            <a href="<%= Url.Action("AdminLanding", "Admin") %>"><img src="<%= Url.Content("~/Images/report.png") %>" /><br />Administration</a>
        </li>
        <% } %>
    </ul>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
