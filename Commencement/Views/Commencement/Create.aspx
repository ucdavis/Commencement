<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.CreateCommencementViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create Commencement
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Create</h2>

    <% using(Html.BeginForm("Create", "Commencement", FormMethod.Post)) { %>

    <ul class="registration_form">
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
            
                <%= Html.ListBox("AvailableMajors", Model.MajorCodes.Select(a=> new SelectListItem(){Text = a.Name, Value = a.Id}))%>
            
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

    <script src="<%= Url.Content("~/Scripts/ui.multiselect.js") %>" type="text/javascript"></script>

    <script type="text/javascript">
        $(function() {
            $("#AvailableMajors").multiselect();
        });
    </script>

</asp:Content>
