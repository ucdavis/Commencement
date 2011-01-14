<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.MoveMajorViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Commencement | Admin - Move Major
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Move Major</h2>

    <%: Html.AntiForgeryToken() %>

    <ul class="registration_form">
        <li><strong>Major:</strong>
            <%= this.Select("Major").Options(Model.MajorCodes, x=>x.Id,x=>x.Name).FirstOption("--Select a Major--") %>
        </li>
        <li><strong>Ceremony:</strong>
            <%= this.Select("Ceremony").Options(Model.Ceremonies, x=>x.Id, x=>x.DateTime.ToString("g")).FirstOption("--Select a Ceremony--") %>
        </li>
        <li><strong>Validation:</strong>
            <span id="validationMessage"></span>
        </li>
    </ul>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    <style type="text/css">
        select { width: 300px }
    </style>

    <script type="text/javascript">
        $(function () {
            $("select").change(function () {
                var major = $("#Major").val();
                var ceremony = $("#Ceremony").val();

                if (major == "" || ceremony == "") alert("not ready");
                else {
                    var url = '<%: Url.Action("ValidateMoveMajor", "Admin") %>';
                    //$.post(url, { majorCode: major, ceremonyId: ceremony, __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val() }, function (data) { alert("hi"); });
                    $.getJSON(url, { majorCode: major, ceremonyId: ceremony }, function (data) { $("#validationMessage").html(data); });
                }
            });
        });
    </script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>
