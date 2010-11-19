<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Core.Domain.TermCode>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Edit <%= Html.Encode(Model.Id) %></h2>

    <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>
        <%= Html.AntiForgeryToken() %>
        <%= Html.HiddenFor(a => a.Id) %>
        <fieldset>
            <legend>Fields</legend>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Name) %>
            </div>
            <div class="editor-field">
                <%: Html.DisplayFor(model => model.Name) %>
                <%--<%: Html.ValidationMessageFor(model => model.Name) %>--%>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.IsActive) %>
            </div>
            <div class="editor-field">
                <%: Html.DisplayFor(model => model.IsActive) %>
                <%--<%: Html.ValidationMessageFor(model => model.IsActive) %>--%>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.LandingText) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.LandingText) %>
                <%: Html.ValidationMessageFor(model => model.LandingText) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.RegistrationWelcome) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.RegistrationWelcome) %>
                <%: Html.ValidationMessageFor(model => model.RegistrationWelcome) %>
            </div>

            
            <p>
                <input type="submit" value="Save" />
            </p>
        </fieldset>

    <% } %>

    <div>
        <%: Html.ActionLink("Back to List", "Index") %>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>

