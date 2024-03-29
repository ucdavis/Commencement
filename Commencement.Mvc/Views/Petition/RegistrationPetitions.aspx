﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.AdminPetitionsViewModel>" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>
<%@ Import Namespace="Commencement.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Commencement | Registration Petitions
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="btn">
        <li><%= Html.ActionLink<PetitionController>(a=>a.Index(), "Back to Petitions") %></li>
    </ul>

    <h2>Registration Petitions</h2>

    <%
        Html.Grid(Model.PendingRegistrationPetitions.OrderByDescending(a=>a.DateSubmitted))
            .Name("Petitions")
            .Sortable()
            .CellAction(cell=>
                            {
                                if (cell.Column.Member == "Ceremony.DateTime")
                                {
                                    cell.Text = cell.DataItem.Ceremony.DateTime.ToString("g");
                                }
                            })
            .Columns(col =>
                         {
                             col.Add(a =>{ %>
                                        <%--<%= Html.ActionLink<PetitionController>(b=>b.RegistrationPetition(a.Id), "Select") %>--%>
                                        <a href="<%: Url.Action("RegistrationPetition", "Petition", new {id = a.Id}) %>">
                                            <img src="<%: Url.Content("~/Images/to-do-list_checked1_small.gif") %>" />
                                        </a>
                                         <% });
                             col.Bound(a => a.Registration.Student.LastName);
                             col.Bound(a => a.Registration.Student.FirstName);
                             col.Bound(a => a.Registration.Student.EarnedUnits).Title("Units");
                             col.Bound(a => a.MajorCode.Name).Title("Major");
                             col.Bound(a => a.MajorCode.College.Id).Title("College");
                             col.Bound(a => a.Ceremony.DateTime).Title("Ceremony");
                             col.Bound(a => a.NumberTickets);
                             col.Bound(a => a.DateSubmitted);
                         })
            .Render();
%>


</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>