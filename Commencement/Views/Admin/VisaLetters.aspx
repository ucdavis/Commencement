<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.AdminVisaLetterListViewModel>" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>
<%@ Import Namespace="Commencement.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Visa Letters
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="btn">
        <li><%: Html.ActionLink<AdminController>(a=>a.Index(), "Home") %></li>
    </ul>
    <div class="page_bar">
        <div class="col1"><h2>Visa Letters</h2></div>

    </div>

    
    <%: Html.AntiForgeryToken() %>

    <%: Html.Partial("VisaLetterTablePartial", Model.VisaLetters) %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">


    <style type="text/css">
        .cancel { cursor: pointer;}
    </style>

</asp:Content>
