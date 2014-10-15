<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.AdminVisaLetterListViewModel>" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>
<%@ Import Namespace="Commencement.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Visa Letters
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="btn">
        <li><%: Html.ActionLink<HomeController>(a=>a.Index(), "TODO") %></li>
    </ul>
    <div class="page_bar">
        <div class="col1"><h2>Visa Letters</h2></div>

    </div>

    
    <%: Html.AntiForgeryToken() %>

    <% Html.Grid(Model.VisaLetters)
           .Name("VisaLetters")
           .Columns(col =>
                        {
                            col.Bound(a => a.Student.StudentId); //Remove once testing is done
                            col.Bound(a => a.Student.FullName).Title("Name");
                            col.Bound(a => a.ApprovedBy);
                            col.Bound(a => a.DateDecided);
                            col.Bound(a => a.DateCreated);
                            col.Bound(a => a.IsApproved);
                            col.Bound(a => a.IsPending);
                        })
           .Render();
        %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">


    <style type="text/css">
        .cancel { cursor: pointer;}
    </style>

</asp:Content>
