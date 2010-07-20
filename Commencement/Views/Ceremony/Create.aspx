<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.CreateCommencementViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create Commencement
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Create</h2>

    <%= Html.ValidationSummary("Please correct all errors below") %>
    <%= Html.ClientSideValidation<Commencement.Core.Domain.Ceremony>("Ceremony") %>

    <% using(Html.BeginForm("Create", "Ceremony", FormMethod.Post)) { %>

        <%= Html.AntiForgeryToken() %>

    <ul class="registration_form">
        <li>
            <strong>Term Code:</strong>
            <%= this.Select("term").Options(Model.TermCodes, x => x.Id, x => x.Description).FirstOption("--Select a Term--")%>
            
            <%--<%= Html.DropDownList("term", Model.TermCodes.Select(x=>new SelectListItem(){Text = x.Description, Value = x.Id})) %>--%>
        </li>
        <li>
            <strong>Date/Time:</strong>
            <%= Html.TextBoxFor(x=>x.Ceremony.DateTime) %>
            <%= Html.ValidationMessageFor(x=>x.Ceremony.DateTime) %>
        </li>
        <li>
            <strong>Location:</strong>
            <%= Html.TextBoxFor(x=>x.Ceremony.Location) %>
            <%= Html.ValidationMessageFor(x=>x.Ceremony.Location) %>
        </li>
        <li>
            <strong>Tickets per Student:</strong>
            <%= Html.TextBoxFor(x=>x.Ceremony.TicketsPerStudent) %>
            <%= Html.ValidationMessageFor(x=>x.Ceremony.TicketsPerStudent) %>
        </li>
        <li>
            <strong>Total Tickets:</strong>
            <%= Html.TextBoxFor(x=>x.Ceremony.TotalTickets) %>
            <%= Html.ValidationMessageFor(x=>x.Ceremony.TotalTickets) %>
            
        </li>
        <li>
            <strong>Registration Deadline:</strong>
            <%= Html.TextBoxFor(x => x.Ceremony.RegistrationDeadline.ToString("d"))%>
            <%= Html.ValidationMessageFor(x=>x.Ceremony.RegistrationDeadline) %>
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
    <script src="<%= Url.Content("~/Scripts/ui.multiselect.js") %>" type="text/javascript"></script>
    
    <script type="text/javascript">
        $(function() {
            $("#Ceremony_DateTime").datetimepicker();
            $("#Ceremony_RegistrationDeadline").datepicker();

            $("#AvailableMajors").multiselect();
        });
    </script>

</asp:Content>
