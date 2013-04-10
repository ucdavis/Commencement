<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<List<Commencement.Core.Domain.RegistrationParticipation>>" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Retroactive Search
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Retroactive Search</h2>
    
    <% using (Html.BeginForm()) { %>
        <%: Html.AntiForgeryToken() %>
         <ul class="registration_form">
             <li><strong>Student Id</strong>
                 <%: Html.TextBox("id", ViewData["Id"]) %>
             </li>
             <li><strong>First Name</strong>
                <%: Html.TextBox("FirstName", ViewData["FirstName"]) %>
             </li>
             <li><strong>Last Name</strong>
                <%: Html.TextBox("LastName", ViewData["LastName"]) %>
             </li>
             <li><strong>&nbsp;</strong>
                <input type="submit" value="Search" class="button"/>
             </li>
         </ul>

    <% } %>
    
    <% if (Model != null && Model.Any()) { %>
        <div class="grid">
        <% Html.Grid(Model)
           .Transactional()
           .Name("Students")
           .Columns(col=>
                        {
                            col.Add(a =>
                                        { %>    
                                            <%: Html.ActionLink("Select", "Details", new {a.Id}) %>
                                        <% });
                            col.Bound(x => x.Registration.Student.StudentId).Title("Student Id");
                            col.Bound(x => x.Registration.Student.FirstName).Title("First Name");
                            col.Bound(x => x.Registration.Student.LastName).Title("Last Name");
                            col.Bound(x => x.Registration.Student.Email);
                            col.Bound(x => x.Major.MajorId).Title("Major");
                            col.Bound(x => x.Ceremony.CeremonyName);
                            col.Bound(x => x.DateRegistered);
                            col.Add(x => {%> <%: x.Cancelled ? "Cancelled" : "Active" %> <% } ).Title("Status");
                        })
           .Pageable(p=>p.PageSize(100))
           .Render(); 
           %>
        </div>
    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    
    <style type="text/css">
        .grid {
            margin-top: 2em;
        }
    </style>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>
