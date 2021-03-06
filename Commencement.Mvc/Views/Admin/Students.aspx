<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.AdminStudentViewModel>" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>
<%@ Import Namespace="Commencement.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Commencement | Students
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="btn">
        <li><%= Html.ActionLink<AdminController>(a=>a.Index(), "Home")  %></li>
    </ul>

    <div class="page_bar">
        <div class="col1"><h2>Students</h2></div>
        <div class="col2"><%= Html.ActionLink<AdminController>(a => a.AddStudent(null), "Add Student", new { @class="button" })%></div>
    </div>
    
    <div id="filter_container">
        <h3><a href="#">Filters</a></h3>
        <% using (Html.BeginForm("Students", "Admin", FormMethod.Post)) { %>
            <%= Html.AntiForgeryToken() %>
        <ul class="registration_form">
        
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
            <% if (Model.Colleges.Count() > 1) { %>
                <li>
                    <strong>College:</strong>
                    <%= this.Select("college").Options(Model.Colleges, x=> x.Id, x=> x.Name).Selected(Model.collegeCodeFilter).FirstOption(string.Empty, "--Select a College--") %>
                </li>
            <% } %>
            <li><strong></strong><%= Html.SubmitButton("Submit", "Filter", new { @class="button" })%></li>
        </ul>
        <% } %>
    </div>

    <% Html.Grid(Model.StudentRegistrationModels)
           .Transactional()
           .Name("Students")
           .CellAction(cell =>
                      {
                          if (cell.Column.Member == "Registration")
                          {
                              cell.Text = cell.DataItem.Registration ? "Yes" : "No";
                          }
                      })
           .Columns(col=>
                        {
                            col.Add(a =>
                                        { %>
                                        <a href="<%: Url.Action("StudentDetails", "Admin", new {id=a.Student.Id}) %>"> 
                                            <img src="<%: Url.Content("~/Images/to-do-list_checked1_small.gif") %>" />
                                        </a>
                                        <% });
                            col.Bound(x => x.Student.StudentId).Title("Student Id");
                            col.Bound(x => x.Student.LastName).Title("Last Name");
                            col.Bound(x => x.Student.FirstName).Title("First Name");
                            col.Bound(x => x.Student.TotalUnits);
                            col.Bound(x => x.Student.Email);
                            col.Bound(x => x.Student.StrMajorCodes ).Title("Majors");
                            col.Bound(x => x.Registration).Title("Registered").Width(40);
                        })
           .DataBinding(binding=>binding.Server().Select<AdminController>(a=>a.Students(Model.studentidFilter, Model.lastNameFilter, Model.firstNameFilter, Model.majorCodeFilter, Model.collegeCodeFilter)))
           .Sortable(s=>s.OrderBy(a=>a.Add(b=>b.Student.LastName)))
           .Pageable(p=>p.PageSize(100))
           .Render(); 
           %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

    <script type="text/javascript">
        $(function() {
            $("#filter_container").accordion({collapsible:true, autoHeight: false, active: false });
        });
    </script>
    <style type="text/css">
        tr:hover, tr.t-alt:hover {background-color:#BAC6CF;}
    </style>
</asp:Content>
