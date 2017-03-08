<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Core.Domain.VisaLetter>" %>
<%@ Import Namespace="Commencement.MVC.Controllers" %>
<%@ Import Namespace="Commencement.MVC.Controllers.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Commencement | Request Visa Letter
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="btn">
        <li><%: Html.ActionLink<StudentController>(a=>a.VisaLetters(), "Back to List of Letter Requests") %></li>
    </ul>

    <h2>Create Visa Letter Request For <%: Model.Student.FullName %></h2>
    
        <%: Html.ValidationSummary(true) %>

    <% using (Html.BeginForm()) { %>
    <%: Html.AntiForgeryToken() %>


    <ul class="registration_form">

    <% Html.RenderPartial("EditVisaLetterPartial"); %>

        <li><strong>&nbsp;</strong>
            <%: Html.SubmitButton("Submit", "Save", new { @class="button" })%>
        </li>
        </ul>   
    <% } %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

</asp:Content>
