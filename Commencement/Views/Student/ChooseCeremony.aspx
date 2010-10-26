<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.List<Commencement.Controllers.Services.CeremonyWithMajor>>"
    MasterPageFile="~/Views/Shared/Site.Master" %>

<%--<%@ Import Namespace="Resources" %>--%>

<%@ Import Namespace="Commencement.Core.Resources" %>

<asp:Content runat="server" ID="Content" ContentPlaceHolderID="TitleContent">
    Choose A Ceremony To Attend</asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="HeaderContent">
</asp:Content>
<asp:Content runat="server" ID="Content2" ContentPlaceHolderID="MainContent">
    <h2>
        <%= Html.Encode(StaticValues.Txt_MultipleCeremonies) %>
    </h2>
        <br />
    <ul class="choose_ceremony_menu">
        <% foreach (var ceremonyWithMajor in Model)
           { %>
        <li>
            <a href="<%= Url.Action("Register", new { id = ceremonyWithMajor.Ceremony.Id, major = ceremonyWithMajor.MajorCode.Id }) %>">
                <p>Commencement for</p><h4><%= Html.Encode(ceremonyWithMajor.MajorCode.Name) %></h4>
                <h5>College of <%: ceremonyWithMajor.MajorCode.College.Name %></h5>
                <p><%= Html.Encode(string.Format("Will take place on {0} at {1}", ceremonyWithMajor.Ceremony.DateTime, ceremonyWithMajor.Ceremony.Location)) %></p>
            
            </a>
        </li>
        <% }%>
    </ul>
</asp:Content>
