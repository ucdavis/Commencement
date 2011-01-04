﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="Commencement.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	AdminLanding
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="btn">
        <li><%= Html.ActionLink<AdminController>(a=>a.Index(), "Back to Home") %></li>
    </ul>

    <h2>Administration</h2>

    <ul class="front_menu">
        <li class="left"><a href="<%: Url.Action("Index", "TermCode") %>"><span>Term Codes</span></a></li>
        <li class="left"><a href="<%: Url.Action("Index", "SpecialNeeds") %>"><span>Special Needs</span></a></li>
    </ul>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>
