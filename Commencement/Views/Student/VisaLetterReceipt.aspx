<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="Commencement.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Visa Letter Receipt
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <ul class="btn">
            <li><%: Html.ActionLink<HomeController>(a=>a.Index(), "Ceremony") %></li>
            <li><%: Html.ActionLink<StudentController>(a=>a.VisaLetters(), "Visa Letters") %></li>
        </ul>
<h2>Visa Letter Receipt</h2>
    <div>
        Thank you for requesting a Visa Letter. Once your request has been decided upon, you will be notified by email.
        You may also check back <%: Html.ActionLink<StudentController>(a=>a.VisaLetters(), "here") %> to see the status of your requests.
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>
