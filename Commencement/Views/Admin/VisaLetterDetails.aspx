<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.AdminVisaDetailsModel>" %>
<%@ Import Namespace="Commencement.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Visa Letter Request
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <ul class="btn">
        <li><%: Html.ActionLink<AdminController>(a=>a.VisaLetters(null, null, false), "Back to List of Letter Requests") %></li>
    </ul>

    <h2>Visa Letter Request</h2>
    
    <% Html.RenderPartial("VisaLetterDetailsPartial", Model.VisaLetter); %>
    <fieldset>
        <legend>Related Letters</legend>
        <% Html.RenderPartial("VisaLetterTablePartial", Model.RelatedLetters); %>
    </fieldset>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>
