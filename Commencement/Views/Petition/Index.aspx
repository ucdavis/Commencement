<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.AdminPetitionsViewModel>" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>
<%@ Import Namespace="Commencement.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <%= Html.ActionLink<HomeController>(a=>a.Index(), "Back Home") %>

    <div id="Ceremony_Container">
    
        <ul>
            <% foreach(var ceremony in Model.Ceremonies) { %>
                <li>
                    <strong><%= ceremony.Name %> (<%= ceremony.DateTime %>) :</strong>
                    <%= ceremony.AvailableTickets %>
                </li>
            <% } %>
        </ul>
    
    </div>

    <div id="tabs">

        <ul>
            <li><a href="#tabs-1">Extra Ticket Petition</a></li>
            <li><a href="#tabs-2">Registration Petition</a></li>
        </ul>

        <div id="tabs-1">
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
                                     col.Bound(a => a.Ceremony.DateTime).Title("Ceremony");
                                 })
                    .Render();
            %>
        </div>
        
        <div id="tabs-2">
            2nd tab
        </div>
    </div>
    
    

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

    <script type="text/javascript">
        $(function() {
            $("#tabs").tabs();
        });
    </script>

</asp:Content>
