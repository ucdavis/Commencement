<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.CeremonyViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create Commencement
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <a href="#" onClick="history.go(-1)">Back</a> 

    <h2>Create</h2>

    <%= Html.ValidationSummary("Please correct all errors below") %>
    <%= Html.ClientSideValidation<Commencement.Core.Domain.Ceremony>("Ceremony") %>

    <% using (Html.BeginForm("Create", "Ceremony", FormMethod.Post)) { %>

        <%= Html.AntiForgeryToken()%>
        
        <% Html.RenderPartial("CeremonyPartial"); %>

    <% } %>    

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

    

</asp:Content>
