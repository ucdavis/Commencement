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
        Thank you for submitting your request for a visa letter of support. You will receive an email within a week if your letter is approved. The email will include the link to download and print your letter.
For information regarding the Commencement ceremony please visit commencement.ucdavis.edu
To view the history of letters you have requested please <%: Html.ActionLink<StudentController>(a=>a.VisaLetters(), "click Visa Letter") %> Home button. 

    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>
