<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.CeremonyViewModel>" %>
<%@ Import Namespace="Commencement.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create Commencement
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<ul class="btn">
    <li>
    <%= Html.ActionLink<CeremonyController>(a => a.Index() , "Back to List") %>
    </li></ul>

    <h2>Create</h2>

    <%= Html.ValidationSummary("Please correct all errors below") %>
    <%= Html.ClientSideValidation<Commencement.Core.Domain.Ceremony>("Ceremony") %>

    <% using (Html.BeginForm("Create", "Ceremony", FormMethod.Post)) { %>

        <%= Html.AntiForgeryToken()%>
        
        <% Html.RenderPartial("CeremonyPartial"); %>

    <% } %>    

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

    

</asp:Content>
