<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Commencement.Core.Domain.EmailQueue>>" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>
<%@ Import Namespace="Commencement.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Commencement | Email Queue
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="btn">
        <li><%: Html.ActionLink<AdminController>(a=>a.Index(), "Home") %></li>
    </ul>

    <div class="page_bar">
        <h1>(Test All Immediate) Total Pending Emails:<%=ViewBag.TotalPending%></h1>
        <div class="col1"><h2>Email Queue</h2></div>
        <div class="col2">
            <% if (Convert.ToBoolean(Request.QueryString["showAll"])) { %>
                <%: Html.ActionLink<EmailQueueController>(a => a.Index(false, false, false), "Show Pending", new { @class = "button" })%>
            <% } else { %>
                <%: Html.ActionLink<EmailQueueController>(a => a.Index(true, false, false), "Show All", new { @class = "button" })%>
                <%: Html.ActionLink<EmailQueueController>(a => a.Index(true, true, false), "Show All Current Term", new { @class = "button" })%>
            <%: Html.ActionLink<EmailQueueController>(a => a.Index(true, true, true), "Everything Else", new { @class = "button" })%>
            <% } %>
            <%: Html.ActionLink<EmailQueueController>(a => a.EmailStudents(), "Mass Email", new { @class="button" })%>
        </div>
    </div>

    
    <%: Html.AntiForgeryToken() %>

    <% Html.Grid(Model)
           .Name("EmailQueue")
           .Columns(col =>
                        {
                            col.Add(a => { %>
                                        <%: Html.ActionLink("Details", "Details", new {id=a.Id}, new {@class="button"}) %>  <%if(!a.Pending) {%><img class="resend" src="<%: Url.Content("~/Images/resend.png") %>" data-id='<%: a.Id %>' title="Resend Email"  style="padding-top: 0px; margin-top: 1px; border-top-width: 0px; margin-bottom: -11px;"/><%} %>
                                        <% });
                            col.Bound("Student.StudentId");
                            col.Bound("Student.FullName").Title("Name");
                            col.Bound("Subject");
                            col.Bound("Created");
                            col.Bound("Id");
                            col.Bound(a => a.Attachment.Id).Title("Attachment Id");
                            col.Add(a => { %>
                                <img class="cancel" src="<%: Url.Content("~/Images/Cancel-1.png") %>" data-id='<%: a.Id %>' />
                            <% });
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

            $(".resend").click(function () {
                var url = '<%: Url.Action("ReSendEmail", "EmailQueue") %>';
                var that = this;
                $.post(url, { id: $(this).data("id"), __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val() },
                    function (result) {
                        if (result) {
                            if (result.Success) {
                                $(that).parents("tr").css("background", "green");
                            } else {
                                alert("Failed: " + result.Message);
                                $(that).parents("tr").css("background", "red");
                            }

                        } else {
                            alert("Error. Email Queue not updated");
                            $(that).parents("tr").css("background", "red");
                        }
                    });
            });
        });
    </script>
    
    <style type="text/css">
        .cancel { cursor: pointer;}
    </style>

</asp:Content>
