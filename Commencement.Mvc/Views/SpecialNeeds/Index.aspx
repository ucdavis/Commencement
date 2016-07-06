<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Commencement.Core.Domain.SpecialNeed>>" %>
<%@ Import Namespace="Commencement.Mvc.Controllers" %>
<%@ Import Namespace="Commencement.Mvc.Controllers.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Commencement | Special Needs Maintenance
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ul class="btn">
        <li>
            <%= Html.ActionLink<AdminController>(a=>a.AdminLanding(), "Back to Administration") %>
        </li>
    </ul>

    <div class="page_bar">
        <div class="col1"><h2>Special Needs Maintenance</h2></div>
        <div class="col2"><%= Html.ActionLink<SpecialNeedsController>(a => a.Create(), "Create New", new { @class="button" })%></div>
    </div>

        <div>
        <% Html.Grid(Model)
               .Transactional()
               .Name("SpecialNeeds")
                .CellAction(cell =>
                {
                    if (cell.Column.Member == "IsActive")
                    {
                        cell.Text = cell.DataItem.IsActive ? "Yes" : "No";
                    }
                })
            .Columns(col=>
                        {
                            col.Add(a =>
                                        { %>
                                        <%if (a.IsActive){%>
                                            <%= Html.ActionLink<SpecialNeedsController>(b => b.ToggleActive(a.Id), "Deactivate")%>

                                        <%}
                                          else { %>
                                            <%= Html.ActionLink<SpecialNeedsController>(b => b.ToggleActive(a.Id), "Activate")%>
                                          <%}%>
                                        <% });
                            col.Bound(x => x.Name).Title("Description");
                            col.Bound(x => x.IsActive).Title("Active");
                        })
           .Sortable()
           .Pageable(p=>p.PageSize(20))
           .Render();  %>
    </div>



</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>

