<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="Commencement.MVC.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Commencement | Petitions
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="btn">
        <li><%= Html.ActionLink<HomeController>(a=>a.Index(), "Back Home") %></li>
    </ul>

    <ul class="front_menu">
        <li class="left"><a href="<%: Url.Action("ExtraTicketPetitions", "Petition") %>"><span>Extra Ticket Petitions</span></a></li>
        <li class="left"><a href="<%: Url.Action("RegistrationPetitions", "Petition") %>"><span>Registration Petitions</span></a></li>
    </ul>
   

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">


</asp:Content>
