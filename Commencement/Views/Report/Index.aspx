<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ReportViewModel>" %>
<%@ Import Namespace="Commencement.Controllers" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Index</h2>

    <ul>
        <li><%= Html.ActionLink<ReportController>(a => a.GenerateAveryLabels(TermService.GetCurrent().Id), "Avery Labels")%></li>
    </ul>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
