<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.AdminPetitionsViewModel>" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>
<%@ Import Namespace="Commencement.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	RegistrationPetitions
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="btn">
        <li><%= Html.ActionLink<PetitionController>(a=>a.Index(), "Back to Petitions") %></li>
    </ul>

    <h2>Registration Petitions</h2>

    <%
        Html.Grid(Model.PendingRegistrationPetitions.OrderByDescending(a=>a.DateSubmitted))
            .Name("Petitions")
            .CellAction(cell=>
                            {
                                if (cell.Column.Name == "Ceremony.DateTime")
                                {
                                    cell.Text = cell.DataItem.Ceremony.DateTime.ToString("g");
                                }
                            })
            .Columns(col =>
                         {
                             col.Add(a =>{ %>
                                        <%= Html.ActionLink<PetitionController>(b=>b.RegistrationPetition(a.Id), "Select") %>
                                         <% });
                             col.Bound(a => a.Registration.Student.LastName);
                             col.Bound(a => a.Registration.Student.FirstName);
                             col.Bound(a => a.MajorCode.Name);
                             col.Bound(a => a.Ceremony.DateTime).Title("Ceremony");
                             col.Bound(a => a.NumberTickets);
                             col.Bound(a => a.DateSubmitted);
                         })
            .Render();
%>


</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>
