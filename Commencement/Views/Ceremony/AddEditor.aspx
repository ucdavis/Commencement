<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.AddEditorViewModel>" %>
<%@ Import Namespace="Commencement.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	AddEditor
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

        <ul class="btn">
    <li>
    <%= Html.ActionLink<CeremonyController>(a => a.EditPermissions(Model.Ceremony.Id) , "Back to Ceremony") %>
    </li>
    </ul>

    <h2>Add Editor to <%: Model.Ceremony.Name %></h2>

    <% using (Html.BeginForm("AddEditor", "Ceremony", FormMethod.Post)) { %>

        <%= Html.AntiForgeryToken() %>
        <ul class="registration_form">
            
            <li><strong>User:</strong>
                <%= this.Select("userId").Options(Model.Users, x=>x.Id, x=>x.FullName).Selected(Model.User != null ? Model.User.Id : 0).FirstOption("--Select a User--") %>
            </li>
            <li>
                <strong></strong>
                <%= Html.SubmitButton("submit", "Save") %>
            </li>

        </ul>

    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
