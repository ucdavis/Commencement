<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Commencement.Core.Domain.Registration>" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content runat="server" ID="Content" ContentPlaceHolderID="TitleContent">Display Registration</asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="HeaderContent"></asp:Content>
<asp:Content runat="server" ID="Content2" ContentPlaceHolderID="MainContent">
    <h1><%= Model.Ceremony.Name %></h1>
    <p>
        Some quick intriduction, two or three lines explaining what this is, what they need
        to do, the <strong>due date</strong>, and what will happen after they do this. We
        need to include a line stating <strong>all fields are required</strong></p>
        
    <h2>
        Student Information</h2>
    <ul class="registration_form">
        <li class="prefilled"><strong>Name:</strong><span><%= Html.Encode(Model.Student.FullName) %></span>
        </li>
        <li class="prefilled"><strong>Student ID:</strong><span><%= Html.Encode(Model.Student.StudentId) %></span> </li>
        <li class="prefilled"><strong>Units Complted:</strong><span><%= Html.Encode(Model.Student.Units) %></span> </li>
        <li class="prefilled"><strong>Major:</strong><span><%= Html.Encode(Model.Major.Name)%></span></li>        
    </ul>
    <h2>
        Contact Information</h2>    
        
    <ul class="registration_form">
        <li><strong>Address Line 1:</strong><span><%= Html.Encode(Model.Address1) %></span>
        </li>
        <li><strong>Address Line 2:</strong><span><%= Html.Encode(Model.Address2) %></span>
        </li>
        <li><strong>City:</strong><span><%= Html.Encode(Model.City) %></span>
        </li>
        <li><strong>State:</strong><span><%= Html.Encode(Model.State) %></span>
        </li>
        <li><strong>Zip Code:</strong><span><%= Html.Encode(Model.Zip) %></span>
        </li>
        <li class="prefilled"><strong>Email Address:</strong> <span><%= Model.Student.Email %></span>
        </li>
        <% if (!string.IsNullOrEmpty(Model.Email)) { %>
        <li><strong>Secondary Email Address:</strong><span><%= Html.Encode(Model.Email)%></span>
        </li>
        <% } %>
    </ul>
    <h2>
        Ceremony Information</h2>
    <ul class="registration_form">
        <li><strong>Tickets Requested:</strong><span><%= Model.NumberTickets %></span>
        </li>
        <li class="prefilled"><strong>Ceremony Date:</strong> <span><%= Html.Encode(string.Format("{0}", Model.Ceremony.DateTime)) %></span> </li>
    </ul>
        
    <h3>
        Disclaimer about the page and the process. Disclaimer about the page and the process.
        Disclaimer about the page and the process. Disclaimer about the page and the process.
        Disclaimer about the page and the process. Disclaimer about the page and the process.
        Disclaimer about the page and the process. Disclaimer about the page and the process.
    </h3>
</asp:Content>

