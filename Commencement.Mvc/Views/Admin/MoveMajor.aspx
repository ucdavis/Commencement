<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Mvc.Controllers.ViewModels.MoveMajorViewModel>" %>
<%@ Import Namespace="Commencement.Mvc.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Commencement | Move Major
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="btn">
        <li><%: Html.ActionLink<AdminController>(a=>a.Index(), "Back to Home") %></li>
    </ul>

    <h2>Move Major</h2>

    <% using (Html.BeginForm()) { %>
    <%: Html.AntiForgeryToken() %>

    <ul class="registration_form">
        <li><strong>Major:</strong>
            <%= this.Select("majorCode").Options(Model.MajorCodes, x=>x.Id,x=>x.Name).FirstOption("--Select a Major--") %>
        </li>
        <li><strong>Ceremony:</strong>
            <%= this.Select("ceremonyId").Options(Model.Ceremonies, x=>x.Id, x=>x.DateTime.ToString("g")).FirstOption("--Select a Ceremony--") %>
        </li>
        <li><strong>Validation:</strong>
            <span id="validationMessage"></span>
        </li>
        <li><strong>&nbsp;</strong>
            <%: Html.SubmitButton("Save", "Save", new { @class="button" })%>
            |
            <%: Html.ActionLink("Cancel", "Index", "Admin") %>
        </li>
    </ul>
    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    <style type="text/css">
        select { width: 300px }
    </style>

    <script type="text/javascript">
        $(function () {
            $("select").change(function () {
                validateMajor();
            });
        });
        
        function validateMajor() {
            var major = $("#majorCode").val();
            var ceremony = $("#ceremonyId").val();

            if (major != "" && ceremony != "") {
                var url = '<%: Url.Action("ValidateMoveMajor", "Admin") %>';
                $.getJSON(url, { majorCode: major, ceremonyId: ceremony }, function (data) { $("#validationMessage").html(data); });
            }            
        }
    </script>
</asp:Content>