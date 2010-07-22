<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Core.Domain.Template>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Create</h2>

    <%= Html.ValidationSummary("Create was unsuccessful. Please correct the errors and try again.") %>
    <%= Html.ClientSideValidation<Commencement.Core.Domain.Ceremony>("") %>

    <% using (Html.BeginForm()) {%>

        <%= Html.AntiForgeryToken() %>

        <ul class="registration_form">
            <li>
                <strong>BodyText:</strong>
                <%= Html.TextAreaFor(a=>a.BodyText) %>
                <%= Html.ValidationMessageFor(a=>a.BodyText) %>
            </li>
            
            <li>
                <h2>Template Types</h2>
            </li>
            <li>
                <strong>Registration Confirmation</strong>
                <%= Html.CheckBox("RegistrationConfirmation", Model.RegistrationConfirmation) %>
            </li>
            
            <li>
                <strong>Registration Petition</strong>
                <%= Html.CheckBox("RegistrationPetition", Model.RegistrationPetition) %>
            </li>
            
            <li>
                <strong>Extra Ticket Petition:</strong>
                <%= Html.CheckBox("ExtraTicketPetition", Model.ExtraTicketPetition) %>
            </li>
            
            <li>
                <strong></strong>
                <input type="submit" value="Create" />
            </li>
        </ul>

    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    <script src="<%= Url.Content("~/Scripts/tiny_mce/jquery.tinymce.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.enableTinyMce.js") %>" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function() {
            $("#BodyText").enableTinyMce({ script_location: '<%= Url.Content("~/Scripts/tiny_mce/tiny_mce.js") %>' });
        });
   </script>
</asp:Content>

