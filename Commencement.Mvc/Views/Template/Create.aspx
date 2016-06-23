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
    </li>
    </ul>

    <h2>Create</h2>

    <%= Html.ValidationSummary("Create was unsuccessful. Please correct the errors and try again.") %>

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

                <input type="button" id="copy-template" value="Copy" class="button" />
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
            <li><strong>&nbsp;</strong>
                <input type="submit" value="Create" class="button" />
                <input type="button" value="Send Test Email" id="send-test" class="button" />
                |
                <%= Html.ActionLink<TemplateController>(a=>a.Index(Model.Ceremony.Id), "Cancel") %>
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

    <div id="copy-dialog" title="Copy">
    
        <table id="available-templates">
            <thead>
                <tr>
                    <td></td>
                    <td>Ceremony</td>
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>

    </div>

    <div style="clear: both;"></div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    <script src="<%= Url.Content("~/Scripts/tiny_mce/jquery.tinymce.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.enableTinyMce.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/dateFormat.js") %>" type="text/javascript"></script>

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

                var antiForgeryToken = $("input[name='__RequestVerificationToken']").val();

                $.post(url, {subject:subject, message:txt,__RequestVerificationToken:antiForgeryToken }, function(result){
                    if(result) alert("Message has been mailed to you."); 
                    else alert("there was an error sending test email");
                    }
                );
            });
        });

   </script>

   <script type="text/javascript">
       $(document).ready(function () {

           /*
           $.ajaxSetup({ "error": function (XMLHttpRequest, textStatus, errorThrown) {
           alert(textStatus);
           alert(errorThrown);
           alert(XMLHttpRequest.responseText);
           }
           });
           */


           $("#copy-dialog").dialog({
               width: 300,
               modal: true,
               autoOpen: false,
               buttons: { Cancel: function () { $(this).dialog("close"); } }
           });

           $("#copy-template").click(function () {

               // open the dialog
               $("#copy-dialog").dialog("open");

               // populate the dialog
               var url = '<%: Url.Action("LoadOldTemplates") %>';
               var id = $("#TemplateType").val();

               $.getJSON(url, { "templateTypeId": id }, function (result) {

                   // load the tbody
                   var tbody = $("#available-templates tbody");
                   // clear the tbody
                   tbody.empty();

                   $.each(result, function (index, item) {

                       var date = new Date(parseInt(item.Name.substr(6)));

                       var row = $("<tr>");
                       var button = $("<input>").attr("type", "button").val("Select").data("id", item.Id).addClass("select-template button");
                       var col1 = $("<td>").append(button);
                       var col2 = $("<td>").html(dateFormat(date, "m/dd/yy h:MM TT"));

                       button.button();

                       row.append(col1).append(col2);

                       tbody.append(row);
                   });
               });
           });

           $(".select-template").live("click", function () {

               var url = '<%: Url.Action("LoadOldTemplate") %>';
               var id = $(this).data("id");

               $.getJSON(url, { "templateId": id }, function (result) {

                   var subject = result.Subject;
                   var body = result.BodyText;

                   // set the controls
                   $("#Subject").val(subject);
                   tinyMCE.execInstanceCommand("BodyText", "mceReplaceContent", false, body);

                   // close the dialog
                   $("#copy-dialog").dialog("close");
               });

           });
       });
   </script>
</asp:Content>

