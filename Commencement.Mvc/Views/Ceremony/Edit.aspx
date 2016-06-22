<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.CeremonyViewModel>" %>
<%@ Import Namespace="Commencement.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Commencement | Edit Ceremony
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="btn">
        <li>
        <%= Html.ActionLink<CeremonyController>(a => a.Index() , "Back to List") %>
        </li>
    </ul>

    <div class="page_bar">
        <div class="col1"><h2>Edit Ceremony</h2></div>
        <div class="col2">
            <%: Html.ActionLink<CeremonyController>(a => a.CanceledRegistrations(Model.Ceremony.Id), "Canceled", new { @class="button" })%>
            <%: Html.ActionLink<CeremonyController>(a => a.EditPermissions(Model.Ceremony.Id), "Permissions", new { @class="button" })%>
            <%= Html.ActionLink<TemplateController>(a => a.Index(Model.Ceremony.Id), "Email Templates", new { @class = "button" })%>
        </div>
    </div>

    <%= Html.ValidationSummary("Please correct all errors below") %>
    <%= Html.ClientSideValidation<Commencement.Core.Domain.Ceremony>("Ceremony") %>

    <% using (Html.BeginForm("Edit", "Ceremony", FormMethod.Post)) { %>
        <%= Html.AntiForgeryToken() %>
    
        <% Html.RenderPartial("CeremonyPartial"); %>

    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    <script src="<%= Url.Content("~/Scripts/tiny_mce/jquery.tinymce.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.enableTinyMce.js") %>" type="text/javascript"></script>  
</asp:Content>
