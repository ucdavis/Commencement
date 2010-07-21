<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.List<Commencement.Controllers.Helpers.CeremonyWithMajor>>" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content runat="server" ID="Content" ContentPlaceHolderID="TitleContent">Choose A Ceremony To Attend</asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="HeaderContent"></asp:Content>
<asp:Content runat="server" ID="Content2" ContentPlaceHolderID="MainContent">

<h2>You have qualifed for multiple ceremonies.  Please choose the one you would like to attend</h2>

<ul>
    <li>
        <input type="radio" name="ceremonies" value="1" />Commencement ceremony #1 information
    </li>
    <li>
        <input type="radio" name="ceremonies" value="2" />Commencement ceremony #2 information
    </li>

</ul>

</asp:Content>
