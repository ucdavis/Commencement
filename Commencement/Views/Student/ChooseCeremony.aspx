<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.List<Commencement.Controllers.Helpers.CeremonyWithMajor>>"
    MasterPageFile="~/Views/Shared/Site.Master" %>

<%@ Import Namespace="Resources" %>

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
                Commencement for <%= Html.Encode(ceremonyWithMajor.MajorCode.Name) %>.
                <br />
                <%= Html.Encode(string.Format("Will take place on {0} at {1}", ceremonyWithMajor.Ceremony.DateTime, ceremonyWithMajor.Ceremony.Location)) %>
            
            </a>
        </li>
        <% }%>
    </ul>
</asp:Content>
