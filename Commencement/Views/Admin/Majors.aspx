<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.AdminMajorsViewModel>" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Major Counts
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Major Counts</h2>

    <% foreach (var a in Model.CeremonyCounts) { %>

        <div style="border-bottom: 1px solid lightgray; margin-bottom: 10px; margin-top:10px;">
            <%: a.Ceremony.DateTime.ToString("g") %>
        </div>

        <% Html.Grid(a.MajorCounts.OrderBy(b=>b.Major.MajorName))
               .Name(a.Ceremony.Id.ToString())
               .Columns(col =>
                            {
                                col.Bound(b => b.Major.MajorName);
                                col.Bound(b => b.TotalTickets);
                                col.Bound(b => b.TotalStreaming);
                                col.Bound(b => b.ProjectedTickets);
                                col.Bound(b => b.ProjectedStreamingTickets);
                            })
               .Render();
                %>
    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>
