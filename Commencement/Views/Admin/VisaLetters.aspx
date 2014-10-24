<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.AdminVisaLetterListViewModel>" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>
<%@ Import Namespace="Commencement.Controllers" %>
<%@ Import Namespace="Commencement.Resources" %>

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
    
        <div id="filter_container">
        <h3><a href="#">Filters</a></h3>
        <% using (Html.BeginForm("VisaLetters", "Admin", FormMethod.Post)) { %>
            <%= Html.AntiForgeryToken() %>
        <ul class="registration_form">
        
            <li>
                <%= Html.CheckBoxFor(a => a.ShowAll) %> <strong>Show All</strong>
            </li>
            <li><strong>Start Date:</strong>
                <%= Html.TextBoxFor(a => a.StartDate, new{@class="date"}) %>
            </li>
            <li><strong>End Date:</strong>
                <%= Html.TextBoxFor(a => a.EndDate, new{@class="date"}) %>
            </li>
            <li><strong>CollegeCode</strong>
                <%: Html.DropDownListFor(a=>a.CollegeCode, SelectLists.CollegeNames) %>
            </li>
            <li><strong></strong><%= Html.SubmitButton("Submit", "Filter", new { @class="button" })%></li>
        </ul>
        <% } %>
    </div>
    
    <%: Html.AntiForgeryToken() %>

    <%: Html.Partial("VisaLetterTablePartial", Model.VisaLetters) %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
        <script type="text/javascript">
        $(function() {
            $("#filter_container").accordion({ collapsible: true, autoHeight: false, active: false });
        });
    </script>

    <style type="text/css">
        .cancel { cursor: pointer;}
    </style>

</asp:Content>
