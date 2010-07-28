<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.CommencementViewModel>" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>
<%@ Import Namespace="Commencement.Controllers" %>
<%@ Import Namespace="Telerik.Web.Mvc.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <%= Html.ActionLink<AdminController>(a=>a.Index(), "Home") %>

    <h2>Index</h2>

    <%= Html.ActionLink<CeremonyController>(a=>a.Create(), "Create New") %>

    <% Html.Grid(Model.Ceremonies)
           .Name("Ceremonies")
           .Columns(col =>
                        {
                            col.Template(a =>
                                        {   %>
                                            <%= Html.ActionLink<CeremonyController>(b=>b.Edit(a.Id), "Select") %>                                            
                                            <%
                                        });
                            col.Bound(a => a.TermCode.Id).Title("Term Code");
                            col.Bound(a => a.DateTime);
                            col.Bound(a => a.Location);
                            col.Bound(a => a.TotalTickets);
                            col.Bound(a => a.RegistrationDeadline).Format("{0:MM/dd/yyyy}");
                        } )
           .Sortable(a=> a.SortMode(GridSortMode.MultipleColumn).OrderBy(b=>b.Add(c=>c.DateTime).Descending()))
           .Pageable()
           .Render();
        
           %>    

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
