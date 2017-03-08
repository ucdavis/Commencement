<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Commencement.MVC.Controllers.ViewModels.RegistrationModel>" MasterPageFile="~/Views/Shared/Site.Master" %>
<%@ Import Namespace="Commencement.MVC.Controllers" %>
<asp:Content runat="server" ID="Content" ContentPlaceHolderID="TitleContent">Edit registration</asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="HeaderContent">
    
    <script type="text/javascript">
        $(function () {

            $('input[type="radio"]').click(function () {
                $(this).parents("ul.registration_form").find('input[type="radio"]').removeAttr("checked");
                $(this).attr("checked", "checked");
            });

        });
    </script>

</asp:Content>

<asp:Content runat="server" ID="Content3" ContentPlaceHolderID="logoContent">
</asp:Content>

<asp:Content runat="server" ID="Content2" ContentPlaceHolderID="MainContent">

    <ul class="btn">
        <li><%: Html.ActionLink<StudentController>(a=>a.DisplayRegistration(), "Back") %></li>
    </ul>

    <p>
        Welcome back <%= Html.Encode(Model.Student.FirstName) %>.  
    </p>
    
    <%= Html.ValidationSummary("Please correct all errors below") %>
    
    <% using (Html.BeginForm()) { %>
        <%= Html.AntiForgeryToken() %>

        <% Html.RenderPartial("RegistrationEditForm"); %>    
    
        <fieldset>
        <ul class="registration_form">
            <li><strong>&nbsp;</strong>
                <input type="submit" value="Update Registration" class="button" />
                |
                <%: Html.ActionLink<StudentController>(a=>a.DisplayRegistration(), "Cancel") %>
            </li>
        </ul>
        
        </fieldset>
    <% } %>


</asp:Content>
