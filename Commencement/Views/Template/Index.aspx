<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.TemplateViewModel>" %>
<%@ Import Namespace="Commencement.Controllers"%>
<%@ Import Namespace="Commencement.Controllers.Helpers"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<ul class="btn">
    <li>
    <%= Html.ActionLink<AdminController>(a=>a.Index(), "Home") %>
    </li></ul>

    <h2>Templates</h2>

    <%= Html.ActionLink("Create", "Create", "Template") %>

    <div id="template_container">
    
        <% foreach(var t in Model.Templates) { %>
        
            <fieldset class="template">
                <legend>
                    <%= Html.ActionLink<TemplateController>(a=>a.Create(t.Id), t.TemplateType.Name) %>
                </legend>
                
                <div class="template_description">
                    <%= Html.Encode(t.TemplateType.Description) %>
                </div>
                
                <div class="template_body">
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
