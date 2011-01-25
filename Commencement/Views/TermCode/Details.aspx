<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Core.Domain.TermCode>" %>
<%@ Import Namespace="Commencement.Controllers" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Commencement | Term Code Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<ul class="btn">
    <li>
        <%= Html.ActionLink<TermCodeController>(a => a.Index() , "Back to List") %>
    </li>
    <li>
        <%= Html.ActionLink<TermCodeController>(a => a.Edit(Model.Id) , "Edit") %>
    </li>
</ul>
    <h2>Details</h2>

        <ul class="registration_form">
            <li>
                <strong><%=Html.Label("Term Code:") %></strong>
                <%=Html.DisplayFor(a => a.Id) %>
            </li>
            <li>
               <strong> <%=Html.Label("Description:")%></strong>
                <%=Html.DisplayFor(a => a.Name) %>
            </li>
            <li>
                <strong><%=Html.Label("IsActive:")%></strong>
                <%=Html.DisplayFor(a => a.IsActive) %>
            </li>
            <li>                
                <fieldset>
                <legend><strong>LandingText</strong></legend>
                <%= Html.HtmlEncode(Model.LandingText)%>
                </fieldset>
            </li>
            <li>
                <fieldset>
                <legend><strong>RegistrationWelcome</strong></legend>
                <%=Html.HtmlEncode(Model.RegistrationWelcome)%>
                </fieldset>
            </li>
        </ul>
      


</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>

