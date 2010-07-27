<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.TemplateViewModel>" %>
<%@ Import Namespace="Commencement.Controllers"%>
<%@ Import Namespace="Commencement.Controllers.Helpers"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <%= Html.ActionLink<AdminController>(a=>a.Index(), "Home") %>

    <h2>Templates</h2>

    <%= Html.ActionLink("Create", "Create", "Template") %>

    <div id="template_container">
    
        <% foreach(var t in Model.Templates) { %>
        
            <fieldset>
                <legend>
                    <% if (t.RegistrationConfirmation) {%>
                        <%= Html.ActionLink<TemplateController>(a=>a.Create(t.Id), "Registration Confirmation") %>
                    <% } else if (t.RegistrationPetition) { %>
                        <%= Html.ActionLink<TemplateController>(a=>a.Create(t.Id), "Registration Petition") %>
                    <% } else if (t.ExtraTicketPetition) { %>
                        <%= Html.ActionLink<TemplateController>(a=>a.Create(t.Id), "Extra Ticket Petition") %>
                    <% } %>
                </legend>
                
                <div>
                    <%= Html.HtmlEncode(t.BodyText) %>
                </div>
                
            </fieldset>
        
        <% } %>
    
    </div>

<%--    <% Html.Grid(Model.Templates)
           .Transactional()
           .Name("Templates")
           .Render(); %>--%>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
