<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.AdminRegistrationViewModel>" %>
<%@ Import Namespace="Commencement.Controllers"%>
<%@ Import Namespace="Commencement.Controllers.Helpers"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Registrations
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="btn"><li><%= Html.ActionLink<AdminController>(a=>a.Index(), "Home")  %></li></ul>

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
                <%= this.Select("majorCode").Options(Model.MajorCodes, x=>x.Id, x=>x.Name).Selected(Model.majorCodeFilter).FirstOption(string.Empty, "--Select a Major--") %>
            </li>
            <li>
                <strong>Ceremony:</strong>
                <%= this.Select("ceremonyId").Options(Model.Ceremonies,x=>x.Id, x=>x.DateTime)
                        .FirstOption("-1", "--Select a Ceremony--")
                        .Selected(Model.ceremonyFilter)
                        %>
            </li>
            <li>
                <strong>College</strong>
                <%= this.Select("collegeCode").Options(Model.Colleges,x=>x.Id, x=>x.Name).FirstOption(string.Empty, "--Select a College--").Selected(Model.collegeFilter) %>
            </li>
            <li><strong></strong><%= Html.SubmitButton("Submit", "Filter") %></li>
        </ul>
        <% } %>
        </div>
<%--        <h3><a href="#">Totals</a></h3>
        <div>
            <% if (Model.Registrations.Count() > 0) { %>
            <ul>
                <% foreach(var c in Model.Registrations.Select(x=>x.Ceremony).Distinct()) { %>
                    <li>
                        <strong><%= Html.Encode(c.DateTime) %>:</strong>
                        <%= Html.Encode(c.Registrations.Sum(x=>x.TotalTickets)) %>
                    </li>
                <% } %>
            </ul>
            <% } else { %>
                <p>
                    No registrations for the current term have been found.
                </p>
            <% } %>
            
        </div>--%>
    </div>

        <div id="ticketCounts">
            <ul>
                <%foreach(var a in Model.Ceremonies) {%>
                    <li><strong>Ceremony: </strong><%: a.DateTime.ToString("g") %></li>
                    <li><strong># Tickets: </strong><%: a.TicketCount %></li>
                    <%if (a.HasStreamingTickets) { %><li><strong># Streaming Tickets: </strong><%: a.TicketStreamingCount %></li><% } %>
                <% } %>


<%--                <li><strong>Projected Available:</strong> <span id="projectedAvailabeTickets"><%: Model.Ceremony.AvailableTickets %></span></li>
                <% if (Model.Ceremony.HasStreamingTickets) { %><li><strong>Projected Streaming Available:</strong> <span id="projectedAvailableStreaming"><%: Model.Ceremony.AvailableStreamingTickets.HasValue ? Model.Ceremony.AvailableStreamingTickets.Value.ToString() : "n/a" %></span></li><% } %>
                <li><strong>Projected Ticket Count:</strong> <span id="projectedTickets"><%: Model.Ceremony.ProjectedTicketCount %></span></li>
                <% if (Model.Ceremony.HasStreamingTickets) { %><li><strong>Projected Streaming Count:</strong> <span id="projectedStreaming"><%: Model.Ceremony.ProjectedTicketStreamingCount.HasValue ? Model.Ceremony.ProjectedTicketStreamingCount.Value.ToString() : "n/a" %></span></li><% } %>--%>
            </ul>
        </div>

            <% Html.Grid(Model.Participations)
                   .Transactional()
                   .Name("Registrations")
                   .CellAction(cell =>
                                   {
                                       if (cell.Column.Name == "Registration.MailTickets")
                                       {
                                           cell.Text = cell.DataItem.Registration.MailTickets == true ? "Yes" : "No";
                                       }
                                   })
                    .Columns(col =>
                                 {
                                     col.Add(a=> {%>
                                        <%= Html.ActionLink<AdminController>(b=>b.StudentDetails(a.Registration.Student.Id, true), "Select") %>
                                     <% });
                                     col.Bound(a => a.Registration.Student.StudentId);
                                     col.Bound(a => a.Registration.Student.LastName);
                                     col.Bound(a => a.Registration.Student.FirstName);
                                     col.Bound(a => a.Registration.MailTickets);
                                     col.Bound(a => a.NumberTickets);
                                     col.Bound(a => a.Major.MajorName).Title("Major");
                                 })
                    .DataBinding(binding=>binding.Server().Select<AdminController>(a=>a.Registrations(Model.studentidFilter, Model.lastNameFilter, Model.firstNameFilter, Model.majorCodeFilter, Model.ceremonyFilter, Model.collegeFilter)))
                    .Sortable(s=>s.OrderBy(a=>a.Add(b=>b.Registration.Student.LastName)))
                    .Render(); %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

    <script type="text/javascript">
        $(function() {
            $("#filter_container").accordion({ collapsible: true, autoHeight: false, active: false  });
        });
    </script>

        <style = type="text/css">
        .tickets
        {
            width: 50px;
        }
        
        .loading
        {
            display:none;
        }
        
        .cancel
        {
            float:right;
            display:none;
        }
        
        .check
        {
            float:right;
            display:none;
        }
        
        #ticketCounts
        {
            position:relative;
            border: 1px solid black;
            margin-top: 5px;
            margin-bottom: 5px;
        }
        
        #ticketCounts ul
        {
            list-style:none;
        }
        
        #ticketCounts ul li
        {
            display:inline;
            margin-left:20px;
        }
    </style>

</asp:Content>
