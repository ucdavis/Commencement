<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IQueryable<Commencement.Core.Domain.TransferRequest>>" %>
<%@ Import Namespace="Commencement.MVC.Controllers.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Transfer Requests
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Transfer Requests</h2>
    
    <% Html.Grid(Model.OrderBy(a => a.RegistrationParticipation.Registration.Student.LastName))
           .Transactional()
           .Name("TransferRequests")
           .Columns(col =>
               {
                   col.Add(a => { %>
                       <% if(a.Pending) {%>
                        <a href="<%: Url.Action("Review", new {id=a.Id}) %>">
                            <img src="<%: Url.Content("~/Images/to-do-list_checked1_small.gif") %>"/>
                        </a>
                        <% } %>
                        <% });
                   col.Bound(x => x.RegistrationParticipation.Registration.Student.StudentId).Title("Student Id");
                   col.Bound(x => x.RegistrationParticipation.Registration.Student.FirstName).Title("First Name");
                   col.Bound(x => x.RegistrationParticipation.Registration.Student.LastName).Title("Last Name");
                   col.Bound(x => x.DateRequested);
                   col.Bound(x => x.Ceremony.CeremonyName);
                   col.Bound(x => x.Ceremony.DateTime);
                   col.Bound(x => x.Pending);
               })
            .Render();
            %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>
