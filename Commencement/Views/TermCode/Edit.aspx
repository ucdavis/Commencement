<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Commencement.Core.Domain.TermCode>" %>
<%@ Import Namespace="Commencement.Controllers" %>
<%@ Import Namespace="Commencement.Controllers.Helpers" %>
<%@ Import Namespace="Commencement.Core.Domain" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Commencement | Edit Term Code
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="btn">
        <li>
            <%= Html.ActionLink<TermCodeController>(a => a.Index() , "Back to List") %>
        </li>
    </ul>
    
    <div class="page_bar">
        <div class="col1"><h2>Edit for <%= Html.Encode(Model.Id) %></h2></div>
        <div class="col2"><%= Html.ActionLink<TermCodeController>(a => a.Details(Model.Id) , "View Details", new{@class="button"}) %></div>
    </div>

    <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>
        <%= Html.AntiForgeryToken() %>
        <%= Html.HiddenFor(a => a.Id) %>
        
            <fieldset>
                <legend>Information</legend>
                <ul class="registration_form">
                    <li>
                        <strong><%: Html.LabelFor(a => a.Name, DisplayOptions.HumanizeAndColon) %></strong>
                        <%: Html.DisplayFor(model => model.Name) %>
                    </li>            
                    <li>
                        <strong><%:Html.LabelFor(a => a.IsActive, DisplayOptions.HumanizeAndColon) %></strong>
                        <%: Model.IsActive ? "Yes" : "No" %>
                    </li>
                </ul> 
            </fieldset>
            
            <fieldset>
                
                <legend>Deadlines</legend>
                <ul class="registration_form">
                    <li><strong>Registration:</strong>
                        <%: Html.TextBox("RegistrationBegin", Model.RegistrationBegin.ToString("d"), new {@class="date"}) %>
                        through
                        <%: Html.TextBox("RegistrationDeadline", Model.RegistrationDeadline.ToString("d"), new {@class="date"}) %>
                    </li>           
                    <li>
                        <strong>Cap and Gown Deadline:</strong>
                        <%: Html.TextBox("CapAndGownDeadline", Model.CapAndGownDeadline.ToString("d"), new {@class="date"}) %>
                        <%: Html.ValidationMessageFor(a=>a.CapAndGownDeadline) %>
                    </li>
                    <li>
                        <strong>File to Graduate Deadline:</strong>
                        <%: Html.TextBox("FileToGraduateDeadline", Model.FileToGraduateDeadline.ToString("d"), new {@class="date"}) %>
                        <%: Html.ValidationMessageFor(a=>a.FileToGraduateDeadline) %>
                    </li>    
                </ul>    

            </fieldset>

            <fieldset>
                
                <legend><%:Html.LabelFor(a => a.LandingText, DisplayOptions.Humanize) %></legend>
                
                <%: Html.ValidationMessageFor(model => model.LandingText) %>
                <%: Html.TextAreaFor(model => model.LandingText)%>

            </fieldset>
            
            <fieldset>
                
                <legend><%:Html.LabelFor(a => a.RegistrationWelcome, DisplayOptions.Humanize) %></legend>
                
                <%: Html.ValidationMessageFor(model => model.RegistrationWelcome) %>
                <%: Html.TextAreaFor(model => model.RegistrationWelcome) %>

            </fieldset>

            <fieldset>
                
                <ul class="registration_form">
                    <li><strong>&nbsp;</strong>
                        <input type="submit" value="Save" class="button" />    
                        |
                        <%= Html.ActionLink<TermCodeController>(a => a.Index() , "Cancel") %>
                    </li>
                </ul>

            </fieldset>

    <% } %>


</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    <script src="<%= Url.Content("~/Scripts/tiny_mce/jquery.tinymce.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.enableTinyMce.js") %>" type="text/javascript"></script>  
    <script type="text/javascript">
        $(document).ready(function () {
            $("#LandingText").enableTinyMce({ script_location: '<%= Url.Content("~/Scripts/tiny_mce/tiny_mce.js") %>', overrideWidth: '500', overrideHeight: '250' }); //, overrideShowPreview: 'preview,', overridePlugin_preview_pageurl: '<%= Url.Content("~/Static/Preview.html") %>' });
            $("#RegistrationWelcome").enableTinyMce({ script_location: '<%= Url.Content("~/Scripts/tiny_mce/tiny_mce.js") %>', overrideWidth: '500', overrideHeight: '250' }); //, overrideShowPreview: 'preview,', overridePlugin_preview_pageurl: '<%= Url.Content("~/Static/Preview.html") %>' });
        });
   </script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>

