<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IQueryable<Commencement.Core.Domain.EmailQueue>>" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>
<%@ Import Namespace="Commencement.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="btn">
        <li><%: Html.ActionLink<AdminController>(a=>a.Index(), "Home") %></li>
    </ul>

    <h2>Email Queue</h2>
    <%: Html.AntiForgeryToken() %>

    <% if (Convert.ToBoolean(Request.QueryString["showAll"])) { %>
        <%: Html.ActionLink<EmailQueueController>(a=>a.Index(false), "Show Pending") %>
    <% } else { %>
        <%: Html.ActionLink<EmailQueueController>(a=>a.Index(true), "Show All") %>
    <% } %>

    <% Html.Grid(Model)
           .Name("EmailQueue")
           .Columns(col =>
                        {
                            col.Add(a => { %>
                                        <img class="cancel" src="<%: Url.Content("~/Images/Cancel-1.png") %>" data-id='<%: a.Id %>' />
                                        <% });
                            col.Bound("Student.StudentId");
                            col.Bound("Student.FullName").Title("Name");
                            col.Bound("Subject");
                            col.Bound("Created");
                            col.Bound("Id");
                        })
           .Render();
        %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

    <script type="text/javascript">
        $(function () {
            $(".cancel").click(function () {
                var url = '<%: Url.Action("Cancel", "EmailQueue") %>';
                var that = this;
                $.post(url, { id: $(this).data("id"), __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val() },
                        function (result) {
                            if (result) {
                                $(that).parents("tr").fadeOut(1500, function () { $(that).parents("tr").remove(); });
                            }
                            else {
                                $(that).parents("tr").css("background", "red");
                            }
                        });
            });
        });
    </script>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>
