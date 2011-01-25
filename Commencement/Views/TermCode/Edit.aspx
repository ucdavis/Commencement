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

    <h2>Edit <%= Html.Encode(Model.Id) %></h2>

    <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>
        <%= Html.AntiForgeryToken() %>
        <%= Html.HiddenFor(a => a.Id) %>
        
            <fieldset>
                <legend> Information </legend>
                <ul class="registration_form">
                    <li>
                        <strong><%: Html.LabelFor(a => a.Name, DisplayOptions.HumanizeAndColon) %></strong>
                        <%: Html.DisplayFor(model => model.Name) %>
                    </li>            
                    <li>
                        <strong><%:Html.LabelFor(a => a.IsActive, DisplayOptions.HumanizeAndColon) %></strong>
                        <%: Html.DisplayFor(model => model.IsActive) %>
                    </li>
                </ul> 
            </fieldset>
            <ul class="registration_form">           
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
            <li>
                <strong><%:Html.LabelFor(a => a.LandingText, DisplayOptions.HumanizeAndColon) %></strong>
                <%: Html.TextAreaFor(model => model.LandingText)%>
                <%: Html.ValidationMessageFor(model => model.LandingText) %>
            </li>            
            <li>
                <strong><%:Html.LabelFor(a => a.RegistrationWelcome, DisplayOptions.HumanizeAndColon) %></strong>
                <%: Html.TextAreaFor(model => model.RegistrationWelcome) %>
                <%: Html.ValidationMessageFor(model => model.RegistrationWelcome) %>
            </li>
            
            <p>
                <input type="submit" value="Save" />
            </p>
        </ul>

    <% } %>


</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    <script src="<%= Url.Content("~/Scripts/tiny_mce/jquery.tinymce.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.enableTinyMce.js") %>" type="text/javascript"></script>  
    <script type="text/javascript">
        $(document).ready(function () {
            $("#LandingText").enableTinyMce({ script_location: '<%= Url.Content("~/Scripts/tiny_mce/tiny_mce.js") %>', overrideWidth: '500', overrideHeight: '250' }); //, overrideShowPreview: 'preview,', overridePlugin_preview_pageurl: '<%= Url.Content("~/Static/Preview.html") %>' });
            $("#RegistrationWelcome").enableTinyMce({ script_location: '<%= Url.Content("~/Scripts/tiny_mce/tiny_mce.js") %>', overrideWidth: '500', overrideHeight: '250' }); //, overrideShowPreview: 'preview,', overridePlugin_preview_pageurl: '<%= Url.Content("~/Static/Preview.html") %>' });

            $(".date").datepicker();
        });
   </script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="logoContent" runat="server">
</asp:Content>

