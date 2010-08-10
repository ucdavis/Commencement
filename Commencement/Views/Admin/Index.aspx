<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="Commencement.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Index</h2>

    <ul class="front_menu">
    <li class="left"><a href="#"><img src="../../Images/ceremony.png" /><br />Ceremony List</a></li>
    <li class="left"><a href="#"><img src="../../Images/students.png" /><br />Students</a></li>
    <li><a href="#"><img src="../../Images/registrations.png" /><br />Registrations</a></li>
    <li class="left"><a href="#"><img src="../../Images/email_template.png" /><br />Email Templates</a></li>
    <!--
        <li class="left"><%= Html.ActionLink<CeremonyController>(a=>a.Index(), "Ceremony List") %></li>
        <li><%= Html.ActionLink<AdminController>(a=>a.Students(null, null, null, null), "Students") %></li>
        <li class="left"><%= Html.ActionLink<AdminController>(a =>a.Registrations(null, null, null, null, null), "Registrations") %></li>
        <li><%= Html.ActionLink<TemplateController>(a=>a.Index(), "Email Templates") %></li>
    -->
        
    </ul>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
