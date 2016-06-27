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
        <%: Html.ValidationSummary(true,"Please correct all errors below") %>
        <%=Html.AntiForgeryToken() %>

        <ul class="registration_form">
            
            <li>
                <strong><%:Html.LabelFor(a => a.Name, DisplayOptions.HumanizeAndColon) %></strong>
                <%--<%: Html.TextBoxFor(model => model.Name) %>--%>
                <%: Html.TextBox("specialNeed.Name", Model.Name) %>
                <%: Html.ValidationMessage("SpecialNeed.Name") %>
            </li>
            
            <li>
                <strong><%:Html.LabelFor(a => a.IsActive, DisplayOptions.HumanizeAndColon)%></strong>
                <%--<%: Html.CheckBoxFor(model => model.IsActive) %>--%>
                <%: Html.CheckBox("specialNeed.IsActive", Model.IsActive) %>
                <%: Html.ValidationMessage("specialNeed.IsActive") %>
            </li>
         
            <li><strong>&nbsp;</strong>
                <input type="submit" value="Create" class="button" />
                |
                <%: Html.ActionLink("Cancel", "Index") %>
            </li>
        </ul>

    <% } %>


</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

