<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Mvc.Controllers.ViewModels.MajorCodeViewModel>" %>
<%@ Import Namespace="Commencement.Mvc.Controllers" %>
<%@ Import Namespace="Commencement.Mvc.Controllers.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Commencement | Major Codes Maintenance
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="btn">
        <li><%= Html.ActionLink<AdminController>(a=>a.AdminLanding(), "Back to Administration") %></li>
    </ul>

    <div class="page_bar">
        <div class="col1"><h2>Major Codes Maintenance</h2></div>
        <div class="col2"><%: Html.ActionLink<MajorCodeController>(a => a.Add(), "Add/Activate Major", new { @class="button" })%></div>
    </div>
    

    <%: Html.AntiForgeryToken() %>

    <%
        Html.Grid<MajorCode>(Model.MajorCodes.Where(a=>a.IsActive))
            .Name("Majors")
            .Columns(col=>
                         {
                             col.Add(a=>{%>
                                <img src="<%: Url.Content("~/Images/Cancel-1.png") %>" class="activateButton" data-id="<%: a.Id %>" />                                    
                             <%});
                             col.Bound(a => a.Id);
                             col.Bound(a => a.Name);
                             col.Add(a => { %>
                                            <%--<%= this.Select("College").Options(Model.Colleges, x=>x.Id, x=>x.Id).FirstOption("--Select College--").Selected(a.College).Class("college") %>--%>

                                            <select class="college" data-id="<%: a.Id %>" style="min-width: 70px;">
                                                <% foreach (var b in Model.Colleges.Where(b=>b.Display)) { %>
                                                    <option value="<%: b.Id %>" <%: a.College == b ? "selected" : string.Empty %> ><%: b.Id %></option>
                                                <% } %>
                                            </select>
                                            <img src="<%: Url.Content("~/Images/loading.gif") %>" class="loading indicator" />
                                            <img src="<%: Url.Content("~/Images/Cancel-1.png") %>" class="cancel indicator" />
                                            <img src="<%: Url.Content("~/Images/CheckMark-1.png") %>" class="check indicator" />
                                            <% }).Title("College").Width(100);
                             col.Add(a => { %>
                                            <%--<%= this.Select("Consolidation").Options(Model.MajorCodes, x=>x.Id, x=>x.Name).FirstOption("--Select a Major--").Selected(a).Class("consolidation") %>--%>

                                            <select class="consolidation" data-id="<%: a.Id %>">
                                                <option value="">--Select a Major--</option>
                                                <% foreach (var b in Model.MajorCodes.Where(b=>b.IsActive)) { %>
                                                    <option value="<%: b.Id %>" <%: a.ConsolidationMajor == b ? "selected" : string.Empty %>><%: b.Name %></option>
                                                <% } %>
                                            </select>
                                            <img src="<%: Url.Content("~/Images/loading.gif") %>" class="loading indicator" />
                                            <img src="<%: Url.Content("~/Images/Cancel-1.png") %>" class="cancel indicator" />
                                            <img src="<%: Url.Content("~/Images/CheckMark-1.png") %>" class="check indicator" />
                                            <%}).Title("Consolidation Major");
                         }
            )
            .Render();
            %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    
    <script type="text/javascript">
        $(function () {
            $(".activateButton").click(function () {
                if (confirm("Are you sure you would like to deactivate this major?")) {
                    var majorCode = $(this).data("id");
                    var url = '<%: Url.Action("Deactivate", "MajorCode") %>';

                    var $that = $(this);
                    $that.siblings(".loading").show();

                    $.post(url, { majorCode: majorCode, __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val() }, function (result) {

                        $that.siblings(".loading").fadeOut(2000);

                        if (result != "") {
                            alert(result);
                        }
                        else {
                            $that.parents("tr").fadeOut(1500, function () { $(this).remove(); });
                        }
                    });
                }
            });
            $("select.college").change(function () {
                var $that = $(this);
                var college = $that.val();
                var major = $that.data("id");
                var url = '<%: Url.Action("SetCollege", "MajorCode") %>';

                $that.siblings(".loading").show();

                $.post(url, { majorCode: major, collegeCode: college, __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val() }, function (result) {

                    $that.siblings(".loading").hide();
                    if (result != "") {
                        alert(result);
                        $that.siblings(".cancel").show();
                    }
                    else {
                        $that.siblings(".check").show().fadeOut(2000);
                    }
                });
            });
            $("select.consolidation").change(function () {
                var $that = $(this);
                var consolidation = $that.val();
                var major = $that.data("id");
                var url = '<%: Url.Action("SetConsolidation", "MajorCode") %>';

                $that.siblings(".loading").show();

                $.post(url, { majorCode: major, consolidationCode: consolidation, __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val() }, function (result) {
                    $that.siblings(".loading").hide();

                    if (result != "") {
                        alert(result);
                        $that.siblings(".cancel").show();
                    }
                    else {
                        $that.siblings(".check").show().fadeOut(2000);
                    }
                });
            });
        });
    </script>

    <style type="text/css">
        .activateButton
        {
            cursor: pointer;
        }
        
        .indicator
        {
            display: none;
        }
    </style>

</asp:Content>
