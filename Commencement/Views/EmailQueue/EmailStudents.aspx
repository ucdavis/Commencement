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

    <% using (Html.BeginForm()) { %>
    
        <%= Html.AntiForgeryToken() %>

        <ul class="registration_form" id="left_bar">
        
            <li><strong>Ceremony:</strong>
                <%= this.Select("EmailStudents.Ceremony").Options(Model.Ceremonies, x=>x.Id, x=> string.Format("{0} ({1})", x.CeremonyName, x.DateTime)).FirstOption("--Select Ceremony--").Selected(Model.Ceremony != null ? Model.Ceremony.Id : 0) %>
                <%: Html.ValidationMessage("Ceremony", "*") %>
            </li>
            <li><strong>Student Population:</strong>
                <%--<%: Html.DropDownListFor(model => model.EmailType, new SelectList(Enum.GetValues(typeof(EmailStudentsViewModel.MassEmailType))), "--Select Population--", new {@class="hastip", title="Description here"}) %>--%>
                <%: Html.DropDownList("EmailStudents.EmailType", new SelectList(Enum.GetValues(typeof(EmailStudentsViewModel.MassEmailType)), Model.EmailType), "--Select Population--", new {@class="hastip", title="hello"})%>
            </li>
            <li><strong>Subject:</strong>
                <%: Html.TextBox("EmailStudents.Subject", Model.Subject) %>
                <%: Html.ValidationMessage("Subject", "*") %>
            </li>
            <li><strong>Body:<%: Html.ValidationMessage("Body", "*") %></strong>
                <%: Html.TextArea("EmailStudents.Body", Model.Body) %>
            </li>
            <li><strong>&nbsp;</strong>
                <input type="submit" value="Send" class="button" /> |
                <%: Html.ActionLink("Cancel", "Index", "Admin") %>
            </li>
        </ul>

        <div id="right_bar">
            <div class="tokens">
                <ul>
                    <li><a href="javascript:;" class="add_token" data-token="{StudentId}">Student Id</a></li>
                    <li><a href="javascript:;" class="add_token" data-token="{StudentName}">Student Name</a></li>
                    <li><a href="javascript:;" class="add_token" data-token="{CeremonyName}">Ceremony Name</a></li>
                    <li><a href="javascript:;" class="add_token" data-token="{CeremonyTime}">Ceremony Time</a></li>
                    <li><a href="javascript:;" class="add_token" data-token="{CeremonyLocation}">Ceremony Location</a></li>
                </ul>
            </div>
        </div>
        <div style="clear: both;"></div>
    <% } %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

    <script src="<%= Url.Content("~/Scripts/tiny_mce/jquery.tinymce.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.enableTinyMce.js") %>" type="text/javascript"></script>

    <script type="text/javascript">

        $(function() {

            $("#EmailStudents_Body").enableTinyMce({ script_location: '<%= Url.Content("~/Scripts/tiny_mce/tiny_mce.js") %>', overrideWidth: "700" });

            $(".add_token").click(function (event) {
                tinyMCE.execInstanceCommand("EmailStudents_Body", "mceInsertContent", false, $(this).data("token"));
            });
            
        });

    </script>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>
