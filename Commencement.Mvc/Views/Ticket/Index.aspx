<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.MVC.Controllers.TicketViewModel>" %>
<%@ Import Namespace="Commencement.MVC.Controllers" %>
<%@ Import Namespace="Commencement.MVC.Controllers.Helpers" %>
<%@ Import Namespace="Telerik.Web.Mvc.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Ticketing Export
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="page_bar">
    <div class="col1"><h2>Ticketing Export</h2></div>
    <div class="col2"><%= Html.ActionLink<TicketController>(a => a.GetReport(null), "Export All", new { @class="button" })%></div>
    </div>

    <% Html.Grid(Model.Ceremonies)
        .Name("Ceremonies")
        .Columns(col =>
                    {
                        col.Template(a =>
                                    {   %>
                                        <%= Html.ActionLink<TicketController>(b=>b.GetReport(a.Id), "Select", new{@class="button"}) %>                                            
                                        <%
                                    });
                        col.Bound(a => a.CeremonyName).Title("Name");
                        col.Bound(a => a.TermCode.Id).Title("Term");
                        col.Bound(a => a.DateTime).Format("{0:MM/dd/yyyy hh:mm tt}");
                        col.Bound(a => a.Location);
                        col.Bound(a => a.TicketCount).Title("Registered");
                        col.Bound(a => a.TotalTickets).Title("Total Tickets");
                    } )
        .Sortable(a=> a.SortMode(GridSortMode.MultipleColumn).OrderBy(b=>b.Add(c=>c.DateTime).Descending()))
        .Pageable()
        .Render();
        
        %>    

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>
