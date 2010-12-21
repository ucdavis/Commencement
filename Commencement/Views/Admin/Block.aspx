<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Core.Domain.Student>" %>
<%@ Import Namespace="Commencement.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Block
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="btn">
        <li><%= Html.ActionLink<AdminController>(a=>a.StudentDetails(Model.Id, false), "Back to Student") %></li>
    </ul>

    <h2>Block</h2>

    <% if (Model.SjaBlock || Model.Blocked) { %>

        <p>
            Student has already been blocked from the registration system.  Please use the buttom below if you wish to unblock, please note that if they had previously
            registered that registration will become active again.
        </p>

        <p>
            <% using (Html.BeginForm()) { %>
                <%= Html.AntiForgeryToken() %>
                <%: Html.Hidden("block", false) %>
                <%: Html.SubmitButton("UnBlock", "UnBlock") %>
            <% } %>
        </p>

    <% } else { %>
        
        <% using(Html.BeginForm()) { %>

            <%= Html.AntiForgeryToken() %>
            <%: Html.Hidden("block", true) %>

            <p>Please select a reason for blocking this student:</p>
            <ul>
                <li><input type="radio" value="sja" name="reason" />SJA</li>
                <li><input type="radio" value="other" name="reason" />Other</li>
                <li><%: Html.SubmitButton("Block", "Block") %></li>
            </ul>
        <% } %>
    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>
