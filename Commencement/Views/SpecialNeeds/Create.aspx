<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Core.Domain.SpecialNeed>" %>
<%@ Import Namespace="Commencement.Controllers" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Commencement | Create Special Needs
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="btn">
        <li>
            <%= Html.ActionLink<SpecialNeedsController>(a => a.Index() , "Back to List") %>
        </li>
    </ul>

    <h2>Create</h2>

    <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary() %>
        <%=Html.AntiForgeryToken() %>

        <ul class="registration_form">
            
            <li>
                <strong><%:Html.LabelFor(a => a.Name, DisplayOptions.HumanizeAndColon) %></strong>
                <%: Html.TextBoxFor(model => model.Name) %>
                <%: Html.ValidationMessageFor(model => model.Name) %>
            </li>
            
            <li>
                <strong><%:Html.LabelFor(a => a.IsActive, DisplayOptions.HumanizeAndColon)%></strong>
                <%: Html.CheckBoxFor(model => model.IsActive) %>
                <%: Html.ValidationMessageFor(model => model.IsActive) %>
            </li>
         
            <p>
                <input type="submit" value="Create" />
            </p>
        </ul>

    <% } %>


</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>

