<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Commencement.Controllers.ViewModels.TermCodeUnion>>" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>
<%@ Import Namespace="Commencement.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Index</h2>

    <div>
        <% Html.Grid(Model)
               .Transactional()
               .Name("Term Codes")
               .CellAction(cell =>
                      {
                          if (cell.Column.Name == "IsActive")
                          {
                              cell.Text = cell.DataItem.IsActive ? "Yes" : "No";
                          }
                          if (cell.Column.Name == "IsActive")
                          {
                              cell.Text = cell.DataItem.IsActive ? "Yes" : "No";
                          }
                          if (cell.Column.Name == "IsInTermCode")
                          {
                              cell.Text = cell.DataItem.IsInTermCode ? "Yes" : "No";
                          }
                      })
            .Columns(col=>
                        {
                            col.Add(a =>
                                        { %>
                                        <%if (a.IsInTermCode){%>
                                            <%= Html.ActionLink<TermCodeController>(b => b.Edit(a.TermCodeId), "Edit")%>
                                            | <%= Html.ActionLink<TermCodeController>(b => b.Details(a.TermCodeId), "View")%>
                                            <%if(!a.IsActive) {%>
                                            | <%= Html.ActionLink<TermCodeController>(b => b.Activate(a.TermCodeId), "Activate") %>
                                            <%}%>
                                        <%}
                                          else { %>
                                            <%= Html.ActionLink<TermCodeController>(b => b.Add(a.TermCodeId), "Add") %>
                                          <%}%>
                                        <% });
                            col.Bound(x => x.TermCodeId).Title("Term Code");
                            col.Bound(x => x.Name).Title("Description");
                            col.Bound(x => x.IsActive).Title("Active");
                            col.Bound(x => x.IsInTermCode).Title("Added");
                        })
           .Sortable()
           .Pageable(p=>p.PageSize(20))
           .Render();  %>
    </div>

<%--    <table>
        <tr>
            <th></th>
            <th>
                TermCodeId
            </th>
            <th>
                Name
            </th>
            <th>
                IsActive
            </th>
            <th>
                IsInTermCode
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: Html.ActionLink("Edit", "Edit", new { /* id=item.PrimaryKey */ }) %> |
                <%: Html.ActionLink("Details", "Details", new { /* id=item.PrimaryKey */ })%> |
                <%: Html.ActionLink("Delete", "Delete", new { /* id=item.PrimaryKey */ })%>
            </td>
            <td>
                <%: item.TermCodeId %>
            </td>
            <td>
                <%: item.Name %>
            </td>
            <td>
                <%: item.IsActive %>
            </td>
            <td>
                <%: item.IsInTermCode %>
            </td>
        </tr>
    
    <% } %>

    </table>--%>

    <p>
        <%: Html.ActionLink("Create New", "Create") %>
    </p>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>

