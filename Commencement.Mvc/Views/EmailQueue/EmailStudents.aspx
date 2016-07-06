<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.EmailStudentsViewModel>" %>
<%@ Import Namespace="Commencement.Controllers" %>
<%@ Import Namespace="Commencement.Controllers.ViewModels" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Commencement | EmailStudents
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="btn">
        <li><%: Html.ActionLink("Back to List", "Index") %></li>
    </ul>

    <h2>Email Students</h2>

    <%= Html.ValidationSummary("Please correct the erros and try again.") %>

    <% using (Html.BeginForm("EmailStudents", "EmailQueue", FormMethod.Post, new {enctype="multipart/form-data"})) { %>
    
        <%= Html.AntiForgeryToken() %>

        <ul class="registration_form" id="left_bar">
        
            <li><strong>Ceremony:</strong>
                <%= this.Select("EmailStudents.Ceremony").Options(Model.Ceremonies, x=>x.Id, x=> string.Format("{0} ({1})", x.CeremonyName, x.DateTime)).FirstOption("--Select Ceremony--").Selected(Model.Ceremony != null ? Model.Ceremony.Id : 0).Class("hastip").Title("Required") %>
                <%: Html.ValidationMessageFor(a => a.Ceremony) %>
            </li>
            <li><strong>Template:</strong>
                <%: Html.DropDownList("EmailStudents.TemplateType", new SelectList(Model.TemplateTypes, "Id", "Name"), "--Select Template--", new { @class = "hastip", title = "Optional" })%>
            </li>
            <li><strong>Student Population:</strong>
                <%: Html.DropDownList("EmailStudents.EmailType", new SelectList(Enum.GetValues(typeof(EmailStudentsViewModel.MassEmailType)), Model.EmailType), "--Select Population--", new {@class="hastip", title="Required"})%>
                <%: Html.ValidationMessageFor(a => a.EmailType)%>
            </li>
            <li><strong>Attachment</strong>
                <input type="file" name="file"/>
            </li>
            <li><strong>Subject:</strong>
                <%: Html.TextBox("EmailStudents.Subject", Model.Subject) %>
                <%: Html.ValidationMessageFor(a => a.Subject) %>
            </li>
            <li><strong>Body:</strong>
                <%: Html.TextArea("EmailStudents.Body", Model.Body) %>
                <%: Html.ValidationMessageFor(a => a.Body) %>
            </li>
            <li><strong>&nbsp;</strong>
                <input type="submit" value="Send" class="button" />
                <input type="button" value="Send Test Email" id="send-test" class="button" /> |
                <%: Html.ActionLink("Cancel", "Index", "Admin") %>
                
            </li>
        </ul>

           <div id="right_bar">
                <ul class="registration_form">
                    <% foreach (var a in Model.TemplateTypes) { %>
                        <div id="<%: a.Code %>" class="tokens" style='<%: Model.TemplateType != null && Model.TemplateType.Code == a.Code ? "display:block;" : "display:none;" %>'>
                            <li><a href="javascript:;" class="add_token" data-token="<%: @Html.Encode("{AttachmentLink}") %>"><%: @Html.Encode("Attachment Link")  %></a></li>
                            <% foreach (var b in a.TemplateTokens) { %>
                                <li><a href="javascript:;" class="add_token" data-token="<%: b.Token %>"><%: b.Name %></a></li>
                            <% } %>
                        </div>
                    <% } %>
                </ul>
           </div>
           

        <div style="clear: both;"></div>
    <% } %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

    <script src="<%= Url.Content("~/Scripts/tiny_mce/jquery.tinymce.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.enableTinyMce.js") %>" type="text/javascript"></script>

    <script type="text/javascript">
        
        var templatecodes = [];

        $(function () {
            
            <% foreach(var a in Model.TemplateTypes) { %>
                templatecodes['<%: a.Name %>'] = '<%: a.Code %>';
            <% } %>
            

            $("#EmailStudents_TemplateType").change(function(){
                $(".tokens").hide();    // hide all token containers

                // show the one we want
                var selected = $("#EmailStudents_TemplateType option:selected").text();
                $("#" + templatecodes[selected]).show();
            });

            $("#EmailStudents_Body").enableTinyMce({ script_location: '<%= Url.Content("~/Scripts/tiny_mce/tiny_mce.js") %>', overrideWidth: "700" });

            $(".add_token").click(function (event) {
                tinyMCE.execInstanceCommand("EmailStudents_Body", "mceInsertContent", false, $(this).data("token"));
            });

            $("#EmailStudents_TemplateType").change(function (e) {

                var id = $(this).val();
                var ceremonyId = $("#EmailStudents_Ceremony").val();
                var url = '<%: Url.Action("LoadTemplate", "Template") %>';

                $.get(url, { templateId: id, ceremonyId: ceremonyId }, function (results) {
                    $("#EmailStudents_Subject").val(results.subject);
                    $("#EmailStudents_Body").text(results.body);
                });
                


            });

            $("#send-test").click(function() {
                var url = '<%: Url.Action("SendTestEmail", "Template") %>';
                var subject = $("#EmailStudents_Subject").val();
                var txt = tinyMCE.get("EmailStudents_Body").getContent();
                var antiForgeryToken = $("input[name='__RequestVerificationToken']").val();

                $.post(url, { subject: subject, message: txt, __RequestVerificationToken: antiForgeryToken }, function(result) {
                    if (result) alert("Message has been mailed to you.");
                    else alert("there was an error sending test email");
                }
                );
            });

        });

    </script>
    
    <style type="text/css">
        #right_bar { margin-top: 170px;}
    </style>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>
