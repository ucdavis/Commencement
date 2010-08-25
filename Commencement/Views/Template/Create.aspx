<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.TemplateCreateViewModel>" %>
<%@ Import Namespace="Commencement.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="fix">
<ul class="btn">
    <li>
    <%= Html.ActionLink<TemplateController>(a=>a.Index(), "Back to List") %>
    </li></ul>

    <h2>Create</h2>

    <%= Html.ValidationSummary("Create was unsuccessful. Please correct the errors and try again.") %>
    <%= Html.ClientSideValidation<Commencement.Core.Domain.Ceremony>("") %>

    <% using (Html.BeginForm()) {%>

        <%= Html.AntiForgeryToken() %>


            <p>
                <strong>Template Type:</strong>
                
                <%= this.Select("TemplateType")
                        .Options(Model.TemplateTypes, x=>x.Id, x=>x.Name)
                        .FirstOption("--Select a Template Type--")
                        .Selected(Model.Template != null ? Model.Template.TemplateType.Id.ToString() : string.Empty) %>
            </p>
            <p>
                <strong>BodyText:</strong>
                <%--<%= Html.TextAreaFor(a=>a.Template.BodyText) %>--%>
                <%= Html.TextArea("BodyText", Model.Template != null ? Model.Template.BodyText : string.Empty) %>
                <%= Html.ValidationMessageFor(a=>a.Template.BodyText) %> </p>
                
                <div id="right_menu">
                        <ul class="registration_form">
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
                        <li><a href="javascript:;" class="add_token">{DistributionMethod}</a></li>
                    </div>
                    <div id="registration_tokens" >
                        <li><a href="javascript:;" class="add_token">{SpecialNeeds}</a></li>
                        <li><a href="javascript:;" class="add_token">{Major}</a></li>
                    <div id="registration_petition_tokens" >
                        <li><a href="javascript:;" class="add_token">{ExceptionReason}</a></li>
                        <li><a href="javascript:;" class="add_token">{CompletionTerm}</a></li>
                    </div>
                       <li><a href="javascript:;" class="add_token">{NumberOfExtraTickets}</a></li>
                       <li><a href="javascript:;" class="add_token">{PetitionDecision}</a></li>
               </ul>
               </div>     
 
           
            

            
            <p>
                <strong></strong>
                <input type="submit" value="Create" />
            </p>
        

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
        });
   </script>
   </div>
</asp:Content>

