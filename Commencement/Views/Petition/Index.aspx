<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.AdminPetitionsViewModel>" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>
<%@ Import Namespace="Commencement.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <%= Html.ActionLink<HomeController>(a=>a.Index(), "Back Home") %>

    <div id="Ceremony_Container">
    </div>

    <%
        Html.Grid(Model.PendingExtraTicket)
            .Name("PendingExtraTicketPetitions")
            .Columns(col=>
                         {
                             col.Add(a =>
                                         { %>
                                         
                                            <%= Html.ActionLink<PetitionController>(b=>b.DecideExtraTicketPetition(a.Id, true), "Approve") %>
                                            |
                                            <%= Html.ActionLink<PetitionController>(b=>b.DecideExtraTicketPetition(a.Id, false), "Deny") %>
                                         
                                         <% });
                             col.Bound(a => a.Student.StudentId);
                             col.Bound(a => a.Student.FullName).Title("Name");
                             col.Bound(a => a.ExtraTicketPetition.NumberTickets).Title("# Extra Tickets");
                             col.Bound(a => a.ExtraTicketPetition.DateSubmitted);
                         })
            .Render();
    %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
