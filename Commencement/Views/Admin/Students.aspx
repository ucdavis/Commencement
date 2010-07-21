<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.AdminStudentViewModel>" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>
<%@ Import Namespace="Commencement.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Students
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <%= Html.ActionLink<AdminController>(a=>a.Index(), "Home") %>

    <h2>Students</h2>

    <div id="filter_container">
        
        <% using (Html.BeginForm("Students", "Admin", FormMethod.Post)) { %>
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
            <li><strong></strong><%= Html.SubmitButton("Submit", "Filter") %></li>
        </ul>
        <% } %>
    </div>

    <% Html.Grid(Model.Students)
           .Transactional()
           .Name("Students")
           .Columns(col=>
                        {
                            col.Bound(x => x.StudentId).Title("Student Id");
                            col.Bound(x => x.LastName).Title("Last Name");
                            col.Bound(x => x.FirstName).Title("First Name");
                            col.Bound(x => x.Units);
                            col.Bound(x => x.Email);
                        })
           .Sortable(s=>s.OrderBy(a=>a.Add(b=>b.LastName))).Pageable(p=>p.PageSize(100))
           .Render(); 
           %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
