<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.ChangeMajorViewModel>" %>
<%@ Import Namespace="Commencement.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	ChangeMajor
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="btn">
    <li>
    <%= Html.ActionLink<AdminController>(a=>a.StudentDetails(Model.Student.Id, true), "Back to Student Details") %>
    </li></ul>

    <h2>Change Major</h2>

    <% using (Html.BeginForm()) { %>

    <%= Html.AntiForgeryToken() %>

    <ul class="registration_form">
        <li><strong>Student Id:</strong>
            <%= Html.Encode(Model.Student.StudentId) %>
        </li>
        <li><strong>Name:</strong>
            <%= Html.Encode(Model.Student.FullName) %>
        </li>
        <li><strong>Registered Major:</strong>
            <%= Html.Encode(Model.Registration.Major.Id) %>
        </li>
        <li><strong># of Tickets</strong>
            <%= Html.Encode(Model.Registration.NumberTickets) %>
        </li>
        
        <li></li>
        <li><strong>Change Major:</strong>
            <%= this.Select("ChangeMajor").Options(Model.MajorCodes, x=>x.Id, x=>x.Id).FirstOption("--Select a Major--") %>
        </li>
        <li id="check_response"></li>
        <li><strong></strong>
            <%= Html.SubmitButton("Submit", "Submit") %>
        </li>
    </ul>
    
    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function() {
            $("#ChangeMajor").change(function() {
                $.getJSON('<%= Url.Action("ChangeMajorValidation", "Admin") %>'
                    , { regId: '<%= Model.Registration.Id %>', major: $(this).val() }
                    , function(result) { $("#check_response").html(result); }
            );
            });
        });
    </script>
</asp:Content>
