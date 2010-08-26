<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ErrorViewModel>" %>
<%@ Import Namespace="Commencement.Controllers" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>
<%@ Import Namespace="Commencement.Core.Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    

    <% if (Model.ErrorType == ErrorController.ErrorType.UnauthorizedAccess) { %>
    
        <h2>
            Commencement Registration for the College of Agricultural & Environmental Sciences
        </h2>
        
        <%= string.Format(StaticValues.Txt_NotAuthorized, TermService.GetCurrent().Name) %>
    
        <br />
        <div style="text-align:center;">
        <div id="continue_btn">
            <%= Html.ActionLink<PetitionController>(a=>a.Register(), "Continue") %>
        </div>
        </div>
        
    
    <% } else { %>
        <h2><%= Html.Encode(Model.Title) %></h2>
    
        <p>
            <%= Html.Encode(Model.Description) %>
        </p>
    
    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
