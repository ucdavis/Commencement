<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.CanceledRegistrationsViewModel>" MasterPageFile="~/Views/Shared/Site.Master" %>
<%@ Import Namespace="Commencement.Controllers" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Canceled Registrations
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <ul class="btn">
    <li>
    <%= Html.ActionLink<CeremonyController>(a => a.Edit(Model.Ceremony.Id) , "Back to Ceremony") %>
    </li>
    </ul>
    
    <div class="page_bar">
    <div class="col1"><h2>Canceled Registrations for <%: Model.Ceremony.CeremonyName %></h2></div>    
    </div>

         <% Html.Grid(Model.CancelledRegistrations)
           .Name("CanceledRegs")
           .Columns(col =>
           {
               col.Add(a =>
               { %>
                <a href="<%: Url.Action("StudentDetails", "Admin", new {id = a.Id}) %>"> 
                    <img src="<%: Url.Content("~/Images/to-do-list_checked1_small.gif") %>" />
                </a>
                <% });
                                                                                  
                col.Bound(x => x.StudentId).Title("Student Id");
                col.Bound(x => x.LastName).Title("Last Name");
                col.Bound(x => x.MI).Title("Middle Name");
                col.Bound(x => x.FirstName).Title("First Name");
                col.Bound(x => x.Email);
                col.Bound(x => x.MajorCode);
                col.Bound(x => x.NumberTickets).Title("Tickets");
                col.Bound(x => x.DateRegistered);
                col.Bound(x => x.DateUpdated);
           })
           .Sortable()
           .Render();
        %>
</asp:Content>