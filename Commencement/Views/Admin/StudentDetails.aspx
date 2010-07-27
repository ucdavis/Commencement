<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.RegistrationModel>" %>
<%@ Import Namespace="Commencement.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	StudentDetails
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="back_btn"><a href="#" onClick="history.go(-1)">Back</a> </div>

    <div id="emulate_btn"><%= Html.ActionLink<AccountController>(a=>a.Emulate(Model.Student.Login), "Emulate") %></div>

    <% Html.RenderPartial("StudentInformationPartial", Model.Student); %>
    
    <h2>Registration</h2>
    <% if (Model.Registration == null) { %>
        Student has not registered.
    <% } else { %>
    
        <ul class="registration_form">
            <li><strong>Address Line 1:</strong>
                <%= Html.Encode(Model.Registration.Address1) %>
            </li>
            <li><strong>Address Line 2:</strong>
                <%= Html.Encode(Model.Registration.Address2) %>
            </li>
            <li><strong>City:</strong>
                <%= Html.Encode(Model.Registration.City) %>
            </li>
            <li><strong>State:</strong>
                <%= Html.Encode(Model.Registration.State.Name) %>
            </li>
            <li><strong>Zip Code:</strong>
                <%= Html.Encode(Model.Registration.Zip) %>
            </li>
            <li class="prefilled"><strong>Email Address:</strong> 
                <span><%= Html.Encode(Model.Student.Email) %></span>
            </li>
            <li><strong>Secondary Email Address:</strong>
                <%= Html.Encode(Model.Registration.Email) %>
            </li>
            <li>
                <strong>Ticket Acquisition:</strong>
                <% if (Model.Registration.MailTickets) { %>
                    <%= Html.Encode("Mail tickets to above address.") %>
                <% } else {%>
                    <%= Html.Encode("Will pick up tickets") %>
                <%} %>
            </li>
        </ul>
        <h2>Ceremony Information</h2>
        <ul class="registration_form">
            <li><strong>Tickets Requested:</strong>
                <%= Html.Encode(Model.Registration.NumberTickets) %>
            </li>
            <li class="prefilled"><strong>Ceremony Date:</strong> 
                <span><%= Html.Encode(string.Format("{0}", Model.Registration.Ceremony.DateTime)) %></span> 
            </li>
            <li>
                <strong>Special Needs:</strong>
                <%= Html.Encode(Model.Registration.Comments) %>
            </li>
        </ul>
    
    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
