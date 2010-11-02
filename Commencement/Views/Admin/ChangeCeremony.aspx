<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.ChangeCeremonyViewModel>" %>
<%@ Import Namespace="Commencement.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	ChangeCeremony
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="btn">
    <li>
    <%= Html.ActionLink<AdminController>(a=>a.StudentDetails(Model.Student.Id, true), "Back to Student Details") %>
    </li></ul>

    <h2>Change Ceremony</h2>

    <%= Html.ValidationSummary("Please correct all errors below") %>

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
            <%= Html.Encode(Model.Registration.Major.Name) %>
        </li>
        <li><strong># of Tickets</strong>
            <%= Html.Encode(Model.Registration.NumberTickets) %>
        </li>
        
        <li></li>
        <li><strong>Change Ceremony:</strong>
            <%= this.Select("ceremonyId").Options(Model.Ceremonies, x=>x.Id, x=>x.DateTime.ToString("g")).FirstOption("--Select a Ceremony--") %>
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
            $("#ceremonyId").change(function() {
                $.getJSON('<%= Url.Action("ChangeCeremonyValidation", "Admin") %>'
                    , { regId: '<%= Model.Registration.Id %>', ceremonyId: $(this).val() }
                    , function(result) { $("#check_response").html(result); }
            );
            });
        });
    </script>
</asp:Content>
