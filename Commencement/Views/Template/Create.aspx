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
                
                <ul>
                    <div id="shared_tokens">
                        <li><a href="javascript:;" class="add_token">{StudentId}</a></li>
                        <li><a href="javascript:;" class="add_token">{StudentName}</a></li>
                        <li><a href="javascript:;" class="add_token">{CeremonyName}</a></li>
                        <li><a href="javascript:;" class="add_token">{CeremonyTime}</a></li>
                        <li><a href="javascript:;" class="add_token">{CeremonyLocation}</a></li>
                        <li><a href="javascript:;" class="add_token">{NumberOfTickets}</a></li>
                        <li><a href="javascript:;" class="add_token">{AddressLine1}</a></li>
                        <li><a href="javascript:;" class="add_token">{AddressLine2}</a></li>
                        <li><a href="javascript:;" class="add_token">{City}</a></li>
                        <li><a href="javascript:;" class="add_token">{State}</a></li>
                        <li><a href="javascript:;" class="add_token">{Zip}</a></li>
                        <li><a href="javascript:;" class="add_token">{TicketReceiveMethod}</a></li>
                    </div>
                    <div id="registration_tokens" style="display:none;">
                        <li><a href="javascript:;" class="add_token">{SpecialNeeds}</a></li>
                    </ul>
                    <div id="registration_petition_tokens" style="display:none;">
                        <li><a href="javascript:;" class="add_token">{ExceptionReason}</a></li>
                        <li><a href="javascript:;" class="add_token">{CompletionTerm}</a></li>
                    </div>
                </ul>
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
            $(".add_token").click(function(event) {
                tinyMCE.execInstanceCommand("BodyText", "mceInsertContent", false, $(this).html());
            });

            $("#RegistrationConfirmation").click(function(event) {
                $("#registration_tokens").show();
                $("#registration_petition_tokens").hide();

                $("#RegistrationPetition").attr("checked", false);
                $("#ExtraTicketPetition").attr("checked", false);
            });
            $("#RegistrationPetition").click(function(event) {
                $("#registration_tokens").show();
                $("#registration_petition_tokens").show();

                $("#RegistrationConfirmation").attr("checked", false);
                $("#ExtraTicketPetition").attr("checked", false);
            });
            $("#ExtraTicketPetition").click(function(event) {
                $("#registration_tokens").hide();
                $("#registration_petition_tokens").hide();

                $("#RegistrationConfirmation").attr("checked", false);
                $("#RegistrationPetition").attr("checked", false);
            });
        });
   </script>
</asp:Content>

