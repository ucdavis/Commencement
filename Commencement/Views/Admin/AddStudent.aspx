<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.AdminEditStudentViewModel>" %>
<%@ Import Namespace="Commencement.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	AddStudent
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="btn">
        <li><%: Html.ActionLink<AdminController>(a=>a.Index(), "Back to Home") %></li>
    </ul>

    <h2>Add Student</h2>

    <% if (Model.Student != null) { %>
    <% Html.RenderPartial("EditStudentPartial"); %>
    <% } else { %>
        <% using (Html.BeginForm("AddStudent", "Admin", FormMethod.Get)) { %>
        Student ID:
        <%: Html.TextBox("StudentId") %>
        <%: Html.SubmitButton("Submit", "Search") %>
        <% } %>
    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>
