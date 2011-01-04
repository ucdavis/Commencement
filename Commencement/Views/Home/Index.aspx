<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Core.Domain.TermCode>" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>
<%@ Import Namespace="Commencement.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Index</h2>

    <%: Html.HtmlEncode(Model.LandingText) %>

    <ul>
        <li>Deadline to order your cap and gown: <%: Model.CapAndGownDeadline.ToString("d") %></li>
        <li>Deadline to file to graduate with the registrar: <%: Model.FileToGraduateDeadline.ToString("d") %></li>
    </ul>

    <%: Html.ActionLink<StudentController>(a => a.RegistrationRouting(), "Register")%>


</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
