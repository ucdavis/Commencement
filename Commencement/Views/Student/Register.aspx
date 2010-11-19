<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.RegistrationModel>"
    MasterPageFile="~/Views/Shared/Site.Master" %>
<%--<%@ Import Namespace="Resources" %>--%>
<%@ Import Namespace="Commencement.Core.Resources" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>

<asp:Content runat="server" ID="Content" ContentPlaceHolderID="TitleContent">
    Commencement Registration</asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="HeaderContent">
</asp:Content>

<asp:Content runat="server" ID="Content3" ContentPlaceHolderID="logoContent">
    
</asp:Content>

<asp:Content runat="server" ID="Content2" ContentPlaceHolderID="MainContent">
    <%--<h1><%= Model.Ceremony.Name %></h1>--%>
    <p>
        <% if (Model.Ceremonies.Min(a=>a.PrintingDeadline) > DateTime.Now) { %>
            <%= Html.Encode(string.Format(StaticValues.Txt_Introduction, Model.Student.FirstName)) %>
        <% } else { %>
            <%: string.Format(StaticValues.Txt_Introduction_AfterPrintingDeadling, Model.Student.FirstName) %>
        <% } %>
    </p>
    
    <%= Html.ValidationSummary("Please correct all errors below") %>
    <%= Html.ClientSideValidation<Commencement.Core.Domain.Registration>("Registration") %>
    
    <% using (Html.BeginForm()) { %>
        <%= Html.AntiForgeryToken() %>

    <% Html.RenderPartial("RegistrationEditForm"); %>    
    
    <h3>
        <%= string.Format(StaticValues.Txt_Disclaimer) %>
        
        <br />
        <label for="agreeToDisclaimer">I Agree</label> <%= Html.CheckBox("agreeToDisclaimer", new { @class = "required" }) %>
    </h3>

    <input type="submit" value="Register for Commencement" />
    
    <% } %>
    
</asp:Content>
