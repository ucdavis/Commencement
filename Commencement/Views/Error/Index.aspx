<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ErrorViewModel>" %>
<%@ Import Namespace="Commencement.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%= Html.Encode(Model.Title) %></h2>

    <% if (Model.ErrorType == ErrorController.ErrorType.UnauthorizedAccess) { %>
    
        <p>
            You are unauthorized for your request.  If you believe this is an error please contact the system administrator for help.
        </p>
        
        <p>
            If you are a student please check to make sure that you have completed at least 140 units and are registered in a major
            within the College of Agricultural and Environmental Sciences.  If you do not meet the criteria or do meet the criteria and believe
            you should be able to register for commencement please fill out the form found <a href="#">here</a> and submit it to
            Francesca Ross (email here).
        </p>
    
    <% } else { %>
    
        <p>
            <%= Html.Encode(Model.Description) %>
        </p>
    
    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
