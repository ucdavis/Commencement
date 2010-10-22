<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.TemplateViewModel>" %>
<%@ Import Namespace="Commencement.Controllers"%>
<%@ Import Namespace="Commencement.Controllers.Helpers"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Email Templates
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<ul class="btn">
    <li>
    <%= Html.ActionLink<CeremonyController>(a=>a.Edit(Model.Ceremony.Id), "Back to Ceremony") %>
    </li></ul>

    <h2>Email Templates for <%: Model.Ceremony.Name %></h2>

    <%= Html.ActionLink<TemplateController>(a=>a.Create(Model.Ceremony.Id, null), "Create  New Template") %>

    <div id="template_container">
    
        <% if (Model.Templates.Count > 0) { %>

        <% foreach(var t in Model.Templates) { %>
        
            <fieldset class="template">
                <legend>
                    <%= Html.ActionLink<TemplateController>(a=>a.Create(Model.Ceremony.Id, t.Id), t.TemplateType.Name) %>
                </legend>
                
                <div class="template_description">
                    <%= Html.Encode(t.TemplateType.Description) %>
                </div>
                
                <div class="template_subject">
                    <%: t.Subject %>
                </div>

                <div class="template_body">
                    <%= Html.HtmlEncode(t.BodyText) %>
                </div>
                
            </fieldset>
        
        <% } %>
    
        <% } else { %>

            No email templates have been created.

        <% } %>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
