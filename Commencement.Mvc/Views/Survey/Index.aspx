<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Commencement.Core.Domain.Survey>>" %>
<%@ Import Namespace="Commencement.MVC.Controllers" %>
<%@ Import Namespace="Commencement.MVC.Controllers.Helpers" %>
<%@ Import Namespace="Telerik.Web.Mvc.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Surveys
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="page_bar">
    <div class="col1"><h2>Surveys</h2></div>
    <div class="col2"><%= Html.ActionLink<SurveyController>(a => a.Create(), "Create New", new { @class="button" })%></div>
    </div>

    <% Html.Grid(Model)
        .Name("Surveys")
        .Columns(col =>
                    {
                        col.Template(a =>
                                    {   %>
                                        <%= Html.ActionLink("Edit", "Edit", new {id=a.Id}, new{@class="button"}) %>                                            
                                        <%= Html.ActionLink("Preview", "Preview", new {id=a.Id}, new {@class="button"}) %>
                                        <%= Html.ActionLink("Responses", "Results", new {id=a.Id}, new {@class="button"}) %>
                                        <%
                                    });
                        col.Bound(a => a.Name);
                        col.Bound(a => a.Created);
                    } )
        .Pageable()
        .Render();
        
        %>    

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>
