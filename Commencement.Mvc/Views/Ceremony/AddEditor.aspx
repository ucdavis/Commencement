<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.MVC.Controllers.ViewModels.AddEditorViewModel>" %>
<%@ Import Namespace="Commencement.MVC.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Commencement | Add Editor
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="btn">
    <li>
        <%= Html.ActionLink<CeremonyController>(a => a.EditPermissions(Model.Ceremony.Id) , "Back to Permissions") %>
    </li>
    </ul>

    <h2>Add Editor to <%: Model.Ceremony.CeremonyName %></h2>

    <% using (Html.BeginForm("AddEditor", "Ceremony", FormMethod.Post)) { %>

        <%= Html.AntiForgeryToken() %>
        <ul class="registration_form">
            
            <li><strong>User:</strong>
                <%= this.Select("userId").Options(Model.Users, x=>x.Id, x=>x.FullName).Selected(Model.User != null ? Model.User.Id : 0).FirstOption("--Select a User--") %>
            </li>
            <li>
                <strong>&nbsp;</strong>
                <%= Html.SubmitButton("submit", "Save", new { @class="button" })%>
                |
                <%= Html.ActionLink<CeremonyController>(a => a.EditPermissions(Model.Ceremony.Id) , "Cancel") %>
            </li>

        </ul>

    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
