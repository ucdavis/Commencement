<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.MVC.Controllers.ViewModels.CommencementViewModel>" %>
<%@ Import Namespace="Commencement.MVC.Controllers.Helpers" %>
<%@ Import Namespace="Commencement.MVC.Controllers" %>
<%@ Import Namespace="Telerik.Web.Mvc.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Commencement | Ceremonies
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<ul class="btn">
    <li>
    <%= Html.ActionLink<AdminController>(a=>a.Index(), "Home") %>
    </li></ul>

    <div class="page_bar">
    <div class="col1"><h2>Ceremonies</h2></div>
    <div class="col2"><%= Html.ActionLink<CeremonyController>(a => a.Create(), "Create New", new { @class="button" })%></div>
    </div>

    <% Html.Grid(Model.Ceremonies)
           .Name("Ceremonies")
           .Columns(col =>
                        {
                            col.Template(a =>
                                        {   %>
                                            <%= Html.ActionLink<CeremonyController>(b=>b.Edit(a.Id), "Select", new{@class="button"}) %>                                            
                                            <%
                                        });
                            col.Bound(a => a.CeremonyName).Title("Name");
                            col.Bound(a => a.TermCode.Id).Title("Term");
                            col.Bound(a => a.DateTime).Format("{0:MM/dd/yyyy hh:mm tt}");
                            col.Bound(a => a.Location);
                            col.Bound(a => a.TotalTickets);
                        } )
           .Sortable(a=> a.SortMode(GridSortMode.MultipleColumn).OrderBy(b=>b.Add(c=>c.DateTime).Descending()))
           .Pageable()
           .Render();
        
           %>    

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
