<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.TemplateCreateViewModel>" %>
<%@ Import Namespace="Commencement.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Commencement | Create Email Template
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="fix">
<ul class="btn">
    <li>
    <%= Html.ActionLink<TemplateController>(a=>a.Index(Model.Ceremony.Id), "Back to List") %>
    </li></ul>

    <h2>Create</h2>

    <%= Html.ValidationSummary("Create was unsuccessful. Please correct the errors and try again.") %>
    <%= Html.ClientSideValidation<Commencement.Core.Domain.Ceremony>("") %>

    <% using (Html.BeginForm()) {%>

        <%= Html.AntiForgeryToken() %>
        <%: Html.Hidden("Ceremony", Model.Ceremony.Id) %>

            <ul class="registration_form" id="left_bar">
            <li>
                <strong>Template Type:</strong>
                
                <%= this.Select("TemplateType")
                        .Options(Model.TemplateTypes, x=>x.Id, x=>x.Name)
                        .FirstOption("--Select a Template Type--")
                        .Selected(Model.Template != null ? Model.Template.TemplateType.Id.ToString() : string.Empty) %>
            </li>
            <li>
                <strong>Subject: </strong>
                <%: Html.TextBox("Subject", Model.Template != null ? Model.Template.Subject : string.Empty, new { @style="width:20em;"})%>
            </li>
            <li>
                <strong>BodyText:</strong>
                <%= Html.TextArea("BodyText", Model.Template != null ? Model.Template.BodyText : string.Empty) %>
                <%= Html.ValidationMessageFor(a=>a.Template.BodyText) %> 
            </li>
            <li>
                <input type="submit" value="Create" />
                <input type="button" value="Send Test Email" id="send-test" />
            </li>
            </ul>
                 
           <div id="right_bar">
                <ul class="registration_form">
                    <% foreach (var a in Model.TemplateTypes) { %>
                        <div id="<%: a.Code %>" class="tokens" style='<%: Model.Template != null && Model.Template.TemplateType.Code == a.Code ? "display:block;" : "display:none;" %>'>
                            <% foreach (var b in a.TemplateTokens) { %>
                                <li><a href="javascript:;" class="add_token" data-token="<%: b.Token %>"><%: b.Name %></a></li>
                            <% } %>
                        </div>
                    <% } %>
                </ul>
           </div>
    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    <script src="<%= Url.Content("~/Scripts/tiny_mce/jquery.tinymce.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.enableTinyMce.js") %>" type="text/javascript"></script>

    <script type="text/javascript">

        var templatecodes = [];

        $(document).ready(function() {
            $("#BodyText").enableTinyMce({ script_location: '<%= Url.Content("~/Scripts/tiny_mce/tiny_mce.js") %>', overrideWidth: "700" });
            
            $(".add_token").click(function(event) {
                tinyMCE.execInstanceCommand("BodyText", "mceInsertContent", false, $(this).data("token"));
            });

            <% foreach(var a in Model.TemplateTypes) { %>
                templatecodes['<%: a.Name %>'] = '<%: a.Code %>';
            <% } %>

            $("#TemplateType").change(function(){
                $(".tokens").hide();    // hide all token containers

                // show the one we want
                var selected = $("#TemplateType option:selected").text();
                $("#" + templatecodes[selected]).show();
            });

            $("#send-test").click(function(){
                var url = '<%: Url.Action("SendTestEmail", "Template") %>';
                var subject = $("#Subject").val();
                var txt = tinyMCE.get("BodyText").getContent();
                
                $.getJSON(url, {subject:subject,message: txt}, function(result){if (result) alert("Message has been mailed to you.");});
                
            });
        });

   </script>
   </div>
</asp:Content>

