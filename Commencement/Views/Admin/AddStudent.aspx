<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.SearchStudentViewModel>" %>
<%@ Import Namespace="Commencement.Controllers"%>
<%@ Import Namespace="Commencement.Controllers.Helpers"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Add Student
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="btn">
    <li>
    <%= Html.ActionLink<AdminController>(a=>a.Students(null, null, null, null), "Back to List") %>
    </li></ul>

    <h2>Add Student</h2>
    
    <div id="search_container">
        <% using (Html.BeginForm("AddStudent", "Admin", FormMethod.Get)) { %>
        <ul>
            <li><strong>Student Id:</strong>
                <%= Html.TextBox("studentId", Model.StudentId) %>
            </li>
            <li><strong></strong>
                <%= Html.SubmitButton("Submit", "Search") %>
            </li>
        </ul>
        <% } %>
    </div>
    
    <% Html.Grid(Model.SearchStudents)
           .Transactional()
           .Name("SearchStudents")
           .Columns(col=>
                        {
                            col.Add(a =>
                                        {%>
                                           <%= Html.ActionLink<AdminController>(b=>b.AddStudentConfirm(a.Id, a.MajorCode), "Select") %> 
                                        <%});
                            col.Bound(a => a.FirstName);
                            col.Bound(a => a.LastName);
                            col.Bound(a => a.HoursEarned).Title("Units");
                            col.Bound(a => a.Email);
                            col.Bound(a => a.MajorCode).Title("Major");
                            col.Bound(a => a.CollegeCode).Title("College");
                            col.Bound(a => a.DegreeCode);
                            col.Bound(a => a.LoginId);
                        })
           .Render();
            %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
