<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Core.Domain.TermCode>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Details</h2>

    <fieldset>
        <legend>Fields</legend>
        <ul>
            <li>
                <%=Html.Label("Term Code:") %>
                <%=Html.DisplayFor(a => a.Id) %>
            </li>
            <li>
                <%=Html.Label("Description:")%>
                <%=Html.DisplayFor(a => a.Name) %>
            </li>
            <li>
                <%=Html.Label("IsActive:")%>
                <%=Html.DisplayFor(a => a.IsActive) %>
            </li>
            <li>
                <%=Html.Label("LandingText:")%>
                <%=Html.DisplayFor(a => a.LandingText) %>
            </li>
            <li>
                <%=Html.Label("RegistrationWelcome:")%>
                <%=Html.DisplayFor(a => a.RegistrationWelcome) %>
            </li>
        </ul>
        
<%--        <div class="display-label">Term Code</div>
        <div class="display-field"><%: Model.Id %></div>

        <div class="display-label">Description</div>
        <div class="display-field"><%: Model.Name %></div>
        
        <div class="display-label">IsActive</div>
        <div class="display-field"><%: Model.IsActive %></div>
        
        <div class="display-label">LandingText</div>
        <div class="display-field"><%: Model.LandingText %></div>
        
        <div class="display-label">RegistrationWelcome</div>
        <div class="display-field"><%: Model.RegistrationWelcome %></div>--%>
        
    </fieldset>
    <p>
        <%--<%: Html.ActionLink("Edit", "Edit", new { /* id=Model.PrimaryKey */ }) %> |--%>
        <%: Html.ActionLink("Back to List", "Index") %>
    </p>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>

