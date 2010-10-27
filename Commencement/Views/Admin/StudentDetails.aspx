<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.RegistrationModel>" %>
<%@ Import Namespace="Commencement.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	StudentDetails
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="btn">
    <li>
        <% if (Request.QueryString["Registration"]!= null && Convert.ToBoolean(Request.QueryString["Registration"])) { %>
            <%= Html.ActionLink<AdminController>(a=>a.Registrations(null, null, null, null, null, null), "Back to List") %>
        <% } else { %>
            <%= Html.ActionLink<AdminController>(a=>a.Students(null, null, null, null), "Back to List") %>
        <% } %>
    </li>
    <li><%= Html.ActionLink<AccountController>(a=>a.Emulate(Model.Student.Login), "Emulate") %></li>
    
    <% if (Model.Registration != null) { %>
        <li><div id="changeMajr_btn"><%= Html.ActionLink<AdminController>(a=>a.ChangeMajor(Model.Registration.Id), "Change Major") %></div></li>
        <li><div id="changeCeremony_btn"><%= Html.ActionLink<AdminController>(a=>a.ChangeCeremony(Model.Registration.Id), "Change Ceremony") %></div></li>
    <% } %>
    
        <li><%= Html.ActionLink<AdminController>(a=>a.ToggleSJAStatus(Model.Student.Id), "Change SJA Status") %></li>
        <li><%= Html.ActionLink<AdminController>(a=>a.ToggleBlock(Model.Student.Id), "Block from Reg") %></li>
    </ul>

    <% if (Model.Student.SjaBlock) { %>
        <div style="background-color:red;">
            SJA has asked that we not allow this student to walk.
        </div>
    <% } %>

    <% if (Model.Registration != null) { %>
        <!-- Student is registered, show that information -->
        <% Html.RenderPartial("RegistrationDisplay", Model.Registration); %>
    <% } else { %>
        <!-- Student is not registered -->
        <% Html.RenderPartial("StudentInformationPartial", Model.Student); %>
        
        <h2>Registration</h2>
        Student has not registered.
    <% } %>
    

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
