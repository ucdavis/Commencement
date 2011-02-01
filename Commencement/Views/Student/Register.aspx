<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.RegistrationModel>"
    MasterPageFile="~/Views/Shared/Site.Master" %>
<%@ Import Namespace="Commencement.Core.Resources" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>

<asp:Content runat="server" ID="Content" ContentPlaceHolderID="TitleContent">
    Commencement Registration</asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="HeaderContent">
</asp:Content>

<asp:Content runat="server" ID="Content3" ContentPlaceHolderID="logoContent">
    
</asp:Content>

<asp:Content runat="server" ID="Content2" ContentPlaceHolderID="MainContent">

    <p><%: Html.HtmlEncode(string.Format(Model.Ceremonies.FirstOrDefault().TermCode.RegistrationWelcome, Model.Student.FullName, Model.Ceremonies.FirstOrDefault().Name)) %></p>

    <%= Html.ValidationSummary("Please correct all errors below") %>
    <%= Html.ClientSideValidation<Commencement.Core.Domain.Registration>("Registration") %>
    
    <% using (Html.BeginForm()) { %>
        <%= Html.AntiForgeryToken() %>

        <% Html.RenderPartial("RegistrationEditForm"); %>    

    <h3>
        <div class="legaldisclaimer"><%= Html.CheckBox("gradTrack") %><label for="gradTrack"><%: string.Format(StaticValues.Txt_GradTrack) %></label></div>
    </h3>
    
    <h3>
        <%= string.Format(StaticValues.Txt_Disclaimer) %>
        
        <br />
        <div class="legaldisclaimer"><%= Html.CheckBox("agreeToDisclaimer", new { @class = "required" }) %><label for="agreeToDisclaimer">I Agree</label></div>
    </h3>



    <input type="submit" value="Register for Commencement" />
    
    <% } %>
    
</asp:Content>
