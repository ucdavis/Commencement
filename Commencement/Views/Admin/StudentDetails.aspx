<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.RegistrationModel>" %>
<%@ Import Namespace="Commencement.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	StudentDetails
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="btn">
    <li>
        <% if (Request.QueryString["Registration"]!= null && Convert.ToBoolean(Request.QueryString["Registration"])) { %>
            <%= Html.ActionLink<AdminController>(a=>a.Registrations(null, null, null, null, null), "Back to List") %>
        <% } else { %>
            <%= Html.ActionLink<AdminController>(a=>a.Students(null, null, null, null), "Back to List") %>
        <% } %>
    </li>
    <li><%= Html.ActionLink<AccountController>(a=>a.Emulate(Model.Student.Login), "Emulate") %></li>
    
    <% if (Model.Registration != null) { %>
        <li><div id="changeMajr_btn"><%= Html.ActionLink<AdminController>(a=>a.ChangeMajor(Model.Registration.Id), "Change Major") %></div></li>
        <li><div id="changeCeremony_btn"><%= Html.ActionLink<AdminController>(a=>a.ChangeCeremony(Model.Registration.Id), "Change Ceremony") %></div></li>
    <% } %>
    </ul>

    <% if (Model.Registration != null) { %>
        <!-- Student is registered, show that information -->
        <% Html.RenderPartial("RegistrationDisplay", Model.Registration); %>
    <% } else { %>
        <!-- Student is not registered -->
        <% Html.RenderPartial("StudentInformationPartial", Model.Student); %>
        
        <h2>Registration</h2>
        Student has not registered.
    <% } %>
    

    <%--
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
    
    <% } %>--%>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
