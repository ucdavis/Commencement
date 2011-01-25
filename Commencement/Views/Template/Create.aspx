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

            <p>
                <strong>Template Type:</strong>
                
                <%= this.Select("TemplateType")
                        .Options(Model.TemplateTypes, x=>x.Id, x=>x.Name)
                        .FirstOption("--Select a Template Type--")
                        .Selected(Model.Template != null ? Model.Template.TemplateType.Id.ToString() : string.Empty) %>
            </p>
            <p>
                <strong>Subject: </strong>
                <%: Html.TextBox("Subject", Model.Template != null ? Model.Template.Subject : string.Empty) %>
            </p>
            <p>
                <strong>BodyText:</strong>
                <%= Html.TextArea("BodyText", Model.Template != null ? Model.Template.BodyText : string.Empty) %>
                <%= Html.ValidationMessageFor(a=>a.Template.BodyText) %> 
            </p>
                
<%--                <div id="right_menu">
                      <ul class="registration_form">
                       <% Html.RenderPartial("TokenPartial", Model.Template != null ? Model.Template.TemplateType : new TemplateType()); %>
                      </ul>
               </div>     --%>
 
           <div id="right_menu">
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
        });

   </script>
   </div>
</asp:Content>

