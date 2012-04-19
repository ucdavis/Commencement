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
        
        <fieldset>
            
            <legend>Grad Track</legend>
            
            <div class="disclaimer">
                May we provide your information to Grad Trak? Grad Trak will use your information to provide you with your photo proof(s) after the ceremony. There is no obligation to purchase photos and your personal information will be used for the delivery of proofs only. Grad Trak will not provide the information to any other entity, except for if required by law.
            </div>
            
            <ul class="registration_form">
                <li><strong>&nbsp;</strong>
                    <%: Html.CheckBox("gradTrack") %><label for="gradTrack">I authorize</label>
                </li>
            </ul>

        </fieldset>

        <fieldset>
            
            <legend>Legal Disclaimer</legend>
            
            <div class="disclaimer">
                <%= string.Format(StaticValues.Txt_Disclaimer) %>
            </div>
            
            <ul class="registration_form">
                <li><strong>&nbsp;</strong>
                    <%= Html.CheckBox("agreeToDisclaimer", new { @class = "required" }) %><label for="agreeToDisclaimer">I Agree</label>
                    <span style="color: red; font-weight: bold;">*</span>
                </li>
            </ul>

        </fieldset>

        <fieldset>
            
            <ul class="registration_form">
                <li><strong>&nbsp;</strong>
                    <input type="submit" value="Register for Commencement" class="button" />            
                </li>
            </ul>

        </fieldset>

    
    
    <% } %>
    
</asp:Content>
