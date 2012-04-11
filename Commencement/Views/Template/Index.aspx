<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Controllers.ViewModels.TemplateViewModel>" %>
<%@ Import Namespace="Commencement.Controllers"%>
<%@ Import Namespace="Commencement.Controllers.Helpers"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Commencement | Email Templates
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<ul class="btn">
    <li>
    <%= Html.ActionLink<CeremonyController>(a=>a.Edit(Model.Ceremony.Id), "Back to Ceremony") %>
    </li></ul>

    <h2>Email Templates for <%: Model.Ceremony.CeremonyName %></h2>

    <%= Html.ActionLink<TemplateController>(a=>a.Create(Model.Ceremony.Id, null), "Create  New Template") %>

    <div id="template_container">
    
        <% if (Model.Templates.Count > 0) { %>

        <% foreach(var t in Model.Templates) { %>
        
            <fieldset class="template">
                <legend>
                    <%= Html.ActionLink<TemplateController>(a=>a.Create(Model.Ceremony.Id, t.Id), t.TemplateType.Name) %>
                </legend>
                
                <div class="template_description" style="margin-bottom:20px;">
                    <%= Html.Encode(t.TemplateType.Description) %>
                </div>
                <div>Subject:</div>
                <div class="template_subject" style="background-color:#f4f4f4; border:1px solid #666; margin:10px 20px 10px 20px; padding:10px;">
                    <%: t.Subject %>
                </div>
                <div>Body:</div>
                <div class="template_body" style="background-color:#f4f4f4; border:1px solid #666; margin:10px 20px 10px 20px; padding:10px; line-height:1.5em;">
                    <%: Html.HtmlEncode(t.BodyText) %>
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
