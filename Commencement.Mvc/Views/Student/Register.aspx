<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.RegistrationModel>"
    MasterPageFile="~/Views/Shared/Site.Master" %>
<%@ Import Namespace="Commencement.Controllers.Services" %>
<%@ Import Namespace="Commencement.Core.Resources" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>

<asp:Content runat="server" ID="Content" ContentPlaceHolderID="TitleContent">
    Commencement Registration</asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="HeaderContent">

    <style type="text/css">
        #petition-warning, #printing-warning, #late-reg
        {
            border: 1px solid #eed3d7;
            background-color: #F4F4F4;
            padding: 1em;
            margin: 10px 20px;
            background-color: #f2dede;
            color: #b94a48;
            font-weight: bold;
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

    <%: Html.HtmlEncode(TermService.GetCurrent().RegistrationWelcome) %>

    <% if (Model.Participations.Any(a => a.Ceremony.IsPastPrintingDeadline())) { %>
        <div id="printing-warning">
            <p>Due to the late registration and printing deadlines we cannot guarantee your name will appear in the program or that you will receive the maximum number of tickets allotted per person.</p>
        </div>
    <% } %>
    
    <% if (TermService.GetCurrent().RegistrationDeadline.Date < DateTime.Now.Date) { %>
        <div id="late-reg">
            <p>The registration deadline has passed.  You can petition but may not receive as many tickets.</p>
        </div>
    <% } %>

    <%= Html.ValidationSummary("Please correct all errors below") %>
    
    <% using (Html.BeginForm()) { %>
        <%= Html.AntiForgeryToken() %>

        <% Html.RenderPartial("RegistrationEditForm"); %>    
        
        <fieldset>
            
            <legend>GradImages</legend>
            
            <div class="disclaimer">
                May we provide your information to GradImages? GradImages will use your information to provide you with your proof(s) after the ceremony. There is no obligation to purchase photos and your personal information will be used for the delivery of proofs only. GradImages will not provide your information to any other entity, except for if required by law.
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
                    <%: Html.ValidationMessage("agreeToDisclaimer") %>
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
