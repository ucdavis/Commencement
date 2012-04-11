<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.RegistrationModel>"
    MasterPageFile="~/Views/Shared/Site.Master" %>
<%@ Import Namespace="Commencement.Core.Resources" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>

<asp:Content runat="server" ID="Content" ContentPlaceHolderID="TitleContent">
    Commencement Registration</asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="HeaderContent">

    <style type="text/css">
        #petition-warning, #printing-warning
        {
            border: 1px solid red;
            background-color: #F4F4F4;
            padding: 0 10px;
            margin: 10px 20px;
        }
        
        #petition-warning p, #printing-warning p
        {
            margin: 1em 0;
        }
    </style>

</asp:Content>

<asp:Content runat="server" ID="Content3" ContentPlaceHolderID="logoContent">
    
</asp:Content>

<asp:Content runat="server" ID="Content2" ContentPlaceHolderID="MainContent">

    <p><%: Html.HtmlEncode(string.Format(Model.Ceremonies.FirstOrDefault().TermCode.RegistrationWelcome, Model.Student.FullName, Model.Ceremonies.FirstOrDefault().CeremonyName)) %></p>

    <% if (Model.Participations.Any(a => a.NeedsPetition)) { %>
    <div id="petition-warning">
        <p>According to our records you do not meet the minimum requirements to participate in the commencement ceremony.  See our website for the list of reuqirements: <a href="www.caes.ucdavis.edu/commencement">www.caes.ucdavis.edu/commencement</a></p>
        <p>Please complete the petition if you would like to participate.</p>
    </div>
    <% } %>

    <% if (Model.Participations.Any(a => a.Ceremony.IsPastPrintingDeadline())) { %>
        <div id="printing-warning">
            <p>Due to the late registration and printing deadlines we  cannot guarantee your name will appear in the program or that you will receive the maximum number of tickets allotted per person.</p>
        </div>
    <% } %>

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
