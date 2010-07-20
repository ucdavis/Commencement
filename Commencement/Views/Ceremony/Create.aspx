<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.CreateCommencementViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create Commencement
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Create</h2>

    <% using(Html.BeginForm("Create", "Ceremony", FormMethod.Post)) { %>

        <%= Html.AntiForgeryToken() %>

    <ul class="registration_form">
        <li>
            <strong>Term Code:</strong>
            <%= this.Select("vTermCode").Options(Model.TermCodes, x=>x.Id, x=>x.Description) %>
        </li>
        <li>
            <strong>Date/Time:</strong>
            <%= Html.TextBox("DateTime") %>
        </li>
        <li>
            <strong>Location:</strong>
            <%= Html.TextBox("Location") %>
        </li>
        <li>
            <strong>Tickets per Student:</strong>
            <%= Html.TextBox("TicketsPerStudent") %>
        </li>
        <li>
            <strong>Total Tickets:</strong>
            <%= Html.TextBox("TotalTickets") %>
        </li>
        <li>
            <strong>Registration Deadline:</strong>
            <%= Html.TextBox("RegistrationDeadline") %>
        </li>
        
        <li>
            <strong>Majors:</strong>
            <span>
            
                <%= Html.ListBox("Majors", Model.MajorCodes.Select(a=> new SelectListItem(){Text = a.Name, Value = a.Id}))%>
            
            </span>
        </li>
        
        <li>
            <strong></strong>
            <input type="submit" value="Submit" />
        </li>
    </ul>
    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

    <script src="<%= Url.Content("~/Scripts/jquery.ui.datetimepicker.min.js") %>" type="text/javascript"></script>
    
    <script type="text/javascript">
        $(function() {
            $("#DateTime").datetimepicker();
            $("#RegistrationDeadline").datepicker();
        });
    </script>

<%--    <script src="<%= Url.Content("~/Scripts/ui.multiselect.js") %>" type="text/javascript"></script>

    <script type="text/javascript">
        $(function() {
            $("#AvailableMajors").multiselect();
        });
    </script>--%>

</asp:Content>
