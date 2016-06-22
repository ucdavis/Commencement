<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Commencement.Controllers.ViewModels.TermCodeUnion>>" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>
<%@ Import Namespace="Commencement.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Commencement | Term Codes
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ul class="btn">
        <li>
            <%= Html.ActionLink<AdminController>(a=>a.AdminLanding(), "Back to Administration") %>
        </li>
    </ul>
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
                          if (cell.Column.Name == "IsInTermCode")
                          {
                              cell.Text = cell.DataItem.IsInTermCode ? "Yes" : "No";
                          }
                          if (cell.Column.Name == "RegistrationBegin")
                          {
                              cell.Text = cell.DataItem.RegistrationBegin.ToString("d");
                          }
                          if (cell.Column.Name == "RegistrationDeadline")
                          {
                              cell.Text = cell.DataItem.RegistrationDeadline.ToString("d");
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
                            col.Bound(x => x.RegistrationBegin);
                            col.Bound(x => x.RegistrationDeadline);
                            col.Bound(x => x.IsActive).Title("Active");
                            col.Bound(x => x.IsInTermCode).Title("Added");
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

