<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.MajorCodeViewModel>" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>
<%@ Import Namespace="Commencement.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Commencement | Major Codes Maintenance
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Major Codes Maintenance</h2>

    <%: Html.ActionLink<MajorCodeController>(a=>a.Add(), "Add/Activate Major") %>

    <%
        Html.Grid(Model.MajorCodes.Where(a=>a.IsActive))
            .Name("Majors")
            .Columns(col=>
                         {
                             col.Add(a=>{%>
                             
                                <% if (a.IsActive) { %>
                                    <img src="<%: Url.Content("~/Images/Cancel-1.png") %>" class="activateButton" data-isactive="false" />                                    
                                <% } else { %>
                                    <img src="<%: Url.Content("~/Images/CheckMark-1.png") %>" class="activateButton" data-isactive="true" />
                                <% } %>

                             <%});
                             col.Bound(a => a.Id);
                             col.Bound(a => a.Name);
                             col.Add(a => { %>
                                            <%= this.Select("College").Options(Model.Colleges, x=>x.Id, x=>x.Id).FirstOption("--Select College--").Selected(a.College).Class("college") %>
                                            <% }).Title("College");
                             col.Add(a => { %>
                                            <%= this.Select("Consolidation").Options(Model.MajorCodes, x=>x.Id, x=>x.Name).FirstOption("--Select a Major--").Selected(a).Class("consolidation") %>
                                            <%}).Title("Consolidation Major");
                         }
            )
            .Render();
            
            %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    
    <script type="text/javascript">
        $(function () {
            $(".activateButton").click(function () { });
            $("select.college").change(function () { });
            $("select.consolidation").change(function () { });
        });
    </script>

    <style type="text/css">
        .activateButton
        {
            cursor: pointer;
        }
    </style>

</asp:Content>
