<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.AdminRegistrationViewModel>" %>
<%@ Import Namespace="Commencement.Controllers"%>
<%@ Import Namespace="Commencement.Controllers.Helpers"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Registrations
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <%= Html.ActionLink<AdminController>(a=>a.Index(), "Home") %>

    <h2>Registrations</h2>

    <div id="filter_container">
        <h3><a href="#">Filters</a></h3>
        <div>
        <% using (Html.BeginForm("Registrations", "Admin", FormMethod.Post)) { %>
            <%= Html.AntiForgeryToken() %>
        <ul>
        
            <li><strong>Student Id:</strong>
                <%= Html.TextBox("studentId", Model.studentidFilter) %>
            </li>
            <li><strong>Last Name:</strong>
                <%= Html.TextBox("lastName", Model.lastNameFilter) %>
            </li>
            <li><strong>First Name:</strong>
                <%= Html.TextBox("firstName", Model.firstNameFilter) %>
            </li>
            <li>
                <strong>Major Code:</strong>
                <%= Html.TextBox("majorCode", Model.majorCodeFilter) %>
            </li>
            <li>
                <strong>Ceremony:</strong>
                <%= this.Select("ceremonyId").Options(Model.Ceremonies,x=>x.Id, x=>x.DateTime)
                        .FirstOption("-1", "--Select a Ceremony--")
                        .Selected(Model.ceremonyFilter)
                        %>
            </li>
            <li><strong></strong><%= Html.SubmitButton("Submit", "Filter") %></li>
        </ul>
        <% } %>
        </div>
        <h3><a href="#">Totals</a></h3>
        <div>
            <% if (Model.Registrations.Count() > 0) { %>
            <ul>
                <% foreach(var c in Model.Registrations.Select(x=>x.Ceremony).Distinct()) { %>
                    <li>
                        <strong><%= Html.Encode(c.DateTime) %>:</strong>
                        <%= Html.Encode(c.Registrations.Sum(x=>x.NumberTickets)) %>
                    </li>
                <% } %>
            </ul>
            <% } else { %>
                <p>
                    No registrations for the current term have been found.
                </p>
            <% } %>
            
        </div>
    </div>

    <% Html.Grid(Model.Registrations)
           .Transactional()
           .Name("Registrations")
           .Columns(col =>
                        {
                            col.Add(a =>
                                        {%>
                                        <%= Html.ActionLink<AdminController>(b=>b.StudentDetails(a.Student.Id), "Select") %>
                                        <%});
                            col.Bound(a => a.Student.StudentId);
                            col.Bound(a => a.Student.LastName);
                            col.Bound(a => a.Student.FirstName);
                            col.Bound(a => a.NumberTickets);
                            col.Bound(a => a.MailTickets);
                            col.Bound(a => a.Ceremony.DateTime).Title("Ceremony");
                            col.Bound(a => a.Major.Id);
                        })
           .Pageable()
           .Sortable()
           .Render();
            %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

    <script type="text/javascript">
        $(function() {
            $("#filter_container").accordion({ collapsible: true });
        });
    </script>

</asp:Content>
