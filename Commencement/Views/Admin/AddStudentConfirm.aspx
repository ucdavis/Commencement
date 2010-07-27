<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Core.Domain.Student>" %>
<%@ Import Namespace="Commencement.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	AddStudentConfirm
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <%= Html.ActionLink<AdminController>(a=>a.Students(null, null, null, null), "Cancel") %>

    <h2>AddStudentConfirm</h2>

    <% using(Html.BeginForm("AddStudentConfirm", "Admin", FormMethod.Post)) { %>
    <%= Html.AntiForgeryToken() %>
    <%= Html.HiddenFor(x=>x.Pidm) %>
    <%= Html.Hidden("majorCode", Model.StrMajorCodes) %>

    <ul class="registration_form">
        <li>
            <strong>Student Id:</strong>
            <%= Html.Encode(Model.StudentId) %>
            <%= Html.HiddenFor(x=>x.StudentId) %>
        </li>
        <li>
            <strong>Name:</strong>
            <%= Html.Encode(Model.FullName) %>
            <%= Html.HiddenFor(x=>x.FirstName) %>
            <%= Html.HiddenFor(x=>x.LastName) %>
        </li>
         <li>
            <strong>Units:</strong>
            <%= Html.Encode(String.Format("{0:F}", Model.Units)) %>
            <%= Html.HiddenFor(x=>x.Units) %>
        </li>
        <li>
            <strong>Email:</strong>
            <%= Html.Encode(Model.Email) %>
            <%= Html.HiddenFor(x=>x.Email) %>
        </li>
        <li>
            <strong>Login:</strong>
            <%= Html.Encode(Model.Login) %>
            <%= Html.HiddenFor(x=>x.Login) %>
        </li>
        <li>
            <strong>Major:</strong>
            <%= Html.Encode(Model.StrMajorCodes) %>
        </li>
        <li>
            <strong></strong>
            <%= Html.SubmitButton("Submit", "Save") %>
        </li>
    </ul>
    
    <% } %>
    
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

